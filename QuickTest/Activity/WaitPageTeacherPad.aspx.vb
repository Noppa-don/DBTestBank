Public Class WaitPageTeacherPad
    Inherits System.Web.UI.Page
    Public GroupName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GroupName = Session("selectedSession").ToString() 'GroupName

        Dim ClsSelectedSession As New ClsSelectSession
        ClsSelectedSession.checkCurrentPage(Session("UserId").ToString(), GroupName)
    End Sub

End Class