Public Class MainSupportPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If ClassSupport.CheckInternetConnection() = True Then
                Response.Redirect("~/Support/PostErrorDetails.aspx")
            Else
                Response.Redirect("~/Support/ShowContactInfo.aspx")
            End If
        End If
    End Sub

End Class