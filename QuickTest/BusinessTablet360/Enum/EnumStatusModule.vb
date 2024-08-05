Public Enum EnStatusModule
    HomeworkNotStart
    HomeworkNotFinish
    HomeworkFinish
End Enum

Public Class EnumStatusModule
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStatusModule.HomeworkNotStart, "ยังไม่ทำการบ้าน")
        AddItem(EnStatusModule.HomeworkNotFinish, "ยังทำการบ้านไม่เสร็จ")
        AddItem(EnStatusModule.HomeworkFinish, "ทำการบ้านเสร็จแล้ว")

    End Sub

End Class
