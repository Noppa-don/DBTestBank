Imports BusinessTablet360
Imports System.Web.Script.Serialization
Imports KnowledgeUtils
Imports System.Web

Public Class ChooseQuestionSet
    Inherits System.Web.UI.Page

    Dim objPdf As New ClsPDF(New ClassConnectSql)
    Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
    Dim ClsPractice As New Service.ClsPracticeMode(New ClassConnectSql)
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    ' Dim GroupSubjectId As String
    'Dim LevelId As String
    'Dim DeviceUniqueID As String
    Public player_Id As String
    Public GroupName As String
    Public DVID As String
    Protected TokenId As String

    Public txtStep1, txtStep2, txtStep3, txtStep4, ChkFontSize, MarginSize, PaddingSize As String

    Public IE As String
    Protected IsMobile As Boolean

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected SubjectName As String = ""
    Protected IsMaxOnet As Boolean = ClsKNSession.IsMaxONet

    Protected BackUrl As String = ""
    Protected RedirectMode As String = ""
    Protected ChooseClassURL As String = ""
    Protected ChooseSubjectURL As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Session("SessionStatus") = "1"
        End If

        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString().ToLower()
        If AgentString.IndexOf("android") <> -1 OrElse AgentString.IndexOf("iphone") <> -1 OrElse AgentString.IndexOf("ipad") <> -1 Then
            IsMobile = True
        End If

        DVID = Request.QueryString("deviceuniqueid")

        If IsMaxOnet Then
            If Session("ChooseMode") Is Nothing Then Session("ChooseMode") = Request.QueryString("DashboardMode")
            Session("selectedSession") = "PracticeFromComputer"
            TokenId = Request.QueryString("token")
            RedirectMode = Request.QueryString("RedirectMode")
        End If

        If Session("PClassId") Is Nothing Or Session("SchoolCode") Is Nothing Then
            If IsMaxOnet Then
                Response.Redirect(String.Format("ChooseTestsetMaxOnet.aspx?deviceUniqueId={0}&token={1}", DVID, TokenId))
            End If
            Exit Sub
        End If

        ' Dim SchoolCode As String = Request.Form("SchoolCode")
        ' Dim LevelId As String = Request.Form("LevelId")
        ' Dim GroupSubjectId As String = Request.Form("GroupSubjectId")

#If DEBUG Then
        'Session("userid") = "1"
        'Session("PDeviceId") = "RealTest16"
        'Session("PracticeMode") = "true"
        'DeviceUniqueID = "F45A84A9-0A35-4313-BE07-6E56EAE5D93B" ' มี
        'GroupSubjectId = "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
        'Session("PClassId") = "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C"
#End If
#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("PClassId") = "14a28f3d-1aff-429d-b7a1-927a28e010bd"
        Session("SchoolCode") = "1000001"
        Session("PSubjectName") = "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
        IE = "1"
