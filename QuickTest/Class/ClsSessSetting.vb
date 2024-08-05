Public Class ClsSessSetting

    Dim IsGUID As New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)


    'Private _Quiz_Id As String 'เก็บค่า Quiz_Id
    'Public Property Quiz_Id As String
    '    Get
    '        Quiz_Id = _Quiz_Id
    '    End Get
    '    Set(ByVal value As String)
    '        If IsGUID.IsMatch(value) Then
    '            _Quiz_Id = value
    '        Else
    '            Throw New FormatException("Value ต้องเป็น GUID เท่านั้น")
    '        End If
    '    End Set
    'End Property

    Private _NeedShowScore As Boolean 'เก็บค่าว่าให้แสดงคะแนนหรือเปล่า
    Public Property NeedShowScore As Boolean
        Get
            NeedShowScore = _NeedShowScore
            Return NeedShowScore
        End Get
        Set(ByVal value As Boolean)
            _NeedShowScore = value
        End Set
    End Property

    Private _NeedShowScoreAfterComplete As Boolean 'เก็บค่าว่าให้แสดงคะแนนแบบข้อต่อข้อหรือเปล่า
    Public Property NeedShowScoreAfterComplete As Boolean
        Get
            NeedShowScoreAfterComplete = _NeedShowScoreAfterComplete
        End Get
        Set(ByVal value As Boolean)
            _NeedShowScoreAfterComplete = value
        End Set
    End Property

    Private _IsDifferentQuestion As Boolean 'เก็บค่าว่าคำถามเหมือนกันหรือเปล่า
    Public Property IsDifferentQuestion As Boolean
        Get
            IsDifferentQuestion = _IsDifferentQuestion
        End Get
        Set(ByVal value As Boolean)
            _IsDifferentQuestion = value
        End Set
    End Property

    Private _IsDifferentAnswer As Boolean 'เก็บค่าว่าคำตอบเหมือนกันหรือเปล่า
    Public Property IsDifferentAnswer As Boolean
        Get
            IsDifferentAnswer = _IsDifferentAnswer
        End Get
        Set(ByVal value As Boolean)
            _IsDifferentAnswer = value
        End Set
    End Property

    Private _SelfPace As Boolean 'เก็บค่าว่าเป็่นโหมดแบบทำไม่พร้อมกันหรือเปล่า
    Public Property SelfPace As Boolean
        Get
            SelfPace = _SelfPace
        End Get
        Set(ByVal value As Boolean)
            _SelfPace = value
        End Set
    End Property

    Private _CheckInStrQuestionId As String 'เก็บค่า QuestionId ที่ต่อ String กันเพื่อเอามาเช็คก่อนเข้า Store Procedure
    Public Property CheckInStrquestionId As String
        Get
            CheckInStrquestionId = _CheckInStrQuestionId
        End Get
        Set(ByVal value As String)
            _CheckInStrQuestionId = value
        End Set
    End Property



End Class
