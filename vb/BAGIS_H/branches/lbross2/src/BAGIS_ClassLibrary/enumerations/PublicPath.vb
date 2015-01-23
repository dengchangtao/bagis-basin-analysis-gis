Imports System.ComponentModel

Public Enum PublicPath

    'weasel folder structure
    <Description("\output\surfaces\dem\filled")> DEM
    <Description("\output\surfaces\dem")> SourceDEM
    <Description("\output\surfaces\dem\filled\flow-direction")> FlowDirection
    <Description("\output\surfaces\dem\filled\flow-direction\flow-accumulation")> FlowAccumulation
    <Description("\output\surfaces\dem\filled\slope")> Slope
    <Description("\output\surfaces\dem\filled\aspect")> Aspect
    'AOI folder structure
    <Description("\maps")> Maps
    <Description("\tables")> Tables
    <Description("\layers")> Layers
    <Description("\analysis")> Analysis
    <Description("\layers\PRISM")> PRISM
    <Description("\output\surfaces\dem\filled\hillshade")> Hillshade
    <Description("\layers\cov_den")> Canopy
    'HRU folder structure
    <Description("\output")> Output
    '<Description("\zones")> Zones
    <Description("\zones")> HruDirectory
    <Description("\output\zones")> HruWeaselDirectory
    <Description("\grid")> HruGrid
    <Description("\grid_v.shp")> HruVector
    <Description("\polygrid_v.shp")> HruPolyVector
    <Description("\log.xml")> HruXml
    <Description("\aoi_v.shp")> AoiVector
    <Description("\aoibagis")> AoiGrid
    <Description("\templates.xml")> Templates
    <Description("\templates_old.xml")> OldTemplates
    <Description("\data_descriptor.xml")> DataDescriptor
    <Description("\rules.xml")> RulesXml
    <Description("\log.html")> HruHtmlFile
    <Description("\BAGIS\settings.xml")> BagisPSettings
    <Description("\BAGIS\methods")> BagisPMethods
    <Description("\BAGIS\profiles")> BagisPProfiles
    <Description("\param")> BagisParamFolder
    <Description("\param.gdb")> BagisParamGdb
    <Description("\paramdata.gdb")> BagisDataBinGdb
    <Description("\param\profiles")> BagisLocalProfiles
    <Description("\param\methods")> BagisLocalMethods
    <Description("\param\settings.xml")> BagisLocalSettings
    <Description("\basinanalyst_addins.def")> BagisSettingFilename
    <Description("\Default.gdb")> BagisPDefaultWorkspace
    <Description("\aoi")> AoiGridWeasel
    <Description("\aoib")> AoiBufferedGrid
    <Description("\p_aoi")> AoiPrismGrid
    <Description("\HruProfileStatus.xml")> HruProfileStatus
    <Description("\grid_zones_v")> HruZonesVector
    <Description("\aoi_streams")> AoiStreamsVector
    <Description("\BAGIS\parameter_descriptions.txt")> BagisPParameterDescriptions
    <Description("\subaoi.gdb")> BagisSubAoiGdb
    <Description("\analysis.xml")> AnalysisXml
    <Description("\Analysis.xsl")> AnalysisXsl
    <Description("\scenario_report.html")> ScenarioReportHtml
    <Description("\bagis_parameters.txt")> BagisParameters
    <Description("\ExportProfile.xsl")> ExportProfileXsl
    <Description("\ExportProfile.xml")> ExportProfileXml
    <Description("\export_report.html")> ExportReportHtml
    <Description("\BAGIS_Parameters.csv")> DefaultParameterTemplate
End Enum
