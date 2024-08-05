Imports BusinessTablet360

Public Class DashboardPracticePage
    Inherits System.Web.UI.Page

    Public IE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession.StoredValue("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
#End If

        Log.Record(Log.LogType.PageLoad, "pageload dashboardpractice", False)

        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "dashboardpractice session หลุด", False)
            Response.Redirect("~/loginpage.aspx")
        End If

        Dim UserID As String = Session("UserId").ToString()
        ' REPEATER
        MyCtlTestset.RepeaterTestsetControl(EnumDashBoardType.Practice, UserID)

        'GRAPH
        Dim ClsGraphDashboard As New ClsGraphDashboard(EnumDashBoardType.Practice, UserID)
        Dim dtDataGraph As DataTable = ClsGraphDashboard.GetDataGraph()

        If Not dtDataGraph.Rows.Count = 0 Then
            RadChartDashboard.DataSource = dtDataGraph

            Dim value As Integer = dtDataGraph.AsEnumerable().Max(Function(row) row("NoOfPractice"))
            ' ถ้าจำนวนข้อมูลทุก row มีค่าต่ำกว่า 5 ให้ set ค่า max = 5 ใน แกน y
            If value < 5 Then
                RadChartDashboard.PlotArea.YAxis.AutoScale = False
                RadChartDashboard.PlotArea.YAxis.LabelStep = 1
                RadChartDashboard.PlotArea.YAxis.MaxValue = 5
            Else
                RadChartDashboard.PlotArea.YAxis.AutoScale = True
            End If

            ' ถ้าข้อมมูลมากกว่า 15 ปรับเป็นแนวนอน
            If dtDataGraph.Rows.Count > 15 Then
                RadChartDashboard.Height = dtDataGraph.Rows.Count * 40
                RadChartDashboard.SeriesOrientation = Telerik.Charting.ChartSeriesOrientation.Horizontal
            Else
                RadChartDashboard.Height = 250
            End If
            RadChartDashboard.PlotArea.XAxis.DataLabelsColumn = "ClassName"
            RadChartDashboard.DataBind()

            If ClsKNSession.RunMode = "standalonenotablet" Then
                RadChartDashboard.Visible = False
            End If
        Else
            RadChartDashboard.Visible = False
        End If

    End Sub
End Class