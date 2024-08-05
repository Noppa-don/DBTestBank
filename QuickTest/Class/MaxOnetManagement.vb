Imports KnowledgeUtils
Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

Public Class MaxOnetManagement
    Private dbLicenseKey As New ClassConnectSql(False, ConfigurationManager.ConnectionStrings("LicensKeyConnectionString").ConnectionString)
    Private db As New ClassConnectSql()
    Public Property KCU_Type As MaxOnetRegisterType

    'Private MethodName As String
    Public Property KeyCode As String
    Public Property DeviceId As String
    Public Property TokenId As String
    Public Property StudentId As String
    Public Property StudentName As String
    Public Property StudentLastName As String
    Public Property StudentClass As String
    Public Property StudentClassName As String
    Public Property StudentRoom As String
    Public Property StudentRoomId As String
    Public Property StudentNumber As String
    Public Property StudentGender As String
    Public Property StudentPhone As String
    Public Property UserName As String
    Public Property Password As String
    Public Property StudentSchool As String
    Public Property SujectList As List(Of String)

    Private SchoolCode As String
    Private RoomId As String
    Dim redis As New RedisStore()

    Public Sub New()
        If HttpContext.Current.Application("DefaultSchoolCode") Is Nothing Then
            KNConfigData.LoadData()
            SchoolCode = HttpContext.Current.Application("DefaultSchoolCode").ToString()
        End If

    End Sub

    Public Function GetToken() As String

        db.OpenWithTransection()

        Try
            'STEP :: 1 Validate Key
            'Check Wrong Password
            If Not ValidKeyCode() Then
                If KCU_Type = MaxOnetRegisterType.parent Then
                    Return String.Format("|@#{0}#@|", "-1")
                Else
                    ':::: 2 ::::
                    Return "-1"
                End If
            End If
            ':::: 1 ::::

            'Check ExpireDate
            If IsExpireDate() Then
                If KCU_Type = MaxOnetRegisterType.parent Then
                    Return String.Format("|@#{0}#@|", "-2")
                Else
                    ':::: 4,6 ::::
                    Return "-1"
                End If
            End If
            ClsLog.Record("STEP 1. Validate Key ผ่าน")
            ':::: 3,5 ::::

            'STEP :: 2 Check History
            Dim tokenId As String = "-1"
            Dim studentId As String = ""

            'ถ้าเป็นนักเรียน
            If KCU_Type = MaxOnetRegisterType.student Then
                ClsLog.Record("เริ่มลงทะเบียนนักเรียน")

                ClsLog.Record("Check History 1 : Keycode : " & KeyCode.ToString & " เคยลงทะเบียนกับ Device อะไรบ้าง")
                Dim dtDevices As DataTable = GetDevicesStudentRegisteredKeyCode()

                If dtDevices.Rows.Count = 0 Then
                    ':::: 7 ::::
                    ClsLog.Record("ยังไม่มี Device ที่ลงทะเบียนกับ KeyCode : " & KeyCode.ToString)

                    ClsLog.Record("Check History 2 : check ว่า Device : " & DeviceId.ToString & " เคยลงทะเบียนกับ KeyCode อื่นหรือไม่")
                    If DeviceRegisteredWithOtherKeycode() Then
                        ':::: 12 ::::
                        ClsLog.Record("Device : " & DeviceId.ToString & " เคยลงทะเบียนกับ KeyCode อื่นแล้ว , Disable ทิ้ง")
                        DisableDevice(DeviceId, False)
                    Else
                        ':::: 11 ::::
                        ClsLog.Record("Device : " & DeviceId.ToString & " ยังไม่เคยใช้ลงทะเบียน")
                    End If

                    tokenId = InstallDevice()

                    ClsLog.Record("Register : TokenId = " & tokenId)

                    ClsLog.Record("Register : Update Key ที่ LicenseKey แบบ Activate ครั้งแรก")
                    UpdateRegisterDate(False)

                Else
                    ':::: 8 ::::
                    ClsLog.Record("มี Device ที่ลงทะเบียนกับ KeyCode : " & KeyCode.ToString & " ทั้งหมด " & dtDevices.Rows.Count & " เครื่อง")
                    ClsLog.Record("Check History 1.2 : check ว่า Device : " & DeviceId.ToString & " คือเครื่องที่เคยลงอยู่แล้วหรือไม่")

                    If dtDevices.Rows(0)("KCU_DeviceId") = DeviceId Then
                        ':::: 10 ::::
                        tokenId = dtDevices.Rows(0)("KCU_Token").ToString().Replace("-", "")
                        ClsLog.Record("Register : ใช้เครื่องเดิม TokenId = " & tokenId.ToString)

                        'Update Device อื่นทิ้ง Update ปัจจุบันเป็น 1
                        DisableOldKeyCodeAndEnableNewDevice(KeyCode, DeviceId)
                        ClsLog.Record("Register : Disable Device อื่นๆ และ Enable Device นี้")

                        UpdateRegisterDate(False)
                        ClsLog.Record("Register : Update Key ที่ LicenseKey แบบ Update เครื่องเดิม")
                    Else
                        ':::: 9 ::::
                        ClsLog.Record("Register : ไม่ใช่เครื่องเดิม")
                        ClsLog.Record("Register : ลงทะเบียนเป็นเครื่องใหม่ และทำการ Disable เครื่องเก่าทิ้ง")


                        tokenId = InstallDevice()
                        ClsLog.Record("Register : TokenId = " & tokenId)

                        'DisableOldKeyCodeAndEnableNewDevice(KeyCode)

                        ClsLog.Record("Register : Update Key ที่ LicenseKey แบบ Activate ครั้งแรก")
                        UpdateRegisterDate(True)

                    End If
                End If

            ElseIf KCU_Type = MaxOnetRegisterType.parent Then
                'ถ้าเป็นผู้ปกครอง
                'ดึง Device นี้ที่ลง Keycode นี้ออกมา

                Dim dtParentKeycode As DataTable = GetParentRegisteredAllKeyCode()
                ClsLog.Record("1 ดึง Device ผู้ปกครองที่เคยลงทะเบียนกับ KeyCode นี้ ออกมาได้ " & dtParentKeycode.Rows.Count & " Row")
                If dtParentKeycode.Rows.Count > 0 Then
                    ClsLog.Record("2 ถ้ามีแล้วเริ่ม Update Register แบบ Update ของเดิม")
                    UpdateRegisterDate(True)
                    tokenId = dtParentKeycode.Rows(0)("KCU_Token").ToString().Replace("-", "")
                    studentId = dtParentKeycode.Rows(0)("KCU_OwnerId").ToString()
                    ClsLog.Record("TokenId " & tokenId & "StudentId " & studentId)
                Else
                    ClsLog.Record("2 ถ้าไม่มีแล้วเริ่ม Update Register แบบครั้งใหม่")
                    UpdateRegisterDate(False)
                    studentId = db.ExecuteScalarWithTransection("SELECT KCU_OwnerId FROM maxonet_tblkeycodeusage WHERE KCU_Type = 0  And KeyCode_Code = '" & KeyCode & "';")
                    ClsLog.Record("StudentId " & studentId)
                    tokenId = InstallParentDevice(studentId, KeyCode, DeviceId)

                    ClsLog.Record("TokenId " & tokenId & "StudentId " & studentId)
                End If
            End If

            'dbLicenseKey.CommitTransection()
            db.CommitTransection()
            db = Nothing
            If (KCU_Type = MaxOnetRegisterType.parent) Then
                If tokenId = "-1" Or tokenId = "-2" Then
                    Return String.Format("|@#{0}#@|", tokenId)
                Else
                    Return String.Format("|@#{0},{1}#@|", tokenId, studentId)
                End If
            End If

            ClsLog.Record("4. Return TokenId")
            Return tokenId

        Catch ex As Exception When TypeOf ex Is FormatException
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            dbLicenseKey.RollbackTransection()
            db.RollbackTransection()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            dbLicenseKey.RollbackTransection()
            db.RollbackTransection()
        Catch ex As Exception
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            dbLicenseKey.RollbackTransection()
            db.RollbackTransection()
        End Try

    End Function

    Public Function ValidKeyCode() As Boolean
        'Check ว่าที่ DB LicenseKey มี Key นี้มั้ย
        Try
            Dim dt As DataTable = dbLicenseKey.getdata("SELECT * FROM maxonet_tblKeyCode WHERE KeyCode_IsActive = 1 And KeyCode_Code = '" & KeyCode.CleanSQL & "' 
                                                    And KeyCode_Username = '" & UserName.CleanSQL & "';")
            If dt.Rows.Count > 0 Then
                'Check SchoolCode ถ้า Key นี้ผูกโรงเรียน ให้ Insert โรงเรียนให้ถูก
                If Not IsDBNull(dt.Rows(0)("Keycode_School")) Then
                    SchoolCode = dt.Rows(0)("Keycode_School").ToString
                    StudentSchool = dt.Rows(0)("Keycode_School").ToString
                    HttpContext.Current.Application("DefaultSchoolCode") = SchoolCode
                End If
                Return True
                Else
                    ClsLog.Record("1.1 CheckUserPass ไม่ผ่าน")
                Return False
            End If
        Catch ex As Exception
            ClsLog.Record("CheckUserPass Error!! : " & ex.InnerException.ToString)
            Return False
        End Try

    End Function
    Private Function IsExpireDate() As Boolean
        'Check LicenseKey
        'ถ้าหมดแล้ว ไปดูว่า ถ้าเคยลงทะเบียนแล้ว หมดอายุหรือยัง
        Try
            Dim sql As String = "SELECT case when KeyCode_ExpireDate is null then 1 
			when KeyCode_ExpireDate >= dbo.GetThaiDate() then 1
			when KeyCode_ExpireDate < dbo.GetThaiDate() then 0 end as IsExpire
            FROM maxonet_tblKeyCode WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "';"

            Dim IsExpire As String = dbLicenseKey.ExecuteScalar(sql)

            If IsExpire = "0" Or IsExpire = "" Then
                ':::: 4 ::::
                sql = "SELECT case when KCU_ExpireDate is null then 1 
			            when KCU_ExpireDate >= dbo.GetThaiDate() then 1
			            when KCU_ExpireDate < dbo.GetThaiDate() then 0 end as IsExpire
                        FROM maxonet_tblKeyCodeUsage WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "' 
                        and KCU_IsActive = 1 and KCU_Type <> 2;"
                IsExpire = db.ExecuteScalar(sql)

                If IsExpire = "0" Or IsExpire = "" Then
                    ':::: 6 ::::
                    ClsLog.Record("1.2 CheckIsExpire ไม่ผ่าน")
                    Return True
                Else
                    ':::: 5 ::::
                    Return False
                End If

            Else
                ':::: 3 ::::
                Return False
            End If
        Catch ex As Exception
            ClsLog.Record("CheckIsExpire Error!! : " & ex.InnerException.ToString)
            Return True
        End Try

    End Function
    Private Function IsEndDate() As Boolean
        Dim dt As DataTable = dbLicenseKey.getdata("SELECT * FROM maxonet_tblKeyCode WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "' AND (dbo.GetThaiDate() <= KeyCode_EndDate);")
        If dt.Rows.Count = 0 Then Return True
        Return False
    End Function
    Private Function GetDevicesStudentRegisteredKeyCode() As DataTable
        Try
            Return db.getdataWithTransaction("SELECT * FROM maxonet_tblKeyCodeUsage WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";")
        Catch ex As Exception
            ClsLog.Record("GetDevicesStudentRegisteredKeyCode Error!! : " & ex.InnerException.ToString)
        End Try

    End Function

    Private Function GetParentRegisteredAllKeyCode() As DataTable
        Return db.getdataWithTransaction("SELECT * FROM maxonet_tblKeyCodeUsage WHERE KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND KCU_Type = " & KCU_Type & " and KeyCode_Code = '" & KeyCode.CleanSQL & "';")
    End Function

    Private Function DeviceRegisteredWithOtherKeycode()
        Dim dt As DataTable = db.getdataWithTransaction("SELECT * FROM maxonet_tblKeyCodeUsage WHERE KCU_IsActive = 1 AND KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND KeyCode_Code <> '" & KeyCode.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";")
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Sub EnableDevice()
        db.ExecuteScalarWithTransection("UPDATE maxonet_tblKeyCodeUsage SET KCU_IsActive = 1,KCU_LastUpdate = dbo.GetThaiDate() WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "' AND KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";")
    End Sub

    Private Sub DisableDevice(ByVal KCU_DeviceId As String, ByVal IsCurrentKeyCode As Boolean)
        Dim operate As String = If((IsCurrentKeyCode = True), "=", "<>")
        db.ExecuteScalarWithTransection("UPDATE maxonet_tblKeyCodeUsage SET KCU_IsActive = 0,KCU_LastUpdate = dbo.GetThaiDate() 
                                            WHERE KCU_IsActive = 1 AND KCU_DeviceId = '" & KCU_DeviceId.CleanSQL & "' 
                                            AND KeyCode_Code " & operate.CleanSQL & " '" & KeyCode.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";")
    End Sub

    Private Sub DisableOldKeyCodeAndEnableNewDevice(KeyCode As String, Optional Deviceid As String = "")
        Dim sql As String

        sql = "UPDATE maxonet_tblKeyCodeUsage SET KCU_IsActive = 0,KCU_LastUpdate = dbo.GetThaiDate() 
                             WHERE KCU_IsActive = 1 AND KeyCode_Code = '" & KeyCode.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";"
        If Deviceid = "" Then
            sql &= "UPDATE maxonet_tblKeyCodeUsage SET KCU_IsActive = 1,KCU_LastUpdate = dbo.GetThaiDate() 
                             WHERE KCU_DeviceId = '" & Deviceid & "' AND KCU_Type = " & KCU_Type & ";"
        End If


        db.ExecuteScalarWithTransection(sql)
    End Sub

    Private Function InstallDevice() As String

        Try
            Dim token = Guid.NewGuid()

            Dim isActive As Integer = If((KCU_Type = MaxOnetRegisterType.student), 0, 1) ' ตอนลงทะเบียนถ้าเป็น เด็กต้องให้ isactive = 0 ไว้ก่อน เผื่อลงทะเบียนไม่สำเร็จ หยุดกลางทาง

            Dim dtKeyCodeDetail As DataTable
            Dim sql As String = "SELECT KeyCode_EndDateAmount,KeyCode_LimitExam,KeyCode_Credit FROM maxonet_tblKeyCode WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "';"
            dtKeyCodeDetail = dbLicenseKey.getdata(sql)

            Dim LimitExam As String = dtKeyCodeDetail.Rows(0)("KeyCode_LimitExam").ToString
            If LimitExam = "" Or LimitExam = "0" Then
                LimitExam = "null"
            End If

            Dim EndDateAmount As String = dtKeyCodeDetail.Rows(0)("KeyCode_EndDateAmount").ToString
            If EndDateAmount = "" Or EndDateAmount = "0" Then
                EndDateAmount = "null"
            Else
                EndDateAmount = "dbo.GetThaiDate() + " & EndDateAmount
            End If

            Dim KeycodeCredit As String = dtKeyCodeDetail.Rows(0)("KeyCode_Credit")

            sql = "INSERT INTO maxonet_tblKeyCodeUsage VALUES(NEWID(),'" & KeyCode.CleanSQL & "','" & token.ToString().CleanSQL & "',
                            '" & DeviceId.CleanSQL & "'," & isActive & ",dbo.GetThaiDate()," & KCU_Type & ",dbo.GetThaiDate(),NULL,
                            " & LimitExam & "," & EndDateAmount & ",'" & KeycodeCredit & "');"

            db.ExecuteWithTransection(sql)
            ClsLog.Record("InstallDevice เรียบร้อย TokenId = " & token.ToString)
            Return token.ToString().Replace("-", "")

        Catch ex As Exception
            ClsLog.Record(" InstallDevice Error!! ex = " & ex.ToString)
            ClsLog.Record(" InstallDevice Error!! ex-message = " & ex.Message)
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
        Return -1
    End Function

    Private Function InstallParentDevice(studentIdValue As String, keycodeValue As String, deviceValue As String) As String
        Try

            Dim token = Guid.NewGuid()

            Dim sql As String = "SELECT KeyCode_LimitExam FROM maxonet_tblKeyCode WHERE KeyCode_Code = '" & keycodeValue.CleanSQL & "';"
            Dim LimitExam As String = dbLicenseKey.ExecuteScalar(sql)

            If LimitExam = "" Or LimitExam = "0" Then
                LimitExam = "null"
            End If

            If studentIdValue <> "" Then
                studentIdValue = "'" & studentIdValue & "'"
            Else
                studentIdValue = "null"
            End If

            sql = "INSERT INTO maxonet_tblKeyCodeUsage VALUES(NEWID(),'" & keycodeValue.CleanSQL & "','" & token.ToString().CleanSQL & "','" & deviceValue.CleanSQL & "',1
                            ,dbo.GetThaiDate(),1,dbo.GetThaiDate()," & studentIdValue & "," & LimitExam & ",null,null);"

            ClsLog.Record("ก่อน InsertSQL")
            ClsLog.Record(sql)
            db.ExecuteWithTransection(sql)
            ClsLog.Record("หลัง InsertSQL")

            Return token.ToString().Replace("-", "")

        Catch ex As Exception When TypeOf ex Is FormatException
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            Dim Errortxt As String
            Errortxt = ex.ToString + "|" + ex.Message
            Return Errortxt
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            Dim Errortxt As String
            Errortxt = ex.ToString + "|" + ex.Message
            Return Errortxt
        Catch ex As Exception
            ClsLog.Record(" ex = " & ex.ToString)
            ClsLog.Record(" ex-message = " & ex.Message)
            Dim Errortxt As String
            Errortxt = ex.ToString + "|" + ex.Message
            Return Errortxt
        End Try

    End Function

    ''' <summary>
    ''' function ในการ update date ลงทะเบียน,จำนวนของการลงทะเบียน,วันหมดอายุ ของ keycode นั้นๆ ที่ maxonet_tblKeycode
    ''' </summary>
    ''' <param name="IsActivatedKey"></param>
    Private Sub UpdateRegisterDate(ByVal IsActivatedKey As Boolean)
        Try
            Dim sql As String
            If IsActivatedKey Then
                'ครั้งต่อๆ มา ไม่ต้อง Update วันหมดอายุ
                sql = " UPDATE maxonet_tblKeyCode Set KeyCode_DateLastRegister = dbo.GetThaiDate(),KeyCode_LastUpdate = dbo.GetThaiDate(),
                    KeyCode_RegistrationCounter = KeyCode_RegistrationCounter + 1 WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "';"
            Else
                'ครั้งแรก
                sql = " UPDATE maxonet_tblKeyCode SET KeyCode_DateFirstRegister = dbo.GetThaiDate(),KeyCode_DateLastRegister = dbo.GetThaiDate(),
                    KeyCode_LastUpdate = dbo.GetThaiDate(),KeyCode_ExpireDate = DATEADD(DAY,KeyCode_EndDateAmount,dbo.GetThaiDate()),
                    KeyCode_RegistrationCounter = KeyCode_RegistrationCounter + 1 
                    WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "'; "

            End If

            dbLicenseKey.ExecuteScalar(sql)

        Catch ex As Exception
            ClsLog.Record("RegisterStudentMaxOnet ex" & ex.InnerException.ToString)
        End Try

    End Sub

    ''' <summary>
    '''  function insert or update student maxonet
    ''' </summary>
    ''' 
    Public Function RegisterStudentMaxOnet() As String

        db.OpenWithTransection()

        Try
            KeyCode = GetKeyCodeByTokenId()
            If KeyCode = "" Then Return "-1"
            StudentClass = ChangeClassNameFromTablet()

            If Not IsKeyCodeWasFirstRegisterdWithT360() Then 'thisKeyCodeHasBeenRegistered()
                StudentId = GetStudentIdT360()
                'DisableDeviceT360IsActiveWithThisKeyCode() 'update tablet ที่ใช้กับ key นี้อยู่ให้ isactive = 0
                Dim deviceStatus As EnumDeviceStatus = GetDeviceT360Status()

                If deviceStatus = EnumDeviceStatus.NotRegister Then
                    NewTabletT360()
                    NewOwnerTabletT360()
                Else
                    If DeviceRegisteredWithOtherKeycode() Then 'device register with other keycode and isactive = 1 ?
                        DisableDeviceT360IsActiveWithOtherKeyCode()
                    End If

                    EnableDevice()
                    EnableTabletT360()
                    ' device นี้ เคยลงกับ key นี้มั้ย
                    If IsThisDeviceRegisteredWithThisKeyCode() Then
                        ClsLog.Record("เคยลงทะเบียนไปแล้ว กำลังจะ enable tablet")
                        EnableOwnerTabletT360()
                    Else
                        NewOwnerTabletT360()
                    End If
                End If

                UpdateStudentT360()
                'UpdateStudentSubjectToNewDevice(KeyCode)
            Else
                Dim deviceStatus As EnumDeviceStatus = GetDeviceT360Status()
                Select Case deviceStatus
                    Case EnumDeviceStatus.NotRegister
                        NewTabletT360()
                    Case EnumDeviceStatus.RegisteredWithActive
                        DisableOwnerTableT360()
                    Case EnumDeviceStatus.RegisteredWithNotActive
                        EnableTabletT360()
                End Select
                StudentId = GetAnotherDeviceStudentId(KeyCode)
                If StudentId = "" Then
                    StudentId = Guid.NewGuid().ToString()
                    NewStudentT360()
                End If

                NewOwnerTabletT360()


                'NewStudentRoomT360() comment ไว้ ให้ไป insert ตอน confirm หลังจากเลือกชั้นวิชาแล้ว
            End If
            UpdateMaxOnetOwnerId()
            db.CommitTransection()

            'ปิดแล้วเปิดใหม่เพราะต้องการ Data ที่ต้อง Commit ไปก่อน
            db.OpenWithTransection()
            'UpdateParentOwnerAndInsertParentData(KeyCode, False, TokenId.ToString().Replace("-", ""))
            'PopulateParentReportDataRedis(Nothing, True)
            ' db.CommitTransection()

            ClsLog.Record("RegisterStudentMaxOnet 4")
            Return "1"
        Catch ex As Exception When TypeOf ex Is FormatException
            db.RollbackTransection()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
        Catch ex As Exception
            ClsLog.Record("RegisterStudentMaxOnet ex" & ex.InnerException.ToString)
            db.RollbackTransection()
        End Try
        Return "-1"
    End Function



    Public Function UpdateStudentMaxOnet() As String
        Try
            db.OpenWithTransection()
            Dim prefixName As String = If((StudentGender = "m"), "ด.ช.", "ด.ญ.")

            Dim sql As String = ""
            sql = "Update t360_tblStudent set Student_FirstName = '" & StudentName & "' , Student_LastName = '" & StudentLastName & "', Student_Gender = '" & StudentGender & "', 
                    Student_PrefixName = '" & prefixName & "', UserName = '" & UserName & "', Password = '" & Password & "',Student_CurrentClass = '" & StudentClassName & "'
                    where Student_Id = '" & StudentId.ToString & "'"


            db.ExecuteScalarWithTransection(sql)

            sql = " select r.Room_Id,s.Student_CurrentClass,s.Student_CurrentRoom,s.School_Code from t360_tblRoom r right join t360_tblStudent s 
                    on r.Class_Name = s.Student_CurrentClass and r.Room_Name = s.Student_CurrentRoom  
                    where s.Student_Id = '" & StudentId.ToString & "'"

            Dim RoomId As String = ""
            Dim dtRoom As DataTable = db.getdataWithTransaction(sql)
            If dtRoom.Rows(0)("Room_Id").ToString = "" Then
                Dim NewRoomId = Guid.NewGuid()
                sql = "Insert into t360_tblRoom(School_Code,Class_Name,Room_Name,Room_Id,Room_IsActive,LastUpdate,ClientId,Room_Address)
                        values('" & dtRoom.Rows(0)("School_Code").ToString & "','" & dtRoom.Rows(0)("Student_CurrentClass").ToString & "'
                        ,'" & dtRoom.Rows(0)("Student_CurrentRoom").ToString & "','" & NewRoomId.ToString & "',1,dbo.GetThaiDate(),null,null);"
                db.ExecuteScalarWithTransection(sql)
                RoomId = NewRoomId.ToString
            Else
                RoomId = dtRoom.Rows(0)("Room_Id").ToString
            End If

            sql = "Update t360_tblStudent set Student_CurrentRoomId = '" & RoomId & "' where Student_Id = '" & StudentId.ToString & "';"
            db.ExecuteScalarWithTransection(sql)

            Dim tokenguid As Guid = New Guid(TokenId.ToString)

            sql = "Update maxonet_tblKeyCodeUsage Set KCU_OwnerId = '" & StudentId.ToString & "',KCU_IsActive = 1 where KCU_Token = '" & tokenguid.ToString & "' and KCU_DeviceId = '" & DeviceId.ToString & "'"
            db.ExecuteScalarWithTransection(sql)

            db.CommitTransection()
            Return "1"
        Catch ex As Exception When TypeOf ex Is FormatException
            db.RollbackTransection()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
        Catch ex As Exception
            ClsLog.Record("RegisterStudentMaxOnet ex" & ex.InnerException.ToString)
            db.RollbackTransection()
        End Try
        Return "-1"
    End Function

#Region "maxonet management"

    Private Sub UpdateMaxOnetOwnerId()
        Dim sql As String = "UPDATE maxonet_tblKeyCodeUsage SET KCU_OwnerId = '" & StudentId.CleanSQL & "',KCU_LastUpdate = dbo.GetThaiDate(),KCU_IsActive = 1 WHERE KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND (KCU_Type = 0 or kcu_Type = 4) AND KeyCode_Code = '" & KeyCode.CleanSQL & "' AND KCU_Token = '" & Guid.Parse(TokenId).ToString("D").CleanSQL & "';"
        db.ExecuteScalarWithTransection(sql)
    End Sub
    Private Function IsKeyCodeWasFirstRegisterdWithT360() As Boolean
        Dim sql As String = "SELECT * FROM maxonet_tblKeyCodeUsage WHERE KCU_IsActive = 1 AND KCU_Type = 0 AND KCU_OwnerId IS NOT NULL AND KeyCode_Code = '" & KeyCode.CleanSQL & "';"
        If db.getdataWithTransaction(sql).Rows.Count = 0 Then Return True
        Return False
    End Function

    Private Function IsThisDeviceRegisteredWithThisKeyCode() As Boolean
        Dim sql As New StringBuilder
        sql.Append("SELECT * FROM t360_tblTablet t INNER JOIN t360_tblTabletOwner o ON t.Tablet_Id = o.Tablet_Id ")
        sql.Append("WHERE t.Tablet_SerialNumber = '" & DeviceId.CleanSQL & "' AND o.Owner_Id = '" & StudentId.CleanSQL & "';")
        Dim dt As DataTable = db.getdataWithTransaction(sql.ToString())
        If dt.Rows.Count > 0 Then Return True
        Return False
    End Function

    Private Sub DisableDeviceT360IsActiveWithThisKeyCode()
        Dim sql As String = "SELECT t.Tablet_Id FROM maxonet_tblKeyCodeUsage k INNER JOIN t360_tblTablet t ON k.KCU_DeviceId = t.Tablet_SerialNumber WHERE k.KCU_IsActive = 1 AND k.KeyCode_Code = '" & KeyCode.CleanSQL & "';"
        Dim tabletId As String = db.ExecuteScalarWithTransection(sql)
        If tabletId <> "" Then
            db.ExecuteScalarWithTransection("UPDATE maxonet_tblKeyCodeUsage SET KCU_IsActive = 0,KCU_LastUpdate = dbo.GetThaiDate() WHERE KCU_IsActive = 1 AND KeyCode_Code = '" & KeyCode.CleanSQL & "' AND KCU_Type = " & KCU_Type & ";")
            DisableDeviceT360(tabletId)
        End If
    End Sub

    Private Sub DisableDeviceT360IsActiveWithOtherKeyCode()
        Dim sql As String = "SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "';"
        Dim tabletId As String = db.ExecuteScalarWithTransection(sql)
        If tabletId <> "" Then
            DisableDeviceT360(tabletId)
        End If
    End Sub

    Private Sub DisableDeviceT360(tabletId As String)
        Dim sql As String = "UPDATE t360_tblTablet SET Tablet_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE  Tablet_Id = '" & tabletId.CleanSQL & "';"
        sql &= "UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Tablet_Id = '" & tabletId.CleanSQL & "' AND Owner_Id = '" & StudentId.CleanSQL & "';"
        db.ExecuteScalarWithTransection(sql)
    End Sub

    Private Function GetStudentIdT360() As String
        Dim sql As String = "SELECT TOP 1 KCU_OwnerId FROM maxonet_tblKeyCodeUsage WHERE KCU_OwnerId IS NOT NULL AND KCU_Type = 0 AND KeyCode_Code = '" & KeyCode.CleanSQL & "';"
        Return db.ExecuteScalarWithTransection(sql)
    End Function

    Private Function GetDeviceRegisteredWithT360()
        Dim sql As New StringBuilder()
        sql.Append("SELECT * FROM maxonet_tblKeyCodeUsage m INNER JOIN t360_tblTablet t ON m.KCU_DeviceId = t.Tablet_SerialNumber ")
        sql.Append(" INNER JOIN t360_tblTabletOwner o ON t.Tablet_Id = o.Tablet_Id ")
        sql.Append(" WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "'  AND KCU_Type = 0;") ' AND KCU_Token = '" & Guid.Parse(TokenId).ToString("D") & "'
        Return db.getdataWithTransaction(sql.ToString())
    End Function

    Private Function GetKeyCodeByTokenId() As String
        Return db.ExecuteScalarWithTransection("SELECT KeyCode_Code FROM maxonet_tblKeyCodeUsage WHERE KCU_Token = '" & Guid.Parse(TokenId).ToString("D").CleanSQL & "' AND KCU_DeviceId = '" & DeviceId.CleanSQL & "';")
    End Function

    Public Function GetTokenByDevice() As String
        Return db.ExecuteScalar("SELECT KCU_Token FROM maxonet_tblKeyCodeUsage WHERE KCU_DeviceId = '" & DeviceId.CleanSQL & "';")
    End Function

    Private Function GetDeviceT360Status() As EnumDeviceStatus
        Dim sql As String = "SELECT * FROM t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "' {0};"
        If db.getdataWithTransaction(String.Format(sql, "")).Rows().Count() = 0 Then Return EnumDeviceStatus.NotRegister
        If db.getdataWithTransaction(String.Format(sql, " AND Tablet_IsActive = 1 ")).Rows().Count() > 0 Then Return EnumDeviceStatus.RegisteredWithActive
        Return EnumDeviceStatus.RegisteredWithNotActive
    End Function

    Public Function NewTablet360PC()
        Try
            db.OpenWithTransection()
            NewTabletT360()
            NewOwnerTabletT360()
            db.CommitTransection()
            Return "1"
        Catch ex As Exception When TypeOf ex Is FormatException
            db.RollbackTransection()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
        Catch ex As Exception
            ClsLog.Record("NewTablet360PC ex" & ex.InnerException.ToString)
            db.RollbackTransection()
        End Try

    End Function

    Private Sub NewTabletT360()
        db.ExecuteScalarWithTransection("INSERT INTO t360_tblTablet VALUES (NEWID(),'" & SchoolCode.CleanSQL & "','" & DeviceId.CleanSQL & "',1,1,1,'',dbo.GetThaiDate(),dbo.GetThaiDate(),NULL,NULL);")
    End Sub

    Private Sub EnableTabletT360()
        db.ExecuteScalarWithTransection("UPDATE t360_tblTablet SET Tablet_IsActive = 1,LastUpdate = dbo.GetThaiDate() WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "';")
    End Sub

    Private Sub DisableOwnerTableT360()
        db.ExecuteScalarWithTransection("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE TabletOwner_IsActive = 1 AND Tablet_Id = (SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "');")
    End Sub

    Private Function GetAnotherDeviceStudentId(keyCode As String) As String
        Dim sql As String = ""
        sql = "select distinct KCU_OwnerId from maxonet_tblKeyCodeUsage where KeyCode_Code = '" & keyCode & "' and KCU_OwnerId is not null;"
        Dim OwnerId As String = db.ExecuteScalarWithTransection(sql)
        Return OwnerId
    End Function

    Private Sub NewOwnerTabletT360()
        Dim sql As New StringBuilder()
        sql.Append("DECLARE @TabletID AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "');")
        sql.Append("INSERT INTO t360_tblTabletOwner VALUES(NEWID(),@TabletID,'" & StudentId.CleanSQL & "','" & StudentSchool.CleanSQL & "',2,dbo.GetThaiDate(),NULL,1,dbo.GetThaiDate(),NULL);")
        db.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Sub EnableOwnerTabletT360()
        Dim sql As New StringBuilder()
        sql.Append("DECLARE @TabletID AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_SerialNumber = '" & DeviceId.CleanSQL & "');")
        sql.Append("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 1,LastUpdate = dbo.GetThaiDate() WHERE Tablet_Id = @TabletID AND Owner_Id = '" & StudentId.CleanSQL & "';")
        db.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Sub NewStudentT360()
        Dim prefixName As String = If((StudentGender = "m"), "ด.ช.", "ด.ญ.")
        Dim sql As New StringBuilder()
        If UserName IsNot Nothing Then
            sql.Append("INSERT INTO t360_tblStudent VALUES('" & StudentId.CleanSQL & "','" & StudentSchool.CleanSQL & "','0000',N'" & prefixName.CleanSQL & "',N'" & StudentName.CleanSQL & "',N'" & StudentLastName.CleanSQL & "',NULL,NULL,NULL,NULL,NULL,")
            sql.Append("1,0,N'" & StudentClass.CleanSQL & "','" & StudentRoom.CleanSQL & "','" & StudentRoomId.CleanSQL & "','" & StudentPhone.CleanSQL & "',NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,'" & StudentGender.CleanSQL & "',NULL,NULL,NULL,NULL,0,'" & UserName.CleanSQL & "','" & Password.CleanSQL & "');")
        Else
            sql.Append("INSERT INTO t360_tblStudent VALUES('" & StudentId.CleanSQL & "','" & StudentSchool.CleanSQL & "','0000',N'" & prefixName.CleanSQL & "',N'" & StudentName.CleanSQL & "',N'" & StudentLastName.CleanSQL & "',NULL,NULL,NULL,NULL,NULL,")
            sql.Append("1,0,N'" & StudentClass.CleanSQL & "','" & StudentRoom.CleanSQL & "','" & StudentRoomId.CleanSQL & "','" & StudentPhone.CleanSQL & "',NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,'" & StudentGender.CleanSQL & "',NULL,NULL,NULL,NULL,0,NULL,NULL);")
        End If


        db.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Sub NewStudentRoomT360()

        'แบบสุ่มห้อง
        'Dim sql As New StringBuilder()
        'sql.Append("INSERT INTO t360_tblStudentRoom VALUES(NEWID(),'" & StudentId.CleanSQL & "','" & StudentSchool.CleanSQL & "','" & StudentNumber.CleanSQL & "',N'" & StudentClass.CleanSQL & "',N'" & StudentRoom.CleanSQL & "'")
        'sql.Append(",dbo.GetThaiDate(),'2559',0,1,'" & GetCalendarID() & "',dbo.GetThaiDate(),'" & RoomId.CleanSQL & "',NULL);")
        'db.ExecuteScalarWithTransection(sql.ToString())

        'แบบลงตามที่กรอกในหน้าลงทะเบียน
        Dim sql As String = ""
        sql = "insert into t360_tblStudentRoom
                select newid(),Student_Id,School_Code,Student_CurrentNoInRoom,Student_CurrentClass,Student_CurrentRoom,GETDATE(),'2560',0,1,(select top 1 Calendar_Id 
                from t360_tblCalendar where School_Code = std.School_Code),getdate(),std.Student_CurrentRoomId,null
                from t360_tblStudent std where Student_id = '" & StudentId.CleanSQL & "';"
        db.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Function GetCalendarID() As String
        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & StudentSchool.CleanSQL & "'; "
        Return db.ExecuteScalarWithTransection(sql)
    End Function

    Private Sub UpdateStudentT360()
        Dim prefixName As String = If((StudentGender = "m"), "ด.ช.", "ด.ญ.")
        Dim sql As New StringBuilder()
        sql.Append("UPDATE t360_tblStudent SET Student_PrefixName = N'" & prefixName.CleanSQL & "',Student_FirstName = N'" & StudentName.CleanSQL & "',Student_LastName = N'" & StudentLastName.CleanSQL & "',")
        sql.Append("Student_Phone = N'" & StudentPhone.CleanSQL & "',Student_Gender = '" & StudentGender.CleanSQL & "' {0} WHERE Student_Id = '" & StudentId.CleanSQL & "';")
        db.ExecuteScalarWithTransection(String.Format(sql.ToString(), ""))

        'comment ไว้ update เฉพาะ native เท่านั้น ไม่ต้อง update class แล้ว เพราะยินยันชั้นไปแล้ว
        'If IsOldStudentClass() Then
        '    db.ExecuteScalarWithTransection(String.Format(sql.ToString(), ""))
        'Else
        '    DisableStudentClassRoom()
        '    RandomRoomAndNumber()
        '    NewStudentRoomT360()
        '    Dim tempSql As String = ",Student_Number = " & StudentNumber & ",Student_CurrentClass = '" & StudentClass & "',Student_CurrentRoom = '" & StudentRoom & "',Student_CurrentRoomId = '" & RoomId & "'"
        '    db.ExecuteScalarWithTransection(String.Format(sql.ToString(), tempSql))
        'End If
    End Sub

    Private Sub UpdateStudentSubjectToNewDevice(KeyCode As String)
        db.ExecuteScalarWithTransection("Update maxonet_tblStudentSubject set SS_StudentId = '" & StudentId.CleanSQL & "',SS_LastUpdate = dbo.GetThaiDate() where SS_KeyCode = '" & KeyCode & "'")
    End Sub

    Private Sub UpdateStudentT360WhenConfirm()
        Dim sql As String = "UPDATE t360_tblStudent SET Student_Number = " & StudentNumber.CleanSQL & ",Student_CurrentClass = N'" & StudentClass.CleanSQL & "',Student_CurrentRoom = N'" & StudentRoom.CleanSQL & "' "
        sql &= ",Student_CurrentRoomId = '" & RoomId.CleanSQL & "',LastUpdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId.CleanSQL & "';"
        db.ExecuteScalarWithTransection(sql)
    End Sub

    Private Function IsOldStudentClass() As Boolean
        Dim sql As String = "SELECT * FROM t360_tblStudent WHERE Student_Id = '" & StudentId.CleanSQL & "' AND Student_CurrentClass = N'" & StudentClass.CleanSQL & "';"
        Dim dt As DataTable = db.getdataWithTransaction(sql)
        If dt.Rows.Count > 0 Then Return True
        Return False
    End Function

    Private Sub DisableStudentClassRoom()
        Dim sql As String = "UPDATE t360_tblStudentRoom SET SR_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId.CleanSQL & "' AND SR_IsActive = 1;"
        db.ExecuteScalarWithTransection(sql)
    End Sub

    Private Sub RandomRoomAndNumber()
        Dim r As Random = New Random
        StudentRoom = String.Format("/{0}", r.Next(1, 20) + 1)
        If db.getdataWithTransaction("SELECT * FROM t360_tblSchoolClass WHERE Class_Name = N'" & StudentClass.CleanSQL & "' AND School_Code = '" & StudentSchool.CleanSQL & "';").Rows().Count() = 0 Then
            db.ExecuteScalarWithTransection("INSERT INTO dbo.t360_tblSchoolClass ( School_Code, Class_Name ) VALUES  ( '" & StudentSchool.CleanSQL & "', N'" & StudentClass.CleanSQL & "' ); ")
            NewRoomT360()
        Else
            RoomId = db.ExecuteScalarWithTransection("SELECT Room_Id FROM t360_tblRoom WHERE Class_Name = N'" & StudentClass.CleanSQL & "' AND Room_Name = N'" & StudentRoom.CleanSQL & "' AND School_Code = '" & StudentSchool.CleanSQL & "' AND Room_IsActive = 1;")
            If RoomId = "" Then
                NewRoomT360()
            Else
                StudentNumber = db.ExecuteScalarWithTransection("SELECT COUNT(*) + 1 FROM t360_tblStudent WHERE Student_CurrentClass = N'" & StudentClass.CleanSQL & "' AND Student_CurrentRoom = N'" & StudentRoom.CleanSQL & "' AND School_Code = '" & SchoolCode.CleanSQL & "';")
            End If
        End If
    End Sub

    Private Sub NewRoomT360()
        RoomId = db.ExecuteScalarWithTransection("SELECT NEWID();")
        db.ExecuteScalarWithTransection("INSERT INTO dbo.t360_tblRoom( School_Code ,Class_Name ,Room_Name ,Room_Id ,Room_IsActive) VALUES  ( '" & StudentSchool.CleanSQL & "' ,N'" & StudentClass.CleanSQL & "' , N'" & StudentRoom.CleanSQL & "' , '" & RoomId.CleanSQL & "','1');")
        StudentNumber = 1
    End Sub

    Private Function ChangeClassNameFromTablet() As String
        If StudentClass <> "" Then
            StudentClass = StudentClass.ToUpper()
            Select Case StudentClass
                Case "K1"
                    Return "อ.1"
                Case "K2"
                    Return "อ.2"
                Case "K3"
                    Return "อ.3"
                Case "K4"
                    Return "ป.1"
                Case "K5"
                    Return "ป.2"
                Case "K6"
                    Return "ป.3"
                Case "K7"
                    Return "ป.4"
                Case "K8"
                    Return "ป.5"
                Case "K9"
                    Return "ป.6"
                Case "K10"
                    Return "ม.1"
                Case "K11"
                    Return "ม.2"
                Case "K12"
                    Return "ม.3"
                Case "K13"
                    Return "ม.4"
                Case "K14"
                    Return "ม.5"
                Case "K15"
                    Return "ม.6"
                Case Else
                    Return ""
            End Select
        End If
        Return "-1"
    End Function
#End Region

#Region "Parent"
    'MaxOnetParent ดึงค่า
    Public Function GetParentReportDataFromRedis() As String
        Log.Record(Log.LogType.ParentRegister, "GetParentReportDataFromRedis_tokenId = " & TokenId.ToLower, 0)
        Dim sql As String
        sql = "select top 1 maxonet_tblParentReport.KeyCode_Code from maxonet_tblParentReport "
        sql &= " inner join maxonet_tblKeyCodeUsage on maxonet_tblParentReport.KeyCode_Code = maxonet_tblKeyCodeUsage.KeyCode_Code"
        sql &= " where Convert(varchar(10), PR_LastUpdate, 103) <> Convert(varchar(10), dbo.GetThaiDate(), 103) And PR_IsActive = 1 "
        sql &= " And maxonet_tblKeyCodeUsage.KCU_IsActive = 1"
        sql &= " and KCU_OwnerId is not null "
        Dim IsUpdate As String = db.ExecuteScalar(sql)

        If IsUpdate IsNot Nothing Then
            UpdateParentReport()
        End If

        Dim parentDataFromRedis As String = redis.Getkey(Of String)("ParentReportData_" & TokenId.ToLower)

        If parentDataFromRedis Is Nothing Then
            PopulateParentReportDataRedis(TokenId)
            parentDataFromRedis = redis.Getkey(Of String)("ParentReportData_" & TokenId.ToLower)
        End If

        Log.Record(Log.LogType.ParentRegister, "parentDataFromRedis = " & parentDataFromRedis, 0)
        Return parentDataFromRedis

    End Function

    Private Sub PopulateParentReportDataRedis(Optional Token As String = Nothing, Optional IsUseTransaction As Boolean = False)
        Try
            ClsLog.Record("Populate Parent Redis token : " & Token & " IsUseTransaction : " & IsUseTransaction)
            Dim sql As String
            Dim dt As New DataTable

            sql = "select maxonet_tblParentReport.*,maxonet_tblKeyCodeUsage.KCU_Token from maxonet_tblParentReport inner join maxonet_tblKeyCodeUsage "
            sql &= " on maxonet_tblParentReport.KeyCode_Code = maxonet_tblKeyCodeUsage.KeyCode_Code "
            sql &= " where KCU_Type = '1' and KCU_IsActive = '1' and PR_IsActive = '1'"

            If Token IsNot Nothing Then
                Dim TokenGUId As Guid = Guid.Parse(Token)
                sql &= " and KCU_Token = '" & TokenGUId.ToString & "'"
            End If

            If IsUseTransaction Then
                ClsLog.Record("UseTransaction : " & sql)
                dt = db.getdataWithTransaction(sql)
            Else
                ClsLog.Record("NotUseTransaction : " & sql)
                Dim _DB As New ClassConnectSql()
                dt = _DB.getdata(sql)
            End If

            ClsLog.Record("ParentAmount : " & dt.Rows.Count)

            For Each eachparentReport In dt.Rows

                Dim tokenClear As String = eachparentReport("KCU_Token").ToString.Replace("-", "")

                redis.DEL("ParentReportData_" & tokenClear)

                Dim strParentData As New StringBuilder()
                strParentData.Append("|@#")
                strParentData.Append(eachparentReport("PR_SummaryPercent"))
                strParentData.Append("%|,")
                strParentData.Append(eachparentReport("PR_ResultTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_ResultDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_RecommendTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_RecommendDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_personalDataTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_personalDataDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QuantityDailyTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QuantityDailyDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QuantityPracticeTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QuantityPracticeDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QualityDailyTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QualityDailyDetail"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QualityPracticeTitle"))
                strParentData.Append("|,")
                strParentData.Append(eachparentReport("PR_QualityPracticeDetail"))
                strParentData.Append("#@|")

                ClsLog.Record("token : " & tokenClear & " ParentData : " & strParentData.ToString)

                redis.SetKey(Of String)("ParentReportData_" & tokenClear.ToLower, strParentData.ToString)

            Next

        Catch ex As Exception
            ClsLog.Record("ex : " & ex.ToString)
        End Try

    End Sub

    Public Sub UpdateParentReport()
        ClsLog.Record("Exec Store UpdateMaxOnetParentReport")
        Dim sql As String = "exec UpdateMaxOnetParentReport;"
        db.Execute(sql)
        PopulateParentReportDataRedis()
    End Sub
    ''' <summary>
    ''' เมื่อ Register เสร็จให้ Update OwnerId ด้วย OwnerId ของนักเรียน และทำการ InsertParentData
    ''' </summary>
    ''' <param name="KeyCode"></param>
    ''' <remarks></remarks>
    Public Function UpdateParentOwnerAndInsertParentData(KeyCode As String, IsParent As Boolean, tokenId As String) As Boolean
        Try
            ClsLog.Record("UpdateParentOwnerAndInsertParentData tokenId = " & tokenId)

            Dim sql As New StringBuilder

            sql.Append("Update maxonet_tblKeyCodeUsage set KCU_OwnerId = (select KCU_OwnerId from maxonet_tblKeyCodeUsage where KeyCode_Code = '")
            sql.Append(KeyCode)
            sql.Append("' and KCU_Type = '0' and KCU_IsActive = '1') where KeyCode_Code = '")
            sql.Append(KeyCode)
            sql.Append("' and KCU_Type = '1' and KCU_IsActive = '1';")
            sql.Append("select top 1 KCU_OwnerId from maxonet_tblKeyCodeUsage where KeyCode_Code = '")
            sql.Append(KeyCode)
            sql.Append("' and KCU_IsActive = '1';")

            Dim OwnerId As String = db.ExecuteScalarWithTransection(sql.ToString)

            ClsLog.Record("เริ่ม InsertParentData")
            Dim IsParentDataExists As Boolean = CheckParentDataExists(KeyCode)
            If Not IsParentDataExists Then
                ClsLog.Record("เริ่ม InsertParentReportAndGraph")
                InsertParentReportAndGraph(KeyCode, IsParent, OwnerId)
            ElseIf Not IsParent Then
                ClsLog.Record("ก่อน UpdateParentReportAndGraph")
                UpdateParentReportAndGraph(KeyCode, OwnerId)
            End If
            If IsParent Then
                CreateParentGraphFolder(KeyCode, tokenId, IsParentDataExists)
            End If

        Catch ex As Exception
            ClsLog.Record("ERROR : = " & ex.ToString)
        End Try


    End Function

    ''' <summary>
    ''' Insert maxOnet_tblParentReport,maxOnet_tblParentGraph
    ''' </summary>
    ''' <param name="KeyCode"></param>
    ''' <param name="IsParent"></param>
    ''' <param name="OwnerId"></param>
    ''' <remarks></remarks>
    Private Sub InsertParentReportAndGraph(KeyCode As String, IsParent As Boolean, OwnerId As String)
        Try
            Dim sql As New StringBuilder

            Dim dt As DataTable = GetPersonalData(KeyCode, False)
            If dt.Rows.Count <> 0 Then
                Dim strPersonalData As String = "ลงทะเบียนเมื่อวันที่ " & dt.Rows(0)("ToDayDate")

                If IsParent = False Then
                    If dt.Rows(0)("KeyCode_Type") = "0" Then
                        strPersonalData = GetClassAndSubject(OwnerId, True)
                    ElseIf dt.Rows(0)("KeyCode_Type") = "1" Then
                        strPersonalData = strPersonalData & GetClassAndSubject(OwnerId, False) & " วิชา ไทย,ภาษาอังกฤษ,สังคม"
                    ElseIf dt.Rows(0)("KeyCode_Type") = "2" Then
                        strPersonalData = strPersonalData & GetClassAndSubject(OwnerId, False) & " วิชา ไทย,ภาษาอังกฤษ,สังคม,คณิตฯ,วิทยาศาสตร์"
                    ElseIf dt.Rows(0)("KeyCode_Type") = "3" Then
                        strPersonalData = strPersonalData & GetClassAndSubject(OwnerId, False) & " วิชา ไทย,ภาษาอังกฤษ,สังคม,คณิตฯ,วิทยาศาสตร์,การงาน,สุขศึกษาฯ,ศิลปะ"
                    End If
                End If

                strPersonalData = strPersonalData & " หมดอายุวันที่ "

                If dt.Rows(0)("KeyCode_ExpireDate") IsNot Nothing Then
                    strPersonalData = strPersonalData & dt.Rows(0)("KeyCode_ExpireDate")
                Else
                    strPersonalData = strPersonalData & dt.Rows(0)("NextYearDate")
                End If

                ClsLog.Record("PersonalData = " & strPersonalData)

                If OwnerId <> "" Then
                    'Insert ก่อนเลย
                    sql.Append("insert into maxonet_tblParentReport select '")
                    sql.Append(KeyCode)
                    sql.Append("','0.00',")
                    sql.Append("N'ผลงาน',N'เข้าทำน้อยมากหรือทำผิดมากค่ะ',")
                    sql.Append("N'สิ่งที่ควรเพิ่มเติม',N'ผู้ปกครองต้องช่วยกระตุ้นให้น้องเข้าทำเพิ่มขึ้นและตั้งใจคิดคำตอบ โดยอาจศึกษาจากคำอธิบายของแต่ละตัวเลือก หรืออ่านหนังสือเรียนเพิ่มเติมค่ะ',")
                    sql.Append("N'ข้อมูลทั่วไป',N'")
                    sql.Append(strPersonalData)
                    sql.Append(" ค่ะ',")
                    sql.Append("N'เส้นสีเขียว - % ความถูกต้องของการทำฝึกฝนเตรียมสอบ',N'")
                    sql.Append(dt.Rows(0)("ToDayDate"))
                    sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                    sql.Append("N'เส้นสีน้ำเงิน - % ความถูกต้องของการทบทวนตามบทเรียน',N'")
                    sql.Append(dt.Rows(0)("ToDayDate"))
                    sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                    sql.Append("N'เส้นสีเขียว - % การเข้าทำฝึกฝนเตรียมสอบ',N'")
                    sql.Append(dt.Rows(0)("ToDayDate"))
                    sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                    sql.Append("N'เส้นสีน้ำเงิน - % การเข้าทบทวนตามบทเรียน',N'")
                    sql.Append(dt.Rows(0)("ToDayDate"))
                    sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                    sql.Append("'1',dbo.GetThaiDate();")
                Else
                    sql.Append("insert into maxonet_tblParentReport select '")
                    sql.Append(KeyCode)
                    sql.Append("','0.00',")
                    sql.Append("N'ผลงาน',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ',")
                    sql.Append("N'สิ่งที่ควรเพิ่มเติม',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ',")
                    sql.Append("N'ข้อมูลทั่วไป',N'")
                    sql.Append(strPersonalData)
                    sql.Append(" ค่ะ',")
                    sql.Append("N'เส้นสีเขียว - % ความถูกต้องของการทำฝึกฝนเตรียมสอบ',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ',")
                    sql.Append("N'เส้นสีน้ำเงิน - % ความถูกต้องของการทบทวนตามบทเรียน',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ',")
                    sql.Append("N'เส้นสีเขียว - % การเข้าทำฝึกฝนเตรียมสอบ',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ',")
                    sql.Append("N'เส้นสีน้ำเงิน - % การเข้าทบทวนตามบทเรียน',N'ลงแอพ max-O-net ในแท็บเลตนักเรียนก่อนนะคะ','1',dbo.GetThaiDate();")
                End If

                sql.Append("insert into maxonet_tblParentGraph select '")
                sql.Append(KeyCode)
                sql.Append("',null,null,null,null,null,null,null,null,null,null,null,null,null,null,")
                sql.Append("null,null,null,null,null,null,null,null,null,null,null,null,null,null,1,dbo.GetThaiDate();")

                db.ExecuteScalarWithTransection(sql.ToString)

                ClsLog.Record("หลัง InsertParentReportAndGraph")
            End If

        Catch ex As Exception
            ClsLog.Record("Error : " & ex.ToString)
        End Try

    End Sub

    Private Sub UpdateParentReportAndGraph(KeyCode As String, OwnerId As String)
        ClsLog.Record("เริ่ม UpdateParentReportAndGraph")
        Try

            Dim dt As DataTable = GetPersonalData(KeyCode, True)

            Dim sql As New StringBuilder

            If dt.Rows.Count <> 0 Then
                Dim strPersonalData As String = "ลงทะเบียนเมื่อวันที่ " & dt.Rows(0)("FirstRegister")


                If dt.Rows(0)("KeyCode_Type") = "0" Then
                    strPersonalData &= GetClassAndSubject(OwnerId, True)
                ElseIf dt.Rows(0)("KeyCode_Type") = "1" Then
                    strPersonalData &= GetClassAndSubject(OwnerId, False) & " วิชา ไทย-ภาษาอังกฤษ-สังคม"
                ElseIf dt.Rows(0)("KeyCode_Type") = "2" Then
                    strPersonalData &= GetClassAndSubject(OwnerId, False) & " วิชา ไทย-ภาษาอังกฤษ-สังคม-คณิตฯ-วิทยาศาสตร์"
                ElseIf dt.Rows(0)("KeyCode_Type") = "3" Then
                    strPersonalData &= GetClassAndSubject(OwnerId, False) & " วิชา ไทย-ภาษาอังกฤษ-สังคม-คณิตฯ-วิทยาศาสตร์-การงาน-สุขศึกษาฯ-ศิลปะ"
                End If

                strPersonalData = strPersonalData & " หมดอายุวันที่ " & dt.Rows(0)("KeyCode_ExpireDate")

                ClsLog.Record("PersonalData = " & strPersonalData)

                'Update
                sql.Append("Update maxonet_tblParentReport set ")
                sql.Append("PR_ResultDetail = 'เข้าทำน้อยมากหรือทำผิดมากค่ะ',")
                sql.Append("PR_RecommendDetail = 'ผู้ปกครองต้องช่วยกระตุ้นให้น้องเข้าทำเพิ่มขึ้นและตั้งใจคิดคำตอบ โดยอาจศึกษาจากคำอธิบายของแต่ละตัวเลือก หรืออ่านหนังสือเรียนเพิ่มเติมค่ะ',")
                sql.Append("PR_PersonalDataDetail = '")
                sql.Append(strPersonalData)
                sql.Append("',")
                sql.Append("PR_QualityDailyDetail = '")
                sql.Append(dt.Rows(0)("ToDayDate"))
                sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                sql.Append("PR_QualityPracticeDetail = '")
                sql.Append(dt.Rows(0)("ToDayDate"))
                sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                sql.Append("PR_QuantityDailyDetail = '")
                sql.Append(dt.Rows(0)("ToDayDate"))
                sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                sql.Append("PR_QuantityPracticeDetail = '")
                sql.Append(dt.Rows(0)("ToDayDate"))
                sql.Append(" ไม่ได้เข้าทำกิจกรรมค่ะ',")
                sql.Append("PR_LastUpdate = dbo.GetThaiDate() where KeyCode_Code = '")
                sql.Append(KeyCode)
                sql.Append("';")
                db.ExecuteScalarWithTransection(sql.ToString)

            End If
        Catch ex As Exception
            ClsLog.Record("Error : " & ex.ToString)
        End Try

    End Sub

    Private Function GetPersonalData(Keycode As String, IsUpdate As Boolean) As DataTable
        Dim sql As New StringBuilder

        If Not IsUpdate Then
            sql.Append("select convert(varchar(10),dateadd(year,543,cast(KeyCode_ExpireDate as date)),103) as KeyCode_ExpireDate,KeyCode_Type,convert(varchar(10),dateadd(year,543,dbo.GetThaiDate()),103) as ToDayDate")
            sql.Append(",convert(varchar(10),dateadd(year,544,dbo.GetThaiDate()),103) as NextYearDate from maxonet_tblKeyCode where KeyCode_Code = '")
            sql.Append(Keycode)
            sql.Append("';")
        Else
            sql.Append("select convert(varchar,dateadd(year,543,cast(KeyCode_DateFirstRegister as Date)),103) as FirstRegister")
            sql.Append(" ,KeyCode_Type,convert(varchar,dateadd(year,543,cast(dbo.GetThaiDate() as Date)),103) as ToDayDate")
            sql.Append(" ,convert(varchar,dateadd(year,543,cast(KeyCode_ExpireDate as Date)),103) as KeyCode_ExpireDate")
            sql.Append(" from maxonet_tblKeyCode  where KeyCode_Code = '")
            sql.Append(Keycode)
            sql.Append("';")

        End If
        Dim dt As DataTable

        If dbLicenseKey.conn.State = ConnectionState.Open Then
            dt = dbLicenseKey.getdataWithTransaction(sql.ToString)
        Else
            dbLicenseKey.OpenWithTransection()
            dt = dbLicenseKey.getdataWithTransaction(sql.ToString)
            dbLicenseKey.CommitTransection()
        End If

        Return dt
    End Function

    Private Sub CreateParentGraphFolder(keyCode As String, tokenId As String, IsParentDataExists As Boolean)
        ClsLog.Record("CreateGraphFolder : TokenId = " & tokenId)
        Dim TokenPath As String = "~/Maxonet/ParentGraph/" & tokenId.ToString().Replace("-", "").ToLower
        Dim ImgQualityPath As String = "~/Images/MaxOnet/Parent/graphnodata.png"
        Dim ImageQuantityPath As String = "~/Images/MaxOnet/Parent/graphnodata.png"

        'ถ้าเคยมีผู้ปกครองลงทะเบียนด้วยรหัสนี้แล้ว ให้ใช้รูปกราฟเดียวกันเพิ่มให้ผู้ปกครองที่ลงทะเบียนมาใหม่
        If IsParentDataExists Then
            Dim sql As String = "select top 1 KCU_Token from maxonet_tblKeyCodeUsage where KeyCode_Code = '" & keyCode & "' and KCU_IsActive = '1'"
            Dim TokenOtherParent As String = db.ExecuteScalarWithTransection(sql.ToString)
            If TokenOtherParent IsNot Nothing Then
                ImgQualityPath = "~/Maxonet/ParentGraph/" & TokenOtherParent.ToString().Replace("-", "").ToLower & "/Quality.png"
                ImageQuantityPath = "~/Maxonet/ParentGraph/" & TokenOtherParent.ToString().Replace("-", "").ToLower & "/Quantity.png"
            End If
        End If

        Dim file As New FileInfo(HttpContext.Current.Server.MapPath(TokenPath))
        file.Directory.Create()

        My.Computer.FileSystem.CopyFile(HttpContext.Current.Server.MapPath(ImgQualityPath), HttpContext.Current.Server.MapPath(TokenPath & "/Quality.png"), UIOption.AllDialogs, UICancelOption.DoNothing)
        My.Computer.FileSystem.CopyFile(HttpContext.Current.Server.MapPath(ImageQuantityPath), HttpContext.Current.Server.MapPath(TokenPath & "/Quantity.png"), UIOption.AllDialogs, UICancelOption.DoNothing)
    End Sub

    Private Function CheckParentDataExists(keycode As String) As Boolean
        Dim sql As String = "select KeyCode_Code from maxonet_tblParentGraph where KeyCode_Code = '" & keycode & "'"
        Dim KeyCodeDB As String = db.ExecuteScalarWithTransection(sql)

        If KeyCodeDB <> "" Then
            ClsLog.Record("CheckParentDataExists : True")
            Return True
        Else
            ClsLog.Record("CheckParentDataExists : False")
            Return False
        End If
    End Function

    Private Function GetClassAndSubject(OwnerId As String, IsSelectSubject As Boolean) As String

        Dim sql As New StringBuilder
        If IsSelectSubject Then
            sql.Append("select ' ชั้น ' + Student_CurrentClass + ' วิชา ' + GroupSubject_ShortName")
            sql.Append(" from maxonet_tblStudentSubject")
            sql.Append(" inner join tblGroupSubject on maxonet_tblStudentSubject.SS_SubjectId = tblGroupSubject.GroupSubject_Id")
            sql.Append(" inner join t360_tblStudent on maxonet_tblStudentSubject.SS_StudentId  = t360_tblStudent.Student_Id")
            sql.Append(" where SS_StudentId  = '")
            sql.Append(OwnerId)
            sql.Append("';")
        Else
            sql.Append("select ' ชั้น ' + Student_CurrentClass from t360_tblStudent where Student_Id  = '")
            sql.Append(OwnerId)
            sql.Append("';")
        End If

        Return db.ExecuteScalarWithTransection(sql.ToString)

    End Function

#End Region

    ''' <summary>
    ''' function ในการ update หลังจากยินยันการเรื่องชั้น วิชาแล้ว
    ''' </summary>
    ''' 
    Public Function SaveStudentSubject() As Boolean
        db.OpenWithTransection()
        Try
            StudentClass = ChangeClassNameFromTablet()
            'insert t360_tblstudentRoom
            RandomRoomAndNumber()
            NewStudentRoomT360()

            'update t360_tblstudent
            UpdateStudentT360WhenConfirm()

            ' เอามาเช็คอีกชั้นนึงเพื่อป้องกันการ insert ซ้ำ
            If CountStudentSubjects() = 0 Then
                'insert maxonet_tblStudentSubject
                AddStudentSubjects()
            End If

            db.CommitTransection()
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
        Return False
    End Function

    Public Function SaveStudentMultiSubjectClass(subjectClass As List(Of MaxonetSubjectClassRegister)) As Boolean
        db.OpenWithTransection()
        Try
            If Not IsStudentExist() Then
                'StudentClass = ChangeClassNameFromTablet()
                'insert t360_tblstudentRoom
                'RandomRoomAndNumber()
                NewStudentRoomT360()
                'update t360_tblstudent
                'UpdateStudentT360WhenConfirm()
            End If
            'add วิชาลง db
            AddMultiSubjectClassStudent(subjectClass, db)
            db.CommitTransection()
            db.CloseConnect()
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
            Return False
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Public Function GetExpiredDate(keycode As String) As String
        dbLicenseKey.OpenWithTransection()

        Dim sql As String = "select KeyCode_ExpireDate from maxonet_tblKeyCode where KeyCode_Code = '" & keycode & "';"
        Dim KeyExpired As String = dbLicenseKey.ExecuteScalarWithTransection(sql)
        Return KeyExpired

    End Function

    Private Sub AddStudentSubjects()
        Dim sql As New StringBuilder()
        For Each subject In SujectList
            sql.Append("INSERT INTO maxonet_tblStudentSubject VALUES(NEWID(),'" & StudentId.CleanSQL & "','" & subject.CleanSQL & "',dbo.GetThaiDate(),1);")
        Next
        db.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    ''' <summary>
    ''' function ในการเพิ่มวิชาและชั้น ของ maxonet
    ''' </summary>
    Private Sub AddMultiSubjectClassStudent(subjectClass As List(Of MaxonetSubjectClassRegister), Optional ByRef InputConn As ClassConnectSql = Nothing)

        Dim dtKeyCode As DataTable
        Dim sql As String = "select KeyCode_Code,kcu_creditamount - UsedAmount as Totaly from(
                            select kcu.KeyCode_Code,kcu.KCU_CreditAmount,case when ss.userCreditAmount is null then 0 else ss.userCreditAmount end as UsedAmount,
                            kcu_Expiredate ,KCU_LastUpdate from maxonet_tblKeyCodeUsage kcu left join (select SS_KeyCode,count(SSId) as userCreditAmount
                            from maxonet_tblStudentSubject where SS_StudentId = '" & StudentId.CleanSQL & "' group by SS_KeyCode) ss
                            on kcu.KeyCode_Code = ss.SS_KeyCode where KCU_ownerId = '" & StudentId.CleanSQL & "')sd where sd.UsedAmount < KCU_CreditAmount and (kcu_Expiredate >= dbo.GetThaiDate() or KCU_ExpireDate is null)
                            order by kcu_lastupdate;"
        dtKeyCode = InputConn.getdataWithTransaction(sql)
        sql = ""
        Dim KeyCode As String = dtKeyCode.Rows(0)("KeyCode_Code")
        Dim KeyCodeCredit As Integer = CInt(dtKeyCode.Rows(0)("Totaly"))
        Dim CreditCounter As Integer = 0
        Dim KeyCodeCounter As Integer = 0

        For Each sc In subjectClass
            For Each classId In sc.ClassId
                If CreditCounter >= KeyCodeCredit Then
                    KeyCodeCounter += 1
                    KeyCode = dtKeyCode.Rows(KeyCodeCounter)("KeyCode_Code")
                    KeyCodeCredit = dtKeyCode.Rows(KeyCodeCounter)("Totaly")
                    CreditCounter = 0
                End If
                sql &= " INSERT INTO maxonet_tblStudentSubject VALUES(NEWID(),'" & StudentId.CleanSQL & "','" & sc.SubjectId.CleanSQL & "',dbo.GetThaiDate(),1,
                      '" & classId & "','" & KeyCode & "');"
                CreditCounter += 1
            Next
        Next
        InputConn.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Function IsStudentExist() As Boolean
        Dim sql As String = "SELECT * FROM t360_tblStudentRoom WHERE Student_Id = '" & StudentId & "';"
        If db.getdataWithTransaction(sql).Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function CountStudentSubjects() As Integer
        Dim sql As String = "SELECT * FROM maxonet_tblStudentSubject WHERE ss_studentId = '" & StudentId.CleanSQL & "';"
        Return db.getdataWithTransaction(sql).Rows.Count
    End Function

    Private Enum EnumDeviceStatus
        NotRegister
        RegisteredWithActive
        RegisteredWithNotActive
    End Enum

    Private Function getSQLDeleteTablet(serial As String) As String
        Dim sql As New StringBuilder()
        sql.Append(" DECLARE @tabletid AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_Tbltablet WHERE Tablet_SerialNumber = '" & serial & "'); ")
        sql.Append(" DELETE t360_tbltablet WHERE Tablet_Id = @tabletid; ")
        sql.Append(" DELETE t360_tbltabletOwner WHERE Tablet_Id = @tabletid;")
        Return sql.ToString()
    End Function

    Private Function getSQLPrepareMaxonet(key As String) As String
        Dim sql As New StringBuilder()
        sql.Append("Declare @keycode AS VARCHAR(12) = '" & key & "';")
        sql.Append(" Declare @stuId AS UNIQUEIDENTIFIER = (SELECT TOP 1 KCU_OwnerId FROM maxonet_tblKeyCodeUsage WHERE KeyCode_Code = @keycode);")
        sql.Append(" DELETE maxonet_tblStudentSubject WHERE SS_StudentId = @stuId;")
        sql.Append(" DELETE t360_tblStudent WHERE Student_Id = @stuId;")
        sql.Append(" DELETE t360_tblStudentRoom WHERE Student_Id = @stuId;")
        sql.Append(" DELETE maxonet_tblKeyCodeUsage WHERE KeyCode_Code = @keycode;")
        sql.Append(" DELETE maxonet_tblParentGraph WHERE KeyCode_Code = @keycode;")
        sql.Append(" DELETE maxonet_tblParentReport WHERE KeyCode_Code = @keycode;")
        sql.Append(" DELETE tblStudentPoint WHERE Student_Id = @stuId;")
        Return sql.ToString()
    End Function

    Public Function PrepareDataAppium(deviceName As String, keyName As String) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = getSQLPrepareMaxonet(keyName)
            db.ExecuteWithTransection(sql)

            Dim sql2 As String = getSQLDeleteTablet(deviceName)
            db.ExecuteWithTransection(sql2)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetResultData() As String
        Try
            ClsLog.Record("1. GetResultData : TokenId = " & TokenId)
            Dim studentId As String = db.ExecuteScalar("SELECT KCU_OwnerId FROM maxonet_tblKeycodeUsage WHERE KCU_Token = '" & TokenId & "'  AND KCU_IsActive = 1;")

            Dim m As New MaxonetParentData()

            If studentId <> "" Then
                ClsLog.Record("2. StudentId = " & studentId)
                'ชื่อนักเรียน
                Dim studentName As String = db.ExecuteScalar("SELECT Student_PrefixName + Student_FirstName + ' ' + Student_LastName AS StudentName 
                                                                FROM t360_tblStudent WHERE Student_Id = '" & studentId & "';")
                m.StudentName = studentName
                ClsLog.Record("3. StudentName = " & studentName)

                'จน.การทำ Quiz
                Dim dtResult As DataTable = GetQuizResultData(studentId)

                If dtResult.Rows.Count = 0 Then
                    ClsLog.Record("case เด็กนักเรียนยังไม่ได้เริ่มใช้งาน")
                    m.NumberOfTimes = "นักเรียนยังไม่เริ่มใช้งาน"
                    m.TotalTime = "นักเรียนยังไม่เริ่มใช้งาน"
                    m.Workings = "นักเรียนยังไม่เริ่มใช้งาน"
                    m.Recommend = "เริ่มต้นการใช้งานโดยการเข้าทำฝึกฝนเตรียมสอบหรือทบทวนตามบทเรียนค่ะ"
                Else
                    m.NumberOfTimes = dtResult.Rows.Count & " ครั้ง"
                    ClsLog.Record("4. NumberOfTimes = " & m.NumberOfTimes)

                    Dim q = From r In dtResult.AsEnumerable()
                            Group r By Subject = r.Field(Of String)("GroupSubject_ShortName") Into total = Group
                            Select New With {
                            Key Subject,
                            .TimeTotal = total.Sum(Function(rr) rr.Field(Of Integer)("TimeTotal")),
                            .TotalScore = CInt(total.Sum(Function(rr) rr.Field(Of Decimal)("TotalScore"))),
                            .FullScore = CInt(total.Sum(Function(rr) rr.Field(Of Decimal)("FullScore")))
                            }

                    Dim minutes As Integer = 0
                    Dim minutesDetail As String = ""
                    Dim totalScore As Integer = 0
                    Dim fullScore As Integer = 0
                    For Each s In q

                        m.Subjects.Add(New SubjectResult With {.Name = s.Subject, .Result = s.TotalScore & "/" & s.FullScore})
                        Dim timeTotal As Integer = s.TimeTotal / 60
                        'ถ้าใช้เวลาทำน้อยกว่า 1 นาที จะแสดงเป็น 0 นาที ซึ่งดูผิดลอจิก จึงเพิ่มเป็นอย่างน้อยเข้าทำ 1 นาที
                        If timeTotal < 1 Then timeTotal = 1
                        minutes += Math.Floor(timeTotal)
                        minutesDetail &= String.Format("{0} {1} นาที,", s.Subject, Math.Floor(timeTotal))
                        totalScore += s.TotalScore
                        fullScore += s.FullScore
                    Next

                    'minutes = minutes / 60 ' หารเพื่อเอาเป็นเวลานาที
                    'ถ้าใช้เวลาทำน้อยกว่า 1 นาที จะแสดงเป็น 0 นาที ซึ่งดูผิดลอจิก จึงเพิ่มเป็นอย่างน้อยเข้าทำ 1 นาที   
                    'If minutes < 1 Then minutes = 1

                    m.TotalResult = Format(((totalScore / fullScore) * 100), "N2")
                    ClsLog.Record("5. TotalResult = " & m.TotalResult)

                    m.TotalTime = String.Format("{0} นาที ({1})", Math.Floor(minutes), minutesDetail.Substring(0, minutesDetail.Length - 1))
                    ClsLog.Record("6. TotalTime = " & m.TotalTime)

                    m.Workings = GetWorkingsTxt(m.TotalResult)
                    ClsLog.Record("7. Workings = " & m.Workings)

                    m.Recommend = GetRecommendTxt(m.TotalResult)
                    ClsLog.Record("8. Recommend = " & m.Recommend)

                    '-------------------------------------
                    Dim q2 = From r In dtResult.AsEnumerable()
                             Group r By Subject = r.Field(Of String)("GroupSubject_ShortName"), IsPracticeMode = r.Field(Of Boolean)("IsPracticeMode") Into total = Group
                             Select New With {
                            Key Subject,
                            .NoOfQuiz = total.Count(),
                            .IsPracticeMode = IsPracticeMode
                            }

                    Dim tempActivityQuantity As Integer = 0
                    Dim tempPracticeQuantity As Integer = 0
                    For Each r In q2
                        If r.IsPracticeMode Then
                            m.PracticesQuiz.Add(New SubjectQuantityResult() With {.Name = r.Subject, .Amount = r.NoOfQuiz})
                            tempPracticeQuantity += r.NoOfQuiz
                        Else
                            m.ActivitiesQuiz.Add(New SubjectQuantityResult() With {.Name = r.Subject, .Amount = r.NoOfQuiz})
                            tempActivityQuantity += r.NoOfQuiz
                        End If
                    Next

                    m.PracticesQuantityPercent = (tempPracticeQuantity / dtResult.Rows.Count) * 100
                    ClsLog.Record("9. PracticesQuantityPercent = " & m.PracticesQuantityPercent)

                    m.ActivitiesQuantityPercent = (tempActivityQuantity / dtResult.Rows.Count) * 100
                    ClsLog.Record("10. ActivitiesQuantityPercent = " & m.ActivitiesQuantityPercent)

                    m.PracticesQuantityPercentTxt = "การเข้าทำฝึกฝน = " & Format(m.PracticesQuantityPercent, "N2") & " %"
                    ClsLog.Record("11. PracticesQuantityPercentTxt = " & m.PracticesQuantityPercentTxt)

                    m.ActivitiesQuantityPercentTxt = "การเข้าทำกิจกรรม = " & Format(m.ActivitiesQuantityPercent, "N2") & " %"
                    ClsLog.Record("12. ActivitiesQuantityPercentTxt = " & m.ActivitiesQuantityPercentTxt)

                    For Each s In m.PracticesQuiz
                        s.Percent = Format((s.Amount / tempPracticeQuantity) * m.PracticesQuantityPercent, "N2")
                    Next
                    For Each s In m.ActivitiesQuiz
                        s.Percent = Format((s.Amount / tempActivityQuantity) * m.ActivitiesQuantityPercent, "N2")
                    Next
                End If

            Else
                ClsLog.Record("case เด็กนักเรียนยังไม่ได้ลงทะเบียน")
                m.StudentName = "ยังไม่ได้ลงทะเบียนนักเรียน"
                m.NumberOfTimes = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
                m.TotalTime = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
                m.Workings = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
                m.Recommend = "ลงทะเบียนเข้าใช้งานได้ที่ Application MaxOnet ค่ะ"
            End If

            Dim js As New JavaScriptSerializer()
            Return js.Serialize(m)
        Catch ex As Exception
            ClsLog.Record("Error!!! = " & ex.ToString())
            Return ex.ToString()
        End Try
    End Function

    Private Function GetWorkingsTxt(totalResult As Decimal) As String
        If totalResult < 34.99 Then
            Return "เข้าทำน้อยมากหรือทำผิดมากค่ะ"
        ElseIf totalResult > 34.99 AndAlso totalResult < 60.0 Then
            Return "ต้องพยายามมากขึ้นนะคะ"
        ElseIf totalResult > 59.99 AndAlso totalResult < 75.0 Then
            Return "ทำได้ดีทีเดียวค่ะ"
        ElseIf totalResult > 74.99 AndAlso totalResult < 85.0 Then
            Return "ทำได้ดีมากค่ะ"
        Else
            Return "ทำได้ยอดเยี่ยมมากค่ะ"
        End If
    End Function

    Private Function GetRecommendTxt(totalResult As Decimal) As String
        If totalResult < 34.99 Then
            Return "ผู้ปกครองต้องช่วยกระตุ้นให้น้องเข้าทำเพิ่มขึ้นและตั้งใจคิดคำตอบ โดยอาจศึกษาจากคำอธิบายของแต่ละตัวเลือก หรืออ่านหนังสือเรียนเพิ่มเติมค่ะ"
        ElseIf totalResult > 34.99 AndAlso totalResult < 60.0 Then
            Return "ทำกิจกรรมให้มากขึ้นค่ะ ค่าคะแนนยังน้อยไปซักนิดนะคะ"
        ElseIf totalResult > 59.99 AndAlso totalResult < 75.0 Then
            Return "หมั่นฝึกฝนเพิ่มเติมอย่างสม่ำเสมอ เดี๋ยวคะแนนจะค่อยๆ ดีขึ้นค่ะ"
        ElseIf totalResult > 74.99 AndAlso totalResult < 85.0 Then
            Return "ฝึกฝนทบทวนข้อที่ยังผิดอีกนิดนะคะ"
        Else
            Return "ลองเข้าทำข้อสอบที่ยากขึ้น เพื่อพัฒนาทักษะให้สูงขึ้นอีกค่ะ"
        End If
    End Function

    Public Function GetQuizResultData(studentId As String) As DataTable
        Dim sql As String
        sql = "SELECT q.Quiz_Id,qs.TimeTotal,q.FullScore,qz.TotalScore,t.GroupSubject_ShortName,q.IsPracticeMode FROM tblQuiz q INNER JOIN "
        sql &= "(SELECT Quiz_Id, SUM(TimeTotal) AS TimeTotal FROM tblQuizScore WHERE Student_Id = '" & studentId & "' GROUP BY Quiz_Id) qs ON qs.Quiz_Id = q.Quiz_Id "
        sql &= " INNER JOIN tblQuizSession qz ON q.Quiz_Id = qz.Quiz_Id "
        sql &= " INNER JOIN tblTestset t On t.Testset_Id = q.Testset_Id "
        sql &= " ORDER BY t.GroupSubject_ShortName;"
        Return db.getdata(sql)
    End Function


    Public Function GetStudentSubjects() As List(Of StudentSubjects)
        Dim studentSubjectsList As New List(Of StudentSubjects)
        Dim dt As DataTable = GetStudentSubjectsData()
        Dim todayDate As String = db.ExecuteScalar("SELECT CONVERT(date, getdate());")
        For Each r In dt.Rows
            Dim studentSubjects As New StudentSubjects With {
                .SubjectId = r("SS_SubjectId").ToString(),
                .SubjectName = r("SS_SubjectId").ToString().SubjectIdToShortThName,
                .TodayDate = todayDate
            }
            studentSubjectsList.Add(studentSubjects)
        Next
        Return studentSubjectsList
    End Function

    Private Function GetStudentSubjectsData() As DataTable
        Return db.getdata("SELECT SS_SubjectId FROM maxonet_tblStudentSubject WHERE SS_StudentId = '" & Me.StudentId & "' GROUP BY SS_SubjectId;")
    End Function

    ''' <summary>
    ''' function get ข้อมูล สำหรับ chart กิจกรรมประจำวัน
    ''' </summary>
    ''' <returns></returns>
    Public Function GetScoreActivityChart() As List(Of ScoreChart)
        Dim condition As String = " AND q.IsHomeWorkMode = 1 "
        Return GetScoreChartData(condition)
    End Function

    Public Function GetScorePracticeChart() As List(Of ScoreChart)
        Dim condition As String = " AND q.IsPracticeMode = 1 "
        Return GetScoreChartData(condition)
    End Function

    Private Function GetScoreChartData(condition As String)
        Dim scoreChart As New List(Of ScoreChart)
        ' หาวิชาที่นักเรียนลงทะเบียนไว้
        Dim dtSubjects As DataTable = GetStudentSubjectsData()
        For Each subject In dtSubjects.Rows
            ' หาคะแนนตามช่วงเวลา 7 วัน
            Dim s As New ScoreChart
            s.SubjectName = subject("SS_SubjectId").ToString().SubjectIdToShortThName
            Dim dt As DataTable = GetDataActivity(subject("SS_SubjectId").ToString(), condition)
            Dim i As Integer = 0
            For Each r In dt.Rows
                Dim day As String = If((i Mod 2 = 0), " ", r("DNAME"))
                Dim score = r("TotalScore")
                Dim t
                t = New With {Key .score = CInt(score), .day = r("DNAME")}
                s.Scores.Add(t)
                i = i + 1
            Next
            scoreChart.Add(s)
        Next
        Return scoreChart
    End Function

    Private Function GetDataActivity(GroupSubjectId As String, condition As String) As DataTable
        Dim sql As New StringBuilder
        sql.Append("  DECLARE @start DATE,@end DATE;SELECT @start = GETDATE() - 7, @end = GETDATE(); ")
        sql.Append("  ;With n As (Select TOP (DATEDIFF(DAY,@start,@end) + 1) ")
        sql.Append(" n = ROW_NUMBER() OVER (ORDER BY [object_id]) FROM sys.all_objects) ")
        sql.Append("  SELECT DNAME,GroupSubject_Id,t1.GroupSubject_Name,case when TotalScore is null then 0 else TotalScore end as TotalScore,StartTime,t2.GroupSubject_Name FROM ( ")
        sql.Append(" SELECT CONVERT(VARCHAR(10), DATEADD(DAY, n-1,@start), 120) AS DNAME,g.GroupSubject_Id,g.GroupSubject_Name FROM n, ")
        sql.Append(" tblGroupSubject g WHERE GroupSubject_Id = '" & GroupSubjectId & "') t1 ")
        sql.Append(" LEFT JOIN ( ")
        sql.Append(" Select SUM(a.TotalScore) As TotalScore,a.StartTime,a.GroupSubject_Name FROM ( ")
        sql.Append("  SELECT qz.TotalScore,CONVERT(VARCHAR(10), q.StartTime, 120) as StartTime,t.GroupSubject_Name ")
        sql.Append("  FROM tblQuizScore qs INNER JOIN tblQuiz q ON qs.Quiz_Id = q.Quiz_Id INNER JOIN tblQuizSession qz ON q.Quiz_Id = qz.Quiz_Id ")
        sql.Append("  INNER JOIN tblTestset t ON t.Testset_Id = q.Testset_Id INNER JOIN tblGroupSubject g ON t.GroupSubject_Name = g.GroupSubject_Name ")
        sql.Append("  WHERE Student_Id = '" & Me.StudentId & "' " & condition & " ")
        sql.Append("  GROUP BY q.Quiz_Id,qz.TotalScore,q.StartTime,t.GroupSubject_Name) a ")
        sql.Append("  GROUP BY a.StartTime, a.GroupSubject_Name) t2 On t1.DNAME = t2.StartTime And t1.GroupSubject_Name = t2.GroupSubject_Name ")
        sql.Append("  ORDER BY t1.DNAME ")

        Log.Record(Log.LogType.BrowserAgentNotChrome, "GetDataActivity", 0, Me.StudentId)
        Log.Record(Log.LogType.BrowserAgentNotChrome, sql.ToString(), 0, Me.StudentId)
        Return db.getdata(sql.ToString())
    End Function

    Public Function GetAllScoreData() As List(Of Object)
        Dim dt As DataTable = GetAllScore()
        Dim data As New List(Of Object)
        For Each r In dt.Rows
            Dim score = If((r("TotalScore") Is DBNull.Value), 0, r("TotalScore"))
            Dim subject = New With {Key .subjectName = r("SS_SubjectId").ToString().SubjectIdToShortThName, .score = CInt(score)}
            data.Add(subject)
        Next
        Return data
    End Function

    Private Function GetAllScore() As DataTable
        Dim sql As New StringBuilder
        sql.Append(" SELECT * FROM ( ")
        sql.Append(" Select DISTINCT(SS_SubjectId) FROM maxonet_tblStudentSubject ")
        sql.Append(" WHERE SS_StudentId = '" & Me.StudentId & "' ")
        sql.Append(") t1 LEFT JOIN ( ")
        sql.Append("  SELECT SUM(qz.TotalScore) AS TotalScore,g.GroupSubject_Id FROM tblquiz q INNER JOIN tblQuizSession qz ON q.Quiz_Id = qz.Quiz_Id ")
        sql.Append("  INNER JOIN tblTestset t ON q.Testset_Id = t.Testset_Id ")
        sql.Append("  INNER JOIN tblGroupSubject g ON t.GroupSubject_Name = g.GroupSubject_Name ")
        sql.Append("  WHERE q.user_id = '" & Me.StudentId & "'  ")
        sql.Append("  GROUP BY g.GroupSubject_Id ")
        sql.Append(") t2 On t1.SS_SubjectId = t2.GroupSubject_Id ")
        Return db.getdata(sql.ToString())
    End Function

    ''' <summary>
    ''' function สำหรับ get data ปริมาณการทำกิจกรรมประจำวัน
    ''' </summary>
    ''' <returns></returns>
    Public Function GetQuantityActivityChartData() As List(Of Object)
        Return GetQuantityChartData("q.IsHomeWorkMode = 1")
    End Function

    ''' <summary>
    ''' function สำหรับ get data ปริมาณการทำฝึกฝน
    ''' </summary>
    ''' <returns></returns>
    Public Function GetQuantityPracticeChartData() As List(Of Object)
        Return GetQuantityChartData("q.IsPracticeMode = 1")
    End Function

    Private Function GetQuantityChartData(condition As String) As List(Of Object)
        Dim dt As DataTable = GetQuantityActivity(condition)
        Dim data As New List(Of Object)
        For Each r In dt.Rows
            Dim amount = If((r("amount") Is DBNull.Value), 0, r("amount"))
            Dim subject = New With {Key .subjectName = r("SS_SubjectId").ToString().SubjectIdToShortThName, .amount = CInt(amount)}
            data.Add(subject)
        Next
        Return data
    End Function

    Private Function GetQuantityActivity(condition As String) As DataTable
        Dim sql As New StringBuilder
        sql.Append(" Select * FROM ( ")
        sql.Append(" Select DISTINCT(SS_SubjectId) FROM maxonet_tblStudentSubject  ")
        sql.Append(" WHERE SS_StudentId = '" & Me.StudentId & "' ")
        sql.Append(") t1 LEFT JOIN ")
        sql.Append(" (SELECT g.GroupSubject_Id,COUNT(g.GroupSubject_Id) AS amount FROM tblQuiz q  ")
        sql.Append(" INNER JOIN tblTestset t ON q.TestSet_Id = t.TestSet_Id ")
        sql.Append(" INNER JOIN tblGroupSubject g ON t.GroupSubject_Name = g.GroupSubject_Name ")
        sql.Append(" WHERE " & condition & " AND q.User_Id = '" & Me.StudentId & "' ")
        sql.Append(" GROUP BY g.GroupSubject_Id) t2 ON t1.SS_SubjectId = t2.GroupSubject_Id ")
        Return db.getdata(sql.ToString())
    End Function

    Public Function AddCreditMaxonet() As Integer
        Try
            ClsLog.Record("AddCreditMaxonet")
            'กระบวนการ Add Credit ควรจะ Insert เพิ่มที่ MaxOnet_tblKeyCodeUsage เพื่อบอกได้ว่า เครื่องนี้ได้เติม Credit ไปกี่ครั้ง วันที่เท่าไหร่บ้าง และเพื่อทำให้แต่ละ Credit มีวันหมดอายุถูกต้อง 

            'Check ว่า Key ถูกหรือไม่
            If IsValidKey() = -1 Then Return -1

            'check ว่า key นี้ถูกใช้หรือยัง
            If IsKeyCodeUsed() = -2 Then Return -2

            'check วันหมดอายุ
            If CheckIsExpiredate() = -3 Then Return -3

            dbLicenseKey.OpenWithTransection()

            db.OpenWithTransection()

            'update ให้เป็น code ที่ใช้แล้ว
            Dim sql As String = " UPDATE maxonet_tblKeyCode SET KeyCode_DateFirstRegister = dbo.GetThaiDate(),KeyCode_DateLastRegister = dbo.GetThaiDate(),
                                    KeyCode_LastUpdate = dbo.GetThaiDate(),KeyCode_ExpireDate = DATEADD(DAY,KeyCode_EndDateAmount,dbo.GetThaiDate()),
                                    KeyCode_RegistrationCounter = KeyCode_RegistrationCounter + 1,KeyCode_IsActive = 1 WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "'; "
            dbLicenseKey.ExecuteScalarWithTransection(sql)

            Dim dtKeyCodeDetail As DataTable
            sql = "SELECT KeyCode_EndDateAmount,KeyCode_LimitExam,KeyCode_Credit FROM maxonet_tblKeyCode WHERE KeyCode_Code = '" & KeyCode.CleanSQL & "';"
            dtKeyCodeDetail = dbLicenseKey.getdataWithTransaction(sql)

            Dim LimitExam As String = dtKeyCodeDetail.Rows(0)("KeyCode_LimitExam").ToString
            If LimitExam = "" Or LimitExam = "0" Then
                LimitExam = "null"
            End If
            ClsLog.Record("LimitExam = " & LimitExam)

            Dim EndDateAmount As String = dtKeyCodeDetail.Rows(0)("KeyCode_EndDateAmount").ToString
            If EndDateAmount = "" Or EndDateAmount = "0" Then
                EndDateAmount = "null"
            Else
                EndDateAmount = "dbo.GetThaiDate() + " & EndDateAmount
            End If
            ClsLog.Record("EndDateAmount = " & EndDateAmount)

            Dim KeycodeCredit As String = dtKeyCodeDetail.Rows(0)("KeyCode_Credit").ToString
            ClsLog.Record("KeycodeCredit = " & KeycodeCredit)

            Dim kcuType As String = "3"
            ClsLog.Record("KCU_Type = " & kcuType)

            sql = "INSERT INTO maxonet_tblKeyCodeUsage(KCU_Id,KeyCode_Code,KCU_Token,KCU_DeviceId,KCU_IsActive,KCU_LastUpdate,KCU_Type,KCU_DateFirstRegister,
                            KCU_OwnerId,KCU_LimitExamAmount,KCU_ExpireDate,KCU_CreditAmount) VALUES(NEWID(),'" & KeyCode.CleanSQL & "','" & TokenId & "',
                            '" & DeviceId.CleanSQL & "','1',dbo.GetThaiDate(),'" & kcuType & "',dbo.GetThaiDate(),'" & StudentId & "',
                            " & LimitExam & "," & EndDateAmount & ",'" & KeycodeCredit & "');"
            ClsLog.Record("sql = " & sql)


            db.ExecuteScalarWithTransection(sql)

            dbLicenseKey.CommitTransection()
            db.CommitTransection()
            ClsLog.Record("1")
            Return 1
        Catch ex As Exception
            dbLicenseKey.RollbackTransection()
            ClsLog.Record("-4")
            ClsLog.Record(ex.ToString)
            Return -4
        End Try
    End Function
    Private Function IsValidKey() As Integer
        Dim sql As String = "SELECT * FROM maxonet_tblKeyCode WHERE KeyCode_Username = '" & Me.UserName & "' AND KeyCode_Code = '" & Me.KeyCode & "';"
        If dbLicenseKey.getdata(sql).Rows.Count > 0 Then
            Return 1
        End If
        Return -1
    End Function
    Private Function IsKeyCodeUsed() As Integer
        Dim sql As String = "SELECT * FROM maxonet_tblKeyCode WHERE KeyCode_Username = '" & Me.UserName & "' AND KeyCode_Code = '" & Me.KeyCode & "' 
                                AND KeyCode_DateFirstRegister IS NULL;"
        If dbLicenseKey.getdata(sql).Rows.Count > 0 Then
            Return 1
        End If
        Return -2
    End Function

    Private Function CheckIsExpiredate() As Integer
        Dim sql As String = "select * from maxonet_tblKeyCode where KeyCode_Code = '" & Me.KeyCode & "' and KeyCode_Username = '" & Me.UserName & "' 
                                and (KeyCode_ExpireDate > dbo.GetThaiDate() or KeyCode_ExpireDate is null)"
        If dbLicenseKey.getdata(sql).Rows.Count > 0 Then
            Return 1
        End If
        Return -3
    End Function

    Public Function GetRoomId(ClassName As String, RoomName As String) As String
        Dim SchoolCode As String = HttpContext.Current.Application("DefaultSchoolCode").ToString
        RoomName = "/" & RoomName
        Dim sql As String = "select Room_Id from t360_tblRoom where School_Code = '" & SchoolCode & "' and Class_Name = '" & ClassName & "' and Room_Name = '" & RoomName & "' and Room_IsActive = 1"

        Dim RoomId As String = db.ExecuteScalar(sql)
        If RoomId = "" Then
            Dim NewRoomId = Guid.NewGuid()
            sql = "insert into t360_tblRoom(School_Code,Class_Name,Room_Name,Room_Id,Room_IsActive,LastUpdate) values ('" & SchoolCode & "','" & ClassName & "','" & RoomName & "','" & NewRoomId.ToString & "'
                    ,1,dbo.GetThaiDate());"
            db.Execute(sql)
            RoomId = NewRoomId.ToString
        End If
        Return RoomId
    End Function

#Region "ReportParent"
    Public Function GetChartActivityByFilter(subjects As String, startDate As String, endDate As String) As List(Of ScoreChart)
        Dim condition As String = "AND q.IsHomeWorkMode = 1"
        Return GetChartByFilter(condition, subjects, startDate, endDate)
    End Function

    Public Function GetChartPracticeByFilter(subjects As String, startDate As String, endDate As String) As List(Of ScoreChart)
        Dim condition As String = "AND q.IsPracticeMode = 1"
        Return GetChartByFilter(condition, subjects, startDate, endDate)
    End Function

    Private Function GetChartByFilter(condition As String, subjects As String, startDate As String, endDate As String)

        Dim scoreChart As New List(Of ScoreChart)
        Dim dt As DataTable = GetScoreActivityDataByFilter(subjects, startDate, endDate, condition)
        Dim DoQuizAmount As Integer = dt.Rows.Count

        If DoQuizAmount < 60 Then
            Dim s As New ScoreChart
            s.SubjectName = subjects.SubjectIdToShortThName
            s.DateRangeAmount = DiffSelectedDate(startDate, endDate)
            s.DoQuizAmount = DoQuizAmount.ToString
            For Each r In dt.Rows
                Dim score = r("TotalScore")
                Dim t = New With {Key .score = CInt(score), .day = r("DNAME")}
                s.Scores.Add(t)
            Next
            scoreChart.Add(s)
        Else
            Dim ArrType() As String = {"Max", "Mean", "Min", "LastScore"}
            Dim FieldName() As String = {"MaxTotalScore", "MeanScore", "MinTotalScore", "LastScoreMonth"}

            dt = GetScoreDataByFilterMore60Days(subjects, startDate, endDate, condition)

            For i = 0 To ArrType.Count - 1
                Dim s As New ScoreChart
                s.SubjectName = ArrType(i)
                s.DateRangeAmount = DiffSelectedDate(startDate, endDate)
                s.DoQuizAmount = DoQuizAmount.ToString
                For Each eachRow In dt.Rows
                    Dim score = eachRow(FieldName(i))
                    Dim t = New With {Key .score = CInt(score), .day = eachRow("DNAME")}
                    s.Scores.Add(t)
                Next
                scoreChart.Add(s)
            Next
        End If

        Return scoreChart
    End Function

    Private Function GetScoreActivityDataByFilter(GroupSubjectId As String, startDate As String, endDate As String, condition As String) As DataTable

        'v.1 Decha
        'Dim sql As New StringBuilder
        'sql.Append("  DECLARE @start DATE,@end DATE;SELECT @start = CONVERT(DATE,'" & startDate & "'), @end = CONVERT(DATE,'" & endDate & "'); ")
        'sql.Append("  ;With n As (Select TOP (DATEDIFF(DAY,@start,@end) + 1) ")
        'sql.Append(" n = ROW_NUMBER() OVER (ORDER BY [object_id]) FROM sys.all_objects) ")
        'sql.Append("  SELECT * FROM ( ")
        'sql.Append(" SELECT CONVERT(VARCHAR(10), DATEADD(DAY, n-1,@start), 120) AS DNAME,g.GroupSubject_Id,g.GroupSubject_Name FROM n, ")
        'sql.Append(" tblGroupSubject g WHERE GroupSubject_Id = '" & GroupSubjectId & "') t1 ")
        'sql.Append(" INNER JOIN ( ")
        'sql.Append(" Select SUM(a.TotalScore) As TotalScore,a.StartTime,a.GroupSubject_Name FROM ( ")
        'sql.Append("  SELECT qz.TotalScore,CONVERT(VARCHAR(10), q.StartTime, 120) as StartTime,t.GroupSubject_Name ")
        'sql.Append("  FROM tblQuizScore qs INNER JOIN tblQuiz q ON qs.Quiz_Id = q.Quiz_Id INNER JOIN tblQuizSession qz ON q.Quiz_Id = qz.Quiz_Id ")
        'sql.Append("  INNER JOIN tblTestset t ON t.Testset_Id = q.Testset_Id INNER JOIN tblGroupSubject g ON t.GroupSubject_Name = g.GroupSubject_Name ")
        'sql.Append("  WHERE Student_Id = '" & Me.StudentId & "' " & condition)
        'sql.Append("  GROUP BY q.Quiz_Id,qz.TotalScore,q.StartTime,t.GroupSubject_Name) a ")
        'sql.Append("  GROUP BY a.StartTime, a.GroupSubject_Name) t2 On t1.DNAME = t2.StartTime And t1.GroupSubject_Name = t2.GroupSubject_Name ")
        'sql.Append("  ORDER BY t1.DNAME ")
        'Log.Record(Log.LogType.BrowserAgentNotChrome, sql.ToString, False, Me.StudentId)
        'Return db.getdata(sql.ToString())

        'V.2 ไหม ถ้าเกิน 60 วันให้ทำเป็นเดือน
        'Dim sql As String
        'sql = " select a.DNAME,sum(TotalScore) as totalScore,a.GroupSubject_Id,a.GroupSubject_Name from ( " &
        '      " select distinct CONVERT(CHAR(10),q.starttime, 120) as DNAME,qus.TotalScore,g.GroupSubject_Id,g.GroupSubject_Name " &
        '      " from tblquiz q inner join tblQuizSession qus on q.quiz_id = qus.Quiz_Id " &
        '      " inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id " &
        '      " inner join  tblTestSetQuestionSet tsqs on tsqs.TestSet_Id = t.TestSet_Id " &
        '      " inner join tblquestionset qs on tsqs.QSet_Id  = qs.QSet_Id " &
        '      " inner join tblquestioncategory qc on qs.QCategory_Id = qc.QCategory_Id " &
        '      " inner join tblBook b on qc.Book_Id = b.BookGroup_Id " &
        '      " inner join tblGroupSubject g on b.groupsubject_id = g.GroupSubject_Id " &
        '      " where g.GroupSubject_Id = '" & GroupSubjectId & "' " & condition & " and q.user_Id = '" & Me.StudentId & "' " &
        '      " And CONVERT(CHAR(10),q.starttime, 120) between CONVERT(date,'" & startDate & "') and CONVERT(date,'" & endDate & "'))a " &
        '      " Group by a.DNAME,a.GroupSubject_Id,a.GroupSubject_Name order by DNAME"

        'Dim dtScore As DataTable = db.getdata(sql.ToString())

        'If dtScore.Rows.Count() > 60 Then
        '    sql = " select cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar) as DNAME ,max(TotalScore) as totalScore,a.GroupSubject_Id,a.GroupSubject_Name from ( " &
        '          " select distinct CONVERT(CHAR(10),q.starttime, 120) as DNAME,qus.TotalScore,g.GroupSubject_Id,g.GroupSubject_Name " &
        '          " from tblquiz q inner join tblQuizSession qus on q.quiz_id = qus.Quiz_Id " &
        '          " inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id " &
        '          " inner join  tblTestSetQuestionSet tsqs on tsqs.TestSet_Id = t.TestSet_Id " &
        '          " inner join tblquestionset qs on tsqs.QSet_Id  = qs.QSet_Id " &
        '          " inner join tblquestioncategory qc on qs.QCategory_Id = qc.QCategory_Id " &
        '          " inner join tblBook b on qc.Book_Id = b.BookGroup_Id " &
        '          " inner join tblGroupSubject g on b.groupsubject_id = g.GroupSubject_Id " &
        '          " where g.GroupSubject_Id = '" & GroupSubjectId & "' " & condition & " and q.user_Id = '" & Me.StudentId & "' " &
        '          " And CONVERT(CHAR(10),q.starttime, 120) between CONVERT(date,'" & startDate & "') and CONVERT(date,'" & endDate & "'))a " &
        '          " Group by cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar),a.GroupSubject_Id,a.GroupSubject_Name " &
        '          " order by cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar)"
        '    dtScore.Clear()
        '    dtScore = db.getdata(sql.ToString())
        'End If

        'Log.Record(Log.LogType.BrowserAgentNotChrome, sql.ToString, False, Me.StudentId)
        'Return dtScore

        'V.3 ไหม เกิน 60 วันทำ Graph Min,Max,Mean, LastScore
        Dim sql As String

        sql = " select a.DNAME,sum(TotalScore) as totalScore,a.GroupSubject_Id,a.GroupSubject_Name from ( " &
                  " select distinct CONVERT(CHAR(10),q.starttime, 120) as DNAME,qus.TotalScore,g.GroupSubject_Id,g.GroupSubject_Name " &
                  " from tblquiz q inner join tblQuizSession qus on q.quiz_id = qus.Quiz_Id " &
                  " inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id " &
                  " inner join  tblTestSetQuestionSet tsqs on tsqs.TestSet_Id = t.TestSet_Id " &
                  " inner join tblquestionset qs on tsqs.QSet_Id  = qs.QSet_Id " &
                  " inner join tblquestioncategory qc on qs.QCategory_Id = qc.QCategory_Id " &
                  " inner join tblBook b on qc.Book_Id = b.BookGroup_Id " &
                  " inner join tblGroupSubject g on b.groupsubject_id = g.GroupSubject_Id " &
                  " where g.GroupSubject_Id = '" & GroupSubjectId & "' " & condition & " and q.user_Id = '" & Me.StudentId & "' " &
                  " And CONVERT(CHAR(10),q.starttime, 120) between CONVERT(date,'" & startDate & "') and CONVERT(date,'" & endDate & "'))a " &
                  " Group by a.DNAME,a.GroupSubject_Id,a.GroupSubject_Name order by DNAME"

        Dim dtscore As DataTable = db.getdata(sql)
        Log.Record(Log.LogType.BrowserAgentNotChrome, sql.ToString, False, Me.StudentId)
        Return dtscore

    End Function

    Private Function GetScoreDataByFilterMore60Days(GroupSubjectId As String, startDate As String, endDate As String, condition As String) As DataTable

        Dim sql As String

        'v.1 มี 0 ข้างหน้า 60 วัน แสดง Max Mean Min LastScore

        sql = " select c.DNAME,C.MaxTotalScore,C.MinTotalScore,c.MeanScore,b.TotalScore as LastScoreMonth from( " &
                  " select cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar) as DNAME , " &
                  " max(TotalScore) As MaxTotalScore,min(TotalScore) As MinTotalScore,sum(totalScore) / count(TotalScore) As MeanScore,max(Dname) As LastDate ,a.GroupSubject_ShortName " &
                  " from (Select distinct CONVERT(Char(10),q.starttime, 120) As DNAME,qus.TotalScore As TotalScore,t.GroupSubject_ShortName " &
                  " from tblquiz q inner join tblQuizSession qus on q.quiz_id = qus.Quiz_Id " &
                  " inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id  inner join  tblTestSetQuestionSet tsqs on tsqs.TestSet_Id = t.TestSet_Id " &
                  " inner join tblquestionset qs on tsqs.QSet_Id  = qs.QSet_Id  inner join tblquestioncategory qc on qs.QCategory_Id = qc.QCategory_Id " &
                  " inner join tblBook b on qc.Book_Id = b.BookGroup_Id  inner join tblGroupSubject g on b.groupsubject_id = g.GroupSubject_Id " &
                  " where g.GroupSubject_Id = '" & GroupSubjectId & "' " & condition &
                  " and q.user_Id = '" & Me.StudentId & "' " &
                  " And CONVERT(CHAR(10),q.starttime, 120) between CONVERT(date,'" & startDate & "') and CONVERT(date,'" & endDate & "') )a " &
                  " Group by cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar),DATEPART(MONTH, DNAME),a.GroupSubject_ShortName " &
                  " )c inner join ( select max(qs.totalScore) as totalScore, CONVERT(CHAR(10),q.starttime, 120) as SName, t.GroupSubject_ShortName " &
                  " from tblQuiz q inner join tblQuizSession qs on q.Quiz_Id = qs.Quiz_Id " &
                  " inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id " &
                  " where q.user_Id = '" & Me.StudentId & "' " & condition &
                  " group by CONVERT(CHAR(10),q.starttime, 120), t.GroupSubject_ShortName " &
                  " )b on c.LastDate = b.SName And c.GroupSubject_ShortName = b.GroupSubject_ShortName " &
                  " group by c.DNAME,C.MaxTotalScore,C.MinTotalScore,c.MeanScore,b.TotalScore order by max(Sname) "

        'v.2 มี 0 ข้างหน้า

        '       sql = " select '' as DNAME,0 as MaxTotalScore,0 as MinTotalScore,0 as MeanScore,0 as LastScoreMonth,getdate() - 6000 as LastDate
        '               Union
        '               select c.DNAME,C.MaxTotalScore,C.MinTotalScore,c.MeanScore,b.TotalScore as LastScoreMonth,LastDate 
        '               from(  
        '            select cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar) as DNAME , max(TotalScore) As MaxTotalScore,
        '            min(TotalScore) As MinTotalScore,sum(totalScore) / count(TotalScore) As MeanScore,max(Dname) As LastDate,a.GroupSubject_ShortName  
        '        from (
        '	Select distinct CONVERT(Char(10),q.starttime, 120) As DNAME,qus.TotalScore As TotalScore,t.GroupSubject_ShortName  
        '	from tblquiz q inner join tblQuizSession qus on q.quiz_id = qus.Quiz_Id  
        '	inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id  inner join  tblTestSetQuestionSet tsqs on tsqs.TestSet_Id = t.TestSet_Id  
        '	inner join tblquestionset qs on tsqs.QSet_Id  = qs.QSet_Id inner join tblquestioncategory qc on qs.QCategory_Id = qc.QCategory_Id  
        '	inner join tblBook b on qc.Book_Id = b.BookGroup_Id  
        '	inner join tblGroupSubject g on b.groupsubject_id = g.GroupSubject_Id  
        '	where g.GroupSubject_Id = '" & GroupSubjectId & "' " & condition & " and q.user_Id = '" & Me.StudentId & "'  
        '	And CONVERT(CHAR(10),q.starttime, 120) between CONVERT(date,'" & startDate & "') and CONVERT(date,'" & endDate & "') 
        '	)a  
        '	Group by cast(DATENAME(MONTH, DNAME) as varchar(3)) + '/' + cast(DATEPART(YEAR, DNAME) as varchar),DATEPART(MONTH, DNAME),a.GroupSubject_ShortName  
        ')c inner join (
        '	select max(qs.totalScore) as totalScore, CONVERT(CHAR(10),q.starttime, 120) as SName,t.GroupSubject_ShortName  
        '	from tblQuiz q inner join tblQuizSession qs on q.Quiz_Id = qs.Quiz_Id  
        '	inner join tblTestSet t on q.TestSet_Id = t.TestSet_Id  
        '	where q.user_Id = '" & Me.StudentId & "' " & condition & "
        '	group by CONVERT(CHAR(10),q.starttime, 120), t.GroupSubject_ShortName  
        '	)b 
        '  on c.LastDate = b.SName And c.GroupSubject_ShortName = b.GroupSubject_ShortName  
        '     Group by c.DNAME,C.MaxTotalScore,C.MinTotalScore,c.MeanScore,b.TotalScore,LastDate 
        'order by LastDate "

        Dim dtscore As DataTable = db.getdata(sql)
        Log.Record(Log.LogType.BrowserAgentNotChrome, sql.ToString, False, Me.StudentId)
        Return dtscore
    End Function

    Private Function DiffSelectedDate(startDate As String, endDate As String) As String
        Dim sql As String
        sql = "SELECT DATEDIFF(DAY,'" & startDate & "','" & endDate & "') + 1"
        Dim DateAmount As String = db.ExecuteScalar(sql)
        Return DateAmount
    End Function
#End Region

    Public Function IsLimitExam(StudentId As String) As String
        Dim sql As String = "select kcu_limitexamAmount from maxonet_tblKeyCodeUsage where KCU_OwnerId = '" & StudentId & "';"

        Dim LimitExam As String = db.ExecuteScalar(sql)
        Return LimitExam

    End Function

    Public Function UpdatePCType(TokenId As String) As Boolean
        Dim sql As String = "Update maxonet_tblKeyCodeUsage set kcu_Type = 4 where KCU_Token = '" & TokenId & "';"
        Dim db As New ClassConnectSql()
        db.ExecuteScalar(sql)
        Return True
    End Function

#Region "ReportTeacher"
    Public Function GetREprtdata() As DataTable

    End Function

#End Region
End Class

Public Enum MaxOnetRegisterType
    student = 0
    parent = 1
End Enum

Public Enum MaxOnetExamMode
    'ทบทวนตามบทเรียน
    Practice = 0
    'ฝึกฝนเตรียมสอบ
    Quiz = 1
End Enum

Public Class ScoreChart
    Public DoQuizAmount As String = ""
    Public DateRangeAmount As String = ""
    Public SubjectName As String
    Public Scores As New List(Of Object)
End Class

Public Class StudentSubjects
    Public SubjectId As String
    Public SubjectName As String
    Public TodayDate As String
End Class