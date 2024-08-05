Public Class ChatSelectStudent
    Inherits System.Web.UI.Page
    'ตัวแปรใช้จัดการกับฐานข้อมูล Insert,Update,Delete
    Dim db As New ClassConnectSql()
    'ตัวแปร FingerPrint ของเครื่อง Tablet
    Private DeviceId As String

    ''' <summary>
    ''' ทำการหาข้อมูลว่า ผู้ปกครองคนนี้มีลูกมากกว่า 1 คนหรือเปล่า ถ้ามีต้องสร้าง Div นักเรียนให้เลือกนักเรียนก่อน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'เช็คว่าเคยลงทะเบียนหรือยัง ถ้ายังต้องไปหน้าลงทะเบียนก่อน
        CheckRegister(Request.QueryString("DeviceId").ToString())

        'Dummy ParentId 
        Dim ParentId As String = GetParentIdByDeviceId(Request.QueryString("DeviceId").ToString()) '"6A875127-D037-4F84-86E0-AE6803599EDB"
        Session("SchoolID") = GetSchoolIdByDeviceId(Request.QueryString("DeviceId").ToString()) '"1000001"
        DeviceId = Request.QueryString("DeviceId").ToString()
       

        Session("Owner_Id") = ParentId
        Dim dt As DataTable = CheckIsMoreThanOneStudent(ParentId)

        If dt.Rows.Count > 0 Then
            If dt.Rows.Count > 1 Then
                CreateDivStudent(dt)
            Else
                Response.Redirect("~/Watch/ChatSelectTeacher.aspx?StudentId=" & dt.Rows(0)("Student_Id").ToString() & "&MorethanOne=False&DeviceId=" & DeviceId)
            End If
        End If

    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceId"></param>
    ''' <remarks></remarks>
    Private Sub CheckRegister(ByVal DeviceId As String)
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & db.CleanString(DeviceId) & "' AND IsActive = 1; "
        Dim CheckCount As String = db.ExecuteScalar(sql)

        If CType(CheckCount, Integer) = 0 Then
            'Response.Redirect("~/Watch/RegisterParent.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
            Response.Redirect("~/Watch/AddChild2.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/ChatSelectStudent.aspx")
        End If
    End Sub

    ''' <summary>
    ''' Function เช็คว่าผู้ปกครองคนนี้มีนักเรียนมากกว่า 1 คนหรือเปล่า
    ''' </summary>
    ''' <param name="PrId">Id ของผู้ปกครอง</param>
    ''' <returns>Datatable:นักเรียนซึ่งเป็นลูกของผู้ปกครองคนนี้</returns>
    ''' <remarks></remarks>
    Private Function CheckIsMoreThanOneStudent(ByVal PrId As String) As DataTable
        ' Dim sql As String = " select Student_Id from tblStudentParent where PR_Id = '" & PrId & "' and IsActive = 1 "
        Dim sql As String = "SELECT s.Student_Id,Student_HasPhoto FROM tblStudentParent sp INNER JOIN t360_tblStudent s ON sp.Student_Id = s.Student_Id WHERE sp.PR_Id = '" & PrId & "' and IsActive = 1 and s.Student_IsActive = 1;"
        Dim dt As New DataTable
        dt = db.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' สร้าง Div นักเรียนเพื่อให้ผู้ปกครองเลือกนักเรียนที่ต้องการดู การบ้าน
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub CreateDivStudent(ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            'loop เพื่อสร้าง Div นักเรียน , เงื่อนไขการจบ loop คือ สร้าง div นักเรียนที่เป็นลูกของผู้ปกครองคนนี้ให้ครบหมดทุกคน
            For index = 0 To dt.Rows.Count - 1
                Dim CountId As Integer = index + 1
                Dim StudentName As String = GetStudentNameByStudentId(dt.Rows(index)("Student_Id").ToString())
                Dim SchoolId As String = GetSchoolIdByStudentId(dt.Rows(index)("Student_Id").ToString())

                Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & SchoolId & "/{" & dt.Rows(index)("Student_Id").ToString() & "}/Id.png")
                Dim OwnerImg As String = ""
                If System.IO.File.Exists(OwnerPath) = True Then
                    OwnerImg = "../UserData/" & SchoolId & "/{" & dt.Rows(index)("Student_Id").ToString() & "}/Id.png"
                Else
                    OwnerImg = "../UserControl/MonsterID.axd?seed=" & dt.Rows(index)("Student_Id").ToString() & "&size=179"
                End If

                sb.Append("<div id='DivCover" & CountId & "' class='DivWidth DivCover' onclick=""ChooseStudent('" & dt.Rows(index)("Student_Id").ToString() & "','" & DeviceId & "');"">")
                sb.Append("<div id='Divpicture" & CountId & "' class='DivWidth DivPicture' style='background-size:cover;background-image:url(" & OwnerImg & ");' ></div>")
                sb.Append("<div id='DivName1" & CountId & "' class='DivWidth DivName' >")
                sb.Append(StudentName)
                sb.Append("</div></div>")
            Next
            DivSelectStudent.InnerHtml = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' หาชื่อนักเรียน จาก รหัสนักเรียน
    ''' </summary>
    ''' <param name="StudentId">Id ของนักเรียน</param>
    ''' <returns>String:ชื่อนักเรียน</returns>
    ''' <remarks></remarks>
    Private Function GetStudentNameByStudentId(ByVal StudentId As String)
        Dim sql As String = " select Student_FirstName   + ' ' + Student_LastName  from t360_tblStudent where " & _
                            " Student_Id = '" & StudentId & "' and Student_IsActive = 1 "
        Dim StudentName As String = db.ExecuteScalar(sql)
        Return StudentName
    End Function

    ''' <summary>
    ''' หารหัสโรงเรียน จากรหัสนักเรียน
    ''' </summary>
    ''' <param name="StudentId">Id ของนักเรียน</param>
    ''' <returns>String:รหัสโรงเรียน</returns>
    ''' <remarks></remarks>
    Private Function GetSchoolIdByStudentId(ByVal StudentId As String) As String
        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' "
        Dim SchoolId As String = db.ExecuteScalar(sql)
        Return SchoolId
    End Function

    ''' <summary>
    ''' หารหัสผู้ปกครองจาก DeviceId ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="DeviceId">Id ของเครื่องที่กำลังใช้งานอยู่</param>
    ''' <returns>String:Id ของผู้ปกครอง</returns>
    ''' <remarks></remarks>
    Public Shared Function GetParentIdByDeviceId(DeviceId As String) As String
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT PR_Id FROM dbo.tblParent WHERE DeviceId = '" & DeviceId & "' AND IsActive = 1; "
        Dim parentId As String = db.ExecuteScalar(sql)
        db = Nothing
        Return parentId
    End Function

    ''' <summary>
    ''' หารหัสโรงเรียนจาก DeviceId ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="DeviceId">Id ของเครื่องที่กำลังใช้งานอยู่</param>
    ''' <returns>String:รหัสโรงเรียน</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSchoolIdByDeviceId(DeviceId As String) As String
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT School_Code FROM dbo.tblParent WHERE DeviceId = '" & DeviceId & "' AND IsActive = 1; "
        Dim schoolId As String = db.ExecuteScalar(sql)
        db = Nothing
        Return schoolId
    End Function




End Class