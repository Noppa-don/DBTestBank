Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web
Imports System.Text

Public Interface ISchoolManager

    '<<< School
    Function GetSchoolCode() As String
    Function GetSchoolByCode(ByVal School_Code As String) As tblSchool
    Function GetTbl360SchoolByCode(ByVal School_Code As String) As t360_tblSchool
    Function GetTbl360SchoolJoinByCode(ByVal School_Code As String) As Object
    Function InsertSchool(ByVal Item As t360_tblSchool) As Boolean
    Function UpdateSchool(ByVal Item As t360_tblSchool) As Boolean
    Function DeleteSchool(ByVal Item As t360_tblSchool) As Boolean
    Function ValidateDuplicateSchool(ByVal School_Code As String, Optional ByVal OldSchool_Code As String = "") As Boolean
    Function SetSyncTime(StartSync As TimeSpan, EndSync As TimeSpan, OnSync As Boolean, SchoolCode As String) As t360_tblSchool
    Function SetReportEmail(Email As String, SchoolCode As String) As t360_tblSchool
    Function GetSchoolSubjectClass(ByVal SchoolCode As String)
    Function GetSchoolAllowedClass(ByVal SchoolCode As String)

    '<<< Class
    'Function UpdateClass(ByVal Item As t360_tblClass) As Boolean
    Function UpdateClassActive(SchoolCode As String, ByVal ListtblSchoolClassActive As t360_tblSchoolClass(), ByVal ListtblSchoolClassNotActive As t360_tblSchoolClass()) As Boolean
    ''' <summary>
    ''' ทุกชั้น
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassAll() As t360_tblClass()
    ''' <summary>
    ''' ชั้นที่มีอยู่ในโรงเรียน
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassInSchool(Optional ByVal SchoolCode As String = "") As t360_tblClass()
    ''' <summary>
    ''' ชั้นที่มีอยู่ดูจากข้อมูลนักเรียน
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassHaveRoom(Optional ByVal SchoolCode As String = "") As t360_tblClass()
    ''' <summary>
    ''' ชั้นที่มีอยู่ เฉพาะที่มีห้องเรียนแล้ว
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassInStudent(Optional ByVal SchoolCode As String = "") As ClassDto()
    ''' <summary>
    ''' ชั้นที่มีอยู่ดูจากข้อมูลนักเรียน
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassInStudentBySchoolCodeAndCurrentId(ByVal SchoolCode As String, CurrentId As Guid) As ClassDto()
    ''' <summary>
    ''' เอาเฉพาะชั้นที่มีนักเรียนอยู่ในโรงเรียน
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassOfStudent(ByVal SchoolCode As String) As Object()
    ''' <summary>
    ''' เอาชั้นเรียนที่ต่ำกว่าชั้นสุดท้ายทุกชั้น และ มีนักเรียนอยู่ด้วย ยกตัวอย่างถ้าชั้นสุดท้ายคือ ม.3 ก็จะมีได้ชั้นตั้งแต่ ม.2 ลงไป
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassOfStudentNotOver() As Object()
    ''' <summary>
    ''' เอาเฉพาะชั้นที่มีนักเรียนอยู่และอยู่ในชั้นที่เป็นชั้นจบของโรงเรียนเช่น ป.6, ม.3, ม.6
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassOfStudentFinish() As Object()
    ''' <summary>
    ''' ฟั่งชั่นหาชั้นที่อยู่เหนือกว่าชั้นที่ส่งเข้ามา ยกตัวอย่าง ป.6 ก็จะได้ ม.1
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassOverMe(ByVal Item As t360_tblClass) As t360_tblClass
    ''' <summary>
    ''' หาชั้นโดยหาตามเงื่อนไขชื่อชั้น ไม่ได้สนใจว่า IsActive หรือเปล่า
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetClassByClassName(ByVal Item As t360_tblClass) As t360_tblClass
    Function GetClassForTeacher(CalendarId As Guid, SchoolCode As String) As Object()
    Function GetTypeClass(SchoolCode As String, ClassName As String) As EnTypeClass
    Function GetHighClass(SchoolCode As String) As String
    Function GetLevelAll() As tblLevel()
    Function GetLevelByLevel(Level As Integer) As tblLevel


    '<<< Room
    ''' <summary>
    ''' ค้นหา Room ตามเงื่อนไขที่ส่ง
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function InsertRoom(ByVal Item As t360_tblRoom) As Boolean
    Function UpdateRoom(ByVal Item As t360_tblRoom) As Boolean
    Function DeleteRoom(ByVal Item As t360_tblRoom) As Boolean
    Function ValidateDuplicateRoom(ByVal Class_Name As String, ByVal Room_Name As String, _
                                   Optional ByVal Room_Id As Guid? = Nothing) As Boolean
    Function GetRoomBySchoolCode(SchoolCode As String) As t360_tblRoom()
    'ดึงห้องทั้งหมดของชั้นนี้
    Function GetRoomByClassName(ByVal Item As t360_tblRoom) As t360_tblRoom()
    Function GetRoomByRoomId(ByVal RoomId As Guid) As t360_tblRoom
    Function GetRoomByClassNameAndRoomName(ByVal ClassName As String, ByVal RoomName As String) As t360_tblRoom
    Function GetRoomInUpLevel(SchoolCode As String) As ClassDto()
    'ดึงห้องที่มีนักเรียน
    Function GetStudentRoomByClassName(ByVal Item As t360_tblRoom) As t360_tblRoom()

    '<<< Calendar
    Function InsertCalendar(ByVal Item As t360_tblCalendar) As Boolean
    Function UpdateCalendar(ByVal Item As t360_tblCalendar) As Boolean
    Function DeleteCalendar(ByVal Item As t360_tblCalendar) As Boolean
    Function DeleteCalendarInYear(ByVal Year As String) As Boolean
    Function ValidateDuplicateCalendar(SchoolCode As String, ByVal Calendar_Name As String, StartDate As DateTime, ToDate As DateTime, Optional Calendar_Id As Guid? = Nothing) As Boolean
    Function ValidateDuplicateTermCalendar(SchoolCode As String, StartDate As DateTime, ToDate As DateTime, Optional Calendar_Id As Guid? = Nothing) As Boolean
    Function GetCalendarByCrit(Of T)(ByVal Item As CalendarDTO) As T()
    ''' <summary>
    ''' ฟังชั่นคืนค่าปฏิทินไว้ใช้กับหน้าปฏิทินเพระจะคิวรี่เอาวันที่คาบเกี่ยวระหว่างเดือนมาด้วย
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="Item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCalendarForScheduler(Of T)(ByVal Item As CalendarDTO) As T()
    ''' <summary>
    ''' ฟังชั่นคืนค่าปีปฏิทินของปีล่าสุด
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCalendarMaxYear() As String
    ''' <summary>
    ''' ฟังชั่นคืนค่าปีของปฏิทินที่ถูกบันทึกทังหมด
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCalendarAllYear() As String()
    ''' <summary>
    ''' ฟั่งชั่น copy calendar ข้ามปีโดยส่งปีที่ต้องการ copy มา
    ''' </summary>
    ''' <param name="FromYear"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CopyCalendar(ByVal FromYear As String, ByVal ToYear As String) As Boolean
    Function GetCalendarByCalendarId(ByVal CalendarId As Guid) As t360_tblCalendar
    Function GetNextCalendar(ByVal CalendarId As Guid) As t360_tblCalendar
    Function IsCanDeleteCalendar(CalendarId As Guid, SchoolCode As String) As Boolean
    Function IsNewYearCalendar(SchoolCode As String) As Boolean
    Function IsSemesterBreak() As Boolean

    '<<< News
    Function GetNewsAll() As t360_tblNew()
    Function GetNewsById(ByVal News_Id As Guid) As t360_tblNew
    Function InsertNews(ByVal ItemNews As t360_tblNew, ByVal ListNewsRoom As t360_tblNewsRoom()) As Boolean
    Function UpdateNews(ByVal ItemNews As t360_tblNew, ByVal ListNewsRoom As t360_tblNewsRoom()) As Boolean
    Function DeleteNews(ByVal Item As t360_tblNew) As Boolean

    '<<< NewsRoom
    Function GetNewsRoomByNewsId(ByVal News_Id As Guid) As t360_tblNewsRoom()
    Function GetNewsRoomByCrit(Of T)(ByVal Item As NewsDTO) As T()

