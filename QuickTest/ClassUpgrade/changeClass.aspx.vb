Imports System.Web.Script.Serialization
Imports System.Web

Public Class changeClass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            'Session("SchoolID") = 1000001

            '****** 28/01/2014 ปิดไว้ก่อน หน้านี้ยังไม่ได้ทดสอบ ให้ทดสอบก่อนใช้ใหม่
            'createClassInSchool() ' สร้างชั้นเรียน
            'clearSessionVirtualRoom() 'clear session
            '******
        End If

    End Sub

    Private Sub createClassInSchool()
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT DISTINCT(Class_Name) FROM t360_tblStudentRoom WHERE School_Code = '" & Session("SchoolID") & "' AND Class_Name <> '' AND SR_IsActive = '1'; "
        Dim dt As DataTable = db.getdata(sql)
        clearSessionVirtualRoom() 'clear session
        Dim divClass As String = ""
        Try
            If (dt.Rows().Count > 0) Then
                For i As Integer = 0 To dt.Rows().Count - 1
                    divClass &= "<div class='divclass'> " & dt.Rows(i)("Class_Name") & "</div>"
                Next
            End If
            divAllClassInSchool.InnerHtml = divClass
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            divAllClassInSchool.InnerHtml = ""
        End Try
    End Sub

    <Services.WebMethod()>
    Public Shared Function createRoomInClass(ByVal classRoom As String) As String
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT Student_Id,Student_NoInRoom,Class_Name,Room_Name FROM t360_tblStudentRoom WHERE Class_Name  = '" & classRoom & "' AND School_Code = '" & HttpContext.Current.Session("SchoolID") & "' AND SR_AcademicYear = '" & GetAcademicYear().ToString() & "' AND SR_IsActive = '1' ORDER BY Room_Name,Student_NoInRoom ; "
        Dim dt As DataTable = db.getdata(sql)
        Dim divClassRoomAndStudent As String = ""
        Dim curentRoom As String = ""
        clearSessionVirtualRoom() 'clear session
        Try
            For i As Integer = 0 To dt.Rows().Count - 1
                If (curentRoom <> dt.Rows(i)("Room_Name")) Then
                    curentRoom = dt.Rows(i)("Room_Name")
                    If (i <> 0) Then
                        divClassRoomAndStudent &= "</div>"
                    End If
                    Dim c As String = dt.Rows(i)("Room_Name")
                    c = c.Replace("/", "")
                    divClassRoomAndStudent &= "<h3><a href='#' style=""background-color: #FFC76F;"">" & dt.Rows(i)("Room_Name") & "</a></h3>"
                    divClassRoomAndStudent &= "<div id='room" & c & "' style=""background-color: #F4F7FF;""><span class='" & dt.Rows(i)("Student_Id").ToString() & "_g' number='" & dt.Rows(i)("Student_NoInRoom") & "' id='" & dt.Rows(i)("Student_Id").ToString() & "'>" & "เลขที่  " & dt.Rows(i)("Student_NoInRoom") & "</span>"
                Else
                    divClassRoomAndStudent &= "<span class='" & dt.Rows(i)("Student_Id").ToString() & "_g' number='" & dt.Rows(i)("Student_NoInRoom") & "' id='" & dt.Rows(i)("Student_Id").ToString() & "'>" & "เลขที่  " & dt.Rows(i)("Student_NoInRoom") & "</span>"
                End If
            Next
            divClassRoomAndStudent &= "</div>"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            divClassRoomAndStudent = ""
        End Try

        Return divClassRoomAndStudent
    End Function

    <Services.WebMethod()>
    Public Shared Function createRoomInClassForChange(ByVal classSelected As String, ByVal roomSelected As String) As String
        roomSelected = roomSelected.Trim()
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT Student_Id,Student_NoInRoom,Class_Name,Room_Name FROM t360_tblStudentRoom WHERE Class_Name = '" & classSelected & "' AND Room_Name <> '" & roomSelected & "' AND School_Code = '" & HttpContext.Current.Session("SchoolID") & "' AND SR_AcademicYear = '" & GetAcademicYear().ToString() & "' AND SR_IsActive = '1' "
        Dim divClassRoomAndStudentForChange As String = ""
        Dim currentRoom As String = ""

        If (HttpContext.Current.Session("newRoom") <> "" Or HttpContext.Current.Session("newRoom") IsNot Nothing) Then
            Dim newRoom As String = HttpContext.Current.Session("newRoom")
            Dim newRoomArr = newRoom.Split(",")
            For i As Integer = 0 To newRoomArr.Length - 1
                If newRoomArr(i).Trim() <> roomSelected Then
                    sql &= " UNION (SELECT NEWID(),'','','" & newRoomArr(i) & "')"
                End If
            Next
        End If
        sql &= " ORDER BY Room_Name,Student_NoInRoom ; "

        Dim dt As DataTable = db.getdata(sql)
        Try
            For i As Integer = 0 To dt.Rows().Count - 1
                If (currentRoom <> dt.Rows(i)("Room_Name")) Then
                    currentRoom = dt.Rows(i)("Room_Name")
                    If (i <> 0) Then
                        divClassRoomAndStudentForChange &= "</div>"
                    End If
                    Dim c As String = dt.Rows(i)("Room_Name")
                    c = c.Replace("/", "").Trim()

                    divClassRoomAndStudentForChange &= "<h3><a href='#' style=""background-color: #FFC76F;"">" & dt.Rows(i)("Room_Name") & "</a></h3>"
                    divClassRoomAndStudentForChange &= "<div id='room" & c & "_change' style=""background-color: #F4F7FF;""><span class='" & dt.Rows(i)("Student_Id").ToString() & "_g' number='" & dt.Rows(i)("Student_NoInRoom") & "' id='" & dt.Rows(i)("Student_Id").ToString() & "_change'>" & "เลขที่  " & dt.Rows(i)("Student_NoInRoom") & "</span>"
                Else
                    divClassRoomAndStudentForChange &= "<span class='" & dt.Rows(i)("Student_Id").ToString() & "_g' number='" & dt.Rows(i)("Student_NoInRoom") & "'  id='" & dt.Rows(i)("Student_Id").ToString() & "_change'>" & "เลขที่  " & dt.Rows(i)("Student_NoInRoom") & "</span>"
                End If
            Next
            divClassRoomAndStudentForChange &= "</div>"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            divClassRoomAndStudentForChange = ""
        End Try

        Return divClassRoomAndStudentForChange
    End Function

    <Services.WebMethod()>
    Public Shared Function updateStudentChangeCodeBehind(ByVal studentChange As Object)

        '****** 28/01/2014 ปิดไว้ก่อน ตรงนี้ยังไม่ได้ทดสอบ ให้ทดสอบก่อนใช้ใหม่

        'Dim js As New JavaScriptSerializer()
        'Dim student = js.DeserializeObject(studentChange)

        'Dim StudentForChange As New ArrayList
        'Dim stuIdArr As New ArrayList 'ใช้เพื่อ check ว่ามีซ้ำกันหรือเปล่า เนื่องจาก StudentForChange เป็น array แบบ object เลยไม่สามารถ check ได้
        'For Each stu In student
        '    If (Not stuIdArr.Contains(stu("StudentId"))) Then
        '        stuIdArr.Add(stu("StudentId"))
        '        StudentForChange.Add(New With {.StudentId = stu("StudentId"), .Room = stu("StudentNewRoom"), .StudentStatus = stu("StundentTypeChange"), .StudentNoInNewRoom = stu("StudentNewNumber"), .studentClass = stu("StudentClass")})
        '    End If
        'Next

        'clearSessionVirtualRoom() ' claer session ห้องเรียนที่สร้างขึ้นมาลอยๆ

        'For i As Integer = 0 To StudentForChange.Count - 1
        '    'Dim StudentId = student(i)("StudentId")
        '    'Dim Room As String = student(i)("StudentNewRoom")
        '    'Dim StudentStatus As String = student(i)("StundentTypeChange")
        '    'Dim StudentNoInNewRoom As String = student(i)("StudentNewNumber")
        '    'Dim studentClass As String = student(i)("StudentClass")

        '    Dim StudentId = StudentForChange(i).StudentId
        '    Dim Room As String = StudentForChange(i).Room
        '    Dim StudentStatus As String = StudentForChange(i).StudentStatus
        '    Dim StudentNoInNewRoom As String = StudentForChange(i).StudentNoInNewRoom
        '    Dim studentClass As String = StudentForChange(i).studentClass

        '    Dim sql As String
        '    Dim DB As New ClassConnectSql
        '    ' update 
        '    If StudentStatus = "0" Or StudentStatus = "3" Then
        '        sql = "update t360_tblStudent set Student_Status = '" & StudentStatus & "' , Student_IsActive = '0',LastUpdate = dbo.GetThaiDate() where Student_Id = '" & StudentId & "'"
        '        DB.Execute(sql)

        '        sql = "update t360_tblStudent set IsActive = '0',LastUpdate = dbo.GetThaiDate() where Student_Id = '" & StudentId & "'"

        '        ' ย้ายห้อง
        '    ElseIf StudentStatus = "1" Then
        '        sql = "update t360_tblStudent set Student_CurrentRoom = '" & Room & "' ,Student_CurrentNoInRoom = '" & StudentNoInNewRoom & "',LastUpdate = dbo.GetThaiDate() where Student_Id = '" & StudentId & "'"
        '        DB.Execute(sql)
        '        Dim AcademicYear As String
        '        Dim ThisYear As Integer = CInt(Now.Year) + 543
        '        Dim ThisMonth As Integer = CInt(Now.Month)

        '        If ThisMonth > "3" Or ThisMonth = "3" Then
        '            AcademicYear = ThisYear.ToString
        '        Else
        '            AcademicYear = (ThisYear - 1).ToString
        '        End If
        '        Dim dt As New DataTable

        '        sql = "update t360_tblStudentRoom set SR_MoveType = '2',SR_IsActive = '0',LastUpdate = dbo.GetThaiDate() where Student_Id = '" & StudentId & "'"
        '        DB.Execute(sql)

        '        sql = "insert into t360_tblStudentRoom(Student_Id,School_Code,Student_NoInRoom,Class_Name,Room_Name,"
        '        sql &= " SR_MoveDate,SR_AcademicYear,SR_MoveType,SR_IsActive)"
        '        sql &= " values('" & StudentId & "','" & HttpContext.Current.Session("SchoolId") & "','" & StudentNoInNewRoom.ToString & "','" & Trim(studentClass) & "',"
        '        sql &= " '" & Room & "',dbo.GetThaiDate(),'" & AcademicYear & "','1','1')"
        '        DB.Execute(sql)
        '    End If
        'Next


        'Return "success"
    End Function

    <Services.WebMethod()>
    Public Shared Function setSessionNewRoom(ByVal newRoom As String)
        If (HttpContext.Current.Session("newRoom") Is Nothing Or HttpContext.Current.Session("newRoom") = "") Then
            HttpContext.Current.Session("newRoom") = newRoom
        ElseIf (HttpContext.Current.Session("newRoom") <> newRoom) Then
            HttpContext.Current.Session("newRoom") = HttpContext.Current.Session("newRoom") & "," & newRoom
        End If
        Return "F"
    End Function

    Private Shared Sub clearSessionVirtualRoom()
        HttpContext.Current.Session("newRoom") = Nothing
    End Sub

    Private Shared Function GetAcademicYear() As String

        Dim CurrentYear As Integer = Year(Now)
        Dim CurrentDate As New Date(Year(Now), Month(Now), Day(Now))
        Dim Fixdate As New Date(Year(Now), 3, 1)

        If DateValue(Fixdate) > DateValue(CurrentDate) Then
            CurrentYear -= 1
        End If

        If CurrentYear < 2400 Then
            CurrentYear += 543
        End If

        GetAcademicYear = CurrentYear.ToString()
        Return GetAcademicYear

    End Function
End Class