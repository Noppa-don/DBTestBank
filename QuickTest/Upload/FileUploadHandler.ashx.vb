Imports System.IO
Imports System.Web
Imports System.Web.Services

Public Class FileUploadHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If context.Request.Files.Count > 0 Then
            Try
                Dim qsetId As String = context.Request.QueryString("qsetId").ToString()
                Dim RefId As String = context.Request.QueryString("RefId").ToString()
                Dim RefType As String = context.Request.QueryString("RefType").ToString()
                Dim MLevel As String = context.Request.QueryString("MLevel").ToString()
                Dim files As HttpFileCollection = context.Request.Files

                Dim lPath As String = ""
                Dim sPath As String = ""
                Dim fname As String = ""
                Dim sName As String = ""

                GetPathToSaveFile(qsetId, lPath, sPath)
                For i As Integer = 0 To files.Count - 1
                    Dim file As HttpPostedFile = files(i)

                    fname = lPath & file.FileName
                    sName = sPath & file.FileName

                    file.SaveAs(fname)

                    If file.ContentType = "audio/mpeg" Then
                        'Save MultimediaObject
                        Dim ct As New ClsTestSet("")
                        ct.SaveMultimediaFile(qsetId, RefId, file.FileName, file.ContentType, RefType, MLevel)
                    End If
                Next
                context.Response.ContentType = "text/plain"
                context.Response.Write(sName)
            Catch ex As Exception
                context.Response.ContentType = "text/plain"
                context.Response.Write("False")
            End Try
        End If

    End Sub

    Private Sub GetPathToSaveFile(qsetId As String, ByRef lPath As String, ByRef sPath As String)

        Dim PathProJect As String = HttpContext.Current.Server.MapPath("../file/")
        Dim SerPath As String = "../file/"

        Dim ArrayCreateFolder As New ArrayList
        ArrayCreateFolder.Add(qsetId.Substring(0, 1))
        ArrayCreateFolder.Add(qsetId.Substring(1, 1))
        ArrayCreateFolder.Add(qsetId.Substring(2, 1))
        ArrayCreateFolder.Add(qsetId.Substring(3, 1))
        ArrayCreateFolder.Add(qsetId.Substring(4, 1))
        ArrayCreateFolder.Add(qsetId.Substring(5, 1))
        ArrayCreateFolder.Add(qsetId.Substring(6, 1))
        ArrayCreateFolder.Add(qsetId.Substring(7, 1))
        ArrayCreateFolder.Add("{" & qsetId & "}")

        For Each i In ArrayCreateFolder
            PathProJect &= i & "\"
            SerPath &= i & "\"

            Dim CreateFolder As New DirectoryInfo(PathProJect)
            If Not CreateFolder.Exists Then
                CreateFolder.Create()
            End If

        Next

        lPath = PathProJect
        sPath = SerPath

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class