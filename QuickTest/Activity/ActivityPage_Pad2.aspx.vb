Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports KnowledgeUtils
Imports System.Web

Public Class ActivityPage_Pad2
    Inherits System.Web.UI.Page

#Region "Variable"

    Dim UseCls As New ClassConnectSql
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    Dim ClsUser As New ClsUser(New ClassConnectSql)
    Dim SClsPractice As New Service.ClsPracticeMode(New ClassConnectSql)
    Dim redis As New RedisStore()

    Public Shared KnSession As New ClsKNSession()

    Public GroupName As String
    Public IsPracticeMode As String
    Public Dialog As String = "false"
    Public DialogTitle As String
    Public MyAnswer As String
    Public CorrectAnswer As String
    Public ViewAmount As String
    Public SizeTopForImgExit As String
    Public SizeBtnExitQuiz As String
    Public txtSendComplete As String
    Public txtExitQuiz As String
    Public IsShowNextSlide As String = "False"
    Public ScoreChoiceToChoice As String
    Public F5 As String ' สำหรับโหมด debug ฝั่ง client
    Public DeviceId As String
    Public CurrentQuizId As String
    Public questionId As String
    Public NotReplyMode As String

    Public IndexChoice As Integer 'เลขข้อ
    Public CheckNoData As Integer
    Public ReplyAmount As Integer

    Public IsLastQuestion As Boolean
    Public SwapStatus As Boolean = False
    Public IsHomeWork As Boolean
    Public IsShowScoreAfterCompleteState As Boolean
    Public VBCheckIsNeedTimerPerQuestion As Boolean
    Public IsNoAnswer As Boolean
    Public EnableIntro As Boolean
    Public IsShowBtnCompleteHomework As Boolean
    Public UseTools As Boolean = False
    Public tools_Calculator As Boolean = False
    Public tools_WordBook As Boolean = False
    Public tools_Note As Boolean = False
    Public tools_Protractor As Boolean = False
    Public tools_Dictionary As Boolean = False
    Public ShowDivScoreChoiceToChoice As Boolean
    Public IsSeenIntro As Boolean = True
    Public EPageNumber As Integer = 1

    Protected NeedShowTip As Boolean
    Protected IsMobile As Boolean
    Protected TokenId As String
    Protected qsetFilePath As String = ""
    Dim conDB As New ClassConnectSql

    Public Property ViewIntroQsetId As String
        Get
            Return ViewState("_ViewIntroQsetId")
        End Get
        Set(ByVal value As String)
            ViewState("_ViewIntroQsetId") = value
        End Set
    End Property
    Public Property JvIsSelfPace As Boolean
        Get
            Return ViewState("_JvIsSelfPace")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_JvIsSelfPace") = value
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
    Public Property PageNumber As String
        Get
            Return ViewState("_PageNumber")
        End Get
        Set(ByVal value As String)
            ViewState("_PageNumber") = value
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
    Public Property ExamNum As Integer
        Get
            Return ViewState("_ExamNum")
        End Get
        Set(ByVal value As Integer)
            ViewState("_ExamNum") = value
        End Set
    End Property
    Public Property VQuizId As String
        Get
            Return ViewState("_VQuizId")
        End Get
        Set(ByVal value As String)
            ViewState("_VQuizId") = value
        End Set
    End Property
    Public Property IsPerQuestion As Boolean
        Get
            Return ViewState("_IsPerQuestion")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_IsPerQuestion") = value
        End Set
    End Property
    Public Property ShowCorrectAfterCompleteState As Boolean
        Get
            Return ViewState("ShowCorrectAfterCompleteState")
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowCorrectAfterCompleteState") = value
        End Set
    End Property
    Public Property VBShowScoreChoiceToChoice As Boolean
        Get
            Return ViewState("_VBShowScoreChoiceToChoice")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_VBShowScoreChoiceToChoice") = value
        End Set
    End Property
    Public Property IsHalfWay As Boolean
        Get
            Return ViewState("_IsHalfWay")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_IsHalfWay") = value
        End Set
    End Property
    Public Property ExamAmount As String
        Get
            Return ViewState("_ExamAmount")
        End Get
        Set(ByVal value As String)
            ViewState("_ExamAmount") = value
        End Set
    End Property
    Public Property LeapQuestions As Dictionary(Of Integer, Integer)
        Get
            Return ViewState("_LaepQuestions")
        End Get
        Set(ByVal value As Dictionary(Of Integer, Integer))
            ViewState("_LaepQuestions") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString().ToLower()
            If AgentString.IndexOf("android") <> -1 OrElse AgentString.IndexOf("iphone") <> -1 OrElse AgentString.IndexOf("ipad") <> -1 Then
                IsMobile = True
            End If
            AnsweredMode.Value = 0
            OrderNo.Value = 0
        End If

        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "activitypage session หลุด", False)
            'Response.Redirect("~/LoginPage.aspx")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Redirect", "window.parent.location='../LoginPage.aspx';", True)
            Exit Sub
        End If

        Dim start As DateTime = DateTime.Now

        If HDNotReplyMode.Value = "True" Then 'ทำเพื่อเอาไปใช้ใน Function savescore เพื่อตรวจว่าควรจะเลื่อนตำแหน่ง Array มั้ย
            NotReplyMode = True
        End If

        'Open Connection
        Dim connActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(connActivity)

        DeviceId = Request.QueryString("DeviceUniqueID").ToString()

        If BusinessTablet360.ClsKNSession.IsMaxONet Then
            TokenId = Request.QueryString("token")
        End If

        If Request.QueryString("dialyactivities") IsNot Nothing Then Session("ChooseMode") = EnumDashBoardType.Homework

        Dim QuizId As String = ""
        Dim IsLab As Boolean 'ไว้เช็คว่า quiz ห้อง lab หรือเปล่า

        If Session("ChooseMode") = Nothing Then
            Session("ChooseMode") = EnumDashBoardType.Quiz
            Session("HomeworkMode") = False
        End If

        If Session("ChooseMode") = EnumDashBoardType.Practice Then
            IsPracticeMode = True
            Session("HomeworkMode") = False

            Dim KNS As New KNAppSession()
            If Not IsPostBack Then

                QuizId = SClsPractice.SaveQuizDetail(Request.QueryString("ItemId"), Session("SchoolCode"), Session("UserId").ToString,
                         True, False, "1", KNS.StoredValue("CurrentCalendarId").ToString, Session("ChooseMode"), connActivity, DeviceId, TokenId)
                VQuizId = QuizId
                If ClsActivity.CheckQuizIsSoundLab(QuizId, connActivity) = True OrElse ClsActivity.CheckQuizIsSpareTablet(DeviceId, connActivity) Then ' เครื่องสำรองให้ทำงานเหมือนกับ ห้อง lab เลย
                    PlayerId = GetPlayerIdByDeviceId(DeviceId, QuizId, connActivity) 'ได้ Id ของเด็กคนนี้           
                Else
                    PlayerId = GetPlayerIdByDeviceId(DeviceId, , connActivity) 'ได้ Id ของเด็กคนนี้
                End If
                Dim TestsetName As String = ClsActivity.GetTestsetName(QuizId, connActivity)
                Log.Record(Log.LogType.Practice, "เข้าทำฝึกฝน """ & TestsetName & """ ", True, PlayerId)
                Session("QuizUseTablet") = SClsPractice.GetUseTablet(QuizId, connActivity)

                CheckUserViewPageWithTip()
            End If
        ElseIf Session("ChooseMode") = EnumDashBoardType.Homework Then
            Session("HomeworkMode") = True
            IsPracticeMode = False

            If Not IsPostBack Then
                QuizId = Request.QueryString("ItemId").ToString
                VQuizId = QuizId
                If ClsActivity.CheckQuizIsSoundLab(QuizId, connActivity) = True Then
                    PlayerId = GetPlayerIdByDeviceId(DeviceId, QuizId, connActivity) 'ได้ Id ของเด็กคนนี้
                Else
                    PlayerId = GetPlayerIdByDeviceId(DeviceId, , connActivity) 'ได้ Id ของเด็กคนนี้
                End If
                Dim TestsetName As String = ClsActivity.GetTestsetName(QuizId, connActivity)
                Log.Record(Log.LogType.Homework, "ทำการบ้าน """ & TestsetName & """ ", True, PlayerId)
                If Request.QueryString("Status") = "0" Then
                    ClsActivity.SetStatusQuiz(QuizId, PlayerId, "1", connActivity)
                End If
            End If

        Else

            IsPracticeMode = False
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
            If q IsNot Nothing Then
                QuizId = q.QuizId
                VQuizId = q.QuizId
                PlayerId = q.PlayerId
                IsLab = q.IsLab
            End If

        End If

        If PlayerId = "" Then
            Exit Sub
        End If


        'If Not IsPostBack Then
        Session("_StudentId") = PlayerId
        Session("QuizIdForShowScore") = VQuizId

        If Not IsPostBack Then
            If IsLab Then
                Dim Player As New Student(DeviceId, True, connActivity)
                Session("UserId") = Player.StudentId
                Session("selectedSession") = Player.RoomId
                Session("SchoolCode") = Player.SchooldId
                Session("SchoolID") = Player.SchooldId
                Dim c As New ClsSelectSession()
                c.SetCalendarId()
            Else
                If Session("UserId") Is Nothing Then
                    Dim Player As New Student(DeviceId, connActivity)
                    Session("UserId") = Player.StudentId
                    Session("selectedSession") = Player.RoomId
                    Session("SchoolCode") = Player.SchooldId
                    Session("SchoolID") = Player.SchooldId
                    Dim c As New ClsSelectSession()
                    c.SetCalendarId()
                End If
            End If
        End If

        If Not IsPostBack Then
            'ถ้าเป็น Mode ฝึกฝนให้โชว์ Intro VS มาก่อน
            If IsPracticeMode = True Then
                GenHtmlVSIntro(PlayerId, VQuizId)
                IsSeenIntro = False
            ElseIf BusinessTablet360.ClsKNSession.IsMaxONet AndAlso Session("ChooseMode") = EnumDashBoardType.Homework Then
                DivVs.InnerHtml = HtmlDialyActivities()
                IsSeenIntro = False
            End If
        End If

        'Session("Quiz_Id") = QuizId
        If QuizId Is Nothing Or QuizId.Trim() = String.Empty Then
            QuizId = VQuizId
        End If

        Dim IsSelfPace As Boolean
        If Session("ChooseMode") = EnumDashBoardType.Homework Or Session("ChooseMode") = EnumDashBoardType.Practice Then
            IsSelfPace = True
            KnSession(QuizId & "|" & "SelfPace") = True
            KnSession(QuizId & "|" & "IsDifferentQuestion") = False
            KnSession(QuizId & "|" & "IsDifferentAnswer") = False
        Else
            IsSelfPace = KnSession(QuizId & "|" & "SelfPace")
        End If

        JvIsSelfPace = IsSelfPace
        Dim dtSetting As DataTable = ClsActivity.GetSetting(VQuizId.ToString, connActivity)

        If dtSetting.Rows(0)("NeedCorrectAnswer") = True Then
            If dtSetting.Rows(0)("IsShowCorrectAfterComplete") Then
                Session("ShowCorrectAfterComplete") = True
            Else
                Session("ShowCorrectAfterComplete") = False
            End If
        Else
            Session("ShowCorrectAfterComplete") = False
        End If
        Dim needShowScore = dtSetting(0)("NeedShowScore")
        VBCheckIsNeedTimerPerQuestion = dtSetting(0)("IsPerQuestionMode")
        Dim NeedShowScoreAfterComplete = dtSetting(0)("NeedShowScoreAfterComplete")
        Session("NeedShowScoreAfterComplete") = NeedShowScoreAfterComplete
        If needShowScore = True And NeedShowScoreAfterComplete = False And IsSelfPace = False Then
            VBShowScoreChoiceToChoice = True
            ShowDivScoreChoiceToChoice = True
            ScoreChoiceToChoice = ClsActivity.GetTotalScoreForChoiceToChoice(QuizId.ToString, PlayerId.ToString, connActivity)
        Else
            VBShowScoreChoiceToChoice = False
        End If
        If dtSetting(0)("IsHomeWorkMode") Is DBNull.Value Then
            IsHomeWork = False
            IsShowBtnCompleteHomework = False
        Else
            If dtSetting(0)("IsHomeWorkMode") Then
                IsHomeWork = True
                Dim Status As String = Request.QueryString("Status").ToString()
                HDStatusHomework.Value = Status
                If (Status = "0" And BusinessTablet360.ClsKNSession.IsMaxONet = False) Then
                    IsShowBtnCompleteHomework = True
                    SizeTopForImgExit = "12"
                    SizeBtnExitQuiz = "100"
                Else
                    IsShowBtnCompleteHomework = False
                    SizeTopForImgExit = "23"
                    SizeBtnExitQuiz = "125"
                End If
                IsSelfPace = True
                JvIsSelfPace = IsSelfPace
            Else
                IsHomeWork = False
                IsShowBtnCompleteHomework = False
                SizeTopForImgExit = "23"
                SizeBtnExitQuiz = "125"
            End If
        End If

        If dtSetting.Rows(0)("Selfpace") Then
            ViewState("ToolsInQuiz") = dtSetting.Rows(0)("EnabledTools") 'Update 11-06-56 set value to viewstateTools
        Else
            ViewState("ToolsInQuiz") = dtSetting.Rows(0)("EnabledTools")
        End If

        Dim NeedCorrectAnswer As Boolean = dtSetting(0)("NeedCorrectAnswer")
        Dim IsShowCorrectAfterComplete As Boolean = dtSetting(0)("IsShowCorrectAfterComplete")

        If IsSelfPace = True Then 'ถ้าเป็นโหมดไปไม่พร้อมกัน

            If IsPracticeMode = True Then
                If Not IsPostBack Then
                    _AnswerState = 0
                End If
            ElseIf IsHomeWork = True Then
                Dim State As String = Request.QueryString("Status").ToString()
                If (BusinessTablet360.ClsKNSession.IsMaxONet) And _AnswerState = 2 Then
                    State = 3
                End If
                If State = "3" Then
                    _AnswerState = "2"
                Else
                    _AnswerState = State
                End If
            Else
                '_AnswerState = ClsDroidPad.GetAnsStateFromApplication(Quizid)
                If Not IsPostBack Then
                    If NeedCorrectAnswer Then 'ถ้าเป็นเฉลย
                        If IsShowCorrectAfterComplete Then
                            _AnswerState = "0"
                        Else
                            _AnswerState = "1"
                        End If
                    Else
                        _AnswerState = "0"
                    End If
                End If

            End If

            If HDIsLeapChoice.Value = "True" Then
                ExamNum = HDQQ_No.Value
                HttpContext.Current.Session("ExamNum") = ExamNum
                HDIsLeapChoice.Value = "False"
                If Not Page.IsPostBack Then
                    HDIsLeapChoice.Value = ""
                End If
            Else
                If Not IsPostBack Then
                    ExamNum = 1
                End If
            End If
            ' สำหรับเช็คว่าถ้า quiz มีข้อเดียว พอกด next ต้องขึ้น dialog เลย
            ExamAmount = ClsActivity.GetExamAmount(VQuizId, connActivity)
            Dialog = If(ExamAmount = 1, "True", "False")
            'CInt(ClsActivity.GetExamNum(Quizid, PlayerId)) ' Get ข้อปัจจุบัน
            IsPerQuestion = getNeedTimerPerQuestion(VQuizId, connActivity) 'set viewstate isperquestion
            'Check ก่อนว่ามี Insert QuizScore ข้อนี้ไปหรือยัง จะได้ไม่ Insert ซ้ำ+
            If CheckIsHaveQuestionStudent(VQuizId, PlayerId, ExamNum, connActivity) = False Then
                'SetQuizScore ข้อแรก
                If ClsActivity.SetQuizScoreStudent(ExamNum, QuizId, PlayerId, connActivity) = "Error" Then
                    Exit Sub
                End If
            End If
            'If Not IsPostBack Then
            renderExamSelfPace(ExamNum, _AnswerState, connActivity)
            questionId = ClsActivity.GetQuestionID(VQuizId, ExamNum, connActivity)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, connActivity).ToString()
            'End If


        Else
            _AnswerState = ClsDroidPad.GetAnsStateFromApplication(QuizId)

            CheckAndRenderData(_AnswerState, connActivity) 'ถ้าเป็นโหมดไปพรัอมกัน

        End If
        If needShowScore AndAlso NeedShowScoreAfterComplete = False Then 'ถ้าแสดงคะแนนข้อต่อข้อ
            Session("showScore") = True
            lblScore.Text = ClsActivity.GetScoreOfPlayer(VQuizId, PlayerId, connActivity).ToPointplusScore()
            lblSumScore.Text = ClsActivity.GetTotalScore(VQuizId, connActivity).ToPointplusScore()
            IsShowScoreAfterCompleteState = False
        ElseIf needShowScore AndAlso NeedShowScoreAfterComplete Then
            IsShowScoreAfterCompleteState = True
        Else
            Session("showScore") = False
        End If

        If ((_AnswerState = "1" Or _AnswerState = "0") And NotReplyMode = "True") Then
            If HDIsLeapChoice.Value = "True" Then
                SetValueLeapChoice(0)
            End If
            If HDLastChoice.Value = "True" Then
                SetValueLeapChoice(3)
            End If
        Else
            MainDivPad.Attributes.Remove("Class")
            MainDivPad.Attributes.Add("Class", "MainDivNormal")
            DivNotHaveDontReplyChoice.Style.Add("display", "none")
        End If

        If Not ViewState("ToolsInQuiz") = 0 Then 'Update 11-06-56 set variable Tools
            UseTools = False
            getToolsInQuiz(ViewState("ToolsInQuiz"))
        End If

        If HttpContext.Current.Session("_QuizId") Is Nothing Then
            HttpContext.Current.Session("Quiz_Id") = QuizId
        Else
            HttpContext.Current.Session("Quiz_Id") = HttpContext.Current.Session("_QuizId")
        End If

        If JvIsSelfPace Then
            GenPanelShowInfo(PlayerId, VQuizId, ExamNum, , connActivity) ' ส่วนขนาดของ div
        End If

        SwapStatus = ClsActivity.SwapStatus

        If Not IsPostBack Then
            Session("StartTimeQuestion") = Date.Now
        End If

        'keep examnum for getnextaction
        KnSession(DeviceId & "|" & "_ExmanNum") = ExamNum

        'keep AnswerState for GetnextAction
        KnSession(DeviceId & "|" & "CurrentAnsState") = _AnswerState

        'Close Connection
        UseCls.CloseExclusiveConnect(connActivity)

        If IsPracticeMode Then
            ExamAmount = ClsActivity.GetExamAmount(VQuizId)
            If IsHalfWay = False Then
                If ExamAmount / ExamNum = 2 Then
                    IsHalfWay = True
                    ClsActivity.UpdateIsHalfWay(VQuizId)
                End If
            End If
        End If

        qsetFilePath = (ClsActivity.GetQsetIDFromExamNum(ExamNum, HttpContext.Current.Session("Quiz_Id"))).ToFolderFilePath()

        'If _AnswerState = "2" Then
        '    btnMoreDetail.Style.Add("display", "block")
        'Else
        '    btnMoreDetail.Style.Add("display", "none")
        'End If

        If HttpContext.Current.Session("ShowSelectExamPanel") = True Then
            divSelectExplain.Style.Add("display", "block")
            Dim scriptKey As String = "UniqueKeyForThisScript"
            Dim javaScript As String = "<script type='text/javascript'>CreateButtonSelectExplain(1);</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)
            HttpContext.Current.Session("ShowSelectExamPanel") = False
            'btnMoreDetail.Style.Add("display", "block")
        Else
            divSelectExplain.Style.Add("display", "none")
        End If

    End Sub
    Private Sub CheckUserViewPageWithTip()
        If Not Page.IsPostBack Then
            ' ส่วนของการแสดง qtip
            If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
            End If
        End If
        'NeedShowTip = True
    End Sub
    Private Sub SetValueLeapChoice(ByVal ActionMode As String, Optional ByRef InputConn As SqlConnection = Nothing) '0=กดมาจาก Panel,1=กดมาจากปุ่ม next,2กดมาจากปุ่ม Back

        If ActionMode = 3 Then
            HDNotReplyMode.Value = "False"
            NotReplyMode = "False"
            ExamNum = CStr(CInt(ClsActivity.GetNextChoiceAfterReply(VQuizId, PlayerId, InputConn)) - 1)

            MainDivPad.Attributes.Remove("Class")
            MainDivPad.Attributes.Add("Class", "MainDivNormal")
            controlBtnNext()
            Exit Sub
        Else

            'ถ้าทวนข้อข้าม 
            ' สร้าง Array ข้อข้ามก่อน
            Dim ArrNotReply As New ArrayList
            Dim dt As DataTable = ClsActivity.GetNotReplyNum(VQuizId, PlayerId, InputConn)
            If Not dt.Rows.Count = 0 Then

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
                ExamNum = CStr(CInt(ClsActivity.GetNextChoiceAfterReply(VQuizId, PlayerId, InputConn)) - 1)

                MainDivPad.Attributes.Remove("Class")
                MainDivPad.Attributes.Add("Class", "MainDivNormal")
                If ClsActivity.CountLeapExam(VQuizId, PlayerId, InputConn) <> GetTopQQNoByQuizId(VQuizId, InputConn) Then
                    'DivNotHaveDontReplyChoice.Style.Add("display", "block")
                End If

                controlBtnNext()
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
            renderExamSelfPace(CurrentExamNum, CurrentAnsState, InputConn)
            GenPanelShowInfo(PlayerId, VQuizId, CurrentExamNum, , InputConn)
            ExamNum = CurrentExamNum
            Session("IsReplyAnswer") = False
            HDIsLeapChoice.Value = "False" 'Render เสร็จต้องเปลี่ยนค่ากลับเป็น False ไม่งั้นเดียวจะเข้า Function นี้ทุกครั้ง
        End If

        If HDLastChoice.Value = "True" Then

            HDLastChoice.Value = "False"
        End If

    End Sub
    Private Sub CheckAndRenderData(ByVal AnswerState As String, Optional ByRef InputConn As SqlConnection = Nothing) ' Render Quiz แบบไปพร้อมกัน
        If Request.QueryString("DeviceUniqueID") = Nothing Then
            Response.Redirect("../Activity/EmptySession.aspx")
        End If
        'Dim DeviceId As String = Request.QueryString("DeviceUniqueID").ToString()
        If DeviceId <> "" Then
            Dim dtQuizDetail As New DataTable
            Dim QuizId As String = ""
            Dim StudentId As String = ""
            If ClsActivity.CheckQuizIsSoundLab(VQuizId, InputConn) = True Then
                dtQuizDetail = GetdtSoundlabQuizDetail(VQuizId, DeviceId, InputConn)
            Else
                'dtQuizDetail = ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceId, InputConn)
            End If

            'Dim redis As New RedisStore()
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)

            If q IsNot Nothing Then
                QuizId = q.QuizId
                CurrentQuizId = q.QuizId
                StudentId = q.PlayerId
                'If dtQuizDetail.Rows.Count > 0 Then
                'If dtQuizDetail.Rows(0)("Quiz_Id") Is DBNull.Value Or dtQuizDetail.Rows(0)("Player_Id") Is DBNull.Value Then
                '    CheckNoData = 1
                '    Exit Sub
                'End If
                'QuizId = dtQuizDetail.Rows(0)("Quiz_Id").ToString()
                'CurrentQuizId = QuizId 'เก็บ Quiz_id ไปใช้ที่ JavaScript
                'StudentId = dtQuizDetail.Rows(0)("Player_Id").ToString()

                'Session("_QuizId") = QuizId
                'Session("_StudentId") = StudentId
                'Session("_School_Code") = dtQuizDetail.Rows(0)("School_Code").ToString()

                Dim dt2 As New DataTable
                dt2 = ClsDroidPad.GetLastChoiceQuestion(QuizId, InputConn)
                If dt2.Rows.Count > 0 Then
                    'Dim QuestionsId As String = dt2.Rows(0)("question_id").ToString()
                    Dim QQNo As Integer = dt2.Rows(0)("QQ_no")
                    Session("_QQNo") = QQNo
                    ExamNum = QQNo

                    'Dim sql1 As String = "select qset_id,question_name from tblquestion where question_id = '" & QuestionsId & "'"
                    'Dim dt3 As New DataTable
                    'dt3 = UseCls.getdata(sql1)
                    'If dt3.Rows.Count > 0 Then
                    '    Dim QuestionName As String = "" 'dt3.Rows(0)("Question_Name")
                    '    Dim QsetId As String = dt3.Rows(0)("QSet_Id").ToString()
                    '    Dim QsetType As String = ClsDroidPad.GetQsetTypeFromQuestionId(QuestionsId)
                    '    If QsetType <> "" Then
                    '        Dim dtQuestion As New DataTable
                    '        If QsetType = "6" Then
                    '            QuestionName = ClsActivity.GetQuestionSetName(QuestionsId)
                    '            QuestionName = clsPDf.CleanSetNameText(QuestionName)
                    '        Else
                    '            QuestionName = dt3.Rows(0)("Question_Name")
                    '        End If
                    '    End If
                    '    QuestionName = QuestionName.Replace("___MODULE_URL___", clsPDf.GenFilePath(dt3.Rows(0)("QSet_Id").ToString))
                    '    mainQuestion.InnerHtml = QQNo & ". " & QuestionName


                    QuestionTd.InnerHtml = ClsActivity.RenderQuestion(QuizId, StudentId, AnswerState, QQNo, False, InputConn)

                    Dim IntroQset As String = ViewIntroQsetId
                    Dim DataIntro As String = ClsActivity.RenderIntro(QuizId, StudentId, QQNo, IntroQset, InputConn)
                    If DataIntro <> "" Then
                        Dim ArrIntro() As String = Split(DataIntro, "@:@")
                        Dim TagIntro As String = ArrIntro(0)
                        Dim TypeIntro As String = ArrIntro(1)
                        Dim LimitAmount As String = ArrIntro(2)
                        ViewIntroQsetId = ArrIntro(3)
                        ShowLimitAmount.InnerHtml = LimitAmount
                        If TypeIntro = "2" Then
                            mainIntro.InnerHtml = TagIntro
                        Else
                            mainIntro.InnerHtml = "<span id=""spnTest"">Clickkkkkkkkkkkkkkkkkkkk</span>"""
                            introHtml.InnerHtml = TagIntro
                        End If

                        ' ปิดการใช้งาน intro ไปก่อน ==> EnableIntro = True
                        EnableIntro = False
                    Else
                        EnableIntro = False
                    End If

                    'Dim TagAnswerName As String = GetAnswerDetails(QuestionsId, QsetId, Quizid, DeviceId)
                    'If _AnswerState = 2 And Session("PracticeFromcomputer") Then
                    If _AnswerState = 2 Then
                        If ClsActivity.GetNotAnswer(QuizId, StudentId, QQNo, InputConn) Then '
                            IsNoAnswer = True
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNotReply")

                        Else
                            IsNoAnswer = False
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNormal")
                        End If
                    Else
                        IsNoAnswer = False
                        MainDivPad.Attributes.Remove("Class")
                        MainDivPad.Attributes.Add("Class", "MainDivNormal")
                    End If

                    Dim TagAnswerName As String
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        TagAnswerName = ClsActivity.RenderAnswer(StudentId, AnswerState, QuizId, QQNo, False, False, Session("HomeworkMode"), True, InputConn)
                    Else
                        TagAnswerName = ClsActivity.RenderAnswer(StudentId, AnswerState, QuizId, QQNo, False, False, Session("HomeworkMode"), False, InputConn)
                    End If

                    AnswerTbl.InnerHtml = TagAnswerName
                    MyAnswer = ClsActivity.MyAnswer
                    CorrectAnswer = ClsActivity.CorrectAnswer
                    SwapStatus = ClsActivity.SwapStatus

                    If _AnswerState = 2 Then
                        If ClsActivity.GetNotAnswer(QuizId, StudentId.ToString, QQNo, InputConn) Then '
                            IsNoAnswer = True
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNotReply")
                        Else
                            IsNoAnswer = False
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNormal")
                        End If
                        AnswerExp.InnerHtml = ClsActivity.htmlAnswerExp
                        'btnMoreDetail.Style.Remove("display")
                        'btnMoreDetail.Style.Add("display", "block")
                    Else
                        IsNoAnswer = False
                        MainDivPad.Attributes.Remove("Class")
                        MainDivPad.Attributes.Add("Class", "MainDivNormal")
                        'btnMoreDetail.Style.Remove("display")
                        'btnMoreDetail.Style.Add("display", "none")

                    End If
                Else
                    CheckNoData = 1 'ไม่มีคำถาม,คำตอบ
                End If
            End If
            'End If
        Else
            CheckNoData = 1  'ไม่มี DeviceId ส่งมา
        End If
    End Sub
    Private Function GetAnswerIdByAnswerName(ByVal AnswerName As String, ByVal QuestionId As String)
        Dim AnswerId As String = ""
        If AnswerName <> "" Then
            Dim sql As String
            sql = " select top 1 Answer_Id from tblAnswer where  Answer_Name Like '%" & AnswerName.CleanSQL & "%' AND Question_Id = '" & QuestionId.CleanSQL & "' ;"
            AnswerId = UseCls.ExecuteScalar(sql)
        Else
            Return "-1"
        End If
        Return AnswerId
    End Function

    <Services.WebMethod()>
    Public Shared Function UpdateModule(ByVal IsHomework As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim Player_Id As String = HttpContext.Current.Session("_StudentId").ToString
        Dim Quiz_Id As String = HttpContext.Current.Session("_QuizId").ToString
        'เมื่อกดจบการบ้านเช็คว่าการบ้านทำครบทุกข้อมั้ย
        If IsHomework = "1" Then
            Dim ClsHomework As New ClsHomework(New ClassConnectSql)
            Dim ClsActivity As New ClsActivity(New ClassConnectSql)
            'นับว่าข้อที่ยังไม่ทำมีกี่ข้อ
            'Dim ReplyAmount = ClsActivity.CountLeapExam(Quiz_Id, Player_Id)
            'If ReplyAmount = "0" Then
            Dim StatusHomework As String = ClsHomework.TestGetStatusHomework(Quiz_Id, Player_Id)
            ClsHomework.UpdateStatusWhenEndQuiz(Quiz_Id, Player_Id, StatusHomework)
            'Else
            '    ClsHomework.UpdateStatusWhenEndQuiz(Quiz_Id, Player_Id, "2")
            'End If
        Else
            'Update End_Date ที่ tblquiz
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function UpdateExitByUser(ByVal IsHomework As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim ClsHomework As New ClsHomework(New ClassConnectSql)
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        'เมื่อกดส่งการบ้าน Update tblModuleDetailCompletion, tblQuizSession, tblQuiz
        'ต้องรู้ด้วยว่าทำครบหรือเปล่า เพื่อ Update Status ได้ 
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString
        Dim Player_Id As String = HttpContext.Current.Session("_StudentId").ToString
        'เช็คก่อนว่าสถานะอะไร แต่มันเป็น 1 อยู่แล้ว เหลือ 2 กับ 3
        'นับว่าข้อที่ยังไม่ทำมีกี่ข้อ
        Dim ReplyAmount = ClsActivity.CountLeapExam(Quiz_Id, Player_Id)
        If ReplyAmount = "0" Then
            ClsHomework.UpdateExitByUser(Quiz_Id, Player_Id, "3")
        Else
            ClsHomework.UpdateExitByUser(Quiz_Id, Player_Id, "2")
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function testChkFrom()
        HttpContext.Current.Session("testSession") = True
        Return "suc"
    End Function

    Protected Sub btnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        controlBtnPrev()
    End Sub
    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        controlBtnNext()
    End Sub
    Private Sub controlBtnPrev()
        'Dim ExamNum As Integer  'เลขข้อปัจจุบัน
        'Dim ExamAmount As Integer = "" 'จำนวนข้อทั้งหมด
        'Dim State As String = "" 'สถานะ state 0-ข้อสอบไม่มีเฉลยหรือเฉลยทีหลัง 1-ข้อสอบมีเฉลยข้อต่อข้อ 2-เฉลย

        IsSeenIntro = True

        If NotReplyMode = "True" Then

            SetValueLeapChoice(2)
            Exit Sub
        End If

        If ((ExamNum - 1) < 1) Then
            ' //ไม่ต้องทำอะไร
        Else
            ExamNum = ExamNum - 1
            '''''''''''''''''''''''''''''''''''''''''''''''
            GenPanelShowInfo(PlayerId, VQuizId, ExamNum, False) 'Panel ที่บอกจำนวนข้อที่ทำ - ข้าม - เหลือ
            '''''''''''''''''''''''''''''''''''''''''''''''
            If (_AnswerState = "0") Then
                ' // function render ปกติ
                renderExamSelfPace(ExamNum, _AnswerState)
            Else
                _AnswerState = "2"
                ' //function Render เฉลย

                renderExamSelfPace(ExamNum, _AnswerState)
            End If
            '// Tools
            questionId = ClsActivity.GetQuestionID(VQuizId, ExamNum)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId).ToString()

        End If
    End Sub

    Private Sub controlBtnNext()

        IsSeenIntro = True

        'Open Connection
        Dim ConnActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(ConnActivity)

        If HttpContext.Current.Session("_AnswerState") IsNot Nothing Then
            _AnswerState = HttpContext.Current.Session("_AnswerState")
            HttpContext.Current.Session("_AnswerState") = Nothing

            ExamNum = HttpContext.Current.Session("ExamNum")
            HttpContext.Current.Session("ExamNum") = Nothing

            HDNotReplyMode.Value = "False"
            NotReplyMode = "False"
        End If

        If NotReplyMode = "True" Then
            SetValueLeapChoice(1, ConnActivity)
            Exit Sub
        End If

        'หาจำนวนข้อทั้งหมดก่อน 
        Dim ExamAmount As String = GetTopQQNoByQuizId(VQuizId, ConnActivity) 'จำนวนข้อทั้งหมด
        If ExamAmount = "" Then
            Exit Sub
        End If

        Dim ExamForCheck As String = ""
        If ExamNum < ExamAmount Then
            ExamForCheck = (CInt(ExamNum) + 1).ToString
        Else
            ExamForCheck = (CInt(ExamAmount)).ToString()
        End If

        If ExamForCheck = ExamAmount Then
            Dialog = "True"
        Else
            Dialog = "False"
        End If

        If (ExamNum = 0) And (_AnswerState = "0") And (Session("ShowCorrectAfterCompleteState") = True) Then
            If Session("ShowCorrectAfterComplete") = True Then
                _AnswerState = "2"
                Session("ShowCorrectAfterCompleteState") = True
            End If
        End If

        If (_AnswerState = "1") And (Session("ChooseMode") <> EnumDashBoardType.Homework) Then
            _AnswerState = "2"
            'Function UpdateIsscore ExamNum
            ClsActivity.UpdateIsScore(VQuizId, ExamNum, PlayerId, ConnActivity)
        Else 'State <> 1
            If ExamNum < ExamAmount Then
                ExamNum += 1
            End If

            If _AnswerState <> "0" Then

                If Not Session("ShowCorrectAfterCompleteState") Then
                    If Session("ChooseMode") = EnumDashBoardType.Homework Then
                        _AnswerState = "2"
                    Else
                        _AnswerState = "1"
                    End If

                End If
                Dim IsSelfPace As Boolean = KnSession(VQuizId & "|" & "SelfPace")
                If VBShowScoreChoiceToChoice = True Or _AnswerState = "2" Then
                    'Function UpdateIsscore ExamNum
                    ClsActivity.UpdateIsScore(VQuizId, ExamNum, PlayerId, ConnActivity)
                End If
            End If

            'มีข้อสอบมั้ย
            If ClsActivity.MeHaveQuestion(VQuizId, ExamNum, PlayerId, ConnActivity) Then
                'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
                If KnSession(VQuizId & "|" & "SelfPace") Then
                    'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
                    If ClsActivity.HaveIsScored(VQuizId, ExamNum, ConnActivity) Then
                        'ถ้าตรวจแล้ว
                        If _AnswerState <> "0" Then
                            _AnswerState = "2"
                        End If

                        'End If
                    End If


                End If

            Else

                ClsActivity.SetQuizScoreStudent(ExamNum, VQuizId, PlayerId, ConnActivity)
                If _AnswerState = "0" And VBShowScoreChoiceToChoice = True Then
                    ClsActivity.UpdateIsScore(VQuizId, CStr(CInt(ExamNum) - 1), PlayerId, ConnActivity)
                End If
            End If

        End If

        If Not (BusinessTablet360.ClsKNSession.IsMaxONet) Then
            If IsHomeWork = True Then
                Dim state As String = Request.QueryString("Status").ToString
                If state = "3" Then
                    _AnswerState = "2"
                Else
                    _AnswerState = state
                End If

            End If
        End If

        If Not ExamNum > ExamAmount Then
            questionId = ClsActivity.GetQuestionID(VQuizId, ExamNum, ConnActivity)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, ConnActivity).ToString()

            GenPanelShowInfo(PlayerId, VQuizId, ExamNum, , ConnActivity)
            renderExamSelfPace(ExamNum, _AnswerState, ConnActivity)
        End If

        'Close Connection
        UseCls.CloseExclusiveConnect(ConnActivity)
    End Sub

    ''' <summary>
    ''' controlBtnNext เดิมก่อนลบ Comment ทิ้งเพื่อจะได้ไล่โค้ดง่ายๆ
    ''' </summary>
    Private Sub controlBtnNext_old()

        IsSeenIntro = True

        'Open Connection
        Dim ConnActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(ConnActivity)

        'If Not HttpContext.Current.Session("ExamNum") Is Nothing Then
        '    ExamNum = HttpContext.Current.Session("ExamNum")
        '    _AnswerState = HttpContext.Current.Session("_AnswerState")
        '    HttpContext.Current.Session("ExamNum") = Nothing
        '    HttpContext.Current.Session("_AnswerState") = Nothing
        'End If

        If HttpContext.Current.Session("_AnswerState") IsNot Nothing Then
            _AnswerState = HttpContext.Current.Session("_AnswerState")
            HttpContext.Current.Session("_AnswerState") = Nothing
            ExamNum = HttpContext.Current.Session("ExamNum")
            HttpContext.Current.Session("ExamNum") = Nothing

            HDNotReplyMode.Value = "False"
            NotReplyMode = "False"
        End If

        If NotReplyMode = "True" Then

            SetValueLeapChoice(1, ConnActivity)
            Exit Sub

        End If

        '17/11/2015 ย้าย Funcion เก็บเวลาแต่ละข้อไปไว้ตอนเด็กกดตอบแทน
        'GetTextForDialog()

        ' เก็บเวลาเพื่อหาเวลาที่ใช้ไป
        'Session("EndTimeQuestion") = Date.Now

        'ClsActivity.SetTotalTime(Session("StartTimeQuestion"), Session("EndTimeQuestion"), VQuizId, ExamNum, PlayerId, ConnActivity)
        'ClsActivity.SetTotalScore(VQuizId, PlayerId, ConnActivity)


        'หาจำนวนข้อทั้งหมดก่อน 
        Dim ExamAmount As String = GetTopQQNoByQuizId(VQuizId, ConnActivity) 'จำนวนข้อทั้งหมด
        If ExamAmount = "" Then
            Exit Sub
        End If

        'สถานะ state 0-ข้อสอบไม่มีเฉลยหรือเฉลยทีหลัง 1-ข้อสอบมีเฉลยข้อต่อข้อ 2-เฉลย

        'If ExamNum = ExamAmount Then
        '    Dialog = "True"
        'Else
        '    Dialog = "False"
        'End If

        'เช็คว่าทำจบแล้วหรือยัง

        'If Session("CheckLastQuestion") = False Then             ้
        Dim ExamForCheck As String = ""
        'If Session("ChooseMode") = EnumDashBoardType.Practice Then
        If ExamNum < ExamAmount Then
            ExamForCheck = (CInt(ExamNum) + 1).ToString
        Else
            ExamForCheck = (CInt(ExamAmount)).ToString()
        End If

        'ElseIf Session("ChooseMode") = EnumDashBoardType.Quiz Then
        '    ExamForCheck = (ExamNum).ToString
        'End If

        If ExamForCheck = ExamAmount Then
            Dialog = "True"

        Else
            Dialog = "False"
        End If

        If (ExamNum = 0) And (_AnswerState = "0") And (Session("ShowCorrectAfterCompleteState") = True) Then
            If Session("ShowCorrectAfterComplete") = True Then
                _AnswerState = "2"
                Session("ShowCorrectAfterCompleteState") = True
            End If
        End If

        'ExamNum = (CInt(ExamNum) + 1).ToString()
        '''''''''''''''''''''''''''''''''''''''''''''''
        'GenPanelShowInfo(PlayerId, VQuizId, ExamNum) 'Panel ที่บอกจำนวนข้อที่ทำ - ข้าม - เหลือ
        '''''''''''''''''''''''''''''''''''''''''''''''
        '----------------------------------------------------------------------------------
        'If (_AnswerState = "1") Then
        '    _AnswerState = "2"
        '    ' //function Render เฉลย
        '    renderExamSelfPace(ExamNum, _AnswerState)
        '    ' //function update score ของ PlayerId คนที่เล่นอยู่
        '    If ClsActivity.UpdateIsScore(VQuizId, CInt(ExamNum) - 1, PlayerId) = "-1" Then
        '        Exit Sub
        '    End If
        '    Else
        '    checkStateForRender(ExamNum, _AnswerState)
        'End If
        '------------------------------------------------------------------------------------

        If (_AnswerState = "1") And (Session("ChooseMode") <> EnumDashBoardType.Homework) Then
            _AnswerState = "2"
            'Function UpdateIsscore ExamNum
            ClsActivity.UpdateIsScore(VQuizId, ExamNum, PlayerId, ConnActivity)
        Else 'State <> 1
            If ExamNum < ExamAmount Then
                ExamNum += 1
            End If

            If _AnswerState <> "0" Then

                If Not Session("ShowCorrectAfterCompleteState") Then
                    If Session("ChooseMode") = EnumDashBoardType.Homework Then
                        _AnswerState = "2"
                    Else
                        _AnswerState = "1"
                    End If

                End If
                Dim IsSelfPace As Boolean = KnSession(VQuizId & "|" & "SelfPace")
                If VBShowScoreChoiceToChoice = True Or _AnswerState = "2" Then
                    'Function UpdateIsscore ExamNum
                    ClsActivity.UpdateIsScore(VQuizId, ExamNum, PlayerId, ConnActivity)
                End If
            End If

            'มีข้อสอบมั้ย
            If ClsActivity.MeHaveQuestion(VQuizId, ExamNum, PlayerId, ConnActivity) Then
                'ถ้ามีข้อสอบ ให้เช็คว่าไปพร้อมกันมั้ย
                If KnSession(VQuizId & "|" & "SelfPace") Then
                    'ถ้าไปไม่พร้อมกัน ให้เช็คว่าเป็นฝึกฝนจากคอมมั้ย
                    'If HttpContext.Current.Session("PracticeFromComputer") Then
                    '    'เป็นฝึกฝนจากคอมให้ SetQuizScore
                    '    If _AnswerState <> "2" And Not ClsActivity.HaveQuestion(VQuizId, ExamNum) Then
                    '        ClsActivity.SetQuizScore(ExamNum, VQuizId, Session("PracticeMode"), Session("PracticeFromComputer"))
                    '    End If

                    'Else
                    'ไปไม่พร้อมกัน ไม่ได้ฝึกฝนจากคอม
                    If ClsActivity.HaveIsScored(VQuizId, ExamNum, ConnActivity) Then
                        'ถ้าตรวจแล้ว
                        If _AnswerState <> "0" Then
                            _AnswerState = "2"
                        End If

                        'End If
                    End If


                End If

            Else
                'ClsActivity.SetQuizScore(ExamNum, VQuizId, Session("PracticeMode"), Session("PracticeFromComputer"))
                ClsActivity.SetQuizScoreStudent(ExamNum, VQuizId, PlayerId, ConnActivity)
                'If _AnswerState = "0" And Not KnSession(VQuizId &"|"& "NeedShowScoreAfterComplete") Then
                If _AnswerState = "0" And VBShowScoreChoiceToChoice = True Then
                    'Function UpdateIsscore ExamNum - 1
                    ClsActivity.UpdateIsScore(VQuizId, CStr(CInt(ExamNum) - 1), PlayerId, ConnActivity)
                End If
            End If

        End If

        If Not (BusinessTablet360.ClsKNSession.IsMaxONet) Then
            If IsHomeWork = True Then
                Dim state As String = Request.QueryString("Status").ToString
                If state = "3" Then
                    _AnswerState = "2"
                Else
                    _AnswerState = state
                End If

            End If
        End If


        'If (ExamNum = ExamAmount) And (_AnswerState = "0") Then
        '    If Session("ShowCorrectAfterComplete") = True Then
        '        Dialog = "False"
        '    Else
        '        Dim LeapExam As String = ClsActivity.CountLeapExam(VQuizId, sharePlayerId)
        '        DialogTitle = setTitleDialog(LeapExam)
        '        Dialog = "True"
        '    End If
        'ElseIf (ExamNum = ExamAmount) And (_AnswerState = "2") Then
        '    DialogTitle = setTitleDialog("0")
        '    Dialog = "True"
        'Else
        '    Dialog = "False"
        'End If



        ' If (ExamNum = ExamAmount) And (_AnswerState = "0" Or _AnswerState = "2") Then 'เพิ่ม And HaveQuestion ด้วย เพื่อให้รู้ว่าเคยมาถึงข้อนี้มั้ย

        ' ถ้า state ไม่เป็น 0 และ ไม่มีเฉลยตอนท้าย
        'If Not (_AnswerState = "0" And ShowCorrectAfterCompleteState) Then
        'If LeapExam = "0" Then
        '    DialogTitle = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยไหมคะ ?"
        'Else
        '    DialogTitle = "ยังไม่ได้ทำข้อสอบอีก " & LeapExam & " ข้อ จบควิซเลยไหมคะ ?"
        'End If


        'If (ExamNum = ExamAmount) And (_AnswerState = "0") Then
        '    If Session("ShowCorrectAfterComplete") = True Then
        '        ExamNum = 0
        '        _AnswerState = "2"
        '        ShowCorrectAfterCompleteState = True
        '    End If
        'End If

        'End If
        'End If

        '// Tools

        If _AnswerState = "2" Then
            Dim n As Integer = CInt(OrderNo.Value)
            OrderNo.Value = n + 1

            Dim arrAnsNo(ExamAmount) As String

            If ArrNo.Value = "" Then
                arrAnsNo(0) = "1"
                ExamNum = "1"
            Else
                arrAnsNo = Split((ArrNo.Value), ",")
                ExamNum = arrAnsNo(CInt(OrderNo.Value))
            End If

        End If


        If Not ExamNum > ExamAmount Then

            questionId = ClsActivity.GetQuestionID(VQuizId, ExamNum, ConnActivity)
            hdIsGroupEng.Value = ClsActivity.IsGroupSubjectEng(questionId, ConnActivity).ToString()

            GenPanelShowInfo(PlayerId, VQuizId, ExamNum, , ConnActivity)
            renderExamSelfPace(ExamNum, _AnswerState, ConnActivity)



        End If

        'Close Connection
        UseCls.CloseExclusiveConnect(ConnActivity)
        'If HttpContext.Current.Session("ShowSelectExamPanel") = True Then
        '    divSelectExplain.Style.Add("display", "block")
        '    Dim scriptKey As String = "UniqueKeyForThisScript"
        '    Dim javaScript As String = "<script type='text/javascript'>CreateButtonSelectExplain();</script>"
        '    ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)
        '    HttpContext.Current.Session("ShowSelectExamPanel") = False
        'Else
        '    divSelectExplain.Style.Add("display", "none")
        'End If

    End Sub
    Private Sub checkStateForRender(ByVal ExamNum As String, ByVal State As String)
        Dim KnSession As New ClsKNSession()
        Dim IsHasQuestion As Boolean = CheckIsHaveQuestionOrCheckIsScore(ExamNum, False) ' ค่าที่ return มาจาก query ที่เช็คว่ามีข้อสอบหรือยัง
        Dim IsScored As Boolean = CheckIsHaveQuestionOrCheckIsScore(ExamNum, True) 'ค่าที่บอกว่าข้อสอบตรวจหรือยัง
        Dim IsShowScoreAfterComplete As Boolean = KnSession(VQuizId & "|" & "NeedShowScoreAfterComplete") 'ค่าที่บอกว่าโชว์คะแนนข้อต่อข้อหรือเปล่า

        If (State <> 0) Then
            State = "1"
        End If

        If (IsHasQuestion) Then
            If (IsScored) Then
                If (State = "0") Then
                    ' //function Render ปกติ
                    renderExamSelfPace(ExamNum, State)
                Else
                    State = "2"
                    ' //function Render เฉลย
                    renderExamSelfPace(ExamNum, State)
                    ' //function update score ของ PlayerId คนที่เล่นอยู่
                End If
            Else
                renderExamSelfPace(ExamNum, State)
            End If
        Else
            '//function insert tblquizscore ของ playerId คนที่เล่นอยู่
            ClsActivity.SetQuizScoreStudent(ExamNum, VQuizId, PlayerId)
            If (State = "0" AndAlso IsShowScoreAfterComplete) Then
                ' //function update score ข้อที่แล้วของ PlayerId คนที่เล่นอยู่
                If ClsActivity.UpdateIsScore(VQuizId, CInt(ExamNum) - 1, PlayerId) = "-1" Then
                    Exit Sub
                End If
                ' //function Render ปกติ
                renderExamSelfPace(ExamNum, State)
            Else
                renderExamSelfPace(ExamNum, State)
            End If
        End If
    End Sub
    Private Sub renderExamSelfPace(ByVal ExamNum As String, ByVal _AnswerState As String, Optional ByRef InputConn As SqlConnection = Nothing)

        'If Request.QueryString("DeviceUniqueID") = Nothing Then

        '    'Response.Redirect("../Activity/EmptySession.aspx")
        'End If

        Dim DeviceId As String = Request.QueryString("DeviceUniqueID").ToString() '"teacher1" 

        If HttpContext.Current.Application("Sess" & DeviceId) Is Nothing Then
            'Response.Redirect("../Activity/EmptySession.aspx")
        End If

        If Session("ChooseMode") = EnumDashBoardType.Practice Then
            IsPracticeMode = True
        Else
            IsPracticeMode = False
        End If

        If DeviceId <> "" Then
            Dim dt1 As New DataTable
            Dim Quiz_Id As String = ""
            Dim Student_Id As String = ""
            If Session("ChooseMode") = EnumDashBoardType.Homework Or Session("ChooseMode") = EnumDashBoardType.Practice Then
                Quiz_Id = VQuizId
                Student_Id = Session("UserID")
                Session("_QuizId") = Quiz_Id
            Else
                'dt1 = GetQuizFromDeviceUniqueID(DeviceId) 'ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceId)
                'Quiz_Id = dt1.Rows(0)("Quiz_Id").ToString()
                'Student_Id = dt1.Rows(0)("Player_Id").ToString()
                'Session("_QuizId") = Quiz_Id

                'Dim redis As New RedisStore()
                Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
                If q IsNot Nothing Then
                    Quiz_Id = q.QuizId
                    Student_Id = q.PlayerId
                    Session("_QuizId") = Quiz_Id
                    VQuizId = Quiz_Id
                End If
            End If


            If Quiz_Id <> "" Then

                QuestionTd.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Student_Id, _AnswerState, ExamNum, True, InputConn)

                'Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, Student_Id, ExamNum, ViewIntroQsetId)
                Dim DataIntro As String = ClsActivity.RenderIntro(Quiz_Id, Student_Id, ExamNum, ViewIntroQsetId, InputConn)
                If DataIntro <> "" Then
                    Dim ArrIntro() As String = Split(DataIntro, "@:@")
                    Dim TagIntro As String = ArrIntro(0)
                    Dim TypeIntro As String = ArrIntro(1)
                    Dim LimitAmount As String = ArrIntro(2)
                    ViewIntroQsetId = ArrIntro(3)


                    ShowLimitAmount.InnerHtml = LimitAmount

                    If TypeIntro = "2" Then
                        mainIntro.InnerHtml = TagIntro
                    Else
                        mainIntro.InnerHtml = "<span id=""spnTest"">Clickkkkkkkkkkkkkkkkkkkk</span>"""
                        introHtml.InnerHtml = TagIntro
                    End If

                    ' ปิดการใช้งาน intro ไปก่อน ==> EnableIntro = True
                    EnableIntro = False
                        Else
                        EnableIntro = False
                    End If
                    If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Student_Id, _AnswerState, Quiz_Id, ExamNum, IsPracticeMode, True, Session("HomeworkMode"), True, InputConn)
                    Else
                        AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Student_Id, _AnswerState, Quiz_Id, ExamNum, IsPracticeMode, True, Session("HomeworkMode"), False, InputConn)
                    End If

                    MyAnswer = ClsActivity.MyAnswer
                    CorrectAnswer = ClsActivity.CorrectAnswer
                    SwapStatus = ClsActivity.SwapStatus

                    If _AnswerState = "2" Then

                        SetLeapChoice(Quiz_Id, InputConn)

                        If ClsActivity.GetNotAnswer(Quiz_Id, Student_Id.ToString, ExamNum, InputConn) Then 'And (ExamNum <= HttpContext.Current.Session("LeapChoiceAmount").ToString)
                            IsNoAnswer = True
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNormal")

                        Else
                            IsNoAnswer = False
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNormal")
                        End If
                        AnswerExp.InnerHtml = ClsActivity.htmlAnswerExp
                    Else

                        IsNoAnswer = False
                        If HDLastChoice.Value = "True" Then
                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNormal")
                        ElseIf HDNotReplyMode.Value = "True" Then

                            MainDivPad.Attributes.Remove("Class")
                            MainDivPad.Attributes.Add("Class", "MainDivNotReply")

                        End If
                    End If

                End If

            End If

    End Sub
    Private Function GetQuizFromDeviceUniqueID(ByVal DeviceUniqueID As String) As DataTable
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable
        Dim sql As String = " select top 1 tqs.Quiz_Id,tqs.Player_Id,tqs.School_Code from t360_tblTablet tt inner join t360_tblTabletOwner tto "
        sql &= " on tt.Tablet_Id = tto.Tablet_Id inner join tblQuizSession tqs on tqs.Tablet_Id = tto.Tablet_Id "
        sql &= " where tt.Tablet_SerialNumber = '" & db.CleanString(DeviceUniqueID.Trim()) & "'  order by tqs.LastUpdate desc ; "
        dt = db.getdata(sql)
        Return dt
    End Function

    Public sizeDivDestination As Integer 'ขนาดของ size div destination
    Public sizeBtnNext As Integer 'play mode time per question
    Private Sub GenPanelShowInfo(ByVal PlayerId As String, ByVal QuizId As String, ByVal ExamNum As String, Optional ByVal IsNextBtn As Boolean = True, Optional ByRef InputConn As SqlConnection = Nothing)

        If PlayerId Is Nothing Or PlayerId = "" Or QuizId Is Nothing Or QuizId = "" Then
            Exit Sub
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Dim redis As New RedisStore()
        Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
        'If q IsNot Nothing Then

        Dim MaxmimumWidth As Integer  'ความกว้างของ Div ที่เอามาเป็นตัว * ถ้ามีการลดเพิ่มขนาดต้องมาเปลี่ยนที่นี่ **********
        ' check ว่าเปิดงานเช็คสกอ กับ เครื่องมือหรือเปล่า
        If Session("showScore") = True AndAlso (Session("ShowCorrectAfterCompleteState") = False) AndAlso UseTools = True Then
            sizeDivDestination = 429
            MaxmimumWidth = 338
        ElseIf Session("showScore") = True Or UseTools = True Then
            sizeDivDestination = 526
            MaxmimumWidth = 435
        Else
            sizeDivDestination = 623
            MaxmimumWidth = 531
        End If

        If IsPerQuestion Then
            sizeBtnNext = 310
        Else
            If IsShowBtnCompleteHomework Then
                sizeBtnNext = 135
            Else
                sizeBtnNext = 155
            End If
        End If

        Dim sql As String = ""
        Dim TotalQuestion As String = "" 'หาจำนวนข้อสอบทั้งหมดว่ามีกี่ข้อ
        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
            TotalQuestion = q.AmountQuestion
        Else
            sql = " select MAX(qq_No) from tblQuizQuestion where Quiz_Id = '" & QuizId & "' "
            TotalQuestion = UseCls.ExecuteScalar(sql, InputConn)
            If HttpContext.Current.Application(QuizId & "|Max(qq_No)") Is Nothing Then
                HttpContext.Current.Application.Lock()
                HttpContext.Current.Application(QuizId & "|Max(qq_No)") = UseCls.ExecuteScalar(sql, InputConn)
                HttpContext.Current.Application.UnLock()
            End If
            TotalQuestion = HttpContext.Current.Application(QuizId & "|Max(qq_No)")
        End If

        If TotalQuestion = "" Then
            Exit Sub
        End If
        spnTotalQuestion.InnerHtml = TotalQuestion
        Dim pos_spanCenter As Integer = (MaxmimumWidth / 2) - 2
        'spnTotalQuestion.Style.Add("margin-left", pos_spanCenter & "px")
        spnTotalQuestion.Style.Add("margin-left", "auto")
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim Done As String = "" 'หาจำนวนข้อที่ทำไปแล้ว
        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
            If q.NoOfDone Is Nothing Then
                Done = "0"
            Else
                Done = CStr(q.NoOfDone.Count())
            End If
        Else
            sql = " SELECT COUNT(QuizScore_Id) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' AND Answer_Id IS NOT NULL AND Student_Id = '" & PlayerId.CleanSQL & "' And IsActive = '1'; "
            Done = UseCls.ExecuteScalar(sql, InputConn)
        End If

        If Done = "" Then
            Exit Sub
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        SetLeapChoice(QuizId, InputConn)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim SetWidth As Double = 0
        'สูตรคือ เอาจำนวนข้อที่... / ข้อทั้งหมด * ความกว้าง
        'หา Width ให้กับ Div ข้อที่ทำไปแล้ว
        'SetWidth = ((CInt(Done) / CInt(TotalQuestion)) * MaxmimumWidth)
        SetWidth = ((CInt(Done) / CInt(TotalQuestion)) * 100)
        DivDone.Style.Add("width", SetWidth & "%")
        If SetWidth > 0 Then
            DivDone.InnerHtml = ""
        Else
            DivDone.InnerHtml = ""
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'หา Width ให้กับ Div ข้อที่ข้าม
        'SetWidth = (CInt(PassChoice) / CInt(TotalQuestion) * MaxmimumWidth)
        'DivPass.Style.Add("width", SetWidth & "px")
        'If SetWidth > 0 Then
        '    DivPass.InnerHtml = "ข้าม " & PassChoice
        'Else
        '    DivPass.InnerHtml = ""
        'End If

        'หา Width ให้กับ Div ข้อที่เหลือ
        'Dim ResultQuestion As Integer = 0
        'Dim DoneAndPass As Double = CInt(Done) + CInt(PassChoice)
        'If CInt(TotalQuestion) > CInt(DoneAndPass) Then
        '    ResultQuestion = CInt(TotalQuestion) - (CInt(Done) + CInt(PassChoice))
        'Else
        '    ResultQuestion = (CInt(Done) + CInt(PassChoice)) - CInt(TotalQuestion)
        'End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'หา Width ให้กับ Div ข้อที่เหลือ
        Dim ResultQuestion As Integer = 0
        If CInt(TotalQuestion) > CInt(Done) Then
            ResultQuestion = CInt(TotalQuestion) - CInt(Done)
        Else
            ResultQuestion = CInt(Done) - CInt(TotalQuestion)
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'SetWidth = (ResultQuestion / CInt(TotalQuestion)) * MaxmimumWidth
        SetWidth = MaxmimumWidth
        'DivResult.Style.Add("width", SetWidth & "px")
        'If SetWidth > 0 Then
        '    DivResult.InnerHtml = ""
        'Else
        '    DivResult.InnerHtml = ""
        'End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If _AnswerState = 2 Then
            imgHere.Style.Add("visibility", "hidden")
        Else
            imgHere.Style.Add("visibility", "hidden")
            'Dim OffSetImg As Integer = 36 'หาค่า OffSet ของรูปมาก่อน เปลี่ยนรูปต้องหาขนาดมาแล้วแก้ตรงนี้ ******************
            ''SetWidth = ((CInt(Done) + CInt(PassChoice)) / CInt(TotalQuestion)) * MaxmimumWidth
            'SetWidth = (ExamNum / CInt(TotalQuestion)) * MaxmimumWidth
            'SetWidth = SetWidth + OffSetImg

            'imgHere.Src = If((BusinessTablet360.ClsKNSession.IsMaxONet), "../images/Activity/NowChoice.PNG", "../images/Activity/now.PNG")
            'If Not BusinessTablet360.ClsKNSession.IsMaxONet Then SetWidth += 7
            'imgHere.Style.Add("margin-left", SetWidth & "px")
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If _AnswerState = 2 Then
            ExamNumSpan.Style.Add("visibility", "hidden")
        Else
            ExamNumSpan.Style.Add("visibility", "hidden")
            'Dim OffSetSpan As Integer = 44 'ถ้าเปลี่ยนรูปต้องมาแก้ Offset ของ Span ด้วย *******************
            ''SetWidth = ((CInt(Done) + CInt(PassChoice)) / CInt(TotalQuestion)) * MaxmimumWidth
            'SetWidth = (ExamNum / CInt(TotalQuestion)) * 100
            'SetWidth = SetWidth + OffSetSpan
            'ExamNumSpan.Style.Add("margin-left", SetWidth & "%")
            'ExamNumSpan.InnerHtml = ExamNum
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''   
        'End If

    End Sub
    Private Sub SetLeapChoice(QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim PassChoice As String = "" 'หาจำนวนข้อที่ข้าม
        Dim sql As String
        If _AnswerState = "2" Or Request.QueryString("Status") = "1" Then
            If HttpContext.Current.Session("LeapChoiceAmount") Is Nothing Then
                sql = " SELECT COUNT(QuizScore_Id) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' AND Answer_Id IS NULL AND " &
                                    " Student_Id = '" & PlayerId.CleanSQL & "' And IsActive = '1';"
                PassChoice = UseCls.ExecuteScalar(sql, InputConn)
                HttpContext.Current.Session("LeapChoiceAmount") = PassChoice

            End If

        Else
            'sql = " SELECT COUNT(QuizScore_Id) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' AND Answer_Id IS NULL AND " &
            '       " Student_Id = '" & PlayerId.CleanSQL & "' And IsActive = '1' AND QQ_No <> (SELECT TOP 1 QQ_No FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' " &
            '      " AND Student_Id = '" & PlayerId.CleanSQL & "' ORDER BY QQ_No DESC ) "

            sql = "SELECT COUNT(QuizScore_Id) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' AND Answer_Id IS NULL AND Student_Id = '" & PlayerId.CleanSQL & "' And IsActive = '1';"
            PassChoice = UseCls.ExecuteScalar(sql, InputConn)
            If LeapQuestions Is Nothing Then LeapQuestions = New Dictionary(Of Integer, Integer)
            Dim q = LeapQuestions.Where(Function(f) f.Key = ExamNum)
            If Not q.Any() Then
                PassChoice = PassChoice - 1
                LeapQuestions.Add(ExamNum, 1)
            Else
                LeapQuestions(ExamNum) += 1
            End If
            HttpContext.Current.Session("LeapQuestions") = LeapQuestions
        End If

        If PassChoice = "" Then
            Exit Sub
        End If
        If CInt(PassChoice) > 0 Then
            CheckIsHaveLeapChoice.Value = 1
            QuantityLeapChoice.Value = CInt(PassChoice)
        Else
            CheckIsHaveLeapChoice.Value = 0
        End If
    End Sub
    Private Function GetQuizIdByDeviceId(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim QuizId As String = ""
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return QuizId
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT TOP 1 tblQuiz.Quiz_Id FROM t360_tblTablet INNER JOIN " &
                            " tblQuizSession ON t360_tblTablet.Tablet_Id = tblQuizSession.Tablet_Id INNER JOIN " &
                            " tblQuiz ON tblQuizSession.Quiz_Id = tblQuiz.Quiz_Id " &
                            " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "' ORDER BY tblquiz.LastUpdate desc "
        'QuizId = _DB.ExecuteScalar(sql, InputConn)
        If HttpContext.Current.Application(DeviceId & "|QuizId") Is Nothing Then
            HttpContext.Current.Application(DeviceId & "|QuizId") = _DB.ExecuteScalar(sql, InputConn)
        End If
        QuizId = HttpContext.Current.Application(DeviceId & "|QuizId")

        _DB = Nothing
        Return QuizId
    End Function
    Private Function GetPlayerIdByDeviceId(ByVal DeviceId As String, Optional ByVal QuizId As String = "", Optional ByRef InputConn As SqlConnection = Nothing)
        Dim PlayerId As String = ""
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return PlayerId
        End If

        Dim sql As String = ""

        sql = "select tablet_isowner from t360_tbltablet where Tablet_SerialNumber = '" & DeviceId.CleanSQL & "'"

        Dim IsOwner As String = UseCls.ExecuteScalar(sql, InputConn)

        If IsOwner = "False" Then
            sql = " Select tblQuizSession.Player_Id FROM tblQuizSession INNER JOIN t360_tblTablet ON tblQuizSession.Tablet_Id = t360_tblTablet.Tablet_Id " &
                     " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "') AND (tblQuizSession.Quiz_Id = '" & QuizId.ToString().CleanSQL & "') "
        Else
            If QuizId <> "" Then
                sql = " Select tblQuizSession.Player_Id FROM tblQuizSession INNER JOIN t360_tblTablet ON tblQuizSession.Tablet_Id = t360_tblTablet.Tablet_Id " &
                      " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "') AND (tblQuizSession.Quiz_Id = '" & QuizId.ToString().CleanSQL & "') "
            Else
                sql = " SELECT TOP 1 t360_tblTabletOwner.Owner_Id FROM t360_tblTablet INNER JOIN " &
                      " t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
                      " WHERE  (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1); "
            End If
        End If

        PlayerId = UseCls.ExecuteScalar(sql, InputConn)
        Return PlayerId
    End Function
    Private Function GetTopQQNoByQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim QQNo As String = ""
        If QuizId Is Nothing Or QuizId = "" Then
            Return QQNo
        End If
        Dim sql As String = " SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId.CleanSQL & "' ORDER BY QQ_No DESC "
        QQNo = UseCls.ExecuteScalar(sql, InputConn)
        Return QQNo
    End Function
    Private Function CheckIsHaveQuestionOrCheckIsScore(ByVal ExamNum As String, ByVal CheckIsScore As Boolean)

        Dim sql As String = " SELECT  "
        If CheckIsScore = True Then
            sql &= " IsScored "
        Else
            sql &= " COUNT(QuizScore_Id) "
        End If
        sql &= " FROM dbo.tblQuizScore WHERE Quiz_Id = '" & VQuizId.CleanSQL & "' AND QQ_No = '" & ExamNum.CleanSQL & "' " &
               " AND Student_Id = '" & PlayerId.CleanSQL & "' "
        Dim Check As String = ""
        Check = UseCls.ExecuteScalar(sql)
        Dim Flag As Boolean

        If CheckIsScore = True Then
            If Check = "1" Then
                Flag = True
            Else
                Flag = False
            End If
        Else
            If CInt(Check) > 0 Then
                Flag = True
            Else
                Flag = False
            End If
        End If

        Return Flag

    End Function
    Private Function CheckIsHaveQuestionStudent(ByVal QuizId As String, ByVal StudentId As String, ByVal QQNo As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim IsHaveQuestion As Boolean = False
        Dim sql As String = "SELECT COUNT(*) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId.CleanSQL & "' AND QQ_No = '" & QQNo.CleanSQL & "' AND Student_Id = '" & StudentId.CleanSQL & "' "
        Dim ResultCheck As Integer = CInt(UseCls.ExecuteScalar(sql, InputConn))
        If ResultCheck > 0 Then
            IsHaveQuestion = True
        Else
            IsHaveQuestion = False
        End If
        Return IsHaveQuestion
    End Function
    Private Function getNeedTimerPerQuestion(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = " SELECT IsPerQuestionMode FROM tblQuiz WHERE Quiz_Id = '" & QuizId.CleanSQL & "';"
        getNeedTimerPerQuestion = db.ExecuteScalar(sql, InputConn)
    End Function

    'Update 11-06-56 BitWiseComparison Tools
    Private Sub getToolsInQuiz(ByVal EnabledTools As Integer)
        If (EnabledTools And 2) = 2 Then
            tools_Calculator = False ' calculator
        End If
        If (EnabledTools And 4) = 4 Then
            tools_Dictionary = False ' dictionary
        End If
        If (EnabledTools And 8) = 8 Then
            tools_WordBook = False ' wordbook
        End If
        If (EnabledTools And 16) = 16 Then
            tools_Note = False ' note
        End If
        If (EnabledTools And 32) = 32 Then
            tools_Protractor = False ' protractor
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Sub ClearAppDeviceUniqueID(ByVal DeviceUniqueID As String)
        KnSession(DeviceUniqueID & "|" & "QuizId") = Nothing
        KnSession(DeviceUniqueID & "|" & "_ExmanNum") = Nothing
        KnSession(DeviceUniqueID & "|" & "CurrentAnsState") = Nothing
        KnSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = Nothing
        HttpContext.Current.Session("ChooseMode") = Nothing
    End Sub

    'Private Function CheckQuizIsSoundLab(ByVal Quizid As String) As Boolean
    '    Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & Quizid.ToString() & "' and TabletLab_Id is not null "
    '    Dim CheckCount As String = UseCls.ExecuteScalar(sql)
    '    If CInt(CheckCount) > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    Private Function GetdtSoundlabQuizDetail(ByVal QuizId As String, ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String = " SELECT t360_tblStudent.Student_Id as Player_Id,tblQuiz.Quiz_Id,tblQuiz.t360_SchoolCode as School_Code FROM t360_tblTablet INNER JOIN " &
                            " tblTabletLabDesk ON t360_tblTablet.Tablet_Id = tblTabletLabDesk.Tablet_Id INNER JOIN " &
                            " tblQuiz ON tblTabletLabDesk.TabletLab_Id = tblQuiz.TabletLab_Id INNER JOIN  t360_tblStudent ON tblTabletLabDesk.DeskName " &
                            " = t360_tblStudent.Student_CurrentNoInRoom AND tblQuiz.t360_ClassName = t360_tblStudent.Student_CurrentClass AND " &
                            " tblQuiz.t360_RoomName = t360_tblStudent.Student_CurrentRoom And tblQuiz.t360_SchoolCode = t360_tblStudent.School_Code " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "') AND (tblQuiz.Quiz_Id = '" & QuizId.CleanSQL & "') "
        Dim dt As New DataTable
        dt = UseCls.getdata(sql, , InputConn)
        Return dt
    End Function
    'Private Sub GenHtmlVSIntro(ByVal StudentId As String, ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
    '    Dim sb As New StringBuilder()

    '    Dim OwnerImg As String



    '    Dim PhotoStatus As Boolean
    '    PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)

    '    If PhotoStatus Then

    '        OwnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/Id.jpg"

    '    Else

    '        OwnerImg = "MonsterID.axd?seed=" & StudentId & "&size=120"

    '    End If

    '    'Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/avt_large.png")
    '    'Dim OwnerImg As String = ""
    '    'If System.IO.File.Exists(OwnerPath) = True Then
    '    '    OwnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/avt_large.png" 'OwnerPath
    '    'Else
    '    '    OwnerImg = "../UserData/dummy.png"
    '    'End If

    '    Dim pathVS As String = If((BusinessTablet360.ClsKNSession.IsMaxONet), "../Images/VS.png", "../Images/Activity/VS.png")

    '    sb.Append("<div id='DivOwner' class='ForEachDivVs' style=""background-image:url('" & OwnerImg & "');background-size:contain;background-position: center;margin-right: 35px;background-repeat: no-repeat;"">")
    '    sb.Append("</div>")
    '    sb.Append("<div id='VS' class='ForEachDivVs' style=""width: 300px;height: 250px;position: relative;"">")
    '    sb.Append("<img src='" & pathVS & "' /><div style=""background-color:transparent; background-image:url('../images/activity/start.gif');background-size:cover;background-position: center;""></div></div>")
    '    Dim WinnerId As String = GetTestsetWinnerIdByQuizId(QuizId, InputConn)
    '    Dim WinnerImg As String = ""
    '    'ถ้ายังไม่มีคนยึดชุดนี้ได้ต้องไปสุ่มรุปมา 10 รูป
    '    If WinnerId = "" Then
    '        Dim randomNumber As Integer = GetRandomNumber(10)
    '        WinnerImg = "../GenAvatar/VS/" & randomNumber.ToString() & ".png"
    '    Else

    '        PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)

    '        If PhotoStatus = "1" Then

    '            WinnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/Id.jpg"

    '        Else

    '            WinnerImg = "MonsterID.axd?seed=" & StudentId & "&size=120"

    '        End If

    '        'Dim WinnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & HttpContext.Current.Session("SchoolCode").ToString() & "/{" & WinnerId & "}/avt_large.png")
    '        'If System.IO.File.Exists(WinnerPath) = True Then
    '        '    WinnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & WinnerId & "}/avt_large.png" 'OwnerPath
    '        'Else
    '        '    WinnerImg = "../UserData/dummy.png"
    '        'End If
    '    End If
    '    sb.Append("<div id='DivEnemy' class='ForEachDivVs' style=""background-image:url('" & WinnerImg & "');background-size:contain;background-position: center;margin-left:35px;background-repeat: no-repeat;"">")
    '    sb.Append("</div>")
    '    DivVs.InnerHtml = sb.ToString()
    'End Sub

    Private Sub GenHtmlVSIntro(ByVal StudentId As String, ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sb As New StringBuilder()

        Dim OwnerImg As String



        Dim PhotoStatus As Boolean
        PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)

        If PhotoStatus Then

            OwnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/Id.jpg"

        Else

            OwnerImg = "MonsterID.axd?seed=" & StudentId & "&size=120"

        End If

        'Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/avt_large.png")
        'Dim OwnerImg As String = ""
        'If System.IO.File.Exists(OwnerPath) = True Then
        '    OwnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/avt_large.png" 'OwnerPath
        'Else
        '    OwnerImg = "../UserData/dummy.png"
        'End If

        Dim pathVS As String = If((BusinessTablet360.ClsKNSession.IsMaxONet), "../Images/VS.png", "../Images/Activity/VS.png")

        sb.Append("<div id='DivOwner' class='ForEachDivVs' style=""background-image:url('" & OwnerImg & "');background-size:contain;background-position: center;margin-right: 35px;background-repeat: no-repeat;"">")
        sb.Append("</div>")
        sb.Append("<div id='VS' class='ForEachDivVs' style=""width: 300px;height: 250px;position: relative;"">")
        sb.Append("<div><img src='" & pathVS & "' /><div style=""background-color:transparent; background-image:url('../images/activity/start.gif');background-size:cover;background-position: center;""></div></div>")
        Dim WinnerId As String = GetTestsetWinnerIdByQuizId(QuizId, InputConn)
        Dim WinnerImg As String = ""
        'ถ้ายังไม่มีคนยึดชุดนี้ได้ต้องไปสุ่มรุปมา 10 รูป
        If WinnerId = "" Then
            Dim randomNumber As Integer = GetRandomNumber(10)
            WinnerImg = "../GenAvatar/VS/" & randomNumber.ToString() & ".png"
        Else

            PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)

            If PhotoStatus = "1" Then

                WinnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & StudentId & "}/Id.jpg"

            Else

                WinnerImg = "MonsterID.axd?seed=" & StudentId & "&size=120"

            End If

            'Dim WinnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & HttpContext.Current.Session("SchoolCode").ToString() & "/{" & WinnerId & "}/avt_large.png")
            'If System.IO.File.Exists(WinnerPath) = True Then
            '    WinnerImg = "../UserData/" & Session("SchoolCode").ToString() & "/{" & WinnerId & "}/avt_large.png" 'OwnerPath
            'Else
            '    WinnerImg = "../UserData/dummy.png"
            'End If
        End If
        'sb.Append("<div id='DivEnemy' class='ForEachDivVs' style=""background-image:url('" & WinnerImg & "');background-size:contain;background-position: center;margin-left:35px;background-repeat: no-repeat;"">")
        'sb.Append("</div>")
        DivVs.InnerHtml = sb.ToString()
    End Sub
    Public Function HtmlDialyActivities() As String
        Dim sb As New StringBuilder()
        Dim PhotoStatus As Boolean
        PhotoStatus = ClsUser.GetStudentHasPhoto(PlayerId)
        Dim playerImg As String = If((PhotoStatus), "../UserData/" & BusinessTablet360.ClsKNSession.DefaultSchoolCode & "/{" & PlayerId & "}/Id.jpg", "MonsterID.axd?seed=" & PlayerId & "&size=120")
        'sb.Append("<div class='divDialy' ><img src='" & playerImg & "' /></div><div class='btnToDialy' style=""background-color:transparent; background-image:url('../images/activity/start.gif');background-size:cover;background-position: center;""><span>เริ่มกันเลย</span></div>")
        sb.Append("<div class='divDialy' ><img src='" & playerImg & "' /><div class='btnToDialy' style=""background-color:transparent; background-image:url('../images/activity/start.gif');background-size:cover;background-position: center;""><span > </span></div></div>")
        Return sb.ToString()
    End Function
    Private Function GetRandomNumber(ByVal MaxValue As Integer) As Integer
        Dim rdm As New Random
        Dim NumberRandom As Integer = rdm.Next(1, MaxValue + 1)
        Return NumberRandom
    End Function
    Private Function GetTestsetWinnerIdByQuizId(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " SELECT TSW_StudentID FROM dbo.tblTestSetWinner INNER JOIN dbo.tblQuiz " &
                            " ON dbo.tblTestSetWinner.TestSet_ID = dbo.tblQuiz.TestSet_Id " &
                            " WHERE dbo.tblQuiz.Quiz_Id = '" & QuizId.CleanSQL & "' AND dbo.tblTestSetWinner.SchoolId = '" & HttpContext.Current.Session("SchoolCode").ToString().CleanSQL & "' " &
                            " AND IsStandard = '0' "
        Dim WinnerId As String = UseCls.ExecuteScalar(sql, InputConn)
        Return WinnerId
    End Function

    <Services.WebMethod()>
    Public Shared Function GetCurrentTime(ByVal IsStart As Integer) As String
        'If HttpContext.Current.Application(HttpContext.Current.Session("Quiz_Id").ToString() & "|Time") IsNot Nothing Then
        '    HttpContext.Current.Application.Lock()
        '    Dim t As Dictionary(Of String, String) = HttpContext.Current.Application(HttpContext.Current.Session("Quiz_Id").ToString() & "|Time")
        '    If t.ContainsKey(HttpContext.Current.Session("UserId").ToString()) Then
        '        t.Add(HttpContext.Current.Session("UserId").ToString() & "_2", DateTime.Now.ToString())
        '    Else
        '        t.Add(HttpContext.Current.Session("UserId").ToString(), DateTime.Now.ToString())
        '    End If

        '    HttpContext.Current.Application(HttpContext.Current.Session("Quiz_Id").ToString() & "|Time") = t
        '    HttpContext.Current.Application.UnLock()
        'End If
        'If IsStart = 1 Then
        '    HttpContext.Current.Application(HttpContext.Current.Session("de").ToString() & "_ClientStartEnd") = DateTime.Now().ToString("hh:mm:ss:fff")
        'Else
        '    HttpContext.Current.Application(HttpContext.Current.Session("de").ToString() & "_ClientStartEnd") &= " ------ " & DateTime.Now().ToString("hh:mm:ss:fff")
        'End If
        If HttpContext.Current.Application("ReloadTime") IsNot Nothing Then
            HttpContext.Current.Application.Lock()
            Dim t As Dictionary(Of String, String) = HttpContext.Current.Application("ReloadTime")
            If t.ContainsKey(HttpContext.Current.Session("de").ToString()) Then
                t(HttpContext.Current.Session("de").ToString()) = t(HttpContext.Current.Session("de").ToString()) & " " & DateTime.Now().ToString("hh:mm:ss:fff")
            End If
            HttpContext.Current.Application("ReloadTime") = t
            HttpContext.Current.Application.UnLock()
        End If
        Return DateTime.Now.ToString()
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

        Dim QuestionId As String = hdQuestionId.Value

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

        'Dim QuestionDetail As String
        'QuestionDetail = "แจ้งปัญหาข้อสอบ<br>วิชา : " & SubjectName & " ชั้น : " & LevelShortName & "<br>"
        'QuestionDetail &= "บท : " & QCategory & " คำสั่ง : " & Qset & "<br>"
        'QuestionDetail &= "ข้อที่ : " & QNo & " (" & QId & ")<br>"
        'QuestionDetail &= "รหัสควิซ : " & VQuizId & "<br><br>"

        'Dim description As String = conDB.CleanString(Request.Form("descript"))

        'If description <> "" Then

        '    'send email
        '    Dim strBody As String = QuestionDetail & "หัวข้อ :: " & title & "<br>รายละเอียด : " & description
        '    SaveReportProblem(QuestionId, strBody, PlayerId)

        '    Dim clsTS As New ClsTestSet("")
        '    If clsTS.sendEmailToAdmin("แจ้งปัญหาข้อสอบ MaxOnet", strBody) Then
        '        DisplayAlert("แจ้งปัญหาเรียบร้อยค่ะ")
        '    Else
        '        DisplayAlert("ติดปัญหาไม่สามารถแจ้งปัญหาได้ค่ะ")
        '    End If
        'Else
        '    DisplayAlert("กรุณากรอกรายละเอียดด้วยนะคะ")
        'End If

        'Dim QuestionDetail As String
        'QuestionDetail = "แจ้งปัญหาข้อสอบ  วิชา : " & SubjectName & " ชั้น : " & LevelShortName
        'QuestionDetail &= "  บท : " & QCategory & " คำสั่ง : " & Qset
        'QuestionDetail &= "  ข้อที่ : " & QNo & " (" & QId & ")<br>"

        Dim description As String = conDB.CleanString(Request.Form("descript"))

        SaveReportProblem(QuestionId, title, description, PlayerId)

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

    Public Function SaveReportProblem(QuestionId As String, ProblemTopic As String, ProblemDetail As String, Reporter As String) As Boolean
        Try
            Dim sql As String = ""
            'sql = "insert into tblReporterProblemQuestion(RPQId,QuestionId,Annotation,ReporterId,ReportTime,ReportStatus,IsActive)
            '    values(newid(),'" & QuestionId & "','" & ProblemDetail & "','" & Reporter & "',dbo.GetThaiDate(),1,1)"

            sql = "insert into tblQuestionProblem(QuestionId,ProblemTopic,ProblemDetail,ReporterId) values ('" & QuestionId & "','" & ProblemTopic & "','" & ProblemDetail & "','" & Reporter & "')"
            conDB.Execute(sql)

            sql = "insert into tblQuestionProblemDetail(QPId,ReporterId) select top 1 QPId,'" & Reporter & "' 
                    from tblQuestionProblem where QuestionId = '" & QuestionId & "' and CurrentStatus = 1 order by lastupdate desc"
            conDB.Execute(sql)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


#Region "Bin Code -- code ใครหายตามได้ที่ activityservice นะครับ"
    '<Services.WebMethod()>
    'Public Shared Function CheckQQNoAfterLoading()
    '    If shareExamNum Is Nothing Or shareExamNum = "" Then
    '        Return "Error"
    '    End If
    '    Dim _DB As New ClassConnectSql()
    '    Dim sql As String = " Select TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & HttpContext.Current.Session("_QuizId") & "' ORDER BY lastupdate DESC; "
    '    Dim CurrentQQNo As String = _DB.ExecuteScalar(sql)
    '    If CurrentQQNo <> shareExamNum Then
    '        Return "Reload"
    '    Else
    '        Return "NotReload"
    '    End If
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function saveAnswerSortQuestion(ByVal questionIdAll As String)
    '    Dim db As New ClassConnectSql()
    '    Dim sqlCorrectAnswer, sqlMakeAnswer, sqlUpdateScore, sqlUpdateSortAnswer, sql As String
    '    Dim dtCorrectAnswer, dtMakeAnswer As DataTable
    '    ' get question ของเด็กใน tblQuizScore
    '    'sql = " SELECT Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & shareQuizId & "' AND School_Code = '" & shareSchoolId & "' AND Student_Id = '" & sharePlayerId & "' AND QQ_No = '" & shareExamNum & "';"
    '    'Dim CurrentQuestion As String = db.ExecuteScalar(sql)

    '    ' update คำตอบข้อสอบแบบเรียงลำดับตามที่เด็กตอบ
    '    Dim questionIdArr = questionIdAll.Split(",")
    '    Dim num As Integer = 1
    '    For Each q In questionIdArr
    '        'sqlUpdateSortAnswer = " UPDATE tblQuizAnswer SET Question_Id = '" & q.ToString() & "', LastUpdate = dbo.GetThaiDate() , Answer_Id = (select Answer_Id from tblanswer where Question_Id ='" & q.ToString() & "') WHERE Quiz_Id = '" & shareQuizId & "' AND QA_No = '" & num & "' AND Player_Id = '" & sharePlayerId & "' ; "
    '        sqlUpdateSortAnswer = " UPDATE tblQuizAnswer SET QA_No = '" & num & "', LastUpdate = dbo.GetThaiDate() WHERE Quiz_Id = '" & shareQuizId & "' AND Player_Id = '" & sharePlayerId & "' AND Question_Id = '" & q.ToString() & "' ;  "
    '        db.Execute(sqlUpdateSortAnswer)
    '        num = num + 1
    '    Next
    '    ' datatable คำตอบทึ่เรียงแบบถูก กับ คำตอบที่เด็กทำ
    '    'sqlCorrectAnswer = " SELECT Question_Id FROM tblAnswer WHERE Question_Id IN (SELECT Question_Id FROM tblQuizQuestion WHERE Quiz_Id = '" & shareQuizId & "' And QQ_No = '" & shareExamNum & "') ORDER BY CAST(Answer_Name as varchar); "
    '    sqlCorrectAnswer = " SELECT  dbo.tblQuizAnswer.Question_Id " & _
    '                "  FROM dbo.tblQuizAnswer INNER JOIN dbo.tblQuestion  " & _
    '                " ON dbo.tblQuizAnswer.Question_Id = dbo.tblQuestion.Question_Id WHERE dbo.tblQuizAnswer.Question_Id IN ( " & _
    '                " SELECT Question_Id FROM dbo.tblQuizQuestion WHERE QQ_No IN ( " & _
    '                " SELECT QQ_No FROM tblQuizQuestion WHERE (Quiz_Id = '" & shareQuizId & "') AND (Question_Id IN ( " & _
    '                " SELECT Question_Id FROM  tblQuizScore WHERE (Quiz_Id = '" & shareQuizId & " ') AND " & _
    '                " (Student_Id = '" & sharePlayerId & "') AND (QQ_No = '" & shareExamNum & "')))) " & _
    '                " AND dbo.tblQuizQuestion.Quiz_Id = '" & shareQuizId & "' ) " & _
    '                " AND dbo.tblQuizAnswer.Quiz_Id = '" & shareQuizId & "' AND dbo.tblQuizAnswer.Player_Id = '" & sharePlayerId & "' " & _
    '                " ORDER BY dbo.tblQuizAnswer.QA_No "
    '    dtCorrectAnswer = db.getdata(sqlCorrectAnswer)

    '    'sqlMakeAnswer = " SELECT Question_Id FROM tblQuizAnswer WHERE Quiz_Id = '" & shareQuizId & "' AND Player_Id = '" & sharePlayerId & "' ORDER BY QA_No; "
    '    sqlMakeAnswer = "select Question_Id from tblAnswer where QSet_Id = (Select QSet_Id from tblQuestion where Question_Id = ("
    '    sqlMakeAnswer &= " Select Question_Id FROM tblQuizScore WHERE Quiz_Id = '" & shareQuizId & "'  "
    '    sqlMakeAnswer &= " And QQ_No = '" & shareExamNum & "' and Student_Id = '" & sharePlayerId & "')) ORDER BY CAST(Answer_Name as varchar);"
    '    dtMakeAnswer = db.getdata(sqlMakeAnswer)
    '    ' Loop check คำตอบใน datatable ว่าเรียงถูกหรือเปล่า
    '    Dim scored As String = "1"
    '    For i As Integer = 0 To dtCorrectAnswer.Rows.Count - 1
    '        If (dtCorrectAnswer.Rows(i)("Question_Id").ToString <> dtMakeAnswer(i)("Question_Id").ToString) Then
    '            scored = "0"
    '            Exit For
    '        End If
    '    Next
    '    sql = " SELECT  tblQuizAnswer.Question_Id,tblQuizAnswer.Answer_Id FROM tblQuizScore INNER JOIN tblQuizAnswer ON tblQuizScore.Question_Id = tblQuizAnswer.Question_Id "
    '    sql &= " WHERE tblQuizScore.Quiz_Id = '" & shareQuizId & "'  "
    '    sql &= " AND tblQuizScore.School_Code = '" & shareSchoolId & "' AND tblQuizScore.Student_Id = '" & sharePlayerId & "' "
    '    sql &= " AND tblQuizScore.QQ_No = '" & shareExamNum & "' AND tblQuizAnswer.Quiz_Id = '" & shareQuizId & "' AND tblQuizAnswer.Player_Id = '" & sharePlayerId & "' ;"
    '    Dim dtQuestionAndAnswer As DataTable = db.getdata(sql)
    '    Dim CurrentQuestion As String = dtQuestionAndAnswer.Rows(0)("Question_Id").ToString()
    '    Dim CurrentAnswer As String = dtQuestionAndAnswer.Rows(0)("Answer_Id").ToString()
    '    ' Update คะแนน,responseAmount,lastupdate,isscored,score ที่ tblQuizScore
    '    sqlUpdateScore = " UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), " & _
    '        " LastUpdate = dbo.GetThaiDate(),ResponseAmount = ResponseAmount + 1,IsScored = '0',Answer_Id = '" & CurrentAnswer & "', " & _
    '        " Score = '" & scored & "' WHERE Student_Id = '" & sharePlayerId & "' AND Quiz_Id = '" & shareQuizId & "' " & _
    '        " AND School_Code = '" & shareSchoolId & "' AND Question_Id = '" & CurrentQuestion & "';"
    '    db.Execute(sqlUpdateScore)
    '    Return "success"
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function SaveAnswerPairQuestion(ByVal QuestionIdAll As String, ByVal AnswerIdAll As String) As String
    '    Dim db As New ClassConnectSql()
    '    Dim ArrQuestion As Array = QuestionIdAll.Split(",")
    '    Dim ArrAnswer As Array = AnswerIdAll.Split(",")
    '    Dim sql As New StringBuilder()
    '    Dim QA_No As Integer = 1
    '    ' Update QuizAnswer
    '    For i As Integer = 0 To ArrQuestion.Length - 1
    '        sql.Append(" UPDATE tblQuizAnswer SET Answer_Id ='")
    '        sql.Append(ArrAnswer(i))
    '        sql.Append("' WHERE Quiz_Id = '")
    '        sql.Append(shareQuizId)
    '        sql.Append("' AND Question_Id = '")
    '        sql.Append(ArrQuestion(i))
    '        sql.Append("' AND QA_No = '")
    '        sql.Append(QA_No)
    '        sql.Append("' AND Player_Id = '")
    '        sql.Append(sharePlayerId)
    '        sql.Append("';")
    '        QA_No = QA_No + 1
    '    Next
    '    db.Execute(sql.ToString())
    '    sql.Clear()
    '    sql.Append(" SELECT SUM(CASE tblAnswer.Answer_Id WHEN tblQuizAnswer.Answer_Id THEN 1 ELSE 0 END) AS Score  FROM tblAnswer ")
    '    sql.Append(" LEFT JOIN tblQuizAnswer ON tblAnswer.Question_Id = tblQuizAnswer.Question_Id ")
    '    sql.Append(" WHERE tblAnswer.Question_Id IN (")
    '    For j As Integer = 0 To ArrQuestion.Length - 1
    '        sql.Append("'")
    '        sql.Append(ArrQuestion(j))
    '        If j = ArrQuestion.Length - 1 Then
    '            sql.Append("'")
    '        Else
    '            sql.Append("',")
    '        End If
    '    Next
    '    sql.Append(") AND tblQuizAnswer.Quiz_Id = '")
    '    sql.Append(shareQuizId)
    '    sql.Append("' AND Player_Id = '")
    '    sql.Append(sharePlayerId)
    '    sql.Append("';")
    '    Dim scored As String = db.ExecuteScalar(sql.ToString())
    '    Dim CurrentAnswer As String = ArrAnswer(0)
    '    ' Update คะแนน,responseAmount,lastupdate,isscored,score ที่ tblQuizScore
    '    sql.Clear()
    '    sql.Append(" UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), LastUpdate = dbo.GetThaiDate(),ResponseAmount = ResponseAmount + 1,IsScored = '0', Answer_Id = '")
    '    sql.Append(CurrentAnswer)
    '    sql.Append("', Score = '")
    '    sql.Append(scored)
    '    sql.Append("' WHERE Student_Id = '")
    '    sql.Append(sharePlayerId)
    '    sql.Append("' AND Quiz_Id = '")
    '    sql.Append(shareQuizId)
    '    sql.Append("' AND School_Code = '")
    '    sql.Append(shareSchoolId)
    '    sql.Append("' AND QQ_No = '")
    '    sql.Append(shareExamNum)
    '    sql.Append("';")
    '    db.Execute(sql.ToString())
    '    Return ""
    'End Function

    'Public Shared Sub GetTextForDialog()
    '    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    '    Dim ClsHomework As New ClsHomework(New ClassConnectSql)
    '    ReplyAmount = ClsActivity.CountLeapExam(HttpContext.Current.Session("_QuizId"), sharePlayerId)
    '    If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework Then
    '        If ReplyAmount = "0" Then
    '            txtSendComplete = "ส่งการบ้านเรียบร้อยค่ะ"

    '            If ClsHomework.GetSendHomework(HttpContext.Current.Session("_QuizId"), sharePlayerId) Then
    '                txtExitQuiz = ""
    '            Else
    '                txtExitQuiz = "ยังไม่ได้ส่งการบ้านเลยค่ะ ออกเลยมั้ยคะ"
    '            End If

    '            txtExitQuiz = ""
    '        Else
    '            txtSendComplete = "ยังไม่ได้ทำอีก " & ReplyAmount & " ข้อ ต้องการส่งการบ้านเลยมั้ยคะ"
    '            txtExitQuiz = "ยังไม่ได้ทำอีก " & ReplyAmount & " ข้อ ออกเลยมั้ยคะ"
    '        End If
    '    Else
    '        If ReplyAmount = "0" Then
    '            txtExitQuiz = ""
    '        Else
    '            txtExitQuiz = "ยังไม่ได้ทำอีก " & ReplyAmount & " ข้อ ออกเลยมั้ยคะ"
    '        End If
    '    End If
    'End Sub

    '<Services.WebMethod()>
    'Public Shared Function SaveOnCodeBehindScore(ByVal DeviceIdFromJS As String, ByVal Questionid As String, ByVal AnswerId As String)
    '    'ถ้าอยู่ในโหมดทวนข้อข้ามแล้วกดตอบให้สร้าง Array
    '    If NotReplyMode = "True" Then
    '        HttpContext.Current.Session("IsReplyAnswer") = True
    '    End If
    '    Dim UseCls As New ClassConnectSql
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim CheckSaveSuccess As String = ""
    '    If DeviceIdFromJS Is Nothing Then
    '        Return "-1"
    '    End If
    '    If DeviceIdFromJS <> "" Then
    '        'If HttpContext.Current.Session("_StudentId") Is Nothing Then
    '        '    Dim dt As New DataTable
    '        '    dt = ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceIdFromJS)
    '        '    If dt.Rows.Count > 0 Then
    '        '        Dim StudentId As String = dt.Rows(0)("Player_Id").ToString()
    '        '        Dim QuizId As String = dt.Rows(0)("Quiz_Id").ToString()
    '        '        HttpContext.Current.Session("_QuizId") = QuizId
    '        '        HttpContext.Current.Session("_StudentId") = StudentId
    '        '    End If
    '        'End If
    '        If HttpContext.Current.Session("_StudentId") Is Nothing Then
    '            Dim QuizId As String
    '            Dim Ishomework As Boolean
    '            If HttpContext.Current.Session("HomeworkMode") Then
    '                Ishomework = True
    '            Else
    '                Ishomework = False
    '            End If
    '            QuizId = ClsDroidPad.GetQuizIdFromDeviceUniqueIDToScalar(DeviceIdFromJS, Ishomework)
    '            If QuizId <> "" Then
    '                HttpContext.Current.Session("_QuizId") = QuizId
    '            End If
    '            Dim StudentID As String = ClsDroidPad.GetPlayerIdByDeviceUniqeId(QuizId, DeviceIdFromJS)
    '            If StudentID <> "" Then
    '                HttpContext.Current.Session("_StudentId") = StudentID
    '            End If
    '        End If
    '        If HttpContext.Current.Session("_StudentId") IsNot Nothing Then
    '            Dim SrID As String = ClsDroidPad.GetSR_IdFromStudentId(HttpContext.Current.Session("_StudentId"))
    '            Dim IsSaveComplate As Boolean = ClsDroidPad.UpdateWhenStudentClick(AnswerId, HttpContext.Current.Session("_QuizId"), Questionid, HttpContext.Current.Session("_StudentId"), SrID)
    '            If IsSaveComplate = True Then
    '                CheckSaveSuccess = "1"
    '            Else
    '                CheckSaveSuccess = ""
    '            End If
    '        End If
    '    End If
    '    'GetTextForDialog()
    '    Return CheckSaveSuccess
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function NextToLeapChoice(ByVal IsCorrect As String)
    '    Dim clsActivity As New ClsActivity(New ClassConnectSql)
    '    Dim PlayerId As String = HttpContext.Current.Session("_StudentId")
    '    If IsCorrect = 1 Then
    '        shareExamNum = 0
    '        shareAnsState = "2" 'HttpContext.Current.Session("CheckLastQuestion") = False
    '        HttpContext.Current.Session("ShowCorrectAfterCompleteState") = True
    '        Dialog = "False"
    '    Else
    '        Dim UnComPlete As String = clsActivity.CountLeapExam(HttpContext.Current.Session("_QuizId").ToString, PlayerId).ToString
    '        If UnComPlete = "0" Then
    '            shareExamNum = 0
    '        Else
    '            Dim FirstLeapChoice As Integer
    '            FirstLeapChoice = CInt(clsActivity.GetFirstLeapChoice(HttpContext.Current.Session("_QuizId").ToString)) - 1
    '            shareExamNum = FirstLeapChoice
    '        End If
    '        shareAnsState = "0" 'HttpContext.Current.Session("CheckLastQuestion") = False
    '        Dialog = "False"
    '        HttpContext.Current.Session("ShowCorrectAfterCompleteState") = False
    '    End If
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function CreateStringLeapChoice(ByVal IsNormalSort As String, ByVal DeviceUniqueID As String, ByVal StudentId As String, ByVal QuizId As String) 'Function สร้างปุ่มกดข้ามข้อ
    '    'ใช้จริงต้องรับ DeviceId เพื่อเอามาหา QuizId กับ StudentId
    '    If DeviceUniqueID Is Nothing Or DeviceUniqueID = "" Or IsNormalSort Is Nothing Or IsNormalSort = "" Then
    '        Return "-1"
    '    End If
    '    Dim _DB As New ClassConnectSql
    '    Dim ClsKNSession As New ClsKNSession()
    '    Dim sql As String = ""
    '    If IsNormalSort = "True" Then 'ถ้าเรียงแบบปกติใช้คิวรี่นี้
    '        sql = " SELECT Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
    '                       " AND Student_Id = '" & StudentId & "' ORDER BY  QQ_No "
    '        'sql = " SELECT Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "'  " & _
    '        '      " AND Student_Id = '" & StudentId & "' AND QQ_No <> (SELECT TOP 1 QQ_No FROM dbo.tblQuizScore " & _
    '        '      " WHERE Quiz_Id = '" & QuizId & "' " & _
    '        '      " AND Student_Id = '" & StudentId & "' ORDER BY QQ_No DESC) " & _
    '        '      " ORDER BY  QQ_No  "
    '    Else 'ถ้าเรียงจากข้อที่ยังไม่ได้ทำขึ้นก่อนใช้คิวรี่นี้
    '        sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
    '              " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL and QQ_No <> '" & shareExamNum & "' ORDER BY Answer_Id,QQ_No) as a " '& _
    '        '      '" UNION all " & _
    '        ''" SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
    '        ''" AND Student_Id = '" & StudentId & "' AND Answer_Id IS NOT NULL ORDER BY QQ_No) as b "
    '    End If


    '    Dim dt As New DataTable
    '    dt = _DB.getdata(sql)
    '    'If dt.Rows.Count > 10 Then
    '    'IsShowNextSlide = "True"
    '    'Else
    '    'IsShowNextSlide = "False"
    '    'End If


    '    Dim CheckQuantity As Integer = 1
    '    Dim sb As New StringBuilder
    '    If dt.Rows.Count > 0 Then

    '        Dim EachQQNo As String = ""
    '        Dim EachIsScore As String = ""
    '        For i = 0 To dt.Rows.Count - 1
    '            ' StudentId = dt.Rows(i)("Student_Id").ToString() 'รับ QuestionId เพื่อเอามาเป็น Id ให้แต่ละปุ่ม
    '            EachQQNo = dt.Rows(i)("QQ_No").ToString() 'รับ QQ_No เพื่อเอามาเป็น Text ให้ปุ่ม
    '            EachIsScore = dt.Rows(i)("IsScored").ToString() 'รับ IsScore เพื่อที่จะได้รู้ว่าข้อนั้นตรวจหรือยัง
    '            If CheckQuantity <= 10 Then 'เช็คเพื่อให้สร้างปุ่มได้แค่หน้าละ 10 ปุ่มเท่านั้น
    '                If CheckQuantity = 1 Then 'ถ้าเป็นรอบแรกของการสร้างแต่ละรอบต้องสร้าง Div ขึ้นมาเพื่อครอบก่อนสร้างปุ่ม
    '                    sb.Append("<div style='width:550px;margin-top:33px;margin-left:115px;' class='slide'>")
    '                End If

    '                If (IsNormalSort = True) And (i = dt.Rows.Count - 1) And (dt.Rows(i)("Answer_Id") Is DBNull.Value) Then
    '                    sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""F"");' src='../Images/Batch_Runner-big_logo.png') id='" & EachIsScore & "' class='ForBtn'   /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                Else
    '                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then 'ถ้าข้อนี้ตอบแล้วเข้า If
    '                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
    '                            sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""F"");' src='../Images/Activity/mostWrongFace/veryhappy.png') id='" & EachIsScore & "' class='ForBtn'   /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                        Else
    '                            sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""F"");' src='../Images/Activity/mostWrongFace/veryhappy.png') ' id='" & EachIsScore & "' class='ForBtn' /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                        End If
    '                        CheckQuantity += 1
    '                    Else 'ถ้าข้อนี้ยังไม่ได้ตอบเข้า Else
    '                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
    '                            sb.Append("<div id='btnChoice' ><img onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""T"");' src='../Images/Activity/mostWrongFace/skipbadge.png' ' id='" & EachIsScore & "' class='ForBtn' /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                        Else
    '                            sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""T"");' src='../Images/Activity/mostWrongFace/skipbadge.png' id='" & EachIsScore & "' class='ForBtn' /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                        End If
    '                        CheckQuantity += 1
    '                    End If
    '                End If
    '            Else 'ถ้าเกิน 10 ปุ่มแล้วต้องขึ้นหน้าใหม่

    '                sb.Append("</div>") 'ปิด Tag Div ก่อนที่จะสร้าง Div ที่ครอบปุ่มอันต่อไป
    '                sb.Append("<div style='width:550px;margin-top:33px;margin-left:115px;' class='slide'>")
    '                If i = dt.Rows.Count - 1 Then
    '                    sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""F"");' src='../Images/Batch_Runner-big_logo.png') id='" & EachIsScore & "' class='ForBtn'   /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                Else
    '                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then
    '                        sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""F"");' src='../Images/Activity/mostWrongFace/veryhappy.png') id='" & EachIsScore & "' class='ForBtn'   /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                    Else
    '                        sb.Append("<div id='btnChoice' ><img  onclick='LeapChoiceOnclick(""" & EachQQNo & """,""" & EachIsScore & """,""T"");' src='../Images/Activity/mostWrongFace/skipbadge.png' id='" & EachIsScore & "' class='ForBtn' /><span style='font-size:20px;margin-left:25px;'>ข้อที่" & EachQQNo & "</span></div>")
    '                    End If
    '                End If
    '                CheckQuantity = 2
    '            End If
    '        Next
    '        sb.Append("</div>") 'ปิด Tag Div 
    '    Else
    '        sb.Append("<div class='NotReplyEmpty'><span style='font-size:20px;margin-left:25px;'>ไม่มีข้อข้ามแล้ว ทำข้อที่ยังไม่ทำต่อเลยค่ะ</span></div>")
    '    End If

    '    If dt.Rows.Count > 10 Then
    '        CheckOverOnePage = True
    '    Else
    '        CheckOverOnePage = False
    '    End If
    '    Return sb.ToString()
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function GetRenderForNextStep(ByVal EventType As String)
    '    Dim PlayerId As String = HttpContext.Current.Session("_StudentId")
    '    'EvenType 1 = next 2 = Exit 3 = Complete
    '    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    '    Dim UnCompleteAmount As String = ""
    '    Dim txtForShowDialog As String = ""
    '    Dim IsShowDialog As String = ""
    '    Dim DialogType As String = ""
    '    Dim NextStep As String = ""
    '    Dim QuizId As String = HttpContext.Current.Session("Quiz_Id")
    '    UnCompleteAmount = ClsActivity.CountLeapExam(HttpContext.Current.Session("_QuizId").ToString, PlayerId).ToString
    '    If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Quiz Then
    '        If _AnswerState = "0" Then
    '            If HttpContext.Current.Session("ShowCorrectAfterComplete") = True Then
    '                If UnCompleteAmount = "0" Then
    '                    IsShowDialog = "False"
    '                    txtForShowDialog = ""
    '                    DialogType = ""
    '                    NextStep = ""
    '                Else
    '                    ' บังคับดูเฉลย
    '                    IsShowDialog = "True"
    '                    txtForShowDialog = "ตอนทำได้ข้ามมา " & UnCompleteAmount & " ข้อค่ะ จะทบทวนข้อที่ข้ามมาหรือดูเฉลยเลยคะ"
    '                    DialogType = "3"
    '                    NextStep = ""
    '                End If
    '            Else
    '                If UnCompleteAmount = "0" Then
    '                    IsShowDialog = "True"
    '                    txtForShowDialog = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยมั้ยคะ ?"
    '                    DialogType = "1"
    '                    If ShareShowScoreAfterCompleteState Then
    '                        NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
    '                    Else
    '                        NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
    '                    End If
    '                Else
    '                    IsShowDialog = "True"
    '                    txtForShowDialog = "ตอนทำได้ข้ามมา " & UnCompleteAmount & " ข้อค่ะ จะทบทวนข้อที่ข้ามมาหรือจบควิซเลยคะ"
    '                    DialogType = "5"
    '                    If ShareShowScoreAfterCompleteState Then
    '                        NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
    '                    Else
    '                        NextStep = "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & DeviceId
    '                    End If
    '                End If
    '            End If
    '        ElseIf _AnswerState = "2" Then
    '            IsShowDialog = "True"
    '            txtForShowDialog = "ทำควิซหมดข้อสุดท้ายแล้วค่ะ จบควิซเลยมั้ยคะ ?"
    '            DialogType = "1"
    '            If HttpContext.Current.Session("NeedShowScoreAfterComplete") Then
    '                NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
    '            Else
    '                NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '            End If
    '        End If
    '    End If
    '    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '    If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Practice Then
    '        If _AnswerState = "0" Then
    '            If UnCompleteAmount = "0" Then
    '                IsShowDialog = "False"
    '                txtForShowDialog = ""
    '                DialogType = ""
    '                NextStep = ""
    '            Else
    '                IsShowDialog = "True"
    '                txtForShowDialog = "ตอนทำได้ข้ามมา " & UnCompleteAmount & " ข้อค่ะ จะทบทวนข้อที่ข้ามมาหรือออกจากฝึกฝนเลยคะ"
    '                DialogType = "3"
    '                NextStep = ""
    '            End If
    '        ElseIf _AnswerState = "2" Then
    '            IsShowDialog = "True"
    '            txtForShowDialog = "ทำฝึกฝนหมดข้อสุดท้ายแล้วค่ะ จบฝึกฝนเลยมั้ยคะ ?"
    '            DialogType = "1"
    '            NextStep = "../Activity/ShowScore.aspx?DeviceUniqueID=" & DeviceId & "&QuizId=" & QuizId
    '        End If
    '    End If

    '    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    '    If HttpContext.Current.Session("Choosemode") = EnumDashBoardType.Homework Then
    '        'เช็คก่อนว่าส่งยัง 
    '        'ส่งแล้วครบมั้ย
    '        If _AnswerState = "0" Then
    '            If ClsActivity.GetCompleteHomework(QuizId.ToString, PlayerId) Then
    '                'ถ้าส่งแล้ว กด next ไปต่อ กดออก ไม่มีปุ่มส่ง
    '                'EvenType 1 = next 2 = Exit 3 = Complete
    '                If EventType = "1" Then
    '                    IsShowDialog = "True"
    '                    txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ออกจากการบ้านเลยมั้ยคะ ?"
    '                    DialogType = "2"
    '                    NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '                ElseIf EventType = "2" Then
    '                    IsShowDialog = "True"
    '                    txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ออกเลยมั้ยคะ ?"
    '                    DialogType = "6"
    '                    NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '                End If
    '            Else
    '                If UnCompleteAmount = "0" Then
    '                    'ครบแล้วถ้ากดส่ง ส่งเรียบร้อย กด next ยังไม่ส่ง ส่งเลยมั้ย กดออก ยังไม่ส่ง ส่งเลยมั้ย 
    '                    'EvenType 1 = next 2 = Exit 3 = Complete
    '                    '2 ชั้น อยู่ที่เดิม
    '                    If EventType = "1" Or EventType = "3" Then
    '                        IsShowDialog = "True"
    '                        txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ส่งเลยมั้ยคะ ?"
    '                        DialogType = "6"
    '                        NextStep = ""
    '                    Else
    '                        IsShowDialog = "True"
    '                        txtForShowDialog = "ทำครบทุกข้อแล้วค่ะ ส่งเลยมั้ยคะ ?"
    '                        DialogType = "7"
    '                        NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '                    End If
    '                Else
    '                    IsShowDialog = "True"
    '                    If EventType = "1" Or EventType = "3" Then
    '                        DialogType = "4"
    '                        txtForShowDialog = "ตอนทำได้ข้ามมา " & UnCompleteAmount & " ข้อค่ะ จะทบทวนข้อที่ข้ามมาหรือส่งเลยคะ"
    '                        NextStep = ""
    '                    Else
    '                        DialogType = "5"
    '                        txtForShowDialog = "ตอนทำได้ข้ามมา " & UnCompleteAmount & " ข้อค่ะ จะทบทวนข้อที่ข้ามมาหรือออกเลยคะ"
    '                        NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '                    End If
    '                End If
    '            End If
    '        Else
    '            IsShowDialog = "True"
    '            txtForShowDialog = "ออกจากการบ้านเลยมั้ยคะ ?"
    '            DialogType = "2"
    '            NextStep = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" & DeviceId
    '        End If
    '    End If
    '    Dim JsonString
    '    JsonString = New With {.IsShowDialog = IsShowDialog, .txtForShowDialog = txtForShowDialog, .DialogType = DialogType, .NextStep = NextStep}
    '    GetRenderForNextStep = js.Serialize(JsonString)
    'End Function

    'Public Shared js As New JavaScriptSerializer()
    '<Services.WebMethod()>
    'Public Shared Function getModeQuizAndTimer()
    '    Dim AllTime As Integer = 0
    '    Dim TimeRemain As Integer = 0
    '    Dim NeedTimer As Boolean
    '    Dim timerType As Boolean = False
    '    Dim timeTotal As Integer = 0
    '    Dim noWatch As Boolean = False
    '    Dim IsHomeWork As Boolean = False
    '    Dim Deadline As String = ""
    '    Dim db As New ClassConnectSql()
    '    Dim QuizId As String = HttpContext.Current.Session("Quiz_Id")
    '    Dim PlayerId As String = HttpContext.Current.Session("_StudentId")
    '    Dim sql As String
    '    If HttpContext.Current.Session("HomeworkMode") = True Then
    '        'sql = "select DATEDIFF(SECOND,LastUpdate,dbo.GetThaiDate())as timeDiff from tblQuiz Where Quiz_Id = '" & shareQuizId & "'"
    '        sql = " SELECT  tblModuleAssignment.End_Date FROM tblQuiz INNER JOIN tblModuleDetailCompletion ON tblQuiz.Quiz_Id = tblModuleDetailCompletion.Quiz_Id "
    '        sql &= " INNER JOIN tblModuleAssignment ON tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id "
    '        sql &= " WHERE tblQuiz.Quiz_Id = '" & QuizId & "' AND tblModuleAssignment.IsActive = 1 "
    '        sql &= " AND tblModuleDetailCompletion.Student_Id = '" & PlayerId & "'; "
    '    Else
    '        sql = "  SELECT NeedTimer,IsPerQuestionMode,TimePerQuestion,TimePerTotal,IsTimeShowCorrectAnswer,TimePerCorrectAnswer,DATEDIFF(SECOND,StartTime,dbo.GetThaiDate()) as timeDiff,DATEDIFF(SECOND,dbo.GetThaiDate(),DATEADD(MINUTE,TimePerTotal,StartTime)) as timeRemain FROM tblQuiz WHERE Quiz_Id = '" & QuizId & "'; "
    '    End If
    '    Dim dt = db.getdata(sql)
    '    If (dt.Rows.Count > 0) Then
    '        If HttpContext.Current.Session("HomeworkMode") = True Then
    '            NeedTimer = False
    '            IsHomeWork = True
    '            Dim ClsTimeAgo As New ClsTimeAgo()
    '            Deadline = "กำหนดส่ง " & ClsTimeAgo.GetCreepOnTime(dt.Rows(0)("End_Date"))
    '        Else
    '            If (dt.Rows(0)("NeedTimer")) Then
    '                ' จับเวลาในการทำควิซ ทั้ง ข้อต่อข้อและทั้งหมด
    '                NeedTimer = True
    '                ' state ทำข้อสอบ 0 หรือ 1
    '                If (_AnswerState = "0" Or _AnswerState = "1") Then
    '                    If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                        ' จับเวลาต่อข้อ
    '                        AllTime = CInt(dt.Rows(0)("TimePerQuestion"))
    '                        timerType = True
    '                    Else
    '                        ' จับเวลาทั้งหมด
    '                        'AllTime = CInt(dt.Rows(0)("timeDiff"))
    '                        If CInt(dt.Rows(0)("timeRemain")) > 0 Then
    '                            TimeRemain = CInt(dt.Rows(0)("timeRemain"))
    '                            timeTotal = CInt(dt.Rows(0)("TimePerTotal"))
    '                        End If
    '                    End If
    '                Else
    '                    ' state เฉลย
    '                    If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
    '                        ' เฉลยแบบมีเวลา
    '                        'TimeRemain = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                        If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                            ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
    '                            '' เมื่อเฉลยหมดเวลาให้กดปุ่ม next ให้
    '                            AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                            timerType = True
    '                        Else
    '                            ' ข้อสอบแบบจับเวลาทั้งหมด
    '                            If (CInt(dt.Rows(0)("timeRemain")) > 0) Then
    '                                ' เมื่อหมดเฉลยจะกดปุ่ม next ให้ แต่ถ้าหมดเวลาสอบจะขึ้น dialog
    '                                AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                                timerType = True
    '                            End If
    '                        End If
    '                    Else
    '                        ' เฉลยแบบไม่มีเวลา
    '                        If (dt.Rows(0)("IsPerQuestionMode") = "1") Then
    '                            ' เป็นข้อสอบแบบจับเวลาข้อต่อข้อ
    '                            '' เอาเวลาจากไหน เพื่อไม่ให้มันเซ็ต dialog ขึ้น หรือมันกด next เอง
    '                            noWatch = True
    '                        Else
    '                            ' ข้อสอบแบบจับเวลาทั้งหมด 
    '                            '' ให้ใช้เวลาเดียวกับตอนที่ render โจทย์
    '                            If CInt(dt.Rows(0)("timeRemain")) > 0 Then
    '                                AllTime = CInt(dt.Rows(0)("timeRemain"))
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                ' ไม่จับเวลาในการทำควิซ 
    '                '' แต่ถ้ามีเฉลยข้อต่อข้อแล้วใส่เวลา เมื่อถึง state เฉลย เวลาก็ยังคงเป็นแบบเดินไปเรื่อยๆๆ
    '                NeedTimer = False
    '                AllTime = dt.Rows(0)("timeDiff")
    '                If (_AnswerState = "2") Then
    '                    ' state เฉลย
    '                    If (dt.Rows(0)("IsTimeShowCorrectAnswer") = "1") Then
    '                        NeedTimer = True
    '                        AllTime = CInt(dt.Rows(0)("TimePerCorrectAnswer"))
    '                        timerType = True
    '                    End If
    '                End If
    '            End If
    '        End If
    '        Dim JsonString
    '        JsonString = New With {.NeedTimer = NeedTimer, .AllTime = AllTime, .timerType = timerType, .TimeRemain = TimeRemain, .timeTotal = timeTotal, .noWatch = noWatch, .IsHomeWork = IsHomeWork, .Deadline = Deadline}
    '        getModeQuizAndTimer = js.Serialize(JsonString)
    '    Else
    '        getModeQuizAndTimer = "ERROR"
    '    End If
    'End Function
#End Region


End Class
