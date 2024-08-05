Imports BusinessTablet360

Public Class ClsGraphDashboard
    Dim db As New ClassConnectSql()
    Dim KnSession As New KNAppSession()

    Private _DashboardMode As Integer
    Private _UserId As String

    Public Sub New(ByVal Mode As Integer, ByVal User_Id As String)
        _DashboardMode = Mode
        _UserId = User_Id
    End Sub

    Private Function GetSqlGraph(Optional ByVal IsPieGraph As Boolean = False) As String
        Dim sql As New StringBuilder()
        Select Case _DashboardMode
            Case EnumDashBoardType.Quiz
                sql.Append(" SELECT (t360_ClassName + t360_RoomName) AS ClassName,COUNT(t360_ClassName + t360_RoomName) AS NoOfStu   ")
                sql.Append(" ,replicate('0', 20-len(t360_ClassName)) + t360_ClassName + replicate('0', 20-len(replace(t360_RoomName,'/',''))) +  replace(t360_RoomName,'/','') as FixedLengthClassAndRoom ")
                sql.Append(" FROM tblQuiz INNER JOIN tblAssistant ON tblQuiz.[User_Id] = tblAssistant.Teacher_id ")
                sql.Append("  INNER JOIN t360_tblRoom r ON r.Class_Name = t360_ClassName  AND r.Room_Name = tblQuiz.t360_RoomName WHERE tblAssistant.Assistant_id = '")
                sql.Append(_UserId)
                sql.Append("' AND tblQuiz.IsActive = 1 AND Calendar_id = '")
                sql.Append(KnSession.StoredValue("SelectedCalendarId"))
                sql.Append("' AND tblQuiz.IsQuizMode = 1 AND tblQuiz.t360_ClassName IS NOT NULL AND r.Room_IsActive = 1 ")
                sql.Append(" GROUP BY (t360_ClassName + t360_RoomName) ")
                sql.Append(" , replicate('0', 20-len(t360_ClassName)) + t360_ClassName + replicate('0', 20-len(replace(t360_RoomName,'/',''))) +  replace(t360_RoomName,'/','') ")
                sql.Append(" ORDER BY FixedLengthClassAndRoom;")
            Case EnumDashBoardType.Homework
                If IsPieGraph Then
                    sql.Append(" SELECT CASE tblAllStatus.AllStatus WHEN 0 THEN 'ยังไม่ได้ทำ' WHEN 1 THEN 'ยังไม่เสร็จ' WHEN 2 THEN 'เสร็จแล้ว' END AS strStatus ,tblAllStatus.AllStatus,COUNT(*) AS SumStatus FROM  ")
                    'sql.Append("( SELECT CASE SUM(tblModuleDetailCompletion.Module_Status) WHEN (2 * COUNT(tblModuleDetailCompletion.Module_Status)) THEN 2 WHEN 0 THEN 0 ELSE 1 END AS AllStatus ")
                    sql.Append(" ( SELECT CASE tblModuleDetailCompletion.Module_Status WHEN 3  THEN 2 WHEN 0 THEN 0 WHEN 2 THEN 2 ELSE 1 END AS AllStatus ")
                    sql.Append(" FROM tblmodule INNER JOIN tblModuleDetail ON tblmodule.Module_Id = tblModuleDetail.Module_Id  ")
                    sql.Append(" INNER JOIN tblModuleAssignment ON tblmodule.Module_Id = tblModuleAssignment.Module_Id ")
                    sql.Append(" INNER JOIN tblModuleDetailCompletion ON tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id ")
                    sql.Append(" INNER JOIN tblAssistant ON tblAssistant.Teacher_id = tblmodule.Create_By WHERE tblAssistant.Assistant_id = '")
                    sql.Append(_UserId)
                    sql.Append("' AND tblModule.IsActive = 1 AND tblModuleDetail.IsActive = 1  ")
                    sql.Append(" AND dbo.GetThaiDate() > tblModuleAssignment.Start_Date AND dbo.GetThaiDate() < tblModuleAssignment.End_Date ") 'ช่วงเวลาก่อน deadline
                    'sql.Append(" AND tblModuleAssignment.Calendar_Id = ( select top 1 calendar_id from t360_tblCalendar where Calendar_Type = 3 and Calendar_Id = tblModuleAssignment.calendar_id order by Calendar_FromDate desc) ")
                    sql.Append(" AND tblModuleAssignment.Calendar_Id = '")
                    sql.Append(KnSession.StoredValue("SelectedCalendarId"))
                    'sql.Append("' GROUP BY tblmodule.Module_Id,tblModuleDetailCompletion.Student_Id  ")
                    sql.Append("' ) AS tblAllStatus")
                    sql.Append(" GROUP BY tblAllStatus.AllStatus; ")
                Else
                    'sql.Append(" SELECT (SUM((tblQuizSession.TotalScore / tblQuiz.FullScore) * 100) / COUNT(*)) AS AVGR, ")
                    'sql.Append(" SUM(CAST((CASE WHEN tblModuleDetailCompletion.TimeExitedByUser IS NULL THEN 0 ELSE 1 END) AS float)) / COUNT(*) * 100 AS IsExitedByUserPercent  , ")
                    'sql.Append(" SUM(CAST((CASE tblQuizSession.SessionStatus WHEN 3 THEN 1 ELSE 0 END) AS float)) / COUNT(*) * 100 AS IsAllAnsweredPercent ")
                    sql.Append(" SELECT ROUND(SUM(round(( cast(tblQuizSession.TotalScore as float) / cast(tblQuiz.FullScore as float)),2) * 100 )/ COUNT(*),2)   AS AVGR, ")
                    sql.Append(" ROUND(SUM(CAST((CASE WHEN tblModuleDetailCompletion.TimeExitedByUser IS NULL THEN 0 ELSE 1 END) AS float)) / COUNT(*),2) * 100 AS IsExitedByUserPercent  , ")
                    sql.Append(" ROUND( SUM(CAST((CASE tblQuizSession.SessionStatus WHEN 3 THEN 1 ELSE 0 END) AS float)) / COUNT(*),2) * 100 AS IsAllAnsweredPercent  ")
                    sql.Append(" FROM tblModule INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id ")
                    sql.Append(" INNER JOIN tblModuleAssignment ON tblmodule.Module_Id = tblModuleAssignment.Module_Id ")
                    sql.Append(" INNER JOIN tblModuleDetailCompletion ON tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id ")
                    sql.Append(" INNER JOIN tblAssistant ON tblAssistant.Teacher_id = tblModule.Create_By ")
                    sql.Append(" INNER JOIN tblQuizSession ON tblModuleDetailCompletion.Quiz_Id = tblQuizSession.Quiz_Id AND tblModuleDetailCompletion.Student_Id = tblQuizSession.Player_Id ")
                    sql.Append(" INNER JOIN tblQuiz ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id ")
                    sql.Append(" WHERE tblAssistant.Assistant_id = '")
                    sql.Append(_UserId)
                    sql.Append("' AND tblModule.IsActive = 1 AND tblModuleDetail.IsActive = 1 ")
                    sql.Append(" AND dbo.GetThaiDate() > tblModuleAssignment.End_Date ") 'หลัง deadline
                    'sql.Append(" AND tblModule.Calendar_Id = ( select top 1 calendar_id from t360_tblCalendar where Calendar_Type = 3 and Calendar_Id = tblModule.calendar_id order by Calendar_FromDate desc); ")
                    sql.Append(" AND tblModuleAssignment.Calendar_Id = '")
                    sql.Append(KnSession.StoredValue("SelectedCalendarId"))
                    sql.Append("';")
                End If
            Case EnumDashBoardType.Practice
                sql.Append(" SELECT (tblQuiz.t360_ClassName + tblQuiz.t360_RoomName) AS ClassName,COUNT(DIstinct tblQuiz.User_Id) AS NoOfStudent,COUNT(tblQuiz.t360_ClassName + tblQuiz.t360_RoomName) AS NoOfPractice FROM tblTestSet ")
                sql.Append(" INNER JOIN tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN tblAssistant ON tblTestSet.UserId = tblAssistant.Teacher_id AND tblQuiz.User_Id <> tblAssistant.Assistant_id")
                sql.Append(" WHERE tblAssistant.Assistant_id = '")
                sql.Append(_UserId)
                sql.Append("' AND tblQuiz.IsPracticeMode = 1 AND tblQuiz.IsActive = 1 AND tblQuiz.Calendar_id = '")
                sql.Append(KnSession.StoredValue("SelectedCalendarId"))
                sql.Append("' AND tblQuiz.t360_ClassName <> 'NULL' and tblquiz.t360_RoomName <> 'NULL' ")
                sql.Append(" Group BY (tblQuiz.t360_ClassName + tblQuiz.t360_RoomName) ORDER BY (tblQuiz.t360_ClassName + tblQuiz.t360_RoomName); ")
        End Select
        Return sql.ToString()
    End Function

    Public Function GetDataGraph(Optional ByVal IsPieGraph As Boolean = False) As DataTable
        Dim sql As String = GetSqlGraph(IsPieGraph)
        GetDataGraph = db.getdata(sql)
    End Function
End Class