#End If

        If HttpContext.Current.Application("Sess" & Session("PDeviceId")) IsNot Nothing Then
            HttpContext.Current.Application("Sess" & Session("PDeviceId")) = Nothing
        End If

        CreateNav()

        If Session("PracticeFromComputer") = False Then 'ถ้าไม่ได้ฝึกฝนผ่านคอมพิมเตอร์ต้องหา GroupName เพื่อ add ให้ SignalR
            If Not Session("IsTeacher") = "1" Then 'ถ้าเป็นเด็ก
                DVID = Session("PDeviceId")
                If Session("selectedSession") Is Nothing Then
                    Session("selectedSession") = ClsActivity.GetGroupNameByDVID(DVID)
                End If
                GroupName = Session("selectedSession").ToString()

                If Not Page.IsPostBack Then
                    ' ส่วนของการแสดง qtip
                    If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                        Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                        Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                        NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
                    End If
                End If
                'NeedShowTip = True
            End If
        End If

        Dim Check51 As String = ""
        Dim Check44 As String = ""


        If Page.IsPostBack Then

            If Request.Form("Show51") IsNot Nothing Then
                Check51 = Request.Form("Show51").ToString()
            End If

            If Request.Form("Show44") IsNot Nothing Then
                Check44 = Request.Form("Show44").ToString()
            End If

            If Check51 = "on" And Check44 = "on" Then
                Session("GetCkeckedYear") = "5144"


            ElseIf Check51 = "on" Then
                Session("GetCkeckedYear") = "51"

            Else
                Session("GetCkeckedYear") = "44"



            End If
        Else
            Session("GetCkeckedYear") = "51"
        End If

        player_Id = Session("UserId").ToString

        If IsMaxOnet Then
            SubjectName = Session("PSubjectName").ToString().ToSubjectShortThName() & " " & Session("PClassId").ToString().ToLevelShortName()
            SetBackURL()
        Else
            BackUrl = "../PracticeMode_Pad/ChooseSubject.aspx"
        End If

        Dim dtExam As DataTable
        If IsMaxOnet Then
            dtExam = ClsPracticeMode.GetQCategoryByGroupSubjectAndLevel(Session("PSubjectName"), Session("PClassId"), Session("GetCkeckedYear"), IsMaxOnet, TokenId)
        Else
            dtExam = ClsPracticeMode.GetQCategoryByGroupSubjectAndLevel(Session("PSubjectName"), Session("PClassId"), Session("GetCkeckedYear"), IsMaxOnet)
        End If

        divListExam.InnerHtml = ClsPractice.CreateTestUnitList(dtExam)

    End Sub

    Private Sub SetBackURL()
        'เช็คจำนวนวิชา
        If ClsPracticeMode.CheckOneSubjectMaxOnet(player_Id, Session("PClassId").ToString()) Then
            'มีวิชาเดียว เช็คว่ามีชั้นเดียวด้วยมั้ย
            If ClsPracticeMode.CheckOneLevelMaxOnet(player_Id) Then
                'มีวิชาเดียวชั้นเดียว กลับหน้าแรกเลย
                BackUrl = "../PracticeMode_Pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId
            Else
                'มีวิชาเดียว หลายชั้น กลับหน้าเลือกชั้น
                BackUrl = "../PracticeMode_Pad/ChooseClass.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=9&RedirectMode=" & RedirectMode
                ChooseClassURL = BackUrl
            End If
        Else
            'มีหลายวิชากลับหน้าเลือกวิชา
            BackUrl = "../PracticeMode_Pad/ChooseSubject.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=9&RedirectMode=" & RedirectMode
            ChooseSubjectURL = BackUrl

            ChooseClassURL = "../PracticeMode_Pad/ChooseClass.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=9&RedirectMode=" & RedirectMode
        End If
    End Sub

    Protected Friend Function CreateCategoryUnit(ByVal Qcategory_Id As String)

        Dim IsComputer As Boolean
        If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            IsComputer = True
        Else
            IsComputer = False
        End If

        Dim dt As DataTable = ClsPracticeMode.GetQSetByQcategory(Qcategory_Id, player_Id, IsComputer)
        Dim sb As New System.Text.StringBuilder()



        If Not IsNothing(dt) Then
            Dim qSetId, QuestionSet, Ispracticed, QSetYear As String
            For i = 0 To dt.Rows.Count - 1
                qSetId = dt.Rows(i)("Qset_Id").ToString()
                QuestionSet = dt.Rows(i)("QSet_Name").ToString()
                Ispracticed = dt.Rows(i)("IsPracticed").ToString
                QSetYear = dt.Rows(i)("Book_Syllabus").ToString
                'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)

                If i Mod 2 = 0 Then
                    sb.Append("<tr id=" & qSetId & " ><td style='background-color:#FFC;'>")
                Else
                    sb.Append("<tr id=" & qSetId & " ><td  style='background-color:white;'>")
                End If

                sb.Append(" <img id=""play_" & qSetId & """ src=""../Images/upgradeClass/Actions-arrow-right-icon.png"" class=""imgPlayQuiz"" />")

                sb.Append(" <img id = ""User_" & qSetId & """ src=""../Images/Homework/EverMade.png""class=""UserImage"" />")

                'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
                'Dim PositionCategory As Integer
                'Dim QuestionAfTerLine As String
                'Dim index As Integer = QuestionSet.IndexOf("</b> - ")
                '        PositionCategory = InStr(QuestionSet, "</b> - ")
                '        QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

                '        If QuestionAfTerLine.Length > 75 Then
                'Dim Strcut As String = CutString(qSetId)
                '            sb.Append(Strcut)
                '        Else
                sb.Append("<B>[")
                sb.Append(QSetYear)
                sb.Append("]  </B>")
                sb.Append(QuestionSet)
                'End If

                'If ExamAmount.Equals("0") Then
                '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a></td></tr>")
                'Else
                '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                'End If
                sb.Append(" <img src=""../Images/Delete-icon.png"" style='float:right; cursor: pointer; visibility:" & Ispracticed & "' />")
                sb.Append("</td></tr>")

            Next
        End If
        Return sb.ToString()

    End Function


    <Services.WebMethod()>
    Public Shared Function SaveTestset(ByVal QsetId As String, ByVal TokenId As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        '  Dim UserID As String = HttpContext.Current.Session("UserId").ToString(

        Dim KNS As New KNAppSession()

        Dim ClsP As New Service.ClsPracticeMode(New ClassConnectSql)

        Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)

        Dim ClsPrintTestset As New ClsPrintTestset(HttpContext.Current.Session("UserId").ToString(), QsetId, KNS.StoredValue("CurrentCalendarId").ToString, HttpContext.Current.Session("IsTeacher").ToString)

        'HttpContext.Current.Session("UserId") = HttpContext.Current.Session("PUserId")

        Dim TestSetID As String = ClsPrintTestset.GetTestSetID()

        If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PrintTestset Then
            Return "../testset/genpdf.aspx?TestsetID=" & TestSetID
        ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework Then
            Return "../Module/HomeworkAssignPage.aspx?TestsetID=" & TestSetID & "&IsNew=True&PageName=PracticeMode_Pad/ChooseQuestionset.aspx"
        ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Quiz Then
            Return "../Activity/SettingActivity.aspx?TestsetID=" & TestSetID
        ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice Or HttpContext.Current.Session("ChooseMode") = 9 Then

            HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice
            Dim dtUser As DataTable = ClsP.GetPlayerTypeFromTestset(HttpContext.Current.Session("UserId").ToString())

            If dtUser.Rows.Count <> 0 Then

                Dim UserType As String = dtUser.Rows(0)("Owner_Type").ToString
                Dim DeviceId As String = dtUser.Rows(0)("Tablet_SerialNumber").ToString

                If UserType = "2" Then
                    If BusinessTablet360.ClsKNSession.IsMaxONet Then
                        Return "../Activity/ActivityPage_Pad.aspx?ItemId=" & TestSetID & "&DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId") & "&token=" & TokenId
                    Else
                        Return "../Activity/ActivityPage_Pad.aspx?ItemId=" & TestSetID & "&DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")
                    End If
                Else
                    Return "../Activity/ActivityPage.aspx?TestsetID=" & TestSetID
                End If
            ElseIf HttpContext.Current.Session("PDeviceId") IsNot Nothing Then
                Dim dtLabTablet As DataTable = ClsPracticeMode.CheckLabTablet(HttpContext.Current.Session("PDeviceId"))
                If dtLabTablet.Rows.Count <> 0 Then
                    Return "../Activity/ActivityPage_Pad.aspx?ItemId=" & TestSetID & "&DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")
                End If

                Dim dtTabletSpare As DataTable = New ClassConnectSql().getdata(String.Format("SELECT * FROM t360_tblTablet WHERE Tablet_IsActive = 1 AND Tablet_SerialNumber = '{0}' AND Tablet_IsOwner = 0;", HttpContext.Current.Session("PDeviceId")))
                If dtTabletSpare.Rows.Count > 0 Then
                    Return "../Activity/ActivityPage_Pad.aspx?ItemId=" & TestSetID & "&DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")
                End If

            Else

                Return "../Activity/ActivityPage.aspx?TestsetID=" & TestSetID
            End If

        ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
            Return "../Activity/ActivityPage.aspx?TestsetID=" & TestSetID
        End If


        'Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
        'Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        'Dim KnSession As New ClsKNSession()
        'Dim ClassName As String
        'Dim RoomName As String
        'Dim SchoolCode As String
        'Dim player_Id As String
        'Dim IsUseTablet As String

        'If HttpContext.Current.Session("PracticeFromComputer") Then
        '    ClassName = "ม.1"
        '    RoomName = "/1"
        '    SchoolCode = HttpContext.Current.Application("DefaultSchoolCode")
        '    player_Id = HttpContext.Current.Application("DefaultUserId")
        '    IsUseTablet = "0"
        'Else
        '    Dim dtPlayer As DataTable = ClsPracticeMode.GetPlayerDetail(HttpContext.Current.Session("PDeviceId")) 'หาข้อมูลนักเรียนคนนั้น
        '    ClassName = dtPlayer.Rows(0)("ClassName").ToString
        '    RoomName = dtPlayer.Rows(0)("RoomName").ToString
        '    SchoolCode = dtPlayer.Rows(0)("School_Code").ToString
        '    player_Id = dtPlayer.Rows(0)("Student_Id").ToString
        '    IsUseTablet = "1"
        'End If


        'HttpContext.Current.Session("UserId") = player_Id


        'Dim PtestsetId As String = ClsPracticeMode.GetTestsetId(QsetId, player_Id, SchoolCode, HttpContext.Current.Session("PClassId"))

        'Dim testsetValue() As String = Split(PtestsetId, ",")

        'If testsetValue(1) = "New" Then 'Testset ใหม่ต้องเอาข้อสอบใส่ Table TestsetQuestionSet และ TestsetQuestionDetail ด้วย
        '    ClsPracticeMode.SetTSQSAndSetTSQD(testsetValue(0), QsetId)
        'End If



        'HttpContext.Current.Session("Quiz_Id") = ClsPracticeMode.SaveQuizDetail(PtestsetId, ClassName, RoomName, SchoolCode, player_Id, "1", False)

        'ClsActivity.getQsetInQuiz(PtestsetId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion

        'If Not HttpContext.Current.Session("PracticeFromComputer") Then
        '    ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, HttpContext.Current.Session("PracticeFromComputer"), ClsPracticeMode.GetQSetTypeFromQSetId(QsetId))
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

    End Function

    Public Function GetYearChecked(ByVal checkedYear As String) As String

        If HttpContext.Current.Session("GetCkeckedYear") = "5144" Then
            GetYearChecked = "checked=""checked"""
        ElseIf HttpContext.Current.Session("GetCkeckedYear") = "51" Then
            If checkedYear = "51" Then
                GetYearChecked = "checked=""checked"""
            Else
                GetYearChecked = String.Empty
            End If
        ElseIf HttpContext.Current.Session("GetCkeckedYear") = "44" Then
            If checkedYear = "44" Then
                GetYearChecked = "checked=""checked"""
            Else
                GetYearChecked = String.Empty
            End If
        End If

    End Function

    Private Sub CreateNav()

        If Session("ChooseMode") = EnumDashBoardType.Homework Then
            ChkFontSize = "21px"
            MarginSize = "60px"
            txtStep1 = " เลือกชั้น -->"
            txtStep2 = " เลือกวิชา -->"
            txtStep3 = " เลือกหน่วยฯ -->"
            txtStep4 = " สั่งการบ้าน"
            PaddingSize = "8px 10px 6px 10px"
        ElseIf Session("ChooseMode") = EnumDashBoardType.PrintTestset Then
            ChkFontSize = "21px"
            MarginSize = "60px"
            txtStep1 = " เลือกชั้น -->"
            txtStep2 = " เลือกวิชา -->"
            txtStep3 = " เลือกหน่วยฯ -->"
            txtStep4 = " สั่งพิมพ์ใบงาน"
            PaddingSize = "8px 10px 6px 10px"
        Else
            ChkFontSize = "25px"
            MarginSize = "100px"
            txtStep1 = "   เลือกชั้น -->   "
            txtStep2 = "   เลือกวิชา -->   "
            txtStep3 = "   เลือกหน่วยฯ   "
            PaddingSize = "5px 10px 6px 10px"
        End If

    End Sub

    Private Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click

        If IsMaxOnet Then
            Response.Redirect("ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId)
        Else
            If HttpContext.Current.Session("PracticeFromComputer") = True Then
                Response.Redirect("~/Loginpage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Quiz Then
                Response.Redirect("../Quiz/DashboardQuizPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework Then
                Response.Redirect("../Homework/DashboardHomeworkPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice Then
                Response.Redirect("../Practice/DashboardPracticePage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PrintTestset Then
                Response.Redirect("../PrintTestset/DashboardPrintTestsetPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.SetUp Then
                Response.Redirect("../Testset/DashboardSetupPage.aspx")
            ElseIf HttpContext.Current.Session("ChooseMode") = 9 Then
                Response.Redirect("~/PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId").ToString())
            End If
        End If


    End Sub

    <Services.WebMethod()>
    Public Shared Function getQuestionSetName(ByVal qSetId As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        If qSetId = "SpanFullDetail" Then
            Exit Function
        Else
            Dim Qset As String = qSetId
            Dim strSql As String = "select qs.QSet_Name ,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id= '" & Qset.CleanSQL & "'"
            Dim db As New ClassConnectSql()
            Dim ds As DataTable
            ds = db.getdata(strSql)
            Dim qSetName As String = ""
            Dim qSetNameBeforeComplete As String
            Dim CategoryName As String = ""

            CategoryName = ds.Rows(0)("QCategory_Name")
            If Not IsNothing(ds) Then
                qSetNameBeforeComplete = ds.Rows(0)("QSet_Name").ToString()
                qSetName = qSetNameBeforeComplete
            End If


            Return qSetName
        End If

    End Function

    Private Sub btnback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        'If IsMaxOnet Then
        'If Session("IsOneSubjectMaxOnet") IsNot Nothing AndAlso Session("IsOneSubjectMaxOnet") = True Then
        '        'ถ้าเป็นโหมด maxonet แบบวิชาเดียวให้กลับไปที่หน้าแรกของ maxonet เลย
        '        Response.Redirect("..\PracticeMode_Pad\ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId)
        '    Else
        '        'If Session("SubjectAmount") > 1 Then
        '        '    Response.Redirect("../PracticeMode_Pad/ChooseSubject.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=9")
        '        'End If

        '        'If Session("LevelAmount") > 1 Then
        '        '    Response.Redirect("../PracticeMode_Pad/chooseClass.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=9")
        '        'End If

        '        If ClsPracticeMode.CheckOneSubjectMaxOnet(Student Then

        '        End If
        '        Else
        '    Response.Redirect("../PracticeMode_Pad/ChooseSubject.aspx")
        'End If

        Response.Redirect(BackUrl)
    End Sub

    <Services.WebMethod()>
    Public Shared Function GetAvatarPic(Item_Id As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If


        Dim js As New JavaScriptSerializer()
        Dim ClsPractice As New ClsPracticeMode(New ClassConnectSql)

        Dim StdWinner As String = ClsPractice.GetTestsetWinnerFromMode(Item_Id, 1)
        Dim imgWinner As String = ""
        Dim IsShowImgWinner As String
        If StdWinner <> "" Then
            IsShowImgWinner = True
            If System.IO.File.Exists(HttpContext.Current.Server.MapPath("../UserData/" & HttpContext.Current.Session("SchoolCode").ToString() & "/{" & StdWinner & "}/avt.png")) Then
                imgWinner = "../UserData/" & HttpContext.Current.Session("SchoolID").ToString() & "/{" & StdWinner & "}/avt.png"
            Else
                imgWinner = "../UserData/dummy.png"
            End If
        Else
            IsShowImgWinner = False
            imgWinner = ""
        End If

        Dim JsonString
        JsonString = New With {.IsShowImgWinner = IsShowImgWinner, .imgWinner = imgWinner}
        Return js.Serialize(JsonString)

    End Function

End Class