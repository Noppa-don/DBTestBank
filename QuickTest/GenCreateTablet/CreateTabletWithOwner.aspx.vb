Public Class CreateTabletWithOwner
    Inherits System.Web.UI.Page
    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnSaveTeacher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveTeacher.Click

        Dim DeviceId As String = Guid.NewGuid().ToString()
        Dim TeacherName As String = txtTeacherName.Text
        Dim TeacherLastname As String = txtTeacherLastName.Text
        Dim Room As String = txtTeacherRoom.Text
        Dim SchoolId As String = txtSchoolCode.Text
        Dim SchoolPass As String = txtSchoolPass.Text

        If TeacherName = "" Or TeacherLastname = "" Or Room = "" Or SchoolId = "" Or SchoolPass = "" Then
            Response.Write("ต้องใส่ข้อมูลให้ครบครับ")
            Exit Sub
        End If

        Dim StrClass As String = ""
        Dim StrSubject As String = ""
        Dim cb As New CheckBox
        For index = 1 To 12
            cb = Page.FindControl("ChkTC" & index)
            If cb IsNot Nothing Then
                If index = 1 Then
                    If cb.Checked = True Then
                        StrClass = "|K4"
                    End If
                ElseIf index = 2 Then
                    If cb.Checked = True Then
                        StrClass &= "|K5"
                    End If
                ElseIf index = 3 Then
                    If cb.Checked = True Then
                        StrClass &= "|K6"
                    End If
                ElseIf index = 4 Then
                    If cb.Checked = True Then
                        StrClass &= "|K7"
                    End If
                ElseIf index = 5 Then
                    If cb.Checked = True Then
                        StrClass &= "|K8"
                    End If
                ElseIf index = 6 Then
                    If cb.Checked = True Then
                        StrClass &= "|K9"
                    End If
                ElseIf index = 7 Then
                    If cb.Checked = True Then
                        StrClass &= "|K10"
                    End If
                ElseIf index = 8 Then
                    If cb.Checked = True Then
                        StrClass &= "|K11"
                    End If
                ElseIf index = 9 Then
                    If cb.Checked = True Then
                        StrClass &= "|K12"
                    End If
                ElseIf index = 10 Then
                    If cb.Checked = True Then
                        StrClass &= "|K13"
                    End If
                ElseIf index = 11 Then
                    If cb.Checked = True Then
                        StrClass &= "|K14"
                    End If
                ElseIf index = 12 Then
                    If cb.Checked = True Then
                        StrClass &= "|K15"
                    End If
                End If
            End If
        Next


        For z = 1 To 8
            Dim cbsj As New CheckBox
            cbsj = Page.FindControl("ChkJubject" & z)
            If cbsj IsNot Nothing Then
                If z = 1 Then
                    If cbsj.Checked = True Then
                        StrSubject = "|S1"
                    End If
                ElseIf z = 2 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S2"
                    End If
                ElseIf z = 3 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S3"
                    End If
                ElseIf z = 4 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S4"
                    End If
                ElseIf z = 5 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S5"
                    End If
                ElseIf z = 6 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S6"
                    End If
                ElseIf z = 7 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S7"
                    End If
                ElseIf z = 8 Then
                    If cbsj.Checked = True Then
                        StrSubject &= "|S8"
                    End If
                End If
            End If
        Next

        StrClass = StrClass.Substring(1, StrClass.Length - 1)
        StrSubject = StrSubject.Substring(1, StrSubject.Length - 1)
    

        'ลงทะเบียนเครื่องกับโรงเรียน
        If ClsDroidPad.GetRegistrationInfo(DeviceId, SchoolId, SchoolPass) = "-1" Then
            Response.Write("รหัสผ่านผิด")
            Exit Sub
        End If

        'ลงทะเบียนครู
        If ClsDroidPad.GetTeacherInfo(DeviceId, TeacherName, TeacherLastname, StrClass, Room, StrSubject) = "-1" Then
            Response.Write("Insert Teacher ไม่ได้")
            Exit Sub
        End If

        Response.Redirect("~/GenCreateTablet/ShowTabletOwnerInfo.aspx")

    End Sub

    Private Sub btnSaveStudent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveStudent.Click

        Dim StudentClass As String = ""
        Dim DeviceId As String = Guid.NewGuid().ToString()
        Dim SchoolId As String = txtSchoolCode.Text
        Dim SchoolPass As String = txtSchoolPass.Text
        Dim StudentName As String = txtStudentName.Text
        Dim StudentLastName As String = txtStudentLastName.Text()
        Dim Room As String = txtStudentRoom.Text()
        Dim StudentCode As String = txtStudentCode.Text
        Dim StudentNoInRoom As String = txtStudentNoInRoom.Text()

        If SchoolId = "" Or SchoolPass = "" Or StudentName = "" Or StudentLastName = "" Or Room = "" Or StudentCode = "" Or StudentNoInRoom = "" Then
            Response.Write("ต้องใส่ข้อมูลให้ครบครับ")
            Exit Sub
        End If

        If rd1.Checked = True Then
            StudentClass = "K4"
        ElseIf rd2.Checked = True Then
            StudentClass = "K5"
        ElseIf rd3.Checked = True Then
            StudentClass = "K6"
        ElseIf rd4.Checked = True Then
            StudentClass = "K7"
        ElseIf rd5.Checked = True Then
            StudentClass = "K8"
        ElseIf rd6.Checked = True Then
            StudentClass = "K9"
        ElseIf rd7.Checked = True Then
            StudentClass = "K10"
        ElseIf rd8.Checked = True Then
            StudentClass = "K11"
        ElseIf rd9.Checked = True Then
            StudentClass = "K12"
        ElseIf rd10.Checked = True Then
            StudentClass = "K13"
        ElseIf rd11.Checked = True Then
            StudentClass = "K14"
        ElseIf rd12.Checked = True Then
            StudentClass = "K15"
        End If

        'ลงทะเบียนเครื่องนี้กับ โรงเรียน
        If ClsDroidPad.GetRegistrationInfo(DeviceId, SchoolId, SchoolPass) = "-1" Then
            Response.Write("Insert Tablet กับ รร.นี้ไม่ได้")
            Exit Sub
        End If

        'ลงทะเบียนกับนักเรียน
        If ClsDroidPad.GetStudentInfo(DeviceId, StudentName, StudentLastName, StudentClass, Room, StudentCode, StudentNoInRoom, "M") = "-1" Then
            Response.Write("Insert นักเรียนไม่ได้")
            Exit Sub
        End If

        Response.Redirect("~/GenCreateTablet/ShowTabletOwnerInfo.aspx")

    End Sub

    

    Private Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Response.Redirect("~/GenCreateTablet/ShowTabletOwnerInfo.aspx")
    End Sub

End Class