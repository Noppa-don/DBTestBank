Public Class ClassAnalysisReport

    Dim _DB As ClsConnect
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

#Region "School Section"

    'Function หลักที่ไล่หาข้อมูล และ Insert ลง tmp สำหรับ SchoolReport
    Public Function ProcessTmpSchool(ByVal SchoolId As String, ByVal SelectedDate As DateTime) As String
        Dim Month As String = SelectedDate.Month
        Dim Year As String = CleanFormatCalendar(SelectedDate.Year)
        Dim TotalHour As String = ""
        Dim TotalSchoolActive As String = ""
        Dim TotalStudentActive As String = ""
        Dim TotalDevicePerfect As Integer = 0
        Dim TotalDeviceBroken As Integer = 0

        Try
            _DB.OpenWithTransection()

            'Insert tmp_SchoolReport
            InsertInTmpReportStudent(SchoolId, Year, Month, _DB)

            'Insert tmp_SchoolReportPractice
            InsertInTmpReportStudentPractice(SchoolId, Year, Month, _DB)

            'ได้จำนวนชั่วโมงทั้งหมดของเดือนนี้
            TotalHour = GetTotalHourActive(SchoolId, Month, Year, _DB)

            'ได้ Str ที่บอกว่าโรงเรียนนี้ใช้งานมากขึ้นหรือน้อยลงกี่ %
            TotalSchoolActive = GetCheckTotalSchoolActive(SchoolId, Month, Year, TotalHour, _DB)

            'ได้ Str ที่บอกว่านักเรียนมีการฝึกฝนมากขึ้นหรือน้อยลง
            TotalStudentActive = GetTotalStudentPracticeActive(SchoolId, Month, Year, _DB)

            'ได้ Str ที่บอกว่ามีอุปกรณ์เสียทั้งหมดเท่าไหร่
            TotalDeviceBroken = GetTotalBrokenDevice(SchoolId, Month, Year, _DB)

            'ได้ Str ที่บอกว่ามีอุปกรณ์ที่ไม่เสียหายทั้งหมดเท่าไหร่
            TotalDevicePerfect = 100 - CInt(TotalDeviceBroken)

            Dim sql As String = " insert into tmp_SchoolReportUse(SchoolId,MonthDate,Totalhour,TotalSchoolActive,TotalStudentActive,TotalDevicePerfect,TotalDeviceBroken) " & _
                                " values('" & SchoolId & "',CAST('" & Year & "-" & Month & "-1' as SmallDatetime),'" & TotalHour & "', " & _
                                " '" & TotalSchoolActive & "','" & TotalStudentActive & "','" & TotalDevicePerfect & "','" & TotalDeviceBroken & "') "
            _DB.ExecuteWithTransection(sql)
            _DB.CommitTransection()
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return ex.ToString()
            Exit Function
        End Try

    End Function

    'Funciton Insert ลง tmp แบบรายวันก่อนแล้วค่อยเอาข้อมูลจาก tmp นี้ไปหาข้อมูลที่ต้องการอีกทีนึง
    Private Sub InsertInTmpReportStudent(ByVal SchoolId As String, ByVal SelectedYear As String, ByVal SelectedMonth As String, ByRef ObjDb As ClassConnectSql)
        Try
            Dim sql As String = " insert into tmp_SchoolReport(SchoolCode,Day,TotalMinute) select tblQuiz.t360_SchoolCode,CAST(tblQuizScore.FirstResponse as date) as UsageDate ,  " & _
                                " SUM( cast(DATEDIFF( SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) as decimal(10,2) ) ) / 60  as TotalMinute " & _
                                " from tblQuiz inner join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id where DATEPART(YEAR,tblQuizScore.FirstResponse) = '" & SelectedYear & "' " & _
                                " and DATEPART(MONTH,tblQuizScore.FirstResponse) = '" & SelectedMonth & "' and (DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) between 0 and 10 ) " & _
                                " and tblQuiz.t360_SchoolCode = '" & SchoolId & "' and tblQuizScore.Isactive = 1 group by CAST(tblQuizScore.FirstResponse as date),tblQuiz.t360_SchoolCode "
            ObjDb.ExecuteWithTransection(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            HttpContext.Current.Response.Write(ex.ToString())
            Exit Sub
        End Try

    End Sub

    'Function Insert ลง tmp ฝึกฝน เพื่อเอาไว้ใช้เปรียบเทียบเวลาฝึกฝนของนักเรียนว่าเยอะขึ้นหรือน้อยลง
    Private Sub InsertInTmpReportStudentPractice(ByVal SchoolId As String, ByVal SelectedYear As String, ByVal SelectedMonth As String, ByRef ObjDb As ClassConnectSql)
        Try
            Dim sql As String = " insert into tmp_SchoolReportPractice(SchoolId,DateMonth,TotalUsePractice) " & _
                                " select tblQuiz.t360_SchoolCode,CAST(tblQuizScore.FirstResponse as date), " & _
                                " SUM( cast(DATEDIFF( SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) as decimal(10,2) ) ) / 60  as TotalMinute  " & _
                                " from tblQuiz inner join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id  " & _
                                " where tblQuiz.t360_SchoolCode = '" & SchoolId & "' and tblQuiz.IsPracticeMode = 1 " & _
                                " and DATEPART(MONTH,tblQuizScore.FirstResponse) = '" & SelectedMonth & "' and DATEPART(YEAR,tblQuizScore.FirstResponse) = '" & SelectedYear & "' and " & _
                                " tblQuizScore.IsActive = 1  and (DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) between 0 and 10 ) " & _
                                " group by tblQuiz.t360_SchoolCode,CAST( tblQuizScore.FirstResponse as date) "
            ObjDb.ExecuteWithTransection(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            HttpContext.Current.Response.Write(ex.ToString())
            Exit Sub
        End Try
    End Sub

    'Function หาเวลากรใช้งานรวมทั้งหมดของโรงเรียนนี้
    Private Function GetTotalHourActive(ByVal SchoolId As String, ByVal SelectedMonth As String, ByVal SelectedYear As String, ByRef ObjDb As ClassConnectSql)
        Dim sql As String = " select (SUM(TotalMinute)/60) as totalhour  from  tmp_SchoolReport where datepart(year,day) = '" & SelectedYear & "' and DATEPART(MONTH,day) = '" & SelectedMonth & "' "
        Dim TotalHour As Decimal = CType(ObjDb.ExecuteScalarWithTransection(sql), Decimal)
        TotalHour = TotalHour.ToString("N2")
        Return TotalHour
    End Function

    'Function เช็คว่ามีการใช้งานมากขึ้นหรือลดลงเทียบกับเดือนก่อนหน้า
    Private Function GetCheckTotalSchoolActive(ByVal SchoolId As String, ByVal Month As String, ByVal Year As String, ByVal TotalHour As String, ByRef ObjDb As ClassConnectSql)
        Month = CInt(Month - 1)
        Dim StrReturn As String = ""
        Dim ProcessValue As Decimal = 0
        Dim PreviousMonthValue As Double = CInt(GetPreviousMonthData(SchoolId, Month, Year, ObjDb))
        If PreviousMonthValue = 0 Then
            ProcessValue = ((TotalHour - PreviousMonthValue) * 100) / 1
        Else
            ProcessValue = ((TotalHour - PreviousMonthValue) * 100) / TotalHour
        End If
        If ProcessValue > 0 Then
            StrReturn = "เพิ่มขึ้น " & ProcessValue.ToString("N2") & " %"
        Else
            StrReturn = "ลดลง " & ProcessValue.ToString("N2") & " %"
        End If
        Return StrReturn
    End Function

    'Function เช็คว่านักเรียนใช้งานฝึกฝนมากขึ้นหรือลดลง
    Private Function GetTotalStudentPracticeActive(ByVal SchoolId As String, ByVal Month As String, ByVal Year As String, ByRef ObjDb As ClassConnectSql)
        Dim StrReturn As String = ""
        Dim Processvalue As Decimal = 0
        Dim sql As String = " select Sum(TotalUsePractice) from tmp_SchoolReportPractice where DATEPART(MONTH,datemonth) = '" & Month & "' and DATEPART(YEAR,DateMonth) = '" & Year & "' "
        Dim ThisMonthValue As String = ObjDb.ExecuteScalarWithTransection(sql)

        sql = " select Sum(TotalUsePractice) from tmp_SchoolReportPractice where DATEPART(MONTH,datemonth) = '" & (Month - 1) & "' and DATEPART(YEAR,DateMonth) = '" & Year & "' "
        Dim PreviousMonthValue As String = ObjDb.ExecuteScalarWithTransection(sql)

        If PreviousMonthValue = "" Then
            PreviousMonthValue = 0
        End If

        If PreviousMonthValue = 0 Then
            Processvalue = ((ThisMonthValue - PreviousMonthValue) * 100) / 1
        Else
            Processvalue = ((ThisMonthValue - PreviousMonthValue) * 100) / PreviousMonthValue
        End If

        If Processvalue > 0 Then
            StrReturn = "เพิ่มขึ้น " & Processvalue.ToString("N2") & " %"
        Else
            StrReturn = "น้อยลง " & Processvalue.ToString("N2") & " %"
        End If
        Return StrReturn
    End Function

    'Function เช็คว่าจำนวนอุปกรณ์ที่ใช้ในโรงเรียนมีที่เสียทั้งหมดกี่ %
    Private Function GetTotalBrokenDevice(ByVal Schoolid As String, ByVal Month As String, ByVal Year As String, ByRef ObjDb As ClassConnectSql)
        Dim StrReturn As Integer = 0

        'หาจำนวน Device ทั้งหมดในโรงเรียน
        Dim sql As String = " select COUNT(distinct t360_tblTablet.Tablet_Id) + COUNT(distinct t360_tblNetwork.Network_Id) as TotalDeviceInSchool " & _
                            " from t360_tblTablet inner join t360_tblNetwork on t360_tblTablet.School_Code = t360_tblNetwork.School_Code " & _
                            " where t360_tblTablet.School_Code = '" & Schoolid & "' and t360_tblTablet.Tablet_IsActive = 1 and t360_tblNetwork.Network_IsActive = 1 "
        Dim TotalDeviceInSchool As String = ObjDb.ExecuteScalarWithTransection(sql)

        'หาจำนวน Tablet เครื่องที่เสียทั้งหมด
        sql = " select COUNT(*) from t360_tblTablet where School_Code = '" & Schoolid & "' and Tablet_IsActive =1  and Tablet_Status <> 1 "
        Dim TabletBroken As String = ObjDb.ExecuteScalarWithTransection(sql)

        'หาจำนวน อุปกรณ์ที่เสียมาเกิน 7 วัน
        sql = " select COUNT(*) from t360_tblNetwork where DATEDIFF(DAY,CAST('" & Year & "-" & Month & "-1' as datetime),Network_LastDate) > 7 and School_Code = '" & Schoolid & "' "
        Dim DeviceBroken As String = ObjDb.ExecuteScalarWithTransection(sql)

        StrReturn = ((CInt(TabletBroken) + CInt(DeviceBroken)) * 100) / CInt(TotalDeviceInSchool)
        Return StrReturn
    End Function

#End Region

#Region "Student Section"

    'Function หลัก วนทำข้อมูลเกียวกับนักเรียน
    Public Function ProcessTmpStudent(ByVal SchoolId As String, ByVal SelectedDate As DateTime) As String
        Dim Month As String = SelectedDate.Month()
        Dim Year As String = CleanFormatCalendar(SelectedDate.Year())
        Dim dt As DataTable = GetDtStudent(SchoolId)
        If dt.Rows.Count > 0 Then
            _DB.OpenWithTransection()
            For index = 0 To dt.Rows.Count - 1
                Try
                    'Insert tmp_StudentReportTotalActivePieChart ของ การใช้งาน
                    InsertStudentReportPieChart(dt.Rows(index)("Student_Id").ToString(), Month, Year)

                    'Insert tmp_StudentReportTotalUsageBySubject สำหรับ SpiderChart ของ Quiz
                    InsertStudentReportSpiderChart(dt.Rows(index)("Student_Id").ToString(), Month, Year)

                    'Insert tmp_StudentReportTotalUsageBySubject สำหรับ กราฟแท่ง ของ Practice
                    InsertTmpStudentReportTotalUsageBySubject(dt.Rows(index)("Student_Id").ToString(), Month, Year)

                    'tmp_StudentReportSendHomework สำหรับ กราฟแท่ง ของ Homework
                    InsertTmpStudentReportSendHomework(dt.Rows(index)("Student_Id").ToString(), Month, Year)
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection()
                    Return ex.ToString()
                    Exit Function
                End Try
            Next
            _DB.CommitTransection()
            Return "Complete"
        End If
    End Function

    'Insert tmp_StudentReportTotalActivePieChart
    Private Sub InsertStudentReportPieChart(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)
        Dim sql As String = " select COUNT(distinct case when tblQuiz.IsQuizMode = 1 then tblQuiz.Quiz_Id else null end) as TotalQuiz, " & _
                            " count(distinct case when tblQuiz.IsPracticeMode = 1 then tblQuiz.Quiz_Id else null end) as TotalPractice, " & _
                            " count(distinct case when tblQuiz.IsHomeWorkMode = 1 then tblQuiz.Quiz_Id else null end) as TotalHomework " & _
                            " from tblQuiz inner join tblQuizScore on tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id where " & _
                            " tblQuizScore.Student_Id = '" & StudentId & "' and  DATEPART(MONTH,tblQuiz.StartTime) = '" & Month & "' " & _
                            " and DATEPART(YEAR,tblQuiz.StartTime) = '" & Year & "' "
        Dim dt As New DataTable
        dt = _DB.getdataWithTransaction(sql)

        If dt.Rows.Count > 0 Then
            sql = " insert into tmp_StudentReportTotalActivePieChart(Student_Id,DateMonth,TotalQuiz,TotalPractice,TotalHomework) " & _
                  " values('" & StudentId & "',CAST('" & Year & "-" & Month & "-1' as SmallDatetime),'" & dt.Rows(0)("TotalQuiz") & "','" & dt.Rows(0)("TotalPractice") & "','" & dt.Rows(0)("TotalHomework") & "') "
            _DB.ExecuteWithTransection(sql)
        End If
    End Sub

    'Insert tmp_StudentReportQuizScorePerSubject
    Private Sub InsertStudentReportSpiderChart(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)
        'Dim sql As String = " SELECT round ((SUM(tblQuizScore.Score)  * 100) / SUM( uvw_FullScoreBySubjectPerQuiz.FullScore),2) as TotalScore , tblGroupSubject.GroupSubject_ShortName " & _
        '                    " FROM tblGroupSubject INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN " & _
        '                    " tblQuestionCategory INNER JOIN tblQuestionset ON tblQuestionCategory.QCategory_Id = tblQuestionset.QCategory_Id  " & _
        '                    " ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN uvw_FullScoreBySubjectPerQuiz INNER JOIN " & _
        '                    " tblQuiz ON uvw_FullScoreBySubjectPerQuiz.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
        '                    " tblQuestion ON tblQuizScore.Question_Id = tblQuestion.Question_Id ON tblGroupSubject.GroupSubject_Id = uvw_FullScoreBySubjectPerQuiz.GroupSubject_Id " & _
        '                    " WHERE (tblQuizScore.Student_Id = '" & StudentId & "') AND (tblQuizScore.IsActive = 1) AND (tblQuiz.IsQuizMode = 1) " & _
        '                    " and DATEPART(MONTH,tblQuiz.StartTime) = '" & Month & "' and DATEPART(YEAR,tblQuiz.StartTime) = '" & Year & "' " & _
        '                    " GROUP BY tblGroupSubject.GroupSubject_ShortName "
        Dim sql As String = " SELECT ROUND(StudentScore/FullScore,2)AS TotalScore,GroupSubject_ShortName FROM (SELECT SUM(tblQuizScore.Score) * 100 AS StudentScore " & _
                           " ,dbo.tblGroupSubject.GroupSubject_Id,dbo.tblQuiz.Quiz_Id  FROM tblGroupSubject INNER JOIN tblBook ON " & _
                           " tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN tblQuestionCategory INNER JOIN tblQuestionset " & _
                           " ON tblQuestionCategory.QCategory_Id = tblQuestionset.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN " & _
                           " tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN tblQuestion " & _
                           " ON tblQuizScore.Question_Id = tblQuestion.Question_Id ON tblQuestionset.QSet_Id = tblQuestion.QSet_Id WHERE " & _
                           " (tblQuizScore.Student_Id = '" & StudentId & "') AND (tblQuizScore.IsActive = 1) AND (tblQuiz.IsQuizMode = 1) " & _
                           " AND (DATEPART(MONTH, tblQuiz.StartTime) = '" & Month & "') AND (DATEPART(YEAR, tblQuiz.StartTime) = '" & Year & "') " & _
                           " GROUP BY dbo.tblGroupSubject.GroupSubject_Id,dbo.tblQuiz.Quiz_Id)AS tblxxx INNER JOIN dbo.uvw_FullScoreBySubjectPerQuiz " & _
                           " ON tblxxx.GroupSubject_Id = dbo.uvw_FullScoreBySubjectPerQuiz.GroupSubject_Id AND tblxxx.Quiz_Id = dbo.uvw_FullScoreBySubjectPerQuiz.Quiz_Id "
        Dim dt As New DataTable
        dt = _DB.getdataWithTransaction(sql)
        If dt.Rows.Count > 0 Then
            Dim Thai As Decimal = 0
            Dim English As Decimal = 0
            Dim Social As Decimal = 0
            Dim Health As Decimal = 0
            Dim Art As Decimal = 0
            Dim Math As Decimal = 0
            Dim Career As Decimal = 0
            Dim Science As Decimal = 0
            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("GroupSubject_ShortName") = "ภาษาไทย" Then
                    Thai = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "ศิลปะ" Then
                    Art = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "การงานฯ/เทคโนฯ" Then
                    Career = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "สุขศึกษา/พละฯ" Then
                    Health = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "ภาษาตปท." Then
                    English = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "สังคมฯ" Then
                    Social = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "วิทย์ฯ" Then
                    Science = dt.Rows(index)("TotalScore")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "คณิตฯ" Then
                    Math = dt.Rows(index)("TotalScore")
                End If
            Next
            sql = " insert into tmp_StudentReportTotalUsageBySubject(Student_Id,DayMonth,Thai,English,Social,Health,Art,Math,Careers,Science,IsQuiz,IsPractice) " & _
                  " values('" & StudentId & "',CAST('" & Year & "-" & Month & "-1' as Smalldatetime),'" & Thai & "','" & English & "','" & Social & "','" & Health & "','" & Art & "','" & Math & "','" & Career & "','" & Science & "','1','0') "
            _DB.ExecuteWithTransection(sql)
        End If
    End Sub

    'Insert tmp_StudentReportTotalUsageBySubject
    Private Sub InsertTmpStudentReportTotalUsageBySubject(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)
        Dim sql As String = " SELECT tblGroupSubject.GroupSubject_ShortName,SUM(CAST(DATEDIFF( SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) as decimal(10,2))/3600 ) as TotalTime " & _
                            " FROM tblGroupSubject INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN tblQuestionCategory INNER JOIN " & _
                            " tblQuestionset ON tblQuestionCategory.QCategory_Id = tblQuestionset.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN " & _
                            " tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN tblQuestion ON tblQuizScore.Question_Id = tblQuestion.Question_Id " & _
                            " ON tblQuestionset.QSet_Id = tblQuestion.QSet_Id WHERE (tblQuiz.IsPracticeMode = 1) AND (tblQuizScore.IsActive = 1) AND " & _
                            " (tblQuizScore.Student_Id = '" & StudentId & "') and DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) " & _
                            " between 0 and 10 and DATEPART(MONTH,tblQuizScore.FirstResponse) = '" & Month & "' and DATEPART(YEAR,tblQuizScore.FirstResponse) = '" & Year & " ' " & _
                            " group by tblGroupSubject.GroupSubject_ShortName "
        Dim dt As New DataTable
        dt = _DB.getdataWithTransaction(sql)
        If dt.Rows.Count > 0 Then
            Dim Thai As Decimal = 0
            Dim English As Decimal = 0
            Dim Social As Decimal = 0
            Dim Health As Decimal = 0
            Dim Art As Decimal = 0
            Dim Math As Decimal = 0
            Dim Career As Decimal = 0
            Dim Science As Decimal = 0
            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("GroupSubject_ShortName") = "ภาษาไทย" Then
                    Thai = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "ศิลปะ" Then
                    Art = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "การงานฯ/เทคโนฯ" Then
                    Career = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "สุขศึกษา/พละฯ" Then
                    Health = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "ภาษาตปท." Then
                    English = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "สังคมฯ" Then
                    Social = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "วิทย์ฯ" Then
                    Science = dt.Rows(index)("TotalTime")
                ElseIf dt.Rows(index)("GroupSubject_ShortName") = "คณิตฯ" Then
                    Math = dt.Rows(index)("TotalTime")
                End If
            Next
            sql = " insert into tmp_StudentReportTotalUsageBySubject(Student_Id,DayMonth,Thai,English,Social,Health,Science,Art,Math,Careers,IsQuiz,IsPractice) " & _
                  " values('" & StudentId & "',CAST('" & Year & "-" & Month & "-1' as Smalldatetime),'" & Thai & "','" & English & "','" & Social & "','" & Health & "','" & Science & "','" & Art & "','" & Math & "','" & Career & "' ,'0','1') "
            _DB.ExecuteWithTransection(sql)
        End If
    End Sub

    'Insert tmp_StudentReportSendHomework
    Private Sub InsertTmpStudentReportSendHomework(ByVal StudentId As String, ByVal Month As String, ByVal Year As String)

        Dim HomeworkCompleteAndBeforeDeadline As String = "" 'ส่งครบทัน
        Dim HomeworkNotComplete As String = "" 'ทำไม่ครบ
        Dim HomeworkNotSend As String = "" 'ไม่ส่ง

        'Dim sql As String = " SELECT COUNT(distinct case when tblModuleDetailCompletion.TimeExitedByUser is null then tblModule.Module_Id else null end) as HomeworkNotSend " & _
        '                    " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
        '                    " tblModuleAssignmentDetail ON tblModuleAssignment.MA_Id = tblModuleAssignmentDetail.MA_Id INNER JOIN " & _
        '                    " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN tblModuleDetailCompletion ON tblModuleAssignment.MA_Id " & _
        '                    " = tblModuleDetailCompletion.MA_Id where tblModuleDetailCompletion.Student_Id = '" & StudentId & "' " & _
        '                    " and DATEPART(MONTH,tblModuleAssignment.End_Date) = '" & Month & "' and DATEPART(YEAR,tblModuleAssignment.End_Date) = '" & Year & "' "
        'HomeworkCompleteAndBeforeDeadline = _DB.ExecuteScalarWithTransection(sql)

        'sql = " SELECT COUNT(distinct case when DATEDIFF(HOUR,tblModuleAssignment.End_Date,tblModuleDetailCompletion.TimeExitedByUser) > 3 " & _
        '      " then tblModule.Module_Id else null end ) as SendBeforeEnddate, COUNT(distinct case when DATEDIFF(HOUR,tblModuleAssignment.End_Date,tblModuleDetailCompletion.TimeExitedByUser)" & _
        '      "  <= 3 then tblModule.Module_Id else null end) as SendInTime FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = " & _
        '      " tblModuleAssignment.Module_Id INNER JOIN tblModuleAssignmentDetail ON tblModuleAssignment.MA_Id = tblModuleAssignmentDetail.MA_Id INNER JOIN " & _
        '      " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = " & _
        '      " tblModuleDetailCompletion.MA_Id where tblModuleDetailCompletion.Student_Id = '" & StudentId & "' " & _
        '      " and DATEPART(MONTH,tblModuleAssignment.End_Date) = '" & Month & "' and DATEPART(YEAR,tblModuleAssignment.End_Date) = '" & Year & "' and " & _
        '      " (tblModuleDetailCompletion.TimeExitedByUser is not null)"
        'Dim dt As New DataTable
        'dt = _DB.getdataWithTransaction(sql)
        'If dt.Rows.Count > 0 Then
        '    HomeworkSendBeforeEndTime = dt.Rows(0)("SendBeforeEnddate")
        '    HomeworkSendInTime = dt.Rows(0)("SendInTime")
        'End If

        'ส่งครบทัน
        Dim sql As String = " SELECT COUNT(*) FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
                            " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id " & _
                            " WHERE (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND (DATEPART(MONTH, tblModuleAssignment.End_Date) = '" & Month & "') AND " & _
                            " (DATEPART(YEAR, tblModuleAssignment.End_Date) = '" & Year & "') AND (dbo.tblModuleDetailCompletion.Module_Status = 3) " & _
                            " AND (dbo.tblModuleDetailCompletion.TimeExitedByUser IS NOT NULL) AND dbo.tblModuleDetailCompletion.IsActive = 1 AND dbo.tblModule.IsActive = 1 "
        HomeworkCompleteAndBeforeDeadline = _DB.ExecuteScalarWithTransection(sql)

        'ทำไม่ครบ
        sql = " SELECT COUNT(*) FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
              " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id WHERE " & _
              " (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND (DATEPART(MONTH, tblModuleAssignment.End_Date) = '" & Month & "') AND " & _
              " (DATEPART(YEAR, tblModuleAssignment.End_Date) = '" & Year & "') AND (dbo.tblModuleDetailCompletion.Module_Status = 1 OR dbo.tblModuleDetailCompletion.Module_Status = 2) " & _
              " AND dbo.tblModuleDetailCompletion.IsActive = 1 AND dbo.tblModule.IsActive = 1 "
        HomeworkNotComplete = _DB.ExecuteScalarWithTransection(sql)

        'ไม่ส่ง
        sql = " SELECT COUNT(*) FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
              " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id " & _
              " WHERE (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND DATEPART(MONTH, tblModuleAssignment.End_Date) = '" & Month & "' " & _
              " AND DATEPART(YEAR, tblModuleAssignment.End_Date) = '" & Year & "' AND dbo.tblModuleDetailCompletion.IsActive = 1 " & _
              " AND dbo.tblModule.IsActive = 1 AND dbo.tblModuleDetailCompletion.TimeExitedByUser IS NULL "
        HomeworkNotSend = _DB.ExecuteScalarWithTransection(sql)

        sql = " insert into tmp_StudentReportSendHomework(Student_Id,DayMonth,CompleteAndBeforeDeadline,NotComplete,NotSend) " & _
              " values('" & StudentId & "',CAST('" & Year & "-" & Month & "-1' as smalldatetime),'" & HomeworkCompleteAndBeforeDeadline & "','" & HomeworkNotComplete & "','" & HomeworkNotSend & "') "
        _DB.ExecuteWithTransection(sql)
    End Sub

#End Region

    'Function หาจำนวนชั่วโมงของเดือนที่แล้ว
    Private Function GetPreviousMonthData(ByVal SchoolId As String, ByVal PreviousMonth As String, ByVal Year As String, ByRef ObjDb As ClassConnectSql)
        Dim sql As String = " select TotalHour from tmp_SchoolReportUse where DATEPART(MONTH,MonthDate) = '" & PreviousMonth & "' and DATEPART(YEAR,MonthDate) = '" & Year & "' and SchoolId = '" & SchoolId & "' "
        Dim PreviousMonthData As String = ObjDb.ExecuteScalarWithTransection(sql)
        If PreviousMonthData = "" Then
            PreviousMonthData = 0
        End If
        Return PreviousMonthData
    End Function

    'Function เช็คว่าปีเป็น พ.ศ. หรือเปล่า ถ้าเป็น พ.ศ. ต้องแปลงกลับเป็น ค.ศ.
    Private Function CleanFormatCalendar(ByVal Year As String) As Integer
        If CInt(Year) > 2500 Then
            Year -= 543
            Return Year
        Else
            If CInt(Year) < 2000 Then
                Year += 543
            Else
                Return Year
            End If
        End If
    End Function

    'Function หานักเรียนทั้งหมดในโรงเรียน
    Private Function GetDtStudent(ByVal SchoolId As String) As DataTable
        Dim sql As String = " select  Student_Id from t360_tblStudent where School_Code = '" & SchoolId & "' and Student_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function


End Class
