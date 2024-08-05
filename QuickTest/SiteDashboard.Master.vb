Public Class newSite
    Inherits System.Web.UI.MasterPage

    Public IE As String
    Protected IsAndroid As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'SetCurrentPage()
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If

        If HttpContext.Current.Session("TabletUser") Is Nothing Then
            HttpContext.Current.Session("TabletUser") = If((Not Request.QueryString("u") Is Nothing), True, False)
        End If



#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
#End If
        ' If Not Page.IsPostBack Then
        ProcessHelpPanel()
        ' End If
    End Sub

    'Sub เกี่ยวกับ Help ในแต่ละหน้าที่ใช้ Masterpage นี้
    Private Sub ProcessHelpPanel()
        Try
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
            Log.Record(Log.LogType.Login, "DashboardMaster complete", True)
        Catch ex As Exception
            Log.Record(Log.LogType.Login, "catch DashboardMaster " & ex.ToString, True)
        End Try

    End Sub

    Private Sub SetCurrentPage()
        Dim ClsSelectSession As New ClsSelectSession()
        'Dim res As String = ResolveUrl("~").ToLower()

        Dim CurrentPage As String = HttpContext.Current.Request.Url.AbsolutePath.ToLower().Substring(1)
        Log.Record(Log.LogType.Login, "CurrentPage " & CurrentPage, True)
        ClsSelectSession.SetCurrentPage(CurrentPage)
    End Sub
End Class