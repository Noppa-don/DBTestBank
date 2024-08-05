
Namespace Database.Sqlite

    ''' <summary>
    ''' คลาสช่วยจัดการ Sqlite
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageSqlite

        Public Shared Function ConvertDateTime(DateSource As DateTime) As String
            Dim D = DateSource.Year.ToString & "-" & DateSource.Month.ToString("00") & "-" & DateSource.Day.ToString("00")
            Dim T = DateSource.Hour.ToString("00") & ":" & DateSource.Minute.ToString("00") & ":" & DateSource.Second.ToString("00")
            Return D & " " & T
        End Function

    End Class

End Namespace


