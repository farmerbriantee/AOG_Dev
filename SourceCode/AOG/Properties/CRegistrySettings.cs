using Microsoft.Win32;
using System;
using System.IO;

namespace AOG
{
    public static class RegistrySettings
    {
        public static string culture = "en";

        public static string vehiclesDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG", "Vehicles");

        public static string toolsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG", "Tools");

        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG", "Logs");

        public static string vehicleFileName = "";
        public static string toolFileName = "";
        public static string workingDirectory = "Default";
        public static string baseDirectory = workingDirectory;
        public static string fieldsDirectory = workingDirectory;

        public static void Load()
        {
            try
            {
                //Base Directory Registry Key
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AOG");

                workingDirectory = regKey.GetValue("WorkingDirectory", "Default").ToString();

                //Vehicle File Name Registry Key
                vehicleFileName = regKey.GetValue("VehicleFileName", "Default").ToString();

                //Tool File Name Registry Key
                toolFileName = regKey.GetValue("ToolFileName", "Default").ToString();

                //Language Registry Key
                culture = regKey.GetValue("Language", "en").ToString();
                if (culture == "") culture = "en";

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
            Log.CheckLogSize(Path.Combine(logsDirectory, "AOG_Events_Log.txt"), 500000);

            Settings.User.Load();
            Settings.Vehicle.Load();
            Settings.Tool.Load();
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AOG");

                if (name == "VehicleFileName")
                    vehicleFileName = value;
                else if (name == "ToolFileName")
                    toolFileName = value;
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

                Load();
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
                baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG");
            }
            else //user set to other
            {
                baseDirectory = Path.Combine(workingDirectory, "AOG");
            }

            //get the interface directory, if not exist, create
            //try
            //{
            //    userDirectory = Path.Combine(baseDirectory);
            //    if (!string.IsNullOrEmpty(userDirectory) && !Directory.Exists(userDirectory))
            //    {
            //        Directory.CreateDirectory(userDirectory);
            //        Log.EventWriter("Interfaces Dir Created");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.EventWriter("Catch, Serious Problem Making Interfaces Directory: " + ex.ToString());
            //}

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

            //get the Tools directory, if not exist, create
            try
            {
                toolsDirectory = Path.Combine(baseDirectory, "Tools");
                if (!string.IsNullOrEmpty(toolsDirectory) && !Directory.Exists(toolsDirectory))
                {
                    Directory.CreateDirectory(toolsDirectory);
                    Log.EventWriter("Tools Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Tools Directory: " + ex.ToString());
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