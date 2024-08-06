Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

''' <summary>
''' (PK Database) ST_Id 
''' (PK Real) ST_Name, IsActive 
''' </summary>
''' <remarks></remarks>
Public Interface ISubjectTypeRepo

    Function GetSubjectTypeByName(ByVal Ctx As DataClassesTablet360DataContext, ByVal ST_Name As String) As t360_tblSubjectType
    Function GetSubjectTypeAll(ByVal Ctx As DataClassesTablet360DataContext) As t360_tblSubjectType()
    Function GetSubjectTypeInSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblSubjectType()
    Sub UpdateSubjectTypeActive(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal ListSchoolSubjectType As t360_tblSchoolSubjectType())

End Interface

Public Class SubjectTypeRepo
    Implements ISubjectTypeRepo

    Public Function GetSubjectTypeByName(ByVal Ctx As DataClassesTablet360DataContext, ByVal ST_Name As String) As t360_tblSubjectType Implements ISubjectTypeRepo.GetSubjectTypeByName
        Return Ctx.t360_tblSubjectTypes.Where(Function(q) q.ST_Name = ST_Name).SingleOrDefault
    End Function

    Public Function GetSubjectTypeAll(ByVal Ctx As DataClassesTablet360DataContext) As t360_tblSubjectType() Implements ISubjectTypeRepo.GetSubjectTypeAll
        Return (Ctx.t360_tblSubjectTypes.ToArray)
    End Function

    Public Function GetSubjectTypeInSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblSubjectType() Implements ISubjectTypeRepo.GetSubjectTypeInSchool
        Return (From q1 In Ctx.t360_tblSubjectTypes Select q1).ToArray
    End Function

    Public Sub UpdateSubjectTypeActive(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal ListSchoolSubjectType As t360_tblSchoolSubjectType()) Implements ISubjectTypeRepo.UpdateSubjectTypeActive
        Dim DeleteSchoolSubjectType = Ctx.t360_tblSchoolSubjectTypes.Where(Function(q) q.School_Code = School_Code).ToList
        'ลบของเก่า
        Ctx.t360_tblSchoolSubjectTypes.DeleteAllOnSubmit(Of t360_tblSchoolSubjectType)(DeleteSchoolSubjectType)
        'เพิ่มของใหม่
        Ctx.t360_tblSchoolSubjectTypes.InsertAllOnSubmit(Of t360_tblSchoolSubjectType)(ListSchoolSubjectType)
        Ctx.SubmitChanges()
    End Sub

End Class
