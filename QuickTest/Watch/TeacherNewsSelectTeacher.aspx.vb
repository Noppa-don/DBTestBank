Public Class TeacherNewsSelectTeacher
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected DeviceId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'เช็คว่าเคยลงทะเบียนหรือยัง ถ้ายังต้องไปหน้าลงทะเบียนก่อน
        CheckRegister(Request.QueryString("DeviceId").ToString())


        'Dummy ข้อมูล
        Dim ParentId As String = ChatSelectStudent.GetParentIdByDeviceId(Request.QueryString("DeviceId").ToString()) '"B7DDE4BB-24EB-4F63-BBFF-C516EC05B147"

        If Request.QueryString("DeviceId").ToString() IsNot Nothing Then
            DeviceId = Request.QueryString("DeviceId").ToString()
        End If

        Dim dtCheckManyTeacher As DataTable = CheckIsManyTeacher(ParentId)
        If dtCheckManyTeacher.Rows.Count > 1 Then
            CreateDivSelectTeacher(dtCheckManyTeacher)
        ElseIf dtCheckManyTeacher.Rows.Count <> 0 Then
            Response.Redirect("~/Watch/TeacherNewsDetail.aspx?TeacherId=" & dtCheckManyTeacher.Rows(0)("Teacher_Id").ToString() & "&MorethanOne=False&DeviceId=" & DeviceId)
        End If

    End Sub

    Private Sub CheckRegister(ByVal DeviceId As String)

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & _DB.CleanString(DeviceId) & "' AND IsActive = 1; "
        Dim CheckCount As String = _DB.ExecuteScalar(sql)

        If CType(CheckCount, Integer) = 0 Then
            'Response.Redirect("~/Watch/RegisterParent.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
            Response.Redirect("~/Watch/AddChild2.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/TeacherNewsSelectTeacher.aspx")
        End If

    End Sub

    'Function สร้าง Div ที่ให้เลือก อาจารย์
    Private Sub CreateDivSelectTeacher(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then

            Dim sb As New StringBuilder
            For index = 0 To dt.Rows.Count - 1
                Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & dt.Rows(index)("School_Code").ToString() & "/{" & dt.Rows(index)("Teacher_Id").ToString() & "}/Id.jpg")
                Dim OwnerImg As String = ""
                If System.IO.File.Exists(OwnerPath) = True Then
                    OwnerImg = "../UserData/" & dt.Rows(index)("School_Code").ToString() & "/{" & dt.Rows(index)("Teacher_Id").ToString() & "}/Id.jpg" 'OwnerPath
                Else
                    OwnerImg = "../Images/default-profile-image.png"
                End If

                Dim CountLoop As Integer = index + 1
                Dim TeacherName As String = GetTeacherNameByTeacherId(dt.Rows(index)("Teacher_Id").ToString())
                sb.Append("<div id='DivCover" & CountLoop & "' class='DivWidth DivCover' onclick=""TeacherClick('" & dt.Rows(index)("Teacher_Id").ToString() & "');"">")
                sb.Append("<div id='Divpicture" & CountLoop & "' class='DivWidth DivPicture' style='background-image: url('" & OwnerImg & "');'>")
                sb.Append("</div><div id='DivName" & CountLoop & "' class='DivWidth DivName' >")
                sb.Append(TeacherName)
                sb.Append("</div></div>")
            Next
            DivSelectTeacher.InnerHtml = sb.ToString()
        End If

    End Sub

    'Function เช็คว่ามีครูมากกว่า 1 คนหรือเปล่า
    Private Function CheckIsManyTeacher(ByVal ParentId As String)
        Dim dt As DataTable
        'Dim sql As String = "  SELECT distinct tblTeacherNews.Teacher_Id ,tblStudentParent.School_Code "
        'sql &= " FROM tblTeacherNews INNER JOIN tblTeacherNewsDetail on tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id "
        'sql &= " inner join tblTeacherNewsDetailCompletion ON tblTeacherNewsDetail.TND_Id = tblTeacherNewsDetailCompletion.TND_Id"
        'sql &= " INNER JOIN  tblStudentParent ON tblTeacherNewsDetailCompletion.Student_Id = tblStudentParent.Student_Id  "
        'sql &= "WHERE (tblStudentParent.PR_Id = '" & ParentId & "') "

        Dim sql As New StringBuilder()
        sql.Append(" SELECT distinct tblTeacherNews.Teacher_Id ,tblStudentParent.School_Code ")
        sql.Append(" FROM tblTeacherNews INNER JOIN tblTeacherNewsDetail on tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id ")
        sql.Append(" inner join tblTeacherNewsDetailCompletion ON tblTeacherNewsDetail.TND_Id = tblTeacherNewsDetailCompletion.TND_Id ")
        sql.Append(" INNER JOIN  tblStudentParent ON tblTeacherNewsDetailCompletion.Student_Id = tblStudentParent.Student_Id ")
        sql.Append(" WHERE (tblStudentParent.PR_Id = '" & ParentId & "');")
        dt = _DB.getdata(sql.ToString())
        Return dt

    End Function

    'Function หาชื่อ อาจารย์
    Private Function GetTeacherNameByTeacherId(ByVal TeacherId As String) As String

        Dim sql As String = " select Teacher_FirstName + ' ' + Teacher_LastName from t360_tblTeacher where Teacher_id = '" & TeacherId & "' "
        Dim Teachername As String = _DB.ExecuteScalar(sql)
        Return Teachername

    End Function

End Class