Public Class KeyCodeExpiredPage
    Inherits System.Web.UI.Page
    Protected TokenId As String
    Protected DeviceId As String
    Protected SubjectsIdStr As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TokenId = Request.QueryString("TokenId")
        DeviceId = Request.QueryString("DeviceId")
        SubjectsIdStr = Request.QueryString("SubjectsIdStr")
    End Sub

End Class