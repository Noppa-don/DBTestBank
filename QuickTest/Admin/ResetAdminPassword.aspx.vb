Public Class ResetAdminPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Service.ClsSystem.CheckIsLocalhost() = False Then
            Response.Redirect("~/Loginpage.aspx")
        End If
    End Sub

End Class