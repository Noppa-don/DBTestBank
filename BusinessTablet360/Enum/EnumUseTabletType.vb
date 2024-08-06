Public Enum EnUseTabletType
    Other
    ReadBook
    DoHomework
    PlayInternet
    PlayGame
End Enum

Public Class EnumUseTabletType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnUseTabletType.Other, "อื่นๆ")
        AddItem(EnUseTabletType.ReadBook, "อ่านบทเรียน")
        AddItem(EnUseTabletType.DoHomework, "ทำการบ้าน")
        AddItem(EnUseTabletType.PlayInternet, "ใช้อินเทอร์เน็ต")
        AddItem(EnUseTabletType.PlayGame, "เล่นเกมส์")
    End Sub

End Class
