Imports STROKESCRIBECLSLib

Public Class ProcessQRReport
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    '*****************************************************************
    Private tempFolderForFileGenerator As String = "../QRReport/tmpPDFReport"
    '*****************************************************************

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'ถ้าเป็นแบบเลือก Print ซ่อมเฉพาะนักเรียน
            If HttpContext.Current.Request.QueryString("StudentId") IsNot Nothing And HttpContext.Current.Request.QueryString("Position") IsNot Nothing Then
                Dim StudentId As String = _DB.CleanString(HttpContext.Current.Request.QueryString("StudentId").ToString().Trim())
                ProcessStudentOnly(StudentId, HttpContext.Current.Request.QueryString("Position").ToString())
            Else 'ถ้าเลือก Print ทั้งห้องต้องส่ง StudentID มา
                If HttpContext.Current.Request.QueryString("ClassName") IsNot Nothing And HttpContext.Current.Request.QueryString("RoomName") IsNot Nothing And HttpContext.Current.Request.QueryString("SchoolCode") IsNot Nothing Then
                    Dim ClassName As String = _DB.CleanString(HttpContext.Current.Request.QueryString("ClassName").ToString().Trim())
                    Dim RoomName As String = _DB.CleanString(HttpContext.Current.Request.QueryString("RoomName").ToString().Trim())
                    Dim SchoolCode As String = _DB.CleanString(HttpContext.Current.Request.QueryString("SchoolCode").ToString().Trim())
                    ProcessClassRoom(ClassName, RoomName, SchoolCode)
                End If
            End If
            _DB = Nothing
        End If
    End Sub

    Private Sub ProcessClassRoom(ByVal ClassName As String, ByVal RoomName As String, ByVal SchoolCode As String)
        Try
            Dim sql As String = " SELECT Student_Id,Student_FirstName,Student_LastName,Student_CurrentClass,Student_CurrentRoom,Student_Code,Student_CurrentNoInRoom " & _
                                " FROM dbo.t360_tblStudent WHERE Student_CurrentClass = '" & ClassName & "' AND Student_CurrentRoom = '" & RoomName & "' " & _
                                " AND School_Code = '" & SchoolCode & "' AND Student_IsActive = 1 ORDER BY Student_CurrentNoInRoom "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                'GenQRCode
                For index = 0 To dt.Rows.Count - 1
                    GenQRStudent(dt.Rows(index)("Student_Id").ToString(), dt.Rows(index)("Student_FirstName"), dt.Rows(index)("Student_LastName"), dt.Rows(index)("Student_CurrentClass"), dt.Rows(index)("Student_CurrentRoom"), dt.Rows(index)("Student_Code").ToString(), dt.Rows(index)("Student_CurrentNoInRoom"))
                Next

                'เปิดไฟล์ Template ขึ้นมาเพื่อ GenReport
                Dim InstanceReport As New Telerik.Reporting.UriReportSource()
                InstanceReport.Uri = HttpContext.Current.Server.MapPath("../QRReport/ReportTemplate/QRReportClass.trdx")
                InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentClass", ClassName))
                InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentRoom", RoomName))
                InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmSchoolCode", SchoolCode))

                'Gen(PDF)
                'Dim filename As String = GetNewFileName()
                Dim fileName As String = GenFileName()
                Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
                Dim deviceinfo As New System.Collections.Hashtable()
                Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
                Using fs As New System.IO.FileStream(filename, System.IO.FileMode.Create)
                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                    fs.Close()
                    fs.Dispose()
                End Using

                ''อันนี้จะเด้งให้เซฟที่ clientBrowser เลย
                Response.Buffer = False
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.ContentEncoding = System.Text.Encoding.UTF8
                Response.Charset = "utf-8"
                Response.ContentType = "application/pdf"    '  Response.ContentType = "application/octet-stream" ถ้ามีปัญหาอาจลองเปลี่ยนเป็น contentType ตัวนี้
                AddFileNameToHeader("QRRoom" + "_" + ClassName + RoomName + ".pdf") 'ต้องแปลงชื่อไม่งั้นมีปัญหากับ ภาษาไทย  
                Response.TransmitFile(fileName) ' ใช้ transmitfile จะไม่อ่านไฟล์ขึ้น memory ของ server

                ' ''อันนี้จะเปิดด้วย default behavior ของ แต่ละ client Browser ซึ่งอาจจะเด้งให้เซฟ, หรือ อาจจะแสดงผลใน browser's plug in 
                'Response.Redirect(tempFolderForFileGenerator + System.IO.Path.GetFileName(fileName))

                result = Nothing
                deviceinfo = Nothing
                reportProcessor = Nothing

                Response.End()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ProcessStudentOnly(ByVal StudentId As String, ByVal Position As Integer)
        Try
            Dim sql As String = " SELECT School_Code,Student_Id,Student_PrefixName,Student_FirstName,Student_LastName,Student_CurrentClass," & _
                                " SchoolName,Student_CurrentRoom,Student_Code" & _
                                " ,Student_CurrentNoInRoom,'../QRReport/QRStudent/' + CAST(Student_Id AS VARCHAR(50)) + '.jpg' AS PicName " & _
                                " FROM dbo.t360_tblStudent INNER JOIN dbo.tblSchool ON dbo.t360_tblStudent.School_Code = dbo.tblSchool.SchoolId " & _
                                " WHERE Student_Id = '" & StudentId & "' AND Student_IsActive = 1 "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                'GenQRCode
                GenQRStudent(dt.Rows(0)("Student_Id").ToString(), dt.Rows(0)("Student_FirstName"), dt.Rows(0)("Student_LastName"), dt.Rows(0)("Student_CurrentClass"), dt.Rows(0)("Student_CurrentRoom"), dt.Rows(0)("Student_Code").ToString(), dt.Rows(0)("Student_CurrentNoInRoom"))
                Dim dtComplete As DataTable = GetDtStudentOnly(dt, Position)
                
                'เปิดไฟล์ Template ขึ้นมาเพื่อ GenReport
                Dim settings = New System.Xml.XmlReaderSettings()
                settings.IgnoreWhitespace = True
                Dim instanceReport As Telerik.Reporting.InstanceReportSource
                Using xmlReader As System.Xml.XmlReader = System.Xml.XmlReader.Create(HttpContext.Current.Server.MapPath("../QRReport/ReportTemplate/QRReportStudent.trdx"), settings)
                    Dim xmlSerializer = New Telerik.Reporting.XmlSerialization.ReportXmlSerializer()
                    Dim report = DirectCast(xmlSerializer.Deserialize(xmlReader), Telerik.Reporting.Report)
                    'modify report
                    report.DataSource = dtComplete
                    instanceReport = New Telerik.Reporting.InstanceReportSource() With {.ReportDocument = report}
                End Using

                'เปิดไฟล์ Template ขึ้นมาเพื่อ GenReport
                'Dim InstanceReport As New Telerik.Reporting.UriReportSource()
                'InstanceReport.Uri = HttpContext.Current.Server.MapPath("../QRReport/ReportTemplate/QRReportStudent.trdx")
                'InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentStudentId", StudentId))
                'InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentStudentId", "select School_Code,Student_Code,Student_Id,Student_PrefixName,Student_FirstName,Student_LastName,Student_CurrentNoInRoom,Student_CurrentClass,Student_CurrentRoom,SchoolName,'../QRReport/QRStudent/' + CAST(Student_Id AS VARCHAR(50)) + '.jpg' AS PicName from t360_tblstudent INNER JOIN dbo.tblSchool ON dbo.t360_tblStudent.School_Code = dbo.tblSchool.SchoolId WHERE Student_IsActive = 1 AND Student_Id = " & StudentId & " "))

                'Gen(PDF)
                'Dim filename As String = GetNewFileName()
                Dim fileName As String = GenFileName(True)
                Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
                Dim deviceinfo As New System.Collections.Hashtable()
                Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
                Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
                    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                    fs.Close()
                    fs.Dispose()
                End Using

                ''อันนี้จะเด้งให้เซฟที่ clientBrowser เลย
                Response.Buffer = False
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.ContentEncoding = System.Text.Encoding.UTF8
                Response.Charset = "utf-8"
                Response.ContentType = "application/pdf"    '  Response.ContentType = "application/octet-stream" ถ้ามีปัญหาอาจลองเปลี่ยนเป็น contentType ตัวนี้
                AddFileNameToHeader("StudentOnly" + "_" + dt.Rows(0)("Student_FirstName") + ".pdf") 'ต้องแปลงชื่อไม่งั้นมีปัญหากับ ภาษาไทย  
                Response.TransmitFile(fileName) ' ใช้ transmitfile จะไม่อ่านไฟล์ขึ้น memory ของ server

                ' ''อันนี้จะเปิดด้วย default behavior ของ แต่ละ client Browser ซึ่งอาจจะเด้งให้เซฟ, หรือ อาจจะแสดงผลใน browser's plug in 
                'Response.Redirect(tempFolderForFileGenerator + System.IO.Path.GetFileName(fileName))

                result = Nothing
                deviceinfo = Nothing
                reportProcessor = Nothing

                Response.End()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        For i = 1 To 4
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

    Private Function GetNewFileName() As String
        Dim FileName As Integer = GetRndSeqNumber()
        Do Until System.IO.File.Exists(HttpContext.Current.Server.MapPath("../QRReport/tmpPDFReport/" & FileName.ToString() & ".pdf")) = False
            FileName = GetRndSeqNumber()
        Loop
        Return FileName.ToString()
    End Function

    Private Sub GenQRStudent(ByVal StudentId As String, ByVal FirstName As String, ByVal LastName As String, ByVal ClassName As String, ByVal RoomName As String, ByVal StudentCode As String, ByVal NoInRoom As String)
        Dim ss As New StrokeScribeClass
        Dim StudentDQR As String = "" '"สมรัก พรรคเพื่อเก้ง,ป.3/1,เลขที่ 1,โรงเรียน:1000001"
        If FirstName IsNot Nothing And FirstName <> "" Then
            StudentDQR &= "{" & FirstName & "},"
            If LastName IsNot Nothing And LastName <> "" Then
                StudentDQR &= "{" & LastName & "},"
                If ClassName IsNot Nothing And ClassName <> "" Then
                    StudentDQR &= "{" & ClassName & "},"
                    If RoomName IsNot Nothing And RoomName <> "" Then
                        StudentDQR &= "{" & RoomName & "},"
                        If StudentCode IsNot Nothing And StudentCode <> "" Then
                            StudentDQR &= "{" & StudentCode & "},"
                            If NoInRoom IsNot Nothing And NoInRoom <> "" Then
                                StudentDQR &= "{" & NoInRoom & "}"
                            End If
                        End If
                    End If
                End If
            End If
        End If
        ss.Alphabet = enumAlphabet.QRCODE
        ss.Text = StudentDQR
        ss.SavePicture(Server.MapPath("../QRReport/QRStudent/" & StudentId & ".jpg"), enumFormats.BMP, 100, 100)
    End Sub

    Private Function GenFileName(Optional ByVal IsStudentOnly As Boolean = False) As String
        Dim randomFileCode As String = RandomChar(5)
        Dim tmpFileName As String
        If IsStudentOnly = False Then
            tmpFileName = HttpContext.Current.Server.MapPath(tempFolderForFileGenerator + "/QRRoom" + randomFileCode + ".pdf")  'เวลา gen tempFile ให้ random เลข 5 หลัก มาแถมด้วย, เพื่อป้องกันปัญหา file lock 
        Else
            tmpFileName = HttpContext.Current.Server.MapPath(tempFolderForFileGenerator + "/StudentOnly" + randomFileCode + ".pdf")  'เวลา gen tempFile ให้ random เลข 5 หลัก มาแถมด้วย, เพื่อป้องกันปัญหา file lock 
        End If
        If System.IO.File.Exists(tmpFileName) Then
            Return GenFileName() 'ถ้าบังเอิญชื่อไฟล์ซ้ำกันจริงๆ ก็ต้อง gen ใหม่
        End If
        Return tmpFileName
    End Function

    Private Function RandomChar(charCount As Integer) As String
        Dim rnd As New Random(Now.Millisecond)

        Dim returnVal As String = ""
        If charCount > 0 Then
            For i = 0 To charCount - 1
                returnVal = returnVal + CStr(rnd.Next(0, 9))     ' ตัวเลข 0-9 เท่านั้น
            Next
        End If
        Return returnVal
    End Function

    Private Function AddFileNameToHeader(fileName As String)
        Dim contentDisposition As String
        If Request.Browser.Browser = "IE" AndAlso (Request.Browser.Version = "7.0" OrElse Request.Browser.Version = "8.0") Then
            contentDisposition = "attachment; filename=" & Uri.EscapeDataString(fileName)
        ElseIf Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.ToLowerInvariant().Contains("android") Then
            ' android built-in download manager (all browsers on android)
            contentDisposition = "attachment; filename=""" & MakeAndroidSafeFileName(fileName) & """"
        Else
            contentDisposition = "attachment; filename=""" & fileName & """; filename*=UTF-8''" & Uri.EscapeDataString(fileName)
        End If
        Response.AddHeader("Content-Disposition", contentDisposition)

    End Function

    Private Shared ReadOnly AndroidAllowedChars As Dictionary(Of Char, Char) = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._-+,@£$€!½§~'=()[]{}0123456789".ToDictionary(Function(c) c)
    Private Function MakeAndroidSafeFileName(fileName As String) As String
        Dim newFileName As Char() = fileName.ToCharArray()
        For i As Integer = 0 To newFileName.Length - 1
            If Not AndroidAllowedChars.ContainsKey(newFileName(i)) Then
                newFileName(i) = "_"c
            End If
        Next
        Return New String(newFileName)
    End Function

    Private Function GetDtStudentOnly(ByVal dt As DataTable, ByVal Position As Integer) As DataTable
        Dim dtComplete As DataTable = CreateDtStudentOnly()
        If dt.Rows.Count > 0 Then
            For index = 0 To 13
                If (index + 1) = Position Then
                    dtComplete.Rows.Add(index)("StudentName") = dt.Rows(0)("Student_PrefixName").ToString() & " " & dt.Rows(0)("Student_FirstName") & " " & dt.Rows(0)("Student_LastName")
                    dtComplete.Rows(index)("StudentDetail") = "เลขที่ " & dt.Rows(0)("Student_CurrentNoInRoom").ToString() & " ห้อง " & dt.Rows(0)("Student_CurrentClass") & dt.Rows(0)("Student_CurrentRoom") & " รหัสนร : " & dt.Rows(0)("Student_Code").ToString()
                    dtComplete.Rows(index)("SchoolDetail") = dt.Rows(0)("SchoolName").ToString() & " รหัส : " & dt.Rows(0)("School_Code")
                    dtComplete.Rows(index)("PicName") = "../QRReport/QRStudent/" & dt.Rows(0)("Student_Id").ToString() & ".jpg"
                Else
                    dtComplete.Rows.Add(index)("StudentName") = ""
                    dtComplete.Rows(index)("StudentDetail") = ""
                    dtComplete.Rows(index)("SchoolDetail") = ""
                    dtComplete.Rows(index)("PicName") = "../Images/Activity/BG_Opacity/whLayer90pct.gif"
                End If
            Next
        End If
        Return dtComplete
    End Function

    Private Function CreateDtStudentOnly() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("StudentName", GetType(String))
        dt.Columns.Add("StudentDetail", GetType(String))
        dt.Columns.Add("SchoolDetail", GetType(String))
        dt.Columns.Add("PicName", GetType(String))
        Return dt
    End Function

End Class