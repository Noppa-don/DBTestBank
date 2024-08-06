Public Enum EnStudentProcessType
    Up
    Finish
End Enum

Public Class EnumStudentProcessType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStudentProcessType.Up, "เลื่อนชั้น")
        AddItem(EnStudentProcessType.Finish, "จบ")
    End Sub
End Class
