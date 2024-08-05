Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class RegisterTabletDeskOrSpare
    Inherits System.Web.UI.Page

    'ตัวแปรที่ใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'ตัวแปร รหัสเครื่อง Tablet ใช้ทั้งหน้า
    Private Property DeviceId As String
        Get
            DeviceId = ViewState("_DeviceId")
        End Get
        Set(value As String)
            ViewState("_DeviceId") = value
        End Set
    End Property

    ''' <summary>
    ''' ทำการ Bind DDL ห้อง Lab และเช็คว่า tablet นี้เป็นเครื่องเด็กหรือครูเพื่อทำการซ่อน radio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            If Request.QueryString("DeviceId") IsNot Nothing Then
                If Request.QueryString("DeviceId").ToString() <> "" Then
                    DeviceId = _DB.CleanString(Request.QueryString("DeviceId").ToString().Trim())
                    Dim ConnRegister As New SqlConnection
                    'Open Connection
                    _DB.OpenExclusiveConnect(ConnRegister)
                    'Bind DDL ชื่อห้อง Lab
                    DLLBinding(DeviceId, ConnRegister)
                End If
            End If

            If Request.QueryString("IsOwner") IsNot Nothing Then
                Dim IsOwner As String = Request.QueryString("IsOwner").ToString()
                If IsOwner = "T" Then
                    ' ซ่อน radio เด็ก + เลขที่นั่ง
                    rdoStudent.Visible = False
                    lblStudentAmount.Visible = False
                    rdoTeacher.Checked = True
                ElseIf IsOwner = "S" Then
                    ' ซ่อน radio ครู
                    rdoTeacher.Visible = False
                    lblTeacherAmount.Visible = False
                    rdoStudent.Checked = True
                End If
            Else
                Response.Write("Nothing")
            End If
        End If
    End Sub

    ''' <summary>
    ''' หาข้อมูลของห้อง Lab เพื่อนำมา Bind เข้ากับ DDL
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub DLLBinding(ByVal DeviceId As String, ByRef InputConn As SqlConnection)
        Dim sql As String = " SELECT dbo.tblTabletLab.TabletLab_Id,dbo.tblTabletLab.TabletLab_Name FROM dbo.tblTabletLab INNER JOIN " & _
                            " dbo.t360_tblTablet ON dbo.tblTabletLab.School_Code = dbo.t360_tblTablet.School_Code " & _
                            " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' AND dbo.tblTabletLab.IsActive = 1 AND dbo.t360_tblTablet.Tablet_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then
            DDlLabName.DataSource = dt
            DDlLabName.DataTextField = "TabletLab_Name"
            DDlLabName.DataValueField = "TabletLab_Id"
            DDlLabName.DataBind()
            DDlLabName.SelectedIndex = 0
            DDlLabName.SelectedValue = 0
            'ทำการหาข้อมูลที่นั่งในห้อง Lab ตามที่ DDL เลือก
            ChangeLabeltxt(InputConn)
        End If
    End Sub

    ''' <summary>
    ''' หาจำนวนที่ในห้อง Lab นั้นๆตาม DDL ห้อง Lab ที่เลือกมา ทั้งแบบของครู และ ของนักเรียน + Set Label แสดงจำนวนที่ของนักเรียน/ครู
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub ChangeLabeltxt(Optional ByRef InputConn As SqlConnection = Nothing)
        If DDlLabName.SelectedValue IsNot Nothing Then
            'หาจำนวนว่ามี Tablet ครูกี่ทีแล้วในห้อง Lab นี้
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblTabletLabDesk WHERE TabletLab_Id = '" & DDlLabName.SelectedValue.ToString() & "' AND Player_Type = 1 AND IsActive = 1 "
            Dim TeacherAmount As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
            lblTeacherAmount.Text = "(มีแล้ว " & TeacherAmount & " ที่)"
            'หาจำนวนว่ามี Tablet นักเรียนกี่ทีแล้วในห้อง Lab นี้
            sql = " SELECT COUNT(*) FROM dbo.tblTabletLabDesk WHERE TabletLab_Id = '" & DDlLabName.SelectedValue.ToString() & "' AND Player_Type = 2 AND IsActive = 1 "
            Dim StudentAmount As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
            lblStudentAmount.Text = "(มีแล้ว " & StudentAmount & " ที่)"
            txtDeskNumber.Text = StudentAmount + 1
        End If
    End Sub

    ''' <summary>
    ''' เมื่อกดปุ่มลงทะเบียนกับห้อง Lab จะทำการแสดง Div ข้อมูลที่จะลงทะเบียนกับห้อง Lab และ ซ่อน Div ที่ลงทะเบียนเป็นเครื่องสำรองไป
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnShowRegisterLab_Click1(sender As Object, e As EventArgs) Handles BtnShowRegisterLab.Click
        DivRegisterSpare.Visible = False
        DivRegisterLab.Visible = True
    End Sub

    ''' <summary>
    ''' เมื่อกดปุ่มลงทะเบียนเป็นเครื่องสำรอง จะทำการแสดง Div ข้อมูลที่จะลงทะเบียนเป็นเครื่องสำรอง และ ซ่อน Div ที่ลงทะเบียนกับห้อง Lab
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnShowRegisterSpare_Click1(sender As Object, e As EventArgs) Handles BtnShowRegisterSpare.Click
        DivRegisterLab.Visible = False
        DivRegisterSpare.Visible = True
    End Sub

    ''' <summary>
    ''' เมื่อทำการเปลี่ยนค่าใน DDL ห้อง Lab จะต้องไปหาข้อมูลที่นั่งในห้อง Lab นั้นๆใหม่
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DDlLabName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDlLabName.SelectedIndexChanged
        ChangeLabeltxt()
    End Sub

    ''' <summary>
    ''' กดปุ่มตกลงที่ div ลงทะเบียนกับห้อง Lab ทำการ Insert ข้อมูลผูกให้ลงทะเบียนกับห้อง Lab
    ''' และเช็คว่าเป็นครูหรือนักเรียนแล้ว set ค่าเก็บไว้ใน Redis แล้วทำการ Redirect ไปหน้าถัดไป
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnRegisterLab_Click(sender As Object, e As EventArgs) Handles BtnRegisterLab.Click
        Dim ConnRegister As New SqlConnection
        Try
            _DB.OpenExclusiveConnect(ConnRegister)
            'ทำการ Insert ข้อมูลผูก Tablet เครื่องนี้กับห้อง Lab
            RegisterTabletLab(ConnRegister)
            _DB.CloseExclusiveConnect(ConnRegister)
            Dim Redis As New RedisStore()
            Redis.DEL(DeviceId & "_Register")
            If rdoTeacher.Checked = True Then
                Redis.SetKey(DeviceId & "_LabTeacher", True)
                Response.Redirect("~/LoginPage.aspx")
            Else
                'Response.Redirect("~/PracticeMode_Pad/ChooseClass.aspx?UseComputer=1&DashboardMode=6")
                'Dim Redis As New RedisStore()
                'Redis.DEL(DeviceId & "_Register")
                Response.Redirect("~/PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueId=" & DeviceId)
            End If
        Catch ex As Exception
            _DB.CloseExclusiveConnect(ConnRegister)
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ Insert ข้อมูลลงทะเบียนผูก Tablet เครื่องนี้เข้ากับห้อง Lab ที่เลือกมา
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub RegisterTabletLab(ByRef InputConn As SqlConnection)
        If DDlLabName.SelectedValue IsNot Nothing Then
            Try
                _DB.OpenWithTransection(InputConn)
                'ตัวแปรที่จะบอกว่าเป็นครูหรือนักเรียน 1=ครู , 2=นักเรียน
                Dim PlayerType As Integer = 0
              
                'ชื่อโต๊ะในกรณีที่เป็นนัเกรียน
                Dim DeskName As String

                'If rdoTeacher.Checked = True Then
                '    PlayerType = 1
                'End If
                'If rdoStudent.Checked = True Then
                '    PlayerType = 2
                '    DeskName = CInt(txtDeskNumber.Text)
                'End If

                'เลขที่โต๊ะ
                Dim DeskNo As Integer
                If (Request.QueryString("IsOwner").ToString().ToUpper() = "T") Then
                    PlayerType = 1
                    DeskNo = 0
                    DeskName = "Desk Teacher " & CInt(Regex.Match(lblTeacherAmount.Text, "\d+").Value) + 1
                Else
                    PlayerType = 2
                    DeskNo = CInt(txtDeskNumber.Text)
                    DeskName = "Desk " & DeskNo
                End If

                'Update ข้อมูลใน TabletOwner ก่อนเผื่อว่าเคยผูกกับคนไว้ก่อนแล้ว
                Dim sql As String = " UPDATE dbo.t360_tblTabletOwner SET TabletOwner_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Owner_Id IN " &
                                    " (SELECT Owner_Id FROM dbo.t360_tblTabletOwner INNER JOIN dbo.t360_tblTablet ON dbo.t360_tblTabletOwner.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " &
                                    " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' AND dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 " &
                                    " AND dbo.t360_tblTablet.Tablet_IsActive = 1); "
                _DB.ExecuteWithTransection(sql, InputConn)

                'Update TabletIsOnwer ให้เป็น 1 เพราะว่ามาอยู่ในห้อง Lab แล้ว
                ' ปรับเพิ่ม Tablet_IsOwner ของห้องแลปคือ 0
                sql = " UPDATE dbo.t360_tblTablet SET Tablet_IsOwner = 0,Tablet_LastUpdate = dbo.GetThaiDate() WHERE Tablet_SerialNumber = '" & DeviceId & "' AND Tablet_IsActive = 1; "
                _DB.ExecuteWithTransection(sql, InputConn)

                'update tabletdesk เก่า เผื่อเคย เป็นเครื่อง lab เก่า
                sql = String.Format(SQLUpdateTabletLabDesk(), DeviceId)
                _DB.ExecuteWithTransection(sql, InputConn)


                'Insert ลง tblTabletDesk
                sql = " INSERT INTO dbo.tblTabletLabDesk( TLD_Id ,TabletLab_Id ,Tablet_Id ,DeskName ,Player_Type ,IsActive ,LastUpdate) " &
                      " SELECT NEWID(),'" & DDlLabName.SelectedValue().ToString() & "',Tablet_Id,'" & DeskNo & "','" & PlayerType & "',1,dbo.GetThaiDate() " &
                      " FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId & "' AND Tablet_IsActive = 1; "
                _DB.ExecuteWithTransection(sql, InputConn)

                'Update t360_tbltablet Update TabletName
                'sql = " UPDATE dbo.t360_tblTablet SET Tablet_TabletName = 'Desk" & DeskName & "' WHERE Tablet_SerialNumber = '" & DeviceId & "'; "
                sql = " UPDATE dbo.t360_tblTablet SET Tablet_TabletName = '" & DeskName & "' WHERE Tablet_SerialNumber = '" & DeviceId & "'; "
                _DB.ExecuteWithTransection(sql, InputConn)

                'Update TabletStatus ให้เป็น 0 ก่อน
                sql = " Update t360_tblTabletStatusDetail set IsActive = 0, LastUpdate = dbo.GetThaiDate() " &
                      " where Tablet_Id in(select Tablet_Id from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "')"
                _DB.ExecuteWithTransection(sql, InputConn)
                'Insert สถานะลงทะเบียน
                sql = " insert into t360_tblTabletStatusDetail " &
                      " select  newid(),Tablet_Id,1,3,null,null,null,null,null,null,'" & DeskName & "',School_Code,dbo.GetThaiDate(),1,null from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "';"
                _DB.ExecuteWithTransection(sql, InputConn)

                _DB.CommitTransection(InputConn)
                lblValidate.Visible = False


            Catch ex As Exception
                _DB.RollbackTransection(InputConn)
                lblValidate.Text = "บางอย่างผิดพลาดโปรดลองอีกครั้ง"
                lblValidate.Visible = True
                Throw ex
            End Try
        End If
    End Sub

    ''' <summary>
    ''' ทำการลงทะเบียน Tablet เครื่องนี้ให้เป็นเครื่องสำรอง และ Redirect ไปหน้าถัดไป
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnRegisterSpare_Click(sender As Object, e As EventArgs) Handles BtnRegisterSpare.Click
        Dim ConnRegister As New SqlConnection
        Try
            Dim isOwner As String = Request.QueryString("IsOwner").ToString()
            _DB.OpenExclusiveConnect(ConnRegister)
            'ทำการ Insert ข้อมูลให้เป็นเครื่องสำรอง
            Dim CheckInsert As Boolean = RegisterTabletSpare(ConnRegister)
            _DB.CloseExclusiveConnect(ConnRegister)
            Dim Redis As New RedisStore()
            Redis.DEL(DeviceId & "_Register")
            If CheckInsert = True Then
                If isOwner = "T" Then
                    Response.Redirect("~/Loginpage.aspx")
                Else
                    Response.Redirect("~/PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueId=" & DeviceId)
                End If
            End If
        Catch ex As Exception
            _DB.CloseExclusiveConnect(ConnRegister)
        End Try
    End Sub

    ''' <summary>
    ''' Insert ข้อมูล Tablet ให้เป็นเครื่องสำรอง
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function RegisterTabletSpare(ByRef InputConn As SqlConnection) As Boolean
        If txtTabletName.Text.Trim() <> "" Then
            Try
                _DB.OpenWithTransection(InputConn)
                lblValidate.Visible = False
                'Update ข้อมูลใน TabletOwner ก่อนเผื่อว่าเคยผูกกับคนไว้ก่อนแล้ว
                Dim sql As String = " UPDATE dbo.t360_tblTabletOwner SET TabletOwner_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Owner_Id IN " &
                                    " (SELECT Owner_Id FROM dbo.t360_tblTabletOwner INNER JOIN dbo.t360_tblTablet ON dbo.t360_tblTabletOwner.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " &
                                    " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' AND dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 " &
                                    " AND dbo.t360_tblTablet.Tablet_IsActive = 1) "
                _DB.ExecuteWithTransection(sql, InputConn)

                'update tabletdesk เก่า เผื่อเคย เป็นเครื่อง lab เก่า
                sql = String.Format(SQLUpdateTabletLabDesk(), DeviceId)
                _DB.ExecuteWithTransection(sql, InputConn)

                'Update t360_tblTablet
                sql = " UPDATE dbo.t360_tblTablet SET Tablet_IsOwner = 0 , Tablet_TabletName = '" & _DB.CleanString(txtTabletName.Text.Trim()) & "' , LastUpdate = dbo.GetThaiDate() , Tablet_LastUpdate = dbo.GetThaiDate() " &
                      " WHERE Tablet_SerialNumber = '" & DeviceId & "' AND Tablet_IsActive = 1 "
                _DB.ExecuteWithTransection(sql, InputConn)

                'Update TabletStatus ให้เป็น 0 ก่อน
                sql = " Update t360_tblTabletStatusDetail set IsActive = 0, LastUpdate = dbo.GetThaiDate() " &
                      " where Tablet_Id in(select Tablet_Id from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "')"
                _DB.ExecuteWithTransection(sql, InputConn)

                'Insert สถานะลงทะเบียน
                sql = " insert into t360_tblTabletStatusDetail " &
                      " select  newid(),Tablet_Id,1,3,null,null,null,null,null,null,'" & _DB.CleanString(txtTabletName.Text.Trim()) & "',School_Code,dbo.GetThaiDate(),1,null from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "';"
                _DB.ExecuteWithTransection(sql, InputConn)
                _DB.CommitTransection(InputConn)
                Return True
            Catch ex As Exception
                _DB.RollbackTransection(InputConn)
                lblValidate.Text = "บางอย่างผิดพลาดโปรดลองอีกครั้ง"
                lblValidate.Visible = True
                Throw ex
                Return False
            End Try
        Else
            lblValidate.Text = "ต้องพิมพ์ชื่อแท็บเลตด้วยนะคะ"
            lblValidate.Visible = True
            Return False
        End If
    End Function


    Private Function SQLUpdateTabletLabDesk() As String
        Dim sql As New StringBuilder
        sql.Append(" DECLARE @tabid AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_IsActive = 1 AND Tablet_SerialNumber = '{0}');")
        sql.Append(" UPDATE tblTabletLabDesk SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Tablet_Id = @tabid AND IsActive = 1;")
        Return sql.ToString()
    End Function
End Class