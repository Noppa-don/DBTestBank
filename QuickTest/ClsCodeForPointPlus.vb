Public Class ClsCodeForPointPlus


    Dim _DB As ClsConnect
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub


    Public Function SetQuizScore(ByVal Examnum As Integer, ByVal Quiz_Id As String) 'Function เอาไว้ Insert ข้อมูลลง tblQuizAnswer ของครู(โหมดแบบไปพร้อมกันครู Insert ให้)

        If Examnum = 0 Or Quiz_Id Is Nothing Or Quiz_Id = "" Then
            Return "Error"
        End If

        Dim KNSession As New ClsKNSession(False)
        Dim CheckError As String = ""

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "IsDifferentQuestion") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
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

        If IsSelfPace = True Then 'เช็คว่าเป็นโหมดแบบ ทำพร้อมกัน ? 

            If IsDifferentQuestion = False Then 'เช็คว่า คำถามเหมือนกัน ?

                'รอถาม Query กับพี่ชินอีกทีว่าถูกไหม
                If InsertQuizScore(Quiz_Id, SchoolCode, Examnum) = "" Then 'Insert ข้อมูลลง tblQuizScore
                    Return "Error"
                End If

                Dim QuestionId As String = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, Examnum) 'Select หา QuestionId
                If QuestionId = "" Then
                    Return "Error"
                End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                'ส่งเข้า Function SetQuizAnswer(QsetType)
                If SetQuizAnswer(QsetType, Quiz_Id, QuestionId) = "Error" Then
                    Return "Error"
                End If

            Else 'ถ้าคำถามไม่เหมือนกัน เข้าเงื่อนไขนี้

                Dim dtPlayer As New DataTable
                sql = " SELECT Player_Id FROM dbo.tblQuizSession WHERE Quiz_Id = '" & Quiz_Id & "' "
                dtPlayer = _DB.getdata(sql)
                Dim CurrentStudentId As String = ""
                Dim EachQuestionId As String = ""
                If dtPlayer.Rows.Count > 0 Then
                    _DB.OpenWithTransection() 'เปิด Transaction
                    For i = 0 To dtPlayer.Rows.Count - 1 'วน Loop นักเรียนทั้งหมดเพื่อ Insert ข้อมูลลง tblQuizScore
                        Try
                            CurrentStudentId = dtPlayer.Rows(i)("Player_Id").ToString()
                            EachQuestionId = GetTop1QuestionIdByPlayerId(CurrentStudentId, Quiz_Id)
                            If EachQuestionId = "" Then 'Select QuestionId เพื่อ Insert แต่ละคน
                                Return "Error"
                            End If
                            'Insert ข้อมูลลง tblQuizScore แบบใช้ Transaction
                            If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, CurrentStudentId, EachQuestionId, _DB, True) = "" Then
                                _DB.RollbackTransection() 'RollBack Transaction
                                Return "Error"
                            End If

                            QsetType = GetQSetTypeFromQuestionId(EachQuestionId) 'Select QsetType จาก QuestionId เพื่อหา Type 
                            If QsetType = "" Then
                                _DB.RollbackTransection() 'RollBack Transaction
                                Return "Error"
                            End If

                            'ส่งเข้า Function SetQuizAnswer(QsetType)
                            If SetQuizAnswer(QsetType, Quiz_Id, EachQuestionId, CurrentStudentId) = "Error" Then
                                Return "Error"
                            End If

                        Catch ex As Exception
                            _DB.RollbackTransection() 'RollBack Transaction
                            Return "Error"
                        End Try
                    Next
                    _DB.CommitTransection() 'Commit Transaction
                Else
                    Return "Error"
                End If

            End If

        Else 'ถ้าทำไม่พร้อมกัน เข้าเงื่อนไขนี้ เพื่อ Insert ข้อมูลให้ครูอย่างเดียว
            'SetQuizAnswer()
            Dim QuestionId As String = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, Examnum)
            If QuestionId = "" Then
                Return "Error"
            End If
            QsetType = GetQSetTypeFromQuestionId(QuestionId)
            If QsetType = "" Then
                Return "Error"
            End If
            Dim TeacherId As String = GetTeacherIdFromQuizId(Quiz_Id) 'หา TeacherId เพื่อเอาไป Insert tblQuizAnswer
            If TeacherId = "" Then
                Return "Error"
            End If
            If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, TeacherId) = "Error" Then
                Return "Error"
            End If

        End If

        Return "Complete"

    End Function

    Private Function SetQuizScoreStudent(ByVal ExamNum As Integer, ByVal Quiz_Id As String, ByVal Player_Id As String) 'Function SetQuizScore ของเด็ก

        If Quiz_Id Is Nothing Or Quiz_Id = "" Or ExamNum = 0 Or Player_Id Is Nothing Or Player_Id = "" Then
            Return "Error"
        End If

        Dim KNSession As New ClsKNSession(False)
        Dim CheckError As String = ""

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "IsDifferentQuestion") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
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

        '--------------------------------------------------------------------------------------------------------------------------------------------------------

        Dim QuestionId As String = ""
        Dim QsetType As String = ""

        If IsSelfPace = True Then 'เช็คว่าเป็นโหมดทำพร้อมกันหรือเปล่า
            'Render เลย
        Else
            'ถ้าเป็นแบบไปไม่พร้อมกันเข้าเงื่อนไขนี้
            If IsDifferentQuestion = False Then 'เช็คว่าคำถามเหมือนกัน ?
                QuestionId = GetQuestionIdFromQuizIdAndExamNum(Quiz_Id, ExamNum) 'หา QuestionId เพื่อนำไป Insert ลง tblQuizScore
                If QuestionId = "" Then
                    Return "Error"
                End If

                If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, Player_Id, QuestionId, _DB, False) = "" Then 'Insert ข้อมูลลง tblQuizScore
                    Return "Error"
                End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                'Function SetQuizAnswer(QsetType)
                If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, Player_Id) = "Error" Then
                    Return "Error"
                End If

            Else 'ถ้าคำถามไม่เหมือนกันเข้าเงื่อนไขนี้
                QuestionId = GetTop1QuestionIdByPlayerId(Player_Id, Quiz_Id)
                If QuestionId = "" Then
                    Return "Error"
                End If

                If InsertQuizScoreWithTransaction(Quiz_Id, SchoolCode, Player_Id, QuestionId, _DB, False) = "" Then 'Insert ข้อมูลลง tblQuizScore
                    Return "Error"
                End If

                QsetType = GetQSetTypeFromQuestionId(QuestionId) 'Select หา QsetType จาก QuestionId ว่าเป็น Type อะไร
                If QsetType = "" Then
                    Return "Error"
                End If

                'Function SetQuizAnswer(QsetType)
                If SetQuizAnswer(QsetType, Quiz_Id, QuestionId, Player_Id) = "Error" Then
                    Return "Error"
                End If

            End If

        End If

        Return "Complete"


    End Function

    Public Function SetQuizAnswer(ByVal QsetType As String, ByVal Quiz_Id As String, ByVal Question_Id As String, Optional ByVal Player_Id As String = "") 'Function SetQuizAnswer สำหรับ Insert ข้อมูลลง tblQuizAnswer

        If QsetType Is Nothing Or QsetType = "" Or Quiz_Id Is Nothing Or Quiz_Id = "" Or Question_Id Is Nothing Or Question_Id = "" Then
            Return "Error"
        End If

        Dim KNSession As New ClsKNSession(False)
        Dim CheckError As String = ""

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") 'หาค่าว่าเป็นโหมดแบบไปพร้อมกันหรือเปล่า
        Dim IsSelfPace As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsSelfPace = CType(CheckError, Boolean)
        End If

        CheckError = KNSession.GetValueFromClsSess(Quiz_Id, "IsDifferentAnswer") 'หาค่าว่าเป็นโหมดแบบคำถามเหมือนกันหรือเปล่า
        Dim IsDifferentAnswer As Boolean
        If CheckError Is Nothing Then
            Return "Error"
        Else
            IsDifferentAnswer = CType(CheckError, Boolean)
        End If

        Dim SchoolCode As String = GetSchoolCodeFromQuizId(Quiz_Id) 'หาค่า SchoolCode จาก QuizId
        If SchoolCode = "" Then
            Return "Error"
        End If

        '---------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim sql As String = ""
        Dim ReturnValue As String = ""
        Dim MergeQuestionId As String = ""

        If IsDifferentAnswer = False Then 'เช็คว่าคำตอบเหมือนกันหรือเปล่า ?

            If QsetType = "6" Then 'เช็คว่าเป็น Type 6 หรือเปล่า
                Dim Qsetid As String = GetQsetIdFromQuestionId(Question_Id)
                If Qsetid = "" Then
                    Return "Error"
                End If
                'Function SaveAnswerForEachStudent ใช้ Transaction
                If SaveAnswerForEachStudent(Quiz_Id, Question_Id, False, Qsetid, Player_Id) = "Error" Then
                    Return "Error"
                End If

            Else 'ถ้าไม่ใช่ Type 6 เข้าเงื่อนไขนี้
                Dim AllQuestionId As String = KNSession.GetValueFromClsSess(Quiz_Id, "CheckInStrquestionId") 'ดึงค่า QuestionId ที่ต่อสตริงกันออกมาเพื่อเอามาเช็คก่อนเข้า Store
                If AllQuestionId Is Nothing Then 'ถ้าค่าที่ดึงออกมาเป็น Nothing แสดงว่ายังไม่มีการ Add ค่าให้ตัวแปรนี้
                    sql = " EXEC dbo.StoreSetQuizAnswer @Question_Id = '" & Question_Id & "', @Quiz_ID = '" & Quiz_Id & "' "
                    ReturnValue = _DB.ExecuteScalar(sql)
                    If ReturnValue <> "" Then
                        KNSession.AddValueForClsSess(Quiz_Id, "CheckInStrquestionId", ReturnValue) 'Add ค่าให้กับตัวแปรนี้
                    End If
                Else 'ถ้าค่าที่ดึงได้ไม่เป็น Nothing ให้เอามาเช็คก่อนว่า QuestionId มีอยู่ใน Instr หรือยัง
                    If InStr(AllQuestionId, Question_Id) = 0 Then 'ถ้ายังไม่มีก็เข้า Store และ Add ค่าเข้าไปใน InStr
                        sql = " EXEC dbo.StoreSetQuizAnswer @Question_Id = '" & Question_Id & "', @Quiz_ID = '" & Quiz_Id & "' "
                        ReturnValue = _DB.ExecuteScalar(sql)
                        If ReturnValue <> "" Then
                            MergeQuestionId = AllQuestionId & "," & ReturnValue
                            KNSession.AddValueForClsSess(Quiz_Id, "CheckInStrquestionId", MergeQuestionId) 'Add ค่าให้กับตัวแปรนี้
                        End If
                    End If
                End If

            End If

        Else 'ถ้าคำตอบไม่เหมือนกันเข้าเงื่อนไขนี้

            If QsetType = 6 Then 'เช็คว่าเป็น Type 6 หรือเปล่า ถ้าเป็น Type 6 ต้องส่ง QsetId เข้าไปด้วย
                Dim Qsetid As String = GetQsetIdFromQuestionId(Question_Id)
                If Qsetid = "" Then
                    Return "Error"
                End If
                'Function SaveAnswerForEachStudent  ไม่ใช้ Transaction
                If SaveAnswerForEachStudent(Quiz_Id, Question_Id, True, Qsetid, Player_Id) = "Error" Then
                    Return "Error"
                End If
            Else ' ถ้าไม่เป็น Type 6 ไม่ต้องส่ง QsetId เข้าไป
                'Function SaveAnswerForEachStudent ไม่ใช้ Transaction
                If SaveAnswerForEachStudent(Quiz_Id, Question_Id, True, , Player_Id) = "Error" Then
                    Return "Error"
                End If
            End If




        End If

        Return "Complete"

    End Function

    Public Function SaveAnswerForEachStudent(ByVal QuizId As String, ByVal QuestionId As String, ByVal IsRandomAnswer As Boolean, Optional ByVal Qset_Id As String = "", Optional ByVal Player_Id As String = "")

        If QuizId Is Nothing Or QuizId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return "Error"
        End If

        '---------------------------------------------------------------------------------------------------------------------------------------------------------------

        If Player_Id = "" Then 'เช็คว่ามี Player_Id หรือยังถ้ายังไม่มีต้องหา Player_Id ก่อน
            Dim dtPlayerId As DataTable = GetPlayerIdFromQuizId(QuizId)
            If dtPlayerId.Rows.Count > 0 Then
                _DB.OpenWithTransection() 'เปิด Transaction 
                Dim EachPlayerId As String = ""
                For i = 0 To dtPlayerId.Rows.Count - 1
                    EachPlayerId = dtPlayerId.Rows(i)("Player_Id").ToString()
                    If MiniFunctionSaveAnswerForEachStudent(QuizId, EachPlayerId, QuestionId, IsRandomAnswer, _DB, True, Qset_Id) = "Error" Then
                        _DB.RollbackTransection() 'ถ้าเกิด Error ให้ RollBackTransaction
                        Return "Error"
                    End If
                Next
                _DB.CommitTransection() 'CommitTransaction
            Else
                Return "Error"
            End If
        Else 'ถ้ามี Player_Id แล้วเข้าเงื่อนไขนี้
            If MiniFunctionSaveAnswerForEachStudent(QuizId, Player_Id, QuestionId, IsRandomAnswer, _DB, False, Qset_Id) = "Error" Then
                Return "Error"
            End If
        End If

        Return "Complete"

    End Function

    Public Function MiniFunctionSaveAnswerForEachStudent(ByVal QuizId As String, ByVal PlayerId As String, ByVal QuestionId As String, ByVal IsRandomAnswer As Boolean, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean, Optional ByVal QsetId As String = "")

        If QuizId Is Nothing Or QuizId = "" Or PlayerId Is Nothing Or PlayerId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return "Error"
        End If

        '------------------------------------------------------------------------------------------------------------------------------------------------------------
        Dim sql As String = ""
        Dim dtAnswer As New DataTable

        If QsetId = "" Then 'ถ้าไม่มี QsetId แสดงว่าเป็นคำถามที่ไม่ใช่ Type 6
            sql = " SELECT Question_Id,Answer_Id FROM tblAnswer WHERE (Question_Id = '" & QuestionId & "') "
            If IsRandomAnswer = True Then 'ถ้าคำถามไม่เหมือนกันเติม Order By NewId() เพิ่ม
                sql &= " ORDER BY NEWID() "
            End If
            dtAnswer = _DB.getdata(sql)
            If LoopInsertQuizAnswer(dtAnswer, QuizId, PlayerId, ObjDB, UseTransaction) = "Error" Then 'วนลูป Insert ข้อมูลลง tblQuizAnswer
                Return "Error"
            End If

        Else 'ถ้ามี QsetId ส่งมาแสดงว่าเป็นคำถามแบบ Type6
            sql = " SELECT tblAnswer.Answer_Id, tblQuizQuestion.Question_Id " & _
                  " FROM tblAnswer INNER JOIN tblQuizQuestion ON tblAnswer.Question_Id = tblQuizQuestion.Question_Id " & _
                  " WHERE (tblAnswer.QSet_Id = '" & QsetId & "') "
            If IsRandomAnswer = True Then 'ถ้าคำถามไม่เหมือนกันเติม Order By NewId() เพิ่ม
                sql &= " ORDER BY NEWID() "
            End If
            dtAnswer = _DB.getdata(sql)
            If LoopInsertQuizAnswer(dtAnswer, QuizId, PlayerId, ObjDB, UseTransaction) = "Error" Then
                Return "Error"
            End If

        End If

        Return "Complete"

    End Function

    Public Function LoopInsertQuizAnswer(ByVal dtAnswer As DataTable, ByVal QuizId As String, ByVal PlayerId As String, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean)

        If dtAnswer.Rows.Count = 0 Or QuizId Is Nothing Or QuizId = "" Or PlayerId Is Nothing Or PlayerId = "" Then
            Return "Error"
        End If

        '----------------------------------------------------------------------------------------------------------------------------------------
        Dim QANumber As Integer = 1
        For i = 0 To dtAnswer.Rows.Count - 1 'วนลูป Insert ลง tblQuizAnswer
            Dim sql As String = " INSERT INTO dbo.tblQuizAnswer " & _
                                " ( QuizAnswer_Id ,Quiz_Id ,Question_Id ,Answer_Id ,QA_No ,IsActive ,LastUpdate , Player_Id ) " & _
                                " VALUES  ( NEWID() , '" & QuizId & "' , '" & dtAnswer.Rows(i)("Question_Id") & "' , '" & dtAnswer.Rows(i)("Answer_Id") & "' " & _
                                " , " & QANumber & " , 1 , GETDATE() , '" & PlayerId & "') "

            Try
                If UseTransaction = True Then
                    ObjDB.ExecuteWithTransection(sql)
                Else
                    ObjDB.Execute(sql)
                End If
            Catch ex As Exception
                Return "Error"
            End Try
            QANumber += 1
        Next

        Return "Complete"

    End Function

    Public Function GetPlayerIdFromQuizId(ByVal QuizId As String) As DataTable

        Dim dtPlayerId As New DataTable
        If QuizId Is Nothing Or QuizId = "" Then
            Return dtPlayerId
        End If

        Dim sql As String = " SELECT Player_Id FROM dbo.tblQuizSession WHERE Quiz_Id = '" & QuizId & "' "
        dtPlayerId = _DB.getdata(sql)
        Return dtPlayerId

    End Function

    Public Function GetTop1QuestionIdByPlayerId(ByVal PlayerId As String, ByVal QuizId As String) As String

        If PlayerId Is Nothing Or PlayerId = "" Or QuizId Is Nothing Or QuizId = "" Then
            Return ""
        End If

        Dim QuestionId As String = ""
        Dim Sql As String = ""
        'Select QuestionId เพื่อ Insert แต่ละคน
        Sql = " SELECT TOP 1 Question_Id FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "'  " & _
                                     " AND Question_Id NOT IN (SELECT Question_Id FROM dbo.tblQuizScore " & _
                                     " WHERE Quiz_Id = '" & QuizId & "' AND Student_Id = '" & PlayerId & "') ORDER BY NEWID() "
        QuestionId = _DB.ExecuteScalar(Sql)
        Return QuestionId

    End Function

    Public Function InsertQuizScore(ByVal QuizId As String, ByVal SchoolCode As String, ByVal Examnum As Integer)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or Examnum = 0 Then
            Return ""
        End If

        Dim sql As String = " INSERT INTO dbo.tblQuizScore( QuizScore_Id ,Quiz_Id ,School_Code ,Question_Id , " & _
                            " Answer_Id ,ResponseAmount ,FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive ,Student_Id ,SR_ID) " & _
                            " SELECT NEWID() ,'" & QuizId & "' , '" & SchoolCode & "' ,tblQuizQuestion.Question_Id, NULL ,0 , " & _
                            " NULL AS  ,GETDATE() , 0 , 0 ,1 , tblQuizScore.Student_Id, t360_tblStudentRoom.SR_ID " & _
                            " FROM t360_tblStudentRoom INNER JOIN " & _
                            " tblQuizScore ON t360_tblStudentRoom.Student_Id = tblQuizScore.Student_Id INNER JOIN " & _
                            " tblQuizQuestion ON tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id AND tblQuizScore.Question_Id = tblQuizQuestion.Question_Id " & _
                            " WHERE (t360_tblStudentRoom.SR_IsActive = 1) AND (tblQuizQuestion.QQ_No = '" & Examnum & "') AND (tblQuizQuestion.Quiz_Id = '" & QuizId & "') "
        'รอถาม Query กับพี่ชินอีกทีว่าถูกไหม
        Try
            _DB.Execute(sql) 'Insert ข้อมูลลง tblQuizScore
        Catch ex As Exception
            Return ""
        End Try

        Return "Complete"

    End Function

    Public Function InsertQuizScoreWithTransaction(ByVal QuizId As String, ByVal SchoolCode As String, ByVal StudentId As String, ByVal QuestionId As String, ByRef ObjDB As ClsConnect, ByVal UseTransaction As Boolean)

        If QuizId Is Nothing Or QuizId = "" Or SchoolCode Is Nothing Or SchoolCode = "" Or StudentId Is Nothing Or StudentId = "" Or QuestionId Is Nothing Or QuestionId = "" Then
            Return ""
        End If

        Dim sql As String = " INSERT INTO dbo.tblQuizScore  " & _
                            " ( QuizScore_Id ,Quiz_Id ,School_Code ,Student_Id_Old ,Question_Id ,Answer_Id , " & _
                            " ResponseAmount ,FirstResponse ,LastUpdate ,Score ,IsScored ,IsActive , " & _
                            " Student_Id ,SR_ID) " & _
                            " SELECT     NEWID() ,'" & QuizId & "' , '" & SchoolCode & "' ,'" & QuestionId & "' ,NULL ,0 " & _
                            " ,NULL ,GETDATE() ,0 ,0 ,1 ,'" & StudentId & "' ,t360_tblStudentRoom.SR_ID " & _
                            " FROM  tblQuizScore INNER JOIN " & _
                            " t360_tblStudentRoom ON tblQuizScore.Student_Id = t360_tblStudentRoom.Student_Id " & _
                            " WHERE (t360_tblStudentRoom.SR_IsActive = 1) AND (tblQuizScore.Question_Id = '" & QuestionId & "')  " & _
                            " AND (tblQuizScore.Student_Id = '" & StudentId & "')  " & _
                            " AND (dbo.tblQuizScore.Quiz_Id = '" & QuizId & "')  "
        Try
            If UseTransaction = True Then
                ObjDB.ExecuteWithTransection(sql)
            Else
                ObjDB.Execute(sql)
            End If

        Catch ex As Exception
            Return ""
        End Try

        Return "Complete"

    End Function


    Public Function GetQSetTypeFromQuestionId(ByVal QuestionId As String) As String

        Dim QsetType As String = ""
        If QuestionId Is Nothing Or QuestionId = "" Then
            Return QsetType
        End If

        Dim sql As String = " SELECT QSet_Type FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "' "
        QsetType = _DB.ExecuteScalar(sql)
        Return QsetType


    End Function

    Public Function GetQsetIdFromQuestionId(ByVal QuestionId As String)

        Dim QsetId As String = ""
        If QuestionId Is Nothing Or QuestionId = "" Then
            Return QsetId
        End If

        Dim sql As String = " SELECT Qset_Id FROM dbo.uvw_GetQsetTypeFromQuestionId WHERE Question_Id = '" & QuestionId & "' "
        QsetId = _DB.ExecuteScalar(sql)
        Return QsetId

    End Function

    Public Function GetSchoolCodeFromQuizId(ByVal QuizId As String) As String

        Dim SchoolCode As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return SchoolCode
        End If

        Dim sql As String = " SELECT t360_SchoolCode FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
        SchoolCode = _DB.ExecuteScalar(sql)

        Return SchoolCode

    End Function

    Public Function GetQuestionIdFromQuizIdAndExamNum(ByVal QuizId As String, ByVal ExamNum As Integer)

        Dim QuestionId As String = ""
        If QuizId Is Nothing Or QuizId = "" Or ExamNum = 0 Then
            Return QuestionId
        End If

        Dim sql As String = " SELECT Question_Id from dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' AND QQ_No = '" & ExamNum & "' "
        QuestionId = _DB.ExecuteScalar(sql)
        Return QuestionId

    End Function

    Public Function GetTeacherIdFromQuizId(ByVal QuizId As String)

        Dim TeacherId As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return TeacherId
        End If

        Dim sql As String = " SELECT TOP 1 Player_Id from dbo.tblQuizSession WHERE Player_Type = 1 AND Quiz_Id = '" & QuizId & "' "
        TeacherId = _DB.ExecuteScalar(sql)
        Return TeacherId

    End Function

End Class
