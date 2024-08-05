Public Class UserDTO

    Public Property School_Code As String
    Public Property User_Id As Guid?
    Public Property User_Name As String
    Public Property User_IsActive As Boolean?
    Public Property MenuItem_Code As Short?

    Public Sub New()
        User_IsActive = True
    End Sub

End Class
