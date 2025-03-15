using AgIO.Properties;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;

namespace AgIO
{
    public static class RegistrySettings
    {
        public static string culture = "en";

        public static string profilesDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG", "AgIO");

        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AOG", "Logs");

        public static string profileName = "";
        public static string workingDirectory = "Default";
        public static string baseDirectory = workingDirectory;
        public static string profileDirectory = workingDirectory;

        public static void Load()
        {
            try
            {
                //Base Directory Registry Key
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");

                workingDirectory = regKey.GetValue("WorkingDirectory", "Default").ToString();

                //Vehicle File Name Registry Key
                profileName = regKey.GetValue("ProfileName", "Default").ToString();

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
            Log.CheckLogSize(Path.Combine(logsDirectory, "AgIO_Events_Log.txt"), 500000);

            Settings.User.Load();
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");

                if (name == "ProfileName")
                    profileName = value;
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
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgIO");

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
                baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgIO");
            }
            else //user set to other
            {
                baseDirectory = Path.Combine(workingDirectory, "AgIO");
            }

            //get the profiles directory, if not exist, create
            try
            {
                profileDirectory = Path.Combine(baseDirectory, "Profiles");
                if (!string.IsNullOrEmpty(profileDirectory) && !Directory.Exists(profileDirectory))
                {
                    Directory.CreateDirectory(profileDirectory);
                    Log.EventWriter("Profiles Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Profiles Directory: " + ex.ToString());
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