Public Class MaxOnetKeyPage
    Inherits System.Web.UI.Page

    Private dbLicenseKey As New ClassConnectSql(False, ConfigurationManager.ConnectionStrings("LicensKeyConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitialData()
    End Sub

    Private Sub InitialData()
        Dim dtMaxOnetKey As DataTable = GetMaxOnetKey()
        If dtMaxOnetKey IsNot Nothing Then
            gvMaxOnetKey.DataSource = dtMaxOnetKey
            gvMaxOnetKey.DataBind()
        End If

    End Sub


    Private Function GetMaxOnetKey() As DataTable
        Try
            dbLicenseKey.OpenWithTransection()
            Dim sql As String = "select keycode_userName,keycode_Code,KeyCode_Type,KeyCode_DateFirstRegister from maxonet_tblKeyCode order by KeyCode_Type;"
            Dim dt As DataTable = dbLicenseKey.getdataWithTransaction(sql)
            dbLicenseKey.CommitTransection()
            Return dt
        Catch ex As Exception
            dbLicenseKey.RollbackTransection()
            Return Nothing
        End Try
    End Function

End Class