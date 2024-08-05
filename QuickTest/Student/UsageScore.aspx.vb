Imports Telerik.Web.UI
Imports KnowledgeUtils

Public Class UsageScore
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TestsetHeaderControl1.SetByTestSetId("0F20DC7A-4E77-43F5-A3FA-00B906E5C417")
        CheckModeAndCreateGrid()
    End Sub

#Region "Function & Query"

    Private Sub CheckModeAndCreateGrid()
        Dim dt As DataTable

        If Request.QueryString("MA_Id") IsNot Nothing Then
            Dim TestsetId As String = GetTestsetByMaId(Request.QueryString("MA_Id").ToString())
            'TestsetHeaderControl1.SetByQuizId(Request.QueryString("HwQuizId").ToString(), Request.QueryString("StudentId").ToString())
            TestsetHeaderControl1.SetByMaId(Request.QueryString("MA_Id").ToString())
            dt = GetDtHomework(Request.QueryString("MA_Id").ToString())
            GridHomewrok(dt)
        ElseIf Request.QueryString("TestSetId") IsNot Nothing Then
            TestsetHeaderControl1.SetByTestSetId(Request.QueryString("TestsetId").ToString())
            dt = GetDtPractice(Request.QueryString("TestSetId").ToString())
            GridPractice(dt)
        ElseIf Request.QueryString("QuizId") IsNot Nothing Then
            TestsetHeaderControl1.SetByQuizId(Request.QueryString("QuizId").ToString(), "")
            dt = GetDtQuiz(Request.QueryString("QuizId").ToString())
            GridQuiz(dt)
        End If

    End Sub

    Private Sub GridHomewrok(ByVal dt As DataTable)

        Dim dtUsage As DataTable = GenDtUsage(1)
        If dt.Rows.Count > 0 Then
            DynamicAddColumnRadGrid(1)

            For index = 0 To dt.Rows.Count - 1
                dtUsage.Rows.Add(index)("Student_Name") = dt.Rows(index)("Student_Name")
                dtUsage.Rows(index)("ClassRomm_Name") = dt.Rows(index)("ClassRomm_Name")
                dtUsage.Rows(index)("Score") = dt.Rows(index)("Score").ToString().ToPointplusScore()
                'เช็ค Column สถานะว่า ส่งแล้ว,ยังไม่ทำ,ยังไม่เสร็จ,ทำเสร็จแล้ว
                'If dt.Rows(index)("TimeExitedByUser") IsNot DBNull.Value Then
                '   dtUsage.Rows(index)("StartTime") = "เริ่มทำ " & Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime() & " ส่ง " & Convert.ToDateTime(dt.Rows(index)("TimeExitedByUser")).ToPointPlusTime()
                'Else
                '    If dt.Rows(index)("Module_Status") = 0 Then
                '        dtUsage.Rows(index)("Module_Status") = "ยังไม่ทำ"
                '        dtUsage.Rows(index)("StartTime") = "เริ่มทำ -"
                '    ElseIf dt.Rows(index)("Module_Status") = 1 Then
                '        dtUsage.Rows(index)("Module_Status") = "ยังไม่เสร็จ"
                '        dtUsage.Rows(index)("StartTime") = "เริ่มทำ " & Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime() & " ส่ง -"
                '    Else
                '        dtUsage.Rows(index)("Module_Status") = "เสร็จแล้ว"
                '        dtUsage.Rows(index)("StartTime") = "เริ่มทำ " & Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime() & " ส่ง -"
                '    End If
                'End If
                Dim ClsAc As New ClsActivity(New ClassConnectSql)
                dtUsage.Rows(index)("Module_Status") = ClsAc.GetStatusQuiz(dt.Rows(index)("Quiz_Id").ToString(), dt.Rows(index)("Student_Id").ToString(), EnumDashBoardType.Homework, dt.Rows(index)("TimeExitedByUser").ToString())

                If dtUsage.Rows(index)("Module_Status") <> "นร.ยังไม่ได้เข้าทำเลย" Then
                    If dt.Rows(index)("TimeExitedByUser") IsNot DBNull.Value Then
                        dtUsage.Rows(index)("StartTime") = "เริ่มทำ " & Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime() & " ส่ง " & Convert.ToDateTime(dt.Rows(index)("TimeExitedByUser")).ToPointPlusTime()
                    Else
                        dtUsage.Rows(index)("StartTime") = "เริ่มทำ " & Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime()
                    End If

                Else
                    dtUsage.Rows(index)("StartTime") = "-"
                End If
            Next
            GridDetail.DataSource = dtUsage
            GridDetail.DataBind()
        End If

    End Sub

    Private Sub GridPractice(ByVal dt As DataTable)
        Dim clsAc As New ClsActivity(New ClassConnectSql)
        Dim dtUsage As DataTable = GenDtUsage(2)
        DynamicAddColumnRadGrid(2)
        If dt.Rows.Count > 0 Then

            For index = 0 To dt.Rows.Count - 1
                dtUsage.Rows.Add(index)("Student_Name") = dt.Rows(index)("Student_Name")
                dtUsage.Rows(index)("Student_ClassRoom") = dt.Rows(index)("Student_ClassRoom")
                dtUsage.Rows(index)("Score") = dt.Rows(index)("Score").ToString().ToPointplusScore()
                dtUsage.Rows(index)("StartTime") = clsAc.GetTimeDoQuiz(dt.Rows(index)("Quiz_id").ToString, dt.Rows(index)("Student_id").ToString)
            Next
            GridDetail.DataSource = dtUsage
            GridDetail.DataBind()
        End If

    End Sub

    Private Sub GridQuiz(ByVal dt As DataTable)

        DynamicAddColumnRadGrid(3)
        Dim clsAc As New ClsActivity(New ClassConnectSql)
        'Dim dtScore As DataTable = clsAc.GetStatusQuiz()
        If dt.Rows.Count > 0 Then

            For Each r As DataRow In dt.Rows
                r("Score") = r("Score").ToString().ToPointplusScore()
                r("QuizStatus") = clsAc.GetStatusQuiz(r("Quiz_Id").ToString, r("Player_Id").ToString, EnumDashBoardType.Quiz)
                If r("QuizStatus") <> "นร.ยังไม่ได้เข้าทำเลย" Then
                    r("StudentTime") = clsAc.GetTimeDoQuiz(r("Quiz_Id").ToString, r("Player_Id").ToString)
                Else
                    r("StudentTime") = "-"
                End If

            Next

            GridDetail.DataSource = dt
            GridDetail.DataBind()
        End If

    End Sub

    Private Function GetDtHomework(ByVal MA_Id As String) As DataTable

        'Dim dt As New DataTable
        'dt.Columns.Add("Student_Name", GetType(String))
        'dt.Columns.Add("ClassRomm_Name", GetType(String))
        'dt.Columns.Add("Score", GetType(String))
        'dt.Columns.Add("Module_Status", GetType(String))
        'dt.Columns.Add("StartTime", GetType(String))
        'dt.Columns.Add("TimeExitedByUser", GetType(String))

        'For index = 0 To 30
        '    dt.Rows.Add(index)("Student_Name") = "สมชาข หัวจรดเท้า"
        '    dt.Rows(index)("ClassRomm_Name") = "ม.1/4"
        '    dt.Rows(index)("Score") = "10/50"
        '    dt.Rows(index)("Module_Status") = "0"
        '    dt.Rows(index)("StartTime") = Date.Now
        '    dt.Rows(index)("TimeExitedByUser") = Date.Now
        'Next

        'Dim sql As String = " SELECT tblModuleDetailCompletion.Quiz_Id,tblModuleDetailCompletion.Student_Id, t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS Student_Name, " & _
        '                    " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom AS ClassRomm_Name, " & _
        '                    " CASE WHEN tblQuizSession.TotalScore IS NULL THEN '0' ELSE CAST(tblQuizSession.TotalScore as varchar(max)) END + '/' " & _
        '                    " + CAST(tblQuiz.FullScore as varchar(max)) AS Score, tblModuleDetailCompletion.Module_Status, tblQuiz.StartTime, " & _
        '                    " tblModuleDetailCompletion.TimeExitedByUser FROM tblModuleDetailCompletion INNER JOIN " & _
        '                    " tblModuleDetail ON tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id INNER JOIN " & _
        '                    " t360_tblStudent ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id INNER JOIN " & _
        '                    " tblModuleAssignment ON tblModuleDetail.Module_Id = tblModuleAssignment.Module_Id AND " & _
        '                    " tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id INNER JOIN " & _
        '                    " tblQuiz ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id LEFT JOIN " & _
        '                    " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND t360_tblStudent.Student_Id = tblQuizSession.Player_Id " & _
        '                    " WHERE (t360_tblStudent.Student_IsActive = 1) AND (tblModuleAssignment.MA_Id = '" & MA_Id & "') " & _
        '                    " ORDER BY dbo.FixedLengthClassAndRoom(t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom), Student_CurrentNoInRoom  "
        Dim sql As String = " SELECT tblModuleDetailCompletion.Quiz_Id,tblModuleDetailCompletion.Student_Id, t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS Student_Name," & _
                            " t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom AS ClassRomm_Name," & _
                            " CASE WHEN tblQuizSession.TotalScore IS NULL THEN '0' ELSE CAST(tblQuizSession.TotalScore as varchar(max)) END + '/'  + CAST(tblQuiz.FullScore as varchar(max)) AS Score," & _
                            " tblModuleDetailCompletion.Module_Status, min(tblQuizScore.FirstResponse) as StartTime,  tblModuleDetailCompletion.TimeExitedByUser FROM tblModuleDetailCompletion" & _
                            " INNER JOIN  tblModuleDetail ON tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id" & _
                            " INNER JOIN  t360_tblStudent ON tblModuleDetailCompletion.Student_Id = t360_tblStudent.Student_Id" & _
                            " INNER JOIN  tblModuleAssignment ON tblModuleDetail.Module_Id = tblModuleAssignment.Module_Id AND  tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id " & _
                            " INNER JOIN  tblQuiz ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id" & _
                            " LEFT JOIN  tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND t360_tblStudent.Student_Id = tblQuizSession.Player_Id" & _
                            " LEFT JOIN tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id and tblModuleDetailCompletion.Student_Id = tblQuizScore.Student_Id" & _
                            " WHERE (t360_tblStudent.Student_IsActive = 1) AND (tblModuleAssignment.MA_Id = '" & MA_Id & "')  AND (tblModuleDetailCompletion.IsActive = 1)" & _
                            " group by  tblModuleDetailCompletion.Quiz_Id,tblModuleDetailCompletion.Student_Id, t360_tblStudent.Student_FirstName , t360_tblStudent.Student_LastName ," & _
                            " t360_tblStudent.Student_CurrentClass , t360_tblStudent.Student_CurrentRoom,tblQuizSession.TotalScore,tblQuiz.FullScore, tblModuleDetailCompletion.Module_Status," & _
                            " tblModuleDetailCompletion.TimeExitedByUser, Student_CurrentNoInRoom ORDER BY dbo.FixedLengthClassAndRoom(t360_tblStudent.Student_CurrentClass,t360_tblStudent.Student_CurrentRoom), Student_CurrentNoInRoom "
        ' Query คะแนนให้เหมือนกันให้หมดกับที่ใช้ในหน้าอื่นๆ คือ CAST(tblQuiz.FullScore as varchar(max))
        ' " CASE WHEN tblQuizSession.TotalScore IS NULL THEN '0' ELSE CAST(CAST(tblQuizSession.TotalScore AS int) AS VARCHAR(MAX)) END + '/' "
        ' " + CAST(CAST(tblQuiz.FullScore AS int) AS VARCHAR(MAX)) AS Score, tblModuleDetailCompletion.Module_Status, tblQuiz.StartTime, " 

        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetDtQuiz(ByVal QuizId As String) As DataTable

        Dim sql As String = " SELECT tblquiz.Quiz_Id,tblquizsession.Player_Id,(t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName)AS Student_Name, " & _
                            " (CAST(tblQuizSession.TotalScore AS VARCHAR(max)) + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) AS Score, '' as QuizStatus , '' as StudentTime " & _
                            " FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN  " & _
                            " t360_tblStudent ON tblQuizSession.Player_Id = t360_tblStudent.Student_Id WHERE " & _
                            " (t360_tblStudent.Student_IsActive = 1) AND (tblQuiz.Quiz_Id = '" & QuizId & "') "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetDtPractice(ByVal TestSetId As String) As DataTable

        Dim sql As String = " SELECT tblquiz.Quiz_Id,t360_tblStudent.Student_Id,(t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName) AS Student_Name " & _
                            " , (t360_tblStudent.Student_CurrentClass + t360_tblStudent.Student_CurrentRoom) AS Student_ClassRoom,  " & _
                            " (CAST(tblQuizSession.TotalScore AS VARCHAR(max))  + '/' + CAST(tblQuiz.FullScore AS VARCHAR(max)) ) AS Score, tblQuiz.StartTime " & _
                            " FROM tblQuizSession INNER JOIN tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN " & _
                            " t360_tblStudent ON tblQuizSession.Player_Id = t360_tblStudent.Student_Id WHERE " & _
                            " (t360_tblStudent.Student_IsActive = 1) AND (tblQuiz.TestSet_Id = '" & TestSetId & "') " & _
                            "  AND (tblQuiz.IsPracticeMode = 1) "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function


    Private Sub DynamicAddColumnRadGrid(ByVal Mode As Integer)

        Dim BoundColumn As New GridBoundColumn
        GridDetail.AutoGenerateColumns = False
        GridDetail.Columns.Clear()
        'If Not IsPostBack Then
        If Mode = 1 Then

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderStyle.Width = Unit.Pixel(250)
            BoundColumn.HeaderText = "ชื่อ"
            BoundColumn.DataField = "Student_Name"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderStyle.Width = Unit.Pixel(73)
            BoundColumn.HeaderText = "ห้อง"
            BoundColumn.DataField = "ClassRomm_Name"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderStyle.Width = Unit.Pixel(75)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderStyle.Width = Unit.Pixel(170)
            BoundColumn.HeaderText = "สถานะ"
            BoundColumn.DataField = "Module_Status"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderStyle.Width = Unit.Pixel(210)
            BoundColumn.HeaderText = "เวลา"
            BoundColumn.DataField = "StartTime"

        ElseIf Mode = 2 Then

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "ชื่อ"
            BoundColumn.DataField = "Student_Name"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "ห้อง"
            BoundColumn.DataField = "Student_ClassRoom"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "เริ่มทำ"
            BoundColumn.DataField = "StartTime"

        ElseIf Mode = 3 Then

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "ชื่อ"
            BoundColumn.DataField = "Student_Name"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"
            BoundColumn.FilterControlWidth = 20

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "สถานะ"
            BoundColumn.DataField = "QuizStatus"

            BoundColumn = New GridBoundColumn
            GridDetail.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "เวลา"
            BoundColumn.DataField = "StudentTime"

        End If
        'End If
    End Sub

    Private Function GenDtUsage(ByVal Mode As String)

        Dim dt As New DataTable

        If Mode = 1 Then
            dt.Columns.Add("Student_Name", GetType(String))
            dt.Columns.Add("ClassRomm_Name", GetType(String))
            dt.Columns.Add("Score", GetType(String))
            dt.Columns.Add("Module_Status", GetType(String))
            dt.Columns.Add("StartTime", GetType(String))
        ElseIf Mode = 2 Then
            dt.Columns.Add("Student_Name", GetType(String))
            dt.Columns.Add("Student_ClassRoom", GetType(String))
            dt.Columns.Add("Score", GetType(String))
            dt.Columns.Add("StartTime", GetType(String))
        ElseIf Mode = 3 Then
            dt.Columns.Add("Student_Name", GetType(String))
            dt.Columns.Add("Score", GetType(String))
        End If

        Return dt

    End Function

    Public Function GetTestsetByMaId(ByVal MaId As String)

        Dim sql As String = " SELECT Reference_Id FROM dbo.tblModuleAssignment INNER JOIN dbo.tblModuleDetail ON dbo.tblModuleAssignment.Module_Id = " & _
                            " dbo.tblModuleDetail.Module_Id WHERE MA_Id = '" & MaId & "' "
        Dim TestSetId As String = _DB.ExecuteScalar(sql)
        Return TestSetId
    End Function

#End Region

End Class