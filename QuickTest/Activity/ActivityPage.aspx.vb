Imports System.Web.Script.Serialization
Imports BusinessTablet360
Imports Microsoft.AspNet.SignalR.Infrastructure
Imports Microsoft.AspNet.SignalR
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports System.Web

Public Class ActivityPage

#Region "Variable"
    Inherits System.Web.UI.Page
    'Dim objTestSet As ClsTestSet
    Shared Index As Integer = 0
    'Dim cls As New ClsPDF(New ClassConnectSql)
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim SClsPractice As New Service.ClsPracticeMode(New ClassConnectSql)
    Dim UseClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    Dim KNSession As New ClsKNSession()
    Dim _DB As New ClassConnectSql()
    Dim IsGUID As New Regex("^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)
    Public GroupName As String
    Public Dialog As String = "False"
    Public DialogTitle As String
    Public ShowDialogState As String = "False"
    Dim UseCls As New ClassConnectSql
    Public PracticeFromComputer As String
    Public GroupOld As String

    Public shareSelfPace As Boolean
    Public IsNoAnswer As Boolean

    Public MyAnswer As String
    Public CorrectAnswer As String
    Public ViewIntroQsetId As String = ""
    Public ViewAmount As String
    Public EnableIntro As Boolean
    ' Public Shared SwapStatus As String
    Public srcFileIntro As String
    Public typeFileIntro As String

    'Update 09-05-56 Variable Tools
    Public tools_Calculator As Boolean = False
    Public tools_WordBook As Boolean = False
    Public tools_Note As Boolean = False
    Public tools_Protractor As Boolean = False
    Public tools_Dictionary As Boolean = False


    Public js As New JavaScriptSerializer()
    Public IsTimePerQuestion As Boolean
    Protected IsAndroid As Boolean

    Public UserId As String
    Public CurrentQuizId As String
    Public questionId As String
    Public NotReplyMode As String

    Public IE As String

    Protected NeedShowTip As Boolean

    Protected qsetFilePath As String = ""

    Dim redis As New RedisStore()
#End Region

#Region "ViewState"
    Protected Property TestsetName As String
        Get
            Return ViewState("TestsetName")
        End Get
        Set(value As String)
            ViewState("TestsetName") = value
        End Set
    End Property

    Public Property _ExamNum As Integer
        Get
            Return ViewState("_ExamNum")
        End Get
        Set(ByVal value As Integer)
            ViewState("_ExamNum") = value
        End Set
    End Property
    Protected Property _AnswerState As String ' None 0, Question 1, CorrectAnswer 2
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

    Public Property ArrExamNumIsPassAlready As ArrayList
        Get
            Return ViewState("ArrExamNumIsPassAlready")
        End Get
        Set(value As ArrayList)
            ViewState("ArrExamNumIsPassAlready") = value
        End Set
    End Property
    Public Property PlayerId As String
        Get
            Return ViewState("_PlayerId")
        End Get
        Set(ByVal value As String)
            ViewState("_PlayerId") = value
        End Set
    End Property
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Log.Record(Log.LogType.PageLoad, "pageload activitypage", False)
        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "activitypage session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        End If

        'NeedShowTip = True ' รอเทสเรื่อง กระพริบก่อน เผื่อกระทบกับตรงจุดนี้
        If Not Page.IsPostBack Then
            ' ส่วนของการแสดง qtip
            If Session("ChooseMode") <> EnumDashBoardType.PracticeFromComputer Then
                If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                    Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                    Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                    NeedShowTip = ClsUserViewPageWithTip.CheckUpdateUserViewPageWithTip(pageName)
                End If
            End If
        End If


        'HttpContext.Current.Application("NeedAddEvaluationIndex") = False   'เอาไว้เทสเฉยๆ
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        'Session("Quiz_Id") = "516915A4-017B-4A56-89E7-ACBD15ED4D67"
        Session("Quiz_Id") = "7E11AF72-637C-4C8F-8E85-1160072AD2D8"
        Session("SchoolId") = "1000001"
        Session("ChooseMode") = EnumDashBoardType.Quiz
        Session("QuizUseTablet") = "True"
        IE = "1"
        _ExamNum = 1
#End If

        'Open Connection
        Dim connActivity As New SqlConnection
        _DB.OpenExclusiveConnect(connActivity)

        If Request.QueryString("TestsetID") Is Nothing Then ' Group SignalR
            GroupName = ClsActivity.GetGroupNameFromQuizId(Session("Quiz_Id").ToString(), connActivity) 'หา GroupName เพื่อ addGroup ให้ SignalR
            HttpContext.Current.Session("GroupName") = GroupName
        End If


#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("1"))
#End If

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        Else
            If Session("ChooseMode") Is Nothing Then
                'If Request.QueryString("DashBoardMode").ToString = "6" Then
                If Not Request.QueryString("DashBoardMode") Is Nothing Then ' Choosemode จากคอมพิวเตอร์มี querystring ถ้าครูฝึกฝนจาก dashboard ไมมี querystring
                    Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer
                    Session("PracticeMode") = True
                    Session("PracticeFromComputer") = True

                Else
                    Session("ChooseMode") = EnumDashBoardType.Practice ' ครูฝึกฝน
                End If
            End If
            If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                PracticeFromComputer = "True"
                PlayerId = Session("UserId").ToString()

            Else
                If Session("ChooseMode") = EnumDashBoardType.Practice Then
                    Session("PracticeMode") = True
                    Session("PracticeFromComputer") = True 'ถ้าครูเข้าทำฝึกฝนผ่าน dashboard ต้อง setsession เป็น true
                Else
                    Session("PracticeMode") = False
                    Session("PracticeFromComputer") = False
                End If
                PracticeFromComputer = "False"
            End If

            UserId = Session("UserId").ToString() 'sharePlayerId เปลีย่นใช้ตัวนี้แทน จะได้ลดตัวแปรลงไปบ้าง

            'Help
            ProcessHelpPanel()

            If HDNotReplyMode.Value = "True" Then 'ทำเพื่อเอาไปใช้ใน Function savescore เพื่อตรวจว่าควรจะเลื่อนตำแหน่ง Array มั้ย
                NotReplyMode = True
            End If

            If NotReplyMode = "True" Then
                If HDIsLeapChoice.Value = "True" Then
                    SetValueLeapChoice(0)
                End If
                If HDLastChoice.Value = "True" Then
                    SetValueLeapChoice(3)
                End If
                'MainDivPad.Attributes.Remove("Class")
                'MainDivPad.Attributes.Add("Class", "MainDivNotReply")
            Else
                'MainDiv.Attributes.Remove("Class")
                'MainDivPad.Attributes.Add("Class", "MainDivNormal")
                DivNotHaveDontReplyChoice.Style.Add("display", "none")
            End If


            If Not IsPostBack Then

                If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                    Dim IsUseTablet As String
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        IsUseTablet = "0"
                    Else
                        IsUseTablet = "1"
                    End If
                    'HttpContext.Current.Session("UserId") = player_Id
                    'Dim PtestsetId As String = ClsPracticeMode.GetTestsetId(QsetId, player_Id, SchoolCode, HttpContext.Current.Session("PClassId"))
                    'Dim testsetValue() As String = Split(PtestsetId, ",")
                    'If testsetValue(1) = "New" Then 'Testset ใหม่ต้องเอาข้อสอบใส่ Table TestsetQuestionSet และ TestsetQuestionDetail ด้วย
                    '    ClsPracticeMode.SetTSQSAndSetTSQD(testsetValue(0), QsetId)
                    'End If
                    Dim KNS As New KNAppSession()
                    If Session("Quiz_Id") Is Nothing Or (Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then 'ดักถ้าเป็นครูทำฝึกฝนตามเข้ามา ไม่ต้องไป save quiz อีกรอบ
                        Session("Quiz_Id") = SClsPractice.SaveQuizDetail(Request.QueryString("TestsetID"), Session("SchoolCode"), UserId, _
                                                                         IsUseTablet, False, "1", KNS.StoredValue("CurrentCalendarId").ToString(), Session("ChooseMode").ToString(), connActivity)
                        If Session("ChooseMode") = EnumDashBoardType.Quiz Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                            Dim ClsSelectSession As New ClsSelectSession() ' set session ให้ด้วย สำหรับการตามเข้ามาที่ quiz หรือ ฝึกฝนของครู
                            ClsSelectSession.SetSessionChooseMode(Session("ChooseMode"))
                            ClsSelectSession.SetSessionQuizId(Session("Quiz_Id"))
                        End If
                    End If
                    Session("QuizUseTablet") = SClsPractice.GetUseTablet(Session("Quiz_Id").ToString, connActivity)
                    'ClsActivity.getQsetInQuiz(PtestsetId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion
                    'If Not HttpContext.Current.Session("PracticeFromComputer") Then
                    'ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, HttpContext.Current.Session("PracticeFromComputer"), ClsPracticeMode.GetQSetTypeFromQSetId(QsetId))
                    'End If
                    'ClsPracticeMode.SaveQuiznswer(HttpContext.Current.Session("Quiz_Id"), HttpContext.Current.Session("UserId"), SchoolCode, HttpContext.Current.Session("PDeviceId"), HttpContext.Current.Session("PracticeFromComputer"))
                    'KnSession(HttpContext.Current.Session("Quiz_Id") & "|" & "SelfPace") = True
                    'If HttpContext.Current.Session("PracticeFromComputer") Then
                    '    HttpContext.Current.Session("SchoolId") = SchoolCode
                    '    HttpContext.Current.Session("QuizUseTablet") = "False"
                    '    Return "../Activity/ActivityPage.aspx"
                    'Else
                    '    Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")
                    'End If
                    'End If
                End If

                Session("PlayerId") = Session("UserId").ToString()

                Dim Quiz_Id As String = Session("Quiz_Id").ToString()
                Dim dtSetting As DataTable = ClsActivity.GetSetting(Quiz_Id, connActivity)
                'Dim testSetName As String
                'testSetName = ClsActivity.GetTestsetNamePC(Quiz_Id, Session("PracticeMode"), connActivity)
                TestsetName = ClsActivity.GetTestsetNamePC(Quiz_Id, Session("PracticeMode"), connActivity)
                lblTestsetName.Text = TestsetName
                lblSideText.Text = TestsetName
                ViewState("ToolsInQuiz") = dtSetting.Rows(0)("EnabledTools") 'Update 09-05-56 set value to viewstateTools

                If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                    PracticeFromComputer = True
                End If

#If IE = "1" Then
                Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("2"))
#End If

                If Not IsPostBack Then
                    If dtSetting.Rows(0)("IsUseTablet") IsNot DBNull.Value Then
                        Session("IsUseTablet") = dtSetting.Rows(0)("IsUseTablet")
                    End If
                    Session("NeedTimer") = dtSetting.Rows(0)("NeedTimer")
                    If dtSetting.Rows(0)("NeedTimer") = True AndAlso dtSetting.Rows(0)("IsPerQuestionMode") = True AndAlso dtSetting.Rows(0)("Selfpace") = False Then
                        Session("IsTimePerQuestion") = True
                    Else
                        Session("IsTimePerQuestion") = False
                    End If
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

                    HttpContext.Current.Session("SetAnswerStateTeacher") = False

                    Session("Selfpace") = dtSetting.Rows(0)("Selfpace")
                    ' If KNSession(Quiz_Id & "|" & "NeedShowScore") Is Nothing Then
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
                    'End If
                    'Dim MaxExamNum As String = ClsActivity.GetExamNum(Quiz_Id).ToString
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        If _ExamNum = Nothing Then
                            _ExamNum = 1
                        End If
                    Else
#If IE = "1" Then
                        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("3"))
                        _ExamNum = 1
                        checkStratTimer.Value = 1
                        checkFirstPage.Value = 1
