Imports System.Web.Script.Serialization
Imports System.Web

Public Class ActivityPage_PadTeacher
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Shared Index As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim UseClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    Dim KNSession As New ClsKNSession()
    Dim IsGUID As New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)
    Public GroupName As String
    Public Dialog As String
    Public DialogTitle As String
    Public PracticeFromComputer As String
    Public GroupOld As String
    Public shareSelfPace As Boolean
    Public MyAnswer As String
    Public CorrectAnswer As String
    'Update 22-05-56 Variable Tools
    Public useTools As Boolean = False
    Public tools_Calculator As Boolean = False
    Public tools_WordBook As Boolean = False
    Public tools_Note As Boolean = False
    Public tools_Protractor As Boolean = False
    Public tools_Dictionary As Boolean = False

    Public UserId As String
    Public questionId As String

    Dim ClsSelectSession As New ClsSelectSession()

    Public Property _ExamNum As Integer
        Get
            Return ViewState("_ExamNum")
        End Get
        Set(ByVal value As Integer)
            ViewState("_ExamNum") = value
        End Set
    End Property
    Public Property _AnswerState As String ' None 0, Question 1, CorrectAnswer 2
        Get
            Return ViewState("AnswerState")
        End Get
        Set(ByVal value As String)
            ViewState("AnswerState") = value
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

    Public Property VBIsSelfPace As Boolean
        Get
            Return ViewState("_VBIsSelfPace")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_VBIsSelfPace") = value
        End Set
    End Property

    Public Property VBNeedShowScoreChoiceToChoice As Boolean
        Get
            Return ViewState("_VBNeedShowScoreChoiceToChoice")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_VBNeedShowScoreChoiceToChoice") = VBNeedShowScoreChoiceToChoice
        End Set
    End Property


    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("Quiz_Id") = "238d3b91-7b35-4886-a446-5865559d9a2f"
        'Session("Quiz_Id") = "1921B46B-492E-47CE-9F40-009B92749C13"
        'Session("SchoolId") = "1000001"
        'Session("QuizUseTablet") = True
        'HttpContext.Current.Application("NeedAddEvaluationIndex") = False   'เอาไว้เทสเฉยๆ
        'Session("QuizUseTablet") = "True"

        'Session("UserId").ToString

        ' Group SignalR
        GroupName = ClsActivity.GetGroupNameFromQuizId(Session("Quiz_Id").ToString()) 'หา GroupName เพื่อ addGroup ให้ SignalR
        GroupOld = Session("selectedSession").ToString()

        If Session("PracticeFromComputer") Then
            PracticeFromComputer = "True"
        Else
            'Dim clsSelectSess As New ClsSelectSession()
            'If (Session("Quiz_Id") Is Nothing Or Session("Quiz_Id") = "") Then
            '    clsSelectSess.checkCurrentPage(Session("UserId").ToString, Session("selectedSession").ToString())
            'End If

            'GroupName = ClsActivity.GetGroupNameFromQuizId(Session("Quiz_Id").ToString()) 'หา GroupName เพื่อ addGroup ให้ SignalR
            'GroupOld = Session("selectedSession").ToString()

            'Dim res As String = ResolveUrl("~").ToLower()
            'Dim currentPage As String = res & "activity/activitypage.aspx"
            'clsSelectSess.setCurrentPage(currentPage)

            PracticeFromComputer = "False"

        End If

        If Session("UserId").ToString = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else

            If Not IsPostBack Then
                Session("PlayerId") = Session("UserId").ToString


                Dim Quiz_Id As String = Session("Quiz_Id").ToString
                Dim dtSetting As DataTable = ClsActivity.GetSetting(Quiz_Id)
                Dim testSetName As String = ClsActivity.GetTestsetName(Quiz_Id)
                lblTestsetName.Text = testSetName

                'Update 22-05-56 set value to viewstateTools
                ViewState("ToolsInQuiz") = dtSetting.Rows(0)("EnabledTools")

                If Not IsPostBack Then

                    Session("NeedTimer") = dtSetting.Rows(0)("NeedTimer")

                    If dtSetting.Rows(0)("NeedCorrectAnswer") = True Then
                        If dtSetting.Rows(0)("IsShowCorrectAfterComplete") Then
                            _AnswerState = "0"
                            Session("ShowCorrectAfterComplete") = True
                            Session("ShowCorrectAfterCompleteState") = False
                        Else
                            _AnswerState = "1"
                            Session("ShowCorrectAfterComplete") = False
                            Session("ShowCorrectAfterCompleteState") = False
                        End If
                        Session("showAnswer") = True
                    Else
                        _AnswerState = "0"
                        Session("ShowCorrectAfterComplete") = False
                        Session("ShowCorrectAfterCompleteState") = False
                        Session("showAnswer") = False
                    End If

                    Session("Selfpace") = dtSetting.Rows(0)("Selfpace")

                    If KNSession(Quiz_Id & "|" & "NeedShowScore") Is Nothing Then
                        Dim NeedShowScore As Object
                        NeedShowScore = dtSetting.Rows(0)("NeedShowScore")
                        KNSession(Quiz_Id & "|" & "NeedShowScore") = NeedShowScore

                        Dim NeedShowScoreAfterComplete As Object
                        NeedShowScoreAfterComplete = dtSetting.Rows(0)("NeedShowScoreAfterComplete")
                        KNSession(Quiz_Id & "|" & "NeedShowScoreAfterComplete") = NeedShowScoreAfterComplete

                        Dim IsDifferentQuestion As Object
                        IsDifferentQuestion = dtSetting.Rows(0)("IsDifferentQuestion")
                        KNSession(Quiz_Id & "|" & "IsDifferentQuestion") = IsDifferentQuestion

                        Dim IsDifferentAnswer As Object
                        IsDifferentAnswer = dtSetting.Rows(0)("IsDifferentAnswer")
                        KNSession(Quiz_Id & "|" & "IsDifferentAnswer") = IsDifferentAnswer

                        Dim SelfPace As Object
                        SelfPace = dtSetting.Rows(0)("SelfPace")
                        KNSession(Quiz_Id & "|" & "SelfPace") = SelfPace
                        VBIsSelfPace = SelfPace

                        If NeedShowScore = True And NeedShowScoreAfterComplete = False Then
                            VBNeedShowScoreChoiceToChoice = True
                        Else
                            VBNeedShowScoreChoiceToChoice = False
                        End If

                    End If

                    _ExamNum = ClsSelectSession.GetExamNum()
                    If _ExamNum = Nothing Then
                        Dim MaxExamNum As String = ClsActivity.GetExamNum(Quiz_Id).ToString
                        If MaxExamNum = "" Then
                            _ExamNum = 1
                        Else
                            _ExamNum = MaxExamNum
                        End If
                    End If

                    _ExamAmount = ClsActivity.GetExamAmount(Quiz_Id)

                    Session("_ExamAmount") = _ExamAmount

                    If Not ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
                        ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"))
                    End If





                    UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บค่า AnswerState ไปใช้ที่หน้าเด็ก
                    mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Session("PlayerId").ToString, _AnswerState, _ExamNum, Session("Selfpace"))
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), True)
                    Else
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), False)
                    End If

                    MyAnswer = ClsActivity.MyAnswer
                    CorrectAnswer = ClsActivity.CorrectAnswer

                    questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum)
                    hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId).ToString()
                    'lblNoExam.Text = _ExamNum & " / " & _ExamAmount
                    'lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount


                    UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Session("Quiz_Id").ToString, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
                    UpdateLastUpdateWhenNextOrPrev(Session("Quiz_Id").ToString, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
                    ' save คำตอบที่ checkmark2 temptblChoice
                    If (Session("QuizUseTamplate") = True) Then
                        Dim dtAnswer As DataTable
                        dtAnswer = ClsActivity.GetCorrectAnswerDetail(questionId, Session("Quiz_Id").ToString)
                        For j As Integer = 0 To dtAnswer.Rows.Count - 1
                            If (dtAnswer.Rows(j)("Answer_Score") = "1") Then
                                ' set โจทย์และคำตอบไปยัง temptblChoice
                                Dim ChkMark As New ClsCheckMark
                                ChkMark.saveCorrectAnswerToCheckmark(_ExamNum, (j + 1))
                            End If
                        Next
                    End If
                End If

                HDCheckChangeQuestion.Value = "Reload" 'กำหนดค่าให้ HiddenField มีค่าเป็นคำว่า Reload เพื่อส่ง SignalR

            End If

            shareSelfPace = VBIsSelfPace

        End If

        'Update 22-05-56 set variable Tools
        If Not ViewState("ToolsInQuiz") = 0 Then
            useTools = True
            getToolsInQuiz(ViewState("ToolsInQuiz"))
        End If
        UserId = Session("UserId").ToString


    End Sub

    Private Sub UpdateLastUpdateWhenNextOrPrev(ByVal QuizID As String, ByVal QuestionID As String)
        Dim _DB As New ClassConnectSql
        If QuizID Is Nothing Or QuestionID Is Nothing Then 'ถ้า QuizId หรือ QuestionId ไม่มีค่าให้ออกจาก Sub ทันที
            Exit Sub
        End If
        If QuizID <> "" Or QuestionID <> "" Then
            Dim sql As String
            Try
                If InStr(1, QuestionID.ToString, ",") = 0 Then 'ถ้าคำถามไม่ใช่ Type 6 Update แค่ QuestionId นั้น
                    sql = " UPDATE dbo.tblQuizQuestion SET LastUpdate = dbo.GetThaiDate() , ClientId = Null WHERE Quiz_Id = '" & _DB.CleanString(QuizID) & "' " &
                                " AND Question_Id = '" & _DB.CleanString(QuestionID) & "' "
                Else 'ถ้าคำถามเป็น Type 6 Update QuestionId ทั้งหมดที่อยู่ใน QuizQuestion นั้น ทีเป็น Type 6
                    sql = " UPDATE dbo.tblQuizQuestion SET LastUpdate = dbo.GetThaiDate(), ClientId = Null WHERE Question_Id IN ( " &
                          " SELECT tblQuizQuestion_1.Question_Id " &
                          " FROM tblQuizQuestion INNER JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                          " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                          " tblQuestion AS tblQuestion_1 ON tblQuestionSet.QSet_Id = tblQuestion_1.QSet_Id INNER JOIN " &
                          " tblQuizQuestion AS tblQuizQuestion_1 ON tblQuestion_1.Question_Id = tblQuizQuestion_1.Question_Id " &
                          " WHERE (tblQuestionSet.QSet_Type = 6) AND (tblQuizQuestion.Quiz_Id = '" & QuizID & "') AND  " &
                          " (tblQuizQuestion.Question_Id = '" & QuestionID & "') AND (tblQuizQuestion_1.Quiz_Id = '" & QuizID & "')) "
                End If
                _DB.Execute(sql)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Exit Sub
            End Try
        End If
    End Sub

    Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        controlBtnPrev()
        ClsSelectSession.SetExamNum(_ExamNum)
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        controlBtnNext()
        ClsSelectSession.SetExamNum(_ExamNum)
    End Sub

    Private Sub controlBtnPrev()

        If ((_ExamNum - 1) < 1) Then
            ' //ไม่ต้องทำอะไร
        Else
            _ExamNum = _ExamNum - 1
            If Not _AnswerState = "0" Then
                _AnswerState = "2"
                If Not KNSession(Session("Quiz_Id").ToString() & "|" & "SelfPace") Then
                    'Functin UpdateIsScore
                End If
            End If
        End If

        'UseClsDroidPad.RemoveAndAddNewAnsState(Session("Quiz_Id").ToString, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
        'UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Session("Quiz_Id").ToString, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
        'mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Session("Quiz_Id").ToString, Session("PlayerId").ToString, _AnswerState, _ExamNum, False)
        'AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Session("Quiz_Id").ToString, _ExamNum, Session("PracticeMode"), False)
        'questionId = ClsActivity.GetQuestionID(Session("Quiz_Id").ToString, _ExamNum)
        'UpdateLastUpdateWhenNextOrPrev(Session("Quiz_Id").ToString, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
        'HDCheckChangeQuestion.Value = "Reload"
        'hdCmdOldSession.Value = "Prev"



        UseClsDroidPad.RemoveAndAddNewAnsState(Session("Quiz_Id").ToString(), _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
        UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Session("Quiz_Id").ToString, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
        mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Session("Quiz_Id").ToString(), Session("PlayerId").ToString, _AnswerState, _ExamNum, Session("Selfpace"))
        If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Session("Quiz_Id").ToString(), _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), True)
        Else
            AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Session("Quiz_Id").ToString(), _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), False)
        End If

        'MyAnswer = ClsActivity.MyAnswer
        'CorrectAnswer = ClsActivity.CorrectAnswer
        questionId = ClsActivity.GetQuestionID(Session("Quiz_Id").ToString(), _ExamNum)
        hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId).ToString()


        If VBIsSelfPace = False Then
            UpdateLastUpdateWhenNextOrPrev(Session("Quiz_Id").ToString, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
            HDCheckChangeQuestion.Value = "Reload"
        End If

    End Sub


    Private Sub controlBtnNext()
        Dim Quiz_Id As String = Session("Quiz_Id").ToString
        Dim PlayerType As String = ClsActivity.GetPlayerType(Session("PlayerId").ToString, Quiz_Id)
        Dim IsTeacher As Boolean
        If PlayerType = "1" Then
            IsTeacher = True
        End If
        'Dim HaveQuestion As Boolean
        'Dim HaveIsScored As Boolean

        '**************
        'เช็คก่อนว่า มีข้อสอบยัง ถ้ายังแสดงว่ายังไม่ได้เล่นข้อนี้
        If (_ExamNum = _ExamAmount) And (_AnswerState = "0") Then
            If Session("ShowCorrectAfterComplete") = True Then
                _ExamNum = 0
                _AnswerState = "2"

                Session("ShowCorrectAfterCompleteState") = True
            End If
        End If

        ' ขอตัด havequestion ออกก่อนนะครับ And (ClsActivity.HaveQuestion(Quiz_Id, _ExamNum)) 'เพิ่ม And HaveQuestion ด้วย เพื่อให้รู้ว่าเคยมาถึงข้อนี้มั้ย
        If (_ExamNum = _ExamAmount) And (_AnswerState = "0" Or _AnswerState = "2") Then
            Dim LeapExam As String = ClsActivity.CountLeapExam(Session("Quiz_Id").ToString, Session("UserId").ToString)

            If LeapExam = "0" Then
                DialogTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
            Else
                If IsTeacher Then
                    DialogTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
                Else
                    DialogTitle = "ยังไม่ได้ทำข้อสอบอีก " & LeapExam & " ข้อ จบควิซเลยไหมคะ ?"
                End If

            End If
            Dialog = "True"
        Else

            If _AnswerState = "1" Then
                _AnswerState = "2"
                If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                    'Function UpdateIsscore ExamNum
                    ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum)
                Else
                    ClsActivity.UpdateIsScoredTeacher(Quiz_Id, _ExamNum)
                End If
            Else 'State <> 1
                _ExamNum += 1

                If _AnswerState <> "0" Then
                    If Not Session("ShowCorrectAfterCompleteState") Then
                        _AnswerState = "1"
                    End If

                    If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                        'Function UpdateIsscore ExamNum
                        'ถ้าเป็นแสดงคะแนนแบบข้อต่อข้อ เข้าไป Update IsScore
                        If VBNeedShowScoreChoiceToChoice = True Then
                            ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum)
                        End If
                    End If

                End If

                'มีข้อสอบมั้ย
                If ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
                    'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
                    If KNSession(Quiz_Id & "|" & "SelfPace") Then
                        'ถ้าไปไม่พร้อมกัน ให้เช็คว่าเป็นฝึกฝนจากคอมมั้ย
                        If HttpContext.Current.Session("PracticeFromComputer") Then
                            'เป็นฝึกฝนจากคอมให้ SetQuizScore
                            If _AnswerState <> "2" And Not ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
                                ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"))
                            End If

                        Else
                            'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
                            If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum) Then
                                'ถ้าตรวจแล้ว 
                                If _AnswerState <> "0" Then
                                    _AnswerState = "2"
                                End If

                            End If

                            If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum) Then
                                If _AnswerState <> "0" Then
                                    _AnswerState = "2"
                                End If
                            End If

                        End If
                    Else

                        If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum) Then
                            'ถ้าตรวจแล้ว
                            If _AnswerState <> "0" Then
                                _AnswerState = "2"
                            End If

                        End If
                    End If

                Else
                    ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"))
                    If KNSession(Quiz_Id & "|" & "SelfPace") Then
                        If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum) Then
                            If _AnswerState <> "0" Then
                                _AnswerState = "2"
                            End If
                        End If
                    End If
                    If _AnswerState = "0" And Not KNSession(Quiz_Id & "|" & "NeedShowScoreAfterComplete") And VBNeedShowScoreChoiceToChoice = True Then
                        If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                            'Function UpdateIsscore ExamNum - 1
                            ClsActivity.UpdateIsScore(Quiz_Id, CStr(CInt(_ExamNum) - 1))
                        End If
                    End If
                End If

            End If



            UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
            UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Session("Quiz_Id").ToString, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก

            mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Session("PlayerId").ToString, _AnswerState, _ExamNum, Session("Selfpace"))

            If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), True)
            Else
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), Session("HomeworkMode"), False)
            End If

            MyAnswer = ClsActivity.MyAnswer
            CorrectAnswer = ClsActivity.CorrectAnswer


            questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId).ToString()
            'lblNoExam.Text = _ExamNum & " / " & _ExamAmount
            'lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

            '_ExamNum = _ExamNum + 1

            If (_ExamNum = _ExamAmount) And (_AnswerState = "0") Then
                If Session("ShowCorrectAfterComplete") = True Then
                    Dialog = "False"
                Else
                    Dim LeapExam As String = ClsActivity.CountLeapExam(Session("Quiz_Id").ToString, Session("UserId").ToString)
                    DialogTitle = setTitleDialog(LeapExam, IsTeacher)
                    Dialog = "True"
                End If
            ElseIf (_ExamNum = _ExamAmount) And (_AnswerState = "2") Then
                Dim LeapExam As String = ClsActivity.CountLeapExam(Session("Quiz_Id").ToString, Session("UserId").ToString)
                DialogTitle = setTitleDialog(LeapExam, IsTeacher)
                Dialog = "True"
            Else
                Dialog = "False"
            End If


        End If

        If VBIsSelfPace = False Then
            UpdateLastUpdateWhenNextOrPrev(Session("Quiz_Id").ToString, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
            HDCheckChangeQuestion.Value = "Reload"
        End If

        'SwapStatus = ClsActivity.SwapStatus

    End Sub

    Private Function setTitleDialog(ByVal LeapExam As String, ByVal IsTeacher As Boolean) As String
        Dim strTitle As String = ""
        If LeapExam = "0" Then
            strTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
        Else
            If IsTeacher Then
                strTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
            Else
                strTitle = "ยังไม่ได้ทำข้อสอบอีก " & LeapExam & " ข้อ จบควิซเลยไหมคะ ?"
            End If
        End If
        setTitleDialog = strTitle
    End Function
    'Private Sub controlBtnNext()

    '    Dim Quiz_Id As String = Session("Quiz_Id").ToString
    '    Dim PlayerType As String = ClsActivity.GetPlayerType(Session("PlayerId").ToString, Quiz_Id)
    '    Dim IsTeacher As Boolean
    '    If PlayerType = "1" Then
    '        IsTeacher = True
    '    End If

    '    If _ExamNum = _ExamAmount And (_AnswerState = "0" Or _AnswerState = "2") Then

    '        'show dialog
    '    Else
    '        If _AnswerState = "1" Then
    '            _AnswerState = "2"
    '        Else 'State <> 1
    '            _ExamNum += 1

    '            If _AnswerState <> "0" Then
    '                _AnswerState = "1"
    '                If Not KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") Then
    '                    'Function UpdateIsscore ExamNum
    '                    ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum)
    '                End If
    '            End If

    '            'มีข้อสอบมั้ย
    '            If ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
    '                'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
    '                If KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") Then
    '                    'ถ้าไปไม่พร้อมกัน ให้เช็คว่าเป็นฝึกฝนจากคอมมั้ย
    '                    If HttpContext.Current.Session("PracticeFromComputer") Then
    '                        'เป็นฝึกฝนจากคอมให้ SetQuizScore
    '                        ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"))
    '                    Else
    '                        'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
    '                        If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum) Then
    '                            'ถ้าตรวจแล้ว
    '                            If _AnswerState <> "0" Then
    '                                _AnswerState = "2"
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"))
    '                If _AnswerState = "0" And Not KNSession.GetValueFromClsSess(Quiz_Id, "NeedShowScoreAfterComplete") Then
    '                    If Not KNSession.GetValueFromClsSess(Quiz_Id, "SelfPace") Then
    '                        'Function UpdateIsscore ExamNum - 1
    '                        ClsActivity.UpdateIsScore(Quiz_Id, CStr(CInt(_ExamNum) - 1))
    '                    End If
    '                End If
    '            End If
    '        End If

    '        'DROIDPAD
    '        UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
    '        UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Session("Quiz_Id").ToString, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก

    '        'RENDER EXAM
    '        mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Session("PlayerId").ToString, _AnswerState, _ExamNum, False)
    '        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Session("PlayerId").ToString, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), False)

    '        questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum)

    '    End If


    '    UpdateLastUpdateWhenNextOrPrev(Session("Quiz_Id").ToString, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด

    '    'SET VALUE Hidden
    '    HDCheckChangeQuestion.Value = "Reload"
    '    'hdCmdOldSession.Value = "Next"
    'End Sub

    'Private Sub checkStateForRender(ByVal ExamNum As String)
    '    Dim IsHasQuestion As Boolean = True ' ค่าที่ return มาจาก query ที่เช็คว่ามีข้อสอบหรือยัง
    '    Dim IsScored As Boolean = True 'ค่าที่บอกว่าข้อสอบตรวจหรือยัง
    '    Dim IsShowScore As Boolean = True 'ค่าที่บอกว่าโชว์คะแนนข้อต่อข้อหรือเปล่า

    '    If (_AnswerState <> "0") Then
    '        _AnswerState = "1"
    '    End If

    '    If (IsHasQuestion) Then
    '        If (IsScored) Then
    '            If (_AnswerState = "0") Then
    '                ' //function Render ปกติ
    '                renderExam(ExamNum)
    '            Else
    '                _AnswerState = "2"
    '                ' //function Render เฉลย
    '                renderExam(ExamNum)
    '                ' //function update score ของ PlayerId คนที่เล่นอยู่
    '            End If
    '        End If
    '    Else
    '        '//function insert tblquizscore ของ playerId คนที่เล่นอยู่
    '        If (_AnswerState = "0" AndAlso IsShowScore) Then
    '            ' //function update score ข้อที่แล้วของ PlayerId คนที่เล่นอยู่
    '            ' //function Render ปกติ
    '            renderExam(ExamNum)
    '        End If
    '    End If
    'End Sub

    'Private Sub renderExam(ByVal ExamNum As String)

    '    'If Request.QueryString("DeviceUniqueID") = Nothing Then
    '    '    Response.Redirect("../Activity/EmptySession.aspx")
    '    'End If

    '    'Dim DeviceId As String = Request.QueryString("DeviceUniqueID").ToString()
    '    Dim DeviceId = "teacher1"

    '    'If HttpContext.Current.Application("Sess" & DeviceId) Is Nothing Then
    '    '    Response.Redirect("../Activity/EmptySession.aspx")
    '    'End If

    '    If DeviceId <> "" Then
    '        'DVID = DeviceId
    '        Dim dt1 As New DataTable
    '        Dim Quiz_Id As String = ""
    '        Dim Player_Id As String = ""
    '        'dt1 = ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceId)

    '        dt1 = GetQuizFromDeviceUniqueID(DeviceId)

    '        If (dt1.Rows.Count > 0) Then
    '            Quiz_Id = dt1.Rows(0)("Quiz_Id").ToString()
    '            'Quiz_Id = "6A35CD11-6C3D-4B84-9B80-78A9C2B49DE6"
    '            Player_Id = dt1.Rows(0)("Player_Id").ToString()
    '            Session("Quiz_Id") = Quiz_Id
    '            If (_ExamNum = 0) Then
    '                _ExamNum = ClsActivity.GetExamAmount(Quiz_Id)
    '            End If

    '            IsPerQuestion = getNeedTimerPerQuestion(Quiz_Id)

    '            mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Player_Id, _AnswerState, ExamNum)
    '            AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Player_Id, _AnswerState, Quiz_Id, ExamNum, False)

    '        End If
    '    End If
    'End Sub

    Private Function GetQuizFromDeviceUniqueID(ByVal DeviceUniqueID As String) As DataTable
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable
        Dim sql As String = " select top 1 tqs.Quiz_Id,tqs.Player_Id,tqs.School_Code from t360_tblTablet tt inner join t360_tblTabletOwner tto "
        sql &= " on tt.Tablet_Id = tto.Tablet_Id inner join tblQuizSession tqs on tqs.Tablet_Id = tto.Tablet_Id "
        sql &= " where tt.Tablet_SerialNumber = '" & db.CleanString(DeviceUniqueID.Trim()) & "'  order by tqs.LastUpdate desc ; "
        dt = db.getdata(sql)
        Return dt
    End Function

    Private Function CheckBeforeAddQuestion() As Boolean
        Dim dtSettingQuiz As DataTable = ClsActivity.GetSetting(Session("Quiz_Id"))
        If (dtSettingQuiz.Rows(0)("Selfpace") = "0") Then

        End If
    End Function


    <Services.WebMethod()>
    Public Shared Function CreateStringLeapChoice(ByVal IsNormalSort As String) 'Function สร้างปุ่มกดข้ามข้อ


        'If DeviceUniqueId Is Nothing Or DeviceUniqueId = "" or  IsNormalSort is Nothing or IsNormalSort = "" Then
        '    Return "-1"
        'End If

        Dim _DB As New ClassConnectSql
        Dim ClsKNSession As New ClsKNSession()
        'Dim AnsState As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "CurrentAnsState")
        'If AnsState Is Nothing Or AnsState = "" Then
        '    Return "-1"
        'End If

        Dim QuizId As String = "2AD4DE6E-FF20-408F-8B87-EC83DCCF4BF9" 'Test
        'Dim QuizId As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "QuizId")
        'If QuizId Is Nothing Or QuizId = "" Then
        '    Return "-1"
        'End If
        Dim StudentId As String = "C8BB6C2B-B368-4D7B-B7DD-3FA62B3DE869" 'Test
        'Dim StudentId As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "CurrentPlayerId")
        'If StudentId Is Nothing Or StudentId = "" Then
        '    Return "-1"
        'End If
        Dim sql As String = ""

        If IsNormalSort = "True" Then 'ถ้าเรียงแบบปกติใช้คิวรี่นี้
            sql = " SELECT Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                            " AND Student_Id = '" & StudentId & "' ORDER BY  QQ_No "
        Else 'ถ้าเรียงจากข้อที่ยังไม่ได้ทำขึ้นก่อนใช้คิวรี่นี้
            sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                  " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL ORDER BY Answer_Id,QQ_No) as a " &
                  " UNION all " &
                  " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " &
                  " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NOT NULL ORDER BY QQ_No) as b "
        End If

        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        Dim CheckQuantity As Integer = 1
        Dim sb As New StringBuilder
        If dt.Rows.Count > 0 Then
            Dim EachQQNo As String = ""
            Dim EachIsScore As String = ""
            For i = 0 To dt.Rows.Count - 1
                ' StudentId = dt.Rows(i)("Student_Id").ToString() 'รับ QuestionId เพื่อเอามาเป็น Id ให้แต่ละปุ่ม
                EachQQNo = dt.Rows(i)("QQ_No").ToString() 'รับ QQ_No เพื่อเอามาเป็น Text ให้ปุ่ม
                EachIsScore = dt.Rows(i)("IsScored").ToString() 'รับ IsScore เพื่อที่จะได้รู้ว่าข้อนั้นตรวจหรือยัง
                If CheckQuantity <= 10 Then 'เช็คเพื่อให้สร้างปุ่มได้แค่หน้าละ 10 ปุ่มเท่านั้น
                    If CheckQuantity = 1 Then 'ถ้าเป็นรอบแรกของการสร้างแต่ละรอบต้องสร้าง Div ขึ้นมาเพื่อครอบก่อนสร้างปุ่ม
                        sb.Append("<div style='width:500px;margin-top:95px;margin-left:83px;' class='slide'>")
                    End If
                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then 'ถ้าข้อนี้ตอบแล้วเข้า If
                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='margin-left:90px;background-color:Green;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        Else
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='background-color:Green;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        End If
                        CheckQuantity += 1
                    Else 'ถ้าข้อนี้ยังไม่ได้ตอบเข้า Else
                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='margin-left:90px;background-color:Red;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        Else
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='background-color:Red;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        End If
                        CheckQuantity += 1
                    End If
                Else 'ถ้าเกิน 10 ปุ่มแล้วต้องขึ้นหน้าใหม่
                    sb.Append("</div>") 'ปิด Tag Div ก่อนที่จะสร้าง Div ที่ครอบปุ่มอันต่อไป
                    sb.Append("<div style='width:500px;margin-top:95px;margin-left:83px;' class='slide'>")
                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then
                        sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='margin-left:90px;background-color:Green;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                    Else
                        sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style='margin-left:90px;background-color:Red;' id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                    End If
                    CheckQuantity = 2
                End If
            Next
            sb.Append("</div>") 'ปิด Tag Div 
        Else
            Return "-1"
        End If

        Return sb.ToString()

    End Function


    Private Function getNeedTimerPerQuestion(ByVal QuizId As String) As Boolean
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = " SELECT IsPerQuestionMode FROM tblQuiz WHERE Quiz_Id = '" & QuizId & "';"
        getNeedTimerPerQuestion = db.ExecuteScalar(sql)

    End Function




    ' check tablet check in class
    <Services.WebMethod()>
    Public Shared Function getStudentCheckInClass() As String
        Dim db As New ClassConnectSql()
        Dim sql As String = ""
        Dim dt As DataTable

        sql = "SELECT  DISTINCT qs.Player_Id as Player_Id,qs.Player_Type,sd.Student_CurrentNoInRoom, tl.Tablet_Id,"
        sql &= " tl.Tablet_IsOwner, tl.Tablet_TabletName, qs.LastUpdate, qs.IsActive "
        sql &= "FROM tblQuizSession qs "
        sql &= "INNER JOIN (SELECT Player_Id, MAX(lastupdate) AS NewestRow FROM tblQuizSession "
        sql &= "WHERE tblQuizSession.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' GROUP BY Player_Id) AS NewestUserSubTable "
        sql &= "ON qs.Player_Id = NewestUserSubTable.Player_Id AND qs.lastupdate = NewestUserSubTable.NewestRow "
        sql &= "LEFT JOIN t360_tblStudent sd  "
        sql &= "ON qs.Player_Id = sd.Student_Id AND qs.School_Code = sd.School_Code "
        sql &= "LEFT JOIN t360_tblTablet tl "
        sql &= "ON qs.Tablet_Id = tl.Tablet_Id AND qs.School_Code = tl.School_Code "
        sql &= "WHERE  qs.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' AND qs.School_Code = '" & HttpContext.Current.Session("SchoolID") & "' "
        'sql &= "--AND qs.IsActive = '1'  "
        sql &= "ORDER BY sd.Student_CurrentNoInRoom "

        dt = db.getdata(sql)

        Dim strHtml As String = ""
        Dim strCheckIn As String = ""
        Dim strClassHtml As String = ""

        If dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                If row("IsActive").ToString() = "False" Then
                    strClassHtml = "divNotReady"
                Else
                    strClassHtml = "divReady"
                End If
                If row("Player_Type").ToString() <> "1" Then
                    strHtml += "<div id=" + row("Player_Id").ToString() + " class=" + strClassHtml + ">" + row("Student_CurrentNoInRoom").ToString() + "<span>" + row("Tablet_TabletName").ToString() + " </span></div>"
                End If
            Next
        End If

        getStudentCheckInClass = strHtml

    End Function

    'Update 22-05-56 BitWiseComparison Tools
    Private Sub getToolsInQuiz(ByVal EnabledTools As Integer)
        ' calculator
        If (EnabledTools And 2) = 2 Then
            tools_Calculator = True
        End If
        ' dictionary
        If (EnabledTools And 4) = 4 Then
            tools_Dictionary = True
        End If
        ' wordbook
        If (EnabledTools And 8) = 8 Then
            tools_WordBook = True
        End If
        ' note
        If (EnabledTools And 16) = 16 Then
            tools_Note = True
        End If
        ' protractor
        If (EnabledTools And 32) = 32 Then
            tools_Protractor = True
        End If
    End Sub

