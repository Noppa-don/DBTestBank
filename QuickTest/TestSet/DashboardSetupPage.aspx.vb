Imports BusinessTablet360
Public Class DashboardSetupPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
#End If
        Log.Record(Log.LogType.PageLoad, "pageload dashboardtestset", False)

        If Not Page.IsPostBack Then
            Log.RecordLog(Log.LogCategory.DashboardPrint, Log.LogAction.PageLoad, True, "เข้าเมนูใบงาน", Session("UserId").ToString)
        End If

        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "dashboardtestset session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        End If

        Dim UserID As String = Session("UserId").ToString()
        MyCtlTestset.RepeaterTestsetControl(EnumDashBoardType.SetUp, UserID)

        'ทุกครั้งที่โหลดหน้านี้ Clear session ทิ้งทุกครั้ง
        Session("EditID") = Nothing
        Session("newTestSetId") = Nothing
        Session("objTestSet") = Nothing
        Session("TempTestset") = Nothing
    End Sub

End Class