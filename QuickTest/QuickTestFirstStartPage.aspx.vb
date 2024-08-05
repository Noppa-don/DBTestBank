Imports System.IO
Imports System.Net
Imports BusinessTablet360

Public Class QuickTestFirstStartPage
    Inherits System.Web.UI.Page
    'ตัวแปรเอาไว้จัดการกับไฟล์ langset.bin
    'Dim ClsCheckLang As New ClsLanguage()
    'Dim ClsKnConfigData As New KNConfigData()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' ทำการ Post Key เพื่อไป Activate กับทาง QuicktestAdmin
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles BtnSubmit.Click
        'Dim debugText As String

        If RadMaskedTextBox1.Text.Length = 32 Then
            lblWarning.Visible = False
            'แปลง Key ให้อยู่ในรูป GUID ก่อน
            Dim InputKey As String = TranformFormatStrToGUID(RadMaskedTextBox1.Text)
            If InputKey <> "" Then
                'ทำการหารหัสเครื่องของเครื่องที่กำลัง Activate เพื่อที่จะส่งไปทาง QuickTestAdmin
                Dim FingerPrint As String = ClsLanguage.GenerateMachineIdentification()
                'debugText = debugText + "FingerPrint : " & FingerPrint
                'ทำการเข้ารหัสข้อมูลเพื่อที่จะส่งไป check กับ QuickTestAdmin เป็น Pattern รหัสเครื่อง + ' ' + Key + ' ' โปรแกรมที่จะActivate 0,1
                Dim EncodeData As String = KNConfigData.KnEncryption(FingerPrint & " " & InputKey & " " & EnLangType.Quick)
                'debugText = debugText + " Inputkey : " & InputKey
                'debugText = debugText + " EncodeData : " & EncodeData

                If EncodeData <> "" Then
                    'Post ไป QuickTestAdmin/Activation.aspx
                    Dim retConfig As String
                    retConfig = PostData(EncodeData)
                    'debugText = debugText + " retConfiger : " & retConfig
                    'ถ้ามีการตอบกลับมาก็ต้องเอาข้อความที่ตอบกลับมามาถอดรหัสแล้วเช็คดูว่าผ่านหรือเปล่า
                    If retConfig <> "-1" Then
                        'ทำการหา ConnectionString จาก ข้อความที่ส่งคืนมาจากฝั่ง server เพื่อเตรียมเอาไปเขียนในไฟล์  langset.bin
                        Dim ConnectionString As String = GetConnStr(retConfig)
                        If ClsLanguage.SwitchLang(retConfig & " " & KNConfigData.KnEncryption(FingerPrint) & " " & KNConfigData.KnEncryption(InputKey) & " " & KNConfigData.KnEncryption(ConnectionString)) Then
                            'Insert UserSubjectClass ให้มี User Default ขึ้นมาหลังจาก Activate เสร็จเรียบร้อย
                            Service.ClsSystem.InsertNewUserAfterDeleteData()

                            'update database set schoolid ตาม config file
                            Dim schoolId As String = GetSchoolIdFromConfig(retConfig)
                            UpdateSchoolCodeWhenActivated(schoolId)

                            lblWarning.ForeColor = Drawing.Color.Green
                            lblWarning.Text = "ลงทะเบียนเรียบร้อยแล้วค่ะ"
                            lblWarning.Visible = True
                            TriggerScript()
                        End If
                    Else
                        lblWarning.ForeColor = Drawing.Color.Red
                        lblWarning.Text = "Key ผิดค่ะ " '& debugText
                        lblWarning.Visible = True
                    End If
                End If
            Else
                lblWarning.ForeColor = Drawing.Color.Red
                lblWarning.Text = "Key ผิดค่ะ " '& debugText
                lblWarning.Visible = True
            End If
        Else
            lblWarning.ForeColor = Drawing.Color.Red
            lblWarning.Text = "ใส่จำนวน Key ไม่ครบค่ะ"
            lblWarning.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' ทำการเรียกใช้ Javascript หลังจากที่ Activate ผ่านเรียบร้อย โดยให้ทำการ Redirect ไปหน้า Loginpage.aspx ใน 15 วินาที
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TriggerScript()
        Dim ScriptStr As String = "<script type='text/javascript'>$(function () {setTimeout(function () { window.location = 'Default.aspx';}, 15000);});</script>"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", ScriptStr)
    End Sub

    ''' <summary>
    ''' ทำการแทรก - เข้าไปใน Key ให้มันกลายเป็น Pattern GUID เพื่อที่จะได้เอาไปตรวจกับ DB ได้
    ''' </summary>
    ''' <param name="InputStr">Key ที่กรอกเข้ามา</param>
    ''' <returns>String:ที่ Insert - เข้าไปเรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Private Function TranformFormatStrToGUID(ByVal InputStr As String)
        If InputStr IsNot Nothing And InputStr.Trim() <> "" Then
            InputStr = InputStr.Insert(8, "-")
            InputStr = InputStr.Insert(13, "-")
            InputStr = InputStr.Insert(18, "-")
            InputStr = InputStr.Insert(23, "-")
            Return InputStr
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' ทำการ Post ข้อมูลที่เข้ารหัสเรียบร้อยแล้วไปเช็คที่ทาง QuickTestAdmin
    ''' </summary>
    ''' <param name="EncodeString">ข้อมูลที่จะ Post ไปที่เข้ารหัสเรียบร้อยแล้ว</param>
    ''' <returns>String:-1=ไม่สำเร็จ,ค่าที่ตอบกลับมา=สำเร็จ</returns>
    ''' <remarks></remarks>
    Private Function PostData(ByVal EncodeString As String) As String
        'Dim stepDebug As String
        Try
            Using Client As New Net.WebClient
                Dim activationUrl As String = ConfigurationManager.AppSettings("ActivationURL").ToString()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
                'ตัวแปรนี้เป็นเหมือน collection ที่เอาไว้เก็บ Parameter ที่เราจะส่งไป เป็นลักษณะของ Key,Value
                Dim param As New Specialized.NameValueCollection
                'stepDebug = stepDebug + " 1 : "
                param.Add("StrEncoding", EncodeString)
                'ทำการ Post ไปหา QuickTestAdmin ตาม URL https://Activation.iknow.co.th/Activation.aspx
#If F5 = "1" Then
                'throw new exception("ss")
                'Dim responsebytes = Client.UploadValues("https://activationwebapp.azurewebsites.net/Activation.aspx", "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
                Dim responsebytes = Client.UploadValues(activationUrl, "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
                'Dim responsebytes = Client.UploadValues("http://Activation.iknow.co.th/Activation.aspx", "POST", param)
                'Dim responsebytes = Client.UploadValues("http://localhost:40661/Activation.aspx", "POST", param)
#Else
                'Dim responsebytes = Client.UploadValues("http://203.113.25.85/quicktestadmin/Activation.aspx", "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
                'stepDebug = stepDebug + " 2 : "
                ' Dim responsebytes = Client.UploadValues("https://activationwebapp.azurewebsites.net/Activation.aspx", "POST", param)
                'Dim responsebytes = Client.UploadValues("http://Activation.iknow.co.th/Activation.aspx", "POST", param)

                ' Client.Proxy.Credentials = New NetworkCredential("day", "d2280*", "192.168.5.254:8080")

                ' สำหรับ office ที่มีปัญหาด้าน Proxy
                'Dim wp As New WebProxy("192.168.5.254:8080")
                'wp.Credentials = CredentialCache.DefaultCredentials
                'Client.Proxy = wp

                'Dim responsebytes = Client.UploadValues("https://activationwebapp.azurewebsites.net/Activation.aspx", "POST", param)
                'Dim responsebytes = Client.UploadValues("http://Activation.iknow.co.th/Activation.aspx", "POST", param)
                'stepDebug = stepDebug + " 3 : "
                'Dim responsebytes = Client.UploadValues("http://localhost:40661/Activation.aspx", "POST", param)
                 Dim responsebytes = Client.UploadValues(activationUrl, "POST", param) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว

#End If
                'stepDebug = stepDebug + " 4 : "
                'ทำการเอาค่าที่ตอบกลับมา Return กลับไป
                Dim responsebody = (New System.Text.UTF8Encoding).GetString(responsebytes)
                'stepDebug = stepDebug + " 5 : "
                'Response.Write(stepDebug)
                'Response.End()
                Return responsebody

            End Using
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            'Response.Write(ex.Message)
            'Response.Write(ex.Data.ToString())
            'stepDebug = stepDebug + " 6 : "
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1" '& stepDebug
        End Try
    End Function

    ''' <summary>
    ''' ทำการหาว ConnectionString จากค่าConfig ที่ส่งคืนมาจากทางฝั่ง QuickTestAdmin
    ''' </summary>
    ''' <param name="InputKNConfig">ค่า Config</param>
    ''' <returns>String:ConnectionString</returns>
    ''' <remarks></remarks>
    Private Function GetConnStr(ByVal InputKNConfig As String) As String
        InputKNConfig = KNConfigData.DecryptData(InputKNConfig)
        Dim objKnConfig = InputKNConfig.Split("|")
        Dim objEachKey = Nothing
        Dim ConnStr As String = ""
        'loop เพื่อหาว่ามี Key อันไหนที่เกี่ยวข้องกับ ConnectionString บ้าง โดยถ้าเป็น standalone ก็จะใช้เป็น localdb , ส่วนเครื่องส่วนกลางจะใช้เป็น centraldb , เงื่อนไขการจบ loop คือ วนจนกว่าจะเจอ Key รูปแบบใดรูปแบบนึง
        For Each r In objKnConfig
            objEachKey = r.ToString().Split("*")
            Dim Eachkey As String = objEachKey(0)
            If Eachkey.ToLower() = "localdb" Then
                ConnStr = objEachKey(1)
                Exit For
            ElseIf Eachkey.ToLower() = "centraldb" Then
                ConnStr = objEachKey(1)
                Exit For
            End If
        Next
        Return ConnStr
    End Function

    Private Function GetSchoolIdFromConfig(ConfigStr As String) As String
        Dim cf As String = KNConfigData.DecryptData(ConfigStr)
        Dim configs As List(Of String) = cf.Split("|").ToList()
        Dim s As String = configs.Where(Function(t) t.ToLower().IndexOf("defaultschoolcode") > -1).SingleOrDefault()
        If s Is Nothing Then Return "1000001"
        Return s.ToLower().Replace("defaultschoolcode*", "")
    End Function

    Private Sub UpdateSchoolCodeWhenActivated(SchoolId As String)
        Dim sql As New StringBuilder()
        sql.Append(" DECLARE @schoolid AS varchar(50) = '" & SchoolId & "';")
        sql.Append(" UPDATE t360_tblCalendar SET School_Code = @schoolid; ")
        sql.Append(" UPDATE t360_tblSchoolClass SET School_Code = @schoolid; ")
        sql.Append(" UPDATE t360_tblTeacher SET School_Code = @schoolid;")
        sql.Append(" UPDATE t360_tblUser SET School_Code = @schoolid; ")
        sql.Append(" UPDATE t360_tblUserMenuItem SET School_Code = @schoolid; ")
        sql.Append(" UPDATE tblUser SET SchoolId = @schoolid; ")
        Dim db As New ClassConnectSql()
        db.Execute(sql.ToString())
    End Sub

End Class