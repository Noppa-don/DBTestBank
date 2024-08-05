Public Class ReporterProblemQuestion
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Private QsetId As String
    Public QuestionId As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        QsetId = Request.QueryString("qsetId")
        QuestionId = If((Request.QueryString("questionId") Is Nothing), "", Request.QueryString("questionId"))

        lblHeader.Text = If((QuestionId = ""), "แจ้งหน่วยการเรียนรู้มีปัญหา", "แจ้งข้อสอบมีปัญหา")

        Dim dt As DataTable = GetQsetName()
        If dt.Rows.Count > 0 Then
            lblSubjectName.Text = dt.Rows(0)("Book_Name").ToString()
            lblQcatName.Text = dt.Rows(0)("Qcategory_Name").ToString()
            lblQsetName.Text = dt.Rows(0)("Qset_Name").ToString()
        End If

        If QuestionId <> "" Then
            dt = GetQuestion()
            lblQuestionName.Text = dt.Rows(0)("Question_Name").ToString().Replace("___MODULE_URL___", QsetId.ToFolderFilePath())
        End If

    End Sub

    Private Function GetQsetName() As DataTable
        Dim sql As String = "SELECT b.Book_Name, qc.QCategory_Name, qs.QSet_Name FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc "
        sql &= " ON qs.QCategory_Id = qc.QCategory_Id INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id "
        sql &= " WHERE QSet_Id = '" & QsetId & "';"
        Return db.getdata(sql)
    End Function

    Private Function GetQuestion() As DataTable
        Dim sql As String = " SELECT * FROM tblQuestion q INNER JOIN tblAnswer a ON q.Question_Id = a.Question_Id "
        sql &= " WHERE q.Question_Id = '" + QuestionId + "';"
        Return db.getdata(sql)
    End Function

End Class