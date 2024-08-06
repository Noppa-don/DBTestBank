
Public Enum EnStatusTabletUse
    Always
    Often
    Rarely
    Less
    None
End Enum

Public Class EnumStatusTabletUse
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStatusTabletUse.Always, "สม่ำเสมอ")
        AddItem(EnStatusTabletUse.Often, "บ่อยครั้ง")
        AddItem(EnStatusTabletUse.Rarely, "นานๆครั้ง")
        AddItem(EnStatusTabletUse.Less, "แทบจะไม่")
        AddItem(EnStatusTabletUse.None, "ไม่มีเลย")
    End Sub

End Class
