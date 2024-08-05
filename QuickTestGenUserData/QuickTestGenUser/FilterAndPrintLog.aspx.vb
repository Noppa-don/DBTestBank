Imports System.IO

Public Class FilterAndPrintLog
    Inherits System.Web.UI.Page

    Dim CsSql As New ClassConnectSql
    Dim ClsPDF As New ClsPDF(CsSql)
    Dim clsexcel As New clsExportExcel

    Public Property dt() As DataTable
        Get
            Return ViewState("dtLog")
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtLog") = value
        End Set
    End Property

    Public Property dtLevel() As DataTable
        Get
            Return ViewState("dtLevelLog")
        End Get
        Set(value As DataTable)
            ViewState("dtLevelLog") = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return ViewState("SelectedStartDate")
        End Get
        Set(ByVal value As String)
            ViewState("SelectedStartDate") = value
        End Set
    End Property
    Public Property EndDate() As String
        Get
            Return ViewState("SelectedEndDate")
        End Get
        Set(ByVal value As String)
            ViewState("SelectedEndDate") = value
        End Set
    End Property
    Public Property LogUserName() As String
        Get
            Return ViewState("vLogUserName")
        End Get
        Set(ByVal value As String)
            ViewState("vLogUserName") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsLoggedIn") = Nothing Then
            Response.Redirect("~/QuickTestGenUser/LoginGenUser.aspx", False)
        Else
            If Not Page.IsPostBack Then
                DpkEndDate.SelectedDate = Date.Now
                DpkStartDate.SelectedDate = Date.Now
                btnExport.Visible = False
                GetUserLogName()
            End If

        End If

        clsexcel.excelreport()

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportExcel(LogUserName, dt)

    End Sub

    Private Sub ExportExcel(LogUserName As String, dt As Data.DataTable)

        Dim FileName As String = "UserLog_" & LogUserName & "_" & DateTime.Now.ToString("ddMMyy") & ".xls"

        dt.Columns("LogDate").ColumnName = "วันที่"
        dt.Columns("QNo").ColumnName = "ลำดับข้อ"
        dt.Columns("QSetName").ColumnName = "ชุด"
        dt.Columns("QCatName").ColumnName = "บท"
        dt.Columns("LogDetail").ColumnName = "รายละเอียด"
        dt.Columns("LevelName").ColumnName = "ชั้น"

        Dim dgGrid As New DataGrid
        dgGrid.DataSource = dt
        dgGrid.DataBind()
        dgGrid.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
        dgGrid.ItemStyle.HorizontalAlign = HorizontalAlign.Left
        dgGrid.HeaderStyle.VerticalAlign = WebControls.VerticalAlign.Middle
        dgGrid.ItemStyle.VerticalAlign = WebControls.VerticalAlign.Middle
        dgGrid.HeaderStyle.Height = "21"
        dgGrid.ItemStyle.Height = "22"

        Response.ClearContent()
        Response.AddHeader("content-disposition", "attachment;filename=" & FileName)
        Response.ContentEncoding = System.Text.Encoding.Unicode
        Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        Response.ContentType = "application/ms-excel"

        Dim tw As New System.IO.StringWriter()
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)

        dgGrid.RenderControl(hw)

        Dim HeaderTable As String = ""
        HeaderTable = "<table style='border:1px solid;vertical-align: middle;text-aling: center;'>"
        HeaderTable &= "<tr><td style='font-weight:bold;font-size:15px;border:1px solid;' colspan='3'>วันที่ " & StartDate & " - " & EndDate & "</td><td colspan='3' style='font-weight:bold;font-size:18px;border:1px solid; width:500px;'>รายงานการเข้าใช้งานระบบ Quicktest</td></tr>"
        HeaderTable &= "<tr><td style='font-weight:bold;font-size:15px;border:1px solid;' colspan='6'>User : " & LogUserName & "</td></tr></table>"
        Response.Write(HeaderTable)

        Dim ms As New MemoryStream
        Dim ds As New DataSet
        Dim dtNew As Data.DataTable = dt
        ds.Tables.Add(dtNew)

        ' ms = mdExportExcel.Main
        ms.WriteTo(Response.OutputStream)
        Response.BinaryWrite(ms.ToArray())
        Response.Write(tw.ToString())
        Response.End()

    End Sub


    Private Sub btnShowLog_Click(sender As Object, e As EventArgs) Handles btnShowLog.Click

        StartDate = PadZero(DpkStartDate.SelectedDate.Value.Day) & "/" & PadZero(DpkStartDate.SelectedDate.Value.Month) & "/" & ToDBYear(DpkStartDate.SelectedDate.Value.Year)
        Dim StartDateAddTime As String = ToDBYear(DpkStartDate.SelectedDate.Value.Year) & "-" & PadZero(DpkStartDate.SelectedDate.Value.Month) & "-" & PadZero(DpkStartDate.SelectedDate.Value.Day) & " 00:00:00"

        EndDate = PadZero(DpkEndDate.SelectedDate.Value.Day) & "/" & PadZero(DpkEndDate.SelectedDate.Value.Month) & "/" & ToDBYear(DpkEndDate.SelectedDate.Value.Year)
        Dim EndDateAndTime As String = ToDBYear(DpkEndDate.SelectedDate.Value.Year) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Month) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Day) & " 23:59:00"

        Dim Sql As String = "select l.LastUpdate as LogDate,lv.Level_ShortName as LevelName,qc.QCategory_Name as QcatName,qs.QSet_Name as QsetName,q.Question_No as QNo,qs.Qset_Id as QsetId,cast(l.Description as varchar(max)) as LogDetail
                from tblLog l
                inner join tblQuestion q on l.questionId = q.Question_Id
                inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id
                inner join tblbook b on qc.Book_Id = b.BookGroup_Id
				inner join tblLevel Lv on b.Level_Id = Lv.level_Id
                where l.QuestionId is not null and l.UserId = '" & Request.QueryString("id").ToString & "' 
                And l.LastUpdate between '" & StartDateAddTime & "' and '" & EndDateAndTime & "'
                order by l.lastupdate"

        dt = CsSql.getdata(Sql)

        If dt.Rows.Count > 0 Then
            For Each dtLog In dt.Rows
                dtLog("LogDetail") = dtLog("LogDetail").Replace("___MODULE_URL___", ClsPDF.GenFilePath(dtLog("QsetId").ToString))
            Next
            btnExport.Visible = True
        Else
            btnExport.Visible = False
        End If

        GvLog.DataSource = dt
        GvLog.DataBind()

    End Sub

    Private Sub GetUserLogName()
        Dim sql As String = "Select FirstName + ' ' + LastName from tbluser where GUID = '" & Request.QueryString("id").ToString & "'"
        LogUserName = CsSql.ExecuteScalar(sql)
        lblLogName.InnerHtml = "ดูข้อมูลการเข้าใช้งานของ " & LogUserName
    End Sub

    Private Function PadZero(InputNum As Integer) As String
        If InputNum < 10 Then
            Return "0" + InputNum.ToString
        Else
            Return InputNum.ToString
        End If
    End Function

    Private Function ToDBYear(InputYear As Integer) As String
        If InputYear > 2500 Then
            Return (InputYear - 543).ToString
        Else
            Return InputYear.ToString
        End If
    End Function

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/QuickTestGenUser/GenUserManagerPage.aspx?")
    End Sub
End Class