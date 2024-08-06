Public Enum EnStudentStatus
    Resign
    Study
    Graduating
    RestStudy
    NoOverClass 'กรณีไม่มีห้องเรียนในชั้นถัดไป เช่น อยู่ ม.1 แล้วชั้น ม.2 ไม่มีห้อง -- ไม่ใช่ไม่มีชั้นนะ ถ้าไม่มีชั้นจะเป็นจบการศึกษา 
    NewStudent 'นักเรียนที่เข้าใหม่ในช่วงปิดเทอม
End Enum

Public Class EnumStudentStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStudentStatus.Resign, "ลาออก")
        AddItem(EnStudentStatus.Study, "เรียนอยู่")
        AddItem(EnStudentStatus.Graduating, "จบการศึกษา")
        AddItem(EnStudentStatus.RestStudy, "พักการเรียน")
        AddItem(EnStudentStatus.NoOverClass, "ไม่มีห้องในชั้นถัดไป")
        AddItem(EnStudentStatus.NewStudent, "นักเรียนใหม่")
    End Sub

End Class
