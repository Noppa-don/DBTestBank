Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class ClsActivity
    Dim _DB As ClsConnect
    Dim cls As New ClsPDF(New ClassConnectSql)
    Dim ClsHomework As New ClsHomework(New ClassConnectSql)
    Dim ClsP As New Service.ClsPracticeMode(New ClassConnectSql)
    Public MyAnswer As String
    Public CorrectAnswer As String
    Public SwapStatus As Boolean = False
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Private _htmlAnswerExp As String
    Public Property htmlAnswerExp() As String
        Get
            Return _htmlAnswerExp
        End Get
        Set(ByVal value As String)
            _htmlAnswerExp = value
        End Set
    End Property
    Public Function GetTestsetName(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String
        Dim testsetName As String
        sql = " select Testset_Name from tblTestset where testset_id = (select testset_id from tblQuiz where Quiz_Id = '" & Quiz_Id & "');"
        testsetName = _DB.ExecuteScalar(sql, InputConn)

        GetTestsetName = testsetName

    End Function
    Public Function GetTestsetNamePC(ByVal Quiz_Id As String, ByVal IsPractice As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim sql As String
        Dim testsetName As String

        If IsPractice Then
            sql = "SELECT top 1 tblGroupSubject.GroupSubject_Name FROM tblGroupSubject INNER JOIN"
            sql &= " tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN"
            sql &= " tblQuestionCategory ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN"
            sql &= " tblQuestionSet ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id INNER JOIN"
            sql &= " tblTestSetQuestionSet ON tblQuestionSet.QSet_Id = tblTestSetQuestionSet.QSet_Id INNER JOIN"
            sql &= " tblTestSet ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN"
            sql &= " tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id"
            sql &= " where Quiz_Id = '" & Quiz_Id & "' And tblTestSetQuestionSet.IsActive = '1'"
            testsetName = _DB.ExecuteScalar(sql, InputConn)

            Select Case testsetName
                Case "กลุ่มสาระการเรียนรู้ภาษาไทย"
                    testsetName = "ไทย"

                Case "กลุ่่มสาระการเรียนรู้ศิลปะ"
                    testsetName = "ศิลปะ"

                Case "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี"
                    testsetName = "การงานฯ"

                Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
                    testsetName = "สุขศึกษาฯ"

                Case " กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                    testsetName = "ภาษาอังกฤษ"

                Case "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม"
                    testsetName = "สังคมฯ"

                Case "กลุ่มสาระการเรียนรู้วิทยาศาสตร์"
                    testsetName = "วิทยาศาสตร์"

                Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                    testsetName = "คณิตฯ"
            End Select

            testsetName = "โหมดฝึกฝนตัวเอง วิชา " & testsetName
        Else

            sql = " select Testset_Name from tblTestset where testset_id = (select testset_id from tblQuiz where Quiz_Id = '" & Quiz_Id & "');"
            testsetName = _DB.ExecuteScalar(sql, InputConn)



        End If

        GetTestsetNamePC = testsetName
    End Function
    Public Function GetNotAnswer(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal ExamNum As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean

        Dim sql As String
        sql = "select ResponseAmount from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "'  "
        sql &= "and Student_Id = '" & Player_Id & "'"
        Dim ResponseAmount As String = _DB.ExecuteScalar(sql, InputConn)

        If ResponseAmount = "0" Or ResponseAmount = "" Then
            GetNotAnswer = True
        Else
            GetNotAnswer = False
        End If
    End Function
    Public Function GetNeedCheckMark(ByVal testset_Id As String) As String

        Dim sql As String

        sql = " select NeedConnectCheckmark from tblTestSet where  TestSet_Id = '" & testset_Id & "' "
        GetNeedCheckMark = _DB.ExecuteScalar(sql)



    End Function
    Public Function GetQuestionDetail(ByVal Question_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        sql = " select Question_Name,tblquestionset.QSet_Id,tblquestionset.qset_Name,Question_Expain from tblQuestion inner join tblquestionset on tblquestion.qset_id = tblquestionset.QSet_Id where Question_Id = '" & Question_Id & "';"
        GetQuestionDetail = _DB.getdata(sql, , InputConn)
    End Function
    Public Function GetAnswerDetailForStudentPad(ByVal Question_Id As String, ByVal Quiz_Id As String) As DataTable

        Dim sql As String
        sql = "select count(QQ_No) as QuestionAmount  from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "' and Question_Id = '" & Question_Id & "';"
        Dim dt As DataTable = _DB.getdata(sql)

        sql = " select qset_type from tblQuestionSet where QSet_Id in (select QSet_Id from tblQuestion where Question_Id = '" & Question_Id & "'); "
        Dim dtType As DataTable = _DB.getdata(sql)
        If Not dt.Rows(0)("QuestionAmount") = 0 Then

            If dtType.Rows(0)("qset_type") = "3" Then

                'If RandState Then
                'sql = "select cast (answer_name as varchar) as Answer_name from tblAnswer "
                'sql &= " where QSet_Id in (select QSet_Id from tblTestSetQuestionSet where  TestSet_Id in "
                'sql &= " (select TestSet_Id from tblQuiz where Quiz_Id = '" & Quiz_Id & "') "
                'sql &= " and QSet_Id in (select QSet_Id from tblQuestionSet where QSet_Type = '3')) group by cast (answer_name as varchar) order by newid();"
                'Else
                sql = "select cast (answer_name as varchar(max)) as Answer_name from tblAnswer "
                sql &= " where QSet_Id in (select QSet_Id from tblTestSetQuestionSet where  TestSet_Id in "
                sql &= " (select TestSet_Id from tblQuiz where Quiz_Id = '" & Quiz_Id & "') "
                sql &= " and QSet_Id in (select QSet_Id from tblQuestionSet where QSet_Type = '3') And tblTestSetQuestionSet.isActive = '1') group by cast (answer_name as varchar(max));"
                'End If
            Else
                'If RandState Then
                'sql = "select answer_Id,answer_name from tblanswer where question_id = '" & Question_Id & "' order by newid();"
                'Else
                sql = "select answer_Id,answer_name from tblanswer where question_id = '" & Question_Id & "' order by Answer_No;"
                'End If
            End If
        Else
            sql = " select a.answer_name , a.Answer_id from tblAnswer a  where a.Answer_Id in (select Answer_Id from tblQuizAnswer where "
            sql &= " Quiz_Id = '" & Quiz_Id & "' and Question_Id = '" & Question_Id & "');"

        End If

        GetAnswerDetailForStudentPad = _DB.getdata(sql)
    End Function
    Public Function GetQSetType(ByVal QSet_Id As String, ByRef InputConn As SqlConnection) As String
        Dim sql As String
        sql = "Select Qset_Type,Qset_Name from tblQuestionSet where Qset_Id = '" & QSet_Id & "';"
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        GetQSetType = dt.Rows(0)("Qset_Type").ToString

    End Function
    Public Function GetQuestionSortExamFromtblQuizQuestion(ByVal Quiz_id As String, ByVal Qset_Id As String) As DataTable
        Dim sql As String

        sql = " SELECT     tblQuestion.Question_Name, tblQuestion.Question_Id " &
              " FROM         tblQuestion INNER JOIN tblQuizQuestion ON " &
              " tblQuestion.Question_Id = tblQuizQuestion.Question_Id " &
              " WHERE     (tblQuizQuestion.Quiz_Id = '" & Quiz_id & "') AND (tblQuestion.QSet_Id = '" & Qset_Id & "')"

        GetQuestionSortExamFromtblQuizQuestion = _DB.getdata(sql)

    End Function
    Public Function GetAnswerSortExam(ByVal Quiz_Id As String, ByVal QSet_Id As String) As DataTable

        Dim sql As String
        sql = " select answer_name from tblAnswer where Question_Id in ( "
        sql &= " select Question_Id from tblTestSetQuestionDetail where TSQS_Id in ("
        sql &= " select TSQS_Id from tblTestSetQuestionSet where Qset_Id = '" & QSet_Id & "' And IsActive = '1' "
        sql &= " and TestSet_Id in (select TestSet_Id from tblQuiz where Quiz_Id = '" & Quiz_Id & "')) And IsActive = '1')"
        GetAnswerSortExam = _DB.getdata(sql)

    End Function
    Public Function GetCorrectAnswerSortExam(ByVal Qset_Id As String, ByRef InputConn As SqlConnection) As DataTable
        Dim sql As String
        sql = "select Answer_Name , Question_Name from tblAnswer a, tblQuestion q "
        sql &= " where(a.Question_Id = q.Question_Id And a.QSet_Id = q.QSet_Id)"
        sql &= " and a.QSet_Id = '" & Qset_Id & "' order by cast (cast(answer_Name as varchar(max))as integer)"
        GetCorrectAnswerSortExam = _DB.getdata(sql, , InputConn)

    End Function
    Public Function GetCorrectAnswerMatchExam(Quiz_id As String, Question_Id As String, ByRef InputConn As SqlConnection, Optional IsStudentAnswer As Boolean = False) As DataTable
        Dim sql As String
        sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Id AS CorrectAnswer_Id,tblAnswer.Answer_Expain ,tblquizscore.ResponseAmount "
        sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
        sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id  inner join tblquizscore on tblQuizQuestion.Quiz_Id = tblQuizScore.Quiz_Id "
        sql &= If(IsStudentAnswer, " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id ", " INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id ")
        sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_id & "' "
        sql &= " AND tblQuizQuestion.QQ_No = ("
        sql &= String.Format("SELECT QQ_No FROM tblQuizQuestion WHERE Question_Id = '{0}' AND Quiz_Id = '{1}'", Question_Id, Quiz_id)
        sql &= ")"
        sql &= " ORDER BY tblQuizAnswer.QA_No; "
        GetCorrectAnswerMatchExam = _DB.getdata(sql, , InputConn)
    End Function
    Public Function GetCorrectAnswerDetail(ByVal Question_Id As String, ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim sql As String

        Dim TypeExam As String = GetTypeExam(Question_Id, InputConn)

        If TypeExam = "3" Then

            sql = "select a.answer_Id ,a.Answer_Name ,(select  case when COUNT(*) = 0 then 0 else 1 end ,a.Answer_Expain"
            sql &= " from tblAnswer A where A.Answer_Id = QA.Answer_Id and A.Question_Id = QA.Question_Id) as Answer_Score"
            sql &= " from tblQuizanswer QA,tblAnswer a"
            sql &= " where Quiz_Id = '" & Quiz_Id & "'"
            sql &= " and qa.Question_Id = '" & Question_Id & "'"
            sql &= " and QA.Answer_Id = a.Answer_Id"
            sql &= " order by qa.QA_No;"

        ElseIf TypeExam = "6" Then

            sql = "Select Answer_Id,Answer_Name,1 as answer_score from tblanswer"
            sql &= " where Question_Id = '" & Question_Id & "'"
        Else

            sql = " select  tblanswer.answer_Id,cast(tblanswer.answer_name as varchar(max))  as answer_name ,tblanswer.answer_Score,"
            sql &= " tblQuizAnswer.QA_No,AlwaysShowInLastRow,tblanswer.Answer_Expain from tblanswer inner join tblQuizAnswer on tblAnswer.Answer_Id = tblQuizAnswer.Answer_Id "
            sql &= " and tblAnswer.Question_Id = tblQuizAnswer.Question_Id"
            sql &= " and tblQuizAnswer.Question_Id = '" & Question_Id & "' "
            sql &= " and tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' order by AlwaysShowInLastRow,tblQuizAnswer.QA_No;"
        End If

        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)

        If dt.Rows.Count = 0 Then
            If TypeExam = "3" Then
                sql = "select answer_Id ,Answer_Name ,(select  case when COUNT(*) = 0 then 0 else 1 end  "
                sql &= " from tblAnswer as aa where aa.Question_Id = '" & Question_Id & "' "
                sql &= " and aa.Answer_Id = a.Answer_Id  ) as Answer_Score from tblAnswer a where QSet_Id = "
                sql &= " (select QSet_Id from tblQuestion where Question_Id = '" & Question_Id & "')"
            ElseIf TypeExam = "6" Then
                sql = "Select Answer_Id,Answer_Name,1 as answer_score from tblanswer"
                sql &= " where Question_Id = '" & Question_Id & "'"
            Else
                sql = "select Answer_Id,Answer_Name,Answer_Score,AlwaysShowInLastRow,Answer_Expain from tblAnswer where Question_Id = '" & Question_Id & "'  ORDER BY AlwaysShowInLastRow "
            End If

            dt = _DB.getdata(sql, , InputConn)
        End If

        GetCorrectAnswerDetail = dt
    End Function
    Public Function GetTypeExam(ByVal Question_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String
        sql = " select qset_type from tblQuestionSet where QSet_Id in (select QSet_Id from tblQuestion where Question_Id = '" & Question_Id & "'); "
        Dim dtType As DataTable = _DB.getdata(sql, , InputConn)
        GetTypeExam = dtType.Rows(0)("qset_type").ToString

    End Function
    Public Sub SaveAnswer(ByVal Quiz_Id As String, ByVal Question_Id As String, ByVal QA_No As Integer, ByVal Answer_name As String)
        'Insert ข้อมูลลง tblQuizAnswer เอา Answer_Name มาหา Answer_Id ก่อน
        Dim AnsId As String
        Dim sql As String
        sql = "Select Qset_Id from tblQuestion where Question_Id = '" & Question_Id & "';"
        Dim dt As DataTable = _DB.getdata(sql)
        Dim QsetId As String = dt.Rows(0)("Qset_Id").ToString
        Answer_name = GenPathForSaveImage(Answer_name, QsetId)
        If GetTypeExam(Question_Id) = "3" Then
            sql = " Select Answer_Name from tblAnswer where Question_Id = '" & Question_Id & "';"
            Dim dtMatchAns As DataTable = _DB.getdata(sql)
            Dim AnsName As String = dtMatchAns.Rows(0)("Answer_Name")
            If AnsName = Answer_name Then
                sql = "Select Answer_Id from tblAnswer where Question_Id = '" & Question_Id & "';"
                Dim dtAnsId As DataTable = _DB.getdata(sql)
                AnsId = dtAnsId.Rows(0)("Answer_Id").ToString
            Else
                sql = " select top 1 Answer_Id from tblAnswer where  Answer_Name like '%" & Answer_name & "%'"
                Dim dtAns As DataTable = _DB.getdata(sql)
                AnsId = dtAns.Rows(0)("Answer_Id").ToString
            End If

            sql = "select count(QA_No) as AnswerAmount from tblQuizAnswer where Question_Id = '" & Question_Id & "' "
            sql &= " and Quiz_Id = '" & Quiz_Id & "' and Answer_Id = '" & AnsId & "'; "
            Dim dtCheck As DataTable = _DB.getdata(sql)
            If dtCheck.Rows(0)("AnswerAmount") = 0 Then
                'ถ้าจะใช้ Function นี้ ต้องเพิ่ม SchoolCode
                sql = "insert into tblQuizAnswer (Quiz_Id,Question_Id,Answer_Id,QA_No) Values('" & Quiz_Id & "','" & Question_Id & "','" & AnsId & "'," & QA_No & ");"
                _DB.Execute(sql)
            End If


        Else
            If GetTypeExam(Question_Id) = "2" Then
                If Answer_name = "ถูก" Then
                    Answer_name = "True"
                ElseIf Answer_name = "ผิด" Then
                    Answer_name = "False"
                End If
            End If

            sql = " select Answer_Id from tblAnswer where Question_Id = '" & Question_Id & "' and Answer_Name like '%" & Answer_name & "%'"
            Dim dtAns As DataTable = _DB.getdata(sql)
            AnsId = dtAns.Rows(0)("Answer_Id").ToString

            sql = "select count(QA_No) as AnswerAmount from tblQuizAnswer where Question_Id = '" & Question_Id & "' "
            sql &= " and Quiz_Id = '" & Quiz_Id & "' and Answer_Id = '" & AnsId & "';  "
            Dim dtCheck As DataTable = _DB.getdata(sql)
            If dtCheck.Rows(0)("AnswerAmount").ToString = "0" Then
                'ถ้าจะใช้ Function นี้ ต้องเพิ่ม SchoolCode
                sql = "insert into tblQuizAnswer (Quiz_Id,Question_Id,Answer_Id,QA_No) Values('" & Quiz_Id & "','" & Question_Id & "','" & AnsId & "'," & QA_No & ");"
                _DB.Execute(sql)
            End If

        End If

    End Sub
    Public Function SetStudent(ByVal Quiz_Id As String, ByVal School_Code As String, ByVal Current_Class As String, ByVal Current_Room As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        'sql = " select Owner_id ,Owner_Type,Tablet_Id "
        sql = " select Owner_id ,Owner_Type,o.Tablet_Id,t.Tablet_SerialNumber  "
        sql &= " FROM t360_tblStudent AS s INNER JOIN"
        sql &= " t360_tblTabletOwner AS o ON s.Student_Id = o.Owner_Id AND s.School_Code = o.School_Code "
        sql &= " INNER JOIN t360_tblTablet t ON t.Tablet_Id = o.Tablet_Id "
        sql &= " WHERE     (o.School_Code = '" & School_Code & "') AND (s.Student_CurrentClass = N'" & Current_Class & "') AND (s.Student_CurrentRoom = N'" & Current_Room & "') AND o.TabletOwner_IsActive = 1;"
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)

        For i = 0 To dt.Rows.Count - 1
            sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id,Tablet_Id) values ('" & School_Code & "',"
            sql &= "'" & dt.Rows(i)("Owner_id").ToString & "','" & dt.Rows(i)("Owner_Type") & "','" & Quiz_Id & "','" & dt.Rows(i)("Tablet_Id").ToString & "');"
            _DB.Execute(sql, InputConn)
            Dim TestsetName As String = GetTestsetName(Quiz_Id, InputConn)
            Log.Record(Log.LogType.Quiz, "ทำ Quiz """ & TestsetName & """", True, dt.Rows(i)("Owner_id").ToString())
        Next
        Return dt
    End Function
    Public Function SetStudentUseSoundLab(ByVal Quiz_Id As String, ByVal School_Code As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        '        Dim sql As String = " SELECT distinct t360_tblStudent.Student_Id,t360_tblStudent.Student_Id as Owner_id , " & _
        '"(select tblTabletLabDesk.Tablet_Id from tblTabletLabDesk where tblTabletLabDesk.DeskName = t360_tblStudent.Student_CurrentNoInRoom " & _
        '"and tblTabletLabDesk.TabletLab_Id = tblQuiz.TabletLab_Id) as Tablet_Id " & _
        '"FROM         tblQuiz LEFT OUTER JOIN " & _
        '                      "tblTabletLabDesk ON tblQuiz.TabletLab_Id = tblTabletLabDesk.TabletLab_Id RIGHT OUTER JOIN " & _
        '                      "t360_tblStudent ON tblQuiz.t360_ClassName = t360_tblStudent.Student_CurrentClass AND tblQuiz.t360_RoomName = t360_tblStudent.Student_CurrentRoom " & _
        '"WHERE     (tblQuiz.Quiz_Id = '" & Quiz_Id & "') AND (t360_tblStudent.School_Code = '" & School_Code & "') "

        Dim sql As String = "  SELECT distinct t360_tblStudent.Student_Id,t360_tblStudent.Student_Id as Owner_id ,tblTabletLabDesk.Tablet_Id  , t360_tbltablet.tablet_SerialNumber "
        sql &= "  FROM  tblQuiz LEFT OUTER JOIN tblTabletLabDesk ON tblQuiz.TabletLab_Id = tblTabletLabDesk.TabletLab_Id "
        sql &= " inner join t360_tbltablet on tblTabletLabDesk.tablet_id = t360_tbltablet.tablet_id  "
        sql &= " RIGHT OUTER JOIN t360_tblStudent ON tblQuiz.t360_ClassName = t360_tblStudent.Student_CurrentClass AND tblQuiz.t360_RoomName = t360_tblStudent.Student_CurrentRoom "
        sql &= " WHERE     (tblQuiz.Quiz_Id = '" & Quiz_Id.ToString & "') AND (t360_tblStudent.School_Code = '" & School_Code & "') "
        sql &= " and tblTabletLabDesk.DeskName = t360_tblStudent.Student_CurrentNoInRoom and tblTabletLabDesk.TabletLab_Id = tblQuiz.TabletLab_Id and t360_tblStudent.Student_IsActive = '1';"
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then

            For i = 0 To dt.Rows.Count - 1

                If dt.Rows(i)("Tablet_Id").ToString <> "" Then
                    sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id,Tablet_Id) values ('" & School_Code & "',"
                    sql &= "'" & dt.Rows(i)("Student_Id").ToString & "','2','" & Quiz_Id & "','" & dt.Rows(i)("Tablet_Id").ToString & "');"
                Else
                    sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id) values ('" & School_Code & "',"
                    sql &= "'" & dt.Rows(i)("Student_Id").ToString & "','2','" & Quiz_Id & "');"
                End If


                _DB.Execute(sql, InputConn)
                Dim TestsetName As String = GetTestsetName(Quiz_Id, InputConn)
                Log.Record(Log.LogType.Quiz, "ทำ Quiz """ & TestsetName & """", True, dt.Rows(i)("Student_Id").ToString())
            Next
        End If
        Return dt
    End Function
    Public Sub setTeacher(ByVal userId As String, ByVal Quiz_Id As String, ByVal School_Code As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String
        sql = " select tbo.Tablet_Id as Tablet_Id,tbo.Owner_Id as Owner_Id,tbo.Owner_Type as Owner_Type from tblUser tu "
        sql &= " LEFT JOIN t360_tblTabletOwner tbo "
        sql &= " ON tu.GUID = tbo.Owner_Id "
        sql &= " WHERE tu.GUID = '" & userId & "' "
        sql &= " AND tu.SchoolId = '" & School_Code & "' "
        sql &= " AND tbo.TabletOwner_IsActive = '1' "
        sql &= " AND tbo.Owner_Type = '1' "
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)

        If dt.Rows.Count > 0 Then
            Try
                sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id,Tablet_Id) values ('" & School_Code & "',"
                sql &= "'" & dt.Rows(0)("Owner_Id").ToString & "','" & dt.Rows(0)("Owner_Type") & "','" & Quiz_Id & "','" & dt.Rows(0)("Tablet_Id").ToString & "');"
                _DB.Execute(sql, InputConn)
                Dim TestsetName As String = GetTestsetName(Quiz_Id, InputConn)
                Log.Record(Log.LogType.Quiz, "ครูเปิด Quiz """ & TestsetName & """", True)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End If

    End Sub
    Public Sub SetTeacherSoundLab(ByVal UserId As String, ByVal QuizId As String, ByVal SchoolId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT tblTabletLabDesk.Tablet_Id FROM tblQuiz INNER JOIN tblTabletLabDesk ON tblQuiz.TabletLab_Id = tblTabletLabDesk.TabletLab_Id " &
                            " WHERE (tblQuiz.Quiz_Id = '" & QuizId.ToString() & "') AND (tblTabletLabDesk.Player_Type = 1) and (tblTabletLabDesk.IsActive = 1) "
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then
            sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id,Tablet_Id) values ('" & SchoolId & "',"
            sql &= "'" & UserId.ToString() & "','1','" & QuizId & "','" & dt.Rows(0)("Tablet_Id").ToString() & "');"
            _DB.Execute(sql, InputConn)
            Dim TestsetName As String = GetTestsetName(QuizId, InputConn)
            Log.Record(Log.LogType.Quiz, "ครูเปิด Quiz """ & TestsetName & """", True, dt.Rows(0)("Tablet_Id").ToString())
        End If

    End Sub
    Public Function GetQuestionSetName(ByVal Question_Id As String) As String
        Dim sql As String
        sql = "select Qset_name from tblQuestionSet where QSet_Id in "
        sql &= " (Select QSet_Id from tblQuestion where Question_Id = '" & Question_Id & "') "
        Dim dt As DataTable = _DB.getdata(sql)
        GetQuestionSetName = dt.Rows(0)("Qset_name").ToString

    End Function
    Public Function GetReviewQset(ByVal quizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        sql = " select MAX(qq.qq_no) , qs.QSet_Id , qs.QSet_Type from tblQuizQuestion qq, tblQuestion q , tblQuestionSet qs"
        sql &= " where qq.Quiz_Id = '" & quizId & "' and QQ.Question_Id  = Q.Question_Id "
        sql &= " and Q.QSet_Id = QS.QSet_Id group by qs.QSet_Id, qs.QSet_Type"
        sql &= " order by  MAX(qq.qq_no) "

        GetReviewQset = _DB.getdata(sql, , InputConn)

    End Function
    Public Function GetSetting(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable 'Get ค่า Setting ต่างๆ

        Dim sql As String
        sql = " select IsShowCorrectAfterComplete,NeedCorrectAnswer,NeedRandomQuestion,NeedRandomAnswer,NeedTimer,IsPerQuestionMode,IsShowCorrectAfterComplete,IsHomeWorkMode,"
        sql &= " NeedShowScore,NeedShowScoreAfterComplete,IsDifferentQuestion,IsDifferentAnswer,Selfpace,t360_SchoolCode,EnabledTools,IsUseTablet,IsTimeShowCorrectAnswer"
        sql &= " from tblQuiz where Quiz_Id = '" & Quiz_Id & "';"
        GetSetting = _DB.getdata(sql, , InputConn)

    End Function
    Public Function HaveQuestion(ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal CheckIsSelfPace As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim sql As String
        'If CheckIsSelfPace = True Then
        '    HaveQuestion = False
        'Else
        sql = " select COUNT(Question_id)as QuestionAmount from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' "
        sql &= " and  QQ_No = '" & ExamNum & "';"
        Dim QuestionAmount As String = _DB.ExecuteScalar(sql, InputConn)
        If CInt(QuestionAmount) > 0 Then
            HaveQuestion = True
        Else
            HaveQuestion = False
        End If
        'End If
    End Function
    Public Function MeHaveQuestion(ByVal Quiz_id As String, ByVal Examnum As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim sql As String
        'sql = "select COUNT(Question_id)as QuestionAmount from tblQuizScore"
        'sql &= " where Quiz_Id = '" & Quiz_id & "'"
        'sql &= " and Question_Id in (select Question_Id from tblQuizQuestion where Quiz_Id = '" & Quiz_id & "' and QQ_No = '" & Examnum & "')"
        'sql &= " and Student_Id = '" & Player_Id & "'"
        sql = "select COUNT(Question_id)as QuestionAmount from tblQuizScore "
        sql &= " where Quiz_Id = '" & Quiz_id & "' "
        sql &= " and Student_Id = '" & Player_Id & "' and QQ_No = '" & Examnum & "' and isactive = '1' "
        Dim QuestionAmount As String = _DB.ExecuteScalar(sql, InputConn)

        If CInt(QuestionAmount) > 0 Then
            MeHaveQuestion = True
        Else
            MeHaveQuestion = False
        End If

    End Function
    Public Function CountLeapExam(ByVal Quiz_Id As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        ' เปลี่ยน query จำนวนข้อทั้งหมด จาก COUNT(QuizQuestion_Id) เป็น COUNT(DISTINCT(QQ_No))
        Dim sql As String
        sql = " select AllExam - MadeExam As CrossExam from "
        sql &= " (select COUNT(DISTINCT(QQ_No))as AllExam  from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "')AllExam,"
        sql &= " (select COUNT(QuizScore_Id)as MadeExam from tblquizscore where Quiz_Id = '" & Quiz_Id & "' "
        sql &= " and ResponseAmount <> 0 and Student_Id = '" & Player_Id & "')MadeExam; "
        CountLeapExam = _DB.ExecuteScalar(sql, InputConn)
    End Function
    Public Function HaveIsScored(ByVal Quiz_Id As String, ByVal Examnum As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim sql As String
        sql = "select top 1 IsScored from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & Examnum & "' and IsActive = '1'"
        Dim IsScored As String = _DB.ExecuteScalar(sql, InputConn)

        If IsScored = "False" Then
            HaveIsScored = False
        Else
            HaveIsScored = True
        End If

    End Function
    Public Function HaveIsScoredTeacher(ByVal Quiz_Id As String, ByVal Examnum As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean

        Dim sql As String
        sql = " SELECT TOP 1 IsScored_Teacher FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & Quiz_Id & "' AND QQ_No = '" & Examnum & "' "
        Dim IsScored As String = _DB.ExecuteScalar(sql, InputConn)

        If IsScored = "False" Or IsScored = "" Then
            HaveIsScoredTeacher = False
        Else
            HaveIsScoredTeacher = True
        End If

    End Function
    Public Function GetQuestionID(ByVal quiz_Id As String, ByVal ExamNum As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim sql As String = "select question_id from tblQuizQuestion where  Quiz_Id = '" & quiz_Id & "' and QQ_No = '" & ExamNum & "';"
        GetQuestionID = _DB.ExecuteScalar(sql, InputConn)
    End Function
    Public Function RenderQuestion(ByVal Quiz_Id As String, ByVal PlayerId As String, ByVal AnswerState As String, ByVal ExamNum As String, ByVal IsSelfpace As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String

        If HttpContext.Current.Application(Quiz_Id & "|IsSelfPace") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(Quiz_Id & "|IsSelfPace") = IsSelfpace
            HttpContext.Current.Application.UnLock()
        End If

        Dim PlayerType As String = GetPlayerTypeByPlayerId(PlayerId, Quiz_Id, InputConn)

#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5.1"))
#End If

#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5.2"))
#End If

        'Dim QsetType As String = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, PlayerId, InputConn)
        Dim QsetType As String

        Dim DifferentQuestion As Boolean ' check ว่าเป็นคำถามต่างกันหรือเปล่า
        If HttpContext.Current.Application(Quiz_Id & "|DifferentQuestion") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(Quiz_Id & "|DifferentQuestion") = CBool(_DB.ExecuteScalar(" SELECT IsDifferentQuestion FROM tblQuiz WHERE Quiz_Id = '" & Quiz_Id & "' "))
            HttpContext.Current.Application.UnLock()
        End If
        DifferentQuestion = HttpContext.Current.Application(Quiz_Id & "|DifferentQuestion")

        If IsSelfpace = False And DifferentQuestion = False Then ' เป็นชุดไปพร้อมกัน ไม่สลับคำถาม ?
            If HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType") Is Nothing Then
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType") = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, PlayerId, InputConn)
                HttpContext.Current.Application.UnLock()
            End If
            QsetType = HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType")
        Else
            QsetType = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, PlayerId, InputConn)
        End If

        Dim dtQuestion As DataTable = GetQuestion(Quiz_Id, PlayerId, ExamNum, InputConn)

#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5.3"))
#End If

        If QsetType = "6" Or QsetType = "3" Then
            RenderQuestion = dtQuestion.Rows(0)("QSet_Name")
            RenderQuestion = cls.CleanSetNameText(RenderQuestion)
        Else
            If dtQuestion.Rows(0)("Question_Name_Quiz") IsNot DBNull.Value Then
                RenderQuestion = dtQuestion.Rows(0)("Question_Name_Quiz")
            Else
                RenderQuestion = dtQuestion.Rows(0)("Question_Name")
            End If
        End If

#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5.4"))
#End If

        SwapStatus = False
        RenderQuestion = ExamNum & ". " & RenderQuestion.Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))

        'RenderQuestion = "<div style='display:inline-block;width:30px;'>" & ExamNum & ". " & "<div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></div><div style='display:inline-block;vertical-align:top;margin-left:10px;width:750px;'>" & RenderQuestion.Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)) & "</div>"

        'RenderQuestion = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & ExamNum & ". " & "</div><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></td><td style='padding-left:10px;vertical-align:top;'>" & RenderQuestion.Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)) & "</td></tr></table>"

        If AnswerState = "2" Then
            If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value And dtQuestion.Rows(0)("Question_Expain").ToString() <> "" Then
                RenderQuestion = String.Format("{0}<div style=""display:none;"" id=""QuestionExp"">{1}</div>", RenderQuestion, dtQuestion.Rows(0)("Question_Expain").Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)))
            End If
        End If
#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5.5"))
#End If

        'เวลาเรียกใช้  mainQuestion.InnerHtml = ExamNum.ToString & ". " & Quest

    End Function
    Private Function GetQuestion(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal ExamNum As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim PlayerType As String = GetPlayerTypeByPlayerId(Player_Id, Quiz_Id, InputConn)

        Dim sql As String = ""

        If PlayerType = "1" Then
            sql = " SELECT DISTINCT dbo.tblQuestionSet.QSet_Id,CAST(dbo.tblQuestionSet.QSet_Name as varchar(max)) as QSet_Name "
            sql &= " ,CAST(dbo.tblQuestion.Question_Name as varchar(max))as Question_Name,tblquestion.Question_Id,CAST(tblQuestion.Question_Expain as varchar(max)) as Question_Expain "
            sql &= " ,CAST(dbo.tblQuestion.Question_Name_Quiz as varchar(max))as Question_Name_Quiz,tblquestion.Question_Id,CAST(tblQuestion.Question_Expain_Quiz as varchar(max)) as Question_Expain_Quiz "
            sql &= " FROM dbo.tblQuestionSet INNER JOIN"
            sql &= " dbo.tblQuestion ON dbo.tblQuestionSet.QSet_Id = dbo.tblQuestion.QSet_Id INNER JOIN"
            sql &= " dbo.tblTestSetQuestionDetail ON dbo.tblQuestion.Question_Id = dbo.tblTestSetQuestionDetail.Question_Id INNER JOIN"
            sql &= " dbo.tblQuizQuestion ON dbo.tblTestSetQuestionDetail.Question_Id = dbo.tblQuizQuestion.Question_Id"
            sql &= " WHERE (dbo.tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "') AND (dbo.tblQuizQuestion.QQ_No = '" & ExamNum & "') And tblTestsetQuestionDetail.IsActive = '1';"
        Else

            sql = " SELECT tblQuestionSet.QSet_Id,CAST(tblQuestionSet.QSet_Name AS VARCHAR(MAX))AS QSet_Name, " &
                                         " CAST(tblQuestion.Question_Name AS VARCHAR(MAX)) AS Question_Name,tblquestion.Question_Id,CAST(tblQuestion.Question_Expain   as varchar(max)) as Question_Expain ,CAST(dbo.tblQuestion.Question_Name_Quiz as varchar(max))as Question_Name_Quiz,tblquestion.Question_Id,CAST(tblQuestion.Question_Expain_Quiz as varchar(max)) as Question_Expain_Quiz  FROM tblQuizScore INNER JOIN " &
                                         " tblQuestion ON tblQuizScore.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                                         " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id WHERE (tblQuizScore.Quiz_Id = '" & Quiz_Id & "') " &
                                        " AND  (tblQuizScore.QQ_No = '" & ExamNum & "') " &
                                        "AND (tblQuizScore.Student_Id = '" & Player_Id & "')"

        End If

        If HttpContext.Current.Application(Quiz_Id & "|IsSelfPace") = False And HttpContext.Current.Application(Quiz_Id & "|DifferentQuestion") = False Then
            If HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Question") IsNot Nothing Then
                GetQuestion = DirectCast(HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Question"), DataTable)
            Else
                Dim dt As New DataTable
                dt = _DB.getdata(sql, , InputConn)
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Question") = dt
                HttpContext.Current.Application.UnLock()
                GetQuestion = dt
            End If
        Else
            GetQuestion = _DB.getdata(sql, , InputConn)

            If GetQuestion.Rows.Count = 0 Then
                sql = " SELECT tblQuestionSet.QSet_Id,CAST(tblQuestionSet.QSet_Name AS VARCHAR(MAX)) AS QSet_Name, 
                        CAST(tblQuestion.Question_Name AS VARCHAR(MAX)) AS Question_Name,
                        tblquestion.Question_Id,CAST(tblQuestion.Question_Expain   as varchar(max)) as Question_Expain ,
                        CAST(dbo.tblQuestion.Question_Name_Quiz as varchar(max))as Question_Name_Quiz,
                        tblquestion.Question_Id,CAST(tblQuestion.Question_Expain_Quiz as varchar(max)) as Question_Expain_Quiz  
                        FROM tblQuizQuestion INNER JOIN  tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id 
                        INNER JOIN  tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id 
                        WHERE (tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "') AND (tblQuizQuestion.QQ_No = '" & ExamNum & "')"
            End If

            GetQuestion = _DB.getdata(sql, , InputConn)

        End If
    End Function

    'Public Function GetIsValidateHomework(ByVal Quiz_Id As String, ByVal Player_Id As String) As Boolean
    '    Dim sql As String
    '    sql = "  SELECT tblModuleDetailCompletion.IsValidate"
    '    sql &= " FROM tblModuleDetail INNER JOIN"
    '    sql &= " tblTestSet INNER JOIN"
    '    sql &= " tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id INNER JOIN"
    '    sql &= " tblModuleDetailCompletion ON tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id"
    '    sql &= " where tblQuiz.Quiz_Id = '" & Quiz_Id & "'"
    '    sql &= " and tblModuleDetailCompletion.student_id = '" & Player_Id & "'"

    '    GetIsValidateHomework = _DB.ExecuteScalar(sql)

    'End Function
    Public Function RenderIntro(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal ExamNum As String, ByVal ViewIntroQsetId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        'Return tagIntro : ViewIntroQsetId : LimitAmount (ถ้าเป็นของเด็ก จะส่งไปให้ว่าเหลืออีกกี่รอบ) : เล่นตอนแรกหรือเล่นทุกข้อ
        'Return tagIntro : Type : LimitAmount : ViewIntroQsetId

        'ถ้าข้อสอบผูกกับ QSet ถ้าเล่นได้ทุกข้อ จะต้องรู้จำกัดการเล่นมั้ย ถ้าจำกัด เมื่อเปลี่ยนข้อแล้วจะเล่นได้กี่ครั้ง เหลืออีกกี่ครั้ง 
        'ถ้าข้อสอบผูกกับ Question เมื่อเปลี่ยนข้อ ไม่ต้องสนใจว่าเหลือกี่ครั้ง ให้เอาจำนวนที่เล่นได้ 
        'คิดเผื่อข้อสอบนึงมีหลาย Intro ด้วย
        'ViewIntroQsetId ครั้งแรกจะเป็นค่าว่าง เมื่อเล่น Intro ไปแล้ว จะใส่ค่า Qset ที่แสดงเพื่อบอกว่าแสดงไปแล้ว 

        Dim TagIntro As String
        Dim IntroType As String
        Dim sql As String

        'หาว่าเป็นครูหรือนักเรียน
        Dim PlayerType As String = GetPlayerTypeByPlayerId(Player_Id, Quiz_Id, InputConn)

        'หา Question_Id,Qset_Id
        Dim dtQuestion As DataTable = GetQuestion(Quiz_Id, Player_Id, ExamNum, InputConn)
        Dim Question_Id As String = dtQuestion.Rows(0)("Question_id").ToString
        Dim Qset_Id As String = dtQuestion.Rows(0)("Qset_Id").ToString

        'เช็คก่อนว่า Qset นี้มี Intro มั้ย
        sql = "select introqset_Id,IsShowEveryQuestion from tblIntroQuestionSet where Qset_Id = '" & Qset_Id & "'"
        Dim dtHaveIntro As DataTable = _DB.getdata(sql, , InputConn)

        If dtHaveIntro.Rows.Count <> 0 Then
            Dim dtAboutIntro As DataTable = GetIntroSettingFromQuestion(Question_Id, InputConn)

            'เช็คว่า intro นี้ผูกอยู่กับคำถามข้อนี้มั้ย
            sql = "select IntroQQ_Id from tblIntroQuestionSetQuestion where question_Id = '" & Question_Id & "' and IntroQset_Id in ("
            sql &= " select introqset_Id from tblIntroQuestionSet where Qset_Id = '" & Qset_Id & "')"

            Dim dtQuestionIntro As DataTable = _DB.getdata(sql, , InputConn)
            Dim IsShowEveryQuestion As String = dtHaveIntro.Rows(0)("IsShowEveryQuestion").ToString

            If dtQuestionIntro.Rows.Count <> 0 Then
                'Intro นี้ผูกอยู่กับคำถาม
                'ดึง Intro ของคำถามนี้
                Dim IntroDetailsrc As String
                Dim IntroDetailData As String
                IntroType = dtAboutIntro.Rows(0)("Intro_Type").ToString
                IntroDetailsrc = dtAboutIntro.Rows(0)("Intro_Detail").Replace("scr=""", "scr=""" & cls.GenFilePath(Qset_Id))
                IntroDetailData = dtAboutIntro.Rows(0)("Intro_Detail").Replace("<source scr=""", "data=""" & cls.GenFilePath(Qset_Id))

                If IntroType = "2" Then

                    TagIntro = "<video controls preload=""auto"" width=""100%"" height=""50"" id=""myVideoJS"" class=""video-js vjs-default-skin"""
                    TagIntro &= " data-setup='{""example_option"":true}'><source "
                    TagIntro &= IntroDetailsrc
                    TagIntro &= " type=""video/mp4""> </video><object id=""objVideo"" width=""100%"" height=""50"" "
                    TagIntro &= IntroDetailData
                    TagIntro &= " </object>"
                ElseIf IntroType = "1" Then
                    'Video เป็นรูปให้กดแล้วกาง Div ออกมาเป็น Video 
                    TagIntro = "<video controls preload=""auto"" width=""100%"" height=""400"" id=""myVideoJS"" class=""video-js vjs-default-skin"""
                    TagIntro &= " data-setup='{""example_option"":true}'><source "
                    TagIntro &= IntroDetailsrc
                    TagIntro &= " type=""video/mp4""> </video><object id=""objVideo"" width=""100%"" height=""50"" "
                    TagIntro &= IntroDetailData
                    TagIntro &= " </object>"
                End If

                'ElseIf IntroType = "3" Then



                '    'TagIntro = "<span id=""spnTest"">"
                '    'TagIntro &= dtAboutIntro.Rows(0)("Intro_Detail").ToString.Substring(0, 50) & "...อ่านต่อ"
                '    'TagIntro &= "</span>"

                'End If

                'Return tagIntro : Type : LimitAmount : ViewIntroQsetId

                Dim ViewPlayAmountLimit As Integer = CInt(dtAboutIntro.Rows(0)("ViewPlayAmountLimit").ToString)
                If PlayerType = "1" Then
                    'ถ้าเป็นครู Render เลย Return จำนวนที่เด็กกดได้เป็นเลขเดิมตลอด
                    Return TagIntro & "@:@" & IntroType & "@:@" & ViewPlayAmountLimit & "@:@" & ViewIntroQsetId
                Else
                    'ของเด็กต้องเช็คก่อนว่าดูได้อีกกี่ครั้ง ถ้า 0 ครั้ง ต้องส่งเป็น image ถ้ามากกว่า 0 ต้องส่ง Intro_detail
                    sql = " SELECT ViewIntroAmount as PlayedIntro FROM tblQuizScore"
                    sql &= " where Student_Id = '" & Player_Id & "' and Quiz_Id = '" & Quiz_Id & "' "
                    sql &= " and Question_Id = '" & Question_Id & "'"

                    'ได้ จำนวนที่กดดูไปแล้ว
                    Dim ViewAmount As String = _DB.ExecuteScalar(sql, InputConn)
                    'ถ้าเป็นแบบ Question เดียว หลาย Intro น่าจะต้องเพิ่ม For เพื่อวนเอา Intro ทุกตัวออกมา

                    If ViewPlayAmountLimit <> 0 Then

                        If CInt(ViewAmount) < CInt(ViewPlayAmountLimit) Then

                            Dim CanViewAmount As String = CInt(ViewPlayAmountLimit) - CInt(ViewAmount)

                            Return TagIntro & "@:@" & IntroType & "@:@" & CanViewAmount & "@:@" & ViewIntroQsetId

                        Else
                            Return "เล่นไม่ได้แล้วค่ะ@:@" & IntroType & "@:@0@:@" & ViewIntroQsetId
                        End If
                    End If
                End If
            End If
            'Else
            '        'Intro นี้ไม่ได้ผูกกับคำถาม ต้องสนว่าถ้า Set เป็นแบบเล่นตอนแรก ต้องเล่นก่อนค่อยทำข้อสอบ ถ้าเล่นทีละข้อต้องให้เล่นได้ทุกข้อ ตอนนี้ยังไม่มีข้อสอบแบบนี้เลย

            '        Dim IntroQsetDetail As Object

            '        Dim dtSettingQsetIntro As DataTable = GetIntroSettingFromQuestionSet(Qset_Id)

            '        IntroQsetDetail = dtSettingQsetIntro.Rows(0)("Intro_Detail").Replace("scr=""", "scr=""" & cls.GenFilePath(dtSettingQsetIntro.Rows(0)("Qset_Id").ToString))

            '        If PlayerType = 1 Then

            '            If InStr(ViewIntroQsetId, dtSettingQsetIntro.Rows(0)("Qset_Id").ToString) = 0 Then
            '                If ViewIntroQsetId = "" Then
            '                    ViewIntroQsetId = dtSettingQsetIntro.Rows(0)("Qset_Id").ToString
            '                Else
            '                    ViewIntroQsetId = ViewIntroQsetId & "," & dtSettingQsetIntro.Rows(0)("Qset_Id").ToString
            '                End If
            '                Return IntroQsetDetail & ":" & ViewIntroQsetId & ":" & dtSettingQsetIntro.Rows(0)("ViewPlayAmountLimit").ToString & ":" & IsShowEveryQuestion
            '            End If

            '        Else

            '            If InStr(ViewIntroQsetId, dtSettingQsetIntro.Rows(0)("Qset_Id").ToString) = 0 Then
            '                If ViewIntroQsetId = "" Then
            '                    ViewIntroQsetId = dtSettingQsetIntro.Rows(0)("Qset_Id").ToString
            '                Else
            '                    ViewIntroQsetId = ViewIntroQsetId & "," & dtSettingQsetIntro.Rows(0)("Qset_Id").ToString
            '                End If


            '                sql = " SELECT ViewIntroAmount as PlayedIntro FROM tblQuizScore"
            '                sql &= " where Student_Id = '" & Player_Id & "' and Quiz_Id = '" & Quiz_Id & "' "
            '                sql &= " and Question_Id = '" & Question_Id & "'"

            '                'ได้ จำนวนที่กดดูไปแล้ว 
            '                Dim ViewIntroAmount As String = _DB.ExecuteScalar(sql)
            '                'ถ้าเป็นแบบ Question เดียว หลาย Intro น่าจะต้องเพิ่ม For เพื่อวนเอา Intro ทุกตัวออกมา
            '                If CInt(ViewIntroAmount) < CInt(dtSettingQsetIntro.Rows(0)("ViewPlayAmountLimit")) Then

            '                    Dim CanViewAmount As String = CInt(dtSettingQsetIntro.Rows(0)("ViewPlayAmountLimit")) - CInt(ViewIntroAmount)

            '                    Return IntroQsetDetail & ":" & ViewIntroQsetId & ":" & CanViewAmount & ":" & IsShowEveryQuestion

            '                Else
            '                    Return IntroQsetDetail & ":" & ViewIntroQsetId & ":" & "0:" & IsShowEveryQuestion
            '                End If
            '            End If

            '        End If

            'End If
        Else

            Return ""

        End If

    End Function
    Private Function GetIntroSettingFromQuestion(ByVal Question_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim sql As String

        sql = "select iqs.IsShowEveryQuestion,iqs.ViewPlayAmountLimit,iqs.ViewTimeLimit,i.Intro_Detail,iqs.IntroQset_Id,iqs.Qset_Id,i.Intro_Type"
        sql &= " from tblIntroQuestionSet iqs,tblIntroQuestionSetQuestion iqsq ,tblIntro i"
        sql &= " where(iqs.IntroQset_Id = iqsq.IntroQset_Id)"
        sql &= " and iqs.Intro_Id = i.Intro_Id"
        sql &= " and Question_Id = '" & Question_Id & "'"

        GetIntroSettingFromQuestion = _DB.getdata(sql, , InputConn)

    End Function
    Private Function GetIntroSettingFromQuestionSet(ByVal Qset_Id As String) As DataTable

        Dim sql As String

        sql = "select iqs.IsShowEveryQuestion,iqs.ViewPlayAmountLimit,iqs.ViewTimeLimit,i.Intro_Detail,iqs.IntroQset_Id,iqs.Qset_Id"
        sql &= " from tblIntroQuestionSet iqs ,tblIntro i"
        sql &= " where(iqs.Intro_Id = i.Intro_Id)"
        sql &= " and iqs.Qset_Id = '" & Qset_Id & "'"
        GetIntroSettingFromQuestionSet = _DB.getdata(sql)

    End Function
    Public Function GetQSetTypeExam(ByVal Quiz_id As String, ByVal ExamNum As String, ByVal PlayerType As String, ByVal PlayerId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String 'ดูว่าข้อนี้ เป็น Type อะไร

        'Dim sql As String
        'sql = "  SELECT DISTINCT dbo.tblQuestionSet.QSet_Type"
        'sql &= " FROM dbo.tblQuestionSet INNER JOIN"
        'sql &= " dbo.tblQuestion ON dbo.tblQuestionSet.QSet_Id = dbo.tblQuestion.QSet_Id INNER JOIN"
        'sql &= " dbo.tblTestSetQuestionDetail ON dbo.tblQuestion.Question_Id = dbo.tblTestSetQuestionDetail.Question_Id INNER JOIN"
        'sql &= " dbo.tblQuizQuestion ON dbo.tblTestSetQuestionDetail.Question_Id = dbo.tblQuizQuestion.Question_Id"
        'sql &= " WHERE (dbo.tblQuizQuestion.Quiz_Id = '" & Quiz_id & "') AND (dbo.tblQuizQuestion.QQ_No = '" & ExamNum & "');"
        'GetQSetTypeExam = _DB.ExecuteScalar(sql)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sql As String

        If PlayerType = "1" Then
            sql = "  SELECT DISTINCT dbo.tblQuestionSet.QSet_Type"
            sql &= " FROM dbo.tblQuestionSet INNER JOIN"
            sql &= " dbo.tblQuestion ON dbo.tblQuestionSet.QSet_Id = dbo.tblQuestion.QSet_Id INNER JOIN"
            sql &= " dbo.tblTestSetQuestionDetail ON dbo.tblQuestion.Question_Id = dbo.tblTestSetQuestionDetail.Question_Id INNER JOIN"
            sql &= " dbo.tblQuizQuestion ON dbo.tblTestSetQuestionDetail.Question_Id = dbo.tblQuizQuestion.Question_Id"
            sql &= " WHERE (dbo.tblQuizQuestion.Quiz_Id = '" & Quiz_id & "') AND (dbo.tblQuizQuestion.QQ_No = '" & ExamNum & "') And tblTestsetQuestionDetail.IsActive = '1';"
        Else
            sql = " Select tblQuestionSet.QSet_Type FROM tblQuizScore INNER JOIN " &
                  " tblQuestion ON tblQuizScore.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                  " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " &
                  " WHERE (tblQuizScore.QQ_No = '" & ExamNum & "') AND (tblQuizScore.Quiz_Id = '" & Quiz_id & "') " &
                  " AND (tblQuizScore.Student_Id = '" & PlayerId & "') "
        End If

        GetQSetTypeExam = _DB.ExecuteScalar(sql, InputConn)

    End Function
    Public Function GetPlayerID(ByVal User_Id As String) As String
        Dim sql As String
        sql = " Select GUID from tblUser where UserID = '" & User_Id & "'"
        GetPlayerID = _DB.ExecuteScalar(sql)
    End Function
    Public Function GetPlayerType(ByVal Player_Id As String, ByVal Quiz_id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim Sql As String = "Select Player_Type from tblQuizSession Where Player_Id = '" & Player_Id & "' and Quiz_Id = '" & Quiz_id & "';"
        'Dim pType = _DB.ExecuteScalar(Sql, InputConn)

        Dim pType As String
        If HttpContext.Current.Application(Quiz_id & "|" & Player_Id & "|PlayerType") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(Quiz_id & "|" & Player_Id & "|PlayerType") = _DB.ExecuteScalar(Sql, InputConn)
            HttpContext.Current.Application.UnLock()
        End If
        pType = HttpContext.Current.Application(Quiz_id & "|" & Player_Id & "|PlayerType")

        If pType = "" Then
            pType = "1"
        End If
        GetPlayerType = pType

    End Function
    Public Function GetQsetIDFromExamNum(ByVal ExamNum As String, ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String = " select qset_Id from tblQuestion where Question_Id in "
        sql &= " (select top 1 Question_Id from tblQuizQuestion where QQ_No = '" & ExamNum & "' and Quiz_Id = '" & Quiz_Id & "') "
        GetQsetIDFromExamNum = _DB.ExecuteScalar(sql, InputConn)

    End Function
    Public Function RenderAnswer(ByVal Player_Id As String, ByVal AnswerState As String,
                                 ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal IsPracticeMode As Boolean,
                                 ByVal IsSelfPace As Boolean, ByVal IsHomework As Boolean, IsPracticeModeFromcomputer As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String 'รับ ExamNum มาสร้าง html Answer 

        Dim sql As String
        Dim PlayerType As String = GetPlayerType(Player_Id, Quiz_Id, InputConn)
        Dim IsTeacher As Boolean
        If PlayerType = "1" Then
            IsTeacher = True
        End If


        'Dim QsetType As String = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, Player_Id, InputConn)
        Dim QsetType As String

        Dim DiffExam As Boolean
        If HttpContext.Current.Application(Quiz_Id & "|DifferentAnswer") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(Quiz_Id & "|DifferentAnswer") = GetIsDiffExam(Quiz_Id)
            HttpContext.Current.Application.UnLock()
        End If
        DiffExam = HttpContext.Current.Application(Quiz_Id & "|DifferentAnswer")

        If IsSelfPace = False And DiffExam = False Then ' เป็นชุดไปพร้อมกัน ไม่สลับคำถาม ?
            If HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType") Is Nothing Then
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType") = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, Player_Id, InputConn)
                HttpContext.Current.Application.UnLock()
            End If
            QsetType = HttpContext.Current.Application(Quiz_Id & "|ExamNum_" & ExamNum & "|QsetType")
        Else
            QsetType = GetQSetTypeExam(Quiz_Id, ExamNum, PlayerType, Player_Id, InputConn)
        End If

        Dim tagAnswer As String

        If QsetType = "6" Then
            tagAnswer = GetAnswerType6(Quiz_Id, ExamNum, IsTeacher, AnswerState, Player_Id, IsPracticeMode, IsHomework, InputConn)
        ElseIf QsetType = "3" Then
            tagAnswer = GetAnswerType3(Quiz_Id, ExamNum, IsTeacher, AnswerState, Player_Id, IsPracticeMode, IsHomework, IsPracticeModeFromcomputer, InputConn)
        Else
            'Dim DiffExam As Boolean = GetIsDiffExam(Quiz_Id)
            tagAnswer = GetAnswer(Quiz_Id, ExamNum, IsTeacher, AnswerState, QsetType, IsSelfPace, Player_Id, DiffExam, IsHomework, IsPracticeModeFromcomputer, InputConn)
        End If

        Return tagAnswer

    End Function
    Public Function GetExamAmount(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim sql As String = "select MAX(qq_no) as QuestionAmount from tblQuizQuestion where Quiz_Id = '" & Quiz_Id & "';"
        GetExamAmount = _DB.ExecuteScalar(sql, InputConn)
    End Function

    Public Function GetIsDiffExam(ByVal Quiz_ID As String) As Boolean
        Dim sql As String = "select IsDifferentQuestion,IsDifferentAnswer from tblQuiz where Quiz_Id = '" & Quiz_ID & "';"
        Dim DtDiff As DataTable = _DB.getdata(sql)

        If DtDiff.Rows(0)("IsDifferentAnswer") Then
            'If DtDiff.Rows(0)("IsDifferentQuestion") Or DtDiff.Rows(0)("IsDifferentAnswer") Then
            GetIsDiffExam = True
        Else
            GetIsDiffExam = False
        End If

    End Function

    Public Function IsDiffExamForLeapchoice(ByVal Quiz_ID As String) As Boolean
        Dim sql As String = "select IsDifferentQuestion,IsDifferentAnswer from tblQuiz where Quiz_Id = '" & Quiz_ID & "';"
        Dim DtDiff As DataTable = _DB.getdata(sql)

        If DtDiff.Rows.Count <> 0 Then
            If DtDiff.Rows(0)("IsDifferentAnswer") Or DtDiff.Rows(0)("IsDifferentQuestion") Then
                'If DtDiff.Rows(0)("IsDifferentQuestion") Or DtDiff.Rows(0)("IsDifferentAnswer") Then
                Return True

            Else
                Return False
            End If
        End If


    End Function

    Public Function GetAnswerType6(ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal IsTeacher As Boolean,
                                   ByVal AnswerState As String, ByVal player_Id As String, ByVal IsPracticeMode As Boolean, ByVal IsHomework As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim ClsDroidpad As New ClassDroidPad(New ClassConnectSql)
        ClsDroidpad.RemoveAndAddNewAnsState(Quiz_Id, AnswerState) 'เก็บค่า AnsState ไว้ในตัวแปร Application เพื่อเอาไปเช็คกับหน้าเด็ก

        Dim sql As String

        Dim dtQuestion As DataTable
        Dim dtCorrectAnswer As DataTable
        Dim dtCheckAnswer As DataTable
        Dim BG As String
        Dim Sortable As String
        Dim tagAnswer As String = ""
        Dim tagCorrectAnswer As String = ""
        Dim QuestionName As String
        Dim CorrectQuestionName As String = ""
        Dim QuestionId As String
        Dim CorrectQuestionId As String = ""

        Dim AnswerExp As New StringBuilder()
        Dim ClassHtmlAnswerExpain As String


        If AnswerState = "2" Then

            'ถ้าเป็นการเล่น Quiz 
            '     ถ้าเป็นครู เฉลย แบบเรียงถูกป้ายเขียวหมด Select จาก tblQuizQuestion
            '     ถ้าเป็นเด็ก เฉลย แบบเรียงตามที่ตอบ ป้ายเขียวแดง จาก tblQuizAnswer 
            'ถ้าเป็นฝึกฝน
            '     เฉลย แบบเรียงตามที่ตอบ ป้ายเขียวแดง จาก tblQuizAnswer

            If (IsTeacher = True) And (IsPracticeMode = False) Then 'ถ้าเฉลยของครูแบบ Quiz ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
                'sql = "SELECT ROW_NUMBER() over(order by cast(Answer_Name as varchar(max)))as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id"
                'sql &= " FROM tblQuizAnswer INNER JOIN"
                'sql &= " tblQuizQuestion ON tblQuizAnswer.Question_Id = tblQuizQuestion.Question_Id "
                'sql &= " AND tblQuizAnswer.Quiz_Id = tblQuizQuestion.Quiz_Id INNER JOIN"
                'sql &= " tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id INNER JOIN"
                'sql &= " tblAnswer ON tblQuestion.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id AND "
                'sql &= " tblQuizAnswer.Question_Id = tblAnswer.Question_Id And tblQuizQuestion.Question_Id = tblAnswer.Question_Id"
                'sql &= " where tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' "
                'sql &= " and player_id = '" & player_Id & "' "
                'sql &= " and tblQuizQuestion.Question_Id in (select Question_Id from tblQuizQuestion where QQ_No = '" & ExamNum & "' and Quiz_Id = '" & Quiz_Id & "') "
                'sql &= " order by CAST(Answer_Name as varchar(max))"

                sql = " SELECT ROW_NUMBER() over(order by QA_No)as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id "
                sql &= " FROM tblQuizAnswer INNER JOIN"
                sql &= " tblQuizQuestion ON tblQuizAnswer.Question_Id = tblQuizQuestion.Question_Id "
                sql &= " AND tblQuizAnswer.Quiz_Id = tblQuizQuestion.Quiz_Id INNER JOIN"
                sql &= " tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id"
                sql &= " where tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and player_id = '" & player_Id & "' "
                sql &= " and tblQuizQuestion.Question_Id in (select Question_Id from tblQuizQuestion where QQ_No = '" & ExamNum & "'  and Quiz_Id = '" & Quiz_Id & "') order by QA_No;"

                dtQuestion = _DB.getdata(sql, , InputConn)
                'BG = "background-color:#1EEE1E;"
                Sortable = ""

            Else 'ถ้าเฉลยของเด็กหรือของครูแบบฝึกฝน 

                '1. ให้เรียงตามที่ตอบ ป้ายสีเขียวแดง เลื่อนไม่ได้

                sql = " SELECT tblQuestion.Question_Name, tblQuizAnswer.Question_Id,tblQuestion.QSet_Id , tblAnswer.Answer_Name as number ,tblAnswer.Answer_Expain "
                sql &= " FROM tblQuizAnswer INNER JOIN"
                sql &= " tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
                sql &= " tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
                sql &= " where tblQuizAnswer.Question_Id in (select Question_Id from tblQuestion where QSet_Id = "
                sql &= " (select QSet_Id from tblQuestion where Question_Id in ("
                sql &= " select Question_Id from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and Student_Id = '" & player_Id & "' and QQ_No = '" & ExamNum & "' )))"
                sql &= " and Quiz_Id = '" & Quiz_Id & "'"
                sql &= " and Player_Id = '" & player_Id & "'"
                sql &= " order by QA_No"

                dtQuestion = _DB.getdata(sql, , InputConn)

                sql = "select Question_id from tblAnswer where QSet_Id = (Select QSet_Id from tblQuestion where Question_Id = ("
                sql &= " Select Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & Quiz_Id & "'  "
                sql &= " And QQ_No = '" & ExamNum & "' and Student_Id = '" & player_Id & "')) ORDER BY CAST(Answer_Name as varchar(max));"

                dtCheckAnswer = _DB.getdata(sql, , InputConn)

                '2.  ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
                sql = "SELECT ROW_NUMBER() over(order by cast(Answer_Name as varchar(max)))as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id,tblAnswer.Answer_Expain "
                sql &= " FROM tblQuizAnswer INNER JOIN"
                sql &= " tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
                sql &= " tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id"
                sql &= " WHERE tblQuizAnswer.Question_Id IN"
                sql &= " (SELECT Question_Id FROM tblQuestion WHERE QSet_Id = (SELECT QSet_Id FROM tblQuestion"
                sql &= " WHERE  Question_Id IN (SELECT Question_Id FROM tblQuizScore"
                sql &= " WHERE Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND Student_Id = '" & player_Id & "' "
                sql &= " AND QQ_No = '" & ExamNum & "'))) "
                sql &= " AND tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "'"
                sql &= " ORDER BY cast(tblAnswer.Answer_Name as varchar(max))"

                dtCorrectAnswer = _DB.getdata(sql, , InputConn)
                Sortable = ""

            End If

        Else 'ถ้าคำถาม ให้เรียงตาม QuizAnswer ไม่ต้องป้ายสี ถ้าเคยตอบแล้วเลื่อนไม่ได้ ยังไม่ตอบเลื่อนได้
            BG = ""
            If IsTeacher = True Then
                sql = " SELECT ROW_NUMBER() over(order by QA_No)as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id"
                sql &= " FROM tblQuizAnswer INNER JOIN"
                sql &= " tblQuizQuestion ON tblQuizAnswer.Question_Id = tblQuizQuestion.Question_Id "
                sql &= " AND tblQuizAnswer.Quiz_Id = tblQuizQuestion.Quiz_Id INNER JOIN"
                sql &= " tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id"
                sql &= " where tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and player_id = '" & player_Id & "' "
                sql &= " and tblQuizQuestion.Question_Id in (select Question_Id from tblQuizQuestion where QQ_No = '" & ExamNum & "'  and Quiz_Id = '" & Quiz_Id & "') order by QA_No"
                dtQuestion = _DB.getdata(sql, , InputConn)
            Else
                sql = " SELECT  ROW_NUMBER() over(order by dbo.tblQuizAnswer.QA_No)as Number , dbo.tblQuizAnswer.Question_Id,dbo.tblQuestion.Question_Name " &
                      " ,dbo.tblQuestion.QSet_Id FROM dbo.tblQuizAnswer INNER JOIN dbo.tblQuestion  " &
                      " ON dbo.tblQuizAnswer.Question_Id = dbo.tblQuestion.Question_Id WHERE dbo.tblQuizAnswer.Question_Id IN ( " &
                      " SELECT Question_Id FROM dbo.tblQuizQuestion WHERE QQ_No IN ( " &
                      " SELECT QQ_No FROM tblQuizQuestion WHERE (Quiz_Id = '" & Quiz_Id & "') AND (Question_Id IN ( " &
                      " SELECT Question_Id FROM  tblQuizScore WHERE (Quiz_Id = '" & Quiz_Id & " ') AND " &
                      " (Student_Id = '" & player_Id & "') AND (QQ_No = '" & ExamNum & "')))) " &
                      " AND dbo.tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' ) " &
                      " AND dbo.tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' AND dbo.tblQuizAnswer.Player_Id = '" & player_Id & "' " &
                      " ORDER BY dbo.tblQuizAnswer.QA_No "
                dtQuestion = _DB.getdata(sql, , InputConn)
            End If

            sql = " select top 1 IsScored from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "'"
            Dim IsScored As String = _DB.ExecuteScalar(sql, InputConn)
            If IsScored = "0" Or IsScored = "False" Then
                Sortable = "id=""sortable"""
            Else
                Sortable = "id=""normal"""
            End If

            'Test Practice OK !!!

        End If


        tagAnswer &= "<tr class=""6"" id=""Answer""><td><ul " & Sortable & " style=""margin-left:-40px;"" >"
        tagCorrectAnswer &= "<tr><td><ul style=""margin-left:-40px;"" >"

        For i = 0 To dtQuestion.Rows.Count - 1

            If AnswerState = "2" Then
                If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
                    'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง
                    If dtQuestion.Rows(i)("Question_Id") = dtCheckAnswer.Rows(i)("Question_Id") Then
                        BG = "background-color:#2CA505;"
                        ClassHtmlAnswerExpain = "Correct"
                    Else
                        BG = "background-color:#FF0B00;"
                        ClassHtmlAnswerExpain = "InCorrect"
                    End If
                End If

                QuestionName = dtQuestion.Rows(i)("Question_Name")
                QuestionName = QuestionName.Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
                QuestionId = dtQuestion.Rows(i)("Question_Id").ToString
                ' เพิ่มคำอธิบายคำตอบ
                If dtQuestion.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtQuestion.Rows(i)("Answer_Expain").ToString() <> "" Then
                    AnswerExp.Append(String.Format("<div class=""{0}"">{1}", ClassHtmlAnswerExpain, dtQuestion.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))))
                    'AnswerExp.Append(String.Format("<div class=""{0}"">{1}", ClassHtmlAnswerExpain, "วาดหน้าสุนัข แล้วตัดเจาะช่องลูกตา"))
                    AnswerExp.Append("</div>")
                End If

                tagAnswer &= "<li id=""" & QuestionId & """ style=""" & BG & """><span class=""CorrectLi"">ลำดับที่ " & dtQuestion.Rows(i)("Number").ToString & " </span>" & QuestionName & AnswerExp.ToString() & "</li>"

                AnswerExp.Clear()

                If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
                    CorrectQuestionName = dtCorrectAnswer.Rows(i)("Question_Name").ToString
                    CorrectQuestionName = CorrectQuestionName.Replace("___MODULE_URL___", cls.GenFilePath(dtCorrectAnswer.Rows(0)("QSet_Id").ToString))
                    CorrectQuestionId = dtCorrectAnswer.Rows(i)("Question_Id").ToString

                    ' เพิ่มคำอธิบายคำตอบ
                    If dtCorrectAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtCorrectAnswer.Rows(i)("Answer_Expain").ToString() <> "" Then
                        AnswerExp.Append(String.Format("<div class=""{0}"">{1}", "Correct", dtCorrectAnswer.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", cls.GenFilePath(dtCorrectAnswer.Rows(0)("QSet_Id").ToString))))
                        AnswerExp.Append("</div>")
                    End If

                    tagCorrectAnswer &= "<li id=""" & CorrectQuestionId & """ style=""background-color:#2CA505;""><span class=""CorrectLi"">ลำดับที่ " & dtCorrectAnswer.Rows(i)("Number").ToString & " </span>" & CorrectQuestionName & AnswerExp.ToString() & "</li>"
                End If

                AnswerExp.Clear()
            Else

                QuestionName = dtQuestion.Rows(i)("Question_Name")
                QuestionName = QuestionName.Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
                QuestionId = dtQuestion.Rows(i)("Question_Id").ToString

                tagAnswer &= "<li id=""" & QuestionId & """ style=""" & BG & """>" & QuestionName & "</li>"


            End If
        Next

        tagAnswer &= "</ul></td></tr>"
        tagCorrectAnswer &= "</ul></td></tr>"
        MyAnswer = tagAnswer
        CorrectAnswer = tagCorrectAnswer

        If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
            If AnswerState = "2" Then
                SwapStatus = True
            Else
                SwapStatus = False
            End If
        Else
            SwapStatus = False
        End If
        Return tagAnswer

    End Function

    ' UPDATE Render Type 3 !!!!!!!!
    Public Function GetAnswerType3(ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal IsTeacher As Boolean,
                                   ByVal AnswerState As String, ByVal player_Id As String, ByVal IsPracticeMode As Boolean, ByVal IsHomework As Boolean, IsPracticeFromComputer As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim ClsDroidpad As New ClassDroidPad(New ClassConnectSql)
        ClsDroidpad.RemoveAndAddNewAnsState(Quiz_Id, AnswerState) 'เก็บค่า AnsState ไว้ในตัวแปร Application เพื่อเอาไปเช็คกับหน้าเด็ก

        Dim sql As String

        Dim dtQuestion As DataTable
        Dim dtCorrectAnswer As DataTable
        'Dim dtCheckAnswer As DataTable
        Dim BG As String = ""
        Dim IsDrag As String = ""
        Dim tagAnswer As String = ""
        Dim tagCorrectAnswer As String = ""
        Dim QuestionName As String
        Dim QuestionId As String
        Dim AnswerId As String
        Dim AnswerName As String

        Dim AnswerExp As New StringBuilder()

        If AnswerState = "2" Then
            'ถ้าเป็นการเล่น Quiz 
            '     ถ้าเป็นครู เฉลย แบบเรียงถูกป้ายเขียวหมด Select จาก tblQuizQuestion
            '     ถ้าเป็นเด็ก เฉลย แบบเรียงตามที่ตอบ ป้ายเขียวแดง จาก tblQuizAnswer 
            'ถ้าเป็นฝึกฝน
            '     เฉลย แบบเรียงตามที่ตอบ ป้ายเขียวแดง จาก tblQuizAnswer

            If (IsTeacher = True) And (IsPracticeMode = False) Then 'ถ้าเฉลยของครูแบบ Quiz ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้               
                'sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name FROM tblQuizQuestion "
                'sql &= " INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                'sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                'sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id "
                'sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                'sql &= " AND tblQuizQuestion.QQ_No = '" & ExamNum & "' "
                'sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                'sql &= " ORDER BY tblQuizAnswer.QA_No;"
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain FROM tblQuizQuestion "
                sql &= " INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = '" & ExamNum & "' "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                sql &= " ORDER BY tblQuizAnswer.QA_No;"
                dtQuestion = _DB.getdata(sql, , InputConn)
                'BG = "background-color:#1EEE1E;"
                IsDrag = ""


                '2.  ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain FROM tblQuizQuestion "
                sql &= " INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = '" & ExamNum & "' "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                sql &= " ORDER BY tblQuizAnswer.QA_No;"
                dtCorrectAnswer = _DB.getdata(sql, , InputConn)

                ' case นี้เจอเมื่อ ครูกดไปถึงข้อจับคู่ แล้วออกก่อน
                If dtQuestion.Rows.Count = 0 And dtCorrectAnswer.Rows.Count = 0 Then
                    dtQuestion = GetTempQuestionType3(Quiz_Id, ExamNum, InputConn)
                    dtCorrectAnswer = dtQuestion
                End If

            Else 'ถ้าเฉลยของเด็กหรือของครูแบบฝึกฝน 
                '1. ให้เรียงตามที่ตอบ ป้ายสีเขียวแดง เลื่อนไม่ได้
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain "
                sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = (SELECT tblQuizScore.QQ_No FROM tblQuizScore INNER JOIN tblQuizQuestion "
                sql &= " ON tblQuizScore.Question_Id = tblQuizQuestion.Question_Id AND tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id "
                sql &= " WHERE tblQuizScore.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Student_Id = '" & player_Id & "') "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                sql &= " ORDER BY tblQuizAnswer.QA_No; "
                dtQuestion = _DB.getdata(sql, , InputConn)

                'sql = "select Question_id from tblAnswer where QSet_Id = (Select QSet_Id from tblQuestion where Question_Id = ("
                'sql &= " Select Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & Quiz_Id & "'  "
                'sql &= " And QQ_No = '" & ExamNum & "' and Student_Id = '" & player_Id & "')) ORDER BY CAST(Answer_Name as varchar);"

                'dtCheckAnswer = _DB.getdata(sql)

                '2.  ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain "
                sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = (SELECT tblQuizScore.QQ_No FROM tblQuizScore INNER JOIN tblQuizQuestion "
                sql &= " ON tblQuizScore.Question_Id = tblQuizQuestion.Question_Id AND tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id "
                sql &= " WHERE tblQuizScore.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Student_Id = '" & player_Id & "') "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                sql &= " ORDER BY tblQuizAnswer.QA_No; "
                dtCorrectAnswer = _DB.getdata(sql, , InputConn)
                IsDrag = ""

                ' case นี้เจอเมื่อ ครูกดไปถึงข้อจับคู่ แล้วออกก่อน
                If dtQuestion.Rows.Count = 0 And dtCorrectAnswer.Rows.Count = 0 Then
                    dtQuestion = GetTempQuestionType3(Quiz_Id, ExamNum, InputConn)
                    dtCorrectAnswer = dtQuestion
                End If

            End If

        Else 'ถ้าคำถาม ให้เรียงตาม QuizAnswer ไม่ต้องป้ายสี ถ้าเคยตอบแล้วเลื่อนไม่ได้ ยังไม่ตอบเลื่อนได้
            BG = ""
            If IsTeacher = True Then ' เรียงคำถาม type 3                
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name FROM tblQuizQuestion "
                sql &= " INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = '" & ExamNum & "' "
                If Not IsPracticeFromComputer Then
                    sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                End If
                sql &= " ORDER BY tblQuizAnswer.QA_No;"
                dtQuestion = _DB.getdata(sql, , InputConn)

                ' case ถ้าเปิดควิซ ครูจะไม่ insert ข้อสอบลง quizanswer
                If dtQuestion.Rows.Count = 0 Then
                    dtQuestion = GetTempQuestionType3(Quiz_Id, ExamNum, InputConn)
                End If

                IsDrag = If(IsPracticeFromComputer, "drag", "")
            Else
                sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name "
                sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
                sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
                sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
                sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizQuestion.QQ_No = (SELECT tblQuizScore.QQ_No FROM tblQuizScore INNER JOIN tblQuizQuestion "
                sql &= " ON tblQuizScore.Question_Id = tblQuizQuestion.Question_Id AND tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id "
                sql &= " WHERE tblQuizScore.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Student_Id = '" & player_Id & "') "
                sql &= " AND tblQuizAnswer.Player_Id = '" & player_Id & "' "
                sql &= " ORDER BY tblQuizAnswer.QA_No; "
                dtQuestion = _DB.getdata(sql, , InputConn)

                ' check ด้วยว่า เฉลยไปหรือยัง
                sql = " SELECT TOP 1 IsScored FROM tblQuizScore WHERE Quiz_Id = '" & Quiz_Id & "' AND QQ_No = '" & ExamNum & "' AND Student_Id = '" & player_Id & "'; "
                Dim IsScored As String = _DB.ExecuteScalar(sql, InputConn)
                If IsScored = "0" Or IsScored = "False" Then
                    IsDrag = "drag"
                Else
                    IsDrag = ""
                End If
            End If
        End If

        ' LOOP สร่้าง คำถามตอบ
        For i = 0 To dtQuestion.Rows.Count - 1

            'Render 
            QuestionId = dtQuestion.Rows(i)("Question_Id").ToString()
            QuestionName = dtQuestion.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
            AnswerId = dtQuestion.Rows(i)("Answer_Id").ToString()
            AnswerName = dtQuestion.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))

            If AnswerState = "2" Then
                If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
                    'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง
                    If dtQuestion.Rows(i)("Answer_Id") = dtCorrectAnswer.Rows(i)("Answer_Id") Then
                        BG = "background-color:#1EEE1E;"
                    Else
                        BG = "background-color:#FF0000;"
                    End If

                    BG = If(IsTeacher, "", BG)

                    Dim CorrectAnswerId As String = dtCorrectAnswer.Rows(i)("Answer_Id").ToString()
                    Dim CorrectAnswerName As String = dtCorrectAnswer.Rows(i)("Answer_Name").ToString()
                    ' render tag Correct
                    tagCorrectAnswer &= "<tr  style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>"
                    tagCorrectAnswer &= "<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" "
                    tagCorrectAnswer &= "style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span id=""" & CorrectAnswerId & """ style=""background-color:#1EEE1E;"" >" & CorrectAnswerName & "</span></td></tr>"


                    ' เพิ่มคำอธิบายคำตอบ
                    If dtCorrectAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtCorrectAnswer.Rows(i)("Answer_Expain").ToString() <> "" Then
                        AnswerExp.Append("<div>")
                        AnswerExp.Append(String.Format("<div class='Correct'>{0}  คู่กับ  {1}", QuestionName, CorrectAnswerName))
                        AnswerExp.Append("<div>")
                        AnswerExp.Append(dtCorrectAnswer.Rows(i)("Answer_Expain")) '.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id)))
                        AnswerExp.Append("</div>")
                        AnswerExp.Append("</div>")
                        AnswerExp.Append("</div>")
                    End If
                ElseIf (IsTeacher = True) And (IsPracticeMode = False) Then ' แก้ bug ไปก่อน ค่อย optimize ทีหลัง
                    If dtCorrectAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtCorrectAnswer.Rows(i)("Answer_Expain").ToString() <> "" Then
                        AnswerExp.Append("<div>")
                        AnswerExp.Append(String.Format("<div class='Correct'>{0}  คู่กับ  {1}", QuestionName, dtCorrectAnswer.Rows(i)("Answer_Name").ToString()))
                        AnswerExp.Append("<div>")
                        AnswerExp.Append(dtCorrectAnswer.Rows(i)("Answer_Expain")) '.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id)))
                        AnswerExp.Append("</div>")
                        AnswerExp.Append("</div>")
                        AnswerExp.Append("</div>")
                    End If
                End If
            End If

            ' render tag 
            tagAnswer &= "<tr class=""3"" id=""Answer" & i & """><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>"
            tagAnswer &= "<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" "
            tagAnswer &= "style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;text-align:center;""><span id=""" & AnswerId & """ class=""" & IsDrag & """ style=""    border: solid 1px;"
            tagAnswer &= "padding: 0 20px 0 20px; border-radius:  5.5px; box-shadow: inset 0 0 7px #06466b; border-color:  #06466b; background-color:  #bde2f7;""" & BG & """ > " & AnswerName & "</span></td></tr>"

        Next

        htmlAnswerExp = AnswerExp.ToString()

        MyAnswer = tagAnswer
        CorrectAnswer = tagCorrectAnswer

        If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
            If AnswerState = "2" Then
                SwapStatus = True
            Else
                SwapStatus = False
            End If
        Else
            SwapStatus = False
        End If

        Return tagAnswer
    End Function

    ' function สำหรับ get ข้อสอบถ้า ครู กดไปไม่ถึงช้อที่เป็นจับคู่ ที่อยู่ใน testset นั้นๆ
    Public Function GetTempQuestionType3(QuizId As String, ExamNum As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        sql = " Select a.QSet_Id,a.Question_Id,q.Question_Name,a.Answer_Id,a.Answer_Name,a.Answer_Expain"
        sql &= " FROM tblQuizQuestion qq INNER JOIN tblAnswer a On qq.Question_Id = a.Question_Id "
        sql &= " INNER JOIN tblQuestion q On q.Question_Id = a.Question_Id "
        sql &= " WHERE qq.Quiz_Id = '" & QuizId & "' and qq.QQ_No = " & ExamNum & " ;"
        Return _DB.getdata(sql, , InputConn)
    End Function


    Public Function GetAnswer(ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal IsTeacher As Boolean,
                               ByVal AnswerState As String, ByVal Qset_Type As String,
                              ByVal IsSelfPace As Boolean, ByVal Player_id As String, ByVal IsDiff As Boolean,
                              ByVal IsHomework As Boolean, ByVal IsPracticeFromComputer As Boolean, Optional _
                              ByRef InputConn As SqlConnection = Nothing) As String

        Dim ClsDroidpad As New ClassDroidPad(New ClassConnectSql)
        ClsDroidpad.RemoveAndAddNewAnsState(Quiz_Id, AnswerState) 'เก็บค่า AnsState ไว้ในตัวแปร Application เพื่อเอาไปเช็คกับหน้าเด็ก

        Dim sql As String
        Dim dtAnswer As New DataTable
        Dim dtStuAnwer As New DataTable
        Dim StuAnswer As String = "" 'Answer_Id ที่เด็กตอบ
        Dim StuIsScored As Boolean
        Dim StudentIsAnswerAlready As Boolean = False
        Dim NeedClick1 As String = ""
        Dim NeedClick2 As String = ""
        Dim BG As String
        Dim ClassHtmlAnswerExpain As String
        Dim Answered As String

        Dim tagAnswer As String

        If IsTeacher Then

            If IsDiff Then 'คำตอบต่างกัน 

                'If Qset_Type = "3" Then 'Type 3 เอามาจาก tblanswer
                '    sql = " select tblAnswer.answer_id,(select question_id "
                '    sql &= " from tblquizQuestion where QQ_No = '" & ExamNum & "' and Quiz_Id = '" & Quiz_Id & "') as question_id ,"
                '    sql &= "  tblAnswer.Answer_name,tblAnswer.Answer_Score FROM tblQuizQuestion AS qq INNER JOIN"
                '    sql &= "  tblQuestion AS tq ON qq.Question_Id = tq.Question_Id INNER JOIN tblAnswer ON tq.Question_Id = tblAnswer.Question_Id"
                '    sql &= " where  tq.qset_id = (select qset_id from tblquestion where question_id = (select question_id "
                '    sql &= " from tblquizQuestion where QQ_No = '8' and Quiz_Id = '" & Quiz_Id & "') )"
                '    sql &= " and   qq.Quiz_Id = '" & Quiz_Id & "'"
                'Else 'Select จาก QuizAnswer เอาคำถามจาก QuizQuestion
                sql = " select qa.answer_id,qa.Question_Id, Answer_name,Answer_Score,AlwaysShowInLastRow,Answer_Expain   "
                sql &= " from tblQuizAnswer qa,tblAnswer a  "
                sql &= " where(qa.Answer_Id = a.Answer_id) and Quiz_Id = '" & Quiz_Id & "'  "
                sql &= " and qa.Question_Id =  ("
                sql &= " select top 1 question_id "
                sql &= " from tblQuizQuestion "
                sql &= " where  Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and QQ_No = '" & ExamNum & "') "
                sql &= " and Player_Id = '" & Player_id & "' ORDER BY qa_no,AlwaysShowInLastRow "
                ''ต้องหาก่อนว่าคำถามข้อนี้ยอมให้สลับคำตอบหรือเปล่า
                'If ThisQuestionAllowedShuffleAnswer(Quiz_Id, ExamNum, InputConn) = True Then
                '    sql &= "  "
                'Else
                '    sql &= " ORDER BY AlwaysShowInLastRow "
                'End If
                '    sql &= " order by qa_no"
                'End If
            Else 'ถ้าข้อสอบเหมือนกัน ให้ดึงโดยไม่ต้องสน Player_Id

                sql = " select qa.answer_id,qa.Question_Id, Answer_name,Answer_Score,AlwaysShowInLastRow ,Answer_Expain "
                sql &= " from tblQuizAnswer qa,tblAnswer a "
                sql &= " where(qa.Answer_Id = a.Answer_id) and a.isactive = '1' and Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and qa.Question_Id =  (select question_id from tblQuizQuestion where  Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "') "
                sql &= "order by qa_no,AlwaysShowInLastRow "

            End If

        Else 'เด็ก

            If IsDiff Then 'ถ้าข้อสอบต่างกัน ให้ดึงของตัวเอง

                sql = " select qa.answer_id,qa.Question_Id, Answer_name,Answer_Score,AlwaysShowInLastRow,Answer_Expain   "
                sql &= " from tblQuizAnswer qa,tblAnswer a  "
                sql &= " where(qa.Answer_Id = a.Answer_id) and a.isactive = '1' and Quiz_Id = '" & Quiz_Id & "'  "
                sql &= " and qa.Question_Id =  ("
                sql &= " select top 1 question_id "
                sql &= " from tblQuizscore "
                sql &= " where  Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and Student_Id = '" & Player_id & "'"
                sql &= " and QQ_No = '" & ExamNum & "') "
                sql &= " and Player_Id = '" & Player_id & "' "
                sql &= " order by qa_no,AlwaysShowInLastRow "

            Else 'ถ้าข้อสอบเหมือนกัน ให้ดึงโดยไม่ต้องสน Player_Id

                sql = " select qa.answer_id,qa.Question_Id, Answer_name,Answer_Score,AlwaysShowInLastRow,Answer_Expain  "
                sql &= " from tblQuizAnswer qa,tblAnswer a "
                sql &= " where(qa.Answer_Id = a.Answer_id) and a.isactive = '1' and Quiz_Id = '" & Quiz_Id & "' "
                sql &= " and qa.Question_Id in  (select question_id from tblQuizScore where  Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "' and Student_Id = '" & Player_id & "') "
                sql &= "order by qa_no,AlwaysShowInLastRow "
            End If
        End If

        'ถ้าเป็นโหมดแบบไปพร้อมกัน และเป็นนักเรียนจะให้มันเก็บค่าต่างๆไว้ใน Application เลย จะได้ไม่ต้องมาดึงทุกครั้ง ทุกคน
        If IsDiff = False And IsTeacher = False And IsSelfPace = False Then
            If HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Answer" & "|" & Player_id) IsNot Nothing Then
                dtAnswer = DirectCast(HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Answer" & "|" & Player_id), DataTable)
            Else
                dtAnswer = _DB.getdata(sql, , InputConn)
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Answer" & "|" & Player_id) = dtAnswer
                HttpContext.Current.Application.UnLock()
            End If
        Else
            dtAnswer = _DB.getdata(sql, , InputConn)
        End If

        If dtAnswer.Rows.Count = 0 Then
            sql = " select answer_id,tblquizquestion.question_id , Answer_name,Answer_Score,AlwaysShowInLastRow,Answer_Expain "
            sql &= " from tblanswer inner join tblQuizQuestion on tblAnswer.Question_Id = tblquizquestion.question_id "
            sql &= " where tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' and tblanswer.isactive = '1' and tblQuizQuestion.QQ_No = '" & ExamNum & "' ORDER BY Answer_No,AlwaysShowInLastRow "
            If IsDiff = False And IsTeacher = False And IsSelfPace = False Then
                dtAnswer = _DB.getdata(sql, , InputConn)
                HttpContext.Current.Application(Quiz_Id & "|" & ExamNum & "|Answer" & "|" & Player_id) = dtAnswer
            Else
                dtAnswer = _DB.getdata(sql, , InputConn)
            End If
        End If

        If Qset_Type = "3" Then

            For Each a In dtAnswer.Rows
                a("Answer_Score") = GetType3Score(a("question_id").ToString, a("answer_id").ToString)
            Next

        End If

        'ได้ข้อสอบมาแล้ว ถ้าเป็นเด็ก ให้หาว่าตอบอะไร ตอบไปแล้วมั้ย ต้องการให้ Click ได้อีกหรือเปล่า

        'If Not ((IsTeacher = True) And (IsPracticeFromComputer = False)) Then
        If (IsTeacher = False) Or (IsPracticeFromComputer = True) Then
            sql = "select Answer_id,IsScored from tblQuizScore "
            sql &= " where  Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "' "
            sql &= " and Student_Id = '" & Player_id & "' "

            dtStuAnwer = _DB.getdata(sql, , InputConn)

            If dtStuAnwer.Rows.Count <> 0 Then
                StuAnswer = dtStuAnwer.Rows(0)("Answer_Id").ToString
                StuIsScored = dtStuAnwer.Rows(0)("IsScored").ToString
            Else
                StuAnswer = ""
                StuIsScored = False
            End If



        End If

        'If IsPracticeFromComputer = True Then
        '    sql = "select Answer_id,IsScored from tblQuizScore "
        '    sql &= " where  Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "' "
        '    sql &= " and Student_Id = '" & Player_id & "'"

        '    dtStuAnwer = _DB.getdata(sql, , InputConn)


        '    StuAnswer = dtStuAnwer.Rows(0)("Answer_Id").ToString
        '    StuIsScored = dtStuAnwer.Rows(0)("IsScored").ToString
        'End If

        If Qset_Type = "2" Then
            For Each i In dtAnswer.Rows
                Select Case i("Answer_name")
                    Case "True"
                        i("Answer_name") = "ถูก"
                    Case "False"
                        i("Answer_name") = "ผิด"
                End Select
            Next
        End If

        Dim AnswerExp As New StringBuilder()

        For i = 0 To dtAnswer.Rows.Count - 1
            Dim PrefixAnswer() As String = If(IsGroupSubjectEng(dtAnswer.Rows(i)("Question_ID").ToString(), InputConn), {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"}, {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ", "ฒ", "ณ", "ด", "ต", ""})


            Dim OldAnswer As String = ""

            If (AnswerState = "0" Or AnswerState = "1") And StuAnswer <> "" Then
                If dtAnswer.Rows(i)("answer_id").ToString = StuAnswer Then
                    BG = "background-color:#FCFC05;" 'Yellow
                    OldAnswer = "ans" '"class=""ans"""
                    StudentIsAnswerAlready = True
                Else
                    BG = ""
                End If
            Else
                If AnswerState = "2" Then
                    If IsTeacher And IsPracticeFromComputer = False Then
                        If CInt(dtAnswer.Rows(i)("Answer_score")) > 0 Then
                            BG = "background-color:#2CA505;color:white;" 'Green
                            NeedClick1 = "class='reply' "
                            NeedClick2 = "class='reply' "
                        Else
                            BG = ""
                            NeedClick1 = ""
                            NeedClick2 = ""
                        End If
                    Else
                        If StuAnswer <> "" Then
                            If dtAnswer.Rows(i)("Answer_Id").ToString = StuAnswer Then
                                If CInt(dtAnswer.Rows(i)("Answer_Score")) > 0 Then
                                    BG = "background-color:#2CA505;color:white;" 'Green
                                Else
                                    BG = "background-color:#FF0B00;color:white;" 'Red
                                End If
                                StudentIsAnswerAlready = True
                                Answered = "<img src=""../Images/Activity/ChooseCircle_pad.png"" class=""ImgCircle"" style=""display:block !important;"">"
                            Else
                                If CInt(dtAnswer.Rows(i)("Answer_Score")) > 0 Then
                                    BG = "background-color:#2CA505;color:white;" 'Green
                                Else
                                    BG = ""
                                End If

                            End If
                        Else
                            If CInt(dtAnswer.Rows(i)("Answer_Score")) > 0 Then
                                BG = "background-color:#2CA505;color:white;" 'Green
                            Else
                                BG = ""
                            End If
                        End If
                    End If
                Else
                    BG = ""
                End If
            End If



            If (StuIsScored = False) And (IsTeacher = False) And (AnswerState <> "2") Then
                'NeedClick1 = "onclick=""HilightClick1(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                'NeedClick2 = "onclick=""HilightClick2(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                NeedClick1 = "class='stuAns' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='t' "
                NeedClick2 = "class='stuAns' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='f' "
            Else
                ' ของเก่ายังไงก็บังคับให้ ="" ผมเลยขอดักอีกอันสำหรับโหมดเฉลย
                If NeedClick1.Trim() <> "class='reply'" And NeedClick2.Trim() <> "class='reply'" Then
                    NeedClick1 = ""
                    NeedClick2 = ""
                End If
            End If

            If IsHomework = True Then
                Dim IsComplete As Boolean = ClsHomework.GetIsComplete(Quiz_Id, Player_id, AnswerState)

                If IsComplete Then
                    NeedClick1 = ""
                    NeedClick2 = ""
                Else
                    'NeedClick1 = "onclick=""HilightClick1(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                    'NeedClick2 = "onclick=""HilightClick2(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                    NeedClick1 = "class='stuAns' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='t' "
                    NeedClick2 = "class='stuAns' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='f' "
                End If
            End If

            If IsPracticeFromComputer Then
                If AnswerState <> "2" Then
                    'NeedClick1 = "onclick=""HilightClick1(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                    'NeedClick2 = "onclick=""HilightClick2(this,'" & dtAnswer.Rows(i)("Question_ID").ToString & "','" & dtAnswer.Rows(i)("Answer_Id").ToString & "');"""
                    NeedClick1 = "class='stuAns " & OldAnswer & "' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='t' "
                    NeedClick2 = "class='stuAns " & OldAnswer & "' questionid='" & dtAnswer.Rows(i)("Question_ID").ToString() & "' answerid='" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' IsOne='f' "
                Else
                    NeedClick1 = ""
                    NeedClick2 = ""
                End If
            End If


            'Dim Qset_Id As String = GetQsetIDFromExamNum(ExamNum, Quiz_Id, InputConn)
            Dim Qset_Id As String
            If IsDiff = False And IsTeacher = False And IsSelfPace = False Then
                If HttpContext.Current.Application(Quiz_Id & "|Examnum_" & ExamNum & "|QsetId") Is Nothing Then
                    HttpContext.Current.Application.Lock()
                    HttpContext.Current.Application(Quiz_Id & "|Examnum_" & ExamNum & "|QsetId") = GetQsetIDFromExamNum(ExamNum, Quiz_Id, InputConn)
                    HttpContext.Current.Application.UnLock()
                End If
                Qset_Id = HttpContext.Current.Application(Quiz_Id & "|Examnum_" & ExamNum & "|QsetId")
            Else
                Qset_Id = GetQsetIDFromExamNum(ExamNum, Quiz_Id, InputConn)
            End If

            Dim Answer As String = dtAnswer.Rows(i)("Answer_name").Replace("___MODULE_URL___", cls.GenFilePath(Qset_Id))

            If i Mod 2 = 0 Then
                'Answer ฝั่งซ้าย
                tagAnswer &= "<tr style='border-bottom: solid 1px #AFAFAF;vertical-align: top;'>"
                tagAnswer &= " <td " & NeedClick1 & ""
                tagAnswer &= " style=""" & BG & "height: 50px;font-weight: bold;width:35px;position:relative;"">"
                If IsTeacher = True And IsPracticeFromComputer = False Then
                    tagAnswer &= PrefixAnswer(i) & ".</td> <td " & NeedClick2 & ""
                Else
                    If StudentIsAnswerAlready = True Then
                        tagAnswer &= PrefixAnswer(i) & ".<img src='../Images/Activity/ChooseCircle_pad.png' style='display:block !important;' class='ImgCircle' /></td> <td " & NeedClick2 & ""
                    Else
                        tagAnswer &= PrefixAnswer(i) & ".<img src='../Images/Activity/ChooseCircle_pad.png' class='ImgCircle' /></td> <td " & NeedClick2 & ""
                    End If
                End If
                'tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"" " & OldAnswer & ">" & dtAnswer.Rows(i)("Answer_name") & "</td> "
                'tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"" " & OldAnswer & ">" & Answer & "</td> "
                tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"">" & Answer & "</td> "
            Else
                'Answer ฝั่งขวา
                tagAnswer &= " <td style=""height: 50px;width:30px;""></td> "
                tagAnswer &= " <td " & NeedClick1 & ""
                tagAnswer &= "style=""" & BG & "height: 50px;font-weight: bold;width:35px;position:relative;"">"
                If IsTeacher = True And IsPracticeFromComputer = False Then
                    tagAnswer &= PrefixAnswer(i) & ".</td> <td " & NeedClick2 & ""
                Else
                    If StudentIsAnswerAlready = True Then
                        tagAnswer &= PrefixAnswer(i) & ".<img src='../Images/Activity/ChooseCircle_pad.png' style='display:block !important;' class='ImgCircle' /></td> <td " & NeedClick2 & ""
                    Else
                        tagAnswer &= PrefixAnswer(i) & ".<img src='../Images/Activity/ChooseCircle_pad.png' class='ImgCircle' /></td> <td " & NeedClick2 & ""
                    End If
                End If
                'tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"" " & OldAnswer & ">" & dtAnswer.Rows(i)("Answer_name") & "</td> "
                'tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"" " & OldAnswer & ">" & Answer & "</td> "
                tagAnswer &= " style=""" & BG & "height: 50px;width:45%;"">" & Answer & "</td> "
            End If
            StudentIsAnswerAlready = False

            If AnswerState = "2" Then
                If dtAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value Then
                    If InStr(BG, "#2CA505") Then
                        ClassHtmlAnswerExpain = "Correct"
                    ElseIf InStr(BG, "#FF0B00") Then
                        ClassHtmlAnswerExpain = "InCorrect"
                    Else
                        ClassHtmlAnswerExpain = "NotAnswered"
                    End If
                    AnswerExp.Append("<div>")
                    AnswerExp.Append(String.Format("<div class='{0}'>{1}", ClassHtmlAnswerExpain, Answered))
                    AnswerExp.Append("<table style='width:100%;'><tr style='vertical-align:top;'><td>")
                    AnswerExp.Append(PrefixAnswer(i) & ". ")
                    AnswerExp.Append("</td><td>")
                    AnswerExp.Append(Answer)

                    If dtAnswer.Rows(i)("Answer_Expain").ToString().Trim() <> "" Then
                        AnswerExp.Append("<div>")
                        AnswerExp.Append(dtAnswer.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", cls.GenFilePath(Qset_Id)))
                        AnswerExp.Append("</div>")
                    End If
                    AnswerExp.Append("</td></tr></table>")
                    AnswerExp.Append("</div>")
                    AnswerExp.Append("</div>")
                    Answered = ""
                End If
            End If
        Next

        htmlAnswerExp = AnswerExp.ToString()
        Return tagAnswer

    End Function

    Public Function GetAnswerForReview(Quiz_id As String, Question_Id As String, Optional _
                              ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = ""
        Dim dtStuAnwer As New DataTable

        sql = "select Answer_id from tblQuizScore "
        sql &= " where  Quiz_Id = '" & Quiz_id & "' and Question_id = '" & Question_Id & "' "
        'sql &= " and Student_Id = '" & player_id & "'"

        dtStuAnwer = _DB.getdata(sql, , InputConn)

        Dim StuAnwer As String

        If dtStuAnwer.Rows.Count > 0 Then
            StuAnwer = dtStuAnwer.Rows(0)("Answer_id").ToString
        Else
            StuAnwer = ""
        End If

        Return StuAnwer

    End Function

    Public Function GetScoreForPracticeFromComputer(ByVal Quiz_Id As String)
        Dim sql As String

        'sql = "select top 1 cast(round(cast(tblquizsession.totalscore as int),0) as varchar) + ' / ' + cast(round(cast(tblQuiz.fullscore as int),0) as varchar)"
        sql = " select top 1 cast(tblquizsession.totalscore as varchar) + ' / ' + cast(tblQuiz.fullscore  as varchar) "
        sql &= " from tblquiz inner join tblquizsession "
        sql &= " on tblquiz.quiz_Id = tblquizsession.quiz_id where tblquiz.quiz_id = '" & Quiz_Id & "';"

        Dim ScoreAndFullScore As String = _DB.ExecuteScalar(sql).ToString()

        Return ScoreAndFullScore.ToString().ToPointplusScore(True)

    End Function

    Public Function getStatusAnswer(ByVal Quiz_id As String, ByVal QQ_No As String)
        Dim sql As String
        sql = "select  score from tblquizscore where quiz_id = '" & Quiz_id & "' and qq_no = '" & QQ_No & "'"

        Dim Score As String = _DB.ExecuteScalar(sql).ToString()

        If Score = "" Then
            Score = "2"
        End If

        Return Score

    End Function

    Public Function GetType3Score(ByVal Question_id As String, ByVal Answer_Id As String) As String

        Dim sql As String
        sql = "Select Answer_Score from tblanswer where Question_Id = '" & Question_id & "' and answer_id = '" & Answer_Id & "';"

        Dim Score As String = _DB.ExecuteScalar(sql)

        If Score = "" Then
            GetType3Score = "0"
        Else
            GetType3Score = "1"
        End If

    End Function

    ' Get ชื่อ testset , จำนวนข้อ และเวลา
    Public Function GetTestset(ByVal TestSet_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        'sql = "select t.testset_name,COUNT(tsqd.Question_id) as QuestionAmount,t.TestSet_Time from tbltestset t ,tblTestSetQuestionDetail tsqd "
        'sql &= " where TestSet_Id = '" & TestSet_Id & "'"
        'sql &= " And tsqd.IsActive = '1' and tsqd.TSQS_Id in (select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & TestSet_Id & "' And IsActive = '1')"
        'sql &= " group by t.TestSet_Name,t.TestSet_Time;"


        sql = " SELECT TestSet_Name,SUM(QuestionAmount) AS QuestionAmount,TestSet_Time "
        sql &= "  FROM (SELECT qs.QSet_Id,qs.QSet_Type,t.testset_name,"
        sql &= " CASE qs.QSet_Type WHEN 3 THEN 1 ELSE COUNT(td.question_id) END AS QuestionAmount,t.TestSet_Time "
        sql &= " FROM tblTestSet t INNER JOIN tblTestSetQuestionSet ts ON t.TestSet_Id = ts.TestSet_Id "
        sql &= " INNER JOIN tblTestSetQuestionDetail td ON ts.TSQS_Id = td.TSQS_Id "
        sql &= " INNER JOIN tblQuestionset qs ON ts.QSet_Id = qs.QSet_Id "
        sql &= " WHERE t.TestSet_Id = '" & TestSet_Id & "' AND td.IsActive = 1 AND t.IsActive = 1 AND ts.IsActive = 1 "
        sql &= " GROUP BY qs.QSet_Id,qs.QSet_Type,t.testset_name,t.TestSet_Time) AS a"
        sql &= " GROUP BY TestSet_Name,TestSet_Time;"

        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        GetTestset = dt
    End Function

    ' Get การตั้งค่าของ Quiz
    Public Function GetUserSettingDetail(ByVal user_Id As String, ByVal TestSet_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        'ถ้าเคยเล่น testset นี้แล้ว เอาค่าที่ตั้งไว้ออกมา ถ้าไม่เคยเล่น เอาค่าที่ตั้ง Quiz ล่าสุดออกมา
        Dim sql As String
        sql = " select top 1 * from tblQuiz where User_Id = '" & user_Id & "' and TestSet_Id = '" & TestSet_Id & "' and IsQuizMode = 1 order by LastUpdate desc;"
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count = 0 Then
            sql = " select top 1 * from tblQuiz where User_Id = '" & user_Id & "' and IsQuizMode = 1 order by LastUpdate desc;"
            dt = _DB.getdata(sql, , InputConn)
            If dt.Rows.Count > 0 Then
                dt(0)("EnabledTools") = 0
            End If
        End If
        GetUserSettingDetail = dt
    End Function

    ' Get ชั้นที่สูงสุดของ testset ตัวนั้น
    Public Function GetMaxLevel(ByVal TestSet_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        'เปลี่ยนจาก Subquery เป็น Join Tune Performance ต้น 22/11/56
        'Dim sql As String = " select Class_Name from t360_tblClass, tblLevel where Class_Order = level and Level_Name in ("

        'sql &= " select top 1  Level_Name from tblLevel  where Level_Id in (select Level_Id from tblbook where BookGroup_Id in ("
        'sql &= " select Book_Id from tblQuestionCategory where QCategory_Id in (select QCategory_Id from tblQuestionSet "
        'sql &= " where QSet_Id in (select QSet_Id from tblTestSetQuestionSet  where TestSet_Id = '" & TestSet_Id & "')))) "
        'sql &= " order by Level desc);"

        Dim sql As String = " SELECT  TOP 1   t360_tblClass.Class_Name FROM tblTestSetQuestionSet INNER JOIN " &
                            " tblTestSet ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN " &
                            " tblQuestionset ON tblTestSetQuestionSet.QSet_Id = tblQuestionset.QSet_Id INNER JOIN " &
                            " tblQuestionCategory ON tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN " &
                            " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN " &
                            " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN " &
                            " t360_tblClass ON tblLevel.[Level] = t360_tblClass.Class_Order " &
                            " WHERE (tblTestSet.TestSet_Id = '" & TestSet_Id & "') And tblTestSetQuestionSet.IsActive = '1' ORDER BY dbo.tblLevel.Level DESC "
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then
            GetMaxLevel = dt.Rows(0)("Class_Name")
        Else
            GetMaxLevel = ""
        End If
    End Function

    Public Function GetStudentAmount(ByVal ClassName As String, ByVal RoomName As String, ByVal SchoolCode As String) As String

        Dim sql As String
        sql = "select COUNT(student_Id) as StudentAmount  from dbo.t360_tblStudent where Student_CurrentClass = N'" & ClassName & "'"
        sql &= " And Student_CurrentRoom = '" & RoomName & "'and School_Code = '" & SchoolCode & "' and Student_Status = 1 AND Student_IsActive = 1;"


        GetStudentAmount = _DB.ExecuteScalar(sql)
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

        Dim ShotYear As String = CurrentYear
        ShotYear = ShotYear.Substring(2)
        GetAcademicYear = ShotYear
        Return GetAcademicYear

    End Function

    Public Sub setEndTimeNotNullBeforeStartQuiz(ByVal className As String, ByVal roomName As String, ByVal schoolId As String, ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sqlUpdate As String = ""
        sqlUpdate = " UPDATE tblQuiz SET EndTime = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE t360_ClassName = '" & className & "' AND t360_RoomName = '" & roomName & "' AND t360_SchoolCode = '" & schoolId & "' AND Quiz_Id = '" & QuizId & "' AND EndTime IS NULL ;"
        _DB.Execute(sqlUpdate, InputConn)
    End Sub


    ' Insert tblQuiz
    Public Function SaveQuizDetail(ByVal Testset_Id As String, ByVal ClassName As String, ByVal RoomName As String, ByVal StudentAmount As String,
                                   ByVal needTimer As String, ByVal IsPerQuestinMode As String, ByVal TimePerQuestion As String,
                                   ByVal TimePerTotal As String, ByVal NeedCorrectAnswer As String, ByVal TimePerCorrectAnswer As String,
                                   ByVal IsShowCorrectAfterComplete As String, ByVal IsRushMode As String, ByVal needRandomQuestion As String,
                                   ByVal NeedRandomAnswer As String, ByVal User_Id As String, ByVal SchoolCode As String,
                                   ByVal TeacherId As String, ByVal IsTimeShowCorrectAnswer As String, ByVal IsUseTablet As String,
                                   ByVal NeedShowScore As String, ByVal NeedShowScoreAfterComplete As String, ByVal IsDifferentQuestion As String,
                                   ByVal Selfpace As String, ByVal IsDifferentAnswer As String, ByVal EnabledTools As String, ByVal CurrentCalendarId As String, Optional ByVal TabletLabId As String = "", Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String

        'sql = "select newid() as Quiz_Id"
        'Dim dt As DataTable = _DB.getdata(sql)
        'Dim QuizId As String = dt.Rows(0)("Quiz_Id").ToString

        'sql = " select sum(answer_score) as FullScore from tblAnswer where Question_Id in (select Question_Id "
        'sql &= " from tblTestSetQuestionDetail where TSQS_Id in(select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & Testset_Id & "' And IsActive = '1') And IsActive = '1')"

        'Dim FullScore As String = _DB.ExecuteScalar(sql, InputConn)

        Dim FullScore As String = ClsP.GetFullScore(Testset_Id, InputConn)

        Dim QuizId As String = _DB.ExecInsertSqlReturnNewID(InputConn)

        If TabletLabId = "" Then
            TabletLabId = "NULL"
        Else
            TabletLabId = "'" & TabletLabId.ToString() & "'"
        End If

        sql = "  insert into tblQuiz (Quiz_Id,TestSet_Id,t360_ClassName,t360_RoomName,StudentAmount,NeedTimer,IsPerQuestionMode,IsTimeShowCorrectAnswer,"
        sql &= " TimePerQuestion, TimePerTotal,NeedCorrectAnswer,TimePerCorrectAnswer,IsShowCorrectAfterComplete,IsRushMode,NeedRandomQuestion,NeedRandomAnswer,"
        sql &= " User_Id,t360_SchoolCode,t360_TeacherId,IsUseTablet,NeedShowScore,NeedShowScoreAfterComplete,IsDifferentQuestion,Selfpace,IsDifferentAnswer,EnabledTools,Calendar_Id,IsQuizMode,FullScore,TabletLab_Id)"
        sql &= " values('" & QuizId & "','" & Testset_Id & "','" & ClassName & "','" & RoomName & "','" & StudentAmount & "'," & needTimer & ","
        sql &= IsPerQuestinMode & ",'" & IsTimeShowCorrectAnswer & "','" & TimePerQuestion & "','" & TimePerTotal & "'," & NeedCorrectAnswer & ",'" & TimePerCorrectAnswer & "'," & IsShowCorrectAfterComplete & ","
        sql &= IsRushMode & "," & needRandomQuestion & "," & NeedRandomAnswer & ",'" & User_Id & "','" & SchoolCode & "','" & TeacherId & "','" & IsUseTablet & "',"
        sql &= NeedShowScore & "," & NeedShowScoreAfterComplete & "," & IsDifferentQuestion & "," & Selfpace & "," & IsDifferentAnswer & "," & EnabledTools & ",'" & CurrentCalendarId & "',1,'" & FullScore & "'," & TabletLabId & ");"

        _DB.Execute(sql, InputConn)

        SaveQuizDetail = QuizId

    End Function

    Public Function GetClassName(ByVal User_Id As String, ByVal prefix As String) As DataTable

        Dim sql As String
        sql = " select c.Level_ShortName,tc.Class_Order,SUBSTRING(c.Level_ShortName,3,len(c.Level_ShortName)) as NumLevel from tblLevel c,t360_tblClass tc "
        sql &= " where [Level] in  (select  distinct classid  from tblUserSubjectClass "
        sql &= " where UserId = '" & User_Id & "' AND IsActive = '1' ) and c.Level_ShortName = tc.Class_Name and SUBSTRING( tc.Class_Name,1,1) = '" & prefix & "';"
        Dim dt As DataTable = _DB.getdata(sql)
        GetClassName = dt

    End Function

    Public Function GetRoomName(ByVal ClassName As String) As DataTable

        Dim sql As String
        'sql = " select replace(Room_Name,'/','')as CutRoom from t360_tblRoom where Class_Name = '" & ClassName & "' and Room_IsActive = 1 and School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "' order by CutRoom"
        'sql = "  select ROW_NUMBER() over(order by CAST(replace(Room_Name,'/','') AS int)) as RowsNum, replace(Room_Name,'/','')as CutRoom,'0' as RowsGroup "
        'sql &= " from t360_tblRoom where Class_Name = '" & ClassName & "' AND  School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "' "
        'sql &= " AND  Room_IsActive = 1 AND isnumeric(replace(Room_Name,'/','')) = 1 GROUP BY replace(Room_Name,'/','')"
        'sql &= " union select ROW_NUMBER() over(order by replace(Room_Name,'/','')) as RowsNum, replace(Room_Name,'/','')as CutRoom,'1' as RowsGroup "
        'sql &= " from t360_tblRoom where Class_Name = '" & ClassName & "' "
        'sql &= " AND  School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "' AND  Room_IsActive = 1 AND isnumeric(replace(Room_Name,'/','')) <> 1"
        'sql &= " GROUP BY replace(Room_Name,'/','') order by RowsGroup,RowsNum"
        sql = " select replace(Room_Name,'/','')as CutRoom from t360_tblRoom inner join t360_tblStudent on "
        sql &= " t360_tblRoom.Class_Name = t360_tblStudent.Student_CurrentClass And t360_tblRoom.Room_Name = t360_tblStudent.Student_CurrentRoom"
        'sql &= " where Class_Name = 'ป.1' AND  t360_tblRoom.School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'  AND  Room_IsActive = 1"
        sql &= " where Class_Name = '" & ClassName & "' AND  t360_tblRoom.School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'  AND  Room_IsActive = 1"
        sql &= " AND Student_IsActive = 1 GROUP BY Room_Name,Class_Name order by dbo.FixedLengthClassAndRoom(Class_Name,Room_Name)"
        Dim dt As DataTable = _DB.getdata(sql)
        GetRoomName = dt

    End Function

    Public Function GetReviewExamAmount(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        'Dim sql As String
        'sql = "Select Count(Question_Id) as ExamAmount from tblQuizQuestion "
        'sql &= " where Quiz_Id = '" & Quiz_Id & "'"
        'Dim dt As DataTable = _DB.getdata(sql, , InputConn)

        'GetReviewExamAmount = dt.Rows(0)("ExamAmount")
        Return GetExamAmount(Quiz_Id, InputConn)
    End Function

    Private Function GenPathForSaveImage(ByVal Answer_Name As String, ByVal QsetId As String) As String

        Dim StrModule As String = "___MODULE_URL___"
        Dim rootPathHavePoint As String = "../file/"
        Dim rootPathNoPoint As String = "/File/"
        Dim filePathNoPoint As String
        Dim filePathHavePoint As String
        Dim PathComplete1 As String
        Dim PathComplete2 As String


        filePathNoPoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) +
     "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) +
     "/" + QsetId.Substring(6, 1) + "/" + QsetId.Substring(7, 1) + "/"
        filePathNoPoint = rootPathHavePoint + filePathNoPoint + "{" + QsetId + "}/"

        filePathHavePoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) +
         "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) +
         "/" + QsetId.Substring(6, 1) + "/" + QsetId.Substring(7, 1) + "/"
        filePathHavePoint = rootPathNoPoint + filePathHavePoint + "{" + QsetId + "}/"

        PathComplete1 = Replace(Answer_Name, filePathHavePoint, StrModule)
        PathComplete2 = Replace(PathComplete1, filePathNoPoint, StrModule)

        Return PathComplete2

    End Function

    Public Function SetQuizScore(ByVal Examnum As Integer, ByVal Quiz_Id As String, ByVal IsPractice As String,
                                 ByVal IsUseComputer As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) 'Function เอาไว้ Insert ข้อมูลลง tblQuizAnswer ของครู(โหมดแบบไปพร้อมกันครู Insert ให้)
        Dim KNSession As New ClsKNSession()

        If Examnum = 0 Or Quiz_Id Is Nothing Or Quiz_Id = "" Then
            Return "Error"
        End If

        Dim CheckError As String = ""

        CheckError = KNSession(Quiz_Id & "|" & "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession(Quiz_Id & "|" & "IsDifferentQuestion") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
        Dim IsDifferentQuestion As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsDifferentQuestion = CType(CheckError, Boolean)
        End If

        Dim SchoolCode As String = GetSchoolCodeFromQuizId(Quiz_Id) 'หาค่า SchoolCode จาก QuizId
        If SchoolCode = "" Then
            Return "Error"
        End If


        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sql As String = ""
        Dim QsetType As String = ""

        If IsSelfPace = False Then 'เช็คว่าเป็นโหมดแบบ ทำพร้อมกัน ? 

            If IsDifferentQuestion = False Then 'เช็คว่า คำถามเหมือนกัน ?

                'รอถาม Query กับพี่ชินอีกทีว่าถูกไหม
                If InsertQuizScore(Quiz_Id, SchoolCode, Examnum, InputConn) = "" Then 'Insert ข้อมูลลง tblQuizScore
                    Return "Error"
                End If

                Dim QuestionId As String = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, Examnum, InputConn) 'Select หา QuestionId
                If QuestionId = "" Then
                    Return "Error"
                End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId, False, InputConn) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                'ส่งเข้า Function SetQuizAnswer(QsetType)
                If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, False, , InputConn) = "Error" Then
                    Return "Error"
                End If

            Else 'ถ้าคำถามไม่เหมือนกัน เข้าเงื่อนไขนี้

                Dim dtPlayer As New DataTable
                sql = " SELECT Player_Id,Player_Type FROM dbo.tblQuizSession WHERE Quiz_Id = '" & Quiz_Id & "' ORDER BY Player_Type "
                dtPlayer = _DB.getdata(sql, , InputConn)
                Dim CurrentStudentId As String = ""
                Dim EachQuestionId As String = ""
                Dim EachPlayerType As String = ""
                If dtPlayer.Rows.Count > 0 Then
                    Try
                        '_DB.OpenWithTransection() 'เปิด Transaction
                        For i = 0 To dtPlayer.Rows.Count - 1 'วน Loop นักเรียนทั้งหมดเพื่อ Insert ข้อมูลลง tblQuizScore
                            EachPlayerType = dtPlayer.Rows(i)("Player_Type").ToString()
                            CurrentStudentId = dtPlayer.Rows(i)("Player_Id").ToString()

                            'EachQuestionId = GetTop1QuestionIdByPlayerId(CurrentStudentId, Quiz_Id, True, EachPlayerType, Examnum)
                            'If EachQuestionId = "" Then 'Select QuestionId เพื่อ Insert แต่ละคน
                            '    Return "Error"
                            'End If

                            ''Insert ข้อมูลลง tblQuizScore แบบใช้ Transaction  
                            'If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, CurrentStudentId, EachQuestionId, _DB, True, Examnum) = "" Then
                            '    '_DB.RollbackTransection() 'RollBack Transaction
                            '    Return "Error"
                            'End If
                            'QsetType = GetQSetTypeFromQuestionId(EachQuestionId, True) 'Select QsetType จาก QuestionId เพื่อหา Type 
                            'If QsetType = "" Then
                            '    '_DB.RollbackTransection() 'RollBack Transaction
                            '    Return "Error"
                            'End If
                            'ส่งเข้า Function SetQuizAnswer(QsetType)
                            'If SetQuizAnswer(QsetType, Quiz_Id, EachQuestionId, True, CurrentStudentId) = "Error" Then
                            '    Return "Error"
                            'End If

                            If Examnum = 1 Then
                                ' ถ้าเป็นข้อ 1 ให้ใช้โค้ดเดิมในการไปหา questionId
                                EachQuestionId = GetTop1QuestionIdByPlayerId(CurrentStudentId, Quiz_Id, False, EachPlayerType, Examnum, InputConn)
                            Else
                                ' ถ้าไม่ใช่ข้อ 1 ให้เอา questionId ไปหาที่เหลือใน qset ถ้าขอหมดจาก qset แล้ว get ข้อมาใหม่
                                Dim lastExam As Integer = Examnum - 1
                                EachQuestionId = GetQuestionInQset(Quiz_Id, CurrentStudentId, lastExam, EachPlayerType, Examnum, InputConn)

                                ' ถ้าค่าที่คืนมาเป็นค่าว่างให้ไป getTop1 มาใหม่ เพราะว่า ใน qset ไม่มีข้อเหลือแล้ว
                                If EachQuestionId = "" Then
                                    EachQuestionId = GetTop1QuestionIdByPlayerId(CurrentStudentId, Quiz_Id, False, EachPlayerType, Examnum, InputConn)
                                End If
                            End If
                            'type ครู ไม่ต้อง add quizscore
                            If EachPlayerType <> "1" Then
                                InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, CurrentStudentId, EachQuestionId, _DB, False, Examnum, InputConn)
                            End If

                            QsetType = GetQSetTypeFromQuestionId(EachQuestionId, False, InputConn) 'Select QsetType จาก QuestionId เพื่อหา Type

                            If SetQuizAnswer(QsetType, Quiz_Id, EachQuestionId, False, CurrentStudentId, InputConn) = "Error" Then
                                Return "Error"
                            End If

                        Next
                        '_DB.CommitTransection() 'Commit Transaction
                    Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                        '_DB.RollbackTransection() 'RollBack Transaction
                        Return "Error"
                    End Try
                Else
                    Return "Error"
                End If

            End If

        Else 'ถ้าทำไม่พร้อมกัน เข้าเงื่อนไขนี้ เพื่อ Insert ข้อมูลให้ครูอย่างเดียว
            'SetQuizAnswer()
            Dim QuestionId As String = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, Examnum, InputConn)
            If QuestionId = "" Then
                Return "Error"
            End If
            QsetType = GetQSetTypeFromQuestionId(QuestionId, False, InputConn)
            If QsetType = "" Then
                Return "Error"
            End If
            Dim TeacherId As String
            If IsPractice = "True" Then
                TeacherId = GetUserPracticeMode(Quiz_Id, InputConn)
                InsertQuizScorePracticeMode(Quiz_Id, SchoolCode, Examnum, IsUseComputer, QsetType, InputConn)
            Else

                TeacherId = GetTeacherIdFromQuizId(Quiz_Id, InputConn) 'หา TeacherId เพื่อเอาไป Insert tblQuizAnswer 
            End If
            If TeacherId = "" Then
                Return "Error"
            End If

            If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, False, TeacherId, InputConn) = "Error" Then
                Return "Error"
            End If

        End If

        Return "Complete"

    End Function

    ' '' function หาคำถามข้อแรก เผื่อไว้ ถ้า qset ชุดนั้น random ไม่ได้
    'Private Function GetFirstQuestionIdIsRandom(ByVal QuestionId As String, ByVal QuizId As String)
    '    Dim sql As String = " SELECT qs.QSet_IsRandomQuestion,tsqs.TestSet_Id,qs.QSet_Id FROM tblQuestionSet qs "
    '    sql &= " INNER JOIN tblTestSetQuestionSet tsqs ON qs.QSet_Id = tsqs.QSet_Id "
    '    sql &= " INNER JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id "
    '    sql &= " INNER JOIN tblQuiz q ON tsqs.TestSet_Id = q.TestSet_Id "
    '    sql &= " WHERE tsqd.Question_Id = '" & QuestionId & "' AND q.Quiz_Id = '" & QuizId & "' "
    '    Dim dt As DataTable = _DB.getdata(sql)

    'End Function

    '' function หาคำถามที่เหลือจาก qset คำถามไม่เหมือนกัน แต่ไปพร้อมกัน ซึงต้องเรียนตาม qset
    Private Function GetQuestionInQset(ByVal QuizId As String, ByVal PlayerId As String, ByVal LastExamnum As String, ByVal PlayerType As String, ByVal Examnum As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sqlQuestionId As String

        If PlayerType = "1" Then
            sqlQuestionId = " SELECT Question_Id FROM tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & Examnum & "'; "
            GetQuestionInQset = _DB.ExecuteScalar(sqlQuestionId, InputConn).ToString()
        Else
            'Dim sqlLastQuestionId As String = " SELECT Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & QuizId & "' AND Student_Id = '" & PlayerId & "' AND QQ_No = '" & LastExamnum & "'; "
            'Dim lastQuestionId As String = _DB.ExecuteScalar(sqlLastQuestionId).ToString()

            'Dim sqlTestsetId As String = " SELECT testset_id FROM tblQuiz WHERE Quiz_Id = '" & QuizId & "';"
            'Dim TestsetId As String = _DB.ExecuteScalar(sqlTestsetId).ToString

            'Dim isDiffQuestion = getQset_IsRandomQuestion(lastQuestionId)
            'Dim orderBy As String = " ORDER BY TSQD_No "
            'If isDiffQuestion Then
            '    orderBy = " ORDER BY NEWID() "
            'End If

            Dim sqlDtQset As String = " SELECT TOP 1 ts.QSet_IsRandomQuestion,ts.QSet_Type,Q.TestSet_Id FROM tblQuestionSet ts LEFT JOIN tblTestSetQuestionSet tsqs ON ts.QSet_Id = tsqs.QSet_Id "
            sqlDtQset &= " INNER JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id INNER JOIN tblQuiz Q ON tsqs.TestSet_Id = Q.TestSet_Id  "
            sqlDtQset &= " INNER JOIN tblQuizScore tq ON tq.Question_Id = tsqd.Question_Id "
            sqlDtQset &= " WHERE tq.Quiz_Id = '" & QuizId & "' AND tq.Student_Id = '" & PlayerId & "' AND tq.QQ_No = '" & LastExamnum & "' "
            sqlDtQset &= " And tsqs.IsActive = '1' And tsqd.IsActive = '1' ;"
            Dim dtQset As DataTable = _DB.getdata(sqlDtQset, , InputConn)

            If dtQset(0)("QSet_Type") = "6" Then
                GetQuestionInQset = ""
            Else
                ' check ว่าสามารถ สลับได้มั้ย
                Dim orderBy As String = " ORDER BY TSQD_No "
                If dtQset(0)("QSet_IsRandomQuestion") Then
                    orderBy = " ORDER BY NEWID() "
                End If

                Dim TestsetId As String = dtQset(0)("TestSet_Id").ToString()

                sqlQuestionId = " SELECT TOP 1 Question_Id FROM tblTestSetQuestionDetail WHERE TSQS_Id IN( "
                sqlQuestionId &= " SELECT tsqs.TSQS_Id FROM tblTestSetQuestionSet tsqs INNER JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id "
                sqlQuestionId &= " INNER JOIN tblQuizScore tq ON tq.Question_Id = tsqd.Question_Id "
                sqlQuestionId &= " WHERE tq.Quiz_Id = '" & QuizId & "' AND tq.Student_Id = '" & PlayerId & "' "
                sqlQuestionId &= " AND tq.QQ_No = '" & LastExamnum & "' AND tsqs.TestSet_Id = '" & TestsetId & "' And tsqs.IsActive = '1')  And tblTestSetQuestionDetail.IsActive = '1'"
                sqlQuestionId &= " AND Question_Id NOT IN (SELECT Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & QuizId & "' AND Student_Id = '" & PlayerId & "') "
                sqlQuestionId &= orderBy

                GetQuestionInQset = _DB.ExecuteScalar(sqlQuestionId, InputConn).ToString()
            End If

            'Dim QuestionId As String = _DB.ExecuteScalar(sqlQuestionId).ToString()
            'Dim QsetType As String = GetQSetTypeFromQuestionId(QuestionId, False)
            'If QsetType = "6" Then
            '    GetQuestionInQset = ""
            'Else
            '    GetQuestionInQset = QuestionId
            'End If
        End If
    End Function

    Public Function SetQuizScoreStudent(ByVal ExamNum As Integer, ByVal Quiz_Id As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) 'Function SetQuizScore ของเด็ก

        If Quiz_Id Is Nothing Or Quiz_Id = "" Or ExamNum = 0 Or Player_Id Is Nothing Or Player_Id = "" Then
            Return "Error"
        End If

        Dim KNSession As New ClsKNSession()
        Dim CheckError As String = ""

        CheckError = KNSession(Quiz_Id & "|" & "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession(Quiz_Id & "|" & "IsDifferentQuestion") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
        Dim IsDifferentQuestion As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsDifferentQuestion = CType(CheckError, Boolean)
        End If

        Dim SchoolCode As String = GetSchoolCodeFromQuizId(Quiz_Id, InputConn) 'หาค่า SchoolCode จาก QuizId
        If SchoolCode = "" Then
            Return "Error"
        End If

        '--------------------------------------------------------------------------------------------------------------------------------------------------------

        Dim QuestionId As String = ""
        Dim QsetType As String = ""

        If IsSelfPace = False Then 'เช็คว่าเป็นโหมดทำพร้อมกันหรือเปล่า
            'Render เลย
        Else
            'ถ้าเป็นแบบไปไม่พร้อมกันเข้าเงื่อนไขนี้
            If IsDifferentQuestion = False Then 'เช็คว่าคำถามเหมือนกัน ?
                QuestionId = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, ExamNum, InputConn) 'หา QuestionId เพื่อนำไป Insert ลง tblQuizScore
                If QuestionId = "" Then
                    Return "Error"
                End If

                If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, Player_Id, QuestionId, _DB, False, ExamNum, InputConn) = "" Then 'Insert ข้อมูลลง tblQuizScore
                    Return "Error"
                End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId, False, InputConn) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                'Function SetQuizAnswer(QsetType)
                If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, False, Player_Id, InputConn) = "Error" Then
                    Return "Error"
                End If

            Else 'ถ้าคำถามไม่เหมือนกันเข้าเงื่อนไขนี้
                QuestionId = GetTop1QuestionIdByPlayerId(Player_Id, Quiz_Id, False, "2", ExamNum, InputConn)
                If QuestionId = "" Then
                    Return "Error"
                End If

                'If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, Player_Id, QuestionId, _DB, False, ExamNum) = "" Then 'Insert ข้อมูลลง tblQuizScore
                '    Return "Error"
                'End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId, False, InputConn) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                ''Function SetQuizAnswer(QsetType)
                'If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, False, Player_Id) = "Error" Then
                '    Return "Error"
                'End If

                Dim dtQuestionInQset As DataTable = InsertQsetToQuizScoreWithTransaction(Quiz_Id, SchoolCode, Player_Id, QuestionId, _DB, False, ExamNum, QsetType, InputConn)
                For Each row As DataRow In dtQuestionInQset.Rows
                    'Function SetQuizAnswer(QsetType)
                    If SetQuizAnswer(QsetType, Quiz_Id, row(0).ToString(), False, Player_Id, InputConn) = "Error" Then
                        Return "Error"
                    End If
                Next
            End If

        End If

        Return "Complete"


    End Function

    Public Function SetQuizAnswer(ByVal QsetType As String, ByVal Quiz_Id As String, ByVal Question_Id As String, ByVal UseTransaction As Boolean, Optional ByVal Player_Id As String = "", Optional ByRef InputConn As SqlConnection = Nothing) 'Function SetQuizAnswer สำหรับ Insert ข้อมูลลง tblQuizAnswer

        If QsetType Is Nothing Or QsetType = "" Or Quiz_Id Is Nothing Or Quiz_Id = "" Or Question_Id Is Nothing Or Question_Id = "" Then
            Return "Error"
        End If

        Dim KNSession As New ClsKNSession()
        Dim CheckError As String = ""

        CheckError = KNSession(Quiz_Id & "|" & "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession(Quiz_Id & "|" & "IsDifferentAnswer") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
        Dim IsDifferentAnswer As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsDifferentAnswer = CType(CheckError, Boolean)
        End If

        Dim SchoolCode As String = ""

        If UseTransaction = True Then
            SchoolCode = GetSchoolCodeFromQuizIdWithTransaction(Quiz_Id, InputConn) 'หาค่า SchoolCode จาก QuizId
        Else
            SchoolCode = GetSchoolCodeFromQuizId(Quiz_Id, InputConn) 'หาค่า SchoolCode จาก QuizId
        End If
        If SchoolCode = "" Then
            Return "Error"
        End If

        '---------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim sql As String = ""
        Dim ReturnValue As String = ""
        Dim MergeQuestionId As String = ""
        Dim QuizAnswerAmount As String

        If QsetType = "3" Then
            'sql = "select COUNT(Question_id)as QuestionAmount from tblQuizScore where Quiz_Id = '" & Quiz_Id.ToString & "';"

            sql = String.Format("SELECT COUNT(*) FROM tblQuizQuestion qq INNER JOIN tblQuizAnswer qa ON qq.Question_Id = qa.Question_Id WHERE qq.Quiz_Id = '{0}' AND qa.Quiz_Id = '{0}' AND qa.Question_Id = '{1}';", Quiz_Id, Quiz_Id, Question_Id)

            If UseTransaction = True Then
                QuizAnswerAmount = _DB.ExecuteScalarWithTransection(sql, InputConn)
            Else
                QuizAnswerAmount = _DB.ExecuteScalar(sql, InputConn)
            End If
            If QuizAnswerAmount > 0 Then
                Return "Complete"
                'Exit Function
            End If

        End If

        If IsDifferentAnswer = False Then 'เช็คว่าคำตอบเหมือนกันหรือเปล่า ?

            If QsetType = "6" Or QsetType = "3" Then 'เช็คว่าเป็น Type 6 หรือเปล่า
                Dim Qsetid As String = GetQsetIdFromQuestionId(Question_Id, UseTransaction, InputConn)
                If Qsetid = "" Then
                    Return "Error"
                End If

                If UseTransaction = True Then
                    If MiniFunctionSaveAnswerForEachStudent(Quiz_Id, Player_Id, Question_Id, False, _DB, True, Qsetid, InputConn) = "Error" Then
                        Return "Error"
                    End If
                Else
                    'Function SaveAnswerForEachStudent ใช้ Transaction

                    If SaveAnswerForEachStudent(Quiz_Id, Question_Id, False, Qsetid, Player_Id, InputConn) = "Error" Then
                        Return "Error"
                    End If


                End If

            Else 'ถ้าไม่ใช่ Type 6 เข้าเงื่อนไขนี้
                Dim AllQuestionId As String = KNSession(Quiz_Id & "|" & "CheckInStrquestionId") 'ดึงค่า QuestionId ที่ต่อสตริงกันออกมาเพื่อเอามาเช็คก่อนเข้า Store
                If AllQuestionId Is Nothing Then 'ถ้าค่าที่ดึงออกมาเป็น Nothing แสดงว่ายังไม่มีการ Add ค่าให้ตัวแปรนี้
                    sql = " EXEC dbo.StoreSetQuizAnswer @Question_Id = '" & Question_Id & "', @Quiz_ID = '" & Quiz_Id & "' "
                    If UseTransaction = True Then
                        ReturnValue = _DB.ExecuteScalarWithTransection(sql, InputConn)
                    Else
                        ReturnValue = _DB.ExecuteScalar(sql, InputConn)
                    End If
                    If ReturnValue <> "" Then
                        KNSession(Quiz_Id & "|" & "CheckInStrquestionId") = ReturnValue 'Add ค่าให้กับตัวแปรนี้
                    End If
                Else 'ถ้าค่าที่ดึงได้ไม่เป็น Nothing ให้เอามาเช็คก่อนว่า QuestionId มีอยู่ใน Instr หรือยัง
                    If InStr(AllQuestionId, Question_Id) = 0 Then 'ถ้ายังไม่มีก็เข้า Store และ Add ค่าเข้าไปใน InStr
                        sql = " EXEC dbo.StoreSetQuizAnswer @Question_Id = '" & Question_Id & "', @Quiz_ID = '" & Quiz_Id & "' "
                        If UseTransaction = True Then
                            ReturnValue = _DB.ExecuteScalarWithTransection(sql, InputConn)
                        Else
                            ReturnValue = _DB.ExecuteScalar(sql, InputConn)
                        End If
                        If ReturnValue <> "" Then
                            MergeQuestionId = AllQuestionId & "," & ReturnValue
                            KNSession(Quiz_Id & "|" & "CheckInStrquestionId") = MergeQuestionId 'Add ค่าให้กับตัวแปรนี้
                        End If
                    End If
                End If

            End If

        Else 'ถ้าคำตอบไม่เหมือนกันเข้าเงื่อนไขนี้

            If QsetType = 6 Or QsetType = 3 Then 'เช็คว่าเป็น Type 6 หรือเปล่า ถ้าเป็น Type 6 ต้องส่ง QsetId เข้าไปด้วย
                Dim Qsetid As String = GetQsetIdFromQuestionId(Question_Id, UseTransaction, InputConn)
                If Qsetid = "" Then
                    Return "Error"
                End If
                'Function SaveAnswerForEachStudent  ไม่ใช้ Transaction
                If UseTransaction = True Then
                    If MiniFunctionSaveAnswerForEachStudent(Quiz_Id, Player_Id, Question_Id, True, _DB, True, Qsetid, InputConn) = "Error" Then
                        Return "Error"
                    End If
                Else
                    If SaveAnswerForEachStudent(Quiz_Id, Question_Id, True, Qsetid, Player_Id, InputConn) = "Error" Then
                        Return "Error"
                    End If
                End If

            Else ' ถ้าไม่เป็น Type 6 ไม่ต้องส่ง QsetId เข้าไป
                'Function SaveAnswerForEachStudent ไม่ใช้ Transaction
                If UseTransaction = True Then
                    If MiniFunctionSaveAnswerForEachStudent(Quiz_Id, Player_Id, Question_Id, True, _DB, True, "", InputConn) = "Error" Then
                        Return "Error"
                    End If
                Else
                    If SaveAnswerForEachStudent(Quiz_Id, Question_Id, True, , Player_Id, InputConn) = "Error" Then
                        Return "Error"
                    End If
                End If

            End If

        End If




        Return "Complete"

    End Function

    Public Function SaveAnswerForEachStudent(ByVal QuizId As String, ByVal QuestionId As String, ByVal IsRandomAnswer As Boolean, Optional ByVal Qset_Id As String = "", Optional ByVal Player_Id As String = "", Optional ByRef InputConn As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return "Error"
        End If

        '---------------------------------------------------------------------------------------------------------------------------------------------------------------

        If Player_Id = "" Then 'เช็คว่ามี Player_Id หรือยังถ้ายังไม่มีต้องหา Player_Id ก่อน
            Dim dtPlayerId As DataTable = GetPlayerIdFromQuizId(QuizId, InputConn)
            If dtPlayerId.Rows.Count > 0 Then
                _DB.OpenWithTransection(InputConn) 'เปิด Transaction 
                Dim EachPlayerId As String = ""
                For i = 0 To dtPlayerId.Rows.Count - 1
                    EachPlayerId = dtPlayerId.Rows(i)("Player_Id").ToString()
                    If MiniFunctionSaveAnswerForEachStudent(QuizId, EachPlayerId, QuestionId, IsRandomAnswer, _DB, True, Qset_Id, InputConn) = "Error" Then
                        _DB.RollbackTransection(InputConn) 'ถ้าเกิด Error ให้ RollBackTransaction
                        Return "Error"
                    End If
                Next
                _DB.CommitTransection(InputConn) 'CommitTransaction
            Else
                Return "Error"
            End If
        Else 'ถ้ามี Player_Id แล้วเข้าเงื่อนไขนี้
            If MiniFunctionSaveAnswerForEachStudent(QuizId, Player_Id, QuestionId, IsRandomAnswer, _DB, False, Qset_Id, InputConn) = "Error" Then
                Return "Error"
            End If
        End If

        Return "Complete"

    End Function

    Public Function MiniFunctionSaveAnswerForEachStudent(ByVal QuizId As String, ByVal PlayerId As String, ByVal QuestionId As String, ByVal IsRandomAnswer As Boolean, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, Optional ByVal QsetId As String = "", Optional ByRef InputConn As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or PlayerId Is Nothing Or PlayerId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return "Error"
        End If

        '------------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim sql As String = ""
        Dim dtAnswer As New DataTable
        Dim NewDb As New ClassConnectSql()
        If QsetId = "" Then 'ถ้าไม่มี QsetId แสดงว่าเป็นคำถามที่ไม่ใช่ Type 6

            Dim CheckType3 As String = ""
            sql = " SELECT tblQuestionSet.QSet_Type FROM tblQuestion INNER JOIN " &
                  " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " &
                  " WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
            If UseTransaction = True Then
                CheckType3 = _DB.ExecuteScalarWithTransection(sql, InputConn)
            Else
                CheckType3 = _DB.ExecuteScalar(sql, InputConn)
            End If

            If CheckType3 = "3" Then
                sql = " SELECT tblQuestion.Question_Id,tblAnswer.Answer_Id " &
                      " FROM   tblTestSetQuestionSet INNER JOIN " &
                      " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                      " tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id INNER JOIN " &
                      " tblAnswer ON tblTestSetQuestionDetail.Question_Id = tblAnswer.Question_Id INNER JOIN " &
                      " tblQuestion ON tblQuestionSet.QSet_Id = tblQuestion.QSet_Id INNER JOIN " &
                      " tblTestSet ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN " &
                      " tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id " &
                      " WHERE     (tblQuestion.Question_Id = '" & QuestionId & "') AND (tblQuiz.Quiz_Id = '" & QuizId & "') " &
                      " And tblTestSetQuestionSet.IsActive = '1' And = tblTestSetQuestionDetail.IsActive = '1'"

                '' check ค่าจาก db ด้วยว่า qset นี้ให้ random คำตอบด้วยหรือไม่
                Dim Qset_isRandomAnswer = getQSet_IsRandomAnswer(QuestionId, UseTransaction, InputConn)
                If IsRandomAnswer = True AndAlso Qset_isRandomAnswer Then
                    sql &= " ORDER BY NEWID() "
                Else
                    sql &= " ORDER BY Answer_Id "
                End If
            Else
                sql = " SELECT Question_Id,Answer_Id,AlwaysShowInLastRow FROM tblAnswer WHERE (Question_Id = '" & QuestionId & "') "
                '' check ค่าจาก db ด้วยว่า qset นี้ให้ random คำตอบด้วยหรือไม่
                Dim Qset_isRandomAnswer = getQSet_IsRandomAnswer(QuestionId, UseTransaction, InputConn)
                'หาก่อนว่า คำถามข้อนี้ ยอมให้สลับคำตอบได้หรือเปล่า
                Dim IsAllowedShuffleAnswer As Boolean = GetIsAllowedShuffleAnswer(QuestionId, UseTransaction, InputConn)
                If IsRandomAnswer = True AndAlso Qset_isRandomAnswer AndAlso IsAllowedShuffleAnswer = True Then 'ถ้าคำถามไม่เหมือนกันเติม Order By NewId() เพิ่ม
                    sql &= " ORDER BY AlwaysShowInLastRow,NEWID() "
                Else
                    sql &= " ORDER BY Answer_No "
                End If
            End If

            If UseTransaction = True Then
                dtAnswer = ObjDB.getdataWithTransaction(sql, , InputConn)
            Else
                dtAnswer = NewDb.getdata(sql, , InputConn)
            End If
            If CheckIsHaveInQuizAnswer(QuizId, PlayerId, dtAnswer.Rows(0)("Answer_Id").ToString(), UseTransaction, QuestionId, InputConn) = "0" Then
                If LoopInsertQuizAnswer(dtAnswer, QuizId, PlayerId, ObjDB, UseTransaction, InputConn) = "Error" Then 'วนลูป Insert ข้อมูลลง tblQuizAnswer
                    Return "Error"
                End If
            End If


        Else 'ถ้ามี QsetId ส่งมาแสดงว่าเป็นคำถามแบบ Type6
            sql = " SELECT tblAnswer.Answer_Id, tblQuizQuestion.Question_Id,tblAnswer.AlwaysShowInLastRow " &
                  " FROM tblAnswer INNER JOIN tblQuizQuestion ON tblAnswer.Question_Id = tblQuizQuestion.Question_Id " &
                  " WHERE (tblAnswer.QSet_Id = '" & QsetId & "') and  tblQuizQuestion.Quiz_Id = '" & QuizId & "' "

            Dim Qset_isRandomAnswer = ""
            If UseTransaction = True Then
                Qset_isRandomAnswer = _DB.ExecuteScalarWithTransection("SELECT QSet_IsRandomAnswer FROM tblQuestionSet WHERE QSet_Id = '" & QsetId & "';", InputConn)
            Else
                Qset_isRandomAnswer = _DB.ExecuteScalar("SELECT QSet_IsRandomAnswer FROM tblQuestionSet WHERE QSet_Id = '" & QsetId & "';", InputConn)
            End If

            If IsRandomAnswer = True AndAlso Qset_isRandomAnswer Then 'ถ้าคำถามไม่เหมือนกันเติม Order By NewId() เพิ่ม
                sql &= " ORDER BY AlwaysShowInLastRow,NEWID() "
            End If

            If UseTransaction = True Then
                dtAnswer = _DB.getdataWithTransaction(sql, , InputConn)
            Else
                dtAnswer = NewDb.getdata(sql, , InputConn)
            End If

            ' check type ก่อน insert ถ้าเป็น type3 ต้องสลับคำตอบตลอด 
            Dim QsetType As String = ""
            If UseTransaction = True Then
                QsetType = _DB.ExecuteScalarWithTransection(" SELECT QSet_Type FROM tblQuestionSet WHERE QSet_Id = '" & QsetId & "';", InputConn)
            Else
                QsetType = NewDb.ExecuteScalar(" SELECT QSet_Type FROM tblQuestionSet WHERE QSet_Id = '" & QsetId & "';", InputConn)
            End If

            'Dim QsetType = "3"

            'ดักก่อนว่ามี Add QuizAnswer รึยัง ถ้ายังค่อย Insert QuizAnswer
            If CheckIsHaveInQuizAnswer(QuizId, PlayerId, dtAnswer.Rows(0)("Answer_Id").ToString(), UseTransaction, , InputConn) = "0" Then
                If QsetType = "3" Then
                    sql &= " ORDER BY AlwaysShowInLastRow,NEWID(); "
                    Dim dtAnswerRandom As New DataTable
                    If UseTransaction = True Then
                        dtAnswerRandom = _DB.getdataWithTransaction(sql, , InputConn)
                    Else
                        dtAnswerRandom = NewDb.getdata(sql, , InputConn)
                    End If
                    If LoopInsertQuizAnswerType3(dtAnswer, QuizId, PlayerId, ObjDB, UseTransaction, dtAnswerRandom, InputConn) = "Error" Then
                        Return "Error"
                    End If
                Else
                    ' type 6 ใช่ loop เดิม
                    If LoopInsertQuizAnswer(dtAnswer, QuizId, PlayerId, ObjDB, UseTransaction, InputConn) = "Error" Then
                        Return "Error"
                    End If
                End If
            End If

        End If
        NewDb = Nothing
        Return "Complete"

    End Function

    Public Function LoopInsertQuizAnswer(ByVal dtAnswer As DataTable, ByVal QuizId As String, ByVal PlayerId As String, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)

        If dtAnswer.Rows.Count = 0 Or QuizId Is Nothing Or QuizId = "" Or PlayerId Is Nothing Or PlayerId = "" Then
            Return "Error"
        End If

        '----------------------------------------------------------------------------------------------------------------------------------------
        Dim QANumber As Integer = 1
        For i = 0 To dtAnswer.Rows.Count - 1 'วนลูป Insert ลง tblQuizAnswer
            Dim sql As String = " INSERT INTO dbo.tblQuizAnswer " &
                                " ( QuizAnswer_Id ,Quiz_Id ,Question_Id ,Answer_Id ,QA_No ,IsActive ,LastUpdate , Player_Id ) " &
                                " VALUES  ( NEWID() , '" & QuizId & "' , '" & dtAnswer.Rows(i)("Question_Id").ToString() & "' , '" & dtAnswer.Rows(i)("Answer_Id").ToString() & "' , " &
                                " '" & QANumber & "' , 1 , dbo.GetThaiDate() , '" & PlayerId & "') "

            Try
                If UseTransaction = True Then
                    ObjDB.ExecuteWithTransection(sql, InputConn)
                Else
                    ObjDB.Execute(sql, InputConn)
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return "Error"
            End Try
            QANumber += 1
        Next

        Return "Complete"

    End Function

    ' UPDATE เพิ่ม insert type 3 สลับคำตอบตลอด
    Public Function LoopInsertQuizAnswerType3(ByVal dtAnswer As DataTable, ByVal QuizId As String, ByVal PlayerId As String, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, ByVal dtAnswerRandom As DataTable, Optional ByRef InputConn As SqlConnection = Nothing)
        If dtAnswer.Rows.Count = 0 Or QuizId Is Nothing Or QuizId = "" Or PlayerId Is Nothing Or PlayerId = "" Then
            Return "Error"
        End If

        '----------------------------------------------------------------------------------------------------------------------------------------
        Dim QANumber As Integer = 1
        For i = 0 To dtAnswer.Rows.Count - 1 'วนลูป Insert ลง tblQuizAnswer
            Dim sql As String = " INSERT INTO dbo.tblQuizAnswer " &
                                " ( QuizAnswer_Id ,Quiz_Id ,Question_Id ,Answer_Id ,QA_No ,IsActive ,LastUpdate , Player_Id ) " &
                                " VALUES  ( NEWID() , '" & QuizId & "' , '" & dtAnswer.Rows(i)("Question_Id").ToString() & "' , '" & dtAnswerRandom.Rows(i)("Answer_Id").ToString() & "' , " &
                                " '" & QANumber & "' , 1 , dbo.GetThaiDate() , '" & PlayerId & "') "
            Try
                If UseTransaction = True Then
                    ObjDB.ExecuteWithTransection(sql, InputConn)
                Else
                    ObjDB.Execute(sql, InputConn)
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return "Error"
            End Try
            QANumber += 1
        Next

        Return "Complete"
    End Function


    Public Function GetPlayerIdFromQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim dtPlayerId As New DataTable
        If QuizId Is Nothing Or QuizId = "" Then
            Return dtPlayerId
        End If

        Dim sql As String = " SELECT Player_Id FROM dbo.tblQuizSession WHERE Quiz_Id = '" & QuizId & "' "
        dtPlayerId = _DB.getdata(sql, , InputConn)
        Return dtPlayerId

    End Function

    Public Function GetTop1QuestionIdByPlayerId(ByVal PlayerId As String, ByVal QuizId As String, ByVal UseTransaction As Boolean, ByVal PlayerType As String, ByVal QQ_No As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        If PlayerId Is Nothing Or PlayerId = "" Or QuizId Is Nothing Or QuizId = "" Then
            Return ""
        End If

        Dim QuestionId As String = ""
        Dim Sql As String = ""

        If PlayerType = "1" Then
            Sql = " SELECT Question_Id FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & QQ_No & "' "
        Else
            'Select QuestionId เพื่อ Insert แต่ละคน
            Sql = " SELECT dbo.tblQuizQuestion.Question_Id FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' " &
                  " AND Question_Id NOT IN (SELECT Question_Id FROM dbo.tblQuizScore  " &
                  " WHERE Quiz_Id = '" & QuizId & "' AND Student_Id = '" & PlayerId & "')  " &
                  " AND (dbo.tblQuizQuestion.QQ_No NOT IN " &
                  " (SELECT     tblQuizQuestion.QQ_No FROM         tblQuizScore INNER JOIN " &
                  " tblQuizQuestion ON tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id AND tblQuizScore.Question_Id = tblQuizQuestion.Question_Id " &
                  " WHERE (dbo.tblQuizScore.Quiz_Id = '" & QuizId & "') AND (tblQuizScore.Student_Id = '" & PlayerId & "') AND (dbo.tblQuizScore.IsActive = 1))) " &
                  " ORDER BY NEWID()  "
        End If

        If UseTransaction = True Then
            QuestionId = _DB.ExecuteScalarWithTransection(Sql, InputConn)
        Else
            QuestionId = _DB.ExecuteScalar(Sql, InputConn)
        End If

        Return QuestionId

    End Function

    Public Function CheckIsHaveInQuizAnswer(ByVal QuizId As String, ByVal PlayerId As String, ByVal AnswerId As String, ByVal UseTransaction As Boolean, Optional ByVal QuestionId As String = "", Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuizAnswer WHERE Quiz_Id = '" & QuizId & "' " &
                            " AND Player_Id = '" & PlayerId & "' " &
                            " AND Answer_Id = '" & AnswerId & "' "
        If QuestionId <> "" Then
            sql &= " AND Question_Id = '" & QuestionId & "' "
        End If
        Dim CheckResult As String = ""

        If UseTransaction = True Then
            CheckResult = _DB.ExecuteScalarWithTransection(sql, InputConn)
        Else
            CheckResult = _DB.ExecuteScalar(sql, InputConn)
        End If

        Return CheckResult

    End Function

    Public Function InsertQuizScore(ByVal QuizId As String, ByVal SchoolCode As String, ByVal Examnum As Integer, Optional ByRef InputConn As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or Examnum = 0 Then
            Return ""
        End If

        'ตัด Distinct ออกเพระคิดว่ายังไงก็ Select ได้ Type เดียวอยู่แล้ว
        Dim sql As String = " SELECT tblQuestionSet.QSet_Type FROM  tblQuizQuestion INNER JOIN " &
                            " tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                            " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " &
                            " WHERE     (tblQuizQuestion.Quiz_Id = '" & QuizId & "') AND (tblQuizQuestion.QQ_No = '" & Examnum & "') "

        Dim QsetType As String = _DB.ExecuteScalar(sql, InputConn)
        If QsetType = "" Then
            Return ""
        End If

        'ถ้าข้อสอบเป็น Type 6 ต้องวน Insert ทีละคน
        If QsetType = 6 Or QsetType = 3 Then
            If InsertQuizScoreType6(QuizId, SchoolCode, Examnum, InputConn) = "Error" Then
                Return ""
            End If
        Else
            'ถ้าข้อสอบไม่ใช่ Type 6 Insert ปกติ
            sql = " INSERT INTO dbo.tblQuizScore( QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id , " &
              " Answer_Id ,ResponseAmount ,FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID,QQ_No) " &
              " SELECT NEWID() AS Expr1, '" & QuizId & "' AS Expr2, '" & SchoolCode & "' AS Expr3, tblQuizQuestion.Question_Id, NULL AS Expr4, 0 AS Expr5, dbo.GetThaiDate() , " &
              " dbo.GetThaiDate() AS Expr6, 0 AS Expr7, 0 AS Expr8, 1 AS Expr9,tblQuizSession.Player_Id, t360_tblStudentRoom.SR_ID,'" & Examnum & "' " &
              " FROM tblQuizSession INNER JOIN tblQuizQuestion ON tblQuizSession.Quiz_Id = tblQuizQuestion.Quiz_Id INNER JOIN " &
              " t360_tblStudentRoom ON tblQuizSession.Player_Id = t360_tblStudentRoom.Student_Id " &
              " WHERE     (t360_tblStudentRoom.SR_IsActive = 1) AND (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "') "
        End If

        Try
            _DB.Execute(sql, InputConn) 'Insert ข้อมูลลง tblQuizScore
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

        Return "Complete"

    End Function

    Private Function InsertQuizScoreType6(ByVal QuizId As String, ByVal SchoolCode As String, ByVal Examnum As Integer, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT dbo.t360_tblStudent.Student_Id FROM tblQuiz INNER JOIN " &
                            " t360_tblStudent ON tblQuiz.t360_RoomName = t360_tblStudent.Student_CurrentRoom AND " &
                            " tblQuiz.t360_ClassName = t360_tblStudent.Student_CurrentClass AND " &
                            " tblQuiz.t360_SchoolCode = t360_tblStudent.School_Code " &
                            " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND (dbo.t360_tblStudent.Student_IsActive = 1) " &
                            " ORDER BY dbo.t360_tblStudent.Student_CurrentNoInRoom "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then

            sql = " SELECT TOP 1 Question_Id FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & Examnum & "' "
            Dim TopQuestionId As String = _DB.ExecuteScalar(sql, InputConn)
            If TopQuestionId = "" Then
                Return "Error"
            End If

            _DB.OpenWithTransection(InputConn)
            For index = 0 To dt.Rows.Count - 1

                Dim EachStudentId As String = dt.Rows(index)("Student_Id").ToString()
                sql = " SELECT t360_tblStudentRoom.SR_ID FROM t360_tblStudent INNER JOIN " &
                      " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id " &
                      " WHERE (t360_tblStudent.Student_Id = '" & EachStudentId & "') AND (t360_tblStudentRoom.SR_IsActive = 1) "
                Dim EachSR_ID As String = _DB.ExecuteScalarWithTransection(sql, InputConn)
                If EachSR_ID = "" Then
                    _DB.RollbackTransection(InputConn)
                    Return "Error"
                End If
                sql = " INSERT INTO dbo.tblQuizScore( QuizScore_Id , " &
                      " Quiz_Id ,School_Code ,Question_Id ,Answer_Id ,ResponseAmount ,FirstResponse , " &
                      " LastUpdate ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID ,QQ_No) " &
                      " VALUES  ( NEWID() , '" & QuizId & "' , '" & SchoolCode & "' , '" & TopQuestionId & "' , NULL ,  " &
                      " 0 , dbo.GetThaiDate() , dbo.GetThaiDate() , 0 , 0 ,  " &
                      " 1 , '" & EachStudentId & "' , '" & EachSR_ID & "' , '" & Examnum & "' ) "
                Try
                    _DB.ExecuteWithTransection(sql, InputConn)
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection(InputConn)
                    Return "Error"
                End Try
            Next

        Else
            Return "Error"
        End If

        _DB.CommitTransection(InputConn)
        Return "Complete"


    End Function

    Public Function InsertQuizScorePracticeMode(ByVal QuizId As String, ByVal SchoolCode As String, ByVal Examnum As Integer,
                                                ByVal IsUseComputer As Boolean, ByVal QsetType As String, Optional ByRef InputConn As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or Examnum = 0 Then
            Return ""
        End If

        Dim sql As String

        If IsUseComputer Then

            Dim player As String = HttpContext.Current.Session("UserId").ToString

            ' sql = "select '974DFFB5-E261-4E8B-B89B-981EF7CDC9B2' as SR_ID,'" & HttpContext.Current.Application("DefaultUserId") & "' as User_Id"
            sql = "select '974DFFB5-E261-4E8B-B89B-981EF7CDC9B2' as SR_ID,'" & player & "' as User_Id"
        Else
            sql = " SELECT t360_tblStudentRoom.SR_ID, tblQuiz.User_Id"
            sql &= " FROM t360_tblStudent INNER JOIN"
            sql &= " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN"
            sql &= " tblQuiz ON t360_tblStudent.Student_Id = tblQuiz.User_Id"
            sql &= " where Quiz_Id = '" & QuizId & "'  "
        End If


        Dim dt As DataTable = _DB.getdata(sql, , InputConn)

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
            _DB.Execute(sql, InputConn) 'Insert ข้อมูลลง tblQuizScore
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

        Return "Complete"

    End Function


    'INSERT INTO dbo.tblQuizScore(QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id ,  Answer_Id ,ResponseAmount , FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id , SR_ID,QQ_No)  
    ' SELECT     NEWID() , 'c4572c5f-c8de-445d-8766-7ac209a2cfa6', '1000001', Question_Id, NULL ,0,dbo.GetThaiDate() , 
    ' dbo.GetThaiDate() , 0 ,0 ,1 , 'student','sr','2' 
    'FROM         tblQuizQuestion
    '  where (tblQuizQuestion.QQ_No = '2') AND (tblQuizQuestion.Quiz_Id = 'c4572c5f-c8de-445d-8766-7ac209a2cfa6') 


    Public Function InsertQuizScoreWithTransaction(ByVal QuizId As String, ByVal SchoolCode As String, ByVal StudentId As String, ByVal QuestionId As String,
                                                   ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, ByVal ExamNum As String,
                                                   Optional ByRef InputConn As SqlConnection = Nothing, Optional ByRef IsSoundLab As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or StudentId Is Nothing Or StudentId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return ""
        End If

        Dim sql As String

        If ChkAndGetTabletLab(StudentId, QuizId) <> "" OrElse CheckQuizIsSpareTablet(HttpContext.Current.Session("PDeviceId"), InputConn) Then
            sql = "  INSERT INTO dbo.tblQuizScore   ( QuizScore_Id ,Quiz_Id ,School_Code  ,Question_Id ,Answer_Id ,  ResponseAmount ,FirstResponse " &
                    " ,LastUpdate  ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID,QQ_No)" &
                    " SELECT   TOP 1 NEWID() ,'" & QuizId & "' , '" & SchoolCode & "' ,'" & QuestionId & "'  ,NULL ,0  ," &
                    " dbo.GetThaiDate() ,dbo.GetThaiDate()  ,0 ,0 ,1 ,'" & StudentId & "',null,'" & ExamNum & "'" &
                    " FROM    tblQuizSession   INNER JOIN tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id " &
                    " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "')" &
                    " AND (dbo.tblQuizSession.Player_Id = '" & StudentId & "')"
        Else
            sql = "  INSERT INTO dbo.tblQuizScore   ( QuizScore_Id ,Quiz_Id ,School_Code  ,Question_Id ,Answer_Id ,  ResponseAmount ,FirstResponse ,LastUpdate " &
                          " ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID,QQ_No)  " &
                          " SELECT   TOP 1 NEWID() ,'" & QuizId & "' , '" & SchoolCode & "' ,'" & QuestionId & "' ,NULL ,0  ,dbo.GetThaiDate() ,dbo.GetThaiDate() " &
                          " ,0 ,0 ,1 ,'" & StudentId & "',t360_tblStudentRoom.SR_ID,'" & ExamNum & "'  FROM  t360_tblStudent INNER JOIN " &
                          " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN " &
                          " tblQuizSession INNER JOIN tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id ON t360_tblStudent.Student_Id = tblQuizSession.Player_Id " &
                          " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND (dbo.tblQuizSession.Player_Id = '" & StudentId & "') AND (dbo.t360_tblStudentRoom.SR_IsActive = 1) "
        End If

        Try
            If UseTransaction = True Then
                ObjDB.ExecuteWithTransection(sql, InputConn)
            Else
                ObjDB.Execute(sql, InputConn)
            End If

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

        Return "Complete"

    End Function

    Private Function ChkAndGetTabletLab(ByVal Player_id As String, ByVal Quiz_id As String) As String

        Dim sql As String
        sql = " select tabletlab_id from tbltabletlabdesk inner join t360_tbltablet on tbltabletlabdesk.tablet_Id = t360_tbltablet.tablet_id"
        sql &= " inner join tblquizsession on tblquizsession.tablet_id = t360_tbltablet.tablet_id "
        sql &= " where quiz_id = '" & Quiz_id & "'"
        sql &= " and player_id = '" & Player_id & "'"

        Dim TabletLab_Id As String = _DB.ExecuteScalar(sql).ToString

        Return TabletLab_Id

    End Function

    '' get israndomquestion
    Private Function getQset_IsRandomQuestion(ByVal QuestionId As String, Optional ByVal UseTransaction As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        If UseTransaction = False Then
            getQset_IsRandomQuestion = _DB.ExecuteScalar(" SELECT QSet_IsRandomQuestion FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
        Else
            getQset_IsRandomQuestion = _DB.ExecuteScalarWithTransection(" SELECT QSet_IsRandomQuestion FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
        End If
    End Function
    '' get israndomAnswer
    Private Function getQSet_IsRandomAnswer(ByVal QuestionId As String, Optional ByVal UseTransaction As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        If UseTransaction = False Then
            getQSet_IsRandomAnswer = _DB.ExecuteScalar(" SELECT QSet_IsRandomAnswer FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
        Else
            getQSet_IsRandomAnswer = _DB.ExecuteScalarWithTransection(" SELECT QSet_IsRandomAnswer FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "';", InputConn)
        End If
    End Function
    '' ต่อจากของ funtion InsertQuizScoreWithTransaction แต่จะเป็น insert ข้อสอบทั้ง qset เลย
    Public Function InsertQsetToQuizScoreWithTransaction(ByVal QuizId As String, ByVal SchoolCode As String, ByVal StudentId As String, ByVal QuestionId As String, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, ByVal ExamNum As String, ByVal QsetType As String, Optional ByRef InputConn As SqlConnection = Nothing)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or StudentId Is Nothing Or StudentId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return ""
        End If

        Dim sql As String = ""
        Dim sqlInsertQset As String = ""
        Dim dt As New DataTable
        If QsetType <> "6" Then

            Dim sqlTestsetId As String = "select testset_id from tblQuiz where Quiz_Id = '" & QuizId & "'"
            Dim TestsetId As String = _DB.ExecuteScalar(sqlTestsetId, InputConn).ToString

            Dim isDiffQuestion As Boolean = getQset_IsRandomQuestion(QuestionId, UseTransaction, InputConn)
            Dim orderBy As String = " ORDER BY TSQD_No; "
            If isDiffQuestion Then
                orderBy = " ORDER BY NEWID(); "
            End If

            sql = " SELECT Question_Id FROM tblTestSetQuestionDetail WHERE TSQS_Id IN ( "
            sql &= " SELECT tsqs.TSQS_Id FROM tblTestSetQuestionSet tsqs LEFT JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id "
            sql &= " WHERE tsqs.TestSet_Id = '" & TestsetId & "' AND  tsqd.Question_Id = '" & QuestionId & "'"
            sql &= " And tsqs.IsActive = '1' And tsqd.IsActive = '1') And tblTestSetQuestionDetail.IsActive = '1' " & orderBy
            dt = _DB.getdata(sql, , InputConn)

            Dim i As Integer = ExamNum
            For Each row As DataRow In dt.Rows
                sqlInsertQset &= strSqlInsertQuizScore(QuizId, SchoolCode, row(0).ToString(), StudentId, i)
                i = i + 1
            Next

        Else
            sqlInsertQset = strSqlInsertQuizScore(QuizId, SchoolCode, QuestionId, StudentId, ExamNum)
            dt.Columns.Add("QuestionId")
            Dim r As DataRow = dt.NewRow
            r("QuestionId") = QuestionId
            dt.Rows.Add(r)
        End If

        Try
            If UseTransaction = True Then
                ObjDB.ExecuteWithTransection(sqlInsertQset, InputConn)
            Else
                ObjDB.Execute(sqlInsertQset, InputConn)
            End If

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

        Return dt

    End Function

    Private Function strSqlInsertQuizScore(ByVal QuizId As String, ByVal SchoolCode As String, ByVal QuestionId As String, ByVal StudentId As String, ByVal ExamNum As String) As String
        strSqlInsertQuizScore = ("  INSERT INTO dbo.tblQuizScore   ( QuizScore_Id ,Quiz_Id ,School_Code  ,Question_Id ,Answer_Id ,  ResponseAmount ,FirstResponse ,LastUpdate " &
                                " ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID,QQ_No)  " &
                                " SELECT   TOP 1 NEWID() ,'" & QuizId & "' , '" & SchoolCode & "' ,'" & QuestionId & "' ,NULL ,0  ,dbo.GetThaiDate() ,dbo.GetThaiDate() " &
                                " ,0 ,0 ,1 ,'" & StudentId & "',t360_tblStudentRoom.SR_ID,'" & ExamNum & "'  FROM  t360_tblStudent INNER JOIN " &
                                " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id INNER JOIN " &
                                " tblQuizSession INNER JOIN tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id ON t360_tblStudent.Student_Id = tblQuizSession.Player_Id " &
                                " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND (dbo.tblQuizSession.Player_Id = '" & StudentId & "') AND (dbo.t360_tblStudentRoom.SR_IsActive = 1); ")
    End Function


    Public Function GetQSetTypeFromQuestionId(ByVal QuestionId As String, ByVal UseTransaction As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim QsetType As String = ""
        If QuestionId Is Nothing Or QuestionId = "" Then
            Return QsetType
        End If

        Dim sql As String = " SELECT QSet_Type FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "' "
        If UseTransaction = True Then
            QsetType = _DB.ExecuteScalarWithTransection(sql, InputConn)
        Else
            QsetType = _DB.ExecuteScalar(sql, InputConn)
        End If

        Return QsetType

    End Function

    Public Function GetQsetIdFromQuestionId(ByVal QuestionId As String, ByVal UseTransaction As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim QsetId As String = ""
        If QuestionId Is Nothing Or QuestionId = "" Then
            Return QsetId
        End If

        Dim sql As String = " SELECT Qset_Id FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "' "
        If UseTransaction = True Then
            QsetId = _DB.ExecuteScalarWithTransection(sql, InputConn)
        Else
            QsetId = _DB.ExecuteScalar(sql, InputConn)
        End If

        Return QsetId

    End Function

    Public Function GetSchoolCodeFromQuizIdWithTransaction(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim SchoolCode As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return SchoolCode
        End If

        Dim sql As String = " SELECT t360_SchoolCode FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "

        SchoolCode = _DB.ExecuteScalarWithTransection(sql, InputConn)

        Return SchoolCode

    End Function

    Public Function GetSchoolCodeFromQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim SchoolCode As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return SchoolCode
        End If

        Dim sql As String = " SELECT t360_SchoolCode FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
        SchoolCode = _DB.ExecuteScalar(sql, InputConn)

        Return SchoolCode

    End Function

    Public Function GetQuestionIdFromQuizIdAndExamNum(ByVal QuizId As String, ByVal ExamNum As Integer, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim QuestionId As String = ""
        If QuizId Is Nothing Or QuizId = "" Or ExamNum = 0 Then
            Return QuestionId
        End If

        Dim sql As String = " SELECT Question_Id from dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & ExamNum & "' "
        QuestionId = _DB.ExecuteScalar(sql, InputConn)
        Return QuestionId

    End Function

    Public Function GetTeacherIdFromQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim TeacherId As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return TeacherId
        End If

        Dim sql As String = " SELECT TOP 1 Player_Id from dbo.tblQuizSession WHERE Player_Type = 1 AND Quiz_Id = '" & QuizId & "' "
        TeacherId = _DB.ExecuteScalar(sql, InputConn)
        Return TeacherId

    End Function

    Public Function GetUserPracticeMode(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim Player_id As String

        Dim sql = "select user_id from tblquiz where quiz_id = '" & Quiz_Id & "'"

        Player_id = _DB.ExecuteScalar(sql, InputConn)
        Return Player_id

    End Function

    Public Function GetGroupNameFromQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) 'Function หา GroupName เพื่อเอาไปใช้ AddGroup ตอน SigNalR

        Dim GroupName As String = ""
        Dim dt As New DataTable
        If QuizId Is Nothing Or QuizId = "" Then
            Return GroupName
        End If

        'Dim sql As String = " SELECT t360_ClassName,t360_RoomName,TabletLab_Id FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
        Dim sql As String = " SELECT q.t360_ClassName,q.t360_RoomName,q.TabletLab_Id,r.Room_Id FROM tblQuiz q INNER JOIN t360_tblRoom r "
        sql &= " ON q.t360_ClassName = r.Class_Name AND REPLACE(q.t360_RoomName,'/','') = REPLACE(r.Room_Name,'/','')  "
        sql &= " AND q.t360_SchoolCode = r.School_Code WHERE q.Quiz_Id = '" & QuizId & "'; "

        dt = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("TabletLab_Id") Is DBNull.Value Then 'ถ้าไม่ได้เป็นห้อง SoundLab ดึงชั้นกับห้องมาเป็น GroupName ได้เลย
                'GroupName = dt.Rows(0)("t360_ClassName") & dt.Rows(0)("t360_RoomName")
                GroupName = dt.Rows(0)("Room_Id").ToString()
            Else 'ถ้าเป็นห้อง SoundLab หาชื่อห้อง SoundLab มาเป็น GroupName แทน
                'Dim TabletLabId As String = dt.Rows(0)("TabletLab_Id").ToString()
                'sql = " SELECT TabletLab_Name FROM dbo.tblTabletLab WHERE TabletLab_Id = '" & TabletLabId & "' "
                'GroupName = _DB.ExecuteScalar(sql, InputConn)
                GroupName = dt.Rows(0)("TabletLab_Id").ToString()
            End If
        Else
            Return GroupName
        End If

        Return GroupName

    End Function

    Public Function GetGroupNameByDVID(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) 'Function หา GroupName เพื่อเอาไปใช้ AddGroup ตอน SigNalR

        Dim GroupName As String = ""
        Dim dt As New DataTable
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return GroupName
        End If
        'Query แรกเพื่อหาก่อนว่า Tablet เครื่องนี้เป็นเครื่องประจำตัวนักเรียนหรือเปล่า
        Dim sql As String = " SELECT TOP 1 t360_tblStudent.Student_CurrentClass, t360_tblStudent.Student_CurrentRoom, t360_tblRoom.Room_Id " &
                            " FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id INNER JOIN  t360_tblStudent " &
                            " ON t360_tblTabletOwner.Owner_Id = t360_tblStudent.Student_Id INNER JOIN t360_tblRoom ON t360_tblStudent.Student_CurrentClass = t360_tblRoom.Class_Name " &
                            " AND REPLACE(t360_tblStudent.Student_CurrentRoom,'/','') = REPLACE(t360_tblRoom.Room_Name,'/','') AND t360_tblStudent.School_Code = t360_tblRoom.School_Code " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') " &
                            " AND(dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1)  ORDER BY (dbo.t360_tblTabletOwner.TON_ReceiveDate) desc "

        dt = _DB.getdata(sql, , InputConn)

        If dt.Rows.Count > 0 Then 'ถ้ามีมากกว่า 0 แสดงว่าเป็นเครื่องของนักเรียนให้ GroupName คือห้องของนักเรียนคนนี้
            'GroupName = dt.Rows(0)("Student_CurrentClass") & dt.Rows(0)("Student_CurrentRoom")
            GroupName = dt.Rows(0)("Room_Id").ToString()
        Else 'ถ้าไม่มีแสดงว่าเป็น Tablet ของห้อง Soundlab ต้องหาชื่อห้อง SoundLab อีกทีนึง
            sql = " SELECT tblTabletLab.TabletLab_Id FROM t360_tblTablet INNER JOIN " &
                  " tblTabletLabDesk ON t360_tblTablet.Tablet_Id = tblTabletLabDesk.Tablet_Id INNER JOIN " &
                  " tblTabletLab ON tblTabletLabDesk.TabletLab_Id = tblTabletLab.TabletLab_Id " &
                  " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') "
            GroupName = _DB.ExecuteScalar(sql, InputConn)
        End If

        Return GroupName

    End Function

    Private Function GetPlayerTypeByPlayerId(ByVal PlayerId As String, ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim PlayerType As String = ""
        If PlayerId Is Nothing Or PlayerId = "" Then
            Return PlayerType
        End If

        'Dim sql As String = " SELECT top 1 Player_Type FROM dbo.tblQuizSession WHERE Quiz_Id = '" & QuizId & "' AND Player_Id = '" & PlayerId & "' "
        'ใน email พี่ชิน บอกเอาออกแล้ว run เร็วขึ้น
        Dim sql As String = " SELECT Player_Type FROM dbo.tblQuizSession WHERE Quiz_Id = '" & QuizId & "' AND Player_Id = '" & PlayerId & "' "
        'PlayerType = _DB.ExecuteScalar(sql, InputConn)

        If HttpContext.Current.Application(QuizId & "|" & PlayerId & "|PlayerType") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(QuizId & "|" & PlayerId & "|PlayerType") = _DB.ExecuteScalar(sql, InputConn)
            HttpContext.Current.Application.UnLock()
        End If
        PlayerType = HttpContext.Current.Application(QuizId & "|" & PlayerId & "|PlayerType")

        If PlayerType = "" Then
            PlayerType = "1"
        End If

        Return PlayerType

    End Function


    '********************************************************************

    Public Sub getQsetInQuiz(ByVal testset_id As String, ByVal Quiz_Id As String)

        Dim db As New ClassConnectSql()

        Dim sqlGetQuestionInTestset As String = " SELECT tqs.QSet_Id,qs.QSet_Type FROM tblTestSetQuestionSet tqs LEFT JOIN tblQuestionSet qs "
        sqlGetQuestionInTestset &= " ON tqs.QSet_Id = qs.QSet_Id "
        sqlGetQuestionInTestset &= " WHERE tqs.TestSet_Id = '" & testset_id & "' And tqs.IsActive = '1' "

        Dim dtQset As New DataTable()

        'If (isDifferentQuestion) Then
        '    sqlGetQuestionInTestset &= " ORDER BY NEWID(); "
        '    dtQset = db.getdata(sqlGetQuestionInTestset)
        '    insertQuestionToQuizQuestion(dtQset, True, testset_id)
        'Else
        dtQset = db.getdata(sqlGetQuestionInTestset)
        insertQuestionToQuizQuestion(dtQset, testset_id, Quiz_Id)
        'End If

    End Sub

    ' <<< insert ข้อสอบ จาก qset >>>
    Private Sub insertQuestionToQuizQuestion(ByVal dtQset As DataTable, ByVal testset_id As String, ByVal Quiz_Id As String)

        Dim db As New ClassConnectSql()
        Dim qq_no As Integer = 1

        For i As Integer = 0 To dtQset.Rows.Count - 1
            Dim sqlQuestionInQset As String = " SELECT distinct tsqd.Question_Id FROM tblTestSetQuestionSet tsqs LEFT JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id "
            sqlQuestionInQset &= " WHERE tsqs.TestSet_Id = '" & testset_id & "' AND tsqs.QSet_Id = '" & dtQset(i)("QSet_Id").ToString() & "' AND tsqd.IsActive = '1' AND tsqs.IsActive = '1' " 'sql get question in qset

            Dim dtQuestionInQset As DataTable

            'If (isDiffQuestion) Then 'question ใน qset ต้องสุ่มหรือเปล่า
            '    sqlQuestionInQset &= " ORDER BY NEWID(); "
            '    dtQuestionInQset = db.getdata(sqlQuestionInQset)
            'Else
            dtQuestionInQset = db.getdata(sqlQuestionInQset)
            'End If

            db.OpenWithTransection()
            Dim sqlInsertQuestion As String = ""
            If dtQset.Rows(i)("QSet_Type") = "6" Or dtQset.Rows(i)("QSet_Type") = "3" Then
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No) VALUES('" & Quiz_Id & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion)
                Next
                qq_no = qq_no + 1
            Else
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No) VALUES('" & Quiz_Id & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion)
                    qq_no = qq_no + 1
                Next
            End If
            db.CommitTransection()
        Next

    End Sub

    Public Function UpdateIsScore(ByVal QuizId As String, ByVal ExamNum As String, Optional ByVal PlayerId As String = "", Optional ByRef InputConn As SqlConnection = Nothing)
        If QuizId = "" Or QuizId Is Nothing Or ExamNum Is Nothing Or ExamNum = "" Then
            Return "-1"
        End If
        Dim sql As String = " UPDATE dbo.tblQuizScore SET IsScored = 1,LastUpdate = dbo.GetThaiDate(),ClientId = NULL " &
                            " WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & ExamNum & "' "
        If PlayerId <> "" Then
            sql &= " AND Student_Id = '" & PlayerId & "' "
        End If
        Try
            _DB.Execute(sql, InputConn)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
        Return "Complete"
    End Function

    Public Function UpdateIsScoredTeacher(ByVal QuizId As String, ByVal ExamNum As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " UPDATE dbo.tblQuizQuestion SET IsScored_Teacher = 1,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & ExamNum & "' ;"
        Try
            _DB.Execute(sql, InputConn)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
        Return "Complete"
    End Function

    Public Sub UpdateScore(ByVal AnswerId As String, ByVal Quizid As String, ByVal QuestionId As String, ByVal StudentID As String)

        Dim sql As String

        sql = "select tblanswer.Answer_Score,tblanswer.Answer_ScoreMinus,tblquizscore.ResponseAmount,tblQuizScore.Score from tblanswer "
        sql &= " inner join tblquizscore on tblanswer.Answer_Id = tblQuizScore.Answer_Id where tblquizscore.quiz_id = '" & Quizid & "' "
        sql &= " and tblquizscore.Answer_Id = '" & AnswerId & "' and Student_Id = ' " & StudentID & "';"

        Dim dt As New DataTable

        dt = _DB.getdata(sql)

        Dim TotalScoreForUpdate As String
        Dim AnswerScore As String = dt.Rows(0)("Answer_Score")
        Dim Score As String = dt.Rows(0)("Score")
        Dim AnswerScoreMinus As String = dt.Rows(0)("Answer_ScoreMinus")

        If dt.Rows(0)("ResponseAmount") <> 0 Then
            If Score = "1" And AnswerScore = "0" Then
                TotalScoreForUpdate = ",TotalScore = TotalScore - " & Score
            ElseIf Score = "0" And AnswerScore = "1" Then
                TotalScoreForUpdate = ",TotalScore = TotalScore + " & AnswerScore
            End If
        Else
            TotalScoreForUpdate = ",TotalScore = TotalScore + " & AnswerScore
        End If

        sql = "update tblQuizScore set Answer_Id = '" & AnswerId & "',"
        sql &= " ResponseAmount = ResponseAmount +1,"
        sql &= " FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end),"
        sql &= " LastUpdate = dbo.GetThaiDate(),ClientId = NULL,"
        sql &= " Score = '" & Score & "', IsScored = 1 where Quiz_Id = '" & Quizid & "' and Question_Id = '" & QuestionId & "'"
        sql &= " and Student_Id = '" & StudentID & "'; "

        sql &= "update tblQuizSession set LastUpdate = dbo.GetThaiDate(),ClientId = NULL" & TotalScoreForUpdate
        sql &= " where Player_Id = '" & StudentID & "' and Quiz_Id = '" & Quizid & "';"

        _DB.Execute(sql)

    End Sub


    '-----------------------------------------------------------------------------------------------------------------

    Public Function GetScoreOfQuiz(ByVal Quiz_Id As String, ByVal Mode As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim sql As String

        sql = "select t360_ClassName,t360_roomName,TestSet_Id, Calendar_Id from tblQuiz where Quiz_Id = '" & Quiz_Id & "'"
        Dim dtQuizDetail As DataTable = _DB.getdata(sql, , InputConn)

        If Mode = "3" Then
            sql = " select isStandard from tblTestSet inner join tblQuiz on tblTestSet.TestSet_Id = tblQuiz.TestSet_Id "
            sql &= " where quiz_id = '" & Quiz_Id & "'"
            Dim IsStandard As Boolean = _DB.ExecuteScalar(sql, InputConn)

            If IsStandard = True Then
                sql = "  select tblTestSetQuestionSet.QSet_Id from tblTestSetQuestionSet inner join tblTestSet "
                sql &= " on tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id inner join tblQuiz "
                sql &= " on tblQuiz.TestSet_Id = tblTestSet.TestSet_Id "
                sql &= " where tblquiz.Quiz_Id = '" & Quiz_Id & "' And tblTestSetQuestionSet.IsActive = '1'"

                Dim QsetID As String = _DB.ExecuteScalar(sql, InputConn)

                sql = " select tblQuiz.FullScore,tblQuizSession.TotalScore,tblQuiz.Quiz_Id,tblQuizSession.Player_Id "
                sql &= " from tblquizSession inner join tblQuiz on tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id inner join tblTestSet "
                sql &= " on tblquiz.TestSet_Id = tbltestset.TestSet_Id inner join tblTestSetQuestionSet "
                sql &= " on tbltestset.TestSet_Id = tblTestSetQuestionSet.TestSet_Id"
                sql &= " and tbltestset.IsStandard = '1' and tblquiz.IsPracticeMode = '1' and tblQuiz.IsQuizMode = '0' "
                sql &= " and tblQuiz.IsHomeWorkMode = '0' and tblTestSetQuestionSet.QSet_Id in ('" & QsetID & "') And tbltestsetQuestionSet.IsActive = '1'"

            Else
                sql = " select tblQuiz.FullScore,CASE WHEN tblquizsession.TotalScore IS NOT NULL THEN dbo.tblQuizSession.TotalScore ELSE 0 END AS TotalScore,tblquiz.Quiz_ID,tblQuizSession.Player_Id "
                sql &= " from tblQuiz inner join tblQuizSession on tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id "
                sql &= " where TestSet_Id = '" & dtQuizDetail.Rows(0)("TestSet_Id").ToString & "' and t360_ClassName = '" & dtQuizDetail.Rows(0)("t360_ClassName").ToString & "' "
                sql &= " and t360_RoomName = '" & dtQuizDetail.Rows(0)("t360_roomName").ToString & "' and Calendar_Id = '" & dtQuizDetail.Rows(0)("Calendar_Id").ToString & "'"
            End If



        ElseIf Mode = "6" Then
            sql = " SELECT tblQuiz.FullScore, tblQuizSession.TotalScore, tblQuiz.Quiz_Id, tblQuizSession.Player_Id"
            sql &= " FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id"
            sql &= " where tblQuiz.Quiz_Id = '" & Quiz_Id & "'"
            sql &= " and FullScore is not null and TotalScore is not null;"
        Else
            sql = " SELECT tblQuiz.FullScore, tblQuizSession.TotalScore, tblQuiz.Quiz_Id, tblQuizSession.Player_Id"
            sql &= " FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id"
            sql &= " where tblQuiz.Quiz_Id = '" & Quiz_Id & "'"
            sql &= " and Player_Type  = '2' and FullScore is not null and TotalScore is not null;"
        End If
        GetScoreOfQuiz = _DB.getdata(sql, , InputConn)

    End Function

    Public Function GetTotalScore(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        ' sum Score ของ testset ที่เล่นควิซอยู่ออกมา ได้คะแนนเต็มของข้อสอบชุดนั้น
        Dim sql As String = ""
        Dim dtQset As New DataTable
        Dim TotalScore As Integer = 0
        Dim QsetScore As Integer = 0

        'sql = "SELECT sum(tblAnswer.Answer_Score) as TotalScore"
        'sql &= " FROM   tblAnswer INNER JOIN"
        'sql &= " tblQuestion ON tblAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
        'sql &= "  tblTestSetQuestionDetail ON tblQuestion.Question_Id = tblTestSetQuestionDetail.Question_Id INNER JOIN"
        'sql &= " tblTestSetQuestionSet ON tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_Id INNER JOIN"
        'sql &= "  tblQuiz ON tblTestSetQuestionSet.TestSet_Id = tblQuiz.TestSet_Id "
        'sql &= " where quiz_id = '" & Quiz_Id & "' and answer_score <> 0"

        'GetTotalScore = _DB.ExecuteScalar(sql)

        sql = "select distinct TbltestsetQuestionset.QSet_Id,QSet_Type from tblTestSetQuestionSet inner join tblQuestionSet on tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id "
        sql &= " where TestSet_Id in (select TestSet_Id from tblQuiz where Quiz_Id ='" & Quiz_Id & "') And tblTestsetQuestionSet.IsActive = '1';"
        dtQset = _DB.getdata(sql, , InputConn)

        For Each a In dtQset.Rows
            If a("QSet_Type").ToString = "6" Or a("QSet_Type").ToString = "3" Then
                TotalScore += 1
            Else
                sql = "SELECT SUM(tblAnswer.Answer_Score) AS TotalScore"
                sql &= " FROM tblAnswer INNER JOIN"
                sql &= " tblQuestion ON tblAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
                sql &= " tblTestSetQuestionDetail ON tblQuestion.Question_Id = tblTestSetQuestionDetail.Question_Id INNER JOIN"
                sql &= " tblTestSetQuestionSet ON tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_Id INNER JOIN"
                sql &= " tblQuiz ON tblTestSetQuestionSet.TestSet_Id = tblQuiz.TestSet_Id"
                sql &= " WHERE  tblQuiz.Quiz_Id = '" & Quiz_Id & "' "
                sql &= " AND tblTestSetQuestionSet.QSet_Id = '" & a("QSet_Id").ToString & "'"
                sql &= " And tblTestsetQuestionSet.IsActive = '1' and tblTestsetQuestionDetail.IsActive = '1' and tblAnswer.IsActive = 1"
                QsetScore = _DB.ExecuteScalar(sql, InputConn)
                TotalScore += QsetScore
            End If
        Next

        'sql = " SELECT SUM(Score) as TotalScore FROM dbo.tblQuizScore WHERE Quiz_Id = '" & Quiz_Id & "' AND IsActive = 1 "
        'dtQset = _DB.getdata(sql)

        'TotalScore = dtQset(0)("TotalScore")

        Return TotalScore

    End Function

    Public Function GetScoreOfPlayer(ByVal Quiz_Id As String, ByVal PlayerId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim sql As String

        sql = "select SUM(score)as MySc from tblquizscore where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & PlayerId & "' "
        sql &= " group by student_id"

        GetScoreOfPlayer = _DB.ExecuteScalar(sql, InputConn)

    End Function

    Public Function GetExamNum(ByVal Quiz_Id As String, Optional ByVal Player_Id As String = "") As String 'หาว่าตอนนี้ทำไปถึงข้อไหน เพื่่อกันกด refresh แล้วกลับไปข้อ 1

        Dim sql As String
        sql = "select max(QQ_No)from tblquizscore where Quiz_Id = '" & Quiz_Id & "'"
        If Player_Id <> "" Then
            sql &= " and Student_Id = '" & Player_Id & "'"
        End If

        GetExamNum = _DB.ExecuteScalar(sql)

        If GetExamNum = "" Then
            GetExamNum = "1"
        End If

    End Function

    Sub UpdateModule(ByVal Quiz_Id As String)
        Throw New NotImplementedException
    End Sub

    Public Function IsGroupSubjectEng(ByVal QuestionId As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim db As New ClassConnectSql()
        Dim sql As New StringBuilder()
        sql.Append(" SELECT TOP 1 tgs.GroupSubject_Name FROM tblQuestion tq INNER JOIN tblQuestionSet tqs ON tq.QSet_Id = tqs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory tqc ON tqs.QCategory_Id = tqc.QCategory_Id ")
        sql.Append(" INNER JOIN tblbook tb ON tqc.Book_Id = tb.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject tgs ON tb.GroupSubject_Id = tgs.GroupSubject_Id ")
        sql.Append(" WHERE tq.Question_Id = '")
        sql.Append(QuestionId)
        sql.Append("';")

        Dim G_Subject As String = db.ExecuteScalar(sql.ToString(), InputConn)
        If G_Subject = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetdtDetailQuizByStudentId(ByVal QuizId As String, ByVal StudentId As String) As DataTable

        Dim sql As String = " SELECT ( CAST(tblQuizSession.Score AS VARCHAR(max)) + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) AS Score, " &
                            " tblQuizSession.IsAllAnswered,dbo.tblQuiz.StartTime, (MAX(tblQuizScore.LastUpdate)  - tblQuiz.StartTime)AS TimeTotal " &
                            " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " &
                            " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND tblQuizScore.Student_Id = tblQuizSession.Player_Id " &
                            " WHERE  (tblQuiz.Quiz_Id = '" & QuizId & "') AND " &
                            " (tblQuizScore.Student_Id = '" & StudentId & "') " &
                            " GROUP BY ( CAST(tblQuizSession.Score AS VARCHAR(max)) + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) " &
                            " , tblQuizSession.IsAllAnswered,dbo.tblQuiz.StartTime "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Public Function GetArrayQuizProperty(ByVal QuizId As String, IsStudent As Boolean)

        Dim ArrProperty As New ArrayList
        Dim sql As String = " SELECT  StartTime,case when EndTime is not null then EndTime else (Select max(LastUpdate) from tblquizscore where Quiz_Id = tblQuiz.Quiz_Id) end as EndTime,NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,NeedCorrectAnswer " &
                            " ,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,IsShowCorrectAfterComplete,NeedRandomQuestion,NeedRandomAnswer " &
                            " FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            'เปิดควิซ
            Dim StartDate As String = Format(CDate(dt.Rows(0)("StartTime")), "dd/MM/yyyy")
            Dim StartTime As String = Format(CDate(dt.Rows(0)("StartTime")), "HH:mm")
            Dim EndTime As String = ""
            If dt.Rows(0)("EndTime") IsNot DBNull.Value Then
                EndTime = Format(CDate(dt.Rows(0)("EndTime")), "HH:mm")
            End If

            Dim OpenQuizTime As String

            OpenQuizTime = ""

            If IsStudent Then
                OpenQuizTime = "ครูเริ่มควิซเมื่อ "
            Else
                OpenQuizTime = "เปิดควิซเมื่อ "
            End If
            OpenQuizTime &= StartDate.ToString() & " " & StartTime.ToString()

            If EndTime <> "" Then
                OpenQuizTime &= "-" & EndTime.ToString()
            End If

            ArrProperty.Add(OpenQuizTime)

            'จับเวลา
            If dt.Rows(0)("NeedTimer") = True Then
                If dt.Rows(0)("IsPerQuestionMode") = True Then
                    ArrProperty.Add("จับเวลาข้อต่อข้อ ข้อละ" & dt.Rows(0)("TimePerQuestion").ToString() & " วินาที")
                Else
                    ArrProperty.Add("จับเวลาทั้งหมด " & dt.Rows(0)("TimePerTotal").ToString() & "นาที")
                End If
            Else
                ArrProperty.Add("ไม่จับเวลา")
            End If
            'แสดงเฉลยหรือเปล่า
            If dt.Rows(0)("NeedCorrectAnswer") = True Then
                'แสดงเฉลยตอนสุดท้ายหรือเปล่า
                If dt.Rows(0)("IsShowCorrectAfterComplete") = True Then
                    ArrProperty.Add("แสดงเฉลยตอนสุดท้าย")
                Else
                    'จับเวลาแสดงเฉลยหรือเปล่า
                    If dt.Rows(0)("IsTimeShowCorrectAnswer") = True Then
                        ArrProperty.Add("แสดงเฉลยข้อละ " & dt.Rows(0)("TimePerCorrectAnswer").ToString() & " วินาที")
                    Else
                        ArrProperty.Add("แสดงเฉลยข้อต่อข้อ")
                    End If
                End If
            Else
                ArrProperty.Add("ไม่แสดงเฉลย")
            End If
            'สลับคำถามหรือเปล่า
            If dt.Rows(0)("NeedRandomQuestion") = True Then
                ArrProperty.Add("สลับคำถาม")
            Else
                ArrProperty.Add("ไม่สลับคำถาม")
            End If
            'สลับคำตอบหรือเปล่า
            If dt.Rows(0)("NeedRandomAnswer") = True Then
                ArrProperty.Add("สลับคำตอบ")
            Else
                ArrProperty.Add("ไม่สลับคำตอบ")
            End If
            Return ArrProperty
        Else
            Return ArrProperty
        End If

    End Function

    Public Function GetStatusQuiz(Quiz_Id As String, StudentId As String, Mode As EnumDashBoardType, Optional TimeExitedByUser As String = "") As String
        Dim sql As String
        sql = " select tblQuiz.Quiz_id,tblQuizSession.Player_Id,tblQuizSession.IsActive as isDoQuiz,Count(distinct tblQuizQuestion.QQ_No) as QuestionAmount ," &
              " (select Sum(case when tblquizscore.Answer_Id is null then 0 else 1 end)  from tblQuizScore where Quiz_Id = tblquiz.Quiz_Id and tblQuizScore.Student_Id = tblQuizSession.Player_Id) as AnsweredAmount " &
              " from tblquiz left Outer join tblQuizSession on tblquiz.Quiz_Id = tblQuizSession.Quiz_Id" &
              " left outer join tblQuizQuestion on tblQuiz.Quiz_Id = tblQuizQuestion.Quiz_Id" &
              " where tblquiz.Quiz_Id = '" & Quiz_Id & "'" &
              " and tblQuizSession.Player_Id = '" & StudentId & "'" &
              " group by tblQuiz.Quiz_id,tblQuizSession.Player_Id,tblQuizSession.IsActive"

        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        Dim StrReturn As String
        If dt.Rows.Count = 0 Then
            StrReturn = "นร.ยังไม่ได้เข้าทำเลย"
        ElseIf dt.Rows(0)("IsDoQuiz") = 0 Then
            StrReturn = "นร.ยังไม่ได้เข้าทำเลย"
        Else
            If dt.Rows(0)("AnsweredAmount") = 0 Then
                StrReturn = "ไม่ได้กดตอบ"
            Else
                If dt.Rows(0)("QuestionAmount") = 1 Then
                    StrReturn = "ทำแล้ว"
                Else
                    If dt.Rows(0)("QuestionAmount") = dt.Rows(0)("AnsweredAmount") Then
                        StrReturn = "ทำครบทุกข้อ"
                    Else
                        StrReturn = "ทำไม่ครบข้อ"
                    End If
                End If
            End If


            If Mode = EnumDashBoardType.Homework Then
                If TimeExitedByUser = "" Then
                    StrReturn &= "-ยังไม่ส่ง"
                Else
                    StrReturn &= "-ส่งแล้ว"
                End If
            End If
        End If
        Return StrReturn
    End Function

    Public Function GetTimeDoQuiz(Quiz_Id As String, Student_Id As String)
        Dim sql As String = " select min(FirstResponse) as StartDoQuiz,sum(TimeTotal) as TotalTimeDoQuiz" &
                            " from tblQuizScore where Quiz_Id = '" & Quiz_Id & "'" &
                            " and Student_Id = '" & Student_Id & "'"
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        Dim strReturn As String = ""
        If dt.Rows.Count <> 0 Then
            If dt.Rows(0)("TotalTimeDoQuiz") = 0 Then
                strReturn = "เริ่มทำ " & CDate(dt.Rows(0)("StartDoQuiz")).ToPointPlusTime
            Else
                strReturn = "เริ่มทำ " & CDate(dt.Rows(0)("StartDoQuiz")).ToPointPlusTime & "<br> ใช้เวลาทำไป " & GetStringDurationTime(dt.Rows(0)("TotalTimeDoQuiz"))
            End If
        End If

        Return strReturn
    End Function

    Public Function GetStringDurationTime(ByVal TotalTime As Integer)

        Dim StrReturn As String = ""
        Dim MinuteTime As Integer
        Dim SecondTime As Integer

        MinuteTime = TotalTime \ 60
        SecondTime = TotalTime Mod 60

        If MinuteTime <> 0 Then
            StrReturn = MinuteTime & " นาที "
        End If

        If SecondTime <> 0 Then
            StrReturn &= SecondTime & " วิ. "
        End If

        Return StrReturn

    End Function

    Public Function GetPlayerIdByDeviceId(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim PlayerId As String = ""
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return PlayerId
        End If
        Dim sql As String = " SELECT TOP 1 t360_tblTabletOwner.Owner_Id FROM t360_tblTablet INNER JOIN " &
                            " t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
                            " WHERE  (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) "
        PlayerId = _DB.ExecuteScalar(sql, InputConn)
        Return PlayerId

    End Function

    Public Sub SetStatusQuiz(ByVal Quiz_Id As String, ByVal Player_Id As String, ByVal Status As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String

        sql = "Update tblQuizSession set SessionStatus = '" & Status & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL where Quiz_Id = '" & Quiz_Id & "' AND Player_Id = '" & Player_Id & "';"

        _DB.Execute(sql, InputConn)

    End Sub

    Public Sub SetTotalTime(ByVal StartTime As Date, ByVal EndTime As Date, ByVal Quiz_Id As String, ByVal ExamNum As String, ByVal Player_id As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String
        Dim TotalTime As Integer

        TotalTime = DateDiff(DateInterval.Second, StartTime, EndTime)

        sql = " Update tblQuizScore Set TimeTotal = Timetotal + " & TotalTime & ",Lastupdate = dbo.GetThaiDate(),ClientId = NULL "
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and QQ_No = '" & ExamNum & "' and Student_Id = '" & Player_id & "';"

        _DB.Execute(sql, InputConn)

    End Sub

    'Public Sub SetTotalScore(ByVal Quiz_Id As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)
    '    Dim sql As String

    '    sql = " update tblQuizSession set LastUpdate = dbo.GetThaiDate(),ClientId = NULL,TotalScore = "
    '    sql &= " (select SUM(Score)as score from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' "
    '    sql &= " and student_Id = '" & Player_Id & "') "
    '    sql &= " where Player_Id = '" & Player_Id & "' and Quiz_Id = '" & Quiz_Id & "' ; "

    '    _DB.Execute(sql, InputConn)

    'End Sub

    Public Function GetFirstLeapChoice(ByVal Quiz_Id As String) As String

        Dim sql As String
        sql = "select min(qq_no)as FirstLeapChoice from tblquizquestion where Question_Id in "
        sql &= " (select Question_Id from tblQuizScore where Answer_Id is null and Quiz_Id = '" & Quiz_Id & "')"
        sql &= " and Quiz_Id = '" & Quiz_Id & "' "

        Dim FirstLeapChoice As String = _DB.ExecuteScalar(sql)

        If FirstLeapChoice IsNot Nothing And FirstLeapChoice <> "" Then
            GetFirstLeapChoice = FirstLeapChoice
        Else
            GetFirstLeapChoice = "0"
        End If

    End Function

    Public Function GetCompleteHomework(ByVal Quiz_Id As String, ByVal Player_Id As String) As Boolean

        Dim sql As String

        sql = "select (case when TimeExitedByUser is NULL then 0 else 1 end)as Complete from tblModuleDetailCompletion "
        sql &= " where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "'"

        Dim IsComplete As Boolean = _DB.ExecuteScalar(sql)

        Return IsComplete

    End Function

    Public Function GetNotReplyNum(ByVal Quiz_id As String, ByVal Player_id As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = ""
        sql = " SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & Quiz_id & "'"
        sql &= " AND Student_Id = '" & Player_id & "' AND Answer_Id IS NULL ORDER BY Answer_Id,QQ_No"

        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        Return dt
    End Function

    Public Function GetNextChoiceAfterReply(ByVal Quiz_Id As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = ""
        sql = "  select  cast(max(QQ_no) as int)+1 as QNum from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' and Student_Id= '" & Player_Id & "'"
        Dim NextChoice As String = _DB.ExecuteScalar(sql, InputConn)

        Return NextChoice
    End Function

    Public Function CheckQuizIsSoundLab(ByVal Quizid As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & Quizid.ToString() & "' and TabletLab_Id is not null "
        Dim CheckCount As String = _DB.ExecuteScalar(sql, InputConn)
        If CInt(CheckCount) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CheckQuizIsSpareTablet(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim dtTabletSpare As DataTable = _DB.getdata(String.Format("SELECT * FROM t360_tblTablet WHERE Tablet_IsActive = 1 AND Tablet_SerialNumber = '{0}';", DeviceId))
        If dtTabletSpare.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function GetTotalScoreForChoiceToChoice(ByVal Quiz_Id As String, ByVal Player_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String
        sql = " select cast(TotalScore as int) as TotalScore from tblQuizSession where Quiz_Id = '" & Quiz_Id & "' and Player_Id = '" & Player_Id & "'"
        Dim TotalScore As String = _DB.ExecuteScalar(sql, InputConn)
        Return TotalScore

    End Function

    'Public Function TeacherSetTotalScore(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)

    '    Dim sql As String

    '    sql = " select Player_Id from tblQuizSession where Quiz_Id = '" & Quiz_Id & "' and IsActive = '1';"
    '    Dim dt As DataTable = _DB.getdata(sql, , InputConn)

    '    If Not dt.Rows.Count = 0 Then
    '        For Each i In dt.Rows
    '            sql = "update tblQuizSession set LastUpdate = dbo.GetThaiDate() ,TotalScore ="
    '            sql &= " (select SUM(Score)as score from tblQuizScore where Quiz_Id = '" & Quiz_Id & "' "
    '            sql &= " and student_Id = '" & i("Player_Id").ToString & "')"
    '            sql &= " where Player_Id = '" & i("Player_Id").ToString & "' and Quiz_Id = '" & Quiz_Id & "' ;"
    '            _DB.Execute(sql, InputConn)
    '        Next
    '    End If

    'End Function

    Public Function GetDNTExam(ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String
        sql = "select cast(MAX(qq.qq_no) as int)  - cast(MAX(qs.qq_no)as int) as DNTExam "
        sql &= " from tblQuizQuestion qq inner join tblQuizScore qs on qq.Quiz_Id = qs.Quiz_Id "
        sql &= " where qq.quiz_id = '" & Quiz_Id & "' "
        Dim DNTExam As String = _DB.ExecuteScalar(sql, InputConn)

        Return DNTExam
    End Function

    Public Function GetGroupNameForSpareTablet(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim GroupName As String = ""
        Dim dt As New DataTable
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return GroupName
        End If

        Dim sql As String
        'sql = "SELECT top 1 tblQuiz.t360_ClassName + tblQuiz.t360_RoomName"
        'sql &= " FROM t360_tblTablet INNER JOIN"
        'sql &= " tblQuizSession ON t360_tblTablet.Tablet_Id = tblQuizSession.Tablet_Id INNER JOIN"
        'sql &= " tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id "
        'sql &= " where t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "'"
        'sql &= " order by tblquiz.LastUpdate desc"

        sql = " SELECT TOP 1 s.Student_CurrentRoomId FROM tblQuizSession qz INNER JOIN t360_tblTablet t ON qz.Tablet_Id = t.Tablet_Id "
        sql &= " INNER JOIN tblQuiz q ON q.Quiz_Id = qz.Quiz_Id"
        sql &= " INNER JOIN t360_tblStudent s ON qz.Player_Id = s.Student_Id"
        sql &= " WHERE t.Tablet_SerialNumber =  '" & DeviceId & "' " '--AND q.EndTime IS NULL"
        sql &= " ORDER BY q.LastUpdate DESC;"


        GroupName = _DB.ExecuteScalar(sql, InputConn)

        Return GroupName

    End Function

    Public Function GetTabletLab(SchoolCode As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String
        sql = " select Count(TabletLab_Id)as TabletLabAmount from tbltabletlab where School_Code = '" & SchoolCode & "' and IsActive= '1'"

        Dim TabletLabAmount As String = _DB.ExecuteScalar(sql, InputConn)

        Return TabletLabAmount

    End Function

    Public Function GetDDLLab(ByVal SchoolCode As String, Optional ByRef StudentAmount As String = Nothing, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String

        If StudentAmount IsNot Nothing Then
            sql = " select tblTabletLab.TabletLab_Id,tblTabletLab.TabletLab_Name " &
                    " from tblTabletLab inner join tblTabletLabDesk  on tblTabletLab.TabletLab_Id = tblTabletLabDesk.TabletLab_Id " &
                    " where School_Code = '" & SchoolCode & "' and tblTabletLab.IsActive = '1' and tblTabletLabDesk.IsActive = '1' " &
                    " and tblTabletLabDesk.Player_Type = '2' and tblTabletLabDesk.IsActive = '1'" &
                    " group by tblTabletLab.TabletLab_Id,tblTabletLab.TabletLab_Name" &
                    " having COUNT(tblTabletLabDesk.Tablet_Id) >= '" & StudentAmount & "' order by tbltabletlab.TabletLab_Name"
        Else
            sql = " select TabletLab_Id,TabletLab_Name from tblTabletLab where School_Code = '" & SchoolCode & "' and IsActive = 1 "
        End If

        Dim dt As New DataTable

        dt = _DB.getdata(sql, , InputConn)

        'If dt.Rows.Count > 0 Then
        '    For index = 0 To dt.Rows.Count - 1
        '        DDLSoundLabName.Items.Add(dt.Rows(index)("TabletLab_Name").ToString())
        '        DDLSoundLabName.Items(index).Value = dt.Rows(index)("TabletLab_Id").ToString()
        '    Next
        'Else
        '    DDLSoundLabName.Items.Add("ไม่มีห้อง Soundlab")
        'End If
        'DDLSoundLabName.SelectedIndex = 0

        Return dt
    End Function

    Public Function CheckAndAddValueToArrayExamNum(ByVal InputArray As ArrayList, ByVal InputExamNum As Integer) As ArrayList
        If InputArray Is Nothing Then
            InputArray = New ArrayList
        End If
        If InputArray IsNot Nothing And InputExamNum <> 0 Then
            If InputArray.Contains(InputExamNum) = False Then
                InputArray.Add(InputExamNum)
            End If
        End If
        Return InputArray
    End Function

    Public Function CheckExamNumIsContainInArray(ByVal InputArray As ArrayList, ByVal InputExamNum As Integer) As Boolean
        If InputArray IsNot Nothing Then
            If InputArray.Contains(InputExamNum) = True Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
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

    Private Function ThisQuestionAllowedShuffleAnswer(Quiz_Id As String, ExamNum As String, InputConn As SqlConnection) As Boolean
        Dim sql As String = " SELECT NoChoiceShuffleAllowed FROM dbo.tblQuizQuestion INNER JOIN dbo.tblQuestion ON " &
                            " dbo.tblQuizQuestion.Question_Id = dbo.tblQuestion.Question_Id WHERE Quiz_Id = '" & Quiz_Id & "' AND QQ_No = '" & ExamNum & "'; "
        Dim CheckValue As Boolean = CType(_DB.ExecuteScalar(sql, InputConn), Boolean)
        If CheckValue = False Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub UpdateIsHalfWay(QuizId As String)
        Dim sql As String = "Update tblquiz set Ishalfway = '1' where quiz_id = '" & QuizId & "'"
        _DB.Execute(sql)
    End Sub

    Public Function GetIsAnsweredType3(QuizId As String, question_id As String, PlayerId As String) As Boolean
        Dim sql As String = "select ResponseAmount from tblquizscore where Quiz_Id  = '" & QuizId & "'" &
                            " And Student_Id = '" & PlayerId & "' and Question_Id = '" & question_id & "';"
        Dim IsAnswered As String = _DB.ExecuteScalar(sql)

        If IsAnswered = "0" Then
            Return False
        Else
            Return True
        End If

    End Function

    'เช็คว่าใน Testset นี้ มี Qset ที่ห้ามสุ่มคำถาม-คำตอบ หรือไม่
    Public Function CheckQsetRandomSettingByTestset(TestsetId As String) As DataTable
        Dim sql As New StringBuilder

        sql.Append(" select sum(cast (QSet_IsRandomQuestion as tinyint)) as RandomQuestion ,sum(cast(QSet_IsRandomAnswer as tinyint)) as RandomAnswer,")
        sql.Append(" sum(cast(NoChoiceShuffleAllowed as tinyint)) as Shuffle,Count(Question_Id) as QuestionAmount")
        sql.Append(" from tblTestSetQuestionSet inner join tblQuestionset on tblTestSetQuestionSet.QSet_Id = tblQuestionset.QSet_Id")
        sql.Append(" inner join tblquestion on tblQuestionset.QSet_Id = tblQuestion.QSet_Id")
        sql.Append(" where TestSet_Id = '")
        sql.Append(TestsetId)
        sql.Append("' and tblQuestionset.IsActive = '1' and tblQuestion.IsActive = '1' and tblTestSetQuestionSet.IsActive = '1'")

        Dim dt As DataTable
        dt = _DB.getdata(sql.ToString)
        Return dt
    End Function

End Class