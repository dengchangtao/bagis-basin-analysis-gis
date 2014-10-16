Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary

Public Interface IRule

    'Unique Id for the rule
    'Ex: 1
    Property RuleId() As Integer
    'Display name for input layer
    'Ex: Aspect
    Property InputLayerName() As String
    'Complete file path to input layer
    'Ex: C:\Docs\Lesley\UCO_RioG_SantaFe_R_nr_SantaFe_092010\output\surfaces\dem\filled\aspect\grid
    Property InputFolderPath() As String
    'Name of output dataset
    'Ex: r001
    Property OutputDatasetName() As String
    'Text value for type of rule; Matches entry in dropdown on HRU zone creation form
    'Ex: Template - Aspect
    Property RuleTypeText() As String
    'Is the rule defined correctly?
    'Ex: true
    Property Defined() As Boolean
    'Has the rule been run?
    'Ex: Complete
    Property FactorStatus() As FactorStatus

End Interface
