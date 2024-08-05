Imports BusinessTablet360

Public Class DashboardPrintTestsetPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not Page.IsPostBack Then
                Log.RecordLog(Log.LogCategory.DashboardPrint, Log.LogAction.PageLoad, True, "เข้าเมนูใบงาน", Session("UserId").ToString)
            End If

            Log.Record(Log.LogType.PageLoad, "printword PageLoad", False)
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
                    ClsSelectSession.NewSession("PrintTestset/dashboardprinttestsetpage.aspx")

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
#End If
            Log.Record(Log.LogType.PageLoad, "pageload dashboardprintword", False)
            If Session("UserId") Is Nothing Then
                Log.Record(Log.LogType.PageLoad, "dashboardprintword session หลุด", False)
                Response.Redirect("~/LoginPage.aspx")
            End If

            Dim UserID As String = Session("UserId").ToString()
            MyCtlTestset.RepeaterTestsetControl(EnumDashBoardType.PrintTestset, UserID)
            Log.Record(Log.LogType.PageLoad, "printword PageLoad Complete", False)

        Catch ex As Exception
            Log.Record(Log.LogType.PageLoad, "printword Error " & ex.ToString, False)
        End Try

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