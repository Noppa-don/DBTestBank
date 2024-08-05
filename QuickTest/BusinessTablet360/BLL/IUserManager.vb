Imports KnowledgeUtils.Encode.ManageEncode
Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils
Imports System.Text
Imports System.Web

''' <summary>
''' User
''' (PK Database) School_Code, User_Id 
''' (PK Real) School_Code, User_Id, UserName
''' 
''' MenuItem
''' (PK Database) MenuItem_Code 
''' 
''' UserMenuItem
''' (PK Database) School_Code, User_Id, MenuItem_Code
''' </summary>
''' <remarks></remarks>
Public Interface IUserManager

    '<<< User
    Function GetUserByCrit(Of T)(ByVal Item As UserDTO) As T()
    Function InsertUser(ByVal Item As t360_tblUser, ByVal MenuItems As t360_tblUserMenuItem()) As Boolean
    Function UpdateUser(ByVal Item As t360_tblUser, ByVal MenuItems As t360_tblUserMenuItem()) As Boolean
    Function UpdatePassword(ByVal NewPassword As String) As Boolean
    Function DeleteUser(ByVal User_Id As Guid) As Boolean
    Function ValidateDuplicateUser(ByVal User_Name As String, Optional ByVal User_Id As Guid? = Nothing) As Boolean

    ' Pointplus User
    Function ValidateDuplicateUserPointplus(ByVal User_Name As String, Optional ByVal User_Id As Guid? = Nothing) As Boolean

    ''' <summary>
    ''' เช็คความถูกต้องของ User Password
    ''' </summary>
    ''' <param name="User_Name"></param>
    ''' <param name="User_Password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CheckPassword(ByVal User_Name As String, ByRef User_Password As String, ByVal SchoolCode As String) As Guid?

    '<<< MenuItem
    Function GetMenuItemByCrit(Of T)(ByVal Item As MenuItemDTO) As T()

    '<<< UserMenuItem
    Function GetUserMenuItemByCrit(Of T)(ByVal Item As UserDTO) As T()
    Function GetPermission(ByVal EventPermission As EnPermission, Optional ByVal User_Id As Guid? = Nothing) As Boolean

    '<<< Log
    Function InsertLog(ByVal Item As t360_tblLog) As Boolean
    Function GetLogBySchoolIdAndType(Of T)(ByVal SelectedType As String) As T()
    Sub Log(ByVal LogType As String, ByVal LogDescription As String)

    '<<< Mobile
    Function GetAllMobileAccessPasswordByScoolCode(ByVal SchoolCode As String) As tblMobileAccessPassword()
    Function GetMobileAccessPasswordByMapTypeAndScoolCode(ByVal SchoolCode As String, ByVal Map_Type As Integer) As tblMobileAccessPassword
    Function GetMobileAccessPasswordByScoolCodeAndPassword(ByVal SchoolCode As String, ByVal GeneratedPassword As String) As tblMobileAccessPassword
    Function GetMobileRegistrationByDeviceId(ByVal DeviceId As String) As tblMobileRegistration
    Function GetMobileRegistrationByMapId(ByVal Map_Id As Guid) As tblMobileRegistration()
    Function GetAllMobileRegistrationByMapTypeAndScoolCode(ByVal SchoolCode As String, ByVal Map_Type As Integer) As tblMobileRegistration()
    Function CheckDuplicateGenPass(ByVal Password As String) As Boolean
    Function InsertMobileAccessPassword(ByVal Item As tblMobileAccessPassword, ByVal OldMap_Id As Guid?, ByVal IsDeleteRegister As Boolean) As tblMobileAccessPassword
    Function InsertMobileRegistration(ByVal Item As tblMobileRegistration) As tblMobileRegistration
    Function DeleteMobileRegistration(ByVal MR_Ids As Guid()) As Boolean
    Function AccessMobile(ByVal DeviceId As String) As Boolean

    'System
    Function GetSetEmailBySchoolCode(ByVal SchoolCode As String) As tblSetEmail
    Function GetUserSubjectClassByUser(GUID As Guid) As tblUserSubjectClass()
    Function GetUserByUserId(GUID As Guid) As tblUser
    Function GetUserByUserName(UserName As String, ByVal SchoolCode As String) As tblUser


End Interface

Public Class UserManager
    'Inherits DatabaseManager
    Implements IUserManager

#Region "Dependency"

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

#End Region

#Region "User"

    Public Function DeleteUser(ByVal User_Id As Guid) As Boolean Implements IUserManager.DeleteUser
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Item = (From r In Ctx.t360_tblUsers Where r.User_Id = User_Id).SingleOrDefault
                Item.User_IsActive = False
                Item.LastUpdate = GetTime
                Item.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Item = (From r In Ctx.t360_tblUsers Where r.User_Id = User_Id).SingleOrDefault
                Dim LogDetail As New StringBuilder
                LogDetail.Append("ผู้ดูแล -ชื่อสกุล: ")
                LogDetail.Append(Item.User_FirstName & " " & Item.User_LastName)
                LogDetail.Append(" ,t360_tblUser.Id=")
                LogDetail.Append(Item.User_Id.ToString)
                Log(EnLogType.ImportantDelete, LogDetail.ToString)
            End Using

        End Try
    End Function

    Public Function InsertUser(ByVal Item As t360_tblUser, ByVal MenuItems As t360_tblUserMenuItem()) As Boolean Implements IUserManager.InsertUser
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                With Item
                    .User_Id = Guid.NewGuid
                    .School_Code = UserConfig.GetCurrentContext.School_Code
                    .User_IsActive = True
                    .LastUpdate = GetTime
                    .User_Salt = GetSalt()
                    .User_Password = Encode(.User_Password, .User_Salt)
                End With
                For Each Row In MenuItems
                    Row.UMI_Id = Guid.NewGuid
                    Row.IsActive = True
                    Row.LastUpdate = GetTime
                Next
                Item.t360_tblUserMenuItems.AddRange(MenuItems)
                Ctx.t360_tblUsers.InsertOnSubmit(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ผู้ดูแล-ชื่อสกุล: ")
            LogDetail.Append(Item.User_FirstName & " " & Item.User_LastName)
            LogDetail.Append(",t360_tblUser.Id=" & Item.User_Id.ToString)
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = LogDetail.ToString
            End With
            Me.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateUser(ByVal Item As t360_tblUser, ByVal MenuItems As t360_tblUserMenuItem()) As Boolean Implements IUserManager.UpdateUser
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Target As New t360_tblUser
                Target = (From r In Ctx.t360_tblUsers Where r.User_Id = Item.User_Id).SingleOrDefault
                With Item
                    Target.User_Name = .User_Name
                    Target.User_FirstName = .User_FirstName
                    Target.User_LastName = .User_LastName
                    Target.User_Email = .User_Email
                    Target.User_Phone = .User_Phone
                    Target.LastUpdate = GetTime
                    Target.ClientId = Nothing

                    If Not .User_Password Is Nothing Then
                        Target.User_Salt = GetSalt()
                        Target.User_Password = Encode(.User_Password, Target.User_Salt)
                    End If
                    'UserMenuItem
                    Dim ItemUserMenuItem = Ctx.t360_tblUserMenuItems.Where(Function(q) q.User_Id = Target.User_Id And q.IsActive = True).ToArray
                    For Each Row In ItemUserMenuItem
                        Row.LastUpdate = GetTime
                        Row.IsActive = False
                        Row.ClientId = Nothing
                    Next

                    For Each row In MenuItems
                        row.UMI_Id = Guid.NewGuid
                        row.LastUpdate = GetTime
                        row.IsActive = True
                    Next
                    Target.t360_tblUserMenuItems.AddRange(MenuItems)
                End With
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ผู้ดูแล -ชื่อสกุล: ")
            LogDetail.Append(Item.User_FirstName & " " & Item.User_LastName)
            LogDetail.Append(" ,t360_tblUser.Id=")
            LogDetail.Append(Item.User_Id.ToString)
            Log(EnLogType.ImportantUpdate, LogDetail.ToString)
        End Try
    End Function

    Public Function UpdatePassword(ByVal NewPassword As String) As Boolean Implements IUserManager.UpdatePassword
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Target As New t360_tblUser
                Target = (From r In Ctx.t360_tblUsers Where r.User_Id = UserConfig.GetCurrentContext.User_Id).SingleOrDefault
                Target.User_Salt = GetSalt()
                Target.User_Password = Encode(NewPassword, Target.User_Salt)
                Target.LastUpdate = GetTime
                Target.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Page = "เปลี่ยนรหัสผ่านส่วนตัว"
            End With
            Me.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateUser(ByVal User_Name As String, Optional ByVal User_Id As Guid? = Nothing) As Boolean Implements IUserManager.ValidateDuplicateUser
        Using Ctx = GetLinqToSql.GetDataContext
            If User_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblUsers Where r.User_Name = User_Name AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.User_IsActive = True).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim q = (From r In Ctx.t360_tblUsers Where r.User_Name = User_Name AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.User_IsActive = True AndAlso r.User_Id <> User_Id).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Using
    End Function

    Public Function ValidateDuplicateUserPointplus(ByVal User_Name As String, Optional ByVal User_Id As Guid? = Nothing) As Boolean Implements IUserManager.ValidateDuplicateUserPointplus
        Using Ctx = GetLinqToSql.GetDataContext
            If User_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblUsers Where r.User_Name = User_Name AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.User_IsActive = True).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim q = (From r In Ctx.tblUsers Where r.UserName = User_Name AndAlso r.SchoolId = UserConfig.GetCurrentContext.School_Code AndAlso r.IsActive = True AndAlso r.GUID <> User_Id).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Using
    End Function

    Public Function GetUserByCrit(Of T)(ByVal Item As UserDTO) As T() Implements IUserManager.GetUserByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblUser WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("School_Code={0}", Item.School_Code)
            f.AddPart("User_IsActive={0}", Item.User_IsActive)
            f.AddPart("User_Name={0}", Item.User_Name)
            f.AddPart("User_Id={0}", Item.User_Id)

            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

    Public Function CheckPassword(ByVal User_Name As String, ByRef User_Password As String, ByVal SchoolCode As String) As Guid? Implements IUserManager.CheckPassword
        'ถ้าเป็น admin เช็คก่อนว่ามี user นี้ยังถ้ายังจะ add ให้โดยมีรหัส kl123 ของโรงเรียนนั้นๆ
        If User_Name = "admin" Then
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim UserAdmin = (From q In Ctx.t360_tblUsers Where q.User_IsActive = True And q.User_Name = User_Name And q.School_Code = SchoolCode).SingleOrDefault
                If UserAdmin Is Nothing Then
                    Dim NewAdmin As New t360_tblUser
                    With NewAdmin
                        .LastUpdate = GetTime
                        .School_Code = SchoolCode
                        .User_FirstName = "admin"
                        .User_Id = Guid.NewGuid
                        .User_IsActive = True
                        .User_Name = "admin"
                        .User_Salt = GetSalt()
                        .User_Password = Encode("kl123", .User_Salt)
                    End With
                    Dim AllRole = Ctx.t360_tblMenuItems.Where(Function(q) q.MenuItem_IsActive = True)
                    For Each Row In AllRole
                        Dim NewUserMenu As New t360_tblUserMenuItem
                        With NewUserMenu
                            .IsActive = True
                            .LastUpdate = GetTime
                            .MenuItem_Code = Row.MenuItem_Code
                            .UMI_Id = Guid.NewGuid
                            .School_Code = SchoolCode
                        End With
                        NewAdmin.t360_tblUserMenuItems.Add(NewUserMenu)
                    Next

                    Ctx.t360_tblUsers.InsertOnSubmit(NewAdmin)

                    Dim School = Ctx.tblSchools.Where(Function(q) q.SchoolId = SchoolCode).SingleOrDefault
                    If School IsNot Nothing Then
                        Dim NewSchoolT360 As New t360_tblSchool
                        With NewSchoolT360
                            .ClientId = Nothing
                            .LastUpdate = GetTime
                            .School_Code = SchoolCode
                            .School_Name = School.SchoolName
                            .SubDistrict_Id = School.TambolId
                            .District_Id = School.AmphurId
                            .Province_Id = School.ProvinceId
                            .School_IsActive = True
                            .GUID = School.GUID
                        End With
                        Ctx.t360_tblSchools.InsertOnSubmit(NewSchoolT360)
                    End If

                    Ctx.SubmitChanges()
                End If
            End Using
        End If

        Dim User = GetUserByCrit(Of t360_tblUser)(New UserDTO With {.User_Name = User_Name, .School_Code = SchoolCode}).SingleOrDefault
        If User IsNot Nothing Then
            Dim EncodePaword As String = Encode(User_Password, User.User_Salt)
            If User.User_Password = EncodePaword Then

                If UserConfig.GetCurrentContext.User_Id Is Nothing Then

                    Dim Sys As New Service.ClsSystem(New ClassConnectSql)

                    Dim NewConfig As New UserConfig
                    With NewConfig
                        .School_Code = SchoolCode
                        .User_Id = User.User_Id
                        .User_FirstName = User.User_FirstName
                        .User_LastName = User.User_LastName
                        .User_Name = User.User_Name
                        If Sys.GetCalendarId(SchoolCode) <> "" Then
                            .CurrentCalendar = New Guid(Sys.GetCalendarId(SchoolCode))
                            Using Ctx = GetLinqToSql.GetDataContext
                                Dim C = Ctx.t360_tblCalendars.Where(Function(q) q.Calendar_Id = .CurrentCalendar).SingleOrDefault
                                .CalendarYear = C.Calendar_Year
                                .CalendarName = C.Calendar_Name
                            End Using
                        End If
                        '.UseSync = CType(System.Configuration.ConfigurationManager.AppSettings("UseSync"), Boolean)
                    End With
                    UserConfig.SetCurrentContext(NewConfig)

                    Dim ItemLog As New t360_tblLog
                    With ItemLog
                        .Log_Type = EnLogType.Login
                        .Log_Page = "LoginPage.aspx"
                        .Log_Description = User.User_FirstName & " " & User.User_LastName
                    End With
                    InsertLog(ItemLog)

                End If


                Return User.User_Id
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

#End Region

#Region "MenuItem"

    Public Function GetMenuItemByCrit(Of T)(ByVal Item As MenuItemDTO) As T() Implements IUserManager.GetMenuItemByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblMenuItem WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("MenuItem_IsActive={0}", Item.MenuItem_IsActive)
            f.AddPart("MenuItem_Type={0}", Item.MenuItem_Type)
            f.AddPart("MenuItem_Parent={0}", Item.MenuItem_Parent)
            f.AddPart("MenuItem_Code={0}", Item.MenuItem_Code)

            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

#End Region

#Region "UserMenuItem"

    Public Function GetUserMenuItemByCrit(Of T)(ByVal Item As UserDTO) As T() Implements IUserManager.GetUserMenuItemByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblUserMenuItem WHERE {F} ORDER BY Lastupdate DESC"
            Dim f As New SqlPart
            f.AddPart("IsActive={0}", True)
            f.AddPart("User_Id={0}", Item.User_Id)
            f.AddPart("MenuItem_Code={0}", Item.MenuItem_Code)
            .ApplySqlPart("F", f)
            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

    Public Function GetPermission(ByVal EventPermission As EnPermission, Optional ByVal User_Id As Guid? = Nothing) As Boolean Implements IUserManager.GetPermission
        Using Ctx = GetLinqToSql.GetDataContext
            If User_Id Is Nothing Then
                User_Id = UserConfig.GetCurrentContext.User_Id
            End If
            Dim r = (From q In Ctx.t360_tblUserMenuItems Where q.User_Id = User_Id AndAlso q.MenuItem_Code = EventPermission AndAlso q.IsActive = True).ToArray
            If r.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Using
    End Function

#End Region

#Region "Log"

    Public Function InsertLog(ByVal Item As t360_tblLog) As Boolean Implements IUserManager.InsertLog
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                With Item
                    .Log_Id = Guid.NewGuid
                    .User_Id = UserConfig.GetCurrentContext.User_Id
                    .School_Code = UserConfig.GetCurrentContext.School_Code
                    .LastUpdate = GetTime
                    .Log_IsActive = True
                End With

                Ctx.t360_tblLogs.InsertOnSubmit(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Sub Log(LogType As String, LogDescription As String) Implements IUserManager.Log

        Dim ItemLog As New t360_tblLog
        With ItemLog
            .Log_Type = LogType
            .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            .Log_Description = LogDescription
        End With
        InsertLog(ItemLog)
    End Sub


    Public Function GetLogBySchoolIdAndType(Of T)(SelectedType As String) As T() Implements IUserManager.GetLogBySchoolIdAndType
        'Delete,Insert,Warning,Info
        Dim selectedLogType As String = CreateStringSearchSomeType(SelectedType)

        With GetLinqToSql
            'select * from t360_tblLog where School_Code = '1000001' order by 
            .MainSql = "select Log_Id,Log_Type,'a' as Log_TypeImage,'a' as Log_TypeName, " & _
                        " case when Log_Description is null Then '-' else Log_Description end as Log_Description, " & _
                        " case when Log_Description is null Then '-' else SUBSTRING(log_Description ,0,50) end as Log_DescriptionCutString, " & _
                        " User_FirstName + ' ' + User_LastName as UserName,t360_tblLog.LastUpdate as LastUpdate" & _
                        " FROM t360_tblLog INNER JOIN t360_tblUser ON t360_tblLog.User_Id = t360_tblUser.User_Id WHERE {F} "
            If selectedLogType <> "" Then
                .MainSql &= " And t360_tblLog.Log_Type in" & selectedLogType & ""
            End If


            Dim f As New SqlPart
            f.AddPart(" t360_tblLog.School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            'If CreateStringSearchSomeType(SelectedType) <> "" Then
            '    f.AddPart("t360_tblLog.Log_Type in{0}", selectedLogType)
            'End If

            .ApplySqlPart("F", f)
            Dim Logdata As IList(Of LogDTO) = .DataContextExecuteObjects(Of T)().ToArray

            For i As Integer = 0 To Logdata.Count - 1
                Logdata(i).Log_TypeName = (New EnumLogType).GetText(CInt(Logdata(i).Log_Type))
                Logdata(i).Log_TypeImage = GetLogTypeImage(Logdata(i).Log_Type)
            Next

            Return Logdata
        End With

    End Function

    Private Function CreateStringSearchSomeType(SelectedType) As String
        'Delete,Insert,Warning,Info
        If SelectedType = "0000" Or SelectedType = "1111" Then
            Return ""
        Else
            Dim stSqlForType As String = ""
            Select Case SelectedType
                Case "0001"
                    stSqlForType = "(1,2,3,4,7,17,18,19,20,23)"
                Case "0010"
                    stSqlForType = "(6,8,10,11,12,13,14,15,16,21,22)"
                Case "0100"
                    stSqlForType = "(5)"
                Case "1000"
                    stSqlForType = "(9)"

                Case "0011"
                    stSqlForType = "(1,2,3,4,6,7,8,10,11,12,13,14,15,16,17,18,19,20,21,22,23)"
                Case "0101"
                    stSqlForType = "(1,2,3,4,5,7,17,18,19,20,23)"
                Case "0110"
                    stSqlForType = "(5,6,8,10,11,12,13,14,15,16,21,22)"
                Case "0111"
                    stSqlForType = "(1,2,3,4,5,6,7,8,10,11,12,13,14,15,16,17,18,19,20,21,22,23)"
                Case "1001"
                    stSqlForType = "(1,2,3,4,7,9,17,18,19,20,23)"
                Case "1010"
                    stSqlForType = "(6,8,9,10,11,12,13,14,15,16,21,22)"
                Case "1011"
                    stSqlForType = "(1,2,3,4,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23)"
                Case "1100"
                    stSqlForType = "(5,9)"
                Case "1101"
                    stSqlForType = "(1,2,3,4,5,7,9,17,18,19,20,2)"
                Case "1110"
                    stSqlForType = "(5,6,8,9,10,11,12,13,14,15,16,21,22)"
            End Select

            Return stSqlForType
        End If


    End Function

    Private Function GetLogTypeImage(LogType As String)
        Dim ImageURL As String = ""

        If LogType = "1" Or LogType = "2" Or LogType = "3" Or LogType = "4" Or LogType = "7" Or LogType = "17" Or LogType = "18" Or LogType = "19" Or LogType = "20" Or LogType = "23" Then
            ImageURL = "~/Images/Log/View.png"
        ElseIf LogType = "6" Or LogType = "8" Or LogType = "10" Or LogType = "11" Or LogType = "12" Or LogType = "13" Or LogType = "14" Or LogType = "15" Or LogType = "16" Or LogType = "21" Or LogType = "22" Then
            ImageURL = "~/Images/Log/Warning.png"
        ElseIf LogType = "5" Then
            ImageURL = "~/Images/Log/Add.png"
        ElseIf LogType = "9" Then
            ImageURL = "~/Images/Log/Delete.png"
        End If

        Return ImageURL

    End Function

#End Region

#Region "Mobile"

    Public Function GetMobileAccessPasswordByMapTypeAndScoolCode(ByVal SchoolCode As String, ByVal Map_Type As Integer) As tblMobileAccessPassword Implements IUserManager.GetMobileAccessPasswordByMapTypeAndScoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileAccessPasswords Where q.SchoolCode = SchoolCode And q.Map_Type = Map_Type And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetMobileAccessPasswordByScoolCodeAndPassword(ByVal SchoolCode As String, ByVal GeneratedPassword As String) As tblMobileAccessPassword Implements IUserManager.GetMobileAccessPasswordByScoolCodeAndPassword
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileAccessPasswords Where q.GeneratedPassword = GeneratedPassword And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetAllMobileAccessPasswordByScoolCode(ByVal SchoolCode As String) As tblMobileAccessPassword() Implements IUserManager.GetAllMobileAccessPasswordByScoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileAccessPasswords Where q.SchoolCode = SchoolCode And q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetMobileRegistrationByMapId(ByVal Map_Id As System.Guid) As tblMobileRegistration() Implements IUserManager.GetMobileRegistrationByMapId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileRegistrations Where q.Map_Id = Map_Id And q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetMobileRegistrationByDeviceId(ByVal DeviceId As String) As tblMobileRegistration Implements IUserManager.GetMobileRegistrationByDeviceId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileRegistrations Where q.Device_Id = DeviceId And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetAllMobileRegistrationByMapTypeAndScoolCode(ByVal SchoolCode As String, ByVal Map_Type As Integer) As tblMobileRegistration() Implements IUserManager.GetAllMobileRegistrationByMapTypeAndScoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileAccessPasswords Join _
                    q1 In Ctx.tblMobileRegistrations On q.Map_Id Equals q1.Map_Id
                    Where q.SchoolCode = SchoolCode And q.Map_Type = Map_Type And q1.IsActive = True _
                    Select q1).ToArray
        End Using
    End Function

    Public Function CheckDuplicateGenPass(ByVal Password As String) As Boolean Implements IUserManager.CheckDuplicateGenPass
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblMobileAccessPasswords Where q.GeneratedPassword = Password And q.IsActive = True).SingleOrDefault IsNot Nothing
        End Using
    End Function

    Public Function InsertMobileAccessPassword(ByVal Item As tblMobileAccessPassword, ByVal OldMap_Id As Guid?, ByVal IsDeleteRegister As Boolean) As tblMobileAccessPassword Implements IUserManager.InsertMobileAccessPassword
        With GetLinqToSql
            Try
                Dim Ctx = .GetDataContextWithTransaction()
                If OldMap_Id IsNot Nothing Then
                    Dim Data = Ctx.tblMobileAccessPasswords.Where(Function(q) q.Map_Id = OldMap_Id).SingleOrDefault
                    Data.IsActive = False
                    Data.LastUpdate = Now
                    Data.ClientId = Nothing
                End If

                If OldMap_Id IsNot Nothing AndAlso IsDeleteRegister Then
                    Dim Registers = (From q In Ctx.tblMobileAccessPasswords Join _
                    q1 In Ctx.tblMobileRegistrations On q.Map_Id Equals q1.Map_Id
                    Where q.SchoolCode = Item.SchoolCode And q.Map_Type = Item.Map_Type And q1.IsActive = True _
                    Select q1).ToArray
                    For Each Row In Registers
                        Row.IsActive = False
                        Row.LastUpdate = Now
                        Row.ClientId = Nothing
                    Next
                End If
                Ctx.tblMobileAccessPasswords.InsertOnSubmit(Item)

                Ctx.SubmitChanges()
                .DataContextCommitTransaction()
                Return Item
            Catch ex As Exception
                .DataContextRollbackTransaction()
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End With
    End Function

    Public Function InsertMobileRegistration(ByVal Item As tblMobileRegistration) As tblMobileRegistration Implements IUserManager.InsertMobileRegistration
        With GetLinqToSql
            Try
                Dim Ctx = .GetDataContextWithTransaction()
                Item.LastUpdate = Now
                Ctx.tblMobileRegistrations.InsertOnSubmit(Item)

                Ctx.SubmitChanges()
                .DataContextCommitTransaction()
                Return Item
            Catch ex As Exception
                .DataContextRollbackTransaction()
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End With
    End Function

    Public Function DeleteMobileRegistration(ByVal MR_Ids As Guid()) As Boolean Implements IUserManager.DeleteMobileRegistration
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                For Each Id In MR_Ids
                    Dim Ori = (From q In Ctx.tblMobileRegistrations Where q.Mr_Id = Id).SingleOrDefault
                    Ori.IsActive = False
                    Ori.LastUpdate = Now
                    Ori.ClientId = Nothing
                Next

                Ctx.SubmitChanges()
                Return True
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function AccessMobile(ByVal DeviceId As String) As Boolean Implements IUserManager.AccessMobile
        Try
            Dim Sys As New Service.ClsSystem(New ClassConnectSql)
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Regis = (From MobileAccessPassword In Ctx.tblMobileAccessPasswords Join MobileRegistration In Ctx.tblMobileRegistrations
                                On MobileAccessPassword.Map_Id Equals MobileRegistration.Map_Id
                                 Where MobileRegistration.Device_Id = DeviceId And MobileRegistration.IsActive = True
                                  Select MobileAccessPassword, MobileRegistration).SingleOrDefault
                'If Regis Is Nothing Then
                '    Throw New Exception
                'End If

                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .School_Code = Regis.MobileAccessPassword.SchoolCode
                    Dim Calendar As Guid?
                    Dim C As String = Sys.GetCalendarId(Regis.MobileAccessPassword.SchoolCode)
                    If C <> "" Then
                        Calendar = New Guid(C)
                    End If
                    .CurrentCalendar = Calendar
                    .MobileAccessType = Regis.MobileAccessPassword.Map_Type
                    Select Case Regis.MobileAccessPassword.Map_Type
                        Case EnMobileAccessType.HeadArt
                            .GroupSubjectId = New Guid("73C4639B-267C-4B7E-B5A4-1B4EBB428019")
                        Case EnMobileAccessType.HeadForeign
                            .GroupSubjectId = New Guid("FB677859-87DA-4D8D-A61E-8A76566D69D8")
                        Case EnMobileAccessType.HeadHealth
                            .GroupSubjectId = New Guid("6A4A7294-F5A7-4D64-ADBC-73DC14377737")
                        Case EnMobileAccessType.HeadMath
                            .GroupSubjectId = New Guid("A4B9F5CB-2D3C-4F6A-8666-FD2620E69723")
                        Case EnMobileAccessType.HeadScience
                            .GroupSubjectId = New Guid("58802565-23BB-4F22-8238-E983AC781B0F")
                        Case EnMobileAccessType.HeadSocial
                            .GroupSubjectId = New Guid("FDA224D9-CEBE-4642-ACD0-D7F7282E36AE")
                        Case EnMobileAccessType.HeadThai
                            .GroupSubjectId = New Guid("E7EDF837-4A6A-4E69-A62D-158F26A2BB7D")
                        Case EnMobileAccessType.HeadWork
                            .GroupSubjectId = New Guid("47A224EF-3348-41B7-84D0-7250648F8706")
                        Case Else
                            .GroupSubjectId = Nothing
                    End Select
                End With
                UserConfig.SetCurrentContext(Config)

                Return True
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

#End Region

#Region "System"

    Public Function GetSetEmailBySchoolCode(SchoolCode As String) As tblSetEmail Implements IUserManager.GetSetEmailBySchoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblSetEmails Where q.SchoolId = SchoolCode And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetUserSubjectClassByUser(GUID As Guid) As tblUserSubjectClass() Implements IUserManager.GetUserSubjectClassByUser
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblUserSubjectClasses Where q.UserId = GUID And q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetUserByUserId(GUID As Guid) As tblUser Implements IUserManager.GetUserByUserId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblUsers Where q.GUID = GUID).SingleOrDefault
        End Using
    End Function

    Public Function GetUserByUserName(UserName As String, ByVal SchoolCode As String) As tblUser Implements IUserManager.GetUserByUserName
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblUsers Where q.UserName = UserName And q.IsActive = True And q.SchoolId.ToString = SchoolCode).SingleOrDefault
        End Using
    End Function

#End Region

 
End Class

