Imports System.Data.SqlClient
Imports System.Web

Public Class SchoolNewsPage
    Inherits System.Web.UI.Page
    'ตัวแปรที่ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    Dim Activity As New ClsActivity(New ClassConnectSql)
    'ตัวแปรที่ไว้เช็คว่า user ที่กำลังใช้งานอยู่ตอนนี้เป็นนักเรียนหรือเปล่า ถ้าเป็นนักเรียนต้องซ่อน div เพิ่มข่าวประกาศสำหรับครูออกไป (เอาไปใช้ฝั่ง JavaScript)
    Protected IsStudent As Boolean = False
    Protected CheckIsStudent As String = "false"

    Protected IsAllowNewsPost As Boolean

    ''' <summary>
    ''' ทำการ BindRepeater ข่าวทั้งหมด และเช็คว่า user ที่ใช้งานอยู่ตอนนี้เป็น ครู หรือ นักเรียน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HttpContext.Current.Session("SchoolCode") = "1000001"
        If HttpContext.Current.Request.QueryString("IsStudent") IsNot Nothing Then
            IsStudent = True
            CheckIsStudent = "true"
        Else
            IsAllowNewsPost = GetIsAllowNewsPost()
        End If

        If Not Page.IsPostBack Then
            If HttpContext.Current.Request.QueryString("AfterInsert") IsNot Nothing Then
                BindRepeaterOurNews()
                DivNewsDetail.Style.Add("display", "none")
                DivCreateNews.Style.Add("display", "none")
                DivOurNewsDetail.Style.Add("display", "block")
            Else
                BindingRepeaterNews()
            End If

        End If
    End Sub

    ''' <summary>
    ''' function get isAllowNewsPost ว่าครูสามารถมีสิทธ์ Post ข่าวได้หรือปล่าว
    ''' </summary>
    ''' <returns>boolean</returns>
    Private Function GetIsAllowNewsPost() As Boolean
        Try
            Dim sql As String = " SELECT IsAllowNewsPost FROM t360_tblTeacher WHERE Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "';"
            Dim db As New ClassConnectSql()
            Return Convert.ToBoolean(db.ExecuteScalar(sql))
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' ทำการ select ข้อมูลของข่าวของโรงเรียน ในกรณีที่เป็นครูหาข่าวที่ครูเป็นผู้ประกาศ แต่ในกรณีที่เป็นนักเรียนก็หาข่าวที่ประกาศถึงตัวนักเรียน หรือ ห้องของนักเรียน
    ''' </summary>
    ''' <param name="IsCurrentNews">ตัวแปรที่บอกว่าเป็นข่าวในปัจจุบันหรือเปล่า หรือ ว่าเป็นข่าวที่เลยเวลาไปแล้ว</param>
    ''' <remarks></remarks>
    Private Sub BindingRepeaterNews(Optional ByVal IsCurrentNews As Boolean = True)
        'Open Connection
        Dim connSchoolNews As New SqlConnection
        _DB.OpenExclusiveConnect(connSchoolNews)

        Dim sql As String

        Dim sqlDateNews As String = " CAST(DATEPART(DAY,News_StartDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(MONTH,News_StartDate) AS VARCHAR(2)) + '/' +  "
        sqlDateNews &= " CAST((DATEPART(YEAR,News_StartDate) + 543) AS VARCHAR(4)) + ' - ' +  CAST(DATEPART(DAY,News_EndDate) AS VARCHAR(2)) + '/' +  "
        sqlDateNews &= " CAST(DATEPART(MONTH,News_EndDate) AS VARCHAR(2)) + '/' +  CAST(DATEPART(YEAR,News_EndDate) + 543 AS VARCHAR(4)) AS DateNews   "

        Dim sqlWhereNormalField As String = " WHERE t360_tblNews.School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString()
        sqlWhereNormalField &= "' AND News_IsActive = '1' and t360_tblNewsRoom.IsActive = '1' "


        Dim sqlCurrentNews As String
        If IsCurrentNews = True Then
            sqlCurrentNews = " AND dbo.GetThaiDate() BETWEEN News_StartDate AND News_EndDate "
        Else
            sqlCurrentNews = " AND News_EndDate < dbo.GetThaiDate() "
        End If

        Dim dt As New DataTable

        Dim IsTeacherNoRoom As Boolean = False
        If IsStudent = False Then
            IsTeacherNoRoom = CheckIsTeacherNoRoom()
        End If

        'If IsTeacherNoRoom Then
        '    sql = "select distinct * from (select t360_tblNews.News_Id, cast(News_Information as varchar(max)) as News_Information,News_Announcer, News_StartDate,TeacherIsSeen"
        '    sql &= sqlDateNews & " from t360_tblNewsRoom inner join t360_tblNews on t360_tblNews.News_Id = t360_tblNewsRoom.News_Id"
        '    sql &= sqlWhereNormalField & " and t360_tblNews.News_ToTeacherNoRoom = '1' " & sqlCurrentNews
        '    sql &= ") as a ORDER BY a.News_StartDate"

        If IsStudent Then
            sql = "select t360_tblNews.News_Id, cast(News_Information as varchar(max)) as News_Information,News_Announcer, News_StartDate,t360_tblNewsDetailCompletion.StudentIsSeen,"
            sql &= sqlDateNews & " from t360_tblNewsRoom inner join t360_tblNews on t360_tblNews.News_Id = t360_tblNewsRoom.News_Id"
            sql &= " inner join t360_tblNewsDetailCompletion on t360_tblNewsRoom.NR_Id = t360_tblNewsDetailCompletion.NR_Id"
            sql &= " inner join t360_tblTabletOwner on t360_tblNewsDetailCompletion.User_Id = t360_tblTabletOwner.Owner_Id  "
            sql &= " inner join t360_tblTablet on t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id"
            sql &= sqlWhereNormalField & " and Tablet_SerialNumber = '" & HttpContext.Current.Session("PDeviceId").ToString() & "' and t360_tblNews.News_ToStudent = '1' "
            sql &= sqlCurrentNews & " ORDER BY t360_tblNewsDetailCompletion.Lastupdate desc"

        Else
            sql = "select t360_tblNews.News_Id, cast(News_Information as varchar(max)) as News_Information,News_Announcer, News_StartDate,TeacherIsSeen,"
            sql &= sqlDateNews
            sql &= " from t360_tblNewsRoom inner join t360_tblNews on t360_tblNews.News_Id = t360_tblNewsRoom.News_Id"
            sql &= " inner join t360_tblNewsDetailCompletion on t360_tblNewsRoom.NR_Id = t360_tblNewsDetailCompletion.NR_Id  "
            sql &= sqlWhereNormalField & " and User_Id = '" & HttpContext.Current.Session("UserId").ToString() & "' and t360_tblNews.News_ToTeacher = '1' "
            sql &= sqlCurrentNews & " ORDER BY t360_tblNewsDetailCompletion.Lastupdate  desc"

        End If

        dt = _DB.getdata(sql, , connSchoolNews)

        If dt.Rows.Count <> 0 Then
            If IsStudent Then
                SetIconNew(dt, True)
            Else
                SetIconNew(dt, False)
            End If

            Repeater1.DataSource = dt
            Repeater1.DataBind()
            If IsStudent Then
                Dim StudentId As String = Activity.GetPlayerIdByDeviceId(HttpContext.Current.Session("PDeviceId").ToString())
                UpdateStudentIsSeen(StudentId, False)
            Else
                UpdateTeacherIsSeen(HttpContext.Current.Session("UserId").ToString())
            End If
        End If

        'CloseConnection
        _DB.CloseExclusiveConnect(connSchoolNews)
    End Sub
    ''' <summary>
    ''' เช็คว่าครูที่ Login อยู่มีห้องประจำชั้นรึเปล่า
    ''' </summary>
    ''' <returns>True เมื่อไม่มีห้องประจำชั้น</returns>
    ''' <remarks>ตั้งห้องประจำชั้นที่ t360 , ครู 1 คนเพิ่มเป็นครูประจำชั้นได้หลายห้อง</remarks>
    Private Function CheckIsTeacherNoRoom() As Boolean
        Dim sql As String = ""
        sql = "select top 1 Class_Name from t360_tblTeacher inner join t360_tblTeacherRoom on t360_tblTeacher.Teacher_id = t360_tblTeacherRoom.Teacher_Id  "
        sql &= " where t360_tblTeacher.Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' And Teacher_IsActive = '1'"
        Dim classname As String = _DB.ExecuteScalar(sql)
        Dim IsTeacherNoRoom As Boolean = False
        If classname = "" Then
            IsTeacherNoRoom = True
        End If

        Return IsTeacherNoRoom
    End Function

    ''' <summary>
    ''' หาข้อมูลของนักเรียน
    ''' </summary>
    ''' <returns>dt ข้อมูลนักเรียน</returns>
    ''' <remarks></remarks>
    Private Function GetDtStudent() As DataTable
        Dim sql As String
        sql = " select 'Student' as UserType,Student_CurrentClass as CurrentClass,Student_CurrentRoom as CurrentRoom,Student_Id,Student_CurrentRoomId from t360_tblStudent "
        sql &= " where Student_id = '" & HttpContext.Current.Session("UserId").ToString() & "' And Student_IsActive = '1';"
        Return _DB.getdata(sql)
    End Function


    ''' <summary>
    ''' หาเฉพาะข่าวของที่ครูประกาศ
    ''' </summary>
    ''' <param name="Inputconn">ตัวแปร Connection</param>
    ''' <returns>dt ที่เป็นข่าวของที่ตัวเองประกาศเท่านั้น(ครู)</returns>
    ''' <remarks></remarks>
    Private Function GetDtOurNews(Optional ByRef Inputconn As SqlConnection = Nothing) As DataTable
        Dim sql As String = " SELECT tblTeacherNews.TN_Id, tblTeacherNews.Description, 'วันที่ ' + CAST(DATEPART(DAY, tblTeacherNews.StartDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(MONTH,  " &
                            " tblTeacherNews.StartDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(YEAR, tblTeacherNews.StartDate) + 543 AS VARCHAR(4)) + ' ' + CAST(CONVERT(TIME,  " &
                            " tblTeacherNews.StartDate) AS VARCHAR(8)) + ' ถึง ' + CAST(DATEPART(DAY, tblTeacherNews.EndDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(MONTH,  " &
                            " tblTeacherNews.EndDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(YEAR, tblTeacherNews.EndDate) + 543 AS VARCHAR(4)) + ' ' + CAST(CONVERT(TIME,  " &
                            " tblTeacherNews.EndDate) AS VARCHAR(8)) AS DurationDateTime,CASE WHEN dbo.tblTeacherNewsDetail.Room_Id IS NULL " &
                            " THEN 'ประกาศถึง ' + dbo.t360_tblStudent.Student_FirstName + ' ' + dbo.t360_tblStudent.Student_LastName " &
                            " ELSE 'ประกาศถึง ' + dbo.t360_tblRoom.Class_Name + ' ' + dbo.t360_tblRoom.Room_Name END as NoticeTo " &
                            " FROM tblTeacherNews INNER JOIN tblTeacherNewsDetail ON tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id LEFT OUTER JOIN " &
                            " t360_tblRoom ON tblTeacherNewsDetail.Room_Id = t360_tblRoom.Room_Id LEFT OUTER JOIN " &
                            " t360_tblStudent ON tblTeacherNewsDetail.Student_Id = t360_tblStudent.Student_Id " &
                            " WHERE (tblTeacherNews.Teacher_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') AND (tblTeacherNews.IsActive = 1) ORDER BY dbo.tblTeacherNews.LastUpdate DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , Inputconn)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการลบข่าวที่เลือกมา(ซึ่งเป็นข่าวของตัวเอง(ครู))
    ''' </summary>
    ''' <param name="TNID">TeacherNewsId:tblTeacherNews</param>
    ''' <remarks></remarks>
    Private Sub DeleteOurNews(ByVal TNID As String)
        If TNID IsNot Nothing And TNID <> "" Then
            Try
                Dim sql As String = " UPDATE dbo.tblTeacherNews SET IsActive = 0,LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE TN_Id = '" & TNID & "' "
                _DB.Execute(sql)
            Catch ex As Exception

            End Try
        End If
    End Sub

    ''' <summary>
    ''' ทำการ BindRepeater ข่าวที่ตัวเองเป็นผู้ประกาศลงไป(ครู)
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub BindRepeaterOurNews(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim dt As New DataTable
        dt = GetDtOurNews(InputConn)
        If dt.Rows.Count > 0 Then
            Repeater2.DataSource = dt
            Repeater2.DataBind()
        Else
            Repeater2.DataSource = Nothing
            Repeater2.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' เลือกดูข่าวเฉพาะที่ยังอยู่ในช่วงเวลาที่ Active อยู่
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CurrentNews_Click(sender As Object, e As EventArgs) Handles CurrentNews.Click
        DivOurNewsDetail.Style.Add("display", "none")
        DivCreateNews.Style.Add("display", "none")
        DivNewsDetail.Style.Add("display", "block")
        BindingRepeaterNews()
    End Sub

    ''' <summary>
    ''' ดูข่าวที่มันเลยช่วงเวลาที่ Active ไปแล้ว (เป็นประวัติของข่าว)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HistoryNews_Click(sender As Object, e As EventArgs) Handles HistoryNews.Click
        DivOurNewsDetail.Style.Add("display", "none")
        DivCreateNews.Style.Add("display", "none")
        DivNewsDetail.Style.Add("display", "block")
        BindingRepeaterNews(False)
    End Sub

    ''' <summary>
    ''' ทำการ BindRepeater ข่าวเฉพาะที่ตัวเองเป็นผู้ประกาศเท่านั้น (ในกรณีของครูเท่านั้น)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOurNews_Click(sender As Object, e As EventArgs) Handles btnOurNews.Click
        If HttpContext.Current.Session("UserId") IsNot Nothing And HttpContext.Current.Session("UserId").ToString() <> "" Then
            'Open Connection
            Dim connTeacherNews As New SqlConnection
            _DB.OpenExclusiveConnect(connTeacherNews)
            DivNewsDetail.Style.Add("display", "none")
            DivCreateNews.Style.Add("display", "none")
            DivOurNewsDetail.Style.Add("display", "block")
            BindRepeaterOurNews(connTeacherNews)
            'Close Connection
            _DB.CloseExclusiveConnect(connTeacherNews)
        End If
    End Sub

    ''' <summary>
    ''' ทำการแสดง Div ที่ให้ครูประกาศข่าว เพื่อกรอกรายละเอียด เลือก ชั้น/ห้อง/คน เพื่อประกาศข่าว (ครูเท่านั้น)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCreateNews_Click(sender As Object, e As EventArgs) Handles btnCreateNews.Click
        HttpContext.Current.Session("TNIDEditNews") = Nothing
        DivOurNewsDetail.Style.Add("display", "none")
        DivNewsDetail.Style.Add("display", "none")
        DivCreateNews.Style.Add("display", "block")
        'Open Connection
        Dim ConnCreateNews As New SqlConnection
        _DB.OpenExclusiveConnect(ConnCreateNews)
        GenHtmlClassPanel(ConnCreateNews)
        'Close Connetion
        _DB.CloseExclusiveConnect(ConnCreateNews)
    End Sub

    ''' <summary>
    ''' เป็น Event ที่เมื่อกดปุ่ม ดินสอ , ยางลบ ใน Repeater จะมาเข้า Function นี้ และดักแยกถ้าเป็นดินสอจะทำการเก็บ TNID เอาไว้ใน Session เพื่อเอาไป bind ข้อมูลใส่ Panel แก้ไข/เพิ่ม ข่าวอีกทีนึง แต่ถ้าเป็น
    ''' ปุ่มยางลบก็ให้ทำการลบข่าวนั้นทิ้งเลย
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Repeater2_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles Repeater2.ItemCommand
        If e.CommandName = "ImgEditClick" Then
            'Response.Write(e.CommandArgument.ToString())
            GenHtmlEditTeacherNews(e.CommandArgument.ToString())
            'เอา Session ตัวนี้ไปใช้เป็นตัว Where ในการ update ข้อมูลอีกทีนึงในขั้นตอนการแก้ไข
            HttpContext.Current.Session("TNIDEditNews") = e.CommandArgument.ToString()
            SetHDRoomOrStudent(HttpContext.Current.Session("TNIDEditNews"))
        ElseIf e.CommandName = "DeleteOurNews" Then
            DeleteOurNews(e.CommandArgument.ToString())
            BindRepeaterOurNews()
        End If
    End Sub

    Private Sub SetHDRoomOrStudent(TNID As String)
        Dim sql As String
        sql = "select t360_tblStudent.Student_Id ,Student_CurrentClass + Student_CurrentRoom as ClassRoom from tblTeacherNewsDetailCompletion "
        sql &= " inner join t360_tblstudent on tblTeacherNewsDetailCompletion.Student_Id = t360_tblStudent.Student_Id "
        sql &= " inner join tblTeacherNewsDetail on tblTeacherNewsDetailCompletion.TND_Id = tblTeacherNewsDetail.TND_Id "
        sql &= " where TN_Id = '" & TNID & "'"

        Dim dt As DataTable = _DB.getdata(sql)

        If dt.Rows.Count() = 1 Then
            HDClassRommName.Value() = dt.Rows(0)("ClassRoom").ToString
            HDStudentId.Value() = dt.Rows(0)("Student_Id").ToString
        ElseIf dt.Rows.Count() > 1 Then
            HDClassRommName.Value() = dt.Rows(0)("ClassRoom").ToString
        Else
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' ทำการ select หาข้อมูลต่างๆที่เกียวกับข่าวนี้ขึ้นมา bind ใส่ control
    ''' </summary>
    ''' <param name="TNID">TeacherNewsId ที่ต้องการแก้ไขข่าว</param>
    ''' <remarks></remarks>
    Private Sub GenHtmlEditTeacherNews(ByVal TNID As String)
        If TNID.ToString <> "" And TNID IsNot Nothing Then
            Dim sql As String = " SELECT dbo.tblTeacherNews.TN_Id,StartDate,EndDate " &
                                " ,Class_Name,Room_Id,Student_Id,Description FROM dbo.tblTeacherNews INNER JOIN dbo.tblTeacherNewsDetail " &
                                " ON dbo.tblTeacherNews.TN_Id = dbo.tblTeacherNewsDetail.TN_Id WHERE dbo.tblTeacherNews.TN_Id = '" & TNID & "' "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GenHtmlEditDate(dt.Rows(0)("StartDate"), dt.Rows(0)("EndDate"))
                If dt.Rows(0)("Room_Id") IsNot DBNull.Value Then
                    GenHtmlEditClassRoomStudent(dt.Rows(0)("Room_Id").ToString(), "", dt.Rows(0)("Description").ToString())
                Else
                    GenHtmlEditClassRoomStudent("", dt.Rows(0)("Student_Id").ToString(), dt.Rows(0)("Description").ToString())
                End If
                DivOurNewsDetail.Style.Add("display", "none")
                DivNewsDetail.Style.Add("display", "none")
                DivCreateNews.Style.Add("display", "block")
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการหาสตริง วันที่/เวลา ที่เริ่มประกาศข่าว และ วันสิ้นสุด จาก Format ที่ select ขึ้นมาได้จาก DB แล้วทำการแปลงเป็นรูปแบบ วัน/เดือน/ปี แล้ว bind กับเข้าไปใน txtbox
    ''' </summary>
    ''' <param name="InputStartDate">วันที่เริ่มประกาศข่าว</param>
    ''' <param name="InputEndDate">วันที่สิ้นสุด</param>
    ''' <remarks></remarks>
    Private Sub GenHtmlEditDate(ByVal InputStartDate As Date, ByVal InputEndDate As Date)
        Dim GetStartDate As Date = InputStartDate
        Dim GetEndDate As Date = InputEndDate
        If GetStartDate.Year > 2400 Then
            GetStartDate = New Date((GetStartDate.Year - 543), GetStartDate.Month, GetStartDate.Day)
            GetEndDate = New Date((GetEndDate.Year - 543), GetEndDate.Month, GetEndDate.Day)
        End If
        Dim StrStartDate As String = GetStartDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("EN"))
        Dim StrEndDate As String = GetEndDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("EN"))
        StartDate.Value = StrStartDate
        EndDate.Value = StrEndDate
    End Sub

    ''' <summary>
    ''' ทำการ Gen สตริง HTML ชั้น/ห้อง/คน แล้วทำการ selected ข้อมูลที่เลือกไว้โดยใช้ข้อมูลจาก DB
    ''' </summary>
    ''' <param name="RoomId">ห้องที่เลือก</param>
    ''' <param name="StudentId">นักเรียนที่เลือก</param>
    ''' <param name="txtNews">รายละเอียดของข่าว</param>
    ''' <remarks></remarks>
    Private Sub GenHtmlEditClassRoomStudent(ByVal RoomId As String, ByVal StudentId As String, ByVal txtNews As String)
        Dim sql As String = ""
        Dim dt As New DataTable
        If RoomId <> "" Then 'ถ้า RoomId ไม่ได้เป็นค่าว่างแสดงว่าเขาเลือกมาสุดแค่ห้อง
            sql = " SELECT Class_Name,Room_Name FROM dbo.t360_tblRoom WHERE Room_Id = '" & RoomId & "' "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                'Gen HTML ชั้นที่เลือก
                GenHtMLClassNameByEdit(dt.Rows(0)("Class_Name"))
                'Gen HTML ห้องที่เลือก
                GenHTMLRoomNameByEdit(dt.Rows(0)("Class_Name"), dt.Rows(0)("Room_Name"))
                'Gem HTML นักเรียนแต่ไม่ต้องเลือก
                GenHtmlStudentNameByEdit(dt.Rows(0)("Class_Name"), dt.Rows(0)("Room_Name"), "")
            End If
        Else 'Gen ถึงนักเรียนที่เลือกเลย
            sql = " SELECT Student_CurrentClass AS Class_Name,Student_CurrentRoom AS Room_Name " &
                  " FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                'Gen HTML ชั้นที่เลือก
                GenHtMLClassNameByEdit(dt.Rows(0)("Class_Name"))
                'Gen HTML ห้องที่เลือก
                GenHTMLRoomNameByEdit(dt.Rows(0)("Class_Name"), dt.Rows(0)("Room_Name"))
                'Gem HTML นักเรียน
                GenHtmlStudentNameByEdit(dt.Rows(0)("Class_Name"), dt.Rows(0)("Room_Name"), StudentId)
            End If
        End If
        CreateNewstxt.InnerHtml = txtNews
    End Sub

    ''' <summary>
    ''' ทำการ Gen HTML ชั้น โดยให้ selected ชั้นเรียนที่เลือก
    ''' </summary>
    ''' <param name="ClassName">ชั้นเรียนที่เลือก</param>
    ''' <remarks></remarks>
    Private Sub GenHtMLClassNameByEdit(ByVal ClassName As String)
        Dim Sb As New StringBuilder()
        Dim dt As New DataTable
        Dim sql As String = " SELECT DISTINCT Student_CurrentClass FROM dbo.t360_tblStudent " &
                            " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' AND Student_IsActive = 1 " &
                            " ORDER BY Student_CurrentClass "
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Sb.Append("<div style='height:170px;overflow:auto;'><table id='tableClass' runat='server' style='text-align:center;'>")
            'ทำการ loop เพื่อ Gen ชั้นเรียนทั้งหมด และ ดักว่าถ้าเป็นชั้นเรียนที่เลือกต้องทำการ selected ด้วย , เงื่อนไขการจบ loop คือ วนจนหมดทุกชั้นใน รร. นี้
            For index = 0 To dt.Rows.Count - 1
                Sb.Append("<tr>")
                If dt.Rows(index)("Student_CurrentClass").ToString().Trim() = ClassName.Trim() Then
                    Sb.Append("<td classname='" & dt.Rows(index)("Student_CurrentClass") & "' class='FortdClass' Style='background-color:rgb(255, 132, 0);color:white;'>")
                Else
                    Sb.Append("<td classname='" & dt.Rows(index)("Student_CurrentClass") & "' class='FortdClass'>")
                End If
                Sb.Append(dt.Rows(index)("Student_CurrentClass"))
                Sb.Append("</td></tr>")
            Next
            Sb.Append("</table></div>")
            DivClass.InnerHtml = Sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen HTML ห้อง โดยให้ selected ห้องที่เลือก
    ''' </summary>
    ''' <param name="ClassName">ชั้นที่เลือก</param>
    ''' <param name="RoomName">ห้องที่เลือก</param>
    ''' <remarks></remarks>
    Private Sub GenHTMLRoomNameByEdit(ByVal ClassName As String, ByVal RoomName As String)
        Dim sql As String = " SELECT DISTINCT Student_CurrentClass + Student_CurrentRoom AS ClassRoomName FROM dbo.t360_tblStudent " &
                          " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' " &
                          " AND Student_CurrentClass = '" & ClassName & "' " &
                          " AND Student_IsActive = 1 ORDER BY Student_CurrentClass + Student_CurrentRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim sb As New StringBuilder()
        If dt.Rows.Count > 0 Then
            sb.Append("<div style='height:170px;overflow:auto;'><table id='tableRoom' style='text-align:center;'>")
            'loop เพื่อ สร้างห้องจากชั้นที่เลือกมา และทำการดักว่าตรงกับห้องที่เลือกก็ให้ทำการ selected ห้องนั้นด้วย , เงื่อนไขการจบ loop คือ วนจนครบทุกห้องจากชั้นที่เลือกมา
            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("ClassRoomName").ToString().Trim() = ClassName.Trim() & RoomName.Trim() Then
                    sb.Append("<tr><td roomname='" & dt.Rows(index)("ClassRoomName") & "' class='FortdRoom' style='background-color:rgb(219, 175, 0);color:white;'>")
                Else
                    sb.Append("<tr><td roomname='" & dt.Rows(index)("ClassRoomName") & "' class='FortdRoom'>")
                End If
                sb.Append(dt.Rows(index)("ClassRoomName") & "</td></tr>")
            Next
            sb.Append("</table></div>")
            DivRoom.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen HTML นักเรียนที่อยู่ใน ชั้น/ห้อง ที่เลือกมา และถ้าเป็นนักเรียนที่เลือกก็ให้ทำการ Selected
    ''' </summary>
    ''' <param name="ClassName">ชั้นที่เลือกมา</param>
    ''' <param name="RoomName">ห้องที่เลือกมา</param>
    ''' <param name="StudentId">นักเรียนที่เลือกมา</param>
    ''' <remarks></remarks>
    Private Sub GenHtmlStudentNameByEdit(ByVal ClassName As String, ByVal RoomName As String, ByVal StudentId As String)
        Dim sql As String = " SELECT Student_Id,Student_FirstName + ' ' + Student_LastName AS StudentName FROM dbo.t360_tblStudent " &
                            " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' AND Student_CurrentClass = '" & ClassName & "' " &
                            " AND Student_CurrentRoom = '" & RoomName & "' " &
                            " AND Student_IsActive = 1 ORDER BY Student_CurrentNoInRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim sb As New StringBuilder()
        If dt.Rows.Count > 0 Then
            sb.Append("<div style='height:170px;overflow:auto;'><table id='tableStudent' style='text-align:center;'>")
            'loop เพื่อ Gen นักเรียนที่อยู่ในห้องที่เลือกมา และ ถ้านักเรียนตรงกับนักเรียนที่เลือกมาต้องทำการ selected , เงื่อนไขการจบ loop คือ วนนักเรียนจนครบทุกคนที่อยู่ในห้องที่เลือกมา
            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("Student_Id").ToString() = StudentId Then
                    sb.Append(" <tr><td stuId='" & dt.Rows(index)("Student_Id").ToString() & "' style='background-color:rgb(255, 27, 203);color:white;' class='FortdStudent'>" & dt.Rows(index)("StudentName") & "</td></tr><tr>")
                Else
                    sb.Append(" <tr><td stuId='" & dt.Rows(index)("Student_Id").ToString() & "' class='FortdStudent'>" & dt.Rows(index)("StudentName") & "</td></tr><tr>")
                End If
            Next
            sb.Append("</table></div>")
            DivStudent.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen HTML ของชั้นทั้งหมดใน รร. นี้ เพื่อเป็นข้อมูลให้เลือกตอนจะประกาศข่าว
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub GenHtmlClassPanel(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " SELECT DISTINCT Student_CurrentClass FROM dbo.t360_tblStudent " &
                            " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' AND Student_IsActive = 1 " &
                            " ORDER BY Student_CurrentClass "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        Dim sb As New StringBuilder()
        If dt.Rows.Count > 0 Then
            sb.Append("<div style='height:170px;overflow:auto;'><table id='tableClass' runat='server' style='text-align:center;'>")
            'loop เพื่อ Gen ชั้นทั้งหมดที่อยู่ใน รร. นี้ , เงื่อนไขการจบ loop คือ วนจรครบทุกชั้น
            For index = 0 To dt.Rows.Count - 1
                sb.Append("<tr>")
                sb.Append("<td classname='" & dt.Rows(index)("Student_CurrentClass") & "' class='FortdClass'>")
                sb.Append(dt.Rows(index)("Student_CurrentClass"))
                sb.Append("</td></tr>")
            Next
            sb.Append("</table></div>")
            DivClass.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary> 
    ''' ทำการ Gen HTML ของห้องทั้งหมดที่อยู่ใน ชั้นที่เลือกมา เพื่อเป็นข้อมูลให้เลือกตอนจะประกาศข่าว
    ''' </summary>
    ''' <param name="ClassName">ชั้นที่เลือกมา</param>
    ''' <returns>สตริง HTML ห้องทั้งหมดจากชั้นที่เลือกมา</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GenHtmlRoomPanel(ByVal ClassName As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT DISTINCT Student_CurrentClass + Student_CurrentRoom AS ClassRoomName FROM dbo.t360_tblStudent " &
                            " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' " &
                            " AND Student_CurrentClass = '" & ClassName & "' " &
                            " AND Student_IsActive = 1 ORDER BY Student_CurrentClass + Student_CurrentRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim StrRoom As String = "<div style='height:170px;overflow:auto;'><table id='tableRoom' style='text-align:center;'>"
            'loop เพื่อ Gen ห้องทั้งหมดจากห้องที่เลือกมา , เงื่อนไขการจบ loop คือ วนให้หมดทุกห้อง
            For index = 0 To dt.Rows.Count - 1
                StrRoom &= "<tr><td roomname='" & dt.Rows(index)("ClassRoomName") & "' class='FortdRoom'>"
                StrRoom &= dt.Rows(index)("ClassRoomName") & "</td></tr>"
            Next
            StrRoom &= "</table></div>"
            _DB = Nothing
            Return StrRoom
        Else
            _DB = Nothing
            Return ""
        End If
    End Function

    ''' <summary>
    ''' ทำการ Gen HTML ของนักเรียนทั้งหมดจากห้องที่เลือกมา เพื่อเป็นข้อมูลให้เลือกตอนจะประกาศข่าว
    ''' </summary>
    ''' <param name="ClassName">ชั้นที่เลือกมา</param>
    ''' <param name="RoomName">ห้องที่เลือกมา</param>
    ''' <returns>สตริง HTML นักเรียนทั้งหมดจาก ห้อง/ชั้น ที่เลือกมา</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GenHtmlStudentPanel(ByVal ClassName As String, ByVal RoomName As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT Student_Id,Student_FirstName + ' ' + Student_LastName AS StudentName FROM dbo.t360_tblStudent " &
                            " WHERE School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "' AND Student_CurrentClass = '" & ClassName & "' " &
                            " AND Student_CurrentRoom = '" & RoomName & "' " &
                            " AND Student_IsActive = 1 ORDER BY Student_CurrentNoInRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim strStudent As String = "<div style='height:170px;overflow:auto;'><table id='tableStudent' style='text-align:center;'>"
            'loop เพื่อ Gen นักเรียนทั้งหมดจาก ห้อง/ชั้น ที่เลือกมา , เงือนไขการจบ loop คือวนจนครบทุกคน
            For index = 0 To dt.Rows.Count - 1
                strStudent &= " <tr><td stuId='" & dt.Rows(index)("Student_Id").ToString() & "' class='FortdStudent'>" & dt.Rows(index)("StudentName") & "</td></tr><tr>"
            Next
            strStudent &= "</table></div>"
            _DB = Nothing
            Return strStudent
        Else
            _DB = Nothing
            Return ""
        End If
    End Function

    ''' <summary>
    ''' เป็นการ save ข้อมูลแบบประกาศข่าวไปถึงห้อง (ไม่ได้เฉพาะเจาะจงตัวนักเรียน)
    ''' </summary>
    ''' <param name="ClassName">ชั้นที่เลือก</param>
    ''' <param name="RoomName">ห้องที่เลือก</param>
    ''' <param name="txtNews">รายละเอียดข่าว</param>
    ''' <param name="StartDate">ประกาศตั้งแต่</param>
    ''' <param name="EndDate">ถึงวันที่</param>
    ''' <returns>"" ถ้าไม่ถูกต้อง,"Complete" ถ้าถูกต้อง</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveNewsRoom(ByVal ClassName As String, ByVal RoomName As String, ByVal txtNews As String, ByVal StartDate As String, ByVal EndDate As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If
        If txtNews.ToString().Trim() <> "" And txtNews IsNot Nothing Then
            Dim cultureInfo As New System.Globalization.CultureInfo("en-US")
            Dim dateTime As New DateTime
            dateTime = DateTime.Parse(StartDate)
            StartDate = dateTime.ToString("MM/dd/yyyy", cultureInfo)

            dateTime = DateTime.Parse(EndDate)
            EndDate = dateTime.ToString("MM/dd/yyyy", cultureInfo)

            Dim _DB As New ClassConnectSql()
            Dim KnSession As New KNAppSession()
            Try
                _DB.OpenWithTransection()
                If HttpContext.Current.Session("TNIDEditNews") = Nothing Then 'Insert ข่าว
                    Dim sql As String = " select NEWID() "
                    Dim TNID As String = _DB.ExecuteScalarWithTransection(sql)
                    Dim TNDID As String = _DB.ExecuteScalarWithTransection(sql)
                    'Insert tblTeacherNews
                    sql = " INSERT INTO dbo.tblTeacherNews " &
                          " ( TN_Id ,School_Id ,Teacher_Id ,Description ,StartDate ,EndDate ,LastUpdate ,IsActive ,Calendar_Id,School_Code) " &
                          " VALUES  ( '" & TNID & "','" & HttpContext.Current.Session("SchoolCode") & "','" & HttpContext.Current.Session("UserId").ToString() & "'," &
                          " '" & _DB.CleanString(txtNews.Trim()) & "','" & StartDate & "','" & EndDate & "',dbo.GetThaiDate(),1 ,'" & KnSession("CurrentCalendarId").ToString() & "','" & HttpContext.Current.Session("SchoolCode") & "') "
                    _DB.ExecuteWithTransection(sql)
                    'Insert tblTeacherNewsDeatail
                    sql = " SELECT Room_Id FROM dbo.t360_tblRoom WHERE Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName & "' " &
                          " AND School_Code = '" & HttpContext.Current.Session("SchoolCode") & "' AND Room_IsActive = 1 "
                    Dim RoomId As String = _DB.ExecuteScalarWithTransection(sql)
                    If RoomId <> "" Then
                        sql = " INSERT INTO dbo.tblTeacherNewsDetail( TND_Id ,TN_Id ,Class_Name ,Room_Id ,Student_Id ,LastUpdate ,IsActive,School_Code) " &
                              " VALUES  ( '" & TNDID & "','" & TNID & "' , NULL ,'" & RoomId & "',NULL,dbo.GetThaiDate(),1,'" & HttpContext.Current.Session("SchoolCode") & "') "
                        _DB.ExecuteWithTransection(sql)
                        sql = " INSERT INTO dbo.tblTeacherNewsDetailCompletion(TNDC_Id ,TND_Id,Student_Id,LastUpdate ,IsActive,School_Code ) " &
                                                " SELECT NEWID(),'" & TNDID & "',Student_Id,dbo.GetThaiDate(),1,'" & HttpContext.Current.Session("SchoolCode") & "' FROM dbo.t360_tblStudent " &
                                                " WHERE Student_CurrentClass = '" & ClassName & "' AND Student_CurrentRoom = '" & RoomName & "' AND dbo.t360_tblStudent.Student_IsActive = 1 " &
                                                " AND School_Code = '" & HttpContext.Current.Session("SchoolCode") & "' "
                        _DB.ExecuteWithTransection(sql)
                    Else
                        _DB.RollbackTransection()
                        Return ""
                    End If
                    'Insert tblTeachernewsDetailCompletion

                Else 'Update ข่าว
                    'Update tblTeacherNews
                    Dim sql As String = " UPDATE dbo.tblTeacherNews SET Description = '" & _DB.CleanString(txtNews.Trim()) & "' , StartDate = '" & StartDate & "' ,EndDate = '" & EndDate & "' " &
                                        " ,LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "' "
                    _DB.ExecuteWithTransection(sql)
                    sql = " SELECT Room_Id FROM dbo.t360_tblRoom WHERE Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName & "' " &
                         " AND School_Code = '" & HttpContext.Current.Session("SchoolCode") & "' AND Room_IsActive = 1 "
                    Dim RoomId As String = _DB.ExecuteScalarWithTransection(sql)
                    If RoomId <> "" Then
                        'Update tblTeacherNewsDetail
                        'Update Student_Id เป็น Null ด้วย เผื่อตอนแรกประกาศให้นักเรียนคนเดียว แล้วเข้ามาแก้ไขเป็นประกาศให้ทั้งห้องแทน
                        sql = " UPDATE dbo.tblTeacherNewsDetail SET Student_Id = NULL, Room_Id = '" & RoomId & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "' "
                        _DB.ExecuteWithTransection(sql)

                        'Update isActive ของเดิมเป็น 0 ก่อน แล้วค่อย Insert ใหม่
                        sql = " Update tblTeacherNewsDetailCompletion set IsActive = '0',lastupdate = dbo.GetThaiDate(),ClientId = null where TND_Id in(select TND_Id from tblTeacherNewsDetail where TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "')"
                        _DB.ExecuteWithTransection(sql)
                        sql = " insert into tblTeacherNewsDetailCompletion " &
                              " select newid(),TND_Id,t360_tblstudent.Student_id,'0',dbo.GetThaiDate(),'1',null,t360_tblstudent.School_Code,'0' from t360_tblStudent " &
                              " inner join tblTeacherNewsDetail on t360_tblStudent.Student_CurrentRoomId = tblTeacherNewsDetail.Room_Id where " &
                              " Student_IsActive = '1' and tblTeacherNewsDetail.TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "'"
                        _DB.ExecuteWithTransection(sql)
                    Else
                        _DB.RollbackTransection()
                    End If
                End If
                _DB.CommitTransection()
                Return "Complete"
            Catch ex As Exception
                _DB.RollbackTransection()
                HttpContext.Current.Session("TNIDEditNews") = Nothing
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' เป็นการ save ข้อมูลแบบประกาศข่าวไปถึงตัวนักเรียนเป็นรายคนไป
    ''' </summary>
    ''' <param name="StudentId">นักเรียนที่เลือกมา</param>
    ''' <param name="txtNews">รายละเอียดข่าว</param>
    ''' <param name="StartDate">ประกาศตั้งแต่</param>
    ''' <param name="EndDate">ถึงวันที่</param>
    ''' <returns>"" ถ้าไม่ถูกต้อง,"Complete" ถ้าถูกต้อง</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveNewsStudent(ByVal StudentId As String, ByVal txtNews As String, ByVal StartDate As String, ByVal EndDate As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If

        If txtNews.ToString().Trim() <> "" And txtNews IsNot Nothing Then
            Dim cultureInfo As New System.Globalization.CultureInfo("en-US")
            Dim dateTime As New DateTime
            dateTime = DateTime.Parse(StartDate)
            StartDate = dateTime.ToString("MM/dd/yyyy", cultureInfo)

            dateTime = DateTime.Parse(EndDate)
            EndDate = dateTime.ToString("MM/dd/yyyy", cultureInfo)


            Dim _DB As New ClassConnectSql()
            Dim KnSession As New KNAppSession()
            Try
                _DB.OpenWithTransection()
                If HttpContext.Current.Session("TNIDEditNews") = Nothing Then 'Insert ข่าว
                    Dim sql As String = " select NEWID() "
                    Dim TNID As String = _DB.ExecuteScalarWithTransection(sql)
                    Dim TNDID As String = _DB.ExecuteScalarWithTransection(sql)
                    'Insert tblTeacherNews
                    sql = " INSERT INTO dbo.tblTeacherNews " &
                          " ( TN_Id ,School_Id ,Teacher_Id ,Description ,StartDate ,EndDate ,LastUpdate ,IsActive ,Calendar_Id,School_Code) " &
                          " VALUES  ( '" & TNID & "','" & HttpContext.Current.Session("SchoolCode") & "','" & HttpContext.Current.Session("UserId").ToString() & "'," &
                          " '" & _DB.CleanString(txtNews.Trim()) & "','" & StartDate & "','" & EndDate & "',dbo.GetThaiDate(),1 ,'" & KnSession("CurrentCalendarId").ToString() & "','" & HttpContext.Current.Session("SchoolCode") & "') "
                    _DB.ExecuteWithTransection(sql)
                    'Insert tblTeacherNewsDeatail
                    sql = " INSERT INTO dbo.tblTeacherNewsDetail( TND_Id ,TN_Id ,Class_Name ,Room_Id ,Student_Id ,LastUpdate ,IsActive,School_Code) " &
                      " VALUES  ( '" & TNDID & "','" & TNID & "' , NULL ,NULL,'" & StudentId & "',dbo.GetThaiDate(),1,'" & HttpContext.Current.Session("SchoolCode") & "') "
                    _DB.ExecuteWithTransection(sql)
                    'Insert tblTeacherNewsDetailCompletion
                    sql = " INSERT INTO dbo.tblTeacherNewsDetailCompletion ( TNDC_Id ,TND_Id ,Student_Id ,LastUpdate ,IsActive ,ClientId,School_Code) " &
                          " VALUES  ( NEWID() ,'" & TNDID & "' ,'" & StudentId & "' , dbo.GetThaiDate() ,1 ,NULL,'" & HttpContext.Current.Session("SchoolCode") & "') "
                    _DB.ExecuteWithTransection(sql)
                Else 'Update ข่าว
                    Dim sql As String = " UPDATE dbo.tblTeacherNews SET Description = '" & _DB.CleanString(txtNews.Trim()) & "' , StartDate = '" & StartDate & "' ,EndDate = '" & EndDate & "' " &
                                        " ,LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "' "
                    _DB.ExecuteWithTransection(sql)
                    'Update Room_Id = Null ด้วย เผื่อกรณีประกาศถึงทั้งห้องก่อน แล้วเข้ามาแก้เป็นประกาศถึงนักเรียนคนเดียว
                    sql = " UPDATE dbo.tblTeacherNewsDetail SET Room_Id = NULL, Student_Id = '" & StudentId & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "' "
                    _DB.ExecuteWithTransection(sql)
                    'Update isActive ของเดิมเป็น 0 ก่อน แล้วค่อย Insert ใหม่
                    sql = " Update tblTeacherNewsDetailCompletion set IsActive = '0',lastupdate = dbo.GetThaiDate(),ClientId = null where TND_Id in(select TND_Id from tblTeacherNewsDetail where TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "')"
                    _DB.ExecuteWithTransection(sql)
                    sql = " insert into tblTeacherNewsDetailCompletion " &
                          " select newid(),TND_Id,'" & StudentId & "','0',dbo.GetThaiDate(),'1',null,'" & HttpContext.Current.Session("SchoolCode") & "','0' from tblTeacherNewsDetail " &
                          " where tblTeacherNewsDetail.TN_Id = '" & HttpContext.Current.Session("TNIDEditNews") & "'"
                    _DB.ExecuteWithTransection(sql)
                End If
                _DB.CommitTransection()
                Return "Complete"
            Catch ex As Exception
                _DB.RollbackTransection()
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' เป็นการดูข่าวจากที่ครูประกาศ โดยแสดงบน tablet เด็ก
    ''' </summary>   
    ''' <remarks></remarks>
    ''' 
    Private Sub BtnTeacherNews_Click(sender As Object, e As EventArgs) Handles BtnTeacherNews.Click
        Dim dtStudent As DataTable = GetDtStudent()
        If dtStudent.Rows.Count > 0 Then
            Dim dt As DataTable = GetTeacherNews(dtStudent)
            If dt.Rows.Count > 0 Then
                SetIconNew(dt, True)
                Repeater1.DataSource = dt
            Else
                Repeater1.DataSource = Nothing
            End If
            Repeater1.DataBind()
            UpdateStudentIsSeen(dtStudent.Rows(0)("Student_Id").ToString, True)
        End If
    End Sub
    ''' <summary>
    ''' ใส่รูป Icon new ให้กับข่าวที่ยังไม่เคยเห็น
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub SetIconNew(ByRef dt As DataTable, IsStudent As Boolean)
        If IsStudent Then
            For Each Eachnews In dt.Rows
                If Eachnews("StudentIsSeen") = "0" Then
                    Eachnews("News_Announcer") = "<img src=""../Images/NewIcon.png"" class=""NewIcon"">" & Eachnews("News_Announcer")
                End If
            Next
        Else
            For Each Eachnews In dt.Rows
                If Eachnews("TeacherIsSeen") = "0" Then
                    Eachnews("News_Announcer") = "<img src=""../Images/NewIcon.png"" class=""NewIcon"">" & Eachnews("News_Announcer")
                End If
            Next
        End If

    End Sub
    ''' <summary>
    ''' ปรับสถานะให้เป็นนักเรียนเห็นข่าวประกาศแล้ว
    ''' </summary>
    ''' <param name="StudentId">Id นักเรียน</param>
    ''' <remarks></remarks>
    Public Sub UpdateStudentIsSeen(StudentId As String, IsTeacherNews As Boolean)
        Dim Sql As String
        If IsTeacherNews Then
            Sql = " Update tblTeacherNewsDetailCompletion set StudentIsSeen = '1' where Student_Id = '" & StudentId & "' and StudentIsSeen = '0'"
        Else
            Sql = " Update t360_tblNewsDetailCompletion set StudentIsSeen = '1' where User_Id = '" & StudentId & "' and StudentIsSeen = '0'"
        End If

        _DB.Execute(Sql)
    End Sub

    Public Sub UpdateTeacherIsSeen(TeacherId As String)
        Dim Sql As String

        Sql = " Update t360_tblNewsDetailCompletion set TeacherIsSeen = '1' where User_Id = '" & TeacherId & "' and TeacherIsSeen = '0'"

        _DB.Execute(Sql)
    End Sub


    ''' <summary>
    ''' หาข่าวทั้งหมดจากที่ครูประกาศผ่าน PP โดยดูจากชั้น ห้อง และรหัสนักเรียน
    ''' </summary>
    ''' <param name="dtStudent">นักเรียนที่เลือกมา</param>   
    ''' <returns>datatable</returns>
    ''' <remarks></remarks>
    Private Function GetTeacherNews(dtStudent As DataTable) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT tn.Description as News_Information ,t.Teacher_FirstName as News_Announcer,StudentIsSeen,")
        sql.Append(" CAST(DATEPART(DAY,tn.StartDate) AS VARCHAR(2)) + '/' + CAST(DATEPART(MONTH,tn.StartDate) AS VARCHAR(2)) + '/' + ")
        sql.Append(" CAST((DATEPART(YEAR,tn.StartDate) + 543) AS VARCHAR(4)) + ' - ' +  CAST(DATEPART(DAY,tn.EndDate) AS VARCHAR(2)) + '/' + ")
        sql.Append(" CAST(DATEPART(MONTH,tn.EndDate) AS VARCHAR(2)) + '/' +  CAST(DATEPART(YEAR,tn.EndDate) + 543 AS VARCHAR(4)) AS DateNews ")
        sql.Append(" FROM tblTeacherNews tn INNER JOIN tblTeacherNewsDetail tnd ON tn.TN_Id = tnd.TN_Id ")
        sql.Append(" INNER JOIN t360_tblTeacher t ON tn.Teacher_Id = t.Teacher_id ")
        sql.Append("  inner join tblTeacherNewsDetailCompletion tndc on tnd.TND_Id = tndc.TND_Id")
        sql.Append(" WHERE (tn.IsActive = 1 AND tnd.IsActive = 1) AND (dbo.GetThaiDate() BETWEEN tn.StartDate AND tn.EndDate) ")
        sql.Append(" AND (tnd.Student_Id = '")
        sql.Append(dtStudent.Rows(0)("Student_Id"))
        sql.Append("' OR tnd.Room_Id = '") ' dt.Rows(0)("CurrentClass")
        sql.Append(dtStudent.Rows(0)("Student_CurrentRoomId"))
        sql.Append("') AND tndc.Student_Id = '")
        sql.Append(dtStudent.Rows(0)("Student_Id"))
        sql.Append("' AND tn.School_Id = '")
        sql.Append(HttpContext.Current.Session("SchoolCode"))
        sql.Append("' ORDER BY tn.StartDate,tn.LastUpdate desc;")
        Return _DB.getdata(sql.ToString())
    End Function
End Class