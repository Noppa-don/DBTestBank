Public Class Childs
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql
    Public DeviceId As String
    Private SchoolId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DeviceId = Request.QueryString("DeviceId").ToString()
        If DeviceId Is Nothing Or DeviceId = "" Then
            'todo
        End If

        If Not ParentRegister() Then
            Response.Redirect("~/Watch/AddChild2.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/AddChild.aspx")
        End If

        SchoolId = db.ExecuteScalar(String.Format(" SELECT School_Code FROM dbo.tblParent WHERE DeviceId = '{0}' AND IsActive = 1;", DeviceId))

        CreateChildsElement()
    End Sub

    Private Function ParentRegister() As Boolean
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & db.CleanString(DeviceId) & "' AND IsActive = 1;"
        Dim CheckCount As String = db.ExecuteScalar(sql)
        If CType(CheckCount, Integer) = 0 Then
            Return False
        End If
        Return True
    End Function

    Private Function GetChilds() As DataTable
        Dim sql As New StringBuilder
        sql.Append("DECLARE @schoolid AS VARCHAR(10) = (SELECT School_Code FROM dbo.tblParent WHERE DeviceId = '{0}' AND IsActive = 1);")
        sql.Append(" SELECT s.* FROM tblParent p INNER JOIN tblStudentParent sp ON p.PR_Id = sp.PR_Id ")
        sql.Append(" INNER JOIN t360_tblStudent s ON s.Student_Id = sp.Student_Id ")
        sql.Append(" WHERE p.IsActive = 1 AND p.School_Code = @schoolid AND p.DeviceId = '{0}' and s.Student_IsActive = 1;")
        Return db.getdata(String.Format(sql.ToString(), DeviceId))
    End Function

    Private Sub CreateChildsElement()
        Dim dt As DataTable = GetChilds()
        If dt.Rows.Count > 0 Then
            'create div student
            Dim s As New StringBuilder
            For Each row As DataRow In dt.Rows
                Dim studentId As String = row("Student_Id").ToString()
                row("Student_HasPhoto") = If(IsDBNull(row("Student_HasPhoto")), False, row("Student_HasPhoto"))
                Dim imgStudentPath As String = If(row("Student_HasPhoto"), "../UserData/" & SchoolId & "/{" & studentId & "}/Id.jpg", "../UserControl/MonsterID.axd?seed=" & studentId & "&size=179")
                Dim studentName As String = String.Format("{0}{1}  {2}", row("Student_PrefixName").ToString(), row("Student_FirstName").ToString(), row("Student_LastName").ToString())
                Dim studentClass As String = String.Format("เลขที่ {0}  {1}{2}", row("Student_CurrentNoInRoom").ToString(), row("Student_CurrentClass").ToString(), row("Student_CurrentRoom").ToString())

                s.Append("<div class=""children""><div class=""imgChild"" style=""background-image: url('")
                s.Append(imgStudentPath)
                s.Append("');"" ></div><div><span>")
                s.Append(studentName)
                s.Append("</span></div><div><span>")
                s.Append(studentClass)
                s.Append("</span></div></div>")

            Next
            s.Append("<div class='addChild'><div></div></div>")

            divChilds.InnerHtml = s.ToString()
        Else
            'return null string
            divChilds.InnerHtml = ""
        End If
       
    End Sub



End Class