#Else
                        Dim ClsSelectSession As New ClsSelectSession()
                        Dim MaxExamNum As Integer = ClsSelectSession.GetExamNum()
                        'If MaxExamNum = "" Then
                        If MaxExamNum = Nothing Then
                            _ExamNum = 1
                            ClsSelectSession.SetExamNum(1)
                        Else
                            _ExamNum = MaxExamNum
                            checkStratTimer.Value = 1
                        End If
#End If

                    End If

                    _ExamAmount = ClsActivity.GetExamAmount(Quiz_Id, connActivity)
                    Session("_ExamAmount") = _ExamAmount

#If IE = "1" Then
                    Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("4"))
#End If

                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

                    If Not ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace, connActivity) Then
                        ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"), connActivity)
                    End If

                    If Session("ChooseMode") = EnumDashBoardType.Quiz AndAlso Session("QuizUseTablet") = True Then
                        'check ว่า หลังจาก startquiz มาเปิด quiz ซ้ำกับครูคนอื่นหรือเปล่า
                        If HttpContext.Current.Session("QuizDuplicate") Is Nothing Then
                            Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
#If F5 = "1" Then
                            HttpContext.Current.Application.Lock()
#End If
                            context.Clients.Group(GroupName).send("Reload")
#If F5 = "1" Then
                            Dim t As New Dictionary(Of String, String)
                            t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                            HttpContext.Current.Application(Quiz_Id & "|Time") = t
                            HttpContext.Current.Application("ReloadTime") = Nothing
                            HttpContext.Current.Application.UnLock()
#End If
                        Else
                            HttpContext.Current.Session("QuizDuplicate") = Nothing
                            Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
                            Dim FName As String = Session("FirstName").ToString()
                            Dim LName As String = Session("LastName").ToString()
                            context.Clients.Group(GroupName).send("QuizDuplicate|" & FName & " " & LName)
#If F5 = "1" Then
                            Dim t As New Dictionary(Of String, String)
                            t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                            HttpContext.Current.Application(Quiz_Id & "|Time") = t
                            HttpContext.Current.Application("ReloadTime") = Nothing
#End If
                        End If

                        ' Set ค่าลง redis
                        If HttpContext.Current.Application(Quiz_Id & "_AllPlayer") IsNot Nothing Then
                            'HttpContext.Current.Application.Lock()

                            Dim dt As DataTable = HttpContext.Current.Application(Quiz_Id & "_AllPlayer")
                            'Dim dt As DataTable = redis.Getkey(Of DataTable)("PlayerInQuiz_" & Quiz_Id)
                            Dim q As Quiz = HttpContext.Current.Application("Quiz_" & Quiz_Id)
                            For Each r As DataRow In dt.Rows
                                q.PlayerId = r("Owner_id").ToString()
                                redis.SetKey(Of Quiz)(r("Tablet_SerialNumber"), q)
                                redis.Expire(r("Tablet_SerialNumber"), 900)
                                redis.SAdd(Quiz_Id, r("Tablet_SerialNumber"))
                            Next
                            redis.DEL("PlayerInQuiz_" & Quiz_Id)
                            HttpContext.Current.Application(Quiz_Id & "_AllPlayer") = Nothing
                            'HttpContext.Current.Application.UnLock()
                        End If
                    End If


#If IE = "1" Then
                    Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("5"))
#End If

                    UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บค่า AnswerState ไปใช้ที่หน้าเด็ก

                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then ' ขอใส่ใน if นี้นะครับ คิดว่าเป็น bug เวลา refresh หน้า แล้ว _ExamNum = 1 กระทบกับ SignalR
                        If HDIsLeapChoice.Value = "True" Then
                            _ExamNum = HDQQ_No.Value
                            If Not Page.IsPostBack Then
                                HDIsLeapChoice.Value = ""
                            End If
                        Else
                            If Not IsPostBack Then
                                _ExamNum = 1
                            End If
                        End If
                    End If


                    mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, UserId, _AnswerState, _ExamNum, Session("Selfpace"), connActivity)
                    Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, UserId, _ExamNum, ViewIntroQsetId, connActivity)

#If IE = "1" Then
                    Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("6"))
#End If
                    'Return tagIntro : Type : LimitAmount : ViewIntroQsetId
                    If DataIntro <> "" Then
                        Dim ArrIntro() As String = Split(DataIntro, "@:@")
                        Dim TagIntro As String = ArrIntro(0)
                        Dim TypeIntro As String = ArrIntro(1)
                        Dim LimitAmount As String = ArrIntro(2)
                        ViewIntroQsetId = ArrIntro(3)
                        ViewLimitAmount.InnerHtml = LimitAmount
                        If TypeIntro = "2" Then
                            mainIntro.InnerHtml = TagIntro
                        Else
                            mainIntro.InnerHtml = "<span id=""spnTest"">Clickkkkkkkkkkkkkkkkkkkk</span>"""
                            introHtml.InnerHtml = TagIntro
                        End If
                        EnableIntro = True
                    Else
                        EnableIntro = False
                    End If

                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(UserId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, True, connActivity)
                    Else
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(UserId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, False, connActivity)
                    End If

#If IE = "1" Then
                    Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("7"))
#End If

                    MyAnswer = ClsActivity.MyAnswer
                    CorrectAnswer = ClsActivity.CorrectAnswer

                    Dim IsHaveCurrentQuizState As Boolean = False
                    If Not HttpContext.Current.Application(Quiz_Id & "_CurrentQuizState") Is Nothing Then
                        Dim ArrHastable = CType(HttpContext.Current.Application(Quiz_Id & "_CurrentQuizState"), Hashtable)
                        IsHaveCurrentQuizState = ArrHastable.ContainsKey(Quiz_Id.ToString().ToUpper() & "_CurrentQuestionNo")
                    End If
                    If Not IsHaveCurrentQuizState Then
                        questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum, connActivity)
                        hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, connActivity).ToString()
                        UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Quiz_Id, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
                        UpdateLastUpdateWhenNextOrPrev(Quiz_Id, questionId, connActivity) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
                    Else
                        checkFirstPage.Value = 1
                        CallStudentFirst.Value = 1
                    End If

                    lblNoExam.Text = _ExamNum & " / " & _ExamAmount
                    lblRunNoExam.Text = _ExamNum & " / " & _ExamAmount
                    lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

                    If _ExamNum <> 1 Then
                        'Dim positionImg As Integer = ((490 / _ExamAmount) * _ExamNum)
                        'imgRun.Style.Add("left", CStr(positionImg) + "px;")
                        'lblRunNoExam.Style.Add("margin-left", CStr(positionImg + 113) + "px;")
                        SetPositionRun(_ExamAmount, _ExamNum)
                    Else
                        lblRunNoExam.Style.Add("margin-left", "113px;")
                    End If
                    'Dim positionImg As Integer = 152 + ((570 / _ExamAmount) * _ExamNum)
                    'Dim positionImg As Integer = (500 / _ExamAmount) * (_ExamNum - 1)
                    'imgRun.Style.Add("left", CStr(positionImg) + "px;")
                    'If Session("QuizUseTablet") = True Then
                    '    ClsActivity.SetAnswer(Session("Quiz_Id"), Session("SchoolId"), questionId)
                    'End If

                    ' save คำตอบที่ checkmark2 temptblChoice
                    If (Session("QuizUseTamplate") = True) Then
                        Dim dtAnswer As DataTable = ClsActivity.GetCorrectAnswerDetail(questionId, Quiz_Id, connActivity)
                        For j As Integer = 0 To dtAnswer.Rows.Count - 1
                            If (dtAnswer.Rows(j)("Answer_Score") = "1") Then
                                Dim ChkMark As New ClsCheckMark ' set โจทย์และคำตอบไปยัง temptblChoice
                                ChkMark.saveCorrectAnswerToCheckmark(_ExamNum, (j + 1))
                                Exit For
                            End If
                        Next
                    End If

                    hdReplyMode.Value = If(dtSetting.Rows(0)("IsTimeShowCorrectAnswer") = True, 1, 4)
                End If

#If IE = "1" Then
                Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("8"))
#End If


                If Session("QuizUseTablet") = True Then ' เช็คดูว่าใช้แทปเล็ตในการสอบหรือเปล่า
                    'SizeWidthForDivs = 180
                    SideMenuDiv = 155
                    If (_ExamAmount.Length = 1) Then ' เช็คดูว่าเลขจำวนวข้อสอบตรงเมนูเลื่อนข้อ เป็นเลขกี่หลัก
                        WidthDivExamAmount = 170
                        If (Session("NeedTimer") = True) Then ' เช็คดูว่ามีการจับเวลาในการทำควิซหรือไม่  
                            SizeWidthForDivs = 330
                        Else
                            SizeWidthForDivs = 308
                        End If
                    ElseIf (_ExamAmount.Length = 2) Then
                        WidthDivExamAmount = 195
                        If (Session("NeedTimer") = True) Then
                            SizeWidthForDivs = 307
                        Else
                            SizeWidthForDivs = 284
                        End If
                    Else
                        WidthDivExamAmount = 225
                        If (Session("NeedTimer") = True) Then
                            SizeWidthForDivs = 277
                        Else
                            SizeWidthForDivs = 252
                        End If
                    End If
                ElseIf Session("PracticeMode") Then
                    SideMenuDiv = 260
                    WidthDivExamAmount = 240 'Div ใส่ปุ่ม Next,Previous
                    '22/5/58 ไหมเพิ่ม if เพื่อปรับให้เต็ม Panel ถ้าต่อไปเปิด Panel ข้อข้ามขึ้นมาใช้ให้เอา if ออก ใช้ 435 เหมือนกัน
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        SizeWidthForDivs = 540
                    Else
                        SizeWidthForDivs = 435
                    End If

                Else
                    'SizeWidthForDivs = 415
                    SideMenuDiv = 260
                    If (_ExamAmount.Length = 1) Then
                        WidthDivExamAmount = 170
                        If (Session("NeedTimer") = True) Then
                            SizeWidthForDivs = 530
                        Else
                            SizeWidthForDivs = 507
                        End If
                    ElseIf (_ExamAmount.Length = 2) Then
                        WidthDivExamAmount = 195
                        If (Session("NeedTimer") = True) Then
                            SizeWidthForDivs = 505
                        Else
                            SizeWidthForDivs = 483
                        End If
                    Else
                        WidthDivExamAmount = 225
                        If (Session("NeedTimer") = True) Then
                            SizeWidthForDivs = 475
                        Else
                            SizeWidthForDivs = 452
                        End If
                    End If
                End If

                HDCheckChangeQuestion.Value = "Reload" 'กำหนดค่าให้ HiddenField มีค่าเป็นคำว่า Reload เพื่อส่ง SignalR

            End If

        End If

        CurrentQuizId = HttpContext.Current.Session("Quiz_Id").ToString()

        qsetFilePath = (ClsActivity.GetQsetIDFromExamNum(_ExamNum, CurrentQuizId)).ToFolderFilePath()

        shareSelfPace = VBIsSelfPace

        'SwapStatus = ClsActivity.SwapStatus
        getToolsInQuiz(ViewState("ToolsInQuiz")) 'Update 09-05-56 set variable Tools
        If Not IsPostBack Then
            HttpContext.Current.Session("StartTimeQuestion") = Date.Now
            ''Else
            ''    HttpContext.Current.Session("StartTimeQuestion") = HttpContext.Current.Session("EndTimeQuestion")
        End If

        IsTimePerQuestion = Session("IsTimePerQuestion")

        'If Session("StartTime") = True Then
        '    If Session("t") Is Nothing Then
        '        Dim t As New System.Timers.Timer
        '        Session("t") = t
        '        t.Interval = 10000
        '        AddHandler t.Elapsed, AddressOf Timeup
        '        t.Enabled = True
        '        t.Start()
        '        Dim s As New ClsSelectSession()
        '        s.SetStartTimer()
        '        s.SetT()
        '    End If
        'End If

        'Close Connection

        If _ExamAmount = 1 Then
            Dialog = "True"
        End If

        SetAttributeQsetId(connActivity)

        _DB.CloseExclusiveConnect(connActivity)

