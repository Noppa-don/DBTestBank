Public Enum EnFileColumnStudent
    PrefixName = 1
    FirstName = 2
    LastName = 3
    CurrentClass = 4
    CurrentRoom = 5
    Code = 0
    NoInRoom = 6
End Enum

Public Enum EnFileColumnTeacher
    PrefixName = 1
    FirstName = 2
    LastName = 3
    CurrentClass = 4
    CurrentRoom = 5
    Code = 0
    Phone = 6
    User = 7
    Password = 8
End Enum

Public Class EnumFileColumnStudent
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnFileColumnStudent.Code, "รหัส")
        AddItem(EnFileColumnStudent.PrefixName, "คำนำหน้าชื่อ")
        AddItem(EnFileColumnStudent.FirstName, "ชื่อ")
        AddItem(EnFileColumnStudent.LastName, "สกุล")
        AddItem(EnFileColumnStudent.CurrentClass, "ชั้น")
        AddItem(EnFileColumnStudent.CurrentRoom, "ห้อง")
        AddItem(EnFileColumnStudent.NoInRoom, "เลขที่")
    End Sub

End Class

Public Class EnumFileColumnTeacher
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnFileColumnTeacher.Code, "รหัส")
        AddItem(EnFileColumnTeacher.PrefixName, "คำนำหน้าชื่อ")
        AddItem(EnFileColumnTeacher.FirstName, "ชื่อ")
        AddItem(EnFileColumnTeacher.LastName, "สกุล")
        AddItem(EnFileColumnTeacher.CurrentClass, "ชั้น")
        AddItem(EnFileColumnTeacher.CurrentRoom, "ห้อง")
        AddItem(EnFileColumnTeacher.User, "ชื่อผู้ใช้งาน")
        AddItem(EnFileColumnTeacher.Password, "รหัสผ่าน")
    End Sub

End Class
