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

namespace AgOpenGPS.Properties
{
    public sealed class ToolSettings
    {
        private static ToolSettings toolSettings_ = new ToolSettings();
        public static ToolSettings Default
        {
            get
            {
                return toolSettings_;
            }
        }

        public CNozzleSettings setNozzleSettings = new CNozzleSettings();
        public CToolSteerSettings setToolSteer = new CToolSteerSettings();

        public bool setApp_isNozzleApp = false;
        public bool setApPGN_isNozzleApp = false;

        public double setSection_position1 = -2;
        public double setSection_position10 = 0;
        public double setSection_position11 = 0;
        public double setSection_position12 = 0;
        public double setSection_position13 = 0;
        public double setSection_position14 = 0;
        public double setSection_position15 = 0;
        public double setSection_position16 = 0;
        public double setSection_position17 = 0;
        public double setSection_position2 = -1;
        public double setSection_position3 = 1;
        public double setSection_position4 = 2;
        public double setSection_position5 = 0;
        public double setSection_position6 = 0;
        public double setSection_position7 = 0;
        public double setSection_position8 = 0;
        public double setSection_position9 = 0;
        public bool setSection_isFast = true;


        public bool setTool_isDirectionMarkers = true;
        public bool setTool_isDisplayTramControl = true;
        public bool setTool_isSectionOffWhenOut = true;
        public bool setTool_isSectionsNotZones = true;
        public bool setTool_isToolFront = false;
        public bool setTool_isToolRearFixed = false;
        public bool setTool_isToolTBT = false;
        public bool setTool_isToolTrailing = true;
        public bool setTool_isTramOuterInverted = false;
        public int setTool_numSectionsMulti = 20;
        public string setTool_zones = "2,10,20,0,0,0,0,0,0";
        public double setTool_defaultSectionWidth = 2;
        public double setTool_sectionWidthMulti = 0.5;
        public double setTool_toolTrailingHitchLength = -2.5;
        public double setTool_trailingToolToPivotLength = 0;

        public bool setTram_isTramOnBackBuffer = true;
        public double setTram_alpha = 0.8;
        public double setTram_offset = 0.0;
        public double setTram_snapAdj = 1.0;
        public double setTram_tramWidth = 24.0;
        public int setTram_BasedOn = 0;
        public int setTram_passes = 1;
        public int setTram_Skips = 0;


        public double setVehicle_hitchLength = -1;
        public double setVehicle_hydraulicLiftLookAhead = 2;
        public double setVehicle_lookAheadMinimum = 2;
        public double setVehicle_slowSpeedCutoff = 0.5;
        public double setVehicle_tankTrailingHitchLength = 3;
        public double setVehicle_toolLookAheadOff = 0.5;
        public double setVehicle_toolLookAheadOn = 1;
        public double setVehicle_toolOffDelay = 0;
        public double setVehicle_toolOffset = 0;
        public double setVehicle_toolOverlap = 0.0;
        public double setVehicle_toolWidth = 4.0;
        public int setVehicle_minCoverage = 100;
        public int setVehicle_numSections = 3;

        public LoadResult Load()
        {
            string path = Path.Combine(RegistrySettings.toolsDirectory, RegistrySettings.toolFileName + ".XML");
            var result = XmlSettingsHandler.LoadXMLFile(path, this);
            if (result == LoadResult.MissingFile)
            {
                Log.EventWriter("Tool file does not exist or is Default, Default Vehicle used");
                RegistrySettings.Save("ToolFileName", "");
            }
            else if (result == LoadResult.Failed)
            {
                Log.EventWriter("Tool Loaded With Error:" + result.ToString());
            }

            return result;
        }

        public void Save()
        {
            string path = Path.Combine(RegistrySettings.toolsDirectory, RegistrySettings.toolFileName + ".XML");

            if (RegistrySettings.toolFileName != "")
                XmlSettingsHandler.SaveXMLFile(path, this);
            else
                Log.EventWriter("Default Tool Not saved to Tools");
        }

        public void Reset()
        {
            toolSettings_ = new ToolSettings();
            toolSettings_.Save();
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

            public static void SaveXMLFile(string filePath, object obj)
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
                        xml.WriteStartElement("configuration");
                        xml.WriteStartElement("userSettings");
                        xml.WriteStartElement(obj.ToString());

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
                        xml.WriteEndElement(); // AgOpenGPS.Properties.Settings
                        xml.WriteEndElement(); // userSettings
                        xml.WriteEndElement(); // configuration

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
