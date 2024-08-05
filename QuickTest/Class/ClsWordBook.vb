Public Class ClsWordBook

    Dim db As New ClassConnectSql()

    ' funtion รวม โดยส่งแค่ QuestionId 
    Public Function InsertWordToWordBook(ByVal QuestionId As String)
        'Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT tq.Question_Name,ta.Answer_Name FROM tblQuestion tq "
        sql &= " INNER JOIN tblAnswer ta ON tq.Question_Id = ta.Question_Id "
        sql &= " WHERE tq.Question_Id = '" & QuestionId & "' AND tq.IsActive = 1 AND ta.IsActive = 1;"
        Dim dtQuestionNameAndAnswerName As DataTable = db.getdata(sql)
        Dim strQuestionAndAnswer As New StringBuilder()

        Dim i As Integer = 1
        For Each questionAndAnswer In dtQuestionNameAndAnswerName.Rows
            If i = 1 Then
                strQuestionAndAnswer.Append(questionAndAnswer("Question_Name"))
                strQuestionAndAnswer.Append(",")
                strQuestionAndAnswer.Append(questionAndAnswer("Answer_Name"))
            Else
                strQuestionAndAnswer.Append(",")
                strQuestionAndAnswer.Append(questionAndAnswer("Answer_Name"))
            End If
            i += 1
        Next

        ' cover
        'strQuestionAndAnswer = CoverWordWithDotAndRemoveHtml(StripTags(strQuestionAndAnswer.ToString()))
        Dim newStrQuestionAndAnswer As String
        newStrQuestionAndAnswer = CoverWordWithDotAndRemoveHtml(StripTags(strQuestionAndAnswer.ToString()))

        ' split
        Dim ArrWord As ArrayList
        ArrWord = SplitWord(newStrQuestionAndAnswer)

        ' insert
        InsertWordToWordBook = InsertWords(ArrWord, QuestionId)
        
    End Function

    ' replace คำศัพท์ที่มี dot อยู่ระหว่างคำ ให้เป็น $ เพื่อเวลาไป split แล้วคำจะได้ไม่แตก (ต้องมีคำศัพท์ถึงจะ Replace)
    Private Function CoverWordWithDotAndRemoveHtml(ByVal strQuestionAndAnswer As String) As String
        'Dim db As New ClassConnectSql(False, Str)
        Dim sql As String = " SELECT esearch FROM eng2thai WHERE esearch like '%.%'; "
        Dim dtWordWithDot As DataTable = db.getdata(sql)
        Dim newStrQuestionAndAnswer As String = strQuestionAndAnswer

        For Each word In dtWordWithDot.Rows.ToString()
            If InStr(strQuestionAndAnswer, word) > 0 Then
                Dim newWord As String = word.ToString().Replace(".", "$")
                newStrQuestionAndAnswer = strQuestionAndAnswer.Replace(word, newWord)
            End If
        Next
        CoverWordWithDotAndRemoveHtml = newStrQuestionAndAnswer
    End Function

    ' remove tag HTML
    Private Function StripTags(ByVal html As String) As String
        Return Regex.Replace(html, "<.*?>", "")
    End Function

    ' split 
    Private Function SplitWord(ByVal strQuestionAndAnswerForSplit As String) As ArrayList
        Dim ArrWord As Array = strQuestionAndAnswerForSplit.Split(New [Char]() {" "c, ","c, "."c, ":"c, ";"c, "?"c, "!"c, "("c, ")"c, "["c, "]"c, "{"c, "}"c, "<"c, ">"c, "'"c}) '"/"c, "\"c, "_"c
        'Dim ArrNewWord As New ArrayList
        'For Each word In ArrWord
        '    If word <> "" Then
        '        ArrNewWord.Add(word)
        '    End If
        'Next

        Dim ArrWithOutDuplicateWord As ArrayList
        ArrWithOutDuplicateWord = RemoveDuplicateWord(ArrWord)
        'ArrWithOutDuplicateWord.Sort()
        SplitWord = ArrWithOutDuplicateWord
    End Function

    ' ลบค่าที่ซ้ำให้เหลือตัวเดียว และ ลบค่าที่มีตัวอักษรเดียวออกไป
    Private Function RemoveDuplicateWord(ByRef arrWord As Array) As ArrayList
        ' remove ค่าซ้ำ
        Dim ArrDuplicateWord As New ArrayList
        Dim wordTrim As String
        For Each word In arrWord
            wordTrim = word.ToString().Trim()
            If wordTrim.Length() > 1 And Not (IsNumeric(wordTrim)) Then
                If Not ArrDuplicateWord.Contains(wordTrim) Then
                    ArrDuplicateWord.Add(wordTrim)
                End If
            End If
        Next

        ' remove ตัวอักษรเดียว และ ตัวเลข
        'Dim i As Integer = 0
        'Dim ArrOneCharAt As New ArrayList
        'For Each word In ArrDuplicateWord
        '    If word.Length() = 1 Or IsNumeric(word) Then
        '        ArrOneCharAt.Add(word)
        '    End If
        '    i += 1
        'Next
        'For Each at In ArrOneCharAt
        '    ArrDuplicateWord.Remove(at)
        'Next

        RemoveDuplicateWord = ArrDuplicateWord
    End Function

   
    ' Insert คำศัพท์ลง table
    Private Function InsertWords(ByRef ArrWords As ArrayList, ByVal QuestionId As String) As String
        'Dim db As New ClassConnectSql()
        Dim strWords As New StringBuilder

        Dim i As Boolean = True
        For Each words In ArrWords
            Dim sql As String = getStrSqlForAppend(words)
            If i Then
                strWords.Append(sql)
                i = False
            Else
                strWords.Append(" UNION ")
                strWords.Append(sql)
            End If
        Next

        If strWords.Length() > 0 Then
            strWords.Append(" ORDER BY esearch;")

            Dim dt As New DataTable
            Try
                dt = db.getdata(strWords.ToString())
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

            Dim sqlInsertWord As New StringBuilder
            Dim wordIndex As Integer = 1
            For Each dr In dt.Rows
                sqlInsertWord.Append(" INSERT INTO tblWordBook (Question_Id,WordBook_Word,WordBook_Index) VALUES ('" & QuestionId & "' ,'" & dr("esearch") & "','" & wordIndex & "');")
                wordIndex += 1
            Next

            If sqlInsertWord.Length() > 0 Then
                Try
                    db.Execute(sqlInsertWord.ToString())
                    Return "Success"
                Catch ex As Exception
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    Return "Fail"
                End Try
            Else
                Return "No data"
            End If
        Else
            Return "No data"
        End If

    End Function

    ' sql หาคำแปลทั้งหมดของคำใน array
    Private Function getStrSqlForAppend(ByVal words As String) As String
        If words.Last() = "s" And words.Length() > 3 Then
            Dim word As String = words.Substring(0, words.Length - 1)
            getStrSqlForAppend = " SELECT DISTINCT(esearch) FROM eng2thai WHERE esearch = '" & words & "' OR esearch = '" & word & "' "
        Else
            getStrSqlForAppend = " SELECT DISTINCT(esearch) FROM eng2thai WHERE esearch = '" & words & "' "
        End If
    End Function

    ' check ว่า QuestionId เป็นของวิชาอังกฤษ หรือเปล่า
    Public Function QuestionIdIsEnglish(ByVal QuestionId As String) As Boolean
        Dim sql As String = " SELECT q.Question_Id FROM tblbook bk "
        sql &= " INNER JOIN tblQuestionCategory qc ON bk.BookGroup_Id = qc.Book_Id "
        sql &= " INNER JOIN tblQuestionSet qs ON qc.QCategory_Id = qs.QCategory_Id "
        sql &= " INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id"
        sql &= " WHERE bk.GroupSubject_Id = 'FB677859-87DA-4D8D-A61E-8A76566D69D8' AND q.Question_Id = '" & QuestionId & "';"

        Dim IsEng As String = ""
        Try
            IsEng = db.ExecuteScalar(sql)
            If IsEng <> "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return False
        End Try
        
    End Function

    ' delete questionid ออกจาก tblwordbook
    Public Sub ClearQuestionIdInWordbook(ByVal QuestionID As String)
        Dim sqlSb As New StringBuilder()
        sqlSb.Append(" Update tblWordBook Set IsActive = '0' WHERE Question_Id = '" & QuestionID & "'; ")
        Try
            db.Execute(sqlSb.ToString())
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub
End Class
