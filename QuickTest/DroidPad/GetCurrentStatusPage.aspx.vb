Public Class GetCurrentStatusPage
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ทำการเช็คข้อมูลเหรียญใน tmp และ update ข้อมูลจริง
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim ReturnValue As String = ""
                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                Dim tmp_Id As String = Request.Form("tmp_Id")
                Dim StrJsonData As String = Request.Form("JsonData")
                Dim DeviceId As String = ""
                If tmp_Id IsNot Nothing And tmp_Id <> "" Then
                    DeviceId = Request.Form("DeviceId")
                    If DeviceId <> "" And DeviceId IsNot Nothing Then
                        'เช็ค tmpid ก่อน ว่ามีอยู่ใน db จริงหรือเปล่า
                        Dim tmpIdIsComplete = ClsDroidPad.ChecktmpIdIsComplete(tmp_Id)
                        'ถ้า tmpId ปรับเป็น 1 แล้วคืน OK กลับไปเลย
                        If tmpIdIsComplete = True Then
                            ReturnValue = "OK"
                            ClsLog.Record("Return From No Update TempId = " & ReturnValue)
                            Response.Write(ReturnValue)
                            Response.End()
                        Else
                            'ถ้า tmpId ยังไม่ได้ปรับเป็น 1 ต้องมาทำการ update ข้อมูลจริงลง tblStudentPoint
                            ReturnValue = ClsDroidPad.UpdatetmpToRealDb(DeviceId, tmp_Id)
                            ClsLog.Record("Return From Update TempId = " & ReturnValue)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                Else
                    'ทำการส่ง JsonString ข้อมูลเหรียญต่างๆมาเพื่อ update ลง DB
                    If StrJsonData IsNot Nothing And StrJsonData <> "" Then
                        ClsLog.Record("JsonData จาก App = " & StrJsonData)
                        Dim ObjCurrentStatus
                        Try
                            'ทำการแปลง JsonString ให้กลายเป็น Object ต่างๆ
                            ObjCurrentStatus = deserialize(Of ListCurrentStatus)(StrJsonData)
                        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                            Response.Write(-1)
                            Response.End()
                        End Try
                        'ดึงรหัสเครื่อง Tablet มาด้วย
                        DeviceId = ObjCurrentStatus.user_details.deviceId.ToString()
                        'object ที่เก็บข้อมูลเกี่ยวกับรูปภาพเครื่องแต่งตัวของตัวละคนของนักเรียน
                        Dim objUserAvatar = ObjCurrentStatus.user_details.user_avatar
                        If DeviceId IsNot Nothing Then
                            'ทำการสร้าง Hashtable ของ Item เครื่องแต่งตัวของนักเรียน
                            Dim HashItem As Hashtable = CreateHashtableUserAvatar(objUserAvatar)
                            'จำนวนเหรียญทองทั้งหมด
                            Dim ResultGold As Integer = CInt(ObjCurrentStatus.user_details.user_gold_coin_spent)
                            'จำนวนเหรียญเงินทั้งหมด
                            Dim ResultSilver As Integer = CInt(ObjCurrentStatus.user_details.user_silver_coin_spent)
                            'จำนวนเพชรทั้งหมด
                            Dim ResultDiamond As Integer = CInt(ObjCurrentStatus.user_details.user_diamond_spent)
                            'จำนวนเพชรที่ได้เพิ่มมา
                            Dim ResultRecieveDiamond As Integer = CInt(ObjCurrentStatus.user_details.user_diamond_recieve)
                            ClsLog.Record("ResultGold = " & ResultGold & ", ResultSilver = " & ResultSilver & ", ResultDiamond = " & ResultDiamond)
                            If methodName.ToLower() = "getcurrentstatus" Then
                                If Not IsNothing(DeviceId) Then
                                    'ทำการส่งค่าไป update ข้อมูลที่ server
                                    ReturnValue = ClsDroidPad.GetCurrentStatus(DeviceId, HashItem, ResultGold, ResultSilver, ResultDiamond, ResultRecieveDiamond, StrJsonData)
                                    ClsLog.Record("JsonData Return From Server = " & ReturnValue)
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
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                        Response.Write(-1)
                        Response.End()
                    Else
                        Response.Write(-1)
                        Response.End()
                    End If
                End If
            End If
        Else
            Response.Write(-1)
            Response.End()
        End If

    End Sub

    ''' <summary>
    ''' ทำการเปลี่ยน JsonString ให้กลายมาเป็น Object ต่างๆ
    ''' </summary>
    ''' <typeparam name="T">List ที่ต้องการแปลง</typeparam>
    ''' <param name="jsonStr">ข้อมูล JsonString</param>
    ''' <returns>T-Object</returns>
    ''' <remarks></remarks>
    Private Function deserialize(Of T)(ByVal jsonStr As String) As T
        Try
            Dim s = New System.Web.Script.Serialization.JavaScriptSerializer()
            Return s.Deserialize(Of T)(jsonStr)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Throw New Exception("-2")
        End Try
    End Function

    ''' <summary>
    ''' ทำการนำข้อมูลจาก object เครื่องแต่งตัวของนักเรียนมา add ใส่ใน Hashtable
    ''' </summary>
    ''' <param name="UserAvatarObj">object เครื่องแต่งตัวของตัวละคนของนักเรียน</param>
    ''' <returns>Hashtable:ItemKey,Index</returns>
    ''' <remarks></remarks>
    Private Function CreateHashtableUserAvatar(ByVal UserAvatarObj As Object) As Hashtable

        Dim HashItem As New Hashtable

        If UserAvatarObj.character.startSpriteId <> "" And UserAvatarObj.character.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.character.startSpriteId, 0)
        End If

        If UserAvatarObj.shirt.startSpriteId <> "" And UserAvatarObj.shirt.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.shirt.startSpriteId, 0)
        End If

        If UserAvatarObj.pants.startSpriteId <> "" And UserAvatarObj.pants.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.pants.startSpriteId, 0)
        End If

        If UserAvatarObj.shoe.startSpriteId <> "" And UserAvatarObj.shoe.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.shoe.startSpriteId, 0)
        End If

        If UserAvatarObj.head.startSpriteId <> "" And UserAvatarObj.head.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.head.startSpriteId, 0)
        End If

        If UserAvatarObj.scarf.startSpriteId <> "" And UserAvatarObj.scarf.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.scarf.startSpriteId, 0)
        End If

        If UserAvatarObj.armband.startSpriteId <> "" And UserAvatarObj.armband.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.armband.startSpriteId, 0)
        End If

        If UserAvatarObj.watch.startSpriteId <> "" And UserAvatarObj.watch.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.watch.startSpriteId, 0)
        End If

        If UserAvatarObj.mobilephone.startSpriteId <> "" And UserAvatarObj.mobilephone.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.mobilephone.startSpriteId, 0)
        End If

        If UserAvatarObj.glasses.startSpriteId <> "" And UserAvatarObj.glasses.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.glasses.startSpriteId, 0)
        End If

        If UserAvatarObj.hat.startSpriteId <> "" And UserAvatarObj.hat.startSpriteId IsNot Nothing Then
            HashItem.Add(UserAvatarObj.hat.startSpriteId, 0)
        End If

        If UserAvatarObj.room.id <> "" And UserAvatarObj.room.id IsNot Nothing Then
            HashItem.Add(UserAvatarObj.room.id, 0)
        End If

        Dim objPet = UserAvatarObj.pets
        Dim indexPet As Integer = 1
        'loop เพราะว่านักเรียนมีสัตว์เลี้ยงได้หลายตัว ต้องหาตำแหน่งของสัตว์เลี้ยงแต่ละตัวว่าอยู่ตำแหน่งไหน , เงื่อนไขการจบ loop คือ วนจนครบทุกสัตว์เลี้ยง
        For Each r In objPet
            If r.startSpriteId <> "" And r.startSpriteId IsNot Nothing Then
                If HashItem.ContainsKey(r.startSpriteId) = False Then
                    HashItem.Add(r.startSpriteId, indexPet)
                    indexPet += 1
                End If
            End If
        Next

        Return HashItem

    End Function

End Class

#Region "Class รับ json อันใหม่ที่ส่งข้อมูลมาทั้งก้อน"
'Class ที่เอาไว้รับข้อมูล JsonString ที่เกี่ยวกับ เหรียญ ของนักเรียน และ Item ต่างๆเพื่อเอาไปทำเป็นรูปภาพ
Public Class ListCurrentStatus
    Private _user_details As user_details
    Public Property user_details As user_details
        Get
            user_details = _user_details
        End Get
        Set(ByVal value As user_details)
            _user_details = value
        End Set
    End Property
End Class

<Serializable()>
Public Class user_details

    Private _deviceId As String
    Public Property deviceId As String
        Get
            deviceId = _deviceId
        End Get
        Set(ByVal value As String)
            _deviceId = value
        End Set
    End Property

    Private _user_silver_coin_spent As Integer
    Public Property user_silver_coin_spent As Integer
        Get
            user_silver_coin_spent = _user_silver_coin_spent
        End Get
        Set(ByVal value As Integer)
            _user_silver_coin_spent = value
        End Set
    End Property

    Private _user_gold_coin_spent As Integer
    Public Property user_gold_coin_spent As Integer
        Get
            user_gold_coin_spent = _user_gold_coin_spent
        End Get
        Set(ByVal value As Integer)
            _user_gold_coin_spent = value
        End Set
    End Property

    Private _user_diamond_spent As Integer
    Public Property user_diamond_spent As Integer
        Get
            user_diamond_spent = _user_diamond_spent
        End Get
        Set(ByVal value As Integer)
            _user_diamond_spent = value
        End Set
    End Property

    Private _user_diamond_recieve As Integer
    Public Property user_diamond_recieve As Integer
        Get
            user_diamond_recieve = _user_diamond_recieve
        End Get
        Set(ByVal value As Integer)
            _user_diamond_recieve = value
        End Set
    End Property

    Private _itemSequenceKeys As ArrayList
    Public Property itemSequenceKeys As ArrayList
        Get
            itemSequenceKeys = _itemSequenceKeys
        End Get
        Set(ByVal value As ArrayList)
            _itemSequenceKeys = value
        End Set
    End Property

    Private _user_avatar As ListOfUseravatar
    Public Property user_avatar As ListOfUseravatar
        Get
            user_avatar = _user_avatar
        End Get
        Set(ByVal value As ListOfUseravatar)
            _user_avatar = value
        End Set
    End Property

End Class

<Serializable()>
Public Class ListOfUseravatar

    Private _character As ListOfCharacter
    Public Property character As ListOfCharacter
        Get
            character = _character
        End Get
        Set(ByVal value As ListOfCharacter)
            _character = value
        End Set
    End Property

    Private _shirt As ListOfShirt
    Public Property shirt As ListOfShirt
        Get
            shirt = _shirt
        End Get
        Set(ByVal value As ListOfShirt)
            _shirt = value
        End Set
    End Property

    Private _pants As ListOfPants
    Public Property pants As ListOfPants
        Get
            pants = _pants
        End Get
        Set(ByVal value As ListOfPants)
            _pants = value
        End Set
    End Property

    Private _shoe As ListOfShoe
    Public Property shoe As ListOfShoe
        Get
            shoe = _shoe
        End Get
        Set(ByVal value As ListOfShoe)
            _shoe = value
        End Set
    End Property

    Private _head As ListOfHead
    Public Property head As ListOfHead
        Get
            head = _head
        End Get
        Set(ByVal value As ListOfHead)
            _head = value
        End Set
    End Property

    Private _scarf As ListOfScarf
    Public Property scarf As ListOfScarf
        Get
            scarf = _scarf
        End Get
        Set(ByVal value As ListOfScarf)
            _scarf = value
        End Set
    End Property

    Private _armband As ListOfArmband
    Public Property armband As ListOfArmband
        Get
            armband = _armband
        End Get
        Set(ByVal value As ListOfArmband)
            _armband = value
        End Set
    End Property

    Private _watch As ListOfWatch
    Public Property watch As ListOfWatch
        Get
            watch = _watch
        End Get
        Set(ByVal value As ListOfWatch)
            _watch = value
        End Set
    End Property

    Private _mobilephone As ListOfMobilePhone
    Public Property mobilephone As ListOfMobilePhone
        Get
            mobilephone = _mobilephone
        End Get
        Set(ByVal value As ListOfMobilePhone)
            _mobilephone = value
        End Set
    End Property

    Private _glasses As ListOfGlasses
    Public Property glasses As ListOfGlasses
        Get
            glasses = _glasses
        End Get
        Set(ByVal value As ListOfGlasses)
            _glasses = value
        End Set
    End Property

    Private _hat As ListOfHat
    Public Property hat As ListOfHat
        Get
            hat = _hat
        End Get
        Set(ByVal value As ListOfHat)
            _hat = value
        End Set
    End Property

    Private _room As ListOfRoom
    Public Property room As ListOfRoom
        Get
            room = _room
        End Get
        Set(ByVal value As ListOfRoom)
            _room = value
        End Set
    End Property

    Private _pets As List(Of ListOfPets)
    Public Property pets As List(Of ListOfPets)
        Get
            pets = _pets
        End Get
        Set(ByVal value As List(Of ListOfPets))
            _pets = value
        End Set
    End Property

End Class

Public Class ListOfCharacter
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property

End Class

Public Class ListOfShirt
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfPants
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfShoe
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfHead
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfScarf
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfArmband
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfWatch
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfMobilePhone
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfGlasses
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfHat
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfPets
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

Public Class ListOfRoom
    Private _id As String
    Public Property id As String
        Get
            id = _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _startSpriteId As String
    Public Property startSpriteId As String
        Get
            startSpriteId = _startSpriteId
        End Get
        Set(ByVal value As String)
            _startSpriteId = value
        End Set
    End Property

    Private _totalFrames As Integer
    Public Property totalFrames As Integer
        Get
            totalFrames = _totalFrames
        End Get
        Set(ByVal value As Integer)
            _totalFrames = value
        End Set
    End Property

    Private _spritePath As String
    Public Property spritePath As String
        Get
            spritePath = _spritePath
        End Get
        Set(ByVal value As String)
            _spritePath = value
        End Set
    End Property
End Class

#End Region

#Region "Class อันเก่า"
Public Class ITList
    Public Property Items() As List(Of Items)
        Get
            Return _Items
        End Get
        Set(ByVal value As List(Of Items))
            _Items = value
        End Set
    End Property
    Private _Items As List(Of Items)
End Class

<Serializable()> _
Public Class Items
    Private _Id As String
    Public Property Id As String
        Get
            Return _Id
        End Get
        Set(ByVal value As String)
            _Id = value
        End Set
    End Property

    Private _Position As String
    Public Property Position As String
        Get
            Return _Position
        End Get
        Set(ByVal value As String)
            _Position = value
        End Set
    End Property
End Class
#End Region

