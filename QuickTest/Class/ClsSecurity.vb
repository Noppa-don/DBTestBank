Public Class ClsSecurity
    Public Shared Sub CheckConnectionIsSecure()
#If F5 <> "1" Then
        'If HttpContext.Current.Request.IsSecureConnection() = False Then
        '    Throw New Exception("connection is insecurity")
        'End If
#End If
    End Sub
End Class
