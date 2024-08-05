Public Class ClsSessApp

    Dim IsGUID As New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)

    Private _QuizId As String
    Public Property QuizId As String 'เก็บ QuizID เช็คว่าเป็น GUID หรือเปล่า
        Get
            QuizId = _QuizId
            Return QuizId
        End Get
        Set(ByVal value As String)
            If IsGUID.IsMatch(value) Then
                _QuizId = value
            Else
                Throw New FormatException("Value ต้องเป็น GUID เท่านั้น")
            End If
        End Set
    End Property

    Private _CurrentPlayerId As String
    Public Property CurrentPlayerId As String 'เก็บ Student_Id 
        Get
            CurrentPlayerId = _CurrentPlayerId
            Return CurrentPlayerId
        End Get
        Set(ByVal value As String)
            If IsGUID.IsMatch(value) Then
                _CurrentPlayerId = value
            Else
                Throw New FormatException("Value ต้องเป็น GUID เท่านั้น")
            End If
        End Set
    End Property

    Private _IsUpdateCheckTablet As String
    Public Property IsUpdateCheckTablet As String 'ต้องเช็คก่อนจะเก็บค่าต้องเป็นคำว่า 1 เท่านั้น
        Get
            IsUpdateCheckTablet = _IsUpdateCheckTablet
            Return IsUpdateCheckTablet
        End Get
        Set(ByVal value As String)
            If value = "1" Then
                _IsUpdateCheckTablet = value
            Else
                Throw New FormatException("Value ต้องเป็นคำว่า 1 เท่านั้น")
            End If
        End Set
    End Property

    Private ExmanNum As Integer
    Public Property _ExmanNum As Integer 'เช็คก่อนว่าเลขข้อล่าสุดที่ส่งมาต้องไม่เป็นเลข 0
        Get
            _ExmanNum = ExmanNum
            Return _ExmanNum
        End Get
        Set(ByVal value As Integer)
            If value <> 0 Then
                ExmanNum = value
            Else
                Throw New FormatException("ต้องมีค่ามากกว่า 0 เท่านั้น")
            End If
        End Set
    End Property

    Private _CurrentAnsState As String
    Public Property CurrentAnsState As String 'ต้องเช็คก่อนว่า State ที่ไว้เช็คว่าตอนนี้เป็นเฉลยหรือเปล่านั้นต้องเป็นข้อความตัวเลขระหว่าง 0-2 เท่านั้น
        Get
            CurrentAnsState = _CurrentAnsState
            Return CurrentAnsState
        End Get
        Set(ByVal value As String)
            If value = "0" Or value = "1" Or value = "2" Then
                _CurrentAnsState = value
            Else
                Throw New FormatException("ต้องมีค่าเป็น สตริง 0 1 หรือ 2 เท่านั้น")
            End If
        End Set
    End Property

    Private _IsUseDeviceId As Boolean
    Public Property IsUseDeviceId As Boolean
        Get
            IsUseDeviceId = _IsUseDeviceId
            Return IsUseDeviceId
        End Get
        Set(ByVal value As Boolean)
            If TypeName(value) <> "Boolean" Then
                Throw New FormatException("ต้องเป็นตัวแปรชนิด Boolean เท่านั้น")
            Else
                _IsUseDeviceId = value
            End If
        End Set
    End Property



End Class
