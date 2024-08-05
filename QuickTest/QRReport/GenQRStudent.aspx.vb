Public Class GenQRStudent
    Inherits System.Web.UI.Page
    Public StudentId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Request.QueryString("StudentId") IsNot Nothing And HttpContext.Current.Request.QueryString("StudentId").ToString() <> "" Then
            StudentId = HttpContext.Current.Request.QueryString("StudentId").ToString()
        Else
            Response.Write("ไม่มี StudentId")
        End If

    End Sub

End Class