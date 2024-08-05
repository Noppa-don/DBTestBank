Imports System.Web

Public Class RepeaterReportHomework
    Inherits System.Web.UI.Page
    'ตัวแปรที่เอาไว้เก็บข้อมูล เป็นการเก็บแบบ Application ซึ่งทาง KN สร้าง Class ขึ้นมา Wrap ให้ใช้ง่าย
    Dim KNSession As New KNAppSession()
    'ตัวแปรที่ไว้ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()

    ''' <summary>
    ''' ทำการ Bind Repeater การบ้าน ที่ ครูคนนี้/ครูผู้ช่วย เป็นคนสั่ง
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
    ''' หาข้อมูลการบ้านเพื่อนำมา Bind Repeater การบ้าน
    ''' </summary>
    ''' <param name="CalendarId">ปีการศึกษาปัจจุบัน</param>
    ''' <remarks></remarks>
    Private Sub BindRepeater(ByVal CalendarId As String)

        Dim dt As DataTable = GetDtReportHomework(CalendarId)

        If dt.Rows.Count > 0 Then
            Listing.DataSource = dt
            Listing.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' ทำการ select หา dt การบ้านเพื่อนำไป Bind Repeater การบ้าน
    ''' </summary>
    ''' <param name="CalendarId">ปีการศึกษาปัจจุบัน</param>
    ''' <returns>DT การบ้าน</returns>
    ''' <remarks></remarks>
    Private Function GetDtReportHomework(ByVal CalendarId As String) As DataTable
        'หา ชื่อการบ้าน,สั่งให้,เวลาที่เริ่ม,กำหนดส่ง
        Dim sql As String = " SELECT tblTestSet.TestSet_Name, tblModuleAssignment.AssignTo, tblModuleAssignment.Start_Date, tblModuleAssignment.End_Date, " & _
                            " tblModuleDetailCompletion.Quiz_Id,dbo.tblModuleAssignment.MA_Id FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id " & _
                            " INNER JOIN tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN tblModuleDetailCompletion ON " & _
                            " tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id INNER JOIN tblTestSet ON tblModuleDetail.Reference_Id = " & _
                            " tblTestSet.TestSet_Id INNER JOIN tblAssistant ON tblModule.Create_By = tblAssistant.Teacher_id WHERE " & _
                            " (tblModuleAssignment.Calendar_Id = '" & CalendarId & "') AND (tblModule.IsActive = 1) AND " & _
                            " (tblAssistant.Assistant_id = '" & HttpContext.Current.Session("UserId").ToString() & "') GROUP BY tblTestSet.TestSet_Name, tblModuleAssignment.AssignTo, " & _
                            " tblModuleAssignment.Start_Date, tblModuleAssignment.End_Date, tblModuleDetailCompletion.Quiz_Id,dbo.tblModuleAssignment.MA_Id ORDER BY (dbo.tblModuleAssignment.Start_Date) DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    ''' <summary>
    ''' Function ที่ทำงานร่วมกับ Repeater จะทำการส่ง class ของ TD กลับไปเมื่อตัวอักษารที่ได้รับมายาวเกิน 40 ตัวอักษรจะให้ไปทำการขึ้น ToolTip แทน
    ''' </summary>
    ''' <param name="AssignToTxt">ห้อง,ชั้น/คน ที่ถูกสั่งการบ้าน</param>
    ''' <returns>ชื่อ Class สำหรับ TD ใน Repeater</returns>
    ''' <remarks></remarks>
    Public Function GetClassForTdAssignTo(ByVal AssignToTxt As Object) As String

        If AssignToTxt Is DBNull.Value Then
            Return ""
        End If

        If AssignToTxt.Length >= 40 Then
            Return "class='AssignToIsLong'"
        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' Function ที่ทำงานร่วมกับ Repeater จะทำการหา txt ห้องที่ถุกสั่งการบ้านคินกลับไปโดยต้องดูก่อนว่าตัวอักษรยาวเกิน 40 ตัวอักษร หรือเปล่า ถ้ายาวเกิน จะตัดคำแล้วให้ไปขึ้นเป็น ToolTip แทนเมื่อเอา Mouse ไปลอย
    ''' </summary>
    ''' <param name="AssignToTxt">ชั้น/ห้อง/คน ที่ถูกสั่งการบ้าน</param>
    ''' <param name="MaId">ModuleAssignMentId ของ Record นั้นๆ</param>
    ''' <returns>string ผู้ที่ถูกสั่งการบ้าน</returns>
    ''' <remarks></remarks>
    Public Function CheckAssignToIsLong(ByVal AssignToTxt As Object, ByVal MaId As String) As String

        If AssignToTxt Is DBNull.Value Then
            Return ""
        End If

        If AssignToTxt.Length >= 40 Then
            'Return AssignToTxt.ToString().Substring(0, 15) & "<a id='" & MaId & "'>ดูเพิ่มเติม...</a>"
            'color: #2370FA;
            Return AssignToTxt.ToString().Substring(0, 15) & "<span id='" & MaId & "' style='color: #2370FA;'>...ดูเพิ่มเติม</span>"
        Else
            Return AssignToTxt.ToString()
        End If

    End Function

    ''' <summary>
    ''' ทำการหา ชั้น/ห้อง/คน ที่ถูกสั่งการบ้านแบบเต็มๆเพื่อไปแสดงใน ToolTip เมื่อมันยาวเกิน 40 ตัวอักษร
    ''' </summary>
    ''' <param name="MaId">ModuleAssignMentId ของ Record นั้นๆ</param>
    ''' <returns>ชื่อ/ห้อง/คน ที่ถูกสั่งการบ้านแบบเต็มๆไม่ตัดคำ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GetFullAssignTo(ByVal MaId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT AssignTo FROM dbo.tblModuleAssignment WHERE MA_Id = '" & MaId & "' "
        Dim AssignToTxt As String = _DB.ExecuteScalar(sql)
        Return AssignToTxt

    End Function

End Class