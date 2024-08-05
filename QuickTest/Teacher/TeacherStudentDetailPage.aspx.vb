Imports BusinessTablet360
Imports System.Data.SqlClient
Imports System.IO

Public Class TeacherStudentDetailPage

    Inherits System.Web.UI.Page
    Dim ClsStudent As New Service.ClsStudent(New ClassConnectSql())
    Dim _DB As New ClassConnectSql()
    Dim KnSession As New KNAppSession()
    Public CheckIsHaveInfo As Boolean
    Protected IsAndroid As Boolean
    Dim ClsUser As New ClsUser(New ClassConnectSql())

    Public Property StudentId As String
        Get
            StudentId = ViewState("_StudentId")
        End Get
        Set(ByVal value As String)
            ViewState("_StudentId") = value
        End Set
    End Property

    Public Property TeacherId As String
        Get
            TeacherId = ViewState("_TeacherId")
        End Get
        Set(ByVal value As String)
            ViewState("_TeacherId") = value
        End Set
    End Property

    Public Property SelectedCalendarId As String
        Get
            SelectedCalendarId = ViewState("_SelectedCalendarId")
        End Get
        Set(ByVal value As String)
            ViewState("_SelectedCalendarId") = value
        End Set
    End Property

    Dim UseCls As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("SchoolID") = "1000001"
        Session("SchoolCode") = "1000001"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
        knSession("SelectedCalendarName") = "เทอม 2 / 2556"
        'StudentId = "6902a943-75fe-48e1-8263-7610074019f6"
        StudentId = "cfe47508-52b3-46a0-984b-4e2cc54a2bdd"
