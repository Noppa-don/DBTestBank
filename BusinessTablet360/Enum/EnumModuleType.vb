Public Enum EnModuleType
    Homework
End Enum

Public Class EnumModuleType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnModuleType.Homework, "การบ้าน")
    End Sub
End Class

Public Enum EnModuleUI
    MdModule
    MdClass
    MdRoom
    MdStudent
    MdModuleHomeworkTime
End Enum

Public Class EnumModuleUI
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnModuleUI.MdModule, "MdModule")
        AddItem(EnModuleUI.MdClass, "MdClass")
        AddItem(EnModuleUI.MdRoom, "MdRoom")
        AddItem(EnModuleUI.MdStudent, "MdStudent")
        AddItem(EnModuleUI.MdModuleHomeworkTime, "MdModuleHomeworkTime")
    End Sub
End Class
