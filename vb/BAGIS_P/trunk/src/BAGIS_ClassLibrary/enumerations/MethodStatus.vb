'Imports System.ComponentModel

' Indicates the status of a BAGIS-P method
Public Enum MethodStatus

    Missing     'Method has no status
    Pending     'Method has been added but not validated
    Verified    'Method verified and ready-to-run
    Invalid     'Method couldn't be verified
    Complete    'Method has run successfully
    Failed      'Method failed when it ran

End Enum
