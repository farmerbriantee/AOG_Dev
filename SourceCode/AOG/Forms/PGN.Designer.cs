using System;
using System.Security.Cryptography;

namespace AOG
{
    public partial class FormGPS
    {
        public void LoadPGNS()
        {
            PGN_252.pgn[PGN_252.gainProportional] = Settings.Vehicle.setAS_Kp;
            PGN_252.pgn[PGN_252.highPWM] = Settings.Vehicle.setAS_highSteerPWM;
            PGN_252.pgn[PGN_252.lowPWM] = Settings.Vehicle.setAS_lowSteerPWM;
            PGN_252.pgn[PGN_252.minPWM] = Settings.Vehicle.setAS_minSteerPWM;
            PGN_252.pgn[PGN_252.countsPerDegree] = Settings.Vehicle.setAS_countsPerDegree;
            PGN_252.pgn[PGN_252.wasOffsetHi] = unchecked((byte)(Settings.Vehicle.setAS_wasOffset >> 8)); ;
            PGN_252.pgn[PGN_252.wasOffsetLo] = unchecked((byte)(Settings.Vehicle.setAS_Kp));
            PGN_252.pgn[PGN_252.ackerman] = Settings.Vehicle.setAS_ackerman;

            //pin relays
            string[] words = Settings.Tool.setRelay_pinConfig.Split(',');

            PGN_236.pgn[PGN_236.pin0] = (byte)int.Parse(words[0]);
            PGN_236.pgn[PGN_236.pin1] = (byte)int.Parse(words[1]);
            PGN_236.pgn[PGN_236.pin2] = (byte)int.Parse(words[2]);
            PGN_236.pgn[PGN_236.pin3] = (byte)int.Parse(words[3]);
            PGN_236.pgn[PGN_236.pin4] = (byte)int.Parse(words[4]);
            PGN_236.pgn[PGN_236.pin5] = (byte)int.Parse(words[5]);
            PGN_236.pgn[PGN_236.pin6] = (byte)int.Parse(words[6]);
            PGN_236.pgn[PGN_236.pin7] = (byte)int.Parse(words[7]);
            PGN_236.pgn[PGN_236.pin8] = (byte)int.Parse(words[8]);
            PGN_236.pgn[PGN_236.pin9] = (byte)int.Parse(words[9]);

            PGN_236.pgn[PGN_236.pin10] = (byte)int.Parse(words[10]);
            PGN_236.pgn[PGN_236.pin11] = (byte)int.Parse(words[11]);
            PGN_236.pgn[PGN_236.pin12] = (byte)int.Parse(words[12]);
            PGN_236.pgn[PGN_236.pin13] = (byte)int.Parse(words[13]);
            PGN_236.pgn[PGN_236.pin14] = (byte)int.Parse(words[14]);
            PGN_236.pgn[PGN_236.pin15] = (byte)int.Parse(words[15]);
            PGN_236.pgn[PGN_236.pin16] = (byte)int.Parse(words[16]);
            PGN_236.pgn[PGN_236.pin17] = (byte)int.Parse(words[17]);
            PGN_236.pgn[PGN_236.pin18] = (byte)int.Parse(words[18]);
            PGN_236.pgn[PGN_236.pin19] = (byte)int.Parse(words[19]);

            PGN_236.pgn[PGN_236.pin20] = (byte)int.Parse(words[20]);
            PGN_236.pgn[PGN_236.pin21] = (byte)int.Parse(words[21]);
            PGN_236.pgn[PGN_236.pin22] = (byte)int.Parse(words[22]);
            PGN_236.pgn[PGN_236.pin23] = (byte)int.Parse(words[23]);

            //nozzle settings
            PGN_225.pgn[PGN_225.zeroTankVolumeLo] = 0;
            PGN_225.pgn[PGN_225.zeroTankVolumeHi] = 0;
            PGN_225.pgn[PGN_225.auto] = 1;
            PGN_225.pgn[PGN_225.up] = 0;
            PGN_225.pgn[PGN_225.dn] = 0;
            PGN_225.pgn[PGN_225.rate] = 50;
        }
    }

    //AutoSteerData
    public static class PGN_254
    {
        /// <summary>
        /// 8 bytes
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 254, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int speedLo = 5;
        public static int speedHi = 6;
        public static int status = 7;
        public static int steerAngleLo = 8;
        public static int steerAngleHi = 9;
        public static int lineDistance = 10;
        public static int sc1to8 = 11;
        public static int sc9to16 = 12;
    }

