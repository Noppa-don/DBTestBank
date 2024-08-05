Public Class PrepareDataMaxonetCI
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim result As String = PrepareData()
        Response.Write(result)
    End Sub

    Private Function PrepareData() As String
        Try
            db.OpenWithTransection()
            Dim keys As New List(Of String)({"497046156553", "801338963327", "595814291322", "968380089041", "801338963327", "112233445566", "112233445567", "112233445568", "112233445569", "112233445570", "112233445571"})
            For Each key In keys
                Dim sql As String = getSQLPrepareMaxonet(key)
                db.ExecuteWithTransection(sql)
            Next
            Dim tablets As New List(Of String)({"IK001", "IK002", "IK003", "IK009", "Bug1", "Bug2", "Case1S1", "Case2S3", "Case3S5"})
            'delete tablet
            'Dim dtTablets As DataTable = db.getdataWithTransaction("SELECT KCU_DeviceId FROM maxonet_tblkeycodeusage WHERE KeyCode_Code = '" & key & "';")
            For Each tablet In tablets
                Dim sql2 As String = getSQLDeleteTablet(tablet)
                db.ExecuteWithTransection(sql2)
            Next

            db.CommitTransection()
            Return "OK"
        Catch ex As Exception
            Return "Fail"
        End Try
    End Function

    Private Function getSQLDeleteTablet(serial As String) As String
        Dim sql As New StringBuilder()
        sql.Append(" DECLARE @tabletid AS UNIQUEIDENTIFIER = (SELECT Tablet_Id FROM t360_Tbltablet WHERE Tablet_SerialNumber = '" & serial & "'); ")
        sql.Append(" DELETE t360_tbltablet WHERE Tablet_Id = @tabletid; ")
        sql.Append(" DELETE t360_tbltabletOwner WHERE Tablet_Id = @tabletid;")
        Return sql.ToString()
    End Function

    Private Function getSQLPrepareMaxonet(key As String) As String
        Dim sql As New StringBuilder()
        sql.Append("Declare @keycode AS VARCHAR(12) = '" & key & "';")
        sql.Append(" Declare @stuId AS UNIQUEIDENTIFIER = (SELECT TOP 1 KCU_OwnerId FROM maxonet_tblKeyCodeUsage WHERE KeyCode_Code = @keycode);")
        sql.Append(" DELETE maxonet_tblStudentSubject WHERE SS_StudentId = @stuId;")
        sql.Append(" DELETE t360_tblStudent WHERE Student_Id = @stuId;")
        sql.Append(" DELETE t360_tblStudentRoom WHERE Student_Id = @stuId;")
        sql.Append(" DELETE maxonet_tblKeyCodeUsage WHERE KeyCode_Code = @keycode;")
        sql.Append(" DELETE maxonet_tblParentGraph WHERE KeyCode_Code = @keycode;")
        sql.Append("DELETE maxonet_tblParentReport WHERE KeyCode_Code = @keycode;")
        Return sql.ToString()
    End Function
End Class