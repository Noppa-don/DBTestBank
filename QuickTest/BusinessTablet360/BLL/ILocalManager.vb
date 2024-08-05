Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface ILocalManager
    Function GetProviceAll() As tblProvince()
    Function GetDistrictByCrit(ByVal Item As tblProvince) As tblAmphur()
    Function GetSubdistrictByCrit(ByVal Item As tblAmphur) As tblTambol()
End Interface

Public Class LocalManager
    Implements ILocalManager

#Region "Dependency"

    Private _ProviceRepo As IProviceRepo
    <Dependency()> Public Property ProviceRepo() As IProviceRepo
        Get
            Return _ProviceRepo
        End Get
        Set(ByVal value As IProviceRepo)
            _ProviceRepo = value
        End Set
    End Property

    Private _DistrictRepo As IDistrictRepo
    <Dependency()> Public Property DistrictRepo() As IDistrictRepo
        Get
            Return _DistrictRepo
        End Get
        Set(ByVal value As IDistrictRepo)
            _DistrictRepo = value
        End Set
    End Property

    Private _SubdistrictRepo As ISubdistrictRepo
    <Dependency()> Public Property SubdistrictRepo() As ISubdistrictRepo
        Get
            Return _SubdistrictRepo
        End Get
        Set(ByVal value As ISubdistrictRepo)
            _SubdistrictRepo = value
        End Set
    End Property

#End Region

    Public Function GetAllProvice() As tblProvince() Implements ILocalManager.GetProviceAll
        Using Ctx = GetLinqToSql.GetDataContext()
            Return ProviceRepo.GetProviceAll(Ctx)
        End Using
    End Function

    Public Function GetDistrictByCrit(ByVal Item As tblProvince) As tblAmphur() Implements ILocalManager.GetDistrictByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Return DistrictRepo.GetDistrictByCrit(Ctx, Item)
        End Using
    End Function

    Public Function GetSubdistrictByCrit(ByVal Item As tblAmphur) As tblTambol() Implements ILocalManager.GetSubdistrictByCrit
        Using Ctx = GetLinqToSql.GetDataContext()
            Return SubdistrictRepo.GetSubdistrictByCrit(Ctx, Item)
        End Using
    End Function

End Class
