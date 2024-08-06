Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

''' <summary>
''' (PK Database) Class_Name
''' </summary>
''' <remarks></remarks>
Public Interface IClassRepo

    Function GetClassAll(ByVal Ctx As DataClassesTablet360DataContext) As t360_tblClass()
    Function GetClassInSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblClass()
    Function GetClassHaveRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblClass()
    Function GetClassOverMe(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Item As t360_tblClass) As t360_tblClass
    Function GetClassByClassName(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Item As t360_tblClass) As t360_tblClass
    Sub UpdateClass(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblClass)
    Sub UpdateClassActive(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal ListtblSchoolClass As t360_tblSchoolClass())

End Interface

Public Class ClassRepo
    'Inherits DatabaseManager
    Implements IClassRepo


    Public Function GetClassAll(ByVal Ctx As DataClassesTablet360DataContext) As t360_tblClass() Implements IClassRepo.GetClassAll
        Dim Result = (From r In Ctx.t360_tblClasses Where r.Class_IsActive = True)
        Return Result.ToArray
    End Function

    Public Function GetClassInSchool(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblClass() Implements IClassRepo.GetClassInSchool
        Return (From q1 In Ctx.t360_tblClasses Join q2 In Ctx.t360_tblSchoolClasses On q1.Class_Name Equals q2.Class_Name Where q1.Class_IsActive = True AndAlso q2.School_Code = School_Code AndAlso q2.IsActive = True Order By q1.Class_Order Ascending Select q1).ToArray
    End Function

    Public Function GetClassHaveRoom(Ctx As DataClassesTablet360DataContext, School_Code As String) As t360_tblClass() Implements IClassRepo.GetClassHaveRoom
        Return (From q In Ctx.t360_tblClasses Join q2 In Ctx.t360_tblSchoolClasses On q.Class_Name Equals q2.Class_Name
                Join q3 In Ctx.t360_tblRooms On q.Class_Name Equals q3.Class_Name
                Where q.Class_IsActive = True AndAlso q2.School_Code = School_Code AndAlso q2.IsActive = True AndAlso q3.Room_IsActive = True
                Order By q.Class_Order Ascending Select q).Distinct.ToArray
    End Function

    Public Function GetClassOverMe(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Item As t360_tblClass) As t360_tblClass Implements IClassRepo.GetClassOverMe
        Dim ClassActive = GetClassInSchool(Ctx, School_Code)
        Dim CurrentOrder = Ctx.t360_tblClasses.Where(Function(q) q.Class_Name = Item.Class_Name).Select(Function(q) q.Class_Order).SingleOrDefault
        Dim Result = (From q In ClassActive Where q.Class_Order > CurrentOrder AndAlso _
                                                  q.Class_IsActive = Item.Class_IsActive) _
                                                  .OrderBy(Function(q) q.Class_Order).FirstOrDefault
        Return Result
    End Function

    Public Function GetClassByClassName(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Item As t360_tblClass) As t360_tblClass Implements IClassRepo.GetClassByClassName
        Dim ClassActive = GetClassInSchool(Ctx, School_Code)
        Dim Result = (From q In ClassActive Where q.Class_Name = Item.Class_Name).SingleOrDefault
        Return Result
    End Function

    Public Sub UpdateClass(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblClass) Implements IClassRepo.UpdateClass
        Dim Target As New t360_tblClass
        Using ctx1 = GetLinqToSql.GetDataContext
            Target = (From r In ctx1.t360_tblClasses Where r.Class_Name = Item.Class_Name).SingleOrDefault
        End Using
        Ctx.t360_tblClasses.Attach(Item, Target)
        Ctx.SubmitChanges()
    End Sub

    Public Sub UpdateClassActive(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal ListtblSchoolClass() As t360_tblSchoolClass) Implements IClassRepo.UpdateClassActive
        Dim DeleteSchoolClasse = Ctx.t360_tblSchoolClasses.Where(Function(q) q.School_Code = School_Code).ToList
        'ลบของเก่า
        For Each Row In DeleteSchoolClasse
            Row.IsActive = False
            Row.LastUpdate = Now
        Next
        Ctx.t360_tblSchoolClasses.DeleteAllOnSubmit(Of t360_tblSchoolClass)(DeleteSchoolClasse)
        'เพิ่มของใหม่
        Ctx.t360_tblSchoolClasses.InsertAllOnSubmit(Of t360_tblSchoolClass)(ListtblSchoolClass)
        Ctx.SubmitChanges()
    End Sub

   
End Class