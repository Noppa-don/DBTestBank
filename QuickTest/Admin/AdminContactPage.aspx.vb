Public Class AdminContactPage
    Inherits System.Web.UI.Page
    Dim conDB As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If (Not IsPostBack) Then
                LoadData()
            End If
        End If
    End Sub

    'Public Sub bindGv()
    '    Dim sql As String = "SELECT * FROM tblQuestion ORDER BY QuestionID DESC"
    '    Dim dt = conDB.getdata(sql)
    '    ' rename headColumn
    '    dt.Columns("QuestionID").ColumnName = "ลำดับล่าสุดของการแจ้ง"
    '    dt.Columns("Title").ColumnName = "สอบถามเกี่ยวกับเรื่อง"
    '    dt.Columns("Description").ColumnName = "คำถาม,แนะนำ,หรือข้อสงสัย"
    '    dt.Columns("Tel").ColumnName = "ชื่อ,เบอร์โทร"
    '    dt.Columns("IsTel").ColumnName = "ต้องการให้โทรกลับ"

    '    ' bind datasourc 
    '    GvContact.DataSource = dt

    '    GvContact.AllowPaging = True
    '    GvContact.PageSize = 20


    '    GvContact.DataBind()
    'End Sub

    Private Sub LoadData()
        ' query DB
        Dim sql As String = "SELECT * FROM tblUserContact ORDER BY ReceivedDate DESC"
        Dim dt = conDB.getdata(sql)

        ' datasource
        GvContact.DataSource = dt
        ' rename headColumn
        'dt.Columns("QuestionID").ColumnName = "ลำดับล่าสุดของการแจ้ง"
        'dt.Columns("Title").ColumnName = "สอบถามเกี่ยวกับเรื่อง"
        'dt.Columns("Description").ColumnName = "คำถาม,แนะนำ,หรือข้อสงสัย"
        'dt.Columns("Tel").ColumnName = "ชื่อ,เบอร์โทร"
        'dt.Columns("IsTel").ColumnName = "ต้องการให้โทรกลับ"
    End Sub


    Protected Sub GvContact_PageIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles GvContact.PageIndexChanged
        LoadData()
    End Sub

    Protected Sub GvContact_PageSizeChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles GvContact.PageSizeChanged
        LoadData()
    End Sub

    Protected Sub GvContact_SortCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridSortCommandEventArgs) Handles GvContact.SortCommand
        LoadData()
    End Sub



    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnHome.Click
        Log.Record(Log.LogType.Home, "กลับหน้าหลัก", True)
        Response.Redirect("~/MenuPage.aspx", False)
    End Sub
End Class