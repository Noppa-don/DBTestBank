Public Class ExamMatchTemplate
    Inherits System.Web.UI.Page
    Shared Index As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)
    Dim Size() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If

        If Not IsPostBack Then
            Dim FontSize As String = cls.GetFontSize(ClsPDF.FontSize.Small)
            Size = Split(FontSize, ",")

            Dim dtQuestion As System.Data.DataTable
            dtQuestion = cls.GetQuestion(Request.QueryString("id").ToString)
            For Each i In dtQuestion.Rows
                i("Question_Name") = "<td style=""font-size: " & Size(2) & "pt; font-family: Angsana New;"">" & i("Question_Name")
            Next
            QuestionListing.DataSource = dtQuestion
            QuestionListing.DataBind()

            Dim dtAns As DataTable = cls.GetMatchAnswerDetails(Request.QueryString("id").ToString)
            Dim sb As New System.Text.StringBuilder
            Dim Choice() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ณ", "ญ", "ด", "ต", "ถ", "ท", "ธ"}

            For i = 0 To dtAns.Rows.Count - 1
                dtAns.Rows(i)("answer_Name") = "<td style=""width: 80px;font-size: " & Size(2) & "pt;valign=""top"" style=""padding-top: 5px;"">" & Choice(i) & "</td><td style=""font-size: " & Size(2) & "pt; font-family: Angsana New;"">" & dtAns.Rows(i)("answer_Name").ToString() & "</td>"
                'dtAns.Rows(i)("answer_Name") = "<td style=""font-size: " & Size(2) & "pt; font-family: Angsana New;"">" & dtAns.Rows(i)("answer_Name").ToString() & "</td>"

            Next

            Repeater1.DataSource = dtAns
            Repeater1.DataBind()
            Index = 0
        End If

    End Sub

    Public Function GetQuestionNo() As String
        Index += 1
        Return "<td valign=""top"" style=""padding-top: 5px; font-family: Angsana New;font-size: " & Size(2) & "pt;"" >" & Index
    End Function

End Class