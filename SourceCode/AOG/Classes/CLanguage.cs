using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AOG.Classes
{
    public static class gStr
    {
        public static Dictionary<string, string> cultureDictionary = new Dictionary<string, string>();

        public static bool Load()
        {
            string filePath = Path.Combine(Application.StartupPath, "Translations.xlsx");
            if (!File.Exists(filePath))
            {
                return false;
            }

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    IExcelDataReader reader;

                    reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);

                    var dataTable = dataSet.Tables[0];

                    int column = 2;

                    for (int i = 1; i < dataTable.Rows.Count; i++)
                    {
                        string bob = dataTable.Rows[0][i].ToString();

                        if (dataTable.Rows[0][i].ToString() == RegistrySettings.culture)
                        {
                            column = i;
                            break;
                        }
                    }

                    cultureDictionary?.Clear();

                    for (int i = 1; i < dataTable.Rows.Count; ++i)
                    {
                        if (!String.IsNullOrEmpty(dataTable.Rows[i][column].ToString()))
                        {
                            cultureDictionary.Add(dataTable.Rows[i][0].ToString(), dataTable.Rows[i][column].ToString());
                        }
                        else
                        {
                            cultureDictionary.Add(dataTable.Rows[i][0].ToString(), dataTable.Rows[i][2].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Catch Language Load: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        public static void GenerateReferenceKeys()
        {
            string filePath = Path.Combine(Application.StartupPath, "Translations.xlsx");
            if (!File.Exists(filePath))
            {
                return;
            }

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    IExcelDataReader reader;

                    reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);

                    var dataTable = dataSet.Tables[0];

                    //write out the file
                    using (StreamWriter writer = new StreamWriter(Path.Combine(Application.StartupPath, "TranslationReferences.txt")))
                    {
                        writer.WriteLine("public static class gs");
                        writer.WriteLine("{");
                        for (int i = 1; i < dataTable.Rows.Count; i++)
                        {
                            writer.WriteLine($"     public static string {dataTable.Rows[i][0]} = \"{dataTable.Rows[i][0]}\";");
                        }

                        writer.WriteLine("}");
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Catch Language Reference Keys error: {ex.Message}");
                }
            }
        }

        public static string Get(string _gstr)
        {
            string result;
            if (cultureDictionary.TryGetValue(_gstr, out result))
            {
                return result;
            }
            else
            {
                return "Fail";
            }
        }
    }

    public static class gs
    {
        public static string gsABCurve = "gsABCurve";
        public static string gsABDraw = "gsABDraw";
        public static string gsABline = "gsABline";
        public static string gsADConverter = "gsADConverter";
        public static string gsAOGMenu = "gsAOGMenu";
        public static string gsAPlus = "gsAPlus";
        public static string gsAbout = "gsAbout";
        public static string gsAckermann = "gsAckermann";
        public static string gsActionHasBeenCancelled = "gsActionHasBeenCancelled";
        public static string gsActive = "gsActive";
        public static string gsActual = "gsActual";
        public static string gsAdvancedTramLines = "gsAdvancedTramLines";
        public static string gsAgree = "gsAgree";
        public static string gsAgressiveness = "gsAgressiveness";
        public static string gsAlpha = "gsAlpha";
        public static string gsAntennaHeight = "gsAntennaHeight";
        public static string gsAntennaOffset = "gsAntennaOffset";
        public static string gsApplied = "gsApplied";
        public static string gsAquire = "gsAquire";
        public static string gsAquireDescription = "gsAquireDescription";
        public static string gsAquireFactor = "gsAquireFactor";
        public static string gsAreYouSure = "gsAreYouSure";
        public static string gsArea = "gsArea";
        public static string gsAttachmentStyle = "gsAttachmentStyle";
        public static string gsAuto = "gsAuto";
        public static string gsAutoOffAgIO = "gsAutoOffAgIO";
        public static string gsAutoStartAgIO = "gsAutoStartAgIO";
        public static string gsAutoSteer = "gsAutoSteer";
        public static string gsAutoSteerConfiguration = "gsAutoSteerConfiguration";
        public static string gsAutoSteerPort = "gsAutoSteerPort";
        public static string gsAutoSwitchDualFix = "gsAutoSwitchDualFix";
        public static string gsAutoSwitchDualFixSpeed = "gsAutoSwitchDualFixSpeed";
        public static string gsBackground = "gsBackground";
        public static string gsBasedOnField = "gsBasedOnField";
        public static string gsBottomMenu = "gsBottomMenu";
        public static string gsBoundary = "gsBoundary";
        public static string gsBoundaryLineFilesAreCorrupt = "gsBoundaryLineFilesAreCorrupt";
        public static string gsBoundaryMenu = "gsBoundaryMenu";
        public static string gsBrightness = "gsBrightness";
        public static string gsBuild = "gsBuild";
        public static string gsBuildAround = "gsBuildAround";
        public static string gsButFieldIsLoaded = "gsButFieldIsLoaded";
        public static string gsButtonPicker = "gsButtonPicker";
        public static string gsCameraBehavior = "gsCameraBehavior";
        public static string gsCancel = "gsCancel";
        public static string gsCenter = "gsCenter";
        public static string gsCentimeters = "gsCentimeters";
        public static string gsCharts = "gsCharts";
        public static string gsCheckForUpdates = "gsCheckForUpdates";
        public static string gsChoose = "gsChoose";
        public static string gsChooseADifferentField = "gsChooseADifferentField";
        public static string gsChooseADifferentName = "gsChooseADifferentName";
        public static string gsChooseBuildDifferentone = "gsChooseBuildDifferentone";
        public static string gsClipLine = "gsClipLine";
        public static string gsClose = "gsClose";
        public static string gsCloseAllWindowsFirst = "gsCloseAllWindowsFirst";
        public static string gsCloseFieldFirst = "gsCloseFieldFirst";
        public static string gsCmPix = "gsCmPix";
        public static string gsColorPicker = "gsColorPicker";
        public static string gsColors = "gsColors";
        public static string gsCompletelyDeleteBoundary = "gsCompletelyDeleteBoundary";
        public static string gsConfiguration = "gsConfiguration";
        public static string gsContourFileIsCorrupt = "gsContourFileIsCorrupt";
        public static string gsContourOn = "gsContourOn";
        public static string gsCopyCurrentToolAs = "gsCopyCurrentToolAs";
        public static string gsCopyCurrentVehicleAs = "gsCopyCurrentVehicleAs";
        public static string gsCouldntGenerateValidPath = "gsCouldntGenerateValidPath";
        public static string gsCountsPerDegree = "gsCountsPerDegree";
        public static string gsCoverage = "gsCoverage";
        public static string gsCreate = "gsCreate";
        public static string gsCreateABoundaryFirst = "gsCreateABoundaryFirst";
        public static string gsCreateNewField = "gsCreateNewField";
        public static string gsCreateNewFromIsoXML = "gsCreateNewFromIsoXML";
        public static string gsCreateNewJob = "gsCreateNewJob";
        public static string gsCurrent = "gsCurrent";
        public static string gsCurrentTurnSensor = "gsCurrentTurnSensor";
        public static string gsCurve = "gsCurve";
        public static string gsCurveLineFileIsCorrupt = "gsCurveLineFileIsCorrupt";
        public static string gsCurveNotOn = "gsCurveNotOn";
        public static string gsDay = "gsDay";
        public static string gsDeadzone = "gsDeadzone";
        public static string gsDefault = "gsDefault";
        public static string gsDelete = "gsDelete";
        public static string gsDeleteAllContoursAndSections = "gsDeleteAllContoursAndSections";
        public static string gsDeleteAppliedArea = "gsDeleteAppliedArea";
        public static string gsDeleteBoundaryMapping = "gsDeleteBoundaryMapping";
        public static string gsDeleteContourPaths = "gsDeleteContourPaths";
        public static string gsDeleteField = "gsDeleteField";
        public static string gsDeleteForSure = "gsDeleteForSure";
        public static string gsDiameter = "gsDiameter";
        public static string gsDirect = "gsDirect";
        public static string gsDirectionMarkers = "gsDirectionMarkers";
        public static string gsDirectories = "gsDirectories";
        public static string gsDirectoryExists = "gsDirectoryExists";
        public static string gsDisagree = "gsDisagree";
        public static string gsDiscussions = "gsDiscussions";
        public static string gsDisplay = "gsDisplay";
        public static string gsDistance = "gsDistance";
        public static string gsDistanceToFlag = "gsDistanceToFlag";
        public static string gsDriveIn = "gsDriveIn";
        public static string gsDriveThru = "gsDriveThru";
        public static string gsDriving = "gsDriving";
        public static string gsDualAntennaSetting = "gsDualAntennaSetting";
        public static string gsDualpositionAntennaRight = "gsDualpositionAntennaRight";
        public static string gsEast = "gsEast";
        public static string gsEditABCurve = "gsEditABCurve";
        public static string gsEditABLine = "gsEditABLine";
        public static string gsEditColor = "gsEditColor";
        public static string gsEditFieldName = "gsEditFieldName";
        public static string gsElevationlog = "gsElevationlog";
        public static string gsEnable = "gsEnable";
        public static string gsEncoderCounts = "gsEncoderCounts";
        public static string gsEnterCoordinatesForSimulator = "gsEnterCoordinatesForSimulator";
        public static string gsEnterFieldName = "gsEnterFieldName";
        public static string gsEnterJobName = "gsEnterJobName";
        public static string gsEnterName = "gsEnterName";
        public static string gsEnterRecordName = "gsEnterRecordName";
        public static string gsEnterSimCoords = "gsEnterSimCoords";
        public static string gsError = "gsError";
        public static string gsErrorreadingKML = "gsErrorreadingKML";
        public static string gsExit = "gsExit";
        public static string gsExitToWindows = "gsExitToWindows";
        public static string gsExtraGuideLines = "gsExtraGuideLines";
        public static string gsFast = "gsFast";
        public static string gsField = "gsField";
        public static string gsFieldFileIsCorrupt = "gsFieldFileIsCorrupt";
        public static string gsFieldIsOpen = "gsFieldIsOpen";
        public static string gsFieldMenu = "gsFieldMenu";
        public static string gsFieldNotOpen = "gsFieldNotOpen";
        public static string gsFieldPicker = "gsFieldPicker";
        public static string gsFieldTexture = "gsFieldTexture";
        public static string gsFileError = "gsFileError";
        public static string gsFix2Fix = "gsFix2Fix";
        public static string gsFixAlarm = "gsFixAlarm";
        public static string gsFixAlarmStop = "gsFixAlarmStop";
        public static string gsFlagByLatLon = "gsFlagByLatLon";
        public static string gsFlagFileIsCorrupt = "gsFlagFileIsCorrupt";
        public static string gsFlags = "gsFlags";
        public static string gsForNow = "gsForNow";
        public static string gsFormFlag = "gsFormFlag";
        public static string gsFromExisting = "gsFromExisting";
        public static string gsFromKml = "gsFromKml";
        public static string gsGap = "gsGap";
        public static string gsGpsStep = "gsGpsStep";
        public static string gsGrid = "gsGrid";
        public static string gsGuidanceStopped = "gsGuidanceStopped";
        public static string gsHardwareMessages = "gsHardwareMessages";
        public static string gsHeading = "gsHeading";
        public static string gsHeadingChart = "gsHeadingChart";
        public static string gsHeadingOffset = "gsHeadingOffset";
        public static string gsHeadland = "gsHeadland";
        public static string gsHeadlandForm = "gsHeadlandForm";
        public static string gsHelp = "gsHelp";
        public static string gsHitchLength = "gsHitchLength";
        public static string gsHold = "gsHold";
        public static string gsHydraulicLift = "gsHydraulicLift";
        public static string gsHydraulicLiftConfig = "gsHydraulicLiftConfig";
        public static string gsHydraulicLiftLookAhead = "gsHydraulicLiftLookAhead";
        public static string gsIMUAxis = "gsIMUAxis";
        public static string gsIfWrongDirectionTapVehicle = "gsIfWrongDirectionTapVehicle";
        public static string gsImage = "gsImage";
        public static string gsImuFusion = "gsImuFusion";
        public static string gsInches = "gsInches";
        public static string gsInner = "gsInner";
        public static string gsIntegral = "gsIntegral";
        public static string gsIntegralInfo = "gsIntegralInfo";
        public static string gsInvertHydraulicRelays = "gsInvertHydraulicRelays";
        public static string gsInvertMotor = "gsInvertMotor";
        public static string gsInvertRelays = "gsInvertRelays";
        public static string gsInvertRoll = "gsInvertRoll";
        public static string gsInvertWas = "gsInvertWas";
        public static string gsKMH = "gsKMH";
        public static string gsKeyboard = "gsKeyboard";
        public static string gsLanguage = "gsLanguage";
        public static string gsLatLon = "gsLatLon";
        public static string gsLatLonPlus = "gsLatLonPlus";
        public static string gsLateral = "gsLateral";
        public static string gsLatitude = "gsLatitude";
        public static string gsLeft = "gsLeft";
        public static string gsLess = "gsLess";
        public static string gsLightbar = "gsLightbar";
        public static string gsLineSmooth = "gsLineSmooth";
        public static string gsLineWidth = "gsLineWidth";
        public static string gsLongtitude = "gsLongtitude";
        public static string gsLookAhead = "gsLookAhead";
        public static string gsLookAheadTiming = "gsLookAheadTiming";
        public static string gsLowerTime = "gsLowerTime";
        public static string gsMPH = "gsMPH";
        public static string gsMachineModule = "gsMachineModule";
        public static string gsMachinePort = "gsMachinePort";
        public static string gsMakeBoundaryContours = "gsMakeBoundaryContours";
        public static string gsManual = "gsManual";
        public static string gsManualTurns = "gsManualTurns";
        public static string gsMapForBackground = "gsMapForBackground";
        public static string gsMaxLimit = "gsMaxLimit";
        public static string gsMaxSpeed = "gsMaxSpeed";
        public static string gsMaxSteerAngle = "gsMaxSteerAngle";
        public static string gsMeters = "gsMeters";
        public static string gsMinSpeed = "gsMinSpeed";
        public static string gsMinToMove = "gsMinToMove";
        public static string gsMissingABLinesFile = "gsMissingABLinesFile";
        public static string gsMissingBoundaryFile = "gsMissingBoundaryFile";
        public static string gsMissingContourFile = "gsMissingContourFile";
        public static string gsMissingFlagsFile = "gsMissingFlagsFile";
        public static string gsMissingSectionFile = "gsMissingSectionFile";
        public static string gsMode = "gsMode";
        public static string gsMore = "gsMore";
        public static string gsMotorDriver = "gsMotorDriver";
        public static string gsMultiColorSections = "gsMultiColorSections";
        public static string gsN_East = "gsN_East";
        public static string gsN_West = "gsN_West";
        public static string gsNew = "gsNew";
        public static string gsNewDefaultTool = "gsNewDefaultTool";
        public static string gsNewDefaultVehicle = "gsNewDefaultVehicle";
        public static string gsNextGuidanceLine = "gsNextGuidanceLine";
        public static string gsNight = "gsNight";
        public static string gsNoABLineActive = "gsNoABLineActive";
        public static string gsNoBoundary = "gsNoBoundary";
        public static string gsNoFieldsFound = "gsNoFieldsFound";
        public static string gsNoGuidanceLines = "gsNoGuidanceLines";
        public static string gsNoSettingsWithIMU = "gsNoSettingsWithIMU";
        public static string gsNone = "gsNone";
        public static string gsNoneUsed = "gsNoneUsed";
        public static string gsNorth = "gsNorth";
        public static string gsNothingDeleted = "gsNothingDeleted";
        public static string gsNudge = "gsNudge";
        public static string gsNudgeDistance = "gsNudgeDistance";
        public static string gsNudgeRefTrack = "gsNudgeRefTrack";
        public static string gsOff = "gsOff";
        public static string gsOffset = "gsOffset";
        public static string gsOffsetFix = "gsOffsetFix";
        public static string gsOn = "gsOn";
        public static string gsOnDelay = "gsOnDelay";
        public static string gsOnOff = "gsOnOff";
        public static string gsOpacity = "gsOpacity";
        public static string gsOpen = "gsOpen";
        public static string gsOuter = "gsOuter";
        public static string gsOverlap = "gsOverlap";
        public static string gsOverlapGap = "gsOverlapGap";
        public static string gsOverride = "gsOverride";
        public static string gsOvershootReduction = "gsOvershootReduction";
        public static string gsPass = "gsPass";
        public static string gsPasses = "gsPasses";
        public static string gsPastEndOfCurve = "gsPastEndOfCurve";
        public static string gsPause = "gsPause";
        public static string gsPivot = "gsPivot";
        public static string gsPivotDistance = "gsPivotDistance";
        public static string gsPlantPop = "gsPlantPop";
        public static string gsPleaseEnterABLine = "gsPleaseEnterABLine";
        public static string gsPleaseWait = "gsPleaseWait";
        public static string gsPoint = "gsPoint";
        public static string gsPoints = "gsPoints";
        public static string gsPointsToProcess = "gsPointsToProcess";
        public static string gsPolygons = "gsPolygons";
        public static string gsPowerLoss = "gsPowerLoss";
        public static string gsPresetColor = "gsPresetColor";
        public static string gsPressureSensorValueLabel = "gsPressureSensorValueLabel";
        public static string gsPressureTurnSensor = "gsPressureTurnSensor";
        public static string gsPreview = "gsPreview";
        public static string gsProblemMakingPath = "gsProblemMakingPath";
        public static string gsProgramWillExitPleaseRestart = "gsProgramWillExitPleaseRestart";
        public static string gsProportionalGain = "gsProportionalGain";
        public static string gsQuickAB = "gsQuickAB";
        public static string gsRaiseTime = "gsRaiseTime";
        public static string gsRate = "gsRate";
        public static string gsReallyResetEverything = "gsReallyResetEverything";
        public static string gsRecord = "gsRecord";
        public static string gsRecordedPathFileIsCorrupt = "gsRecordedPathFileIsCorrupt";
        public static string gsRecordedPathMenu = "gsRecordedPathMenu";
        public static string gsRecordedPathPicker = "gsRecordedPathPicker";
        public static string gsReducedPoints = "gsReducedPoints";
        public static string gsRemain = "gsRemain";
        public static string gsRemoveOffset = "gsRemoveOffset";
        public static string gsReset = "gsReset";
        public static string gsResetAll = "gsResetAll";
        public static string gsResetAllForSure = "gsResetAllForSure";
        public static string gsResume = "gsResume";
        public static string gsReverseDistance = "gsReverseDistance";
        public static string gsReverseSteer = "gsReverseSteer";
        public static string gsRight = "gsRight";
        public static string gsRightMenu = "gsRightMenu";
        public static string gsRollFilter = "gsRollFilter";
        public static string gsS_East = "gsS_East";
        public static string gsS_West = "gsS_West";
        public static string gsSaveAndReturn = "gsSaveAndReturn";
        public static string gsSaveAs = "gsSaveAs";
        public static string gsScreenButtons = "gsScreenButtons";
        public static string gsSectionColorsSet = "gsSectionColorsSet";
        public static string gsSectionLines = "gsSectionLines";
        public static string gsSections = "gsSections";
        public static string gsSelectButtons = "gsSelectButtons";
        public static string gsSelectPreset = "gsSelectPreset";
        public static string gsSendAndSave = "gsSendAndSave";
        public static string gsSentToMachineModule = "gsSentToMachineModule";
        public static string gsSetPoint = "gsSetPoint";
        public static string gsShiftGPSPosition = "gsShiftGPSPosition";
        public static string gsShutdown = "gsShutdown";
        public static string gsShutdownIn = "gsShutdownIn";
        public static string gsSideHillComp = "gsSideHillComp";
        public static string gsSimpleTramLines = "gsSimpleTramLines";
        public static string gsSimulatorOn = "gsSimulatorOn";
        public static string gsSingleAntennaSetting = "gsSingleAntennaSetting";
        public static string gsSlow = "gsSlow";
        public static string gsSlowDownBelow = "gsSlowDownBelow";
        public static string gsSmooth = "gsSmooth";
        public static string gsSmoothABCurve = "gsSmoothABCurve";
        public static string gsSort = "gsSort";
        public static string gsSound = "gsSound";
        public static string gsSouth = "gsSouth";
        public static string gsSpacing = "gsSpacing";
        public static string gsSpeedFactor = "gsSpeedFactor";
        public static string gsSpeedo = "gsSpeedo";
        public static string gsStart = "gsStart";
        public static string gsStartDeleteABoundary = "gsStartDeleteABoundary";
        public static string gsStartFullscreen = "gsStartFullscreen";
        public static string gsStartNewField = "gsStartNewField";
        public static string gsStatus = "gsStatus";
        public static string gsSteerAngle = "gsSteerAngle";
        public static string gsSteerBar = "gsSteerBar";
        public static string gsSteerChart = "gsSteerChart";
        public static string gsSteerEnable = "gsSteerEnable";
        public static string gsSteerInReverse = "gsSteerInReverse";
        public static string gsSteerResponse = "gsSteerResponse";
        public static string gsSteerSwitch = "gsSteerSwitch";
        public static string gsSteerWizard = "gsSteerWizard";
        public static string gsStopRecordPauseBoundary = "gsStopRecordPauseBoundary";
        public static string gsSvennArrow = "gsSvennArrow";
        public static string gsTermsConditions = "gsTermsConditions";
        public static string gsTermsOne = "gsTermsOne";
        public static string gsTermsThree = "gsTermsThree";
        public static string gsTermsTwo = "gsTermsTwo";
        public static string gsToFile = "gsToFile";
        public static string gsTooFast = "gsTooFast";
        public static string gsToolLeft = "gsToolLeft";
        public static string gsToolOffset = "gsToolOffset";
        public static string gsToolRight = "gsToolRight";
        public static string gsToolsMenu = "gsToolsMenu";
        public static string gsToolSteerConfiguration = "gsToolSteerConfiguration";
        public static string gsTopFieldView = "gsTopFieldView";
        public static string gsTotal = "gsTotal";
        public static string gsTrack = "gsTrack";
        public static string gsTracks = "gsTracks";
        public static string gsTramLines = "gsTramLines";
        public static string gsTramWidth = "gsTramWidth";
        public static string gsTurnABCurveOn = "gsTurnABCurveOn";
        public static string gsTurnOffDelay = "gsTurnOffDelay";
        public static string gsTurnOffRecordedPath = "gsTurnOffRecordedPath";
        public static string gsTurnOnContourOrMakeABLine = "gsTurnOnContourOrMakeABLine";
        public static string gsTurnSensor = "gsTurnSensor";
        public static string gsUnit = "gsUnit";
        public static string gsUnits = "gsUnits";
        public static string gsUseSelected = "gsUseSelected";
        public static string gsUser1 = "gsUser1";
        public static string gsUser2 = "gsUser2";
        public static string gsUser3 = "gsUser3";
        public static string gsUser4 = "gsUser4";
        public static string gsUturn = "gsUturn";
        public static string gsUturnCompensation = "gsUturnCompensation";
        public static string gsUturnExtension = "gsUturnExtension";
        public static string gsUturnSmooth = "gsUturnSmooth";
        public static string gsVehiclegroupbox = "gsVehiclegroupbox";
        public static string gsWasZero = "gsWasZero";
        public static string gsWebCam = "gsWebCam";
        public static string gsWest = "gsWest";
        public static string gsWheelbase = "gsWheelbase";
        public static string gsWidth = "gsWidth";
        public static string gsWindowsStillOpen = "gsWindowsStillOpen";
        public static string gsWizard = "gsWizard";
        public static string gsWizards = "gsWizards";
        public static string gsWorkSwitch = "gsWorkSwitch";
        public static string gsWorkWidth = "gsWorkWidth";
        public static string gsWorked = "gsWorked";
        public static string gsXTEChart = "gsXTEChart";
        public static string gsZeroRoll = "gsZeroRoll";
        public static string gsZone = "gsZone";
        public static string gsZones = "gsZones";
        public static string gsZoomIn = "gsZoomIn";
    }
}