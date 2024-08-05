Public Class initial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If F5 = "1" Then
        If Service.ClsSystem.CheckIsLocalhost() = False Then
            Response.Redirect("~/Default.aspx")
        End If
#Else
        'If Service.ClsSystem.CheckIsCallFromIP("203.113.25.85") = False Then
        '    Response.Redirect("~/Default.aspx")
        'End If
#End If
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                'Response.Write("1")
                Dim passcode As String = Request.Form("passcode")
                If methodName.ToLower() = "resetstudent" Then
                    Response.Write("2")
                    Response.Write(passcode)
                    If Not IsNothing(passcode) Then
                        Response.Write("3")
                        If passcode = "confirmtodelete" Then
                            Response.Write("4")
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            ReturnValue = ClsDroidPad.Reset()
                            Response.Write("5")
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "resetmaxonet" Then
                    If Not IsNothing(passcode) Then
                        If passcode = "confirmtodelete" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            ReturnValue = ClsDroidPad.ResetMaxOnet()
                            Response.Write(ReturnValue)
                            Response.End()
                        Else
                            Response.Write("-1")
                        End If
                    End If
                End If
            End If
        End If


    End Sub

End Class