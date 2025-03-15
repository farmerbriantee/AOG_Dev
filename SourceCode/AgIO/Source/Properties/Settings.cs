using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections;

namespace AgIO
{
    public sealed class Settings
    {

        private static UserSettings user_ = new UserSettings();
        public static UserSettings User
        {
            get
            {
                return user_;
            }
        }

        public sealed class UserSettings
        {
            public string setPort_portNameGPS = "GPS**";
            public string setPort_portNameTool = "Tool*";
            public string setPort_portNameSteer = "Steer*";
            public string setPort_portNameMachine = "Mach**";
            public string setPort_portNameGPS2 = "GPS2";
            public string setPort_portNameRtcm = "RTCM";
            public string setPort_portNameIMU = "IMU*";

            public int setPort_baudRateGPS = 9600;
            public int setPort_baudRateRtcm = 9600;
            public int setPort_baudRateGPS2 = 9600;
            
            public bool setPort_wasMachineModuleConnected = false;
            public bool setPort_wasIMUConnected = false;
            public bool setPort_wasSteerModuleConnected = false;
            public bool setPort_wasModule3Connected = false;
            public bool setPort_wasRtcmConnected = false;
            public bool setPort_wasGPSConnected = false;

            public bool setMod_isIMUConnected = true;
            public bool setMod_isMachineConnected = true;
            public bool setMod_isSteerConnected = true;
            
            public string setPort_portNameGPSOut = "Out";
            public int setPort_baudRateGPSOut = 9600;
            public bool setMod_isGPSOutConnected = true;
            public bool setPort_wasGPSOutConnected = false;
            
            public int sendRateGGA = 0;
            public int sendRateVTG = 0;
            public int sendRateRMC = 0;
            public int sendRateZDA = 0;
            public int sendRateGSA = 0;

            public string sendPrefixGPGN = "$GP";

            public string setNTRIP_casterIP = "69.75.31.235";
            public string setNTRIP_casterURL = "NTRIP.itsware.net";
            public string setNTRIP_mount = "SCSC";
            public string setNTRIP_userName = "";
            public string setNTRIP_userPassword = "";

            public int setNTRIP_casterPort = 2101;
            public int setNTRIP_sendGGAInterval = 10;
            public int setNTRIP_sendToUDPPort = 2233;
            public int setNTRIP_packetSize = 256;

            public double setNTRIP_manualLat = 53;
            public double setNTRIP_manualLon = -111;

            public bool setNTRIP_isOn = false;
            public bool setNTRIP_isGGAManual = false;
            public bool setNTRIP_isTCP = false;
            public bool setNTRIP_isHTTP10 = false;
            public bool setNTRIP_sendToSerial = true;
            public bool setNTRIP_sendToUDP = true;
            public bool setPass_isOn = false;

            public bool setUDP_isOn = false;
            public byte etIP_SubnetOne = 192;
            public byte etIP_SubnetTwo = 168;
            public byte etIP_SubnetThree = 5;

            public byte eth_loopOne = 127;
            public byte eth_loopTwo = 255;
            public byte eth_loopThree = 255;
            public byte eth_loopFour = 255;

            public bool setDisplay_isAutoRunGPS_Out = false;

            public List<CRadioChannel> setRadio_Channels = new List<CRadioChannel>();

            public bool setRadio_isOn = false;
            public string setPort_baudRateRadio = "9600";
            public string setPort_portNameRadio = "***";
            public string setPort_radioChannel = "439.000";

            public LoadResult Load()
            {
                string path = Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".XML");
                var result = XmlSettingsHandler.LoadXMLFile(path, this);
                if (result == LoadResult.MissingFile)
                {
                    Log.EventWriter("User file does not exist or is Default, Default Interface used");
                }
                else if (result == LoadResult.Failed)
                {
                    Log.EventWriter("User XML Loaded With Error:" + result.ToString());
                }

                return result;
            }

            public void Save()
            {
                string path = Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".XML");

                XmlSettingsHandler.SaveXMLFile(path, "User", this);
            }

