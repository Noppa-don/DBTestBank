Imports System.Web
Public Class ChatTeacherSearchStudentParent
    Inherits System.Web.UI.Page
    'ตัวแปรใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'สตริง HTML ที่จะนำไป Gen Panel เลือกห้องทางด้านซ้ายมือ
    Public htmlClass As String

    ''' <summary>
    ''' ทำการสร้าง Panel เลือกห้องทางซ้ายมือ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindClassMenu()
        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้าง Panel เลือกห้องด้านซ้ายมือ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindClassMenu()
        'Dim htmlClass As New StringBuilder()
        Dim dt As DataTable = GetDtClassForSideMunu(HttpContext.Current.Session("SchoolCode").ToString)
        If Not dt.Rows.Count = 0 Then
            Dim start As Integer = dt.Rows(0)("ClassOrder")
            Dim startClass As String = dt.Rows(0)("ClassName").ToString().Substring(0, 3)
            htmlClass &= "<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>"
            'loop เพื่อสร้าง ห้อง , เงื่อนไขการจบ loop คือ วนจนครบทุกห้อง
            For Each row In dt.Rows
                If Not row("ClassOrder") = start Then
                    htmlClass &= "</div>"
                    start = row("ClassOrder")
                    startClass = row("ClassName").ToString().Substring(0, 3)
                    htmlClass &= "<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>"
                End If
                htmlClass &= "<div class='menuAcdItem'>" & row("ClassName") & "</div>"
            Next
            htmlClass &= "</div>"
            'a.InnerHtml = htmlClass.ToString()
        End If
    End Sub

    ''' <summary>
    ''' ทำการหาชั้นห้อง ของรร. นี้เพื่อนำไปสร้าง Panel เลือกห้องทางด้านซ้ายมือ
    ''' </summary>
    ''' <param name="SchoolId">Id ของ รร.นี้</param>
    ''' <returns>Datatable:ชั้น/ห้อง ที่อยู่ใน รร. นี้</returns>
    ''' <remarks></remarks>
    Private Function GetDtClassForSideMunu(ByVal SchoolId As String) As DataTable
        Dim sql As String = " SELECT DISTINCT dbo.t360_tblStudent.Student_CurrentClass + dbo.t360_tblStudent.Student_CurrentRoom AS ClassName,dbo.t360_tblClass.Class_Order as ClassOrder " & _
                            " FROM dbo.tblStudentPhoto INNER JOIN dbo.t360_tblStudent ON dbo.tblStudentPhoto.Student_Id = dbo.t360_tblStudent.Student_Id " & _
                            " INNER JOIN dbo.t360_tblRoom ON t360_tblStudent.Student_CurrentClass = dbo.t360_tblRoom.Class_Name " & _
                            " AND t360_tblstudent.Student_CurrentRoom = dbo.t360_tblRoom.Room_Name INNER JOIN dbo.t360_tblClass ON dbo.t360_tblRoom.Class_Name = dbo.t360_tblClass.Class_Name " & _
                            " WHERE IsFromParentTablet = 1 And Approval_Status = 1 AND dbo.t360_tblStudent.School_Code = '" & SchoolId & "' ORDER BY dbo.t360_tblClass.Class_Order "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการหาข้อมูลผู้ปกครอง หลังจากที่เลือกห้องมาแล้ว
    ''' </summary>
    ''' <param name="ClassRoom">ห้องที่เลือก</param>
    ''' <returns>String:HTML div ผู้ปกครองของนักเรียนที่อยู่ในห้องที่เลือกมา</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GetChatStudentParent(ByVal ClassRoom As String) As String
        Dim strReturn As String = ""
        Dim _DB As New ClassConnectSql()
        Dim SplitStr = ClassRoom.Split("/")
        Dim ClassName As String = SplitStr(0).Trim()
        Dim RoomName As String = "/" & SplitStr(1).Trim()
        Dim sql As String = " SELECT tblParent.PR_FirstName,tblParent.PR_Id,Student_CurrentClass + Student_CurrentRoom + ' ' + t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName + ' เลขที่ ' " & _
                            " + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(10))  AS StudentDetail FROM t360_tblStudent INNER JOIN " & _
                            " tblStudentParent ON t360_tblStudent.Student_Id = tblStudentParent.Student_Id INNER JOIN tblParent ON tblStudentParent.PR_Id = tblParent.PR_Id INNER JOIN " & _
                            " tblStudentPhoto ON t360_tblStudent.Student_Id = tblStudentPhoto.Student_Id WHERE (t360_tblStudent.Student_CurrentClass = '" & ClassName & "') " & _
                            " AND (t360_tblStudent.Student_CurrentRoom = '" & RoomName & "') AND (t360_tblStudent.School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "') " & _
                            " AND (tblStudentPhoto.IsFromParentTablet = 1) AND (tblStudentPhoto.Approval_Status = 1)  ORDER BY t360_tblStudent.Student_CurrentNoInRoom; "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            'loop เพื่อทำการต่อสตริง HTML เป็น Div ผู้ปกครอง , เงื่อนไขการจบ loop คือ วนจนครบทุกคน
            For index = 0 To dt.Rows.Count - 1
                strReturn &= "<div id='DivCover" & index + 1 & "' class='DivWidth DivCover' prid='" & dt.Rows(index)("PR_Id").ToString() & "'><div id='Divpicture" & index + 1 & "' class='DivWidth DivPicture' ></div><div id='DivName" & index + 1 & "' class='DivWidth DivName' ><div>" & dt.Rows(index)("PR_FirstName") & "</div><div class='ForDivDetailStudent'>" & dt.Rows(index)("StudentDetail") & "</div></div></div>"
            Next
        End If
        Return strReturn
    End Function


    ''' <summary>
    ''' ทำการหาผู้ปกครองที่เคยคุยกับครูคนนี้ ตามข้อความที่กรอกเข้ามา
    ''' </summary>
    ''' <param name="txtSearch">คำที่ค้นหา</param>
    ''' <returns>String:สตริง HTML ที่เป็น Div ผู้ปกครองในกรณีที่ค้นหาแล้วเจอ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GetSearchChatStudentParent(ByVal txtSearch As String) As String
        Dim strReturn As String = ""
        Dim _DB As New ClassConnectSql()
        txtSearch = _DB.CleanString(txtSearch.Trim())
        Dim sql As String = " SELECT tblParent.PR_FirstName,tblParent.PR_Id,Student_CurrentClass + Student_CurrentRoom + ' ' + t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName + ' เลขที่ ' " & _
                            " + CAST(t360_tblStudent.Student_CurrentNoInRoom AS VARCHAR(10))  AS StudentDetail FROM t360_tblStudent INNER JOIN  tblStudentParent " & _
                            " ON t360_tblStudent.Student_Id = tblStudentParent.Student_Id INNER JOIN tblParent ON tblStudentParent.PR_Id = tblParent.PR_Id INNER JOIN  tblStudentPhoto " & _
                            " ON t360_tblStudent.Student_Id = tblStudentPhoto.Student_Id WHERE (t360_tblStudent.Student_CurrentClass like '%" & txtSearch & "%' or t360_tblStudent.Student_CurrentRoom LIKE '%" & txtSearch & "%' " & _
                            " OR Student_FirstName LIKE '%" & txtSearch & "%' OR Student_LastName LIKE '%" & txtSearch & "%' OR dbo.tblParent.PR_FirstName LIKE '%" & txtSearch & "%' OR Student_NickName LIKE '%" & txtSearch & "%') " & _
                            " AND (t360_tblStudent.School_Code = '" & HttpContext.Current.Session("SchoolCode").ToString() & "')  AND (tblStudentPhoto.IsFromParentTablet = 1) " & _
                            " AND (tblStudentPhoto.Approval_Status = 1)  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            'loop เพื่อทำการต่อสตริงให้เป็น Div ผู้ปกครอง , เงื่อนไขการจบ loop คือ วนจนครบหมดทุกคนที่ข้อมูลตรงกับคำที่ค้นหาเข้ามา
            For index = 0 To dt.Rows.Count - 1
                strReturn &= "<div id='DivCover" & index + 1 & "' class='DivWidth DivCover' prid='" & dt.Rows(index)("PR_Id").ToString() & "'><div id='Divpicture" & index + 1 & "' class='DivWidth DivPicture' ></div><div id='DivName" & index + 1 & "' class='DivWidth DivName' ><div>" & dt.Rows(index)("PR_FirstName") & "</div><div>" & dt.Rows(index)("StudentDetail") & "</div></div></div>"
            Next
        End If
        Return strReturn
    End Function

    ''' <summary>
    ''' ทำการสร้างห้อง Chat ใหม่ในกรณีที่ยังไม่มี ChatRoom มาก่อน
    ''' </summary>
    ''' <param name="PR_ID">Id ของผู้ปกครองที่ครูต้องการคุยด้วย</param>
    ''' <returns>String:ChatRoomId=สำเร็จ,""=ไม่สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GotoChatRoom(ByVal PR_ID As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT dbo.tblChatJoin.ChatRoom_Id FROM (SELECT ChatRoom_Id FROM dbo.tblChatJoin WHERE ChatUser_Id = '" & HttpContext.Current.Session("UserId").ToString() & "')tblxxx " & _
                            " INNER JOIN dbo.tblChatJoin ON tblxxx.ChatRoom_Id = dbo.tblChatJoin.ChatRoom_Id WHERE dbo.tblChatJoin.ChatUser_Id = '" & PR_ID & "' "
        Dim ChatRoomId As String = _DB.ExecuteScalar(sql)
        'หาก่อนว่าครูเคยคุยกับ ผู้ปกครองคนนี้แล้วหรือยัง ถ้าเคยแล้วให้หา ChatroomId อันเก่าแล้วคืนไปเลย
        If ChatRoomId = "" Then
            Try
                Dim NewChatRoomId As String = Guid.NewGuid().ToString()
                _DB.OpenWithTransection()
                'Insert tblChatRoom
                sql = " INSERT INTO dbo.tblChatRoom( ChatRoom_Id, LastUpdate, IsActive ,School_Code )VALUES  ( '" & NewChatRoomId & "',dbo.GetThaiDate(),1,'" & HttpContext.Current.Session("SchoolCode").ToString() & "');  "
                _DB.ExecuteWithTransection(sql)

                'Insert tblChatJoin ครู
                sql = " INSERT INTO dbo.tblChatJoin( CJ_Id ,ChatRoom_Id ,ChatUser_Id ,ChatUserType ,LastUpdate ,IsActive,School_Code) " &
                      " VALUES  ( NEWID() , '" & NewChatRoomId & "' ,'" & HttpContext.Current.Session("UserId").ToString() & "' ,0 , dbo.GetThaiDate() ,1,'" & HttpContext.Current.Session("SchoolCode").ToString() & "') "
                _DB.ExecuteWithTransection(sql)

                'Insert tblChatJoin ผู้ปกครอง
                sql = " INSERT INTO dbo.tblChatJoin( CJ_Id ,ChatRoom_Id ,ChatUser_Id ,ChatUserType ,LastUpdate ,IsActive,School_Code) " &
                      " VALUES  ( NEWID() , '" & NewChatRoomId & "' ,'" & PR_ID & "' ,0 , dbo.GetThaiDate() ,1,'" & HttpContext.Current.Session("SchoolCode").ToString() & "') "
                _DB.ExecuteWithTransection(sql)

                _DB.CommitTransection()
                HttpContext.Current.Session("Owner_Id") = HttpContext.Current.Session("UserId").ToString()
                Return NewChatRoomId
            Catch ex As Exception
                _DB.RollbackTransection()
                Return ""
            End Try
        Else
            HttpContext.Current.Session("Owner_Id") = HttpContext.Current.Session("UserId").ToString()
            Return ChatRoomId
        End If
    End Function
End Class