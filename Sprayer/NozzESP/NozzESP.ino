 //Spray module

#include <EEPROM.h> 

//all the program variables
#include "zVar.h"

#include <Wire.h>

//MCP23017 A0 to A2 tied low
#define MCP1 0x20
  
// if not in eeprom, overwrite 
#define EEP_Ident 5109
#define EEPROM_SIZE 512
//PWM for flow motor control

#define Motor1 4
#define Motor2 16

#define FLOWPIN 32    //This is the input pin on the Arduino for flow meter

byte ErrorCount;

#include <WiFi.h>
#include <NetworkUdp.h>

// WiFi network name and password:
const char *networkName = "JohnDeere";
//const char *networkPswd = "bookworm";

//IP address to send UDP data to:
// either use the ip address of the server or
// a network broadcast address
const char *udpAddress = "192.168.5.255";
const int udpPort = 8888;

#define maxReadBuffer 100	// bytes
  uint8_t udpData[maxReadBuffer];
//Are we currently connected?
bool connected = false;

//The udp library class
NetworkUDP udp;

void setup() {

    //set the baud rate
    Serial.begin(38400);
    EEPROM.begin(EEPROM_SIZE);

    pinMode(FLOWPIN, INPUT_PULLUP);
    pinMode(Motor1, OUTPUT);
    pinMode(Motor2, OUTPUT);

    attachInterrupt(32, isr_Flow, HIGH);  //Configures interrupt 0 (pin 2 on the Arduino Uno) to run the isr

    EEPROM.get(0, EEread);              // read identifier

    if (EEread != EEP_Ident)   // check on first start and write EEPROM
    {
        EEPROM.put(0, EEP_Ident);
        EEPROM.put(4, settings);
        EEPROM.commit();
    }
    else
    {
        EEPROM.get(4, settings);     // read the Settings
    }

    // flow pwm
    ledcAttachChannel(Motor1, 500, 8, 0);
    ledcAttachChannel(Motor2, 500, 8, 1);

    // I2C
    Wire.begin();     // I2C on pins SCL 22, SDA 21
    Wire.setClock(400000);  //Increase I2C data rate to 400kHz

    // MCP23017 I/O expander on default address
    Serial.println("");
    Serial.println("Starting MCP23017 ...");

    ErrorCount = 0;

    bool MCP23017_found = false;
    while (!MCP23017_found)
    {
        Serial.print(".");
        Wire.beginTransmission(MCP1);
        MCP23017_found = (Wire.endTransmission() == 0);
        ErrorCount++;
        delay(500);
        if (ErrorCount > 5) break;
    }

    Serial.println("");
    if (MCP23017_found)
    {
        Wire.beginTransmission(MCP1);
        Wire.write(0x00); // IODIRA register
        Wire.write(0x00); // set all of port A to outputs
        Wire.endTransmission();

        Wire.beginTransmission(MCP1);
        Wire.write(0x01); // IODIRB register
        Wire.write(0x00); // set all of port B to outputs
        Wire.endTransmission();

        Serial.println("MCP23017 found.");
    }
    else
    {
        Serial.println("MCP23017 not found.");
    }

    //Connect to the WiFi network
    connectToWiFi(networkName);
}

