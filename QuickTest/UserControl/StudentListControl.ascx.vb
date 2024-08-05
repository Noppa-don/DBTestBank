Imports System.Web.Services
Imports System.Data.SqlClient
Imports BusinessTablet360

Public Class StudentListControl
    Inherits System.Web.UI.UserControl

    Dim _DB As New ClassConnectSql()
    Dim KnSession As New KNAppSession()
    Dim Clsuser As New ClsUser(New ClassConnectSql())
    Dim ClsStudent As New Service.ClsStudent(New ClassConnectSql())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim dt As New DataTable
        'dt.Columns.Add("Student_Id", GetType(String))
        'dt.Columns.Add("Student_FirstName", GetType(String))
        'dt.Columns.Add("Student_LastName", GetType(String))
        'dt.Columns.Add("IsFavorite", GetType(Integer))

        'For index = 0 To 4

        '    If index > 10 Then
        '        dt.Rows.Add(index)("Student_FirstName") = "สมชายasd"
        '        dt.Rows(index)("Student_LastName") = "เข็มกลัดฟหกฟหก"
        '        dt.Rows(index)("IsFavorite") = 1
        '        dt.Rows(index)("Student_Id") = "01413B8B-2DD8-4DC3-9A5B-084A69FA4860"
        '    Else
        '        dt.Rows.Add(index)("Student_FirstName") = "สมชาย"
        '        dt.Rows(index)("Student_LastName") = "เข็มกลัด"
        '        dt.Rows(index)("IsFavorite") = 0
        '        dt.Rows(index)("Student_Id") = "01413B8B-2DD8-4DC3-9A5B-084A69FA4860"
        '    End If
        'Next

        'CreateDivStudent(dt, True, True)

    End Sub

    Public Sub SetByTeacher(ByVal TeacherId As String, Optional ByVal InputConn As SqlConnection = Nothing)

        Dim dt As DataTable = GetStudentIdByTeacherId(TeacherId, InputConn)
        If dt.Rows.Count > 0 Then
            MainDiv.InnerHtml = CreateDivStudent(dt, False, True, InputConn)
        Else
            MainDiv.InnerHtml = CreateDivStudent(dt, False, False, InputConn)
        End If

    End Sub

    Public Sub SetByRoom(ByVal ClassName As String)

        Dim dt As DataTable = GetStudentIdByClassAndCalendarId(ClassName)
        If dt.Rows.Count > 0 Then
            MainDiv.InnerHtml = CreateDivStudent(dt, True, True)
        Else
            MainDiv.InnerHtml = CreateDivStudent(dt, True, False)
        End If

    End Sub

    'GEt html ไป
    Public Function GetHtmlByRoom(ByVal ClassName As String, Optional IsFromStudentListPage As Boolean = False) As String
        Dim dt As DataTable = GetStudentIdByClassAndCalendarId(ClassName)
        If dt.Rows.Count > 0 Then
            GetHtmlByRoom = CreateDivStudent(dt, True, True, , IsFromStudentListPage)
        Else
            GetHtmlByRoom = CreateDivStudent(dt, True, False, , IsFromStudentListPage)
        End If
    End Function

    Private Function CreateDivStudent(ByVal dt As DataTable, ByVal SetByRoom As Boolean, ByVal IsHaveData As Boolean, Optional ByVal InputConn As SqlConnection = Nothing, Optional IsFromStudentListPage As Boolean = False) As String

        Dim sb As New StringBuilder

        If IsHaveData = True Then
            If SetByRoom = True Then
                sb.Append("<div id='DivHaveData' class='ForMainDivHaveDataRoom' >")
            Else
                sb.Append("<div id='DivHaveData' class='ForMainDivHaveDataTeacher' >")
            End If
            For index = 0 To dt.Rows.Count - 1
                Dim LoopId As Integer = (index + 1)
                'sb.Append("<div id='DivCover" & LoopId & "' class='DivWidth DivCover'><div id='Divpicture" & LoopId & "' onclick=""GotoTeacherStudentDetailPage('" & dt.Rows(index)("Student_Id").ToString() & "',event);"" class='DivWidth DivPicture' >")
                sb.Append("<div id='DivCover" & LoopId & "' class='DivWidth DivCover'><div id='Divpicture" & LoopId & "' stuid='" & dt.Rows(index)("Student_Id").ToString() & "' class='DivWidth DivPicture' ")
                ' ใส่รูปจริงๆ
                sb.Append("style=""background-image: url('")
                sb.Append(GetUserPic(HttpContext.Current.Session("SchoolID").ToString(), dt.Rows(index)("Student_Id").ToString()))
                sb.Append("');"" >")
                'If SetByRoom = True Then
                '    ''ต้องมีดักว่าถ้าถูกติดดาวแล้วต้องเป็นดาวสีเหลือง ถ้ายังต้องเป็นดาวสีเทา
                '    'If dt.Rows(index)("IsFavorite") = 1 Then
                '    '    sb.Append("<img src='../Images/dashboard/student/Unfavorite.png' StId='" & dt.Rows(index)("Student_Id").ToString() & "' class='imgStar ForYellowStar' />")
                '    'Else
                '    '    sb.Append("<img src='../Images/dashboard/student/Favorite.png' StId='" & dt.Rows(index)("Student_Id").ToString() & "' class='imgStar ForGrayStar' />")
                '    'End If
                'Else
                '    sb.Append("<img src='../Images/dashboard/student/Unfavorite.png' StId='" & dt.Rows(index)("Student_Id").ToString() & "' class='ForDeleteFavorite' />")
                'End If

                ' สร้าง icon ตาม code ที่ favorite ไว้
                Dim dtStudentFavorite As DataTable = ClsStudent.getStudentFavoriteCode(HttpContext.Current.Session("UserId").ToString(), dt.Rows(index)("Student_Id").ToString(), InputConn)
                'Dim starColor As String = If((favoriteCode = "0"), "ForGrayStar", "ForYellowStar")
                sb.Append("<div class='favoriteStudent'  studentid='" & dt.Rows(index)("Student_Id").ToString() & "'>")

                sb.Append(FavoriteHelper.getImgStudentFavorite(dtStudentFavorite))

                sb.Append("</div>")

                'Div เล็กๆที่อยู่ใน Div นักเรียน
                'Dim dtGetInfoRightPanel As DataTable = GetDoneTodayByStudentId(dt.Rows(index)("Student_Id").ToString(), InputConn)
                'Dim HomeworkToday As Integer = 0
                'Dim QuizToday As Integer = 0
                'Dim PracticeToday As Integer = 0
                'If dtGetInfoRightPanel.Rows.Count > 0 Then
                '    HomeworkToday = dtGetInfoRightPanel.Rows(0)("HomeworkToday")
                '    QuizToday = dtGetInfoRightPanel.Rows(0)("QuizToday")
                '    PracticeToday = dtGetInfoRightPanel.Rows(0)("PricticeToday")
                'End If
                'Dim HomeworkNotSend As Integer = GetHomeworkNotSend(dt.Rows(index)("Student_Id").ToString(), False, InputConn)
                Dim HomeworkNotSendToday As Integer = GetHomeworkNotSend(dt.Rows(index)("Student_Id").ToString(), True, InputConn)

                'sb.Append("<div id='ForRightPanel" & LoopId & "' style='float:right;'>")
                'If HomeworkToday <> 0 Then 'จำนวนการบ้านที่ทำในวันนี้
                '    sb.Append("<div id='HomeworkToday" & LoopId & "' class='HTD StandardForSmallDiv ForSmallDivRight'>" & HomeworkToday & "</div>")
                'End If
                'If QuizToday <> 0 Then 'จำนวนควิซที่ทำในวันนี้
                '    sb.Append("<div id='QuizToday" & LoopId & "' class='QTD StandardForSmallDiv ForSmallDivRight'>" & QuizToday & "</div>")
                'End If
                'If PracticeToday <> 0 Then 'จำนวนฝึกฝนที่ทำในวันนี้
                '    sb.Append("<div id='PracticeToday" & LoopId & "' class='PTD StandardForSmallDiv ForSmallDivRight'>" & PracticeToday & "</div>")
                'End If
                'sb.Append("</div>")

                sb.Append("<div id='ForLeftPanel" & LoopId & "' stuid='" & dt.Rows(index)("Student_Id").ToString() & "' Class='ForLeftPanel' >")
                If HomeworkNotSendToday <> 0 Then 'จำนวนการบ้านที่ยังไม่ส่ง ที่จะถึง Deadline ภายใน 24 ชม.
                    sb.Append("<div id='HomeworkNotSendToday" & LoopId & "' class='HWTD StandardForSmallDiv ForSmallDivLeft'>" & HomeworkNotSendToday & "</div>")
                End If

                'If HomeworkNotSend <> 0 Then 'จำนวนการบ้านที่ยังไม่ส่งทั้งหมด (การบ้านที่ยัง Active อยู่)
                '    sb.Append("<div id='AllHomeworkNotsend" & LoopId & "' class='HWALL StandardForSmallDiv ForSmallDivLeft'>" & HomeworkNotSend & "</div>")
                'End If
                sb.Append("</div>")

                If IsFromStudentListPage Then
                    sb.Append(String.Format("<div id='StudentNo" & LoopId & "' Class='ForRightPanel'>เลขที่ {0}</div>", dt.Rows(index)("Student_CurrentNoInRoom").ToString()))
                Else
                    sb.Append("<div id='StudentNo" & LoopId & "' Class='ForRightPanel'>เลขที่" & dt.Rows(index)("Student_CurrentNoInRoom").ToString())
                    sb.Append("(" & dt.Rows(index)("Student_CurrentClass") & dt.Rows(index)("Student_CurrentRoom") & ")")
                    sb.Append("</div>")
                End If


                'Div ที่แสดงชื่อนักเรียน
                sb.Append("</div><div id='DivName" & (index + 1) & "' class='DivWidth DivName' >")
                sb.Append(dt.Rows(index)("Student_FirstName").ToString() & "  " & dt.Rows(index)("Student_LastName").ToString())
                sb.Append("</div></div>")
            Next
            sb.Append("</div>")
        Else
            sb.Append("<div id='DivNoData' class='ForMainDivNoData' >")
            If SetByRoom = False Then
                sb.Append("<div id='DivShowInfo' class='ForDivShowInFo' >")
                sb.Append("<img src='../Images/star.gif' style='width:120px;position:absolute;top:25px;left:30px;' />")
                sb.Append("<span style='font-size: 40px; font-weight: bold; position: relative; top: 25px;'>แสดงนักเรียนที่ติดดาวไว้</span>")
                sb.Append("<br />")
                sb.Append("<span style='font-size: 30px; position: relative; top: 30px;'>(ใช้ติดตามเด็กที่ต้องการดูแลเป็นพิเศษ)</span>")
                sb.Append("<br />")
                sb.Append("<span class='hint' style='top:30px;position: relative;'>ติดดาวเด็กที่ต้องการดูแลเป็นพิเศษได้ที่ <a href='../Student/StudentListPage.aspx' class='hint'>หน้ารายชื่อนักเรียน</a> ค่ะ</span>")
                sb.Append("</div>")
                sb.Append("</div>")
                MainDiv.Attributes.Add("class", "nodata")
            Else
                'sb.Append("<div id='DivShowInfo' class='ForDivShowInFo'>")
                'sb.Append("<img src='../Images/blue-arrow-pointing-left-hi.png' style='width: 130px; position: absolute; top: 40px;left:20px;' />")
                'sb.Append("<span style='font-size: 60px; font-weight: bold; position: relative; top: 60px;'>เลือกห้องก่อนค่ะ</span>")
                'sb.Append("</div>")
                sb.Clear()
                sb.Append("True")
            End If
        End If

        'sb.Append("</div>")
        CreateDivStudent = sb.ToString()

    End Function

