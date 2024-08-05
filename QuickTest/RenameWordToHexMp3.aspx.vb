Imports System.IO

Public Class RenameWordToHexMp3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub RenameWordToHex()
        Dim path As String = ""
        Dim dir As New DirectoryInfo(path)
        

    End Sub


    Private Shared Function StrToHex(ByVal Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        While Data.Length > 0
            sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
            Data = Data.Substring(1, Data.Length - 1)
            sHex = sHex & sVal & "-"
        End While
        Return sHex.Substring(0, sHex.Length - 1)
    End Function

    Private Sub btnReNameWordToHex_Click(sender As Object, e As EventArgs) Handles btnReNameWordToHex.Click
        Dim folderPath As String = "D:\newMp3\words"
        For Each p In My.Computer.FileSystem.GetDirectories(folderPath)
            'd:\newmp3\words\1-500
            For Each file In My.Computer.FileSystem.GetFiles(p)
                Dim fileInfo As New System.IO.FileInfo(file.ToString())
                Dim nameHex As String = StrToHex(fileInfo.Name.Replace(fileInfo.Extension, ""))
                fileInfo.CopyTo(String.Format("D:\newMp3\renameWords\{0}.mp3", nameHex), True)
                'fileInfo.Delete()
            Next
        Next

       
    End Sub


End Class