Imports System.Configuration
Imports System.Collections
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Text
Imports EvoPdf.HtmlToPdf
Imports System.Drawing
Imports System.IO

Public Class TestGenQuestion
    Inherits System.Web.UI.Page
    Dim ClsPDF As New ClsPDF(New ClassConnectSql)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If
    End Sub

    Public Sub GenPDFFile()

        ' get the html string for the report
        Dim htmlStringWriter As New StringWriter()
        Server.Execute("ExamTemplate.aspx?id=7A91A026-5E4D-46F2-ABBE-E3CCE69AFEDF", htmlStringWriter)
        Dim htmlCodeToConvert As String = htmlStringWriter.GetStringBuilder().ToString()
        htmlStringWriter.Close()

        'initialize the PdfConvert object
        Dim pdfConverter As New PdfConverter()

        ' set the license key - required
        pdfConverter.LicenseKey = "ORIJGQoKGQkZCxcJGQoIFwgLFwAAAAA="
        pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4

        pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal
        pdfConverter.PdfDocumentOptions.ShowHeader = True
        pdfConverter.PdfDocumentOptions.ShowFooter = True
        pdfConverter.PdfDocumentOptions.EmbedFonts = True

        pdfConverter.PdfHeaderOptions.TextArea = New TextArea(520, 10, "หน้า &p;", New Font(New FontFamily("Angsana New"), 14, GraphicsUnit.Point))
        pdfConverter.PdfHeaderOptions.TextArea.EmbedTextFont = True


        pdfConverter.PdfHeaderOptions.HeaderHeight = 20
        pdfConverter.PdfHeaderOptions.DrawHeaderLine = False

        pdfConverter.PdfFooterOptions.FooterHeight = 20
        pdfConverter.PdfFooterOptions.DrawFooterLine = False

        pdfConverter.PdfDocumentOptions.LeftMargin = 20
        pdfConverter.PdfDocumentOptions.RightMargin = 20

        ' get the base url for string conversion which is the url from where the html code was retrieved
        ' the base url is used by the converter to get the full URL of the external CSS and images referenced by relative URLs
        Dim baseUrl As String = HttpContext.Current.Request.Url.AbsoluteUri

        ' get the pdf bytes from html string
        Dim pdfBytes() As Byte = pdfConverter.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseUrl)

        Dim response As HttpResponse = HttpContext.Current.Response
        response.Clear()
        response.AddHeader("Content-Type", "application/pdf")
        response.AddHeader("Content-Disposition", String.Format("attachment; filename=PdfExam.pdf; size={0}", pdfBytes.Length.ToString()))
        response.BinaryWrite(pdfBytes)
        ' Note: it is important to end the response, otherwise the ASP.NET
        ' web page will render its content to PDF document stream
        response.End()
    End Sub

    Protected Sub btnGenKey_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenKey.Click
        Session("IsAnswerSheet") = True
        GenPDFFile()
    End Sub

    Protected Sub btnGenPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenPDF.Click
        Session("IsAnswerSheet") = False
        GenPDFFile()
    End Sub
End Class