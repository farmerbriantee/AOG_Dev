/*
   UDP Autosteer code for Teensy 4.1
   For AgOpenGPS
   01 Feb 2022
   Like all Arduino code - copied from somewhere else :)
   So don't claim it as your own
*/

////////////////// User Settings /////////////////////////

//How many degrees before decreasing Max PWM
#define LOW_HIGH_DEGREES 3.0

/*  PWM Frequency ->
     490hz (default) = 0
     122hz = 1
     3921hz = 2
*/
#define PWM_Frequency 0

/////////////////////////////////////////////

// if not in eeprom, overwrite
#define EEP_Ident 2400

//   ***********  Motor drive connections  **************888
//Connect ground only for cytron, Connect Ground and +5v for IBT2

//Dir1 for Cytron Dir, Both L and R enable for IBT2
#define DIR1_RL_ENABLE  4

//PWM1 for Cytron PWM, Left PWM for IBT2
#define PWM1_LPWM  2

//Not Connected for Cytron, Right PWM for IBT2
#define PWM2_RPWM  3

//--------------------------- Switch Input Pins ------------------------
#define STEERSW_PIN 32
#define WORKSW_PIN 34
#define REMOTE_PIN 37

//Define sensor pin for current or pressure sensor
#define CURRENT_SENSOR_PIN A17
#define PRESSURE_SENSOR_PIN A10

#define CONST_180_DIVIDED_BY_PI 57.2957795130823

#include <Wire.h>
#include <EEPROM.h>
#include "zADS1115.h"
ADS1115_lite adc(ADS1115_DEFAULT_ADDRESS);     // Use this for the 16-bit version ADS1115

#include <IPAddress.h>

#ifdef ARDUINO_TEENSY41
// ethernet
#include <NativeEthernet.h>
#include <NativeEthernetUdp.h>
#endif

#ifdef ARDUINO_TEENSY41
//uint8_t Ethernet::buffer[200]; // udp send and receive buffer
uint8_t toolSteerUdpData[UDP_TX_PACKET_MAX_SIZE];  // Buffer For Receiving UDP Data
#endif

//loop time variables in microseconds
const uint16_t LOOP_TIME = 25;  //40Hz
uint32_t steerLoopLastTime = LOOP_TIME;
uint32_t currentTime = LOOP_TIME;

const uint16_t WATCHDOG_THRESHOLD = 100;
const uint16_t WATCHDOG_FORCE_VALUE = WATCHDOG_THRESHOLD + 2; // Should be greater than WATCHDOG_THRESHOLD
uint8_t watchdogTimer = WATCHDOG_FORCE_VALUE;

//Heart beat hello AgIO
uint8_t helloFromIMU[] = { 128, 129, 121, 121, 5, 0, 0, 0, 0, 0, 71 };
uint8_t helloFromAutoSteer[] = { 0x80, 0x81, 126, 126, 5, 0, 0, 0, 0, 0, 71 };
int16_t helloSteerPosition = 0;

//fromAutoSteerData FD 253 - ActualSteerAngle*100 -5,6, SwitchByte-7, pwmDisplay-8
uint8_t PGN_223[] = {0x80,0x81, 126, 223, 8, 0, 0, 0, 0, 0,0,0,0, 0xCC };
int8_t PGN_223_Size = sizeof(PGN_223) - 1;

//EEPROM
int16_t EEread = 0;

//Switches
uint8_t remoteSwitch = 0, workSwitch = 0, steerSwitch = 1, switchByte = 0;

//On Off
uint8_t guidanceStatus = 0;

//speed sent as *10
float gpsSpeed = 0;

//steering variables
float steerAngleActual = 0;

//from AgOpen
float toolXTE_AOG = 0; //tool XTE from AgOpen
float vehicleXTE_AOG = 0; //vehicle XTE from AgOpen
float vehicleHeading_AOG = 0; //vehicle heading from AgOpen

int16_t steeringPosition = 0; //from steering sensor
float toolCorrectionError = 0; //setpoint - actual

//pwm variables
int16_t pwmDrive = 0, pwmDisplay = 0;
float pValue = 0;
float errorAbs = 0;
float highLowPerDeg = 0;

////Variables for settings
//struct Storage {
//  uint8_t Kp = 40;              // proportional gain
//  uint8_t lowPWM = 10;          // band of no action
//  int16_t wasOffset = 0;
//  uint8_t minPWM = 9;
//  uint8_t highPWM = 60;         // max PWM value
//  float steerSensorCounts = 30;
//  float AckermanFix = 1;        // sent as percent
//};  Storage steerSettings;      // 11 bytes

