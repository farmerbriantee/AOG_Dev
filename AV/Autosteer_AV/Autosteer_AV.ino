  /*
  * USB Autosteer code For AgOpenGPS
  * 4 Feb 2021, Brian Tischler
  * Like all Arduino code - copied from somewhere else :)
  * So don't claim it as your own
  */
  
////////////////// User Settings /////////////////////////  

  /*  PWM Frequency -> 
   *   490hz (default) = 0
   *   122hz = 1
   *   3921hz = 2
   */
  #define PWM_Frequency 0
  
/////////////////////////////////////////////

  // if not in eeprom, overwrite 
  #define EEP_Ident 5100 

  // BNO08x definitions
  #define REPORT_INTERVAL 90 //Report interval in ms (same as the delay at the bottom)

  //   ***********  Motor drive connections  **************888
  //Connect ground only for cytron, Connect Ground and +5v for IBT2
    
  //Dir1 for Cytron Dir, Both L and R enable for IBT2
  #define DIR1_RL_ENABLE  4  //PD4

  //PWM1 for Cytron PWM, Left PWM for IBT2
  #define PWM1_LPWM  3  //PD3

  //Not Connected for Cytron, Right PWM for IBT2
  #define PWM2_RPWM  9 //D9

  //--------------------------- Switch Input Pins ------------------------
  #define STEERSW_PIN 6 //PD6
  #define WORKSW_PIN 7  //PD7
  #define REMOTE_PIN 8  //PB0
  
  #define CONST_180_DIVIDED_BY_PI 57.2957795130823

  #include <EEPROM.h> 
 
  //loop time variables in microseconds  
  const uint16_t LOOP_TIME = 20;  //50Hz    
  uint32_t lastTime = LOOP_TIME;
  uint32_t currentTime = LOOP_TIME;
  
  const uint8_t WATCHDOG_LIMIT = 100;
  uint8_t watchdogTimer = 200;
  
   //Parsing PGN
  bool isPGNFound = false, isHeaderFound = false;
  uint8_t pgn = 0, dataLength = 0, idx = 0;
  int16_t tempHeader = 0;

  //show life in AgIO
  uint8_t helloAgIO[] = {0x80,0x81, 0x7f, 0xC7, 1, 0, 0x47 };
  uint8_t helloCounter=0;

  //fromAutoSteerData FD 253 - ActualSteerAngle*100 -5,6, Heading-7,8, 
        //Roll-9,10, SwitchByte-11, pwmDisplay-12, CRC 13
  uint8_t PGN_253[] = {0x80,0x81, 0x7f, 0xFD, 8, 0, 0, 0, 0, 0,0,0,0, 0xCC };
  int8_t PGN_253_Size = sizeof(PGN_253) - 1;
  
  //EEPROM
  int16_t EEread = 0;
 
  //Relays
  bool isRelayActiveHigh = true;
  uint8_t relay = 0, relayHi = 0, uTurn = 0;
  uint8_t xte = 0;
  
  //Switches
  uint8_t remoteSwitch = 0, workSwitch = 0, steerSwitch = 1, switchByte = 0;

  //On Off
  uint8_t guidanceStatus = 0;
  uint8_t prevGuidanceStatus = 0;
  bool guidanceStatusChanged = false;

  //speed sent as *10
  float gpsSpeed = 0;
  float angularVelocityError = 0;
 
  //pwm variables
  int16_t pwmDrive = 0, pwmDisplay = 0;
  float pValue = 0;
 
  //Steer switch button  ***********************************************************************************************************
  uint8_t currentState = 1, reading, previous = 0;
  uint8_t pulseCount = 0; // Steering Wheel Encoder
  bool encEnable = false; //debounce flag
  uint8_t thisEnc = 0, lastEnc = 0;

   //Variables for settings  
   struct Storage {
      uint8_t Kp = 120;  //proportional gain
      uint8_t lowPWM = 30;  //band of no action
      int16_t wasOffset = 0;
      uint8_t minPWM = 25;
      uint8_t highPWM = 160;//max PWM value
      float steerSensorCounts = 30;        
      float AckermanFix = 1;     //sent as percent
  };  Storage steerSettings;  //14 bytes

   //Variables for settings - 0 is false  
   struct Setup {
      uint8_t InvertWAS = 0;
      uint8_t IsRelayActiveHigh = 0; //if zero, active low (default)
      uint8_t MotorDriveDirection = 0;
      uint8_t SingleInputWAS = 1;
      uint8_t CytronDriver = 1;
      uint8_t SteerSwitch = 0;  //1 if switch selected
      uint8_t SteerButton = 0;  //1 if button selected
      uint8_t ShaftEncoder = 0;
      uint8_t PressureSensor = 0;
      uint8_t CurrentSensor = 0;
      uint8_t PulseCountMax = 5;
      uint8_t IsDanfoss = 0; 
      uint8_t IsUseY_Axis = 0;
  };  Setup steerConfig;          //9 bytes

  //reset function
  void(* resetFunc) (void) = 0;

  void setup()
  {
      Serial.begin(38400);

      //keep pulled high and drag low to activate, noise free safe   
      pinMode(WORKSW_PIN, INPUT_PULLUP);
      pinMode(STEERSW_PIN, INPUT_PULLUP);
      pinMode(REMOTE_PIN, INPUT_PULLUP);
      pinMode(DIR1_RL_ENABLE, OUTPUT);

      if (steerConfig.CytronDriver) pinMode(PWM2_RPWM, OUTPUT);

      EEPROM.get(0, EEread);              // read identifier

      if (EEread != EEP_Ident)   // check on first start and write EEPROM
      {
          EEPROM.put(0, EEP_Ident);
          EEPROM.put(10, steerSettings);
          EEPROM.put(40, steerConfig);
      }
      else
      {
          EEPROM.get(10, steerSettings);     // read the Settings
          EEPROM.get(40, steerConfig);
      }
  }// End of Setup

  void loop()
  {
      // Loop triggers every 50 msec and sends back steer angle etc   
      currentTime = millis();

      if (currentTime - lastTime >= LOOP_TIME)
      {
          lastTime = currentTime;

          //reset debounce
          encEnable = true;

          //If connection lost to AgOpenGPS, the watchdog will count up and turn off steering
          if (watchdogTimer++ > 253) watchdogTimer = WATCHDOG_LIMIT + 1;

          //read all the switches
          workSwitch = digitalRead(WORKSW_PIN);  // read work switch

          if (steerConfig.SteerSwitch == 1)         //steer switch on - off
          {
              steerSwitch = digitalRead(STEERSW_PIN); //read auto steer enable switch open = 0n closed = Off
          }
          else if (steerConfig.SteerButton == 1)    //steer Button momentary
          {
              reading = digitalRead(STEERSW_PIN);
              if (reading == LOW && previous == HIGH)
              {
                  if (currentState == 1)
                  {
                      currentState = 0;
                      steerSwitch = 0;
                  }
                  else
                  {
                      currentState = 1;
                      steerSwitch = 1;
                  }
              }
              previous = reading;
          }

          remoteSwitch = digitalRead(REMOTE_PIN); //read auto steer enable switch open = 0n closed = Off

          switchByte = 0;
          switchByte |= (remoteSwitch << 2); //put remote in bit 2
          switchByte |= (steerSwitch << 1);   //put steerswitch status in bit 1 position
          switchByte |= workSwitch;

          if (steerConfig.InvertWAS)
          {
              angularVelocityError *= -1;
          }

          if (watchdogTimer > WATCHDOG_LIMIT || (bitRead(guidanceStatus, 0) == 0) || (gpsSpeed < 2) || (steerSwitch == 1))
          {
              //we've lost the comm to AgOpenGPS, or just stop request, or slow
              //Disable H Bridge for IBT2, hyd aux, etc for cytron
              if (steerConfig.CytronDriver)
              {
                  if (steerConfig.IsRelayActiveHigh) digitalWrite(PWM2_RPWM, 1);
                  else digitalWrite(PWM2_RPWM, 0);
              }
              else digitalWrite(DIR1_RL_ENABLE, 0); //IBT2

              pwmDrive = 0; //turn off steering motor
              motorDrive(); //out to motors the pwm value
              pulseCount = 0;
          }
          else
          {
              //Enable H Bridge for IBT2, hyd aux, etc for cytron
              if (steerConfig.CytronDriver)
              {
                  if (steerConfig.IsRelayActiveHigh) digitalWrite(PWM2_RPWM, 0);
                  else digitalWrite(PWM2_RPWM, 1);
              }
              else digitalWrite(DIR1_RL_ENABLE, 1);

              calcSteeringPID();  //do the pid
              motorDrive();       //out to motors the pwm value
          }

          //send empty pgn to AgIO to show activity
          if (++helloCounter > 150)
          {
              Serial.write(helloAgIO, sizeof(helloAgIO));
              helloCounter = 0;
          }
      } //end of timed loop

      //This runs continuously, not timed //// Serial Receive Data/Settings /////////////////

      // Serial Receive
      //Do we have a match with 0x8081?    
      if (Serial.available() > 1 && !isHeaderFound && !isPGNFound)
      {
          uint8_t temp = Serial.read();
          if (tempHeader == 0x80 && temp == 0x81)
          {
              isHeaderFound = true;
              tempHeader = 0;
          }
          else
          {
              tempHeader = temp;     //save for next time
              return;
          }
      }

      //Find Source, PGN, and Length
      if (Serial.available() > 2 && isHeaderFound && !isPGNFound)
      {
          Serial.read(); //The 7F or less
          pgn = Serial.read();
          dataLength = Serial.read();
          isPGNFound = true;
          idx = 0;
      }

      //The data package
      if (Serial.available() > dataLength && isHeaderFound && isPGNFound)
      {
          if (pgn == 254) //FE AutoSteerData
          {
              watchdogTimer = 0;  //reset watchdog   

              //bit 5,6
              gpsSpeed = ((float)(Serial.read() | Serial.read() << 8)) * 0.1;

              prevGuidanceStatus = guidanceStatus;

              //bit 7
              guidanceStatus = Serial.read();
              guidanceStatusChanged = (guidanceStatus != prevGuidanceStatus);

              //Bit 8,9    set point steer angle * 100 is sent
              angularVelocityError = ((float)(Serial.read() | Serial.read() << 8)) * 0.01; //high low bytes


              //Bit 10 Tram 
              xte = Serial.read();

              //Bit 11 section 1 to 8
              relay = Serial.read();

              //Bit 12 section 9 to 16
              relayHi = Serial.read();


              //Bit 13 CRC
              Serial.read();

              //reset for next pgn sentence
              isHeaderFound = isPGNFound = false;
              pgn = dataLength = 0;

              //----------------------------------------------------------------------------
              //Serial Send to agopenGPS
              // Steer Data to AOG
              int16_t sa = (int16_t)(angularVelocityError * 100);
              PGN_253[5] = (uint8_t)sa;
              PGN_253[6] = sa >> 8;

              //heading         
              PGN_253[7] = (uint8_t)9999;
              PGN_253[8] = 9999 >> 8;

              //roll
              PGN_253[9] = (uint8_t)8888;
              PGN_253[10] = 8888 >> 8;

              PGN_253[11] = switchByte;
              PGN_253[12] = (uint8_t)pwmDisplay;

              //add the checksum for AOG
              int16_t CK_A = 0;
              for (uint8_t i = 2; i < PGN_253_Size; i++)
              {
                  CK_A = (CK_A + PGN_253[i]);
              }

              PGN_253[PGN_253_Size] = CK_A;

              //send to AOG
              Serial.write(PGN_253, sizeof(PGN_253));

              // Stop sending the helloAgIO message
              if (helloCounter) helloCounter = 0;
              //--------------------------------------------------------------------------              
          }

          else if (pgn == 252) //FC AutoSteerSettings
          {
              //PID values
              steerSettings.Kp = Serial.read();   // read Kp from AgOpenGPS

              steerSettings.highPWM = Serial.read();

              steerSettings.lowPWM = Serial.read();   // read lowPWM from AgOpenGPS

              steerSettings.minPWM = Serial.read(); //read the minimum amount of PWM for instant on

              float temp = steerSettings.minPWM;
              temp *= 1.2;

              steerSettings.lowPWM = (uint8_t)temp;

              steerSettings.steerSensorCounts = Serial.read(); //sent as setting displayed in AOG

              steerSettings.wasOffset = (Serial.read());  //read was zero offset Hi

              steerSettings.wasOffset |= (Serial.read() << 8);  //read was zero offset Lo

              steerSettings.AckermanFix = (float)Serial.read() * 0.01;

              //crc
              //udpData[13];        //crc
              Serial.read();

              //store in EEPROM
              EEPROM.put(10, steerSettings);

              //reset for next pgn sentence
              isHeaderFound = isPGNFound = false;
              pgn = dataLength = 0;
          }

          else if (pgn == 251) //FB - steerConfig
          {
              uint8_t sett = Serial.read();

              if (bitRead(sett, 0)) steerConfig.InvertWAS = 1; else steerConfig.InvertWAS = 0;
              if (bitRead(sett, 1)) steerConfig.IsRelayActiveHigh = 1; else steerConfig.IsRelayActiveHigh = 0;
              if (bitRead(sett, 2)) steerConfig.MotorDriveDirection = 1; else steerConfig.MotorDriveDirection = 0;
              if (bitRead(sett, 3)) steerConfig.SingleInputWAS = 1; else steerConfig.SingleInputWAS = 0;
              if (bitRead(sett, 4)) steerConfig.CytronDriver = 1; else steerConfig.CytronDriver = 0;
              if (bitRead(sett, 5)) steerConfig.SteerSwitch = 1; else steerConfig.SteerSwitch = 0;
              if (bitRead(sett, 6)) steerConfig.SteerButton = 1; else steerConfig.SteerButton = 0;
              if (bitRead(sett, 7)) steerConfig.ShaftEncoder = 1; else steerConfig.ShaftEncoder = 0;

              steerConfig.PulseCountMax = Serial.read();

              //was speed
              Serial.read();

              sett = Serial.read(); //byte 8 - setting1 - Danfoss valve etc

              if (bitRead(sett, 0)) steerConfig.IsDanfoss = 1; else steerConfig.IsDanfoss = 0;
              if (bitRead(sett, 1)) steerConfig.PressureSensor = 1; else steerConfig.PressureSensor = 0;
              if (bitRead(sett, 2)) steerConfig.CurrentSensor = 1; else steerConfig.CurrentSensor = 0;
              if (bitRead(sett, 3)) steerConfig.IsUseY_Axis = 1; else steerConfig.IsUseY_Axis = 0;

              Serial.read(); //byte 9
              Serial.read(); //byte 10

              Serial.read(); //byte 11
              Serial.read(); //byte 12

              //crc byte 13
              Serial.read();

              EEPROM.put(40, steerConfig);

              //reset for next pgn sentence
              isHeaderFound = isPGNFound = false;
              pgn = dataLength = 0;

              //reset the arduino
              resetFunc();
          }

          //clean up strange pgns
          else
          {
              //reset for next pgn sentence
              isHeaderFound = isPGNFound = false;
              pgn = dataLength = 0;
          }

      } //end if (Serial.available() > dataLength && isHeaderFound && isPGNFound)      

      if (encEnable)
      {
          thisEnc = digitalRead(REMOTE_PIN);
          if (thisEnc != lastEnc)
          {
              lastEnc = thisEnc;
              if (lastEnc) EncoderFunc();
          }
      }

  } // end of main loop

  //ISR Steering Wheel Encoder
  void EncoderFunc()
  {
      if (encEnable)
      {
          pulseCount++;
          encEnable = false;
      }
  }

  //TCCR2B = TCCR2B & B11111000 | B00000001;    // set timer 2 divisor to     1 for PWM frequency of 31372.55 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000010;    // set timer 2 divisor to     8 for PWM frequency of  3921.16 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000011;    // set timer 2 divisor to    32 for PWM frequency of   980.39 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000100;    // set timer 2 divisor to    64 for PWM frequency of   490.20 Hz (The DEFAULT)
  //TCCR2B = TCCR2B & B11111000 | B00000101;    // set timer 2 divisor to   128 for PWM frequency of   245.10 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000110;    // set timer 2 divisor to   256 for PWM frequency of   122.55 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000111;    // set timer 2 divisor to  1024 for PWM frequency of    30.64 Hz

  //TCCR1B = TCCR1B & B11111000 | B00000001;    // set timer 1 divisor to     1 for PWM frequency of 31372.55 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000010;    // set timer 1 divisor to     8 for PWM frequency of  3921.16 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000011;    // set timer 1 divisor to    64 for PWM frequency of   490.20 Hz (The DEFAULT)
  //TCCR1B = TCCR1B & B11111000 | B00000100;    // set timer 1 divisor to   256 for PWM frequency of   122.55 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000101;    // set timer 1 divisor to  1024 for PWM frequency of    30.64 Hz
