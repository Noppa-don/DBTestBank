Public Class DashboardTestset
    Inherits System.Web.UI.Page
    Public StrTestsetName As String
    Public StrTestsetDetail As String
    Dim _DB As New ClassConnectSql()
    Dim ClsGenChart As New ClassGenChart

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If HttpContext.Current.Request.QueryString("TestsetId") IsNot Nothing And HttpContext.Current.Request.QueryString("TestsetId").ToString() <> "" Then
                GetTestsetNameByTestsetId(HttpContext.Current.Request.QueryString("TestsetId").ToString())
                ProcessChart(HttpContext.Current.Request.QueryString("TestsetId").ToString())
            End If
        End If
    End Sub

    Private Sub GetTestsetNameByTestsetId(ByVal TestsetId As String)
        Dim sql As String = " SELECT TestSet_Name FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestsetId & "' "
        StrTestsetName = _DB.ExecuteScalar(sql)
    End Sub
  
    Private Sub ProcessChart(ByVal TestsetId As String)
        Dim ArrDate As ArrayList = FilterControl1.GetDuratoinDate()
        Dim ArrTotalUseAndDownload As ArrayList = GetArrTopDownloadAndUsage(ArrDate(0), ArrDate(1), TestsetId)
        Dim dtLineChartTotalusage As DataTable = GetDtLineChartTotalUse(ArrDate(0), ArrDate(1), TestsetId)
        Dim dtHorizontalChart As DataTable = GetDtHorizontalChart(TestsetId)
        Dim StrTotaldownloadAndUsage As String = ClsGenChart.GenStrVerticalBarChart("DivChartTotalDownloadAndUsage", "ปริมาณการดาวน์โหลด/เข้าทำ", ArrTotalUseAndDownload(0), ArrTotalUseAndDownload(1))
        Dim StrLineChartTotalUsage As String = ClsGenChart.GenStrLineChart("DivLineUsage", "จำนวนการเข้าทำชุดข้อสอบ", dtLineChartTotalusage, "จำนวนคนเข้าทำ")
        Dim StrHorizontalChart As String = ClsGenChart.GenStrHorizontalBarChart("DivBarChartBottom", "ปริมาณคนตอบถูกแยกเป็นข้อ", dtHorizontalChart)
        Dim ArrStr As New ArrayList
        ArrStr.Add(StrTotaldownloadAndUsage)
        ArrStr.Add(StrLineChartTotalUsage)
        ArrStr.Add(StrHorizontalChart)
        Dim CompleteStr As String = ClsGenChart.GenStrCompleteDashboard(ArrStr)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "test", CompleteStr)
    End Sub

    Private Function GetArrTopDownloadAndUsage(ByVal Date1 As String, ByVal Date2 As String, ByVal TestsetId As String)
        Dim ArrTopDAndU As New ArrayList
        Dim sql As String = " SELECT COUNT(*) AS Totaluse FROM dbo.tblQuizCreatorTestset INNER JOIN dbo.tblQuizCreatorLog ON	dbo.tblQuizCreatorTestset.Testset_Id = dbo.tblQuizCreatorLog.Testset_Id " & _
                            " WHERE dbo.tblQuizCreatorTestset.Testset_Id = '" & TestsetId & "' AND dbo.tblQuizCreatorLog.QCL_TimeStamp BETWEEN '" & Date1 & "' AND '" & Date2 & "' " & _
                            " AND dbo.tblQuizCreatorLog.QCL_Type = 0 AND dbo.tblQuizCreatorLog.IsActive = 1 AND dbo.tblQuizCreatorTestset.QCT_IsActive = 1 "
        Dim TotalUse As Integer = CInt(_DB.ExecuteScalar(sql))
        sql = " SELECT COUNT(*) AS TotalDownload FROM dbo.tblQuizCreatorTestset INNER JOIN dbo.tblQuizCreatorLog ON	dbo.tblQuizCreatorTestset.Testset_Id = dbo.tblQuizCreatorLog.Testset_Id " & _
              " WHERE dbo.tblQuizCreatorTestset.Testset_Id = '" & TestsetId & "' AND dbo.tblQuizCreatorLog.QCL_TimeStamp BETWEEN '" & Date1 & "' AND '" & Date2 & "' " & _
              " AND dbo.tblQuizCreatorLog.QCL_Type = 1 AND dbo.tblQuizCreatorLog.IsActive = 1 AND dbo.tblQuizCreatorTestset.QCT_IsActive = 1 "
        Dim TotalDownload As Integer = CInt(_DB.ExecuteScalar(sql))
        ArrTopDAndU.Add(TotalUse)
        ArrTopDAndU.Add(TotalDownload)
        Return ArrTopDAndU
    End Function

    Private Function GetDtLineChartTotalUse(ByVal Date1 As String, ByVal Date2 As String, ByVal TestsetId As String)
        Dim sql As String = " SELECT CAST(CAST(QCL_TimeStamp AS DATE) AS VARCHAR(50)) AS QCL_TimeStamp,COUNT(*) AS TotalUse FROM dbo.tblQuizCreatorLog " & _
                            " WHERE Testset_Id = '" & TestsetId & "' AND QCL_Type = 0 AND IsActive = 1 AND dbo.tblQuizCreatorLog.QCL_TimeStamp " & _
                            " BETWEEN '" & Date1 & "' And '" & Date2 & "' GROUP BY CAST(CAST(QCL_TimeStamp AS DATE) AS VARCHAR(50)) "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Function GetDtHorizontalChart(ByVal TestsetId As String)
        Dim sql As String = " SELECT ROW_NUMBER() OVER(ORDER BY dbo.tblQuizScore.Question_Id DESC) AS QNo, " & _
                            " SUM(dbo.tblQuizScore.Score) AS TotalCorrect FROM dbo.tblQuiz INNER JOIN dbo.tblQuizScore ON dbo.tblQuiz.Quiz_Id = dbo.tblQuizScore.Quiz_Id " & _
                            " WHERE TestSet_Id = '" & TestsetId & "' AND dbo.tblQuiz.IsActive = 1 AND dbo.tblQuizScore.IsActive = 1 " & _
                            " GROUP BY dbo.tblQuizScore.Question_Id "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Dim ArrTest As ArrayList = FilterControl1.GetDuratoinDate()
        If ArrTest.Count = 0 Then
            lblWarning.Visible = True
        Else
            lblWarning.Visible = False
            ProcessChart(HttpContext.Current.Request.QueryString("TestsetId").ToString())
        End If
    End Sub

End Class