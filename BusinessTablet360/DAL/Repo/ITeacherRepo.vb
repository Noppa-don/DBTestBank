Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web

Public Interface ITeacherRepo
    ''' (PK Database) Teacher_Id 
    ''' (PK Real) School_Code, Teacher_Code, IsActive
    Function GetTeacherByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T()
    Function GetTeacherForChangePassword(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T()
    Function GetTeacherForChangePasswordFromId(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As TeacherDTO)
    Sub InsertTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher)
    Sub UpdateTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher)
    Sub DeleteTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher)
    Function ValidateDuplicateTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Teacher_Code As String, ByVal School_Code As String, Optional ByVal Teacher_Id As Guid? = Nothing) As Boolean

End Interface

Public Class TeacherRepo
    Implements ITeacherRepo

    Public Sub DeleteTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher) Implements ITeacherRepo.DeleteTeacher
        Dim Data = (From r In Ctx.t360_tblTeachers Where r.Teacher_id = Item.Teacher_id And r.Teacher_IsActive = True).SingleOrDefault
        Data.Teacher_IsActive = False
        Ctx.SubmitChanges()
    End Sub

    Public Function GetTeacherByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As TeacherDTO, Optional ByVal Page As Integer = 0, Optional ByVal PageRow As Integer = 50) As T() Implements ITeacherRepo.GetTeacherByCrit

        With GetLinqToSql
            .MainSql = "SELECT DISTINCT T.* FROM t360_tblTeacher AS T LEFT JOIN t360_tblTeacherRoom AS TR ON T.Teacher_Id = TR.Teacher_Id AND TR.TR_IsActive=1 WHERE {F}"
            Dim f As New SqlPart
            f.AddPart("T.Teacher_Id={0}", Item.Teacher_Id)
            f.AddPart("T.Teacher_PrefixName={0}", Item.Teacher_PrefixName)
            f.AddPart("T.Teacher_FirstName like {0}", Item.Teacher_FirstName.FusionText("%"))
            f.AddPart("T.Teacher_LastName like {0}", Item.Teacher_LastName.FusionText("%"))
            f.AddPart("T.Teacher_Code like {0}", Item.Teacher_Code.FusionText("%"))
            f.AddPart("T.Teacher_IsActive={0}", Item.Teacher_IsActive)
            f.AddPart("T.Teacher_Status={0}", Item.Teacher_Status)
            f.AddPart("T.School_Code={0}", Item.School_Code)

            f.AddPart("TR.Class_Name={0}", Item.Teacher_CurrentClass)
            f.AddPart("TR.Room_Name={0}", Item.Teacher_CurrentRoom)
            '  f.AddPart("TR.Calendar_Id={0}", Item.Calendar_Id)
            .ApplySqlPart("F", f)

            If Page = 0 Then
                'Dim ItemLog As New t360_tblLog
                'With ItemLog
                '    .Log_Type = EnLogType.Search
                '    .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                '    .Log_Description = "ครู"
                'End With
                'UserManager.InsertLog(ItemLog)
                Return .DataContextExecuteObjects(Of T)(Ctx).ToArray
            Else
                'ทำ Paging
                Dim r = .DataContextExecuteObjects(Of T)(Ctx)
                Return r.Skip((Page - 1) * PageRow).Take(PageRow).ToArray
            End If
        End With
    End Function

    Public Sub UpdateTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher) Implements ITeacherRepo.UpdateTeacher
        Dim Target As New t360_tblTeacher
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblTeachers Where r.Teacher_id = Item.Teacher_id And r.Teacher_IsActive = True).SingleOrDefault
        End Using
        Ctx.t360_tblTeachers.Attach(Item, Target)
        Ctx.SubmitChanges()
    End Sub

    Public Function ValidateDuplicateTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Teacher_Code As String, ByVal School_Code As String, Optional ByVal Teacher_Id As Guid? = Nothing) As Boolean Implements ITeacherRepo.ValidateDuplicateTeacher
        If Teacher_Id Is Nothing Then
            Dim q = (From r In Ctx.t360_tblTeachers Where r.Teacher_Code = Teacher_Code And r.School_Code = School_Code And r.Teacher_IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblTeachers Where r.Teacher_Code = Teacher_Code And r.School_Code = School_Code And r.Teacher_IsActive = True And r.Teacher_id <> Teacher_Id).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Sub InsertTeacher(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblTeacher) Implements ITeacherRepo.InsertTeacher
        Ctx.t360_tblTeachers.InsertOnSubmit(Item)
        Ctx.SubmitChanges()
    End Sub

    Public Function GetTeacherForChangePassword(Of T)(Ctx As DataClassesTablet360DataContext, Item As TeacherDTO, Optional Page As Integer = 0, Optional PageRow As Integer = 50) As T() Implements ITeacherRepo.GetTeacherForChangePassword
        With GetLinqToSql

            .MainSql = "SELECT DISTINCT t360_tblTeacher.Teacher_Id,t360_tblTeacher.Teacher_FirstName,t360_tblTeacher.Teacher_LastName,tbluser.UserName FROM t360_tblTeacher " & _
                        "inner join tbluser on t360_tblTeacher.Teacher_id = tbluser.GUID AND t360_tblTeacher.Teacher_IsActive = '1' " & _
                        "and tblUser.IsActive = '1' WHERE {F}"

            '.MainSql = "SELECT DISTINCT T.* FROM t360_tblTeacher AS T LEFT JOIN t360_tblTeacherRoom AS TR ON T.Teacher_Id = TR.Teacher_Id AND TR.TR_IsActive=1 WHERE {F}"
            Dim f As New SqlPart
            f.AddPart("t360_tblTeacher.Teacher_FirstName like {0}", Item.Teacher_FirstName.FusionText("%"))
            f.AddPart("Teacher_Id={0}", Item.Teacher_Id)
            f.AddPart("t360_tblTeacher.School_Code={0}", Item.School_Code)
            .ApplySqlPart("F", f)

            If Page = 0 Then
                Return .DataContextExecuteObjects(Of T)(Ctx).ToArray
            Else
                'ทำ Paging
                Dim r = .DataContextExecuteObjects(Of T)(Ctx)
                Return r.Skip((Page - 1) * PageRow).Take(PageRow).ToArray
            End If
        End With
    End Function

    Public Function GetTeacherForChangePasswordFromId(Of T)(Ctx As DataClassesTablet360DataContext, Item As TeacherDTO) As Object Implements ITeacherRepo.GetTeacherForChangePasswordFromId
        With GetLinqToSql

            .MainSql = "SELECT DISTINCT t360_tblTeacher.Teacher_Id,t360_tblTeacher.Teacher_FirstName,t360_tblTeacher.Teacher_LastName,tbluser.UserName FROM t360_tblTeacher " & _
                        "inner join tbluser on t360_tblTeacher.Teacher_id = tbluser.GUID AND t360_tblTeacher.Teacher_IsActive = '1' " & _
                        "and tblUser.IsActive = '1' WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("Teacher_Id={0}", Item.Teacher_Id)
            f.AddPart("t360_tblTeacher.School_Code={0}", Item.School_Code)
            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of T)(Ctx).ToArray

        End With
    End Function
End Class
