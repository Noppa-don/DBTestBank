Public Class GenUserManagerPage
    Inherits System.Web.UI.Page

    Implements IPostBackEventHandler
    Dim ClsUser As New ServerBudget(New ClassConnectSql)

    Public Property SelectId() As String
        Get
            Return ViewState("ID")
        End Get
        Set(ByVal value As String)
            ViewState("ID") = value
        End Set
    End Property

    Public Property SchoolId() As Integer
        Get
            Return ViewState("SID")
        End Get
        Set(ByVal value As Integer)
            ViewState("SID") = value
        End Set
    End Property

    Dim CsSql As New ClassConnectSql

    Private Sub BtnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnFind.Click
        'Log.Record(Log.LogType.ManageUser, "ค้นหาข้อมูล", True)
        SearchData()
    End Sub

    Private Sub GvUser_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GvUser.ItemCommand

        If e.CommandName = "Delete" Then
            SelectId = e.CommandArgument.ToString

            ClsUser.DeleteUser(SelectId)
            Dim msg As String
            msg = "ลบข้อมุลเรียบร้อยแล้วค่ะ"
            'WindowsTelerik.ShowConfirmDouble(msg, Me, RadDialogConfirmFirst)
            WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
            Log.Record(Log.LogType.GenUser, "ลบข้อมูล", True)
            SearchData()
        End If

        If e.CommandName = "Update" Then
            Dim id = CType(e.CommandArgument, String)
            'Log.Record(Log.LogType.ManageUser, "แก้ไข", True)

            Response.Redirect("~/QuickTestGenUser/GenUserPage.aspx?id=" & id.ToString, False)
        End If

        If e.CommandName = "ViewLog" Then
            Dim id = CType(e.CommandArgument, String)
            Response.Redirect("~/QuickTestGenUser/FilterAndPrintLog.aspx?id=" & id.ToString, False)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("IsLoggedIn") = Nothing Then
            Response.Redirect("~/QuickTestGenUser/LoginGenUser.aspx", False)
        Else
            If Not IsPostBack Then

                'BindCombo()
                If ConfigurationManager.AppSettings("IsQuicktestProduction") = False Then
                    Lc1.Visible = False
                    tdSearch.Attributes("class") = "style6"
                    txtSchoolCode.Text = "1000001"
                End If
                If Request.QueryString("state") = "insert" Then
                    WindowsTelerik.ShowAlert("บันทึกข้อมูลเรียบร้อยค่ะ", Me, RadDialogAlert)
                ElseIf Request.QueryString("state") = "update" Then
                    WindowsTelerik.ShowAlert("แก้ไขข้อมูลเรียบร้อยค่ะ", Me, RadDialogAlert)
                End If

            End If
        End If
    End Sub

    Public Sub RaisePostBackEvent1(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent
        Select Case eventArgument
            Case "delete"
                Dim CsSql As New ClassConnectSql
                Dim sql As String = "Update tblUser Set IsActive = 0 where UserId = " & SelectId & ""
                CsSql.Execute(sql)
        End Select
        SearchData()

    End Sub

    Private Sub SelectSchool()

        Dim CsSql As New ClassConnectSql
        Dim sql = "select * from tblSchool where (1=1) and IsActive = 1  "

        If Lc1.ProvinceId <> 0 Then
            sql = sql + " and ProvinceId =" & Lc1.ProvinceId & " "
        End If

        If Lc1.DistrictId <> 0 Then
            sql = sql + " and AmphurId =" & Lc1.DistrictId & " "
        End If

        If Lc1.SubDistrictId <> 0 Then
            sql = sql + " and TambolId =" & Lc1.SubDistrictId & " "
        End If

        With CmbSchool
            .DataTextField = "SchoolName"
            .DataValueField = "SchoolId"
            Dim data As DataTable = CsSql.getdata(sql)
            Dim Row = data.NewRow
            Row("SchoolName") = "เลือก"
            Row("SchoolId") = 0
            data.Rows.InsertAt(Row, 0)
            .DataSource = data
            .DataBind()
        End With



    End Sub

    Public Sub SearchData()

        Dim CsSql As New ClassConnectSql
        Dim sql = "select 0 as a,GUID as UserId,FirstName,LastName,UserName from tblUser where (1=1) and IsActive = 1 and SchoolId <> 0"
        If txtName.Text <> "" Then
            sql = sql + " and UserName like'%" & txtName.Text & "%' "
        End If

        If CmbSchool.SelectedValue <> "" Then

            If CmbSchool.SelectedValue <> 0 Then
                sql = sql + " and SchoolId = " & CmbSchool.SelectedValue.ToString
            End If
            SchoolId = CmbSchool.SelectedValue
        End If

        If txtSchoolCode.Text <> "" And CmbSchool.SelectedValue = "" Then
            sql = sql + " and SchoolId = " & txtSchoolCode.Text
            CmbSchool.SelectedValue = txtSchoolCode.Text
            SchoolId = txtSchoolCode.Text

        End If


        Dim dt = CsSql.getdata(sql)
        Dim No As Integer = 1
        For Each r In dt.Rows
            r("a") = No
            No += 1
        Next

        GvUser.DataSource = dt
        GvUser.DataBind()
    End Sub

    Private Sub Lc1_SubDistrictChange() Handles Lc1.SubDistrictChange
        SelectSchool()
    End Sub

    Protected Sub lbAddUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAddUser.Click

        If Not SchoolId.ToString = 0 Then
            'Log.Record(Log.LogType.ManageUser, "เพิ่มผู้ใช้ใหม่", True)
            Response.Redirect("~/QuickTestGenUser/GenUserPage.aspx?SchoolId=" & SchoolId.ToString, False)
        ElseIf txtSchoolCode.Text <> "" Then
            Response.Redirect("~/QuickTestGenUser/GenUserPage.aspx?SchoolId=" & txtSchoolCode.Text, False)
        Else
            WindowsTelerik.ShowAlert("เลือกโรงเรียนก่อนนะคะ", Me, RadDialogAlert)
        End If

    End Sub

    Protected Sub CmbSchool_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CmbSchool.SelectedIndexChanged
        SchoolId = CmbSchool.SelectedValue
        txtSchoolCode.Text = CmbSchool.SelectedValue
    End Sub

    'Protected Sub lbtnHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHome.Click
    '    'Log.Record(Log.LogType.Home, "กลับหน้าหลัก", True)
    '    'Response.Redirect("~/MenuPage.aspx", False)
    'End Sub

End Class