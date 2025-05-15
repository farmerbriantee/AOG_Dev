namespace AOG
{
    public class CNozzle
    {
        //pointers to mainform controls Nozzz
        private readonly FormGPS mf;

        public double currentSectionsWidthMeters = 0;

        public double volumePerAreaSetSelected = 0;

        public double volumePerAreaActualFiltered = 0;

        public int volumePerMinuteSet = 0;
        public int volumePerMinuteActual = 0;
        public double frequency = 0;

        public int pressureActual = 0;

        public bool isFlowingFlag = false;

        public int pwmDriveActual = 0;
        public bool isSprayAutoMode = true;

        public double volumeAppliedLast = 0;

        public int percentWidthBypass = 1;

        public CNozzle(FormGPS _f)
        {
            //constructor
            mf = _f;
            volumePerAreaSetSelected = Settings.Tool.setNozz.volumePerAreaSet1;
        }

        public void BuildRatePGN()
        {
            mf.nozz.volumePerMinuteSet = 0;
            mf.nozz.currentSectionsWidthMeters = 0;

            for (int i = 0; i < mf.section.Count; i++)
            {
                //calculate gallons per minute - GPM = GPA X MPH X Width (in inches)/ 5,940
                if (mf.section[i].isSectionOn)
                {
                    mf.nozz.currentSectionsWidthMeters += mf.section[i].sectionWidth;
                }
            }

            mf.nozz.percentWidthBypass = (int)(mf.nozz.currentSectionsWidthMeters / Settings.Tool.toolWidth * 100);

            if (Settings.Tool.setNozz.isBypass)
            {
                mf.nozz.currentSectionsWidthMeters = Settings.Tool.toolWidth;
            }

            if (mf.nozz.currentSectionsWidthMeters != 0)
            {
                if (Settings.User.isMetric)
                {
                    //Liters * 0.00167 𝑥 𝑠𝑤𝑎𝑡ℎ 𝑤𝑖𝑑𝑡ℎ 𝑥 𝐾mh * ( to send as integer 100)
                    mf.nozz.volumePerMinuteSet =
                        (int)(mf.nozz.volumePerAreaSetSelected * 0.167 * mf.nozz.currentSectionsWidthMeters * mf.avgSpeed);
                }
                else
                {
                    mf.nozz.volumePerMinuteSet = (int)(mf.nozz.volumePerAreaSetSelected *
                                                    (mf.avgSpeed * glm.kmhToMphOrKmh) * mf.nozz.currentSectionsWidthMeters * glm.m2InchOrCm / 5940 * 100);
                }

                PGN_227.pgn[PGN_227.volumePerMinuteSetLo] = (byte)(mf.nozz.volumePerMinuteSet);
                PGN_227.pgn[PGN_227.volumePerMinuteSetHi] = unchecked((byte)((mf.nozz.volumePerMinuteSet) >> 8));
                PGN_227.pgn[PGN_227.percentWidthBypass] = (byte)(mf.nozz.percentWidthBypass);
            }
            else
            {
                mf.nozz.volumePerMinuteSet = 0;

                PGN_227.pgn[PGN_227.volumePerMinuteSetLo] = 0;
                PGN_227.pgn[PGN_227.volumePerMinuteSetHi] = 0;
                PGN_227.pgn[PGN_227.percentWidthBypass] = 0;
            }

            PGN_227.pgn[PGN_227.sec1to8] = PGN_254.pgn[PGN_254.sc1to8];
            PGN_227.pgn[PGN_227.sec9to16] = PGN_254.pgn[PGN_254.sc9to16];
            
            PGN_227.pgn[PGN_227.speed] = (byte)(mf.avgSpeed * 10);


            mf.SendPgnToLoop(PGN_227.pgn);
        }
    }

    public class CNozzleSettings
    {
        //used in Properties.settings.settings
        public CNozzleSettings()
        { }

        public double volumePerAreaSet1 = 6;
        public double volumePerAreaSet2 = 12;

        public int pressureMax = 100;
        public int pressureMin = 10;

        public int flowCal = 3300;
        public int pressureCal = 1;

        public byte Kp = 1;
        public byte Ki = 1;

        public byte fastPWM = 100;
        public byte slowPWM = 50;

        public byte deadbandError = 5;

        public byte switchAtFlowError = 20;

        public double rateAlarmPercent = 0.1;

        //manual function up down rate
        public byte manualRate = 50;

        public bool isBypass = false;
        public bool isMeter = false;

        public double volumeApplied = 0;
        public int volumeTankStart = 0;
        

        public string unitsApplied = "Gallons";
        public string unitsPerArea = "GPA";

        public bool isAppliedUnitsNotTankDisplayed = true;

        public double rateNudge = 1;

        public bool isSectionValve3Wire = true;
    }
}