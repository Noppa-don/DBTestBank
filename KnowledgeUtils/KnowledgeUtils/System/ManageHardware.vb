Imports System.Management

Namespace System

    Public Class ManageHardware

        Public Shared Function GetCpuId() As String
            Dim Query As New SelectQuery("Win32_processor")
            Dim Search As New ManagementObjectSearcher(Query)
            Dim id As String = ""
            For Each Info As ManagementObject In Search.Get()
                id = Info("processorid").ToString
            Next
            Return id
        End Function

    End Class

End Namespace


