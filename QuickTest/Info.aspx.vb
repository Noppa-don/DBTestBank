Imports System.Net.Mail
Imports System.Management
Imports System.Reflection

Public Class Info
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Protected InternetOK As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Service.ClsSystem.CheckIsLocalhost() = True Then
            If Not Page.IsPostBack Then
                lblFingerPrint.Text = "รหัสเครื่อง : " & ClsLanguage.GenerateMachineIdentification()
                If CheckInterNet() = True Then
                    DivInternetStatus.Style.Add("background", "rgb(31, 214, 31)")
                    InternetOK = "true"
                Else
                    DivInternetStatus.Style.Add("background", "red")
                    'btnOpenTeamViewer.Visible = False
                    InternetOK = "false"
                    btnSendEmail.Visible = False
                End If

                'แสดงจำนวนข้อสอบ
                Dim dtQuestions As DataTable = GetQuestionAmount()
                rptQuestionWppAmount.DataSource = dtQuestions.AsEnumerable().Where(Function(r) r.Field(Of Boolean)("IsWpp") = True)
                rptQuestionWppAmount.DataBind()

                rptQuestionAmount.DataSource = dtQuestions.AsEnumerable().Where(Function(r) r.Field(Of Boolean)("IsWpp") = False)
                rptQuestionAmount.DataBind()
            End If
        Else
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    Private Function CheckInterNet() As Boolean
        Dim url As String = "http://www.google.com"
        Try
            Dim myRequest As System.Net.WebRequest = System.Net.WebRequest.Create(url)
            Dim myResponse As System.Net.WebResponse = myRequest.GetResponse()
        Catch generatedExceptionName As System.Net.WebException
            Return False
        End Try
        Return True
    End Function

    'Private Sub btnOpenTeamViewer_Click(sender As Object, e As EventArgs) Handles btnOpenTeamViewer.Click
    '    'If System.IO.File.Exists("C:\Program Files (x86)\TeamViewer\Version9\TeamViewer.exe") = True Then
    '    Process.Start("C:\app\QuickTest\TeamViewer_9")
    '    'End If
    'End Sub

    Private Sub btnSendEmail_Click(sender As Object, e As EventArgs) Handles btnSendEmail.Click
        If CheckInterNet() = True Then
            Try
                Dim sql As String = " SELECT TOP 1 SMTPServerIP,SMTPServerPort,SMTPServerUser,SMTPServerPassword FROM dbo.tblSetEmail WHERE IsActive = 1 "
                Dim dt As New DataTable
                dt = _DB.getdata(sql)
                ' default server
                Dim serverName As String = ""
                If dt.Rows(0)("SMTPServerIP") IsNot DBNull.Value Then
                    serverName = dt.Rows(0)("SMTPServerIP").ToString()
                End If

                Dim toEmail As String = "support@iknow.co.th"
                Dim Port As Integer = 0
                If dt.Rows(0)("SMTPServerPort") <> 0 Then
                    Port = dt.Rows(0)("SMTPServerPort")
                End If

                Dim UserName As String = ""
                If dt.Rows(0)("SMTPServerUser") IsNot DBNull.Value And dt.Rows(0)("SMTPServerUser") <> "" Then
                    UserName = dt.Rows(0)("SMTPServerUser").ToString()
                End If

                Dim Password As String = ""
                If dt.Rows(0)("SMTPServerPassword") IsNot DBNull.Value And dt.Rows(0)("SMTPServerPassword") <> "" Then
                    Password = dt.Rows(0)("SMTPServerPassword").ToString()
                End If

                ' email
                Dim mail As New MailMessage
                mail.To.Add(New MailAddress(toEmail)) ' send to email

                Dim SubjectMail As String = "KEY : " & ClsLanguage.GetKeyIdDec() & " , " & "FingerPrint : " & ClsLanguage.GenerateMachineIdentification()
                Dim BodyMail As String = GetStrBodyEmail()

                mail.From = New MailAddress("quicktest-question@klnetwork.com") ' from email
                mail.Subject = SubjectMail
                mail.IsBodyHtml = True ' แทรกข้อความแบบ html
                mail.Body = BodyMail ' ข้อความที่ส่ง

                'Dim Smtp As New SmtpClient("smtp.gmail.com", 587) ' ip ของ server mail
                Dim Smtp As New SmtpClient()
                Smtp.EnableSsl = True
                If Port <> 0 Then
                    Smtp = New SmtpClient(serverName, Port)
                Else
                    Smtp = New SmtpClient(serverName)
                End If

                If UserName <> "" And Password <> "" Then
                    Smtp.Credentials = New System.Net.NetworkCredential(UserName.Trim(), Password.Trim())
                End If

                If serverName.IndexOf("74.125") <> -1 Or Port = 587 Or Port = 465 Or Port = 995 Then
                    Smtp.EnableSsl = True
                End If

                Smtp.Timeout = 2000
                Smtp.Send(mail)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Function GetStrBodyEmail() As String
        Dim BodyString As String = GetCpuUsageOfComputer() & "<br />" & GetRamOfComputer() & "<br />" & GetSizeofDiskDrive("c") & "<br />" & GetSizeofDiskDrive("d") & "<br />" & GetConnectionString() & "<br />" & GetQuickTestDllVersion()
        Return BodyString
    End Function

    Private Function GetRamOfComputer() As String
        Dim a As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")
        Dim TotalRam As Double
        Dim FreeRam As Double
        For Each queryObj As ManagementObject In a.[Get]()
            FreeRam = FormatNumber(queryObj("FreePhysicalMemory") / 1024 / 1024, 2)
            'UsageRam = FormatNumber(queryObj("FreeVirtualMemory") / 1024 / 1024, 2)
            'TotalRam = FormatNumber(queryObj("TotalVirtualMemorySize") / 1024 / 1024, 2)
            TotalRam = FormatNumber(queryObj("TotalVisibleMemorySize") / 1024 / 1024, 2)
        Next
        'Return "RAM : " & TotalRam - FreeRam & "/" & TotalRam & " (Free/Total)"
        Return "RAM : " & FreeRam & "/" & TotalRam & " MB (Free/Total)"
    End Function

    Private Function GetCpuUsageOfComputer() As String
        Try
            Using pc As New PerformanceCounter("Processor", "% Processor Time", "_Total")
                Return "CPU Usage : " & pc.NextValue.ToString() & "%"
            End Using
        Catch ex As Exception
            Try
                Using pc As New PerformanceCounter("Processor Information", "% Processor Time", "_Total")
                    Return "CPU Usage : " & pc.NextValue.ToString() & "%"
                End Using
            Catch ex2 As Exception
                Return "CPU USage : ไม่พบข้อมูล CPU"
            End Try
        End Try
    End Function

    Private Function GetSizeofDiskDrive(ByVal DriveName As String) As String
        Dim disk As New ManagementObject("win32_logicaldisk.deviceid=""" & DriveName.Trim() & ":""")
        disk.[Get]()
        Dim TotalSize As Integer = FormatNumber(disk("Size") / 1024 / 1024 / 1024, 0)
        Dim FreeSpace As Integer = FormatNumber(disk("FreeSpace") / 1024 / 1024 / 1024, 0)
        Return "Disk " & DriveName.ToUpper() & " : " & FreeSpace & "/" & TotalSize & " GB (Free/Total)"
    End Function

    Private Function GetConnectionString() As String
        Return "ConnectionString : " & ClsLanguage.GetConStr()
    End Function

    Private Function GetQuickTestDllVersion()
        Dim asm As Assembly = Assembly.GetExecutingAssembly()
        Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(asm.Location)
        Return "QuickTest_Version(dll) : " & [String].Format("{0}.{1}", fvi.ProductMajorPart, fvi.ProductMinorPart)
    End Function

    Private Function GetQuestionAmount() As DataTable
        Dim sql As New StringBuilder()
        sql.Append("select a.Level_ShortName,a.GroupSubject_ShortName,sum(a.QuestionAmount) as  amount,replace(convert(varchar,cast(sum(a.QuestionAmount) as money),1),'.00','') as  QuestionAmountTotal,a.IsWpp from ( ")
        sql.Append(" Select tbllevel.Level_ShortName,tblGroupSubject.GroupSubject_ShortName,count(tblquestion.Question_Id) as QuestionAmount,tblQuestion.IsWpp from tblquestion ")
        sql.Append(" inner join tblQuestionset on tblQuestion.QSet_Id = tblQuestionset.QSet_Id ")
        sql.Append(" inner join tblQuestionCategory on tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id ")
        sql.Append(" inner join tblBook on tblQuestionCategory.Book_Id = tblBook.BookGroup_Id inner join tbllevel on tblBook.Level_Id = tblLevel.Level_Id ")
        sql.Append(" inner join tblGroupSubject on tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id ")
        sql.Append(" where tblquestion.IsActive = '1' and tblQuestionset.IsActive = '1' and tblQuestionCategory.IsActive = '1' and tblBook.IsActive = '1' and tblquestionset.QSet_Type <> '3' ")
        sql.Append(" and tblbook.Book_Syllabus = '51' ")
        sql.Append(" group by tbllevel.Level_ShortName, tblGroupSubject.GroupSubject_ShortName,tblQuestion.IsWpp ")
        sql.Append(" union ")
        sql.Append(" Select tbllevel.Level_ShortName, tblGroupSubject.GroupSubject_ShortName, count(tblquestion.qset_id) as QuestionAmount,tblQuestion.IsWpp from tblquestion ")
        sql.Append(" inner join tblQuestionset on tblQuestion.QSet_Id = tblQuestionset.QSet_Id ")
        sql.Append(" inner join tblQuestionCategory on tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id  ")
        sql.Append(" inner join tblBook on tblQuestionCategory.Book_Id = tblBook.BookGroup_Id inner join tbllevel on tblBook.Level_Id = tblLevel.Level_Id ")
        sql.Append(" inner join tblGroupSubject on tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id ")
        sql.Append(" where tblquestion.IsActive = '1' and tblQuestionset.IsActive = '1' and tblQuestionCategory.IsActive = '1' and tblBook.IsActive = '1' and tblquestionset.QSet_Type = '3' ")
        sql.Append(" And tblbook.Book_Syllabus = '51' ")
        sql.Append(" group by tbllevel.Level_ShortName, tblGroupSubject.GroupSubject_ShortName,tblQuestion.IsWpp) a ")
        sql.Append(" group by Level_ShortName, GroupSubject_ShortName,IsWpp ")
        sql.Append(" order by Level_ShortName,amount;")
        Return _DB.getdata(sql.ToString())
    End Function

End Class