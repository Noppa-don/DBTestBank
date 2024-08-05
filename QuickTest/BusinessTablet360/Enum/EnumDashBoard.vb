Public Enum EnumDashBoardType
    Quiz = 1
    Homework = 2
    Practice = 3
    PrintTestset = 4
    SetUp = 5
    PracticeFromComputer = 6
End Enum


Public Class EnumDashBoard
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnumDashBoardType.Quiz, "ควิซ")
        AddItem(EnumDashBoardType.Homework, "การบ้าน")
        AddItem(EnumDashBoardType.Practice, "ฝึกฝน")
        AddItem(EnumDashBoardType.PrintTestset, "พิมพ์ข้อสอบ/Genword")
        AddItem(EnumDashBoardType.SetUp, "จัดชุดใหม่")
        AddItem(EnumDashBoardType.PracticeFromComputer, "ฝึกฝนจากคอมพิวเตอร์")
    End Sub

End Class
