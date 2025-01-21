using AgOne.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;

namespace AgOne
{
    public static class RegistrySettings
    {
        public static string culture = "en";

        public static string profileDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "AgOne");

        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");

        public static string profileName = "Default Profile";

        public static void Load()
        {
            try
            {
                try
                {
                    //create Logs directory if not exist
                    if (!string.IsNullOrEmpty(logsDirectory) && !Directory.Exists(logsDirectory))
                    {
                        Directory.CreateDirectory(logsDirectory);
                        Log.EventWriter("Logs Dir Created\r");
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch, Serious Problem Making Logs Directory: " + ex.ToString());
                }

                //keep below 500 kb
                Log.CheckLogSize(Path.Combine(logsDirectory, "AgOne_Events_Log.txt"), 500000);

                try
                {
                    //create Logs directory if not exist
                    if (!string.IsNullOrEmpty(profileDirectory) && !Directory.Exists(profileDirectory))
                    {
                        Directory.CreateDirectory(profileDirectory);
                        Log.EventWriter("Profile Dir Created\r");
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch, Serious Problem Making Profile Directory: " + ex.ToString());
                }

                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                ////create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    //Key.SetValue("Language", "en");
                    Key.SetValue("ProfileNameOne", "Default Profile");
                    Key.SetValue("Language", "en");
                    Key.Close();
                    Log.EventWriter("Registry -> SubKey AgOne and Keys Created\r");
                }
                else
                {
                    try
                    {
                        //Culture from Registry Key
                        if (regKey.GetValue("Language") == null || regKey.GetValue("Language").ToString() == "")
                        {
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                            key.SetValue("Language", "en");
                            Log.EventWriter("Registry -> Culture was null and Created");
                        }
                        else
                        {
                            culture = regKey.GetValue("Language").ToString();
                        }

                        //Profile File Name from Registry Key
                        if (regKey.GetValue("ProfileNameOne") == null || regKey.GetValue("ProfileNameOne").ToString() == "")
                        {
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                            key.SetValue("ProfileNameOne", "Default Profile");
                            Log.EventWriter("Registry -> Key Profile Name was null and Created");
                        }
                        else
                        {
                            profileName = regKey.GetValue("ProfileNameOne").ToString();

                            //get the Documents directory, if not exist, create
                            profileDirectory =
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "AgOne");

                            if (!string.IsNullOrEmpty(profileDirectory) && !Directory.Exists(profileDirectory))
                            {
                                Directory.CreateDirectory(profileDirectory);
                            }

                            //what's in the vehicle directory
                            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.profileDirectory);
                            FileInfo[] vehicleFiles = dinfo.GetFiles("*.xml");

                            bool isProfileExist = false;

                            foreach (FileInfo file in vehicleFiles)
                            {
                                string temp = Path.GetFileNameWithoutExtension(file.Name).Trim();

                                if (temp == profileName)
                                {
                                    isProfileExist = true;
                                }
                            }

                            //does current vehicle exist?
                            if (isProfileExist && profileName != "Default Profile")
                            {
                                SettingsIO.ImportSettings(Path.Combine(profileDirectory, profileName + ".XML"));
                                Log.EventWriter("Registry -> " + profileName + ".XML Profile Loaded");
                            }
                            else
                            {
                                Log.EventWriter("Registry -> " + profileName + ".XML Profile does not exist. Called in Program.cs");
                                profileName = "Default Profile";
                                Save();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter("Registry -> Catch, Serious Problem Loading Profile, Doing Registry Reset: " + ex.ToString());
                        Reset();

                        //reset to Default Profile and save
                        Settings.Default.Reset();
                        Save();
                    }
                    regKey.Close();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Creating Registry keys: " + ex.ToString());
                Reset();
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            try
            {
                key.SetValue("ProfileNameOne", profileName);
                Log.EventWriter(profileName + " Saved to registry key");
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Saving keys: " + ex.ToString());
            }
            key.Close();

            if (RegistrySettings.profileName != "Default Profile")
            {
                Thread.Sleep(500);
                SettingsIO.ExportSettings(Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".xml"));
            }
        }

        public static void Reset()
        {
            Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgOpenGPS");

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            try
            {
                key.SetValue("ProfileNameOne", "Default Profile");
                key.SetValue("Language", "en");
                Log.EventWriter("Registry -> Resetting Registry keys");
            }
            catch (Exception ex)
            {
                Log.EventWriter("\"Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());
            }
            key.Close();
        }
    }
}