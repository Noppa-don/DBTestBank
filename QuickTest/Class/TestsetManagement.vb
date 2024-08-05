Public Class TestsetManagement

    Private db As New ClassConnectSql()
    Property UserId As String
    Property CalendarId As String
    Property TestsetId As String
    Property SchoolId As String
    Property TempTestset As Testset

    Public Sub New(userId As String, calendarId As String, schoolId As String, testset As Testset)
        Me.UserId = userId
        Me.CalendarId = calendarId
        Me.SchoolId = schoolId
        Me.TempTestset = testset
    End Sub

    Public Sub New(userId As String, calendarId As String, testsetId As String, schoolId As String)
        Me.UserId = userId
        Me.CalendarId = calendarId
        Me.TestsetId = testsetId
        Me.SchoolId = schoolId
    End Sub

    Public Function AddQsetToTestset(qsetId As String) As Boolean
        Try
            db.OpenWithTransection()
            If Not IsQsetExist(qsetId) Then
                Dim dtQuestionsInTestset As DataTable = GetQuestionsInTestset()
                Dim dtQuestions As DataTable = GetQuestionsInQset(qsetId)
                Dim tsqsId As String = NewId()
                Dim levelId As String = GetQsetLevelId(qsetId)
                Dim currentQuestionNumber As Integer = dtQuestionsInTestset.Rows.Count

                Dim sql As String = "INSERT INTO tblTestSetQuestionSet VALUES ('" & tsqsId & "','" & TestsetId & "','" & qsetId & "',1,'" & levelId & "',1,dbo.GetThaiDate(),NULL);"
                db.ExecuteWithTransection(sql)
                For Each r In dtQuestions.Rows
                    currentQuestionNumber += 1
                    sql = "INSERT INTO tblTestSetQuestionDetail VALUES (NEWID(),'" & tsqsId & "','" & r("Question_Id").ToString() & "'," & currentQuestionNumber & ",1,dbo.GetThaiDate(),NULL);"
                    db.ExecuteWithTransection(sql)
                Next
            End If
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Public Function NewId() As String
        Return db.ExecuteScalarWithTransection("SELECT NEWID();")
    End Function

    Private Function IsQsetExist(qsetId As String)
        Dim sql As String = "SELECT * FROM tblTestSetQuestionSet WHERE IsActive = 1 AND TestSet_Id = '" & TestsetId & "' AND QSet_Id = '" & qsetId & "';"
        Dim dt As DataTable = db.getdataWithTransaction(sql)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function GetQuestionsInTestset() As DataTable
        Dim sql As String = "SELECT * FROM tblTestSetQuestionDetail WHERE TSQS_Id IN (SELECT TSQS_Id FROM tblTestSetQuestionSet WHERE TestSet_Id = '" & TestsetId & "') ORDER BY TSQD_No;"
        Return db.getdataWithTransaction(sql)
    End Function

    Private Function GetQsetLevelId(qsetId As String) As String
        Dim sql As String = " SELECT b.Level_Id FROM tblquestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
        sql &= " INNER JOIN tblBook b ON b.BookGroup_Id = qc.Book_Id WHERE qs.QSet_Id = '" & qsetId & "';"
        Return db.ExecuteScalarWithTransection(sql)
    End Function

    Private Function GetQuestionsInQset(qsetId As String) As DataTable
        Dim sql As String = "SELECT * FROM tblQuestion WHERE IsActive = 1 AND QSet_Id = '" & qsetId & "' ORDER BY Question_No;"
        Return db.getdataWithTransaction(sql)
    End Function

    ''' <summary>
    ''' function ในการ เอา temptestset ลง db
    ''' </summary>
    ''' <param name="tempTestset"></param>
    ''' <returns>Boolean</returns>
    Public Function SaveTestset(tempTestset As Testset) As Boolean
        Dim sql As String
        Dim db As New ClassConnectSql()
        Dim strSQL As String = ""

        Dim needCheckMark As Boolean = False
        Try
            db.OpenWithTransection()
            If tempTestset.IsEdit Then
                sql = " update tbltestset set testset_name = N'" & tempTestset.Name & "', isactive='1', lastupdate=dbo.GetThaiDate(),ClientId = NULL,IsQuizMode = '" & tempTestset.IsQuizMode & "' , IsHomeWorkMode = '" & tempTestset.IsHomeworkMode & "' ,  "
                sql &= " IsPracticeMode = '" & tempTestset.IsPracticeMode & "' , IsReportMode = '" & tempTestset.IsReportMode & "', GroupSubject_Name = N'" & tempTestset.GetTestsetSubjectName & "' ,GroupSubject_ShortName = N'" & tempTestset.GetTestsetSubjectShortName & "' "
                sql &= " WHERE testset_id = '" & tempTestset.Id & "';"
                db.ExecuteWithTransection(sql)

                'clear old question qset
                sql = "DELETE tblTestSetQuestionDetail WHERE TSQS_Id in (SELECT TSQS_Id FROM tblTestSetQuestionSet WHERE TestSet_Id = '" & tempTestset.Id & "');"
                db.ExecuteWithTransection(sql)
                sql = "DELETE tblTestSetQuestionSet WHERE TestSet_Id = '" & tempTestset.Id & "';"
                db.ExecuteWithTransection(sql)
            Else
                sql = "INSERT INTO tblTestSet VALUES('" & tempTestset.Id & "','" & tempTestset.Name & "',1,'" & SchoolId & "','DD73B147-B098-4F1D-8144-C5FCF510AEA9','" & tempTestset.Time & "',1,dbo.GetThaiDate(),'" & tempTestset.FontSize & "','" & tempTestset.IsPracticeMode & "','" & tempTestset.IsHomeworkMode & "',"
                sql &= " '" & tempTestset.IsReportMode & "','" & tempTestset.IsQuizMode & "',0,'" & needCheckMark & "','" & UserId & "',1,'" & CalendarId & "','" & tempTestset.GetTestsetSubjectName & "','" & tempTestset.GetTestsetSubjectShortName & "',NULL,NULL);"
                db.ExecuteWithTransection(sql)
            End If

            Dim qsetNo As Integer = 1
            Dim questionNo As Integer = 1
            For Each sc In tempTestset.ListSubjectClassQuestion
                For Each q In sc.ListQset
                    If q.QuestionSelectedAmount > 0 Then ' ต้องเป็นข้อสอบที่ถูกเลือกมากว่า 1 ข้อเท่านั้น ถึงจะถูกเข้าบันทึกเป็นชุด จาก temp testset
                        'insert qset
                        Dim tsqsId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
                        sql = "INSERT INTO tblTestSetQuestionSet VALUES ('" & tsqsId & "','" & tempTestset.Id & "','" & q.QsetId & "'," & qsetNo & ",'" & sc.ClassId & "',1,dbo.GetThaiDate(),NULL);"
                        db.ExecuteWithTransection(sql)
                        'insert question
                        For Each question In q.ListQuestion
                            sql = "INSERT INTO tblTestSetQuestionDetail VALUES (NEWID(),'" & tsqsId & "','" & question.QuestionId & "'," & questionNo & ",'" & question.IsActive & "',dbo.GetThaiDate(),NULL);"
                            db.ExecuteWithTransection(sql)
                            questionNo += 1
                        Next
                        qsetNo += 1
                    End If
                Next
            Next
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Public Function CopyTestset() As Boolean
        Try
            db.OpenWithTransection()

            ' Me.TestsetId = db.ExecuteScalarWithTransection("SELECT NEWID();")
            Dim sql As String = "INSERT INTO tblTestSet "
            sql &= " SELECT '" & TempTestset.Id & "','" & TempTestset.Name & "',UserIdOld,SchoolId,Level_Id,TestSet_Time,IsActive,dbo.GetThaiDate(),TestSet_FontSize,IsPracticeMode,IsHomeWorkMode,IsReportMode,IsQuizMode,IsStandard, "
            'sql &= " NeedConnectCheckmark,'" & Me.UserId & "',UserType,'" & Me.CalendarId & "',GroupSubject_Name,GroupSubject_ShortName,ClientId,PrefixForRunningNoInWordFile FROM tblTestSet WHERE TestSet_Id = '" & Me.OtherTestsetId & "';"
            db.ExecuteWithTransection(sql)

            ' หา qset ทั้งหมดที่อยู่ใน testset ที่ทำการ copy มา
            sql = "SELECT * FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionset qs ON tsqs.QSet_Id = qs.QSet_Id "
            sql &= " INNER JOIN tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id "
            sql &= " WHERE TestSet_Id = '" & TempTestset.Id & "' ORDER BY tsqs.TSQS_No;"
            Dim dt As DataTable = db.getdataWithTransaction(sql)
            ' loop insert testsetquestionset
            For Each r In dt.Rows
                If r("IsWpp") Then
                    Dim tsqsId As String = Guid.NewGuid().ToString()
                    sql = "INSERT INTO tblTestSetQuestionSet VALUES ('" & tsqsId & "','" & TempTestset.Id & "','" & r("Qset_Id").ToString() & "','" & r("TSQS_No") & "','" & r("Level_Id").ToString() & "',1,dbo.GetThaiDate(),NULL);"
                    db.ExecuteWithTransection(sql)

                    sql = "SELECT * FROM tblTestSetQuestionDetail WHERE TSQS_Id = '" & r("TSQS_Id").ToString() & "' AND IsActive = 1 ORDER BY TSQD_No;"
                    Dim dtTestsetQuestionDetail As DataTable = db.getdataWithTransaction(sql)
                    For Each question In dtTestsetQuestionDetail.Rows
                        sql = "INSERT INTO tblTestSetQuestionDetail VALUES (NEWID(),'" & tsqsId & "','" & question("Question_Id").ToString() & "','" & question("TSQD_No") & "',1,dbo.GetThaiDate(),NULL);"
                        db.ExecuteWithTransection(sql)
                    Next
                Else
                    ' ทำการ เพิ่ม questioncategory ที่ copy มา
                    Dim bookId As String = r("Book_Id").ToString()
                    Dim qCategoryName As String = r("QCategory_Name").ToString()
                    Dim qCategoryId As String
                    sql = "SELECT * FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id =  '" & UserId & "' AND Book_Id =  '" & bookId & "' AND QCategory_Name =  '" & qCategoryName & "';"
                    Dim dtQuestionCategory As DataTable = db.getdataWithTransaction(sql)
                    If dtQuestionCategory.Rows.Count = 0 Then
                        qCategoryId = Guid.NewGuid().ToString()
                        sql = "SELECT COUNT(*) FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id =  '" & UserId & "' AND Book_Id =  '" & bookId & "';"
                        Dim qcatNo As Integer = CInt(db.ExecuteScalarWithTransection(sql)) + 1
                        sql = "INSERT INTO tblQuestionCategory VALUES ( '" & qCategoryId & "',NULL, '" & Me.UserId & "', '" & bookId & "',1," & qcatNo & ", '" & qCategoryName & "',NULL,NULL,1,NULL,NULL,0,dbo.GetThaiDate(),NULL);"
                        db.ExecuteScalarWithTransection(sql)
                    Else
                        qCategoryId = dtQuestionCategory.Rows(0)("QCategory_Id").ToString()
                    End If

                    ' หา detail ของ qset เพื่อทำการ copy มาเป็นของตัวเอง
                    Dim qsetId As String = r("Qset_Id").ToString()
                    sql = "SELECT * FROM tblQuestionset WHERE QSet_Id = '" & qsetId & "';"
                    Dim dtQuestionset As DataTable = db.getdataWithTransaction(sql)

                    ' เพิ่ม qset ลง db เพราะว่าเป็น qset ที่สร้างขึ้นเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                    Dim newQsetId As String = Guid.NewGuid().ToString()
                    Dim qsetName As String = dtQuestionset.Rows(0)("Qset_Name").ToString()
                    Dim qsetType As Integer = dtQuestionset.Rows(0)("QSet_Type")
                    Dim qsetNo As Integer = CInt(db.ExecuteScalarWithTransection("SELECT COUNT(*) FROM tblQuestionset WHERE QCategory_Id = '" & qCategoryId & "';")) + 1
                    sql = "INSERT INTO tblQuestionset VALUES ( '" & newQsetId & "', '" & qCategoryId & "',NULL," & qsetNo & ", '" & qsetName & "'," & qsetType & ",0,1,1,1,1,NULL,0,0,dbo.GetThaiDate(),NULL, '" & qsetName & "');"
                    db.ExecuteScalarWithTransection(sql)

                    ' ทำการ เพิ่ม qset ใหม่ เข้า testset
                    Dim newTsqsId As String = Guid.NewGuid().ToString()
                    sql = "INSERT INTO tblTestSetQuestionSet VALUES ('" & newTsqsId & "','" & TempTestset.Id & "','" & newQsetId & "','" & r("TSQS_No") & "','" & r("Level_Id").ToString() & "',1,dbo.GetThaiDate(),NULL);"
                    db.ExecuteWithTransection(sql)

                    'หาข้อสอบที่อยู่ในชุดนั้น ใน testsetQuestionDetail ที่ copy มา
                    sql = "SELECT * FROM tblTestSetQuestionDetail WHERE TSQS_Id = '" & r("TSQS_Id").ToString() & "' AND IsActive = 1 ORDER BY TSQD_No;"
                    Dim dtTestsetQuestionDetail As DataTable = db.getdataWithTransaction(sql)

                    'หาข้อสอบ (เพิ่มเอง) ที่อยู่ใน db ของ Qset ที่ copy มา
                    sql = "SELECT * FROM tblQuestion WHERE QSet_Id = '" & qsetId & "' AND IsActive = 1 ORDER BY Question_No;"
                    Dim dtQuestion As DataTable = db.getdataWithTransaction(sql)
                    For Each q In dtQuestion.Rows
                        ' questionId เดิม ของคำถามที่ทำการ copy มา
                        Dim questionId As String = q("Question_Id").ToString()
                        ' เพิ่มคำถามลง db เพราะว่าเป็นคำถามที่เพิ่มขึ้นมาเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                        Dim newQuestionId As String = Guid.NewGuid().ToString()
                        sql = "INSERT INTO tblQuestion VALUES ('" & newQuestionId & "','" & newQsetId & "',NULL,NULL,'" & q("Question_No") & "','" & q("Question_Name") & "' "
                        sql &= " ,'" & q("Question_Expain") & "',1,1,'" & q("Question_Name") & "',dbo.GetThaiDate(),0,NULL,0,'" & q("Question_Name") & "','" & q("Question_Expain") & "');"
                        db.ExecuteWithTransection(sql)

                        'หาว่า คำถามข้อนี้ มีอยู่ใน testset ที่ copy มาหรือเปล่า ถ้ามีต้อง เพิ่มใน testset ให้ด้วย
                        Dim questionTestsetDetail = (From t In dtTestsetQuestionDetail.AsEnumerable Where t.Field(Of Guid)("Question_Id").ToString() = questionId Select t).SingleOrDefault
                        If questionTestsetDetail IsNot Nothing Then
                            sql = "INSERT INTO tblTestSetQuestionDetail VALUES (NEWID(),'" & newTsqsId & "','" & newQuestionId & "','" & questionTestsetDetail("TSQD_No") & "',1,dbo.GetThaiDate(),NULL);"
                            db.ExecuteWithTransection(sql)
                        End If

                        ' เพิ่มคำตอบของคำถามนี้ เพิ่อลง db เพราะว่าเป็นคำตอบของคำถามที่เพิ่มขึ้นมาเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                        sql = "SELECT * FROM tblAnswer WHERE Question_Id = '" & questionId & "';"
                        Dim dtAnswers As DataTable = db.getdataWithTransaction(sql)
                        For Each answer In dtAnswers.Rows
                            sql = "INSERT INTO tblAnswer VALUES (NEWID(),'" & newQuestionId & "',NULL,'" & newQsetId & "','" & answer("Answer_No") & "','" & answer("Answer_Name") & "' "
                            sql &= " ,'" & answer("Answer_Expain") & "','" & answer("Answer_Score") & "','" & answer("Answer_ScoreMinus") & "','" & answer("Answer_Position") & "',1 "
                            sql &= " ,'" & answer("Answer_Name") & "',dbo.GetThaiDate(),NULL,0,NULL,'" & answer("Answer_Name") & "','" & answer("Answer_Expain") & "');"
                            db.ExecuteWithTransection(sql)
                        Next
                    Next

                End If
            Next

            db.CommitTransection()

        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Function

    ''' <summary>
    ''' copy qset ของครูคนอื่นที่ได้สร้างไว้ มาสร้างเป็น testset ของตัวเอง 
    ''' </summary>
    Public Function CopyOtherQsetToMyTestset(qsetId As String) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = ""

            ' หา qset ทั้งหมดที่อยู่ใน testset ที่ทำการ copy มา
            sql = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
            sql &= " INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id  WHERE qs.QSet_Id = '" & qsetId & "'; "
            Dim dt As DataTable = db.getdataWithTransaction(sql)
            ' loop insert testsetquestionset
            For Each r In dt.Rows
                ' ทำการ เพิ่ม questioncategory ที่ copy มา
                Dim bookId As String = r("Book_Id").ToString()
                Dim qCategoryName As String = r("QCategory_Name").ToString()
                Dim qCategoryId As String
                sql = "Select * FROM tblQuestionCategory WHERE IsWpp = 0 And Parent_Id =  '" & UserId & "' AND Book_Id =  '" & bookId & "' AND QCategory_Name =  '" & qCategoryName & "';"
                Dim dtQuestionCategory As DataTable = db.getdataWithTransaction(sql)
                If dtQuestionCategory.Rows.Count = 0 Then
                    qCategoryId = Guid.NewGuid().ToString()
                    sql = "SELECT COUNT(*) FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id =  '" & UserId & "' AND Book_Id =  '" & bookId & "';"
                    Dim qcatNo As Integer = CInt(db.ExecuteScalarWithTransection(sql)) + 1
                    sql = "INSERT INTO tblQuestionCategory VALUES ( '" & qCategoryId & "',NULL, '" & Me.UserId & "', '" & bookId & "',1," & qcatNo & ", '" & qCategoryName & "',NULL,NULL,1,NULL,NULL,0,dbo.GetThaiDate(),NULL);"
                    db.ExecuteScalarWithTransection(sql)
                Else
                    qCategoryId = dtQuestionCategory.Rows(0)("QCategory_Id").ToString()
                End If

                ' เพิ่ม qset ลง db เพราะว่าเป็น qset ที่สร้างขึ้นเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                Dim newQsetId As String = Guid.NewGuid().ToString()
                Dim qsetName As String = r("Qset_Name").ToString()
                Dim qsetType As Integer = r("QSet_Type")
                Dim qsetNo As Integer = CInt(db.ExecuteScalarWithTransection("SELECT COUNT(*) FROM tblQuestionset WHERE QCategory_Id = '" & qCategoryId & "';")) + 1
                sql = "INSERT INTO tblQuestionset VALUES ( '" & newQsetId & "', '" & qCategoryId & "',NULL," & qsetNo & ", '" & qsetName & "'," & qsetType & ",0,1,1,1,1,NULL,0,0,dbo.GetThaiDate(),NULL, '" & qsetName & "');"
                db.ExecuteScalarWithTransection(sql)

                'สร้าง testset ขึ้นมาใหม่ ใน db
                sql = " INSERT INTO tblTestSet VALUES ('" & TempTestset.Id & "','" & TempTestset.Name & "',1,'" & Me.SchoolId & "','" & r("Level_Id").ToString() & "',60,1,dbo.GetThaiDate(),0"
                sql &= " ,'" & TempTestset.IsPracticeMode & "','" & TempTestset.IsHomeworkMode & "','" & TempTestset.IsReportMode & "','" & TempTestset.IsQuizMode & "',0,0,'" & Me.UserId & "',1"
                sql &= " ,'" & Me.CalendarId & "','" & r("GroupSubject_Id").ToString().ToSubjShortThName() & "','" & r("GroupSubject_Id").ToString().ToSubjectShortThName() & "',NULL,NULL);"
                db.ExecuteWithTransection(sql)

                ' ทำการ เพิ่ม qset ใหม่ เข้า testset
                Dim tsqsNo As Integer = 1 ' เป็น 1 ได้เลยเพราะมีแค่ qset เดียว ที่ทำการ copy มา
                Dim newTsqsId As String = Guid.NewGuid().ToString()
                sql = "INSERT INTO tblTestSetQuestionSet VALUES ('" & newTsqsId & "','" & TempTestset.Id & "','" & newQsetId & "','" & tsqsNo & "','" & r("Level_Id").ToString() & "',1,dbo.GetThaiDate(),NULL);"
                db.ExecuteWithTransection(sql)


                'หาข้อสอบ (เพิ่มเอง) ที่อยู่ใน db ของ Qset ที่ copy มา
                sql = "SELECT * FROM tblQuestion WHERE QSet_Id = '" & qsetId & "' AND IsActive = 1 ORDER BY Question_No;"
                Dim dtQuestion As DataTable = db.getdataWithTransaction(sql)
                For Each q In dtQuestion.Rows
                    ' questionId เดิม ของคำถามที่ทำการ copy มา
                    Dim questionId As String = q("Question_Id").ToString()
                    ' เพิ่มคำถามลง db เพราะว่าเป็นคำถามที่เพิ่มขึ้นมาเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                    Dim newQuestionId As String = Guid.NewGuid().ToString()
                    'sql = "INSERT INTO tblQuestion VALUES ('" & newQuestionId & "','" & newQsetId & "',NULL,NULL,'" & q("Question_No") & "','" & q("Question_Name") & "' "
                    'sql &= " ,'" & q("Question_Expain") & "',1,0,'" & q("Question_Name") & "',dbo.GetThaiDate(),0,NULL,0,'" & q("Question_Name") & "','" & q("Question_Expain") & "');"
                    sql = "INSERT INTO tblQuestion(Question_Id,QSet_Id,Question_No,Question_Name,Question_Expain,IsActive,Question_Name_Backup,LastUpdate,IsWpp,ClientId,NoChoiceShuffleAllowed, "
                    sql &= " Question_Name_Quiz,Question_Expain_Quiz) VALUES('" & newQuestionId & "','" & newQsetId & "'," & q("Question_No") & ",'" & q("Question_Name") & "',"
                    sql &= " '" & q("Question_Expain") & "',1,'" & q("Question_Name") & "',dbo.GetThaiDate(),0,NULL,0,'" & q("Question_Name") & "','" & q("Question_Expain") & "');"
                    db.ExecuteWithTransection(sql)
                    'หาว่า คำถามข้อนี้ มีอยู่ใน testset ที่ copy มาหรือเปล่า ถ้ามีต้อง เพิ่มใน testset ให้ด้วย
                    sql = "INSERT INTO tblTestSetQuestionDetail VALUES (NEWID(),'" & newTsqsId & "','" & newQuestionId & "','" & q("Question_No") & "',1,dbo.GetThaiDate(),NULL);"
                    db.ExecuteWithTransection(sql)

                    ' เพิ่มคำตอบของคำถามนี้ เพิ่อลง db เพราะว่าเป็นคำตอบของคำถามที่เพิ่มขึ้นมาเองและมาจากการ copy เลยต้องสร้างเป็นของตัวเอง
                    sql = "SELECT * FROM tblAnswer WHERE Question_Id = '" & questionId & "';"
                    Dim dtAnswers As DataTable = db.getdataWithTransaction(sql)
                    For Each answer In dtAnswers.Rows
                        sql = "INSERT INTO tblAnswer VALUES (NEWID(),'" & newQuestionId & "',NULL,'" & newQsetId & "','" & answer("Answer_No") & "','" & answer("Answer_Name") & "' "
                        sql &= " ,'" & answer("Answer_Expain") & "','" & answer("Answer_Score") & "','" & answer("Answer_ScoreMinus") & "','" & answer("Answer_Position") & "',1 "
                        sql &= " ,'" & answer("Answer_Name") & "',dbo.GetThaiDate(),NULL,0,NULL,'" & answer("Answer_Name") & "','" & answer("Answer_Expain") & "');"
                        db.ExecuteWithTransection(sql)
                    Next
                Next
            Next
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Function CreateQsetByUser()

    End Function

    Private Function DeleteOldTestset()
        Return 0
    End Function
End Class
