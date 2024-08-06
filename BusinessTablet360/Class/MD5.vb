Imports System.Security.Cryptography
Imports System.Text
Public Class Encryption

    Public Shared Function MD5(ByVal strPlainText As String) As String
        Dim ASCIIenc As New ASCIIEncoding
        Dim strReturn As String
        Dim ByteSourceText() As Byte = ASCIIenc.GetBytes(strPlainText)
        Dim Md5Hash As New MD5CryptoServiceProvider
        Dim ByteHash() As Byte = Md5Hash.ComputeHash(ByteSourceText)

        strReturn = ""

        For Each b As Byte In ByteHash
            strReturn = strReturn & b.ToString("x2")
        Next
        Return strReturn
    End Function
End Class
