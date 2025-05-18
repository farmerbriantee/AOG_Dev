// Motion Module Interface:
#include "EasyObjectDictionary.h"
#include "EasyProfile.h"

EasyObjectDictionary eOD;
EasyProfile          eP(&eOD);


// PGN - 211
  uint8_t data[] = {0x80,0x81,0x7D, 211, 6, 0,0,0,0, 0,0, 15};
  int16_t dataSize = sizeof(data);

  uint8_t toSend = 0;

  int16_t roll = 0, yaw = 0;

  float angVel = 0.0;
  int16_t sendAV;
  float lastYaw;
    
  void setup()
  {
      Serial.begin(115200);     // For print()
      delay(500);
      Serial1.begin(115200);  // For communication with the Motion Module
      delay(500);
  }
  
  void loop()
  {
      if (toSend >= 9)
      {
          toSend = 0;
          sendAV = (int16_t)(angVel);
          data[5] = (uint8_t)yaw;
          data[6] = yaw >> 8;

          //the roll x10
          data[7] = (uint8_t)roll;
          data[8] = roll >> 8;

          //the roll x10
          data[9] = (uint8_t)sendAV;
          data[10] = sendAV >> 8;

          angVel = 0;

          int16_t CK_A = 0;

          for (int16_t i = 2; i < dataSize - 1; i++)
          {
              CK_A = (CK_A + data[i]);
          }

          data[dataSize - 1] = CK_A;

          Serial.write(data, dataSize);
          //Serial.print(roll);
          //Serial.print(",");
          //Serial.print(yaw);
          //Serial.print(",");
          //Serial.println(sendAV);
          Serial.flush();
      }

      SerialRX();
  }

  void SerialRX() 
  {
      while (Serial1.available()) 
      {
          // Read the received byte:
          char rxByte = (char)Serial1.read();                     // Step 1: read the received data buffer of the Serial Port
          char* rxData = &rxByte;                                   //         and then convert it to data types acceptable by the
          int    rxSize = 1;                                         //         Communication Abstraction Layer (CAL).
          
          Ep_Header header;
          
          if (EP_SUCC_ == eP.On_RecvPkg(rxData, rxSize, &header)) {    // Step 2: Tell the CAL that new data has arrived.

              switch (header.cmd)
              {                                  // Step 3.2: header.cmd tells what kind of data is inside the payload.

              case EP_CMD_RPY_: //reading of the received Roll Pitch and Yaw

                  Ep_RPY ep_RPY;

                  if (EP_SUCC_ == eOD.Read_Ep_RPY(&ep_RPY))
                  {
                      roll = ep_RPY.roll * 100;
                      yaw = ep_RPY.yaw * 10;
                      toSend++;

                      angVel += ((ep_RPY.yaw*100) - lastYaw*100);
                      lastYaw = ep_RPY.yaw;					  
                  }
                  break;
              }
          }
      }
  }
