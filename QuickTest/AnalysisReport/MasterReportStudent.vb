Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing
Imports System.IO
Imports System.Web.UI.DataVisualization.Charting

Partial Public Class MasterReportStudent
    Inherits Telerik.Reporting.Report
    Public Sub New(ByVal dtStudentInfoBottom As DataTable, ByVal ArrStudentPieChart As ArrayList, ByVal DtTotalPracticeBySubject As DataTable, ByVal ArrStudentSendHomework As ArrayList, ByVal IndexPage As String, ByVal DtTotalQuizBySubject As DataTable)

        InitializeComponent()
        CreatePieChartTotalUsage(ArrStudentPieChart)
        FillStudentInFoBottom(dtStudentInfoBottom)
        CreateBarChartTotalPracticeBySubject(DtTotalPracticeBySubject)
        CreateSpiderChartQuizBySubject(DtTotalQuizBySubject)
        CreateBarChartStudentSendHomework(ArrStudentSendHomework)
        txtIndexPage.Value = IndexPage
        'TextBox7.Value = Testaa

    End Sub

    Private Sub CreatePieChartTotalUsage(ByVal ArrData As ArrayList)

        If ArrData.Count > 0 Then

            Dim TotalQuiz As Integer = ArrData(0)
            Dim TotalPractice As Integer = ArrData(1)
            Dim TotalHomework As Integer = ArrData(2)

            Dim ChartSeries As New Telerik.Reporting.Charting.ChartSeries
            ChartSeries.Type = Telerik.Charting.ChartSeriesType.Pie
            ChartSeries.Name = "ปริมาณการใช้งาน"

            Dim ItemQuiz As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemQuiz.YValue = TotalQuiz
            ChartSeries.Items.Add(ItemQuiz)

            Dim ItemPractice As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemPractice.YValue = TotalPractice
            ChartSeries.Items.Add(ItemPractice)

            Dim ItemHomework As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemHomework.YValue = TotalHomework
            ChartSeries.Items.Add(ItemHomework)

            ChartTotalUsage.Series.Add(ChartSeries)
            ChecktxtTotalUsage(TotalQuiz, TotalPractice, TotalHomework)

        End If

    End Sub

    Private Sub CreateSpiderChartQuizBySubject(ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then

            Dim dtComplete As New DataTable
            dtComplete.Columns.Add("Criteria")
            dtComplete.Columns.Add("คะแนน")

            For index = 0 To dt.Columns.Count - 1
                dtComplete.Rows.Add(index)("Criteria") = dt.Columns(index).ColumnName.ToString()
                dtComplete.Rows(index)("คะแนน") = dt.Rows(0)(index)
            Next

            Dim aRadarChart As New DataVisualization.Charting.Chart()
            aRadarChart.Width = WebControls.Unit.Pixel(500)
            aRadarChart.DataSource = dtComplete
            ' Add title 
            aRadarChart.Titles.Add("คะแนนเฉลี่ยที่ได้")
            Dim ca As New ChartArea("myChartArea")
            aRadarChart.ChartAreas.Add(ca)

            Dim subj As String = [String].Empty
            For i As Integer = 1 To dtComplete.Columns.Count - 1
                subj = dtComplete.Columns(i).ColumnName
                aRadarChart.Series.Add(subj)
                aRadarChart.Series(subj).YValueMembers = subj
                aRadarChart.Series(subj).XValueMember = dtComplete.Columns(0).ColumnName
                aRadarChart.Series(subj).ChartType = SeriesChartType.Radar
            Next

            'for saving image 
            Dim Filname As String = GetRndNumber()
            Do Until File.Exists(HttpContext.Current.Server.MapPath("~/AnalysisReport/SpiderPic/" & Filname & ".jpg")) = False
                Filname = GetRndNumber()
            Loop
            aRadarChart.SaveImage(HttpContext.Current.Server.MapPath("~/AnalysisReport/SpiderPic/" & Filname & ".jpg"))
            PictureQuizChart.Value = "~/AnalysisReport/SpiderPic/" & Filname & ".jpg"

        End If
    End Sub

    Private Sub CreateBarChartTotalPracticeBySubject(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then

            ChartTotalPracticeBySubject.PlotArea.XAxis.AutoScale = False
            ChartTotalPracticeBySubject.PlotArea.XAxis.AddRange(0, 7, 1)
            ChartTotalPracticeBySubject.PlotArea.XAxis(0).TextBlock.Text = "ไทย"
            ChartTotalPracticeBySubject.PlotArea.XAxis(1).TextBlock.Text = "อังกฤษ"
            ChartTotalPracticeBySubject.PlotArea.XAxis(2).TextBlock.Text = "สังคม"
            ChartTotalPracticeBySubject.PlotArea.XAxis(3).TextBlock.Text = "สุขศึกษา"
            ChartTotalPracticeBySubject.PlotArea.XAxis(4).TextBlock.Text = "วิทย์"
            ChartTotalPracticeBySubject.PlotArea.XAxis(5).TextBlock.Text = "ศิลปะ"
            ChartTotalPracticeBySubject.PlotArea.XAxis(6).TextBlock.Text = "คณิตฯ"
            ChartTotalPracticeBySubject.PlotArea.XAxis(7).TextBlock.Text = "การงานฯ"

            Dim ChartSeries As New Telerik.Reporting.Charting.ChartSeries
            ChartSeries.Type = Telerik.Charting.ChartSeriesType.Bar
            ChartSeries.Name = "ปริมาณการฝึกฝนตัวเอง"

            Dim ItemThai As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemThai.YValue = dt.Rows(0)("Thai")
            'ItemThai.Label.TextBlock.Text = "ไทย"
            ChartSeries.Items.Add(ItemThai)

            Dim ItemEnglish As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemEnglish.YValue = dt.Rows(0)("English")
            'ItemEnglish.Label.TextBlock.Text = "อังกฤษ"
            ChartSeries.Items.Add(ItemEnglish)

            Dim ItemSocial As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemSocial.YValue = dt.Rows(0)("Social")
            'ItemSocial.Label.TextBlock.Text = "สังคม"
            ChartSeries.Items.Add(ItemSocial)

            Dim ItemHealth As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemHealth.YValue = dt.Rows(0)("Health")
            'ItemHealth.Label.TextBlock.Text = "สุขศึกษา"
            ChartSeries.Items.Add(ItemHealth)

            Dim ItemScience As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemScience.YValue = dt.Rows(0)("Science")
            'ItemScience.Label.TextBlock.Text = "วิทย์"
            ChartSeries.Items.Add(ItemScience)

            Dim ItemArt As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemArt.YValue = dt.Rows(0)("Art")
            'ItemArt.Label.TextBlock.Text = "ศิลปะ"
            ChartSeries.Items.Add(ItemArt)

            Dim ItemMath As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemMath.YValue = dt.Rows(0)("Math")
            'ItemMath.Label.TextBlock.Text = "คณิตฯ"
            ChartSeries.Items.Add(ItemMath)

            Dim ItemCareers As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemCareers.YValue = dt.Rows(0)("Careers")
            'ItemCareers.Label.TextBlock.Text = "การงานฯ"
            ChartSeries.Items.Add(ItemCareers)

            ChartTotalPracticeBySubject.Series.Add(ChartSeries)

        End If

    End Sub

    Private Sub CreateBarChartStudentSendHomework(ByVal ArrData As ArrayList)

        If ArrData.Count > 0 Then

            ChartSendHomework.PlotArea.XAxis.AutoScale = False
            ChartSendHomework.PlotArea.XAxis.AddRange(0, 2, 1)
            ChartSendHomework.PlotArea.XAxis(0).TextBlock.Text = "ส่งครบทัน"
            ChartSendHomework.PlotArea.XAxis(1).TextBlock.Text = "ทำไม่ครบ"
            ChartSendHomework.PlotArea.XAxis(2).TextBlock.Text = "ไม่ส่ง"

            Dim BeforeEndTime As Integer = ArrData(0)
            Dim InEndTime As Integer = ArrData(1)
            Dim NotSend As Integer = ArrData(2)

            Dim ChartSeries As New Telerik.Reporting.Charting.ChartSeries
            ChartSeries.Type = Telerik.Charting.ChartSeriesType.Bar
            ChartSeries.Name = "การส่งการบ้าน"

            Dim ItemBeforeEndTime As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemBeforeEndTime.YValue = BeforeEndTime
            ChartSeries.Items.Add(ItemBeforeEndTime)

            Dim ItemInEndTime As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemInEndTime.YValue = InEndTime
            ChartSeries.Items.Add(ItemInEndTime)

            Dim ItemNotSend As New Telerik.Reporting.Charting.ChartSeriesItem
            ItemNotSend.YValue = NotSend
            ChartSeries.Items.Add(ItemNotSend)

            ChartSendHomework.Series.Add(ChartSeries)

        End If

    End Sub

    Private Sub FillStudentInFoBottom(ByVal dtStudentInfoBottom As DataTable)

        If dtStudentInfoBottom.Rows.Count > 0 Then

            If dtStudentInfoBottom.Rows(0)("StudentName") IsNot DBNull.Value Then
                txtStudentName.Value = dtStudentInfoBottom.Rows(0)("StudentName")
            Else
                txtStudentName.Value = "ไม่พบชื่อ"
            End If

            If dtStudentInfoBottom.Rows(0)("StudentSchoolInfo") IsNot DBNull.Value Then
                txtStudentSchoolInfo.Value = dtStudentInfoBottom.Rows(0)("StudentSchoolInfo")
                TextBox7.Value = dtStudentInfoBottom.Rows(0)("InfoWithoutCode")
            Else
                txtStudentSchoolInfo.Value = "ไม่พบข้อมูล"
            End If

            If dtStudentInfoBottom.Rows(0)("StudentParentInfo") IsNot DBNull.Value Then
                txtStudentParentInfo.Value = "ผู้ปกครอง " & dtStudentInfoBottom.Rows(0)("StudentParentInfo")
            Else
                txtStudentParentInfo.Value = "ไม่พบข้อมูลผู้ปกครอง"
            End If

            Dim PathImage As String = HttpContext.Current.Server.MapPath("../UserData/" & dtStudentInfoBottom.Rows(0)("School_Code").ToString() & "/{" & dtStudentInfoBottom.Rows(0)("Student_Id").ToString() & "}/Id.png")
            Dim CheckImageStudent As New DirectoryInfo(HttpContext.Current.Server.MapPath("../UserData/" & dtStudentInfoBottom.Rows(0)("School_Code").ToString() & "/{" & dtStudentInfoBottom.Rows(0)("Student_Id").ToString() & "}/Id.png"))
            If File.Exists(PathImage) = True Then
                StudentPic.Value = "~/UserData/" & dtStudentInfoBottom.Rows(0)("School_Code").ToString() & "/{" & dtStudentInfoBottom.Rows(0)("Student_Id").ToString() & "}/Id.png"
            Else
                StudentPic.Value = "~/Images/Student.png"
            End If



        End If

    End Sub

    Private Sub ChecktxtTotalUsage(ByVal TotalQuiz As Integer, ByVal TotalPractice As Integer, ByVal TotalHomework As Integer)

        Dim txtExplain As String = ""

        If TotalPractice > TotalHomework And TotalHomework > TotalQuiz Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalPractice > TotalQuiz And TotalQuiz > TotalHomework Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalQuiz > TotalHomework And TotalHomework > TotalPractice Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalQuiz > TotalPractice And TotalPractice > TotalHomework Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalHomework > TotalPractice And TotalPractice > TotalQuiz Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalHomework > TotalQuiz And TotalQuiz > TotalPractice Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        ElseIf TotalPractice = TotalHomework And TotalHomework = TotalQuiz And TotalPractice = TotalQuiz Then
            txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้วยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        End If

    End Sub

    Private Function GetRndNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        For i = 1 To 4
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

End Class