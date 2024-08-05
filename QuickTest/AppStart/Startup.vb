Imports Owin
Imports System
Imports Microsoft.Owin



<Assembly: OwinStartup(GetType(SignalRChat2.Startup))>
Namespace SignalRChat2

    Public Class Startup
        Public Sub Configuration(app As IAppBuilder)
            app.MapSignalR()

        End Sub
    End Class
End Namespace





