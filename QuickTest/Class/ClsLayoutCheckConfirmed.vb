Imports System.Data.SqlClient

Public Class ClsLayoutCheckConfirmed
    'ตัวแปรใช้จัดการเกี่ยวกับฐานข้อมูล Insert,Update,Delete
    Dim Db As New ClassConnectSql()

    'Enum ประเภท แก้ไข,วิชาการ,พิสูจน์อักษร
    Enum ConfirmedType
        Edit
        Technical
        PrePress
        Auther
        Certify
        SuperAdmin
    End Enum

    'Enum ประเภท ควิซ,ใบงาน
    Enum LayoutType
        Word
        Quiz
    End Enum

    ''' <summary>
    ''' ทำการ หาข้อมูลของชุดข้อสอบ โดยแบ่งตามประเภท ควิซ,ใบงาน และทำการ Bind Repeater ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="qSetid">Id ของชุดข้อสอบ</param>
    ''' <param name="WhereField">ชื่อฟิลด์ที่จะนำไปทำการ Where ใน Query</param>
    ''' <param name="InputRepeater">Repeater Control ที่จะนำมา Bind ข้อมูล</param>
    ''' <param name="IsQuizMode">เป็นโหมดควิซ</param>
    ''' <remarks></remarks>
    Public Sub BindRepeater(ByVal qSetid As String, ByVal WhereField As String, ByRef InputRepeater As Repeater, Optional ByVal IsQuizMode As Boolean = False)
        Dim conn As New SqlConnection
        Db.OpenExclusiveConnect(conn)
        Try
            Dim selectField As String = "Question_Name"
            'ถ้าเป็นโหมดควิซ ต้องไปเอาเนื้อข้อมูลจาก field Question_Name_Quiz
            If IsQuizMode = True Then
                selectField = "Question_Name_Quiz"
            End If
            Dim sql As String = " SELECT tblQuestion.Question_Id,ROW_NUMBER()OVER(ORDER BY Dbo.tblQuestion.Question_No) AS Question_No," &
                                " case when cast(tblQuestion." & selectField & " as varchar) = null then '' else tblQuestion." & selectField & " end as " & selectField & ", " &
                                " tblQuestionSet.QSet_Name, tblLayoutConfirmed." & WhereField & "EditConfirmed, tblLayoutConfirmed." & WhereField & "TechnicalConfirmed, " &
                                " tblLayoutConfirmed." & WhereField & "PrePressConfirmed,tblLayoutConfirmed." & WhereField & "AutherConfirmed,tblLayoutConfirmed." & WhereField & "CertifyConfirmed FROM tblLayoutConfirmed INNER JOIN tblQuestion ON " &
                                " tblLayoutConfirmed.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                                " tblQuestionSet ON tblLayoutConfirmed.Qset_Id = tblQuestionSet.QSet_Id WHERE (tblQuestion.IsActive = 1) AND " &
                                " (tblQuestionSet.IsActive = 1) AND (tblLayoutConfirmed.IsActive = 1) AND Dbo.tblLayoutConfirmed.Qset_Id = '" & qSetid & "' ORDER BY Dbo.tblQuestion.Question_No "
            Dim dt As New DataTable
            dt = Db.getdata(sql, , conn)
            '2017/06/14 maii ไม่ต้องวนเช็คเพราะทำใน Query แล้ว
            'If dt.Rows.Count > 0 Then
            '    For Each a In dt.Rows
            '        If a(selectField) Is DBNull.Value Then
            '            a(selectField) = ""
            '        End If
            '    Next
            InputRepeater.DataSource = dt
            InputRepeater.DataBind()
            'End If
            Db.CloseExclusiveConnect(conn)
        Catch ex As Exception
            Db.CloseExclusiveConnect(conn)
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ Replace เนื้อข้อมูลให้ข้อมูลที่เป็น Path รูปภาพถูกต้องตาม Pattern เพื่อให้รูปขึ้น
    ''' </summary>
    ''' <param name="QuestionName">เนื้อข้อมูล</param>
    ''' <param name="QSetId">QsetId ของคำถามข้อนี้</param>
    ''' <returns>String:เนื้อข้อมูลที่ทำการ Replace เรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Public Function ReplaceModuleURL(ByVal QuestionName As String, ByVal QSetId As String) As String
        If QuestionName IsNot DBNull.Value Then
            Dim rootPath As String = "../file/"
            Dim filePath As String
            filePath = QSetId.Substring(0, 1) + "/" + QSetId.Substring(1, 1) + "/" + QSetId.Substring(2, 1) +
                "/" + QSetId.Substring(3, 1) + "/" + QSetId.Substring(4, 1) + "/" + QSetId.Substring(5, 1) +
                "/" + QSetId.Substring(6, 1) + "/" + QSetId.Substring(7, 1) + "/"
            filePath = filePath + "{" + QSetId + "}/"
            filePath = rootPath + filePath
            Return QuestionName.ToString.Replace("___MODULE_URL___", filePath)
        Else
            Return QuestionName
        End If

    End Function

    ''' <summary>
    ''' ทำการ check ข้อมูลว่า checkbox นี้ถูกติ๊กอยู่หรือเปล่า และ สามารถแก้ไขได้หรือเปล่า
    ''' </summary>
    ''' <param name="Tick">ค่าจาก DB 0=ไม่Tick,1=Tick</param>
    ''' <param name="confirmtype">ประเถทของ checkbox ว่าเป็น แก้ไข,วิชาการ,พิสูจน์อักษร</param>
    ''' <param name="QuestionId">Id ของคำถามข้อนี้</param>
    ''' <returns>String:Tag Checkbox เป็น HTML</returns>
    ''' <remarks></remarks>
    Public Function CheckThisCheckboxIsTick(ByVal Tick As Boolean, ByVal confirmtype As ConfirmedType, ByVal QuestionId As String) As String
        'ตัวแปรที่จะทำการต่อสตริง HTML checkbox
        Dim HTMLCheckboxStr As New StringBuilder()
        'ตัวแปรที่บอกว่า checkbox นี้อยู่ในประเภทไหน ระหว่าง แก้ไข,วิชาการ,พิสูจน์อักษร
        Dim strType As String = ""
        'ตัวแปรที่บอกว่า checkbox นี้ถูก tick หรือเปล่า
        Dim checked As String = ""
        'ตัวแปรที่จะบอกว่า checkbox นี้สามารถแก้ไขได้หรือเปล่า
        Dim disabled As String = ""

        Dim UserCheck As String = HttpContext.Current.Session("UserCheckExamType")

        If Tick = True Then
            checked = "checked='checked'"
        End If

        If confirmtype = ConfirmedType.Edit Then
            strType = "edit"
            disabled = "disabled='disabled'"
        ElseIf confirmtype = ConfirmedType.Technical Then
            'Proof 
            strType = "technical"
            If UserCheck <> "approve" Then
                disabled = "disabled='disabled'"
            End If
        ElseIf confirmtype = ConfirmedType.PrePress Then
            'Approve
            strType = "prepress"
            If UserCheck <> "proof" Then
                disabled = "disabled='disabled'"
            End If
        ElseIf confirmtype = ConfirmedType.Auther Then
            'Auther
            strType = "auther"
            If UserCheck <> "auther" Then
                disabled = "disabled='disabled'"
            End If
        ElseIf confirmtype = ConfirmedType.Certify Then
            'Certify
            strType = "certify"
            If UserCheck <> "certify" Then
                disabled = "disabled='disabled'"
            End If
        End If

        If UserCheck = "superadmin" And strType <> "edit" Then
            disabled = ""
        End If



        HTMLCheckboxStr.Append("<input type='checkbox' " & checked & " qid='" & QuestionId & "' class='" & strType & "' " & disabled & " />")
        Return HTMLCheckboxStr.ToString()
    End Function

    ''' <summary>
    ''' ทำการต่อสตริง Tag รูป เป็นรูปดินสอ สำหรับโหมดใบงาน , รูปแว่นขยายสำหรับโหมด ควิซ โดยมีการใส่ attr สำคัญๆไปด้วย
    ''' </summary>
    ''' <param name="QuestionId">Id ของคำถามข้อนี้</param>
    ''' <param name="QsetId">QsetId ของคำถามข้อนี้</param>
    ''' <param name="ClassName">ชื่อ Class ของ Tag Image</param>
    ''' <param name="ImgName">ชื่อไฟล์รูปภาพ</param>
    ''' <returns>String:Tag Image</returns>
    ''' <remarks></remarks>
    Public Function GenEditAndPreviewImg(ByVal QuestionId As String, ByVal QsetId As String, QuestionNo As String, ByVal ClassName As String, ByVal ImgName As String) As String
        Dim imgPreview As New StringBuilder()
        imgPreview.Append("<img class='" & ClassName & "' qid='" & QuestionId & "' qsetid='" & QsetId & "' QNo='" & QuestionNo & "' src='../Images/" & ImgName & ".png' >")
        Return imgPreview.ToString()
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูลคำถามทั้งหมดใน ชุดคำถามเพิมเข้าไปใน tblLayoutConfirmed ด้วย
    ''' </summary>
    ''' <param name="QsetId">Id ของชุดคำถาม</param>
    ''' <remarks></remarks>
    Public Sub InsertQuestionInTblLayoutCheckConfirmed(ByVal QsetId As String)
        Dim conn As New SqlConnection()
        'open connection
        Db.OpenExclusiveConnect(conn)
        'open transaction
        Db.OpenWithTransection(conn)
        Dim sql As String = ""
        Dim dt As New DataTable
        Try
            'ไปหา question ทั้งหมดที่อยู่ใน qsetid นี้มาก่อน
            sql = " SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1 ORDER BY Question_No "
            dt = Db.getdataWithTransaction(sql, , conn)
            If dt.Rows.Count > 0 Then
                'วน loop insert ลง tblLayoutConfirmed ทีละข้อ , เงื่อนไขการจบ loop วนคำถามทั้งหมดในชุดคำถามนี้
                For index = 0 To dt.Rows.Count - 1
                    sql = " INSERT INTO dbo.tblLayoutConfirmed ( LC_Id ,Qset_Id ,Question_Id ,WordEditConfirmed ,WordTechnicalConfirmed ,WordPrePressConfirmed , " &
                          " WordEditConfirmed_UserId ,WordEditConfirmed_LastUpdate ,WordTechnicalConfirmed_UserId , WordTechnicalConfirmed_LastUpdate ,WordPrePressConfirmed_UserId ,WordPrePressConfirmed_LastUpdate , " &
                          " QuizEditConfirmed ,QuizTechnicalConfirmed ,QuizPrePressConfirmed ,QuizEditConfirmed_UserId , QuizEditConfirmed_LastUpdate ,QuizTechnicalConfirmed_UserId ,QuizTechnicalConfirmed_LastUpdate , " &
                          " QuizPrePressConfirmed_UserId ,QuizPrePressConfirmed_LastUpdate ,LastUpdate ,IsActive ) VALUES  ( NEWID() , '" & QsetId & "', '" & dt.Rows(index)("Question_Id").ToString() & "' , 0 , 0 , 0 , NULL ,  " &
                          " NULL , NULL , NULL , NULL , NULL , 0 , 0 , 0 , NULL , NULL , NULL , NULL , NULL , NULL , dbo.GetThaiDate() ,1 ) "
                    Db.ExecuteWithTransection(sql, conn)
                Next
            End If
            'commit transaction
            Db.CommitTransection(conn)
            'close connection
            Db.CloseExclusiveConnect(conn)
        Catch ex As Exception
            'commit transaction
            Db.RollbackTransection(conn)
            'close connection
            Db.CloseExclusiveConnect(conn)
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ Update ข้อมูลลง tblLayoutConfirmed หลังจากที่ได้ทำการแก้ไขแล้ว
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ของคำถามข้อที่ต้องการ Update</param>
    ''' <param name="CheckType">ประเภทที่ต้องการ Update มี Quiz,Word</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามที่ต้องการ Update</param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateEditConfirmedThisQuestion(ByVal QuestionId As String, ByVal CheckType As LayoutType, Optional ByVal QsetId As String = Nothing)
        Dim db As New ClassConnectSql()
        Dim conn As New SqlConnection()
        'Open connection
        db.OpenExclusiveConnect(conn)
        Try
            'ใช้เพื่อมาแทน Field ที่จะ Update
            Dim updateField As String = ""
            If CheckType = LayoutType.Quiz Then
                updateField = "QuizEditConfirmed"
            ElseIf CheckType = LayoutType.Word Then
                updateField = "WordEditConfirmed"
            End If
            'update confirmed this quesiton
            Dim sql As String = ""
            'ถ้าส่ง Qset มาก็ให้ update ทั้ง qset เลย
            If QsetId IsNot Nothing Then
                sql = "UPDATE dbo.tblLayoutConfirmed SET " & updateField & " = 1,LastUpdate = dbo.GetThaiDate() WHERE Qset_Id = '" & QsetId & "'; "

            Else
                sql = " UPDATE dbo.tblLayoutConfirmed SET " & updateField & " = 1,LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & QuestionId & "'; "
            End If
            db.Execute(sql, conn)
            'log
            Log.Record(Log.LogType.ManageExam, "ยินยัน Layout ฝั่ง " & CheckType.ToString & " (QuestionId='" & QuestionId & "')", True, "", QuestionId)
            'close connection
            db.CloseExclusiveConnect(conn)
        Catch ex As Exception
            'close connection
            db.CloseExclusiveConnect(conn)
        End Try
    End Sub

End Class
