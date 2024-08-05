Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface IProviceRepo
    Function GetProviceAll(ByVal Ctx As DataClassesTablet360DataContext) As tblProvince()
    Function GetProviceByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblProvice) As t360_tblProvice()
End Interface

Public Class ProviceRepo
    Implements IProviceRepo

    Public Function GetProviceAll(ByVal Ctx As DataClassesTablet360DataContext) As tblProvince() Implements IProviceRepo.GetProviceAll
        Return Ctx.tblProvinces.ToArray
    End Function

    Public Function GetProviceByCrit(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblProvice) As t360_tblProvice() Implements IProviceRepo.GetProviceByCrit
        
    End Function

End Class