End Interface

Public Class SchoolManager
    'Inherits DatabaseManager
    Implements ISchoolManager

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

    Private _RoomRepo As IRoomRepo
    <Dependency()> Public Property RoomRepo() As IRoomRepo
        Get
            Return _RoomRepo
        End Get
        Set(ByVal value As IRoomRepo)
            _RoomRepo = value
        End Set
    End Property

    Private _SchoolRepo As ISchoolRepo
    <Dependency()> Public Property SchoolRepo() As ISchoolRepo
        Get
            Return _SchoolRepo
        End Get
        Set(ByVal value As ISchoolRepo)
            _SchoolRepo = value
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

    Private _CalendarRepo As ICalendarRepo
    <Dependency()> Public Property CalendarRepo() As ICalendarRepo
        Get
            Return _CalendarRepo
        End Get
        Set(ByVal value As ICalendarRepo)
            _CalendarRepo = value
        End Set
    End Property

    Private _NewsRepo As INewsRepo
    <Dependency()> Public Property NewsRepo() As INewsRepo
        Get
            Return _NewsRepo
        End Get
        Set(ByVal value As INewsRepo)
            _NewsRepo = value
        End Set
    End Property

    Private _NewsRoomRepo As INewsRoomRepo
    <Dependency()> Public Property NewsRoomRepo() As INewsRoomRepo
        Get
            Return _NewsRoomRepo
        End Get
        Set(ByVal value As INewsRoomRepo)
            _NewsRoomRepo = value
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

    Private _UserManager As IUserManager
    <Dependency()> Public Property UserManager() As IUserManager
        Get
            Return _UserManager
        End Get
        Set(ByVal value As IUserManager)
            _UserManager = value
        End Set
    End Property

    'todo !!! ระวังการเรียก Dependency กันไปมายกตัวอย่าง ถ้า IStudentManager มี ISchoolManager แล้วก็ไม่ควรให้ ISchoolManager มี IStudentManager
    'Private _StudentManager As IStudentManager
    '<Dependency()> Public Property StudentManager() As IStudentManager
    '    Get
    '        Return _StudentManager
    '    End Get
    '    Set(ByVal value As IStudentManager)
    '        _StudentManager = value
    '    End Set
    'End Property

#End Region

#Region "School"

    Public Function GetSchoolCode() As String Implements ISchoolManager.GetSchoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return SchoolRepo.GetSchoolCode(Ctx)
        End Using
    End Function

    Public Function GetTbl360SchoolByCode(ByVal School_Code As String) As t360_tblSchool Implements ISchoolManager.GetTbl360SchoolByCode
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Result = SchoolRepo.GetTbl360SchoolByCode(Ctx, School_Code)
            Return Result
        End Using
    End Function

    Public Function GetTbl360SchoolJoinByCode(ByVal School_Code As String) As Object Implements ISchoolManager.GetTbl360SchoolJoinByCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From School In Ctx.t360_tblSchools Join Province In Ctx.tblProvinces On School.Province_Id Equals Province.ProvinceId
                    Join District In Ctx.tblAmphurs On School.District_Id Equals District.AmphurId
                    Join SubDistrict In Ctx.tblTambols On School.SubDistrict_Id Equals SubDistrict.TambolId Where
                    School.School_Code = School_Code).SingleOrDefault
        End Using
    End Function

    Public Function GetSchoolByCode(ByVal School_Code As String) As tblSchool Implements ISchoolManager.GetSchoolByCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return SchoolRepo.GetSchoolByCode(Ctx, School_Code)
        End Using
    End Function

    Public Function InsertSchool(ByVal Item As t360_tblSchool) As Boolean Implements ISchoolManager.InsertSchool
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Ctx.t360_tblSchools.InsertOnSubmit(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = "ข้อมูลโรงเรียน"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    ''' <summary>
    ''' ไม่หน้าจะใช้
    ''' </summary>
    ''' <param name="Item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteSchool(ByVal Item As t360_tblSchool) As Boolean Implements ISchoolManager.DeleteSchool
        Try
            Dim Ctx = GetLinqToSql.GetDataContext

            SchoolRepo.DeleteSchool(Ctx, Item)
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Delete
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = "ห้องเรียน"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateSchool(ByVal Item As t360_tblSchool) As Boolean Implements ISchoolManager.UpdateSchool
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = GetLinqToSql.GetDataContext
            Item.LastUpdate = GetTime
            Item.ClientId = Nothing

            Dim Target As New t360_tblSchool
            With GetLinqToSql
                Using ctx1 = .GetDataContext()
                    Target = (From r In ctx1.t360_tblSchools Where r.School_Code = Item.School_Code And r.School_IsActive = True).SingleOrDefault
                End Using

                Ctx.t360_tblSchools.Attach(Item, Target)
                Ctx.SubmitChanges()
            End With

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Page = "ข้อมูลโรงเรียน"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateSchool(ByVal School_Code As String, Optional ByVal OldSchool_Code As String = "") As Boolean Implements ISchoolManager.ValidateDuplicateSchool
        Using Ctx = GetLinqToSql.GetDataContext
            Return SchoolRepo.ValidateDuplicateSchool(Ctx, School_Code, OldSchool_Code)
        End Using
    End Function

    Public Function SetSyncTime(StartSync As TimeSpan, EndSync As TimeSpan, OnSync As Boolean, SchoolCode As String) As t360_tblSchool Implements ISchoolManager.SetSyncTime
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = (From q In Ctx.t360_tblSchools Where q.School_Code = SchoolCode And q.School_IsActive = True).SingleOrDefault
                Data.StartSync = StartSync
                Data.EndSync = EndSync
                Data.OnSync = OnSync
                Data.LastUpdate = GetTime

                Ctx.SubmitChanges()
                Return Data
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    Public Function SetReportEmail(Email As String, SchoolCode As String) As t360_tblSchool Implements ISchoolManager.SetReportEmail
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = (From q In Ctx.t360_tblSchools Where q.School_Code = SchoolCode And q.School_IsActive = True).SingleOrDefault
                Data.Report_Email = IIf(Email = "", Nothing, Email)
                Data.LastUpdate = GetTime
                Ctx.SubmitChanges()
                Return Data
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function



    Public Function GetSchoolSubjectClass(SchoolCode As String) Implements ISchoolManager.GetSchoolSubjectClass
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From ssc In Ctx.tblSchoolSubjectClasses
                        Join g In Ctx.tblGroupSubjects On ssc.GroupSubjectId Equals g.GroupSubject_Id
                        Join L In Ctx.tblLevels On ssc.LevelId Equals L.Level_Id
                        Where ssc.IsActive = True And ssc.SchoolCode = SchoolCode Select L.Level_ShortName, g.GroupSubject_ShortName).ToArray
            End Using
        End With
    End Function

    Public Function GetSchoolAllowedClass(SchoolCode As String) Implements ISchoolManager.GetSchoolAllowedClass
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From SC In Ctx.t360_tblSchoolClasses
                        Join C In Ctx.tblLevels On SC.Class_Name Equals C.Level_ShortName
                        Where SC.IsActive = True And SC.School_Code = SchoolCode Select C.Level_Id, C.Level_ShortName Order By Level_ShortName).ToArray
            End Using
        End With
    End Function



