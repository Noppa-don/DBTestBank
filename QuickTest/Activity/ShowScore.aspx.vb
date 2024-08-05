Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports System.Web

Public Class ShowScore
    Inherits System.Web.UI.Page

    Public AvgVal As String
    Public MyScoreVal As String
    Public MaxScoreVal As String

    Public Img As String

    Public AvgScore As Decimal = 0
    Public MyScore As Decimal = 0
    Public MaxScore As Decimal = 0
    Public FullScore As Decimal = 0
    Public NextPage As String

    Public ModeOfShowScore As String

    Public ClsActivity As New ClsActivity(New ClassConnectSql)
    Public ClsPractice As New ClsPracticeMode(New ClassConnectSql)

    Private Quiz_Id As String
    Public PlayerId As String

    Public HeightLeftAvt As String
    Public WidthLeftAvt As String
    Public HeightRightAvt As String
    Public WidthRighttAvt As String

    Public MarginLLeftAvt As String
    Public MarginTLeftAvt As String

    Public MarginLRightAvt As String
    Public MarginTRightAvt As String

    Public MarginLTxtAvt As String
    Public MarginTTxtAvt As String

    Public imgLeft As String
    Public imgRight As String

    Public Wintext As String

    Dim _DB As New ClassConnectSql()

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected IsMaxOnet As Boolean
    Protected TokenId As String

    Protected IsMobile As Boolean

    Protected NextUrl As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString().ToLower()
        If AgentString.IndexOf("android") <> -1 OrElse AgentString.IndexOf("iphone") <> -1 OrElse AgentString.IndexOf("ipad") <> -1 Then
            IsMobile = True
        End If

        IsMaxOnet = ClsKNSession.IsMaxONet
        If IsMaxOnet Then TokenId = Request.QueryString("token")


        If Session("UserId") Is Nothing Then
            If IsMaxOnet Then
                Response.Redirect(String.Format("..\PracticeMode_Pad\ChooseTestsetMaxOnet.aspx?deviceUniqueId={0}&token={1}", Request.QueryString("deviceuniqueid"), TokenId))
            End If
            Exit Sub
        End If

        'Open Connection
        Dim connShowScore As New SqlConnection
        _DB.OpenExclusiveConnect(connShowScore)

        'คะแนนเต็ม 20 สูงสุดได้ 19 ทำได้ 16 เฉลี่ย 12
        Dim DVID As String
        'Dim Player_Id As String
        Dim ScoreOfQuiz As Integer = 0

        'If Request.QueryString("QuizId").ToString() = "Nothing" Then
        '    Player_Id = HttpContext.Current.Application("DefaultUserId")
        'Else
        '    DVID = Request.QueryString("DeviceUniqueID").ToString()
        '    Player_Id = GetPlayerIDFromSerial(DVID.ToString).ToUpper()
        'End If

        ' Dim ModeOfShowScore As String = "" 'จำนวนแท่ง

        ' แก้ด้านบนครับ
        If Request.QueryString("QuizId") IsNot Nothing Then
            Quiz_Id = Request.QueryString("QuizId").ToString()
        End If

        If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then 'เป็น Userid ที่เอาจาก Webconfig
            PlayerId = Session("UserId").ToString
            ModeOfShowScore = "1"
        Else
            If Request.QueryString("DeviceUniqueID") IsNot Nothing Then
                DVID = Request.QueryString("DeviceUniqueID").ToString()
                PlayerId = GetPlayerIDFromSerial(DVID.ToString, Quiz_Id, connShowScore).ToUpper()
                ModeOfShowScore = "3"

                ' จบ quiz remove application ออกให้หมด
                HttpContext.Current.Application.Remove(DVID & "|" & "IsUpdateCheckTablet")
                HttpContext.Current.Application.Remove(DVID & "|" & "QuizId")
                HttpContext.Current.Application.Remove(DVID & "|" & "_ExmanNum")
                HttpContext.Current.Application.Remove(DVID & "|" & "CurrentAnsState")

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

            'update ด้วยว่า เข้ามาทำกิจกรรมประจำวันแล้ว
            If IsMaxOnet AndAlso HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Homework Then
                ModeOfShowScore = "1" ' เนื่องด้วยเป็นกิจกรรมประจำวัน แต่ละคนได้ชุดข้อสอบไม่เหมือนกัน จึงไม่ต้องมีคะแนนเทียบกับคนอื่นๆ
                Img = "../images/Activity/ShowScore/Myscore.png"
                Dim ClsHomework As New ClsHomework(New ClassConnectSql)
                Dim ClsActivity As New ClsActivity(New ClassConnectSql)
                Dim ReplyAmount = ClsActivity.CountLeapExam(Quiz_Id, PlayerId)
                If ReplyAmount = "0" Then
                    ClsHomework.UpdateExitByUser(Quiz_Id, PlayerId, "3")
                Else
                    ClsHomework.UpdateExitByUser(Quiz_Id, PlayerId, "2")
                End If
            End If
        End If

