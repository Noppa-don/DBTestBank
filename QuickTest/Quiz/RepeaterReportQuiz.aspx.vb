Public Class RepeaterReportQuiz
    Inherits System.Web.UI.Page
    'ตัวแปรที่ไว้ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'ตัวแปรที่เอาไว้เก็บข้อมูล เป็นการเก็บแบบ Application ซึ่งทาง KN สร้าง Class ขึ้นมา Wrap ให้ใช้ง่าย
    Dim KNSession As New KNAppSession()

    ''' <summary>
    ''' ทำการ Bind Repeater ข้อสอบทั้งหมดของ mode Quiz ของ ครูคนนี้/ผู้ช่วย
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim CalendarId As String = KNSession("CurrentCalendarId").ToString()

        If Not Page.IsPostBack Then
            BindRepeater(CalendarId)
        End If

    End Sub

    ''' <summary>
    ''' Bind Repeater โดยหาข้อมูลเฉพาะ ปีการศึกษานี้
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
    ''' ทำการหา dt ข้อมูล Quiz เพื่อนำไป Bind Repeater
    ''' </summary>
    ''' <param name="CalendarId">ปีการศึกษาปัจจุบัน</param>
    ''' <returns>DT ข้อมูล Quiz</returns>
    ''' <remarks></remarks>
    Private Function GetdtReportQuiz(ByVal CalendarId As String) As DataTable
        'ทำการ select หา ชั้น + ห้อง , QuizId , ชื่อชุดข้อสอบ , เวลาที่เริ่มทำ
        Dim sql As String = " SELECT tblQuiz.t360_ClassName + tblQuiz.t360_RoomName AS ClassRoomName, tblQuiz.Quiz_Id, tblTestSet.TestSet_Name, tblQuiz.StartTime " & _
                            " FROM tblQuiz INNER JOIN tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN tblAssistant ON tblQuiz.User_Id = " & _
                            " tblAssistant.Teacher_id WHERE (tblAssistant.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "') AND " & _
                            " (dbo.tblQuiz.IsActive = 1) AND (dbo.tblQuiz.Calendar_Id = '" & CalendarId & "') AND (tblQuiz.IsQuizMode = 1) ORDER BY dbo.tblQuiz.LastUpdate DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        For Each eachQuiz In dt.Rows
            eachQuiz("TestSet_Name") = HttpUtility.HtmlEncode(eachQuiz("TestSet_Name"))
        Next
        Return dt

    End Function

End Class