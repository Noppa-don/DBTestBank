Public Class setEmailToAdmin
    Inherits System.Web.UI.Page
    Dim conDB As New ClassConnectSql
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim sql As String
        sql = "SELECT * FROM tblSetEmail"
        'conDB.Execute(Sql)
        Dim dt = conDB.getdata(sql)
        If dt.Rows.Count > 0 Then
            sql = "UPDATE tblSetEmail SET serverName = '" & txtServerName.Text & "', emailName = '" & txtEmail.Text & "'"
            conDB.Execute(sql)
            returnToMenu()
        Else
            sql = "INSERT INTO tblSetEmail(serverName, emailName) VALUES ('" & txtServerName.Text & "','" & txtEmail.Text & "');"
            conDB.Execute(sql)
            returnToMenu()
        End If
    End Sub

    Protected Sub lnkHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkHome.Click
        Log.Record(Log.LogType.Home, "กลับเมนูหลัก", True)
        Response.Redirect("MenuPage.aspx", False)
    End Sub

    Public Sub returnToMenu()
        Log.Record(Log.LogType.SetEmail, "ยืนยันการตั้งค่าอีเมล์", True)
        MsgBox("ตั้งค่าอีเมล์เรียบร้อยแล้ว", MsgBoxStyle.OkOnly, "ตั้งค่าอีเมล์")
        'Response.Redirect("MenuPage.aspx",false)
    End Sub
End Class