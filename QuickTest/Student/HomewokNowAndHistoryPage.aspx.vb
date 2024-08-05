Imports System.Web
Public Class HomewokNowAndHistoryPage
    Inherits System.Web.UI.Page
    'ตัวแปรที่ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'ตัวแปรที่บอกว่าตอนนี้เลือกดูข้อมูลที่อยู่ในช่วงเวลา ปัจจุบัน หรือ ประวัติการบ้าน
    Dim IsCurrent As Boolean
    'ตัวแปรที่เอาไว้เก็บข้อมูล เป็นการเก็บแบบ Application ซึ่งทาง KN สร้าง Class ขึ้นมา Wrap ให้ใช้ง่าย
    Dim KnSession As New KNAppSession()
    'เก็บ ชั้น/ห้อง เอาไว้ใช้ตอน PostBack
    Public Property ClassName As String
        Get
            ClassName = ViewState("_ClassName")
        End Get
        Set(ByVal value As String)
            ViewState("_ClassName") = value
        End Set
    End Property

    'เก็บชื่อห้องเอาไว้ใช้กับฝั่ง Javascript
    Public JSClassName As String
    'เก็บ state ว่าตอนนี้เลือกข้อมูลปัจจุบัน หรือ ประวัติ เอาไว้ใช้กับฝั่ง Javascript
    Public JSIsCurrent As Boolean
    'เก็บ String HTML ของ Panel เลือกห้อง/ชั้น ทางซ้ายมือ
    Public htmlClass As String

    Public IE As String

    ''' <summary>
    ''' ทำการสร้าง Div การบ้านขึ้นมาในกรณีที่มีการเลือกห้องมาแล้ว แต่ถ้ายังไม่ได้เลือกห้องก็จะทำการสร้าง div ที่ไม่มีข้อมูลขึ้นมาก่อน , สร้าง Panel เลือกห้อง/ชั้น ทางซ้ายมือ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        End If

        If Not Page.IsPostBack Then
#If IE = "1" Then
            IsCurrent = False
            JSIsCirrent = False
            Dim CalendarId As String = knSession("SelectedCalendarId").ToString()
            ClassName = "ป.3/1"
            JSClassName = ClassName
            Dim dt As DataTable = GetDtInfoByClass(ClassName, IsCurrent, CalendarId)
            If dt.Rows.Count > 0 Then
                CreateDivHomework(dt)
            End If
            IsSelected.Value = "true"
            hdKeepModeRoom.Value = True
            ModeClassName.InnerHtml = "ห้องทั้งหมด"
            htmlClass = SetMenuClassRoom(True)
#Else
            If Request.QueryString("IsCurrent") Is Nothing Then
                IsCurrent = True
            Else
                IsCurrent = Request.QueryString("IsCurrent")
            End If

            Dim CalendarId As String = KnSession("SelectedCalendarId").ToString()
            Dim CurrentCalendarId As String = KnSession("CurrentCalendarId").ToString()
            If CalendarId <> CurrentCalendarId Then
                IsCurrent = False
                BtnCurrent.Visible = False
            End If
            JSIsCurrent = IsCurrent
            If Request.QueryString("ClassName") IsNot Nothing Then
                ClassName = Request.QueryString("ClassName")
                JSClassName = ClassName
                Dim dt As DataTable = GetDtInfoByClass(ClassName, IsCurrent, CalendarId)
                If dt.Rows.Count > 0 Then
                    CreateDivHomework(dt)
                End If
                IsSelected.Value = "true"
                hdKeepModeRoom.Value = True
                ModeClassName.InnerHtml = "ห้องทั้งหมด"
                htmlClass = SetMenuClassRoom(True)
            Else
                CreateDivNoData()
                IsSelected.Value = "false"
                hdKeepModeRoom.Value = True
                ModeClassName.InnerHtml = "ห้องทั้งหมด"
                htmlClass = SetMenuClassRoom(True)
                Exit Sub
            End If
