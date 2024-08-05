Public Class RepeaterReportPractice
    Inherits System.Web.UI.Page
    'ตัวแปรที่ไว้ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'ตัวแปรที่เอาไว้เก็บข้อมูล เป็นการเก็บแบบ Application ซึ่งทาง KN สร้าง Class ขึ้นมา Wrap ให้ใช้ง่าย
    Dim KnSession As New KNAppSession()

    ''' <summary>
    ''' ทำการ Bind Repeater ฝึกฝนที่ ครูคนนี้/ครูผู้ช่วย เป็นคนจัด
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim CalendarId As String = KnSession("CurrentCalendarId").ToString()

        If Not Page.IsPostBack Then
            BindRepeater(CalendarId)
        End If

    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลฝึกฝนและนำมา Bind Repeater
    ''' </summary>
    ''' <param name="CalendarId">ปีการศึกษาปัจจุบัน</param>
    ''' <remarks></remarks>
    Private Sub BindRepeater(ByVal CalendarId As String)

        Dim dt As DataTable = GetdtReportQuiz(CalendarId)

        If dt.Rows.Count > 0 Then
            Listing.DataSource = dt
            Listing.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' ทำการ select หาข้อมูลเพื่อนำมา Bind Repeater ฝึกฝน
    ''' </summary>
    ''' <param name="CalendarId">ปีการศึกษาปัจจุบัน</param>
    ''' <returns>Datatable ที่มีข้อมูลฝึกฝน</returns>
    ''' <remarks></remarks>
    Private Function GetdtReportQuiz(ByVal CalendarId As String) As DataTable

        'Dim sql As String = " SELECT tblTestSet.TestSet_Name, COUNT(DISTINCT tblQuizScore.Student_Id) TotalStudentPractice,dbo.tblTestSet.TestSet_Id " & _
        '                    " FROM tblTestSet INNER JOIN tblAssistant ON tblAssistant.Assistant_id = tblTestSet.UserId INNER JOIN " & _
        '                    " tblQuiz ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id " & _
        '                    " WHERE (tblTestSet.IsPracticeMode = 1) AND (tblAssistant.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "') " & _
        '                    " AND (tblQuiz.Calendar_Id = '" & CalendarId & "') AND (dbo.tblQuiz.IsActive = 1)  AND (dbo.tblQuiz.IsPracticeMode = 1) " & _
        '                    " GROUP BY tblTestSet.TestSet_Name,dbo.tblTestSet.LastUpdate,dbo.tblTestSet.TestSet_Id ORDER BY tblTestSet.LastUpdate DESC "

        'หา ชื่อชุดข้อสอบ,จำนวนครั้งที่เข้าทำฝึกฝนชุดนั้นๆ,TestsetId
        Dim sql As String = " SELECT tblTestSet.TestSet_Name, COUNT(DISTINCT tblQuizScore.Student_Id) TotalStudentPractice,dbo.tblTestSet.TestSet_Id  " & _
                            " FROM tblTestSet INNER JOIN tblAssistant AS a1 ON a1.Assistant_id = tblTestSet.UserId INNER JOIN  tblQuiz " & _
                            " ON tblTestSet.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id  " & _
                            " WHERE (tblTestSet.IsPracticeMode = 1) AND (a1.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "')  " & _
                            " AND (tblQuiz.Calendar_Id = '" & CalendarId & "') AND (dbo.tblQuiz.IsActive = 1)  AND (dbo.tblQuiz.IsPracticeMode = 1) " & _
                            " AND NOT EXISTS (SELECT * FROM dbo.tblQuizScore AS qs2  INNER JOIN dbo.tblAssistant ON qs2.Student_Id = dbo.tblAssistant.Teacher_id " & _
                            " WHERE qs2.QuizScore_Id = dbo.tblQuizScore.QuizScore_Id ) AND dbo.tblQuizScore.Student_Id <> '4AF763E3-C133-4B76-A1A2-6D69CF0909D9' " & _
                            " GROUP BY tblTestSet.TestSet_Name,dbo.tblTestSet.LastUpdate,dbo.tblTestSet.TestSet_Id ORDER BY tblTestSet.LastUpdate DESC  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function


End Class