Public Class Quiz

    Private _QuizId As String
    Public Property QuizId() As String
        Get
            Return _QuizId
        End Get
        Set(ByVal value As String)
            _QuizId = value
        End Set
    End Property

    Private _TestsetId As String
    Public Property TestsetId() As String
        Get
            Return _TestsetId
        End Get
        Set(ByVal value As String)
            _TestsetId = value
        End Set
    End Property


    Private _IsSelfPace As Boolean
    Public Property IsSelfPace() As String
        Get
            Return _IsSelfPace
        End Get
        Set(ByVal value As String)
            _IsSelfPace = value
        End Set
    End Property

    Private _Examnum As String
    Public Property Examnum() As String
        Get
            If _Examnum Is Nothing OrElse _Examnum = "" Then
                Return "-1"
            Else
                Return _Examnum
            End If
        End Get
        Set(ByVal value As String)
            _Examnum = value
        End Set
    End Property

    Private _AmountQuestion As String
    Public Property AmountQuestion() As String
        Get
            Return _AmountQuestion
        End Get
        Set(ByVal value As String)
            _AmountQuestion = value
        End Set
    End Property

    Private _AnswerState As String
    Public Property AnswerState() As String
        Get
            If _AnswerState Is Nothing OrElse _AnswerState = "" Then
                Return "-1"
            Else
                Return _AnswerState
            End If
        End Get
        Set(ByVal value As String)
            _AnswerState = value
        End Set
    End Property

    Private _IsLab As Boolean
    Public Property IsLab() As Boolean
        Get
            Return _IsLab
        End Get
        Set(ByVal value As Boolean)
            _IsLab = value
        End Set
    End Property

#Region "เฉพาะกิจ - OOP In Future"
    Private _NoOfDone As Dictionary(Of String, Integer) ' ข้อที่ตอบไปแล้วใน quiz
    Public Property NoOfDone() As Dictionary(Of String, Integer)
        Get
            Return _NoOfDone
        End Get
        Set(ByVal value As Dictionary(Of String, Integer))
            _NoOfDone = value
        End Set
    End Property

    Private _PlayerId As String ' id ของ student ที่ทำการตอบ
    Public Property PlayerId() As String
        Get
            Return _PlayerId
        End Get
        Set(ByVal value As String)
            _PlayerId = value
        End Set
    End Property

    Private _CheckIn As Boolean
    Public Property CheckIn() As Boolean
        Get
            Return _CheckIn
        End Get
        Set(ByVal value As Boolean)
            _CheckIn = value
        End Set
    End Property

#End Region
End Class

Public Class NowPlayer
    Private _NowDevice As String
    Public Property NowDevice() As String
        Get
            Return _NowDevice
        End Get
        Set(ByVal value As String)
            _NowDevice = value
        End Set
    End Property
End Class