#End Region

#Region "Class"

    Public Function GetClassAll() As t360_tblClass() Implements ISchoolManager.GetClassAll
        Using Ctx = GetLinqToSql.GetDataContext
            Return ClassRepo.GetClassAll(Ctx)
        End Using
    End Function

    Public Function GetClassInSchool(Optional ByVal SchoolCode As String = "") As t360_tblClass() Implements ISchoolManager.GetClassInSchool
        Dim r As t360_tblClass()
        Using Ctx = GetLinqToSql.GetDataContext
            If SchoolCode = "" Then
                SchoolCode = UserConfig.GetCurrentContext.School_Code
            End If
            r = ClassRepo.GetClassInSchool(Ctx, SchoolCode)
        End Using
        Return r
    End Function

    Public Function GetClassHaveRoom(Optional SchoolCode As String = "") As t360_tblClass() Implements ISchoolManager.GetClassHaveRoom
        Dim r As t360_tblClass()
        Using Ctx = GetLinqToSql.GetDataContext
            If SchoolCode = "" Then
                SchoolCode = UserConfig.GetCurrentContext.School_Code
            End If
            r = ClassRepo.GetClassHaveRoom(Ctx, SchoolCode)
        End Using
        Return r
    End Function

    Public Function GetClassInStudent(Optional ByVal SchoolCode As String = "") As ClassDto() Implements ISchoolManager.GetClassInStudent
        Using Ctx = GetLinqToSql.GetDataContext
            If SchoolCode = "" Then
                SchoolCode = UserConfig.GetCurrentContext.School_Code
            End If

            Return (From s In Ctx.t360_tblStudents Join sr In Ctx.t360_tblStudentRooms On s.Student_CurrentRoomId Equals sr.Room_Id And s.Student_Id Equals sr.Student_Id
                    Join r In Ctx.t360_tblRooms On sr.Room_Id Equals r.Room_Id
                    Join c In Ctx.t360_tblClasses On sr.Class_Name Equals c.Class_Name
                    Where s.Student_IsActive = True And s.Student_Status = EnStudentStatus.Study And s.School_Code = SchoolCode And sr.SR_IsActive = True And r.Room_IsActive = True
                    Select New ClassDto With {.Class_Name = r.Class_Name, .Room_Id = s.Student_CurrentRoomId, .Room_Name = r.Room_Name}).Distinct().ToArray
        End Using
    End Function

    Public Function GetClassInStudentBySchoolCodeAndCurrentId(SchoolCode As String, CurrentId As Guid) As ClassDto() Implements ISchoolManager.GetClassInStudentBySchoolCodeAndCurrentId
        Using Ctx = GetLinqToSql.GetDataContext
            If SchoolCode = "" Then
                SchoolCode = UserConfig.GetCurrentContext.School_Code
            End If
            Return (From s In Ctx.t360_tblStudents Join sr In Ctx.t360_tblStudentRooms On s.Student_Id Equals sr.Student_Id
                    Join r In Ctx.t360_tblRooms On sr.Room_Id Equals r.Room_Id
                    Where s.Student_IsActive = True And s.Student_Status = EnStudentStatus.Study And s.School_Code = SchoolCode And sr.SR_IsActive = True And sr.Calendar_Id = CurrentId And r.Room_IsActive = True
                    Select New ClassDto With {.Class_Name = r.Class_Name, .Room_Id = s.Student_CurrentRoomId, .Room_Name = r.Room_Name}).Distinct().ToArray
        End Using
    End Function

    Public Function GetClassOverMe(ByVal Item As t360_tblClass) As t360_tblClass Implements ISchoolManager.GetClassOverMe
        Using Ctx = GetLinqToSql.GetDataContext
            Item.Class_IsActive = EnIsActiveStatus.Active
            Return ClassRepo.GetClassOverMe(Ctx, UserConfig.GetCurrentContext.School_Code, Item)
        End Using
    End Function

    Public Function GetClassOfStudent(ByVal SchoolCode As String) As Object() Implements ISchoolManager.GetClassOfStudent
        Using Ctx = GetLinqToSql.GetDataContext
            Return StudentRepo.GetClassOfStudent(Ctx, SchoolCode)
        End Using
    End Function

    Public Function GetClassOfStudentNotOver() As Object() Implements ISchoolManager.GetClassOfStudentNotOver
        Using Ctx = GetLinqToSql.GetDataContext
            Dim ClassActive = ClassRepo.GetClassInSchool(Ctx, UserConfig.GetCurrentContext.School_Code)
            If ClassActive.Length > 0 Then
                'เอาชื่อชั้นสูงสุด
                Dim Top = (From c In ClassActive Order By c.Class_Order Descending).FirstOrDefault.Class_Name
                'เอาชั้นสูงสุดออก
                Dim ClassNotTop = (From c In ClassActive Where Not c.Class_Name.Contains(Top) Select c.Class_Name)
                Dim R = (From s In Ctx.t360_tblStudents Where s.Student_IsActive = True AndAlso s.Student_Status = EnStudentStatus.Study _
                         AndAlso ClassNotTop.Contains(s.Student_CurrentClass) Select New With {.Class_Name = s.Student_CurrentClass}).Distinct
                Return R.ToArray
            Else
                Return {}
            End If
        End Using
    End Function

    Public Function GetClassOfStudentFinish() As Object() Implements ISchoolManager.GetClassOfStudentFinish
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Final = Ctx.t360_tblClasses.Where(Function(c) c.Class_Finish = True).Select(Function(q) q.Class_Name).ToArray
            Dim R = (From s In Ctx.t360_tblStudents Where s.Student_IsActive = True AndAlso s.Student_Status = EnStudentStatus.Study _
                    AndAlso Final.Contains(s.Student_CurrentClass) Select New With {.Class_Name = s.Student_CurrentClass}).Distinct
            Return R.ToArray
        End Using
    End Function

    Public Function GetClassByClassName(ByVal Item As t360_tblClass) As t360_tblClass Implements ISchoolManager.GetClassByClassName
        Using Ctx = GetLinqToSql.GetDataContext
            Item.Class_IsActive = EnIsActiveStatus.Active
            Return ClassRepo.GetClassByClassName(Ctx, UserConfig.GetCurrentContext.School_Code, Item)
        End Using
    End Function

    Public Function GetTypeClass(SchoolCode As String, ClassName As String) As EnTypeClass Implements ISchoolManager.GetTypeClass
        'แบบชื่อฟิกอยู่ไม่ยืดหยุ่น
        Using Ctx = GetLinqToSql.GetDataContext
            Dim HighClass = (From q In Ctx.t360_tblSchoolClasses Where q.School_Code = SchoolCode).OrderByDescending(Function(q) q.Class_Name).First.Class_Name
            If HighClass = ClassName Then
                Return EnTypeClass.Top
            ElseIf ClassName = "ป.3" Or ClassName = "ป.6" Or ClassName = "ม.3" Or ClassName = "ม.6" Then
                Return EnTypeClass.Final
            Else
                Return EnTypeClass.Normal
            End If
        End Using
    End Function

    Public Function GetHighClass(SchoolCode As String) As String Implements ISchoolManager.GetHighClass
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblSchoolClasses Where q.School_Code = SchoolCode And q.IsActive = True).OrderByDescending(Function(q) q.Class_Name).First.Class_Name
        End Using
    End Function

    Public Function UpdateClassActive(ByVal SchoolCode As String, ByVal ListtblSchoolClassActive As t360_tblSchoolClass(), ByVal ListtblSchoolClassNotActive As t360_tblSchoolClass()) As Boolean Implements ISchoolManager.UpdateClassActive
        Dim ClassSetActive As String = ""
        Dim ClassSetUnActive As String = ""
        With GetLinqToSql
            Try
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate

                Dim Ctx = .GetDataContextWithTransaction()
                For Each Row In ListtblSchoolClassNotActive
                    Dim Ori As t360_tblSchoolClass
                    Ori = Ctx.t360_tblSchoolClasses.Where(Function(q) q.School_Code = SchoolCode And q.Class_Name = Row.Class_Name And q.IsActive = True).SingleOrDefault
                    If Ori IsNot Nothing Then
                        With Ori
                            .IsActive = False
                            .LastUpdate = GetTime
                            .ClientId = Nothing
                        End With
                        ClassSetUnActive = Row.Class_Name & " , "
                    End If
                Next

                If ClassSetUnActive <> "" Then ClassSetUnActive = Left(ClassSetUnActive, ClassSetUnActive.Length - 3)

                For Each Row In ListtblSchoolClassActive
                    Dim Ori As t360_tblSchoolClass
                    Ori = Ctx.t360_tblSchoolClasses.Where(Function(q) q.School_Code = SchoolCode And q.Class_Name = Row.Class_Name And q.IsActive = True).SingleOrDefault
                    If Ori Is Nothing Then
                        With Row
                            .Sc_Id = Guid.NewGuid
                            .IsActive = True
                            .LastUpdate = GetTime
                            .School_Code = SchoolCode
                        End With
                        Ctx.t360_tblSchoolClasses.InsertOnSubmit(Row)
                        ClassSetActive = Row.Class_Name & " , "
                    End If
                Next
                If ClassSetActive <> "" Then ClassSetActive = Left(ClassSetActive, ClassSetActive.Length - 3)

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Dim clsCheckmark As New ClsCheckMark()
                    If Not clsCheckmark.RemoveClass(ListtblSchoolClassNotActive) Then
                        Throw New Exception("ไม่สามารถ save ลง checkmark ได้")
                    End If
                    If Not clsCheckmark.AddClass(ListtblSchoolClassActive) Then
                        Throw New Exception("ไม่สามารถ save ลง checkmark ได้")
                    End If
                End If

                Ctx.SubmitChanges()
                .DataContextCommitTransaction()
                Return True
            Catch ex As Exception
                .DataContextRollbackTransaction()
                ElmahExtension.LogToElmah(ex)
                Return False
            Finally
                Dim LogDetail As New StringBuilder

                If ClassSetUnActive <> "" Then
                    LogDetail.Append("ลบชั้นเรียน - ")
                    LogDetail.Append(ClassSetUnActive)
                    UserManager.Log(EnLogType.ImportantDelete, LogDetail.ToString)
                End If
                LogDetail.Clear()
                If ClassSetActive <> "" Then
                    LogDetail.Append("เพิ่มชั้นเรียน - ")
                    LogDetail.Append(ClassSetActive)
                    UserManager.Log(EnLogType.Insert, LogDetail.ToString)
                End If

            End Try
        End With
    End Function

    Public Function GetLevelAll() As tblLevel() Implements ISchoolManager.GetLevelAll
        Using Ctx = GetLinqToSql.GetDataContext
            Return Ctx.tblLevels.Where(Function(q) q.IsActive = True).ToArray
        End Using
    End Function

    Public Function GetLevelByLevel(Level As Integer) As tblLevel Implements ISchoolManager.GetLevelByLevel
        Using Ctx = GetLinqToSql.GetDataContext
            Return Ctx.tblLevels.Where(Function(q) q.Level = Level).SingleOrDefault
        End Using
    End Function

