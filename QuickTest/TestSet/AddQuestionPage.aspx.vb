Imports System.Web.Script.Serialization

Public Class AddQuestionPage
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()

    Protected Book As New QuestionService.Book
    Protected QuestionSet As New QuestionService.QuestionSet

    Protected QsetId As String
    'Protected QuestionSetJSONStr As String
    'Protected QuestionsJSONStr As String

    'Private jsonSerialiser = New JavaScriptSerializer()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("EditID") = ""
        'Session("newTestSetId") = "17CFBCF9-156B-4005-A35E-09D859FA1991"
        QsetId = Request.QueryString("qsetId").ToString()

        HttpContext.Current.Session("QsetImagePath") = QsetId

        InitialData()

        'QuestionsJSONStr = jsonSerialiser.Serialize(QuestionSet.Questions)
        'QuestionSetJSONStr = jsonSerialiser.Serialize(QuestionSet)
    End Sub

    Private Sub InitialData()
        Try
            Dim userId As String = HttpContext.Current.Session("UserId").ToString() ' "EE859090-0B59-4DED-9BCF-797324F87E08"
            Dim sql As String = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id WHERE qs.QSet_Id = '" & QsetId & "' AND qc.Parent_Id = '" & userId & "' AND qs.IsWpp = 0;"
            db.OpenWithTransection()
            Dim dt As DataTable = db.getdataWithTransaction(sql)
            If dt.Rows.Count > 0 Then
                Book.Id = dt.Rows(0)("BookGroup_Id").ToString().ToLower()
                Book.Name = dt.Rows(0)("Book_Name").ToString()
                Book.LevelId = dt.Rows(0)("Level_Id").ToString().ToLower()
                Book.GroupSubjectId = dt.Rows(0)("GroupSubject_Id").ToString().ToLower()

                Dim questionCategory As New List(Of QuestionService.QuestionCategory)
                questionCategory.Add(New QuestionService.QuestionCategory With {.Id = dt.Rows(0)("QCategory_Id").ToString(), .Name = dt.Rows(0)("QCategory_Name").ToString(), .BookId = Book.Id})
                Book.QuestionCategories = questionCategory

                QuestionSet.Id = dt.Rows(0)("QSet_Id").ToString().ToLower()
                QuestionSet.Name = ChangePathImg(dt.Rows(0)("QSet_Name").ToString(), dt.Rows(0)("QSet_Id").ToString())
                QuestionSet.Type = dt.Rows(0)("QSet_Type").ToString()
                QuestionSet.TypeName = GetQsetTypeName(QuestionSet.Type)

                'QuestionSet.Questions = GetQuestions()
            End If
            db.CommitTransection()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Sub

    'Private Function GetQuestions() As List(Of QuestionService.Question)
    '    Try
    '        Dim questionManagement As New QuestionManagement(QsetId, QuestionSet.Type)
    '        Dim dt As DataTable = questionManagement.GetQuestionsInQset()
    '        Dim questions As New List(Of QuestionService.Question)
    '        If dt.Rows.Count = 0 Then
    '            Dim question As QuestionService.Question = questionManagement.NewQuestion()
    '            questions.Add(question)
    '        Else
    '            For Each r In dt.Rows
    '                Dim qsetId As String = r("Qset_Id").ToString()
    '                Dim question As New QuestionService.Question With {
    '                    .Id = r("Question_Id").ToString(),
    '                    .Name = ChangePathImg(r("Question_Name_Quiz"), qsetId), 'Web.HttpUtility.UrlEncode(r("Question_Name_Quiz").ToString().Replace("___MODULE_URL___", r("Qset_Id").ToString().ToFolderFilePath())),
    '                .ExplainName = ChangePathImg(r("Question_Expain_Quiz"), qsetId),'Web.HttpUtility.UrlEncode(r("Question_Expain_Quiz").ToString().Replace("___MODULE_URL___", r("Qset_Id").ToString().ToFolderFilePath())),
    '                    .No = r("Question_No")}

    '                Dim dtAnswers As DataTable = questionManagement.GetAnswersInQuestion(question.Id)
    '                Dim answers As New List(Of QuestionService.Answer)
    '                For Each ans In dtAnswers.Rows
    '                    Dim answer As New QuestionService.Answer With {
    '                        .Id = ans("Answer_Id").ToString(),
    '                        .Name = ChangePathImg(ans("Answer_Name_Quiz"), qsetId),
    '                        .ExplainName = ChangePathImg(ans("Answer_Expain_Quiz"), qsetId),
    '                        .No = ans("Answer_No"),
    '                        .Score = ans("Answer_Score"),
    '                        .QsetId = qsetId,
    '                        .QuestionId = question.Id}
    '                    answers.Add(answer)
    '                Next
    '                question.Answers = answers
    '                questions.Add(question)
    '            Next
    '        End If
    '        Return questions
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' function ในการแปลง pathimg จาก ___MODULE_URL___ เป็นตาม format ของ qset ต้อง encode ด้วย เพื่อให้ฝั่ง javascript ทำงานได้
    ''' </summary>
    ''' <param name="txtName"></param>
    ''' <param name="qsetId"></param>
    ''' <returns></returns>
    Private Function ChangePathImg(txtName As String, qsetId As String) As String
        ' Return Web.HttpUtility.UrlEncode(txtName.Replace("___MODULE_URL___", qsetId.ToFolderFilePath()))
        Return txtName.Replace("___MODULE_URL___", qsetId.ToFolderFilePath())
    End Function

    Private Function GetQsetTypeName(qsetType As Integer)
        Select Case qsetType
            Case 1
                Return "ปรนัย"
            Case 2
                Return "ถูกผิด"
            Case 3
                Return "จับคู่"
            Case 6
                Return "เรียงลำดับ"
            Case Else
                Return "อัตนัย"
        End Select
    End Function

End Class