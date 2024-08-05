Public Class SelectTermControl
    Inherits System.Web.UI.UserControl
    Dim KnSession As New KNAppSession()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserID As String = Session("UserId").ToString()
        lblTerm.InnerHtml = KnSession.StoredValue("SelectedCalendarName").ToString()
    End Sub

End Class