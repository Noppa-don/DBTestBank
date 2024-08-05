Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text.pdf
Imports System.Net.Mail

Public Class SumUsageReport1
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Enum EnumDurationTime
        OneWeek = 7
        OneMonth = 30
        OneYear = 365
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            TruncateTmpTable()
            DeleteAllFileByFolderPath()
            Dim ConnReport As New SqlConnection
            _DB.OpenExclusiveConnect(ConnReport)

            'ย้อนหลัง 1 อาทิตย์
            GenDataReport(EnumDurationTime.OneWeek, ConnReport)

            'ย้อนหลัง 1 เดือน
            GenDataReport(EnumDurationTime.OneMonth, ConnReport)

            'ย้อนหลัง 1 ปี
            GenDataReport(EnumDurationTime.OneYear, ConnReport)

            'เริ่ม GenReport
            GenReportPDF(ConnReport)

            _DB.CloseExclusiveConnect(ConnReport)
            Response.Write("Complete")
        End If
    End Sub

    Private Sub GenDataReport(ByVal Duration As EnumDurationTime, ByRef ConnReport As SqlConnection)
        Try
            Dim CheckTypeWpp As Integer = 0
            Dim CheckTypeOtherUser As Integer = 0
            If Duration = EnumDurationTime.OneWeek Then
                CheckTypeWpp = 1
                CheckTypeOtherUser = 4
            ElseIf Duration = EnumDurationTime.OneMonth Then
                CheckTypeWpp = 2
                CheckTypeOtherUser = 5
            ElseIf Duration = EnumDurationTime.OneYear Then
                CheckTypeWpp = 3
                CheckTypeOtherUser = 6
            End If
            'ทำของ วพ. ก่อน
            ProcessDataTotmpReport(ConnReport, Duration, True, CheckTypeWpp)
            'ทำของ user ทั้งหมด
            ProcessDataTotmpReport(ConnReport, Duration, False, CheckTypeOtherUser)
        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Sub ProcessDataTotmpReport(ByRef ConnReport As SqlConnection, ByVal Duration As EnumDurationTime, ByVal IsWpp As Boolean, ByVal Type As Integer)
        Try
            _DB.OpenWithTransection(ConnReport)
            Dim dtWppUser As DataTable = GetDtUserId(ConnReport, IsWpp)
            If dtWppUser.Rows.Count > 0 Then
                'วนตาม UserId เพื่อ GenReport
                For z = 0 To dtWppUser.Rows.Count - 1
                    Dim dtEachUser As DataTable = GetDtLogDataByUserId(dtWppUser(z)("GUID").ToString(), ConnReport, Duration)
                    Dim LogFirstTime As New DateTime
                    Dim LogPreviousTime As New DateTime
                    Dim CheckMaxDifferenceTime As Long = 0
                    Dim CheckMinDifferenceTime As Long = 0
                    Dim TotalLogIn As Integer = 0
                    Dim TotalSpentTime As Double = 0
                    Dim AverageTime As Double = 0

                    Dim ZeroToFiveMinute As Integer = 0
                    Dim FiveToTenMinute As Integer = 0
                    Dim TenToFifteenMinute As Integer = 0
                    Dim FifteenToThirtyMinute As Integer = 0
                    Dim ThirtyPlusMinute As Integer = 0
                    For index = 0 To dtEachUser.Rows.Count - 1
                        'ทำการวนทีละ Row ของแต่ละ User เพื่อ Insert เข้า tmpReport01_UsageReport
                        If index = 0 Then
                            'เข้าครั้งแรกจับ TotalLogIn + 1 ก่อนเลย และเก็บค่าเวลา LogIn ครั้งแรกไว้ก่อน
                            TotalLogIn += 1
                            LogFirstTime = dtEachUser.Rows(index)("LastUpdate")
                            LogPreviousTime = dtEachUser.Rows(index)("LastUpdate")

                            'ถ้าเกิดว่ามันมี Row เดียวจะต้อง assign ค่าต่างๆให้ตัวแปร
                            If index = dtEachUser.Rows.Count - 1 Then
                                CheckMinDifferenceTime = 60
                                CheckMaxDifferenceTime = 60
                                TotalSpentTime = 60
                                ZeroToFiveMinute += 1
                            End If
                        Else
                            If dtEachUser.Rows(index)("LogType") = 1 Or index = dtEachUser.Rows.Count - 1 Then

                                If index <> dtEachUser.Rows.Count - 1 Or dtEachUser.Rows(dtEachUser.Rows.Count - 1)("LogType") = 1 Then
                                    'ถ้าเจอ Type เป็น 1 ก็แสดงว่า LogIn ครั้งต่อไปแล้ว
                                    TotalLogIn += 1
                                End If

                                Dim DiffenceTime As New Long
                                DiffenceTime = (DateDiff(DateInterval.Second, LogFirstTime, LogPreviousTime) + 60)

                                'หาว่าการ Login ครั้งนี้แตกต่างกันเท่าไหร่ อยู่ในช่วงเวลากลุ่มไหน
                                If (DiffenceTime / 60) > 30 Then
                                    ThirtyPlusMinute += 1
                                ElseIf (DiffenceTime / 60) > 15 And (DiffenceTime / 60) <= 30 Then
                                    FifteenToThirtyMinute += 1
                                ElseIf (DiffenceTime / 60) > 10 And (DiffenceTime / 60) <= 15 Then
                                    TenToFifteenMinute += 1
                                ElseIf (DiffenceTime / 60) > 5 And (DiffenceTime / 60) <= 10 Then
                                    FiveToTenMinute += 1
                                ElseIf (DiffenceTime / 60) >= 0 And (DiffenceTime / 60) <= 5 Then
                                    ZeroToFiveMinute += 1
                                End If

                                'หา MinTime
                                If CheckMinDifferenceTime = 0 Then
                                    CheckMinDifferenceTime = DiffenceTime
                                Else
                                    If DiffenceTime < CheckMinDifferenceTime Then
                                        CheckMinDifferenceTime = DiffenceTime
                                    End If
                                End If

                                'หา MaxTime
                                If CheckMaxDifferenceTime = 0 Then
                                    CheckMaxDifferenceTime = DiffenceTime
                                Else
                                    If DiffenceTime > CheckMaxDifferenceTime Then
                                        CheckMaxDifferenceTime = DiffenceTime
                                    End If
                                End If

                                'สุดท้ายต้องเก็บ LogFirstTime,LogPreviousTime เป็นเวลาที่ Log-In ครั้งใหม่นี้
                                LogFirstTime = dtEachUser.Rows(index)("LastUpdate")
                                LogPreviousTime = dtEachUser.Rows(index)("LastUpdate")

                                'ถ้าเป็นรอบสุดท้ายและเป็น Type 1 ต้องให้มันใช้เวลาในการ Login ครั้งนี้ = 1 นาที และเช็คเวลามากที่สุดกับน้อยที่สุดด้วย
                                If index = dtEachUser.Rows.Count - 1 And dtEachUser.Rows(dtEachUser.Rows.Count - 1)("LogType") = 1 Then
                                    ZeroToFiveMinute += 1
                                    'หา MinTime
                                    If CheckMinDifferenceTime = 0 Then
                                        CheckMinDifferenceTime = 60
                                    Else
                                        If 60 < CheckMinDifferenceTime Then
                                            CheckMinDifferenceTime = 60
                                        End If
                                    End If

                                    'หา MaxTime
                                    If CheckMaxDifferenceTime = 0 Then
                                        CheckMaxDifferenceTime = 60
                                    Else
                                        If 60 > CheckMaxDifferenceTime Then
                                            CheckMaxDifferenceTime = 60
                                        End If
                                    End If
                                    'ต้องเอา 1 นาทีของ Login ซึ่งเป็นครั้งสุดท้ายไปรวมไว้ในเวลารวมด้วย
                                    TotalSpentTime += 60
                                End If

                                'เอาเวลารวมกันไปเรื่อยๆเพื่อเป็น เวลารวมทั้งหมด ***** หน่วยมันเป็น วินาที สุดท้ายต้องทำให้เป็น ชม:นาที
                                TotalSpentTime += DiffenceTime
                            Else
                                LogPreviousTime = dtEachUser.Rows(index)("LastUpdate")
                            End If
                        End If
                    Next
                    If dtEachUser.Rows.Count > 0 Then
                        'Insert ลง tmpReport01_UsageReport
                        InsertInTmpReportUsage(ConnReport, dtWppUser(z)("UserName"), TotalLogIn, ChangeSecondToStringHourAndMinute(TotalSpentTime), GetAvgTime(TotalSpentTime, TotalLogIn), _
                                               ChangeSecondToStringMinuteAndSecond(CheckMinDifferenceTime), ChangeSecondToStringMinuteAndSecond(CheckMaxDifferenceTime), ZeroToFiveMinute, FiveToTenMinute, TenToFifteenMinute, _
                                              FifteenToThirtyMinute, ThirtyPlusMinute, Type)
                    End If
                Next
                _DB.CommitTransection(ConnReport)
            Else
                _DB.RollbackTransection(ConnReport)
            End If
        Catch ex As Exception
            _DB.RollbackTransection(ConnReport)
            Throw ex
        End Try
    End Sub

    Private Sub TruncateTmpTable()
        Try
            Dim sql As String = " TRUNCATE TABLE dbo.tmpReport01_UsageReport "
            _DB.Execute(sql)
        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try
    End Sub

    Private Function GetDtUserId(ByRef InputConn As SqlConnection, Optional ByVal IsWppUser As Boolean = False) As DataTable
        Dim dt As New DataTable
        Try
            Dim sql As String = ""
            If IsWppUser = True Then
                sql = " SELECT DISTINCT GUID,UserName FROM dbo.tblUser WHERE SchoolId = '1000000' AND IsActive = 1 and username <> 'cat' and username <> 'mai' and username <> 'shin' and username <> 'teacher' "
                dt = _DB.getdataWithTransaction(sql, , InputConn)
            Else
                sql = " SELECT DISTINCT GUID,UserName FROM dbo.tblUser WHERE SchoolId <> '1000000' AND IsActive = 1 and username <> 'cat' and username <> 'mai' and username <> 'shin' and username <> 'teacher' "
                dt = _DB.getdataWithTransaction(sql, , InputConn)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Private Function GetDtLogDataByUserId(ByVal UserId As String, ByRef InputConn As SqlConnection, ByVal Durationtime As EnumDurationTime) As DataTable
        Dim dt As New DataTable
        Try
            If UserId <> "" And UserId IsNot Nothing Then
                Dim sql As String = " SELECT LogType,LastUpdate FROM dbo.tblLog WHERE UserId = '" & UserId & "' AND IsActive = 1 " &
                                    " AND LastUpdate BETWEEN DATEADD(DAY,-" & Durationtime & ",dbo.GetThaiDate()) AND dbo.GetThaiDate() " &
                                    " ORDER BY LastUpdate "
                dt = _DB.getdataWithTransaction(sql, , InputConn)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Private Sub InsertInTmpReportUsage(ByRef InputConn As SqlConnection, ByVal UserName As String, ByVal TotalLogIn As Integer, ByVal TotalSpentTime As String _
                                       , ByVal AverageTime As String, ByVal MinTime As String, ByVal MaxTime As String, ByVal ZeroToFiveMinute As Integer, ByVal FiveToTenMinute As Integer _
                                       , ByVal TenToFifteenMinute As Integer, ByVal FifteenToThirtyMinute As Integer, ByVal ThirtyPlusMinute As Integer, ByVal Type As Integer)
        Try
            Dim sql As String = " INSERT INTO dbo.tmpReport01_UsageReport( UserName ,TotalLogIn ,TotalSpentTime ,AverageTime , " & _
                                " MinTime ,MaxTime ,ZeroToFive_Minute ,FiveToTen_Minute , TenToFifteen_Minute ,FifteenToThirty_Minute ,ThirtyPlus_Minute ,Type) " & _
                                " VALUES  ( '" & UserName & "' , " & TotalLogIn & " ,'" & TotalSpentTime & "' ,'" & AverageTime & "' , " & _
                                " '" & MinTime & "' , '" & MaxTime & "' , '" & ZeroToFiveMinute & "' ,'" & FiveToTenMinute & "' ,'" & TenToFifteenMinute & "' " & _
                                " ,'" & FifteenToThirtyMinute & "' ,'" & ThirtyPlusMinute & "' , '" & Type & "') "
            _DB.ExecuteWithTransection(sql, InputConn)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function ChangeSecondToStringHourAndMinute(ByVal InputSecond As Double) As String
        Dim iSpan As TimeSpan = TimeSpan.FromSeconds(InputSecond)
        Return iSpan.Hours.ToString().PadLeft(2, "0"c) & ":" & iSpan.Minutes.ToString().PadLeft(2, "0"c)
    End Function

    Private Function ChangeSecondToStringMinuteAndSecond(ByVal InputSecond As Double) As String
        'Dim iSpan As TimeSpan = TimeSpan.FromSeconds(InputSecond)
        Dim Second As Integer = InputSecond Mod 60
        Dim Minute As String = (InputSecond \ 60).ToString()
        'Return iSpan.Minutes.ToString().PadLeft(2, "0"c) & ":" & iSpan.Seconds.ToString().PadLeft(2, "0"c) & " นาที"
        Return Minute & ":" & Second.ToString()
    End Function

    Private Function GetAvgTime(ByVal TotalSpentTime As Double, ByVal TotalLogIn As Integer) As String
        Dim AvgTime As Double = TotalSpentTime / TotalLogIn
        Dim StrAvgTime As String = ChangeSecondToStringMinuteAndSecond(AvgTime)
        Return StrAvgTime
    End Function

    Private Sub GenReportPDF(ByRef InputConn As SqlConnection)
        Dim InCompanyUser As String = "เฉพาะ KN/CR/Sup/MC/WPP"
        Dim OtherUser As String = "เฉพาะลูกค้าโรงเรียน"
        GenPDFEachFile(InputConn, 1, InCompanyUser)
        GenPDFEachFile(InputConn, 2, InCompanyUser)
        GenPDFEachFile(InputConn, 3, InCompanyUser)
        GenPDFEachFile(InputConn, 4, OtherUser)
        GenPDFEachFile(InputConn, 5, OtherUser)
        GenPDFEachFile(InputConn, 6, OtherUser)
        ProccessFolder("Report_PDF")
        SendEmail(InputConn)
    End Sub

    Private Function GetDtUsageReportByType(ByVal Type As Integer, ByRef InputConn As SqlConnection) As DataTable
        Dim sql As String = " SELECT * FROM dbo.tmpReport01_UsageReport WHERE Type = " & Type & " ORDER BY TotalLogIn DESC , UserName; "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        Return dt
    End Function

    Private Function GetListOfClassUsageReport(ByVal dt As DataTable) As List(Of ClsSumUsageReport)
        Dim ListReport As New List(Of ClsSumUsageReport)
        For Each row In dt.Rows
            Dim ClsUsageReport As New ClsSumUsageReport
            ClsUsageReport.UserName = row("UserName")
            ClsUsageReport.TotalLogIn = row("TotalLogIn")
            ClsUsageReport.TotalSpentTime = row("TotalSpentTime")
            ClsUsageReport.AverageTime = row("AverageTime")
            ClsUsageReport.MinTime = row("MinTime")
            ClsUsageReport.MaxTime = row("MaxTime")
            ClsUsageReport.ZeroToFive_Minute = row("ZeroToFive_Minute")
            ClsUsageReport.FiveToTen_Minute = row("FiveToTen_Minute")
            ClsUsageReport.TenToFifteen_Minute = row("TenToFifteen_Minute")
            ClsUsageReport.FifteenToThirty_Minute = row("FifteenToThirty_Minute")
            ClsUsageReport.ThirtyPlus_Minute = row("ThirtyPlus_Minute")
            ListReport.Add(ClsUsageReport)
        Next
        Return ListReport
    End Function

    Private Sub GenPDFEachFile(ByRef InputConn As SqlConnection, ByVal Type As Integer, ByVal StringDetailType As String)
        Dim dt As DataTable = GetDtUsageReportByType(Type, InputConn)
        Dim InstanceReport As New Telerik.Reporting.InstanceReportSource
        Dim ListReport As List(Of ClsSumUsageReport) = GetListOfClassUsageReport(dt)
        Dim CurrentFileName As String = ""
        Dim TxtDetail As String = ""
        If Type = 1 Or Type = 4 Then
            TxtDetail = "ปริมาณการใช้งาน 1 สัปดาห์ย้อนหลัง (-7วัน ถึงวันนี้)"
            If Type = 1 Then
                CurrentFileName = "WppUserOneWeek"
            ElseIf Type = 4 Then
                CurrentFileName = "OtherUserOneWeek"
            End If
        ElseIf Type = 2 Or Type = 5 Then
            TxtDetail = "ปริมาณการใช้งาน 1 เดือนย้อนหลัง (-30วัน ถึงวันนี้)"
            If Type = 2 Then
                CurrentFileName = "WppUserOneMonth"
            ElseIf Type = 5 Then
                CurrentFileName = "OtherUserOneMonth"
            End If
        ElseIf Type = 3 Or Type = 6 Then
            TxtDetail = "ปริมาณการใช้งาน 1 ปีย้อนหลัง (-365วัน ถึงวันนี้)"
            If Type = 3 Then
                CurrentFileName = "WppUserOneYear"
            ElseIf Type = 6 Then
                CurrentFileName = "OtherUserOneYear"
            End If
        End If
        Dim CurrentDate As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.CreateSpecificCulture("TH"))
        InstanceReport.ReportDocument = New Report_SumUsageReport(ListReport, StringDetailType & "-" & "ดึงข้อมูล ณ วันที่ " & CurrentDate, TxtDetail)

        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim deviceinfo As New System.Collections.Hashtable()
        Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
        'Dim fileName As String = Server.MapPath("./Report_PDF/" & CurrentFileName & "." + result.Extension)
        Dim fileName As String = "D:\data\tmp\UsageReport\Report_PDF\" & CurrentFileName & "." + result.Extension
        Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
            fs.Close()
            fs.Dispose()
        End Using
        InstanceReport.ReportDocument.Dispose()
    End Sub

    Sub ProccessFolder(ByVal sFolderPath As String)

        Dim bOutputfileAlreadyExists As Boolean = False
        Dim oFolderInfo As New System.IO.DirectoryInfo(sFolderPath)
        'Dim Folderpath As String = Server.MapPath("./" & sFolderPath)
        Dim Folderpath As String = "D:\data\tmp\UsageReport\" & sFolderPath
        'Dim sOutFilePath As String = Server.MapPath("./" & sFolderPath & "/Complete_" & DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH")) & ".pdf")
        Dim sOutFilePath As String = "D:\data\tmp\UsageReport\" & sFolderPath & "\Complete_" & DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH")) & ".pdf"
        If IO.File.Exists(sOutFilePath) Then
            Try
                IO.File.Delete(sOutFilePath)
            Catch ex As Exception
                Response.Write(ex.ToString())
            End Try
        End If

        Dim iPageCount As Integer = GetPageCount(Folderpath)
        If iPageCount > 0 And bOutputfileAlreadyExists = False Then

            Dim oFiles As Array = New System.IO.DirectoryInfo(Folderpath).GetFiles.OrderBy(Function(x) x.CreationTime).Select(Function(q) q.FullName).ToArray
            Dim oPdfDoc As New iTextSharp.text.Document()
            Dim oPdfWriter As PdfWriter = PdfWriter.GetInstance(oPdfDoc, New FileStream(sOutFilePath, FileMode.Create))
            oPdfDoc.Open()

            For i As Integer = 0 To oFiles.Length - 1
                Dim sFromFilePath As String = oFiles(i).ToString()
                Dim oFileInfo As New FileInfo(sFromFilePath)

                Dim sExt As String = UCase(oFileInfo.Extension).Substring(1, 3)
                Try
                    AddPdf(sFromFilePath, oPdfDoc, oPdfWriter)
                Catch ex As Exception
                    Response.Write(ex.ToString())
                End Try

            Next

            Try
                oPdfDoc.Close()
                oPdfWriter.Close()
            Catch ex As Exception
                Response.Write(ex.ToString())
                Try
                    IO.File.Delete(sOutFilePath)
                Catch ex2 As Exception
                    Response.Write(ex2.ToString())
                End Try
            End Try

        End If

        Dim oFolders As String() = Directory.GetDirectories(Folderpath)
        For i As Integer = 0 To oFolders.Length - 1
            Dim sChildFolder As String = oFolders(i)
            Dim iPos As Integer = sChildFolder.LastIndexOf("\")
            Dim sFolderName As String = sChildFolder.Substring(iPos + 1)
            ProccessFolder(sChildFolder)
        Next

    End Sub

    Function GetPageCount(ByVal sFolderPath As String) As Integer
        Dim iRet As Integer = 0
        Dim oFiles As String() = Directory.GetFiles(sFolderPath)

        For i As Integer = 0 To oFiles.Length - 1
            Dim sFromFilePath As String = oFiles(i)
            Dim oFileInfo As New FileInfo(sFromFilePath)
            iRet += 1
        Next

        Return iRet
    End Function

    Sub AddPdf(ByVal sInFilePath As String, ByRef oPdfDoc As iTextSharp.text.Document, ByVal oPdfWriter As PdfWriter)

        Dim oDirectContent As iTextSharp.text.pdf.PdfContentByte = oPdfWriter.DirectContent
        Dim oPdfReader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(sInFilePath)
        Dim iNumberOfPages As Integer = oPdfReader.NumberOfPages
        Dim iPage As Integer = 0

        Do While (iPage < iNumberOfPages)
            iPage += 1
            oPdfDoc.SetPageSize(oPdfReader.GetPageSizeWithRotation(iPage))
            oPdfDoc.NewPage()

            Dim oPdfImportedPage As iTextSharp.text.pdf.PdfImportedPage = oPdfWriter.GetImportedPage(oPdfReader, iPage)
            Dim iRotation As Integer = oPdfReader.GetPageRotation(iPage)
            If (iRotation = 90) Or (iRotation = 270) Then
                oDirectContent.AddTemplate(oPdfImportedPage, 0, -1.0F, 1.0F, 0, 0, oPdfReader.GetPageSizeWithRotation(iPage).Height)
            Else
                oDirectContent.AddTemplate(oPdfImportedPage, 1.0F, 0, 0, 1.0F, 0, 0)
            End If
        Loop

    End Sub

    Private Sub SendEmail(ByRef InputConn As SqlConnection)
        Try
            Dim sql As String = " SELECT TOP 1 SMTPServerIP,SMTPServerPort,SMTPServerUser,SMTPServerPassword FROM dbo.tblSetEmail WHERE IsActive = 1 "
            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)
            If dt.Rows.Count > 0 Then
                ' default server
                Dim serverName As String = ""
                If dt.Rows(0)("SMTPServerIP") IsNot DBNull.Value Then
                    serverName = dt.Rows(0)("SMTPServerIP").ToString()
                End If

                Dim toEmail As String = "QTUsage@klnetwork.com"
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

                Dim SubjectMail As String = "QT AutoRpt #01, Usage Report, AsOf " & DateTime.Now.ToString("dd/MM/yyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH"))
                Dim BodyMail As String = "รายงานปริมาณการใช้งานย้อนหลัง ณ วันที่ " & DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH")) & " ตามไฟล์แนบ"

                mail.From = New MailAddress("quicktest-question@klnetwork.com") ' from email
                mail.Subject = SubjectMail
                mail.IsBodyHtml = True ' แทรกข้อความแบบ html
                mail.Body = BodyMail ' ข้อความที่ส่ง

                'Attachment
                'Dim FilePath As String = HttpContext.Current.Server.MapPath("./Report_PDF/Complete_" & DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH")) & ".pdf")
                Dim FilePath As String = "D:\data\tmp\UsageReport\Report_PDF\Complete_" & DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.CreateSpecificCulture("TH")) & ".pdf"
                Dim attach As Net.Mail.Attachment = New Attachment(FilePath)
                mail.Attachments.Add(attach)

                'Dim Smtp As New SmtpClient("smtp.gmail.com", 587) ' ip ของ server mail
                'Dim Smtp As New SmtpClient(serverName)
                Dim Smtp As New SmtpClient()
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
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub

    'Function ลบไฟล์ทั้งหมด
    Private Sub DeleteAllFileByFolderPath()
        'File.Delete("d:\temp\aa\FunctionExecute.au3")
        Try
            'Dim FolderPath As String = Server.MapPath("./Report_PDF")
            Dim FolderPath As String = "D:\data\tmp\UsageReport\Report_PDF"
            Dim s As String
            For Each s In Directory.GetFiles(FolderPath)
                File.SetAttributes(s, FileAttributes.Normal)
                File.Delete(s)
            Next
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub

