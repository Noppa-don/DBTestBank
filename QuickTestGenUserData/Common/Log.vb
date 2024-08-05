Imports KnowledgeUtils.System
Imports BusinessTablet360

Public Class Log
    Public Enum LogType
        Home = 99
        Login = 1
        ManageBook = 2
        AdminContact = 3
        ManageUser = 4
        AdminLog = 5
        ManageUserAdmin = 6
        SetEmail = 7
        ChangePassword = 8
        UploadPic = 9
        GenPDF = 10
        ExamStep = 11
        SetEvaluationIndex = 12
        ManageExam = 13
        SetFavoriteStudent = 14
        PageLoad = 15
        Homework = 16
        Quiz = 17
        Practice = 18
        ParentRegister = 19
        StudentOpenTabletApp = 20
        GenUser = 98
    End Enum
    Public Shared Function EmptyIfNull(ByRef inobj As Object) As String
        If inobj Is Nothing Then
            Return ""
        Else
            Return inobj.ToString
        End If
        Return ""
    End Function
    Public Shared Function ZeroIfNull(ByRef inobj As Object) As Integer
        If inobj Is Nothing Then
            Return 0
        Else
            Return CInt(inobj)
        End If
        Return ""
    End Function
    'ชิน ปิดไป, เพราะเปลี่ยนจากการใช้ int, ไปใช้การทำงานผ่าน guid -> newid() แทน, 20/1/2557
    'Private Shared Function MaxAuto() As Integer

    '    Dim sql As String = "Select max(Logid) as Maxid from tblLog where IsActive = 1 and ClientId = '0'"
    '    Dim CsSql As New ClassConnectSql
    '    Dim dtMax As DataTable = CsSql.getdata(sql)
    '    If dtMax.Rows(0)("Maxid") Is DBNull.Value Then
    '        MaxAuto = 1

    '    Else
    '        MaxAuto = CInt(dtMax.Rows(0)("Maxid")) + 1
    '    End If


    'End Function
    Public Shared Sub Record(ByVal loggingType As LogType, ByVal logText As String, ByVal isManualAction As Boolean, Optional ByVal StudentId As String = "")

        If ConfigurationManager.AppSettings("IsQuicktestProduction") = False Then
            Exit Sub
        End If

        Dim userId As String = 0
        If StudentId = "" Then
            If Not (HttpContext.Current.Session Is Nothing) Then
                If Not (HttpContext.Current.Session("userid") Is Nothing) Then
                    userId = HttpContext.Current.Session("UserId").ToString
                End If
            End If
        Else
            userId = StudentId
        End If

        Dim logTypeId As Integer
        logTypeId = DirectCast([Enum].Parse(GetType(LogType), [Enum].GetName(GetType(LogType), loggingType)), Integer)

        'Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE GETDATE() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
        'Dim db As New ClassConnectSql()
        'Dim CalendarId As String = db.ExecuteScalar(sql)

        ' - ใช้ canledarid จาก knsession แทน
        'Dim ClsSelectSess As New ClsSelectSession()
        'Dim dt As DataTable = ClsSelectSess.GetCalendarID(HttpContext.Current.Session("SchoolID").ToString())
        Dim CalendarId As String
        Dim SchoolCode As String


        'ไหมเพิ่ม LogType ตัวใหม่เพื่อใช้กับตัว GenUser 
        If loggingType = LogType.GenUser Then
            CalendarId = "EA271D72-0E25-4659-898D-7B0BE812BAF0"
            SchoolCode = "0"
            userId = "EA271D72-0E25-4659-898D-7B0BE812BAF0"

        End If


        Dim strsql As String
        strsql = "insert into tbllog ( logid, guid, logtype, description, ismanualaction, userid, isactive, lastupdate,Calendar_Id,School_Code,ClientId) values "
        strsql = strsql + " ( '1', newid(), '" & logTypeId.ToString() & "', '" & logText & "', '" & IIf(isManualAction, "1", "0") & "', '" & userId & "', '1', getdate(),'" & CalendarId & "','" & SchoolCode & "','QTProduction' );"
        Dim CsSql As New ClassConnectSql
        CsSql.Execute(strsql)

    End Sub


End Class



Public Enum EnLogType
    AllType = 0
    Login = 1
    Managebook = 2
    AdminContact = 3
    ManageUser = 4
    AdminLog = 5
    ManageUserAdmin = 6
    SetEmail = 7
    ChangePassword = 8
    UploadPic = 9
    GenPDF = 10
    ExamStep = 11
    ManageExam = 12
End Enum

Public Class EnumLogType
    Inherits EnumRegister

    Public Sub New()
        Add(New EnumItem With {.Text = "ดูทั้งหมด", .Value = EnLogType.AllType})
        Add(New EnumItem With {.Text = "ดูประวัติการเข้าระบบ", .Value = EnLogType.Login})
        Add(New EnumItem With {.Text = "ดูประวัติการจัดชุดข้อสอบใหม่", .Value = EnLogType.Managebook})
        Add(New EnumItem With {.Text = "ดูประวัติคำถามเพิ่มเติม", .Value = EnLogType.AdminContact})
        Add(New EnumItem With {.Text = "ดูประวัติจัดการข้อมูลผู้ใช้(โรงเรียน)", .Value = EnLogType.ManageUser})
        Add(New EnumItem With {.Text = "ดูประวัติจัดการข้อมูลผู้ใช้(Admin)", .Value = EnLogType.ManageUserAdmin})
        Add(New EnumItem With {.Text = "ดูประวัติการใช้งานระบบ", .Value = EnLogType.AdminLog})
        Add(New EnumItem With {.Text = "ดูประวัติการตั้งค่าEmailรับข้อความจากหน้าเว็บ", .Value = EnLogType.SetEmail})
        Add(New EnumItem With {.Text = "ดูประวัติการเปลี่ยนรหัสผ่าน", .Value = EnLogType.ChangePassword})
        Add(New EnumItem With {.Text = "ดูประวัติการอัพโหลดรูปภาพ", .Value = EnLogType.UploadPic})
        Add(New EnumItem With {.Text = "ดูประวัติการสร้างไฟล์ข้อสอบ", .Value = EnLogType.GenPDF})
        Add(New EnumItem With {.Text = "ดูประวัติการจัดชุดข้อสอบ", .Value = EnLogType.ExamStep})
        Add(New EnumItem With {.Text = "แก้ไขจัดการข้อสอบ", .Value = EnLogType.ManageExam})
    End Sub

End Class