Imports WebSupergoo.ABCpdf8
Imports System.IO
Imports BusinessTablet360
Imports Microsoft.Practices.Unity
Imports KnowledgeUtils

Public Class LoginPage
    Inherits System.Web.UI.Page
    Public NewPath As String

#Region "Dependency"

    Public Sub New()
        CType(Context.ApplicationInstance, Global_asax).Container.BuildUp(Me.GetType, Me)
    End Sub

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

#End Region

    Private isEncryptPwd As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Log.RecordLog(Log.LogCategory.Login, Log.LogAction.PageLoad, False, "เข้าหน้าจอ Login", "")
            txtvalidate.Visible = False
            If ClsLanguage.Chklang() = True Then
                Try
                    'ดึงค่า Config ทั้งหมดขึ้น Application
                    KNConfigData.LoadData()
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    txtvalidate.Text = "ข้อมูล Config เสียหาย, เข้าใช้งานไม่ได้นะคะ, แจ้งผู้ดูแลให้ลงทะเบียนเครื่องใหม่ค่ะ"
                    txtvalidate.Visible = True
                End Try

            Else
                Response.Redirect("~/QuickTestFirstStartPage.aspx")
            End If

            ByPassSchoolCode()


        End If
    End Sub

    Function ValidatePage() As Boolean

        If txtSchoolid.Text = "" OrElse txtusername.Text = "" OrElse txtpassword.Text = "" Then
            If HttpContext.Current.Application("DefaultSchoolCode") IsNot Nothing Then
                txtvalidate.Text = "* กรอกชื่อผู้ใช้และรหัสผ่านก่อนนะคะ"
                txtvalidate.Visible = True
            Else
                'txtemty.Visible = True
            End If
            Return False
        End If

        If IsNumeric(txtSchoolid.Text) = False Then
            'txtemty.Visible = True
            Return False
        End If

        If Request.QueryString("id") Is Nothing Then
            Dim CsSql As New ClassConnectSql
            Dim Sql As String = ""
            Dim schoolID As String = txtSchoolid.Text.Trim()

            If schoolID = "0" Then
                If Not IsNothing(Session("FromLoginAdminPage")) Then
                    schoolID = "-1"
                End If
            End If

            Session("FromLoginAdminPage") = Nothing

            Dim pwd As String = If((isEncryptPwd), Encryption.MD5(txtpassword.Text), txtpassword.Text)
            Dim dt = GetUserDetail(txtusername.Text, pwd, schoolID)

            If dt.Rows.Count = 0 Then
                Dim TabletSerialNumber As String = CheckStudentLogin(txtusername.Text, pwd, schoolID)
                If TabletSerialNumber <> "" Then
                    HttpContext.Current.Application("StudentLoginFromPC") = True
                    Response.Redirect("~/PracticeMode_Pad/ChooseTestset.aspx?DeviceUniqueID=" & TabletSerialNumber)
                Else
                    If HttpContext.Current.Application("DefaultSchoolCode") IsNot Nothing Then
                        txtvalidate.Text = "* ชื่อผู้ใช้หรือรหัสผ่านผิด ลองอีกครั้งนะคะ"
                    End If
                    txtvalidate.Visible = True
                    Return False
                End If

            Else
                UserConfig.AddSchoolCode(dt.Rows(0)("SchoolId"))
                UserConfig.AddUserId(dt(0)("GUID"))

                'Clear Session ของเก่าทิ้ง ถ้า user login ใหม่ แบบ new tab
                If Session("UserId") IsNot Nothing Then
                    Session.RemoveAll()
                End If

                Session("UserId") = dt(0)("GUID")
                Session("UserName") = dt(0)("UserName")
                Session("FirstName") = dt(0)("FirstName")
                Session("LastName") = dt(0)("LastName")
                Session("IsAllowMenuManageUserSchool") = dt.Rows(0)("IsAllowMenuManageUserSchool")
                Session("IsAllowMenuManageUserAdmin") = dt.Rows(0)("IsAllowMenuManageUserAdmin")
                Session("IsAllowMenuAdminLog") = dt.Rows(0)("IsAllowMenuAdminLog")
                Session("IsAllowMenuContact") = dt.Rows(0)("IsAllowMenuContact")
                Session("IsAllowMenuSetEmail") = dt.Rows(0)("IsAllowMenuSetEmail")
                Session("SchoolID") = dt.Rows(0)("SchoolId")
                Session("SchoolCode") = dt.Rows(0)("SchoolId")
                Session("TeacherId") = dt.Rows(0)("SchoolId")
                Session("IsTeacher") = True
                Session("UnLoad") = False 'ใช้กับ signalR

                If IsDBNull(dt.Rows(0)("FontSize")) Then
                    SetFontSize(dt(0)("GUID").ToString())
                    Session("FontSize") = 0
                Else
                    Session("FontSize") = dt.Rows(0)("FontSize")
                End If

                'check userid ว่ามีสิทธิ์จะอนุมัติข้อสอบได้หรือเปล่า ใน mode word,quiz,ดัชนีชี้วัด(วิชาการ,Prepress)
                CheckIsSuperUser()

                'ส่วนของการเช็คว่าจะโชว์ Qtip แสดงการใช้งานในแต่ละหน้าหรือเปล่า    
                Dim ClsUserViewPageWithTip As New UserViewPageWithTip(dt(0)("GUID").ToString())
                If IsDBNull(dt.Rows(0)("IsViewAllTips")) Then
                    ClsUserViewPageWithTip.CheckIsViewAllTips(False)
                Else
                    ClsUserViewPageWithTip.CheckIsViewAllTips(dt.Rows(0)("IsViewAllTips"))
                End If

            End If
        End If

        Return True
    End Function

    'Function เช็คว่า รร. นี้มีข้อมูลใน t360_tblCalendar หรือเปล่า
    Private Function CheckIsHaveCalendarData() As Boolean
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.t360_tblCalendar WHERE School_Code = '" & HttpContext.Current.Session("SchoolID").ToString() & "' "
        Dim CheckCalendar As String = _DB.ExecuteScalar(sql)
        If CInt(CheckCalendar) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetUserDetail(ByVal UserName As String, ByVal Password As String, ByVal SchoolId As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblUser LEFT JOIN tblUserSetting ON tblUser.GUID = tblUserSetting.User_Id ")
        sql.Append(" WHERE tblUser.UserName = '")
        sql.Append(UserName)
        sql.Append("' AND tblUser.Password = '")
        sql.Append(Password)
        sql.Append("' AND tblUser.SchoolId='")
        sql.Append(SchoolId)
        sql.Append("' AND tblUser.IsActive = 1;")
        GetUserDetail = New ClassConnectSql().getdata(sql.ToString())
    End Function

    Private Function CheckStudentLogin(ByVal UserName As String, ByVal Password As String, ByVal SchoolId As String) As String
        Dim sql As String
        sql = "select t360_tblTablet.Tablet_SerialNumber " & _
              " from t360_tblTablet inner join t360_tblTabletOwner on t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " & _
              " inner join t360_tblStudent on t360_tblTabletOwner.Owner_Id = t360_tblStudent.Student_Id " & _
              " where t360_tblStudent.UserName = '" & UserName & "' and t360_tblStudent.Password = '" & Password & "' and t360_tblStudent.School_Code = '" & SchoolId & "' and t360_tblStudent.Student_IsActive = 1 " & _
              " and t360_tblTablet.Tablet_IsActive = 1 and t360_tblTabletOwner.TabletOwner_IsActive = 1"

        Dim TabletSerial As String
        TabletSerial = New ClassConnectSql().ExecuteScalar(sql).ToString()
        Return TabletSerial
    End Function

    Private Sub SetFontSize(ByVal User_Id As String)
        Dim sql As String = " INSERT tblUserSetting VALUES ('" & User_Id & "',0,1,dbo.GetThaiDate(),NULL);"
        Dim db As New ClassConnectSql()
        db.Execute(sql)
    End Sub

    Private Sub ByPassSchoolCode()
        If HttpContext.Current.Application("DefaultSchoolCode") IsNot Nothing Then
            If HttpContext.Current.Application("DefaultSchoolCode").ToString() <> "" Then
                If HttpContext.Current.Application("DefaultSchoolCode").ToString().Trim() <> "-1" Then
                    'txtSchoolid.Visible = False
                    trSchoolCode.Style.Add("display", "none")
                    txtSchoolid.Text = HttpContext.Current.Application("DefaultSchoolCode").ToString().Trim()
                End If
            End If
        End If
    End Sub

    Public Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSubmit.Click, btnLogin.Click
        isEncryptPwd = If((TypeOf sender Is ImageButton), True, False)
        If ValidatePage() Then
            If CheckIsHaveCalendarData() Then
                Log.RecordLog(Log.LogCategory.Login, Log.LogAction.Click, True, "เข้าสู่ระบบ", Session("UserId").ToString)
                If txtSchoolid.Text <> "0" Then
                    'ดักเปิดโหมดแบบให้มีหน้า SelectSession  หรือเปล่า  
                    Dim ClsSelectSession As New ClsSelectSession()
                    Dim CurrentPageFromRunMode As String = ClsSelectSession.GetCurrentPageFromRunMode()
                    If HttpContext.Current.Application("NeedSelectSesstion") = True Then
                        If ClsSelectSession.IsHaveSession Then
                            Response.Redirect("~/Session/SelectSession.aspx")
                        Else
                            CreateNewSession(ClsSelectSession, CurrentPageFromRunMode)
                        End If
                    Else
                        CreateNewSession(ClsSelectSession, CurrentPageFromRunMode)
                    End If
                Else
                    Response.Redirect("~/MenuPage.aspx", False)
                End If
            Else
                Response.Redirect("~/Setuppage.aspx", False)
            End If
        End If
    End Sub

    Private Sub CreateNewSession(ByRef ClsSelectSession As ClsSelectSession, ByVal CurrentPageFromRunMode As String)
        Try
            ClsSelectSession.NewSession(CurrentPageFromRunMode)
            Log.Record(Log.LogType.Login, "CreateNewSession And Redirect to ~/" & CurrentPageFromRunMode, True)
            Response.Redirect("~/" & CurrentPageFromRunMode)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            txtvalidate.Text = "ปฏิทินการศึกษาไม่ถูกต้องค่ะ เข้าใช้งานไม่ได้"
            txtvalidate.Visible = True
        End Try
    End Sub

    Private Sub createLinkCss()
        Dim linkCss As New HtmlLink()
        linkCss.Attributes.Add("type", "text/css")
        linkCss.Attributes.Add("rel", "stylesheet")
        If (HttpContext.Current.Application("NeedQuizMode") = True) Then
            linkCss.Attributes.Add("href", "./css/styleQuiz.css")
        Else
            linkCss.Attributes.Add("href", "./css/style.css")
        End If
       ' Me.Header.Controls.Add(linkCss)
    End Sub

    Private Sub CheckIsSuperUser()

        Session("SuperUser") = False

        'proof,auther,certify
        If Session("UserName").ToString().Length >= 5 Then
            If Session("UserName").ToString().Substring(0, 5).ToLower() = "proof" Then
                Session("UserCheckExamType") = "proof"
                Exit Sub
            End If
        End If

        If Session("UserName").ToString().Length >= 6 Then
            If Session("UserName").ToString().Substring(0, 6).ToLower() = "auther" Then
                Session("UserCheckExamType") = "auther"
                Exit Sub
            End If
        End If

        If Session("UserName").ToString().Length >= 7 Then
            If Session("UserName").ToString().Substring(0, 7).ToLower() = "certify" Then
                Session("UserCheckExamType") = "certify"
                Exit Sub
            End If

            If Session("UserName").ToString().Substring(0, 7).ToLower() = "approve" Then
                Session("UserCheckExamType") = "approve"
                Session("SuperUser") = True
                Exit Sub
            End If
        End If

        If Session("UserName").ToString().Length >= 10 Then
            If Session("UserName").ToString().Substring(0, 10).ToLower() = "superadmin" Then
                Session("UserCheckExamType") = "superadmin"
                Exit Sub
            End If
        End If

    End Sub

End Class