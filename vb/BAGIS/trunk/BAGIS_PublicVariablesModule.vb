Imports ESRI.ArcGIS.Display

Module BAGIS_PublicVariablesModule

    'Basin Analyst GIS Version text
    Public Const BA_VersionText = "Basin Analysis GIS Ver 2.0 11/30/2012"
    Public Const BA_CompatibleVersion1Text = "Basin Analysis GIS Ver 1.0 7/4/2010"
    Public Const BA_SubVersionText = " Release 2.0K (02/05/2014)"

    'Global variables
    Public BasinFolderBase As String 'need to append a back slash to form a path
    Public AOIFolderBase As String 'need to append a back slash to form a path
    Public PRISMFolderBase As String 'need to append a back slash to form a path
    Public StaticLayersFolderBase As String
    Public PRISMLayer(16) As String '18 elements
    Public BA_ElevationUnitString As String
    Public BA_Excel_Available As Boolean
    Public BA_AOI_Forecast_ID As String 'Ver1E Update -

    'Basin Layer Names
    Public BasinLayerDisplayNames(0 To 7) As String

    'these constants are used to ID whether a folder is a basin or an AOI or both
    Public Const BA_Basin_Type = "output\surfaces\deminfo.txt"
    Public Const BA_AOI_Type = "output\source.weasel"
    Public Const BA_AOIWindow_Type = "output\window.weasel"
    Public Const BA_DEMExtentFolder = ""
    Public Const BA_DEMExtentShapefile = "aoi_v" 'vector
    Public Const BA_AOIExtentCoverage = "aoi_v" 'vector
    Public Const BA_AOIExtentRaster = "aoibagis" 'raster
    Public Const BA_BufferedAOIExtentCoverage = "aoib_v" 'vector
    Public Const BA_BufferedAOIExtentRaster = "aoib" 'raster
    Public Const BA_POURPOINTCoverage = "pourpoint"
    Public Const BA_AOIStreamShapefile = "aoi_streams"
    Public Const BA_MinimumAOIArea = 0.0036 'Sq Km - 4 pixels of 30 meter DEM

    'AOI clipped file name
    Public Const BA_SNOTELSites = "snotel_sites"
    Public Const BA_SnowCourseSites = "snowcourse_sites"
    Public Const BA_PRISMClipAOI = "p_aoi_v" 'vector
    Public Const BA_PRISMClipAOIRaster = "p_aoi" 'raster
    Public BA_PRISMClipBuffer As Double = 1000 'default value = 1000 meters, buffer distance for clipping PRISM data
    Public Const BA_NameFieldWidth = 60 'name field in the attribute table to stor reclass range data

    'AOI snotel site, snow course site, and pseudo-site attributes
    Public Const BA_SiteNameField = "BA_SNAME"
    Public Const BA_SiteElevField = "BA_SELEV"

    'AOI pourpoint area and area unit field names
    Public Const BA_AOIShapeAreaField = "AOISHPAREA"
    Public Const BA_AOIShapeUnitField = "AOISHPUNIT"
    Public Const BA_AOIRefAreaField = "AOIREFAREA"
    Public Const BA_AOIRefUnitField = "AOIREFUNIT"
    'Ver1E update - new field to be added to pourpoin and aoi layers
    Public Const BA_AOI_IDField = "awdb_id"

    'analysis output data names
    Public Const BA_RasterElevationZones = "elevzone"
    Public Const BA_VectorElevationZones = "elevzone_v"
    Public Const BA_SubElevationZones = "subelev"
    Public Const BA_RasterAspectZones = "aspzone"
    Public Const BA_VectorAspectZones = "aspzone_v"
    Public Const BA_RasterSlopeZones = "slpzone"
    Public Const BA_VectorSlopeZones = "slpzone_v"
    Public Const BA_RasterSNOTELZones = "stelzone"
    Public Const BA_VectorSNOTELZones = "stelzone_v"
    Public Const BA_RasterSnowCourseZones = "scoszone"
    Public Const BA_VectorSnowCourseZones = "scoszone_v"
    Public Const BA_RasterPrecipitationZones = "preczone"
    Public Const BA_VectorPrecipitationZones = "preczone_v"
    Public Const BA_TempPRISM = "prismtmp" 'hold temporary aggregated monthly PRISM data
    Public Const BA_AOIAnalysisSummaryFile = "analysis.txt"
    Public Const BA_MapParameterFile = "map_parameters.txt"
    Public Const BA_BufferDistanceFile = "buffer"

    'scenario output rasters
    Public Const BA_PseudoNP = "nppseduo"
    Public Const BA_ActualNP = "npactual"
    Public Const BA_ScenarioResult = "scenario_results.txt"

    'Field values for site scenario tool
    Public Const BA_ValNonRepresentedBelow As Double = 2
    Public Const BA_ValRepresented As Double = 1
    Public Const BA_ValNonRepresentedAbove As Double = 3
    Public Const BA_ValNonRepresentedOutside As Double = 4
    Public Const BA_ValNonRepresented As Double = 5
    'mapframe
    Public Const BA_DefaultMapName = "Basin Analysis"
    Public Const BA_MapAOI = "AOI Boundary"
    Public Const BA_MapElevationZone = "Elevation Zones"
    Public Const BA_MapSNOTELZone = "SNOTEL Elevation Zones"
    Public Const BA_MapSNOTELSite = "SNOTEL Sites"
    Public Const BA_MapSnowCourseZone = "Snow Course Elevation Zones"
    Public Const BA_MapSnowCourseSite = "Snow Courses"
    Public Const BA_MapPrecipZone = "Precipitation Zones"
    Public Const BA_MapHillshade = "Hillshade"
    Public Const BA_MapStream = "AOI Streams"
    Public Const BA_MapAspect = "Aspect"
    Public Const BA_MapSlope = "Slope"
    Public Const BA_MapPRISMElevation = "PRISM Elevation Zones"
    Public Const BA_MapActualRepresentation = "Actual Represented Area"
    Public Const BA_MapPseudoRepresentation = "Pseudo Represented Area"

    'Excel chart
    Public Const BA_ChartWidth = 600
    Public Const BA_ChartHeight = 330
    Public Const BA_ChartSpacing = 5

    'location of the basin analyst definition file
    Public BA_Settings_Filepath As String 'read from ARCGISHOME environ variable
    Public Const BA_Settings_Filename = "basinanalyst.def"
    Public Const BA_Settings_PathVariable = "BAGIS"

    'control flags for menu items
    'Basin Analysis tools
    'Public SetOption_Flag as Boolean 'Option menu is always available
    Public SaveAOIMXD_Flag As Boolean 'Save mxd tool is available only when AOI is selected
    Public BasinInfo_Flag As Boolean
    Public AOIInfo_Flag As Boolean
    Public SelectBasin_Flag As Boolean
    Public SetSettings_Flag As Boolean
    Public AddRefLayers_Flag As Boolean

    'Basin Tools
    Public SetDEMExtent_Flag As Boolean
    Public ClipDEM_Flag As Boolean

    'AOI Tools
    'Public SelectAOI_Flag As Boolean
    'Public SetPourPoint_Flag As Boolean
    'Public CreateAOIStream_Flag As Boolean

    'Basin Analysis Tools
    Public GenerateMaps_Flag As Boolean
    Public ElevationScenario_Flag As Boolean
    Public ElevDistMap_Flag As Boolean
    Public ElevSNOTELMap_Flag As Boolean
    Public ElevSnowCourseMap_Flag As Boolean
    Public PrecipDistMap_Flag As Boolean
    Public SlopeDistMap_Flag As Boolean
    Public AspectDistMap_Flag As Boolean
    Public Scenario1Map_Flag As Boolean
    Public Scenario2Map_Flag As Boolean
    Public RepDifferenceMap_Flag As Boolean
    Public SiteRepresentationMap_Flag As Boolean

    Public Maps_Are_Generated As Boolean 'see if basin analysis maps are displayed in the map frame
    Public ScenarioMaps_Are_Generated As Boolean 'see if the scenario maps are displayed in the map frame

    'for generating excel and maps purposes
    Public AOI_HasSNOTEL As Boolean
    Public AOI_HasSnowCourse As Boolean
    Public AOI_HasPseudoSite As Boolean
    Public AOI_ReferenceArea As Double
    Public AOI_ReferenceUnit As String
    Public AOI_ShapeArea As Double
    Public AOI_ShapeUnit As String

    'variables for keeping track of internal DEM min and max, unit is in meters
    Public AOI_DEMMin As Double
    Public AOI_DEMMax As Double
    Public AOI_DEMRange As Double
    Public Map_Display_Elevation_in_Meters As Boolean

    Public pSelectColor As IRgbColor
    Public pDisplayColor As IRgbColor

    'variables for keeping track of settings form PourPoint NameField index, . . . _
    ' . . . SNOTEL NameFieldIndex, SnowCourse NameField Index, and lstLayers items names
    Public PourNameFieldIndex As Integer
    Public SNOTEL_NameFieldIndex As Integer
    Public SCourse_NameFieldIndex As Integer

    'this variable is for snapping dem when creating BASINs
    Public BA_DEMDimension As BA_DEMInfo

    Public Structure BA_SystemSettingsType
        Public Status As Integer '1 ready, else, not set yet
        Public DEM10M As String
        Public DEM30M As String
        Public DEM10MPreferred As Boolean 'if True then 10 meter is preferred, else 30 meter
        Public DEM_ZUnit_IsMeter As Boolean
        Public PourPointLayer As String
        Public PourPointField As String
        Public PourAreaField As String
        Public PourAreaUnit As String
        Public SNOTELLayer As String
        Public SNOTEL_ElevationField As String
        Public SNOTEL_NameField As String
        Public SNOTEL_ZUnit_IsMeter As Boolean
        Public SCourseLayer As String
        Public SCourse_ElevationField As String
        Public SCourse_NameField As String
        Public SCourse_ZUnit_IsMeter As Boolean
        Public PRISMFolder As String
        Public GaugeID As String
        Public Ref_Terrain As String
        Public Ref_Drainage As String
        Public Ref_Watershed As String
        Public OtherLayers() As String
        Public listCount As Integer
        Public GenerateAOIOnly As Boolean
        'Public listlayers As Integer
    End Structure

    Public BA_SystemSettings As BA_SystemSettingsType

    Public Structure BA_DEMInfo
        Public IDText As String
        Public Cellsize As Double
        Public Min As Double
        Public Max As Double
        Public Range As Double
        Public Exist As Boolean
        Public X_CellSize As Double
        Public Y_CellSize As Double
        Public Width_inPixels As Long
        Public Height_inPixels As Long
        Public Min_MapX As Double
        Public Max_MapX As Double
        Public Min_MapY As Double
        Public Max_MapY As Double
    End Structure

    Public Structure BA_SubFolderList
        Public Name As String 'name of the folder
        Public gdbdem As String 'the folder is a BASIN
        Public gdbAOI As Boolean 'the folder is an AOI
        Public weaseldem As String 'the folder is a weasel BASIN
        Public weaselAOI As Boolean 'the folder is a weasel AOI
        Public hasAOI As Boolean 'the folder has at least one AOI (either FGDB or weasel)
    End Structure

    'Public BASIN_Folder_Types As BA_SubFolderList
    ''this global variable is to track a folder's BAGIS properties in an effort to speed up the folder navigation task
End Module
