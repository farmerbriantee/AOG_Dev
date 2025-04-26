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

double NewPWM = 0;


void DoPID(void)
{
    if (settings.isMeter)
    {
        flowError = (setGPM - actualGPM);
        if (abs(flowError) > (settings.deadbandError * setGPM))
        {
            flowError = constrain(flowError, setGPM * -1, setGPM);
            if (abs(flowError) < (0.5 * setGPM))
            {
                //flowError *= 0.1;
                NewPWM += flowError * gainPWM;
            }
            else
            {
                //flowError *= 0.1;
                NewPWM += 2 * flowError * gainPWM;
            }

            NewPWM = constrain(NewPWM, 0, 255);
            pwmDrive = NewPWM;
        }
    }
    else
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
    }

    SetPWM(abs(pwmDrive));
}

void DoManualPID(void)
{
    SetPWM(abs(pwmDrive));
}
