Public Class MenuPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Session("IsAllowMenuManageUserSchool") = True Then
                btnUser.Visible = True
            Else
                btnUser.Visible = False
            End If
            If Session("IsAllowMenuManageUserAdmin") = True Then
                btnUserAdmin.Visible = True
            Else
                btnUserAdmin.Visible = False
            End If
            If Session("IsAllowMenuAdminLog") = True Then
                btnLogPage.Visible = True
            Else
                btnLogPage.Visible = False
            End If
            If Session("IsAllowMenuContact") = True Then
                btnQuestion.Visible = True
            Else
                btnQuestion.Visible = False
            End If
            If Session("IsAllowMenuSetEmail") = True Then
                btnSetEmail.Visible = True
            Else
                btnSetEmail.Visible = False
            End If
        End If
    End Sub

    Protected Sub btnUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUser.Click
            Log.Record(Log.LogType.ManageUser, "จัดการข้อมูลผู้ใช้", True)
        Response.Redirect("~/UserManager/UserManagerPage.aspx", False)
    End Sub


    Protected Sub btnLogPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogPage.Click
            Log.Record(Log.LogType.AdminLog, "จัดการข้อมูลใช้งาน", True)
        Response.Redirect("~/Admin/AdminLogPage.aspx", False)
    End Sub


    Protected Sub btnQuestion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQuestion.Click
        Log.Record(Log.LogType.AdminContact, "จัดการข้อมูลคำถามเพิ่มเติม", True)
        Response.Redirect("~/Admin/AdminContactPage.aspx", False)

    End Sub

    Protected Sub btnChangePassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChangePassword.Click
        Log.Record(Log.LogType.ChangePassword, "เปลี่ยนรหัสผ่าน", True)
        Response.Redirect("~/UserManager/UserChangePassword.aspx", False)
    End Sub

    Protected Sub btnUserAdmin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUserAdmin.Click
        Log.Record(Log.LogType.ManageUserAdmin, "จัดการข้อมูลผู้ใช้(Admin)", True)
        Response.Redirect("~/Admin/AdminManagePage.aspx", False)
    End Sub

    Protected Sub btnSetEmail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetEmail.Click
        Log.Record(Log.LogType.SetEmail, "ตั้งค่าอีเมล์", True)
        Response.Redirect("~/setEmailToAdmin.aspx", False)
    End Sub

    Protected Sub btnCreateEvalIndex_Click(sender As Object, e As EventArgs) Handles btnCreateEvalIndex.Click
        Log.Record(Log.LogType.SetEvaluationIndex, "กำหนดค่าดัชนีชี้วัด ตัวชี้วัด", True)
        Response.Redirect("~/testset/createevaluationindex.aspx", False)
    End Sub
End Class