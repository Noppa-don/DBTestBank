Imports Telerik.Web.UI
Imports BusinessTablet360
Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class GridDetailStudentControl
    Inherits System.Web.UI.UserControl

    Dim ClsActivity As New Service.ClsActivity(New ClassConnectSql())
    Dim ClsPractice As New Service.ClsPracticeMode(New ClassConnectSql())
    Dim ClsHomework As New Service.ClsHomework(New ClassConnectSql())
    Protected IsAndroid As Boolean

    Property DeviceuniqueId As String
    Property Token As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

    End Sub

#Region "Function & Query"


    'สร้าง Gird ประวัติควิซ 
    Public Sub CreateGridQuizHistory(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByVal IsForStudent As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim dt As DataTable = ClsActivity.GetDtQuizInfoByStudentId(StudentId, CalendarId, TeacherId, InputConn, Session("TimeMode"))
        Dim dtResult As DataTable = CreateDtBindGridByMode("QuizHistory")

        If dt.Rows.Count > 0 Then
            CreateDynamicColumnGridByMode("QuizHistory")

            For index = 0 To dt.Rows.Count - 1
                If IsForStudent = False Then
                    dtResult.Rows.Add(index)("TestSet_Name") = "<div><div class='DivForQuiz'><a style='cursor:pointer;' onclick=""ShowTestSetDetailPage('" & dt.Rows(index)("Quiz_Id").ToString() & "','" & StudentId & "')"">" & dt.Rows(index)("TestSet_Name") & "</a></div><div class='TestForIcon'><img src='../Images/ShowHomeworkDetail.png' class='ForImg' onclick=""ShowUsageScorePage('" & dt.Rows(index)("Quiz_Id").ToString() & "|" & StudentId & "','Quiz')"" /></div></div>"
                Else
                    dtResult.Rows.Add(index)("TestSet_Name") = dt.Rows(index)("TestSet_Name")
                End If
                dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime()
                'If IsForStudent = False Then
                '    dtResult.Rows(index)("Score") = "<div><a style='cursor:pointer;' onclick=""ShowChartStudentInfo('Quiz','" & StudentId & "','" & dt.Rows(index)("Quiz_Id").ToString() & "')"">" & dt.Rows(index)("Score").ToString().ToPointplusScore() & "</a></div>"
                'Else
                dtResult.Rows(index)("Score") = dt.Rows(index)("Score").ToString().ToPointplusScore()
                'End If
            Next
            ForDivNodata.Style.Add("display", "none")
            MainDiv.Visible = True
            RadGrid1.Visible = True
            RadGrid1.DataSource = dtResult
            RadGrid1.DataBind()
        Else
            RadGrid1.Visible = False
            CreateDivNoData("ยังไม่ได้เข้าทำควิซค่ะ")
        End If

    End Sub

    'สร้าง Grid การบ้าน
    Public Sub CreateGridHomework(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByVal IsForStudent As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByRef TimeMode As String = Nothing)

        'ดึงข้อมูลประวัติ
        Dim dt As DataTable = If((ClsKNSession.IsMaxONet), ClsHomework.GetMaxonetActivity(StudentId, CalendarId), ClsHomework.GetDtHomeByStudentCalendarAndTeacherId(StudentId, CalendarId, TeacherId, InputConn, Session("TimeMode")))
        'สร้าง dt สำหรับเก็บข้อมูลประวัติที่ทำการปรับแล้ว
        Dim dtResult As DataTable = CreateDtBindGridByMode("Homework")

        If dt.Rows.Count > 0 Then

            'สร้าง column Grid
            CreateDynamicColumnGridByMode("Homework")

            Dim listItemName As New List(Of String)
            'วนปรับข้อมูล
            For index = 0 To dt.Rows.Count - 1

                If IsForStudent = False Then
                    'ครู
                    dtResult.Rows.Add(index)("TestSet_Name") = "<div><div class='DivForHomework'><a style='cursor:pointer;' 
                                                                onclick=""ShowTestSetDetailPage('" & dt.Rows(index)("Quiz_Id").ToString() & "','" &
                                                                StudentId & "')"">" & dt.Rows(index)("TestSet_Name") & "</a></div><div class='TestForIcon'>
                                                                <img src='../Images/ShowHomeworkDetail.png' class='ForImg' 
                                                                onclick=""ShowUsageScorePage('" & dt.Rows(index)("MA_Id").ToString() & "|" & StudentId & "|" &
                                                                dt.Rows(index)("Quiz_Id").ToString() & "','Homework')"" /></div></div>"
                Else
                    'นักเรียน
                    If ClsKNSession.IsMaxONet Then
                        Dim tName As String = dt.Rows(index)("TestSet_Name").ToString().Replace("DA_", "")
                        tName = tName & "_" & dt.Rows(index)("Class_Name").ToString()
                        dtResult.Rows.Add(index)("TestSet_Name") = tName
                        listItemName.Add(tName)
                        dtResult.Rows(index)("StartDateTime") = dt.Rows(index)("StartDateTime").ToString()
                        dtResult.Rows(index)("Quiz_Id") = dt.Rows(index)("Quiz_Id").ToString()
                        dtResult.Rows(index)("Testset_Id") = dt.Rows(index)("Testset_Id").ToString()
                        dtResult.Rows(index)("ActivityPreview") = "<img src='../images/preview.png' class='imgPreview' id='" & dt.Rows(index)("Quiz_Id").ToString() & "' />"
                    Else
                        dtResult.Rows.Add(index)("TestSet_Name") = dt.Rows(index)("TestSet_Name")
                    End If
                End If

                dtResult.Rows(index)("Score") = dt.Rows(index)("Score").ToString().ToPointplusScore()

                'Dim CurrentTime = DateTime.Now()

                'If dt.Rows(index)("End_Date") Is DBNull.Value Then
                '    dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("StartDateTime")).ToPointPlusTime()
                'ElseIf CDate(dt.Rows(index)("End_Date")) > CurrentTime Then
                '    dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("End_Date")).ToPointPlusTime()
                'Else
                '    dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("End_Date")).ToPointPlusTime()
                'End If
                'Dim CheckTimeExitedByUserNotNull As String = ""
                'If dt.Rows(index)("TimeExitedByUser") IsNot DBNull.Value Then
                '    CheckTimeExitedByUserNotNull = dt.Rows(index)("TimeExitedByUser").ToString()
                'Else
                '    CheckTimeExitedByUserNotNull = ""
                'End If
                'dtResult.Rows(index)("Status") = ClsHomework.GetStringInBracket(dt.Rows(index)("Module_Status"), CheckTimeExitedByUserNotNull, dt.Rows(index)("IsEnd"),
                '                                                                dt.Rows(index)("QuestionAmount"), dt.Rows(index)("IsDoQuiz"), dt.Rows(index)("AnsweredAmount"))
            Next
            If ClsKNSession.IsMaxONet Then
                For Each r In dtResult.Rows
                    Dim TestSetName As String = r("TestSet_Name")
                    Dim testsetAmount As Integer = listItemName.FindAll(Function(f) f = TestSetName).Count
                    r("TestSet_Name") = r("TestSet_Name") & " (ครั้งที่ " & testsetAmount & ")"


                    Dim i As Integer = listItemName.FindLastIndex(Function(f) f = TestSetName)
                    listItemName.RemoveAt(i)
                Next
            End If
            ForDivNodata.Style.Add("display", "none")
            MainDiv.Visible = True
            RadGrid1.Visible = True
            RadGrid1.DataSource = dtResult
            RadGrid1.DataBind()
        Else
            RadGrid1.Visible = False
            Dim txt As String = If((ClsKNSession.IsMaxONet), "ยังไม่เคยทำฝึกฝนให้ชำนาญค่ะ", "ยังไม่มีการบ้านค่ะ")
            CreateDivNoData(txt)
        End If

    End Sub

    'สร้าง Grid ฝึกฝน
    Public Sub CreateGridPracticeHistory(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByVal IsForStudent As Boolean = False, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim dt As DataTable = GetDtPracticeHistory(StudentId, CalendarId, TeacherId, InputConn, Session("TimeMode"))
        Dim dtResult As DataTable = CreateDtBindGridByMode("PracticeHistory")

        If dt.Rows.Count > 0 Then
            CreateDynamicColumnGridByMode("PracticeHistory")

            Dim tempTestsetId As New List(Of String)

            For index = 0 To dt.Rows.Count - 1
                If IsForStudent = False Then
                    dtResult.Rows.Add(index)("TestSet_Name") = "<div><div class='DivForPractice'><a style='cursor:pointer;' onclick=""ShowTestSetDetailPage('" & dt.Rows(index)("Quiz_Id").ToString() & "','" & StudentId & "')"">" & dt.Rows(index)("TestSet_Name") & "</a></div><div class='TestForIcon'><img src='../Images/ShowHomeworkDetail.png' class='ForImg' onclick=""ShowUsageScorePage('" & dt.Rows(index)("TestSet_Id").ToString() & "','Practice')"" /></div></div>"
                Else
                    Dim testsetId As String = dt.Rows(index)("Testset_Id").ToString()
                    tempTestsetId.Add(testsetId)

                    dtResult.Rows.Add(index)("TestSet_Name") = dt.Rows(index)("TestSet_Name")

                    If ClsKNSession.IsMaxONet Then
                        dtResult.Rows(index)("Quiz_Id") = dt.Rows(index)("Quiz_Id").ToString()
                        dtResult.Rows(index)("Testset_Id") = dt.Rows(index)("Testset_Id").ToString()
                        dtResult.Rows(index)("ActivityPreview") = "<img src='../images/preview.png' class='imgPreview' id='" & dt.Rows(index)("Quiz_Id").ToString() & "' />"
                    End If
                End If

                'If IsForStudent = False Then
                '    dtResult.Rows(index)("Score") = "<div><a style='cursor:pointer;' onclick=""ShowChartStudentInfo('Practice','" & StudentId & "','" & dt.Rows(index)("Quiz_Id").ToString() & "')"">" & dt.Rows(index)("Score").ToString().ToPointplusScore() & "</a></div>"
                'Else
                dtResult.Rows(index)("Score") = dt.Rows(index)("Score").ToString().ToPointplusScore()
                'End If
                dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("StartTime")).ToPointPlusTime()
            Next

            If ClsKNSession.IsMaxONet Then
                For Each r In dtResult.Rows
                    Dim testsetId As String = r("Testset_Id")
                    Dim testsetAmount As Integer = tempTestsetId.FindAll(Function(f) f = testsetId).Count
                    r("TestSet_Name") = r("TestSet_Name") & " (ครั้งที่ " & testsetAmount & ")"


                    Dim i As Integer = tempTestsetId.FindLastIndex(Function(f) f = testsetId)
                    tempTestsetId.RemoveAt(i)
                Next
            End If

            ForDivNodata.Style.Add("display", "none")
            MainDiv.Visible = True
            RadGrid1.Visible = True
            RadGrid1.DataSource = dtResult
            RadGrid1.DataBind()
        Else
            RadGrid1.Visible = False
            CreateDivNoData("ยังไม่เคยฝึกฝนค่ะ")
        End If

    End Sub

    'สร้าง Grid กิจกรรม
    Public Sub CreateGridLog(ByVal StudentId As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection)

        Dim dt As DataTable = GetdtLog(StudentId, CalendarId, InputConn, Session("TimeMode"))
        Dim dtResult As DataTable = CreateDtBindGridByMode("Log")

        If dt.Rows.Count > 0 Then
            CreateDynamicColumnGridByMode("Log")

            For index = 0 To dt.Rows.Count - 1
                Dim BackgroundColor As String = ""
                If dt.Rows(index)("LogType") = 1 Then
                    BackgroundColor = "style='background:Orange;'"
                ElseIf dt.Rows(index)("LogType") = 3 Then
                    BackgroundColor = "style='background:Black;'"
                ElseIf dt.Rows(index)("LogType") = 8 Then
                    BackgroundColor = "style='background:white;'"
                ElseIf dt.Rows(index)("LogType") = 16 Then
                    BackgroundColor = "style='background:Red;'"
                ElseIf dt.Rows(index)("LogType") = 17 Then
                    BackgroundColor = "style='background:Green;'"
                ElseIf dt.Rows(index)("LogType") = 18 Then
                    BackgroundColor = "style='background:Blue;'"
                End If
                dtResult.Rows.Add(index)("Description") = "<div class='ForMaindivLogType'><div class='ForDivLogType' " & BackgroundColor & " ></div>" & dt.Rows(index)("Description") & "</div>"
                dtResult.Rows(index)("StartDateTime") = Convert.ToDateTime(dt.Rows(index)("LastUpdate")).ToPointPlusTime()
            Next
            RadGrid1.Visible = True
            RadGrid1.DataSource = dtResult
            RadGrid1.DataBind()
        Else
            RadGrid1.Visible = False
            CreateDivNoData("ยังไม่เคยทำกิจกรรมค่ะ")
        End If

    End Sub

    'สร้าง Column Gird แบบตามโหมดที่เลือกมา
    Private Sub CreateDynamicColumnGridByMode(ByVal Mode As String)

        Dim BoundColumn As New GridBoundColumn
        RadGrid1.AutoGenerateColumns = False
        RadGrid1.Columns.Clear()

        If Mode = "Homework" Then 'โหมด "การบ้าน"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = If((ClsKNSession.IsMaxONet), "ฝึกฝนให้ชำนาญ", "การบ้าน")
            BoundColumn.DataField = "TestSet_Name"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"

            'อันนี้ใช้ Function โยนวันเวลาเข้าไป จะคืน String กลับมา
            If Not ClsKNSession.IsMaxONet Then
                BoundColumn = New GridBoundColumn
                RadGrid1.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = "กำหนดส่ง"
                BoundColumn.DataField = "StartDateTime"

                'อันนี้ Function เดียวกับ TestsetHeaderControl ที่หาข้อความในวงเล็บ (CreateStringInBracket) แต่เอาเฉพาะส่วนที่เป็นการบ้าน
                BoundColumn = New GridBoundColumn
                RadGrid1.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = "สถานะ"
                BoundColumn.DataField = "Status"
            End If

            If ClsKNSession.IsMaxONet Then
                BoundColumn = New GridBoundColumn
                RadGrid1.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = "วันที่ทำ"
                BoundColumn.DataField = "StartDateTime"

                BoundColumn = New GridBoundColumn
                RadGrid1.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = ""
                BoundColumn.DataField = "ActivityPreview"
            End If

        ElseIf Mode = "QuizHistory" Then 'โหมด "ประวัติควิซ"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "ควิซ"
            BoundColumn.DataField = "TestSet_Name"
            BoundColumn.HeaderStyle.Width = Unit.Pixel(400)

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"
            BoundColumn.HeaderStyle.Width = Unit.Pixel(200)

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "วันที่ควิซ"
            BoundColumn.DataField = "StartDateTime"
            BoundColumn.HeaderStyle.Width = Unit.Pixel(200)

        ElseIf Mode = "PracticeHistory" Then 'โหมด "ประวัติฝึกฝน"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "ทำเพื่อเข้าใจ"
            BoundColumn.DataField = "TestSet_Name"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "คะแนน"
            BoundColumn.DataField = "Score"

            'เข้า Function โยนวันที่ลงไปคืน String กลับออกมา
            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "วันที่ทำ"
            BoundColumn.DataField = "StartDateTime"

            If ClsKNSession.IsMaxONet Then
                BoundColumn = New GridBoundColumn
                RadGrid1.MasterTableView.Columns.Add(BoundColumn)
                BoundColumn.HeaderText = ""
                BoundColumn.DataField = "ActivityPreview"
            End If


        ElseIf Mode = "Log" Then 'โหมด "กิจกรรม"

            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "รายละเอียด"
            BoundColumn.DataField = "Description"

            'เข้า Function โยนวันที่ลงไปคืน String กลับออกมา
            BoundColumn = New GridBoundColumn
            RadGrid1.MasterTableView.Columns.Add(BoundColumn)
            BoundColumn.HeaderText = "เวลา"
            BoundColumn.DataField = "StartDateTime"

        End If

    End Sub

    'สร้าง div ที่แสดงเมื่อหาข้อมูลไม่เจอ รับ String เข้ามาเพื่อเอามาแสดงใน Div 
    Private Sub CreateDivNoData(ByVal ShowString As String)

        Dim sb As New StringBuilder
        sb.Append("<div id='DivNoData' class='ForMainDivNoData' >")
        sb.Append("<div id='DivShowInfo' class='ForDivShowInFo'>")
        sb.Append("<span class='ForSpanNoData' >" & ShowString & "</span>")
        sb.Append("</div></div>")
        ForDivNodata.InnerHtml = sb.ToString()
        MainDiv.Visible = False
        ForDivNodata.Style.Add("display", "block")
    End Sub

    'สร้าง dt สำหรับ Bind ให้ Grid แบ่งตาม Mode
    Private Function CreateDtBindGridByMode(ByVal Mode As String)

        Dim dt As New DataTable

        If Mode = "Homework" Then
            dt.Columns.Add("TestSet_Name", GetType(String))
            dt.Columns.Add("Score", GetType(String))
            dt.Columns.Add("StartDateTime", GetType(String))
            dt.Columns.Add("Status", GetType(String))
        ElseIf Mode = "QuizHistory" Then
            dt.Columns.Add("TestSet_Name", GetType(String))
            dt.Columns.Add("Score", GetType(String))
            dt.Columns.Add("StartDateTime", GetType(String))
        ElseIf Mode = "PracticeHistory" Then
            dt.Columns.Add("TestSet_Name", GetType(String))
            dt.Columns.Add("Score", GetType(String))
            dt.Columns.Add("StartDateTime", GetType(String))
        ElseIf Mode = "Log" Then
            dt.Columns.Add("Description", GetType(String))
            dt.Columns.Add("StartDateTime", GetType(String))
        End If

        If ClsKNSession.IsMaxONet Then
            dt.Columns.Add("Quiz_Id", GetType(String))
            dt.Columns.Add("Testset_Id", GetType(String))
            dt.Columns.Add("ActivityPreview", GetType(String))
        End If

        Return dt

    End Function

    'getDtPracticeHistory รอ ClsPractice ว่างแล้วย้ายเข้าไป
    Private Function GetDtPracticeHistory(ByVal StudentId As String, ByVal CalendarId As String, Optional ByVal TeacherId As String = "", Optional ByRef InputConn As SqlConnection = Nothing, Optional TimeMode As String = Nothing)
        Dim _DB As New ClassConnectSql()
        Dim dt As New DataTable
        Dim sql As String = ""

        'sql = " SELECT tblTestSet.TestSet_Name, CAST(tblQuizSession.TotalScore as varchar(max))  + '/' + CAST(tblQuiz.FullScore as varchar(max)) as Score " & _
        '      " , tblQuiz.StartTime,tblQuiz.Quiz_Id,tblTestset.TestSet_Id FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " & _
        '      " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id  WHERE (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') AND " & _
        '      " (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') AND (tblQuiz.IsPracticeMode = 1)  "

        'sql = " SELECT tblTestSet.TestSet_Name, CAST(tblQuizSession.TotalScore as varchar(10))  + '/' + CAST(tblQuiz.FullScore as varchar(10)) as Score  , 
        sql = " Select tblTestSet.TestSet_Name, Case When dbo.tblQuizSession.TotalScore Is NULL Then '0' + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) " &
              " ELSE CAST(tblQuizSession.TotalScore AS varchar(10)) + '/' + CAST(tblQuiz.FullScore AS VARCHAR(10)) END AS Score,  " &
              " tblQuiz.StartTime,tblQuiz.Quiz_Id,tblTestset.TestSet_Id " &
              " FROM tblQuiz INNER JOIN tblQuizSession On tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id " &
              " INNER JOIN  tblTestSet On tblQuiz.TestSet_Id = tblTestSet.TestSet_Id  " &
              " WHERE (tblQuiz.Calendar_Id = '" & CalendarId.CleanSQL & "') " &
              " AND  (tblQuizSession.Player_Id = '" & StudentId.CleanSQL & "') " &
              " AND (tblQuiz.IsPracticeMode = 1) "
        ' ตัดออก เพราะอยากให้แสดงทุกการฝึกฝนที่เคยทำมา
        '" and tblQuiz.Quiz_Id in (select Quiz_Id from tblQuiz Q where Q.TestSet_Id=tblTestset.TestSet_Id " &
        '" and StartTime=(select Max(starttime) from tblQuiz q2 inner join tblquizsession on q2.Quiz_Id = tblQuizSession.Quiz_Id " &
        '" where q2.TestSet_Id=tblTestset.TestSet_Id and tblQuizSession.Player_Id = '" & StudentId.CleanSQL & "'))"

        'sql = " SELECT tblTestSet.TestSet_Name, " & _
        '      " CAST(tblQuizSession.TotalScore as varchar(max))  + '/' + CAST(tblQuiz.FullScore as varchar(max)) as Score " & _
        '      " , tblQuiz.StartTime,tblQuiz.Quiz_Id,tblTestset.TestSet_Id FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " & _
        '      " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN  tblAssistant ON tblTestSet.UserId = tblAssistant.Teacher_id " & _
        '      " WHERE (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') AND (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') AND (tblQuiz.IsPracticeMode = 1) " & _
        '      " AND (tblAssistant.Assistant_id = '" & _DB.CleanString(TeacherId) & "') "
        'sql = " SELECT tblTestSet.TestSet_Name, " & _
        '      " CAST(tblQuizSession.TotalScore as varchar(max))  + '/' + CAST(tblQuiz.FullScore as varchar(max)) as Score " & _
        '      " , tblQuiz.StartTime,tblQuiz.Quiz_Id,tblTestset.TestSet_Id FROM tblQuiz INNER JOIN tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " & _
        '      " tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id  WHERE (tblQuiz.Calendar_Id = '" & _DB.CleanString(CalendarId) & "') AND " & _
        '      " (tblQuizSession.Player_Id = '" & _DB.CleanString(StudentId) & "') AND (tblQuiz.IsPracticeMode = 1) "         
        If TeacherId <> "" Then
            sql &= " and tblTestSet.UserId = '" & TeacherId.CleanSQL & "'"
        End If

        If TimeMode <> 4 And TimeMode IsNot Nothing Then
            If TimeMode = 0 Then
                sql &= "  AND (convert(varchar(10),tblQuiz.StartTime,120) >= convert(varchar(10),dbo.GetThaiDate(),120))"
            Else
                sql &= "  AND (convert(varchar(10),tblQuiz.StartTime,120) >= convert(varchar(10),dbo.GetThaiDate() - " & TimeMode & ",120))"
            End If
        End If

        sql &= " ORDER BY dbo.tblQuiz.LastUpdate DESC "
        dt = _DB.getdata(sql, , InputConn)
        Return dt
    End Function

    'get ข้อมูลจาก tblLog เอามาสร้าง Grid ของกิจกรรม
    Private Function GetdtLog(ByVal StudentId As String, ByVal CalendarId As String, ByRef InputConn As SqlConnection, Optional TimeMode As String = Nothing)

        Dim _DB As New ClassConnectSql()
        Dim dt As New DataTable
        Dim sql As String = " select LogType,[Description],LastUpdate from tblLog where UserId = '" & StudentId.CleanSQL & "' " &
                            " and calendar_id = '" & CalendarId.CleanSQL & "' "

        If TimeMode <> 4 And TimeMode IsNot Nothing Then
            If TimeMode = 0 Then
                sql &= "  AND (convert(varchar(10),tblLog.LastUpdate,120) >= convert(varchar(10),dbo.GetThaiDate(),120))"
            Else
                sql &= "  AND (convert(varchar(10),tblLog.LastUpdate,120) >= convert(varchar(10),dbo.GetThaiDate() - " & TimeMode & ",120))"
            End If
        End If

        sql &= " order by Lastupdate desc "
        dt = _DB.getdata(sql, , InputConn)
        Return dt

    End Function




#End Region


    Private Sub RadGrid1_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadGrid1.ItemDataBound

        If (TypeOf (e.Item) Is GridDataItem) Then
            If e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("Score") IsNot Nothing Then
                Dim dataBoundItem As GridDataItem = e.Item
                If dataBoundItem("Score") IsNot Nothing Then
                    dataBoundItem("Score").Style.Add("text-align", "center")
                End If
            End If

            If e.Item.OwnerTableView.Columns.FindByUniqueNameSafe("StartDateTime") IsNot Nothing Then
                Dim dataBoundItem As GridDataItem = e.Item
                If dataBoundItem("StartDateTime") IsNot Nothing Then
                    dataBoundItem("StartDateTime").Style.Add("text-align", "center")
                End If
            End If
        End If

    End Sub


    Private Sub RadGrid1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadGrid1.PreRender

        For Each r As GridColumn In RadGrid1.Columns
            If r.UniqueName = "Score" Then
                r.HeaderStyle.Width = 100
            ElseIf r.UniqueName = "StartDateTime" Then
                r.HeaderStyle.Width = 150
            End If
        Next

    End Sub


End Class