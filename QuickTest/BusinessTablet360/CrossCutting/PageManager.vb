
Public MustInherit Class PageManager

    Public MustOverride Sub DisplayUIDynamic(ByVal Status As EnStatusUI)
    Public MustOverride Function ValidatePage(ByVal Status As EnStatusUI) As Boolean
    Public MustOverride Sub BindGrid(Optional ByVal PageIndex As Integer = 0)

End Class
