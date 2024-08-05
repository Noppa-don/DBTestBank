Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports System.Web
Imports KnowledgeUtils
Imports System.Text

''' <summary>
''' Network
''' (PK Database) Network_Id 
''' (PK Real) School_Code, Network_IP, IsActive
''' </summary>
''' <remarks></remarks>
Public Interface INetworkManager

    '<<< Network Type
    Function GetNetworkTypeAll() As EnumNetworkType

    '<<< Network
    Function GetNetworkAll() As t360_tblNetwork()
    Function GetNetworkByCrit(Of T)(ByVal Item As NetworkDTO) As T()
    Function InsertNetwork(ByVal Item As t360_tblNetwork) As Boolean
    Function UpdateNetwork(ByVal Item As t360_tblNetwork) As Boolean
    Function DeleteNetwork(ByVal Network_Id As Guid) As Boolean
    Function ValidateDuplicateNetwork(ByVal NetworkIP As String, ByVal NetworkName As String, Optional ByVal Network_Id As Guid? = Nothing) As Boolean

    '<<< NetworkHistory
    Function InsertNetworkHistory(ByVal Item As t360_tblNetworkHistory) As Boolean

End Interface

Public Class NetworkManager
    Implements INetworkManager

#Region "Dependency"

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

    Private _UserManager As IUserManager
    <Dependency()> Public Property UserManager() As IUserManager
        Get
            Return _UserManager
        End Get
        Set(ByVal value As IUserManager)
            _UserManager = value
        End Set
    End Property

#End Region

#Region "NetworkType"

    Public Function GetNetworkTypeAll() As EnumNetworkType Implements INetworkManager.GetNetworkTypeAll
        Return (New EnumNetworkType)
    End Function

#End Region

