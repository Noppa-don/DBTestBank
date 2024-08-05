Public Class ShowTabletOwnerInfo
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dt As DataTable = GetDtTabletInfo()
        If dt.Rows.Count > 0 Then
            BindGrid(dt)
        End If

    End Sub

    Private Sub BindGrid(ByVal dt As DataTable)

        GridShowInfo.DataSource = dt
        GridShowInfo.DataBind()

    End Sub


    Private Function GetDtTabletInfo() As DataTable

        Dim sql As String = " SELECT dbo.t360_tblTablet.School_Code,dbo.t360_tblTablet.Tablet_SerialNumber,t360_tblStudent.Student_Code, ( CASE WHEN dbo.t360_tblTabletOwner.Owner_Type = 1 THEN 'ครู' " & _
                            " WHEN dbo.t360_tblTabletOwner.Owner_Type = 2 THEN 'นักเรียน' END ) AS TYPE, " & _
                            " CASE WHEN dbo.t360_tblStudent.Student_Id IS NULL THEN dbo.t360_tblTeacher.Teacher_id " & _
                            " ELSE dbo.t360_tblStudent.Student_Id END AS StudentOrTeacherId,CASE WHEN t360_tblTeacher.Teacher_FirstName IS NULL " & _
                            " THEN (t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName) ELSE dbo.t360_tblTeacher.Teacher_FirstName " & _
                            " + ' ' + Teacher_LastName END  AS TeacherOrStudentName,t360_tblStudent.Student_CurrentNoInRoom, " & _
                            " dbo.t360_tblStudent.Student_CurrentClass, dbo.t360_tblStudent.Student_CurrentRoom " & _
                            " FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id LEFT OUTER JOIN " & _
                            " t360_tblTeacher ON t360_tblTabletOwner.Owner_Id = t360_tblTeacher.Teacher_id LEFT OUTER JOIN " & _
                            " t360_tblStudent ON t360_tblTabletOwner.Owner_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (t360_tblTablet.Tablet_IsActive = 1) AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) AND " & _
                            " (t360_tblStudent.Student_IsActive = 1 OR Student_IsActive IS NULL) AND " & _
                            " (t360_tblTeacher.Teacher_IsActive = 1 OR Teacher_IsActive IS NULL)  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function


    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("~/GenCreateTablet/CreateTabletWithOwner.aspx")
    End Sub

    Public Function TestTestTest(ByVal InputType As String, ByVal TabletSerialNumber As String)

        Dim StringReturn As String = ""



        If InputType = "นักเรียน" Then
            StringReturn = "<input type='button' value='ควิซ' style='width:90px;height:30px;margin:5px 5px 5px 5px;' onclick=""ShowActivitypagePad('" & TabletSerialNumber & "')"" />" & _
                           "<input type='button' value='การบ้าน/ฝึกฝน' style='width:90px;height:30px;margin:5px 5px 5px 5px;' onclick=""ShowChooseTestSet('" & TabletSerialNumber & "')"" />"
        Else
            StringReturn = ""
        End If

        Return StringReturn

    End Function

End Class