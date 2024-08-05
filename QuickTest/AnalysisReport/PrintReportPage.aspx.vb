Imports System.IO
Imports iTextSharp.text.pdf

Public Class PrintReportPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Dim ClsAnalysisReport As New ClassAnalysisReport(New ClassConnectSql())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'Server.ScriptTimeout = 700
            Try
                BindDDLMonth()
            Catch ex As Exception
                HttpContext.Current.Response.Write(ex.ToString())
            End Try
        End If

    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        Dim SelectedDate As Date = #12/1/2013#
        Dim CheckInSertTmpSchool As String = ClsAnalysisReport.ProcessTmpSchool(txtSchoolId.Text.Trim(), SelectedDate)
        Dim CheckInsertStudent As String = ClsAnalysisReport.ProcessTmpStudent(txtSchoolId.Text.Trim(), SelectedDate)
        If CheckInSertTmpSchool = "Complete" And CheckInsertStudent = "Complete" Then
            Dim FolderName As String = CreateRandomFolder()
            Dim dt As DataTable = GetDtStudent(txtSchoolId.Text)
            If dt.Rows.Count > 0 Then
                Dim StudentFileName As String = ""
                Dim URL As String = "http://localhost:18615/AnalysisReport/SchoolReport.aspx?SchoolId=" & _DB.CleanString(txtSchoolId.Text) & "&Month=" & DDLMonth.SelectedValue & "&Year=" & _DB.CleanString(txtYear.Text) & "&FolderName=" & FolderName & "&IndexPage=" & "1/" & (dt.Rows.Count + 1)
                HttpGet(URL)
                For index = 0 To dt.Rows.Count - 1
                    StudentFileName = "P000" & (index + 1)
                    URL = "http://localhost:18615/AnalysisReport/ReportStudent.aspx?StudentId=" & dt.Rows(index)("Student_Id").ToString() & "&Month=" & DDLMonth.SelectedValue & "&Year=" & _DB.CleanString(txtYear.Text) & "&FileName=" & StudentFileName & "&FolderName=" & FolderName & "&IndexPage=" & (index + 2) & "/" & (dt.Rows.Count + 1)
                    HttpGet(URL)
                Next
                ProccessFolder(FolderName)
            End If
        Else
            Response.Write(CheckInSertTmpSchool)
        End If

    End Sub

#Region "CreateFolderRandomNumber"

    Private Function CreateRandomFolder()

        Dim FolderName As String = GetRandomNumber5Digit()
        Dim CheckFolderNameIsExist As Boolean = CheckFolderIsExist(FolderName)

        If CheckFolderNameIsExist = True Then
            Do Until CheckFolderNameIsExist = False
                FolderName = GetRandomNumber5Digit()
                CheckFolderNameIsExist = CheckFolderIsExist(FolderName)
            Loop
            Return FolderName
        Else
            Return FolderName
        End If

    End Function

    Private Function GetRandomNumber5Digit() As String

        Dim FolderName As String = ""
        Dim rand As Random = New Random()
        For index = 1 To 5
            FolderName &= rand.Next(0, 9).ToString()
        Next
        Return FolderName

    End Function

    Private Function CheckFolderIsExist(ByVal FolderName As String) As Boolean

        'Dim path As String = Server.MapPath("./PDF/" & FolderName)
        Dim path As String = "D:\data\tmp\AnalysisReport\PDF\" & FolderName
        Dim FolderIsExist As New DirectoryInfo(path)
        If FolderIsExist.Exists Then
            Return True
        Else
            FolderIsExist.Create()
            Return False
        End If

    End Function

#End Region

    Private Function HttpGet(ByVal url As String)
        Try
            Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
            'req.Timeout = 10000
            Dim resp As System.Net.WebResponse = req.GetResponse()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream())
            Return "Complete"
        Catch ex As Exception
            Return "Error"
        End Try
    End Function


    Private Function GetDtStudent(ByVal SchoolId As String) As DataTable
        Dim sql As String = " SELECT Student_Id FROM dbo.t360_tblStudent WHERE School_Code = '" & _DB.CleanString(SchoolId) & "' AND Student_IsActive = 1 " & _
                            " ORDER BY Student_CurrentClass,Student_CurrentRoom,Student_CurrentNoInRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Sub BindDDLMonth()

        DDLMonth.Items.Add("มกราคม")
        DDLMonth.Items(0).Value = 1

        DDLMonth.Items.Add("กุมภาพันธ์")
        DDLMonth.Items(1).Value = 2

        DDLMonth.Items.Add("มีนาคม")
        DDLMonth.Items(2).Value = 3

        DDLMonth.Items.Add("เมษายน")
        DDLMonth.Items(3).Value = 4

        DDLMonth.Items.Add("พฤษภาคม")
        DDLMonth.Items(4).Value = 5

        DDLMonth.Items.Add("มิถุนายน")
        DDLMonth.Items(5).Value = 6

        DDLMonth.Items.Add("กรกฏาคม")
        DDLMonth.Items(5).Value = 7

        DDLMonth.Items.Add("สิงหาคม")
        DDLMonth.Items(7).Value = 8

        DDLMonth.Items.Add("กันยายน")
        DDLMonth.Items(8).Value = 9

        DDLMonth.Items.Add("ตุลาคม")
        DDLMonth.Items(9).Value = 10

        DDLMonth.Items.Add("พฤศจิกายน")
        DDLMonth.Items(10).Value = 11

        DDLMonth.Items.Add("ธันวาคม")
        DDLMonth.Items(11).Value = 12

    End Sub

    Sub ProccessFolder(ByVal sFolderPath As String)

        Dim bOutputfileAlreadyExists As Boolean = False
        Dim oFolderInfo As New System.IO.DirectoryInfo(sFolderPath)
        'Dim Folderpath As String = Server.MapPath("./PDF/" & sFolderPath)
        Dim Folderpath As String = "D:\data\tmp\AnalysisReport\PDF\" & sFolderPath
        'Dim sOutFilePath As String = Server.MapPath("./PDF/" & sFolderPath & "/Complete.pdf")
        Dim sOutFilePath As String = "D:\data\tmp\AnalysisReport\PDF\" & sFolderPath & "\Complete.pdf"
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

End Class