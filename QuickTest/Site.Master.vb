Public Class Site
    Inherits System.Web.UI.MasterPage

    Protected BGName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'SetCurrentPage()
        KNConfigData.LoadData()

        Page.Header.DataBind()
        createLinkCss()
        'If Not Page.IsPostBack Then
        ProcessHelpPanel()
        'End If

        setBGName()

    End Sub

    Private Sub createLinkCss()
        Dim linkCss As New HtmlLink()
        linkCss.Attributes.Add("type", "text/css")
        linkCss.Attributes.Add("rel", "stylesheet")
        If (HttpContext.Current.Application("NeedQuizMode") = True) Then
            linkCss.Attributes.Add("href", "./css/styleQuiz.css")
        Else
            linkCss.Attributes.Add("href", "./css/style.css")
        End If
        Me.HeadStyleContent.Controls.Add(linkCss)
        If ClsKNSession.IsMaxONet Then 'check ว่า maxonet?
            Dim cssMaxOnet As New HtmlLink()
            cssMaxOnet.Attributes.Add("type", "text/css")
            cssMaxOnet.Attributes.Add("rel", "stylesheet")
            cssMaxOnet.Attributes.Add("href", "./css/MaxOnetStyle.css")
            Me.HeadStyleContent.Controls.Add(cssMaxOnet)
        End If
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
                If PageName = "AlternativePage" Then
                    If Session("ChooseMode") = EnumDashBoardType.Quiz Then
                        StrCheckImage = HttpContext.Current.Server.MapPath("../HowTo/Helpimg/" & ClsKNSession.RunMode & "/Activity_AlternativePageQuiz00.png")
                    Else
                        StrCheckImage = HttpContext.Current.Server.MapPath("../HowTo/Helpimg/" & ClsKNSession.RunMode & "/Activity_AlternativePagePractice00.png")
                    End If
                Else
                    StrCheckImage = HttpContext.Current.Server.MapPath("../HowTo/Helpimg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & "00.png")
                End If

                'End If
                If System.IO.File.Exists(StrCheckImage) = False Then
                    Help.Style.Add("display", "none")
                Else
                    Dim StrScript As String
                    If PageName = "AlternativePage" Then
                        If Session("ChooseMode") = EnumDashBoardType.Quiz Then
                            StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=Activity&PageName=AlternativePageQuiz','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                        Else
                            StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=Activity&PageName=AlternativePagePractice','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                        End If
                    Else
                        StrScript = "<script type='text/javascript'>$(function () {$('#Help').click(function () {$.fancybox({'autoScale': true,'blackBG':true,'transitionIn': 'none','transitionOut': 'none','href': '../ShowImgHelpPage.aspx?FolderName=" & FolderName & "&PageName=" & PageName & "','type': 'iframe','width': 750,'minHeight':425});});});</script>"
                    End If
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)
                End If
            End If
        End If

    End Sub

    Private Sub setBGName()
        Dim bg As String = ""
        If Session("PSubjectName") IsNot Nothing AndAlso Session("PSubjectName").ToString() <> "" Then
            Dim subjectId As String = Session("PSubjectName").ToString().ToUpper()
            Select Case subjectId
                Case CommonSubjectsText.ArtSubjectId
                    bg = "art"
                Case CommonSubjectsText.EnglishSubjectId
                    bg = "eng"
                Case CommonSubjectsText.HealthSubjectId
                    bg = "health"
                Case CommonSubjectsText.HomeSubjectId
                    bg = "home"
                Case CommonSubjectsText.MathSubjectId
                    bg = "math"
                Case CommonSubjectsText.ScienceSubjectId
                    bg = "science"
                Case CommonSubjectsText.SocialSubjectId
                    bg = "social"
                Case CommonSubjectsText.ThaiSubjectId
                    bg = "thai"
            End Select
        End If
        BGName = bg
    End Sub

    Private Sub SetCurrentPage()
        Dim ClsSelectSession As New ClsSelectSession()
        'Dim res As String = ResolveUrl("~").ToLower()

        Dim CurrentPage As String = HttpContext.Current.Request.Url.AbsolutePath.ToLower().Substring(1)
        Log.Record(Log.LogType.Login, "CurrentPage " & CurrentPage, True)
        ClsSelectSession.SetCurrentPage(CurrentPage)
    End Sub

End Class