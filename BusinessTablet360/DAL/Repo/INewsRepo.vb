Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface INewsRepo

    Function GetNewsAll(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblNew()
    Function GetNewsById(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal News_Id As Guid) As t360_tblNew
    Function GetNewsByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO) As T()
    Sub UpdateNews(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblNew)
    Sub DeleteNews(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblNew)

End Interface

Public Class NewsRepo
    Implements INewsRepo

    Public Function GetNewsAll(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String) As t360_tblNew() Implements INewsRepo.GetNewsAll
        Return Ctx.t360_tblNews.Where(Function(n) n.School_Code = School_Code AndAlso n.News_IsActive = True).ToArray
    End Function

    Public Function GetNewsByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO) As T() Implements INewsRepo.GetNewsByCrit

    End Function

    Public Function GetNewsById(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal News_Id As Guid) As t360_tblNew Implements INewsRepo.GetNewsById
        Return Ctx.t360_tblNews.Where(Function(n) n.School_Code = School_Code AndAlso n.News_Id = News_Id).SingleOrDefault
    End Function

    Public Sub UpdateNews(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblNew) Implements INewsRepo.UpdateNews
        Dim Target As New t360_tblNew
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblNews Where r.School_Code = Item.School_Code And r.News_Id = Item.News_Id And r.News_IsActive = True).SingleOrDefault
        End Using
        Ctx.t360_tblNews.Attach(Item, Target)
        Ctx.SubmitChanges()
    End Sub

    Public Sub DeleteNews(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblNew) Implements INewsRepo.DeleteNews
        Dim Data = (From r In Ctx.t360_tblNews Where r.School_Code = Item.School_Code AndAlso r.News_Id = Item.News_Id And r.News_IsActive = True).SingleOrDefault
        Data.News_IsActive = False
        Ctx.SubmitChanges()
    End Sub

End Class
