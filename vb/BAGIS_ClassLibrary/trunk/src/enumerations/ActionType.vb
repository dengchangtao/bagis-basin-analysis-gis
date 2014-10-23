Imports System.ComponentModel

'Used in template rules. Each template rule has a collection of actions defined in this 
'ActionType enum. Each action has supporting code to execute it.
Public Enum ActionType

    <Description("Majority filter")> MajorityFilter
    <Description("Convert to raster")> Rasterize
    <Description("Reclass (continuous data)")> ReclCont    'Aspect, canopy, slope
    <Description("Low pass Filter")> LowPassFilter
    <Description("Reclass (discrete data)")> ReclDisc    'LULC
End Enum