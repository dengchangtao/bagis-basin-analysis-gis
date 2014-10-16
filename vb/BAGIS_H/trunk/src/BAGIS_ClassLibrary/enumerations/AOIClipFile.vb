Imports System.ComponentModel

'Stores file names by key for files that are used for clipping
Public Enum AOIClipFile

    <Description("aoi_v")> AOIExtentCoverage
    <Description("aoib_v")> BufferedAOIExtentCoverage
    <Description("p_aoi_v")> PrismClipAOIExtentCoverage

End Enum
