Public Class QuizLayoutConfirmed
    Inherits System.Web.UI.Page
    'Class ที่ใช้เกี่ยวกับการ select,update ข้อมูล ใช้กับหน้านี้ และ WordLayoutConfirmed
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

    ''' <summary>
    ''' ทำการ Bind Repeater ข้อสอบชุดนี้ แสดงว่าข้อนี้ มีการแก้ไข หรือ อนุมัติบ้างหรือยัง ในรูปแบบ Quiz
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("qsetid") IsNot Nothing Then
            Qset_Id = Request.QueryString("qsetid").ToString()
            clslayoutcheck.BindRepeater(Qset_Id, "Quiz", rptListQuestion, True)
        End If
    End Sub

End Class