#Region "Bin Code"
    'Public Shared js As New JavaScriptSerializer()
    '<Services.WebMethod()>
    'Public Shared Function getModeQuizAndTimer()
    '    Dim db As New ClassConnectSql()
    '    Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString
    '    Dim sql As String = "  SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,DATEDIFF(SECOND,StartTime,dbo.GetThaiDate()) as timeDiff,DATEDIFF(SECOND,dbo.GetThaiDate(),DATEADD(MINUTE,TimePerTotal,StartTime)) as timeRemain FROM tblQuiz WHERE Quiz_Id = '" & QuizId & "'; "
    '    Dim dt = db.getdata(sql)

    '    Dim AllTime As Integer = 0
    '    Dim TimeRemain As Integer = 0
    '    Dim NeedTimer As Boolean
    '    Dim timerType As Boolean = False
    '    Dim timeTotal As Integer = 0
    '    Dim noWatch As Boolean = False

    '    If (dt.Rows.Count > 0) Then
    '        If (dt.Rows(0)("NeedTimer")) Then
    '            ' จับเวลาในการทำควิซ ทั้ง ข้อต่อข้อและทั้งหมด
    '            NeedTimer = True
    '            ' state ทำข้อสอบ 0 หรือ 1
    '            If (shareAnsState = "0" Or shareAnsState = "1") Then
    '                If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                    ' จับเวลาต่อข้อ
    '                    AllTime = CInt(dt.Rows(0)("TimePerQuestion"))
    '                    timerType = True
    '                Else
    '                    ' จับเวลาทั้งหมด
    '                    'AllTime = CInt(dt.Rows(0)("timeDiff"))
    '                    If CInt(dt.Rows(0)("timeRemain")) > 0 Then
    '                        TimeRemain = CInt(dt.Rows(0)("timeRemain"))
    '                        timeTotal = CInt(dt.Rows(0)("TimePerTotal"))
    '                    End If
    '                End If
    '            Else
    '                ' state เฉลย
    '                If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
    '                    ' เฉลยแบบมีเวลา
    '                    'TimeRemain = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                    If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                        ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
    '                        '' เมื่อเฉลยหมดเวลาให้กดปุ่ม next ให้
    '                        AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                        timerType = True
    '                    Else
    '                        ' ข้อสอบแบบจับเวลาทั้งหมด
    '                        If (CInt(dt.Rows(0)("timeRemain")) > 0) Then
    '                            ' เมื่อหมดเฉลยจะกดปุ่ม next ให้ แต่ถ้าหมดเวลาสอบจะขึ้น dialog
    '                            AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                            timerType = True
    '                        End If
    '                    End If
    '                Else
    '                    ' เฉลยแบบไม่มีเวลา
    '                    If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                        ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
    '                        '' เอาเวลาจากไหน เพื่อไม่ให้มันเซ็ต dialog ขึ้น หรือมันกด next เอง
    '                        noWatch = True
    '                    Else
    '                        ' ข้อสอบแบบจับเวลาทั้งหมด 
    '                        '' ให้ใช้เวลาเดียวกับตอนที่ render โจทย์
    '                        If CInt(dt.Rows(0)("timeRemain")) > 0 Then
    '                            AllTime = CInt(dt.Rows(0)("timeRemain"))
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Else
    '            ' ไม่จับเวลาในการทำควิซ 
    '            '' แต่ถ้ามีเฉลยข้อต่อข้อแล้วใส่เวลา เมื่อถึง state เฉลย เวลาก็ยังคงเป็นแบบเดินไปเรื่อยๆๆ
    '            NeedTimer = False
    '            AllTime = dt.Rows(0)("timeDiff")

    '            If (shareAnsState = "2") Then
    '                ' state เฉลย
    '                If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
    '                    NeedTimer = True
    '                    AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                    timerType = True
    '                End If
    '            End If
    '        End If

    '        Dim JsonString

    '        JsonString = New With {.NeedTimer = NeedTimer, .AllTime = AllTime, .timerType = timerType, .TimeRemain = TimeRemain, .timeTotal = timeTotal, .noWatch = noWatch}
    '        getModeQuizAndTimer = js.Serialize(JsonString)

    '    Else
    '        getModeQuizAndTimer = "ERROR"

    '    End If

    'End Function
#End Region

End Class