#Region "Function & Query"

    Private Function GetUserPic(ByVal School_Id As String, ByVal Student_Id As String)

        'Dim StrPath As String = HttpContext.Current.Server.MapPath("../UserData/" & School_Id.ToString() & "/{" & Student_Id & "}/Id.jpg")

        '    If System.IO.File.Exists(StrPath) Then
        '        SetHasPhoto(Student_Id)
        '        UserData = "../UserData/" & School_Id & "/{" & Student_Id & "}/Id.jpg"
        '    End If

        Dim UserData As String


        Dim PhotoStatus As Boolean

        PhotoStatus = Clsuser.GetStudentHasPhoto(Student_Id)

        If PhotoStatus Then

            UserData = "../UserData/" & School_Id & "/{" & Student_Id & "}/Id.jpg"

        Else

            UserData = "MonsterID.axd?seed=" & Student_Id & "&size=179"

        End If

        Return UserData

    End Function

    Private Function CheckHasPhoto(ByVal Student_Id As String) As String

        Dim sql As String
        sql = "select Student_HasPhoto from t360_tblstudent where Student_id = '" & Student_Id & "'"
        Dim HasPhoto As String
        HasPhoto = _DB.ExecuteScalar(sql)

        Return HasPhoto

    End Function
    Private Sub SetHasPhoto(ByVal Student_Id As String)
        Dim sql As String
        sql = "update t360_tblstudent set Student_HasPhoto = '1' where Student_id = '" & Student_Id & "'"
        _DB.Execute(sql)
    End Sub

    Private Function GetStudentIdByTeacherId(ByVal TeacherId As String, Optional ByVal InputConn As SqlConnection = Nothing)

        Dim sql As String
        sql = " SELECT t360_tblStudent.Student_Id, t360_tblStudent.Student_FirstName, t360_tblStudent.Student_LastName,t360_tblStudent.Student_CurrentNoInRoom,t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom"
        sql &= " FROM tblTeacherFavorite INNER JOIN t360_tblStudent ON tblTeacherFavorite.Student_Id = t360_tblStudent.Student_Id  "
        sql &= " INNER JOIN tblAssistant ON tblTeacherFavorite.Teacher_id = tblAssistant.Teacher_id  "
        sql &= " INNER JOIN tblTeacherFavoriteDetail ON tblTeacherFavorite.Tf_id = tblTeacherFavoriteDetail.Tf_Id "
        sql &= " WHERE (t360_tblStudent.Student_IsActive = 1) AND (tblAssistant.Assistant_id = '" & TeacherId & "') "
        sql &= " And (tblTeacherFavorite.IsActive = '1')  AND tblTeacherFavoriteDetail.FavoriteScore <> 3 "
        sql &= " GROUP BY  t360_tblStudent.Student_Id, t360_tblStudent.Student_FirstName, t360_tblStudent.Student_LastName,t360_tblStudent.Student_CurrentNoInRoom,t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom "
        sql &= " order by dbo.FixedLengthClassAndRoom(t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom),t360_tblStudent.Student_CurrentNoInRoom; "

        Return _DB.getdata(sql, , InputConn)
    End Function

    Private Function GetStudentIdByClassAndCalendarId(ByVal ClassRoom As String)
        'Session("Calendar_Id") = "79A7DD30-0E0B-449F-BCA7-161A2D056513"
        Dim Calendar_Id As String = KnSession.StoredValue("SelectedCalendarId").ToString()
        Dim SplitStr = ClassRoom.Split("/")
        Dim ClassName As String = SplitStr(0).ToString()
        Dim RoomName As String = "/" & SplitStr(1).ToString()
        'Dim sql As String = " SELECT t360_tblStudent.Student_Id,COUNT(tblTeacherFavorite.Student_Id) AS IsFavorite, t360_tblStudent.Student_LastName, t360_tblStudent.Student_FirstName " & _
        '                    " FROM t360_tblStudentRoom INNER JOIN t360_tblStudent ON t360_tblStudentRoom.Student_Id = t360_tblStudent.Student_Id LEFT OUTER JOIN " & _
        '                    " tblTeacherFavorite ON t360_tblStudentRoom.Student_Id = tblTeacherFavorite.Student_Id " & _
        '                    " WHERE (t360_tblStudentRoom.Class_Name = '" & _DB.CleanString(ClassName) & "') AND " & _
        '                    " (t360_tblStudentRoom.Room_Name = '" & _DB.CleanString(RoomName) & "') AND " & _
        '                    " (t360_tblStudentRoom.SR_IsActive = 1) AND (t360_tblStudentRoom.Calendar_Id = '" & Calendar_Id & "') " & _
        '                    " AND (dbo.tblTeacherFavorite.Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' OR dbo.tblTeacherFavorite.Teacher_id IS NULL ) " & _
        '                    " GROUP BY t360_tblStudent.Student_LastName, t360_tblStudent.Student_FirstName, t360_tblStudentRoom.Student_Id,t360_tblStudent.Student_Id "
        Dim sql As String = " SELECT t360_tblStudent.Student_CurrentNoInRoom,t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom, t360_tblStudent.Student_Id, t360_tblStudent.Student_LastName, t360_tblStudent.Student_FirstName, " & _
                            " (SELECT COUNT(*) FROM dbo.tblTeacherFavorite AS tf WHERE tf.Student_Id = t360_tblStudentRoom.Student_Id  " & _
                            " AND tf.Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' And tf.IsActive = '1') AS IsFavorite FROM t360_tblStudentRoom INNER JOIN " & _
                            " t360_tblStudent ON t360_tblStudentRoom.Student_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (t360_tblStudentRoom.Class_Name = '" & _DB.CleanString(ClassName) & "') AND (t360_tblStudentRoom.Room_Name = '" & _DB.CleanString(RoomName) & "') AND " & _
                            " (t360_tblStudentRoom.SR_IsActive = 1) AND (t360_tblStudentRoom.Calendar_Id = '" & Calendar_Id & "') " & _
                            " AND t360_tblStudent.Student_IsActive = 1  AND t360_tblStudent.School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'  " & _
                            " GROUP BY t360_tblStudent.Student_LastName, t360_tblStudent.Student_FirstName, t360_tblStudentRoom.Student_Id, t360_tblStudent.Student_Id,dbo.t360_tblStudent.Student_CurrentNoInRoom ,t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom " & _
                            " ORDER BY dbo.t360_tblStudent.Student_CurrentNoInRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetHomeworkNotSend(ByVal StudentId As String, ByVal IsToday As Boolean, Optional ByVal InputConn As SqlConnection = Nothing) As Integer

        Dim sql As String = " SELECT COUNT(tblModuleDetailCompletion.Quiz_Id) AS ResultHomework " &
                            " FROM tblModuleAssignment INNER JOIN tblModule ON tblModuleAssignment.Module_Id = tblModule.Module_Id INNER JOIN " &
                            " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " &
                            " tblModuleDetailCompletion ON tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id AND " &
                            " tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN " &
                            " tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id WHERE " &
                            " (tblAssistant.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "') AND " &
                            " (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND " &
                            " (tblModuleDetailCompletion.TimeExitedByUser IS NULL) AND ( dbo.GetThaiDate() >=  dbo.tblModuleAssignment.Start_Date) " &
                            " AND (dbo.GetThaiDate() <= dbo.tblModuleAssignment.End_Date) AND (dbo.tblModuleDetail.Reference_Type = 0) And (tblModuleDetailCompletion.IsActive = 1) "
        If IsToday = True Then
            sql &= " AND (dbo.GetThaiDate() >=  DATEADD(DAY,-1,dbo.tblModuleAssignment.End_Date) ) "
        End If
        Dim HomeworkNotsend As String = _DB.ExecuteScalar(sql, InputConn)

        Dim NumberOfHomeworkNotSend As Integer = CType(HomeworkNotsend, Integer)

        Return NumberOfHomeworkNotSend

    End Function


    Private Function GetDoneTodayByStudentId(ByVal StudentId As String, Optional ByVal InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT COUNT(DISTINCT tblQuizScore.Quiz_Id) AS HomeworkToday, " &
                            " COUNT(DISTINCT tblQuizScore_1.Quiz_Id) AS QuizToday, COUNT(DISTINCT tblQuizScore_2.Quiz_Id) AS PricticeToday " &
                            " FROM tblQuizScore INNER JOIN tblQuiz ON tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN " &
                            " tblModuleDetail INNER JOIN tblModule ON tblModuleDetail.Module_Id = tblModule.Module_Id ON " &
                            " tblQuiz.TestSet_Id = tblModuleDetail.Reference_Id RIGHT OUTER JOIN tblQuiz AS tblQuiz_1 INNER JOIN " &
                            " tblQuizScore AS tblQuizScore_1 ON tblQuiz_1.Quiz_Id = tblQuizScore_1.Quiz_Id RIGHT OUTER JOIN " &
                            " tblTestSet INNER JOIN tblAssistant ON tblTestSet.UserId = tblAssistant.Teacher_id INNER JOIN " &
                            " tblQuiz AS tblQuiz_2 ON tblTestSet.TestSet_Id = tblQuiz_2.TestSet_Id INNER JOIN " &
                            " tblQuizScore AS tblQuizScore_2 ON tblQuiz_2.Quiz_Id = tblQuizScore_2.Quiz_Id ON tblQuiz_1.User_Id = tblAssistant.Teacher_id ON " &
                            " tblModule.Create_By = tblAssistant.Teacher_id WHERE (tblQuiz.IsHomeWorkMode = 1) AND " &
                            " (tblAssistant.Assistant_id ='" & HttpContext.Current.Session("UserId").ToString() & "') AND (tblQuiz_1.IsQuizMode = 1) " &
                            " AND (tblTestSet.IsActive = 1) AND (tblQuiz_2.IsPracticeMode = 1) " &
                            " AND (tblQuizScore.Student_Id = '" & StudentId & "' OR dbo.tblQuizScore.Student_Id IS NULL) " &
                            " AND (tblQuizScore_1.Student_Id = '" & StudentId & "' OR tblQuizScore_1.Student_Id IS NULL)  " &
                            " AND (tblQuizScore_2.Student_Id = '" & StudentId & "' OR tblQuizScore_2.Student_Id IS NULL) " &
                            " AND (DATEDIFF(DAY,dbo.GetThaiDate(),dbo.tblQuizScore.LastUpdate) = 0 OR dbo.tblQuizScore.LastUpdate IS NULL ) " &
                            " AND (DATEDIFF(DAY,dbo.GetThaiDate(),tblQuizScore_1.LastUpdate) = 0 OR tblQuizScore_1.LastUpdate IS NULL  ) " &
                            " AND (DATEDIFF(DAY,dbo.GetThaiDate(),tblQuizScore_2.LastUpdate) = 0 OR tblQuizScore_2.LastUpdate IS NULL ) "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        Return dt

    End Function


#End Region

End Class