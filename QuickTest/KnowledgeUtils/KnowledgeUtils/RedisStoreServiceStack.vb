Imports ServiceStack.Redis

Public Class RedisStoreOld

    Private _sourceClient As RedisClient
    Public ReadOnly Property SourceClient() As RedisClient
        Get
            Return _sourceClient
        End Get
    End Property


#Region "Constructors"
    Public Sub New()
        MyClass.New(False)
    End Sub

    Public Sub New(ByVal ForceCheckServer As Boolean)
        _sourceClient = New RedisClient
        If ForceCheckServer AndAlso Not IsServiceAlive() Then
            Throw New Exception("This server has not been started!")
        End If
    End Sub

    Public Sub New(RedisConnectionString As String)
        _sourceClient = New RedisClient(RedisConnectionString)
    End Sub


#End Region

    Private Function IsServiceAlive() As Boolean
        Try
            Return SourceClient.Ping
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

#Region "Get/Set Keys"
    Public Function SetKey(ByVal key As String, ByVal value As String) As Boolean
        Return SourceClient.Set(key, value)
    End Function

    Public Function SetKey(Of T)(ByVal key As String, ByVal value As T) As Boolean
        Return SourceClient.Set(Of T)(key, value)
    End Function

    Public Function Getkey(ByVal key As String) As String
        If SourceClient.Get(key) Is Nothing Then
            Return ""
        End If
        Return Helper.GetString(SourceClient.Get(key))
    End Function

    Public Function Getkey(Of T)(ByVal key As String) As T
        If SourceClient.Get(Of T)(key) Is Nothing Then
            Return Nothing
        End If
        Return SourceClient.Get(Of T)(key)
    End Function
#End Region

#Region "Hash Get/Set"
    'Public Function HMSet(ByVal key As String, ByVal field As Array, ByVal value As Array) As Boolean
    '    Dim a As Array = {"", ""}
    '    Return SourceClient.HMSet(key, a, a)
    'End Function
#End Region

    Public Function SAdd(ByVal key As String, ByVal value As String) As Integer
        Return SourceClient.SAdd(key, Helper.GetBytes(value))
    End Function

    Public Function SMembers(ByVal key As String) As List(Of String)
        Dim members As New List(Of String)
        For Each mem As Byte() In SourceClient.SMembers(key)
            members.Add(Helper.GetString(mem))
        Next
        Return members
    End Function

    Public Function Expire(ByVal key As String, ByVal value As Integer) As Integer
        Return SourceClient.Expire(key, value)
    End Function

    Public Function TTL(ByVal key As String) As Integer
        Return SourceClient.Ttl(key)
    End Function

    Public Function DEL(ByVal key As String) As Integer
        Return SourceClient.Del(key)
    End Function

End Class

Public Class Helper
    Private Shared ReadOnly UTF8EncObj As New Text.UTF8Encoding()

    Public Shared Function GetBytes(ByVal source As Object) As Byte()
        Return UTF8EncObj.GetBytes(source)
    End Function

    Public Shared Function GetString(ByVal sourceBytes As Byte()) As String
        Return UTF8EncObj.GetString(sourceBytes)
    End Function

End Class
