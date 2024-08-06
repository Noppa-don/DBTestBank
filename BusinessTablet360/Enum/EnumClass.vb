Public Enum EnClass
    p1 = 1
    p2 = 2
    p3 = 3
    p4 = 4
    p5 = 5
    p6 = 6
    m1 = 7
    m2 = 8
    m3 = 9
    m4 = 10
    m5 = 11
    m6 = 12
End Enum

Public Class EnumClass
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnClass.p1, "ป.1")
        AddItem(EnClass.p2, "ป.2")
        AddItem(EnClass.p3, "ป.3")
        AddItem(EnClass.p4, "ป.4")
        AddItem(EnClass.p5, "ป.5")
        AddItem(EnClass.p6, "ป.6")
        AddItem(EnClass.m1, "ม.1")
        AddItem(EnClass.m2, "ม.2")
        AddItem(EnClass.m3, "ม.3")
        AddItem(EnClass.m4, "ม.4")
        AddItem(EnClass.m5, "ม.5")
        AddItem(EnClass.m6, "ม.6")
    End Sub

End Class
