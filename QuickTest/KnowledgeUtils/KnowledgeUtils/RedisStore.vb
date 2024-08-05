Imports System.Configuration
Imports Newtonsoft.Json
Imports StackExchange.Redis

Public Class RedisStore

    Private Shared hostConnectionString As String = ConfigurationManager.ConnectionStrings("RedisConnectionString").ConnectionString
    '"activationRedis.redis.cache.windows.net:6380,password=FCClg3IKQNEJJzXiW7DD8rM75dLAPLIgMRFPa6Rb3fQ=,ssl=True,abortConnect=False"



    Private Shared lazyConnection As New Lazy(Of ConnectionMultiplexer)(Function()
                                                                            Return ConnectionMultiplexer.Connect(hostConnectionString)

                                                                        End Function)


    Public Shared ReadOnly Property Connection As ConnectionMultiplexer
        Get
            'Dim red As ConnectionMultiplexer
            'red = ConnectionMultiplexer.Connect("activationRedis.redis.cache.windows.net:6380,password=FCClg3IKQNEJJzXiW7DD8rM75dLAPLIgMRFPa6Rb3fQ=,ssl=True,abortConnect=False")
            Return lazyConnection.Value
            'Return red
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(RedisConnectionString As String)
        hostConnectionString = RedisConnectionString
    End Sub

#Region "Get/Set Keys"
    Public Function SetKey(ByVal key As String, ByVal value As String) As Boolean
        Dim result As Boolean
        Try
            Dim cache As IDatabase = Connection.GetDatabase()
            result = cache.StringSet(key, value)

        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)

        End Try
        Return result
    End Function



    Public Function SetKey(Of T)(ByVal key As String, ByVal value As T) As Boolean
        'cache.StringSet("e25", JsonConvert.SerializeObject(new Employee(25, "Clayton Gragg")));
        Dim result As Boolean
        Try
            Dim cache As IDatabase = Connection.GetDatabase()

            result = cache.StringSet(key, JsonConvert.SerializeObject(value))


        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)

        End Try
        Return result
    End Function


    Public Function Getkey(ByVal key As String) As String
        If IsNothing(key) Then Return ""
        Dim result As String = Nothing
        Try
            Dim cache As IDatabase = Connection.GetDatabase()

            result = cache.StringGet(key)
            If IsNothing(result) Then result = ""

        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)

        End Try
        Return result
    End Function
    Public Function Getkey(Of T)(ByVal key As String) As T
        'Dim needDebugGetKeyOfT As Boolean = False

        'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("A value of key = " & key))
        'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("B value of isnothing(key) = " & IsNothing(key).ToString))

        If IsNothing(key) Then Return Nothing
        Dim result 'As T = Nothing
        Try
            Dim cache As IDatabase = Connection.GetDatabase()
            Dim cacheValue = cache.StringGet(key)

            'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("C value of isnothing(cacheValue) = " & IsNothing(cacheValue).ToString))
            'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("D value of cacheValue = " & cacheValue.ToString))

            If Not cacheValue.IsNullOrEmpty Then
                'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("E 1"))
                result = JsonConvert.DeserializeObject(Of T)(cacheValue)
                'If needDebugGetKeyOfT Then ElmahExtension.LogToElmah(New Exception("F 2"))
            End If
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
        End Try
        Return result
    End Function


#End Region

    Public Function Expire(ByVal key As String, ByVal value As Integer) As Integer
        Dim cache As IDatabase = Connection.GetDatabase()
        Return cache.KeyExpire(key, TimeSpan.FromSeconds(value))
    End Function

    Public Function TTL(ByVal key As String) As Integer
        Dim cache As IDatabase = Connection.GetDatabase()
        Dim remainingTTL As TimeSpan = cache.KeyTimeToLive(key)
        Return remainingTTL.TotalSeconds
    End Function

    Public Function DEL(ByVal key As String) As Integer
        Dim cache As IDatabase = Connection.GetDatabase()
        Return cache.KeyDelete(key)
    End Function



    Public Function SAdd(ByVal key As String, ByVal value As String) As Integer
        Dim cache As IDatabase = Connection.GetDatabase()
        'cache.SetAdd(key, value)
        Return cache.SetAdd(key, RedisSetHelper.GetBytes(value)) 'SourceClient.SAdd(key, Helper.GetBytes(value))
    End Function

    Public Function SMembers(ByVal key As String) As List(Of String)
        Dim cache As IDatabase = Connection.GetDatabase()

        Dim members As New List(Of String)
        For Each mem As Byte() In cache.SetMembers(key)
            members.Add(RedisSetHelper.GetString(mem))
        Next
        Return members
    End Function

    Private Class RedisSetHelper
        Private Shared ReadOnly UTF8EncObj As New Text.UTF8Encoding()

        Public Shared Function GetBytes(ByVal source As Object) As Byte()
            Return UTF8EncObj.GetBytes(source)
        End Function

        Public Shared Function GetString(ByVal sourceBytes As Byte()) As String
            Return UTF8EncObj.GetString(sourceBytes)
        End Function

    End Class
End Class