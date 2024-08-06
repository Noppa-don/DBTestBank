Public Enum EnStudentMoveType
    Normal = 1 'ปกติรวมไปถึงเข้าใหม่ และ เลื่อนชั้นด้วย
    MoveRoom = 2 'ย้ายห้อง

    EditClassUp = 6 'แบบนี้เราจะไม่ได้เอาสาถานะเก็บเข้าดาต้าเบสนะ แค่ไว้ดักเช็คตอนส่งจาก ui เข้า bll
    EditClassDown = 7 'แบบนี้เราจะไม่ได้เอาสาถานะเก็บเข้าดาต้าเบสนะ แค่ไว้ดักเช็คตอนส่งจาก ui เข้า bll
    EditRoom = 8 'แก้ไขจากหน้า แก้ไขนักเรียน
End Enum

Public Class EnumStudentMoveType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStudentMoveType.Normal, "ปกติ")
        AddItem(EnStudentMoveType.MoveRoom, "ย้ายห้อง")
    End Sub

End Class
