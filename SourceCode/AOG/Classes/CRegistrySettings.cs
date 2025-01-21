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
        public static string vehicleFileName = "Default Vehicle";
        public static string workingDirectory = "Default";
        public static string baseDirectory = workingDirectory;
        public static string fieldsDirectory = workingDirectory;

        public static void Load()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                ////create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("VehicleFileName", "Default Vehicle");
                    Key.SetValue("WorkingDirectory", "Default");
                    Key.Close();
                }

                //Base Directory Registry Key
                regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                if (regKey.GetValue("WorkingDirectory") == null || regKey.GetValue("WorkingDirectory").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("WorkingDirectory", "Default");
                    key.Close();
                }
                workingDirectory = regKey.GetValue("WorkingDirectory").ToString();

                if (workingDirectory == "Default")
                {
                    baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
                }
                else //user set to other
                {
                    baseDirectory = Path.Combine(workingDirectory, "AgOpenGPS");
                }

                //Vehicle File Name Registry Key
                if (regKey.GetValue("VehicleFileName") == null || regKey.GetValue("VehicleFileName").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("VehicleFileName", "Default Vehicle");
                    key.Close();
                }

                vehicleFileName = regKey.GetValue("VehicleFileName").ToString();

                //Language Registry Key
                if (regKey.GetValue("Language") == null || regKey.GetValue("Language").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("Language", "en");
                    key.Close();
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

            //what's in the vehicle directory
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(vehiclesDirectory);
                FileInfo[] vehicleFiles = dinfo.GetFiles("*.xml");

                bool isVehicleExist = false;

                foreach (FileInfo file in vehicleFiles)
                {
                    string temp = Path.GetFileNameWithoutExtension(file.Name).Trim();

                    if (temp == vehicleFileName)
                    {
                        isVehicleExist = true;
                    }
                }

                //does current vehicle exist?
                if (isVehicleExist && vehicleFileName != "Default Vehicle")
                {
                    SettingsIO.ImportAll(Path.Combine(RegistrySettings.vehiclesDirectory, vehicleFileName + ".XML"));
                }
                else
                {
                    vehicleFileName = "Default Vehicle";
                    Log.EventWriter("Vehicle file does not exist or is Default, Default Vehicle selected");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Loading Vehicle, Doing Registry Reset: " + ex.ToString());
                Reset();

                //reset to Default Vehicle and save
                Settings.Default.Reset();
                Settings.Default.Save();
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            try
            {
                key.SetValue("VehicleFileName", vehicleFileName);
                key.SetValue("Language", culture);
                key.SetValue("WorkingDirectory", workingDirectory);

                Log.EventWriter(vehicleFileName + " Saved to registry key");
            }
            catch (Exception ex)
            {
                Thread.Sleep(500);
                Log.EventWriter("Registry -> Catch, Serious Problem Saving keys: " + ex.ToString());
            }
            key.Close();

            try
            {
                if (vehicleFileName != "Default Vehicle")
                {
                    Thread.Sleep(500);
                    SettingsIO.ExportAll(Path.Combine(vehiclesDirectory, vehicleFileName + ".xml"));
                }
                else
                {
                    Log.EventWriter("Default Vehicle Not saved to Vehicles");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Unable to save Vehicle FileName: " + ex.ToString());
            }
        }

        public static void Reset()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgOpenGPS");

                //create all new key
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
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