#If IE = "1" Then
        Elmah.ErrorSignal.FromCurrentContext().Raise(New Exception("9-END"))
#End If


    End Sub

    Private Sub SetAttributeQsetId(con As SqlConnection)
        If questionId IsNot Nothing AndAlso questionId <> "" Then
            Dim qsetId As String = _DB.ExecuteScalar("SELECT QSet_Id FROM tblQuestion WHERE Question_Id = '" & questionId & "';", con)
            mainQuestion.Attributes.Add("qsetId", qsetId)
            mainQuestion.Attributes.Add("questionId", questionId)
        End If
    End Sub

    Private Sub SetValueLeapChoice(ByVal ActionMode As String, Optional ByRef InputConn As SqlConnection = Nothing) '0=กดมาจาก Panel,1=กดมาจากปุ่ม next,2กดมาจากปุ่ม Back

        If ActionMode = 3 Then
            HDNotReplyMode.Value = "False"
            NotReplyMode = "False"
            _ExamNum = CStr(CInt(ClsActivity.GetNextChoiceAfterReply(Session("Quiz_Id").ToString(), PlayerId, InputConn)) - 1)

            'MainDivPad.Attributes.Remove("Class")
            'MainDivPad.Attributes.Add("Class", "MainDivNormal")
            controlBtnNext(False)
            Exit Sub
        Else

            'ถ้าทวนข้อข้าม 
            ' สร้าง Array ข้อข้ามก่อน
            Dim ArrNotReply As New ArrayList
            Dim dt As DataTable = ClsActivity.GetNotReplyNum(Session("Quiz_Id").ToString(), PlayerId, InputConn)
            If Not dt.Rows.Count = 0 Then

                If dt.Rows.Count = 1 Then
                    HDLastChoice.Value = True
                End If

                If Not dt.Rows.Count = 0 Then
                    Dim i As Integer = 0
                    For Each a In dt.Rows
                        ArrNotReply.Add(a("QQ_No").ToString)
                        If ActionMode = 0 Then
                            If a("QQ_No").ToString = HDQQ_No.Value.ToString Then
                                Session("PositionArr") = i
                            End If
                        End If
                        i += 1
                    Next

                    If ActionMode = 1 Then

                        If Not Session("IsReplyAnswer") = True Then
                            Session("PositionArr") += 1
                        End If

                        If Session("PositionArr") > ArrNotReply.ToArray.Length - 1 Then
                            Session("PositionArr") = 0
                        End If
                    ElseIf ActionMode = 2 Then
                        Session("PositionArr") -= 1
                        If Session("PositionArr") < 0 Then
                            Session("PositionArr") = ArrNotReply.ToArray.Length - 1
                        End If
                    End If
                End If
            Else
                'ArrNotReply.Add(ClsActivity.GetNextChoiceAfterReply(VQuizId, PlayerId).ToString)
                'Session("PositionArr") = 0
                HDNotReplyMode.Value = "False"
                NotReplyMode = "False"
                _ExamNum = CStr(CInt(ClsActivity.GetNextChoiceAfterReply(Session("Quiz_Id").ToString(), PlayerId, InputConn)) - 1)

                'MainDivPad.Attributes.Remove("Class")
                'MainDivPad.Attributes.Add("Class", "MainDivNormal")
                If ClsActivity.CountLeapExam(Session("Quiz_Id").ToString(), PlayerId, InputConn) <> GetTopQQNoByQuizId(Session("Quiz_Id").ToString(), InputConn) Then
                    DivNotHaveDontReplyChoice.Style.Add("display", "block")
                End If

                controlBtnNext(False)
                Exit Sub
            End If

            Dim CurrentAnsState As String = ""
            Dim CurrentExamNum As String = ArrNotReply(Session("PositionArr"))
            If _AnswerState = "0" Then
                CurrentAnsState = "0"

            Else
                If HDIsScore.Value = "False" Then
                    CurrentAnsState = "1"
                    _AnswerState = "1"
                ElseIf HDIsScore.Value = "True" Then
                    CurrentAnsState = "2"
                    _AnswerState = "2"
                End If
                HDIsScore.Value = Nothing
                HDQQ_No.Value = Nothing
            End If
            'ต้อง Render เลย เพราะแสดงว่ากดมาจากหน้าจอข้ามข้อ

            'renderExamSelfPace(CurrentExamNum, CurrentAnsState, InputConn)
            'GenPanelShowInfo(PlayerId, VQuizId, CurrentExamNum, , InputConn)
            _ExamNum = CurrentExamNum - 1
            controlBtnNext(True)
            Session("IsReplyAnswer") = False
            HDIsLeapChoice.Value = "False" 'Render เสร็จต้องเปลี่ยนค่ากลับเป็น False ไม่งั้นเดียวจะเข้า Function นี้ทุกครั้ง
        End If

        If HDLastChoice.Value = "True" Then

            HDLastChoice.Value = "False"
        End If

    End Sub

    Private Function GetTopQQNoByQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim QQNo As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return QQNo
        End If
        Dim sql As String = " SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' ORDER BY QQ_No DESC "

        QQNo = UseCls.ExecuteScalar(sql, InputConn)
        Return QQNo
    End Function


#Region "เcode เผื่ออนาคต"
    'Private Sub Timeup(sender As Object, e As System.Timers.ElapsedEventArgs)
    '    'btnNext_Click(btnNextTop, ImageClickEventArgs.Empty)
    '    'btnNext_Click(btnNext, Nothing)
    '    Dim t = Session("t")
    '    t.Stop()
    '    TestNext()
    '    Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
    '    'context.Clients.Group(Session("selectedSession").ToString()).send("Reload")
    '    context.Clients.Group(GroupName).send("Reload")
    '    t.Start()

    'End Sub

    'Private Sub TestNext()
    '    Dim Quiz_Id As String = Session("Quiz_Id").ToString()
    '    Dim PlayerId As String = Session("PlayerId").ToString() ' = Session("UserId").ToString ด้วย 
    '    Dim PlayerType As String = ClsActivity.GetPlayerType(PlayerId, Quiz_Id)
    '    Dim IsTeacher As Boolean
    '    If PlayerType = "1" Then
    '        IsTeacher = True
    '    End If
    '    'Dim HaveQuestion As Boolean
    '    'Dim HaveIsScored As Boolean
    '    Session("EndTimeQuestion") = Date.Now
    '    ClsActivity.SetTotalTime(Session("StartTimeQuestion"), Session("EndTimeQuestion"), Quiz_Id, _ExamNum, PlayerId)
    '    ClsActivity.SetTotalScore(Quiz_Id, PlayerId)
    '    '**************
    '    'เช็คก่อนว่า มีข้อสอบยัง ถ้ายังแสดงว่ายังไม่ได้เล่นข้อนี้

    '    If (Session("showAnswer") = True) Then
    '        If (Session("ShowCorrectAfterComplete") = True) Then
    '            If (_ExamNum + 1 = _ExamAmount) Then
    '                Dialog = "True"
    '            End If
    '        ElseIf (_ExamNum = _ExamAmount) And _AnswerState = "1" Then
    '            Dialog = "True"
    '        End If
    '    End If
    '    If Session("SetAnswerStateTeacher") = True Then
    '        _ExamNum = Session("ExamNum")
    '        _AnswerState = Session("_AnswerState")
    '        Session("ShowCorrectAfterCompleteState") = True
    '        Session("SetAnswerStateTeacher") = False
    '    End If


    '    If _AnswerState = "1" Then
    '        _AnswerState = "2"
    '        If Not Application(Quiz_Id & "|" & "SelfPace") Then
    '            ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum) 'Function UpdateIsscore ExamNum
    '        Else
    '            ClsActivity.UpdateIsScoredTeacher(Quiz_Id, _ExamNum)
    '        End If
    '    Else 'State <> 1
    '        _ExamNum += 1 'ตอนขึ้นข้อใหม่
    '        If Session("ChooseMode") = EnumDashBoardType.Quiz Or Session("ChooseMode") = EnumDashBoardType.Practice Then
    '            Dim ClsSelectSession As New ClsSelectSession(PlayerId, Session("SchoolId").ToString(), Session("selectedSession"), Application(PlayerId))
    '            ClsSelectSession.SetExamNum(_ExamNum)
    '        End If
    '        questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum)
    '        hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId).ToString() 'เอาใช้ใน tools

    '        'If VBIsSelfPace = False Or Session("ChooseMode") = EnumDashBoardType.Practice Then
    '        UpdateLastUpdateWhenNextOrPrev(Quiz_Id, questionId) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
    '        'UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Quiz_Id, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
    '        HDCheckChangeQuestion.Value = "Reload"



    '        If _AnswerState <> "0" Then
    '            If Not Session("ShowCorrectAfterCompleteState") Then
    '                _AnswerState = "1"
    '            End If
    '            If Not Application(Quiz_Id & "|" & "SelfPace") Then
    '                'Function UpdateIsscore ExamNum
    '                'ถ้าเป็นแสดงคะแนนแบบข้อต่อข้อ เข้าไป Update IsScore
    '                If VBNeedShowScoreChoiceToChoice = True Then
    '                    ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum)
    '                End If
    '            End If
    '        End If
    '        'มีข้อสอบมั้ย
    '        If ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
    '            'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
    '            If Application(Quiz_Id & "|" & "SelfPace") Then
    '                'ถ้าไปไม่พร้อมกัน ให้เช็คว่าเป็นฝึกฝนจากคอมมั้ย
    '                If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
    '                    'เป็นฝึกฝนจากคอมให้ SetQuizScore
    '                    If _AnswerState <> "2" And Not ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace) Then
    '                        ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"), False, Application(""), Application(""))
    '                    End If
    '                Else
    '                    'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
    '                    If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum) Then
    '                        'ถ้าตรวจแล้ว 
    '                        If _AnswerState <> "0" Then
    '                            _AnswerState = "2"
    '                        End If
    '                    End If
    '                    If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum) Then
    '                        If _AnswerState <> "0" Then
    '                            _AnswerState = "2"
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum) Then
    '                    'ถ้าตรวจแล้ว
    '                    If _AnswerState <> "0" Then
    '                        _AnswerState = "2"
    '                    End If
    '                End If
    '            End If
    '        Else
    '            If _AnswerState <> "2" Then
    '                ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"), False, Application(Quiz_Id & "|" & "SelfPace"), Application(Quiz_Id & "|" & "IsDifferentQuestion"), Application(Quiz_Id & "|" & "CheckInStrquestionId"))
    '                ' save คำตอบที่ checkmark2 temptblChoice // รอปรับแก้ด้วย ต้องเป็นควิซไม่สลับคำถามและคำตอบ
    '                If (Session("QuizUseTamplate") = True) Then
    '                    Dim dtAnswer As DataTable = ClsActivity.GetCorrectAnswerDetail(questionId, Quiz_Id)
    '                    For j As Integer = 0 To dtAnswer.Rows.Count - 1
    '                        If (dtAnswer.Rows(j)("Answer_Score") = "1") Then
    '                            Dim ChkMark As New ClsCheckMark ' set โจทย์และคำตอบไปยัง temptblChoice
    '                            ChkMark.saveCorrectAnswerToCheckmark(_ExamNum, (j + 1))
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '            End If
    '            If Application(Quiz_Id & "|" & "SelfPace") Then
    '                If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum) Then
    '                    If _AnswerState <> "0" Then
    '                        _AnswerState = "2"
    '                    End If
    '                End If
    '            End If
    '            If _AnswerState = "0" And Not Application(Quiz_Id & "|" & "NeedShowScoreAfterComplete") And VBNeedShowScoreChoiceToChoice = True Then
    '                If Not Application(Quiz_Id & "|" & "SelfPace") Then
    '                    'Function UpdateIsscore ExamNum - 1
    '                    ClsActivity.UpdateIsScore(Quiz_Id, CStr(CInt(_ExamNum) - 1))
    '                End If
    '            End If
    '        End If
    '    End If

    '    'Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
    '    'context.Clients.Group(GroupName).send("Reload")

    '    'UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ

    '    If _AnswerState = 2 And Session("PracticeFromcomputer") Then
    '        If ClsActivity.GetNotAnswer(Quiz_Id, PlayerId, _ExamNum) Then '
    '            IsNoAnswer = True
    '        Else
    '            IsNoAnswer = False
    '        End If
    '    Else
    '        IsNoAnswer = False
    '    End If


    '    'If (_ExamNum = _ExamAmount) And (_AnswerState = "0") Then
    '    '    If Session("ShowCorrectAfterComplete") = True Then
    '    '        Dialog = "False"
    '    '    Else
    '    '        Dim LeapExam As String = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
    '    '        DialogTitle = setTitleDialog(LeapExam, IsTeacher)
    '    '        Dialog = "True"
    '    '    End If
    '    'ElseIf (_ExamNum = _ExamAmount) And (_AnswerState = "2") Then
    '    '    Dim LeapExam As String = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
    '    '    DialogTitle = setTitleDialog(LeapExam, IsTeacher)
    '    '    Dialog = "True"
    '    'Else
    '    '    Dialog = "False"
    '    'End If

    '    Session("StartTimeQuestion") = Session("EndTimeQuestion")
    '    ClsActivity.TeacherSetTotalScore(Quiz_Id)

    'End Sub

    <Services.WebMethod()>
    Public Shared Function StartTimer()
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        HttpContext.Current.Session("StartTime") = True
        Return 1
    End Function
