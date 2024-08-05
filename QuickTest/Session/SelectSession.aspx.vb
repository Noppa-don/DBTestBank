Imports System.Web.Services
Imports Telerik.Web.UI
Imports Microsoft.Practices.Unity
Imports KnowledgeUtils

Public Class SelectSession
    Inherits System.Web.UI.Page

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

    Public lb As String = "False"
    Public HaveSelected As String = "False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("SchoolID") = "1000001"
        Dim a As New ClsSessionInFo
        a.PKInfo = 123
        a.TimeStamp = DateTime.Now
        a.CurrentPage = "student/dashboardstudentpage.aspx"
        a.CurrentQuerystring = ""
        a.SchoolId = "1000001"
        Dim b As New ClsSessionInFo
        b.PKInfo = 555
        b.TimeStamp = DateTime.Now
        b.CurrentPage = "testset/dashboardsetuppage.aspx"
        b.CurrentQuerystring = ""
        b.SchoolId = "1000001"
        Dim ArrApplication As New ArrayList()
        ArrApplication.Add(a)
        ArrApplication.Add(b)
        HttpContext.Current.Application(Session("UserId").ToString()) = ArrApplication
#End If


        'If Not Page.IsPostBack Then
        '    TestMakeData()
        'End If

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
                If IsDBNull(dt.Rows(0)("FontSize")) Then
                    Session("FontSize") = 0
                Else
                    Session("FontSize") = dt.Rows(0)("FontSize")
                End If
            End If
        End If


        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        End If

        If Application(Session("UserId").ToString()) Is Nothing Then
            Response.Write("ไม่มี ApplicationUserId นี้")
        End If

        If Not Request.QueryString("lb") Is Nothing Then
            'BtnNewSession.Attributes.Add("OnClientClick", "ParentReload();")
            lb = "True"
            BtnNewSession.OnClientClick = "ParentReload();"
            HaveSelected = HttpContext.Current.Session("selectedSession").ToString()
        End If
        'If Not HttpContext.Current.Session("selectedSession") Is Nothing Then

        'End If

        If Not Page.IsPostBack Then
            Dim ArrData As New ArrayList
            ArrData = Application(Session("UserId").ToString()) 'เอา Array มารับข้อมมูลจากตัวแปร Application เพราะว่าค่าของตัวแปร Application เป็น Array
            If lb = "True" Then
                CreateDataOnLightbox(ArrData)
            Else
                CreateDataByArrData(ArrData) 'สร้าง Datatable เพื่อเอาไป Bind ใน Gridview
            End If
        End If

    End Sub

    Private Function GetUserDetailForTablet(ByVal UserId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblUser LEFT JOIN tblUserSetting ON tblUser.GUID = tblUserSetting.User_Id ")
        sql.Append(" WHERE tblUser.GUID = '")
        sql.Append(UserId)
        sql.Append("';")
        GetUserDetailForTablet = New ClassConnectSql().getdata(sql.ToString())
    End Function

    Private Sub BtnNewSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNewSession.Click
        Dim querystring As String = ""
        If Request.QueryString("u") IsNot Nothing Then
            querystring = "?u=" & Request.QueryString("u").ToString()
        End If

        Dim ClsSelectSession As New ClsSelectSession()
        Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
        If Request.QueryString("lb") Is Nothing Then
            ClsSelectSession.NewSession(CurrentPageFromRunMode)
            Log.Record(Log.LogType.Login, "เข้าสู่ระบบแบบ session ใหม่", True)
            Log.Record(Log.LogType.Login, querystring, True)
            Log.Record(Log.LogType.Login, "~/" & CurrentPageFromRunMode & querystring, True)
            Response.Redirect("~/" & CurrentPageFromRunMode & querystring)
        Else
            Dim d As New DashboardSignalRService()
            d.Relogin()
            ClsSelectSession.NewSession(CurrentPageFromRunMode)
            Log.Record(Log.LogType.Login, "เข้าสู่ระบบแบบ session ใหม่จาก lightbox", True)
            Response.Redirect("~/" & CurrentPageFromRunMode & querystring)
        End If
    End Sub

    Private Sub CreateDataByArrData(ByVal ArrData As ArrayList)
        If ArrData.Count = 0 Then
            Response.Write("Array ไม่มีค่า")
        Else
            Dim dt As New DataTable
            Dim ArrStrColumn As Array = {"PK", "ลำดับ", "หน้าจอ", "ห้อง", "วัน-เวลา", "ชุดข้อสอบ", "ห้อง Lab"}
            Dim BoundColumn As New Telerik.Web.UI.GridBoundColumn

            For i As Integer = 0 To ArrStrColumn.Length - 1
                'สร้าง Coulumn ให้ Datatable เพื่อเอาไป Bind ให้ Grid
                dt.Columns.Add(ArrStrColumn(i))
                'สร้าง grid column
                BoundColumn = New GridBoundColumn
                GVSession.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = ArrStrColumn(i)
                BoundColumn.DataField = ArrStrColumn(i)
                If i = 0 Or i = 5 Or i = 6 Then
                    BoundColumn.Visible = False
                End If
            Next

            Dim Pk As New Integer
            'Dim Cur_status As String
            Dim Index As Integer = 1
            Dim TimeStamp As New DateTime
            Dim TestSetName As String = ""
            Dim ClassName As String = ""
            Dim LabName As String = ""
            Dim ScreenName As String = ""
            Dim ClsSelectSession As New ClsSelectSession()

            Dim timeString As String

            For Each EachObj In ArrData 'วน Add ข้อมูลลง Datatable ทีละ Cls จนครบตามจำนวน Array ของ Application
                'Cur_status = ""
                Dim ClsSessInfo As ClsSessionInFo = EachObj
                Pk = ClsSessInfo.PKInfo
                'If Not Session("selectedSession") Is Nothing And Not Request.QueryString("lb") Is Nothing Then
                '    If CInt(Session("selectedSession")) = Pk Then
                '        Cur_status = "<img src=""../images/ApproveButton.png"" style='position:absolute;left:0;top:0;z-index:1;'/>"
                '    End If
                'End If
                'TimeStamp = ClsSessInfo.TimeStamp
                timeString = Convert.ToDateTime(ClsSessInfo.TimeStamp).ToPointPlusTime()

                TestSetName = ClsSessInfo.TestSetName
                ClassName = If((ClsSessInfo.ClassName <> ""), ClsSessInfo.ClassName, "-")
                LabName = ClsSessInfo.LabName
                ScreenName = ClsSelectSession.ScreenName(ClsSessInfo.CurrentPage.ToString())
                'Dim txtHmtl As String = "<div style='position:relative;'><div style='z-index: 2;position: relative;'>" & Index & "</div>" & Cur_status & "</div>"
                dt.Rows.Add(Pk, Index, ScreenName, ClassName, timeString, TestSetName, LabName)
                ClsSessInfo = Nothing
                Index += 1
            Next
            GVSession.DataSource = dt
            GVSession.DataBind()
        End If
    End Sub

    Private Sub CreateDataOnLightbox(ArrData As ArrayList)
        Dim dt As New DataTable
        Dim ArrStrColumn As Array = {"PK", "ลำดับ", "หน้าจอ", "ห้อง", "วัน-เวลา"}
        Dim BoundColumn As New Telerik.Web.UI.GridBoundColumn

        For i As Integer = 0 To ArrStrColumn.Length - 1
            'สร้าง Coulumn ให้ Datatable เพื่อเอาไป Bind ให้ Grid
            dt.Columns.Add(ArrStrColumn(i))
            'สร้าง grid column
            BoundColumn = New GridBoundColumn
            GVSession.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = ArrStrColumn(i)
            BoundColumn.DataField = ArrStrColumn(i)
            If i = 0 Then
                BoundColumn.Visible = False
            End If
        Next

        Dim Pk As New Integer
        Dim Cur_status As String
        Dim Index As Integer = 1
        Dim TimeStamp As New DateTime
        Dim TestSetName As String = ""
        Dim ClassName As String = ""
        Dim LabName As String = ""
        Dim ScreenName As String = ""
        Dim ClsSelectSession As New ClsSelectSession()

        Dim timeString As String

        Dim ActivityName As New StringBuilder()
        Dim ActivityTime As New StringBuilder()

        For Each EachObj In ArrData 'วน Add ข้อมูลลง Datatable ทีละ Cls จนครบตามจำนวน Array ของ Application
            Cur_status = ""
            Dim ClsSessInfo As ClsSessionInFo = EachObj
            Pk = ClsSessInfo.PKInfo
            If Not Session("selectedSession") Is Nothing Then
                If CInt(Session("selectedSession")) = Pk Then
                    Cur_status = "<img src=""../images/CurrentSession.png"" style='position:absolute;left:0;top:0;z-index:1;'/>"
                End If
            End If
            Dim txtHmtl As String = "<div style='position:relative;'><div style='z-index: 2;position: relative;'>" & Index & "</div>" & Cur_status & "</div>"


            ClassName = ClsSessInfo.ClassName
            LabName = ClsSessInfo.LabName

            ScreenName = ClsSelectSession.ScreenName(ClsSessInfo.CurrentPage.ToString())
            TestSetName = ClsSessInfo.TestSetName
            ActivityName.Append("<hh>")
            ActivityName.Append(ScreenName)
            ActivityName.Append("</hh><br/>")
            ActivityName.Append(TestSetName)

            timeString = Convert.ToDateTime(ClsSessInfo.TimeStamp).ToPointPlusTime()
            ActivityTime.Append("<hh>")
            ActivityTime.Append(timeString)
            ActivityTime.Append("</hh><br/>")
            If LabName <> "" Then
                ActivityTime.Append(LabName)
                ActivityTime.Append(" - ")
                ActivityTime.Append(ClassName)
            Else
                If ClassName <> "" Then
                    ActivityTime.Append(ClassName)
                End If
            End If

            ClassName = If((ClassName <> ""), ClassName, "-")

            dt.Rows.Add(Pk, txtHmtl, ActivityName.ToString(), ClassName, ActivityTime.ToString())
            ClsSessInfo = Nothing
            ActivityName.Clear()
            ActivityTime.Clear()
            Index += 1
        Next
        GVSession.DataSource = dt
        GVSession.DataBind()
    End Sub

    Private Sub GVSession_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles GVSession.ColumnCreated
        e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center
    End Sub

    'Private Sub Relogin()
    '    Dim s As Object = GetKeepSessionLogin()
    '    Session.Clear() 'Clear all session 
    '    Session("UserId") = s.UserId
    '    Session("UserName") = s.UserName
    '    Session("FirstName") = s.FirstName
    '    Session("LastName") = s.LastName
    '    Session("IsAllowMenuManageUserSchool") = s.IsAllowMenuManageUserSchool
    '    Session("IsAllowMenuManageUserAdmin") = s.IsAllowMenuManageUserAdmin
    '    Session("IsAllowMenuAdminLog") = s.IsAllowMenuAdminLog
    '    Session("IsAllowMenuContact") = s.IsAllowMenuContact
    '    Session("IsAllowMenuSetEmail") = s.IsAllowMenuSetEmail
    '    Session("SchoolID") = s.SchoolId
    '    Session("SchoolCode") = s.SchoolId
    '    Session("TeacherId") = s.SchoolId
    '    Session("IsTeacher") = True
    '    Session("NeedEditButton") = s.NeedEditButton
    '    Session("NeedJoinQ40") = s.NeedJoinQ40
    '    Session("NeedQuizMode") = s.NeedQuizMode
    '    Session("NeedAddEvaluationIndex") = s.NeedAddEvaluationIndex
    '    Session("NeedHomeWork") = s.NeedHomeWork
    '    Session("NeedReportButton") = s.NeedReportButton
    '    Session("NeedAddNewQCatAndQsetButton") = s.NeedAddNewQCatAndQsetButton
    '    Session("NeedDeleteQcatAndQset") = s.NeedDeleteQcatAndQset
    '    Session("NeedAddNewQuestionButton") = s.NeedAddNewQuestionButton
    '    Session("NeedDeleteQuestionButton") = s.NeedDeleteQuestionButton
    '    Session("NeedSelectSesstion") = s.NeedSelectSesstion
    '    Session("NeedChangePasswordMode") = s.NeedChangePasswordMode
    '    Session("NeedManageSchoolInfo") = s.NeedManageSchoolInfo
    '    Session("NeedEditQuestionCategory") = s.NeedEditQuestionCategory
    '    Session("FontSize") = s.FontSize
    '    Session("SuperUser") = s.SuperUser
    'End Sub

    'Private Function GetKeepSessionLogin() As Object
    '    Dim SessionLogin As Object =
    '        New With {
    '            .UserId = HttpContext.Current.Session("UserId"),
    '            .UserName = HttpContext.Current.Session("UserId"),
    '            .FirstName = HttpContext.Current.Session("FirstName"),
    '            .LastName = HttpContext.Current.Session("LastName"),
    '            .IsAllowMenuManageUserSchool = HttpContext.Current.Session("IsAllowMenuManageUserSchool"),
    '            .IsAllowMenuManageUserAdmin = HttpContext.Current.Session("IsAllowMenuManageUserAdmin"),
    '            .IsAllowMenuAdminLog = HttpContext.Current.Session("IsAllowMenuAdminLog"),
    '            .IsAllowMenuContact = HttpContext.Current.Session("IsAllowMenuContact"),
    '            .IsAllowMenuSetEmail = HttpContext.Current.Session("IsAllowMenuSetEmail"),
    '            .SchoolID = HttpContext.Current.Session("SchoolID"),
    '            .SchoolCode = HttpContext.Current.Session("SchoolCode"),
    '            .TeacherId = HttpContext.Current.Session("TeacherId"),
    '            .NeedEditButton = HttpContext.Current.Session("NeedEditButton"),
    '            .NeedJoinQ40 = HttpContext.Current.Session("NeedJoinQ40"),
    '            .NeedQuizMode = HttpContext.Current.Session("NeedQuizMode"),
    '            .NeedAddEvaluationIndex = HttpContext.Current.Session("NeedAddEvaluationIndex"),
    '            .NeedHomeWork = HttpContext.Current.Session("NeedHomeWork"),
    '            .NeedReportButton = HttpContext.Current.Session("NeedReportButton"),
    '            .NeedAddNewQCatAndQsetButton = HttpContext.Current.Session("NeedAddNewQCatAndQsetButton"),
    '            .NeedDeleteQcatAndQset = HttpContext.Current.Session("NeedDeleteQcatAndQset"),
    '            .NeedAddNewQuestionButton = HttpContext.Current.Session("NeedAddNewQuestionButton"),
    '            .NeedDeleteQuestionButton = HttpContext.Current.Session("NeedDeleteQuestionButton"),
    '            .NeedSelectSesstion = HttpContext.Current.Session("NeedSelectSesstion"),
    '            .NeedChangePasswordMode = HttpContext.Current.Session("NeedChangePasswordMode"),
    '            .NeedManageSchoolInfo = HttpContext.Current.Session("NeedManageSchoolInfo"),
    '            .NeedEditQuestionCategory = HttpContext.Current.Session("NeedEditQuestionCategory"),
    '            .FontSize = HttpContext.Current.Session("FontSize"),
    '            .SuperUser = HttpContext.Current.Session("FontSize")
    '        }
    '    Return SessionLogin
    'End Function

#Region "Old Code"
    'Private Sub BtnNewSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNewSession.Click

    '    Dim NewArrData As New ArrayList
    '    NewArrData = Application(Session("UserId").ToString())

    '    Dim DataIndex As Integer = 0


    '    For i = 0 To NewArrData.Count - 1
    '        Dim LastRound As Integer = NewArrData.Count - 1
    '        If i = LastRound Then
    '            Dim ObjClsSessInfo As ClsSessionInFo = NewArrData(i)
    '            DataIndex = ObjClsSessInfo.Index
    '            ObjClsSessInfo = Nothing
    '        End If
    '    Next

    '    Dim NewObjClsSessInfo As New ClsSessionInFo
    '    DataIndex += 1

    '    Dim clsSelectSess As New ClsSelectSession()
    '    Dim pkGuid = clsSelectSess.Number4Digit()

    '    NewObjClsSessInfo.PKInfo = pkGuid
    '    NewObjClsSessInfo.Index = DataIndex
    '    NewObjClsSessInfo.TimeStamp = DateTime.Now
    '    Dim res As String = ResolveUrl("~").ToLower()
    '    Dim currentPage As String = res & "student/dashboardstudentpage.aspx"
    '    NewObjClsSessInfo.CurrentPage = currentPage
    '    NewObjClsSessInfo.SchoolId = Session("SchoolID").ToString()
    '    Session("selectedSession") = pkGuid

    '    NewArrData.Add(NewObjClsSessInfo)

    '    Application(Session("UserId").ToString()) = NewArrData

    '    SetCalendarId()
    '    Response.Redirect("~/Student/DashboardStudentPage.aspx")

    'End Sub


    'Private Sub SetCalendarId()

    '    'Session CalendarID,CalendarName (Cuurent,Selected)
    '    Dim KnSession As New KNAppSession()
    '    If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
    '        Dim dtCalendar As DataTable = GetCalendarID(Session("SchoolID").ToString())
    '        'ค่าถาวร
    '        KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
    '        KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
    '        'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
    '        KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
    '        KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
    '    End If

    'End Sub

    'get calendar from date now 
    'Private Function GetCalendarID(ByVal SchoolID As String) As DataTable
    '    Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "'; "
    '    Dim db As New ClassConnectSql()
    '    GetCalendarID = db.getdata(sql)
    'End Function


    '<Services.WebMethod()>
    'Public Shared Function SetSession(ByVal PkInFo As Integer)

    '    HttpContext.Current.Session("selectedSession") = PkInFo
    '    Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
    '    Dim ClsSelectSession As New ClsSelectSession
    '    Dim currentPage As String = ClsSelectSession.checkCurrentPage(UserId, PkInFo.ToString())

    '    '
    '    'Session CalendarID,CalendarName (Cuurent,Selected)
    '    Dim KnSession As New KNAppSession()
    '    If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
    '        Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
    '        Dim db As New ClassConnectSql()
    '        Dim dtCalendar As DataTable = db.getdata(sql)
    '        'ค่าถาวร
    '        KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
    '        KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
    '        'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
    '        KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
    '        KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
    '    End If
    '    '

    '    Return currentPage

    'End Function

#End Region

End Class
