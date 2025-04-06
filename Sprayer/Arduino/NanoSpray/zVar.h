  const uint16_t LOOP_TIME = 200; //5hz
  uint32_t lastTime = LOOP_TIME;
  uint32_t currentTime = LOOP_TIME;

  //Comm checks
  uint8_t watchdogTimer = 0; //make sure we are talking to AOG
  uint8_t serialResetTimer = 0; //if serial buffer is getting full, empty it
  
   //Communication with AgOpenGPS
  int16_t temp, EEread = 0;

   //Parsing PGN
  bool isPGNFound = false, isHeaderFound = false;
  uint8_t pgn = 0, dataLength = 0, idx = 0;
  int16_t tempHeader = 0;

  //pgn 224 or E0 data return;
  uint8_t AOG[] = {0x80,0x81, 0x7f, 224, 8, 0, 0, 0, 0, 0,0,0,0, 0xCC };

  uint8_t flipFlop = 0;

  //The variables used for sections
  uint8_t relayLo = 0;
  uint8_t relayHi = 0;
  
  //flowmeter isr
  volatile uint8_t isr_isFlowing = 0; //have we updated since last loop    
  volatile float isr_flowCountThisLoop = 0; //the total count of flowmeter pulses from loop to loop
  volatile uint32_t isr_flowTime = 1000UL; //the usec of count
  volatile uint32_t isr_flowTimeArr[] = {5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,
                                          5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL
                                          } ; //the usec of count avergaged                                          
  
  uint32_t isr_flowCount = 1UL; //the total count of flowmeter pulses  
  uint8_t ringPos = 0;

  //time between falling edge of flow - ie one "count"
  uint32_t isr_currentTime = 100UL;
  uint32_t isr_lastTime = 50UL;
  
  bool isFlowmeterMoving = true;   
  
  // Actual calculated volume, sent as 10x actual
  uint16_t totalVolume = 0;    
  
  //Set point for the flow control in gal/min
  uint16_t setGPM = 0;  
  float actualGPM;    // Actual calculated flow rate

  //Pressure sensor
  float pressureActual = 50;
  bool isPressureLow = false;

  //functions
  uint8_t autoMode=1, manualUp=0, manualDn=0, manualCounter=0;
  uint16_t zeroTankVolume=0;
  
  //PWM Control
  int16_t pwmDrive = 0; 
  float flowError = 0;
  float integral = 0;

  //Variables for settings stored in EEPROM
   struct Storage {
      uint8_t Kp = 60;  //proportional gain
      float Ki = 0.1;  //integral gain      
      float pressureCalFactor = 1.0;  //pressure gauge gain
      float flowCalFactor = 1000.0; //counts per 10 us gallon sent - but used counts per gallon
      uint8_t minPressurePSI = 0;
      uint8_t fastPWM = 100;
      uint8_t slowPWM = 50;
      uint8_t deadbandError = 20;
      uint8_t switchAtFlowError = 100;
      uint8_t isBypass = 0;
      uint8_t is3Wire = 1;
};  Storage settings;

  //////////
