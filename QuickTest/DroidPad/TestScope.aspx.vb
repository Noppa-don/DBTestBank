Public Class TestScope
    Inherits System.Web.UI.Page
    Dim UseCls As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("AllowRegisteredTabletOwnerToLogIn") = "True"
        'Session("_SchoolID") = "1000008"
    End Sub

End Class