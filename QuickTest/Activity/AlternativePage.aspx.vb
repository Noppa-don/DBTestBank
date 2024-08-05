Imports KnowledgeUtils

Public Class Alternative2
    Inherits System.Web.UI.Page
    Public GroupName As String
    Public VBQuizId As String
    Public NewQuizButtonName As String

    Public IE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("QuizUseTablet") = "True"
        IE = "1"
#End If
        If Session("ChooseMode") = EnumDashBoardType.Practice Then
            NewQuizButtonName = "Practice"
        ElseIf Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            NewQuizButtonName = "PracticeFromComputer"

        ElseIf Session("ChooseMode") = EnumDashBoardType.Quiz Then
            NewQuizButtonName = "Quiz"
        End If
        Log.Record(Log.LogType.PageLoad, "pageload alternative", False)
        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "alternative session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        Else
            If Not Page.IsPostBack Then
                If Not IsNothing(Session("Quiz_Id")) Then
                    VBQuizId = HttpContext.Current.Session("Quiz_Id").ToString()
                    Dim objDroidPad As New ClassDroidPad(New ClassConnectSql)
                    objDroidPad.CloseQuiz(Session("Quiz_Id").ToString())
                    objDroidPad = Nothing
                    SetQuizExpire()
                    ClearApplicationInQuiz(Session("Quiz_Id").ToString()) ' clear application ที่ใช้ใน quiz แบบไปพร้อมกัน ไม่สลับคำถามคำตอบ ที่ลดจำนวนการ query 
                    HttpContext.Current.Application.Remove("Quiz_" & Session("Quiz_Id").ToString()) 'remove application ที่ใช้สำหรับ getnextaction 
                    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
                    GroupName = ClsActivity.GetGroupNameFromQuizId(Session("Quiz_Id").ToString()) 'หา GroupName เพื่อเอาไปใช้ AddGroup SignalR
                    ClsActivity = Nothing
                End If
            End If
        End If

        
    End Sub

    Private Sub ClearApplicationInQuiz(ByVal QuizId As String)
        For Each AppName In HttpContext.Current.Application.AllKeys
            If AppName.IndexOf(QuizId) > -1 Then
                HttpContext.Current.Application.Remove(AppName)
            End If
        Next
    End Sub

    Private Sub SetQuizExpire()
        If Session("QuizUseTablet") = True Then
            Dim redis As New RedisStore()
            Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
            For Each m In redis.SMembers(QuizId)
                redis.DEL(m)
            Next
            redis.DEL(QuizId)
        End If
    End Sub

    'Protected Sub btnViewReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewReport.Click
    '    Response.Redirect("../Activity/ActivityReport.aspx")
    'End Sub

    Protected Sub BtnReview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnReview.Click
        'Dim ClsSelectSession As New ClsSelectSession()
        'ClsSelectSession.SetCurrentPage("activity/reviewmostwronganswer.aspx")
        Response.Redirect("../Activity/ReviewMostWrongAnswer.aspx")
    End Sub

    Protected Sub BtnPracticeFromComputer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPracticeFromComputer.Click
        Dim objTestSet As ClsTestSet
        Dim dt As New DataTable


        If Session("PClassId") IsNot Nothing And Session("PSubjectName") IsNot Nothing Then
            Response.Redirect("../PracticeMode_Pad/ChooseQuestionSet.aspx")
        Else
            dt = objTestSet.GetClassAndSubjectFromQuizId(VBQuizId)
            If dt.Rows.Count = 0 Then
                Response.Redirect("../LoginPage.aspx")
            Else
                Session("PClassId") = dt.Rows(0)("Level_Id")
                Session("PSubjectName") = dt.Rows(0)("GroupSubject_Id")
                Response.Redirect("../PracticeMode_Pad/ChooseQuestionSet.aspx")
            End If
        End If

    End Sub

    Private Sub BtnQuiz_Click(sender As Object, e As EventArgs) Handles BtnQuiz.Click
        EndQuiz()
        Response.Redirect("../Quiz/DashboardQuizPage.aspx")
    End Sub

    Private Sub BtnPractice_Click(sender As Object, e As EventArgs) Handles BtnPractice.Click
        EndQuiz()
        Response.Redirect("../Practice/DashboardPracticePage.aspx")
    End Sub

    Private Sub EndQuiz()
        Dim c As New ClsSelectSession()
        c.SetClassName("")
    End Sub
End Class