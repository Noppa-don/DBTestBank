Imports BusinessTablet360

Public Class CompareChartControl
    Inherits System.Web.UI.UserControl

    Dim KnSession As New KNAppSession()
    Dim ClsHomework As New Service.ClsHomework(New ClassConnectSql())
    Dim ClsActivity As New Service.ClsActivity(New ClassConnectSql())
    Dim ClsViewreport As New ClassViewReport(New ClassConnectSql())
    Dim _DB As New ClassConnectSql()


    'Public Property SelectedStudentId As String
    '    Get
    '        SelectedStudentId = ViewState("_StudentId")
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("_StudentId") = value
    '    End Set
    'End Property

    'Public Property SelectedTeacherId As String
    '    Get
    '        SelectedTeacherId = ViewState("_TeacherId")
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("_TeacherId") = value
    '    End Set
    'End Property

    'Public Property SelectedCalendarId As String
    '    Get
    '        SelectedCalendarId = ViewState("_CalendarId")
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("_CalendarId") = value
    '    End Set
    'End Property

    'Public Property SelectedQuizOrHomework As String
    '    Get
    '        SelectedQuizOrHomework = ViewState("_SelectedQuizOrHomework")
    '    End Get
    '    Set(ByVal value As String)
    '        ViewState("_SelectedQuizOrHomework") = value
    '    End Set
    'End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub CreateChartStudentInfo(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

        Session("SelectedStudentId") = StudentId
        Session("SelectedCalendarId") = CalendarId
        Session("SelectedTeacherId") = TeacherId

        If CheckIsHaveData(StudentId, CalendarId, TeacherId) = True Then
            If Session("SelectedQuizOrHomework") = "Homework" Or Session("SelectedQuizOrHomework") Is Nothing Then
                ProcessHomework(StudentId, CalendarId, TeacherId)
            ElseIf Session("SelectedQuizOrHomework") = "Quiz" Then
                ProcessQuiz(StudentId, CalendarId, TeacherId)
            End If
        Else
            Dim StringDivNoData As String = GetStringNoData(StudentId, CalendarId)
            CreateDivNoData(StringDivNoData)
        End If

    End Sub

    Public Sub CreateEmpty()

    End Sub

    Private Sub btnHomework_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHomework.Click
        Session("SelectedQuizOrHomework") = "Homework"
        ProcessHomework(Session("SelectedStudentId"), Session("SelectedCalendarId"), Session("SelectedTeacherId"))
    End Sub

    Private Sub btnQuiz_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuiz.Click
        Session("SelectedQuizOrHomework") = "Quiz"
        ProcessQuiz(Session("SelectedStudentId"), Session("SelectedCalendarId"), Session("SelectedTeacherId"))
    End Sub

     
