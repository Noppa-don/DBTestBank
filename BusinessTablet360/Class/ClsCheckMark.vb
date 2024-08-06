Imports System.Data.SqlClient
Imports System.Web
Imports System.Text
Imports BusinessTablet360.Service
Imports KnowledgeUtils

Public Class ClsCheckMark
    Private Str As String
    Private ClsKNConfig As New KNConfigData()
    'Private db

    'Private dtClassName As DataTable
    Private ClassIdInCheckMark As New Dictionary(Of Integer, String)

    Public Sub New()
        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            If System.Configuration.ConfigurationManager.ConnectionStrings("CheckMarkConnectionString") IsNot Nothing Then
                If System.Configuration.ConfigurationManager.ConnectionStrings("CheckMarkConnectionString").ConnectionString IsNot Nothing Then
                    Dim StrDecryption As String = System.Configuration.ConfigurationManager.ConnectionStrings("CheckMarkConnectionString").ConnectionString
                    Str = KNConfigData.DecryptData(StrDecryption)
                    'Str = "Data Source=10.100.1.116\sqlserver2008r2,1600;Initial Catalog=CheckAnswer2;Persist Security Info=True;User ID=sa;Password=kl123"
                    'Str = "Data Source=10.100.1.160;Initial Catalog= CheckAnswer2;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"
                    SetData()
                Else
                    Throw New ArgumentException("ไม่ได้กำหนด Checkmark ConnectionString")
                End If
            Else
                Throw New ArgumentException("ไม่ได้กำหนด Checkmark ConnectionString")
            End If
        End If
    End Sub

    Private Sub SetData() ' get ค่า classid ของ checkmark
        'dtClassName = db.getdata("SELECT * FROM tblClass ORDER BY Class_Name;")
        Dim className As New List(Of String)(New String() {"อ.1", "อ.2", "อ.3", "ป.1", "ป.2", "ป.3", "ป.4", "ป.5", "ป.6", "ม.1", "ม.2", "ม.3", "ม.4", "ม.5", "ม.6"})
        For i As Integer = 0 To className.Count() - 1
            ClassIdInCheckMark.Add(i + 1, className.Item(i))
        Next
        'db = New ClassConnectSql(, Str)
    End Sub

    Public Function GetClassId(ByVal ClassName As String) As Integer
        'Return (From c In dtClassName Where c("Class_Name").ToString().Contains(ClassName) Select c.Field(Of Integer)("Class_ID")).SingleOrDefault()
        Return (From c In ClassIdInCheckMark Where c.Value.Contains(ClassName) Select c.Key).SingleOrDefault()
    End Function

    ' เพิ่มนักเรียนคนเดียว
    Public Function AddStudent(ByVal Student As StudentCheckMark) As Boolean
        Dim sql As String = Student.ToStringSQLInsertCheckmark()
        Return ExecuteSQLWithTransection(sql)
    End Function

    ' เพิ่มนักเรียนแบบเป็นหลายคน
    Public Function AddStudents(ByVal Students As List(Of StudentCheckMark)) As Boolean
        Dim sql As New StringBuilder()
        For Each eachStudent In Students
            sql.Append(eachStudent.ToStringSQLInsertCheckmark())
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function

    Public Function RemoveStudent(ByVal Student As StudentCheckMark) As Boolean
        Dim sql As String = Student.ToStringSQLDeleteCheckmark()
        Return ExecuteSQLWithTransection(sql)
    End Function

    Public Function RemoveStudents(ByVal Students As List(Of StudentCheckMark)) As Boolean
        Dim sql As New StringBuilder()
        For Each eachStudent In Students
            sql.Append(eachStudent.ToStringSQLDeleteCheckmark())
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function

    Public Function RemoveStudents(ByVal ClassName As String, ByVal RoomName As String)
        Dim classId As Integer = GetClassId(ClassName)
        Dim sql As String = String.Format("DELETE tblStudent WHERE Class_ID = {0} AND StudentRoom = N'{1}';", classId, RoomName.CleanSQL)
        Return ExecuteSQLWithTransection(sql)
    End Function

    Public Function EditStudent(ByVal Student As StudentCheckMark, ByVal StudentOld As StudentCheckMark) As Boolean
        Dim sql As String = Student.ToStringSQLUpdateCheckmark(StudentOld)
        Return ExecuteSQLWithTransection(sql)
    End Function

    Public Function EditStudents(ByVal Students As List(Of StudentCheckMark)) As Boolean
        Dim sql As New StringBuilder()
        For Each eachStudent In Students
            sql.Append(eachStudent.ToStringSQLDeleteCheckmark())
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function

    Public Function AddClass(ByVal ClassToAdd As t360_tblSchoolClass()) As Boolean
        Dim db As New ClassConnectSql(, Str)
        Dim sql As New StringBuilder()
        Dim dtClassName As DataTable = GetClassInCheckmark(db)
        For Each c In ClassToAdd
            Dim q = (From cl In dtClassName Where cl("Class_Name").ToString().Contains(c.Class_Name) Select cl.Field(Of Integer)("Class_ID")).SingleOrDefault()
            If q = 0 Then
                Dim classId As Integer = GetClassId(c.Class_Name)
                sql.Append(String.Format(" INSERT INTO tblClass VALUES('{0}',N'{1}');", classId, c.Class_Name.CleanSQL))
            End If
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function

    Public Function RemoveClass(ByVal ClassToRemove As t360_tblSchoolClass()) As Boolean
        Dim sql As New StringBuilder()
        For Each c In ClassToRemove
            Dim classId As Integer = GetClassId(c.Class_Name)
            sql.Append(String.Format(" DELETE tblClass WHERE Class_ID = '{0}' AND Class_Name = N'{1}';", classId, c.Class_Name))
            sql.Append(String.Format(" DELETE tblstudent WHERE Class_ID = '{0}';", classId))
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function

    Private Function GetClassInCheckmark(ByRef db As ClassConnectSql) As DataTable
        Return db.getdata("SELECT * FROM tblClass ORDER BY Class_Name;")
    End Function

    Public Function RemoveRoom() As Boolean
        'ลบห้องไม่ต้องทำอะไร เพราะ t360 ก่อนจะลบห้อง จะต้องไม่มีนักเรียนในห้องก่อน
        Return False
    End Function

    'แก้ไขชื่อห้องจาก T360 ต้องมาอัพเดทที่ tblStudent เพราะเลขห้องมีที่นี้ที่เดียว
    Public Function EditRoom(ByVal Room As t360_tblRoom, ByVal OldRoom As t360_tblRoom) As Boolean
        Dim classId As String = GetClassId(OldRoom.Class_Name)
        Dim sql As String = String.Format("UPDATE tblStudent SET StudentRoom = N'{0}' WHERE Class_ID = '{1}' AND StudentRoom = N'{2}';", Room.Room_Name.Replace("/", "").CleanSQL, classId, OldRoom.Room_Name.Replace("/", "").CleanSQL)
        Return ExecuteSQLWithTransection(sql)
    End Function

    'edit room จาก setting
    Public Function EditRoom(ByVal ClassName As String, ByVal RoomName As String, ByVal OldRoomName As String) As Boolean
        Dim classId As String = GetClassId(ClassName)
        Dim sql As String = String.Format("UPDATE tblStudent SET StudentRoom = N'{0}' WHERE Class_ID = '{1}' AND StudentRoom = N'{2}';", RoomName.CleanSQL, classId, OldRoomName.CleanSQL)
        Return ExecuteSQLWithTransection(sql)
    End Function

    ' function สำหรับ run เลขตัวแรกของรหัสนักเรียน ป = 1, ม = 2
    Public Function GetClassRunNumber(ByRef ClassName As String) As String
        If ClassName.Contains("ป.") Then
            Return "1"
        End If
        Return "2"
    End Function


    Public Function ExecuteSQLWithTransection(ByRef Sql As String) As Boolean
        If Sql = "" Then
            Return True
        End If
        Dim db As New ClassConnectSql(, Str)
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(Sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Public Function NewStudentCheckmark(ByRef ClsCheckmark As ClsCheckMark, ByRef Item As t360_tblStudent) As StudentCheckMark
        Dim classId As Integer = ClsCheckmark.GetClassId(Item.Student_CurrentClass)
        Dim s As New StudentCheckMark()
        s.StudentId = Item.Student_Code
        s.StudentFName = Item.Student_FirstName
        s.StudentLName = Item.Student_LastName
        s.ClassId = classId
        s.ClassName = Item.Student_CurrentClass
        s.StudentNumber = Item.Student_CurrentNoInRoom
        s.StudentRoom = Item.Student_CurrentRoom.Replace("/", "") ' ฝั่ง checkmark เลขห้องไม่มี / และเป็นตัวเลขเท่านั้น
        s.StudentPrefixName = Item.Student_PrefixName
        s.SchoolId = Item.School_Code
        Return s
    End Function


    Public Sub saveQuizToCheckmark(ByVal SetupAnswer_Name As String, ByVal TemplateName As String, ByVal Amountchoice As String, ByVal Class_Name As String, ByVal testSetId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        ' Dim db As New ClassConnectSql(, Str)
        Dim sql As New StringBuilder
        Dim Semester_Name As String = GetAcademicYear()
        Dim Term_name As String = GetAcademicTerm_name()
        Dim subject = setGroupSubjectToCheckmark(testSetId)
        Dim Subject_ID As String = subject.SubjectId
        Dim Subject_Name As String = subject.SubjectName
        Dim Gsubjec_name As String = subject.GroupSubject
        Dim TypeExam_Name As String = "ข้อสอบจากโปรแกรมควิซ(PointPlus)"
        sql.Append(" INSERT INTO temptblsetup (SetupAnswer_Name,TemplateName,Subject_ID,Amountchoice,Semester_Name,SetupAnswer_Date,Term_name,Active,Subject_Name,Gsubjec_name,TypeExam_Name,Class_Name,Subject_Audit) ")
        sql.Append(" VALUES (N'" & SetupAnswer_Name.CleanSQL & "',N'" & TemplateName.CleanSQL & "','" & Subject_ID & "', ")
        sql.Append(" '" & Amountchoice.CleanSQL & "',N'" & Semester_Name.CleanSQL & "', cast(floor(cast(dbo.GetThaiDate() as float)) as smalldatetime),N'" & Term_name.CleanSQL & "','0', ")
        sql.Append(" N'" & Subject_Name.CleanSQL & "',N'" & Gsubjec_name.CleanSQL & "',N'" & TypeExam_Name.CleanSQL & "',N'" & Class_Name.CleanSQL & "','1'); SELECT scope_identity();")
        Dim db As New ClassConnectSql(, Str)
        db.OpenWithTransection(InputConn)
        Dim RefToCheckMark = db.ExecuteScalarWithTransection(sql.ToString(), InputConn)
        If (RefToCheckMark = "" Or RefToCheckMark Is Nothing) Then
            db.RollbackTransection(InputConn)
        Else
            db.CommitTransection(InputConn)
        End If

        HttpContext.Current.Session("RefToCheckMark") = RefToCheckMark
    End Sub
    ' ตั้งชื่อของ กลุ่มสาระการเรียนรู้ ของ checkmark
    Private Function setGroupSubjectToCheckmark(ByVal testSetId As String)
        Dim db As New ClassConnectSql()
        Dim sql = " SELECT  DISTINCT tblGroupSubject.GroupSubject_Name "
        sql &= " FROM tblTestSet INNER JOIN tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN "
        sql &= " tblQuestionSet INNER JOIN tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN "
        sql &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id"
        sql &= " WHERE tblTestSet.TestSet_Id = '" & testSetId.CleanSQL & "' And tblTestsetQuestionSet.IsActive = '1' ; "

        Dim Gsubject = db.getdata(sql)

        Dim GsubjectArr As Array = {"กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี", "กลุ่มสาระการเรียนรู้คณิตศาสตร์", "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ", "กลุ่มสาระการเรียนรู้ภาษาไทย", "กลุ่มสาระการเรียนรู้วิทยาศาสตร์", "กลุ่่มสาระการเรียนรู้ศิลปะ", "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม", "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"}
        Dim subjectArr As Array = {"การงานฯ", "คณิตฯ", "ภาษาอังกฤษ", "ไทย", "วิทยาศาสตร์", "ศิลปะ", "สังคมฯ", "สุขศึกษาฯ"}
        'Dim subject As String = "" 'กลุ่มสาระ
        'Dim subjectId As String = "" 'รหัสวิชา

        Dim subject As subjectName
        Dim fromProgram As String = ""
        If (HttpContext.Current.Application("NeedQuizMode") = True) Then
            fromProgram = "(จากโปรแกรม PointPlus)"
        Else
            fromProgram = "(จากโปรแกรม Quicktest)"
        End If
        ' check ว่า testset นี้มีกลุ่มสาระมากกว่า 1 หรือเปล่า
        If (Gsubject.Rows.Count > 1) Then
            ' ถ้ามากกว่า 1
            subject.GroupSubject = "รวมหลายวิชา" & fromProgram
            subject.SubjectName = "รวมหลายวิชา"
            Dim j As Integer
            Dim subjectId As String = ""
            For j = 0 To Gsubject.Rows.Count - 1
                For i As Integer = 0 To GsubjectArr.Length - 1
                    If (Gsubject(j)("GroupSubject_Name").ToString() = GsubjectArr(i).ToString()) Then
                        subjectId &= subjectArr(i).ToString() & "|"
                        Exit For
                    End If
                Next
            Next
            subject.SubjectId = subjectId.Remove(subjectId.Length - 1).ToString()
        ElseIf (Gsubject.Rows.Count = 1) Then
            ' ถ้ามีกลุ่มเดียว
            subject.GroupSubject = GetSubjectNameCheckPoint(Gsubject(0)("GroupSubject_Name").ToString()) & fromProgram
            subject.SubjectName = GetSubjectNameCheckPoint(Gsubject(0)("GroupSubject_Name").ToString())
            Dim i As Integer
            For i = 0 To GsubjectArr.Length - 1
                If (Gsubject(0)("GroupSubject_Name").ToString() = GsubjectArr(i).ToString()) Then
                    subject.SubjectId = subjectArr(i).ToString()
                    Exit For
                End If
            Next
        End If

        Return subject
    End Function

    ' ปรับชืือวิชาให้ตรงตามที่พีิชินอยากได้
    Public Function GetSubjectNameCheckPoint(GroupSubjectName As String) As String
        Select Case GroupSubjectName
            Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                Return "คณิตศาสตร์"
            Case "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี"
                Return "การงาน"
            Case "กลุ่มสาระการเรียนรู้ภาษาไทย"
                Return "ภาษาไทย"
            Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
                Return "สุขศึกษา"
            Case "กลุ่มสาระการเรียนรู้วิทยาศาสตร์"
                Return "วิทยาศาสตร์"
            Case "กลุ่่มสาระการเรียนรู้ศิลปะ"
                Return "ศิลปะ"
            Case "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม"
                Return "สังคม"
            Case "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                Return "ภาษาอังกฤษ"
            Case Else
                Return ""
        End Select
        Return ""
    End Function


    ' save แบบข้อต่อข้อใน point plus
    Public Sub saveCorrectAnswerToCheckmark(ByVal questionNo As String, ByVal correctAnswer As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim db As New ClassConnectSql(, Str)
        Dim refChkMark As String = HttpContext.Current.Session("RefToCheckMark").ToString()
        Dim dt As DataTable = db.getdata(" SELECT * FROM temptblChoice WHERE SetupAnswer_Id = '" & refChkMark.CleanSQL & "' AND Choice_Name = '" & questionNo.CleanSQL & "';")
        If dt.Rows.Count = 0 Then
            Dim sql As String = ""
            sql &= " INSERT INTO temptblChoice (SetupAnswer_Id,Choice_Name,Choice_Answer) "
            sql &= " VALUES ('" & refChkMark & "','" & questionNo & "','" & correctAnswer & "'); "
            sql &= " UPDATE temptblsetup SET Amountchoice = (SELECT (MAX(Amountchoice)+1) FROM temptblsetup WHERE SetupAnswer_ID = '" & refChkMark.CleanSQL & "') " ',Lastupdate = dbo.GetThaiDate() "
            sql &= " WHERE SetupAnswer_ID = '" & refChkMark & "';"

            db.OpenWithTransection(InputConn)
            db.ExecuteWithTransection(sql, InputConn)
            db.CommitTransection(InputConn)
        End If
    End Sub
    ' save แบบ quicktest
    Public Sub saveAllCorrectAnswerToCheckmark()

        Dim correctAnswer As DataTable = getQuestionNoAndCorrectAnswer()

        Dim db As New ClassConnectSql(, Str)
        Dim refChkMark As String = HttpContext.Current.Session("RefToCheckMark").ToString()
        Dim sql As String = ""

        For i As Integer = 0 To correctAnswer.Rows.Count - 1
            sql &= " INSERT INTO temptblChoice (SetupAnswer_Id,Choice_Name,Choice_Answer) "
            sql &= " VALUES ('" & refChkMark & "','" & correctAnswer(i)("TSQD_No") & "','" & correctAnswer(i)("Answer_No") & "'); "
        Next

        db.OpenWithTransection()
        db.ExecuteWithTransection(sql)
        db.CommitTransection()
    End Sub
    ' get เลขข้อและคำตอบ ใน testset
    Private Function getQuestionNoAndCorrectAnswer() As DataTable
        Dim db As New ClassConnectSql()
        Dim sql As String = ""
        sql &= " SELECT tblTestSetQuestionDetail.TSQD_No, tblAnswer.Question_Id AS Question_Id, tblAnswer.Answer_No,tblAnswer.Answer_Score "
        sql &= " FROM tblTestSetQuestionSet INNER JOIN "
        sql &= " tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id INNER JOIN "
        sql &= " tblAnswer ON tblTestSetQuestionDetail.Question_Id = tblAnswer.Question_Id "
        sql &= " WHERE TestSet_Id = '" & HttpContext.Current.Session("newTestSetId").ToString.CleanSQL & "' AND Answer_Score = '1'  "
        sql &= " And tblTestsetQuestionSet.IsActive = '1' and tblTestsetQuestionDetail.isActive = '1' ORDER BY TSQD_No"
        getQuestionNoAndCorrectAnswer = db.getdata(sql)
        Return getQuestionNoAndCorrectAnswer
    End Function
    ' insert testset กับ setupanswer_id ใน tblTestSet_CM_temptblsetup
    Public Sub InsertRefToCheckMarkIntblCM(ByVal testSetId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim db As New ClassConnectSql()
        Dim sql As String
        'sql = " UPDATE tblQuiz SET RefToCheckMark = '" & HttpContext.Current.Session("RefToCheckMark") & "' WHERE Quiz_Id = '" & quizId & "' AND t360_SchoolCode = '" & schoolCode & "' ; "
        sql = " INSERT tblTestSet_CM_temptblsetup (TCT_Id,TestSet_Id,SetupAnswer_ID) VALUES (NEWID(),'" & testSetId.CleanSQL & "','" & HttpContext.Current.Session("RefToCheckMark").CleanSQL & "') "

        db.OpenWithTransection(InputConn)
        db.ExecuteWithTransection(sql, InputConn)
        db.CommitTransection(InputConn)
    End Sub
    ' update active ใน temptblsetup เมื่อลบ testset
    Public Sub updateActiveCheckMarkWhenUserDelTestset(ByVal refToChekmark As DataTable)

        ' ดักไว้ บางที บาง testset อาจไม่เคยสอบโดย checkmark เลยก็ได้ จะได้ไม่ต้องหนักเรื่อง execute
        If (refToChekmark.Rows.Count > 0) Then
            Dim db As New ClassConnectSql(, Str)
            Dim sql As String = " UPDATE temptblsetup SET Active = '2' WHERE  "
            Dim lastRow = refToChekmark.Rows.Count - 1
            For i As Integer = 0 To lastRow
                If (i <> lastRow) Then
                    sql &= " SetupAnswer_ID = '" & refToChekmark(i)("SetupAnswer_ID").ToString().CleanSQL & "' OR "
                Else
                    sql &= " SetupAnswer_ID = '" & refToChekmark(i)("SetupAnswer_ID").ToString().CleanSQL & "' ;"
                End If
            Next
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
        End If

    End Sub
    ' update needconnectcheckmark
    Public Sub updateConnectCheckmark(ByVal check As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim db As New ClassConnectSql()
        ' เอา update lastupdate ออก เนื่องจากไม่ใช่เป็นการแก้ไข testset แค่นำชุดไปใช้เล่น quiz เฉยๆ
        Dim sql As String = " UPDATE tblTestSet SET needConnectCheckmark = '" & check.CleanSQL & "',ClientId = NULL where TestSet_Id = '" & HttpContext.Current.Session("newTestSetId").ToString.CleanSQL & "';"
        db.Execute(sql, InputConn)
    End Sub


    Private Function GetAcademicYear() As String
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

    Private Function GetAcademicTerm_name() As String
        Dim CurrentYear As Integer = Year(Now)
        Dim CurrentDate As New Date(Year(Now), Month(Now), Day(Now))
        Dim AcademicYear = GetAcademicYear()

        Dim term_1_start As New Date(AcademicYear, 4, 20) ' เทอม 1 เปิด 1 พฤษภาคม 55 - เปิดก่อน 10 วัน
        Dim term_1_stop As New Date(AcademicYear, 10, 10) ' เทอม 1 ปิดประมาน 1 ตุลาคม 55 - ปิดทีหลัง 10 วัน
        Dim term_2_start As New Date(AcademicYear, 11, 1) ' เทอม 2 เปิด 21 ตุลาคม 55 - เปิดก่อน 10 วัน
        Dim term_2_stop As New Date((CInt(AcademicYear) + 1).ToString(), 3, 10) ' เทอม 2 ปิด 1 มีนาคม 56 ปิดทีหลัง 10 วัน
        'Dim summerOpen As New Date(AcademicYear, 3, 20) ' เทอม 3 เปิด 1 เมษายน 56 เปิดก่อน 10 วัน
        'Dim summerClosed As New Date(AcademicYear, 4, 20) ' เทอม 3 ปิด 30 เมษายน 56 เปิดก่อน 10 วัน

        Dim currentTerm As String = ""

        If (DateValue(CurrentDate) >= DateValue(term_1_start) AndAlso DateValue(CurrentDate) <= DateValue(term_1_stop)) Then
            currentTerm = "1"
        ElseIf (DateValue(CurrentDate) >= DateValue(term_2_start) AndAlso DateValue(CurrentDate) <= DateValue(term_2_stop)) Then
            currentTerm = "2"
        End If

        GetAcademicTerm_name = currentTerm
        Return GetAcademicTerm_name
    End Function

    Private Structure subjectName
        Public GroupSubject As String
        Public SubjectName As String
        Public SubjectId As String
    End Structure

    'Private Sub test()
    '    String.Format("INSERT INTO tblStudent VALUES('{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7},{8},{9},{10});",
    '                                   Student.StudentId, Student.StudentFName, Student.StudentLName, Student.StudentSex, Student.ClassId, Student.StudentBirth,
    '                                   Student.StudentPassword, Student.TermId, Student.TitleId, Student.StudentNumber, Student.StudentRoom)
    'End Sub

End Class