//Variables for settings - 0 is false
struct Setup {
    //uint8_t InvertWAS = 0;
    uint8_t IsRelayActiveHigh = 0;    // if zero, active low (default)
    //uint8_t MotorDriveDirection = 0;
    uint8_t SingleInputWAS = 1;
    uint8_t CytronDriver = 1;
    uint8_t SteerSwitch = 0;          // 1 if switch selected
    uint8_t SteerButton = 1;          // 1 if button selected
    //uint8_t ShaftEncoder = 0;
    //uint8_t PressureSensor = 0;
    //uint8_t CurrentSensor = 0;
    //uint8_t PulseCountMax = 5;
    uint8_t IsDanfoss = 0;
    //uint8_t IsUseY_Axis = 0;     //Set to 0 to use X Axis, 1 to use Y avis
}; Setup steerConfig;               // 9 bytes

//Variables for settings - 0 is false
struct Tool_Config {
    uint8_t invertWAS = 0;
    uint8_t invertSteer = 0; // if zero, active low (default)
    uint8_t maxSteerAngle = 30;
    uint8_t isSteer = 1;
}; Tool_Config toolConfig;

//Variables for settings
struct Tool_Settings {
    uint8_t Kp = 40;              // proportional gain
    uint8_t Ki = 0;
    uint8_t minPWM = 9;
    uint8_t lowPWM = 15;          // band of no action
    uint8_t highPWM = 60;         // max PWM value
    float steerSensorCounts = 30;
    int16_t wasOffset = 0;
    float AckermanFix = 1;        // sent as percent
};  Tool_Settings toolSettings;      // 11 bytes

// 9 bytes

void steerConfigInit()
{
  //if (steerConfig.CytronDriver) 
  {
    pinMode(PWM2_RPWM, OUTPUT);
  }
}

void toolSettingsInit()
{
  // for PWM High to Low interpolator
  highLowPerDeg = ((float)(toolSettings.highPWM - toolSettings.minPWM)) / LOW_HIGH_DEGREES;
}

void ToolsteerSetup()
{
    //PWM rate settings. Set them both the same!!!!
    /*  PWM Frequency ->
         490hz (default) = 0
         122hz = 1
         3921hz = 2
    */
    if (PWM_Frequency == 0)
    {
        analogWriteFrequency(PWM1_LPWM, 490);
        analogWriteFrequency(PWM2_RPWM, 490);
    }
    else if (PWM_Frequency == 1)
    {
        analogWriteFrequency(PWM1_LPWM, 122);
        analogWriteFrequency(PWM2_RPWM, 122);
    }
    else if (PWM_Frequency == 2)
    {
        analogWriteFrequency(PWM1_LPWM, 3921);
        analogWriteFrequency(PWM2_RPWM, 3921);
    }

    //keep pulled high and drag low to activate, noise free safe
    pinMode(WORKSW_PIN, INPUT_PULLUP);
    pinMode(STEERSW_PIN, INPUT_PULLUP);
    pinMode(REMOTE_PIN, INPUT_PULLUP);
    pinMode(DIR1_RL_ENABLE, OUTPUT);

    // Disable digital inputs for analog input pins
    pinMode(CURRENT_SENSOR_PIN, INPUT_DISABLE);
    pinMode(PRESSURE_SENSOR_PIN, INPUT_DISABLE);

    //set up communication
    Wire1.end();
    Wire1.begin();

    // Check ADC 
    if (adc.testConnection())
    {
        Serial.println("ADC Connecton OK");
    }
    else
    {
        Serial.println("ADC Connecton FAILED!");
    }

    //50Khz I2C
    //TWBR = 144;   //Is this needed?

    EEPROM.get(0, EEread);              // read identifier

    if (EEread != EEP_Ident)            // check on first start and write EEPROM
    {
        EEPROM.put(0, EEP_Ident);
        EEPROM.put(10, toolSettings);
        EEPROM.put(40, toolConfig);
        EEPROM.put(60, networkAddress);
    }
    else
    {
        EEPROM.get(10, toolSettings);     // read the Settings
        EEPROM.get(40, toolConfig);
        EEPROM.get(60, networkAddress);
    }

    toolSettingsInit();
    steerConfigInit();

    Serial.println("Autosteer running, waiting for AgOpenGPS");
    // Autosteer Led goes Red if ADS1115 is found
    digitalWrite(AUTOSTEER_ACTIVE_LED, 0);
    digitalWrite(AUTOSTEER_STANDBY_LED, 1);

    adc.setSampleRate(ADS1115_REG_CONFIG_DR_128SPS); //128 samples per second
    adc.setGain(ADS1115_REG_CONFIG_PGA_6_144V);

}// End of Setup

