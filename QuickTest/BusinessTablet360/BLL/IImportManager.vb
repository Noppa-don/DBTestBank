Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils
Imports Excel = Microsoft.Office.Interop.Excel

Public Interface IImportManager

    Function ImportCVS(ByVal File As String, Optional ByVal IsSkip As Boolean = True, Optional ByVal IsStudent As Boolean = True, Optional ByVal FilterClass As String = "", Optional ByVal FilterRoom As String = "") As Boolean
    Function InsertTempStudent(ByVal Items As IEnumerable(Of t360_tblTempStudent)) As Boolean
    Function DeleteTempStudent() As Boolean
    Function InsertTempTeacher(ByVal Items As IEnumerable(Of t360_tblTempTeacher)) As Boolean
    Function DeleteTempTeacher() As Boolean
    Function GetTempStudentAll() As t360_tblTempStudent()
    Function GetTempStudentproblem() As t360_tblTempStudent()
    Function GetTempStudentReadyImport() As t360_tblTempStudent()
    Function GetTempStudentByStudentCode(ByVal Student_Code As String) As t360_tblTempStudent
    Function GetTempTeacherAll() As t360_tblTempTeacher()
    Function GetTempTeacherByTeacherCode(ByVal Teacher_Code As String) As t360_tblTempTeacher

End Interface

Public Class ImportManager
    Implements IImportManager


#Region "Dependency"

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

    Private _StudentManager As IStudentManager
    <Dependency()> Public Property StudentManager() As IStudentManager
        Get
            Return _StudentManager
        End Get
        Set(ByVal value As IStudentManager)
            _StudentManager = value
        End Set
    End Property

    Private _TeacherManager As ITeacherManager
    <Dependency()> Public Property TeacherManager() As ITeacherManager
        Get
            Return _TeacherManager
        End Get
        Set(ByVal value As ITeacherManager)
            _TeacherManager = value
        End Set
    End Property

    Private _SchoolManager As ISchoolManager
    <Dependency()> Public Property SchoolManager() As ISchoolManager
        Get
            Return _SchoolManager
        End Get
        Set(ByVal value As ISchoolManager)
            _SchoolManager = value
        End Set
    End Property

    Private _UserManager As IUserManager
    <Dependency()> Public Property UserManager() As IUserManager
        Get
            Return _UserManager
        End Get
        Set(ByVal value As IUserManager)
            _UserManager = value
        End Set
    End Property

