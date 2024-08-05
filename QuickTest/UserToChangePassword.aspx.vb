Imports System.Web
Public Class UserToChangePassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            lblSchoolId.Text = Session("SchoolID")
            lblUserName.Text = Session("UserName")
        End If

    End Sub

    <Services.WebMethod()>
    Public Shared Function checkOldPwdCodeBehind(ByVal oldPwd As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim sql As String
        Dim checkPwd As String = ""
        'HttpContext.Current.Session("UserName")
        sql = "select * from tbluser where UserName='" & HttpContext.Current.Session("UserName") & "' and password='" & Encryption.MD5(oldPwd) & "'  and IsActive=1 and SchoolID='" & HttpContext.Current.Session("SchoolID") & "';"
        checkPwd = db.ExecuteScalar(sql)

        If checkPwd = "" Then
            Return "error"
        Else
            Return "pass"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function changePwdCodeBehind(ByVal newPwd As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = "update tblUser set Password = '" & Encryption.MD5(newPwd) & "',Lastupdate = dbo.GetThaiDate() where UserName='" & HttpContext.Current.Session("UserName") & "' and SchoolId='" & HttpContext.Current.Session("SchoolID") & "' and IsActive='1' ; "

        Try
            db.Execute(sql)
            Log.Record(Log.LogType.ChangePassword, "เปลี่ยนรหัสผ่าน", True)
        Catch ex As Exception

        End Try
        Return ""
    End Function
End Class