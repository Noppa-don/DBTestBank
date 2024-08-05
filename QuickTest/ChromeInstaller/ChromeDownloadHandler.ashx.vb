Imports System.Web
Imports System.Web.Services

Public Class ChromeDownloadHandler
    Implements System.Web.IHttpHandler
    Dim FileName As String
    Dim OSName As String

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        If System.IO.File.Exists(HttpContext.Current.Server.MapPath("./" & OSName & "/" & FileName)) = True Then
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/octet-stream"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" & FileName)
            HttpContext.Current.Response.WriteFile(HttpContext.Current.Server.MapPath("./" & OSName & "/" & FileName))
            HttpContext.Current.Response.End()
        End If
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Public Sub New()
        If HttpContext.Current.Request.QueryString("OSName") IsNot Nothing Then
            If HttpContext.Current.Request.QueryString("OSName").ToLower() = "windows" Then
                OSName = "windows"
                FileName = "ChromeSetup.exe"
            ElseIf HttpContext.Current.Request.QueryString("OSName").ToLower() = "mac" Then
                OSName = "mac"
                FileName = "googlechrome.dmg"
            Else
                If HttpContext.Current.Request.QueryString("FileVersion") IsNot Nothing Then
                    OSName = "linux"
                    FileName = HttpContext.Current.Request.QueryString("FileVersion").ToString().Trim()
                End If
            End If
        End If
    End Sub

End Class