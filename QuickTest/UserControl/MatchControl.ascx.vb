Public Class MatchControl
    Inherits System.Web.UI.UserControl
    Shared Index As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)

    Dim dt As System.Data.DataTable
    Dim Choice = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ณ", "ญ", "ด", "ต", "ถ", "ท", "ธ", "น", "บ", "ป", "ผ", "ฝ", "พ"}.ToList
    Dim newChoice As New List(Of String)
    Dim Answer As New List(Of String)

    Private _QSetId As String
    Dim QSetName As String
    Private _TestSetId As String
    Public Property QSetId() As String
        Get
            Return _QSetId
        End Get
        Set(ByVal value As String)
            _QSetId = value

            QSetName = cls.GetQSetName(value)
        End Set
    End Property

    Public Property TestSetId() As String
        Get
            Return _TestSetId
        End Get
        Set(ByVal value As String)
            _TestSetId = value
           
            dt = cls.GetQuestion(value, _QSetId, Session("IsPreviewPage"))


        End Set
    End Property
    Private _IsFirstControl As Boolean
    Public Property IsFirstControl As Boolean
        Set(ByVal value As Boolean)
            _IsFirstControl = value

        End Set
        Get
            Return _IsFirstControl
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
 
            ltQSetName.Text = "<font class=""setname""><B>" & cls.CleanSetNameText(QSetName) & " </b></font>"

            If Session("IsAnswerSheet") Then
                GetRandomAnswer()
            End If


            Dim dtQuestion As System.Data.DataTable

            dtQuestion = cls.GetQuestion(TestSetId, QSetId, Session("IsPreviewPage"))


            For Each i In dtQuestion.Rows
                i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
                i("Question_Name") = "<td class=""setname"">" & cls.CleanQuestionText(i("Question_Name")) & "</td>"
            Next
            QuestionListing.DataSource = dtQuestion
            QuestionListing.DataBind()

            Dim dtAns As DataTable = cls.GetMatchAnswerDetails(QSetId, TestSetId)
            Dim dtAnsRand As New DataTable
            dtAnsRand.Columns.Add("answer_Name")
            Dim dr As DataRow
            Dim sb As New System.Text.StringBuilder
            If Session("IsAnswerSheet") Then
                Dim a As String
                For i = 0 To dtAns.Rows.Count - 1
                    a = Answer.FindIndex(Function(value As String)
                                             Return value(0) = Choice(i)
                                         End Function)
                    dr = dtAnsRand.NewRow
                    Dim ans = dtAns.Rows(a)("answer_Name").ToString()
                    ans = cls.CleanAnswerText(ans)
                    ans = ans.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
                    dr("answer_Name") = "<td class=""MatchAnswer"">" & Choice(i) & "</td><td class=""setname"">" & ans & "</td>"
                    dtAnsRand.Rows.Add(dr)
                 Next
            Else
                For i = 0 To dtAns.Rows.Count - 1
                    dr = dtAnsRand.NewRow
                    Dim ans = dtAns.Rows(i)("answer_Name").ToString()
                    ans = cls.CleanAnswerText(ans)
                    ans = ans.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
                    dr("answer_Name") = "<td class=""MatchAnswer"">" & Choice(i) & "</td><td class=""setname"">" & ans & "</td>"
                    dtAnsRand.Rows.Add(dr)
                 Next
            End If


            Repeater1.DataSource = dtAnsRand
            Repeater1.DataBind()
            Index = 0

        End If
    End Sub
    Public Function GetQuestionNo() As String

        Index += 1
        If Session("IsAnswerSheet") Then
            If Answer.Count > 0 Then 'พี่ชินเพิ่มตัวดักไว้, เพราะเห็นมันมี error โดย Array - answer มันมี 0 item ทำให้ index outofrange
                Return "<td class=""AnswerSheetMath"" >" & Answer(Index - 1) & "   " & "</td><td class=""MathAnswer"">" & Index & ".  </td>"
            Else
                Return ""
            End If
        Else
            Return "<td class=""setname""> ___" & Index & ".  </td>"
        End If


            Return ""
    End Function

    Public Sub GetRandomAnswer()
        Dim dtAns As DataTable = cls.GetMatchAnswerDetails(QSetId, TestSetId)
        Dim AnswerAmount As Integer = dtAns.Rows.Count
        Dim Ran As New Random

        For j = 0 To AnswerAmount - 1
            newChoice.Add(Choice(j))
        Next

        If Not newChoice.Count = 0 Then
            For I = 1 To AnswerAmount
                Dim Position = Ran.Next(0, AnswerAmount)
                Answer.Add(newChoice(Position))

                newChoice.Remove(newChoice(Position))
                AnswerAmount -= 1
            Next
        End If
        Session("newChoice") = newChoice
    End Sub

    Public Function InsertPageBreakTag()
        If _IsFirstControl Then
            Return String.Empty
        Else
            Return "<div style=""page-break-before:always;""> &nbsp; </div> "
        End If
    End Function

End Class