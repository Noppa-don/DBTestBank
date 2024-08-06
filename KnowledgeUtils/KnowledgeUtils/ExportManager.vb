Imports KnowledgeUtils.IO
Imports System.Text
Imports BusinessTablet360

Namespace Export

    ''' <summary>
    ''' คลาสช่วยการเรื่อง Export รูปแบบต่างๆ
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ExportManager

        Public Shared Function ExportCSV(ByVal Source As DataTable, ByVal Head As IEnumerable(Of String), ByVal FilePath As String) As Boolean
            Try
                Dim Result As String = "ไม่พบข้อมูล"

                Dim Sb As New StringBuilder
                If Source.Rows.Count > 0 Then
                    Sb.Append(String.Join(",", Head) & vbNewLine)

                    For Each Row As DataRow In Source.Rows
                        Dim Datas As New List(Of Object)
                        For index = 0 To Row.Table.Columns.Count - 1
                            Dim Data As String = IIf(Row.Item(index) Is DBNull.Value, "", Row.Item(index).ToString)
                            Datas.Add(Data)
                        Next
                        Sb.Append(String.Join(",", Datas) & vbNewLine)
                    Next
                    Result = Sb.ToString
                End If

                Dim Fi As New ManageFile(FilePath)
                Fi.CreateFile(TextInput:=Result)
                Return True
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

    End Class

End Namespace


