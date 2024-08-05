Imports System.Web
Imports System.Data.SqlClient
Imports System.Text

Namespace Service

    Public Class ClsHomework
        Dim _DB As ClsConnect
        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB
        End Sub

        Public Function SaveQuizHomework(ByVal testsetId As String, ByVal StudentAmont As String) As String

            Dim ClsPracticeMode As New Service.ClsPracticeMode(New ClassConnectSql)
            Dim ClsActivity As New Service.ClsActivity(New ClassConnectSql)
            ' Dim KnSession As New ClsKNSession(False)

            'HttpContext.Current.Session("Quiz_Id") = ClsPracticeMode.SaveQuizDetail(testsetId, dtPlayer.Rows(0)("ClassName").ToString, dtPlayer.Rows(0)("RoomName").ToString, _
            'dtPlayer.Rows(0)("School_Code").ToString, dtPlayer.Rows(0)("Student_Id").ToString, "1")

            'Dim ClassName As String = ""
            'Dim RoomName As String = ""
            Dim SchoolCode As String = HttpContext.Current.Session("SchoolId")
            Dim player_Id = HttpContext.Current.Session("UserId")

            Dim Quiz_Id As String = ""

            Quiz_Id = ClsPracticeMode.SaveQuizDetail(testsetId, SchoolCode, player_Id.ToString, "1", True, StudentAmont, (New KNAppSession)("CurrentCalendarId").ToString, "2")
            'HttpContext.Current.Session("HomeworkMode") = True

            'Dim ArrQuizIdState() As String = Split(QuizIdState, ":")

            'ClsActivity.getQsetInQuiz(testsetId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion
            'ClsPracticeMode.SaveQuiznswer(HttpContext.Current.Session("Quiz_Id"), HttpContext.Current.Session("UserId"), SchoolCode, HttpContext.Current.Session("PDeviceId"), False)
            'ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, False, ClsPracticeMode.GetFirstQsetTypeFromQuizId(HttpContext.Current.Session("Quiz_Id")))

            Return Quiz_Id

        End Function

        'Function หาข้อมูลของการบ้านของนักเรียนคนนั้นในเทอมนั้น ใช้หน้า TeacherStudentDetailpage
        Public Function GetDtHomeByStudentCalendarAndTeacherId(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByRef InputConn As SqlConnection = Nothing, Optional ByRef TimeMode As String = Nothing)
            Dim dt As New DataTable
            Dim sql As String = ""
            'Select
            sql = " SELECT tblTestSet.Testset_Id,tblTestSet.TestSet_Name, CASE WHEN dbo.tblQuizSession.TotalScore IS NULL THEN '0' + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) " &
                      " ELSE CAST(tblQuizSession.TotalScore AS varchar(10)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) END AS Score, tblQuiz.Quiz_Id, " &
                      " tblModuleAssignment.MA_Id, tblModuleAssignment.End_Date, tblModuleDetailCompletion.Module_Status, tblModuleDetailCompletion.TimeExitedByUser, " &
                      " tblQuiz.Quiz_Id AS Expr1, CASE WHEN dbo.GetThaiDate() > tblModuleAssignment.End_Date THEN 1 ELSE 0 END AS IsEnd,Count(distinct tblquizquestion.QQ_No)  as QuestionAmount" &
                      "  ,case when tblQuizSession.IsActive is null then 0 else tblQuizSession.IsActive end as IsDoQuiz, count(distinct Answer_Id) as AnsweredAmount,tblQuiz.StartTime "

            If TeacherId = "" Then

                sql &= " FROM tblModule INNER JOIN " &
                      " tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN tblModuleDetailCompletion " &
                      " ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN  tblModuleDetail ON " &
                      " tblModule.Module_Id = tblModuleDetail.Module_Id AND tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id INNER JOIN " &
                      " tblTestSet ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id LEFT OUTER JOIN tblQuiz " &
                      " ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id LEFT OUTER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id " &
                      " AND tblModuleDetailCompletion.Student_Id = tblQuizSession.Player_Id " &
                      " INNER JOIN tblQuizQuestion on tblquiz.Quiz_Id = tblQuizQuestion.Quiz_Id Left Outer join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id and tblQuizScore.QQ_No = tblQuizQuestion.QQ_No" &
                      " WHERE (tblQuiz.IsHomeWorkMode = 1 OR dbo.tblQuiz.IsHomeWorkMode IS NULL) AND " &
                      " (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (tblModuleDetail.Reference_Type = 0) " &
                      " AND (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') "
                If ClsKNSession.IsMaxONet Then sql &= " AND tblModuleDetailCompletion.Module_Status > 0 "
            Else
                sql &= " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " &
                      " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN " &
                      " tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id " &
                      " AND tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id INNER JOIN tblTestSet " &
                      " ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id LEFT OUTER JOIN tblQuiz ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id " &
                      " LEFT OUTER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND tblModuleDetailCompletion.Student_Id = tblQuizSession.Player_Id " &
                      "  INNER JOIN tblQuizQuestion on tblquiz.Quiz_Id = tblQuizQuestion.Quiz_Id Left Outer join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id and tblQuizScore.QQ_No = tblQuizQuestion.QQ_No" &
                      " WHERE (tblQuiz.IsHomeWorkMode = 1 OR dbo.tblQuiz.IsHomeWorkMode IS NULL) AND (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') " &
                      " AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (tblModuleDetail.Reference_Type = 0) AND (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') "

            End If

            sql &= " AND (tblModuleDetailCompletion.IsActive = 1) "

            If TimeMode <> 4 And TimeMode IsNot Nothing Then
                If TimeMode = 0 Then
                    sql &= " AND (convert(varchar(10),tblModuleAssignment.Start_Date,120) >= convert(varchar(10),dbo.GetThaiDate(),120)) "
                Else
                    sql &= " AND (convert(varchar(10),tblModuleAssignment.Start_Date,120) >= convert(varchar(10),dbo.GetThaiDate() - " & TimeMode & ",120)) "
                End If
            End If

            sql &= " group by tblTestSet.TestSet_Id,tblTestSet.TestSet_Name, dbo.tblQuizSession.TotalScore,tblQuiz.FullScore,CAST(tblQuizSession.TotalScore AS varchar(MAX)),CAST(tblQuiz.FullScore AS VARCHAR(MAX))" &
                   ", tblQuiz.Quiz_Id,  tblModuleAssignment.MA_Id, tblModuleAssignment.End_Date, tblModuleDetailCompletion.Module_Status, tblModuleDetailCompletion.TimeExitedByUser,tblQuiz.Quiz_Id,tblModuleAssignment.End_Date,dbo.tblModule.LastUpdate,tblQuizSession.IsActive,tblQuiz.StartTime " &
                   "  ORDER BY dbo.tblModule.LastUpdate DESC "

            dt = _DB.getdata(sql, , InputConn)
            Return dt
        End Function

        ''' <summary>
        ''' function ในการหากิจกรรมรวมทั้งหมดของนักเรียน โหมด Maxonet
        ''' </summary>
        ''' <param name="StudentId"></param>
        ''' <param name="CalendarId"></param>
        ''' <returns></returns>
        Public Function GetMaxonetActivity(ByVal StudentId As String, ByVal CalendarId As String) As DataTable
            Dim dt As New DataTable
            Dim sql As New StringBuilder

            sql.Append(" SELECT tblTestSet.Testset_Id,tblTestSet.TestSet_Name, CASE WHEN dbo.tblQuizSession.TotalScore IS NULL THEN '0' + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) ")
            sql.Append(" ELSE CAST(tblQuizSession.TotalScore AS varchar(10)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) END AS Score, tblQuiz.Quiz_Id, ")
            sql.Append(" tblModuleAssignment.MA_Id, tblModuleAssignment.End_Date, tblModuleDetailCompletion.Module_Status, tblModuleDetailCompletion.TimeExitedByUser, ")
            sql.Append(" tblQuiz.Quiz_Id AS Expr1, CASE WHEN dbo.GetThaiDate() > tblModuleAssignment.End_Date THEN 1 ELSE 0 END AS IsEnd,Count(distinct tblquizquestion.QQ_No)  as QuestionAmount")
            sql.Append("  ,case when tblQuizSession.IsActive is null then 0 else tblQuizSession.IsActive end as IsDoQuiz, count(distinct Answer_Id) as AnsweredAmount,tblQuiz.StartTime as StartDateTime ,mad.Class_Name ")

            sql.Append(" FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id ")
            sql.Append(" INNER JOIN tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id ")
            sql.Append(" INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id AND tblModuleDetailCompletion.ModuleDetail_Id = tblModuleDetail.ModuleDetail_Id ")
            sql.Append(" INNER JOIN tblTestSet ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id ")
            sql.Append(" INNER JOIN tblModuleAssignmentDetail mad ON mad.MA_Id = tblModuleAssignment.MA_Id ")
            sql.Append(" LEFT OUTER JOIN tblQuiz ON tblModuleDetailCompletion.Quiz_Id = tblQuiz.Quiz_Id ")
            sql.Append(" LEFT OUTER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND tblModuleDetailCompletion.Student_Id = tblQuizSession.Player_Id ")
            sql.Append(" INNER JOIN tblQuizQuestion on tblquiz.Quiz_Id = tblQuizQuestion.Quiz_Id Left Outer join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id and tblQuizScore.QQ_No = tblQuizQuestion.QQ_No")
            sql.Append(" WHERE (tblQuiz.IsHomeWorkMode = 1 OR dbo.tblQuiz.IsHomeWorkMode IS NULL) AND ")
            sql.Append(" (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (tblModuleDetail.Reference_Type = 0) ")
            sql.Append(" AND (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "')") 'AND tblModuleDetailCompletion.Module_Status > 0 ")
            sql.Append(" AND (tblModuleDetailCompletion.IsActive = 1) ")

            sql.Append(" group by tblTestSet.TestSet_Id,tblTestSet.TestSet_Name, dbo.tblQuizSession.TotalScore,tblQuiz.FullScore,CAST(tblQuizSession.TotalScore AS varchar(MAX)),CAST(tblQuiz.FullScore AS VARCHAR(MAX))")
            sql.Append(", tblQuiz.Quiz_Id,  tblModuleAssignment.MA_Id, tblModuleAssignment.End_Date, tblModuleDetailCompletion.Module_Status, tblModuleDetailCompletion.TimeExitedByUser, ")
            sql.Append(" tblQuiz.Quiz_Id,tblModuleAssignment.End_Date,dbo.tblModule.LastUpdate,tblQuizSession.IsActive,tblQuiz.StartTime,mad.Class_Name ")
            sql.Append(" ORDER BY dbo.tblModule.LastUpdate DESC ")

            Return _DB.getdata(sql.ToString())

        End Function

        'หาข้อความ ว่าการบ้านนี้เป็น Pattern ไหน
        Public Function GetStringInBracket(ByVal ModuleStatus As Integer, ByVal TimeExiteByUser As String, ByVal IsEnd As Integer, QuestionAmount As Integer, IsDoQuiz As Integer, AnsweredAmount As Integer)

            Dim StringReturn As String = ""

            If IsDoQuiz = 0 Then 'เช็คก่อนว่าทำหรือเปล่า
                StringReturn = "นร.ยังไม่ได้เข้าทำเลย"
            Else
                If AnsweredAmount = 0 Then
                    StringReturn = "ไม่ได้กดตอบ"
                Else
                    If QuestionAmount = 1 Then
                        StringReturn = "ทำแล้ว"
                    Else
                        If QuestionAmount = AnsweredAmount Then
                            StringReturn = "ทำครบทุกข้อ"
                        Else
                            StringReturn = "ทำไม่ครบข้อ"
                        End If
                    End If
                End If
                If TimeExiteByUser = "" Then
                    StringReturn = StringReturn & " - ยังไม่ส่ง"
                Else
                    StringReturn = StringReturn & " - ส่งแล้ว"
                End If
            End If



            Return StringReturn

        End Function

        'Function เช็คว่า ในเทอมนี้ นักเรียนคนนี้ มีการบ้านหรือเปล่า Option TeacherId
        Public Function CheckIsHaveHomework(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

            Dim sql As String = ""
            Dim CountHomework As String = ""
            If TeacherId = "" Then
                sql = " SELECT COUNT(DISTINCT tblModuleDetailCompletion.Quiz_Id) AS CountHomework FROM tblModule INNER JOIN " &
                      " tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " &
                      " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " &
                      " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id AND " &
                      " tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id " &
                      " WHERE (tblModuleDetail.Reference_Type = 0) AND (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') AND " &
                      " (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') "
                CountHomework = _DB.ExecuteScalar(sql)
                If CType(CountHomework, Integer) > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                sql = " SELECT COUNT(DISTINCT tblModuleDetailCompletion.Quiz_Id) AS CountHomework " &
                      " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " &
                      " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " &
                      " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id AND " &
                      " tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id INNER JOIN " &
                      " tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id  WHERE (tblModuleDetail.Reference_Type = 0) AND " &
                      " (tblModuleDetailCompletion.Student_Id = '" & _DB.CleanString(StudentId) & "') " &
                      " AND (tblModuleAssignment.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " &
                      " AND (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') "
                CountHomework = _DB.ExecuteScalar(sql)
                If CType(CountHomework, Integer) > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        'Function เช็คว่า มีการบ้านที่เป็นวิชาเดียวหรือเปล่า
        Public Function CheckIsManySubject(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "") As Boolean

            Dim sql As String = " SELECT COUNT(DISTINCT Quiz_Id) FROM dbo.uvw_CountSubjectInTermByStudentIdHomework " &
                                " WHERE Calendar_Id = '" & _DB.CleanString(CalendarId) & "' AND Student_Id = '" & _DB.CleanString(StudentId) & "' AND (dbo.uvw_CountSubjectInTermByStudentIdHomework.TotalSubject > 1 ) "
            If TeacherId <> "" Then
                sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
            End If
            Dim CheckCount As String = _DB.ExecuteScalar(sql)
            If CType(CheckCount, Integer) > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        'Function หาวิชาทั้งหมดที่ได้มาจากนักเรียน ในเทอม
        Public Function GetSubjectNameByStudentId(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

            Dim sql As String = " SELECT DISTINCT tblGroupSubject.GroupSubject_Id, tblGroupSubject.GroupSubject_Name " &
                                " FROM tblGroupSubject INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN " &
                                " uvw_CountSubjectInTermByStudentIdHomework INNER JOIN tblQuiz ON uvw_CountSubjectInTermByStudentIdHomework.Quiz_Id = " &
                                " tblQuiz.Quiz_Id INNER JOIN tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN " &
                                " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " &
                                " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                                " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id " &
                                " = tblQuestionCategory.Book_Id WHERE (uvw_CountSubjectInTermByStudentIdHomework.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') AND " &
                                " (uvw_CountSubjectInTermByStudentIdHomework.Student_Id = '" & _DB.CleanString(StudentId) & "') AND (dbo.uvw_CountSubjectInTermByStudentIdHomework.TotalSubject = 1 ) "
            If TeacherId <> "" Then
                sql &= " AND (dbo.uvw_CountSubjectInTermByStudentIdHomework.Assistant_id = '" & _DB.CleanString(TeacherId) & "') "
            End If
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            Return dt

        End Function

        'Function หา QuizId ทั้งหมดเพื่อเอาไปสร้าง UserControl CompareChartcontrol
        Public Function GetDtQuizIdHomeworkForCreateChart(ByVal StudentId As String, ByVal CalendarId As String, ByVal GroupSubjectId As String, Optional ByVal TeacherId As String = "")

            Dim dt As New DataTable
            Dim sql As String = ""
            If GroupSubjectId = "" Then
                sql = " SELECT DISTINCT Quiz_Id FROM dbo.uvw_CountSubjectInTermByStudentIdHomework " &
                      " WHERE Student_Id = '" & _DB.CleanString(StudentId) & "' AND Calendar_Id = '" & _DB.CleanString(CalendarId) & "' " &
                      " AND TotalSubject > 1 "
                If TeacherId <> "" Then
                    sql &= " -AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
                End If
            Else
                sql = " SELECT DISTINCT uvw_CountSubjectInTermByStudentIdHomework.Quiz_Id FROM uvw_CountSubjectInTermByStudentIdHomework INNER JOIN " &
                      " tblQuiz ON uvw_CountSubjectInTermByStudentIdHomework.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN " &
                      " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN tblTestSetQuestionSet ON tblTestSet.TestSet_Id " &
                      " = tblTestSetQuestionSet.TestSet_Id INNER JOIN tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                      " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN " &
                      " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN tblGroupSubject ON tblBook.GroupSubject_Id " &
                      " = tblGroupSubject.GroupSubject_Id WHERE (uvw_CountSubjectInTermByStudentIdHomework.Student_Id = '" & _DB.CleanString(StudentId) & "') AND " &
                      " (uvw_CountSubjectInTermByStudentIdHomework.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " &
                      " AND (tblGroupSubject.GroupSubject_Id = '" & _DB.CleanString(GroupSubjectId) & "') " &
                      " AND (dbo.uvw_CountSubjectInTermByStudentIdHomework.TotalSubject = 1) "
                If TeacherId <> "" Then
                    sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
                End If
            End If

            dt = _DB.getdata(sql)
            '***********************************
            dt = DummyData()
            '***********************************
            Return dt

        End Function

        Public Function DummyData() As DataTable

            Dim dt As New DataTable
            dt.Columns.Add("Testset_Name", GetType(String))
            dt.Columns.Add("StudentScore", GetType(Decimal))
            dt.Columns.Add("ClassScore", GetType(Decimal))
            dt.Columns.Add("RoomScore", GetType(Decimal))

            For index = 0 To 4
                dt.Rows.Add(index)("Testset_Name") = "การบ้าน " & index
                Dim RandomNumber As New Random()
                dt.Rows(index)("StudentScore") = RandomNumber.Next(0, 50)
                dt.Rows(index)("ClassScore") = RandomNumber.Next(0, 50)
                dt.Rows(index)("RoomScore") = RandomNumber.Next(0, 50)
            Next
            Return dt

        End Function


    End Class

End Namespace


