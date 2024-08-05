Imports Highchart.Core
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Web

Public Class ReviewMostWrongAnswer

    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Shared Index As Integer = 0

    Dim db As New ClassConnectSql

    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim clsPDf As New ClsPDF(New ClassConnectSql)

    Protected IsFullScore As Boolean = False


    Public Property SizeWidthForDivs As Integer
        Get
            Return ViewState("_SizeWidthForDivs")
        End Get
        Set(ByVal value As Integer)
            ViewState("_SizeWidthForDivs") = value
        End Set
    End Property
    Public Property SideMenuDiv As Integer
        Get
            Return ViewState("_SideMenuDiv")
        End Get
        Set(ByVal value As Integer)
            ViewState("_SideMenuDiv") = value
        End Set
    End Property
    Public Property WidthDivExamAmount As Integer
        Get
            Return ViewState("_WidthDivExamAmount")
        End Get
        Set(ByVal value As Integer)
            ViewState("_WidthDivExamAmount") = value
        End Set
    End Property
    Public Property SizeForSetPicture As Integer
        Get
            Return ViewState("_SizeForSetPicture")
        End Get
        Set(ByVal value As Integer)
            ViewState("_SizeForSetPicture") = value
        End Set
    End Property
    Public Property IndexRunChoice As Integer
        Get
            Return ViewState("_IndexRunChoice")

        End Get
        Set(ByVal value As Integer)
            ViewState("_IndexRunChoice") = value
        End Set
    End Property
    Public Property SizeChart As Double
        Get
            Return ViewState("_SizeChart")
        End Get
        Set(ByVal value As Double)
            ViewState("_SizeChart") = value
        End Set
    End Property
    Public Property MarginLeft As Integer
        Get
            Return ViewState("_MarginLeft")
        End Get
        Set(ByVal value As Integer)
            ViewState("_MarginLeft") = value
        End Set
    End Property
    Public Property ViewStateListQId() As List(Of ListQuestionId2)
        Get
            If ViewState("ListQId") Is Nothing Then
                Return New List(Of ListQuestionId2)
            Else
                Return ViewState("ListQId")
            End If
        End Get
        Set(ByVal value As List(Of ListQuestionId2))
            ViewState("ListQId") = value
        End Set
    End Property
    Public Property _ExamNum As String
        Get
            Return ViewState("_ExamNum")
        End Get
        Set(ByVal value As String)
            ViewState("_ExamNum") = value
        End Set
    End Property
    Public Property _ExamAmount As String
        Get
            Return ViewState("_ExamAmount")
        End Get
        Set(ByVal value As String)
            ViewState("_ExamAmount") = value
        End Set
    End Property

    Public IE As String
    Public IsNoAnswer As Boolean
    Public Score As String
    Protected TestsetName As String

    Protected CorrectAnswerType3 As String
    Protected MyAnswerType3 As String
    Protected IsSwapAnswerType3 As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        'Session("Quiz_Id") = "516915A4-017B-4A56-89E7-ACBD15ED4D67"
        Session("Quiz_Id") = "7E11AF72-637C-4C8F-8E85-1160072AD2D8"
        Session("QuizUseTablet") = "True"
        IE = "1"
