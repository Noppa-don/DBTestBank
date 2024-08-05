Public Class ClsSumUsageReport

    Private Property _UserName As String
    Public Property UserName As String
        Get
            UserName = _UserName
        End Get
        Set(value As String)
            _UserName = value
        End Set
    End Property

    Private Property _TotalLogIn As String
    Public Property TotalLogIn As String
        Get
            TotalLogIn = _TotalLogIn
        End Get
        Set(value As String)
            _TotalLogIn = value
        End Set
    End Property

    Private Property _TotalSpentTime As String
    Public Property TotalSpentTime As String
        Get
            TotalSpentTime = _TotalSpentTime
        End Get
        Set(value As String)
            _TotalSpentTime = value
        End Set
    End Property

    Private Property _AverageTime As String
    Public Property AverageTime As String
        Get
            AverageTime = _AverageTime
        End Get
        Set(value As String)
            _AverageTime = value
        End Set
    End Property

    Private Property _MinTime As String
    Public Property MinTime As String
        Get
            MinTime = _MinTime
        End Get
        Set(value As String)
            _MinTime = value
        End Set
    End Property

    Private Property _MaxTime As String
    Public Property MaxTime As String
        Get
            MaxTime = _MaxTime
        End Get
        Set(value As String)
            _MaxTime = value
        End Set
    End Property

    Private Property _ZeroToFive_Minute As String
    Public Property ZeroToFive_Minute As String
        Get
            ZeroToFive_Minute = _ZeroToFive_Minute
        End Get
        Set(value As String)
            _ZeroToFive_Minute = value
        End Set
    End Property

    Private Property _FiveToTen_Minute As String
    Public Property FiveToTen_Minute As String
        Get
            FiveToTen_Minute = _FiveToTen_Minute
        End Get
        Set(value As String)
            _FiveToTen_Minute = value
        End Set
    End Property

    Private Property _TenToFifteen_Minute As String
    Public Property TenToFifteen_Minute As String
        Get
            TenToFifteen_Minute = _TenToFifteen_Minute
        End Get
        Set(value As String)
            _TenToFifteen_Minute = value
        End Set
    End Property

    Private Property _FifteenToThirty_Minute As String
    Public Property FifteenToThirty_Minute As String
        Get
            FifteenToThirty_Minute = _FifteenToThirty_Minute
        End Get
        Set(value As String)
            _FifteenToThirty_Minute = value
        End Set
    End Property

    Private Property _ThirtyPlus_Minute As String
    Public Property ThirtyPlus_Minute As String
        Get
            ThirtyPlus_Minute = _ThirtyPlus_Minute
        End Get
        Set(value As String)
            _ThirtyPlus_Minute = value
        End Set
    End Property

End Class