#Region "Function"

    'Function ทำเมื่อกดปุ่ม การบ้าน
    Private Sub ProcessHomework(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

        'เช็คว่า มีการบ้านในเทอมนี้หรือเปล่า
        DivBtnSubject.Controls.Clear()
        If ClsHomework.CheckIsHaveHomework(StudentId, CalendarId, TeacherId) = True Then
            Dim FlagManySubject As Boolean = False
            Dim FlagOneSubject As Boolean = False
            'เช็คว่ามีการบ้านที่เป็นหลายวิชาหรือเปล่า
            If ClsHomework.CheckIsManySubject(StudentId, CalendarId, TeacherId) = True Then
                CreateBtnSubject("", "หลายวิชา")
                FlagManySubject = True
            End If
            Dim dt As DataTable = ClsHomework.GetSubjectNameByStudentId(StudentId, CalendarId, TeacherId)
            'เช็คว่ามี การบ้านที่เป็นวิชาเดียวหรือเปล่า และมีวิชาอะไรบ้าง
            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    CreateBtnSubject(dt.Rows(index)("GroupSubject_Id").ToString(), dt.Rows(index)("GroupSubject_Name"))
                Next
                FlagOneSubject = True
            End If
            If FlagManySubject = True Then
                CreateRadChart(StudentId, CalendarId, "", "Quiz", TeacherId)
            ElseIf FlagOneSubject = True Then
                CreateRadChart(StudentId, CalendarId, dt.Rows(0)("GroupSubject_Id").ToString(), "Homework", TeacherId)
            End If
        Else
            CreateDivSmallNoData("ยังไม่มีคะแนนการบ้านค่ะ")
        End If

    End Sub

    'Function ทำเมื่อกดปุ่ม ควิซ
    Private Sub ProcessQuiz(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

        'เช็คว่า มีควิซในเทอมนี้หรืเอเปล่า
        DivBtnSubject.Controls.Clear()
        If ClsActivity.CheckIsHaveQuiz(StudentId, CalendarId, TeacherId) = True Then
            Dim FlagManySubject As Boolean = False
            Dim FlagOneSubject As Boolean = False
            'เช็คว่ามี Quiz ที่เป็นหลายวิชาหรือเปล่า
            If ClsActivity.CheckIsManySubject(StudentId, CalendarId, TeacherId) = True Then
                CreateBtnSubject("", "หลายวิชา")
                FlagManySubject = True
            End If
            Dim dt As DataTable = ClsActivity.GetSubjectNameByStudentId(StudentId, CalendarId, TeacherId)
            'เช็คว่ามี ควิซที่เป็นวิชาเดียวบ้างหรือเปล่า และมีวิชาอะไรบ้าง
            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    CreateBtnSubject(dt.Rows(index)("GroupSubject_Id").ToString(), dt.Rows(index)("GroupSubject_Name"))
                Next
                FlagOneSubject = True
            End If
            If FlagManySubject = True Then
                CreateRadChart(StudentId, CalendarId, "", "Quiz", TeacherId)
            ElseIf FlagOneSubject = True Then
                CreateRadChart(StudentId, CalendarId, dt.Rows(0)("GroupSubject_Id").ToString(), "Quiz", TeacherId)
            End If
        Else 'ถ้าไม่มีให้สร้าง Div ที่ไม่มีข้อมูล
            CreateDivSmallNoData("ยังไม่เคยทำควิซค่ะ")
        End If

    End Sub

    'Function สร้างปุ่ม
    Private Sub CreateBtnSubject(ByVal GroupsubjectId As String, ByVal GroupsubjectName As String)

        Dim NewBtn As New Button
        NewBtn.CssClass = "WidthSmallBtn Forbtn"
        If GroupsubjectId = "" Then
            NewBtn.ID = "ManySubject"
            NewBtn.Text = GroupsubjectName
        Else
            NewBtn.ID = GroupsubjectId
            NewBtn.Text = ClsViewreport.GenSubjectName(GroupSubjectName)
        End If
        AddHandler NewBtn.Click, AddressOf Me.SubjectClick
        DivBtnSubject.Controls.Add(NewBtn)

    End Sub

    'Function เพื่อเช็คว่า เทอมนี้นักเรียนคนนี้ มีการทำการบ้านหรือควิซบ้างไหม
    Private Function CheckIsHaveData(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "") As Boolean

        Dim CheckHaveData As String = ""
        Dim sql As String = ""
        'ถ้าเป็นโหมดแบบ ข้อมูลทั้งโรงเรียน ไม่ where TeacherId
        If TeacherId = "" Then
            'เช็ค การบ้านก่อนว่ามีเคยสั่งการบ้าน นักเรียนคนนี้ในเทอมนี้หรือเปล่า
            sql = " SELECT COUNT(DISTINCT tblModuleDetailCompletion.Quiz_Id) AS Expr1 FROM tblModule INNER JOIN " & _
                  " tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
                  " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " & _
                  " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id " & _
                  " WHERE (tblModuleDetail.Reference_Type = 0) AND (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " & _
                  " AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "')  "
            CheckHaveData = _DB.ExecuteScalar(sql)
            If CType(CheckHaveData, Integer) > 0 Then
                Return True
            End If
            'เช็ค ว่าเคยสั่งควิซ นักเรียนคนนี้ในเทอมนี้หรือเปล่า ถ้าไม่มีคืน False กลับไปเลยเพราะว่าเช็คครบทั้ง การบ้าน , ควิซ แล้ว
            sql = " SELECT COUNT(DISTINCT tblQuiz.Quiz_Id) AS Expr1 FROM tblQuiz INNER JOIN " & _
                  " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id WHERE (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " & _
                  " AND (tblQuiz.IsQuizMode = 1) AND (tblQuiz.IsActive = 1) AND (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') "
            CheckHaveData = _DB.ExecuteScalar(sql)
            If CType(CheckHaveData, Integer) > 0 Then
                Return True
            Else
                Return False
            End If
        Else 'ถ้าเป็นโหมดแบบ ข้อมูลเฉพาะที่สอน Where TeacherId ด้วย
            'เช็คการบ้านก่อน
            sql = " SELECT COUNT(DISTINCT tblModuleDetailCompletion.Quiz_Id) AS Expr1 FROM tblModule INNER JOIN " & _
                  " tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
                  " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " & _
                  " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN " & _
                  " tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id WHERE (tblModuleDetail.Reference_Type = 0) AND " & _
                  " (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') AND " & _
                  " (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') AND " & _
                  " (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') "
            CheckHaveData = _DB.ExecuteScalar(sql)
            If CType(CheckHaveData, Integer) > 0 Then
                Return True
            End If
            'เช็ค ควิซ
            sql = " SELECT COUNT(DISTINCT tblQuiz.Quiz_Id) AS Expr1 FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id " & _
                  " INNER JOIN tblAssistant ON tblQuiz.User_Id = tblAssistant.Teacher_id WHERE (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " & _
                  " AND (tblQuiz.IsQuizMode = 1) AND (tblQuiz.IsActive = 1) AND (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') " & _
                  " AND (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') "
            CheckHaveData = _DB.ExecuteScalar(sql)
            If CType(CheckHaveData, Integer) > 0 Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    'สร้าง div ที่แสดงเมื่อหาข้อมูลไม่เจอ รับ String เข้ามาเพื่อเอามาแสดงใน Div 
    Private Sub CreateDivNoData(ByVal ShowString As String)

        Dim sb As New StringBuilder
        sb.Append("<div id='DivNoData' class='ForMainDivNoData' >")
        sb.Append("<div id='DivShowInfo' class='ForDivShowInFo'>")
        sb.Append("<span class='ForSpanNoData' >" & ShowString & "</span>")
        sb.Append("</div></div>")
        MainDiv.Controls.Clear()
        MainDiv.InnerHtml = sb.ToString()

    End Sub

    'สร้าง div ไม่มี data อันเล็ก
    Private Sub CreateDivSmallNoData(ByVal InputString As String)

        Dim sb As New StringBuilder
        sb.Append("<div class='ForMainDivNoData'><span class='ForSpanNoData' style='top:90px;'>" & InputString & "</span></div>")
        DivChart.Controls.Clear()
        DivChart.InnerHtml = sb.ToString()

    End Sub

    'หาชื่อนักเรียน และ เทอม เพื่อเอาไปแสดงใน DivNoData
    Private Function GetStringNoData(ByVal StudentId As String, ByVal CalendarId As String)

        Dim sql As String = " SELECT Student_FirstName + ' ' + Student_LastName FROM dbo.t360_tblStudent WHERE Student_Id = '" & _DB.CleanString(StudentId) & "' AND " & _
                            " Student_IsActive = 1 "
        Dim StudentName As String = _DB.ExecuteScalar(sql)
        sql = " SELECT Calendar_Name + '/' + Calendar_Year FROM dbo.t360_tblCalendar WHERE Calendar_Id = '" & _DB.CleanString(CalendarId) & "' "
        Dim CalandarName As String = _DB.ExecuteScalar(sql)
        Dim ReturnString As String = "ใน " & CalandarName & "<br /> ไม่มีข้อมูลของ " & StudentName & " ค่ะ"
        Return ReturnString

    End Function

    'Function เมื่อกดปุ่มเลือกวิชา
    Protected Sub SubjectClick(ByVal sender As Object, ByVal e As EventArgs)

        Dim NewBtn As New Button
        NewBtn = sender
        Dim SubjectId As String = NewBtn.ID.ToString()
        If SubjectId = "ManySubject" Then
            SubjectId = ""
        End If
        'เมื่อกดแล้วต้องไปสร้างกราฟต่อ
        CreateRadChart(Session("SelectedStudentId"), Session("SelectedCalendarId"), SubjectId, Session("SelectedQuizOrHomework"), Session("SelectedTeacherId"))

    End Sub

    'Function สร้าง Chart ตามโหมด และ วิชา
    Private Sub CreateRadChart(ByVal StudentId As String, ByVal CalendarId As String, ByVal GroupsubjectId As String, ByVal Mode As String, Optional ByVal TeacherId As String = "")

        Dim dt As DataTable
        If Mode = "Homework" Then
            dt = ClsHomework.GetDtQuizIdHomeworkForCreateChart(StudentId, CalendarId, GroupsubjectId, TeacherId)
            CreateChartByDt(dt, "การบ้าน")
        ElseIf Mode = "Quiz" Then
            dt = ClsActivity.GetDtQuizForCreateChart(StudentId, CalendarId, GroupsubjectId, TeacherId)
            CreateChartByDt(dt, "ควิซ")
        End If

    End Sub

    'Function สร้าง Chart
    Private Sub CreateChartByDt(ByVal dt As DataTable, ByVal ChartTitle As String)

        If dt.Rows.Count > 0 Then
            RadChart1.Clear()
            RadChart1.PlotArea.XAxis.AutoScale = False
            RadChart1.PlotArea.XAxis.AddRange(0, dt.Rows.Count - 1, 1)
            lblChart.Text = GetStringForlblChart(Session("SelectedCalendarId"))
            Dim SeriesStudent As New Telerik.Charting.ChartSeries
            SeriesStudent.Type = Telerik.Charting.ChartSeriesType.Bar
            Dim StudentName As String = GetStudentNameByStudentId(Session("SelectedStudentId"))
            SeriesStudent.Name = StudentName
            Dim SeriesClass As New Telerik.Charting.ChartSeries
            SeriesClass.Type = Telerik.Charting.ChartSeriesType.Bar
            SeriesClass.Name = "ห้อง"
            Dim SeriesRoom As New Telerik.Charting.ChartSeries
            SeriesRoom.Type = Telerik.Charting.ChartSeriesType.Bar
            SeriesRoom.Name = "ขั้น"
            For index = 0 To dt.Rows.Count - 1
                RadChart1.PlotArea.XAxis(index).TextBlock.Text = dt.Rows(index)("Testset_Name")
                Dim ChartStudent As New Telerik.Charting.ChartSeriesItem
                Dim ChartClass As New Telerik.Charting.ChartSeriesItem
                Dim ChartRoom As New Telerik.Charting.ChartSeriesItem
                ChartStudent.YValue = dt.Rows(index)("StudentScore")
                ChartClass.YValue = dt.Rows(index)("ClassScore")
                ChartRoom.YValue = dt.Rows(index)("RoomScore")
                SeriesStudent.Items.Add(ChartStudent)
                SeriesClass.Items.Add(ChartClass)
                SeriesRoom.Items.Add(ChartRoom)
            Next

            'RadChart1.ChartTitle.TextBlock.Text = ChartTitle
            DivChart.Controls.Clear()
            RadChart1.ChartTitle.Appearance.Visible = False
            RadChart1.AddChartSeries(SeriesStudent)
            RadChart1.AddChartSeries(SeriesClass)
            RadChart1.AddChartSeries(SeriesRoom)
            DivChart.Controls.Add(RadChart1)
        End If

    End Sub

    'Function สร้างข้อความใต้ Chart
    Private Function GetStringForlblChart(ByVal CalendarId As String)

        Dim sql As String = "  SELECT 'ปีการศึกษา ' + Calendar_Year + ' ' + Calendar_Name FROM dbo.t360_tblCalendar WHERE Calendar_Id = '" & CalendarId & "' "
        Dim StringCalendar As String = _DB.ExecuteScalar(sql)
        Return StringCalendar

    End Function

    'Function หาชื่อนักเรียน
    Private Function GetStudentNameByStudentId(ByVal StudentId As String) As String

        Dim sql As String = " select Student_FirstName from t360_tblStudent where Student_Id = '" & StudentId & "' and Student_IsActive = 1 "
        Dim StudentName As String = _DB.ExecuteScalar(sql)
        Return StudentName

    End Function

#End Region

End Class