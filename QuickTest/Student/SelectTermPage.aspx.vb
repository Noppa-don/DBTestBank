Public Class SelectTermPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CreateDivTerm()
        End If
    End Sub

    Private Sub CreateDivTerm()

        Dim dt As DataTable = GetTermBySchoolCode(Session("SchoolID").ToString())
        Dim sb As New StringBuilder
        If dt.Rows.Count > 0 Then
            sb.Append(" <h1 style='font-size:50px;margin:0 0 10px 0;'>เทอม</h1>")
            For index = 0 To dt.Rows.Count - 1
                Dim Counting As Integer = index + 1
                Dim CalendarId As String = dt.Rows(index)("Calendar_Id").ToString()
                Dim CalendarName As String = dt.Rows(index)("Calendar_Name")
                Dim CalendarYear As String = dt.Rows(index)("Calendar_Year").ToString()
                sb.Append("<div id='DivTerm" & Counting & "' class='ForDivTerm' TermId='" & CalendarId & "' >")
                sb.Append(CalendarName & "/" & CalendarYear)
                sb.Append("</div>")
            Next
            MainDivTerm.InnerHtml = sb.ToString()
        End If

    End Sub

    Public Function GetTermBySchoolCode(SchoolCode As String)

        Dim sql As String = " select Calendar_Id,Calendar_Name,Calendar_Year from t360_tblCalendar " & _
                            " where School_Code = '" & SchoolCode & "'  and Calendar_Type = 3 AND Isactive = 1 order by Calendar_Year desc,Calendar_Name desc ; "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

End Class