Imports System.Web.SessionState
Imports Microsoft.Practices.Unity
Imports BusinessTablet360
Imports KnowledgeUtils
Imports System.Web.Optimization
Imports System.Web.Routing

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public Property Container() As IUnityContainer
        Get
            Return Application("UnityContainer")
        End Get
        Set(ByVal value As IUnityContainer)
            Application("UnityContainer") = value
        End Set
    End Property

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        'Bundle
        BundleConfig.RegisterBundles(BundleTable.Bundles)
        AuthConfig.RegisterOpenAuth()
        RouteConfig.RegisterRoutes(RouteTable.Routes)

        ' Fires when the application is started
        KnowledgeUtils.Database.DBFactory.RegisterApplicationManager(New BusinessTablet360.WebApplicationQuickTestManager)
        KnowledgeUtils.Database.DBFactory.RegisterDbCommon(New KnowledgeUtils.Database.DbCommonSqlServer)

        Application.Lock()
        Try
            Container = New UnityContainer
            Container.RegisterType(Of KnowledgeUtils.Database.IApplicationManager, BusinessTablet360.WebApplicationQuickTestManager)()
            Container.RegisterType(Of IUserConfigManager, UserConfigManager)()

            Container.RegisterType(Of ISchoolManager, SchoolManager)()
            Container.RegisterType(Of IStudentManager, StudentManager)()
            Container.RegisterType(Of ILocalManager, LocalManager)()
            Container.RegisterType(Of ITeacherManager, TeacherManager)()
            Container.RegisterType(Of ISubjectManager, SubjectManager)()
            Container.RegisterType(Of ITabletManager, TabletManager)()
            Container.RegisterType(Of INetworkManager, NetworkManager)()
            Container.RegisterType(Of IUserManager, UserManager)()
            Container.RegisterType(Of IImportManager, ImportManager)()
            Container.RegisterType(Of IModuleManager, ModuleManager)()

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

        Catch ex As Exception

            Application.UnLock()
        End Try

        If HttpContext.Current.Application("EnableReportQuestion") Is Nothing Then
            HttpContext.Current.Application("EnableReportQuestion") = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings("EnableReportQuestion"))
        End If
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        'Dim UserConMng As New UserConfigManager(Context)
        'Container.RegisterInstance(GetType(IUserConfigManager), UserConMng)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs

        Dim httpException As HttpException = TryCast(Server.GetLastError(), HttpException)
        If httpException IsNot Nothing Then
            Dim HttpCode As Integer = httpException.GetHttpCode()
            If HttpCode = 404 Then
                Response.StatusCode = 404
                Response.TrySkipIisCustomErrors = True
                Server.ClearError()
                Response.Redirect("~/error/404.htm")
                Return
            End If
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class