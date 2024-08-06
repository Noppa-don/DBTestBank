Imports System.Net.Mail
Imports BusinessTablet360

Namespace Email

    ''' <summary>
    ''' ใช้จัดการเกี่ยวกับระบบอีเมล
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageEmail

        ''' <summary>
        ''' ส่ง email
        ''' </summary>
        ''' <param name="Msg">รายละเอียดตัวอีเมล</param>
        ''' <param name="MailSetting">รายละเอียดการตั้งค่า SMTP</param>
        ''' <remarks></remarks>
        Public Function SendEmail(ByVal Msg As EmailDetail, ByVal MailSetting As SmtpSetting) As String
            ' สร้าง instance ของ SmtpClient เพื่อเชื่อมต่อ Mail Server ด้วย Protocol แบบ SMTP
            Dim SmtpClient As New SmtpClient
            SmtpClient.Host = MailSetting.Host
            SmtpClient.Port = MailSetting.Port
            'สร้าง instance ของ NetworkCredential สำหรับยืนยันสิทธิ์ในการเข้าใช้ระบบบริการอีเมล
            If MailSetting.UseAuthentication AndAlso (MailSetting.Username <> "") Then
                Dim credential As New Net.NetworkCredential
                credential.UserName = MailSetting.Username
                credential.Password = MailSetting.Password
                SmtpClient.Credentials = credential
            End If

            'กำหนดให้มีการส่งข้อมูลแบบมีการเข้ารหัสเพื่อความปลอดภัยด้วย SSL
            SmtpClient.EnableSsl = True

            Dim M As New MailMessage
            Dim MailFrom As String = Msg.EmailFrom
            Dim MailTo As String = Msg.EmailTo

            With M
                .From = New MailAddress(MailFrom)
                .To.Add(New MailAddress(MailTo))

                .SubjectEncoding = Text.Encoding.UTF8
                .Subject = Msg.Subject
                .BodyEncoding = Text.Encoding.UTF8
                .Body = Msg.Body
            End With

            ' Send the mail message
            Try
                SmtpClient.Send(M)
                Return ""
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return ex.Message
            End Try
        End Function

    End Class

    ''' <summary>
    ''' ใช้เก็บรายละเอีดยในอีเมลว่าต้องมีอะไรบ้าง เช่น ส่งให้ใคร, จากใคร
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EmailDetail

        Public EmailFrom As String
        Public EmailTo As String
        Public Subject As String
        Public Body As String

    End Class

    ''' <summary>
    ''' ใช้เก็บรายละเอียดการตั้า SMTP
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SmtpSetting

        Public Host As String
        Public Port As String
        Public Username As String
        Public Password As String
        Public UseAuthentication As Boolean = False

    End Class

End Namespace

