Public Class Step1_PadTeacher
    Inherits System.Web.UI.Page
    Public GroupName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238" 'player_Id สำหรับเทส
        'Session("SchoolCode") = "1000001" 'schoolcode สำหรับเทส

        'If Session("UserId") Is Nothing Then
        '    If Request.QueryString("DeviceId") IsNot Nothing Then
        '        Dim clsDroidPad As New ClassDroidPad(New ClassConnectSql)
        '        GroupName = clsDroidPad.GetUserIdByDeviceId(Request.QueryString("DeviceId"))
        '        If GroupName = "" Then
        '            Response.Write("ไม่สามารถหา UserId จาก DeviceId นี้ได้")
        '        End If
        '    Else
        '        Response.Write("ไม่มี QueryString และ Session UserId")
        '    End If
        'Else
        '    GroupName = Session("UserId").ToString()
        'End If

        GroupName = Session("selectedSession").ToString()

        Dim objTestSet As ClsTestSet
        If IsNothing(Session("objTestSet")) Then
            objTestSet = New ClsTestSet(Session("UserId"))
            Session("objTestSet") = objTestSet
        Else
            objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
        End If

        Listing.DataSource = objTestSet.GetAllTestSet()
        Listing.DataBind()

        ' บันทึกค่าใน application array เมื่อเปลี่ยนหน้า
        'Session("newTestSetId") = ""
        'Dim ClsSelectSess As New ClsSelectSession
        'ClsSelectSess.resetValueInSession()
        'ClsSelectSess.setApplicationWhenChangeCurrentPage(Session("newTestSetId").ToString(), objTestSet)
    End Sub

End Class