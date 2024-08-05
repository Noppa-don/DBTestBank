Imports Elmah
Imports System.Web
Imports System

Public Class ElmahExtension

    Public Shared Sub LogToElmah(ByVal ex As Exception)
        If HttpApplication IsNot Nothing AndAlso HttpContext.Current IsNot Nothing Then
            Try
                ErrorSignal.FromCurrentContext().Raise(ex)
            Catch Elmahex As Exception
                InitNoContext()
                ErrorSignal.Get(HttpApplication).Raise(ex)
            End Try
        Else
            If HttpApplication Is Nothing Then
                InitNoContext()
            End If
            ErrorSignal.Get(HttpApplication).Raise(ex)
        End If
    End Sub

    Private Shared HttpApplication As HttpApplication = Nothing
    Private Shared errorFilter As ErrorFilterConsole = New ErrorFilterConsole

    Public Shared ErrorMail As ErrorMailModule = New ErrorMailModule()
    Public Shared ErrorLog As ErrorLogModule = New ErrorLogModule()
    Public Shared ErrorTweet As ErrorTweetModule = New ErrorTweetModule()

    Private Shared Sub InitNoContext()
        HttpApplication = New HttpApplication()
        errorFilter.Init(HttpApplication)

        CType(ErrorMail, IHttpModule).Init(HttpApplication)
        errorFilter.HookFiltering(ErrorMail)
        CType(ErrorLog, IHttpModule).Init(HttpApplication)
        errorFilter.HookFiltering(ErrorLog)
        CType(ErrorTweet, IHttpModule).Init(HttpApplication)
        errorFilter.HookFiltering(ErrorTweet)
    End Sub

    Private Class ErrorFilterConsole
        Inherits ErrorFilterModule
        Public Sub HookFiltering(ByVal md As IExceptionFiltering)
            AddHandler md.Filtering, AddressOf MyBase.OnErrorModuleFiltering
        End Sub
    End Class

End Class

