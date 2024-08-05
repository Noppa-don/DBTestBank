Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Imports KnowledgeUtils

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ActivityService
    Inherits System.Web.Services.WebService

    Public js As New JavaScriptSerializer()

    <WebMethod(EnableSession:=True)> _
    Public Function GetTotalStudentInQuiz() ' get จำนวนนักเรียน (function ของครู)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As String = " SELECT StudentAmount FROM dbo.tblQuiz WHERE Quiz_Id = '" & Quiz_Id & "'; "
        Dim TotalStudent As String = _DB.ExecuteScalar(sql)
        If TotalStudent <> "" Then
            Return TotalStudent
        Else
            Return 0
        End If
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetStudentDontReply(ByVal ExamNum As String) ' get จำนวนคนยังไม่ตอบใน Quiz
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As String
        sql = " SELECT t360_tblStudent.Student_CurrentNoInRoom as Student_CurrentNoInRoom FROM tblQuizScore INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id "
        sql += "WHERE tblQuizScore.Student_Id IN (SELECT Player_Id FROM tblQuizSession WHERE IsActive = '1' AND Player_Type = '2' AND Quiz_Id = '" & Quiz_Id & "')"
        sql += "AND tblQuizScore.Quiz_Id = '" & Quiz_Id & "' AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Answer_Id IS NULL ORDER BY Student_CurrentNoInRoom"
        Dim dt As DataTable = db.getdata(sql)
        ' แปลงค่าเป็น JSON
        Dim sb As New StringBuilder()
        Dim JsonString As New ArrayList
        If dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                'JsonString = New With {.stuId = row("Student_Id")}
                JsonString.Add(New With {.stuId = row("Student_CurrentNoInRoom")})
            Next
        Else
            JsonString.Add(New With {.stuId = "0"})
        End If
        Dim StudentID = js.Serialize(JsonString)
        Return StudentID
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function CheckQQNoAfterLoading(ByVal ExamNum As String) ' check ข้อปัจจุบัน ท้ายเพจ
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        If ExamNum Is Nothing Or ExamNum = "" Then
            Return "Error"
        End If
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & Quiz_Id & "' ORDER BY lastupdate DESC; "
        Dim CurrentQQNo As String = _DB.ExecuteScalar(sql)
        If CurrentQQNo <> ExamNum Then
            Return "Reload"
        Else
            Return "NotReload"
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetTabletLost() As Integer
        Dim dt As DataTable = GetTabletInQuiz()
        Dim redis As New RedisStore()
        For Each r As DataRow In dt.Rows
            If redis.Getkey(r("Tablet_SerialNumber").ToString() & "_status") = "" Then
                Return r("Student_CurrentNoInRoom").ToString()
            End If
        Next
        Return 0
    End Function

    <WebMethod(EnableSession:=True)>
    Private Function GetTabletInQuiz() As DataTable
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim SchoolId As String = HttpContext.Current.Session("SchoolCode").ToString()
        Dim sql As New StringBuilder()
        sql.Append("SELECT  tl.Tablet_SerialNumber,ts.Student_CurrentNoInRoom,q.lastTime FROM tblQuizSession qs ")
        sql.Append("INNER JOIN t360_tblTablet tl ON qs.Tablet_Id = tl.Tablet_Id ")
        sql.Append("INNER JOIN t360_tblStudent ts ON qs.Player_Id = ts.Student_Id ")
        sql.Append("INNER JOIN (SELECT Quiz_Id,MAX(LastUpdate) AS lastTime FROM tblQuizScore GROUP BY Quiz_Id) AS q ON qs.Quiz_Id = q.Quiz_Id ")
        sql.Append("WHERE qs.Quiz_Id = '" & QuizId & "' AND Player_Type = 2 AND qs.School_Code = '" & SchoolId & "' ")
        sql.Append("ORDER BY q.lastTime DESC;")
        Return New ClassConnectSql().getdata(sql.ToString())
    End Function

#Region "Time"
#Region "Time Mode FOR Quiz Student"
    <WebMethod(EnableSession:=True)>
    Public Function GetModeQuizAndTimerStudent(ByVal _AnswerState As String, IsStartQuiz As Boolean)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return "ERROR"
        End If
        Dim AllTime As Integer = 0
        Dim TimeRemain As Integer = 0
        Dim NeedTimer As Boolean
        Dim timerType As Boolean = False
        Dim timeTotal As Integer = 0
        Dim noWatch As Boolean = False
        Dim IsHomeWork As Boolean = False
        Dim Deadline As String = ""
        Dim db As New ClassConnectSql()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim PlayerId As String = HttpContext.Current.Session("_StudentId").ToString()
        Dim sql As String = ""

        'If HttpContext.Current.Session("HomeworkMode") = True Then
        '    'sql = "select DATEDIFF(SECOND,LastUpdate,dbo.GetThaiDate())as timeDiff from tblQuiz Where Quiz_Id = '" & shareQuizId & "'"
        '    sql = " SELECT  tblModuleAssignment.End_Date FROM tblQuiz INNER JOIN tblModuleDetailCompletion ON tblQuiz.Quiz_Id = tblModuleDetailCompletion.Quiz_Id "
        '    sql &= " INNER JOIN tblModuleAssignment ON tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id "
        '    sql &= " WHERE tblQuiz.Quiz_Id = '" & QuizId & "' AND tblModuleAssignment.IsActive = 1 "
        '    sql &= " AND tblModuleDetailCompletion.Student_Id = '" & PlayerId & "'; "
        'Else
        If IsStartQuiz Then
            sql = "Update tblQuiz set StartTime = getdate() WHERE Quiz_Id = '" & QuizId & "'; "
            db.Execute(sql)
        End If

        sql = "  SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,DATEDIFF(SECOND,StartTime,dbo.GetThaiDate()) as timeDiff,DATEDIFF(SECOND,dbo.GetThaiDate(),DATEADD(MINUTE,TimePerTotal,StartTime)) as timeRemain,IsPracticeMode FROM tblQuiz WHERE Quiz_Id = '" & QuizId & "'; "
        'End If

        Dim dt = db.getdata(sql)
        If (dt.Rows.Count > 0) Then
            'If HttpContext.Current.Session("HomeworkMode") = True Then
            '    NeedTimer = False
            '    IsHomeWork = True

            '    If BusinessTablet360.ClsKNSession.IsMaxONet Then
            '        Deadline = "ฝึกฝนเตรียมสอบ"
            '    Else
            '        Deadline = "กำหนดส่ง " & Convert.ToDateTime(dt.Rows(0)("End_Date")).ToPointPlusTime()
            '    End If

            'Else
            If (dt.Rows(0)("NeedTimer")) Then
                    ' จับเวลาในการทำควิซ ทั้ง ข้อต่อข้อและทั้งหมด
                    NeedTimer = True
                    ' state ทำข้อสอบ 0 หรือ 1
                    If (_AnswerState = "0" Or _AnswerState = "1") Then
                        If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                            ' จับเวลาต่อข้อ
                            AllTime = CInt(dt.Rows(0)("TimePerQuestion"))
                            timerType = True
                        Else
                            ' จับเวลาทั้งหมด
                            'AllTime = CInt(dt.Rows(0)("timeDiff"))
                            If CInt(dt.Rows(0)("timeRemain")) > 0 Then
                                TimeRemain = CInt(dt.Rows(0)("timeRemain"))
                                timeTotal = CInt(dt.Rows(0)("TimePerTotal"))
                            End If
                        End If
                    Else
                        ' state เฉลย
                        If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
                            ' เฉลยแบบมีเวลา
                            'TimeRemain = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                            If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                                ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
                                '' เมื่อเฉลยหมดเวลาให้กดปุ่ม next ให้
                                AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                                timerType = True
                            Else
                                ' ข้อสอบแบบจับเวลาทั้งหมด
                                If (CInt(dt.Rows(0)("timeRemain")) > 0) Then
                                    ' เมื่อหมดเฉลยจะกดปุ่ม next ให้ แต่ถ้าหมดเวลาสอบจะขึ้น dialog
                                    AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                                    timerType = True
                                End If
                            End If
                        Else
                            ' เฉลยแบบไม่มีเวลา
                            If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                                ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
                                '' เอาเวลาจากไหน เพื่อไม่ให้มันเซ็ต dialog ขึ้น หรือมันกด next เอง
                                noWatch = True
                            Else
                                ' ข้อสอบแบบจับเวลาทั้งหมด 
                                '' ให้ใช้เวลาเดียวกับตอนที่ render โจทย์
                                'If CInt(dt.Rows(0)("timeRemain")) > 0 Then
                                '    AllTime = CInt(dt.Rows(0)("timeRemain"))
                                'End If

                                ' เฉลยตอนสุดท้ายไม่ต้องใช้เวลาแล้ว
                                noWatch = True
                            End If
                        End If
                    End If
                Else
                    ' ไม่จับเวลาในการทำควิซ 
                    '' แต่ถ้ามีเฉลยข้อต่อข้อแล้วใส่เวลา เมื่อถึง state เฉลย เวลาก็ยังคงเป็นแบบเดินไปเรื่อยๆๆ
                    NeedTimer = False
                    AllTime = dt.Rows(0)("timeDiff")
                    If (_AnswerState = "2") Then
                        ' state เฉลย
                        If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
                            NeedTimer = True
                            AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                            timerType = True
                        End If
                    'If (dt.Rows(0)("IsPracticeMode") = "1") Then
                    sql = "  select DATEDIFF(SECOND,StartTime,tblQuiz.Lastupdate) as timeDiff from tblquiz"
                            sql &= " where tblquiz.Quiz_Id = '" & QuizId & "'"
                            Dim lastTotalTime As String = db.ExecuteScalar(sql)
                            NeedTimer = False
                            AllTime = CInt(lastTotalTime)
                            timerType = False
                    'End If
                End If
                End If
                'End If
                'AllTime = 0
                Dim JsonString
            JsonString = New With {.NeedTimer = NeedTimer, .AllTime = AllTime, .timerType = timerType, .TimeRemain = TimeRemain, .timeTotal = timeTotal, .noWatch = noWatch, .IsHomeWork = IsHomeWork, .Deadline = Deadline}
            GetModeQuizAndTimerStudent = js.Serialize(JsonString)
        Else
            GetModeQuizAndTimerStudent = "ERROR"
        End If
    End Function
