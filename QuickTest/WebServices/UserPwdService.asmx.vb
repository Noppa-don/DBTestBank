Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports GdPicture10
Imports System.IO
Imports System.Drawing
Imports ZXing
Imports System.Security.Cryptography
Imports KnowledgeUtils
Imports System.Net

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class UserPwdService
    Inherits System.Web.Services.WebService

    <WebMethod(EnableSession:=True)>
    Public Function CheckKeyIsCorrect(ByVal InputKey As String) As String
        If InputKey Is Nothing AndAlso InputKey.Length <> 39 Then
            Return "False"
        End If
        If InputKey IsNot Nothing And InputKey.ToString() <> "" Then
            Try
                InputKey = InputKey.Replace("-", "")
                InputKey = InputKey.Insert(8, "-")
                InputKey = InputKey.Insert(13, "-")
                InputKey = InputKey.Insert(18, "-")
                InputKey = InputKey.Insert(23, "-")
                Dim CheckKey As String = ClsLanguage.GetKeyIdDec()

                If CheckKey.Length <> 36 Then
                    Return "False"
                ElseIf CheckKey.ToLower() = InputKey.ToLower() Then
                    Return "True"
                Else
                    Return "False"
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ElmahExtension.LogToElmah(ex)
                Return "False"
            End Try
        Else
            Return "False"
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function ResetUserPwd(UserName As String) As String
        UserName = UserName.ToLower
        Dim pwd As String = If(UserName = "admin", Encryption.MD5("network"), Encryption.MD5("1234"))
        Try
            ResetPwd(UserName, pwd)
            Return "True"
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return "False"
        End Try
    End Function

    Private Sub ResetPwd(UserName As String, Pwd As String)
        Dim sql As String = String.Format("UPDATE tblUser SET Password = '{0}' WHERE UserName = '{1}';", Pwd, UserName)
        Dim db As New ClassConnectSql()
        db.Execute(sql)
    End Sub

    <WebMethod(EnableSession:=True)>
    Public Function CheckPwdRegister(SchoolCode As String, ParentCode As String, DeviceId As String) As String
        'check schoolid and pwd   ---> return studentid
        If ParentCode.Length < 8 OrElse ParentCode.Length > 8 Then
            Return False
        End If
        Dim sql As String = String.Format("SELECT Student_Id FROM tblParentCode WHERE IsActive = 1 AND School_Code = '{0}' AND ParentCode = '{1}';", SchoolCode, ParentCode)
        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)

        If dt.Rows.Count = 1 Then
            Dim parentId As String = db.ExecuteScalar("SELECT PR_Id FROM tblParent WHERE IsActive = 1 AND DeviceId = '" & DeviceId & "';")
            If parentId = "" Then
                Return True
            End If

            Dim studentId As String = dt.Rows(0)("Student_Id").ToString()
            Dim studentRegistered As DataTable = db.getdata("SELECT * FROM tblStudentParent WHERE IsActive = 1 AND PR_Id = '" & parentId & "' AND Student_Id = '" & studentId & "';")
            If studentRegistered.Rows.Count > 0 Then
                Return "Registered"
            End If
            Return True
        End If
        Return False


    End Function

    <WebMethod(EnableSession:=True)>
    Public Function CheckDeviceRegistered(DeviceId As String) As Boolean
        Dim db As New ClassConnectSql
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & db.CleanString(DeviceId) & "' AND IsActive = 1;"
        Dim CheckCount As String = db.ExecuteScalar(sql)
        If CType(CheckCount, Integer) = 0 Then
            Return False
        End If
        Return True
    End Function


    <WebMethod(EnableSession:=True)>
    Public Function CheckUser(ByVal StrBase64 As String) As String
        'Dim JsonData As String
        'StrBase64 = StrBase64.Replace(" ", "+")
        'StrBase64 = StrBase64.Replace("data:image/png;base64,", "")
        'StrBase64 = StrBase64.Replace("data:image/jpeg;base64,", "")
        ''ทำการแปลงสตริง Base64 ให้เป็น Byte ก่อน
        'Dim Imagebyte() As Byte = Convert.FromBase64String(StrBase64)
        'Dim ms As MemoryStream = New MemoryStream(Imagebyte)
        'ms.Write(Imagebyte, 0, Imagebyte.Length)
        ''ทำการสร้างรูปจาก Base64
        'Dim Image As Image = Image.FromStream(ms)
        ''นำเลขที่ random มาได้มาตั้งเป็นชื่อรูป
        'Dim FileName As String = GetRndSeqNumber().ToString()

        ''Do Until File.Exists(Server.MapPath("../DroidPad/temp/" & FileName & ".png")) = False
        ''    FileName = GetRndSeqNumber().ToString()
        ''Loop

        ''ทำการเช็คก่อนว่าชื่อรูปที่ Random มาได้มันซ้ำหรือยัง ถ้าซ้ำแล้วต้องไป Random มาใหม่ , เงื่อนไขการจบ loop คือ ชื่อต้องไม่ซ้ำ
        'Do Until File.Exists("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png") = False
        '    FileName = GetRndSeqNumber().ToString()
        'Loop
        ''Image.Save(Server.MapPath("../DroidPad/temp/" & FileName & ".png"))
        ''ทำการ save รูปลงไปที่ tmp ก่อน

        'Image.Save("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
        'ClsLog.Record("After UpLoad Pic")
        ''ตัวแปรที่จะนำไปอ่าน QR-Code

        'Dim readCode As New ZXing.QrCode.QRCodeReader()
        'Dim bitMap As New Bitmap("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
        'bitMap = Convert1bpp(bitMap)

        'Dim RbitMap = ResizeImage(bitMap, 239, 217)

        'RbitMap.Save("D:\data\tmp\DroidPad\QrReader\" & FileName & "AfterResize.png", Imaging.ImageFormat.Png)

        'Dim lumianaceSource = New ZXing.BitmapLuminanceSource(RbitMap)
        'Dim binarizer = New ZXing.Common.HybridBinarizer(lumianaceSource)

        'Dim mapa As New ZXing.BinaryBitmap(binarizer)

        'Dim reader = New MultiFormatReader()

        'Dim Result = reader.decode(mapa)

        'Dim txt As String = Result.Text().Replace("*strokescribe.com FREE*", "")
        'Dim lists As New List(Of String)(txt.Split(","c))
        Dim guid As Guid
        'Dim tokenId As String = If((lists.Count > 0), lists.Item(3).Replace("{", "").Replace("}", ""), "")
        If Guid.TryParse(StrBase64, guid) Then
            Return GetUserNameAndPassword(StrBase64)
        End If
        Return "False"
        'JsonData = Result.Text
        'Return JsonData
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function CheckMaxOnetUser(UserName As String, Password As String) As String
        Try
            '/maxonet/WebServices/UserPwdService.asmx/CheckMaxOnetUser
            Dim hostName As String = HttpContext.Current.Request.Url.ToString()
            hostName = hostName.Replace("http://", "")
            hostName = hostName.Replace("https://", "")
            Dim spHostName() As String = Split(hostName, "/")

            Dim SiteName As String = spHostName(0)
            Log.Record(Log.LogType.AdminLog, hostName, True)
            Log.Record(Log.LogType.AdminLog, SiteName, True)

            Dim db As New ClassConnectSql
            Dim sql As String = ""

            sql = "select SchoolId from tblSchool where siteName = '" & SiteName & "'  and isactive = 1;"

            Dim SchoolId As String = db.ExecuteScalar(sql)

            sql = "select KCU_DeviceId,KCU_Token,KCU_OwnerId,KCU_LimitExamAmount from t360_tblStudent s inner join maxonet_tblKeyCodeUsage k on s.Student_Id = k.KCU_OwnerId 
                where UserName = '" & UserName & "' and password = '" & Password & "' and School_Code = '" & SchoolId & "' and KCU_IsActive = 1 and Student_IsActive = 1"

            Dim dtUser As DataTable = db.getdata(sql)

            If dtUser.Rows.Count <> 0 Then
                sql &= " And (KCU_ExpireDate > dbo.GetThaiDate() or KCU_ExpireDate is null)"

                Dim dtCheckUserExpire As DataTable = db.getdata(sql)

                If dtCheckUserExpire.Rows.Count > 0 Then
                    Session("DeviceId") = dtUser.Rows(0)("KCU_DeviceId").ToString
                    Session("TokenId") = dtUser.Rows(0)("KCU_Token").ToString
                    Return "OK," & dtUser.Rows(0)("KCU_DeviceId").ToString & "," & dtUser.Rows(0)("KCU_Token").ToString & "," & dtUser.Rows(0)("kcu_OwnerId").ToString
                Else
                    Return "Expired," & dtUser.Rows(0)("KCU_DeviceId").ToString & "," & dtUser.Rows(0)("KCU_Token").ToString & "," & dtUser.Rows(0)("kcu_OwnerId").ToString
                End If
            Else
                Return "False"
            End If
        Catch ex As Exception
            Log.Record(Log.LogType.AdminLog, ex.ToString, True)
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function CheckMaxOnetUserByKeycode(UserName As String, Password As String) As String

        Dim db As New ClassConnectSql
        Dim sql As String = ""

        Dim dbLicenseKey As New ClassConnectSql(False, ConfigurationManager.ConnectionStrings("LicensKeyConnectionString").ConnectionString)
        Dim dt As DataTable = dbLicenseKey.getdata("SELECT * FROM maxonet_tblKeyCode WHERE KeyCode_IsActive = 1 And KeyCode_Code = '" & Password & "' 
                                                    And KeyCode_Username = '" & UserName & "';")

        If dt.Rows.Count > 0 Then
            'KeyCode ถูกต้อง
            sql = "select distinct KCU_DeviceId,KCU_Token,kcu_OwnerId,KCU_LimitExamA,ujujuj                                                                                                                                              zmount,KCU_ExpireDate,KCU_CreditAmount
                             from maxOnet_tblKeycodeUsage where KeyCode_Code = '" & Password & "'"

            Dim dtKeycodeDetail As DataTable = db.getdata(sql)
            If dtKeycodeDetail.Rows.Count > 0 Then
                'ลงทะเบียนแล้ว
                sql &= " and (KCU_ExpireDate > dbo.GetThaiDate() or KCU_ExpireDate is null)"
                Dim dtCheckExpire As DataTable = db.getdata(sql)
                If dtCheckExpire.Rows.Count > 0 Then
                    sql &= " and KCU_Type = '4'"
                    Dim dtCheckPCUser As DataTable = db.getdata(sql)
                    If dtCheckPCUser.Rows.Count > 0 Then
                        'ไม่หมดอายุ/ลงทะเบียนใน PC แล้ว
                        Session("DeviceId") = dtCheckExpire.Rows(0)("KCU_DeviceId").ToString
                        Session("TokenId") = dtCheckExpire.Rows(0)("KCU_Token").ToString
                        Return "OK," & dtCheckExpire.Rows(0)("KCU_DeviceId").ToString & "," & dtCheckExpire.Rows(0)("KCU_Token").ToString & "," & dtCheckExpire.Rows(0)("kcu_OwnerId").ToString
                    Else
                        'ไม่หมดอายุ/ยังไม่ลงทะเบียนใน PC/Copy ข้อมูลมาลทะเบียน
                        Dim NewDeviceId As String = Guid.NewGuid.ToString
                        Dim NewTokenId As String = Guid.NewGuid.ToString
                        Dim NewTabId As String = Guid.NewGuid.ToString

                        sql = "Select School_Code from maxonet_tblKeyCodeUsage inner join t360_tblStudent On KCU_OwnerId = Student_Id 
                                where KCU_OwnerId = '" & dtCheckExpire.Rows(0)("kcu_OwnerId").ToString & "';"

                        Dim SchoolCode As String = db.ExecuteScalar(sql)

                        sql = "insert into maxonet_tblKeyCodeUsage select top 1 newid(),'" & Password & "','" & NewTokenId & "','" & NewDeviceId & "',1,dbo.GetThaiDate(),4,
                                dbo.GetThaiDate(),KCU_OwnerId,KCU_LimitExamAmount,KCU_ExpireDate,KCU_CreditAmount from maxonet_tblKeyCodeUsage where (KeyCode_Code = '" & Password & "' and KCU_Type = 0);"
                        sql &= " insert into t360_tblTablet select '" & NewTabId & "','" & SchoolCode & "','" & NewDeviceId & "',1,1,1,'',
                                dbo.GetThaiDate(),dbo.GetThaiDate(),null,null;"
                        sql &= "insert into t360_tblTabletOwner select newid(),'" & NewTabId & "','" & dtCheckExpire.Rows(0)("kcu_OwnerId").ToString & "',
                                '" & SchoolCode & "',2,dbo.GetThaiDate(),null,1,dbo.GetThaiDate(),null;"
                        db.Execute(sql)

                        Session("DeviceId") = NewDeviceId
                        Session("TokenId") = NewTokenId
                        Return "OK," & NewDeviceId & "," & NewTokenId & "," & dtCheckExpire.Rows(0)("kcu_OwnerId").ToString
                    End If
                Else
                    'ลงทะเบียนแล้ว/หมดอายุ
                    Return "Expired," & dtCheckExpire.Rows(0)("KCU_DeviceId").ToString & "," & dtCheckExpire.Rows(0)("KCU_Token").ToString & "," & dtCheckExpire.Rows(0)("kcu_OwnerId").ToString
                End If
            Else
                'ยังไม่ได้ลงทะเบียน
                Return "InstallDevice,0"
            End If
        End If

        'Keycode ผิด
        Return "False"
    End Function
    Private Function GetUserNameAndPassword(tokenId As String) As String
        Dim db As New ClassConnectSql
        Dim sql As String = "Select u.UserName,u.Password FROM tbluser u INNER JOIN t360_tblQRToken q On u.GUID = q.UserId WHERE q.IsActive = 1 And q.QRTokenId = '" & tokenId & "';"
        Dim dt As DataTable = db.getdata(sql)
        If dt.Rows.Count > 0 Then

            Return dt.Rows(0)("UserName") & "," & dt.Rows(0)("Password")
        End If
        Return "False"
    End Function


    ''' <summary>
    ''' ทำการสุ่มเลข 5 หลักเพื่อนำไปตั้งเป็นชื่อรูป
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        'loop เพื่อทำการสุ่มเลข 1-9 เพื่อนำไปต่อสตริงให้เป็นชื่อรูปภาพ , เงื่อนไขการจบ loop คือ วน 5 รอบ
        For i = 1 To 5
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

    Private Function Convert1bpp(Img As Bitmap) As Bitmap

        For i As Integer = 0 To Img.Width - 1
            For j As Integer = 0 To Img.Height - 1
                Dim col As Color = Img.GetPixel(i, j)
                Dim gray As Integer = (CInt(col.R) + CInt(col.G) + CInt(col.B)) / 3
                Img.SetPixel(i, j, Color.FromArgb(gray, gray, gray))
            Next
        Next
        Return Img
    End Function

    Private Function ResizeImage(originalImage As Bitmap, maxWidth As Integer, maxHeight As Integer) As Bitmap
        'Dim originalImage As New Bitmap(streamImage)
        Dim newWidth As Integer = originalImage.Width
        Dim newHeight As Integer = originalImage.Height
        Dim aspectRatio As Double = CDbl(originalImage.Width) / CDbl(originalImage.Height)
        If aspectRatio <= 1 AndAlso originalImage.Width > maxWidth Then
            newWidth = maxWidth
            newHeight = CInt(Math.Round(newWidth / aspectRatio))
        ElseIf aspectRatio > 1 AndAlso originalImage.Height > maxHeight Then
            newHeight = maxHeight
            newWidth = CInt(Math.Round(newHeight * aspectRatio))
        End If
        Dim newImage As New Bitmap(originalImage, newWidth, newHeight)
        Dim g As Graphics = Graphics.FromImage(newImage)
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
        g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height)
        originalImage.Dispose()
        Return newImage
    End Function


    <WebMethod(EnableSession:=True)>
    Public Function CheckIsNowDevice(DeviceId As String, UserId As String) As String
        Dim redis As New RedisStore()
        'User นี้ ใช้เครื่องไหนอยู่
        Dim NowDevice As String = redis.Getkey("NowDevice_" & UserId)

        If NowDevice = "" Then
            'ถ้าไม่มี แสดงว่าเข้ามาครั้งแรก SetKey ใหม่
            redis.SetKey("NowDevice_" & UserId, DeviceId)
            Return "True"
        Else
            'ถ้ามี ตรวจสอบว่า DeviceId  ตรงกันมั้ย
            If NowDevice = DeviceId Then
                ''ตรง แสดงว่าเครื่องเดิมเล่นต่อ ผ่านไปได้เลย
                Return "True"
            Else
                ''ไม่ตรง แสดงว่าเครื่องใหม่เข้ามา ให้แสดง Dialog ถามว่าจะออกจากเครื่องเก่ามาใช้เครื่องใหม่เลยมั้ย
                Return "False"
            End If
        End If

        Return "False"
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetNowDeviceToRedis(DeviceId As String) As Boolean
        Dim redis As New RedisStore()
        redis.DEL("NowDevice_" & Session("UserId"))
        redis.SetKey("NowDevice_" & Session("UserId"), DeviceId)
        Session("DeviceId") = DeviceId
        Return True
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetStatusSession(StatusType As String) As Boolean
        Session("SessionStatus") = StatusType
    End Function
End Class