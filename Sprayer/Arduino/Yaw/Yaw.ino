
// Motion Module Interface:
#include "EasyObjectDictionary.h"
#include "EasyProfile.h"
EasyObjectDictionary eOD;
EasyProfile          eP(&eOD);

float rollTM=0;

#include "BNO_RVC.h"
//Roomba Vac mode for BNO085 and data
uint8_t toSend = 0;
BNO_rvc rvc = BNO_rvc();
BNO_rvcData bnoData;

void setup() {
  Serial.begin(115200);     // For print()
  delay(1000);
  Serial1.begin(115200);  // For communication with the Motion Module
  delay(1000);

  HardwareSerial* SerialIMU = &Serial2;
  SerialIMU->begin(115200); // This is the baud rate specified by the BNO datasheet
  delay(1000);
  rvc.begin(SerialIMU);
}

void loop() 
{
  SerialRX(); //call the function
  
    if (rvc.read(&bnoData) )
    {
      toSend++;
      //if (toSend > 1)

      if (bnoData.yawX10 == 0) return;
      {
        toSend = 0;        
    
        //the roll x10
        Serial.print(bnoData.yawX10+40);
        Serial.print(",");
        Serial.println(rollTM*10);
      }
    }
}

void SerialRX() {
  while (Serial1.available()) {
    // Read the received byte:
    char rxByte = (char)Serial1.read();                     // Step 1: read the received data buffer of the Serial Port
    char*  rxData = &rxByte;                                   //         and then convert it to data types acceptable by the
    int    rxSize = 1;                                         //         Communication Abstraction Layer (CAL).
    Ep_Header header;
    if(EP_SUCC_ == eP.On_RecvPkg(rxData, rxSize, &header)){    // Step 2: Tell the CAL that new data has arrived.
                                                               //         It does not matter if the new data only contains a fraction
                                                               //         of a complete package, nor does it matter if the data is broken
                                                               //         during the transmission. On_RecvPkg() will only return EP_SUCC_
                                                               //         when a complete and correct package has arrived.

        // Example Reading of the Short ID of the device who send the data:
        uint32 fromId = header.fromId;                         // Step 3.1:  Now we are able to read the received payload data.
                                                               //            header.fromId tells us from which Motion Module the data comes.
        //Supress "parameter unused" complier warning:
        (void)fromId;

        switch (header.cmd) {                                  // Step 3.2: header.cmd tells what kind of data is inside the payload.
        case EP_CMD_ACK_:{                                     //           We can use a switch() as demonstrated here to do different
            Ep_Ack ep_Ack;                                     //           tasks for different types of data.
            if(EP_SUCC_ == eOD.Read_Ep_Ack(&ep_Ack)){

            }
        }break;
        case EP_CMD_STATUS_:{
            Ep_Status ep_Status;
            if(EP_SUCC_ == eOD.Read_Ep_Status(&ep_Status)){

            }
        }break;
        
        case EP_CMD_RPY_:{
            Ep_RPY ep_RPY;
            if(EP_SUCC_ == eOD.Read_Ep_RPY(&ep_RPY)){     //           Another Example reading of the received Roll Pitch and Yaw
                float roll  = ep_RPY.roll*10;
                float pitch = ep_RPY.pitch;
                float yaw   = ep_RPY.yaw;
                uint32 timeStamp = ep_RPY.timeStamp;      //           TimeStamp indicates the time point (since the Module has been powered on),
                                                          //           when this particular set of Roll-Pitch-Yaw was calculated. (Unit: uS)
                                                          //           Note that overflow will occure when the uint32 type reaches its maximum value.
                uint32 deviceId  = ep_RPY.header.fromId;  //           The ID indicates from wich IMU Module the data comes from.


                // Display the data:
                rollTM = yaw;
                //Serial.print("Roll:");  
                //Serial.println(roll); 
                //Serial.print(" Pitch:");Serial.print(pitch); 
                //Serial.print(" Yaw");   Serial.print(yaw); 
                //Serial.print("  TimeStamp:");      Serial.print(timeStamp);
                //Serial.print("  Device Short ID:");Serial.println(deviceId);
            }
        }break;

        }

    }

  }
}
