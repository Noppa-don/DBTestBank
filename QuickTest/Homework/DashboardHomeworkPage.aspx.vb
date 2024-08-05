Imports Telerik.Charting
Imports BusinessTablet360

Public Class DashboardHomeworkPage
    Inherits System.Web.UI.Page

    Public IE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
#End If

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/loginpage.aspx")
        End If

        Dim UserID As String = Session("UserId").ToString()
        ' REPEATER
        MyCtlTestset.RepeaterTestsetControl(EnumDashBoardType.Homework, UserID)

        ' GRAPH
        Dim ClsGraphDashboard As New ClsGraphDashboard(EnumDashBoardType.Homework, UserID)
        Dim dtDataGraphPie As DataTable = ClsGraphDashboard.GetDataGraph(True)
        GetCreatePieGraph(dtDataGraphPie)
        Dim dtDataGraphBar As DataTable = ClsGraphDashboard.GetDataGraph()
        GetCreateBarGraph(dtDataGraphBar)

    End Sub

    ' PIE GRAPH
    Private Sub GetCreatePieGraph(ByVal dtDataGraph As DataTable)
        'dtDataGraph = Nothing
        'dtDataGraph = New DataTable
        'dtDataGraph.Columns.Add("strStatus", GetType(String))
        'dtDataGraph.Columns.Add("SumStatus", GetType(Integer))
        'dtDataGraph.Rows.Add("ยังไม่ได้ทำ", 30)
        'dtDataGraph.Rows.Add("ยังไม่เสร็จ", 20)
        'dtDataGraph.Rows.Add("เสร็จแล้ว", 50)
        For Each eachData In dtDataGraph.Rows
            eachData("strStatus") = eachData("strStatus") & " (" & eachData("SumStatus").ToString & ")"
        Next
        If Not dtDataGraph.Rows.Count = 0 Then
            Dim ChartSeries = New ChartSeries()
            ChartSeries.DataYColumn = "SumStatus"
            ChartSeries.DataLabelsColumn = "strStatus"
            ChartSeries.Type = ChartSeriesType.Pie
            RadChartPie.AddChartSeries(ChartSeries)
            RadChartPie.DataSource = dtDataGraph
            RadChartPie.DataBind()
        Else
            RadChartPie.Visible = False
        End If
    End Sub
    Protected Sub RadChartPie_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Charting.ChartItemDataBoundEventArgs) Handles RadChartPie.ItemDataBound
        e.SeriesItem.ActiveRegion.Tooltip = e.SeriesItem.YValue & " การบ้าน"
    End Sub

    ' BAR GRAPH
    Private Sub GetCreateBarGraph(ByVal dt As DataTable)
        If Not dt.Rows(0)("AVGR") Is DBNull.Value Then
            Dim dtDataGraph As New DataTable
            dtDataGraph.Columns.Add("StatName")
            dtDataGraph.Columns.Add("StatValue")
            Dim str As Array = {"คะแนนเฉลี่ย", "ส่งภายในกำหนดเวลา", "ทำครบทุกข้อ"}
            For i As Integer = 0 To 2
                dtDataGraph.Rows.Add(str(i), dt(0)(i))
            Next
            RadChartDashboard.PlotArea.XAxis.DataLabelsColumn = "StatName"
            RadChartDashboard.DataSource = dtDataGraph
            RadChartDashboard.DataBind()
        Else
            RadChartDashboard.Visible = False
        End If
    End Sub

End Class