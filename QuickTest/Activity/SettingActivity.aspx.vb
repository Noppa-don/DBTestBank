Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Web
Imports KnowledgeUtils


Public Class SettingActivity
    Inherits System.Web.UI.Page
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Public GroupName As String
    'Dim ClsSelectSess As New ClsSelectSession
    ' 02-05-56 update variable tools
    Public toolsAllSubject As Boolean = True
    Public toolsSubject_Eng As Boolean = False
    Public toolsSubject_Math As Boolean = False
    Protected DefaultClass As String = ""
    Protected DefaultRoom As String = ""
    Dim _DB As New ClassConnectSql()
    Protected IsAndroid As Boolean

    Dim redis As New RedisStore()

    Public IE As String

    Protected NeedShowTip As Boolean
    Protected ClassRoomCanMakeQuiz As String

    'Random
    Public RandomQuestion As Boolean = True
    Public RandomAnswer As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Log.Record(Log.LogType.PageLoad, "pageload settingquiz", False)
        If (Session("UserId") Is Nothing) Then
            Log.Record(Log.LogType.PageLoad, "settingquiz session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        End If


        If Not Page.IsPostBack Then
            ' ส่วนของการแสดง qtip
            If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                NeedShowTip = ClsUserViewPageWithTip.CheckUpdateUserViewPageWithTip(pageName)
            End If


            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If
        End If

        'Open Connection 
        Dim connSetting As New SqlConnection
        _DB.OpenExclusiveConnect(connSetting)

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("newTestSetId") = "B2AFB3CA-273D-4760-B406-E1941DDF5FD6"
        Session("SchoolCode") = "1000001"
        Session("SchoolID") = "1000001"
        'Session("selectedSession") = 4444
        IE = "1"
#End If


        If Not Page.IsPostBack Then
            'Help
            ProcessHelpPanel()

            'Bind SoundlabRoomName
            BindDDLSoundlabName(HttpContext.Current.Session("SchoolID").ToString(), connSetting)

            'GroupName = Session("selectedSession").ToString()
            Session("PracticeFromComputer") = False

            Session("QuizUseTablet") = True ' default use tablet

            ' รับ querystring ต่อจากหน้า choosequestionset กับ dashboradquiz
            If Not Request.QueryString("TestsetID") Is Nothing Then
                Session("newTestSetId") = Request.QueryString("TestsetID")
            End If

            Dim TestsetId As String = Session("newTestSetId").ToString
            Dim dtQuizDetail As DataTable = ClsActivity.GetTestset(TestsetId, connSetting)
            Dim QuizClass As String = ClsActivity.GetMaxLevel(TestsetId, connSetting).ToString
            Dim dtUserSettingDetail As DataTable = ClsActivity.GetUserSettingDetail(Session("UserId").ToString, TestsetId, connSetting)

            lblTestsetName.Text = dtQuizDetail.Rows(0)("Testset_Name").ToString
            Dim TSName As String = dtQuizDetail.Rows(0)("Testset_Name").ToString ' ใช้กับ ClsCheckMark
            SetupAnswer_Name.Value() = HttpUtility.HtmlEncode(TSName)
            lblQuestionAmount.Text = dtQuizDetail.Rows(0)("QuestionAmount").ToString
            QuestionAmount.Value() = dtQuizDetail.Rows(0)("QuestionAmount").ToString ' ใช้กับ ClsCheckMark
            lblLevel.Text = QuizClass
            HiddenTest.Value = QuizClass

            If Not dtUserSettingDetail.Rows.Count = 0 Then 'ถ้าครูเคย Set Quiz แล้ว

                'หาห้อง - ชั้น สำหรับเอาไป set ให้ Dialog เลือกห้อง - ชั้น ตอนแรก
                If dtUserSettingDetail.Rows(0)("t360_ClassName") IsNot DBNull.Value And dtUserSettingDetail.Rows(0)("t360_RoomName") IsNot DBNull.Value Then
                    DefaultClass = dtUserSettingDetail.Rows(0)("t360_ClassName").ToString()
                    DefaultRoom = dtUserSettingDetail.Rows(0)("t360_RoomName").ToString().Replace("/", "")
                    If Not IsExistRoom(DefaultClass, DefaultRoom) Then
                        DefaultRoom = ""
                    End If
                End If

                'If dtUserSettingDetail.Rows(0)("NeedTimer") Is DBNull.Value Then
                '    chkCheckTime.Checked = False
                'Else
                '    chkCheckTime.Checked = dtUserSettingDetail.Rows(0)("NeedTimer")
                'End If

                chkCheckTime.Checked = If(dtUserSettingDetail.Rows(0)("NeedTimer") Is DBNull.Value, False, dtUserSettingDetail.Rows(0)("NeedTimer"))

                'If dtUserSettingDetail.Rows(0)("IsPerQuestionMode") Then
                '    IsPerQuestion.Checked = True
                '    txtTimePerQuestion.Enabled = True
                '    IsAll.Checked = False
                '    txtTimeAll.Enabled = False
                'Else
                '    IsPerQuestion.Checked = False
                '    txtTimePerQuestion.Enabled = False
                '    IsAll.Checked = True
                '    txtTimeAll.Enabled = False
                'End If

                If chkCheckTime.Checked Then
                    IsPerQuestion.Checked = dtUserSettingDetail.Rows(0)("IsPerQuestionMode")
                    txtTimePerQuestion.Enabled = dtUserSettingDetail.Rows(0)("IsPerQuestionMode")
                    IsAll.Checked = Not dtUserSettingDetail.Rows(0)("IsPerQuestionMode")
                    txtTimeAll.Enabled = Not dtUserSettingDetail.Rows(0)("IsPerQuestionMode")
                End If

                'Dim perQuestion As Boolean = dtUserSettingDetail.Rows(0)("IsPerQuestionMode")
                'IsPerQuestion.Checked = perQuestion
                'txtTimePerQuestion.Enabled = perQuestion
                'IsAll.Checked = Not perQuestion
                'txtTimeAll.Enabled = Not perQuestion

                'IsPerQuestion.Checked = True
                'txtTimePerQuestion.Enabled = True
                'IsAll.Checked = False
                'txtTimeAll.Enabled = False

                txtTimePerQuestion.Text = dtUserSettingDetail.Rows(0)("TimePerQuestion")
                hdTimePerQuestion.Value = dtUserSettingDetail.Rows(0)("TimePerQuestion")
                txtTimeAll.Text = dtUserSettingDetail.Rows(0)("TimePerTotal")
                hdTimeAll.Value = dtUserSettingDetail.Rows(0)("TimePerTotal")
                lblTimeAllPerQuestion.Text = GetPerTimeString() 'Math.Round(CInt(lblQuestionAmount.Text) * CInt(txtTimePerQuestion.Text) / 60).ToString
                lblTimeAll.Text = GetAllTimeString() ' Math.Round((CInt(txtTimeAll.Text) * 60) / CInt(lblQuestionAmount.Text), 2).ToString

                If dtUserSettingDetail.Rows(0)("Selfpace") Is DBNull.Value Then
                    chkSelfPace.Checked = False
                Else
                    chkSelfPace.Checked = dtUserSettingDetail.Rows(0)("Selfpace")
                End If

                If dtUserSettingDetail.Rows(0)("NeedCorrectAnswer") Is DBNull.Value Then
                    chkShowAnswer.Checked = False
                Else
                    chkShowAnswer.Checked = dtUserSettingDetail.Rows(0)("NeedCorrectAnswer")
                End If

                If dtUserSettingDetail.Rows(0)("IsRushMode") Is DBNull.Value Then
                    chkRushMode.Checked = False
                Else
                    chkRushMode.Checked = dtUserSettingDetail.Rows(0)("IsRushMode")
                End If

                If dtUserSettingDetail.Rows(0)("IsShowCorrectAfterComplete") Then
                    rdbAnswerAfter.Checked = True
                    rdbAnswerPerQuestion.Checked = False
                Else
                    rdbAnswerAfter.Checked = False
                    rdbAnswerPerQuestion.Checked = True
                End If

                If dtUserSettingDetail.Rows(0)("IsTimeShowCorrectAnswer") Is DBNull.Value Then
                    ChkInShow1.Checked = False
                    txtTimeShowAnswer.Enabled = False
                Else
                    ChkInShow1.Checked = dtUserSettingDetail.Rows(0)("IsTimeShowCorrectAnswer")
                    txtTimeShowAnswer.Enabled = dtUserSettingDetail.Rows(0)("IsTimeShowCorrectAnswer")
                End If

                txtTimeShowAnswer.Text = dtUserSettingDetail.Rows(0)("TimePerCorrectAnswer")

                If dtUserSettingDetail.Rows(0)("NeedShowScore") Is DBNull.Value Then
                    chkShowScore.Checked = False
                Else
                    chkShowScore.Checked = dtUserSettingDetail.Rows(0)("NeedShowScore")
                End If

                If dtUserSettingDetail.Rows(0)("NeedShowScoreAfterComplete") Then
                    rdbEndQuiz.Checked = True
                    rdbByStep.Checked = False
                Else
                    rdbEndQuiz.Checked = False
                    rdbByStep.Checked = True
                End If

             

                If dtUserSettingDetail.Rows(0)("NeedRandomAnswer") Is DBNull.Value Then
                    chkRandomAnswer.Checked = False
                Else
                    chkRandomAnswer.Checked = dtUserSettingDetail.Rows(0)("NeedRandomAnswer")
                End If

                If dtUserSettingDetail.Rows(0)("NeedRandomQuestion") Is DBNull.Value Then
                    chkRandomQuestion.Checked = False
                Else
                    chkRandomQuestion.Checked = dtUserSettingDetail.Rows(0)("NeedRandomQuestion")
                End If

                If dtUserSettingDetail.Rows(0)("IsDifferentQuestion") Is DBNull.Value Then
                    chkDiffQuestion.Checked = False
                Else
                    If dtUserSettingDetail.Rows(0)("IsDifferentQuestion") Then
                        chkDiffQuestion.Checked = True
                    Else
                        If dtUserSettingDetail.Rows(0)("IsDifferentAnswer") Is DBNull.Value Then
                            chkDiffQuestion.Checked = False
                        Else
                            chkDiffQuestion.Checked = True
                        End If
                    End If
                End If

                If dtUserSettingDetail.Rows(0)("IsUseTablet") Is DBNull.Value Then
                    chkQuizUseTablet.Checked = False
                Else
                    chkQuizUseTablet.Checked = dtUserSettingDetail.Rows(0)("IsUseTablet")
                End If

                setCheckboxUseTools(dtUserSettingDetail.Rows(0)("EnabledTools"))
            Else

                'ไม่เคย Set Quiz มาก่อนเลย ไม่เคยเปิด quiz เลย

                chkCheckTime.Checked = False

                IsPerQuestion.Checked = True
                txtTimePerQuestion.Enabled = True

                IsAll.Checked = False
                txtTimeAll.Enabled = False


                txtTimePerQuestion.Text = "10"
                hdTimePerQuestion.Value = 10
                txtTimeAll.Text = "30"
                hdTimeAll.Value = 30
                lblTimeAllPerQuestion.Text = GetPerTimeString() 'Math.Round(CInt(lblQuestionAmount.Text) * CInt(txtTimePerQuestion.Text) / 60).ToString
                lblTimeAll.Text = GetAllTimeString() 'Math.Round((CInt(txtTimeAll.Text) * 60) / CInt(lblQuestionAmount.Text), 2).ToString


                'ไปด้วยตัวเอง
                chkSelfPace.Checked = False
                'เฉลย
                chkShowAnswer.Checked = True
                'โหมดแข่งกันตอบ
                chkRushMode.Checked = False
                'เฉลยหลังจบควิซ
                rdbAnswerAfter.Checked = True
                rdbAnswerPerQuestion.Checked = False
                'มีเวลาเฉลย?
                ChkInShow1.Checked = False
                txtTimeShowAnswer.Text = "30"
                'โชว์คะแนน
                chkShowScore.Checked = False
                'โชว์ตอนจบ รึ ข้่อต่อข้อ
                rdbEndQuiz.Checked = True
                rdbByStep.Checked = False
                ' random โจทย์ คำตอบ
                chkRandomAnswer.Checked = False
                chkRandomQuestion.Checked = False
                ' random ทุกคนไม่เหมือนกัน
                chkDiffQuestion.Checked = False
                ' ใช้ tablet
                chkQuizUseTablet.Checked = True

            End If

            '// Random Check Disabled Checkbox ถ้าข้อสอบนี้ไม่อนุญาตด้วย
            Dim dtQsetAllowRandomQuestionAndAnswer As DataTable
            dtQsetAllowRandomQuestionAndAnswer = ClsActivity.CheckQsetRandomSettingByTestset(TestsetId)

            '//ถ้าจำนวนข้อที่สุ่มได้ = 0 ให้ Disable Checkbox
            If dtQsetAllowRandomQuestionAndAnswer.Rows(0)("RandomQuestion") = 0 Then
                chkRandomQuestion.Enabled = False
                chkRandomQuestion.Checked = False
                RandomQuestion = False
                '// ถ้าามีข้อที่สุ่มไม่ได้
            ElseIf dtQsetAllowRandomQuestionAndAnswer.Rows(0)("RandomQuestion") <> dtQsetAllowRandomQuestionAndAnswer.Rows(0)("QuestionAmount") Then
                RandomQuestion = False
            End If

            If dtQsetAllowRandomQuestionAndAnswer.Rows(0)("RandomAnswer") = 0 Then
                chkRandomAnswer.Enabled = False
                chkRandomAnswer.Checked = False
                RandomAnswer = False
            ElseIf (dtQsetAllowRandomQuestionAndAnswer.Rows(0)("RandomAnswer") <> dtQsetAllowRandomQuestionAndAnswer.Rows(0)("QuestionAmount")) Or (dtQsetAllowRandomQuestionAndAnswer.Rows(0)("Shuffle") <> 0) Then
                RandomAnswer = False
            End If

            'set ค่าให้ checkmark
            If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                setElementCheckmarkOnPageLoad(TestsetId, dtQuizDetail.Rows(0)("QuestionAmount").ToString, connSetting)
            End If

            'ถ้าเป็นโหมดห้อง Lab ให้ไปเช็ค Redis ก่อนว่าเครื่องที่เปิดนี้เป็นห้อง Lab ห้องไหน จะได้ Lock ไม่ให้เลือกห้องอื่นได้

            If ClsKNSession.RunMode = "labonly" Then
                ChkSoundLabRoom()
            End If


            'หาชั้นที่สามารถทำควิซได้
            ClassRoomCanMakeQuiz = getClassRoomCanMakeQuiz(connSetting)

        End If

        PClass.InnerHtml = GetHtmlClass("ป")
        MClass.InnerHtml = GetHtmlClass("ม")
        TRoom.InnerHtml = GetHtmlRoom(lblLevel.Text)

        ' 02-05-56 update หาวิชาที่อยู่ใน testset เพื่อแสดง tools ให้เลือกใช้
        getSubjectInTestsetAndSetTools(Session("newTestSetId").ToString(), connSetting)




        'CloseConnect() For
        _DB.CloseExclusiveConnect(connSetting)

    End Sub

    Private Function GetPerTimeString() As String
        Dim questionAmount As Integer = CInt(lblQuestionAmount.Text)
        Dim secTotal As Integer = questionAmount * CInt(txtTimePerQuestion.Text) ' หาเวลาทั้งหมด เป็นวินาที จำนวนข้อ * กับ default 10 วินาที  
        Return String.Format("(ทั้งหมด {0})", GetTimeString(secTotal))
    End Function

    Private Function GetAllTimeString() As String
        Dim questionAmount As Integer = CInt(lblQuestionAmount.Text)
        Dim secTotal As Integer = 60 * CInt(txtTimeAll.Text) ' หาวินาที default เป็นเวลา 30 นาที
        Dim secPerQuestion As Integer = Math.Floor(secTotal / questionAmount)
        Return String.Format("(ข้อละ {0})", GetTimeString(secPerQuestion))
    End Function

    Private Function GetTimeString(sec) As String
        If sec >= 60 Then
            Dim m As Integer = Math.Floor(sec / 60)
            Dim s As Integer = sec Mod 60
            Return If(s = 0, String.Format("{0} นาที", m), String.Format("{0} นาที {1} วินาที", m, s))
        End If
        Return String.Format("{0} วินาที", sec)
    End Function

    Private Function IsExistRoom(className As String, roomName As String)
        Dim sql As String = String.Format("SELECT * FROM t360_tblRoom WHERE Class_Name = '{0}' AND Room_Name = '{1}' AND School_Code = '{2}' AND Room_IsActive = 1;",
                                          className, roomName.Insert(0, "/"), Session("SchoolId").ToString())
        Dim dt As DataTable = New ClassConnectSql().getdata(sql)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    ' optimize code การทำงานเหมือนกัน
    Private Function GetHtmlClass(ByVal ClassName As String) As String
        Dim dtClass As DataTable = ClsActivity.GetClassName(Session("UserId").ToString, ClassName)
        Dim styleBtn As String = "min-width:60px;height:60px;padding: 0px 5px 0px 5px;"
        Dim styleBtnClass As String = "ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
        Dim name As String
        Dim StrClass As String = "selectClass"
        If ClassName = "ป" Then
            name = "ประถมศึกษา"
        Else
            name = "มัธยมศึกษา"
        End If
        Dim i As Integer = 1
        Dim tagClass As String = ""

        If Not dtClass.Rows.Count = 0 Then
            tagClass = "<tr><td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;padding-right:20px;'>" & name & "</td>"
            For Each row In dtClass.Rows
                'tagClass &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'><input id='btnChangeLv' type=""button"" class='" & StrClass & "' value='" & i & "' onclick=""ChangeLv('" & row("Level_ShortName") & "')"" style='" & styleBtn & "' class='" & styleBtnClass & "'/></td>"
                tagClass &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'><input id='btnChangeLv' type=""button"" class='" & StrClass & "' value='" & row("NumLevel") & "' lv='" & row("Level_ShortName") & "' style='" & styleBtn & "' class='" & styleBtnClass & "'/></td>"
                i = i + 1
            Next
            tagClass &= "</tr>"

        End If
        GetHtmlClass = tagClass
    End Function
    <Services.WebMethod()>
    Public Shared Function GetHtmlRoom(ByVal ClassName As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim dtClass As DataTable = ClsActivity.GetRoomName(ClassName)
        Dim styleBtn As String = "width:60px;height:60px;"
        Dim styleBtnClass As String = "ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
        Dim i As Integer = 1
        Dim tagClass As String = ""
        Dim StrClass As String = "SelectRoom"
        For Each row In dtClass.Rows
            tagClass &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'>"
            'tagClass &= "<input id='btnChangeLv' type=""button"" value='" & row("cutRoom") & "' onclick=""ChangeRoom('" & row("cutRoom") & "')"" "
            tagClass &= "<input id='btnChangeLv' type=""button"" value='" & row("cutRoom") & "' class='SelectRoom' room='" & row("cutRoom") & "' "
            tagClass &= " style='" & styleBtn & "' class='" & styleBtnClass & "'/></td>"
            i = i + 1
        Next
        If ClsKNSession.RunMode = "standalonenotablet" Then
            'ใส่ปุ่มเพิ่มห้องสำหรับโหมด standalone
            tagClass &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'>"
            tagClass &= "<input id='btnAddRoom' type=""button"" value='+' class='AddRoom' room='1' "
            tagClass &= " style='" & styleBtn & "' class='AddRoom'/></td>"
        End If
        'tagClass &= "</tr>"
        GetHtmlRoom = tagClass


    End Function

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        'Open Connection 
        Dim connSetting As New SqlConnection
        _DB.OpenExclusiveConnect(connSetting)

        ' Clear Application Not Used
        If HttpContext.Current.Application("ClearApplication_TimeStamp") Is Nothing OrElse DateDiff(DateInterval.Hour, HttpContext.Current.Application("ClearApplication_TimeStamp"), DateTime.Now()) > 6 Then
            ClearApplicationInQuiz(connSetting)
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application("ClearApplication_TimeStamp") = DateTime.Now()
            HttpContext.Current.Application.UnLock()
        End If

        Session("ChooseMode") = EnumDashBoardType.Quiz

        Dim ArrCheckBoolean() As String = {chkCheckTime.Checked, IsPerQuestion.Checked, chkShowAnswer.Checked, rdbAnswerAfter.Checked, _
                                          chkRushMode.Checked, chkRandomQuestion.Checked, chkRandomAnswer.Checked, ChkInShow1.Checked, _
                                          chkShowScore.Checked, rdbEndQuiz.Checked, chkDiffQuestion.Checked, chkSelfPace.Checked}

        For j = 0 To ArrCheckBoolean.Length - 1
            If ArrCheckBoolean(j) = True Then
                ArrCheckBoolean(j) = "1"
            Else
                ArrCheckBoolean(j) = "0"
            End If
        Next

        ' แก้ Bug ปุ่ม back ไม่ขึ้น 
        If ArrCheckBoolean(0) = "0" Then
            ArrCheckBoolean(1) = "0"
        End If

        Dim totalStudent As String
        If Session("QuizUseTablet") = False Then
            totalStudent = "0"
        Else
            totalStudent = Session("TotalStudent").ToString()
        End If

        'คำถามแต่ละคนเหมือนกันหรือเปล่า
        Dim differentQuestion As String = GetDifferentExam(PartExam.Question)
        'คำตอบแต่ละคนเหมือนกันหรือเปล่า
        Dim differentAnswer As String = GetDifferentExam(PartExam.Answer)

        'Dim AcademicYear As String = ClsActivity.GetAcademicYear()
        Dim LvName As String = HiddenTest.Value
        Dim RoomName As String = HiddenRoom.Value.Insert(0, "/")

        ' 02-05-56 update use tools
        Dim EnabledTools As Integer = 0
        If chkUseTools.Checked Then
            EnabledTools = setToolsInQuiz()
        End If

        Dim IsUseSoundLab As Boolean = chkQuizFromSoundlab.Checked
        Dim TabletLabid As String = ""
        If IsUseSoundLab = True Then
            HttpContext.Current.Session("IsSoundLab") = True
            TabletLabid = DDLSoundLabName.SelectedValue
        Else
            HttpContext.Current.Session("IsSoundLab") = False
        End If

        ' ทำการ update endtime ที่ยังเป็น null โดยเป็นของห้องที่กำลังจะจัดควิซ ครูจงใจที่จะเปิดควิซซ้ำในห้องนั้น
        If HttpContext.Current.Session("QuizDuplicate") IsNot Nothing Then
            Dim q = HttpContext.Current.Session("QuizDuplicate")
            ClsActivity.setEndTimeNotNullBeforeStartQuiz(LvName, RoomName, Session("SchoolId").ToString(), q.Quiz_Id, connSetting)
            ' clear quiz in redis ทิ้ง
            redis.DEL(q.Quiz_Id)
        End If
        ' set session quizusetablet
        If chkQuizUseTablet.Checked Then
            HttpContext.Current.Session("QuizUseTablet") = True
        Else
            If HttpContext.Current.Application("StudentLoginFromPC") = True Then
                HttpContext.Current.Session("QuizUseTablet") = True
            Else
                HttpContext.Current.Session("QuizUseTablet") = False
            End If
        End If

        ' 02-05-56 update เพิ่มฟิลด์ EnabledTools เก็บค่า tools ที่ใช้ในควิซนั้น

        If ArrCheckBoolean(2) = "0" Then
            ArrCheckBoolean(3) = "0"
        End If

        If ArrCheckBoolean(8) = "0" Then
            ArrCheckBoolean(9) = "0"
        End If


        Dim timePerQuestion As Integer = hdTimePerQuestion.Value
        Dim timeAll As Integer = hdTimeAll.Value


        'txtTimePerQuestion.Text = If(txtTimePerQuestion.Text = "", TimePerQuestion, txtTimePerQuestion)
        'txtTimeAll.Text = If(txtTimeAll.Text = "", TimeAll, txtTimeAll)

        Dim KnSession As New KNAppSession()
        Dim CurrentCalendarId As String = KnSession("CurrentCalendarId").ToString()
        Dim QuizId As String = ClsActivity.SaveQuizDetail(Session("newTestSetId").ToString(), LvName, RoomName, totalStudent, ArrCheckBoolean(0), _
                                   ArrCheckBoolean(1), timePerQuestion, timeAll, ArrCheckBoolean(2), txtTimeShowAnswer.Text, _
                                  ArrCheckBoolean(3), ArrCheckBoolean(4), ArrCheckBoolean(5), ArrCheckBoolean(6), Session("UserId").ToString(), _
                                   Session("SchoolId").ToString(), Session("TeacherId").ToString(), ArrCheckBoolean(7), chkQuizUseTablet.Checked, ArrCheckBoolean(8), _
                                  ArrCheckBoolean(9), differentQuestion, ArrCheckBoolean(11), differentAnswer, EnabledTools, CurrentCalendarId, TabletLabid, connSetting)
        Session("Quiz_Id") = QuizId
        'Session("TotalStudent") = txtTotalStudent.Text        

        If Session("QuizUseTablet") = True Then

            Dim dtPlayerId As New DataTable 'ไว้ใช้ตอน add redis

            If IsUseSoundLab = True Then
                dtPlayerId = ClsActivity.SetStudentUseSoundLab(QuizId, HttpContext.Current.Session("SchoolId").ToString(), connSetting)
                ClsActivity.SetTeacherSoundLab(HttpContext.Current.Session("UserId").ToString(), QuizId, HttpContext.Current.Session("SchoolId").ToString(), connSetting)
            Else
                dtPlayerId = ClsActivity.SetStudent(QuizId, Session("SchoolId").ToString(), LvName, RoomName, connSetting)
                ClsActivity.setTeacher(Session("UserId").ToString(), QuizId, Session("SchoolId").ToString(), connSetting) ' เพิ่ม tablet ครู เข้าไปใน quizsession ด้วย
            End If

            ' Create Application(Quiz) For GetNextAction
            'HttpContext.Current.Application.Lock()
            Dim qu As New Quiz
            qu.QuizId = QuizId
            qu.TestsetId = Session("newTestSetId").ToString()
            If chkSelfPace.Checked = True Then qu.IsSelfPace = True Else qu.IsSelfPace = False
            qu.AmountQuestion = lblQuestionAmount.Text.Trim()
            qu.Examnum = "1"
            qu.IsLab = IsUseSoundLab
            qu.NoOfDone = New Dictionary(Of String, Integer)
            qu.CheckIn = False
            If chkShowAnswer.Checked = True AndAlso rdbAnswerAfter.Checked = False Then qu.AnswerState = "1" Else qu.AnswerState = "0"
            HttpContext.Current.Application("Quiz_" & QuizId) = qu
            ' HttpContext.Current.Application.UnLock()

            ' Redis Set Key DeviceId
            HttpContext.Current.Application(QuizId & "_AllPlayer") = dtPlayerId
            'redis.SetKey(Of DataTable)("PlayerInQuiz_" & QuizId, dtPlayerId)

            'Dim lq As New List(Of Quiz)
            'For Each r As DataRow In dtPlayerId.Rows
            '    qu.PlayerId = r("Owner_id").ToString()
            '    lq.Add(qu)
            'Next

        Else
            'ClsActivity.setTeacher(Session("UserId").ToString(), QuizId, Session("SchoolId").ToString(), connSetting)
            Try
                Dim Sql As String
                Sql = " insert into tblQuizSession (School_Code,Player_Id,Player_Type,Quiz_Id,Tablet_Id) values ('" & Session("SchoolId").ToString() & "',"
                Sql &= "'" & Session("UserId").ToString() & "','1','" & QuizId & "',NULL);"
                _DB.Execute(Sql)
                Log.Record(Log.LogType.Quiz, "ครูเปิด Quiz """ & lblTestsetName.Text & """", True)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End If

        ' save question ลง tblQuizQuestion
        Dim swapQuestion As Boolean = chkRandomQuestion.Checked
        Dim swapAnswer As Boolean = chkRandomAnswer.Checked
        getQsetInQuiz(swapQuestion, swapAnswer, connSetting)

        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            ' save ข้อมูล ลงไปใน table checkAnswer2
            If chkUseTemplate.Checked = True Then
                Dim ClsCheckMark As New ClsCheckMark
                Dim TemplateName = setAmountChoice(QuestionAmount.Value().ToString()) 'get tamplate from questionAmount
                Dim qAmount = 0 ' บันทึกจำนวนข้อตามจำนวนที่กด next เลยให้เป็น 0 ก่อน
                If SetupAnswer_Name.Value().ToString().Length > 20 Then
                    SetupAnswer_Name.Value() = SetupAnswer_Name.Value().ToString().Substring(0, 19)
                End If
                Dim detail As String = String.Format("({0} - {1})", LvName & RoomName, Date.Now.ToString("dd/MM/yy HH:mm"))   'setAnserNameCheckmark(LvName)
                Dim setupName As String = SetupAnswer_Name.Value().ToString() & detail
                ClsCheckMark.saveQuizToCheckmark(setupName, TemplateName, qAmount, LvName, Session("newTestSetId").ToString) 'save data to db checkAnswer2
                ClsCheckMark.InsertRefToCheckMarkIntblCM(Session("newTestSetId").ToString, connSetting)
                ClsCheckMark.updateConnectCheckmark("1", connSetting)
                Session("QuizUseTamplate") = True
            Else
                Dim ClsCheckMark As New ClsCheckMark
                ClsCheckMark.updateConnectCheckmark("0", connSetting)
                Session("QuizUseTamplate") = False
            End If
        End If

        ' set selected session
        Dim clsSelectSess As New ClsSelectSession()
        clsSelectSess.SetSessionChooseMode(Session("ChooseMode"))
        clsSelectSess.SetSessionQuizId(QuizId)
        clsSelectSess.SetClassName(LvName & RoomName)
        clsSelectSess.SetSessionQuizUseTablet(Session("QuizUseTablet"))
        clsSelectSess.SetCurrentPage("activity/activitypage.aspx") 'สำหรับ demo ไปก่อน set current
        clsSelectSess.SetCurrentQuerystring("")

        If chkShowAnswer.Checked Then
            If chkRushMode.Checked Then
                Response.Redirect("RaceActivity.aspx")
            Else
                Response.Redirect("~/activity/ActivityPage.aspx")
            End If
        Else
            Response.Redirect("~/activity/ActivityPage.aspx")
        End If

        'Close Connection
        _DB.CloseExclusiveConnect(connSetting)

    End Sub


    Private Sub ClearApplicationInQuiz(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim dt As DataTable = _DB.getdata(" SELECT * FROM tblQuiz WHERE StartTime BETWEEN Dateadd(day,-3,dbo.GetThaiDate()) AND DATEADD(hour,-6,dbo.GetThaiDate()) AND EndTime IS NULL; ", , InputConn)
        For Each r In dt.AsEnumerable
            Dim QuizId As String = DirectCast(r.Field(Of Guid)("Quiz_Id").ToString(), String)
            Application.Lock()
            For Each AppName In HttpContext.Current.Application.AllKeys
                If AppName.IndexOf(QuizId) > -1 Then
                    HttpContext.Current.Application.Remove(AppName)
                End If
            Next
            Application.UnLock()
        Next
    End Sub


    ' หาว่าคำถาามและคำตอบต่างกันหรือปล่าว
    Private Function GetDifferentExam(ByVal DiffMode As Integer) As String
        Dim Different As String
        Dim IsDiff As Boolean
        If DiffMode = PartExam.Question Then
            IsDiff = chkRandomQuestion.Checked
        ElseIf DiffMode = PartExam.Answer Then
            IsDiff = chkRandomAnswer.Checked
        End If

        If (IsDiff AndAlso chkDiffQuestion.Checked) Then
            Different = "1"
        Else
            Different = "0"
        End If
        GetDifferentExam = Different
    End Function
    Private Enum PartExam As Integer
        Question = 1
        Answer = 2
    End Enum

    ' <<< หาจำนวนนักเรียนในห้อง >>>
    <Services.WebMethod()>
    Public Shared Function GetStudentAmountCodeBehide(ByVal ClassName As String, ByVal RoomName As String) As Object
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        RoomName = "/" & RoomName
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim SchoolCode As String = HttpContext.Current.Session("SchoolID").ToString
        Dim studentAmount = ClsActivity.GetStudentAmount(ClassName, RoomName, SchoolCode)
        HttpContext.Current.Session("TotalStudent") = studentAmount
        'GetStudentAmountCodeBehide = "นักเรียนทั้งหมด " & studentAmount & " คน"
        Dim dt As New DataTable
        dt = CheckQuizAtThisTime(ClassName, RoomName)
        Dim IsDuplicate As String = "False"
        Dim TextHtml As New StringBuilder("")
        HttpContext.Current.Session("QuizDuplicate") = Nothing
        Dim q = (From r In dt.AsEnumerable()
                 Where DirectCast(r.Field(Of DateTime?)("EndTime").ToString(), String) = ""
                 Select New With {
                    .User_Id = DirectCast(r.Field(Of Guid)("User_Id").ToString(), String),
                    .Name = r.Field(Of String)("Name"),
                    .TestSet_Name = r.Field(Of String)("TestSet_Name"),
                    .Quiz_Id = DirectCast(r.Field(Of Guid)("Quiz_Id").ToString(), String),
                    .ClassRoom = r.Field(Of String)("ClassRoom"),
                    .StartTime = r.Field(Of String)("StartTime")
                }).SingleOrDefault()
        If Not q Is Nothing Then
            Dim g As ArrayList = GetQuizIdFromApplication()
            If g.Count() > 0 Then
                Dim q2 = (From r In g
                          Where r = q.Quiz_Id
                          Select r).SingleOrDefault()
                If Not q2 Is Nothing Then
                    IsDuplicate = "True"
                    HttpContext.Current.Session("QuizDuplicate") = q
                    TextHtml.Append("<p>")
                    If Not String.Compare(q.User_Id, HttpContext.Current.Session("UserId").ToString()) = 0 Then
                        TextHtml.Append("โดย ครู")
                        TextHtml.Append(q.Name)
                        TextHtml.Append("</br>")
                    End If
                    TextHtml.Append("ทำชุด ")
                    TextHtml.Append(q.TestSet_Name)
                    TextHtml.Append("</br>")
                    TextHtml.Append("ห้อง ")
                    TextHtml.Append(q.ClassRoom)
                    TextHtml.Append("       ")
                    TextHtml.Append("เมื่อ ")
                    TextHtml.Append(q.StartTime)
                    TextHtml.Append("</br>")
                    TextHtml.Append("ต้องการเปิดควิซใหม่แน่นะคะ")
                    TextHtml.Append("</p>")
                End If
            End If
        End If

        'Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim da As DataTable = ClsActivity.GetDDLLab(SchoolCode, studentAmount)
        Dim detail As New List(Of String)()
        If da.Rows.Count = 0 Then
            detail.Add("0")
        Else
            detail.Add("1")
            For Each a In da.Rows
                detail.Add(a("TabletLab_Id").ToString)
                detail.Add(a("TabletLab_Name").ToString)
            Next
        End If

        Dim js As New JavaScriptSerializer()
        Dim txtNoOfStudent As String = ""
        If studentAmount > 0 Then
            txtNoOfStudent = "นักเรียนทั้งหมด " & studentAmount & " คน"
        End If

        Dim JsonString = New With {.IsDuplicate = IsDuplicate, .NoOfStudent = txtNoOfStudent, .TextHtml = TextHtml.ToString(), .Detail = detail.ToArray}
        Return js.Serialize(JsonString)

    End Function

    'เช็คว่าชั้นห้องที่เลือกมีควิซเปิดอยู่หรือเปล่า
    Private Shared Function CheckQuizAtThisTime(ByVal ClassName As String, ByVal RoomName As String) As DataTable
        Dim KnSession As New KNAppSession()
        Dim CurrentCalendarId As String = KnSession("CurrentCalendarId").ToString()
        Dim sql As New StringBuilder()
        sql.Append(" SELECT TOP 1 (u.FirstName + '  ' + u.LastName) [Name],ts.TestSet_Name,(q.t360_ClassName + q.t360_RoomName) [ClassRoom],CONVERT(char(5),q.StartTime,108) [StartTime], q.User_Id,q.Quiz_Id,q.EndTime ")
        sql.Append(" FROM tblQuiz q INNER JOIN tblTestSet ts ON q.TestSet_Id = ts.TestSet_Id INNER JOIN tblUser u ON q.User_Id = u.GUID ")
        sql.Append(" WHERE q.t360_ClassName = '")
        sql.Append(ClassName)
        sql.Append("' AND q.t360_RoomName = '")
        sql.Append(RoomName)
        'sql.Append("' AND q.EndTime Is Null AND dbo.GetThaiDate() BETWEEN q.StartTime AND DATEADD(HOUR,3,q.StartTime) ")
        sql.Append("' AND q.Calendar_Id = '")
        sql.Append(CurrentCalendarId)
        sql.Append("' AND q.t360_SchoolCode = '")
        sql.Append(HttpContext.Current.Session("SchoolID").ToString())
        sql.Append("' ORDER BY q.LastUpdate DESC;")
        Return New ClassConnectSql().getdata(sql.ToString())
    End Function

    'หา quiz_id ทั้งหมด จาก aplication
    Private Shared Function GetQuizIdFromApplication() As ArrayList
        HttpContext.Current.Application.Lock()
        Dim g As Guid
        Dim l As New ArrayList
        For Each allApp In HttpContext.Current.Application.AllKeys
            If Not allApp Is Nothing AndAlso Guid.TryParse(allApp.ToString(), g) Then
                Dim arrApplication As New ArrayList
                arrApplication = HttpContext.Current.Application(allApp.ToString())
                For i = 0 To arrApplication.Count - 1
                    If TypeOf arrApplication.Item(i) Is ClsSessionInFo Then
                        Dim objArrApplication As ClsSessionInFo = arrApplication.Item(i)
                        If Not objArrApplication.QuizId Is Nothing Then
                            l.Add(objArrApplication.QuizId.ToString())
                        End If
                    End If
                Next
            End If
        Next
        HttpContext.Current.Application.UnLock()
        Return l
    End Function

    ' <<< session ของ useTablet >>>
    <Services.WebMethod()>
    Public Shared Function checkQuizUseTablet(ByVal checked As String)
        Dim chk As Boolean
        If (checked = "checked") Then
            HttpContext.Current.Session("QuizUseTablet") = True
            chk = True
        Else
            HttpContext.Current.Session("QuizUseTablet") = False
            chk = False
        End If

        Return chk
    End Function

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnBack.Click
        If Not Session("ChooseMode") Is Nothing Then
            ' มาจากหน้า ChooseQuestionSet
            Response.Redirect("../PracticeMode_Pad/ChooseQuestionSet.aspx")
        Else
            ' มาจากหน้า dashboard
            Response.Redirect("../Quiz/DashboardQuizPage.aspx")
        End If
    End Sub

    ' <<< set ชื่อของกระดาษคำตอบ checkmark >>>
    Private Function setAmountChoice(ByVal questionAmount As String) As String
        Dim template As String = ""
        If (CInt(questionAmount) <= 60) Then
            template = "Wpp02 QR 120 Choice"
        ElseIf (CInt(questionAmount) > 60) Then
            template = "Wpp02 QR 120 Choice"
        End If
        Return template
    End Function

    ' <<< session ของ checkmark >>>
    <Services.WebMethod()>
    Public Shared Function checkQuizUseTamplate(ByVal checked As String)
        If (checked = "checked") Then
            HttpContext.Current.Session("QuizUseTamplate") = True
        Else
            HttpContext.Current.Session("QuizUseTamplate") = False
        End If
        Return "success"
    End Function

    ' <<< set รายละเอียดต่างๆๆ ของ checkmark >>>
    Private Function setAnserNameCheckmark(ByVal LvName As String) As String
        Dim className As String = LvName & "/" & lblRoom.Text.Trim()
        Dim currentDate As String = Date.Now.ToString("dd/MM/yy HH:mm")
        setAnserNameCheckmark = "(" & className & " - " & currentDate & ")"
        Return setAnserNameCheckmark
    End Function

    ' <<< set checkbox ของ checkmark ว่าชุดก่อนถูกใช้หรือเปล่า >>>
    Private Sub setElementCheckmarkOnPageLoad(ByVal testsetId As String, ByVal qAmount As String, Optional ByVal InputConn As SqlConnection = Nothing)
        'ถ้าข้อสอบเกิน 120 ให้ checkbox-checkmark disabled
        If (CInt(qAmount) > 120) Then
            chkUseTemplate.Enabled = False
            'divUseTemplate.Attributes("class") = "Over"
        Else
            Dim db As New ClassConnectSql()
            Dim sql As String = " SELECT tblQuestionSet.QSet_Type FROM tblTestSetQuestionSet INNER JOIN "
            sql &= " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id"
            sql &= " WHERE tblTestSetQuestionSet.TestSet_Id = '" & testsetId & "' AND tblQuestionSet.QSet_Type <> '1' "
            sql &= " AND tblQuestionSet.QSet_Type <> '2' And tblTestSetQuestionSet.IsActive = '1';"
            Dim dt As DataTable = db.getdata(sql, , InputConn)
            ' เช็คว่าชุดนั้นมีข้อสอบที่ไม่สามารถใช้ checkmark ได้หรือไม่
            If (dt.Rows.Count > 0) Then
                chkUseTemplate.Enabled = False
                'divUseTemplate.Attributes("class") = "TypeError"
            Else
                Dim needCheckmark As String = db.ExecuteScalar(" SELECT NeedConnectCheckmark FROM tblTestSet WHERE TestSet_Id = '" & testsetId & "'; ")
                ' ถ้าชุดข้อสอบนี้ เคยใช้ checkmark แล้วในครั้งก่อน
                If (needCheckmark = "True") Then
                    chkQuizUseTablet.Checked = False
                    chkUseTemplate.Checked = True
                    chkSelfPace.Enabled = False
                    chkShowScore.Enabled = False
                    chkUseTools.Enabled = False
                    Session("QuizUseTablet") = False
                Else
                    Session("QuizUseTablet") = True
                End If
            End If
        End If
    End Sub

    ' <<< หา qset ที่อยู่ใน quiz >>>
    Private Sub getQsetInQuiz(ByVal isDifferentQuestion As Boolean, ByVal isDiffAnswer As Boolean, Optional ByVal InputConn As SqlConnection = Nothing)

        'สุ่มคำถาม (สุ่ม Qset) ทำให้ข้อสอบของแต่ละ Qset ไม่สามารถปนกันได้
        Dim db As New ClassConnectSql()

        Dim sqlGetQuestionInTestset As String = " SELECT tqs.QSet_Id,qs.QSet_Type,qs.QSet_IsRandomQuestion,qs.QSet_IsRandomAnswer "
        sqlGetQuestionInTestset &= " FROM tblTestSetQuestionSet tqs LEFT JOIN tblQuestionSet qs "
        sqlGetQuestionInTestset &= " ON tqs.QSet_Id = qs.QSet_Id "
        sqlGetQuestionInTestset &= " WHERE tqs.TestSet_Id = '" & Session("newTestSetId").ToString & "' And tqs.IsActive = '1'"

        Dim dtQset As New DataTable()

        If (isDifferentQuestion) Then
            sqlGetQuestionInTestset &= " ORDER BY NEWID(); "
            dtQset = db.getdata(sqlGetQuestionInTestset, , InputConn)
            insertQuestionToQuizQuestion(dtQset, True, isDiffAnswer)
        Else
            dtQset = db.getdata(sqlGetQuestionInTestset, , InputConn)
            insertQuestionToQuizQuestion(dtQset, False, isDiffAnswer)
        End If

    End Sub

    ' <<< insert ข้อสอบ จาก qset >>>
    Private Sub insertQuestionToQuizQuestion(ByVal dtQset As DataTable, ByVal isDiffQuestion As Boolean, ByVal isDiffAnswer As Boolean, Optional ByVal InputConn As SqlConnection = Nothing)

        Dim db As New ClassConnectSql()
        Dim qq_no As Integer = 1

        For i As Integer = 0 To dtQset.Rows.Count - 1
            Dim sqlQuestionInQset As String = " SELECT tsqd.Question_Id FROM tblTestSetQuestionSet tsqs LEFT JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id "
            sqlQuestionInQset &= " WHERE tsqs.TestSet_Id = '" & Session("newTestSetId").ToString & "' AND tsqs.QSet_Id = '" & dtQset(i)("QSet_Id").ToString() & "' " 'sql get question in qset
            sqlQuestionInQset &= " And tsqs.IsActive = '1' and tsqd.IsActive = '1'"


            'ถ้าเป็น Type 6 ต้องดูด้วยว่าสุ่มคำตอบมั้ย ถ้าสุ่มคำตอบต้องสุ่มคำถาม เพราะเราเอาคำถามไปเป็นคำตอบ ถ้าไม่เช็ค เวลาเลือกสุ่มคำตอบอย่างเดียว จะไม่สุ่มให้
            'If dtQset.Rows(i)("QSet_Type") = "6" Then
            '    If (isDiffAnswer) AndAlso dtQset.Rows(i)("QSet_IsRandomQuestion") Then
            '        sqlQuestionInQset &= " ORDER BY NEWID(); "
            '    Else
            '        sqlQuestionInQset &= " ORDER BY tsqd.TSQD_No; "
            '    End If
            'Else
            '    If (isDiffQuestion) AndAlso dtQset.Rows(i)("QSet_IsRandomQuestion") Then 'question ใน qset ต้องสุ่มหรือเปล่า
            '        sqlQuestionInQset &= " ORDER BY NEWID(); "
            '    Else
            '        sqlQuestionInQset &= " ORDER BY tsqd.TSQD_No; "
            '    End If
            'End If

            ' Check แค่ 
            If (isDiffQuestion) AndAlso dtQset.Rows(i)("QSet_IsRandomQuestion") Then 'question ใน qset ต้องสุ่มหรือเปล่า
                sqlQuestionInQset &= " ORDER BY NEWID(); "
            Else
                sqlQuestionInQset &= " ORDER BY tsqd.TSQD_No; "
            End If

            Dim dtQuestionInQset As DataTable = db.getdata(sqlQuestionInQset, , InputConn)

            db.OpenWithTransection(InputConn)
            Dim sqlInsertQuestion As String
            If dtQset.Rows(i)("QSet_Type") = "6" Or dtQset.Rows(i)("QSet_Type") = "3" Then
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Session("Quiz_Id").ToString & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion, InputConn)
                Next
                qq_no = qq_no + 1
            Else
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Session("Quiz_Id").ToString & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion, InputConn)
                    qq_no = qq_no + 1
                Next
            End If
            db.CommitTransection(InputConn)
        Next

    End Sub

    ' 02-05-56 update หาวิชาที่อยู่ใน testset เพื่อเปิดการใช้งาน tools
    Private Sub getSubjectInTestsetAndSetTools(ByVal Testset_Id As String, Optional ByVal InputConn As SqlConnection = Nothing)

        Dim db As New ClassConnectSql()
        Dim sqlSubject As String = " SELECT tgs.GroupSubject_Name FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionSet tqs "
        sqlSubject &= " ON tsqs.QSet_Id = tqs.QSet_Id INNER JOIN tblQuestionCategory tqc "
        sqlSubject &= "ON tqs.QCategory_Id = tqc.QCategory_Id INNER JOIN tblBook tb "
        sqlSubject &= "ON tqc.Book_Id = tb.BookGroup_Id INNER JOIN tblGroupSubject tgs "
        sqlSubject &= "ON tb.GroupSubject_Id = tgs.GroupSubject_Id "
        sqlSubject &= "WHERE tsqs.TestSet_Id = '" & Testset_Id & "' And tsqs.isactive = '1';"

        Dim dtSubject As DataTable = db.getdata(sqlSubject, , InputConn)

        For Each subject As DataRow In dtSubject.Rows()
            Select Case subject.Item("GroupSubject_Name").ToString()
                Case "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                    toolsSubject_Eng = True
                Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                    toolsSubject_Math = True
            End Select
        Next
    End Sub

    Private Function getClassRoomCanMakeQuiz(Optional ByVal InputConn As SqlConnection = Nothing) As String
        Dim db As New ClassConnectSql()
        Dim sql As New StringBuilder()
        sql.Append("SELECT TOP 1 l.Level_ShortName FROM tblTestSetQuestionSet ts ")
        sql.Append("INNER JOIN tblQuestionset qs ON ts.QSet_Id = qs.QSet_Id ")
        sql.Append("INNER JOIN tblQuestionCategory q ON qs.QCategory_Id = q.QCategory_Id ")
        sql.Append("INNER JOIN tblBook b ON q.Book_Id = b.BookGroup_Id ")
        sql.Append("INNER JOIN tblLevel l ON b.Level_Id = l.Level_Id ")
        sql.Append("WHERE ts.TestSet_Id = '" & Session("newTestSetId").ToString & "' AND ts.IsActive = 1 ")
        sql.Append("ORDER BY l.Level DESC;")

        Dim className As String = db.ExecuteScalar(sql.ToString(), InputConn)

        'Dim classAll As New StringBuilder()
        'If className.Substring(0, 2) = "ป." Then
        '    For i = roomStart To 6
        '        classAll.Append(String.Format("ป.{0}", i))
        '    Next
        '    For j = 1 To 6
        '        classAll.Append(String.Format("ม.{0}", j))
        '    Next
        'Else
        '    For j = roomStart To 6
        '        classAll.Append(String.Format("ม.{0}", j))
        '    Next
        'End If

        If className.IndexOf("-") > -1 Then
            Dim classAll As New StringBuilder()
            Dim c As String = className.Substring(0, 1)
            Dim firstRoom As Integer = className.Substring(2, 1)
            Dim lastRoom As Integer = className.Substring(className.Length - 1, 1)
            For i = firstRoom To lastRoom
                classAll.Append(String.Format("{0}.{1}", c, i))
            Next
            Return classAll.ToString()
        End If

        Return className
    End Function

    ' update 02-05-56 tools ที่ใช้ใน quiz มีอะไรบ้าง เอาเลขไป Insert เริ่มกันเลย
    Private Function setToolsInQuiz() As Integer
        Dim arrTools As Array = {chkWithCalculator.Checked, chkWithDictionary.Checked, chkWithWordBook.Checked, chkWithNotes.Checked, chkWithProtractor.Checked}
        Dim EnabledTools As Integer = 0
        Dim Tools As Array = {2, 4, 8, 16, 32}
        Dim num As Integer = 0

        For Each arrChecked As Boolean In arrTools
            If arrChecked = True Then
                EnabledTools = EnabledTools + Tools(num)
            End If
            num = num + 1
        Next

        Return EnabledTools
    End Function

    ' Set checkbox Tools T/F on page load เมื่อตอน Pageload ค่าเก่าใช้อะไรบ้าง
    Private Sub setCheckboxUseTools(ByVal EnabledTools As Integer)
        If Not EnabledTools = 0 Then
            chkUseTools.Checked = True
            ' calculator
            If (EnabledTools And 2) = 2 Then
                chkWithCalculator.Checked = True
            End If
            ' dictionary
            If (EnabledTools And 4) = 4 Then
                chkWithDictionary.Checked = True
            End If
            ' wordbook
            If (EnabledTools And 8) = 8 Then
                chkWithWordBook.Checked = True
            End If
            ' note
            If (EnabledTools And 16) = 16 Then
                chkWithNotes.Checked = True
            End If
            ' protractor
            If (EnabledTools And 32) = 32 Then
                chkWithProtractor.Checked = True
            End If
        End If
    End Sub

    Private Sub BindDDLSoundlabName(ByVal SchoolId As String, Optional ByVal Inputconn As SqlConnection = Nothing)

        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim dt As DataTable = ClsActivity.GetDDLLab(SchoolId)

        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                DDLSoundLabName.Items.Add(dt.Rows(index)("TabletLab_Name").ToString())
                DDLSoundLabName.Items(index).Value = dt.Rows(index)("TabletLab_Id").ToString()
            Next
        Else
            DDLSoundLabName.Items.Add("ไม่มีห้อง Soundlab")
        End If
        DDLSoundLabName.SelectedIndex = 0
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
                    Dim StrScript As String = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=" & FolderName & "&PageName=" & PageName & "','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)
                End If
            End If
        End If
    End Sub

    ' <<< ChkTabletLab >>>
    <Services.WebMethod()>
    Public Shared Function GetTabletLab(ByVal Room As String, ByVal Level As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        'output สถาะว่ามี ไม่มี มีห้องอะไรบ้าง 
        'ดูว่าโรงเรียนนี้มีห้องแล็บป่าวbtnChangeLv

        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim TabletLabAmount As String = ClsActivity.GetTabletLab(HttpContext.Current.Session("SchoolID").ToString())

        Return TabletLabAmount

    End Function

    'เช็คว่าห้องที่กำลัง SettingQuiz นี้อยู่เป็นห้อง Lab ไหน
    Private Sub ChkSoundLabRoom()
        Try
            Dim ClientIP As String = ServiceSystem.GetIPAddress()
            Dim ClsRedis As New KnowledgeUtils.RedisStore()
            If ClsRedis.Getkey(ClientIP) <> "" Then
                Dim RoomName As String = ClsRedis.Getkey(ClientIP).ToString().Trim()
                For Each r As ListItem In DDLSoundLabName.Items
                    If r.Text.ToLower() = RoomName.ToLower() Then
                        DDLSoundLabName.ClearSelection()
                        r.Selected = True
                        DDLSoundLabName.Enabled = False
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

#Region "function ในการเพิ่มห้องของ standalone + checkmark"
    <Services.WebMethod()>
    Public Shared Function GetNoOfStudentInRoom(ByVal ClassName As String, ByVal RoomName As String) As String
        Dim schoolCode As String = HttpContext.Current.Session("SchoolID").ToString()
        'Dim sb As New StringBuilder()
        'sb.Append("select COUNT(student_Id) as StudentAmount  from dbo.t360_tblStudent where Student_CurrentClass = '" & ClassName & "' ")
        'sb.Append(" And Student_CurrentRoom = '" & RoomName.Insert(0, "/") & "'and School_Code = '" & schoolCode & "' and Student_Status = 1 AND Student_IsActive = 1;")
        'Try
        '    Dim db As New ClassConnectSql()
        '    Return CInt(db.ExecuteScalar(sb.ToString()))
        'Catch ex As Exception
        '    Return 0
        'End Try
        Dim dt As DataTable = New ClassConnectSql().getdata("SELECT Student_Code,Student_CurrentNoInRoom FROM t360_tblStudent WHERE Student_CurrentClass = '" & ClassName & "' AND Student_CurrentRoom = '" & RoomName.Insert(0, "/") & "' AND Student_IsActive = 1 AND Student_Status = 1 AND School_Code = '" & schoolCode & "' ORDER BY Student_CurrentNoInRoom;")
        Dim strJson As New StringBuilder()
        For Each r In dt.Rows
            strJson.Append(String.Format("{{""number"":""{0}"",""studentid"":""{1}""}},", r("Student_CurrentNoInRoom"), r("Student_Code")))
        Next
        Dim result As String = strJson.ToString()
        Return String.Format("{{""students"":[{0}]}}", result.Substring(0, result.Length - 1))
    End Function

    Private Shared Function deserialize(Of T)(ByVal jsonStr As String) As T
        Dim s = New System.Web.Script.Serialization.JavaScriptSerializer()
        Return s.Deserialize(Of T)(jsonStr)
    End Function

    Private Shared Function GetRoomInClass(ByVal ClassName As String, ByVal RoomName As String) As DataTable
        Dim db As New ClassConnectSql()
        Dim schoolCode As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim sql As String = " SELECT * FROM t360_tblRoom WHERE School_Code = '" & schoolCode & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "' AND Room_IsActive = 1; "
        Return db.getdata(sql)
    End Function

    Private Shared Function ExecuteSQLWithTransection(ByRef Sql As String) As Boolean
        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(Sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <Services.WebMethod()>
    Public Shared Function AddNewRoom(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As String) As String
        If RoomName = "" AndAlso (NoOfStudent <> 0 AndAlso NoOfStudent <> "") Then
            Return "NOT"
        End If
        '
        Dim dt As DataTable = GetRoomInClass(ClassName, RoomName)
        If dt.Rows.Count = 0 Then
            Dim StudentsJson = deserialize(Of studentCollection)(NoOfStudent)
            Dim strJsonStudentDuplicate As String = CheckStudentIdDuplicate(StudentsJson)
            If strJsonStudentDuplicate <> "" Then
                Return String.Format("{{""students"":[{0}]}}", strJsonStudentDuplicate.Substring(0, strJsonStudentDuplicate.Length - 1))
            End If
            If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                Return AddRoomAndStudentWithCheckMark(ClassName, RoomName, StudentsJson).ToString()
            Else
                Return AddRoomAndStudent(ClassName, RoomName, StudentsJson).ToString()
            End If
        End If
        Return "ExistRoom"
    End Function

    Private Shared Function CheckStudentIdDuplicate(ByVal StudentsJson As studentCollection) As String
        Dim dt As DataTable = New ClassConnectSql().getdata("SELECT Student_Code,Student_CurrentNoInRoom FROM t360_tblStudent WHERE Student_IsActive = 1 AND Student_Status = 1 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "';")
        Dim strJson As New StringBuilder()
        For Each s In StudentsJson.students
            Dim q = (From t In dt.AsEnumerable() Where t.Field(Of String)("Student_Code") = s.studentid.ToString() Select t.Field(Of String)("Student_Code")).SingleOrDefault()
            If q <> "" Then
                strJson.Append(String.Format("{{""number"":""{0}"",""studentid"":""{1}""}},", s.number, s.studentid))
            End If
        Next
        Return strJson.ToString()
    End Function

    Private Shared Function AddRoomAndStudent(ByVal ClassName As String, ByVal RoomName As String, ByVal StudentsJson As studentCollection) As Boolean
        Dim sb As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        sb.Append(" DECLARE @RoomId AS uniqueidentifier;  SET @RoomId =  NEWID(); ")
        sb.Append(" DECLARE @AcademicYear as smallint; SET @AcademicYear = (SELECT TOP 1 Calendar_Year FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        sb.Append(" DECLARE @CalendarId AS uniqueidentifier; SET @CalendarId = (SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        sb.Append(" INSERT INTO t360_tblRoom VALUES ('" & schoolId & "','" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,1,dbo.GetThaiDate(),NULL);")
        'Dim a As String = GetClassRunNumber(ClassName) ' เลขรหัสนักเรียนตัวแรกบอกระดับชั้น ประถม,มัธยม
        'Dim b As String = ClassName.Substring(2, 1) 'เลขชั้น
        'For i As Integer = 1 To NoOfStudent
        '    Dim studentId As String = String.Format("{0}{1}{2}{3}", a, b, RoomName, i.ToString("00"))
        '    sb.Append(String.Format("DECLARE @studentId{0} AS uniqueidentifier;  SET @studentId{0} =  NEWID();", i))
        '    sb.Append(" INSERT INTO t360_tblStudent VALUES(@studentId" & i & ",'" & schoolId & "','" & studentId & "','ไม่ระบุ','นักเรียน','เลขที่ '" & i & ",NULL,NULL,NULL,NULL,NULL,1," & i & ",'" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,NULL,NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,NULL,NULL,NULL,NULL,NULL,0); ")
        '    sb.Append(String.Format(" INSERT INTO t360_tblStudentRoom VALUES(NEWID(),@studentId{0},{1},{0},{2},{3},dbo.GetThaiDate(),8,1,@CalendarId,dbo.GetThaiDate(),@RoomId,NULL);",
        '                            i, schoolId, ClassName, RoomName))
        'Next

        'Dim StudentsJson = deserialize(Of studentCollection)(NoOfStudent)
        For Each student In StudentsJson.students
            Dim studentId As String = student.studentid.ToString()
            sb.Append(String.Format("DECLARE @studentId{0} AS uniqueidentifier;  SET @studentId{0} =  NEWID();", student.number))
            sb.Append(" INSERT INTO t360_tblStudent VALUES(@studentId" & student.number & ",'" & schoolId & "','" & studentId & "','ไม่ระบุ','นักเรียน','เลขที่ " & student.number & "',NULL,NULL,NULL,NULL,NULL,1," & student.number & ",'" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,NULL,NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,NULL,NULL,NULL,NULL,NULL,0); ")
            sb.Append(String.Format(" INSERT INTO t360_tblStudentRoom VALUES(NEWID(),@studentId{0},'{1}',{0},'{2}','/{3}',dbo.GetThaiDate(),@AcademicYear,8,1,@CalendarId,dbo.GetThaiDate(),@RoomId,NULL);",
                                    student.number, schoolId, ClassName, RoomName))
        Next

        Return ExecuteSQLWithTransection(sb.ToString())
    End Function

    Private Shared Function AddRoomAndStudentWithCheckMark(ByVal ClassName As String, ByVal RoomName As String, ByVal StudentsJson As studentCollection) As Boolean
        Dim sb As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim clsCheckmark As New ClsCheckMark()
        Dim classId As Integer = clsCheckmark.GetClassId(ClassName) ' classid ของ checkmark
        Dim students As New List(Of StudentCheckMark)
        sb.Append(" DECLARE @RoomId AS uniqueidentifier;  SET @RoomId =  NEWID(); ")
        sb.Append(" DECLARE @AcademicYear as smallint; SET @AcademicYear = (SELECT TOP 1 Calendar_Year FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        sb.Append(" DECLARE @CalendarId AS uniqueidentifier; SET @CalendarId = (SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        sb.Append(" INSERT INTO t360_tblRoom VALUES ('" & schoolId & "','" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,1,dbo.GetThaiDate(),NULL);")

        'For i As Integer = 1 To NoOfStudent
        '    Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, i, schoolId)
        '    students.Add(student)
        '    sb.Append(student.ToStringSQLInsertT360())
        'Next

        'Dim StudentsJson = deserialize(Of studentCollection)(NoOfStudent)
        For Each s In StudentsJson.students
            Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, s.number, schoolId, s.studentid)
            students.Add(student)
            sb.Append(student.ToStringSQLInsertT360())
        Next


        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sb.ToString())
            If clsCheckmark.AddStudents(students) Then
                db.CommitTransection()
                Return True
            End If
            db.RollbackTransection()
            Return False
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    ' function สำหรับ run เลขตัวแรกของรหัสนักเรียน ป = 1, ม = 2
    Private Shared Function GetClassRunNumber(ByRef ClassName As String) As String
        If ClassName.Contains("ป.") Then
            Return "1"
        End If
        Return "2"
    End Function

    Private Shared Function NewStudentCheckMark(ByRef ClassName As String, ByRef RoomName As String, ByRef classId As Integer, ByRef i As Integer, ByRef schoolId As String, ByVal studentId As String) As StudentCheckMark
        Dim s As New StudentCheckMark()
        s.StudentId = studentId 'String.Format("{0}{1}{2}{3}", GetClassRunNumber(ClassName), ClassName.Substring(2, 1), RoomName, i.ToString("00"))
        s.StudentFName = "นักเรียน"
        s.StudentLName = "เลขที่ " & i.ToString()
        s.ClassId = classId
        s.ClassName = ClassName
        s.StudentNumber = i
        s.StudentRoom = RoomName
        s.StudentPrefixName = "ด.ช."
        s.SchoolId = schoolId
        Return s
    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteRoom(ByVal ClassName As String, ByVal RoomName As String) As Boolean
        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Return DeleteRoomAndStudentWithCheckMark(ClassName, RoomName)
        End If
        Return DeleteRoomAndStudent(ClassName, RoomName)
    End Function

    Private Shared Function DeleteRoomAndStudent(ByVal ClassName As String, ByVal RoomName As String) As Boolean
        Dim sql As String = GetSQLDeleteRoomAndStudentT360(ClassName, RoomName)
        Return ExecuteSQLWithTransection(sql)
    End Function

    Public Shared Function DeleteRoomAndStudentWithCheckMark(ByVal ClassName As String, ByVal RoomName As String) As Boolean
        Dim sql As String = GetSQLDeleteRoomAndStudentT360(ClassName, RoomName)
        Dim clsCheckMark As New ClsCheckMark()
        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql)
            If clsCheckMark.RemoveStudents(ClassName, RoomName) Then
                db.CommitTransection()
                Return True
            End If
            db.RollbackTransection()
            Return False
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Shared Function GetSQLDeleteRoomAndStudentT360(ByVal ClassName As String, ByVal RoomName As String) As String
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim sb As New StringBuilder()
        sb.Append(" DECLARE @roomid AS uniqueidentifier ")
        sb.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "'  AND Room_IsActive = 1);")
        sb.Append(" UPDATE t360_tblRoom SET Room_IsActive = 0 WHERE School_Code = '" & schoolId & "' AND Room_Id = @roomid;")
        sb.Append(" UPDATE t360_tblStudent SET Student_IsActive = 0  WHERE School_Code = '" & schoolId & "' AND Student_CurrentRoomId = @roomid;")
        sb.Append(" UPDATE t360_tblStudentRoom SET SR_IsActive = 0 WHERE School_Code = '" & schoolId & "' AND Room_Id = @roomid;")
        sb.Append(String.Format(" UPDATE tblQuiz SET IsActive = 0, LastUpdate = dbo.GetThaiDate() WHERE t360_SchoolCode = '{0}' AND t360_ClassName = '{1}' AND t360_RoomName = '/{2}';", schoolId, ClassName, RoomName))
        Return sb.ToString()
    End Function

    Private Shared Function CheckStudentExistWhenUpdate(ByVal oldStudent As studentCollection, ByVal StudentsJson As studentCollection) As String
        Dim sql As String = "SELECT Student_Code,Student_CurrentNoInRoom FROM t360_tblStudent WHERE Student_IsActive = 1 AND Student_Status = 1 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "' AND Student_Code NOT IN ("
        Dim sqltemp As New StringBuilder()
        For Each o In oldStudent.students
            sqltemp.Append("'" & o.studentid & "',")
        Next
        sql &= String.Format("{0});", sqltemp.ToString().Substring(0, sqltemp.ToString().Length - 1))
        Dim dt As DataTable = New ClassConnectSql().getdata(sql)
        Dim strJson As New StringBuilder()
        For Each s In StudentsJson.students
            Dim q = (From t In dt.AsEnumerable() Where t.Field(Of String)("Student_Code") = s.studentid.ToString() Select t.Field(Of String)("Student_Code")).SingleOrDefault()
            If q <> "" Then
                strJson.Append(String.Format("{{""number"":""{0}"",""studentid"":""{1}""}},", s.number, s.studentid))
            End If
        Next
        Return strJson.ToString()
    End Function


    <Services.WebMethod()>
    Public Shared Function UpdateRoom(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As String, ByVal NewNoOfStudent As String, ByVal NewRoomName As String) As String
        'If CInt(NewNoOfStudent) = 0 Then ' ถ้าจำนวนนักเรียนที่จะอัพเดทเป็น 0 ให้ออกเลย
        '    Return True
        'End If

        Dim dt As DataTable = GetRoomInClass(ClassName, NewRoomName)
        If dt.Rows.Count = 0 Then

            Dim oldStudentsJson = deserialize(Of studentCollection)(NoOfStudent)
            Dim newStudentsJson = deserialize(Of studentCollection)(NewNoOfStudent)

            Dim strJsonStudentDuplicate As String = CheckStudentExistWhenUpdate(oldStudentsJson, newStudentsJson)
            If strJsonStudentDuplicate <> "" Then
                Return String.Format("{{""students"":[{0}]}}", strJsonStudentDuplicate.Substring(0, strJsonStudentDuplicate.Length - 1))
            End If

            Dim oldData = oldStudentsJson.students.ToList() ' แปลงเป็น list จะได้ใช้งาน except ได้
            Dim newData = newStudentsJson.students.ToList()

            ' ก่อนจะลงไปเซฟ ต้องดูก่อนว่ามีรหัสที่จะ insert หรือยัง
            If Not NewUpdateStudent(ClassName, RoomName, NewRoomName, oldData, newData) Then
                Return "False"
            End If

            'update แค่เลขห้องอย่างเดียว
            If RoomName <> NewRoomName Then
                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Return UpdateRoomAndStudentWithCheckmark(ClassName, RoomName, NewRoomName)
                End If
                Return UpdateRoomAndStudent(ClassName, RoomName, NewRoomName)
            End If

            Return "True"

        Else
            ' case ถ้าเลขห้องเดิม update แต่เด็กอย่างเดียว
            If RoomName = NewRoomName Then
                Dim oldStudentsJson = deserialize(Of studentCollection)(NoOfStudent)
                Dim newStudentsJson = deserialize(Of studentCollection)(NewNoOfStudent)

                Dim strJsonStudentDuplicate As String = CheckStudentExistWhenUpdate(oldStudentsJson, newStudentsJson)
                If strJsonStudentDuplicate <> "" Then
                    Return String.Format("{{""students"":[{0}]}}", strJsonStudentDuplicate.Substring(0, strJsonStudentDuplicate.Length - 1))
                End If

                Dim oldData = oldStudentsJson.students.ToList() ' แปลงเป็น list จะได้ใช้งาน except ได้
                Dim newData = newStudentsJson.students.ToList()

                ' ก่อนจะลงไปเซฟ ต้องดูก่อนว่ามีรหัสที่จะ insert หรือยัง
                If Not NewUpdateStudent(ClassName, RoomName, NewRoomName, oldData, newData) Then
                    Return "False"
                End If
                Return "True"
            End If

        End If

        Return "ExistRoom"
    End Function

    Public Shared Function NewUpdateStudent(ByVal ClassName As String, ByVal RoomName As String, ByVal NewRoomName As String, ByVal dataTodelete As List(Of studentJson), ByVal dataToInsert As List(Of studentJson)) As Boolean
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim clsCheckMark As New ClsCheckMark()

        Dim classId As Integer = clsCheckMark.GetClassId(ClassName) ' classid ของ checkmark
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "' AND Room_IsActive = 1);")
        sql.Append(" DECLARE @AcademicYear as smallint; SET @AcademicYear = (SELECT TOP 1 Calendar_Year FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        sql.Append(" DECLARE @CalendarId AS uniqueidentifier; SET @CalendarId = (SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND IsActive = 1);")
        ' student todelete
        Dim StudentsToDelete As New List(Of StudentCheckMark)
        For Each eachStudentToDelete In dataTodelete
            Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, eachStudentToDelete.number, schoolId, eachStudentToDelete.studentid)
            sql.Append(student.ToStringSQLDeleteT360())
            StudentsToDelete.Add(student)
        Next

        ' student toinsert
        Dim StudentsToInsert As New List(Of StudentCheckMark)
        For Each eachStudentToInsert In dataToInsert
            Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, NewRoomName, classId, eachStudentToInsert.number, schoolId, eachStudentToInsert.studentid)
            sql.Append(student.ToStringSQLInsertT360())
            StudentsToInsert.Add(student)
        Next

        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql.ToString)

            If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                If Not clsCheckMark.RemoveStudents(StudentsToDelete) Then
                    db.RollbackTransection()
                    Return False
                End If
                If Not clsCheckMark.AddStudents(StudentsToInsert) Then
                    db.RollbackTransection()
                    Return False
                End If
            End If

            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function




    Private Shared Function UpdateRoomAndStudent(ByVal ClassName As String, ByVal RoomName As String, ByVal NewRoomName As String) As Boolean '4
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "' AND Room_IsActive = 1);")
        sql.Append(String.Format("UPDATE t360_tblRoom SET Room_Name = '/{0}',LastUpdate = dbo.GetThaiDate() WHERE Room_Id = @roomid;", NewRoomName))
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function
    Private Shared Function UpdateRoomAndStudent(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As Integer, ByVal NoOfNewStudent As Integer, ByVal NewRoomName As String) As Boolean '3
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim runNumberLevel As String = GetClassRunNumber(ClassName) ' เลขรหัสนักเรียนตัวแรกบอกระดับชั้น ประถม,มัธยม
        Dim runNumberClass As String = ClassName.Substring(2, 1) 'เลขชั้น
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "' AND Room_IsActive = 1);")
        sql.Append(String.Format("UPDATE t360_tblRoom SET Room_Name = '/{0}',LastUpdate = dbo.GetThaiDate() WHERE Room_Id = @roomid;", NewRoomName))
        Dim r = GetRunningNumber(NoOfStudent, NoOfNewStudent)
        For i As Integer = r.startStudentId To r.endStudentId
            Dim studentId As String = String.Format("{0}{1}{2}{3}", runNumberLevel, runNumberClass, RoomName, i.ToString("00"))
            If r.isAddStudent Then
                sql.Append(" INSERT INTO t360_tblStudent VALUES(NEWID(),'" & schoolId & "','" & studentId & "','ไม่ระบุ','นักเรียน','เลขที่ '" & i & ",NULL,NULL,NULL,NULL,NULL,1," & i & ",'" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,NULL,NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,NULL,NULL,NULL); ")
            Else
                sql.Append("UPDATE t360_tblStudent SET Student_IsActive = 0  WHERE School_Code = '" & schoolId & "' AND Student_CurrentRoomId = @roomid AND Student_Code = '" & studentId & "' ;")
            End If
        Next
        Return ExecuteSQLWithTransection(sql.ToString())
    End Function
    Private Shared Function UpdateRoomAndStudent(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As Integer, ByVal NoOfNewStudent As Integer) As Boolean '2
        Dim sb As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim runNumberLevel As String = GetClassRunNumber(ClassName) ' เลขรหัสนักเรียนตัวแรกบอกระดับชั้น ประถม,มัธยม
        Dim runNumberClass As String = ClassName.Substring(2, 1) 'เลขชั้น
        sb.Append(" DECLARE @roomid AS uniqueidentifier ")
        sb.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "');")
        If NoOfNewStudent <> NoOfNewStudent Then
            Dim r = GetRunningNumber(NoOfStudent, NoOfNewStudent)
            For i As Integer = r.startStudentId To r.endStudentId
                Dim studentId As String = String.Format("{0}{1}{2}{3}", runNumberLevel, runNumberClass, RoomName, i.ToString("00"))
                If r.isAddStudent Then
                    sb.Append(" INSERT INTO t360_tblStudent VALUES(NEWID(),'" & schoolId & "','" & studentId & "','ไม่ระบุ','นักเรียน','เลขที่ '" & i & ",NULL,NULL,NULL,NULL,NULL,1," & i & ",'" & ClassName & "','" & RoomName.Insert(0, "/") & "',@RoomId,NULL,NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,NULL,NULL,NULL); ")
                Else
                    sb.Append("UPDATE t360_tblStudent SET Student_IsActive = 0  WHERE School_Code = '" & schoolId & "' AND Student_CurrentRoomId = @roomid AND Student_Code = '" & studentId & "' ;")
                End If
            Next
        End If
        Return ExecuteSQLWithTransection(sb.ToString())
    End Function

    Private Shared Function UpdateRoomAndStudentWithCheckmark(ByVal ClassName As String, ByVal RoomName As String, ByVal NewRoomName As String) As Boolean '4
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim clsCheckMark As New ClsCheckMark()
        Dim Students As New List(Of StudentCheckMark)
        Dim classId As Integer = clsCheckMark.GetClassId(ClassName) ' classid ของ checkmark
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "');")
        sql.Append(String.Format("UPDATE t360_tblRoom SET Room_Name = '{0}',LastUpdate = dbo.GetThaiDate() WHERE Room_Id = @roomid;", NewRoomName))


        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql.ToString)
            If Not clsCheckMark.EditRoom(ClassName, NewRoomName, RoomName) Then
                db.RollbackTransection()
                Return False
            End If
            db.CommitTransection()
            Return False
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function
    Private Shared Function UpdateRoomAndStudentWithCheckmark(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As Integer, ByVal NoOfNewStudent As Integer, ByVal NewRoomName As String) As Boolean '3
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim clsCheckMark As New ClsCheckMark()
        Dim Students As New List(Of StudentCheckMark)
        Dim classId As Integer = clsCheckMark.GetClassId(ClassName) ' classid ของ checkmark
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "'  AND Room_IsActive = 1);")
        sql.Append(String.Format("UPDATE t360_tblRoom SET Room_Name = '/{0}',LastUpdate = dbo.GetThaiDate() WHERE Room_Id = @roomid;", NewRoomName))
        Dim r = GetRunningNumber(NoOfStudent, NoOfNewStudent)
        If r.isAddStudent Then
            For i As Integer = r.startStudentId To r.endStudentId
                'Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, i, schoolId)
                'Students.Add(student)
                'sql.Append(student.ToStringSQLInsertT360())
            Next
        Else
            For i As Integer = r.startStudentId To r.endStudentId
                'Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, i, schoolId)
                'Students.Add(student)
                'sql.Append(student.ToStringSQLUpdateT360())
            Next
        End If
        sql.Append(String.Format("UPDATE t360_tblStudent SET Student_CurrentRoom = '/{0}' WHERE School_Code = '{1}' AND Student_CurrentRoomId = @roomid AND Student_IsActive = 1;", NewRoomName, schoolId))
        sql.Append(String.Format("UPDATE t360_tblStudentRoom SET Room_Name = '/{0}' WHERE School_Code = '{1}' AND Room_Id = @roomid AND SR_IsActive = 1;", NewRoomName, schoolId))
        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql.ToString)
            If r.isAddStudent Then
                If Not clsCheckMark.AddStudents(Students) Then
                    db.RollbackTransection()
                    Return False
                End If
            Else
                If Not clsCheckMark.EditStudents(Students) Then
                    db.RollbackTransection()
                    Return False
                End If
                If Not clsCheckMark.EditRoom(ClassName, NewRoomName, RoomName) Then
                    db.RollbackTransection()
                    Return False
                End If
            End If
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function
    Private Shared Function UpdateRoomAndStudentWithCheckmark(ByVal ClassName As String, ByVal RoomName As String, ByVal NoOfStudent As Integer, ByVal NoOfNewStudent As Integer) As Boolean
        Dim sql As New StringBuilder()
        Dim schoolId As String = HttpContext.Current.Session("SchoolID").ToString()
        Dim clsCheckMark As New ClsCheckMark()
        Dim Students As New List(Of StudentCheckMark)
        Dim classId As Integer = clsCheckMark.GetClassId(ClassName) ' classid ของ checkmark
        sql.Append(" DECLARE @roomid AS uniqueidentifier ")
        sql.Append(" SET @roomid = (SELECT Room_Id FROM t360_tblRoom WHERE School_Code = '" & schoolId & "' AND Class_Name = '" & ClassName & "' AND Room_Name = '" & RoomName.Insert(0, "/") & "');")
        Dim r = GetRunningNumber(NoOfStudent, NoOfNewStudent)
        If r.isAddStudent Then
            For i As Integer = r.startStudentId To r.endStudentId
                'Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, i, schoolId)
                'Students.Add(student)
                'sql.Append(student.ToStringSQLInsertT360())
            Next
        Else
            For i As Integer = r.startStudentId To r.endStudentId
                'Dim student As StudentCheckMark = NewStudentCheckMark(ClassName, RoomName, classId, i, schoolId)
                'Students.Add(student)
                'sql.Append(student.ToStringSQLUpdateT360())
            Next
        End If

        Dim db As New ClassConnectSql()
        Try
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql.ToString)
            If r.isAddStudent Then
                If clsCheckMark.AddStudents(Students) Then
                    db.CommitTransection()
                    Return True
                End If
            Else
                If clsCheckMark.EditStudents(Students) Then
                    db.CommitTransection()
                    Return True
                End If
            End If
            db.RollbackTransection()
            Return False
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Shared Function GetRunningNumber(ByVal NoOfStudent As Integer, ByVal NoOfNewStudent As Integer) As Object
        If NoOfStudent < NoOfNewStudent Then
            Return New With {.startStudentId = NoOfStudent + 1, .endStudentId = NoOfNewStudent, .isAddStudent = True}
        End If
        Return New With {.startStudentId = (NoOfStudent - (NoOfStudent - NoOfNewStudent)) + 1, .endStudentId = NoOfStudent, .isAddStudent = False}
    End Function

#End Region

End Class

Public Class studentCollection
    Private _students As IEnumerable(Of studentJson)
    Public Property students() As IEnumerable(Of studentJson)
        Get
            Return _students
        End Get
        Set(ByVal value As IEnumerable(Of studentJson))
            _students = value
        End Set
    End Property
End Class
Public Class studentJson
    Private _number As Integer
    Public Property number() As Integer
        Get
            Return _number
        End Get
        Set(ByVal value As Integer)
            _number = value
        End Set
    End Property
    Private _studentid As String
    Public Property studentid() As String
        Get
            Return _studentid
        End Get
        Set(ByVal value As String)
            _studentid = value
        End Set
    End Property
End Class
