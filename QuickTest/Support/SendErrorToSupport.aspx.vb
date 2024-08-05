Imports System.Data.SqlClient

Public Class SendErrorToSupport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("UserId") IsNot Nothing AndAlso Request.QueryString("QuestionId") IsNot Nothing Then
            ViewState("UserId") = Request.QueryString("UserId").ToString()
            ViewState("QuestionId") = Request.QueryString("QuestionId").ToString()
        Else
            Response.Redirect("~/Loginpage.aspx")
        End If
    End Sub

    Private Sub btnSendError_Click(sender As Object, e As EventArgs) Handles btnSendError.Click
        If SendErrorFromQuestion(txtMessage.Text, ViewState("UserId"), ViewState("QuestionId")) = True Then
            lblComplete.Visible = True
        End If
    End Sub

    Private Function SendErrorFromQuestion(Message As String, UserId As String, QuestionId As String) As Boolean
        Dim db As New ClassConnectSql()
        Dim conn As New SqlConnection()
        db.OpenExclusiveConnect(conn)
        Try
            Dim sql As String = " INSERT INTO dbo.tblErrorSupport( ErrorSupportId ,UserId ,Question_Id ,Message ,LastUpdate ,IsActive ) " &
                                " VALUES  ( NEWID() , '" & UserId & "' , '" & QuestionId & "' ,'" & db.CleanString(Message) & "' ,dbo.GetThaiDate() ,1 ); "
            db.Execute(sql, conn)
            db.CloseExclusiveConnect(conn)
            db = Nothing
            Return True
        Catch ex As Exception
            db.CloseExclusiveConnect(conn)
            db = Nothing
            Return False
        End Try
    End Function

End Class