Public Class TeacherDTO
    Public Property School_Code As String
    Public Property Teacher_Id As Guid?
    Public Property Teacher_Code As String
    Public Property Teacher_PrefixName As String
    Public Property Teacher_FirstName As String
    Public Property Teacher_LastName As String
    Public Property Teacher_Phone As String
    Public Property Teacher_Status As Byte?
    Public Property Teacher_CurrentClass As String
    Public Property Teacher_CurrentRoom As String
    Public Property Teacher_Number As String
    Public Property Teacher_Soi As String
    Public Property Teacher_Street As String
    Public Property SubDistrict_Id As Integer?
    Public Property District_Id As Integer?
    Public Property Province_Id As Integer?
    Public Property Teacher_IsActive As Boolean?
    Public Property RowNumber As Integer?
    Public Property Calendar_Id As Guid?
    Public Property UserName As String
    Public Property EditPassword As String
 
    Public Sub New()
        'ระบุค่า Default
        Teacher_IsActive = EnIsActiveStatus.Active
    End Sub

End Class
