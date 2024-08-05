Public Class UserPage
    Inherits System.Web.UI.Page

    Dim clsSchool As New clsSchool
    Dim clsUser As New ClsUser
    Dim IsValidateData As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not (Request.QueryString("SchoolId") Is Nothing) Then
            If Not Page.IsPostBack Then
                Dim dtSchoolDetail As DataTable = clsSchool.GetSchoolDetail(Request.QueryString("SchoolId").ToString)
                If dtSchoolDetail.Rows.Count() > 0 Then
                    txtPreficSchool.Text = dtSchoolDetail.Rows(0)("SchoolShortName").ToString

                    If Not (Request.QueryString("EditId") Is Nothing) Then

                    Else
                        HeaderDetailText.InnerText = "เพิ่มข้อมูลผู้ใช้ สังกัดโรงเรียน : " & dtSchoolDetail.Rows(0)("SchoolName").ToString
                        txtPreficSchool.Text = clsSchool.GetPrefixSchool(Request.QueryString("SchoolId").ToString)
                        DetailDiv.Visible = False
                    End If
                End If
            End If
        Else
            Response.Redirect("~/LoginPage.aspx?")
        End If

    End Sub

#Region "CheckBox"
    Private Sub ChkAll_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAll.CheckedChanged

        Dim IsChecked As Boolean = True

        If ChkAll.Checked Then
            IsChecked = True
        Else
            IsChecked = False
        End If

        If radioP.Checked Or radioPM.Checked Then
            CheckedPrimaryControl(IsChecked)
        End If

        If radioM.Checked Or radioPM.Checked Then
            CheckedMiddleControl(IsChecked)
        End If

    End Sub

    Private Sub CheckedPrimaryControl(IsChecked As Boolean)
        For Each control In PrimaryPanel.Controls
            If TypeOf control Is CheckBox Then
                Dim chkbox As CheckBox = CType(control, CheckBox)
                chkbox.Checked = IsChecked
            End If
        Next
    End Sub
    Private Sub CheckedMiddleControl(IsChecked As Boolean)
        For Each control In MiddlePanel.Controls
            If TypeOf control Is CheckBox Then
                Dim chkbox As CheckBox = CType(control, CheckBox)
                chkbox.Checked = IsChecked
            End If
        Next
    End Sub

    Private Sub radioPM_CheckedChanged(sender As Object, e As EventArgs) Handles radioP.CheckedChanged, radioM.CheckedChanged, radioPM.CheckedChanged
        If radioP.Checked Then
            PrimaryPanel.Visible = True
            MiddlePanel.Visible = False
            If ChkAll.Checked Then
                CheckedPrimaryControl(True)
            Else
                CheckedPrimaryControl(False)
            End If
            CheckedMiddleControl(False)
        ElseIf radioM.Checked Then
            PrimaryPanel.Visible = False
            MiddlePanel.Visible = True
            If ChkAll.Checked Then
                CheckedMiddleControl(True)
            Else
                CheckedMiddleControl(False)
            End If
            CheckedPrimaryControl(False)
        ElseIf radioPM.Checked Then
            PrimaryPanel.Visible = True
            MiddlePanel.Visible = True
            If ChkAll.Checked Then
                CheckedPrimaryControl(True)
                CheckedMiddleControl(True)
            Else
                CheckedPrimaryControl(False)
                CheckedMiddleControl(False)
            End If
        End If
    End Sub

    Private Sub rdbAllSubject_CheckedChanged(sender As Object, e As EventArgs) Handles rdbAllSubject.CheckedChanged, rdbSelectSubject.CheckedChanged
        If rdbAllSubject.Checked Then
            DetailDiv.Visible = False
            btnOK.Visible = True
            'btnOK2.Visible = False
            radioPM.Checked = True
        ElseIf rdbSelectSubject.Checked Then
            DetailDiv.Visible = True
            btnOK.Visible = False
            'btnOK2.Visible = True
        End If
    End Sub
