Imports KnowledgeUtils

Public Class ChartStudentInfo
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            RadTabStrip1.SelectedIndex = 0
            RadMultiPage1.SelectedIndex = 0
            Dim QuizId As String = Request.QueryString("QuizId").ToString()
            Dim StudentId As String = Request.QueryString("StudentId").ToString()
            Dim Mode As String = Request.QueryString("Mode").ToString()
            Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Score")
            If dt.Rows.Count > 0 Then
                CreateChartByDt(dt, "Score", StudentId, QuizId)
            End If
        End If

        'Dim Series1 As New Telerik.Charting.ChartSeries
        'Series1.Type = Telerik.Charting.ChartSeriesType.Line
        'Series1.Name = "สมชาย"
        'For index = 0 To 5
        '    Dim ChartItem As New Telerik.Charting.ChartSeriesItem
        '    ChartItem.YValue = index
        '    Series1.Items.Add(ChartItem)
        'Next

        'RadChart1.ChartTitle.TextBlock.Text = "Title เทสภาษาไทย"
        'RadChart1.AddChartSeries(Series1)

    End Sub

    Private Sub btnScore_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnScore.Click
        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        Dim StudentId As String = Request.QueryString("StudentId").ToString()
        Dim Mode As String = Request.QueryString("Mode").ToString()
        Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Score")
        If dt.Rows.Count > 0 Then
            CreateChartByDt(dt, "Score", StudentId, QuizId)
        End If
    End Sub

    Private Sub BtnTime_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnTime.Click
        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        Dim StudentId As String = Request.QueryString("StudentId").ToString()
        Dim Mode As String = Request.QueryString("Mode").ToString()
        Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Time")
        If dt.Rows.Count > 0 Then
            CreateChartByDt(dt, "Time", StudentId, QuizId)
        End If
    End Sub

    Private Sub BtnFrequency_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFrequency.Click
        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        Dim StudentId As String = Request.QueryString("StudentId").ToString()
        Dim Mode As String = Request.QueryString("Mode").ToString()
        Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Frequency")
        If dt.Rows.Count > 0 Then
            CreateChartByDt(dt, "Frequency", StudentId, QuizId)
        End If
    End Sub


    Private Sub RadTabStrip1_TabClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTabStripEventArgs) Handles RadTabStrip1.TabClick
        If e.Tab.Text = "วิเคราะห์ข้อมูลนักเรียนของคำถามชุดนี้" Then
            'ให้กลับไปเลือก Pageview 1 
            Dim QuizId As String = Request.QueryString("QuizId").ToString()
            Dim StudentId As String = Request.QueryString("StudentId").ToString()
            Dim Mode As String = Request.QueryString("Mode").ToString()
            Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Score")
            If dt.Rows.Count > 0 Then
                CreateChartByDt(dt, "Score", StudentId, QuizId)
            End If
        Else
            'ให้เลือก Pageview 2 
            Dim QuizId As String = Request.QueryString("QuizId").ToString()
            Dim StudentId As String = Request.QueryString("StudentId").ToString()
            Dim dt As DataTable = GetDtDifficult(QuizId, StudentId)
            If dt.Rows.Count > 0 Then
                CreateChartQuestionDifficult(dt)
            End If
        End If
    End Sub

Private Sub bt1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bt1.Click
 'ให้เลือก Pageview 2 
        ' Dim QuizId As String = Request.QueryString("QuizId").ToString()
        'Dim StudentId As String = Request.QueryString("StudentId").ToString()
        'Dim dt As DataTable = GetDtDifficult(QuizId, StudentId)
        ' If dt.Rows.Count > 0 Then
        '      CreateChartQuestionDifficult(dt)
        ' End If
        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        Dim StudentId As String = Request.QueryString("StudentId").ToString()
        Dim Mode As String = Request.QueryString("Mode").ToString()
        Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Score")
        If dt.Rows.Count > 0 Then
            CreateChartByDt(dt, "Score", StudentId, QuizId)
        End If
        RadMultiPage1.SelectedIndex = 0
        RadTabStrip1.SelectedIndex = 0
    End Sub

    Private Sub bt2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bt2.Click
 'ให้กลับไปเลือก Pageview 1 
        'Dim QuizId As String = Request.QueryString("QuizId").ToString()
        ' Dim StudentId As String = Request.QueryString("StudentId").ToString()
        'Dim Mode As String = Request.QueryString("Mode").ToString()
        'Dim dt As DataTable = GetDtByModeChart(Mode, QuizId, StudentId, "Score")
        'If dt.Rows.Count > 0 Then
        '   CreateChartByDt(dt, "Score", StudentId, QuizId)
        'End If
        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        Dim StudentId As String = Request.QueryString("StudentId").ToString()
        Dim dt As DataTable = GetDtDifficult(QuizId, StudentId)
        If dt.Rows.Count > 0 Then
            CreateChartQuestionDifficult(dt)
        End If
        RadMultiPage1.SelectedIndex = 1
        RadTabStrip1.SelectedIndex = 1
    End Sub



