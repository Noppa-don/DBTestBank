
Public Class DialogMsg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PartMsg.InnerText = Request.QueryString("msg")
    End Sub

End Class