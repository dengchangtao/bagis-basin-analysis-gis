Imports System.ComponentModel

'Used in template rules. Each template rule has a collection of actions defined by the
'ActionType enum. Each action has a set of parameters defined by this enum. These keys allow
'the parameters to be retrieved when the rule is run and recorded in the process log.
Public Enum ActionParameter

    IterationCount
    RectangleWidth
    RectangleHeight
    ReclassItems
    Directions
    SlopeClassifyDescr
    SlopeOptions
    SlopeUnits
    LandUseDescr
    InputLayer
    ReclassField
    LandUseOptions
    CanopyClassifyDescr
    CanopyOptions
End Enum