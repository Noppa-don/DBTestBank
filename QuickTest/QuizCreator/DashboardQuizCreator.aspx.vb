Imports Highchart.Core
Imports System.Drawing

Public Class DashboardQuizCreator
    Inherits System.Web.UI.Page
    Dim clsGenChart As New ClassGenChart
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HttpContext.Current.Session("QC_UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"

        If Not Page.IsPostBack Then
            ProcessChart()
        End If
    End Sub

    Private Sub ProcessChart()
        Dim ArrTest As ArrayList = FilterControl1.GetDuratoinDate()
        Dim dtTopUsage As DataTable = GetDtTop10PieChart(ArrTest(0), ArrTest(1), 0)
        Dim dtTopDownlad As DataTable = GetDtTop10PieChart(ArrTest(0), ArrTest(1), 1)
        Dim dtTopRating As DataTable = GetDtTop10Rating(ArrTest(0), ArrTest(1))
        Dim dtLineChart As DataTable = GetDtLineChart(ArrTest(0), ArrTest(1))
        Dim StrTopUsage As String = clsGenChart.GenStrPieChart("TopUsageChart", "ข้อสอบที่มีคนทำมากที่สุด 10 อันดับแรก", "ชุด", dtTopUsage)
        Dim StrTopDownload As String = clsGenChart.GenStrPieChart("TopDownloadChart", "ข้อสอบที่มีคนโหลดมากที่สุด 10 อันดับแรก", "ชุด", dtTopDownlad)
        Dim StrTopRating As String = clsGenChart.GenStrPieChart("TopRatingChart", "ข้อสอบที่มีความนิยมสูงที่สุด 10 อันดับแรก", "", dtTopRating)
        Dim StrLineChart As String = clsGenChart.GenStrLineChart("DivLineChart", "จำนวนการเข้าทำชุดข้อสอบ", dtLineChart, "จำนวนคนเข้าทำ")
        Dim ArrStr As New ArrayList
        ArrStr.Add(StrTopUsage)
        ArrStr.Add(StrTopDownload)
        ArrStr.Add(StrTopRating)
        ArrStr.Add(StrLineChart)
        Dim CompleteStr As String = clsGenChart.GenStrCompleteDashboard(ArrStr)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "test", CompleteStr)
    End Sub



    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Dim ArrTest As ArrayList = FilterControl1.GetDuratoinDate()
        If ArrTest.Count = 0 Then
            lblWarning.Visible = True
        Else
            lblWarning.Visible = False
            ProcessChart()
        End If
    End Sub

#Region "Function"
    'Function Get ข้อมูล ข้อสอบที่มี ... มากที่สุด 10 อันดับแรก
    Private Function GetDtTop10PieChart(ByVal Date1 As String, ByVal Date2 As String, ByVal QcType As Integer)
        Dim sql As String = " SELECT TOP 10 COUNT(*) AS Totaluse,dbo.tblTestSet.TestSet_Name FROM tblTestSet INNER JOIN tblQuizCreatorLog ON tblTestSet.TestSet_Id = tblQuizCreatorLog.Testset_Id " & _
                            " WHERE (tblTestSet.UserId = '" & HttpContext.Current.Session("QC_UserId").ToString() & "') AND (tblTestSet.IsActive = 1) AND (tblQuizCreatorLog.IsActive = 1) AND (tblQuizCreatorLog.QCL_Type = " & QcType & ") " & _
                            " AND dbo.tblQuizCreatorLog.QCL_TimeStamp BETWEEN '" & Date1 & "' AND '" & Date2 & "' GROUP BY dbo.tblTestSet.TestSet_Name ORDER BY COUNT(*) DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    'Function Get ข้อมูล ข้อสอบที่มี Rating มากที่สุด 10 อันดับแรก
    Private Function GetDtTop10Rating(ByVal Date1 As String, ByVal Date2 As String)
        Dim sql As String = " SELECT TOP 10 SUM(dbo.tblQuizCreatorLog.QCL_Rating) AS Totaluse,dbo.tblTestSet.TestSet_Name FROM tblTestSet INNER JOIN tblQuizCreatorLog " & _
                            " ON tblTestSet.TestSet_Id = tblQuizCreatorLog.Testset_Id WHERE (tblTestSet.UserId = '" & HttpContext.Current.Session("QC_UserId").ToString() & "') AND (tblTestSet.IsActive = 1) " & _
                            " AND (tblQuizCreatorLog.IsActive = 1) AND (tblQuizCreatorLog.QCL_Type = 2) AND dbo.tblQuizCreatorLog.QCL_TimeStamp " & _
                            " BETWEEN '" & Date1 & "'  AND '" & Date2 & "'  GROUP BY dbo.tblTestSet.TestSet_Name ORDER BY SUM(dbo.tblQuizCreatorLog.QCL_Rating) DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    'Function Get ข้อมูล จำนวนคนที่เข้าทำเป็นรายวัน กราฟเส้น
    Private Function GetDtLineChart(ByVal Date1 As String, ByVal Date2 As String)
        Dim sql As String = " SELECT CAST(CAST(QCL_TimeStamp AS DATE) AS VARCHAR(50)) AS QCL_TimeStamp ,COUNT(*) AS TotalUse FROM tblTestSet INNER JOIN tblQuizCreatorLog ON tblTestSet.TestSet_Id = tblQuizCreatorLog.Testset_Id " & _
                            " WHERE (tblTestSet.UserId = '" & HttpContext.Current.Session("QC_UserId").ToString() & "') AND (tblTestSet.IsActive = 1) " & _
                            " AND (tblQuizCreatorLog.IsActive = 1) AND (tblQuizCreatorLog.QCL_Type = 0) AND dbo.tblQuizCreatorLog.QCL_TimeStamp  BETWEEN '" & Date1 & "'  AND '" & Date2 & "' " & _
                            " GROUP BY QCL_TimeStamp ORDER BY QCL_TimeStamp "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function
#End Region

End Class