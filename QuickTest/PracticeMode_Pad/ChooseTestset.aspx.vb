Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports System.Web

Public Class ChooseTestset
    Inherits System.Web.UI.Page

    Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
    Dim Player_Id As String
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim ClsHomework As New ClsHomework(New ClassConnectSql)
    Public GroupName As String
    Public DVID As String
    'TotalNotExited & ":" & TotalNotChecked
    Public TotalNotExited As String
    Public TotalNotChecked As String
    Public txtWithOutHaveHomework As String
    Public txtColum As String

    Dim KnSession As New KNAppSession()

    Dim UseCls As New ClassConnectSql
    Protected IsAndroid As Boolean

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected IsPostback As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If DEBUG Then
        'Session("PracticeMode") = True
        'Session("PDeviceId") = "129"
        'Session("PDeviceId") = "RealTest16"
#End If
        'If Session("selectedSession") Is Nothing Then
        '    Session("selectedSession") = "ม.1/2"
        'End If

        'Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        'Session("SchoolID") = "1000001"

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

        If Not Page.IsPostBack Then
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If
            IsPostback = "False"
        Else
            IsPostback = "True"
        End If

        HttpContext.Current.Session("ChooseMode") = Nothing

        Session("PDeviceId") = Request.QueryString("DeviceUniqueID")
        Session("TabletUser") = True ' ไม่ว่าจะเป็นเครื่องเด็กหรือเครื่องสำรองห้องแล็บ จะมีปุ่ม Logout ไม่ได้
        Dim IsTabletOwner As String

        'Log.Record(Log.LogType.PageLoad, "1", True)

        If HttpContext.Current.Application("Sess" & Session("PDeviceId")) IsNot Nothing Then
            HttpContext.Current.Application("Sess" & Session("PDeviceId")) = Nothing
            'Log.Record(Log.LogType.PageLoad, "2", True)
        End If

        'Open Connection

        Dim connActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(connActivity)

        'Log.Record(Log.LogType.PageLoad, "3", True)

        'Check ก่อนว่าเครื่องที่เข้ามาคือเครื่องแบบไหน -- เครื่องส่วนตัวเด็ก เครื่องสำรอง เครื่องในห้องแล็บ

        IsTabletOwner = ClsPracticeMode.CheckSpareTablet(Session("PDeviceId"), connActivity)

        'Log.Record(Log.LogType.PageLoad, "4", True)

        'เช็คว่าเป็นเครื่องสำรองก่อนป่าว
        If IsTabletOwner = "True" Then

            Dim dtPlayer As DataTable

            dtPlayer = ClsPracticeMode.GetPlayerDetail(Session("PDeviceId"), connActivity)

            'Log.Record(Log.LogType.PageLoad, "5", True)
            If dtPlayer Is Nothing Then
                'ถ้าไม่เจอ tablet ให้ return ไปหน้าติดต่อศูนย์คอม กรณีนี้อาจเกิดจากการคืนเครื่อง
                Response.Redirect("~/DroidPad/NotUsage.aspx")
            End If


            If dtPlayer.Rows.Count > 0 Then

                Dim SchoolCode As String = dtPlayer.Rows(0)("School_code").ToString
                Player_Id = dtPlayer.Rows(0)("Student_Id").ToString
                Dim ClassName As String = dtPlayer.Rows(0)("ClassName").ToString
                Dim RoomName As String = dtPlayer.Rows(0)("RoomName").ToString
                Session("SchoolCode") = SchoolCode
                Session("SchoolID") = SchoolCode
                'Log.Record(Log.LogType.PageLoad, "5.1", True)
                'Session("selectedSession") = ClassName & RoomName
                Session("selectedSession") = dtPlayer.Rows(0)("Room_Id").ToString()
                Session("UserId") = Player_Id

                SetCalendarId()


                'Log.Record(Log.LogType.PageLoad, "5.2", True)

                Log.Record(Log.LogType.StudentOpenTabletApp, "เปิดแท็บเล็ต (นักเรียน)", True)

                SetRepeaterHomework(dtPlayer, connActivity)

                HdChkSpareTablet.Value = False

                redis.SetKey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString()), dtPlayer.Rows(0)("IsViewAllTips"))

                If Not Page.IsPostBack Then
                    ' ส่วนของการแสดง qtip
                    If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                        Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                        Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                        NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
                    End If
                End If
                'NeedShowTip = True          

            End If

        Else
            'ไม่ใช่เครื่องเด็ก เช็คว่าเป็นเครื่องสำรองหรือเครื่องในห้องแล็บ

            Dim dtLabTablet As DataTable = ClsPracticeMode.CheckLabTablet(Session("PDeviceId"), connActivity)

            Dim TabId As DataTable = ClsPracticeMode.GetTabId(Session("PDeviceId"), connActivity)

            If Not dtLabTablet.Rows.Count = 0 Then
                Session("SchoolCode") = dtLabTablet.Rows(0)("School_Code").ToString
                Session("SchoolID") = dtLabTablet.Rows(0)("School_Code").ToString
                Session("selectedSession") = dtLabTablet.Rows(0)("TabletLab_Id").ToString
                HttpContext.Current.Session("UserId") = TabId.Rows(0)("Tablet_Id").ToString
                Log.Record(Log.LogType.StudentOpenTabletApp, "เปิดแท็บเล็ต (เครื่องห้องแล็บ)", True)
            Else
                Session("SchoolCode") = HttpContext.Current.Application("DefaultSchoolCode")
                Session("SchoolID") = HttpContext.Current.Application("DefaultSchoolCode")
                Session("selectedSession") = "Spare"
                HttpContext.Current.Session("UserId") = TabId.Rows(0)("Tablet_Id").ToString
                Log.Record(Log.LogType.StudentOpenTabletApp, "เปิดแท็บเล็ต (เครื่องสำรอง)", True)
            End If

            GroupName = Session("selectedSession")
            Session("PracticeFromComputer") = False
            DVID = Session("PDeviceId")
            HdChkSpareTablet.Value = True
            SetCalendarId()

        End If

        'Log.Record(Log.LogType.PageLoad, "6", True)

        If Session("PracticeFromComputer") = False And IsTabletOwner = "True" Then 'ถ้าไม่ได้เป็นฝึกฝนผ่านคอมพิวเตอร์ต้องหา GroupName เพื่อ Add ให้ SignalR
            Session("PracticeFromComputer") = False
            DVID = Session("PDeviceId")
            GroupName = Session("selectedSession")
            'GroupName = ClsActivity.GetGroupNameByDVID(DVID)
            'Log.Record(Log.LogType.PageLoad, "7", True)
        End If

        If HttpContext.Current.Session("ShowFullPractice") Is Nothing OrElse HttpContext.Current.Session("ShowFullPractice").ToString() = "" Then
            HttpContext.Current.Session("ShowFullPractice") = "False"
            'Log.Record(Log.LogType.PageLoad, "8", True)
        End If

        'Close Connection
        UseCls.CloseExclusiveConnect(connActivity)
        'Log.Record(Log.LogType.PageLoad, "9", True)
    End Sub

    Private Sub SetRepeaterHomework(dtPlayer As DataTable, Optional ByRef InputConn As SqlConnection = Nothing)
        'Bind Repeater การบ้าน
        'Dim CheckHdVariable As String = ""
        'If Page.IsPostBack Then
        '    If Request.Form("TestGetValue") IsNot Nothing Then
        '        CheckHdVariable = Request.Form("TestGetValue").ToString()
        '        'Log.Record(Log.LogType.PageLoad, "5.3", True)
        '    End If
        'End If

        Dim StrScript As String = ""

        'Log.Record(Log.LogType.PageLoad, "5.4", True)

        'Dim KnSession As New KNAppSession()
        Dim Calendar_ID As String = KnSession.StoredValue("CurrentCalendarId").ToString

        'Log.Record(Log.LogType.PageLoad, "5.5", True)
        Dim DataAmount As String = ClsHomework.GetDataAmount(Player_Id, Calendar_ID, InputConn)
        'TotalNotExited & ":" & TotalNotChecked
        Dim ArrDataAmount() As String = Split(DataAmount, ":")
        TotalNotExited = ArrDataAmount(0)
        TotalNotChecked = ArrDataAmount(1)

        'Log.Record(Log.LogType.PageLoad, "5.6", True)
        If chkShowCompleteTS.Text = "การบ้าน" Or chkShowCompleteTS.Text = "ดูการบ้านปัจจุบัน" Then
            BindRepeaterHomeWork(dtPlayer.Rows(0)("Student_Id").ToString(), False, InputConn)
            'StrScript = "<script type='text/javascript'>$('#chkShowCompleteTS').removeAttr('checked');</script>"
            chkShowCompleteTS.Text = "ดูประวัติการบ้าน"
            txtColum = "สถานะ"
            'Log.Record(Log.LogType.PageLoad, "5.8", True)
        Else
            BindRepeaterHomeWork(dtPlayer.Rows(0)("Student_Id").ToString(), True, InputConn)
            'StrScript = "<script type='text/javascript'>$('#chkShowCompleteTS').attr('checked', 'checked');$('#divHomeWork').hide();</script>"
            'StrScript = "<script type='text/javascript'>$('#chkShowCompleteTS').attr('checked', 'checked');</script>"
            chkShowCompleteTS.Text = "ดูการบ้านปัจจุบัน"
            txtColum = "คะแนน"
            'Log.Record(Log.LogType.PageLoad, "5.7", True)
        End If
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)

        'Log.Record(Log.LogType.PageLoad, "5.9", True)
        Dim dtTestset As DataTable = ClsPracticeMode.GetTestsetFromClass(Player_Id, HttpContext.Current.Session("ShowFullPractice"), InputConn)
        ' Log.Record(Log.LogType.PageLoad, "5.10", True)
        If dtTestset.Rows.Count = 0 Then
            Listing.Visible = False
            'Log.Record(Log.LogType.PageLoad, "5.11", True)
        Else
            Listing.Visible = True
            Listing.DataSource = dtTestset
            Listing.DataBind()
            'Log.Record(Log.LogType.PageLoad, "5.12", True)
        End If
        'Log.Record(Log.LogType.PageLoad, "5.13", True)

    End Sub

    Private Sub SetCalendarId(Optional ByRef InputConn As SqlConnection = Nothing)
        'Session CalendarID,CalendarName (Cuurent,Selected)
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
    'get calendar from date now 
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

    Private Sub BindRepeaterHomeWork(ByVal PlayerId As String, ByVal DisplayIsComplete As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim KnSession As New KNAppSession()
        Dim Calendar_ID As String = KnSession.StoredValue("CurrentCalendarId").ToString

        Dim dtOld As DataTable = ClsHomework.GetHomeWorkByStudentId(PlayerId, True, Calendar_ID, InputConn)
        Dim dtNew As DataTable = ClsHomework.GetHomeWorkByStudentId(PlayerId, False, Calendar_ID, InputConn)

        'ถ้าไม่เคยมีการบ้านทั้งเก่าทั้งใหม่
        'divhomework.Visible = False
        If DisplayIsComplete = True And dtOld.Rows.Count = 0 Then
            'ถ้าดูการบ้านเก่า แล้วไม่มี
            RptHomeWork.Visible = False
            divWithoutHomework.Visible = True
            txtWithOutHaveHomework = "ไม่มีประวัติการบ้านค่ะ"
            'divHomeWork.InnerHtml = "<div style='height:100px;background-color:white;overflow-y:initial;border:1px dashed black;color:black;line-height: 5em;font-weight: bold;'>ไม่มีประวัติการบ้านค่ะ</div>"
            'divHomeWork.Style.Add("height", "auto")
        ElseIf DisplayIsComplete = True And dtOld.Rows.Count <> 0 Then
            'ถ้าดูการบ้านเก่าแล้วมี
            divWithoutHomework.Visible = False
            RptHomeWork.Visible = True
            RptHomeWork.DataSource = dtOld
            RptHomeWork.DataBind()
        ElseIf DisplayIsComplete = False And dtNew.Rows.Count = 0 Then
            'ถ้าดูการบ้านใหม่แล้วไม่มี
            RptHomeWork.Visible = False
            divWithoutHomework.Visible = True
            txtWithOutHaveHomework = "ไม่มีการบ้านค้างค่ะ"
            'divHomeWork.InnerHtml = "<div style='height:100px;background-color:white;overflow-y:initial;border:1px dashed black;color:black;line-height: 5em;font-weight: bold;'>ไม่มี</div>"
            'divHomeWork.Style.Add("height", "auto")

        ElseIf DisplayIsComplete = False And dtNew.Rows.Count <> 0 Then
            'ถ้าดูการบ้านใหม่แล้วมี
            divWithoutHomework.Visible = False
            RptHomeWork.Visible = True
            RptHomeWork.DataSource = dtNew
            RptHomeWork.DataBind()
            ClsHomework.SetIsChecked(PlayerId)
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function SaveQuiz(ByVal ItemId As String, ByVal IsHomework As String, ByVal Status As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim ClsHomework As New ClsHomework(New ClassConnectSql)
        Dim SClsPractice As New Service.ClsPracticeMode(New ClassConnectSql)
        ' หาข้อมูลเด็ก

        Dim dtPlayer As DataTable = ClsPracticeMode.GetPlayerDetail(HttpContext.Current.Session("PDeviceId"))

        Dim ClassName As String = dtPlayer.Rows(0)("ClassName").ToString
        Dim RoomName As String = dtPlayer.Rows(0)("RoomName").ToString
        Dim SchoolCode As String = dtPlayer.Rows(0)("School_Code").ToString
        Dim player_Id = dtPlayer.Rows(0)("Student_Id").ToString
        HttpContext.Current.Session("UserId") = player_Id

        'ถ้าเป็นการบ้าน ให้ดูสถานะว่า Quiz นี้ User นี้มี Quizsession ยัง ถ้าไม่มีให้ insert ถ้ามีไม่ต้องทำไร ส่ง Quiz_Id กับ Type ไปให้ ActivityPagePad Choosemode เป็น Homework
        'ต้องดูด้วยว่าสถานะของการบ้านคืออะไรถ้า เลย deadline ก็ต้องเป็น เฉลย

        'ถ้าเป็นฝึกฝน ให้ส่ง testset_Id ไปให้ หน้า ActivityPagePad เป็นตัว insert ChooseMode เป็น Practice

        If IsHomework = "1" Then
            HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework

            If Not ClsHomework.HaveSession(player_Id, ItemId) Then
                If Status = "0" Then
                    ClsHomework.InSertSessionHomeworkForUser(ItemId, player_Id, SchoolCode, HttpContext.Current.Session("PDeviceId"))
                Else
                    ClsHomework.SetQuizSession(ItemId, player_Id, SchoolCode, HttpContext.Current.Session("PDeviceId"), False)
                End If
            End If
            'ส่ง DeviceId , Status , QuizId
            Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId") & "&Status=" & Status & "&ItemId=" & ItemId


        Else
            HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice
            Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId") & "&ItemId=" & ItemId
        End If


        'Dim QuizIdState As String = ""

        'If IsHomework = "1" Then

        '    QuizIdState = ClsPracticeMode.SaveQuizDetail(ItemId, ClassName, RoomName, SchoolCode, player_Id, "1", True)
        '    HttpContext.Current.Session("HomeworkMode") = True
        'ElseIf IsHomework = "0" Then
        '    QuizIdState = ClsPracticeMode.SaveQuizDetail(ItemId, ClassName, RoomName, SchoolCode, player_Id, "1", False)
        '    HttpContext.Current.Session("HomeworkMode") = False
        'End If

        'Dim ArrQuizIdState() As String = Split(QuizIdState, ":")
        'HttpContext.Current.Session("Quiz_Id") = ArrQuizIdState(0)
        'Dim IsHomeworkState As String = ArrQuizIdState(1).ToString

        'If IsHomeworkState <> "" Then
        '    If IsHomeworkState = False Then
        '        ClsActivity.getQsetInQuiz(ItemId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion
        '        ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, False, ClsPracticeMode.GetFirstQsetTypeFromQuizId(HttpContext.Current.Session("Quiz_Id")))
        '        ClsPracticeMode.SaveQuiznswer(HttpContext.Current.Session("Quiz_Id"), HttpContext.Current.Session("UserId"), SchoolCode, HttpContext.Current.Session("PDeviceId"), False)
        '    End If
        'End If


        ''KnSession.AddValueForClsSess(HttpContext.Current.Session("Quiz_Id"), "SelfPace", True)

        'Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")

    End Function

    <Services.WebMethod()>
    Public Shared Function GetTestset(ByVal IsShowFullPractice As Boolean) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
        Dim dtPlayer As DataTable = ClsPracticeMode.GetPlayerDetail(HttpContext.Current.Session("PDeviceId"))
        Dim Player_Id As String = dtPlayer.Rows(0)("Student_Id").ToString
        Dim dtTestset As DataTable = ClsPracticeMode.GetTestsetFromClass(Player_Id, IsShowFullPractice)
        If dtTestset.Rows.Count = 0 Then
            Return "hidden"
        Else
            Return "visible"
        End If
    End Function

    Private Sub Listing_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles Listing.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.DataItem("Testset_Name") = "ดูเพิ่มเติม" Then
                Dim EditTd As HtmlControls.HtmlTableCell = e.Item.FindControl("tdItem")
                EditTd.Style.Add("text-align", "center")
                EditTd.Attributes.Add("class", "SeeFullPractice")
            End If
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function SetValueToSessionShowFullPractice() As Boolean
        HttpContext.Current.Session("ShowFullPractice") = "True"
        Return True
    End Function

    <Services.WebMethod()>
    Public Shared Function GetAvatarPic(Item_Id As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim js As New JavaScriptSerializer()
        Dim ClsPractice As New ClsPracticeMode(New ClassConnectSql)

        Dim StdWinner As String = ClsPractice.GetTestsetWinnerFromMode(Item_Id, 2)
        Dim imgWinner As String = ""
        Dim IsShowImgWinner As String
        If StdWinner <> "" Then
            IsShowImgWinner = True
            'บรรทัด 299 ไม่ใช่ StdWinner หรอ? รอถามไหมอีกที
            If System.IO.File.Exists(HttpContext.Current.Server.MapPath("../UserData/" & HttpContext.Current.Session("SchoolCode").ToString() & "/{" & HttpContext.Current.Session("UserId") & "}/avt.png")) Then
                imgWinner = "../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & HttpContext.Current.Session("UserId") & "}/avt.png"
            Else
                imgWinner = "../UserData/dummy.png"
            End If
        Else
            IsShowImgWinner = False
            imgWinner = ""
        End If

        Dim JsonString
        JsonString = New With {.IsShowImgWinner = IsShowImgWinner, .imgWinner = imgWinner, .StdWinnerId = StdWinner}
        Return js.Serialize(JsonString)

    End Function

End Class
