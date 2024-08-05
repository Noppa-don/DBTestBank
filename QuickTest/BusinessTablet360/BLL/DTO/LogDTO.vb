Public Class LogDTO
    Public Property Log_Id As System.Guid
    Public Property Log_Type As Byte
    Public Property School_Code As String
    Public Property User_Id As System.Guid
    Public Property Log_Description As String
    Public Property Log_Page As String
    Public Property LastUpdate As Date
    Public Property Log_IsActive As Boolean
    Public Property Calendar_Id As System.Nullable(Of System.Guid)
    Public Property ClientId As String
    Public Property UserName As String
    Public Property Log_TypeName As String
    Public Property Log_TypeImage As String
    Public Property Log_DescriptionCutString As String
End Class