#End If
        End If

    End Sub

#Region "Function & Query"

    ''' <summary>
    ''' ทำการสร้าง div ของการบ้านที่สั่งไปแต่ละอัน
    ''' </summary>
    ''' <param name="dt">ข้อมูลการบ้านของ ห้อง/ชั้น ที่เลือกมา</param>
    ''' <remarks></remarks>
    Private Sub CreateDivHomework(ByVal dt As DataTable)

        Dim sb As New StringBuilder
        If dt.Rows.Count > 0 Then
            'Dim Counter As Integer = 0
            'ชื่อครูผู้สั่งการบ้าน
            Dim TeacherName As String = ""
            'กำหนดส่ง
            Dim EndTime As String = ""
            'ชื่อการบ้าน
            Dim HomeworkName As String = ""
            'ModuleAssignmentId
            Dim MaId As String = ""
            'จำนวนนักเรียนทั้งหมดที่ต้องทำการบ้านชุดนี้
            Dim StudentRelated As Integer = 0
            'จำนวนนักเรียนที่ยังไม่ส่งการบ้าน
            Dim StudentNotSendHomwork As Integer = 0
            'จำนวนนักเรียนที่ยังทำการบ้านไม่เสร็จ
            Dim StudentNotComplete As Integer = 0
            'ข้อความที่จะไปขึ้นใน Div เล็กทางขวาบนของ div การบ้าน
            Dim txtSmallDiv As String = ""
            'ทำการวน loop การบ้านแต่ละอัน เพื่อสร้าง Div การบ้านขึ้นมา , เงื่อนไขการจบ loop คือ วนจนครบทุกการบ้าน
            For index = 0 To dt.Rows.Count - 1
                'Counter = index + 1
                TeacherName = dt.Rows(index)("TeacherName")
                EndTime = dt.Rows(index)("End_Date").ToString().Substring(0, dt.Rows(index)("End_Date").ToString().Length - 3)
                HomeworkName = dt.Rows(index)("TestSet_Name").ToString()
                'ในกรณีที่ชื่อการบ้านยาวเกิน 50 ตัวอักษรต้องทำการตัดให้เหลือแต่ 50 ตัว และเติม '...' เข้าไปแทน
                If HomeworkName.Length > 50 Then
                    HomeworkName = HomeworkName.Substring(0, 50) & "..."
                End If
                MaId = dt.Rows(index)("MA_Id").ToString()
                StudentRelated = dt.Rows(index)("StudentRelated").ToString()
                StudentNotSendHomwork = dt.Rows(index)("StudentNotSend").ToString()
                StudentNotComplete = dt.Rows(index)("StudentNotComplete").ToString()
                'หาจำนวนคนที่ส่งการบ้าน / นักเรียนทั้งหมด
                txtSmallDiv = (StudentRelated - StudentNotSendHomwork) & "/" & StudentRelated

                sb.Append("<div id='DivHomework' class='ForDivWidth ForDivHomework'>")
                sb.Append("<div id='DivTop' class='ForDivTop'>")
                sb.Append("<span class='ForSpanTeacherName' MAID='" & MaId & "' >" & HomeworkName & "</span>")
                sb.Append("<span class='ForSpanEndTime'>" & TeacherName & "</span>")
                sb.Append("<img  class='ImgShowInfo' MAID='" & MaId & "' src='../Images/homework/ShowHomeworkDetail.png' />")
                sb.Append("<div id='RightPanel' class='ForDivRightPanel'>")
                sb.Append("<div id='DivStudentRelateHomework' class='TxtSmallDiv ForSmallDivRightPanel' >" & txtSmallDiv & "</div>")

                'sb.Append("<div id='DivStudentRelateHomework' class='SR ForSmallDivRightPanel' >" & StudentRelated & "</div>")
                'sb.Append("<div id='DivStudentNotSend' class='SN ForSmallDivRightPanel' >" & StudentNotSendHomwork & "</div>")
                'sb.Append("<div id='DivResultStudentNotComplete' class='RS ForSmallDivRightPanel' >" & StudentNotComplete & "</div>")

                sb.Append("</div></div>")
                sb.Append("<div id='DivBottom' class='ForDivBottom'>")
                sb.Append("<span class='ForSpanHomeworkName'>ส่ง " & EndTime & "</span>")
                'sb.Append("<img class='ImgEdit' onclick=""EditHomework('" & MaId & "')"" src='../Images/homework/pencil.png' />")
                sb.Append("<img class='ImgEdit' maid='" & MaId & "' src='../Images/homework/pencil.png' />")
                sb.Append("</div></div>")
            Next
            MainDivHomework.Attributes.Add("style", "width: 800px;height: 400px; overflow: auto; margin-left: auto; margin-right: auto; padding: 10px;border: 1px dashed #FFA032; border-radius: 15px; margin-top: 15px;")
            MainDivHomework.InnerHtml = sb.ToString()
        End If

    End Sub

    ''' <summary>
    ''' ทำการสร้าง Div ที่ไม่มีข้อมูล ในกรณีที่ไม่ได้เลือกห้อง
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateDivNoData()

        BtnCurrent.Visible = False
        BtnHistory.Visible = False
        Dim sb As New StringBuilder
        sb.Append("<div id='DivNoData' class='ForMainDivNoData' >")
        sb.Append("<div id='DivShowInfo' class='ForDivShowInFo'>")
        sb.Append("<span style='font-size: 25px;font-weight: bold;color: rgb(122, 119, 119);'>เลือกห้องก่อนค่ะ</span>")
        sb.Append("</div></div>")
        MainDivHomework.Attributes.Add("style", "width: 800px;height: 456px; overflow: auto; margin-left: auto; margin-right: auto; padding: 10px;border: 1px dashed #FFA032; border-radius: 15px; margin-top: 15px;")
        MainDivHomework.InnerHtml = sb.ToString()

    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลการบ้านจาก ห้อง/ชั้น ที่เลือกมา เพื่อนำ dt นี้ไปสร้าง div การบ้านต่อไป
    ''' </summary>
    ''' <param name="ClassAndRoomName">ชั้น/ห้อง ที่เลือกมา</param>
    ''' <param name="IsCurrent">ตัวแปรที่บอกว่าเลือกข้อมูลที่เป็นปัจจุบัน หรือ ประวัติ</param>
    ''' <param name="CalendarId">ปีการศึกษาที่เลือกมา</param>
    ''' <returns>dt การบ้าน</returns>
    ''' <remarks></remarks>
    Private Function GetDtInfoByClass(ByVal ClassAndRoomName As String, ByVal IsCurrent As Boolean, ByVal CalendarId As String) As DataTable

        'ทำการ split str ห้อง,ชั้น ออกจากกัน
        Dim SplitStr = ClassAndRoomName.Split("/")
        Dim ClassName As String = SplitStr(0)
        Dim RoomName As String = "/" & SplitStr(1)
        Dim sql As String = " SELECT t360_tblTeacher.Teacher_FirstName + ' ' + t360_tblTeacher.Teacher_LastName AS TeacherName, tblModuleAssignment.End_Date, " & _
                            " COUNT(DISTINCT tblModuleDetailCompletion.Student_Id) AS StudentRelated, tblTestSet.TestSet_Name, tblModule.Module_Id, " & _
                            " COUNT(DISTINCT CASE WHEN tblModuleDetailCompletion.TimeExitedByUser IS NULL THEN dbo.tblModuleDetailCompletion.Student_Id ELSE NULL END) " & _
                            " AS StudentNotSend, COUNT(DISTINCT CASE WHEN tblModuleDetailCompletion.Module_Status <> 2 THEN dbo.tblModuleDetailCompletion.Student_Id ELSE NULL END) " & _
                            " AS StudentNotComplete, tblModuleAssignment.MA_Id FROM tblModuleAssignment INNER JOIN " & _
                            " tblModule ON tblModuleAssignment.Module_Id = tblModule.Module_Id INNER JOIN  tblModuleDetail ON " & _
                            " tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN tblModuleDetailCompletion ON " & _
                            " tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id AND " & _
                            " tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN " & _
                            " tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id INNER JOIN " & _
                            " tblTestSet ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id INNER JOIN " & _
                            " t360_tblTeacher ON tblAssistant.Assistant_id = t360_tblTeacher.Teacher_id INNER JOIN " & _
                            " t360_tblStudentRoom ON tblModuleDetailCompletion.Student_Id = t360_tblStudentRoom.Student_Id " & _
                            " WHERE (t360_tblStudentRoom.Class_Name = '" & ClassName & "') AND (t360_tblStudentRoom.Room_Name = '" & RoomName & "') " & _
                            " AND (t360_tblStudentRoom.SR_IsActive = 1)  AND (tblModuleDetailCompletion.IsActive = 1)" & _
                            "  AND (tblAssistant.Assistant_id = '" & Session("UserId").ToString() & "') "
        'ถ้าดูข้อมูลในปัจจุบัน ต้องหาการบ้านที่ยังอยู่ในช่วง เริ่มต้น , กำหนดส่ง
        If IsCurrent = True Then
            sql &= " AND (dbo.GetThaiDate() < tblModuleAssignment.End_Date) AND (dbo.GetThaiDate() > tblModuleAssignment.Start_Date)  "
        Else
            'ถ้าหาข้อมูลประวัติ ก็หาที่อยู่ในช่วงที่ >= กำหนดส่งไปแล้ว
            sql &= " AND (dbo.GetThaiDate() >= End_Date AND (tblModuleAssignment.Calendar_Id = '" & CalendarId & "')) "
        End If
        sql &= " GROUP BY t360_tblTeacher.Teacher_FirstName, t360_tblTeacher.Teacher_LastName, tblModuleAssignment.End_Date, tblTestSet.TestSet_Name, " & _
               " tblModule.Module_Id , dbo.tblModuleAssignment.MA_Id " & _
               "  ORDER BY tblModuleAssignment.End_Date asc, COUNT(DISTINCT tblModuleDetailCompletion.Student_Id) desc," & _
                " COUNT(DISTINCT CASE WHEN tblModuleDetailCompletion.TimeExitedByUser IS NULL THEN dbo.tblModuleDetailCompletion.Student_Id ELSE NULL END) desc; "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    ''' <summary>
    ''' หา ห้อง/ชั้น เมื่อกดสลับโหมด ระหว่าง 'ห้องที่สอน','ห้องทั้งหมด'
    ''' </summary>
    ''' <param name="IsSelectedClassInSchool">ตัวแปรที่บอกว่าต้องการเลือกแบบห้องทั้งหมด</param>
    ''' <returns>String HTML ห้อง/ชั้น เพื่อที่จะได้นำไป Append ข้อมูลใน Panel เลือกห้อง ทางด้านซ้ายมือ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SetMenuClassRoom(ByVal IsSelectedClassInSchool As Boolean) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim dt As DataTable
        Dim ClsStudent As New Service.ClsStudent(New ClassConnectSql)
        If IsSelectedClassInSchool Then
            dt = ClsStudent.GetClassNameInSchool()
        Else
            dt = ClsStudent.GetClassNameHaveQuizOrHomework()
        End If

        Dim htmlClass As New StringBuilder()
        If Not dt.Rows.Count = 0 Then
            Dim start As Integer = dt.Rows(0)("ClassOrder")
            Dim startClass As String = dt.Rows(0)("ClassName").ToString().Substring(0, 3)
            htmlClass.Append("<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>")
            'loop เพื่อทำการต่อสตริงสร้าง html ของห้อง / ชั้น , เงือนไขการจบ loop คือวนจนครบทุกห้อง ทุกชั้น
            For Each row In dt.Rows
                If Not row("ClassOrder") = start Then
                    htmlClass.Append("</div>")
                    start = row("ClassOrder")
                    startClass = row("ClassName").ToString().Substring(0, 3)
                    htmlClass.Append("<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>")
                End If
                htmlClass.Append("<div class='menuAcdItem'>" & row("ClassName") & "</div>")
            Next
            htmlClass.Append("</div>")
        End If
        SetMenuClassRoom = htmlClass.ToString()
    End Function

    ''' <summary>
    ''' ทำการค้นหาการบ้านที่ครูกรอกเข้ามาทาง Panel ด้านซ้ายมือ
    ''' </summary>
    ''' <param name="txtSearchHomework">ข้อความที่ค้นหา</param>
    ''' <returns>String HTML เพื่อนำไป Append ทางฝั่ง Javascript เป็น Div การบ้านที่ Search</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SearchHomework(ByVal txtSearchHomework As String)

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim KnSession As New KNAppSession()
        Dim CalendarId As String = KnSession("SelectedCalendarId").ToString()
        Dim sb As New StringBuilder
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT t360_tblTeacher.Teacher_FirstName + ' ' + t360_tblTeacher.Teacher_LastName AS TeacherName, tblModuleAssignment.End_Date,  " & _
                            " COUNT(DISTINCT tblModuleDetailCompletion.Student_Id) AS StudentRelated, tblTestSet.TestSet_Name, tblModule.Module_Id,  " & _
                            " COUNT(DISTINCT CASE WHEN tblModuleDetailCompletion.TimeExitedByUser IS NULL THEN dbo.tblModuleDetailCompletion.Student_Id ELSE NULL END)  " & _
                            " AS StudentNotSend, COUNT(DISTINCT CASE WHEN tblModuleDetailCompletion.Module_Status <> 2 THEN dbo.tblModuleDetailCompletion.Student_Id ELSE NULL END) " & _
                            " AS StudentNotComplete, tblModuleAssignment.MA_Id FROM tblModuleAssignment INNER JOIN  tblModule ON tblModuleAssignment.Module_Id = tblModule.Module_Id " & _
                            " INNER JOIN  tblModuleDetail ON  tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN tblModuleDetailCompletion " & _
                            " ON  tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id AND  tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id " & _
                            " INNER JOIN  tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id INNER JOIN  tblTestSet ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id " & _
                            " INNER JOIN  t360_tblTeacher ON tblAssistant.Assistant_id = t360_tblTeacher.Teacher_id INNER JOIN  t360_tblStudentRoom " & _
                            " ON tblModuleDetailCompletion.Student_Id = t360_tblStudentRoom.Student_Id  WHERE (t360_tblStudentRoom.SR_IsActive = 1) AND  " & _
                            " (tblModuleDetailCompletion.IsActive = 1) AND (tblAssistant.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "')  " & _
                            " and (tblTestSet.TestSet_Name like '%" & _DB.CleanString(txtSearchHomework) & "%') AND (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') GROUP BY t360_tblTeacher.Teacher_FirstName, t360_tblTeacher.Teacher_LastName, " & _
                            " tblModuleAssignment.End_Date, tblTestSet.TestSet_Name,  tblModule.Module_Id , dbo.tblModuleAssignment.MA_Id  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            'Dim Counter As Integer = 0
            Dim TeacherName As String = ""
            Dim EndTime As String = ""
            Dim HomeworkName As String = ""
            Dim MaId As String = ""
            Dim StudentRelated As Integer = 0
            Dim StudentNotSendHomwork As Integer = 0
            Dim StudentNotComplete As Integer = 0
            Dim txtSmallDiv As String = ""
            For index = 0 To dt.Rows.Count - 1 'ทำการ loop การบ้านเพื่อสร้าง div การบ้านที่ค้นหามา , เงื่อนไขการจบ loop คือ วนจนครบหมดทุกการบ้าน 
                'Counter = index + 1
                TeacherName = dt.Rows(index)("TeacherName")
                EndTime = dt.Rows(index)("End_Date").ToString().Substring(0, dt.Rows(index)("End_Date").ToString().Length - 3)
                HomeworkName = dt.Rows(index)("TestSet_Name").ToString()
                If HomeworkName.Length > 50 Then
                    HomeworkName = HomeworkName.Substring(0, 50) & "..."
                End If
                MaId = dt.Rows(index)("MA_Id").ToString()
                StudentRelated = dt.Rows(index)("StudentRelated").ToString()
                StudentNotSendHomwork = dt.Rows(index)("StudentNotSend").ToString()
                StudentNotComplete = dt.Rows(index)("StudentNotComplete").ToString()
                txtSmallDiv = (StudentRelated - StudentNotSendHomwork) & "/" & StudentRelated

                sb.Append("<div id='DivHomework' class='ForDivWidth ForDivHomework'>")
                sb.Append("<div id='DivTop' class='ForDivTop'>")
                sb.Append("<span class='ForSpanTeacherName'>" & HomeworkName & "</span>")
                sb.Append("<span class='ForSpanEndTime'>" & TeacherName & "</span>")
                sb.Append("<img  class='ImgShowInfo' MAID='" & MaId & "' src='../Images/homework/ShowHomeworkDetail.png' />")
                sb.Append("<div id='RightPanel' class='ForDivRightPanel'>")
                sb.Append("<div id='DivStudentRelateHomework' class='TxtSmallDiv ForSmallDivRightPanel' >" & txtSmallDiv & "</div>")

                'sb.Append("<div id='DivStudentRelateHomework' class='SR ForSmallDivRightPanel' >" & StudentRelated & "</div>")
                'sb.Append("<div id='DivStudentNotSend' class='SN ForSmallDivRightPanel' >" & StudentNotSendHomwork & "</div>")
                'sb.Append("<div id='DivResultStudentNotComplete' class='RS ForSmallDivRightPanel' >" & StudentNotComplete & "</div>")

                sb.Append("</div></div>")
                sb.Append("<div id='DivBottom' class='ForDivBottom'>")
                sb.Append("<span class='ForSpanHomeworkName'>ส่ง " & EndTime & "</span>")
                sb.Append("<img class='ImgEdit' onclick=""EditHomework('" & MaId & "')"" src='../Images/homework/pencil.png' />")
                sb.Append("</div></div>")
            Next
        Else
            sb.Append("<div id=""DivNoData"" class=""ForMainDivNoData"" >")
            sb.Append("<div id=""DivShowInfo"" class=""ForDivShowInFo"">")
            sb.Append("<span style=""font-size: 25px;font-weight: bold;color: rgb(122, 119, 119);"">ไม่พบข้อมูลค่ะ</span>")
            sb.Append("</div></div>")
        End If

        KnSession = Nothing
        Return sb.ToString()

    End Function

#End Region

    ''' <summary>
    ''' ทำการ Reload หน้าใหม่โดยส่ง querystring classname , iscurrent = true ไปเพื่อบอกว่าต้องการดูข้อมูลที่อยู่ในช่วงปัจจุบัน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCurrent.Click
        Response.Redirect("~/Student/HomewokNowAndHistoryPage.aspx?ClassName=" & ClassName & "&IsCurrent=true")
    End Sub

    ''' <summary>
    ''' ทำการ Reload หน้าใหม่โดยส่ง Querystring Classname , Iscurrent = False เพื่อบอกว่าต้องการดูข้อมูลประวัติ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnHistory.Click
        Response.Redirect("~/Student/HomewokNowAndHistoryPage.aspx?ClassName=" & ClassName & "&IsCurrent=false")
    End Sub

End Class