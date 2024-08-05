Imports Microsoft.AspNet.SignalR
Imports Microsoft.AspNet.SignalR.Hubs

Namespace MySignalRApplication

    Public Class [Global]
        Inherits System.Web.HttpApplication
        Protected Sub application_start(ByVal sender As Object, ByVal e As EventArgs)
            GlobalHost.HubPipeline.AddModule(New LoggingPipelineModule())

        End Sub
    End Class
    Public Class LoggingPipelineModule
        Inherits HubPipelineModule
        Protected Overrides Function OnBeforeIncoming(ByVal context As IHubIncomingInvokerContext) As Boolean
            Debug.WriteLine("=> Invoking " & Convert.ToString(context.MethodDescriptor.Name) & " on hub " & Convert.ToString(context.MethodDescriptor.Hub.Name))
            Return MyBase.OnBeforeIncoming(context)
        End Function

        Protected Overrides Function OnBeforeOutgoing(ByVal context As IHubOutgoingInvokerContext) As Boolean
            Debug.WriteLine("<= Invoking " & Convert.ToString(context.Invocation.Method) & " on client hub " & Convert.ToString(context.Invocation.Method))
            Return MyBase.OnBeforeOutgoing(context)
        End Function


    End Class

End Namespace
