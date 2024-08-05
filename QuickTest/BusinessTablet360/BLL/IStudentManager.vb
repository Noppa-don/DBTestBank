Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils.System
Imports System.Web
Imports KnowledgeUtils
Imports System.Text

Public Interface IStudentManager

    't360_tblUpLevel เก็บการตั้งค่าการ เลื่อนชั้น ถ้า rowไหน isactive=1 หมายถึง เทอมนั้นเป็นเทอมล่าสุดของการปรับเลื่อนชั้นอีกความหนายหนึ่งคือเทอมปัจจุบันที่เรียนกันอยู่
    't360_tblUpLevelDetails เก็บรายละเอียดในการเลื่อนชั้น คนที่ทำการปรับไว้ isactive ส่งสัยไม่ได้ใช้ isconfirm หลังจาก เลื่อนชั้นแล้วจะปรับเป็น true ถ้าไม่เลื่อนเป็น false
    't360_tblUplevelConfirm เก็บห้องที่ได้ยืนยันการปรับเปลี่ยนเลื่อนชั้น isactive=1 หมายถึง ห้องนั้นถูก confirm แล้วหลังจากปรับการเลื่อนชั้นสำเร็จ isactive=0 หรือ กรณีที่มีการแก้คนใดคนหนึ่งในห้องนั้น isactive=0 เช่นกัน

    '<<< Student
    Function GetStudentByCrit(Of t)(ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t()
    Function GetStudentNextTerm(Of t)(ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t()
    Function GetStudentBySchoolCode(ByVal SchoolCode As String) As t360_tblStudent()
    Function GetStudentCountByCrit(Of t)(ByVal Item As StudentDTO) As Integer
    Function GetStudentByRoom(ByVal RoomName As String, ByVal SchoolCode As String) As t360_tblStudent()
    Function GetStudentDoPractice(ByVal SchoolCode As String, ByVal Calendar_Id As Guid, ByVal Subject_Id As System.Guid?) As Object()
    Function GetStudentDoHomework(ByVal SchoolCode As String, ByVal Calendar_Id As Guid, ByVal Subject_Id As System.Guid?) As Object()
    Function GetStudentDoQuiz(ByVal SchoolCode As String, ByVal Calendar_Id As Guid, ByVal Subject_Id As System.Guid?) As Object()
    Function GetRoomCountDoPratice(ByVal SchoolCode As String, ByVal Calendar_Id As Guid, ByVal Subject_Id As Guid?) As Object()
    Function GetStudentNotProcessUpLevelGetByCrit(ByVal Item As StudentDTO, ByVal SchoolCode As String) As StudentDTO()
    Function GetUpLevelConfirmBySchoolCode(SchoolCode As String) As t360_tblUplevelConfirm()
    Function GetIsConfirmAllBySchoolCodeAndCalendarYear(SchoolCode As String, CurrentId As Guid) As Boolean
    Function GetSetTypeRunStudentNumber(SchoolCode As String) As t360_tblSetTypeRunStudentNumber
    Function GetStudentUpLevelBySchoolCodeAndRoomId(SchoolCode As String, RoomId As Guid) As StudentDTO()
    Function GetStudentUpLevelForSearch(Item As StudentDTO) As StudentDTO()
    Function GetStudentForNewsDetail(SchoolCode As String, CurrentRoom As String, CurrentClass As String) As t360_tblStudent()
    Function GetUpLevel(ByVal SchoolCode As String) As t360_tblUpLevel
    Function GetAllUpLevel() As t360_tblUpLevel()
    Function GetUpLevelDetailBySchoolCodeAndRoomId(SchoolCode As String, RoomId As Guid) As StudentDTO()
    Function GetUpLevelDetailBySudentId(StudentId As Guid) As t360_tblUpLevelDetail
    Function GetSexByPrefix(PrefixName As String) As EnTypeSex
    Function GetRoomNotConfirm(SchoolCode As String, CalendarId As Guid) As ClassDto()
    Function IsProcessUpLevelByCalendarYear(SchoolCode As String, CalendarYear As String) As Boolean

    Function ValidateDuplicateStudent(ByVal Student_Code As String, Optional ByVal Student_Id As Guid? = Nothing) As Boolean
    Function ValidateDuplicateStudentNoInRoom(ByVal Student_Code As String, ClassName As String, RoomName As String, Optional ByVal Student_Id As Guid? = Nothing) As Boolean
    Function ValidateDuplicateStudentInUpLevel(SchoolCode As String, ByVal StudentNoInRoom As String, Optional ByVal Student_Id As Guid? = Nothing) As Boolean

    Function InsertStudent(ByVal ItemStudent As t360_tblStudent, ByVal ItemStudentRoom As t360_tblStudentRoom) As Boolean
    Function UpdateStudent(ByVal ItemStudent As t360_tblStudent, ByVal ItemStudentRoom As t360_tblStudentRoom, ByVal IsChangeRoom As Boolean, ByVal IsEdit As Boolean) As Boolean
    Function DeleteStudent(ByVal Item As t360_tblStudent) As Boolean
    Function UpdateUplevel(ByVal Items As t360_tblUpLevelDetail(), ByVal ItemDeletes As Guid(), ByVal SchoolCode As String) As Boolean
    Function InsertUpLevelConfirm(Item As t360_tblUplevelConfirm) As Boolean
    Function DeleteUpLevelConfirm(SchoolCode As String, RoomId As Guid, CalendarYear As String) As Boolean
    Function InsertSetTypeRunStudentNumber(Item As t360_tblSetTypeRunStudentNumber) As Boolean
    Function UpdateSetTypeRunStudentNumber(SchoolCode As String, Type As EnTypeStudentRunNo) As Boolean
    Function UpdateStudentNumberInUpdateLevelByStudentId(Items As StudentDTO()) As Boolean
    Function UpdateStudentInUpdateLevel(ByVal Items As t360_tblUpLevelDetail(), ByVal ItemDeletes As Guid(), ByVal SchoolCode As String) As Boolean
    Function DeleteStudentInUpdateLevel(StudentId As Guid, CurrentClassRoom As String) As Boolean
    Function SetConfirmUpLevelByRoomId(ByVal SchoolCode As String, CalendarYear As String, RoomId As Guid) As Boolean
    Function SetTimeUpLevel(ByVal ScheduleDate As DateTime, ByVal SchoolCode As String) As Boolean
    Function ProcessUpLevel(ByVal SchoolCode As String, TypeUpLevel As EnTypeRunUpLevel) As Boolean
    Function SetStudentNumberInUpdateLevelByRoomId(SchoolCode As String, RoomId As Guid, Optional IsUpLevel As Boolean = False) As Boolean

    Function GetCountStudentByroom(ByVal Class_Name As String, ByVal Room_Name As String, CalendarId As Guid) As Integer
    Function GetChartTableStatus(ByVal School_Code As String) As ChartTableStatus()

    'Function GetParentWithDevice(ByVal StudentId As Guid) As ParentDTO()

    Function InsertTabletDummy(ByVal Item As t360_tblTablet, ByVal Itemowner As t360_tblTabletOwner) As Boolean

End Interface

Public Class StudentManager
    Implements IStudentManager

#Region "Dependency"

    Private _ClassRepo As IClassRepo
    <Dependency()> Public Property ClassRepo() As IClassRepo
        Get
            Return _ClassRepo
        End Get
        Set(ByVal value As IClassRepo)
            _ClassRepo = value
        End Set
    End Property

    Private _StudentRepo As IStudentRepo
    <Dependency()> Public Property StudentRepo() As IStudentRepo
        Get
            Return _StudentRepo
        End Get
        Set(ByVal value As IStudentRepo)
            _StudentRepo = value
        End Set
    End Property

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

    Private _StudentRoomRepo As IStudentRoomRepo
    <Dependency()> Public Property StudentRoomRepo() As IStudentRoomRepo
        Get
            Return _StudentRoomRepo
        End Get
        Set(ByVal value As IStudentRoomRepo)
            _StudentRoomRepo = value
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

    Private _TabletManager As ITabletManager
    <Dependency()> Public Property TabletManager() As ITabletManager
        Get
            Return _TabletManager
        End Get
        Set(ByVal value As ITabletManager)
            _TabletManager = value
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

#End Region

#Region "Student"

    Public Function GetStudentByCrit(Of t)(ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t() Implements IStudentManager.GetStudentByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return StudentRepo.GetStudentByCrit(Of t)(Ctx, Item, Page, PageRow)
        End Using
    End Function


    Public Function GetStudentNextTerm(Of t)(Item As StudentDTO, Optional Page As Integer = 0, Optional PageRow As Integer = 50) As t() Implements IStudentManager.GetStudentNextTerm
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return StudentRepo.GetStudentNextTerm(Of t)(Ctx, Item, Page, PageRow)
        End Using
    End Function

    Public Function GetStudentBySchoolCode(SchoolCode As String) As t360_tblStudent() Implements IStudentManager.GetStudentBySchoolCode
        Using Ctx = GetLinqToSql.GetDataContext()
            Return (From q In Ctx.t360_tblStudents Where q.Student_IsActive = True And q.Student_Status = EnStudentStatus.Study And q.School_Code = SchoolCode).ToArray
        End Using
    End Function

    Public Function GetStudentCountByCrit(Of t)(ByVal Item As StudentDTO) As Integer Implements IStudentManager.GetStudentCountByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return StudentRepo.GetStudentByCrit(Of t)(Ctx, Item).Count
        End Using
    End Function

    Public Function GetStudentByRoom(ByVal RoomName As String, ByVal SchoolCode As String) As t360_tblStudent() Implements IStudentManager.GetStudentByRoom
        Using Ctx = GetLinqToSql.GetDataContext()
            Return (From q In Ctx.t360_tblStudents Where q.Student_CurrentClass & q.Student_CurrentRoom = RoomName And
                    q.Student_IsActive = True And q.Student_Status = EnStudentStatus.Study And q.School_Code = SchoolCode).ToArray
        End Using
    End Function

    Public Function GetCountStudentByroom(ByVal Class_Name As String, ByVal Room_Name As String, CalendarId As Guid) As Integer Implements IStudentManager.GetCountStudentByroom
        Using Ctx = GetLinqToSql.GetDataContext()
            Return StudentRepo.GetCountStudentByroom(Ctx, UserConfig.GetCurrentContext.School_Code, Class_Name, Room_Name, CalendarId)
        End Using
    End Function

    Public Function GetStudentDoHomework(ByVal SchoolCode As String, ByVal Calendar_Id As System.Guid, ByVal Subject_Id As System.Guid?) As Object() Implements IStudentManager.GetStudentDoHomework
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            If Subject_Id Is Nothing Then
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsHomeWorkMode = True _
                    Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                             StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            Else
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsHomeWorkMode = True And q.GroupSubject_Id = Subject_Id _
                   Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                            StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            End If
            Return r
        End Using
    End Function

    Public Function GetStudentDoPractice(ByVal SchoolCode As String, ByVal Calendar_Id As System.Guid, ByVal Subject_Id As System.Guid?) As Object() Implements IStudentManager.GetStudentDoPractice
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            If Subject_Id Is Nothing Then
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsPracticeMode = True _
                    Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                             StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            Else
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsPracticeMode = True And q.GroupSubject_Id = Subject_Id _
                   Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                            StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            End If
            Return r
        End Using
    End Function

    Public Function GetStudentDoQuiz(ByVal SchoolCode As String, ByVal Calendar_Id As System.Guid, ByVal Subject_Id As System.Guid?) As Object() Implements IStudentManager.GetStudentDoQuiz
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            If Subject_Id Is Nothing Then
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsQuizMode = True _
                    Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                             StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            Else
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsQuizMode = True And q.GroupSubject_Id = Subject_Id _
                   Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom, _
                            StudentFirstName = q.Student_FirstName, StudentLastName = q.Student_LastName, TotalScore = q.TotalScore Into Group).ToArray
            End If
            Return r
        End Using
    End Function

    Public Function GetRoomCountDoPratice(ByVal SchoolCode As String, ByVal Calendar_Id As System.Guid, ByVal Subject_Id As System.Guid?) As Object() Implements IStudentManager.GetRoomCountDoPratice
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            If Subject_Id Is Nothing Then
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.IsPracticeMode = True _
                    Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom Into Group, Count()).ToArray
            Else
                r = (From q In Ctx.uvw_StudentDoAllQuizs Where q.School_Code = SchoolCode And q.Calendar_Id = Calendar_Id And q.GroupSubject_Id = Subject_Id And q.IsPracticeMode = True _
                    Group By StudentCurrentClass = q.Student_CurrentClass, StudentCurrentRoom = q.Student_CurrentRoom Into Group, Count()).ToArray
            End If
            Return r
        End Using
    End Function

    Public Function GetChartTableStatus(ByVal School_Code As String) As ChartTableStatus() Implements IStudentManager.GetChartTableStatus
        ' Type 20 นักเรียนเข้าทาง tablet เปิด app
        Dim Tablets = TabletManager.GetStudentHaveTablet(New TabletOwnerDTO With {.School_Code = School_Code, .Tablet_IsActive = True, .Student_IsActive = True})

        Using Ctx = GetLinqToSql.GetDataContext
            Dim StudentToday = (From q In Ctx.tblLogs Join q2 In Ctx.t360_tblTabletOwners On q.UserId Equals q2.Owner_Id Where q2.TabletOwner_IsActive = True And q.LastUpdate = Today And q.LogType = 20 And q.School_Code = School_Code Select q2.Tablet_Id).Distinct()
            Dim StudentMore3Day = (From q In Ctx.tblLogs Join q2 In Ctx.t360_tblTabletOwners On q.UserId Equals q2.Owner_Id Where q2.TabletOwner_IsActive = True And q.LastUpdate < Today.AddDays(-3) And q.LogType = 20 And q.School_Code = School_Code Select q2.Tablet_Id).Distinct()
            Dim StudentLess3Day = (From q In Ctx.tblLogs Join q2 In Ctx.t360_tblTabletOwners On q.UserId Equals q2.Owner_Id Where q2.TabletOwner_IsActive = True And (q.LastUpdate >= Today.AddDays(-3) And q.LastUpdate < Today) And q.LogType = 20 And q.School_Code = School_Code Select q2.Tablet_Id).Distinct()
            Dim Dto As New List(Of ChartTableStatus)
            Dto.Add(New ChartTableStatus With {.Status = "วันนี้ใช้อยู่", .StatusNumber = StudentToday.Count})
            Dto.Add(New ChartTableStatus With {.Status = "พึ่งใช้ใน 3 วันนี้", .StatusNumber = StudentLess3Day.Count})
            Dim SumTab = StudentToday.Union(StudentLess3Day).Union(StudentMore3Day).ToArray
            Dim TabletNotLog = Tablets.Where(Function(q) Not SumTab.Contains(q.Tablet_Id)).ToArray

            Dto.Add(New ChartTableStatus With {.Status = "ไม่ใช้นานมาก", .StatusNumber = StudentMore3Day.Count + TabletNotLog.Count})
            If Tablets.Count > 0 Then
                Return Dto.ToArray
            Else
                Return Nothing
            End If
        End Using
    End Function

    Public Function GetStudentNotProcessUpLevelGetByCrit(ByVal Item As StudentDTO, ByVal SchoolCode As String) As StudentDTO() Implements IStudentManager.GetStudentNotProcessUpLevelGetByCrit
        With GetLinqToSql
            .MainSql = "SELECT  t360_tblStudent.Student_Id, t360_tblStudent.School_Code, t360_tblStudent.Student_Code, Student_PrefixName, Student_FirstName, Student_LastName, Student_NickName, Student_CurrentClass, " & _
                       "Student_CurrentRoom, Student_CurrentRoomId, t360_tblStudentRoom.Student_NoInRoom " & _
                       "FROM t360_tblStudent INNER JOIN t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id " & _
                       "WHERE t360_tblStudent.Student_Id NOT IN (select Student_Id from t360_tblUpLevelDetail where t360_tblUpLevelDetail.IsConfirm = 0 AND t360_tblUpLevelDetail.IsActive = 1 AND t360_tblUpLevelDetail.School_Code={Sch}) AND t360_tblStudentRoom.SR_IsActive=1 AND {F}"

            Dim f As New SqlPart
            With Item
                f.AddPart("t360_tblStudent.School_Code = {0}", .School_Code)
                f.AddPart("t360_tblStudent.Student_Code = {0}", .Student_Code)
                f.AddPart("t360_tblStudent.Student_FirstName LIKE {0}", .Student_FirstName.FusionText("%"))
                f.AddPart("t360_tblStudent.Student_LastName LIKE {0}", .Student_LastName.FusionText("%"))
                f.AddPart("t360_tblStudent.Student_CurrentClass = {0}", .Class_Name)
                f.AddPart("t360_tblStudent.Student_CurrentRoom = {0}", .Room_Name)
            End With
            .ApplySqlPart("F", f)
            .ApplyTagWithValue("Sch", UserConfig.GetCurrentContext.School_Code)

            Return .DataContextExecuteObjects(Of StudentDTO).ToArray
        End With
    End Function

    Public Function GetUpLevelConfirmBySchoolCode(SchoolCode As String) As t360_tblUplevelConfirm() Implements IStudentManager.GetUpLevelConfirmBySchoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblUplevelConfirms Where q.School_Code = SchoolCode And q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetIsConfirmAllBySchoolCodeAndCalendarYear(SchoolCode As String, CurrentId As Guid) As Boolean Implements IStudentManager.GetIsConfirmAllBySchoolCodeAndCalendarYear
        Using Ctx = GetLinqToSql.GetDataContext
            ' Dim AllRoom = SchoolManager.GetClassInStudent(SchoolCode).Select(Function(q) q.Room_Id).ToArray

            Dim AllRoom = (From s In Ctx.t360_tblStudents Join sr In Ctx.t360_tblStudentRooms On s.Student_CurrentRoomId Equals sr.Room_Id And s.Student_Id Equals sr.Student_Id
                             Where s.Student_IsActive = True And s.Student_Status = EnStudentStatus.Study And s.School_Code = SchoolCode And sr.SR_IsActive = True And sr.Calendar_Id = CurrentId
                             Select s.Student_CurrentRoomId).Distinct().ToArray
            Dim AllRoomConfirm = (From q In Ctx.t360_tblUplevelConfirms _
                                   Where q.School_Code = SchoolCode _
                                   And q.IsActive = True _
                                   Select q.Room_Id).ToArray

            Return (AllRoom.Count = AllRoomConfirm.Count)
        End Using
    End Function

    Public Function GetStudentUpLevelBySchoolCodeAndRoomId(SchoolCode As String, RoomId As Guid) As StudentDTO() Implements IStudentManager.GetStudentUpLevelBySchoolCodeAndRoomId
        With GetLinqToSql
            Dim Calendar = GetUpLevel(SchoolCode).Calendar_Id

            'Load จาก t360_tblUpLevelDetail
            .MainSql = "SELECT u.Student_Id, u.Student_Code, u.Student_PrefixName, u.Student_FirstName, u.Student_LastName, " & _
                       "u.Student_Status, u.Student_CurrentRoomId, 1 AS FromUpLevel, sr.Class_Name AS Student_CurrentClass, " & _
                       "sr.Room_Name AS Student_CurrentRoom, sr.Student_NoInRoom, r.Class_Name, r.Room_Name,u.Room_Id,u.Student_CurrentNoInRoomOld as Student_CurrentNoInRoom " & _
                       "FROM   t360_tblUpLevelDetail AS u LEFT JOIN t360_tblRoom as r On u.Room_Id = r.Room_Id " & _
                       "INNER JOIN t360_tblStudentRoom as sr On u.Student_CurrentRoomId = sr.Room_Id AND u.Student_Id = sr.Student_Id AND sr.SR_IsActive=1 " & _
                       "WHERE  u.Student_CurrentRoomId={CurrentRoom_Id} AND " & _
                       "u.School_Code ={School_Code} AND  " & _
                       "u.IsActive=1 AND u.IsConfirm=0 AND u.Student_Status is not null"
            .ApplyTagWithValue("CurrentRoom_Id", RoomId.ToString)
            .ApplyTagWithValue("School_Code", SchoolCode)
            Dim AllStudentInLevelUp = .DataContextExecuteObjects(Of StudentDTO).ToArray

            'แก้ให้โหลดเฉพาะ เทอมที่มาจาก calendarid ของ tblupdatelevel
            'Load จาก t360_tblStudent ที่ไม่ได้อยู่ใน t360_tblUpLevelDetail
            .MainSql = "SELECT  * " & _
                    "FROM  t360_tblStudent INNER JOIN t360_tblStudentRoom  On t360_tblStudent.Student_CurrentRoomId = t360_tblStudentRoom.Room_Id AND t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id AND t360_tblStudentRoom.SR_IsActive=1 " & _
                    "WHERE t360_tblStudent.Student_CurrentRoomId={Student_CurrentRoomId} AND " & _
                    "t360_tblStudentRoom.Calendar_Id={CalendarFromUpLevel} AND " & _
                    "t360_tblStudent.School_Code ={School_Code} AND t360_tblStudent.Student_Status={Student_Status} AND " & _
                    "t360_tblStudent.Student_IsActive=1 AND t360_tblStudent.Student_Id NOT IN ( " & _
                       "SELECT Student_Id " & _
                       "FROM   t360_tblUpLevelDetail " & _
                       "WHERE School_Code ={School_Code2} AND  " & _
                       "IsActive=1 AND IsConfirm=0)"

            .ApplyTagWithValue("Student_CurrentRoomId", RoomId.ToString)
            .ApplyTagWithValue("CalendarFromUpLevel", Calendar.ToString)
            .ApplyTagWithValue("School_Code", SchoolCode)
            .ApplyTagWithValue("School_Code2", SchoolCode)
            .ApplyTagWithValue("Student_Status", EnStudentStatus.Study)
            Dim AllStudent = .DataContextExecuteObjects(Of StudentDTO).ToArray

            Return AllStudentInLevelUp.Union(AllStudent).ToArray
        End With
    End Function

    Public Function GetStudentUpLevelForSearch(Item As StudentDTO) As StudentDTO() Implements IStudentManager.GetStudentUpLevelForSearch
        With GetLinqToSql
            .MainSql = "SELECT u.Student_Id, u.Student_Code, u.Student_FirstName, u.Student_LastName, " & _
                       "u.Student_Status, u.Student_CurrentRoomId, 1 AS FromUpLevel, " & _
                       "u.Room_Id " & _
                       "FROM   t360_tblUpLevelDetail AS u INNER JOIN t360_tblRoom AS r ON u.Student_CurrentRoomId = r.Room_Id AND r.School_Code={sch} AND r.Room_IsActive=1 " & _
                       "WHERE u.IsActive=1 AND u.IsConfirm=0 AND {f}"

            Dim f As New SqlPart
            With Item
                f.AddPart("u.Student_FirstName LIKE {0}", .Student_FirstName.FusionText("%"))
                f.AddPart("u.Student_LastName LIKE {0}", .Student_LastName.FusionText("%"))
                f.AddPart("u.Student_Code = {0}", .Student_Code)
                f.AddPart("u.School_Code = {0}", .School_Code)
                f.AddPart("r.Class_Name = {0}", .Class_Name)
                f.AddPart("r.Room_Name = {0}", .Room_Name)
            End With
            .ApplySqlPart("f", f)
            .ApplyTagWithValue("sch", Item.School_Code)

            Dim AllStudentInLevelUp = .DataContextExecuteObjects(Of StudentDTO).ToArray


            .MainSql = "SELECT  * " & _
                    "FROM  t360_tblStudent " & _
                    "WHERE t360_tblStudent.Student_IsActive=1 AND Student_Status=1 AND t360_tblStudent.Student_Id NOT IN ( " & _
                       "SELECT Student_Id " & _
                       "FROM   t360_tblUpLevelDetail " & _
                       "WHERE School_Code ={School_Code2} AND  " & _
                       "IsActive=1 AND IsConfirm=0) AND {f1}"

            Dim f1 As New SqlPart
            With Item
                f1.AddPart("Student_FirstName LIKE {0}", .Student_FirstName.FusionText("%"))
                f1.AddPart("Student_LastName LIKE {0}", .Student_LastName.FusionText("%"))
                f1.AddPart("School_Code = {0}", .School_Code)
                f1.AddPart("Student_Code = {0}", .Student_Code)
                f1.AddPart("Student_CurrentClass = {0}", .Class_Name)
                f1.AddPart("Student_CurrentRoom = {0}", .Room_Name)
            End With
            .ApplySqlPart("f1", f1)
            .ApplyTagWithValue("School_Code2", Item.School_Code)
            .ApplyTagWithValue("Student_Status", 1)

            Dim AllStudent = .DataContextExecuteObjects(Of StudentDTO).ToArray

            Return AllStudentInLevelUp.Union(AllStudent).ToArray
        End With
    End Function
    Public Function GetStudentForNewsDetail(SchoolCode As String, CurrentRoom As String, CurrentClass As String) As t360_tblStudent() Implements IStudentManager.GetStudentForNewsDetail
        Using Ctx = GetLinqToSql.GetDataContext()
            Return (From q In Ctx.t360_tblStudents Where q.Student_IsActive = True And q.Student_Status = EnStudentStatus.Study And q.School_Code = SchoolCode _
                    And q.Student_CurrentClass = CurrentClass And q.Student_CurrentRoom = CurrentRoom).ToArray
        End Using
    End Function

    Public Function GetSetTypeRunStudentNumber(SchoolCode As String) As t360_tblSetTypeRunStudentNumber Implements IStudentManager.GetSetTypeRunStudentNumber
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Data = (From q In Ctx.t360_tblSetTypeRunStudentNumbers Where q.School_Code = SchoolCode).SingleOrDefault
            If Data Is Nothing Then
                Dim Item As New t360_tblSetTypeRunStudentNumber
                Item.Syr_Id = Guid.NewGuid
                Item.School_Code = SchoolCode
                Item.Syr_Type = EnTypeStudentRunNo.Code
                InsertSetTypeRunStudentNumber(Item)
                Data = (From q In Ctx.t360_tblSetTypeRunStudentNumbers Where q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
            End If
            Return Data
        End Using
    End Function

    Public Function GetUpLevel(SchoolCode As String) As t360_tblUpLevel Implements IStudentManager.GetUpLevel
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblUpLevels Where q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetAllUpLevel() As t360_tblUpLevel() Implements IStudentManager.GetAllUpLevel
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblUpLevels Where q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetUpLevelDetailBySchoolCodeAndRoomId(SchoolCode As String, RoomId As Guid) As StudentDTO() Implements IStudentManager.GetUpLevelDetailBySchoolCodeAndRoomId
        Using Ctx = GetLinqToSql.GetDataContext
            Dim OldStudent = (From upd In Ctx.t360_tblUpLevelDetails Join room In Ctx.t360_tblRooms On upd.Student_CurrentRoomId Equals room.Room_Id _
                    Where upd.School_Code = SchoolCode And upd.Room_Id = RoomId And upd.IsConfirm = False And upd.IsActive = True _
                    Select New StudentDTO With {.Student_Code = upd.Student_Code, _
                                                .Student_FirstName = upd.Student_FirstName, _
                                                .Student_LastName = upd.Student_LastName, _
                                                .Student_Id = upd.Student_Id, _
                                                .Student_No = upd.Student_CurrentNoInRoom, _
                                                .Student_CurrentClass = room.Class_Name, _
                                                .Student_CurrentRoom = room.Room_Name}).ToArray
            'นักเรียนที่เพิ่มเข้ามาในระหว่างการทำการเลื่อนชั้น
            Dim NewStudent = (From upd In Ctx.t360_tblUpLevelDetails _
                    Where upd.School_Code = SchoolCode And upd.Room_Id = RoomId And upd.IsConfirm = False And upd.IsActive = True And upd.Student_CurrentRoomId Is Nothing _
                    Select New StudentDTO With {.Student_Code = upd.Student_Code, _
                                                .Student_FirstName = upd.Student_FirstName, _
                                                .Student_LastName = upd.Student_LastName, _
                                                .Student_Id = upd.Student_Id, _
                                                .Student_No = upd.Student_CurrentNoInRoom}).ToArray

            Return OldStudent.Union(NewStudent).ToArray
        End Using
    End Function

    Public Function GetUpLevelDetailBySudentId(StudentId As Guid) As t360_tblUpLevelDetail Implements IStudentManager.GetUpLevelDetailBySudentId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From upd In Ctx.t360_tblUpLevelDetails _
                    Where upd.Student_Id = StudentId And upd.IsActive = True And upd.IsConfirm = False).SingleOrDefault
        End Using
    End Function

    Public Function GetSexByPrefix(PrefixName As String) As EnTypeSex Implements IStudentManager.GetSexByPrefix
        Select Case PrefixName
            Case "ด.ช.", "นาย"
                Return EnTypeSex.Male
            Case "ด.ญ.", "นาง", "นางสาว"
                Return EnTypeSex.Female
            Case Else
                Return EnTypeSex.None
        End Select
    End Function

    Public Function GetRoomNotConfirm(SchoolCode As String, CalendarId As Guid) As ClassDto() Implements IStudentManager.GetRoomNotConfirm
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Confirm = (From q In Ctx.t360_tblUplevelConfirms Where q.School_Code = SchoolCode And q.IsActive = True).Select(Function(q) q.Room_Id).ToList
            Dim AllRoom = SchoolManager.GetClassInStudentBySchoolCodeAndCurrentId(SchoolCode, CalendarId)
            Dim RoomNotConfirm = AllRoom.Where(Function(q) Not Confirm.Contains(q.Room_Id))

            Return RoomNotConfirm.ToArray
        End Using
    End Function

    Public Function IsProcessUpLevelByCalendarYear(SchoolCode As String, CalendarYear As String) As Boolean Implements IStudentManager.IsProcessUpLevelByCalendarYear
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r = (From q In Ctx.t360_tblUpLevels Where q.Calendar_Year = CalendarYear And q.Rundate IsNot Nothing And q.School_Code = SchoolCode).SingleOrDefault
            Return (r IsNot Nothing)
        End Using
    End Function

    Public Function InsertStudent(ByVal ItemStudent As t360_tblStudent, ByVal ItemStudentRoom As t360_tblStudentRoom) As Boolean Implements IStudentManager.InsertStudent
        'จะทำการ Insert tblSudentRoom ด้วย
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            ItemStudent.Student_Id = Guid.NewGuid
            ItemStudent.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemStudent.Student_IsActive = True
            ItemStudent.LastUpdate = GetTime

            StudentRepo.InsertStudent(Ctx, ItemStudent)

            ItemStudentRoom.Student_Id = ItemStudent.Student_Id
            ItemStudentRoom.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemStudentRoom.LastUpdate = GetTime
            StudentRoomRepo.InsertStudentRoom(Ctx, ItemStudentRoom)

            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim Logdetail As New StringBuilder
            Logdetail.Append("นักเรียน-ชื่อสกุล: ")
            Logdetail.Append(ItemStudent.Student_FirstName & " " & ItemStudent.Student_LastName)
            Logdetail.Append(" -ห้อง: ")
            Logdetail.Append(ItemStudent.Student_CurrentClass & ItemStudent.Student_CurrentRoom)
            Logdetail.Append(",t360_tblStudent.Id=" & ItemStudent.Student_Id.ToString)
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = Logdetail.ToString
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateStudent(ByVal ItemStudent As t360_tblStudent, ByVal ItemStudentRoom As t360_tblStudentRoom,
                                  ByVal IsChangeRoom As Boolean, ByVal IsEdit As Boolean) As Boolean Implements IStudentManager.UpdateStudent
        'IsEdit หมายถึง แก้ห้องผิดไม่ใช่ ย้ายห้อง
        'ถ้ามีการแก้ไขห้องเรียนใหม่จะเป็นการ Insert เข้าที่ t360_tblStudentRoom
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            ItemStudent.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemStudent.LastUpdate = GetTime
            ItemStudent.ClientId = Nothing
            ItemStudentRoom.School_Code = UserConfig.GetCurrentContext.School_Code

            '<<< Student
            StudentRepo.UpdateStudent(Ctx, ItemStudent)

            '<<< StudentRoom
            If IsChangeRoom Then
                Dim Ori = Ctx.t360_tblStudentRooms.Where(Function(q) q.Student_Id = ItemStudent.Student_Id And q.SR_IsActive = True).SingleOrDefault
                If IsEdit Then
                    'เป็นการแก้ผิด
                    Ori.Class_Name = ItemStudent.Student_CurrentClass
                    Ori.Room_Name = ItemStudent.Student_CurrentRoom
                    Ori.Room_Id = ItemStudent.Student_CurrentRoomId
                    Ori.Student_NoInRoom = ItemStudent.Student_CurrentNoInRoom
                    Ori.LastUpdate = GetTime
                    Ori.ClientId = Nothing
                    Ctx.SubmitChanges()
                Else
                    'ถ้าเป็นการย้ายห้องปรับ isactive ก็พอ
                    Ori.SR_IsActive = False
                    Ori.LastUpdate = GetTime
                    Ori.ClientId = Nothing
                    'ให้เอา Calendar_Id จาก row เก่า
                    ItemStudentRoom.Calendar_Id = Ori.Calendar_Id
                    ItemStudentRoom.Student_Id = ItemStudent.Student_Id
                    ItemStudentRoom.LastUpdate = GetTime
                    ItemStudentRoom.Student_NoInRoom = ItemStudent.Student_CurrentNoInRoom
                    StudentRoomRepo.InsertStudentRoom(Ctx, ItemStudentRoom)
                End If
            Else
                Dim Ori = Ctx.t360_tblStudentRooms.Where(Function(q) q.Student_Id = ItemStudent.Student_Id And q.SR_IsActive = True).SingleOrDefault
                Ori.Student_NoInRoom = ItemStudent.Student_CurrentNoInRoom
                Ctx.SubmitChanges()
            End If

            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("นักเรียน -ชื่อสกุล: ")
            LogDetail.Append(ItemStudent.Student_FirstName & " " & ItemStudent.Student_LastName)
            LogDetail.Append(" -ห้อง: ")
            LogDetail.Append(ItemStudent.Student_CurrentClass & ItemStudent.Student_CurrentRoom)
            LogDetail.Append(",t360_tblStudent.Id=")
            LogDetail.Append(ItemStudent.Student_Id)
            UserManager.Log(EnLogType.ImportantUpdate, LogDetail.ToString)
        End Try
    End Function

    Public Function DeleteStudent(ByVal Item As t360_tblStudent) As Boolean Implements IStudentManager.DeleteStudent
        Dim DeleteData As String = ""
        Try
            Using ctx = GetLinqToSql.GetDataContext()
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate

                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Dim Target = (From r In ctx.t360_tblStudents Where r.Student_Id = Item.Student_Id And r.School_Code = Item.School_Code And r.Student_IsActive = True).SingleOrDefault
                DeleteData = Target.Student_FirstName & " " & Target.Student_LastName & " ห้อง " & Target.Student_CurrentClass & "/" & Target.Student_CurrentRoom & ", t360_tblStudent.Id=" & Target.Student_Id.ToString
                Target.Student_IsActive = EnIsActiveStatus.Delete
                Target.LastUpdate = GetTime
                Target.ClientId = Nothing

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    StudentRepo.DeleteStudentWithCheckmark(ctx, Target)
                End If

                ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.ImportantDelete
                .Log_Page = "StudentManagerPage.aspx"
                .Log_Description = "นักเรียน - ชื่อสกุล:" & DeleteData
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateStudent(ByVal Student_Code As String, Optional ByVal Student_id As Guid? = Nothing) As Boolean Implements IStudentManager.ValidateDuplicateStudent
        Using ctx = GetLinqToSql.GetDataContext()
            Dim School_Code = UserConfig.GetCurrentContext.School_Code
            If Student_id Is Nothing Then
                Return StudentRepo.ValidateDuplicateStudent(ctx, Student_Code, School_Code)
            Else
                Return StudentRepo.ValidateDuplicateStudent(ctx, Student_Code, School_Code, Student_id)
            End If
        End Using
    End Function

    Public Function ValidateDuplicateStudentNoInRoom(StudentNoInRoom As String, ClassName As String, RoomName As String, Optional Student_Id As Guid? = Nothing) As Boolean Implements IStudentManager.ValidateDuplicateStudentNoInRoom
        Using ctx = GetLinqToSql.GetDataContext()
            Dim School_Code = UserConfig.GetCurrentContext.School_Code
            If Student_Id Is Nothing Then
                Return StudentRepo.ValidateDuplicateStudentNoInRoom(ctx, StudentNoInRoom, School_Code, ClassName, RoomName)
            Else
                Return StudentRepo.ValidateDuplicateStudentNoInRoom(ctx, StudentNoInRoom, School_Code, ClassName, RoomName, Student_Id)
            End If
        End Using
    End Function

    Public Function ValidateDuplicateStudentInUpLevel(SchoolCode As String, Student_Code As String, Optional Student_Id As Guid? = Nothing) As Boolean Implements IStudentManager.ValidateDuplicateStudentInUpLevel
        Using ctx = GetLinqToSql.GetDataContext()
            If Student_Id Is Nothing Then
                Dim r = (From q In ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Student_Code = Student_Code).SingleOrDefault
                Return (r Is Nothing)
            Else
                Dim r = (From q In ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Student_Code = Student_Code And q.Student_Id <> Student_Id).SingleOrDefault
                Return (r Is Nothing)
            End If
        End Using
    End Function

    Public Function UpdateUplevel(ByVal Items As t360_tblUpLevelDetail(), ByVal ItemDeletes As Guid(), ByVal SchoolCode As String) As Boolean Implements IStudentManager.UpdateUplevel
        'ปรับปรุงข้อมูล t360_tblUpLevel , t360_tblUpLevelDetails
        'ตอนลบ t360_tblUpLevelDetails ลบข้อมูล คนที่IsConfirm = false
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Dim IsNewUpdateLevel = (Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = SchoolCode).SingleOrDefault Is Nothing)
            Dim UpLevelId As Guid
            UpLevelId = Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = SchoolCode).SingleOrDefault.UpLevel_Id
            'If IsNewUpdateLevel Then
            '    Dim DataUpdateLevel As New t360_tblUpLevel
            '    With DataUpdateLevel
            '        .School_Code = SchoolCode
            '        .UpLevel_Id = Guid.NewGuid
            '        .IsActive = True
            '        .LastUpdate = GetTime
            '    End With
            '    Ctx.t360_tblUpLevels.InsertOnSubmit(DataUpdateLevel)
            '    UpLevelId = DataUpdateLevel.UpLevel_Id
            'Else
            '    UpLevelId = Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = SchoolCode).SingleOrDefault.UpLevel_Id
            'End If

            For Each Detail In ItemDeletes
                Dim Data = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.Student_Id = Detail And q.IsConfirm = False).SingleOrDefault
                Data.IsActive = False
                Data.LastUpdate = GetTime
                Data.ClientId = Nothing
            Next

            For Each Detail In Items
                Dim Target As New t360_tblUpLevelDetail
                Target = (From r In Ctx.t360_tblUpLevelDetails Where r.School_Code = SchoolCode And r.Student_Id = Detail.Student_Id And r.IsConfirm = False And r.IsActive = True).SingleOrDefault
                If Target Is Nothing Then
                    Detail.ULD_Id = Guid.NewGuid
                    Detail.UpLevel_Id = UpLevelId
                    Detail.IsConfirm = False
                    Detail.IsActive = True
                    Detail.LastUpdate = GetTime
                    Detail.School_Code = SchoolCode
                    Ctx.t360_tblUpLevelDetails.InsertOnSubmit(Detail)
                Else
                    Target.Student_Status = Detail.Student_Status
                    Target.Room_Id = Detail.Room_Id
                    Target.LastUpdate = GetTime
                    Target.ClientId = Nothing
                End If
            Next

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function UpdateStudentInUpdateLevel(Items() As t360_tblUpLevelDetail, ItemDeletes() As Guid, SchoolCode As String) As Boolean Implements IStudentManager.UpdateStudentInUpdateLevel
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()

            For Each Detail In ItemDeletes
                Dim Data = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.Student_Id = Detail And q.IsConfirm = False).SingleOrDefault
                Data.IsActive = False
                Data.LastUpdate = GetTime
                Data.ClientId = Nothing
            Next

            For Each Detail In Items
                Dim Target As New t360_tblUpLevelDetail
                Using Ctx1 = GetLinqToSql.GetDataContext()
                    Target = (From r In Ctx1.t360_tblUpLevelDetails Where r.School_Code = SchoolCode And r.Student_Id = Detail.Student_Id And r.IsConfirm = False And r.IsActive = True).SingleOrDefault
                End Using
                Detail.ClientId = Nothing
                Detail.LastUpdate = Now
                Ctx.t360_tblUpLevelDetails.Attach(Detail, Target)
            Next

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function DeleteStudentInUpdateLevel(StudentId As Guid, CurrentClassRoom As String) As Boolean Implements IStudentManager.DeleteStudentInUpdateLevel
        Dim LogDetail As New StringBuilder
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.Student_Id = StudentId).SingleOrDefault
                Data.IsActive = False
                Data.LastUpdate = GetTime
                Data.ClientId = Nothing
                Dim StudentDetail = GetUpLevelDetailBySudentId(StudentId)
                LogDetail.Append("นักเรียน -ชื่อสกุล: ")
                LogDetail.Append(StudentDetail.Student_FirstName & " " & StudentDetail.Student_LastName)
                LogDetail.Append(" -ห้อง: ")
                LogDetail.Append(CurrentClassRoom)
                LogDetail.Append(" ,t360_tblStudent.Id=")
                LogDetail.Append(StudentDetail.Student_Id.ToString)

                Ctx.SubmitChanges()
            End Using

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            UserManager.Log(EnLogType.Delete, LogDetail.ToString)
        End Try
    End Function

    Public Function SetConfirmUpLevelByRoomId(SchoolCode As String, CalendarYear As String, RoomId As Guid) As Boolean Implements IStudentManager.SetConfirmUpLevelByRoomId
        Dim Factory = GetLinqToSql
        Try
            Dim Ctx = Factory.GetDataContextWithTransaction()
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            'บันทึก Confirm
            Dim DataConfirm = (From q In Ctx.t360_tblUplevelConfirms Where q.School_Code = SchoolCode And q.Room_Id = RoomId And q.IsActive = True And q.Calendar_Year = CalendarYear).SingleOrDefault
            If DataConfirm Is Nothing Then
                Dim Item As New t360_tblUplevelConfirm
                With Item
                    .Calendar_Year = CalendarYear
                    .IsActive = True
                    .Room_Id = RoomId
                    .School_Code = SchoolCode
                    .Ulv_Id = Guid.NewGuid
                    .LastUpdate = GetTime
                End With
                Ctx.t360_tblUplevelConfirms.InsertOnSubmit(Item)
            End If

            'เช็คว่ามีนักเรียนที่อยู่เทอมหน้าแล้วไม่ได้อยู่ในเทอมที่อยู่ใน tblupdatedetail 
            'ถ้ามีนำนักเรียนคนนี้เข้าที  tblupdatedetail ด้วย (เผื่อที่จะได้มองเห็นที่ห้องอนาคตและจัดเลขที่ให้เค้าได้ด้วย)
            Dim DataUpLevel = Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = SchoolCode).SingleOrDefault
            Dim UpLevelId = DataUpLevel.UpLevel_Id
            Dim CalendarNextTerm = SchoolManager.GetNextCalendar(DataUpLevel.Calendar_Id)
            Dim StudentIdNextTermInDatabase = (From s In Ctx.t360_tblStudents Join sr In Ctx.t360_tblStudentRooms On s.Student_Id Equals sr.Student_Id
                                               Where sr.SR_IsActive = True And sr.Calendar_Id = CalendarNextTerm.Calendar_Id And s.Student_Status = EnStudentStatus.Study
                                               Select s.Student_Id).ToArray
            Dim StudentInUpLevelDetails = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.IsConfirm = False And q.IsActive = True And q.School_Code = SchoolCode).Select(Function(q) New Guid(q.Student_Id.ToString)).ToArray
            Dim StudentMustAdd = StudentIdNextTermInDatabase.Except(StudentInUpLevelDetails).ToArray

            For Each Row In StudentMustAdd
                Dim Student = Ctx.t360_tblStudents.Where(Function(q) q.Student_Id = Row).SingleOrDefault

                Dim NewUpLevelDetail As New t360_tblUpLevelDetail
                With NewUpLevelDetail
                    .ULD_Id = Guid.NewGuid
                    .UpLevel_Id = UpLevelId
                    .IsConfirm = False
                    .IsActive = True
                    '.LastUpdate = GetTime
                    .School_Code = SchoolCode

                    .Student_Id = Student.Student_Id
                    .Room_Id = Student.Student_CurrentRoomId 'ห้องใหม่
                    .Student_Code = Student.Student_Code
                    .Student_FirstName = Student.Student_FirstName
                    .Student_LastName = Student.Student_LastName
                    .Student_PrefixName = Student.Student_PrefixName
                    .Student_CurrentRoomId = Student.Student_CurrentRoomId
                    .sex = GetSexByPrefix(Student.Student_PrefixName)
                    .Student_CurrentNoInRoomOld = Student.Student_CurrentNoInRoom
                    .Student_CurrentNoInRoom = Student.Student_CurrentNoInRoom
                    Ctx.t360_tblUpLevelDetails.InsertOnSubmit(NewUpLevelDetail)
                End With
            Next

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()

            'ถ้า confirm ครบ จะทำการปรับเลขที่ใหม่ให้
            If GetIsConfirmAllBySchoolCodeAndCalendarYear(SchoolCode, DataUpLevel.Calendar_Id) Then
                Dim Result As Boolean = True
                Dim RoomInUpLevel = SchoolManager.GetRoomInUpLevel(SchoolCode)
                For Each I In RoomInUpLevel
                    Dim R1 = SetStudentNumberInUpdateLevelByRoomId(SchoolCode, I.Room_Id, True)
                    Result = Result And R1
                Next
                If Not Result Then
                    Throw New Exception("มีปัญหาตอนปรับเลขที่คะ")
                End If
            End If

            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function SetTimeUpLevel(ByVal ScheduleDate As Date, ByVal SchoolCode As String) As Boolean Implements IStudentManager.SetTimeUpLevel
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = Ctx.t360_tblUpLevels.Where(Function(q) q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
                Data.ScheduleDate = ScheduleDate
                Data.LastUpdate = GetTime
                Data.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using

            HttpContext.Current.Application("DateSchedulerUplevel") = ScheduleDate

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function ProcessUpLevel(ByVal SchoolCode As String, TypeUpLevel As EnTypeRunUpLevel) As Boolean Implements IStudentManager.ProcessUpLevel
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Dim Ctx = Factory.GetDataContextWithTransaction()

            'เลื่อนชั้น
            Dim DataUpLevel = Ctx.t360_tblUpLevels.Where(Function(q) q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
            Dim CurrentYearId = DataUpLevel.Calendar_Id
            Dim DataNextYear = SchoolManager.GetNextCalendar(CurrentYearId)
            Dim NextYearId = DataNextYear.Calendar_Id
            Dim NextYear = CType(DataNextYear.Calendar_Year, Short)
            Dim sqlCheckmark As New StringBuilder() ' sql สำหรับการ insert update delete ที่ db checkmark
            Dim clsCheckmark As New ClsCheckMark()
            Dim AllStudent = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.School_Code = SchoolCode And (q.Student_Status = EnStudentStatus.Study Or q.Student_Status Is Nothing) And q.IsActive = True And q.IsConfirm = False).ToList
            For Each Detail In AllStudent
                'เด็กปกติ
                Dim Student = (From q In Ctx.t360_tblStudents Where q.Student_Id = Detail.Student_Id).SingleOrDefault
                If Student IsNot Nothing Then
                    Dim RoomNew = (From q In Ctx.t360_tblRooms Where q.Room_Id = Detail.Room_Id).SingleOrDefault

                    If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                        Dim studentOld As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Student) ' เก้บชั้นห้องนักเรียนเก่า
                        'เด็กเก่า มีทั้งแบบ อาจโดนเพิ่มห้องที่เทอมหน้ามาก่อนแล้วด้วย กับ แบบที่ยัง
                        Student.Student_CurrentClass = RoomNew.Class_Name
                        Student.Student_CurrentRoom = RoomNew.Room_Name
                        Student.Student_CurrentRoomId = Detail.Room_Id
                        Student.Student_CurrentNoInRoom = Detail.Student_CurrentNoInRoom
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                        Dim studentNew As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Student) ' เก้บชั้นห้องนักเรียนใหม่
                        sqlCheckmark.Append(studentNew.ToStringSQLUpdateCheckmark(studentOld))
                    Else
                        'เด็กเก่า มีทั้งแบบ อาจโดนเพิ่มห้องที่เทอมหน้ามาก่อนแล้วด้วย กับ แบบที่ยัง
                        Student.Student_CurrentClass = RoomNew.Class_Name
                        Student.Student_CurrentRoom = RoomNew.Room_Name
                        Student.Student_CurrentRoomId = Detail.Room_Id
                        Student.Student_CurrentNoInRoom = Detail.Student_CurrentNoInRoom
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                    End If

                    Dim StudentRoom = (From q In Ctx.t360_tblStudentRooms Where q.Student_Id = Detail.Student_Id And q.SR_IsActive = True).SingleOrDefault
                    If StudentRoom.Calendar_Id <> NextYearId Then
                        'เคสคนทีมีห้องของเทอมหน้าแล้วเราจะไม่เพิ่มห้องใหม่ให้ และ ไม่ปรับของเก่าทิ้ง
                        StudentRoom.SR_IsActive = False
                        StudentRoom.LastUpdate = GetTime
                        StudentRoom.ClientId = Nothing

                        Dim NewStudentRoom As New t360_tblStudentRoom
                        With NewStudentRoom
                            .Calendar_Id = NextYearId
                            .Room_Id = Detail.Room_Id
                            .Class_Name = RoomNew.Class_Name
                            .Room_Name = RoomNew.Room_Name
                            .School_Code = SchoolCode
                            .SR_Id = Guid.NewGuid
                            .SR_IsActive = True
                            .Student_Id = Detail.Student_Id
                            .SR_MoveDate = GetTime
                            .Student_NoInRoom = Detail.Student_CurrentNoInRoom
                            .SR_AcademicYear = NextYear
                            .LastUpdate = GetTime
                        End With
                        Ctx.t360_tblStudentRooms.InsertOnSubmit(NewStudentRoom)
                    Else
                        StudentRoom.Student_NoInRoom = Detail.Student_CurrentNoInRoom
                        StudentRoom.LastUpdate = GetTime
                        StudentRoom.ClientId = Nothing
                    End If
                Else
                    'เด็กเพิ่มเอง
                    Dim RoomNew = (From q In Ctx.t360_tblRooms Where q.Room_Id = Detail.Room_Id).SingleOrDefault
                    Dim NewStudent = Detail.ToType(Of t360_tblStudent)()
                    With NewStudent
                        .Student_CurrentRoomId = Detail.Room_Id
                        .Student_CurrentClass = RoomNew.Class_Name
                        .Student_CurrentRoom = RoomNew.Room_Name
                        .Student_IsActive = True
                        .LastUpdate = GetTime
                    End With

                    If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                        Dim studentNew As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, NewStudent)
                        sqlCheckmark.Append(studentNew.ToStringSQLInsertCheckmark())
                    End If

                    Dim NewStudentRoom As New t360_tblStudentRoom
                    With NewStudentRoom
                        .Calendar_Id = NextYearId
                        .Room_Id = Detail.Room_Id
                        .Class_Name = RoomNew.Class_Name
                        .Room_Name = RoomNew.Room_Name
                        .School_Code = SchoolCode
                        .SR_Id = Guid.NewGuid
                        .SR_IsActive = True
                        .SR_MoveDate = Now
                        .Student_NoInRoom = NewStudent.Student_CurrentNoInRoom
                        .SR_AcademicYear = NextYear
                        .LastUpdate = GetTime
                    End With
                    NewStudent.t360_tblStudentRooms.Add(NewStudentRoom)
                    Ctx.t360_tblStudents.InsertOnSubmit(NewStudent)
                End If
            Next

            'จบการศึกษา
            AllStudent = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.School_Code = SchoolCode And q.Student_Status = EnStudentStatus.Graduating And q.IsActive = True And q.IsConfirm = False).ToList
            For Each Detail In AllStudent
                Dim Student = (From q In Ctx.t360_tblStudents Where q.Student_Id = Detail.Student_Id).SingleOrDefault
                Student.Student_Status = EnStudentStatus.Graduating
                Student.LastUpdate = GetTime
                Student.ClientId = Nothing

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Dim tempSqlCheckmark As String = sqlCheckmark.ToString()
                    sqlCheckmark.Clear()
                    sqlCheckmark.Append(GetSQLCheckmarkSpecialCase(tempSqlCheckmark, clsCheckmark, Student))
                End If


                Dim NewSchoolRoom As New t360_tblStudentFinish
                With NewSchoolRoom
                    Dim Room = SchoolManager.GetRoomByRoomId(Detail.Student_CurrentRoomId)
                    .Class_Name = Room.Class_Name
                    .School_Code = Detail.School_Code
                    .Student_Id = Detail.Student_Id
                    .LastUpdate = GetTime
                    .IsActive = True
                End With
                Ctx.t360_tblStudentFinishes.InsertOnSubmit(NewSchoolRoom)
            Next

            'ลาออก
            AllStudent = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.School_Code = SchoolCode And q.Student_Status = EnStudentStatus.Resign And q.IsActive = True And q.IsConfirm = False).ToList
            For Each Detail In AllStudent
                Dim Student = (From q In Ctx.t360_tblStudents Where q.Student_Id = Detail.Student_Id).SingleOrDefault
                Student.Student_Status = EnStudentStatus.Resign
                Student.LastUpdate = GetTime
                Student.ClientId = Nothing

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Dim tempSqlCheckmark As String = sqlCheckmark.ToString()
                    sqlCheckmark.Clear()
                    sqlCheckmark.Append(GetSQLCheckmarkSpecialCase(tempSqlCheckmark, clsCheckmark, Student))
                End If

            Next

            'พักการศึกษา
            AllStudent = Ctx.t360_tblUpLevelDetails.Where(Function(q) q.School_Code = SchoolCode And q.Student_Status = EnStudentStatus.RestStudy And q.IsActive = True And q.IsConfirm = False).ToList
            For Each Detail In AllStudent
                Dim Student = (From q In Ctx.t360_tblStudents Where q.Student_Id = Detail.Student_Id).SingleOrDefault
                Student.Student_Status = EnStudentStatus.RestStudy
                Student.LastUpdate = GetTime
                Student.ClientId = Nothing

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Dim tempSqlCheckmark As String = sqlCheckmark.ToString()
                    sqlCheckmark.Clear()
                    sqlCheckmark.Append(GetSQLCheckmarkSpecialCase(tempSqlCheckmark, clsCheckmark, Student))
                End If

            Next

            'ปรับ level ปรับ t360_tblUpLevel ให้ IsActive = false
            Dim UpLevel = (From q In Ctx.t360_tblUpLevels Where q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
            UpLevel.IsActive = False
            UpLevel.Rundate = Now
            UpLevel.UpLevel_Type = TypeUpLevel
            UpLevel.LastUpdate = GetTime
            UpLevel.ClientId = Nothing
            'เพิ่ม Row สำหรับเทอมใหม่เข้าไปด้วย
            Dim NewUpLevel As New t360_tblUpLevel
            With NewUpLevel
                .Calendar_Year = NextYear
                .Calendar_Id = NextYearId
                .ClientId = Nothing
                .IsActive = True
                .LastUpdate = GetTime
                .School_Code = SchoolCode
                .UpLevel_Id = Guid.NewGuid
            End With
            Ctx.t360_tblUpLevels.InsertOnSubmit(NewUpLevel)


            'ปรับ level detail  ปรับ t360_tblUpLevelDetail ให้ IsConfirm = true
            Dim UpLevelDetail = (From q In Ctx.t360_tblUpLevelDetails Where q.UpLevel_Id = UpLevel.UpLevel_Id)
            For Each I In UpLevelDetail
                I.IsConfirm = True
                I.ClientId = Nothing
                I.LastUpdate = GetTime
            Next

            'ปรับ level confirm ปรับ t360_tblUplevelConfirms ให้ IsActive = false
            Dim UpLevelConfirm = (From q In Ctx.t360_tblUplevelConfirms Where q.School_Code = SchoolCode And q.IsActive = True)
            For Each I In UpLevelConfirm
                I.IsActive = False
                I.ClientId = Nothing
                I.LastUpdate = GetTime
            Next

            If sqlCheckmark.ToString() <> "" Then 'เช็คเผื่อไม่ใช่ ใช้รวมกับ Checkmark
                If Not clsCheckmark.ExecuteSQLWithTransection(sqlCheckmark.ToString()) Then 'ถ้า save ที่ checmark ไม่สำหรับ ให้ t360 rollback ด้วย
                    Throw New Exception("ไม่สามารถ save ข้อมูลลง checkmark ได้")
                End If
            End If

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            HttpContext.Current.Application("DateSchedulerUplevel") = Nothing

            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Private Function GetSQLCheckmarkSpecialCase(ByVal SqlCheckmark As String, ByRef ClsCheckMark As ClsCheckMark, ByVal Student As t360_tblStudent) As String
        Dim studentNew As StudentCheckMark = ClsCheckMark.NewStudentCheckmark(ClsCheckMark, Student)
        Dim sql As New StringBuilder()
        sql.Append(studentNew.ToStringSQLDeleteCheckmark())
        sql.Append(SqlCheckmark) 'เอา temp มาต่อท้ายอีกทีนึง
        Return sql.ToString()
    End Function

    Public Function InsertUpLevelConfirm(Item As t360_tblUplevelConfirm) As Boolean Implements IStudentManager.InsertUpLevelConfirm
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Item.IsActive = True
            Item.LastUpdate = GetTime
            Item.Ulv_Id = Guid.NewGuid
            Ctx.t360_tblUplevelConfirms.InsertOnSubmit(Item)

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function DeleteUpLevelConfirm(SchoolCode As String, RoomId As Guid, CalendarYear As String) As Boolean Implements IStudentManager.DeleteUpLevelConfirm
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Dim r = (From q In Ctx.t360_tblUplevelConfirms Where q.Room_Id = RoomId And q.School_Code = SchoolCode And q.IsActive = True).SingleOrDefault
            If r IsNot Nothing Then
                r.IsActive = False
                r.LastUpdate = GetTime
                r.ClientId = Nothing

                Ctx.SubmitChanges()
                Factory.DataContextCommitTransaction()
            End If

            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function InsertSetTypeRunStudentNumber(Item As t360_tblSetTypeRunStudentNumber) As Boolean Implements IStudentManager.InsertSetTypeRunStudentNumber
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Item.LastUpdate = GetTime
            Item.IsActive = True
            Ctx.t360_tblSetTypeRunStudentNumbers.InsertOnSubmit(Item)

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function UpdateSetTypeRunStudentNumber(SchoolCode As String, Type As EnTypeStudentRunNo) As Boolean Implements IStudentManager.UpdateSetTypeRunStudentNumber
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Dim Data = Ctx.t360_tblSetTypeRunStudentNumbers.Where(Function(q) q.School_Code = SchoolCode).SingleOrDefault
            Data.Syr_Type = Type
            Data.LastUpdate = GetTime
            Data.ClientId = Nothing

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function SetStudentNumberInUpdateLevelByRoomId(SchoolCode As String, RoomId As Guid, Optional IsUpLevel As Boolean = False) As Boolean Implements IStudentManager.SetStudentNumberInUpdateLevelByRoomId
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Dim SetType
            If IsUpLevel Then
                SetType = EnTypeStudentRunNo.NoInRoom
            Else
                SetType = GetSetTypeRunStudentNumber(SchoolCode).Syr_Type
            End If

            Select Case SetType
                Case EnTypeStudentRunNo.Code
                    Dim AllStudent = (From q In Ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Room_Id = RoomId And q.IsActive = True And q.IsConfirm = False).OrderBy(Function(q) q.Student_Code).ToArray
                    Dim StudentNo As Integer = 0
                    For Each Student In AllStudent
                        StudentNo += 1
                        Student.Student_CurrentNoInRoom = StudentNo
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                    Next
                Case EnTypeStudentRunNo.Name
                    Dim AllStudent = (From q In Ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Room_Id = RoomId And q.IsActive = True And q.IsConfirm = False).OrderBy(Function(q) q.Student_FirstName).ThenBy(Function(q) q.Student_Code).ToArray
                    Dim StudentNo As Integer = 0
                    For Each Student In AllStudent
                        StudentNo += 1
                        Student.Student_CurrentNoInRoom = StudentNo
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                    Next
                Case EnTypeStudentRunNo.NoInRoom
                    Dim AllStudent = (From q In Ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Room_Id = RoomId And q.IsActive = True And q.IsConfirm = False).OrderBy(Function(q) q.Student_CurrentNoInRoomOld).ThenBy(Function(q) q.LastUpdate).ToArray
                    Dim StudentNo As Integer = 0
                    For Each Student In AllStudent
                        StudentNo += 1
                        Student.Student_CurrentNoInRoom = StudentNo
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                    Next
                Case Else
                    Dim AllStudent = (From q In Ctx.t360_tblUpLevelDetails Where q.School_Code = SchoolCode And q.Room_Id = RoomId And q.IsActive = True And q.IsConfirm = False).OrderBy(Function(q) q.sex).ThenBy(Function(q) q.Student_Code).ToArray
                    Dim StudentNo As Integer = 0
                    For Each Student In AllStudent
                        StudentNo += 1
                        Student.Student_CurrentNoInRoom = StudentNo
                        Student.LastUpdate = GetTime
                        Student.ClientId = Nothing
                    Next
            End Select

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try

    End Function

    Public Function UpdateStudentNumberInUpdateLevelByStudentId(Items() As StudentDTO) As Boolean Implements IStudentManager.UpdateStudentNumberInUpdateLevelByStudentId
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            For Each I In Items
                Dim Data = (From q In Ctx.t360_tblUpLevelDetails Where q.Student_Id = I.Student_Id And q.IsActive = True And q.IsConfirm = False).SingleOrDefault
                Data.Student_CurrentNoInRoom = I.Student_No
                Data.ClientId = Nothing
                Data.LastUpdate = GetTime
            Next

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

#End Region

    'Public Function GetParentWithDevice(StudentId As Guid) As ParentDTO() Implements IStudentManager.GetParentWithDevice
    '    With GetLinqToSql
    '        .MainSql = " SELECT ROW_NUMBER() OVER (ORDER BY sp.LastUpdate) AS ParentNumber, p.PR_Id AS ParentId,p.PR_FirstName AS ParentFirstName,p.PR_LastName AS ParentLastName,p.School_Code,sp.LastUpdate AS RegisterDate FROM tblParent p INNER JOIN tblStudentParent sp ON p.PR_Id = sp.PR_Id " & _
    '                   " WHERE sp.IsActive = 1 AND sp.Student_Id = '" & StudentId.ToString() & "';"
    '        .LockWhere = True
    '        Return .DataContextExecuteObjects(Of ParentDTO).ToArray
    '    End With
    'End Function


    Public Function InsertTabletDummy(Item As t360_tblTablet, ItemOwner As t360_tblTabletOwner) As Boolean Implements IStudentManager.InsertTabletDummy
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()

            Item.LastUpdate = GetTime
            Item.Tablet_LastUpdate = GetTime
            Ctx.t360_tblTablets.InsertOnSubmit(Item)

            ItemOwner.LastUpdate = GetTime
            ItemOwner.TON_ReceiveDate = GetTime
            ItemOwner.TabletOwner_IsActive = True
            Ctx.t360_tblTabletOwners.InsertOnSubmit(ItemOwner)

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function
End Class

Public Enum EnTypeStudentRunNo
    Code 'เรียงดัวยรหัสนักเรียน
    Sex 'เรียงด้วยเพศแล้วตามด้วยรหัสนักเรียน
    Name 'เรียงด้วยชื่อ
    NoInRoom 'เรียงเลขที่ ไหมเพิ่มมาใช้เรียงเลขที่นักเรียนที่เพิ่มเข้าก่อนเทอม ให้สามารถแทรกนักเรียนเข้าใหม่แล้วดันคนเก่าลงได้
End Enum

Public Enum EnTypeSex
    Male
    Female
    None
End Enum

Public Enum EnTypeRunUpLevel
    Manual
    Auto
End Enum