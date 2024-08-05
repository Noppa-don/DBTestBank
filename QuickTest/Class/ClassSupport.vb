Imports System.Net
Imports System.Data.SqlClient

Public Class ClassSupport

    Public Shared Function CheckInternetConnection() As Boolean
        Dim url As String = "http://www.google.com"
        Try
            Dim myRequest As System.Net.WebRequest = System.Net.WebRequest.Create(url)
            Dim proxy As IWebProxy = GetProxy()
            'เช็คว่ามี set proxy หรือเปล่าโดยดูที่ tblSetEmail
            If proxy IsNot Nothing Then
                myRequest.Proxy = proxy
            End If
            Dim myResponse As System.Net.WebResponse = myRequest.GetResponse()
        Catch generatedExceptionName As System.Net.WebException
            Return False
        End Try
        Return True
    End Function

    Private Shared Function GetProxy() As IWebProxy
        If HttpContext.Current.Application("DefaultSchoolCode") Is Nothing Then
            Return Nothing
        End If
        Dim dt As DataTable = GetDTProxyDetails(HttpContext.Current.Application("DefaultSchoolCode"))
        'ไปหาข้อมูลจาก tblSetEmail ว่ามี set proxy ไว้หรือเปล่า
        If dt.Rows.Count > 0 Then
            Dim proxyIP As String = ""
            Dim Port As String = "80"
            If dt.Rows(0)("ProxyIP") IsNot DBNull.Value Then proxyIP = dt.Rows(0)("ProxyIP")
            If dt.Rows(0)("ProxyPort") IsNot DBNull.Value Then Port = dt.Rows(0)("ProxyPort")
            Dim currentProxy As New WebProxy(proxyIP & ":" & Port)
            'ถ้ามี set username , password เอาไว้ต้องเอามาสร้าง Credential ด้วย
            If dt.Rows(0)("ProxyUser") IsNot DBNull.Value AndAlso dt.Rows(0)("ProxyPassword") IsNot DBNull.Value Then SetProxyCredential(currentProxy, dt.Rows(0)("ProxyUser"), dt.Rows(0)("ProxyPassword"))
            Return currentProxy
        Else
            Return Nothing
        End If
    End Function

    Private Shared Sub SetProxyCredential(ByRef Proxy As WebProxy, UserName As String, Password As String)
        Proxy.Credentials = New NetworkCredential(UserName, Password)
    End Sub

    Private Shared Function GetDTProxyDetails(SchoolId As String) As DataTable
        Dim dt As New DataTable
        Dim db As New ClassConnectSql()
        Dim conn As New SqlConnection()
        db.OpenExclusiveConnect(conn)
        Try
            Dim sql As String = " SELECT ProxyIP,ProxyPort,ProxyUser,ProxyPassword FROM dbo.tblSetEmail WHERE SchoolId = '" & SchoolId & "' AND IsActive = 1; "
            dt = db.getdata(sql, , conn)
            db.CloseExclusiveConnect(conn)
            db = Nothing
            Return dt
        Catch ex As Exception
            db.CloseExclusiveConnect(conn)
            db = Nothing
            Return dt
        End Try
    End Function

End Class
