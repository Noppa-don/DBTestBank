Public Class Testset
    Property Id As String
    Property Name As String
    Property Time As String
    Property FontSize As String
    Property IsPracticeMode As Boolean
    Property IsQuizMode As Boolean
    Property IsReportMode As Boolean
    Property IsHomeworkMode As Boolean
    Property SubjectClassSelected As New Dictionary(Of String, List(Of String))
    Property ListSubjectClassQuestion As New List(Of TestsetSubjectClassQuestion)
    Property IsEdit As Boolean

    Public Sub New()
        Me.Id = Guid.NewGuid.ToString()
        Me.Name = ""
        Me.Time = 60
        Me.FontSize = ""
        Me.IsPracticeMode = True
        Me.IsHomeworkMode = False
        Me.IsQuizMode = False
        Me.IsReportMode = False
        Me.IsEdit = False
    End Sub

    Public Sub New(dtTestset As DataTable)
        Me.Id = dtTestset.Rows(0)("testset_Id").ToString()
        Me.Name = dtTestset.Rows(0)("testset_name").ToString()
        Me.Time = dtTestset.Rows(0)("testset_time").ToString()
        Me.FontSize = dtTestset.Rows(0)("TestSet_FontSize").ToString()
        Me.IsPracticeMode = dtTestset.Rows(0)("IsPracticeMode")
        Me.IsHomeworkMode = dtTestset.Rows(0)("IsHomeworkMode")
        Me.IsQuizMode = dtTestset.Rows(0)("IsQuizMode")
        Me.IsReportMode = dtTestset.Rows(0)("IsReportMode")
        Me.IsEdit = True
    End Sub

    Public ReadOnly Property QuestionAmount As Integer
        Get
            Dim amount As Integer = 0
            For Each r In ListSubjectClassQuestion
                amount += r.Amount
            Next
            Return amount
        End Get
    End Property


    Public Function IsSubjectClassSelected(subjectId As String, classId As String) As Boolean
        If SubjectClassSelected.ContainsKey(subjectId) Then
            Dim listClassId As List(Of String) = SubjectClassSelected(subjectId)
            If listClassId.Contains(classId) Then
                Return True
            End If
            Return False
        End If
        Return False
    End Function

    Public Function GetSubjectClassQuestion(classId As String, subjectId As String) As TestsetSubjectClassQuestion
        'If ListSubjectClassQuestion.Count = 0 Then
        '    Return Nothing
        'End If
        Return ListSubjectClassQuestion.Where(Function(t) t.ClassId = classId And t.SubjectId = subjectId).SingleOrDefault()
    End Function

    ''' <summary>
    ''' function ในการหาจำนวนข้อสอบที่เลือกมา ในวิชาและชั้นนั้นๆ
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <param name="isWpp"></param>
    ''' <returns></returns>
    Public Function GetSubjectClassQuestionSelectedAmount(subjectId As String, classId As String, isWpp As Boolean) As Integer
        Dim questionSelectedAmount As Integer = 0
        Dim subjectClassQuestion As TestsetSubjectClassQuestion = GetSubjectClassQuestion(classId, subjectId)
        If subjectClassQuestion IsNot Nothing Then
            For Each qset In subjectClassQuestion.ListQset.Where(Function(f) f.IsWPP = isWpp)
                questionSelectedAmount += qset.QuestionSelectedAmount '.ListQuestion.Count
            Next
        End If
        Return questionSelectedAmount
    End Function

    Public ReadOnly Property GetTestsetSubjectName As String
        Get
            Dim listSubjects = (From s In ListSubjectClassQuestion Select s.SubjectId).Distinct()
            If listSubjects.Count = 1 Then
                Return listSubjects(0).ToGroupSubjectThName
            End If
            Return "รวมวิชา"
        End Get
    End Property

    Public ReadOnly Property GetTestsetSubjectShortName As String
        Get
            Dim listSubjects = (From s In ListSubjectClassQuestion Select s.SubjectId).Distinct()
            If listSubjects.Count = 1 Then
                Return listSubjects(0).ToSubjectShortThName
            End If
            Return "รวมวิชา"
        End Get
    End Property

    Public Sub ClearSubjectClassSelected()
        Me.SubjectClassSelected.Clear()
    End Sub

    Public Sub ClearQuestionsNotSelected()
        Dim tempListSubjectClassQuestion As New List(Of TestsetSubjectClassQuestion)
        For Each tmp In Me.ListSubjectClassQuestion
            If Not Me.SubjectClassSelected.ContainsKey(tmp.SubjectId) Then
                tempListSubjectClassQuestion.Add(tmp)
            Else
                Dim listClass As List(Of String) = Me.SubjectClassSelected(tmp.SubjectId)
                Dim c = listClass.Where(Function(t) t = tmp.ClassId)
                If c.Count = 0 Then
                    tempListSubjectClassQuestion.Add(tmp)
                End If
            End If
        Next

        ' ลบวิชา ที่ไม่ได้เลือกแล้วออกจาก list
        For Each tmp In tempListSubjectClassQuestion
            Me.ListSubjectClassQuestion.Remove(tmp)
        Next
    End Sub


    Public ReadOnly Property SeletedQsetAmount() As Integer
        Get
            Dim m As Integer = 0
            For Each r In ListSubjectClassQuestion
                m += r.QsetAmount
            Next
            Return m
        End Get
    End Property
End Class

Public Class TestsetSubjectClassQuestion
    Property SubjectId As String
    Property ClassId As String
    'Property ListQsetId As New Dictionary(Of String, List(Of TestsetQuestion))
    Property ListQset As New List(Of TestSetQuestionset)

    Public ReadOnly Property QsetAmount() As Integer
        Get
            Return Me.ListQset.Count
        End Get
    End Property

    Public ReadOnly Property Amount As Integer
        Get
            'Dim q = From r In listTestsetQuestions Select New With {Key r.QuestionId} Distinct.ToList
            'Return q.Count
            Return 0
        End Get
    End Property


    'Public Sub AddQsetAndQuestion(qsetId As String, dtQuestions As DataTable, AddById As String)
    '    Dim questions As New List(Of TestsetQuestion)
    '    For Each r In dtQuestions.Rows
    '        Dim testsetQuestion As New TestsetQuestion With {.QuestionId = r("Question_Id").ToString(), .AddById = AddById, .AddBy = EnumAddBy.Qset}
    '        questions.Add(testsetQuestion)
    '    Next
    '    ListQsetId.Add(qsetId, questions)
    'End Sub

    ''' <summary>
    ''' function ในการเช็คว่า ได้ add qset นี้เข้าใน temp แล้วหรือยัง
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <returns></returns>
    Public Function IsTestsetQuestionsetExist(qsetId As String) As Boolean
        Dim questionSet As TestSetQuestionset = ListQset.Where(Function(f) f.QsetId = qsetId).SingleOrDefault()
        If questionSet Is Nothing Then
            Return False
        End If
        Return True
    End Function

    Public Sub AddTestsetQuestionsetWithQuestion(qsetId As String, qsetType As EnumQsetType, dtQuestions As DataTable, AddById As String, isWpp As Boolean)
        'Dim questionSet As TestSetQuestionset = ListQset.Where(Function(f) f.QsetId = qsetId).Single()
        'If questionSet Is Nothing Then
        Dim questions As New List(Of TestsetQuestion)
        For Each r In dtQuestions.Rows
            Dim testsetQuestion As New TestsetQuestion With {.QuestionId = r("Question_Id").ToString(), .AddById = AddById, .AddBy = EnumAddBy.Qset, .IsActive = r("IsActive")}
            questions.Add(testsetQuestion)
        Next
        Dim questionSet As New TestSetQuestionset With {.QsetId = qsetId, .QsetType = qsetType, .ListQuestion = questions, .IsWPP = isWpp}
        ListQset.Add(questionSet)
        'End If
    End Sub

    Public Sub RemoveTestsetQuestionset(qsetId As String)
        Dim questionSet As TestSetQuestionset = GetQuestionsetById(qsetId) 'ListQset.Where(Function(f) f.QsetId = qsetId).SingleOrDefault()
        Me.ListQset.Remove(questionSet)
    End Sub

    ''' <summary>
    ''' function ในการ get qset ออกมาใช้งาน
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <returns>TestSetQuestionset</returns>
    Public Function GetQuestionsetById(qsetId As String) As TestSetQuestionset
        Return ListQset.Where(Function(f) f.QsetId = qsetId).SingleOrDefault()
    End Function

    Public Sub ClearQuestionSetNotSelected()
        Dim temp As New List(Of TestSetQuestionset)
        For Each qset In ListQset
            If qset.QuestionSelectedAmount = 0 Then
                temp.Add(qset)
            End If
        Next
        ' remove qset ออกจากของจริง อ้างอิงจาก temp
        For Each qset In temp
            ListQset.Remove(qset)
        Next
    End Sub
End Class

Public Class TestSetQuestionset
    Property QsetId As String
    Property QsetType As EnumQsetType
    Property IsWPP As Boolean
    Property ListQuestion As List(Of TestsetQuestion)

    Public ReadOnly Property QuestionAmount() As Integer
        Get
            If QsetType = EnumQsetType.Pair Or QsetType = EnumQsetType.Sort Then
                Return 1
            End If
            Return ListQuestion.Count
        End Get
    End Property

    Public ReadOnly Property QuestionSelectedAmount() As Integer
        Get
            Dim selectedAmount As Integer = ListQuestion.Where(Function(f) f.IsActive = True).Count
            If QsetType = EnumQsetType.Pair Or QsetType = EnumQsetType.Sort Then
                Return If((selectedAmount = 0), selectedAmount, 1)
            End If
            Return selectedAmount
        End Get
    End Property

    ''' <summary>
    ''' function ในการ get question ออกมาจาก listquestion ใน qset
    ''' </summary>
    ''' <param name="questionId"></param>
    ''' <returns>TestsetQuestion</returns>
    Public Function GetQuestionById(questionId As String) As TestsetQuestion
        Return ListQuestion.Where(Function(f) f.QuestionId = questionId).SingleOrDefault()
    End Function

    Public ReadOnly Property IsSelectedAll() As Boolean
        Get
            Dim selectedAmount As Integer = ListQuestion.Where(Function(f) f.IsActive = True).Count
            ''
            If QsetType = EnumQsetType.Pair Or QsetType = EnumQsetType.Sort Then
                Return If((selectedAmount = 0), False, True)
            End If
            ''
            If QuestionAmount = selectedAmount Then
                Return True
            End If
            Return False
        End Get
    End Property


End Class

Public Class TestsetQuestion
    Property QuestionId As String
    'Property QsetId As String
    Property AddById As String
    Property AddBy As EnumAddBy
    Property IsActive As Boolean
End Class

Public Enum EnumAddBy
    Qset
    KPA
    Evalution
End Enum