#End If
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/Loginpage.aspx")
        End If

        'หา CalendarId ถ้าไม่มี CalendarId ให้กลับไปหน้า LoginPage
        If KnSession("SelectedCalendarId") IsNot Nothing Then
            SelectedCalendarId = KnSession("SelectedCalendarId").ToString()
        Else
            Response.Redirect("~/Loginpage.aspx")
        End If

        Dim conn As New SqlConnection
        If Not Page.IsPostBack Then
            
            If Request.QueryString("StudentId") IsNot Nothing And Request.QueryString("StudentId") <> "" Then
                StudentId = Request.QueryString("StudentId").ToString()
            End If

            If Session("UserId") IsNot Nothing And Session("UserId").ToString() <> "" Then
                TeacherId = Session("UserId").ToString()
            End If

            UseCls.OpenExclusiveConnect(conn)

            CreateDivStudentInfo(StudentId, conn)
            CreateAccordionStudent(StudentId, SelectedCalendarId, conn)
            
            UseCls.CloseExclusiveConnect(conn)
        End If

        RenderHistoryByMode()

    End Sub


    Private Sub CreateDivStudentInfo(ByVal StudentId As String, ByRef InputConn As SqlConnection)

        Dim dt As DataTable = ClsStudent.GetDtStudentDetailByCalendarAndStudentId(StudentId, SelectedCalendarId, InputConn)
        Dim sb As New StringBuilder

        Dim StudentClassRoom As String = ""
        Dim StudentCode As String = ""
        Dim StudentFather As String = ""
        Dim StudentMother As String = ""
        Dim StudentPhone As String = ""
        Dim StudentNumber As String = ""

        If dt.Rows.Count > 0 Then

            Dim StudentName As String = dt.Rows(0)("Student_FirstName")
            If dt.Rows(0)("Student_NickName") IsNot Nothing Then
                StudentName &= " (" & dt.Rows(0)("Student_NickName") & ")"
            End If
            StudentName &= " " & dt.Rows(0)("Student_LastName")

            If dt.Rows(0)("Class_Name") IsNot DBNull.Value Then
                StudentClassRoom = dt.Rows(0)("Class_Name") & dt.Rows(0)("Room_Name")
            End If

            If dt.Rows(0)("Student_CurrentNoInRoom") IsNot DBNull.Value Then
                StudentNumber = " เลขที่ " & dt.Rows(0)("Student_CurrentNoInRoom")
            End If

            If dt.Rows(0)("Student_Code") IsNot DBNull.Value Then
                StudentCode = " รหัส : " & dt.Rows(0)("Student_Code")
            End If

            If dt.Rows(0)("Student_FatherName") IsNot DBNull.Value And dt.Rows(0)("Student_FatherPhone") IsNot DBNull.Value Then
                StudentFather = "บิดา : " & dt.Rows(0)("Student_FatherName") & " " & dt.Rows(0)("Student_FatherPhone") & " , "
            End If

            If dt.Rows(0)("Student_MotherName") IsNot DBNull.Value And dt.Rows(0)("Student_MotherPhone") IsNot DBNull.Value Then
                StudentMother = "มารดา : " & dt.Rows(0)("Student_MotherName") & " " & dt.Rows(0)("Student_MotherPhone")
            End If

            If dt.Rows(0)("Student_Phone") IsNot DBNull.Value Then
                StudentPhone = "เบอร์ : " & dt.Rows(0)("Student_Phone") & " , "
            End If

            'Dim IsFavorite As Boolean = ClsStudent.CheckStudentIsFavoriteByTeacherIdAndStudentId(Session("UserId").ToString(), StudentId, InputConn)
            sb.Append("<div class='DivForImg' >")


            'Dim PathImg As String = "../UserData/" & Session("SchoolID").ToString() & "/{" & StudentId & "}/Id.jpg"
            '' check ว่ามีไฟล์รูปอยู่หรือเปล่า
            'If File.Exists(HttpContext.Current.Server.MapPath("/quicktest_test/" & PathImg.Substring(3))) = False Then
            '    PathImg = "../UserData/IDdummy.png"
            'End If

            Dim UserData As String

            Dim PhotoStatus As Boolean
            PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)

            If PhotoStatus Then

                UserData = "../UserData/" & Session("SchoolID").ToString() & "/{" & StudentId & "}/Id.jpg"

            Else

                UserData = "MonsterID.axd?seed=" & StudentId & "&size=179"

            End If



            sb.Append("<img src='" & UserData & "' style='width: 100%;height:100%;border-radius:5px 15px 5px 5px;' />")

            'If IsFavorite = True Then
            '    sb.Append("<img src='../Images/dashboard/student/Unfavorite.png' StId='" & StudentId & "' class='imgStar ForYellowStar' />")
            'Else
            '    sb.Append("<img src='../Images/dashboard/student/Favorite.png' StId='" & StudentId & "' class='imgStar ForGrayStar' />")
            'End If

            sb.Append("<div class='favoriteStudent'  studentid='" & StudentId & "'>")

            ' สร้าง icon ตาม code ที่ favorite ไว้
            Dim dtStudentFavorite As DataTable = ClsStudent.getStudentFavoriteCode(Session("UserId").ToString(), StudentId, InputConn)
            sb.Append(FavoriteHelper.getImgStudentFavorite(dtStudentFavorite))

            sb.Append("</div>")

            sb.Append("</div>")
            sb.Append("<div id='DivStudentInfo' class='DivStudentInfo' >")
            'sb.Append("<table>")
            'sb.Append("<tr><td>")
            'sb.Append(StudentName)
            'sb.Append("</td><td>")
            'sb.Append(StudentFather)
            'sb.Append("</td></tr><tr><td>")
            'sb.Append(StudentClassRoom)
            'sb.Append("</td><td>")
            'sb.Append(StudentMother)
            'sb.Append("</td></tr><tr><td>")
            'sb.Append(StudentCode)
            'sb.Append("</td><td>")
            'sb.Append(StudentPhone)
            sb.Append("<table>")
            sb.Append("<tr><td id='tdStudentName'>")
            sb.Append(StudentName)
            sb.Append("</td><td id='tdStudentRoomDetail' style='padding-top: 6px;'>")
            sb.Append(StudentClassRoom)
            sb.Append(StudentNumber)
            sb.Append(StudentCode)
            sb.Append("</td></tr><tr><td id='tdStudentDetail' colspan='2'>")
            sb.Append(StudentPhone)
            sb.Append(StudentFather)
            sb.Append(StudentMother)
            sb.Append(" </td></tr></table></div>")
            DivStudentDetail.InnerHtml = sb.ToString()
        End If

    End Sub

    Private Sub CreateAccordionStudent(ByVal StudentId As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection)

        'หาห้องกับชั้นทั้งหมดว่ามีหรือเปล่า
        Dim dt As DataTable = ClsStudent.GetStudentClassNameByStudentAndCalendarId(StudentId, CalendarId, InputConn)

        If dt.Rows.Count > 0 Then 'ถ้านักเรียนคนนี้มีตัวตนแล้วในปีที่เลือก
            CheckIsHaveInfo = True
            Dim sb As New StringBuilder
            'วน Loop เผื่อปีที่เลือกเด็กคนนี้อยู่มากกว่า 1 ห้องก็ต้องเอาทุกห้องขึ้นมาแสดงใน Accordion
            For index = 0 To dt.Rows.Count - 1

                sb.Append("<h3>")
                sb.Append(dt.Rows(index)("Class_Name") & dt.Rows(index)("Room_Name"))
                sb.Append("</h3><div>")

                Dim ClassName As String = dt.Rows(index)("Class_Name")
                Dim RoomName As String = dt.Rows(index)("Room_Name")
                Dim dtStudent As DataTable = ClsStudent.GetAllStudentByClassRoomAndTeacherId(ClassName, RoomName, CalendarId, InputConn)
                If dtStudent.Rows.Count > 0 Then
                    For a = 0 To dtStudent.Rows.Count - 1
                        'Dim ImgPath As String = "../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & dtStudent.Rows(a)("Student_Id").ToString() & "}/id-small.jpg" 'รูปเล็ก
                        'Dim ImgPath As String = "../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & dtStudent.Rows(a)("Student_Id").ToString() & "}/id.jpg"

                        '' check ว่ามีไฟล์รูปอยู่หรือเปล่า
                        'If File.Exists(HttpContext.Current.Server.MapPath("/quicktest_test/" & ImgPath.Substring(3))) = False Then
                        '    ImgPath = "../UserData/IDdummy.png"
                        'End If

                        Dim UserData As String
                        Dim PhotoStatus As Boolean

                        PhotoStatus = ClsUser.GetStudentHasPhoto(dtStudent.Rows(a)("Student_Id").ToString())

                        If PhotoStatus Then

                            UserData = "../UserData/" & Session("SchoolID").ToString() & "/{" & dtStudent.Rows(a)("Student_Id").ToString() & "}/Id.jpg"

                        Else

                            UserData = "MonsterID.axd?seed=" & dtStudent.Rows(a)("Student_Id").ToString() & "&size=179"

                        End If

                        'sb.Append("<div id='" & dtStudent.Rows(a)("Student_Id").ToString() & "' onclick=""ChooseStudentAccordion('" & dtStudent.Rows(a)("Student_Id").ToString() & "','" & TeacherId & "')""  class='itmAcd' >")
                        sb.Append("<div id='" & dtStudent.Rows(a)("Student_Id").ToString() & "' stdId='" & dtStudent.Rows(a)("Student_Id").ToString() & "' tcId='" & TeacherId & "'  class='itmAcd' >")
                        sb.Append(dtStudent.Rows(a)("Student_FirstName") & " " & dtStudent.Rows(a)("Student_LastName"))
                        sb.Append("<img src='" & UserData & "' stdId='" & dtStudent.Rows(a)("Student_Id").ToString() & "' class='ImgSmallPic' />")
                        sb.Append("</div>")
                    Next
                    sb.Append("</div>")
                End If
            Next
            ac.InnerHtml = sb.ToString()
        Else 'ถ้านักเรียนคนนี้ยังไม่มีตัวตนในปีที่เลือก
            btnToggle.Enabled = False
            CheckIsHaveInfo = False
        End If

    End Sub

    Private Sub RenderHistoryByMode()

        If Session("TimeMode") Is Nothing Then
            Session("TimeMode") = 0
        End If

        If Session("SelectedMode") Is Nothing Then
            If ClsKNSession.RunMode = "" Then
                Session("SelectedMode") = "Homework"
                ProcessModeHomework()
            Else
                If ClsKNSession.RunMode = "labonly" Then
                    Session("SelectedMode") = "QuizHistory"
                    ProcessModeQuizHistory()
                Else
                    Session("SelectedMode") = "Homework"
                    ProcessModeHomework()
                End If
            End If
            'ElseIf Session("SelectedMode") = "Compare" Then
            '    ProcessModeCompare()
        ElseIf Session("SelectedMode") = "Homework" Then
            ProcessModeHomework()
        ElseIf Session("SelectedMode") = "QuizHistory" Then
            ProcessModeQuizHistory()
        ElseIf Session("SelectedMode") = "PracticeHistory" Then
            ProcessModePracticeHistory()
        ElseIf Session("SelectedMode") = "Log" Then
            ProcessModeLog()
        End If

        SetSelectedButton()

    End Sub

    'ปุ่มครูทั้งหมด กับของตัวเอง
    Private Sub btnToggle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnToggle.Click

        If TeacherId = "" Then
            TeacherId = HttpContext.Current.Session("UserId").ToString()
            btnToggle.Text = "ข้อมูลเฉพาะที่สอน"
        Else
            TeacherId = ""
            btnToggle.Text = "ข้อมูลทั้งโรงเรียน"
        End If

        'Open Connection
        Dim conn As New SqlConnection
        UseCls.OpenExclusiveConnect(conn)

        'เช็คว่าก่อนที่จะกดปุ่มสลับโหมด มันเป็นโหมดไหนอยู่ให้กลับไปโหมดเดิม
        If Session("SelectedMode") = "Compare" Then
            ProcessModeCompare(conn)
        ElseIf Session("SelectedMode") = "Homework" Then
            ProcessModeHomework(conn)
        ElseIf Session("SelectedMode") = "QuizHistory" Then
            ProcessModeQuizHistory(conn)
        ElseIf Session("SelectedMode") = "PracticeHistory" Then
            ProcessModePracticeHistory(conn)
        ElseIf Session("SelectedMode") = "Log" Then
            ProcessModeLog(conn)
        End If
        CreateDivStudentInfo(StudentId, conn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, conn)

        'Close Connection
        UseCls.CloseExclusiveConnect(conn)

    End Sub

    'Set สีปุ่มที่กดให้รู้ว่าตอนนี้เลือก Filter ใดบ้าง
    Private Sub SetSelectedButton()
        If Session("TimeMode") = 0 Then
            btnNow.ForeColor = Drawing.Color.Yellow
            btnSevenDay.ForeColor = Drawing.Color.White
            btnFifteenDay.ForeColor = Drawing.Color.White
            btnMonth.ForeColor = Drawing.Color.White
            btnTerm.ForeColor = Drawing.Color.White

            btnNow.BorderColor = Drawing.Color.White
        ElseIf Session("TimeMode") = 6 Then
            btnNow.ForeColor = Drawing.Color.White
            btnSevenDay.ForeColor = Drawing.Color.Yellow
            btnFifteenDay.ForeColor = Drawing.Color.White
            btnMonth.ForeColor = Drawing.Color.White
            btnTerm.ForeColor = Drawing.Color.White

            btnSevenDay.BorderColor = Drawing.Color.White
        ElseIf Session("TimeMode") = 14 Then
            btnNow.ForeColor = Drawing.Color.White
            btnSevenDay.ForeColor = Drawing.Color.White
            btnFifteenDay.ForeColor = Drawing.Color.Yellow
            btnMonth.ForeColor = Drawing.Color.White
            btnTerm.ForeColor = Drawing.Color.White

            btnFifteenDay.BorderColor = Drawing.Color.White
        ElseIf Session("TimeMode") = 29 Then
            btnNow.ForeColor = Drawing.Color.White
            btnSevenDay.ForeColor = Drawing.Color.White
            btnFifteenDay.ForeColor = Drawing.Color.White
            btnMonth.ForeColor = Drawing.Color.Yellow
            btnTerm.ForeColor = Drawing.Color.White

            btnMonth.BorderColor = Drawing.Color.White
        ElseIf Session("TimeMode") = 4 Then
            btnNow.ForeColor = Drawing.Color.White
            btnSevenDay.ForeColor = Drawing.Color.White
            btnFifteenDay.ForeColor = Drawing.Color.White
            btnMonth.ForeColor = Drawing.Color.White
            btnTerm.ForeColor = Drawing.Color.Yellow

            btnTerm.BorderColor = Drawing.Color.White
        End If

        If Session("SelectedMode") = "Homework" Then
            btnHomework.ForeColor = Drawing.Color.Yellow
            btnQuizHistory.ForeColor = Drawing.Color.White
            btnPracticeHistory.ForeColor = Drawing.Color.White
            btnLog.ForeColor = Drawing.Color.White

            btnHomework.BorderColor = Drawing.Color.White
        ElseIf Session("SelectedMode") = "QuizHistory" Then
            btnHomework.ForeColor = Drawing.Color.White
            btnQuizHistory.ForeColor = Drawing.Color.Yellow
            btnPracticeHistory.ForeColor = Drawing.Color.White
            btnLog.ForeColor = Drawing.Color.White

            btnQuizHistory.BorderColor = Drawing.Color.White
        ElseIf Session("SelectedMode") = "PracticeHistory" Then
            btnHomework.ForeColor = Drawing.Color.White
            btnQuizHistory.ForeColor = Drawing.Color.White
            btnPracticeHistory.ForeColor = Drawing.Color.Yellow
            btnLog.ForeColor = Drawing.Color.White

            btnPracticeHistory.BorderColor = Drawing.Color.White
        ElseIf Session("SelectedMode") = "Log" Then
            btnHomework.ForeColor = Drawing.Color.White
            btnQuizHistory.ForeColor = Drawing.Color.White
            btnPracticeHistory.ForeColor = Drawing.Color.White
            btnLog.ForeColor = Drawing.Color.Yellow

            btnLog.BorderColor = Drawing.Color.White
        End If
    End Sub

