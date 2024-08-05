Public Class ActivityPage_Preview
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql
    Private clsPDF As New ClsPDF(db)
    Private QuizId As String
    Private QsetId As String
    Private QsetType As Integer
    Private AnswerExpHtml As New StringBuilder

    Public StudentId As String
    Public ExplainPageNumber As Integer = 1

    Protected LastQuestionNo As Integer
    Protected CorrectAnswer As String = ""
    Protected MyAnswer As String = ""
    Protected IsNoAnswer As Boolean = True
    Protected DeviceUinqueId As String = ""
    Protected Token As String = ""
    Protected IsJumpNo As Boolean = False
    Dim conDB As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InitialData()

        If Not IsPostBack Then
            ArrNo.Value = 1
            ExamNo.Value = 1
            OrderNo.Value = 0
            AnsweredMode.Value = 0
            QuizPreview()
            IsSelectedExplain.Value = True
            GenPanelSelectExplain()
            IsJumpTo.Value = False
        Else
            If IsJumpTo.Value Then
                If CInt(AnsweredMode.Value) = 0 Then
                    imgAnsweredMode.ImageUrl = "../Images/Activity/SelectExplain/AllQuestion.png"
                ElseIf CInt(AnsweredMode.Value) = 1 Then
                    imgAnsweredMode.ImageUrl = "../Images/Activity/SelectExplain/RightAnswer.png"
                ElseIf CInt(AnsweredMode.Value) = 2 Then
                    imgAnsweredMode.ImageUrl = "../Images/Activity/SelectExplain/WrongAnswer.png"
                ElseIf CInt(AnsweredMode.Value) = 3 Then
                    imgAnsweredMode.ImageUrl = "../Images/Activity/SelectExplain/SkipAnswer.png"
                End If
                QuizPreview()
                IsJumpTo.Value = False
            End If
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim n As Integer = CInt(OrderNo.Value)
        OrderNo.Value = n + 1
        QuizPreview()
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        Dim n As Integer = CInt(OrderNo.Value)
        If n > 0 Then OrderNo.Value = n - 1
        QuizPreview()
    End Sub

    Private Sub InitialData()
        Me.QuizId = Request.QueryString("quizid").ToString()
        HttpContext.Current.Session("Quiz_Id") = Request.QueryString("quizid").ToString()
        Me.DeviceUinqueId = Request.QueryString("deviceuniqueid").ToString()
        Me.Token = Request.QueryString("token").ToString()
        SetQuizDetail()
    End Sub

    Private Sub QuizPreview()

        Dim arrAnsNo() As String

        If ArrNo.Value = "" Then
            arrAnsNo(0) = "1"
        Else
            arrAnsNo = Split((ArrNo.Value), ",")
        End If

        ExamNo.Value = arrAnsNo(CInt(OrderNo.Value))
        Dim questionNo As String = CInt(ExamNo.Value)

        Dim arrQuestion() As String = RenderQuestion(questionNo)

        QuestionTd.InnerHtml = arrQuestion(0)
        'tdReportDetail.Attributes.Add("name", arrQuestion(2).ToString)
        txtHidden.InnerText = arrQuestion(1).ToString
        AnswerTbl.InnerHtml = RenderAnswer(questionNo)

        AnswerExp.InnerHtml = AnswerExpHtml.ToString()

    End Sub

    Private Sub GenPanelSelectExplain()
        divSelectExplain.Style.Add("display", "block")
        Dim scriptKey As String = "UniqueKeyForThisScript"
        Dim javaScript As String = "<script type='text/javascript'>CreateButtonSelectExplain();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)
    End Sub


    Private Function RenderQuestion(ByVal ExamNum As String) As Array
        Dim QuestionDetail(2) As String
        Dim tempQuestion As String

        Dim dtQuestion As DataTable = GetCurrentQuestionName(ExamNum)

        If dtQuestion.Rows.Count > 0 Then
            Me.QsetId = dtQuestion.Rows(0)("QSet_Id").ToString()
            Me.QsetType = dtQuestion.Rows(0)("QSet_Type")
            If Me.QsetType = EnumQsetType.Sort Or Me.QsetType = EnumQsetType.Pair Then
                tempQuestion = clsPDF.CleanSetNameText(dtQuestion.Rows(0)("QSet_Name"))
            Else
                If dtQuestion.Rows(0)("Question_Name_Quiz") IsNot DBNull.Value Then
                    tempQuestion = dtQuestion.Rows(0)("Question_Name_Quiz")
                Else
                    tempQuestion = dtQuestion.Rows(0)("Question_Name")
                End If
            End If

            If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value And dtQuestion.Rows(0)("Question_Expain").ToString() <> "" Then
                tempQuestion = String.Format("{0}<div id=""QuestionExp"" style=""display: none;"">{1}</div>", tempQuestion, dtQuestion.Rows(0)("Question_Expain").Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath))
            End If

            QuestionDetail(0) = ExamNum & ". " & tempQuestion.Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath)
            QuestionDetail(1) = dtQuestion.Rows(0)("Question_Id").ToString
        End If

        Return QuestionDetail
    End Function


    Private Function GetCurrentQuestionName(questionNo As Integer) As DataTable
        Dim sql As New StringBuilder
        sql.Append(" SELECT qs.QSet_Id,CAST(qs.QSet_Name AS VARCHAR(MAX))AS QSet_Name,  CAST(q.Question_Name AS VARCHAR(MAX)) AS Question_Name,q.Question_Id, ")
        sql.Append(" CAST(q.Question_Expain   As varchar(max)) As Question_Expain ,CAST(q.Question_Name_Quiz As varchar(max))As Question_Name_Quiz,q.Question_Id, ")
        sql.Append(" CAST(q.Question_Expain_Quiz as varchar(max)) as Question_Expain_Quiz ,qs.QSet_Type  ")
        sql.Append(" FROM tblQuizQuestion qq INNER JOIN tblQuestion q ON qq.Question_Id = q.Question_Id ")
        sql.Append(" INNER JOIN tblQuestionset qs ON qs.QSet_Id = q.QSet_Id INNER JOIN tblQuiz qu ON qu.Quiz_Id = qq.Quiz_Id ")
        sql.Append(" WHERE qq.Quiz_Id = '" & QuizId & "' AND qu.User_Id = '" & StudentId & "' AND qq.QQ_No = " & questionNo & ";")
        Return db.getdata(sql.ToString())
    End Function


    Private Function RenderAnswer(ByVal ExamNum As String) As String 'รับ ExamNum มาสร้าง html Answer 

        Dim tempAnswer As String = ""

        If Me.QsetType = EnumQsetType.Sort Then
            tempAnswer = GetAnswerType6(ExamNum)
        ElseIf Me.QsetType = EnumQsetType.Pair Then
            tempAnswer = GetAnswerType3(ExamNum)
        Else
            tempAnswer = GetHtmlAnswer(ExamNum)
        End If

        Return tempAnswer

    End Function


    Private Function GetHtmlAnswer(ByVal ExamNum As String) As String
        Dim tempHtmlAnswer As New StringBuilder

        Dim dtAnswer As DataTable = GetAnswer(ExamNum)

        If dtAnswer.Rows.Count > 0 Then

            If Me.QsetType = EnumQsetType.TrueFalse Then
                For Each i In dtAnswer.Rows
                    i("Answer_name") = If((i("Answer_name") = "True"), "ถูก", "ผิด")
                Next
            End If

            Dim StuAnswer As String = If((dtAnswer.Rows(0)("AnsweredId") IsNot DBNull.Value), dtAnswer.Rows(0)("AnsweredId").ToString(), "") 'Answer_Id ที่เด็กตอบ

            Dim BG As String = ""
            Dim ClassHtmlAnswerExpain As String
            Dim Answered As String = ""

            Dim greenBG As String = "background-color:#2CA505;color:white;"
            Dim redBG As String = "background-color:#FF0B00;color:white;"

            'Dim ShowCircle As String = ""

            ' LOOP สร้างส่วนของคำตอบ
            For i = 0 To dtAnswer.Rows.Count - 1


                Dim PrefixAnswer() As String = If(IsGroupSubjectEng(dtAnswer.Rows(i)("Question_ID").ToString()), {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"}, {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ", "ฒ", "ณ", "ด", "ต", ""})

                ' ShowCircle = ""
                Answered = ""

                If StuAnswer <> "" Then
                    IsNoAnswer = False
                    If dtAnswer.Rows(i)("Answer_Id").ToString = StuAnswer Then
                        BG = If((CInt(dtAnswer.Rows(i)("Answer_Score")) > 0), greenBG, redBG)
                        'ShowCircle = "style='display:block !important;'"
                        'เอาไว้ใช้ตรงส่วนของคำอธิบายคำตอบ
                        Answered = "<img src=""../Images/Activity/ChooseCircle_pad.png"" class=""ImgCircle"" style=""display:block !important;"" />"
                    Else
                        BG = If((CInt(dtAnswer.Rows(i)("Answer_Score")) > 0), greenBG, "")
                    End If
                Else
                    BG = If((CInt(dtAnswer.Rows(i)("Answer_Score")) > 0), greenBG, "")
                End If


                Dim AnswerName As String = dtAnswer.Rows(i)("Answer_name").Replace("___MODULE_URL___", QsetId.ToFolderFilePath)

                If i Mod 2 = 0 Then
                    'Answer ฝั่งซ้าย
                    tempHtmlAnswer.Append("<tr style='border-bottom: solid 1px #AFAFAF;vertical-align: top;'>")
                    tempHtmlAnswer.Append("<td style=""" & BG & "height: 50px;font-weight: bold;width:35px;position:relative;"">")

                    tempHtmlAnswer.Append(PrefixAnswer(i) & "." & Answered & "</td>")
                    tempHtmlAnswer.Append("<td style=""" & BG & "height: 50px;width:45%;"">" & AnswerName & "</td>")
                Else
                    'Answer ฝั่งขวา
                    tempHtmlAnswer.Append("<td style=""height: 50px;width:30px;""></td>")
                    tempHtmlAnswer.Append("<td style=""" & BG & "height: 50px;font-weight: bold;width:35px;position:relative;"">")

                    'tempHtmlAnswer.Append(PrefixAnswer(i) & ".<img src='../Images/Activity/ChooseCircle_pad.png' " & ShowCircle & " class='ImgCircle' /></td>")
                    tempHtmlAnswer.Append(PrefixAnswer(i) & "." & Answered & "</td>")
                    tempHtmlAnswer.Append("<td style=""" & BG & "height: 50px;width:45%;"">" & AnswerName & "</td>")

                End If

                If dtAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value Then
                    If InStr(BG, "#2CA505") Then
                        ClassHtmlAnswerExpain = "Correct"
                    ElseIf InStr(BG, "#FF0B00") Then
                        ClassHtmlAnswerExpain = "InCorrect"
                    Else
                        ClassHtmlAnswerExpain = "NotAnswered"
                    End If
                    AnswerExpHtml.Append("<div>")
                    AnswerExpHtml.Append(String.Format("<div class='{0}'>{1}", ClassHtmlAnswerExpain, Answered))
                    AnswerExpHtml.Append("<table style='width:100%;'><tr style='vertical-align:top;'><td>")
                    AnswerExpHtml.Append(PrefixAnswer(i) & ". ")
                    AnswerExpHtml.Append("</td><td>")
                    AnswerExpHtml.Append(AnswerName)

                    If dtAnswer.Rows(i)("Answer_Expain").ToString().Trim() <> "" Then
                        AnswerExpHtml.Append("<div>")
                        AnswerExpHtml.Append(dtAnswer.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", QsetId.ToFolderFilePath))
                        AnswerExpHtml.Append("</div>")
                    End If
                    AnswerExpHtml.Append("</td></tr></table>")
                    AnswerExpHtml.Append("</div>")
                    AnswerExpHtml.Append("</div>")
                End If

            Next
        End If

        Return tempHtmlAnswer.ToString()

    End Function

    Private Function GetAnswer(questionNo As Integer) As DataTable
        Dim sql As New StringBuilder
        sql.Append("SELECT a.Answer_Id,qq.Question_Id , Answer_name,Answer_Score,AlwaysShowInLastRow,Answer_Expain ,qs.Answer_Id as AnsweredId,qs.IsScored ")
        sql.Append("FROM tblAnswer a INNER JOIN tblQuizQuestion qq on a.Question_Id = qq.Question_Id LEFT JOIN tblQuizScore qs ")
        sql.Append("ON qq.Quiz_Id = qs.Quiz_Id AND qq.Question_Id = qs.Question_Id ")
        sql.Append("WHERE qq.Quiz_Id = '" & QuizId & "'  AND a.isactive = 1 AND qq.QQ_No = '" & questionNo & "' ")
        sql.Append("ORDER BY Answer_No,AlwaysShowInLastRow;")
        Return db.getdata(sql.ToString())
    End Function


    Private Function GetAnswerType6(ByVal ExamNum As String) As String



        Dim sql As String

        Dim dtQuestion As DataTable
        Dim dtCorrectAnswer As DataTable
        Dim dtCheckAnswer As DataTable

        Dim BG As String

        Dim tempHtmlAnswer As New StringBuilder
        Dim tempHtmlCorrectAnswer As New StringBuilder

        Dim QuestionName As String
        Dim CorrectQuestionName As String = ""
        Dim QuestionId As String
        Dim CorrectQuestionId As String = ""

        Dim ClassHtmlAnswerExpain As String

        Dim greenBG As String = "background-color:#2CA505;color:white;"
        Dim redBG As String = "background-color:#FF0B00;color:white;"


        '1. ให้เรียงตามที่ตอบ ป้ายสีเขียวแดง เลื่อนไม่ได้

        sql = " SELECT tblQuestion.Question_Name, tblQuizAnswer.Question_Id,tblQuestion.QSet_Id , tblAnswer.Answer_Name as number ,tblAnswer.Answer_Expain "
        sql &= " FROM tblQuizAnswer INNER JOIN"
        sql &= " tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
        sql &= " tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
        sql &= " where tblQuizAnswer.Question_Id in (select Question_Id from tblQuestion where QSet_Id = "
        sql &= " (select QSet_Id from tblQuestion where Question_Id in ("
        sql &= " select Question_Id from tblQuizScore where Quiz_Id = '" & Me.QuizId & "' "
        sql &= " and Student_Id = '" & Me.StudentId & "' and QQ_No = '" & ExamNum & "' )))"
        sql &= " and Quiz_Id = '" & Me.QuizId & "'"
        sql &= " and Player_Id = '" & Me.StudentId & "'"
        sql &= " order by QA_No"

        dtQuestion = db.getdata(sql)

        sql = "select Question_id from tblAnswer where QSet_Id = (Select QSet_Id from tblQuestion where Question_Id = ("
        sql &= " Select Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & Me.QuizId & "'  "
        sql &= " And QQ_No = '" & ExamNum & "' and Student_Id = '" & Me.StudentId & "')) ORDER BY CAST(Answer_Name as varchar(max));"
        dtCheckAnswer = db.getdata(sql)

        '2.  ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
        sql = "SELECT ROW_NUMBER() over(order by cast(Answer_Name as varchar(max)))as Number,tblQuestion.Question_Name, tblQuizAnswer.Question_Id, tblQuestion.QSet_Id,tblAnswer.Answer_Expain "
        sql &= " FROM tblQuizAnswer INNER JOIN"
        sql &= " tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN"
        sql &= " tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id AND tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id"
        sql &= " WHERE tblQuizAnswer.Question_Id IN"
        sql &= " (SELECT Question_Id FROM tblQuestion WHERE QSet_Id = (SELECT QSet_Id FROM tblQuestion"
        sql &= " WHERE  Question_Id IN (SELECT Question_Id FROM tblQuizScore"
        sql &= " WHERE Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND Student_Id = '" & Me.StudentId & "' "
        sql &= " AND QQ_No = '" & ExamNum & "'))) "
        sql &= " AND tblQuizAnswer.Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND tblQuizAnswer.Player_Id = '" & Me.StudentId & "'"
        sql &= " ORDER BY cast(tblAnswer.Answer_Name as varchar(max))"
        dtCorrectAnswer = db.getdata(sql)

        tempHtmlAnswer.Append("<tr class=""6"" id=""Answer""><td><ul style=""margin-left:-40px;"" >")
        tempHtmlCorrectAnswer.Append("<tr><td><ul style=""margin-left:-40px;"" >")

        For i = 0 To dtQuestion.Rows.Count - 1

            'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง
            If dtQuestion.Rows(i)("Question_Id") = dtCheckAnswer.Rows(i)("Question_Id") Then
                BG = greenBG
                ClassHtmlAnswerExpain = "Correct"
            Else
                BG = redBG
                ClassHtmlAnswerExpain = "InCorrect"
            End If

            QuestionName = dtQuestion.Rows(i)("Question_Name")
            QuestionName = QuestionName.Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath)
            QuestionId = dtQuestion.Rows(i)("Question_Id").ToString

            ' เพิ่มคำอธิบายคำตอบ
            If dtQuestion.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtQuestion.Rows(i)("Answer_Expain").ToString() <> "" Then
                AnswerExpHtml.Append(String.Format("<div class=""{0}"">{1}", ClassHtmlAnswerExpain, dtQuestion.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath)))
                AnswerExpHtml.Append("</div>")
            End If

            tempHtmlAnswer.Append("<li id=""" & QuestionId & """ style=""" & BG & """><span class=""CorrectLi"">ลำดับที่ " & dtQuestion.Rows(i)("Number").ToString & " </span>" & QuestionName & AnswerExp.ToString() & "</li>")


            CorrectQuestionName = dtCorrectAnswer.Rows(i)("Question_Name").ToString
            CorrectQuestionName = CorrectQuestionName.Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath)
            CorrectQuestionId = dtCorrectAnswer.Rows(i)("Question_Id").ToString

            ' เพิ่มคำอธิบายคำตอบ
            If dtCorrectAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtCorrectAnswer.Rows(i)("Answer_Expain").ToString() <> "" Then
                AnswerExpHtml.Append(String.Format("<div class=""{0}"">{1}", "Correct", dtCorrectAnswer.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", Me.QsetId.ToFolderFilePath)))
                AnswerExpHtml.Append("</div>")
            End If

            tempHtmlCorrectAnswer.Append("<li id=""" & CorrectQuestionId & """ style=""background-color:#2CA505;""><span class=""CorrectLi"">ลำดับที่ " & dtCorrectAnswer.Rows(i)("Number").ToString & " </span>" & CorrectQuestionName & AnswerExp.ToString() & "</li>")


        Next

        tempHtmlAnswer.Append("</ul></td></tr>")
        tempHtmlCorrectAnswer.Append("</ul></td></tr>")

        MyAnswer = tempHtmlAnswer.ToString()
        CorrectAnswer = tempHtmlCorrectAnswer.ToString()

        Return tempHtmlAnswer.ToString()

    End Function

    Private Function GetAnswerType3(ByVal ExamNum As String) As String

        Dim sql As String

        Dim dtQuestion As DataTable
        Dim dtCorrectAnswer As DataTable
        'Dim dtCheckAnswer As DataTable

        Dim BG As String = ""

        Dim tempHtmlAnswer As New StringBuilder
        Dim tempHtmlCorrectAnswer As New StringBuilder

        Dim QuestionName As String = ""
        Dim AnswerName As String = ""

        Dim greenBG As String = "background-color:#2CA505;color:white;"
        Dim redBG As String = "background-color:#FF0B00;color:white;"


        'ถ้าเฉลยของเด็กหรือของครูแบบฝึกฝน 
        '1. ให้เรียงตามที่ตอบ ป้ายสีเขียวแดง เลื่อนไม่ได้
        sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain "
        sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
        sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
        sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id "
        sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND tblQuizQuestion.QQ_No = (SELECT tblQuizScore.QQ_No FROM tblQuizScore INNER JOIN tblQuizQuestion "
        sql &= " ON tblQuizScore.Question_Id = tblQuizQuestion.Question_Id AND tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id "
        sql &= " WHERE tblQuizScore.Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Student_Id = '" & Me.StudentId & "') "
        sql &= " AND tblQuizAnswer.Player_Id = '" & Me.StudentId & "' "
        sql &= " ORDER BY tblQuizAnswer.QA_No; "
        dtQuestion = db.getdata(sql)


        '2.  ให้เรียงแบบถูก ป้ายสีเขียวทั้งหมด เลื่อนไม่ได้
        sql = " SELECT tblQuestion.QSet_Id,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id,tblAnswer.Answer_Name,tblAnswer.Answer_Expain "
        sql &= " FROM tblQuizQuestion INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id "
        sql &= " INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id "
        sql &= " INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id "
        sql &= " WHERE tblQuizQuestion.Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND tblQuizQuestion.QQ_No = (SELECT tblQuizScore.QQ_No FROM tblQuizScore INNER JOIN tblQuizQuestion "
        sql &= " ON tblQuizScore.Question_Id = tblQuizQuestion.Question_Id AND tblQuizScore.Quiz_Id = tblQuizQuestion.Quiz_Id "
        sql &= " WHERE tblQuizScore.Quiz_Id = '" & Me.QuizId & "' "
        sql &= " AND tblQuizScore.QQ_No = '" & ExamNum & "' AND tblQuizScore.Student_Id = '" & Me.StudentId & "') "
        sql &= " AND tblQuizAnswer.Player_Id = '" & Me.StudentId & "' "
        sql &= " ORDER BY tblQuizAnswer.QA_No; "
        dtCorrectAnswer = db.getdata(sql)


        ' case นี้เจอเมื่อ ครูกดไปถึงข้อจับคู่ แล้วออกก่อน
        If dtQuestion.Rows.Count = 0 And dtCorrectAnswer.Rows.Count = 0 Then
            dtQuestion = GetTempQuestionType3(ExamNum)
            dtCorrectAnswer = dtQuestion
        End If

        Dim fileQsetPath As String = Me.QsetId.ToFolderFilePath

        ' LOOP สร่้าง คำถามตอบ
        For i = 0 To dtQuestion.Rows.Count - 1

            QuestionName = dtQuestion.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", fileQsetPath)
            AnswerName = dtQuestion.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", fileQsetPath)

            'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง
            BG = If((dtQuestion.Rows(i)("Answer_Id") = dtCorrectAnswer.Rows(i)("Answer_Id")), greenBG, redBG)

            Dim CorrectAnswerName As String = dtCorrectAnswer.Rows(i)("Answer_Name").ToString()
            ' render tag Correct
            tempHtmlCorrectAnswer.Append("<tr  style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>")
            tempHtmlCorrectAnswer.Append("<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td  ")
            tempHtmlCorrectAnswer.Append("style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span style=""background-color:#1EEE1E;"" >" & CorrectAnswerName & "</span></td></tr>")


            ' เพิ่มคำอธิบายคำตอบ
            If dtCorrectAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value And dtCorrectAnswer.Rows(i)("Answer_Expain").ToString() <> "" Then
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(String.Format("<div class='Correct'>{0}  คู่กับ  {1}", QuestionName, CorrectAnswerName))
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(dtCorrectAnswer.Rows(i)("Answer_Expain"))
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
            End If

            ' render tag 
            tempHtmlAnswer.Append("<tr class=""3"" style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>")
            tempHtmlAnswer.Append("<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td ")
            tempHtmlAnswer.Append("style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span style=""" & BG & """ >" & AnswerName & "</span></td></tr>")

        Next

        MyAnswer = tempHtmlAnswer.ToString()
        CorrectAnswer = tempHtmlCorrectAnswer.ToString()

        Return tempHtmlAnswer.ToString()

    End Function

    Private Function GetTempQuestionType3(ExamNum As String) As DataTable
        Dim sql As String
        sql = " SELECT a.QSet_Id,a.Question_Id,q.Question_Name,a.Answer_Id,a.Answer_Name,a.Answer_Expain"
        sql &= " FROM tblQuizQuestion qq INNER JOIN tblAnswer a ON qq.Question_Id = a.Question_Id "
        sql &= " INNER JOIN tblQuestion q ON q.Question_Id = a.Question_Id "
        sql &= " WHERE qq.Quiz_Id = '" & Me.QuizId & "' and qq.QQ_No = " & ExamNum & " ;"
        Return db.getdata(sql)
    End Function


    Private Function IsGroupSubjectEng(ByVal QuestionId As String) As Boolean
        Dim sql As New StringBuilder()
        sql.Append(" SELECT TOP 1 tgs.GroupSubject_Name FROM tblQuestion tq INNER JOIN tblQuestionSet tqs ON tq.QSet_Id = tqs.QSet_Id ")
        sql.Append(" INNER JOIN tblQuestionCategory tqc ON tqs.QCategory_Id = tqc.QCategory_Id ")
        sql.Append(" INNER JOIN tblbook tb ON tqc.Book_Id = tb.BookGroup_Id ")
        sql.Append(" INNER JOIN tblGroupSubject tgs ON tb.GroupSubject_Id = tgs.GroupSubject_Id ")
        sql.Append(" WHERE tq.Question_Id = '")
        sql.Append(QuestionId)
        sql.Append("';")

        If db.ExecuteScalar(sql.ToString()) = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ" Then
            Return True
        End If
        Return False

    End Function

    Private Sub SetQuizDetail()
        Dim dtDetail As DataTable = GetQuizDetail()
        If dtDetail.Rows.Count > 0 Then
            Dim tempTestsetName As String = dtDetail.Rows(0)("Testset_Name").ToString()
            If (InStr(tempTestsetName, "DA_") > -1) Then
                tempTestsetName = tempTestsetName.Replace("DA_", "")
                tempTestsetName = tempTestsetName & "_" & Convert.ToDateTime(dtDetail.Rows(0)("StartTime")).ToString("dd/MM/yy")
            End If

            lblTestsetName.Text = tempTestsetName
            lblScore.Text = dtDetail.Rows(0)("Score").ToString()
            lblSumScore.Text = dtDetail.Rows(0)("FullScore").ToString()
            LastQuestionNo = dtDetail.Rows(0)("QuestionAmount")

            Me.StudentId = dtDetail.Rows(0)("User_Id").ToString()
        End If
    End Sub

    Private Function GetQuizDetail() As DataTable
        Dim sql As New StringBuilder
        sql.Append(" SELECT t.TestSet_Name,CONVERT(int,q.FullScore) AS FullScore,CONVERT(int,SUM(qs.Score)) AS Score,COUNT(qq.Question_Id) AS QuestionAmount ,q.User_Id,q.StartTime ")
        sql.Append(" FROM tblQuiz q INNER JOIN tblTestSet t ON q.TestSet_Id = t.TestSet_Id ")
        sql.Append(" INNER JOIN tblQuizQuestion qq ON qq.Quiz_Id = q.Quiz_Id  LEFT JOIN tblQuizScore qs ON qq.Quiz_Id = qs.Quiz_Id AND qq.Question_Id = qs.Question_Id ")
        sql.Append(" WHERE  q.Quiz_Id = '" & Me.QuizId & "' ")
        sql.Append(" GROUP BY t.TestSet_Name,q.FullScore,q.User_Id,q.StartTime;")
        Return db.getdata(sql.ToString())
    End Function

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim title As String = Request.Form("dropdown")
        If title = 1 Then
            title = "คำถาม/คำตอบ ไม่ชัดเจน"
        ElseIf title = 2 Then
            title = "คำอธิบายคำถาม/คำอธิบายคำตอบ ไม่ชัดเจน"
        ElseIf title = 3 Then
            title = "เฉลยผิด"
        ElseIf title = 4 Then
            title = "สับสน ไม่เข้าใจว่าใช้งานยังไง"
        ElseIf title = 5 Then
            title = "ปัญหาอื่นๆ"
        Else
            title = "ไม่ระบุหัวข้อ"
        End If

        Dim QuestionId As String = conDB.CleanString(Request.Form("txtHidden"))

        Dim dtQuestionDetail As DataTable
        dtQuestionDetail = GetQuestionDetail(QuestionId)

        Dim SubjectName As String = dtQuestionDetail.Rows(0)("GroupSubject_ShortName")
        Dim QCategory As String = dtQuestionDetail.Rows(0)("QCategory_Name")
        Dim Qset As String = dtQuestionDetail.Rows(0)("QSet_Name")
        Dim QNo As String = dtQuestionDetail.Rows(0)("Question_No")
        Dim QId As String = dtQuestionDetail.Rows(0)("Question_Id").ToString

        Dim LevelShortName As String = ""
        If dtQuestionDetail.Rows.Count > 1 Then
            For Each i In dtQuestionDetail.Rows
                LevelShortName &= i("Level_ShortName") & ","
            Next

            LevelShortName = LevelShortName.Substring(0, dtQuestionDetail.Rows.Count - 2)
        Else
            LevelShortName = dtQuestionDetail.Rows(0)("Level_ShortName").ToString
        End If



        Dim QuestionDetail As String
        QuestionDetail = "แจ้งปัญหาข้อสอบ<br>วิชา : " & SubjectName & " ชั้น : " & LevelShortName & "<br>"
        QuestionDetail &= "บท : " & QCategory & " คำสั่ง : " & Qset & "<br>"
        QuestionDetail &= "ข้อที่ : " & QNo & " (" & QId & ")<br>"
        QuestionDetail &= "รหัสควิซ : " & Request.QueryString("quizid").ToString() & "<br><br>"

        Dim description As String = conDB.CleanString(Request.Form("descript"))

        If description <> "" Then

            'send email
            Dim strBody As String = QuestionDetail & "หัวข้อ :: " & title & "<br>รายละเอียด : " & description
            SaveReportProblem(QuestionId, strBody, StudentId)

            Dim clsTS As New ClsTestSet("")
            If clsTS.sendEmailToAdmin("แจ้งปัญหาข้อสอบ MaxOnet", strBody) Then
                DisplayAlert("แจ้งปัญหาเรียบร้อยค่ะ")
            Else
                DisplayAlert("ติดปัญหาไม่สามารถแจ้งปัญหาได้ค่ะ")
            End If
        Else
            DisplayAlert("กรุณากรอกรายละเอียดด้วยนะคะ")
        End If

    End Sub

    Protected Overridable Sub DisplayAlert(message As String)
        ClientScript.RegisterStartupScript(Me.[GetType](), Guid.NewGuid().ToString(), String.Format("alert('{0}');", message.Replace("'", "\'").Replace(vbLf, "\n").Replace(vbCr, "\r")), True)
    End Sub

    Public Function GetQuestionDetail(QuestionId As String) As DataTable
        Dim sql As String = ""
        sql = "select q.Question_No,Question_Id,qs.QSet_Name,qc.QCategory_Name,g.GroupSubject_ShortName,l.Level_ShortName
                from tblquestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id
                inner join tblBook b on qc.Book_Id = b.BookGroup_Id
                inner join tblGroupSubject g on b.GroupSubject_Id = g.GroupSubject_Id
                inner join tblLevel L on b.Level_Id = L.Level_Id where Question_Id = '" & QuestionId & "'"

        Dim dtResult As DataTable
        dtResult = conDB.getdata(sql)

        Return dtResult
    End Function

    Public Function SaveReportProblem(QuestionId As String, ProblemDetail As String, Reporter As String)
        Try
            Dim sql As String = ""
            sql = "insert into tblReporterProblemQuestion(RPQId,QuestionId,Annotation,ReporterId,ReportTime,IsActive)
                values(newid(),'" & QuestionId & "','" & ProblemDetail & "','" & Reporter & "',dbo.GetThaiDate(),1)"
            conDB.Execute(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class

Public Enum EnumShowMode
    AllQuestion = 0
    RightQuestion = 1
    WrongQuestion = 2
    SkipQuestion = 3
End Enum