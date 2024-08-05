Public Class test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GenSI()
    End Sub
    Public Shared Function GenSI()
        Dim db As New ClassConnectSql()
        Dim sql As String
        Dim dt As DataTable

        sql = "SELECT guid from tblschool where isactive  = 1 "
        dt = db.getdata(sql)


        Dim execQ As String = ""
        If dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                'JsonString = New With {.stuId = row("Student_Id")}
                execQ = "insert into tblschoolinfo (si_id, school_guid,  si_password, isactive, lastupdate) values ("
                execQ = execQ + "newid(), '" + row("guid").ToString() + "', '" + Encryption.MD5("1234") + "', '1', dbo.GetThaiDate());"
                '// ปิดไว้ก่อน เพราะใช้ได้จริง เดี๋ญวข้อมูลรวน                 
                '//db.Execute(execQ)
            Next
        End If
 
    End Function
End Class