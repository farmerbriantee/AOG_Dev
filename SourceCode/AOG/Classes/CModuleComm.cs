namespace AgOpenGPS
{
    public class CModuleComm
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //Critical Safety Properties
        public bool isOutOfBounds = true;

        // ---- Section control switches to AgOpenGPS  ---------------------------------------------------------
        //PGN - 32736 - 127.249 0x7FF9
        public byte[] ss = new byte[9];

        public byte[] ssP = new byte[9];

        public int
            swHeader = 0,
            swMain = 1,
            swReserve = 2,
            swReserve2 = 3,
            swNumSections = 4,
            swOnGr0 = 5,
            swOffGr0 = 6,
            swOnGr1 = 7,
            swOffGr1 = 8;

        public int pwmDisplay = 0;
        public double actualSteerAngleDegrees = 0;
        public int actualSteerAngleChart = 0, sensorData = -1;

        //for the workswitch
        public bool isSteerWorkSwitchEnabled;

        public bool workSwitchHigh, oldWorkSwitchHigh, steerSwitchHigh, oldSteerSwitchHigh, oldSteerSwitchRemote;

        public double actualToolSteerAngleDegrees = 0;


        //constructor
        public CModuleComm(FormGPS _f)
        {
            mf = _f;
        }

        //Called from "OpenGL.Designer.cs" when requied
        public void CheckWorkAndSteerSwitch()
        {
            //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
            if (steerSwitchHigh != oldSteerSwitchRemote)
            {
                oldSteerSwitchRemote = steerSwitchHigh;
                //steerSwith is active low
                if (steerSwitchHigh == mf.isBtnAutoSteerOn)
                {
                    mf.btnAutoSteer.PerformClick();
                }
            }

            if (Settings.Vehicle.setF_isWorkSwitchEnabled && (oldWorkSwitchHigh != workSwitchHigh))
            {
                oldWorkSwitchHigh = workSwitchHigh;

                if (workSwitchHigh != Settings.Vehicle.setF_isWorkSwitchActiveLow)
                {
                    if (Settings.Vehicle.setF_isWorkSwitchManualSections)
                    {
                        if (mf.manualBtnState != btnStates.On)
                            mf.btnSectionMasterManual.PerformClick();
                    }
                    else
                    {
                        if (mf.autoBtnState != btnStates.Auto)
                            mf.btnSectionMasterAuto.PerformClick();
                    }
                }

                else//Checks both on-screen buttons, performs click if button is not off
                {
                    if (mf.autoBtnState != btnStates.Off)
                        mf.btnSectionMasterAuto.PerformClick();
                    if (mf.manualBtnState != btnStates.Off)
                        mf.btnSectionMasterManual.PerformClick();
                }
            }

            if (Settings.Vehicle.setF_isSteerWorkSwitchEnabled && (oldSteerSwitchHigh != steerSwitchHigh))
            {
                oldSteerSwitchHigh = steerSwitchHigh;

                if ((mf.isBtnAutoSteerOn && true) || false && !steerSwitchHigh)
                {
                    if (Settings.Vehicle.setF_isSteerWorkSwitchManualSections)
                    {
                        if (mf.manualBtnState != btnStates.On)
                            mf.btnSectionMasterManual.PerformClick();
                    }
                    else
                    {
                        if (mf.autoBtnState != btnStates.Auto)
                            mf.btnSectionMasterAuto.PerformClick();
                    }
                }

                else//Checks both on-screen buttons, performs click if button is not off
                {
                    if (mf.autoBtnState != btnStates.Off)
                        mf.btnSectionMasterAuto.PerformClick();
                    if (mf.manualBtnState != btnStates.Off)
                        mf.btnSectionMasterManual.PerformClick();
                }
            }
        }
    }
}