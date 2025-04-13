namespace AOG
{
    public class CFeatureSettings
    {
        public CFeatureSettings()
        { }

        //public bool ;
        public bool isHeadlandOn = true;

        public bool isTramOn = false;
        public bool isBoundaryOn = true;
        public bool isBndContourOn = false;
        public bool isRecPathOn = false;
        public bool isABSmoothOn = false;

        public bool isHideContourOn = false;
        public bool isWebCamOn = false;
        public bool isOffsetFixOn = false;
        public bool isAgIOOn = true;

        public bool isContourOn = true;
        public bool isYouTurnOn = true;
        public bool isSteerModeOn = true;

        public bool isManualSectionOn = true;
        public bool isAutoSectionOn = true;
        public bool isCycleLinesOn = true;
        public bool isABLineOn = true;
        public bool isCurveOn = true;
        public bool isAutoSteerOn = true;

        public bool isUTurnOn = true;
        public bool isLateralOn = true;

        public CFeatureSettings(CFeatureSettings _feature)
        {
            isHeadlandOn = _feature.isHeadlandOn;
            isTramOn = _feature.isTramOn;
            isBoundaryOn = _feature.isBoundaryOn;
            isBndContourOn = _feature.isBndContourOn;
            isRecPathOn = _feature.isRecPathOn;

            isABSmoothOn = _feature.isABSmoothOn;
            isHideContourOn = _feature.isHideContourOn;
            isWebCamOn = _feature.isWebCamOn;
            isOffsetFixOn = _feature.isOffsetFixOn;
            isAgIOOn = _feature.isAgIOOn;

            isContourOn = _feature.isContourOn;
            isYouTurnOn = _feature.isYouTurnOn;
            isSteerModeOn = _feature.isSteerModeOn;

            isManualSectionOn = _feature.isManualSectionOn;
            isAutoSectionOn = _feature.isAutoSectionOn;
            isCycleLinesOn = _feature.isCycleLinesOn;
            isABLineOn = _feature.isABLineOn;
            isCurveOn = _feature.isCurveOn;

            isAutoSteerOn = _feature.isAutoSteerOn;
            isLateralOn = _feature.isLateralOn;
            isUTurnOn = _feature.isUTurnOn;
        }
    }

    public class CToolSteerSettings
    {
        public CToolSteerSettings()
        { }

        //public bool ;
        public bool isGPSToolActive = false;

        public byte gainP           = 50;
        public byte integral        = 0;
        public byte minPWM          = 30;
        public byte highPWM         = 200;
        public byte countsPerDegree = 100;

        public int wasOffset     = 0;
        public byte ackermann       = 100;
        public byte maxSteerAngle = 20;

        public byte isInvertWAS = 0;
        public byte isInvertSteer = 0;
        public byte isSteerNotSlide = 1;

        public double antennaHeight = 0;
        public double antennaOffset = 0;

        public CToolSteerSettings(CToolSteerSettings _setting)
        {
            isGPSToolActive = _setting.isGPSToolActive;

            gainP = _setting.gainP;
            integral = _setting.integral;
            minPWM = _setting.minPWM;
            highPWM = _setting.highPWM;
            countsPerDegree = _setting.countsPerDegree;

            wasOffset = _setting.wasOffset;
            ackermann = _setting.ackermann;

            isInvertWAS = _setting.isInvertWAS;
            isInvertSteer = _setting.isInvertSteer;
            maxSteerAngle = _setting.maxSteerAngle;
            isSteerNotSlide = _setting.isSteerNotSlide;

            antennaHeight = _setting.antennaHeight;
            antennaOffset = _setting.antennaOffset;
        }
    }
}