#Region "Example"
    'Dim dt As New DataTable
    '    dt.Columns.Add("a")
    '    dt.Columns.Add("b")
    '    dt.Columns.Add("c")
    '    dt.Columns.Add("d")
    '    dt.Columns.Add("e")
    '    dt.Columns.Add("f")
    '    dt.Columns.Add("g")
    '    dt.Columns.Add("h")
    '    dt.Columns.Add("i")
    '    dt.Columns.Add("j")

    '    For index = 0 To 9
    '        dt.Rows.Add(index)("a") = index + 1
    '        dt.Rows(index)("b") = "b"
    '        dt.Rows(index)("c") = "c"
    '        dt.Rows(index)("d") = "d"
    '        dt.Rows(index)("e") = "e"
    '        dt.Rows(index)("f") = "f"
    '        dt.Rows(index)("g") = "g"
    '        dt.Rows(index)("h") = "h"
    '        dt.Rows(index)("i") = "i"
    '        dt.Rows(index)("j") = "j"
    '    Next

    '    For z = 0 To dt.Rows.Count - 1

    '    Next

    'Dim ListTest As New List(Of ClsSumUsageReport)

    '    For Each row In dt.Rows
    'Dim a As New ClsSumUsageReport
    '        a.UserName = row("a")
    '        a.TotalLogIn = row("b")
    '        a.TotalSpentTime = row("c")
    '        a.AverageTime = row("d")
    '        a.MinTime = row("e")
    '        a.MaxTime = row("f")
    '        a.ZeroToFive_Minute = row("g")
    '        a.FiveToTen_Minute = row("h")
    '        a.TenToFifteen_Minute = row("i")
    '        a.FifteenToThirty_Minute = row("j")
    '        a.ThirtyPlus_Minute = "dsdsd"
    '        ListTest.Add(a)
    '    Next

    'Dim InstanceReport As New Telerik.Reporting.InstanceReportSource
    '    InstanceReport.ReportDocument = New Report_SumUsageReport(ListTest)
    ''ReportViewer1.ReportSource = InstanceReport

    'Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
    'Dim deviceinfo As New System.Collections.Hashtable()
    'Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
    'Dim fileName As String = Server.MapPath("./abcdef/zxcrmvb." + result.Extension)
    '    Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
    '        fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
    '        fs.Close()
    '        fs.Dispose()
    '    End Using
    '    InstanceReport.ReportDocument.Dispose()
#End Region

End Class