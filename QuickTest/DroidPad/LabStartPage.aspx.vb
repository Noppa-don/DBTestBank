Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class LabStartPage
    Inherits System.Web.UI.Page
    'ตัวแปรที่ใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql())

    Private redis As New RedisStore()

    ''' <summary>
    ''' Function ที่เป็นขั้นตอนแรกหลังจากเข้าจาก Tablet ที่เป็น ModeLab
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            If HttpContext.Current.Request.QueryString("DeviceUniqueID") IsNot Nothing And HttpContext.Current.Request.QueryString("NeedRegister") IsNot Nothing Then
                If HttpContext.Current.Request.QueryString("DeviceUniqueID").ToString() <> "" And HttpContext.Current.Request.QueryString("NeedRegister").ToString() <> "" Then
                    Dim ConnLabStartPage As New SqlConnection
                    'รหัสเครื่อง Tablet
                    Dim DeviceId As String = HttpContext.Current.Request.QueryString("DeviceUniqueID").ToString().Trim()
                    'ตัวแปรที่บอกว่าต้องการจะลงทะเบียนหรือเปล่า
                    Dim NeedRegister As String = HttpContext.Current.Request.QueryString("NeedRegister").ToString().Trim()
                    Dim IsOwner As String = Request.QueryString("IsOwner").ToString()


                    If redis.Getkey(DeviceId & "_Register") <> "" Then
                        Response.Redirect("~/DroidPad/RegisterTabletDeskOrSpare.aspx?DeviceId=" & DeviceId & "&IsOwner=" & IsOwner)
                        Exit Sub
                    End If


                    'If NeedRegister.ToLower() = "t" Then
                    '    Response.Redirect("~/DroidPad/RegisterTabletDeskOrSpare.aspx?DeviceId=" & DeviceId)
                    'Else
                    'Open Connection
                    _DB.OpenExclusiveConnect(ConnLabStartPage)
                    'หาก่อนว่า Tablet เครื่องนี้ลงทะเบียนหรือยัง
                    If ClsDroidPad.CheckTabletIsRegistered(DeviceId, ConnLabStartPage) = True Then
                        'ถ้าลงทะเบียนแล้วก็ไป Set Label + Timer 60 วิ Redirect
                        'Default เป็น Student ก่อน ถ้าเช็คแล้วเป็นครู ค่อยเปลี่ยน
                        Dim CheckIsTeacher As Boolean = False

                        redis.SetKey(DeviceId & "_LabTeacher", False)

                        'เช็คว่าเป็นครูรึเปล่า
                        If CheckDeviceIsTeacher(DeviceId, ConnLabStartPage) = True Or IsOwner = "T" Then
                            redis.SetKey(DeviceId & "_LabTeacher", True)
                            CheckIsTeacher = True
                        End If

                        ClsLog.Record(" LabStartPage : CheckIsTeacher = " & CheckIsTeacher & ", IsOwner = " & IsOwner)

                        'ทำการ set label ข้อมูลต่างๆของ Tablet เครื่องนี้
                        SetTxtDetailToLabel(DeviceId, ConnLabStartPage)
                        'ทำการเรียกให้ Javascript ทำงานโดยให้ Redirect ไปหน้าถัดไป ถ้าเป็นครูให้ไปหน้า Loginpage.aspx , ถ้านักเรียนให้ไปหน้า /Practicemode_Pad/ChooseTestset.aspx
                        TriggerScript(CheckIsTeacher, DeviceId)
                    Else
                        'Dim n As NameValueCollection = Request.QueryString
                        'For Each nn As String In n.Keys
                        '    Response.Write(nn & "<br/>")
                        'Next
                        'ถ้ายังไม่ได้ลงทะเบียน Redirect ไปหน้าลงทะเบียน
                        'Dim IsOwner As String = Request.QueryString("IsOwner").ToString()
                        Response.Redirect("~/DroidPad/RegisterTabletDeskOrSpare.aspx?DeviceId=" & DeviceId & "&IsOwner=" & IsOwner)
                    End If
                    'Close Connection
                    _DB.CloseExclusiveConnect(ConnLabStartPage)
                    'End If
                End If
            End If
        End If
    End Sub

    'Private Function CheckTabletIsRegistered(ByVal DeviceId As String, ByRef InputConn As SqlConnection) As Boolean
    '    If DeviceId IsNot Nothing And DeviceId <> "" Then
    '        'เช็คก่อนว่า tblTabletDesk มีผูกอยู่หรือเปล่า
    '        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblTabletLabDesk INNER JOIN dbo.t360_tblTablet ON dbo.tblTabletLabDesk.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " & _
    '                            " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' " & _
    '                            " AND dbo.t360_tblTablet.Tablet_IsActive = 1 AND dbo.tblTabletLabDesk.IsActive = 1 "
    '        Dim CountCheckIsRegistered As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
    '        If CountCheckIsRegistered > 0 Then
    '            Return True
    '        Else
    '            'เช็คต่อว่าเป็นเครื่องสำรองหรือเปล่า
    '            sql = " SELECT COUNT(*) FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceId) & "' AND Tablet_IsActive = 1 AND Tablet_IsOwner = 0 "
    '            CountCheckIsRegistered = CInt(_DB.ExecuteScalar(sql, InputConn))
    '            If CountCheckIsRegistered > 0 Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    Else
    '        Return False
    '    End If
    'End Function

    ''' <summary>
    ''' หาว่า Tablet เครื่องนี้ถูกลงทะเบียนเป็นครูหรือเปล่า
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function CheckDeviceIsTeacher(ByVal DeviceId As String, ByRef InputConn As SqlConnection) As Boolean
        'Dim sql As String = " SELECT COUNT(*) FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId & "' AND Tablet_IsActive = 1 AND Tablet_IsOwner = 1 "
        'Dim CheckIsOwner As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
        'If CheckIsOwner = 0 Then
        '    Return True
        'Else
        Dim Sql As String = " SELECT Player_Type FROM dbo.tblTabletLabDesk INNER JOIN dbo.t360_tblTablet " & _
              " ON dbo.tblTabletLabDesk.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " & _
              " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' " & _
              " AND dbo.t360_tblTablet.Tablet_IsActive = 1 AND dbo.tblTabletLabDesk.IsActive = 1; "

        Dim p As String = _DB.ExecuteScalar(Sql, InputConn)
        If p = "" Then Return False
        Dim PlayerType As Integer = CInt(p) 'CInt(_DB.ExecuteScalar(Sql, InputConn))
        If PlayerType = 1 Then
            Return True
        Else
            Return False
        End If
        'End If
    End Function

    ''' <summary>
    ''' ทำการ set label ข้อมูลต่างๆ ชื่อห้อง Lab , ชื่อเครื่อง , หรือว่าเป็นเครื่องสำรอง
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub SetTxtDetailToLabel(ByVal DeviceId As String, ByRef InputConn As SqlConnection)
        Dim sql As String = "  "
        'เช็คก่อนว่าเป็นเครื่องห้อง Lab หรือเปล่า
        sql = " SELECT dbo.tblTabletLab.TabletLab_Name,dbo.t360_tblTablet.Tablet_TabletName FROM dbo.t360_tblTablet INNER JOIN dbo.tblTabletLabDesk " & _
              " ON dbo.t360_tblTablet.Tablet_Id = dbo.tblTabletLabDesk.Tablet_Id INNER JOIN dbo.tblTabletLab ON dbo.tblTabletLabDesk.TabletLab_Id = dbo.tblTabletLab.TabletLab_Id " & _
              " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & _DB.CleanString(DeviceId.Trim()) & "' AND dbo.t360_tblTablet.Tablet_IsActive = 1 " & _
              " AND dbo.tblTabletLabDesk.IsActive = 1 AND dbo.tblTabletLab.IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        'ถ้ามีข้อมูลก็ให้มาหาข้อมูลว่าอยู่ห้อง Lab ไหน , เป็นเครื่องเท่าไหร่
        If dt.Rows.Count > 0 Then
            lblMainDetail.Text = "ห้อง Lab : " & dt.Rows(0)("TabletLab_Name").ToString()
            lblSecondDetail.Text = "เครื่อง : " & dt.Rows(0)("Tablet_TabletName").ToString()
        Else
            'ถ้าไม่มีข้อมูลแสดงว่าเป็นเครื่องสำรอง
            sql = " SELECT Tablet_TabletName FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceId.Trim()) & "' AND Tablet_IsActive = 1 "
            Dim TabletName As String = _DB.ExecuteScalar(sql, InputConn)
            If TabletName <> "" Then
                lblMainDetail.Text = "เครื่องสำรอง"
                lblSecondDetail.Text = TabletName
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Inject Javascript เพื่อให้ไปหน้าถัดไป ในกรณีเป็นครูไปหน้า Loginpage.aspx , นักเรียน ไปหน้า /Practicemode_Pad/ChooseTestset.aspx
    ''' </summary>
    ''' <param name="IsTeacher">เป็นครู ?</param>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <remarks></remarks>
    Private Sub TriggerScript(ByVal IsTeacher As Boolean, ByVal DeviceId As String)
        Dim ScriptStr As String = ""
        If IsTeacher = True Then
            ScriptStr = "<script type='text/javascript'>$(function () {setTimeout(function () {window.location = '../LoginPage.aspx';}, 60000);$('html').click(function () {window.location = '../LoginPage.aspx';});});</script><title></title>"
        Else
            'ScriptStr = "<script type='text/javascript'>$(function () {setTimeout(function () {window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=1&DashboardMode=6';}, 60000);$('html').click(function () {window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=1&DashboardMode=6';});});</script><title></title>"
            'PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueId=
            ScriptStr = "<script type='text/javascript'>$(function () {setTimeout(function () {window.location = '../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueId=" & DeviceId & "';}, 60000);$('html').click(function () {window.location = '../PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueId=" & DeviceId & "';});});</script><title></title>"
        End If
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", ScriptStr)
    End Sub


End Class