void toolsteerLoop()
{
    // Loop triggers every 100 msec and sends back gyro heading, and roll, steer angle etc
    currentTime = systick_millis_count;

    if (currentTime - steerLoopLastTime >= LOOP_TIME)
    {
        steerLoopLastTime = currentTime;

        //If connection lost to AgOpenGPS, the watchdog will count up and turn off steering
        if (watchdogTimer++ > 250) watchdogTimer = WATCHDOG_FORCE_VALUE;

        //read all the switches
        workSwitch = digitalRead(WORKSW_PIN);  // read work switch

        steerSwitch = digitalRead(STEERSW_PIN); //read auto steer enable switch open = 0n closed = Off

        remoteSwitch = digitalRead(REMOTE_PIN); //read auto steer enable switch open = 0n closed = Off

        switchByte = 0;
        switchByte |= (remoteSwitch << 2); //put remote in bit 2
        switchByte |= (steerSwitch << 1);   //put steerswitch status in bit 1 position
        switchByte |= workSwitch;

        //get steering position
        if (steerConfig.SingleInputWAS)   //Single Input ADS
        {
            adc.setMux(ADS1115_REG_CONFIG_MUX_SINGLE_0);
            steeringPosition = adc.getConversion();
            adc.triggerConversion();//ADS1115 Single Mode

            steeringPosition = (steeringPosition >> 1); //bit shift by 2  0 to 13610 is 0 to 5v
            helloSteerPosition = steeringPosition - 6800;
        }
        else    //ADS1115 Differential Mode
        {
            adc.setMux(ADS1115_REG_CONFIG_MUX_DIFF_0_1);
            steeringPosition = adc.getConversion();
            adc.triggerConversion();

            steeringPosition = (steeringPosition >> 1); //bit shift by 2  0 to 13610 is 0 to 5v
            helloSteerPosition = steeringPosition - 6800;
        }

        //DETERMINE ACTUAL STEERING POSITION

        //convert position to steer angle. 32 counts per degree of steer pot position in my case
        //  ***** make sure that negative steer angle makes a left turn and positive value is a right turn *****
        if (toolConfig.invertWAS)
        {
            steeringPosition = (steeringPosition - 6805 - toolSettings.wasOffset);   // 1/2 of full scale
            steerAngleActual = (float)(steeringPosition) / -toolSettings.steerSensorCounts;
        }
        else
        {
            steeringPosition = (steeringPosition - 6805 + toolSettings.wasOffset);   // 1/2 of full scale
            steerAngleActual = (float)(steeringPosition) / toolSettings.steerSensorCounts;
        }

        //Ackerman fix
        if (steerAngleActual < 0) steerAngleActual = (steerAngleActual * toolSettings.AckermanFix);

        if (watchdogTimer < WATCHDOG_THRESHOLD && guidanceStatus == 1)
        {
            //Enable H Bridge for IBT2, hyd aux, etc for cytron
            if (steerConfig.CytronDriver)
            {
                if (steerConfig.IsRelayActiveHigh)
                {
                    digitalWrite(PWM2_RPWM, 0);
                }
                else
                {
                    digitalWrite(PWM2_RPWM, 1);
                }
            }
            else digitalWrite(DIR1_RL_ENABLE, 1);

            toolCorrectionError = ((float)(toolXTE_AOG) * 0.1);   //calculate the error

            //if (abs(steerAngleError)< steerSettings.lowPWM) steerAngleError = 0;

            calcSteeringPID();  //do the pid
            motorDrive();       //out to motors the pwm value
            // Autosteer Led goes GREEN if autosteering

            digitalWrite(AUTOSTEER_ACTIVE_LED, 1);
            digitalWrite(AUTOSTEER_STANDBY_LED, 0);
        }
        else
        {
            //we've lost the comm to AgOpenGPS, or just stop request
            //Disable H Bridge for IBT2, hyd aux, etc for cytron
            if (steerConfig.CytronDriver)
            {
                if (steerConfig.IsRelayActiveHigh)
                {
                    digitalWrite(PWM2_RPWM, 1);
                }
                else
                {
                    digitalWrite(PWM2_RPWM, 0);
                }
            }
            else digitalWrite(DIR1_RL_ENABLE, 0); //IBT2

            pwmDrive = 0; //stop it
            motorDrive(); //out to motors the pwm value

            // Autosteer Led goes back to RED when autosteering is stopped
            digitalWrite(AUTOSTEER_STANDBY_LED, 1);
            digitalWrite(AUTOSTEER_ACTIVE_LED, 0);
        }
    } //end of timed loop

} // end of main loop

