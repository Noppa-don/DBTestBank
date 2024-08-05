Public Class PrintEditEachQuestion
    Inherits System.Web.UI.Page
    Dim pdfCls As New ClsPDF(New ClassConnectSql)
    Dim _DB As New ClassConnectSql()
    Public txtInfoLbl As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GenQuesionText()
        GenAnswerText()
        GenInfoString()

    End Sub


    Private Sub GenQuesionText()

        Dim QuestionId As String = Request.QueryString("QuestionId").ToString()
        Dim QsetId As String = Request.QueryString("QsetId").ToString()

        Dim sql As String = " SELECT Question_Name,Question_Expain FROM dbo.tblQuestion WHERE Question_Id = '" & _DB.CleanString(QuestionId) & "' and Isactive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then

            If dt.Rows(0)("Question_Name") Is DBNull.Value Then
                lblQuestion.Text = ""
            Else
                Dim QuestionNameComplete As String = dt.Rows(0)("Question_Name").ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
                lblQuestion.Text = QuestionNameComplete
            End If

            If dt.Rows(0)("Question_Expain") Is DBNull.Value Then
                lblQuestionExplain.Text = ""
            Else
                lblQuestionExplain.Text = dt.Rows(0)("Question_Expain").Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
            End If

        End If

    End Sub


    Private Sub GenAnswerText()

        Dim QuestionId As String = Request.QueryString("QuestionId").ToString()
        Dim QsetId As String = Request.QueryString("QsetId").ToString()
        Dim sql As String = " SELECT Answer_Name,Answer_Expain,Answer_Score,Answer_No FROM dbo.tblAnswer WHERE Question_Id = '" & QuestionId & "' and Isactive = 1 ORDER BY Answer_No "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)


        If dt.Rows.Count > 0 Then


            For i = 0 To dt.Rows.Count - 1

                Dim HeadAnswer As String = "คำตอบข้อที่ " & dt.Rows(i)("Answer_No").ToString()

                If dt.Rows(i)("Answer_Score") = 0 Then
                    HeadAnswer = HeadAnswer & " ข้อนี้เป็นข้อ ผิด"
                Else
                    HeadAnswer = HeadAnswer & " ข้อนี้เป็นข้อ ถูก"
                End If

                Dim AnswerName As String = ""
                Dim AnswerExplain As String = ""

                If dt.Rows(i)("Answer_Name") Is DBNull.Value Then
                    AnswerName = ""
                Else
                    AnswerName = dt.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
                End If

                If dt.Rows(i)("Answer_Expain") Is DBNull.Value Then
                    AnswerExplain = ""
                Else
                    AnswerExplain = dt.Rows(i)("Answer_Expain").ToString().Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
                End If


                If i = 0 Then
                    lblAnswerHead1.Text = HeadAnswer
                    lblAnswer1.Text = AnswerName
                    lblAnswerExplain1.Text = AnswerExplain
                ElseIf i = 1 Then
                    Panel5.Visible = True
                    lblAnswerHead2.Text = HeadAnswer
                    lblAnswer2.Text = AnswerName
                    lblAnswerExplain2.Text = AnswerExplain
                ElseIf i = 2 Then
                    Panel1.Visible = True
                    lblAnswerHead3.Text = HeadAnswer
                    lblAnswer3.Text = AnswerName
                    lblAnswerExplain3.Text = AnswerExplain
                ElseIf i = 3 Then
                    Panel2.Visible = True
                    lblAnswerHead4.Text = HeadAnswer
                    lblAnswer4.Text = AnswerName
                    lblAnswerExplain4.Text = AnswerExplain
                ElseIf i = 4 Then
                    Panel3.Visible = True
                    lblAnswerHead5.Text = HeadAnswer
                    lblAnswer5.Text = AnswerName
                    lblAnswerExplain5.Text = AnswerExplain
                ElseIf i = 5 Then
                    Panel4.Visible = True
                    lblAnswerHead6.Text = HeadAnswer
                    lblAnswer6.Text = AnswerName
                    lblAnswerExplain6.Text = AnswerExplain
                End If

            Next

          
        End If



    End Sub


    Private Sub GenInfoString()

        Dim QuestionId As String = Request.QueryString("QuestionId").ToString()
        Dim QsetId As String = Request.QueryString("QsetId").ToString()

       
        'หาหลักสูตร
        Dim Ysql As String = " SELECT tblBook.Book_Syllabus FROM tblQuestion INNER JOIN " & _
                             " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                             " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN " & _
                             " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id " & _
                             " WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
        Dim Year As String = _DB.ExecuteScalar(Ysql)

        Dim FullStr As String = "หลักสูตรปี " & Year & " , "

        FullStr &= "ชั้น"

        'หาชั้น
        Dim sql As String = " SELECT tblLevel.Level_Name FROM tblLevel INNER JOIN tblBook ON tblLevel.Level_Id = tblBook.Level_Id INNER JOIN " & _
                      " tblQuestion INNER JOIN tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                      " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                      " WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim LevelName As String = ""

        If dt.Rows.Count > 0 Then
            If dt.Rows.Count > 2 Then
                LevelName = "มัธยมศึกษาปีที่ 4-6"
            Else
                LevelName = dt.Rows(0)("Level_Name")
            End If
        End If
        
        FullStr &= LevelName & " , "

        'หาวิชา
        sql = "Select tblGroupSubject.GroupSubject_Name FROM tblBook INNER JOIN " & _
              " tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id INNER JOIN " & _
              " tblQuestionCategory ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN " & _
              " tblQuestion INNER JOIN tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id ON " & _
              " tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id " & _
              " WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
        Dim SubjectName As String = _DB.ExecuteScalar(sql)
        If SubjectName = "" Then
            SubjectName = "ไม่พบชื่อวิชา"
        End If
        FullStr &= SubjectName & " , "

        'หาหน่วยการเรียนรู้
        sql = " SELECT tblQuestionCategory.QCategory_Name FROM tblQuestionSet INNER JOIN " & _
              " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id " & _
              " WHERE (tblQuestionSet.QSet_Id = '" & QsetId & "') "
        Dim QCatName As String = _DB.ExecuteScalar(sql)
        If QCatName = "" Then
            QCatName = "ไม่พบหน่วยการเรียนรู้"
        End If
        FullStr &= QCatName & " , "

        'หาชื่อชุดข้อสอบ
        sql = " SELECT QSet_Name FROM dbo.tblQuestionSet WHERE QSet_Id = '" & QsetId & "' "
        Dim QsetName As String = _DB.ExecuteScalar(sql)
        FullStr &= QsetName


        'หาข้อที่
        sql = " SELECT Question_No FROM dbo.tblQuestion WHERE Question_Id = '" & QuestionId & "' "
        Dim QuestionNo As String = _DB.ExecuteScalar(sql)
        If QuestionNo = "" Then
            QuestionNo = "ไม่พบจำนวนข้อ"
        End If
        Dim FullStr2 As String = "ข้อที่ " & QuestionNo & " , "

        'หาจำนวนข้อทั้งชุด
        sql = " SELECT COUNT(Question_Id) FROM dbo.tblQuestion WHERE QSet_Id = '" & QsetId & "' and Isactive = 1 "
        Dim QuestionAmount As String = _DB.ExecuteScalar(sql)
        If QuestionAmount = "" Then
            QuestionAmount = "ไม่พบจำนวนข้อของชุดนี้"
        End If
        FullStr2 &= "จำนวนข้อทั้งชุด " & QuestionAmount & " ข้อ"

        lblInfoQuestion.Text = FullStr
        lblInfoQuestion2.Text = FullStr2

    End Sub


End Class