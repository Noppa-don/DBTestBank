Imports System.Data.SqlClient
Imports System.Text
Imports System.Web

Namespace Service

    Public Class ClsPracticeMode
        Dim _DB As ClsConnect
        Dim Activity As New Service.ClsActivity(New ClassConnectSql)

        Private IsMaxOnet As Boolean

        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB

            ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
            If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
                KNConfigData.LoadData()
            End If
            IsMaxOnet = ClsKNSession.IsMaxONet
        End Sub

        Public Function GetLimitExamAmount(TokenId As String) As String
            Dim sql As String
            sql = "select KCU_LimitExamAmount from maxonet_tblKeyCodeUsage where KCU_Token = '" & TokenId & "'"
            Dim LimitAmount As String = _DB.ExecuteScalar(sql)
            Return LimitAmount
        End Function

        Public Function SaveQuizDetail(ByVal Testset_Id As String, ByVal SchoolCode As String,
                                       ByVal Player_Id As String, ByVal IsUseTablet As String, ByVal IsHomework As Boolean,
                                       ByVal StudentAmount As String, ByVal Calendar_Id As String, ByVal Mode As String,
                                       Optional ByRef InputConn As SqlConnection = Nothing, Optional ByRef DeviceId As String = "",
                                       Optional TokenId As String = "") As String

            Dim sql As String
            Dim IsRandomQuestion As String
            Dim IsRandomAnswer As String
            Dim IsHomeworkmode As String
            Dim IsPracticemode As String
            Dim showscore As String
            Dim showCorrect As String
            Dim NeedCorrectAnswer As String
            'Dim EditHomework As String
            Dim EnabledTools As Integer = 0 ' tools ทีใช้
            Dim ShowScoreAfter As String

            Dim ClassName As String = "NULL"
            Dim RoomName As String = "NULL"

            If IsHomework Then
                IsRandomQuestion = 0
                IsRandomAnswer = 0
                IsHomeworkmode = 1
                IsPracticemode = 0
                showCorrect = 0

                showscore = 0
                ShowScoreAfter = 0
                NeedCorrectAnswer = 0

            Else
                'ถ้าเป็นฝึกฝน ให้ดูด้วยว่าชุดนี้สามารถ Random คำถาม-คำตอบ ได้หรือไม่ InSertให้ถูกต้อง
                sql = "select sum(cast (tblQuestionset.QSet_IsRandomQuestion as tinyint)) as RandomQuestion,sum(cast (tblQuestionset.QSet_IsRandomAnswer as tinyint)) as RandomAnswer "
                sql &= " from tbltestset inner join tblTestSetQuestionSet on tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id "
                sql &= " inner join tblQuestionset on tblTestSetQuestionSet.QSet_Id = tblQuestionset.QSet_Id "
                sql &= " where tbltestset.TestSet_Id = '" & Testset_Id & "'"

                Dim dtRandom As DataTable
                dtRandom = _DB.getdata(sql)
                If dtRandom.Rows.Count <> 0 Then
                    If dtRandom.Rows(0)("RandomQuestion") <> "0" Then
                        IsRandomQuestion = 1
                    Else
                        IsRandomQuestion = 0
                    End If
                    If dtRandom.Rows(0)("RandomAnswer") <> "0" Then
                        IsRandomAnswer = 1
                    Else
                        IsRandomAnswer = 0
                    End If
                Else
                    IsRandomQuestion = 1
                    IsRandomAnswer = 1
                End If

                IsHomeworkmode = 0
                IsPracticemode = 1

                If Mode = "6" Then
                    showCorrect = 0
                Else
                    showCorrect = 1
                End If
                If IsMaxOnet Then
                    showscore = 0
                    ShowScoreAfter = 0
                Else
                    showscore = 1
                    ShowScoreAfter = 1
                End If


                If Mode = "6" Then
                    NeedCorrectAnswer = 0
                Else
                    NeedCorrectAnswer = 1
                End If


                ' ใส่ชั้นห้องด้วย ถ้าเด็กทำฝึกฝน
                Dim sqlGetClassRoom As String = " SELECT Student_CurrentClass,Student_CurrentRoom FROM t360_tblStudent WHERE Student_Id = '" & Player_Id & "' AND Student_IsActive = 1 AND School_Code = '" & SchoolCode & "';"
                Dim dtClassRoom As DataTable = _DB.getdata(sqlGetClassRoom, , InputConn)
                If Not dtClassRoom.Rows.Count = 0 Then
                    ClassName = dtClassRoom.Rows(0)("Student_CurrentClass").ToString()
                    RoomName = dtClassRoom.Rows(0)("Student_CurrentRoom").ToString()
                End If
            End If

            EnabledTools = getSubjectInTestsetAndSetTools(Testset_Id, InputConn) ' tools ที่มีให้ ตาม testset

            Dim Quiz_Id As String = ""

            'If IsHomework Then
            '    sql = "select quiz_id from tblQuiz where TestSet_Id = '" & Testset_Id & "' and User_Id = '" & Player_Id & "' "
            '    Quiz_Id = _DB.ExecuteScalar(sql)
            'End If

            'If Quiz_Id = "" Then




            sql = "select newid() as Quiz_Id"
            Quiz_Id = _DB.ExecuteScalar(sql, InputConn).ToString()

            'Dim AcademicYear As String = GetAcademicYear()
            'AcademicYear = AcademicYear.Substring(2)

            Dim TabletLab_Id As String = ChkAndGetTabletLab(DeviceId).ToString


            sql = "  insert into tblquiz (Quiz_Id, TestSet_Id, StudentAmount, StartTime, "
            sql &= " needTimer, IsPerQuestionMode,TimePerQuestion, TimePerTotal, NeedCorrectAnswer, IsTimeShowCorrectAnswer, "
            sql &= " TimePerCorrectAnswer, IsShowCorrectAfterComplete, IsRushMode,needRandomQuestion, IsPracticeMode, NeedRandomAnswer, NeedShowScore, "
            sql &= " NeedShowScoreAfterComplete, IsDifferentQuestion, IsDifferentAnswer, Selfpace,IsActive, LastUpdate, User_IdOld, t360_SchoolCode, "
            sql &= " t360_TeacherId, IsUseTablet, User_Id,IsHomeworkMode,EnabledTools,Calendar_Id,FullScore,t360_ClassName,t360_RoomName,TabletLab_Id) "
            sql &= " values ('" & Quiz_Id & "','" & Testset_Id & "','" & StudentAmount & "',dbo.GetThaiDate(),"
            sql &= " 0,0,'1500','50'," & NeedCorrectAnswer & ",0,"
            sql &= " '30'," & showCorrect & ",0," & IsRandomQuestion & "," & IsPracticemode & "," & IsRandomAnswer & "," & showscore & ","
            sql &= ShowScoreAfter & ",0,0,1,1,dbo.GetThaiDate(),"
            sql &= " '1','" & SchoolCode & "','" & SchoolCode & "','" & IsUseTablet & "','" & Player_Id & "','" & IsHomeworkmode & "','" & EnabledTools & "','" & Calendar_Id & "','"

            If TabletLab_Id = "" Then
                sql &= GetFullScore(Testset_Id, InputConn) & "',N'" & ClassName & "',N'" & RoomName & "',null);"
            Else
                sql &= GetFullScore(Testset_Id, InputConn) & "',N'" & ClassName & "',N'" & RoomName & "','" & TabletLab_Id & "');"
            End If


            _DB.Execute(sql, InputConn)


            'EditHomework = False

            'Else
            'EditHomework = True
            'sql = "update tblQuizSession set LastUpdate = dbo.GetThaiDate() where Quiz_Id = '" & Quiz_Id & "'"
            '_DB.Execute(sql)
            'sql = "update tblQuiz set LastUpdate = dbo.GetThaiDate() where Quiz_Id = '" & Quiz_Id & "'"
            '_DB.Execute(sql)
            'End If

            Activity.getQsetInQuiz(Testset_Id, Quiz_Id, SchoolCode, InputConn, IsMaxOnet, TokenId) 'Insert QuizQuestion



            If Not IsHomework Then
                Dim DVID As String
                Dim IsUseComputer As Boolean
                If DeviceId <> "" Then
                    DVID = DeviceId
                Else
                    If Mode = "3" Then
                        sql = " select Tablet_SerialNumber from t360_tblTablet where Tablet_Id in (select Tablet_Id from t360_tblTabletOwner "
                        sql &= " where Owner_Id = '" & Player_Id & "');"

                        DVID = _DB.ExecuteScalar(sql, InputConn)
                        IsUseComputer = False
                    Else
                        DVID = ""
                        IsUseComputer = True
                    End If
                End If


                SaveQuiznswer(Quiz_Id, Player_Id, SchoolCode, DVID, IsUseComputer, InputConn)

                Dim QsetId As String = GetFirstQset(Quiz_Id, InputConn).ToString

                Dim dtTabletSpare As DataTable = New ClassConnectSql().getdata(String.Format("SELECT * FROM t360_tblTablet WHERE Tablet_SerialNumber = '{0}' and Tablet_IsOwner = '0';", DeviceId))

                If TabletLab_Id <> "" OrElse dtTabletSpare.Rows.Count > 0 Then
                    Activity.InsertQuizScorePracticeMode(Quiz_Id, SchoolCode, 1, True, GetQSetTypeFromQSetId(QsetId))
                End If

            End If



            SaveQuizDetail = Quiz_Id

        End Function

        Private Function ChkAndGetTabletLab(ByVal DeviceId As String) As String

            Dim sql As String
            sql = " select tabletlab_id from tbltabletlabdesk inner join t360_tbltablet on tbltabletlabdesk.tablet_Id = t360_tbltablet.tablet_id"
            sql &= " where t360_tbltablet.tablet_serialNumber = '" & DeviceId & "' AND IsActive = 1; "

            Dim TabletLab_Id As String = _DB.ExecuteScalar(sql).ToString

            Return TabletLab_Id

        End Function

        Public Sub InSertSessionHomeworkForUser(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String, ByVal Device_Id As String)

            SaveQuiznswer(Quiz_Id, Player_Id, SchoolCode, Device_Id, False)

            Dim QsetId As String = GetFirstQset(Quiz_Id).ToString

            Activity.InsertQuizScorePracticeMode(Quiz_Id, SchoolCode, 1, False, GetQSetTypeFromQSetId(QsetId))

        End Sub


        Public Function GetFullScore(ByVal TestsetId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

            'Dim sql As String
            'sql = " select sum(answer_score) as FullScore from tblAnswer where Question_Id in (select Question_Id "
            'sql &= " from tblTestSetQuestionDetail where TSQS_Id in(select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & TestsetId & "'))"
            'GetFullScore = _DB.ExecuteScalar(sql)

            'comment ไว้ แก้ปัญหาเฉพาะหน้าให้คิดแค่คะแนนของ Type 1 ก่อน
            'Dim sql As New StringBuilder
            'sql.Append("SELECT tqs.QSet_Id,qs.QSet_Type,a.Answer_Score FROM tblTestSetQuestionSet tqs ")
            'sql.Append("INNER JOIN tblTestSetQuestionDetail tqd ON tqs.TSQS_Id = tqd.TSQS_Id ")
            'sql.Append("INNER JOIN tblQuestionset qs ON qs.QSet_Id = tqs.QSet_Id ")
            'sql.Append("INNER JOIN tblAnswer a ON a.Question_Id = tqd.Question_Id ")
            'sql.Append("WHERE tqs.IsActive = 1 AND  tqs.TestSet_Id = '" & TestsetId & "' ORDER BY qs.QSet_Type;")

            'Dim dt As DataTable = _DB.getdata(sql.ToString())

            'Dim scoretype1 As Double = dt.AsEnumerable().Where(Function(r) r.Field(Of Int16)("QSet_Type") = 1).Sum(Function(x) x.Field(Of Double)("Answer_Score"))
            'Dim scoretype2 As Double = dt.AsEnumerable().Where(Function(r) r.Field(Of Int16)("QSet_Type") = 2).Sum(Function(x) x.Field(Of Double)("Answer_Score"))
            'Dim scoretype3 As Double = (dt.AsEnumerable().Where(Function(r) r.Field(Of Int16)("QSet_Type") = 3).GroupBy(Function(r) r.Field(Of Guid)("QSet_Id"))).Count()

            'Return (scoretype1 + scoretype2 + scoretype3)

            Dim sql As String = ""
            sql = " select distinct q.Question_Id,a.Answer_Id from tblTestset t 
                        inner join tblTestSetQuestionSet tsqs on t.TestSet_Id = tsqs.TestSet_Id
                        inner join tblTestSetQuestionDetail tsqd on tsqs.TSQS_Id = tsqd.TSQS_Id
                        inner join tblQuestion q on tsqd.question_id = q.question_Id
                        inner join tblAnswer a on q.Question_Id = a.Question_Id
                        where t.TestSet_Id = '" & TestsetId & "'
                        and q.IsActive = 1 and a.IsActive = 1 and a.Answer_Score = 1
                        order by q.Question_Id,a.Answer_Id"
            Dim dt As DataTable = _DB.getdata(sql.ToString())

            Return dt.Rows.Count

        End Function

        Public Function GetUseTablet(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

            Dim sql As String
            sql = "select IsUseTablet from tblQuiz where Quiz_Id = '" & Quiz_Id & "'"
            Dim IsUse As String = _DB.ExecuteScalar(sql, InputConn)

            If IsUse = "1" Then
                GetUseTablet = "True"
            Else
                GetUseTablet = "False"
            End If

        End Function

        Public Function GetQSetTypeFromQSetId(ByVal Qset_Id As String) As String
            Dim sql As String
            sql = "select Qset_Type from tblQuestionSet where Qset_Id = '" & Qset_Id & "'; "

            GetQSetTypeFromQSetId = _DB.ExecuteScalar(sql)

        End Function

        Private Function GetFirstQset(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

            Dim sql As String
            sql = "select Qset_id from tblQuestion where Question_Id in"
            sql &= " (select top 1 Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "')"

            GetFirstQset = _DB.ExecuteScalar(sql)

        End Function

        Public Sub SaveQuiznswer(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String, ByVal DeviceId As String, ByVal IsUseComputer As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)

            ''------------insert QuizAnswer-------------
            Dim sql As String

            sql = " SELECT   tblQuestion.QSet_Id, Qset_type FROM tblQuestion INNER JOIN"
            sql &= " tblQuizQuestion ON tblQuestion.Question_Id = tblQuizQuestion.Question_Id INNER JOIN"
            sql &= " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id"
            sql &= " where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1'"

            Dim dt As New DataTable
            dt = _DB.getdata(sql, , InputConn)

            Dim ExamType As String = dt.Rows(0)("Qset_type").ToString
            Dim ExamId As String = dt.Rows(0)("QSet_Id").ToString
            If ExamType = 6 Then
                sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "'"
            ElseIf ExamType = 3 Then

                sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "' ORDER BY Question_id;"
            Else 'type 1 2
                'หา QuestionId ก่อน
                sql = " select Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1' "
                Dim CurrentQuestionId As String = _DB.ExecuteScalar(sql, InputConn)
                If CurrentQuestionId <> "" Then
                    sql = " select question_id,Answer_Id from tblAnswer "
                    sql &= " where Question_Id = '" & CurrentQuestionId & "' "
                    'ต้องเช็คก่อนว่า คำถามข้อนี้ ยอมให้สลับคำตอบหรือเปล่า
                    Dim Qset_isRandomAnswer = getQSet_IsRandomAnswer(CurrentQuestionId, False, InputConn)
                    Dim IsAllowedShuffleAnswer As Boolean = GetIsAllowedShuffleAnswer(CurrentQuestionId, False, InputConn)
                    If Qset_isRandomAnswer = True AndAlso IsAllowedShuffleAnswer = True Then
                        sql &= " ORDER BY AlwaysShowInLastRow,NEWID(); "
                    Else
                        sql &= " ORDER BY Answer_No; "
                    End If
                End If
            End If

            Dim dtQuestionAndAns As DataTable = _DB.getdata(sql, , InputConn)

            If ExamType = 3 Then
                Dim dtRandomAnswer As DataTable
                dtRandomAnswer = dtQuestionAndAns.Copy()

                Dim random As New Random()
                Dim r As Integer


                For Each row In dtQuestionAndAns.Rows
                    r = random.Next(0, dtRandomAnswer.Rows.Count)
                    row("Answer_id") = dtRandomAnswer.Rows(r)("Answer_id")
                    dtRandomAnswer.Rows.RemoveAt(r)
                Next

            End If

            If dtQuestionAndAns.Rows.Count > 0 Then
                For i = 0 To dtQuestionAndAns.Rows.Count - 1
                    sql = "insert into tblQuizAnswer (QuizAnswer_Id,Quiz_Id,Question_Id,Answer_Id,QA_No,IsActive,LastUpdate,School_Code,Player_Id) "
                    sql &= " values (newid(),'" & Quiz_Id & "','" & dtQuestionAndAns.Rows(i)("Question_id").ToString & "',"
                    sql &= " '" & dtQuestionAndAns.Rows(i)("Answer_id").ToString & "','" & i + 1 & "','1',dbo.GetThaiDate(),'" & SchoolCode & "','" & Player_Id & "')"

                    _DB.Execute(sql, InputConn)
                Next
            End If




            '------------insert QuizSession-------------
            Dim tabId As String
            Dim Player_Type As String
            If IsUseComputer Then
                'tabId = "8B8F168D-3D82-447F-8BF1-E3A05C498AC3"
                tabId = "null"
                Player_Type = "1"

            Else
                tabId = "'" & GetTabId(DeviceId, InputConn).ToString & "'"
                Player_Type = "2"
            End If

            sql = "insert into tblQuizSession (QuizSession_Id, School_Code, Quiz_Id, Player_Type,LastUpdate, Player_Id, Tablet_Id, IsActive)"
            sql &= " values (NEWID(),'" & SchoolCode & "','" & Quiz_Id.ToString & "','" & Player_Type & "',dbo.GetThaiDate(),'" & Player_Id.ToString & "'," & tabId.ToString & ",'1')"
            _DB.Execute(sql, InputConn)

        End Sub

        Private Function getSubjectInTestsetAndSetTools(ByVal Testset_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)
            Dim EnabledTools As Integer = 16
            Dim db As New ClassConnectSql()
            Dim sqlSubject As String = " SELECT DISTINCT tgs.GroupSubject_Name FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionSet tqs "
            sqlSubject &= " ON tsqs.QSet_Id = tqs.QSet_Id INNER JOIN tblQuestionCategory tqc "
            sqlSubject &= "ON tqs.QCategory_Id = tqc.QCategory_Id INNER JOIN tblBook tb "
            sqlSubject &= "ON tqc.Book_Id = tb.BookGroup_Id INNER JOIN tblGroupSubject tgs "
            sqlSubject &= "ON tb.GroupSubject_Id = tgs.GroupSubject_Id "
            sqlSubject &= "WHERE tsqs.TestSet_Id = '" & Testset_Id & "' ;"

            Dim dtSubject As DataTable = db.getdata(sqlSubject, , InputConn)

            For Each subject As DataRow In dtSubject.Rows()
                Select Case subject.Item("GroupSubject_Name").ToString()
                    Case "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                        EnabledTools += 12
                    Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                        EnabledTools += 34
                End Select
            Next

            Return EnabledTools
        End Function


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


        Public Function GetTabId(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
            Dim sql As String = "select tablet_id from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "'  and Tablet_IsActive = 1 "
            Dim Tablet_Id As String = _DB.ExecuteScalar(sql, InputConn)

            Return Tablet_Id
        End Function


        'Public Function GetFirstQsetTypeFromQuizId(ByVal Quiz_Id As String) As String
        '    Dim sql As String
        '    sql = "Select tblQuestionSet.QSet_Type"
        '    sql &= " FROM tblQuestion INNER JOIN"
        '    sql &= " tblQuizQuestion ON tblQuestion.Question_Id = tblQuizQuestion.Question_Id INNER JOIN"
        '    sql &= " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id "
        '    sql &= " where Quiz_Id  = '" & Quiz_Id & "' and QQ_No = '1'"

        'End Function

        Public Function GetPlayerTypeFromTestset(ByVal Player_Id As String) As DataTable
            Dim sql As String

            sql = " SELECT t360_tblTabletOwner.Owner_Type, t360_tblTablet.Tablet_SerialNumber"
            sql &= " FROM t360_tblTablet INNER JOIN"
            sql &= " t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id"
            sql &= " where Owner_Id = '" & Player_Id & "' AND t360_tblTabletOwner.TabletOwner_IsActive = 1 ;"

            GetPlayerTypeFromTestset = _DB.getdata(sql)

        End Function

        Public Function HaveReccommend(ByVal Quiz_Id)
            Dim sql As String
            sql = " select COUNT(Question_Id) as WrongQuestionAmount from ("
            sql &= " select Question_Id from tblQuizScore where quiz_id = '" & Quiz_Id & "' and score = 0"
            sql &= " union"
            sql &= " select question_id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and Question_Id not in ("
            sql &= " select Question_Id from tblQuizScore where Quiz_Id = '" & Quiz_Id & "')) as WrongQuestion "

            Dim WrongAmount As String = _DB.ExecuteScalar(sql)

            If WrongAmount = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function GetEIName(ByVal Quiz_ID As String)
            'สำหรับสร้าง div แสดงทักษะที่ต้องปรับปรุง
            Dim ArrEIName As New ArrayList
            Dim sql As New StringBuilder

            sql.Append(" select count(WrongQuestion.question_id)as WrongQuestionOfEI,qe.ei_id,en.EI_Code,en.EI_Name")
            sql.Append(" from (select Question_Id from tblQuizScore where quiz_id = '")
            sql.Append(Quiz_ID.ToString)
            sql.Append("' and score = 0 union select question_id from tblQuizQuestion where Quiz_Id = '")
            sql.Append(Quiz_ID.ToString)
            sql.Append("'  and Question_Id not in (select Question_Id from tblQuizScore where Quiz_Id = '")
            sql.Append(Quiz_ID.ToString)
            sql.Append("')) as WrongQuestion inner join (SELECT dbo.tblQuestionEvaluationIndexItem.* FROM dbo.tblQuestionEvaluationIndexItem")
            sql.Append(" INNER JOIN dbo.uvw_EvaluationIndex_KPA ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_KPA.EI_Id")
            sql.Append(" WHERE dbo.tblQuestionEvaluationIndexItem.IsActive = 1")
            sql.Append(" UNION SELECT dbo.tblQuestionEvaluationIndexItem.* FROM dbo.tblQuestionEvaluationIndexItem")
            sql.Append(" INNER JOIN dbo.uvw_EvaluationIndex_NewEvaIndex ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_NewEvaIndex.EI_Id")
            sql.Append(" WHERE dbo.tblQuestionEvaluationIndexItem.IsActive = 1) as qe on WrongQuestion.question_id = qe.question_id")
            sql.Append(" inner join tblEvaluationIndexNew en on qe.EI_Id = en.EI_Id group by qe.ei_id,en.EI_Name,en.EI_Code order by WrongQuestionOfEI desc")

            Dim dt As New DataTable
            dt = _DB.getdata(sql.ToString)
            If dt.Rows.Count > 0 Then
                For Each a In dt.Rows
                    If a("EI_Name") IsNot DBNull.Value Then
                        ArrEIName.Add(a("EI_Name").ToString)
                    Else
                        ArrEIName.Add(a("EI_Code").ToString)
                    End If

                Next
                Return ArrEIName
            Else
                Return ArrEIName
            End If

        End Function

        Public Function GetQsetConcerned(ByVal Quiz_Id As String)
            Dim sql As String
            sql = "SELECT tblQuestionCategory.QCategory_Name, tblQuestionset.QSet_Id, tblQuestionset.QSet_Name, "
            sql &= " count(tblQuestion.Question_Id) as questionAmount, tblBook.Book_Syllabus"
            sql &= " FROM tblQuestionset INNER JOIN"
            sql &= " tblQuestionCategory ON tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
            sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN"
            sql &= " tblQuestion ON tblQuestionset.QSet_Id = tblQuestion.QSet_Id "
            sql &= " where tblQuestionCategory.QCategory_Id in"
            sql &= " ( select distinct tblQuestionCategory.QCategory_Id from dbo.tblQuestion inner join dbo.tblquestionset  "
            sql &= " on dbo.tblQuestion.QSet_Id = dbo.tblQuestionset.QSet_Id   INNER JOIN tblQuestionCategory "
            sql &= " ON tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id "
            sql &= " where dbo.tblQuestion.Question_Id in ( "
            sql &= " select Question_Id from tblQuizScore where quiz_id = '" & Quiz_Id & "' and score = 0 union"
            sql &= " select question_id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and Question_Id not in ( select Question_Id "
            sql &= "  from tblQuizScore where Quiz_Id = '" & Quiz_Id & "')))"
            sql &= " group by tblQuestionCategory.QCategory_Name, tblQuestionset.QSet_Id, tblQuestionset.QSet_Name,tblBook.Book_Syllabus"
            sql &= " order by NEWID()"

            Dim dt As DataTable
            dt = _DB.getdata(sql)

            Return dt

        End Function



        Public Function CreateTestUnitList(ByVal dt As DataTable)

            Dim sb As New StringBuilder

            If Not dt.Rows.Count = 0 Then

                Dim QSet_Id As String, QuestionSet As String, questionAmount As String
                sb.Append("<table>")
                For Each a In dt.Rows
                    QSet_Id = a("QSet_Id").ToString()
                    Dim QSet_Name As String = a("QSet_Name").ToString()
                    Dim QCategory_Name As String = a("QCategory_Name").ToString()

                    'Dim GroupSubjectId As String = ds.Tables(0).Rows(i)("GroupSubject_Id").ToString()
                    'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)
                    questionAmount = a("questionAmount").ToString()

                    'Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId").ToString, qSetId)
                    Dim book_Syllabus As String = a("book_Syllabus").ToString()

                    'sb.Append("<table><tr id=" & QSet_Id & " ><td>")
                    sb.Append("<tr id=" & QSet_Id & " Style="" cursor: pointer;"" ><td>")

                    If ClsKNSession.IsMaxONet Then
                        sb.Append(" <img id=""play_" & QSet_Id & """ src=""../Images/Homework/btnPlay.png"" class=""imgPlayQuiz"" />")
                    Else
                        sb.Append(" <img id=""play_" & QSet_Id & """ src=""../Images/Activity/PLAY.png"" class=""imgPlayQuiz"" />")
                    End If

                    sb.Append(" <img id = ""User_" & QSet_Id & """ src=""../Images/Homework/EverMade.png""class=""UserImage"" />")
                    'sb.Append("<label>[ " & book_Syllabus & " ]</label>")
                    'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
                    Dim PositionCategory As Integer
                    Dim QuestionAfTerLine As String
                    Dim index As Integer = QSet_Name.IndexOf("</b> - ")
                    PositionCategory = InStr(QSet_Name, "</b> - ")

                    'ไหมเพิ่ม If นี้มา เพราะ เจอ QuestionSet_Name ที่สั้นกว่า 7 ตัวอักษรแล้วพัง
                    If QSet_Name.Length > 7 Then
                        QuestionAfTerLine = QSet_Name.Substring((PositionCategory + 7))
                    Else
                        QuestionAfTerLine = QSet_Name
                    End If


                    If QuestionAfTerLine.Length > 75 Then
                        Dim Strcut As String = CutStringAndReturn50Alphabet(QSet_Id)
                        sb.Append(Strcut)
                    Else
                        sb.Append("<b>" & QCategory_Name & " - </b>" & QSet_Name)
                    End If

                    'Dim IsHaveHomeWork As String = ""
                    'If HttpContext.Current.Application("NeedHomeWork") = True Then
                    '    IsHaveHomeWork = "<img style='width:80px;height:45px;margin-left:35px;cursor:pointer;' src='../Images/HomeWork/homework_0.jpg' onclick=""GoToHomeWork('" & qSetId & "','" & _DB.CleanString(QsetName) & "')"" />"
                    'Else
                    '    IsHaveHomeWork = ""
                    'End If

                    QSet_Name = QSet_Name.Replace("""", "&quot;")

                    'Dim DecodeQsetName As String = Server.UrlDecode(QsetName)
                    'Dim EditBtn As String = "<img title='แก้ไขชื่อชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/freehand.png' onclick=""EditQsetName('" & qSetId & "',escape('" & QsetName & "'))"" />"
                    Dim EditBtn As String = ""
                    Dim DeleteBtn As String = ""

                    'If HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = True Then
                    '    EditBtn = "<img title='แก้ไขชื่อชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/freehand.png' onclick=""EditQsetName('" & qSetId & "','" & QsetName & "')"" />"
                    'Else
                    '    EditBtn = ""
                    'End If

                    'If HttpContext.Current.Application("NeedDeleteQcatAndQset") = True Then
                    '    DeleteBtn = "<img style='margin-left:45px;cursor:pointer;' onclick=""DeleteQcatOrQset('" & qSetId & "','" & QsetName & "','" & QCatName & "')"" src='../Images/Delete-icon.png' />"
                    'Else
                    '    DeleteBtn = ""
                    'End If

                    'If ExamAmount.Equals("0") Then
                    'sb.Append("<br />  <label id=""spnTotal_" & QSet_Id & """> มีทั้งหมด " & questionAmount & "</label> ข้อ</td></tr></table>")


#If ShowQsetTypeName = "1" Then
                    'sb.Append("<br />  <label id=""spnTotal_" & QSet_Id & """> มีทั้งหมด " & questionAmount & "</label> ข้อ<span style=""margin-left: 30px;color: red;"">" & GetQsetTypeName(a("QSet_Type")) & "</span></td></tr>")
                    sb.Append("<br />  <label id=""spnTotal_" & QSet_Id & """> มีทั้งหมด " & questionAmount & "</label> ข้อ</td></tr>")
#Else
                    sb.Append("<br />  <label id=""spnTotal_" & QSet_Id & """> มีทั้งหมด " & questionAmount & "</label> ข้อ</td></tr>")
#End If



                    'Else
                    'sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&classId=" & classId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a>" & EditBtn & DeleteBtn & "</td></tr>")
                    'End If

                    ' sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                    'sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                    '<a href=""#"" onClick=""select('" & ModID & "');"">(เลือก <span class=""spnSelectedQuestions"" id=""spnSelected_" & ModID & """>0</span> จาก <span id=""spnTotal_" & ModID & """>" & numberOfQuestions & "</span> ข้อ)</a></tr>")

                Next
                sb.Append("</table>")
            End If
            Return sb.ToString()


            '<tr><td><input type="checkbox" name="test[]" value="" id="thai1.1"><label for="thai1.1">หน่วยที่ 1 : ตอนที่ 1 สระเสียงต่ำ</label> <a href="#" onClick="selece ();">(เลือก 15 จาก 15 ข้อ)</a></tr>
            '                  <tr><td><input type="checkbox" name="test[]" value="" id="thai1.2"><label for="thai1.2">หน่วยที่ 2 : ตอนที่ 2 สระเสียงต่ำ</label> <a href="#" onClick="selece ();"> (เลือก 0 จาก 20 ข้อ)</tr>
        End Function


        Private Function GetQsetTypeName(ByVal QsetType As Integer) As String
            Select Case QsetType
                Case 1
                    Return "ช๊อยส์"
                Case 2
                    Return "ถูกผิด"
                Case 3
                    Return "จับคู่"
                Case 6
                    Return "ถูกผิด"
            End Select
            Return "ไม่ระบุ"
        End Function

        Private Function CutStringAndReturn50Alphabet(ByVal QSetId) As String
            Dim clsData As New ClassConnectSql
            Dim dtQuestionSet As New DataTable

            Dim sqlQuestionSet As String = "Select qs.QSet_Name,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc Where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id = '" & QSetId & "'"
            'dtQuestionSet = New DataTable
            dtQuestionSet = clsData.getdata(sqlQuestionSet)

            Dim QCategoryName As String = dtQuestionSet.Rows(0)("QCategory_Name")
            Dim QuestionSetName As String = dtQuestionSet.Rows(0)("QSet_Name")

            Dim CheckBrOld As Boolean = QuestionSetName.Contains("<br>")
            Dim CheckBrNew As Boolean = QuestionSetName.Contains("<br />")

            Dim strFormatReturn As String = "<b>{0}</b> - {1}<span class='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
            'If IsMaxOnet Then
            '    Return String.Format("<b>{0}</b> - {1}", QCategoryName, QuestionSetName)
            'End If
            If QuestionSetName.ToString.Length > 50 Then
                If CheckBrNew = False And CheckBrOld = False Then  ' ไม่มี <br> และ <br />                
                    Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
                ElseIf CheckBrNew = False And CheckBrOld = True Then ' มี <br>
                    If QuestionSetName.IndexOf("<br>") < 50 Then
                        QuestionSetName = QuestionSetName.Replace("<br>", "&nbsp;")
                    End If
                    Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
                ElseIf CheckBrNew = True And CheckBrOld = False Then 'มี <br />
                    If QuestionSetName.IndexOf("<br />") < 50 Then
                        QuestionSetName = QuestionSetName.Replace("<br />", "&nbsp;")
                    End If
                    Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
                Else ' มี <br> และ <br />
                    QuestionSetName = QuestionSetName.Replace("<br>", "&nbsp;").Replace("<br />", "&nbsp;")
                    Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
                End If
            End If
            Return String.Format(strFormatReturn, QCategoryName, QuestionSetName)
        End Function

        'Private Function CutString(ByVal QSetId As String) As String
        '    Dim clsData As New ClassConnectSql
        '    Dim dtQuestionSet As New DataTable

        '    Dim QCategoryName As String
        '    Dim sqlQuestionSet As String = "Select qs.QSet_Name,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc Where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id = '" & QSetId & "'"
        '    dtQuestionSet = New DataTable
        '    dtQuestionSet = clsData.getdata(sqlQuestionSet)

        '    Dim QuestionSetName As String = dtQuestionSet.Rows(0)("QSet_Name")

        '    QCategoryName = dtQuestionSet.Rows(0)("QCategory_Name")
        '    Dim CompleteStr As String
        '    Dim CheckBrOld As Boolean = QuestionSetName.Contains("<br>")
        '    Dim CheckBrNew As Boolean = QuestionSetName.Contains("<br />")
        '    Dim CutQuestionSetName As String

        '    If QuestionSetName.ToString.Length > 50 Then

        '        If CheckBrNew = False And CheckBrOld = False Then
        '            CutQuestionSetName = QuestionSetName.Substring(0, 50) & "</b><span id='" & QSetId.ToString & "' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        '            CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & CutQuestionSetName
        '        Else
        '            If QuestionSetName.IndexOf("<br") > 50 Then
        '                CutQuestionSetName = QuestionSetName.Substring(0, 50) & "</b><span id='" & QSetId.ToString & "' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        '                CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & CutQuestionSetName
        '            Else

        '                Dim InstrOldBr As String
        '                Dim CutStrNewBr As String
        '                Dim CutStrOldBr As String

        '                If CheckBrOld = True Then
        '                    InstrOldBr = InStr(QuestionSetName, "<br>")
        '                    CutStrOldBr = QuestionSetName.Substring(0, InstrOldBr - 1)
        '                Else
        '                    CutStrOldBr = QuestionSetName
        '                End If

        '                If CheckBrNew = True Then
        '                    Dim InstrNewBr As String = InStr(CutStrOldBr, "<br />")
        '                    If InstrNewBr <> 0 Then
        '                        CutStrNewBr = QuestionSetName.Substring(0, InstrNewBr - 1) & "</b><span id='" & QSetId.ToString & "' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        '                    Else
        '                        CutStrNewBr = CutStrOldBr & "</b><span id='" & QSetId.ToString & "' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        '                    End If
        '                Else
        '                    CutStrNewBr = CutStrOldBr & "</b><span id='" & QSetId.ToString & "' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        '                End If

        '                CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & CutStrNewBr

        '            End If
        '        End If

        '    End If

        '    Return CompleteStr

        'End Function

        Public Function GetQsetEI(ByVal Quiz_Id As String)

            Dim sql As New StringBuilder

            sql.Append(" select top 10 AllConcernedEi.QCategory_Name, AllConcernedEi.QSet_Id,AllConcernedEi.QSet_Name,allconcernedei.questionAmount,")
            sql.Append(" book_Syllabus from ((select a.QSet_Id,a.QSet_Name, a.QCategory_Name,a.EI_Id,a.QsetQuestionAmount,b.questionAmount,a.book_Syllabus")
            sql.Append(" from (select uvw_GetQsetWithEvalution.* from uvw_GetQsetWithEvalution inner join")
            sql.Append(" (select distinct GroupSubject_Id from uvw_GetSubjectOfAllQuiz where quiz_id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("') as tblGetSubject on tblGetSubject.GroupSubject_Id = uvw_GetQsetWithEvalution.GroupSubject_Id inner join")
            sql.Append(" (select distinct Level_Id from uvw_GetLevelOfAllQuiz where Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("')as tblGetLevel on uvw_GetQsetWithEvalution.Level_Id = tblGetLevel.Level_Id WHERE EI_Id IN (SELECT qe.EI_Id")
            sql.Append(" FROM (SELECT Question_Id FROM dbo.tblQuizScore WHERE (Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("') AND (Score = 0)")
            sql.Append(" UNION SELECT  Question_Id FROM dbo.tblQuizQuestion WHERE (Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("') AND (Question_Id NOT IN (SELECT  Question_Id FROM dbo.tblQuizScore AS tblQuizScore_1 WHERE (Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("')))) AS WrongQuestion INNER JOIN (SELECT dbo.tblQuestionEvaluationIndexItem.* FROM dbo.tblQuestionEvaluationIndexItem")
            sql.Append(" INNER JOIN dbo.uvw_EvaluationIndex_KPA ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_KPA.EI_Id")
            sql.Append(" WHERE dbo.tblQuestionEvaluationIndexItem.IsActive = 1 UNION SELECT dbo.tblQuestionEvaluationIndexItem.* FROM")
            sql.Append(" dbo.tblQuestionEvaluationIndexItem INNER JOIN dbo.uvw_EvaluationIndex_NewEvaIndex")
            sql.Append(" ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_NewEvaIndex.EI_Id")
            sql.Append(" WHERE (dbo.tblQuestionEvaluationIndexItem.IsActive = 1))as qe")
            sql.Append(" on WrongQuestion.Question_Id = qe.Question_Id))as a inner join ( SELECT QSet_Id, COUNT(Question_Id) AS questionAmount")
            sql.Append(" FROM dbo.tblQuestion WHERE (IsActive = 1) GROUP BY QSet_Id )as b on a.QSet_Id = b.QSet_Id )as AllConcernedEi")
            sql.Append(" inner join (select count(WrongQuestion.question_id)as WrongQuestionOfEI,ei_id from")
            sql.Append(" (select Question_Id from tblQuizScore where quiz_id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("' and score = 0 union select question_id from tblQuizQuestion where Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("' and Question_Id not in ( select Question_Id from tblQuizScore where Quiz_Id = '")
            sql.Append(Quiz_Id.ToString)
            sql.Append("')) as WrongQuestion inner join (SELECT dbo.tblQuestionEvaluationIndexItem.* FROM dbo.tblQuestionEvaluationIndexItem")
            sql.Append(" INNER JOIN dbo.uvw_EvaluationIndex_KPA ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_KPA.EI_Id")
            sql.Append(" WHERE dbo.tblQuestionEvaluationIndexItem.IsActive = 1 UNION SELECT dbo.tblQuestionEvaluationIndexItem.*")
            sql.Append(" FROM dbo.tblQuestionEvaluationIndexItem INNER JOIN dbo.uvw_EvaluationIndex_NewEvaIndex")
            sql.Append(" ON dbo.tblQuestionEvaluationIndexItem.EI_Id = dbo.uvw_EvaluationIndex_NewEvaIndex.EI_Id")
            sql.Append(" WHERE dbo.tblQuestionEvaluationIndexItem.IsActive = 1) as qe on WrongQuestion.question_id = qe.question_id")
            sql.Append(" group by ei_id )as WrongWithEi on WrongWithEi.EI_Id = AllConcernedEi.EI_Id) group by AllConcernedEi.QCategory_Name,")
            sql.Append(" AllConcernedEi.QSet_Id,AllConcernedEi.QSet_Name,allconcernedei.questionAmount,book_Syllabus")
            sql.Append(" order by  sum(cast (((cast (AllConcernedEi.QsetQuestionAmount as decimal(9,2)) / cast (allconcernedei.questionAmount as decimal(9,2)))")
            sql.Append(" *100)* cast (WrongWithEi.WrongQuestionOfEI as decimal(9,2))as decimal(9,0)))  desc")

            Dim dt As DataTable
            dt = _DB.getdata(sql.ToString)

            Return dt

        End Function


        '' get israndomAnswer
        Private Function getQSet_IsRandomAnswer(ByVal QuestionId As String, Optional ByVal UseTransaction As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
            If UseTransaction = False Then
                getQSet_IsRandomAnswer = _DB.ExecuteScalar(" SELECT QSet_IsRandomAnswer FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
            Else
                getQSet_IsRandomAnswer = _DB.ExecuteScalarWithTransection(" SELECT QSet_IsRandomAnswer FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
            End If
        End Function

        Private Function GetIsAllowedShuffleAnswer(QuestionId As String, UseTransaction As Boolean, InputConn As SqlConnection) As Boolean
            Dim sql As String = " SELECT NoChoiceShuffleAllowed FROM dbo.tblQuestion WHERE Question_Id = '" & QuestionId & "'; "
            Dim CheckValue As Boolean = False
            If UseTransaction = True Then
                CheckValue = CType(_DB.ExecuteScalarWithTransection(sql, InputConn), Boolean)
            Else
                CheckValue = CType(_DB.ExecuteScalar(sql, InputConn), Boolean)
            End If
            If CheckValue = False Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class


End Namespace