#End If

        ' default value สำหรับ type3
        CorrectAnswerType3 = ""
        MyAnswerType3 = ""
        IsSwapAnswerType3 = False

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        Else

            'Help
            ProcessHelpPanel()

            'Open Connection
            Dim connActivity As New SqlConnection
            db.OpenExclusiveConnect(connActivity)

            If Not IsPostBack Then
                ShowReviewMostWrong.Value = "1"
                _ExamAmount = ClsActivity.GetReviewExamAmount(Session("Quiz_Id"), connActivity)
                'Dim ListQId = ViewStateListQId
                'Dim dt As DataTable = ClsActivity.GetQuestion(Quiz_Id, False)
                'For i = 0 To dt.Rows.Count - 1
                '    ListQId.Add(New ListQuestionId2 With {.QuestionId = dt.Rows(i)("Question_Id").ToString, .RowNum = i + 1})
                'Next
                'ViewStateListQId = ListQId
            Else
                ShowReviewMostWrong.Value = "0"
            End If

            If Session("QuizUseTablet") = True Then
                'SizeWidthForDivs = 260 '133
                SideMenuDiv = 130
                If (_ExamAmount.Length = 1) Then
                    WidthDivExamAmount = 165
                    SizeWidthForDivs = 315
                ElseIf (_ExamAmount.Length = 2) Then
                    WidthDivExamAmount = 180
                    SizeWidthForDivs = 300
                Else
                    WidthDivExamAmount = 200
                    SizeWidthForDivs = 280
                End If
            Else
                'SizeWidthForDivs = 420 '185 '175
                SideMenuDiv = 245
                If (_ExamAmount.Length = 1) Then
                    WidthDivExamAmount = 165
                    SizeWidthForDivs = 475
                ElseIf (_ExamAmount.Length = 2) Then
                    WidthDivExamAmount = 180
                    SizeWidthForDivs = 460
                Else
                    WidthDivExamAmount = 200
                    SizeWidthForDivs = 440
                End If
            End If

            'Dim ts_name As String = ClsActivity.GetTestsetName(Session("Quiz_Id"), connActivity)
            Dim hiddenTestsetName As String = ClsActivity.GetTestsetName(Session("Quiz_Id"))

            TestsetName = System.Web.HttpUtility.JavaScriptStringEncode(hiddenTestsetName, False)
            lblTestsetName.Text = hiddenTestsetName
            lblSideText.Text = hiddenTestsetName

            If (sortQuestion.Value = 0) Then
                getMostWrongAnswer(False, connActivity)
            Else
                getMostWrongAnswer(True, connActivity)
            End If



            'Close Connection
            db.CloseExclusiveConnect(connActivity)
        End If

    End Sub

    '<summary>
    'สร้าง String Html ของคำตอบ
    '</summary>
    '<param name="Question_Id">Id ของคำถามที่ต้องการจะสร้างคำตอบ</param>
    '<param name="Qset_Id">Id ของชุดคำถามที่ต้องการจะสร้างคำตอบ</param>
    '<param name="InputConn">Connection ที่จะใช้</param>
    '<returns></returns>
    Private Function CreateTagAnswer(ByVal Question_Id As String, ByVal QSet_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim qSetType As String = ""
        Dim htmlStringAnswer As String = ""
        Dim dtAnswer As New DataTable
        'หา Type ของข้อสอบ
        qSetType = ClsActivity.GetQSetType(QSet_Id, InputConn)
        If qSetType = "1" Or qSetType = "2" Then
            htmlStringAnswer = CreateStringAnswerType1(Question_Id, QSet_Id, qSetType, InputConn)
        ElseIf qSetType = "3" Then
            If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                IsSwapAnswerType3 = True
                htmlStringAnswer = CreateStringAnswerType3_Practice(Question_Id, qSetType, InputConn)
            Else
                htmlStringAnswer = CreateStringAnswerType3(Question_Id, qSetType, InputConn)
            End If
        Else
            htmlStringAnswer = CreateStringAnswerType6(Question_Id, QSet_Id, InputConn)
        End If

        Return htmlStringAnswer.ToString
    End Function

    '<summary>
    'หาคำตอบขอข้อที่ส่งมา
    '</summary>
    '<param name="Question_Id">Id ของคำถามที่ต้องการจะหาคำตอบ</param>
    '<param name="Qset_Type">ประเภทของข้อสอบ</param>
    '<param name="InputConn">Connection ที่จะใช้</param>
    '<returns></returns>
    Private Function CreateStringAnswerType1(Question_Id As String, Qset_Id As String, Qset_Type As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim dtAnswer As DataTable
        Dim BGColour As String = ""
        Dim WrongAnswer As Boolean
        Dim tagAnswer As String = ""
        Dim PrefixAnswer() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ"}

        dtAnswer = ClsActivity.GetCorrectAnswerDetail(Question_Id, Session("Quiz_Id"), InputConn)

        Dim stuAnswer As String = ""
        stuAnswer = ClsActivity.GetAnswerForReview(Session("Quiz_Id"), Question_Id, InputConn)


        Dim ClassHtmlAnswerExpain As String
        Dim Answered As String

        Dim AnswerExpHtml As New StringBuilder()



        For i = 0 To dtAnswer.Rows.Count - 1

            WrongAnswer = False

            CheckWrongAnswer(dtAnswer.Rows(i)("Answer_Id").ToString, dtAnswer.Rows(i)("Answer_Score"), stuAnswer, BGColour, WrongAnswer)

            Dim AnswerName As String = dtAnswer.Rows(i)("Answer_Name")
            AnswerName = AnswerName.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id))

            If Qset_Type = "2" Then
                If AnswerName = "True" Then
                    AnswerName = "ถูก"
                ElseIf AnswerName = "False" Then
                    AnswerName = "ผิด"
                End If
            End If

            If i Mod 2 = 0 Then
                'Answer ฝั่งซ้าย       
                tagAnswer &= "<tr style='border-bottom: solid 1px #AFAFAF;vertical-align: top;'><td style=""" & BGColour
                tagAnswer &= "height: 50px;font-weight: bold;width:35px;position:relative;"">" & PrefixAnswer(i) & "."
                If WrongAnswer = True Then
                    tagAnswer &= "<img src='../Images/Activity/ChooseCircle_pad.png' style='display:block !important;' class='ImgCircle' />"
                End If
                tagAnswer &= "</td> <td style=""" & BGColour
                tagAnswer &= "height: 50px;width:275px;"">" & AnswerName & "</td> "
            Else
                'Answer ฝั่งขวา
                tagAnswer &= "<td style=""height: 50px;width:30px;""></td> <td style=""" & BGColour
                tagAnswer &= "height: 50px;font-weight: bold;width:35px;position:relative;"">" & PrefixAnswer(i) & "."
                If WrongAnswer = True Then
                    tagAnswer &= " <img src='../Images/Activity/ChooseCircle_pad.png' style='display:block !important;' class='ImgCircle' />"
                End If
                tagAnswer &= " </td><td style=""" & BGColour
                tagAnswer &= "height: 50px;width:275px;"">" & AnswerName & "</td></tr> "
            End If

            If dtAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value Then
                If InStr(BGColour, "#2CA505") Then
                    ClassHtmlAnswerExpain = "Correct"
                ElseIf InStr(BGColour, "#FF0B00") Then
                    ClassHtmlAnswerExpain = "InCorrect"
                Else
                    ClassHtmlAnswerExpain = "NotAnswered"
                End If
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(String.Format("<div class='{0}'>{1}", ClassHtmlAnswerExpain, Answered))
                AnswerExpHtml.Append(PrefixAnswer(i) & ". " & AnswerName)

                If dtAnswer.Rows(i)("Answer_Expain").ToString().Trim() <> "" Then
                    AnswerExpHtml.Append("<div>")
                    AnswerExpHtml.Append(dtAnswer.Rows(i)("Answer_Expain").Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id)))
                    AnswerExpHtml.Append("</div>")
                End If

                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
                Answered = ""
            End If
        Next

        AnswerExp.InnerHtml = AnswerExpHtml.ToString()


        Return tagAnswer.ToString
    End Function


    Private Function CreateStringAnswerType3_Practice(Question_Id As String, Qset_Type As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim dtAnswer As DataTable = ClsActivity.GetCorrectAnswerMatchExam(Session("Quiz_Id"), Question_Id, InputConn)
        Dim dtStudentAnswer As DataTable = ClsActivity.GetCorrectAnswerMatchExam(Session("Quiz_Id"), Question_Id, InputConn, True)


        Dim QuestionId As String
        Dim QuestionName As String
        Dim AnswerId As String
        Dim AnswerName As String
        Dim tagAnswer As String = ""
        Dim tempCorrectAnswer As String = ""
        Dim BG As String
        Dim IsDrag As String = ""

        Dim AnswerExpHtml As New StringBuilder()

        If dtStudentAnswer.Rows(0)("ResponseAmount") = 0 Then
            IsNoAnswer = True
        Else
            IsNoAnswer = False
        End If

        For i = 0 To dtStudentAnswer.Rows.Count - 1

            'Render 
            QuestionId = dtStudentAnswer.Rows(i)("Question_Id").ToString()
            QuestionName = dtStudentAnswer.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", clsPDf.GenFilePath(dtAnswer.Rows(0)("QSet_Id").ToString))
            AnswerId = dtStudentAnswer.Rows(i)("Answer_Id").ToString()
            AnswerName = dtStudentAnswer.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", clsPDf.GenFilePath(dtAnswer.Rows(0)("QSet_Id").ToString))

            'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง           
            BG = If(dtStudentAnswer.Rows(i)("Answer_Id") = dtAnswer.Rows(i)("CorrectAnswer_Id"), "background-color:#2CA505;color:white;", "background-color:#FF0000;")
            If (BG = "background-color:#FF0000;") Then
                BG = If(dtStudentAnswer.Rows(i)("Answer_Name") = dtAnswer.Rows(i)("Answer_Name"), "background-color:#2CA505;color:white;", "background-color:#FF0000;")
            End If

            Dim CorrectAnswerId As String = dtAnswer.Rows(i)("CorrectAnswer_Id").ToString()
            Dim CorrectAnswerName As String = dtAnswer.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", clsPDf.GenFilePath(dtAnswer.Rows(0)("QSet_Id").ToString))

            'render tag Correct
            tempCorrectAnswer &= "<tr  style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>"
            tempCorrectAnswer &= "<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" "
            tempCorrectAnswer &= "style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span id=""" & CorrectAnswerId & """ style=""background-color:#1EEE1E;"" >" & CorrectAnswerName & "</span></td></tr>"

            'Render(tag)
            tagAnswer &= "<tr class=""3"" style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>"
            tagAnswer &= "<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" "
            tagAnswer &= "style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span id=""" & AnswerId & """ class=""" & IsDrag & """ style=""" & BG & """ >" & AnswerName & "</span></td></tr>"

            If dtAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value Then
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(String.Format("<div class='Correct'>{0}  คู่กับ  {1}", QuestionName, CorrectAnswerName))
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(dtAnswer.Rows(i)("Answer_Expain")) '.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id)))
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
            End If

        Next

        AnswerExp.InnerHtml = AnswerExpHtml.ToString() 'สำหรับโชว์อธิบายคำตอบ

        CorrectAnswerType3 = tempCorrectAnswer 'ไว้สลับคำตอบถูก 
        MyAnswerType3 = tagAnswer 'ไว้สลับ user ตอบ

        Return tagAnswer

    End Function



    Private Function CreateStringAnswerType3(Question_Id As String, Qset_Type As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim QuizId As String = Session("Quiz_Id").ToString()

        Dim dtAnswer As DataTable = ClsActivity.GetCorrectAnswerMatchExam(QuizId, Question_Id, InputConn)
        Dim QuestionId As String
        Dim QuestionName As String
        Dim AnswerId As String
        Dim AnswerName As String
        Dim tagAnswer As String = ""
        Dim BG As String
        Dim IsDrag As String = ""

        Dim AnswerExpHtml As New StringBuilder()

        If dtAnswer.Rows.Count = 0 Then
            Dim ExamNum As String = db.ExecuteScalar(String.Format(" SELECT QQ_No FROM tblQuizQuestion WHERE Quiz_Id = '{0}' AND Question_Id = '{1}';", QuizId, Question_Id))
            dtAnswer = ClsActivity.GetTempQuestionType3(QuizId, ExamNum, InputConn)
        End If


        For i = 0 To dtAnswer.Rows.Count - 1
            'Render 
            QuestionId = dtAnswer.Rows(i)("Question_Id").ToString()
            QuestionName = dtAnswer.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", clsPDf.GenFilePath(dtAnswer.Rows(0)("QSet_Id").ToString))
            AnswerId = dtAnswer.Rows(i)("Answer_Id").ToString()
            AnswerName = dtAnswer.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", clsPDf.GenFilePath(dtAnswer.Rows(0)("QSet_Id").ToString))

            'If Not ((IsTeacher = True) And (IsPracticeMode = False)) Then
            'ถ้า Question_Id ที่ตอบ ตรงกับ Question_Id ที่เรียงถูก แสดงว่าตอบถูกให้ป้ายสีเขียว ถ้าผิดป้ายสีแดง
            If dtAnswer.Rows(i)("Answer_Id") = dtAnswer.Rows(i)("Answer_Id") Then
                BG = "background-color:#2CA505;color:white;"
            Else
                BG = "background-color:#FF0000;color:white;"
            End If

            Dim CorrectAnswerId As String = dtAnswer.Rows(i)("Answer_Id").ToString()
            Dim CorrectAnswerName As String = dtAnswer.Rows(i)("Answer_Name").ToString()

            'Render(tag)
            tagAnswer &= "<tr class=""3"" style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;"">" & QuestionName & "</td>"
            tagAnswer &= "<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" "
            tagAnswer &= "style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><span id=""" & AnswerId & """ class=""" & IsDrag & """ style=""" & BG & """ >" & AnswerName & "</span></td></tr>"

            If dtAnswer.Rows(i)("Answer_Expain") IsNot DBNull.Value Then
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(String.Format("<div class='Correct'>{0}  คู่กับ  {1}", QuestionName, CorrectAnswerName))
                AnswerExpHtml.Append("<div>")
                AnswerExpHtml.Append(dtAnswer.Rows(i)("Answer_Expain")) '.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id)))
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
                AnswerExpHtml.Append("</div>")
            End If
        Next

        AnswerExp.InnerHtml = AnswerExpHtml.ToString() 'สำหรับโชว์อธิบายคำตอบ

        Return tagAnswer

    End Function
    Private Function CreateStringAnswerType6(Question_Id As String, Qset_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim dtAnswer As DataTable
        dtAnswer = ClsActivity.GetCorrectAnswerSortExam(Qset_Id, InputConn)
        Dim tagAnswer As String

        For i = 0 To dtAnswer.Rows.Count - 1

            Dim Question As String = dtAnswer.Rows(i)("Question_Name")
            Question = Question.Replace("___MODULE_URL___", clsPDf.GenFilePath(Qset_Id))

            tagAnswer &= "<tr style='border-bottom: solid 1px #AFAFAF;vertical-align: top;'><td style=""background-color:#1EEE1E;height:50px;font-weight: bold;width:300px; padding-left:10px;padding-top: 9px;"">ลำดับที่ "
            tagAnswer &= i + 1 & "</td><td style=""height: 50px;width:1400px;padding-left:10px;padding-top: 9px;"">" & Question & "</td>"

        Next
        Return tagAnswer.ToString

    End Function

    Private Sub CheckWrongAnswer(AnswerId As String, AnswerScore As String, PlayerAnswer As String, ByRef BGColour As String, ByRef WrongAnswer As Boolean)

        If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then

            If (AnswerScore = "1") Then
                'BGColour = "background-color:#1EEE1E;"
                BGColour = "background-color:#2CA505;color:white;"
                If AnswerId = PlayerAnswer Then
                    WrongAnswer = True
                Else
                    WrongAnswer = False
                End If

            ElseIf (AnswerScore = "0" And AnswerId = PlayerAnswer) Then
                BGColour = "background-color:#FF0000;"
                WrongAnswer = True
            Else
                BGColour = String.Empty
                WrongAnswer = False
            End If

            If PlayerAnswer = "" Then
                IsNoAnswer = True
                WrongAnswer = False
            Else
                IsNoAnswer = False
            End If
        Else
            If AnswerScore = "1" Then
                'BGColour = "background-color:#1EEE1E;"
                BGColour = "background-color:#2CA505;color:white;"
                WrongAnswer = True

            Else
                BGColour = String.Empty
            End If
        End If
    End Sub


    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNextTop.Click, btnNextSide.Click 'btnNext.Click,
        '  Dim Quiz_Id As String = Session("Quiz_Id")
        If Not _ExamNum + 1 > _ExamAmount Then

            'Open Connection
            Dim connActivity As New SqlConnection
            db.OpenExclusiveConnect(connActivity)

            Dim ListQuestion = ViewStateListQId
            Dim r As ListQuestionId2
            'ถ้าเป็นโหมดแบบปกติจะเข้า If
            If (sortQuestion.Value = 0) Then
                r = (From q In ListQuestion Where q.IndexForChoice = _ExamNum + 1).SingleOrDefault
                _ExamNum = _ExamNum + 1
            Else 'ถ้าเป็นโหมดแบบเรียงข้อผิดขึ้นก่อนจะเข้า Else
                r = (From q In ListQuestion Where q.RowNum = _ExamNum).SingleOrDefault
                Dim currentExamNumOfMostWrongMode As Integer = r.IndexForChoice + 1
                r = (From q In ListQuestion Where q.IndexForChoice = currentExamNumOfMostWrongMode).SingleOrDefault
                _ExamNum = r.RowNum
            End If

            Dim dtQuestion As DataTable = ClsActivity.GetQuestionDetail(r.QuestionId, connActivity)
            Dim Qset_Type As String = ClsActivity.GetQSetType(dtQuestion.Rows(0)("QSet_Id").ToString(), connActivity) 'เช็ค Type ข้อสอบก่อน
            Dim Quest As String
            If Qset_Type = "6" Or Qset_Type = "3" Then
                Quest = dtQuestion.Rows(0)("QSet_Name")
                'Quest = cls.CleanSetNameText(RenderQuestion)
            Else
                Quest = dtQuestion.Rows(0)("Question_Name")
            End If

            Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
            Dim IndexChoice As String = r.IndexForChoice
            'QuestionName.InnerHtml = Quest
            'Question_No.InnerText = IndexChoice

            'QuestionTd.InnerHtml = r.RowNum + ". " + Quest
            'QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & r.RowNum & ". " & "</div><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"
            QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & r.RowNum & ". " & "</div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"

            If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value And dtQuestion.Rows(0)("Question_Expain").ToString() <> "" Then
                QuestionTd.InnerHtml = String.Format("{0}<div id=""QuestionExp"">{1}</div>", QuestionTd.InnerHtml, dtQuestion.Rows(0)("Question_Expain").Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)))
            End If


            IndexRunChoice += 1

            Dim taganswer As String = CreateTagAnswer(r.QuestionId.ToString, dtQuestion.Rows(0)("QSet_Id").ToString(), connActivity)

            'Dim tagAnswer As String
            'tagAnswer = ClsActivity.RenderAnswer(Session("UserId").ToString, "2", Session("Quiz_Id").ToString, _ExamNum + 1, False, Session("Selfpace"))
            'Answer.InnerHtml = taganswer
            AnswerTbl.InnerHtml = taganswer

            HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = _ExamNum
            lblNoExam.Text = r.RowNum & " / " & _ExamAmount
            lblNoExamSide.Text = r.RowNum & " / " & _ExamAmount
            ChagePositionCartoon(r.RowNum, 1, r.QuestionId, connActivity)

            'Close Connection
            db.CloseExclusiveConnect(connActivity)

        End If
    End Sub

    Protected Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrvTop.Click, btnPrvSide.Click 'btnPrevious.Click,

        ' Dim Quiz_Id As String = Session("Quiz_Id")
        If _ExamNum - 1 > 0 Then
            'If _AnswerState = "0" Then

            'Open Connection
            Dim connActivity As New SqlConnection
            db.OpenExclusiveConnect(connActivity)

            Dim ListQuestion = ViewStateListQId
            Dim r As ListQuestionId2 '= (From q In ListQuestion Where q.IndexForChoice = _ExamNum - 1).SingleOrDefault
            'ถ้าเป็นโหมดแบบปกติจะเข้า If
            If (sortQuestion.Value = 0) Then
                r = (From q In ListQuestion Where q.IndexForChoice = _ExamNum - 1).SingleOrDefault
                _ExamNum = _ExamNum - 1
            Else 'ถ้าเป็นโหมดแบบเรียงข้อผิดขึ้นก่อนจะเข้า Else
                r = (From q In ListQuestion Where q.RowNum = _ExamNum).SingleOrDefault
                Dim currentExamNumOfMostWrongMode As Integer = r.IndexForChoice - 1
                r = (From q In ListQuestion Where q.IndexForChoice = currentExamNumOfMostWrongMode).SingleOrDefault
                _ExamNum = r.RowNum
            End If
            Dim dtQuestion As DataTable = ClsActivity.GetQuestionDetail(r.QuestionId)
            Dim Qset_Type As String = ClsActivity.GetQSetType(dtQuestion.Rows(0)("QSet_Id").ToString(), connActivity) 'เช็ค Type ข้อสอบก่อน
            Dim Quest As String
            If Qset_Type = "6" Or Qset_Type = "3" Then
                Quest = dtQuestion.Rows(0)("QSet_Name")
                'Quest = cls.CleanSetNameText(RenderQuestion)
            Else
                Quest = dtQuestion.Rows(0)("Question_Name")
            End If
            Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
            Dim IndexChoice As String = r.IndexForChoice
            'Question_No.InnerText = IndexChoice
            'QuestionName.InnerHtml = Quest
            'QuestionTd.InnerHtml = r.RowNum + ". " + Quest
            'QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & r.RowNum & ". " & "</div><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"
            QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & r.RowNum & ". " & "</div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"


            If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value And dtQuestion.Rows(0)("Question_Expain").ToString() <> "" Then
                QuestionTd.InnerHtml = String.Format("{0}<div id=""QuestionExp"">{1}</div>", QuestionTd.InnerHtml, dtQuestion.Rows(0)("Question_Expain").Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)))
            End If

            Dim taganswer As String = CreateTagAnswer(r.QuestionId.ToString, dtQuestion.Rows(0)("QSet_Id").ToString, connActivity)
            'Answer.InnerHtml = taganswer
            AnswerTbl.InnerHtml = taganswer

            IndexRunChoice -= 1

            '_ExamNum = _ExamNum - 1
            HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = _ExamNum
            lblNoExam.Text = r.RowNum & " / " & _ExamAmount
            lblNoExamSide.Text = r.RowNum & " / " & _ExamAmount
            ChagePositionCartoon(r.RowNum, 0, r.QuestionId, connActivity)

            'Close Connection
            db.CloseExclusiveConnect(connActivity)

        End If
    End Sub

    Private Sub getMostWrongAnswer(ByVal OrderByWrong As Boolean, Optional ByRef InputConn As SqlConnection = Nothing) 'เรียงตามเลขข้อ
        'Dim db As New ClassConnectSql()

        Dim dt As DataTable
        Dim dtQuestionId As New DataTable
        Dim idQuestion As String = "", noQuestion As String = "", QQ_No As String = "", QSetId As String
        Dim dtMostWrongAnswerDesc As New DataTable
        dtMostWrongAnswerDesc.Columns.Add("Question_Id", GetType(String))
        dtMostWrongAnswerDesc.Columns.Add("QQ_No", GetType(String))
        dtMostWrongAnswerDesc.Columns.Add("sumWrongAnswer", GetType(Integer))
        dtMostWrongAnswerDesc.Columns.Add("sumStudent", GetType(Integer))

        Dim sql As String = "SELECT QQ.*,Q.QSet_Id,row_Number() over(order by QQ_No) as RowNumber FROM tblQuizQuestion QQ, tblQuestion Q WHERE Quiz_Id = '" & Session("Quiz_Id") & "' "
        sql &= " and QQ.Question_Id  = Q.Question_Id ORDER BY QQ_No;"

        dtQuestionId = db.getdata(sql, , InputConn)


        Dim sumWrongAnswer2 As String = ""
        Dim sumStudent2 As String = ""
        Dim CheckQset As String
        Dim CheckFirstOrderQuestion As Boolean = False 'เพื่อเช็คว่าข้อสอบแบบเรียงลำดับจะต้องมี QQ_No ของ Question_Id อันแรกอันเดียวเท่านั้น เปลี่ยนเป็น True เมื่อได้เลข QQ_No แล้ว 

        Dim dtQset As DataTable = ClsActivity.GetReviewQset(Session("Quiz_ID").ToString, InputConn)
        Dim RowNum As Integer = 1
        For Each a In dtQset.Rows
            If a("QSet_type").ToString = "6" Then
                CheckQset = a("QSet_id").ToString
                For Each b As DataRow In dtQuestionId.Rows
                    If b("QSet_id").ToString = CheckQset Then
                        If CheckFirstOrderQuestion = False Then
                            idQuestion = "," & b("Question_Id").ToString
                            QQ_No = b("RowNumber").ToString
                            CheckFirstOrderQuestion = True
                        Else
                            idQuestion &= "," & b("Question_Id").ToString
                        End If
                    End If
                Next
                idQuestion = Right(idQuestion, idQuestion.Length - 1)
                getSumMostWrong(idQuestion, InputConn)
                sumWrongAnswer2 = _getSumMostWrong.sumWrongAnswer.ToString
                sumStudent2 = _getSumMostWrong.sumStudent.ToString
                dtMostWrongAnswerDesc.Rows.Add(idQuestion, RowNum, sumWrongAnswer2, sumStudent2)
                CheckFirstOrderQuestion = False
                RowNum += 1
            ElseIf a("QSet_type") = 3 Then ' หน้า swap page เลือกข้อ สำหรับ type 3 ให้แสดงข้อเดียวเท่านั้น ต่อ 1 Qset
                For Each row In dtQuestionId.Rows
                    If row("QSet_id") = a("QSet_id") Then
                        idQuestion = row("Question_Id").ToString
                    End If
                Next

                getSumMostWrong(idQuestion, InputConn)
                sumWrongAnswer2 = _getSumMostWrong.sumWrongAnswer.ToString
                sumStudent2 = _getSumMostWrong.sumStudent.ToString
                dtMostWrongAnswerDesc.Rows.Add(idQuestion, RowNum, sumWrongAnswer2, sumStudent2)
                CheckFirstOrderQuestion = False
                QQ_No = RowNum
                RowNum += 1
            Else
                CheckQset = a("QSet_id").ToString
                For Each b As DataRow In dtQuestionId.Rows

                    If b("QSet_id").ToString = CheckQset Then
                        idQuestion = b("Question_Id").ToString
                        QQ_No = b("RowNumber").ToString
                        getSumMostWrong(idQuestion, InputConn)
                        sumWrongAnswer2 = _getSumMostWrong.sumWrongAnswer.ToString
                        sumStudent2 = _getSumMostWrong.sumStudent.ToString
                        dtMostWrongAnswerDesc.Rows.Add(idQuestion, RowNum, sumWrongAnswer2, sumStudent2)
                        RowNum += 1
                    End If
                Next
            End If

        Next


        'For r As Integer = 0 To dtQuestionId.Rows.Count - 1

        '    idQuestion = dtQuestionId.Rows(r)("Question_Id").ToString
        '    QQ_No = dtQuestionId.Rows(r)("QQ_No").ToString
        '    getSumMostWrong(idQuestion)
        '    sumWrongAnswer2 = _getSumMostWrong.sumWrongAnswer.ToString
        '    sumStudent2 = _getSumMostWrong.sumStudent.ToString
        '    dtMostWrongAnswerDesc.Rows.Add(idQuestion, QQ_No, sumWrongAnswer2, sumStudent2)
        'Next
        Dim dtView As New DataView(dtMostWrongAnswerDesc)
        If OrderByWrong Then
            dtView.Sort = "sumWrongAnswer DESC"
        Else
            dtView = New DataView(dtMostWrongAnswerDesc)
        End If

        dtMostWrongAnswerDesc = dtView.ToTable

        'If (Session("QuizUseTablet") = True) Then
        '    CreateChart(dtView)
        '    'ChagePositionCartoon(IndexRunChoice, 1)
        'End If


        '---------------------------------------------------------------

        Dim sumOfQuestion, pageNumber As Integer
        sumOfQuestion = dtMostWrongAnswerDesc.Rows.Count 'จำนวนทั้งหมดของข้อสอบ

        Dim defaultQuestionAmountPerPage As Integer = 15 ' จำนวนข้อสอบต่อ 1 หน้า

        If sumOfQuestion <= defaultQuestionAmountPerPage Then
            BackSlide.Style.Add("display", "none")
            NextSlide.Style.Add("display", "none")
        Else
            BackSlide.Style.Add("display", "block")
            NextSlide.Style.Add("display", "block")
        End If

        pageNumber = Math.Ceiling(sumOfQuestion / defaultQuestionAmountPerPage) 'หารเพื่อกำหนดจำนวนหน้าแสดงข้อสอบ หน้าละ 15 ข้อ        

        mainReview.InnerHtml = ""

        Dim strDiv As String = ""
        Dim divBlock As String = ""
        Dim j As Integer, i As Integer = 1

        Dim toNo As Integer = If(sumOfQuestion < defaultQuestionAmountPerPage, sumOfQuestion, defaultQuestionAmountPerPage)  'ใช้กำหนดเพื่อให้ loop แสดงหน้าละจำนวน 15 ข้อ

        'If (sumOfQuestion < 18) Then
        '    toNo = sumOfQuestion
        'End If



        If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            Dim scoreTxt As String = ClsActivity.GetScoreForPracticeFromComputer(Session("Quiz_Id").ToString)
            Dim tmpArr As Array = scoreTxt.Split("/")
            IsFullScore = (tmpArr(0).ToString().Trim() = tmpArr(1).ToString().Trim())
            Score = "ได้คะแนน " & scoreTxt & " ค่ะ"
            btnSortQuestionNormal.Visible = False
        End If

        Dim RunMode As String = ClsKNSession.RunMode
        If RunMode = "standalonenotablet" AndAlso Not (Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
            Score = "กดเลือกข้อที่ต้องการทบทวนค่ะ"
        End If

        For j = 1 To pageNumber
            'strDiv = "<div style='width: 90%; margin-left: auto; margin-right: auto;' id='divSwipe" + CStr(j) + "' runat='server'>"
            'mainReview.InnerHtml += strDiv

            'ใช้ Panel มาสร้าง block ข้อสอบ
            Dim panelPage As New Panel()
            With panelPage
                .ID = "divSwipe" + CStr(j)
                .Style.Add("width", "700px")
                .Style.Add("margin-left", "auto")
                .Style.Add("margin-right", "auto")
                .Style.Add("padding-left", "42px")
            End With


            For i = i To toNo
                idQuestion = dtMostWrongAnswerDesc.Rows(i - 1)("Question_Id").ToString
                noQuestion = dtMostWrongAnswerDesc.Rows(i - 1)("QQ_No")

                getSumMostWrong(idQuestion, InputConn) 'ไป get ค่า คนผิดและจำนวนนักเรียนในข้อนั้น

                Dim perMostWrong As Integer
                Dim sumWrongAnswer As String = _getSumMostWrong.sumWrongAnswer.ToString
                Dim sumStudent As String = _getSumMostWrong.sumStudent.ToString



                If (Session("QuizUseTablet") = True) Or Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                    If Not sumStudent = 0 Then
                        perMostWrong = (CInt(sumWrongAnswer) / CInt(sumStudent)) * 100
                        'perMostWrong = (CInt(5) / CInt(5)) * 100
                    End If
                End If
                'divBlock = "<div class='divBlockMostWrongAns' id='" + idQuestion + "'><table style='width: 100%'>"
                'divBlock += "<tr><td><span><font size='5px'>0.<b>" + noQuestion + "</b></font></span></td></tr>"
                'divBlock += "<tr><td style='text-align: center'><img src='" + getImageMostWrong(perMostWrong) + "' /></td></tr>"
                'divBlock += "<tr><td style='text-align: center'><img src='../Images/Activity/mostWrongFace/wrong.gif' width='25px' height='25px' />"
                'divBlock += "<span><b><font color='red' id='mostAns'>" + sumWrongAnswer + "</font>  /  <font color='green'>" + sumStudent + "</font></b></span></td></tr></table></div>"
                'mainReview.InnerHtml += divBlock

                'สร้างตัว block ข้อสอบ
                Dim panelBlock As New Panel()
                With panelBlock
                    .ID = "QQ_no" + noQuestion
                    .CssClass = "QQ_no"
                    .Style.Add("position", "relative")
                    .Style.Add("background-color", "#F4F7FF")
                    .Style.Add("margin", "6px")
                    .Style.Add("border", "2px solid #AFAFAF")
                    .Style.Add("width", "120px")
                    .Style.Add("height", "120px")
                    .Style.Add("float", "left")
                    .Style.Add("-webkit-border-radius", "15px")
                End With

                ' เช็คว่าเป็น quiz ที่ใช้ tablet หรือเปล่า
                If (Session("QuizUseTablet") = True) Then
                    ' สร้างเลขข้อ
                    Dim pnlNo_Question As New Panel
                    Dim lblNo_Question As New Label()
                    With lblNo_Question
                        .ID = "lblNo_Question" + noQuestion
                        .Text = "ข้อที่ " & noQuestion
                        .Style.Add("position", "relative")
                        .Style.Add("top", "5px")
                        .Style.Add("left", "5px")
                        .Style.Add("font", "bold 15px 'THSarabunNew'")
                    End With
                    pnlNo_Question.Controls.Add(lblNo_Question)

                    ' สร้างจำนวนคนผิด และ จำนวนคนที่สอบทั้งหมด
                    Dim lblNo_Wrong As New Label()
                    With lblNo_Wrong
                        .ID = "lblNo_WrongAndAllStudent" + noQuestion
                        .Text = "ผิด " & sumWrongAnswer.ToString
                        .Style.Add("position", "relative")
                        .Style.Add("bottom", "-69px")
                        .Style.Add("left", "21px")
                        .Style.Add("color", "red")
                        .Style.Add("font", "bold 12px 'THSarabunNew'")
                    End With

                    Dim lblNo_AllStudent As New Label()
                    With lblNo_AllStudent
                        .ID = "lblNo_AllStudent" + noQuestion
                        .Text = "จาก " & sumStudent
                        .Style.Add("position", "relative")
                        .Style.Add("bottom", "-69px")
                        .Style.Add("left", "30px")
                        .Style.Add("color", "green")
                        .Style.Add("font", "bold 12px 'THSarabunNew'")
                    End With

                    ' สร้าง btn image หน้าคนยิ้ม
                    Dim btnImg As New ImageButton()
                    With btnImg
                        .ID = idQuestion
                        .ImageUrl = getImageMostWrong(perMostWrong)
                        .Attributes.Add("NoQuestion", noQuestion)
                        .Style.Add("position", "relative")
                        .Style.Add("left", "28px")
                        .Style.Add("top", "-25px")
                        .Style.Add("width", "65px")
                        .CssClass = "imgQuestion"
                    End With
                    AddHandler btnImg.Click, AddressOf Me.getQuestionBySelect

                    'panelBlock.Controls.Add(lblNo_Question) ' block add label no.question
                    panelBlock.Controls.Add(pnlNo_Question)
                    panelBlock.Controls.Add(lblNo_Wrong) ' block add no.wrong
                    panelBlock.Controls.Add(lblNo_AllStudent) ' block add no.allstudent
                    panelBlock.Controls.Add(btnImg) ' block add button

                    If sumWrongAnswer = sumStudent Then 'รอทำสูตรเช็คว่า ให้นักเรียนไม่กดตอบกี่เปอเซนถึงจะให้เป็นข้อข้าม
                        'Dim panelSkip As New Panel()
                        'With panelSkip
                        '    .ID = "Skip" + noQuestion
                        '    .Style.Add("position", "absolute")
                        '    .Style.Add("background-image", "../Images/Activity/mostWrongFace/skipbadge.png")
                        '    .Style.Add("width", "120px")
                        '    .Style.Add("height", "120px")
                        '    .Style.Add("float", "left")
                        '    .Style.Add("top", "0px")
                        'End With
                        'panelBlock.Controls.Add(panelSkip) ' add page skips
                        panelBlock.Style.Add("background-image", "../Images/Activity/skip90.png")
                    End If


                Else
                    ' ถ้าเล่นโดยไม่ใช้ tablet ให้แสดงแค่เลขข้อ
                    ' สร้างปุ่มที่มีแค่เลขที่ข้อ
                    Dim btnImg As New Button()
                    With btnImg
                        .ID = idQuestion
                        .Text = "ข้อที่ " & noQuestion
                        .Attributes.Add("NoQuestion", noQuestion)
                        .Style.Add("position", "relative")
                        .Style.Add("left", "0px")
                        .Style.Add("top", "0px")
                        .Style.Add("width", "inherit")
                        .Style.Add("height", "inherit")
                        '.Style.Add("background-color", "#1EC9F4")
                        .Style.Add("font", "bold 20px 'THSarabunNew'")
                        '.Style.Add("color", "#47433F")
                        '.Style.Add("-webkit-border-radius", "40px")
                        '.Style.Add("-webkit-box-shadow", "0 1px 2px rgba(0,0,0,.2)")
                        '.Style.Add("border", "1px solid #C6E7F0")
                        .Style.Add("cursor", "pointer")
                        '.Style.Add("background", "-webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9))")
                        .Style.Add("-webkit-border-radius", "15px")
                        .Style.Add("border", "0px")
                        .CssClass = "btnQuestion"
                    End With
                    AddHandler btnImg.Click, AddressOf Me.getQuestionBySelect

                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then

                        Dim ScoreStatus As String = ClsActivity.getStatusAnswer(Session("Quiz_Id").ToString(), noQuestion)
                        If ScoreStatus = "0.00" Or ScoreStatus = "2" Then
                            btnImg.Style.Add("background-image", "../Images/Activity/reviewBadgeWrong.png")
                        Else
                            btnImg.Style.Add("background-image", "../Images/Activity/reviewBadgeRight.png")
                        End If
                    Else
                        btnImg.Style.Add("background-image", "../Images/Activity/reviewBadge.png")
                    End If

                    panelBlock.Controls.Add(btnImg) ' block add button
                End If

                panelPage.Controls.Add(panelBlock) ' page add block
            Next

            i = i 'ให้ i เท่ากับข้อล่าสุด
            If (sumOfQuestion > defaultQuestionAmountPerPage) Then
                sumOfQuestion = sumOfQuestion - defaultQuestionAmountPerPage 'ถ้าจำนวนข้อเกิน 15 ข้อ ให้หักออกไป 15 ข้อ
                'If (sumOfQuestion > 18) Then
                '    toNo = toNo + 18 ' ถ้ายังเกิน 15 อยู่ให้ทำ +15 
                'Else
                '    toNo = toNo + sumOfQuestion
                'End If

                toNo = If(sumOfQuestion > defaultQuestionAmountPerPage, toNo + defaultQuestionAmountPerPage, toNo + sumOfQuestion) 'เช็คจำนวนข้อว่าเหลือกี่ข้อ เพื่อเอาไปใช้ในการวนลูป ในหน้าถัดไป

            End If

            'strDiv = "</div>"
            'mainReview.InnerHtml += strDiv
            mainReview.Controls.Add(panelPage)
        Next

        Dim ListQId = ViewStateListQId
        ' Dim dt As DataTable = ClsActivity.GetQuestion(Quiz_Id, False)

        Dim NewdtTable As New DataTable
        NewdtTable = dtView.ToTable

        ListQId.Clear()

        For i = 0 To NewdtTable.Rows.Count - 1
            ListQId.Add(New ListQuestionId2 With {.QuestionId = NewdtTable.Rows(i)("Question_Id").ToString, .RowNum = NewdtTable.Rows(i)("QQ_No"), .IndexForChoice = i + 1})
        Next

        _ExamAmount = NewdtTable.Rows.Count


        'Question_No.InnerText = "1"

        'If Not Page.IsPostBack Then
        '    _ExamNum = 1

        'End If
        If HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") Is Nothing Then
            _ExamNum = 1
            HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = 1
        Else
            _ExamNum = HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review")
            'ShowReviewMostWrong.Value = 1
        End If

        ViewStateListQId = ListQId

        'Dim ListQuestion = ViewStateListQId
        Dim rNormal = (From q In ListQId Where q.RowNum = _ExamNum).SingleOrDefault
        Dim dtQuestion As DataTable = ClsActivity.GetQuestionDetail(rNormal.QuestionId)
        Dim Qset_Type As String = ClsActivity.GetQSetType(dtQuestion.Rows(0)("QSet_Id").ToString(), InputConn) 'เช็ค Type ข้อสอบก่อน
        Dim Quest As String
        If Qset_Type = "6" Or Qset_Type = "3" Then
            Quest = dtQuestion.Rows(0)("QSet_Name")
            'Quest = cls.CleanSetNameText(RenderQuestion)
        Else
            Quest = dtQuestion.Rows(0)("Question_Name")
        End If

        Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
        Dim IndexChoice As String = rNormal.IndexForChoice
        Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
        'QuestionName.InnerHtml = Quest
        'Question_No.InnerText = IndexChoice

        'QuestionTd.InnerHtml = rNormal.RowNum + ". " + Quest
        ' QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & rNormal.RowNum & ". " & "</div><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"
        QuestionTd.InnerHtml = "<table><tr><td style='text-align:center;vertical-align:top;'><div>" & rNormal.RowNum & ". " & "</div></td><td style='padding-left:10px;vertical-align:top;'>" & Quest & "</td></tr></table>"

        If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value And dtQuestion.Rows(0)("Question_Expain").ToString() <> "" Then
            QuestionTd.InnerHtml = String.Format("{0}<div id=""QuestionExp"">{1}</div>", QuestionTd.InnerHtml, dtQuestion.Rows(0)("Question_Expain").Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString)))
        End If

        'IndexRunChoice = 1
        ChagePositionCartoon(IndexRunChoice, 1, rNormal.QuestionId, InputConn)

        _ExamNum = rNormal.RowNum
        Dim taganswer As String = CreateTagAnswer(rNormal.QuestionId.ToString, dtQuestion.Rows(0)("QSet_Id").ToString(), InputConn)
        'Answer.InnerHtml = taganswerselect totalscore from tblquizsession
        AnswerTbl.InnerHtml = taganswer

        lblNoExam.Text = rNormal.RowNum & " / " & _ExamAmount
        lblNoExamSide.Text = rNormal.RowNum & " / " & _ExamAmount

        ViewStateListQId = ListQId
    End Sub

    Dim _getSumMostWrong

    Private Sub getSumMostWrong(ByVal question_id As String, Optional ByRef InputConn As SqlConnection = Nothing) 'หาจำนวนนักเรียนกับจำนวนคนผิด
        Dim dtQuestionId As DataTable
        Dim sql As String
        If InStr(question_id, ",") = 0 Then
            sql = "SELECT (SELECT COUNT(Student_id) FROM tblQuizScore WHERE Quiz_Id = '" & Session("Quiz_Id") & "' AND Question_Id = '" + question_id.ToString + "' ) as sumStudent,"
            sql &= "(SELECT COUNT(Student_id) FROM tblQuizScore WHERE Quiz_Id = '" & Session("Quiz_Id").ToString & "' AND Question_Id = '" + question_id.ToString + "' AND Score='0') as sumWrongAnswer"

            dtQuestionId = db.getdata(sql, , InputConn)

            If dtQuestionId.Rows.Count > 0 Then
                _getSumMostWrong = New With {.sumStudent = dtQuestionId.Rows(0)("sumStudent"), .sumWrongAnswer = dtQuestionId.Rows(0)("sumWrongAnswer")}
            Else
                _getSumMostWrong = New With {.sumStudent = "1", .sumWrongAnswer = "1"}
            End If
        Else
            Dim ArrQuestion() As String = Split(question_id, ",")
            Dim QuestionGetStudent As String = ArrQuestion(0)
            question_id = "'" & question_id & "'"
            question_id = question_id.Replace(",", "','")
            sql = "SELECT (SELECT COUNT(Student_id) FROM tblQuizScore WHERE Quiz_Id = '" & Session("Quiz_Id") & "' AND Question_Id = '" + QuestionGetStudent.ToString + "' ) as sumStudent,"
            sql &= "(select count(distinct student_Id) from tblQuizScore where Question_Id in(" & question_id & ") "
            sql &= " and quiz_ID = '" & Session("Quiz_Id").ToString & "' and Score = '0' ) as sumWrongAnswer"

            dtQuestionId = db.getdata(sql, , InputConn)

            If dtQuestionId.Rows.Count > 0 Then
                _getSumMostWrong = New With {.sumStudent = dtQuestionId.Rows(0)("sumStudent"), .sumWrongAnswer = dtQuestionId.Rows(0)("sumWrongAnswer")}
            Else
                _getSumMostWrong = New With {.sumStudent = "1", .sumWrongAnswer = "1"}
            End If
        End If
    End Sub

    Private Function getImageMostWrong(ByVal noOfWrong As String) As String

        If (noOfWrong >= 0 AndAlso noOfWrong <= 20) Then
            getImageMostWrong = "../Images/Activity/mostWrongFace/veryhappy.png" '"ดีเยี่ยม"
        ElseIf (noOfWrong >= 21 AndAlso noOfWrong <= 40) Then
            getImageMostWrong = "../Images/Activity/mostWrongFace/happy.png" '"เกือบดี"
        ElseIf (noOfWrong >= 41 AndAlso noOfWrong <= 60) Then
            getImageMostWrong = "../Images/Activity/mostWrongFace/neutral.png" 'ปานกลาง"
        ElseIf (noOfWrong >= 61 AndAlso noOfWrong <= 80) Then
            getImageMostWrong = "../Images/Activity/mostWrongFace/sad.png" 'อ่อนนิด"
        ElseIf (noOfWrong >= 81 AndAlso noOfWrong <= 100) Then
            getImageMostWrong = "../Images/Activity/mostWrongFace/verysad.png" 'อ่อนโคต
        End If

        Return getImageMostWrong
    End Function

    Protected Sub btnSortQuestionNormal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSortQuestionNormal.Click
        'sortQuestion.Value = 0
        btnSortQuestionNormal.Enabled = False
        btnSortQuestionMostWrong.Enabled = True
        getMostWrongAnswer(False)
        ShowReviewMostWrong.Value = "1"
    End Sub

    Protected Sub btnSortQuestionMostWrong_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSortQuestionMostWrong.Click
        'sortQuestion.Value = 1
        btnSortQuestionNormal.Enabled = True
        btnSortQuestionMostWrong.Enabled = False
        getMostWrongAnswer(True)
        ShowReviewMostWrong.Value = "1"
    End Sub

    Private Sub ChagePositionCartoon(ByVal CheckIndexChoice As Integer, ByVal CheckNextOrPrev As Integer, ByVal QID As String, Optional ByRef InputConn As SqlConnection = Nothing)
        If QID <> "" Then '''''''''''''''
            Dim BottomCartoon As Double = 20
            'เช็ครูป
            getSumMostWrong(QID, InputConn)
            Dim TotalWrong As Integer = _getSumMostWrong.sumWrongAnswer.ToString()
            Dim TotalStudent As Integer = _getSumMostWrong.sumStudent.ToString()
            Dim ResultChooseImg As Integer
            Try
                ResultChooseImg = (CInt(TotalWrong) / CInt(TotalStudent)) * 100
            Catch
            End Try

            Dim TotalAllrightStudent As Integer = TotalStudent - TotalWrong
            ChangeWidthImgChart(TotalAllrightStudent, TotalWrong, TotalStudent) 'Set Width ยืดรูปให้เป็นกราฟ

            'If (ResultChooseImg >= 0 AndAlso ResultChooseImg <= 20) Then
            '    imgCartoonAtChart.ImageUrl = "../Images/Activity/mostWrongFace/veryhappy.png"
            'ElseIf (ResultChooseImg >= 21 AndAlso ResultChooseImg <= 40) Then
            '    imgCartoonAtChart.ImageUrl = "../Images/Activity/mostWrongFace/happy.png"
            'ElseIf (ResultChooseImg >= 41 AndAlso ResultChooseImg <= 60) Then
            '    imgCartoonAtChart.ImageUrl = "../Images/Activity/mostWrongFace/neutral.png"
            'ElseIf (ResultChooseImg >= 61 AndAlso ResultChooseImg <= 80) Then
            '    imgCartoonAtChart.ImageUrl = "../Images/Activity/mostWrongFace/sad.png"
            'ElseIf (ResultChooseImg >= 81 AndAlso ResultChooseImg <= 100) Then
            '    imgCartoonAtChart.ImageUrl = "../Images/Activity/mostWrongFace/verysad.png"
            'End If
            '--------------------------------------------------------------------------------
            Dim SizeCartoon As Double = SizeChart + 5 'เอาขนาดแท่ง + 5 เป็นขนาดของตัวการ์ตูน
            BottomCartoon = 45 - SizeCartoon 'ระยะห่างหน้าการ์ตูนกับขอบล่าง = 72 - ขนาดตัวการ์ตูน
            Dim PostitionSet As Double = 56 + (((CheckIndexChoice * 2) - 1) * SizeChart) - (SizeCartoon / 2) 'ระยะการเลื่อน = 48 + (((จำนวนข้อ * 2) - 1) * ขนาดแท่ง) - ครึ่งนึงของขนาดตัวการ์ตูน
            'imgCartoonAtChart.Style.Add("margin-left", PostitionSet & "px")
            'imgCartoonAtChart.Style.Add("width", SizeCartoon & "px")
            'imgCartoonAtChart.Style.Add("bottom", BottomCartoon & "px")
        End If
    End Sub

    Private Sub ChangeWidthImgChart(ByVal TotalAllrightStudent As Integer, ByVal TotalWrong As Integer, ByVal TotalStudent As Integer)
        lblChart.Text = "ถูก " & TotalAllrightStudent & " ผิด " & TotalWrong

        lblSideChartCorrect.Text = "ถูก " & TotalAllrightStudent
        lblSideChartWrong.Text = "ผิด " & TotalWrong

        Dim CalCulateWidth As Integer = 62
        Dim CalCulateHeight As Integer = 45
        Dim MaximumWidthPx As Integer = 125
        Dim MaximunHeightpx As Integer = 50
        If Session("QuizUseTablet") = True Then
            MaximumWidthPx = 119 '105
            MaximunHeightpx = 70
        Else
            MaximumWidthPx = 155 '145
            MaximunHeightpx = 70
        End If

        If TotalAllrightStudent > TotalWrong Then
            ImgGreen.Style.Add("width", MaximumWidthPx & "px")
            CalCulateWidth = (TotalWrong / TotalAllrightStudent) * MaximumWidthPx
            ImgRed.Style.Add("width", CalCulateWidth & "px")

            SideGreen.Style.Add("height", MaximunHeightpx & "px")
            CalCulateHeight = (TotalWrong / TotalAllrightStudent) * MaximunHeightpx
            SideRed.Style.Add("height", CalCulateHeight & "px")
        ElseIf TotalAllrightStudent = 0 AndAlso TotalWrong = 0 Then
            ImgGreen.Style.Add("width", "0px")
            ImgRed.Style.Add("width", "0px")

            SideGreen.Style.Add("height", "0px")
            SideRed.Style.Add("height", "0px")
        ElseIf TotalAllrightStudent = TotalWrong Then
            'ImgGreen.Style.Add("width", (MaximumWidthPx / 2) & "px")
            'ImgRed.Style.Add("width", (MaximumWidthPx / 2) & "px")
            ImgGreen.Style.Add("width", (MaximumWidthPx) & "px")
            ImgRed.Style.Add("width", (MaximumWidthPx) & "px")

            'SideGreen.Style.Add("height", (MaximunHeightpx / 2) & "px")
            'SideRed.Style.Add("height", (MaximunHeightpx / 2) & "px")
            SideGreen.Style.Add("height", (MaximunHeightpx) & "px")
            SideRed.Style.Add("height", (MaximunHeightpx) & "px")
        Else
            ImgRed.Style.Add("width", MaximumWidthPx & "px")
            CalCulateWidth = (TotalAllrightStudent / TotalWrong) * MaximumWidthPx
            ImgGreen.Style.Add("width", CalCulateWidth & "px")

            SideRed.Style.Add("height", MaximunHeightpx & "px")
            CalCulateHeight = (TotalAllrightStudent / TotalWrong) * MaximunHeightpx
            SideGreen.Style.Add("height", CalCulateHeight & "px")
        End If
    End Sub

    Private Sub CreateChart(ByVal dtData As DataView)

        Dim dtNew As New DataTable
        dtNew = dtData.ToTable

        SizeChart = 533 / (dtNew.Rows.Count * 2)  'ขนาดแท่ง = 661 / จำนวนข้อ * 2


        'CartoonChart.YAxis.Clear()
        Dim YItem As New YAxisItem
        Dim titleY As New Highchart.Core.Title("คนตอบผิด")
        YItem.title = titleY

        ' CartoonChart.YAxis.Add(YItem)


        ' CartoonChart.Series.Clear()
        Dim series As New Highchart.Core.SerieCollection
        Dim serie1 As New Highchart.Core.Data.Chart.Serie
        Dim SeriesData(dtNew.Rows.Count - 1) As Object

        Dim CategoriesX(dtNew.Rows.Count - 1) As Object


        For a = 0 To dtNew.Rows.Count - 1
            Dim EachWrongAnswer As Double = dtNew.Rows(a)("sumWrongAnswer")
            SeriesData(a) = EachWrongAnswer
            Dim QQNo As Integer = dtNew.Rows(a)("QQ_No")
            CategoriesX(a) = QQNo
        Next

        'CartoonChart.XAxis.Clear()
        Dim Xitem As New XAxisItem
        Dim titleX As New Highchart.Core.Title("ข้อที่")
        'titleX.style.color = "black"
        'Xitem.title.style.color = "Black"
        Xitem.categories = CategoriesX

        Xitem.title = titleX
        '  CartoonChart.XAxis.Add(Xitem)
        '


        serie1.data = SeriesData
        serie1.type = RenderType.column
        serie1.name = "ทบทวนข้อที่ผิดเยอะ"
        series.Add(serie1)

        'CartoonChart.BackColor = System.Drawing.Color.Transparent
        'CartoonChart.Theme = "dark-blue"
        'CartoonChart.Theme = "pink-floral"


        ' CartoonChart.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)
        ' CartoonChart.Legend.enabled = False

        ' CartoonChart.Tooltip = New ToolTip("'<b>' + 'ทบทวนข้อที่ตอบผิด' + '<b>' + '<br />' + 'ข้อที่ ' + '<b>' + this.x + '</b>' + ' ผิด ' + '<b>' + this.y + '</b>' + ' คน'")




        '  CartoonChart.DataSource = series
        '   CartoonChart.DataBind()

        'CartoonChart.Title.style.color



    End Sub

    Protected Sub getQuestionBySelect(ByVal sender As Object, ByVal e As EventArgs)

        If TypeOf sender Is Button Then
            Dim btn As Button = sender
            HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = btn.Attributes("NoQuestion")
        ElseIf TypeOf sender Is ImageButton Then
            Dim btn As ImageButton = sender
            HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = btn.Attributes("NoQuestion")
        End If

        Dim connActivity As New SqlConnection
        db.OpenExclusiveConnect(connActivity)

        If (sortQuestion.Value = 0) Then
            getMostWrongAnswer(False, connActivity)
        Else
            getMostWrongAnswer(True, connActivity)
        End If

        db.CloseExclusiveConnect(connActivity)
        'Dim newBtn As ImageButton, newB As New Button()
        'Dim question_id As String
        'Try
        '    newBtn = sender
        '    question_id = newBtn.ID
        'Catch
        '    newB = sender
        '    question_id = newB.ID
        'End Try
        ''Dim question_id As String = newBtn.ID ' ค่าจากการกด button


        'Dim Quiz_Id As String = Session("Quiz_Id")
        'Dim ListQuestion = ViewStateListQId

        'Dim r = (From q In ListQuestion)
        'Dim noQuestion As String

        'For Each i In r
        '    If (i.QuestionId = question_id) Then
        '        noQuestion = i.IndexForChoice.ToString
        '        _ExamNum = noQuestion
        '    End If
        'Next
        'HttpContext.Current.Application(Session("Quiz_Id").ToString() & "_Review") = _ExamNum

        ''Open Connection
        'Dim connActivity As New SqlConnection
        'UseCls.OpenExclusiveConnect(connActivity)

        'Dim dtQuestion As DataTable = ClsActivity.GetQuestionDetail(question_id, connActivity)
        'Dim Quest As String = dtQuestion.Rows(0)("Question_Name")
        'Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))

        ''Question_No.InnerText = noQuestion
        ''QuestionName.InnerHtml = Quest
        'QuestionTd.InnerHtml = noQuestion + ". " + Quest

        'Dim taganswer As String = GetAnswerDetails(question_id, dtQuestion.Rows(0)("QSet_Id").ToString, connActivity)
        ''Answer.InnerHtml = taganswer
        'AnswerTbl.InnerHtml = taganswer
        'ChagePositionCartoon(_ExamNum, 1, question_id, connActivity)

        'lblNoExam.Text = _ExamNum & " / " & _ExamAmount
        'lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

        ''Close Connection
        'UseCls.CloseExclusiveConnect(connActivity)
    End Sub

    'Sub เกี่ยวกับ Help ในแต่ละหน้าที่ใช้ Masterpage นี้
    Private Sub ProcessHelpPanel()
        If ClsKNSession.RunMode <> "" Then
            Dim UrlPage As String = HttpContext.Current.Request.Url.AbsolutePath
            Dim appnamepath As String = HttpContext.Current.Request.ApplicationPath.ToLower()
            If appnamepath.Trim() = "/" Then
                appnamepath = ""
            Else
                UrlPage = UrlPage.ToLower().Replace(appnamepath, "")
            End If
            UrlPage = UrlPage.Substring(1, UrlPage.Length - 6)
            Dim SpliteUrl = UrlPage.Split("/")

            If SpliteUrl.Count > 0 Then
                Dim FolderName As String = SpliteUrl(0)
                Dim PageName As String = SpliteUrl(1)
                Dim StrCheckImage As String = ""
                'If RunMode.ToLower() = "standalonenotablet" Then
                '    StrCheckImage = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/HowTo/Helpimg/" & RunMode & "/" & FolderName & "_" & PageName & "00.png")
                'Else
                StrCheckImage = HttpContext.Current.Server.MapPath("../HowTo/Helpimg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & "00.png")
                'End If
                If System.IO.File.Exists(StrCheckImage) = False Then
                    Help.Style.Add("display", "none")
                Else
                    Dim StrScript As String
                    If HttpContext.Current.Application("DefaultUserId") = Session("UserId").ToString() Then
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=" & FolderName & "FromLogin&PageName=" & PageName & "','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                    Else
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=" & FolderName & "&PageName=" & PageName & "','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                    End If

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)
                End If
            End If
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function GetCurrentExamnum(ByVal Examnum As Integer) As String
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim CurrentExamnum As Integer = HttpContext.Current.Application(QuizId & "_Review")
        If Examnum = CurrentExamnum Then
            Return ""
        Else
            Return "Reload"
        End If
    End Function

    'Private Sub sortQuestionMostWrong() 'เรียงตามคนผิดมาก
    '    Dim db As New ClassConnectSql()
    '    Dim dt, dtMostWrongAnswerDesc As New DataTable
    '    Dim sql As String = "SELECT * FROM tblQuizQuestion WHERE Quiz_Id = '" & Session("Quiz_Id") & "' ORDER BY QQ_No ;"
    '    Dim idQuestion As String = "", noQuestion As String = "", QQ_No As String = ""
    '    dtMostWrongAnswerDesc.Columns.Add("Question_Id", GetType(String))
    '    dtMostWrongAnswerDesc.Columns.Add("QQ_No", GetType(String))
    '    dtMostWrongAnswerDesc.Columns.Add("sumWrongAnswer", GetType(Integer))
    '    dtMostWrongAnswerDesc.Columns.Add("sumStudent", GetType(Integer))
    '    dt = db.getdata(sql)
    '    Dim sumWrongAnswer As String = ""
    '    Dim sumStudent As String = ""
    '    For r As Integer = 0 To dt.Rows.Count - 1
    '        idQuestion = dt.Rows(r)("Question_Id").ToString
    '        QQ_No = dt.Rows(r)("QQ_No").ToString
    '        getSumMostWrong(idQuestion)
    '        sumWrongAnswer = _getSumMostWrong.sumWrongAnswer.ToString
    '        sumStudent = _getSumMostWrong.sumStudent.ToString
    '        dtMostWrongAnswerDesc.Rows.Add(idQuestion, QQ_No, sumWrongAnswer, sumStudent)
    '    Next
    '    Dim dtView As New DataView(dtMostWrongAnswerDesc)
    '    dtView.Sort = "sumWrongAnswer DESC"
    '    dtMostWrongAnswerDesc = dtView.ToTable
    '    If (Session("QuizUseTablet") = True) Then
    '        CreateChart(dtView)
    '        IndexRunChoice = 1
    '    End If
    '    'เรียงตามคนผิดจากมากไปน้อย
    '    Dim sumOfQuestion, pageNumber As Integer
    '    sumOfQuestion = dtMostWrongAnswerDesc.Rows.Count 'จำนวนทั้งหมดของข้อสอบ
    '    pageNumber = Math.Ceiling(sumOfQuestion / 15) 'หารเพื่อกำหนดจำนวนหน้าแสดงข้อสอบ หน้าละ 15 ข้อ     
    '    mainReview.InnerHtml = ""
    '    Dim strDiv As String = ""
    '    Dim divBlock As String = ""
    '    Dim j As Integer, i As Integer = 1
    '    Dim toNo As Integer = 15
    '    If (sumOfQuestion < 15) Then
    '        toNo = sumOfQuestion
    '    End If
    '    For j = 1 To pageNumber
    '        'strDiv = "<div style='width: 90%; margin-left: auto; margin-right: auto;' id='divSwipe" + CStr(j) + "' runat='server'>"
    '        'mainReview.InnerHtml += strDiv
    '        'ใช้ Panel มาสร้าง block ข้อสอบ
    '        Dim panelPage As New Panel()
    '        With panelPage
    '            .ID = "divSwipe" + CStr(j)
    '            .Style.Add("width", "700px")
    '            .Style.Add("margin-left", "auto")
    '            .Style.Add("margin-right", "auto")
    '            .Style.Add("padding-left", "20px")
    '        End With
    '        For i = i To toNo
    '            idQuestion = dtMostWrongAnswerDesc.Rows(i - 1)("Question_Id").ToString
    '            noQuestion = dtMostWrongAnswerDesc.Rows(i - 1)("QQ_No")
    '            sumWrongAnswer = dtMostWrongAnswerDesc.Rows(i - 1)("sumWrongAnswer")
    '            sumStudent = dtMostWrongAnswerDesc.Rows(i - 1)("sumStudent")
    '            getSumMostWrong(idQuestion) 'ไป get ค่า คนผิดและจำนวนนักเรียนในข้อนั้น
    '            Dim perMostWrong As Integer
    '            'If Not sumStudent = 0 Then
    '            If (Session("QuizUseTablet") = True) Then
    '                If Not sumStudent = 0 Then
    '                    perMostWrong = (CInt(sumWrongAnswer) / CInt(sumStudent)) * 100
    '                End If
    '            End If
    '            'divBlock = "<div class='divBlockMostWrongAns' id='" + idQuestion + "'><table style='width: 100%'>"
    '            'divBlock += "<tr><td><span><font size='5px'><b>" + noQuestion + "</b></font></span></td></tr>"
    '            'divBlock += "<tr><td style='text-align: center'><img src='" + getImageMostWrong(perMostWrong) + "' /></td></tr>"
    '            'divBlock += "<tr><td style='text-align: center'><img src='../Images/Activity/mostWrongFace/wrong.gif' width='25px' height='25px' />"
    '            'divBlock += "<span><b><font color='red' id='mostAns'>" + sumWrongAnswer + "</font>  /  <font color='green'>" + sumStudent + "</font></b></span></td></tr></table></div>"
    '            'mainReview.InnerHtml += divBlock
    '            'สร้างตัว block ข้อสอบ
    '            Dim panelBlock As New Panel()
    '            With panelBlock
    '                .ID = "QQ_no" + noQuestion
    '                .Style.Add("position", "relative")
    '                .Style.Add("background-color", "#F4F7FF")
    '                .Style.Add("margin", "6px")
    '                .Style.Add("border", "2px solid #AFAFAF")
    '                .Style.Add("width", "120px")
    '                .Style.Add("height", "120px")
    '                .Style.Add("float", "left")
    '                .Style.Add("-webkit-border-radius", "15px")
    '            End With
    '            ' เช็คว่าเป็น quiz ที่ใช้ tablet หรือเปล่า
    '            If (Session("QuizUseTablet") = True) Then
    '                ' สร้างเลขข้อ
    '                Dim pnlNo_Question As New Panel
    '                Dim lblNo_Question As New Label()
    '                With lblNo_Question
    '                    .ID = "lblNo_Question" + noQuestion
    '                    .Text = "ข้อที่ " & noQuestion
    '                    .Style.Add("position", "relative")
    '                    .Style.Add("top", "5px")
    '                    .Style.Add("left", "5px")
    '                    .Style.Add("font", "bold 15px 'THSarabunNew'")
    '                End With
    '                pnlNo_Question.Controls.Add(lblNo_Question)
    '                ' สร้างจำนวนคนผิด และ จำนวนคนที่สอบทั้งหมด
    '                Dim lblNo_Wrong As New Label()
    '                With lblNo_Wrong
    '                    .ID = "lblNo_WrongAndAllStudent" + noQuestion
    '                    .Text = "ผิด " & sumWrongAnswer.ToString
    '                    .Style.Add("position", "relative")
    '                    .Style.Add("bottom", "-69px")
    '                    .Style.Add("left", "21px")
    '                    .Style.Add("color", "red")
    '                    .Style.Add("font", "bold 12px 'THSarabunNew'")
    '                End With
    '                Dim lblNo_AllStudent As New Label()
    '                With lblNo_AllStudent
    '                    .ID = "lblNo_AllStudent" + noQuestion
    '                    .Text = "จาก " & sumStudent
    '                    .Style.Add("position", "relative")
    '                    .Style.Add("bottom", "-69px")
    '                    .Style.Add("left", "30px")
    '                    .Style.Add("color", "green")
    '                    .Style.Add("font", "bold 12px 'THSarabunNew'")
    '                End With
    '                ' สร้าง btn image หน้าคนยิ้ม
    '                Dim btnImg As New ImageButton()
    '                With btnImg
    '                    .ID = idQuestion
    '                    .ImageUrl = getImageMostWrong(perMostWrong)
    '                    .Style.Add("position", "relative")
    '                    .Style.Add("left", "28px")
    '                    .Style.Add("top", "-25px")
    '                    .Style.Add("width", "65px")
    '                    .CssClass = "imgQuestion"
    '                End With
    '                AddHandler btnImg.Click, AddressOf Me.getQuestionBySelect
    '                panelBlock.Controls.Add(pnlNo_Question) ' block add label no.question
    '                panelBlock.Controls.Add(lblNo_Wrong) ' block add no.wrong
    '                panelBlock.Controls.Add(lblNo_AllStudent) ' block add no.allstudent
    '                panelBlock.Controls.Add(btnImg) ' block add button
    '                If sumWrongAnswer = sumStudent Then 'รอทำสูตรเช็คว่า ให้นักเรียนไม่กดตอบกี่เปอเซนถึงจะให้เป็นข้อข้าม
    '                    'Dim panelSkip As New Panel()
    '                    'With panelSkip
    '                    '    .ID = "Skip" + noQuestion
    '                    '    .Style.Add("position", "absolute")
    '                    '    .Style.Add("background-image", "../Images/Activity/mostWrongFace/skipbadge.png")
    '                    '    .Style.Add("width", "120px")
    '                    '    .Style.Add("height", "120px")
    '                    '    .Style.Add("float", "left")
    '                    '    .Style.Add("top", "0px")
    '                    'End With
    '                    'panelBlock.Controls.Add(panelSkip) ' add page skips
    '                    panelBlock.Style.Add("background-image", "../Images/Activity/skip90.png")
    '                End If
    '            Else
    '                ' ถ้าเล่นโดยไม่ใช้ tablet ให้แสดงแค่เลขข้อ
    '                ' สร้างปุ่มที่มีแค่เลขที่ข้อ
    '                Dim btnImg As New Button()
    '                With btnImg
    '                    .ID = idQuestion
    '                    .Text = "ข้อที่ " & noQuestion
    '                    .Style.Add("position", "relative")
    '                    .Style.Add("left", "0px")
    '                    .Style.Add("top", "0px")
    '                    .Style.Add("width", "inherit")
    '                    .Style.Add("height", "inherit")
    '                    '.Style.Add("background-color", "#1EC9F4")
    '                    .Style.Add("font", "bold 20px 'THSarabunNew'")
    '                    '.Style.Add("color", "#47433F")
    '                    '.Style.Add("-webkit-border-radius", "40px")
    '                    '.Style.Add("-webkit-box-shadow", "0 1px 2px rgba(0,0,0,.2)")
    '                    '.Style.Add("border", "1px solid #C6E7F0")
    '                    .Style.Add("cursor", "pointer")
    '                    '.Style.Add("background", "-webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9))")
    '                    .Style.Add("-webkit-border-radius", "15px")
    '                    .Style.Add("border", "0px")
    '                    .CssClass = "btnQuestion"
    '                End With
    '                AddHandler btnImg.Click, AddressOf Me.getQuestionBySelect
    '                btnImg.Style.Add("background-image", "../Images/Activity/reviewBadge.png")
    '                panelBlock.Controls.Add(btnImg) ' block add button
    '            End If
    '            panelPage.Controls.Add(panelBlock) ' page add block
    '        Next
    '        i = i 'ให้ i เท่ากับข้อล่าสุด
    '        If (sumOfQuestion > 15) Then
    '            sumOfQuestion = sumOfQuestion - 15 'ถ้าจำนวนข้อเกิน 15 ข้อ ให้หักออกไป 15 ข้อ
    '            If (sumOfQuestion > 15) Then
    '                toNo = toNo + 15 ' ถ้ายังเกิน 15 อยู่ให้ทำ +15 
    '            Else
    '                toNo = toNo + sumOfQuestion
    '            End If
    '        End If
    '        'strDiv = "</div>"
    '        'mainReview.InnerHtml += strDiv
    '        mainReview.Controls.Add(panelPage)
    '    Next
    '    Dim NewDatatb As New DataTable
    '    NewDatatb = dtView.ToTable
    '    Dim ListQId = ViewStateListQId
    '    ' Dim dt As DataTable = ClsActivity.GetQuestion(Quiz_Id, False)
    '    ListQId.Clear()
    '    For i = 0 To NewDatatb.Rows.Count - 1
    '        ListQId.Add(New ListQuestionId2 With {.QuestionId = NewDatatb.Rows(i)("Question_Id").ToString, .RowNum = i + 1, .IndexForChoice = NewDatatb.Rows(i)("QQ_No")})
    '    Next
    '    If Not Page.IsPostBack Then
    '        _ExamNum = 1
    '    End If
    '    Dim rSort = (From q In ListQId Where q.RowNum = _ExamNum).SingleOrDefault
    '    Dim dtQuestion As DataTable = ClsActivity.GetQuestionDetail(rSort.QuestionId)
    '    Dim Quest As String = dtQuestion.Rows(0)("Question_Name")
    '    Quest = Quest.Replace("___MODULE_URL___", clsPDf.GenFilePath(dtQuestion.Rows(0)("QSet_Id").ToString))
    '    Dim IndexChoice As String = rSort.IndexForChoice
    '    'Question_No.InnerText = IndexChoice
    '    'QuestionName.InnerHtml = Quest
    '    mainQuestion.InnerHtml = IndexChoice + ". " + Quest
    '    _ExamNum = IndexChoice
    '    Dim taganswer As String = GetAnswerDetails(rSort.QuestionId.ToString, dtQuestion.Rows(0)("QSet_Id").ToString)
    '    'Answer.InnerHtml = taganswer
    '    AnswerTbl.InnerHtml = taganswer
    '    lblNoExam.Text = _ExamNum & " / " & _ExamAmount
    '    lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount
    '    ViewStateListQId = ListQId
    '    Dim rows = (From q In ListQId Where q.RowNum = _ExamNum).SingleOrDefault
    '    ChagePositionCartoon(IndexRunChoice, 1, rows.QuestionId)
    'End Sub
    'function share  
    '<Services.WebMethod()>
    'Public Shared Function GetQuestionDetailsCodeBehind(ByVal Question_ID As String) As String
    '    Dim taganswer As String = ""
    '    Dim ShowAnswer = True, questionName As String = "", QQ_No As String = ""
    '    Dim sqlQuestion As String
    '    sqlQuestion = "select (select Question_name from tblQuestion where question_id = '" & Question_ID & "') as Question_name,"
    '    sqlQuestion += "(select QQ_No from tblQuizQuestion WHERE Quiz_Id = 'CA487E8E-6A55-464B-A2CB-FC4BAD101C80' AND question_id = '" & Question_ID & "') as QQ_No"
    '    Dim dtQuestion As DataTable
    '    Dim dbQuestion As New ClassConnectSql()
    '    dtQuestion = dbQuestion.getdata(sqlQuestion)
    '    questionName = dtQuestion.Rows(0)("Question_name")
    '    QQ_No = dtQuestion.Rows(0)("QQ_No")
    '    Dim Ans1 As String, Ans2 As String, Ans3 As String, Ans4 As String, right As String
    '    Dim sql As String
    '    sql = "select answer_name from tblanswer where question_id = '" & Question_ID & "' order by answer_no;"
    '    Dim dt As DataTable
    '    Dim db As New ClassConnectSql()
    '    dt = db.getdata(sql)
    '    If ShowAnswer = True Then
    '        sql = "select answer_name from tblanswer where question_id = '" & Question_ID & "' and answer_score = '1' order by answer_no;"
    '        Dim dtQuestionId As DataTable
    '        dtQuestionId = db.getdata(sql)
    '        If dt.Rows(0)("Answer_Name") = dtQuestionId.Rows(0)("Answer_Name") Then
    '            Ans1 = dt.Rows(0)("Answer_Name")
    '            right = "1"
    '        Else
    '            Ans1 = dt.Rows(0)("Answer_Name")
    '        End If
    '        If dt.Rows(1)("Answer_Name") = dtQuestionId.Rows(0)("Answer_Name") Then
    '            Ans2 = (dt.Rows(1)("Answer_Name"))
    '            right = "2"
    '        Else
    '            Ans2 = (dt.Rows(1)("Answer_Name"))
    '        End If
    '        If dt.Rows(2)("Answer_Name") = dtQuestionId.Rows(0)("Answer_Name") Then
    '            Ans3 = dt.Rows(2)("Answer_Name")
    '            right = "3"
    '        Else
    '            Ans3 = dt.Rows(2)("Answer_Name")
    '        End If
    '        If dt.Rows(3)("Answer_Name") = dtQuestionId.Rows(0)("Answer_Name") Then
    '            Ans4 = dt.Rows(3)("Answer_Name")
    '            right = "4"
    '        Else
    '            Ans4 = dt.Rows(3)("Answer_Name")
    '        End If
    '    Else
    '        taganswer = "<tr><td>ก.</td> <td>" & dt.Rows(0)("Answer_Name") & "</td> "
    '        taganswer &= "<td ></td> <td>ข.</td><td>" & dt.Rows(1)("Answer_Name") & "</td></tr> "
    '        taganswer &= "<tr><td>ค.</td> <td>" & dt.Rows(2)("Answer_Name") & "</td> "
    '        taganswer &= "<td></td> <td>ง.</td><td>" & dt.Rows(3)("Answer_Name") & "</td></tr>"
    '    End If
    '    Dim js As New JavaScriptSerializer()
    '    Dim sb As New StringBuilder()
    '    'Dim JsonString = New With {.stuId = studentId, .TabletIsActive = tabIsActive}
    '    Dim JsonString = New With {.question = questionName, .QQ_No = QQ_No, .Ans1 = Ans1, .Ans2 = Ans2, .Ans3 = Ans3, .Ans4 = Ans4, .idRigth = right}
    '    Dim idAndActive = js.Serialize(JsonString)
    '    Return idAndActive
    'End Function
End Class
<Serializable()>
Public Class ListQuestionId2
    Private _QuestionId As String
    Public Property QuestionId() As String
        Get
            Return _QuestionId
        End Get
        Set(ByVal value As String)
            _QuestionId = value
        End Set
    End Property

    Private _RowNum As String
    Public Property RowNum() As String
        Get
            Return _RowNum
        End Get
        Set(ByVal value As String)
            _RowNum = value
        End Set
    End Property

    Private _IndexForChoice As String
    Public Property IndexForChoice() As String
        Get
            Return _IndexForChoice
        End Get
        Set(ByVal value As String)
            _IndexForChoice = value
        End Set
    End Property

End Class
