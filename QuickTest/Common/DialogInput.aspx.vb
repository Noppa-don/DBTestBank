Public Class DialogInput
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Msg() = Request.QueryString("msg").Split(",")
        Dim Title = Msg(0)
        Dim Head = Msg(1)
        Dim OldValue = Msg(2)
        Me.Title = Title
        lblHead.Text = Head
        TxtData.Text = OldValue
    End Sub

End Class