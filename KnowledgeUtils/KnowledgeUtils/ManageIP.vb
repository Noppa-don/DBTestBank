Imports System.Text
Imports System.Net
Imports System.Management

Public Class ManageIP
    Public Shared Sub SetNewIP(ByVal UserNameWithAdminRight As String, ByVal UserPassword As String, ByVal UserDomain As String, NewIP As IPAddress, NewGateWay As IPAddress, NewSubnetMask As IPAddress, NewDNS1 As IPAddress, ByVal LANInterfaceName As String, Optional NewDNS2 As IPAddress = Nothing)
        Try
            Dim objImpersonate As New ManageImpersonate(UserNameWithAdminRight, UserDomain, UserPassword)
            'Insert your code that runs under the security context of a specific user here.
            Set_IP(LANInterfaceName, NewIP.ToString(), NewSubnetMask.ToString(), NewGateWay.ToString())
            Dim CheckDNS2 As String = ""
            If NewDNS2 IsNot Nothing Then
                CheckDNS2 = NewDNS2.ToString()
            End If
            Set_DNS(LANInterfaceName, NewDNS1.ToString(), CheckDNS2)

            objImpersonate.undoImpersonation()

        Catch arex As ArgumentException
            'Your impersonation failed. Therefore, include a fail-safe mechanism here.
            ElmahExtension.LogToElmah(arex)
            Throw New ArgumentException("username/domain/password combination ไม่ถูกต้อง, ไม่สามารถใช้สิทธิ์ของ username ตามที่ขอเรียกใช้")
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            'Your impersonation failed. Therefore, include a fail-safe mechanism here.
            Throw New ApplicationException("ไม่สามารถ set ip ใหม่ได้, username/password/domain ถูกต้อง, แต่อาจไม่มีสิทธิ์เพียงพอ หรือ unknownerror อื่นๆ" + vbNewLine + ex.ToString())
        End Try


    End Sub


    Private Shared Function Set_DNS(ByVal LANInterfaceName As String, ByVal NewDNS1 As String, Optional ByVal NewDNS2 As String = "")

        'ตัวอย่างโค้ด
        'netsh interface ip delete dns name="Wireless Network connection" all
        'netsh interface ip add dns name="Wireless Network connection" addr=8.8.8.8 index=1
        'netsh interface ip add dns name="Wireless Network connection" addr=4.4.4.4 index=2


        Dim p As New Process()
        p.StartInfo.FileName = "netsh.exe"

        'Clear All DNS
        Dim parameters As New StringBuilder()
        parameters.Append(" interface ip delete dns name=""").Append(LANInterfaceName)
        parameters.Append(""" all")
        p.StartInfo.Arguments = parameters.ToString()
        p.StartInfo.UseShellExecute = False
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.RedirectStandardOutput = True
        Dim infoString As String
        Try
            p.Start()
            p.WaitForExit(30000)
            infoString = p.StandardOutput.ReadToEnd()

            'DNS1
            parameters.Clear()
            parameters.Append(" interface ip add dns name=""").Append(LANInterfaceName)
            parameters.Append(""" addr=").Append(NewDNS1)
            parameters.Append(" index=1")
            p.StartInfo.Arguments = parameters.ToString()

            p.Start()
            p.WaitForExit(30000)
            infoString = p.StandardOutput.ReadToEnd()

            'DNS2
            If NewDNS2 <> "" Then
                parameters.Clear()
                parameters.Append(" interface ip add dns name=""").Append(LANInterfaceName)
                parameters.Append(""" addr=").Append(NewDNS2)
                parameters.Append(" index=1")
                p.StartInfo.Arguments = parameters.ToString()

                p.Start()
                p.WaitForExit(30000)
                infoString = p.StandardOutput.ReadToEnd()
            End If

            Return 1
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
        End Try

    End Function
    Private Shared Function Set_IP(ByVal LANInterfaceName As String, ByVal IPAddress As String, ByVal SubnetMask As String, ByVal Gateway As String)

        'Process.Start("netsh interface ipv4 set address ""Wireless Network Connection"" static 192.168.50.131 255.255.255.0 192.168.50.1")

        Dim p As New Process()
        p.StartInfo.FileName = "netsh.exe"

        ' Now build the parameters string, which consists of these parameters and identifiers with spaces between each:
        ' 1. "interface ip" - context for command
        ' 2. "add address" - add an address
        ' 3. "name=" - to which adapter to add the address
        ' 4. "addr=" - which address to add to the adapter
        ' 5. "mask=" - subnet mask of address to add
        ' 6. "gateway=" - add the address also as a gateway for the adapter
        ' 7. "gwmetric=" - set a metric of one so this gateway is used
        Dim parameters As New StringBuilder()
        parameters.Append(" interface ip set address name=""").Append(LANInterfaceName)
        parameters.Append(""" addr=").Append(IPAddress)
        parameters.Append(" mask=").Append(SubnetMask)
        parameters.Append(" gateway=").Append(Gateway)
        parameters.Append(" gwmetric=1")
        p.StartInfo.Arguments = parameters.ToString()
        p.StartInfo.UseShellExecute = False
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.RedirectStandardOutput = True
        Dim infoString As String
        Try
            p.Start()
            p.WaitForExit(30000)
            infoString = p.StandardOutput.ReadToEnd()
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
        End Try


        'Process.Start("netsh interface ipv4 set address ""Wireless Network Connection"" static 192.168.50.131 255.255.255.0 192.168.50.1")
        Return 1



        ''Dim managementClass As New ManagementClass("Win32_NetworkAdapterConfiguration")
        ''Dim mgObjCollection As ManagementObjectCollection = managementClass.GetInstances()
        ''Dim Flag As Boolean

        ''For Each mgObject As ManagementObject In mgObjCollection
        ''    If Not CType(mgObject("IPEnabled"), Boolean) Then Continue For

        ''    Try
        ''        Dim objNewIP As ManagementBaseObject = Nothing
        ''        Dim objSetIP As ManagementBaseObject = Nothing
        ''        Dim objNewGate As ManagementBaseObject = Nothing

        ''        objNewIP = mgObject.GetMethodParameters("EnableStatic")
        ''        objNewGate = mgObject.GetMethodParameters("SetGateways")

        ''        ' Set the default gateway (decided to declare and initialise
        ''        ' variables rather than attempting to initialize the array
        ''        ' while communicating with the WMI.
        ''        Dim tmpStrArray() As String = {Gateway}

        ''        objNewGate("DefaultIPGateway") = tmpStrArray
        ''        Dim tmpIntArray() As Integer = {1}
        ''        objNewGate("GatewayCostMetric") = tmpIntArray

        ''        ' Set the IP address and subnet.
        ''        tmpStrArray(0) = IPAddress
        ''        objNewIP("IPAddress") = tmpStrArray
        ''        tmpStrArray(0) = SubnetMask
        ''        objNewIP("SubnetMask") = tmpStrArray

        ''        objSetIP = mgObject.InvokeMethod("EnableStatic", objNewIP, Nothing)
        ''        objSetIP = mgObject.InvokeMethod("SetGateways", objNewGate, Nothing)
        ''        Flag = True
        ''    Catch ex As Exception
        ''        'MessageBox.Show("An error occured: " + ex.Message)
        ''        Flag = False
        ''    End Try
        ''Next
        ''If Flag = True Then
        ''    Return 1
        ''Else
        ''    Return 0
        ''End If
    End Function
End Class



'' <summary>
' ''' Helper class to set networking configuration like IP address, DNS servers, etc.
' ''' </summary>
'Public Class NetworkConfigurator
'    ''' <summary>
'    ''' Set's a new IP Address and it's Submask of the local machine
'    ''' </summary>
'    ''' <param name="ipAddress">The IP Address</param>
'    ''' <param name="subnetMask">The Submask IP Address</param>
'    ''' <param name="gateway">The gateway.</param>
'    ''' <remarks>Requires a reference to the System.Management namespace</remarks>
'    Public Sub SetIP(ipAddress As String, subnetMask As String, gateway As String)
'        Using networkConfigMng = New ManagementClass("Win32_NetworkAdapterConfiguration")
'            Using networkConfigs = networkConfigMng.GetInstances()
'                For Each managementObject As var In networkConfigs.Cast(Of ManagementObject)().Where(Function(managementObject) CBool(managementObject("IPEnabled")))
'                    Using newIP = managementObject.GetMethodParameters("EnableStatic")
'                        ' Set new IP address and subnet if needed
'                        If (Not [String].IsNullOrEmpty(ipAddress)) OrElse (Not [String].IsNullOrEmpty(subnetMask)) Then
'                            If Not [String].IsNullOrEmpty(ipAddress) Then
'								newIP("IPAddress") = New () {ipAddress}
'                            End If

'                            If Not [String].IsNullOrEmpty(subnetMask) Then
'								newIP("SubnetMask") = New () {subnetMask}
'                            End If

'                            managementObject.InvokeMethod("EnableStatic", newIP, Nothing)
'                        End If

'                        ' Set mew gateway if needed
'                        If Not [String].IsNullOrEmpty(gateway) Then
'                            Using newGateway = managementObject.GetMethodParameters("SetGateways")
'								newGateway("DefaultIPGateway") = New () {newGateway}
'								newGateway("GatewayCostMetric") = New () {1}
'                                managementObject.InvokeMethod("SetGateways", newGateway, Nothing)
'                            End Using
'                        End If
'                    End Using
'                Next
'            End Using
'        End Using
'    End Sub

'    ''' <summary>
'    ''' Set's the DNS Server of the local machine
'    ''' </summary>
'    ''' <param name="nic">NIC address</param>
'    ''' <param name="dnsServers">Comma seperated list of DNS server addresses</param>
'    ''' <remarks>Requires a reference to the System.Management namespace</remarks>
'    Public Sub SetNameservers(nic As String, dnsServers As String)
'        Using networkConfigMng = New ManagementClass("Win32_NetworkAdapterConfiguration")
'            Using networkConfigs = networkConfigMng.GetInstances()
'                For Each managementObject As var In networkConfigs.Cast(Of ManagementObject)().Where(Function(objMO) CBool(objMO("IPEnabled")) AndAlso objMO("Caption").Equals(nic))
'                    Using newDNS = managementObject.GetMethodParameters("SetDNSServerSearchOrder")
'                        newDNS("DNSServerSearchOrder") = dnsServers.Split(","c)
'                        managementObject.InvokeMethod("SetDNSServerSearchOrder", newDNS, Nothing)
'                    End Using
'                Next
'            End Using
'        End Using
'    End Sub
'End Class