Public Class AdminLogPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not IsPostBack Then
                If Not IsPostBack Then
                    LogType()

                End If

            End If
        End If
    End Sub
    Public Sub SearchData()
        Dim CsSql As New ClassConnectSql
        Dim sql = "SELECT 0 as a , * from uvw_AdminLog where isactive = 1"

        If Lc1.ProvinceId <> 0 Then
            sql = sql + " and ProvinceId =" & Lc1.ProvinceId & " "
        End If

        If Lc1.DistrictId <> 0 Then
            sql = sql + " and AmphurId =" & Lc1.DistrictId & " "
        End If

        If Lc1.SubDistrictId <> 0 Then
            sql = sql + " and TambolId =" & Lc1.SubDistrictId & " "
        End If

        If cbLogType.SelectedValue <> "0" Then
            sql = sql + " and LogType =" & cbLogType.SelectedValue & " "
        End If


        Dim dt = CsSql.getdata(sql)

        Dim No As Integer = 1
        For Each r In dt.Rows
            r("a") = No
            No += 1
        Next

        GvAdminLog.DataSource = dt
        GvAdminLog.DataBind()
    End Sub

    Protected Sub BtnFind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnFind.Click
        Log.Record(Log.LogType.AdminLog, "ค้นหา", True)
        SearchData()
    End Sub
    Private Sub LogType()
        'Dim showdata As DataTable
        'Dim DB As New ClassConnectSql
        'showdata = DB.getdata("Select ICId,ICName from tblItemCategory where IsActive =1 union select 0,'ทั้งหมด' order by ICID")

        cbLogType.DataTextField = "Text"
        cbLogType.DataValueField = "Value"
        cbLogType.DataSource = (New EnumLogType)
        cbLogType.DataBind()
    End Sub

    Private Sub GvAdminLog_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles GvAdminLog.SortCommand
        SearchData()
    End Sub

    Protected Sub lbtnHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHome.Click
        Log.Record(Log.LogType.Home, "กลับหน้าหลัก", True)
        Response.Redirect("~/MenuPage.aspx", False)
    End Sub
End Class