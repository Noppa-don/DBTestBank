Public Class AdminManagerPage
    Inherits System.Web.UI.Page
    Implements IPostBackEventHandler
    Dim ClsUser As New ServerBudget(New ClassConnectSql)
    Dim CsSql As New ClassConnectSql
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not IsPostBack Then

                'BindCombo()

                If Request.QueryString("state") = "insert" Then
                    WindowsTelerik.ShowAlert("บันทึกข้อมูลเรียบร้อยค่ะ", Me, RadDialogAlert)
                ElseIf Request.QueryString("state") = "update" Then
                    WindowsTelerik.ShowAlert("แก้ไขข้อมูลเรียบร้อยค่ะ", Me, RadDialogAlert)
                End If
                SearchData()
            End If
        End If
    End Sub
    Protected Sub lbtnHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHome.Click
        '  Log.Record(Log.LogType.Home, "กลับหน้าหลัก", True)
        Response.Redirect("~/MenuPage.aspx", False)
    End Sub
    Public Property SelectId() As Integer
        Get
            Return ViewState("ID")
        End Get
        Set(ByVal value As Integer)
            ViewState("ID") = value
        End Set
    End Property

    Private Sub GvUserAdmin_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GvUserAdmin.ItemCommand

        If e.CommandName = "Delete" Then
            SelectId = e.CommandArgument
            Dim Msg1 As String = "เมื่อต้องการลบข้อมูล ข้อมูลผู้ใช้นี้จะถูกลบทั้งหมด แน่ใจนะคะ?"
            Dim Msg2 As String = "ลบข้อมูลเรียบร้อยคะ"
            Dim Msg As String = Msg1 & "," & Msg2
            WindowsTelerik.ShowConfirmDouble(Msg, Me, RadDialogConfirmFirst)
            Log.Record(Log.LogType.ManageUserAdmin, "ลบข้อมูล", True)
            SearchData()
        End If

        If e.CommandName = "Update" Then
            Dim id = CType(e.CommandArgument, Integer)
            Log.Record(Log.LogType.ManageUserAdmin, "แก้ไข", True)
            Response.Redirect("~/Admin/AdminPage.aspx?id=" & id.ToString, False)
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
    Public Sub SearchData()

        Dim CsSql As New ClassConnectSql
        Dim sql = "select 0 as a,UserId,FirstName,LastName,UserName from tblUser where (1=1) and IsActive = 1 and SchoolId = 0"
        Dim dt = CsSql.getdata(sql)
        Dim No As Integer = 1
        For Each r In dt.Rows
            r("a") = No
            No += 1
        Next

        GvUserAdmin.DataSource = dt
        GvUserAdmin.DataBind()
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        Log.Record(Log.LogType.ManageUser, "เพิ่มผู้ใช้ใหม่", True)
        Response.Redirect("~/Admin/AdminPage.aspx", False)

    End Sub

End Class