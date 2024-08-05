Public Class TestViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Response.Redirect("~/QRReport/ProcessQRReport.aspx?ClassName=ป.3&RoomName=/7&SchoolCode=1000001")
        Response.Redirect("~/QRReport/ProcessQRReport.aspx?StudentId=474445DD-4C94-4AE3-9994-5E57309A471D&Position=1")

        'Dim InstanceReport As New Telerik.Reporting.UriReportSource()
        'InstanceReport.Uri = HttpContext.Current.Server.MapPath("./ReportTemplate/QRReportClass.trdx")
        'InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentClass", "ป.3"))
        'InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmCurrentRoom", "/9"))
        'InstanceReport.Parameters.Add(New Telerik.Reporting.Parameter("RptPrmSchoolCode", "1000001"))
        ''InstanceReport.Parameters.Add("CurrentClass", "ป.3")
        ''InstanceReport.Parameters.Add("CurrentRoom", "/1")
        ''InstanceReport.Parameters.Add("SchoolCode", "1000001")
        'ReportViewer1.ReportSource = InstanceReport

        'Gen(PDF)
        'Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        'Dim deviceinfo As New System.Collections.Hashtable()
        'Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
        'Dim fileName As String = HttpContext.Current.Server.MapPath("../QRReport/tmpPDFReport/SampleFile.pdf")
        'Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
        '    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        '    fs.Close()
        '    fs.Dispose()
        'End Using

        'deviceinfo = Nothing
        ''InstanceReport.ReportDocument.Dispose()
        ''dtStudentInfoBottom.Dispose()
        ''dtTotalPracticeBySubject.Dispose()
        ''ArrDataPieChart = Nothing
        ''ArrStudentSendHomework = Nothing


    End Sub

End Class