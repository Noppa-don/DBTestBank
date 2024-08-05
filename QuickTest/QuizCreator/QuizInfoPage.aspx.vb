Public Class QuizInfoPage
    Inherits System.Web.UI.Page
    Public TestsetName As String
    Public TeacherName As String
    Public CreateDate As String
    Public TotalQuestion As String
    Public FullScore As String
    Public PassScore As String
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.QueryString("TestsetId") IsNot Nothing And Request.QueryString("TestsetId").ToString() <> "" Then
                FilltxtToLabel(Request.QueryString("TestsetId").ToString())
            End If
        End If

    End Sub

    Private Sub FilltxtToLabel(ByVal TestsetId As String)
        Dim sql As String = " SELECT tblTestSet.TestSet_Name, t360_tblTeacher.Teacher_FirstName + ' ' + t360_tblTeacher.Teacher_LastName AS CreateBy, " & _
                            " tblQuizCreatorTestset.QCT_UploadDate,COUNT(tblTestSetQuestionDetail.Question_Id) AS TotalQuestion, " & _
                            " COUNT(tblAnswer.Answer_Score) AS FullScore,dbo.tblQuizCreatorTestset.QCT_PassScore " & _
                            " FROM tblQuizCreatorTestset INNER JOIN tblTestSet ON tblQuizCreatorTestset.Testset_Id = tblTestSet.TestSet_Id " & _
                            " INNER JOIN t360_tblTeacher ON tblTestSet.UserId = t360_tblTeacher.Teacher_id " & _
                            " INNER JOIN tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id " & _
                            " INNER JOIN tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id " & _
                            " INNER JOIN tblQuestion ON tblTestSetQuestionDetail.Question_Id = tblQuestion.Question_Id " & _
                            " INNER JOIN tblAnswer ON tblQuestion.Question_Id = tblAnswer.Question_Id " & _
                            " WHERE dbo.tblQuizCreatorTestset.Testset_Id = '" & TestsetId & "' " & _
                            " And tblTestsetQuestionSet.IsActive = '1' and tbltestsetQuestionDetail.Isactive = '1' " & _
                            " GROUP BY tblTestSet.TestSet_Name, t360_tblTeacher.Teacher_FirstName + ' ' + t360_tblTeacher.Teacher_LastName, " & _
                            " tblQuizCreatorTestset.QCT_UploadDate, tblAnswer.Answer_Score,dbo.tblQuizCreatorTestset.QCT_PassScore "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            TestsetName = dt.Rows(0)("TestSet_Name")
            TeacherName = dt.Rows(0)("CreateBy")
            CreateDate = dt.Rows(0)("QCT_UploadDate").ToString()
            TotalQuestion = dt.Rows(0)("TotalQuestion")
            FullScore = dt.Rows(0)("FullScore").ToString()
            If dt.Rows(0)("QCT_PassScore") IsNot DBNull.Value Then
                PassScore = "ผ่าน " & dt.Rows(0)("QCT_PassScore").ToString() & " คะแนน"
            Else
                PassScore = ""
            End If
        End If
    End Sub

End Class