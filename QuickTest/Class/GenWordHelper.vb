Public Module GenWordHelper

    Dim d As New DataTable

    Public Function Indicators(dt As DataTable) As String
        d = dt
        Dim sb As New StringBuilder()
        sb.Append("<table>")
        sb.Append(IndicationsTable())
        sb.Append("</table>")
        Return sb.ToString()
    End Function

    Private Function IndicationsTable() As String
        Dim sb As New StringBuilder()
        sb.Append("<tr><td colspan='3'>" & CommonTexts.Course51 & "</td></tr>") 'หลักสูตรแกนกลาง

        Dim dt As DataTable = d.AsEnumerable().Where(Function(r) r.Field(Of String)("ParentEI_Code") = CommonTexts.Indicators).AsEnumerable()
        Dim tempEiId As Guid
        Dim tempEiId2 As Guid
        For Each r In dt.Rows
            If tempEiId = Guid.Empty OrElse tempEiId <> r("Child1EI_Id") Then 'สาระ
                sb.Append(String.Format("<tr><td></td><td colspan='2'>{0} {1}</td></tr>", r("Child1EI_Code"), r("Child1EI_Name")))
                tempEiId = r("Child1EI_Id")
            End If
            If tempEiId2 = Guid.Empty OrElse tempEiId2 <> r("Child2EI_Id") Then 'มาตรฐาน
                sb.Append(String.Format("<tr><td></td><td colspan='2'>{0} {1}</td></tr>", r("Child2EI_Code"), r("Child2EI_Name")))
                tempEiId2 = r("Child2EI_Id")
            End If
            sb.Append(String.Format("<tr><td></td><td></td><td>{0} {1}</td></tr>", r("EI_Code"), r("EI_Name"))) ' ลูกมาตรฐาน
        Next
        Return sb.ToString()
    End Function

    Public Function KPA(dt As DataTable) As String
        Dim s As New StringBuilder()
        s.Append(CommonTexts.KPA)
        s.Append(GetKPA(dt, CommonTexts.K))
        s.Append(GetKPA(dt, CommonTexts.P))
        s.Append(GetKPA(dt, CommonTexts.A))
        Return s.ToString()
    End Function

    Private Function GetKPA(dt As DataTable, Text As String) As String
        Dim dtTemp As DataTable = dt.AsEnumerable().Where(Function(r) r.Field(Of String)("IsWpp") = Text).AsEnumerable()
        Dim s As New StringBuilder()
        If dtTemp.Rows.Count > 0 Then
            s.Append(Text)
            For Each r As DataRow In dtTemp.Rows
                s.Append(r("eicode"))
            Next
            Return s.ToString()
        End If
        Return ""
    End Function

    Public Function NationalTest(dt As DataTable) As String
        Dim s As New StringBuilder()
        s.Append(CommonTexts.NationalTest)
        For Each r As DataRow In dt.Rows
            s.Append(r(""))
        Next
        Return s.ToString()
    End Function

    Public Function LevelOfTest(dt As DataTable) As String
        Dim s As New StringBuilder()
        s.Append(CommonTexts.LevelOfTest)
        For Each r As DataRow In dt.Rows
            s.Append(r(""))
        Next
        Return s.ToString()
    End Function
End Module

