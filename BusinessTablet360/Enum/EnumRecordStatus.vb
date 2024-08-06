Public Enum EnRecordStatus
    Import = 1
    Modify = 2
    Skip = 3
    ChangeStatus = 5
    Delete = 6
End Enum

Public Class EnumRecordStatus
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnRecordStatus.Import, "นำเข้า")
        AddItem(EnRecordStatus.Modify, "ปรับปรุงข้อมูล")
        AddItem(EnRecordStatus.Skip, "ข้าม")
        AddItem(EnRecordStatus.ChangeStatus, "ปรับสถานะ")
        AddItem(EnRecordStatus.Delete, "ลาออก")
    End Sub
End Class