    public static class PGN_253
    {
        /// <summary>
        /// From steer module
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 253, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int actualLo = 5;
        public static int actualHi = 6;
        public static int headLo = 7;
        public static int headHi = 8;
        public static int rollLo = 9;
        public static int rollHi = 10;
        public static int switchStatus = 11;
        public static int pwm = 12;
    }


    //AutoSteer Settings
    public static class PGN_252
    {
        /// <summary>
        /// PGN - 252 - FC gainProportional=5 HighPWM=6  LowPWM = 7 MinPWM = 8 
        /// CountsPerDegree = 9 wasOffsetHi = 10 wasOffsetLo = 11 
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 252, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int gainProportional = 5;
        public static int highPWM = 6;
        public static int lowPWM = 7;
        public static int minPWM = 8;
        public static int countsPerDegree = 9;
        public static int wasOffsetLo = 10;
        public static int wasOffsetHi = 11;
        public static int ackerman = 12;
    }

    //Autosteer Board Config
    public static class PGN_251
    {
        /// <summary>
        /// 
        /// PGN - 251 - FB 
        /// set0=5 maxPulse = 6 minSpeed = 7 ackermanFix = 8
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 251, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int set0 = 5;
        public static int maxPulse = 6;
        public static int minSpeed = 7;
        public static int set1 = 8;
        public static int angVel = 9;
        //public static int  = 10;
        //public static int  = 11;
        //public static int  = 12;

    }

    //Machine Data
    public static class PGN_239
    {
        /// <summary>
        /// PGN - 239 - EF 
        /// uturn=5  tree=6  hydLift = 8 
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 239, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int uturn = 5;
        public static int speed = 6;
        public static int hydLift = 7;
        public static int tram = 8;
        public static int geoStop = 9; //out of bounds etc
                                       //public static int  = 10;
        public static int sc1to8 = 11;
        public static int sc9to16 = 12;
    }

    public static class PGN_229
    {
        /// <summary>
        /// PGN - 229 - E5 
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 229, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int sc1to8 = 5;
        public static int sc9to16 = 6;
        public static int sc17to24 = 7;
        public static int sc25to32 = 8;
        public static int sc33to40 = 9;
        public static int sc41to48 = 10;
        public static int sc49to56 = 11;
        public static int sc57to64 = 12;
        public static int toolLSpeed = 13;
        public static int toolRSpeed = 14;
    }

    //Machine Config
    public static class PGN_238
    {
        /// <summary>
        /// PGN - 238 - EE 
        /// raiseTime=5  lowerTime=6   enableHyd= 7 set0 = 8
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 238, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int raiseTime = 5;
        public static int lowerTime = 6;
        public static int enableHyd = 7;
        public static int set0 = 8;
        public static int user1 = 9;
        public static int user2 = 10;
        public static int user3 = 11;
        public static int user4 = 12;
    }

    //Relay Config
    public static class PGN_236
    {
        /// <summary>
        /// PGN - 236 - EC
        /// Pin conifg 1 to 20
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 236, 24,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0xCC };

        //where in the pgn is which pin
        public static int pin0 = 5;
        public static int pin1 = 6;
        public static int pin2 = 7;
        public static int pin3 = 8;
        public static int pin4 = 9;
        public static int pin5 = 10;
        public static int pin6 = 11;
        public static int pin7 = 12;
        public static int pin8 = 13;
        public static int pin9 = 14;

        public static int pin10 = 15;
        public static int pin11 = 16;
        public static int pin12 = 17;
        public static int pin13 = 18;
        public static int pin14 = 19;
        public static int pin15 = 20;
        public static int pin16 = 21;

