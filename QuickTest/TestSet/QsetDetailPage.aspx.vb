Public Class QsetDetailPage
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Private TestsetId As String '= "9DB8CAA0-6B45-4360-BC53-8CC6B377A850"
    Private QsetId As String '= "CAB79557-CE9D-4699-BE36-5B704F930234"
    Private QuestionAmount As Integer

    Private TempTestset As Testset

    Private Tsqs As TestSetQuestionset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TestsetId = If((Session("EditID").ToString = ""), Session("newTestSetId"), Session("EditID"))
        QsetId = Request.QueryString("qsetId").ToString()
        TempTestset = Session("TempTestset")

        InitialData()
    End Sub

    Public Sub InitialData()
        Dim dtQsetDetail As DataTable = GetQsetDetail()



        If dtQsetDetail.Rows.Count > 0 Then

            Dim subjectId As String = dtQsetDetail.Rows(0)("GroupSubject_Id").ToString()
            Dim classId As String = dtQsetDetail.Rows(0)("Level_Id").ToString()

            Dim sc As TestsetSubjectClassQuestion = TempTestset.GetSubjectClassQuestion(classId, subjectId)
            Me.Tsqs = sc.GetQuestionsetById(QsetId)

            lblSubjectName.Text = dtQsetDetail.Rows(0)("GroupSubject_ShortName") & " " & dtQsetDetail.Rows(0)("Level_ShortName")
            lblQsetName.Text = "<b>" & dtQsetDetail.Rows(0)("QCategory_Name") & "</b> - " & dtQsetDetail.Rows(0)("QSet_Name")

            divQuestions.InnerHtml = GetContentQuestions()
            lblQuestionAmount.Text = Tsqs.QuestionSelectedAmount & " ข้อ"
        End If
    End Sub

    Private Function GetQsetDetail() As DataTable
        Dim sql As String = "SELECT  g.GroupSubject_Id,l.Level_Id, g.GroupSubject_ShortName,l.Level_ShortName,qc.QCategory_Name,qs.QSet_Name FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
        sql &= " INNER JOIN tblBook b On qc.Book_Id = b.BookGroup_Id INNER JOIN tblGroupSubject g On b.GroupSubject_Id = g.GroupSubject_Id INNER JOIN tblLevel l On b.Level_Id = l.Level_Id "
        sql &= " WHERE qs.QSet_Id = '" & QsetId & "';"
        Return db.getdata(sql)
    End Function

    Private Function GetContentQuestions() As String
        Dim content As String = ""
        Dim dtQuestions As DataTable = GetQuestionsInQset()

        If dtQuestions.Rows.Count > 0 Then
            Dim qsetType As Integer = dtQuestions.Rows(0)("QSet_Type")
            If qsetType = EnumQsetType.Choice Then
                'QuestionAmount = dtQuestions.Rows.Count
                Return GetContentQsetType1(dtQuestions)
            ElseIf qsetType = EnumQsetType.TrueFalse Then
                ' QuestionAmount = dtQuestions.Rows.Count
                Return GetContentQsetType2(dtQuestions)
            ElseIf qsetType = EnumQsetType.Pair Then
                ' QuestionAmount = 1
                Return GetContentQsetType3(dtQuestions)
            ElseIf qsetType = EnumQsetType.Sort Then
                ' QuestionAmount = 1
                Return GetContentQsetType6(dtQuestions)
            ElseIf qsetType = EnumQsetType.Subjective Then
                Return "In Progress"
            End If
        End If
        Return ""
    End Function



    Private Function GetQuestionsInQset() As DataTable
        'Dim sql As String = "Select qs.Qset_Id,qs.QSet_Type,q.Question_Id,q.Question_Name from tblTestSetQuestionSet tsqs inner join tblTestSetQuestionDetail tsqd On tsqs.TSQS_Id = tsqd.TSQS_Id "
        'sql &= " inner join tblQuestionset qs On qs.QSet_Id = tsqs.QSet_Id inner join tblQuestion q On q.Question_Id = tsqd.Question_Id "
        'sql &= " where tsqs.TestSet_Id = '" & TestsetId & "' and tsqs.QSet_Id = '" & QsetId & "' "
        'sql &= " order by tsqd.TSQD_No;"

        Dim sql As String = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id  WHERE q.IsActive = 1 And qs.QSet_Id = '" & QsetId & "' ORDER BY q.Question_No;"

        'Dim sql As New StringBuilder
        'sql.Append("SELECT q.*,qs.* FROM tblQuestion q INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        'sql.Append(" INNER JOIN tblTestSetQuestionDetail tsqd ON tsqd.Question_Id = q.Question_Id ")
        'sql.Append(" INNER JOIN tblTestSetQuestionSet tsqs ON tsqd.TSQS_Id = tsqs.TSQS_Id ")
        'sql.Append(" WHERE q.IsActive = 1 And qs.QSet_Id = '" & QsetId & "'  ")
        'sql.Append(" AND tsqs.TestSet_Id = '" & TempTestset.Id & "' ")
        'sql.Append(" And tsqd.IsActive = 1 ")
        'sql.Append(" ORDER BY q.Question_No;")

        Return db.getdata(sql.ToString())
    End Function

    Private Function GetAnswersInQuestion(questionId As String) As DataTable
        Dim sql As String = "select * from tblAnswer where Question_Id = '" & questionId & "' and IsActive = 1 order by Answer_No;"
        Return db.getdata(sql)
    End Function

    Private Function GetContentQsetType1(dtQuestions As DataTable) As String

        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim question As TestsetQuestion = Tsqs.GetQuestionById(questionId)
            If question.IsActive Then
                content.Append("<li><div class='divQuestion' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")
                content.Append("<div class='divQuestionName' ><b>ข้อที่ " & questionNo & "</b> " & r("Question_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath()) & "</div>")
                content.Append("<div class='divAnswer'>")
                Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)
                Dim answerNo As Integer = 0
                For Each answer In dtAnswers.Rows
                    Dim correctAnswer As String = If((answer("Answer_Score") = 1), "class='correct'", "")
                    content.Append("<div " & correctAnswer & " >" & PrefixAnswer.Thai(answerNo) & ". " & answer("Answer_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath()) & "</div><br>")
                    answerNo += 1
                Next
                content.Append("</div></div></li>")
                questionNo += 1
            End If
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function

    Private Function GetContentQsetType2(dtQuestions As DataTable) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim question As TestsetQuestion = Tsqs.GetQuestionById(questionId)
            If question.IsActive Then
                content.Append("<li><div class='divQuestion' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")
                Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)
                Dim answerCorrect As String = (From a In dtAnswers.AsEnumerable() Where a.Field(Of Double)("Answer_Score") = 1 Select a.Field(Of String)("Answer_Name")).SingleOrDefault()
                Dim answer As String = If((answerCorrect = "ถูก"), "<img src='../images/right.png' alt='' />", "<img src='../images/wrong.png' alt='' />")

                content.Append("<div class='divQuestionName' ><b>ข้อที่ " & questionNo & "</b> " & answer & " " & r("Question_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath()) & "</div>")
                content.Append("</div></li>")
                questionNo += 1
            End If
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function

    Private Function GetContentQsetType3(dtQuestions As DataTable) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim question As TestsetQuestion = Tsqs.GetQuestionById(questionId)
            If question.IsActive Then
                content.Append("<li><div class='divQuestion' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")
                Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)

                content.Append("<div class='divQuestionName' ><table style='width:100%;'><tr><td style='width:45%;'>" & r("Question_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath()) & "</td><td>คู่กับ</td><td style='width:45%;'>" & dtAnswers.Rows(0)("Answer_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath()) & "</td></tr></table></div>")
                content.Append("</div></li>")
                questionNo += 1
            End If
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function

    Private Function GetContentQsetType6(dtQuestions As DataTable) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim question As TestsetQuestion = Tsqs.GetQuestionById(questionId)
            If question.IsActive Then
                content.Append("<li><div class='divQuestion' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")
                'Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)

                Dim questionName As String = r("Question_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath())
                content.Append("<div class='divQuestionName' ><table style='width:100%;'><tr><td> ลำดับที่ " & questionNo & " " & questionName & "</td></tr></table></div>")
                content.Append("</div></li>")
                questionNo += 1
            End If
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function
End Class