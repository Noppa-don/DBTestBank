Imports BusinessTablet360
Imports KnowledgeUtils

Public Class TestsetHeaderControl
    Inherits System.Web.UI.UserControl
    Dim _DB As New ClassConnectSql()
    Dim ClsActivity As New ClsActivity(New ClassConnectSql())
    Dim ClsHomeWork As New ClsHomework(New ClassConnectSql())
    Dim ClsPractice As New ClsPracticeMode(New ClassConnectSql())

    'Enum EnumDashBoardType
    '    Quiz = 1
    '    Practice = 2
    '    Homework = 3
    'End Enum

    'Dim _StudentId As String = ""
    'Public WriteOnly Property StudentId As String
    '    Set(ByVal value As String)
    '        _StudentId = value
    '    End Set
    'End Property

    'Dim _QuizId As String
    'Public WriteOnly Property QuizId As String
    '    Set(ByVal value As String)
    '        _QuizId = value
    '    End Set
    'End Property

    'Quiz
    'QuizId  = "F45A9B8D-C76B-4ACD-8EFC-EDF1959A1E44"
    'Student = "4C12E915-84DB-4E5C-A9B5-00E57A84CF02"

    'Practice
    'Quiid = "284715FC-C4E4-47BD-A9CA-00E0C097811F"
    'StudentId = ""

    'Homework
    'QuizId = "09E975BB-5ED6-4410-B30E-073B2F944DEE"
    'StudentId = "DFA6AC84-610E-4024-955F-8BD47FA8D043"
    'quizid : 09E975BB-5ED6-4410-B30E-073B2F944DEE
    'module_id : 81E85318-7CF2-409F-B0C7-9FEF16FE99A8
    'moduleDetail_id : D0B14BE6-8EB9-4640-8744-F75EB1A82830
    'module AssignMent : DF6F1F13-673C-490B-8F64-256D61C70118
    'testset : FB083ABA-EDA1-4026-A92B-1E076449880A

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ProcessByQuizId()

    End Sub

    Public Sub SetByQuizId(ByVal Quiz_Id As String, ByVal StudentId As String)

        ProcessByQuizId(Quiz_Id, StudentId)

    End Sub

    Public Sub SetByTestSetId(ByVal TestSetId As String)

        ProcessByTestSetId(TestSetId)

    End Sub

    ''' <summary>
    ''' ทำการ set ข้อมูลแบบการบ้าน โดยใช้ MAID จาก tblModuleAssignment
    ''' </summary>
    ''' <param name="MAID"></param>
    ''' <remarks></remarks>
    Public Sub SetByMaId(ByVal MAID As String)

        ProcessByMAID(MAID)

    End Sub

    Private Sub ProcessByQuizId(ByVal Quiz_Id As String, ByVal StudentId As String)

        Dim sql As String = " SELECT IsQuizMode,IsPracticeMode,IsHomeWorkMode FROM dbo.tblQuiz WHERE Quiz_Id = '" & Quiz_Id & "' "
        Dim dt As New DataTable
        Dim CheckType As String = ""
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("IsQuizMode") = True Then
                ProcessByType(EnumDashBoardType.Quiz, Quiz_Id, StudentId)
            ElseIf dt.Rows(0)("IsPracticeMode") = True Then
                ProcessByType(EnumDashBoardType.Practice, Quiz_Id, StudentId)
            ElseIf dt.Rows(0)("IsHomeWorkMode") = True Then
                ProcessByType(EnumDashBoardType.Homework, Quiz_Id, StudentId)
            End If
        End If

    End Sub

    Public Sub ProcessByTestSetId(ByVal TestSetId As String)

        Dim HeaderTxt As String = GetTestSetNameByTestSetId(TestSetId)
        CreateRow1(HeaderTxt)

        Dim ArrDetail As ArrayList = GetArrSubjectAndLevelByTestSetIdOrQuizId(TestSetId, True)
        If ArrDetail.Count > 0 Then
            CreateRow3(ArrDetail)
        End If

    End Sub

    ''' <summary>
    ''' ทำการหาชื่อการบ้าน , และรายละเอียดต่างๆของการบ้านที่ set ค่าไว้ จาก MAID จาก tblModuleAssignment
    ''' </summary>
    ''' <param name="MAID">Id ของการบ้านที่เลือก tblModuleAssignment</param>
    ''' <remarks></remarks>
    Public Sub ProcessByMAID(ByVal MAID As String)

        Dim HeaderTxt As String = GetTestSetNameByMAID(MAID)
        CreateRow1(HeaderTxt)

        Dim ArrDetail As ArrayList = GetArrHomeworkDetailByMAID(MAID)
        If ArrDetail.Count > 0 Then
            CreateRow3(ArrDetail)
        End If

    End Sub

    Private Sub ProcessByType(ByVal TypeForAccess As EnumDashBoardType, ByVal QuizId As String, ByVal StudentId As String)

        Dim HeaderTxt As String = ClsActivity.GetTestsetName(QuizId)
        CreateRow1(HeaderTxt)
        Dim Detailtxt As String = ""
        If StudentId <> "" Then
            CreateStringRow2ByMode(TypeForAccess, QuizId, StudentId)
        End If
        Dim ArrQuizSetting As New ArrayList
        If TypeForAccess = EnumDashBoardType.Quiz Then
            If StudentId = "" Then
                ArrQuizSetting = ClsActivity.GetArrayQuizProperty(QuizId, False)
            Else
                ArrQuizSetting = ClsActivity.GetArrayQuizProperty(QuizId, True)
            End If

        ElseIf TypeForAccess = EnumDashBoardType.Practice Then
            ArrQuizSetting = GetArrSubjectAndLevelByTestSetIdOrQuizId(QuizId, False)
        ElseIf TypeForAccess = EnumDashBoardType.Homework Then
            ArrQuizSetting = ClsHomeWork.GetArrPropertyHomework(QuizId)
        End If

        If ArrQuizSetting.Count > 0 Then
            CreateRow3(ArrQuizSetting)
        End If

    End Sub


    ''' <summary>
    ''' ทำการนำข้อมูลที่ได้มา append เข้าไปใน Div ซึ่งจะเป็นพวก ชื่อการบ้าน , ชื่อชุดข้อสอบ , ชื่อชุดที่ฝึกฝน
    ''' </summary>
    ''' <param name="Header"></param>
    ''' <remarks></remarks>
    Private Sub CreateRow1(ByVal Header As String)

        DivRow1.InnerHtml = Header

    End Sub

    Private Sub CreateRow2(ByVal DetailString As String)

        DivRow2.InnerHtml = DetailString

    End Sub

    ''' <summary>
    ''' ทำการสร้าง div รายละเอียดก้อนสีส้มเล็กๆ บอกรายละเอียดต่างๆตามข้อมูลใน Array ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="DetailArray">Array ที่เก็บข้อมูลเอาไว้แบ่งตาม mode ควิซ,การบ้าน,ฝึกฝน</param>
    ''' <remarks></remarks>
    Private Sub CreateRow3(ByVal DetailArray As ArrayList)

        Dim Sb As New StringBuilder
        For Each r In DetailArray
            Sb.Append("<div Class='ForSmallDetailDiv' >")
            Sb.Append(r)
            Sb.Append("</div>")
        Next
        Sb.Append("<div style='clear:both;'></div>")
        DivRow3.InnerHtml = Sb.ToString()

    End Sub

