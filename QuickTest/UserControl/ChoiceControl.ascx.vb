Public Class ChoiceControl
    Inherits System.Web.UI.UserControl
    Shared Index As Integer = 0
    Shared Count As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)

    Dim dt As System.Data.DataTable

    Private _QSetId As String
    Dim QSetName As String
    Private _TestSetId As String
    Private _IsFirstControl As Boolean
    Public Property IsFirstControl As Boolean
        Set(ByVal value As Boolean)
            _IsFirstControl = value

        End Set
        Get
            Return _IsFirstControl
        End Get
    End Property
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then



            ' ''        'd:\files\8\C\E\5\B\{8CE5B734-24C9-4A79-A8B3-A968C5F34CC3}_13-18.jpg
            ' ''        'd:\files\8\C\E\5\B\{8CE5B734-24C9-4A79-A8B3-A968C5F34CC3}_13-18.jpg
            ' ''        Dim q_name As String = dr(qestion_name).tostring()
            ' ''        q_name = qname.replace("___MODULE_URL___", genFilePath(dr(qset_id).tostring()))
            ' ''        'qname = "./images/files/8/C\E\5\B\{8CE5B734-24C9-4A79-A8B3-A968C5F34CC3}_13-18.jpg

            ' ''function genFilePath(currentQSetId as string)
            ' ''    Dim rootPath As String = "./images/"
            ' ''    Dim filePath As String
            ' ''    filePath = currentQSetId.Substring(1, 1) + "/" + currentQSetId.Substring(2, 1) + "/" + currentQSetId.Substring(3, 1) + "/" + currentQSetId.Substring(4, 1) + "/" + currentQSetId.Substring(5, 1)
            ' ''    filePath = filePath + "{" + currentQSetId + "}_"
            ' ''    Return rootPath + filePath
            ' ''end funtion 

            For Each i In dt.Rows

                i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
                Dim spanCol As String = "2"
                If Session("IsAnswerSheet") = True Then
                    spanCol = "3"
                End If
                i("Question_Name") = "<td colspan=""" & spanCol & """ class=""questiontext"">" & cls.CleanQuestionText(i("Question_Name")) & "</td>"
            Next
            QuestionListing.DataSource = dt
            QuestionListing.DataBind()
            Index = 0
            ltQSetName.Text = "<font class=""setname""><B>" & cls.CleanSetNameText(QSetName) & " </b></font>"
        End If
    End Sub

    Public Function GetQuestionNo() As String
        Index += 1
        Return "<td valign=""top"" class=""questionno"" >" & Index & ".</td>"
    End Function

    Public Function GetAnswerDetails(ByVal questionId) As String


        Dim dt As DataTable = cls.GetAnswerDetails(questionId)
        Dim sb As New System.Text.StringBuilder
        Dim Choice() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ณ", "ญ", "ด", "ต", "ถ", "ท", "ธ", "น", "บ", "ป", "ผ", "ฝ", "พ"}

        For i = 0 To dt.Rows.Count - 1
            Dim Answer As String = dt.Rows(i)("Answer_Name")
            Answer = cls.CleanAnswerText(Answer)
            Answer = Answer.Replace("___MODULE_URL___", cls.GenFilePath(_QSetId))
            Dim Score As String = dt.Rows(i)("Answer_Score")
            If Session("IsAnswerSheet") = True Then
                If Score = "1" Then
                    Score = "X  "
                Else
                    Score = " "
                End If
                sb.Append("<tr><td class=""answerbulletTF"">" & Score & "</td><td class=""answerbullet"">" & Choice(i) & "</td><td class=""questiontext"">" & LTrim(Answer) & "</td></tr>")
            Else
                sb.Append("<tr><td class=""answerbullet"">" & Choice(i) & "</td><td class=""questiontext"">" & LTrim(Answer) & "</td></tr>")
            End If

        Next
        GetAnswerDetails = sb.ToString()
    End Function
 
    Public Function InsertPageBreakTag()
        If _IsFirstControl Then
            Return String.Empty
        Else
            Return "<div style=""page-break-before:always;""> &nbsp; </div> "
        End If
    End Function

End Class