#Region "Event Click Filter Mode & Time"

    'แสดง UserControl ส่วนของประวัติควิซ
    Private Sub btnQuizHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuizHistory.Click
        ProcessModeQuizHistory()
        btnQuizHistory.ForeColor = Drawing.Color.Yellow
        btnQuizHistory.BorderColor = Drawing.Color.White
    End Sub

    'แสดง UserControl ส่วนของ ประวัติฝึกฝน
    Private Sub btnPracticeHistory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPracticeHistory.Click
        ProcessModePracticeHistory()
        btnPracticeHistory.ForeColor = Drawing.Color.Yellow
        btnPracticeHistory.BorderColor = Drawing.Color.White
    End Sub

    'แสดง Usercontrol ส่วนของ กิจกรรม(Log)
    Private Sub btnLog_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLog.Click
        ProcessModeLog()
        btnLog.ForeColor = Drawing.Color.Yellow
        btnLog.BorderColor = Drawing.Color.White
    End Sub

    'แสดง Usercontrol ส่วนของ การบ้าน
    Private Sub btnHomework_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHomework.Click
        ProcessModeHomework()
        btnHomework.ForeColor = Drawing.Color.Yellow
        btnHomework.BorderColor = Drawing.Color.White
    End Sub

    Private Sub btnNow_Click(sender As Object, e As EventArgs) Handles btnNow.Click
        Session("TimeMode") = 0
        RenderHistoryByMode()

    End Sub

    Private Sub btnSevenDay_Click(sender As Object, e As EventArgs) Handles btnSevenDay.Click
        Session("TimeMode") = 6
        RenderHistoryByMode()

    End Sub

    Private Sub btnFifteenDay_Click(sender As Object, e As EventArgs) Handles btnFifteenDay.Click
        Session("TimeMode") = 14
        RenderHistoryByMode()

    End Sub

    Private Sub btnMonth_Click(sender As Object, e As EventArgs) Handles btnMonth.Click
        Session("TimeMode") = 29
        RenderHistoryByMode()

    End Sub

    Private Sub btnTerm_Click(sender As Object, e As EventArgs) Handles btnTerm.Click
        Session("TimeMode") = 4
        RenderHistoryByMode()

    End Sub

