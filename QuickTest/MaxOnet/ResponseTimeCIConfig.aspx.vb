Public Class ResponseTimeConfig
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        showResponseTimeONLabel()
    End Sub

    Private Sub showResponseTimeONLabel()
        'lblResponseTime.Text = $"Response Time For CI = {MaxOnetCI.ResponseTime}"
    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTimeDefault.Click, btnTimeThirty.Click, btnTimeSixty.Click
        Dim responseTime As Integer = 0
        Dim btn As Button = sender
        If btn Is btnTimeThirty Then
            responseTime = 30
        ElseIf btn Is btnTimeSixty Then
            responseTime = 60
        End If
        MaxOnetCI.ResponseTime = responseTime
        showResponseTimeONLabel()
    End Sub
End Class