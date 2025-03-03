namespace AgOpenGPS
{
    public class CAHRS
    {
        //private readonly FormGPS mf;

        //Roll and heading from the IMU
        public double imuHeading = 99999, prevIMUHeading = 0, imuRoll = 0, imuPitch = 0, imuYawRate = 0;

        public System.Int16 angVel;

        //actual value in degrees
        public double rollZero;

        //Roll Filter Value
        public double rollFilter;

        //is the auto steer in auto turn on mode or not
        public bool isAutoSteerAuto, isRollInvert, isReverseOn;

        //the factor for fusion of GPS and IMU
        public double fusionWeight;

        //constructor
        public CAHRS()
        {
            rollZero = Settings.Vehicle.setIMU_rollZero;

            rollFilter = Settings.Vehicle.setIMU_rollFilter;

            fusionWeight = Settings.Vehicle.setIMU_fusionWeight2;

            //isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn;
            isAutoSteerAuto = true;


            isRollInvert = Settings.Vehicle.setIMU_invertRoll;

            isReverseOn = Settings.Vehicle.setIMU_isReverseOn;
        }
    }
}