#End Region

#Region "Time Mode For Quiz Teacher"
    <WebMethod(EnableSession:=True)>
    Public Function GetModeQuizAndTimer(ByVal _AnswerState As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return "ERROR"
        End If
        Dim db As New ClassConnectSql()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As String = "  SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,DATEDIFF(SECOND,StartTime,dbo.GetThaiDate()) as timeDiff,DATEDIFF(SECOND,dbo.GetThaiDate(),DATEADD(MINUTE,TimePerTotal,StartTime)) as timeRemain FROM tblQuiz WHERE Quiz_Id = '" & Quiz_Id & "'; "
        Dim dt As DataTable = db.getdata(sql)
        Dim AllTime As Integer = 0
        Dim TimeRemain As Integer = 0
        Dim NeedTimer As Boolean
        Dim timerType As Boolean = False
        Dim noWatch As Boolean = False
        If (dt.Rows.Count > 0) Then
            If (dt.Rows(0)("NeedTimer")) Then
                ' จับเวลาในการทำควิซ ทั้ง ข้อต่อข้อและทั้งหมด
                NeedTimer = True
                ' state ทำข้อสอบ 0 หรือ 1
                If (_AnswerState = "0" Or _AnswerState = "1") Then
                    If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                        ' จับเวลาต่อข้อ
                        AllTime = CInt(dt.Rows(0)("TimePerQuestion"))
                        timerType = True
                    Else
                        ' จับเวลาทั้งหมด
                        If CInt(dt.Rows(0)("timeRemain")) > 0 Then
                            AllTime = CInt(dt.Rows(0)("timeDiff"))
                            TimeRemain = CInt(dt.Rows(0)("timeRemain"))
                            Dim redis As New RedisStore()
                            If redis.Getkey(Quiz_Id & "_TimePause") <> "" Then
                                Dim timeOnPause As Integer = CInt(redis.Getkey(Quiz_Id & "_TimePause"))
                                If timeOnPause > AllTime Then
                                    AllTime -= (timeOnPause / 2)
                                    TimeRemain += (timeOnPause / 2)
                                Else
                                    AllTime -= timeOnPause
                                    TimeRemain += timeOnPause
                                End If
                            End If
                        End If
                    End If
                Else
                    ' state เฉลย
                    If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
                        ' เฉลยแบบมีเวลา
                        If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                            ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
                            '' เมื่อเฉลยหมดเวลาให้กดปุ่ม next ให้
                            AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                            timerType = True
                        Else
                            ' ข้อสอบแบบจับเวลาทั้งหมด
                            If (CInt(dt.Rows(0)("timeRemain")) > 0) Then
                                ' เมื่อหมดเฉลยจะกดปุ่ม next ให้ แต่ถ้าหมดเวลาสอบจะขึ้น dialog
                                AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                                timerType = True
                            End If
                        End If
                    Else
                        ' เฉลยแบบไม่มีเวลา
                        If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
                            ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
                            '' เอาเวลาจากไหน เพื่อไม่ให้มันเซ็ต dialog ขึ้น หรือมันกด next เอง
                            noWatch = True
                        Else
                            ' ข้อสอบแบบจับเวลาทั้งหมด 
                            '' ให้ใช้เวลาเดียวกับตอนที่ render โจทย์
                            'If CInt(dt.Rows(0)("timeRemain")) > 0 Then
                            '    AllTime = CInt(dt.Rows(0)("timeDiff"))
                            '    TimeRemain = CInt(dt.Rows(0)("timeRemain"))
                            'End If

                            ' เฉลยตอนสุดท้ายไม่ต้องใช้เวลาแล้ว
                            noWatch = True
                        End If
                    End If
                End If
            Else
                ' ไม่จับเวลาในการทำควิซ 
                '' แต่ถ้ามีเฉลยข้อต่อข้อแล้วใส่เวลา เมื่อถึง state เฉลย เวลาก็ยังคงเป็นแบบเดินไปเรื่อยๆๆ               
                NeedTimer = False
                AllTime = dt.Rows(0)("timeDiff")
                If (_AnswerState = "2") Then
                    ' state เฉลย
                    If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
                        NeedTimer = True
                        AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
                        timerType = True
                    End If
                End If
            End If
            Dim js As New JavaScriptSerializer()
            Dim JsonString
            JsonString = New With {.NeedTimer = NeedTimer, .AllTime = AllTime, .timerType = timerType, .TimeRemain = TimeRemain, .noWatch = noWatch}
            GetModeQuizAndTimer = js.Serialize(JsonString)
        Else
            GetModeQuizAndTimer = "ERROR"
        End If
    End Function
#End Region

    <WebMethod(EnableSession:=True)>
    Public Function UpdateQuizTimeStartOnReady() As String 'update เวลา start quiz เมื่อ checktablet เรียบร้อยแล้ว
        If Not HttpContext.Current.Session("Quiz_Id").ToString() Is Nothing Then
            Dim sql As String = " UPDATE tblQuiz SET StartTime = dbo.GetThaiDate(),LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "'; "
            Dim db As New ClassConnectSql()
            db.Execute(sql)
        End If
        Return "1"
    End Function

    <WebMethod(EnableSession:=True)>
    Public Sub SaveTimeOnPause(ByVal t As Integer)
        Dim redis As New RedisStore()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        If redis.Getkey(QuizId & "_TimePause") <> "" Then
            Dim timeOnPause As Integer = CInt(redis.Getkey(QuizId & "_TimePause"))
            t += timeOnPause
        End If
        redis.SetKey(QuizId & "_TimePause", t)
    End Sub

#End Region

#Region "SaveAnswer"
#Region "Save Answer SortQuestion"
    <WebMethod(EnableSession:=True)>
    Public Sub SetAnswerSortQuestion(ByVal QuestionIdAll As String, ByVal PlayerId As String, ByVal ExamNum As String, ByVal DeviceId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Exit Sub
        End If
        Dim db As New ClassConnectSql()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim SchoolId As String = HttpContext.Current.Session("SchoolCode").ToString()
        Dim sql As New StringBuilder()
        Dim dtCorrectAnswer, dtPlayerAnswer As DataTable

        ' update คำตอบข้อสอบแบบเรียงลำดับตามที่เด็กตอบ
        Dim questionIdArr = QuestionIdAll.Split(",") 'arr questionid ของข้อเรียงลำดับ
        Dim num As Integer = 1
        sql.Clear()
        For Each Q In questionIdArr
            sql.Append(" UPDATE tblQuizAnswer SET QA_No = '" & num & "', LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE Quiz_Id = '" & QuizId & "' AND Player_Id = '" & PlayerId & "' AND Question_Id = '" & Q.ToString() & "' ;")
            num = num + 1
        Next
        db.Execute(sql.ToString())

        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
            Dim redis As New RedisStore()
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
            Dim IsDone As Boolean = False
            If q IsNot Nothing Then
                For Each QuestionId In questionIdArr
                    If q.NoOfDone.ContainsKey(QuestionId) Then
                        IsDone = True
                        Exit For
                    End If
                Next
                If IsDone = False Then
                    q.NoOfDone.Add(questionIdArr(0), 1)
                    redis.SetKey(Of Quiz)(DeviceId, q)
                End If
            End If
        End If


        ' datatable คำตอบทึ่เรียงแบบถูก 
        sql.Clear()
        sql.Append(" SELECT  dbo.tblQuizAnswer.Question_Id FROM dbo.tblQuizAnswer INNER JOIN dbo.tblQuestion ON dbo.tblQuizAnswer.Question_Id = dbo.tblQuestion.Question_Id WHERE dbo.tblQuizAnswer.Question_Id IN ( ")
        sql.Append(" SELECT Question_Id FROM dbo.tblQuizQuestion WHERE QQ_No IN (SELECT QQ_No FROM tblQuizQuestion WHERE (Quiz_Id = '" & QuizId & "') AND (Question_Id IN ( ")
        sql.Append(" SELECT Question_Id FROM  tblQuizScore WHERE (Quiz_Id = '" & QuizId & " ') AND ")
        sql.Append(" (Student_Id = '" & PlayerId & "') AND (QQ_No = '" & ExamNum & "')))) AND dbo.tblQuizQuestion.Quiz_Id = '" & QuizId & "' ) ")
        sql.Append(" AND dbo.tblQuizAnswer.Quiz_Id = '" & QuizId & "' AND dbo.tblQuizAnswer.Player_Id = '" & PlayerId & "' ")
        sql.Append(" ORDER BY dbo.tblQuizAnswer.QA_No;")
        dtCorrectAnswer = db.getdata(sql.ToString())

        ' datatable คำตอบที่เด็กทำ
        sql.Clear()
        sql.Append(" select Question_Id from tblAnswer where QSet_Id = (Select QSet_Id from tblQuestion where Question_Id = ( ")
        sql.Append(" Select Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & QuizId & "' ")
        sql.Append(" And QQ_No = '" & ExamNum & "' and Student_Id = '" & PlayerId & "')) ORDER BY CAST(Answer_Name as varchar);")
        dtPlayerAnswer = db.getdata(sql.ToString())

        ' Loop check คำตอบใน datatable ว่าเรียงถูกหรือเปล่า
        Dim scored As String = "1"
        For i As Integer = 0 To dtCorrectAnswer.Rows.Count - 1
            If (dtCorrectAnswer.Rows(i)("Question_Id").ToString <> dtPlayerAnswer(i)("Question_Id").ToString) Then
                scored = "0"
                Exit For
            End If
        Next

        sql.Clear()
        sql.Append(" SELECT  tblQuizAnswer.Question_Id,tblQuizAnswer.Answer_Id FROM tblQuizScore INNER JOIN tblQuizAnswer ON tblQuizScore.Question_Id = tblQuizAnswer.Question_Id ")
        sql.Append(" WHERE tblQuizScore.Quiz_Id = '" & QuizId & "' AND tblQuizScore.School_Code = '" & SchoolId & "' AND tblQuizScore.Student_Id = '" & PlayerId & "' ")
        sql.Append(" AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizAnswer.Quiz_Id = '" & QuizId & "' AND tblQuizAnswer.Player_Id = '" & PlayerId & "';")

        Dim dtQuestionAndAnswer As DataTable = db.getdata(sql.ToString())
        Dim CurrentQuestion As String = dtQuestionAndAnswer.Rows(0)("Question_Id").ToString()
        Dim CurrentAnswer As String = dtQuestionAndAnswer.Rows(0)("Answer_Id").ToString()

        ' Update คะแนน,responseAmount,lastupdate,isscored,score ที่ tblQuizScore
        sql.Clear()
        sql.Append(" UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), ")
        sql.Append(" LastUpdate = dbo.GetThaiDate(),ClientId = NULL,ResponseAmount = ResponseAmount + 1,IsScored = '0',Answer_Id = '" & CurrentAnswer & "', ")
        sql.Append(" Score = '" & scored & "' WHERE Student_Id = '" & PlayerId & "' AND Quiz_Id = '" & QuizId & "' ")
        sql.Append(" AND School_Code = '" & SchoolId & "' AND Question_Id = '" & CurrentQuestion & "';")
        db.Execute(sql.ToString())
    End Sub
#End Region

#Region "Save Answer PairQuestion"
    <WebMethod(EnableSession:=True)>
    Public Sub SetAnswerPairQuestion(ByVal QuestionIdAll As String, ByVal AnswerIdAll As String, ByVal PlayerId As String, ByVal ExamNum As String, ByVal DeviceId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Exit Sub
        End If
        Dim db As New ClassConnectSql()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim SchoolId As String = HttpContext.Current.Session("SchoolCode").ToString()
        Dim ArrQuestion As Array = QuestionIdAll.Split(",")
        Dim ArrAnswer As Array = AnswerIdAll.Split(",")
        Dim sql As New StringBuilder()
        Dim QA_No As Integer = 1

        ' Update QuizAnswer
        For i As Integer = 0 To ArrQuestion.Length - 1
            sql.Append(" UPDATE tblQuizAnswer SET LastUpdate = dbo.GetThaiDate(),ClientId = NULL,Answer_Id ='")
            sql.Append(ArrAnswer(i))
            sql.Append("' WHERE Quiz_Id = '")
            sql.Append(QuizId)
            sql.Append("' AND Question_Id = '")
            sql.Append(ArrQuestion(i))
            sql.Append("' AND QA_No = '")
            sql.Append(QA_No)
            sql.Append("' AND Player_Id = '")
            sql.Append(PlayerId)
            sql.Append("';")
            QA_No = QA_No + 1

        Next
        db.Execute(sql.ToString())

        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
            Dim redis As New RedisStore()
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
            Dim IsDone As Boolean = False
            If q IsNot Nothing Then
                For Each QuestionId In ArrQuestion
                    If q.NoOfDone.ContainsKey(QuestionId) Then
                        IsDone = True
                        Exit For
                    End If
                Next
                If IsDone = False Then
                    q.NoOfDone.Add(ArrQuestion(0), 1)
                    redis.SetKey(Of Quiz)(DeviceId, q)
                End If
            End If
        End If

        sql.Clear()
        sql.Append(" SELECT SUM(CASE tblAnswer.Answer_Id WHEN tblQuizAnswer.Answer_Id THEN Answer_Score ELSE Answer_ScoreMinus END) AS Score  FROM tblAnswer ")
        sql.Append(" LEFT JOIN tblQuizAnswer ON tblAnswer.Question_Id = tblQuizAnswer.Question_Id ")
        sql.Append(" WHERE tblAnswer.Question_Id IN (")
        For j As Integer = 0 To ArrQuestion.Length - 1
            sql.Append("'")
            sql.Append(ArrQuestion(j))
            If j = ArrQuestion.Length - 1 Then
                sql.Append("'")
            Else
                sql.Append("',")
            End If
        Next
        sql.Append(") AND tblQuizAnswer.Quiz_Id = '")
        sql.Append(QuizId)
        sql.Append("' AND Player_Id = '")
        sql.Append(PlayerId)
        sql.Append("';")
        Dim scored As String = db.ExecuteScalar(sql.ToString())
        Dim CurrentAnswer As String = ArrAnswer(0)
        ' Update คะแนน,responseAmount,lastupdate,isscored,score ที่ tblQuizScore
        'sql.Clear()
        'sql.Append(" UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), LastUpdate = dbo.GetThaiDate(),ClientId = NULL,ResponseAmount = ResponseAmount + 1,IsScored = '0', Answer_Id = '")
        'sql.Append(CurrentAnswer)
        'sql.Append("', Score = '")
        'sql.Append(scored)
        'sql.Append("' WHERE Student_Id = '")
        'sql.Append(PlayerId)
        'sql.Append("' AND Quiz_Id = '")
        'sql.Append(QuizId)
        'sql.Append("' AND School_Code = '")
        'sql.Append(SchoolId)
        'sql.Append("' AND QQ_No = '")
        'sql.Append(ExamNum)
        'sql.Append("'; ")
        'db.Execute(sql.ToString())

        'sql.Append(" Update tblquizsession set TotalScore = (select sum(Score) from tblQuizScore")
        'sql.Append(" where Quiz_Id = '" & QuizId & "' and Player_Id = '" & PlayerId & "'),LastUpdate = dbo.GetThaiDate(),ClientId = null ")
        'sql.Append(" where Quiz_Id = '" & QuizId & "' and Player_Id = '" & PlayerId & "';")
        'db.Execute(sql.ToString())
        IsCorrectAnswerPairQuestion(ArrQuestion, PlayerId, db)

    End Sub

    Private Shared Function IsCorrectAnswerPairQuestion(Qusetions As Array, PlayerId As String, ByRef db As ClassConnectSql) As Boolean
        Dim allQuestions As String = ""
        For Each question In Qusetions
            allQuestions &= String.Format("'{0}',", question)
        Next
        allQuestions = allQuestions.Substring(0, allQuestions.Length - 1)
        Dim sql As New StringBuilder()
        sql.Append(" SELECT q.Quiz_Id,q.Question_Id,q.Answer_Id,a.Answer_Id AS CorrectAnswer_Id,a.Answer_Name FROM tblQuizAnswer q INNER JOIN tblAnswer a on q.Question_Id = a.Question_Id WHERE q.Question_Id IN ")
        sql.Append(String.Format("({0}) AND q.Quiz_Id = '{1}' and q.Player_Id = '{2}' ORDER BY q.QA_No;;", allQuestions, HttpContext.Current.Session("Quiz_Id").ToString(), PlayerId))
        Dim dt As DataTable = db.getdata(sql.ToString())

        sql.Clear()
        sql.Append(" SELECT q.Quiz_Id,q.Question_Id,q.Answer_Id,a.Answer_Id AS CorrectAnswer_Id,a.Answer_Name FROM tblQuizAnswer q INNER JOIN tblAnswer a on q.Answer_Id = a.Answer_Id WHERE q.Question_Id IN ")
        sql.Append(String.Format("({0}) AND q.Quiz_Id = '{1}'  and q.Player_Id = '{2}' ORDER BY q.QA_No;;", allQuestions, HttpContext.Current.Session("Quiz_Id").ToString(), PlayerId))
        Dim dtUserAnswer As DataTable = db.getdata(sql.ToString())

        For i As Integer = 0 To dt.Rows.Count - 1
            If (dt.Rows(i)("Answer_Id") <> dt.Rows(i)("CorrectAnswer_Id")) Then
                If (dt.Rows(i)("Answer_Name") <> dtUserAnswer.Rows(i)("Answer_Name")) Then
                    UpdateScorePairQuestion(0, dt, db)
                    Return False
                End If
            End If
        Next
        UpdateScorePairQuestion(1, dt, db)
        Return True
    End Function

    ' update score ตอนตอบคำถามแบบ จับคู่
    Private Shared Sub UpdateScorePairQuestion(Score As Integer, dtQuestion As DataTable, ByRef db As ClassConnectSql)
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        Dim StartQuestion As Date = HttpContext.Current.Session("StartTimeQuestion")
        Dim EndQuesion As Date = Date.Now

        Dim TimeDoQuestion = DateDiff(DateInterval.Second, StartQuestion, EndQuesion)
        Dim sql As New StringBuilder
        For Each r In dtQuestion.Rows
            sql.Append(String.Format("UPDATE tblQuizScore SET Answer_Id = '{0}',ResponseAmount = ResponseAmount + 1,LastUpdate = dbo.GetThaiDate(),Score = {1}, TimeTotal = TimeTotal + {5} WHERE Student_Id = '{2}' AND Quiz_Id = '{3}' AND Question_Id = '{4}';", r("Answer_Id"), Score, UserId, Quiz_Id, r("Question_Id"), TimeDoQuestion))
        Next
        db.Execute(sql.ToString())

        HttpContext.Current.Session("StartTimeQuestion") = Date.Now

        sql.Clear()
        sql.Append(String.Format("UPDATE tblQuizSession SET TotalScore = (SELECT SUM(Score) FROM tblQuizScore WHERE Quiz_Id = '{0}' and Student_Id = '{1}') WHERE Quiz_Id = '{0}' And Player_Id = '{1}';", Quiz_Id, UserId))
        db.Execute(sql.ToString()) 'update totalscore ตอนตอบแบบจับคู่



    End Sub

#End Region

#Region "Save Answer ChoiceQuestion"
    <WebMethod(EnableSession:=True)>
    Public Function SetAnswerChoiceQuestion(ByVal QuestionId As String, ByVal AnswerId As String, ByVal PlayerId As String, ByVal NotReplyMode As String, ByVal DeviceId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim CheckSaveSuccess As String = ""
        'ถ้าอยู่ในโหมดทวนข้อข้ามแล้วกดตอบให้สร้าง Array
        If NotReplyMode = "True" Then
            HttpContext.Current.Session("IsReplyAnswer") = True
        End If
        Dim SrID As String = ClsDroidPad.GetSR_IdFromStudentId(PlayerId)
        Dim IsSaveComplate As Boolean = ClsDroidPad.UpdateWhenStudentClick(AnswerId, QuizId, QuestionId, PlayerId, SrID)
        If IsSaveComplate = True Then
            CheckSaveSuccess = "1"
        End If
        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
            Dim redis As New RedisStore()
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
            If q IsNot Nothing Then
                If q.NoOfDone.ContainsKey(QuestionId) = False Then
                    q.NoOfDone.Add(QuestionId, 1)
                    redis.SetKey(Of Quiz)(DeviceId, q)
                End If
            End If
        End If
        Return CheckSaveSuccess
    End Function
#End Region
#End Region

#Region "Event ตอนกด Next LeapChoice"
    <WebMethod(EnableSession:=True)>
    Public Function NextToLeapChoice(ByVal IsCorrect As String, ByVal PlayerId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        If IsCorrect = 1 Then

            'Diff เวลาไว้ไปแสดงในหน้าเฉลย
            Dim Sql As String = "Update tblquiz set Lastupdate = dbo.getThaiDate() where quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "';"
            Dim db As New ClassConnectSql()

            db.Execute(Sql)
            HttpContext.Current.Session("ExamNum") = 0
            HttpContext.Current.Session("_AnswerState") = "2"
            'HttpContext.Current.Session("CheckLastQuestion") = False
            HttpContext.Current.Session("ShowCorrectAfterCompleteState") = True
            HttpContext.Current.Session("ShowSelectExamPanel") = True
            If PlayerId = "1" Then
                HttpContext.Current.Session("SetAnswerStateTeacher") = True
            End If
            Return "False"
        Else
            Dim clsActivity As New ClsActivity(New ClassConnectSql)
            Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()


            Dim UnComPlete As String = clsActivity.CountLeapExam(QuizId, PlayerId).ToString
            If UnComPlete = "0" Then
                HttpContext.Current.Session("ExamNum") = 0
            Else
                Dim FirstLeapChoice As Integer
                FirstLeapChoice = CInt(clsActivity.GetFirstLeapChoice(QuizId)) - 1
                HttpContext.Current.Session("ExamNum") = FirstLeapChoice
            End If
            HttpContext.Current.Session("_AnswerState") = "0" 'HttpContext.Current.Session("CheckLastQuestion") = False

            HttpContext.Current.Session("ShowCorrectAfterCompleteState") = False
            Return "False"
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetRenderForNextStep(ByVal EventType As String, ByVal DeviceId As String, ByVal _AnswerState As String, ByVal IsShowScoreAfterCompleteState As Boolean)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim PlayerId As String = HttpContext.Current.Session("_StudentId")
        'EvenType 1 = next 2 = Exit 3 = Complete
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim UnCompleteAmount As String = ""
        Dim txtForShowDialog As String = ""
        Dim IsShowDialog As String = ""
        Dim DialogType As String = ""
        Dim NextStep As String = ""
        Dim DNTExam As String = ""

        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim ShowCorrectAnswer As Boolean = HttpContext.Current.Session("ShowCorrectAfterComplete")
        Dim NeedShowScoreAfterComplete As Boolean = HttpContext.Current.Session("NeedShowScoreAfterComplete")
        UnCompleteAmount = ClsActivity.CountLeapExam(QuizId, PlayerId).ToString
        DNTExam = ClsActivity.GetDNTExam(QuizId)

        'Update TotalScore ของนักเรียนเมื่อกดจบ quiz , homework , practice
        'ClsActivity.SetTotalScore(QuizId, PlayerId)

        If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Quiz Then
            If _AnswerState = "0" Then
                If UnCompleteAmount = "0" Then
                    If ShowCorrectAnswer = True Then
                        IsShowDialog = "True"
                        txtForShowDialog = "ทำครบทุกข้อแล้ว จะดูเฉลยเลยหรือทบทวนอีกสักรอบก่อนส่งคะ"
                        DialogType = "8"
                        NextStep = ""
                    ElseIf NeedShowScoreAfterComplete = True Then
                        IsShowDialog = "True"
                        txtForShowDialog = "ทำครบทุกข้อแล้ว จะดูคะแนนเลยหรือจะทวนอีกสักรอบก่อนส่งคะ"
                        DialogType = "9"
                        NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
                    Else
                        IsShowDialog = "True"
                        txtForShowDialog = "ทำควิซหมดข้อสุดท้ายแล้ว จบควิซเลยมั้ยคะ ?"
                        DialogType = "1"
                        NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
                    End If
                Else
                    If DNTExam = 0 Then
                        If ShowCorrectAnswer = True Then
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ด้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                            DialogType = "3"
                            NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
                        ElseIf NeedShowScoreAfterComplete = True Then
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ด้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือดูคะแนนเลยคะ"
                            DialogType = "10"
                            NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
                        Else
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ด้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือจบควิซเลยคะ"
                            DialogType = "5"
                            NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
                        End If
                    Else
                        If ShowCorrectAnswer = True Then
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ <br> จะทบทวนหรือดูเฉลยเลยคะ"
                            DialogType = "11"
                            NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
                        ElseIf NeedShowScoreAfterComplete = True Then
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ <br> จะทบทวนหรือดูคะแนนเลยคะ"
                            DialogType = "12"
                            NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
                        Else
                            IsShowDialog = "True"
                            txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ <br> จะทบทวนหรือจบควิซเลยคะ"
                            DialogType = "13"
                            NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
                        End If
                    End If


                End If
            ElseIf _AnswerState = "2" Then


                If NeedShowScoreAfterComplete = True Then
                    IsShowDialog = "True"
                    txtForShowDialog = "ทำครบทุกข้อแล้ว จะดูคะแนนเลยหรือจะทวนอีกสักรอบก่อนส่งคะ"
                    DialogType = "9"
                    NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
                Else
                    IsShowDialog = "True"
                    txtForShowDialog = "ครบทุกข้อแล้ว ออกจากกิจกรรมเลยมั้ยคะ ?"
                    DialogType = "1"
                    NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
                End If

            End If
        End If

        '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Practice Then
            If _AnswerState = "0" Then
                If UnCompleteAmount = "0" Then
                    'ต้องไปเฉลย
                    IsShowDialog = "True"
                    txtForShowDialog = "ทำครบทุกข้อแล้ว จะดูเฉลยเลยหรือทบทวนอีกสักรอบก่อนส่งคะ"
                    DialogType = "8"
                    NextStep = ""
                Else
                    If DNTExam = 0 Then
                        IsShowDialog = "True"
                        txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                        DialogType = "3"
                        NextStep = ""
                    Else
                        IsShowDialog = "True"
                        txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                        DialogType = "11"
                        NextStep = ""
                    End If

                End If
            ElseIf _AnswerState = "2" Then
                IsShowDialog = "True"
                txtForShowDialog = "ครบทุกข้อแล้ว ออกจากกิจกรรมเลยมั้ยคะ ?"
                DialogType = "1"
                NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
            End If
        End If
        '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Homework Then
            Dim pageNextStep As String = If((BusinessTablet360.ClsKNSession.IsMaxONet), "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId, "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId)
            Dim txtMode As String = If((BusinessTablet360.ClsKNSession.IsMaxONet), "กิจกรรม", "การบ้าน")
            'เช็คก่อนว่าส่งยัง 
            'ส่งแล้วครบมั้ย
            If _AnswerState = "0" Then
                If ClsActivity.GetCompleteHomework(QuizId, PlayerId) Then
                    'ถ้าส่งแล้ว กด next ไปต่อ กดออก ไม่มีปุ่มส่ง
                    'EvenType 1 = next 2 = Exit 3 = Complete
                    If EventType = "1" Then
                        IsShowDialog = "True"
                        txtForShowDialog = String.Format("ทำครบทุกข้อแล้วค่ะ ออกจาก{0}เลยมั้ยคะ ?", txtMode)
                        DialogType = "2"
                        NextStep = pageNextStep
                    ElseIf EventType = "2" Then
                        IsShowDialog = "True"
                        txtForShowDialog = String.Format("ส่ง{0}เรียบร้อยแล้ว ออกเลยมั้ยคะ ?", txtMode)
                        DialogType = "2"
                        NextStep = pageNextStep
                    End If
                Else
                    If UnCompleteAmount = "0" Then
                        'ครบแล้วถ้ากดส่ง ส่งเรียบร้อย กด next ยังไม่ส่ง ส่งเลยมั้ย กดออก ยังไม่ส่ง ส่งเลยมั้ย 
                        'EvenType 1 = next 2 = Exit 3 = Complete
                        '2 ชั้น อยู่ที่เดิม
                        If EventType = "1" Or EventType = "3" Then
                            If BusinessTablet360.ClsKNSession.IsMaxONet Then
                                IsShowDialog = "True"
                                DialogType = "11"
                                txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                                NextStep = ""
                            Else
                                IsShowDialog = "True"
                                txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ส่งเลยมั้ยคะ ?"
                                DialogType = "6"
                                NextStep = pageNextStep
                            End If
                        Else
                            If BusinessTablet360.ClsKNSession.IsMaxONet Then
                                IsShowDialog = "True"
                                DialogType = "11"
                                txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                                NextStep = ""
                            Else
                                IsShowDialog = "True"
                                txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ส่งเลยมั้ยคะ ?"
                                DialogType = "7"
                                NextStep = pageNextStep
                            End If
                        End If
                    Else
                        IsShowDialog = "True"
                        If EventType = "1" Or EventType = "3" Then
                            DialogType = "4"
                            txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือส่งเลยคะ"
                            NextStep = ""
                        Else
                            If BusinessTablet360.ClsKNSession.IsMaxONet Then
                                DialogType = "11"
                                txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือดูเฉลยเลยคะ"
                                NextStep = ""
                            Else
                                DialogType = "5"
                                txtForShowDialog = "ยังไม่ได้ทำอีก " & UnCompleteAmount & " ข้อค่ะ จะทบทวนหรือออกเลยคะ"
                                NextStep = pageNextStep
                            End If

                        End If
                    End If
                End If
            Else
                IsShowDialog = "True"
                txtForShowDialog = String.Format("ออกจาก{0}เลยมั้ยคะ ?", txtMode)
                DialogType = "2"
                NextStep = pageNextStep
            End If
        End If
        Dim JsonString
        JsonString = New With {.IsShowDialog = IsShowDialog, .txtForShowDialog = txtForShowDialog, .DialogType = DialogType, .NextStep = NextStep}
        Return js.Serialize(JsonString)
    End Function
#End Region

#Region "Panel ข้ามข้อ"
    <WebMethod(EnableSession:=True)>
    Public Function CreateStringLeapChoice(ByVal IsNormalSort As String, ByVal StudentId As String, ByVal ExamNum As String, ByVal PageNumber As String) 'Function สร้างปุ่มกดข้ามข้อ
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        'ใช้จริงต้องรับ DeviceId เพื่อเอามาหา QuizId กับ StudentId

        'หา Choice ก่อน
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim dt As New DataTable
        dt = GetChoice(IsNormalSort, QuizId, StudentId, ExamNum)

        'ได้ Choice ที่จะแสดงมาแล้ว เริ่มสร้าง Page

        Dim CheckQuantity As Integer = 1
        Dim IsLastPage As String = "False"
        'If (dt.Rows.Count() - 1) < 9 Then
        '    CheckQuantity = 1
        'Else
        CheckQuantity = Math.Floor((dt.Rows.Count()) / 10)
        'End If

        If (dt.Rows.Count() Mod 10) <> 0 Then
            CheckQuantity = CheckQuantity + 1
        End If


        If PageNumber > CheckQuantity Then
            PageNumber = 1
        ElseIf PageNumber + 1 > CheckQuantity Then
            IsLastPage = "True"
        ElseIf PageNumber = 0 Then
            If IsNormalSort Then
                IsLastPage = "True"
            End If

            PageNumber = CheckQuantity
        End If

        Dim CheckFirstChoice As Boolean = True
        Dim sb As New StringBuilder
        'Dim PageNumber As Integer = 0
        If dt.Rows.Count > 0 Then
            'PageNumber = 1
            Dim EachQQNo As String = ""
            Dim EachIsScore As String = ""
            Dim StartPage As Integer = 0
            Dim EndPage As Integer = 9
            If PageNumber <> "1" Then
                StartPage = (CInt(PageNumber) - 1) * 10
            End If

            If StartPage + 9 < dt.Rows.Count() - 1 Then
                EndPage = StartPage + 9
            Else
                EndPage = dt.Rows.Count() - 1
            End If

            For i = StartPage To EndPage
                EachQQNo = dt.Rows(i)("QQ_No").ToString() 'รับ QQ_No เพื่อเอามาเป็น Text ให้ปุ่ม
                EachIsScore = dt.Rows(i)("IsScored").ToString() 'รับ IsScore เพื่อที่จะได้รู้ว่าข้อนั้นตรวจหรือยัง
                'เช็คข้อสุดท้าย
                Dim IsLastChoice As Boolean = False
                If i = dt.Rows.Count - 1 Then
                    IsLastChoice = True
                End If

                Dim IsAnswered As Boolean = False
                'เช็คตอบหรือยัง
                If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then
                    IsAnswered = True
                End If

                'เช็คเพื่อให้สร้างปุ่มได้แค่หน้าละ 10 ปุ่มเท่านั้น ถ้าเป็นปุ่มที่หาร 11 ได้ลงตัว หมายถึงต้องขึ้น Page ใหม่
                If CheckFirstChoice Then
                    CheckFirstChoice = False
                    'ถ้าเป็นรอบแรกของการสร้างแต่ละรอบต้องสร้าง Div ขึ้นมาเพื่อครอบก่อนสร้างปุ่ม
                    sb.Append("<div id='divswipe' style='width:550px;margin-top:33px;margin-left:115px; display:block;'> ")
                End If
                sb.Append(CreateStringChoicePanel(IsNormalSort, IsLastChoice, IsAnswered, EachQQNo, EachIsScore))
            Next
            sb.Append("</div>")
        Else
            sb.Append("<div class='NotReplyEmpty'><span style='font-size:20px;margin-left:25px;'>ไม่มีข้อข้ามแล้ว ทำข้อที่ยังไม่ทำต่อเลยค่ะ</span></div>")
        End If



        Dim JsonString = New With {.HtmlLeapChoice = sb.ToString(), .CheckOverOnePage = dt.Rows.Count(), .PageNumber = PageNumber, .IsLastPage = IsLastPage}
        Return js.Serialize(JsonString)

    End Function
    Private Function GetChoice(IsNormalSort As String, QuizId As String, StudentId As String, Examnum As String) As DataTable
        Dim Activity As New ClsActivity(New ClassConnectSql)
        Dim _DB As New ClassConnectSql
        Dim sql As String = ""
        Dim dt As New DataTable
        If IsNormalSort Is Nothing Or IsNormalSort = "" Then
            Return dt
        End If

        If IsNormalSort = "True" Then 'ถ้าเรียงแบบปกติใช้คิวรี่นี้
            sql = " SELECT Answer_Id,QQ_No,IsScored,Score FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                           " AND Student_Id = '" & StudentId & "' ORDER BY  QQ_No "
            'sql = " SELECT Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "'  " & _
            '      " AND Student_Id = '" & StudentId & "' AND QQ_No <> (SELECT TOP 1 QQ_No FROM dbo.tblQuizScore " & _
            '      " WHERE Quiz_Id = '" & QuizId & "' " & _
            '      " AND Student_Id = '" & StudentId & "' ORDER BY QQ_No DESC) " & _
            '      " ORDER BY  QQ_No  "
        Else 'ถ้าเรียงจากข้อที่ยังไม่ได้ทำขึ้นก่อนใช้คิวรี่นี้

            If Activity.IsDiffExamForLeapchoice(QuizId) Then
                sql = " SELECT * FROM (SELECT TOP " & Examnum & " Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                                 " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL and QQ_No <> '" & Examnum & "'  and QQ_No <= '" & Examnum & "' ORDER BY Answer_Id,QQ_No) as a "
            Else
                Dim currentQuestionNo As Integer = CInt(_DB.ExecuteScalar(" SELECT MAX(QQ_No) FROM tblQuizScore WHERE Quiz_Id = '" & QuizId & "' AND Student_Id = '" & StudentId & "';"))

                If Examnum < currentQuestionNo Then
                    sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                                " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL  ORDER BY Answer_Id,QQ_No) as a "
                ElseIf Examnum = currentQuestionNo Then
                    Dim LeapQuestions As Dictionary(Of Integer, Integer) = HttpContext.Current.Session("LeapQuestions")
                    Dim n As Integer = LeapQuestions(Examnum)
                    sql = "SELECT COUNT(*) FROM tblQuizScore WHERE Answer_Id IS NULL AND QQ_No = '" & Examnum & "' AND Quiz_Id = '" & QuizId & "';"
                    If CInt(_DB.ExecuteScalar(sql)) > 0 AndAlso n > 1 Then
                        sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                                " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL  ORDER BY Answer_Id,QQ_No) as a "
                    Else
                        sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                              " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL and QQ_No <> '" & Examnum & "'  ORDER BY Answer_Id,QQ_No) as a "
                    End If
                Else
                    sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                               " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL and QQ_No <> '" & Examnum & "'  ORDER BY Answer_Id,QQ_No) as a "
                End If
            End If

            '& _
            '      '" UNION all " & _
            ''" SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
            ''" AND Student_Id = '" & StudentId & "' AND Answer_Id IS NOT NULL ORDER BY QQ_No) as b "
        End If

        dt = _DB.getdata(sql)
        Return dt
    End Function
    Private Function CreateStringChoicePanel(IsNormalSort As String, IsLastChoice As Boolean, IsAnswered As Boolean, EachQQNo As String, EachisScore As String) As String
        Dim btnChoiceStr As String = "<div id='btnChoice{0}' class='ForBtnChoice'  onclick='LeapChoiceOnclick(""{0}"",""{1}"",""{2}"");' ><img  src='{3}' id='{1}' class='ForBtn' /><br /><span>ข้อที่ {0}</span></div>"
        Dim Answered As String
        Dim imgName As String = ""
        If (IsNormalSort = True) And IsLastChoice And IsAnswered = False Then
            Answered = "F"
            imgName = "../Images/Batch_Runner-big_logo.png"
        Else
            If IsAnswered Then
                Answered = "F"
                imgName = "../Images/Activity/mostWrongFace/veryhappy.png"
            Else
                Answered = "T"
                imgName = "../Images/Activity/mostWrongFace/skipbadge.png"
            End If
        End If
        Return String.Format(btnChoiceStr, EachQQNo, EachisScore, Answered, imgName)
    End Function
#End Region

#Region "Panel เลือกข้อเฉลย"
    <WebMethod(EnableSession:=True)>
    Public Function CreateStringSelectExplain(ByVal StudentId As String, ByVal ExamNum As String, ByVal PageNumber As String, AnsweredMode As Integer) 'Function สร้างปุ่มกดข้ามข้อ
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        'ดึงข้อมูลข้อสอบในควิซ
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim dt As New DataTable
        dt = GetAnswerExplainChoice(QuizId, StudentId, AnsweredMode)

        'ได้ Choice ที่จะแสดงมาแล้ว เริ่มสร้าง Page
        Dim CheckQuantity As Integer = 1
        Dim IsLastPage As String = "False"

        Dim QuizTotalScore As Integer = 0
        Dim PlayerScore As Integer = 0
        Dim RightAmount As Integer = 0
        Dim WrongAmount As Integer = 0
        Dim SkipAmount As Integer = 0
        Dim ArrQuestionNo As String = ""
        Dim LastQuestionNo As String = ""

        CheckQuantity = Math.Floor((dt.Rows.Count()) / 10) 'จำนวน Page

        If (dt.Rows.Count() Mod 10) <> 0 Then
            CheckQuantity = CheckQuantity + 1
        End If

        If PageNumber > CheckQuantity Then
            PageNumber = 1
        ElseIf PageNumber + 1 > CheckQuantity Then
            IsLastPage = "True"
        ElseIf PageNumber = 0 Then
            PageNumber = 1
        End If

        Dim CheckFirstChoice As Boolean = True
        Dim sb As New StringBuilder
        'Dim PageNumber As Integer = 0

        If dt.Rows.Count > 0 Then
            'PageNumber = 1
            Dim EachQQNo As String = ""
            Dim EachIsScore As String = ""
            Dim StartPage As Integer = 0
            Dim EndPage As Integer = 9
            If PageNumber <> "1" Then
                StartPage = (CInt(PageNumber) - 1) * 10
            End If

            If StartPage + 9 < dt.Rows.Count() - 1 Then
                EndPage = StartPage + 9
            Else
                EndPage = dt.Rows.Count() - 1
            End If

            Dim a As Integer = 0

            For i = StartPage To EndPage
                EachQQNo = dt.Rows(i)("QQ_No").ToString() 'รับ QQ_No เพื่อเอามาเป็น Text ให้ปุ่ม

                Dim IsAnswered As String = "0"
                'เช็คตอบหรือยัง
                If dt.Rows(i)("Answered") = "Responce" Then
                    IsAnswered = "1"
                End If

                'เช็คเพื่อให้สร้างปุ่มได้แค่หน้าละ 10 ปุ่มเท่านั้น ถ้าเป็นปุ่มที่หาร 11 ได้ลงตัว หมายถึงต้องขึ้น Page ใหม่
                If CheckFirstChoice Then
                    CheckFirstChoice = False
                    'ถ้าเป็นรอบแรกของการสร้างแต่ละรอบต้องสร้าง Div ขึ้นมาเพื่อครอบก่อนสร้างปุ่ม
                    'If i = 0 Then
                    sb.Append("<div id='divswipe' style='width:550px;margin-top:33px;margin-left:auto; margin-right:auto; display:block;'> ")

                End If

                Dim IsWrongAnswer As Boolean = True
                'เช็คตอบถูกหรือผิด
                If dt.Rows(i)("Score") > 0 Then
                    IsWrongAnswer = False
                End If

                sb.Append(CreateStringSelectExplain(IsAnswered, EachQQNo, IsWrongAnswer, a.ToString))


                a += 1
            Next

            For Each Eachdt In dt.Rows
                ArrQuestionNo &= "," & Eachdt("QQ_No").ToString
            Next
            sb.Append("</div>")
            LastQuestionNo = dt.Rows(dt.Rows.Count - 1)("QQ_No").ToString()
            ArrQuestionNo = ArrQuestionNo.Substring(1, ArrQuestionNo.Length() - 1)
            Dim db As New ClassConnectSql()
            Dim clsAc As New ClsActivity(db)

            'ได้ 10/20 คะแนน ตอบถูก 10 ข้อ ตอบผิด 3 ข้อ ข้ามไม่ตอบ 7 ข้อ

            QuizTotalScore = clsAc.GetTotalScore(QuizId)

            RightAmount = (From row As DataRow In dt.Rows Where row("Score") > 0).Count()
            WrongAmount = (From row As DataRow In dt.Rows Where row("Score") <= 0 And row("Answered") = "Responce").Count()
            SkipAmount = (From row As DataRow In dt.Rows Where row("Answered") = "NotResponce").Count()
            PlayerScore = RightAmount

        End If

        Dim JsonString = New With {.htmlExplainAnswer = sb.ToString(), .CheckOverOnePage = dt.Rows.Count(), .PageNumber = PageNumber, .IsLastPage = IsLastPage,
             .PlayerScore = PlayerScore, .QuizTotalScore = QuizTotalScore, .RightAmount = RightAmount, .WrongAmount = WrongAmount, .SkipAmount = SkipAmount, .ArrNo = ArrQuestionNo, .LastQuestionNo = LastQuestionNo}

        Return js.Serialize(JsonString)

    End Function
    Private Function GetAnswerExplainChoice(QuizId As String, StudentId As String, AnsweredMode As Integer) As DataTable
        Dim sql As String = ""
        sql = "SELECT QQ.QQ_No,QQ.Question_Id,case when Score is null then 0 else Score end as Score
                ,case when ResponseAmount is null or ResponseAmount = 0 then 'NotResponce' else 'Responce' end as Answered 
                FROM tblQuizQuestion as QQ
                left join (select Quiz_Id,Question_Id,Score,ResponseAmount from tblQuizScore 
                where tblQuizScore.Quiz_Id = '" & QuizId & "' 
                AND Student_Id = '" & StudentId & "' )QS 
                on QQ.Quiz_Id = Qs.Quiz_Id and QQ.Question_Id = QS.Question_Id
                WHERE QQ.Quiz_Id = '" & QuizId & "'"

        Select Case AnsweredMode
            Case 1
                sql &= " and Score > 0"
            Case 2
                sql &= " and (Score = 0 and (ResponseAmount <> 0))"
            Case 3
                sql &= " and (ResponseAmount is null or ResponseAmount = 0)"
        End Select

        sql &= " ORDER BY  QQ.QQ_No"

        Dim dt As DataTable
        Dim _DB As New ClassConnectSql
        dt = _DB.getdata(sql)
        Return dt
    End Function
    Private Function CreateStringSelectExplain(IsAnswered As String, EachQQNo As String, IsWrongAnswer As Boolean, OrderNo As String) As String

        Dim btnChoiceStr As String = "<div id='btnChoice{0}' class='ForBtnChoice {4}'  onclick='LeapChoiceOnclick(""{0}"",""{1}"",""{2}"",""{5}"");' ><img src='{3}' id='{1}' class='ForBtn' /><br /><span>ข้อที่ {0}</span></div>"
        Dim Answered As String
        Dim imgName As String = ""
        Dim AnsweredMode As String = ""
        If IsAnswered Then
            Answered = "F"
            If IsWrongAnswer Then
                imgName = "../Images/Activity/SelectExplain/WrongAnswer.png"
                AnsweredMode = "WrongAnswer"
            Else
                imgName = "../Images/Activity/SelectExplain/RightAnswer.png"
                AnsweredMode = "RightAnswer"
            End If


        Else
            Answered = "F"
            imgName = "../Images/Activity/SelectExplain/skipAnswer.png"
            AnsweredMode = "SkipAnswer"
        End If
        Return String.Format(btnChoiceStr, EachQQNo, IsAnswered, Answered, imgName, AnsweredMode, OrderNo)

        'Return sb.ToString
    End Function
#End Region



#Region "Get เฉลย type 3 (โหมดเฉลย)"
    <WebMethod(EnableSession:=True)> _
    Public Function GetCorrectTypeThree(ByVal ExamNum As Integer) As String
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim Player_Id As String = HttpContext.Current.Session("UserId").ToString()
        Dim Sql As String
        Sql = " Select tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name FROM tblQuizQuestion "
        Sql &= " INNER JOIN tblQuizAnswer On tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id And tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
        Sql &= " INNER JOIN tblQuestion On tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
        Sql &= " INNER JOIN tblAnswer On tblQuizAnswer.Question_Id = tblAnswer.Question_Id "
        Sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_Id & "' "
        Sql &= " AND tblQuizQuestion.QQ_No = '" & ExamNum & "' "
        Sql &= " AND tblQuizAnswer.Player_Id = '" & Player_Id & "' "
        Sql &= " ORDER BY tblQuizAnswer.QA_No;"
        Dim db As New ClassConnectSql()
        Dim dtQuestion As DataTable = db.getdata(Sql)

        ' ถ้าข้อสอบทำไม่เคยถึงให้
        If dtQuestion.Rows.Count = 0 Then
            Dim c As New ClsActivity(db)
            dtQuestion = c.GetTempQuestionType3(Quiz_Id, ExamNum)
        End If

        Dim cls As New ClsPDF(db)
        Dim htmlCorrect As New StringBuilder()
        For i = 0 To dtQuestion.Rows.Count - 1
            ' render tag 
            htmlCorrect.Append("<tr style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">")
            htmlCorrect.Append(dtQuestion.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)) & "</td>")
            htmlCorrect.Append("<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""")
            htmlCorrect.Append(dtQuestion.Rows(i)("Question_Id").ToString() & """ class=""drop"" ")
            htmlCorrect.Append("style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span id=""" & dtQuestion.Rows(i)("Answer_Id").ToString() & """ class="""" style=""background-color:#2CA505;color:white;"" >" & dtQuestion.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)) & "</span></td></tr>")
        Next

        Return htmlCorrect.ToString()
    End Function
#End Region
#Region "Get เฉลย type 6 (โหมดเฉลย)"
    <WebMethod(EnableSession:=True)> _
    Public Function GetCorrectTypeSix(ByVal ExamNum As Integer) As String
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim Player_Id As String = HttpContext.Current.Session("UserId").ToString()
        Dim Sql As String
        Sql = "SELECT ROW_NUMBER() over(order by cast(Answer_Name as varchar(max)))as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id"
        Sql &= " FROM tblQuizAnswer INNER JOIN"
        Sql &= " tblQuizQuestion ON tblQuizAnswer.Question_Id = tblQuizQuestion.Question_Id "
        Sql &= " AND tblQuizAnswer.Quiz_Id = tblQuizQuestion.Quiz_Id INNER JOIN"
        Sql &= " tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id INNER JOIN"
        Sql &= " tblAnswer ON tblQuestion.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id AND "
        Sql &= " tblQuizAnswer.Question_Id = tblAnswer.Question_Id And tblQuizQuestion.Question_Id = tblAnswer.Question_Id"
        Sql &= " where tblQuizAnswer.Quiz_Id = '" & Quiz_Id & "' "
        Sql &= " and player_id = '" & Player_Id & "' "
        Sql &= " and tblQuizQuestion.Question_Id in (select Question_Id from tblQuizQuestion where QQ_No = '" & ExamNum & "' and Quiz_Id = '" & Quiz_Id & "') "
        Sql &= " order by CAST(Answer_Name as varchar(max));"
        Dim db As New ClassConnectSql()
        Dim dtQuestion As DataTable = db.getdata(Sql)
        Dim cls As New ClsPDF(db)
        Dim htmlCorrect As New StringBuilder()
        htmlCorrect.Append("<tr id=""Answer""><td><ul style=""margin-left:-40px;"" >")
        For i = 0 To dtQuestion.Rows.Count - 1
            htmlCorrect.Append("<li id=""" & dtQuestion.Rows(i)("Question_Id").ToString & """ style=""background-color:#1EEE1E;""><span class=""CorrectLi"">ลำดับที่ " & dtQuestion.Rows(i)("Number").ToString & " </span>" & dtQuestion.Rows(i)("Question_Name").Replace("___MODULE_URL___", cls.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)) & "</li>")
        Next
        htmlCorrect.Append("</ul></td></tr>")
        Return htmlCorrect.ToString()
    End Function
#End Region

End Class
