Public Class upgradeClass
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '28/01/2014 ปิดไว้ก่อน หน้านี้ยังไม่ได้ทดสอบ ให้ทดสอบก่อนใช้ใหม่

        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If
    End Sub

    Protected Sub btnUpgradeClass_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpgradeClass.Click
        Response.Redirect("../ClassUpgrade/changeClass.aspx")
    End Sub
End Class