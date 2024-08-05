
Public Enum EnCalendarType
    Normal = 1
    Holiday = 2
    Term = 3
End Enum

Public Class EnumCalendarType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnCalendarType.Normal, "ทั่วไป")
        AddItem(EnCalendarType.Holiday, "วันหยุดนักขัตฤกษ์")
        AddItem(EnCalendarType.Term, "เทอมการศึกษา")
    End Sub

End Class