#Region "Function & Query"

    Private Function GetTestSetNameByTestSetId(ByVal TestSetId As String)

        Dim sql As String = " SELECT TestSet_Name FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestSetId & "' AND IsActive = 1 "
        Dim TestSetName As String = _DB.ExecuteScalar(sql)
        Return TestSetName

    End Function

    Private Sub CreateStringRow2ByMode(ByVal TypeForAccess As EnumDashBoardType, ByVal QuizId As String, ByVal StudentId As String)

        Dim Detailtxt As String = ""

        Dim dt As New DataTable

        If TypeForAccess = EnumDashBoardType.Quiz Then 'ถ้าเป็น Quiz จะมีรูปแบบแค่ ไม่ได้ทำ,ทำครบ,ทำไม่ครบ
            dt = CreateDtStringRow2ForQuizMode(QuizId, StudentId)
        ElseIf TypeForAccess = EnumDashBoardType.Practice Then 'ถ้าเป็นฝึกฝน มีรุปแบบแค่ ทำไม่ครบ,ทำครบ
            dt = CreateDtStringRow2ForPracticeMode(QuizId, StudentId)
        ElseIf TypeForAccess = EnumDashBoardType.Homework Then 'ถ้าเป้นการบ้าน จะมีหลาย pattern ทำครบ - ส่งแล้ว,ทำครบ - ยังไม่ส่ง,ทำไม่ครบ - ส่งแล้ว,ทำไม่ครบ - กำลังทำอยู่/ไม่ได้กดส่ง,ไม่เข้าทำ
            dt = CreateDtStringRow2ForHomeworkMode(QuizId, StudentId)
        End If
        Dim ClsAc As New ClsActivity(New ClassConnectSql)
        Dim IsFully As String = ""
        If dt.Rows.Count > 0 Then

            'IsFully = CreateStringInBracket(dt, TypeForAccess)
            If TypeForAccess = EnumDashBoardType.Homework Then
                IsFully = ClsAc.GetStatusQuiz(QuizId, StudentId, TypeForAccess, dt.Rows(0)("TimeExitedByUser").ToString)
            Else
                IsFully = ClsAc.GetStatusQuiz(QuizId, StudentId, TypeForAccess)
            End If


            Dim Score As String = dt.Rows(0)("Score").ToString()

            Dim StartTime As String
            If dt.Rows(0)("StartTime") IsNot DBNull.Value Then
                StartTime = CDate(dt.Rows(0)("StartTime")).ToPointPlusTime
            Else
                StartTime = "-"
            End If

            Dim TimeTotal As Integer = CInt(dt.Rows(0)("TotalTimeDoQuiz"))
            Dim ResultTime As String = ""
            If TimeTotal <> 0 Then
                ResultTime = ClsAc.GetStringDurationTime(TimeTotal)
            End If

            Detailtxt = "ได้ " & Score.ToString().ToPointplusScore() & " (" & IsFully & ") "

            If TypeForAccess = EnumDashBoardType.Homework Then
                'เข้าทำ
                Detailtxt &= " เริ่มทำ " & StartTime
                'ถ้ากดตอบจะมีเวลาทำไป
                If TimeTotal <> 0 Then
                    Detailtxt &= " ใช้เวลาทำไป " & ResultTime
                    If dt.Rows(0)("TimeExitedByUser") IsNot DBNull.Value Then
                        Detailtxt &= " ส่ง " & CDate(dt.Rows(0)("TimeExitedByUser")).ToPointPlusTime
                    Else
                        Detailtxt &= " ส่ง -"
                    End If
                ElseIf dt.Rows(0)("TimeExitedByUser") IsNot DBNull.Value Then
                    Detailtxt &= " ใช้เวลาทำไป - ส่ง " & CDate(dt.Rows(0)("TimeExitedByUser")).ToPointPlusTime
                End If

            Else
                Detailtxt &= " เข้าทำเมื่อ " & StartTime
                If TimeTotal <> 0 Then
                    Detailtxt &= " ใช้เวลาทำไป " & ResultTime
                End If
            End If
        Else
            Dim FullScore As String = _DB.ExecuteScalar("select FullScore from tblquiz where Quiz_Id = '" & QuizId & "';")

            Detailtxt = "ได้ 0/" & FullScore.ToPointplusScore() & " (นร.ยังไม่ได้เข้าทำเลย)"

        End If
        CreateRow2(Detailtxt)

    End Sub

    Private Function CreateStringInBracket(ByVal dt As DataTable, ByVal Mode As EnumDashBoardType)

        Dim StringReturn As String = ""

        If Mode = EnumDashBoardType.Quiz Then 'ถ้าเป็น Quiz จะมีรูปแบบแค่ ไม่ได้ทำ,ทำครบ,ทำไม่ครบ
            If dt.Rows(0)("ResultIsDone") > 0 Then
                If dt.Rows(0)("SessionStatus") = 0 Then
                    StringReturn = "ทำไม่ครบ"
                Else
                    StringReturn = "ทำครบ"
                End If
            Else
                StringReturn = "ไม่ได้ทำ"
            End If
        ElseIf Mode = EnumDashBoardType.Practice Then 'ถ้าเป็นฝึกฝน มีรุปแบบแค่ ทำไม่ครบ,ทำครบ
            If dt.Rows(0)("SessionStatus") = True Then
                StringReturn = "ทำครบ"
            Else
                StringReturn = "ทำไม่ครบ"
            End If
        ElseIf Mode = EnumDashBoardType.Homework Then 'ถ้าเป้นการบ้าน จะมีหลาย pattern ทำครบ - ส่งแล้ว,ทำครบ - ยังไม่ส่ง,ทำไม่ครบ - ส่งแล้ว,ทำไม่ครบ - กำลังทำอยู่/ไม่ได้กดส่ง,ไม่เข้าทำ
            If dt.Rows(0)("Module_Status") = 0 Then 'เช็คก่อนว่าทำหรือเปล่า
                StringReturn = "ไม่เข้าทำ"
            Else 'ถ้าเข้าทำค่อยไปเช็คเรื่องทำครบไม่ครบ และ ส่งไม่ส่ง 
                'ทำครบ - ไม่ครบ
                If dt.Rows(0)("SessionStatus") = 1 Then 'ถ้าทำครบ
                    StringReturn = "ทำครบ "
                    'เช็คว่า ส่งหรือยัง
                    If dt.Rows(0)("TimeExitedByUser") IsNot DBNull.Value Then
                        StringReturn &= " - ส่งแล้ว"
                    Else
                        StringReturn &= " - ยังไม่ส่ง"
                    End If
                Else 'ถ้าทำไม่ครบ
                    StringReturn = "ทำไม่ครบ "
                    'เช็คว่า ส่งหรือยัง
                    If dt.Rows(0)("TimeExitedByUser") IsNot DBNull.Value Then
                        StringReturn &= " - ส่งแล้ว"
                    Else
                        If dt.Rows(0)("IsEnd") = 1 Then  'ถ้าจบไปแล้ว
                            StringReturn &= " - ยังไม่ได้กดส่ง"
                        Else 'ถ้ายังไม่จบ
                            StringReturn &= " - กำลังทำอยู่"
                        End If
                    End If
                End If
            End If
        End If

        Return StringReturn

    End Function

    Private Function CreateDtStringRow2ForQuizMode(ByVal QuizId As String, ByVal StudentId As String)

        Dim sql As String = " SELECT CAST(tblQuizSession.TotalScore AS VARCHAR(MAX)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(MAX)) AS Score, " & _
                            " tblQuizSession.SessionStatus,Sum(tblquizscore.TimeTotal) as TotalTimeDoQuiz, tblQuiz.StartTime, " & _
                            " SUM(CASE WHEN dbo.tblQuizScore.Answer_Id IS NOT NULL THEN 1 ELSE 0 END) AS ResultIsDone " & _
                            " ,dbo.tblQuizSession.SessionStatus FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                            " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND tblQuizScore.Student_Id = tblQuizSession.Player_Id " & _
                            " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND (tblQuizScore.Student_Id = '" & StudentId & "') " & _
                            " GROUP BY CAST(tblQuizSession.TotalScore AS VARCHAR(MAX)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(MAX)), tblQuizSession.SessionStatus, " & _
                            " tblQuiz.StartTime "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function CreateDtStringRow2ForPracticeMode(ByVal QuizId As String, ByVal StudentId As String)

        Dim sql As String = " SELECT ( CAST(tblQuizSession.TotalScore AS VARCHAR(max)) + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) AS Score, " & _
                            " tblQuizSession.SessionStatus,dbo.tblQuiz.StartTime, Sum(tblquizscore.TimeTotal) as TotalTimeDoQuiz, tblQuiz.StartTime " & _
                            " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                            " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id AND tblQuizScore.Student_Id = tblQuizSession.Player_Id " & _
                            " WHERE  (tblQuiz.Quiz_Id = '" & QuizId & "') AND " & _
                            " (tblQuizScore.Student_Id = '" & StudentId & "') " & _
                            " GROUP BY ( CAST(tblQuizSession.TotalScore AS VARCHAR(max)) + '/' + CAST(dbo.tblQuiz.FullScore AS VARCHAR(MAX)) ) " & _
                            " , tblQuizSession.SessionStatus,dbo.tblQuiz.StartTime "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function CreateDtStringRow2ForHomeworkMode(ByVal QuizId As String, ByVal StudentId As String)

        Dim sql As String = " SELECT CAST(tblQuizSession.TotalScore AS VARCHAR(MAX)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(MAX)) AS Score, tblModuleDetailCompletion.TimeExitedByUser, " &
                            " CASE WHEN dbo.GetThaiDate() > tblModuleAssignment.End_Date THEN 1 ELSE 0 END AS IsEnd " &
                            " , tblModuleDetailCompletion.Module_Status, min(tblQuizScore.FirstResponse) as StartTime, tblQuizSession.SessionStatus" &
                            " , Sum(tblQuizScore.TimeTotal) AS TotalTimeDoQuiz  FROM tblQuizScore INNER JOIN tblQuiz ON tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id INNER JOIN " &
                            " tblModuleDetailCompletion ON tblQuizScore.Student_Id = tblModuleDetailCompletion.Student_Id AND  " &
                            " tblQuizScore.Quiz_Id = tblModuleDetailCompletion.Quiz_Id INNER JOIN " &
                            " tblModuleAssignment ON tblModuleDetailCompletion.MA_Id = tblModuleAssignment.MA_Id LEFT OUTER JOIN " &
                            " tblQuizSession ON tblModuleDetailCompletion.Quiz_Id = tblQuizSession.Quiz_Id AND tblModuleDetailCompletion.Student_Id = tblQuizSession.Player_Id " &
                            " WHERE (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND  " &
                            " (tblModuleDetailCompletion.Quiz_Id = '" & QuizId & "') " &
                            " GROUP BY  CAST(tblQuizSession.TotalScore AS VARCHAR(MAX)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(MAX)) , tblModuleDetailCompletion.TimeExitedByUser,  " &
                            " tblModuleAssignment.End_Date, tblModuleDetailCompletion.Module_Status, tblQuizSession.LastUpdate, tblQuizSession.SessionStatus "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GetStringDurationTime(ByVal StartDate As DateTime, ByVal EndUpdate As DateTime)

        Dim StrReturn As String = ""
        Dim DoneHour As Integer = 0
        Dim DoneMinte As Integer = 0

        'Diff Hour 
        DoneHour = DateDiff(DateInterval.Hour, StartDate, EndUpdate)

        If DoneHour > 0 Then
            StartDate = StartDate.AddHours(DoneHour)
        End If

        'Diff Minute
        DoneMinte = DateDiff(DateInterval.Minute, StartDate, EndUpdate)

        StrReturn = "ทำไป "

        If DoneHour > 0 Then
            StrReturn &= DoneHour & " ชม. "
        End If

        If DoneMinte > 0 Then
            StrReturn &= DoneMinte & " นาที"
        End If

        If DoneHour = 0 And DoneMinte = 0 Then
            StrReturn = ""
        End If

        Return StrReturn

    End Function

    Private Function GetArrSubjectAndLevelByTestSetIdOrQuizId(ByVal InputId As String, ByVal IsTestSetId As Boolean)

        Dim ClsViewReport As New ClassViewReport(New ClassConnectSql())
        Dim ArrReturn As New ArrayList
        Dim Sql As String = ""
        If IsTestSetId = True Then
            Sql = " SELECT tblLevel.Level_Name, tblGroupSubject.GroupSubject_Name " & _
                  " FROM tblGroupSubject INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN " & _
                  " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN tblTestSet INNER JOIN tblTestSetQuestionSet ON " & _
                  " tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN tblQuestionSet ON " & _
                  " tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN tblQuestionCategory ON " & _
                  " tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                  " WHERE (tblTestSet.TestSet_Id = '" & InputId & "') AND (tblQuestionSet.IsActive = 1) AND tblTestsetQuestionSet.IsActive = '1'  " & _
                  " GROUP BY tblLevel.Level_Name, tblGroupSubject.GroupSubject_Name "
            Dim dt As New DataTable
            dt = _DB.getdata(Sql)
            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    Dim SubjectName As String = ClsViewReport.GenSubjectName(dt.Rows(index)("GroupSubject_Name").ToString())
                    Dim LevelName As String = ClsPractice.GetShortLevelNameByLongLevelName(dt.Rows(index)("Level_Name").ToString())
                    ArrReturn.Add(SubjectName & "  " & LevelName)
                Next
            End If
            ClsViewReport = Nothing
            Return ArrReturn
        Else
            ArrReturn = ClsPractice.GetArrPropertyByQuizId(InputId)
            Return ArrReturn
        End If

    End Function

    ''' <summary>
    ''' ทำการหาชื่อการบ้าน
    ''' </summary>
    ''' <param name="MAID">ModuleAssignmentId ที่เลือกมา</param>
    ''' <returns>ชื่อการบ้าน</returns>
    ''' <remarks></remarks>
    Private Function GetTestSetNameByMAID(ByVal MAID As String)

        Dim sql As String = " SELECT tblTestSet.TestSet_Name FROM tblModuleDetail INNER JOIN " & _
                            " tblModuleAssignment ON tblModuleDetail.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " & _
                            " tblTestSet ON tblModuleDetail.Reference_Id = tblTestSet.TestSet_Id " & _
                            " WHERE (tblModuleAssignment.MA_Id = '" & MAID & "') "
        Dim TestSetName As String = _DB.ExecuteScalar(sql)
        Return TestSetName

    End Function

    ''' <summary>
    ''' ทำการหารายละเอียดต่างๆของการบ้านที่เลือกมา ชั้น/ห้อง , ชื่อนักเรียน ในกรณีที่สั่งเป็นรายคนด้วย
    ''' </summary>
    ''' <param name="MAID">Id ของ tblModuleAssignment ที่เลือกมา</param>
    ''' <returns>Array ที่มีรายละเอียดของการบ้านอยู่</returns>
    ''' <remarks></remarks>
    Private Function GetArrHomeworkDetailByMAID(ByVal MAID As String) As ArrayList

        Dim ClsHomework As New ClsHomework(New ClassConnectSql())
        Dim ArrHomework As New ArrayList
        Dim sql As String = " SELECT Start_Date,End_Date FROM dbo.tblModuleAssignment WHERE MA_Id = '" & MAID & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim StrDatetxt As String = ""
        If dt.Rows.Count > 0 Then
            StrDatetxt = ClsHomework.CheckDateDuration(dt.Rows(0)("Start_Date"), dt.Rows(0)("End_Date"))
            ArrHomework.Add(StrDatetxt)
        End If

        sql = " SELECT tblModuleAssignmentDetail.Class_Name, t360_tblRoom.Class_Name + '' + t360_tblRoom.Room_Name AS ClassRoomName , " & _
              " t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName AS StudentName  FROM tblModuleAssignmentDetail LEFT OUTER JOIN " & _
              " t360_tblRoom ON tblModuleAssignmentDetail.Room_Id = t360_tblRoom.Room_Id LEFT OUTER JOIN " & _
              " t360_tblStudent ON tblModuleAssignmentDetail.Student_Id = t360_tblStudent.Student_Id WHERE (tblModuleAssignmentDetail.MA_Id = '" & MAID & "') " & _
              "  AND (tblModuleAssignmentDetail.IsActive = 1) order by studentname,dbo.FixedLengthClassAndRoom(t360_tblRoom.Class_Name,t360_tblRoom.Room_Name)"

        dt.Clear()
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            'loop เพื่อ หารายละเอียด ห้อง/ชั้น , ชื่อนักเรียน เพื่อ add เข้าไปใน Array , วนจนครบทุก record ใน datatable
            For index = 0 To dt.Rows.Count - 1
                If dt.Rows(index)("Class_Name") IsNot DBNull.Value Then
                    ArrHomework.Add(dt.Rows(index)("Class_Name").ToString())
                ElseIf dt.Rows(index)("ClassRoomName") IsNot DBNull.Value Then
                    ArrHomework.Add(dt.Rows(index)("ClassRoomName"))
                ElseIf dt.Rows(index)("StudentName") IsNot DBNull.Value Then
                    ArrHomework.Add(dt.Rows(index)("StudentName"))
                End If
            Next
        End If

        Return ArrHomework

    End Function

#End Region



End Class