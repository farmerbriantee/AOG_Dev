void SetPWM(double PWM)
{
    if (PWM > 0)
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
  
  //Proportional
  flowError = (setGPM - actualGPM);      
  pwmDrive = (int16_t)(settings.Kp * flowError * 0.1);

  if ( abs(flowError) < settings.deadbandError) pwmDrive = 0;
  

/*
  //tolerance for integral to add
  uint8_t upper = 40;
  uint8_t lower = setGPM / 50; //1% for now

  if (abs(flowError) > lower && abs(flowError) < upper)
  {
      integral += flowError * (float)settings.Ki;
  }
  else //prevent windup
  {
    integral = 0;
  }

  //set a minimum value so it doesn't take forever to count up
  if (flowError > 0 && integral < 15) integral = 15;
  if (flowError < 0 && integral > -15) integral = -15;
  
  if (abs(flowError) < lower && abs(flowError) > -lower)
    integral = 0;

  //set a maximum integral value
  if (integral < -upper) integral = -upper;
  if (integral > upper) integral = upper;

  pwmDrive += integral;
  
  //We have switched direction so zero out integral
  if (flowError < lower && flowError > -lower)
  {
    pwmDrive = 0;
    integral = 0;
  }
  */

  //if the flow error is less then this, creep with slow pwm 80 = 0.8
  if (abs(flowError) < settings.switchAtFlowError)
    pwmDrive = (constrain(pwmDrive, -settings.slowPWM, settings.slowPWM));
  else
    pwmDrive = (constrain(pwmDrive, -settings.fastPWM, settings.fastPWM));
 
  SetPWM(pwmDrive);  
 }

 void DoManualPID(void) 
 {    
  SetPWM(pwmDrive);
 }