#End Region

    Private Sub controlBtnNext(IsFirstLeavechoice As Boolean)
        'Open Connection
        Dim connActivity As New SqlConnection
        _DB.OpenExclusiveConnect(connActivity)

        If NotReplyMode = "True" And IsFirstLeavechoice = False Then

            SetValueLeapChoice(1)
            Exit Sub

        End If

        Dim Quiz_Id As String = Session("Quiz_Id").ToString()
        Dim PlayerId As String = Session("PlayerId").ToString() ' = Session("UserId").ToString ด้วย 
        Dim PlayerType As String = ClsActivity.GetPlayerType(PlayerId, Quiz_Id, connActivity)
        Dim IsTeacher As Boolean
        If PlayerType = "1" Then
            IsTeacher = True
        End If
        'Dim HaveQuestion As Boolean
        'Dim HaveIsScored As Boolean
        Session("EndTimeQuestion") = Date.Now
        ClsActivity.SetTotalTime(HttpContext.Current.Session("StartTimeQuestion"), HttpContext.Current.Session("EndTimeQuestion"), Quiz_Id, _ExamNum, PlayerId, connActivity)
        'ClsActivity.SetTotalScore(Quiz_Id, PlayerId, connActivity)
        '**************
        'เช็คก่อนว่า มีข้อสอบยัง ถ้ายังแสดงว่ายังไม่ได้เล่นข้อนี้

      
        ' ขอตัด havequestion ออกก่อนนะครับ And (ClsActivity.HaveQuestion(Quiz_Id, _ExamNum)) 'เพิ่ม And HaveQuestion ด้วย เพื่อให้รู้ว่าเคยมาถึงข้อนี้มั้ย
        'If (_ExamNum = _ExamAmount) And (_AnswerState = "0" Or _AnswerState = "2") Then
        '    'Dim LeapExam As String = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
        '    'ShowDialogState = "True"
        '    'If LeapExam = "0" Then
        '    '    If Session("ChooseMode") = EnumDashBoardType.Quiz Or Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
        '    '        DialogTitle = "ทำฝึกฝนหมดข้อสุดท้ายแล้วค่ะ ดูคะแนนเลยไหมคะ ?"
        '    '    Else
        '    '        DialogTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
        '    '    End If

        '    'Else
        '    '    'If IsTeacher Then
        '    '    DialogTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
        '    '    'Else
        '    '    '    DialogTitle = "ยังไม่ได้ทำข้อสอบอีก " & LeapExam & " ข้อ จบควิซเลยไหมคะ ?"
        '    '    'End If
        '    'End If
        '    Dialog = "True"

        'Dialog = "False"
        If Session("SetAnswerStateTeacher") = True Then
            _ExamNum = Session("ExamNum")
            _AnswerState = Session("_AnswerState")
            Session("ShowCorrectAfterCompleteState") = True
            Session("SetAnswerStateTeacher") = False
        End If

        If _AnswerState = "1" Then
            _AnswerState = "2"

            'ถ้าเป็นโหมดไม่ใช้ Tablet และ เฉลยข้อต่อข้อ ต้องเอาข้อที่เฉลยแล้วไป Add ใน Array ให้ด้วย เพื่อเอาไปเฃ็คตอนกด Next ครั้งต่อไปว่าข้อนี้เฉลยแล้วจะได้เปลี่ยน AnswerState เป็น 2
            If Session("IsUseTablet") IsNot Nothing Then
                If Session("IsUseTablet") = False And _AnswerState <> 0 Then
                    ArrExamNumIsPassAlready = ClsActivity.CheckAndAddValueToArrayExamNum(ArrExamNumIsPassAlready, _ExamNum)
                End If
            End If

            If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum, , connActivity) 'Function UpdateIsscore ExamNum
            Else
                ClsActivity.UpdateIsScoredTeacher(Quiz_Id, _ExamNum, connActivity)
            End If

            mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, PlayerId, _AnswerState, _ExamNum, Session("Selfpace"), connActivity)

            If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, True, connActivity)
            Else
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, False, connActivity)
            End If

            MyAnswer = ClsActivity.MyAnswer
            CorrectAnswer = ClsActivity.CorrectAnswer

            AnswerExp.InnerHtml = ClsActivity.htmlAnswerExp

            lblNoExam.Text = _ExamNum & " / " & _ExamAmount
            lblRunNoExam.Text = _ExamNum & " / " & _ExamAmount
            lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

            SetPositionRun(_ExamAmount, _ExamNum)

            'Dim positionImg As Integer = ((490 / _ExamAmount) * _ExamNum)
            ''Dim positionImg As Integer = (500 / _ExamAmount) * (_ExamNum - 1)
            'imgRun.Style.Add("left", CStr(positionImg) + "px;")
            'lblRunNoExam.Style.Add("margin-left", CStr(positionImg + 113) + "px;")
            'Send signalr
            If (Not Session("ChooseMode") = EnumDashBoardType.Practice) And (Not Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
                If Session("QuizUseTablet") = True Then
                    Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
                    HttpContext.Current.Application.Lock()
                    Dim q As Quiz = HttpContext.Current.Application("Quiz_" & Quiz_Id)
                    q.Examnum = _ExamNum
                    q.AnswerState = _AnswerState
                    HttpContext.Current.Application("Quiz_" & Quiz_Id) = q
                    HttpContext.Current.Application.UnLock()
#If F5 = "1" Then
                    HttpContext.Current.Application.Lock()
                    Dim t As New Dictionary(Of String, String)
                    t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                    HttpContext.Current.Application(Quiz_Id & "|Time") = t
                    HttpContext.Current.Application("ReloadTime") = Nothing
                    HttpContext.Current.Application.UnLock()
#End If
                    context.Clients.Group(GroupName).send("Reload")
                    '#If F5 = "1" Then
                    '                    Dim t As New Dictionary(Of String, String)
                    '                    t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                    '                    HttpContext.Current.Application(Quiz_Id & "|Time") = t
                    '                    HttpContext.Current.Application("ReloadTime") = Nothing
                    '                    HttpContext.Current.Application.UnLock()
                    '#End If
                    UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
                End If
            End If

        Else 'State <> 1
            If (_ExamNum + 1) <= _ExamAmount Then
                _ExamNum += 1 'ตอนขึ้นข้อใหม่
                If Session("ChooseMode") = EnumDashBoardType.Quiz Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                    Dim ClsSelectSession As New ClsSelectSession()
                    ClsSelectSession.SetExamNum(_ExamNum)
                End If
                questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum, connActivity)
                If Not questionId = "" Then
                    hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, connActivity).ToString() 'เอาใช้ใน tools
                End If

                'If VBIsSelfPace = False Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                UpdateLastUpdateWhenNextOrPrev(Quiz_Id, questionId, connActivity) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
                UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Quiz_Id, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
                HDCheckChangeQuestion.Value = "Reload"

                If _AnswerState <> "0" Then
                    If Not Session("ShowCorrectAfterCompleteState") Then
                        If Session("IsUseTablet") IsNot Nothing Then
                            'ถ้าเป็นโหมดไม่ใช้ Tablet แล้วก็เฉลย ข้อต่อข้อ ต้องไปหาค่าจาก Array ก่อนว่่าข้อสอบที่กำลังจะไปเคยเฉลยไปยัง ถ้าเฉลยไปแล้ว AnswerState ต้องเป็น 2
                            If Session("IsUseTablet") = False And _AnswerState <> 0 Then
                                If ClsActivity.CheckExamNumIsContainInArray(ArrExamNumIsPassAlready, _ExamNum) = True Then
                                    _AnswerState = "2"
                                Else
                                    _AnswerState = "1"
                                End If
                            Else
                                _AnswerState = "1"
                            End If
                        Else
                            _AnswerState = "1"
                        End If
                    End If
                    If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                        'Function UpdateIsscore ExamNum
                        'ถ้าเป็นแสดงคะแนนแบบข้อต่อข้อ เข้าไป Update IsScore
                        If VBNeedShowScoreChoiceToChoice = True Then
                            ClsActivity.UpdateIsScore(Quiz_Id, _ExamNum, , connActivity)
                        End If
                    End If
                End If
                'มีข้อสอบมั้ย
                If ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace, connActivity) Then
                    'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
                    If KNSession(Quiz_Id & "|" & "SelfPace") Then
                        'ถ้าไปไม่พร้อมกัน ให้เช็คว่าเป็นฝึกฝนจากคอมมั้ย
                        If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                            'เป็นฝึกฝนจากคอมให้ SetQuizScore
                            If _AnswerState <> "2" And Not ClsActivity.HaveQuestion(Quiz_Id, _ExamNum, VBIsSelfPace, connActivity) Then
                                ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"), connActivity)
                            End If
                        Else
                            'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
                            If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum, connActivity) Then
                                'ถ้าตรวจแล้ว 
                                If _AnswerState <> "0" Then
                                    _AnswerState = "2"
                                End If
                            End If
                            If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum, connActivity) Then
                                If _AnswerState <> "0" Then
                                    _AnswerState = "2"
                                End If
                            End If
                        End If
                    Else
                        If ClsActivity.HaveIsScored(Quiz_Id, _ExamNum, connActivity) Then
                            'ถ้าตรวจแล้ว
                            If _AnswerState <> "0" Then
                                _AnswerState = "2"
                            End If
                        End If
                    End If
                Else
                    If _AnswerState <> "2" Then
                        ClsActivity.SetQuizScore(_ExamNum, Quiz_Id, Session("PracticeMode"), Session("PracticeFromComputer"), connActivity)
                        ' save คำตอบที่ checkmark2 temptblChoice // รอปรับแก้ด้วย ต้องเป็นควิซไม่สลับคำถามและคำตอบ
                        If (Session("QuizUseTamplate") = True) Then
                            Dim dtAnswer As DataTable = ClsActivity.GetCorrectAnswerDetail(questionId, Quiz_Id, connActivity)
                            For j As Integer = 0 To dtAnswer.Rows.Count - 1
                                If (dtAnswer.Rows(j)("Answer_Score") = "1") Then
                                    Dim ChkMark As New ClsCheckMark ' set โจทย์และคำตอบไปยัง temptblChoice
                                    ChkMark.saveCorrectAnswerToCheckmark(_ExamNum, (j + 1))
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                    If KNSession(Quiz_Id & "|" & "SelfPace") Then
                        If ClsActivity.HaveIsScoredTeacher(Quiz_Id, _ExamNum, connActivity) Then
                            If _AnswerState <> "0" Then
                                _AnswerState = "2"
                            End If
                        End If
                    End If
                    If _AnswerState = "0" And Not KNSession(Quiz_Id & "|" & "NeedShowScoreAfterComplete") And VBNeedShowScoreChoiceToChoice = True Then
                        If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                            'Function UpdateIsscore ExamNum - 1
                            ClsActivity.UpdateIsScore(Quiz_Id, CStr(CInt(_ExamNum) - 1), , connActivity)
                        End If
                    End If
                End If


                If (Not Session("ChooseMode") = EnumDashBoardType.Practice) And (Not Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
                    If Session("QuizUseTablet") = True Then
                        Dim dt As New DataTable()
                        dt.Columns.Add("question_id", GetType(String))
                        dt.Columns.Add("QQ_No", GetType(Integer))
                        dt.Rows.Add(questionId, _ExamNum)
                        HttpContext.Current.Application.Lock()
                        HttpContext.Current.Application(Quiz_Id & "|QuestionId_QQ_No") = dt
                        Dim q As Quiz = HttpContext.Current.Application("Quiz_" & Quiz_Id)
                        q.Examnum = _ExamNum
                        q.AnswerState = _AnswerState
                        HttpContext.Current.Application("Quiz_" & Quiz_Id) = q
                        HttpContext.Current.Application.UnLock()

                        Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
#If F5 = "1" Then
                        HttpContext.Current.Application.Lock()
#End If
                        context.Clients.Group(GroupName).send("Reload")
#If F5 = "1" Then
                        Dim t As New Dictionary(Of String, String)
                        t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                        HttpContext.Current.Application(Quiz_Id & "|Time") = t
                        HttpContext.Current.Application("ReloadTime") = Nothing
                        HttpContext.Current.Application.UnLock()
#End If
                        UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ
                        SetQuizExpire() 'set อายุของ redis device
                    End If
                End If

                'Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, Session("PlayerId").ToString, _ExamNum, ViewIntroQsetId)
                'If DataIntro <> "" Then
                '    Dim ArrDAtaIntro = Split(DataIntro, ":")
                '    'IntroDetail.InnerHtml = ArrDAtaIntro(0)"
                '    Dim arrConvertIntro = ConvertStringIntro(ArrDAtaIntro(0))
                '    srcFileIntro = arrConvertIntro(0)
                '    typeFileIntro = arrConvertIntro(1)
                '    ViewIntroQsetId = ArrDAtaIntro(1)
                '    ViewAmount = ArrDAtaIntro(2)
                '    ViewLimitAmount.InnerHtml = ArrDAtaIntro(2)
                '    EnableIntro = True
                'Else
                'EnableIntro = False
                ''End If

                'Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, Session("PlayerId").ToString, _ExamNum, ViewIntroQsetId)
                ''Return tagIntro : Type : LimitAmount : ViewIntroQsetId
                'If DataIntro <> "" Then
                '    Dim ArrIntro() As String = Split(DataIntro, "@:@")
                '    Dim TagIntro As String = ArrIntro(0)
                '    Dim TypeIntro As String = ArrIntro(1)
                '    Dim LimitAmount As String = ArrIntro(2)
                '    ViewIntroQsetId = ArrIntro(3)

                '    ViewLimitAmount.InnerHtml = LimitAmount

                '    If TypeIntro = "2" Then
                '        mainIntro.InnerHtml = TagIntro
                '    Else
                '        mainIntro.InnerHtml = "<span id=""spnTest"">Clickkkkkkkkkkkkkkkkkkkk</span>"""
                '        introHtml.InnerHtml = TagIntro
                '    End If

                '    EnableIntro = True
                'Else
                '    EnableIntro = False
                'End If

                mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, PlayerId, _AnswerState, _ExamNum, Session("Selfpace"), connActivity)

                If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                    AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, True, connActivity)
                Else
                    AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, False, connActivity)
                End If

                MyAnswer = ClsActivity.MyAnswer
                CorrectAnswer = ClsActivity.CorrectAnswer

                AnswerExp.InnerHtml = ClsActivity.htmlAnswerExp

                'If _AnswerState = 2 And Session("PracticeFromcomputer") Then
                '    If ClsActivity.GetNotAnswer(Quiz_Id, PlayerId, _ExamNum, connActivity) Then '
                '        IsNoAnswer = True
                '    Else
                '        IsNoAnswer = False
                '    End If
                'Else
                '    IsNoAnswer = False
                'End If
                IsNoAnswer = False

                lblNoExam.Text = _ExamNum & " / " & _ExamAmount
                lblRunNoExam.Text = _ExamNum & " / " & _ExamAmount
                lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

                SetPositionRun(_ExamAmount, _ExamNum)

                'Dim positionImg As Integer = ((490 / _ExamAmount) * _ExamNum)
                ''Dim positionImg As Integer = (500 / _ExamAmount) * (_ExamNum - 1)
                'imgRun.Style.Add("left", CStr(positionImg) + "px;")
                'lblRunNoExam.Style.Add("margin-left", CStr(positionImg + 113) + "px;")

                'If (_ExamNum = _ExamAmount) And (_AnswerState = "0") Then
                '    If Session("ShowCorrectAfterComplete") = True Then
                '        Dialog = "False"
                '    Else
                '        Dim LeapExam As String = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
                '        DialogTitle = setTitleDialog(LeapExam, IsTeacher)
                '        Dialog = "True"
                '    End If
                'ElseIf (_ExamNum = _ExamAmount) And (_AnswerState = "2") Then
                '    Dim LeapExam As String = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
                '    DialogTitle = setTitleDialog(LeapExam, IsTeacher)
                '    Dialog = "True"
                'Else
                '    Dialog = "False"
                'End If

                Session("StartTimeQuestion") = Session("EndTimeQuestion")

                'ClsActivity.TeacherSetTotalScore(Quiz_Id, connActivity)

                'SwapStatus = ClsActivity.SwapStatus     

            End If
        End If

        If (Session("showAnswer") = True) Then
            If (Session("ShowCorrectAfterComplete") = True) Then
                If (_ExamNum = _ExamAmount) Then
                    Dialog = "True"
                End If
            ElseIf (_ExamNum = _ExamAmount) And _AnswerState = "2" Then
                Dialog = "True"
            End If
        Else
            If (_ExamNum = _ExamAmount) Then
                Dialog = "True" 'ถ้าไม่โชว์เฉลย ข้อสุดท้ายขึ้น dialog ด้วย
            End If
        End If

        SetAttributeQsetId(connActivity)

        'Close Connection
        _DB.CloseExclusiveConnect(connActivity)
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNext.Click, btnNextTop.Click, btnNextSide.Click


        controlBtnNext(False)

    End Sub

    Private Function ConvertStringIntro(ByVal strIntro As String) As Array
        Dim str As String = strIntro.Replace("<source scr=", "").Replace("type=""", "").Replace(""">", "").Replace("""", "")
        ConvertStringIntro = Split(str, " ")
    End Function

    'Private Function setTitleDialog(ByVal LeapExam As String, ByVal IsTeacher As Boolean) As String
    '    Dim strTitle As String = ""
    '    If LeapExam = "0" Then
    '        strTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
    '    Else
    '        If IsTeacher Then
    '            strTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
    '        Else
    '            strTitle = "ยังไม่ได้ทำข้อสอบอีก " & LeapExam & " ข้อ จบควิซเลยไหมคะ ?"
    '        End If
    '    End If
    '    setTitleDialog = strTitle
    'End Function

    Protected Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrevious.Click, btnPrvTop.Click, btnPrvSide.Click

        'Open Connection
        Dim connActivity As New SqlConnection
        _DB.OpenExclusiveConnect(connActivity)

        If NotReplyMode = "True" Then

            SetValueLeapChoice(2)
            Exit Sub
        End If

        Dim Quiz_Id As String = Session("Quiz_Id").ToString()
        Dim PlayerId As String = Session("PlayerId").ToString()
        If _ExamNum <> 1 Then
            _ExamNum -= 1
            If Not _AnswerState = "0" Then
                _AnswerState = "2"
                If Not KNSession(Quiz_Id & "|" & "SelfPace") Then
                    'Functin UpdateIsScore
                End If
            End If

            If Session("ChooseMode") = EnumDashBoardType.Quiz Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                Dim ClsSelectSession As New ClsSelectSession()
                ClsSelectSession.SetExamNum(_ExamNum)
            End If

            lblNoExam.Text = _ExamNum & " / " & _ExamAmount
            lblRunNoExam.Text = _ExamNum & " / " & _ExamAmount
            lblNoExamSide.Text = _ExamNum & " / " & _ExamAmount

            SetPositionRun(_ExamAmount, _ExamNum)

            questionId = ClsActivity.GetQuestionID(Quiz_Id, _ExamNum, connActivity)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, connActivity).ToString()

            'If VBIsSelfPace = False Or Session("ChooseMode") = EnumDashBoardType.Practice Then
            UpdateLastUpdateWhenNextOrPrev(Quiz_Id, questionId, connActivity) 'Update tblQuizQuestion ให้ข้อที่ถุกเลือกมาเป็นข้อบนสุด
            UseClsDroidPad.RemoveAndAddNewQQNoToApplication(Quiz_Id, _ExamNum) 'เก็บค่า Examnum ลงตัวแปรเพื่อเป็นเงื่อนไขในการเช็คเปลี่ยนข้อหน้าเด็ก
            HDCheckChangeQuestion.Value = "Reload"
            'End If

            UseClsDroidPad.RemoveAndAddNewAnsState(Quiz_Id, _AnswerState) 'เก็บ Ansstate เมื่อครุคลิกเปลี่ยนข้อ

            If (Not Session("ChooseMode") = EnumDashBoardType.Practice) And (Not Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
                If Session("QuizUseTablet") = True Then
                    Dim dt As New DataTable()
                    dt.Columns.Add("question_id", GetType(String))
                    dt.Columns.Add("QQ_No", GetType(Integer))
                    dt.Rows.Add(questionId, _ExamNum)
                    HttpContext.Current.Application.Lock()
                    HttpContext.Current.Application(Quiz_Id & "|QuestionId_QQ_No") = dt
                    Dim q As Quiz = HttpContext.Current.Application("Quiz_" & Quiz_Id)
                    q.Examnum = _ExamNum
                    q.AnswerState = _AnswerState
                    HttpContext.Current.Application("Quiz_" & Quiz_Id) = q
                    HttpContext.Current.Application.UnLock()
                    Dim context = GlobalHost.ConnectionManager.GetHubContext(Of hubSignalR)()
#If F5 = "1" Then
                    HttpContext.Current.Application.Lock()
#End If
                    context.Clients.Group(GroupName).send("Reload")
#If F5 = "1" Then
                    Dim t As New Dictionary(Of String, String)
                    t.Add(Session("UserId").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
                    HttpContext.Current.Application(Quiz_Id & "|Time") = t
                    HttpContext.Current.Application("ReloadTime") = Nothing
                    HttpContext.Current.Application.UnLock()
#End If
                    SetQuizExpire() 'set อายุของ redis device
                End If
            End If

            Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, PlayerId, _ExamNum, ViewIntroQsetId, connActivity)
            'Return tagIntro : Type : LimitAmount : ViewIntroQsetId
            If DataIntro <> "" Then
                Dim ArrIntro() As String = Split(DataIntro, "@:@")
                Dim TagIntro As String = ArrIntro(0)
                Dim TypeIntro As String = ArrIntro(1)
                Dim LimitAmount As String = ArrIntro(2)
                ViewIntroQsetId = ArrIntro(3)
                ViewLimitAmount.InnerHtml = LimitAmount
                If TypeIntro = "2" Then
                    mainIntro.InnerHtml = TagIntro
                Else
                    mainIntro.InnerHtml = "<span id=""spnTest"">Clickkkkkkkkkkkkkkkkkkkk</span>"""
                    introHtml.InnerHtml = TagIntro
                End If
                EnableIntro = True
            Else
                EnableIntro = False
            End If

            mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, PlayerId, _AnswerState, _ExamNum, Session("Selfpace"), connActivity)

            If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, True, connActivity)
            Else
                AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(PlayerId, _AnswerState, Quiz_Id, _ExamNum, Session("PracticeMode"), Session("Selfpace"), False, False, connActivity)
            End If
            MyAnswer = ClsActivity.MyAnswer
            CorrectAnswer = ClsActivity.CorrectAnswer

            SetPositionRun(_ExamAmount, _ExamNum)

            'Dim positionImg As Integer = ((490 / _ExamAmount) * _ExamNum)
            ''Dim positionImg As Integer = (500 / _ExamAmount) * (_ExamNum - 1)
            'imgRun.Style.Add("left", CStr(positionImg) + "px;")
            If _AnswerState = 2 Then
                AnswerExp.InnerHtml = ClsActivity.htmlAnswerExp
            End If

            If _AnswerState = 2 And Session("PracticeFromcomputer") Then
                'If ClsActivity.GetNotAnswer(Quiz_Id, Session("PlayerId").ToString, _ExamNum, connActivity) Then '
                '    IsNoAnswer = True
                'Else
                '    IsNoAnswer = False
                'End If
                IsNoAnswer = False
            Else
                IsNoAnswer = False
            End If
        End If

        SetAttributeQsetId(connActivity)

        'Close Connection
        _DB.CloseExclusiveConnect(connActivity)


    End Sub

    Private Sub UpdateLastUpdateWhenNextOrPrev(ByVal QuizID As String, ByVal QuestionID As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim _DB As New ClassConnectSql
        If QuizID Is Nothing Or QuestionID Is Nothing Then 'ถ้า QuizId หรือ QuestionId ไม่มีค่าให้ออกจาก Sub ทันที
            Exit Sub
        End If
        If QuizID <> "" Or QuestionID <> "" Then
            Dim sql As String
            Try
                If InStr(1, QuestionID.ToString, ",") = 0 Then 'ถ้าคำถามไม่ใช่ Type 6 Update แค่ QuestionId นั้น
                    sql = " UPDATE dbo.tblQuizQuestion SET LastUpdate = dbo.GetThaiDate(), ClientId = Null  WHERE Quiz_Id = '" & _DB.CleanString(QuizID) & "' " &
                                " AND Question_Id = '" & _DB.CleanString(QuestionID) & "' "
                Else 'ถ้าคำถามเป็น Type 6 Update QuestionId ทั้งหมดที่อยู่ใน QuizQuestion นั้น ทีเป็น Type 6
                    sql = " UPDATE dbo.tblQuizQuestion SET LastUpdate = dbo.GetThaiDate(), ClientId = Null WHERE Question_Id IN ( " &
                          " SELECT tblQuizQuestion_1.Question_Id " &
                          " FROM tblQuizQuestion INNER JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id INNER JOIN " &
                          " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                          " tblQuestion AS tblQuestion_1 ON tblQuestionSet.QSet_Id = tblQuestion_1.QSet_Id INNER JOIN " &
                          " tblQuizQuestion AS tblQuizQuestion_1 ON tblQuestion_1.Question_Id = tblQuizQuestion_1.Question_Id " &
                          " WHERE (tblQuestionSet.QSet_Type = 6) AND (tblQuizQuestion.Quiz_Id = '" & QuizID.CleanSQL & "') AND  " &
                          " (tblQuizQuestion.Question_Id = '" & QuestionID.CleanSQL & "') AND (tblQuizQuestion_1.Quiz_Id = '" & QuizID.CleanSQL & "')) "
                End If
                _DB.Execute(sql, InputConn)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Exit Sub
            End Try
        End If
    End Sub

    Public Function CutStr(ByVal QSet_name As String) As String
        Dim CheckBrOld As Boolean = QSet_name.Contains("<br>")
        Dim CheckBrNew As Boolean = QSet_name.Contains("<br />")
        Dim InstrOldBr As String
        Dim CutStrNewBr As String
        Dim CutStrOldBr As String
        If CheckBrOld = True Then
            InstrOldBr = InStr(QSet_name, "<br>")
            CutStrOldBr = QSet_name.Substring(0, InstrOldBr - 1)
        Else
            CutStrOldBr = QSet_name
        End If
        CutStr = CutStrNewBr
    End Function

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
                        'ฝึกฝนเด็ก(จากหน้า Login)
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {if (s_needTimer == 'True') {KeepTime();if (IsTimePerQuestion == 'False') { CountTime();}} $.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=Practice_PadFromLogin&PageName=Activity','type': 'iframe','width': 750,'minHeight':425,'beforeClose': function () { if (s_needTimer == 'True') { StopCountTime(); timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false); timeNormal(arrKeepTime[2], arrKeepTime[3], true);}} });});});</script>"
                    ElseIf Session("PracticeFromComputer") = True Then
                        'ฝึกฝนครู(จากหน้า Dashboard ฝึกฝน)
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {if (s_needTimer == 'True') {KeepTime();if (IsTimePerQuestion == 'False') { CountTime();}} $.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=Practice_PadFromTeacher&PageName=Activity','type': 'iframe','width': 750,'minHeight':425,'beforeClose': function () { if (s_needTimer == 'True') { StopCountTime(); timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false); timeNormal(arrKeepTime[2], arrKeepTime[3], true);}} });});});</script>"
                    Else
                        'ทำควิซ
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {if (s_needTimer == 'True') {KeepTime();if (IsTimePerQuestion == 'False') { CountTime();}} $.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=" & FolderName & "&PageName=" & PageName & "','type': 'iframe','width': 750,'minHeight':425,'beforeClose': function () { if (s_needTimer == 'True') { StopCountTime(); timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false); timeNormal(arrKeepTime[2], arrKeepTime[3], true);}} });});});</script>"
                    End If

                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)
                End If
            End If
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function saveAnswerSortQuestion(ByVal questionIdAll As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        'Dim sqlCorrectAnswer, sqlMakeAnswer, sqlUpdateScore, sql As String
        'Dim dtCorrectAnswer, dtMakeAnswer As DataTable
        Dim sqlUpdateSortAnswer As String
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        ' get question ของเด็กใน tblQuizScore
        'sql = " SELECT Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & shareQuizId & "' AND School_Code = '" & HttpContext.Current.Session("SchoolId") & "' AND Student_Id = '" & sharePlayerId & "' AND QQ_No = '" & shareExamNum & "';"
        'Dim CurrentQuestion As String = db.ExecuteScalar(sql)

        ' update คำตอบข้อสอบแบบเรียงลำดับตามที่เด็กตอบ
        Dim questionIdArr = questionIdAll.Split(",")
        Dim num As Integer = 1
        For Each q In questionIdArr
            'sqlUpdateSortAnswer = " UPDATE tblQuizAnswer SET Question_Id = '" & q.ToString() & "', LastUpdate = dbo.GetThaiDate() , Answer_Id = (select Answer_Id from tblanswer where Question_Id ='" & q.ToString() & "') WHERE Quiz_Id = '" & shareQuizId & "' AND QA_No = '" & num & "' AND Player_Id = '" & sharePlayerId & "' ; "
            sqlUpdateSortAnswer = " UPDATE tblQuizAnswer SET QA_No = '" & num & "', LastUpdate = dbo.GetThaiDate(), ClientId = Null WHERE Quiz_Id = '" & Quiz_Id.CleanSQL & "' AND Player_Id = '" & UserId & "' AND Question_Id = '" & q.ToString().CleanSQL & "' ;  "
            db.Execute(sqlUpdateSortAnswer)
            num = num + 1
        Next

        ' datatable คำตอบทึ่เรียงแบบถูก กับ คำตอบที่เด็กทำ
        'sqlCorrectAnswer = " SELECT Question_Id FROM tblAnswer WHERE Question_Id IN (SELECT Question_Id FROM tblQuizQuestion WHERE Quiz_Id = '" & shareQuizId & "' And QQ_No = '" & shareExamNum & "') ORDER BY CAST(Answer_Name as varchar); "
        'dtCorrectAnswer = db.getdata(sqlCorrectAnswer)
        'sqlMakeAnswer = " SELECT Question_Id FROM tblQuizAnswer WHERE Quiz_Id = '" & shareQuizId & "' AND Player_Id = '" & sharePlayerId & "' ORDER BY QA_No; "
        'dtMakeAnswer = db.getdata(sqlMakeAnswer)

        ' Loop check คำตอบใน datatable ว่าเรียงถูกหรือเปล่า
        'Dim scored As String = "1"
        'For i As Integer = 0 To dtCorrectAnswer.Rows.Count - 1
        '    If (dtCorrectAnswer.Rows(i)("Question_Id").ToString <> dtMakeAnswer(i)("Question_Id").ToString) Then
        '        scored = "0"
        '        Exit For
        '    End If
        'Next

        ' Update คะแนน,responseAmount,lastupdate,isscored,score ที่ tblQuizScore
        'sqlUpdateScore = " UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), " & _
        '    " LastUpdate = dbo.GetThaiDate(),ResponseAmount = ResponseAmount + 1,IsScored = '1',Answer_Id = '" & CurrentQuestion & "', " & _
        '    " Score = '" & scored & "' WHERE Student_Id = '" & sharePlayerId & "' AND Quiz_Id = '" & shareQuizId & "' " & _
        '    " AND School_Code = '" & HttpContext.Current.Session("SchoolId") & "' AND Question_Id = '" & CurrentQuestion & "';"
        'db.Execute(sqlUpdateScore)

        Return "success"
    End Function

    ' Save คำตอบแบบจับคู่
    <Services.WebMethod()>
    Public Shared Function SaveAnswerPairQuestion(ByVal QuestionIdAll As String, ByVal AnswerIdAll As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim db As New ClassConnectSql()
        Dim ArrQuestion As Array = QuestionIdAll.Split(",")
        Dim ArrAnswer As Array = AnswerIdAll.Split(",")
        Dim sql As New StringBuilder()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        Dim QA_No As Integer = 1
        ' Update QuizAnswer
        For i As Integer = 0 To ArrQuestion.Length - 1
            sql.Append(" UPDATE tblQuizAnswer SET Answer_Id ='")
            sql.Append(ArrAnswer(i))
            sql.Append("',LastUpdate = dbo.GetThaiDate(), ClientId = Null WHERE Quiz_Id = '")
            sql.Append(Quiz_Id.CleanSQL)
            sql.Append("' AND Question_Id = '")
            sql.Append(ArrQuestion(i))
            sql.Append("' AND QA_No = '")
            sql.Append(QA_No.ToString())
            sql.Append("' AND Player_Id = '")
            sql.Append(UserId.CleanSQL)
            sql.Append("';")
            QA_No = QA_No + 1
        Next
        Try
            db.Execute(sql.ToString())
            'ตอบถูกหมดทุกข้อถึงจะได้ 1 คะแนน
            IsCorrectAnswerPairQuestion(ArrQuestion, db)

            Return "Success"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "Fail"
        End Try
    End Function


    Private Shared Function IsCorrectAnswerPairQuestion(Qusetions As Array, ByRef db As ClassConnectSql) As Boolean
        Dim allQuestions As String = ""
        For Each question In Qusetions
            allQuestions &= String.Format("'{0}',", question)
        Next
        allQuestions = allQuestions.Substring(0, allQuestions.Length - 1)
        Dim sql As New StringBuilder()
        sql.Append(" SELECT q.Quiz_Id,q.Question_Id,q.Answer_Id,a.Answer_Id AS CorrectAnswer_Id,a.Answer_Name FROM tblQuizAnswer q INNER JOIN tblAnswer a on q.Question_Id = a.Question_Id WHERE q.Question_Id IN ")
        sql.Append(String.Format("({0}) AND q.Quiz_Id = '{1}' ORDER BY q.QA_No;;", allQuestions.CleanSQL, HttpContext.Current.Session("Quiz_Id").ToString().CleanSQL))
        Dim dt As DataTable = db.getdata(sql.ToString())

        sql.Clear()
        sql.Append(" SELECT q.Quiz_Id,q.Question_Id,q.Answer_Id,a.Answer_Id AS CorrectAnswer_Id,a.Answer_Name FROM tblQuizAnswer q INNER JOIN tblAnswer a on q.Answer_Id = a.Answer_Id WHERE q.Question_Id IN ")
        sql.Append(String.Format("({0}) AND q.Quiz_Id = '{1}' ORDER BY q.QA_No;;", allQuestions.CleanSQL, HttpContext.Current.Session("Quiz_Id").ToString().CleanSQL))
        Dim dtUserAnswer As DataTable = db.getdata(sql.ToString())

        For i As Integer = 0 To dt.Rows.Count - 1
            If (dt.Rows(i)("Answer_Id") <> dt.Rows(i)("CorrectAnswer_Id")) Then
                If (dt.Rows(i)("Answer_Name") <> dtUserAnswer.Rows(i)("Answer_Name")) Then
                    UpdateScorePairQuestion(0, dt, db)
                    Return False
                End If
            End If
        Next
        UpdateScorePairQuestion(1, dt, db)
        Return True
    End Function

    ' update score ตอนตอบคำถามแบบ จับคู่
    Private Shared Sub UpdateScorePairQuestion(Score As Integer, dtQuestion As DataTable, ByRef db As ClassConnectSql)
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        Dim sql As New StringBuilder
        For Each r In dtQuestion.Rows
            sql.Append(String.Format("UPDATE tblQuizScore SET Answer_Id = '{0}',ResponseAmount = ResponseAmount + 1,LastUpdate = dbo.GetThaiDate(),Score = {1} WHERE Student_Id = '{2}' AND Quiz_Id = '{3}' AND Question_Id = '{4}';", r("Answer_Id"), Score, UserId.CleanSQL, Quiz_Id.CleanSQL, r("Question_Id")))
        Next
        db.Execute(sql.ToString())

        sql.Clear()
        sql.Append(String.Format("UPDATE tblQuizSession SET TotalScore = (SELECT SUM(Score) FROM tblQuizScore WHERE Quiz_Id = '{0}' and Student_Id = '{1}') WHERE Quiz_Id = '{0}' And Player_Id = '{1}';", Quiz_Id.CleanSQL, UserId.CleanSQL))
        db.Execute(sql.ToString()) 'update totalscore ตอนตอบแบบจับคู่

    End Sub


    <Services.WebMethod()>
    Public Shared Function GetScoreStudent()
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        If HttpContext.Current.Session("SchoolId").ToString Is Nothing Then Return ""
        If HttpContext.Current.Session("Quiz_Id").ToString Is Nothing Then Return ""
        Dim _DB As New ClassConnectSql()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As String = " SELECT Student_CurrentNoInRoom,SUM(dbo.tblQuizScore.Score) AS TotalScore FROM dbo.tblQuizScore INNER JOIN dbo.t360_tblStudent " &
                            " ON dbo.tblQuizScore.Student_Id = dbo.t360_tblStudent.Student_Id " &
                            " WHERE (dbo.tblQuizScore.Quiz_Id = '" & Quiz_Id.CleanSQL & "') AND (dbo.tblQuizScore.IsActive = 1) " &
                            " GROUP BY Student_CurrentNoInRoom ORDER BY SUM(dbo.tblQuizScore.Score) DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim ArrData As New ArrayList
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                ArrData.Add(New With {.StudentNo = dt.Rows(i)("Student_CurrentNoInRoom"), .StudentScore = dt.Rows(i)("TotalScore")})
            Next
        End If
        Dim js As New JavaScriptSerializer()
        Dim ReturnValue = js.Serialize(ArrData)
        Return ReturnValue
    End Function

    Public Function SetHeightDiv() As String
        Dim _DB As New ClassConnectSql()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As String = " SELECT StudentAmount FROM dbo.tblQuiz WHERE Quiz_Id = '" & Quiz_Id.CleanSQL & "'; "
        Dim TotalStudent As Integer = CInt(_DB.ExecuteScalar(sql))
        If TotalStudent >= 13 Then
            Return "90"
        Else
            Return "50"
        End If
    End Function

    'ตอนกดตอบแบบ choice
    <Services.WebMethod()>
    Public Shared Function UpdateScore(ByVal Questionid As String, ByVal AnswerId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        ClsActivity.UpdateScore(AnswerId, HttpContext.Current.Session("Quiz_Id").ToString, Questionid, HttpContext.Current.Session("PlayerId"))
        Return "1"
    End Function

    'Update 09-05-56 BitWiseComparison Tools
    Private Sub getToolsInQuiz(ByVal EnabledTools As Integer)
        '' calculator
        'If (EnabledTools And 2) = 2 Then
        '    tools_Calculator = True
        'End If
        '' dictionary
        'If (EnabledTools And 4) = 4 Then
        '    tools_Dictionary = True
        'End If
        '' wordbook
        'If (EnabledTools And 8) = 8 Then
        '    tools_WordBook = True
        'End If
        '' note
        'If (EnabledTools And 16) = 16 Then
        '    tools_Note = True
        'End If
        '' protractor
        'If (EnabledTools And 32) = 32 Then
        '    tools_Protractor = True
        'End If
    End Sub

#Region "function share ใครหาย ตามได้ที่ activityservice นะครับ"
    '<Services.WebMethod()>
    'Public Shared Function getNeedTimer()
    '    Dim db As New ClassConnectSql()
    '    Dim dt As DataTable
    '    Dim sql As String
    '    sql = "SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,TimePerCorrectAnswer,IsTimeShowCorrectAnswer FROM tblQuiz WHERE Quiz_Id = '" + HttpContext.Current.Session("Quiz_Id").ToString + "';" 'AND TestSet_Id = '" + HttpContext.Current.Session("newTestSetId") + "';"
    '    dt = db.getdata(sql)
    '    Dim TimePerQuestion, TimeShowCorrect
    '    Dim IsQuestionMode As String = dt.Rows(0)("IsPerQuestionMode") 'โหมดเวลา 1=เวลาต่อข้อ , 0=เวลาทั้งหมด
    '    Dim IsTimeForCorrect As String = dt.Rows(0)("IsTimeShowCorrectAnswer") 'โหมดเวลาเฉลย 1=มี , 0=ไม่มี
    '    If (ansState = "1" <> ansState = "0") Then
    '        'ถ้าเป็นคำถาม
    '        If (IsQuestionMode = True) Then
    '            TimePerQuestion = dt.Rows(0)("TimePerQuestion")
    '        Else
    '            Dim examAmount As Integer = CInt(HttpContext.Current.Session("_ExamAmount").ToString())
    '            'TimePerQuestion = dt.Rows(0)("TimePerTotal")
    '            Dim timePerExam As Integer = (CInt(dt.Rows(0)("TimePerTotal").ToString()) * 60) / examAmount
    '            If (timePerExam < 10) Then
    '                timePerExam = 10
    '            End If
    '            TimePerQuestion = timePerExam
    '        End If
    '        TimeShowCorrect = "1"

    '    ElseIf (ansState = "2") Then
    '        'ถ้าเป็นเฉลย
    '        If (IsTimeForCorrect = True) Then
    '            TimePerQuestion = dt.Rows(0)("TimePerCorrectAnswer")
    '            TimeShowCorrect = "1"
    '        Else
    '            TimeShowCorrect = "0"
    '        End If
    '    End If
    '    Dim JsonString
    '    If dt.Rows.Count > 0 Then
    '        JsonString = New With {.NeedTimer = dt.Rows(0)("NeedTimer"), .IsPerQuestionMode = dt.Rows(0)("IsPerQuestionMode"), .TimePerQuestion = TimePerQuestion, .TimePerTotal = dt.Rows(0)("TimePerTotal"), .TimeShowCorrect = TimeShowCorrect, .ansState = ansState}
    '    End If
    '    Dim needTimer = js.Serialize(JsonString)
    '    Return needTimer
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function GetTotalStudentInQuiz()
    '    Dim _DB As New ClassConnectSql()
    '    Dim sql As String = " SELECT StudentAmount FROM dbo.tblQuiz WHERE Quiz_Id = '" & shareQuizId & "' "
    '    Dim TotalStudent As String = _DB.ExecuteScalar(sql)
    '    If TotalStudent <> "" Then
    '        Return TotalStudent
    '    Else
    '        Return 0
    '    End If
    'End Function

    'Private Function GetGroupNameFromQuizId(ByVal QuizId As String) 'Function หา GroupName เพื่อเอาไปใช้ AddGroup ตอน SigNalR
    '    Dim GroupName As String = ""
    '    Dim _DB As New ClassConnectSql()
    '    Dim dt As New DataTable
    '    If QuizId Is Nothing Or QuizId = "" Then
    '        Return GroupName
    '    End If

    '    Dim sql As String = " SELECT t360_ClassName,t360_RoomName,TabletLab_Id FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
    '    dt = _DB.getdata(sql)
    '    If dt.Rows.Count > 0 Then
    '        If dt.Rows(0)("TabletLab_Id") Is DBNull.Value Then 'ถ้าไม่ได้เป็นห้อง SoundLab ดึงชั้นกับห้องมาเป็น GroupName ได้เลย
    '            GroupName = dt.Rows(0)("t360_ClassName") & dt.Rows(0)("t360_RoomName")
    '        Else 'ถ้าเป็นห้อง SoundLab หาชื่อห้อง SoundLab มาเป็น GroupName แทน
    '            Dim TabletLabId As String = dt.Rows(0)("TabletLab_Id").ToString()
    '            sql = " SELECT TabletLab_Name FROM dbo.tblTabletLab WHERE TabletLab_Id = '" & TabletLabId & "' "
    '            GroupName = _DB.ExecuteScalar(sql)
    '        End If
    '    Else
    '        Return GroupName
    '    End If

    '    _DB = Nothing
    '    Return GroupName
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function CheckQQNoAfterLoading(ByVal QQ_No As String)
    '    Dim _DB As New ClassConnectSql()
    '    Dim sql As String = " SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & shareQuizId & "' ORDER BY lastupdate DESC; "
    '    Dim CurrentQQNo As String = _DB.ExecuteScalar(sql)
    '    If CurrentQQNo <> QQ_No Then
    '        Return "Reload"
    '    Else
    '        Return "NotReload"
    '    End If
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function getStudentDontReply()
    '    Dim db As New ClassConnectSql()
    '    Dim sql As String
    '    Dim dt As DataTable
    '    'If HttpContext.Current.Session("SchoolId").ToString Is Nothing Then Return ""
    '    'If HttpContext.Current.Session("Quiz_Id").ToString Is Nothing Then Return ""
    '    'sql = "SELECT Student_Id FROM tblQuizScore WHERE Quiz_Id = '" + HttpContext.Current.Session("Quiz_Id") + "' "
    '    'sql += "AND Question_Id = '" + questionId + "' AND School_Code = '" + HttpContext.Current.Session("SchoolId") + "' AND Answer_Id IS null ORDER BY Student_Id"
    '    'sql = " SELECT ts.Student_CurrentNoInRoom as Student_CurrentNoInRoom FROM tblQuizScore AS tq "
    '    'sql += " LEFT JOIN t360_tblStudent AS ts "
    '    'sql += " ON tq.Student_Id = ts.Student_Id "
    '    'sql += " AND tq.School_Code = tq.School_Code "
    '    'sql += " WHERE (tq.Answer_Id Is null) "
    '    'sql += " AND tq.Quiz_Id = '" + HttpContext.Current.Session("Quiz_Id").ToString() + "' "
    '    'sql += " AND tq.QQ_No = '" + shareExamNum + "'"
    '    'sql += " AND tq.School_Code = '" + HttpContext.Current.Session("SchoolId").ToString() + "' "
    '    'sql += " ORDER BY ts.Student_CurrentNoInRoom "

    '    sql = " SELECT t360_tblStudent.Student_CurrentNoInRoom as Student_CurrentNoInRoom FROM tblQuizScore INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id "
    '    sql += "WHERE tblQuizScore.Student_Id IN (SELECT Player_Id FROM tblQuizSession WHERE IsActive = '1' AND Player_Type = '2' AND Quiz_Id = '" & shareQuizId & "')"
    '    sql += "AND tblQuizScore.Quiz_Id = '" & shareQuizId & "' AND tblQuizScore.QQ_No = '" & shareExamNum & "' AND tblQuizScore.Answer_Id IS NULL ORDER BY Student_CurrentNoInRoom"
    '    dt = db.getdata(sql)
    '    ' แปลงค่าเป็น JSON
    '    Dim sb As New StringBuilder()
    '    Dim JsonString As New ArrayList
    '    If dt.Rows.Count > 0 Then
    '        For Each row As DataRow In dt.Rows
    '            'JsonString = New With {.stuId = row("Student_Id")}
    '            JsonString.Add(New With {.stuId = row("Student_CurrentNoInRoom")})
    '        Next
    '    Else
    '        JsonString.Add(New With {.stuId = "0"})
    '    End If
    '    Dim StudentID = js.Serialize(JsonString)
    '    Return StudentID
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function getModeQuizAndTimer()
    '    Dim db As New ClassConnectSql()
    '    Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
    '    Dim sql As String = "  SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,DATEDIFF(SECOND,StartTime,dbo.GetThaiDate()) as timeDiff,DATEDIFF(SECOND,dbo.GetThaiDate(),DATEADD(MINUTE,TimePerTotal,StartTime)) as timeRemain FROM tblQuiz WHERE Quiz_Id = '" & Quiz_Id & "'; "
    '    Dim dt As DataTable = db.getdata(sql)
    '    Dim AllTime As Integer = 0
    '    Dim TimeRemain As Integer = 0
    '    Dim NeedTimer As Boolean
    '    Dim timerType As Boolean = False
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
    '                    If CInt(dt.Rows(0)("timeRemain")) > 0 Then
    '                        AllTime = CInt(dt.Rows(0)("timeDiff"))
    '                        TimeRemain = CInt(dt.Rows(0)("timeRemain"))
    '                    End If
    '                End If
    '            Else
    '                ' state เฉลย
    '                If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
    '                    ' เฉลยแบบมีเวลา
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
    '                            AllTime = CInt(dt.Rows(0)("timeDiff"))
    '                            TimeRemain = CInt(dt.Rows(0)("timeRemain"))
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
    '        Dim js As New JavaScriptSerializer()
    '        Dim JsonString
    '        JsonString = New With {.NeedTimer = NeedTimer, .AllTime = AllTime, .timerType = timerType, .TimeRemain = TimeRemain, .noWatch = noWatch}
    '        getModeQuizAndTimer = js.Serialize(JsonString)
    '    Else
    '        getModeQuizAndTimer = "ERROR"
    '    End If
    'End Function
#End Region


    <Services.WebMethod()>
    Public Shared Function GetRenderNextStep(ByVal EventType As String, ByVal AnswerState As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim IsShowDialog As String
        Dim txtForShowDialog As String
        Dim DialogType As String
        Dim NextStep As String
        Dim js As New JavaScriptSerializer()
        'Dim LeapExam As String = ClsActivity.CountLeapExam(HttpContext.Current.Session("Quiz_Id").ToString(), HttpContext.Current.Session("UserId").ToString)

        If EventType = "1" Then
            'If LeapExam = "0" Then
            If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Quiz Then
                IsShowDialog = "True"

                If HttpContext.Current.Session("ShowCorrectAfterComplete") And AnswerState <> "2" Then
                    DialogType = "1"
                    NextStep = ""
                    txtForShowDialog = "ดูเฉลยเลยหรือจะทบทวนอีกสักรอบก่อนคะ"
                Else
                    DialogType = "2"
                    NextStep = "../Activity/AlternativePage.aspx"
                    txtForShowDialog = "จบควิซเลยหรือจะทบทวนอีกสักรอบก่อนคะ"
                End If
            Else
                If AnswerState = "0" Then
                    IsShowDialog = "True"

                    If HttpContext.Current.Session("ShowCorrectAfterComplete") Then
                        txtForShowDialog = "ดูเฉลยเลยหรือจะทบทวนอีกสักรอบก่อนคะ"
                        NextStep = ""
                        DialogType = "1"
                    Else
                        txtForShowDialog = "จบฝึกฝนเลยหรือจะทบทวนอีกสักรอบก่อนคะ"
                        NextStep = "../Activity/ReviewMostWrongAnswer.aspx"
                        DialogType = "2"
                    End If
                ElseIf AnswerState = "2" Then
                    IsShowDialog = "True"
                    txtForShowDialog = "จบฝึกฝนเลยหรือจะทบทวนอีกสักรอบก่อนคะ"
                    DialogType = "2"
                    NextStep = "../Activity/AlternativePage.aspx"
                End If
            End If

        End If

        Dim JsonString
        JsonString = New With {.IsShowDialog = IsShowDialog, .txtForShowDialog = txtForShowDialog, .DialogType = DialogType, .NextStep = NextStep}
        Return js.Serialize(JsonString)
    End Function
    Private Sub SetQuizExpire()
        Dim QuizId As String = HttpContext.Current.Session("Quiz_Id").ToString()
        For Each m In redis.SMembers(QuizId)
            redis.Expire(m, 900)
        Next
    End Sub
    Private Sub SetPositionRun(ByVal ExamAmount As Integer, ByVal ExamNum As Integer)

        Dim positionImg As Integer = If(ExamNum = 1, 0, ((490 / (ExamAmount - 1)) * (ExamNum - 1)))
        imgRun.Style.Add("left", CStr(positionImg) + "px;")
        lblRunNoExam.Style.Add("margin-left", CStr(positionImg + 113) + "px;")

    End Sub

End Class