int currentRoll = 0;
int rollLeft = 0;
int steerLeft = 0;

// UDP Receive
void ReceiveUdp()
{
    // When ethernet is not running, return directly. parsePacket() will block when we don't
    if (!Ethernet_running)
    {
        return;
    }

    uint16_t len = Eth_udpToolSteer.parsePacket();

    // if (len > 0)
    // {
    //  Serial.print("ReceiveUdp: ");
    //  Serial.println(len);
    // }

    // Check for len > 4, because we check byte 0, 1, 3 and 3
    if (len > 4)
    {
        Eth_udpToolSteer.read(toolSteerUdpData, UDP_TX_PACKET_MAX_SIZE);

        if (toolSteerUdpData[0] == 0x80 && toolSteerUdpData[1] == 0x81 && toolSteerUdpData[2] == 0x7F) //Data
        {
            if (toolSteerUdpData[3] == 233)  //tool steer data
            {
                // xteLo = 5;
                // xteHi = 6;
                // status = 7;
                // xteVehLo = 8;
                // xteVehHi = 9;
                // speed10 = 10;
                // headLo = 11;
                // headHi = 12;

                //Bit 5,6   Tool XTE from AOG * 100 is sent
                toolXTE_AOG = ((float)(toolSteerUdpData[5] | ((int8_t)toolSteerUdpData[6]) << 8)) * 0.01; //low high bytes
                
                guidanceStatus = toolSteerUdpData[7];

                //Bit 8,9   Tool XTE from AOG * 100 is sent
                vehicleXTE_AOG = ((float)(toolSteerUdpData[8] | ((int8_t)toolSteerUdpData[9]) << 8)) * 0.01; //low high bytes

                gpsSpeed = ((float)(toolSteerUdpData[10])) * 0.1;

                //Bit 11,12   Tool XTE from AOG * 100 is sent
                vehicleHeading_AOG = ((float)(toolSteerUdpData[11] | ((int8_t)toolSteerUdpData[12]) << 8)) * 0.01; //low high bytes

                //Serial.print("steerAngleSetPoint: ");//Serial.println(steerAngleSetPoint); //Serial.println(gpsSpeed);

                //if ((bitRead(guidanceStatus, 0) == 0) || (gpsSpeed < 0.1) || (steerSwitch == 1))
                if ( (bitRead(guidanceStatus, 0) == 0) || (gpsSpeed < 0.1) )
                    {
                    watchdogTimer = WATCHDOG_FORCE_VALUE; //turn off steering motor
                }
                else          //valid conditions to turn on autosteer
                {
                    watchdogTimer = 0;  //reset watchdog
                }

                //----------------------------------------------------------------------------
                //Serial Send to agopenGPS

                int16_t sa = (int16_t)(steerAngleActual * 100);

                PGN_223[5] = (uint8_t)sa;
                PGN_223[6] = sa >> 8;

                // heading
                PGN_223[7] = (uint8_t)9999;
                PGN_223[8] = 9999 >> 8;

                // roll
                PGN_223[9] = (uint8_t)8888;
                PGN_223[10] = 8888 >> 8;

                PGN_223[11] = switchByte;
                PGN_223[12] = (uint8_t)pwmDisplay;

                //checksum
                int16_t CK_A = 0;
                for (uint8_t i = 2; i < PGN_223_Size; i++)
                    CK_A = (CK_A + PGN_223[i]);

                PGN_223[PGN_223_Size] = CK_A;

                //off to AOG
                SendUdp(PGN_223, sizeof(PGN_223), Eth_ipDestination, portDestination);

                //Serial.println(steerAngleActual);
                //--------------------------------------------------------------------------
            }

            //steer settings
            else if (toolSteerUdpData[3] == 232) 
            {
                // gainP = 5;
                // integral = 6;
                // minPWM = 7;
                // highPWM = 8;
                // countsPerDegree = 9;
                // wasOffsetLo = 10;
                // wasOffsetHi = 11;
                // ackerman = 12;

                //PID values
                toolSettings.Kp = ((float)toolSteerUdpData[5]);   // read Kp from AgOpenGPS

                toolSettings.Ki = toolSteerUdpData[6]; // read high pwm

                toolSettings.minPWM = toolSteerUdpData[7]; //read the minimum amount of PWM for instant on

                float temp = (float)toolSettings.minPWM * 1.2;
                toolSettings.lowPWM = (byte)temp;

                toolSettings.highPWM = toolSteerUdpData[8]; // read high pwm

                toolSettings.steerSensorCounts = toolSteerUdpData[9]; //sent as setting displayed in AOG

                toolSettings.wasOffset = (toolSteerUdpData[10]);  //read was zero offset Lo
                toolSettings.wasOffset |= (toolSteerUdpData[11] << 8);  //read was zero offset Hi

                toolSettings.AckermanFix = (float)toolSteerUdpData[12] * 0.01;

                //crc
                //autoSteerUdpData[13];

                //store in EEPROM
                EEPROM.put(10, toolSettings);

                // Re-Init steer settings
                toolSettingsInit();
            }

            else if (toolSteerUdpData[3] == 231)  //Tool Steer Config
            {
                // invertWAS = 5;
                // invertSteer = 6;
                // maxSteerAngle = 7;
                // isSteer = 8;

                toolConfig.invertWAS = toolSteerUdpData[5]; 

                toolConfig.invertSteer = toolSteerUdpData[6];

                toolConfig.maxSteerAngle = toolSteerUdpData[7]; 

                //for steering or sliding, not sure if it is needes
                toolConfig.isSteer = toolSteerUdpData[8]; 
                
                EEPROM.put(40, steerConfig);

                // Re-Init
                steerConfigInit();

            }//end FB
            else if (toolSteerUdpData[3] == 200) // Hello from AgIO
            {
                int16_t sa = (int16_t)(steerAngleActual * 100);

                helloFromAutoSteer[5] = (uint8_t)sa;
                helloFromAutoSteer[6] = sa >> 8;

                helloFromAutoSteer[7] = (uint8_t)helloSteerPosition;
                helloFromAutoSteer[8] = helloSteerPosition >> 8;
                helloFromAutoSteer[9] = switchByte;

                SendUdp(helloFromAutoSteer, sizeof(helloFromAutoSteer), Eth_ipDestination, portDestination);
            }

            else if (toolSteerUdpData[3] == 201)
            {
                //make really sure this is the subnet pgn
                if (toolSteerUdpData[4] == 5 && toolSteerUdpData[5] == 201 && toolSteerUdpData[6] == 201)
                {
                    networkAddress.ipOne = toolSteerUdpData[7];
                    networkAddress.ipTwo = toolSteerUdpData[8];
                    networkAddress.ipThree = toolSteerUdpData[9];

                    //save in EEPROM and restart
                    EEPROM.put(60, networkAddress);
                    SCB_AIRCR = 0x05FA0004; //Teensy Reset
                }
            }//end 201

            //whoami
            else if (toolSteerUdpData[3] == 202)
            {
                //make really sure this is the reply pgn
                if (toolSteerUdpData[4] == 3 && toolSteerUdpData[5] == 202 && toolSteerUdpData[6] == 202)
                {
                    IPAddress rem_ip = Eth_udpToolSteer.remoteIP();

                    //hello from AgIO
                    uint8_t scanReply[] = { 128, 129, Eth_myip[3], 203, 7,
                        Eth_myip[0], Eth_myip[1], Eth_myip[2], Eth_myip[3],
                        rem_ip[0],rem_ip[1],rem_ip[2], 23 };

                    //checksum
                    int16_t CK_A = 0;
                    for (uint8_t i = 2; i < sizeof(scanReply) - 1; i++)
                    {
                        CK_A = (CK_A + scanReply[i]);
                    }
                    scanReply[sizeof(scanReply) - 1] = CK_A;

                    static uint8_t ipDest[] = { 255,255,255,255 };
                    //uint16_t portDest = 19999; //AOG port that listens

                    //off to AOG
                    SendUdp(scanReply, sizeof(scanReply), ipDest, portDestination);
                }
            }
        } //end if 80 81 7F
    }
}

void SendUdp(uint8_t *data, uint8_t datalen, IPAddress dip, uint16_t dport)
{
  Eth_udpToolSteer.beginPacket(dip, dport);
  Eth_udpToolSteer.write(data, datalen);
  Eth_udpToolSteer.endPacket();
}
