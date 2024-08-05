Public Class HomeworkDetail
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Dim ClsViewReport As New ClassViewReport(New ClassConnectSql())
    Public IsHaveBackBtn As String
    Public StudentName As String
    Public StudentClassRoom As String
    Public StudentCode As String
    Dim CalendarId As String
    Dim StudentId As String
    Public DeviceId As String

    Public Property IsHistoryHomework As Boolean
        Get
            Return ViewState("_IsHistoryHomework")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_IsHistoryHomework") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HttpContext.Current.Session("SchoolID") = "1000001"
        'HttpContext.Current.Session("Owner_Id") = "A9070A88-4DC9-40A1-9E75-A1AD10A5795F"

        If Request.QueryString("MorethanOne") = True Then
            'IsHaveBackBtn = "True"
            btnBack.Visible = True
        Else
            'IsHaveBackBtn = "False"
            btnBack.Visible = False
        End If

        If Request.QueryString("StudentId") Is Nothing Then
            Exit Sub
        End If

        DeviceId = Request.QueryString("DeviceId").ToString()

        If HttpContext.Current.Session("SchoolID") Is Nothing OrElse HttpContext.Current.Session("SchoolID").ToString = "" Then
            HttpContext.Current.Session("SchoolID") = ChatSelectStudent.GetSchoolIdByDeviceId(Request.QueryString("DeviceId").ToString())
        End If

        If CalendarId = "" Then
            CalendarId = GetCalendarId()
        End If

        If StudentId = "" Then
            StudentId = Request.QueryString("StudentId").ToString()
        End If

        'เช็คว่าเคยลงทะเบียนหรือยัง ถ้ายังต้องไปหน้าลงทะเบียนก่อน
        'CheckRegister(Request.QueryString("DeviceId").ToString())

        If Not Page.IsPostBack Then
            Dim dtStudentInfo As DataTable = GetDtStudentInfo(StudentId)
            CreateDivStudentInfo(dtStudentInfo)
            IsHistoryHomework = False
            Dim dtHomeworkInfo As DataTable = GetDtHomeworkInfo(StudentId, CalendarId, False)
            CreateDivHomeworkInfo(dtHomeworkInfo)
        End If
        
    End Sub

    Private Sub CheckRegister(ByVal DeviceId As String)

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & _DB.CleanString(DeviceId) & "' AND IsActive = 1 "
        Dim CheckCount As String = _DB.ExecuteScalar(sql)

        If CType(CheckCount, Integer) = 0 Then
            Response.Redirect("~/Watch/RegisterParent.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
        End If

    End Sub

    'sub ส่งค่าให้กับตัวแปร ชื่อ ห้อง เลขประจำตัวนักเรียนเพื่อให้เอาไปใช้ฝั่ง aspx
    Private Sub CreateDivStudentInfo(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            StudentName = dt.Rows(0)("StudentName").ToString()
            StudentClassRoom = dt.Rows(0)("StudentClassRoom").ToString()
            StudentCode = dt.Rows(0)("Student_Code").ToString()
        End If

    End Sub

    'Function Get ค่าต่างเพื่อเอาไปสร้าง DivStudentInfo
    Private Function GetDtStudentInfo(ByVal StudentId As String) As DataTable

        Dim sql As String = " select (Student_FirstName + ' ' + Student_LastName + ' (' + CASE WHEN Student_NickName IS NULL THEN '' ELSE Student_NickName END + ')' )as StudentName,Student_Code, " & _
                            " (Student_CurrentClass + Student_CurrentRoom) as StudentClassRoom " & _
                            " from t360_tblStudent where Student_Id = '" & _DB.CleanString(StudentId) & "' and Student_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    'Function สร้าง DivHomeworkInfo
    Private Sub CreateDivHomeworkInfo(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            For index = 0 To dt.Rows.Count - 1
                Dim CountLoop As Integer = index + 1
                Dim StringSubject As String = GetStringSubject(dt.Rows(index)("Reference_Id").ToString())
                Dim StringStatus As String = ""
                If dt.Rows(index)("TimeExitedByUser") IsNot DBNull.Value Then
                    StringStatus = "ส่งแล้ว"
                Else
                    StringStatus = GetStringStatus(dt.Rows(index)("Module_Status"), dt.Rows(index)("Quiz_Id").ToString())
                End If
                Dim FontColor As String = GetColorByStatus(StringStatus)
                Dim TeacherName As String = dt.Rows(index)("Teacher_FirstName").ToString() & " " & dt.Rows(index)("Teacher_LastName").ToString()
                sb.Append("<div id='DivInfo1" & CountLoop & "' class='ForDivInfo'>")
                sb.Append("<table style='width:100%'><tr>")
                sb.Append("<tr>")
                sb.Append("<td class='FortdStudentHomework'>")
                sb.Append("<div id='DivSubject" & CountLoop & "' class='ForDivSubject'>")
                sb.Append(StringSubject)
                sb.Append("</div>")
                sb.Append("</td>")
                sb.Append("<td class='FortdStudentHomework'>")
                sb.Append("<span class='spnStatus' style='color:" & FontColor & ";' >" & StringStatus & "</span>")
                sb.Append("</td>")
                sb.Append("</tr>")
                sb.Append("<tr>")
                sb.Append("<td class='FortdStudentHomework' style='font-size:20px;'>ส่ง " & dt.Rows(index)("End_Date").ToString() & "</td>")
                sb.Append("<td class='FortdStudentHomework' style='font-size:20px;'>สั่งโดย " & TeacherName & "</td>")
                sb.Append("</tr>")
                sb.Append("</table>")
                sb.Append("</div>")
            Next
            DivHomeworkInfo.InnerHtml = sb.ToString()
        End If

    End Sub

    'Function หาว่า การบ้านนี้เป็นวิชาอะไร หรือเป็น หลายวิชา
    Private Function GetStringSubject(ByVal TestsetId As String) As String

        Dim sql As String = " SELECT COUNT(distinct tblGroupSubject.GroupSubject_Id) FROM tblGroupSubject " & _
                            " INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN tblTestSet INNER JOIN " & _
                            " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                            " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                            " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                            " WHERE (tblTestSet.TestSet_Id = '" & TestsetId & "') AND tblTestsetQuestionSet.IsActive = '1' "
        Dim CheckIsManySubject As String = _DB.ExecuteScalar(sql)

        If CType(CheckIsManySubject, Integer) > 1 Then
            Return "รวมวิชา"
        Else
            sql = " SELECT tblGroupSubject.GroupSubject_Name FROM tblGroupSubject " & _
                  " INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN tblTestSet INNER JOIN " & _
                  " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                  " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                  " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                  " WHERE (tblTestSet.TestSet_Id = '" & TestsetId & "') AND tblTestsetQuestionSet.Isactive = '1' "
            Dim GroupSubjectName As String = _DB.ExecuteScalar(sql)
            GroupSubjectName = ClsViewReport.GenSubjectName(GroupSubjectName)
            Return GroupSubjectName
        End If

    End Function

    'Function หา Status ของนักเรียนว่า ยังไม่ทำ , เริ่มทำ ../.. , ทำเสร็จแล้ว
    Private Function GetStringStatus(ByVal ModuleStatus As Integer, ByVal QuizId As String) As String

        Dim StatusText As String = ""
        'If ModuleStatus = 3 Then
        '    StatusText = "ทำเสร็จแล้ว"
        '    'ElseIf ModuleStatus = 0 Then
        '    '    StatusText = "ยังไม่ทำ"
        'Else
        Dim sql As String = " SELECT COUNT(distinct case when tblQuizScore.Answer_Id is not null then tblQuizScore.Answer_Id else null end) as Isdone, " & _
                          " COUNT(distinct tblTestSetQuestionDetail.Question_Id) as TotalQuestion FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                          " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                          " tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id " & _
                          " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND (tblQuizScore.Student_Id = '" & StudentId & "') " & _
                          " And tbltestsetQuestionSet.IsActive = '1' And tblTestsetQuestionDetail.Isactive = '1'"
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Isdone").ToString() = "0" And dt.Rows(0)("TotalQuestion").ToString() = "0" Then
                StatusText = "ยังไม่ทำ"
            ElseIf dt.Rows(0)("Isdone").ToString() = dt.Rows(0)("TotalQuestion").ToString() Then
                StatusText = "ทำเสร็จแล้ว"
            Else
                If IsHistoryHomework Then
                    StatusText = "ทำได้ " & dt.Rows(0)("Isdone").ToString() & "/" & dt.Rows(0)("TotalQuestion").ToString() & " ข้อ"
                Else
                    StatusText = "ทำไปแล้ว " & dt.Rows(0)("Isdone").ToString() & "/" & dt.Rows(0)("TotalQuestion").ToString() & " ข้อ"
                End If
            End If

        End If
        'End If
        Return StatusText

    End Function

    'Function หาสีให้ String Status
    Private Function GetColorByStatus(ByVal InputStatus As String) As String

        Dim FontColor As String = ""
        'If InputStatus = "ยังไม่ทำ" Then
        '    FontColor = "Red"
        'ElseIf InputStatus = "ทำเสร็จแล้ว" Then
        '    FontColor = "rgb(107, 64, 213)"
        'ElseIf InputStatus = "ส่งแล้ว" Then
        '    FontColor = "green"
        'Else
        '    FontColor = "#CA7305"
        'End If
        If InputStatus = "ส่งแล้ว" Then
            FontColor = "green"
        Else
            FontColor = "Red"
        End If
        Return FontColor

    End Function

    'Function หา Dt สำหรับสร้าง DivHomeworkInfo
    Private Function GetDtHomeworkInfo(ByVal StudentId As String, ByVal CalendarId As String, ByVal IsHistory As Boolean)

        Dim sql As String = ""

        If IsHistory = True Then
            'ถ้าเป็นประวัติการบ้าน ให้เรียงตาม End_date ไม่ต้องดู Status
            sql = "SELECT t360_tblTeacher.Teacher_FirstName, t360_tblTeacher.Teacher_LastName, tblModuleDetailCompletion.TimeExitedByUser,  "
            sql &= " tblModuleDetailCompletion.Module_Status,tblModuleAssignment.End_Date, tblModuleDetail.Reference_Id, tblModuleDetailCompletion.Quiz_Id "
            sql &= " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id "
            sql &= " INNER JOIN  tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id "
            sql &= " INNER JOIN  tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id "
            sql &= " AND tblModuleDetail.ModuleDetail_Id =  tblModuleDetailCompletion.ModuleDetail_Id "
            sql &= " INNER JOIN t360_tblTeacher ON tblModule.Create_By = t360_tblTeacher.Teacher_id  "
            sql &= " WHERE (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') AND (tblModuleDetail.Reference_Type = 0)  "
            sql &= " AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "')  "
            sql &= " and tblmodule.IsActive = '1' and tblModuleDetailCompletion.IsActive = '1'"
            sql &= " and (dbo.GetThaiDate() > tblModuleAssignment.End_Date) "
            sql &= " order by End_Date"
        Else
            'การบ้านปัจจุบัน แยกการบ้านที่ส่งแล้วไว้ล่างสุด แล้วเรียงตาม End_Date

            sql = "SELECT t360_tblTeacher.Teacher_FirstName, t360_tblTeacher.Teacher_LastName, tblModuleDetailCompletion.TimeExitedByUser,  "
            sql &= " tblModuleDetailCompletion.Module_Status, tblModuleAssignment.End_Date, tblModuleDetail.Reference_Id, tblModuleDetailCompletion.Quiz_Id ,'0' as odr"
            sql &= " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN  tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id "
            sql &= " INNER JOIN  tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id AND tblModuleDetail.ModuleDetail_Id =  tblModuleDetailCompletion.ModuleDetail_Id "
            sql &= " INNER JOIN t360_tblTeacher ON tblModule.Create_By = t360_tblTeacher.Teacher_id  "
            sql &= " WHERE (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') AND (tblModuleDetail.Reference_Type = 0)  "
            sql &= " AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "')  "
            sql &= " and tblmodule.IsActive = '1' and tblModuleDetailCompletion.IsActive = '1'"
            sql &= " and TimeExitedByUser is null"
            sql &= " and (dbo.GetThaiDate() <= tblModuleAssignment.End_Date) union "
            sql &= " SELECT t360_tblTeacher.Teacher_FirstName, t360_tblTeacher.Teacher_LastName, tblModuleDetailCompletion.TimeExitedByUser,  tblModuleDetailCompletion.Module_Status, "
            sql &= " tblModuleAssignment.End_Date, tblModuleDetail.Reference_Id, tblModuleDetailCompletion.Quiz_Id  ,'1' as odr"
            sql &= " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN  tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id "
            sql &= " INNER JOIN  tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id AND tblModuleDetail.ModuleDetail_Id =  tblModuleDetailCompletion.ModuleDetail_Id "
            sql &= " INNER JOIN t360_tblTeacher ON tblModule.Create_By = t360_tblTeacher.Teacher_id  "
            sql &= " WHERE (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') AND (tblModuleDetail.Reference_Type = 0)  "
            sql &= " AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "')  "
            sql &= " and tblmodule.IsActive = '1' and tblModuleDetailCompletion.IsActive = '1'"
            sql &= " and TimeExitedByUser is not null"
            sql &= " and (dbo.GetThaiDate() <= tblModuleAssignment.End_Date) "
            sql &= " order by odr,End_Date"
        End If

        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    'Function GetCadlendarId
    Private Function GetCalendarId() As String

        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql)
        Return CalendarId

    End Function


    Private Sub btnShowAll_Click(sender As Object, e As EventArgs) Handles btnShowAll.Click
        If IsHistoryHomework = True Then
            IsHistoryHomework = False
            btnShowAll.Text = "ดูประวัติการบ้าน"
        Else
            IsHistoryHomework = True
            btnShowAll.Text = "ดูการบ้านปัจจุบัน"
        End If

        Dim dtStudentInfo As DataTable = GetDtStudentInfo(StudentId)
        CreateDivStudentInfo(dtStudentInfo)
        Dim dtHomeworkInfo As DataTable = GetDtHomeworkInfo(StudentId, CalendarId, IsHistoryHomework)
        CreateDivHomeworkInfo(dtHomeworkInfo)
    End Sub

End Class