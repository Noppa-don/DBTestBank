Imports System.Web
Public Class MyTestSet
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If HttpContext.Current.Session("QC_UserId") IsNot Nothing And HttpContext.Current.Session("QC_UserId").ToString() <> "" Then
                BindRepeater(HttpContext.Current.Session("QC_UserId").ToString())
            End If
        End If
    End Sub

    Private Sub BindRepeater(ByVal UserId As String, Optional ByVal SearchTestsetName As String = "")
        Dim sql As String = " SELECT dbo.tblQuizCreatorTestset.QCT_Id,dbo.tblQuizCreatorTestset.Testset_Id,dbo.tblQuizCreatorTestset.QCT_IsOnline,dbo.tblTestSet.TestSet_Name " & _
                            " ,dbo.tblQuizCreatorTestset.QCT_AVGRating,dbo.tblQuizCreatorTestset.QCT_TotalUse, " & _
                            " CASE WHEN dbo.tblQuizCreatorTestset.QCT_IsOnline = 1 THEN 'eye' ELSE 'closeEye' END AS ImageName" & _
                            " ,CASE WHEN dbo.tblQuizCreatorTestset.QCT_IsOnline = 1 THEN 'OpenEye' ELSE 'CloseEye' END	AS ClassName " & _
                            " ,CASE WHEN dbo.tblQuizCreatorTestset.QCT_IsOnline = 1 THEN 'วันที่อัพโหลด: ' + CAST(CAST(dbo.tblQuizCreatorTestset.QCT_UploadDate AS DATE)AS VARCHAR(50)) + ' , จำนวนคนดาวนโหลด:' + CAST(dbo.tblQuizCreatorTestset.QCT_TotalDownload AS VARCHAR(50)) + ' คน' ELSE '' END AS txtDetail " & _
                            " FROM dbo.tblQuizCreatorTestset INNER JOIN dbo.tblTestSet ON dbo.tblQuizCreatorTestset.Testset_Id = dbo.tblTestSet.TestSet_Id " & _
                            " WHERE dbo.tblTestSet.UserId = '" & UserId & "' AND dbo.tblTestSet.IsActive = 1 AND dbo.tblQuizCreatorTestset.QCT_IsActive = 1 "
        If SearchTestsetName <> "" Then
            sql &= " AND dbo.tblTestSet.TestSet_Name LIKE '%" & _DB.CleanString(SearchTestsetName.Trim()) & "%' "
        End If
        sql &= " ORDER BY dbo.tblQuizCreatorTestset.QCT_UploadDate DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            RptItem.DataSource = dt
            RptItem.DataBind()
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function UpdateIsOnline(ByVal QCT_Id As String, ByVal UpdateValue As Integer)
        Dim _DB As New ClassConnectSql()
        Try
            Dim sql As String = " UPDATE dbo.tblQuizCreatorTestset SET QCT_IsOnline = '" & UpdateValue & "',QCT_Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCT_Id = '" & QCT_Id.ToString() & "' "
            _DB.Execute(sql)
            _DB = Nothing
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB = Nothing
            Return "Error"
        End Try
    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteQCTestset(ByVal QCT_Id As String)
        Dim _DB As New ClassConnectSql()
        Try
            Dim sql As String = " UPDATE dbo.tblQuizCreatorTestset SET QCT_IsActive = 0,QCT_Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCT_Id = '" & QCT_Id.ToString() & "' "
            _DB.Execute(sql)
            _DB = Nothing
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB = Nothing
            Return "Error"
        End Try
    End Function

    Private Sub btnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click
        If txtSearch.Text <> "" Then
            BindRepeater(HttpContext.Current.Session("QC_UserId").ToString(), txtSearch.Text)
        Else
            BindRepeater(HttpContext.Current.Session("QC_UserId").ToString())
        End If
    End Sub

End Class