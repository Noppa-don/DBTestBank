Public Enum EnIsActiveStatus
    Delete
    Active
End Enum

Public Class EnumIsActiveStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnIsActiveStatus.Delete, "ข้อมูลถูกลบ")
        AddItem(EnIsActiveStatus.Active, "ข้อมูลยังใช้อยู่")
    End Sub

End Class
