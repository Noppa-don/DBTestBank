Public Class ResetGroupSubjectPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        End If
        If HttpContext.Current.Session("UserName").ToString().ToLower() <> "admin" Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

End Class