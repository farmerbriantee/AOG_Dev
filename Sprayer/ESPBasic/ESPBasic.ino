#include <WiFi.h>
#include <WifiUDP.h>

const char* ssid     = "SRX160AP";
const char* password = "1122334455";

IPAddress local_ip(192,168,0,1);
IPAddress gateway(192,168,0,1);
IPAddress subnet(255,255,255,0);

const char* udpAddress = "192.168.0.255";
const int udpPort = 8888;

//The udp library class
WiFiUDP udp;

void setup()
{
    Serial.begin(115200);
    Serial.println("\n[*] Creating AP");

    WiFi.mode(WIFI_AP);
    WiFi.softAPConfig(local_ip, gateway, subnet);
    WiFi.softAP(ssid, NULL);

    Serial.print("[+] AP Created with IP Gateway ");
    Serial.println(WiFi.softAPIP());

    udp.begin(WiFi.localIP(), udpPort);
}

void loop()
{
    udp.beginPacket(udpAddress, 9999);
    udp.printf("Seconds since boot: %lu", millis() / 1000);
    udp.endPacket();

    delay(1000);
}



/*
 *  This sketch sends random data over UDP on a ESP32 device
 *
 
#include <WiFi.h>
#include <NetworkUdp.h>

// WiFi network name and password:
const char *networkName = "Skynet";
const char *networkPswd = "bookworm";

//IP address to send UDP data to:
// either use the ip address of the server or
// a network broadcast address
const char *udpAddress = "192.168.1.255";
const int udpPort = 8888;

//Are we currently connected?
boolean connected = false;

//The udp library class
NetworkUDP udp;

void setup() {
  // Initialize hardware serial:
  Serial.begin(115200);

  //Connect to the WiFi network
  connectToWiFi(networkName, networkPswd);
}

void loop() {
  //only send data when connected
  if (connected) {
    //Send a packet
    udp.beginPacket(udpAddress, 9999);
    udp.printf("Seconds since boot: %lu", millis() / 1000);
    udp.endPacket();

    displayIPaddress(udpAddress, udpPort);
  }
  //Wait for 1 second
  delay(1000);

  uint16_t len = udp.parsePacket();
  if (len > 6)
  {
      displayIPaddress(udp.remoteIP(), udp.remotePort());
      byte PGNlength;
      byte Data[255];
      udp.read(Data, 255);
      uint16_t PGN = Data[1] << 8 | Data[0];
  }
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


void connectToWiFi(const char *ssid, const char *pwd) {
  Serial.println("Connecting to WiFi network: " + String(ssid));

  // delete old config
  WiFi.disconnect(true);
  //register event handler
  WiFi.onEvent(WiFiEvent);  // Will call WiFiEvent() from another thread.

  //Initiate connection
  WiFi.begin(ssid, pwd);

  Serial.println("Waiting for WIFI connection...");
}

// WARNING: WiFiEvent is called from a separate FreeRTOS task (thread)!
void WiFiEvent(WiFiEvent_t event) {
  switch (event) {
    case ARDUINO_EVENT_WIFI_STA_GOT_IP:
      //When connected set
      Serial.print("WiFi connected! IP address: ");
      Serial.println(WiFi.localIP());
      //initializes the UDP state
      //This initializes the transfer buffer
      udp.begin(WiFi.localIP(), udpPort);
      connected = true;
      break;
    case ARDUINO_EVENT_WIFI_STA_DISCONNECTED:
      Serial.println("WiFi lost connection");
      connected = false;
      break;
    default: break;
  }
}
*/
