Public Class SchoolInfo
    Inherits System.Web.UI.Page
    Public Shared TotalHour As String
    Public Shared spnTotalHourClass As String
    Public Shared MaleActive As String
    Public Shared FemaleActive As String
    Public Shared TotalQuiz As String
    Public Shared spnTotalQuizClass As String
    Public Shared TotalPractice As String
    Public Shared spnTotalPracticeClass As String
    Public Shared TotalHomework As String
    Public Shared spnTotalHomeworkClass As String
    Dim _DB As New ClassConnectSql()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim SchoolId As String = "1000001"
 
        TotalHour = GetTotalHour(SchoolId)

        If TotalHour.Length = 2 Then
            spnTotalHourClass = "ForspnTotalHour2Digit"
        ElseIf TotalHour.Length = 3 Then
            spnTotalHourClass = "ForspnTotalHour3Digit"
        ElseIf TotalHour.Length = 4 Then
            spnTotalHourClass = "ForspnTotalHour4Digit"
        Else
            spnTotalHourClass = "ForspnTotalHour1Digit"
        End If

        MaleActive = GetMaleAndFemaleActive(SchoolId) & "%"

        FemaleActive = (100 - GetMaleAndFemaleActive(SchoolId)).ToString() & "%"

        TotalQuiz = GetTotalActivityByMode(SchoolId, 1)
        If TotalQuiz.Length = 2 Then
            spnTotalQuizClass = "ForspnTotalQuiz2Digit"
        ElseIf TotalQuiz.Length = 3 Then
            spnTotalQuizClass = "ForspnTotalQuiz3Digit"
        ElseIf TotalQuiz.Length = 5 Then
            spnTotalQuizClass = "ForspnTotalQuiz4Digit"
        Else
            spnTotalQuizClass = "ForspnTotalQuiz1Digit"
        End If

        TotalHomework = GetTotalActivityByMode(SchoolId, 2)
        If TotalHomework.Length = 2 Then
            spnTotalHomeworkClass = "ForspnTotalHomework2Dgit"
        ElseIf TotalHomework.Length = 3 Then
            spnTotalHomeworkClass = "ForspnTotalHomework3Dgit"
        ElseIf TotalHomework.Length = 5 Then
            spnTotalHomeworkClass = "ForspnTotalHomework4Dgit"
        Else
            spnTotalHomeworkClass = "ForspnTotalHomework1Dgit"
        End If

        TotalPractice = GetTotalActivityByMode(SchoolId, 3)
        If TotalPractice.Length = 2 Then
            spnTotalPracticeClass = "ForspnTotalPractice2Digit"
        ElseIf TotalPractice.Length = 3 Then
            spnTotalPracticeClass = "ForspnTotalPractice3Digit"
        ElseIf TotalPractice.Length = 5 Then
            spnTotalPracticeClass = "ForspnTotalPractice4Digit"
        Else
            spnTotalPracticeClass = "ForspnTotalPractice1Digit"
        End If

    End Sub

    Private Function GetTotalHour(ByVal SchoolId As String)

        Dim sql As String = " SELECT  SUM(  DATEDIFF( SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) ) / 3600 as TotalHour " & _
                            " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id WHERE (tblQuiz.t360_SchoolCode = '" & SchoolId & "') " & _
                            " AND (tblQuizScore.IsActive = 1) AND (tblQuiz.IsActive = 1) and (DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) between 0 and 10 ) "
        Dim TotalHour As Integer = CType(_DB.ExecuteScalar(sql), Integer)
        
        Return TotalHour.ToString("#,##0")

    End Function

    Private Function GetMaleAndFemaleActive(SchoolId As String)

        Dim sql As String = " SELECT COUNT (distinct t360_tblStudent.Student_Id) " & _
                            " FROM tblQuizSession INNER JOIN t360_tblStudent ON tblQuizSession.Player_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (tblQuizSession.School_Code = '" & SchoolId & "') and (t360_tblStudent.Student_PrefixName = 'นาย' or " & _
                            " t360_tblStudent.Student_PrefixName = 'นางสาว') "
        Dim AssignValue As String = ""
        AssignValue = _DB.ExecuteScalar(sql)
        Dim StudentValue As Double = 0
        Dim TotalQuantity As Double = CType(AssignValue, Double)

        sql = " SELECT COUNT (distinct t360_tblStudent.Student_Id) FROM tblQuizSession INNER JOIN " & _
              " t360_tblStudent ON tblQuizSession.Player_Id = t360_tblStudent.Student_Id " & _
              " WHERE (tblQuizSession.School_Code = '" & SchoolId & "') and (t360_tblStudent.Student_PrefixName = 'นาย' ) "
        AssignValue = _DB.ExecuteScalar(sql)
        StudentValue = CType(AssignValue, Double)

        'sql = " SELECT COUNT (distinct t360_tblStudent.Student_Id) FROM  tblQuizSession INNER JOIN " & _
        '      " t360_tblStudent ON tblQuizSession.Player_Id = t360_tblStudent.Student_Id " & _
        '      " WHERE (tblQuizSession.School_Code = '" & SchoolId & "') and ( t360_tblStudent.Student_PrefixName = 'นางสาว') "
        'AssignValue = _DB.ExecuteScalar(sql)
        'StudentValue = CType(AssignValue, Double)

        Dim TotalActive As Integer = Math.Ceiling((StudentValue / TotalQuantity) * 100)
        Return TotalActive

    End Function

    Private Function GetTotalActivityByMode(ByVal SchoolId As String, Mode As Integer) As String

        Dim CalendarId As String = GetCalendarId(SchoolId)
        If CalendarId = "" Then
            Return "-1"
        End If

        Dim sql As String = " select COUNT(*) from tblQuiz where t360_SchoolCode = '" & SchoolId & "' and " & _
                            " Calendar_Id = '" & CalendarId.ToString() & "' "

        If Mode = 1 Then
            sql &= " and IsQuizMode = 1 "
        ElseIf Mode = 2 Then
            sql &= " and IsHomeWorkMode = 1 "
        ElseIf Mode = 3 Then
            sql &= " and IsPracticeMode = 1 "
        End If

        Dim TotalActivity As Integer = CType(_DB.ExecuteScalar(sql), Integer)
        Dim CountQuiz As String = TotalActivity.ToString("#,##0")
        Return CountQuiz

    End Function

    Private Function GetCalendarId(ByVal SchoolId As String)

        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                            " AND Calendar_Type = 3 AND School_Code = '" & SchoolId & "'; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql)
        Return CalendarId

    End Function
     

End Class