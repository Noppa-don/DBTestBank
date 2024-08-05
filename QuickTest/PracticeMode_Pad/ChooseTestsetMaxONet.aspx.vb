Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class ChooseTestsetMaxONet
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)

    Public GroupName As String
    Protected DVID As String
    Protected TokenId As String

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected StudentId As String
    Private ClassName As String
    Private RoomName As String

    Protected SubjectAmount As Integer
    Protected LevelAmount As Integer
    Protected DailyActivitiesURL As String = ""


    Protected CheckPage As Integer = 1

    Private KnSession As New KNAppSession()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Session("SessionStatus") = "1"
        End If

        HttpContext.Current.Session("ChooseMode") = Nothing
        Session("PDeviceId") = Request.QueryString("DeviceUniqueID")
        Session("DeviceId") = Request.QueryString("DeviceUniqueID")
        If Request.QueryString("token") IsNot Nothing And Request.QueryString("token") <> "" Then
            Dim GuidToken As Guid = New Guid(Request.QueryString("token"))
            TokenId = GuidToken.ToString
        Else
            Dim MManagement As New MaxOnetManagement
            MManagement.DeviceId = Request.QueryString("DeviceUniqueID")
            TokenId = MManagement.GetTokenByDevice().ToString
        End If

        'Open Connection
        Dim connActivity As New SqlConnection
        db.OpenExclusiveConnect(connActivity)

        Dim dtPlayer As DataTable = ClsPracticeMode.GetPlayerDetail(Session("PDeviceId"), connActivity)

        If dtPlayer IsNot Nothing AndAlso dtPlayer.Rows.Count > 0 Then
            Dim SchoolCode As String = dtPlayer.Rows(0)("School_code").ToString
            ClassName = dtPlayer.Rows(0)("ClassName").ToString
            RoomName = dtPlayer.Rows(0)("RoomName").ToString
            Session("SchoolCode") = SchoolCode
            Session("SchoolID") = SchoolCode
            Session("selectedSession") = "PracticeFromComputer"
            Session("UserId") = dtPlayer.Rows(0)("Student_Id").ToString
            StudentId = Session("UserId")

            Dim limitExam As String = dtPlayer.Rows(0)("KCU_LimitExamAmount").ToString

            If limitExam = "" OrElse CInt(limitExam) >= 100 Then
                LimitAmount.Value = "100"
            Else
                LimitAmount.Value = limitExam
            End If
            'Session("PClassId") = GetLevelId(connActivity)
            InitialStudentSubject()

                SetCalendarId()

                'redis.SetKey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString()), dtPlayer.Rows(0)("IsViewAllTips"))
                'If Not Page.IsPostBack Then
                '    ' ส่วนของการแสดง qtip
                '    If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                '        Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                '        Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                '        NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
                '    End If
                'End If
            Else
                'ถ้าไม่เจอ tablet ให้ return กรณีนี้อาจเกิดจากการลงทะเบียนเพิ่มกับเครื่องอื่น
                Response.Redirect("~/DroidPad/NotUsage.aspx")
        End If

        DVID = Session("PDeviceId")
        GroupName = Session("selectedSession") 'signalR
        HttpContext.Current.Session("IsTeacher") = False

        'แสดงจำนวนเครดิต ที่ยังเหลืออยู่
        spnCreditAmount.InnerText = String.Format("({0})", GetCreditAmount())

        If Session("KeyCodeExpireDate") Is Nothing Then
            Session("KeyCodeExpireDate") = GetExpireDate()
        End If
        'label expiredate
        lblExpireDate.Text = Session("KeyCodeExpireDate").ToString()

        If HttpContext.Current.Session("DisableGetDailyActivities") Is Nothing Then
            Dim currentTime As DateTime = DateTime.Now()
            Dim fixTime As DateTime = Convert.ToDateTime("16:00 PM")
            'If currentTime > fixTime Then
            'If DateTime.Now.TimeOfDay >= New TimeSpan(16, 0, 0) AndAlso DateTime.Now.TimeOfDay >= New TimeSpan(24, 0, 0) Then
            DailyActivitiesURL = GetDailyActivities()
            'End If
        End If

        'Close Connection
        db.CloseExclusiveConnect(connActivity)

    End Sub

    Private Function GetExpireDate() As String
        If IsNothing(Session("ChooseTestSetMaxONet_GetExpireDate")) OrElse (Session("ChooseTestSetMaxONet_GetExpireDate") = "") Then

            Dim sql As String
            sql = "SELECT max(KCU_ExpireDate) as MaxExpireDate FROM maxonet_tblKeyCodeUsage WHERE KCU_Type <> 1 and KCU_OwnerId = '" & StudentId.CleanSQL & "'; "
            Dim MaxExpireDate As String = db.ExecuteScalar(sql)

            If MaxExpireDate = "" Then
                Session("ChooseTestSetMaxONet_GetExpireDate") = ""
            Else
                Dim exDate As DateTime = CDate(MaxExpireDate)
                Session("ChooseTestSetMaxONet_GetExpireDate") = String.Format("ใช้ได้จนถึง {0}", exDate.ToString("dd/MM/yyyy HH:mm"))
            End If

        End If

        Return Session("ChooseTestSetMaxONet_GetExpireDate").ToString()
    End Function

    Private Function GetDailyActivities() As String
        'Dim dt As DataTable = GetActivities()
        'Dim row As IEnumerable(Of DataRow) = dt.AsEnumerable().Where(Function(t) t.Field(Of Byte)("Module_Status") = 0)

        'If row.Count > 0 Then
        '    Dim urlActivity As String = "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & DVID & "&token=" & TokenId & "&Status=0&ItemId={0}"
        '    If SubjectAmount = 1 Then
        '        Return String.Format(urlActivity, dt.Rows(0)("Quiz_Id").ToString())
        '    Else
        '        Dim dailyHTML As New StringBuilder()
        '        Dim divActivity As String = "<div id='{4}' class=""{0}"" urlactivity=""{1}""><div style=""background-image: url('../Images/MaxOnet/Subject/{2}.png');""></div><span>{3}</span></div>"
        '        For Each r In dt.Rows()
        '            Dim subjectId As String = r("Subject_Id").ToString()
        '            Dim c As String = If((r("Module_Status") = 0), "subjectActivities", "success")
        '            If ("ม.4ม.5ม.6").IndexOf(ClassName) > -1 AndAlso r("Subject_Id").ToString().ToUpper() = CommonSubjectsText.EnglishSubjectId Then c = "success" ' วิชาอังกฤษต้องปิดไปก่อนของ ม.ปลาย
        '            Dim imageName As String = GetSubjectImage(r("Subject_Id").ToString())
        '            dailyHTML.Append(String.Format(divActivity, c, String.Format(urlActivity, r("Quiz_Id").ToString()), imageName, subjectId.SubjectIdToShortThName, subjectId))
        '        Next
        '        daily.InnerHtml = dailyHTML.ToString()
        '        Return "url"
        '    End If
        'End If
        'daily.InnerHtml = ""
        'Return ""
        Dim dam As New DialyActivityManagement
        Dim dt As DataTable = GetStudentSubjects()
        If dt.Rows.Count > 0 Then
            Dim isAllSuccess As Boolean = True
            Dim dailyHTML As New StringBuilder()
            Dim divActivity As String = "<div id='{0}' class=""{1}""><div style=""background-image: url('../Images/MaxOnet/Subject/{2}.png');""></div><span>{3}</span></div>"
            For Each r In dt.Rows
                Dim subjectId As String = r("SS_SubjectId").ToString()
                Dim classCss As String = If((dam.IsSubjectActivitySuccess(StudentId, subjectId)), "success", "subjectActivities")
                'If ("ม.4ม.5ม.6").IndexOf(ClassName) > -1 AndAlso r("Subject_Id").ToString().ToUpper() = CommonSubjectsText.EnglishSubjectId Then c = "success" ' วิชาอังกฤษต้องปิดไปก่อนของ ม.ปลาย
                Dim imageName As String = GetSubjectImage(subjectId)
                dailyHTML.Append(String.Format(divActivity, subjectId, classCss, imageName, subjectId.SubjectIdToShortThName))

                If classCss = "subjectActivities" Then isAllSuccess = False
            Next
            daily.InnerHtml = dailyHTML.ToString()
            Return If((isAllSuccess), "", "url")
        End If
        daily.InnerHtml = ""
        Return ""
    End Function

    Private Function GetStudentSubjects() As DataTable
        Dim sql As String = "SELECT distinct SS_StudentId,SS_SubjectId FROM maxonet_tblstudentsubject 
                                inner join maxonet_tblKeyCodeUsage on KeyCode_Code = SS_KeyCode 
								WHERE SS_StudentId = '" & StudentId & "'
                                and (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null)"
        Return db.getdata(sql)
    End Function

    Private Function GetSubjectImage(SubjectId As String) As String
        Select Case SubjectId.ToUpper
            Case CommonSubjectsText.ThaiSubjectId
                Return "Thai"
            Case CommonSubjectsText.EnglishSubjectId
                Return "eng"
            Case CommonSubjectsText.SocialSubjectId
                Return "social"
            Case CommonSubjectsText.MathSubjectId
                Return "math"
            Case CommonSubjectsText.ScienceSubjectId
                Return "Science"
            Case CommonSubjectsText.HomeSubjectId
                Return "home"
            Case CommonSubjectsText.HealthSubjectId
                Return "suk"
            Case CommonSubjectsText.ArtSubjectId
                Return "art"
            Case Else
                Return ""
        End Select
    End Function

    Private Function GetActivities() As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT mad.Student_Id,mdc.Quiz_Id,mad.Subject_Id,mdc.Module_Status,mad.LastUpdate FROM tblModuleAssignmentDetail mad  ")
        sql.Append(" INNER JOIN tblModuleDetailCompletion mdc ON mad.MA_Id = mdc.MA_Id WHERE mad.Student_Id = '" & StudentId & "' ")
        sql.Append(" AND mdc.MA_Id IN (SELECT m.MA_Id FROM tblModuleAssignmentDetail m where m.Subject_Id = mad.Subject_Id and  m.LastUpdate = ( ")
        sql.Append(" Select MAX(LastUpdate) FROM tblModuleAssignmentDetail WHERE tblModuleAssignmentDetail.Subject_Id = mad.Subject_Id ")
        sql.Append(" AND tblModuleAssignmentDetail.Student_Id = '" & StudentId & "')) ")
        Dim dt As DataTable = db.getdata(sql.ToString())
        sortSubjectMaxOnet(dt)
        Return dt
    End Function

    ''' <summary>
    ''' function ในการ sort วิชาตามลำดับของ maxonet thai,eng,social,math,science
    ''' </summary>
    ''' <param name="dt"></param>
    Private Sub sortSubjectMaxOnet(ByRef dt As DataTable)
        Dim col As New DataColumn
        col.DataType = GetType(Integer)
        col.AllowDBNull = False
        col.Caption = "Order"
        col.ColumnName = "Order"
        col.DefaultValue = 0
        dt.Columns.Add(col)

        For Each r In dt.Rows
            If r("Subject_Id").ToString().ToLower() = CommonSubjectsText.ThaiSubjectId.ToLower() Then
                r("Order") = 1
            ElseIf r("Subject_Id").ToString().ToLower() = CommonSubjectsText.EnglishSubjectId.ToLower() Then
                r("Order") = 2
            ElseIf r("Subject_Id").ToString().ToLower() = CommonSubjectsText.SocialSubjectId.ToLower() Then
                r("Order") = 3
            ElseIf r("Subject_Id").ToString().ToLower() = CommonSubjectsText.MathSubjectId.ToLower() Then
                r("Order") = 4
            ElseIf r("Subject_Id").ToString().ToLower() = CommonSubjectsText.ScienceSubjectId.ToLower() Then
                r("Order") = 5
            End If
        Next

        Dim View As New DataView(dt)
        View.Sort = "Order ASC"
        dt = View.ToTable()
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

    Private Function GetCalendarID(ByVal SchoolID As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String = " Select TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate And Calendar_ToDate And Calendar_Type = 3 And School_Code = '" & SchoolID.CleanSQL & "' AND IsActive = 1; "
        Dim db As New ClassConnectSql()
        Dim dt As DataTable
        dt = db.getdata(sql, , InputConn)
        If dt.Rows.Count = 0 Then
            dt.Clear()
            sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE Calendar_Type = '3' AND School_Code = '" & SchoolID.CleanSQL & "' " &
                  " AND dbo.GetThaiDate() >= Calendar_ToDate AND IsActive = 1 ORDER BY Calendar_ToDate DESC; "
            dt = db.getdata(sql, , InputConn)
        End If
        Return dt
    End Function

    Private Function GetLevelId(Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim sql As String = "SELECT Level_Id FROM tblLevel WHERE Level_ShortName = N'" & ClassName.CleanSQL & "';"
        Return db.ExecuteScalar(sql, InputConn)
    End Function

    Private Sub InitialStudentSubject()
        Dim sql As String = ""
        sql = "SELECT distinct SS_LevelId 
                FROM maxonet_tblStudentSubject inner join maxonet_tblKeyCodeUsage on KeyCode_Code = SS_KeyCode
                WHERE SS_StudentId = '" & StudentId.CleanSQL & "' and (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null);"

        Dim dtLevel As DataTable = db.getdata(sql)
        LevelAmount = dtLevel.Rows.Count()

        If LevelAmount = 1 Then
            Session("PClassId") = dtLevel.Rows(0)("SS_LevelId").ToString
        End If

        sql = "SELECT distinct SS_SubjectId 
                FROM maxonet_tblStudentSubject inner join maxonet_tblKeyCodeUsage on KeyCode_Code = SS_KeyCode
                WHERE SS_StudentId = '" & StudentId.CleanSQL & "' and (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null);"

        Dim dtSubject As DataTable = db.getdata(sql)
        SubjectAmount = dtSubject.Rows.Count()

        If SubjectAmount = 1 Then
            Session("PSubjectName") = dtSubject.Rows(0)("SS_SubjectId").ToString()
            Session("PClassId") = dtLevel.Rows(0)("SS_LevelId").ToString()
            Session("IsOneSubjectMaxOnet") = True
        Else
            Session("PSubjectName") = Nothing
            Session("IsOneSubjectMaxOnet") = False
            Session("SubjectAmount") = SubjectAmount
            Session("LevelAmount") = LevelAmount
        End If
    End Sub

    Private Function GetCreditAmount() As Integer
        Dim sql As String = "select sum(KCU_CreditAmount) as TotalCredit from maxonet_tblKeyCodeUsage where KCU_Token = '" & TokenId.CleanSQL & "'
                                and (KCU_ExpireDate > dbo.GetThaiDate() or KCU_ExpireDate is null)"
        Dim result As String = db.ExecuteScalar(sql)
        Dim KeyCodeCredit As Integer = If((result = ""), -1, CInt(result))

        Return KeyCodeCredit - StudentSubjectRegisteredAmount()

    End Function

    Private Function StudentSubjectRegisteredAmount() As Integer
        Dim sql As String = "SELECT count(distinct ssid) FROM maxonet_tblStudentSubject inner join maxonet_tblKeyCodeUsage on KeyCode_Code = SS_KeyCode
                                and (KCU_ExpireDate > dbo.GetThaiDate() or KCU_ExpireDate is null) WHERE SS_StudentId = '" & StudentId & "' 
                                and KCU_Isactive = 1  and (kcu_type = 0 or KCU_Type = 3 or KCU_Type = 4);"
        Return db.ExecuteScalar(sql)
    End Function
End Class