﻿using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgOpenGPS.Classes
{
    public static class Lang
    {
        public static Dictionary<string, string> sub = new Dictionary<string, string>();

        public static void Load()
        {
            string fileAndDirectory = Path.Combine(Application.StartupPath, "Translations.csv");
            if (!File.Exists(fileAndDirectory))
            {
                return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        int column = 4;
                        //read header
                        string line = reader.ReadLine(); //header
                        string[] header = line.Split(',');
                        if (RegistrySettings.culture == "en") column = 4;
                        else
                        {
                            string cult = "." + RegistrySettings.culture;

                            for (int i = 0; i < header.Length; i++)
                            {
                                if (header[i] == cult)
                                {
                                    column = i;
                                    break;
                                }
                            }
                        }

                        while (line != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length > 2)
                            {
                                sub.Add(parts[2], parts[column]);
                            }
                            line = reader.ReadLine();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static string Get(string gstr)
        {
            string result;
            if (sub.TryGetValue(gstr, out result))
            {
                return result;
            }
            else
            {
                return "Fail";
            }
        }
    }
}
