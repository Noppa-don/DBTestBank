Imports BusinessTablet360
Imports System.Data.SqlClient
Imports Microsoft.Practices.Unity

Public Class DashboardStudentPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Public IE As String


#Region "Dependency"

    Public Sub New()
        CType(Context.ApplicationInstance, Global_asax).Container.BuildUp(Me.GetType, Me)
    End Sub

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Request.QueryString("u") Is Nothing Then
            Dim u As String = Request.QueryString("u").ToString()
            Dim dt As DataTable = GetUserDetailForTablet(u)
            If dt.Rows.Count > 0 Then
                Session("UserId") = dt(0)("GUID")
                Session("UserName") = dt(0)("UserName")
                Session("FirstName") = dt(0)("FirstName")
                Session("LastName") = dt(0)("LastName")
                Session("IsAllowMenuManageUserSchool") = dt.Rows(0)("IsAllowMenuManageUserSchool")
                Session("IsAllowMenuManageUserAdmin") = dt.Rows(0)("IsAllowMenuManageUserAdmin")
                Session("IsAllowMenuAdminLog") = dt.Rows(0)("IsAllowMenuAdminLog")
                Session("IsAllowMenuContact") = dt.Rows(0)("IsAllowMenuContact")
                Session("IsAllowMenuSetEmail") = dt.Rows(0)("IsAllowMenuSetEmail")
                Session("SchoolID") = dt.Rows(0)("SchoolId")
                Session("SchoolCode") = dt.Rows(0)("SchoolId")
                Session("TeacherId") = dt.Rows(0)("SchoolId")
                Session("IsTeacher") = True
                Session("UnLoad") = False 'ใช้กับ signalR

                UserConfig.AddSchoolCode(dt.Rows(0)("SchoolId"))
                UserConfig.AddUserId(dt(0)("GUID"))

                If IsDBNull(dt.Rows(0)("FontSize")) Then
                    Session("FontSize") = 0
                Else
                    Session("FontSize") = dt.Rows(0)("FontSize")
                End If
                Dim ClsSelectSession As New ClsSelectSession()
                ClsSelectSession.NewSession("student/dashboardstudentpage.aspx")

                Log.Record(Log.LogType.StudentOpenTabletApp, "เปิดแท็บเล็ต (ครู)", True)

                'ส่วนของการเช็คว่าจะโชว์ Qtip แสดงการใช้งานในแต่ละหน้าหรือเปล่า    
                Dim ClsUserViewPageWithTip As New UserViewPageWithTip(dt(0)("GUID").ToString())
                If IsDBNull(dt.Rows(0)("IsViewAllTips")) Then
                    ClsUserViewPageWithTip.CheckIsViewAllTips(False)
                Else
                    ClsUserViewPageWithTip.CheckIsViewAllTips(dt.Rows(0)("IsViewAllTips"))
                End If
            End If
        End If

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("SchoolID") = "1000001"
        Session("SchoolCode") = "1000001"
        IE = "1"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
        knSession("SelectedCalendarName") = "เทอม 2 / 2556"
#End If

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
            Exit Sub
        End If
        Session("SelectedMode") = Nothing ' clear ค่า สำหรับ ไปหน้า teacherstudentdetailpage
        'Open Connection 
        Dim connDashboardStudent As New SqlConnection
        _DB.OpenExclusiveConnect(connDashboardStudent)

        Dim UserID As String = Session("UserId").ToString()
        MyCtlStudentList.SetByTeacher(UserID, connDashboardStudent)

        GetRoomHaveQuizAndHomework(connDashboardStudent)

    End Sub

    Private Function GetUserDetailForTablet(ByVal UserId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblUser LEFT JOIN tblUserSetting ON tblUser.GUID = tblUserSetting.User_Id ")
        sql.Append(" WHERE tblUser.GUID = '")
        sql.Append(UserId)
        sql.Append("';")
        GetUserDetailForTablet = New ClassConnectSql().getdata(sql.ToString())
    End Function

    Private Sub GetRoomHaveQuizAndHomework(Optional ByVal InputConn As SqlConnection = Nothing)
        Dim ClsStudent As New Service.ClsStudent((New ClassConnectSql))
        Dim dtClassRoom As DataTable = ClsStudent.GetClassNameHaveQuizOrHomework()
        'dtRoom.Columns.Add("ClassName", GetType(String))
        'dtRoom.Rows.Add("ป.1/1")
        'dtRoom.Rows.Add("ป.1/2")
        'dtRoom.Rows.Add("ป.1/3")
        'dtRoom.Rows.Add("ป.2/1")
        'dtRoom.Rows.Add("ป.2/3")
        'dtRoom.Rows.Add("ป.2/4")
        'dtRoom.Rows.Add("ม.1/1")
        'dtRoom.Rows.Add("ม.1/4")
        'dtRoom.Rows.Add("ม.2/1")
        'dtRoom.Rows.Add("ม.3/1")
        'dtRoom.Rows.Add("ม.4/2")
        'dtRoom.Rows.Add("ม.6/6")
        If dtClassRoom.Rows.Count > 0 Then
            Dim htmlRoom As New StringBuilder()
            For Each row In dtClassRoom.Rows
                htmlRoom.Append("<div>")
                htmlRoom.Append(row("ClassName"))
                htmlRoom.Append("</div>")
            Next
            RoomsHaveQuizAndHomework.InnerHtml = htmlRoom.ToString()
            RoomsHaveQuizAndHomework.Attributes.Add("class", "HaveRoom")
        Else
            RoomsHaveQuizAndHomework.InnerHtml = "<a href='../Homework/DashboardHomeworkPage.aspx' class='hint'>สั่งการบ้าน</a> หรือ <a href='../Quiz/DashboardQuizPage.aspx' class='hint'>ทำควิซ</a> ก่อนค่ะ"
            RoomsHaveQuizAndHomework.Attributes.Add("class", "NotHaveRoom")
        End If
    End Sub


End Class