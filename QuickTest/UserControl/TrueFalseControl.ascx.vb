Public Class TrueFalseControl
    Inherits System.Web.UI.UserControl
    Shared Index As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)

    Dim dt As System.Data.DataTable


    Dim QSetName As String

    Private _QSetId As String
    Public Property QSetId() As String
        Get
            Return _QSetId
        End Get
        Set(ByVal value As String)
            _QSetId = value

            QSetName = cls.GetQSetName(value)
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

    Private _TestSetId As String
    Public Property TestSetId() As String
        Get
            Return _TestSetId
        End Get
        Set(ByVal value As String)
            _TestSetId = value
            dt = cls.GetQuestion(value, _QSetId, Session("IsPreviewPage"))
        End Set
    End Property

    Private _PDFType As String
    Public Property PDFType() As String
        Get
            Return _PDFType
        End Get
        Set(ByVal value As String)
            _PDFType = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
 

            Dim dtQuestion As System.Data.DataTable
            dtQuestion = cls.GetQuestion(TestSetId, QSetId, Session("IsPreviewPage"))
            For Each i In dt.Rows
                i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
                i("Question_Name") = "<td class=""questiontext"">" & cls.CleanQuestionText(i("Question_Name")) & "</td>"
            Next
            QuestionListing.DataSource = dt
            QuestionListing.DataBind()
            Index = 0
 
            ltQSetName.Text = "<font class=""setname""><B>" & cls.CleanSetNameText(QSetName) & " </b></font>"


        End If
    End Sub
    Public Function GetQuestionNo() As String
        Index += 1
        Return "<td class=""MatchAnswer"">" & Index & ". </td>"
    End Function

 

    Public Function GetAnswer(ByVal Question_Id As String) As String
        If Session("IsAnswerSheet") Then
            Dim dt As DataTable = cls.GetAnswerDetails(Question_Id)
            If dt.Rows(0)("Answer_Name") = "True" Then
                Return "<td class=""AnswerSheetMath""> / </td>"
            Else
                Return "<td class=""AnswerSheetMath""> X </td>"
            End If
        Else
            Return "<td class=""setname""> ___ </td>"
        End If
        Return ""
    End Function

    Public Function InsertPageBreakTag()
        If _IsFirstControl Then
            Return String.Empty
        Else
            Return "<div style=""page-break-before:always;""> &nbsp; </div> "
        End If
    End Function


End Class