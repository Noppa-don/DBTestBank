Public Class Azure

    'ถ้า run in Azure จะ มี ตัวแปร ชื่อ WEBSITE_SITE_NAME นี้
    Public Shared Function IsRunInAzure() As Boolean
        Return Not [String].IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"))
    End Function

End Class
