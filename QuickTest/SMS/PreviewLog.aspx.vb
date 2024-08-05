Public Class PreviewLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindGrid()
        End If
    End Sub

    Private Sub BindGrid()
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT dbo.tblExternalApp.ExternalAppName as AppId,dbo.tblExternalLogApp.DeviceId as DeviceUniqueId, " & _
                            " CASE WHEN LogAppType = 0 THEN 'RunApp' WHEN LogAppType = 1 THEN 'EnterStation' " & _
                            " WHEN LogAppType = 2 THEN 'Action' WHEN LogAppType = 3 THEN 'Edit' WHEN LogAppType = 4 THEN 'Close' " & _
                            " END AS ActionType,dbo.tblExternalLogApp.ActionTimeStamp, dbo.tblExternalLogApp.LastUpdate as ServerReceivedDateTime FROM dbo.tblExternalLogApp  INNER JOIN dbo.tblExternalApp " & _
                            " ON dbo.tblExternalLogApp.ExternalAppId = dbo.tblExternalApp.ExternalAppId " & _
                            " WHERE dbo.tblExternalApp.IsActive = 1 AND dbo.tblExternalLogApp.IsActive = 1 ORDER BY dbo.tblExternalLogApp.LastUpdate DESC; "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            GridView1.DataSource = dt
            GridView1.DataBind()
        Else
            Response.Write("ไม่พบข้อมูล")
        End If
    End Sub

End Class