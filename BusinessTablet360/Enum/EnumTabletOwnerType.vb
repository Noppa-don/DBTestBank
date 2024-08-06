Public Enum EnTabletOwnerType
    Teacher = 1
    Student = 2
    Lab = 3
    Spare = 4
End Enum

Public Class EnumTabletOwnerType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnTabletOwnerType.Teacher, "ครู")
        AddItem(EnTabletOwnerType.Student, "นักเรียน")
        AddItem(EnTabletOwnerType.Lab, "เครื่องในห้องแล็บ")
        AddItem(EnTabletOwnerType.Spare, "เครื่องสำรองส่วนกลาง")
    End Sub

End Class
