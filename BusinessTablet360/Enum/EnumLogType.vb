Public Enum EnLogType
    Login = 1
    Logout = 2
    Menu = 3
    Search = 4
    Insert = 5
    ImportantUpdate = 6
    Update = 7
    ImportantDelete = 8
    Delete = 9
    ManageUpLevelChangeRoom = 10
    ManageUpLevelChangeStatus = 11
    ManageUpLevelConfirm = 12
    SetTimeProcessUpLevel = 13
    ProcessUpLevel = 14
    ChangeStudentNoInGrid = 15
    ChangeStudentNoInSchool = 16
    ManageTabletStatus = 17
    LoadExcel = 18
    PrintQRCode = 19
    GenReport = 20
    SetTimeSync = 21
    SyncStatus = 22
    CopyCalendar = 23

    UpdateSubjectClass = 24
    'UpdateActiveClass = 25
    'TabletReturn = 26
    'ManageUpLevelEditStudent = 27
    'ManageUpLevelAddStudent = 28



End Enum

Public Class EnumLogType
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnLogType.LogIn, "เข้าสู่ระบบ")
        AddItem(EnLogType.Logout, "ออกจากระบบ")
        AddItem(EnLogType.Menu, "เข้าเมนู")
        AddItem(EnLogType.Search, "ค้นหา")
        AddItem(EnLogType.Insert, "เพิ่ม")
        AddItem(EnLogType.ImportantUpdate, "แก้ไขข้อมูลสำคัญ")
        AddItem(EnLogType.Update, "แก้ไข")
        AddItem(EnLogType.ImportantDelete, "ลบข้อมูลสำคัญ")
        AddItem(EnLogType.Delete, "ลบ")
        AddItem(EnLogType.ManageUpLevelChangeRoom, "จัดการเลื่อนชั้น - เปลี่ยนห้อง")
        AddItem(EnLogType.ManageUpLevelChangeStatus, "จัดการเลื่อนชั้น - เปลี่ยนสถานะนักเรียน")
        AddItem(EnLogType.ManageUpLevelConfirm, "จัดการเลื่อนชั้น - ยืนยันข้อมูล")
        AddItem(EnLogType.SetTimeProcessUpLevel, "จัดการเลื่อนชั้น - ตั้งวันที่มีผลล่วงหน้า")
        AddItem(EnLogType.ProcessUpLevel, "จัดการเลื่อนชั้น - ปรับปรุงข้อมุลให้มีผลทันที")
        AddItem(EnLogType.ChangeStudentNoInGrid, "จัดการเลื่อนชั้น - ปรับเลขที่โดยการลากวาง")
        AddItem(EnLogType.ChangeStudentNoInSchool, "จัดการเลื่อนชั้น - ปรับเลขที่ทั้งโรงเรียน")
        AddItem(EnLogType.ManageTabletStatus, "แจ้งสถานะแท็บเลต")
        AddItem(EnLogType.LoadExcel, "โหลด Excel")
        AddItem(EnLogType.PrintQRCode, "พิมพ์ QRCode")
        AddItem(EnLogType.GenReport, "ดูตัวอย่างก่อนพิมพ์")
        AddItem(EnLogType.SetTimeSync, "ตั้งเวลา Sync")
        AddItem(EnLogType.SyncStatus, "ตั้งสถานะ Sync")
        AddItem(EnLogType.CopyCalendar, "คัดลอกปฏิทิน")
    End Sub
End Class
