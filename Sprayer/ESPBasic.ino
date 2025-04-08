//#include <WiFi.h>
//#include <WiFiUdp.h>
//
//WiFiUDP udp;
//byte packetBuffer[255];
//unsigned int localPort = 9999;
//const char* ssid = "BlaBla";
//const char* password = "SuperBla";
//byte bix = 0;
//
//void setup() {
//    WiFi.mode(WIFI_AP);
//    WiFi.softAP(ssid, password);
//    delay(100); // some do, some don't ...
//    udp.begin(localPort);
//}
//
//void loop() {
//    udp.beginPacket("192.168.4.2", localPort); // IP client ESP
//    udp.write(packetBuffer, 255);
//    udp.endPacket();
//
//    packetBuffer[0] = bix++; // just an upcounting number for tests
//    delay(500);
//}

#include <WiFi.h>
#include <WiFiUdp.h>

WiFiUDP udp;
unsigned int localPort = 8888;
const char* ssid = "BlaBla";
const char* password = "Bla";

void setup() {
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, password);
    udp.begin(localPort);
}

void loop() {
    int packetSize = udp.parsePacket(); // always zero :-(((

    if (packetSize) {
        //...
    }
}
