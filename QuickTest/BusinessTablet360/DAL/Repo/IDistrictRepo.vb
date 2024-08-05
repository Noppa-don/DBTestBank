Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface IDistrictRepo
    Function GetDistrictByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As tblProvince) As tblAmphur()
End Interface

Public Class DistrictRepo
    Implements IDistrictRepo

    Public Function GetDistrictByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As tblProvince) As tblAmphur() Implements IDistrictRepo.GetDistrictByCrit
        Return Ctx.tblAmphurs.Where(Function(r) r.ProvinceId = Item.ProvinceId).ToArray
    End Function
End Class
