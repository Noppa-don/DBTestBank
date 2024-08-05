Public Enum EnTabletStatus
    Normal = 1
    TabletReturn = 2
    Damage = 3
    Collapse = 4
    SendToFix = 5
    Lost = 6

End Enum

Public Enum EnTabletOwnerStatus
    NotOwner = 0
    Owner = 1
End Enum

Public Class EnumTabletStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnTabletStatus.Normal, "ลงทะเบียน")
        AddItem(EnTabletStatus.TabletReturn, "รับคืน")
        AddItem(EnTabletStatus.Damage, "เสียหาย (ใช้งานได้)")
        AddItem(EnTabletStatus.Collapse, "เสียหาย (ใช้งานไม่ได้)")
        AddItem(EnTabletStatus.SendToFix, "ส่งซ่อม")
        AddItem(EnTabletStatus.Lost, "สูญหาย")

        'มีเจ้าของ, ไม่มีเจ้าของ
        'ปกติ, แบตเสื่อม, ส่งซ่อม, แจ้งหาย, ขาดการติดต่อ,
        'ITabletManager(UpdateReturnTablet)*,EnTabletStatus* ,TabletManagePage.aspx*
    End Sub

End Class
