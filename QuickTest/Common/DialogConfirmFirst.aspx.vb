Public Class DialogConfirmFirst
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Msg = Request.QueryString("msg")
        Dim ListMsg() = Msg.Split(",")
        PartMsg.InnerText = ListMsg(0)
        HfMsg2.Value = ListMsg(1)
    End Sub

End Class