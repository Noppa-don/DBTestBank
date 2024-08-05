Public Class ReportStudent
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("StudentId") Is Nothing Or Request.QueryString("Month") Is Nothing Or Request.QueryString("Year") Is Nothing Or Request.QueryString("FileName") Is Nothing Or Request.QueryString("FolderName") Is Nothing Then
            Response.Write("ไม่ได้รับ QueryString")
            Exit Sub
        End If

        'HARD CODE
        Dim StudentId As String = Request.QueryString("StudentId").ToString() '"6902A943-75FE-48E1-8263-7610074019F6"
        Dim Month As String = Request.QueryString("Month") '"8"
        Dim Year As String = Request.QueryString("Year") '"2013"
        Dim FolderName As String = Request.QueryString("FolderName")
        Dim NameFile As String = Request.QueryString("FileName").ToString()
        Dim IndexPage As String = "หน้าที่ " & Request.QueryString("IndexPage")

        If Not Page.IsPostBack Then
            Dim InstanceReport As New Telerik.Reporting.InstanceReportSource
            Dim dtStudentInfoBottom As DataTable = GetdtStudentInfoBottom(StudentId)
            Dim ArrDataPieChart As ArrayList = GetArrDataTotalUsagePieChart(StudentId, Month, Year)
            Dim dtTotalPracticeBySubject As DataTable = GetDtTotalPracticeBySubject(StudentId, Month, Year)
            Dim dtTotalQuizBySubject As DataTable = GetDtTotalQuizBySubject(StudentId, Month, Year)
            Dim ArrStudentSendHomework As ArrayList = GetArrStudentSendHomework(StudentId, Month, Year)
            InstanceReport.ReportDocument = New MasterReportStudent(dtStudentInfoBottom, ArrDataPieChart, dtTotalPracticeBySubject, ArrStudentSendHomework, IndexPage, dtTotalQuizBySubject)
            'ReportViewer1.ReportSource = InstanceReport
            'ReportViewer1.RefreshReport()

            'Gen(PDF)
            Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
            Dim deviceinfo As New System.Collections.Hashtable()
            Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", InstanceReport, deviceinfo)
            Dim fileName As String = "D:\data\tmp\AnalysisReport\PDF\" & FolderName & "\" & NameFile & "." + result.Extension
            Using fs As New System.IO.FileStream(fileName, System.IO.FileMode.Create)
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                fs.Close()
                fs.Dispose()
            End Using

            deviceinfo = Nothing
            InstanceReport.ReportDocument.Dispose()
            dtStudentInfoBottom.Dispose()
            dtTotalPracticeBySubject.Dispose()
            ArrDataPieChart = Nothing
            ArrStudentSendHomework = Nothing

        End If
        
    End Sub

    Private Function GetArrDataTotalUsagePieChart(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)

        Dim ArrData As New ArrayList
        Dim sql As String = " select TotalQuiz,TotalPractice,TotalHomework from tmp_StudentReportTotalActivePieChart " & _
                            " where Student_Id = '" & StudentId & "' and DATEPART(MONTH,DateMonth) = '" & Month & "' and DATEPART(YEAR,DateMonth) = '" & Year & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            ArrData.Add(dt.Rows(0)("TotalQuiz"))
            ArrData.Add(dt.Rows(0)("TotalPractice"))
            ArrData.Add(dt.Rows(0)("TotalHomework"))
        End If
        Return ArrData

    End Function

    Private Function GetDtTotalPracticeBySubject(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)

        Dim sql As String = " select Thai,English,Social,Health,Science,Art,Math,Careers from tmp_StudentReportTotalUsageBySubject " & _
                            " where Student_Id = '" & StudentId & "' and IsPractice = 1 " & _
                            " and DATEPART(MONTH,DayMonth) = '" & Month & "' and DATEPART(YEAR,DayMonth) = '" & Year & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetDtTotalQuizBySubject(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)
        Dim sql As String = " select Thai as ไทย,English as อังกฤษ,Social as สังคม,Health as สุขศึกษา,Science as วิทย์,Art as ศิลปะ,Math as คณิต,Careers as การงานฯ from tmp_StudentReportTotalUsageBySubject " & _
                         " where Student_Id = '" & StudentId & "' and IsQuiz = 1 " & _
                         " and DATEPART(MONTH,DayMonth) = '" & Month & "' and DATEPART(YEAR,DayMonth) = '" & Year & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Function GetArrStudentSendHomework(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)

        Dim ArrData As New ArrayList
        Dim sql As String = " select CompleteAndBeforeDeadline,NotComplete,NotSend from tmp_StudentReportSendHomework where Student_Id = '" & StudentId & "' " & _
                            " and DATEPART(MONTH,DayMonth) = '" & Month & "' and DATEPART(YEAR,DayMonth) = '" & Year & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            ArrData.Add(dt.Rows(0)("CompleteAndBeforeDeadline"))
            ArrData.Add(dt.Rows(0)("NotComplete"))
            ArrData.Add(dt.Rows(0)("NotSend"))
        End If
        Return ArrData

    End Function

    Private Function GetdtStudentInfoBottom(ByVal StudentId As String)

        Dim sql As String = " SELECT t360_tblStudent.School_Code, t360_tblStudent.Student_Id,t360_tblStudent.Student_FirstName + ' (' + t360_tblStudent.Student_NickName + ') ' " & _
                            " + t360_tblStudent.Student_LastName AS StudentName,t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + " & _
                            " ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS varchar(10)) + ' รหัส ' + t360_tblStudent.Student_Code AS StudentSchoolInfo, " & _
                            " tblParent.PR_FirstName + ' โทร : ' + tblParent.PR_Phone AS StudentParentInfo, t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + " & _
                            " ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS varchar(10)) AS InfoWithoutCode FROM tblParent INNER JOIN tblStudentParent ON " & _
                            " tblParent.PR_Id = tblStudentParent.PR_Id RIGHT OUTER JOIN t360_tblStudent ON tblStudentParent.Student_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (t360_tblStudent.Student_Id = '" & StudentId & "') "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function



End Class