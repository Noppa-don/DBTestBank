
Namespace Database.SqlServer

    ''' <summary>
    ''' คลาสช่วยจัดการ Sql Server
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageSqlServer

        Public Shared Function ConvertDateTimeToString(ByVal DateSource As DateTime) As String
            Dim D = DateSource.Year.ToString & "-" & DateSource.Month.ToString("00") & "-" & DateSource.Day.ToString("00")
            Dim T = DateSource.Hour.ToString("00") & ":" & DateSource.Minute.ToString("00") & ":" & DateSource.Second.ToString("00") &
                    "." & DateSource.Millisecond.ToString("000")

            Return D & " " & T
        End Function

    End Class

End Namespace



