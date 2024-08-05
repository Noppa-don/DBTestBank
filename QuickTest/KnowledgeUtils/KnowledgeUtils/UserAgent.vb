Imports System.Web

Public Module UserAgent

    Public Function IsAndroid() As Boolean
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            Return True
        End If
        Return False
    End Function
End Module
