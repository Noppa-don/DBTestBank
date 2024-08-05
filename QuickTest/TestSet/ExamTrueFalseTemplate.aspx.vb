Public Class ExamTrueFalseTemplate
    Inherits System.Web.UI.Page

    Dim cls As New ClsPDF(New ClassConnectSql)
    Shared Index As Integer = 0
    Dim Size() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If

        If Not IsPostBack Then

            Dim FontSize As String = cls.GetFontSize(ClsPDF.FontSize.Small)
            Size = Split(FontSize, ",")

            Dim dt As System.Data.DataTable
            dt = cls.GetQuestion(Request.QueryString("id"))
            For Each i In dt.Rows
                i("Question_Name") = "<td style=""text-align: left;font-family: Angsana New;font-size: " & Size(2) & "pt;"">" & i("Question_Name") & "</td>"
            Next
            QuestionListing.DataSource = dt
            QuestionListing.DataBind()
            Index = 0

        End If
    End Sub

    Public Function GetQuestionNo() As String
        Index += 1
        Return "<td valign=""top"" style=""padding-top: 5px; font-family: Angsana New;font-size: " & Size(2) & "pt;"" >" & Index
    End Function


End Class