void loop()
{
  //Loop triggers every 200 msec
    currentTime = millis();

    //200 msec
    if (currentTime - lastTime >= LOOP_TIME)
    {
        lastTime = currentTime;

        //If connection lost to AgOpenGPS, the watchdog will count up 
        if (watchdogTimer++ > 250) watchdogTimer = 30;

        //read pressure and be above the min
        pressureActual = (float)analogRead(A0);
        pressureActual /= 10;

        //////////////    Fix me  ***********
        //pressureActual = 20;          
        isPressureLow = pressureActual < settings.minPressurePSI;
        isPressureLow = false;

        if (isr_isFlowing)
        {
            //1 million divided by flow time in microseconds times 60 times 100 all divided by flowmeter Cal number
            actualGPM = ((1000000.0 / (float)isr_flowTime) * 6000) / settings.flowCalFactor;
        }
        else
        {
            actualGPM = 0;
        }

        //turn sections on/off          
        DoRelays();

        //if (isPressureLow || watchdogTimer > 29)
        if (watchdogTimer > 29 && setGPM > 50)
        {
            //make sure flow control doesn't move
            relayLo = 0;
            relayHi = 0;

            flowError = 0;
            pwmDrive = 0;
            integral = 0;
            DoManualPID();
        }
        else
        {
            //Volume x 10
            totalVolume = (isr_flowCount * 10) / settings.flowCalFactor;

            //Two modes, Manual and Auto
            if (autoMode)
            {
                if (relayLo == 0)
                {
                    flowError = 0;
                    pwmDrive = 0;
                    integral = 0;
                    DoManualPID();
                }
                else
                {
                    DoPID();
                }
            }
            else //is manual control mode
            {
                if (manualCounter > 0)
                {
                    manualCounter--;
                }
                else
                {
                    pwmDrive = 0;
                }
                flowError = (setGPM - actualGPM); //for dsplay only   
                DoManualPID();
            }
        }

        flipFlop++;

        if (flipFlop > 1)
        {
            flipFlop = 0;

            AOG[5] = (byte)(totalVolume);
            AOG[6] = (byte)(totalVolume >> 8);

            uint16_t aGPM = actualGPM;
            AOG[7] = (uint8_t)aGPM;
            AOG[8] = (uint8_t)(aGPM >> 8);

            AOG[9] = (int8_t)(settings.deadbandError * 100);
            AOG[10] = (int8_t)(isr_isFlowing);

            //1 is positive pwm drive, 0 is negative
            uint8_t syne = 1;
            if (pwmDrive < 0) syne = 0;
            AOG[11] = (int8_t)(abs(pwmDrive));
            AOG[12] = syne;

			float bob = (float)(isr_flowTime) / 1000.0;
			bob = 1 / bob;
            bob = bob * 1000;

            AOG[13] = (byte)(bob);
            AOG[14] = (byte)(((uint16_t)bob >> 8));

			//Serial.println(isr_flowTime);

            //add the checksum
            int16_t CK_A = 0;
            for (uint8_t i = 2; i < sizeof(AOG) - 1; i++)
            {
                CK_A = (CK_A + AOG[i]);
            }
            AOG[sizeof(AOG) - 1] = CK_A;


            if (connected)
            {
                //Send a packet
                udp.beginPacket(udpAddress, 9999);
                udp.write(AOG, sizeof(AOG));
                udp.endPacket();
            }
        }

        //Serial.write(AOG, sizeof(AOG));
        //Serial.flush();   // flush out buffer

        if (isr_isFlowing == 1) isr_isFlowing = 0;

    }//end of timed loop

    // UDP from AgIO
    UDP_Receive();
}

void displayIPaddress(const IPAddress address, unsigned int port) {
    Serial.print(" IP ");
    for (int i = 0; i < 4; i++) {
        Serial.print(address[i], DEC);
        if (i < 3) Serial.print(".");
    }
    Serial.print(" port ");
    Serial.println(port);
}

void DoRelays()
{
    uint8_t mcpOutA = 0; // Output for port A
    uint8_t mcpOutB = 0; // Output for port B


    // Calculate output for port A
    mcpOutB = (bitRead(relayLo, 0) ? 1 : 0) |
        (bitRead(relayLo, 1) ? 4 : 0) |
        (bitRead(relayLo, 2) ? 16 : 0) |
        (bitRead(relayLo, 3) ? 64 : 0);

    // Send both outputs in a single transmission
    Wire.beginTransmission(MCP1);
    Wire.write(0x12); // address port A of MCP
    Wire.write(mcpOutA); // value for port A
    Wire.write(mcpOutB); // value for port B
    Wire.endTransmission();
}  


void connectToWiFi(const char* ssid) {
    Serial.println("Connecting to WiFi network: " + String(ssid));

    // delete old config
    WiFi.disconnect(true);
    //register event handler
    WiFi.onEvent(WiFiEvent, WiFiEvent_t::ARDUINO_EVENT_WIFI_STA_GOT_IP);  // Will call WiFiEvent() from another thread.

    //Initiate connection
    WiFi.begin(ssid);

    Serial.println("Waiting for WIFI connection...");
}

// WARNING: WiFiEvent is called from a separate FreeRTOS task (thread)!
void WiFiEvent(WiFiEvent_t event, WiFiEventInfo_t info) {
    switch (event)
    {

    case ARDUINO_EVENT_WIFI_STA_GOT_IP:
        //When connected set
        Serial.print("WiFi connected! IP address: ");
        Serial.println(WiFi.localIP());
        Serial.println(WiFi.RSSI());

        //initializes the UDP state
        udp.begin(WiFi.localIP(), udpPort);
        connected = true;
        break;

    case ARDUINO_EVENT_WIFI_STA_DISCONNECTED:
        Serial.println("WiFi lost connection");
        Serial.println(info.wifi_sta_disconnected.reason);

        //Try to reconnect
        WiFi.disconnect(true);
        WiFi.begin(networkName);
        Serial.print("WiFi RECONNECT! IP address: ");
        Serial.println(WiFi.localIP());

        break;
    default: break;
    }
}
