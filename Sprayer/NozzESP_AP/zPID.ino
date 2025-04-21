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

double NewPWM = 0;
const double Scaling = 0.0001;
const double SampleTime = 50;   // ms
uint32_t LastCheck;


void DoPID(void)
{
    if (settings.isMeter)
    {
        flowError = (setGPM - actualGPM);
        if (abs(flowError) > (settings.deadbandError * setGPM))
        {
            flowError = constrain(flowError, setGPM * -1, setGPM);
            if (abs(flowError) < (settings.switchAtFlowError * setGPM))
            {
                flowError *= 0.1;
                NewPWM += flowError * gainPWM;
            }
            else
            {
                flowError *= 0.1;
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
