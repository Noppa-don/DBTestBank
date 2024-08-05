Public Class ChatTeacherSelectParent
    Inherits System.Web.UI.Page
    'ตัวแปรใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()

    ''' <summary>
    ''' ทำการหาว่ามีข้อความแจ้งเตือนของครูคนนี้บ้างรึเปล่า ถ้ามีก็จะต้องสร้าง div แจ้งเตือนแยกเป็นรายคน , ถ้าไม่มีก็ให้ข้ามไปหน้าเลือกผู้ปกครองได้เลย
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If HttpContext.Current.Session("UserId") IsNot Nothing Then
                'ต้องเอา Session นี้ไปใช้ในหน้าต่อๆไป
                HttpContext.Current.Session("Owner_Id") = HttpContext.Current.Session("UserId").ToString()
                'หาข้อความแจ้งเตือน
                Dim dt As DataTable = GetDtChatNotification(HttpContext.Current.Session("UserId").ToString())
                'ถ้ามีให้ทำการสร้าง div ผู้ปกครองที่ส่งข้อความมา
                If dt.Rows.Count > 0 Then
                    CreateDivSelectParent(dt)
                Else
                    Response.Redirect("~/Watch/ChatTeacherSearchStudentParent.aspx")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลข้อความที่คุณครูยังไม่ได้เปิดอ่าน เพื่อนำไปสร้างเป็น Div แจ้งเตือน
    ''' </summary>
    ''' <param name="TeacherId">Id ของครูคนนี้</param>
    ''' <returns>Datatable:ข้อมูลของผู้ปกครองที่ส่งข้อความมาแล้วครูยังไม่ได้อ่าน</returns>
    ''' <remarks></remarks>
    Private Function GetDtChatNotification(ByVal TeacherId As String) As DataTable
        Dim sql As String = " SELECT dbo.tblChatJoin.ChatRoom_Id,COUNT(*) AS Totalnotification,PR_FirstName " & _
                            " FROM tblChatJoin INNER JOIN tblChatMessage ON tblChatJoin.ChatRoom_Id = tblChatMessage.ChatRoom_Id INNER JOIN " & _
                            " tblChatRecipient ON tblChatMessage.CM_Id = tblChatRecipient.CM_Id INNER JOIN tblParent ON tblChatRecipient.ChatUser_Id = tblParent.PR_Id " & _
                            " WHERE (tblChatJoin.ChatUser_Id = '" & _DB.CleanString(TeacherId) & "') AND (tblChatMessage.ChatFrom_Id <> '" & _DB.CleanString(TeacherId) & "') AND " & _
                            " (tblChatRecipient.ChatSeen Is NULL) And (tblChatJoin.IsActive = 1) And (tblChatMessage.IsActive = 1) And (tblChatRecipient.IsActive = 1) " & _
                            " GROUP BY dbo.tblChatJoin.ChatRoom_Id,PR_FirstName "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการต่อสตริง HTML เพื่อสร้าง div ผู้ปกครองที่ส่งข้อความมาแล้วครูยังไม่ได้เปิดอ่าน
    ''' </summary>
    ''' <param name="dt">Datatable ข้อมูล</param>
    ''' <remarks></remarks>
    Private Sub CreateDivSelectParent(ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            'loop เพื่อสร้างข้อมูลให้ผู้ครองทีละคน , เงื่อนไขการจบ loop คือ วนจนกว่าจะครบผู้ปกครองทุกคน
            For index = 0 To dt.Rows.Count - 1
                Dim CountLoop As Integer = index + 1
                Dim ParentName As String = dt.Rows(index)("PR_FirstName").ToString()
                sb.Append("<div id='DivCover" & CountLoop & "' class='DivWidth DivCover' onclick=""ParentClick('" & dt.Rows(index)("ChatRoom_Id").ToString() & "');"">")
                sb.Append("<div id='Divpicture" & CountLoop & "' class='DivWidth DivPicture' >")
                sb.Append("<div class='ForSpnNotification'>" & dt.Rows(index)("Totalnotification").ToString() & "</div>")
                sb.Append("</div><div id='DivName" & CountLoop & "' class='DivWidth DivName' >")
                sb.Append(ParentName)
                sb.Append("</div></div>")
            Next
            DivSelectParent.InnerHtml = sb.ToString()
        End If
    End Sub

End Class