Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

''' <summary>
''' (PK Database) Subject_Id 
''' (PK Real) School_Code, Subject_Code, IsActive
''' </summary>
''' <remarks></remarks>
Public Interface ISubjectRepo

    '<<< Subject
    Sub InsertSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject)
    Sub UpdateSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject)
    Sub DeleteSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject)
    Function ValidateDuplicateSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Subject_Code As String, Optional ByVal Subject_Id As Guid? = Nothing) As Boolean
    Function GetSubjectByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As SubjectDTO) As T()

End Interface

Public Class SubjectRepo
    Implements ISubjectRepo

    Public Sub DeleteSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject) Implements ISubjectRepo.DeleteSubject
        Dim Data = (From r In Ctx.t360_tblSubjects Where r.Subject_Id = Item.Subject_Id AndAlso r.School_Code = Item.School_Code And r.Subject_IsActive = True).SingleOrDefault
        Data.Subject_IsActive = False
        Ctx.SubmitChanges()
    End Sub

    Public Function GetSubjectByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As SubjectDTO) As T() Implements ISubjectRepo.GetSubjectByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblSubject WHERE " & _
                            "{F}"
            Dim f As New SqlPart
            f.AddPart("Subject_IsActive={0}", Item.Subject_IsActive)
            f.AddPart("School_Code={0}", Item.School_Code)
            f.AddPart("Subject_Code={0}", Item.Subject_Code)
            f.AddPart("Subject_Id={0}", Item.Subject_Id)
            f.AddPart("Subject_Name={0}", Item.Subject_Name)
            f.AddPart("ST_Id={0}", Item.ST_Id)

            .ApplySqlPart("F", f)
            Return .DataContextExecuteObjects(Of T)(Ctx).ToArray
        End With

    End Function

    Public Sub InsertSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject) Implements ISubjectRepo.InsertSubject
        Ctx.t360_tblSubjects.InsertOnSubmit(Item)
        Ctx.SubmitChanges()
    End Sub

    Public Sub UpdateSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblSubject) Implements ISubjectRepo.UpdateSubject
        Dim Target As New t360_tblSubject
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblSubjects Where r.Subject_Id = Item.Subject_Id AndAlso r.School_Code = Item.School_Code And r.Subject_IsActive = True).SingleOrDefault
        End Using
        Ctx.t360_tblSubjects.Attach(Item, Target)
        Ctx.SubmitChanges()
    End Sub

    Public Function ValidateDuplicateSubject(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Subject_Code As String, Optional ByVal Subject_Id As Guid? = Nothing) As Boolean Implements ISubjectRepo.ValidateDuplicateSubject
        If Subject_Id Is Nothing Then
            Dim q = (From r In Ctx.t360_tblSubjects Where r.School_Code = School_Code AndAlso r.Subject_Code = Subject_Code AndAlso r.Subject_IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblSubjects Where r.School_Code = School_Code AndAlso r.Subject_Code = Subject_Code AndAlso r.Subject_IsActive = True AndAlso r.Subject_IsActive = True AndAlso r.Subject_Id <> Subject_Id).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
End Class
