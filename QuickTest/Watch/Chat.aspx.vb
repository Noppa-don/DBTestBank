Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hub
Imports Microsoft.AspNet.SignalR.Hubs
Imports KnowledgeUtils
Imports System.Web

Public Class Chat
    Inherits System.Web.UI.Page

    'Class ที่เอาจัดการข้อมูลเกี่ยวกับเรื่อง Chat มีใช้หลายจุุด
    Dim ClsChat As New ClassChat(New ClassConnectSql())
    'เก็บ ChatRoomId ใช้ฝั่ง Javascript ด้วย
    Public ChatRoomId As String
    'เก็บ ChatRoomId ใช้ฝั่ง Javascript ด้วย
    Public HeaderTxt As String
    'เก็บ UserId ผู้ปกครอง/ครู ใช้ฝั่ง Javascript ด้วย
    Public OwnerId As String
    'ตัวแปรที่บอกว่าเราได้เห็นข้อความที่ผู้ทีคุยกับเราแล้ว ถ้าเป็น True จะต้องทำการ Update tblChatRecipient ด้วยว่าเราเห็นแล้ว ใช้ฝั่ง Javascript ด้วย
    Public Flag As String
    'ตัวแปรที่เช็คว่าเป็นครูหรือเปล่า เพราะถ้าเป็นครูตอนกด Back ต้องไปคนละหน้ากับผู้ปกครอง
    Public IsProcessFromTeacher As String

    Public DeviceId As String

    ''' <summary>
    ''' ทำการ Render ชื่อผู้ที่เราสนทนาด้วย และ ข้อความเก่าๆทั้งหมด (ในกรณีที่มีข้อมูลมาก่อนหน้าแล้ว)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Id ประจำตัวของคนที่ถือเครื่อง ครู - ผู้ปกครอง
        'Session("Owner_Id") = "227FBCEB-7294-4B80-91B8-3A9AD36B1F2A"
        'ChatRoomId = "3AFC0CB4-221F-4436-A5BF-C88D91F351E3"

        If Request.QueryString("ChatRoom_Id") Is Nothing Then
            Response.Write("ไม่มี ChatroomId")
            Exit Sub
        End If

        'If HttpContext.Current.Request.QueryString("FromTeacher") Is Nothing Then
        '    HttpContext.Current.Session("Owner_Id") = "227FBCEB-7294-4B80-91B8-3A9AD36B1F2A"
        'End If

        If HttpContext.Current.Request.QueryString("FromTeacher") IsNot Nothing Then
            IsProcessFromTeacher = "True"
        Else
            IsProcessFromTeacher = "False"
        End If

        If HttpContext.Current.Session("Owner_Id") IsNot Nothing Then
            OwnerId = HttpContext.Current.Session("Owner_Id").ToString()
            DeviceId = New ClassConnectSql().ExecuteScalar("SELECT DeviceId FROM dbo.tblParent WHERE PR_Id = '" & HttpContext.Current.Session("Owner_Id").ToString & "' AND IsActive = 1;")
        Else
            Response.Write("ไม่มี Session Id ของเจ้าของเครื่อง")
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            ChatRoomId = Request.QueryString("ChatRoom_Id").ToString().ToUpper()
            HeaderTxt = ClsChat.GetHeaderTxtByChatRoomId(ChatRoomId, OwnerId)
            CreateMsgHistory()

            If IsProcessFromTeacher = "False" Then
                HttpContext.Current.Session("ChatRoom_Id") = ChatRoomId
            End If
        End If

    End Sub

    ''' <summary>
    ''' สร้าง Div ที่มีข้อความในอดีต
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateMsgHistory()

        'ตัวแปรที่บอกว่าให้ select ข้อความเป็นจำนวนเท่าไหร่
        Dim SelectTop As Integer = 50
        'Session 'FocusScreen' ที่บอกว่าจะให้หน้าจอ Focus อยู่ตำแหน่งไหน
        If HttpContext.Current.Session("FocusScreen") Is Nothing Then
            HttpContext.Current.Session("FocusScreen") = "Bottom"
        End If
        'Session 'SelecTopMsg' ที่บอกว่าจะให้ทำการ Select ข้อความกี่ความล่าสุดขึ้นมา
        If HttpContext.Current.Session("SelectTopMsg") IsNot Nothing Then
            SelectTop = HttpContext.Current.Session("SelectTopMsg")
        Else
            HttpContext.Current.Session("SelectTopMsg") = 50
        End If
        'ทำการ Get ข้อความขึ้นมา
        Dim dt As DataTable = ClsChat.GetHistoryMessage(ChatRoomId, SelectTop)
        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            'ถ้าข้อความยาวเกิน 50 ตัวอักษรจะต้องทำการตัดข้อความออก แล้ว ใส่ปุ่ม ... เพื่อให้กดแสดงรายละเอียดอีกที
            If ClsChat.CheckIsMoreThan50Message(ChatRoomId) = True Then
                'sb.Append("<div><input type='button' id='btnShowHistory' onclick='ShowMoreMessage();' value='...' /></div>")
                BtnShowMoreMsg.Style.Add("display", "inline")
                'MainContent.Style.Add("top", "100px")
                'MainContent.Style.Add("height", "315px")
            End If

            'loop เพื่อทำการสร้างข้อความที่ได้สนทนาไป , เงื่อนไขการจบ loop คือ วนสร้างจนครบทุกข้อความที่ select ขึ้นมา
            For index = dt.Rows.Count - 1 To 0 Step -1
                'ถ้าเป็นข้อความของตัวเอง
                If dt.Rows(index)("ChatFrom_Id").ToString().ToLower() = HttpContext.Current.Session("Owner_Id").ToString().ToLower() Then
                    sb.Append("<div class='MsgSetting MsgRight'>")
                    If dt.Rows(index)("ChatSeen") IsNot DBNull.Value Then
                        sb.Append("<img class='ImgEyeSeen' src='../Images/400_F_20499381_ktZBQQHYEaCwCQ38XpIhzwLO1OLavUrV_PXP.jpg' /><span class='spnRight'>")
                    Else
                        sb.Append("<img class='ImgEye' id='" & dt.Rows(index)("CM_Id").ToString() & "' src='../Images/400_F_20499381_ktZBQQHYEaCwCQ38XpIhzwLO1OLavUrV_PXP.jpg' /><span class='spnRight'>")
                    End If
                    sb.Append(dt.Rows(index)("Chat_Message").ToString())
                    sb.Append("</span><br /><span class='spnDate'>")
                    sb.Append(dt.Rows(index)("StrDate").ToString())
                    sb.Append("</span></div>")
                Else 'ถ้าเป็นข้อความของคนที่เราคุยด้วย
                    'ถ้าเรายังไม่เคยเห็นข้อความของเขาให้ Update เพื่อแสดงว่าเราเห็นแล้ว
                    If dt.Rows(index)("ChatSeen") Is DBNull.Value Then
                        ClsChat.UpdateRecipient(dt.Rows(index)("CR_Id").ToString())
                        Flag = "True"
                    End If
                    sb.Append("<div class='MsgSetting MsgLeft'><span class='spnLeft'>")
                    sb.Append(dt.Rows(index)("Chat_Message").ToString())
                    sb.Append("</span><br /><span class='spnDate'>")
                    sb.Append(dt.Rows(index)("StrDate").ToString())
                    sb.Append("</span></div>")
                End If
            Next
            MainContent.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' เมื่อคลิกดูข้อความเพิ่มเติมจากที่มีอยู่
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SetSessionWhenClickMoreMsg()
        'ทำการ +50 เข้าไปเพื่อให้ไปดึงข้อความมาเพิ่มอีก 50 ข้อความ
        If HttpContext.Current.Session("SelectTopMsg") IsNot Nothing Then
            HttpContext.Current.Session("SelectTopMsg") += 50
        Else
            HttpContext.Current.Session("SelectTopMsg") = 50
        End If

        'ให้หน้าจอ Focus อยู่ที่ด้านบนเลย
        If HttpContext.Current.Session("FocusScreen") IsNot Nothing Then
            HttpContext.Current.Session("FocusScreen") = "Top"
        End If

        Return "Complete"
    End Function

    ''' <summary>
    ''' ให้ set ค่า เป็น true เมื่อ tablet าส่งข้อความ
    ''' </summary>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Sub SetFocusScreen()
        HttpContext.Current.Session("FocusScreen") = "Bottom"
    End Sub




    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If IsProcessFromTeacher = "True" Then
            Response.Redirect("~/Watch/ChatTeacherSearchStudentParent.aspx")
        End If
        HttpContext.Current.Session("ChatRoom_Id") = Nothing
        Response.Redirect("~/Watch/ChatSelectStudent.aspx?DeviceId=" & DeviceId)
    End Sub
End Class