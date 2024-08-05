Public Class PreviewAllLogPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindAllGrid()
    End Sub

    Private Sub BindAllGrid()
        Try
            Dim sql As String = ""
            Dim dt As New DataTable
            'tblExternalApp
            sql = " SELECT TOP 50 * FROM dbo.tblExternalApp WHERE IsActive = 1 ORDER BY LastUpdate DESC  "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternallApp.DataSource = dt
                GridtblExternallApp.DataBind()
            End If
            dt.Clear()

            'tblExternalAppStation
            sql = " SELECT TOP 50 * FROM dbo.tblExternalAppStation WHERE IsActive = 1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalAppStation.DataSource = dt
                GridtblExternalAppStation.DataBind()
            End If
            dt.Clear()

            'tblExternalLogAppStation
            sql = " SELECT TOP 50 * FROM dbo.tblExternalLogAppStation WHERE IsActive = 1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalLogAppStation.DataSource = dt
                GridtblExternalLogAppStation.DataBind()
            End If
            dt.Clear()

            'tblExterlLogApp
            sql = " SELECT TOP 50 * FROM dbo.tblExternalLogApp WHERE IsActive = 1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalLogApp.DataSource = dt
                GridtblExternalLogApp.DataBind()
            End If
            dt.Clear()


            'tblExterlLogAction
            sql = " SELECT TOP 50 * FROM dbo.tblExternalLogAction WHERE IsActive = 1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalLogAction.DataSource = dt
                GridtblExternalLogAction.DataBind()
            End If
            dt.Clear()

            'tblExterlAppQuestion
            sql = " SELECT TOP 50 * FROM dbo.tblExternalAppQuestion WHERE IsActive = 1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalAppQuestion.DataSource = dt
                GridtblExternalAppQuestion.DataBind()
            End If
            dt.Clear()

            'tblExterlAppAnswer
            sql = " SELECT TOP 50 * FROM dbo.tblExternalAppAnswer WHERE IsActive =1 ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                GridtblExternalAppAnswer.DataSource = dt
                GridtblExternalAppAnswer.DataBind()
            End If
            dt.Clear()

        Catch ex As Exception
            Response.Write(ex.ToString())
        End Try
    End Sub

End Class