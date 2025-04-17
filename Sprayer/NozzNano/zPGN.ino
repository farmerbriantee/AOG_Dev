//Do we have a match with 0x8081?  

void DoSerialReceive(void)
{
    if (Serial.available() > 4 && !isHeaderFound && !isPGNFound)
    {
        uint8_t temp = Serial.read();
        if (tempHeader == 0x80 && temp == 0x81)
        {
            isHeaderFound = true;
            tempHeader = 0;
        }
        else
        {
            tempHeader = temp;     //save for next time
            return;
        }
    }

    //Find Source, PGN, and Length
    if (Serial.available() > 2 && isHeaderFound && !isPGNFound)
    {
        Serial.read(); //The 7F or less
        pgn = Serial.read();
        dataLength = Serial.read();
        isPGNFound = true;
        idx = 0;
    }

    //The data package
    if (Serial.available() > dataLength && isHeaderFound && isPGNFound)
    {
        if (pgn == 227) // Spray data
            {
                //do the percent calc first
                isr_flowCount = isr_flowCount + isr_flowCountThisLoop;
                isr_flowCountThisLoop = 0;

                //just 8 sections
                relayLo = Serial.read();          // read relay control from AgOpenGPS    
                relayHi = Serial.read();         // read relay control from AgOpenGPS    

                setGPM = (Serial.read() | Serial.read() << 8); //sent as x100

                sectionWidthPercent = (float)(Serial.read());

                if (settings.isBypass)
                {
                    sectionWidthPercent *= 0.01;
                }
                else
                {
                    sectionWidthPercent = 1;
                }

                speed = (float)(Serial.read());
                speed *= 0.1;

                //Bit 13 CRC
                Serial.read();

                //reset watchdog
                watchdogTimer = 0;

                //Reset serial Watchdog  
                serialResetTimer = 0;

                isHeaderFound = isPGNFound = false;
                pgn = dataLength = 0;

            }

        //else if (pgn == 200) // Hello from AgIO
        //{
        //    if (udpData[7] == 1)
        //    {
        //        relayLo -= 255;
        //        relayHi -= 255;
        //        watchdogTimer = 0;
        //    }

        //    helloFromMachine[5] = relayLo;
        //    helloFromMachine[6] = relayHi;

        //    udp.beginPacket(udpAddress, 9999);
        //    udp.write(helloFromMachine, sizeof(helloFromMachine));
        //    udp.endPacket();
        //}

        else if (pgn == 226) // Spray setttings (E2)   16 bytes
        {
            settings.flowCalFactor = (float)(Serial.read() | Serial.read() << 8);
            settings.flowCalFactor *= 0.1;

            settings.pressureCalFactor = (Serial.read() | Serial.read() << 8);
            settings.pressureCalFactor *= 0.1;

            //Kp and Ki not used
            settings.Kp = Serial.read();

            settings.Ki = Serial.read();
            settings.Ki = 1 / settings.Ki;

            settings.minPressurePSI = Serial.read();

            settings.fastPWM = Serial.read();
            settings.fastPWM = ((float)(settings.fastPWM)) * 2.5;

            settings.slowPWM = Serial.read();
            settings.slowPWM = ((float)(settings.slowPWM)) * 2.5;

            settings.deadbandError = (float)Serial.read();
            settings.deadbandError *= 0.01;

            settings.switchAtFlowError = (float)Serial.read();
            settings.switchAtFlowError *= 0.01;

            settings.isBypass = Serial.read();

            settings.is3Wire = Serial.read();

            //Bit CRC
            Serial.read();

            //store in EEPROM
            EEPROM.put(4, settings);

            //reset watchdog
            watchdogTimer = 0;

            //Reset serial Watchdog  
            serialResetTimer = 0;

            Serial.println("Spray Settings");

            isHeaderFound = isPGNFound = false;
            pgn = dataLength = 0;

        }

        else if (pgn == 225) // Spray Functions (E1)
        {
            zeroTankVolume = (Serial.read() | Serial.read() << 8);
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

            autoMode = Serial.read();

            manualUp = Serial.read();
            manualDn = Serial.read();

            settings.manualRate = Serial.read();
            settings.manualRate = ((float)(settings.manualRate)) * 2.5;

            if (manualUp)
            {
                manualCounter = 2;
                pwmDrive = settings.manualRate;
            }

            if (manualDn)
            {
                manualCounter = 2;
                pwmDrive = -settings.manualRate;
            }

            //Bit 13 CRC
            Serial.read();

            //reset watchdog
            watchdogTimer = 0;

            //Reset serial Watchdog  
            serialResetTimer = 0;

            Serial.println("Spray Functions");

            isHeaderFound = isPGNFound = false;
            pgn = dataLength = 0;

        }

        else //nothing found, clean up
        {
            isHeaderFound = isPGNFound = false;
            pgn = dataLength = 0;
        }
    }    
}
