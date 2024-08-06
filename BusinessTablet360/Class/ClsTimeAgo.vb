Imports System.Globalization
Imports System.Text
Imports System.Data.SqlClient

Namespace Service

    Public Class ClsTimeAgo
        Public Sub New()
            CurrentTime = Date.Now
        End Sub

        Private Enum TimeType As Integer
            TimeAgo = 1
            TimeCreepOn = 2
        End Enum

        ' function set current time ก่อน
        Private CurrentTime As DateTime ' time now
        Private InTime As DateTime ' income time

        'Public Sub SetCurrentTime(Optional ByRef Conn As SqlConnection = Nothing)
        '    CurrentTime = New ClassConnectSql().ExecuteScalar(" SELECT dbo.GetThaiDate();", Conn).ToString()
        '    'CurrentTime = CheckFormatYear(c.ToString("yyyy-MM-dd HH:mm:ss"))
        'End Sub

        ' เวลาหลังจาก diff กันแล้ว
        Private Function GetDiffTime() As Object
            Select Case _TimeMode
                Case TimeType.TimeAgo
                    Return New With {.days = DateDiff(DateInterval.Day, InTime, CurrentTime),
                                            .hours = DateDiff(DateInterval.Hour, InTime, CurrentTime),
                                            .mins = DateDiff(DateInterval.Minute, InTime, CurrentTime)
                                           }
                Case TimeType.TimeCreepOn
                    Return New With {.days = DateDiff(DateInterval.Day, CurrentTime, InTime),
                                            .hours = DateDiff(DateInterval.Hour, CurrentTime, InTime),
                                            .mins = DateDiff(DateInterval.Minute, CurrentTime, InTime)
                                           }
            End Select
        End Function

        ' เปลี่ยนเลขปี
        Private Function CheckFormatYear(ByVal t As String) As String
            Dim Year As Integer = CInt(t.Substring(0, 4))
            If Year > 2500 Then
                Return t.Replace(t.Substring(0, 4), CStr(Year - 543))
            Else
                Return t
            End If
        End Function

        ' เวลาที่ผ่านไปแล้ว
        Public Function GetPhaseTimeAgo(ByVal Time As DateTime) As String
            '_Time = CheckFormatYear(Time.ToString("yyyy-MM-dd HH:mm:ss"))
            InTime = Time
            _TimeMode = TimeType.TimeAgo

            Dim difftime As String = GetDifferentTime(TimeType.TimeAgo)
            Dim strTime As New StringBuilder()
            strTime.Append(difftime)
            GetPhaseTimeAgo = strTime.ToString()
        End Function

        ' เวลาข้างหน้า
        Public Function GetCreepOnTime(ByVal Time As DateTime) As String
            '_Time = CheckFormatYear(Time.ToString("yyyy-MM-dd HH:mm:ss"))
            InTime = Time
            _TimeMode = TimeType.TimeCreepOn

            Dim difftime As String = GetDifferentTime(TimeType.TimeCreepOn)
            Dim strTime As New StringBuilder()
            strTime.Append(difftime)
            GetCreepOnTime = strTime.ToString()
        End Function

        Private Function GetDifferentTime(ByVal TimeMode As Integer) As String
            'Dim dtTime As DataTable = GetTime()
            'Dim days As Integer = dtTime.Rows(0)("days")
            'Dim hours As Integer = dtTime.Rows(0)("hours")
            'Dim mins As Integer = dtTime.Rows(0)("mins")

            Dim t As Object = GetDiffTime()
            Dim days As Integer = t.days
            Dim hours As Integer = CInt(t.hours) Mod 24
            Dim mins As Integer = CInt(t.mins) Mod 60
            Dim sec As Integer = 1
            _Time = InTime


            Dim StrTime As String

            If days >= 2 Then
                ' return เมื่อ 10/5
                If TimeMode = TimeType.TimeAgo Then
                    StrTime = "เมื่อ " & Convert.ToDateTime(_Time).ToString("dd/MM")
                Else
                    StrTime = Convert.ToDateTime(_Time).ToString("dd/MM")
                End If
            ElseIf days = 1 Then
                ' return เมื่อวาน/พรุ่งนี้ 18:00
                If TimeMode = TimeType.TimeAgo Then
                    StrTime = "เมื่อวาน " & Convert.ToDateTime(_Time).ToString("HH:mm")
                Else
                    StrTime = "พรุ่งนี้ " & Convert.ToDateTime(_Time).ToString("HH:mm")
                End If
            Else
                If hours >= 6 Then
                    If mins = 0 And sec = 0 Then
                        ' return เมื่อ/อีก 6 ชม เป๊ะะะะ
                        If TimeMode = TimeType.TimeAgo Then
                            StrTime = "เมื่อ 6 ชั่วโมงที่แล้ว"
                        Else
                            StrTime = "อีก 6 ชั่วโมง"
                        End If
                    Else
                        ' return เมื่อ/วันนี้ 12:22
                        If TimeMode = TimeType.TimeAgo Then
                            If CurrentTime = InTime Then
                                StrTime = "เมื่อ " & Convert.ToDateTime(_Time).ToString("HH:mm")
                            Else
                                StrTime = "เมื่อวาน " & Convert.ToDateTime(_Time).ToString("HH:mm")
                            End If
                        Else
                            StrTime = "วันนี้ " & Convert.ToDateTime(_Time).ToString("HH:mm")
                        End If
                    End If
                ElseIf hours >= 1 And hours <= 5 Then
                    ' return เมื่อ/อีก 2 ชม
                    If TimeMode = TimeType.TimeAgo Then
                        StrTime = "เมื่อ " & hours & " ชั่วโมงที่แล้ว"
                    Else
                        StrTime = "อีก " & hours & " ชั่วโมง"
                    End If
                Else
                    If mins > 1 Then
                        ' return เมื่อ/อีก 15 นาที
                        If TimeMode = TimeType.TimeAgo Then
                            StrTime = "เมื่อ " & mins & " นาทีที่แล้ว"
                        Else
                            StrTime = "อีก " & mins & " นาที"
                        End If
                    ElseIf mins = 1 Then
                        ' return เมื่อนาทีที่แล้ว
                        If TimeMode = TimeType.TimeAgo Then
                            StrTime = "เมื่อ นาทีที่แล้ว"
                        Else
                            StrTime = "อีก 1 นาที"
                        End If
                    Else
                        ' return เมื่อ/อีก วินาที
                        StrTime = sec & " วินาที"
                    End If
                End If
            End If
            GetDifferentTime = StrTime
        End Function

        ' Query หาค่าเวลาที่ต่างกัน
        Private _Time As String
        Private _TimeMode As Integer
        'Public Function GetTime() As DataTable
        '    ' check year ก่อน
        '    Dim chageYear = CheckAndChangeYear(_Time.Substring(0, 4))
        '    _Time = _Time.Replace(_Time.Substring(0, 4), chageYear)

        '    Dim sql As New StringBuilder()
        '    sql.Append(" DECLARE @Now DATETIME,@Time DATETIME ")
        '    sql.Append("SET @Now = dbo.GetThaiDate() ")
        '    sql.Append("SET @Time = '")
        '    sql.Append(_Time)

        '    Select Case _TimeMode
        '        Case TimeType.TimeAgo
        '            sql.Append("' SELECT DATEDIFF(dd,@Time,@Now) AS days, ")
        '            sql.Append(" DATEDIFF(hh,@Time,@Now) % 24  AS hours, ")
        '            sql.Append(" DATEDIFF(mi,@Time,@Now) % 60 AS mins ")
        '            'sql.Append(" DATEDIFF(SS,@Time,@Now) % 60 AS sec; ")
        '        Case TimeType.TimeCreepOn
        '            sql.Append("' SELECT DATEDIFF(dd,@Now,@Time) AS days, ")
        '            sql.Append(" DATEDIFF(hh,@Now,@Time) %24  AS hours, ")
        '            sql.Append(" DATEDIFF(mi,@Now,@Time) % 60 AS mins ")
        '            'sql.Append(" DATEDIFF(SS,@Now,@Time) % 60 AS sec; ")
        '    End Select
        '    Dim db As New ClassConnectSql()
        '    GetTime = db.getdata(sql.ToString(), , _InputConn)
        'End Function

        Private Function CheckAndChangeYear(ByVal strYear As String) As String
            Dim Year As Integer = CInt(strYear)
            If Year > 2500 Then
                CheckAndChangeYear = (Year - 543).ToString()
            Else
                CheckAndChangeYear = strYear
            End If
        End Function
    End Class

End Namespace


