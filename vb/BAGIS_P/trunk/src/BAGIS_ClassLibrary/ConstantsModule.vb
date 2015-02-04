Public Module ConstantsModule

    'Class library version; Update with each change to class library
    Public Const BA_CLASS_LIBRARY_VERSION As String = "L034"

    ' Used with BA_QueryAttributeTable to customize query
    Public Const BA_STRING_ATTRIBUTE As String = "string_attribute"
    Public Const BA_NUMBER_ATTRIBUTE As String = "number_attribute"

    ' Used as input for BA_Read_FolderType_File 
    Public Const BA_BASIN_TYPE As String = "output\surfaces\deminfo.txt"
    Public Const BA_AOI_TYPE As String = "output\source.weasel"
    Public Const BA_AOIWINDOW_TYPE As String = "output\window.weasel"
    Public Const BA_AOI_FLOW_DIR As String = "output\surfaces\dem\filled\flow-direction\source.weasel"
    Public Const BA_AOI_FLOW_ACC As String = "output\surfaces\dem\filled\flow-direction\flow-accumulation\source.weasel"

    ' Used for BA_DisplayRasterWithSymbol DisplayName argument
    Public Const BA_MAP_PSEUDO_REPRESENTATION As String = "Pseudo Represented Area"
    Public Const BA_MAP_ACTUAL_REPRESENTATION As String = "Actual Represented Area"

    'name field in the attribute table to store reclass range data
    Public Const BA_NAME_FIELD_WIDTH As Short = 60
    Public Const BA_BOUND_FIELD_WIDTH As Short = 24
    Public Const BA_GRID_NAME_MAX_LENGTH As Short = 8
    Public Const BA_SHAPEFILE_FIELD_NAME_MAX_LENGTH As Short = 10
    Public Const BA_GRID_FIELD_NAME_MAX_LENGTH As Short = 16

    'conversion factors
    Public Const BA_METERS_TO_FEET As Double = 3.2808399
    Public Const BA_FEET_TO_METERS As Double = 0.3048
    Public Const BA_SQ_METERS_PER_ACRE As Double = 4046.8564244
    Public Const BA_SQKm_To_SQMile As Double = 0.386102159
    Public Const BA_SQKm_To_ACRE As Double = 247.1053814671653
    Public Const BA_Inches_To_Millimeters As Double = 25.4
    Public Const BA_SQKm_To_HECTARE As Double = 100
    Public Const BA_SQFeet_To_SQMile As Double = 27878400
    Public Const BA_SQMile_To_SQKm As Double = 2.58999

    'analysis output data names
    Public Const BA_RASTER_ELEVATION_ZONES As String = "elevzone"
    Public Const BA_TEMP_PRISM As String = "prismtmp" 'hold temporary aggregated monthly PRISM data
    Public Const BA_TEMP_PREFIX As String = "tmp" 'prefix for tmp files in gdb that will be deleted by BA_RemoveTemporaryRasters

    'field names used when reading/writing to attribute tables
    Public Const BA_FIELD_VALUE As String = "VALUE"
    Public Const BA_FIELD_NAME As String = "NAME"
    Public Const BA_FIELD_LBOUND As String = "LBOUND"
    Public Const BA_FIELD_UBOUND As String = "UBOUND"
    Public Const BA_FIELD_COUNT As String = "COUNT"
    Public Const BA_FIELD_GRIDCODE As String = "GRIDCODE"
    'grid code field name is slightly different using GP tools with a file gdb
    Public Const BA_FIELD_GRIDCODE_GDB As String = "grid_code"
    Public Const BA_FIELD_FROM As String = "FROM_VAL"
    Public Const BA_FIELD_TO As String = "TO_VAL"
    Public Const BA_FIELD_OUT As String = "OUT_VAL"
    Public Const BA_FIELD_AREA_SQKM As String = "AREA_SqKm"
    Public Const BA_FIELD_AREA_ACRE As String = "AREA_Acre"
    Public Const BA_FIELD_AREA_SQMI As String = "AREA_SqMiles"
    Public Const BA_FIELD_HRUID_CO As String = "HRUID_CO"
    Public Const BA_FIELD_HRUID_NC As String = "HRUID_NC"
    Public Const BA_FIELD_ID As String = "ID"
    Public Const BA_FIELD_SHAPE As String = "Shape"
    Public Const BA_FIELD_OBJECT_ID As String = "OBJECTID"
    'field names for PRMS calculation
    Public Const BA_FIELD_TEMP_ASPECT As String = "TempAspect"
    Public Const BA_FIELD_TEMP_SLOPE As String = "TempSlope"
    Public Const BA_FIELD_RADP_SLP As String = "RADP_SLP"
    Public Const BA_FIELD_RADP_ASP As String = "RADP_ASP"
    Public Const BA_FIELD_PRMS_MAJORITY As String = "PRMS_Mj"
    'field name for the hru_id in BAGIS-P parameter table
    Public Const BA_FIELD_HRU_ID As String = "HRU_ID"
    Public Const BA_FIELD_SITE_NAME As String = "BA_SNAME"
    Public Const BA_FIELD_NEW_HRU_ID As String = "NEW_HRU_ID"
    'field name for erams_id in BAGIS-P
    Public Const BA_FIELD_ERAMS_ID As String = "ERAMS_ID"
    Public Const BA_FIELD_FLOW_ACCUM As String = "FLOW_ACC"
    'field names for subAoi functionality in BAGIS-P
    Public Const BA_FIELD_SUB_AOI_ID As String = "SUB_AOI_ID"
    Public Const BA_FIELD_SUB_AOI_NAME As String = "NAME"
    Public Const BA_FIELD_GAUGE_NUMBER As String = "GAUGE_NUMBER"
    Public Const BA_FIELD_AWDB_ID As String = "awdb_id"
    Public Const BA_FIELD_TIMBER_ELEV As String = "TIMBER_ELEV"
    Public Const BA_FIELD_AREA As String = "AREA"

    'mapframe
    Public Const BA_MAPS_DEFAULT_MAP_NAME As String = "Basin Analysis"
    Public Const BA_MAPS_AOI_BOUNDARY = "AOI Boundary"
    Public Const BA_MAPS_ELEVATION_ZONES = "Elevation Zones"
    Public Const BA_MAPS_SNOTEL_ZONES = "SNOTEL Elevation Zones"
    Public Const BA_MAPS_SNOTEL_SITES = "SNOTEL Sites"
    Public Const BA_MAPS_SNOW_COURSE_ZONES = "Snow Course Elevation Zones"
    Public Const BA_MAPS_SNOW_COURSE_SITES = "Snow Courses"
    Public Const BA_MAPS_PRECIPITATION_ZONES = "Precipitation Zones"
    Public Const BA_MAPS_HILLSHADE = "Hillshade"
    Public Const BA_MAPS_STREAMS = "AOI Streams"
    Public Const BA_MAPS_ASPECT = "Aspect"
    Public Const BA_MAPS_SLOPE = "Slope"
    Public Const BA_MAPS_SCENARIO1_REPRESENTATION = "Scenario 1 Represented"
    Public Const BA_MAPS_SCENARIO2_REPRESENTATION = "Scenario 2 Represented"
    Public Const BA_MAPS_PSEUDO_SITES = "Pseudo Sites"
    Public Const BA_MAPS_BOTH_REPRESENTATION = "Represented in Both"
    Public Const BA_MAPS_SNOTEL_SCENARIO1 = "Snotel Scenario 1"
    Public Const BA_MAPS_SNOTEL_SCENARIO2 = "Snotel Scenario 2"
    Public Const BA_MAPS_SNOW_COURSE_SCENARIO1 = "Snow Course Scenario 1"
    Public Const BA_MAPS_SNOW_COURSE_SCENARIO2 = "Snow Course Scenario 2"
    Public Const BA_MAPS_PSEUDO_SCENARIO1 = "Pseudo Site Scenario 1"
    Public Const BA_MAPS_PSEUDO_SCENARIO2 = "Pseudo Site Scenario 2"
    Public Const BA_MAPS_FILLED_DEM = "Filled DEM"
    Public Const BA_MAPS_NOT_REPRESENTED = "Not Represented"

    'these constants are used to ID whether a folder is a basin or an AOI or both
    Public Const BA_BASIN_DEM_EXTENT_SHAPEFILE As String = "aoi_v" 'vector

    'used when name cannot be determined for layer
    Public Const BA_UNKNOWN As String = "Unknown"

    'file names for hru files
    Public Const GRID As String = "grid"

    'constants for hru log display
    Public Const YES As String = "Yes"
    Public Const NO As String = "No"

    'maximum aoi file path length
    'ArcInfo doesn't allow the path string to exceed 128 char and a workspace
    'path to exceed 115 char."
    Public Const MAX_AOI_PATH_LENGTH As Integer = 115

    'maximum file length for conversion progress bar message
    Public Const MAX_PROGRESSOR_MSG_PATH_LENGTH As Integer = 53

    'cookie-cut mode
    Public Const BA_MODE_STAMP As String = "Stamp"
    Public Const BA_MODE_COOKIE_CUT As String = "Cookie-cut"

    'path variables
    Public Const BA_PATH_SETTINGS As String = "BAGIS"

    'SaveAs raster file formats
    Public Const BA_RASTER_FORMAT As String = "GDB"

    'NoData constant for reclass
    Public Const BA_NODATA As String = "NoData"
    Public Const BA_9999 As Integer = -9999

    'Constants for file extensions
    Public Const BA_FILE_EXT_XML As String = ".xml"
    Public Const BA_FILE_EXT_TOOLBOX As String = ".tbx"
    Public Const BA_FILE_EXT_TEXT As String = ".txt"

    'Constant for BAGIS-P system-generated parameters prefix
    Public Const BA_PARAM_PREFIX As String = "sys_"
    Public Const BA_FIELD_PREFIX As String = "fld_"
    Public Const BA_DATABIN_PREFIX As String = "db_"
    Public Const BA_PARAM_TABLE_SUFFIX As String = "_params"

    'Constant for BAGIS-P screen mode
    Public Const BA_BAGISP_MODE_PUBLIC As String = "public"
    Public Const BA_BAGISP_MODE_LOCAL As String = "local"

    'XPath constants to write to metadata
    Public Const BA_XPATH_TAGS As String = "/metadata/dataIdInfo/searchKeys/keyword"
    Public Const BA_XPATH_METSTDN As String = "/metadata/metainfo/metstdn"
    Public Const BA_BAGIS_TAG_PREFIX As String = "BAGIS Tag < Please do not modify: "
    Public Const BA_BAGIS_TAG_SUFFIX As String = " > End Tag"
    Public Const BA_ZUNIT_CATEGORY_TAG As String = "ZUnitCategory|"
    Public Const BA_ZUNIT_VALUE_TAG As String = "ZUnit|"

    'Excel chart
    Public Const BA_ChartWidth = 600
    Public Const BA_ChartHeight = 330
    Public Const BA_ChartSpacing = 5

    'Resampling constants
    Public Const BA_Resample_Nearest = "NEAREST"
    Public Const BA_Resample_Bilinear = "BILINEAR"
    Public Const BA_Resample_Cubic = "CUBIC"
    Public Const BA_Resample_Majority = "MAJORITY"
End Module



