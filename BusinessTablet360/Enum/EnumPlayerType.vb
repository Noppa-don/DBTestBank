Public Enum EnPlayerType
    Teacher = 1
    Student = 2
End Enum

Public Class EnumPlayerType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnPlayerType.Teacher, "ครู")
        AddItem(EnPlayerType.Student, "นักเรียน")
    End Sub
End Class
