Public Class PrepareDataMaxonetAppium
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Response.Clear()
        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim deviceName As String = txtDeviceName.Text.Trim()
        Dim keyCodeName As String = txtKeyCodeName.Text.Trim()

        If deviceName = "" OrElse keyCodeName = "" Then
            Response.Write("ใส่ค่าให้ครบก่อนนะ")
        Else
            Dim m As New MaxOnetManagement
            m.PrepareDataAppium(deviceName, keyCodeName)
            Response.Write("OK เรียบร้อย")
        End If
    End Sub

End Class