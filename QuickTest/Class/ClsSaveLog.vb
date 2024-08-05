Imports System.Data.SQLite
Imports System.Globalization

Public Class ClsSaveLog
    Dim _DB As ClsConnect

    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Sub SaveLog(ByVal LogType As Integer, ByVal Description As String, ByVal UserId As Integer, Optional ByVal SetTime As String = "")

        Dim sql As String = "Insert into tblLog (LogType,Description,IsManualAction,UserId,IsActive,LastUpdate) "
        sql = sql & "VALUES ('" & LogType & "','" & Description & "',1,'" & UserId & "',1,datetime('now','localtime'))"
        _DB.Execute(sql)

        'logtype
        '0 จัดการข้อมูลผู้ใช้งาน
        '1 จัดชุดหนังสือ
        '2 จัดการข้อมูลหนังสือ
        '3 จัดการข้อมูลระบบ
        '4 เปลี่ยนรหัสผ่าน
        '5 จัดการระเบียบงบประมาณ server
        '6 จัดการข้อมูลโรงเรียน server
        '7 จัดการข้อมูลอื่นๆ 
        '8 สร้างรายงาน
        '9 อื่นๆ
        '10 อัพเดทAuto
        '11 login
        '12 แก้ไขชุดหนังสือเดิม
        '13 Home
        '14 Client Sync Error
        '15 Client Upload Complete

        'IsManualAction 
        '0 สั่งโดยระบบ
        '1 สั่งโดยผู้ใช้งาน

    End Sub

    Public Function GetLastClientSync() As DateTime?
        Dim MyDt = _DB.getdata("SELECT MAX(LastUpdate) as DateSync FROM  tblLog WHERE LogType=15 OR LogType=10")
        If MyDt.Rows(0)("DateSync") Is DBNull.Value Then
            Return Nothing
        Else
            Dim provider As CultureInfo = CultureInfo.InvariantCulture
            Dim MyDate As DateTime = DateTime.Parse(MyDt.Rows(0)("DateSync"), provider)
            Return MyDate
        End If
    End Function

    Public Function GetLastClientSyncLog() As DateTime?
        Dim MyDt = _DB.getdata("SELECT MAX(LastUpdate) as DateSync FROM  tblLog WHERE LogType=15")
        If MyDt.Rows(0)("DateSync") Is DBNull.Value Then
            Return Nothing
        Else
            Dim provider As CultureInfo = CultureInfo.InvariantCulture
            Dim MyDate As DateTime = DateTime.Parse(MyDt.Rows(0)("DateSync"), provider)
            Return MyDate
        End If
    End Function

End Class