#Region "Network"

    Public Function DeleteNetwork(ByVal Network_Id As Guid) As Boolean Implements INetworkManager.DeleteNetwork
        Dim DeleteDetail As String = ""
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim Item = (From r In Ctx.t360_tblNetworks Where r.Network_Id = Network_Id AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code And r.Network_IsActive = True).SingleOrDefault
                Item.Network_IsActive = False
                Item.LastUpdate = GetTime
                Item.ClientId = Nothing
                DeleteDetail = "อุปกรณ์เครือข่าย-ชื่อ: " & Item.Network_Name & " " & "-ชนิด: " & Item.Network_Type & "-IP: " & Item.Network_IP & ",t360_tblNetwork.Id=" & Item.Network_Id.ToString
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Delete
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = DeleteDetail
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function InsertNetwork(ByVal Item As t360_tblNetwork) As Boolean Implements INetworkManager.InsertNetwork
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext()
                Item.LastUpdate = GetTime
                Item.Network_IsActive = True
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Ctx.t360_tblNetworks.InsertOnSubmit(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally

            Dim sbDescriptionDeail As New StringBuilder
            sbDescriptionDeail.Append("อุปกรณ์เครือข่าย-ชื่อ: ")
            sbDescriptionDeail.Append(Item.Network_Name)
            sbDescriptionDeail.Append("-ชนิด: ")
            sbDescriptionDeail.Append(Item.Network_Type)
            sbDescriptionDeail.Append("-IP: ")
            sbDescriptionDeail.Append(Item.Network_IP)
            sbDescriptionDeail.Append(",t360_tblNetwork.Id=")
            sbDescriptionDeail.Append(Item.Network_Id.ToString)
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = sbDescriptionDeail.ToString
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function UpdateNetwork(ByVal Item As t360_tblNetwork) As Boolean Implements INetworkManager.UpdateNetwork
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim Target As New t360_tblNetwork
                Target = (From r In Ctx.t360_tblNetworks Where r.Network_Id = Item.Network_Id AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code And r.Network_IsActive = True).SingleOrDefault
                With Item
                    Target.Network_IP = .Network_IP
                    Target.Network_Name = .Network_Name
                    Target.Network_Type = .Network_Type
                    Target.Network_Location = .Network_Location
                    Target.LastUpdate = GetTime
                    Target.ClientId = Nothing
                End With
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            Dim sbDescriptionDeail As New StringBuilder
            sbDescriptionDeail.Append("อุปกรณ์เครือข่าย-ชื่อ: ")
            sbDescriptionDeail.Append(Item.Network_Name)
            sbDescriptionDeail.Append("-ชนิด: ")
            sbDescriptionDeail.Append(Item.Network_Type)
            sbDescriptionDeail.Append("-IP:")
            sbDescriptionDeail.Append(Item.Network_IP)
            sbDescriptionDeail.Append(",t360_tblNetwork.Id=")
            sbDescriptionDeail.Append(Item.Network_Id.ToString)
            With ItemLog
                .Log_Type = EnLogType.Update
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = sbDescriptionDeail.ToString
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function ValidateDuplicateNetwork(ByVal NetworkIP As String, ByVal NetworkName As String, Optional ByVal Network_Id As Guid? = Nothing) As Boolean Implements INetworkManager.ValidateDuplicateNetwork
        Using Ctx = GetLinqToSql.GetDataContext()
            If Network_Id Is Nothing Then
                Dim q = (From r In Ctx.t360_tblNetworks Where r.Network_IP.Contains(NetworkIP) AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.Network_IsActive = True).SingleOrDefault
                If q Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Try
                    Dim q = (From r In Ctx.t360_tblNetworks Where r.Network_IP.Contains(NetworkIP) AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code AndAlso r.Network_IsActive = True AndAlso r.Network_Id <> Network_Id).SingleOrDefault
                    If q Is Nothing Then
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End If
        End Using
    End Function

    Public Function GetNetworkByCrit(Of T)(ByVal Item As NetworkDTO) As T() Implements INetworkManager.GetNetworkByCrit
        With GetLinqToSql
            .MainSql = "SELECT * " &
                       " FROM t360_tblNetwork WHERE {F} {F2} ORDER BY CAST(PARSENAME(REPLACE(Network_IP,':','.'),4) as int) " &
                       " ,CAST(PARSENAME(REPLACE(Network_IP,':','.'),3) as int) " &
                       " ,CAST(PARSENAME(REPLACE(Network_IP,':','.'),2) as int) " &
                       " ,CAST(PARSENAME(REPLACE(Network_IP,':','.'),1) as int) "

            Dim f As New SqlPart
            f.AddPart("School_Code={0}", Item.School_Code)
            f.AddPart("Network_Id={0}", Item.Network_Id)
            f.AddPart("Network_IP={0}", Item.Network_IP)
            f.AddPart("Network_IP LIKE {0}", Item.Network_IP_Like.FusionText("%", EnumTextPosition.Rigth))
            f.AddPart("Network_Name={0}", Item.Network_Name)
            f.AddPart("Network_Name LIKE {0}", Item.Network_Name_Like.FusionText("%"))
            f.AddPart("Network_Type={0}", Item.Network_Type)
            f.AddPart("Network_IsActive={0}", Item.Network_IsActive)

            If Item.FoundDay IsNot Nothing AndAlso Item.NotFoundDay Is Nothing Then
                f.AddPart("Network_LastDate >= {0}", If(Item.FoundDay Is Nothing, Item.FoundDay, Date.Today.AddDays(Item.FoundDay * -1)))
            ElseIf Item.FoundDay Is Nothing AndAlso Item.NotFoundDay IsNot Nothing Then
                f.AddPart("Network_LastDate <= {0} ", If(Item.NotFoundDay Is Nothing, Item.NotFoundDay, Date.Today.AddDays(Item.NotFoundDay * -1)))
            End If

            .ApplySqlPart("F", f)

            Dim f2 As New SqlPart("AND")
            If Item.FoundDay IsNot Nothing AndAlso Item.NotFoundDay IsNot Nothing Then
                f2.joinType = EnJoinType.JoinByOr
                f2.AddPart("(Network_LastDate >= {0}", If(Item.FoundDay Is Nothing, Item.FoundDay, Date.Today.AddDays(Item.FoundDay * -1)))
                f2.AddPart("(Network_LastDate <= {0} OR Network_LastDate IS NULL)", If(Item.NotFoundDay Is Nothing, Item.NotFoundDay, Date.Today.AddDays(Item.NotFoundDay * -1)))
            End If


            .ApplySqlPart("F2", f2)

            'UserManager.InsertLog(ItemLog)
            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

    Public Function GetNetworkAll() As t360_tblNetwork() Implements INetworkManager.GetNetworkAll
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblNetworks.Where(Function(q) q.Network_IsActive = True).ToArray
        End Using
    End Function



    Public Function GetNetworkLost(Of T)() As T()
        With GetLinqToSql
            '.MainSql = "SELECT * FROM t360_tblNetwork WHERE {F}"
            'Dim f As New SqlPart()
            'f.AddPart("((DATEDIFF(HOUR,Network_LastDate,dbo.GetThaiDate()) > {0}) OR (Network_LastDate IS NULL )) AND Network_IsIgnore = 0  ", 1)
            'f.AddPart("School_Code = {0}", WebApplicationManager.GetSchoolId)
            'f.AddPart("Network_IsActive = {0}", 1)
            '.ApplySqlPart("F", f)
            'Return .DataContextExecuteObjects(Of T)().ToArray

            Dim sql As String = String.Format("SELECT n.* FROM t360_tblNetwork n LEFT JOIN (SELECT MAX(NWH_PingTime) AS LastPingTime,Network_Id,Network_Name FROM t360_tblNetworkHistory WHERE School_Code = '{0}' AND NWH_IsFound = 0 GROUP BY Network_Id,Network_Name) AS h ON n.Network_Id = h.Network_Id " &
            " WHERE n.Network_IsActive = 1 AND n.Network_IsIgnore = 0 AND (n.Network_FirstDate IS NULL AND n.Network_LastDate IS NULL OR DATEDIFF(DAY,n.Network_LastDate,h.LastPingTime) > 5);", WebApplicationManager.GetSchoolId)
            .MainSql = sql
            .LockWhere = True
            Return .DataContextExecuteObjects(Of T)().ToArray
        End With
    End Function

    Public Function GetNetworkLostAll(Of T)() As T()

        '
        'Using Ctx = GetLinqToSql.GetDataContext()
        '    Dim q = (From h In Ctx.t360_tblNetworkHistories
        '            Where h.School_Code = WebApplicationManager.GetSchoolId And h.NWH_IsFound = False
        '            Group h By h.Network_Id Into Group
        '            Select New With {
        '                .LastPingTime = Group.Max(Function(h) h.NWH_PingTime),
        '                .Network_Id = Network_Id}).AsEnumerable()

        '    Dim q2 = From n In Ctx.t360_tblNetworks
        '             Join n1 In q
        '             On n1.Network_Id Equals q("Network_Id")

        'End Using

        'With GetLinqToSql
        '    .MainSql = "SELECT * FROM t360_tblNetwork WHERE {F}"
        '    Dim f As New SqlPart()
        '    f.AddPart("((DATEDIFF(HOUR,Network_LastDate,dbo.GetThaiDate()) > {0}) OR (Network_LastDate IS NULL)) ", 1)
        '    f.AddPart("School_Code = {0}", WebApplicationManager.GetSchoolId)
        '    f.AddPart("Network_IsActive = {0}", 1)
        '    .ApplySqlPart("F", f)
        '    Return .DataContextExecuteObjects(Of T)().ToArray
        'End With

        With GetLinqToSql
            Dim sql As String = String.Format("SELECT n.* FROM t360_tblNetwork n LEFT JOIN (SELECT MAX(NWH_PingTime) AS LastPingTime,Network_Id,Network_Name FROM t360_tblNetworkHistory WHERE School_Code = '{0}' AND NWH_IsFound = 0 GROUP BY Network_Id,Network_Name) AS h ON n.Network_Id = h.Network_Id " &
            " WHERE n.Network_IsActive = 1 AND (n.Network_FirstDate IS NULL AND n.Network_LastDate IS NULL OR DATEDIFF(DAY,n.Network_LastDate,h.LastPingTime) > 5);", WebApplicationManager.GetSchoolId)
            .MainSql = sql
            .LockWhere = True
            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

    Public Function SetNetworkIsIgnore(ByVal NetworkId As String, ByVal IsIgnore As Boolean, Optional reasonMsg As String = "") As Boolean
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Dim u As UserConfig = HttpContext.Current.Session("UserConText")
            Dim School_Code As String = u.School_Code
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim Target As New t360_tblNetwork
                Target = (From r In Ctx.t360_tblNetworks Where r.Network_Id = New Guid(NetworkId) AndAlso r.School_Code = School_Code And r.Network_IsActive = True).SingleOrDefault
                Target.Network_IsIgnore = IsIgnore
                Target.LastUpdate = GetTime
                Target.ClientId = Nothing

                If IsIgnore Then
                    Dim Reason As New t360_tblNetworkWarnReason
                    Reason.NWId = Guid.NewGuid()
                    Reason.Network_Id = NetworkId.ToGuid()
                    Reason.User_Id = u.User_Id
                    Reason.User_Name = u.User_FirstName
                    Reason.School_Code = School_Code
                    Reason.WarnReason = reasonMsg
                    Reason.CloseWarnDate = GetTime
                    Ctx.t360_tblNetworkWarnReasons.InsertOnSubmit(Reason)
                End If

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function GetReasonOfDevice(ByVal NetworkId As String)
        Try
            Dim u As UserConfig = HttpContext.Current.Session("UserConText")
            Dim School_Code As String = u.School_Code
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim Target As New t360_tblNetworkWarnReason
                Target = (From r In Ctx.t360_tblNetworkWarnReasons Where r.Network_Id = New Guid(NetworkId) AndAlso r.School_Code = School_Code).OrderByDescending(Function(t) t.CloseWarnDate).Take(1).SingleOrDefault '.OrderByDescending(Function(t) t.CloseWarnDate)
                Return Target
            End Using
        Catch ex As Exception
            Return ""
        End Try
    End Function

#End Region

#Region "NetworkHistory"

    Public Function InsertNetworkHistory(ByVal Item As t360_tblNetworkHistory) As Boolean Implements INetworkManager.InsertNetworkHistory
        Try
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim System As New Service.ClsSystem(New ClassConnectSql(True, Ctx.Connection.ConnectionString))
                Dim GetTime = System.GetThaiDate
                Dim Nw = Ctx.t360_tblNetworks.Where(Function(q) q.Network_Id = Item.Network_Id).SingleOrDefault
                If Item.NWH_IsFound = True Then
                    If Nw.Network_FirstDate Is Nothing Then
                        Nw.Network_FirstDate = Item.NWH_PingTime
                    End If
                    Nw.Network_LastDate = Item.NWH_PingTime
                    Nw.LastUpdate = GetTime
                    Item.School_Code = Item.School_Code
                    Item.LastUpdate = GetTime
                    Item.IsActive = True
                End If
                Nw.t360_tblNetworkHistories.Add(Item)

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

#End Region

End Class
