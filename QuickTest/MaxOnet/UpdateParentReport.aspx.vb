Imports Telerik.Web.UI
Imports Telerik.Charting
Imports KnowledgeUtils

Public Class UpdateParentReport
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Dim QualityDailyPercent As New List(Of Object)
    Dim QualityPracticePercent As New List(Of Object)
    Dim QuantityDailyPercent As New List(Of Object)
    Dim QuantityPracticePercent As New List(Of Object)
    Dim BaseGraph As New List(Of Object)
    Dim TokenId As String
    Dim Date0 As Date

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ClsLog.Record("เริ่มทำการ Update ParentReport")
        Dim MOCls As New MaxOnetManagement
        MOCls.UpdateParentReport()

        ClsLog.Record("เริ่มทำการ GenGraph")

        Dim sql As New StringBuilder
        sql.Append("select maxonet_tblKeyCodeUsage.KCU_Token,maxonet_tblParentGraph.*,convert(varchar(10),dateadd(year,543,cast(PG_LastUpdate as varchar)),103)")
        sql.Append(" as LastUpdate from maxonet_tblParentGraph inner join maxonet_tblKeyCodeUsage on ")
        sql.Append(" maxonet_tblParentGraph.KeyCode_Code = maxonet_tblKeyCodeUsage.KeyCode_Code")
        sql.Append(" where PG_IsActive = 1 and maxonet_tblKeyCodeUsage.KCU_IsActive = 1 and maxonet_tblKeyCodeUsage.KCU_Type = 1 and PG_QualityDailyDay0 is not null;")

        Dim dt As New DataTable
        dt = db.getdata(sql.ToString)

        Date0 = dt.Rows(0)("Lastupdate")

        For Each EachTokenGraph In dt.Rows

            TokenId = Replace(EachTokenGraph("KCU_Token").ToString, "-", "")

            QualityDailyPercent.Clear()
            QualityPracticePercent.Clear()
            QuantityDailyPercent.Clear()
            QuantityPracticePercent.Clear()

            If EachTokenGraph("PG_QualityDailyDayMinus6") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus6")})
            End If
            If EachTokenGraph("PG_QualityDailyDayMinus5") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus5")})
            End If
            If EachTokenGraph("PG_QualityDailyDayMinus4") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus4")})
            End If
            If EachTokenGraph("PG_QualityDailyDayMinus3") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus3")})
            End If
            If EachTokenGraph("PG_QualityDailyDayMinus2") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus2")})
            End If
            If EachTokenGraph("PG_QualityDailyDayMinus1") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDayMinus1")})
            End If
            If EachTokenGraph("PG_QualityDailyDay0") IsNot DBNull.Value Then
                QualityDailyPercent.Add(New With {.QualityDailyValue = EachTokenGraph("PG_QualityDailyDay0")})
            End If

            If EachTokenGraph("PG_QualityPracticeDayMinus6") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus6")})
            End If
            If EachTokenGraph("PG_QualityPracticeDayMinus5") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus5")})
            End If
            If EachTokenGraph("PG_QualityPracticeDayMinus4") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus4")})
            End If
            If EachTokenGraph("PG_QualityPracticeDayMinus3") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus3")})
            End If
            If EachTokenGraph("PG_QualityPracticeDayMinus2") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus2")})
            End If
            If EachTokenGraph("PG_QualityPracticeDayMinus1") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDayMinus1")})
            End If
            If EachTokenGraph("PG_QualityPracticeDay0") IsNot DBNull.Value Then
                QualityPracticePercent.Add(New With {.QualityPracticeValue = EachTokenGraph("PG_QualityPracticeDay0")})
            End If

            CreateAndSaveQualityChart()

            If EachTokenGraph("PG_QuantityDailyDayMinus6") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus6")})
            End If
            If EachTokenGraph("PG_QuantityDailyDayMinus5") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus5")})
            End If
            If EachTokenGraph("PG_QuantityDailyDayMinus4") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus4")})
            End If
            If EachTokenGraph("PG_QuantityDailyDayMinus3") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus3")})
            End If
            If EachTokenGraph("PG_QuantityDailyDayMinus2") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus2")})
            End If
            If EachTokenGraph("PG_QuantityDailyDayMinus1") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDayMinus1")})
            End If
            If EachTokenGraph("PG_QuantityDailyDay0") IsNot DBNull.Value Then
                QuantityDailyPercent.Add(New With {.QuantityDailyValue = EachTokenGraph("PG_QuantityDailyDay0")})
            End If

            If EachTokenGraph("PG_QuantityPracticeDayMinus6") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus6")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDayMinus5") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus5")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDayMinus4") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus4")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDayMinus3") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus3")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDayMinus2") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus2")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDayMinus1") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDayMinus1")})
            End If
            If EachTokenGraph("PG_QuantityPracticeDay0") IsNot DBNull.Value Then
                QuantityPracticePercent.Add(New With {.QuantityPracticeValue = EachTokenGraph("PG_QuantityPracticeDay0")})
            End If

            CreateAndSaveQuantityChart()

        Next
        ClsLog.Record("Finish GenGraph+Report")
    End Sub
