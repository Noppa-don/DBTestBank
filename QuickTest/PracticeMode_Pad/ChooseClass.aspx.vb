Imports BusinessTablet360
Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class ChooseClass
    Inherits System.Web.UI.Page

    Dim _DB As New ClassConnectSql
    Public txtStep1, txtStep2, txtStep3, txtStep4, ChkFontSize, MarginSize, PaddingSize As String
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Public GroupName As String
    Public DVID As String

    Dim UseCls As New ClassConnectSql
    Public IE As String
    Protected IsAndroid As Boolean

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected TokenId As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

        If Not Page.IsPostBack Then
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If

            Session("SessionStatus") = "1"
        End If

        Session("ChooseMode") = Request.QueryString("DashboardMode")

        If ClsKNSession.IsMaxONet Then
            If Session("ChooseMode") Is Nothing Then
                Session("ChooseMode") = Request.QueryString("DashboardMode")
            End If
            Session("selectedSession") = "PracticeFromComputer"
            TokenId = Request.QueryString("token")
        End If

        'Open Connection
        Dim connActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(connActivity)

        If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            'เข้าจาก Dashboard ฝึกฝน มี session อยู่แล้วไม่ต้องเก็บใหม่
            If Session("UserId") Is Nothing Then
                'เข้าจากหน้า Login
                If HttpContext.Current.Application("DefaultSchoolCode").ToString().Trim() <> "-1" Then
                    Session("SchoolCode") = HttpContext.Current.Application("DefaultSchoolCode")
                Else
                    Session("SchoolCode") = "1000000"
                End If
                Session("UserId") = HttpContext.Current.Application("DefaultUserId")
                Session("SchoolID") = Session("SchoolCode")
                Session("selectedSession") = "PracticeFromComputer"
                SetCalendarId(connActivity)
            End If
            Session("IsTeacher") = "1"
            Session("PracticeFromComputer") = True
            Session("PracticeMode") = True

        ElseIf Session("PDeviceId") IsNot Nothing Then
            'เด็ก
            DVID = Session("PDeviceId")
            If Session("selectedSession") Is Nothing Then
                Session("selectedSession") = ClsActivity.GetGroupNameByDVID(DVID, connActivity)
            End If
            GroupName = Session("selectedSession").ToString()
            If Session("UserId") Is Nothing Then
                Session("UserId") = ClsActivity.GetPlayerIdByDeviceId(Session("PDeviceId").ToString, connActivity)
            End If

            Session("IsTeacher") = "0"

            If Not Page.IsPostBack Then
                ' ส่วนของการแสดง qtip
                If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                    Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                    Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                    NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
                End If
            End If
        Else
            Session("IsTeacher") = "1"
        End If

        CreateNav()
        CreateButtonClass()

        'Close Connection
        UseCls.CloseExclusiveConnect(connActivity)
    End Sub

    Private Sub SetCalendarId(Optional ByRef InputConn As SqlConnection = Nothing)
        'Session CalendarID,CalendarName (Cuurent,Selected)
        Dim KnSession As New KNAppSession()
        If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
            Dim dtCalendar As DataTable = GetCalendarID(Session("SchoolID").ToString(), InputConn)
            'ค่าถาวร
            KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
            'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
            KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
        End If
    End Sub

    Private Function GetCalendarID(ByVal SchoolID As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "' AND IsActive = 1; "
        Dim db As New ClassConnectSql()
        Dim dt As DataTable
        dt = db.getdata(sql, , InputConn)
        If dt.Rows.Count = 0 Then
            dt.Clear()
            sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE Calendar_Type = '3' AND School_Code = '" & SchoolID & "' " &
                  " AND dbo.GetThaiDate() >= Calendar_ToDate AND IsActive = 1 ORDER BY Calendar_ToDate DESC; "
            dt = db.getdata(sql, , InputConn)
        End If
        Return dt
    End Function

    Private Sub SetSession(ByVal DashboardMode As Integer)
        Session("ChooseMode") = DashboardMode
    End Sub

    Private Sub CreateNav()
        If Session("ChooseMode") = EnumDashBoardType.Homework Then
            ChkFontSize = "21px"
            MarginSize = "60px"
            txtStep1 = " เลือกชั้น -->"
            txtStep2 = " เลือกวิชา -->"
            txtStep3 = " เลือกหน่วยฯ -->"
            txtStep4 = " สั่งการบ้าน"
            PaddingSize = "8px 10px 6px 10px"
        ElseIf Session("ChooseMode") = EnumDashBoardType.PrintTestset Then
            ChkFontSize = "21px"
            MarginSize = "60px"
            txtStep1 = " เลือกชั้น -->"
            txtStep2 = " เลือกวิชา -->"
            txtStep3 = " เลือกหน่วยฯ -->"
            txtStep4 = " สั่งพิมพ์ใบงาน"
            PaddingSize = "8px 10px 6px 10px"
        Else
            ChkFontSize = "25px"
            MarginSize = "100px"
            txtStep1 = "   เลือกชั้น -->   "
            txtStep2 = "   เลือกวิชา -->   "
            txtStep3 = "   เลือกหน่วยฯ   "
            PaddingSize = "5px 10px 6px 10px"
        End If
    End Sub

    Private Function GetLevelDetail(ByVal Level As String, Optional ByRef InputConn As SqlConnection = Nothing) As Object
        Dim LevelDetail As Object = New With {.LevelId = "", .LevelName = ""}
        Level = Level.Substring(1, Level.Length - 1) 'ตัดตัว K ทิ้งเพื่อเอาเลขไป where ใน tblLevel
        If Level Is Nothing Or Level = "" Then
            Return Nothing
        End If
        Dim sql As String = " SELECT * FROM dbo.tblLevel WHERE Level = '" & Level & "'; "
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)

        If dt.Rows.Count > 0 Then
            LevelDetail.LevelId = dt.Rows(0)("Level_Id").ToString()
            LevelDetail.LevelName = dt.Rows(0)("Level_ShortName").ToString()
        End If
        Return LevelDetail
    End Function

    Private Sub CreateButtonClass()
        Dim TotalClass As String = HttpContext.Current.Application("EnabledClassesInPracticeMode").ToString() '& ",k13,k14,k15,k10,k11,k12,k456,k789,k101112,k131415" '
        Dim SplitStr = If((ClsKNSession.IsMaxONet), GetStudentClassMaxonet(), TotalClass.Split(",").ToArray)
        Dim MainPanel As New Panel
        With MainPanel
            .ID = "MainPanel"
            .ClientIDMode = UI.ClientIDMode.Static
            .Style.Add("margin-left", "auto")
            .Style.Add("margin-right", "auto")
            If IsAndroid = True Then
                .Style.Add("width", "1100px")
            Else
                .Style.Add("width", "810px")
            End If
            .Style.Add("margin-top", "65px")
            .Style.Add("padding-left", "50px")
            .Style.Add("padding-right", "50px")
            .Style.Add("padding-bottom", "50px")
            .Style.Add("background-color", "White")
            .Style.Add("border-radius", "10px")
            .Style.Add("text-align", "center")
        End With
        Dim SecondPanel As New Panel
        With SecondPanel
            .ID = "SecondPanel"
            .Style.Add("background-color", "#D3F2F7")
            .Style.Add("border-radius", "5px")
            .Style.Add("height", "auto")
        End With

        Dim Headlabel As New Label
        With Headlabel
            .Text = "เลือกชั้น"
            .ForeColor = Drawing.Color.Orange
        End With

        Headlabel.Style.Add("position", "relative")
        Headlabel.Style.Add("top", "10px")

        MainPanel.Controls.Add(Headlabel)

        Dim ClassId As String = ""

        Dim EachClass As String = ""
        Dim LevelId As String = ""
        Dim ClassName As String = ""

        Dim FirstPanelPic As New Panel
        Dim SecondPanelPic As New Panel
        Dim ThirdPanelPic As New Panel '' แถว 3 ช่วงชั้น
        Dim FirstPanelBtn As New Panel
        With FirstPanelBtn
            .Style.Add("border-top", "1px solid white")
            .Style.Add("border-bottom", "1px solid white")
        End With
        Dim SecondPanelBtn As New Panel
        With SecondPanelBtn
            .Style.Add("border-top", "1px solid white")
            .Style.Add("border-bottom", "1px solid white")
        End With
        Dim ThirdPanelBtn As New Panel  '' แถว 3 ช่วงชั้น
        With ThirdPanelBtn
            .Style.Add("border-top", "1px solid white")
            .Style.Add("border-bottom", "1px solid white")
        End With
        Dim CheckOverHalf As Integer = 1

        For i = 0 To SplitStr.Length - 1 'วน Loop ตามจำนวนชั้นที่ได้มาจาก Webconfig ** AppSetting("EnabledClassesInPracticeMode") ** เพื่อสร้างปุ่มชั้น

            'ทำปุ่ม
            EachClass = SplitStr(i).ToString()
            'LevelId = GetLevelIdByLevel(EachClass)
            Dim levelDetail As Object = GetLevelDetail(EachClass)
            LevelId = levelDetail.LevelId
            If LevelId = "" Then
                Exit Sub
            End If
            'ClassName = ChangeStrClass(EachClass.ToUpper())
            ClassName = levelDetail.LevelName
            If EachClass = "" Then
                Exit Sub
            End If

            Dim ClassButton As Button = NewClassNameButton(LevelId, ClassName)
            Dim ImgClass As ImageButton = NewImageClassButton(LevelId, EachClass, i)

            If i <= 5 Then
                FirstPanelBtn.Controls.Add(ClassButton)
                FirstPanelPic.Controls.Add(ImgClass)
            ElseIf i > 5 And i <= 11 Then
                SecondPanelBtn.Controls.Add(ClassButton)
                SecondPanelPic.Controls.Add(ImgClass)
            Else
                ThirdPanelBtn.Controls.Add(ClassButton)
                ThirdPanelPic.Controls.Add(ImgClass)
            End If

            CheckOverHalf += 1
        Next

        SecondPanel.Controls.Add(FirstPanelPic)
        SecondPanel.Controls.Add(FirstPanelBtn)
        If CheckOverHalf > 6 Then
            SecondPanel.Controls.Add(SecondPanelPic)
            SecondPanel.Controls.Add(SecondPanelBtn)
        End If
        If ThirdPanelBtn IsNot Nothing Then
            SecondPanel.Controls.Add(ThirdPanelPic)
            SecondPanel.Controls.Add(ThirdPanelBtn)
        End If

        Dim BackButton As New Button
        With BackButton
            .ID = "BackButton"
            .ClientIDMode = UI.ClientIDMode.Static
            .ToolTip = "ย้อนกลับ"
            .Style.Add("position", "relative")
            .Style.Add("width", "70px")
            .Style.Add("height", "70px")
        End With
        AddHandler BackButton.Click, AddressOf Me.BackButtonClick


        MainPanel.Controls.Add(SecondPanel)
        MainDiv.Controls.Add(MainPanel)
        'Page.Controls.Add(MainPanel)
        'Form.Controls.Add(MainPanel)

        ' สร้าง div เพื่อไว้ lock button ให้ติดซ้าย
        Dim panelBackBtn As New Panel
        panelBackBtn.CssClass = "divBackBtn"
        panelBackBtn.Controls.Add(BackButton)
        MainDiv.Controls.Add(panelBackBtn)
    End Sub

    ''' <summary>
    ''' function ในการสร้างปุ่มชื่อชั้น
    ''' </summary>
    ''' <param name="levelId"></param>
    ''' <param name="className"></param>
    ''' <returns>button ที่มีชื่อ classname</returns>
    Private Function NewClassNameButton(levelId As String, className As String) As Button
        Dim ClassButton As New Button
        With ClassButton
            If IsAndroid = True Then
                .Style.Add("width", "130px")
                .Style.Add("height", "60px")
                .Style.Add("margin", "15px 10px 15px")
                .Style.Add("line-height", "55px")
                .Style.Add("font-size", "30px")
            Else
                .Style.Add("width", "95px")
                .Style.Add("height", "40px")
                .Style.Add("margin", "10px 20px 10px")
                .Style.Add("line-height", "40px")
            End If
            .ID = levelId
            .Text = className
            .Style.Add("border-radius", "10px")
            .Style.Add("color", "white")
            .Style.Add("position", "relative")
            .CssClass = "Forbtn"
            .ToolTip = className
        End With
        AddHandler ClassButton.Click, AddressOf Me.ClassButtonClick
        Return ClassButton
    End Function

    Private Function NewImageClassButton(levelId As String, classAssumeName As String, order As Integer) As ImageButton
        Dim ImgClass As New ImageButton
        With ImgClass
            .ID = "Img" & levelId
            .ImageUrl = String.Format("~/Images/Activity/Class/{0}.png", classAssumeName)
            If IsAndroid = True Then
                .Style.Add("width", "90px")
                .Style.Add("height", "90px;")
            Else
                .Style.Add("width", "70px")
                .Style.Add("height", "70px;")
            End If

            If order > 0 And order <> 6 Then
                If IsAndroid = True Then
                    .Style.Add("margin-left", "60px")
                Else
                    .Style.Add("margin-left", "65px")
                End If
            End If
            .Style.Add("margin-top", "15px")
            .CssClass = "ImgbtnCls"
        End With
        AddHandler ImgClass.Click, AddressOf Me.ClassImgBtnClick
        Return ImgClass
    End Function
    Protected Sub ClassButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim newBtn As New Button
        newBtn = sender
        Session("PClassId") = newBtn.ID
        If ClsKNSession.IsMaxONet Then
            Response.Redirect("ChooseSubject.aspx?deviceuniqueid=" & DVID & "&token=" & TokenId & "&DashboardMode=" & Request.QueryString("DashboardMode") & "&RedirectMode=" & Request.QueryString("RedirectMode"))
        Else
            Response.Redirect("ChooseSubject.aspx")
        End If
    End Sub

    Protected Sub ClassImgBtnClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim newBtn As New ImageButton
        newBtn = sender
        Dim SpliteStrId As String = newBtn.ID.Replace("Img", "")
        Session("PClassId") = SpliteStrId
        If ClsKNSession.IsMaxONet Then
            Response.Redirect("ChooseSubject.aspx?deviceuniqueid=" & DVID & "&token=" & TokenId & "&DashboardMode=" & Request.QueryString("DashboardMode") & "&RedirectMode=" & Request.QueryString("RedirectMode"))
        Else
            Response.Redirect("ChooseSubject.aspx")
        End If
    End Sub

    Private Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click
        CheckAndRedirectBeforePage()
    End Sub

    Protected Sub BackButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        CheckAndRedirectBeforePage()
    End Sub

    Private Sub CheckAndRedirectBeforePage()
        If ClsKNSession.IsMaxONet Then
            Response.Redirect("ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId)
        Else
            If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                Response.Redirect("~/PracticeMode_Pad/MainPractice.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Quiz Then
                Response.Redirect("~/Quiz/DashboardQuizPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework Then
                Response.Redirect("~/Homework/DashboardHomeworkPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice Then
                Response.Redirect("~/Practice/DashboardPracticePage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PrintTestset Then
                Response.Redirect("~/PrintTestset/DashboardPrintTestsetPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.SetUp Then
                Response.Redirect("~/Testset/DashboardSetupPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = 9 Then
                Response.Redirect("~/PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId").ToString())
            End If
        End If
    End Sub


    Private Function GetStudentClassMaxonet() As Array
        Dim studentId As String
        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginMaxOnetPage.aspx")
        Else
            studentId = Session("UserId").ToString()
        End If

        Dim sql As String = ""
        sql = "Select (ss.SS_LevelId) FROM maxonet_tblStudentSubject ss INNER JOIN tblLevel l On ss.SS_LevelId = l.Level_Id
                inner join maxonet_tblKeyCodeUsage kcu on ss.SS_KeyCode = kcu.KeyCode_Code 
                WHERE SS_StudentId = '" & studentId & "' and (KCU.KCU_ExpireDate > dbo.GetThaiDate() or kcu.KCU_ExpireDate is null)
                GROUP BY (ss.SS_LevelId), l.Level ORDER BY l.Level;"

        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)

        Dim tempSubjecClass As New List(Of String)

        For Each r In dt.Rows
            tempSubjecClass.Add(r("SS_LevelId").ToString().ToClassKFormat)
        Next

        Return tempSubjecClass.ToArray()
    End Function
End Class
