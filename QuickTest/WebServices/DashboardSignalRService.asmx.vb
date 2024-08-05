Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class DashboardSignalRService
    Inherits System.Web.Services.WebService

    Private ClsSelectSession As New ClsSelectSession()

#Region "JoinSession"
    <WebMethod(EnableSession:=True)> _
    Public Function JoinSession(ByVal PkSession As String, ByVal ClearSession As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        If ClearSession = "True" Then
            Relogin()
        End If
        HttpContext.Current.Session("selectedSession") = PkSession
        ClsSelectSession.SetTimeStamp()
        ClsSelectSession.SetCalendarId()
        HttpContext.Current.Session("ChooseMode") = ClsSelectSession.GetSessionChooseMode()
        HttpContext.Current.Session("PSubjectName") = ClsSelectSession.GetPSubjectName()
        HttpContext.Current.Session("PClassId") = ClsSelectSession.GetPClassId()
        HttpContext.Current.Session("objTestSet") = ClsSelectSession.GetObjTestset()
        HttpContext.Current.Session("EditID") = ClsSelectSession.GetEditId()
        HttpContext.Current.Session("Quiz_Id") = ClsSelectSession.GetSessionQuizId()
        HttpContext.Current.Session("QuizUseTablet") = ClsSelectSession.GetSessionQuizUseTablet()
        'HttpContext.Current.Session("StartTime") = ClsSelectSession.GetStartTimer()
        'HttpContext.Current.Session("t") = ClsSelectSession.GetT()
        Log.Record(Log.LogType.Login, "เข้าสู่ระบบแบบ session เก่า", True)
        JoinSession = ClsSelectSession.GetCurrentPage() & ClsSelectSession.GetCurrentQuerystring()
    End Function
#End Region


#Region "Set/Keep Session Re-Login"
    <WebMethod(EnableSession:=True)> _
    Public Sub Relogin()
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Exit Sub
        End If

        Dim s As Object = GetKeepSessionLogin()
        HttpContext.Current.Session.Clear() 'Clear all session 
        HttpContext.Current.Session("UserId") = s.UserId
        HttpContext.Current.Session("UserName") = s.UserName
        HttpContext.Current.Session("FirstName") = s.FirstName
        HttpContext.Current.Session("LastName") = s.LastName
        HttpContext.Current.Session("IsAllowMenuManageUserSchool") = s.IsAllowMenuManageUserSchool
        HttpContext.Current.Session("IsAllowMenuManageUserAdmin") = s.IsAllowMenuManageUserAdmin
        HttpContext.Current.Session("IsAllowMenuAdminLog") = s.IsAllowMenuAdminLog
        HttpContext.Current.Session("IsAllowMenuContact") = s.IsAllowMenuContact
        HttpContext.Current.Session("IsAllowMenuSetEmail") = s.IsAllowMenuSetEmail
        HttpContext.Current.Session("SchoolID") = s.SchoolId
        HttpContext.Current.Session("SchoolCode") = s.SchoolId
        HttpContext.Current.Session("TeacherId") = s.SchoolId
        HttpContext.Current.Session("IsTeacher") = True
        HttpContext.Current.Application("NeedEditButton") = s.NeedEditButton
        HttpContext.Current.Application("NeedJoinQ40") = s.NeedJoinQ40
        HttpContext.Current.Application("NeedQuizMode") = s.NeedQuizMode
        HttpContext.Current.Application("NeedAddEvaluationIndex") = s.NeedAddEvaluationIndex
        HttpContext.Current.Application("NeedHomeWork") = s.NeedHomeWork
        HttpContext.Current.Application("NeedReportButton") = s.NeedReportButton
        HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = s.NeedAddNewQCatAndQsetButton
        HttpContext.Current.Application("NeedDeleteQcatAndQset") = s.NeedDeleteQcatAndQset
        HttpContext.Current.Application("NeedAddNewQuestionButton") = s.NeedAddNewQuestionButton
        HttpContext.Current.Application("NeedDeleteQuestionButton") = s.NeedDeleteQuestionButton
        HttpContext.Current.Application("NeedSelectSesstion") = s.NeedSelectSesstion
        HttpContext.Current.Application("NeedChangePasswordMode") = s.NeedChangePasswordMode
        HttpContext.Current.Application("NeedManageSchoolInfo") = s.NeedManageSchoolInfo
        HttpContext.Current.Application("NeedEditQuestionCategory") = s.NeedEditQuestionCategory
        HttpContext.Current.Session("FontSize") = s.FontSize
        HttpContext.Current.Session("SuperUser") = s.SuperUser
    End Sub
    <WebMethod(EnableSession:=True)> _
    Private Function GetKeepSessionLogin() As Object
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim SessionLogin As Object =
            New With {
                .UserId = HttpContext.Current.Session("UserId"),
                .UserName = HttpContext.Current.Session("UserId"),
                .FirstName = HttpContext.Current.Session("FirstName"),
                .LastName = HttpContext.Current.Session("LastName"),
                .IsAllowMenuManageUserSchool = HttpContext.Current.Session("IsAllowMenuManageUserSchool"),
                .IsAllowMenuManageUserAdmin = HttpContext.Current.Session("IsAllowMenuManageUserAdmin"),
                .IsAllowMenuAdminLog = HttpContext.Current.Session("IsAllowMenuAdminLog"),
                .IsAllowMenuContact = HttpContext.Current.Session("IsAllowMenuContact"),
                .IsAllowMenuSetEmail = HttpContext.Current.Session("IsAllowMenuSetEmail"),
                .SchoolID = HttpContext.Current.Session("SchoolID"),
                .SchoolCode = HttpContext.Current.Session("SchoolCode"),
                .TeacherId = HttpContext.Current.Session("TeacherId"),
                .NeedEditButton = HttpContext.Current.Application("NeedEditButton"),
                .NeedJoinQ40 = HttpContext.Current.Application("NeedJoinQ40"),
                .NeedQuizMode = HttpContext.Current.Application("NeedQuizMode"),
                .NeedAddEvaluationIndex = HttpContext.Current.Application("NeedAddEvaluationIndex"),
                .NeedHomeWork = HttpContext.Current.Application("NeedHomeWork"),
                .NeedReportButton = HttpContext.Current.Application("NeedReportButton"),
                .NeedAddNewQCatAndQsetButton = HttpContext.Current.Application("NeedAddNewQCatAndQsetButton"),
                .NeedDeleteQcatAndQset = HttpContext.Current.Application("NeedDeleteQcatAndQset"),
                .NeedAddNewQuestionButton = HttpContext.Current.Application("NeedAddNewQuestionButton"),
                .NeedDeleteQuestionButton = HttpContext.Current.Application("NeedDeleteQuestionButton"),
                .NeedSelectSesstion = HttpContext.Current.Application("NeedSelectSesstion"),
                .NeedChangePasswordMode = HttpContext.Current.Application("NeedChangePasswordMode"),
                .NeedManageSchoolInfo = HttpContext.Current.Application("NeedManageSchoolInfo"),
                .NeedEditQuestionCategory = HttpContext.Current.Application("NeedEditQuestionCategory"),
                .FontSize = HttpContext.Current.Session("FontSize"),
                .SuperUser = HttpContext.Current.Session("SuperUser")
            }
        Return SessionLogin
    End Function
#End Region

#Region "Get SelectedSession"
    <WebMethod(EnableSession:=True)> _
    Public Function GetSelectSession() As String
        GetSelectSession = HttpContext.Current.Session("selectedSession")
    End Function
#End Region

#Region "Unload"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetUnload(ByVal unload As Boolean)
        HttpContext.Current.Session("UnLoad") = unload
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Function GetUnload() As Boolean
        GetUnload = HttpContext.Current.Session("UnLoad")
    End Function
#End Region

#Region "CurrentPage"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetCurrentPage(ByVal page As String)
        ClsSelectSession.SetCurrentPage(page)
        ClsSelectSession.SetTimeStamp()
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Function GetCurrentPage() As String
        GetCurrentPage = ClsSelectSession.GetCurrentPage()
    End Function
#End Region
#Region "CurrentQuerystring"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetCurrentQuerystring(ByVal Querystring As String)
        ClsSelectSession.SetCurrentQuerystring(Querystring)
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Function GetCurrentQuerystring() As String
        GetCurrentQuerystring = ClsSelectSession.GetCurrentQuerystring()
    End Function
#End Region

#Region "Set/Get QuizId"
    <WebMethod(EnableSession:=True)> _
    Public Sub GetSessionQuizId()
        HttpContext.Current.Session("Quiz_Id") = ClsSelectSession.GetSessionQuizId()
    End Sub
#End Region

#Region "Set/Get objTestset"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetObjTestset()
        ClsSelectSession.SetObjTestset()
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Sub GetObjTestset()
        HttpContext.Current.Session("objTestSet") = ClsSelectSession.GetObjTestset()
    End Sub
#End Region

#Region "Set/Get EditId"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetEditId()
        ClsSelectSession.setEditId()
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Sub GetEditId()
        HttpContext.Current.Session("EditID") = ClsSelectSession.GetEditId()
    End Sub
#End Region

#Region "Set/Get PClassId"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetPClassId()
        ClsSelectSession.SetPClassId()
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Sub GetPClassId()
        HttpContext.Current.Session("PClassId") = ClsSelectSession.GetPClassId()
    End Sub
#End Region

#Region "Set/Get PSubjectName"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetPSubjectName()
        ClsSelectSession.SetPSubjectName()
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Sub GetPSubjectName()
        HttpContext.Current.Session("PSubjectName") = ClsSelectSession.GetPSubjectName()
    End Sub
#End Region

#Region "ChooseMode"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetSessionChooseMode()
        ClsSelectSession.SetSessionChooseMode(HttpContext.Current.Session("ChooseMode"))
    End Sub
    <WebMethod(EnableSession:=True)> _
    Public Sub GetSessionChooseMode()
        HttpContext.Current.Session("ChooseMode") = ClsSelectSession.GetSessionChooseMode()
    End Sub
#End Region

#Region "Reset Value -- EditId newTestsetId objTestset"
    <WebMethod(EnableSession:=True)> _
    Public Sub SetNothingInSession()
        HttpContext.Current.Session("EditID") = Nothing
        ClsSelectSession.SetEditId()
        HttpContext.Current.Session("newTestSetId") = Nothing
        HttpContext.Current.Session("objTestSet") = Nothing
        ClsSelectSession.SetObjTestset()
        HttpContext.Current.Session("ChooseMode") = Nothing
        ClsSelectSession.SetSessionChooseMode(Nothing)
        HttpContext.Current.Session("Quiz_Id") = Nothing
        ClsSelectSession.SetSessionQuizId(Nothing)
        HttpContext.Current.Session("QuizUseTablet") = Nothing
        ClsSelectSession.SetSessionQuizUseTablet(Nothing)
        ClsSelectSession.SetExamNum(Nothing)
        ClsSelectSession.TestsetId = ""
        HttpContext.Current.Session("TotalStudent") = Nothing
        HttpContext.Current.Session("RefToCheckMark") = Nothing
        HttpContext.Current.Session("QuizUseTamplate") = Nothing
        HttpContext.Current.Session("NeedTimer") = Nothing
        HttpContext.Current.Session("ShowCorrectAfterComplete") = Nothing
        HttpContext.Current.Session("ShowCorrectAfterCompleteState") = Nothing
        HttpContext.Current.Session("showAnswer") = Nothing
        HttpContext.Current.Session("Selfpace") = Nothing
        HttpContext.Current.Session("_ExamAmount") = Nothing
        HttpContext.Current.Session("showAnswer") = Nothing
        HttpContext.Current.Session("showAnswer") = Nothing
    End Sub
#End Region

#Region "Clear Application Student EndQuiz"
    <WebMethod(EnableSession:=True)> _
    Public Sub ClearApplicationQuiz(ByVal DeviceUniqueID As String)
        Dim KNSession As New ClsKNSession()
        KNSession(DeviceUniqueID & "|" & "QuizId") = Nothing
        KNSession(DeviceUniqueID & "|" & "_ExmanNum") = Nothing
        KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = Nothing
        KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = Nothing
    End Sub
#End Region

#Region "Chat"
    <WebMethod(EnableSession:=True)> _
    Public Function GoToChat() As String
        If HttpContext.Current.Session("UserId") IsNot Nothing Then
            HttpContext.Current.Session("Owner_Id") = HttpContext.Current.Session("UserId").ToString()
            Return "Watch/Chat.aspx?ChatRoom_Id=3AFC0CB4-221F-4436-A5BF-C88D91F351E3"
        End If
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function ServiceCheckChatNotification() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        If HttpContext.Current.Session("UserId") IsNot Nothing Then
            Dim _DB As New ClassConnectSql()
            Dim sql As String = " SELECT COUNT(*) AS Totalnotification FROM tblChatJoin INNER JOIN tblChatMessage ON tblChatJoin.ChatRoom_Id = tblChatMessage.ChatRoom_Id INNER JOIN " & _
                                " tblChatRecipient ON tblChatMessage.CM_Id = tblChatRecipient.CM_Id INNER JOIN tblParent ON tblChatRecipient.ChatUser_Id = tblParent.PR_Id " & _
                                " WHERE (tblChatJoin.ChatUser_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') " & _
                                " AND (tblChatMessage.ChatFrom_Id <> '" & HttpContext.Current.Session("UserId").ToString() & "') AND " & _
                                " (tblChatRecipient.ChatSeen IS NULL) AND (tblChatJoin.IsActive = 1) AND (tblChatMessage.IsActive = 1) AND (tblChatRecipient.IsActive = 1) "
            Dim TotalNotification As Integer = _DB.ExecuteScalar(sql)
            Return TotalNotification
        End If
    End Function

#End Region

#Region "SchoolNews"
    <WebMethod(EnableSession:=True)> _
    Public Function CheckIsHaveCurrentNews() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Not Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid)) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        If HttpContext.Current.Session("SchoolCode") IsNot Nothing And HttpContext.Current.Session("SchoolCode").ToString() <> "" Then
            Dim IsStudent As Boolean = False
            Dim sql As String

            sql = " select 'Teacher' as UserType ,Teacher_CurrentClass as CurrentClass,Teacher_CurrentRoom as CurrentRoom from t360_tblTeacher "
            sql &= " where Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' And Teacher_IsActive = '1'"

            Dim dt As DataTable = _DB.getdata(sql)

            If dt.Rows.Count = 0 Then
                sql = " select 'Student' as UserType,Student_CurrentClass as CurrentClass,Student_CurrentRoom as CurrentRoom,Student_CurrentRoomId from t360_tblStudent "
                sql &= " where Student_id = '" & HttpContext.Current.Session("UserId").ToString() & "' And Student_IsActive = '1'"
                dt = _DB.getdata(sql)
                IsStudent = True
            End If

            If dt.Rows.Count <> 0 Then

                sql = "select sum(NewsAmount) as TotalNewsAmount from (select count(t360_tblnews.news_Id) as NewsAmount from t360_tblNews "
                sql &= " inner join t360_tblNewsRoom on t360_tblnews.news_Id  =  t360_tblNewsRoom.news_Id "
                sql &= " where t360_tblnews.News_IsActive = 1 and t360_tblNewsRoom.IsActive = '1' "
                sql &= " and dbo.GetThaiDate() between t360_tblNews.News_StartDate and t360_tblNews.News_EndDate"

                If dt.Rows(0)("UserType") = "Teacher" And dt.Rows(0)("CurrentRoom") IsNot Nothing Then
                    sql &= " and t360_tblNewsRoom.Class_Name = '" & dt.Rows(0)("CurrentClass") & "' "
                    sql &= " and t360_tblNewsRoom.Room_Name = '" & dt.Rows(0)("CurrentRoom") & "' and t360_tblNews.News_ToTeacher = '1'"
                ElseIf dt.Rows(0)("UserType") = "Teacher" And dt.Rows(0)("CurrentRoom") Is Nothing Then
                    sql &= "and t360_tblNews.News_ToTeacherNoRoom = '1'"
                ElseIf dt.Rows(0)("UserType") = "Student" Then
                    sql &= " and t360_tblNewsRoom.Class_Name = '" & dt.Rows(0)("CurrentClass") & "' "
                    sql &= " and t360_tblNewsRoom.Room_Name = '" & dt.Rows(0)("CurrentRoom") & "' and t360_tblNews.News_ToStudent = '1'"
                End If

                If IsStudent Then
                    'sql &= " union  SELECT count(*) FROM tblTeacherNews tn INNER JOIN tblTeacherNewsDetail tnd ON tn.TN_Id = tnd.TN_Id  "
                    'sql &= " INNER JOIN t360_tblTeacher t ON tn.Teacher_Id = t.Teacher_id  WHERE (tn.IsActive = 1 AND tnd.IsActive = 1) "
                    'sql &= " AND (dbo.GetThaiDate() BETWEEN tn.StartDate AND tn.EndDate)  AND (tnd.Student_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') "
                    'sql &= " OR (tnd.Room_Id = '" & dt.Rows(0)("Student_CurrentRoomId").ToString & "')  AND (tn.School_Id = '" & HttpContext.Current.Session("SchoolCode").ToString() & "')"

                    sql &= " union  SELECT count(*)  FROM tblTeacherNews tn INNER JOIN tblTeacherNewsDetail tnd ON tn.TN_Id = tnd.TN_Id  "
                    'sql &= " INNER JOIN t360_tblTeacher t ON tn.Teacher_Id = t.Teacher_id  "
                    sql &= " WHERE tn.IsActive = 1 AND tnd.IsActive = 1 AND (dbo.GetThaiDate() BETWEEN tn.StartDate AND tn.EndDate)  "
                    sql &= " AND (tnd.Student_Id = '" & HttpContext.Current.Session("UserId").ToString() & "' "
                    sql &= " OR tnd.Room_Id = '" & dt.Rows(0)("Student_CurrentRoomId").ToString & "')  AND tn.School_Id = '" & HttpContext.Current.Session("SchoolCode").ToString() & "'"
                End If

                sql &= ") as NewsAmountFrom360AndTeacher"

                Dim IsHaveNews As Integer = CInt(_DB.ExecuteScalar(sql))

                If IsHaveNews > 0 Then
                    Return "Show"
                Else
                    Return "NotShow"
                End If
            Else
                Return "0"
            End If
        Else
            Return "0"

        End If

    End Function
#End Region

End Class