#Region "GenChart"

    Private Sub CreateAndSaveQualityChart()

        If QualityDailyPercent.Count > QualityPracticePercent.Count Then
            BaseGraph = QualityDailyPercent
        Else
            BaseGraph = QualityPracticePercent
        End If

        With QualityChart
            .Clear()
            .Legend.Visible = False

            'H + W
            .Height = "380"
            .Width = "500"

            ' background
            .Appearance.Border.Color = Drawing.Color.MidnightBlue
            .Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient
            .Appearance.FillStyle.MainColor = Drawing.Color.Snow
            .Appearance.FillStyle.SecondColor = Drawing.Color.Snow
            .PlotArea.Appearance.FillStyle.MainColor = Drawing.Color.Snow

            ' border 0
            .PlotArea.Appearance.Border.Visible = False

            'Title
            .ChartTitle.TextBlock.Visible = True
            .ChartTitle.TextBlock.Text = " % คะแนนที่ได้จากการทำกิจกรรม"
            .ChartTitle.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top
            .ChartTitle.TextBlock.Appearance.TextProperties.Color = Drawing.Color.ForestGreen
            .ChartTitle.Appearance.Dimensions.Margins.Top = 0

            'X
            .PlotArea.XAxis.AutoScale = False
            .PlotArea.XAxis.AddRange(1, BaseGraph.Count, 1)
            .PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = New System.Drawing.Font("Sans-Serif", 10)
            .PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Left
            Dim Index = 0
            Dim DateMinus = BaseGraph.Count
            For Each a In BaseGraph
                .PlotArea.XAxis(Index).TextBlock.Text = DateAdd(DateInterval.Day, 0 - DateMinus, Date0).ToString("dd/MM")
                Index += 1
                DateMinus -= 1
            Next

            'Y
            .PlotArea.YAxis.Appearance.LabelAppearance.Visible = True
            '.PlotArea.YAxis.AutoScale = True
            .PlotArea.YAxis.AddRange(0, 100, 1)
            .PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Font = New System.Drawing.Font("Sans-Serif", 10)
            .PlotArea.YAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Center
            .PlotArea.YAxis.IsZeroBased = True


            .PlotArea.YAxis.Appearance.MinorGridLines.Color = Drawing.Color.WhiteSmoke
            .PlotArea.YAxis.Appearance.MinorGridLines.PenStyle = Drawing.Drawing2D.DashStyle.Dash
            .PlotArea.YAxis.Appearance.MinorGridLines.Width = 2
            .PlotArea.YAxis.Appearance.MajorGridLines.Color = Drawing.Color.Lavender
            .PlotArea.YAxis.Appearance.MajorGridLines.PenStyle = Drawing.Drawing2D.DashStyle.Solid
            .PlotArea.YAxis.Appearance.MajorGridLines.Width = 3

            Dim DailySeries As New Telerik.Charting.ChartSeries
            DailySeries.Type = Telerik.Charting.ChartSeriesType.Line
            DailySeries.Appearance.LineSeriesAppearance.Color = Drawing.Color.LimeGreen
            For Each Thm In QualityDailyPercent
                Dim DailySeriesItem As New Telerik.Charting.ChartSeriesItem
                DailySeriesItem.YValue = Thm.QualityDailyValue
                DailySeries.Items.Add(DailySeriesItem)
                DailySeries.Appearance.ShowLabels = False
                DailySeries.Appearance.PointMark.Dimensions.AutoSize = False
                DailySeries.Appearance.PointMark.Dimensions.Width = 10
                DailySeries.Appearance.PointMark.Dimensions.Height = 10
                DailySeries.Appearance.PointMark.FillStyle.MainColor = Drawing.Color.LimeGreen
                DailySeries.Appearance.PointMark.FillStyle.FillType = Styles.FillType.Solid
                DailySeries.Appearance.PointMark.Visible = True

            Next

            Dim PracticeSeries As New Telerik.Charting.ChartSeries
            PracticeSeries.Type = Telerik.Charting.ChartSeriesType.Line
            PracticeSeries.Appearance.LineSeriesAppearance.Color = Drawing.Color.CornflowerBlue
            For Each Thm In QualityPracticePercent
                Dim PracticeSeriesItem As New Telerik.Charting.ChartSeriesItem
                PracticeSeriesItem.YValue = Thm.QualityPracticeValue
                PracticeSeries.Items.Add(PracticeSeriesItem)
                PracticeSeries.Appearance.ShowLabels = False
                PracticeSeries.Appearance.PointMark.Dimensions.AutoSize = False
                PracticeSeries.Appearance.PointMark.Dimensions.Width = 10
                PracticeSeries.Appearance.PointMark.Dimensions.Height = 10
                PracticeSeries.Appearance.PointMark.FillStyle.MainColor = Drawing.Color.CornflowerBlue
                PracticeSeries.Appearance.PointMark.FillStyle.FillType = Styles.FillType.Solid
                PracticeSeries.Appearance.PointMark.Visible = True
            Next


            .AddChartSeries(DailySeries)
            .AddChartSeries(PracticeSeries)

            .Visible = True
            Dim TokenPath As String = "~/Maxonet/ParentGraph/" & TokenId.ToLower
            If (Not System.IO.Directory.Exists(Server.MapPath(TokenPath))) Then
                System.IO.Directory.CreateDirectory(Server.MapPath(TokenPath))
            End If

            If (System.IO.File.Exists(Server.MapPath(TokenPath & "/Quality.png"))) Then
                System.IO.File.Delete(Server.MapPath(TokenPath & "/Quality.png"))
            End If

            .Save(Server.MapPath(TokenPath & "/Quality.png"), System.Drawing.Imaging.ImageFormat.Png)
        End With

    End Sub

    Private Sub CreateAndSaveQuantityChart()

        If QuantityDailyPercent.Count > QuantityPracticePercent.Count Then
            BaseGraph = QuantityDailyPercent
        Else
            BaseGraph = QuantityPracticePercent
        End If

        With QuantityChart
            .Clear()
            .Legend.Visible = False

            'H + W
            .Height = "380"
            .Width = "500"

            ' background พื้นหลัง
            .Appearance.Border.Color = Drawing.Color.MidnightBlue
            .Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient
            .Appearance.FillStyle.MainColor = Drawing.Color.Snow
            .Appearance.FillStyle.SecondColor = Drawing.Color.Snow

            'Background Graph
            .PlotArea.Appearance.FillStyle.MainColor = Drawing.Color.Snow

            'Title
            .ChartTitle.TextBlock.Visible = True
            .ChartTitle.TextBlock.Text = "% จำนวนข้อที่เข้าทำกิจกรรม"
            .ChartTitle.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top
            .ChartTitle.TextBlock.Appearance.TextProperties.Color = Drawing.Color.ForestGreen
            .ChartTitle.Appearance.Dimensions.Margins.Top = 0

            ' border 0
            .PlotArea.Appearance.Border.Visible = False

            'X
            .PlotArea.XAxis.AutoScale = False
            .PlotArea.XAxis.AddRange(1, BaseGraph.Count, 1)
            .PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = New System.Drawing.Font("Sans-Serif", 10)
            .PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Left
            Dim Index = 0
            Dim DateMinus = BaseGraph.Count
            For Each a In BaseGraph
                .PlotArea.XAxis(Index).TextBlock.Text = DateAdd(DateInterval.Day, 0 - DateMinus, Date0).ToString("dd/MM")
                Index += 1
                DateMinus -= 1
            Next

            'Y
            .PlotArea.YAxis.Appearance.LabelAppearance.Visible = True
            .PlotArea.YAxis.AddRange(0, 100, 1)
            .PlotArea.YAxis.Appearance.TextAppearance.TextProperties.Font = New System.Drawing.Font("Sans-Serif", 10)
            .PlotArea.YAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Center
            .PlotArea.YAxis.IsZeroBased = True

            'เส้นแนวนอน
            .PlotArea.YAxis.Appearance.MinorGridLines.Color = Drawing.Color.WhiteSmoke
            .PlotArea.YAxis.Appearance.MinorGridLines.PenStyle = Drawing.Drawing2D.DashStyle.Dash
            .PlotArea.YAxis.Appearance.MinorGridLines.Width = 2
            .PlotArea.YAxis.Appearance.MajorGridLines.Color = Drawing.Color.Lavender
            .PlotArea.YAxis.Appearance.MajorGridLines.PenStyle = Drawing.Drawing2D.DashStyle.Solid
            .PlotArea.YAxis.Appearance.MajorGridLines.Width = 3

            Dim DailySeries As New Telerik.Charting.ChartSeries
            DailySeries.Type = Telerik.Charting.ChartSeriesType.Line
            DailySeries.Appearance.LineSeriesAppearance.Color = Drawing.Color.LimeGreen
            For Each Thm In QuantityDailyPercent
                Dim DailySeriesItem As New Telerik.Charting.ChartSeriesItem
                DailySeriesItem.YValue = Thm.QuantityDailyValue
                DailySeries.Items.Add(DailySeriesItem)
                DailySeries.Appearance.ShowLabels = False
                DailySeries.Appearance.PointMark.Dimensions.AutoSize = False
                DailySeries.Appearance.PointMark.Dimensions.Width = 10
                DailySeries.Appearance.PointMark.Dimensions.Height = 10
                DailySeries.Appearance.PointMark.FillStyle.MainColor = Drawing.Color.LimeGreen
                DailySeries.Appearance.PointMark.FillStyle.FillType = Styles.FillType.Solid
                DailySeries.Appearance.PointMark.Visible = True
            Next

            Dim PracticeSeries As New Telerik.Charting.ChartSeries
            PracticeSeries.Type = Telerik.Charting.ChartSeriesType.Line
            PracticeSeries.Appearance.LineSeriesAppearance.Color = Drawing.Color.CornflowerBlue
            For Each Thm In QuantityPracticePercent
                Dim PracticeSeriesItem As New Telerik.Charting.ChartSeriesItem
                PracticeSeriesItem.YValue = Thm.QuantityPracticeValue
                PracticeSeries.Items.Add(PracticeSeriesItem)
                PracticeSeriesItem.ActiveRegion.Tooltip = Thm.QuantityPracticeValue
                PracticeSeries.Appearance.ShowLabels = False
                PracticeSeries.Appearance.PointMark.Dimensions.AutoSize = False
                PracticeSeries.Appearance.PointMark.Dimensions.Width = 10
                PracticeSeries.Appearance.PointMark.Dimensions.Height = 10
                PracticeSeries.Appearance.PointMark.FillStyle.MainColor = Drawing.Color.CornflowerBlue
                PracticeSeries.Appearance.PointMark.FillStyle.FillType = Styles.FillType.Solid
                PracticeSeries.Appearance.PointMark.Visible = True
            Next


            .AddChartSeries(DailySeries)
            .AddChartSeries(PracticeSeries)

            .Visible = True
            Dim TokenPath As String = "~/Maxonet/ParentGraph/" & TokenId.ToLower
            If (Not System.IO.Directory.Exists(Server.MapPath(TokenPath))) Then
                System.IO.Directory.CreateDirectory(Server.MapPath(TokenPath))
            End If

            If (System.IO.File.Exists(Server.MapPath(TokenPath & "/Quantity.png"))) Then
                System.IO.File.Delete(Server.MapPath(TokenPath & "/Quantity.png"))
            End If

            .Save(Server.MapPath(TokenPath & "/Quantity.png"), System.Drawing.Imaging.ImageFormat.Png)
        End With

    End Sub
#End Region
End Class