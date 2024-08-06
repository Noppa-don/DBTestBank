Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils

Public Interface ISubjectManager

    '<<< SubjectType
    Function GetSubjectTypeByName(ByVal ST_Name As String) As t360_tblSubjectType
    Function GetSubjectTypeAll() As t360_tblSubjectType()
    Function GetSubjectTypeInSchool() As t360_tblSubjectType()
    Function GetSubjectForHomeworkByCalendarId(ByVal Calendar_Id As Guid?) As uvw_GetSubjectForHomework()
    Function GetGroupSubjectAll() As tblGroupSubject()
    Function GetGroupSubjectByID(ByVal GroupSubject_Id As Guid) As tblGroupSubject
    Function UpdateSubjectTypeActive(ByVal Item As t360_tblSubjectType, ByVal ListSchoolSubjectType As t360_tblSchoolSubjectType()) As Boolean

    '<<< Subject
    Function GetSubjectByCrit(Of T)(ByVal Item As SubjectDTO) As T()
    Function InsertSubject(ByVal Item As t360_tblSubject) As Boolean
    Function UpdateSubject(ByVal Item As t360_tblSubject) As Boolean
    Function DeleteSubject(ByVal Item As t360_tblSubject) As Boolean
    Function ValidateDuplicateSubject(ByVal Subject_Code As String, Optional ByVal Subject_Id As Guid? = Nothing) As Boolean

    '<<< SubjectClass
    Function GetCountSubjectClassBySubject(ByVal Subject_Id As Guid) As Integer
    Function GetSubjectClassBySubjectId(ByVal Subject_Id As Guid) As t360_tblSubjectClass()
    Function UpdateSubjectClass(ByVal Item As SubjectDTO, ByVal ListSubjectClass() As t360_tblSubjectClass) As Boolean

End Interface

Public Class SubjectManager
    Implements ISubjectManager

#Region "Dependency"

    Private _SubjectTypeRepo As ISubjectTypeRepo
    <Dependency()> Public Property SubjectTypeRepo() As ISubjectTypeRepo
        Get
            Return _SubjectTypeRepo
        End Get
        Set(ByVal value As ISubjectTypeRepo)
            _SubjectTypeRepo = value
        End Set
    End Property

    Private _SubjectRepo As ISubjectRepo
    <Dependency()> Public Property SubjectRepo() As ISubjectRepo
        Get
            Return _SubjectRepo
        End Get
        Set(ByVal value As ISubjectRepo)
            _SubjectRepo = value
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

#Region "SubjectType"

    Public Function GetSubjectTypeByName(ByVal ST_Name As String) As t360_tblSubjectType Implements ISubjectManager.GetSubjectTypeByName
        Using ctx = GetLinqToSql.GetDataContext()
            Return SubjectTypeRepo.GetSubjectTypeByName(ctx, ST_Name)
        End Using
    End Function

    Public Function GetSubjectTypeInSchool() As t360_tblSubjectType() Implements ISubjectManager.GetSubjectTypeInSchool
        Using ctx = GetLinqToSql.GetDataContext()
            Return SubjectTypeRepo.GetSubjectTypeInSchool(ctx, UserConfig.GetCurrentContext.School_Code)
        End Using
    End Function

    Public Function GetSubjectTypeAll() As t360_tblSubjectType() Implements ISubjectManager.GetSubjectTypeAll
        Using ctx = GetLinqToSql.GetDataContext()
            Return SubjectTypeRepo.GetSubjectTypeAll(ctx)
        End Using
    End Function

    Public Function GetSubjectForHomeworkByCalendarId(ByVal Calendar_Id As System.Guid?) As uvw_GetSubjectForHomework() Implements ISubjectManager.GetSubjectForHomeworkByCalendarId
        Using ctx = GetLinqToSql.GetDataContext()
            Return ctx.uvw_GetSubjectForHomeworks.ToArray
        End Using
    End Function

    Public Function UpdateSubjectTypeActive(ByVal Item As t360_tblSubjectType, ByVal ListSchoolSubjectType As t360_tblSchoolSubjectType()) As Boolean Implements ISubjectManager.UpdateSubjectTypeActive
        Dim Factoray = GetLinqToSql
        Try
            Dim Ctx = Factoray.GetDataContextWithTransaction()
            Array.ForEach(ListSchoolSubjectType, Sub(q) q.School_Code = UserConfig.GetCurrentContext.School_Code)
            SubjectTypeRepo.UpdateSubjectTypeActive(Ctx, UserConfig.GetCurrentContext.School_Code, ListSchoolSubjectType)
            Factoray.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factoray.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function GetGroupSubjectAll() As tblGroupSubject() Implements ISubjectManager.GetGroupSubjectAll
        Using ctx = GetLinqToSql.GetDataContext()
            Return ctx.tblGroupSubjects.ToArray
        End Using
    End Function

    Public Function GetGroupSubjectByID(ByVal GroupSubject_Id As System.Guid) As tblGroupSubject Implements ISubjectManager.GetGroupSubjectByID
        Using ctx = GetLinqToSql.GetDataContext()
            Return ctx.tblGroupSubjects.Where(Function(q) q.GroupSubject_Id = GroupSubject_Id And q.IsActive = True).SingleOrDefault
        End Using
    End Function

