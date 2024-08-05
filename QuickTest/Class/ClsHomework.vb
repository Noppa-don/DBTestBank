Imports BusinessTablet360
Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class ClsHomework
    Dim _DB As ClsConnect

    Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Sub UpdateStatusWhenEndQuiz(ByVal Quiz_Id As String, ByVal Status As String, ByVal Player_id As String)

        Dim sql As String = ""
        sql = " update tblModuleDetailCompletion set Module_Status = '" & Status & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_id & "'"
        _DB.Execute(sql)
        sql = " update tblQuizSession set SessionStatus = '" & Status & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "' and Player_Id = '" & Player_id & "'"
        _DB.Execute(sql)

    End Sub

    Public Sub UpdateExitByUser(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal Status As String)

        Dim sql As String = ""

        'Update tblModuleComplete 

        sql = " Update tblModuleDetailCompletion set module_status = '" & Status & "', TimeExitedByUser = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(),ClientId = NULL"
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "';"
        _DB.Execute(sql)
        'Update tblQuizSession 

        sql = " Update tblQuizSession set SessionStatus = '" & Status & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL "
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and Player_Id = '" & Player_Id & "';"
        _DB.Execute(sql)

        sql = "Update tblQuiz set EndTime = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "'"
        _DB.Execute(sql)

        Dim ClsActivity As New ClsActivity(New ClassConnectSql())
        Dim TestsetName As String = ClsActivity.GetTestsetName(Quiz_Id)
        Log.Record(Log.LogType.Homework, "ส่งการบ้าน """ & TestsetName & """ ", True, Player_Id)

    End Sub

    Public Function GetSendHomework(ByVal Quiz_Id As String, ByVal Player_id As String) As Boolean

        Dim sql As String

        sql = " select TimeExitedByUser from tblModuleDetailCompletion where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_id & "'"

        Dim timeExit As String = _DB.ExecuteScalar(sql).ToString

        If timeExit Is Nothing Or timeExit <> "" Then
            Return False
        Else
            Return True
        End If


    End Function

    Public Function GetDataAmount(ByVal Player_Id As String, ByVal Calendar_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String
        Dim TotalNotChecked As String
        Dim TotalNotExited As String

        sql = "SELECT COUNT(tblModuleDetailCompletion.Quiz_Id) AS TotalNotExited"
        sql &= " FROM tblModuleAssignment INNER JOIN tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id"
        sql &= " where TimeExitedByUser Is null "
        sql &= " and dbo.GetThaiDate() < end_date and dbo.GetThaiDate() > Start_Date and tblModuleDetailCompletion.IsActive = '1'"
        sql &= " and Student_Id = '" & Player_Id & "'  "
        sql &= " and Calendar_Id = '" & Calendar_Id & "';"

        TotalNotExited = _DB.ExecuteScalar(sql, InputConn)

        sql = "SELECT COUNT(tblModuleDetailCompletion.Quiz_Id) AS TotalNotChecked"
        sql &= " FROM tblModuleAssignment INNER JOIN tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id"
        sql &= " where DateChecked Is null "
        sql &= " and Calendar_Id = '" & Calendar_Id & "'"
        sql &= " and dbo.GetThaiDate() > end_date and tblModuleDetailCompletion.IsActive = '1'  "
        sql &= " and Student_Id = '" & Player_Id & "';  "
        TotalNotChecked = _DB.ExecuteScalar(sql, InputConn)

        GetDataAmount = TotalNotExited & ":" & TotalNotChecked

    End Function

    Public Function GetHomeWorkByStudentId(ByVal StudentId As String, ByVal IsValidate As Boolean, ByVal Calendar_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim dtHomework As DataTable
        'Dim dtNowHomeWork As DataTable
        Dim sql As String

        'ดูแบบตรวจแล้วหรือเปล่า
        If IsValidate = False Then

            '#If F5 = "1" Then
            '            sql = "  SELECT cast(Quiz_id as varchar(max))+'_2' as Quiz_id ,TestSet_Name,SubjectName+'#%' as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,0 as Odr"
            '#Else
            sql = "  SELECT cast(Quiz_id as varchar(max))+'_2' as Quiz_id ,TestSet_Name,SubjectName as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,0 as Odr"
            '#End If


            sql &= " FROM dbo.uvw_GetHomeWorkByStudentId WHERE Student_Id = '" & StudentId & "'"
            sql &= " and Start_Date < dbo.GetThaiDate()  and End_Date < dbo.GetThaiDate() and DateChecked is null "
            'and Calendar_Id = '" & Calendar_Id & "'"
            sql &= " union"

            '#If F5 = "1" Then
            '            sql &= " SELECT cast(Quiz_id as varchar(max))+'_0' as Quiz_id ,TestSet_Name,SubjectName+'%$' as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,1 as Odr"
            '#Else
            sql &= " SELECT cast(Quiz_id as varchar(max))+'_0' as Quiz_id ,TestSet_Name,SubjectName as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,1 as Odr"
            '#End If

            sql &= " FROM dbo.uvw_GetHomeWorkByStudentId  WHERE Student_Id = '" & StudentId & "'"
            sql &= " and Start_Date < dbo.GetThaiDate() and End_Date > dbo.GetThaiDate() and TimeExitedByUser is null and DateChecked is null "
            'and Calendar_Id = '" & Calendar_Id & "'"
            sql &= " union"

            '#If F5 = "1" Then
            '            sql &= " SELECT cast(Quiz_id as varchar(max))+'_1' as Quiz_id ,TestSet_Name,SubjectName+'%$' as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,2 as Odr"
            '#Else
            sql &= " SELECT cast(Quiz_id as varchar(max))+'_1' as Quiz_id ,TestSet_Name,SubjectName as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,2 as Odr"
            '#End If


            sql &= " FROM dbo.uvw_GetHomeWorkByStudentId  WHERE Student_Id = '" & StudentId & "'"
            sql &= " and Start_Date < dbo.GetThaiDate() and End_Date > dbo.GetThaiDate() and TimeExitedByUser is not null "
            'and Calendar_Id = '" & Calendar_Id & "'"
            sql &= " order by Odr,End_Date,TestSet_Name"
            'dtNowHomeWork = _DB.getdata(sql)
        Else
            '+'$#'
            sql = "   SELECT cast(Quiz_id as varchar(max))+'_3' as Quiz_id ,TestSet_Name,SubjectName as SubjectName,End_date,'' as ForMatDate,TotalMade,cast((totalmade*100)/FullScore  as varchar)as PercentMade,FullScore, Coalesce(Score,0)as score,3 as Odr"
            sql &= " FROM dbo.uvw_GetHomeWorkByStudentId WHERE Student_Id = '" & StudentId & "'"
            sql &= " and Start_Date < dbo.GetThaiDate()  and End_Date < dbo.GetThaiDate() and DateChecked is not null and Calendar_Id = '" & Calendar_Id & "' order by End_date"
        End If

        dtHomework = _DB.getdata(sql, , InputConn)

        If Not dtHomework.Rows.Count = 0 Then
            dtHomework = SetQuestionAmountAndIcon(dtHomework)
        End If

        Return dtHomework


    End Function

    Public Function HaveSession(ByVal User_id As String, ByVal Quiz_Id As String) As Boolean
        Dim sql As String

        sql = " select Top 1 QuizSession_Id from tblQuizSession where Quiz_Id = '" & Quiz_Id & "' and Player_Id = '" & User_id & "' and IsActive = '1'"

        Dim dtSession As String = _DB.ExecuteScalar(sql)

        If dtSession Is Nothing Or dtSession <> "" Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Function SetQuestionAmountAndIcon(ByVal dt As DataTable, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        'Dim QuestionAmount As String
        'Dim PercentageCompleteAmount As Integer
        Dim ImgCompleteRate As String = ""

        If dt.Rows(0)("Odr").ToString = "3" Then
            For Each a In dt.Rows
                a("PercentMade") = a("Score").ToString & "/" & Strings.Format(a("FullScore"), "0.##")
                a("ForMatDate") = Convert.ToDateTime(a("End_date")).ToPointPlusTime()
            Next
        Else

            For Each a In dt.Rows

                'QuestionAmount = GetQuestionAmount(a("TestSet_Id").ToString)
                'a("TotalAmount") = QuestionAmount

                If a("Odr") = "0" Then 'หมดเวลาแล้ว เห็นคะแนน

                    a("PercentMade") = a("Score").ToString & "/" & Strings.Format(a("FullScore"), "0.##")
                    a("ForMatDate") = Convert.ToDateTime(a("End_date")).ToPointPlusTime()

                ElseIf a("Odr") = "2" Then 'ส่งแล้ว ยังไม่หมดเวลา

                    a("PercentMade") = "<img src=""../Images/HomeWork/SentHomework.png"" style='float:right; cursor: pointer; 'visibility:visible; width:50px; height:50px;'/> "
                    a("ForMatDate") = Convert.ToDateTime(a("End_date")).ToPointPlusTime()

                ElseIf a("Odr") = "1" Then 'ทำอยู่ ยังไม่หมดเวลา


                    If a("PercentMade") < 80 Then
                        ImgCompleteRate = "../Images/HomeWork/neutral.png"

                    ElseIf a("PercentMade") >= 80 And a("PercentMade") < 100 Then

                        ImgCompleteRate = "../Images/HomeWork/happy.png"

                    ElseIf a("PercentMade") = 100 Then

                        ImgCompleteRate = "../Images/HomeWork/veryhappy.png"

                    End If

                    a("PercentMade") = "<img src=""" & ImgCompleteRate & """ style=""float:right; cursor: pointer; visibility:visible; width:50px; height:50px;""/> "
                    a("ForMatDate") = Convert.ToDateTime(a("End_date")).ToPointPlusTime()

                End If

            Next

        End If

        Return dt
    End Function



    Private Function GetQuestionAmount(ByVal testset_id As String) As String

        Dim sql As String
        sql = "select COUNT(tsqd_Id)as QuestionAmount from tblTestSetQuestionDetail"
        sql &= " where TSQS_Id in(select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & testset_id & "' And IsActive = '1') And IsActive = '1'"

        GetQuestionAmount = _DB.ExecuteScalar(sql)
    End Function

    Public Function SetIsChecked(ByVal Player_Id As String)

        Dim sql As String
        sql = " update MDC set DateChecked = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(),ClientId = NULL from tblModuleDetailCompletion as MDC Inner join tblModuleAssignment as MA on MDC.MA_Id = MA.MA_Id    "
        sql &= " where MDC.Student_Id = '" & Player_Id & "' and MA.End_Date < dbo.GetThaiDate()"

        _DB.Execute(sql)

    End Function

    Public Function GetIsComplete(ByVal Quiz_Id As String, ByVal Player_Id As String, AnswerState As String) As Boolean

        Dim sql As String
        sql = " select TimeExitedByUser from tblModuleDetailCompletion where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "';"
        Dim IsExit As String = _DB.ExecuteScalar(sql)

        sql = " select End_Date from tblModuleAssignment where MA_Id in "
        sql &= " (select MA_Id from tblModuleDetailCompletion where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "')"
        Dim IsEndDate As String = _DB.ExecuteScalar(sql)

        If BusinessTablet360.ClsKNSession.IsMaxONet Then
            If AnswerState = 2 Then
                GetIsComplete = True
            Else
                GetIsComplete = False
            End If
        Else
            If ((IsExit <> "" AndAlso IsEndDate <> "") AndAlso IsEndDate < Date.Now) Then
                GetIsComplete = True
            Else
                GetIsComplete = False
            End If
        End If


    End Function

    Public Function GetArrPropertyHomework(ByVal QuizId As String)

        Dim ArrProperty As New ArrayList
        Dim sql As String = " SELECT DISTINCT tblModuleAssignment.Start_Date,dbo.tblModuleAssignment.End_Date,dbo.tblModuleAssignment.MA_Id " &
                            " FROM tblModule INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " &
                            " tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " &
                            " tblModuleDetailCompletion ON tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id AND " &
                            " tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id WHERE (tblModuleDetail.Reference_Type = 0) AND (dbo.tblModuleDetailCompletion.Quiz_Id = '" & QuizId & "') "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim DurationTimetxt As String = CheckDateDuration(dt.Rows(0)("Start_Date"), dt.Rows(0)("End_Date"))
            ArrProperty.Add(DurationTimetxt)
            Dim dtAssignmentDetail As New DataTable
            sql = " SELECT Class_Name,Room_Id,Student_Id FROM dbo.tblModuleAssignmentDetail WHERE MA_Id = '" & dt.Rows(0)("MA_Id").ToString() & "' " &
                  " AND IsActive = 1 "
            dtAssignmentDetail = _DB.getdata(sql)
            If dtAssignmentDetail.Rows.Count > 0 Then
                For index = 0 To dtAssignmentDetail.Rows.Count - 1
                    If dtAssignmentDetail.Rows(index)("Class_Name") IsNot DBNull.Value Then
                        ArrProperty.Add(dtAssignmentDetail.Rows(index)("Class_Name"))
                    ElseIf dtAssignmentDetail.Rows(index)("Room_Id") IsNot DBNull.Value Then
                        sql = " SELECT (Class_Name + Room_Name) AS Roomname FROM dbo.t360_tblRoom " &
                              " WHERE Room_Id = '" & dtAssignmentDetail.Rows(index)("Room_Id").ToString() & "' AND Room_IsActive = 1 "
                        Dim RoomName As String = _DB.ExecuteScalar(sql)
                        If RoomName <> "" Then
                            ArrProperty.Add(RoomName)
                        End If
                    ElseIf dtAssignmentDetail.Rows(index)("Student_Id") IsNot DBNull.Value Then
                        sql = " SELECT (Student_FirstName + '  ' + Student_LastName) AS StudentName " &
                              " FROM dbo.t360_tblStudent WHERE Student_Id = '" & dtAssignmentDetail.Rows(index)("Student_Id").ToString() & "' AND Student_IsActive = 1 "
                        Dim StudentName As String = _DB.ExecuteScalar(sql)
                        If StudentName <> "" Then
                            ArrProperty.Add(StudentName)
                        End If
                    End If
                Next
            End If
            Return ArrProperty
        Else
            Return ArrProperty
        End If

    End Function

    Public Function CheckDateDuration(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As String

        Dim DurationTimetxt As String = ""
        Dim StartDay As String = StartDate.Day
        Dim StartMonth As String = StartDate.Month
        Dim StartYear As String = StartDate.Year
        If StartDate.Year < 2400 Then
            StartYear += 543
        End If
        Dim EndDay As String = EndDate.Day
        Dim EndMonth As String = EndDate.Month
        Dim EndYear As String = EndDate.Year
        If EndDate.Year < 2400 Then
            EndYear += 543
        End If

        If DateDiff(DateInterval.Year, StartDate, EndDate) <> 0 Then
            'คนละปีกัน
            DurationTimetxt = "สั่งให้ทำในช่วง " & StartDay & "/" & StartMonth & "/" & StartYear & " - " & EndDay & "/" & EndMonth & "/" & EndYear

        Else 'ปีเดียวกัน
            'เดือนเดียวกัน
            If DateDiff(DateInterval.Month, StartDate, EndDate) = 0 Then
                DurationTimetxt = "สั่งให้ทำในช่วง " & StartDay & " - " & EndDay & "/" & EndMonth & "/" & EndYear
            Else 'คนละเดือนกัน
                DurationTimetxt = "สั่งให้ทำในช่วง " & StartDay & "/" & StartMonth & " - " & EndDay & "/" & EndMonth & "/" & EndYear
            End If
        End If
        Return DurationTimetxt

    End Function

    Public Function SaveQuizHomework(ByVal testsetId As String) As String

        Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        ' Dim KnSession As New ClsKNSession(False)

        'HttpContext.Current.Session("Quiz_Id") = ClsPracticeMode.SaveQuizDetail(testsetId, dtPlayer.Rows(0)("ClassName").ToString, dtPlayer.Rows(0)("RoomName").ToString, _
        'dtPlayer.Rows(0)("School_Code").ToString, dtPlayer.Rows(0)("Student_Id").ToString, "1")

        Dim ClassName As String = ""
        Dim RoomName As String = ""
        Dim SchoolCode As String = HttpContext.Current.Session("SchoolCode")
        Dim player_Id = HttpContext.Current.Session("UserId")

        Dim QuizIdState As String = ""
        Dim KNS As New KNAppSession()
        Dim Calendar_Id = KNS.StoredValue("CurrentCalendarId").ToString()
        QuizIdState = ClsPracticeMode.SaveQuizDetail(testsetId, ClassName, RoomName, SchoolCode, player_Id, "1", True, Calendar_Id)
        HttpContext.Current.Session("HomeworkMode") = True

        Dim ArrQuizIdState() As String = Split(QuizIdState, ":")

        ClsActivity.getQsetInQuiz(testsetId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion
        ClsPracticeMode.SaveQuiznswer(HttpContext.Current.Session("Quiz_Id"), HttpContext.Current.Session("UserId"), SchoolCode, HttpContext.Current.Session("PDeviceId"), False)
        ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, False, ClsPracticeMode.GetFirstQsetTypeFromQuizId(HttpContext.Current.Session("Quiz_Id")))

        Return ArrQuizIdState(0)

    End Function

    Public Sub InSertSessionHomeworkForUser(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String, ByVal Device_Id As String)

        SaveQuiznswer(Quiz_Id, Player_Id, SchoolCode, Device_Id, False)

        Dim QsetId As String = GetFirstQset(Quiz_Id).ToString

        'InsertQuizScoreHomework(Quiz_Id, SchoolCode, 1, GetQSetTypeFromQSetId(QsetId), Player_Id)

    End Sub

    Public Function GetQSetTypeFromQSetId(ByVal Qset_Id As String) As String
        Dim sql As String
        sql = "select Qset_Type from tblQuestionSet where Qset_Id = '" & Qset_Id & "'; "

        GetQSetTypeFromQSetId = _DB.ExecuteScalar(sql)

    End Function


    Public Function InsertQuizScoreHomework(ByVal QuizId As String, ByVal SchoolCode As String, ByVal _
                                                Examnum As Integer, ByVal QsetType As String, ByVal Player_Id As String)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or Examnum = 0 Then
            Return ""
        End If

        Dim sql As String

        sql = " SELECT SR_ID from t360_tblStudentRoom where student_id = '" & Player_Id & "'"

        Dim SR_ID As String = _DB.ExecuteScalar(sql)

        If SR_ID <> "" Then
            If QsetType = "6" Then
                sql = " INSERT INTO dbo.tblQuizScore(QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id ,  Answer_Id ,ResponseAmount , "
                sql &= " FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id , SR_ID,QQ_No)  "
                sql &= " SELECT Top 1 NEWID() , '" & QuizId & "', '" & SchoolCode & "', Question_Id, NULL ,0,dbo.GetThaiDate() , "
                sql &= " dbo.GetThaiDate() , 0 ,0 ,1 , '" & Player_Id & "','" & SR_ID & "','" & Examnum & "' "
                sql &= " FROM tblQuizQuestion"
                sql &= " where (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "')"
            Else
                sql = " INSERT INTO dbo.tblQuizScore(QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id ,  Answer_Id ,ResponseAmount , "
                sql &= " FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id , SR_ID,QQ_No)  "
                sql &= " SELECT NEWID() , '" & QuizId & "', '" & SchoolCode & "', Question_Id, NULL ,0,dbo.GetThaiDate() , "
                sql &= " dbo.GetThaiDate() , 0 ,0 ,1 , '" & Player_Id & "','" & SR_ID & "','" & Examnum & "' "
                sql &= " FROM tblQuizQuestion"
                sql &= " where (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "')"
            End If

            'รอถาม Query กับพี่ชินอีกทีว่าถูกไหม
            Try
                _DB.Execute(sql) 'Insert ข้อมูลลง tblQuizScore
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return ""
            End Try

            Return "Complete"
        End If



    End Function

    Private Function GetFirstQset(ByVal Quiz_Id As String) As String

        Dim sql As String
        sql = "select Qset_id from tblQuestion where Question_Id in"
        sql &= " (select top 1 Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "')"

        GetFirstQset = _DB.ExecuteScalar(sql)

    End Function

    Public Sub SaveQuiznswer(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String,
                                 ByVal DeviceId As String, ByVal IsUseComputer As Boolean)

        '    ''------------insert QuizAnswer-------------
        '    Dim sql As String

        '    sql = " SELECT   tblQuestion.QSet_Id, Qset_type FROM tblQuestion INNER JOIN"
        '    sql &= " tblQuizQuestion ON tblQuestion.Question_Id = tblQuizQuestion.Question_Id INNER JOIN"
        '    sql &= " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id"
        '    sql &= " where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1'"

        '    Dim dt As New DataTable
        '    dt = _DB.getdata(sql)

        '    Dim ExamType As String = dt.Rows(0)("Qset_type").ToString
        '    Dim ExamId As String = dt.Rows(0)("QSet_Id").ToString
        '    'If ExamType = 6 Then
        '    '    sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "'"
        '    'ElseIf ExamType = 3 Then

        '    '    sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "'"
        '    'Else 'type 1 2
        '    sql = " select question_id,Answer_Id from tblAnswer "
        '    sql &= " where Question_Id in  (select Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1')"
        '    'End If

        '    Dim dtQuestionAndAns As DataTable = _DB.getdata(sql)

        '    For i = 0 To dtQuestionAndAns.Rows.Count - 1
        '        sql = "insert into tblQuizAnswer (QuizAnswer_Id,Quiz_Id,Question_Id,Answer_Id,QA_No,IsActive,LastUpdate) "
        '        sql &= " values (newid(),'" & Quiz_Id & "','" & dtQuestionAndAns.Rows(i)("Question_id").ToString & "',"
        '        sql &= " '" & dtQuestionAndAns.Rows(i)("Answer_id").ToString & "','" & i + 1 & "','1',dbo.GetThaiDate())"
        '        _DB.Execute(sql)
        '    Next

        SetQuizSession(Quiz_Id, Player_Id, SchoolCode, DeviceId, IsUseComputer)

    End Sub

    Public Sub SetQuizSession(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String,
                              ByVal DeviceId As String, ByVal IsUseComputer As Boolean)

        ''------------ insert QuizSession -------------
        Dim sql As String
        Dim tabId As String
        Dim Player_Type As String
        If IsUseComputer Then
            tabId = "8B8F168D-3D82-447F-8BF1-E3A05C498AC3"
            tabId = "null"
            Player_Type = "1"

        Else
            tabId = "'" & GetTabId(DeviceId).ToString & "'"
            Player_Type = "2"
        End If

        sql = "insert into tblQuizSession (QuizSession_Id, School_Code, Quiz_Id, Player_Type,LastUpdate, Player_Id, Tablet_Id, IsActive)"
        sql &= " values (NEWID(),'" & SchoolCode & "','" & Quiz_Id.ToString & "','" & Player_Type & "',dbo.GetThaiDate(),'" & Player_Id.ToString & "'," & tabId.ToString & ",'1')"
        _DB.Execute(sql)
    End Sub

    Public Function GetTabId(ByVal DeviceId As String) As String
        Dim sql As String = "select tablet_id from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "'  and Tablet_IsActive = 1 "
        Dim Tablet_Id As String = _DB.ExecuteScalar(sql)

        Return Tablet_Id
    End Function

    Public Function TestGetStatusHomework(ByVal Quiz_Id As String, ByVal Player_Id As String)

        Dim sql As String
        sql = " select COUNT(QuizQuestion_Id) as TotalAmount from tblQuizQuestion  where Quiz_Id = '" & Quiz_Id & "'"
        Dim TotalQuestion As String = _DB.ExecuteScalar(sql)

        sql = " select COUNT(quizscore_id) as MadeAmount from tblQuizScore  where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "'"
        Dim MadeQuestion As String = _DB.ExecuteScalar(sql)

        sql = " select AllExam - MadeExam As CrossExam from "
        sql &= " (select COUNT(DISTINCT(QQ_No))as AllExam  from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "')AllExam,"
        sql &= " (select COUNT(QuizScore_Id)as MadeExam from tblquizscore where Quiz_Id = '" & Quiz_Id & "' "
        sql &= " and ResponseAmount <> 0 and Student_Id = '" & Player_Id & "')MadeExam; "
        Dim CountLeapExam As String = _DB.ExecuteScalar(sql)

        If MadeQuestion > 0 Then
            If TotalQuestion = MadeQuestion Then
                If CountLeapExam = 0 Then
                    Return 3
                Else
                    Return 2
                End If

            Else
                Return 1
            End If
        Else
            Return 0
        End If

    End Function

End Class
