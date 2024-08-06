Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface ISchoolRepo

    Function GetSchoolCode(ByVal Ctx As DataClassesTablet360DataContext) As String
    Function GetSchoolByCode(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As tblSchool
    Function GetTbl360SchoolByCode(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblSchool
    Sub UpdateSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSchool)
    Sub DeleteSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSchool)
    Function ValidateDuplicateSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, Optional ByVal OldSchool_Code As String = "") As Boolean

End Interface

Public Class SchoolRepo
    'Inherits DatabaseManager
    Implements ISchoolRepo

    Public Function GetSchoolCode(ByVal Ctx As DataClassesTablet360DataContext) As String Implements ISchoolRepo.GetSchoolCode
        'todo ถ้ากรณีรวมดาต้าเบสจะเป็นไงไม่รู้ตรงนี้
        Dim Result = (From r In Ctx.t360_tblSchools Select r.School_Code).FirstOrDefault
        Return Result
    End Function

    Public Function GetSchoolByCode(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As tblSchool Implements ISchoolRepo.GetSchoolByCode
        Dim Result = (From r In Ctx.tblSchools
                      Where r.SchoolId.ToString = School_Code
                     ).FirstOrDefault

        'Select New t360_tblSchool With {.District_Id = q.TambolId, _
        '                                              .Province_Id = q.ProvinceId, _
        '                                              .School_Director_Phone1 = r.School_Director_Phone1, _
        '                                              .School_Director_Phone2 = r.School_Director_Phone2, _
        '                                              .School_DirectorName = r.School_DirectorName, _
        '                                              .School_Email = r.School_Email, _
        '                                              .School_Fax = r.School_Fax, _
        '                                              .School_Name = q.SchoolName, _
        '                                              .School_Number = If(q.AreaId, ""), _
        '                                              .School_Code = q.SchoolId.ToString, _
        '                                              .School_Phone1 = r.School_Director_Phone1, _
        '                                              .School_Phone2 = r.School_Phone2, _
        '                                              .School_Street = r.School_Street, _
        '                                              .School_Website = r.School_Website, _
        '                                              .SubDistrict_Id = q.AmphurId}).FirstOrDefault
        Return Result
    End Function

    Public Function GetTbl360SchoolByCode(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblSchool Implements ISchoolRepo.GetTbl360SchoolByCode
        Dim Result = (From t360_tblSchool In Ctx.t360_tblSchools
                      Where t360_tblSchool.School_Code = School_Code
                  ).FirstOrDefault
        Return Result
    End Function

    Public Sub DeleteSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSchool) Implements ISchoolRepo.DeleteSchool
        Dim Data = (From r In Ctx.t360_tblSchools Where r.School_Code = Item.School_Code And r.School_IsActive = True).SingleOrDefault
        Ctx.t360_tblSchools.DeleteOnSubmit(Data)
        Ctx.SubmitChanges()
    End Sub

    Public Sub UpdateSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSchool) Implements ISchoolRepo.UpdateSchool
        Dim Target As New t360_tblSchool
        With GetLinqToSql
            Using ctx1 = .GetDataContext()
                Target = (From r In ctx1.t360_tblSchools Where r.School_Code = Item.School_Code And r.School_IsActive = True).SingleOrDefault
            End Using
            Ctx.t360_tblSchools.Attach(Item, Target)
            Ctx.SubmitChanges()
        End With
    End Sub

    Public Function ValidateDuplicateSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, Optional ByVal OldSchool_Code As String = "") As Boolean Implements ISchoolRepo.ValidateDuplicateSchool
        If OldSchool_Code = "" Then
            Dim q = (From r In Ctx.t360_tblSchools Where r.School_Code = School_Code And r.School_IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblSchools Where r.School_Code = School_Code And r.School_IsActive = True And r.School_Code <> OldSchool_Code).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

End Class
