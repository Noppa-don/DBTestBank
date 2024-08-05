Public Class CoronaCheckStatusPhoto
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ทำการเช็คว่ารูปที่ upload เข้าไปได้รับการอนุมัติหรือไม่
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            'เลขยืนยันที่จะเอามาเทียบว่าเป็น record ไหน
            Dim SeqNo As String = Request.Form("SeqNo")
            'รหัสเครื่อง tablet
            Dim DeviceId As String = Request.Form("DeviceId")
            If Not IsNothing(methodName) Then
                'เช็คชื่อ method อีกทีต้องเป็นชือ Function นี้จริงๆ
                If methodName.ToLower() = "checkstatusphoto" Then
                    If SeqNo IsNot Nothing And SeqNo <> "" And DeviceId IsNot Nothing And DeviceId <> "" Then
                        Dim _DB As New ClassConnectSql()
                        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblStudentPhoto INNER JOIN dbo.t360_tblTabletOwner ON " & _
                                            " dbo.tblStudentPhoto.Student_Id = dbo.t360_tblTabletOwner.Owner_Id " & _
                                            " INNER JOIN dbo.t360_tblTablet ON dbo.t360_tblTabletOwner.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " & _
                                            " WHERE dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 AND dbo.t360_tblTablet.Tablet_SerialNumber = '" & _DB.CleanString(DeviceId.Trim()) & "' " & _
                                            " AND dbo.tblStudentPhoto.SP_Seq = '" & _DB.CleanString(SeqNo.Trim()) & "' "
                        Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql))
                        'หาก่อนว่า tabet นี้มีการ upload รูปเข้าไปหรือเปล่า
                        If CountCheck > 0 Then
                            'ถ้ามีก็ต้องไปหา Status ของมันว่า เป็น สถานะอะไร แล้วคืนค่ากลับไป (0 = รออนุมัติ , 1 = อนุมัติ , 2 = ไม่อนุมัติ)
                            sql = " SELECT Approval_Status FROM dbo.tblStudentPhoto INNER JOIN dbo.t360_tblTabletOwner " & _
                                  " ON dbo.tblStudentPhoto.Student_Id = dbo.t360_tblTabletOwner.Owner_Id " & _
                                  " INNER JOIN dbo.t360_tblTablet ON dbo.t360_tblTabletOwner.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " & _
                                  " AND dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 AND dbo.t360_tblTablet.Tablet_SerialNumber = '" & _DB.CleanString(DeviceId.Trim()) & "' " & _
                                  " AND dbo.tblStudentPhoto.SP_Seq = '" & _DB.CleanString(SeqNo.Trim()) & "' "
                            Dim CheckStatus As String = _DB.ExecuteScalar(sql)
                            Response.Write(CheckStatus)
                            Response.End()
                        Else
                            Response.Write("-2")
                            Response.End()
                        End If
                    Else
                        Response.Write(-1)
                        Response.End()
                    End If
                Else
                    Response.Write(-1)
                    Response.End()
                End If
            Else
                Response.Write(-1)
                Response.End()
            End If
        End If
    End Sub

End Class