Public Class EmptySession
    Inherits System.Web.UI.Page
    Public DVID As String
    Public GroupName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DVID = Request.QueryString("DeviceId")

    End Sub

End Class