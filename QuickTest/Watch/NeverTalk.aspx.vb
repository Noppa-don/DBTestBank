Public Class NeverTalk
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Response.Redirect("~/Watch/ChatSelectStudent.aspx?DeviceId=" & Request.QueryString("DeviceId").ToString())
    End Sub
End Class