Public Class UserMainPage
    Inherits System.Web.UI.Page

    Dim ClsSchool As New clsSchool
    Dim ClsUser As New ClsUser
    Dim ClsValidate As New ClsValidateData

    Protected Property dtSchool As DataTable
        Get
            Return ViewState("VdtSchool")
        End Get
        Set(value As DataTable)
            ViewState("VdtSchool") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Dim dtProvice As DataTable = ClsSchool.GetProvince()
            BindDDLData("ProvinceName", "ProvinceId", dtProvice, ddlProvince)

            ddlAmphur.Items.Clear()
            ddlTambol.Items.Clear()
            ddlSchool.Items.Clear()

            dtSchool = ClsSchool.GetSchool()
        End If

    End Sub

    Private Sub ddlProvince_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvince.SelectedIndexChanged

        Dim dtAmphur As DataTable = ClsSchool.GetAmphur(ddlProvince.SelectedValue.ToString)
        BindDDLData("AmphurName", "AmphurId", dtAmphur, ddlAmphur)

        ddlTambol.Items.Clear()

        dtSchool = ClsSchool.GetSchool(ddlProvince.SelectedValue.ToString)
        BindDDLData("SchoolName", "SchoolId", dtSchool, ddlSchool)

        txtSchoolName.Text = ""

    End Sub

    Private Sub ddlAmphur_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAmphur.SelectedIndexChanged

        Dim dtTambol As DataTable = ClsSchool.GetTambol(ddlAmphur.SelectedValue.ToString)
        BindDDLData("TambolName", "TambolId", dtTambol, ddlTambol)

        dtSchool = ClsSchool.GetSchool(ddlProvince.SelectedValue.ToString, ddlAmphur.SelectedValue.ToString)
        BindDDLData("SchoolName", "SchoolId", dtSchool, ddlSchool)

        txtSchoolName.Text = ""

    End Sub

    Private Sub ddlTambol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTambol.SelectedIndexChanged
        dtSchool = ClsSchool.GetSchool(ddlProvince.SelectedValue.ToString, ddlAmphur.SelectedValue.ToString, ddlTambol.SelectedValue.ToString)
        BindDDLData("SchoolName", "SchoolId", dtSchool, ddlSchool)

        txtSchoolName.Text = ""
    End Sub

    Private Sub txtSchoolName_TextChanged(sender As Object, e As EventArgs) Handles txtSchoolName.TextChanged

        Dim dtFilterSchool As DataTable
        Dim SelectedValue As Integer = 0
        If txtSchoolName.Text <> "" Then
            Dim Data = From c In dtSchool.AsEnumerable() Where c("SchoolName").ToString().Contains(txtSchoolName.Text.Trim())
            If Data IsNot Nothing Then
                dtFilterSchool = Data.CopyToDataTable
                If dtFilterSchool.Rows.Count = 1 Then
                    SelectedValue = dtFilterSchool.Rows(0)("SchoolId")
                    Dim dtSchoolData As DataTable
                    dtSchoolData = ClsSchool.GetSchoolAddressId(SelectedValue.ToString)

                    If dtSchoolData.Rows.Count <> 0 Then

                        Dim dtProvice As DataTable = ClsSchool.GetProvince()
                        BindDDLData("ProvinceName", "ProvinceId", dtProvice, ddlProvince, dtSchoolData.Rows(0)("ProvinceId"))

                        Dim dtAmphur As DataTable = ClsSchool.GetAmphur(dtSchoolData.Rows(0)("ProvinceId"))
                        BindDDLData("AmphurName", "AmphurId", dtAmphur, ddlAmphur, dtSchoolData.Rows(0)("AmphurId"))

                        ddlTambol.Items.Clear()
                        Dim dtTambol As DataTable = ClsSchool.GetTambol(ddlAmphur.SelectedValue.ToString)
                        BindDDLData("TambolName", "TambolId", dtTambol, ddlTambol, dtSchoolData.Rows(0)("TambolId"))

                    End If
                    DetailDiv.Visible = True
                Else
                    Dim R As DataRow = dtFilterSchool.NewRow
                    R("SchoolName") = "เลือกโรงเรียน"
                    R("SchoolId") = 0
                    dtFilterSchool.Rows.Add(R)
                    dtFilterSchool = ClsValidate.OrderbyDatatable(dtFilterSchool, "SchoolId")

                    DetailDiv.Visible = False
                End If
            Else
                'เตือน
                dtFilterSchool = dtSchool
            End If
        Else
            dtFilterSchool = dtSchool
        End If

        BindDDLData("SchoolName", "SchoolId", dtFilterSchool, ddlSchool, SelectedValue)
    End Sub

    Private Sub ddlSchool_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSchool.SelectedIndexChanged
        Dim dtSchoolData As DataTable
        dtSchoolData = ClsSchool.GetSchoolAddressId(ddlSchool.SelectedValue.ToString)

        If dtSchoolData.Rows.Count <> 0 Then
            ddlAmphur.SelectedValue = dtSchoolData.Rows(0)("AmphurId")

            Dim dtTambol As DataTable = ClsSchool.GetTambol(ddlAmphur.SelectedValue.ToString)
            BindDDLData("TambolName", "TambolId", dtTambol, ddlTambol, dtSchoolData.Rows(0)("TambolId"))
        End If

    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim SelectedSchoolId As String = ddlSchool.SelectedValue.ToString
        If Not (SelectedSchoolId = "0" Or SelectedSchoolId = "" Or SelectedSchoolId Is Nothing) Then
            Dim dtUserInSelectedSchool As DataTable = ClsUser.GetUserInSchool(SelectedSchoolId)
            gvUser.DataSource = dtUserInSelectedSchool
            gvUser.DataBind()
            lblSchoolSelected.InnerText = "ผู้ใช้ระบบของโรงเรียน : " & ddlSchool.SelectedItem.ToString
            DetailDiv.Style("display") = "block"
        Else

        End If
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Dim SelectedSchoolId As String = ddlSchool.SelectedValue.ToString
        If Not (SelectedSchoolId = "0" Or SelectedSchoolId = "" Or SelectedSchoolId Is Nothing) Then
            Response.Redirect("~/ManageUser/UserPage.aspx?SchoolId=" & SelectedSchoolId)
        End If

    End Sub

    Private Sub BindDDLData(TextField As String, ValueField As String, dtSource As DataTable, objDDL As DropDownList, Optional SelectedValue As Integer = 0)

        objDDL.Items.Clear()

        With objDDL
            .DataTextField = TextField
            .DataValueField = ValueField
            .DataSource = dtSource
            .DataBind()
            .SelectedValue = SelectedValue
        End With
    End Sub
End Class