

Public Class RegisterParent
    Inherits System.Web.UI.Page

    Dim _DB As New ClassConnectSql()


    ' 1. function save เมื่อกดปุ่ม save ต้องไป save ลงใน table รอก่อน
    ' 2. function ที่เช็คตอนแรกว่าเครื่องนี้เคยลงทะเบียนหรือยังถ้าลงแล้วให้เอา ข้อมูล มาไล่เติมลงใน textbox

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            LoadProvince()
        End If

    End Sub

    'Save
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If Validate() = True Then
            Dim StudentId As String = RadComboBoxStudent.SelectedValue
            SavePic(StudentId, FileUpload1.PostedFile)
        Else
            Exit Sub
        End If

    End Sub

    'เลือกจังหวัด
    Private Sub RadComboBoxProvince_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxProvince.SelectedIndexChanged
        If e.Value <> "" Then
            RadComboBoxAmphur.Text = ""
            RadComboBoxSchool.Items.Clear()
            RadComboBoxClass.Items.Clear()
            RadComboBoxRoom.Items.Clear()
            RadComboBoxStudent.Items.Clear()
            LoadAmphur(e.Value)
        End If
    End Sub

    'เลือกอำเภอ
    Private Sub RadComboBoxAmphur_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxAmphur.SelectedIndexChanged
        If e.Value <> "" Then
            RadComboBoxSchool.Text = ""
            RadComboBoxClass.Items.Clear()
            RadComboBoxRoom.Items.Clear()
            RadComboBoxStudent.Items.Clear()
            LoadSchool(e.Value)
        End If
    End Sub

    'เลือกโรงเรียน
    Private Sub RadComboBoxSchool_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxSchool.SelectedIndexChanged
        If e.Value <> "" Then
            RadComboBoxClass.Text = ""
            RadComboBoxRoom.Items.Clear()
            RadComboBoxStudent.Items.Clear()
            LoadClass(e.Value)
        End If
    End Sub

    'เลือกชั้น
    Private Sub RadComboBoxClass_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxClass.SelectedIndexChanged
        If e.Value <> "" Then
            RadComboBoxRoom.Text = ""
            RadComboBoxStudent.Items.Clear()
            LoadRoom(e.Value)
        End If
    End Sub

    'เลือกห้อง
    Private Sub RadComboBoxRoom_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxRoom.SelectedIndexChanged
        If e.Value <> "" Then
            LoadStudent(e.Value)
        End If
    End Sub

    Private Sub LoadProvince()

        Dim sql As String = " select ProvinceId,ProvinceName from tblProvince where IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxProvince.Items.Clear()
            RadComboBoxProvince.DataTextField = "ProvinceName"
            RadComboBoxProvince.DataValueField = "ProvinceId"
            RadComboBoxProvince.DataSource = dt
            RadComboBoxProvince.DataBind()
            RadComboBoxProvince.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกจังหวัด"))
            RadComboBoxProvince.SelectedIndex = 0
        Else
            Response.Write("ไม่มีจังหวัด")
        End If

    End Sub

    Private Sub LoadAmphur(ByVal ProvinceId As Integer)

        Dim sql As String = " select AmphurId,AmphurName from tblAmphur where ProvinceId = " & ProvinceId & " and IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxAmphur.Enabled = True
            RadComboBoxAmphur.Items.Clear()
            RadComboBoxAmphur.DataTextField = "AmphurName"
            RadComboBoxAmphur.DataValueField = "AmphurId"
            RadComboBoxAmphur.DataSource = dt
            RadComboBoxAmphur.DataBind()
            RadComboBoxAmphur.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกอำเภอ"))
            RadComboBoxAmphur.SelectedIndex = 0
        Else
            Response.Write("ไม่มีอำเภอ")
        End If

    End Sub

    Private Sub LoadSchool(ByVal AmphurId As Integer)

        Dim sql As String = " select SchoolId,SchoolName from tblSchool where AmphurId = " & AmphurId & " and IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxSchool.Enabled = True
            RadComboBoxSchool.Items.Clear()
            RadComboBoxSchool.DataTextField = "SchoolName"
            RadComboBoxSchool.DataValueField = "SchoolId"
            RadComboBoxSchool.DataSource = dt
            RadComboBoxSchool.DataBind()
            RadComboBoxSchool.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกโรงเรียน"))
            RadComboBoxSchool.SelectedIndex = 0
        Else
            Response.Write("ไม่มีโรงเรียน")
        End If

    End Sub

    Private Sub LoadClass(ByVal SchoolId As String)

        Dim CalendarId As String = GetCalendarId(SchoolId)
        If CalendarId = "" Then
            Response.Write("ไม่มี calendarId")
            Exit Sub
        End If

        HttpContext.Current.Session("ChooseSchoolId") = SchoolId

        Dim sql As String = " select distinct Class_Name from t360_tblStudentRoom where School_Code = '" & SchoolId & "' " & _
                            " and Calendar_Id = '" & CalendarId & "' and SR_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxClass.Enabled = True
            RadComboBoxClass.Items.Clear()
            RadComboBoxClass.DataTextField = "Class_Name"
            RadComboBoxClass.DataValueField = "Class_Name"
            RadComboBoxClass.DataSource = dt
            RadComboBoxClass.DataBind()
            RadComboBoxClass.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกชั้น"))
            RadComboBoxClass.SelectedIndex = 0
        Else
            Response.Write("ไม่มีชั้น")
        End If

    End Sub

    Private Sub LoadRoom(ByVal ClassName As String)

        Dim CalendarId As String = GetCalendarId(HttpContext.Current.Session("ChooseSchoolId"))
        If CalendarId = "" Then
            Response.Write("ไม่มี calendarId")
            Exit Sub
        End If

        Dim sql As String = " select distinct (Class_Name + Room_Name) as RoomName from t360_tblStudentRoom where School_Code = '" & HttpContext.Current.Session("ChooseSchoolId").ToString() & "' " & _
                            " and Calendar_Id = '" & CalendarId & "' and Class_Name = '" & _DB.CleanString(ClassName) & "' and SR_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxRoom.Enabled = True
            RadComboBoxRoom.Items.Clear()
            RadComboBoxRoom.DataTextField = "RoomName"
            RadComboBoxRoom.DataValueField = "RoomName"
            RadComboBoxRoom.DataSource = dt
            RadComboBoxRoom.DataBind()
            RadComboBoxRoom.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกห้อง"))
            RadComboBoxRoom.SelectedIndex = 0
        Else
            Response.Write("ไม่มีห้อง")
        End If

    End Sub

    Private Sub LoadStudent(ByVal RoomName As String)

        Dim CalendarId As String = GetCalendarId(HttpContext.Current.Session("ChooseSchoolId"))
        If CalendarId = "" Then
            Response.Write("ไม่มี calendarId")
            Exit Sub
        End If

        Dim splitStr = RoomName.Split("/")

        Dim CurrentClassName As String = ""
        Dim CurrentRoomName As String = ""

        If splitStr.Count > 0 Then
            CurrentClassName = splitStr(0)
            CurrentRoomName = "/" & splitStr(1)
        Else
            Response.Write("ไม่มีห้องส่งมา")
            Exit Sub
        End If
        
        Dim sql As String = " select t360_tblStudent.Student_Id,(t360_tblStudent.Student_FirstName + ' ' + t360_tblStudent.Student_LastName) StudentName " & _
                            " from t360_tblStudent inner join t360_tblStudentRoom on t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id " & _
                            " where t360_tblStudentRoom.Class_Name = '" & _DB.CleanString(CurrentClassName) & "' and t360_tblStudentRoom.Room_Name = '" & _DB.CleanString(CurrentRoomName) & "' and " & _
                            " t360_tblStudentRoom.Calendar_Id = '" & CalendarId & "' and t360_tblStudentRoom.School_Code = '" & HttpContext.Current.Session("ChooseSchoolId") & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            RadComboBoxStudent.Enabled = True
            RadComboBoxStudent.Items.Clear()
            RadComboBoxStudent.DataTextField = "StudentName"
            RadComboBoxStudent.DataValueField = "Student_Id"
            RadComboBoxStudent.DataSource = dt
            RadComboBoxStudent.DataBind()
            RadComboBoxStudent.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem("เลือกนักเรียน"))
            RadComboBoxStudent.SelectedIndex = 0
        Else
            Response.Write("ไม่มีนักเรียน")
        End If

    End Sub


    Private Sub SavePic(ByVal PicId As String, ByVal ParentPic As HttpPostedFile)

        'ParentPic.SaveAs(Server.MapPath("../Watch/SomeFolder/" & PicId & ".jpg"))
        ParentPic.SaveAs("D:\data\tmp\Watch\RegisterParent\" & PicId & ".jpg")

    End Sub

    Private Function GetCalendarId(ByVal SchoolId As String)

        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                            " AND Calendar_Type = 3 AND School_Code = '" & SchoolId & "'; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql)
        Return CalendarId

    End Function

    Private Function Validate()
        If txtPRFirstName.Text = "" Or txtPRPhone.Text = "" Or FileUpload1.HasFile = False Or RadComboBoxProvince.SelectedValue = "" Or RadComboBoxAmphur.SelectedValue = "" Or _
            RadComboBoxSchool.SelectedValue = "" Or RadComboBoxClass.SelectedValue = "" Or RadComboBoxRoom.SelectedValue = "" Or RadComboBoxStudent.SelectedValue = "" Then

            If txtPRFirstName.Text = "" Then
                lblValidateFirstName.Visible = True
            Else
                lblValidateFirstName.Visible = False
            End If

            If txtPRPhone.Text = "" Then
                lblValidatePhone.Visible = True
            Else
                lblValidatePhone.Visible = False
            End If

            If FileUpload1.HasFile = False Then
                lblValidateUploadPic.Visible = True
            Else
                lblValidateUploadPic.Visible = False
            End If

            If RadComboBoxProvince.SelectedValue = "" Then
                lblValidateProvince.Visible = True
            Else
                lblValidateProvince.Visible = False
            End If

            If RadComboBoxAmphur.SelectedValue = "" Then
                lblValidateAmphur.Visible = True
            Else
                lblValidateAmphur.Visible = False
            End If

            If RadComboBoxSchool.SelectedValue = "" Then
                lblValidateSchool.Visible = True
            Else
                lblValidateSchool.Visible = False
            End If

            If RadComboBoxClass.SelectedValue = "" Then
                lblValidateClass.Visible = True
            Else
                lblValidateClass.Visible = False
            End If

            If RadComboBoxRoom.SelectedValue = "" Then
                lblValidateRoom.Visible = True
            Else
                lblValidateRoom.Visible = False
            End If

            If RadComboBoxStudent.SelectedValue = "" Then
                lblValidateStudent.Visible = True
            Else
                lblValidateStudent.Visible = False
            End If

            Return False
        End If
        lblValidateFirstName.Visible = False
        lblValidatePhone.Visible = False
        lblValidateUploadPic.Visible = False
        lblValidateProvince.Visible = False
        lblValidateAmphur.Visible = False
        lblValidateSchool.Visible = False
        lblValidateClass.Visible = False
        lblValidateRoom.Visible = False
        lblValidateStudent.Visible = False
        Return True
    End Function

End Class