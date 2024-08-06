Imports Microsoft.Practices.Unity
Imports KnowledgeUtils

Public Class FactoryUnity

    Shared Sub New()
        Try
            KnowledgeUtils.Database.DBFactory.RegisterApplicationManager(New BusinessTablet360.WindowsApplicationCheckNetworkManager)
            KnowledgeUtils.Database.DBFactory.RegisterDbCommon(New KnowledgeUtils.Database.DbCommonSqlServer)

            Container = New UnityContainer
            Container.RegisterType(Of KnowledgeUtils.Database.IApplicationManager, BusinessTablet360.WindowsApplicationManager)()
            Container.RegisterType(Of IUserConfigManager, UserConfigApplicationManager)()

            Container.RegisterType(Of ISchoolManager, SchoolManager)()
            Container.RegisterType(Of IStudentManager, StudentManager)()
            Container.RegisterType(Of ILocalManager, LocalManager)()
            Container.RegisterType(Of ITeacherManager, TeacherManager)()
            Container.RegisterType(Of ISubjectManager, SubjectManager)()
            Container.RegisterType(Of ITabletManager, TabletManager)()
            Container.RegisterType(Of INetworkManager, NetworkManager)()
            Container.RegisterType(Of IUserManager, UserManager)()
            Container.RegisterType(Of IImportManager, ImportManager)()

            Container.RegisterType(Of IClassRepo, ClassRepo)()
            Container.RegisterType(Of IRoomRepo, RoomRepo)()
            Container.RegisterType(Of IStudentRepo, StudentRepo)()
            Container.RegisterType(Of ISchoolRepo, SchoolRepo)()
            Container.RegisterType(Of IProviceRepo, ProviceRepo)()
            Container.RegisterType(Of IDistrictRepo, DistrictRepo)()
            Container.RegisterType(Of ISubdistrictRepo, SubdistrictRepo)()
            Container.RegisterType(Of IStudentRoomRepo, StudentRoomRepo)()
            Container.RegisterType(Of ICalendarRepo, CalendarRepo)()
            Container.RegisterType(Of INewsRepo, NewsRepo)()
            Container.RegisterType(Of INewsRoomRepo, NewsRoomRepo)()
            Container.RegisterType(Of ITeacherRepo, TeacherRepo)()
            Container.RegisterType(Of ISubjectTypeRepo, SubjectTypeRepo)()
            Container.RegisterType(Of ISubjectRepo, SubjectRepo)()

            '<<< Initialization UerConfig
            'Dim ItemUserConfig As New UserConfig
            'With ItemUserConfig
            '    .School_Code = GetSchoolManager.GetSchoolCode
            'End With
            'Dim UserConfig As New UserConfigApplicationManager()
            'Container.RegisterInstance(GetType(IUserConfigManager), UserConfig)
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
        End Try
    End Sub

    Private Shared _Container As IUnityContainer
    Public Shared Property Container() As IUnityContainer
        Get
            Return _Container
        End Get
        Set(ByVal value As IUnityContainer)
            _Container = value
        End Set
    End Property

    Public Shared Function GetNetworkManager() As NetworkManager
        Try
            Return Container.Resolve(Of INetworkManager)()
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function GetSchoolManager() As SchoolManager
        Try
            Return Container.Resolve(Of ISchoolManager)()
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

End Class
