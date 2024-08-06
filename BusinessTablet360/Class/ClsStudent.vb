Imports System.Web
Imports System.Text
Imports System.Data.SqlClient

Namespace Service
    <Serializable()> _
    Public Class ClsStudent

        Dim _DB As ClsConnect
        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB
        End Sub

        ''' <summary>
        ''' Get ชื่อห้องที่มี quiz หรือ hw ที่มีอยู่ในเทอมที่เลือกและเป็นของ teacher นั้น
        ''' </summary>
        ''' <param name="InputConn">ตัวแปร connetion</param>
        ''' <returns>dt ชื่อห้อง / ชั้น</returns>
        ''' <remarks></remarks>
        Public Function GetClassNameHaveQuizOrHomework(Optional ByVal InputConn As SqlConnection = Nothing) As DataTable
            Dim KnSession As New KNAppSession()
            Dim sql As New StringBuilder()
            'sql.Append(" SELECT (tblQuiz.t360_ClassName + tblQuiz.t360_RoomName) AS ClassName,t360_tblClass.Class_Order AS ClassOrder ")
            'sql.Append(" FROM tblQuiz INNER JOIN tblAssistant ON tblQuiz.User_Id = tblAssistant.Teacher_id ")
            'sql.Append(" INNER JOIN t360_tblClass ON t360_tblClass.Class_Name = tblQuiz.t360_ClassName ")
            'sql.Append(" WHERE tblQuiz.IsActive = 1  AND (tblquiz.IsQuizMode = 1 OR tblquiz.IsHomeWorkMode = 1) AND tblAssistant.Assistant_id = '")
            'sql.Append(HttpContext.Current.Session("UserId").ToString())
            'sql.Append("' AND tblQuiz.Calendar_Id = '")
            'sql.Append(KnSession("SelectedCalendarId").ToString())
            'sql.Append("' GROUP BY tblQuiz.t360_ClassName, tblQuiz.t360_RoomName,t360_tblClass.Class_Order UNION ")
            'sql.Append(" SELECT (t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom) AS ClassName,t360_tblClass.Class_Order AS ClassOrder ")
            'sql.Append(" FROM tblModule INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id ")
            'sql.Append(" INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id ")
            'sql.Append(" INNER JOIN tblModuleDetailCompletion ON tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id  ")
            'sql.Append(" AND tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id ")
            'sql.Append(" INNER JOIN tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id ")
            'sql.Append(" INNER JOIN t360_tblStudent ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id ")
            'sql.Append(" INNER JOIN t360_tblClass ON t360_tblStudent.Student_CurrentClass = t360_tblClass.Class_Name ")
            'sql.Append(" WHERE tblModule.IsActive = 1 AND tblAssistant.Assistant_id = '")
            'sql.Append(HttpContext.Current.Session("UserId").ToString())
            'sql.Append("' AND tblModuleAssignment.Calendar_Id = '")
            'sql.Append(KnSession("SelectedCalendarId").ToString())
            'sql.Append("' GROUP BY t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom,t360_tblClass.Class_Order ")
            sql.Append("select (a.ClassName + a.RoomName) AS ClassName,ClassOrder from (SELECT tblQuiz.t360_ClassName as ClassName ,tblQuiz.t360_RoomName AS RoomName")
            sql.Append(",t360_tblClass.Class_Order AS ClassOrder FROM tblQuiz INNER JOIN tblAssistant ON tblQuiz.User_Id = tblAssistant.Teacher_id  ")
            sql.Append(" INNER JOIN t360_tblClass ON t360_tblClass.Class_Name = tblQuiz.t360_ClassName WHERE tblQuiz.IsActive = 1  ")
            sql.Append(" AND (tblquiz.IsQuizMode = 1 OR tblquiz.IsHomeWorkMode = 1) AND tblAssistant.Assistant_id = '")
            sql.Append(HttpContext.Current.Session("UserId").ToString())
            sql.Append("' AND tblQuiz.Calendar_Id = '")
            sql.Append(KnSession("SelectedCalendarId").ToString())
            sql.Append("' GROUP BY tblQuiz.t360_ClassName, tblQuiz.t360_RoomName,t360_tblClass.Class_Order UNION  ")
            sql.Append(" SELECT t360_tblStudent.Student_CurrentClass  as ClassName , t360_tblStudent.Student_CurrentRoom as RoomName,")
            sql.Append("t360_tblClass.Class_Order AS ClassOrder FROM tblModule INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id  ")
            sql.Append("INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id  INNER JOIN tblModuleDetailCompletion ON ")
            sql.Append(" tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id ")
            sql.Append(" And tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id INNER JOIN tblAssistant ")
            sql.Append(" ON tblModule.Create_By = tblAssistant.Teacher_id  INNER JOIN t360_tblStudent ")
            sql.Append(" ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id INNER JOIN t360_tblClass ")
            sql.Append(" ON t360_tblStudent.Student_CurrentClass = t360_tblClass.Class_Name  ")
            sql.Append(" WHERE tblModule.IsActive = 1 AND tblAssistant.Assistant_id = '")
            sql.Append(HttpContext.Current.Session("UserId").ToString())
            sql.Append("' AND tblModuleAssignment.Calendar_Id = '")
            sql.Append(KnSession("SelectedCalendarId").ToString())
            sql.Append("' GROUP BY t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom,t360_tblClass.Class_Order) a")
            sql.Append(" order by dbo.FixedLengthClassAndRoom(a.ClassName, a.RoomName)")
            Dim db As New ClassConnectSql()

            GetClassNameHaveQuizOrHomework = _DB.getdata(sql.ToString(), , InputConn)
        End Function

        ''' <summary>
        ''' Get ชื่อห้อง ทั้งหมด IN School
        ''' </summary>
        ''' <returns>dt ห้อง/ชั้น</returns>
        ''' <remarks></remarks>
        Public Function GetClassNameInSchool()
            Dim sql As New StringBuilder()
            sql.Append(" SELECT (t360_tblRoom.Class_Name + t360_tblRoom.Room_Name) AS ClassName,t360_tblclass.Class_Order AS ClassOrder FROM t360_tblclass INNER JOIN ")
            sql.Append(" t360_tblRoom ON  t360_tblclass.Class_Name = t360_tblRoom.Class_Name WHERE t360_tblRoom.Room_IsActive = 1 AND Class_IsActive = 1 AND t360_tblRoom.School_Code = '")
            sql.Append(HttpContext.Current.Session("SchoolID").ToString())
            sql.Append("' ORDER BY t360_tblclass.Class_Order,len(t360_tblRoom.Room_Name),t360_tblRoom.Room_Name; ")
            'Dim db As New ClassConnectSql()
            GetClassNameInSchool = _DB.getdata(sql.ToString())
        End Function

        'Get ชื่อ + นามสกุล (สมชาย หัวจรดเท้า) นักเรียนโดย StudentId
        Public Function GetStudentFirstNameAndLastNameByStudentId(ByVal StudentId As String)
            Dim sql As String = " SELECT Student_FirstName + ' ' + Student_LastName FROM dbo.t360_tblStudent WHERE " & _
                                " Student_Id = '" & _DB.CleanString(StudentId) & "' AND Student_IsActive = 1 "
            Dim StudentName As String = _DB.ExecuteScalar(sql)
            Return StudentName
        End Function

        'Get Datatable ชื่อ,นามสกุล,ชื่อเล่น,ห้อง,ชั้น,รหัสนักเรียน,ชื่อพ่อ,เบอร์พ่อ,ชื่อแม่,เบอร์แม่,เบอร์นักเรียน
        Public Function GetDtStudentDetail(ByVal StudentId As String)
            Dim sql As String = " SELECT Student_FirstName,Student_LastName,Student_NickName,Student_CurrentClass,Student_CurrentRoom,Student_Code, " & _
                                " Student_FathertName, Student_FatherPhone, Student_MotherName, Student_MotherPhone, Student_Phone " & _
                                " FROM dbo.t360_tblStudent WHERE Student_Id = '" & _DB.CleanString(StudentId) & "' AND Student_IsActive = 1 "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            Return dt
        End Function

        'Get Dt รายละเอียดนักเรียน LeftJoin Student (ถึงปีนั้นนักเรียนคนนี้จะยังไม่ได้เรียนก็สามารถเอาข้อมูลที่อยู่ใน t360_tblStudent ออกมาได้แต่ชั้นกับปีในปีนั้นจะเป็น Null)
        'ไหมเอา Where IsActive StudentRoom ออก เพื่อที่เมื่อเปลี่ยน Calendar แล้วจะยังเจอข้อมูลของเด็กคนนี้
        'ยังไม่ได้ลองเทส เคสที่เด็กคนนี้เข้าเรียนระหว่างเทอม
        Public Function GetDtStudentDetailByCalendarAndStudentId(ByVal StudentId As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection)
            Dim sql As String = " SELECT t360_tblStudent.Student_FirstName, t360_tblStudent.Student_LastName, t360_tblStudentRoom.Class_Name,t360_tblStudent.Student_CurrentNoInRoom, " & _
                                " t360_tblStudentRoom.Room_Name, t360_tblStudent.Student_Code, t360_tblStudent.Student_FatherName, t360_tblStudent.Student_FatherPhone, " & _
                                " t360_tblStudent.Student_MotherName,t360_tblStudent.Student_MotherPhone, t360_tblStudent.Student_Phone,Student_NickName " & _
                                " FROM t360_tblStudentRoom RIGHT OUTER JOIN t360_tblStudent ON t360_tblStudentRoom.Student_Id = t360_tblStudent.Student_Id " & _
                                " WHERE (t360_tblStudentRoom.Calendar_Id = '" & _DB.CleanString(CalendarId) & "' OR dbo.t360_tblStudentRoom.Calendar_Id IS NULL) " & _
                                " AND (t360_tblStudent.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (t360_tblStudent.Student_IsActive = 1) "
            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)
            Return dt
        End Function

        'Check ว่านักเรียนคนนี้ถูกติดดาวหรือเปล่า (where ตาม TeacherId)
        Public Function CheckStudentIsFavoriteByTeacherIdAndStudentId(ByVal TeacherId As String, ByVal StudentId As String, ByRef InputConn As SqlConnection) As Boolean
            Dim sql As String = " SELECT COUNT(*) AS CheckFavorite FROM tblAssistant INNER JOIN " & _
                                " tblTeacherFavorite ON tblAssistant.Teacher_id = tblTeacherFavorite.Teacher_id " & _
                                " WHERE (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') AND " & _
                                " (tblTeacherFavorite.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (tblTeacherFavorite.Isactive = '1');"
            Dim IsFavorite As Boolean = False
            Dim CheckFavorite As String = _DB.ExecuteScalar(sql, InputConn)

            If CType(CheckFavorite, Integer) > 0 Then
                IsFavorite = True
            Else
                IsFavorite = False
            End If
            Return IsFavorite
        End Function


        ''' <summary>
        ''' function ในการหา student favorite code อะไร
        ''' </summary>
        ''' <param name="TeacherId"></param>
        ''' <param name="StudentId"></param>
        ''' <param name="InputConn"></param>
        ''' <returns>code favorite is number</returns>
        ''' 
        Public Function getStudentFavoriteCode(ByVal TeacherId As String, ByVal StudentId As String, ByRef InputConn As SqlConnection) As DataTable
            'Dim sql As String = " SELECT tblTeacherFavorite.FavoriteCode FROM tblAssistant INNER JOIN tblTeacherFavorite ON tblAssistant.Teacher_id = tblTeacherFavorite.Teacher_id " &
            '                  " WHERE (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') AND " &
            '                  " (tblTeacherFavorite.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (tblTeacherFavorite.Isactive = '1');"
            Dim sql As New StringBuilder()
            sql.Append("Select  tfd.FavoriteCode,tfd.FavoriteScore FROM tblTeacherFavorite tf INNER JOIN tblTeacherFavoriteDetail tfd On tf.Tf_id = tfd.Tf_Id ")
            sql.Append(" WHERE tf.Teacher_id = '" & TeacherId & "' AND tf.Student_Id = '" & StudentId & "' AND tfd.FavoriteScore <> 3 ORDER BY tfd.FavoriteOrder; ")
            Return _DB.getdata(sql.ToString(),, InputConn)
        End Function


        'หา Datatable ห้อง,ชั้น ของนักเรียนคนนั้นโดยใช้ calendarId
        Public Function GetStudentClassNameByStudentAndCalendarId(ByVal StudentId As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection)

            Dim sql As String = " SELECT Class_Name,Room_Name FROM dbo.t360_tblStudentRoom WHERE Student_Id = '" & _DB.CleanString(StudentId).ToString() & "' " & _
                                " AND Calendar_Id = '" & _DB.CleanString(CalendarId).ToString() & "' AND SR_IsActive = 1 "
            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)
            Return dt

        End Function

        'หานักเรียนทุกคนในห้อง 
        Public Function GetAllStudentByClassRoomAndTeacherId(ByVal ClassName As String, ByVal RoomName As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection)

            Dim sql As String = " SELECT t360_tblStudent.Student_FirstName, t360_tblStudent.Student_LastName, t360_tblStudent.Student_Id " & _
                                " FROM t360_tblStudent INNER JOIN t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id " & _
                                " WHERE (t360_tblStudentRoom.SR_IsActive = 1) AND (t360_tblStudent.Student_IsActive = 1) " & _
                                " AND (t360_tblStudentRoom.Class_Name = '" & _DB.CleanString(ClassName) & "') AND " & _
                                " (t360_tblStudentRoom.Room_Name = '" & _DB.CleanString(RoomName) & "') AND " & _
                                " (t360_tblStudentRoom.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " & _
                                " AND t360_tblStudent.School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "';"
            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)
            Return dt

        End Function


    End Class

End Namespace
