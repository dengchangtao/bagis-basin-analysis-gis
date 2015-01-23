Imports System.ComponentModel

' Note: order of this enum must be kept in sync with FrmHruZone.CboRuleType items
Public Enum HruRuleType

    <Description("Contributing Area")> ContributingArea
    <Description("DA Flow Type Zones")> DAFlowTypeZones
    <Description("Prism Precipitation")> PrismPrecipitation
    <Description("Raster Reclass (Continuous data)")> RasterReclassContinuous
    <Description("Raster Reclass (Discrete data)")> RasterReclassification
    <Description("Raster Slice")> RasterSlices
    <Description("Template - Aspect")> Aspect
    <Description("Template - Canopy")> Canopy
    <Description("Template - Land Use")> LandUse
    <Description("Template - Slope")> Slope
End Enum