Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class ClsPracticeMode
    Dim _DB As ClsConnect
    Dim ClsPDF As New ClsPDF(New ClassConnectSql)
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Function CheckOneSubjectMaxOnet(StudentId As String, PClassId As String) As Boolean
        Dim sql As String
        sql = "select distinct ss.SS_SubjectId from maxonet_tblStudentSubject ss 
                inner join maxonet_tblKeyCodeUsage kcu on ss.SS_KeyCode = kcu.KeyCode_Code
                where (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null)
                and ss.SS_StudentId = '" & StudentId & "' and ss.SS_LevelId = '" & PClassId & "' and ss.ss_Isactive = 1;"

        Dim dtStudentSubjectAmount As DataTable
        dtStudentSubjectAmount = _DB.getdata(sql)

        If dtStudentSubjectAmount.Rows.Count > 1 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function CheckOneLevelMaxOnet(StudentId As String) As Boolean
        Dim sql As String

        sql = "SELECT distinct ss.SS_levelId FROM maxonet_tblStudentSubject ss inner join maxonet_tblKeyCodeUsage kcu on ss.SS_KeyCode = kcu.KeyCode_Code
                WHERE ss.SS_StudentId = '" & StudentId & "' and (kcu.KCU_ExpireDate > dbo.GetThaiDate() or kcu.KCU_ExpireDate is null) ;"
        Dim dtStudentLevelAmount As DataTable
        dtStudentLevelAmount = _DB.getdata(sql)

        If dtStudentLevelAmount.Rows.Count > 1 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function GetTestsetFromClass(ByVal Player_Id As String, ByVal IsShowFull As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        'ข้อสอบในโหมดฝึกฝนที่ใช้ได้และบอกว่าเคยเล่นหรือยัง
        'ถ้าเด็กเคยเล่นในห้องเรียนแล้ว ต้องขึ้นว่าเคยเล่นแล้วมั้ย ?
        Dim sql As String
        'If IsShowFull = True Then
        '    sql = " SELECT  tblTestSet.TestSet_Id, tblTestSet.TestSet_Name,case "
        'Else
        '    sql = " SELECT TOP 5 tblTestSet.TestSet_Id, tblTestSet.TestSet_Name,case "
        'End If
        sql = " SELECT  tblTestSet.TestSet_Id, tblTestSet.TestSet_Name,case "
        sql &= " (	SELECT count(tblQuiz.Quiz_Id) FROM tblQuiz"
        sql &= "  where(TestSet_Id = tblTestSet.TestSet_Id) AND User_Id = '" & Player_Id & "')  "
        sql &= " when '0' then 'hidden' else 'visible' End as IsPracticed"

        sql &= " FROM t360_tblClass INNER JOIN"
        sql &= " tblLevel ON t360_tblClass.Class_Order = tblLevel.[Level] INNER JOIN"
        sql &= " tblQuestionCategory INNER JOIN"
        sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN"
        sql &= " tblQuestionSet ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id INNER JOIN"
        sql &= " tblTestSet INNER JOIN"
        sql &= " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id ON tblQuestionSet.QSet_Id = tblTestSetQuestionSet.QSet_Id ON "
        sql &= " tblLevel.Level_Id = tblBook.Level_Id INNER JOIN"
        sql &= " t360_tblStudent ON t360_tblClass.Class_Name = t360_tblStudent.Student_CurrentClass"
        sql &= " WHERE  t360_tblStudent.Student_Id = '" & Player_Id & "'"
        sql &= " AND tblTestSet.IsActive = '1' "
        sql &= " AND tblTestSet.IsPracticeMode = '1'"
        sql &= " And tbltestsetQuestionSet.IsActive = '1'"
        sql &= " and tblTestSet.UserType = '1'"
        sql &= " GROUP BY tblTestSet.TestSet_Id, tblTestSet.TestSet_Name,t360_tblStudent.School_Code,tblTestSet.LastUpdate ORDER BY tblTestSet.LastUpdate DESC;"

        GetTestsetFromClass = _DB.getdata(sql, , InputConn)
        If IsShowFull = False Then
            If GetTestsetFromClass.Rows.Count > 5 Then

                For i As Integer = 5 To GetTestsetFromClass.Rows.Count - 1
                    GetTestsetFromClass.Rows(i).Delete()
                Next

                Dim newRow As DataRow = GetTestsetFromClass.NewRow()
                newRow("TestSet_Id") = Guid.Empty
                newRow("TestSet_Name") = "ดูเพิ่มเติม"
                newRow("IsPracticed") = "hidden"
                GetTestsetFromClass.Rows.Add(newRow)
            End If
        End If

        Dim a As String = ""

    End Function

    Public Function GetPlayerDetail(ByVal Tablet_Serial As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        Dim dtTablet As DataTable = GetTabId(Tablet_Serial, InputConn)

        If dtTablet.Rows.Count = 0 Then Return Nothing ' ไม่มี tabletid ของ deviceid ที่ส่งเข้ามาให้ return nothing ออกไป

        Dim Tablet_Id As String = dtTablet.Rows(0)("tablet_id").ToString
        Dim tablet_Isowner As String = dtTablet.Rows(0)("Tablet_IsOwner").ToString

        sql = "  Select s.School_Code, s.Student_CurrentClass as ClassName,s.Student_CurrentRoom as RoomName, s.Student_Id,r.Room_Id, s.IsViewAllTips,ku.KCU_LimitExamAmount
                    FROM t360_tblTabletOwner ton INNER JOIN t360_tblStudent s ON ton.Owner_Id = s.Student_Id 
                    INNER JOIN maxonet_tblKeyCodeUsage ku on ku.KCU_OwnerId = ton.Owner_Id 
                    INNER JOIN t360_tblRoom r ON s.School_Code = r.School_Code AND s.Student_CurrentClass = r.Class_Name  
                    AND REPLACE(s.Student_CurrentRoom,'/','') = REPLACE(r.Room_Name,'/','') AND s.School_Code = r.School_Code  
                    where Tablet_Id = '" & Tablet_Id & "' and TabletOwner_IsActive = '1' 
                    and ku.KCU_IsActive = 1 and (ku.KCU_ExpireDate > getdate() or ku.KCU_ExpireDate is null)
                    order by ku.KCU_LastUpdate; "

        GetPlayerDetail = _DB.getdata(sql, , InputConn)

    End Function

    Public Function GetTabId(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = "select tablet_id,Tablet_IsOwner from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "'  and Tablet_IsActive = 1 "
        Dim dtTab As DataTable = _DB.getdata(sql)

        Return dtTab
    End Function

    Public Function CheckSpareTablet(DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = "select tablet_Isowner from t360_tbltablet where Tablet_SerialNumber = '" & DeviceId & "'"
        Dim IsSpareTablet As String = _DB.ExecuteScalar(sql)
        If IsSpareTablet = "" Then
            IsSpareTablet = "True"
        End If
        Return IsSpareTablet
    End Function

    Public Function CheckLabTablet(DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = "select t360_tblTablet.School_Code,tblTabletLabDesk.TabletLab_Id from t360_tblTablet inner join tblTabletLabDesk "
        sql &= " on t360_tblTablet.Tablet_Id  = tblTabletLabDesk.Tablet_Id "
        sql &= " where t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.ToString & "'"

        Dim IsLabTablet As DataTable = _DB.getdata(sql)

        Return IsLabTablet

    End Function

    Public Function CheckTabIsSoundLab(ByVal TabletId As String) As Boolean
        Dim sql As String = " select COUNT(*) from tblTabletLabDesk where Tablet_Id = '" & TabletId.ToString() & "' and IsActive = 1 "
        Dim CountCheck As String = _DB.ExecuteScalar(sql)
        If CInt(CountCheck) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetQSetByQcategory(ByVal QCategory_Id As String, ByVal Player_Id As String, ByVal IsUseComputer As Boolean) As DataTable
        Dim sql As String

        If Not IsUseComputer Then
            'sql = " select tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name,case "
            'sql &= " (Select count(tblQuiz.Quiz_Id)"
            'sql &= " FROM tblTestSet INNER JOIN tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN"
            'sql &= " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id"
            'sql &= " where tblTestSet.UserId = '" & Player_Id.ToString & "' and tblTestSet.IsActive = '1'"
            'sql &= " and tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id)  "

            'sql &= " when '0' then 'hidden' else 'visible' End as IsPracticed "
            'sql &= " from tblQuestionSet"
            'sql &= " where QCategory_Id = '" & QCategory_Id.ToString & "'"
            'sql &= " and tblQuestionSet.IsActive = '1' "

            sql = "SELECT     tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name, tblBook.Book_Syllabus ,"
            sql &= " case  (Select count(tblQuiz.Quiz_Id) FROM tblTestSet INNER JOIN tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN tblTestSetQuestionSet "
            sql &= " ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id "
            sql &= " where tblTestSet.UserId = '" & Player_Id.ToString & "' "
            sql &= " and tblTestSet.IsActive = '1' and tbltestsetQuestionSet.IsActive = '1'"
            sql &= " and tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id)   "
            sql &= " when '0' then 'hidden' else 'visible' End as IsPracticed  "
            sql &= " FROM         tblQuestionSet INNER JOIN"
            sql &= " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
            sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id"
            sql &= " WHERE     (tblQuestionSet.QCategory_Id = '" & QCategory_Id.ToString & "') AND (tblQuestionSet.IsActive = '1')"


        Else
            sql = "SELECT     tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name, tblBook.Book_Syllabus, 'hidden' AS IsPracticed"
            sql &= " FROM         tblQuestionSet INNER JOIN"
            sql &= " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
            sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id"
            sql &= " WHERE (tblQuestionSet.QCategory_Id = '" & QCategory_Id.ToString & "')"

        End If

        Dim dt As DataTable = _DB.getdata(sql)

        For Each r In dt.Rows
            r("QSet_Name") = ClsPDF.CleanSetNameText(r("QSet_Name"))
        Next

        Return dt

    End Function

    Public Function GetQCategoryByGroupSubjectAndLevel(ByVal GroupSubjectId As String, ByVal LevelId As String, ByVal BookYear As String, Optional ByVal IsMaxOnet As Boolean = False, Optional ByVal tokenId As String = "") As DataTable

        Dim sql As String

        If IsMaxOnet Then

            Dim LimitExam As String = GetLimitExamAmount(tokenId)

            If LimitExam <> "" Then
                sql = "select distinct top 1  qc.QCategory_Name, qs.QSet_Id, qs.QSet_Name," & LimitExam & " as QuestionAmount,b.Book_Syllabus,qs.QSet_Type,  
                    ROW_NUMBER() OVER(PARTITION BY g.GroupSubject_ShortName,l.Level_ShortName ORDER BY count(q.Question_Id)) AS rk
                    from tblbook b inner join tblQuestionCategory qc on b.BookGroup_Id = qc.Book_Id
                    inner join tblQuestionset qs on qc.QCategory_Id = qs.QCategory_Id 
                    inner join tblQuestion q on qs.QSet_Id = q.QSet_Id
                    inner join tblGroupSubject g on b.GroupSubject_Id = g.GroupSubject_Id
                    inner join tblLevel l on b.Level_Id = l.Level_Id
                    where b.IsActive = 1 and qc.IsActive = 1 and qs.IsActive = 1 
                    and q.IsActive = 1 and b.Book_Syllabus = '51' and b.GroupSubject_Id in ('" & GroupSubjectId & "') and l.Level_Id in('" & LevelId & "')
                    group by qc.QCategory_Name, qs.QSet_Id, qs.QSet_Name,b.Book_Syllabus,qs.QSet_Type,g.GroupSubject_ShortName,l.Level_ShortName
                    having count(q.Question_Id) >= " & LimitExam & ""

                Return _DB.getdata(sql)

            End If
        End If
        sql = "  Select  tblQuestionCategory.QCategory_Name, tblQuestionset.QSet_Id, tblQuestionset.QSet_Name,count(tblQuestion.Question_Id) As questionAmount, tblBook.Book_Syllabus,tblQuestionset.QSet_Type "
            sql &= " FROM tblQuestionset INNER JOIN"
            sql &= " tblQuestionCategory On tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
            sql &= " tblBook On tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN"
            sql &= " tblQuestion On tblQuestionset.QSet_Id = tblQuestion.QSet_Id "
            sql &= " where tblBook.GroupSubject_Id = '" & GroupSubjectId & "'"
            sql &= " and tblBook.Level_Id = '" & LevelId & "' "

            sql &= " AND (tblQuestionCategory.QCategory_Id NOT IN"
            sql &= " (SELECT b"
            sql &= " FROM (SELECT COUNT(tblQuestionSet.QSet_Id) AS a, tblQuestionCategory_1.QCategory_Id AS b, tblQuestionCategory_1.QCategory_Name AS c, "
            sql &= " tblQuestionCategory_1.Book_Id AS d"
            sql &= " FROM  tblQuestionCategory AS tblQuestionCategory_1 LEFT OUTER JOIN"
            sql &= " tblQuestionSet ON tblQuestionCategory_1.QCategory_Id = tblQuestionSet.QCategory_Id"
            sql &= " GROUP BY tblQuestionCategory_1.QCategory_Id, tblQuestionCategory_1.QCategory_Name, tblQuestionCategory_1.Book_Id) AS tbla"
            sql &= " WHERE  (a = 0)))"


            If BookYear <> "5144" Then
                sql &= " and tblbook.Book_Syllabus = '" & BookYear & "'  "
            End If

            sql &= " and tblQuestion.IsActive = 1 and tblQuestionCategory.IsActive = 1 and tblQuestionset.IsActive = 1 and tblBook.IsActive = 1 "

            sql &= " group by tblQuestionCategory.QCategory_Name, tblQuestionset.QSet_Id, tblQuestionset.QSet_Name,tblBook.Book_Syllabus,tblQuestionset.QSet_Type "
        sql &= " order by tblQuestionCategory.QCategory_Name,tblQuestionset.QSet_Name"


        GetQCategoryByGroupSubjectAndLevel = _DB.getdata(sql)

    End Function

    Public Function GetLimitExamAmount(TokenId As String) As String
        Dim sql As String
        sql = "select KCU_LimitExamAmount from maxonet_tblKeyCodeUsage where KCU_Token = '" & TokenId & "'"
        Dim LimitAmount As String = _DB.ExecuteScalar(sql)
        Return LimitAmount
    End Function

    Public Function GetQsetAmount(ByVal Quiz_Id As String, ByVal User_Id As String) As String
        Dim sql As String

        sql = " SELECT COUNT(tblQuiz.Quiz_Id) AS QsetAmount"
        sql &= " FROM tblQuestionCategory INNER JOIN"
        sql &= " tblQuestionSet ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id INNER JOIN"
        sql &= " tblTestSetQuestionSet ON tblQuestionSet.QSet_Id = tblTestSetQuestionSet.QSet_Id INNER JOIN"

        sql &= " tblQuiz INNER JOIN"
        sql &= " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id"
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and User_Id = '" & User_Id & "' and tbltestsetQuestionSet.IsActive = '1'"
        sql &= " group by  tblQuestionCategory.QCategory_Id,tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name"

        GetQsetAmount = _DB.ExecuteScalar(sql)

    End Function

    Public Function GetQSetNearQuiz(ByVal Quiz_id As String, ByVal Player_Id As String, ByVal Subject_Id As String) As DataTable

        Dim sql As String

        Dim QsetAmount As Integer = CInt(GetQsetAmount(Quiz_id, Player_Id))

        If QsetAmount < 10 Then
            Dim NeedQSet As String = (10 - QsetAmount).ToString

            sql = " SELECT distinct  top " & NeedQSet & "   tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name, tblQuestionCategory.QCategory_Id,case "

            sql &= " (	"
            sql &= " Select count(tblQuiz.Quiz_Id)"
            sql &= " FROM        tblTestSet INNER JOIN"
            sql &= " tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN"
            sql &= " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id"
            sql &= " where tblTestSet.UserId = '" & Player_Id & "' and tblTestSet.IsActive = '1'"
            sql &= " and tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id and tbltestsetquestionset.isActive = '1'"
            sql &= " )  "

            sql &= " when '0' then 'hidden' else 'visible' End as IsPracticed "

            sql &= " FROM tblQuestionSet INNER JOIN"
            sql &= " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
            sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN"
            sql &= " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN"
            sql &= " t360_tblClass ON tblLevel.[Level] = t360_tblClass.Class_Order INNER JOIN"
            sql &= " tblQuiz ON t360_tblClass.Class_Name = tblQuiz.t360_ClassName"

            sql &= " where Quiz_Id = '" & Quiz_id & "'    "
            sql &= " and    tblBook.GroupSubject_Id = '" & Subject_Id & "'"

            sql &= " union"


            sql &= " SELECT      tblQuestionSet.QSet_Id, tblQuestionSet.QSet_Name, tblQuestionCategory.QCategory_Id,'visible' as IsPracticed"
            sql &= " FROM          tblQuestionCategory INNER JOIN"
            sql &= " tblQuestionSet ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id INNER JOIN"
            sql &= " tblTestSetQuestionSet ON tblQuestionSet.QSet_Id = tblTestSetQuestionSet.QSet_Id INNER JOIN"
            sql &= " tblQuiz INNER JOIN"
            sql &= " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id"
            sql &= " where Quiz_Id = '" & Quiz_id & "' and User_Id = '" & Player_Id & "' and tbltestsetQuestionSet.IsActive = '1'"


        Else
            sql = " select tblQuestionSet.Qset_Id,tblQuestionSet.Qset_Name,case "
            sql &= " (select COUNT(*) from tblTestSetQuestionSet where IsActive = 1 and tblQuestionSet.QSet_Id = QSet_Id and TestSet_Id = "
            sql &= " (select TestSet_Id from tblquiz where quiz_id  = '" & Quiz_id & "' "
            sql &= " and IsPracticeMode = '1' ))"

            sql &= " when '0' then 'hidden' Else 'visible' End as IsPracticed"
            sql &= " from tblQuestionSet where IsActive = 1 and QCategory_Id in "
            sql &= " (select QCategory_Id from tblquestionset where IsActive = 1 and QSet_Id in"
            sql &= " (select QSet_Id from tblTestSetQuestionSet where IsActive = 1 and TestSet_Id = "
            sql &= " (select TestSet_Id from tblQuiz where Quiz_Id = '" & Quiz_id & "')))"


        End If

        GetQSetNearQuiz = _DB.getdata(sql)

    End Function

    Public Function SaveQuizDetail(ByVal Testset_Id As String, ByVal ClassName As String, ByVal RoomName As String, _
                                 ByVal SchoolCode As String, ByVal Player_Id As String, ByVal IsUseTablet As String, ByVal IsHomework As Boolean, Calendar_Id As String) As String

        Dim sql As String
        Dim IsRandom As String
        Dim IsHomeworkmode As String
        Dim IsPracticemode As String
        Dim showscore As String
        Dim showCorrect As String
        Dim NeedCorrectAnswer As String
        Dim EditHomework As String
        Dim EnabledTools As Integer = 0 ' tools ทีใช้
        Dim ShowScoreAfter As String

        If IsHomework Then
            IsRandom = 0
            IsHomeworkmode = 1
            IsPracticemode = 0
            showscore = 1
            ShowScoreAfter = 0

            showCorrect = 0
            NeedCorrectAnswer = 0
        Else
            IsRandom = 1
            IsHomeworkmode = 0
            IsPracticemode = 1
            showCorrect = 1
            showscore = 1
            ShowScoreAfter = 1
            NeedCorrectAnswer = 1
            EnabledTools = getSubjectInTestsetAndSetTools(Testset_Id) ' tools ที่มีให้ ตาม testset
        End If

        Dim Quiz_Id As String = ""

        If IsHomework Then
            sql = "select quiz_id from tblQuiz where TestSet_Id = '" & Testset_Id & "' and User_Id = '" & Player_Id & "' "
            Quiz_Id = _DB.ExecuteScalar(sql)
        End If

        If Quiz_Id = "" Then
            sql = "select newid() as Quiz_Id"
            Quiz_Id = _DB.ExecuteScalar(sql).ToString

            Dim AcademicYear As String = GetAcademicYear()
            AcademicYear = AcademicYear.Substring(2)

            'sql = "  insert into tblquiz (Quiz_Id, TestSet_Id, t360_ClassName, t360_RoomName, StudentAmount, StartTime, "
            'sql &= " AcademicYear, needTimer, IsPerQuestionMode,TimePerQuestion, TimePerTotal, NeedCorrectAnswer, IsTimeShowCorrectAnswer, "
            'sql &= " TimePerCorrectAnswer, IsShowCorrectAfterComplete, IsRushMode,needRandomQuestion, IsPracticeMode, NeedRandomAnswer, NeedShowScore, "
            'sql &= " NeedShowScoreAfterComplete, IsDifferentQuestion, IsDifferentAnswer, Selfpace,IsActive, LastUpdate, User_IdOld, t360_SchoolCode, "
            'sql &= " t360_TeacherId, IsUseTablet, User_Id,IsForHomework,EnabledTools) "
            'sql &= " values ('" & Quiz_Id & "','" & Testset_Id & "','" & ClassName & "','" & RoomName & "','1',dbo.GetThaiDate(),'" & AcademicYear & "',"
            'sql &= " '0','0','1500','50','" & NeedCorrectAnswer & "','0','30','" & showCorrect & "','0','" & IsRandom & "','" & IsPracticemode & "','" & IsRandom & "','" & showscore & "','" & ShowScoreAfter & "','0','0','1','1',dbo.GetThaiDate(),"
            'sql &= " '1','" & SchoolCode & "','" & SchoolCode & "','" & IsUseTablet & "','" & Player_Id & "','" & IsHomeworkmode & "','" & EnabledTools & "')"

            sql = "  insert into tblquiz (Quiz_Id, TestSet_Id, StudentAmount, StartTime, "
            sql &= " needTimer, IsPerQuestionMode,TimePerQuestion, TimePerTotal, NeedCorrectAnswer, IsTimeShowCorrectAnswer, "
            sql &= " TimePerCorrectAnswer, IsShowCorrectAfterComplete, IsRushMode,needRandomQuestion, IsPracticeMode, NeedRandomAnswer, NeedShowScore, "
            sql &= " NeedShowScoreAfterComplete, IsDifferentQuestion, IsDifferentAnswer, Selfpace,IsActive, LastUpdate, User_IdOld, t360_SchoolCode, "
            sql &= " t360_TeacherId, IsUseTablet, User_Id,IsHomeworkMode,EnabledTools,Calendar_Id,FullScore,t360_ClassName,t360_RoomName) "
            sql &= " values ('" & Quiz_Id & "','" & Testset_Id & "','1',dbo.GetThaiDate(),"
            sql &= " 0,0,'1500','50'," & NeedCorrectAnswer & ",0,"
            sql &= " '30'," & showCorrect & ",0," & IsRandom & "," & IsPracticemode & "," & IsRandom & "," & showscore & ","
            sql &= ShowScoreAfter & ",0,0,1,1,dbo.GetThaiDate(),"
            sql &= " '1','" & SchoolCode & "','" & SchoolCode & "','" & IsUseTablet & "','" & Player_Id & "','" & IsHomeworkmode & "',"
            sql &= "'" & EnabledTools & "','" & Calendar_Id & "','"
            sql &= GetFullScore(Testset_Id) & "','" & ClassName & "','" & RoomName & "');"

            _DB.Execute(sql)


            EditHomework = False

        Else
            EditHomework = True
            sql = "update tblQuizSession set LastUpdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "'"
            _DB.Execute(sql)
            sql = "update tblQuiz set LastUpdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "'"
            _DB.Execute(sql)
        End If

        SaveQuizDetail = Quiz_Id & ":" & EditHomework

    End Function


    Public Function GetFullScore(ByVal TestsetId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String
        sql = " select sum(answer_score) as FullScore from tblAnswer where Question_Id in (select Question_Id "
        sql &= " from tblTestSetQuestionDetail where TSQS_Id in(select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & TestsetId.CleanSQL & "'))"
        GetFullScore = _DB.ExecuteScalar(sql, InputConn)

    End Function

    Public Function GetNowTime() As Object
        Dim sql As String
        sql = "select dbo.GetThaiDate()"
        GetNowTime = _DB.ExecuteScalar(sql)
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

    Public Function GetTestsetId(ByVal Qset_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String, ByVal Level_Id As String) As String

        Dim sql As String
        sql = " SELECT    tblTestSet.testset_id "
        sql &= " FROM tblTestSetQuestionSet INNER JOIN"
        sql &= " tblTestSet ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id"
        sql &= " where tblTestSet.UserType = '2' and tbltestsetQuestionSet.IsActive = '1' and tbltestset.IsActive = '1'"
        sql &= " and tblTestSetQuestionSet.QSet_Id = '" & Qset_Id & "'"


        Dim TestsetId As String = _DB.ExecuteScalar(sql)

        ' ถ้า testset นี้ไม่เคยมีใครสร้างมาก่อนให้สร้างใหม่

        If TestsetId = "" Then
            sql = "select NEWID() as Testset_Id"
            TestsetId = _DB.ExecuteScalar(sql)


            sql = " insert into tbltestset (TestSet_Id, TestSet_Name, UserIdOld, SchoolId, Level_Id, TestSet_Time, TestSet_FontSize, "
            sql &= " IsPracticeMode, IsActive, LastUpdate, NeedConnectCheckmark, UserId,UserType) "
            sql &= " values ('" & TestsetId.CleanSQL & "',N'P_" & Qset_Id.CleanSQL & "','1','" & SchoolCode.CleanSQL & "','" & Level_Id.CleanSQL & "','30',0,1,1,dbo.GetThaiDate(),0,'" & Player_Id.CleanSQL & "','2')"
            _DB.Execute(sql)
            Return TestsetId & ",New"
        Else
            Return TestsetId & ",Old"
        End If


    End Function

    Public Sub SetTSQSAndSetTSQD(ByVal TestsetId As String, ByVal QSetId As String)

        Dim sql As String

        sql = " select NewID() as TSQS_Id"
        Dim TSQS_ID As String = _DB.ExecuteScalar(sql)

        sql = " insert into tblTestSetQuestionSet (TSQS_Id,TestSet_Id,QSet_Id,TSQS_No,IsActive,LastUpdate)"
        sql &= " values ('" & TSQS_ID.CleanSQL & "','" & TestsetId.CleanSQL & "','" & QSetId.CleanSQL & "','1','1',dbo.GetThaiDate())"
        _DB.Execute(sql)

        sql = " insert into tblTestSetQuestionDetail (TSQD_Id,TSQS_Id,Question_Id,TSQD_No,IsActive,LastUpdate)"
        sql &= " select NEWID(),'" & TSQS_ID.CleanSQL & "',a.Question_Id,a.Row,'1',dbo.GetThaiDate() "
        sql &= " from (select row_number() over(order by question_Id) as Row , Question_id from tblQuestion "
        sql &= " where QSet_Id = '" & QSetId.CleanSQL & "')as a"
        _DB.Execute(sql)

    End Sub

    Public Sub SaveQuiznswer(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal SchoolCode As String, ByVal DeviceId As String, ByVal IsUseComputer As Boolean)

        ''------------insert QuizAnswer-------------
        Dim sql As String

        sql = " SELECT   tblQuestion.QSet_Id, Qset_type FROM tblQuestion INNER JOIN"
        sql &= " tblQuizQuestion ON tblQuestion.Question_Id = tblQuizQuestion.Question_Id INNER JOIN"
        sql &= " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id"
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1'"

        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        Dim ExamType As String = dt.Rows(0)("Qset_type").ToString
        Dim ExamId As String = dt.Rows(0)("QSet_Id").ToString
        If ExamType = 6 Then
            sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "'"
        ElseIf ExamType = 3 Then

            sql = " select Question_id,Answer_id from tblAnswer where QSet_Id = '" & ExamId & "'"
        Else 'type 1 2
            sql = " select question_id,Answer_Id from tblAnswer "
            sql &= " where Question_Id =  (select Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '1')"
        End If

        Dim dtQuestionAndAns As DataTable = _DB.getdata(sql)

        For i = 0 To dtQuestionAndAns.Rows.Count - 1
            sql = "insert into tblQuizAnswer (QuizAnswer_Id,Quiz_Id,Question_Id,Answer_Id,QA_No,IsActive,LastUpdate,School_Code) "
            sql &= " values (newid(),'" & Quiz_Id & "','" & dtQuestionAndAns.Rows(i)("Question_id").ToString & "',"
            sql &= " '" & dtQuestionAndAns.Rows(i)("Answer_id").ToString & "','" & i + 1 & "','1',dbo.GetThaiDate(),'" & SchoolCode & "')"
            _DB.Execute(sql)
        Next


        '------------insert QuizSession-------------
        Dim tabId As String
        Dim Player_Type As String
        If IsUseComputer Then
            'tabId = "8B8F168D-3D82-447F-8BF1-E3A05C498AC3"
            tabId = "null"
            Player_Type = "1"

        Else
            tabId = "'" & GetTabId(DeviceId).ToString & "'"
            Player_Type = "2"
        End If

        sql = "insert into tblQuizSession (QuizSession_Id, School_Code, Quiz_Id, Player_Type,LastUpdate, Player_Id, Tablet_Id, IsActive)"
        sql &= " values (NEWID(),'" & SchoolCode & "','" & Quiz_Id.ToString & "'," & Player_Type & ",dbo.GetThaiDate(),'" & Player_Id.ToString & "'," & tabId.ToString & ",'1')"
        _DB.Execute(sql)

    End Sub

    Public Function GetPlayerInQuiz(ByVal Quiz_ID As String) As String
        Dim sql As String

        sql = "select player_id from tblquizsession where Quiz_Id = '" & Quiz_ID & "'"

        GetPlayerInQuiz = _DB.ExecuteScalar(sql)

    End Function

    Public Function GetSubjectInQuiz(ByVal Quiz_Id As String) As String
        Dim sql As String
        sql = "SELECT     tblGroupSubject.GroupSubject_Id"
        sql &= " FROM         tblBook INNER JOIN"
        sql &= " tblQuestionCategory ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN"
        sql &= "  tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id INNER JOIN"
        sql &= "  tblQuiz INNER JOIN"
        sql &= "   tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN"
        sql &= "   tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN"
        sql &= "   tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id"
        sql &= "    where Quiz_Id = '" & Quiz_Id & "' and tbltestsetQuestionSet.IsActive = '1' and tbltestset.IsActive = '1'"

        GetSubjectInQuiz = _DB.ExecuteScalar(sql)
    End Function

    Public Function GetQSetTypeFromQSetId(ByVal Qset_Id As String) As String
        Dim sql As String
        sql = "select Qset_Type from tblQuestionSet where Qset_Id = '" & Qset_Id & "'; "

        GetQSetTypeFromQSetId = _DB.ExecuteScalar(sql)

    End Function

    Public Function GetFirstQsetTypeFromQuizId(ByVal Quiz_Id As String) As String
        Dim sql As String
        sql = "Select tblQuestionSet.QSet_Type"
        sql &= " FROM tblQuestion INNER JOIN"
        sql &= " tblQuizQuestion ON tblQuestion.Question_Id = tblQuizQuestion.Question_Id INNER JOIN"
        sql &= " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id "
        sql &= " where Quiz_Id  = '" & Quiz_Id & "' and QQ_No = '1'"

    End Function

    ' 02-05-56 update หาวิชาที่อยู่ใน testset เพื่อเปิดการใช้งาน tools
    Private Function getSubjectInTestsetAndSetTools(ByVal Testset_Id As String)
        Dim EnabledTools As Integer = 16
        Dim db As New ClassConnectSql()
        Dim sqlSubject As String = " SELECT DISTINCT tgs.GroupSubject_Name FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionSet tqs "
        sqlSubject &= " ON tsqs.QSet_Id = tqs.QSet_Id INNER JOIN tblQuestionCategory tqc "
        sqlSubject &= "ON tqs.QCategory_Id = tqc.QCategory_Id INNER JOIN tblbook tb "
        sqlSubject &= "ON tqc.Book_Id = tb.Book_Id INNER JOIN tblGroupSubject tgs "
        sqlSubject &= "ON tb.GroupSubject_Id = tgs.GroupSubject_Id "
        sqlSubject &= "WHERE tsqs.TestSet_Id = '" & Testset_Id & "' and tbltestsetquestionset.isActive = '1' ;"

        Dim dtSubject As DataTable = db.getdata(sqlSubject)

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

    Public Function GetArrPropertyByQuizId(ByVal QuizId As String)

        Dim ClsViewReport As New ClassViewReport(New ClassConnectSql())
        Dim ArrProperty As New ArrayList
        Dim sql As String = " SELECT Level_Name,dbo.tblTestSet.GroupSubject_Name FROM dbo.tblQuiz INNER JOIN dbo.tblTestSet " &
                            " ON  dbo.tblQuiz.TestSet_Id = dbo.tblTestSet.TestSet_Id  INNER JOIN dbo.tblTestSetQuestionSet " &
                            " ON dbo.tblTestSet.TestSet_Id = dbo.tblTestSetQuestionSet.TestSet_Id  INNER JOIN dbo.tblQuestionSet " &
                            " ON dbo.tblTestSetQuestionSet.QSet_Id = dbo.tblQuestionSet.QSet_Id INNER JOIN  dbo.tblQuestionCategory " &
                            " ON dbo.tblQuestionSet.QCategory_Id = dbo.tblQuestionCategory.QCategory_Id INNER JOIN  dbo.tblBook " &
                            " ON dbo.tblQuestionCategory.Book_Id = dbo.tblBook.BookGroup_Id INNER JOIN  dbo.tblLevel " &
                            " ON tblbook.Level_Id = dbo.tblLevel.Level_Id  WHERE Quiz_Id = '" & QuizId & "' and tbltestsetquestionSet.IsActive = '1' " &
                            " GROUP BY Level_Name,dbo.tblTestSet.GroupSubject_Name    "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                Dim SubjectName As String = ClsViewReport.GenSubjectName(dt.Rows(index)("GroupSubject_Name").ToString())
                Dim LevelName As String = GetShortLevelNameByLongLevelName(dt.Rows(index)("Level_Name").ToString())
                ArrProperty.Add(SubjectName & "  " & LevelName)
            Next
            Return ArrProperty
        Else
            Return ArrProperty
        End If

    End Function

    Public Function GetShortLevelNameByLongLevelName(ByVal LevelName As String)

        Dim ShortLevelName As String = ""

        Select Case LevelName
            Case "ประถมศึกษาปีที่ 1"
                ShortLevelName = "ป.1"
            Case "ประถมศึกษาปีที่ 2"
                ShortLevelName = "ป.2"
            Case "ประถมศึกษาปีที่ 3"
                ShortLevelName = "ป.3"
            Case "ประถมศึกษาปีที่ 4"
                ShortLevelName = "ป.4"
            Case "ประถมศึกษาปีที่ 5"
                ShortLevelName = "ป.5"
            Case "ประถมศึกษาปีที่ 6"
                ShortLevelName = "ป.6"
            Case "มัธยมศึกษาปีที่ 1"
                ShortLevelName = "ม.1"
            Case "มัธยมศึกษาปีที่ 2"
                ShortLevelName = "ม.2"
            Case "มัธยมศึกษาปีที่ 3"
                ShortLevelName = "ม.3"
            Case "มัธยมศึกษาปีที่ 4"
                ShortLevelName = "ม.4"
            Case "มัธยมศึกษาปีที่ 5"
                ShortLevelName = "ม.5"
            Case "มัธยมศึกษาปีที่ 6"
                ShortLevelName = "ม.6"
            Case Else
                ShortLevelName = ""
        End Select

        Return ShortLevelName

    End Function

    Public Function GetAvartaForQuiz(ByVal Quiz_id As String, ByVal Calendar_Id As String, ByVal MyScore As Integer, player_id As String, SchoolId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String

        Dim dt As DataTable = GettestsetData(Quiz_id, InputConn)

        Dim dtWinnerScore As DataTable = GetTestsetWinner(dt, InputConn)

        If dtWinnerScore.Rows.Count <> 0 Then

            Dim WinnerScore As String = dtWinnerScore.Rows(0)("TSW_WinnerScore").ToString


            If WinnerScore < MyScore Then
                'Win
                sql = "update tblTestSetWinner set tsw_Studentid = '" & player_id & "' , TestSet_ID = '" & dt.Rows(0)("testset_id").ToString & "', "
                sql &= " QSet_ID = '" & dt.Rows(0)("QSet_Id").ToString & "' , TSW_WinnerScore = '" & MyScore & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL"
                sql &= " where TSW_Id = '" & dtWinnerScore.Rows(0)("TSW_Id").ToString & "' "

                _DB.Execute(sql, InputConn)

                Return 1 & "%" & dtWinnerScore.Rows(0)("TSW_StudentID").ToString

            ElseIf WinnerScore > MyScore Then
                Return 4 & "%" & dtWinnerScore.Rows(0)("TSW_StudentID").ToString

            ElseIf WinnerScore = MyScore Then
                sql = "update tblTestSetWinner set tsw_Studentid = '" & player_id.CleanSQL & "' , TestSet_ID = '" & dt.Rows(0)("testset_id").ToString.CleanSQL & "', "
                sql &= " QSet_ID = '" & dt.Rows(0)("QSet_Id").ToString.CleanSQL & "' , TSW_WinnerScore = '" & MyScore & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL"
                sql &= " where TSW_Id = '" & dtWinnerScore.Rows(0)("TSW_Id").ToString.CleanSQL & "' "

                _DB.Execute(sql, InputConn)
                Return 2 & "%" & dtWinnerScore.Rows(0)("TSW_StudentID").ToString.CleanSQL
            End If


        Else
            If player_id <> "" Then
                sql = "insert into tblTestSetWinner select NEWID(),'" & dt.Rows(0)("QSet_Id").ToString.CleanSQL & "','" & dt.Rows(0)("testset_id").ToString.CleanSQL & "'"
                sql &= ", '" & dt.Rows(0)("IsStandard").ToString.CleanSQL & "' , '" & player_id.CleanSQL & "', '" & MyScore & "' ,'" & Calendar_Id.CleanSQL & "','" & SchoolId.CleanSQL & "',dbo.GetThaiDate(),'1',NULL "
                _DB.Execute(sql, InputConn)
            End If
            Return 3 & "%000"
        End If
    End Function

    Public Function GettestsetData(ByVal Quiz_id As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String

        sql = " select top 1 tbltestset.testset_id,tblTestSetQuestionSet.QSet_Id,IsStandard from tblQuiz inner join tblTestSet "
        sql &= " on tblQuiz.testset_Id = tblTestSet.TestSet_Id "
        sql &= " inner join tblTestSetQuestionSet on tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id "
        sql &= " where tblquiz.Quiz_Id = '" & Quiz_id.CleanSQL & "' And tbltestsetQuestionset.IsActive = '1' and tbltestset.IsActive = '1' "

        Dim dt As DataTable = _DB.getdata(sql, , InputConn)

        Return dt
    End Function

    Public Function GetTestsetWinner(dt As DataTable, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String

        sql = "select TSW_Id,TSW_WinnerScore,TSW_StudentID from tblTestSetWinner "

        If dt.Rows(0)("IsStandard") = True Then
            sql &= " where QSet_Id = '" & dt.Rows(0)("QSet_Id").ToString & "'"

        Else
            sql &= " where TestSet_ID = '" & dt.Rows(0)("testset_id").ToString & "'"
        End If

        Dim dtWinnerScore As DataTable = _DB.getdata(sql, , InputConn)

        Return dtWinnerScore

    End Function

    Public Function GetTestsetWinnerFromMode(Item_Id As String, Mode As String)

        Dim sql As String

        sql = "select TSW_StudentID from tblTestSetWinner "

        If Mode = 1 Then
            sql &= " where QSet_Id = '" & Item_Id & "' and IsStandard = '1'"

        Else
            sql &= " where TestSet_ID = '" & Item_Id & "'  and IsStandard = '0'"
        End If

        Dim dtWinnerScore As String = _DB.ExecuteScalar(sql)

        Return dtWinnerScore

    End Function



End Class
