Public Enum EnValidateStatus

    NotFound = 0
    DuplicateSome = 2
    DuplicateCode = 3
    DuplicateCodeAndName = 4
    DuplicateAll = 5
    LostClassRoom = 6
    LostData = 7
    falseData = 8

End Enum

Public Class EnumValidateStatus
    Inherits EnumRegister

    Public Sub New()

        'ไม่พบใน DB นำเข้าได้
        AddItem(EnValidateStatus.NotFound, "ไม่พบ")

        'ซ้ำทุกอย่างยกเว้นชื่อ เดาว่าเคยพิมพ์ชื่อผิด ให้เลือกว่าจะปรับปรุง หรือไม่
        AddItem(EnValidateStatus.DuplicateSome, "ซ้ำทุกอย่างยกเว้นชื่อ")

        'รหัสซ้ำอย่างเดียว ให้ข้ามไป
        AddItem(EnValidateStatus.DuplicateCode, "ซ้ำเฉพาะรหัส")

        'ซ้ำทุกอย่าง ยกเว้น รหัสและชื่อ เดาว่าเพิ่มนักเรียนคนใหม่มาซ้ำกับนักเรียนที่มีอยู่แล้ว ให้ข้ามไป
        AddItem(EnValidateStatus.DuplicateCodeAndName, "ซ้ำรหัสและชื่อ")

        'ปรับสถานะ ต้องดึงสถานะมาใส่ Combo ให้เลือกด้วย
        AddItem(EnValidateStatus.DuplicateAll, "ซ้ำทั้งหมด")

        'ชั้นห้องไม่มี
        AddItem(EnValidateStatus.LostClassRoom, "ข้อมูลผิดเงื่อนไข")

        'อันนี้ไม่น่าเป็นไปได้เพราะหน้า Upload เช็ตอยู่แล้ว
        AddItem(EnValidateStatus.LostData, "ข้อมูลไม่ครบ")

        'อันนี้ของครู
        AddItem(EnValidateStatus.falseData, "ข้อมูลไม่ครบ")


    End Sub
End Class
