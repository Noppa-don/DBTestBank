Public Class AddChild2
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql
    Public DeviceId As String

    Public Sendpage As String

    Private SchoolCode As String
    Private ParentCode As String
    Private ParentFirstName As String
    Private ParentLastName As String
    Private ParentPhone As String

    Private Property StudentId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DeviceId = Request.QueryString("DeviceId").ToString()
        Sendpage = Request.QueryString("Sendpage").ToString()
        
    End Sub

    Private Function AddChildAndParent() As Boolean
        Dim sql As New StringBuilder
        sql.Append(" DECLARE @p AS UNIQUEIDENTIFIER = NEWID();")
        sql.Append(" DECLARE @studid AS UNIQUEIDENTIFIER = (SELECT Student_Id FROM tblParentCode WHERE IsActive = 1 AND School_Code = '{4}' AND ParentCode = '{5}');")
        sql.Append(" INSERT INTO tblParent VALUES (@p,'{0}','{1}','{2}','{3}',dbo.GetThaiDate(),1,NULL,'{4}'); ")
        sql.Append(" INSERT INTO tblStudentParent VALUES (NEWID(),@p,@studid,dbo.GetThaiDate(),1,NULL,'{4}'); ")
        sql.Append(" INSERT INTO tblStudentPhoto VALUES (NEWID(),1,@studid,dbo.GetThaiDate(),1,NEWID(),1,dbo.GetThaiDate(),1,NULL);")
        db.ExecuteWithTransection(String.Format(sql.ToString(), ParentFirstName, ParentLastName, ParentPhone, DeviceId, SchoolCode, ParentCode))
    End Function

    Private Function AddChild(ParentId As String) As Boolean
        Dim sql As New StringBuilder
        sql.Append(" DECLARE @p AS UNIQUEIDENTIFIER = '{0}';")
        sql.Append(" DECLARE @studid AS UNIQUEIDENTIFIER = (SELECT Student_Id FROM tblParentCode WHERE IsActive = 1 AND School_Code = '{1}' AND ParentCode = '{2}');")
        sql.Append(" INSERT INTO tblStudentParent VALUES (NEWID(),@p,@studid,dbo.GetThaiDate(),1,NULL,'{1}'); ")
        sql.Append(" INSERT INTO tblStudentPhoto VALUES (NEWID(),1,@studid,dbo.GetThaiDate(),1,NEWID(),1,dbo.GetThaiDate(),1,NULL);")
        db.ExecuteWithTransection(String.Format(sql.ToString(), ParentId, SchoolCode, ParentCode))
    End Function

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        If DeviceId IsNot Nothing Or DeviceId = "" Then

            SchoolCode = txtSchoolCode.Text.Trim()
            ParentCode = txtParentCode.Text.Trim()

            If Request.QueryString("addChild") Is Nothing Then
                ParentFirstName = txtParentName.Text.Trim()
                ParentLastName = txtParentLastName.Text.Trim()
                ParentPhone = txtParentPhone.Text.Trim()
            End If
            

            If SchoolCode <> "" And ParentCode <> "" Then
                Try
                    db.OpenWithTransection()
                    Dim parentId As String = IsRegistered()
                    If parentId = "" Then
                        AddChildAndParent()
                    Else
                        AddChild(parentId)
                    End If
                    db.CommitTransection()
                    Dim p As String = String.Format("~/{0}?DeviceId={1}", Sendpage, DeviceId)
                    Response.Redirect(p)
                Catch ex As Exception
                    db.RollbackTransection()
                End Try
            End If
        End If
    End Sub

    Private Function IsRegistered() As String
        Dim sql As String = "SELECT PR_Id FROM tblParent WHERE IsActive = 1 AND DeviceId = '" & DeviceId & "';"
        Return db.ExecuteScalarWithTransection(sql)
    End Function
End Class