
Public Enum EnMobileAccessType
    Director
    It
    HeadThai
    HeadArt
    HeadWork
    HeadHealth
    HeadForeign
    HeadSocial
    HeadScience
    HeadMath
    HeadOther
End Enum

Public Class EnumMobileAccessType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnMobileAccessType.Director, "ผอ")
        AddItem(EnMobileAccessType.It, "it")
        AddItem(EnMobileAccessType.HeadThai, "กลุ่มสาระการเรียนรู้ภาษาไทย")
        AddItem(EnMobileAccessType.HeadArt, "กลุ่่มสาระการเรียนรู้ศิลปะ")
        AddItem(EnMobileAccessType.HeadWork, "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี")
        AddItem(EnMobileAccessType.HeadHealth, "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา")
        AddItem(EnMobileAccessType.HeadForeign, "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ")
        AddItem(EnMobileAccessType.HeadSocial, "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม")
        AddItem(EnMobileAccessType.HeadScience, "กลุ่มสาระการเรียนรู้วิทยาศาสตร์")
        AddItem(EnMobileAccessType.HeadMath, "กลุ่มสาระการเรียนรู้คณิตศาสตร์")
        AddItem(EnMobileAccessType.HeadOther, "อื่น")

    End Sub

End Class
