Imports System.Globalization
Imports System.Runtime.CompilerServices

Namespace System.DateTimeUtil

    Public Class ManageDateTime

        Private Shared TraceTimeBegin As DateTime


        Public Shared Sub TraceTimeStart()
            TraceTimeBegin = Now
        End Sub

        Public Shared Function TraceTimeDiff() As String
            Dim DiffTime As TimeSpan = Now - TraceTimeBegin
            TraceTimeStart()
            Return DiffTime.ToString
        End Function

        ''' <summary>
        ''' หาค่าต่างของเวลาสองเวลา
        ''' </summary>
        ''' <param name="Time1">เวลาเริ่ม</param>
        ''' <param name="Time2">เวลาสิ้นสุด</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function DiffTime(ByVal Time1 As TimeSpan, ByVal Time2 As TimeSpan) As String
            Dim T As TimeSpan = Time2 - Time1
            DiffTime = T.Hours & ":" & T.Minutes & ":" & T.Seconds
        End Function

        ''' <summary>
        ''' คำนวนระยะห่างระหว่างวัน ในรูปแบบ วัน เดือน ปี (ช่องที่ 0,ช่องที่ 1,ช่องที่ 2)
        ''' </summary>
        ''' <param name="TargetDate">วันที่เริ่มต้น</param>
        ''' <param name="CalDate">วันที่สิ้นสุด</param>
        ''' <returns>คืนค่าเป็น array of integer</returns>
        ''' <remarks>วันที่ Araayช่องที่ 1,เดือน Araayช่องที่ 2,ปี Araayช่องที่ 3</remarks>
        Public Shared Function CalculateAge(ByVal TargetDate As Date, ByVal CalDate As Date) As Integer()
            Dim TmpDate As Date

            Dim Y As Integer = DateDiff(DateInterval.Year, TargetDate, CalDate)
            TmpDate = DateAdd(DateInterval.Year, Y, TargetDate)
            If TmpDate > CalDate Then
                Y -= 1
            End If
            TargetDate = DateAdd(DateInterval.Year, Y, TargetDate)

            Dim M As Integer = DateDiff(DateInterval.Month, TargetDate, CalDate)
            TmpDate = DateAdd(DateInterval.Month, M, TargetDate)
            If TmpDate > CalDate Then
                M -= 1
            End If

            TargetDate = DateAdd(DateInterval.Month, M, TargetDate)
            Dim D As Integer = DateDiff(DateInterval.Day, TargetDate, CalDate)

            Return New Integer() {D, M, Y}
        End Function

        ''' <summary>
        ''' แปลงวันที่รูปแบบ String เป็น Date เหมาะกับ วันที่ที่ถูกแปลงมาจาก CurrentCulture
        ''' </summary>
        ''' <param name="DateString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ToDate(ByVal DateString As String, Optional ByVal Culture As EnumCultureInfo = Nothing) As Date
            Try
                Dim Cu As CultureInfo = CultureInfo.CurrentCulture
                Select Case Culture
                    Case EnumCultureInfo.enGB
                        Cu = New CultureInfo("en-GB")
                    Case EnumCultureInfo.enUS
                        Cu = New CultureInfo("en-US")
                    Case EnumCultureInfo.thTH
                        Cu = New CultureInfo("th-TH")
                End Select
                Return Date.Parse(DateString, Cu)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Throw New Exception(ex.Message)
            End Try
        End Function

    End Class

    Public Module ModuleManageDateTime

        ''' <summary>
        ''' แปลงวันที่รูปแบบ String เป็น Date เหมาะกับ วันที่ที่ถูกแปลงมาจาก CurrentCulture
        ''' </summary>
        ''' <param name="DateString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function ToDate(ByVal DateString As String, Optional ByVal Culture As EnumCultureInfo = EnumCultureInfo.CurrentCulture) As Date
            Try
                Dim Cu As CultureInfo = CultureInfo.CurrentCulture
                Select Case Culture
                    Case EnumCultureInfo.enGB
                        Cu = New CultureInfo("en-GB")
                    Case EnumCultureInfo.enUS
                        Cu = New CultureInfo("en-US")
                    Case EnumCultureInfo.thTH
                        Cu = New CultureInfo("th-TH")
                End Select
                Return Date.Parse(DateString, Cu)
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Throw New Exception(ex.Message)
            End Try
        End Function

    End Module

    Public Enum EnumCultureInfo
        CurrentCulture
        enGB
        enUS
        thTH
    End Enum

End Namespace



