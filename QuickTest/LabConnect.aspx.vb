Imports BusinessTablet360

Public Class LabConnect
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form("EncryptRoomName") IsNot Nothing Then
            Try
                Dim ClientIP As String = ServiceSystem.GetIPAddress()
                Dim DecryptRoomName As String = SimpleEncrypt.Decrypt(Request.Form("EncryptRoomName").ToString().Trim())
                'เก็บเข้า Redist
                Dim ClsRedis As New KnowledgeUtils.RedisStore()
                ClsRedis.SetKey(ClientIP, DecryptRoomName)
                ClsRedis = Nothing
            Catch ex As Exception

            End Try
        End If
    End Sub

End Class