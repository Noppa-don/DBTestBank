Imports System.Net
Imports System.IO
Imports KnowledgeUtils

Public Class NetUtil

    Public Overloads Function UploadFile(ByVal DataStream As Byte(), ByVal URL As String, ByVal ClientId As String, Service As Object, FileName As String, Optional ByVal UserName As String = "", _
                         Optional ByVal Password As String = "", Optional ByVal Port As String = "") As Boolean

        ServicePointManager.MaxServicePointIdleTime = 4800000

        Dim Msg As String = ""

        Try
            Dim svc = Service
            Dim data As Byte() = DataStream
            Dim strFile As String = Path.GetFileName(FileName)
            Msg = svc.ImportDb(data, strFile)

            Return CType(Msg, Boolean)

        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try

    End Function

    Public Overloads Function UploadFile(ByVal FileToUpload As String, ByVal URL As String, ByVal ClientId As String, Service As Object, Optional ByVal UserName As String = "", _
                          Optional ByVal Password As String = "", Optional ByVal Port As String = "") As Boolean

        ServicePointManager.MaxServicePointIdleTime = 4800000

        Dim Msg As String = ""

        Try
            ' get the file name from the path
            Dim strFile As String = Path.GetFileName(FileToUpload)
            ' create an instance of the Web service
            'Dim svc As New ServiceReference1.TestServiceSoapClient 
            Dim svc = Service
            ' get the file information for the selected file
            Dim fInfo As New FileInfo(FileToUpload)
            ' get the length of the file to see if it is possible
            ' to upload it (with the standard 4096 kb limit)
            Dim numBytes As Long = fInfo.Length
            ' Dim dLen As Double = Convert.ToDouble(fInfo.Length / 1000000)
            ' look for an overrun on file size
            'If (dLen < 30) Then
            ' set up a filestream and binary reader for the file
            Dim fStream As New FileStream(FileToUpload, FileMode.Open,
            FileAccess.Read)
            Dim br As New BinaryReader(fStream)
            ' convert the file to a byte array
            Dim data As Byte() = br.ReadBytes(Convert.ToInt32(numBytes))
            br.Close()
            ' pass the byte array and file name to the Web method
            Msg = svc.TestReceiveXml(data, strFile)

            fStream.Close()
            fStream.Dispose()
            Return CType(Msg, Boolean)

        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try

    End Function

End Class
