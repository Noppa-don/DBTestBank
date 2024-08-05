Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports KnowledgeUtils.Json.Serialization
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports KnowledgeUtils.IO
Imports QuickTest.install

Public Class ClassDroidPad
    Dim _DB As ClsConnect
    Dim Redist As New KnowledgeUtils.RedisStore()
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Dim file As New ManageFile
    Dim PathTextFile As String = ConfigurationManager.AppSettings("PathTextFileDetailLog")

#Region "Register Tablet State1"
    ''' <summary>
    ''' ลงทะเบียนกับ Tablet เมื่อ Tablet กดลงทะเบียนจะเข้ามาที่ Function นี้เพื่อเช็ครหัสผ่าน
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="SchoolID">รหัสโรงเรียน</param>
    ''' <param name="SchoolPassword">รหัสผ่าน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetRegistrationInfo(ByVal DeviceUniqueID As String, ByVal SchoolID As String, ByVal SchoolPassword As String) As String

        HttpContext.Current.Session("_SchoolID") = ""
        If DeviceUniqueID <> "" And SchoolID <> "" And SchoolPassword <> "" Then

            Dim Returnvalue As String = "{""Param"": {""RegistrationInfo"" : ""WRONGPASSWORD""}}"
            Dim sql As String = ""
            Dim CheckPassword As Boolean
            CheckPassword = CheckSchoolPassword(SchoolID, SchoolPassword)
            If CheckPassword = False Then
                Return Returnvalue
            End If

            sql = " SELECT Count(*) FROM dbo.t360_tblTablet WHERE School_Code <> '" & _DB.CleanString(SchoolID.Trim()) & "' AND Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' AND Tablet_IsActive = '1' "
            Dim CheckSchoolId As Integer = _DB.ExecuteScalar(sql)

            ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo : sql = " & sql)
            ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo : CheckSchoolId = " & CheckSchoolId)

            'ถ้าเท่ากับ 1 หรือเกิน 1 แสดงว่า tablet เครื่องนี้เคยลงทะเบียนไปแล้ว
            If CheckSchoolId >= 1 Then
                Returnvalue = "{""Param"": {""RegistrationInfo"" : """ & SchoolID & """}}"

                ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo : Returnvalue = " & Returnvalue)

                Return Returnvalue
            End If

            Try
                'Password ถูกและไม่เคยลงทะเบียน
                sql = " SELECT Count(*) FROM dbo.t360_tblTablet WHERE School_Code = '" & _DB.CleanString(SchoolID.Trim()) & "' AND Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' AND Tablet_IsActive = '1' "
                'ตัวแปรที่เอาไว้เช็คว่ารหัสโรงเรียน/รหัสผ่าน ที่กรอกเข้ามาถูกต้องตรงกับใน DB หรือเปล่า
                Dim CheckRegistered As Integer = _DB.ExecuteScalar(sql)

                ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo(ไม่เคยลงทะเบียน) : sql = " & sql)
                ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo(ไม่เคยลงทะเบียน) : CheckRegistered = " & CheckRegistered)


                If CheckRegistered = 0 Then
                    sql = " INSERT INTO dbo.t360_tblTablet " &
                          " (School_Code , Tablet_SerialNumber,Tablet_IsOwner,Tablet_Status,Tablet_IsActive) " &
                          " VALUES('" & SchoolID & "','" & DeviceUniqueID & "','1','1','1') "
                    _DB.Execute(sql)

                    ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo(ไม่เคยลงทะเบียน) : sql = " & sql)

                Else 'Password ถูกและลงทะเบียนกับ รร. นี้อยู่แล้ว
                    'ไม่ต้องทำอะไร

                    'เพิ่ม ให้สามารถรู้ได้ว่าเป็นเครื่องที่ล้างแล้วลงใหม่ สำหรับเครื่องที่เป็นห้องแล็ปหรือสำรอง
                    redis.SetKey(DeviceUniqueID & "_Register", False)
                End If
                Returnvalue = "{""Param"": {""RegistrationInfo"" : """"}}"
                'HttpContext.Current.Session("_SchoolID") = _DB.CleanString(SchoolID.Trim())

                'ประกาศตัวแปร ArralyList เพื่อ Add ค่า SchoolId และวันเวลา
                Dim ArrSchoolCode As ArrayList
                ArrSchoolCode = GenArrayListSchoolCode(SchoolID)
                If ArrSchoolCode.Count = 0 Then
                    Return "-1"
                End If
                'ทำการ assign array เอาไว้กับ application ที่มี key เป็น tablet เครื่องนี้
                HttpContext.Current.Application(DeviceUniqueID.Trim()) = ArrSchoolCode

                'If Now.AddMinutes(-5) > dateFromApplicationVar Then

                'End If

                ClsLog.Record(" - ClassDroidPad.GetRegistrationInfo(ไม่เคยลงทะเบียน) : Returnvalue = " & Returnvalue)

                Return Returnvalue
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return "-1" 'ดักเผื่อไว้แทบไม่มีโอกาสเข้า Case นี้เลย
            End Try
        Else
            Return "-1"
        End If

    End Function

    ''' <summary>
    ''' ทำการเช็ครหัสผ่านที่กรอกเข้ามาว่าถูกต้องตรงกับรหัสโรงเรียนหรือเปล่า
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="SchoolPassword">รหัสผ่าน</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function CheckSchoolPassword(ByVal SchoolId As String, ByVal SchoolPassword As String) As Boolean

        Dim sql As String = " Select count(*) From uvw_DroidPad_CheckSchoolPassword where SchoolId = '" & _DB.CleanString(SchoolId.Trim()) & "' and SI_Password = '" & Encryption.MD5(SchoolPassword.Trim()) & "' "
        Dim CheckPassword As Integer
        CheckPassword = _DB.ExecuteScalar(sql)

        ClsLog.Record(" - ClassDroidPad.CheckSchoolPassword : sql = " & sql)
        ClsLog.Record(" - ClassDroidPad.CheckSchoolPassword : return (true,false) = " & CheckPassword)

        If CheckPassword = 0 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' ทำการย้ายโรงเรียนให้ tablet ไปผูกกับโรงเรียนใหม่ที่กรอกเข้ามา
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง tablet</param>
    ''' <param name="SchoolID">รหัสโรงเรียน</param>
    ''' <param name="SchoolPassword">รหัสผ่าน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function MovetoNewSchool(ByVal DeviceUniqueID As String, ByVal SchoolID As String, ByVal SchoolPassword As String) As String

        If DeviceUniqueID <> "" And SchoolID <> "" And SchoolPassword <> "" Then

            Dim Returnvalue As String = "{""Param"": {""Result"" : ""0""}}"
            Dim CheckPassword As Boolean
            CheckPassword = CheckSchoolPassword(SchoolID, SchoolPassword)
            If CheckPassword = False Then
                Return Returnvalue
            End If

            _DB.OpenWithTransection()
            Try
                'ทำการ update ข้อมูลเก่าออกไปก่อน
                Dim sql As String = " UPDATE dbo.t360_tblTablet SET Tablet_IsActive = '0',Tablet_LastUpdate = dbo.GetThaiDate(), ClientId = Null " &
                                    " WHERE School_Code <> '" & _DB.CleanString(SchoolID) & "' AND Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID) & "' and Tablet_IsActive = '1' "
                _DB.ExecuteWithTransection(sql) ' ปรับ IsActive ของ Id เก่าให้เป็น 0 ให้หมด

                ClsLog.Record(" - ClassDroidPad.MovetoNewSchool(try)(ปรับ IsActive ของ Id เก่าให้เป็น 0 ให้หมด) : sql = " & sql)

                'insert ผูกกับโรงเรียนใหม่
                sql = " INSERT INTO dbo.t360_tblTablet " &
                      " (School_Code , Tablet_SerialNumber,Tablet_IsOwner,Tablet_Status,Tablet_IsActive) " &
                      " VALUES('" & _DB.CleanString(SchoolID) & "','" & _DB.CleanString(DeviceUniqueID) & "','1','1','1') "
                _DB.ExecuteWithTransection(sql) ' Insert ลงตาราง t360_tblTablet เพื่อย้าย Tablet มาจากโรงเรียนอื่น

                ClsLog.Record(" - ClassDroidPad.MovetoNewSchool(try)(Insert ลงตาราง t360_tblTablet เพื่อย้าย Tablet มาจากโรงเรียนอื่น) : sql = " & sql)

            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                _DB.RollbackTransection()

                ClsLog.Record(" - ClassDroidPad.MovetoNewSchool(catch) : execption = " & ex.ToString)
                ClsLog.Record(" - ClassDroidPad.MovetoNewSchool(catch) : Returnvalue = " & Returnvalue)

                Return Returnvalue
            Finally
                _DB.CommitTransection()
                ClsLog.Record(" - ClassDroidPad.MovetoNewSchool(finally) : commit ")
            End Try
            'HttpContext.Current.Session("_SchoolID") = SchoolID
            Returnvalue = "{""Param"": {""Result"" : ""1""}}"
            'ประกาศตัวแปร ArralyList เพื่อ Add ค่า SchoolId และวันเวลา
            Dim ArrSchoolCode As ArrayList
            ArrSchoolCode = GenArrayListSchoolCode(SchoolID)
            If ArrSchoolCode.Count = 0 Then
                Return "-1"
            End If
            HttpContext.Current.Application(DeviceUniqueID.Trim()) = ArrSchoolCode

            ClsLog.Record(" - ClassDroidPad.MovetoNewSchool : Returnvalue = " & Returnvalue)

            Return Returnvalue
        Else
            Return "-1"
        End If
    End Function
#End Region

#Region "Register Teacher Tablet"
    ''' <summary>
    ''' เมื่อเลือกลงทะเบียนเป็นเครื่องครู
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="TeacherClass">ชั้นที่ประจำชั้น</param>
    ''' <param name="Room">ห้องที่ประจำชั้น</param>
    ''' <param name="Subject">วิชาที่สอน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetTeacherInfo(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String) As String

        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass IsNot Nothing And TeacherClass.Count > 0 And Subject <> "" Then
            'ตัวแปรที่ไว้เช็คว่าเป็นครูที่ยังไม่ได้ลงทะเบียนเลยหรือเปล่า
            Dim IsNewlyCreatedTeacher As Boolean = False
            'ทำการหารหัสโรงเรียนจาก application 
            Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
            If SchoolId = "" Or SchoolId = "-1" Then
                Return "-1"
            End If
            'If HttpContext.Current.Session("_SchoolID") Is Nothing Then
            '    Return "-1"
            'Else
            '    SchoolId = HttpContext.Current.Session("_SchoolID").ToString()
            'End If
            Dim Returnvalue As String = "{""Param"": {""TeacherInfo"" : """"}}"
            Dim CurrentTeacherId As String = GetTeacherIdByNameAndSchoolId(FirstName, LastName, SchoolId)
            _DB.OpenWithTransection()

            'หาว่ามีครูคนนี้อยู่ในตาราง t360_tblTeacher หรือยัง ถ้ายังก็ทำการ insert ข้อมูลใหม่ 
            If CurrentTeacherId = "" Then

                ClsLog.Record(" - ครูไม่ได้อยู่ใน t360_tblteacher ")

                'ทำการ insert ข้อมูลลง t360_tblTeacher,t360_tblTeacherRoom
                CurrentTeacherId = InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId(SchoolId, FirstName, LastName, TeacherClass, Room, Subject, _DB) 'Insert ลงตาราง 360
                If CurrentTeacherId = "-1" Then
                    _DB.RollbackTransection()
                    Return "-1"
                End If

                ClsLog.Record(" - param ก่อน insert tbluser = " & FirstName & "," & LastName & "," & SchoolId & "," & TeacherClass & "," & Subject & "," & CurrentTeacherId)

                Returnvalue = InsertTeacherWithTransactionIntblUser(FirstName, LastName, SchoolId, TeacherClass, Subject, _DB, CurrentTeacherId) 'Insert ลงตาราง User
                If Returnvalue = "-1" Then
                    _DB.RollbackTransection()
                    Return Returnvalue
                End If

                IsNewlyCreatedTeacher = True 'ตัวแปรเพื่อเช็คว่าครูคนนี้เป็นคนใหม่หรือคนเก่า เพื่อเอาไปเป็นเงื่อนไขในการ Update ห้อง - ชั้น อีกทีนึง
            End If

            ClsLog.Record(" - CurrentTeacherId ก่อน if ที่สอง  ")

            If CurrentTeacherId <> "" Then

                ClsLog.Record(" - CurrentTeacherId <> "" ")

                If CheckTabletIsManyPeople(DeviceUniqueID, CurrentTeacherId, _DB) = True Then 'ถ้ามากกว่า 0 แสดงว่าผูกกับครูคนอื่นอยู่ คืนค่าว่างกลับไป
                    _DB.RollbackTransection()
                    Returnvalue = "{""Param"": {""TeacherInfo"" : """"}}"
                    Return Returnvalue
                End If
                If Isregisterd(DeviceUniqueID, SchoolId, CurrentTeacherId, _DB) = True Then 'ถ้าเป็น True แสดงว่า Tablet ผูกกับครูคนนี้อยู่แล้ว คืน TaecherId กลับไป
                    If UpdateTeacherInfo(CurrentTeacherId, TeacherClass, Room, Subject, SchoolId, _DB) = "-1" Then
                        _DB.RollbackTransection()
                        Return "-1"
                    Else
                        Returnvalue = "{""Param"": {""TeacherInfo"" : """ & CurrentTeacherId & """}}"
                        _DB.CommitTransection()
                        Return Returnvalue
                    End If
                Else 'ถ้าสมัครรอบแรกแล้ว Tablet เครื่องนี้ยังไม่ถูกผุกกับใครจะเข้าเงื่อนไขนี้
                    Dim Tablet_ID As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, True)
                    If Tablet_ID <> "" Then
                        Returnvalue = InsertTeacherOrStudentInTabOwner(SchoolId, Tablet_ID, CurrentTeacherId, _DB, True) 'Insert ตาราง TabletOwner ผุกกับครูคนนี้
                        If Returnvalue = "-1" Then
                            _DB.RollbackTransection()
                            Return Returnvalue
                        End If



                        If IsNewlyCreatedTeacher = False Then 'เช็คว่าเป็นครูคนเก่าหรือเปล่า ถ้าเป็นครูคนเก่าต้อง Update ข้อมูล
                            Returnvalue = UpdateTeacherInfo(CurrentTeacherId, TeacherClass, Room, Subject, SchoolId, _DB)
                            If Returnvalue = "-1" Then
                                _DB.RollbackTransection()
                                Return "-1"
                            End If
                        End If

                        'เพิ่ม Tablet Status เป็นลงทะเบียน
                        If InsertTabletStatusDetail(Tablet_ID, FirstName & " " & LastName, SchoolId, _DB, EnTabletOwnerType.Teacher) = "-1" Then
                            _DB.RollbackTransection()
                            Return "-1"
                        End If

                        _DB.CommitTransection()
                        Returnvalue = "{""Param"": {""TeacherInfo"" : """ & CurrentTeacherId & """}}"
                        Return Returnvalue
                    Else
                        _DB.RollbackTransection()
                        Return "-1"
                    End If

                End If
            Else
                _DB.RollbackTransection()
                Return "-1"
            End If
        Else
            _DB.RollbackTransection()
            Return "-1"
        End If

    End Function

    ''' <summary>
    ''' ทำการย้าย Tablet มาผูกกับครูคนใหม่ ถ้ามันมีผูกอยู่กับคนอื่นแล้ว
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="TeacherClass">สตริงห้อง</param>
    ''' <param name="Room">สตริงชั้น</param>
    ''' <param name="Subject">สตริงวิชา</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function MoveToNewTeacher(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String) As String

        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass <> "" And Subject <> "" Then

            Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
            If SchoolId = "" Or SchoolId = "-1" Then
                Return "-1"
            End If
            'Dim SchoolId As String
            'If HttpContext.Current.Session("_SchoolID") Is Nothing Then
            '    Return "-1"
            'Else
            '    SchoolId = HttpContext.Current.Session("_SchoolID").ToString()
            'End If
            Dim Returnvalue As String = "{""Param"": {""TeacherInfo"" : """"}}"
            Dim CurrentTeacherId As String = GetTeacherIdByNameAndSchoolId(FirstName, LastName, SchoolId)
            Dim TabletId As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, False) 'หาก่อนว่าครูคนนี้สมัครแล้วหรือยัง
            _DB.OpenWithTransection()
            If TabletId <> "" Then
                '_DB.OpenWithTransection()
                Try
                    'ถ้าครูคนนี้ยังไม่ได้สมัครก็ Insert ครูคนนี้ใหม่
                    If CurrentTeacherId = "" Then
                        'insert ลง t360_tblTeacher,t360_tblTeacherRoom
                        CurrentTeacherId = InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId(SchoolId, FirstName, LastName, TeacherClass, Room, Subject, _DB) 'Insert ลงตาราง 360
                        If CurrentTeacherId = "-1" Then
                            _DB.RollbackTransection()
                            Return "-1"
                        End If
                        'insert ลง tblUser,tblUserSubjectClass
                        Returnvalue = InsertTeacherWithTransactionIntblUser(FirstName, LastName, SchoolId, TeacherClass, Subject, _DB, CurrentTeacherId) 'Insert ลงตาราง User
                        If Returnvalue = "-1" Then
                            _DB.RollbackTransection()
                            Return "-1"
                        End If
                    End If
                    'ทำการ update ข้อมูล tablet ที่ผูกกับเจ้าของเก่าทิ้ง แล้ว insert ผูกกับคนใหม่
                    Returnvalue = UpdateOldAndInsertNewTabOwner(CurrentTeacherId, TabletId, SchoolId, _DB, True) 'Update Row ของคนเก่าให้เป็น 0 แล้ว Insert คนใหม่เข้าไป
                    If Returnvalue = "-1" Then
                        _DB.RollbackTransection()
                        Return "-1"
                    End If

                    'เพิ่ม Tablet Status เป็นลงทะเบียน
                    If InsertTabletStatusDetail(TabletId, FirstName & " " & LastName, SchoolId, _DB, EnTabletOwnerType.Teacher) = "-1" Then
                        _DB.RollbackTransection()
                        Return "-1"
                    End If
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection()

                    ClsLog.Record(" - ClassDroidPad.MoveToNewTeacher : return -1 --> ex = " & ex.ToString)

                    Return "-1"
                End Try
                _DB.CommitTransection()
                Returnvalue = "{""Param"": {""TeacherInfo"" : """ & CurrentTeacherId & """}}"
                ClsLog.Record(" - ClassDroidPad.MoveToNewTeacher : Returnvalue = " & Returnvalue)

                Return Returnvalue 'ถ้า Insert หรือ Update ผ่านหมดทุกอย่างปิด Transaction แล้ว Return รหัสครูกลับไป
            Else
                _DB.RollbackTransection()
                ClsLog.Record(" - ClassDroidPad.MoveToNewTeacher : return -1 --> TabletId = '' ")

                Return "-1"
            End If
        Else
            Return "-1"
        End If

    End Function

#End Region

#Region "Register Student Tablet"
    ''' <summary>
    ''' ลงทะเบียน tablet เป็นแบบนักเรียน
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="Room">ห้อง</param>
    ''' <param name="StudentCode">รหัสประจำตัวนักเรียน</param>
    ''' <param name="NumberInRoom">เลขที่</param>
    ''' <param name="Gender">เพศ</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetStudentInfo(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String, ByVal Gender As String) As String

        ClsLog.Record(" GetStudentInfo")

        If DeviceUniqueID <> "" And StudentClass <> "" And StudentCode <> "" And NumberInRoom <> "" And Gender <> "" Then

            ClsLog.Record(" DeviceUniqueID : " & DeviceUniqueID & " StudentClass : " & StudentClass & " StudentCode : " & StudentCode & " NumberInRoom : " & NumberInRoom & " Gender : " & Gender)

            redis.DEL(DeviceUniqueID & "_Register")

            ClsLog.Record(" redis.DEL(" & DeviceUniqueID & "_Register)")

            If IsNothing(FirstName) Then FirstName = "ไม่ได้ระบุ" 'ถ้าไม่ได้ส่ง Firstname มาให้เป็นค่าว่าง
            If IsNothing(LastName) Then LastName = "ไม่ได้ระบุ" 'ถ้าไม่ได้ส่ง LastName มาให้เป็นค่าว่าง

            'เช็คก่อนว่า เพศที่ส่งเข้ามาเป็น m,f เท่านั้น
            If Gender.Trim().ToLower() <> "m" And Gender.Trim().ToLower() <> "f" Then
                ClsLog.Record(" check Gender :  " & Gender.Trim().ToLower())
                Return "-1"
            End If

            Dim ItemJsonData As String = ""
            Gender = Gender.Trim().ToLower()
            'ตัวแปรเพื่อไว้ตรวจว่าเป็นนักเรียนคนเก่าหรือนักเรียนคนใหม่
            Dim IsNewlyStudent As Boolean = False
            Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
            If SchoolId = "" Or SchoolId = "-1" Then
                ClsLog.Record(" Check SchoolId : " & SchoolId)
                Return "-1"
            End If
            'Dim SchoolID As String
            'If HttpContext.Current.Session("_SchoolID") Is Nothing Then
            '    Return "-1"
            'Else
            '    SchoolID = HttpContext.Current.Session("_SchoolID").ToString()
            'End If
            Dim Returnvalue As String = "{""Param"": {""StudentInfo"" : """"}}"
            'เช็คว่ามีนักเรียนคนนี้แล้วหรือยัง
            Dim StudentID As String = GetStudentIdByStudentCode(SchoolId, StudentCode, NumberInRoom, StudentClass, Room)

            _DB.OpenWithTransection()

            'Check t360_tblroom ก่อนว่ามีห้องที่เด็กคนนี้อยู่หรือยัง ถ้ายังไม่มีก็ insert ก่อน
            'If CheckAndInsertTblRoom(SchoolId, StudentClass, Room, _DB) = "-1" Then
            '    _DB.RollbackTransection()
            '    Return "-1"
            'End If

            'ลบทิ้ง 
            If redis.Getkey(DeviceUniqueID & "_Duplicate") <> "" Then redis.DEL(DeviceUniqueID & "_Duplicate")

            'ถ้ายังไม่มีนักเรียนก็ Insert นักเรียน
            If StudentID = "" Then
                'check เพิ่มด้วยว่า เป็นรหัสของนักเรียนคนไหนหรือเปล่าในเทอมปัจจุบัน

                Dim duplicateStudentName As String = CheckDuplicateStudent(SchoolId, StudentCode, NumberInRoom, StudentClass, Room, DeviceUniqueID, _DB)
                If duplicateStudentName <> "" Then
                    ' return ชื่อนักเรียนที่ซ้ำกับไปให้ฝั่ง corona 
                    _DB.RollbackTransection()
                    ClsLog.Record("{""Param"": {""StudentInfo"" : """",""StudentDuplicate"" : """ & duplicateStudentName & """},""user_details"": {}}")
                    Return "{""Param"": {""StudentInfo"" : """",""StudentDuplicate"" : """ & duplicateStudentName & """},""user_details"": {}}"
                End If

                Dim NewStudentId As String = ""
                '_DB.OpenWithTransection()
                Try
                    'ทำการ insert ข้อมูลลง t360_tblStudent,t360_tblStudentRoom
                    StudentID = InsertStudentWithTransactionAndGetStudentId(FirstName, LastName, SchoolId, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender) 'Insert นักเรียน ลงตาราง 360
                    ClsLog.Record(" " & StudentID)
                    If StudentID = "-1" Then
                        _DB.RollbackTransection()
                        Return "-1"
                    End If
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection()

                    ClsLog.Record(" - ClassDroidPad.GetStudentInfo : return -1 --> ex = " & ex.ToString)

                    Return "-1"
                End Try
                IsNewlyStudent = True
            Else
                'เช็คว่า นักเรียนคนนี้ เคยถือ tablet มาก่อนหรือเปล่า ถ้าถิอแล้วจะถือว่า duplicate 
                ' case นี้ เป็น case ที่ลงเป็นคนเดิมที่ผูก tablet แล้ว แต่มาผูกอีกเครื่องนึง
                Dim duplicateStudentName As String = IsStudentRegisterWithTablet(DeviceUniqueID, StudentID, _DB)
                If duplicateStudentName <> "" Then

                    'redis.SetKey(DeviceUniqueID & "_Duplicate", StudentID)
                    'Return "{""Param"": {""StudentInfo"" : """",""StudentDuplicate"" : """ & duplicateStudentName & """},""user_details"": {}}"

                    'เช็คว่า เครื่องที่กำลังลงทะเบียน มีใครใช้อยู่หรือเปล่า
                    Dim tabletOwner As String = CheckTabletHasOwner(DeviceUniqueID, StudentID, _DB)
                    _DB.RollbackTransection()
                    If tabletOwner <> "" Then
                        Return "{""Param"": {""StudentInfo"" : """",""StudentDuplicate2"" : """ & tabletOwner & """},""user_details"": {}}"
                    End If
                    Return "{""Param"": {""StudentInfo"" : """",""StudentDuplicate2"" : """"},""user_details"": {}}"
                End If
            End If

            'ดักเผื่อไว้ถ้าเกิดรหัสนักเรียนเป็นค่าว่าง ให้คืน -1
            If StudentID <> "" Then
                'เช็คว่า Tablet เครื่องนี้ผูกกับนักเรียนคนอื่นอยู่หรือเปล่า
                If CheckTabletIsManyPeople(DeviceUniqueID, StudentID, _DB) = True Then
                    _DB.RollbackTransection()
                    ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                    Returnvalue = "{""Param"": {""StudentInfo"" : """"}," & ItemJsonData & "}"
                    Return Returnvalue
                End If
                'เช็ตว่า Tablet เครื่องนี้เคยลงทะเบียนกับนักเรียนคนี้หรือยัง ถ้าลงแล้วคืน Student_Id กลับไป
                Dim Tablet_ID As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, True)
                If Isregisterd(DeviceUniqueID, SchoolId, StudentID, _DB) = True Then
                    'Update ข้อมูลของนักเรียนคนนี้
                    'If UpdateStudentInfo(SchoolId, StudentID, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender) = "-1" Then
                    If UpdateStudentInfo(StudentID, FirstName, LastName, Gender, _DB) = "-1" Then
                        _DB.RollbackTransection()
                        ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                        Returnvalue = "{""Param"": {""StudentInfo"" : """"}," & ItemJsonData & "}"
                        Return "-1"
                        'Return Returnvalue
                    Else

                        _DB.CommitTransection()
                        ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                        Returnvalue = "{""Param"": {""StudentInfo"" : """ & StudentID & """}," & ItemJsonData & "}"
                        Return Returnvalue
                    End If

                Else 'ถ้า Tablet ยังไม่ได้ผูกกับใครเลย ก็ Insert เพื่อผูกกับนักเรียนคนนี้

                    If Tablet_ID <> "" Then
                        'Insert Tablet ผูกกับนักเรียนคนนี้
                        Returnvalue = InsertTeacherOrStudentInTabOwner(SchoolId, Tablet_ID, StudentID, _DB, False)
                        If Returnvalue = "-1" Then
                            _DB.RollbackTransection()
                            Return Returnvalue
                        End If

                        'ถ้าเป็นนักเรียนคนเก่าต้อง Update ข้อมูล
                        If IsNewlyStudent = False Then
                            'Returnvalue = UpdateStudentInfo(SchoolId, StudentID, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender)
                            Returnvalue = UpdateStudentInfo(StudentID, FirstName, LastName, Gender, _DB)
                            If Returnvalue = "-1" Then
                                _DB.RollbackTransection()
                                Return Returnvalue
                            End If
                        End If

                        If InsertTabletStatusDetail(Tablet_ID, FirstName & " " & LastName, SchoolId, _DB, EnTabletOwnerType.Student) = "-1" Then
                            _DB.RollbackTransection()
                            Return "-1"
                        End If

                        Try

                            _DB.CommitTransection()
                            ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                            Returnvalue = "{""Param"": {""StudentInfo"" : """ & StudentID & """}," & ItemJsonData & "}"

                            Return Returnvalue
                        Catch ex As Exception
                            Return "-1"
                        End Try
                    Else
                        Return "-1"
                    End If
                End If



            Else
                _DB.RollbackTransection()
                ClsLog.Record(" - ClassDroidPad.GetStudentInfo : return -1 --> StudentID = '' ")
                Return "-1"
            End If
            'Return "{""Param"": {""StudentInfo"" : """ & StudentID & """},""user_details"": {}}"
        End If
        Return "-1"
    End Function


    ''' <summary>
    ''' ย้ายข้อมูลไปผูกกับนักเรียนคนใหม่
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="Room">ห้อง</param>
    ''' <param name="StudentCode">รหัสประจำตัวนักเรียน</param>
    ''' <param name="NumberInRoom">เลขที่</param>
    ''' <param name="Gender">เพศ</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function MoveToNewStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String, ByVal Gender As String) As String

        If DeviceUniqueID <> "" And StudentClass <> "" And StudentCode <> "" And NumberInRoom <> "" And Gender IsNot Nothing And Gender.ToString() <> "" Then
            If IsNothing(FirstName) Then FirstName = "ไม่ได้ระบุ" 'ถ้าไม่ได้ส่ง FirstName มาให้เป็นค่าว่าง
            If IsNothing(LastName) Then LastName = "ไม่ได้ระบุ" 'ถ้าไม่ได้ส่ง LastName มาให้เป็นค่าว่าง
            Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
            If SchoolId = "" Or SchoolId = "-1" Then
                Return "-1"
            End If
            'Dim SchoolId As String
            'If HttpContext.Current.Session("_SchoolID") Is Nothing Then
            '    Return "-1"
            'Else
            '    SchoolId = HttpContext.Current.Session("_SchoolID").ToString()
            'End If
            Gender = Gender.ToString().Trim().ToLower()
            Dim ItemJsonData As String = ""
            Dim Returnvalue As String = "{""Param"": {""StudentInfo"" : """"}}"
            'เช็คว่ามีนักเรียนคนนี้หรือยัง
            Dim CurrentStudentId As String = GetStudentIdByStudentCode(SchoolId, StudentCode)
            Dim TabletId As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, False)
            _DB.OpenWithTransection()
            If TabletId <> "" Then
                Try

                    'update คนเก่า isactive = 0  ที่ student & studentroom คือนักเรียนที่ซ้ำ
                    Dim StudentIdDuplicate As String = redis.Getkey(DeviceUniqueID & "_Duplicate")
                    If StudentIdDuplicate <> "" Then
                        RemoveStudentDuplicate(StudentIdDuplicate, _DB)
                        CurrentStudentId = ""
                    End If

                    'ถ้ายังไม่มีนักเรียนก็ต้อง Insert นักเรียนใหม่ก่อน
                    If CurrentStudentId = "" Then
                        CurrentStudentId = InsertStudentWithTransactionAndGetStudentId(FirstName, LastName, SchoolId, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender)
                        If CurrentStudentId = "-1" Then
                            _DB.RollbackTransection()
                            Return "-1"
                        End If
                    Else
                        Returnvalue = UpdateStudentInfo(CurrentStudentId, FirstName, LastName, Gender, _DB)
                        If Returnvalue = "-1" Then
                            _DB.RollbackTransection()
                            Return Returnvalue
                        End If
                    End If
                    'Update Tablet ที่ผูกกับนักเรียนคนเก่าเป็น 0 แล้ว Insert ผูกกับนักเรียนคนใหม่
                    Returnvalue = UpdateOldAndInsertNewTabOwner(CurrentStudentId, TabletId, SchoolId, _DB, False)
                    If Returnvalue = "-1" Then
                        _DB.RollbackTransection()
                        Return "-1"
                    End If
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection()
                    ClsLog.Record(" - ClassDroidPad.MoveToNewStudent : return -1 --> ex = " & ex.ToString)

                    Return "-1"
                End Try
                _DB.CommitTransection()
                redis.DEL(DeviceUniqueID & "_Duplicate")
                ItemJsonData = GetItemJsonDataByStudentId(CurrentStudentId)
                'ถ้า Update และ Insert เรียบร้อยคืนรหัสนักเรียนกลับไป
                Returnvalue = "{""Param"": {""StudentInfo"" : """ & CurrentStudentId & """}," & ItemJsonData & "}"

                ClsLog.Record(" - ClassDroidPad.MoveToNewStudent : Returnvalue = " & Returnvalue)

                Return Returnvalue
            Else
                ClsLog.Record(" - ClassDroidPad.MoveToNewStudent : return -1 --> TabletId = '' ")

                Return "-1"
            End If
        Else
            Return "-1"
        End If

    End Function

#End Region

#Region "GetNextaction ดั่งเดิม"
    'Public Function GetNextAction(ByVal DeviceUniqueID As String, ByVal IsFirstTime As String, ByVal IsTeacher As Boolean)

    '    If DeviceUniqueID <> "" Or IsFirstTime <> "" Then

    '        Dim UseCls As New ClassConnectSql
    '        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    '        Dim KNSession As New ClsKNSession()
    '        'Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '        Dim strNextURL As String = ""
    '        Dim Lock, Mute, Visible As Integer
    '        Lock = 0
    '        Mute = 0
    '        Visible = 1
    '        Dim URL As String = ""
    '        Dim MidText As String = ""
    '        Dim BottomText As String = ""



    '        'If KNSession.GetValueFromClsSess(DeviceUniqueID, "QuizId") Is Nothing Then
    '        Dim CurrentQuizIdFirst As String
    '        CurrentQuizIdFirst = GetQuizIdFromDeviceUniqueIDToScalar(DeviceUniqueID, False) 'หา QuizId จาก Serialnumber ของ Tablet
    '        If CurrentQuizIdFirst <> "" Then
    '            Dim QuizIds As String = CurrentQuizIdFirst

    '            'ต้องเช็คก่อนว่ามี Session ไหนที่เกิน 120 นาทีแล้วต้องลบทิ้ง
    '            KNSession(DeviceUniqueID & "|" & "QuizId") = QuizIds 'เก็บค่า QuizId ครั้งนี้ลง Session
    '            'If IsTeacher = True Then
    '            '    Dim UserId As String = GetUserIdByDeviceId(DeviceUniqueID)
    '            '    If UserId = "" Then
    '            '        Return "-1"
    '            '    End If
    '            '    'ต้องเช็คว่าเป็น SoundLab หรือเปล่า ถ้าเป็น Soundlab ไปหน้าทางเลือก
    '            '    Dim NewSession As String = AddNewSession(UserId)
    '            '    URL = "/Alternative_Pad.aspx?PkSession=" & NewSession
    '            'Else
    '            '    URL = "/Activity/EmptySession.aspx?i=t&DeviceId" & DeviceUniqueID 'ไม่มี QuizId ให้ URL เป็นหน้าส้ม
    '            'End If
    '            'HttpContext.Current.Application("Sess" & DeviceUniqueID) = Nothing
    '            'Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"

    '            'ถ้ามี Quiz แล้วต้องส่งหน้า SelectSession เพราะอาจจะเข้า Quiz เดิมหรือ จัดใหม่เป็นอีก Session ก็ได้
    '            'ต้องดักเฉพาะครูถึงส่งหน้า SelectSession ไป
    '            If IsTeacher = True Then 'ถ้าเป็นครูต้องเข้าเงือนไขนี้เพราะว่าส่ง URL คนละอันกับเด็ก
    '                Dim UserId As String = GetUserIdByDeviceId(DeviceUniqueID)
    '                If UserId = "" Then
    '                    Return "-1"
    '                End If
    '                '*************** If IsSoundLab ? ถ้าเป็น SoundLab ส่ง URL เป็นหน้า Activitypage_TeacherPad ไปเลย
    '                Dim LabId As String = IsInSoundLab(DeviceUniqueID)
    '                If LabId <> "" Then 'ถ้าเป็นห้อง Soundlab ส่งหน้าเล่นเกมไปเลย
    '                    URL = "/Activity/ActivityPage_PadTeacher.aspx?DeviceId" & DeviceUniqueID
    '                Else 'ถ้าไม่ใช่ห้อง Soundlab ต้องมาเช็คว่ายังมี Session อยู่ไหมเผื่อ Tablet อยากเข้า Session เดิม
    '                    'If CheckSession(UserId) = True Then 'ถ้ายังมี Session ที่ยังไม่เกิน 120 นาทีเหลือยู่ส่ง URL เป็นหน้าเลือก Session_Pad ไป
    '                    '    URL = "/SelectSession_Pad.aspx?DeviceId=" & DeviceUniqueID
    '                    'Else 'ถ้าไม่มี Session ให้เปิด Session ใหม่ และคืน URL เป็นหน้าทางเลือก
    '                    '    Dim NewSession As String = AddNewSession(UserId)
    '                    '    URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
    '                    'End If
    '                    ' update ใหม่
    '                    If HttpContext.Current.Session("UserId") Is Nothing Then
    '                        Dim dt As DataTable = GetUserDetailForTablet(UserId)
    '                        If dt.Rows.Count > 0 Then
    '                            HttpContext.Current.Session("UserId") = dt(0)("GUID")
    '                            HttpContext.Current.Session("SchoolID") = dt.Rows(0)("SchoolId")
    '                        End If
    '                    End If
    '                    If IsFirstTime = "t" Then
    '                        Dim ClsSelectSession As New ClsSelectSession()
    '                        If ClsSelectSession.IsHaveSession() Then
    '                            URL = "/Session/SelectSession.aspx?i=t&u=" & UserId
    '                        Else
    '                            Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
    '                            URL = "/" & CurrentPageFromRunMode & "?i=t&u=" & UserId
    '                        End If
    '                    Else
    '                        URL = ""
    '                    End If
    '                End If

    '                Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
    '            End If
    '        Else
    '            If IsTeacher = True Then 'ดักว่าเป็น Tablet ครูหรือเปล่า
    '                Dim UserId As String = GetUserIdByDeviceId(DeviceUniqueID)
    '                If UserId = "" Then
    '                    Return "-1"
    '                End If
    '                Dim LabId As String = IsInSoundLab(DeviceUniqueID)
    '                'ถ้าเป็น Soundlab ต้องเปิด Session ใหม่แล้วส่ง PkSession เป็นชื่อห้อง Soundlab ไป
    '                If LabId <> "" Then
    '                    Dim NewSession As String = AddNewSession(LabId)
    '                    URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
    '                Else
    '                    'เฃ็คก่อนว่ามี app ยัง ถ้ามีแล้ว ต้องตรวจว่าแอบไหนเกิน 120 นาทีแล้วลบทิ้ง แล้วส่งหน้า SelectSession ไป
    '                    'If CheckSession(UserId) = True Then 'ถ้ายังมี Session ที่ยังไม่เกิน 120 นาทีเหลือยู่ส่ง URL เป็นหน้าเลือก Session_Pad ไป
    '                    '    'URL = "/SelectSession_Pad.aspx?DeviceId=" & DeviceUniqueID
    '                    ' If IsFirstTime = "t" Then
    '                    '        URL = "/LoginPage.aspx?i=t"
    '                    '    Else
    '                    '        URL = ""
    '                    '    End If
    '                    'Else 'ถ้าไม่มี App ต้องสร้าง session ใหม่แลว add ใส่ application UserId แล้วส่งไปหน้า ทางเลือก
    '                    '    'Dim NewSession As String = AddNewSession(UserId)
    '                    '    'URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
    '                    '    If IsFirstTime = "t" Then
    '                    '        URL = "/LoginPage.aspx?i=t"
    '                    '    Else
    '                    '        URL = ""
    '                    '    End If
    '                    'End If

    '                    'URL = "/Activity/EmptySession.aspx?i=t" 'ไม่มี QuizId ให้ URL เป็นหน้ารอระหว่างจัดชุดของครู
    '                    If HttpContext.Current.Session("UserId") Is Nothing Then
    '                        Dim dt As DataTable = GetUserDetailForTablet(UserId)
    '                        If dt.Rows.Count > 0 Then
    '                            HttpContext.Current.Session("UserId") = dt(0)("GUID")
    '                            HttpContext.Current.Session("SchoolID") = dt.Rows(0)("SchoolId")
    '                        End If
    '                    End If
    '                    If IsFirstTime = "t" Then
    '                        Dim ClsSelectSession As New ClsSelectSession()
    '                        If ClsSelectSession.IsHaveSession() Then
    '                            URL = "/Session/SelectSession.aspx?i=t&u=" & UserId
    '                        Else
    '                            Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
    '                            URL = "/" & CurrentPageFromRunMode & "?i=t&u=" & UserId
    '                        End If
    '                    Else
    '                        URL = ""
    '                    End If

    '                End If

    '            Else
    '                'URL = "/Activity/EmptySession.aspx?i=t&DeviceId=" & DeviceUniqueID 'ไม่มี QuizId ให้ URL เป็นหน้าส้ม
    '                'If HttpContext.Current.Application("1150tab") Is Nothing Then
    '                '    'URL = "/PracticeMode_Pad/ChooseTestset.aspx?i=t&DeviceUniqueID=" & DeviceUniqueID 'ไม่มี QuizId ให้ URL เป็นหน้าส้ม
    '                '    URL = "/PracticeMode_Pad/ChooseTestset.aspx?i=t"
    '                '    HttpContext.Current.Application("1150tab") = "55"
    '                'Else
    '                '    URL = ""
    '                'End If
    '                If IsFirstTime = "t" Then
    '                    URL = "/PracticeMode_Pad/ChooseTestset.aspx?i=t"
    '                Else
    '                    URL = ""
    '                End If
    '                HttpContext.Current.Application("Sess" & DeviceUniqueID) = Nothing
    '            End If
    '            Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
    '        End If
    '        'End If

    '        Dim CurrentPlayerId As String 'เช็คการ Update ตอนเข้าครั้งแรกเพื่อ Update ให้หน้า Checktablet
    '        CurrentPlayerId = GetPlayerIdByDeviceUniqeId(KNSession(DeviceUniqueID & "|" & "QuizId"), _DB.CleanString(DeviceUniqueID.Trim())) 'Select PlayerId เพื่อนำมา Update Isactive Quizsession ให้เป็น 1 เพื่อยืนยันว่า Tablet เครื่องนี้เข้า Quiz แล้ว
    '        If CurrentPlayerId = "-1" Or CurrentPlayerId = "" Then
    '            URL = "/Activity/EmptySession.aspx?i=t" 'ไม่มี QuizId ให้ URL เป็นหน้าส้ม
    '            HttpContext.Current.Application("Sess" & DeviceUniqueID) = Nothing

    '            Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
    '        End If

    '        'If KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet" & CurrentQuizIdFirst) Is Nothing Then ' **** quizid อาจต้องลบ
    '        If KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") Is Nothing Then ' **** quizid อาจต้องลบ
    '            Dim IsUpdateComplete As String = UpdateStudentCheckTabletReady(KNSession(DeviceUniqueID & "|" & "QuizId"), CurrentPlayerId)
    '            If IsUpdateComplete <> "-1" Then
    '                KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = IsUpdateComplete 'ถ้า Update เรียบร้อยให้ Session มีค่าเป็นคำว่า 1 เพื่อที่จะไม่เข้า If นี่อีกในครั้งต่อไป
    '            Else
    '                Return "-1"
    '            End If
    '        End If

    '        If KNSession(DeviceUniqueID & "|" & "QuizId") IsNot Nothing Then 'ถ้าไม่มี QuizId ก็ไม่ให้เข้าเงื่อนไข
    '            Dim IsSelfPace As Boolean = CType(_DB.ExecuteScalar(" SELECT Selfpace FROM tblQuiz WHERE Quiz_Id = '" & KNSession(DeviceUniqueID & "|" & "QuizId") & "' ;"), Boolean)
    '            Dim CurrentQuizID As String = KNSession(DeviceUniqueID & "|" & "QuizId")
    '            If CurrentQuizID Is Nothing Or CurrentQuizID = Nothing Then
    '                Return "-1"
    '            End If
    '            If DeviceUniqueID IsNot Nothing Then
    '                Dim GetTestSetId As String = GetTestSetIdFromQuizId(CurrentQuizID) 'หา TestsetId จาก QuizId
    '                Dim LastChoice As String = GetLastChoiceQuestionByScalar(CurrentQuizID) 'ดึงค่าข้อสุดท้ายขึ้นมาเพื่อเป็น BottomText ส่งไปให้ Tablet
    '                If GetTestSetId <> "" And GetTestSetId IsNot Nothing Then
    '                    Dim dtCheckAmountQuestion As New DataTable
    '                    dtCheckAmountQuestion = ClsActivity.GetTestset(GetTestSetId) 'หาค่าว่าข้อสอบชุดนี้มีทั้งหมดกี่ข้อ
    '                    If dtCheckAmountQuestion.Rows.Count > 0 Then
    '                        BottomText = LastChoice & "/" & dtCheckAmountQuestion.Rows(0)("QuestionAmount").ToString()
    '                    End If
    '                End If

    '                '********************************************** อาจต้องเอา CurrentQuizIdFirst ออก เอาใส่เข้ามาเพราะว่ามันเช็ค Examnum ของทุกควิซเลย แต่ที่จริงอยากให้เช็คแค่ quiz ล่าสุด
    '                'If KNSession(DeviceUniqueID & "|" & "_ExmanNum" & CurrentQuizIdFirst) Is Nothing Then 'เช็คก่อนว่ามี Session _ExmanNum หรือยังถ้ายังไม่มีให้มีค่าเท่ากับข้อล่าสุด
    '                If KNSession(DeviceUniqueID & "|" & "_ExmanNum") Is Nothing Then 'เช็คก่อนว่ามี Session _ExmanNum หรือยังถ้ายังไม่มีให้มีค่าเท่ากับข้อล่าสุด
    '                    Dim CurrentExamnum As String = GetLastChoiceFromApplication(CurrentQuizID)
    '                    If CurrentExamnum = "-1" Then
    '                        Return "-1"
    '                    Else
    '                        '********************************************** อาจต้องเอา CurrentQuizIdFirst ออก เอาใส่เข้ามาเพราะว่ามันเช็ค Examnum ของทุกควิซเลย แต่ที่จริงอยากให้เช็คแค่ quiz ล่าสุด
    '                        KNSession(DeviceUniqueID & "|" & "_ExmanNum") = CurrentExamnum
    '                        'URL = "/Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & DeviceUniqueID
    '                        URL = "/Activity/ActivityPage_Pad.aspx?i=t"
    '                    End If
    '                End If

    '                Dim AppLastChoice As Integer = GetLastChoiceFromApplication(CurrentQuizID) 'ดึงค่าข้อล่าสุดจาก Application
    '                If AppLastChoice.ToString() = "-1" Then
    '                    Return "-1"
    '                End If
    '                If AppLastChoice <> 0 Then
    '                    ' ดักด้วยว่า ถ้าไปไม่พร้อมกัน ไม่ต้องสั่งเด็กให้ reload
    '                    'Dim IsSelfPace As Boolean = CType(_DB.ExecuteScalar(" SELECT Selfpace FROM tblQuiz WHERE Quiz_Id = '" & CurrentQuizIdFirst & "' ;"), Boolean)
    '                    If Not IsSelfPace Then

    '                        If CInt(KNSession(DeviceUniqueID & "|" & "_ExmanNum")) <> AppLastChoice Then 'ถ้า Session QQNo กับข้อล่าสุดที่ถึงไม่เท่ากันแสดงว่าครูกดเปลี่ยนข้อแล้ว
    '                            If IsTeacher = False Then 'เช็คว่าเครื่องที่ส่งมาเป็นเครื่อง ครู หรือ นักเรียน
    '                                URL = "/Activity/ActivityPage_Pad.aspx?i=t"
    '                                KNSession(DeviceUniqueID & "|" & "_ExmanNum") = AppLastChoice 'Assign ค่าข้อล่าสุดให้กับ Session QQNo
    '                            Else
    '                                KNSession(DeviceUniqueID & "|" & "_ExmanNum") = AppLastChoice
    '                                URL = "/Activity/ActivityPage.aspx?i=t"
    '                                'HttpContext.Current.Session("_QQNo") = LastChoice
    '                                'ถ้าครั้งแรกที่เข้ามาต้องไป Step 1 + เก็บ session ทุกอย่างเหมิอนที่หน้า LogIn
    '                                'URL = "/Activity/ActivityPage.aspx?DeviceUniqueID=" & DeviceUniqueID
    '                                'ถ้าคอมพิวเตอรืเปิดอยู่ที่หน้าไหน Tablet เปิดมาต้องอยู่หน้านั้น
    '                                '***ครุเปิดคอมพิวเตอร์มาถึงหน้าข้อสอบแล้ว Tablet ต้องเปิดหน้านั้น
    '                            End If
    '                        End If

    '                    End If
    '                End If

    '                'เช็คว่าตอนนี้อยู่ข้อเฉลยหรือเปล่าถ้าเป็นข้อเฉลยต้องส่ง URL กลัยไปด้วย
    '                If KNSession(DeviceUniqueID & "|" & "CurrentAnsState") Is Nothing Then 'ถ้ายังไม่มี Session AnsState ก็นำค่าจาก Application มาใส่ให้
    '                    Dim AnsStateValue As String = GetAnsStateFromApplication(CurrentQuizID)
    '                    If AnsStateValue = "-1" Then
    '                        Return "-1"
    '                    Else
    '                        KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = AnsStateValue
    '                    End If
    '                End If
    '                Dim LastAnsState As String = GetAnsStateFromApplication(CurrentQuizID)
    '                If LastAnsState = "-1" Then
    '                    Return "-1"
    '                End If
    '                If IsSelfPace = False Then
    '                    If KNSession(DeviceUniqueID & "|" & "CurrentAnsState").ToString() <> LastAnsState Then 'ถ้า AnswerState ปัจจุบัน กับล่าสุดไม่เท่ากันต้อง Return URL กลับไป
    '                        If IsTeacher = True Then
    '                            URL = "/Activity/ActivityPage.aspx?i=t"
    '                        Else
    '                            URL = "/Activity/ActivityPage_Pad.aspx?i=t"
    '                        End If
    '                        KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = LastAnsState
    '                    End If
    '                End If
    '                Dim dtCommand As DataTable = GetAllCommandFromQuizId(CurrentQuizID) 'Select ค่า Command ต่างๆจาก tblQuizCommand
    '                If dtCommand.Rows.Count > 0 Then
    '                    If dtCommand.Rows(0)("IsLock") Then
    '                        Lock = "1"
    '                    Else
    '                        Lock = "0"
    '                    End If
    '                    If dtCommand.Rows(0)("IsMute") Then
    '                        Mute = "1"
    '                    Else
    '                        Mute = "0"
    '                    End If
    '                    If dtCommand.Rows(0)("IsVisible") Then
    '                        Visible = "1"
    '                    Else
    '                        Visible = "0"
    '                    End If
    '                End If

    '            End If

    '        End If

    '        If IsFirstTime = "t" Then 'ถ้าเปิดโปรแกรมครั้งแรกให้ส่ง URL คืนไปเสมอ
    '            'KNSession(DeviceUniqueID &"|"& "IsUseDeviceId") = True
    '            URL = "/Activity/ActivityPage_Pad.aspx?i=t"
    '        End If

    '        Dim ReturnValue As String = "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
    '        Return ReturnValue

    '    Else
    '        Return "-1"
    '    End If
    'End Function
#End Region

#Region "New GetNextAction"

    Dim redis As New RedisStore()

    ''' <summary>
    ''' เป็น Function ที่ Corona จะเข้ามา Get URL และค่า config Lock,Mute,Visible กลับไปเรื่อยๆในแต่ละ xx วินาที
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="IsFirstTime">เป็นครั้งแรกที่เปิดเครื่องหรือเปล่า ?</param>
    ''' <param name="IsTeacher">เป็นครูรึเปล่า ?</param>
    ''' <returns>Stirng</returns>
    ''' <remarks></remarks>
    Public Function GetNextAction(ByVal DeviceUniqueID As String, ByVal IsFirstTime As String, ByVal IsTeacher As Boolean) As String
        ClsLog.Record(" GetNextAction - Param IsFirstTime = " & IsFirstTime)

        If DeviceUniqueID <> "" Or IsFirstTime <> "" Then
            Dim UseCls As New ClassConnectSql
            Dim ClsActivity As New ClsActivity(New ClassConnectSql)
            Dim KNSession As New ClsKNSession()
            'Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
            Dim strNextURL As String = ""
            'ตัวแปรที่บอกว่าให้ ล็อคหน้าจอ , ปิดเสียง , แสดงเนื้อหา ?
            Dim Lock, Mute, Visible As Integer
            Lock = 0
            Mute = 0
            Visible = 1
            'URL ที่จะทำการ Return ค่ากลับไป
            Dim URL As String = ""
            'จำนวนข้อท
            Dim MidText As String = ""
            Dim BottomText As String = ""

            Dim CurrentQuizIdFirst As String = ""
            'CurrentQuizIdFirst = GetQuizIdFromDeviceUniqueIDToScalar(DeviceUniqueID, False) 'หา QuizId จาก Serialnumber ของ Tablet

            'CurrentQuizIdFirst = redis.Getkey(DeviceUniqueID)
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceUniqueID)
            If q IsNot Nothing Then
                CurrentQuizIdFirst = q.QuizId
            End If

            'เก็บสถานะ tablet
            If redis.Getkey(DeviceUniqueID & "_status") = "" Then
                redis.SetKey(DeviceUniqueID & "_status", "1")
            End If
            redis.Expire(DeviceUniqueID & "_status", 15)

            If CurrentQuizIdFirst <> "" Then

                Dim QuizIds As String = CurrentQuizIdFirst

                'ต้องเช็คก่อนว่ามี Session ไหนที่เกิน 120 นาทีแล้วต้องลบทิ้ง
                KNSession(DeviceUniqueID & "|" & "QuizId") = QuizIds 'เก็บค่า QuizId ครั้งนี้ลง Session

                'ถ้ามี Quiz แล้วต้องส่งหน้า SelectSession เพราะอาจจะเข้า Quiz เดิมหรือ จัดใหม่เป็นอีก Session ก็ได้
                'ต้องดักเฉพาะครูถึงส่งหน้า SelectSession ไป
                If IsTeacher = True Then 'ถ้าเป็นครูต้องเข้าเงือนไขนี้เพราะว่าส่ง URL คนละอันกับเด็ก
                    Dim UserId As String = GetUserIdByDeviceId(DeviceUniqueID)
                    If UserId = "" Then
                        Return "-1"
                    End If
                    '*************** If IsSoundLab ? ถ้าเป็น SoundLab ส่ง URL เป็นหน้า Activitypage_TeacherPad ไปเลย
                    Dim LabId As String = IsInSoundLab(DeviceUniqueID)
                    If LabId <> "" Then 'ถ้าเป็นห้อง Soundlab ส่งหน้าเล่นเกมไปเลย
                        URL = "/Activity/ActivityPage_PadTeacher.aspx?DeviceId" & DeviceUniqueID
                    Else 'ถ้าไม่ใช่ห้อง Soundlab ต้องมาเช็คว่ายังมี Session อยู่ไหมเผื่อ Tablet อยากเข้า Session เดิม
                        'If CheckSession(UserId) = True Then 'ถ้ายังมี Session ที่ยังไม่เกิน 120 นาทีเหลือยู่ส่ง URL เป็นหน้าเลือก Session_Pad ไป
                        '    URL = "/SelectSession_Pad.aspx?DeviceId=" & DeviceUniqueID
                        'Else 'ถ้าไม่มี Session ให้เปิด Session ใหม่ และคืน URL เป็นหน้าทางเลือก
                        '    Dim NewSession As String = AddNewSession(UserId)
                        '    URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
                        'End If
                        ' update ใหม่
                        If HttpContext.Current.Session("UserId") Is Nothing Then
                            Dim dt As DataTable = GetUserDetailForTablet(UserId)
                            If dt.Rows.Count > 0 Then
                                HttpContext.Current.Session("UserId") = dt(0)("GUID")
                                HttpContext.Current.Session("SchoolID") = dt.Rows(0)("SchoolId")
                            End If
                        End If
                        If IsFirstTime = "t" Then
                            Dim ClsSelectSession As New ClsSelectSession()
                            If ClsSelectSession.IsHaveSession() Then
                                URL = "/Session/SelectSession.aspx?i=t&u=" & UserId
                            Else
                                Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
                                URL = "/" & CurrentPageFromRunMode & "?i=t&u=" & UserId
                            End If
                        Else
                            URL = ""
                        End If
                    End If

                    Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
                Else
                    'เป็นนักเรียน
                    'Dim d As New Device() With {.QuizId = CurrentQuizIdFirst}
                    'HttpContext.Current.Application(DeviceUniqueID) = d

                End If
            Else
                If IsTeacher = True Then 'ดักว่าเป็น Tablet ครูหรือเปล่า
                    If redis.Getkey(DeviceUniqueID & "_Register") <> "" Then
                        URL = ""
                    Else
                        Dim LabId As String = IsInSoundLab(DeviceUniqueID)
                        'ถ้าเป็น Soundlab ต้องเปิด Session ใหม่แล้วส่ง PkSession เป็นชื่อห้อง Soundlab ไป
                        If LabId <> "" Then
                            'Dim NewSession As String = AddNewSession(LabId)
                            'URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
                            If IsFirstTime = "t" Then
                                URL = "/LoginPage.aspx?i=t"
                            Else
                                URL = ""
                            End If
                        Else
                            'เฃ็คก่อนว่ามี app ยัง ถ้ามีแล้ว ต้องตรวจว่าแอบไหนเกิน 120 นาทีแล้วลบทิ้ง แล้วส่งหน้า SelectSession ไป
                            'If CheckSession(UserId) = True Then 'ถ้ายังมี Session ที่ยังไม่เกิน 120 นาทีเหลือยู่ส่ง URL เป็นหน้าเลือก Session_Pad ไป
                            '    'URL = "/SelectSession_Pad.aspx?DeviceId=" & DeviceUniqueID
                            ' If IsFirstTime = "t" Then
                            '        URL = "/LoginPage.aspx?i=t"
                            '    Else
                            '        URL = ""
                            '    End If
                            'Else 'ถ้าไม่มี App ต้องสร้าง session ใหม่แลว add ใส่ application UserId แล้วส่งไปหน้า ทางเลือก
                            '    'Dim NewSession As String = AddNewSession(UserId)
                            '    'URL = "/Activity/Alternative_Pad.aspx?PkSession" & NewSession
                            '    If IsFirstTime = "t" Then
                            '        URL = "/LoginPage.aspx?i=t"
                            '    Else
                            '        URL = ""
                            '    End If
                            'End If

                            'URL = "/Activity/EmptySession.aspx?i=t" 'ไม่มี QuizId ให้ URL เป็นหน้ารอระหว่างจัดชุดของครู
                            Dim UserId As String = GetUserIdByDeviceId(DeviceUniqueID)
                            If UserId = "" Then
                                'เครื่องสำรองครู
                                If IsFirstTime = "t" Then
                                    URL = "/LoginPage.aspx?i=t"
                                Else
                                    URL = ""
                                End If
                                ' Return "-1"
                            Else
                                If HttpContext.Current.Session("UserId") Is Nothing Then
                                    Dim dt As DataTable = GetUserDetailForTablet(UserId)
                                    If dt.Rows.Count > 0 Then
                                        HttpContext.Current.Session("UserId") = dt(0)("GUID")
                                        HttpContext.Current.Session("SchoolID") = dt.Rows(0)("SchoolId")
                                    End If
                                End If
                                If IsFirstTime = "t" Then
                                    Dim ClsSelectSession As New ClsSelectSession()
                                    If ClsSelectSession.IsHaveSession() Then
                                        URL = "/Session/SelectSession.aspx?i=t&u=" & UserId
                                    Else
                                        Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
                                        URL = "/" & CurrentPageFromRunMode & "?i=t&u=" & UserId
                                    End If
                                Else
                                    URL = ""
                                End If
                            End If
                        End If
                    End If
                Else
                    ' check ว่าเป็น เครื่องห้อง lab รึปล่าว เพราะเวลาไปหน้า registertablet มันกลับส่ง URL choosetestset ไปแทน
                    If redis.Getkey(DeviceUniqueID & "_Register") <> "" Then
                        URL = ""
                    Else
                        ' นักเรียน
                        ' ถ้าไม่ได้ Quiz ใหม่ จะ return ออกไปเลย
                        If IsFirstTime = "t" Then
                            URL = "/PracticeMode_Pad/ChooseTestset.aspx?i=t"
                            'URL = "/sound.aspx?i=t"
                            'URL = ""
                        Else
                            URL = ""
                        End If
                    End If
                    ' ถ้าไม่มี quiz remove application ออกให้หมด     
                    HttpContext.Current.Application.Remove(DeviceUniqueID & "|" & "IsUpdateCheckTablet")
                    HttpContext.Current.Application.Remove(DeviceUniqueID & "|" & "QuizId")
                    HttpContext.Current.Application.Remove(DeviceUniqueID & "|" & "_ExmanNum")
                    HttpContext.Current.Application.Remove(DeviceUniqueID & "|" & "CurrentAnsState")
                End If

                ClsLog.Record(" - Return Value IsTeacher " & IsTeacher & " + URL = " & URL)

                Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
            End If
            'End If

            Dim CurrentPlayerId As String 'เช็คการ Update ตอนเข้าครั้งแรกเพื่อ Update ให้หน้า Checktablet
            'CurrentPlayerId = GetPlayerIdByDeviceUniqeId(KNSession(DeviceUniqueID & "|" & "QuizId"), _DB.CleanString(DeviceUniqueID.Trim())) 'Select PlayerId เพื่อนำมา Update Isactive Quizsession ให้เป็น 1 เพื่อยืนยันว่า Tablet เครื่องนี้เข้า Quiz แล้ว
            'If CurrentPlayerId = "-1" Or CurrentPlayerId = "" Then
            '    URL = "/Activity/EmptySession.aspx?i=t" 'ไม่มี QuizId ให้ URL เป็นหน้าส้ม                
            '    Return "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
            'End If

            'CurrentPlayerId = q(1)
            CurrentPlayerId = q.PlayerId

            ' Check ตาม Step เดิม
            If HttpContext.Current.Application("Quiz_" & CurrentQuizIdFirst) IsNot Nothing Then

                Dim Quiz As Quiz = HttpContext.Current.Application("Quiz_" & CurrentQuizIdFirst) ' เอาค่า Setting quiz ที่ครูได้สร้างไว้

                If Quiz.IsSelfPace = False Then
                    ' ไปพร้อมกัน
                    'If KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") Is Nothing Then ' **** quizid อาจต้องลบ
                    '    Dim IsUpdateComplete As String = UpdateStudentCheckTabletReady(KNSession(DeviceUniqueID & "|" & "QuizId"), CurrentPlayerId)
                    '    If IsUpdateComplete <> "-1" Then
                    '        KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = IsUpdateComplete 'ถ้า Update เรียบร้อยให้ Session มีค่าเป็นคำว่า 1 เพื่อที่จะไม่เข้า If นี่อีกในครั้งต่อไป
                    '    Else
                    '        Return "-1"
                    '    End If
                    'End If

                    If q.CheckIn = False Then
                        If UpdateStudentCheckTabletReady(q.QuizId, CurrentPlayerId) <> "-1" Then
                            q.CheckIn = True
                            redis.SetKey(Of Quiz)(DeviceUniqueID, q)
                        Else
                            Return "-1"
                        End If
                    End If

                    If KNSession(DeviceUniqueID & "|" & "QuizId") IsNot Nothing Then 'ถ้าไม่มี QuizId ก็ไม่ให้เข้าเงื่อนไข
                        BottomText = Quiz.Examnum & "/" & Quiz.AmountQuestion ' ค่า BottomText ส่งไปให้ Tablet
                        If KNSession(DeviceUniqueID & "|" & "_ExmanNum") Is Nothing Then 'เช็คก่อนว่ามี Session _ExmanNum หรือยังถ้ายังไม่มีให้มีค่าเท่ากับข้อล่าสุด
                            If Quiz.Examnum = "-1" Then
                                Return "-1"
                            Else
                                URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                KNSession(DeviceUniqueID & "|" & "_ExmanNum") = Quiz.Examnum
                            End If
                        Else
                            If CInt(KNSession(DeviceUniqueID & "|" & "_ExmanNum")) <> CInt(Quiz.Examnum) Then 'ถ้า QQNo กับข้อล่าสุดที่ถึงไม่เท่ากันแสดงว่าครูกดเปลี่ยนข้อแล้ว
                                If IsTeacher = True Then 'เช็คว่าเครื่องที่ส่งมาเป็นเครื่อง ครู หรือ นักเรียน
                                    URL = "/Activity/ActivityPage.aspx?i=t"
                                Else
                                    URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                End If
                                KNSession(DeviceUniqueID & "|" & "_ExmanNum") = Quiz.Examnum 'Assign ค่าข้อล่าสุดให้กับ Session QQNo
                            End If
                        End If

                        'เช็คว่าตอนนี้อยู่ข้อเฉลยหรือเปล่าถ้าเป็นข้อเฉลยต้องส่ง URL กลัยไปด้วย
                        If KNSession(DeviceUniqueID & "|" & "CurrentAnsState") Is Nothing Then 'ถ้ายังไม่มี Session AnsState ก็นำค่าจาก Application มาใส่ให้
                            If Quiz.AnswerState = "-1" Then
                                Return "-1"
                            Else
                                KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = Quiz.AnswerState
                            End If
                        Else
                            If KNSession(DeviceUniqueID & "|" & "CurrentAnsState").ToString() <> Quiz.AnswerState Then 'ถ้า AnswerState ปัจจุบัน กับล่าสุดไม่เท่ากันต้อง Return URL กลับไป
                                If IsTeacher = True Then
                                    URL = "/Activity/ActivityPage.aspx?i=t"
                                Else
                                    URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                End If
                                KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = Quiz.AnswerState
                            End If
                        End If

                        Dim dtCommand As DataTable = GetAllCommandFromQuizId(Quiz.QuizId) 'Select ค่า Command ต่างๆจาก tblQuizCommand
                        If dtCommand.Rows.Count > 0 Then
                            If dtCommand.Rows(0)("IsLock") Then
                                Lock = "1"
                            Else
                                Lock = "0"
                            End If
                            If dtCommand.Rows(0)("IsMute") Then
                                Mute = "1"
                            Else
                                Mute = "0"
                            End If
                            If dtCommand.Rows(0)("IsVisible") Then
                                Visible = "1"
                            Else
                                Visible = "0"
                            End If
                        End If
                    End If
                Else
                    ' ไปไม่พร้อมกัน
                    If KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") Is Nothing Then ' **** quizid อาจต้องลบ
                        Dim IsUpdateComplete As String = UpdateStudentCheckTabletReady(KNSession(DeviceUniqueID & "|" & "QuizId"), CurrentPlayerId)
                        If IsUpdateComplete <> "-1" Then
                            KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = IsUpdateComplete 'ถ้า Update เรียบร้อยให้ Session มีค่าเป็นคำว่า 1 เพื่อที่จะไม่เข้า If นี่อีกในครั้งต่อไป
                        Else
                            Return "-1"
                        End If
                    End If

                    If KNSession(DeviceUniqueID & "|" & "QuizId") IsNot Nothing Then 'ถ้าไม่มี QuizId ก็ไม่ให้เข้าเงื่อนไข
                        Dim IsSelfPace As Boolean = CType(_DB.ExecuteScalar(" SELECT Selfpace FROM tblQuiz WHERE Quiz_Id = '" & KNSession(DeviceUniqueID & "|" & "QuizId") & "' ;"), Boolean)
                        Dim CurrentQuizID As String = KNSession(DeviceUniqueID & "|" & "QuizId")
                        If CurrentQuizID Is Nothing Or CurrentQuizID = Nothing Then
                            Return "-1"
                        End If
                        If DeviceUniqueID IsNot Nothing Then
                            Dim GetTestSetId As String = GetTestSetIdFromQuizId(CurrentQuizID) 'หา TestsetId จาก QuizId
                            Dim LastChoice As String = GetLastChoiceQuestionByScalar(CurrentQuizID) 'ดึงค่าข้อสุดท้ายขึ้นมาเพื่อเป็น BottomText ส่งไปให้ Tablet
                            If GetTestSetId <> "" And GetTestSetId IsNot Nothing Then
                                Dim dtCheckAmountQuestion As New DataTable
                                dtCheckAmountQuestion = ClsActivity.GetTestset(GetTestSetId) 'หาค่าว่าข้อสอบชุดนี้มีทั้งหมดกี่ข้อ
                                If dtCheckAmountQuestion.Rows.Count > 0 Then
                                    BottomText = LastChoice & "/" & dtCheckAmountQuestion.Rows(0)("QuestionAmount").ToString()
                                End If
                            End If

                            '********************************************** อาจต้องเอา CurrentQuizIdFirst ออก เอาใส่เข้ามาเพราะว่ามันเช็ค Examnum ของทุกควิซเลย แต่ที่จริงอยากให้เช็คแค่ quiz ล่าสุด
                            'If KNSession(DeviceUniqueID & "|" & "_ExmanNum" & CurrentQuizIdFirst) Is Nothing Then 'เช็คก่อนว่ามี Session _ExmanNum หรือยังถ้ายังไม่มีให้มีค่าเท่ากับข้อล่าสุด
                            If KNSession(DeviceUniqueID & "|" & "_ExmanNum") Is Nothing Then 'เช็คก่อนว่ามี Session _ExmanNum หรือยังถ้ายังไม่มีให้มีค่าเท่ากับข้อล่าสุด
                                Dim CurrentExamnum As String = GetLastChoiceFromApplication(CurrentQuizID)
                                If CurrentExamnum = "-1" Then
                                    Return "-1"
                                Else
                                    '********************************************** อาจต้องเอา CurrentQuizIdFirst ออก เอาใส่เข้ามาเพราะว่ามันเช็ค Examnum ของทุกควิซเลย แต่ที่จริงอยากให้เช็คแค่ quiz ล่าสุด
                                    KNSession(DeviceUniqueID & "|" & "_ExmanNum") = CurrentExamnum
                                    'URL = "/Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & DeviceUniqueID
                                    URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                End If
                            End If

                            Dim AppLastChoice As Integer = GetLastChoiceFromApplication(CurrentQuizID) 'ดึงค่าข้อล่าสุดจาก Application
                            If AppLastChoice.ToString() = "-1" Then
                                Return "-1"
                            End If
                            If AppLastChoice <> 0 Then
                                ' ดักด้วยว่า ถ้าไปไม่พร้อมกัน ไม่ต้องสั่งเด็กให้ reload
                                'Dim IsSelfPace As Boolean = CType(_DB.ExecuteScalar(" SELECT Selfpace FROM tblQuiz WHERE Quiz_Id = '" & CurrentQuizIdFirst & "' ;"), Boolean)
                                If Not IsSelfPace Then

                                    If CInt(KNSession(DeviceUniqueID & "|" & "_ExmanNum")) <> AppLastChoice Then 'ถ้า Session QQNo กับข้อล่าสุดที่ถึงไม่เท่ากันแสดงว่าครูกดเปลี่ยนข้อแล้ว
                                        If IsTeacher = False Then 'เช็คว่าเครื่องที่ส่งมาเป็นเครื่อง ครู หรือ นักเรียน
                                            URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                            KNSession(DeviceUniqueID & "|" & "_ExmanNum") = AppLastChoice 'Assign ค่าข้อล่าสุดให้กับ Session QQNo
                                        Else
                                            KNSession(DeviceUniqueID & "|" & "_ExmanNum") = AppLastChoice
                                            URL = "/Activity/ActivityPage.aspx?i=t"
                                            'HttpContext.Current.Session("_QQNo") = LastChoice
                                            'ถ้าครั้งแรกที่เข้ามาต้องไป Step 1 + เก็บ session ทุกอย่างเหมิอนที่หน้า LogIn
                                            'URL = "/Activity/ActivityPage.aspx?DeviceUniqueID=" & DeviceUniqueID
                                            'ถ้าคอมพิวเตอรืเปิดอยู่ที่หน้าไหน Tablet เปิดมาต้องอยู่หน้านั้น
                                            '***ครุเปิดคอมพิวเตอร์มาถึงหน้าข้อสอบแล้ว Tablet ต้องเปิดหน้านั้น
                                        End If
                                    End If

                                End If
                            End If

                            'เช็คว่าตอนนี้อยู่ข้อเฉลยหรือเปล่าถ้าเป็นข้อเฉลยต้องส่ง URL กลัยไปด้วย
                            If KNSession(DeviceUniqueID & "|" & "CurrentAnsState") Is Nothing Then 'ถ้ายังไม่มี Session AnsState ก็นำค่าจาก Application มาใส่ให้
                                Dim AnsStateValue As String = GetAnsStateFromApplication(CurrentQuizID)
                                If AnsStateValue = "-1" Then
                                    Return "-1"
                                Else
                                    KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = AnsStateValue
                                End If
                            End If
                            Dim LastAnsState As String = GetAnsStateFromApplication(CurrentQuizID)
                            If LastAnsState = "-1" Then
                                Return "-1"
                            End If
                            If IsSelfPace = False Then
                                If KNSession(DeviceUniqueID & "|" & "CurrentAnsState").ToString() <> LastAnsState Then 'ถ้า AnswerState ปัจจุบัน กับล่าสุดไม่เท่ากันต้อง Return URL กลับไป
                                    If IsTeacher = True Then
                                        URL = "/Activity/ActivityPage.aspx?i=t"
                                    Else
                                        URL = "/Activity/ActivityPage_Pad.aspx?i=t"
                                    End If
                                    KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = LastAnsState
                                End If
                            End If
                            Dim dtCommand As DataTable = GetAllCommandFromQuizId(CurrentQuizID) 'Select ค่า Command ต่างๆจาก tblQuizCommand
                            If dtCommand.Rows.Count > 0 Then
                                If dtCommand.Rows(0)("IsLock") Then
                                    Lock = "1"
                                Else
                                    Lock = "0"
                                End If
                                If dtCommand.Rows(0)("IsMute") Then
                                    Mute = "1"
                                Else
                                    Mute = "0"
                                End If
                                If dtCommand.Rows(0)("IsVisible") Then
                                    Visible = "1"
                                Else
                                    Visible = "0"
                                End If
                            End If
                        End If
                    End If
                End If

            End If

            If IsFirstTime = "t" Then 'ถ้าเปิดโปรแกรมครั้งแรกให้ส่ง URL คืนไปเสมอ
                URL = "/Activity/ActivityPage_Pad.aspx?i=t"
            End If

            Dim ReturnValue As String = "{""Param"": {""Lock"" : """ & Lock & """,""Mute"" : """ & Mute & """,""Visible"" : """ & Visible & """, ""NextURL"" : """ & URL & """,""MidText"" : """ & MidText & """,""BottomText"" : """ & BottomText & """}}"
            Return ReturnValue

        Else
            Return "-1"
        End If
    End Function
#End Region

#Region "Quiz"
    Public Function GetQuizIdFromDeviceUniqueID(ByVal DeviceUniqueID As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

        Dim dt As New DataTable
        Dim sql As String = " SELECT TOP 1 Quiz_Id,Player_Id,School_Code FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' " &
                             " ORDER BY LastUpdate DESC "
        If HttpContext.Current.Application("uvw_GetQuizIdAndPlayerIdBySerialNumber_" & DeviceUniqueID) Is Nothing Then 'clear ตอนจบ quiz หรือโดนดึงเข้าควิซอื่น (ยังไม่ได้ทำ)
            HttpContext.Current.Application("uvw_GetQuizIdAndPlayerIdBySerialNumber_" & DeviceUniqueID) = _DB.getdata(sql, , InputConn)
        End If
        'dt = _DB.getdata(sql, , InputConn)
        dt = HttpContext.Current.Application("uvw_GetQuizIdAndPlayerIdBySerialNumber_" & DeviceUniqueID)

        If dt.Rows.Count = 0 Then

            sql = " SELECT TOP 1 Quiz_Id,Player_Id,School_Code FROM dbo.uvw_GetplayerIdForSpareTablet WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' " &
                              " ORDER BY LastUpdate DESC "
            dt = _DB.getdata(sql, , InputConn)

        End If

        Return dt

    End Function
    ''' <summary>
    ''' สำหรับการปิด ควิซ เมื่อครู กดที่ปุ่ม "จบกิจกรรม"
    ''' </summary>
    ''' <param name="QuizId"></param>
    ''' <remarks></remarks>
    Public Sub CloseQuiz(ByVal QuizId As String)
        Dim dt As New DataTable
        Dim sql1 As String = " update tblquiz set endtime = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(), ClientId = Null where quiz_id = '" & QuizId & "';"
        _DB.Execute(sql1)
    End Sub

    ''' <summary>
    ''' หา QuizId ที่ Active อยู่ล่าสุด จาก DeviceId
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="HomeworkMode">ดูว่าเป็นการบ้าน ?</param>
    ''' <returns>String:QuizId,""</returns>
    ''' <remarks></remarks>
    Public Function GetQuizIdFromDeviceUniqueIDToScalar(ByVal DeviceUniqueID As String, ByVal HomeworkMode As Boolean) As String

        Dim CurrentQuizId As String
        'Dim sql As String = " SELECT TOP 1 Quiz_Id FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' " & _
        '                    " ORDER BY LastUpdate DESC "

        'หา ก่อนว่า QuizId ล่าสุดนั้นเป็นโหมดแบบไหน
        Dim sql As String = " SELECT TOP 1 * FROM dbo.uvw_GetQuizIdByTabSerialWithoutQuizScore WHERE Tablet_SerialNumber = '" & DeviceUniqueID & "' " &
                            " ORDER BY LastUpdate DESC "
        'Dim CheckIsSelfPace As String = _DB.ExecuteScalar(sql)
        Dim dt As DataTable = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim CheckIsSelfPace As String = dt.Rows(0)("Selfpace").ToString()
            Dim QuizId As String = dt.Rows(0)("Quiz_Id").ToString()
            Dim q = (From r In GetQuizIdFromApplication()).SingleOrDefault(Function(c) c = QuizId)
            ' check ว่า quizid ยังมีอยู่ใน session ครูหรือเปล่า ถ้ายังมีก็ทำกระบวนการเดิม 
            If Not q Is Nothing Then
                'ถ้าเป็นโหมดแบบไปไม่พร้อมกัน Select หา QuizId จาก View ที่ไม่ Join QuizScore
                If CheckIsSelfPace = "True" Then
                    If HomeworkMode Then
                        sql = " SELECT TOP 1 Quiz_Id FROM dbo.uvw_GetQuizIdByTabSerialHomeworkMode WHERE Tablet_SerialNumber = '" & DeviceUniqueID & "'  ORDER BY LastUpdate DESC "
                    Else
                        sql = " SELECT TOP 1 Quiz_Id FROM dbo.uvw_GetQuizIdByTabSerialWithoutQuizScore WHERE Tablet_SerialNumber = '" & DeviceUniqueID & "' " &
                                                    " AND LastUpdate > DATEADD(MINUTE,-15,dbo.GetThaiDate()) ORDER BY LastUpdate DESC "
                    End If
                    CurrentQuizId = _DB.ExecuteScalar(sql)
                    'ถ้าเป็นโหมดแบบไปพร้อมกัน Select หา QuizId จาก View ที่ Join QuizScore
                Else
                    sql = " SELECT TOP 1 Quiz_Id FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueID.Trim()) & "' " &
                               " AND LastUpdate > DATEADD(MINUTE,-15,dbo.GetThaiDate()) AND IsQuizMode = 1 ORDER BY LastUpdate DESC "
                    CurrentQuizId = _DB.ExecuteScalar(sql).ToString()
                End If
                If CurrentQuizId <> "" Then
                    Dim endtime As String
                    sql = " select endtime from tblquiz where quiz_id = '" & CurrentQuizId & "';"
                    endtime = _DB.ExecuteScalar(sql).ToString()
                    If endtime <> "" Then Return ""
                End If
            Else
                CurrentQuizId = "" 'ถ้าไม่มีแล้วให้คืนค่าว่างกลับไป
            End If
        Else
            CurrentQuizId = ""
        End If

        Return CurrentQuizId

    End Function

    Public Function GetQuizIdFromApplication() As ArrayList
        HttpContext.Current.Application.Lock()
        Dim g As Guid
        Dim l As New ArrayList
        Dim q = From r In HttpContext.Current.Application.AllKeys
                Where Guid.TryParse(r.ToString(), g) = True And HttpContext.Current.Application(r.ToString()) IsNot Nothing
                Select r
        For Each k In q
            If TypeOf HttpContext.Current.Application(k.ToString()) Is ArrayList Then
                Dim arrApplication As New ArrayList
                arrApplication = HttpContext.Current.Application(k.ToString())
                For i = 0 To arrApplication.Count - 1
                    If TypeOf arrApplication.Item(i) Is ClsSessionInFo Then
                        Dim objArrApplication As ClsSessionInFo = arrApplication.Item(i)
                        If Not objArrApplication.QuizId Is Nothing Then
                            l.Add(objArrApplication.QuizId.ToString())
                        End If
                    End If
                Next
            End If
        Next
        HttpContext.Current.Application.UnLock()
        Return l
    End Function

    Public Function GetTestSetIdFromQuizId(ByVal QuizId As String) As String
        Dim sqlTestSetId As String = " SELECT TestSet_Id FROM dbo.tblQuiz  WHERE Quiz_Id = '" & QuizId & "' "
        Dim TestsetId As String = _DB.ExecuteScalar(sqlTestSetId).ToString()

        If TestsetId IsNot Nothing Then
            GetTestSetIdFromQuizId = TestsetId
        End If

        Return GetTestSetIdFromQuizId
    End Function

    Public Function GetLastChoiceQuestion(ByVal QuizId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sqlCheckChangePage As String = " SELECT TOP 1 question_id,QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' ORDER BY lastupdate DESC "
        'GetLastChoiceQuestion = _DB.getdata(sqlCheckChangePage, , InputConn)
        If HttpContext.Current.Application(QuizId & "|QuestionId_QQ_No") Is Nothing Then
            HttpContext.Current.Application.Lock()
            HttpContext.Current.Application(QuizId & "|QuestionId_QQ_No") = _DB.getdata(sqlCheckChangePage, , InputConn)
            HttpContext.Current.Application.UnLock()
        End If
        GetLastChoiceQuestion = HttpContext.Current.Application(QuizId & "|QuestionId_QQ_No")

        Return GetLastChoiceQuestion
    End Function

    Private Function GetLastChoiceQuestionByScalar(ByRef QuizId As String)
        Dim Lastchoice As String = ""
        Dim sqlCheckChangePage As String = " SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & QuizId & "' ORDER BY lastupdate DESC "
        Lastchoice = _DB.ExecuteScalar(sqlCheckChangePage)

        Return Lastchoice
    End Function

    Public Function UpdateWhenStudentClick(ByVal AnswerId As String, ByVal Quizid As String, ByVal QuestionId As String, ByVal StudentID As String, ByVal SrId As String)
        Dim UpdateComplete As Boolean
        'Dim sqlUpdate As String = " UPDATE tblQuizScore SET FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), " & _
        '                          " LastUpdate = dbo.GetThaiDate(),ResponseAmount = ResponseAmount + 1,IsScored = '0',Answer_Id = '" & AnswerId & "', " & _
        '                          " Score = (SELECT TOP 1 Answer_Score FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "'),SR_ID = '" & SrId & "' " & _
        '                          " WHERE Quiz_Id = '" & Quizid & "' AND Student_Id = '" & _
        '                            StudentID & "' AND Question_Id = '" & QuestionId & "' "
        Dim Score As String = ""
        Dim sqlUpdate As String = " SELECT tblQuestionSet.QSet_Type FROM tblQuestion INNER JOIN " &
                                  " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " &
                                  " WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
        Dim CheckType As String = _DB.ExecuteScalar(sqlUpdate)
        'ถ้าเป็น Type 3 ต้องเอามาหา QuestionId ว่าตรงกันไหมถ้าตรงแสดงว่าถูก ถ้าไม่ตรงแสดงว่าผิด
        If CheckType = "3" Then
            sqlUpdate = " SELECT COUNT(*) FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "' " &
                        " AND Question_Id = '" & QuestionId & "' "
            Dim CheckCount As String = _DB.ExecuteScalar(sqlUpdate)
            If CInt(CheckCount) > 0 Then
                sqlUpdate = " SELECT TOP 1 Answer_Score FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "' "
                Score = _DB.ExecuteScalar(sqlUpdate)
            Else
                sqlUpdate = " SELECT TOP 1 Answer_ScoreMinus FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "' "
                Score = _DB.ExecuteScalar(sqlUpdate)
            End If
            'ถ้าเป็น Type 1 - 2 เข้ามาใช้ Query อันเดิม
        Else
            sqlUpdate = " SELECT TOP 1 Answer_Score FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "' "
            Dim CheckScore As String = _DB.ExecuteScalar(sqlUpdate)
            'If CheckScore = "0" Then
            '    sqlUpdate = " SELECT TOP 1 Answer_ScoreMinus FROM dbo.tblAnswer WHERE Answer_Id = '" & AnswerId & "' "
            '    Score = _DB.ExecuteScalar(sqlUpdate)
            'Else
            '    Score = CheckScore
            'End If

            '18/9/58 คุยกับพี่ชินไว้ว่า จะยุบ answer_scoreminus (ตอบผิดติดลบ) ออก ไปลงที่ answer_score แทน
            Score = CheckScore
        End If

        Dim StartQuestion As Date = HttpContext.Current.Session("StartTimeQuestion")
        Dim EndQuesion As Date = Date.Now
        Dim TimeDoQuestion = DateDiff(DateInterval.Second, StartQuestion, EndQuesion)

        sqlUpdate = " UPDATE tblQuizScore SET TimeTotal = TimeTotal + " & TimeDoQuestion & ", FirstResponse = (CASE ResponseAmount WHEN 0 THEN dbo.GetThaiDate() ELSE FirstResponse end), " &
                    " LastUpdate = dbo.GetThaiDate(),ResponseAmount = ResponseAmount + 1,IsScored = '0',Answer_Id = '" & AnswerId & "', " &
                    " Score = '" & Score & "', ClientId = NULL, "

        If SrId = "" Then
            sqlUpdate &= " SR_ID =  null "
        Else
            sqlUpdate &= " SR_ID = '" & SrId & "'"
        End If

        sqlUpdate &= " WHERE Quiz_Id = '" & Quizid & "' AND Student_Id = '" & StudentID & "' AND Question_Id = '" & QuestionId & "'; "


        ' ปรับเมื่อ 18/9/58 รอเช็คด้วยว่า ก่อนจะไปวันที่ 14/10 กดตอบกันเยอะๆ จะมีปัญหา table lock หรือไม่

        'sqlUpdate &= " update tblQuizSession set LastUpdate = dbo.GetThaiDate(),ClientId = NULL,TotalScore = TotalScore + " & Score
        'sqlUpdate &= " where Player_Id = '" & StudentID & "' and Quiz_Id = '" & Quizid & "' ; "

        sqlUpdate &= String.Format("UPDATE tblQuizSession SET LastUpdate = dbo.GetThaiDate(),ClientId = NULL, TotalScore = (SELECT SUM(Score) FROM tblQuizScore WHERE Quiz_Id = '{0}' and Student_Id = '{1}') WHERE Quiz_Id = '{0}' And Player_Id = '{1}';", Quizid, StudentID)


        'If IsHalfWay Then
        '    sqlUpdate &= " Update tblquiz set Ishalfway = '1' where quiz_id = '" & Quizid & "'"
        'End If

        Try
            _DB.Execute(sqlUpdate)
            UpdateComplete = True
            HttpContext.Current.Session("StartTimeQuestion") = Date.Now
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            UpdateComplete = False
            HttpContext.Current.Session("StartTimeQuestion") = Date.Now
        End Try

        Return UpdateComplete

    End Function
#End Region

    Private Function GetUserDetailForTablet(ByVal UserId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblUser LEFT JOIN tblUserSetting ON tblUser.GUID = tblUserSetting.User_Id ")
        sql.Append(" WHERE tblUser.GUID = '")
        sql.Append(UserId)
        sql.Append("';")
        GetUserDetailForTablet = New ClassConnectSql().getdata(sql.ToString())
    End Function

    ''' <summary>
    ''' ทำการปิด-เปิด เสียงในจอนักเรียน
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="NeedMute">ปิด/เปิด เสียง</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function SendMuteAll(ByVal DeviceUniqueID As String, ByVal NeedMute As String) As String

        Dim ReturnValue As String = InsertNewQuizCommand(DeviceUniqueID, NeedMute, , )
        Return ReturnValue

    End Function

    ''' <summary>
    ''' ซ่อน/แสดง หน้าจอของนักเรียน
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="NeedHide">ซ่อน/แสดง หน้าจอนักเรียน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function SendHideAll(ByVal DeviceUniqueID As String, ByVal NeedHide As String) As String

        Dim ReturnValue As String = InsertNewQuizCommand(DeviceUniqueID, , , NeedHide)
        Return ReturnValue

    End Function

    ''' <summary>
    ''' ปลดล็อค/ล็อคหน้าจอนักเรียน
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="NeedLock">ปลดล็อค/ล็อค หน้าจอของนักเรียน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function SendLockAll(ByVal DeviceUniqueID As String, ByVal NeedLock As String) As String

        Dim ReturnValue As String = InsertNewQuizCommand(DeviceUniqueID, , NeedLock, )
        Return ReturnValue

    End Function

    ''' <summary>
    ''' ทำการแปลงค่าที่ส่งมาให้เป็น ชั้นภาษาไทย แบบย่อๆ
    ''' </summary>
    ''' <param name="ClassStr">ค่าชั้นที่ส่งมา</param>
    ''' <returns>String:ชื่อชั้น แบบตัวย่อ</returns>
    ''' <remarks></remarks>
    Public Function FindClassName(ByVal ClassStr As String) As String

        If ClassStr <> "" Then
            ClassStr = ClassStr.ToUpper()
            Select Case ClassStr
                Case "K1"
                    FindClassName = "อ.1"
                Case "K2"
                    FindClassName = "อ.2"
                Case "K3"
                    FindClassName = "อ.3"
                Case "K4"
                    FindClassName = "ป.1"
                Case "K5"
                    FindClassName = "ป.2"
                Case "K6"
                    FindClassName = "ป.3"
                Case "K7"
                    FindClassName = "ป.4"
                Case "K8"
                    FindClassName = "ป.5"
                Case "K9"
                    FindClassName = "ป.6"
                Case "K10"
                    FindClassName = "ม.1"
                Case "K11"
                    FindClassName = "ม.2"
                Case "K12"
                    FindClassName = "ม.3"
                Case "K13"
                    FindClassName = "ม.4"
                Case "K14"
                    FindClassName = "ม.5"
                Case "K15"
                    FindClassName = "ม.6"
                Case Else
                    FindClassName = ""
            End Select
        End If

        Return FindClassName

    End Function

    ''' <summary>
    ''' ทำการนำสตริงวิชามาหารหัสวิชา
    ''' </summary>
    ''' <param name="SubjectStr">สตริงวิชา</param>
    ''' <returns>Integer:รหัสวิชา</returns>
    ''' <remarks></remarks>
    Public Function FindSubjectNumber(ByVal SubjectStr As String) As Integer

        If SubjectStr <> "" Then
            Select Case SubjectStr.ToUpper()
                Case "S1"
                    FindSubjectNumber = 1
                Case "S2"
                    FindSubjectNumber = 2
                Case "S3"
                    FindSubjectNumber = 3
                Case "S4"
                    FindSubjectNumber = 4
                Case "S5"
                    FindSubjectNumber = 5
                Case "S6"
                    FindSubjectNumber = 6
                Case "S7"
                    FindSubjectNumber = 7
                Case "S8"
                    FindSubjectNumber = 8
                Case Else
                    FindSubjectNumber = 0
            End Select
        End If

        Return FindSubjectNumber
    End Function

    ''' <summary>
    ''' นำสตริงวิชามาหาชื่อวิชาที่เป็นภาษาอังกฤษ
    ''' </summary>
    ''' <param name="SubjectStr">สตริงวิชา</param>
    ''' <returns>String:ชื่อวิชาเป็นภาษาอังกฤษ</returns>
    ''' <remarks></remarks>
    Public Function FindStrSubject(ByVal SubjectStr As String) As String
        If SubjectStr <> "" Then
            Select Case SubjectStr.ToUpper()
                Case "S1"
                    Return "thai"
                Case "S2"
                    Return "social"
                Case "S3"
                    Return "math"
                Case "S4"
                    Return "science"
                Case "S5"
                    Return "eng"
                Case "S6"
                    Return "health"
                Case "S7"
                    Return "art"
                Case "S8"
                    Return "career"
                Case Else
                    Return ""
            End Select
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' ทำการนำสตริงห้องชั้นที่ส่งเข้ามาทำการ split ออกเพื่อแยกแต่ละห้องแล้วนำไปหารหัสของแต่ละชั้นแล้วใส่เข้าไปใน array
    ''' </summary>
    ''' <param name="ClassStr">สตริงชั้น</param>
    ''' <returns>ArrayList:ที่มีรหัสชั้น</returns>
    ''' <remarks></remarks>
    Public Function CreateArrayClass(ByVal ClassStr As String) As ArrayList

        Dim ArrClass = ClassStr.Trim().Split("|")
        Dim ArrayClass As New ArrayList
        Dim Isvalid As Boolean = True
        'loop เพื่อวนทุกชั้น เพื่อหารหัสของแต่ละชั้น แล้วนำใส่เข้าไปใน array , เงื่อนไขการจบ loop คือ วนจนครบทุกชั้น
        For i = 0 To ArrClass.Length - 1
            'สตริงของแต่ละชั้น
            Dim EachClass As String = ArrClass(i).ToString()
            'รหัสของชั้น
            Dim AfterTransform As Integer
            Select Case EachClass
                Case "K1"
                    AfterTransform = 1
                    ArrayClass.Add(AfterTransform)
                Case "K2"
                    AfterTransform = 2
                    ArrayClass.Add(AfterTransform)
                Case "K3"
                    AfterTransform = 3
                    ArrayClass.Add(AfterTransform)
                Case "K4"
                    AfterTransform = 4
                    ArrayClass.Add(AfterTransform)
                Case "K5"
                    AfterTransform = 5
                    ArrayClass.Add(AfterTransform)
                Case "K6"
                    AfterTransform = 6
                    ArrayClass.Add(AfterTransform)
                Case "K7"
                    AfterTransform = 7
                    ArrayClass.Add(AfterTransform)
                Case "K8"
                    AfterTransform = 8
                    ArrayClass.Add(AfterTransform)
                Case "K9"
                    AfterTransform = 9
                    ArrayClass.Add(AfterTransform)
                Case "K10"
                    AfterTransform = 10
                    ArrayClass.Add(AfterTransform)
                Case "K11"
                    AfterTransform = 11
                    ArrayClass.Add(AfterTransform)
                Case "K12"
                    AfterTransform = 12
                    ArrayClass.Add(AfterTransform)
                Case "K13"
                    AfterTransform = 13
                    ArrayClass.Add(AfterTransform)
                Case "K14"
                    AfterTransform = 14
                    ArrayClass.Add(AfterTransform)
                Case "K15"
                    AfterTransform = 15
                    ArrayClass.Add(AfterTransform)
                Case Else
                    Isvalid = False
            End Select

        Next
        'ถ้ามีสตริงไหนที่หาค่าไม่ได้เลยให้ทำการ clear array ทิ้งไปเลย
        If Isvalid = False Then
            ArrayClass.Clear()
        End If
        Return ArrayClass

    End Function

    ''' <summary>
    ''' ทำการตัดสตริงแยกชั้นต่างๆออกมาแล้วทำให้เป็นตัวเล็กแล้วยัดกลับเข้าไปใน Array
    ''' </summary>
    ''' <param name="ClassStr"></param>
    ''' <returns>ArrayList</returns>
    ''' <remarks></remarks>
    Private Function CreateOriginalArrayClass(ByVal ClassStr As String) As ArrayList
        Dim ArrClass = ClassStr.Trim().Split("|")
        Dim ArrayClass As New ArrayList
        Dim Isvalid As Boolean = True
        'loop เพื่อแปลงค่าสตริงชั้นให้เป็นตัวเล็ก , เงื่อนไขการจบ loop คือวนให้ครบทุกชั้น
        For i = 0 To ArrClass.Length - 1
            ArrayClass.Add(ArrClass(i).ToString().ToLower())
        Next
        Return ArrayClass
    End Function

    ''' <summary>
    ''' ทำการหาปี เพื่อไปทำเป็นปีการศึกษา จาก วันที่/เวลาปัจจุบัน
    ''' </summary>
    ''' <returns>String:ปีการศึกษา</returns>
    ''' <remarks></remarks>
    Public Function GetAcademicYear() As String

        Dim CurrentYear As Integer = DateTime.Now.Year
        ClsLog.Record(" - ClassDroidPad.GetAcademicYear : CurrentYear = " & CurrentYear)
        Dim CurrentDate As New Date(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
        ClsLog.Record(" - ClassDroidPad.GetAcademicYear : CurrentDate = " & CurrentDate)
        Dim Fixdate As New Date(DateTime.Now.Year, 3, 1)
        ClsLog.Record(" - ClassDroidPad.GetAcademicYear : Fixdate = " & Fixdate)

        If DateValue(Fixdate) > DateValue(CurrentDate) Then CurrentYear -= 1

        If CurrentYear < 2400 Then CurrentYear += 543

        Return CurrentYear.ToString()
    End Function

    Public Function GetAllCommandFromQuizId(ByVal Quiz_Id As String) As DataTable

        Dim dt As New DataTable
        Dim sql As String = " SELECT TOP 1 IsMute,IsLock,IsVisible FROM dbo.tblQuizCommand WHERE Quiz_Id = '" & Quiz_Id & "' ORDER BY LastUpdate desc "
        dt = _DB.getdata(sql)
        Return dt

    End Function

    Private Function GenSQLQuizCommandForFieldName(ByVal FieldName As String, ByVal InputValue As String, ByVal Quiz_Id As String) As String
        Dim sb As New StringBuilder
        If InputValue = "-1" Then
            sb.Append("( Select isnull((SELECT TOP 1 ")
            sb.Append(FieldName)
            sb.Append(" FROM dbo.tblQuizCommand WHERE Quiz_Id = '")
            sb.Append(_DB.CleanString(Quiz_Id))
            If FieldName.ToLower() = "isvisible" Then
                sb.Append("' ORDER BY LastUpdate DESC), '1')), ")
            Else
                sb.Append("' ORDER BY LastUpdate DESC), '0')), ")
            End If
        Else
            sb.Append(" '")
            sb.Append(InputValue)
            sb.Append("', ")
        End If
        GenSQLQuizCommandForFieldName = sb.ToString()
    End Function

    ''' <summary>
    ''' Function ที่ทำการ Insert ค่า Config ให้ปิดเสียง,ปิดหน้าจอ,ลอคไม่ให้หน้าจอนักเรียนกดได้ ลงใน DB เพื่อให้ GetNextAction มา Get ค่า Config กลับไป
    ''' </summary>
    ''' <param name="DeviceUniqueId">รหัสเครื่อง Tablet</param>
    ''' <param name="IsMute">ปิดเสียง ?</param>
    ''' <param name="IsLock">ล็อคหน้าจอนักเรียนไม่ให้กดได้ ?</param>
    ''' <param name="IsVisible">ไม่แสดงเนื้อหา ?</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function InsertNewQuizCommand(ByVal DeviceUniqueId As String, Optional ByVal IsMute As String = "-1", Optional ByVal IsLock As String = "-1", Optional ByVal IsVisible As String = "-1") As String

        Dim Returnvalue As String = "{""Param"": {""Result"" : ""-1""}}" ' "{""Param"": {""Result"" : ""0""}}" 

        If (IsMute <> "-1" And IsMute <> "0" And IsMute <> "1") Or
         (IsLock <> "-1" And IsLock <> "0" And IsLock <> "1") Or
        (IsVisible <> "-1" And IsVisible <> "0" And IsVisible <> "1") Or
         (IsMute = "-1" And IsLock = "-1" And IsVisible = "-1") Or
         (IsMute = "" Or IsLock = "" Or IsVisible = "" Or DeviceUniqueId = "") Then
            Return Returnvalue
        End If

        Dim sql As New StringBuilder
        Dim IsVisibleInverted As String = ""
        If IsVisible = "1" Then
            IsVisibleInverted = "0"
        ElseIf IsVisible = "0" Then
            IsVisibleInverted = "1"
        ElseIf IsVisible = "-1" Then
            IsVisibleInverted = "-1" 'กำหนดเป็น -1 เพื่อจะได้ ส่งไป ดึงค่าเก่ามาจาก db ด้วย GenSQLQuizCommandForFieldName
        End If

        Dim Quiz_Id As String = GetQuizIdFromDeviceUniqueIDToScalar(DeviceUniqueId, False)
        If Quiz_Id <> "" Then
            Try
                sql.Append(" INSERT INTO dbo.tblQuizCommand( QuizCommand_Id ,Quiz_Id ,IsMute ,IsLock ,IsVisible ,IsActive ,LastUpdate) ")
                sql.Append(" VALUES  ( newid() , '")
                sql.Append(Quiz_Id)
                sql.Append("' , ")
                sql.Append(GenSQLQuizCommandForFieldName("IsMute", IsMute, Quiz_Id))
                sql.Append(GenSQLQuizCommandForFieldName("IsLock", IsLock, Quiz_Id))
                sql.Append(GenSQLQuizCommandForFieldName("IsVisible", IsVisibleInverted, Quiz_Id))
                sql.Append(" '1' , dbo.GetThaiDate()) ")
                _DB.Execute(sql.ToString())
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return "{""Param"": {""Result"" : ""-1""}}"
            End Try
            Dim newValue As String = ""
            If IsMute <> "-1" Then newValue = IsMute
            If IsVisible <> "-1" Then newValue = IsVisible
            If IsLock <> "-1" Then newValue = IsLock

            Returnvalue = "{""Param"": {""Result"" : """ & newValue & """}}"
            Return Returnvalue
        Else
            Return "{""Param"": {""Result"" : ""-1""}}"
        End If

    End Function

    ''' <summary>
    ''' เช็คว่า tablet เครื่องนี้ และ เจ้าของคนนี้เคยมีข้อมูลอยู่แล้วหรือเปล่า
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="CurrentTeacherAndStudentId">StudentId/TeacherId</param>
    ''' <param name="ObjDB">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function Isregisterd(ByVal DeviceUniqueID As String, ByVal SchoolId As String, ByVal CurrentTeacherAndStudentId As String, ByRef ObjDB As ClsConnect) As Boolean
        Dim sql As String
        Dim CheckIsRegister As Boolean = False
        sql = " SELECT COUNT(*) FROM t360_tblTablet INNER JOIN t360_tblTabletOwner " &
                    " ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
                    " AND t360_tblTablet.School_Code = t360_tblTabletOwner.School_Code " &
                    " WHERE (t360_tblTabletOwner.TabletOwner_IsActive = 1) AND " &
                    " (t360_tblTablet.Tablet_SerialNumber = '" & ObjDB.CleanString(DeviceUniqueID) & "') AND (t360_tblTablet.School_Code = '" & SchoolId & "') AND " &
                    " (t360_tblTablet.Tablet_IsActive = 1) " &
                    " AND (t360_tblTabletOwner.Owner_Id = '" & CurrentTeacherAndStudentId & "') "
        Dim CheckRegistered As Integer = 0
        If IsNumeric(ObjDB.ExecuteScalarWithTransection(sql)) Then
            CheckRegistered = Integer.Parse(ObjDB.ExecuteScalarWithTransection(sql))
        End If
        If CheckRegistered > 0 Then 'ถ้ามีมากกว่า 0 แสดงว่า Tablet ผูกกับครูคนนี้อยู่แล้ว
            CheckIsRegister = True
        End If
        Return CheckIsRegister
    End Function

    ''' <summary>
    ''' เช็คว่า Tablet เครื่องนี้ผูกกับคนอื่นอยู่หรือเปล่า
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="CurrentTeacherIdOrStudentID">รหัสของคนที่กำลังสมัครอยู่</param>
    ''' <param name="ObjDB">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function CheckTabletIsManyPeople(ByVal DeviceUniqueID As String, ByVal CurrentTeacherIdOrStudentID As String, ByRef ObjDB As ClsConnect) As Boolean

        Dim IsManyPeople As Boolean = False
        Dim sql As String
        'หาว่า Tablet มีผูกกับครูคนอื่นอยู่หรือไม่
        sql = " SELECT COUNT(*) FROM t360_tblTablet INNER JOIN " &
              " t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
              " WHERE t360_tblTabletOwner.TabletOwner_IsActive = 1 AND t360_tblTablet.Tablet_SerialNumber = '" & ObjDB.CleanString(DeviceUniqueID) & "' " &
              " AND t360_tblTabletOwner.Owner_Id <> '" & CurrentTeacherIdOrStudentID & "'; "
        Dim TabIsManyPeople As Integer = 0
        If IsNumeric(ObjDB.ExecuteScalarWithTransection(sql)) Then
            TabIsManyPeople = Integer.Parse(ObjDB.ExecuteScalarWithTransection(sql))
        End If
        If TabIsManyPeople > 0 Then
            IsManyPeople = True
        End If
        Return IsManyPeople

    End Function


    Private Function CheckTabletHasOwner(ByVal DeviceUniqueID As String, ByVal StudentID As String, ByRef ObjDB As ClsConnect) As String
        Dim sql As New StringBuilder

        sql.Append(" SELECT s.* FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id ")
        sql.Append(" INNER JOIN t360_tblStudent s ON s.Student_Id = t360_tblTabletOwner.Owner_Id ")
        sql.Append(" WHERE t360_tblTabletOwner.TabletOwner_IsActive = 1 AND t360_tblTablet.Tablet_SerialNumber = '{0}' ")
        sql.Append(" AND t360_tblTabletOwner.Owner_Id <> '{1}'; ")

        Dim dt As DataTable = ObjDB.getdataWithTransaction(String.Format(sql.ToString(), DeviceUniqueID, StudentID))
        If dt.Rows.Count > 0 Then
            Return String.Format("{0} {1}  เลขที่ {2} {3}{4} รหัส {5}", dt.Rows(0)("Student_FirstName"), dt.Rows(0)("Student_LastName"), dt.Rows(0)("Student_CurrentNoInRoom"), dt.Rows(0)("Student_CurrentClass"), dt.Rows(0)("Student_CurrentRoom"), dt.Rows(0)("Student_Code"))
        End If
        Return ""
    End Function

    ''' <summary>
    ''' ทำการหา TabletId จาก รหัสเครื่อง Tablet
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <param name="SchoolID">รหัสโรงเรียน</param>
    ''' <param name="ObjDB">ตัวแปร Connection</param>
    ''' <param name="IsTransaction">ใช้ Transaction ?</param>
    ''' <returns>String:Tablet_Id</returns>
    ''' <remarks></remarks>
    Private Function GetTabletIdPerSchoolFromDeviceId(ByVal DeviceUniqueID As String, ByVal SchoolID As String, ByRef ObjDB As ClsConnect, ByVal IsTransaction As Boolean) As String

        Dim sql As String
        sql = "SELECT Tablet_Id FROM dbo.t360_tblTablet WHERE School_Code = '" & SchoolID & "' AND Tablet_SerialNumber = '" & ObjDB.CleanString(DeviceUniqueID) & "' AND Tablet_IsActive = '1';"
        If IsTransaction = True Then
            GetTabletIdPerSchoolFromDeviceId = ObjDB.ExecuteScalarWithTransection(sql)
        Else
            GetTabletIdPerSchoolFromDeviceId = ObjDB.ExecuteScalar(sql)
        End If

        ClsLog.Record(" - ClassDroidPad.GetTabletIdPerSchoolFromDeviceId : sql = " & sql)
        ClsLog.Record(" - ClassDroidPad.GetTabletIdPerSchoolFromDeviceId : return  = " & GetTabletIdPerSchoolFromDeviceId)

        Return GetTabletIdPerSchoolFromDeviceId

    End Function

    ''' <summary>
    ''' หา TeacherId,StudentId จาก ชื่อ,นามสกุล,รหัสโรงเรียน
    ''' </summary>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <returns>String:Teacher_Id</returns>
    ''' <remarks></remarks>
    Private Function GetTeacherIdByNameAndSchoolId(ByVal FirstName As String, ByVal LastName As String, ByVal SchoolId As String) As String
        Dim sql As String
        sql = " SELECT Teacher_Id FROM dbo.t360_tblTeacher WHERE Teacher_FirstName = '" & _DB.CleanString(FirstName.Trim()) &
              "' AND Teacher_LastName = '" & _DB.CleanString(LastName.Trim()) & "' AND School_Code = '" & _DB.CleanString(SchoolId) & "'  "
        Dim TeacherOrStudentId As String = _DB.ExecuteScalar(sql)

        ClsLog.Record(" - ClassDroidPad.GetTeacherIdByNameAndSchoolId : sql = " & sql)
        ClsLog.Record(" - ClassDroidPad.GetTeacherIdByNameAndSchoolId : return = " & TeacherOrStudentId)

        GetTeacherIdByNameAndSchoolId = TeacherOrStudentId
        Return GetTeacherIdByNameAndSchoolId

    End Function

    Private Function LoopInsert360TeacherRoom(ByVal CurrentTeacherId As String, ByVal TeacherClass As String, ByVal Room As String, ByVal SchoolId As String, ByRef ObjDB As ClsConnect)
        Try
            Dim sql As String
            Dim ClassArr = TeacherClass.Trim().Split("|")
            Dim CurrentRoom As String = "/" & ObjDB.CleanString(Room).Trim()
            Dim CalendarId As String = GetCalendarID(SchoolId)
            For i = 0 To ClassArr.Length - 1
                Dim StrClass As String = FindClassName(ClassArr(i).Trim())
                If StrClass = "" Then
                    'ObjDB.RollbackTransection()
                    Return "-1"
                End If
                sql = " INSERT INTO dbo.t360_tblTeacherRoom " &
                      " ( School_Code ,Teacher_Id ,Class_Name , Room_Name ,TR_UpdateDate ,TR_MoveType ,TR_IsActive ,Calendar_Id) " &
                      " VALUES  ( '" & SchoolId & "','" & CurrentTeacherId & "','" & StrClass & "','" & CurrentRoom & "',dbo.GetThaiDate(),1,1,'" & CalendarId & "') "
                ObjDB.ExecuteWithTransection(sql) 'Insert ลงตาราง t360_tblTeacherRoom
            Next
            Return "1"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function

    Private Function LoopInsertUserSubJectClass(ByVal NewUserGUID As String, ByVal TeacherClass As String, ByVal Subject As String, ByRef ObjDB As ClsConnect)
        Try
            Dim sql As String
            sql = "Select UserId From tblUser where Guid = '" & NewUserGUID & "'"
            Dim NewUserId As String = ObjDB.ExecuteScalarWithTransection(sql)
            If HttpContext.Current.Session("AllowRegisteredTabletOwnerToLogIn") = True Then 'ถ้าไม่มี Session ไม่ต้อง Insert ลง Table นี้
                If NewUserId <> "" Then
                    sql = "SELECT ISNULL(MAX (USCId),0) + 1 FROM dbo.tblUserSubjectClass"
                    Dim nextUSDID As Integer = ObjDB.ExecuteScalarWithTransection(sql)  '= ไปดึงค่ามากสุด + 1 มาถือไว้
                    Dim selectedClassID As New ArrayList
                    selectedClassID = CreateArrayClass(TeacherClass)
                    If selectedClassID.Count = 0 Then
                        'ObjDB.RollbackTransection()
                        Return "-1"
                    End If

                    Dim selectedSubjectID As New ArrayList
                    Dim AllSubject = Subject.Trim().Split("|")
                    Dim EachSubject As Integer = 0
                    For TotalSubject As Integer = 0 To AllSubject.Length - 1
                        EachSubject = FindSubjectNumber(AllSubject(TotalSubject))
                        If EachSubject = 0 Then
                            Return "-1"
                        End If
                        selectedSubjectID.Add(EachSubject)
                    Next

                    Dim sb As New StringBuilder()
                    sb.Append(" INSERT INTO dbo.tblUserSubjectClass ( USCId, UserId, Detailid, SubjectId, ")
                    sb.Append(" ClassId, LastUpdate, ClientId, GUID ) ")

                    For Each itemClass In selectedClassID
                        For Each itemSubject In selectedSubjectID
                            sb.Append(" SELECT '" & CStr(nextUSDID) & "', '" & NewUserId & "', '1', '" & itemSubject.ToString() & "', '" & itemClass.ToString())
                            sb.Append("', dbo.GetThaiDate(), '0' , NEWID()")
                            sb.Append(" union ")
                            nextUSDID += 1
                        Next
                    Next
                    ObjDB.ExecuteWithTransection(sb.ToString().Substring(0, sb.Length - 7))
                Else
                    'ObjDB.RollbackTransection()
                    Return "-1"
                End If
            End If
            Return "1"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูลของครูลง t360_tblTeacher,t360_tblTeacherRoom
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="TeacherClass">ชั้นที่ประจำชั้น</param>
    ''' <param name="Room">ห้องที่ประจำชั้น</param>
    ''' <param name="Subject">วิชา</param>
    ''' <param name="ObjDB">ตัวแปร class connect ต้องส่งมาเพราะใช้ transaction</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId(ByVal SchoolId As String, ByVal FirstName As String, ByVal LastName As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String, ByRef ObjDB As ClsConnect) As String

        Try
            Dim sql As String
            'Dim ClassArr = TeacherClass.Trim().Split("|")
            'Dim CurrentClass = FindClassName(ClassArr(0))

            'If CurrentClass = "" Then
            '    'ObjDB.RollbackTransection()
            '    Return "-1"
            'End If
            'Dim CurrentRoom As String = "/" & ObjDB.CleanString(Room).Trim()

            'array ที่เก็บ ชั้น/ห้อง ที่ครูคนนี้ประจำชั้น
            Dim ArrCurrentClassRoom As ArrayList = GetArrTeacherCurrentRoom(Room)
            Dim GetStrCurrentClassRoom As String = ArrCurrentClassRoom(0)
            Dim SplitCurrentClassRoom = GetStrCurrentClassRoom.Split("/")
            Dim StrCurrentClass As String = SplitCurrentClassRoom(0)
            'ตัวแปร ห้องอย่างเดียวที่ตัดออกมาจาก array index แรกสุด เพื่อเตรียมนำไป insert
            Dim StrCurrentRoom As String = "/" & SplitCurrentClassRoom(1)
            'เก็บ สตริงของวิชาต่างๆเอาไว้
            Dim SubjectArr = Subject.Trim().Split("|")
            'เก็บ TeacherId ของครูที่กำลังจะ Insert เอาไว้ใช้ในส่วนต่างๆของ Function ด้วย
            Dim CurrentTeacherId As String
            sql = "Select Newid()"
            CurrentTeacherId = ObjDB.ExecuteScalarWithTransection(sql).ToString()
            If CurrentTeacherId <> "" Then
                'Insert ลงตาราง t360_tblTeacher
                sql = " INSERT INTO dbo.t360_tblTeacher(School_Code,Teacher_Id,Teacher_Code ,Teacher_PrefixName,Teacher_FirstName,Teacher_LastName,Teacher_Status," &
                      " Teacher_CurrentClass,Teacher_CurrentRoom,SubDistrict_Id,District_Id, " &
                      " Province_Id,Teacher_IsActive) VALUES('" & SchoolId & "','" & CurrentTeacherId & "','99999','ครู','" & ObjDB.CleanString(FirstName.Trim()) & "','" & ObjDB.CleanString(LastName.Trim()) & "'," &
                      " '1','" & StrCurrentClass & "','" & StrCurrentRoom & "','5692','825','2','1') "
                ObjDB.ExecuteWithTransection(sql)

                ClsLog.Record(" - ClassDroidPad.InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId : sql = " & sql)
                ClsLog.Record(" - ClassDroidPad.InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId : CurrentTeacherId = " & CurrentTeacherId)

                'loop เพื่อ ทำการ Insert ห้อง/ชั้น ที่ตัวเองประจำชั้น ลงไปใน t360_tblTeacherRoom , เงื่อนไขการจบ loop คือ วนจนครบทุกห้อง/ชั้น
                For i = 0 To ArrCurrentClassRoom.Count - 1
                    GetStrCurrentClassRoom = ArrCurrentClassRoom(i)
                    SplitCurrentClassRoom = GetStrCurrentClassRoom.Split("/")
                    StrCurrentClass = SplitCurrentClassRoom(0)
                    StrCurrentRoom = "/" & SplitCurrentClassRoom(1)
                    'Dim StrClass As String = FindClassName(ClassArr(i).Trim())
                    'If StrClass = "" Then
                    '    'ObjDB.RollbackTransection()
                    '    Return "-1"
                    'End If

                    'Insert ลงตาราง t360_tblTeacherRoom
                    sql = " INSERT INTO dbo.t360_tblTeacherRoom " &
                          " ( School_Code ,Teacher_Id ,Class_Name , Room_Name ,TR_UpdateDate ,TR_MoveType ,TR_IsActive) " &
                          " VALUES  ( '" & SchoolId & "','" & CurrentTeacherId & "','" & StrCurrentClass & "','" & StrCurrentRoom & "',dbo.GetThaiDate(),1,1 ) "
                    ObjDB.ExecuteWithTransection(sql)
                Next
            Else
                'ObjDB.RollbackTransection()
                ClsLog.Record(" - ClassDroidPad.InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId : return -1 --> CurrentTeacherId = '' ")

                Return "-1"
            End If

            ClsLog.Record(" - ClassDroidPad.InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId : return = " & CurrentTeacherId)

            Dim ReturnCurrentTeacherIs As String = CurrentTeacherId
            Return ReturnCurrentTeacherIs
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)

            ClsLog.Record(" - ClassDroidPad.InserTeachertWithTransactionIntbl360AndGetCurrentTeacherId(catch) : return -1 --> ex = " & ex.ToString())

            Return "-1"
        End Try

        Return "1"

    End Function

    ''' <summary>
    ''' ทำการ insert ข้อมูลให้ผูก tablet กับ ครู/นักเรียน
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="Tablet_ID">รหัส Tablet</param>
    ''' <param name="CurrentTeacherOrStudentId">StudentId/TeacherId</param>
    ''' <param name="ObjDB">ตัวแปร Connection ต้องส่งมาเพราะใช้ Transaction</param>
    ''' <param name="IsTeacher">เป็นครู ?</param>
    ''' <returns>String:0,1,2</returns>
    ''' <remarks></remarks>
    Private Function InsertTeacherOrStudentInTabOwner(ByVal SchoolId As String, ByVal Tablet_ID As String, ByVal CurrentTeacherOrStudentId As String, ByRef ObjDB As ClsConnect, ByVal IsTeacher As Boolean) As String
        Dim sql As String
        Dim Returnvalue As String = "{""Param"": {""TeacherInfo"" : """"}}"
        Dim OwnerType As Integer = 0
        If IsTeacher = True Then
            OwnerType = 1
        Else
            OwnerType = 2
        End If
        If OwnerType = 0 Then
            Return "-1"
        End If

        Try
            'update owner เก่า isactive = 0
            sql = String.Format("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Tablet_Id = '{0}' AND TabletOwner_IsActive = 1;", Tablet_ID)
            ObjDB.ExecuteWithTransection(sql)

            'update tabletlabdesk isactive = 0 เพราะเครื่องอาจจะเป็นเครื่องห้องแล็ปมาก่อน
            sql = String.Format("UPDATE tblTabletLabDesk SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Tablet_Id = '{0}' AND IsActive = 1;", Tablet_ID)
            ObjDB.ExecuteWithTransection(sql)

            'insert owner ใหม่เข้าไป
            sql = " INSERT INTO dbo.t360_tblTabletOwner( School_Code ,Tablet_Id ,Owner_Id ,Owner_Type ,TON_ReceiveDate , " &
                         " TON_ReturnDate ,TabletOwner_IsActive) VALUES  ( '" & SchoolId & "' ,'" & Tablet_ID & "' ,'" & CurrentTeacherOrStudentId & "' , " &
                         " '" & OwnerType & "' ,dbo.GetThaiDate(),NULL ,1); "
            ClsLog.Record(" - ClassDroidPad.InsertTeacherOrStudentInTabOwner : sql = " & sql)
            ObjDB.ExecuteWithTransection(sql)

            ' update isowner ที่ t360_tbltablet ด้วย เพราะเครื่องก่อนหน้าอาจะเป็นเครื่องสำรองมาก่อน
            sql = String.Format("UPDATE t360_tblTablet SET Tablet_IsOwner = 1 WHERE Tablet_Id = '{0}';", Tablet_ID)
            ObjDB.ExecuteWithTransection(sql)

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            ClsLog.Record(" - ClassDroidPad.InsertTeacherOrStudentInTabOwner : ex = " & ex.InnerException.ToString)
            Return "-1"
        End Try
        Return "1"
    End Function

    ''' <summary>
    ''' ทำการ insert ข้อมูลครูให้เข้าไปใน tblUser,tblUserSubjectClass ด้วย
    ''' </summary>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="TeacherClass">ชั้น</param>
    ''' <param name="Subject">วิชา</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <param name="TeacherId">รหัสครู</param>
    ''' <returns>String:1,-1</returns>
    ''' <remarks></remarks>
    Private Function InsertTeacherWithTransactionIntblUser(ByVal FirstName As String, ByVal LastName As String, ByVal SchoolId As String, ByVal TeacherClass As String, ByVal Subject As String, ByRef ObjDB As ClsConnect, ByVal TeacherId As String) As String
        Try

            Dim sql As String
            Dim DummyPassword As String = Encryption.MD5("11119999")
            'Dim DummyUserName As String = "UserNameForTeacherTablet"
            'ถ้าสมัครกับ tablet จะให้ insert เข้าด้วยรหัสนี้ทั้งหมด
            Dim DummyUserName As String = GetRandomNumber8Digit(ObjDB)
            'sql = "Select newid()"
            'Dim NewUserGUID = ObjDB.ExecuteScalarWithTransection(sql).ToString()
            Dim NewUserGUID As String = TeacherId
            'ทำการ insert ลง tblUser
            If NewUserGUID <> "" Then
                sql = " INSERT INTO dbo.tblUser( UserId ,FirstName ,LastName ,UserName ,Password ,SchoolId ,IsActive ,LastUpdate , " &
                      " ClientId, Guid, IsAllowMenuManageUserSchool, IsAllowMenuManageUserAdmin, IsAllowMenuAdminLog, " &
                      " IsAllowMenuContact, IsAllowMenuSetEmail ) " &
                      " VALUES  ( (SELECT ISNULL(  MAX(UserId) , 0) + 1 " &
                      " FROM dbo.tblUser) , " &
                      "'" & ObjDB.CleanString(FirstName.Trim()) & "' , '" & ObjDB.CleanString(LastName.Trim()) & "' , " &
                      " '" & DummyUserName & "' ,'" & DummyPassword & "' ,'" & SchoolId & "', '1' , dbo.GetThaiDate() ,'0' , " &
                      " '" & NewUserGUID & "' ,'0' ,'0','0','0','0');"
                ObjDB.ExecuteWithTransection(sql) 'Insert ลงตาราง tblUser

                ClsLog.Record(" - ClassDroidPad.InsertTeacherWithTransactionIntblUser : Insert tblUser sql =  " & sql)

                ObjDB.ExecuteWithTransection(" INSERT INTO dbo.tblAssistant VALUES(NEWID(),'" & NewUserGUID & "' ,'" & NewUserGUID & "',dbo.GetThaiDate(),1,NULL); ") ' Insert Assistant ด้วย
                sql = "Select UserId From tblUser where Guid = '" & NewUserGUID & "'"
                Dim NewUserId As String = ObjDB.ExecuteScalarWithTransection(sql).ToString()

                ClsLog.Record(" - ClassDroidPad.InsertTeacherWithTransactionIntblUser : Insert tblAssistant sql = " & " INSERT INTO dbo.tblAssistant VALUES(NEWID(),'" & NewUserGUID & "' ,'" & NewUserGUID & "',dbo.GetThaiDate(),1,NULL); ")

                ' ขอเอา If ข้างล่างออกก่อนนะครับ หาที่มาไม่เจอ พอดี ลงทะเบียนจาก Tablet เป็นครูแล้ว ต้อง Insert tblUserSubjectClass ลงไปด้วย
                ' * 21/7/57 คุยกับพี่ชินแล้ว ให้ comment ไว้ก่อน เพราะยังไม่สรุปว่าสองค่านี้เอาไว้ทำอะไร, ไม่รู้ว่าเอาไปใช้ในส่วนไหนบ้างใน Pointplus , พี่ชินบอกว่าไม่รู้ว่าเอาไว้แยกการใช้งานระหว่าง App และ PC หรือเปล่า , ยังสรุปไม่ได้
                'If HttpContext.Current.Application("AllowRegisteredTabletOwnerToLogIn") = True And HttpContext.Current.Application("EnableUserSubjectClass") IsNot Nothing Then 'ถ้าไม่มีสิทธิ์ ไม่ต้อง Insert ลง Table นี้
                If NewUserId <> "" Then
                    sql = "SELECT ISNULL(MAX (USCId),0) + 1 FROM dbo.tblUserSubjectClass"
                    ClsLog.Record(sql)
                    Dim nextUSDID As Integer = ObjDB.ExecuteScalarWithTransection(sql)  ' ไปดึงค่ามากสุด + 1 มาถือไว้
                    ClsLog.Record(nextUSDID)
                    'เก็บรหัสของชั้นทั้งหมดไว้
                    Dim selectedClassID As New ArrayList
                    'เก็บสตริงชั้นที่เป็นตัวเล็กเอาไว้ 
                    Dim ArrCheckClass As New ArrayList
                    selectedClassID = CreateArrayClass(TeacherClass)
                    ArrCheckClass = CreateOriginalArrayClass(TeacherClass)
                    ClsLog.Record("selectedClassID = " & selectedClassID.Count)
                    ClsLog.Record("ArrCheckClass = " & ArrCheckClass.Count)
                    If selectedClassID.Count = 0 And ArrCheckClass.Count = 0 Then
                        'ObjDB.RollbackTransection()
                        Return "-1"
                    End If

                    Dim selectedSubjectID As New ArrayList
                    Dim ArrCheckSubject As New ArrayList
                    Dim AllSubject = Subject.Trim().Split("|")
                    Dim EachSubject As Integer = 0
                    Dim EachStrSubject As String = ""
                    'วนเพื่อนำสตริงของวิชา ไปหา รหัสวิชา และ ชื่อวิชาที่เป็นภาษาอังกฤษ , เงื่อนไขการจบ loop คือ วนให้ครบทุกวิชา
                    For TotalSubject As Integer = 0 To AllSubject.Length - 1
                        EachSubject = FindSubjectNumber(AllSubject(TotalSubject))
                        EachStrSubject = FindStrSubject(AllSubject(TotalSubject))
                        If EachStrSubject <> "" Then
                            ArrCheckSubject.Add(EachStrSubject.ToLower())
                        End If
                        If EachSubject = 0 Then
                            Return "-1"
                        End If
                        selectedSubjectID.Add(EachSubject)
                    Next
                    ClsLog.Record("selectedSubjectID = " & selectedSubjectID.Count)
                    ClsLog.Record("ArrCheckSubject = " & ArrCheckSubject.Count)
                    'วนชั้นก่อน เพื่อ insert ข้อมูลลง tblUsersubjectClass , เงื่อนไขการจบ loop คือ วนให้ครบทุกชั้น
                    For IndexEachClass As Integer = 0 To selectedClassID.Count - 1
                        ClsLog.Record(" - Insert To tblUserSubjectClass วนชั้น")
                        'วนวิชา เพื่อ Insert ข้อมูลลง tblUserSubjectClass ให้เป็นชั้น และ วิชานี้ , เงื่อนไขการจบ loop คือวนให้ครบทุกวิชา
                        For IndexEachSubject As Integer = 0 To selectedSubjectID.Count - 1
                            ClsLog.Record(" - Insert To tblUserSubjectClass วนวิชา")
                            If HttpContext.Current.Application("EnableUserSubjectClass") Is Nothing Then
                                ClsLog.Record("EnableUserSubjectClass Is Nothing")
                            End If

                            'ทำการเช็คก่อนด้วยว่า ชั้น และ วิชานี้ สามารถใช้ได้หรือเปล่าโดยดูจาก config ที่อยู่ใน file langset.bin
                            If CheckClassAndSubjectIsAllowByConfig(ArrCheckClass(IndexEachClass), ArrCheckSubject(IndexEachSubject), HttpContext.Current.Application("EnableUserSubjectClass").ToString().ToLower()) = True Then
                                ClsLog.Record(" - Insert To tblUserSubjectClass เข้า IF ")
                                'รหัสวิชา และ ชื่อวิชา
                                Dim EachGroupsubjectId As String = GetGroupsubjectIdBySubjectId(selectedSubjectID(IndexEachSubject).ToString(), ObjDB)
                                ClsLog.Record(" - Insert To tblUserSubjectClass EachGroupsubjectId = " & EachGroupsubjectId)
                                'รหัสชั้น และ ชื่อชั้น
                                Dim EachLevelId As String = GetLevelidByClassId(selectedClassID(IndexEachClass).ToString(), ObjDB)
                                ClsLog.Record(" - Insert To tblUserSubjectClass EachLevelId = " & EachLevelId)
                                'ถ้าหาค่าได้ครบให้ทำการ insert ข้อมูลลง tblUserSubjectClass
                                If EachGroupsubjectId <> "" Then
                                    sql = " INSERT INTO dbo.tblUserSubjectClass ( USCId, UserId, Detailid, SubjectId, ClassId, LastUpdate, ClientId, GUID ,GroupSubjectId,IsActive,LevelId ) " &
                                      " VALUES  ( '" & nextUSDID & "' ,'" & NewUserGUID & "','1','" & selectedSubjectID(IndexEachSubject).ToString() & "','" & selectedClassID(IndexEachClass).ToString() & "', " &
                                      " dbo.GetThaiDate(),'0',NEWID(),'" & EachGroupsubjectId & "',1,'" & EachLevelId & "') "
                                    ObjDB.ExecuteWithTransection(sql)
                                    ClsLog.Record(" - Insert To tblUserSubjectClass sql = " & sql)
                                    nextUSDID += 1
                                End If
                            End If
                        Next
                    Next

                    'Dim sb As New StringBuilder()
                    'sb.Append(" INSERT INTO dbo.tblUserSubjectClass ( USCId, UserId, Detailid, SubjectId, ")
                    'sb.Append(" ClassId, LastUpdate, ClientId, GUID ) ")

                    'For Each itemClass In selectedClassID
                    '    For Each itemSubject In selectedSubjectID
                    '        sb.Append(" SELECT '" & CStr(nextUSDID) & "', '" & NewUserId & "', '1', '" & itemSubject.ToString() & "', '" & itemClass.ToString())
                    '        sb.Append("', dbo.GetThaiDate(), '0' , NEWID()")
                    '        sb.Append(" union ")
                    '        nextUSDID += 1
                    '    Next
                    'Next
                    'ObjDB.ExecuteWithTransection(sb.ToString().Substring(0, sb.Length - 7))
                Else
                    'ObjDB.RollbackTransection()
                    ClsLog.Record(" - ClassDroidPad.InsertTeacherWithTransactionIntblUser : retrun -1 --> NewUserId = '' ")

                    Return "-1"
                End If
                'End If
            Else
                'ObjDB.RollbackTransection()
                ClsLog.Record(" - ClassDroidPad.InsertTeacherWithTransactionIntblUser : retrun -1 --> NewUserGUID = '' ")

                Return "-1"
            End If

            Return "1" 'ถ้าทำเสร็จผ่านทุกอย่าง Return 1 กลับไป
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            ClsLog.Record(" - ClassDroidPad.InsertTeacherWithTransactionIntblUser(catch) : return -1 --> ex = " & ex.ToString)

            Return "-1"
        End Try

    End Function

    Private Function CheckAndInsertTblRoom(ByVal SchoolId As String, ByVal ClassName As String, ByVal RoomName As String, ByRef InputObjDb As ClsConnect)

        Try

            If Not RoomName.Trim().StartsWith("/") Then
                RoomName = "/" & RoomName.Trim()
            End If

            Dim sql As String = " SELECT COUNT(*) FROM dbo.t360_tblRoom WHERE Class_Name = '" & InputObjDb.CleanString(ClassName) & "' AND Room_Name = '" & InputObjDb.CleanString(RoomName) & "' "
            Dim CheckIsHaveThisRoom As String = InputObjDb.ExecuteScalarWithTransection(sql)
            If CInt(CheckIsHaveThisRoom) = 0 Then
                sql = " INSERT INTO dbo.t360_tblRoom (School_Code, Class_Name, Room_Name,Room_Id, Room_IsActive, LastUpdate) " &
                      " VALUES  ( '" & SchoolId & "' ,'" & InputObjDb.CleanString(ClassName) & "' , '" & InputObjDb.CleanString(RoomName) & "' ,NEWID() ,1 , dbo.GetThaiDate() ) "
                InputObjDb.ExecuteWithTransection(sql)
            End If
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

    End Function

    ''' <summary>
    ''' ทำการ insert ข้อมูลของนักเรียน ลง t360_tblStudent,t360_tblStudentRoom,t360_tblRoom
    ''' </summary>
    ''' <param name="FirstName">ชื่อ</param>
    ''' <param name="LastName">นามสกุล</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="StudentCode">รหัสประจำตัวนักเรียน</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="Room">ห้อง</param>
    ''' <param name="NumberInRoom">เลขที่</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <param name="Gender">เพศ</param>
    ''' <returns>String:1,-1</returns>
    ''' <remarks></remarks>
    Private Function InsertStudentWithTransactionAndGetStudentId(ByVal FirstName As String, ByVal LastName As String, ByVal SchoolId As String, ByVal StudentCode As String, ByVal StudentClass As String, ByVal Room As String, ByVal NumberInRoom As String, ByRef ObjDB As ClsConnect, ByVal Gender As String) As String
        Try
            If IsNothing(FirstName) Then FirstName = "ไม่ได้ระบุ" 'ถ้าไม่มี FirstName ให้เป็นค่าว่าง
            If IsNothing(LastName) Then LastName = "ไม่ได้ระบุ" 'ถ้าไม่มี LastName ให้เป็นค่าว่าง
            Dim Sql As String = "SELECT newid();"
            Dim CurrentStudentId As String
            Dim StudentID As String = ObjDB.ExecuteScalarWithTransection(Sql).ToString() 'Select Newid() เพื่อนำมา Insert จะได้รู้ว่าครูคนนี้ที่เราเพิ่ง Insert มี GUID อะไร
            'StudentId ของนักเรียนคนใหม่ที่กำลังจะ Insert เอาไว้ใช้ส่วนต่อๆไปของ Function ด้วย
            If StudentID <> "" Then
                'ปีการศึกษา
                Dim CalendarId As String = GetCalendarID(SchoolId)
                Dim ClassArr = StudentClass.Trim().Split("|")
                'ชั้น
                Dim CurrentClass As String = FindClassName(ClassArr(0)).ToString
                Dim PrefixName As String = ""
                'ห้อง
                Dim CurrentRoom As String = ""
                Dim CurrentRoomId As String = ""
                If Not Room.Trim().StartsWith("/") Then
                    CurrentRoom = "/" & ObjDB.CleanString(Room.Trim())
                Else
                    CurrentRoom = ObjDB.CleanString(Room.Trim())
                End If
                'เพศ
                If Gender = "f" Then
                    PrefixName = "ด.ญ."
                Else
                    PrefixName = "ด.ช."
                End If

                'Dim CheckIsAlreadyClassOrRoom As String = ""
                'Check ว่าโรงเรียนนี้มี ชั้น กับ ห้อง นี้แล้วหรือยัง ถ้ายังไม่มีก็ Insert มีแล้วก็ข้ามไป
                'เช็คชั้น
                If CheckIsAlreadySchoolClassOrRoom(True, SchoolId, CurrentClass, CurrentRoom, ObjDB) = "No" Then
                    If InsertSchoolClassOrRoom(True, SchoolId, CurrentClass, CurrentRoom, ObjDB) = "-1" Then
                        Return "-1"
                    End If
                End If
                'เช็คห้อง
                If CheckIsAlreadySchoolClassOrRoom(False, SchoolId, CurrentClass, CurrentRoom, ObjDB) = "No" Then
                    If InsertSchoolClassOrRoom(False, SchoolId, CurrentClass, CurrentRoom, ObjDB) = "-1" Then
                        Return "-1"
                    End If
                End If


                'ทำการหาก่อนว่ามี ห้อง/ชั้น นี้อยู่หรือเปล่า
                Sql = "select Room_Id from t360_tblRoom where Class_Name = '" & CurrentClass & "' and Room_Name = '" & CurrentRoom & "' and School_Code = '" & SchoolId & "' and Room_IsActive = 1;"
                ClsLog.Record(" - ClassDroidPad.InsertStudentWithTransactionAndGetStudentId : GetCurrentRoomId = " & Sql)
                'CurrentRoomId = _DB.ExecuteScalar(Sql)
                CurrentRoomId = ObjDB.ExecuteScalarWithTransection(Sql)
                ClsLog.Record(" - ClassDroidPad.InsertStudentWithTransactionAndGetStudentId : RoomId = " & CurrentRoomId)

                'ทำการ insert ข้อมูลนักเรียนลง t360_tblStudent
                Sql = " INSERT INTO dbo.t360_tblStudent( School_Code ,Student_Id,Student_Code, " &
               " Student_PrefixName,Student_FirstName,Student_LastName,Student_Status, " &
               " Student_CurrentNoInRoom,Student_CurrentClass,Student_CurrentRoom,SubDistrict_Id, " &
               " District_Id,Province_Id,Student_IsActive,Student_Gender,Student_CurrentRoomId ) " &
               " VALUES  ('" & SchoolId & "','" & StudentID & "','" & ObjDB.CleanString(StudentCode.Trim()) & "','" & PrefixName & "','" & ObjDB.CleanString(FirstName.Trim()) & "', " &
               " '" & ObjDB.CleanString(LastName.Trim()) & "','1','" & ObjDB.CleanString(NumberInRoom.Trim()) & "','" & CurrentClass & "','" & CurrentRoom & "', " &
               " '0','0','0','1','" & Gender & "','" & CurrentRoomId & "'); "
                ObjDB.ExecuteWithTransection(Sql) 'Insert ลงตาราง t360_tblStudent 
                ClsLog.Record(" - ClassDroidPad.InsertStudentWithTransactionAndGetStudentId : sql = " & Sql)

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    ClsLog.Record(" - Save ลง checkmark")
                    Dim clsCheckmark As New ClsCheckMark()
                    Dim classId As Integer = clsCheckmark.GetClassId(CurrentClass)
                    Dim s As New StudentCheckMark()
                    s.StudentId = StudentCode.Trim()
                    s.StudentFName = FirstName.Trim()
                    s.StudentLName = LastName.Trim()
                    s.ClassId = classId
                    s.ClassName = CurrentClass
                    s.StudentNumber = CInt(NumberInRoom)
                    s.StudentRoom = CInt(CurrentRoom.Replace("/", ""))
                    s.StudentPrefixName = PrefixName
                    s.SchoolId = SchoolId
                    If Not clsCheckmark.AddStudent(s) Then
                        ClsLog.Record(" - Save ลง checkmark ไม่สำเร็จ")
                        Throw New Exception("ไม่สามารถ save ลง checkmark ได้")
                    End If
                    ClsLog.Record(" - Save ลง checkmark จบ")
                End If

                'loop เพื่อ insert ข้อมูลลง t360_tblStudentRoom , เงื่อนไขการจบ loop คือ วนจนกว่าจะครบทุกชั้น (ซึ่งก็น่าจะมีชั้นเดียว)
                For i = 0 To ClassArr.Length - 1
                    Dim ClassStr As String = FindClassName(ClassArr(i))
                    'หาปีการศึกษา
                    Dim AcademicYear As String = GetAcademicYear()
                    Sql = " INSERT INTO dbo.t360_tblStudentRoom( School_Code ,Student_Id ,Student_NoInRoom " &
                          " ,Class_Name ,Room_Name ,SR_MoveDate ,SR_AcademicYear ,SR_MoveType ,SR_IsActive,Calendar_Id,Room_Id) " &
                          " VALUES  ( '" & SchoolId & "' , '" & StudentID & "' ,'" & ObjDB.CleanString(NumberInRoom).Trim() & "' ,'" & ClassStr & "' ,'" & CurrentRoom & "' " &
                          " , dbo.GetThaiDate() ,'" & AcademicYear & "' , 8 , 1 , '" & CalendarId & "','" & CurrentRoomId & "' ); " 'ตอนลงทะเบียนนักเรียนใหม่ของ T360 ให้ SR_MoveType = 8
                    ObjDB.ExecuteWithTransection(Sql) 'Insert ลงตาราง t360_tblStudentRoom 
                Next

            Else
                Return "-1"
            End If

            ClsLog.Record(" - ClassDroidPad.InsertStudentWithTransactionAndGetStudentId : return = " & StudentID)

            CurrentStudentId = StudentID
            Return CurrentStudentId 'ถ้า Insert เรียบร้อย จะคืน StudentID ที่เพิ่ง insert กลับไป
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)

            ClsLog.Record(" - ClassDroidPad.InsertStudentWithTransactionAndGetStudentId : return -1 --> ex = " & ex.ToString)

            Return "-1"
        End Try

    End Function

    ''' <summary>
    ''' เช็คว่าโรงเรียนนี้มีห้องกับชั้นแล้วหรือยัง
    ''' </summary>
    ''' <param name="IsSchoolClass">เช็ค ชั้น?</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="StudentRoom">ห้อง</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <returns>String:No,Yes</returns>
    ''' <remarks></remarks>
    Private Function CheckIsAlreadySchoolClassOrRoom(ByVal IsSchoolClass As Boolean, ByVal SchoolId As String, ByVal StudentClass As String, ByVal StudentRoom As String, ByRef ObjDB As ClassConnectSql) As String

        Dim sql As String = ""
        Dim CheckResult As String = ""
        'เช็คชั้น
        If IsSchoolClass = True Then
            sql = " SELECT COUNT(*) FROM dbo.t360_tblSchoolClass WHERE School_Code = '" & SchoolId & "' AND Class_Name = '" & StudentClass & "'  AND IsActive = 1; "
            CheckResult = ObjDB.ExecuteScalarWithTransection(sql)
            If CheckResult = "0" Then
                Return "No"
            Else
                Return "Yes"
            End If
        Else
            'เช็คห้อง
            sql = " SELECT COUNT(*) FROM dbo.t360_tblRoom WHERE School_Code = '" & SchoolId & "' AND Class_Name = '" & StudentClass & "' AND Room_Name = '" & StudentRoom & "'  AND Room_IsActive = 1; "
            CheckResult = ObjDB.ExecuteScalarWithTransection(sql)
            If CheckResult = "0" Then
                Return "No"
            Else
                Return "Yes"
            End If
        End If

    End Function

    ''' <summary>
    ''' ทำการ insert ห้อง/ชั้น ลง t360_tblSchoolClass,t360_tblRoom
    ''' </summary>
    ''' <param name="IsSchoolClass">insert ชั้น ?</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="StudentRoom">ห้อง</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <returns>String:Complete,-1</returns>
    ''' <remarks></remarks>
    Private Function InsertSchoolClassOrRoom(ByVal IsSchoolClass As Boolean, ByVal SchoolId As String, ByVal StudentClass As String, ByVal StudentRoom As String, ByRef ObjDB As ClassConnectSql) As String

        Dim sql As String = ""
        Try
            If IsSchoolClass = True Then
                'Insert t360_tblSchoolClass
                sql = " INSERT INTO dbo.t360_tblSchoolClass ( School_Code, Class_Name ) " &
                      " VALUES  ( '" & SchoolId & "', '" & StudentClass & "' ); "
                ObjDB.ExecuteWithTransection(sql)
            Else
                'Insert t360_tblRoom
                sql = " INSERT INTO dbo.t360_tblRoom( School_Code ,Class_Name , " &
                      " Room_Name ,Room_Id ,Room_IsActive) " &
                      " VALUES  ( '" & SchoolId & "' ,'" & StudentClass & "' , '" & StudentRoom & "' , NEWID(),'1'); "
                ObjDB.ExecuteWithTransection(sql)
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

        Return "Complete"

    End Function

    ''' <summary>
    ''' ทำการ update ข้อมูลเก่าทิ้งแล้ว insert ข้อมูลผูก tablet เข้ากับเจ้าของคนใหม่
    ''' </summary>
    ''' <param name="CurrentTeacherOrStudentId">StudentId/TeacherId</param>
    ''' <param name="TabletId">รหัส Tablet</param>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <param name="IsTeacher">เป็นครู ?</param>
    ''' <returns>String:1,-1</returns>
    ''' <remarks></remarks>
    Private Function UpdateOldAndInsertNewTabOwner(ByVal CurrentTeacherOrStudentId As String, ByVal TabletId As String, ByVal SchoolId As String, ByRef ObjDB As ClsConnect, ByVal IsTeacher As Boolean) As String

        Dim sql As String
        Dim OwnerType As Integer = 0
        Try
            sql = " UPDATE dbo.t360_tblTabletOwner SET TabletOwner_IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Owner_Id <> '" & CurrentTeacherOrStudentId & "' " &
                  " AND Tablet_Id = '" & TabletId & "' AND TabletOwner_IsActive = 1; "
            ObjDB.ExecuteWithTransection(sql) 'ปรับค่า IsActiveTablet ของคนเก่าให้เป็น 0
            If IsTeacher = True Then
                OwnerType = 1
            Else
                OwnerType = 2
            End If
            If OwnerType = 0 Then
                Return "-1"
            End If
            sql = " INSERT INTO dbo.t360_tblTabletOwner( School_Code ,Tablet_Id , " &
                  " Owner_Id ,Owner_Type ,TON_ReceiveDate ,TabletOwner_IsActive) " &
                  " VALUES  ( '" & SchoolId & "' , '" & TabletId & "' , '" & CurrentTeacherOrStudentId & "' " &
                  " ,'" & OwnerType & "' ,dbo.GetThaiDate() ,1) "
            ObjDB.ExecuteWithTransection(sql) 'Insert Id ของคนใหม่เข้าไปแล้วปรับ IsActive เป็น 1

            ClsLog.Record(" - ClassDroidPad.UpdateOldAndInsertNewTabOwner : sql " & sql)

            'ObjDB.CommitTransection()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            ClsLog.Record(" - ClassDroidPad.UpdateOldAndInsertNewTabOwner : return -1 --> ex = " & ex.ToString)

            Return "-1"
        End Try
        Return "1"

    End Function

    Private Function UpdateTeacherInfo(ByVal TeacherId As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String, ByVal SchoolId As String, ByRef ObjDB As ClsConnect)
        Dim sql As String

        Try
            sql = " Update tblUserSubjectClass Set Isactive = '0' , LastUpdate = dbo.GetThaiDate(),ClientId = Null where Userid = (select top 1 userid from tbluser where GUID = '" & TeacherId & "') and Isactive = '1' " '
            ObjDB.ExecuteWithTransection(sql)
            'Insert UserSubjectClass
            If LoopInsertUserSubJectClass(TeacherId, TeacherClass, Subject, ObjDB) = "-1" Then
                Return "-1"
            End If
            'Update t360_teacher CurrentRoom Class LastUpdate
            Dim ClassArr = TeacherClass.Trim().Split("|")
            Dim CurrentClass = FindClassName(ClassArr(0))
            Dim CurrentRoom As String = "/" & ObjDB.CleanString(Room).Trim()
            If CurrentClass = "" Then
                Return "-1"
            End If
            sql = " UPDATE dbo.t360_tblTeacher SET Teacher_CurrentClass = '" & CurrentClass & "',Teacher_CurrentRoom = '" & CurrentRoom & "',LastUpdate = dbo.GetThaiDate(),ClientId = Null WHERE Teacher_Id = '" & TeacherId & "' "
            ObjDB.ExecuteWithTransection(sql)
            'Update TeacherRoom Isactive 0 lastupdate
            sql = " UPDATE dbo.t360_tblTeacherRoom SET TR_IsActive = '0',TR_UpdateDate = dbo.GetThaiDate(),LastUpdate = dbo.GetThaiDate(),ClientId = Null WHERE Teacher_Id = '" & TeacherId & "' AND TR_IsActive = '1'"
            ObjDB.ExecuteWithTransection(sql)
            'Insert TeacherRoom ใหม่
            If LoopInsert360TeacherRoom(TeacherId, TeacherClass, Room, SchoolId, _DB) = "-1" Then
                Return "-1"
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

    End Function

    ''' <summary>
    ''' ทำการ update ข้อมูลของนักเรียนให้ข้อมูลเป็นข้อมูลปัจจุบันที่กรอกเข้ามา
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="StudentCode">รหัสประจำตัวนักเรียน</param>
    ''' <param name="StudentClass">ชั้น</param>
    ''' <param name="Room">ห้อง</param>
    ''' <param name="NumberInRoom">เลขที่</param>
    ''' <param name="ObjDB">ตัวแปร connection</param>
    ''' <param name="Gender">เพศ</param>
    ''' <returns>String:1,-1</returns>
    ''' <remarks></remarks>
    Private Function UpdateStudentInfo(ByVal SchoolId As String, ByVal StudentId As String, ByVal StudentCode As String, ByVal StudentClass As String, ByVal Room As String, ByVal NumberInRoom As String, ByRef ObjDB As ClsConnect, ByVal Gender As String) As String
        Try
            Dim IsValidate As String = ""
            Dim sql As String
            'Update 360student studentcode,currentno,class,room
            Dim ClassArr = StudentClass.Trim().Split("|")
            Dim CurrentClass As String = FindClassName(ClassArr(0)).ToString
            If CurrentClass = "" Then
                Return "-1"
            End If

            Dim CurrentRoom As String = "/" & ObjDB.CleanString(Room.Trim())
            sql = " UPDATE dbo.t360_tblStudent SET Student_Code = '" & ObjDB.CleanString(StudentCode.Trim()) & "',Student_CurrentNoInRoom = '" & ObjDB.CleanString(NumberInRoom.Trim()) & "' " &
                  " ,Student_CurrentClass = '" & CurrentClass & "',Student_CurrentRoom = '" & CurrentRoom & "',Student_Gender = '" & Gender & "',Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Student_Id = '" & StudentId & "' AND Student_IsActive = '1'"
            ObjDB.ExecuteWithTransection(sql)

            ' dache -> ปรับเพิ่มถ้าเข้าเงื่อนไขนี้ คือนักเรียนคนเดิม ไม่ต้องทำอะไร นอกจาก update ชื่อ-สกุล เพศ
            ''Update 360studentroom ของเก่าปรับ IsActive เป็น 0
            'sql = "UPDATE dbo.t360_tblStudentRoom SET SR_IsActive = '0',LastUpdate = dbo.GetThaiDate(),ClientId = Null WHERE Student_Id = '" & StudentId & "' AND SR_IsActive = '1'"
            'ObjDB.ExecuteWithTransection(sql)
            ''วนลูป Insert ตาราง 360 studentroom ใหม่
            'IsValidate = LoopInsertTo360StudentRoom(SchoolId, StudentId, NumberInRoom, StudentClass, CurrentRoom, ObjDB)
            'If IsValidate = "-1" Or IsValidate = "" Then
            '    Return "-1"
            'End If


            Return "1"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function



    Private Function UpdateStudentInfo(ByVal StudentId As String, ByVal StudentFirstName As String, ByVal StudentLastName As String, ByVal Gender As String, ByRef ObjDB As ClsConnect) As String
        Try
            Dim sql As String
            sql = String.Format("UPDATE t360_tblStudent SET Student_FirstName = '{0}',Student_LastName = '{1}', Student_Gender = '{2}',Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Student_Id = '{3}' AND Student_IsActive = 1;", StudentFirstName, StudentLastName, Gender, StudentId)
            ObjDB.ExecuteWithTransection(sql)

            Return "1"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function
    Private Function LoopInsertTo360StudentRoom(ByVal SchoolId As String, ByVal StudentID As String, ByVal NumberInRoom As String, ByVal StudentClass As String, ByVal CurrentRoom As String, ByRef ObjDB As ClsConnect)
        Try
            Dim sql As String
            Dim ClassArr = StudentClass.Trim().Split("|")
            Dim CalendarId As String = GetCalendarID(SchoolId)
            If CalendarId = "" Then Return "-1"

            sql = "select Room_Id from t360_tblRoom where Class_Name = '" & FindClassName(ClassArr(0)) & "' and Room_Name = '" & CurrentRoom & "' and School_Code = '" & SchoolId & "' and Room_IsActive = 1;"
            Dim CurrentRoomId As String = ObjDB.ExecuteScalarWithTransection(sql)
            If CurrentRoomId = "" Then Return "-1"

            For i = 0 To ClassArr.Length - 1
                Dim ClassStr As String = FindClassName(ClassArr(i))
                Dim AcademicYear As String = GetAcademicYear()
                sql = " INSERT INTO dbo.t360_tblStudentRoom( School_Code ,Student_Id ,Student_NoInRoom " &
                      " ,Class_Name ,Room_Name ,SR_MoveDate ,SR_AcademicYear,Calendar_Id,Room_Id ,SR_MoveType ,SR_IsActive) " &
                      " VALUES  ( '" & SchoolId & "' , '" & StudentID & "' ,'" & ObjDB.CleanString(NumberInRoom).Trim() & "' ,'" & ClassStr & "' ,'" & CurrentRoom & "' " &
                      " , dbo.GetThaiDate() ,'" & AcademicYear & "' ,'" & CalendarId & "' ,'" & CurrentRoomId & "', 1 , 1 ) " 'ไม่รู้ฟิลด์ AcademicYear,SR_MoveType
                ObjDB.ExecuteWithTransection(sql) 'Insert ลงตาราง t360_tblStudentRoom 
            Next
            Return "1"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function

    ''' <summary>
    ''' หา StudentId โดยใช้รหัสโรงเรียน และ รหัสประจำตัวนักเรียน
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <param name="StudentCode">รหัสประจำตัวนักเรียน</param>
    ''' <returns>String:Student_Id</returns>
    ''' <remarks></remarks>
    Private Function GetStudentIdByStudentCode(ByVal SchoolId As String, ByVal StudentCode As String) As String
        Dim CurrentStudentId As String = ""
        Dim sql As String = " SELECT Student_Id FROM dbo.t360_tblStudent WHERE School_Code = '" & SchoolId & "' AND Student_Code = '" & _DB.CleanString(StudentCode.Trim()) & "' "
        CurrentStudentId = _DB.ExecuteScalar(sql)
        ClsLog.Record(" - ClassDroidPad.GetStudentIdByStudentCode : sql = " & sql)
        ClsLog.Record(" - ClassDroidPad.GetStudentIdByStudentCode : return = " & CurrentStudentId)

        GetStudentIdByStudentCode = CurrentStudentId
        Return GetStudentIdByStudentCode
    End Function

    Private Function GetStudentIdByStudentCode(ByVal SchoolId As String, ByVal StudentCode As String, StudentNumber As String, ClassName As String, RoomName As String) As String
        Dim CurrentStudentId As String = ""
        Dim CalendarId As String = GetCalendarID(SchoolId)

        Dim sql As New StringBuilder()
        sql.Append(" SELECT s.Student_Id FROM t360_tblStudent s INNER JOIN t360_tblStudentRoom sr ON s.Student_Id = sr.Student_Id ")
        sql.Append(String.Format(" WHERE s.Student_Code = '{0}' AND s.Student_CurrentNoInRoom = '{1}' AND s.Student_CurrentClass = '{2}' AND s.Student_CurrentRoom = '/{3}' ", StudentCode, StudentNumber, FindClassName(ClassName), RoomName))
        sql.Append(String.Format(" AND s.School_Code = '{0}' AND sr.Calendar_Id = '{1}' AND s.Student_IsActive = 1;", SchoolId, CalendarId))

        CurrentStudentId = _DB.ExecuteScalar(sql.ToString())
        ClsLog.Record(" - ClassDroidPad.GetStudentIdByStudentCode : sql = " & sql.ToString())
        ClsLog.Record(" - ClassDroidPad.GetStudentIdByStudentCode : return = " & CurrentStudentId)

        Return CurrentStudentId
    End Function

    Private Function GetLastChoiceFromApplication(ByVal QuizId As String)

        If QuizId = "" Or QuizId Is Nothing Then
            Return "-1"
        End If
        If HttpContext.Current.Application(QuizId & "_CurrentQuizState") Is Nothing Then
            Return "-1"
        End If
        QuizId = QuizId.ToUpper()
        Dim ArrHastable As Hashtable = CType(HttpContext.Current.Application(QuizId & "_CurrentQuizState"), Hashtable)
        If Not IsNothing(ArrHastable(QuizId & "_CurrentQuestionNo")) Then
            Dim cqn As Integer = Integer.Parse(ArrHastable(QuizId & "_CurrentQuestionNo"))
            Return cqn

        Else
            Return 1
        End If
    End Function

    Public Function GetAnsStateFromApplication(ByVal QuizId As String)

        If QuizId = "" Or QuizId Is Nothing Then
            Return "-1"
        End If
        If HttpContext.Current.Application(QuizId & "_CurrentQuizState") Is Nothing Then
            Return "-1"
        End If
        QuizId = QuizId.ToUpper()
        Dim ArrHastable As Hashtable = CType(HttpContext.Current.Application(QuizId & "_CurrentQuizState"), Hashtable)
        Dim cans As String = "-1"
        cans = ArrHastable(QuizId & "_AnswerState")

        'shin
        ' cans = ArrHastable(ArrHastable.Keys(1))

        If cans Is Nothing Then
            Return "-1"
        Else
            Return cans
        End If


    End Function

    Public Sub RemoveAndAddNewQQNoToApplication(ByVal QuizId As String, ByVal ExamNum As String)
        If QuizId IsNot Nothing Or QuizId <> "" Or ExamNum <> "" Or ExamNum IsNot Nothing Then
            Dim ArrHastable As New Hashtable

            If HttpContext.Current.Application(QuizId & "_CurrentQuizState") Is Nothing Then
                HttpContext.Current.Application(QuizId & "_CurrentQuizState") = ArrHastable
            End If
            QuizId = QuizId.ToUpper()
            ArrHastable = CType(HttpContext.Current.Application(QuizId & "_CurrentQuizState"), Hashtable)
            ArrHastable.Remove(QuizId & "_CurrentQuestionNo")
            ArrHastable.Add(QuizId & "_CurrentQuestionNo", ExamNum)
            HttpContext.Current.Application(QuizId & "_CurrentQuizState") = ArrHastable
        End If
    End Sub

    Public Sub RemoveAndAddNewAnsState(ByVal QuizId As String, ByVal AnsState As String)
        If QuizId IsNot Nothing Or QuizId <> "" Or AnsState IsNot Nothing Or AnsState <> "" Then
            Dim ArrHastable As New Hashtable
            If HttpContext.Current.Application(QuizId & "_CurrentQuizState") Is Nothing Then
                HttpContext.Current.Application(QuizId & "_CurrentQuizState") = ArrHastable
            End If
            QuizId = QuizId.ToUpper()
            ArrHastable = CType(HttpContext.Current.Application(QuizId & "_CurrentQuizState"), Hashtable)
            ArrHastable.Remove(QuizId & "_AnswerState")
            ArrHastable.Add(QuizId & "_AnswerState", AnsState)
            HttpContext.Current.Application(QuizId & "_CurrentQuizState") = ArrHastable
        End If
    End Sub

    Public Function GetQsetTypeFromQuestionId(ByVal QuestionId As String)

        If QuestionId <> "" Or QuestionId IsNot Nothing Then
            Dim sql As String = " SELECT tblQuestionSet.QSet_Type FROM tblQuestion INNER JOIN " &
                                " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " &
                                " WHERE     (tblQuestion.Question_Id = '" & QuestionId & "') "
            Dim QsetType As String = _DB.ExecuteScalar(sql)
            Return QsetType
        Else
            Return ""
        End If

    End Function

    Public Function GetSR_IdFromStudentId(ByVal StudentId As String) As String

        Dim Sr_Id As String = ""
        If StudentId Is Nothing Or StudentId = "" Then
            Return Sr_Id
        End If

        Dim sql As String = " SELECT SR_ID FROM dbo.t360_tblStudentRoom WHERE Student_Id = '" & StudentId & "' AND SR_IsActive = '1' "
        Sr_Id = _DB.ExecuteScalar(sql)
        Return Sr_Id

    End Function

    Private Function UpdateStudentCheckTabletReady(ByVal QuizId As String, ByVal PlayerId As String)
        If QuizId IsNot Nothing And QuizId <> "" And PlayerId IsNot Nothing And PlayerId <> "" Then
            Try
                Dim sql As String = " UPDATE dbo.tblQuizSession SET IsActive = '1',LastUpdate = dbo.GetThaiDate(),ClientId = Null WHERE Quiz_Id = '" & QuizId & "' AND Player_Id = '" & PlayerId & "' "
                _DB.Execute(sql)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return "-1"
            End Try
        Else
            Return "-1"
        End If
        Return "1"
    End Function

    Public Function GetPlayerIdByDeviceUniqeId(ByVal QuizId As String, ByVal DeviceUniqueID As String)
        Dim PlayerId As String = ""
        If QuizId IsNot Nothing And QuizId <> "" And DeviceUniqueID IsNot Nothing And DeviceUniqueID <> "" Then

            Dim sql As String = " SELECT Selfpace FROM dbo.tblQuiz WHERE Quiz_Id = '" & QuizId & "' "
            Dim CheckIsSelfPace As String = _DB.ExecuteScalar(sql)

            If CheckIsSelfPace = "True" Then
                sql = " SELECT Player_Id FROM dbo.uvw_GetQuizIdByTabSerialWithoutQuizScore WHERE " &
                      " Quiz_Id = '" & QuizId & "' AND Tablet_SerialNumber = '" & DeviceUniqueID & "' "
                PlayerId = _DB.ExecuteScalar(sql)
            Else
                sql = " SELECT Player_Id FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE Tablet_SerialNumber = '" & DeviceUniqueID & "' " &
                  " AND Quiz_Id = '" & QuizId & "' "
                PlayerId = _DB.ExecuteScalar(sql)
            End If

        Else
            Return "-1"
        End If
        Return PlayerId
    End Function

    ''' <summary>
    ''' ทำการสร้าง Array ที่มี รหัสโรงเรียน และ วันที่/เวลา ปัจจุบัน
    ''' </summary>
    ''' <param name="SchoolId">รหัสโรงเรียน</param>
    ''' <returns>ArrayList</returns>
    ''' <remarks></remarks>
    Private Function GenArrayListSchoolCode(ByVal SchoolId As String) As ArrayList

        Dim ArrSchoolId As New ArrayList
        If SchoolId Is Nothing Or SchoolId = "" Then
            Return ArrSchoolId
        End If

        ArrSchoolId.Add(_DB.CleanString(SchoolId.Trim()))
        ArrSchoolId.Add(Now)
        Return ArrSchoolId

    End Function

    ''' <summary>
    ''' ทำการเช็ค Array ที่อยู่ใน application ในขั้นตอนการลงทะเบียนขั้นแรก ว่ากว่าจะมาถึงขั้นตอนนี้ต้องไม่เกิน 5 นาที
    ''' </summary>
    ''' <param name="DeviceUniqueID">รหัสเครื่อง Tablet</param>
    ''' <returns>String:SchoolCode,-1</returns>
    ''' <remarks></remarks>
    Private Function GetSchoolCodeFromApplication(ByVal DeviceUniqueID As String) As String

        If DeviceUniqueID Is Nothing Or DeviceUniqueID = "" Then
            Return "-1"
        End If

        Dim ArrSchoolId As New ArrayList
        ArrSchoolId = CType(HttpContext.Current.Application(DeviceUniqueID.Trim()), ArrayList)

        If ArrSchoolId Is Nothing OrElse ArrSchoolId.Count = 0 Then
            ClsLog.Record(" - ClassDroidPad.GetSchoolCodeFromApplication : return -1 --> ArrSchoolId Is Nothing || = 0")

            Return "-1"
        End If
        Dim DateCompare As Date = ArrSchoolId(1)
        Dim SchoolCode As String = ""
        If Now.AddMinutes(-5) > DateCompare Then
            ClsLog.Record(" - ClassDroidPad.GetSchoolCodeFromApplication : return -1 --> Now.AddMinutes(-5) > DateCompare ")

            Return "-1"
        Else
            SchoolCode = ArrSchoolId(0)
        End If
        ClsLog.Record(" - ClassDroidPad.GetSchoolCodeFromApplication : Return = " & SchoolCode)

        Return SchoolCode

    End Function

    Public Function GetUserIdByDeviceId(ByVal DeviceUniqueId As String)

        Dim UserId As String = ""
        If DeviceUniqueId Is Nothing Or DeviceUniqueId = "" Then
            Return UserId
        End If

        Dim sql As String = " SELECT tblUser.GUID FROM t360_tblTablet INNER JOIN " &
                            " t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id INNER JOIN " &
                            " tblUser ON t360_tblTabletOwner.Owner_Id = tblUser.GUID " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & _DB.CleanString(DeviceUniqueId.Trim()) & "') AND t360_tblTabletOwner.TabletOwner_IsActive = 1 "

        UserId = _DB.ExecuteScalar(sql)
        Return UserId

    End Function

    Public Function SetClosePracticeQuiz(ByVal DeviceId As String)

        Dim sql As String = " SELECT TOP 1 Quiz_Id FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE Tablet_SerialNumber = '" & DeviceId & "' AND IsPracticeMode = 1 " &
                            " AND LastUpdate > DATEADD(MINUTE,-15,dbo.GetThaiDate()) ORDER BY LastUpdate DESC "
        Dim QuizId As String = _DB.ExecuteScalar(sql)

        If QuizId <> "" Then
            sql = " UPDATE dbo.tblQuiz SET EndTime = dbo.GetThaiDate(),Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Quiz_Id = '" & QuizId & "' "
        End If

        Return "Complete"

    End Function


    Private Function CheckSession(ByVal UserId As String) As Boolean

        Dim IsManySession As Boolean = False
        If HttpContext.Current.Application(UserId) Is Nothing Then
            Return IsManySession
        End If

        Dim NewArr As ArrayList = HttpContext.Current.Application(UserId)
        Dim ArrMemory As New ArrayList

        'Loop เพื่อหาว่ามี Session ไหนที่น้อยกว่า 120 นาทีแล้วมั่งเอาไปใส่ใน Array อันใหม่
        For index = 0 To NewArr.Count - 1
            Dim objClsSessInFo As ClsSessionInFo = NewArr(index)
            If DateTime.Now > objClsSessInFo.TimeStamp Then
                If DateDiff(DateInterval.Minute, objClsSessInFo.TimeStamp, DateTime.Now) < 120 Then
                    ArrMemory.Add(NewArr(index))
                End If
            End If
            objClsSessInFo = Nothing
        Next

        ''ลบข้อมูลแถวที่เกิน 120 นาทีออก
        'For Each r In ArrMemory
        '    NewArr.RemoveAt(r)
        'Next

        'วน Update Index ใหม่หมด
        Dim RunIndex As Integer = 1
        For Each allObj In ArrMemory
            Dim objClsSessInFo As ClsSessionInFo = allObj
            objClsSessInFo.Index = RunIndex
            RunIndex += 1
            objClsSessInFo = Nothing
        Next

        'เช็คจำนวนแถวที่เหลืออยู่ใน Array
        If ArrMemory.Count > 0 Then
            HttpContext.Current.Application(UserId) = ArrMemory
            Return True
        Else
            Return False
        End If

    End Function


    Private Function AddNewSession(ByVal UserId As String)

        Dim ValueSession As String = ""
        Dim ArrData As New ArrayList
        Dim objClsSessInFo As New ClsSessionInFo
        'Dim pkGuid = System.Guid.NewGuid()
        Dim clsSelectSess As New ClsSelectSession()
        Dim pkGuid = clsSelectSess.Number4Digit()
        'If SoundlabName = "" Then
        '    HttpContext.Current.Session("selectedSession") = pkGuid
        '    ValueSession = pkGuid.ToString()
        'Else
        '    HttpContext.Current.Session("selectedSession") = SoundlabName
        '    ValueSession = SoundlabName
        'End If
        HttpContext.Current.Session("selectedSession") = pkGuid
        ValueSession = pkGuid.ToString()
        objClsSessInFo.PKInfo = pkGuid
        objClsSessInFo.Index = 1
        objClsSessInFo.TimeStamp = DateTime.Now
        objClsSessInFo.CurrentPage = "~/Activity/Alternative_Pad.aspx"
        ArrData.Add(objClsSessInFo)
        HttpContext.Current.Application(UserId) = ArrData
        Return ValueSession

    End Function

    ''' <summary>
    ''' get calendar from date now 
    ''' </summary>
    ''' <param name="SchoolID">รหัสโรงเรียน</param>
    ''' <returns>String:Calendar_Id</returns>
    ''' <remarks></remarks>
    Private Function GetCalendarID(ByVal SchoolID As String) As String
        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "'; "
        Dim db As New ClassConnectSql()
        GetCalendarID = db.ExecuteScalar(sql)
    End Function

    Private Function IsInSoundLab(ByVal DeviceId As String) As String


        Dim sql As String = " SELECT tblTabletLab.TabletLab_Id FROM t360_tblTablet INNER JOIN " &
                            " tblTabletLabDesk ON t360_tblTablet.Tablet_Id = tblTabletLabDesk.Tablet_Id INNER JOIN " &
                            " tblTabletLab ON tblTabletLabDesk.TabletLab_Id = tblTabletLab.TabletLab_Id " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') "
        Dim LabId As String = _DB.ExecuteScalar(sql)

        If LabId <> "" Then
            Return LabId
        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' หาค่า info ต่างๆของนักเรียนที่เป็น JsonString พวก จำนวนเหรียญ,เลเวล,รายละเอียดต่างๆ
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="ObjConn">ตัวแปร Connection</param>
    ''' <returns>String:ItemJsonData</returns>
    ''' <remarks></remarks>
    Private Function GetItemJsonDataByStudentId(ByVal StudentId As String, Optional ByRef ObjConn As ClassConnectSql = Nothing) As String
        If StudentId <> "" And StudentId IsNot Nothing Then
            Dim sql As String = " SELECT ItemJsonData FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' AND Student_IsActive = 1 "
            Dim ItemJsonData As String = ""
            If ObjConn IsNot Nothing Then
                ItemJsonData = ObjConn.ExecuteScalarWithTransection(sql)
            Else
                ItemJsonData = _DB.ExecuteScalar(sql)
            End If
            If ItemJsonData <> "" Then
                ItemJsonData = ItemJsonData.Remove(0, 1)
                ItemJsonData = ItemJsonData.Substring(0, ItemJsonData.Length - 1)
                ItemJsonData = ItemJsonData.Substring(0)
            Else
                ItemJsonData = """user_details"": {}"
            End If
            Return ItemJsonData
        Else
            Return ""
        End If
    End Function

    Public Sub EnableDebug(ByVal ResponseString As String)
        Dim IsEnableDebugMode As String = HttpContext.Current.Application("DebugAPI")
        If IsEnableDebugMode <> "false" Then
            HttpContext.Current.Response.Write(ResponseString)
        End If
    End Sub

#Region "GetCurrentStatus"

    ''' <summary>
    ''' ทำกาานำข้อมูล เหรียญ/Item ของนักเรียนมา update ลง DB 
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="ItemId">Hashtable ที่มี Item ของนักเรียน</param>
    ''' <param name="ResultGold">จำนวนเหรียญทองทั้งหมด</param>
    ''' <param name="ResultSilver">จำนวนเหรียญเงินทั้งหมด</param>
    ''' <param name="ResultDiamond">จำนวนเพชรทั้งหมด</param>
    ''' <param name="ResultRecieveDiamond">จำนวนเพชรที่ได้เพิ่มมา</param>
    ''' <param name="JsonData">ข้อมูลทั้งหมดที่เป็นแบบ JsonString</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetCurrentStatus(ByVal DeviceId As String, ByVal ItemId As Hashtable, ByVal ResultGold As Integer, ByVal ResultSilver As Integer, ByVal ResultDiamond As Integer, ByVal ResultRecieveDiamond As Integer, ByVal JsonData As String) As String
        If DeviceId = "" Or DeviceId Is Nothing Then
            Return "-1"
        End If
        Dim StudentId As String = GetStudentIdByDeviceId(_DB.CleanString(DeviceId))
        Dim SchoolId As String = GetSchoolIdByDeviceId(_DB.CleanString(DeviceId))
        If StudentId <> "" And SchoolId <> "" Then
            Try
                If HttpContext.Current.Session("SchoolCode") Is Nothing Then
                    HttpContext.Current.Session("SchoolCode") = SchoolId
                End If
                'เปิด Transaction
                _DB.OpenWithTransection()
                Dim sql As String = " select COUNT(*) from tblStudentPoint where Student_Id = '" & StudentId & "' "
                Dim CheckIsInStudentPoint As String = _DB.ExecuteScalarWithTransection(sql)
                'หาก่อนว่าเคยมีข้อมูลของนักเรียนคนนี้อยู่ใน tblStudentPoint หรือยัง ถ้ายังไม่มีต้อง Insert ข้อมูลลง tblStudentPoint ก่อน
                If CInt(CheckIsInStudentPoint) = 0 Then
                    'Insert tblStudentPoint
                    If InsertStudentPoint(StudentId, _DB) = -2 Then
                        _DB.RollbackTransection()
                        Return "-2 Insert-tblStdentPoint ไม่ได้"
                    End If
                End If

                'Update Userdata.Json ลง DB
                If UpdateUserDataJson(StudentId, JsonData, _DB) = -2 Then
                    _DB.RollbackTransection()
                    Return "-2 Update Json-String ลง DB ไม่ได้"
                End If

                'ทำการหาจำนวนเหรียญต่างๆในปัจจุบันก่อน
                Dim dt As DataTable = GetDtSilverAndGoldCoin(StudentId, _DB)
                Dim TotalSilver As Integer = 0
                Dim TotalGold As Integer = 0
                Dim TotalDiamond As Integer = 0
                Dim TotalReceiveDiamond As Integer = 0

                If dt.Rows.Count > 0 Then
                    'ทำการนำจำนวนเหรียญในปัจจุบันมา + กับจำนวนเหรียญที่ได้มาเพิ่ม
                    TotalSilver = dt.Rows(0)("Silver") - ResultSilver
                    TotalGold = dt.Rows(0)("Gold") - ResultGold
                    TotalDiamond = dt.Rows(0)("Diamond") - ResultDiamond
                    TotalReceiveDiamond = dt.Rows(0)("TotalDiamond") + ResultRecieveDiamond

                    ClsLog.Record("TotalSilver = " & dt.Rows(0)("Silver") & " - " & ResultSilver)
                    ClsLog.Record("TotalGold = " & dt.Rows(0)("Gold") & " - " & ResultGold)
                    ClsLog.Record("TotalDiamond = " & dt.Rows(0)("Diamond") & " - " & ResultDiamond)

                    'Update ข้อมูลลง tmp ไปก่อน
                    Dim tmpId As String = InsertDataIntmpGetCurrentStatus(TotalGold, TotalSilver, TotalDiamond, TotalReceiveDiamond, _DB)
                    If tmpId = "" Then
                        _DB.RollbackTransection()
                        Return "-2 Update ข้อมูลที่ส่งมาลง tmp ไม่ได้"
                    End If

                    'If UpdateCurrentGoldAndSilver(StudentId, TotalSilver, TotalGold, _DB) = -2 Then
                    '    _DB.RollbackTransection()
                    '    Return -2
                    'End If

                    'Update tblStudentItem set IsActive = 0 ก่อนที่จะ Insert อันล่าสุดลงไป
                    If SetIsActiveFalseStudentItem(StudentId, _DB) = -2 Then
                        _DB.RollbackTransection()
                        Return "-2 update Item เก่าของนักเรียนให้ Isactive = 0 ไม่ได้"
                    End If

                    'Update Student_AvatarSeqNo ให้เป็นค่าติดลบก่อน
                    If UpdateStudentAvatarSeqNo(StudentId, _DB) = "-2" Then
                        _DB.RollbackTran()
                        Return "-2 Update Student_AvatarSeqNo ให้เป็นค่าติดลบ ไม่ได้"
                    End If

                    If ItemId.Count > 0 Then
                        'Loop Insert tblStudentItem , เงื่อนไขการจบ loop คือวนจนครบทุก item
                        For Each StudentItem As DictionaryEntry In ItemId
                            'ทำการหา ItemId ของแต่ละ Item
                            Dim ShopItemId As String = GetShopItemIdByItemId(StudentItem.Key)
                            If ShopItemId <> "" Then
                                'ทำการ Insert ข้อมูล Item นี้ผูกกับนักเรียนคนนี้
                                If InsertLastStudentItem(StudentId, ShopItemId, StudentItem.Value, _DB) = -2 Then
                                    _DB.RollbackTransection()
                                    Return "-2 Error ตอน Insert StudentItem ล่าสุด ShopItemId=" & ShopItemId & ""
                                End If
                            Else
                                _DB.RollbackTransection()
                                Return "-2 ไม่มี Item " & StudentItem.Key & " ใน tblShopItem "
                            End If
                        Next
                    End If

                    'จบ ปิด Transaction
                    _DB.CommitTransection()
                    'ให้ไปเรียก GenAvatar URL ที่ WebService ส่ง Parameter Student_Id เพื่อทำการสร้างรูป avatar ของนักเรียน
                    'webclient.post ("url?parme=AAA")
                    If GenAvatarPicture(StudentId) = "GenAvatarError" Then
                        Return HttpContext.Current.Session("StrError").ToString()
                    End If

                    '{"Param":{"tmpId":"tmpId","ResultGold":"50","ResultSilver":"20"}}
                    Dim StrReturn As String = "{""Param"":{""tmpId"":""" & tmpId & """,""ResultGold"":""" & TotalGold & """,""ResultSilver"":""" & TotalSilver & """,""ResultDiamond"":""" & TotalDiamond & """}}"
                    Return StrReturn
                Else
                    Return "-1 Get เหรียญเงิน,ทอง ใน tblStudentPoint ไม่ได้"
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException

                'Error RollBackTransaction
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                EnableDebug(ex.ToString())
                If ex.ToString() <> "GenAvatarError" Then
                    _DB.RollbackTransection()
                End If
                Return HttpContext.Current.Session("StrError").ToString()
            End Try
        Else
            Return "-1 ไม่มี StudentId,SchoolId"
        End If
    End Function

    Private Function ReplaceCoinToNewValue(ByVal InputJsonData As String, ByVal StrFindToReplace As String, ByVal NewCoinValue As String) As String
        If InputJsonData <> "" Then
            Dim StrFindPosition As Integer = InputJsonData.IndexOf(StrFindToReplace)
            If StrFindPosition <> -1 Then
                Dim ColonPosition As Integer = InputJsonData.IndexOf(":", StrFindPosition) + 1
                If ColonPosition <> -1 Then
                    Dim CommaPosition As Integer = InputJsonData.IndexOf(",", ColonPosition)
                    If CommaPosition <> -1 Then
                        InputJsonData = InputJsonData.Remove(ColonPosition, CommaPosition - ColonPosition)
                        InputJsonData = InputJsonData.Insert(ColonPosition, NewCoinValue)
                    End If
                End If
            End If
        End If
        Return InputJsonData
    End Function

    ''' <summary>
    ''' Function Update User Data Json 
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="JsonData">ข้อมูลนักเรียนที่เป็น JsonString</param>
    ''' <param name="ObjDb">ตัวแปร Connection</param>
    ''' <returns>Integer:1,-2</returns>
    ''' <remarks></remarks>
    Private Function UpdateUserDataJson(ByVal StudentId As String, ByVal JsonData As String, ByRef ObjDb As ClassConnectSql) As Integer
        Dim sql As String = " UPDATE dbo.t360_tblStudent SET ItemJsonData = '" & ObjDb.CleanString(JsonData) & "',Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Student_Id = '" & StudentId & "' "
        Try
            ObjDb.ExecuteWithTransection(sql)
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -2
        End Try
    End Function

    ''' <summary>
    ''' Function ส่ง StudentId ไป Gen รูปที่หน้า MergePNG
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function GenAvatarPicture(ByVal StudentId As String) As String
        Try
            EnableDebug("เข้า GenAvatarPicture<br />" & StudentId)
            Using Client As New Net.WebClient
                Dim param As New Specialized.NameValueCollection
                param.Add("StudentId", StudentId)
#If F5 = "1" Then
                Dim responsebytes = Client.UploadValues("http://localhost:18615/GenAvatar/MergePNG.aspx", "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
#Else
                'TEST
                'Dim responsebytes = Client.UploadValues("http://localhost:18615/GenAvatar/MergePNG.aspx", "POST", param)
                'TODO: อนาคต อาจแยกเป็นอีกเครื่องนึง เช่น ใช้ secondary database (readonly) ของ azure เป็นตัว gen รูปแทน
                'Dim responsebytes = Client.UploadValues("https://maxonetwebapp.azurewebsites.net/GenAvatar/MergePNG.aspx", "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
                Dim responsebytes = Client.UploadValues("http://192.168.111.73/maxonet/GenAvatar/MergePNG.aspx", "POST", param)
                ' Dim responsebytes = Client.UploadValues("http://maxonet.iknow.co.th/GenAvatar/MergePNG.aspx", "POST", param)
#End If
                Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
                Return responsebody
            End Using
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            HttpContext.Current.Session("StrError") &= "Error ตอนที่เข้ามาใน Function GenAvatarPicture อาจเพราะ URL ที่ POST ไปหน้า MergePNG ผิด <br />"
            'Throw New Exception("Error ตอนที่เข้ามาใน Function GenAvatarPicture อาจเพราะ URL ที่ POST ไปหน้า MergePNG ผิด")
        End Try
    End Function

    ''' <summary>
    ''' ทำการ update ข้อมูล AvatarseqNo ให้เป็น -1 ไปก่อน เพื่อเดียวจะต้อง update seq ใหม่
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="ObjDb">ตัวแปร Connection</param>
    ''' <returns>String,Complete,-2</returns>
    ''' <remarks></remarks>
    Private Function UpdateStudentAvatarSeqNo(ByVal StudentId As String, ByRef ObjDb As ClassConnectSql) As String
        Try
            Dim sql As String = " UPDATE dbo.t360_tblStudent SET Student_AvatarSeqNo *= -1,Lastupdate = dbo.GetThaiDate(),ClientId = Null WHERE Student_Id = '" & StudentId & " ' "
            ObjDb.ExecuteWithTransection(sql)
            Return "Complete"
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' ทำการนำข้อมูลเหรียญล่าสุดจาก tmp มา update เข้า db จริงที่ tblStudentPoint
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="tmpId">รหัสของ tmpId</param>
    ''' <returns>String:OK,-1,-2</returns>
    ''' <remarks></remarks>
    Public Function UpdatetmpToRealDb(ByVal DeviceId As String, ByVal tmpId As String) As String

        If tmpId <> "" And tmpId IsNot Nothing And DeviceId IsNot Nothing And DeviceId <> "" Then
            Dim _DB As New ClassConnectSql()
            'รหัสนักเรียน
            Dim StudentId As String = GetStudentIdByDeviceId(_DB.CleanString(DeviceId))
            'ทำการ select จำนวนเหรียญล่าสุดจาก tmp มาก่อน
            Dim sql As String = " SELECT ResultGold,ResultSilver,ResultDiamond,ResultRecieveDiamond FROM dbo.tmp_GetcurrentStatus WHERE FlagId = '" & _DB.CleanString(tmpId) & "' "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                Try
                    _DB.OpenWithTransection()

                    'เช็คก่อนว่า ถ้า TotalSilver = 0 ต้อง Update ให้เท่ากับ ResultSilver
                    sql = " SELECT TotalSilver FROM dbo.tblStudentPoint WHERE Student_Id = '" & StudentId & "' "
                    Dim TotalSilver As Integer = 0
                    Dim CheckTotalSilver As String = _DB.ExecuteScalarWithTransection(sql)
                    If CheckTotalSilver <> "" Then
                        TotalSilver = CInt(CheckTotalSilver)
                    End If
                    If TotalSilver = 0 Then
                        sql = " UPDATE dbo.tblStudentPoint SET TotalSilver  = '" & dt.Rows(0)("ResultSilver") & "',Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId & "' "
                        _DB.ExecuteWithTransection(sql)
                    End If

                    'เช็คก่อนว่า ถ้า TotalGold = 0 ต้อง Update ให้เท่ากับ ResultGold
                    sql = " SELECT TotalGold FROM dbo.tblStudentPoint WHERE Student_Id = '" & StudentId & "' "
                    Dim TotalGold As Integer = 0
                    Dim CheckTotalGold As String = _DB.ExecuteScalarWithTransection(sql)
                    If CheckTotalGold <> "" Then
                        TotalGold = CInt(CheckTotalGold)
                    End If
                    If TotalGold = 0 Then
                        sql = " UPDATE dbo.tblStudentPoint SET TotalGold  = '" & dt.Rows(0)("ResultGold") & "',Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId & "' "
                        _DB.ExecuteWithTransection(sql)
                    End If

                    'เช็คก่อนว่า ถ้า TotalDiamond = 0 ต้อง Update ให้เท่ากับ ResultDiamond
                    sql = " SELECT TotalDiamond FROM dbo.tblStudentPoint WHERE Student_Id = '" & StudentId & "' "
                    Dim TotalDiamond As Integer = 0
                    Dim CheckTotalDiamond As String = _DB.ExecuteScalarWithTransection(sql)
                    If CheckTotalDiamond <> "" Then
                        TotalDiamond = CInt(CheckTotalDiamond)
                    End If
                    If TotalDiamond = 0 Then
                        sql = " UPDATE dbo.tblStudentPoint SET TotalDiamond  = '" & dt.Rows(0)("ResultDiamond") & "',Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId & "' "
                        _DB.ExecuteWithTransection(sql)
                    End If

                    'Update เหรียญล่าสุดขึ้น DB จริง
                    sql = " UPDATE dbo.tblStudentPoint SET Gold = " & dt.Rows(0)("ResultGold") & ", Silver = " & dt.Rows(0)("ResultSilver") & ",Diamond = " & dt.Rows(0)("ResultDiamond") & ",LastUpdate = dbo.GetThaiDate(),TotalDiamond = " & dt.Rows(0)("ResultRecieveDiamond") & " WHERE Student_Id = '" & StudentId & "' "
                    _DB.ExecuteWithTransection(sql)

                    'UPdate TotalScore ที่ tblStudentPoint
                    sql = " UPDATE dbo.tblStudentPoint SET TotalScore = (TotalSilver + (TotalGold*10) + (TotalDiamond*100)),Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '" & StudentId & "'  "
                    _DB.ExecuteWithTransection(sql)

                    'Update Point_Level ที่ tblStudentPoint
                    sql = " UPDATE dbo.tblStudentPoint SET Point_Level = (select case when round(log(case when totalscore is null then 1 when totalscore = 0 then 1 else totalscore end ),0) - 4 < 1 then 1 " &
                          " else round(log(case when totalscore is null then 1 when totalscore = 0 then 1 else totalscore end ),0) - 4 end from dbo.tblstudentpoint " &
                          " WHERE Student_Id = '" & StudentId & "'),Lastupdate = dbo.GetThaiDate() WHERE dbo.tblStudentPoint.Student_Id = '" & StudentId & "' "
                    _DB.ExecuteWithTransection(sql)




                    'Update Isactive ของ tmp ให้เป็น 1
                    sql = " UPDATE dbo.tmp_GetcurrentStatus SET IsActive = 1,Lastupdate = dbo.GetThaiDate() WHERE FlagId = '" & _DB.CleanString(tmpId) & "' "
                    _DB.ExecuteWithTransection(sql)

                    'สุดท้าย Update ItemJsonData ให้เหรียญเป็นค่าล่าสุด
                    sql = " SELECT ItemJsonData,Gold,Silver,Diamond FROM dbo.t360_tblStudent INNER JOIN dbo.tblStudentPoint " &
                          " ON dbo.t360_tblStudent.Student_Id = dbo.tblStudentPoint.Student_Id " &
                          " WHERE dbo.t360_tblStudent.Student_Id = '" & StudentId & "' AND dbo.t360_tblStudent.Student_IsActive = 1 " &
                          " AND dbo.tblStudentPoint.IsActive = 1 "
                    Dim dtCurrentData As New DataTable
                    dtCurrentData = _DB.getdataWithTransaction(sql)
                    If dtCurrentData.Rows.Count > 0 Then
                        Dim ItemJsonData As String = dtCurrentData.Rows(0)("ItemJsonData").ToString()
                        Dim CurrentSilver As String = dtCurrentData.Rows(0)("Silver").ToString()
                        Dim CurrentGold As String = dtCurrentData.Rows(0)("Gold").ToString()
                        Dim CurrentDiamond As String = dtCurrentData.Rows(0)("Diamond").ToString()
                        ItemJsonData = ReplaceCoinToNewValue(ItemJsonData, """user_silver_coin""", CurrentSilver)
                        ItemJsonData = ReplaceCoinToNewValue(ItemJsonData, """user_gold_coin""", CurrentGold)
                        ItemJsonData = ReplaceCoinToNewValue(ItemJsonData, """user_diamond""", CurrentDiamond)
                        'หลังจากแก้ ตัวเลขเสร็จก็ Update ลงไปใหม่
                        sql = " UPDATE dbo.t360_tblStudent SET ItemJsonData = '" & _DB.CleanString(ItemJsonData) & "' WHERE Student_Id = '" & StudentId & "' AND Student_IsActive = 1 "
                        _DB.ExecuteWithTransection(sql)
                    End If

                    _DB.CommitTransection()
                    _DB = Nothing
                    Return "OK"

                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    _DB.RollbackTransection()
                    _DB = Nothing
                    Return -2
                End Try
            Else
                Return -1
            End If
        Else
            _DB = Nothing
            Return -1
        End If

    End Function

    ''' <summary>
    ''' ทำการส่ง FlagId ที่ได้มามาเช็คกับ tmp ดูว่ามีจริงหรือเปล่า
    ''' </summary>
    ''' <param name="tmpId">รหัสของ tmp</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function ChecktmpIdIsComplete(ByVal tmpId As String) As Boolean
        If tmpId <> "" And tmpId IsNot Nothing Then
            Dim _DB As New ClassConnectSql()
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tmp_GetcurrentStatus WHERE FlagId = '" & _DB.CleanString(tmpId) & "' AND IsActive = 1 "
            Dim ChecktmpId As String = _DB.ExecuteScalar(sql)
            If CInt(ChecktmpId) > 0 Then
                _DB = Nothing
                Return True
            Else
                _DB = Nothing
                Return False
            End If
        Else
            Return -1
        End If
    End Function

    ''' <summary>
    ''' ทำการ update ข้อมูล จำนวนเหรียญต่างๆลง tmp ไปก่อน
    ''' </summary>
    ''' <param name="ResultGold">จำนวนเหรียญทอง</param>
    ''' <param name="ResultSilver">จำนวนเหรียญเงิน</param>
    ''' <param name="ResultDiamond">จำนวนเพชร</param>
    ''' <param name="ResultReceiveDiamond">จำนวนเพชรทั้งหมด</param>
    ''' <param name="_ObjDb">ตัวแปร connection</param>
    ''' <returns>String:tmpId,""</returns>
    ''' <remarks></remarks>
    Public Function InsertDataIntmpGetCurrentStatus(ByVal ResultGold As Integer, ByVal ResultSilver As Integer, ByVal ResultDiamond As Integer, ByVal ResultReceiveDiamond As Integer, ByRef _ObjDb As ClassConnectSql) As String

        Try
            Dim sql As String = " SELECT NEWID() "
            Dim tmpId As String = _ObjDb.ExecuteScalarWithTransection(sql)
            sql = " INSERT INTO dbo.tmp_GetcurrentStatus ( FlagId ,ResultGold ,ResultSilver, ResultDiamond,ResultRecieveDiamond ,IsActive ) " &
              " VALUES  ( '" & tmpId.ToString() & "' , " & ResultGold & " ," & ResultSilver & ", " & ResultDiamond & "," & ResultReceiveDiamond & " , 0) "
            _ObjDb.ExecuteWithTransection(sql)
            Return tmpId
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

    End Function

    ''' <summary>
    ''' หา ShopItemId จากชื่อ Item ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="ItemId">ชื่อ Item</param>
    ''' <returns>String:ShopItem_Id</returns>
    ''' <remarks></remarks>
    Public Function GetShopItemIdByItemId(ByVal ItemId As String) As String
        Dim _DB As New ClassConnectSql()
        'Dim sql As String = " SELECT ShopItem_Id FROM dbo.tblShopItem WHERE ItemId = " & ItemId & " "
        Dim sql As String = " SELECT ShopItem_Id FROM dbo.tblShopItem WHERE ShopItem_Name = '" & _DB.CleanString(ItemId) & "' "
        Dim ShopItemId As String = _DB.ExecuteScalar(sql)
        _DB = Nothing
        Return ShopItemId
    End Function

    ''' <summary>
    ''' Insert ข้อมูลผูก Item กับนักเรียน
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="ItemId">รหัส Item</param>
    ''' <param name="ItemPosition">ตำแหน่งของ Item</param>
    ''' <param name="_ObjDb">ตัวแปร Connection</param>
    ''' <returns>Integer:0,-2</returns>
    ''' <remarks></remarks>
    Public Function InsertLastStudentItem(ByVal StudentId As String, ByVal ItemId As String, ByVal ItemPosition As Integer, ByRef _ObjDb As ClassConnectSql) As Integer
        Dim sql As String = " insert into tblStudentItem(SI_Id,Student_Id,ShopItem_Id,SI_Position,IsActive,LastUpdate,School_Code,ClientId) " &
                            " values(NEWID(),'" & _ObjDb.CleanString(StudentId) & "','" & _ObjDb.CleanString(ItemId) & "'," & ItemPosition & ",1,dbo.GetThaiDate(),'" & HttpContext.Current.Session("SchoolCode") & "',NULL) "
        Try
            _ObjDb.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -2
        End Try
    End Function

    ''' <summary>
    ''' ทำการลบข้อมูล Item ของนักเรียนทิ้งให้หมดก่อนเพื่อที่จะได้ Insert เข้าไปใหม่
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="_ObjDB">ตัวแปร Connection</param>
    ''' <returns>Integer:0,-2</returns>
    ''' <remarks></remarks>
    Public Function SetIsActiveFalseStudentItem(ByVal StudentId As String, ByRef _ObjDB As ClassConnectSql) As Integer
        Dim sql As String = " update tblStudentItem set IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL where Student_Id = '" & StudentId & "' "
        Try
            _ObjDB.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -2
        End Try
    End Function

    Public Function UpdateCurrentGoldAndSilver(ByVal StudentId As String, ByVal TotalSilver As Integer, ByVal TotalGold As Integer, ByRef _ObjDB As ClassConnectSql)
        Dim sql As String = " update tblStudentPoint set Silver = '" & TotalSilver & "', Gold = '" & TotalGold & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where Student_Id = '" & StudentId & "' "
        Try
            _ObjDB.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -2
        End Try
    End Function

    ''' <summary>
    ''' หาข้อมูลจำนวนเหรียญต่างๆของนักเรียน
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="_ObjDB">ตัวแปร Connection</param>
    ''' <returns>Datatable</returns>
    ''' <remarks></remarks>
    Public Function GetDtSilverAndGoldCoin(ByVal StudentId As String, ByRef _ObjDB As ClassConnectSql) As DataTable
        Dim sql As String = " select Silver,Gold,Diamond,TotalDiamond from tblStudentPoint where Student_Id = '" & StudentId & "' "
        Dim dt As New DataTable
        dt = _ObjDB.getdataWithTransaction(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูลของนักเรียนเพิ่มเข้าไปใน tblStudentPoint
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรีนย</param>
    ''' <param name="_ObjDb">ตัวแปร Connection</param>
    ''' <returns>Integer:0,-2</returns>
    ''' <remarks></remarks>
    Public Function InsertStudentPoint(ByVal StudentId As String, ByRef _ObjDb As ClassConnectSql) As Integer
        Dim sql As String = " insert into tblStudentPoint(StudentPoint_Id,Student_Id,Silver,Gold,Diamond,TotalDiamond,LastUpdate,IsActive,TotalScore,Point_Level,TotalSilver,TotalGold) " &
                            " values(NEWID(),'" & StudentId & "',0,0,0,0,dbo.GetThaiDate(),1,0,1,0,0) "
        Try
            _ObjDb.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -2
        End Try
    End Function

    ''' <summary>
    ''' ทำการหารหัสนักเรียน จากรหัสเครื่อง Tablet
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <returns>String:Student_Id</returns>
    ''' <remarks></remarks>
    Public Function GetStudentIdByDeviceId(ByVal DeviceId As String) As String
        Dim sql As String = " Select t360_tblTabletOwner.Owner_Id FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) "
        Dim StudentId As String = _DB.ExecuteScalar(sql)
        Return StudentId
    End Function

    ''' <summary>
    ''' หารหัสโรงเรียน จากรหัสเครื่อง Tablet
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <returns>String:School_Code</returns>
    ''' <remarks></remarks>
    Public Function GetSchoolIdByDeviceId(ByVal DeviceId As String) As String
        Dim sql As String = " Select t360_tblTabletOwner.School_Code FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " &
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) "
        Dim SchoolId As String = _DB.ExecuteScalar(sql)
        Return SchoolId
    End Function

    ''' <summary>
    ''' ทำการ split string เพื่อเอาชั้น/ห้อง ที่ครูคนนี้ประจำชั้น ออกมาใส่ใน Array
    ''' </summary>
    ''' <param name="StrRoom">สตริงของห้องที่ประจำชั้น</param>
    ''' <returns>ArrayList:ชั้นห้อง</returns>
    ''' <remarks></remarks>
    Private Function GetArrTeacherCurrentRoom(ByVal StrRoom As String) As ArrayList
        Dim ArrCurrentClassRoom As New ArrayList
        'ทำการตัดแยกแต่ละ ชั้น/ห้อง ออกจากกันก่อน
        Dim SplitRoom = StrRoom.Split("|")
        If SplitRoom.Count > 0 Then
            Dim EachClassRoom As String = ""
            Dim EachClass As String = ""
            Dim EachRoom As String = ""
            'loop เพื่อแยก ห้อง และ ชั้นออกจากกันอีกทีนึง , เงื่อนไขการจบ loop คือ วนจนครบทุกห้อง
            For index = 0 To SplitRoom.Count - 1
                EachClassRoom = SplitRoom(index)
                'ทำการแยก ชั้น และ ห้อง ออกจากกัน
                Dim EachSpliStr = EachClassRoom.Split("/")
                EachClass = FindClassName(EachSpliStr(0))
                EachRoom = "/" & EachSpliStr(1)
                'เมื่อหาสตริงห้องที่เป็นภาษาไทยได้แล้วก็นำมาใส่เข้าไปใน Array
                ArrCurrentClassRoom.Add(EachClass & EachRoom)
            Next
            Return ArrCurrentClassRoom
        Else
            Return ArrCurrentClassRoom
        End If
    End Function

    ''' <summary>
    ''' check ว่า Tablet เครื่องนี้ลงทะเบียนแล้วหรือยังสำหรับ Mode Lab
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="InputConn">ตัวแปร connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function CheckTabletIsRegistered(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        If DeviceId IsNot Nothing And DeviceId <> "" Then
            'เช็คก่อนว่า tblTabletDesk มีผูกอยู่หรือเปล่า
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblTabletLabDesk INNER JOIN dbo.t360_tblTablet ON dbo.tblTabletLabDesk.Tablet_Id = dbo.t360_tblTablet.Tablet_Id " &
                                " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' " &
                                " AND dbo.t360_tblTablet.Tablet_IsActive = 1 AND dbo.tblTabletLabDesk.IsActive = 1 "
            Dim CountCheckIsRegistered As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
            If CountCheckIsRegistered > 0 Then
                Return True
            Else
                'เช็คต่อว่าเป็นเครื่องสำรองหรือเปล่า
                sql = " SELECT COUNT(*) FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceId) & "' AND Tablet_IsActive = 1 AND Tablet_IsOwner = 0 "
                CountCheckIsRegistered = CInt(_DB.ExecuteScalar(sql, InputConn))
                If CountCheckIsRegistered > 0 Then
                    Return True
                Else
                    redis.SetKey(DeviceId & "_Register", False)
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' ทำการนำชั้นและวิชาเข้ามาหาว่าในแ config ได้อนุญาติให้ใช้ได้หรือเปล่า
    ''' </summary>
    ''' <param name="StrClass">สตริงชั้น</param>
    ''' <param name="StrSubject">สตริงวิชา</param>
    ''' <param name="StrConfig">สตริง config</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function CheckClassAndSubjectIsAllowByConfig(ByVal StrClass As String, ByVal StrSubject As String, ByVal StrConfig As String) As Boolean
        ClsLog.Record(" - ClassDroidPad.CheckClassAndSubjectIsAllowByConfig : param = " & StrClass & "," & StrSubject & "," & StrConfig)
        Dim MergeStr As String = StrSubject & "-" & StrClass
        MergeStr = MergeStr.ToLower()
        If StrConfig.ToLower().IndexOf(MergeStr) <> -1 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' ทำการหา GroupsubjectId และ ชื่อวิชา จากรหัสวิชาที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="SubjectId">รหัสวิชา</param>
    ''' <param name="_Obj">ตัวแปร connection</param>
    ''' <returns>String:GroupSubjectId</returns>
    ''' <remarks></remarks>
    Private Function GetGroupsubjectIdBySubjectId(ByVal SubjectId As String, Optional ByRef _Obj As ClsConnect = Nothing) As String
        Dim GroupsubjectName As String = ""
        Select Case SubjectId.ToLower()
            Case "1"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้ภาษาไทย"
            Case "2"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม"
            Case "3"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
            Case "4"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้วิทยาศาสตร์"
            Case "5"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
            Case "6"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
            Case "7"
                GroupsubjectName = "กลุ่่มสาระการเรียนรู้ศิลปะ"
            Case "8"
                GroupsubjectName = "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี"
            Case Else
                GroupsubjectName = ""
        End Select
        If GroupsubjectName <> "" Then
            Dim sql As String = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_Name = '" & GroupsubjectName & "' "
            Dim GroupsubjectId As String = ""
            If _Obj IsNot Nothing Then
                GroupsubjectId = _Obj.ExecuteScalarWithTransection(sql)
            Else
                GroupsubjectId = _DB.ExecuteScalar(sql)
            End If
            Return GroupsubjectId
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' ทำการหา LevelId และ ชื่อชั้น จากรหัสชั้นที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="ClassId">รหัสชั้น</param>
    ''' <param name="_Obj">ตัวแปร connection</param>
    ''' <returns>String:Level_Id</returns>
    ''' <remarks></remarks>
    Private Function GetLevelidByClassId(ByVal ClassId As String, Optional ByRef _Obj As ClsConnect = Nothing) As String
        Dim LevelName As String = ""
        Select Case ClassId
            Case "1"
                LevelName = "อนุบาล 1"
            Case "2"
                LevelName = "อนุบาล 2"
            Case "3"
                LevelName = "อนุบาล 3"
            Case "4"
                LevelName = "ประถมศึกษาปีที่ 1"
            Case "5"
                LevelName = "ประถมศึกษาปีที่ 2"
            Case "6"
                LevelName = "ประถมศึกษาปีที่ 3"
            Case "7"
                LevelName = "ประถมศึกษาปีที่ 4"
            Case "8"
                LevelName = "ประถมศึกษาปีที่ 5"
            Case "9"
                LevelName = "ประถมศึกษาปีที่ 6"
            Case "10"
                LevelName = "มัธยมศึกษาปีที่ 1"
            Case "11"
                LevelName = "มัธยมศึกษาปีที่ 2"
            Case "12"
                LevelName = "มัธยมศึกษาปีที่ 3"
            Case "13"
                LevelName = "มัธยมศึกษาปีที่ 4"
            Case "14"
                LevelName = "มัธยมศึกษาปีที่ 5"
            Case "15"
                LevelName = "มัธยมศึกษาปีที่ 6"
            Case Else
                LevelName = ""
        End Select
        Dim LevelId As String = ""
        Dim sql As String = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_Name = '" & LevelName & "' "
        If _Obj IsNot Nothing Then
            LevelId = _Obj.ExecuteScalarWithTransection(sql)
        Else
            LevelId = _DB.ExecuteScalar(sql)
        End If
        Return LevelId
    End Function

    Private Function GetRandomNumber8Digit(Optional ByRef ObjDb As ClsConnect = Nothing) As String
        Dim ReturnNumber As String = ""
        Dim rd As New Random
        Dim CountCheck As Integer = -1
        Do Until CountCheck = 0
            ReturnNumber = ""
            For index = 1 To 8
                ReturnNumber &= rd.Next(0, 10)
            Next
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblUser WHERE UserName = '" & ReturnNumber & "' AND IsActive = 1 "
            If ObjDb IsNot Nothing Then
                CountCheck = CInt(ObjDb.ExecuteScalarWithTransection(sql))
            Else
                CountCheck = CInt(_DB.ExecuteScalar(sql))
            End If
        Loop
        Return ReturnNumber
    End Function

#End Region

#Region "GetSchoolRank"
    ''' <summary>
    ''' ทำการหา 16 อันดับ ของนักเรียนที่ได้คะแนนสูงสุดของ รร
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSchoolRank(ByVal DeviceId As String)

        If DeviceId Is Nothing Or DeviceId = "" Then
            Return -1
        End If
        Dim SchoolId As String = GetSchoolIdByDeviceId(DeviceId)
        If SchoolId <> "" Then
            'หา 16 อันดับนักเรียนที่ได้คะแนนสูงสุดของ รร.
            Dim sql As String = " SELECT top 16 tblStudentPoint.Student_Id , RANK() over(order by TotalScore DESC) as RankInSchool, Student_AvatarSeqNo " &
                                " FROM tblStudentPoint INNER JOIN t360_tblStudent ON tblStudentPoint.Student_Id = t360_tblStudent.Student_Id " &
                                " WHERE (t360_tblStudent.School_Code = '" & SchoolId & "') AND Student_AvatarSeqNo IS NOT NULL group by tblStudentPoint.Student_Id,Student_AvatarSeqNo,TotalScore; "
            Dim dt As New DataTable
            dt = _DB.getdata(sql)

            If dt.Rows.Count > 0 Then
                'ทำการต่อสตริงให้เป็นแบบ JsonString 16 อันดับเพื่อ Return ค่ากลับไป
                Dim JsonData As String = "{""SchoolLeaderBoard"":["
                Dim StudentId As String = ""
                Dim StudentIndex As String = ""
                Dim StudentSeqNo As String = ""

                'loop เพื่อต่อสตริงในแต่ละอันดับของนักเรียนที่ได้คะแนนสูงสุด , เงื่อนไขการจบ loop คือ วนจนครบนักเรียนทุกคน (ไม่เกิน 16 รอบ)
                For index = 0 To dt.Rows.Count - 1
                    StudentId = dt.Rows(index)("Student_Id").ToString()
                    StudentIndex = dt.Rows(index)("RankInSchool").ToString()
                    StudentSeqNo = dt.Rows(index)("Student_AvatarSeqNo").ToString()
                    JsonData &= "{""index"":""" & StudentIndex & """,""StdId"":""" & StudentId & """,""SeqNo"":""" & StudentSeqNo & """},"
                Next

                If JsonData.EndsWith(",") Then
                    JsonData = JsonData.Substring(0, JsonData.Length - 1)
                End If

                JsonData &= "]}"
                Return JsonData
            Else
                Return -2
            End If
        Else
            Return -2
        End If
    End Function

    Public Function GetPathImageStudentAvatar(ByVal StudentId As String)
        Dim PathReturn As String = ""
        If StudentId <> "" And StudentId IsNot Nothing Then
            PathReturn &= "/" & StudentId.Substring(0, 1)
            PathReturn &= "/" & StudentId.Substring(1, 1)
            PathReturn &= "/" & StudentId.Substring(2, 1)
            PathReturn &= "/" & StudentId.Substring(3, 1)
            PathReturn &= "/" & StudentId.Substring(4, 1)
            PathReturn &= "/" & StudentId.Substring(5, 1)
            PathReturn &= "/" & StudentId.Substring(6, 1)
            PathReturn &= "/" & StudentId.Substring(7, 1)
            PathReturn &= "/" & "{" & StudentId & "}"
            PathReturn &= "/" & StudentId & ".png"
        End If
        Return PathReturn
    End Function

    Public Function GetSequenceStudentAvatar(ByVal StudentId As String)
        Dim sql As String = " select SA_Seq from tblStudentAvatar where Student_Id = '" & _DB.CleanString(StudentId) & "' "
        Dim SeqReturn As String = _DB.ExecuteScalar(sql)
        Return SeqReturn
    End Function

#End Region


#Region "ExternalApp"

#Region "Enum-LogAppType|LogActionType|LogStationType"
    Enum LogAppType
        RunApp = 0
        EnterStation = 1
        Action = 2
        Edit = 3
        CloseApp = 4
    End Enum

    Enum LogActionType
        Action = 0
        Edit = 1
    End Enum

    Enum LogStationType
        Enter = 0
        Close = 1
    End Enum

#End Region

#Region "API"

    Public Shared APIErrorFlag As Boolean = False

    ''' <summary>
    ''' ทำการเก็บ Log เมื่อเปิด App (RunApp)
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เก็บเกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String:0,-1,-2</returns>
    ''' <remarks></remarks>
    Public Function ProcessRunApp(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Dim _DBEx As New ClassConnectSql()
        Dim ConnRunApp As New SqlConnection
        _DBEx.OpenExclusiveConnect(ConnRunApp)
        Try
            Dim CheckProcessInsertExternalLog As String = ProcessInsertExternalLog(DeviceId, AppId, AppKey, LogAppType.RunApp, ActionTimeStamp, LogName, ConnRunApp)
            _DBEx.CloseExclusiveConnect(ConnRunApp)
            _DBEx = Nothing
            Return CheckProcessInsertExternalLog
        Catch ex As Exception
            _DBEx.CloseExclusiveConnect(ConnRunApp)
            _DBEx = Nothing
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' ทำการเก็บ Log เมื่อเข้าด่านต่างๆ (EnterStation)
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String:0,-2</returns>
    ''' <remarks></remarks>
    Public Function ProcessEnterStation(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal StationId As String, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Dim _DBEx As New ClassConnectSql()
        Dim ConnEnterStation As New SqlConnection
        _DBEx.OpenExclusiveConnect(ConnEnterStation)
        Try
            'ทำการหา PK ของ tblExternalLogApp
            Dim ELAId As String = GetNEWID(ConnEnterStation)
            Dim CheckProcessInsertExternalLog As String = ProcessInsertExternalLog(DeviceId, AppId, AppKey, LogAppType.EnterStation, ActionTimeStamp, LogName, ConnEnterStation, ELAId)
            'ถ้าเกิด Error จะต้องไม่ทำต่อให้ Return ค่ากลับออกไปเลย
            If CheckProcessInsertExternalLog = "-1" Or CheckProcessInsertExternalLog = "-2" Then
                _DBEx.CloseExclusiveConnect(ConnEnterStation)
                _DBEx = Nothing
                Return CheckProcessInsertExternalLog
            Else
                'ถ้าไม่ Error ต้องไป Insert tblExternalAppStation , tblExternalLogAppStation
                Dim EASId As String = CheckAndInsertExternalAppStation(StationId, AppId, ConnEnterStation)
                If EASId <> "" Then
                    'Insert ข้อมูล ว่าเข้าด่านไหนยังไง
                    If InsertExternalLogAppStation(ELAId, EASId, LogStationType.Enter, ConnEnterStation) <> "" Then
                        _DBEx.CloseExclusiveConnect(ConnEnterStation)
                        _DBEx = Nothing
                        Return "0"
                    Else
                        _DBEx.CloseExclusiveConnect(ConnEnterStation)
                        _DBEx = Nothing
                        Return "-2"
                    End If
                Else
                    _DBEx.CloseExclusiveConnect(ConnEnterStation)
                    _DBEx = Nothing
                    Return "-2"
                End If
            End If
        Catch ex As Exception
            _DBEx.CloseExclusiveConnect(ConnEnterStation)
            _DBEx = Nothing
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' ทำการเก็บ Log เมื่อกดตอบข้อต่างๆ (Action)
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="AnswerName">คำตอบ</param>
    ''' <param name="Score">คะแนน</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function ProcessAction(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal StationId As String, ByVal QuestionName As String, ByVal AnswerName As String, ByVal Score As Double, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Return ProcessInsertExternalActionOrEdit(LogActionType.Action, DeviceId, AppId, AppKey, StationId, QuestionName, AnswerName, Score, ActionTimeStamp, LogName)
    End Function

    ''' <summary>
    ''' ทำการเก็บ Log เมื่อกดแก้ไขคำตอบข้อต่างๆ (Edit)
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="AnswerName">คำตอบ</param>
    ''' <param name="Score">คะแนน</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function ProcessEdit(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal StationId As String, ByVal QuestionName As String, ByVal AnswerName As String, ByVal Score As Double, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Return ProcessInsertExternalActionOrEdit(LogActionType.Edit, DeviceId, AppId, AppKey, StationId, QuestionName, AnswerName, Score, ActionTimeStamp, LogName)
    End Function

    ''' <summary>
    ''' ทำการเก็บ Log เมื่อกดปิด App (CloseApp)
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function ProcessCloseApp(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal StationId As String, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Dim _DBEx As New ClassConnectSql()
        Dim ConnCloseApp As New SqlConnection
        _DBEx.OpenExclusiveConnect(ConnCloseApp)
        Try
            Dim ELAId As String = GetNEWID(ConnCloseApp)
            Dim CheckProcessInsertExternalLog As String = ProcessInsertExternalLog(DeviceId, AppId, AppKey, LogAppType.CloseApp, ActionTimeStamp, LogName, ConnCloseApp, ELAId)
            If CheckProcessInsertExternalLog = "-1" Or CheckProcessInsertExternalLog = "-2" Then
                _DBEx.CloseExclusiveConnect(ConnCloseApp)
                _DBEx = Nothing
                Return CheckProcessInsertExternalLog
            Else
                Dim ExternalAppId As String = GetExternalAppIdByAppName(AppId, ConnCloseApp)
                If ExternalAppId <> "" Then
                    Dim EASId As String = GetEASIdByExternalAppIdAndStationId(ExternalAppId, StationId, ConnCloseApp)
                    If EASId <> "" Then
                        If InsertExternalLogAppStation(ELAId, EASId, LogStationType.Close, ConnCloseApp) <> "" Then
                            _DBEx.CloseExclusiveConnect(ConnCloseApp)
                            _DBEx = Nothing
                            Return "0"
                        Else
                            _DBEx.CloseExclusiveConnect(ConnCloseApp)
                            _DBEx = Nothing
                            Return "-2"
                        End If
                    Else
                        _DBEx.CloseExclusiveConnect(ConnCloseApp)
                        _DBEx = Nothing
                        Return "-2"
                    End If
                Else
                    _DBEx.CloseExclusiveConnect(ConnCloseApp)
                    _DBEx = Nothing
                    Return "-2"
                End If
            End If
        Catch ex As Exception
            _DBEx.CloseExclusiveConnect(ConnCloseApp)
            _DBEx = Nothing
            Return "-2"
        End Try
    End Function

    'MultiUsage
    ''' <summary>
    ''' ทำการเก็บ Log ทุก ขั้นตอน ตั้งแต่ RunApp,EnterStation,Action,Edit,CloseApp ตาม List ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="RunAppList">List ของการเปิด App</param>
    ''' <param name="EnterStationList">List ของการเข้าด่านต่างๆ</param>
    ''' <param name="ActionList">List ของการกดตอบคำถาม</param>
    ''' <param name="EditList">List ของการแก้ไขคำตอบ</param>
    ''' <param name="CloseAppList">List ของการปิด App</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetMultiUsage(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal RunAppList As String, ByVal EnterStationList As String, ByVal ActionList As String, ByVal EditList As String, ByVal CloseAppList As String) As String

        Dim jss As New JavaScriptSerializer()
        Dim ReturnValue As String = ""
        Try
            If RunAppList <> "" Then
                'RunAppList
                Dim objRunAppList = deserialize(Of RaList)(RunAppList)
                If APIErrorFlag = True Then Return "-1"
                'loop เพื่อเก็บ log การเปิด App ในแต่ละครั้ง , เงื่อนไขการจบ loop คือวนจนครบทุก row ใน List
                For Each obj As RunApp In objRunAppList.RunApp
                    ReturnValue = ProcessRunApp(DeviceId, AppId, AppKey, obj.TimeStamp)
                    HttpContext.Current.Response.Write("RunApp : " & ReturnValue & "<br />")
                Next
            End If

            If EnterStationList <> "" Then
                'EnterStation
                Dim objEnterStationList = deserialize(Of EaList)(EnterStationList)
                If APIErrorFlag = True Then Return "-1"
                'loop เพื่อเก็บ log การเข้าด่านต่างๆในแต่ละครั้ง , เงื่อนไขการจบ loop คือวนจนครบทุก row ใน List
                For Each obj As EnterStation In objEnterStationList.EnterStation
                    ReturnValue = ProcessEnterStation(DeviceId, AppId, AppKey, obj.StationId, obj.TimeStamp)
                    HttpContext.Current.Response.Write("EnterStation : " & ReturnValue & "<br />")
                Next
            End If

            If ActionList <> "" Then
                'ActionList
                Dim objActionList = deserialize(Of AcList)(ActionList)
                If APIErrorFlag = True Then Return "-1"
                'loop เพื่อเก็บ log การกดตอบคำถามในแต่ละครั้ง , เงื่อนไขการจบ loop คือวนจนครบทุก row ใน List
                For Each obj As Action In objActionList.Action
                    ReturnValue = ProcessAction(DeviceId, AppId, AppKey, obj.StationId, obj.QuestionName, obj.AnswerName, obj.Score, obj.TimeStamp)
                    HttpContext.Current.Response.Write("Action : " & ReturnValue & "<br />")
                Next
            End If

            If EditList <> "" Then
                'EditList
                Dim objEditList = deserialize(Of EList)(EditList)
                If APIErrorFlag = True Then Return "-1"
                'loop เพื่อเก็บ log การแก้ไขคำตอบในแต่ละครั้ง , เงื่อนไขการจบ loop คือวนจนครบทุก row ใน List
                For Each obj As Edit In objEditList.Edit
                    ReturnValue = ProcessEdit(DeviceId, AppId, AppKey, obj.StationId, obj.QuestionName, obj.AnswerName, obj.Score, obj.TimeStamp)
                    HttpContext.Current.Response.Write("Edit : " & ReturnValue & "<br />")
                Next
            End If

            If CloseAppList <> "" Then
                'CloseAppList
                Dim objCloseAppList = deserialize(Of CAList)(CloseAppList)
                If APIErrorFlag = True Then Return "-1"
                'loop เพื่อเก็บ log การปิด App แต่ละครั้ง , เงื่อนไขการจบ loop คือวนจนครบทุก row ใน List
                For Each obj As CloseApp In objCloseAppList.CloseApp
                    ReturnValue = ProcessCloseApp(DeviceId, AppId, AppKey, obj.StationId, obj.TimeStamp)
                    HttpContext.Current.Response.Write("Close : " & ReturnValue & "<br />")
                Next
            End If
            'Complete
            Return 0
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' ทำการแปลง JsonString ให้กลายเป็น List ของ Process ต่างๆ
    ''' </summary>
    ''' <typeparam name="T">List ที่ต้องการแปลงจาก JsonString</typeparam>
    ''' <param name="jsonStr">Jsonสตริงที่ส่งเข้ามาเพื่อแปลงให้เป็น List ของ Process ต่างๆ</param>
    ''' <returns>T</returns>
    ''' <remarks></remarks>
    Private Function deserialize(Of T)(ByVal jsonStr As String) As T
        Dim s = New System.Web.Script.Serialization.JavaScriptSerializer()
        Return s.Deserialize(Of T)(jsonStr)
    End Function

    ''' <summary>
    ''' ทำการเช็ค ชื่อ App , AppKey ว่าถูกต้องตรงกับใน DB หรือเปล่า
    ''' </summary>
    ''' <param name="AppName">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ App</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function CheckAppNameAndAppKeyIsCorrect(ByVal AppName As String, ByVal AppKey As String, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Try
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblExternalApp WHERE ExternalAppKey = '" & _DB.CleanString(AppKey.Trim()) & "' AND ExternalAppName = '" & _DB.CleanString(AppName.Trim()) & "' "
            Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
            If CountCheck > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูล Log ที่ส่งเข้ามาลงใน tblExternalLogApp
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppName">ชื่อ App</param>
    ''' <param name="InputLogAppType">Type ของ Log</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <param name="ELAId">Id ของ tblExternalLogApp จะมีในกรณีที่เป็น การกดตอบคำถาม</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Private Function InsertExternalLogApp(ByVal DeviceId As String, ByVal AppName As String, ByVal InputLogAppType As LogAppType, ByVal ActionTimeStamp As String, Optional ByVal LogName As String = Nothing, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal ELAId As String = "") As String
        Try
            Dim sql As String = ""
            Dim ExternalAppId As String = GetExternalAppIdByAppName(AppName, InputConn)
            If ExternalAppId <> "" Then
                If LogName Is Nothing Then
                    LogName = "NULL"
                Else
                    LogName = "'" & LogName & "'"
                End If
                If ELAId <> "" Then
                    ELAId = "'" & ELAId & "'"
                Else
                    ELAId = "NEWID()"
                End If
                sql = " INSERT INTO dbo.tblExternalLogApp( ELAId ,ExternalAppId ,DeviceId ,LogName ,LogAppType ,ActionTimeStamp ,LastUpdate ,IsActive) " &
                      " VALUES  ( " & ELAId & " ,'" & ExternalAppId & "' ,'" & _DB.CleanString(DeviceId.Trim()) & "' , " & _DB.CleanString(LogName.Trim()) & " " &
                      " , " & InputLogAppType & " , '" & _DB.CleanString(ActionTimeStamp.Trim()) & "' ,dbo.GetThaiDate() ,1); "
                _DB.Execute(sql, InputConn)
                Return "Complete"
            Else
                Return "NoId"
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' หา ExternalAppId จาก tblExternal โดยใช้ชื่อ App
    ''' </summary>
    ''' <param name="AppName">ชื่อ App</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:"",0,-1,-2</returns>
    ''' <remarks></remarks>
    Private Function GetExternalAppIdByAppName(ByVal AppName As String, Optional ByVal InputConn As SqlConnection = Nothing) As String
        Try
            Dim sql As String = " SELECT ExternalAppId FROM dbo.tblExternalApp WHERE ExternalAppName = '" & _DB.CleanString(AppName.Trim()) & "' "
            Dim ExternalAppId As String = _DB.ExecuteScalar(sql, InputConn)
            Return ExternalAppId
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' ทำการ Insert Log ของ QApp ตาม Type ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="InputLogAppType">Type ของ Log ที่จะ save</param>
    ''' <param name="ActionTimeStamp">วันเวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <param name="ELAId">Id ของ tblExternalLogApp จะส่งเข้ามาก็ต่อเมื่อเป็นการกดตอบคำถาม</param>
    ''' <returns>String:0,-1,-2</returns>
    ''' <remarks></remarks>
    Private Function ProcessInsertExternalLog(ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal InputLogAppType As LogAppType, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal ELAId As String = "") As String
        Try
            If CheckAppNameAndAppKeyIsCorrect(AppId, AppKey, InputConn) = False Then
                Return "-1"
            Else
                Dim ReturnValue As String = ""
                'ทำการนำ วันที่เวลา มาเปลี่ยน format ให้เป็นแบบที่สามารถส่งเข้าไป update ใน DB ได้
                If ActionTimeStamp = "" Then
                    Dim CultureInfo As New System.Globalization.CultureInfo("en-US")
                    ActionTimeStamp = DateTime.Now().ToString("yyyy-MM-dd HH:MM:ss:fff", CultureInfo)
                End If
                'Insert ข้อมูลลง tblExternalLogApp
                Dim CheckReturnValue As String = InsertExternalLogApp(DeviceId, AppId, InputLogAppType, ActionTimeStamp, LogName, InputConn, ELAId)
                If CheckReturnValue = "" Then
                    ReturnValue = "-2"
                ElseIf CheckReturnValue = "NoId" Then
                    ReturnValue = "-1"
                Else
                    ReturnValue = "0"
                End If
                Return ReturnValue
            End If
        Catch ex As Exception
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' Function ที่ Insert Log + ทุกขั้นตอนสำหรับ Action,Edit
    ''' </summary>
    ''' <param name="InputActionLogType">Type ที่จะเก็บ Log (Action,Edit)</param>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="AppKey">Key ของ Field ExternalAppKey ที่ไม่มี - </param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="AnswerName">คำตอบ</param>
    ''' <param name="Score">คะแนน</param>
    ''' <param name="ActionTimeStamp">วันที่เวลาที่เกิดเหตุการณ์</param>
    ''' <param name="LogName">รายละเอียด</param>
    ''' <returns>String:0,-2</returns>
    ''' <remarks></remarks>
    Private Function ProcessInsertExternalActionOrEdit(ByVal InputActionLogType As LogActionType, ByVal DeviceId As String, ByVal AppId As String, ByVal AppKey As String, ByVal StationId As String, ByVal QuestionName As String, ByVal AnswerName As String, ByVal Score As Double, Optional ByVal ActionTimeStamp As String = "", Optional ByVal LogName As String = Nothing) As String
        Dim _DBEx As New ClassConnectSql()
        Dim ConnAction As New SqlConnection
        _DBEx.OpenExclusiveConnect(ConnAction)
        Try
            Dim ELAId As String = GetNEWID(ConnAction)
            If ELAId <> "" Then
                Dim CheckProcessInsertExternalLog As String = ProcessInsertExternalLog(DeviceId, AppId, AppKey, LogAppType.Action, ActionTimeStamp, LogName, ConnAction, ELAId)
                'ถ้าเกิด Error จะต้องไม่ทำต่อให้ Return ค่ากลับออกไปเลย
                If CheckProcessInsertExternalLog = "-1" Or CheckProcessInsertExternalLog = "-2" Then
                    _DBEx.CloseExclusiveConnect(ConnAction)
                    _DBEx = Nothing
                    Return CheckProcessInsertExternalLog
                Else
                    'ถ้าไม่ Error ค่อยมา Insert ลง tblExternalLogAction อีกที
                    'ทำการ Insert คำถาม
                    Dim EAQId As String = CheckAndInsertExternalAppQuestion(StationId, AppId, QuestionName, ConnAction)
                    If EAQId <> "" Then
                        'ทำการ Insert คำตอบ
                        Dim EAAId As String = CheckAndInsertExternalAppAnswer(EAQId, AnswerName, Score, ConnAction)
                        If EAAId <> "" Then
                            'ทำการเก็บ Log ว่าเป็นการ แก้ไข หรือ กดตอบ Action,Edit
                            If InsertExternalLogAction(ELAId, EAQId, EAAId, InputActionLogType, ConnAction) = True Then
                                _DBEx.CloseExclusiveConnect(ConnAction)
                                _DBEx = Nothing
                                Return "0"
                            Else
                                _DBEx.CloseExclusiveConnect(ConnAction)
                                _DBEx = Nothing
                                Return "-2"
                            End If
                        Else
                            _DBEx.CloseExclusiveConnect(ConnAction)
                            _DBEx = Nothing
                            Return "-2"
                        End If
                    Else
                        _DBEx.CloseExclusiveConnect(ConnAction)
                        _DBEx = Nothing
                        Return "-2"
                    End If
                End If
            Else
                _DBEx.CloseExclusiveConnect(ConnAction)
                _DBEx = Nothing
                Return "-2"
            End If
        Catch ex As Exception
            _DBEx.CloseExclusiveConnect(ConnAction)
            _DBEx = Nothing
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' Function ที่เช็คว่ามีข้อมูลใน tblExternalAppStation หรือยัง ถ้ายังไม่มีจะ Insert ให้แล้วคืน EASId กลับไป แต่ถ้ามีแล้วก็จะคืน EASId กลับไปเลย
    ''' </summary>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:EASId,""</returns>
    ''' <remarks></remarks>
    Private Function CheckAndInsertExternalAppStation(ByVal StationId As String, ByVal AppId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Try
            'ลอง Get ค่าจาก Redist ก่อนถ้าไม่มีค่อยหา
            Dim EASId As String = Redist.Getkey(StationId & "|" & AppId)
            If EASId <> "" Then
                Return EASId
            End If
            'หา Id ของด่านจากชื่อด่าน
            Dim sql As String = " SELECT EASId FROM dbo.tblExternalAppStation WHERE StationName = '" & _DB.CleanString(StationId.Trim()) & "' AND IsActive = 1; "
            EASId = _DB.ExecuteScalar(sql, InputConn)
            'ถ้ามีค่าให้เก็บเข้า Redis ไว้เลย จะได้ไม่ต้องมาหาอีก
            If EASId <> "" Then
                Redist.SetKey(StationId & "|" & AppId, EASId)
                Return EASId
            Else
                'ต้อง Insert ข้อมูลด่านนี้เข้าไปใน tblExternalAppStation ใหม่
                EASId = GetNEWID(InputConn)
                Dim ExternalAppId As String = GetExternalAppIdByAppName(AppId, InputConn)
                If ExternalAppId <> "" Then
                    sql = " INSERT INTO dbo.tblExternalAppStation( EASId ,ExternalAppId ,StationName ,LastUpdate ,IsActive) " &
                          " VALUES  ( '" & EASId & "' , '" & ExternalAppId & "' ,'" & _DB.CleanString(StationId.Trim()) & "' ,dbo.GetThaiDate() ,1); "
                    _DB.Execute(sql, InputConn)
                    'เมื่อ Insert เสร็จก็เก็บค่าเข้า Redis ไว้เลยจะได้ไม่ต้องมาหาใหม่
                    Redist.SetKey(StationId & "|" & AppId, EASId)
                    Return EASId
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Insert Log ในการเข้าด่าน EnterStation
    ''' </summary>
    ''' <param name="ELAId">Id ของ tblExternalLogApp</param>
    ''' <param name="EASId">Id ของด่าน tblExternalAppStation</param>
    ''' <param name="InputLogStationType">Type ของการเข้าด่าน</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:Complete,""</returns>
    ''' <remarks></remarks>
    Private Function InsertExternalLogAppStation(ByVal ELAId As String, ByVal EASId As String, ByVal InputLogStationType As LogStationType, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Try
            Dim sql As String = " INSERT INTO dbo.tblExternalLogAppStation( ELASId ,ELAId ,EASId ,LastUpdate ,IsActive,ELASType) " &
                                " VALUES  ( NEWID() , '" & ELAId & "' , '" & EASId & "' , dbo.GetThaiDate() ,1," & InputLogStationType & "); "
            _DB.Execute(sql, InputConn)
            Return "Complete"
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Function ที่เช็คว่ามีข้อมูลใน tblExternalAppQuestion หรือยัง ถ้ายังไม่มีจะ Insert ให้แล้วคืน EAQId กลับไป แต่ถ้ามีแล้วจะคืน EAQId กลับไปเลย
    ''' </summary>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="AppId">ชื่อ App</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:EAQId,""</returns>
    ''' <remarks></remarks>
    Private Function CheckAndInsertExternalAppQuestion(ByVal StationId As String, ByVal AppId As String, ByVal QuestionName As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Try
            'ลอง Get ค่า จาก Redist ก่อน
            Dim EAQId As String = Redist.Getkey(StationId & "|" & AppId & "|" & QuestionName)
            If EAQId <> "" Then
                Return EAQId
            End If
            'หาค่า EAQID จาก ชื่อ App และ ชื่อด่าน
            Dim sql As String = " SELECT EAQId FROM tblExternalApp INNER JOIN tblExternalAppQuestion ON tblExternalApp.ExternalAppId = tblExternalAppQuestion.ExternalAppId INNER JOIN " &
                                " tblExternalAppStation ON tblExternalAppQuestion.EASId = tblExternalAppStation.EASId WHERE tblExternalAppStation.StationName = '" & _DB.CleanString(StationId.Trim()) & "' " &
                                " AND tblExternalApp.ExternalAppName = '" & _DB.CleanString(AppId.Trim()) & "' AND dbo.tblExternalApp.IsActive = 1 AND dbo.tblExternalAppQuestion.IsActive = 1 " &
                                " AND tblExternalAppQuestion.IsActive = 1 "
            EAQId = _DB.ExecuteScalar(sql, InputConn)
            If EAQId <> "" Then
                'เก็บค่าไว้ใน Redis เลยจะได้ไม่ต้องมาหาอีก
                Redist.SetKey(StationId & "|" & AppId & "|" & QuestionName, EAQId)
                Return EAQId
            Else
                'ทำการ Insert ข้อมูลคำถามเข้าไปใหม่ที่ tblExternalAppQuestion
                EAQId = GetNEWID(InputConn)
                Dim ExternalAppId As String = GetExternalAppIdByAppName(AppId)
                If ExternalAppId <> "" Then
                    'หา Id ของด่านก่อน
                    Dim EASId As String = GetEASIdByExternalAppIdAndStationId(ExternalAppId, StationId)
                    'ทำการ Insert ข้อมูลคำถาม
                    If EASId <> "" Then
                        sql = " INSERT INTO dbo.tblExternalAppQuestion( EAQId ,ExternalAppId ,EASId ,AQName ,LastUpdate ,IsActive) " &
                      " VALUES  ( '" & EAQId & "' , '" & ExternalAppId & "' ,'" & EASId & "' , '" & _DB.CleanString(QuestionName.Trim()) & "' , dbo.GetThaiDate() ,1); "
                        _DB.Execute(sql, InputConn)
                        'เก็บค่าไว้ใน Redis เลยจะได้ไม่ต้องมาหาอีก
                        Redist.SetKey(StationId & "|" & AppId & "|" & QuestionName.Trim(), EAQId)
                        Return EAQId
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Function ที่เช็คว่ามีข้อมูลใน tblExternalAppAnswer หรือยัง ถ้ายังไม่มีจะ Insert ให้แล้วคืน EAAId กลับไป แต่ถ้ามีแล้วจะคืน EAAId กลับไปเลย
    ''' </summary>
    ''' <param name="EAQId">Id ของคำถาม</param>
    ''' <param name="AnswerName">คำตอบ</param>
    ''' <param name="Score">คะแนน</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:EAAId,""</returns>
    ''' <remarks></remarks>
    Private Function CheckAndInsertExternalAppAnswer(EAQId As String, ByVal AnswerName As String, ByVal Score As Double, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Try

            'SHIN - 20140710 - แก้ไขให้รองรับการบันทึกข้อมูลแบบที่ไม่มีคะแนน (เช่นพวก กิจกรรม) เอาไว้บันทึกพฤติกรรมการใช้งานทั่วไป
            Dim ScoreToDb As String
            If Score = -9999999.9999 Then
                ScoreToDb = "NULL"
            Else
                ScoreToDb = Score.ToString()
            End If
            'SHIN - 20140710 - แก้ไขให้รองรับการบันทึกข้อมูลแบบที่ไม่มีคะแนน (เช่นพวก กิจกรรม) เอาไว้บันทึกพฤติกรรมการใช้งานทั่วไป


            'ลอง Get ค่าจาก Redist ก่อน
            'Dim EAAId As String = Redist.Getkey(EAQId & "|" & AnswerName.Trim() & "|" & ScoreToDb)
            Dim EAAId As String = Redist.Getkey(EAQId & "|" & AnswerName.Trim() & "|" & ScoreToDb) 'SHIN - 20140710 - แก้ไขให้รองรับการบันทึกข้อมูลแบบที่ไม่มีคะแนน (เช่นพวก กิจกรรม) เอาไว้บันทึกพฤติกรรมการใช้งานทั่วไป

            If EAAId <> "" Then
                Return EAAId
            End If

            Dim sql As String = " SELECT EAAId FROM dbo.tblExternalAppAnswer WHERE EAQId = '" & EAQId & "' AND AAName = '" & _DB.CleanString(AnswerName)
            'SHIN - 20140710 - แก้ไขให้รองรับการบันทึกข้อมูลแบบที่ไม่มีคะแนน (เช่นพวก กิจกรรม) เอาไว้บันทึกพฤติกรรมการใช้งานทั่วไป
            If ScoreToDb = "NULL" Then
                sql = sql & "' AND Score Is NULL "
            Else
                sql = sql & "' AND Score = " & ScoreToDb
            End If
            sql = sql & " AND IsActive = 1; "
            'SHIN - 20140710 - แก้ไขให้รองรับการบันทึกข้อมูลแบบที่ไม่มีคะแนน (เช่นพวก กิจกรรม) เอาไว้บันทึกพฤติกรรมการใช้งานทั่วไป



            EAAId = _DB.ExecuteScalar(sql, InputConn)
            If EAAId <> "" Then
                'เก็บค่าไว้ใน Redis เลยจะได้ไม่ต้องมาหาอีก
                Redist.SetKey(EAQId & "|" & AnswerName.Trim() & "|" & ScoreToDb, EAAId)
                Return EAAId
            Else
                'Insert ข้อมูลคำตอบใหม่เข้าไปใน tblExternalAppAnswer
                EAAId = GetNEWID(InputConn)
                sql = " INSERT INTO dbo.tblExternalAppAnswer( EAAId ,EAQId ,AAName ,Score ,LastUpdate ,IsActive) " &
                      " VALUES  ( '" & EAAId & "' , '" & EAQId & "' , '" & _DB.CleanString(AnswerName.Trim()) & "' ," & ScoreToDb & " , dbo.GetThaiDate() ,1); "
                _DB.Execute(sql, InputConn)
                'เก็บค่าไว้ใน Redis เลยจะได้ไม่ต้องมาหาอีก
                Redist.SetKey(EAQId & "|" & AnswerName.Trim() & "|" & ScoreToDb, EAAId)
                Return EAAId
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' เก็บ log ที่กดตอบคำถาม ว่าเป็นการกดตอบ หรือ เป็นการแก้ไข
    ''' </summary>
    ''' <param name="ELAId">Id ของ tblExternalLogApp</param>
    ''' <param name="EAQId">Id ของคำถาม</param>
    ''' <param name="EAAId">Id ของคำตอบ</param>
    ''' <param name="InputELAType">Type ของ Action นี้ (Action,Edit)</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function InsertExternalLogAction(ByVal ELAId As String, ByVal EAQId As String, ByVal EAAId As String, ByVal InputELAType As LogActionType, Optional ByRef InputConn As SqlConnection = Nothing) As Boolean
        Try
            Dim sql As String = " INSERT INTO dbo.tblExternalLogAction( ELActionId ,ELAId ,EAQId ,EAAId ,ELAType ,LastUpdate ,IsActive ) " &
                                " VALUES  ( NEWID() ,'" & ELAId & "' ,'" & EAQId & "' , '" & EAAId & "' , " & InputELAType & " ,dbo.GetThaiDate() ,1 ); "
            _DB.Execute(sql, InputConn)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' ทำการ Select GUID อันใหม่ไปให้
    ''' </summary>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:NEWID()</returns>
    ''' <remarks></remarks>
    Private Function GetNEWID(Optional ByVal InputConn As SqlConnection = Nothing) As String
        Dim sql As String = " SELECT NEWID(); "
        Dim NewId As String = _DB.ExecuteScalar(sql, InputConn)
        Return NewId
    End Function

    ''' <summary>
    ''' ทำการหา EASID หรือ Id ของด่าน
    ''' </summary>
    ''' <param name="ExternalAppId">Id ของ tblExternalApp</param>
    ''' <param name="StationId">ชื่อด่าน</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>String:EASId,""</returns>
    ''' <remarks></remarks>
    Private Function GetEASIdByExternalAppIdAndStationId(ByVal ExternalAppId As String, ByVal StationId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Try
            Dim sql As String = " SELECT EASId FROM dbo.tblExternalAppStation WHERE ExternalAppId = '" & _DB.CleanString(ExternalAppId) & "' AND StationName = '" & _DB.CleanString(StationId.Trim()) & "' AND IsActive = 1 "
            Dim EASId As String = _DB.ExecuteScalar(sql, InputConn)
            Return EASId
        Catch ex As Exception
            Return ""
        End Try
    End Function

#End Region

#Region "Class-Serializable"
    Public Class RaList
        Public Property RunApp() As List(Of RunApp)
            Get
                Return _RunApp
            End Get
            Set(ByVal value As List(Of RunApp))
                _RunApp = value
            End Set
        End Property
        Private _RunApp As List(Of RunApp)
    End Class

    <Serializable()>
    Public Class RunApp
        Private _TimeStamp As String
        Public Property TimeStamp As String
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If ClassDroidPad.CheckStrDate(value) = True Then
                        _TimeStamp = value
                    End If
                End If
            End Set
        End Property

        'Private _AppId As String
        'Public Property AppId As String
        '    Get
        '        Return _AppId
        '    End Get
        '    Set(ByVal value As String)
        '        If value.Trim().Length = 0 Then
        '            Throw New ArgumentException("AppId ต้องไม่เป็นค่าว่าง")
        '        Else
        '            _AppId = value
        '        End If
        '    End Set
        'End Property

        'Private _AppKey As String
        'Public Property AppKey As String
        '    Get
        '        Return _AppKey
        '    End Get
        '    Set(value As String)
        '        If value.Trim.Length = 32 Then
        '            _AppKey = value
        '        Else
        '            Throw New ArgumentException("AppKey ต้องมีค่า 32 หลักเท่านั้น")
        '        End If
        '    End Set
        'End Property

    End Class

    Public Class EaList
        Public Property EnterStation() As List(Of EnterStation)
            Get
                Return _EnterStation
            End Get
            Set(ByVal value As List(Of EnterStation))
                _EnterStation = value
            End Set
        End Property
        Private _EnterStation As List(Of EnterStation)
    End Class

    <Serializable()>
    Public Class EnterStation

        Private _TimeStamp As String
        Public Property TimeStamp As String
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If ClassDroidPad.CheckStrDate(value) = True Then
                        _TimeStamp = value
                    End If
                End If
            End Set
        End Property

        Private _StationId As String
        Public Property StationId As String
            Get
                Return _StationId
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("StationId ต้องไม่เป็นค่าว่าง")
                    Else
                        _StationId = value
                    End If
                End If
            End Set
        End Property

        'Private _AppId As String
        'Public Property AppId As String
        '    Get
        '        Return _AppId
        '    End Get
        '    Set(ByVal value As String)
        '        If value.Trim().Length = 0 Then
        '            Throw New ArgumentException("AppId ต้องไม่เป็นค่าว่าง")
        '        Else
        '            _AppId = value
        '        End If
        '    End Set
        'End Property

        'Private _AppKey As String
        'Public Property AppKey As String
        '    Get
        '        Return _AppKey
        '    End Get
        '    Set(value As String)
        '        If value.Trim.Length = 32 Then
        '            _AppKey = value
        '        Else
        '            Throw New ArgumentException("AppKey ต้องมีค่า 32 หลักเท่านั้น")
        '        End If
        '    End Set
        'End Property

    End Class

    Public Class AcList
        Public Property Action() As List(Of Action)
            Get
                Return _Action
            End Get
            Set(ByVal value As List(Of Action))
                _Action = value
            End Set
        End Property
        Private _Action As List(Of Action)
    End Class

    <Serializable()>
    Public Class Action

        Private _TimeStamp As String
        Public Property TimeStamp As String
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If ClassDroidPad.CheckStrDate(value) = True Then
                        _TimeStamp = value
                    End If
                End If
            End Set
        End Property

        Private _StationId As String
        Public Property StationId As String
            Get
                Return _StationId
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("StationId ต้องไม่เป็นค่าว่าง")
                    Else
                        _StationId = value
                    End If
                End If
            End Set
        End Property

        Private _QuestionName As String
        Public Property QuestionName As String
            Get
                Return _QuestionName
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("QuestionName ต้องไม่เป็นค่าว่าง")
                    Else
                        _QuestionName = value
                    End If
                End If
            End Set
        End Property

        Private _AnswerName As String
        Public Property AnswerName As String
            Get
                Return _AnswerName
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("AnswerName ต้องไม่เป็นค่าว่าง")
                    Else
                        _AnswerName = value
                    End If
                End If
            End Set
        End Property

        Private _Score As Double
        Public Property Score As String
            Get
                Return _Score.ToString
            End Get
            Set(value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim = "" Then
                        _Score = -9999999.9999
                    Else
                        If Not IsNumeric(value) Then
                            Throw New FormatException("คะแนนเป็นค่าว่าง หรือ ตัวเลข เท่านั้น (ตัวเลขอยู่ในช่วง -999.99 ถึง 999.99) เท่านั้น")
                        ElseIf (CDbl(value) < -999.99) Or (CDbl(value) > 999.99) Then
                            Throw New FormatException("คะแนนเป็นค่าว่าง หรือ ตัวเลข เท่านั้น (ตัวเลขอยู่ในช่วง -999.99 ถึง 999.99) เท่านั้น")
                        End If
                        _Score = value
                    End If
                End If
            End Set
        End Property

    End Class

    Public Class EList
        Public Property Edit() As List(Of Edit)
            Get
                Return _Edit
            End Get
            Set(ByVal value As List(Of Edit))
                _Edit = value
            End Set
        End Property
        Private _Edit As List(Of Edit)
    End Class

    <Serializable()>
    Public Class Edit

        Private _TimeStamp As String
        Public Property TimeStamp As String
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If ClassDroidPad.CheckStrDate(value) = True Then
                        _TimeStamp = value
                    End If
                End If
            End Set
        End Property

        Private _StationId As String
        Public Property StationId As String
            Get
                Return _StationId
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("StationId ต้องไม่เป็นค่าว่าง")
                    Else
                        _StationId = value
                    End If
                End If
            End Set
        End Property

        Private _QuestionName As String
        Public Property QuestionName As String
            Get
                Return _QuestionName
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("QuestionName ต้องไม่เป็นค่าว่าง")
                    Else
                        _QuestionName = value
                    End If
                End If
            End Set
        End Property

        Private _AnswerName As String
        Public Property AnswerName As String
            Get
                Return _AnswerName
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("AnswerName ต้องไม่เป็นค่าว่าง")
                    Else
                        _AnswerName = value
                    End If
                End If
            End Set
        End Property

        Private _Score As Double
        Public Property Score As String
            Get
                Return _Score.ToString
            End Get
            Set(value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim = "" Then
                        _Score = -9999999.9999
                    Else
                        If Not IsNumeric(value) Then
                            Throw New FormatException("คะแนนเป็นค่าว่าง หรือ ตัวเลข เท่านั้น (ตัวเลขอยู่ในช่วง -999.99 ถึง 999.99) เท่านั้น")
                        ElseIf (CDbl(value) < -999.99) Or (CDbl(value) > 999.99) Then
                            Throw New FormatException("คะแนนเป็นค่าว่าง หรือ ตัวเลข เท่านั้น (ตัวเลขอยู่ในช่วง -999.99 ถึง 999.99) เท่านั้น")
                        End If
                        _Score = value
                    End If
                End If
            End Set
        End Property

    End Class

    Public Class CAList
        Public Property CloseApp() As List(Of CloseApp)
            Get
                Return _CloseApp
            End Get
            Set(ByVal value As List(Of CloseApp))
                _CloseApp = value
            End Set
        End Property
        Private _CloseApp As List(Of CloseApp)
    End Class

    <Serializable()>
    Public Class CloseApp

        Private _TimeStamp As String
        Public Property TimeStamp As String
            Get
                Return _TimeStamp
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If ClassDroidPad.CheckStrDate(value) = True Then
                        _TimeStamp = value
                    End If
                End If
            End Set
        End Property

        Private _StationId As String
        Public Property StationId As String
            Get
                Return _StationId
            End Get
            Set(ByVal value As String)
                If value Is Nothing Then
                    APIErrorFlag = True
                Else
                    If value.Trim().Length = 0 Then
                        Throw New ArgumentException("StationId ต้องไม่เป็นค่าว่าง")
                    Else
                        _StationId = value
                    End If
                End If
            End Set
        End Property

        'Private _AppId As String
        'Public Property AppId As String
        '    Get
        '        Return _AppId
        '    End Get
        '    Set(ByVal value As String)
        '        If value.Trim().Length = 0 Then
        '            Throw New ArgumentException("AppId ต้องไม่เป็นค่าว่าง")
        '        Else
        '            _AppId = value
        '        End If
        '    End Set
        'End Property

        'Private _AppKey As String
        'Public Property AppKey As String
        '    Get
        '        Return _AppKey
        '    End Get
        '    Set(value As String)
        '        If value.Trim.Length = 32 Then
        '            _AppKey = value
        '        Else
        '            Throw New ArgumentException("AppKey ต้องมีค่า 32 หลักเท่านั้น")
        '        End If
        '    End Set
        'End Property

    End Class
#End Region




#End Region



#Region "UserDataByLeaderStdId"
    ''' <summary>
    ''' ทำการหา JsonString ของนักเรียน โดยใช้ รหัสนักเรียน
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <returns>String:ItemJsonData</returns>
    ''' <remarks></remarks>
    Public Function GetJsonDataByStudentId(ByVal StudentId As String) As String
        Dim sql As String = " SELECT ItemJsonData FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' "
        Dim JsonData As String = _DB.ExecuteScalar(sql)
        Return JsonData
    End Function
#End Region


    Public Shared Function CheckStrDate(ByVal InputStrDate As String) As Boolean
        Dim ReturnValue As Boolean = True

        If InputStrDate.Trim().Length <> 19 Then
            Throw New ArgumentException("Format วันที่ไม่ถูกต้อง มีความยาวไม่เท่ากับ 19 ตัวหนังสือ")
        End If

        If CInt(InputStrDate.Substring(11, 2)) > 24 Then
            Throw New ArgumentException("Format ชั่วโมงไม่ถูกต้อง ต้องไม่เกิน 24 ")
        End If

        If CInt(InputStrDate.Substring(14, 2)) > 59 Then
            Throw New ArgumentException("Format นาทีไม่ถูกต้อง ต้องไม่เกิน 59 ")
        End If

        If CInt(InputStrDate.Substring(17, 2)) > 59 Then
            Throw New ArgumentException("Format เสี้ยววินาทีไม่ถูกต้อง ต้องไม่เกิน 59")
        End If

        If CInt(InputStrDate.Substring(0, 4)) > 2500 Then
            Throw New ArgumentException("Format ปีไม่ถูกต้อง ต้องเป็นปี ค.ศ เท่านั้น")
        End If

        If CInt(InputStrDate.Substring(5, 2)) > 13 Then
            Throw New ArgumentException("Format เดือนไม่ถูกต้อง ตัวเลขเกิน 13")
        End If

        If CInt(InputStrDate.Substring(8, 2)) > 31 Then
            Throw New ArgumentException("Format วันที่ไม่ถูกต้อง วันที่ต้องไม่เกิน 31")
        End If

        Dim tmpDate As New DateTime()

        Dim InputYear As Integer = InputStrDate.Substring(0, 4)

        If InputYear < 2500 Then
            InputYear += 543
        End If

        InputStrDate = InputStrDate.Replace(InputStrDate.Substring(0, 4), InputYear)

        If DateTime.TryParse(InputStrDate, tmpDate) = False Then
            Throw New ArgumentException("Format วันที่ไม่ถูกต้อง")
        End If

        Return ReturnValue

    End Function

    Public Function GetActiveQuiz(DeviceId As String) As String

        'Dim  As String = GetUserIdByDeviceId(DeviceId)

        Dim sql As String
        sql = " select top 1 case when EndTime IS NULL Then 'True' Else 'False' end as IsActiveQuiz"
        sql &= " from tblquiz inner join tblQuizSession on tblquiz.Quiz_Id = tblQuizSession.Quiz_Id "
        sql &= " inner join t360_tblTablet on t360_tblTablet.Tablet_Id = tblQuizSession.Tablet_Id"
        sql &= " where t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "'"
        sql &= " order by tblquiz.lastupdate desc"

        Dim IsActiveQuiz As String = _DB.ExecuteScalar(sql)

        Return IsActiveQuiz
    End Function

    ''' <summary>
    ''' เพิ่มนักเรียนแบบ นักเรียนที่มีการซ้ำกับนักเรียนในห้อง
    ''' </summary>
    ''' <param name="ClassId">รหัสชั้น</param>
    ''' <param name="_Obj">ตัวแปร connection</param>
    ''' <returns>String:Level_Id</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function AddDuplicateStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String, ByVal Gender As String) As String
        Dim StudentID As String
        Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
        Dim Returnvalue As String = ""
        Dim ItemJsonData As String = ""

        _DB.OpenWithTransection()

        'update คนเก่า isactive = 0  ที่ student & studentroom
        Dim StudentIdDuplicate As String = redis.Getkey(DeviceUniqueID & "_Duplicate")
        If StudentIdDuplicate <> "" Then
            RemoveStudentDuplicate(StudentIdDuplicate, _DB)
        End If

        'insert คนใหม่ เข้าไป
        Try
            'ทำการ insert ข้อมูลลง t360_tblStudent,t360_tblStudentRoom
            StudentID = InsertStudentWithTransactionAndGetStudentId(FirstName, LastName, SchoolId, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender) 'Insert นักเรียน ลงตาราง 360
            If StudentID = "-1" Then
                _DB.RollbackTransection()
                Return "-1"
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "-1"
        End Try

        'ทำตามกระบวนการเดิมของการเพิ่มนักเรียน()
        If StudentID <> "" Then
            'เช็คว่า Tablet เครื่องนี้ผูกกับนักเรียนคนอื่นอยู่หรือเปล่า
            If CheckTabletIsManyPeople(DeviceUniqueID, StudentID, _DB) = True Then
                _DB.RollbackTransection()
                ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                Return "{""Param"": {""StudentInfo"" : """"}," & ItemJsonData & "}"
            End If

            Dim Tablet_ID As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, True)

            If Tablet_ID <> "" Then
                'update เครื่องของ owner ที่เคยใช้ ให้ isactive = 0 และที่ tablet ให้มี isowner = 1 เพราะต้องมีเจ้าของแน่นอน (เครื่องเก่าอาจจะเคยเป็นเครื่องสำรอง)
                Dim sql As String = String.Format("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0 WHERE TabletOwner_IsActive = 1 AND Tablet_Id = '{0}';UPDATE t360_tblTablet SET Tablet_IsOwner = 1 WHERE Tablet_Id = '{0}';", Tablet_ID)
                _DB.ExecuteScalarWithTransection(sql)

                'Insert Tablet ผูกกับนักเรียนคนนี้
                Returnvalue = InsertTeacherOrStudentInTabOwner(SchoolId, Tablet_ID, StudentID, _DB, False)
                If Returnvalue = "-1" Then
                    _DB.RollbackTransection()
                    Return Returnvalue
                End If
                Try
                    _DB.CommitTransection()
                    ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                    redis.DEL(DeviceUniqueID & "_Duplicate")
                    Return "{""Param"": {""StudentInfo"" : """ & StudentID & """}," & ItemJsonData & "}"
                Catch ex As Exception
                    Return "-1"
                End Try
            End If
        End If

        _DB.RollbackTransection()
        Return "-1"

    End Function

    Private Function CheckDuplicateStudent(ByVal SchoolId As String, ByVal StudentCode As String, StudentNumber As String, ClassName As String, RoomName As String, DeviceUniqueId As String, ByRef ObjDB As ClsConnect) As String
        Dim CalendarId As String = GetCalendarID(SchoolId)
        Dim sql As New StringBuilder
        ' หาก่อนว่ารหัสนี้มีคนใช้หรือเปล่า
        sql.Append("SELECT * FROM t360_tblStudent s INNER JOIN t360_tblStudentRoom sr ON s.Student_Id = sr.Student_Id ")
        sql.Append(String.Format(" WHERE s.Student_Code = '{0}' AND s.School_Code = '{1}' AND sr.Calendar_Id = '{2}' AND s.Student_IsActive = 1;", StudentCode, SchoolId, CalendarId))
        Dim dt As DataTable = ObjDB.getdataWithTransaction(sql.ToString())
        If dt.Rows.Count > 0 Then
            redis.SetKey(DeviceUniqueId & "_Duplicate", dt.Rows(0)("Student_Id").ToString())
            Return String.Format("{0} {1}  เลขที่ {2} {3}{4} รหัส {5}", dt.Rows(0)("Student_FirstName"), dt.Rows(0)("Student_LastName"), dt.Rows(0)("Student_CurrentNoInRoom"), dt.Rows(0)("Student_CurrentClass"), dt.Rows(0)("Student_CurrentRoom"), dt.Rows(0)("Student_Code"))
        End If

        ' หาว่าเลขที่นี้มีซ้ำกันในห้องที่กำลังจะลงทะเบียนหรือยัง
        sql.Clear()
        sql.Append(" SELECT * FROM t360_tblStudent s INNER JOIN t360_tblStudentRoom sr ON s.Student_Id = sr.Student_Id ")
        sql.Append(String.Format(" WHERE s.Student_CurrentNoInRoom = '{0}' AND s.Student_CurrentClass = '{1}' AND s.Student_CurrentRoom = '/{2}' ", StudentNumber, FindClassName(ClassName), RoomName))
        sql.Append(String.Format(" AND s.School_Code = '{0}' AND sr.Calendar_Id = '{1}' AND s.Student_IsActive = 1;", SchoolId, CalendarId))
        dt = ObjDB.getdataWithTransaction(sql.ToString())
        If dt.Rows.Count > 0 Then
            redis.SetKey(DeviceUniqueId & "_Duplicate", dt.Rows(0)("Student_Id").ToString())
            Return String.Format("{0} {1}  เลขที่ {2} {3}{4} รหัส {5}", dt.Rows(0)("Student_FirstName"), dt.Rows(0)("Student_LastName"), dt.Rows(0)("Student_CurrentNoInRoom"), dt.Rows(0)("Student_CurrentClass"), dt.Rows(0)("Student_CurrentRoom"), dt.Rows(0)("Student_Code"))
        End If

        ' check แล้วว่า ไม่ซ้ำกับใครเลย ให้ส่ง string ปล่าวกับไป เพื่อทำการลงทะเบียนตาม logic เดิม
        Return ""
    End Function

    Private Sub RemoveStudentDuplicate(StudentIdDuplicate As String, ByRef ObjDB As ClsConnect)
        Dim sql As New StringBuilder
        sql.Append(String.Format("UPDATE t360_tblStudent SET Student_IsActive = 0,Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '{0}';", StudentIdDuplicate))
        sql.Append(String.Format("UPDATE t360_tblStudentRoom SET SR_IsActive = 0,Lastupdate = dbo.GetThaiDate() WHERE Student_Id = '{0}';", StudentIdDuplicate))
        sql.Append(String.Format("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0,Lastupdate = dbo.GetThaiDate() WHERE Owner_Id = '{0}' AND TabletOwner_IsActive = 1;", StudentIdDuplicate))
        ObjDB.ExecuteScalarWithTransection(sql.ToString())
    End Sub

    Private Function IsStudentRegisterWithTablet(DeviceUniqueID As String, StudentID As String, ByRef ObjDB As ClsConnect) As String
        Dim sql As New StringBuilder
        ' เช็คก่อนว่ามีเครื่อง tablet ที่เคยลงทะเบียนไว้มั้ย
        sql.Append("SELECT * FROM t360_tblStudent s INNER JOIN t360_tblTabletOwner t ON s.Student_Id = t.Owner_Id ")
        sql.Append(String.Format(" WHERE t.TabletOwner_IsActive = 1 AND t.Owner_Id = '{0}';", StudentID))
        Dim dt As DataTable = ObjDB.getdataWithTransaction(sql.ToString())
        If dt.Rows.Count > 0 Then
            Dim tempDuplicateStudent As String = String.Format("{0} {1}  เลขที่ {2} {3}{4} รหัส {5}", dt.Rows(0)("Student_FirstName"), dt.Rows(0)("Student_LastName"), dt.Rows(0)("Student_CurrentNoInRoom"), dt.Rows(0)("Student_CurrentClass"), dt.Rows(0)("Student_CurrentRoom"), dt.Rows(0)("Student_Code"))
            ' เครื่องที่ลงเป็นเครื่องเก่าของตัวเองหรือเปล่า
            sql.Clear()
            sql.Append(" SELECT * FROM t360_tblTablet t INNER JOIN t360_tblTabletOwner o ON t.Tablet_Id = o.Tablet_Id WHERE t.Tablet_IsActive = 1 AND o.TabletOwner_IsActive = 1 AND ")
            sql.Append(String.Format(" t.Tablet_SerialNumber = '{0}' AND o.Owner_Id = '{1}';", DeviceUniqueID, StudentID))
            dt = ObjDB.getdataWithTransaction(sql.ToString())
            If dt.Rows.Count > 0 Then
                Return ""
            End If
            Return tempDuplicateStudent
        End If
        Return ""
    End Function

    Function ChangeTablet(DeviceUniqueID As String, FirstName As String, LastName As String, StudentClass As String, Room As String, StudentCode As String, NumberInRoom As String, Gender As String, HasOwner As Boolean) As String
        Dim Returnvalue As String
        Dim ItemJsonData As String
        Dim SchoolId As String = GetSchoolCodeFromApplication(DeviceUniqueID)
        Dim StudentID As String = GetStudentIdByStudentCode(SchoolId, StudentCode, NumberInRoom, StudentClass, Room)

        _DB.OpenWithTransection()

        'update isactive = 0 สำหรับเครื่องที่เคยใช้อยู่ก่อน
        Dim sql As New StringBuilder
        sql.Append("DECLARE @tabme AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_tblTabletOwner WHERE TabletOwner_IsActive = 1 AND Owner_Id = '{0}'); ")
        sql.Append("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0 ,LastUpdate = dbo.GetThaiDate() WHERE TabletOwner_IsActive = 1 AND Tablet_Id = @tabme AND Owner_Id = '{0}';")
        If HasOwner Then
            sql.Append("DECLARE @tabother AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_tblTablet WHERE Tablet_IsActive = 1 AND Tablet_SerialNumber = '{1}'); ")
            sql.Append("UPDATE t360_tblTabletOwner SET TabletOwner_IsActive = 0 ,LastUpdate = dbo.GetThaiDate() WHERE TabletOwner_IsActive = 1 AND Tablet_Id = @tabother;")
        End If
        _DB.ExecuteWithTransection(String.Format(sql.ToString(), StudentID, DeviceUniqueID))

        Dim Tablet_ID As String = GetTabletIdPerSchoolFromDeviceId(DeviceUniqueID, SchoolId, _DB, True)
        If Tablet_ID <> "" Then
            'Insert Tablet ผูกกับนักเรียนคนนี้
            Returnvalue = InsertTeacherOrStudentInTabOwner(SchoolId, Tablet_ID, StudentID, _DB, False)
            If Returnvalue = "-1" Then
                _DB.RollbackTransection()
                Return Returnvalue
            End If
            Returnvalue = UpdateStudentInfo(StudentID, FirstName, LastName, Gender, _DB)
            If Returnvalue = "-1" Then
                _DB.RollbackTransection()
                Return Returnvalue
            End If
            Try
                _DB.CommitTransection()
                ItemJsonData = GetItemJsonDataByStudentId(StudentID)
                Return "{""Param"": {""StudentInfo"" : """ & StudentID & """}," & ItemJsonData & "}"
            Catch ex As Exception
                Return "-1"
            End Try
        End If

        _DB.RollbackTransection()
        Return "-1"
    End Function

    Public Function NewStudentTest(ByVal FirstName As String, ByVal LastName As String, ByVal SchoolId As String, ByVal StudentCode As String, ByVal StudentClass As String, ByVal Room As String, ByVal NumberInRoom As String, ByVal Gender As String) As String
        _DB.OpenWithTransection()
        If InsertStudentWithTransactionAndGetStudentId(FirstName, LastName, SchoolId, StudentCode, StudentClass, Room, NumberInRoom, _DB, Gender) = "-1" Then
            _DB.RollbackTransection()
            Return "-1"
        End If
        _DB.CommitTransection()
        Return "OK"
    End Function

    Public Function Reset() As String
        _DB.OpenWithTransection()
        Try
            _DB.ExecuteScalarWithTransection("DELETE t360_tblRoom;DELETE t360_tblStudent;DELETE t360_tblStudentRoom;DELETE t360_tblTablet;DELETE t360_tblTabletOwner;")
            InsertStudentWithTransactionAndGetStudentId("สมชาย", "หายหัว", "1000001", "2001", "k5", "1", "1", _DB, "m")
            InsertStudentWithTransactionAndGetStudentId("นคร", "ราชสีมา", "1000001", "9001", "k6", "1", "1", _DB, "m")
            InsertStudentWithTransactionAndGetStudentId("จิตใจ", "ใฝ่ดี", "1000001", "0021", "k4", "2", "1", _DB, "m")

            _DB.CommitTransection()
            Return "OK"
        Catch ex As Exception
            _DB.RollbackTransection()
            Return "-1"
        End Try
    End Function

    Public Function ResetMaxOnet() As String
        _DB.OpenWithTransection()
        Try
            _DB.ExecuteScalarWithTransection("DELETE maxonet_tblKeyCode;DELETE maxonet_tblKeyCodeUsage;")
            _DB.ExecuteScalarWithTransection("DELETE t360_tblTablet;DELETE t360_tblTabletOwner;DELETE t360_tblStudent;DELETE t360_tblStudentRoom;DELETE maxonet_tblStudentSubject;")
            _DB.ExecuteScalarWithTransection("INSERT INTO maxonet_tblKeyCode VALUES('123456789101','a',dbo.GetThaiDate(),NULL,NULL,1,1,dbo.GetThaiDate(),0,NULL,'A0000B0000');")
            _DB.ExecuteScalarWithTransection("INSERT INTO maxonet_tblKeyCode VALUES('123456789102','b',dbo.GetThaiDate(),NULL,NULL,1,1,dbo.GetThaiDate(),0,NULL,'C0000D0000');")
            _DB.ExecuteScalarWithTransection("INSERT INTO maxonet_tblKeyCode VALUES('112233445566','fastcat',dbo.GetThaiDate(),NULL,NULL,1,1,dbo.GetThaiDate(),0,NULL,'C0001D0001');")
            _DB.ExecuteScalarWithTransection("INSERT INTO maxonet_tblKeyCode VALUES('000011112222','bigdog',dbo.GetThaiDate(),NULL,NULL,1,1,dbo.GetThaiDate(),0,NULL,'C0002D0002');")
            _DB.CommitTransection()
            Return "OK"
        Catch ex As Exception
            _DB.RollbackTransection()
            Return "-1"
        End Try
    End Function

    Public Function InsertTabletStatusDetail(TabletId As String, TabletName As String, SchoolCode As String, ByRef ObjDB As ClsConnect, TabletOwnerType As EnTabletOwnerType) As String

        Dim sql As String
        Try
            'Update Status เดิมให้ IsActive = 0 ก่อน
            sql = "Update t360_tblTabletStatusDetail set IsActive = 0,LastUpdate = dbo.GetThaiDate() where Tablet_Id = '" & TabletId & "';"
            ObjDB.ExecuteWithTransection(sql)

            sql = "Insert into t360_tblTabletStatusDetail(Tablet_Id,TSD_Status,TSD_TabletType,TSD_Remark,School_Code) values('" & TabletId & "',1,'" & TabletOwnerType & "','" & TabletName & "','" & SchoolCode & "');"
            ObjDB.ExecuteWithTransection(sql)

            Return "1"
        Catch ex As Exception
            Return "-1"
        End Try


    End Function
End Class