#End Region

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click ', btnOK2.Click
        If txtFirstName.Text <> "" And txtLastName.Text <> "" And txtPassword.Text <> "" And txtUserName.Text <> "" And txtPreficSchool.Text <> "" Then
            Dim PassDBOUser As DBOUser = KeepUserData()

            Dim NewUserId As String = clsUser.InsertUser(PassDBOUser)
            If NewUserId <> "False" Then
                If SaveUserSubjectClass(NewUserId) Then
                    CheckAndSetSchoolShortName(txtPreficSchool.Text, Request.QueryString("SchoolId").ToString)

                    ' AlertMessageAndRedirect("บันทึกข้อมูลเรียบร้อยค่ะ", "UserMainPage.aspx")
                Else
                    AlertMessage("เกิดข้อผิดพลาดในการบันทึกข้อมูล ลองอีกครั้งนะคะ")
                End If
            Else
                AlertMessage("เกิดข้อผิดพลาดในการบันทึกข้อมูล ลองอีกครั้งนะคะ")
            End If
        Else
            AlertMessage("กรอกข้อมูลไม่ครบถ้วน กรุณาตรวจสอบอีกครั้งค่ะ")
        End If

    End Sub

    Private Function KeepUserData() As DBOUser
        Dim DBUser As New DBOUser
        Dim FullPass As String = txtPreficSchool.Text & txtPassword.Text
        With DBUser
            .FirstName = txtFirstName.Text
            .LastName = txtLastName.Text
            .UserName = txtUserName.Text
            .Password = Encryption.MD5(FullPass)
            .PasswordChar = FullPass
            .IsContact = chkIsContact.Checked
            .SchoolId = Request.QueryString("SchoolId").ToString
        End With
        Return DBUser
    End Function

    Public Function SaveUserSubjectClass(UserId As String) As Boolean
        Try
            Dim cb As New CheckBox
            Dim strSubjectClass As String = ""
            For classIndex = 4 To 15
                For subjectIndex = 1 To 8
                    If rdbAllSubject.Checked = False Then
                        cb = Page.FindControl("C" & classIndex & "_" & subjectIndex)
                        If cb IsNot Nothing Then
                            If cb.Checked Then
                                clsUser.InsertUserSubjectClass(classIndex, subjectIndex, UserId)
                            End If
                        End If
                    Else
                        clsUser.InsertUserSubjectClass(classIndex, subjectIndex, UserId)
                    End If
                Next
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function CheckAndSetSchoolShortName(SchoolShortName As String, SchoolId As String) As Boolean
        Dim OldSchoolShortName As String = clsSchool.GetPrefixSchool(SchoolId)

        'ไม่มีของเดิม ให้ Update เลย
        '

        If OldSchoolShortName = "" Then
            'Update shortName
        Else
            If OldSchoolShortName <> SchoolShortName Then
                ConfirmMessageAndUpdateSchoolShortName("โรงเรียนนี้มีรหัสชื่อย่ออยู่แล้ว ถ้าต้องการเปลี่ยนจะทำให้ผู้ใช้ทั้งหมดถูกเปลี่ยนรหัสผ่านนะคะ", txtPreficSchool.Text)
            End If
        End If

    End Function

    Private Sub ConfirmMessageAndUpdateSchoolShortName(MessageStr As String, SchoolShortName As String)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type ='text/javascript'> window.onload = function() { var result = confirm('โรงเรียนนี้มีรหัสชื่อย่ออยู่แล้ว ถ้าต้องการเปลี่ยนจะทำให้ผู้ใช้ทั้งหมดถูกเปลี่ยนรหัสผ่านนะคะ');")
        sb.Append(" if (result == true) {$('#IsAllowUpdateSchoolShortName').val(True);}else{$('#IsAllowUpdateSchoolShortName').val(False);}};</script>")

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirm", sb.ToString())

    End Sub

    Private Sub AlertMessage(MessageStr As String)

        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload=function(){")

        sb.Append("alert('")

        sb.Append(MessageStr)

        sb.Append("')};")

        sb.Append("</script>")

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    Private Sub AlertMessageAndRedirect(MessageStr As String, URLRedirect As String)

        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")

        sb.Append("window.onload=function(){")

        sb.Append("if(!alert('")

        sb.Append(MessageStr)

        sb.Append("')) document.location = '")

        sb.Append(URLRedirect)

        sb.Append("'};")

        sb.Append("</script>")

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
    End Sub

    Private Sub IsAllowUpdateSchoolShortName_ValueChanged(sender As Object, e As EventArgs) Handles IsAllowUpdateSchoolShortName.ValueChanged
        Dim a As Integer
        If IsAllowUpdateSchoolShortName.Value = True Then
            a = 1
        Else
            a = 2
        End If
    End Sub

    <Services.WebMethod(EnableSession:=True)>
    Public Shared Function GetCurrentTime(InputKey As String) As String
        Dim a As Integer
        a = 5 + 1
        Dim b As Integer
        b = a + 1
        Return b.ToString
    End Function
End Class