            public void Reset()
            {
                user_ = new UserSettings();
                user_.Save();
            }
        }

        public enum LoadResult { Ok, MissingFile, Failed };

        public static class XmlSettingsHandler
        {
            public static LoadResult LoadXMLFile(string filePath, object obj)
            {
                bool Errors = false;
                try
                {
                    if (!File.Exists(filePath))
                    {
                        return LoadResult.MissingFile;
                    }
                    using (XmlTextReader reader = new XmlTextReader(filePath))
                    {
                        string name = "";
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (reader.Name == "setting")
                                    {
                                        name = reader.GetAttribute("name");
                                    }
                                    else if (reader.Name == "value")
                                    {
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            var pinfo = obj.GetType().GetField(name);
                                            if (pinfo != null)
                                            {
                                                try
                                                {
                                                    SetFieldValue(pinfo, reader, obj);
                                                }
                                                catch (Exception)
                                                {
                                                    if (Debugger.IsAttached)
                                                        throw;// Re-throws the original exception
                                                    Errors = true;
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case XmlNodeType.EndElement:
                                    break;
                            }
                        }
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    if (Debugger.IsAttached)
                        throw;// Re-throws the original exception
                    Errors = true;
                }
                return Errors ? LoadResult.Failed : LoadResult.Ok;
            }

            private static bool SetFieldValue(FieldInfo pinfo, XmlTextReader reader, object obj)
            {
                Type fieldType = pinfo.FieldType;
                // Read string values
                string value = reader.ReadString();

                if (fieldType == typeof(string))
                {
                    pinfo.SetValue(obj, value);
                }
                else if (fieldType.IsEnum) // Handle Enums
                {
                    var enumValue = Enum.Parse(fieldType, value, ignoreCase: true);
                    pinfo.SetValue(obj, enumValue);
                }
                else if (fieldType.IsPrimitive || fieldType == typeof(decimal))
                {
                    object parsedValue = Convert.ChangeType(value, fieldType, CultureInfo.InvariantCulture);
                    pinfo.SetValue(obj, parsedValue);
                }
                else if (fieldType == typeof(Color))
                {
                    var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 3 && parseInvariantCulture(parts[0], out int r) && parseInvariantCulture(parts[1], out int g) && parseInvariantCulture(parts[2], out int b))
                    {
                        pinfo.SetValue(obj, Color.FromArgb(r, g, b));
                    }
                }
                else if (fieldType == typeof(Point))
                {
                    var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && parseInvariantCulture(parts[0], out int x) && parseInvariantCulture(parts[1], out int y))
                    {
                        pinfo.SetValue(obj, new Point(x, y));
                    }
                }
                else if (fieldType == typeof(Size))
                {
                    var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && parseInvariantCulture(parts[0], out int width) && parseInvariantCulture(parts[1], out int height))
                    {
                        pinfo.SetValue(obj, new Size(width, height));
                    }
                }
                else if (typeof(IEnumerable).IsAssignableFrom(fieldType) && (fieldType.IsGenericType || fieldType.IsArray))
                {
                    Type itemType;

                    if (fieldType.IsGenericType) // For generic collections like List<T>
                        itemType = fieldType.GetGenericArguments()[0];
                    else if (fieldType.IsArray) // For arrays like T[]
                        itemType = fieldType.GetElementType();
                    else
                    {
                        throw new NotSupportedException($"Unsupported collection type: {fieldType}");
                    }

                    // Deserialize XML into the custom object
                    var serializer = new XmlSerializer(typeof(List<>).MakeGenericType(itemType));
                    var list = serializer.Deserialize(reader);

                    if (fieldType.IsArray) // Convert List<T> to T[] for arrays
                    {
                        var array = ((IEnumerable)list).Cast<object>().ToArray();
                        pinfo.SetValue(obj, Array.CreateInstance(itemType, array.Length));
                        Array.Copy(array, (Array)pinfo.GetValue(obj), array.Length);
                    }
                    else // Directly assign the List<T>
                    {
                        pinfo.SetValue(obj, list);
                    }
                }
                else if (fieldType.IsClass)
                {
                    // Deserialize XML into the custom object
                    var serializer = new XmlSerializer(fieldType);
                    var nestedObj = serializer.Deserialize(reader);
                    pinfo.SetValue(obj, nestedObj);
                }
                else
                {
                    if (Debugger.IsAttached)
                        throw new ArgumentException("type not found");
                    return false;
                }
                return true;
            }

            private static bool parseInvariantCulture(string value, out int outValue)
            {
                return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out outValue);
            }

            public static void SaveXMLFile(string filePath, string element, object obj)
            {
                try
                {
                    var dirName = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(dirName);
                    }

                    using (XmlTextWriter xml = new XmlTextWriter(filePath + ".tmp", Encoding.UTF8)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4
                    })
                    {
                        xml.WriteStartDocument();

                        // Start the root element
                        xml.WriteStartElement(element);

                        foreach (var fld in obj.GetType().GetFields())
                        {
                            var value = fld.GetValue(obj);
                            var fieldType = value.GetType();

                            // Start a "setting" element
                            xml.WriteStartElement("setting");

                            // Add attributes to the "setting" element
                            xml.WriteAttributeString("name", fld.Name);

                            if ((fieldType.IsClass && fieldType != typeof(string)) || (typeof(IEnumerable).IsAssignableFrom(fieldType) && (fieldType.IsGenericType || fieldType.IsArray)))
                            {
                                //classes, arrays and lists
                                xml.WriteAttributeString("serializeAs", "Xml");

                                // Write the serialized object to a nested "value" element
                                xml.WriteStartElement("value");

                                var serializer = new XmlSerializer(fieldType);
                                serializer.Serialize(xml, value);

                                xml.WriteEndElement(); // value
                            }
                            else
                            {
                                xml.WriteAttributeString("serializeAs", "String");

                                if (value is Point pointValue)
                                {
                                    xml.WriteElementString("value", $"{pointValue.X.ToString(CultureInfo.InvariantCulture)}, {pointValue.Y.ToString(CultureInfo.InvariantCulture)}");
                                }
                                else if (value is Size sizeValue)
                                {
                                    xml.WriteElementString("value", $"{sizeValue.Width.ToString(CultureInfo.InvariantCulture)}, {sizeValue.Height.ToString(CultureInfo.InvariantCulture)}");
                                }
                                else if (value is Color dd)
                                {
                                    xml.WriteElementString("value", $"{dd.R.ToString(CultureInfo.InvariantCulture)}, {dd.G.ToString(CultureInfo.InvariantCulture)}, {dd.B.ToString(CultureInfo.InvariantCulture)}");
                                }
                                else
                                {
                                    // Write primitive types or strings
                                    string stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                                    xml.WriteElementString("value", stringValue);
                                }
                            }

                            xml.WriteEndElement(); // setting
                        }

                        // Close all open elements
                        xml.WriteEndElement(); //element

                        // End the document
                        xml.WriteEndDocument();
                        xml.Flush();
                    }

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    if (File.Exists(filePath + ".tmp"))
                        File.Move(filePath + ".tmp", filePath);
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Exception saving XML file" + ex.ToString());
                }
            }
        }
    }
}