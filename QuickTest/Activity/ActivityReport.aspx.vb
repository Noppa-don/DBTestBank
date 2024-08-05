Imports Telerik.Web.UI
Imports System.Web.Services
Imports Highchart.Core
Imports System.Drawing

Public Class ViewReport
    Inherits System.Web.UI.Page

    'ตัวแปรที่ใช้บอกว่าตอนนี้อยู่ในโหมดแบบไหน น่าจะไม่ได้ใช้แล้วเพราะว่าตอนนี้เหลือแค่ Mode เดียวแล้ว = 1
    Public Property MainState As Integer
        Get
            Return ViewState("_MainState")
        End Get
        Set(ByVal value As Integer)
            ViewState("_MainState") = value
        End Set
    End Property

    'ตัวแปรที่เก็บข้อมุล เกณฑ์คะแนนที่ผ่าน เอาไว้ใช้ตอนกดปุ่ม เพิ่มเกณฑ์คะแนนที่ผ่าน
    Public Property FilterScore As Double
        Get
            Return ViewState("_FilterScore")
        End Get
        Set(value As Double)
            ViewState("_FilterScore") = value
        End Set
    End Property

    'ตัวแปรเก็บ State ไว้ทำสีตารางฟ้าสลับฟ้าอ่อน
    Public Property OddRow As Boolean
        Get
            Return ViewState("_OddRow")
        End Get
        Set(value As Boolean)
            ViewState("_OddRow") = value
        End Set
    End Property

    'ไว้เก็บ QuizId เพื่อเอาไว้ใช้ทั้งหน้า
    Dim QueryStringQuizId As String
    'เก็บ TestsetId เพื่อเอาไว้ใช้ทั้งหน้า
    Dim QueryStringTestsetId As String
    'ตัวแปรที่ไว้ check ค่าว่ากดมาจาก Repeater Report หรือเปล่า เช่น มาจากหน้า RepeaterReportQuiz,RepeaterReportHomework
    Public CheckIsFromReportMenu As String
    'ตัวแปรเอาไว้เก็บ mode ว่าตอนนี้เป็น mode อะไร Quiz,Homework,Practice
    Public Mode As String
    'ตัวแปรเอาไว้เช็คว่าควิซนี้ใช้แท็บเลตทำหรือไม่
    Public CheckQuizIsUseTablet As String = "True"
    'เก็บ TestsetName เพื่อเอาไว้ใช้ทั้งหน้า และ หลังจาก PostBack แล้ว
    Protected Property FullTestsetName As String
        Get
            Return ViewState("_FullTestsetName")
        End Get
        Set(value As String)
            ViewState("_FullTestsetName") = value
        End Set
    End Property


    Dim useCls As New ClassConnectSql

    ''' <summary>
    ''' ทำการ set txt ที่หัว และ สร้าง chart และตาราง ตาม mode และ QuizId ที่ได้รับมา
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OddRow = True
        'Session("newTestSetId") = "4361FCC9-8E95-4237-9737-A5714FC184B4" 'ใหม่
        'Session("newTestSetId") = " 0F20DC7A-4E77-43F5-A3FA-00B906E5C417" 'เก่า
        'Session("newTestSetId") = "08DA4636-C551-4930-AA65-26E9777C3B6A"


        'QueryStringQuizId = "90BB51A9-D58E-409F-B316-F72A2B48CF48" 'สำหรับ Test State1
        'QueryStringQuizId = "CA487E8E-6A55-464B-A2CB-FC4BAD101C80" 'สำหรับ Test State 2 กับ 3
        'QueryStringQuizId = "31A03DD3-691E-4759-BC37-831A1905321F" 'สำหรับ Test State 1 กับ 3

        'QueryStringQuizId = "98B8C1A5-792E-49A2-8969-F684DB6B59D0" '6 - 2 กับ 3
        'QueryStringQuizId = "C6F1F892-ED67-4F27-946F-EF49883FE24B" 'State 1 - 3

        'QueryStringQuizId = "983C37FC-6F6D-4D44-9520-56989F6BB258"


        'If Session("UserId") = Nothing Then
        '    Response.Redirect("~/LoginPage.aspx", False)
        'Else

        'If Session("QuizUseTablet") IsNot Nothing Then
        '    If Session("QuizUseTablet") = False Then
        '        Exit Sub
        '    End If
        'End If

        If Request.QueryString("ShowBtnBack") = "True" Then
            CheckIsFromReportMenu = "True"
        Else
            CheckIsFromReportMenu = ""
        End If

        If Request.QueryString("ReportMenu") IsNot Nothing Then
            'CheckIsFromReportMenu = Request.QueryString("ReportMenu")
            Mode = Request.QueryString("ReportMenu")
            GvPointFirstActivity.Width = 560
        Else
            'CheckIsFromReportMenu = ""
        End If

        If Request.QueryString("QuizId") Is Nothing And Request.QueryString("TestsetId") Is Nothing Then
            Exit Sub
        End If

        If Request.QueryString("QuizId") IsNot Nothing Then
            QueryStringQuizId = Request.QueryString("QuizId").ToString()
        Else
            QueryStringTestsetId = Request.QueryString("TestsetId").ToString()
        End If

        CheckChartOrTable()
        If Not Page.IsPostBack Then
            SetLabelTitle()

            'If ManyRoomInQuiz(Session("newTestSetId")) = True Then

            '    FromNumber.Checked = True
            '    btnOtherRoom.Visible = True
            '    btnWeRoom.Visible = True
            '    GvPointFirstActivity.Style.Add("display", "none")
            '    GvTerms.Style.Add("display", "none")
            '    GvPointRoom.Style.Add("display", "block")


            'End If


            'If CheckTotalQuizMaytime(QueryStringQuizId) = True Then
            '    CurrentState.Value = 2
            '    MainState = 2
            '    GvPointFirstActivity.Style.Add("display", "none")
            '    GvTerms.Style.Add("display", "block")
            '    GvPointRoom.Style.Add("display", "none")
            '    FromNumber.Checked = True
            '    BindDataOnloadState(2)

            'Else

            If (Request.QueryString("QuizId") IsNot Nothing) AndAlso (GetQuizIsUseTablet(QueryStringQuizId) = False) Then
                CheckQuizIsUseTablet = "False"
                DivNoTabletQuiz.Style.Add("display", "block")
                GvTerms.Style.Add("display", "none")
                GvPointRoom.Style.Add("display", "none")
                GvPointFirstActivity.Style.Add("display", "none")
                DivReport.Style.Add("display", "none")
            Else
                CheckQuizIsUseTablet = "True"
                CurrentState.Value = 1
                MainState = 1
                GvPointFirstActivity.Style.Add("display", "block")
                GvPointFirstActivity.Skin = "GirdQuickTest"
                GvTerms.Style.Add("display", "none")
                GvPointRoom.Style.Add("display", "none")
                FromNumber.Checked = True
                BindDataOnloadState(1)
                DivNoTabletQuiz.Style.Add("display", "none")
            End If

        End If

    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="Quiz_Id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckTotalQuizMaytime(ByVal Quiz_Id As String) As Boolean
        Dim dtCheckTotalQuizMaytime As New DataTable
        Dim sqlCheckTotalQuizMaytime As String = "select t360_RoomName,t360_ClassName,TestSet_Id from tblquiz where Quiz_Id = '" & Quiz_Id & "'"
        dtCheckTotalQuizMaytime = useCls.getdata(sqlCheckTotalQuizMaytime)
        If dtCheckTotalQuizMaytime.Rows.Count > 0 Then
            Dim Roomname As String = dtCheckTotalQuizMaytime.Rows(0)("t360_RoomName")
            Dim Classname As String = dtCheckTotalQuizMaytime.Rows(0)("t360_ClassName")
            Dim TestSetId As Guid = dtCheckTotalQuizMaytime.Rows(0)("TestSet_Id")
            Dim sqlCheckTotalManytime As String = "select COUNT(t360_RoomName) as TotalQuiz from tblQuiz where t360_RoomName = '" & Roomname & "' and TestSet_Id = '" & TestSetId.ToString() & "' and t360_ClassName = '" & Classname & "'"
            dtCheckTotalQuizMaytime = useCls.getdata(sqlCheckTotalManytime)
            If dtCheckTotalQuizMaytime.Rows.Count > 0 Then
                If dtCheckTotalQuizMaytime.Rows(0)("TotalQuiz") > 1 Then
                    CheckTotalQuizMaytime = True
                Else
                    CheckTotalQuizMaytime = False
                End If
            End If
        End If
        Return CheckTotalQuizMaytime
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="TestSet_Id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ManyRoomInQuiz(ByVal TestSet_Id As String) As Boolean

        Dim dtCheckTotalRoomInQuiz As New DataTable
        Dim sqlCheckTotalRoomInQuiz As String = "select count(distinct t360_RoomName) as TotalRoom from tblquiz where TestSet_Id = '" & Session("newTestSetId") & "' and t360_ClassName in(select t360_ClassName from tblQuiz where Quiz_Id = '" & QueryStringQuizId.ToString() & "')"
        dtCheckTotalRoomInQuiz = useCls.getdata(sqlCheckTotalRoomInQuiz)
        If dtCheckTotalRoomInQuiz.Rows.Count > 0 Then
            If dtCheckTotalRoomInQuiz.Rows(0)("TotalRoom") > 1 Then
                ManyRoomInQuiz = True
            Else
                ManyRoomInQuiz = False
            End If
        End If
        Return ManyRoomInQuiz

    End Function

    ''' <summary>
    ''' ทำการ Bind Data ให้เป็นการ sort แบบเรียงตามเลขที่
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FromNumber_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FromNumber.CheckedChanged

        If CurrentState.Value IsNot Nothing Then

            If CurrentState.Value = 1 Then
                BindDataOnloadState(1)
            End If
            If CurrentState.Value = 2 Then
                BindDataOnloadState(2)
            End If
            If CurrentState.Value = 3 Then
                BindDataOnloadState(3)
            End If
        End If


    End Sub

    'Private Sub FromLessthanMore_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FromLessthanMore.CheckedChanged
    '    If CurrentState.Value IsNot Nothing Then

    '        If CurrentState.Value = 1 Then
    '            Dim dtTotaldata As New DataView
    '            dtTotaldata = CreateDatatableState1(2)
    '            SetGrid(dtTotaldata, 1)
    '            SetDataForCreateChartState1(dtTotaldata)
    '        End If

    '        If CurrentState.Value = 2 Then
    '            Dim dtView As New DataView
    '            dtView = CreateDatatableState2(2)
    '            SetGrid(dtView, 2)
    '            SetDataForCreateChartState2(dtView)
    '        End If

    '        If CurrentState.Value = 3 Then
    '            Dim dtTotaldata As New DataView
    '            dtTotaldata = CreateDatatableState3(2)
    '            SetGrid(dtTotaldata, 3)
    '            SetDataForCreateChartState3(dtTotaldata)
    '        End If


    '    End If
    'End Sub

    ''' <summary>
    ''' ทำการ Bind Data Sort ให้เป็นแบบเรียงคะแนนจากมากไปน้อย
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FromMorethanLess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FromMorethanLess.CheckedChanged

        If CurrentState.Value IsNot Nothing Then


            If CurrentState.Value = 1 Then
                Dim dtTotaldata As New DataView
                dtTotaldata = CreateDatatableState1(3)
                SetGrid(dtTotaldata, 1)
                SetDataForCreateChartState1(dtTotaldata)
            End If
            If CurrentState.Value = 2 Then
                Dim dtTotaldata As New DataView
                dtTotaldata = CreateDatatableState2(3)
                SetGrid(dtTotaldata, 2)
                SetDataForCreateChartState2(dtTotaldata)
            End If
            If CurrentState.Value = 3 Then
                Dim dtTotaldata As New DataView
                dtTotaldata = CreateDatatableState3(3)
                SetGrid(dtTotaldata, 3)
                SetDataForCreateChartState3(dtTotaldata)
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการสร้าง Highchart ตามข้อมูลที่ได้รับเข้ามา SeriesData,SeriesName 3-7 ไม่ได้ใช้แล้ว
    ''' </summary>
    ''' <param name="YItemTitle">ข้อความบนแกน Y</param>
    ''' <param name="XitemTitle">ข้อความบนแกน X</param>
    ''' <param name="CategoriesX">Data ของแกน X</param>
    ''' <param name="NameToolTip">Str ToolTip เมื่อเอา Mouse ไปชี้ที่ Chart</param>
    ''' <param name="SeriesData">Data ที่นำไปสร้าง Chart</param>
    ''' <param name="SeriesName">ชื่อของข้อมูลนี้</param>
    ''' <param name="SeriesData2">Data ที่ 2 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName2">ชื่อของ Data2 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesData3">Data ที่ 3 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName3">ชื่อของ Data3 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesData4">Data ที่ 4 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName4">ชื่อของ Data4 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesData5">Data ที่ 5 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName5">ชื่อของ Data5 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesData6">Data ที่ 6 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName6">ชื่อของ Data6 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesData7">Data ที่ 7 ถ้ามีส่งเข้ามา</param>
    ''' <param name="SeriesName7">ชื่อของ Data7 ถ้ามีส่งเข้ามา</param>
    ''' <remarks></remarks>
    Private Sub CreateChart(ByVal YItemTitle As String, ByVal XitemTitle As String, ByVal CategoriesX As Object, ByVal NameToolTip As String, ByVal SeriesData As Object, ByVal SeriesName As String, Optional ByVal SeriesData2 As Object = Nothing, Optional ByVal SeriesName2 As String = Nothing, _
                            Optional ByVal SeriesData3 As Object = Nothing, Optional ByVal SeriesName3 As String = Nothing, Optional ByVal SeriesData4 As Object = Nothing, Optional ByVal SeriesName4 As String = Nothing, _
                            Optional ByVal SeriesData5 As Object = Nothing, Optional ByVal SeriesName5 As String = Nothing, Optional ByVal SeriesData6 As Object = Nothing, Optional ByVal SeriesName6 As String = Nothing, _
                            Optional ByVal SeriesData7 As Object = Nothing, Optional ByVal SeriesName7 As String = Nothing)

        If Mode <> "1" Then
            AllChart.Width = 560
        End If

        If DirectCast(SeriesData, Array).Length > 15 Then
            Dim ChartHeight As Integer = (DirectCast(SeriesData, Array).Length) * 30
            AllChart.Height = ChartHeight
        End If

        AllChart.YAxis.Clear()
        Dim YItem As New YAxisItem 'Object ของแกน Y
        Dim titleY As New Highchart.Core.Title(YItemTitle)
        YItem.lineColor = "Gray"
        YItem.minorGridLineColor = "none"
        YItem.title = titleY
        YItem.min = 0
        YItem.allowDecimals = False
        AllChart.YAxis.Add(YItem)



        AllChart.XAxis.Clear()
        Dim XItem As New XAxisItem 'Object ของแกน X
        Dim titleX As New Highchart.Core.Title(XitemTitle)
        XItem.title = titleX
        XItem.lineColor = "Gray"
        XItem.minorGridLineColor = "none"
        XItem.categories = CategoriesX
        AllChart.XAxis.Add(XItem)

        AllChart.Tooltip = New ToolTip(NameToolTip)

        Dim series As New Highchart.Core.SerieCollection 'เป็น Object ที่เอาไว้ใส่ Data เพื่อที่จะนำไปสร้าง Chart


        AllChart.Series.Clear()
        Dim serie1 As New Highchart.Core.Data.Chart.Serie 'ข้อมูลชุดแรกที่จะแสดงใน Chart เช่น กราฟแท่งทั้งชุด
        serie1.data = SeriesData
        serie1.type = RenderType.bar
        serie1.name = SeriesName
        serie1.showInLegend = False
        series.Add(serie1)

        'AllChart.PlotOptions.marker.enabled = False

        If SeriesData2 IsNot Nothing Then
            Dim series2 As New Highchart.Core.Data.Chart.Serie 'ข้อมูลจะแสดงเป็นกราฟเส้น เกณฑ์ผ่านที่

            series2.data = SeriesData2
            series2.type = RenderType.line
            series2.name = SeriesName2
            series2.showInLegend = False
            series.Add(series2)
        End If

        'ที่เหลือน่าจะไม่ได้ใช้แล้ว NA
        If SeriesData3 IsNot Nothing Then
            Dim series3 As New Highchart.Core.Data.Chart.Serie
            series3.data = SeriesData3
            series3.type = RenderType.column
            series3.name = SeriesName3
            series3.showInLegend = False
            series.Add(series3)
        End If

        If SeriesData4 IsNot Nothing Then
            Dim series4 As New Highchart.Core.Data.Chart.Serie
            series4.data = SeriesData4
            series4.type = RenderType.column
            series4.name = SeriesName4
            series4.showInLegend = False
            series.Add(series4)
        End If

        If SeriesData5 IsNot Nothing Then
            Dim series5 As New Highchart.Core.Data.Chart.Serie
            series5.data = SeriesData5
            series5.type = RenderType.column
            series5.name = SeriesName5
            series5.showInLegend = False
            series.Add(series5)
        End If

        If SeriesData6 IsNot Nothing Then
            Dim series6 As New Highchart.Core.Data.Chart.Serie
            series6.data = SeriesData6
            series6.type = RenderType.column
            series6.name = SeriesName6
            series6.showInLegend = False
            series.Add(series6)
        End If

        If SeriesData7 IsNot Nothing Then
            Dim series7 As New Highchart.Core.Data.Chart.Serie
            series7.data = SeriesData7
            series7.type = RenderType.column
            series7.name = SeriesName7
            series7.showInLegend = False
            series.Add(series7)
        End If



        'AllChart.Theme = "pink-floral"
        'AllChart.Theme = "gray"
        'AllChart.Theme = "dark-blue"
        'AllChart.Theme = "dark-green"
        AllChart.Theme = "grid"

        'AllChart.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(5)
        'AllChart.BorderColor = Drawing.Color.Orange

        'AllChart.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)



        AllChart.DataSource = series
        AllChart.DataBind()

    End Sub

    ''' <summary>
    ''' ทำการ Bind Chart และ ตาราง
    ''' </summary>
    ''' <param name="State">คือโหมด ซึ่งตอนนี้จะเหลือแค่ 1 อย่างเดียวแล้ว</param>
    ''' <param name="IsFilter">คะแนนผ่านเกณฑ์ ซึ่ง User จะกรอกเข้ามาจาก txtbox</param>
    ''' <remarks></remarks>
    Private Sub BindDataOnloadState(ByVal State As Integer, Optional ByVal IsFilter As Boolean = False)

        Dim dtOnLoad As New DataTable

        If State = 1 Then
            Dim dtTotalData As New DataView
            dtTotalData = CreateDatatableState1(1)
            If IsFilter = True Then
                If dtTotalData.Count > 0 Then
                    Dim PassFilter As Integer = 0
                    For Each rowView As DataRowView In dtTotalData 'ทำการ loop เพื่อนับจำนวนคนที่คะแนนผ่านเกณฑ์ ในกรณีที่มีการกรอกคะแนนที่ผ่านเกณฑ์เข้ามา , เงื่อนไขการจบ loop คือ วนจนหมด dt 
                        Dim row As DataRow = rowView.Row
                        If row("คะแนน") >= FilterScore Then
                            PassFilter += 1
                        End If
                    Next
                    lblFilterDetail.Text = "ผ่าน " & PassFilter & "/" & dtTotalData.Count
                    lblFilterDetail.Visible = True
                Else
                    lblFilterDetail.Visible = False
                End If
            End If
            SetGrid(dtTotalData, 1)
            SetDataForCreateChartState1(dtTotalData)
            FromNumber.Checked = True

        ElseIf State = 2 Then 'NA
            Dim dtTotalData As New DataView
            dtTotalData = CreateDatatableState2(1)
            SetGrid(dtTotalData, 2)
            SetDataForCreateChartState2(dtTotalData)
            FromNumber.Checked = True

        ElseIf State = 3 Then 'NA
            Dim dtTotaldata As New DataView
            dtTotaldata = CreateDatatableState3(1)
            SetGrid(dtTotaldata, 3)
            SetDataForCreateChartState3(dtTotaldata)
            FromNumber.Checked = True
        End If



    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOtherRoom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOtherRoom.Click

        BindDataOnloadState(3)
        CurrentState.Value = 3
        GvPointFirstActivity.Style.Add("display", "none")
        GvTerms.Style.Add("display", "none")
        GvPointRoom.Style.Add("display", "block")
        btnOtherRoom.Enabled = False
        btnWeRoom.Enabled = True
        'FromLessthanMore.Checked = False
        FromMorethanLess.Checked = False
        FromNumber.Checked = True


    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnWeRoom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnWeRoom.Click

        If MainState = 1 Then
            If CurrentState.Value = 1 Then
                btnWeRoom.Enabled = False
            Else
                BindDataOnloadState(1)
                CurrentState.Value = 1

                GvPointFirstActivity.Style.Add("display", "block")
                GvTerms.Style.Add("display", "none")
                GvPointRoom.Style.Add("display", "none")
                btnWeRoom.Enabled = False
                btnOtherRoom.Enabled = True
                'FromLessthanMore.Checked = False
                FromMorethanLess.Checked = False
                FromNumber.Checked = True

            End If

        Else
            BindDataOnloadState(2)
            CurrentState.Value = 2

            GvPointFirstActivity.Style.Add("display", "none")
            GvTerms.Style.Add("display", "block")
            GvPointRoom.Style.Add("display", "none")
            btnWeRoom.Enabled = False
            btnOtherRoom.Enabled = True
            'FromLessthanMore.Checked = False
            FromMorethanLess.Checked = False
            FromNumber.Checked = True

        End If


    End Sub

    Private Function GetQuizIsUseTablet(Quiz_id As String) As Boolean
        Dim sql As String = "select IsUseTablet from tblquiz where Quiz_Id = '" & Quiz_id & "'"
        Dim isUseTablet As Boolean = useCls.ExecuteScalar(sql)
        Return isUseTablet
    End Function
    ''' <summary>
    ''' ทำการ select หาข้อมูลตามเงื่อนไขต่างๆเพื่อนำข้อมูลไปสร้างเป็น Chart และ ตาราง
    ''' </summary>
    ''' <param name="SortOrder">คือตัวแปรที่บอกว่าจะให้ sort ข้อมูลยังไง 1 = sort ตามเลขที่ , 2 = sort คะแนนน้อยไปมาก , 3 = คะแนนมากไปน้อย</param>
    ''' <param name="IsForExportExcel">ตัวแปรที่บอกว่าจะให้ทำข้อมูลเพื่อนำไป Export เป็น File Excel หรือเปล่า</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDatatableState1(ByVal SortOrder As Integer, Optional ByVal IsForExportExcel As Boolean = False)
        Dim dtTotalData As New DataTable
        Dim dvData As New DataView
        Dim sql1 As String = ""
        Dim dt1 As New DataTable

        'Quiz'
        If Mode = 1 Then
            sql1 = "select t360_ClassName,t360_RoomName,t360_SchoolCode from tblquiz where Quiz_Id = '" & QueryStringQuizId & "'"
            dt1 = useCls.getdata(sql1)
            If dt1.Rows.Count > 0 Then
                Dim ClassName As String = dt1.Rows(0)("t360_ClassName")
                Dim RoomName As String = dt1.Rows(0)("t360_RoomName")
                Dim SchoolCode As String = dt1.Rows(0)("t360_SchoolCode")
                'Dim sql2 As String = " SELECT ts.Student_CurrentNoInRoom, SUM(qs.Score) AS totalscore,RANK() over(order by sum(qs.score)DESC)as IndexStudent " & _
                '                     " FROM t360_tblStudent AS ts LEFT OUTER JOIN " & _
                '                     " tblQuizScore AS qs ON ts.School_Code = qs.School_Code AND ts.Student_Id = qs.Student_Id " & _
                '                     " where (qs.Quiz_Id = '" & QueryStringQuizId & "' or qs.Quiz_Id  is null) and ts.Student_CurrentClass = '" & ClassName & "' and ts.Student_CurrentRoom = '" & RoomName & "' " & _
                '                     " AND ts.School_Code = '" & SchoolCode & "' AND ts.Student_IsActive = 1 group by ts.Student_CurrentNoInRoom order by ts.Student_CurrentNoInRoom "
                'sql2 ข้อมูลที่จะนำไปสร้าง Chart ใน Mode Quiz
                'Dim sql2 As String = " SELECT CASE WHEN dbo.tblQuizSession.IsActive = 0 THEN CAST(Student_CurrentNoInRoom AS VARCHAR(10)) + '<br /><span style=""color:red;font-size:13px !important;font:initial;"">(ไม่เข้าทำ)</span>' ELSE " & _
                '                     " CAST(Student_CurrentNoInRoom AS VARCHAR(10)) END AS Student_CurrentNoInRoom,t1.Student_FirstName + ' ' + t1.Student_LastName AS StudentName,t1.Student_CurrentClass + '' + t1.Student_CurrentRoom AS StudentClassRomm,  " & _
                '                     " t1.totalscore,IndexStudent FROM (SELECT ts.Student_CurrentNoInRoom,ts.Student_FirstName,ts.Student_LastName,ts.Student_CurrentClass,ts.Student_CurrentRoom,SUM(qs.Score) AS totalscore,RANK() over(order by sum(qs.score)DESC)as IndexStudent,ts.Student_Id,qs.Quiz_Id " & _
                '                     " FROM t360_tblStudent AS ts LEFT OUTER JOIN  tblQuizScore AS qs ON ts.School_Code = qs.School_Code AND ts.Student_Id = qs.Student_Id  " & _
                '                     " where (qs.Quiz_Id = '" & QueryStringQuizId & "' or qs.Quiz_Id  is null) and ts.Student_CurrentClass = '" & ClassName & "' and ts.Student_CurrentRoom = '" & RoomName & "'  " & _
                '                     " AND ts.School_Code = '" & SchoolCode & "' AND ts.Student_IsActive = 1 group by ts.Student_CurrentNoInRoom,ts.Student_Id,qs.Quiz_Id,ts.Student_FirstName,ts.Student_LastName,ts.Student_CurrentClass,ts.Student_CurrentRoom) AS t1 " & _
                '                     " INNER Join dbo.tblQuizSession ON t1.Student_Id = dbo.tblQuizSession.Player_Id AND t1.Quiz_Id = dbo.tblQuizSession.Quiz_Id " & _
                '                     " ORDER BY t1.Student_CurrentNoInRoom "

                Dim sql2 As String = " SELECT case when qss.IsActive = 0 then CAST(ts.Student_CurrentNoInRoom AS VARCHAR(10)) + '<br /><span style=""color:red;font-size:13px !important;font:initial;"">(ไม่เข้าทำ)</span>'" & _
                                     " ELSE  CAST(  ts.Student_CurrentNoInRoom AS VARCHAR(10)) END AS Student_CurrentNoInRoom,ts.Student_Code" & _
                                     " ,ts.Student_FirstName , ts.Student_LastName ,ts.Student_CurrentClass + ' ' + ts.Student_CurrentRoom AS StudentClassRomm ,SUM(isnull(qs.Score,0)) AS totalscore," & _
                                     " RANK() over(order by SUM(qs.Score) DESC)as IndexStudent" & _
                                     " FROM (select * from tblQuizSession where Quiz_Id = '" & QueryStringQuizId & "') qss inner join t360_tblStudent AS ts on qss.Player_Id = ts.Student_Id LEFT JOIN  (select * from tblQuizScore " & _
                                     " where Quiz_Id = '" & QueryStringQuizId & "' ) qs" & _
                                     " on ts.Student_Id=qs.Student_Id " & _
                                     " where ts.Student_CurrentClass = '" & ClassName & "' and ts.Student_CurrentRoom = '" & RoomName & "'   " & _
                                     " AND ts.School_Code = '" & SchoolCode & "' AND ts.Student_IsActive = 1" & _
                                     " group by ts.Student_CurrentNoInRoom,qs.Quiz_Id,ts.Student_FirstName,ts.Student_LastName,ts.Student_CurrentClass,ts.Student_CurrentRoom,qss.IsActive,ts.Student_Code" & _
                                     " order by ts.Student_CurrentNoInRoom"

                dt1.Clear()
                dt1 = useCls.getdata(sql2)
                If IsForExportExcel = True Then
                    Return dt1
                Else
                    dt1.Columns.Remove("Student_FirstName")
                    dt1.Columns.Remove("Student_LastName")
                    dt1.Columns.Remove("StudentClassRomm")
                    dt1.Columns.Remove("Student_Code")
                End If
            End If


            'Homework'
        ElseIf Mode = 2 Then
            'sql1 = " SELECT t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom " & _
            '       " AS VARCHAR(100)) AS Student_CurrentNoInRoom, CASE WHEN SUM(tblQuizScore.Score) <> 0 THEN SUM(dbo.tblQuizScore.Score) ELSE 0 END AS totalscore " & _
            '       " ,RANK() over(order by sum(dbo.tblQuizScore.score)DESC)as IndexStudent FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = " & _
            '       " tblQuizScore.Quiz_Id INNER JOIN tblModuleDetailCompletion ON tblQuiz.Quiz_Id = tblModuleDetailCompletion.Quiz_Id RIGHT OUTER JOIN " & _
            '       " t360_tblStudent ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id AND tblQuizScore.Student_Id = t360_tblStudent.Student_Id " & _
            '       " WHERE (tblModuleDetailCompletion.Quiz_Id = '" & QueryStringQuizId & "' OR dbo.tblModuleDetailCompletion.Quiz_Id IS NULL) " & _
            '       " GROUP BY t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(100)) " & _
            '       " ,t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom , t360_tblStudent.Student_CurrentNoInRoom " & _
            '       " ORDER BY t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom , t360_tblStudent.Student_CurrentNoInRoom "

            'sql1 ข้อมูลที่จะนำไปสร้าง Chart ใน Mode การบ้าน มี join ไปหา tblModule ของพี่เต้อ
            sql1 = " SELECT t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + " & _
                   " CAST(t360_tblStudent.Student_CurrentNoInRoom  AS VARCHAR(100)) AS Student_CurrentNoInRoom, " & _
                   " Student_FirstName + ' ' + Student_LastName AS StudentName, CASE WHEN SUM(tblQuizScore.Score) <> 0 THEN SUM(dbo.tblQuizScore.Score) ELSE 0 END AS totalscore  " & _
                   " ,RANK() over(order by sum(dbo.tblQuizScore.score)DESC)as IndexStudent FROM tblModuleDetailCompletion INNER JOIN " & _
                   " t360_tblStudent ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id LEFT OUTER JOIN " & _
                   " tblQuizScore ON tblModuleDetailCompletion.Quiz_Id = tblQuizScore.Quiz_Id AND t360_tblStudent.Student_Id = tblQuizScore.Student_Id " & _
                   " WHERE (tblModuleDetailCompletion.Quiz_Id = '" & QueryStringQuizId & "') " & _
                   " GROUP BY t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(100)), " & _
                   " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom,t360_tblStudent.Student_CurrentNoInRoom,Student_FirstName + ' ' + Student_LastName " & _
                   " ORDER BY t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom,t360_tblStudent.Student_CurrentNoInRoom "
            dt1.Clear()
            dt1 = useCls.getdata(sql1)
            If IsForExportExcel = True Then
                Return dt1
            End If


            'Practice'
        ElseIf Mode = 3 Then
            Dim KnSession As New KNAppSession()
            Dim CalendarId As String = KnSession("CurrentCalendarId").ToString()
            'sql1 = " SELECT ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) AS Totalscore, " & _
            '       " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(100)) + " & _
            '       " '<br />(ทำ ' + CAST(COUNT(DISTINCT dbo.tblQuiz.Quiz_Id) AS VARCHAR(1000)) + ' ครั้ง)'  AS Student_CurrentNoInRoom, " & _
            '       " RANK() over(order by ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) DESC)as IndexStudent " & _
            '       " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id INNER JOIN " & _
            '       " uvw_SumScorePerTestset ON tblQuiz.TestSet_Id = uvw_SumScorePerTestset.TestSet_Id WHERE (tblQuiz.TestSet_Id = '" & QueryStringTestsetId & "') " & _
            '       " AND (tblQuiz.IsPracticeMode = 1) AND (tblQuiz.Calendar_Id = '" & CalendarId & "') AND (dbo.tblQuiz.IsActive = 1) " & _
            '       " GROUP BY tblQuizScore.Student_Id, t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom, t360_tblStudent.Student_CurrentNoInRoom, " & _
            '       " uvw_SumScorePerTestset.FullScorePerTestset ORDER BY dbo.t360_tblStudent.Student_CurrentClass,dbo.t360_tblStudent.Student_CurrentRoom,dbo.t360_tblStudent.Student_CurrentNoInRoom "
            'sql1 ข้อมูลที่จะนำไปสร้าง Chart ใน Mode ฝึกฝน เป็นการหาคะแนนของการทำฝึกฝนแต่ละชุด ไม่ใช่คะแนนดิบๆเป็นครั้งๆเหมือนโหมด Quiz เพราะว่าการทำฝึกฝนทำซ้ำๆในชุดเดิมได้
            'sql1 = " SELECT ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) AS Totalscore, " & _
            '       " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(100)) + " & _
            '       " '<br />(ทำ ' + CAST(COUNT(DISTINCT dbo.tblQuiz.Quiz_Id) AS VARCHAR(1000)) + ' ครั้ง)'  AS Student_CurrentNoInRoom,Student_FirstName + '' + Student_LastName AS StudentName, " & _
            '       " RANK() over(order by ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) DESC)as IndexStudent " & _
            '       " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id INNER JOIN " & _
            '       " uvw_SumScorePerTestset ON tblQuiz.TestSet_Id = uvw_SumScorePerTestset.TestSet_Id WHERE (tblQuiz.TestSet_Id = '" & QueryStringTestsetId & "') " & _
            '       " AND (tblQuiz.IsPracticeMode = 1) AND (dbo.tblQuiz.IsActive = 1) " & _
            '       " GROUP BY tblQuizScore.Student_Id, t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom, t360_tblStudent.Student_CurrentNoInRoom, " & _
            '       " uvw_SumScorePerTestset.FullScorePerTestset,Student_FirstName + '' + Student_LastName ORDER BY dbo.t360_tblStudent.Student_CurrentClass,dbo.t360_tblStudent.Student_CurrentRoom,dbo.t360_tblStudent.Student_CurrentNoInRoom "

            sql1 = " SELECT t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom + ' เลขที่ ' + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(100)) +  " & _
                   "'<br />(ทำ ' + CAST(COUNT(DISTINCT dbo.tblQuiz.Quiz_Id) AS VARCHAR(1000)) + ' ครั้ง)'  AS Student_CurrentNoInRoom," & _
                   " Student_FirstName + ' ' + Student_LastName AS StudentName, " & _
                   " ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) AS Totalscore, " & _
                   " RANK() over(order by ROUND(( SUM(tblQuizScore.Score) * 100 ) / ( COUNT(DISTINCT tblQuiz.Quiz_Id) * uvw_SumScorePerTestset.FullScorePerTestset ),2) DESC)as IndexStudent " & _
                   " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id  " & _
                   " INNER JOIN  uvw_SumScorePerTestset ON tblQuiz.TestSet_Id = uvw_SumScorePerTestset.TestSet_Id WHERE (tblQuiz.TestSet_Id = '" & QueryStringTestsetId & "') " & _
                   " AND (tblQuiz.IsPracticeMode = 1) AND (dbo.tblQuiz.IsActive = 1)  GROUP BY tblQuizScore.Student_Id, t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom,  " & _
                   " t360_tblStudent.Student_CurrentNoInRoom,  uvw_SumScorePerTestset.FullScorePerTestset,Student_FirstName + ' ' + Student_LastName " & _
                   " ORDER BY dbo.t360_tblStudent.Student_CurrentClass,dbo.t360_tblStudent.Student_CurrentRoom,dbo.t360_tblStudent.Student_CurrentNoInRoom  "
            dt1.Clear()
            dt1 = useCls.getdata(sql1)
            KnSession = Nothing
            If IsForExportExcel = True Then
                Return dt1
            End If
        End If

        If dt1.Rows.Count > 0 Then
            dtTotalData.Columns.Add("เลขที่", GetType(String))
            dtTotalData.Columns.Add("คะแนน", GetType(Double))
            dtTotalData.Columns.Add("ลำดับ", GetType(Integer))

            For a = 0 To dt1.Rows.Count - 1 'ทำการ loop เพื่อนำข้อมูลจาก dt1 มาใส่ใน dtTotalData อีกทีนึง , เงือนไขการจบ loop คือ วนจนครบหมดทุก Row ใน dt1
                Dim EachStudentNo As String = dt1.Rows(a)("Student_CurrentNoInRoom")
                dtTotalData.Rows.Add(a)("เลขที่") = EachStudentNo
                Dim EachSumScore As Double = 0
                If dt1.Rows(a)("totalscore") IsNot DBNull.Value Then
                    EachSumScore = dt1.Rows(a)("totalscore")
                    dtTotalData.Rows(a)("คะแนน") = EachSumScore
                Else
                    dtTotalData.Rows(a)("คะแนน") = 0
                End If
                Dim EachIndex As Integer = dt1.Rows(a)("IndexStudent")
                dtTotalData.Rows(a)("ลำดับ") = EachIndex
            Next

            If SortOrder = 1 Then
                dvData = New DataView(dtTotalData)
            ElseIf SortOrder = 2 Then
                dvData = New DataView(dtTotalData)
                dvData.Sort = "คะแนน"
            ElseIf SortOrder = 3 Then
                dvData = New DataView(dtTotalData)
                dvData.Sort = "ลำดับ,เลขที่"
            End If
        End If





        Return dvData

    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="SortOrder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDatatableState2(ByVal SortOrder As Integer) As DataView
        Dim sql1 As String = " select t360_ClassName,t360_RoomName,t360_SchoolCode from tblQuiz where Quiz_Id = '" & QueryStringQuizId & "' "
        Dim dt As New DataTable
        Dim dtTotalQuiz As DataTable
        Dim dvData As DataView
        dt = useCls.getdata(sql1)
        If dt.Rows.Count > 0 Then
            Dim ClassName As String = dt.Rows(0)("t360_ClassName")
            Dim RoomName As String = dt.Rows(0)("t360_RoomName")
            Dim SchoolCode As String = dt.Rows(0)("t360_SchoolCode")
            'Dim AcademicYears As Integer = CInt(dt.Rows(0)("AcademicYear"))
            dtTotalQuiz = New DataTable
            'dtTotalQuiz.Columns.Add("เลขที่", GetType(String))

            Dim sqlSelectTotalquiz As String = " select Quiz_Id,LastUpdate from tblQuiz where TestSet_Id = '" & Session("newTestSetId") & "' " & _
                                               " and t360_ClassName = '" & ClassName & "' " & _
                                               " AND t360_SchoolCode = '" & SchoolCode & "' AND IsActive = 1 and " & _
                                               " t360_RoomName ='" & RoomName & "'  order by LastUpdate "
            Dim dtTotalSumQuiz As New DataTable
            dtTotalSumQuiz = useCls.getdata(sqlSelectTotalquiz)
            lblCompare.Text = "เปรียบเทียบ " & dtTotalSumQuiz.Rows.Count & " ครั้ง"

            Dim sqlSelectStudent As String = " SELECT dbo.t360_tblStudent.Student_CurrentNoInRoom,dbo.tblQuizScore.Student_Id  " & _
                                             " FROM  dbo.t360_tblStudent LEFT OUTER JOIN  dbo.tblQuizScore " & _
                                             " ON dbo.t360_tblStudent.School_Code = dbo.tblQuizScore.School_Code AND " & _
                                             " dbo.t360_tblStudent.Student_Id = dbo.tblQuizScore.Student_Id  " & _
                                             " WHERE     (dbo.t360_tblStudent.Student_CurrentClass = '" & ClassName & "') AND " & _
                                             "  (dbo.t360_tblStudent.School_Code = '" & SchoolCode & "') AND (dbo.t360_tblStudent.Student_IsActive = 1) " & _
                                             " AND (dbo.t360_tblStudent.Student_CurrentRoom = '" & RoomName & "') " & _
                                             " GROUP BY dbo.t360_tblStudent.Student_CurrentNoInRoom,dbo.tblQuizScore.Student_Id  "
            Dim dtTotalStudent As New DataTable
            dtTotalStudent = useCls.getdata(sqlSelectStudent)
            Dim CountSumQuiz As Integer = dtTotalSumQuiz.Rows.Count
            Dim ArrTotalQuiz As New ArrayList


            If dtTotalStudent.Rows.Count > 0 Then

                dtTotalQuiz.Columns.Add("เลขที่", GetType(String))
                Dim CountAllQuiz As Integer = dtTotalSumQuiz.Rows.Count
                For a = 0 To dtTotalStudent.Rows.Count - 1
                    Dim SID As String = ""
                    'If dtTotalStudent.Rows(a)("Student_Id") IsNot DBNull.Value Then
                    SID = dtTotalStudent.Rows(a)("Student_Id").ToString()
                    'End If
                    dtTotalQuiz.Rows.Add(a)("เลขที่") = dtTotalStudent.Rows(a)("Student_CurrentNoInRoom")
                    '------------------------------------------------------------------------------------------------------------------------
                    For b = 0 To dtTotalSumQuiz.Rows.Count - 1
                        Dim QuizID As String = dtTotalSumQuiz.Rows(b)("Quiz_Id").ToString()
                        If a = 0 Then
                            dtTotalQuiz.Columns.Add("ครั้งที่ " & b + 1, GetType(String))
                        End If
                        'If SID Is Nothing Then
                        'dtTotalQuiz.Rows(a)("ครั้งที่ " & b + 1) = "0"
                        'Else
                        Dim sqlSelectQuiz As String = "select SUM(Score)as totalscore  from tblQuizScore where Quiz_Id = '" & QuizID & "' and Student_Id = '" & SID & "' "
                        Dim dtEachQuiz As New DataTable
                        dtEachQuiz = useCls.getdata(sqlSelectQuiz)
                        If dtEachQuiz.Rows(0)("totalscore") IsNot DBNull.Value Then
                            dtTotalQuiz.Rows(a)("ครั้งที่ " & b + 1) = dtEachQuiz.Rows(0)("totalscore")
                        Else
                            dtTotalQuiz.Rows(a)("ครั้งที่ " & b + 1) = 0
                        End If

                        'End If


                        'If dtEachQuiz.Rows.Count > 0 Then

                        'Else
                        '    'dtTotalQuiz.Rows(a)("ครั้งที่ " & b + 1) = "0"
                        'End If
                        '--------------------------------------------------------------------------------------------------------------------------------------------
                    Next
                    '-----------------------------------------------------------------------------------------------------------------------------------------------
                Next
                dtTotalQuiz.Columns.Add("คะแนนเฉลี่ย", GetType(Double))
                dtTotalQuiz.Columns.Add("พัฒนาการ", GetType(String))
                dtTotalQuiz.Columns.Add("ลำดับที่", GetType(String))

                For c = 0 To dtTotalStudent.Rows.Count - 1
                    Dim FirstScore As Double = CDbl(dtTotalQuiz.Rows(c)(1))
                    Dim LastScore As Double = CDbl(dtTotalQuiz.Rows(c)(CountSumQuiz))
                    Dim AvgScore As Double = (FirstScore + LastScore) / CDbl(CountSumQuiz)
                    dtTotalQuiz.Rows(c)("คะแนนเฉลี่ย") = AvgScore

                    If FirstScore > LastScore Then
                        'dtTotalQuiz.Rows(c)("พัฒนาการ") = "แย่ลง"
                        dtTotalQuiz.Rows(c)("พัฒนาการ") = "<img src='../Images/Activity/arrow-Down-icon.png' style='width:40px;' />"
                    ElseIf FirstScore = LastScore Then
                        'dtTotalQuiz.Rows(c)("พัฒนาการ") = "เท่าเดิม"
                        dtTotalQuiz.Rows(c)("พัฒนาการ") = "<img src='../Images/Activity/Rectangle_.png' style='width:30px;height:9px;' />"
                    ElseIf FirstScore < LastScore Then
                        'dtTotalQuiz.Rows(c)("พัฒนาการ") = "ดีขึ้น"
                        'dtTotalQuiz.Rows(c)("พัฒนาการ") = "<img src='../Images/Activity/arrow-up-icon.png' style='width:40px;' />"
                        dtTotalQuiz.Rows(c)("พัฒนาการ") = "<img src='../Images/Activity/arrow-Down-icon.png' style='width:40px;' />"
                    End If
                Next

                Dim dv = dtTotalQuiz.DefaultView
                dv.Sort = "คะแนนเฉลี่ย desc"

                Dim Index As Integer = 1
                Dim Order As Integer = 1
                Dim ScoreOld As Double = -1
                For Each v In dv
                    Dim row = dtTotalQuiz.Select("เลขที่='" & v("เลขที่") & "'")
                    If ScoreOld <> row(0)("คะแนนเฉลี่ย") Then
                        Order = Index
                    End If
                    row(0)("ลำดับที่") = Order
                    ScoreOld = row(0)("คะแนนเฉลี่ย")
                    Index += 1
                Next
                '-----------------------------------------------------------------------------------------------------------------------------------------------

                If SortOrder = 1 Then
                    dvData = New DataView(dtTotalQuiz)
                ElseIf SortOrder = 2 Then
                    dvData = New DataView(dtTotalQuiz)
                    dvData.Sort = "คะแนนเฉลี่ย"
                ElseIf SortOrder = 3 Then
                    dvData = New DataView(dtTotalQuiz)
                    dvData.Sort = "คะแนนเฉลี่ย DESC"
                End If


            End If

        End If

        Return dvData


    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="SortOrder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDatatableState3(ByVal SortOrder As Integer) As DataView

        Dim sql1 As String = " select distinct t360_ClassName,t360_RoomName,t360_SchoolCode " & _
                             " from tblQuiz where TestSet_Id = '" & Session("newTestSetId") & "' " & _
                             " and t360_ClassName in (select t360_ClassName from tblQuiz where Quiz_Id = '" & QueryStringQuizId & "' " & _
                             " AND t360_SchoolCode = (SELECT t360_SchoolCode FROM dbo.tblQuiz WHERE Quiz_Id = '" & QueryStringQuizId & "') ) "
        Dim dt1 As New DataTable
        Dim dtTotalData As New DataTable
        Dim dvdata As New DataView
        dt1 = useCls.getdata(sql1)
        If dt1.Rows.Count > 0 Then

            lblCompare.Text = "เปรียบเทียบกับห้องอื่น"

            dtTotalData.Columns.Add("ห้อง", GetType(String))
            dtTotalData.Columns.Add("คะแนนต่ำสุด", GetType(Double))
            dtTotalData.Columns.Add("คะแนนสูงสุด", GetType(Double))
            dtTotalData.Columns.Add("คะแนนเฉลี่ย", GetType(Double))
            dtTotalData.Columns.Add("ลำดับที่", GetType(Integer))
            Dim TopScore As Double
            Dim LowScore As Double
            Dim Avgscore As Double

            For a = 0 To dt1.Rows.Count - 1
                Dim ClassName As String = dt1.Rows(a)("t360_ClassName")
                Dim RoomName As String = dt1.Rows(a)("t360_RoomName")
                Dim SchoolCode As String = dt1.Rows(a)("t360_SchoolCode")
                Dim ClassAndRommName As String = ClassName & RoomName
                dtTotalData.Rows.Add(a)("ห้อง") = ClassAndRommName
                Dim sqlFromLeesToMore As String = "select top 1 SumScorePerQuiz as totalscore from uvw_SumScorePerQuizPerStudent " & _
                              " where TestSet_Id = '" & Session("newTestSetId") & "' and t360_ClassName = '" & ClassName & "' " & _
                              " and t360_RoomName = '" & RoomName & "' and t360_SchoolCode = '" & SchoolCode & "' order by SumScorePerQuiz "
                Dim dtLessToMore As New DataTable
                dtLessToMore = useCls.getdata(sqlFromLeesToMore)

                If dtLessToMore.Rows.Count > 0 Then
                    dtTotalData.Rows(a)("คะแนนต่ำสุด") = dtLessToMore.Rows(0)("totalscore")
                    LowScore = dtLessToMore.Rows(0)("totalscore")
                Else
                    dtTotalData.Rows(a)("คะแนนต่ำสุด") = 0
                    LowScore = 0
                End If

                Dim sqlFromMoreToLess As String = "select top 1 SumScorePerQuiz as totalscore from uvw_SumScorePerQuizPerStudent " & _
                              " where TestSet_Id = '" & Session("newTestSetId") & "' and t360_ClassName = '" & ClassName & "' " & _
                              " and t360_SchoolCode = '" & SchoolCode & "' and t360_RoomName = '" & RoomName & "' order by SumScorePerQuiz Desc "
                Dim dtMoreToLess As New DataTable
                dtMoreToLess = useCls.getdata(sqlFromMoreToLess)

                If dtMoreToLess.Rows.Count > 0 Then
                    dtTotalData.Rows(a)("คะแนนสูงสุด") = dtMoreToLess.Rows(0)("totalscore")
                    TopScore = dtMoreToLess.Rows(0)("totalscore")
                Else
                    dtTotalData.Rows(a)("คะแนนสูงสุด") = 0
                    TopScore = 0
                End If

                Avgscore = (TopScore + LowScore) / 2
                dtTotalData.Rows(a)("คะแนนเฉลี่ย") = Avgscore

            Next
            '-------------------------------------------------------------------------------------------------------------------------------------------------
            Dim dv = dtTotalData.DefaultView
            dv.Sort = "คะแนนเฉลี่ย desc"

            Dim Index As Integer = 1
            Dim Order As Integer = 1
            Dim ScoreOld As Double = -1
            For Each v In dv
                Dim row = dtTotalData.Select("ห้อง='" & v("ห้อง") & "'")
                If ScoreOld <> row(0)("คะแนนเฉลี่ย") Then
                    Order = Index
                End If
                row(0)("ลำดับที่") = Order
                ScoreOld = row(0)("คะแนนเฉลี่ย")
                Index += 1
            Next
            '-------------------------------------------------------------------------------------------------------------------------------------------------

            If SortOrder = 1 Then
                dvdata = New DataView(dtTotalData)
            ElseIf SortOrder = 2 Then
                dvdata = New DataView(dtTotalData)
                dvdata.Sort = "คะแนนเฉลี่ย"
            ElseIf SortOrder = 3 Then
                dvdata = New DataView(dtTotalData)
                dvdata.Sort = "คะแนนเฉลี่ย Desc"
            End If

        End If

        Return dvdata

    End Function

    ''' <summary>
    ''' ทำการ Bind Grid โดยใช้ข้อมูลจาก dt ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="dtData">DT ที่มีข้อมูลมาเรียบร้อยแล้ว ซึ่งได้มาจาก Function CreateDatatableState1()</param>
    ''' <param name="ChooseGrid">เลือกให้ Bind ที่ Grid ไหนตามเงื่อนไข แต่ตอนนี้ใช้แค่ 1 อย่างเดียว</param>
    ''' <remarks></remarks>
    Private Sub SetGrid(ByVal dtData As DataView, ByVal ChooseGrid As Integer)

        If dtData.Count > 0 Then
            If dtData IsNot Nothing Then

                If ChooseGrid = 1 Then
                    GvPointFirstActivity.DataSource = dtData
                    GvPointFirstActivity.DataBind()
                ElseIf ChooseGrid = 2 Then
                    GvTerms.DataSource = dtData
                    GvTerms.DataBind()
                ElseIf ChooseGrid = 3 Then
                    GvPointRoom.DataSource = dtData
                    GvPointRoom.DataBind()
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการดึงข้อมูลต่างๆจาก dt มาเพื่อทำตัวแปรเพื่อนำไปสร้าง chart
    ''' </summary>
    ''' <param name="dtData">DT ข้อมูลที่ได้จาก Function CreateDatatableState1()</param>
    ''' <remarks></remarks>
    Private Sub SetDataForCreateChartState1(ByVal dtData As DataView)

        If dtData.Count > 0 Then
            If dtData IsNot Nothing Then
                Dim NewdtTable As New DataTable
                NewdtTable = dtData.ToTable

                Dim TitleY As String = ""
                If Mode = 3 Then
                    TitleY = "เปอร์เซ็นต์เฉลี่ย"
                Else
                    TitleY = "คะแนน"
                End If

                Dim TitleX As String = ""
                If Mode = 1 Then
                    TitleX = "เลขที่"
                Else
                    TitleX = "เลขที่-ห้อง"
                End If

                'Dim SeriesName As String = "ครั้งที่ 1"
                Dim SeriesName As String = "คะแนนที่ทำได้"

                Dim CateGoriesX(NewdtTable.Rows.Count - 1) As Object 'เอาไว้เก็บข้อมูลของแกน X
                Dim SeriesData(NewdtTable.Rows.Count - 1) As Object 'เก็บข้อมูลของแกน Y
                Dim SeriesLineData(NewdtTable.Rows.Count - 1) As Object 'เก็บข้อมูลของกราฟเส้น

                For a = 0 To NewdtTable.Rows.Count - 1 'ทำการวน loop เพื่อสร้าง object Data เพื่อนำไปสร้าง Chart ต่อ , เงื่อนไขการจบ loop คือ วนจนหมด dt
                    Dim StudentNo As String = NewdtTable.Rows(a)("เลขที่")
                    CateGoriesX(a) = StudentNo
                    Dim EachScore As Double = NewdtTable.Rows(a)("คะแนน")
                    SeriesData(a) = EachScore
                    If FilterScore <> 0 Then
                        SeriesLineData(a) = FilterScore
                    End If
                Next

                'tooltip ของกราฟแต่ละแท่ง
                Dim StrToolTip As String = "'<b>' + this.series.name + '</b>' + '<br />' + 'เลขที่ ' + '<b>' + this.x  + '</b>' + '<br />' + '<b>' + this.y + '</b>' + ' คะแนน'"

                If Mode = 3 Then
                    SeriesName = "เปอร์เซ็นต์เฉลี่ยที่ทำได้"
                    StrToolTip = "'<b>' + this.series.name + '</b>' + '<br />' + 'เลขที่ ' + '<b>' + this.x  + '</b>' + '<br />' + '<b>' + this.y + '</b>' + '%'"
                End If

                If FilterScore <> 0 Then
                    CreateChart(TitleY, TitleX, CateGoriesX, StrToolTip, SeriesData, SeriesName, SeriesLineData, "เกณฑ์คะแนน")
                Else
                    CreateChart(TitleY, TitleX, CateGoriesX, StrToolTip, SeriesData, SeriesName)
                End If

            End If
        End If

    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="dtData"></param>
    ''' <remarks></remarks>
    Private Sub SetDataForCreateChartState2(ByVal dtData As DataView)

        If dtData.Count > 0 Then
            If dtData IsNot Nothing Then

                Dim TitleY As String = "คะแนน"
                Dim TitleX As String = "เลขที่นักเรียน"
                Dim SerieNameAvg As String = "คะแนนเฉลี่ย"
                Dim CateGoriesX(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz1(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz2(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz3(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz4(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz5(dtData.Table.Rows.Count - 1) As Object
                Dim Quiz6(dtData.Table.Rows.Count - 1) As Object
                Dim AVGScore(dtData.Table.Rows.Count - 1) As Object
                Dim SerieName1, SerieName2, SerieName3, SerieName4, SerieName5, SerieName6, SerieName7 As String

                Dim NewdtTable As New DataTable
                NewdtTable = dtData.ToTable

                For i = 0 To NewdtTable.Rows.Count - 1
                    Dim EachDataX As String = NewdtTable.Rows(i)("เลขที่")
                    CateGoriesX(i) = EachDataX
                    Dim EachAvg As Double = NewdtTable.Rows(i)("คะแนนเฉลี่ย")
                    AVGScore(i) = EachAvg
                Next
                Dim Test As Integer = NewdtTable.Columns.Count - 4
                For j = 1 To NewdtTable.Columns.Count - 4

                    For k = 0 To NewdtTable.Rows.Count - 1

                        Dim EachDataScore As Double = NewdtTable.Rows(k)(j)

                        If j = 1 Then
                            Quiz1(k) = EachDataScore
                            SerieName1 = "ครั้งที่ 1"
                        ElseIf j = 2 Then
                            Quiz2(k) = EachDataScore
                            SerieName2 = "ครั้งที่ 2"
                        ElseIf j = 3 Then
                            Quiz3(k) = EachDataScore
                            SerieName3 = "ครั้งที่ 3"
                        ElseIf j = 4 Then
                            Quiz4(k) = EachDataScore
                            SerieName4 = "ครั้งที่ 4"
                        ElseIf j = 5 Then
                            Quiz5(k) = EachDataScore
                            SerieName5 = "ครั้งที่ 5"
                        ElseIf j = 6 Then
                            Quiz6(k) = EachDataScore
                            SerieName6 = "ครั้งที่ 6"
                        End If

                    Next

                Next


                If Quiz1(0) Is Nothing Then
                    Quiz1 = Nothing
                End If

                If Quiz2(0) Is Nothing Then
                    Quiz2 = Nothing
                End If

                If Quiz3(0) Is Nothing Then
                    Quiz3 = Nothing
                End If

                If Quiz4(0) Is Nothing Then
                    Quiz4 = Nothing
                End If

                If Quiz5(0) Is Nothing Then
                    Quiz5 = Nothing
                End If

                If Quiz6(0) Is Nothing Then
                    Quiz6 = Nothing
                End If

                Dim StrToolTip As String = "'<b>' + this.series.name + '</b>' + '<br />' + 'เลขที่ ' + '<b>' + this.x  + '</b>' + '<br />' + '<b>' + this.y + '</b>' + ' คะแนน'"

                'CreateChart(TitleY,TitleX,CateGoriesX,StrToolTip,Quiz1,SerieName1,Quiz2,SerieName2,Quiz3,SerieName3,SerieName4
                CreateChart(TitleY, TitleX, CateGoriesX, StrToolTip, AVGScore, SerieNameAvg, Quiz1, SerieName1, Quiz2, SerieName2, Quiz3, SerieName3, Quiz4, SerieName4, Quiz5, SerieName5, Quiz6, SerieName6)
            End If
        End If

    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="dtData"></param>
    ''' <remarks></remarks>
    Private Sub SetDataForCreateChartState3(ByVal dtData As DataView)

        If dtData.Count > 0 Then
            If dtData IsNot Nothing Then

                Dim NewdtTable As New DataTable
                NewdtTable = dtData.ToTable

                Dim TitleY As String = "คะแนนเฉลี่ย"
                Dim TitleX As String = "ห้อง"
                Dim SerieNameAVg As String = "คะแนนเฉลี่ย"
                Dim SerieNameTopScore As String = "คะแนนสูงสุด"
                Dim SerieNameLowScore As String = "คะแนนต่ำสุด"
                Dim CategoriesX(NewdtTable.Rows.Count - 1) As Object
                Dim SeriesDataTopScore(NewdtTable.Rows.Count - 1) As Object
                Dim SeriesDataLowScore(NewdtTable.Rows.Count - 1) As Object
                Dim SeriesDataAvgScore(NewdtTable.Rows.Count - 1) As Object

                For a = 0 To NewdtTable.Rows.Count - 1
                    Dim EachClassRommName As String = NewdtTable.Rows(a)("ห้อง")
                    CategoriesX(a) = EachClassRommName
                    Dim EachTopscore As Double = NewdtTable.Rows(a)("คะแนนสูงสุด")
                    SeriesDataTopScore(a) = EachTopscore
                    Dim EachLowScore As Double = NewdtTable.Rows(a)("คะแนนต่ำสุด")
                    SeriesDataLowScore(a) = EachLowScore
                    Dim EachAvgScore As Double = NewdtTable.Rows(a)("คะแนนเฉลี่ย")
                    SeriesDataAvgScore(a) = EachAvgScore
                Next

                Dim StrToolTip As String = "'<b>' + this.series.name + '</b>' + '<br />'  + 'ห้อง ' + '<b>' + this.x + '</b>' + '<br />' + '<b>' + this.y + '</b>' + ' คะแนน'"

                CreateChart(TitleY, TitleX, CategoriesX, StrToolTip, SeriesDataTopScore, SerieNameTopScore, SeriesDataLowScore, SerieNameLowScore, SeriesDataAvgScore, SerieNameAVg)
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการหา Header เพื่อนำมา set
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetLabelTitle()
        Dim dt As New DataTable
        If Mode = "1" Then
            'หา ชั้น + ห้อง , ชื่อชุดข้อสอบ
            Dim sql1 As String = " select q.t360_ClassName,q.t360_RoomName,ts.TestSet_Name " & _
                            " from tblQuiz q,tblTestSet ts where q.TestSet_Id = ts.TestSet_Id and q.Quiz_Id = '" & QueryStringQuizId & "' "
            dt = useCls.getdata(sql1)
            If dt.Rows.Count > 0 Then
                Dim ClassAndRoom As String = dt.Rows(0)("t360_ClassName") & dt.Rows(0)("t360_RoomName")
                lblClassAndRoom.Text = "จาก " & ClassAndRoom
                'Dim TestSetName As String = dt.Rows(0)("TestSet_Name")
                FullTestsetName = dt.Rows(0)("TestSet_Name")
                lblTestSetName.Text = FullTestsetName
            End If
        Else
            If Mode = "2" Then
                'หาชื่อการบ้าน
                Dim sql As String = " select tbltestset.testset_Name from tblquiz inner join tbltestset on tblquiz.testset_id = tbltestset.testset_Id " & _
                                    " where tblquiz.quiz_id = '" & QueryStringQuizId & "' "
                'Dim TestsetName As String = useCls.ExecuteScalar(sql)
                FullTestsetName = useCls.ExecuteScalar(sql)
                lblClassAndRoom.Visible = False
                lblTestSetName.Text = FullTestsetName
            Else
                'หาชื่อชุดข้อสอบที่ฝึกฝน
                Dim sql As String = " select Testset_Name from tbltestset where testset_id = '" & QueryStringTestsetId & "' "
                'Dim TestsetName As String = useCls.ExecuteScalar(sql)
                FullTestsetName = useCls.ExecuteScalar(sql)
                lblClassAndRoom.Visible = False
                lblTestSetName.Text = FullTestsetName
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการ check ว่าจะต้องแสดง chart หรือตารางหลังจากที่ PostBack มา
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckChartOrTable()
        'ตัวแปร CheckType เป็นตัวแปร HiddenField ที่ทำการเก็บ State ที่เลือกไว้ว่าเป็น Chart หรือ ตาราง ทางฝั่ง Javascript
        If CheckType.Value = 1 Then
            DivReportTable.Style.Add("display", "none")
            DivReport.Style.Add("display", "block")
        ElseIf CheckType.Value = 2 Then
            DivReportTable.Style.Add("display", "block")
            DivReport.Style.Add("display", "none")
        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้างกราฟเส้น เกณฑ์คะแนนที่ผ่านโดยนำค่ามาจาก txtbox ที่ User กรอกเข้ามา
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFilter.Click

        Try
            If IsNumeric(txtFilter.Text) Then
                CurrentState.Value = 1
                MainState = 1
                Dim sql As String = "select fullscore from tblquiz where Quiz_Id = '" & QueryStringQuizId & "' "
                Dim FullScore As String = useCls.ExecuteScalar(sql)

                If txtFilter.Text = "0" Or CDbl(txtFilter.Text) < 0.01 Then
                    'ไม่ให้กรอกเป็น 0
                    txtFilter.Text = "0.01"
                    FilterScore = CDbl(0.01)
                ElseIf CDbl(txtFilter.Text) > CDbl(FullScore) Then
                    txtFilter.Text = FullScore
                    FilterScore = CDbl(FullScore)

                ElseIf txtFilter.Text.Substring(0, 1) = "." Then
                    txtFilter.Text = "0" & txtFilter.Text
                    FilterScore = txtFilter.Text
                Else
                    FilterScore = txtFilter.Text
                End If

            Else
                'ไม่ให้กรอกเป็นค่าว่าง ถ้ากรอกว่าง ให้เป็น 0 และไม่ต้องแสดงเส้นที่กราฟ ไม่ต้องป้ายสีที่ตาราง (case พิมพ์ . ตัวเดียวด้วย)
                FilterScore = 0
                txtFilter.Text = ""
            End If
            BindDataOnloadState(1, True)

        Catch ex As Exception
            FilterScore = 0.01
        End Try


    End Sub

    ''' <summary>
    ''' ทำการป้ายสี td ของตารางเมื่อกำลัง Bind Grid ถ้าคะแนนผ่านเกณฑ์ให้ทำการป้ายสีเขียว , ไม่ผ่านป้ายสีแดง
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GvPointFirstActivity_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles GvPointFirstActivity.ItemDataBound

        If Mode <> "1" Then
            If TypeOf (e.Item) Is GridHeaderItem Then
                Dim Header As GridHeaderItem = e.Item
                Header("เลขที่").Text = "เลขที่-ห้อง"
            End If
        End If
        If (TypeOf (e.Item) Is GridDataItem) Then
            If FilterScore = 0 Then
                Dim dataBoundItem As GridDataItem = e.Item
                If (OddRow = True) Then
                    dataBoundItem("เลขที่").Style.Add("padding", "11px")
                    dataBoundItem.CssClass = "ForOddRow"
                    OddRow = False
                Else
                    dataBoundItem("เลขที่").Style.Add("padding", "11px")
                    dataBoundItem.CssClass = "ForEvenRow"
                    OddRow = True
                End If
            Else

                Dim dataBoundItem As GridDataItem = e.Item
                If (FilterScore > Double.Parse(dataBoundItem("คะแนน").Text)) Then
                    dataBoundItem("เลขที่").Style.Add("padding", "11px")
                    dataBoundItem.CssClass = "ForLessThanFilter"
                Else
                    dataBoundItem("เลขที่").Style.Add("padding", "11px")
                    dataBoundItem.CssClass = "ForMoreThanFilter"
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการสร้าง dt เพื่อนำ dt นี้ไปสร้างเป็น File Excel อีกทีนึง
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnExportExcel_Click(sender As Object, e As ImageClickEventArgs) Handles BtnExportExcel.Click
        Dim dt As DataTable = CreateDatatableState1(1, True)
        If dt.Rows.Count > 0 Then
            Dim StudentClassRoom As String = ""
            If Mode = 1 Then
                dt.Columns("Student_CurrentNoInRoom").ColumnName = "เลขที่"
                dt.Columns("totalscore").ColumnName = "คะแนน"
                dt.Columns("Student_FirstName").ColumnName = "ชื่อ"
                dt.Columns("Student_LastName").ColumnName = "สกุล"
                dt.Columns("IndexStudent").ColumnName = "ลำดับ"
                dt.Columns("Student_Code").ColumnName = "รหัสนักเรียน"
                If dt.Columns.Contains("StudentClassRomm") = True AndAlso dt.Rows(0)("StudentClassRomm") IsNot DBNull.Value Then
                    StudentClassRoom = dt.Rows(0)("StudentClassRomm").ToString()
                    dt.Columns.Remove("StudentClassRomm")
                End If
            Else
                dt.Columns("Student_CurrentNoInRoom").ColumnName = "เลขที่"
                dt.Columns("totalscore").ColumnName = "คะแนน"
                dt.Columns("StudentName").ColumnName = "ชื่อ-สกุล"
                dt.Columns("IndexStudent").ColumnName = "ลำดับ"
            End If
            ExportExcel(dt, StudentClassRoom)
        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้างไฟล์ Excel โดยใช้เทคนิคการนำ dt ข้อมูลมา Bind ใน Grid และทำการ Render ออกมา
    ''' </summary>
    ''' <param name="dt">dt ข้อมูลที่จะนำมาออกเป็น File Excel</param>
    ''' <param name="StudentClassRoom">ห้องที่ทำข้อสอบนี้ ในกรณีที่มีห้องส่งมาจะนำมาเติมเข้าไปใน Excel</param>
    ''' <remarks></remarks>
    Private Sub ExportExcel(ByVal dt As DataTable, Optional ByVal StudentClassRoom As String = "")
        'Dim FileName As String = "sample.xls"
        Dim FileName As String = FullTestsetName.Replace("-", "_").Replace(" ", "_") & "_" & DateTime.Now().ToString("ddMMyy")
        If StudentClassRoom <> "" Then
            FileName &= "_" & StudentClassRoom.Replace(".", "")
        End If
        FileName &= ".xls"

        Dim dgGrid As New DataGrid
        dgGrid.DataSource = dt
        dgGrid.DataBind()
        dgGrid.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        dgGrid.ItemStyle.HorizontalAlign = HorizontalAlign.Center
        dgGrid.HeaderStyle.VerticalAlign = WebControls.VerticalAlign.Middle
        dgGrid.ItemStyle.VerticalAlign = WebControls.VerticalAlign.Middle
        dgGrid.HeaderStyle.Height = "21"
        dgGrid.ItemStyle.Height = "22"

        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment;filename=" & FileName)
        Response.ContentEncoding = System.Text.Encoding.Unicode
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        Response.ContentType = "application/ms-excel"
        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        dgGrid.RenderControl(hw)
        Dim HeaderTable As String = ""
        HeaderTable = "<table style='border:1px solid;vertical-align: middle;'>"
        HeaderTable &= "<tr><td style='font-weight:bold;font-size:17px;text-align:center;border:1px solid;height:44px;vertical-align: middle;' colspan='6'>ชุด : " & FullTestsetName & "</td></tr>"
        If StudentClassRoom <> "" Then
            HeaderTable &= "<tr><td style='font-weight:bold;font-size:15px;border:1px solid;text-align:center;height:33px;vertical-align: middle;'>ห้อง : " & StudentClassRoom & "</td><td style='font-weight:bold;font-size:15px;border:1px solid;text-align:center;vertical-align: middle;' colspan='5'>วันที่ : " & DateTime.Now.ToString("dd-MM-yy")
        Else
            HeaderTable &= "<tr><td style='font-weight:bold;font-size:15px;border:1px solid;text-align:center;height:21px;vertical-align: middle;'>วันที่ : " & DateTime.Now.ToString("dd-MM-yy")
        End If
        HeaderTable &= " เวลา : " & DateTime.Now.ToString("HH:mm") & "</td></tr></table>"
        Response.Write(HeaderTable)
        Response.Write(tw.ToString())
        Response.End()
    End Sub
End Class
