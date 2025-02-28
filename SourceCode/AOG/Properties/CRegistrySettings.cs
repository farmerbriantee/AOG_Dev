using AgOpenGPS.Classes;
using AgOpenGPS.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;

namespace AgOpenGPS
{
    public static class RegistrySettings
    {
        public static string culture = "en";

        public static string vehiclesDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Vehicles");

        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");

        public static string vehicleFileName = "";
        public static string workingDirectory = "";
        public static string baseDirectory = workingDirectory;
        public static string fieldsDirectory = workingDirectory;

        public static void Load()
        {
            try
            {   
                //Base Directory Registry Key
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AOG");

                if (regKey.GetValue("WorkingDirectory") == null)
                {
                    regKey.SetValue("WorkingDirectory", "");
                }
                workingDirectory = regKey.GetValue("WorkingDirectory").ToString();

                //Vehicle File Name Registry Key
                if (regKey.GetValue("VehicleFileName") == null)
                {
                    regKey.SetValue("VehicleFileName", "");
                }
                vehicleFileName = regKey.GetValue("VehicleFileName").ToString();

                //Language Registry Key
                if (regKey.GetValue("Language") == null || regKey.GetValue("Language").ToString() == "")
                {
                    regKey.SetValue("Language", "en");
                }
                culture = regKey.GetValue("Language").ToString();

                //close registry
                regKey.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Creating Registry keys: " + ex.ToString());
                Reset();
            }

            //make sure directories exist and are in right place if not default workingDir
            CreateDirectories();

            //keep below 500 kb
            Log.CheckLogSize(Path.Combine(logsDirectory, "AgOpenGPS_Events_Log.txt"), 500000);



            Settings.Default.Load();
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey  
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AOG");

                if (name == "VehicleFileName")
                    vehicleFileName = value;
                else if (name == "Language")
                    culture = value;

                if (name == "WorkingDirectory" && value == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                {
                    key.SetValue(name, "");
                    Log.EventWriter("Registry -> Key " + name + " Saved to registry key with value: ");
                }
                else//storing the value
                {
                    key.SetValue(name, value);
                    Log.EventWriter("Registry -> Key " + name + " Saved to registry key with value: " + value);
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Unable to save " + name + ": " + ex.ToString());
            }
        }

        public static void Reset()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AOG");

                //create all new key
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AOG");
                key.SetValue("Language", "en");
                key.SetValue("VehicleFileName", "Default Vehicle");
                key.SetValue("WorkingDirectory", "Default");
                key.Close();

                Log.EventWriter("Registry -> Resetting Registry SubKey Tree and Full Default Reset");

                culture = "en";
                vehiclesDirectory =
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Vehicles");
                logsDirectory =
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");
                vehicleFileName = "Default Vehicle";
                workingDirectory = "Default";
                baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
                fieldsDirectory = Path.Combine(baseDirectory, "Fields");

                CreateDirectories();
            }
            catch (Exception ex)
            {
                Log.EventWriter("\"Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());
            }
        }

        public static void CreateDirectories()
        {
            if (workingDirectory == "Default" || workingDirectory == "")
            {
                baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
            }
            else //user set to other
            {
                baseDirectory = Path.Combine(workingDirectory, "AgOpenGPS");
            }


            //get the vehicles directory, if not exist, create
            try
            {
                vehiclesDirectory = Path.Combine(baseDirectory, "Vehicles");
                if (!string.IsNullOrEmpty(vehiclesDirectory) && !Directory.Exists(vehiclesDirectory))
                {
                    Directory.CreateDirectory(vehiclesDirectory);
                    Log.EventWriter("Vehicles Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Vehicles Directory: " + ex.ToString());
            }

            //get the fields directory, if not exist, create
            try
            {
                fieldsDirectory = Path.Combine(baseDirectory, "Fields");
                if (!string.IsNullOrEmpty(fieldsDirectory) && !Directory.Exists(fieldsDirectory))
                {
                    Directory.CreateDirectory(fieldsDirectory);
                    Log.EventWriter("Fields Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Fields Directory: " + ex.ToString());
            }

            //get the logs directory, if not exist, create
            try
            {
                if (!string.IsNullOrEmpty(logsDirectory) && !Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                    Log.EventWriter("Logs Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Logs Directory: " + ex.ToString());
            }
        }
    }
}