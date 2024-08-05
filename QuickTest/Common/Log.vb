Imports KnowledgeUtils.System
Imports KnowledgeUtils
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
        BrowserAgentNotChrome = 21
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
    Public Shared Sub Record(ByVal loggingType As LogType, ByVal logText As String, ByVal isManualAction As Boolean, Optional ByVal StudentId As String = "", Optional QuestionId As String = "", Optional QSetId As String = "")

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

        'Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "'; "
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
        Else
            Try
                Dim KnSession As New KNAppSession()
                CalendarId = KnSession("CurrentCalendarId").ToString()
                SchoolCode = HttpContext.Current.Session("SchoolCode").ToString
            Catch ex As Exception
                ' แก้ไขปัญหาเฉพาะหน้าไปก่อน (standalone เก็บ log test เรื่อง session หลุด) ถ้า session หลุดจริง จะ get ค่า nothing มาทำให้ insert ไม่ได้ 
                userId = "AAA703A8-CEDC-471E-A3DB-E2501039E6CA"
                CalendarId = "AAA703A8-CEDC-471E-A3DB-E2501039E6CA"
                SchoolCode = "1"
            End Try
        End If



        Dim strsql As String

        If QuestionId = "" Then
            strsql = "insert into tbllog ( logtype, description, ismanualaction, userid, isactive, lastupdate,Calendar_Id,School_Code,ClientId) values "
            strsql = strsql + " ( '" & logTypeId.ToString() & "', N'" & logText.CleanSQL & "', '" & IIf(isManualAction, "1", "0") & "', '" & userId.CleanSQL & "', '1', dbo.GetThaiDate(),'" & CalendarId & "','" & SchoolCode.CleanSQL & "',null );"
        Else
            If QSetId = "" Then
                QSetId = "null"
            Else
                QSetId = "'" & QSetId & "'"
            End If

            strsql = "insert into tbllog ( logtype, description, ismanualaction, userid, isactive, lastupdate,Calendar_Id,School_Code,ClientId,QuestionId) values "
            strsql = strsql + " ( '" & logTypeId.ToString() & "', N'" & logText.CleanSQL & "', '" & IIf(isManualAction, "1", "0") & "', '" & userId.CleanSQL & "', '1', dbo.GetThaiDate(),'" & CalendarId & "','" & SchoolCode.CleanSQL & "',null,'" & QuestionId & "');"
        End If

        Dim CsSql As New ClassConnectSql
        CsSql.Execute(strsql)

    End Sub

    Public Shared Sub RecordLog(LogCat As LogCategory, LogAction As LogAction, IsManualAction As Boolean, DescriptionDetail As String, ReferenceId As String)

        Dim db As New ClassConnectSql()
        Try
            Dim UserId, SchoolCode As String
            Try
                If Not (HttpContext.Current.Session Is Nothing) Then
                    If Not (HttpContext.Current.Session("UserId") Is Nothing) Then
                        UserId = HttpContext.Current.Session("UserId").ToString
                    End If
                    SchoolCode = HttpContext.Current.Session("SchoolCode").ToString
                End If

            Catch ex As Exception
                UserId = "AAA703A8-CEDC-471E-A3DB-E2501039E6CA"
                SchoolCode = "1"
            End Try

            Dim isMA As String = "0"

            If IsManualAction Then
                isMA = "1"
            End If

            Dim RefId As String = "null"

            If ReferenceId <> "" Then
                RefId = "'" & ReferenceId & "'"
            End If

            Dim sql = "insert into tblSystemLog(LogCategory,LogAction,IsManualAction,Description,ReferenceId,UserId,SchoolCode) values('" & LogCat & "','" & LogAction & "'," & isMA & ",'" & DescriptionDetail & "'," & RefId & ",'" & UserId & "','" & SchoolCode & "')"

            db.OpenWithTransection()
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            db.CloseConnect()
        Catch ex As Exception
            db.RollbackTransection()
        End Try
    End Sub
    Public Enum LogCategory
        Login = 0
        DashboardTestset = 1
        DashboardPrint = 2
        SelectClassSubject = 3
        SelectQuestionSet = 4
        SelectQuestion = 5
        EditQuestion = 6
    End Enum


    Public Enum LogAction
        PageLoad = 0
        Click = 1
        Insert = 2
        Update = 3
        Delete = 4
    End Enum


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