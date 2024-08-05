Public Class UserDataByLeaderStdIdPage
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ทำการหา JsonString ข้อมูลของนักเรียนที่เลือกมาจาก 16 อันดับ ที่คะแนนสูงสุด
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                If Request.Form("DeviceId") IsNot Nothing And Request.Form("StudentId") IsNot Nothing Then
                    If methodName.ToLower() = "userdatabyleaderstdid" Then
                        Dim DeviceId As String = Request.Form("DeviceId")
                        Dim StudentId As String = Request.Form("StudentId")
                        If Not IsNothing(StudentId) Then
                            If StudentId <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = ClsDroidPad.GetJsonDataByStudentId(StudentId)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    End If
                Else
                    Response.Write(-1)
                    Response.End()
                End If
            End If
            Response.Write(-2)
            Response.End()
        End If
    End Sub

End Class