Imports BusinessTablet360
Imports System.Data.SqlClient

Public Class DashboardQuizPage
    Inherits System.Web.UI.Page

    Public IE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Request.QueryString("u") Is Nothing Then
            Dim u As String = Request.QueryString("u").ToString()
            Dim dt As DataTable = GetUserDetailForTablet(u)
            If dt.Rows.Count > 0 Then
                Session("UserId") = dt(0)("GUID")
                Session("UserName") = dt(0)("UserName")
                Session("FirstName") = dt(0)("FirstName")
                Session("LastName") = dt(0)("LastName")
                Session("IsAllowMenuManageUserSchool") = dt.Rows(0)("IsAllowMenuManageUserSchool")
                Session("IsAllowMenuManageUserAdmin") = dt.Rows(0)("IsAllowMenuManageUserAdmin")
                Session("IsAllowMenuAdminLog") = dt.Rows(0)("IsAllowMenuAdminLog")
                Session("IsAllowMenuContact") = dt.Rows(0)("IsAllowMenuContact")
                Session("IsAllowMenuSetEmail") = dt.Rows(0)("IsAllowMenuSetEmail")
                Session("SchoolID") = dt.Rows(0)("SchoolId")
                Session("SchoolCode") = dt.Rows(0)("SchoolId")
                Session("TeacherId") = dt.Rows(0)("SchoolId")
                Session("IsTeacher") = True
                Session("UnLoad") = False 'ใช้กับ signalR
                If IsDBNull(dt.Rows(0)("FontSize")) Then
                    Session("FontSize") = 0
                Else
                    Session("FontSize") = dt.Rows(0)("FontSize")
                End If
                Dim ClsSelectSession As New ClsSelectSession()
                ClsSelectSession.NewSession("student/dashboardquizpage.aspx")

                'ส่วนของการเช็คว่าจะโชว์ Qtip แสดงการใช้งานในแต่ละหน้าหรือเปล่า    
                Dim ClsUserViewPageWithTip As New UserViewPageWithTip(dt(0)("GUID").ToString())
                If IsDBNull(dt.Rows(0)("IsViewAllTips")) Then
                    ClsUserViewPageWithTip.CheckIsViewAllTips(False)
                Else
                    ClsUserViewPageWithTip.CheckIsViewAllTips(dt.Rows(0)("IsViewAllTips"))
                End If
            End If
        End If

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
#End If
        Log.Record(Log.LogType.PageLoad, "pageload dashboardquiz", False)

        If Session("UserId") Is Nothing Then
            Log.Record(Log.LogType.PageLoad, "dashboardquiz session หลุด", False)
            Response.Redirect("~/loginpage.aspx")
        End If

        Dim UserID As String = Session("UserId").ToString()
        'repeater
        MyCtlTestset.RepeaterTestsetControl(EnumDashBoardType.Quiz, UserID)

        ' GRAPH //ไม่ต้องส่ง connection เข้าไป เพราะเปิดรอบเดียว
        Dim ClsGraphDashboard As New ClsGraphDashboard(EnumDashBoardType.Quiz, UserID)
        Dim dtDataGraph As DataTable = ClsGraphDashboard.GetDataGraph()
        If Not dtDataGraph.Rows.Count = 0 Then
            RadChartDashboard.DataSource = dtDataGraph

            Dim value As Integer = dtDataGraph.AsEnumerable().Max(Function(row) row("NoOfStu"))
            ' ถ้าจำนวนข้อมูลทุก row มีค่าต่ำกว่า 5 ให้ set ค่า max = 5 ใน แกน y
            If value < 5 Then
                RadChartDashboard.PlotArea.YAxis.AutoScale = False
                RadChartDashboard.PlotArea.YAxis.LabelStep = 1
                RadChartDashboard.PlotArea.YAxis.MaxValue = 5
            Else
                RadChartDashboard.PlotArea.YAxis.AutoScale = True
            End If


            ' ถ้าข้อมมูลมากกว่า 15 ปรับเป็นแนวนอน
            If dtDataGraph.Rows.Count > 15 Then
                RadChartDashboard.Height = dtDataGraph.Rows.Count * 40
                RadChartDashboard.SeriesOrientation = Telerik.Charting.ChartSeriesOrientation.Horizontal
            Else
                RadChartDashboard.Height = 250
            End If

            RadChartDashboard.PlotArea.XAxis.DataLabelsColumn = "ClassName"
            RadChartDashboard.DataBind()

            'ปรับสีบนแท่ง บาร์
            For Each item As Telerik.Charting.ChartSeriesItem In RadChartDashboard.Series(0).Items
                item.Label.TextBlock.Appearance.TextProperties.Color = Drawing.Color.Black
            Next
        Else
            RadChartDashboard.Visible = False
        End If

    End Sub

    Private Function GetUserDetailForTablet(ByVal UserId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblUser LEFT JOIN tblUserSetting ON tblUser.GUID = tblUserSetting.User_Id ")
        sql.Append(" WHERE tblUser.GUID = '")
        sql.Append(UserId)
        sql.Append("';")
        GetUserDetailForTablet = New ClassConnectSql().getdata(sql.ToString())
    End Function
End Class