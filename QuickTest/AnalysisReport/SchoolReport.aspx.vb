Public Class SchoolReport
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("SchoolId") Is Nothing Or Request.QueryString("Month") Is Nothing Or Request.QueryString("Year") Is Nothing Or Request.QueryString("FolderName") Is Nothing Or Request.QueryString("IndexPage") Is Nothing Then
            Response.Write("ไม่ได้รับ QueryString")
            Exit Sub
        End If

        'HardCode
        Dim SchoolCode As String = Request.QueryString("SchoolId") '"1000001"
        Dim CurrentMonth As String = Request.QueryString("Month") '"3"
        Dim Currentyear As String = Request.QueryString("Year") '"2013"
        Dim FolderName As String = Request.QueryString("FolderName")
        Dim IndexPage As String = "หน้าที่ " & Request.QueryString("IndexPage")

        If Not Page.IsPostBack Then
            Dim InstanceReport As New Telerik.Reporting.InstanceReportSource
            Dim ArrData As ArrayList = GetArrayDataTxtSchool(SchoolCode, CurrentMonth, Currentyear)
            Dim dt As DataTable = GetDtSchoolData(SchoolCode, CurrentMonth, Currentyear)
            Dim dtForCalendar As DataTable = GetDtForCalendar(SchoolCode, CurrentMonth, Currentyear)
            Dim MaxValue As Integer = GetMaxValueForCalendar(SchoolCode, CurrentMonth, Currentyear)
            InstanceReport.ReportDocument = New MasterSchoolReport(ArrData, dt, dtForCalendar, MaxValue, IndexPage)
            'ReportViewer1.ReportSource = InstanceReport
            'ReportViewer1.RefreshReport()

            'Gen(PDF)
            Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
            Dim deviceinfo As New System.Collections.Hashtable()
            Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
            'Dim fileName As String = Server.MapPath("./PDF/" & FolderName & "/P0000." + result.Extension)
            Dim fileName As String = "D:\data\tmp\AnalysisReport\PDF\" & FolderName & "\P0000." + result.Extension
            Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                fs.Close()
                fs.Dispose()
            End Using
            InstanceReport.ReportDocument.Dispose()
            dtForCalendar.Dispose()
            dt.Dispose()
            ArrData = Nothing

        End If
         
    End Sub

    Public Function GetArrayDataTxtSchool(ByVal SchoolCode As String, ByVal Month As String, ByVal Year As String) As ArrayList

        Dim ArrData As New ArrayList
        'Dim sql As String = " SELECT t360_tblSchool.School_Name ,  t360_tblSchool.School_Number + " & _
        '                    " ' ' + t360_tblSchool.School_Soi + ' ' +  t360_tblSchool.School_Street + ' ' + t360_tblDistrict.District_Name + ' ' + " & _
        '                    " t360_tblProvice.Province_Name as SchoolAddress, t360_tblSchool.School_Fax, t360_tblSchool.School_Phone1 " & _
        '                    " FROM t360_tblSchool INNER JOIN t360_tblDistrict ON t360_tblSchool.District_Id = t360_tblDistrict.District_Id INNER JOIN " & _
        '                    " t360_tblProvice ON t360_tblSchool.Province_Id = t360_tblProvice.Province_Id  where t360_tblSchool.School_Code = '" & SchoolCode & "' "
        Dim sql As String = " SELECT t360_tblSchool.School_Name ,  t360_tblSchool.School_Number +  ' ' +  t360_tblSchool.School_Street " & _
                            " + ' แขวง' + dbo.t360_tblSubDistrict.SubDistrict_Name + ' เขต' + t360_tblDistrict.District_Name + ' ' +  " & _
                            " t360_tblProvice.Province_Name as SchoolAddress, t360_tblSchool.School_Fax, t360_tblSchool.School_Phone1 ,dbo.t360_tblDistrict.District_Name " & _
                            " FROM  t360_tblSubDistrict INNER JOIN t360_tblSchool INNER JOIN t360_tblProvice ON t360_tblSchool.Province_Id = t360_tblProvice.Province_Id INNER JOIN " & _
                            " t360_tblDistrict ON t360_tblSchool.District_Id = t360_tblDistrict.District_Id ON t360_tblSubDistrict.SubDistrict_Id = t360_tblSchool.SubDistrict_Id " & _
                            " WHERE dbo.t360_tblSchool.School_Code = '" & SchoolCode & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            ArrData.Add(dt.Rows(0)("School_Name"))
            ArrData.Add(dt.Rows(0)("SchoolAddress"))
            ArrData.Add(dt.Rows(0)("School_Phone1"))
            ArrData.Add(dt.Rows(0)("School_Fax"))
            ArrData.Add(Month)
            ArrData.Add(Year)
        End If
        Return ArrData

    End Function

    Private Function GetDtForCalendar(ByVal SchoolId As String, ByVal Month As String, ByVal Year As String) As DataTable

        Dim dt As New DataTable
        Dim sql As String = " select DATEPART(DAY,DAY) as Day,TotalMinute,'" & Month & "' as Month from tmp_SchoolReport " & _
                            " where DATEPART(MONTH,day) = '" & Month & "' and DATEPART(YEAR,day) = '" & Year & "' and SchoolCode = '" & SchoolId & "' order by day "
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetMaxValueForCalendar(ByVal SchoolId As String, ByVal Month As String, ByVal Year As String) As Integer

        Dim sql As String = " select  MAX(totalminute) from tmp_SchoolReport where DATEPART(MONTH,day) = '" & Month & "' and " & _
                            " DATEPART(YEAR,day) = '" & Year & "' and SchoolCode = '" & SchoolId & "' "
        Dim MaxValue As Integer = CInt(_DB.ExecuteScalar(sql))
        Return MaxValue

    End Function

    Public Function GetDtSchoolData(ByVal SchoolId As String, ByVal Month As String, ByVal Year As String) As DataTable

        Dim sql As String = " select TotalHour,TotalSchoolActive,TotalStudentActive,TotalDevicePerfect,TotalDeviceBroken from tmp_SchoolReportUse " & _
                            " where DATEPART(MONTH,MonthDate) = '" & Month & "' and DATEPART(YEAR,MonthDate) = '" & Year & "' and SchoolId = '" & SchoolId & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function



End Class