#If IE = "1" Then
        Session("ChooseMode") = EnumDashBoardType.Quiz
        Quiz_Id = "DF8055A0-283E-4EEE-8143-B853C1515D29"
        DVID = "eafca792-230c-4ad8-ab5e-0ab02b62a4d1"
        PlayerId = "4DD03F6C-7188-4D1A-863F-112A90C6A7C6"
        ModeOfShowScore = "3"
#End If

        'ต้องมี playerId เท่านั้น ถึงจะ Gen PanelReward (เพราะอาจใช้ tablet ห้อง Lab มาฝึกฝน จะไม่มี PlayerId จะทำให้พังได้) + เพิ่ม ถ้าไม่ใช่ quiz ก็จะต้องไม่สร้าง panel ป้ายรางวัลด้วย
        '2018/04/11 กรณี Maxonet ให้ปิด PanelReward ไป Req. คือ ไม่ให้มีอะไรเกี่ยวกับการเล่นเกมเลย

        'If PlayerId <> "" AndAlso HttpContext.Current.Session("ChooseMode") IsNot Nothing AndAlso HttpContext.Current.Session("ChooseMode") <> EnumDashBoardType.Quiz Then
        If PlayerId <> "" AndAlso HttpContext.Current.Session("ChooseMode") IsNot Nothing AndAlso HttpContext.Current.Session("ChooseMode") <> EnumDashBoardType.Quiz AndAlso Not IsMaxOnet Then
            GenPanelReward(connShowScore)
        Else
            'ถ้าไม่ Gen PanelReward ต้องซ่อน div ดำๆที่ขึ้นมาขึงตอน Page_Load
            DivBlock.Style.Add("display", "none")
        End If

        Dim dtScore As DataTable = ClsActivity.GetScoreOfQuiz(Quiz_Id, HttpContext.Current.Session("ChooseMode"), connShowScore)

        'Dim atest As Integer = CInt(dt.Compute("AVG(MySc)", ""))

        '  TotalScore = ClsActivity.GetTotalScore(Quiz_Id)

        FullScore = dtScore.Rows(0)("FullScore")

        For Each i In dtScore.Rows

            If i("Quiz_Id").ToString = Quiz_Id Then
                If i("Player_Id").ToString.ToUpper() = PlayerId.ToUpper() Then
                    MyScore = i("TotalScore")
                End If
            End If

            If i("TotalScore") > MaxScore Then
                MaxScore = i("TotalScore")
            End If

            ScoreOfQuiz += CInt(i("TotalScore"))

        Next

        MyScoreVal = System.Math.Round(((MyScore * 100) / FullScore) * 2, 0)
        MyScoreVal = MyScoreVal + 60 & "px"

        If ModeOfShowScore = "3" Then

            AvgScore = FormatNumber(ScoreOfQuiz / dtScore.Rows.Count, 2) 'คะแนนรวมของทุกคนเอามาเฉลี่ย
            AvgVal = System.Math.Round(((AvgScore * 100) / FullScore) * 2, 0)
            AvgVal = AvgVal + 60 & "px"

            'Select max(sum(score)) from tblQuizScore where Quiz_id and User_ID
            MaxScoreVal = System.Math.Round(((MaxScore * 100) / FullScore) * 2, 0)
            MaxScoreVal = MaxScoreVal + 60 & "px"

            If AvgScore > MyScore Then
                Img = "../images/Activity/ShowScore/Myscore.png"
            Else
                Img = "../images/Activity/ShowScore/Myscore.png"
            End If

        End If

        ' ปรับคะแนนให้ตรงตาม format
        AvgScore = AvgScore.ToString().ToPointplusScore()
        MyScore = MyScore.ToString().ToPointplusScore()
        MaxScore = MaxScore.ToString().ToPointplusScore()
        FullScore = FullScore.ToString().ToPointplusScore()

        If Not Page.IsPostBack Then
            SetWinnerPanel(connShowScore)
        End If


        'Close Connection
        _DB.CloseExclusiveConnect(connShowScore)

        NextUrl = GetBackURL()
    End Sub

    Private Function GetBackURL() As String
        If IsMaxOnet Then
            Return "../PracticeMode_Pad/ChooseTestsetMaxonet.aspx?deviceUniqueId=" & Request.QueryString("DeviceUniqueID").ToString() & "&token=" & TokenId
        Else
            If Session("PracticeMode") Or (Session("ChooseMode") = EnumDashBoardType.Practice) Or (Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
                ' comment ทิ้งไปก่อน เรนื่องจาก query หน้านี้ช้ามาก เพราะเกี่ยวกับเรื่องดัชนีชี้วัด ไหมบอกว่ากำลังจะมีการปรับแก้
                'Response.Redirect("..\PracticeMode_Pad\RecommendPage.aspx?QuizId=" & Quiz_Id.ToString)
                Return "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & Request.QueryString("DeviceUniqueID").ToString()
            Else
                Return "../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & Request.QueryString("DeviceUniqueID").ToString()
            End If
        End If
    End Function

    Protected Sub btnNextPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextPage.Click
        'If IsMaxOnet Then
        '    Response.Redirect("..\PracticeMode_Pad\ChooseTestsetMaxonet.aspx?deviceUniqueId=" & Request.QueryString("DeviceUniqueID").ToString() & "&token=" & TokenId)
        'Else
        '    If Session("PracticeMode") Or (Session("ChooseMode") = EnumDashBoardType.Practice) Or (Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer) Then
        '        ' comment ทิ้งไปก่อน เรนื่องจาก query หน้านี้ช้ามาก เพราะเกี่ยวกับเรื่องดัชนีชี้วัด ไหมบอกว่ากำลังจะมีการปรับแก้
        '        'Response.Redirect("..\PracticeMode_Pad\RecommendPage.aspx?QuizId=" & Quiz_Id.ToString)
        '        Response.Redirect("..\PracticeMode_Pad\ChooseTestset.aspx?DeviceUniqueID=" & Request.QueryString("DeviceUniqueID").ToString())
        '    Else
        '        Response.Redirect("..\PracticeMode_Pad\ChooseTestset.aspx?DeviceUniqueID=" & Request.QueryString("DeviceUniqueID").ToString())
        '    End If
        'End If
        Response.Redirect(NextUrl)
    End Sub


    Private Function GetPlayerIdFromQuizId(ByVal Quiz_Id As String) As String
        Dim sql As String
        sql = " select Player_id from tblQuizSession where Quiz_Id = '" & Quiz_Id & "'"
        Dim _DB As New ClassConnectSql()
        GetPlayerIdFromQuizId = _DB.ExecuteScalar(sql)

    End Function


    Private Function GetPlayerIDFromSerial(ByVal Serial As String, ByVal Quiz_Id As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim _DB As New ClassConnectSql()
        Dim PlayerId As String
        Dim sql As String
        sql = " select owner_Id from t360_tblTabletOwner where Tablet_Id = " &
              " (select Tablet_Id from t360_tblTablet where Tablet_SerialNumber = '" & Serial & "') AND TabletOwner_IsActive = 1 "
        PlayerId = _DB.ExecuteScalar(sql, InputConn)

        If PlayerId = "" Then
            sql = " SELECT     tblQuizSession.Player_Id" &
                  " FROM tblQuizSession INNER JOIN " &
                  " t360_tblTablet ON tblQuizSession.Tablet_Id = t360_tblTablet.Tablet_Id " &
                  " where t360_tblTablet.Tablet_SerialNumber = '" & Serial & "' " &
                  " and Quiz_Id = '" & Quiz_Id & "'"
            PlayerId = _DB.ExecuteScalar(sql, InputConn)
        End If

        Return PlayerId

    End Function

    Private Sub SetWinnerPanel(Optional ByRef InputConn As SqlConnection = Nothing)

        Dim KnSession As New KNAppSession()
        Dim Calendar_ID As String = KnSession.StoredValue("CurrentCalendarId").ToString

        Dim ArrReturnData = Split(ClsPractice.GetAvartaForQuiz(Quiz_Id, Calendar_ID, MyScore, PlayerId, Session("SchoolID").ToString(), InputConn), "%")

        Dim StatusPoint = ArrReturnData(0).ToString

        Dim rdn As New System.Random
        Dim Maxrdn As String
        Dim ArrWinTxt() As String

        '1=win,2=same,3=new,4=lose

        If StatusPoint = 1 Then
            ArrWinTxt = {"แต้มเยอะกว่า", "คะแนนสูงกว่า", "ตอบถูกมากกว่า", "เข้าใจดีกว่า"}
            Maxrdn = 3
            HeightLeftAvt = "100px"
            WidthLeftAvt = "60px"
            MarginLLeftAvt = "16px"
            MarginTLeftAvt = "15px"

            HeightRightAvt = "60px"
            WidthRighttAvt = "40px"
            MarginLRightAvt = "223px"
            MarginTRightAvt = "55px"

            MarginLTxtAvt = "97px"
            MarginTTxtAvt = "14px"

            ImgMyAvatar.Attributes.Add("class", "winner")
            ImgOtherAvatar.Attributes.Add("class", "loser")

        ElseIf StatusPoint = 2 Then
            ArrWinTxt = {"เท่ากันพอดี", "คะแนนเท่ากันเป๊ะ", "เก่งเท่าๆกัน", "ฉลาดพอกัน", "แต้มเท่ากันเลย"}
            Maxrdn = 4
            HeightLeftAvt = "100px"
            WidthLeftAvt = "60px"
            MarginLLeftAvt = "16px"
            MarginTLeftAvt = "15px"

            HeightRightAvt = "60px"
            WidthRighttAvt = "40px"
            MarginLRightAvt = "223px"
            MarginTRightAvt = "55px"

            MarginLTxtAvt = "97px"
            MarginTTxtAvt = "14px"

            ImgMyAvatar.Attributes.Add("class", "winner")
            ImgOtherAvatar.Attributes.Add("class", "loser")

        ElseIf StatusPoint = 3 Then
            ArrWinTxt = {"ทำก่อนใคร", "มาไวที่หนึ่ง", "เก่งฉับไว", "ขยันที่สุด"}
            Maxrdn = 3
            HeightLeftAvt = "100px"
            WidthLeftAvt = "66px"
            MarginLLeftAvt = "16px"
            MarginTLeftAvt = "15px"

            HeightRightAvt = "0px"
            WidthRighttAvt = "0px"
            MarginLRightAvt = "0px"
            MarginTRightAvt = "0px"

            MarginLTxtAvt = "97px"
            MarginTTxtAvt = "14px"

            ImgMyAvatar.Attributes.Add("class", "winner")
            ImgOtherAvatar.Width = 0

        ElseIf StatusPoint = 4 Then
            ArrWinTxt = {"ตอบผิดเยอะไป", "ตอบไม่ถูกมากกว่า", "คะแนนน้อยกว่า", "แต้มสู้ไม่ถึง", "คะแนนยังน้อยอยู่", "คะแนนไม่ดีนะ"}
            Maxrdn = 5
            HeightLeftAvt = "60px"
            WidthLeftAvt = "40px"
            MarginLLeftAvt = "16px"
            MarginTLeftAvt = "55px"

            HeightRightAvt = "100px"
            WidthRighttAvt = "66px"
            MarginLRightAvt = "197px"
            MarginTRightAvt = "16px"

            MarginLTxtAvt = "80px"
            MarginTTxtAvt = "14px"


            ImgMyAvatar.Attributes.Add("class", "loser")
            ImgOtherAvatar.Attributes.Add("class", "winner")

        End If

        Dim rdnNum As String = rdn.Next(0, Maxrdn)

        Wintext = ArrWinTxt(rdnNum).ToString

        If System.IO.File.Exists(Server.MapPath("../UserData/" & Session("SchoolID").ToString() & "/{" & PlayerId & "}/avt.png")) Then
            imgLeft = "../UserData/" & Session("SchoolID").ToString() & "/{" & PlayerId & "}/avt.png"
        Else
            imgLeft = If((ClsKNSession.IsMaxONet), "../UserData/dummy.png", "../UserData/newdummy.png")
        End If

        If System.IO.File.Exists(Server.MapPath("../UserData/" & Session("SchoolID").ToString() & "/{" & ArrReturnData(1) & "}/avt.png")) Then
            imgRight = "../UserData/" & Session("SchoolID").ToString() & "/{" & ArrReturnData(1) & "}/avt.png"
        Else
            imgRight = If((ClsKNSession.IsMaxONet), "../UserData/dummy.png", "../UserData/newdummy.png")
        End If

        ImgMyAvatar.Src = imgLeft
        ImgOtherAvatar.Src = imgRight

    End Sub

#Region "เปิดป้ายเพื่อรับรางวัล"

    '      <div class="ForDivMinigame" RewardId="Reward2">
    '          ป้ายที่ 2
    '      </div>
    '       <div id="Reward2" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 2
    '      </div>
    '      <div class="ForDivMinigame" RewardId="Reward3">
    '          ป้ายที่ 3
    '      </div>
    '       <div id="Reward3" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 3
    '      </div>
    '      <div class="ForDivMinigame" RewardId="Reward4">
    '          ป้ายที่ 4
    '      </div>
    '       <div id="Reward4" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 4
    '      </div>
    '        <div class="ForDivMinigame" RewardId="Reward5">
    '          ป้ายที่ 5
    '      </div>
    '        <div id="Reward5" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 5
    '      </div>
    '        <div class="ForDivMinigame" RewardId="Reward6">
    '          ป้ายที่ 6
    '      </div>
    '        <div id="Reward6" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 6
    '      </div>
    '        <div class="ForDivMinigame" RewardId="Reward7">
    '          ป้ายที่ 7
    '      </div>
    '       <div id="Reward7" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 7
    '      </div>
    '        <div class="ForDivMinigame" RewardId="Reward8">
    '          ป้ายที่ 8
    '      </div>
    '       <div id="Reward8" class="ForRewardMinigame" reward="gold|1" >
    '          ทอง 8
    '      </div>

    Private Sub GenHtmlPanelMinigame(ByVal ArrComplete As ArrayList)
        Dim sb As New StringBuilder()
        sb.Append("<div id='DivHeadMinigame'>รับรางวัล</div>")
        Dim lbltxt As String = "X"
        For i = 0 To ArrComplete.Count - 1
            sb.Append("<div class='ForDivMinigame' RewardId='Reward" & (i + 1) & "'>ป้ายที่ " & (i + 1) & "</div>")
            Dim spliteStr = ArrComplete(i).ToString().Split("|")
            Dim ImgPath As String = ""
            If spliteStr(0) = "gold" Then
                lbltxt = "X"
                ImgPath = If((ClsKNSession.IsMaxONet), "../images/Activity/Minigame/RewardGold.png", "../images/Activity/Minigame/newRewardGold.png")
            ElseIf spliteStr(0) = "silver" Then
                lbltxt = "X"
                ImgPath = If((ClsKNSession.IsMaxONet), "../images/Activity/Minigame/RewardSilver.png", "../images/Activity/Minigame/newRewardSilver.png")
            ElseIf spliteStr(0) = "diamond" Then
                lbltxt = "X"
                ImgPath = "../images/Activity/Minigame/RewardDiamond.png"
            ElseIf spliteStr(0) = "stop" Then
                lbltxt = ""
                ImgPath = If((ClsKNSession.IsMaxONet), "../images/Activity/Minigame/ghost-100.png", "../images/Activity/Minigame/newghost.png")
            End If
            'sb.Append("<div id='Reward" & (i + 1) & "' style='background-image:url(""" & ImgPath & """)' class='ForRewardMinigame' reward='" & ArrComplete(i) & "' >")
            sb.Append("<div id='Reward" & (i + 1) & "' class='ForRewardMinigame' reward='" & ArrComplete(i) & "' >")
            If spliteStr(0) = "stop" Then
                sb.Append("<img class='ForImgStop' src='" & ImgPath & "' />")
            Else
                sb.Append("<img class='ForImgReward' src='" & ImgPath & "' />")
            End If
            If spliteStr(0) = "stop" Then
                sb.Append("<span class='ForSpntxt'>" & lbltxt & "</span>")
            Else
                sb.Append("<span class='ForSpntxt'>" & lbltxt & spliteStr(1).ToString() & "</span>")
            End If

            sb.Append("</div>")
        Next
        MainDivMinigame.InnerHtml = sb.ToString()
    End Sub

    Private Sub GenPanelReward(Optional ByRef InputConn As SqlConnection = Nothing)
        'คำนวณเหรียญที่เด็กทำได้
        Dim StudentService As New StudentService()
        Dim dtGoldAndSilver As DataTable = StudentService.GetSilverAndGoldCoin(PlayerId, InputConn)
        Dim Gold As Integer = 1
        Dim Silver As Integer = 1
        If dtGoldAndSilver.Rows.Count > 0 Then
            If dtGoldAndSilver.Rows(0)("Gold") IsNot DBNull.Value Then
                Gold = dtGoldAndSilver.Rows(0)("Gold")
                If Gold = 0 Then
                    Gold = 1
                End If
            End If
            If dtGoldAndSilver.Rows(0)("Silver") IsNot DBNull.Value Then
                Silver = dtGoldAndSilver.Rows(0)("Silver")
                If Silver = 0 Then
                    Silver = 1
                End If
            End If
        End If
        'หาจำนวนของป้ายแต่ละอันว่ามีอันละกี่ป้าย
        Dim ArrTotalReward As ArrayList = GetArrRandomTotalReward()
        Dim ArrComplete As ArrayList = GetArrComplete(ArrTotalReward, Gold, Silver)
        GenHtmlPanelMinigame(ArrComplete)
    End Sub

    'จะคืน Array ที่มี index ตามนี้ คือ Arr(0) = จำนวนตัวหยุุด , Arr(1) = จำนวนเหรียญเงิน , Arr(2) = จำนวนเหรียญทอง , Arr(3) = จำนวนเพชร
    Private Function GetArrRandomTotalReward() As ArrayList
        Dim ArrReward As New ArrayList
        'Random ตัวหยุด มี 3-4 อัน
        Dim StopNumber As Integer = GetrandomNumber(3, 4)
        Dim SilverNumber As Integer = GetrandomNumber(2, 3)
        Dim GoldNumber As Integer = 0
        If (SilverNumber + StopNumber) = 7 Then
            GoldNumber = 1
        Else
            GoldNumber = GetrandomNumber(1, 2)
        End If
        Dim DiamondNumber As Integer = 0
        If (StopNumber + SilverNumber + GoldNumber) <> 8 Then
            DiamondNumber = 8 - (StopNumber + SilverNumber + GoldNumber)
        End If
        ArrReward.Add(StopNumber)
        ArrReward.Add(SilverNumber)
        ArrReward.Add(GoldNumber)
        ArrReward.Add(DiamondNumber)
        Return ArrReward
    End Function
    'Get Array ที่ Random ค่าออกมาเรียบร้อยแล้วเพื่อเอาไปเป็นป้ายรางวัล
    Private Function GetArrComplete(ByVal ArrTotalReward As ArrayList, ByVal Gold As Integer, ByVal Silver As Integer) As ArrayList
        Dim ArrBeforeShuffle As New ArrayList
        'Add ตัวหยุดก่อน
        For TotalStop = 0 To (ArrTotalReward(0) - 1)
            ArrBeforeShuffle.Add("stop|" & (TotalStop + 1))
        Next
        'Add Silver
        For rSilver = 0 To (ArrTotalReward(1) - 1)
            Dim silverValue As Integer = 0
            If rSilver = 0 Then
                silverValue = Math.Floor(Silver * 1)
                ArrBeforeShuffle.Add("silver|" & silverValue)
            ElseIf rSilver = 1 Then
                silverValue = Math.Floor(Silver * 1.5)
                ArrBeforeShuffle.Add("silver|" & silverValue)
            ElseIf rSilver = 2 Then
                silverValue = Math.Floor(Silver * 2.5)
                ArrBeforeShuffle.Add("silver|" & silverValue)
            End If
        Next
        'Add Gold
        For rGold = 0 To (ArrTotalReward(2) - 1)
            Dim GoldValue As Integer = 0
            If rGold = 0 Then
                GoldValue = Math.Floor(Gold * 1)
                ArrBeforeShuffle.Add("gold|" & GoldValue)
            ElseIf rGold = 1 Then
                GoldValue = Math.Floor(Gold * 1.5)
                ArrBeforeShuffle.Add("gold|" & GoldValue)
            End If
        Next
        'Add Diamond
        For rDiamond = 0 To (ArrTotalReward(3) - 1)
            Dim DiamondValue As Integer = GetrandomNumber(1, 3)
            ArrBeforeShuffle.Add("diamond|" & DiamondValue)
        Next

        Dim rdm As New Random()
        For cnt As Integer = 0 To ArrBeforeShuffle.Count - 1
            Dim tmp As Object = ArrBeforeShuffle(cnt)
            Dim idx As Integer = rdm.[Next](8)
            ArrBeforeShuffle(cnt) = ArrBeforeShuffle(idx)
            ArrBeforeShuffle(idx) = tmp
        Next
        Return ArrBeforeShuffle
    End Function

    Private Function GetrandomNumber(ByVal MinValue As Integer, ByVal MaxValue As Integer) As Integer
        Dim random As New Random(Now.Millisecond)
        Return random.Next(MinValue, (MaxValue + 1))
    End Function

    <Services.WebMethod()>
    Public Shared Function UpdateCoinBeforeMinigameEnd(ByVal StudentId As String, ByVal Gold As Integer, ByVal Silver As Integer, ByVal Diamond As Integer)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim js As New JavaScriptSerializer()
        Try
            Dim sql As String = " SELECT Silver,Gold FROM dbo.tblStudentPoint WHERE Student_Id = '" & StudentId & "' "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            Dim JsonString As New ArrayList
            If dt.Rows.Count > 0 Then
                JsonString.Add(New With {.Silver = CStr(dt.Rows(0)("Silver")), .Gold = CStr(dt.Rows(0)("Gold"))})
            Else
                sql = " INSERT INTO dbo.tblStudentPoint( StudentPoint_Id ,Student_Id ,Silver ,Gold ,Diamond ,TotalSilver ,TotalGold , " &
                      " TotalDiamond ,LastUpdate ,IsActive ,TotalScore ,Point_Level,School_Code) VALUES ( NEWID(), '" & StudentId & "' , 0 , 0 , 0 , 0 , 0 , 0 , dbo.GetThaiDate() ,1 , 0 , 1," & HttpContext.Current.Session("SchoolID").ToString() & ") "
                _DB.Execute(sql)
                JsonString.Add(New With {.Silver = Silver, .Gold = Gold})
            End If
            sql = " UPDATE dbo.tblStudentPoint SET Silver += " & Silver & ", Gold += " & Gold & ", Diamond += " & Diamond & ",TotalSilver += " & Silver & ",TotalGold += " & Gold & ",TotalDiamond += " & Diamond & " " &
                  " ,Lastupdate = dbo.GetThaiDate(), ClientId = Null WHERE Student_Id = '" & StudentId & "' "
            _DB.Execute(sql)
            Return js.Serialize(JsonString)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB = Nothing
            Return ""
        End Try
    End Function

#End Region


#Region "Bin Code"
    ' get silver ของ quiz
    'Private Shared Function GetSilver() As Integer
    '    sql.Clear()
    '    sql.Append(" SELECT COUNT(*) AS Silver FROM tblQuizScore WHERE Answer_Id IS NOT NULL AND Quiz_Id = '")
    '    sql.Append(Quiz_Id)
    '    sql.Append("' AND Student_Id = '")
    '    sql.Append(Player_Id)
    '    sql.Append("';")
    '    Dim _db As New ClassConnectSql()
    '    GetSilver = _db.ExecuteScalar(sql.ToString())
    'End Function

    ' มี row student point หรือยัง
    '<Services.WebMethod()>
    'Public Shared Function GetStudentPoint() As String
    '    sql.Clear()
    '    sql.Append(" SELECT TOP 1 * FROM tblStudentPoint WHERE Student_Id = '")
    '    sql.Append(Player_Id)
    '    sql.Append("';")
    '    Dim _db As New ClassConnectSql()
    '    Dim dtPoint As DataTable = _db.getdata(sql.ToString())

    '    Silver = GetSilver() ' เหรียญเงินที่ได้เพิ่ม
    '    Gold = CInt(MyScore) / 10 ' เหรียญทองที่ได้เพิ่ม


    '    Dim JsonString As New ArrayList
    '    If dtPoint.Rows.Count > 0 Then
    '        JsonString.Add(New With {.Silver = CStr(dtPoint.Rows(0)("Silver")), .Gold = CStr(dtPoint.Rows(0)("Gold")), .NewSilver = CStr(Silver), .NewGold = CStr(Gold)})
    '        IsHavePoint = True
    '    Else
    '        JsonString.Add(New With {.Silver = "0", .Gold = "0", .NewSilver = CStr(Silver), .NewGold = CStr(Gold)})
    '        IsHavePoint = False
    '    End If
    '    GetStudentPoint = js.Serialize(JsonString)
    'End Function

    '<Services.WebMethod()>
    'Public Shared Sub SetStudentPoint()
    '    sql.Clear()
    '    If IsHavePoint Then
    '        sql.Append(" UPDATE tblStudentPoint SET Silver += ")
    '        sql.Append(Silver)
    '        sql.Append(",Gold +=")
    '        sql.Append(Gold)
    '        sql.Append(",TotalSilver +=")
    '        sql.Append(Silver)
    '        sql.Append(",TotalGold +=")
    '        sql.Append(Gold)
    '        sql.Append(" WHERE Student_Id = '")
    '        sql.Append(Player_Id)
    '        sql.Append("';")
    '    Else
    '        sql.Append(" INSERT INTO tblStudentPoint (Student_Id,Silver,Gold,TotalSilver,TotalGold) VALUES ('")
    '        sql.Append(Player_Id)
    '        sql.Append("',")
    '        sql.Append(Silver)
    '        sql.Append(",")
    '        sql.Append(Gold)
    '        sql.Append(",")
    '        sql.Append(Silver)
    '        sql.Append(",")
    '        sql.Append(Gold)
    '        sql.Append(");")
    '    End If
    '    Dim _db As New ClassConnectSql()
    '    _db.Execute(sql.ToString())
    'End Sub
#End Region

End Class