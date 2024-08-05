Imports System.IO
Imports System.Web

Public Class ApproveParentRegister
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim TeacherId As String = ""
        Dim dt As New DataTable

        If HttpContext.Current.Session("UserId") IsNot Nothing Then
            TeacherId = HttpContext.Current.Session("UserId").ToString()
            Dim CalendarId As String = GetCalendarId(HttpContext.Current.Session("SchoolID"))
            If CalendarId = "" Then
                Response.Write("ไม่มี CalendarId")
                Exit Sub
            End If
            dt = GetDtInfo(TeacherId, True, CalendarId)
        ElseIf HttpContext.Current.Session("UserId360") IsNot Nothing Then
            TeacherId = HttpContext.Current.Session("UserId360")
            Dim CalendarId As String = GetCalendarId(HttpContext.Current.Session("SchoolID"))
            If CalendarId = "" Then
                Response.Write("ไม่มี CalendarId")
                Exit Sub
            End If
            dt = GetDtInfo(TeacherId, False, CalendarId)
        End If

        If dt.Rows.Count > 0 Then
            CreateListRequest(dt)
        End If

    End Sub

    Private Function GetDtInfo(ByVal TeacherId As String, ByVal IsHaveClassTeacher As Boolean, ByVal CalendarId As String)

        Dim sql As String = ""
        Dim dt As New DataTable

        If IsHaveClassTeacher = True Then
            sql = " SELECT tblRequestParentApproval.PR_FirstName + ' ' + tblRequestParentApproval.PR_LastName AS ParentName, tblRequestParentApproval.LastUpdate, " & _
                  " t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS StudentName, " & _
                  " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom AS ClassRoomName, t360_tblStudent.Student_CurrentNoInRoom,RPAD_Id " & _
                  " FROM tblRequestParentApprovalDetail INNER JOIN t360_tblStudentRoom ON tblRequestParentApprovalDetail.Student_Id = t360_tblStudentRoom.Student_Id " & _
                  " INNER JOIN t360_tblTeacher ON t360_tblStudentRoom.Class_Name = t360_tblTeacher.Teacher_CurrentClass AND " & _
                  " t360_tblStudentRoom.Room_Name = t360_tblTeacher.Teacher_CurrentRoom INNER JOIN tblRequestParentApproval ON " & _
                  " tblRequestParentApprovalDetail.RPA_Id = tblRequestParentApproval.RPA_Id INNER JOIN t360_tblStudent ON tblRequestParentApprovalDetail.Student_Id " & _
                  " = t360_tblStudent.Student_Id WHERE (t360_tblStudentRoom.Calendar_Id = '" & CalendarId & "') AND " & _
                  " (t360_tblTeacher.Teacher_id = '" & TeacherId & "') AND (tblRequestParentApprovalDetail.IsActive = 1) AND (dbo.tblRequestParentApproval.IsApproved = 0) "
        Else
            sql = " SELECT tblRequestParentApproval.PR_FirstName + ' ' + tblRequestParentApproval.PR_LastName AS ParentName, tblRequestParentApprovalDetail.LastUpdate, " & _
                  " t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS StudentName, " & _
                  " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom AS ClassRoomName, tblRequestParentApprovalDetail.RPAD_Id, " & _
                  " t360_tblStudent.Student_CurrentNoInRoom FROM uvw_IsClassRoomWithoutTeacherClass INNER JOIN t360_tblStudentRoom ON " & _
                  " uvw_IsClassRoomWithoutTeacherClass.Calendar_Id = t360_tblStudentRoom.Calendar_Id AND uvw_IsClassRoomWithoutTeacherClass.Class_Name " & _
                  " = t360_tblStudentRoom.Class_Name AND uvw_IsClassRoomWithoutTeacherClass.Room_Name = t360_tblStudentRoom.Room_Name INNER JOIN " & _
                  " tblRequestParentApprovalDetail ON t360_tblStudentRoom.Student_Id = tblRequestParentApprovalDetail.Student_Id INNER JOIN " & _
                  " tblRequestParentApproval ON tblRequestParentApprovalDetail.RPA_Id = tblRequestParentApproval.RPA_Id INNER JOIN " & _
                  " t360_tblStudent ON tblRequestParentApprovalDetail.Student_Id = t360_tblStudent.Student_Id " & _
                  " WHERE (uvw_IsClassRoomWithoutTeacherClass.Calendar_Id = '" & CalendarId & "') "
        End If
        Dim _DB As New ClassConnectSql()
        dt = _DB.getdata(sql)
        Return dt

    End Function


    Private Sub CreateListRequest(ByVal dt As DataTable)

        Dim sb As New StringBuilder
        Dim Counter As Integer
        For index = 0 To dt.Rows.Count - 1
            Counter = index + 1
            sb.Append("<div id='" & dt.Rows(index)("RPAD_Id").ToString() & "'  class='ForDivRequest'>")
            sb.Append("<div id='DivPic" & Counter & "' class='ForDiv DivPic' >")
            sb.Append("<img src='../Watch/RequestImages/" & dt.Rows(index)("RPAD_Id").ToString().ToLower() & ".png' style='width:150px;' />")
            sb.Append("</div><div id='DivInfo" & Counter & "' class='ForDiv DivInFo' >")
            sb.Append(" <div id='ParentInFo' class='ForParentInfo' ><span>ผู้ปกครอง</span><br />")
            sb.Append("<span id='spnParentName'>" & dt.Rows(index)("ParentName") & "</span>")
            sb.Append("<span id='spnRequestDate'>วันที่ " & dt.Rows(index)("LastUpdate").ToString() & "</span></div>")
            sb.Append("<div id='StudentInfo" & Counter & "' class='ForStudentInfo' ><span>นักเรียน</span><br />")
            sb.Append("<span id='spnStudentName'>" & dt.Rows(index)("StudentName") & "</span><br />")
            sb.Append("<span id='spnStudentClassRoom'>" & dt.Rows(index)("ClassRoomName") & "</span>")
            sb.Append("<span id='spnStudentCurrentNoInroom'>เลขที่ " & dt.Rows(index)("Student_CurrentNoInRoom") & "</span></div></div>")
            sb.Append("<div id='DivApprove" & Counter & "' class='ForDiv DivApprove'><div>")
            sb.Append("<img src='../Images/Right Tick/check.jpeg' class='imgApprove' style='width:50px;' onclick=""ApproveRequest('" & dt.Rows(index)("RPAD_Id").ToString() & "')"" />")
            sb.Append("</div><div><img src='../Images/Close.png' onclick=""NotApproveRequest('" & dt.Rows(index)("RPAD_Id").ToString() & "')"" />")
            sb.Append("</div></div></div></div>")
        Next
        MainDiv.InnerHtml = sb.ToString()

    End Sub

    <Services.WebMethod()>
    Public Shared Function ApproveParentRegister(ByVal RPADID As String)

        'update flag ใน tbl หลักให้เรียบร้อยแล้ว insert ข้อมูลลง tbl จริง เสร็จแล้วย้ายรูป

        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""

        Try
            'เปิด Transaction
            _DB.OpenWithTransection()
            'select ข้อมูลที่ request มาก่อน
            sql = " SELECT tblRequestParentApproval.PR_FirstName, tblRequestParentApproval.PR_LastName, " & _
                  " tblRequestParentApproval.PR_Phone, tblRequestParentApproval.RPA_DeviceId, " & _
                  " tblRequestParentApprovalDetail.Student_Id, tblRequestParentApproval.RPA_Id, " & _
                  " t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS StudentName " & _
                  " FROM tblRequestParentApproval INNER JOIN tblRequestParentApprovalDetail ON tblRequestParentApproval.RPA_Id = " & _
                  " tblRequestParentApprovalDetail.RPA_Id INNER JOIN t360_tblStudent ON tblRequestParentApprovalDetail.Student_Id = t360_tblStudent.Student_Id " & _
                  " WHERE (tblRequestParentApprovalDetail.RPAD_Id = '" & RPADID & " ') "

            Dim dt As New DataTable
            dt = _DB.getdataWithTransaction(sql)

            If dt.Rows.Count > 0 Then

                'Insert tblParent ก่อน
                sql = " SELECT NEWID() "
                Dim ParentId As String = _DB.ExecuteScalarWithTransection(sql)
                sql = " INSERT INTO dbo.tblParent ( PR_Id ,PR_FirstName ,PR_LastName , PR_Phone ,DeviceId ,LastUpdate ,IsActive) " &
                      " VALUES  ( '" & ParentId.ToString() & "' , '" & dt.Rows(0)("PR_FirstName") & "' ,'" & dt.Rows(0)("PR_LastName") & "' " &
                      " ,'" & dt.Rows(0)("PR_Phone") & "' ,'" & dt.Rows(0)("RPA_DeviceId") & "' ,dbo.GetThaiDate() ,1) "
                _DB.ExecuteWithTransection(sql)

                'Insert tblStudentparent ต่อ
                sql = " SELECT NEWID() "
                Dim StudentParentId As String = _DB.ExecuteScalarWithTransection(sql)
                sql = " INSERT INTO dbo.tblStudentParent ( SP_Id ,PR_Id ,Student_Id ,LastUpdate ,IsActive) " &
                      " VALUES  ( '" & StudentParentId.ToString() & "' ,'" & ParentId.ToString() & "' ,'" & dt.Rows(0)("Student_Id").ToString() & "' ,dbo.GetThaiDate() ,1 ) "
                _DB.ExecuteWithTransection(sql)

                'Update tblRequestParentAppproval
                sql = " UPDATE dbo.tblRequestParentApproval SET IsApproved = 1 ,Approve_Date = dbo.GetThaiDate(),LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE RPA_Id = '" & dt.Rows(0)("RPA_Id").ToString() & "' "
                _DB.ExecuteWithTransection(sql)

                'ย้ายรูปและลบรูป
                'File.Move(HttpContext.Current.Server.MapPath("../Watch/RequestImages/" & RPADID.ToString().ToLower() & ".png"), HttpContext.Current.Server.MapPath("../Watch/ParentImages/" & StudentParentId.ToLower() & ".png"))
                File.Move("D:\data\tmp\Watch\RequestImages\" & RPADID.ToString().ToLower() & ".png", HttpContext.Current.Server.MapPath("../Watch/ParentImages/" & StudentParentId.ToLower() & ".png"))

                'ปิด transaction
                _DB.CommitTransection()

                'เก็บ Log
                Log.Record(Log.LogType.ParentRegister, "อนุมัติผู้ปกครองที่ลงทะเบียน ชื่อ " & dt.Rows(0)("PR_FirstName") & " ผู้ปกครองของนักเรียน ชื่อ " & dt.Rows(0)("StudentName"), True)
            End If
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "-1"
        End Try
    End Function

    <Services.WebMethod()>
    Public Shared Function NotApproveParentRegister(ByVal RPADID As String)

        'Update Flag เป็นไม่อนุมัติ 
        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""

        Try
            sql = " SELECT dbo.tblRequestParentApproval.RPA_Id FROM dbo.tblRequestParentApproval INNER JOIN dbo.tblRequestParentApprovalDetail " &
                  " ON dbo.tblRequestParentApproval.RPA_Id = dbo.tblRequestParentApprovalDetail.RPA_Id WHERE RPAD_Id = '" & RPADID & "' "
            Dim RPAID As String = _DB.ExecuteScalar(sql)
            sql = " UPDATE dbo.tblRequestParentApproval SET IsApproved = 2 ,Approve_Date = dbo.GetThaiDate(),LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE RPA_Id = '" & RPAID.ToString() & "' "
            _DB.Execute(sql)
            Log.Record(Log.LogType.ParentRegister, "ไม่อนุมัติผู้ปกครองที่ลงทะเบียนที่ RPA_ID = " & RPAID.ToString(), True)
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

    End Function


    Private Function GetCalendarId(ByVal SchoolId As String)

        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                            " AND Calendar_Type = 3 AND School_Code = '" & SchoolId & "'; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql)
        Return CalendarId

    End Function

End Class