Public Class DialogTwoInput
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Msg() = Request.QueryString("msg").Split(",")
        Dim Title = Msg(0)
        Dim Head1 = Msg(1)
        Dim OldValue1 = Msg(2)
        Dim Head2 = Msg(3)
        Dim OldValue2 = Msg(4)
        Me.Title = Msg(0)
        lblHead1.Text = Head1
        TxtData1.Text = OldValue1
        lblHead2.Text = Head2
        TxtData2.Text = OldValue2
    End Sub

End Class