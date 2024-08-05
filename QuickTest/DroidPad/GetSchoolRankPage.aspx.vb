Public Class GetSchoolRankPage
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' หาอันดับของนักเรียนที่ได้คะแนนดีสูงสุด 16 อันดับ ของ รร.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim DeviceId As String = Request.Form("DeviceId")

                If methodName.ToLower() = "getschoolrank" Then
                    If Not IsNothing(DeviceId) Then
                        If DeviceId <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            'ทำการหาข้อมูลนักเรียนที่ได้คะแนนดีสูงสุด 16 อันดับ
                            Dim ReturnValue As String = ClsDroidPad.GetSchoolRank(DeviceId)
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

            End If
            Response.Write(-2)
            Response.End()
        End If

    End Sub

End Class