Imports System.Web.Script.Serialization

Public Class MoveEngExam
    Inherits System.Web.UI.Page
    Protected Shared clslayoutcheck As New ClsLayoutCheckConfirmed()


    Public Property Qset_Id As String
        Get
            Return ViewState("_Qset_Id")
        End Get
        Set(ByVal value As String)
            ViewState("_Qset_Id") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsME As New clsMoveExam()
        If Request.QueryString("qsetid") IsNot Nothing Then
            Qset_Id = Request.QueryString("qsetid").ToString()
            clsME.BindRepeaterMoveExam(Qset_Id, rptListQuestion)
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function GetQCat(ByVal LevelId As String, QuestionId As String)
        Dim clsME As New clsMoveExam()
        Dim dt As DataTable = clsME.GetNewQcatByLevel(LevelId, QuestionId)
        Dim JsonString As New ArrayList

        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                JsonString.Add(New With {.val = dt.Rows(i)("QCatId").ToString, .text = dt.Rows(i)("QCatName").ToString, .selected = dt.Rows(i)("MId").ToString})
            Next
        End If

        Dim js As New JavaScriptSerializer()
        Return js.Serialize(JsonString)
    End Function

    <Services.WebMethod()>
    Public Shared Function GetQSet(ByVal QCatId As String, QuestionId As String)
        Dim clsME As New clsMoveExam()
        Dim dt As DataTable = clsME.GetNewQSetByQCat(QCatId, QuestionId)
        Dim JsonString As New ArrayList

        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                JsonString.Add(New With {.val = dt.Rows(i)("QSetId").ToString, .text = dt.Rows(i)("QSetName").ToString, .selected = dt.Rows(i)("MId").ToString})
            Next
        End If

        Dim js As New JavaScriptSerializer()
        Return js.Serialize(JsonString)
    End Function

    <Services.WebMethod()>
    Public Shared Function UpdateQset(ByVal QSetId As String, QuestionId As String)
        Try
            Dim clsME As New clsMoveExam()
            clsME.UpdateQuestionSet(QSetId, QuestionId)
            Return "1"
        Catch ex As Exception
            Return "0"
        End Try

    End Function

    <Services.WebMethod()>
    Public Shared Function CheckSeletedLevel(QuestionId As String)
        Try
            Dim clsME As New clsMoveExam()
            Dim slevelId As String = clsME.GetSelectedLevelbyQuestion(QuestionId)
            Return slevelId.ToUpper
        Catch ex As Exception
            Return "0"
        End Try

    End Function




End Class