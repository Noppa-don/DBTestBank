Imports System.Data.SqlClient
Imports System.Web
Imports KnowledgeUtils

Namespace Service

    Public Class ClsActivity
        Dim _DB As ClsConnect
        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB
        End Sub

        Public Sub getQsetInQuiz(ByVal testset_id As String, ByVal Quiz_Id As String, School_Code As String, Optional ByRef InputConn As SqlConnection = Nothing, Optional IsMaxOnet As Boolean = False, Optional tokenId As String = "")

            Dim db As New ClassConnectSql()

            Dim sqlGetQuestionInTestset As String = " SELECT tqs.QSet_Id,qs.QSet_Type,qs.QSet_IsRandomQuestion,qs.QSet_IsRandomAnswer FROM tblTestSetQuestionSet tqs LEFT JOIN tblQuestionSet qs "
            sqlGetQuestionInTestset &= " ON tqs.QSet_Id = qs.QSet_Id "
            sqlGetQuestionInTestset &= " WHERE tqs.TestSet_Id = '" & testset_id & "' AND tqs.IsActive = 1  ORDER BY NEWID(); "

            Dim dtQset As New DataTable()

            'If (isDifferentQuestion) Then
            '    sqlGetQuestionInTestset &= " ORDER BY NEWID(); "
            '    dtQset = db.getdata(sqlGetQuestionInTestset)
            '    insertQuestionToQuizQuestion(dtQset, True, testset_id)
            'Else
            dtQset = db.getdata(sqlGetQuestionInTestset, , InputConn)
            If IsMaxOnet Then

            End If
            insertQuestionToQuizQuestion(dtQset, testset_id, Quiz_Id, School_Code, InputConn, IsMaxOnet, tokenId)
            'End If

        End Sub


        Public Function InsertQuizScorePracticeMode(ByVal QuizId As String, ByVal SchoolCode As String, ByVal Examnum As Integer, ByVal IsUseComputer As Boolean, ByVal QsetType As String)

            If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or Examnum = 0 Then
                Return ""
            End If

            Dim sql As String

            If IsUseComputer Then
                sql = "select '974DFFB5-E261-4E8B-B89B-981EF7CDC9B2' as SR_ID,'" & HttpContext.Current.Application("DefaultUserId") & "' as User_Id"
            Else
                sql = " SELECT t360_tblStudentRoom.SR_ID, tblQuiz.User_Id"
                sql &= " FROM t360_tblStudent INNER JOIN"
                sql &= " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN"
                sql &= " tblQuiz ON t360_tblStudent.Student_Id = tblQuiz.User_Id"
                sql &= " where Quiz_Id = '" & QuizId & "'  "
            End If


            Dim dt As DataTable = _DB.getdata(sql)

            If QsetType = "6" Or QsetType = "3" Then
                sql = " INSERT INTO dbo.tblQuizScore(QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id ,  Answer_Id ,ResponseAmount , "
                sql &= " FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id , SR_ID,QQ_No)  "
                sql &= " SELECT Top 1 NEWID() , '" & QuizId & "', '" & SchoolCode & "', Question_Id, NULL ,0,dbo.GetThaiDate() , "
                sql &= " dbo.GetThaiDate() , 0 ,0 ,1 , '" & dt.Rows(0)("User_Id").ToString & "','" & dt.Rows(0)("SR_ID").ToString & "','" & Examnum & "' "
                sql &= " FROM tblQuizQuestion"
                sql &= " where (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "')"
            Else
                sql = " INSERT INTO dbo.tblQuizScore(QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id ,  Answer_Id ,ResponseAmount , "
                sql &= " FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id , SR_ID,QQ_No)  "
                sql &= " SELECT NEWID() , '" & QuizId & "', '" & SchoolCode & "', Question_Id, NULL ,0,dbo.GetThaiDate() , "
                sql &= " dbo.GetThaiDate() , 0 ,0 ,1 , '" & dt.Rows(0)("User_Id").ToString & "','" & dt.Rows(0)("SR_ID").ToString & "','" & Examnum & "' "
                sql &= " FROM tblQuizQuestion"
                sql &= " where (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "')"
            End If

            'รอถาม Query กับพี่ชินอีกทีว่าถูกไหม
            Try
                _DB.Execute(sql) 'Insert ข้อมูลลง tblQuizScore
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return ""
            End Try

            Return "Complete"

        End Function



        ' <<< insert ข้อสอบ จาก qset >>>
        Private Sub insertQuestionToQuizQuestion(ByVal dtQset As DataTable, ByVal testset_id As String, ByVal Quiz_Id As String,
                                                 School_Code As String, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal IsMaxOnet As Boolean = False, Optional ByVal tokenId As String = "")

            Dim db As New ClassConnectSql()
            Dim qq_no As Integer = 1

            For i As Integer = 0 To dtQset.Rows.Count - 1
                'QSet_IsRandomQuestion,QSet_IsRandomAnswer

                Dim sqlQuestionInQset As String

                'Select TOP
                If IsMaxOnet Then
                    Dim clp As New ClsPracticeMode(db)
                    Dim LimitAmount As String = clp.GetLimitExamAmount(tokenId)

                    If LimitAmount <> "" Then
                        sqlQuestionInQset = "SELECT top " & LimitAmount & " tsqd.Question_Id"
                    Else
                        sqlQuestionInQset = "Select tsqd.Question_Id"
                    End If
                Else
                    sqlQuestionInQset = "Select tsqd.Question_Id"
                End If

                'sql get question in qset
                sqlQuestionInQset &= " FROM tblTestSetQuestionSet tsqs LEFT JOIN tblTestSetQuestionDetail tsqd On tsqs.TSQS_Id = tsqd.TSQS_Id 
                                      WHERE tsqs.TestSet_Id = '" & testset_id & "' AND tsqs.QSet_Id = '" & dtQset(i)("QSet_Id").ToString() & "' AND tsqd.IsActive = '1' 
                                      And tsqs.IsActive = '1'   ORDER BY  "

                If dtQset(i)("QSet_IsRandomQuestion").ToString() = "True" Then
                    sqlQuestionInQset &= "NEWID();"
                Else
                    sqlQuestionInQset &= "tsqd_No;"
                End If

                Dim dtQuestionInQset As DataTable

                'If (isDiffQuestion) Then 'question ใน qset ต้องสุ่มหรือเปล่า
                '    sqlQuestionInQset &= " ORDER BY NEWID(); "
                '    dtQuestionInQset = db.getdata(sqlQuestionInQset)
                'Else
                dtQuestionInQset = db.getdata(sqlQuestionInQset, , InputConn)
                'End If

                db.OpenWithTransection(InputConn)
                Dim sqlInsertQuestion As String = ""
                If dtQset.Rows(i)("QSet_Type") = "6" Or dtQset.Rows(i)("QSet_Type") = "3" Then
                    For Each question As DataRow In dtQuestionInQset.Rows()
                        sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Quiz_Id & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & School_Code & "'); "
                        db.ExecuteWithTransection(sqlInsertQuestion, InputConn)
                    Next
                    qq_no = qq_no + 1
                Else
                    For Each question As DataRow In dtQuestionInQset.Rows()
                        sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Quiz_Id & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & School_Code & "'); "
                        db.ExecuteWithTransection(sqlInsertQuestion, InputConn)
                        qq_no = qq_no + 1
                    Next
                End If
                db.CommitTransection(InputConn)
            Next

        End Sub


        Public Function GetAcademicYear() As String

            Dim CurrentYear As Integer = Year(Now)
            Dim CurrentDate As New Date(Year(Now), Month(Now), Day(Now))
            Dim Fixdate As New Date(Year(Now), 3, 1)

            If DateValue(Fixdate) > DateValue(CurrentDate) Then
                CurrentYear -= 1
            End If

            If CurrentYear < 2400 Then
                CurrentYear += 543
            End If

            GetAcademicYear = CurrentYear.ToString()
            Return GetAcademicYear

        End Function

        'GetDt ชื่อควิซ,คะแนน (60/70),วันที่ควิซ
        Public Function GetDtQuizInfoByStudentId(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByRef InputConn As SqlConnection = Nothing, Optional ByRef TimeMode As String = Nothing)

            Dim sql As String = ""
            If TeacherId = "" Then
                '                sql = " SELECT tblTestSet.TestSet_Name, CAST(tblQuizSession.TotalScore AS VARCHAR(MAX)) + '/' +  CAST(tblQuiz.FullScore AS VARCHAR(MAX)) AS Score " &
                sql = " Select tblTestSet.TestSet_Name, Case When dbo.tblQuizSession.TotalScore Is NULL Then '0' + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) " &
                      " ELSE CAST(tblQuizSession.TotalScore AS varchar(10)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) END AS Score,  " &
                      " tblQuiz.StartTime,tblQuiz.Quiz_Id FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " &
                      " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id WHERE (tblQuiz.IsQuizMode = 1) AND (tblQuiz.IsActive = 1) AND " &
                      " (tblQuizSession.Player_Id = '" & StudentId.CleanSQL & "')  AND (dbo.tblQuiz.Calendar_Id = '" & CalendarId.CleanSQL & "') "
            Else

                'sql = " SELECT tblTestSet.TestSet_Name, ( CAST(dbo.tblQuizSession.TotalScore AS VARCHAR(MAX))  + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) " &

                sql = " Select tblTestSet.TestSet_Name, Case When dbo.tblQuizSession.TotalScore Is NULL Then '0' + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) " &
                      " ELSE CAST(tblQuizSession.TotalScore AS varchar(10)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) END AS Score,  " &
                      " tblQuiz.StartTime,tblQuiz.Quiz_Id FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " &
                      " tblAssistant ON tblQuiz.User_Id = tblAssistant.Teacher_id INNER JOIN tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id " &
                      " WHERE (tblAssistant.Assistant_id = '" & TeacherId.CleanSQL & "') AND (tblQuiz.IsActive = 1) " &
                      " AND (tblQuiz.IsQuizMode = 1) AND (tblQuizSession.Player_Id = '" & StudentId.CleanSQL & "') AND " &
                      " (tblQuiz.Calendar_Id = '" & CalendarId.CleanSQL & "') "
            End If

            If TimeMode <> 4 And TimeMode IsNot Nothing Then
                If TimeMode = 0 Then
                    sql &= "  AND (convert(varchar(10),tblQuiz.StartTime,120) >= convert(varchar(10),dbo.GetThaiDate(),120))"
                Else
                    sql &= "  AND (convert(varchar(10),tblQuiz.StartTime,120) >= convert(varchar(10),dbo.GetThaiDate() - " & TimeMode & ",120))"
                End If
            End If

            sql &= " ORDER BY tblQuiz.LastUpdate desc "

            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)
            Return dt

        End Function

        'Function เช็คว่าในเทอมนี้ นักเรียนคนนี้ มีควิซหรือไม่
        Public Function CheckIsHaveQuiz(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "") As Boolean
            Dim sql As String = ""
            Dim CountQuiz As String = ""
            If TeacherId = "" Then
                sql = " SELECT COUNT(DISTINCT tblQuizSession.Quiz_Id) AS CountQuiz FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id " & _
                      " WHERE (tblQuiz.IsQuizMode = 1) AND (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') AND (tblQuiz.IsActive = 1) AND (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') "
                CountQuiz = _DB.ExecuteScalar(sql)
                If CType(CountQuiz, Integer) > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                sql = " SELECT COUNT(DISTINCT tblQuizSession.Quiz_Id) AS CountQuiz FROM tblQuiz INNER JOIN tblQuizSession ON " & _
                      " tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN tblAssistant ON tblQuiz.User_Id = tblAssistant.Teacher_id " & _
                      " WHERE (tblQuiz.IsQuizMode = 1) AND (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') AND (tblQuiz.IsActive = 1) " & _
                      " AND (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') AND (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') "
                CountQuiz = _DB.ExecuteScalar(sql)
                If CType(CountQuiz, Integer) > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        'Function Check ว่า นักเรียนคนนี้ในเทอมนี้ มี quiz ที่เป็นหลายวิชาหรือเปล่า
        Public Function CheckIsManySubject(ByVal Student As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "") As Boolean

            Dim sql As String = ""
            Dim CountManySubject As String = ""

            sql = " SELECT COUNT(Quiz_id) FROM dbo.uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz WHERE " & _
                  " Calendar_Id = '" & _DB.CleanString(CalendarId) & "' AND Player_Id = '" & _DB.CleanString(Student) & "' AND TotalSubject > 1 "
            If TeacherId <> "" Then
                sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
            End If
            CountManySubject = _DB.ExecuteScalar(sql)
            If CType(CountManySubject, Integer) > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        'Function หาว่า Quiz ของนักเรียนคนนี้ในเทอมนี้ มีวิชาอะไรบ้าง
        Public Function GetSubjectNameByStudentId(ByVal Student As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "")

            Dim dt As New DataTable
            Dim sql As String = " SELECT DISTINCT tblGroupSubject.GroupSubject_Name,dbo.tblGroupSubject.GroupSubject_Id FROM tblGroupSubject INNER JOIN " & _
                                " tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN " & _
                                " uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz INNER JOIN tblQuiz ON uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Quiz_Id " & _
                                " = tblQuiz.Quiz_Id INNER JOIN tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN " & _
                                " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                                " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                                " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON " & _
                                " tblBook.BookGroup_Id = tblQuestionCategory.Book_Id WHERE (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Calendar_Id " & _
                                " = '" & _DB.CleanString(CalendarId) & "') AND (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Player_Id = " & _
                                " '" & _DB.CleanString(Student) & "') AND (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.TotalSubject = 1)  "
            If TeacherId <> "" Then
                sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
            End If
            dt = _DB.getdata(sql)
            Return dt

        End Function

        'Function หา dt เพื่อเอาไปสร้างกราฟ Usercontrol CompareChartControl
        Public Function GetDtQuizForCreateChart(ByVal StudentId As String, ByVal CalendarId As String, ByVal GroupsubjectId As String, Optional ByVal TeacherId As String = "")

            Dim sql As String = ""
            Dim dt As New DataTable
            If GroupsubjectId = "" Then 'หลายวิชา
                sql = " SELECT Quiz_Id FROM dbo.uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz WHERE TotalSubject > 1 AND " & _
                      " Player_Id = '" & _DB.CleanString(StudentId) & "' AND Calendar_Id = '" & _DB.CleanString(CalendarId) & "' "
                If TeacherId <> "" Then
                    sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
                End If
                dt = _DB.getdata(sql)
            Else 'หาเฉพาะวิชานั้นๆ
                sql = " SELECT DISTINCT uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Quiz_Id FROM tblGroupSubject INNER JOIN " & _
                      " tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz INNER JOIN " & _
                      " tblQuiz ON uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN tblTestSet " & _
                      " ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                      " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                      " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                      " WHERE (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.TotalSubject = 1) AND " & _
                      " (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Player_Id = '" & _DB.CleanString(StudentId) & "') AND " & _
                      " (uvw_CountSubjectInTermByStudentIdAndTeacherIdQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') " & _
                      " AND (tblGroupSubject.GroupSubject_Id = '" & _DB.CleanString(GroupsubjectId) & "') "
                If TeacherId <> "" Then
                    sql &= " AND Assistant_id = '" & _DB.CleanString(TeacherId) & "' "
                End If
                dt = _DB.getdata(sql)
            End If

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
                dt.Rows.Add(index)("Testset_Name") = "ทำควิซ " & (index + 1)
                Dim RandomNumber As New Random()
                dt.Rows(index)("StudentScore") = RandomNumber.Next(0, 50)
                dt.Rows(index)("ClassScore") = RandomNumber.Next(0, 50)
                dt.Rows(index)("RoomScore") = RandomNumber.Next(0, 50)
            Next
            Return dt

        End Function

        Private Function WHERE(ByVal p1 As Object) As String
            Throw New NotImplementedException
        End Function

        Public Function CheckQuizIsSoundLab(ByVal Quizid As String) As Boolean
            Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & Quizid.ToString() & "' and TabletLab_Id is not null "
            Dim CheckCount As String = _DB.ExecuteScalar(sql)
            If CInt(CheckCount) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class






End Namespace





