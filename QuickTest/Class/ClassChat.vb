Public Class ClassChat

    'ตัวแปรที่ใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As ClsConnect
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    ''' <summary>
    ''' Function Get ข้อความที่คุยกัน default 50 ข้อความแรก
    ''' </summary>
    ''' <param name="ChatRoomId">Id ของห้อง chat</param>
    ''' <param name="Quantity">จำนวนข้อความที่ต้องการต้นหา Default ที่ 50 ข้อความ ล่าสุด</param>
    ''' <returns>Datatable:ข้อควมที่สนทนาล่าสุด xx อันดับ</returns>
    ''' <remarks></remarks>
    Public Function GetHistoryMessage(ByVal ChatRoomId As String, Optional ByVal Quantity As Integer = 50)
        Dim sql As String = " SELECT TOP " & Quantity & " tblChatMessage.Chat_Message, CASE WHEN DATEDIFF(DAY, dbo.tblChatMessage.LastUpdate, dbo.GetThaiDate()) = 0 THEN CONVERT(VARCHAR(5), dbo.tblChatMessage.LastUpdate, 8) " &
                            " ELSE CONVERT(VARCHAR(5), dbo.tblChatMessage.LastUpdate, 3) + ' ' + CONVERT(VARCHAR(5), dbo.tblChatMessage.LastUpdate, 8) END AS StrDate, tblChatMessage.ChatFrom_Id, " &
                            " tblChatRecipient.ChatSeen,dbo.tblChatMessage.CM_Id,dbo.tblChatRecipient.CR_Id FROM tblChatMessage INNER JOIN tblChatRecipient ON tblChatMessage.CM_Id = tblChatRecipient.CM_Id " &
                            " WHERE  (tblChatMessage.ChatRoom_Id = '" & ChatRoomId & "') ORDER BY tblChatMessage.LastUpdate DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' Function Check ว่า ChatRoom นี้มีข้อความมากกว่า 50 ข้อความหรือเปล่า
    ''' </summary>
    ''' <param name="ChatRoomId">Id ของห้อง Chat</param>
    ''' <returns>Boolean:True=มากวว่า 50 ข้อความ,False=น้อยกว่า 50 ข้อความ</returns>
    ''' <remarks></remarks>
    Public Function CheckIsMoreThan50Message(ByVal ChatRoomId As String) As Boolean
        Dim sql As String = " SELECT COUNT(*) FROM  dbo.tblChatMessage WHERE ChatRoom_Id = '" & ChatRoomId & "' AND IsActive = 1 "
        Dim CheckCount As String = _DB.ExecuteScalar(sql)
        If CType(CheckCount, Integer) > 50 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Function Insert ข้อมูลเมื่อกดปุ่ม ส่ง ในหน้า Chat
    ''' </summary>
    ''' <param name="ChatRoomId">Id ของห้อง Chat</param>
    ''' <param name="UserId">Id ของผู้ส่งข้อความ</param>
    ''' <param name="Msg">ข้อความ</param>
    ''' <returns>String:""=ไม่สำเร็จ,รหัสข้อความ + "|" + วันที่เวลาที่ส่ง + "|" + Id ของผู้รับ =สำเร็จ</returns>
    ''' <remarks></remarks>
    Public Function SaveMessage(ByVal ChatRoomId As String, ByVal UserId As String, ByVal Msg As String) As String
        Dim StringReturn = ""
        Try
            Dim sql As String = " SELECT NEWID() "
            'Id ของข้อความต้องเก็บเอาไว้ก่อนเพราะเดียวจะเอาไปต่อสตริงตอนสุดท้าย เพื่อ Return ค่ากลับไป
            Dim CMId As String = _DB.ExecuteScalar(sql)
            sql = " SELECT School_Code FROM dbo.tblChatRoom WHERE ChatRoom_Id = '" & ChatRoomId & "' AND IsActive = 1 "
            Dim SchoolCode As String = _DB.ExecuteScalar(sql)
            If SchoolCode <> "" Then
                sql = " INSERT INTO dbo.tblChatMessage ( CM_Id ,ChatRoom_Id ,ChatFrom_Id , Chat_Message ,LastUpdate ,IsActive,School_Code) " &
                  " VALUES  ( '" & CMId & "' ,'" & _DB.CleanString(ChatRoomId) & "' , '" & UserId & "' , '" & _DB.CleanString(Msg) & "' , dbo.GetThaiDate() ,1 ,'" & SchoolCode & "') "
                _DB.Execute(sql)

                sql = " SELECT NEWID() "
                'Id ของผู้รับต้องเก็บเอาไว้ก่อนเพราะเดียวจะเอาไปต่อสตริงตอนสุดท้าย เพื่อ Return ค่ากลับไป
                Dim CRID As String = _DB.ExecuteScalar(sql)
                sql = " INSERT INTO dbo.tblChatRecipient ( CR_Id ,ChatUser_Id ,CM_Id , ChatSeen ,LastUpdate ,IsActive,School_Code) " &
                      " VALUES  ( '" & CRID & "' , '" & UserId & "' , '" & CMId & "' , NULL ,dbo.GetThaiDate() ,1,'" & SchoolCode & "') "
                _DB.Execute(sql)

                sql = " SELECT CONVERT(VARCHAR(5),LastUpdate,8)  FROM dbo.tblChatMessage  WHERE CM_Id = '" & CMId & "' "
                'วันที่เวลาที่ส่ง ต้องเก็บเอาไว้ก่อนเพราะเดียวจะเอาไปต่อสตริงตอนสุดท้าย เพื่อ Return ค่ากลับไป
                Dim StrDateTime As String = _DB.ExecuteScalar(sql)
                'ต่อสตริงเพื่อ Return ค่ากลับไป
                StringReturn = CMId & "|" & StrDateTime & "|" & CRID
                Return StringReturn
            Else
                Return ""
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return StringReturn
        End Try
    End Function

    ''' <summary>
    ''' Function หาชื่อครูมาเป็น Header ให้โปรแกรม Chat
    ''' </summary>
    ''' <param name="ChatRoomId">Id ของห้อง Chat</param>
    ''' <param name="UserId">Id ของผู้ปกครอง</param>
    ''' <returns>String:ชื่อครู</returns>
    ''' <remarks></remarks>
    Public Function GetHeaderTxtByChatRoomId(ByVal ChatRoomId As String, ByVal UserId As String) As String
        Dim sql As String = " SELECT t360_tblTeacher.Teacher_FirstName, tblParent.PR_FirstName FROM tblChatJoin LEFT OUTER JOIN " &
                            " tblParent ON tblChatJoin.ChatUser_Id = tblParent.PR_Id LEFT OUTER JOIN " &
                            " t360_tblTeacher ON tblChatJoin.ChatUser_Id = t360_tblTeacher.Teacher_id " &
                            " WHERE (tblChatJoin.ChatRoom_Id = '" & ChatRoomId & "') " &
                            " AND (dbo.tblChatJoin.ChatUser_Id <> '" & UserId & "') "
        Dim dt As New DataTable
        Dim HeaderTxt As String = ""
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Teacher_FirstName") IsNot DBNull.Value Then
                HeaderTxt = dt.Rows(0)("Teacher_FirstName").ToString()
            ElseIf dt.Rows(0)("PR_FirstName") IsNot DBNull.Value Then
                HeaderTxt = dt.Rows(0)("PR_FirstName").ToString()
            End If
        End If
        Return HeaderTxt
    End Function

    ''' <summary>
    ''' Function Update Recipient ว่าได้เห็นข้อความแล้ว
    ''' </summary>
    ''' <param name="CRId">Id ของผู้รับข้อความ</param>
    ''' <returns>Boolean:True=สำเร็จ,False=ไม่สำเร็จ</returns>
    ''' <remarks></remarks>
    Public Function UpdateRecipient(ByVal CRId As String) As Boolean
        Try
            Dim sql As String = " UPDATE dbo.tblChatRecipient SET ChatSeen = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(), ClientId = Null WHERE CR_Id = '" & CRId & "' "
            _DB.Execute(sql)
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Function Check ว่า ครุ - ผู้ปกครองคู่นี้เคยมี ChatRoomId หรือยัง
    ''' </summary>
    ''' <param name="ParentId">Id ของผู้ปกครองคนนี้</param>
    ''' <param name="TeacherId">Id ของครูที่เลือก</param>
    ''' <returns>String:ChatRoomId</returns>
    ''' <remarks></remarks>
    Public Function GetChatRoomId(ByVal ParentId As String, ByVal TeacherId As String) As String

        Dim sql As String = " SELECT DISTINCT tblChatJoin.ChatRoom_Id FROM tblChatJoin INNER JOIN " &
                            " tblChatJoin AS tblChatJoin_1 ON tblChatJoin.ChatRoom_Id = tblChatJoin_1.ChatRoom_Id " &
                            " WHERE (tblChatJoin.ChatUser_Id = '" & ParentId & "') " &
                            " AND (tblChatJoin_1.ChatUser_Id = '" & TeacherId & "') "
        Dim ChatRoomId As String = _DB.ExecuteScalar(sql)
        Return ChatRoomId

    End Function

    ''' <summary>
    ''' Function สร้างห้อง Chat
    ''' </summary>
    ''' <param name="ParentId">Id ของผู้ปกครอง</param>
    ''' <param name="TeacherId">Id ของครู</param>
    ''' <returns>String:ChatRoomId=สำเร็จ,""=ไม่สำเร็จ</returns>
    ''' <remarks></remarks>
    Public Function CreateChatRoom(ByVal ParentId As String, ByVal TeacherId As String)
        _DB.OpenWithTransection()
        Try
            Dim sql As String = " SELECT NEWID() "
            Dim ChatRoomId As String = _DB.ExecuteScalarWithTransection(sql)
            sql = " SELECT School_Code FROM dbo.t360_tblTeacher WHERE Teacher_id = '" & TeacherId & "' AND Teacher_IsActive = 1 "
            Dim SchoolCode As String = _DB.ExecuteScalarWithTransection(sql)
            'ต้องหารหัสโรงเรียนก่อน (มาเพิ่มตอนหลังเพราะโปรแกรม Sync พี่เต้อต้องใช้ SchoolCode)
            If SchoolCode.ToString() <> "" Then
                sql = " INSERT INTO dbo.tblChatRoom( ChatRoom_Id, LastUpdate, IsActive ,School_Code) " &
                      " VALUES  ( '" & ChatRoomId & "',dbo.GetThaiDate(), 1,'" & SchoolCode & "') "
                _DB.ExecuteWithTransection(sql)
                'loop เพื่อ Insert ข้อมูลคู่สนทนา ทั้งครู และ ผู้ปกครอง , เงื่อนไขการจบ loop คือ วน 2 รอบ เพื่อ Insert ทั้งครุ และ ผู้ปกครอง
                For index = 1 To 2
                    If index = 1 Then
                        sql = " INSERT INTO dbo.tblChatJoin ( CJ_Id ,ChatRoom_Id ,ChatUser_Id ,ChatUserType ,LastUpdate ,IsActive,School_Code) " &
                              " VALUES  ( NEWID(),'" & ChatRoomId & "' ,'" & ParentId & "' , 0 ,dbo.GetThaiDate(),1,'" & SchoolCode & "' ) "
                    Else
                        sql = " INSERT INTO dbo.tblChatJoin ( CJ_Id ,ChatRoom_Id ,ChatUser_Id ,ChatUserType ,LastUpdate ,IsActive,School_Code) " &
                              " VALUES  ( NEWID(),'" & ChatRoomId & "' ,'" & TeacherId & "' , 0 ,dbo.GetThaiDate(),1 ,'" & SchoolCode & "') "
                    End If
                    _DB.ExecuteWithTransection(sql)
                Next
                _DB.CommitTransection()
                Return ChatRoomId
            Else
                _DB.RollbackTransection()
                Return ""
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return ""
        End Try
    End Function




End Class
