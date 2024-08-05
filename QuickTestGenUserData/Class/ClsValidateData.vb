Public Class ClsValidateData
    Public Function OrderbyDatatable(dt As DataTable, ColumnIdName As String) As DataTable
        Dim Orderdt As DataTable
        Dim dataView As New DataView(dt)

        dataView.Sort = ColumnIdName & " ASC"

        Orderdt = dataView.ToTable()

        Return Orderdt
    End Function

End Class