#End Region

#Region "Room"

    Public Function GetRoomBySchoolCode(SchoolCode As String) As t360_tblRoom() Implements ISchoolManager.GetRoomBySchoolCode
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblRooms Where q.School_Code = SchoolCode And q.Room_IsActive = True).ToArray
        End Using
    End Function

    Public Function GetRoomByClassName(ByVal Item As t360_tblRoom) As t360_tblRoom() Implements ISchoolManager.GetRoomByClassName
        Using Ctx = GetLinqToSql.GetDataContext
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Item.Room_IsActive = EnIsActiveStatus.Active
            Return RoomRepo.GetRoomByClassName(Ctx, Item)
        End Using
    End Function

    Public Function GetStudentRoomByClassName(Item As t360_tblRoom) As t360_tblRoom() Implements ISchoolManager.GetStudentRoomByClassName
        Using Ctx = GetLinqToSql.GetDataContext
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Item.Room_IsActive = EnIsActiveStatus.Active
            Return RoomRepo.GetStudentRoomByClassName(Ctx, Item)
        End Using
    End Function

    Public Function GetRoomInUpLevel(SchoolCode As String) As ClassDto() Implements ISchoolManager.GetRoomInUpLevel
        With GetLinqToSql
            .MainSql = "SELECT distinct t360_tblRoom.Room_Id,t360_tblRoom.Class_Name,t360_tblRoom.Room_Name FROM t360_tblUpLevelDetail INNER JOIN t360_tblRoom ON t360_tblUpLevelDetail.Room_Id = t360_tblRoom.Room_Id " &
                       "WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("t360_tblUpLevelDetail.School_Code = {0}", SchoolCode)
            f.AddPart("t360_tblUpLevelDetail.IsActive = {0}", True)
            f.AddPart("t360_tblUpLevelDetail.IsConfirm = {0}", False)
            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of ClassDto).ToArray
        End With
    End Function

    Public Function GetRoomByRoomId(ByVal RoomId As Guid) As t360_tblRoom Implements ISchoolManager.GetRoomByRoomId
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Item As New t360_tblRoom
            Item.Room_Id = RoomId
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Item.Room_IsActive = EnIsActiveStatus.Active
            Return RoomRepo.GetRoomByRoomId(Ctx, Item)
        End Using
    End Function

    Public Function DeleteRoom(ByVal Item As t360_tblRoom) As Boolean Implements ISchoolManager.DeleteRoom

        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Item.ClientId = Nothing
                RoomRepo.DeleteRoom(Ctx, Item)
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ห้องเรียน: ")
            LogDetail.Append(Item.Class_Name & Item.Room_Name)
            LogDetail.Append(" ,t360_tblRoom.Id=")
            LogDetail.Append(Item.Room_Id.ToString)
            UserManager.Log(EnLogType.Delete, LogDetail.ToString)
        End Try
    End Function

    Public Function InsertRoom(ByVal Item As t360_tblRoom) As Boolean Implements ISchoolManager.InsertRoom
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Item.LastUpdate = GetTime
                Item.Room_IsActive = True

                Ctx.t360_tblRooms.InsertOnSubmit(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = "ห้องเรียน: " & Item.Class_Name & Item.Room_Name & ", t360_tblRoom.Id=" & Item.Room_Id.ToString
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateRoom(ByVal Item As t360_tblRoom) As Boolean Implements ISchoolManager.UpdateRoom
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Ori = (From r In Ctx.t360_tblRooms Where r.Room_Name = Item.Room_Name AndAlso r.School_Code = Item.School_Code AndAlso r.Room_IsActive = True).SingleOrDefault
                If Ori IsNot Nothing Then
                    Throw New Exception("มีห้องที่ต้องการ update แล้ว")
                End If

                Ori = (From r In Ctx.t360_tblRooms Where r.Room_Id = Item.Room_Id AndAlso r.School_Code = Item.School_Code AndAlso r.Room_IsActive = True).SingleOrDefault

                If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
                    Dim clsCheckmark As New ClsCheckMark()
                    If Not clsCheckmark.EditRoom(Item, Ori) Then
                        Throw New Exception("ไม่สามารถ save ลง checkmark ได้")
                    End If
                End If

                'เปลี่ยนชื่อห้องด้วย
                Dim Students = (From s In Ctx.t360_tblStudents Where s.Student_CurrentRoomId = Item.Room_Id AndAlso s.Student_IsActive = True)
                For Each eachStudent In Students
                    eachStudent.Student_CurrentRoom = Item.Room_Name
                Next

                Dim StudentsRoom = (From r In Ctx.t360_tblStudentRooms Where r.Room_Id = Item.Room_Id AndAlso r.SR_IsActive = True)
                For Each eachStudentRoom In StudentsRoom
                    eachStudentRoom.Room_Name = Item.Room_Name
                Next

                Ori.Room_Name = Item.Room_Name
                Ori.LastUpdate = GetTime
                Ori.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Page = "ห้องเรียน"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateRoom(ByVal Class_Name As String, ByVal Room_Name As String, Optional ByVal Room_Id As Guid? = Nothing) As Boolean Implements ISchoolManager.ValidateDuplicateRoom
        Using Ctx = GetLinqToSql.GetDataContext
            Return RoomRepo.ValidateDuplicateRoom(Ctx, School_Code:=UserConfig.GetCurrentContext.School_Code, Class_Name:=Class_Name, Room_Name:=Room_Name, Room_Id:=Room_Id)
        End Using
    End Function

    Public Function GetRoomByClassNameAndRoomName(ByVal ClassName As String, ByVal RoomName As String) As t360_tblRoom Implements ISchoolManager.GetRoomByClassNameAndRoomName
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblRooms Where q.School_Code = UserConfig.GetCurrentContext.School_Code And q.Room_Name = RoomName And q.Class_Name = ClassName And q.Room_IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetClassForTeacher(CalendarId As Guid, SchoolCode As String) As Object() Implements ISchoolManager.GetClassForTeacher
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From tr In Ctx.t360_tblTeacherRooms Join r In Ctx.t360_tblRooms On tr.Room_Id Equals r.Room_Id
                    Where tr.TR_IsActive = True And tr.Calendar_Id = CalendarId And tr.School_Code = SchoolCode
                    Select r.Room_Id, r.Class_Name, r.Room_Name).Distinct.ToArray
        End Using
    End Function

