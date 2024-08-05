Public Class CheckCentralLabTabletRegisterStatus
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ไปเรียก Function CheckTabletIsRegistered() ของ Class DroidPad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            Dim DeviceId As String = Request.Form("DeviceId")
            If methodName IsNot Nothing Then
                If methodName.ToLower() = "needregister" Then
                    If DeviceId IsNot Nothing Then
                        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql())
                        If ClsDroidPad.CheckTabletIsRegistered(DeviceId) = True Then
                            Response.Write(1)
                            Response.End()
                        Else
                            Response.Write(0)
                            Response.End()
                        End If
                    Else
                        Response.Write(-1)
                        Response.End()
                    End If
                Else
                    Response.Write(-1)
                    Response.End()
                End If
            Else
                Response.Write(-1)
                Response.End()
            End If
        End If
    End Sub

End Class