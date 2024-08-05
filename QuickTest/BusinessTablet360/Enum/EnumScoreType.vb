Public Enum enScoreType
    TestInClass = 1
    Homework = 2
End Enum

Public Class EnumScoreType
    Inherits EnumRegister

    Public Sub New()
        AddItem(enScoreType.TestInClass, "ผลทดสอบในห้อง")
        AddItem(enScoreType.Homework, "ผลทำการบ้าน")
    End Sub
End Class
