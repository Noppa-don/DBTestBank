Public Class DefaultNoLogin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsLog.Record("PageLoad_DefaultNoLogin")
    End Sub



End Class