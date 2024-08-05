Public Class QuestionManagement

    Private db As New ClassConnectSql()

    Private QsetId As String
    Private QsetType As EnumQsetType
    Private Question As QuestionService.Question

    Public Sub New(qsetId As String, qsetType As EnumQsetType)
        Me.QsetId = qsetId
        Me.QsetType = qsetType
        Me.Question = New QuestionService.Question(qsetType)
    End Sub

    Public Function GetQuestionsInQset() As DataTable
        Dim sql As String = "SELECT * FROM tblQuestion WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1 ORDER BY Question_No;"
        Return db.getdata(sql)
    End Function

    Public Function GetAnswersInQuestion(questionId As String) As DataTable
        Dim sql As String = "SELECT * FROM tblAnswer WHERE Qset_Id = '" & QsetId & "' AND Question_Id = '" & questionId & "' AND IsActive = 1 ORDER BY Answer_No;"
        Return db.getdata(sql)
    End Function

    Public Function NewQuestion() As QuestionService.Question
        Dim questionNo As Integer = GetQuestionsInQset().Rows.Count() + 1
        db.OpenWithTransection()
        Dim sql As String = "INSERT INTO tblQuestion(Question_Id,QSet_Id,Question_No,Question_Name,Question_Expain,IsActive,Question_Name_Backup,LastUpdate,IsWpp,ClientId,NoChoiceShuffleAllowed,Question_Name_Quiz,Question_Expain_Quiz) VALUES('" & Question.Id & "','" & Me.QsetId & "'," & questionNo & ",'" & Question.Name & "','" & Question.ExplainName & "',1,'" & Question.Name & "',dbo.GetThaiDate(),0,NULL,0,'" & Question.Name & "','" & Question.ExplainName & "');"
        db.ExecuteScalarWithTransection(sql)
        For Each answer In Question.Answers
            Dim answerNo As Integer = If((QsetType = EnumQsetType.Pair OrElse QsetType = EnumQsetType.Sort), questionNo, answer.No)
            sql = "INSERT INTO tblAnswer VALUES('" & answer.Id & "','" & Question.Id & "',NULL,'" & Me.QsetId & "','" & answerNo & "','" & answer.Name & "','" & answer.ExplainName & "','" & answer.Score & "',0,NULL,1,'" & answer.Name & "',dbo.GetThaiDate(),NULL,0,NULL,'" & answer.Name & "','" & answer.ExplainName & "');"
            db.ExecuteWithTransection(sql)
        Next
        db.CommitTransection()
        Return Question

    End Function

    Public Function DeleteQuestion(questionId As String) As Boolean
        Try
            Dim dtQuestions As DataTable = GetQuestionsInQset()
            db.OpenWithTransection()

            Dim result = (From q In dtQuestions.AsEnumerable() Where q.Field(Of Guid)("Question_Id").ToString().ToLower() = questionId.ToLower()).FirstOrDefault()

            If result IsNot Nothing Then
                Dim sql As String = "DELETE tblQuestion WHERE QSet_Id = '" & QsetId & "' AND Question_Id = '" & questionId & "';"
                db.ExecuteWithTransection(sql)

                Dim result2 = (From q In dtQuestions.AsEnumerable() Where q.Field(Of Int16)("Question_No") > result.Field(Of Int16)("Question_No"))
                For Each r In result2
                    sql = "UPDATE tblQuestion SET Question_No = Question_No - 1,LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & r.Field(Of Guid)("Question_Id").ToString() & "' AND QSet_Id = '" & QsetId & "';"
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

    Public Function DeleteAnswer(questionId As String) As Boolean
        Try
            'Dim db As New ClassConnectSql()
            'db.OpenWithTransection()
            'Dim sql As String = "SELECT * FROM tblAnswer WHERE Question_Id = '" & questionId & "' AND QSet_Id = '" & QsetId & "' ORDER BY Answer_No;"
            'Dim dtAnswer As DataTable = db.getdataWithTransaction(sql)
            'Dim isReOrder As Boolean = False

            'For Each r In dtAnswer.Rows
            '    If isReOrder Then
            '        sql = "UPDATE tblAnswer SET Answer_No = Answer_No - 1 WHERE Question_Id = '" & questionId & "' AND QSet_Id = '" & QsetId & "' AND Answer_Id = '" & r("Answer_Id").ToString() & "';"
            '        db.ExecuteWithTransection(sql)
            '    End If
            '    If r("Answer_Id").ToString().ToLower() = answerId.ToLower() Then
            '        isReOrder = True
            '        sql = "DELETE tblAnswer WHERE Answer_Id = '" & answerId & "';"
            '        db.ExecuteWithTransection(sql)
            '    End If
            'Next
            'db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Public Function DeleteQuestionAnswer(questionId As String) As Boolean
        Try
            Dim dtQuestions As DataTable = GetQuestionsInQset()
            db.OpenWithTransection()

            Dim result = (From q In dtQuestions.AsEnumerable() Where q.Field(Of Guid)("Question_Id").ToString().ToLower() = questionId.ToLower()).FirstOrDefault()

            If result IsNot Nothing Then
                Dim sql As String = "DELETE tblQuestion WHERE QSet_Id = '" & QsetId & "' AND Question_Id = '" & questionId & "';"
                db.ExecuteWithTransection(sql)

                Sql = "DELETE tblAnswer WHERE QSet_Id = '" & QsetId & "' AND Question_Id = '" & questionId & "';"
                db.ExecuteWithTransection(sql)

                Dim result2 = (From q In dtQuestions.AsEnumerable() Where q.Field(Of Int16)("Question_No") > result.Field(Of Int16)("Question_No"))
                For Each r In result2
                    Sql = "UPDATE tblQuestion SET Question_No = Question_No - 1,LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & r.Field(Of Guid)("Question_Id").ToString() & "' AND QSet_Id = '" & QsetId & "';"
                    db.ExecuteWithTransection(sql)
                    Sql = "UPDATE tblAnswer SET Answer_No = Answer_No - 1,LastUpdate = dbo.GetThaiDate() WHERE QSet_Id = '" & QsetId & "' AND Question_Id = '" & r.Field(Of Guid)("Question_Id").ToString() & "';"
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

End Class
