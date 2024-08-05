Imports System.Configuration
Imports System.Collections
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Text
'Imports EvoPdf.HtmlToPdf
Imports System.Drawing
Imports System.IO
Imports WebSupergoo.ABCpdf8

Public Class GenPDF
    Inherits System.Web.UI.Page
    'NA
    Dim ClsPDF As New ClsPDF(New ClassConnectSql)
    'NA
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    'NA
    Public GroupName As String
    'class ที่เอาไว้จัดการเกี่ยวกับการ Gen File Word
    Dim ClsGenWord As New ClsGenWord(New ClassConnectSql)
    'เก็บ TestsetId เอาไว้ใช้ทั้งหน้า
    Private TestsetID As String
    Public IE As String

    Private EnablePrefixForRunningNoInWordFile As Boolean = CBool(ConfigurationManager.AppSettings("EnablePrefixForRunningNoInWordFile"))

    ''' <summary>
    ''' ทำการเก็บ log , หา TestsetName
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
#End If
        Log.Record(Log.LogType.PageLoad, "pageload genpdf", False)
        If Session("UserId") = Nothing Then
            Log.Record(Log.LogType.PageLoad, "genpdf session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        End If

#If IE = "1" Then
        'TestsetID = "740DA94D-A040-4E58-83FC-C7BB197EB08C"
        TestsetID = "35b760eb-9b1e-488c-8899-e2f91ed7463b"
#Else
        Dim PageName As String = System.IO.Path.GetFileName(Request.Url.ToString())
        Log.Record(Log.LogType.PageLoad, PageName, True)

        TestsetID = Request.QueryString("TestsetID")
#End If

        If Not IsPostBack Then
            If Not TestsetID Is Nothing Then
                Session("newTestSetId") = TestsetID
                Dim ClsTestset As New ClsTestSet(Session("UserId").ToString())
                Dim dtTestset As DataTable = ClsTestset.GetTestsetName(TestsetID)

                txtTestsetName.Text = dtTestset.Rows(0)("testset_name").ToString().Replace("<br />", "").Replace("<br>", "").Replace("<br/>", "")

                txtTestsetTime.Text = dtTestset.Rows(0)("testset_time").ToString()
                ddlTextSize.SelectedValue = dtTestset.Rows(0)("Level_Id").ToString().ToUpper()

                If EnablePrefixForRunningNoInWordFile Then
                    If Not dtTestset.Rows(0)("PrefixForRunningNoInWordFile") Is DBNull.Value Then
                        txtPrefixRunningNo.Text = dtTestset.Rows(0)("PrefixForRunningNoInWordFile").ToString()
                    End If
                End If

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) Then
                    If dtTestset.Rows(0)("NeedConnectCheckmark") = True Then
                        chkUseCheckmark.Checked = True
                    End If
                End If
                
            End If
        End If
        'GroupName = Session("selectedSession").ToString() 'GroupName
        'HttpContext.Current.Application("NeedQuizMode") = "True"

        'NA
        If HttpContext.Current.Application("NeedQuizMode") = True Then
            ChkFontSize = "16px"
            txtStep1 = "ขั้นที่ 1: จัดชุดควิซ -->"
            txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
            txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
            txtStep4 = "ขั้นที่ 4: บันทึกชุดควิซ -->"
        Else
            ChkFontSize = "20px"
            txtStep1 = "ขั้นที่ 1: จัดชุด -->"
            txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
            txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
            txtStep4 = "ขั้นที่ 4: บันทึก และจัดพิมพ์"
        End If

        'Protected Sub btnGenKey_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenKey.Click
        '    Session("IsAnswerSheet") = True
        '    Session("IsPreviewPage") = False
        '    GenPDFFile()
        'End Sub

        'Protected Sub btnGenPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenPDF.Click
        '    Session("IsAnswerSheet") = False
        '    Session("IsPreviewPage") = False
        '    GenPDFFile()
        'End Sub

        'Protected Sub btnGenPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenPreview.Click
        '    Session("IsAnswerSheet") = False
        '    Session("IsPreviewPage") = True
        '    GenPDFFile()
        'End Sub
        'If Not Page.IsPostBack() Then
        '    Dim tmpFileName As String = GenTempFileName()

        '    Dim ClsSelectSess As New ClsSelectSession
        '    'check session ก่อนว่าเคยสร้าง pdf รียัง
        '    ClsSelectSess.checkCurrentPage(Session("UserId").ToString(), Session("selectedSession").ToString())

        '    If Session("OutputFileName") = "" Then

        '        Session("IsAnswerSheet") = False
        '        Session("IsPreviewPage") = True
        '        GenPDFFile(tmpFileName, "-preview")

        '        Session("IsAnswerSheet") = False
        '        Session("IsPreviewPage") = False
        '        GenPDFFile(tmpFileName, "")

        '        Session("IsAnswerSheet") = True
        '        Session("IsPreviewPage") = False
        '        GenPDFFile(tmpFileName, "-answer")

        '        Session("OutputFileName") = tmpFileName

        '    End If


        'End If

    End Sub


    ''' <summary>
    ''' button ตกลง update testset และ Checkmark ในกรณีที่เลือกให้เป็นการใช้กับ checkmark
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim TestsetName As String = txtTestsetName.Text()
        Dim TestsetTime As String = txtTestsetTime.Text()
        Dim Level_Id As String = ddlTextSize.SelectedValue.ToString()
        Dim NeedConnectCheckmark As Integer
        If chkUseCheckmark.Checked Then
            NeedConnectCheckmark = 1
            Try
                Dim clsCheckmark As New ClsCheckMark()
                Dim setupName As String = String.Format("{0} ({1})", TestsetName, Date.Now.ToString("dd/MM/yy HH:mm"))
                Dim qAmount As Integer = GetQuestionAmountInTestset()
                Dim className As String = GetClassNameFromQset()
                'ไม่แน่ใจว่าทำอะไร
                clsCheckmark.saveQuizToCheckmark(setupName, "Wpp02 QR 120 Choice", qAmount, className, TestsetID)
                'ไม่แน่ใจว่าทำอะไร
                clsCheckmark.InsertRefToCheckMarkIntblCM(TestsetID)
                'ไม่แน่ใจว่าทำอะไร
                ClsCheckMark.saveAllCorrectAnswerToCheckmark()
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        Else
            NeedConnectCheckmark = 0
        End If

        Dim PrefixForRunningNoInWordFile As String = If(EnablePrefixForRunningNoInWordFile, txtPrefixRunningNo.Text, "")

        Dim ClsTestset As New ClsTestSet(Session("UserId").ToString())

        ClsTestset.UpdateTestset(TestsetID, TestsetName, TestsetTime, Level_Id, NeedConnectCheckmark, PrefixForRunningNoInWordFile, EnablePrefixForRunningNoInWordFile)

        divLightbox.Style.Add("display", "none")
        setTestset.Style.Add("display", "none")
    End Sub

    ''' <summary>
    ''' หาชั้นจาก Testset ที่เลือกมา
    ''' </summary>
    ''' <returns>ชั้นแบบเป็นชื่อย่อ เช่น ป.1,ม.4</returns>
    ''' <remarks></remarks>
    Private Function GetClassNameFromQset() As String
        Dim db As New ClassConnectSql()
        Dim sql As New StringBuilder()
        sql.Append(" SELECT TOP 1 l.Level_ShortName FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionset qs ON tsqs.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id ")
        sql.Append(" INNER JOIN tblLevel l ON b.Level_Id = l.Level_Id WHERE tsqs.IsActive = 1 AND tsqs.TestSet_Id = '" & TestsetID & "' ORDER BY l.Level DESC; ")
        Return db.ExecuteScalar(sql.ToString())
    End Function

    ''' <summary>
    ''' หาจำนวนข้อสอบจากชุดที่เลือกมา
    ''' </summary>
    ''' <returns>จำนวนข้อสอบ</returns>
    ''' <remarks></remarks>
    Private Function GetQuestionAmountInTestset() As Integer
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(td.TSQD_Id) as qAmount FROM tblTestSetQuestionSet ts LEFT JOIN tblTestSetQuestionDetail td ON ts.TSQS_Id = td.TSQS_Id WHERE ts.TestSet_Id = '" & TestsetID & "' And ts.IsActive = '1' and td.IsActive = '1';"
        Return CInt(db.ExecuteScalar(sql))
    End Function

    ''' <summary>
    ''' button ยกเลิก กลับไปยังหน้าที่พึ่งมา
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Not Request.QueryString("FromDashboard") Is Nothing Then
            Response.Redirect("~/PrintTestset/DashboardPrintTestsetPage.aspx?cancle=t")
        Else
            Response.Redirect("~/PracticeMode_Pad/ChooseQuestionSet.aspx")
        End If
    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="readStream"></param>
    ''' <param name="writeStream"></param>
    ''' <remarks></remarks>
    Private Sub ReadWriteStream(ByVal readStream As Stream, ByVal writeStream As Stream)
        Dim Length As Integer = 256
        Dim buffer As [Byte]() = New [Byte](Length - 1) {}
        Dim bytesRead As Integer = readStream.Read(buffer, 0, Length)
        ' write the required bytes
        While bytesRead > 0
            writeStream.Write(buffer, 0, bytesRead)
            bytesRead = readStream.Read(buffer, 0, Length)
        End While
        readStream.Close()
        writeStream.Close()
    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenTempFileName() As String
        Dim r As New Random(System.DateTime.Now.Millisecond)
        Dim newFileName As String = ""
        '65-71 = A-G
        newFileName = Chr(r.Next(65, 71))
        For i = 1 To 6
            newFileName = newFileName + r.Next(1, 9).ToString()
        Next
        Return newFileName 'ผลลัพธ์จะเป็น ตัวภาษาอังกฤษ1ตัว, ตามด้วยตัวเลข 6 หลัก
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="tmpFileName"></param>
    ''' <param name="fileSuffixName"></param>
    ''' <remarks></remarks>
    Public Sub GenPDFFile(ByVal tmpFileName As String, ByVal fileSuffixName As String)


        'get the html string for the report
        Dim htmlStringWriter As New StringWriter()
        Server.Execute("ExamTemplate.aspx", htmlStringWriter)
        Dim htmlCodeToConvert As String = htmlStringWriter.GetStringBuilder().ToString()
        htmlStringWriter.Close()


        'Dim tmpFolder As String = HttpContext.Current.Application("TempFolderForPDF").ToString()
        Dim tmpFolder As String = "D:\data\tmp\GenWord\"

        'Dim fileSuffixName As String = ""
        'If Session("IsAnswerSheet") = True Then
        '    fileSuffixName = "-answer"
        'ElseIf Session("IsPreviewPage") = True Then
        '    fileSuffixName = "-preview"
        'End If

        Dim saveTo As String = tmpFolder + tmpFileName + fileSuffixName + ".html"

        Dim writeStream As FileStream = New FileStream(saveTo, FileMode.Create, FileAccess.Write)
        Dim readStream As New MemoryStream(System.Text.Encoding.Default.GetBytes(htmlCodeToConvert))

        ReadWriteStream(readStream, writeStream)

        Dim theDoc As Doc = New Doc()
        'Dim a As XHtmlOptions = theDoc.HtmlOptions
        'a.BreakMethod = HtmlBreakMethodType.CumulativeCohesion

        'theDoc.MediaBox.String = "40 40 595 842"
        theDoc.MediaBox.String = "40 40 555 802"
        theDoc.Rect.String = theDoc.MediaBox.String


        theDoc.CropBox.String = "50 50 545 792"

        Dim theID As Integer
        Dim pageIdx As Integer = 1
        Dim txt As String
        theID = theDoc.AddImageUrl("file://" + saveTo)

        theDoc.Pos.X = 530
        theDoc.Pos.Y = 785

        txt = "P." + pageIdx.ToString()
        theDoc.AddText(txt)
        pageIdx = pageIdx + 1

        While True
            theDoc.FrameRect()
            If Not theDoc.Chainable(theID) Then
                Exit While
            End If
            theDoc.Page = theDoc.AddPage()

            theID = theDoc.AddImageToChain(theID)

            theDoc.Pos.X = 530
            theDoc.Pos.Y = 785
            txt = "P." + pageIdx.ToString()
            theDoc.AddText(txt)

            pageIdx = pageIdx + 1

        End While
        For i As Integer = 1 To theDoc.PageCount
            theDoc.PageNumber = i
            theDoc.Flatten()
        Next


        theDoc.Save(tmpFolder + tmpFileName + fileSuffixName + ".pdf")
        theDoc.Clear()




        'theDoc.HtmlOp()
    End Sub

    'Protected Sub btnGenKey_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenKey.Click
    '    Session("IsAnswerSheet") = True
    '    Session("IsPreviewPage") = False
    '    Log.Record(Log.LogType.GenPDF, "สร้างไฟล์เฉลยข้อสอบ", True)
    '    GenPDFFile()

    'End Sub

    'Protected Sub btnGenPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenPDF.Click
    '    Session("IsAnswerSheet") = False
    '    Session("IsPreviewPage") = False
    '    Log.Record(Log.LogType.GenPDF, "สร้างไฟล์ข้อสอบ", True)
    '    GenPDFFile()
    'End Sub

    'Protected Sub btnGenPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenPreview.Click
    '    Session("IsAnswerSheet") = False
    '    Session("IsPreviewPage") = True
    '    Log.Record(Log.LogType.GenPDF, "สร้างไฟล์ตัวอย่างข้อสอบ", True)
    '    GenPDFFile()
    'End Sub

    ''' <summary>
    ''' ทำการ Gen ไฟล์ Word ในแบบ Preview คือแสดงแค่บางส่วน เพื่อเป็นตัวอย่าง
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        ' ClsGenWord.GenNewDocument(ClsGenWord.GenType.SomeExam)
        ClsGenWord.GenIndexIndicationsDocument()
    End Sub

    ''' <summary>
    ''' ทำการ Gen ไฟล์ Word ในแบบ Full
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        ClsGenWord.GenNewDocument(ClsGenWord.GenType.FullExam)
    End Sub

    ''' <summary>
    ''' ทำการ Gen ไฟล์ PDF ข้อสอบ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnDownloadQuestionSheetPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadQuestionSheetPDF.Click
        ClsGenWord.GenNewPDF(ClsGenWord.GenType.FullExam)
    End Sub

    ''' <summary>
    ''' ทำการ Gen ไฟล์ Word ในแบบให้มีการโชว์เฉลยด้วย
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAnswerSheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnswerSheet.Click, btnDownloadAnswerSheetPDF.Click
        If sender Is btnDownloadAnswerSheetPDF Then
            ClsGenWord.GenNewPDF(ClsGenWord.GenType.ShowCorrectAnswer)
        Else
            ClsGenWord.GenNewDocument(ClsGenWord.GenType.ShowCorrectAnswer)
        End If
    End Sub


    Private Sub btnAnwserSheetWithExplain_Click(sender As Object, e As EventArgs) Handles btnAnwserSheetWithExplain.Click, btnDownloadAnwserSheetWithExplainPDF.Click
        Dim btnSender As Button = sender
        If btnSender Is btnDownloadAnwserSheetWithExplainPDF Then
            ClsGenWord.GenNewPDF(ClsGenWord.GenType.ShowCorrectAnswerWithExplain)
        Else
            ClsGenWord.GenNewDocument(QuickTest.ClsGenWord.GenType.ShowCorrectAnswerWithExplain)
        End If
    End Sub

    ''' <summary>
    ''' ปุ่มถอยกลับ กลับไปหน้าก่อนหน้านี้
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        If Request.QueryString("FromDashboard") IsNot Nothing Then
            Response.Redirect("~/PrintTestset/DashboardPrintTestsetPage.aspx")
        Else
            Response.Redirect("~/PracticeMode_Pad/ChooseQuestionset.aspx")
        End If
    End Sub

   
End Class