#Region "Function & query"

    Private Sub CreateChartByDt(ByVal dt As DataTable, ByVal SelectedMode As String, ByVal StudentId As String, ByVal QuizId As String)

        If dt.Rows.Count > 0 Then
            RadChart1.Clear()
            Dim Title As String = ""
            Dim TestSetName As String = GetTestSetNameByQuizId(QuizId)
            Dim Score As String = GetScoreByQuizId(QuizId, StudentId)
            'Dim TestSetName As String = "การบ้าน 1"
            'Dim Score As String = "20/50"
            Label1.Text = TestSetName & " ได้ " & Score.ToString().ToPointplusScore()
            If SelectedMode = "Score" Then
                Title = "เปรียบเทียบคะแนน"
            ElseIf SelectedMode = "Time" Then
                Title = "เปรียบเทียบเวลาทำ"
            ElseIf SelectedMode = "Frequency" Then
                Title = "เปรียบเทียบการเปลี่ยนคำตอบ"
            End If
            Dim Series1 As New Telerik.Charting.ChartSeries
            Series1.Type = Telerik.Charting.ChartSeriesType.Line
            Dim StudentName As String = GetStudentNameByStudentId(StudentId)
            Series1.Name = StudentName
            Dim Series2 As New Telerik.Charting.ChartSeries
            Series2.Type = Telerik.Charting.ChartSeriesType.Line
            Series2.Name = "ห้อง"
            Dim Series3 As New Telerik.Charting.ChartSeries
            Series3.Type = Telerik.Charting.ChartSeriesType.Line
            Series3.Name = "ขั้น"

            'Hide Label
            Series1.Appearance.ShowLabels = False
            Series2.Appearance.ShowLabels = False
            Series3.Appearance.ShowLabels = False

            For index = 0 To dt.Rows.Count - 1
                Dim ChartItem1 As New Telerik.Charting.ChartSeriesItem
                Dim ChartItem2 As New Telerik.Charting.ChartSeriesItem
                Dim ChartItem3 As New Telerik.Charting.ChartSeriesItem
                ChartItem1.YValue = dt.Rows(index)("StudentResult")
                ChartItem2.YValue = dt.Rows(index)("RoomResult")
                ChartItem3.YValue = dt.Rows(index)("ClassResult")
                Series1.Items.Add(ChartItem1)
                Series2.Items.Add(ChartItem2)
                Series3.Items.Add(ChartItem3)
                'RadChart1.PlotArea.XAxis(index).TextBlock.Text = "ข้อ " & (index + 1)
            Next

            'ถ้าข้อเยอะมากๆให้มี label ขึ้นเฉพาะบางอัน
            If dt.Rows.Count > 60 Then
                RadChart1.PlotArea.XAxis.LabelStep = 10
            ElseIf dt.Rows.Count > 40 Then
                RadChart1.PlotArea.XAxis.LabelStep = 5
            Else
                RadChart1.PlotArea.XAxis.LabelStep = 1
            End If

            RadChart1.ChartTitle.TextBlock.Text = Title
            RadChart1.AddChartSeries(Series1)
            RadChart1.AddChartSeries(Series2)
            RadChart1.AddChartSeries(Series3)
        End If

    End Sub

    Private Sub CreateChartQuestionDifficult(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            RadChart2.Clear()
            RadChart2.PlotArea.YAxis.AutoScale = False
            RadChart2.PlotArea.YAxis.AddRange(1, 5, 1)
            RadChart2.PlotArea.YAxis(0).TextBlock.Text = "ง่ายมาก"
            RadChart2.PlotArea.YAxis(1).TextBlock.Text = "ง่าย"
            RadChart2.PlotArea.YAxis(2).TextBlock.Text = "ปานกลาง"
            RadChart2.PlotArea.YAxis(3).TextBlock.Text = "ยาก"
            RadChart2.PlotArea.YAxis(4).TextBlock.Text = "ยากมาก"
            Label2.Text = "วิเคราะห์จากทุกคนที่ทำชุดนี้"

            RadChart2.ChartTitle.TextBlock.Text = "เปรียบเทียบความยาก - ง่าย ของคำถามชุดนี้"
            Dim SeriesQuestion As New Telerik.Charting.ChartSeries
            SeriesQuestion.Type = Telerik.Charting.ChartSeriesType.Bar
            Dim SeriesStandard As New Telerik.Charting.ChartSeries
            SeriesStandard.Appearance.ShowLabels = False

            'Hide Label
            SeriesQuestion.Appearance.LabelAppearance.Visible = False

            SeriesStandard.Type = Telerik.Charting.ChartSeriesType.Line
            SeriesQuestion.Name = "ความยากง่าย"
            SeriesStandard.Name = "มาตรฐาน"
            For index = 0 To dt.Rows.Count - 1
                Dim ChartStandard As New Telerik.Charting.ChartSeriesItem
                Dim ChartQuestion As New Telerik.Charting.ChartSeriesItem
                ChartStandard.YValue = 3
                ChartQuestion.YValue = dt.Rows(index)("Difficult")
                SeriesStandard.Items.Add(ChartStandard)
                SeriesQuestion.Items.Add(ChartQuestion)
            Next

            'ถ้ามีข้อเยอะๆให้ Label ขึ้นเฉพาะบางอัน
            If dt.Rows.Count > 50 Then
                RadChart2.PlotArea.XAxis.LabelStep = 10
            ElseIf dt.Rows.Count > 40 Then
                RadChart2.PlotArea.XAxis.LabelStep = 5
            ElseIf dt.Rows.Count > 30 Then
                RadChart2.PlotArea.XAxis.LabelStep = 3
            ElseIf dt.Rows.Count > 20 Then
                RadChart2.PlotArea.XAxis.LabelStep = 2
            End If

            SeriesStandard.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.Nothing
            RadChart2.AddChartSeries(SeriesStandard)
            RadChart2.AddChartSeries(SeriesQuestion)
        End If

    End Sub

    Private Function GetDtByModeChart(ByVal ModeChart As String, ByVal QuizId As String, ByVal StudentId As String, ByVal SelectedMode As String) As DataTable

        Dim dt As New DataTable
        Dim sql As String = ""

        If ModeChart = "Quiz" Then
            If SelectedMode = "Score" Then
                dt = GetDtScoreChart("IsQuizMode", QuizId, StudentId)
            ElseIf SelectedMode = "Time" Then
                dt = GetDtTimeChart(StudentId, "IsQuizMode", QuizId)
            ElseIf SelectedMode = "Frequency" Then
                dt = GetDtFrequenctChart(StudentId, "IsQuizMode", QuizId)
            End If
        ElseIf ModeChart = "Practice" Then
            If SelectedMode = "Score" Then
                dt = GetDtScoreChart("IsPracticeMode", QuizId, StudentId)
            ElseIf SelectedMode = "Time" Then
                dt = GetDtTimeChart(StudentId, "IsPracticeMode", QuizId)
            ElseIf SelectedMode = "Frequency" Then
                dt = GetDtFrequenctChart(StudentId, "IsPracticeMode", QuizId)
            End If
        ElseIf ModeChart = "Homework" Then
            If SelectedMode = "Score" Then
                dt = GetDtScoreChart("IsHomeWorkMode", QuizId, StudentId)
            ElseIf SelectedMode = "Time" Then
                dt = GetDtTimeChart(StudentId, "IsHomeWorkMode", QuizId)
            ElseIf SelectedMode = "Frequency" Then
                dt = GetDtFrequenctChart(StudentId, "IsHomeWorkMode", QuizId)
            End If
        End If

        'sql = " select top 50  question_id,ROW_NUMBER() over (order by question_id) as StudentResult,ABS(CHECKSUM(NewId())) % 14 as RoomResult,ABS(CHECKSUM(NewId())) % 14 as ClassResult " & _
        '      " from uvw_forchartstudent "

        'If sql <> "" Then
        '    dt = _DB.getdata(sql)
        'End If

        Return dt
    End Function

    Private Function GetDtDifficult(ByVal QuizId As String, ByVal StudentId As String) As DataTable

        Dim dt As New DataTable
        'Dim sql As String = " select top 10 *, 1 + CRYPT_GEN_RANDOM(1) % 5 as Difficult from uvw_QuestionDifficult "
        Dim sql As String = " SELECT  tblQuizScore.Question_Id, " & _
                            " CASE WHEN ROUND(CAST(SUM(tblQuizScore.Score) AS FLOAT) / COUNT(*), 2) BETWEEN 0 AND 0.20 THEN 5 " & _
                            " WHEN ROUND(CAST(SUM(tblQuizScore.Score) AS FLOAT) / COUNT(*), 2) BETWEEN 0.21 AND 0.40 THEN 4 " & _
                            " WHEN ROUND(CAST(SUM(tblQuizScore.Score) AS FLOAT) / COUNT(*), 2) BETWEEN 0.41 AND 0.60 THEN 3 " & _
                            " WHEN ROUND(CAST(SUM(tblQuizScore.Score) AS FLOAT) / COUNT(*), 2) BETWEEN 0.61 AND 0.80 THEN 2 " & _
                            " WHEN ROUND(CAST(SUM(tblQuizScore.Score) AS FLOAT) / COUNT(*), 2) BETWEEN 0.81 AND 1 THEN 1 " & _
                            " END AS Difficult " & _
                            " FROM tblTestSetQuestionSet INNER JOIN tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id " & _
                            " INNER JOIN tblQuizScore ON tblTestSetQuestionDetail.Question_Id = tblQuizScore.Question_Id INNER JOIN " & _
                            " tblQuiz ON tblTestSetQuestionSet.TestSet_Id = tblQuiz.TestSet_Id " & _
                            " WHERE (tblQuizScore.IsActive = 1) AND (tblQuiz.Quiz_Id = '" & QuizId & "') AND tblTestsetQuestionSet.IsActive = '1' AND tblTestsetQuestionDetail.IsActive = '1' " & _
                            " GROUP BY tblQuizScore.Question_Id, tblQuiz.Quiz_Id,dbo.tblQuizScore.QQ_No ORDER BY dbo.tblQuizScore.QQ_No "
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetStudentNameByStudentId(ByVal StudentId As String) As String

        Dim sql As String = " select Student_FirstName from t360_tblStudent where Student_Id = '" & StudentId & "' and Student_IsActive = 1 "
        Dim StudentName As String = _DB.ExecuteScalar(sql)
        Return StudentName

    End Function

    Private Function GetTestSetNameByQuizId(ByVal QuizId As String) As String

        Dim sql As String = " select tblTestSet.TestSet_Name from tblQuiz inner join tblTestSet on tblQuiz.TestSet_Id = tblTestSet.TestSet_Id " & _
                            " where tblQuiz.Quiz_Id = '" & QuizId & "' "
        Dim TestSetName As String = _DB.ExecuteScalar(sql)
        Return TestSetName

    End Function

    Private Function GetScoreByQuizId(ByVal QuizId As String, ByVal StudentId As String) As String

        Dim sql As String = " select CAST(tblQuizSession.TotalScore as varchar(max)) + '/' + CAST(tblQuiz.FullScore as varchar(max))  " & _
                            " from tblQuiz inner join tblQuizSession on tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id " & _
                            " where tblQuiz.Quiz_Id = '" & QuizId & "' and dbo.tblQuizSession.Player_Id = '" & StudentId & "' "
        Dim Score As String = _DB.ExecuteScalar(sql)
        Return Score

    End Function

    'Function สร้าง Dt สำหรับ กราฟโหมด วิเคราะห์คะแนน 
    Private Function GetDtScoreChart(ByVal WhereMode As String, ByVal QuizId As String, ByVal StudentId As String) As DataTable

        Dim Knsession As New KNAppSession()
        Dim TestsetId As String = GetTestSetIdByQuizId(QuizId)
        Dim CalendarId As String = Knsession("SelectedCalendarId").ToString()
        Dim dtComplete As New DataTable

        If TestsetId <> "" Then

            Dim sql As String = " select tblQuizScore.Question_Id,ROUND(SUM(tblQuizScore.Score)/ cast (( case when COUNT(*) = 0 then 1 else COUNT(*)end) as decimal(10,2)),2) as TotalResult " & _
                                " from tblQuizScore inner join tblQuiz on tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id  " & _
                                " where tblQuiz.TestSet_Id = '" & TestsetId & "' and tblQuiz." & WhereMode & " = 1 and Student_Id = '" & StudentId & "' AND tblQuiz.Calendar_Id = '" & CalendarId & "' " & _
                                " group by tblQuizScore.Question_Id ,tblQuizScore.QQ_No order by tblQuizScore.QQ_No "
            Dim dtStudent As New DataTable
            dtStudent = _DB.getdata(sql)

            If dtStudent.Rows.Count > 0 Then
                sql = " SELECT tblQuizScore.Question_Id,ROUND(SUM(tblQuizScore.Score)/ cast (( case when COUNT(*) = 0 then 1 else COUNT(*)end) as decimal(10,2)),2) TotalResult " & _
                      " FROM tblQuizScore INNER JOIN tblQuiz ON tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN t360_tblStudentRoom ON " & _
                      " tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN t360_tblStudent ON t360_tblStudentRoom.School_Code = t360_tblStudent.School_Code " & _
                      " AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass AND t360_tblStudentRoom.Room_Name = t360_tblStudent.Student_CurrentRoom " & _
                      " WHERE (tblQuiz.TestSet_Id = '" & TestsetId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) " & _
                      " and (tblQuiz." & WhereMode & " = 1) AND (t360_tblStudent.Student_Id = '" & StudentId & "') AND tblQuiz.Calendar_Id = '" & CalendarId & "' " & _
                      " GROUP BY tblQuizScore.Question_Id,tblQuizScore.QQ_No order by tblQuizScore.QQ_No "
                Dim dtRoom As New DataTable
                dtRoom = _DB.getdata(sql)
                If dtRoom.Rows.Count > 0 Then
                    sql = " SELECT tblQuizScore.Question_Id, ROUND(SUM(tblQuizScore.Score) / CAST((CASE WHEN COUNT(*) = 0 THEN 1 ELSE COUNT(*) END) " & _
                        " AS decimal(10, 2)), 2) AS TotalResult FROM tblQuizScore INNER JOIN tblQuiz ON tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN " & _
                        " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN  t360_tblStudent ON " & _
                        " t360_tblStudentRoom.School_Code = t360_tblStudent.School_Code AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass " & _
                        " WHERE (tblQuiz.TestSet_Id = '" & TestsetId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) AND (tblQuiz." & WhereMode & " = 1) AND " & _
                        " (t360_tblStudent.Student_Id = '" & StudentId & "') AND tblQuiz.Calendar_Id = '" & CalendarId & "' GROUP BY tblQuizScore.Question_Id, tblQuizScore.QQ_No " & _
                        " ORDER BY tblQuizScore.QQ_No "
                    Dim dtClass As New DataTable
                    dtClass = _DB.getdata(sql)
                    If dtClass.Rows.Count > 0 Then
                        dtComplete = GetDtCompleteForCreateChart(dtComplete, dtStudent, dtRoom, dtClass)
                        Return dtComplete
                    Else
                        Return dtComplete
                    End If
                Else
                    Return dtComplete
                End If
            Else
                Return dtComplete
            End If
        Else
            Return dtComplete
        End If

    End Function

    Private Function GetDtTimeChart(ByVal StudentId As String, ByVal WhereMode As String, ByVal QuizId As String) As DataTable

        Dim Knsession As New KNAppSession()
        Dim TestsetId As String = GetTestSetIdByQuizId(QuizId)
        Dim CalendarId As String = Knsession("SelectedCalendarId").ToString()
        Dim dtComplete As New DataTable

        Dim sql As String = " SELECT tblQuizScore.Question_Id,SUM( CAST(DATEDIFF(SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) " & _
                            " as decimal(10,2)))/( case when COUNT(*) = 0 then 1 else COUNT(*)end) as TotalResult FROM tblQuiz INNER JOIN " & _
                            " tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id WHERE (tblQuiz.TestSet_Id = '" & TestsetId & "') " & _
                            " AND (tblQuizScore.Student_Id = '" & StudentId & "') AND (tblQuiz." & WhereMode & " = 1) " & _
                            " and (DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) between 0 and 10 ) and " & _
                            " tblQuiz.Calendar_Id = '" & CalendarId & "' group by tblQuizScore.Question_Id,QQ_No order by QQ_No "
        Dim dtStudent As New DataTable
        dtStudent = _DB.getdata(sql)
        If dtStudent.Rows.Count > 0 Then
            sql = " SELECT tblQuizScore.Question_Id, SUM(CAST(DATEDIFF(SECOND, tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) AS decimal(10, 2))) / (CASE WHEN COUNT(*) = 0 " & _
                  " THEN 1 ELSE COUNT(*) END) AS TotalResult FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                  " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN t360_tblStudent ON t360_tblStudentRoom.School_Code " & _
                  " = t360_tblStudent.School_Code AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass AND t360_tblStudentRoom.Room_Name = " & _
                  " t360_tblStudent.Student_CurrentRoom WHERE (tblQuiz.TestSet_Id = '" & TestsetId & "') AND (tblQuiz." & WhereMode & " = 1) " & _
                  " AND (DATEDIFF(MINUTE, tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) BETWEEN 0 AND 10) AND (tblQuiz.Calendar_Id = '" & CalendarId & "') " & _
                  " AND (t360_tblStudent.Student_Id = '" & StudentId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) GROUP BY " & _
                  " tblQuizScore.Question_Id, tblQuizScore.QQ_No ORDER BY tblQuizScore.QQ_No "
            Dim dtRoom As New DataTable
            dtRoom = _DB.getdata(sql)
            If dtRoom.Rows.Count > 0 Then
                sql = " SELECT tblQuizScore.Question_Id, SUM(CAST(DATEDIFF(SECOND, tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) AS decimal(10, 2))) / (CASE WHEN COUNT(*) = 0 " & _
                      " THEN 1 ELSE COUNT(*) END) AS TotalResult FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                      " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN t360_tblStudent ON t360_tblStudentRoom.School_Code " & _
                      " = t360_tblStudent.School_Code AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass WHERE " & _
                      " (tblQuiz.TestSet_Id = '" & TestsetId & "') AND (tblQuiz." & WhereMode & " = 1) AND " & _
                      " (DATEDIFF(MINUTE, tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) BETWEEN 0 AND 10) AND (tblQuiz.Calendar_Id = '" & CalendarId & "') " & _
                      " AND (t360_tblStudent.Student_Id = '" & StudentId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) " & _
                      " GROUP BY tblQuizScore.Question_Id, tblQuizScore.QQ_No ORDER BY tblQuizScore.QQ_No "
                Dim dtClass As New DataTable
                dtClass = _DB.getdata(sql)
                If dtClass.Rows.Count > 0 Then
                    dtComplete = GetDtCompleteForCreateChart(dtComplete, dtStudent, dtRoom, dtClass)
                    Return dtComplete
                Else
                    Return dtComplete
                End If
            Else
                Return dtComplete
            End If
        Else
            Return dtComplete
        End If

    End Function

    Private Function GetDtFrequenctChart(ByVal StudentId As String, ByVal WhereMode As String, ByVal QuizId As String) As DataTable

        Dim Knsession As New KNAppSession()
        Dim TestsetId As String = GetTestSetIdByQuizId(QuizId)
        Dim CalendarId As String = Knsession("SelectedCalendarId").ToString()
        Dim dtComplete As New DataTable

        Dim sql As String = " SELECT tblQuizScore.Question_Id, CAST(SUM(ResponseAmount) as decimal(10,2)) / cast((case when COUNT(*) = 0 then 1 else COUNT(*) end ) " & _
                            " as decimal(10,2)) as TotalResult FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id " & _
                            " WHERE (tblQuizScore.Student_Id = '" & StudentId & "') AND (tblQuiz.TestSet_Id = '" & TestsetId & "') " & _
                            " and tblQuiz." & WhereMode & " = 1 and tblQuiz.Calendar_Id = '" & CalendarId & "' group by tblQuizScore.Question_Id,QQ_No order by QQ_No "
        Dim dtStudent As New DataTable
        dtStudent = _DB.getdata(sql)
        If dtStudent.Rows.Count > 0 Then
            sql = " SELECT tblQuizScore.Question_Id, CAST(SUM(tblQuizScore.ResponseAmount) AS decimal(10, 2)) / CAST((CASE WHEN COUNT(*) = 0 " & _
                  " THEN 1 ELSE COUNT(*) END) AS decimal(10, 2)) AS TotalResult FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                  " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN  t360_tblStudent ON " & _
                  " t360_tblStudentRoom.School_Code = t360_tblStudent.School_Code AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass AND " & _
                  " t360_tblStudentRoom.Room_Name = t360_tblStudent.Student_CurrentRoom WHERE (tblQuiz.TestSet_Id = '" & TestsetId & "') AND " & _
                  " (tblQuiz." & WhereMode & " = 1) AND (tblQuiz.Calendar_Id = '" & CalendarId & "') AND " & _
                  " (t360_tblStudent.Student_Id = '" & StudentId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) " & _
                  " GROUP BY tblQuizScore.Question_Id, tblQuizScore.QQ_No ORDER BY tblQuizScore.QQ_No "
            Dim dtRoom As New DataTable
            dtRoom = _DB.getdata(sql)
            If dtRoom.Rows.Count > 0 Then
                sql = " SELECT tblQuizScore.Question_Id, CAST(SUM(tblQuizScore.ResponseAmount) AS decimal(10, 2)) / CAST((CASE WHEN COUNT(*) = 0 THEN " & _
                      " 1 ELSE COUNT(*) END) AS decimal(10, 2)) AS TotalResult FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                      " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN t360_tblStudent ON t360_tblStudentRoom.School_Code " & _
                      " = t360_tblStudent.School_Code AND t360_tblStudentRoom.Class_Name = t360_tblStudent.Student_CurrentClass WHERE " & _
                      " (tblQuiz.TestSet_Id = '" & TestsetId & "') AND (tblQuiz." & WhereMode & " = 1) AND " & _
                      " (tblQuiz.Calendar_Id = '" & CalendarId & "') AND (t360_tblStudent.Student_Id = '" & StudentId & "') " & _
                      " AND (t360_tblStudentRoom.SR_IsActive = 1) GROUP BY tblQuizScore.Question_Id, tblQuizScore.QQ_No ORDER BY tblQuizScore.QQ_No "
                Dim dtClass As New DataTable
                dtClass = _DB.getdata(sql)
                If dtClass.Rows.Count > 0 Then
                    dtComplete = GetDtCompleteForCreateChart(dtComplete, dtStudent, dtRoom, dtClass)
                    Return dtComplete
                Else
                    Return dtComplete
                End If
            Else
                Return dtComplete
            End If
        Else
            Return dtComplete
        End If

    End Function

    Private Function GetTestSetIdByQuizId(ByVal QuizId As String) As String
        Dim sql As String = " Select Testset_Id from tblquiz where quiz_id = '" & QuizId & "' "
        Dim TestsetId As String = _DB.ExecuteScalar(sql)
        Return TestsetId
    End Function

    Private Function GetDtCompleteForCreateChart(ByVal dtComplete As DataTable, ByVal dtStudent As DataTable, ByVal dtRoom As DataTable, ByVal dtClass As DataTable) As DataTable

        'dtComplete.Columns.Add("Question_Id", GetType(String))
        dtComplete.Columns.Add("StudentResult", GetType(String))
        dtComplete.Columns.Add("RoomResult", GetType(String))
        dtComplete.Columns.Add("ClassResult", GetType(String))

        For a = 0 To dtStudent.Rows.Count - 1
            'dtComplete.Rows.Add(a)("Question_Id") = dtStudent.Rows(a)("Question_Id").ToString()
            Dim QuestionId As String = dtStudent.Rows(a)("Question_Id").ToString()
            dtComplete.Rows.Add(a)("StudentResult") = dtStudent.Rows(a)("TotalResult")
            ' Dim StudentScore As String = dta.Rows(a)("Score")
            Dim rowRoom = dtRoom.Select("Question_Id='" & QuestionId & "'")
            If rowRoom.Count > 0 Then
                dtComplete.Rows(a)("RoomResult") = rowRoom(0)("TotalResult")
            Else
                dtComplete.Rows(a)("RoomResult") = "0"
            End If
            Dim rowClass = dtClass.Select("Question_Id='" & QuestionId & "'")
            If rowClass.Count > 0 Then
                dtComplete.Rows(a)("ClassResult") = rowClass(0)("TotalResult")
            Else
                dtComplete.Rows(a)("ClassResult") = 0
            End If
        Next
        Return dtComplete

    End Function


#End Region





End Class