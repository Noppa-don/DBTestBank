Imports System.Web.Services
Imports Telerik.Web.UI

Public Class SelectSession_Pad
    Inherits System.Web.UI.Page

    Public IsUseTablet As String
    Public DeviceId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"

        'If Not Page.IsPostBack Then
        '    TestMakeData()
        'End If

        If Request.QueryString("DeviceId") IsNot Nothing Then
            DeviceId = Request.QueryString("DeviceId")
            Dim ClsDroidpad As New ClassDroidPad(New ClassConnectSql)
            Dim UserId As String = ClsDroidpad.GetUserIdByDeviceId(Request.QueryString("DeviceId"))
            If UserId = "" Then
                ClsDroidpad = Nothing
                Response.Write("หา UserId จาก DeviceId นี้ไม่ได้")
            Else
                Session("UserId") = UserId
                Session("SchoolID") = New ClassConnectSql().ExecuteScalar(" SELECT SchoolId FROM tblUser WHERE GUID = '" & UserId & "' ;")
                IsUseTablet = "True"
                ClsDroidpad = Nothing
            End If
        End If


        If Application(Session("UserId").ToString()) Is Nothing Then
            Response.Write("ไม่มี ApplicationUserId นี้")
        End If



        If Not Page.IsPostBack Then
            GetDataFromApplication()
        End If

    End Sub

    Public Sub GetDataFromApplication()

        If Application(Session("UserId").ToString()) Is Nothing Then 'ถ้าไม่มีตัวแปร Application ของ UserId นี้ไม่สามารถหาค่าอะไรต่อได้เลย
            Response.Write("ไม่มี Session UserId")
        End If

        Dim ArrData As New ArrayList
        ArrData = Application(Session("UserId").ToString()) 'เอา Array มารับข้อมมูลจากตัวแปร Application เพราะว่าค่าของตัวแปร Application เป็น Array

        CreateDataByArrData(ArrData) 'สร้าง Datatable เพื่อเอาไป Bind ใน Gridview

    End Sub


    Private Sub CreateDataByArrData(ByVal ArrData As ArrayList)

        If ArrData.Count = 0 Then
            Response.Write("Array ไม่มีค่า")
        End If

        Dim dt As New DataTable
        'สร้าง Coulumn ให้ Datatable เพื่อเอาไป Bind ให้ Grid
        dt.Columns.Add("PK")
        dt.Columns.Add("ลำดับ")
        dt.Columns.Add("วัน-เวลา")
        dt.Columns.Add("ชุดข้อสอบ")
        dt.Columns.Add("ห้อง")
        dt.Columns.Add("ห้อง Lab")
        dt.Columns.Add("หน้าจอ")

        Dim BoundColumn As New Telerik.Web.UI.GridBoundColumn

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "PK"
        BoundColumn.DataField = "PK"
        BoundColumn.Visible = False

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "ลำดับ"
        BoundColumn.DataField = "ลำดับ"

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "วัน-เวลา"
        BoundColumn.DataField = "วัน-เวลา"

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "ชุดข้อสอบ"
        BoundColumn.DataField = "ชุดข้อสอบ"

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "ห้อง"
        BoundColumn.DataField = "ห้อง"

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "ห้อง Lab"
        BoundColumn.DataField = "ห้อง Lab"

        BoundColumn = New GridBoundColumn
        GVSession.MasterTableView.Columns.Add(BoundColumn)
        BoundColumn.HeaderText = "หน้าจอ"
        BoundColumn.DataField = "หน้าจอ"


        Dim Pk As New Integer
        Dim Index As New Integer
        Dim TimeStamp As New DateTime
        Dim TestSetName As String = ""
        Dim ClassName As String = ""
        Dim LabName As String = ""
        Dim ScreenName As String = ""

        For Each EachObj In ArrData 'วน Add ข้อมูลลง Datatable ทีละ Cls จนครบตามจำนวน Array ของ Application
            Dim ClsSessInfo As ClsSessionInFo = EachObj
            Pk = ClsSessInfo.PKInfo
            Index = ClsSessInfo.Index
            TimeStamp = ClsSessInfo.TimeStamp
            TestSetName = ClsSessInfo.TestSetName
            ClassName = ClsSessInfo.ClassName
            LabName = ClsSessInfo.LabName
            'ScreenName = ClsSessInfo.CurrentPage
            Dim ClsSelectSess As New ClsSelectSession
            Dim res As String = ResolveUrl("~").ToLower()
            Dim currentPage As String = ""
            If res = "/" Then
                currentPage = ClsSessInfo.CurrentPage.ToString()
            Else
                currentPage = ClsSessInfo.CurrentPage.ToString().Replace(res, "").Insert(0, "/")
            End If
            ScreenName = ClsSelectSess.ScreenName(currentPage)
            dt.Rows.Add(Pk, Index, TimeStamp, TestSetName, ClassName, LabName, ScreenName)
            ClsSessInfo = Nothing
        Next


        GVSession.DataSource = dt
        GVSession.DataBind()


    End Sub

    Private Sub BtnNewSession_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNewSession.Click

        Dim NewArrData As New ArrayList
        NewArrData = Application(Session("UserId").ToString())

        Dim DataIndex As Integer = 0


        For i = 0 To NewArrData.Count - 1
            Dim LastRound As Integer = NewArrData.Count - 1
            If i = LastRound Then
                Dim ObjClsSessInfo As ClsSessionInFo = NewArrData(i)
                DataIndex = ObjClsSessInfo.Index
                ObjClsSessInfo = Nothing
            End If
        Next

        Dim NewObjClsSessInfo As New ClsSessionInFo
        DataIndex += 1

        Dim clsSelectSess As New ClsSelectSession()
        Dim pkGuid = clsSelectSess.Number4Digit()

        NewObjClsSessInfo.PKInfo = pkGuid
        NewObjClsSessInfo.Index = DataIndex
        NewObjClsSessInfo.TimeStamp = DateTime.Now
        Dim res As String = ResolveUrl("~").ToLower()
        Dim currentPage As String = res & "student/dashboardstudentpage.aspx"
        NewObjClsSessInfo.CurrentPage = currentPage
        NewObjClsSessInfo.SchoolId = Session("SchoolID").ToString()
        Session("selectedSession") = pkGuid

        NewArrData.Add(NewObjClsSessInfo)

        Application(Session("UserId").ToString()) = NewArrData
        SetCalendarId()
        Response.Redirect("~/Student/DashboardStudentPage.aspx")

    End Sub

    Private Sub SetCalendarId()
        'Session CalendarID,CalendarName (Cuurent,Selected)
        Dim KnSession As New KNAppSession()
        If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
            Dim dtCalendar As DataTable = GetCalendarID(Session("SchoolID").ToString())
            'ค่าถาวร
            KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
            'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
            KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
        End If
    End Sub
    'get calendar from date now 
    Private Function GetCalendarID(ByVal SchoolID As String) As DataTable
        Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE GETDATE() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "'; "
        Dim db As New ClassConnectSql()
        GetCalendarID = db.getdata(sql)
    End Function


    <Services.WebMethod()>
    Public Shared Function SetSession(ByVal PkInFo As Integer)

        HttpContext.Current.Session("selectedSession") = PkInFo
        Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        Dim ClsSelectSession As New ClsSelectSession
        Dim currentPage As String = ClsSelectSession.checkCurrentPage(UserId, PkInFo.ToString())


        '
        'Session CalendarID,CalendarName (Cuurent,Selected)
        Dim KnSession As New KNAppSession()
        If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
            Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE GETDATE() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
            Dim db As New ClassConnectSql()
            Dim dtCalendar As DataTable = db.getdata(sql)
            'ค่าถาวร
            KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
            'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
            KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
        End If
        '


        Return currentPage

    End Function


    Private Sub GVSession_ColumnCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridColumnCreatedEventArgs) Handles GVSession.ColumnCreated
        e.Column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        e.Column.ItemStyle.HorizontalAlign = HorizontalAlign.Center
    End Sub



End Class
