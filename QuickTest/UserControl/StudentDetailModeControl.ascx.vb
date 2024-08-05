Public Class StudentDetailModeControl
    Inherits System.Web.UI.UserControl

    Private _FilterMode As FilterMode
    Public Property FilterMode() As FilterMode
        Get
            Return _FilterMode
        End Get
        Set(ByVal value As FilterMode)
            _FilterMode = value
        End Set
    End Property
    Private _StartTime As Date
    Public Property StartTime() As Date
        Get
            Return _StartTime
        End Get
        Set(ByVal value As Date)
            _StartTime = value
        End Set
    End Property
    Private _EndTime As Date
    Public Property EndTime() As Date
        Get
            Return _EndTime
        End Get
        Set(ByVal value As Date)
            _EndTime = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _FilterMode = QuickTest.FilterMode.Now Then
            MsgBox("121345643")
        End If
    End Sub

End Class

Public Enum FilterMode As Integer
    Now = 1
    SevenDaysMinus = 2
    HalfMonthMinus = 3
    MonthMinus = 4
    TermMinus = 5
    RangeDateMinus = 6
End Enum
