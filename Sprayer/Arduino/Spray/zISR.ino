 //measure the width of flow pulse.

  void isr_Flow()
  {
      isr_currentTime = micros();
      isr_flowTime = isr_currentTime - isr_lastTime;
      isr_lastTime = isr_currentTime;

      //either way too long or way too short
      if (isr_flowTime > 300000UL || isr_flowTime < 3000UL)
      {
        isr_lastTime = isr_currentTime;
        isr_isFlowing = 0;
        return;
      }

      //counts for total volume
      isr_flowCountThisLoop +=1;
      isr_isFlowing = 1; 

      //use ring counter
      isr_flowTimeArr[ringPos] = isr_flowTime;
      if (ringPos > 15) ringPos = 0;
      isr_flowTime = (isr_flowTimeArr[0]+isr_flowTimeArr[1]+isr_flowTimeArr[2]+isr_flowTimeArr[3]+
                      isr_flowTimeArr[4]+isr_flowTimeArr[5]+isr_flowTimeArr[6]+isr_flowTimeArr[7]+
                      isr_flowTimeArr[8]+isr_flowTimeArr[9]+isr_flowTimeArr[10]+isr_flowTimeArr[11]+
                      isr_flowTimeArr[12]+isr_flowTimeArr[13]+isr_flowTimeArr[14]+isr_flowTimeArr[15]) >> 4;
      
  }
