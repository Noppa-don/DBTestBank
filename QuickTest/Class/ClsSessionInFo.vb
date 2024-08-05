Imports System.ComponentModel

Public Class ClsSessionInFo

    Private _PKInfo As Integer
    Public Property PKInfo As Integer
        Get
            PKInfo = _PKInfo
        End Get
        Set(ByVal value As Integer)
            _PKInfo = value
        End Set
    End Property


    Private _Index As Integer
    Public Property Index As Integer
        Get
            Index = _Index
        End Get
        Set(ByVal value As Integer)
            _Index = value
        End Set
    End Property

    Private _TimeStamp As DateTime
    Public Property TimeStamp As DateTime
        Get
            TimeStamp = _TimeStamp
        End Get
        Set(ByVal value As DateTime)
            _TimeStamp = value
        End Set
    End Property

    Private _TestSetName As String
    Public Property TestSetName As String
        Get
            If _TestSetName Is Nothing Then
                _TestSetName = ""
            End If
            TestSetName = _TestSetName
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then
                value = ""
            End If
            _TestSetName = value
        End Set
    End Property

    Private _TestSetTime As String
    Public Property TestSetTime As String
        Get
            If _TestSetTime Is Nothing Then
                _TestSetTime = ""
            End If
            TestSetTime = _TestSetTime
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then
                value = ""
            End If
            _TestSetTime = value
        End Set
    End Property

    Private _ClassName As String
    Public Property ClassName As String
        Get
            ClassName = _ClassName
        End Get
        Set(ByVal value As String)
            _ClassName = value
        End Set
    End Property

    Private _CurrentPage As String
    Public Property CurrentPage As String
        Get
            CurrentPage = _CurrentPage
        End Get
        Set(ByVal value As String)
            _CurrentPage = value
        End Set
    End Property
    ' QUERYSTRING
    Private _CurrentQuerystring As String
    Public Property CurrentQuerystring As String
        Get
            CurrentQuerystring = _CurrentQuerystring
        End Get
        Set(ByVal value As String)
            _CurrentQuerystring = value
        End Set
    End Property

    Private _LabName As String
    Public Property LabName As String
        Get
            If _LabName Is Nothing Then
                _LabName = ""
            End If
            LabName = _LabName
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then
                value = ""
            End If
            _LabName = value
        End Set
    End Property

    Private _TestsetId As String = ""
    <Bindable(True), DefaultValue("")> _
    Public Property TestsetId As String
        Get
            TestsetId = _TestsetId
        End Get
        Set(ByVal value As String)
            _TestsetId = value
        End Set
    End Property

    Private _EditId As String
    Public Property EditId As String
        Get
            EditId = _EditId
        End Get
        Set(ByVal value As String)
            _EditId = value
        End Set
    End Property

    Private _objTestset As Object
    Public Property objTestset As Object
        Get
            objTestset = _objTestset
        End Get
        Set(ByVal value As Object)
            _objTestset = value
        End Set
    End Property

    Private _OutputFileName As String
    Public Property OutputFileName As String
        Get
            OutputFileName = _OutputFileName
        End Get
        Set(ByVal value As String)
            _OutputFileName = value
        End Set
    End Property

    Private _QuizId As String
    Public Property QuizId As String
        Get
            QuizId = _QuizId
        End Get
        Set(ByVal value As String)
            _QuizId = value
        End Set
    End Property

    Private _SchoolId As String
    Public Property SchoolId As String
        Get
            SchoolId = _SchoolId
        End Get
        Set(ByVal value As String)
            _SchoolId = value
        End Set
    End Property

    Private _OnUnload As Boolean
    Public Property OnUnload As Boolean
        Get
            OnUnload = _OnUnload
        End Get
        Set(ByVal value As Boolean)
            _OnUnload = value
        End Set
    End Property

    ' SESSION ChooseMode
    Private _ChooseMode As String
    Public Property ChooseMode As String
        Get
            ChooseMode = _ChooseMode
        End Get
        Set(ByVal value As String)
            _ChooseMode = value
        End Set
    End Property

    ' SESSION QuizUseTablet
    Private _QuizUseTablet As Boolean
    Public Property QuizUseTablet As Boolean
        Get
            QuizUseTablet = _QuizUseTablet
        End Get
        Set(ByVal value As Boolean)
            _QuizUseTablet = value
        End Set
    End Property

    ' SESSION PClassId // ตอนเลือกชุดแบบมาตรฐาน
    Private _PClassId As String
    Public Property PClassId As String
        Get
            PClassId = _PClassId
        End Get
        Set(ByVal value As String)
            _PClassId = value
        End Set
    End Property

    ' SESSION PClassId // ตอนเลือกชุดแบบมาตรฐาน
    Private _PSubjectName As String
    Public Property PSubjectName As String
        Get
            PSubjectName = _PSubjectName
        End Get
        Set(ByVal value As String)
            _PSubjectName = value
        End Set
    End Property

    ' EXAMNUM
    Private _ExamNum As Integer
    Public Property ExamNum As Integer
        Get
            ExamNum = _ExamNum
        End Get
        Set(ByVal value As Integer)
            _ExamNum = value
        End Set
    End Property

    Private _OnRecieved As String
    Public Property OnRecieved As String
        Get
            OnRecieved = _OnRecieved
        End Get
        Set(ByVal value As String)
            _OnRecieved = value
        End Set
    End Property

    'Private IsStartTimer As Boolean
    'Public Property StarTimer() As Boolean
    '    Get
    '        Return IsStartTimer
    '    End Get
    '    Set(ByVal value As Boolean)
    '        IsStartTimer = value
    '    End Set
    'End Property

    'Private t As System.Timers.Timer
    'Public Property timerQuiz() As System.Timers.Timer
    '    Get
    '        Return t
    '    End Get
    '    Set(ByVal value As System.Timers.Timer)
    '        t = value
    '    End Set
    'End Property
End Class
