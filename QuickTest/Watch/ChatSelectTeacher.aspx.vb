Imports System.Web
Public Class ChatSelectTeacher
    Inherits System.Web.UI.Page
    'เก็บ ChatRoomId ใช้ทั้งหน้า
    Public ChatRoomId As String
    'ตัวแปรใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'Class ที่ทำงานเกี่ยวกับข้อมูล Chat มีใช้งานหลายจุด
    Dim ClsChat As New ClassChat(New ClassConnectSql())
    'ตัวแปรที่เอาไว้ check ว่าผู้ปกครองมีลูกมากกว่า 1 คน ต้องแสดงปุ่มกลับขึ้นมา เพื่อให้กดกลับไปเลือกข้อมูลของนักเรียนคนอื่นได้
    Public IsMoreThanOne As Boolean

    Public DeviceId As String

    ''' <summary>
    ''' ทำการหาข้อมูลว่านักเรียนคนนี้มีครูประจำชั้นหลายคนรึเปล่า ถ้ามีหลายคนก็ให้สร้าง Div ครูก่อน แต่ถ้าไม่มีก็ให้ไปหน้า chat ได้เลย
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("DeviceId") IsNot Nothing Then
            DeviceId = Request.QueryString("DeviceId").ToString()
        End If

        Dim StudentId As String = ""
        If Request.QueryString("StudentId") Is Nothing Then
            Response.Write("ไม่มี StudentId")
            Exit Sub
        Else
            StudentId = Request.QueryString("StudentId").ToString()
        End If

        If Request.QueryString("MorethanOne") IsNot Nothing Then
            IsMoreThanOne = Request.QueryString("MorethanOne")
        End If

        If IsMoreThanOne = False Then
            btnBack.Visible = False
        Else
            btnBack.Visible = True
        End If

        If HttpContext.Current.Session("SchoolID") Is Nothing OrElse HttpContext.Current.Session("SchoolID").ToString = "" Then
            HttpContext.Current.Session("SchoolID") = ChatSelectStudent.GetSchoolIdByDeviceId(Request.QueryString("DeviceId").ToString())
        End If

        Dim CalendarId As String = GetCalendarID(HttpContext.Current.Session("SchoolID").ToString())
        If CalendarId = "" Then
            Response.Write("หา CalendarId ไม่ได้")
            Exit Sub
        End If

        If HttpContext.Current.Session("Owner_Id") Is Nothing OrElse HttpContext.Current.Session("Owner_Id").ToString = "" Then
            Dim parentId As String = ChatSelectStudent.GetParentIdByDeviceId(Request.QueryString("DeviceId").ToString())
            If parentId = "" Then
                Response.Write("หา parentId ไม่เจอ")
                Exit Sub
            End If
            HttpContext.Current.Session("Owner_Id") = parentId
        End If

        ' สำหรับเวลา tablet ผู้ปกครอง rotate ในหน้า chat มันจะโหลดหน้านี้ขึ้นมาใหม่
        If HttpContext.Current.Session("ChatRoom_Id") IsNot Nothing Then
            Response.Redirect("~/Watch/Chat.aspx?ChatRoom_Id=" & HttpContext.Current.Session("ChatRoom_Id").ToString())
        End If

        Dim dtCheckManyTeacher As DataTable = CheckIsManyTeacher(StudentId, CalendarId)
        'ถ้ามีครูประจำชั้นมากกว่า 1 คน ต้องทำการสร้าง Div ครู เพื่อให้เลือก
        If dtCheckManyTeacher.Rows.Count > 1 Then
            CreateDivSelectTeacher(dtCheckManyTeacher)
        Else
            ' check ก่อนว่า มีการคุยกันในเทอมปัจจุบันหรือยัง โดยดูจากการที่ครูสั่งการบ้านให้นักเรียน
            If dtCheckManyTeacher.Rows.Count = 0 Then
                Response.Redirect("~/Watch/NeverTalk.aspx?DeviceId=" & Request.QueryString("DeviceId").ToString())
            End If

            'ทำการหา ChatRoomId ก่อนว่ามีรึเปล่า
            Dim ChatRoomId As String = ClsChat.GetChatRoomId(HttpContext.Current.Session("Owner_Id").ToString(), dtCheckManyTeacher.Rows(0)("Create_By").ToString())
            If ChatRoomId = "" Then
                'ทำการสร้างห้อง Chat ใหม่
                ChatRoomId = ClsChat.CreateChatRoom(HttpContext.Current.Session("Owner_Id").ToString(), dtCheckManyTeacher.Rows(0)("Create_By").ToString())
                If ChatRoomId = "" Then
                    Response.Write("ไม่มี ChatRoomId สร้างห้อง Chat ไม่ได้")
                    Exit Sub
                Else
                    Response.Redirect("~/Watch/Chat.aspx?ChatRoom_Id=" & ChatRoomId)
                End If
            Else
                'ถ้ามีข้อมูลเก่าอยู่แล้วก็ให้ทำการ Redirect ไปหน้า Chat ได้เลย
                Response.Redirect("~/Watch/Chat.aspx?ChatRoom_Id=" & ChatRoomId)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Function สร้าง Div ที่ให้เลือก อาจารย์
    ''' </summary>
    ''' <param name="dt">Datatable ข้อมูลครู</param>
    ''' <remarks></remarks>
    Private Sub CreateDivSelectTeacher(ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            'loop เพื่อต่อสตริง สร้าง Div ครู , เงื่อนไขการจบ loop คือ วนจนครบทุกคน
            For index = 0 To dt.Rows.Count - 1
                Dim CountLoop As Integer = index + 1
                Dim TeacherName As String = GetTeacherNameByTeacherId(dt.Rows(index)("Create_By").ToString())
                Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & dt.Rows(index)("Create_By").ToString() & "}/Id.png")
                Dim OwnerImg As String = ""
                If System.IO.File.Exists(OwnerPath) = True Then
                    OwnerImg = "../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & dt.Rows(index)("Create_By").ToString() & "}/Id.png"
                Else
                    OwnerImg = "../Images/default-profile-image.png"
                End If
                sb.Append("<div id='DivCover" & CountLoop & "' class='DivWidth DivCover' onclick=""TeacherClick('" & dt.Rows(index)("Create_By").ToString() & "');"">")
                sb.Append("<div id='Divpicture" & CountLoop & "' class='DivWidth DivPicture 'style='background-image:url(""" & OwnerImg & """);' >")
                sb.Append("</div><div id='DivName" & CountLoop & "' class='DivWidth DivName' >")
                sb.Append(TeacherName)
                sb.Append("</div></div>")
            Next
            DivSelectTeacher.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' Function เช็คว่ามีครูมากกว่า 1 คนหรือเปล่า
    ''' </summary>
    ''' <param name="StudentId">Id ของนักเรียน</param>
    ''' <param name="CalendarId">ปีการศึกษา</param>
    ''' <returns>Datatable:ข้อมูลครูประจำชั้น</returns>
    ''' <remarks></remarks>
    Private Function CheckIsManyTeacher(ByVal StudentId As String, ByVal CalendarId As String) As DataTable
        Dim sql As String = " SELECT tblModule.Create_By FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
                            " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN " & _
                            " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id AND tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id " & _
                            " WHERE (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') AND " & _
                            " (tblModuleDetail.Reference_Type = 0) " & _
                            " UNION " & _
                            " Select User_Id FROM tblQuiz INNER JOIN dbo.tblQuizScore ON dbo.tblQuiz.Quiz_Id = dbo.tblQuizScore.Quiz_Id " & _
                            " WHERE (Calendar_Id = '" & CalendarId & "') AND dbo.tblQuizScore.Student_Id = '" & StudentId & "' AND (IsQuizMode = 1)"
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' Function หาชื่อ อาจารย์
    ''' </summary>
    ''' <param name="TeacherId">Id ของครู</param>
    ''' <returns>String:ชื่อครู</returns>
    ''' <remarks></remarks>
    Private Function GetTeacherNameByTeacherId(ByVal TeacherId As String) As String
        Dim sql As String = " select Teacher_FirstName + ' ' + Teacher_LastName from t360_tblTeacher where Teacher_id = '" & TeacherId & "' "
        Dim Teachername As String = _DB.ExecuteScalar(sql)
        Return Teachername
    End Function

    ''' <summary>
    ''' เมื่อกดเลือกครูต้องทำการหาข้อมูลเก่าก่อนว่าเคยมีข้อมูลอยู่หรือยัง ถ้ายังไม่มีต้องสร้างห้อง Chat ใหม่
    ''' </summary>
    ''' <param name="TeacherId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function ClickTeacher(ByVal TeacherId As String)
        Dim ClsChat As New ClassChat(New ClassConnectSql())
        Dim ChatRoomId As String = ClsChat.GetChatRoomId(HttpContext.Current.Session("Owner_Id").ToString(), TeacherId.ToString())
        If ChatRoomId = "" Then
            ChatRoomId = ClsChat.CreateChatRoom(HttpContext.Current.Session("Owner_Id").ToString(), TeacherId.ToString())
            If ChatRoomId = "" Then
                Return ""
            Else
                Return ChatRoomId
            End If
        Else
            Return ChatRoomId
        End If
    End Function

    ''' <summary>
    ''' get calendar from date now หาปีการศึกษาปัจจุบัน
    ''' </summary>
    ''' <param name="SchoolID">รหัสโรงเรียน</param>
    ''' <returns>String:CalendarId</returns>
    ''' <remarks></remarks>
    Private Function GetCalendarID(ByVal SchoolID As String) As String
        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql)
        Return CalendarId
    End Function

End Class