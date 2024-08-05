Public Enum EnNetworkType
    AccessPoint = 1
    Server = 2
    Switch = 3
    Router = 4
    OTH = 10
End Enum

Public Class EnumNetworkType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnNetworkType.AccessPoint, "AP-AccessPoint")
        AddItem(EnNetworkType.Server, "SVR-Server")
        AddItem(EnNetworkType.Switch, "SW")
        AddItem(EnNetworkType.Router, "RT-Router")
        AddItem(EnNetworkType.OTH, "OTH-อื่นๆ")
    End Sub

End Class
