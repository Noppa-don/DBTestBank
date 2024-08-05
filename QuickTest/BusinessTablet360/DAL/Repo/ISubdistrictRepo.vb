Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface ISubdistrictRepo
    Function GetSubdistrictByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As tblAmphur) As tblTambol()
End Interface

Public Class SubdistrictRepo
    Implements ISubdistrictRepo

    Public Function GetSubdistrictByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As tblAmphur) As tblTambol() Implements ISubdistrictRepo.GetSubdistrictByCrit
        Return Ctx.tblTambols.Where(Function(r) r.AmphurId = Item.AmphurId).ToArray
    End Function

End Class

