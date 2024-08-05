Imports System.IO

Public Class ClsLog

    'Private Const logFilename As String = ConfigurationManager.AppSettings("PathTextFileDetailLog").ToString()

    Private Shared logFilename As String = ConfigurationManager.AppSettings("PathTextFileDetailLog").ToString()

    Public Shared Sub Log(ByVal txt As String)
        If EnableDetailLog() Then
            'txt = txt.Insert(0, DateTime.Now.ToString).Insert(txt.Length - 1, Environment.NewLine)
            txt = DateTime.Now & txt & Environment.NewLine
            My.Computer.FileSystem.WriteAllText(logFilename, txt, True, Text.Encoding.Default)
        End If
    End Sub

    Public Shared Sub CheckFileLog()
        If Not File.Exists(logFilename) Then
            'My.Computer.FileSystem.WriteAllText(logFilename, "สร้างไฟล์", False, Text.Encoding.Default)
            Log("สร้างไฟล์ Log")
        End If
    End Sub

    Public Shared Function EnableDetailLog() As Boolean
        If HttpContext.Current.Application("EnableDetailLog") IsNot Nothing AndAlso HttpContext.Current.Application("EnableDetailLog") Then
            Return HttpContext.Current.Application("EnableDetailLog")
        End If
        Return False
    End Function
End Class
