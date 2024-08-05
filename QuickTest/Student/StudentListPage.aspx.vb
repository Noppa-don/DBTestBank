Imports System.IO
Imports BusinessTablet360
Imports System.Web

Public Class StudentListPage
    Inherits System.Web.UI.Page

    Public htmlClass As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        Session("SchoolID") = "1000001"
        Session("SchoolCode") = "1000001"
        Dim knSession As New KNAppSession()
        Session("selectedSession") = "0000"
        knSession("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
        knSession("SelectedCalendarName") = "เทอม 2 / 2556"
#End If

        If Session("UserId") Is Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        End If
        Dim UserID As String = Session("UserId").ToString()

#If IE = "1" Then
        Dim classRoom As String = "ป.3/1"
        lblClassRoom.Text = classRoom
        Dim h As String = ReloadUserControl(classRoom)
        If h = "True" Then
            h = "ห้องนี้ยังไม่มีนักเรียนค่ะ"
            ShowStudentSelectedRoom.Attributes.Add("class", "NotSelectedRoom")
        End If
        'ShowStudentSelectedRoom.InnerHtml = ReloadUserControl(classRoom)
        ShowStudentSelectedRoom.InnerHtml = h
        ShowStudentSelectedRoom.Style.Add("display", "block")
        ModeClassName.InnerHtml = "ห้องที่สอน"
        hdKeepModeRoom.Value = False
        htmlClass = SetMenuClassRoom(False)
#Else
        If Request.QueryString("SelectedClassRoom") Is Nothing Then
            lblClassRoom.Text = ""
            ShowStudentSearch.InnerHtml = "เลือกห้องก่อนค่ะ"
            ShowStudentSearch.Attributes.Add("class", "NotSelectedRoom")
            ShowStudentSearch.Style.Add("display", "block")
            ModeClassName.InnerHtml = "ห้องทั้งหมด"
            hdKeepModeRoom.Value = True
            htmlClass = SetMenuClassRoom(True)
        Else
            Dim classRoom As String = Request.QueryString("SelectedClassRoom").ToString()
            lblClassRoom.Text = classRoom
            Dim h As String = ReloadUserControl(classRoom)
            If h = "True" Then
                h = "ห้องนี้ยังไม่มีนักเรียนค่ะ"
                ShowStudentSelectedRoom.Attributes.Add("class", "NotSelectedRoom")
            End If
            'ShowStudentSelectedRoom.InnerHtml = ReloadUserControl(classRoom)
            ShowStudentSelectedRoom.InnerHtml = h
            ShowStudentSelectedRoom.Style.Add("display", "block")
            ModeClassName.InnerHtml = "ห้องที่สอน"
            hdKeepModeRoom.Value = False
            htmlClass = SetMenuClassRoom(False)
        End If
#End If
    End Sub

    ' set menu เลือกห้อง ด้านข้าง
    <Services.WebMethod()>
    Public Shared Function SetMenuClassRoom(ByVal IsSelectedClassInSchool As Boolean) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim dt As DataTable
        Dim ClsStudent As New Service.ClsStudent(New ClassConnectSql)
        If IsSelectedClassInSchool Then
            dt = ClsStudent.GetClassNameInSchool()
        Else
            dt = ClsStudent.GetClassNameHaveQuizOrHomework()
        End If

        Dim htmlClass As New StringBuilder()
        If Not dt.Rows.Count = 0 Then
            Dim start As Integer = dt.Rows(0)("ClassOrder")
            Dim startClass As String = dt.Rows(0)("ClassName").ToString().Substring(0, 3)
            htmlClass.Append("<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>")
            For Each row In dt.Rows
                If Not row("ClassOrder") = start Then
                    htmlClass.Append("</div>")
                    start = row("ClassOrder")
                    startClass = row("ClassName").ToString().Substring(0, 3)
                    htmlClass.Append("<h3 class='menuAcdHeadItem'>" & startClass & "</h3><div>")
                End If
                htmlClass.Append("<div class='menuAcdItem'>" & row("ClassName") & "</div>")
            Next
            htmlClass.Append("</div>")
        End If
        SetMenuClassRoom = htmlClass.ToString()
    End Function

    ' Reload User control
    <Services.WebMethod()>
    Public Shared Function ReloadUserControl(ByVal ClassRoom As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Using Page As New Page
            Dim Uc As New StudentListControl
            Dim htmlRoom As String = Uc.GetHtmlByRoom(ClassRoom, True)
            'Dim Hf As New HtmlForm
            'Using writer As New StringWriter()
            '    Hf.Controls.Add(Uc)
            '    Page.Controls.Add(Hf)
            '    HttpContext.Current.Server.Execute(Page, writer, False)
            '    Return writer.ToString()
            'End Using
            Return htmlRoom
        End Using
    End Function
    ' GetStudentSearch
    <Services.WebMethod()>
    Public Shared Function GetStudentSearch(ByVal txtSearch As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim dt As DataTable = GetStudent(txtSearch)

        Dim htmlStudent As New StringBuilder()
        Dim pos As String
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                If i Mod 3 = 0 Then
                    pos = "class='Left'"
                ElseIf i Mod 3 = 1 Then
                    pos = "class='Middle'"
                Else
                    pos = "class='Right'"
                End If
                'htmlStudent.Append("<div " & pos & "><img src=""../Images/Student.png"" alt="""" /><div>นายฐิติพงษ์  หาญวงศ์ไพบูลย์<br/><label>เต๋า ป.2/1</label></div></div>")
                Dim student As New SetStudent(dt.Rows(i)("Student_Id").ToString(), dt.Rows(i)("StudentName"), dt.Rows(i)("Student_NickName").ToString(), dt.Rows(i)("StudentRoom"), pos)
                htmlStudent.Append(student.GetHtmlStudent())
                student = Nothing
            Next
        Else
            htmlStudent.Append("NotHave")
        End If
        GetStudentSearch = htmlStudent.ToString()
    End Function
    ' GetStudent 
    Private Shared Function GetStudent(ByVal txtSearch As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT Student_Id,(Student_PrefixName + ' ' + Student_FirstName + '  ' + Student_LastName) AS StudentName,Student_NickName,(Student_CurrentClass + Student_CurrentRoom) AS StudentRoom ")
        sql.Append(" FROM t360_tblStudent WHERE Student_FirstName LIKE '%")
        sql.Append(txtSearch)
        sql.Append("%' OR Student_LastName LIKE '%")
        sql.Append(txtSearch)
        sql.Append("%' OR Student_NickName LIKE '%")
        sql.Append(txtSearch)
        sql.Append("%' ")
        sql.Append("AND School_Code = '")
        sql.Append(HttpContext.Current.Session("SchoolID").ToString())
        sql.Append("' ORDER BY Student_CurrentClass;")
        Dim db As New ClassConnectSql()
        GetStudent = db.getdata(sql.ToString())
    End Function
End Class

Public Class SetStudent
    Private StudentId As String
    Private StudentName As String
    Private StudentNickName As String
    Private StudentRoom As String
    Private StudentImage As String
    Private Position As String

    Public Sub New(ByVal Id As String, ByVal Name As String, ByVal NickName As String, ByVal Room As String, ByVal Pos As String)
        StudentId = Id
        StudentName = Name
        StudentNickName = NickName
        StudentRoom = Room
        StudentImage = Id
        ' StudentImage = "92DD8917-B6C7-4897-8042-F49C6E1B69D7"
        Position = Pos
    End Sub

    Public Function GetHtmlStudent() As String
        Dim htmlStudent As New StringBuilder()
        htmlStudent.Append("<div id='")
        htmlStudent.Append(StudentId)
        htmlStudent.Append("' onclick=""SelectedStudent('")
        htmlStudent.Append(StudentId)
        htmlStudent.Append("');"" ")
        htmlStudent.Append(Position)

        Dim clsUser As New ClsUser(New ClassConnectSql)
        Dim HasPhoto As Boolean = clsUser.GetStudentHasPhoto(StudentImage)

        If HasPhoto Then
            htmlStudent.Append("><img alt="""" src=""../UserData/")
            htmlStudent.Append(HttpContext.Current.Session("SchoolID").ToString())
            htmlStudent.Append("/{")
            htmlStudent.Append(StudentImage)
            htmlStudent.Append("}/Id.jpg"" />")
        Else
            Dim MonsterPhoto = "MonsterID.axd?seed=" & StudentImage & "&size=179"
            htmlStudent.Append("><img alt="""" src=" & MonsterPhoto & " />")
        End If
        htmlStudent.Append("<div>")
        htmlStudent.Append(StudentName)
        htmlStudent.Append("<br/><label>")
        htmlStudent.Append(StudentNickName)
        htmlStudent.Append("  ")
        htmlStudent.Append(StudentRoom)
        htmlStudent.Append("</label></div></div>")
        GetHtmlStudent = htmlStudent.ToString()
    End Function
End Class
