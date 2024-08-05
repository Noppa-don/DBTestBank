Public Class LoginGenUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtvalidate.Visible = False
    End Sub

    Function ValidatePage() As Boolean

        If txtusername.Text = "" OrElse txtpassword.Text = "" Then
            txtvalidate.Visible = True
            Return False
        Else
            Return True
        End If


    End Function


    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles BtnSubmit.Click
        Session("IsLoggedIn") = Nothing
        If ValidatePage() Then
            Dim UserNametxt As String = txtusername.Text.ToLower
            Dim PasswordTxt As String = txtpassword.Text
            If ConfigurationManager.AppSettings("IsQuicktestProduction") = False Then
                If UserNametxt = "approvephuvit" And PasswordTxt = "network" Then
                    Session("IsLoggedIn") = "1"
                    Log.Record(Log.LogType.GenUser, "วิชาการเข้าระบบ", True)
                    Response.Redirect("~/QuickTestGenUser/GenUserManagerPage.aspx?")
                End If
            Else
                If (UserNametxt = "mkt" And PasswordTxt = "M4Kt2013#") Or (UserNametxt = "tippawan" And PasswordTxt = "K6M3b2016#") Or (UserNametxt = "chutikorn" And PasswordTxt = "S2F5w2016#") Then
                    Session("IsLoggedIn") = "1"
                    Log.Record(Log.LogType.GenUser, "การตลาดเข้าระบบ", True)
                    Response.Redirect("~/QuickTestGenUser/GenUserManagerPage.aspx?")

                End If
            End If

        End If
    End Sub
End Class