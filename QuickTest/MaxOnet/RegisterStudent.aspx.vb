Public Class RegisterStudent1
    Inherits System.Web.UI.Page
    Private db As New ClassConnectSql()
    Dim MaxOnet As New MaxOnetManagement
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim keycode As String = Request.QueryString("keycode").ToString()
            CheckAndSetOldUser(keycode)
        End If

    End Sub

    Private Sub CheckAndSetOldUser(keycode As String)

        db.OpenWithTransection()
        Try
            Dim sql As String = "select distinct ts.Student_Id,ts.Student_FirstName,ts.Student_LastName,ts.Student_Gender,case when ts.UserName is null then '' else ts.UserName end as userName,
                             case when ts.Password is null then '' else ts.Password end as Password,ts.Student_CurrentClass ,ts.Student_Phone
                             from t360_tblStudent ts inner join maxonet_tblStudentSubject ss on ts.Student_Id = ss.SS_StudentId 
                             where SS_KeyCode = '" & keycode & "'"
            Dim dtOldStudent As DataTable = db.getdataWithTransaction(sql)
            If dtOldStudent.Rows.Count <> 0 Then
                Session("IsRegistered") = True
                MaxOnet.StudentId = dtOldStudent.Rows(0)("Student_Id").ToString
                SetOldStudentData(dtOldStudent)
            Else
                Session("IsRegistered") = False
            End If
            db.CommitTransection()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Sub

    Private Function GetStudentId(keycode) As String
        db.OpenWithTransection()
        Dim UsId As String = ""
        Try
            Dim sql As String = "select distinct ts.Student_Id,ts.Student_FirstName,ts.Student_LastName,ts.Student_Gender,case when ts.UserName is null then '' else ts.UserName end as userName,
                             case when ts.Password is null then '' else ts.Password end as Password,ts.Student_CurrentClass ,ts.Student_Phone
                             from t360_tblStudent ts inner join maxonet_tblStudentSubject ss on ts.Student_Id = ss.SS_StudentId 
                             where SS_KeyCode = '" & keycode & "'"
            Dim dtOldStudent As DataTable = db.getdataWithTransaction(sql)
            If dtOldStudent.Rows.Count <> 0 Then
                UsId = dtOldStudent.Rows(0)("Student_Id").ToString
            End If
            db.CommitTransection()
            Return UsId
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Function

    Private Sub SetOldStudentData(dtStudent As DataTable)
        With dtStudent.Rows(0)
            txtFirstName.Value = dtStudent.Rows(0)("Student_FirstName").ToString
            txtLastName.Value = dtStudent.Rows(0)("Student_LastName").ToString
            txtTel.Value = dtStudent.Rows(0)("Student_Phone").ToString

            If dtStudent.Rows(0)("Student_Gender").ToString = "f" Then
                rdbGirl.Checked = True
            Else
                rdbBoy.Checked = True
            End If
            txtUser.Value = dtStudent.Rows(0)("userName").ToString
            txtPassword.Value = dtStudent.Rows(0)("Password").ToString

            If dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.1" Then
                rdbP1.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.2" Then
                rdbP2.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.3" Then
                rdbP3.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.4" Then
                rdbP4.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.5" Then
                rdbP5.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ป.6" Then
                rdbP6.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.1" Then
                rdbM1.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.2" Then
                rdbM2.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.3" Then
                rdbM3.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.4" Then
                rdbM4.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.5" Then
                rdbM5.Checked = True
            ElseIf dtStudent.Rows(0)("Student_CurrentClass").ToString = "ม.6" Then
                rdbM6.Checked = True
            End If
        End With
    End Sub

    Private Sub btnRegister_ServerClick(sender As Object, e As EventArgs) Handles btnRegister.ServerClick

        If GetStudentValue(MaxOnet) = "1" Then

            Dim ReturnValue As String = ""

            If Session("IsRegistered") Then
                ClsLog.Record("Update ข้อมูลนักเรียนใหม่จาก PC")
                MaxOnet.StudentId = GetStudentId(Request.QueryString("keycode").ToString())
                ReturnValue = MaxOnet.UpdateStudentMaxOnet()
                MaxOnet.NewTablet360PC()
                If ReturnValue.ToString = "-1" Then
                    MsgBox("ลงทะเบียนไม่สำเร็จค่ะ")
                Else
                    Response.Redirect("../practicemode_pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=" & Request.QueryString("DeviceId").ToString() & "&token=" & Request.QueryString("Token").ToString)
                End If
            Else
                ClsLog.Record("ลงทะเบียนนักเรียนใหม่จาก PC")
                ReturnValue = MaxOnet.RegisterStudentMaxOnet()
                Session("UserId") = MaxOnet.StudentId
                If ReturnValue.ToString = "-1" Then
                    MsgBox("ลงทะเบียนไม่สำเร็จค่ะ")
                Else
                    Response.Redirect("../practicemode_pad/DefaultMaxOnet.aspx?token=" & MaxOnet.TokenId.ToString & "&deviceUniqueId=" & MaxOnet.DeviceId.ToString)
                End If
            End If

        End If

    End Sub

    Private Function GetStudentValue(ByRef maxonet As MaxOnetManagement) As String
        Dim firstname As String = txtFirstName.Value.ToString
        If firstname = "" Then
            MsgBox("กรุณากรอกชื่อค่ะ")
            Return "-2"
        End If

        Dim LastName As String = txtLastName.Value.ToString
        If LastName = "" Then
            MsgBox("กรุณากรอกนามสกุลค่ะ")
            Return "-2"
        End If

        Dim Gender As String = ""

        If rdbBoy.Checked Then
            Gender = "m"
        Else
            Gender = "f"
        End If

        If Gender = "" Then
            MsgBox("กรุณาเลือกเพศค่ะ")
            Return "-2"
        End If

        Dim Tel As String = txtTel.Value.ToString
        If Tel = "" Then
            MsgBox("กรุณากรอกเบอร์โทรศัพท์ค่ะ")
            Return "-2"
        End If

        Dim UserName As String = txtUser.Value.ToString
        If UserName = "" Then
            MsgBox("กรุณากรอกชื่อผู้ใช้ค่ะ")
            Return "-2"
        End If

        Dim Password As String = txtPassword.Value.ToString
        If Password = "" Then
            MsgBox("กรุณากรอกรหัสผ่านค่ะ")
            Return "-2"
        End If

        Dim LevelClass As String = ""
        Dim LevelClassName As String = ""
        If rdbP1.Checked Then
            LevelClass = "k4"
            LevelClassName = "ป.1"
        ElseIf rdbP2.Checked Then
            LevelClass = "k5"
            LevelClassName = "ป.2"
        ElseIf rdbP3.Checked Then
            LevelClass = "k6"
            LevelClassName = "ป.3"
        ElseIf rdbP4.Checked Then
            LevelClass = "k7"
            LevelClassName = "ป.4"
        ElseIf rdbP5.Checked Then
            LevelClass = "k8"
            LevelClassName = "ป.5"
        ElseIf rdbP6.Checked Then
            LevelClass = "k9"
            LevelClassName = "ป.6"
        ElseIf rdbM1.Checked Then
            LevelClass = "k10"
            LevelClassName = "ม.1"
        ElseIf rdbM2.Checked Then
            LevelClass = "k11"
            LevelClassName = "ม.2"
        ElseIf rdbM3.Checked Then
            LevelClass = "k12"
            LevelClassName = "ม.3"
        ElseIf rdbM4.Checked Then
            LevelClass = "k13"
            LevelClassName = "ม.4"
        ElseIf rdbM5.Checked Then
            LevelClass = "k14"
            LevelClassName = "ม.5"
        ElseIf rdbM6.Checked Then
            LevelClass = "k15"
            LevelClassName = "ม.6"
        End If
        'LevelClass = "k10"
        'LevelClassName = "ม.1"
        If LevelClass = "" Then
            MsgBox("กรุณาเลือกชั้นค่ะ")
            Return "-2"
        End If

        Dim RoomName As String = txtRoom.Value.ToString.Replace("/", "").Replace(" ", "")
        If RoomName = "" Then
            MsgBox("กรุณากรอกห้องค่ะ")
            Return "-2"
        End If

        Dim RoomId As String = maxonet.GetRoomId(LevelClassName, RoomName)

        maxonet.StudentName = firstname
        maxOnet.StudentLastName = LastName
        maxonet.StudentClass = LevelClass
        maxonet.StudentClassName = LevelClassName
        maxonet.StudentPhone = Tel
        maxonet.UserName = UserName
        maxOnet.Password = Password
        maxOnet.StudentGender = Gender.ToString().Trim().ToLower()
        maxOnet.TokenId = Request.QueryString("Token").ToString
        maxonet.DeviceId = Request.QueryString("DeviceId").ToString()
        maxonet.StudentRoom = "/" & RoomName
        maxonet.StudentRoomId = RoomId
        maxonet.StudentSchool = HttpContext.Current.Application("DefaultSchoolCode").ToString


        ClsLog.Record("StudentName = " & firstname & " ,StudentLastName = " & LastName & " ,StudentClass = " & LevelClass &
              " ,StudentPhone = " & Tel & " ,StudentGender = " & Gender & " ,RoomName = " & RoomName)

        Return "1"
    End Function
End Class