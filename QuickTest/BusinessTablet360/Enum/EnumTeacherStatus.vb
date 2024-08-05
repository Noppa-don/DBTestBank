Public Enum EnTeacherStatus
    Resign
    Teach
End Enum

Public Class EnumTeacherStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStudentStatus.Resign, "ลาออก")
        AddItem(EnStudentStatus.Study, "สอนอยู่")
    End Sub

End Class
