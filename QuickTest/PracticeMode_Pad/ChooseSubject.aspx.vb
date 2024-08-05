Imports BusinessTablet360
Imports KnowledgeUtils

Public Class ChooseSubject
    Inherits System.Web.UI.Page

    Dim _DB As New ClassConnectSql
    Public txtStep1, txtStep2, txtStep3, txtStep4, ChkFontSize, MarginSize, PaddingSize As String
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Public GroupName As String
    Public DVID As String
    Protected TokenId As String

    Public IE As String
    Protected IsMobile As Boolean

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected IsMaxOnet As Boolean

    Protected BackUrl As String = ""
    Protected NextUrl As String = ""

    Protected RedirectMode As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("PClassId") = "14a28f3d-1aff-429d-b7a1-927a28e010bd"
        Session("SchoolCode") = "1000001"
        IE = "1"
#End If
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString().ToLower()
        If AgentString.IndexOf("android") <> -1 OrElse AgentString.IndexOf("iphone") <> -1 OrElse AgentString.IndexOf("ipad") <> -1 Then
            IsMobile = True
        End If

        IsMaxOnet = ClsKNSession.IsMaxONet

        DVID = Request.QueryString("deviceuniqueid")

        If IsMaxOnet Then
            If Session("ChooseMode") Is Nothing Then
                Session("ChooseMode") = Request.QueryString("DashboardMode")
            End If
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
            End If
        End If


        CreateNav()
        CreateSubjectButton(Session("IsTeacher"))

        initBackUrl()


    End Sub
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
    Private Sub CreateSubjectButton(Isteacher As String)

        Dim MainPanel As New Panel
        With MainPanel
            .ID = "MainPanel"
            .ClientIDMode = UI.ClientIDMode.Static
            .Style.Add("margin-left", "auto")
            .Style.Add("margin-right", "auto")
            .Style.Add("width", "760px")
            '.Style.Add("margin-top", "65px")
            .Style.Add("padding-left", "50px")
            .Style.Add("padding-right", "50px")
            .Style.Add("padding-bottom", "50px")
            .Style.Add("background-color", "White")
            .Style.Add("border-radius", "10px")
            .Style.Add("text-align", "center")
        End With
        Dim SubjectPanel As New Panel
        With SubjectPanel
            .ID = "SecondPanel"
            .Style.Add("background-color", "#D3F2F7")
            .Style.Add("border-radius", "5px")
            .Style.Add("padding-bottom", "10px")
        End With

        Dim Headlabel As New Label
        With Headlabel
            .Text = "เลือกวิชา"
            .ForeColor = Drawing.Color.Orange
        End With

        MainPanel.Controls.Add(Headlabel)

        If IsMaxOnet Then
            Dim className As String = _DB.ExecuteScalar("Select Level_ShortName FROM tblLevel WHERE Level_Id = '" & Session("PClassId").ToString() & "';")
            Dim lblClass As New Label With {.Text = className, .CssClass = "lblMaxOnet"}
            MainPanel.Controls.Add(lblClass)
        End If

        Dim ClassId As String = Session("PClassId").ToString() 'เก็บค่า ClassId จาก Session
        Dim SchoolId As String = Session("SchoolCode").ToString() 'เก็บค่า SchoolId จาก Session

        ' เพิ่มว่า ถ้า เป็น maxonet ให้ query อีกแบบนึง
        Dim dtSubject As DataTable = If((IsMaxOnet), GetSubjectIdMaxOnet(), GetSubjectIdFromClassId(ClassId, SchoolId, Isteacher)) 'หาวิชาว่ามีวิชาอะไรบ้างจาก รหัสโรงเรียน และ รหัสวิชา
        If dtSubject.Rows.Count > 0 Then

            If dtSubject.Rows.Count = 1 Then
                Session("PSubjectName") = dtSubject.Rows(0)("GroupSubject_Id").ToString()
                RedirectMode = 3
                RedirectWhenBtnSubjectClick()
            Else

                Dim RowAmount = Math.Ceiling((dtSubject.Rows.Count) / 4)

                Dim FirstPosition = 0
                Dim CurrentPosition = 0
                Dim SubjectAddNo As Integer = 0
                'วนตามจำนวนบรรทัด
                For i = 1 To RowAmount

                    Dim PanelBtn As New Panel
                    Dim PanelPic As New Panel

                    'วนตามจำนวนวิชา
                    For j = SubjectAddNo To dtSubject.Rows.Count - 1

                        If SubjectAddNo = (i + (i * 3)) Then
                            Exit For
                        End If

                        Dim EachSubject As String
                        Dim GroupSubjectId As String = ""

                        EachSubject = dtSubject.Rows(j)("GroupSubject_ShortName").ToString()
                        GroupSubjectId = dtSubject.Rows(j)("GroupSubject_Id").ToString() 'เก็บค่า Id เพื่อเอาไปแทนค่าให้ session

                        Dim SubjectButton As Button
                        If dtSubject.Rows(j)("QuestionAmount").ToString() <> "" Then
                            SubjectButton = BtnSubject(GroupSubjectId, EachSubject)
                        Else
                            SubjectButton = BtnSubject(GroupSubjectId, EachSubject, True)
                        End If
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'ทำรูป
                        Dim ImgClass As ImageButton
                        If dtSubject.Rows(j)("QuestionAmount").ToString() <> "" Then
                            ImgClass = BtnImageSubject(GroupSubjectId, EachSubject)
                        Else
                            ImgClass = BtnImageSubject(GroupSubjectId, EachSubject, True)
                        End If


                        PanelBtn.Controls.Add(SubjectButton)
                        PanelPic.Controls.Add(ImgClass)

                        SubjectAddNo += 1

                    Next

                    SubjectPanel.Controls.Add(PanelPic)
                    SubjectPanel.Controls.Add(PanelBtn)
                Next


                Dim BackButton As New Button
                With BackButton
                    .ID = "BackButton"
                    .ClientIDMode = UI.ClientIDMode.Static
                    .ToolTip = "ย้อนกลับ"
                    .Style.Add("position", "relative")
                    .Style.Add("width", "70px")
                    .Style.Add("height", "70px")
                End With
                AddHandler BackButton.Click, AddressOf Me.BackButtonClick

                ' สร้าง div เพื่อไว้ lock button ให้ติดซ้าย
                Dim panelBackBtn As New Panel
                panelBackBtn.CssClass = "divBackBtn"
                panelBackBtn.Controls.Add(BackButton)
                MainDiv.Controls.Add(panelBackBtn)

                MainPanel.Controls.Add(SubjectPanel)
                MainDiv.Controls.Add(MainPanel)


            End If

        Else
            Dim NewLabel As New Label
            With NewLabel
                .Text = "ชั้นเรียนนี้ไม่มีวิชาที่สามารถใช้งานได้ค่ะ"
                .Style.Add("font-size", "46px")
                .Style.Add("margin-left", "100px")
                .Style.Add("color", "orange")
            End With

            Dim BackButton As Button = BtnBack()

            SubjectPanel.Controls.Add(NewLabel)
            MainPanel.Controls.Add(SubjectPanel)
            MainPanel.Controls.Add(BackButton)
            MainDiv.Controls.Add(MainPanel)
            'Form.Controls.Add(MainPanel)
        End If

    End Sub

    ''' <summary>
    ''' function สร้างปุ่ม พร้อมกับชื่อวิชา
    ''' </summary>
    ''' <returns></returns>
    Private Function BtnSubject(groupSubjectId As String, subjectName As String, Optional disable As Boolean = False) As Button
        Dim btn As New Button
        With btn
            .ID = groupSubjectId
            .Text = subjectName
            If IsMobile = True Then
                .Style.Add("width", "150px")
                .Style.Add("height", "80px")
                .Style.Add("margin", "15px 20px 15px")
                .Style.Add("line-height", "80px")
                .Style.Add("font-size", "25px")
            Else
                .Style.Add("width", "140px")
                .Style.Add("height", "40px")
                .Style.Add("margin", "0 20px 10px 20px")
                .Style.Add("line-height", "40px")
            End If
            .Style.Add("border-radius", "10px")
            .Style.Add("color", "white")
            .Attributes.Add("subjectId", groupSubjectId)

            If disable Then
                .Style.Add("opacity", "0.5")
                .CssClass = "ForDisablebtn"
            Else
                .CssClass = "Forbtn"
                AddHandler btn.Click, AddressOf Me.SubjectButtonClick
            End If
            .ToolTip = subjectName
        End With
        Return btn
    End Function

    ''' <summary>
    ''' function สร้าง button image subject
    ''' </summary>
    ''' <param name="groupSubjectId"></param>
    ''' <param name="subjectName"></param>
    ''' <param name="disable"></param>
    ''' <returns></returns>
    Private Function BtnImageSubject(groupSubjectId As String, subjectName As String, Optional disable As Boolean = False) As ImageButton
        Dim imgBtn As New ImageButton
        With imgBtn
            .ID = "Img" & groupSubjectId
            .ImageUrl = GetSubjectPathImageButton(groupSubjectId)
            .Style.Add("width", "70px")
            If IsMobile = True Then
                .Style.Add("margin-left", "60px")
                .Style.Add("margin-right", "60px")
            Else
                .Style.Add("margin-left", "55px")
                .Style.Add("margin-right", "55px")
            End If
            .Style.Add("margin-top", "15px")
            .CssClass = "ImgBtnSubject"
            .Attributes.Add("subjectId", groupSubjectId)
            If disable Then
                .Style.Add("opacity", "0.3")
            Else
                AddHandler imgBtn.Click, AddressOf Me.SubjectImageButtonClick
            End If
        End With
        Return imgBtn
    End Function
    Private Function BtnBack() As Button
        Dim btn As New Button
        With btn
            .ID = "BackButton"
            .ClientIDMode = UI.ClientIDMode.Static
            .ToolTip = "ย้อนกลับ"
            '.Text = "กลับ"
            '.CssClass = "Forbtn"
            .Style.Add("position", "relative")
            .Style.Add("width", "70px")
            .Style.Add("height", "70px")
            '.Style.Add("line-height", "40px")
        End With
        AddHandler btn.Click, AddressOf Me.BackButtonClick
        Return btn
    End Function

    ''' <summary>
    ''' หา path image ให้กับ imagebutton
    ''' </summary>
    ''' <param name="subjectId">ชื่อวิชาที่ต้องการหารูปภาพ</param>
    ''' 
    Private Function GetSubjectPathImageButton(subjectId As String) As String
        Dim mainPath As String = If((IsMaxOnet), "~/Images/MaxOnet/Subject/", "~/Images/Activity/Subject/")
        Select Case subjectId.ToUpper
            Case CommonSubjectsText.ThaiSubjectId
                Return String.Format("{0}Thai.png", mainPath)
            Case CommonSubjectsText.MathSubjectId
                Return String.Format("{0}Math.png", mainPath)
            Case CommonSubjectsText.HomeSubjectId
                Return String.Format("{0}home.png", mainPath)
            Case CommonSubjectsText.SocialSubjectId
                Return String.Format("{0}Social.png", mainPath)
            Case CommonSubjectsText.EnglishSubjectId
                Return String.Format("{0}Eng.png", mainPath)
            Case CommonSubjectsText.ArtSubjectId
                Return String.Format("{0}Art.png", mainPath)
            Case CommonSubjectsText.HealthSubjectId
                Return String.Format("{0}Suk.png", mainPath)
            Case CommonSubjectsText.ScienceSubjectId
                Return String.Format("{0}Science.png", mainPath)
            Case CommonSubjectsText.SkillSubjectId
                Return String.Format("{0}01.png", mainPath)
            Case CommonSubjectsText.ReliSubjectId
                Return String.Format("{0}21.png", mainPath)
            Case CommonSubjectsText.SelfSubjectId
                Return String.Format("{0}22.png", mainPath)
            Case Else
                Return String.Format("{0}Pisa.png", mainPath) 'PISA
        End Select
    End Function
    Protected Sub SubjectButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim newBtn As New Button
        newBtn = sender
        Session("PSubjectName") = newBtn.ID
        RedirectMode = "4"
        RedirectWhenBtnSubjectClick()
    End Sub
    Protected Sub SubjectImageButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim newBtn As New ImageButton
        newBtn = sender
        Dim ReplaceStrId As String = newBtn.ID.Replace("Img", "")
        Session("PSubjectName") = ReplaceStrId
        RedirectMode = "4"
        RedirectWhenBtnSubjectClick()
    End Sub
    Private Sub RedirectWhenBtnSubjectClick()
        If IsMaxOnet Then
            Response.Redirect("ChooseQuestionset.aspx?deviceuniqueid=" & DVID & "&token=" & TokenId & "&DashboardMode=" & Request.QueryString("DashboardMode") & "&RedirectMode=" & RedirectMode)
        Else
            Response.Redirect("ChooseQuestionset.aspx")
        End If
    End Sub
    Protected Sub BackButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        If IsMaxOnet Then
            If Session("IsOneSubjectMaxOnet") Then
                Response.Redirect("ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId)
            Else
                If Session("LevelAmount") > 1 Then
                    Response.Redirect("ChooseClass.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=" & Request.QueryString("DashboardMode") & "&RedirectMode=" & RedirectMode)
                Else
                    Response.Redirect("ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId)
                End If
            End If
        Else
            Response.Redirect("ChooseClass.aspx?DashboardMode=" & Session("ChooseMode"))
        End If
    End Sub
    Private Function ChangeStrClass(ByVal StrClass As String) 'Function แปลงรหัสชั้น - เป็นชื่อชั้น
        Dim ReturnStr As String = ""
        If StrClass Is Nothing Or StrClass = "" Then
            Return ReturnStr
        End If

        Select Case StrClass
            Case "อ.1"
                ReturnStr = "k1"
            Case "อ.2"
                ReturnStr = "k2"
            Case "อ.3"
                ReturnStr = "k3"
            Case "ป.1"
                ReturnStr = "k4"
            Case "ป.2"
                ReturnStr = "k5"
            Case "ป.3"
                ReturnStr = "k6"
            Case "ป.4"
                ReturnStr = "k7"
            Case "ป.5"
                ReturnStr = "k8"
            Case "ป.6"
                ReturnStr = "k9"
            Case "ม.1"
                ReturnStr = "k10"
            Case "ม.2"
                ReturnStr = "k11"
            Case "ม.3"
                ReturnStr = "k12"
            Case "ม.4"
                ReturnStr = "k13"
            Case "ม.5"
                ReturnStr = "k14"
            Case "ม.6"
                ReturnStr = "k15"
            Case Else
                ReturnStr = ""
        End Select

        Return ReturnStr

    End Function
    Private Function GetSubjectIdMaxOnet() As DataTable
        Dim levelId As String = Session("PClassId").ToString()
        Dim sql As String = ""
        sql = "SELECT distinct g.GroupSubject_Id,g.GroupSubject_Name,g.GroupSubject_ShortName,500 AS QuestionAmount,g.GroupSubject_Id as CheckEnableSubject
                From maxonet_tblStudentSubject s INNER JOIN tblGroupSubject g ON s.SS_SubjectId = g.GroupSubject_Id  
                inner join maxonet_tblKeyCodeUsage kcu on s.SS_KeyCode = kcu.KeyCode_Code
                WHERE s.SS_StudentId = '" & Session("userid").ToString().CleanSQL & "' 
                AND SS_LevelId = '" & levelId & "' AND SS_Isactive = 1  and KCU_IsActive = 1
                and (kcu.KCU_ExpireDate > dbo.GetThaiDate() or kcu.KCU_ExpireDate is null)  and (KCU_Type = 0 or KCU_Type = 3 or KCU_Type = 4);
"
        Dim dt As DataTable = _DB.getdata(sql)
        sortSubjectMaxOnet(dt)
        Return dt
    End Function

    ''' <summary>
    ''' function ในการ sort วิชาตามลำดับของ maxonet thai,eng,social,math,science
    ''' </summary>
    ''' <param name="dt"></param>
    Private Sub sortSubjectMaxOnet(ByRef dt As DataTable)
        Dim col As New DataColumn
        col.DataType = GetType(Integer)
        col.AllowDBNull = False
        col.Caption = "Order"
        col.ColumnName = "Order"
        col.DefaultValue = 0
        dt.Columns.Add(col)

        For Each r In dt.Rows
            If r("GroupSubject_Id").ToString().ToLower() = CommonSubjectsText.ThaiSubjectId.ToLower() Then
                r("Order") = 1
            ElseIf r("GroupSubject_Id").ToString().ToLower() = CommonSubjectsText.EnglishSubjectId.ToLower() Then
                r("Order") = 2
            ElseIf r("GroupSubject_Id").ToString().ToLower() = CommonSubjectsText.SocialSubjectId.ToLower() Then
                r("Order") = 3
            ElseIf r("GroupSubject_Id").ToString().ToLower() = CommonSubjectsText.MathSubjectId.ToLower() Then
                r("Order") = 4
            ElseIf r("GroupSubject_Id").ToString().ToLower() = CommonSubjectsText.ScienceSubjectId.ToLower() Then
                r("Order") = 5
            Else
                r("Order") = 6
            End If
        Next

        Dim View As New DataView(dt)
        View.Sort = "Order ASC"

        dt = View.ToTable()
    End Sub

    Private Function GetSubjectIdFromClassId(ByVal LevelId As String, ByVal SchoolId As String, IsTeacher As String) 'Function หาวิชาที่สามารถใช้ได้ โดยใช้ รหัสโรงเรียน - รหัสวิชา 

        Dim dtSubject As New DataTable
        If LevelId Is Nothing Or LevelId = "" Or SchoolId Is Nothing Or SchoolId = "" Then
            Return dtSubject
        End If

        Dim sql As New StringBuilder

        sql.Append("SELECT DISTINCT b.GroupSubject_Id,b.GroupSubject_Name,a.QuestionAmount,a.GroupSubject_Id as CheckEnableSubject
                    ,b.GroupSubject_ShortName FROM dbo.uvw_PracticeMode_GetSubjectFromSchoolAndClass b left join ( 
				SELECT count(tblquestion.Question_Id) as QuestionAmount, Level_Id,GroupSubject_Id 
				FROM tblbook left join tblQuestionCategory on tblQuestionCategory.Book_Id = tblbook.BookGroup_Id  
			 	left join tblquestionset on tblQuestionCategory.QCategory_Id = tblquestionset.QCategory_Id 
				left join tblquestion on tblQuestionset.QSet_Id = tblQuestion.QSet_Id 
				where Book_Syllabus = '51' and tblbook.IsActive = 1 
				group by Level_Id,GroupSubject_Id)a 
                on a.Level_Id = b.Level_Id and a.GroupSubject_Id = b.GroupSubject_Id 
                WHERE SchoolId = '" & SchoolId.CleanSQL & "' AND b.Level_Id = '" & LevelId.CleanSQL & "' and a.GroupSubject_Id <> 'FB677859-87DA-4D8D-A61E-8A76566D69D8';;")

        dtSubject = _DB.getdata(sql.ToString)

        Return dtSubject

    End Function

    Private Sub initBackUrl()
        If IsMaxOnet Then
            If Session("IsOneSubjectMaxOnet") Then
                Me.BackUrl = "ChooseTestsetMaxonet.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId
            Else
                Me.BackUrl = "ChooseClass.aspx?deviceUniqueId=" & DVID & "&token=" & TokenId & "&DashboardMode=" & Session("ChooseMode")
            End If
            Me.NextUrl = "ChooseQuestionSet.aspx?deviceuniqueid=" & DVID & "&token=" & TokenId
        Else
            Me.BackUrl = "ChooseClass.aspx?DashboardMode=" & Session("ChooseMode")
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


End Class
