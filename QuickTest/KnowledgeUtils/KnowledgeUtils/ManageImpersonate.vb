
Imports System.Security.Principal
Imports System.Runtime.InteropServices
Public Class ManageImpersonate
    Private LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private LOGON32_PROVIDER_DEFAULT As Integer = 0

    Private impersonationContext As WindowsImpersonationContext

    Private Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer

    Private Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal ExistingTokenHandle As IntPtr, _
                            ByVal ImpersonationLevel As Integer, _
                            ByRef DuplicateTokenHandle As IntPtr) As Integer

    Private Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Private Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long
 
    Public Sub New(ByVal userName As String, ByVal domain As String, ByVal password As String)
        If impersonateValidUser(userName, domain, password) <> True Then
            Throw New ArgumentException("username/domain/password combination ไม่ถูกต้อง, ไม่สามารถใช้สิทธิ์ตามที่ขอเรียกใช้ได้")
        End If
    End Sub
    Private Function impersonateValidUser(ByVal userName As String, ByVal domain As String, ByVal password As String) As Boolean

        Dim tempWindowsIdentity As WindowsIdentity
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero
        impersonateValidUser = False

        If RevertToSelf() Then
            If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                         LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                    tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                    impersonationContext = tempWindowsIdentity.Impersonate()
                    If Not impersonationContext Is Nothing Then
                        impersonateValidUser = True
                    End If
                End If
            End If
        End If
        If Not tokenDuplicate.Equals(IntPtr.Zero) Then
            CloseHandle(tokenDuplicate)
        End If
        If Not token.Equals(IntPtr.Zero) Then
            CloseHandle(token)
        End If

    End Function

    Public Sub undoImpersonation()
        impersonationContext.Undo()
    End Sub
End Class