#End Region

#Region "ProcessByMode"
    'Function ที่ใช้เมื่อกดปุ่ม เปรียบเทียบ
    Private Sub ProcessModeCompare(ByRef InputConn As SqlConnection)

        Dim BtmControl = CType(LoadControl("~/UserControl/CompareChartControl.ascx"), CompareChartControl)
        BtmControl.CreateChartStudentInfo(StudentId, SelectedCalendarId, TeacherId)
        CreateDivStudentInfo(StudentId, InputConn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, InputConn)
        DivBottomInfo.Controls.Clear()
        DivBottomInfo.Style.Add("margin-top", "10px")
        DivBottomInfo.Controls.Add(BtmControl)
        Session("SelectedMode") = "Compare" 'ให้จำไว้ว่ากดปุ่ม เปรียบเทียบ

        SetSelectedButton()
    End Sub

    'Function ที่ใช้เมื่อกดปุ่ม การบ้าน
    Private Sub ProcessModeHomework(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim IsnothingConn As Boolean = False 'ตัวเช็ค เปิดปิด connection ของมันเอง สำหรับไม่ได้ส่งเข้ามา
        If InputConn Is Nothing Then
            IsnothingConn = True
            InputConn = New SqlConnection
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        Dim BtmControl = CType(LoadControl("~/UserControl/GridDetailStudentControl.ascx"), GridDetailStudentControl)
        BtmControl.CreateGridHomework(StudentId, SelectedCalendarId, TeacherId, , InputConn)
        CreateDivStudentInfo(StudentId, InputConn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, InputConn)
        DivBottomInfo.Controls.Clear()
        DivBottomInfo.Style.Add("margin-top", "10px")
        DivBottomInfo.Controls.Add(BtmControl)
        Session("SelectedMode") = "Homework" 'ให้จำไว้ว่ากดปุ่ม การบ้าน

        If IsnothingConn Then
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        SetSelectedButton()
    End Sub

    'Function ที่ใช้เมื่อกดปุ่ม ประวัติควิซ
    Private Sub ProcessModeQuizHistory(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim IsnothingConn As Boolean = False 'ตัวเช็ค เปิดปิด connection ของมันเอง สำหรับไม่ได้ส่งเข้ามา
        If InputConn Is Nothing Then
            IsnothingConn = True
            InputConn = New SqlConnection
            UseCls.OpenExclusiveConnect(InputConn)
        End If
        Dim BtmControl = CType(LoadControl("~/UserControl/GridDetailStudentControl.ascx"), GridDetailStudentControl)
        BtmControl.CreateGridQuizHistory(StudentId, SelectedCalendarId, TeacherId, , InputConn)
        'BtmControl.CreateGridQuizHistory("3AE23CB5-1305-4CA0-8360-DD7256E78992", "184D8614-1121-4E67-9E7D-02DB59BA771C", "3BEE2B4F-A667-4419-B359-4D7D35BFC238")
        CreateDivStudentInfo(StudentId, InputConn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, InputConn)
        DivBottomInfo.Controls.Clear()
        DivBottomInfo.Style.Add("margin-top", "10px")
        DivBottomInfo.Controls.Add(BtmControl)
        Session("SelectedMode") = "QuizHistory" 'ให้จำไว้ว่ากดปุ่ม ประวัติควิซ

        If IsnothingConn Then
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        SetSelectedButton()
    End Sub

    'Function ที่ใช้เมื่อกดปุ่ม ประวัติฝึกฝน
    Private Sub ProcessModePracticeHistory(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim IsnothingConn As Boolean = False 'ตัวเช็ค เปิดปิด connection ของมันเอง สำหรับไม่ได้ส่งเข้ามา
        If InputConn Is Nothing Then
            IsnothingConn = True
            InputConn = New SqlConnection
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        Dim BtmControl = CType(LoadControl("~/UserControl/GridDetailStudentControl.ascx"), GridDetailStudentControl)
        BtmControl.CreateGridPracticeHistory(StudentId, SelectedCalendarId, TeacherId, , InputConn)
        CreateDivStudentInfo(StudentId, InputConn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, InputConn)
        DivBottomInfo.Controls.Clear()
        DivBottomInfo.Style.Add("margin-top", "10px")
        DivBottomInfo.Controls.Add(BtmControl)
        Session("SelectedMode") = "PracticeHistory" 'ให้จำไว้ว่ากดปุ่ม ประวัติฝึกฝน

        If IsnothingConn Then
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        SetSelectedButton()
    End Sub

    'Function ที่ใช้เมื่อกดปุ่ม กิจกรรม
    Private Sub ProcessModeLog(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim IsnothingConn As Boolean = False 'ตัวเช็ค เปิดปิด connection ของมันเอง สำหรับไม่ได้ส่งเข้ามา
        If InputConn Is Nothing Then
            IsnothingConn = True
            InputConn = New SqlConnection
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        DivBottomInfo.Controls.Clear()
        Dim labelLog As New Label
        labelLog.ID = "lblLog"
        labelLog.Text = "แสดงทุกๆกิจกรรมของนักเรียนคนนี้"
        labelLog.ForeColor = Drawing.Color.Gray
        labelLog.Style.Add("font-size", "30px")
        labelLog.Style.Add("font-weight", "bold")
        DivBottomInfo.Controls.Add(labelLog)
        DivBottomInfo.Style.Add("text-align", "center")
        DivBottomInfo.Style.Add("margin-top", "10px")
        Dim BtmControl = CType(LoadControl("~/UserControl/GridDetailStudentControl.ascx"), GridDetailStudentControl)
        BtmControl.CreateGridLog(StudentId, SelectedCalendarId, InputConn)
        CreateDivStudentInfo(StudentId, InputConn)
        CreateAccordionStudent(StudentId, SelectedCalendarId, InputConn)
        DivBottomInfo.Controls.Add(BtmControl)
        Session("SelectedMode") = "Log" 'ให้จำไว้ว่ากดปุ่ม กิจกรรม

        If IsnothingConn Then
            UseCls.OpenExclusiveConnect(InputConn)
        End If

        SetSelectedButton()
    End Sub

#End Region



End Class