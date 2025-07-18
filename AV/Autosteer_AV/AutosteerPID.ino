void calcSteeringPID(void)
{
    //Proportional only
    pValue = steerSettings.Kp * angularVelocityError;
    pwmDrive = (int16_t)pValue;

    //limit the pwm drive
    if (pwmDrive > steerSettings.highPWM) pwmDrive = steerSettings.highPWM;
    if (pwmDrive < -steerSettings.highPWM) pwmDrive = -steerSettings.highPWM;

    if (steerConfig.MotorDriveDirection) pwmDrive *= -1;
}

//#########################################################################################

void motorDrive(void)
{
    // Cytron MD30C Driver Dir + PWM Signal
    if (pwmDrive >= 0)
    {
        bitSet(PORTD, 4);  //set the correct direction
    }
    else
    {
        bitClear(PORTD, 4);
        pwmDrive = -1 * pwmDrive;
    }

    //write out the 0 to 255 value 
    analogWrite(PWM1_LPWM, pwmDrive);

    pwmDisplay = pwmDrive;
}