#End Region

#Region "Calendar"

    Public Function GetCalendarByCrit(Of T)(ByVal Item As CalendarDTO) As T() Implements ISchoolManager.GetCalendarByCrit
        Using Ctx = GetLinqToSql.GetDataContext
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return CalendarRepo.GetCalendarByCrit(Of T)(Ctx, Item)
        End Using
    End Function

    Public Function GetCalendarByCalendarId(CalendarId As Guid) As t360_tblCalendar Implements ISchoolManager.GetCalendarByCalendarId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.t360_tblCalendars Where q.Calendar_Id = CalendarId).SingleOrDefault
        End Using
    End Function

    Public Function GetCalendarForScheduler(Of T)(ByVal Item As CalendarDTO) As T() Implements ISchoolManager.GetCalendarForScheduler
        Using Ctx = GetLinqToSql.GetDataContext
            Return CalendarRepo.GetCalendarForScheduler(Of T)(Ctx, Item)
        End Using
    End Function

    Public Function GetCalendarMaxYear() As String Implements ISchoolManager.GetCalendarMaxYear
        Using Ctx = GetLinqToSql.GetDataContext
            Return CalendarRepo.GetCalendarMaxYear(Ctx)
        End Using
    End Function

    Public Function ValidateDuplicateCalendar(SchoolCode As String, ByVal Calendar_Name As String, StartDate As DateTime, ToDate As DateTime, Optional Calendar_Id As Guid? = Nothing) As Boolean Implements ISchoolManager.ValidateDuplicateCalendar
        Using Ctx = GetLinqToSql.GetDataContext

            If Calendar_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = SchoolCode AndAlso r.Calendar_Name = Calendar_Name AndAlso r.Calendar_FromDate = StartDate And r.Calendar_ToDate = ToDate And r.IsActive = True).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = SchoolCode AndAlso r.Calendar_Name = Calendar_Name AndAlso r.Calendar_FromDate = StartDate And r.Calendar_ToDate = ToDate And r.IsActive = True And r.Calendar_Id <> Calendar_Id).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Using
    End Function

    Public Function ValidateDuplicateTermCalendar(SchoolCode As String, StartDate As Date, ToDate As Date, Optional Calendar_Id As Guid? = Nothing) As Boolean Implements ISchoolManager.ValidateDuplicateTermCalendar
        Using Ctx = GetLinqToSql.GetDataContext
            If Calendar_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = SchoolCode And r.IsActive = True And ((StartDate >= r.Calendar_FromDate And StartDate <= r.Calendar_ToDate) Or (ToDate >= r.Calendar_FromDate And ToDate <= r.Calendar_ToDate) Or (r.Calendar_FromDate >= StartDate And r.Calendar_FromDate <= ToDate) Or (r.Calendar_ToDate >= StartDate And r.Calendar_ToDate <= ToDate)))
                If q.Count = 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = SchoolCode And r.IsActive = True And r.Calendar_Id <> Calendar_Id And ((StartDate >= r.Calendar_FromDate And StartDate <= r.Calendar_ToDate) Or (ToDate >= r.Calendar_FromDate And ToDate <= r.Calendar_ToDate) Or (r.Calendar_FromDate >= StartDate And r.Calendar_FromDate <= ToDate) Or (r.Calendar_ToDate >= StartDate And r.Calendar_ToDate <= ToDate)))
                If q.Count = 0 Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Using
    End Function

    Public Function DeleteCalendarInYear(ByVal Year As String) As Boolean Implements ISchoolManager.DeleteCalendarInYear
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = (From r In Ctx.t360_tblCalendars Where r.Calendar_Year = Year AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code).ToArray
                For Each Row In Data
                    Row.IsActive = False
                    Row.LastUpdate = Now
                Next

                'Ctx.t360_tblCalendars.DeleteAllOnSubmit(Data)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function InsertCalendar(ByVal Item As t360_tblCalendar) As Boolean Implements ISchoolManager.InsertCalendar
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Item.ClientId = Nothing
                Item.LastUpdate = GetTime
                Item.IsActive = True
                Ctx.t360_tblCalendars.InsertOnSubmit(Item)
                Ctx.SubmitChanges()

                'เพิ่มเข้าไปที่ t360_tblUpLevel ถ้ายังไม่มี
                Dim DataUpLevel = Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = Item.School_Code).SingleOrDefault
                If DataUpLevel Is Nothing Then
                    Dim NewUpLevel As New t360_tblUpLevel
                    With NewUpLevel
                        .Calendar_Year = Item.Calendar_Year
                        .ClientId = Nothing
                        .IsActive = True
                        .LastUpdate = GetTime
                        .School_Code = Item.School_Code
                        .UpLevel_Id = Guid.NewGuid
                        .Calendar_Id = Item.Calendar_Id
                    End With
                    Ctx.t360_tblUpLevels.InsertOnSubmit(NewUpLevel)
                End If

                Ctx.SubmitChanges()
            End Using


            'ปรับ config เก็บค่าบันทึกใหม่ในกรณีที่เทอมปัจจับันที่บันทึกเข้าไปเปลี่ยน
            Dim Sys As New Service.ClsSystem(New ClassConnectSql)
            If Sys.GetCalendarId(Item.School_Code) <> "" Then
                'เจอ
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = New Guid(Sys.GetCalendarId(Item.School_Code))
                    Using Ctx = GetLinqToSql.GetDataContext
                        Dim Calendar = Ctx.t360_tblCalendars.Where(Function(q) q.Calendar_Id = .CurrentCalendar).SingleOrDefault
                        .CalendarYear = Calendar.Calendar_Year
                        .CalendarName = Calendar.Calendar_Name
                    End Using
                End With
                UserConfig.SetCurrentContext(Config)
            Else
                'ไม่เจอ
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = Nothing
                    .CalendarYear = ""
                    .CalendarName = ""
                End With
                UserConfig.SetCurrentContext(Config)
            End If


            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim CalendraTypeName As String
            If Item.Calendar_Type = "1" Then
                CalendraTypeName = "ทั่วไป"
            ElseIf Item.Calendar_Type = "2" Then
                CalendraTypeName = "วันหยุดนักขัตฤกษ์"
            Else
                CalendraTypeName = "เทอมการศึกษา"
            End If
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ปฏิทิน -หัวข้อ:")
            LogDetail.Append(Item.Calendar_Name)
            LogDetail.Append(" -ประเภท:")
            LogDetail.Append(CalendraTypeName)
            LogDetail.Append(" -ช่วงวันที่:")
            LogDetail.Append(Item.Calendar_FromDate & " - " & Item.Calendar_ToDate)
            LogDetail.Append(",t360_tblCalendar.Id=")
            LogDetail.Append(Item.Calendar_Id)
            UserManager.Log(EnLogType.Insert, LogDetail.ToString)
        End Try
    End Function

    Public Function UpdateCalendar(ByVal Item As t360_tblCalendar) As Boolean Implements ISchoolManager.UpdateCalendar
        Dim Factory = GetLinqToSql
        Try
            'ให้ระวังถ้าวันหลังมี ID อาจทำให้ข้อมูลหายได้
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Ori As t360_tblCalendar
                Using ctx1 = GetLinqToSql.GetDataContext()
                    Ori = ctx1.t360_tblCalendars.Where(Function(q) q.Calendar_Id = Item.Calendar_Id).SingleOrDefault
                End Using
                Item.ClientId = Nothing
                Item.IsActive = True
                Item.LastUpdate = GetTime
                Ctx.t360_tblCalendars.Attach(Item, Ori)

                'แก้ไขฟิว Calendar_Id ที่่ t360_tblUpLevel เผื่อเปลี่ยน
                Dim DataUpLevel = Ctx.t360_tblUpLevels.Where(Function(q) q.IsActive = True And q.School_Code = Item.School_Code And q.Calendar_Id = Item.Calendar_Id).SingleOrDefault
                If DataUpLevel IsNot Nothing Then
                    DataUpLevel.Calendar_Year = Item.Calendar_Year
                    DataUpLevel.LastUpdate = GetTime
                    DataUpLevel.ClientId = Nothing
                End If

                Ctx.SubmitChanges()
            End Using

            'Load Gonfig ใหม่
            Dim Sys As New Service.ClsSystem(New ClassConnectSql)
            If Sys.GetCalendarId(Item.School_Code) <> "" Then
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = New Guid(Sys.GetCalendarId(Item.School_Code))
                    Using Ctx = GetLinqToSql.GetDataContext
                        Dim Calendar = Ctx.t360_tblCalendars.Where(Function(q) q.Calendar_Id = .CurrentCalendar).SingleOrDefault
                        .CalendarYear = Calendar.Calendar_Year
                        .CalendarName = Calendar.Calendar_Name
                    End Using
                End With
                UserConfig.SetCurrentContext(Config)
            Else
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = Nothing
                    .CalendarYear = ""
                    .CalendarName = ""
                End With
                UserConfig.SetCurrentContext(Config)
            End If

            Return True
        Catch ex As Exception
            'Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'Dim ItemLog As New t360_tblLog
            'With ItemLog
            '    .Log_Type = EnLogType.ImportantUpdate
            '    .Log_Page = "ปฏิทิน"
            'End With
            'UserManager.InsertLog(ItemLog)
            Dim CalendarTypeName As String
            If Item.Calendar_Type = "1" Then
                CalendarTypeName = "ทั่วไป"
            ElseIf Item.Calendar_Type = "2" Then
                CalendarTypeName = "วันหยุดนักขัตฤกษ์"
            Else
                CalendarTypeName = "เทอมการศึกษา"
            End If
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ปฏิทิน -หัวข้อ:")
            LogDetail.Append(Item.Calendar_Name)
            LogDetail.Append(" -ประเภท:")
            LogDetail.Append(CalendarTypeName)
            LogDetail.Append(" -ช่วงวันที่:")
            LogDetail.Append(Item.Calendar_FromDate & " - " & Item.Calendar_ToDate)
            LogDetail.Append(",t360_tblCalendar.Id=")
            LogDetail.Append(Item.Calendar_Id)

            UserManager.Log(EnLogType.ImportantUpdate, LogDetail.ToString)
        End Try
    End Function

    Public Function DeleteCalendar(ByVal Item As t360_tblCalendar) As Boolean Implements ISchoolManager.DeleteCalendar
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext
                Dim Data = (From r In Ctx.t360_tblCalendars Where r.Calendar_Id = Item.Calendar_Id).SingleOrDefault
                Data.LastUpdate = GetTime
                Data.IsActive = False
                Data.ClientId = Nothing

                Ctx.SubmitChanges()
                'CalendarRepo.DeleteCalendar(Ctx, Item)
            End Using

            'Load Config
            Dim Sys As New Service.ClsSystem(New ClassConnectSql)
            If Sys.GetCalendarId(Item.School_Code) <> "" Then
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = New Guid(Sys.GetCalendarId(Item.School_Code))
                    Using Ctx = GetLinqToSql.GetDataContext
                        Dim Calendar = Ctx.t360_tblCalendars.Where(Function(q) q.Calendar_Id = .CurrentCalendar).SingleOrDefault
                        .CalendarYear = Calendar.Calendar_Year
                        .CalendarName = Calendar.Calendar_Name
                    End Using
                End With
                UserConfig.SetCurrentContext(Config)
            Else
                Dim Config = UserConfig.GetCurrentContext
                With Config
                    .CurrentCalendar = Nothing
                    .CalendarYear = ""
                    .CalendarName = ""
                End With
                UserConfig.SetCurrentContext(Config)
            End If

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally

            Dim CalendarTypeName As String
            If Item.Calendar_Type = "1" Then
                CalendarTypeName = "ทั่วไป"
            ElseIf Item.Calendar_Type = "2" Then
                CalendarTypeName = "วันหยุดนักขัตฤกษ์"
            Else
                CalendarTypeName = "เทอมการศึกษา"
            End If

            Dim LogDetail As New StringBuilder
            LogDetail.Append("ปฏิทิน -หัวข้อ: ")
            LogDetail.Append(Item.Calendar_Name)
            LogDetail.Append(" -ประเภท: ")
            LogDetail.Append(CalendarTypeName)
            LogDetail.Append(" -ช่วงวันที่: ")
            LogDetail.Append(Item.Calendar_FromDate & "-" & Item.Calendar_ToDate)
            LogDetail.Append(" ,t360_tblCalendar=")
            LogDetail.Append(Item.Calendar_Id.ToString)
            UserManager.Log(EnLogType.ImportantDelete, LogDetail.ToString)
        End Try
    End Function

    Public Function GetCalendarAllYear() As String() Implements ISchoolManager.GetCalendarAllYear
        Using Ctx = GetLinqToSql.GetDataContext
            Return Ctx.t360_tblCalendars.Where(Function(q) q.IsActive = True).Select(Function(c) c.Calendar_Year).Distinct.ToArray
        End Using
    End Function

    Public Function CopyCalendar(ByVal FromYear As String, ByVal ToYear As String) As Boolean Implements ISchoolManager.CopyCalendar
        Dim Factory = GetLinqToSql
        Try
            DeleteCalendarInYear(ToYear)
            Dim DifYear = CType(ToYear, Integer) - CType(FromYear, Integer)
            Factory.MainSql = "INSERT INTO t360_tblCalendar (School_Code, Calendar_Year, Calendar_Name, Calendar_FromDate, Calendar_ToDate, Calendar_Type, LastUpdate, IsActive) " &
                      "SELECT School_Code, " & ToYear & " , Calendar_Name, DATEADD(YEAR," & DifYear & " ,Calendar_FromDate), DATEADD(YEAR," & DifYear & ",Calendar_ToDate), " &
                      "Calendar_Type,dbo.GetThaiDate(),1 FROM t360_tblCalendar WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("Calendar_Year={0}", FromYear)
            Factory.ApplySqlPart("F", f)

            Dim Ctx = Factory.GetDataContextWithTransaction()
            Factory.DataContextExecuteCommand(Ctx)
            Factory.DataContextCommitTransaction()
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("คัดลอกปฎิทินปี ")
            LogDetail.Append(FromYear)
            LogDetail.Append(" ไปใช้ในปี ")
            LogDetail.Append(ToYear)
            UserManager.Log(EnLogType.CopyCalendar, LogDetail.ToString)

        End Try
    End Function

    Public Function GetNextCalendar(ByVal CalendarId As System.Guid) As t360_tblCalendar Implements ISchoolManager.GetNextCalendar
        'เอาเทอมที่มีวันที่เริ่มต้นที่มากกว่า วันที่สิ้นสุดของเทอมที่ส่งเข้ามา แล้วเรียงจากน้อมหามาก เอาตัวบนสุดเรียงจากน้อยไปมาก (ตัวที่มีวันที่ไกล้เคียงกับเทอมที่ส่งมามากที่สุด)
        With GetLinqToSql
            Using Ctx = .GetDataContext
                Dim Data = Ctx.t360_tblCalendars.Where(Function(q) q.Calendar_Id = CalendarId).SingleOrDefault
                'Dim s = (From q In Ctx.t360_tblCalendars Where q.Calendar_Id <> CalendarId And
                '         q.Calendar_FromDate > Data.Calendar_ToDate And q.Calendar_Type = EnCalendarType.Term And
                '         q.School_Code = Data.School_Code And q.IsActive = True).OrderBy(Function(q) q.Calendar_FromDate).ToString
                'Dim d As Date = New Date(Data.Calendar_ToDate.Value.Year, Data.Calendar_ToDate.Value.Month, Data.Calendar_ToDate.Value.Day)
                'Dim a = (From q In Ctx.t360_tblCalendars Where q.Calendar_Id <> CalendarId _
                '         And q.Calendar_Type = EnCalendarType.Term And
                '         q.School_Code = Data.School_Code And q.IsActive = True).OrderBy(Function(q) q.Calendar_FromDate).ToArray
                'Dim b = a.Where(Function(q) q.Calendar_FromDate > d).OrderBy(Function(q) q.Calendar_FromDate).FirstOrDefault

                Return (From q In Ctx.t360_tblCalendars Where q.Calendar_Id <> CalendarId And
                         q.Calendar_FromDate > Data.Calendar_ToDate And q.Calendar_Type = EnCalendarType.Term And
                         q.School_Code = Data.School_Code And q.IsActive = True).OrderBy(Function(q) q.Calendar_FromDate).FirstOrDefault
            End Using
        End With
    End Function

    Public Function IsCanDeleteCalendar(CalendarId As Guid, SchoolCode As String) As Boolean Implements ISchoolManager.IsCanDeleteCalendar
        With GetLinqToSql
            Using Ctx = .GetDataContext
                Dim FoundInStudent = ((From q In Ctx.t360_tblStudentRooms Where q.Calendar_Id = CalendarId And q.School_Code = SchoolCode _
                                      And q.SR_IsActive = True).Count > 0)
                Dim FoundInTeacher = ((From q In Ctx.t360_tblTeacherRooms Where q.Calendar_Id = CalendarId And q.School_Code = SchoolCode _
                                     And q.TR_IsActive = True).Count > 0)
                Dim FoundInUpdateLevel = ((From q In Ctx.t360_tblUpLevels Where q.Calendar_Id = CalendarId And q.School_Code = SchoolCode _
                                    And q.IsActive = True).Count > 0)
                Return (Not FoundInStudent And Not FoundInTeacher And Not FoundInUpdateLevel)
            End Using
        End With
    End Function

    Public Function IsNewYearCalendar(SchoolCode As String) As Boolean Implements ISchoolManager.IsNewYearCalendar
        Dim System As New Service.ClsSystem(New ClassConnectSql())
        Dim GetTime = System.GetThaiDate
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Calendar = (From q In Ctx.t360_tblCalendars Where q.Calendar_FromDate <= GetTime And q.Calendar_ToDate >= GetTime And q.Calendar_Type = EnCalendarType.Term And q.IsActive = True).SingleOrDefault
            If Calendar IsNot Nothing Then
                Dim CalendarOld = (From q In Ctx.t360_tblCalendars Where q.Calendar_ToDate < Calendar.Calendar_ToDate And q.Calendar_Type = EnCalendarType.Term And q.IsActive = True Order By q.Calendar_ToDate Descending).FirstOrDefault
                If CalendarOld IsNot Nothing AndAlso Calendar.Calendar_Year <> CalendarOld.Calendar_Year Then
                    Return True
                Else
                    'ถ้าไม่เจอเทอมเก่าเลย หน้าจะเป็น calendar แรก ไม่ควรถือว่าเป็นปีการศึกษาใหม่
                    Return False
                End If
            Else
                Return False
            End If
        End Using

    End Function

    Public Function IsSemesterBreak() As Boolean Implements ISchoolManager.IsSemesterBreak
        Dim System As New Service.ClsSystem(New ClassConnectSql())
        Dim GetTime = System.GetThaiDate
        Using Ctx = GetLinqToSql.GetDataContext
            Dim Calendar = (From q In Ctx.t360_tblCalendars Where q.Calendar_FromDate <= GetTime And q.Calendar_ToDate >= GetTime And q.Calendar_Type = EnCalendarType.Term And q.IsActive = True).SingleOrDefault
            If Calendar Is Nothing Then
                Return True
            End If

        End Using

        Return False

    End Function

