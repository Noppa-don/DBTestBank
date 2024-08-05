Imports BusinessTablet360
Imports KnowledgeUtils
Imports System.Web

Public Class RecommendPage
    Inherits System.Web.UI.Page
    Dim ClsPractice As New Service.ClsPracticeMode(New ClassConnectSql)
    Dim Practice As New ClsPracticeMode(New ClassConnectSql)

    Public txtWithoutReccommend As String
    Public ShareQuiz_Id As String
    Public ShowDivRecommend As Boolean

    Protected IsAndroid As Boolean

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If

        Dim Quiz_Id As String
        'ใช้จริง()
        Quiz_Id = Request.QueryString("QuizId").ToString()
        ' ไม่มี
        'Quiz_Id = "90EF7DB4-4B45-4D30-BB46-4DA77652A96C"
        ' มี
        'Quiz_Id = "EC5FC7C5-46C1-4086-AA69-C01AD877BDE7"
        'ถูกหมด
        'Quiz_Id = "1C1AEB95-7677-4583-9351-AC9F9FB88BB7"
        'Session("PDeviceId") = "119"
        ShareQuiz_Id = Quiz_Id
        If Not Page.IsPostBack Then
            CreateList(0)
        End If

        If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice Then
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

    End Sub

    Public Sub CreateList(ByVal listType As String)
        Dim dtExam As DataTable
        If listType = 0 Then
            dtExam = ClsPractice.GetQsetEI(ShareQuiz_Id)
        Else
            dtExam = ClsPractice.GetQsetConcerned(ShareQuiz_Id)
        End If
        'Dim dtConcernedSet As DataTable = ClsPractice.GetQsetConcerned(ShareQuiz_Id.ToString)

        If ClsPractice.HaveReccommend(ShareQuiz_Id.ToString) Then

            'มี Reccommend มั้ย

            Dim ArrDetail As ArrayList = ClsPractice.GetEIName(ShareQuiz_Id.ToString)

            If ArrDetail.Count > 0 Then

                divWithoutReccommend.Visible = False
                'ทำ div ดัชนี
                CreateDivEvalution(ArrDetail)
                'ทำ list ข้อสอบ
                divReccommend.InnerHtml = ClsPractice.CreateTestUnitList(dtExam)
                ShowDivRecommend = True

            Else
                divWithoutReccommend.Visible = True
                ShowDivRecommend = False
                txtWithoutReccommend = "ลองฝึกทำข้อสอบอื่นเพิ่มเติมมั้ยคะ"
            End If
        Else
            divWithoutReccommend.Visible = True
            ShowDivRecommend = False
            txtWithoutReccommend = "ดีใจด้วยค่ะที่ได้คะแนนเต็ม ลองฝึกข้อสอบที่เกี่ยวข้องเพิ่มเติมมั้ยคะ"
        End If
    End Sub

    Private Sub CreateDivEvalution(ByVal DetailArray As ArrayList)
        Dim Sb As New StringBuilder
        For Each r In DetailArray
            Sb.Append("<div Class='ForSmallDetailDiv' style='background-color:#FFA032;'>")
            Sb.Append(r)
            Sb.Append("</div>")
        Next
        Sb.Append("<div style='clear:both;'></div>")
        DivEvalution.InnerHtml = Sb.ToString()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Session("ChooseMode") IsNot Nothing Then
            If Session("ChooseMode") = 6 Then
                Response.Redirect("~/LoginPage.aspx")
            Else
                Response.Redirect("~/PracticeMode_Pad/choosetestset.aspx?DeviceUniqueID=" & Session("PDeviceId"))
            End If
        Else
            Response.Redirect("~/PracticeMode_Pad/choosetestset.aspx?DeviceUniqueID=" & Session("PDeviceId"))
        End If
    End Sub

    Private Sub btnRec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRec.Click
        CreateList(0)
    End Sub

    Private Sub btnCon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCon.Click
        CreateList(1)
    End Sub

    <Services.WebMethod()>
    Public Shared Function SaveQuiz(ByVal ItemId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim KNS As New KNAppSession()
        Dim ClsPrintTestset As New ClsPrintTestset(HttpContext.Current.Session("UserId").ToString(), ItemId, KNS.StoredValue("CurrentCalendarId").ToString, HttpContext.Current.Session("IsTeacher").ToString)

        'HttpContext.Current.Session("UserId") = HttpContext.Current.Session("PUserId")
        Dim TestSetID As String = ClsPrintTestset.GetTestSetID()
        HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice
        Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId") & "&ItemId=" & TestSetID
    End Function
End Class