Imports KnowledgeUtils

Public Class TeacherNewsDetail
    Inherits System.Web.UI.Page
    Public CheckIsHaveBackbtn As Boolean
    Public TeacherName As String
    Public TeacherCurrentClass As String = ""
    Dim _DB As New ClassConnectSql()

    Protected DeviceId As String
    Public MoreThanStudent As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("MorethanOne") = True Then
            CheckIsHaveBackbtn = True
        Else
            CheckIsHaveBackbtn = False
        End If

        If Request.QueryString("TeacherId") Is Nothing Then
            Exit Sub
        End If

        If Request.QueryString("DeviceId").ToString() IsNot Nothing Then
            DeviceId = Request.QueryString("DeviceId").ToString()
        End If

        If Not Page.IsPostBack Then
            Dim Teacherid As String = Request.QueryString("TeacherId").ToString()
            GenImageTeacher(Teacherid)
            GetTeacherInfoForCreateDivTeacherInfo(Teacherid)
            MoreThanStudent = CheckMoreThanOneStudent(DeviceId)
            BindRepeaterTeacherNews(Teacherid, DeviceId)
        End If

    End Sub
    ''' <summary>
    ''' เช็คว่าผู้ปกครองคนนี้มีลูกกี่คน
    ''' </summary>
    ''' <param name="DeviceId">เลขเครื่อง Tablet ผู้ปกครอง</param>
    ''' <returns>True เมื่อมีลูกมากกว่า 1 คน</returns>
    ''' <remarks></remarks>
    Private Function CheckMoreThanOneStudent(DeviceId As String) As Boolean
        Dim Sql As String
        Sql = "  select count(Student_Id) from tblStudentParent inner join tblParent on tblStudentParent.PR_Id = tblParent.PR_Id "
        Sql &= " where DeviceId = '" & DeviceId.ToString & "'"
        Dim StudentAmount As String = _DB.ExecuteScalar(Sql)
        If StudentAmount > 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub GenImageTeacher(ByVal TeacherId As String)
        Dim SchoolId As String = GetSchoolIdByTeacherId(TeacherId)
        Dim OwnerPath As String = HttpContext.Current.Server.MapPath("../UserData/" & SchoolId & "/{" & TeacherId & "}/Id.png")
        Dim OwnerImg As String = ""
        If System.IO.File.Exists(OwnerPath) = True Then
            OwnerImg = "../UserData/" & SchoolId & "/{" & TeacherId & "}/Id.png" 'OwnerPath
        Else
            OwnerImg = "../Images/default-profile-image.png"
        End If
        Dim imgStr As String = "<img src=""" & OwnerImg & """ style='width: 150px;height: 150px;' />"
        TeacherImg.InnerHtml = imgStr
    End Sub

    Private Function GetSchoolIdByTeacherId(ByVal TeacherId As String) As String
        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblTeacher WHERE Teacher_id = '" & TeacherId & "' "
        Dim SchoolId As String = _DB.ExecuteScalar(sql)
        Return SchoolId
    End Function
    ''' <summary>
    ''' ดึงข่าวประกาศจากครูแสดงใน Repeater
    ''' </summary>
    ''' <param name="TeacherId">ครูที่เลือกมาหรือครูที่ได้ประกาศข่าว</param>
    ''' <param name="Deviceid">เลขเครื่อง Tablet ผู้ปกครอง</param>
    ''' <remarks>Query เป้นการดึงข่าวที่ครูประกาศให้นักเรียนมา Unoin กับข่าวที่ครูประกาศให้ห้องเรียน</remarks>
    Private Sub BindRepeaterTeacherNews(ByVal TeacherId As String, Deviceid As String)
        Dim sql As New StringBuilder

        'If MoreThanStudent Then
        'sql.Append(" select convert(varchar(20),a.StartDate,120) as StartDate,a.Student_FirstName,a.Description,a.ParentIsSeen ")
        sql.Append(" select StartDate,a.Student_FirstName,a.Description,a.ParentIsSeen ")
        sql.Append(" From (select StartDate,cast(Description as varchar(max)) as Description,t360_tblStudent.Student_FirstName,tblTeacherNews.LastUpdate,")
        sql.Append(" tblTeacherNewsDetailCompletion.ParentIsSeen from tblTeacherNews")
        sql.Append(" inner join tblTeacherNewsDetail on tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id")
        sql.Append(" inner join tblTeacherNewsDetailCompletion on tblTeacherNewsDetail.TND_Id = tblTeacherNewsDetailCompletion.TND_Id")
        sql.Append(" inner join tblStudentParent on tblStudentParent.Student_Id = tblTeacherNewsDetail.Student_Id")
        sql.Append(" inner join tblparent on tblStudentParent.PR_Id = tblParent.PR_Id")
        sql.Append(" and  tblTeacherNewsDetailCompletion.Student_Id = tblStudentParent.Student_Id")
        sql.Append(" inner join t360_tblStudent on tblStudentParent.Student_Id = t360_tblStudent.student_id")
        sql.Append(" where tblTeacherNews.Teacher_Id = '")
        sql.Append(TeacherId)
        sql.Append("' and DeviceId = '")
        sql.Append(Deviceid)
        sql.Append("' union select StartDate,cast(Description as varchar(max)) as Description,t360_tblStudent.Student_FirstName,tblTeacherNews.LastUpdate,")
        sql.Append(" tblTeacherNewsDetailCompletion.ParentIsSeen from tblTeacherNews ")
        sql.Append(" inner join tblTeacherNewsDetail on tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id")
        sql.Append(" inner join tblTeacherNewsDetailCompletion on tblTeacherNewsDetail.TND_Id = tblTeacherNewsDetailCompletion.TND_Id")
        sql.Append(" inner join t360_tblStudent on tblTeacherNewsDetail.Room_Id = t360_tblStudent.Student_CurrentRoomId")
        sql.Append(" inner join tblStudentParent on tblStudentParent.Student_Id = t360_tblStudent.student_id")
        sql.Append(" and  tblTeacherNewsDetailCompletion.Student_Id = tblStudentParent.Student_Id")
        sql.Append(" inner join tblparent on tblStudentParent.PR_Id = tblParent.PR_Id")
        sql.Append(" where tblTeacherNews.Teacher_Id = '")
        sql.Append(TeacherId)
        sql.Append("' and DeviceId = '")
        sql.Append(Deviceid)
        sql.Append("') a order by a.LastUpdate desc")

        Dim dt As New DataTable
        dt = _DB.getdata(sql.ToString)
        ' add column timeago
        dt.Columns.Add("TimeAgo", GetType(String))

        For Each row In dt.Rows
            row("TimeAgo") = Convert.ToDateTime(row("StartDate")).ToPointPlusTime()
        Next

        If dt.Rows.Count > 0 Then
            SetIconNew(dt)
            Repeater1.DataSource = dt
            Repeater1.DataBind()
            UpdateParentIsSeen(Deviceid, TeacherId)
        End If

    End Sub
    ''' <summary>
    ''' ปรับสถานะให้เป็นผู้ปกครองอ่านข่าวทั้งหมดแล้ว
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง ผู้ปกครอง</param>
    ''' <param name="TeacherId">Id ครู</param>
    ''' <remarks></remarks>
    Public Sub UpdateParentIsSeen(DeviceId As String, TeacherId As String)
        Dim Sql As New StringBuilder
        Sql.Append(" update tblTeacherNewsDetailCompletion set ParentIsSeen = '1' from tblTeacherNewsDetailCompletion ")
        Sql.Append(" inner join tblStudentParent on tblTeacherNewsDetailCompletion.Student_Id = tblStudentParent.Student_Id")
        Sql.Append(" inner join tblParent on tblStudentParent.PR_Id = tblParent.PR_Id ")
        Sql.Append(" inner join tblTeacherNewsDetail on tblTeacherNewsDetailCompletion.tnd_id = tblTeacherNewsDetail.TND_Id")
        Sql.Append(" inner join tblTeacherNews on tblTeacherNews.TN_Id = tblTeacherNewsDetail.TN_Id")
        Sql.Append(" where Teacher_Id = '")
        Sql.Append(TeacherId)
        Sql.Append("' and DeviceId = '")
        Sql.Append(DeviceId)
        Sql.Append("' and ParentIsSeen = '0'")
        _DB.Execute(Sql.ToString)
    End Sub

    ''' <summary>
    ''' ใส่รูป Icon new ให้กับข่าวที่ยังไม่เคยเห็น
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub SetIconNew(ByRef dt As DataTable)
        For Each Eachnews In dt.Rows
            If Eachnews("ParentIsSeen") = "0" Then
                Eachnews("TimeAgo") = "<img src=""../Images/NewIcon.png"" class=""NewIcon"">" & Eachnews("TimeAgo").ToString
            End If
        Next
    End Sub

    Private Sub GetTeacherInfoForCreateDivTeacherInfo(ByVal TeacherId As String)

        Dim sql As String = " select (Teacher_FirstName + ' ' + Teacher_LastName) as TeacherName ,"
        sql &= " (case when t360_tblTeacherRoom.Class_Name is null then '-' else (t360_tblTeacherRoom.Class_Name + t360_tblTeacherRoom.Room_Name) end)"
        sql &= " as TeacherClassRoom from t360_tblTeacher left join t360_tblTeacherRoom on t360_tblTeacher.Teacher_id = t360_tblTeacherRoom.Teacher_Id "
        sql &= " where t360_tblTeacher.Teacher_id = '" & TeacherId & "' and Teacher_IsActive = 1"

        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            TeacherName = "ชื่อ - สกุล : " & dt.Rows(0)("TeacherName").ToString()
            TeacherCurrentClass = "ประจำชั้น : "
            For Each eachTeacherRoom In dt.Rows
                TeacherCurrentClass &= eachTeacherRoom("TeacherClassRoom").ToString() & "  "
            Next

        End If

    End Sub


End Class