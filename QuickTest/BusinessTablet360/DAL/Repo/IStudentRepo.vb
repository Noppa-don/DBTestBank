Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web


Public Interface IStudentRepo
    ''' (PK Database) Student_Id 
    ''' (PK Real) School_Code, Student_Code , IsActive
    Function GetStudentByCrit(Of t)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t()
    Function GetStudentNextTerm(Of t)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t()
    Function GetClassOfStudent(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As Object()
    Function GetCountStudentByroom(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Class_Name As String, ByVal Room_Name As String, CalendarId As Guid) As Integer
    Sub DeleteStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent)
    Sub UpdateStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent)
    Function InsertStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) As Guid
    Function DeleteStudentWithCheckmark(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) As Guid
    Function ValidateDuplicateStudent(ByVal Ctx As DataClassesTablet360DataContext, ByVal Student_Code As String, ByVal School_Code As String, Optional ByVal Student_Id As Guid? = Nothing) As Boolean
    Function ValidateDuplicateStudentNoInRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal StudentNoInRoom As String, ByVal School_Code As String, ClassName As String, RoomName As String, Optional ByVal Student_Id As Guid? = Nothing) As Boolean

End Interface

Public Class StudentRepo
    Implements IStudentRepo

    Public Function GetStudentByCrit(Of t)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As StudentDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As t() Implements IStudentRepo.GetStudentByCrit
        With GetLinqToSql
            .MainSql = "SELECT S.School_Code, S.Student_Id, S.Student_Code, S.Student_PrefixName, S.Student_FirstName, S.Student_LastName, S.Student_FatherName, S.Student_FatherPhone, S.Student_FatherPhone2, S.Student_MotherName, S.Student_MotherPhone, S.Student_NickName, " &
                             "S.Student_CurrentClass, S.Student_CurrentNoInRoom, S.Student_CurrentRoom, S.Student_IsActive, S.Student_Status, S.Student_Number, S.Student_Soi, S.Student_Street, S.SubDistrict_Id, S.District_Id, S.Province_Id, S.UserName, S.Password " &
                             "FROM t360_tblStudent AS S INNER JOIN t360_tblStudentRoom AS SA ON S.Student_Id = SA.Student_Id AND S.Student_CurrentRoomId = SA.Room_Id AND SA.SR_IsActive=1 " &
                             "WHERE {FILTER}"

            Dim f As New SqlPart
            With Item
                f.AddPart("S.Student_Status = {0}", .Student_Status)
                f.AddPart("S.Student_IsActive = {0}", Convert.ToInt32(.Student_IsActive))
                f.AddPart("S.Student_Id = {0}", .Student_Id)
                f.AddPart("S.School_Code = {0}", .School_Code)
                f.AddPart("S.Student_FirstName LIKE {0}", .Student_FirstName.FusionText("%"))
                f.AddPart("S.Student_LastName LIKE {0}", .Student_LastName.FusionText("%"))
                f.AddPart("S.Student_Code = {0}", .Student_Code)
                f.AddPart("S.Student_PrefixName = {0}", .Student_PrefixName)
                f.AddPart("S.Student_CurrentNoInRoom = {0}", .Student_CurrentNoInRoom)
                f.AddPart("SA.Class_Name = {0}", .Student_CurrentClass)
                f.AddPart("S.Student_CurrentRoom = {0}", .Student_CurrentRoom)
                f.AddPart("SA.Room_Name = {0}", .Room_Name)
                f.AddPart("SA.Calendar_Id = {0}", .Calendar_Id)
            End With
            .ApplySqlPart("FILTER", f)
            If Page = 0 Then
                Return .DataContextExecuteObjects(Of t)(Ctx).ToArray
            Else
                'ทำ Paging
                Dim r = .DataContextExecuteObjects(Of t)(Ctx)
                Return r.Skip((Page - 1) * PageRow).Take(PageRow).ToArray
            End If
        End With
    End Function


    Public Function GetStudentNextTerm(Of t)(Ctx As DataClassesTablet360DataContext, Item As StudentDTO, Optional Page As Integer = 0, Optional PageRow As Integer = 50) As t() Implements IStudentRepo.GetStudentNextTerm
        With GetLinqToSql
            .MainSql = "SELECT S.School_Code, S.Student_Id, S.Student_Code, S.Student_PrefixName, S.Student_FirstName, S.Student_LastName, S.Student_FatherName, S.Student_FatherPhone, S.Student_MotherName, S.Student_MotherPhone, S.Student_NickName, " & _
                             " r.Class_Name as Student_CurrentClass, S.Student_CurrentNoInRoom, r.Room_Name as Student_CurrentRoom, S.IsActive, S.Student_Status, S.Student_Number, S.Student_Soi, S.Student_Street, S.SubDistrict_Id, S.District_Id, S.Province_Id " & _
                             "FROM t360_tblUpLevelDetail AS S  inner join t360_tblRoom r on s.Room_Id = r.Room_Id " & _
                             "WHERE {FILTER} AND S.Student_Id not in (Select Student_Id From t360_tblStudent where Student_Isactive = '1');"

            Dim f As New SqlPart
            With Item
                f.AddPart("S.IsActive = {0}", .Student_IsActive)
                f.AddPart("S.Student_Id = {0}", .Student_Id)
                f.AddPart("S.School_Code = {0}", .School_Code)
                f.AddPart("S.Student_FirstName LIKE {0}", .Student_FirstName.FusionText("%"))
                f.AddPart("S.Student_LastName LIKE {0}", .Student_LastName.FusionText("%"))
                f.AddPart("S.Student_Code = {0}", .Student_Code)
                f.AddPart("S.Student_PrefixName = {0}", .Student_PrefixName)
                f.AddPart("S.Student_CurrentClass = {0}", .Student_CurrentClass)
                f.AddPart("S.Student_CurrentRoom = {0}", .Room_Name)
            End With
            .ApplySqlPart("FILTER", f)
            If Page = 0 Then
                Return .DataContextExecuteObjects(Of t)(Ctx).ToArray
            Else
                'ทำ Paging
                Dim r = .DataContextExecuteObjects(Of t)(Ctx)
                Return r.Skip((Page - 1) * PageRow).Take(PageRow).ToArray
            End If
        End With
    End Function

    Public Function GetClassOfStudent(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As Object() Implements IStudentRepo.GetClassOfStudent
        Dim Result = (From r In Ctx.t360_tblStudents Join c In Ctx.t360_tblClasses On r.Student_CurrentClass Equals c.Class_Name
                      Where r.Student_IsActive = True AndAlso r.Student_Status = EnStudentStatus.Study _
                      Select New With {.Class_Name = r.Student_CurrentClass, .Class_Order = c.Class_Order}).Distinct.ToArray
        Return Result
    End Function

    Public Function GetCountStudentByroom(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Class_Name As String, ByVal Room_Name As String, CalendarId As Guid) As Integer Implements IStudentRepo.GetCountStudentByroom
        'Dim a = (From r In Ctx.t360_tblStudents Join r2 In Ctx.t360_tblStudentRooms On r.Student_Id Equals r2.Student_Id And r.Student_CurrentRoomId Equals r2.Room_Id
        '        Where r.Student_Status = EnStudentStatus.Study And r.Student_IsActive = True And r.School_Code = School_Code _
        '        And r2.SR_IsActive = True And r2.Class_Name = Class_Name And r2.Room_Name = Room_Name And r2.Calendar_Id = CalendarId).ToArray
        Return (From r In Ctx.t360_tblStudents Join r2 In Ctx.t360_tblStudentRooms On r.Student_Id Equals r2.Student_Id And r.Student_CurrentRoomId Equals r2.Room_Id
                Where r.Student_Status = EnStudentStatus.Study And r.Student_IsActive = True And r.School_Code = School_Code _
                And r2.SR_IsActive = True And r2.Class_Name = Class_Name And r2.Room_Name = Room_Name).Count 'And r2.Calendar_Id = CalendarId
    End Function

    Public Function InsertStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) As Guid Implements IStudentRepo.InsertStudent
        ctx.t360_tblStudents.InsertOnSubmit(Item)
        ctx.SubmitChanges()

        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Dim clsCheckmark As New ClsCheckMark()
            Dim student As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Item)
            If Not clsCheckmark.AddStudent(student) Then
                Throw New Exception("ไม่สามารถ save ลง Checkmark ได้ - InsertStudent ")
            End If
        End If

        Return Item.Student_Id
    End Function

    Public Function DeleteStudentWithCheckmark(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) As Guid Implements IStudentRepo.DeleteStudentWithCheckmark
        Dim clsCheckmark As New ClsCheckMark()
        Dim student As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Item)
        If Not clsCheckmark.RemoveStudent(student) Then
            Throw New Exception("ไม่สามารถ save ลง checkmark ได้ - DeleteStudentWithCheckmark ")
        End If
    End Function

    Public Sub UpdateStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) Implements IStudentRepo.UpdateStudent
        Dim Target As New t360_tblStudent
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblStudents Where r.School_Code = Item.School_Code And r.Student_Id = Item.Student_Id).SingleOrDefault
        End Using
        ctx.t360_tblStudents.Attach(Item, Target)
        ctx.SubmitChanges()

        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Dim clsCheckmark As New ClsCheckMark()
            Dim student As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Item)
            Dim studentOld As StudentCheckMark = clsCheckmark.NewStudentCheckmark(clsCheckmark, Target)
            If Not clsCheckmark.EditStudent(student, studentOld) Then
                Throw New Exception("ไม่สามารถ save ลง checkmark ได้ - UpdateStudent ")
            End If
        End If

    End Sub

    Public Sub DeleteStudent(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudent) Implements IStudentRepo.DeleteStudent
        'หน้าจะไม่ได้ใช้เพราะไม่ได้ลบจริง
        Dim Data = (From r In ctx.t360_tblStudents Where r.Student_Id = Item.Student_Id And r.School_Code = Item.School_Code).SingleOrDefault
        ctx.t360_tblStudents.DeleteOnSubmit(Data)
        ctx.SubmitChanges()
    End Sub

    Public Function ValidateDuplicateStudent(ByVal Ctx As DataClassesTablet360DataContext, ByVal Student_Code As String, ByVal School_Code As String, Optional ByVal Student_id As Guid? = Nothing) As Boolean Implements IStudentRepo.ValidateDuplicateStudent
        If Student_id Is Nothing Then
            Dim q = (From r In Ctx.t360_tblStudents Where r.Student_Code = Student_Code And r.School_Code = School_Code And r.Student_IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblStudents Where r.Student_Code = Student_Code And r.School_Code = School_Code And r.Student_Id <> Student_id And r.Student_IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function ValidateDuplicateStudentNoInRoom(Ctx As DataClassesTablet360DataContext, StudentNoInRoom As String, School_Code As String, ClassName As String, RoomName As String, Optional Student_Id As Guid? = Nothing) As Boolean Implements IStudentRepo.ValidateDuplicateStudentNoInRoom
        If Student_Id Is Nothing Then
            Dim q = (From r In Ctx.t360_tblStudents Where r.Student_CurrentNoInRoom = StudentNoInRoom And r.Student_CurrentClass = ClassName And r.Student_CurrentRoom = RoomName And r.School_Code = School_Code And r.Student_IsActive = True)
            If q.Count = 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblStudents Where r.Student_CurrentNoInRoom = StudentNoInRoom And r.School_Code = School_Code And r.Student_CurrentClass = ClassName And r.Student_CurrentRoom = RoomName And r.Student_Id <> Student_Id And r.Student_IsActive = True)
            If q.Count = 0 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

End Class