#End Region

#Region "News"

    Public Function GetNewsAll() As t360_tblNew() Implements ISchoolManager.GetNewsAll
        Using Ctx = GetLinqToSql.GetDataContext
            Return NewsRepo.GetNewsAll(Ctx, UserConfig.GetCurrentContext.School_Code)
        End Using
    End Function

    Public Function GetNewsById(ByVal News_Id As Guid) As t360_tblNew Implements ISchoolManager.GetNewsById
        Using Ctx = GetLinqToSql.GetDataContext
            Return NewsRepo.GetNewsById(Ctx, UserConfig.GetCurrentContext.School_Code, News_Id)
        End Using
    End Function

    Public Function GetNewsRoomByNewsId(ByVal News_Id As Guid) As t360_tblNewsRoom() Implements ISchoolManager.GetNewsRoomByNewsId
        Using Ctx = GetLinqToSql.GetDataContext
            Return NewsRoomRepo.GetNewsRoomByNewsId(Ctx, UserConfig.GetCurrentContext.School_Code, News_Id)
        End Using
    End Function

    Public Function GetNewsRoomByCrit(Of T)(ByVal Item As NewsDTO) As T() Implements ISchoolManager.GetNewsRoomByCrit
        Using Ctx = GetLinqToSql.GetDataContext
            Item.School_Code = UserConfig.GetCurrentContext.School_Code
            Return NewsRoomRepo.GetNewsRoomByCrit(Of T)(Ctx, Item)
        End Using
    End Function

    Public Function InsertNews(ByVal ItemNews As t360_tblNew, ByVal ListNewsRoom() As t360_tblNewsRoom) As Boolean Implements ISchoolManager.InsertNews
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Dim Ctx = Factory.GetDataContextWithTransaction()
            ItemNews.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemNews.News_IsActive = True
            ItemNews.LastUpdate = GetTime
            For Each Row In ListNewsRoom
                Row.NR_Id = Guid.NewGuid
                Row.LastUpdate = GetTime
                Row.IsActive = True
            Next
            ItemNews.t360_tblNewsRooms.AddRange(ListNewsRoom)
            Ctx.t360_tblNews.InsertOnSubmit(ItemNews)

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()

            Dim News As New Service.ClsNews
            News.InsertNewsDetailCompletion(UserConfig.GetCurrentContext.School_Code, ItemNews.News_Id.ToString)

            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            Dim sbDescriptionDeail As New StringBuilder
            sbDescriptionDeail.Append("ข่าว-หัวข้อ: ")
            sbDescriptionDeail.Append(ItemNews.News_Information)
            sbDescriptionDeail.Append("-ช่วงวันที่: ")
            sbDescriptionDeail.Append(ItemNews.News_StartDate)
            sbDescriptionDeail.Append("-")
            sbDescriptionDeail.Append(ItemNews.News_EndDate)
            sbDescriptionDeail.Append(",t360_tblNews.Id=")
            sbDescriptionDeail.Append(ItemNews.News_Id.ToString)
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Description = sbDescriptionDeail.ToString
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            End With
            UserManager.InsertLog(ItemLog)
        End Try

    End Function

    Public Function UpdateNews(ByVal ItemNews As t360_tblNew, ByVal ListNewsRoom() As t360_tblNewsRoom) As Boolean Implements ISchoolManager.UpdateNews
        '1.ลบ NewsRoom
        '2.อัพเดด News
        '3.เพิ่ม ListNewsRoom เข้าไปที่ NewsRoom
        Dim Factory = GetLinqToSql
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Dim Ctx = Factory.GetDataContextWithTransaction()

            ItemNews.School_Code = UserConfig.GetCurrentContext.School_Code
            ItemNews.News_IsActive = True
            ItemNews.LastUpdate = GetTime
            ItemNews.ClientId = Nothing
            NewsRepo.UpdateNews(Ctx, ItemNews)
            Ctx.SubmitChanges()

            For Each NewsRoom In Ctx.t360_tblNewsRooms.Where(Function(q) q.News_Id = ItemNews.News_Id And q.IsActive = True).ToArray
                NewsRoom.IsActive = False
                NewsRoom.LastUpdate = GetTime
                NewsRoom.ClientId = Nothing
            Next

            For Each Row In ListNewsRoom
                Row.NR_Id = Guid.NewGuid
                Row.School_Code = ItemNews.School_Code
                Row.LastUpdate = GetTime
                Row.IsActive = True
            Next
            Dim Main = (From q In Ctx.t360_tblNews Where q.News_Id = ItemNews.News_Id).SingleOrDefault
            Main.t360_tblNewsRooms.AddRange(ListNewsRoom)

            Ctx.SubmitChanges()
            Factory.DataContextCommitTransaction()

            Dim News As New Service.ClsNews
            News.UpdateNewsDetailcompletion(UserConfig.GetCurrentContext.School_Code, ItemNews.News_Id.ToString)
            Return True
        Catch ex As Exception
            Factory.DataContextRollbackTransaction()
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            Dim sbDescriptionDeail As New StringBuilder
            sbDescriptionDeail.Append("ข่าว-หัวข้อ: ")
            sbDescriptionDeail.Append(ItemNews.News_Information)
            sbDescriptionDeail.Append("-ช่วงวันที่: ")
            sbDescriptionDeail.Append(ItemNews.News_StartDate)
            sbDescriptionDeail.Append("-")
            sbDescriptionDeail.Append(ItemNews.News_EndDate)
            sbDescriptionDeail.Append(",t360_tblNews.Id=")
            sbDescriptionDeail.Append(ItemNews.News_Id.ToString)
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Description = sbDescriptionDeail.ToString
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function DeleteNews(ByVal Item As t360_tblNew) As Boolean Implements ISchoolManager.DeleteNews

        Dim DeleteDetail As String = ""
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim System As New Service.ClsSystem(New ClassConnectSql())
                Dim GetTime = System.GetThaiDate

                Dim Data = (From r In Ctx.t360_tblNews Where r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.News_Id = Item.News_Id And r.News_IsActive = True).SingleOrDefault
                Data.ClientId = Nothing
                Data.LastUpdate = GetTime
                Data.News_IsActive = False
                DeleteDetail = "ข่าว-หัวข้อ:" & Data.News_Information & " " & "-ช่วงวันที่: " & Data.News_StartDate & "-" & Data.News_EndDate & ",t360_tblNews.Id=" & Data.News_Id.ToString
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Delete
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = DeleteDetail
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function


#End Region


End Class


Public Enum EnTypeClass
    Top
    Final
    Normal
End Enum

