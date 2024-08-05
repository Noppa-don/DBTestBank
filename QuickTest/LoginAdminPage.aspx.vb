Public Class LoginAdminPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("FromLoginAdminPage") = "1"
        Response.Redirect("loginpage.aspx", False)
    End Sub

End Class