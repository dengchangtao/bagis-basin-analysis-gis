Imports BAGIS_ClassLibrary

Public Class AreaStatistics

    Dim m_s1RepArea As Double
    Dim m_s1AoiPctRep As String
    Dim m_s1RepAreaSqMi As String
    Dim m_s1RepAreaAcres As String
    Dim m_s1RepAreaSqKm As String
    Dim m_s1RepAreaHect As String
    Dim m_s1NonRepArea As Double
    Dim m_s1AoiPctNonRep As String
    Dim m_s1NonRepAreaSqMi As String
    Dim m_s1NonRepAreaAcres As String
    Dim m_s1NonRepAreaSqKm As String
    Dim m_s1NonRepAreaHect As String
    Dim m_s2RepArea As Double
    Dim m_s2AoiPctRep As String
    Dim m_s2RepAreaSqMi As String
    Dim m_s2RepAreaAcres As String
    Dim m_s2RepAreaSqKm As String
    Dim m_s2RepAreaHect As String
    Dim m_s2NonRepArea As Double
    Dim m_s2AoiPctNonRep As String
    Dim m_s2NonRepAreaSqMi As String
    Dim m_s2NonRepAreaAcres As String
    Dim m_s2NonRepAreaSqKm As String
    Dim m_s2NonRepAreaHect As String
    Dim m_mapNotRep As Double
    Dim m_mapRepS1Only As Double
    Dim m_mapRepS2Only As Double
    Dim m_mapRepBothScen As Double
    Dim m_mapAoiPctNotRep As String
    Dim m_mapAoiPctRepS1Only As String
    Dim m_mapAoiPctRepS2Only As String
    Dim m_mapAoiPctRepBothScen As String

    Sub New()
        MyBase.New()
    End Sub

    Public Property S1RepArea() As Double
        Get
            Return m_s1RepArea
        End Get
        Set(value As Double)
            m_s1RepArea = value
        End Set
    End Property

    Public Property S1AoiPctRep As String
        Get
            Return m_s1AoiPctRep
        End Get
        Set(value As String)
            m_s1AoiPctRep = value
        End Set
    End Property

    Public Property S1RepAreaSqMi As String
        Get
            Return m_s1RepAreaSqMi
        End Get
        Set(value As String)
            m_s1RepAreaSqMi = value
        End Set
    End Property

    Public Property S1RepAreaAcres As String
        Get
            Return m_s1RepAreaAcres
        End Get
        Set(value As String)
            m_s1RepAreaAcres = value
        End Set
    End Property

    Public Property S1RepAreaSqKm As String
        Get
            Return m_s1RepAreaSqKm
        End Get
        Set(value As String)
            m_s1RepAreaSqKm = value
        End Set
    End Property

    Public Property S1RepAreaHect As String
        Get
            Return m_s1RepAreaHect
        End Get
        Set(value As String)
            m_s1RepAreaHect = value
        End Set
    End Property

    Public Property S1NonRepArea() As Double
        Get
            Return m_s1NonRepArea
        End Get
        Set(value As Double)
            m_s1NonRepArea = value
        End Set
    End Property

    Public Property S1AoiPctNonRep As String
        Get
            Return m_s1AoiPctNonRep
        End Get
        Set(value As String)
            m_s1AoiPctNonRep = value
        End Set
    End Property

    Public Property S1NonRepAreaSqMi As String
        Get
            Return m_s1NonRepAreaSqMi
        End Get
        Set(value As String)
            m_s1NonRepAreaSqMi = value
        End Set
    End Property

    Public Property S1NonRepAreaAcres As String
        Get
            Return m_s1NonRepAreaAcres
        End Get
        Set(value As String)
            m_s1NonRepAreaAcres = value
        End Set
    End Property

    Public Property S1NonRepAreaSqKm As String
        Get
            Return m_s1NonRepAreaSqKm
        End Get
        Set(value As String)
            m_s1NonRepAreaSqKm = value
        End Set
    End Property

    Public Property S1NonRepAreaHect As String
        Get
            Return m_s1NonRepAreaHect
        End Get
        Set(value As String)
            m_s1NonRepAreaHect = value
        End Set
    End Property

    Public Property S2RepArea() As Double
        Get
            Return m_s2RepArea
        End Get
        Set(value As Double)
            m_s2RepArea = value
        End Set
    End Property

    Public Property S2AoiPctRep As String
        Get
            Return m_s2AoiPctRep
        End Get
        Set(value As String)
            m_s2AoiPctRep = value
        End Set
    End Property

    Public Property S2RepAreaSqMi As String
        Get
            Return m_s2RepAreaSqMi
        End Get
        Set(value As String)
            m_s2RepAreaSqMi = value
        End Set
    End Property

    Public Property S2RepAreaAcres As String
        Get
            Return m_s2RepAreaAcres
        End Get
        Set(value As String)
            m_s2RepAreaAcres = value
        End Set
    End Property

    Public Property S2RepAreaSqKm As String
        Get
            Return m_s2RepAreaSqKm
        End Get
        Set(value As String)
            m_s2RepAreaSqKm = value
        End Set
    End Property

    Public Property S2RepAreaHect As String
        Get
            Return m_s2RepAreaHect
        End Get
        Set(value As String)
            m_s2RepAreaHect = value
        End Set
    End Property

    Public Property S2NonRepArea() As Double
        Get
            Return m_s2NonRepArea
        End Get
        Set(value As Double)
            m_s2NonRepArea = value
        End Set
    End Property

    Public Property S2AoiPctNonRep As String
        Get
            Return m_s2AoiPctNonRep
        End Get
        Set(value As String)
            m_s2AoiPctNonRep = value
        End Set
    End Property

    Public Property S2NonRepAreaSqMi As String
        Get
            Return m_s2NonRepAreaSqMi
        End Get
        Set(value As String)
            m_s2NonRepAreaSqMi = value
        End Set
    End Property

    Public Property S2NonRepAreaAcres As String
        Get
            Return m_s2NonRepAreaAcres
        End Get
        Set(value As String)
            m_s2NonRepAreaAcres = value
        End Set
    End Property

    Public Property S2NonRepAreaSqKm As String
        Get
            Return m_s2NonRepAreaSqKm
        End Get
        Set(value As String)
            m_s2NonRepAreaSqKm = value
        End Set
    End Property

    Public Property S2NonRepAreaHect As String
        Get
            Return m_s2NonRepAreaHect
        End Get
        Set(value As String)
            m_s2NonRepAreaHect = value
        End Set
    End Property

    Public Property DiffNonRepSqKm As String
        Get
            Dim diff As Double = Convert.ToDouble(S1NonRepAreaSqKm) - Convert.ToDouble(S2NonRepAreaSqKm)
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffRepSqKm As String
        Get
            Dim diff As Double = Convert.ToDouble(S1RepAreaSqKm) - Convert.ToDouble(S2RepAreaSqKm)
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffNonRepSqMi As String
        Get
            Dim diff As Double = Convert.ToDouble(S1NonRepAreaSqMi) - Convert.ToDouble(S2NonRepAreaSqMi)
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffRepSqMi As String
        Get
            Dim diff As Double = Convert.ToDouble(S1RepAreaSqMi) - Convert.ToDouble(S2RepAreaSqMi)
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffNonRepAcres As String
        Get
            Dim diff As Double = Convert.ToDouble(S1NonRepAreaAcres) - Convert.ToDouble(S2NonRepAreaAcres)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffRepAcres As String
        Get
            Dim diff As Double = Convert.ToDouble(S1RepAreaAcres) - Convert.ToDouble(S2RepAreaAcres)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffNonRepHect As String
        Get
            Dim diff As Double = Convert.ToDouble(S1NonRepAreaHect) - Convert.ToDouble(S2NonRepAreaHect)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffRepHect As String
        Get
            Dim diff As Double = Convert.ToDouble(S1RepAreaHect) - Convert.ToDouble(S2RepAreaHect)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffAoiPctNonRep As String
        Get
            Dim diff As Double = Convert.ToDouble(S1AoiPctNonRep) - Convert.ToDouble(S2AoiPctNonRep)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property DiffAoiPctRep As String
        Get
            Dim diff As Double = Convert.ToDouble(S1AoiPctRep) - Convert.ToDouble(S2AoiPctRep)
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapNotRep() As Double
        Get
            Return m_mapNotRep
        End Get
        Set(value As Double)
            m_mapNotRep = value
        End Set
    End Property

    Public Property MapRepS1Only() As Double
        Get
            Return m_mapRepS1Only
        End Get
        Set(value As Double)
            m_mapRepS1Only = value
        End Set
    End Property

    Public Property MapRepS2Only() As Double
        Get
            Return m_mapRepS2Only
        End Get
        Set(value As Double)
            m_mapRepS2Only = value
        End Set
    End Property

    Public Property MapRepBothScen() As Double
        Get
            Return m_mapRepBothScen
        End Get
        Set(value As Double)
            m_mapRepBothScen = value
        End Set
    End Property

    Public Property MapNotRepSqKm() As String
        Get
            Return Format(Math.Round(m_mapNotRep, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapNotRepSqMi() As String
        Get
            Dim diff As Double = m_mapNotRep * BA_SQKm_To_SQMile
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapNotRepAcres() As String
        Get
            Dim diff As Double = m_mapNotRep * BA_SQKm_To_ACRE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapNotRepHect() As String
        Get
            Dim diff As Double = m_mapNotRep * BA_SQKm_To_HECTARE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepBothScenSqKm() As String
        Get
            Return Format(Math.Round(m_mapRepBothScen, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepBothScenSqMi() As String
        Get
            Dim diff As Double = m_mapRepBothScen * BA_SQKm_To_SQMile
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepBothScenAcres() As String
        Get
            Dim diff As Double = m_mapRepBothScen * BA_SQKm_To_ACRE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepBothScenHect() As String
        Get
            Dim diff As Double = m_mapRepBothScen * BA_SQKm_To_HECTARE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS1OnlySqKm() As String
        Get
            Return Format(Math.Round(m_mapRepS1Only, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS1OnlySqMi() As String
        Get
            Dim diff As Double = m_mapRepS1Only * BA_SQKm_To_SQMile
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS1OnlyAcres() As String
        Get
            Dim diff As Double = m_mapRepS1Only * BA_SQKm_To_ACRE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS1OnlyHect() As String
        Get
            Dim diff As Double = m_mapRepS1Only * BA_SQKm_To_HECTARE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS2OnlySqKm() As String
        Get
            Return Format(Math.Round(m_mapRepS2Only), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS2OnlySqMi() As String
        Get
            Dim diff As Double = m_mapRepS2Only * BA_SQKm_To_SQMile
            Return Format(Math.Round(diff, 3), "#0.000")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS2OnlyAcres() As String
        Get
            Dim diff As Double = m_mapRepS2Only * BA_SQKm_To_ACRE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapRepS2OnlyHect() As String
        Get
            Dim diff As Double = m_mapRepS2Only * BA_SQKm_To_HECTARE
            Return Format(Math.Round(diff, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MapAoiPctNotRep As String
        Get
            Return m_mapAoiPctNotRep
        End Get
        Set(value As String)
            m_mapAoiPctNotRep = value
        End Set
    End Property

    Public Property MapAoiRepBothScen As String
        Get
            Return m_mapAoiPctRepBothScen
        End Get
        Set(value As String)
            m_mapAoiPctRepBothScen = value
        End Set
    End Property

    Public Property MapAoiRepS1Only As String
        Get
            Return m_mapAoiPctRepS1Only
        End Get
        Set(value As String)
            m_mapAoiPctRepS1Only = value
        End Set
    End Property

    Public Property MapAoiRepS2Only As String
        Get
            Return m_mapAoiPctRepS2Only
        End Get
        Set(value As String)
            m_mapAoiPctRepS2Only = value
        End Set
    End Property

End Class
