Public Class HomeworkSelectStudent
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Private DeviceId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'เช็คว่าเคยลงทะเบียนหรือยัง ถ้ายังต้องไปหน้าลงทะเบียนก่อน
        CheckRegister(Request.QueryString("DeviceId").ToString())


        'Dummy ParentId 
        Dim ParentId As String = ChatSelectStudent.GetParentIdByDeviceId(Request.QueryString("DeviceId").ToString()) '"B7DDE4BB-24EB-4F63-BBFF-C516EC05B147"
        Session("SchoolID") = ChatSelectStudent.GetSchoolIdByDeviceId(Request.QueryString("DeviceId").ToString()) '"1000001"
        DeviceId = Request.QueryString("DeviceId").ToString()
        ClsLog.Record("DeviceIdFromParentWatchPageload = " & Request.QueryString("DeviceId").ToString())
        ' My.Computer.FileSystem.WriteAllText("D:\data\install.txt", "DeviceIdFromParentWatchPageload = " & Request.QueryString("DeviceId").ToString(), True, Text.Encoding.Default)


        Dim dt As DataTable = CheckIsMoreThanOneStudent(ParentId)

        If dt.Rows.Count > 0 Then
            If dt.Rows.Count > 1 Then
                CreateDivStudent(dt)
            Else
                Response.Redirect("HomeworkDetail.aspx?StudentId=" & dt.Rows(0)("Student_Id").ToString() & "&MorethanOne=False&DeviceId=" & Request.QueryString("DeviceId").ToString())
            End If
        End If

    End Sub

    Private Sub CheckRegister(ByVal DeviceId As String)

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & _DB.CleanString(DeviceId) & "' AND IsActive = 1; "
        Dim CheckCount As String = _DB.ExecuteScalar(sql)

        If CType(CheckCount, Integer) = 0 Then
            'Response.Redirect("~/Watch/RegisterParent.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
            'Response.Redirect("~/Watch/addchild.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
            Response.Redirect("~/Watch/AddChild2.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/HomeworkSelectStudent.aspx")
        End If

    End Sub

    'Function เช็คว่าผู้ปกครองคนนี้มีนักเรียนมากกว่า 1 คนหรือเปล่า
    Private Function CheckIsMoreThanOneStudent(ByVal PrId As String) As DataTable

        'Dim sql As String = " select Student_Id,Student_HasPhoto from tblStudentParent where PR_Id = '" & PrId & "' and IsActive = 1 "
        Dim sql As String = "SELECT s.Student_Id,Student_HasPhoto FROM tblStudentParent sp INNER JOIN t360_tblStudent s ON sp.Student_Id = s.Student_Id WHERE sp.PR_Id = '" & PrId & "' and IsActive = 1 and s.Student_IsActive = 1;"
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    'สร้าง Div นักเรียนเพื่อให้ผู้ปกครองเลือกนักเรียนที่ต้องการดู การบ้าน
    Private Sub CreateDivStudent(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            For index = 0 To dt.Rows.Count - 1
                Dim CountId As Integer = index + 1
                Dim StudentName As String = GetStudentNameByStudentId(dt.Rows(index)("Student_Id").ToString())
                Dim SchoolId As String = GetSchoolIdByStudentId(dt.Rows(index)("Student_Id").ToString())
                sb.Append("<div id='DivCover" & CountId & "' class='DivWidth DivCover' onclick=""ChooseStudent('" & dt.Rows(index)("Student_Id").ToString() & "','" & DeviceId & "');"">")
                If IsDBNull(dt.Rows(index)("Student_HasPhoto")) OrElse dt.Rows(index)("Student_HasPhoto") = False Then
                    sb.Append("<div id='Divpicture" & CountId & "' class='DivWidth DivPicture' style='background-size:cover;background-image:url(../UserControl/MonsterID.axd?seed=" & dt.Rows(index)("Student_Id").ToString() & "&size=179);' ></div>")
                Else
                    sb.Append("<div id='Divpicture" & CountId & "' class='DivWidth DivPicture' style='background-size:cover;background-image:url(../UserData/" & SchoolId & "/{" & dt.Rows(index)("Student_Id").ToString() & "}/id.jpg);' ></div>")
                End If

                sb.Append("<div id='DivName1" & CountId & "' class='DivWidth DivName' >")
                sb.Append(StudentName)
                sb.Append("</div></div>")
            Next
            DivSelectStudent.InnerHtml = sb.ToString()
        End If



    End Sub

    Private Function GetStudentNameByStudentId(StudentId As String)

        Dim sql As String = " select Student_FirstName   + ' ' + Student_LastName  from t360_tblStudent where " & _
                            " Student_Id = '" & StudentId & "' and Student_IsActive = 1 "
        Dim StudentName As String = _DB.ExecuteScalar(sql)
        Return StudentName

    End Function


    Private Function GetSchoolIdByStudentId(ByVal StudentId As String) As String
        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' "
        Dim SchoolId As String = _DB.ExecuteScalar(sql)
        Return SchoolId
    End Function



End Class