        public static int pin17 = 22;
        public static int pin18 = 23;
        public static int pin19 = 24;
        public static int pin20 = 25;
        public static int pin21 = 26;
        public static int pin22 = 27;
        public static int pin23 = 28;
    }

    public static class PGN_235
    {
        /// <summary>
        /// PGN - 235 - EB
        /// Section dimensions
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xEB, 33,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0xCC };

        //where in the pgn is which pin
        public static int sec0Lo = 5;
        public static int sec1Lo = 7;
        public static int sec2Lo = 9;
        public static int sec3Lo = 11;
        public static int sec4Lo = 13;
        public static int sec5Lo = 15;
        public static int sec6Lo = 17;
        public static int sec7Lo = 19;
        public static int sec8Lo = 21;
        public static int sec9Lo = 23;
        public static int sec10Lo = 25;
        public static int sec11Lo = 27;
        public static int sec12Lo = 29;
        public static int sec13Lo = 31;
        public static int sec14Lo = 33;
        public static int sec15Lo = 35;

        public static int sec0Hi = 6;
        public static int sec1Hi = 8;
        public static int sec2Hi = 10;
        public static int sec3Hi = 12;
        public static int sec4Hi = 14;
        public static int sec5Hi = 16;
        public static int sec6Hi = 18;
        public static int sec7Hi = 20;
        public static int sec8Hi = 22;
        public static int sec9Hi = 24;
        public static int sec10Hi = 26;
        public static int sec11Hi = 28;
        public static int sec12Hi = 30;
        public static int sec13Hi = 32;
        public static int sec14Hi = 34;
        public static int sec15Hi = 36;

        public static int numSections = 37;
    }

    public static class PGN_228
    {
        /// <summary>
        /// 8 bytes
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 228, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int rate0 = 5;
        public static int rate1 = 6;
        public static int rate2 = 7;
        //public static int  = 6;
        //public static int = 7;
        //public static int gleLo = 8;
        //public static int gleHi = 9;
        //public static int tance = 10;
        //public static int = 11;
        //public static int  = 12;
    }

    // Nozzle -----------------------------------------------------------------------------------

    //Spray Data
    public static class PGN_227
    {
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 0xE3, 6, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int sec1to8 = 5;
        public static int sec9to16 = 6;
        public static int volumePerMinuteSetLo = 7;
        public static int volumePerMinuteSetHi = 8;
        public static int percentWidthBypass = 9;
        public static int speed = 10;

    }

    //Spray Settings
    public static class PGN_226
    {
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 226, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int flowCalLo = 5;
        public static int flowCalHi = 6;
        public static int pressureCalLo = 7;
        public static int pressureCalHi = 8;
        public static int Kp = 9;
        public static int Ki = 10;
        public static int minPressure = 11;
        public static int fastPWM = 12;
        public static int slowPWM = 13;
        public static int deadbandError = 14;
        public static int switchAtFlowError = 15;
        public static int isBypass = 16;
        public static int isSectionValve3Wire = 17;

    }

    //Spray Functions
    public static class PGN_225
    {
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 225, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };

        public static int zeroTankVolumeLo = 5;
        public static int zeroTankVolumeHi = 6;
        public static int auto = 7;
        public static int up = 8;
        public static int dn = 9;
        public static int rate = 10;
    }

    // Tool Steer ------------------------------------------------------------------------------

    //ToolSteerData
    //Low XTE	High XTE	Status	Vehicle XTE		Speed * 10	Vehicle Heading

    public static class PGN_233
    {
        /// <summary>
        /// PGN_233_E9
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 233, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int xteLo = 5;
        public static int xteHi = 6;
        public static int status = 7;
        public static int xteVehLo = 8;
        public static int xteVehHi = 9;
        public static int speed10 = 10;
        public static int headLo = 11;
        public static int headHi = 12;
    }

    //ToolSteer Settings
    public static class PGN_232
    {
        /// <summary>
        /// PGN - 232_E8 gainP=5 integral=6  MinPWM = 7 hiPWM = 8
        /// CountsPerDegree = 9 wasOffsetLo = 10 wasOffsetHi = 11 
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 232, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int gainP = 5;
        public static int integral = 6;
        public static int minPWM = 7;
        public static int highPWM = 8;
        public static int countsPerDegree = 9;
        public static int wasOffsetLo = 10;
        public static int wasOffsetHi = 11;
        public static int ackerman = 12;
    }

    //Toolsteer Config
    public static class PGN_231
    {
        /// <summary>
        /// 
        /// PGN - 231 - E7 
        /// invWas = 5 invSteer = 6 maxSteer = 7
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 231, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int invertWAS = 5;
        public static int invertSteer = 6;
        public static int maxSteerAngle = 7;
        public static int isSteer = 8;
    }

    //From Tool Steer Board
    public static class PGN_230
    {
        /// <summary>
        /// From Tool steer module
        /// </summary>
        public static byte[] pgn = new byte[] { 0x80, 0x81, 0x7f, 230, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        public static int actualLo = 5;
        public static int actualHi = 6;
        public static int rollLo = 7;
        public static int rollHi = 8;
        public static int pwm = 9;
        public static int status = 10;
    }
}