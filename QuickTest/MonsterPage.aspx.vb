Public Class MonsterPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As New MonsterIDTemp
        m.CreateMonsterTest()
    End Sub

End Class