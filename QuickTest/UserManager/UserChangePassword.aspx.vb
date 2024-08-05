Public Class UserChangePassword
    Inherits System.Web.UI.Page

    Dim conDB As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            getData()
        End If
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        'Dim wc = getData()
        'If txtOldPassword.Text = wc Then

        '        
        '        lblCheck.Visible = False
        '        lblCheckPassword.Visible = False
        '        Log.Record(Log.LogType.ChangePassword, "ยืนยันการเปลี่ยนรหัสผ่าน", True)
        '        MsgBox("เปลี่ยนรหัสผ่านเรียบร้อยแล้ว", MsgBoxStyle.OkOnly, "แจ้งเตือน")
        '        'Response.Redirect("~/MenuPage.aspx",false)

        '    Else
        '        lblCheckPassword.Visible = True
        '        lblCheck.Visible = False
        '    End If
        'Else
        '    lblCheck.Visible = True
        'End If
        If txtNewPassword.Text <> "" And txtNewPasswordConfirm.Text <> "" And txtOldPassword.Text <> "" Then
            Dim sql As String = "select count(*) from tbluser where userId = '" & Session("UserId").ToString() & "' and password = '" & Encryption.MD5(txtOldPassword.Text) & "';"
            If conDB.ExecuteScalar(sql) <= 0 Then
                MsgBox("รหัสผ่านเก่าไม่ถูกต้อง", MsgBoxStyle.OkOnly, "ตรวจสอบก่อนค่ะ")
            Else

                sql = "UPDATE tblUser SET Password = '" & Encryption.MD5(txtNewPassword.Text) & "' WHERE UserID =  '" & Session("UserId").ToString() & "'"
                conDB.Execute(sql)
                Log.Record(Log.LogType.ChangePassword, "ยืนยันการเปลี่ยนรหัสผ่าน", True)
                MsgBox("เปลี่ยนรหัสผ่านเรียบร้อยแล้ว", MsgBoxStyle.OkOnly, "แจ้งเตือน")
            End If

        Else
            MsgBox("กรุณากรอกข้อมูลให้ครบถ้วน", MsgBoxStyle.OkOnly, "แจ้งเตือน")
        End If

    End Sub

    Public Function getData() As String
        Dim username As String = ""
        Dim schoolId As String = ""
        Dim oldPassword As String = ""

        Dim sql As String = "SELECT SchoolID,UserName,Password FROM tblUser WHERE UserID = '" & Session("UserId") & "'"
        'conDB.Execute(sql)
        Dim dt = conDB.getdata(sql)

        If dt.Rows.Count > 0 Then
            username = dt(0)("UserName")
            oldPassword = dt(0)("Password")
            schoolId = dt(0)("SchoolID")
        End If

        lblUserName.Text = username
        lblSchoolID.Text = schoolId

        Return oldPassword
    End Function

    Protected Sub lbtnHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHome.Click
        Log.Record(Log.LogType.Home, "กลับเมนูหลัก", True)
        Response.Redirect("~/MenuPage.aspx", False)
    End Sub

    Function getPassword() As String
        Dim oldPassword As String = "dat"
        Dim sql As String = "SELECT Password FROM tblUser WHERE UserID = '" & Session("UserId") & "'"
        Dim dt = conDB.getdata(sql)
        If (dt.Rows.Count > 0) Then
            oldPassword = dt(0)("Password")
        End If
        Return oldPassword
    End Function
End Class