Imports System.Data.SqlClient
Imports BusinessTablet360
Imports System.Web

Public Class ActivityPage_Pad
    Inherits System.Web.UI.Page

    Dim UseCls As New ClassConnectSql
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)

    Public DeviceId As String
    Public GroupName As String
    Public ActivityType As Integer
    Public ItemId As String
    Public url As String = ""
    Public F5 As String ' โหมด debug f5 สำหรับใช้ฝั่ง client

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim connActivity As New SqlConnection
        UseCls.OpenExclusiveConnect(connActivity)

        If Request.QueryString("ispreview") IsNot Nothing Then
            url = HttpContext.Current.Request.Url.PathAndQuery.ToString().Remove(0, 1).Replace("ActivityPage_Pad", "ActivityPage_Preview").Replace("Quicktest_Test/", "").Replace("quicktest_test/", "")
        Else
            url = HttpContext.Current.Request.Url.PathAndQuery.ToString().Remove(0, 1).Replace("ActivityPage_Pad", "ActivityPage_Pad2").Replace("Quicktest_Test/", "").Replace("quicktest_test/", "")
        End If

        Dim a As Integer = url.IndexOf("Activity/")
        url = url.Substring(a)

        DeviceId = Request.QueryString("DeviceUniqueID").ToString()
        GroupName = ClsActivity.GetGroupNameByDVID(DeviceId, connActivity) 'หา GroupName จาก DeviceId เพื่อเป็นชื่อ Group SignalR

        If ClsKNSession.IsMaxONet Then GroupName = Session("selectedSession") = "PracticeFromComputer"

        If GroupName = "" Then
            GroupName = ClsActivity.GetGroupNameForSpareTablet(DeviceId, connActivity)
        End If

        If (Not Request.QueryString("ItemId") Is Nothing) And (Not Request.QueryString("Status") Is Nothing) Then
            ActivityType = EnumDashBoardType.Homework ' IsHomework
        ElseIf (Not Request.QueryString("ItemId") Is Nothing) And (Request.QueryString("Status") Is Nothing) Then
            ActivityType = EnumDashBoardType.Practice ' IsPractice
        Else
            ActivityType = EnumDashBoardType.Quiz ' // Default IsQuiz
        End If

        HttpContext.Current.Session("de") = DeviceId
    End Sub

    <Services.WebMethod()>
    Public Shared Function Gettime() As String
        If HttpContext.Current.Application("ReloadTime") IsNot Nothing Then
            HttpContext.Current.Application.Lock()
            Dim t As Dictionary(Of String, String) = HttpContext.Current.Application("ReloadTime")
            If t.ContainsKey(HttpContext.Current.Session("de")) Then
                t.Add(HttpContext.Current.Session("de").ToString() & "_2", DateTime.Now().ToString("hh:mm:ss:fff"))
            Else
                t.Add(HttpContext.Current.Session("de").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
            End If
            HttpContext.Current.Application(HttpContext.Current.Application("ReloadTime")) = t
            HttpContext.Current.Application.UnLock()
        Else
            HttpContext.Current.Application.Lock()
            Dim t As New Dictionary(Of String, String)
            t.Add(HttpContext.Current.Session("de").ToString(), DateTime.Now().ToString("hh:mm:ss:fff"))
            HttpContext.Current.Application("ReloadTime") = t
            HttpContext.Current.Application.UnLock()
        End If
        'HttpContext.Current.Application("ReloadTime_" & HttpContext.Current.Session("de").ToString()) = DateTime.Now().ToString("hh:mm:ss:fff")
        Return ""
    End Function
End Class