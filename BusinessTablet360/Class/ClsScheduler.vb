Imports KnowledgeUtils

Public Class ClsScheduler

    Dim _DB As ClsConnect
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Function NextSemester(School_Code As String) As Boolean

        Dim sql As String

        sql = "SELECT TOP 1 Calendar_Id as CalendarNow,Calendar_Year FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate"
        sql &= " AND Calendar_Type = 3 AND School_Code = '" & School_Code & "' and IsActive = '1' "

        Dim dtCalendarNow As DataTable = _DB.getdata(sql)

        sql = " Select distinct Calendar_Id as CalendarOld From t360_tblStudentRoom where School_Code = '" & School_Code & "' and SR_IsActive = '1'"

        Dim dtCalendarOld As DataTable = _DB.getdata(sql)

        If dtCalendarNow.Rows.Count <> 0 And dtCalendarOld.Rows.Count <> 0 Then

            Try

                For Each a In dtCalendarOld.Rows

                    If a("CalendarOld").ToString <> dtCalendarNow.Rows(0)("CalendarNow").ToString Then

                        sql = "select * into tbltmpStudentcld from t360_tblStudentRoom where SR_isActive = '1' " &
                                " and school_Code = '" & School_Code & "' and Calendar_Id = '" & a("CalendarOld").ToString & "'" &
                        " Update t360_tblStudentRoom set SR_IsActive = '0' where SR_isActive = '1' and school_Code = '" & School_Code & "' " &
                         " and Calendar_Id = '" & a("CalendarOld").ToString & "'" &
                        " Update tbltmpStudentcld Set Calendar_Id = '" & dtCalendarNow.Rows(0)("CalendarNow").ToString & "' " &
                         ", SR_ID = NEWID(),Lastupdate= dbo.GetThaiDate(),SR_AcademicYear = '" & dtCalendarNow.Rows(0)("Calendar_Year").ToString & "'" &
                        " insert into t360_tblStudentRoom Select * From tbltmpStudentcld" &
                        " Drop Table tbltmpStudentcld"

                        _DB.Execute(sql)

                        'ปรับ t360_tblUpLevel ถ้าได้มีการปับ
                        sql = "UPDATE t360_tblUpLevel SET Calendar_Id='" & dtCalendarNow.Rows(0)("CalendarNow").ToString & "'" &
                              " ,Calendar_Year='" & dtCalendarNow.Rows(0)("Calendar_Year").ToString & "'" &
                              " ,ClientId=NULL" &
                              " ,LastUpdate=dbo.GetThaiDate()" &
                              " WHERE School_Code='" & School_Code & "' AND IsActive=1 "

                        _DB.Execute(sql)
                    End If
                Next

                Return True
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End If
        Return True
    End Function

End Class