#End Region

#Region "Subject"

    Public Function DeleteSubject(ByVal Item As t360_tblSubject) As Boolean Implements ISubjectManager.DeleteSubject
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                SubjectRepo.DeleteSubject(Ctx, Item)
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Delete
                .Log_Page = "หมวดวิชา-วิชา (จัดการข้อมูลหมวดวิชา - วิชา)"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function GetSubjectByCrit(Of T)(ByVal Item As SubjectDTO) As T() Implements ISubjectManager.GetSubjectByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Item.Subject_IsActive = True
            Return SubjectRepo.GetSubjectByCrit(Of T)(Ctx, Item)
        End Using
    End Function

    Public Function InsertSubject(ByVal Item As t360_tblSubject) As Boolean Implements ISubjectManager.InsertSubject
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Item.Subject_IsActive = True
                SubjectRepo.InsertSubject(Ctx, Item)
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = "หมวดวิชา-วิชา (จัดการข้อมูลหมวดวิชา - วิชา)"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateSubject(ByVal Item As t360_tblSubject) As Boolean Implements ISubjectManager.UpdateSubject
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Item.Subject_IsActive = True
                SubjectRepo.UpdateSubject(Ctx, Item)
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Page = "หมวดวิชา-วิชา (จัดการข้อมูลหมวดวิชา - วิชา)"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateSubject(ByVal Subject_Code As String, Optional ByVal Subject_Id As Guid? = Nothing) As Boolean Implements ISubjectManager.ValidateDuplicateSubject
        Using Ctx = GetLinqToSql.GetDataContext()
            Return SubjectRepo.ValidateDuplicateSubject(Ctx, UserConfig.GetCurrentContext.School_Code, Subject_Code, Subject_Id)
        End Using
    End Function

#End Region

#Region "SubjectClass"

    Public Function GetCountSubjectClassBySubject(ByVal Subject_Id As Guid) As Integer Implements ISubjectManager.GetCountSubjectClassBySubject
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblSubjectClasses.Where(Function(q) q.Subject_Id = Subject_Id AndAlso q.School_Code = UserConfig.GetCurrentContext.School_Code).Count
        End Using
    End Function

    Public Function GetSubjectClassBySubjectId(ByVal Subject_Id As Guid) As t360_tblSubjectClass() Implements ISubjectManager.GetSubjectClassBySubjectId
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblSubjects.Single(Function(q) q.Subject_Id = Subject_Id AndAlso q.School_Code = UserConfig.GetCurrentContext.School_Code).t360_tblSubjectClasses.ToArray
        End Using
    End Function

    Public Function UpdateSubjectClass(ByVal Item As SubjectDTO, ByVal ListSubjectClass() As t360_tblSubjectClass) As Boolean Implements ISubjectManager.UpdateSubjectClass
        Dim Factory = GetLinqToSql
        Try
            Dim Ctx = Factory.GetDataContextWithTransaction()
            'ลบของเก่า
            Dim Subject = Ctx.t360_tblSubjects.Single(Function(q) q.Subject_Id = Item.Subject_Id)
            Dim ListOldSubjectClass = Subject.t360_tblSubjectClasses.ToArray
            Ctx.t360_tblSubjectClasses.DeleteAllOnSubmit(ListOldSubjectClass)
            'เพิ่มของใหม่
            Array.ForEach(ListSubjectClass, Sub(q) q.School_Code = UserConfig.GetCurrentContext.School_Code)
            Ctx.t360_tblSubjectClasses.InsertAllOnSubmit(ListSubjectClass)
            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.UpdateSubjectClass
                .Log_Page = "หมวดวิชา-วิชา (จัดการข้อมูลหมวดวิชา - วิชา)"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

#End Region


End Class
