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

namespace AOG
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

        private static VehicleSettings vehicle_ = new VehicleSettings();
        public static VehicleSettings Vehicle
        {
            get
            {
                return vehicle_;
            }
        }

        private static ToolSettings tool_ = new ToolSettings();
        public static ToolSettings Tool
        {
            get
            {
                return tool_;
            }
        }

        public sealed class UserSettings
        {
            public CFeatureSettings setFeatures = new CFeatureSettings();

            public double setDisplay_camPitch = -62;
            public double setDisplay_camZoom = 9.0;
            public bool setDisplay_isBrightnessOn = false;
            public bool setDisplay_isDayMode = true;
            public bool setDisplay_isHardwareMessages = false;
            public bool setDisplay_isKeyboardOn = true;
            public bool setDisplay_isLineSmooth = false;
            public bool setDisplay_isSectionLinesOn = true;
            public bool setDisplay_isShutdownWhenNoPower = false;
            public bool setDisplay_isStartFullScreen = false;
            public bool setDisplay_isSvennArrowOn = false;
            public bool setDisplay_isTextureOn = true;

            public int setDisplay_brightness = 40;
            public int setDisplay_brightnessSystem = 40;
            public int setDisplay_camSmooth = 50;
            public int setDisplay_lightbarCmPerPixel = 5;
            public int setDisplay_lineWidth = 2;
            public string setDisplay_buttonOrder = "0,1,2,3,4,5,6";
            public string setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";
            public string setDisplay_customSectionColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";

            public bool setWindow_isKioskMode = false;
            public bool setWindow_isShutdownComputer = false;
            public bool setWindow_Maximized = false;
            public bool setWindow_Minimized = false;
            public int setWindow_BingZoom = 15;
            public int setWindow_RateMapZoom = 15;

            public Point setWindow_buildTracksLocation = new Point(40, 40);
            public Point setWindow_formNudgeLocation = new Point(200, 200);
            public Point setWindow_gridLocation = new Point(20, 20);
            public Point setWindow_JobMenulocation = new Point(200, 200);
            public Point setWindow_Location = new Point(30, 30);
            public Point setWindow_QuickABLocation = new Point(100, 100);
            public Point setWindow_steerSettingsLocation = new Point(40, 40);
            public Point setWindow_FieldMenulocation = new Point(200, 200);


            public Size setWindow_abDrawSize = new Size(1022, 742);
            public Size setWindow_BingMapSize = new Size(965, 700);
            public Size setWindow_FieldMenuSize = new Size(640, 530);
            public Size setWindow_formNudgeSize = new Size(200, 400);
            public Size setWindow_gridSize = new Size(400, 400);
            public Size setWindow_HeadAcheSize = new Size(1022, 742);
            public Size setWindow_HeadlineSize = new Size(1022, 742);
            public Size setWindow_MapBndSize = new Size(1022, 742);
            public Size setWindow_RateMapSize = new Size(1022, 742);
            public Size setWindow_Size = new Size(1005, 730);
            public Size setWindow_tramLineSize = new Size(921, 676);

            public Color colorSectionsDay = Color.FromArgb(27, 151, 160);
            public Color colorSectionsNight = Color.FromArgb(27, 100, 100);

            public Color colorDayFrame = Color.FromArgb(210, 210, 230);
            public Color colorNightFrame = Color.FromArgb(50, 50, 65);
            public Color colorFieldDay = Color.FromArgb(160, 160, 185);
            public Color colorFieldNight = Color.FromArgb(60, 60, 60);
            public Color colorTextDay = Color.FromArgb(10, 10, 20);
            public Color colorTextNight = Color.FromArgb(230, 230, 230);
            
            public Color colorVehicle = Color.White;

            public bool isAutoOffAgIO = true;
            public bool isAutoStartAgIO = true;
            public bool isTermsAccepted = false;
            public bool isLogElevation = false;
            public bool isDirectionMarkers = true;

            public bool isCompassOn = true;
            public bool isGridOn = true;
            public bool isLightbarNotSteerBar = false;
            public bool isLightbarOn = true;
            public bool isMetric = true;
            public bool isSideGuideLines = false;
            public bool isSimulatorOn = true;
            public bool isSkyOn = true;
            public bool isSpeedoOn = false;
            public bool isGPSCorrectionLineOn = false;

            public bool sound_isAutoSteerOn = true;
            public bool sound_isHydLiftOn = true;
            public bool sound_isSectionsOn = true;
            public bool sound_isUturnOn = true;

            public int bndToolSpacing = 1;
            public int bndToolSmooth = 1;

            public string hotkeys = "ACFGMNPTYVW12345678";
            public string AgShareServer = "https://agshare.agopengps.com";
            public string AgShareApiKey = "";
            public bool AgShareUploadEnabled = false;

            public LoadResult Load()
            {
                string path = Path.Combine(RegistrySettings.baseDirectory, "User.XML");
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
                string path = Path.Combine(RegistrySettings.baseDirectory, "User.XML");

                XmlSettingsHandler.SaveXMLFile(path, "User", this);
            }

            public void Reset()
            {
                user_ = new UserSettings();
                user_.Save();
            }
        }

        public sealed class VehicleSettings
        {
            public HBrand setBrand_HBrand = HBrand.Aog;
            public TBrand setBrand_TBrand = TBrand.AOG;
            public WDBrand setBrand_WDBrand = WDBrand.Aog;

            public double setAutoSwitchDualFixSpeed = 2.0;
            public bool setAutoSwitchDualFixOn = false;

            public bool setApp_isNozzleApp = false;

            public bool setArdMac_isDanfoss = false;

            public byte setArdMac_hydLowerTime = 4;
            public byte setArdMac_hydRaiseTime = 3;
            public byte setArdMac_isHydEnabled = 0;
            public byte setArdMac_setting0 = 0;
            public byte setArdMac_user1 = 1;
            public byte setArdMac_user2 = 2;
            public byte setArdMac_user3 = 3;
            public byte setArdMac_user4 = 4;

            public byte setArdSteer_maxPulseCounts = 3;
            public byte setArdSteer_maxSpeed = 20;
            public byte setArdSteer_minSpeed = 0;
            public byte setArdSteer_setting0 = 56;
            public byte setArdSteer_setting1 = 0;
            public byte setArdSteer_setting2 = 0;

            public bool setAS_isConstantContourOn = false;
            public bool setAS_isSteerInReverse = false;

            public byte setAS_ackerman = 100;
            public byte setAS_countsPerDegree = 110;
            public byte setAS_highSteerPWM = 180;
            public byte setAS_Kp = 50;
            public byte setAS_lowSteerPWM = 30;
            public byte setAS_minSteerPWM = 25;

            public double setAS_functionSpeedLimit = 12;
            public double setAS_guidanceLookAheadTime = 1.5;
            public double setAS_maxSteerSpeed = 15;
            public double setAS_minSteerSpeed = 0;
            public double setAS_ModeMultiplierStanley = 0.6;
            public double setAS_sideHillComp = 0.0;
            public double setAS_snapDistance = 0.2;
            public double setAS_snapDistanceRef = 0.05;
            public double setAS_uTurnCompensation = 1;
            public double setAS_purePursuitIntegralGain = 0;

            public int setAS_deadZoneDelay = 5;
            public int setAS_deadZoneDistance = 1;
            public int setAS_deadZoneHeading = 10;
            public int setAS_numGuideLines = 10;
            public int setAS_wasOffset = 3;

            public bool setBnd_isDrawPivot = true;

            public bool isVehicleImage = true;
            public int vehicleOpacity = 100;

            public string setF_CurrentFieldDir = "";
            public string setF_CurrentJobDir = "";

            public double setF_boundaryTriggerDistance = 1.0;
            public double setF_minHeadingStepDistance = 0.5;
            public double setF_UserTotalArea = 0.0;

            public bool setF_isSteerWorkSwitchEnabled = false;
            public bool setF_isSteerWorkSwitchManualSections = false;
            public bool setF_isWorkSwitchActiveLow = true;
            public bool setF_isWorkSwitchEnabled = false;
            public bool setF_isWorkSwitchManualSections = false;

            public string setGPS_headingFromWhichSource = "Fix";

            public bool setGPS_isRTK = false;
            public bool setGPS_isRTK_KillAutoSteer = false;

            public double setGPS_dualHeadingOffset = 0.0;
            public double setGPS_dualReverseDetectionDistance = 0.25;
            public double setGPS_forwardComp = 0.15;
            public double setGPS_minimumStepLimit = 0.05;
            public double setGPS_reverseComp = 0.3;
            public double setGPS_SimLatitude = 53.4360564;
            public double setGPS_SimLongitude = -111.160047;

            public int setGPS_ageAlarm = 20;
            public int setGPS_jumpFixAlarmDistance = 0;

            public bool setHeadland_isSectionControlled = true;

            public bool setIMU_invertRoll = false;
            public bool setIMU_isHeadingCorrectionFromAutoSteer = false;
            public bool setIMU_isReverseOn = true;

            public int setIMU_pitchZeroX16 = 0;

            public double setIMU_fusionWeight2 = 0.06;
            public double setIMU_rollFilter = 0.0;
            public double setIMU_rollZero = 0.0;

            public double stanleyDistanceErrorGain = 1;
            public double stanleyHeadingErrorGain = 1;
            public double stanleyIntegralDistanceAwayTriggerAB = 0.25;
            public double stanleyIntegralGainAB = 0;

            public double setVehicle_antennaHeight = 3;
            public double setVehicle_antennaOffset = 0;
            public double setVehicle_antennaPivot = 0.1;
            public double setVehicle_goalPointAcquireFactor = 0.9;
            public double setVehicle_goalPointLookAhead = 3;
            public double setVehicle_goalPointLookAheadHold = 3;
            public double setVehicle_goalPointLookAheadMult = 1.5;

            public double setVehicle_maxAngularVelocity = 0.64;
            public double setVehicle_maxSteerAngle = 30;
            public double setVehicle_minTurningRadius = 8.1;
            public double setVehicle_panicStopSpeed = 0;
            public double setVehicle_trackWidth = 1.9;
            public double setVehicle_wheelbase = 3.3;

            public bool setVehicle_isMachineControlToAutoSteer = false;
            public bool setVehicle_isStanleyUsed = false;

            public double set_youMoveDistance = 0.25;
            public double set_youToolWidths = 2;
            public double set_youTurnDistanceFromBoundary = 2;
            public double set_youTurnRadius = 8.1;

            public int setVehicle_vehicleType = 0;
            public int set_uTurnStyle = 0;
            public int set_youSkipWidth = 1;
            public int set_youTurnExtensionLength = 20;

            public LoadResult Load()
            {
                string path = Path.Combine(RegistrySettings.vehiclesDirectory, RegistrySettings.vehicleFileName + ".XML");
                var result = XmlSettingsHandler.LoadXMLFile(path, this);
                if (result == LoadResult.MissingFile)
                {
                    Log.EventWriter("Vehicle file does not exist or is Default, Default Vehicle used");
                    RegistrySettings.Save("VehicleFileName", "Default");
                }
                else if (result == LoadResult.Failed)
                {
                    Log.EventWriter("Vehicle Loaded With Error:" + result.ToString());
                }

                return result;
            }

            public void Save()
            {
                string path = Path.Combine(RegistrySettings.vehiclesDirectory, RegistrySettings.vehicleFileName + ".XML");

                if (RegistrySettings.vehicleFileName != "" && RegistrySettings.vehicleFileName != "Default")
                    XmlSettingsHandler.SaveXMLFile(path, "Vehicle", this);
                else
                    Log.EventWriter("Default Vehicle Not saved to Vehicles");
            }

            public void Reset()
            {
                vehicle_ = new VehicleSettings();
                vehicle_.Save();
            }
        }

        public sealed class ToolSettings
        {
            public CNozzleSettings setNozz = new CNozzleSettings();
            public CToolSteerSettings setToolSteer = new CToolSteerSettings();

            public bool isDisplayTramControl = true;
            public bool isSectionsNotZones = true;
            public bool isToolFront = false;
            public bool isToolRearFixed = false;
            public bool isToolTBT = false;
            public bool isToolTrailing = true;

            public double toolTrailingHitchLength = -2.5;
            public double trailingToolToPivotLength = 0;
            public double hitchLength = -1;
            public double tankTrailingHitchLength = 3;

            public string zones = "2,10,20,0,0,0,0,0,0";

            public bool isTramOnBackBuffer = true;
            public bool isTramOuterInverted = false;

            public double tram_alpha = 0.8;
            public double tram_offset = 0.0;
            public double tram_snapAdj = 1.0;
            public double tram_Width = 24.0;

            public int tram_BasedOn = 0;
            public int tram_passes = 1;
            public int tram_Skips = 0;

            public double hydraulicLiftLookAhead = 2;
            public double lookAheadMinimum = 2;
            public double slowSpeedCutoff = 0.5;
            public double lookAheadOff = 0.5;
            public double lookAheadOn = 1;
            public double offDelay = 0;
            public double lookAheadDistanceOn = 0;
            public double lookAheadDistanceOff = 0;

            public double offset = 0;
            public double overlap = 0.0;
            public double toolWidth = 4.0;

            public int minCoverage = 100;

            public int numSections = 3;
            public int numSectionsMulti = 20;

            public double defaultSectionWidth = 2;
            public double sectionWidthMulti = 0.5;

            public double[] setSection_Widths = new double[3] { 2, 2, 2 };

            public string setRelay_pinConfig = "1,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";

            public Color setColor_sec01 = Color.FromArgb(249, 22, 10);
            public Color setColor_sec02 = Color.FromArgb(68, 84, 254);
            public Color setColor_sec03 = Color.FromArgb(8, 243, 8);
            public Color setColor_sec04 = Color.FromArgb(233, 6, 233);
            public Color setColor_sec05 = Color.FromArgb(200, 191, 86);
            public Color setColor_sec06 = Color.FromArgb(0, 252, 246);
            public Color setColor_sec07 = Color.FromArgb(144, 36, 246);
            public Color setColor_sec08 = Color.FromArgb(232, 102, 21);
            public Color setColor_sec09 = Color.FromArgb(255, 160, 170);
            public Color setColor_sec10 = Color.FromArgb(205, 204, 246);
            public Color setColor_sec11 = Color.FromArgb(213, 239, 190);
            public Color setColor_sec12 = Color.FromArgb(247, 200, 247);
            public Color setColor_sec13 = Color.FromArgb(253, 241, 144);
            public Color setColor_sec14 = Color.FromArgb(187, 250, 250);
            public Color setColor_sec15 = Color.FromArgb(227, 201, 249);
            public Color setColor_sec16 = Color.FromArgb(247, 229, 215);

            public bool setColor_isMultiColorSections = false;

            public LoadResult Load()
            {
                string path = Path.Combine(RegistrySettings.toolsDirectory, RegistrySettings.toolFileName + ".XML");
                var result = XmlSettingsHandler.LoadXMLFile(path, this);
                if (result == LoadResult.MissingFile)
                {
                    Log.EventWriter("Tool file does not exist or is Default, Default Vehicle used");
                    RegistrySettings.Save("ToolFileName", "Default");
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

                if (RegistrySettings.toolFileName != "" && RegistrySettings.toolFileName != "Default")
                    XmlSettingsHandler.SaveXMLFile(path, "Tool", this);
                else
                    Log.EventWriter("Default Tool Not saved to Tools");
            }

            public void Reset()
            {
                tool_ = new ToolSettings();
                tool_.Save();
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
                        pinfo.SetValue(obj, Color.FromArgb(r, g, b).CheckColorFor255());
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