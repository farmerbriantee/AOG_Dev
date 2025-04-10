void SetPWM(double PWM)
{
    if (pwmDrive > 0)
    {
        ledcWrite(Motor2, 0);   // IN2
        ledcWrite(Motor1, PWM);     // IN1
    }
    else
    {
        ledcWrite(Motor1, 0);       // IN1
        ledcWrite(Motor2, PWM); // IN2
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
