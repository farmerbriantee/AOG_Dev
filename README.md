# AOG Development

Like AgOpenGPS but not.<br>
This version is constantly under development.<br>
Some differences:<br>
The fields and vehicles are **not** backwards compatible. They are stored in ```C:\Documents\AOG```, not ```C:\Documents\AgOpenGPS``` like previous versions were. 

Instead of one vehicle file where everything was stored, there is now a "User" file where the display settings are stored, regardless of which vehicle is used. The Vehicle and Tool are also separated, so you can have one vehicle per tractor, and a Tool for each implement. Fields are also changed, so you can have multiple Jobs per field, saving you from needing to create "Fields From Existing". The painted area is saved in the Job file, and can be re-opened whenever you want. 

AOG also supports the TM 171 IMU via USB. 

AOG also has builtin Rate Control.

AOG also has builtin Active Tool Steering.

AgIO natively supports GPS Out via serial bus, so the GPS_Out application is not required.

There are other minor differences, but these are the main ones.
 
