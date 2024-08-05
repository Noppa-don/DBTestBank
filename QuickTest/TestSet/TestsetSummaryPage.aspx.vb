Public Class TestsetSummaryPage
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Private TestsetId As String
    Private TempTestset As Testset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Log.Record(Log.LogType.PageLoad, "pageload TestsetSummaryPage", False)
        'If (Session("UserId") Is Nothing) Then
        '    Log.Record(Log.LogType.PageLoad, "step3 session หลุด", False)
        '    Response.Redirect("~/LoginPage.aspx")
        'End If

        If Not IsPostBack Then
            InitialData()
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/testset/SelectQuestionsByUser.aspx", False)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'If rdUseAll.Checked Then
        Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนบันทึกชุด", True)
        Response.Redirect("~/testset/step4.aspx", False)
        'Else
        '    Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนเลือกข้อสอบด้วยตัวเอง", True)
        '    Response.Redirect("~/testset/SelectQuestionsByUser.aspx", False)
        'End If
    End Sub

    Private Sub InitialData()
        'TestsetId = If((Session("EditID") Is Nothing OrElse Session("EditID").ToString = ""), Session("newTestSetId"), Session("EditID")) '"BDEC4EF9-0F27-4780-8ADF-C493E3E5260C" 
        TempTestset = Session("TempTestset")
        divShowQset.InnerHtml = GetContentQsetsInTestset()
    End Sub

    Private Function GetContentQsetsInTestset() As String
        'Dim dtGroupSubject As DataTable = GetGroupSubjectInTestset()
        'Dim dtQset As DataTable = GetQsetsInTestset()

        'Dim content As String = "<ul>"
        'For Each subject In dtGroupSubject.Rows
        '    content &= "<li><div class='divClassName'><span>" & subject("GroupSubject_ShortName") & " " & subject("Level_ShortName") & "</span><div class='divAllQset' ><ul>"
        '    Dim qsets = From q In dtQset Where q.Field(Of Guid)("Level_Id") = subject("Level_Id") And q.Field(Of Guid)("GroupSubject_Id") = subject("GroupSubject_Id")
        '    For Each r In qsets
        '        content &= "<li><div class='divQset' id='" & r("QSet_Id").ToString() & "'><img src='../Images/WaterMark/WatermarkWPP.jpg' alt='' />"
        '        content &= "<b>" & r("QCategory_Name") & "</b> - " & r("QSet_Name")
        '        content &= "<span class='qsetQuestionAmount'>" & r("QuestionAmount") & " ข้อ</span></div></li>"
        '    Next
        '    content &= "</ul></div></div></li>"
        'Next
        'content &= "</ul>"
        Dim content As New StringBuilder
        content.Append("<ul>")

        For Each sc In TempTestset.ListSubjectClassQuestion
            content.Append("<li><div class='divClassName'><span>" & sc.SubjectId.ToSubjectShortThName & " " & sc.ClassId.ToLevelShortName & "</span><div class='divAllQset' ><ul>")
            For Each q In sc.ListQset
                If q.QuestionSelectedAmount > 0 Then ' ต้องเป็น qset ที่เลือกมา 1 ข้อขึ้นไปเท่านั้น ที่ทำแบบนี้ เพราะว่า เราสามารถย้อนกลับไปยังหน้าเลือกข้อสอบทีละข้อ เพื่อเอาข้อสอบกลับมาอีกครั้ง แต่เมื่อ save ข้อสอบชุดนี้ จะไปถูกบันทึกลง db
                    Dim dt As DataTable = GetQuestionsInQset(q.QsetId)
                    content.Append("<li><div class='divQset' id='" & q.QsetId & "'>")
                    content.Append(If((q.IsWPP), "<img src='../Images/WaterMark/WatermarkWPP.jpg' alt='' />", "<img src='../Images/upgradeClass/schoolbus.png' alt='' />")) 'ภาพโรงเรียน
                    content.Append("<b>" & dt.Rows(0)("QCategory_Name").ToString() & "</b> - " & dt.Rows(0)("QSet_Name").ToString())
                    content.Append(If((q.IsWPP), "", "<a style='cursor:pointer;' onclick=""editQuestion('" & q.QsetId & "');"">แก้ไขข้อสอบ</a>"))
                    content.Append("<span class='qsetQuestionAmount'>" & q.QuestionSelectedAmount & " ข้อ</span></div></li>") 'แสดงจำนวนข้อสอบที่เลือกมาเท่านั้น
                End If
            Next
            content.Append("</ul></div></div></li>")
        Next
        content.Append("</ul>")

        Return content.ToString()
    End Function


    Private Function GetQuestionsInQset(qsetId As String) As DataTable
        Dim sql As String = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
        sql &= " WHERE q.IsActive = 1 AND qs.QSet_Id = '" & qsetId & "';"
        Return db.getdata(sql)
    End Function

    Private Function GetGroupSubjectInTestset() As DataTable
        Dim sql As String = " select Distinct l.Level_Id,l.Level,l.Level_ShortName,s.GroupSubject_ShortName,s.GroupSubject_Id "
        sql &= " from tblTestSetQuestionSet ts inner join tblQuestionset qs On ts.QSet_Id = qs.QSet_Id inner join tblLevel l On ts.Level_Id = l.Level_Id  "
        sql &= " inner join tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id inner join tblBook b On qc.Book_Id = b.BookGroup_Id  "
        sql &= " inner join tblGroupSubject s On b.GroupSubject_Id = s.GroupSubject_Id inner join tblTestSetQuestionDetail tsqs  On ts.TSQS_Id = tsqs.TSQS_Id "
        sql &= " where TestSet_Id = '" & TestsetId & "' and ts.IsActive = 1  group by  l.Level_Id,l.Level,l.Level_ShortName,s.GroupSubject_ShortName,s.GroupSubject_Id ,ts.LastUpdate "
        sql &= " order by l.Level; "
        Return db.getdata(sql)
    End Function

    Private Function GetQsetsInTestset() As DataTable
        Dim sql As String = "Select ts.QSet_Id,l.Level_Id,l.Level,l.Level_ShortName,s.GroupSubject_ShortName,qc.QCategory_Name,qs.QSet_Name,s.GroupSubject_Id,Count(ts.QSet_Id) As QuestionAmount "
        sql &= " from tblTestSetQuestionSet ts inner join tblQuestionset qs On ts.QSet_Id = qs.QSet_Id inner join tblLevel l On ts.Level_Id = l.Level_Id "
        sql &= " inner join tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id inner join tblBook b On qc.Book_Id = b.BookGroup_Id "
        sql &= " inner join tblGroupSubject s On b.GroupSubject_Id = s.GroupSubject_Id inner join tblTestSetQuestionDetail tsqs  On ts.TSQS_Id = tsqs.TSQS_Id "
        sql &= " where TestSet_Id = '" & TestsetId & "' and ts.IsActive = 1  and tsqs.IsActive = 1 "
        sql &= " group by  ts.QSet_Id,l.Level_Id,l.Level,l.Level_ShortName,s.GroupSubject_ShortName,qc.QCategory_Name,qs.QSet_Name,s.GroupSubject_Id,ts.LastUpdate "
        sql &= " order by l.Level,ts.LastUpdate;"
        Return db.getdata(sql)
    End Function

End Class