Imports System.ComponentModel

'List of success/failure codes returned by BAGIS functions
Public Enum BA_ReturnCode

    Success
    UnknownError
    NotSupportedOperation
    ReadError
    WriteError
    OtherError

End Enum