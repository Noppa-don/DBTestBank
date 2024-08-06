Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web
Imports System.Text

'Teacher_CurrentClass,Teacher_CurrentRoom ไม่ได้ใช้แล้วเพราะสามารถประจำชั้นหลายห้องได้
Public Interface ITeacherManager
    Function GetAllTeacher() As t360_tblTeacher()
    Function GetTeacherByCrit(Of T)(ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T()
    Function GetTeacherForChangePassword(Of T)(ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T()
    Function GetTeacherForChangePasswordFromId(Of T)(ByVal Item As TeacherDTO)
    Function GetTeacherFullById(TeacherId As Guid) As t360_tblTeacher
    Function GetTeacherCountByCrit(Of t)(ByVal Item As TeacherDTO) As Integer
    Function InsertTeacherImport(ByVal ItemTeacher As t360_tblTeacher, ByVal ItemTeacherRoom As t360_tblTeacherRoom, ByVal ItemUser As tblUser) As Boolean
    Function InsertTeacher(ByVal ItemTeacher As t360_tblTeacher, ItemUser As tblUser, ItemUserSubjectClasses As tblUserSubjectClass()) As t360_tblTeacher 'หน้าจะไม่ใช้แล้ว
    Function UpdateTeacher(ByVal ItemTeacher As t360_tblTeacher, ItemUser As tblUser, InsertUserSubjectClasses As tblUserSubjectClass(), DeletetUserSubjectClasses As Guid?()) As t360_tblTeacher
    Function UpdateTeacherPassword(ByVal TeacherId As Guid, TeacherNewPassword As String)

    Function UpdateTeacherImport(ByVal ItemTeacher As t360_tblTeacher, ByVal ItemTeacherRoom As t360_tblTeacherRoom, ByVal IsChangeRoom As Boolean, Username As String, Password As String) As Boolean
    Function DeleteTeacher(ByVal Item As t360_tblTeacher) As Boolean
    Function ValidateDuplicateTeacher(SchoolCode As String, ByVal Teacher_Code As String, Optional ByVal Teacher_Id As Guid? = Nothing) As Boolean

    Function ProcessUpdateTeacherRoom(SchoolCode As String) As Boolean

    Function GetTeacherClass(ByVal SchoolCode As String, ByVal Teacher_id As String) As t360_tblClass()


End Interface

Public Class TeacherManager
    Implements ITeacherManager

#Region "Dependency"

    Private _ClassRepo As IClassRepo
    <Dependency()> Public Property ClassRepo() As IClassRepo
        Get
            Return _ClassRepo
        End Get
        Set(ByVal value As IClassRepo)
            _ClassRepo = value
        End Set
    End Property

    Private _RoomRepo As IRoomRepo
    <Dependency()> Public Property RoomRepo() As IRoomRepo
        Get
            Return _RoomRepo
        End Get
        Set(ByVal value As IRoomRepo)
            _RoomRepo = value
        End Set
    End Property

    Private _TeacherRepo As ITeacherRepo
    <Dependency()> Public Property TeacherRepo() As ITeacherRepo
        Get
            Return _TeacherRepo
        End Get
        Set(ByVal value As ITeacherRepo)
            _TeacherRepo = value
        End Set
    End Property

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

    Private _UserManager As IUserManager
    <Dependency()> Public Property UserManager() As IUserManager
        Get
            Return _UserManager
        End Get
        Set(ByVal value As IUserManager)
            _UserManager = value
        End Set
    End Property



#End Region

    Public Function GetTeacherClass(SchoolCode As String, Teacher_id As String) As t360_tblClass() Implements ITeacherManager.GetTeacherClass
        ' Dim r As t360_tblClass()
        Using Ctx = GetLinqToSql.GetDataContext
            If SchoolCode = "" Then
                SchoolCode = UserConfig.GetCurrentContext.School_Code
            End If
            If Teacher_id = "" Then
                Teacher_id = UserConfig.GetCurrentContext.User_Id.ToString
            End If
            Return (From q1 In Ctx.t360_tblClasses Join q2 In Ctx.tblUserSubjectClasses On q1.Class_Order Equals q2.ClassId Where q2.UserId.ToString = Teacher_id AndAlso q1.Class_IsActive = True AndAlso q2.IsActive = True Order By q1.Class_Order Ascending Select q1).Distinct.ToArray
        End Using
    End Function

    Public Function GetAllTeacher() As t360_tblTeacher() Implements ITeacherManager.GetAllTeacher
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From q In Ctx.t360_tblTeachers Where q.Teacher_IsActive = True And
                        q.Teacher_Status = EnTeacherStatus.Teach).ToArray
            End Using
        End With
    End Function

    Public Function GetTeacherByCrit(Of T)(ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T() Implements ITeacherManager.GetTeacherByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return TeacherRepo.GetTeacherByCrit(Of T)(Ctx, Item, Page, PageRow)
        End Using
    End Function

    Public Function GetTeacherForChangePassword(Of T)(Item As TeacherDTO, Optional Page As Integer = 0, Optional PageRow As Integer = 50) As T() Implements ITeacherManager.GetTeacherForChangePassword
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return TeacherRepo.GetTeacherForChangePassword(Of T)(Ctx, Item, Page, PageRow)
        End Using
    End Function

    Public Function GetTeacherForChangePasswordFromId(Of T)(Item As TeacherDTO) As Object Implements ITeacherManager.GetTeacherForChangePasswordFromId
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return TeacherRepo.GetTeacherForChangePassword(Of T)(Ctx, Item)
        End Using
    End Function

    Public Function GetTeacherCountByCrit(Of t)(ByVal Item As TeacherDTO) As Integer Implements ITeacherManager.GetTeacherCountByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return TeacherRepo.GetTeacherByCrit(Of t)(Ctx, Item).Count
        End Using
    End Function

    Public Function GetTeacherFullById(TeacherId As Guid) As t360_tblTeacher Implements ITeacherManager.GetTeacherFullById
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Dim l As New System.Data.Linq.DataLoadOptions
                l.LoadWith(Of t360_tblTeacher)(Function(q) q.t360_tblTeacherRooms)
                l.LoadWith(Of t360_tblTeacher)(Function(q) q.tblAssistants)
                Ctx.LoadOptions = l

                Dim Data = (From q In Ctx.t360_tblTeachers Where q.Teacher_id = TeacherId).SingleOrDefault()
                Dim DeleteTr = Data.t360_tblTeacherRooms.Where(Function(q) q.TR_IsActive = False).ToArray
                Dim DeleteAss = Data.tblAssistants.Where(Function(q) q.IsActive = False).ToArray
                For Each Row In DeleteTr
                    Data.t360_tblTeacherRooms.Remove(Row)
                Next
                For Each Row In DeleteAss
                    Data.tblAssistants.Remove(Row)
                Next

                Return Data
            End Using
        End With
    End Function


    Public Function InsertTeacher(ItemTeacher As t360_tblTeacher, ItemUser As tblUser, ItemUserSubjectClasses As tblUserSubjectClass()) As t360_tblTeacher Implements ITeacherManager.InsertTeacher
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate

                For Each row In ItemTeacher.t360_tblTeacherRooms
                    row.LastUpdate = GetTime
                Next
                For Each row In ItemTeacher.tblAssistants
                    row.LastUpdate = GetTime
                Next
                ItemTeacher.LastUpdate = GetTime
                ItemTeacher.Teacher_IsActive = True
                Ctx.t360_tblTeachers.InsertOnSubmit(ItemTeacher)

                'user quick test
                Dim Max = Ctx.tblUsers.Select(Function(q) q.UserId).Max
                With ItemUser
                    .UserId = Max + 1
                    .IsActive = True
                    .LastUpdate = GetTime
                    .GUID = ItemTeacher.Teacher_id 'ใช้อันเดียวกับ teacher id
                    .IsAllowMenuAdminLog = True
                    .IsAllowMenuContact = True
                    .IsAllowMenuManageUserAdmin = True
                    .IsAllowMenuManageUserSchool = True
                    .IsAllowMenuSetEmail = True
                    .ClientId = Nothing
                End With
                Ctx.tblUsers.InsertOnSubmit(ItemUser)

                'user subject class
                Max = Ctx.tblUserSubjectClasses.Select(Function(q) q.USCId).Max
                For Each Row In ItemUserSubjectClasses
                    Max += 1
                    Row.USCId = Max
                    Row.UserIdOld = 1
                    Row.Detailid = 1
                    Row.IsActive = True
                    Row.LastUpdate = GetTime
                    Row.GUID = Guid.NewGuid
                    Row.UserId = ItemUser.GUID
                    Row.ClientId = Nothing
                Next
                Ctx.tblUserSubjectClasses.InsertAllOnSubmit(ItemUserSubjectClasses)

                Ctx.SubmitChanges()
            End Using

            Dim LogDetail As New StringBuilder
            LogDetail.Append("ครู-ชื่อสกุล: ")
            LogDetail.Append(ItemTeacher.Teacher_FirstName & " " & ItemTeacher.Teacher_LastName)
            LogDetail.Append(",t360_tblTeacher.Id=" & ItemTeacher.Teacher_id.ToString)
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = LogDetail.ToString
            End With
            UserManager.InsertLog(ItemLog)

            Return ItemTeacher
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    Public Function InsertTeacherImport(ByVal ItemTeacher As t360_tblTeacher, ByVal ItemTeacherRoom As t360_tblTeacherRoom, ByVal ItemUser As tblUser) As Boolean Implements ITeacherManager.InsertTeacherImport
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate

                ItemTeacher.Teacher_IsActive = True
                ItemTeacher.School_Code = UserConfig.GetCurrentContext.School_Code
                ItemTeacher.LastUpdate = GetTime

                ItemTeacherRoom.TR_IsActive = EnIsActiveStatus.Active
                ItemTeacherRoom.School_Code = ItemTeacher.School_Code
                ItemTeacherRoom.LastUpdate = GetTime

                ItemTeacher.t360_tblTeacherRooms.Add(ItemTeacherRoom)

                Ctx.t360_tblTeachers.InsertOnSubmit(ItemTeacher)

                'user quick test
                Dim Max = Ctx.tblUsers.Select(Function(q) q.UserId).Max
                With ItemUser
                    .UserId = Max + 1
                    .IsActive = True
                    .LastUpdate = GetTime
                    .GUID = ItemTeacher.Teacher_id 'ใช้อันเดียวกับ teacher id
                    .IsAllowMenuAdminLog = True
                    .IsAllowMenuContact = True
                    .IsAllowMenuManageUserAdmin = True
                    .IsAllowMenuManageUserSchool = True
                    .IsAllowMenuSetEmail = True
                    .ClientId = Nothing
                End With
                Ctx.tblUsers.InsertOnSubmit(ItemUser)

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
        End Try
    End Function

    Public Function UpdateTeacher(ItemTeacher As t360_tblTeacher, ItemUser As tblUser, InsertUserSubjectClasses As tblUserSubjectClass(), DeletetUserSubjectClasses As Guid?()) As t360_tblTeacher Implements ITeacherManager.UpdateTeacher
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate
                Dim Ori As t360_tblTeacher
                Dim AllTrsIdActive() As Guid
                Dim AllAstIdActive() As Guid
                Using ctx1 = GetLinqToSql.GetDataContext()
                    Dim l As New System.Data.Linq.DataLoadOptions
                    l.LoadWith(Of t360_tblTeacher)(Function(q) q.t360_tblTeacherRooms)
                    Ctx.LoadOptions = l
                    Ori = ctx1.t360_tblTeachers.Where(Function(q) q.Teacher_id = ItemTeacher.Teacher_id).SingleOrDefault
                    AllTrsIdActive = Ori.t360_tblTeacherRooms.Where(Function(q) q.TR_IsActive = True).Select(Function(q) q.TR_Id).ToArray
                    AllAstIdActive = Ori.tblAssistants.Where(Function(q) q.IsActive = True).Select(Function(q) q.Ass_Id).ToArray
                End Using


                'ลบ Room
                For Each Id In AllTrsIdActive
                    Dim Found = (ItemTeacher.t360_tblTeacherRooms.Where(Function(q) q.TR_Id = Id).SingleOrDefault IsNot Nothing)
                    If Not Found Then
                        Dim D = Ctx.t360_tblTeacherRooms.Where(Function(q) q.TR_Id = Id).SingleOrDefault
                        D.LastUpdate = GetTime
                        D.TR_IsActive = False
                        D.ClientId = Nothing
                    End If
                Next
                'เพิ่ม Room
                For Each Tr In ItemTeacher.t360_tblTeacherRooms
                    Tr.LastUpdate = GetTime
                    Tr.TR_IsActive = True
                    Dim OriTr As t360_tblTeacherRoom
                    Using ctx1 = GetLinqToSql.GetDataContext()
                        OriTr = ctx1.t360_tblTeacherRooms.Where(Function(q) q.TR_Id = Tr.TR_Id).SingleOrDefault
                    End Using
                    If OriTr Is Nothing Then
                        Ctx.t360_tblTeacherRooms.InsertOnSubmit(Tr)
                    End If
                Next
                'ลบ Ast
                For Each Id In AllAstIdActive
                    Dim Found = (ItemTeacher.tblAssistants.Where(Function(q) q.Ass_Id = Id).SingleOrDefault IsNot Nothing)
                    If Not Found Then
                        Dim D = Ctx.tblAssistants.Where(Function(q) q.Ass_Id = Id).SingleOrDefault
                        D.LastUpdate = GetTime
                        D.IsActive = False
                        D.ClientId = Nothing

                    End If
                Next
                'เพิ่ม Ast
                For Each Ass In ItemTeacher.tblAssistants
                    Ass.LastUpdate = GetTime
                    Ass.IsActive = True
                    Dim OriAst As tblAssistant
                    Using ctx1 = GetLinqToSql.GetDataContext()
                        OriAst = ctx1.tblAssistants.Where(Function(q) q.Ass_Id = Ass.Ass_Id).SingleOrDefault
                    End Using
                    If OriAst Is Nothing Then
                        Ctx.tblAssistants.InsertOnSubmit(Ass)
                    End If
                Next

                ItemTeacher.LastUpdate = GetTime
                ItemTeacher.Teacher_IsActive = True
                ItemTeacher.ClientId = Nothing
                Ctx.t360_tblTeachers.Attach(ItemTeacher, Ori)

                'user 
                'If ItemUser.Password IsNot Nothing Then
                '    Dim DataUser = Ctx.tblUsers.Where(Function(q) q.GUID = ItemTeacher.Teacher_id).SingleOrDefault
                '    DataUser.UserName = ItemUser.UserName
                '    DataUser.Password = ItemUser.Password
                '    DataUser.LastUpdate = GetTime
                '    DataUser.ClientId = Nothing
                'End If
                Dim DataUser = Ctx.tblUsers.Where(Function(q) q.GUID = ItemTeacher.Teacher_id).SingleOrDefault
                If DataUser.UserName <> ItemUser.UserName AndAlso ItemUser.Password IsNot Nothing Then ' เปลี่ยนทั้ง user name and pwd
                    DataUser.UserName = ItemUser.UserName
                    DataUser.Password = ItemUser.Password
                    DataUser.LastUpdate = GetTime
                    DataUser.ClientId = Nothing
                ElseIf DataUser.UserName <> ItemUser.UserName AndAlso ItemUser.Password Is Nothing Then 'เปลี่ยนเฉพาะ user
                    DataUser.UserName = ItemUser.UserName
                    DataUser.LastUpdate = GetTime
                    DataUser.ClientId = Nothing
                ElseIf DataUser.UserName = ItemUser.UserName AndAlso ItemUser.Password IsNot Nothing Then ' เปลี่ยนเฉพาะ pwd
                    DataUser.Password = ItemUser.Password
                    DataUser.LastUpdate = GetTime
                    DataUser.ClientId = Nothing
                End If

                'user subject class delete
                For Each Row In DeletetUserSubjectClasses
                    Dim DataSubjectClasses = Ctx.tblUserSubjectClasses.Where(Function(q) q.GUID = Row).SingleOrDefault
                    DataSubjectClasses.IsActive = False
                    DataSubjectClasses.LastUpdate = GetTime
                    DataSubjectClasses.ClientId = Nothing
                Next

                'user subject class insert
                Dim Max = Ctx.tblUserSubjectClasses.OrderByDescending(Function(a) a.USCId).Select(Function(q) q.USCId).FirstOrDefault
                For Each Row In InsertUserSubjectClasses
                    Max += 1
                    Row.USCId = Max
                    Row.UserIdOld = 1
                    Row.Detailid = 1
                    Row.IsActive = True
                    Row.LastUpdate = GetTime
                    Row.GUID = Guid.NewGuid
                    Row.UserId = ItemTeacher.Teacher_id
                    Row.ClientId = Nothing
                Next
                Ctx.tblUserSubjectClasses.InsertAllOnSubmit(InsertUserSubjectClasses)

                Ctx.SubmitChanges()
            End Using
            Return ItemTeacher
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ครู -ชื่อสกุล: ")
            LogDetail.Append(ItemTeacher.Teacher_FirstName & " " & ItemTeacher.Teacher_LastName)
            LogDetail.Append(" ,t360_tblteacher.Id=")
            LogDetail.Append(ItemTeacher.Teacher_id)
            UserManager.Log(EnLogType.ImportantUpdate, LogDetail.ToString)
        End Try
    End Function

    Public Function UpdateTeacherImport(ByVal ItemTeacher As t360_tblTeacher, ByVal ItemTeacherRoom As t360_tblTeacherRoom, ByVal IsChangeRoom As Boolean, Username As String, Password As String) As Boolean Implements ITeacherManager.UpdateTeacherImport
        'ถ้ามีการแก้ไขห้องเรียนใหม่จะเป็นการ Insert เข้าที่ t360_tblTeacherRoom
        Dim Factory = GetLinqToSql
        Try
            '<<< Teacher
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Dim Ctx = Factory.GetDataContextWithTransaction()
            ItemTeacher.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemTeacher.LastUpdate = GetTime
            ItemTeacher.ClientId = Nothing
            TeacherRepo.UpdateTeacher(Ctx, ItemTeacher)

            If IsChangeRoom Then
                '<<< TeacherRoom
                Dim Target = (From r In Ctx.t360_tblTeachers Where r.Teacher_id = ItemTeacher.Teacher_id).SingleOrDefault
                Dim Ori = Ctx.t360_tblTeacherRooms.Where(Function(q) q.Class_Name = ItemTeacher.Teacher_CurrentClass And q.Teacher_Id = ItemTeacher.Teacher_id And q.TR_IsActive = True).SingleOrDefault

                If Ori IsNot Nothing Then
                    Ori.TR_IsActive = False
                    Ori.ClientId = Nothing
                    Ori.LastUpdate = GetTime
                End If
                ItemTeacherRoom.LastUpdate = GetTime

                Target.t360_tblTeacherRooms.Add(ItemTeacherRoom)
                Ctx.SubmitChanges()
            End If

            Dim OriUser = Ctx.tblUsers.Where(Function(q) q.GUID = ItemTeacher.Teacher_id).SingleOrDefault
            OriUser.UserName = Username
            OriUser.Password = Password
            OriUser.LastUpdate = GetTime
            OriUser.ClientId = Nothing

            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ครู -ชื่อสกุล: ")
            LogDetail.Append(ItemTeacher.Teacher_FirstName & " " & ItemTeacher.Teacher_LastName)
            LogDetail.Append(" ,t360_tblteacher.Id=")
            LogDetail.Append(ItemTeacher.Teacher_id)
            UserManager.Log(EnLogType.ImportantUpdate, LogDetail.ToString)
        End Try
    End Function

    Public Function DeleteTeacher(ByVal Item As t360_tblTeacher) As Boolean Implements ITeacherManager.DeleteTeacher
        Dim TeacherName As String = Item.Teacher_FirstName & " " & Item.Teacher_LastName
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate
                Dim Target = (From r In Ctx.t360_tblTeachers Where r.Teacher_id = Item.Teacher_id).SingleOrDefault
                Target.Teacher_IsActive = False
                Target.LastUpdate = GetTime
                Target.ClientId = Nothing


                Dim Assistants = Ctx.tblAssistants.Where(Function(q) q.IsActive = True And q.Assistant_id = Item.Teacher_id).ToArray
                For Each Row In Assistants
                    Row.IsActive = False
                    Row.LastUpdate = GetTime
                    Row.ClientId = Nothing
                Next

                Dim Teachers = Ctx.tblAssistants.Where(Function(q) q.IsActive = True And q.Teacher_id = Item.Teacher_id).ToArray
                For Each Row In Teachers
                    Row.IsActive = False
                    Row.LastUpdate = GetTime
                    Row.ClientId = Nothing
                Next

                Dim DataUser = Ctx.tblUsers.Where(Function(q) q.GUID = Item.Teacher_id).SingleOrDefault
                If DataUser IsNot Nothing Then
                    DataUser.IsActive = False
                    DataUser.LastUpdate = GetTime
                    DataUser.ClientId = Nothing
                End If


                Ctx.SubmitChanges()

                'Item.School_Code = UserConfig.GetCurrentContext.School_Code
                'TeacherRepo.DeleteTeacher(Ctx, Item)
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'Dim ItemLog As New t360_tblLog
            'With ItemLog
            '    .Log_Type = EnLogType.Delete
            '    .Log_Page = "รายชื่อครู (จัดการชื่อครู)"
            '    .Log_Description = "รายชื่อครู (จัดการชื่อครู)"
            'End With
            'UserManager.InsertLog(ItemLog)
            Dim LogDeatil As New StringBuilder
            LogDeatil.Append("ครู -ชื่อสกุล: ")
            LogDeatil.Append(Item.Teacher_FirstName & " " & Item.Teacher_LastName)
            LogDeatil.Append(" ,t360_tblTeacher.Id=")
            LogDeatil.Append(Item.Teacher_id.ToString)
            UserManager.Log(EnLogType.ImportantDelete, LogDeatil.ToString)

        End Try
    End Function

    Public Function ValidateDuplicateTeacher(SchoolCode As String, ByVal Teacher_Code As String, Optional ByVal Teacher_Id As Guid? = Nothing) As Boolean Implements ITeacherManager.ValidateDuplicateTeacher
        Using Ctx = GetLinqToSql.GetDataContext()
            If Teacher_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblTeachers Where r.Teacher_Code = Teacher_Code And r.School_Code = SchoolCode And r.Teacher_IsActive = True).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim q = (From r In Ctx.t360_tblTeachers Where r.Teacher_Code = Teacher_Code And r.School_Code = SchoolCode And r.Teacher_IsActive = True And r.Teacher_id <> Teacher_Id).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Using
    End Function

    Public Function ProcessUpdateTeacherRoom(SchoolCode As String) As Boolean Implements ITeacherManager.ProcessUpdateTeacherRoom
        Dim Factory = GetLinqToSql
        Dim Ctx As DataClassesTablet360DataContext
        Try
            'เอาที่ isactive = 1 ใน t360_tblTeacherRoom ที่ไม่เท่ากับ เทอมปัจจุบัน ไปใส่ tmp
            'ปรับ calendarid = เทอมปัจจุบัน และ lastupdate = now และ clientid = nothing และ id=new guid  ที่ tmp
            'ปรับ isactive = 0 และ lastupdate = now และ clientid = nothing ใน t360_tblTeacherRoom ที่ iaactive = 1  และ calendarid <> เทอมปัจจับัน
            'เอา tmp insert กลับเข้าไปที่ t360_tblTeacherRoom
            'ลบ temp ทิ้ง

            Ctx = Factory.GetDataContextWithTransaction
            'เทอมที่กำลังทำงาน
            Factory.MainSql = "SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate" &
                       " AND Calendar_Type = 3 AND School_Code = '" & SchoolCode & "' and IsActive = '1' "
            Factory.LockWhere = True
            Dim Calendar = Factory.DataContextExecuteObjects(Of t360_tblCalendar)().SingleOrDefault
            'ถ้าเจอเทอมถึงจะทำ
            If Calendar IsNot Nothing Then

                'เอาที่ isactive = 1 ใน t360_tblTeacherRoom ที่ไม่เท่ากับ เทอมปัจจุบัน ไปใส่ tmp
                Factory.MainSql = "select * into t360_tblTeacherRoom_" & SchoolCode & " from t360_tblTeacherRoom where TR_IsActive=1 AND Calendar_Id<>'" & Calendar.Calendar_Id.ToString & "'"
                Factory.DataContextExecuteCommand(Ctx)

                'ปรับ calendarid = เทอมปัจจุบัน และ lastupdate = now และ clientid = nothing และ id=new guid ที่ tmp
                Factory.MainSql = "UPDATE t360_tblTeacherRoom_" & SchoolCode & " SET Calendar_Id='" & Calendar.Calendar_Id.ToString & "', LastUpdate=dbo.GetThaiDate(), ClientId=null,tr_id=newid()"
                Factory.DataContextExecuteCommand(Ctx)

                'ปรับ isactive = 0 และ lastupdate = now และ clientid = nothing ใน t360_tblTeacherRoom ที่ iaactive = 1  และ calendarid <> เทอมปัจจับัน
                Factory.MainSql = "UPDATE t360_tblTeacherRoom SET TR_IsActive=0, LastUpdate=dbo.GetThaiDate(), ClientId=null WHERE TR_IsActive=1 AND Calendar_Id<>'" & Calendar.Calendar_Id.ToString & "'"
                Factory.DataContextExecuteCommand(Ctx)

                'เอา tmp insert กลับเข้าไปที่ t360_tblTeacherRoom
                Factory.MainSql = "insert  into t360_tblTeacherRoom select * from t360_tblTeacherRoom_" & SchoolCode
                Factory.DataContextExecuteCommand(Ctx)

                'ลบ temp ทิ้ง
                Factory.MainSql = "drop table t360_tblTeacherRoom_" & SchoolCode
                Factory.DataContextExecuteCommand(Ctx)
            End If


            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.ImportantUpdate
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = "ห้องประจำชั้นของครู"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateTeacherPassword(TeacherId As Guid, TeacherNewPassword As String) As Object Implements ITeacherManager.UpdateTeacherPassword

        'Dim LogDetail As New StringBuilder
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = Ctx.tblUsers.Where(Function(q) q.GUID = TeacherId).SingleOrDefault
                Data.Password = BusinessTablet360.Encryption.MD5(TeacherNewPassword.ToString)
                Data.LastUpdate = GetTime
                Data.ClientId = Nothing
                'Dim StudentDetail = GetUpLevelDetailBySudentId(StudentId)
                'LogDetail.Append("นักเรียน -ชื่อสกุล: ")
                'LogDetail.Append(StudentDetail.Student_FirstName & " " & StudentDetail.Student_LastName)
                'LogDetail.Append(" -ห้อง: ")
                'LogDetail.Append(CurrentClassRoom)
                'LogDetail.Append(" ,t360_tblStudent.Id=")
                'LogDetail.Append(StudentDetail.Student_Id.ToString)

                Ctx.SubmitChanges()
            End Using

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'UserManager.Log(EnLogType.Delete, LogDetail.ToString)
        End Try

        'Dim Factory = GetLinqToSql
        'Dim Ctx = Factory.GetDataContextWithTransaction()
        'Try
        '    Dim NewPassword = BusinessTablet360.Encryption.MD5(TeacherNewPassword.ToString)

        '    Dim OriUser = Ctx.tblUsers.Where(Function(q) q.GUID = TeacherId).SingleOrDefault
        '    'OriUser.UserName = Username
        '    OriUser.Password = NewPassword
        '    'OriUser.LastUpdate = GetTime
        '    'OriUser.ClientId = Nothing

        '    Factory.DataContextCommitTransaction()
        '    Return True
        'Catch ex As Exception

        'Finally
        '    Dim ItemLog As New t360_tblLog
        '    With ItemLog
        '        .Log_Type = EnLogType.ImportantUpdate
        '        .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
        '        .Log_Description = "แก้รหัส Pointplus"
        '    End With
        '    UserManager.InsertLog(ItemLog)
        'End Try

    End Function
End Class
