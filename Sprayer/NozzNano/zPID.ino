void SetPWM(double PWM)
{
    // Dir + PWM Signal
    if (pwmDrive >= 0)
    {
        analogWrite(Motor2, 0);
        analogWrite(Motor1, PWM);
    }
    else
    {
        analogWrite(Motor1, 0);
        analogWrite(Motor2, PWM);
    }
}

void DoPID(void)
{
    flowError = (setGPM - actualGPM);

    if (abs(flowError) < (settings.deadbandError * setGPM))
    {
        pwmDrive = 0;
    }
    else
    {
        if (abs(flowError) < (settings.switchAtFlowError * setGPM))
        {
            pwmDrive = settings.slowPWM;
        }
        else
        {
            pwmDrive = settings.fastPWM;
        }

        if (flowError < 0)
        {
            pwmDrive = -pwmDrive;
        }
    }

    SetPWM(abs(pwmDrive));

}

void DoManualPID(void)
{
    SetPWM(abs(pwmDrive));
}
