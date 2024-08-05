Public Class Default2
    Inherits System.Web.UI.Page
    Public Strhref As String
    Public OSCheck As String
    'Public Property OSName As String
    '    Get
    '        OSName = ViewState("_OSName")
    '    End Get
    '    Set(value As String)
    '        ViewState("_OSName") = value
    '    End Set
    'End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("OSName") IsNot Nothing Then
            If Request.QueryString("OSName").ToLower() = "windows" Then
                PanelWindow.Visible = True
                Strhref = "ChromeInstaller/ChromeDownloadHandler.ashx?OSName=windows"
                OSCheck = "windows"
            ElseIf Request.QueryString("OSName").ToLower() = "mac" Then
                PanelMac.Visible = True
                Strhref = "ChromeInstaller/ChromeDownloadHandler.ashx?OSName=mac"
                OSCheck = "mac"
            ElseIf Request.QueryString("OSName").ToLower() = "ios" Then
                PanelIOS.Visible = True
                Strhref = "https://itunes.apple.com/us/app/chrome-web-browser-by-google/id535886823?mt=8"
                OSCheck = "ios"
            Else
                PanelLinux.Visible = True
                lblHead.Text = "เลือกดาวน์โหลด และติดตั้ง Chrome ให้ตรงกับ OS ที่ใช้อยู่นะคะ"
                DivLinkDownload.Visible = False
                OSCheck = "linux"
            End If
        End If

    End Sub
     
End Class