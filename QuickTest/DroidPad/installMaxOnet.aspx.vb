Public Class installMaxOnet
    Inherits System.Web.UI.Page

    Private MethodName As String

    Private UserName As String
    Private KeyCode As String
    Private DeviceId As String

    Private TokenId As String

    Private StudentId As String
    Private StudentName As String
    Private StudentLastName As String
    Private StudentClass As String
    Private StudentNumber As String
    Private StudentGender As String
    Private StudentPhone As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsLog.Record("PageLoad : installMaxOnet.aspx")

        If Not Page.IsPostBack Then
            'ชื่อ method ที่ post
            MethodName = Request.Form("methodName")
            'รหัสเครื่อง Tablet
            DeviceId = Request.Form("DeviceUniqueID")

            ClsLog.Record("MethodName = " & MethodName & " ,DeviceId = " & DeviceId)

            'ถ้าไม่ได้ส่งค่าหลักเหล่านี้มา ก็ return -1 error ไปเลย
            If (IsNothing(MethodName) OrElse IsNothing(DeviceId)) OrElse (MethodName = "" OrElse DeviceId = "") Then
                ClsLog.Record("Return -1 เนื่องจากข้อมูลผิดพลาด")
                ClsLog.Record("มีการกดลงทะเบียนแต่ข้อมูลที่ส่งมาผิดพลาด")
                Response.Write("-1")
                Response.End()
                Exit Sub
            End If

            Dim ReturnValue As String = "-1"

            Try
                Dim maxOnet As New MaxOnetManagement()
                maxOnet.DeviceId = DeviceId
                maxOnet.KCU_Type = If((MethodName.ToLower() = "registerparent"), MaxOnetRegisterType.parent, MaxOnetRegisterType.student)

                If MethodName.ToLower() = "registerstudent" Or MethodName.ToLower() = "registerparent" Then

                    KeyCode = Request.Form("password").Replace("-", "").Trim()
                    UserName = Request.Form("username").ToLower()

                    maxOnet.UserName = UserName
                    maxOnet.KeyCode = KeyCode

                    Dim LogGuid As String = System.Guid.NewGuid().ToString()
                    ClsLog.Record(LogGuid & " : Start InstallMaxOnet UserName = " & UserName & " ,KeyCode = " & KeyCode)

                    ReturnValue = maxOnet.GetToken()

                ElseIf MethodName.ToLower() = "newstudentmaxonet" Then

                    StudentName = Request.Form("FirstName")
                    StudentLastName = Request.Form("LastName")
                    StudentClass = Request.Form("Class")
                    StudentPhone = Request.Form("Phone")
                    StudentGender = Request.Form("Gender")
                    TokenId = Request.Form("token")

                    maxOnet.StudentName = StudentName
                    maxOnet.StudentLastName = StudentLastName
                    maxOnet.StudentClass = StudentClass
                    maxOnet.StudentPhone = StudentPhone
                    maxOnet.StudentGender = StudentGender.ToString().Trim().ToLower()
                    maxOnet.TokenId = TokenId


                    ClsLog.Record("StudentName = " & StudentName & " ,StudentLastName = " & StudentLastName & " ,StudentClass = " & StudentClass &
                          " ,StudentPhone = " & StudentPhone & " ,StudentGender = " & StudentGender)

                    ClsLog.Record("ก่อนเข้า RegisterStudentMaxOnet")
                    ReturnValue = maxOnet.RegisterStudentMaxOnet()
                End If
            Catch ex As Exception
                ClsLog.Record("Catach ex = " & ex.InnerException.ToString())
            Finally
                Response.Write(ReturnValue)
                Response.End()
            End Try
        End If
    End Sub
End Class