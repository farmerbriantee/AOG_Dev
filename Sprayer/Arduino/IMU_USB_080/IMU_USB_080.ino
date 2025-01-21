#include "BNO_RVC.h"

// PGN - 211
  uint8_t data[] = {0x80,0x81,0x7D,0xD3,8, 0,0,0,0, 0,0,0,0, 15};
  int16_t dataSize = sizeof(data);

  uint8_t toSend = 0;

  //Roomba Vac mode for BNO085 and data
  BNO_rvc rvc = BNO_rvc();
  BNO_rvcData bnoData;

  HardwareSerial* SerialIMU = &Serial1;
    
  void setup()
  {
    Serial.begin(38400);  // Start serial port

    delay(10);
    SerialIMU->begin(115200); // This is the baud rate specified by the BNO datasheet

    rvc.begin(SerialIMU);
    
  }
  
  void loop()
  {     
    
    if (rvc.read(&bnoData) )
    {
      toSend++;
      if (toSend > 8)
      {
        toSend = 0;
        
        // YawRate
        if (rvc.angCounter > 0)
        {
            float angVel;
            angVel = ((float)bnoData.angVel) / (float)rvc.angCounter;
            angVel *= 10.0;
            rvc.angCounter = 0;
            bnoData.angVel = (int16_t)angVel;
        }
        else
        {
            bnoData.angVel = 0;
        }
        data[5] = (uint8_t)bnoData.yawX10;
        data[6] = bnoData.yawX10 >> 8;
    
        //the roll x10
        data[7] = (uint8_t)bnoData.rollX10;
        data[8] = bnoData.rollX10 >> 8;

                //the roll x10
        data[9] = (uint8_t)bnoData.angVel;
        data[10] = bnoData.angVel >> 8;
        
        bnoData.angVel = 0;

        int16_t CK_A = 0;
        
        for (int16_t i = 2; i < dataSize - 1; i++)
        {
            CK_A = (CK_A + data[i]);
        }
        
        data[dataSize - 1] = CK_A;
    
        Serial.write(data, dataSize);
        Serial.flush();             
      }
    }         
  }
