Public Class MoveExamPage
    Inherits System.Web.UI.Page
    Protected Shared clslayoutcheck As New ClsLayoutCheckConfirmed()

    'เก็บ QsetId เอาไว้ใช้กับหน้า Javascript ด้วย
    Public Property Qset_Id As String
        Get
            Return ViewState("_Qset_Id")
        End Get
        Set(ByVal value As String)
            ViewState("_Qset_Id") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("qsetid") IsNot Nothing Then
            Qset_Id = Request.QueryString("qsetid").ToString()
            clslayoutcheck.BindRepeater(Qset_Id, "Quiz", rptListQuestion, True)
        End If
    End Sub

End Class