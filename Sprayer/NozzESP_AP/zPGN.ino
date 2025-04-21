//Do we have a match with 0x8081?  

void UDP_Receive(void)
{
    uint16_t len = udp.parsePacket();
    if (len > 4)
    {
        udp.read(udpData, maxReadBuffer);

        //The data package
        if (udpData[0] == 0x80 && udpData[1] == 0x81 && udpData[2] == 0x7F) //Data
        {
            if (udpData[3] == 227) // Spray data
            {
                //do the percent calc first
                isr_flowCount = isr_flowCount + isr_flowCountThisLoop;
                isr_flowCountThisLoop = 0;

                //just 8 sections
                relayLo = udpData[5];          // read relay control from AgOpenGPS    
                relayHi = udpData[6];         // read relay control from AgOpenGPS    

                setGPM = (udpData[7] | udpData[8] << 8); //sent as x100

                if (settings.isBypass)
                {
                    sectionWidthPercent = (float)(udpData[9]);
                    sectionWidthPercent *= 0.01;
                }
                else
                {
                    sectionWidthPercent = 1;
                }

                speed = (float)(udpData[10]);
                speed *= 0.1;

                //reset watchdog
                watchdogTimer = 0;

                //Reset serial Watchdog  
                serialResetTimer = 0;
            }

            else if (udpData[3] == 200) // Hello from AgIO
            {
                if (udpData[7] == 1)
                {
                    relayLo -= 255;
                    relayHi -= 255;
                    watchdogTimer = 0;
                }

                helloFromMachine[5] = relayLo;
                helloFromMachine[6] = relayHi;

                udp.beginPacket(udpAddress, 9999);
                udp.write(helloFromMachine, sizeof(helloFromMachine));
                udp.endPacket();
            }

            else if (udpData[3] == 226) // Spray setttings (E2)   16 bytes
            {
                settings.flowCalFactor = (float)(udpData[5] | udpData[6] << 8);
                settings.flowCalFactor *= 0.1;

                settings.pressureCalFactor = (udpData[7] | udpData[8] << 8);
                settings.pressureCalFactor *= 0.1;

                //Kp setting goes from 1 to 100
                settings.Kp = udpData[9];
                settings.Kp *= 0.01;

				//Ki goes from 0.01 to 1
                settings.Ki = udpData[10];
                settings.Ki = 1 / settings.Ki;

                settings.minPressurePSI = udpData[11];

                settings.fastPWM = udpData[12];
                settings.fastPWM = ((float)(settings.fastPWM)) * 2.5;

                settings.slowPWM = udpData[13];
                settings.slowPWM = ((float)(settings.slowPWM)) * 2.5;

                settings.deadbandError = udpData[14];
                settings.deadbandError *= 0.01;

                settings.switchAtFlowError = udpData[15];
                settings.switchAtFlowError *= 0.01;

                settings.isBypass = udpData[16] & 1;
                settings.isMeter = udpData[16] & 2;

                settings.is3Wire = udpData[17];

                //store in EEPROM
                EEPROM.put(4, settings);
                EEPROM.commit();

                //reset watchdog
                watchdogTimer = 0;

                //Reset serial Watchdog  
                serialResetTimer = 0;

                Serial.println("Spray Settings");
            }

            else if (udpData[3] == 225) // Spray Functions (E1)
            {
                zeroTankVolume = (udpData[5] | udpData[6] << 8);
                if (zeroTankVolume != 0)
                {
                    if (zeroTankVolume == 1)
                    {
                        isr_flowCount = 0;
                    }
                    else
                    {
                        isr_flowCount = zeroTankVolume * settings.flowCalFactor;
                    }
                }

                autoMode = udpData[7];

                settings.manualRate = udpData[10];
                settings.manualRate = ((float)(settings.manualRate)) * 2.5;

                manualUp = udpData[8];
                if (manualUp)
                {
                    manualCounter = 2;
                    pwmDrive = settings.manualRate;
                }

                manualDn = udpData[9];
                if (manualDn)
                {
                    manualCounter = 2;
                    pwmDrive = -settings.manualRate;
                }

                //reset watchdog
                watchdogTimer = 0;

                //Reset serial Watchdog  
                serialResetTimer = 0;

                Serial.println("Spray Functions");
            }
        }
    }
}
