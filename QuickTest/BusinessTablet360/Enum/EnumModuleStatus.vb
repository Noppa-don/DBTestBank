Public Enum EnModuleStatus
    NotDone
    NotFinish
    FinishNotComplete
    FinishComplete
End Enum

Public Class EnumModuleStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnModuleStatus.NotDone, "ไม่ทำ")
        AddItem(EnModuleStatus.NotFinish, "ทำไม่เสร็จ")
        AddItem(EnModuleStatus.FinishNotComplete, "ทำเสร็จไม่ครบ")
        AddItem(EnModuleStatus.FinishComplete, "ทำเสร็จครบ")
    End Sub

End Class
