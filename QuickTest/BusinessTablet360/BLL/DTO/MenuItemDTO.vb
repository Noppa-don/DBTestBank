Public Class MenuItemDTO

    Public Property MenuItem_Code As String
    Public Property MenuItem_Parent As String
    Public Property MenuItem_Type As Byte?
    Public Property MenuItem_IsActive As Boolean?

    Public Sub New()
        MenuItem_IsActive = True
    End Sub

End Class
