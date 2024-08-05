Imports System.Web.Script.Serialization

Public Class DialyActivityManagement

    Private db As New ClassConnectSql()

    Private Property SchoolCode As String
    Private Property CalendarId As String


    Public Sub New()
        Me.SchoolCode = BusinessTablet360.ClsKNSession.DefaultSchoolCode
        Me.CalendarId = GetCalendarID()
    End Sub

    Private Function GetCalendarID() As String
        Dim sql As String = " Select TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate And Calendar_ToDate And Calendar_Type = 3 And School_Code = '" & Me.SchoolCode & "' AND IsActive = 1; "
        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)
        Dim calendarId As String = ""
        If dt.Rows.Count > 0 Then
            calendarId = dt.Rows(0)("Calendar_Id").ToString()
        Else
            sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE Calendar_Type = '3' AND School_Code = '" & Me.SchoolCode & "' AND dbo.GetThaiDate() >= Calendar_ToDate AND IsActive = 1 ORDER BY Calendar_ToDate DESC; "
            dt = db.getdata(sql)
            If dt.Rows.Count > 0 Then calendarId = dt.Rows(0)("Calendar_Id")
        End If
        Return calendarId
    End Function

    Public Sub Run()
        'Dim studentList As DataTable = GetStudents()
        'For Each r In studentList.Rows
        '    Dim questionAmountToRandom As Integer = GetAmountToRandom(r("Student_CurrentClass"), r("SubjectAmount"))
        '    Dim subjectList As New List(Of String)(r("SubjectList").ToString().Split(","c))
        '    For Each subjectId In subjectList
        '        If IsModuleSubjectCompletion(r("Student_Id").ToString(), subjectId.ToString()) Then
        '            'Dim dtQuestions As DataTable = GetQuestions(questionAmountToRandom, r("Student_Id").ToString(), subjectId.ToString())
        '            'If dtQuestions.Rows.Count = 0 Then dtQuestions = GetQuestionsForNewRegister(questionAmountToRandom, subjectId.ToString(), r("Student_CurrentClass"))
        '            Dim dtQuestions As DataTable = GetQuestionsForNewRegister(questionAmountToRandom, subjectId.ToString(), r("Student_CurrentClass"))
        '            If dtQuestions.Rows.Count > 0 Then
        '                CreateActivity(r("Student_Id").ToString(), subjectId.ToString(), dtQuestions)
        '            End If
        '        Else
        '            UpdateActivityWhenDidNotDo(r("Student_Id").ToString(), subjectId.ToString())
        '        End If
        '    Next
        'Next
    End Sub

    Public Sub Run(StudentId As String)
        'Dim studentList As DataTable = GetStudent(StudentId)
        'For Each r In studentList.Rows
        '    Dim questionAmountToRandom As Integer = GetAmountToRandom(r("Student_CurrentClass"), r("SubjectAmount"))
        '    Dim subjectList As New List(Of String)(r("SubjectList").ToString().Split(","c))
        '    For Each subjectId In subjectList
        '        If IsModuleSubjectCompletion(r("Student_Id").ToString(), subjectId.ToString()) Then
        '            Dim dtQuestions As DataTable = GetQuestionsForNewRegister(questionAmountToRandom, subjectId.ToString(), r("Student_CurrentClass"))
        '            If dtQuestions.Rows.Count > 0 Then
        '                CreateActivity(r("Student_Id").ToString(), subjectId.ToString(), dtQuestions)
        '            End If
        '        Else
        '            UpdateActivityWhenDidNotDo(r("Student_Id").ToString(), subjectId.ToString())
        '        End If
        '    Next
        'Next
    End Sub

    'Public Sub RunDialyActivity(studentId As String)
    '    Dim dtSubjects As DataTable = GetStudentSubjects(studentId)
    '    Dim tempSubjectId As New List(Of String)
    '    For Each r In dtSubjects.Rows
    '        Dim questionAmountToRandom As Integer = 10
    '        Dim subjectId As String = r("SS_SubjectId").ToString()
    '        Dim levelId As String = r("SS_LevelId").ToString()

    '        If Not tempSubjectId.Contains(subjectId) Then
    '            tempSubjectId.Add(subjectId)

    '            If IsModuleSubjectCompletion(studentId, subjectId) Then
    '                Dim dtQuestions As DataTable = GetDialyQuestions(questionAmountToRandom, subjectId, levelId)
    '                If dtQuestions.Rows.Count > 0 Then
    '                    CreateActivity(studentId, subjectId, dtQuestions)
    '                End If
    '            Else
    '                UpdateActivityWhenDidNotDo(studentId, subjectId)
    '            End If
    '        End If
    '    Next
    'End Sub

    ''' <summary>
    ''' function ในการสร้างกิจกรรมประจำวัน และ return url ในการทำควิซออกไป
    ''' </summary>
    ''' <param name="uda"></param>
    Public Function GetUrlDialyActivity(uda As UserDialyActivity) As String
        Dim dtQuestions As New DataTable
        ' get qset แบบมีจำนวนข้อ
        'Dim dtQuestionsSet As DataTable = GetQuestionsSet(uda)
        '' random ข้อสอบแบบชุดคำถามต่อเนื่องที่มีจำนวนข้อน้อยกว่าหรือเท่ากับที่ใส่มา
        'Dim q = (From r In dtQuestionsSet.AsEnumerable Where r("QuestionAmount") <= uda.QuestionAmount).FirstOrDefault()
        'If q IsNot Nothing Then
        '    Dim qsetId As String = q.Field(Of Guid)("Qset_Id").ToString()
        '    dtQuestions = GetQuestionsInQset(qsetId)
        '    'เอาจำนวนข้อสอบแบบชุดมาลบกับจำนวนข้อสอบที่ต้องการทำกิจกรรม
        '    Dim remaining As Integer = uda.QuestionAmount - dtQuestions.Rows.Count
        '    If remaining > 0 Then
        '        'เพิ่มข้อสอบเติมเข้าไปใส่กับข้อสอบแบบชุด เมื่อจำนวนข้อสอบยังไม่ครบ
        '        Dim tempQuestion As DataTable = GetDialyQuestions(remaining, uda.SubjectId, uda.LevelId)
        '        For Each r As DataRow In tempQuestion.Rows
        '            Dim newRow As DataRow = dtQuestions.NewRow
        '            newRow(0) = r("Question_Id")
        '            newRow(1) = r("Qset_Id")
        '            dtQuestions.Rows.Add(newRow)
        '        Next
        '    End If
        'Else
        dtQuestions = GetDialyQuestions(uda)
        'End If

        If dtQuestions.Rows.Count > 0 Then
            uda.Questions = dtQuestions
            Return CreateActivity(uda)
        End If
        Return ""
    End Function

    ''' <summary>
    ''' function get คำถามทั้งหมด ทีอยู่ใน qset
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <returns></returns>
    Private Function GetQuestionsInQset(qsetId As String) As DataTable
        Dim sql As String = "SELECT Question_Id,QSet_Id FROM tblQuestion WHERE Qset_Id = '" & qsetId & "' AND IsActive = 1 ORDER BY Question_No;"
        Return db.getdata(sql)
    End Function

    ''' <summary>
    ''' function ในการ get คำถามแบบชุด เพื่อเอาไปผสมกับข้อสอบแบบที่สามารถสุ่มได้ เพื่อสร้างกิจกรรมประจำวัน
    ''' </summary>
    ''' <param name="uda"></param>
    ''' <returns></returns>
    Private Function GetQuestionsSet(uda As UserDialyActivity) As DataTable
        Dim sql As New StringBuilder
        sql.Append(" SELECT q.QSet_Id,g.GroupSubject_Id,COUNT(q.Qset_Id) AS QuestionAmount FROM tblQuestion q ")
        sql.Append(" INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id  ")
        sql.Append(" INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject g ON b.GroupSubject_Id = g.GroupSubject_Id  ")
        sql.Append(" INNER JOIN tblLevel l ON l.Level_Id = b.Level_Id ")
        sql.Append(" WHERE q.IsActive = 1 AND  g.GroupSubject_Id = '" & uda.SubjectId & "' AND l.Level_Id = '" & uda.LevelId & "' ")
        sql.Append(" And qs.QSet_Type <> 3 And qs.Qset_IsRandomQuestion = 0 And qs.IsActive = 1 And q.IsActive = 1 AND b.Book_Syllabus = '51' ")
        sql.Append(" AND (q.IsWpp = 1 OR q.IsWpp IS NULL) AND (qs.IsWpp = 1 OR qs.IsWpp IS NULL ) AND (qc.IsWpp = 1 OR qc.IsWpp IS NULL ) ")
        sql.Append(" GROUP BY q.Qset_Id,g.GroupSubject_Id ORDER BY NEWID();")
        Return db.getdata(sql.ToString())
    End Function

    Private Function GetStudentSubjects(studentId As String) As DataTable
        Dim sql As String = "SELECT * FROM maxonet_tblStudentSubject WHERE SS_StudentId = '" & studentId & "' ORDER BY SS_SubjectId;"
        Return db.getdata(sql)
    End Function

    Private Sub UpdateActivityWhenDidNotDo(studentId As String, subjectId As String)
        Dim sql As New StringBuilder()
        sql.Append(" UPDATE tblQuiz SET StartTime = dbo.GetThaiDate() WHERE Quiz_Id = ( ")
        sql.Append(" Select TOP 1 md.Quiz_Id from tblModuleAssignmentDetail ma INNER JOIN tblModuleDetailCompletion md On ma.MA_Id = md.MA_Id ")
        sql.Append(" WHERE ma.Student_Id = '" & studentId & "' AND ma.Subject_Id = '" & subjectId & "'  ORDER BY md.LastUpdate DESC);")
        db.Execute(sql.ToString())
    End Sub

    ''' <summary>
    ''' function หานักเรียน เพื่อทำการสร้างชุดกิจกรรมให้ 
    ''' </summary>
    ''' <returns>DataTable</returns>
    Private Function GetStudents() As DataTable
        Dim sql As New StringBuilder()
        sql.Append("Select Student_Id,Student_CurrentClass,COUNT(Student_Id) As SubjectAmount, ")
        sql.Append(" STUFF((Select DISTINCT ',' + CONVERT(nvarchar(50), SS_SubjectId) FROM maxonet_tblStudentSubject WHERE SS_StudentId = Student_Id FOR  XML PATH ('')),1,1,'') AS SubjectList ")
        sql.Append(" FROM (SELECT s.Student_Id,s.Student_CurrentClass,m.SS_SubjectId FROM t360_tblStudent s INNER JOIN maxonet_tblStudentSubject m ON s.Student_Id = m.SS_StudentId ) a ")
        sql.Append(" INNER JOIN t360_tblTabletOwner t ON t.Owner_Id = Student_Id INNER JOIN t360_tblTablet tl ON tl.Tablet_Id = t.Tablet_Id WHERE t.TabletOwner_Isactive = 1 AND tl.Tablet_Isactive = 1 ")
        sql.Append(" GROUP BY Student_Id, Student_CurrentClass;")
        Return db.getdata(sql.ToString())
    End Function

    ''' <summary>
    ''' function หานักเรียน แค่คนเดียว
    ''' </summary>
    ''' <returns></returns>
    Private Function GetStudent(StudentId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT Student_Id,Student_CurrentClass,COUNT(Student_Id) AS SubjectAmount, ")
        sql.Append(" STUFF((SELECT DISTINCT ',' + CONVERT(nvarchar(50), SS_SubjectId) FROM maxonet_tblStudentSubject WHERE SS_StudentId = Student_Id FOR  XML PATH ('')),1,1,'') AS SubjectList ")
        sql.Append(" FROM (SELECT s.Student_Id,s.Student_CurrentClass,m.SS_SubjectId FROM t360_tblStudent s INNER JOIN maxonet_tblStudentSubject m ON s.Student_Id = m.SS_StudentId ) a ")
        sql.Append(" WHERE a.Student_Id = '" & StudentId & "' ")
        sql.Append(" GROUP BY Student_Id, Student_CurrentClass;")
        Return db.getdata(sql.ToString())
    End Function

    Private Function IsModuleSubjectCompletion(StudentId As String, GroupSubjectId As String) As Boolean
        Dim sql As New StringBuilder
        sql.Append(" select md.Module_Status from tblModuleAssignmentDetail ma INNER JOIN tblModuleDetailCompletion md ON ma.MA_Id = md.MA_Id ")
        sql.Append(" WHERE ma.Student_Id = '" & StudentId & "' AND ma.Subject_Id = '" & GroupSubjectId & "'  ORDER BY md.LastUpdate DESC;")
        Dim dt As DataTable = db.getdata(sql.ToString())
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Module_Status") = 0 Then Return False
        End If
        Return True
    End Function

    Private Function GetAmountToRandom(ClassName As String, SubjectAmount As Integer) As Integer
        Dim n As Integer = 10
        Select Case SubjectAmount
            Case 1
                n = 20
            Case 3
                n = 15
            Case 5
                n = 10
            Case Else
                n = 10
        End Select

        '' ประถมต้น-ปลาย
        If ClassName.IndexOf("ป.") > -1 Then Return n
        '' มัธยมต้น
        If CInt(ClassName.Replace("ม.", "")) < 4 Then Return n + 5
        '' มัธยมปลาย
        Return n + 10
    End Function

    Private Function GetQuestions(val As Integer, StudentId As String, GroupSubjectId As String) As DataTable
        Dim sql As String = String.Format("SELECT * FROM (SELECT TOP {0} * FROM tmpDaily2 WHERE Student_Id = '{1}' AND GroupSubject_Id = '{2}' ORDER BY NEWID()) a ORDER BY a.Qset_Id;", val, StudentId, GroupSubjectId)
        Return db.getdata(sql)
    End Function

    Private Function GetQuestionsForNewRegister(val As Integer, GroupSubjectId As String, StudentClass As String)
        Dim sql As New StringBuilder
        sql.Append(" SELECT TOP {0} q.Question_Id,q.QSet_Id,g.GroupSubject_Id FROM tblQuestion q ")
        sql.Append(" INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id ")
        sql.Append(" INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject g ON b.GroupSubject_Id = g.GroupSubject_Id ")
        sql.Append(" INNER JOIN tblLevel l ON l.Level_Id = b.Level_Id ")
        sql.Append(" WHERE q.IsActive = 1 AND  g.GroupSubject_Id = '{1}' AND l.Level_ShortName = N'{2}' AND qs.QSet_Type <> 3 ")
        sql.Append(" ORDER BY NEWID();")
        Return db.getdata(String.Format(sql.ToString(), val, GroupSubjectId, StudentClass))
    End Function

    ''' <summary>
    ''' function get คำถามแบบสุ่ม เพิ่อเอาไปรวมกับคำถามแบบชุด
    ''' </summary>
    ''' <param name="questionAmount"></param>
    ''' <param name="GroupSubjectId"></param>
    ''' <param name="LevelId"></param>
    ''' <returns></returns>
    Private Function GetDialyQuestions(questionAmount As Integer, GroupSubjectId As String, LevelId As String)
        Dim sql As New StringBuilder
        sql.Append(" SELECT TOP {0} q.Question_Id,q.QSet_Id FROM tblQuestion q ")
        sql.Append(" INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id ")
        sql.Append(" INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject g ON b.GroupSubject_Id = g.GroupSubject_Id ")
        sql.Append(" INNER JOIN tblLevel l ON l.Level_Id = b.Level_Id ")
        sql.Append(" WHERE q.IsActive = 1 AND  g.GroupSubject_Id = '{1}' AND l.Level_Id = '{2}' AND qs.QSet_Type <> 3 ")
        sql.Append(" AND qs.Qset_IsRandomQuestion = 1 AND qs.IsActive = 1 AND q.IsActive = 1 AND b.book_syllabus = '51' ")
        sql.Append(" AND (q.IsWpp = 1 OR q.IsWpp IS NULL) AND (qs.IsWpp = 1 OR qs.IsWpp IS NULL ) AND (qc.IsWpp = 1 OR qc.IsWpp IS NULL ) ")
        sql.Append(" ORDER BY NEWID();")
        Return db.getdata(String.Format(sql.ToString(), questionAmount, GroupSubjectId, LevelId))
    End Function

    ''' <summary>
    ''' function ในการสุ่มข้่อสอบเพื่อมาสร้างเป็นกิจกรรมประจำวัน
    ''' </summary>
    ''' <param name="uda"></param>
    ''' <returns></returns>
    Private Function GetDialyQuestions(uda As UserDialyActivity)
        Dim sql As New StringBuilder
        sql.Append(" SELECT TOP {0} q.Question_Id,q.QSet_Id,g.GroupSubject_Id FROM tblQuestion q ")
        sql.Append(" INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id ")
        sql.Append(" INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject g ON b.GroupSubject_Id = g.GroupSubject_Id ")
        sql.Append(" INNER JOIN tblLevel l ON l.Level_Id = b.Level_Id ")
        sql.Append(" WHERE q.IsActive = 1 AND  g.GroupSubject_Id = '{1}' AND l.Level_Id = '{2}' AND qs.QSet_Type <> 3 AND qs.Qset_IsRandomQuestion = 1 AND qs.IsActive = 1 AND q.IsActive = 1 AND b.book_syllabus = '51' ")
        sql.Append(" AND (q.IsWpp = 1 OR q.IsWpp IS NULL) AND (qs.IsWpp = 1 OR qs.IsWpp IS NULL ) AND (qc.IsWpp = 1 OR qc.IsWpp IS NULL ) ")
        sql.Append(" ORDER BY NEWID();")
        Return db.getdata(String.Format(sql.ToString(), uda.QuestionAmount, uda.SubjectId, uda.LevelId))
    End Function

    Private Sub CreateActivity(StudentId As String, SubjectId As String, QuestionsList As DataTable)
        Try

            db.OpenWithTransection()

            Dim sql As New StringBuilder()

            ' testset variable
            Dim testsetName As String = "DA_" & SubjectId.ToSubjectShortThName
            Dim testsetId As String = db.ExecuteScalarWithTransection("Select NEWID();")
            Dim qSetId As String = ""
            Dim tsqsId As String = ""

            ' testset sql
            sql.Append(" INSERT INTO tblTestSet VALUES('" & testsetId & "',N'" & testsetName & "',0,'" & SchoolCode & "',NULL,60,1,dbo.GetThaiDate(),NULL,")
            sql.Append(" 0,1,0,0,0,0,'" & StudentId & "',2,'" & CalendarId & "',N'" & SubjectId.ToGroupSubjectThName & "',N'" & SubjectId.ToSubjectShortThName & "',NULL,NULL);")
            Dim i As Integer = 1
            Dim j As Integer
            For Each r In QuestionsList.Rows
                If qSetId = "" OrElse qSetId <> r("Qset_Id").ToString() Then
                    qSetId = r("Qset_Id").ToString()
                    tsqsId = db.ExecuteScalarWithTransection("SELECT NEWID();")
                    sql.Append(" INSERT INTO tblTestSetQuestionSet VALUES('" & tsqsId & "','" & testsetId & "','" & qSetId & "'," & i & ",NULL,1,dbo.GetThaiDate(),NULL);")
                    i += 1
                    j = 1
                End If
                sql.Append(" INSERT INTO tblTestSetQuestionDetail VALUES(NEWID(),'" & tsqsId & "','" & r("Question_Id").ToString() & "'," & j & ",1,dbo.GetThaiDate(),NULL);")
                j += 1
            Next
            db.ExecuteWithTransection(sql.ToString())
            sql.Clear()

            ' module variable
            Dim moduleId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            Dim moduleDetailId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            Dim maId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            ' quiz variable
            Dim quizId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")

            ' module sql
            sql.Append(" INSERT INTO tblModule VALUES('" & moduleId & "',N'" & testsetName & "',NULL,1,'" & SchoolCode & "',dbo.GetThaiDate(),'" & StudentId & "',NULL); ")
            sql.Append(" INSERT INTO tblModuleDetail VALUES('" & moduleDetailId & "','" & moduleId & "','" & testsetId & "', 0, 1, dbo.GetThaiDate(), NULL); ")
            sql.Append(" INSERT INTO tblModuleAssignment VALUES('" & maId & "','" & moduleId & "', dbo.GetThaiDate(), NULL, 1, dbo.GetThaiDate(),'" & CalendarId & "','" & StudentId & "',NULL); ")
            sql.Append(" INSERT INTO tblModuleDetailCompletion VALUES(NEWID(),'" & moduleDetailId & "','" & maId & "','" & StudentId & "','" & quizId & "',0,NULL,dbo.GetThaiDate(),'" & SchoolCode & "',1,dbo.GetThaiDate(),NULL); ")
            sql.Append(" INSERT INTO tblModuleAssignmentDetail VALUES(NEWID(),'" & maId & "', 1, dbo.GetThaiDate(), NULL,'" & StudentId & "','" & SubjectId & "',NULL,NULL);")
            db.ExecuteWithTransection(sql.ToString())
            sql.Clear()

            Dim toolsNumber As Integer = 16
            If SubjectId.ToLower() = CommonSubjectsText.EnglishSubjectId.ToLower() Then
                toolsNumber += 12 '4,8 word , wordbook
            ElseIf SubjectId.ToLower() = CommonSubjectsText.MathSubjectId.ToLower() Then
                toolsNumber += 34 '2,32 math,protractor
            End If
            ' quiz sql
            sql.Append("INSERT INTO tblQuiz VALUES ('" & quizId & "',N'" & testsetId & "',NULL,NULL,1,dbo.GetThaiDate(),NULL,0,0,1500,50,0,0,30,0,0,0,0,1,dbo.GetThaiDate(),1,'" & SchoolCode & "','" & SchoolCode & "',0,0,1,0,0,0,0,1,1,NULL,'" & StudentId & "'," & toolsNumber & "," & QuestionsList.Rows.Count & ",'" & CalendarId & "',NULL,0);")
            i = 1
            For Each r In QuestionsList.Rows
                sql.Append("INSERT INTO tblQuizQuestion VALUES (NEWID(),'" & quizId & "','" & r("Question_Id").ToString() & "'," & i & ",dbo.GetThaiDate(),1,0,NULL,'" & SchoolCode & "');")
                i += 1
            Next
            Dim tabletId As String = db.ExecuteScalarWithTransection("SELECT Tablet_Id FROM t360_tblTabletOwner WHERE TabletOwner_IsActive = 1 AND Owner_Id = '" & StudentId & "'; ")
            sql.Append("  INSERT INTO tblQuizSession VALUES (NEWID(),'" & SchoolCode & "',NULL,2,'" & quizId & "',NULL,1,dbo.GetThaiDate(),'" & StudentId & "','" & tabletId & "',0,0,NULL);")

            db.ExecuteWithTransection(sql.ToString())
            db.CommitTransection()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Sub


    ''' <summary>
    ''' function สำหรับสร้างกิจกรรมประจำวันโดยเลือกชั้นและวิชามา
    ''' </summary>
    ''' <param name="uda"></param>
    Private Function CreateActivity(uda As UserDialyActivity) As String
        Try
            db.OpenWithTransection()

            Dim sql As New StringBuilder()

            ' testset variable
            Dim testsetName As String = "DA_" & uda.SubjectId.ToSubjectShortThName
            Dim testsetId As String = db.ExecuteScalarWithTransection("Select NEWID();")
            Dim qSetId As String = ""
            Dim tsqsId As String = ""

            ' testset sql
            sql.Append(" INSERT INTO tblTestSet VALUES('" & testsetId & "',N'" & testsetName & "',0,'" & SchoolCode & "',NULL,60,1,dbo.GetThaiDate(),NULL,")
            sql.Append(" 0,1,0,0,0,0,'" & uda.StudentId & "',2,'" & CalendarId & "',N'" & uda.SubjectId.ToGroupSubjectThName & "',N'" & uda.SubjectId.ToSubjectShortThName & "',NULL,NULL);")
            Dim i As Integer = 1
            Dim j As Integer
            For Each r In uda.Questions.Rows
                If qSetId = "" OrElse qSetId <> r("Qset_Id").ToString() Then
                    qSetId = r("Qset_Id").ToString()
                    tsqsId = db.ExecuteScalarWithTransection("SELECT NEWID();")
                    sql.Append(" INSERT INTO tblTestSetQuestionSet VALUES('" & tsqsId & "','" & testsetId & "','" & qSetId & "'," & i & ",NULL,1,dbo.GetThaiDate(),NULL);")
                    i += 1
                    j = 1
                End If
                sql.Append(" INSERT INTO tblTestSetQuestionDetail VALUES(NEWID(),'" & tsqsId & "','" & r("Question_Id").ToString() & "'," & j & ",1,dbo.GetThaiDate(),NULL);")
                j += 1
            Next
            db.ExecuteWithTransection(sql.ToString())
            sql.Clear()

            ' module variable
            Dim moduleId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            Dim moduleDetailId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            Dim maId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
            ' quiz variable
            Dim quizId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")

            ' module sql
            sql.Append(" INSERT INTO tblModule VALUES('" & moduleId & "',N'" & testsetName & "',NULL,1,'" & SchoolCode & "',dbo.GetThaiDate(),'" & uda.StudentId & "',NULL); ")
            sql.Append(" INSERT INTO tblModuleDetail VALUES('" & moduleDetailId & "','" & moduleId & "','" & testsetId & "', 0, 1, dbo.GetThaiDate(), NULL); ")
            sql.Append(" INSERT INTO tblModuleAssignment VALUES('" & maId & "','" & moduleId & "', dbo.GetThaiDate(), NULL, 1, dbo.GetThaiDate(),'" & CalendarId & "','" & uda.StudentId & "',NULL); ")
            sql.Append(" INSERT INTO tblModuleDetailCompletion VALUES(NEWID(),'" & moduleDetailId & "','" & maId & "','" & uda.StudentId & "','" & quizId & "',0,NULL,dbo.GetThaiDate(),'" & SchoolCode & "',1,dbo.GetThaiDate(),NULL); ")
            sql.Append(" INSERT INTO tblModuleAssignmentDetail VALUES(NEWID(),'" & maId & "', 1, dbo.GetThaiDate(), N'" & uda.LevelId.ToLevelShortName & "','" & uda.StudentId & "','" & uda.SubjectId & "',NULL,NULL);")
            db.ExecuteWithTransection(sql.ToString())
            sql.Clear()

            Dim toolsNumber As Integer = 16
            If uda.SubjectId.ToLower() = CommonSubjectsText.EnglishSubjectId.ToLower() Then
                toolsNumber += 12 '4,8 word , wordbook
            ElseIf uda.SubjectId.ToLower() = CommonSubjectsText.MathSubjectId.ToLower() Then
                toolsNumber += 34 '2,32 math,protractor
            End If
            ' quiz sql
            sql.Append("INSERT INTO tblQuiz VALUES ('" & quizId & "',N'" & testsetId & "',NULL,NULL,1,dbo.GetThaiDate(),NULL,0,0,1500,50,1,0,30,1,0,0,0,1,dbo.GetThaiDate(),1,'" & SchoolCode & "','" & SchoolCode & "',0,0,1,0,0,0,0,1,1,NULL,'" & uda.StudentId & "'," & toolsNumber & "," & uda.Questions.Rows.Count & ",'" & CalendarId & "',NULL,0);")
            i = 1
            For Each r In uda.Questions.Rows
                sql.Append("INSERT INTO tblQuizQuestion VALUES (NEWID(),'" & quizId & "','" & r("Question_Id").ToString() & "'," & i & ",dbo.GetThaiDate(),1,0,NULL,'" & SchoolCode & "');")
                i += 1
            Next
            Dim tabletId As String = db.ExecuteScalarWithTransection("SELECT Tablet_Id FROM t360_tblTabletOwner WHERE TabletOwner_IsActive = 1 AND Owner_Id = '" & uda.StudentId & "'; ")
            sql.Append("  INSERT INTO tblQuizSession VALUES (NEWID(),'" & SchoolCode & "',NULL,2,'" & quizId & "',NULL,1,dbo.GetThaiDate(),'" & uda.StudentId & "','" & tabletId & "',0,0,NULL);")

            db.ExecuteWithTransection(sql.ToString())
            db.CommitTransection()

            'return url สำหรับเข้าทำกิจกรรมประจำวัน
            Dim urlActivity As String = "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & uda.DeviceId & "&token=" & uda.TokenId & "&Status=0&ItemId=" & quizId
            Return urlActivity
        Catch ex As Exception
            db.RollbackTransection()
            Return ""
        End Try
    End Function

    Public Function CreateDailyByLevelAndSubject(studentId As String, subjectId As String) As String
        ' หาดูว่าในวิชานี้ ลงทะเบียนชั้นไหนบ้าง
        Dim dtStudentSubjects As DataTable = GetStudentSubjects(studentId, subjectId)
        ' ดึงกิจกรรมที่ทำในวันนี้ และวิชานี้
        Dim dtActivities As DataTable = GetStudentActivityToday(studentId, subjectId)

        Dim content As New StringBuilder
        content.Append("<div>")
        content.Append("<span class='menu'>เลือกชั้น</span><div class='chooseClass'>")
        ' loop check ว่า วิชาชั้นนี้ทำกิจกรรมไปหรือยัง
        For Each r In dtStudentSubjects.Rows
            Dim levelId As String = r("SS_LevelId").ToString()
            Dim levelShortName As String = r("Level_ShortName").ToString()
            'Dim q = (From m In dtActivities.AsEnumerable Where m.Field(Of String)("Class_Name") = levelShortName).SingleOrDefault()
            'Dim className As String = If((q Is Nothing), "classDiv", "success")
            'Dim className As String = If((q Is Nothing), "classDiv", "classDiv")
            'content.Append("<div class='" & className & "' id='" & levelId & "'>" & levelShortName & "</div>")
            content.Append("<div class='classDiv' id='" & levelId & "'>" & levelShortName & "</div>")
        Next

        Dim MONM As New MaxOnetManagement
        Dim IsLimt As String = MONM.IsLimitExam(studentId)


        content.Append("</div>")
        content.Append("<span class='menu'>จำนวนข้อสอบที่ต้องการทำ</span>")
        content.Append("<div class='chooseQuestionAmount' >")

        If IsLimt <> "" Then

            content.Append("<div style='display:none;' class='questionAmountDiv' id='LimitAmount' value='" & IsLimt & "'></div>")

            If CInt(IsLimt) >= 10 Then
                content.Append("<div class='questionAmountDiv Active' value='10'>10 ข้อ</div>")
            End If

            If CInt(IsLimt) >= 15 Then
                content.Append("<div class='questionAmountDiv' value='15'>15 ข้อ</div>")
            End If

            If CInt(IsLimt) >= 20 Then
                content.Append("<div class='questionAmountDiv' value='20'>20 ข้อ</div>")
            End If

            content.Append("<div class='questionAmountDiv' value='' >")
            content.Append("<input type='text' id='OtherQuetionAmount' maxlength='3' onkeypress='return event.charCode >= 48 && event.charCode <= 57' />")

        Else
            content.Append("<div class='questionAmountDiv' value='10'>10 ข้อ</div>")
            content.Append("<div class='questionAmountDiv' value='15'>15 ข้อ</div>")
            content.Append("<div class='questionAmountDiv' value='20'>20 ข้อ</div>")
            content.Append("<div class='questionAmountDiv' value='' >")
            content.Append("<input type='text' id='OtherQuetionAmount' maxlength='3' onkeypress='return event.charCode >= 48 && event.charCode <= 57' />")
        End If

        content.Append("</div>")
        content.Append("</div>")
        content.Append("</div>")
        Return content.ToString()
    End Function

    Private Function GetStudentActivityToday(studentId As String, subjectId As String) As DataTable
        'Dim sql As String = "SELECT * FROM tblModuleAssignmentDetail  mad INNER JOIN tblModuleAssignment ma ON mad.Ma_Id = ma.Ma_Id  "
        'sql &= " WHERE Student_Id = '" & studentId & "'  AND Subject_Id = '" & subjectId & "' "
        'sql &= " AND ma.Start_Date BETWEEN DATEADD(DAY,DATEDIFF(DAY,0,dbo.GetThaiDate()),0) AND dbo.GetThaiDate() and End_Date is not null;"

        '16/8/2017 ไหมแก้ให้ดูที่ tblQuiz ด้วยว่าได้ทำการกดส่งหรือกดจบหรือไม่ ถ้าไม่หมายถึงอาจจะกดออกไปหรือหลุดไป ไม่ได้ออกจากการทำกิจกรรมเอง ให้ทำกิจกรรมใหม่อีกครั้งได้ 
        Dim sql As String = ""
        sql = "SELECT Class_Name,mad.Student_Id,Subject_Id ,q.EndTime,q.StartTime
                FROM tblModuleAssignmentDetail  mad INNER JOIN tblModuleAssignment ma ON mad.Ma_Id = ma.Ma_Id
                inner join tblModuleDetail md on ma.Module_Id = md.Module_Id
                inner join tblModuleDetailCompletion mdc on md.ModuleDetail_Id = mdc.ModuleDetail_Id
                inner join tblQuiz q on mdc.Quiz_Id = q.Quiz_Id
                WHERE mad.Student_Id = '" & studentId & "' AND Subject_Id = '" & subjectId & "'
                AND ma.Start_Date BETWEEN DATEADD(DAY,DATEDIFF(DAY,0,dbo.GetThaiDate()),0) AND dbo.GetThaiDate() 
                and EndTime is not null order by ma.LastUpdate desc"
        Return db.getdata(sql)
    End Function

    Private Function GetStudentSubjects(studentId As String, subjectId As String) As DataTable
        Dim sql As String = ""

        sql = "SELECT mss.*,l.Level_ShortName FROM maxonet_tblstudentsubject  mss INNER JOIN tblLevel l ON mss.SS_LevelId = l.Level_Id  
                                inner join maxonet_tblKeyCodeUsage on KeyCode_Code = SS_KeyCode
                                WHERE mss.SS_StudentId = '" & studentId & "' AND mss.SS_SubjectId = '" & subjectId & "'  
                                and (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null) and mss.SS_IsActive = 1 
                                and maxonet_tblKeyCodeUsage.KCU_IsActive = 1 and (KCU_Type = 0 or KCU_Type = 3 or KCU_Type = 4) ORDER BY Level;"
        Return db.getdata(sql)
    End Function

    Public Function IsSubjectActivitySuccess(studentId As String, subjectId As String) As Boolean

        'If subjectId.ToUpper = "FB677859-87DA-4D8D-A61E-8A76566D69D8" Then
        '    'วิชาอังกฤษปิดไม่ใช้
        '    Return True
        'Else
        'Dim dtStudentSubjects As DataTable = GetStudentSubjects(studentId, subjectId)
        '    Dim dtActivities As DataTable = GetStudentActivityToday(studentId, subjectId)

        'เฉพาะกิจ ปิดวิชาอังกฤษ ม.ปลาย ออกไปก่อน เพราะข้อสอบยังไม่มี
        'If subjectId.ToUpper = "FB677859-87DA-4D8D-A61E-8A76566D69D8" Then
        '    Dim i As Integer = dtStudentSubjects.Rows.Count
        '    For Each r In dtStudentSubjects.Rows
        '        Dim levelId As String = r("SS_LevelId").ToString()
        '        If levelId.ToUpper = "2E0FFC04-BCEE-45BE-9C0C-B40742523F43" Or levelId.ToUpper = "6736D029-6B78-4570-9DBB-991217DA8FEE" Or levelId.ToUpper = "6BF52DC7-314C-40ED-B7F3-BCC87F724880" Then
        '            i = i - 1
        '        End If
        '    Next
        '    If i = dtActivities.Rows.Count Then
        '        Return True
        '    End If
        '    Return False
        'End If

        'If dtStudentSubjects.Rows.Count = dtActivities.Rows.Count Then
        '    Return True
        'End If

        Return False
    End Function

End Class

Public Class UserDialyActivity
    Property DeviceId As String
    Property TokenId As String
    Property StudentId As String
    Property SubjectId As String
    Property LevelId As String
    Property QuestionAmount As Integer
    Property Questions As New DataTable
End Class