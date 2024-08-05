Public Class SelectQuestionsByUser
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()

    Protected TempTestset As Testset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TempTestset = Session("TempTestset")
        InitialData()
    End Sub

    Public Sub InitialData()
        Dim content As New StringBuilder
        For Each subjectClass In TempTestset.ListSubjectClassQuestion
            Dim subjectName As String = subjectClass.SubjectId.ToSubjectShortThName
            Dim className As String = subjectClass.ClassId.ToLevelShortName

            content.Append("<div class='divSubjectClass'>")
            content.Append("<span class='spanSubjectClass'>" & subjectName & " - " & className & "</span>")



            For Each qset In subjectClass.ListQset
                Dim qsetId As String = qset.QsetId
                Dim dtQsetDetail As DataTable = GetQsetDetail(qsetId)
                If dtQsetDetail.Rows.Count > 0 Then
                    content.Append("<div class='divQset' >")
                    content.Append("<span>" & dtQsetDetail.Rows(0)("QCategory_Name").ToString() & " - " & dtQsetDetail.Rows(0)("Qset_Name").ToString() & "</span>")

                    Dim contentQuestions As String = GetContentQuestions(subjectClass, qset)
                    content.Append(contentQuestions)

                    content.Append("</div>")
                End If
            Next

            content.Append("</div>")
        Next

        divContent.InnerHtml = content.ToString()
    End Sub

    Private Function GetQsetDetail(qsetId As String) As DataTable
        Dim sql As String = "Select g.GroupSubject_ShortName, l.Level_ShortName, qc.QCategory_Name, qs.QSet_Name FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
        sql &= " INNER JOIN tblBook b On qc.Book_Id = b.BookGroup_Id INNER JOIN tblGroupSubject g On b.GroupSubject_Id = g.GroupSubject_Id INNER JOIN tblLevel l On b.Level_Id = l.Level_Id "
        sql &= " WHERE qs.QSet_Id = '" & qsetId & "';"
            Return db.getdata(sql)
    End Function

    Private Function GetContentQuestions(subjectClass As TestsetSubjectClassQuestion, tsqs As TestSetQuestionset) As String
        Dim content As String = ""
        Dim dtQuestions As DataTable = GetQuestionsInQset(tsqs.QsetId)
        If dtQuestions.Rows.Count > 0 Then
            Dim qsetType As Integer = dtQuestions.Rows(0)("QSet_Type")
            If qsetType = EnumQsetType.Choice Then
                Return GetContentQsetType1(dtQuestions, subjectClass, tsqs)
            ElseIf qsetType = EnumQsetType.TrueFalse Then
                Return GetContentQsetType2(dtQuestions, subjectClass, tsqs)
            ElseIf qsetType = EnumQsetType.Pair Then
                Return GetContentQsetType3(dtQuestions, subjectClass, tsqs)
            ElseIf qsetType = EnumQsetType.Sort Then
                Return GetContentQsetType6(dtQuestions, subjectClass, tsqs)
            ElseIf qsetType = EnumQsetType.Subjective Then
                Return "In Progress"
            End If
        End If
        Return ""
    End Function

    Private Function GetQuestionsInQset(qsetId As String) As DataTable
        'Dim sql As String = "Select qs.Qset_Id,qs.QSet_Type,q.Question_Id,q.Question_Name from tblTestSetQuestionSet tsqs inner join tblTestSetQuestionDetail tsqd On tsqs.TSQS_Id = tsqd.TSQS_Id "
        'sql &= " inner join tblQuestionset qs On qs.QSet_Id = tsqs.QSet_Id inner join tblQuestion q On q.Question_Id = tsqd.Question_Id "
        'sql &= " where tsqs.TestSet_Id = '" & TestsetId & "' and tsqs.QSet_Id = '" & QsetId & "' "
        'sql &= " order by tsqd.TSQD_No;"

        Dim sql As String = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id  WHERE q.IsActive = 1 And qs.QSet_Id = '" & qsetId & "' ORDER BY q.Question_No;"

        Return db.getdata(sql)
    End Function

    Private Function GetAnswersInQuestion(questionId As String) As DataTable
        Dim sql As String = "select * from tblAnswer where Question_Id = '" & questionId & "' and IsActive = 1 order by Answer_No;"
        Return db.getdata(sql)
    End Function

    Private Function GetContentQsetType1(dtQuestions As DataTable, subjectClass As TestsetSubjectClassQuestion, tsqs As TestSetQuestionset) As String

        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim checkbokName As String = "q_" & questionId

            content.Append("<li>")

            Dim q As TestsetQuestion = tsqs.GetQuestionById(questionId)
            Dim isChecked As String = If((q.IsActive), "checked='checked'", "")

            content.Append("<input class='chkSelected' type='checkbox' " & isChecked & " id='" & checkbokName & "' onclick=""setQuestionIsActive(this,'" & subjectClass.SubjectId & "','" & subjectClass.ClassId & "','" & tsqs.QsetId & "','" & questionId & "');"" /><label for='" & checkbokName & "'></label>")

            Dim isSelected As String = If((q.IsActive), "selected", "notselect")

            content.Append("<div class='divQuestion " & isSelected & "' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")
            content.Append("<span>" & questionNo & ". " & r("Question_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath()) & "</span><br>")

            Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)
            Dim answerNo As Integer = 0
            For Each answer In dtAnswers.Rows
                Dim correctAnswer As String = If((answer("Answer_Score") = 1), "class='correct'", "")
                content.Append("<span " & correctAnswer & " >" & PrefixAnswer.Thai(answerNo) & ". " & answer("Answer_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath()) & "</span><br>")
                answerNo += 1
            Next
            content.Append("</div></li>")
            questionNo += 1
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function

    Private Sub ClearQuestionSetNotSelected()
        For Each sc In TempTestset.ListSubjectClassQuestion
            sc.ClearQuestionSetNotSelected()
        Next

        Session("TempTestset") = TempTestset
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        ' remove qset ที่ไม่ได้เลือกข้อสอบทิ้ง ให้ไปเลือกใหม่ที่หน้า step 3
        ClearQuestionSetNotSelected()
        Response.Redirect("~/testset/NewStep3.aspx", False)
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Response.Redirect("~/testset/TestsetSummaryPage.aspx", False)
    End Sub

    Private Function GetContentQsetType2(dtQuestions As DataTable, subjectClass As TestsetSubjectClassQuestion, tsqs As TestSetQuestionset) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul>")
        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()
            Dim checkbokName As String = "q_" & questionId

            content.Append("<li>")

            Dim q As TestsetQuestion = tsqs.GetQuestionById(questionId)
            Dim isChecked As String = If((q.IsActive), "checked='checked'", "")

            content.Append("<input class='chkSelected' type='checkbox' " & isChecked & " id='" & checkbokName & "' onclick=""setQuestionIsActive(this,'" & subjectClass.SubjectId & "','" & subjectClass.ClassId & "','" & tsqs.QsetId & "','" & questionId & "');"" /><label for='" & checkbokName & "'></label>")

            Dim isSelected As String = If((q.IsActive), "selected", "notselect")
            content.Append("<div class='divQuestion " & isSelected & "' qsetid='" & r("Qset_Id").ToString() & "' id='" & questionId & "'>")

            Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)
            Dim answerCorrect As String = (From a In dtAnswers.AsEnumerable() Where a.Field(Of Double)("Answer_Score") = 1 Select a.Field(Of String)("Answer_Name")).SingleOrDefault()
            Dim answerImage As String = If((answerCorrect = "ถูก"), "<img src='../images/right.png' alt='' />", "<img src='../images/wrong.png' alt='' />")

            content.Append("<div>" & questionNo & ". " & answerImage & " " & r("Question_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath()) & "</div>")
            content.Append("</div></li>")
            questionNo += 1
        Next
        content.Append("</ul>")
        Return content.ToString()
    End Function

    Private Function GetContentQsetType3(dtQuestions As DataTable, subjectClass As TestsetSubjectClassQuestion, tsqs As TestSetQuestionset) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1

        content.Append("<ul><li>")

        Dim checkbokName As String = "q_" & tsqs.QsetId

        Dim isChecked As String = If((tsqs.IsSelectedAll), "checked='checked'", "")
        content.Append("<input class='chkSelected' type='checkbox' " & isChecked & " id='" & checkbokName & "' onclick=""setQuestionsIsActive(this,'" & subjectClass.SubjectId & "','" & subjectClass.ClassId & "','" & tsqs.QsetId & "');"" /><label for='" & checkbokName & "'></label>")

        Dim isSelected As String = If((tsqs.IsSelectedAll), "selected", "notselect")
        content.Append("<div class='divQuestion " & isSelected & "' qsetid='" & tsqs.QsetId & "'>")

        For Each r In dtQuestions.Rows
            Dim questionId As String = r("Question_Id").ToString()

            Dim dtAnswers As DataTable = GetAnswersInQuestion(questionId)
            content.Append("<div><table style='width:100%;'><tr><td style='width:45%;'>" & r("Question_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath()) & "</td><td>คู่กับ</td><td style='width:45%;'>" & dtAnswers.Rows(0)("Answer_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath()) & "</td></tr></table></div>")

            questionNo += 1
        Next
        content.Append("</div></li></ul>")

        Return content.ToString()
    End Function

    Private Function GetContentQsetType6(dtQuestions As DataTable, subjectClass As TestsetSubjectClassQuestion, tsqs As TestSetQuestionset) As String
        Dim content As New StringBuilder
        Dim questionNo As Integer = 1
        content.Append("<ul><li>")

        Dim checkbokName As String = "q_" & tsqs.QsetId
        Dim isChecked As String = If((tsqs.IsSelectedAll), "checked='checked'", "")
        content.Append("<input class='chkSelected' type='checkbox' " & isChecked & " id='" & checkbokName & "' onclick=""setQuestionsIsActive(this,'" & subjectClass.SubjectId & "','" & subjectClass.ClassId & "','" & tsqs.QsetId & "');"" /><label for='" & checkbokName & "'></label>")

        Dim isSelected As String = If((tsqs.IsSelectedAll), "selected", "notselect")
        content.Append("<div class='divQuestion " & isSelected & "' qsetid='" & tsqs.QsetId & "' >")

        For Each r In dtQuestions.Rows
            Dim questionName As String = r("Question_Name").ToString().Replace("___MODULE_URL___", tsqs.QsetId.ToFolderFilePath())
            content.Append("<div><table style='width:100%;'><tr><td> ลำดับที่ " & questionNo & " " & questionName & "</td></tr></table></div>")
            questionNo += 1
        Next
        content.Append("</div></li></ul>")

        Return content.ToString()
    End Function

End Class