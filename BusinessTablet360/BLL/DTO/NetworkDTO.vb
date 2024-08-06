Public Class NetworkDTO

    Public Property School_Code As String
    Public Property Network_Id As Guid?
    Public Property Network_IP As String
    Public Property Network_IP_Like As String
    Public Property Network_Name As String
    Public Property Network_Name_Like As String
    Public Property Network_Type As String
    Public Property Network_FirstDate As Date?
    Public Property Network_FirstDateString As String
    Public Property Network_LastDate As Date?
    Public Property Network_LastDateString As String
    Public Property Network_CreateDate As Date?
    Public Property Network_CreateDateString As String
    Public Property Network_PassDay As Integer?
    Public Property FoundDay As Integer?
    Public Property NotFoundDay As Integer?
    Public Property Network_IsActive As Boolean?
    Public Property RowNumber As Integer?
    Public Property DayPass As Integer?
    Public Property Network_Location As String
    Public Property Network_IsIgnore As Boolean
    Public Property IsIgnore As String
    Public Property DisconectedTimeString As String
    Public Property Status As String

    Public Sub New()
        Network_IsActive = True
    End Sub

End Class
