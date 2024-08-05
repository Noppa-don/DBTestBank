Public Class LocalControl
    Inherits System.Web.UI.UserControl
    Public Event SubDistrictChange()


#Region "Property"

    Public Property ProvinceId() As Integer
        Get
            Return CbProvince.SelectedValue
        End Get
        Set(ByVal value As Integer)
            CbProvince.SelectedValue = value
        End Set
    End Property

    Public Property DistrictId() As Integer
        Get
            If CbDistrict.SelectedIndex = -1 Then
                Return Nothing
            End If
            Return CbDistrict.SelectedValue
        End Get
        Set(ByVal value As Integer)
            CbDistrict.SelectedValue = value
        End Set
    End Property

    Public Property SubDistrictId() As Integer
        Get
            If CbSubDistrict.SelectedIndex = -1 Then
                Return Nothing
            End If
            Return CbSubDistrict.SelectedValue
        End Get
        Set(ByVal value As Integer)
            CbSubDistrict.SelectedValue = value
        End Set
    End Property

    Private _Mode As EnUserControlMode = EnUserControlMode.Insert
    Public Property Mode() As EnUserControlMode
        Get
            Return _Mode
        End Get
        Set(ByVal value As EnUserControlMode)
            _Mode = value
        End Set
    End Property

#End Region

#Region "Function"

    Public Sub BindCbProvice()
        With CbProvince
            .Text = ""
            .DataTextField = "ProvinceName"
            .DataValueField = "ProvinceId"
            Dim showdata As DataTable
            Dim DB As New ClassConnectSql
            showdata = DB.getdata("Select * from tblProvince where IsActive =1")
            Dim Row = showdata.NewRow
            Row("ProvinceName") = "เลือก"
            Row("ProvinceId") = 0
            showdata.Rows.InsertAt(Row, 0)
            .DataSource = showdata
            .DataBind()
        End With
    End Sub

    Private Sub BindCbDistrict(ByVal SelectId As Integer)
        With CbDistrict
            .Text = ""
            .DataTextField = "AmphurName"
            .DataValueField = "AmphurId"
            Dim showdata As DataTable
            Dim DB As New ClassConnectSql
            showdata = DB.getdata("Select * from tblAmphur where IsActive =1 and ProvinceId = " & SelectId & " ")
            Dim Row = showdata.NewRow
            Row("AmphurName") = "เลือก"
            Row("AmphurId") = 0
            showdata.Rows.InsertAt(Row, 0)
            .DataSource = showdata
            .DataBind()
        End With
    End Sub

    Private Sub BindCbSubDistrict(ByVal SelectId As Integer)
        With CbSubDistrict
            .Text = ""
            .DataTextField = "TambolName"
            .DataValueField = "TambolId"
            Dim showdata As DataTable
            Dim DB As New ClassConnectSql
            showdata = DB.getdata("Select * from tblTambol where IsActive =1 and AmphurId = " & SelectId & " ")
            Dim Row = showdata.NewRow
            Row("TambolName") = "เลือก"
            Row("TambolId") = 0
            showdata.Rows.InsertAt(Row, 0)
            .DataSource = showdata
            .DataBind()
            RaiseEvent SubDistrictChange()
        End With
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Mode = EnUserControlMode.Insert Then
                BindCbProvice()
            Else
                BindCbProvice()
                BindCbDistrict(CbProvince.SelectedValue)
                BindCbSubDistrict(CbDistrict.SelectedValue)
            End If
        End If
    End Sub

    Private Sub CbProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CbProvince.SelectedIndexChanged
        BindCbDistrict(CbProvince.SelectedValue)
        BindCbSubDistrict(0)
    End Sub

    Private Sub CbDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles CbDistrict.SelectedIndexChanged
        BindCbSubDistrict(CbDistrict.SelectedValue)
    End Sub
End Class

Public Enum EnUserControlMode
    Insert
    Edit
End Enum