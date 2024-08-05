Imports System.IO

Public Class ClsLog
    Private Shared logFilename As String = ConfigurationManager.AppSettings("PathTextFileDetailLog").ToString()

    Public Shared Sub Log(ByVal txt As String)
        If EnableDetailLog() Then
            txt = DateTime.Now & " - " & txt & Environment.NewLine
            My.Computer.FileSystem.WriteAllText(logFilename, txt, True, Text.Encoding.Default)
        End If
    End Sub

    Public Shared Sub CheckFileLog()
        If Not File.Exists(logFilename) Then
            My.Computer.FileSystem.WriteAllText(logFilename, "สร้างไฟล์", False, Text.Encoding.Default)
            Log("สร้างไฟล์ Log")
        End If
    End Sub

    Public Shared Function EnableDetailLog() As Boolean
        If HttpContext.Current.Application("EnableDetailLog") IsNot Nothing AndAlso HttpContext.Current.Application("EnableDetailLog") Then
            Return HttpContext.Current.Application("EnableDetailLog")
        Else
            Return CBool(ConfigurationManager.AppSettings("EnableDetailLog"))
        End If
        Return False
    End Function

    Public Shared Sub Record(txt As String)

        Dim db As New ClassConnectSql()
        Try
            Dim sql = "insert into tblLog select newid(),null,111,'" & txt & "',1,1,1,dbo.GetThaiDate(),null,newid(),null,null"
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            db.CloseConnect()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Sub
End Class
