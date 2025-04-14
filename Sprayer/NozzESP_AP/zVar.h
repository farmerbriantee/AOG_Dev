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
uint8_t AOG[] = { 0x80,0x81, 0x7f, 224, 10, 0, 0, 0, 0, 0,0,0,0,0,0, 0xCC };

//hello from AgIO
uint8_t helloFromMachine[] = { 128, 129, 123, 123, 5, 0, 0, 0, 0, 0, 71 };

uint8_t flipFlop = 0;

//The variables used for sections
uint8_t relayLo = 0;
uint8_t relayHi = 0;
float sectionWidthPercent = 1;

//flowmeter isr
volatile uint8_t isr_isFlowing = 0; //have we updated since last loop
volatile uint16_t isr_flowCountThisLoop = 0; //the total count of flowmeter pulses from loop to loop
volatile uint32_t isr_flowTime = 1000UL; //the usec of count
volatile uint32_t isr_flowTimeArr[] = { 5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,
										5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL,5000UL
}; //the usec of count avergaged

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
float pressureActual;

//speed
float speed = 0;

//functions
uint8_t autoMode = 1, manualUp = 0, manualDn = 0, manualCounter = 0;
uint16_t zeroTankVolume = 0;

//PWM Control
int16_t pwmDrive = 0;
float flowError = 0;
float integral = 0;

//Variables for settings stored in EEPROM
struct Storage {
	//stored spray settings
	uint8_t Kp = 60;  //proportional gain
	float Ki = 0.1;  //integral gain
	float pressureCalFactor = 10.0;  //pressure gauge gain
	float flowCalFactor = 330; //counts per 10 us gallon sent - but used counts per gallon
	uint8_t minPressurePSI = 0;
	uint8_t fastPWM = 100;
	uint8_t slowPWM = 75;
	float deadbandError = 0.05;
	float switchAtFlowError = 0.25;
	uint8_t isBypass = 0;
	uint8_t is3Wire = 1;

	//spray functions - sets manual up down rate pwm
	uint8_t manualRate = 125;
};  Storage settings;

void IRAM_ATTR isr_Flow()
{
	isr_currentTime = micros();
	isr_flowTime = isr_currentTime - isr_lastTime;
	isr_lastTime = isr_currentTime;

	//either way too long or way too short
	if (isr_flowTime > 200000UL || isr_flowTime < 2000UL)
	{
		isr_lastTime = isr_currentTime;
		isr_isFlowing = 0;
		return;
	}

	//counts for total volume
	isr_flowCountThisLoop = isr_flowCountThisLoop + 1;
	isr_isFlowing = 1;

	//use ring counter
	isr_flowTimeArr[ringPos] = isr_flowTime;
	ringPos++;
	if (ringPos > 15) ringPos = 0;
	isr_flowTime = (isr_flowTimeArr[0] + isr_flowTimeArr[1] + isr_flowTimeArr[2] + isr_flowTimeArr[3] +
		isr_flowTimeArr[4] + isr_flowTimeArr[5] + isr_flowTimeArr[6] + isr_flowTimeArr[7] +
		isr_flowTimeArr[8] + isr_flowTimeArr[9] + isr_flowTimeArr[10] + isr_flowTimeArr[11] +
		isr_flowTimeArr[12] + isr_flowTimeArr[13] + isr_flowTimeArr[14] + isr_flowTimeArr[15]);
	isr_flowTime = isr_flowTime / 16;
}
