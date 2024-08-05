Public Class ExamChoiceTemplate
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

            ''litCustomerName.Text = "<B> ธัญญภรณ์ ธรรมนารักษ์ ที่ </B> <img src = 'Images/book_icon.png' />"
            'litSchoolName.Text = "<font size='" & SizeC(0) & "'><B>โรงเรียนโพธิสารพิทยากร </B>" '
            'litExamDetail.Text = "<font size='" & SizeC(1) & "'>ข้อสอบกลางภาค กลุ่มสาระวิชาภาษาไทย ชั้น ม.1" '
            'litExamAmount.Text = "<font size='" & SizeC(1) & "'>จำนวน 10 ข้อ" '
            'litTotalTime.Text = "<font size='" & SizeC(1) & "'>เวลา 120 นาที" '

            Dim dt As System.Data.DataTable
            'dt = cls.GetQuestion(Request.QueryString("id").ToString)
            For Each i In dt.Rows
                i("Question_Name") = "<td colspan=""2"" style=""text-align: left;font-size: " & Size(2) & "pt;"">" & i("Question_Name") & "</td>"
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

    Public Function GetAnswerDetails(ByVal questionId) As String

        Dim dt As DataTable = cls.GetAnswerDetails(questionId)
        Dim sb As New System.Text.StringBuilder
        Dim Choice() As String = {"ก.", "ข.", "ค.", "ง.", "จ."}

        For i = 0 To dt.Rows.Count - 1
            Dim Answer As String = dt.Rows(i)("Answer_Name")
            sb.Append("<tr><td style=""width: 80px;font-size: " & Size(2) & "pt;"">" & Choice(i) & ".</td><td style=""text-align: left;font-size: " & Size(2) & "pt;"">" & LTrim(Answer) & "</td></tr>")
        Next '<font size="  & ">"
        GetAnswerDetails = sb.ToString()
    End Function

End Class