#End Region

    Public Function ImportCVS(ByVal File As String, Optional ByVal IsSkip As Boolean = True, Optional ByVal IsStudent As Boolean = True, Optional ByVal FilterClass As String = "", Optional ByVal FilterRoom As String = "") As Boolean Implements IImportManager.ImportCVS
        Dim Mf As New KnowledgeUtils.IO.ManageFile
        Dim CodeInFile As String = ""
        Dim App As New Excel.Application
        Dim WS As Excel.Worksheet
        Dim WB As Excel.Workbook

        Try
            Dim ArrayOneLine() As String
            Dim ListStudent As New List(Of t360_tblTempStudent)
            Dim ListTeacher As New List(Of t360_tblTempTeacher)
            Dim IsPass As Boolean

            'ลบ Temp Table
            If IsStudent Then
                IsPass = DeleteTempStudent()
            Else
                IsPass = DeleteTempTeacher()
            End If
            If Not IsPass Then
                Throw New Exception("พบข้อผิดพลาดตอนลบ temp")
            End If

            'อ่านไฟล์
            If Mf.CheckFile(File) Then
                WB = App.Workbooks.Open(File)
                WS = WB.Worksheets(1)
                Dim UR = WS.UsedRange
                Dim RowsCount As Integer = UR.Rows.Count
                Dim ColsCount As Integer = UR.Columns.Count
                Dim URA As Object(,) = UR.Value
                WB.Close()
                App.Quit()

                'ตรวจสอบไฟล์ทีละ Row
                For row As Integer = 2 To RowsCount

                    Dim TxtLine As String = ""
                    For col As Integer = 1 To ColsCount
                        TxtLine &= URA(row, col) & ","
                    Next
                    TxtLine = TxtLine.Substring(0, TxtLine.Length - 1)
                    ArrayOneLine = TxtLine.Split(",")

                    If IsStudent Then
                        CodeInFile = ArrayOneLine(EnFileColumnStudent.Code)
                    Else
                        CodeInFile = ArrayOneLine(EnFileColumnTeacher.Code)
                    End If
                    'Status ของการ Validate Row นั้น
                    Dim ValidateStatus As EnValidateStatus

                    'การดำเนินการขั้นต่อไป
                    Dim RecordStatus As EnRecordStatus
                    Dim IsCheck As Boolean = True

                    'ข้อมูลของนักเรียนในฐานข้อมูล
                    Dim DataInformation As String = ""

                    'แจ้งเตือนว่าควรทำอย่างไรต่อ
                    Dim ValidateMessage As String = ""


                    Select Case IsStudent
                        Case True
                            '<<< นักเรียน >>>
                            '<<< กำหนด ValidateStutus

                            Dim Student As t360_tblStudent
                            Dim StudentStatus As Byte? = Nothing

                            ValidateStatus = EnValidateStatus.NotFound

                            'Case 1 : LostClassRoom
                            If ValidateStatus = EnValidateStatus.NotFound Then
                                ' เช็คชั้น
                                Dim FoundClass = SchoolManager.GetClassInSchool.Where(Function(q) q.Class_Name = ArrayOneLine(EnFileColumnStudent.CurrentClass)).SingleOrDefault
                                If FoundClass Is Nothing Then
                                    ValidateStatus = EnValidateStatus.LostClassRoom
                                    DataInformation = String.Format("ไม่มีชั้น {0} ที่เมนูโรงเรียน", ArrayOneLine(EnFileColumnStudent.CurrentClass))
                                    ValidateMessage = String.Format("เข้าไปเพิ่มชั้น {0} ที่เมนูโรงเรียนก่อนนะคะ", ArrayOneLine(EnFileColumnStudent.CurrentClass))
                                Else
                                    Dim FoundRoom = SchoolManager.GetRoomByClassName(New t360_tblRoom With {.Class_Name = ArrayOneLine(EnFileColumnStudent.CurrentClass)}).Where(Function(q) q.Room_Name = ArrayOneLine(EnFileColumnStudent.CurrentRoom)).SingleOrDefault
                                    ' เช็คห้อง
                                    If FoundRoom Is Nothing Then
                                        ValidateStatus = EnValidateStatus.LostClassRoom
                                        If ValidateMessage = "" Then
                                            DataInformation = String.Format("ไม่มีห้อง {0} ในชั้น {1} ที่เมนูโรงเรียน", ArrayOneLine(EnFileColumnStudent.CurrentRoom), ArrayOneLine(EnFileColumnStudent.CurrentClass))
                                            ValidateMessage = String.Format("เข้าไปเพิ่มห้อง {0} ในชั้น {1} ที่เมนูโรงเรียนก่อนนะคะ", ArrayOneLine(EnFileColumnStudent.CurrentRoom), ArrayOneLine(EnFileColumnStudent.CurrentClass))
                                        End If
                                    End If
                                End If

                            End If

                            'If ValidateStatus = EnValidateStatus.NotFound Then
                            '    'กรณีข้อมูลไม่ครบ
                            '    If ArrayOneLine(EnFileColumnStudent.Code) = "" OrElse ArrayOneLine(EnFileColumnStudent.PrefixName) = "" OrElse
                            '       ArrayOneLine(EnFileColumnStudent.FirstName) = "" OrElse ArrayOneLine(EnFileColumnStudent.LastName) = "" OrElse
                            '       ArrayOneLine(EnFileColumnStudent.CurrentClass) = "" OrElse ArrayOneLine(EnFileColumnStudent.CurrentRoom) = "" OrElse
                            '       ArrayOneLine(EnFileColumnStudent.NoInRoom) = "" Then
                            '        'กรณีข้อมูลไม่ครบ
                            '        ValidateStatus = EnValidateStatus.LostData
                            '    End If
                            'End If

                            'Case 2 : DuplicateAll
                            If ValidateStatus = EnValidateStatus.NotFound Then
                                Student = StudentManager.GetStudentByCrit(Of t360_tblStudent)(New StudentDTO With {.Student_Code = ArrayOneLine(EnFileColumnStudent.Code),
                                                                                                              .School_Code = UserConfig.GetCurrentContext.School_Code,
                                                                                                              .Student_FirstName = ArrayOneLine(EnFileColumnStudent.FirstName),
                                                                                                              .Student_LastName = ArrayOneLine(EnFileColumnStudent.LastName),
                                                                                                               .Student_CurrentClass = ArrayOneLine(EnFileColumnStudent.CurrentClass),
                                                                                                              .Student_CurrentRoom = ArrayOneLine(EnFileColumnStudent.CurrentRoom),
                                                                                                              .Student_CurrentNoInRoom = ArrayOneLine(EnFileColumnStudent.NoInRoom),
                                                                            .Student_IsActive = True}).SingleOrDefault

                                If Student IsNot Nothing Then
                                    ValidateStatus = EnValidateStatus.DuplicateAll
                                    Select Case Student.Student_Status
                                        Case EnStudentStatus.Resign
                                            StudentStatus = EnStudentStatus.Resign
                                            DataInformation = "มีนักเรียนในระบบแล้ว แต่สถานะเป็นลาออก"
                                            ValidateMessage = "ควรไปตรวจสอบว่านักเรียนได้ลาออกไปแล้วหรือไม่"
                                        Case EnStudentStatus.RestStudy
                                            StudentStatus = EnStudentStatus.RestStudy
                                            DataInformation = "มีนักเรียนในระบบแล้ว แต่สถานะเป็นพักการเรียน"
                                            ValidateMessage = "ควรไปตรวจสอบว่านักเรียนได้พักการเรียนจริงหรือไม่"
                                        Case Else
                                            StudentStatus = EnStudentStatus.Study
                                            DataInformation = "มีนักเรียนในระบบแล้ว"
                                            ValidateMessage = "ควรไปตรวจสอบว่านักเรียนได้มีการเปลี่ยนสถานะเป็นพักการเรียน หรือ ลาออกหรือไม่"
                                    End Select
                                End If
                            End If

                            'Case 3 : DuplicateSome
                            If ValidateStatus = EnValidateStatus.NotFound Then
                                Student = StudentManager.GetStudentByCrit(Of t360_tblStudent)(New StudentDTO With {.Student_Code = ArrayOneLine(EnFileColumnStudent.Code),
                                                                                                          .School_Code = UserConfig.GetCurrentContext.School_Code,
                                                                                                          .Student_CurrentClass = ArrayOneLine(EnFileColumnStudent.CurrentClass),
                                                                                                          .Student_CurrentRoom = ArrayOneLine(EnFileColumnStudent.CurrentRoom),
                                                                                                          .Student_CurrentNoInRoom = ArrayOneLine(EnFileColumnStudent.NoInRoom),
                                                                                                         .Student_IsActive = True}).SingleOrDefault
                                If Student IsNot Nothing Then
                                    ValidateStatus = EnValidateStatus.DuplicateSome
                                    DataInformation = "มีนักเรียนรหัส ชั้น เลขที่ นี้แล้ว แต่ชื่อไม่เหมือนกัน"
                                    ValidateMessage = "ควรไปตรวจสอบว่านักเรียนคนนี้ได้เคยกรอกชื่อผิดใช่หรือไม่"
                                End If
                            End If

                            'Case 4 : DuplicateCodeAndName
                            If ValidateStatus = EnValidateStatus.NotFound Then
                                Student = StudentManager.GetStudentByCrit(Of t360_tblStudent)(New StudentDTO With {
                                                                                                       .School_Code = UserConfig.GetCurrentContext.School_Code,
                                                                                                       .Student_CurrentClass = ArrayOneLine(EnFileColumnStudent.CurrentClass),
                                                                                                       .Student_CurrentRoom = ArrayOneLine(EnFileColumnStudent.CurrentRoom),
                                                                                                       .Student_CurrentNoInRoom = ArrayOneLine(EnFileColumnStudent.NoInRoom),
                                                                                                      .Student_IsActive = True}).SingleOrDefault
                                If Student IsNot Nothing Then
                                    DataInformation = "มีนักเรียนชั้น ห้อง เลขที่นี้แล้ว แต่ชื่อและรหัสไม่เหมือนกัน"
                                    ValidateStatus = EnValidateStatus.DuplicateCodeAndName
                                End If
                            End If

                            'Case 5 : 
                            If ValidateStatus = EnValidateStatus.NotFound Then
                                Student = StudentManager.GetStudentByCrit(Of t360_tblStudent)(New StudentDTO With {.Student_Code = ArrayOneLine(EnFileColumnStudent.Code),
                                                                                                                                    .School_Code = UserConfig.GetCurrentContext.School_Code,
                                                                                                                                   .Student_IsActive = True}).SingleOrDefault
                                If Student Is Nothing Then
                                    ValidateStatus = EnValidateStatus.NotFound
                                    DataInformation = ""
                                    ValidateMessage = "ไม่พบนักเรียนคนนี้ในระบบ"
                                Else
                                    ValidateStatus = EnValidateStatus.DuplicateCode
                                    DataInformation = "รหัสนักเรียนซ้ำกับ " & Student.Student_PrefixName & " " & Student.Student_FirstName & " " & Student.Student_LastName & " ชั้น " &
                                        Student.Student_CurrentClass & Student.Student_CurrentRoom & " เลขที่ " & Student.Student_CurrentNoInRoom

                                End If
                            End If

                            '<<< กำหนด RecordStutus
                            Select Case ValidateStatus
                                Case EnValidateStatus.NotFound
                                    RecordStatus = EnRecordStatus.Import
                                Case EnValidateStatus.DuplicateAll
                                    RecordStatus = EnRecordStatus.ChangeStatus
                                Case EnValidateStatus.DuplicateSome
                                    RecordStatus = EnRecordStatus.Modify
                                Case EnValidateStatus.LostClassRoom, EnValidateStatus.DuplicateCodeAndName, EnValidateStatus.DuplicateCode
                                    RecordStatus = EnRecordStatus.Skip
                            End Select

                            '<<< กำหนด IsCheck
                            If RecordStatus = EnRecordStatus.Skip Then
                                IsCheck = False
                            End If

                            Dim Data As New t360_tblTempStudent
                            With Data
                                .Is_Check = IsCheck
                                .Validate_Status = ValidateStatus
                                .Validate_StatusMessage = DataInformation
                                .Validate_Message = ValidateMessage
                                .Record_Status = RecordStatus
                                .Student_Information = ArrayOneLine(EnFileColumnStudent.PrefixName) & " " & ArrayOneLine(EnFileColumnStudent.FirstName) & " " & ArrayOneLine(EnFileColumnStudent.LastName)
                                .Student_Code = ArrayOneLine(EnFileColumnStudent.Code)
                                .Student_PrefixName = ArrayOneLine(EnFileColumnStudent.PrefixName)
                                .Student_FirstName = ArrayOneLine(EnFileColumnStudent.FirstName)
                                .Student_LastName = ArrayOneLine(EnFileColumnStudent.LastName)
                                .Student_CurrentClass = ArrayOneLine(EnFileColumnStudent.CurrentClass)
                                .Student_CurrentRoom = ArrayOneLine(EnFileColumnStudent.CurrentRoom)
                                .Student_CurrentNoInRoom = ArrayOneLine(EnFileColumnStudent.NoInRoom)
                                .Student_Status = StudentStatus
                                If Student IsNot Nothing Then
                                    .Student_Id = Student.Student_Id
                                End If
                            End With
                            ListStudent.Add(Data)
                            Student = Nothing
                        Case False
                            '<<< ครู >>>'

                            '<<< กำหนด ValidateStutus
                            Dim Teacher As t360_tblTeacher
                            ValidateStatus = EnValidateStatus.DuplicateSome
                            'If ValidateStatus = EnValidateStatus.DuplicateSome Then
                            '    'กรณีข้อมูลไม่ครบ
                            '    If ArrayOneLine(EnFileColumnTeacher.Code) = "" OrElse ArrayOneLine(EnFileColumnTeacher.PrefixName) = "" OrElse _
                            '       ArrayOneLine(EnFileColumnTeacher.FirstName) = "" OrElse ArrayOneLine(EnFileColumnTeacher.LastName) = "" OrElse _
                            '       ArrayOneLine(EnFileColumnTeacher.CurrentClass) = "" OrElse ArrayOneLine(EnFileColumnTeacher.CurrentRoom) = "" OrElse _
                            '       ArrayOneLine(EnFileColumnTeacher.User) = "" OrElse ArrayOneLine(EnFileColumnTeacher.Password) = "" Then
                            '        ValidateStatus = EnValidateStatus.LostData
                            '    End If
                            'End If
                            If ValidateStatus = EnValidateStatus.DuplicateSome Then
                                'เช็คกรณีข้อมูลผิดเงื่อนไข
                                Dim FoundClass = SchoolManager.GetClassInSchool.Where(Function(q) q.Class_Name = ArrayOneLine(EnFileColumnTeacher.CurrentClass)).SingleOrDefault
                                If FoundClass Is Nothing Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    'ValidateMessage &= "ไม่พบข้อมูล " & (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.CurrentClass) & " "
                                    ValidateMessage &= "ไม่พบข้อมูล " & ArrayOneLine(EnFileColumnTeacher.CurrentClass) & " "
                                End If
                                Dim FoundRoom = SchoolManager.GetRoomByClassName(New t360_tblRoom With {.Class_Name = ArrayOneLine(EnFileColumnTeacher.CurrentClass)}).Where(Function(q) q.Room_Name = ArrayOneLine(EnFileColumnTeacher.CurrentRoom)).SingleOrDefault
                                If FoundRoom Is Nothing Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    If ValidateMessage = "" Then
                                        'ValidateMessage = "ไม่พบข้อมูล " & (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.CurrentRoom) & " "
                                        ValidateMessage = "ไม่พบข้อมูล " & ArrayOneLine(EnFileColumnTeacher.CurrentRoom) & " "
                                    Else
                                        'ValidateMessage &= "," & (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.CurrentRoom) & " "
                                        ValidateMessage &= "," & ArrayOneLine(EnFileColumnTeacher.CurrentRoom) & " "
                                    End If

                                End If
                                Dim FoundUser = (UserManager.GetUserByUserName(ArrayOneLine(EnFileColumnTeacher.User), UserConfig.GetCurrentContext.School_Code) IsNot Nothing)
                                If FoundUser Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ชื่อผู้ใช้งานนี้ได้ถูกใช้งานแล้ว"
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.FirstName) = "" Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ไม่ได้ระบุชื่อค่ะ"
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.LastName) = "" Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ไม่ได้ระบุนามสกุลค่ะ"
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.Code) = "" Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ไม่ได้ระบุรหัสประจำตัวครูค่ะ"
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.User) = "" Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ไม่ได้ระบุชื่อเข้าใช้งานค่ะ"
                                ElseIf ArrayOneLine(EnFileColumnTeacher.Password) = "" Then
                                    ValidateStatus = EnValidateStatus.falseData
                                    ValidateMessage &= "ไม่ได้ระบุรหัสผ่านเข้าใช้งานค่ะ"
                                End If

                            End If
                            If ValidateStatus = EnValidateStatus.DuplicateSome Then
                                'กรณีข้อมูลไม่พบ
                                Teacher = TeacherManager.GetTeacherByCrit(Of t360_tblTeacher)(New TeacherDTO With {.Teacher_Code = ArrayOneLine(EnFileColumnTeacher.Code),
                                                                                                              .School_Code = UserConfig.GetCurrentContext.School_Code,
                                                                                                              .Teacher_IsActive = True}).SingleOrDefault
                                If Teacher Is Nothing Then
                                    ValidateStatus = EnValidateStatus.NotFound
                                Else
                                    DataInformation = Teacher.Teacher_PrefixName & " " & Teacher.Teacher_FirstName & " " & Teacher.Teacher_LastName & " ชั้น " &
                                                      Teacher.Teacher_CurrentClass & " " & Teacher.Teacher_CurrentRoom & " รหัส " & Teacher.Teacher_Code
                                End If
                            End If
                            If ValidateStatus = EnValidateStatus.DuplicateSome Then
                                'กรณีซ้ำทั้งหมด
                                Dim CountDuplicate As Integer = 0
                                If ArrayOneLine(EnFileColumnTeacher.Code) = Teacher.Teacher_Code Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.Code) & "ซ้ำ "
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.PrefixName) = Teacher.Teacher_PrefixName Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.PrefixName) & "ซ้ำ "
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.FirstName) = Teacher.Teacher_FirstName Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.FirstName) & "ซ้ำ "
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.LastName) = Teacher.Teacher_LastName Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.LastName) & "ซ้ำ "
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.CurrentClass) = Teacher.Teacher_CurrentClass Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.CurrentClass) & "ซ้ำ "
                                End If
                                If ArrayOneLine(EnFileColumnTeacher.CurrentRoom) = Teacher.Teacher_CurrentRoom Then
                                    CountDuplicate += 1
                                    ValidateMessage &= (New EnumFileColumnTeacher).GetText(EnFileColumnTeacher.CurrentRoom) & "ซ้ำ "
                                End If

                                If CountDuplicate = (New EnumFileColumnTeacher).Where(Function(q) q.Value <> EnFileColumnTeacher.Phone).Count Then
                                    ValidateStatus = EnValidateStatus.DuplicateAll
                                End If
                            End If

                            '<<< กำหนด RecordStutus
                            Select Case ValidateStatus
                                Case EnValidateStatus.NotFound
                                    RecordStatus = EnRecordStatus.Import
                                Case EnValidateStatus.LostData, EnValidateStatus.falseData
                                    RecordStatus = EnRecordStatus.Skip
                                Case EnValidateStatus.DuplicateAll
                                    RecordStatus = EnRecordStatus.Skip
                                Case EnValidateStatus.DuplicateSome
                                    If IsSkip Then
                                        RecordStatus = EnRecordStatus.Skip
                                    Else
                                        RecordStatus = EnRecordStatus.Modify
                                    End If
                            End Select

                            '<<< กำหนด IsCheck
                            If RecordStatus = EnRecordStatus.Skip Then
                                IsCheck = False
                            End If

                            Dim Data As New t360_tblTempTeacher
                            With Data
                                .Is_Check = IsCheck
                                .Validate_Status = ValidateStatus
                                .Validate_StatusMessage = (New EnumValidateStatus).GetText(ValidateStatus)
                                .Validate_Message = ValidateMessage
                                .Record_Status = RecordStatus
                                .Teacher_Information = DataInformation
                                .Teacher_Code = ArrayOneLine(EnFileColumnTeacher.Code)
                                .Teacher_PrefixName = ArrayOneLine(EnFileColumnTeacher.PrefixName)
                                .Teacher_FirstName = ArrayOneLine(EnFileColumnTeacher.FirstName)
                                .Teacher_LastName = ArrayOneLine(EnFileColumnTeacher.LastName)
                                .Teacher_CurrentClass = ArrayOneLine(EnFileColumnTeacher.CurrentClass)
                                .Teacher_CurrentRoom = ArrayOneLine(EnFileColumnTeacher.CurrentRoom)
                                .Teacher_Phone = ArrayOneLine(EnFileColumnTeacher.Phone)
                                .UserName = ArrayOneLine(EnFileColumnTeacher.User)
                                .Password = Encryption.MD5(ArrayOneLine(EnFileColumnTeacher.Password)).ToString
                                If Teacher IsNot Nothing Then
                                    .Teacher_Id = Teacher.Teacher_id
                                Else
                                    .Teacher_Id = Guid.NewGuid()
                                End If
                            End With
                            ListTeacher.Add(Data)
                    End Select
                Next

                If ListStudent.Count > 0 Then
                    If InsertTempStudent(ListStudent) Then
                        Return True
                    Else
                        Dim FileFalse As New KnowledgeUtils.IO.ManageFile
                        Mf.CloseFile()
                        FileFalse.DeleteFile(UserConfig.GetPhysicalPathTempFile & "UploadFalse.txt")
                        Dim Msg = "พบข้อผิดพลาดในการนำเข้าข้อมูล "
                        FileFalse.CreateFile(UserConfig.GetPhysicalPathTempFile & "UploadFalse.txt", Msg, False)
                        Return False
                    End If
                End If
                If ListTeacher.Count > 0 Then
                    If InsertTempTeacher(ListTeacher) Then
                        Return True
                    Else
                        Dim FileFalse As New KnowledgeUtils.IO.ManageFile
                        Mf.CloseFile()
                        FileFalse.DeleteFile(UserConfig.GetPhysicalPathTempFile & "UploadFalse.txt")
                        Dim Msg = "พบข้อผิดพลาดในการนำเข้าข้อมูล "
                        FileFalse.CreateFile(UserConfig.GetPhysicalPathTempFile & "UploadFalse.txt", Msg, False)
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Dim FileFalse As New KnowledgeUtils.IO.ManageFile
            Dim Msg = "พบข้อผิดพลาดในไฟล์รหัส " & CodeInFile & " อาจเกิดจากกรอกข้อมูลไม่ครบถ้วนนะคะ"
            FileFalse.CreateFile(UserConfig.GetPhysicalPathTempFile & "UploadFalse.txt", Msg, False)
            Return (False)
        Finally
            Mf.CloseFile()
            Mf.DeleteFile(File)
        End Try
    End Function

    Public Function InsertTempStudent(ByVal Items As IEnumerable(Of t360_tblTempStudent)) As Boolean Implements IImportManager.InsertTempStudent
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Ctx.t360_tblTempStudents.InsertAllOnSubmit(Items)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function DeleteTempStudent() As Boolean Implements IImportManager.DeleteTempStudent
        Try
            With GetLinqToSql
                Using Ctx = .GetDataContext()
                    .MainSql = "DELETE FROM t360_tblTempStudent"
                    .DataContextExecuteCommand(Ctx)
                End Using
            End With

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function InsertTempTeacher(ByVal Items As IEnumerable(Of t360_tblTempTeacher)) As Boolean Implements IImportManager.InsertTempTeacher
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Ctx.t360_tblTempTeachers.InsertAllOnSubmit(Items)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function DeleteTempTeacher() As Boolean Implements IImportManager.DeleteTempTeacher
        Try
            With GetLinqToSql
                Using Ctx = .GetDataContext()
                    .MainSql = "DELETE FROM t360_tblTempTeacher"
                    .DataContextExecuteCommand(Ctx)
                End Using
            End With

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function GetTempStudentAll() As t360_tblTempStudent() Implements IImportManager.GetTempStudentAll
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempStudents.ToArray
        End Using
    End Function

    Public Function GetTempStudentproblem() As t360_tblTempStudent() Implements IImportManager.GetTempStudentproblem
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempStudents.Where(Function(q) q.Record_Status <> EnRecordStatus.Import).ToArray
        End Using
    End Function

    Public Function GetTempStudentReadyImport() As t360_tblTempStudent() Implements IImportManager.GetTempStudentReadyImport
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempStudents.Where(Function(q) q.Record_Status = EnRecordStatus.Import).ToArray
        End Using
    End Function

    Public Function GetTempStudentByStudentCode(ByVal Student_Code As String) As t360_tblTempStudent Implements IImportManager.GetTempStudentByStudentCode
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempStudents.Where(Function(q) q.Student_Code = Student_Code).SingleOrDefault
        End Using
    End Function

    Public Function GetTempTeacherAll() As t360_tblTempTeacher() Implements IImportManager.GetTempTeacherAll
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempTeachers.ToArray
        End Using
    End Function

    Public Function GetTempTeacherByTeacherCode(ByVal Teacher_Code As String) As t360_tblTempTeacher Implements IImportManager.GetTempTeacherByTeacherCode
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTempTeachers.Where(Function(q) q.Teacher_Code = Teacher_Code).SingleOrDefault
        End Using
    End Function


End Class
