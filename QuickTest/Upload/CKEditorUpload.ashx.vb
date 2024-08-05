Imports System.IO
Imports System.Web
Imports System.Web.Services

Public Class CKEditorUpload
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim pdfCls As New ClsPDF(New ClassConnectSql)

        Dim upload As HttpPostedFile
        upload = context.Request.Files("upload")

        Dim CKEditorFuncNum As String = context.Request("CKEditorFuncNum")
        Dim file As String = System.IO.Path.GetFileName(upload.FileName)

        QsetId = context.Session("QsetImagePath").ToString()
        'Dim rootPath As String = pdfCls.GenFilePath(qsetId)


        MapPath = context.Server.MapPath("~") & "\File\"
        Dim path As String = GetPathSaveImage()
        Dim fullPath As String = MapPath & path
        Dim ImgFolder As New DirectoryInfo(fullPath)
        If Not ImgFolder.Exists Then
            CreateFolder()
        End If

        'upload.SaveAs(context.Server.MapPath("~") + "/File/" & rootPath & file)
        upload.SaveAs(fullPath & file)


        'Dim url As String = "http://localhost:18615/File/" & path.Replace("\", "/") & file
        Dim url As String = "../file/" & path.Replace("\", "/") & file

        context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" & CKEditorFuncNum & ", '" & url & "');</script>")
        context.Response.End()
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Property QsetId As String
    Property MapPath As String

    Private Function GetPathSaveImage() As String
        Dim folders As List(Of String) = GetFolders()
        Dim path As String = ""
        For Each f In folders
            path &= f & "\"
        Next
        Return path
    End Function

    Private Function CreateFolder() As Boolean
        Try
            Dim folders As List(Of String) = GetFolders()
            Dim path As String = MapPath
            For Each i In folders
                path &= i & "\"
                Dim f As New DirectoryInfo(path)
                If Not f.Exists Then
                    f.Create()
                End If
            Next
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function GetFolders() As List(Of String)
        Dim folders As New List(Of String)
        folders.Add(QsetId.Substring(0, 1))
        folders.Add(QsetId.Substring(1, 1))
        folders.Add(QsetId.Substring(2, 1))
        folders.Add(QsetId.Substring(3, 1))
        folders.Add(QsetId.Substring(4, 1))
        folders.Add(QsetId.Substring(5, 1))
        folders.Add(QsetId.Substring(6, 1))
        folders.Add(QsetId.Substring(7, 1))
        folders.Add("{" & QsetId & "}")
        Return folders
    End Function

End Class