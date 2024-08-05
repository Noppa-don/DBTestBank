Imports System.Data.SqlClient
Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page
    Public BrowserName As String
    Public BrowserVersion As Double
    Public Property OSName As String
        Get
            OSName = ViewState("_OSName")
        End Get
        Set(value As String)
            ViewState("_OSName") = value
        End Set
    End Property
    Protected IsDBError As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            'Check ว่า Activate ?
            If ClsLanguage.Chklang() = True Then
                'Check DB ก่อน
                If CheckDB() = True Then
                    'Check ว่าถ้าเป็น localDB แล้ว DB มีปัญหาต้องขึ้น dialog แจ้ง User
                    CheckLocalDBIsError()
                    'Detect Browser
                    Dim objBrowser As HttpBrowserCapabilities = HttpContext.Current.Request.Browser
                    'Detect OS
                    Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
                    If objBrowser IsNot Nothing And AgentString <> "" Then
                        BrowserName = objBrowser.Browser()
                        BrowserVersion = CType(objBrowser.Version(), Double)
                        'เช็คว่า OS เป็น Window หรือเปล่า
                        If AgentString.ToLower().IndexOf("windows") <> -1 Then
                            OSName = "windows"
                            If BrowserName.ToLower() = "ie" Then
                                If BrowserVersion < 8 Then
                                    BtnSubmit.Visible = False
                                    Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                    lblHead.Text = "PointPlus ใช้กับ IE เก่ากว่าเวอร์ชั่น 8 หรือ Firefox เก่ากว่าเวอร์ชั่น 3.5 ไม่ได้ค่ะ, ติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหล และเร็วกว่าเดิมมั้ยคะ?"
                                ElseIf BrowserVersion >= 8 Then
                                    If CheckCookiesPassToLogIn() = True Then
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    Else
                                        Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                        lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                        BtnSubmit.Visible = True
                                    End If
                                End If
                            ElseIf BrowserName.ToLower() = "firefox" Then
                                If BrowserVersion < 3.5 Then
                                    BtnSubmit.Visible = False
                                    Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                    lblHead.Text = "PointPlus ใช้กับ IE เก่ากว่าเวอร์ชั่น 8 หรือ Firefox เก่ากว่าเวอร์ชั่น 3.5 ไม่ได้ค่ะ, ติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหล และเร็วกว่าเดิมมั้ยคะ?"
                                ElseIf BrowserVersion >= 3.5 Then
                                    If CheckCookiesPassToLogIn() = True Then
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    Else
                                        Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                        lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                        BtnSubmit.Visible = True
                                    End If
                                End If
                            ElseIf BrowserName.ToLower() = "chrome" Then
                                If BrowserVersion < 32 Then
                                    If CheckCookiesPassToLogIn() = True Then
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    Else
                                        Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                        lblHead.Text = "ปรับปรุง Google Chrome Browser ให้เป็นเวอร์ชั่นใหม่เร็วล่าสุดมั้ยคะ?"
                                        BtnSubmit.Visible = True
                                    End If
                                Else
                                    If IsDBError <> "true" Then
                                        Response.Redirect("~/LoginPage.aspx")
                                    Else
                                        BtnSubmit.Visible = True
                                    End If
                                End If
                            Else 'กรณีที่เป็น Browser อื่นๆ
                                If CheckCookiesPassToLogIn() = True Then
                                    If IsDBError <> "true" Then
                                        Response.Redirect("~/LoginPage.aspx")
                                    Else
                                        BtnSubmit.Visible = True
                                    End If
                                Else
                                    Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                    lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                    BtnSubmit.Visible = True
                                End If
                            End If
                        ElseIf AgentString.ToLower().IndexOf("mac") <> -1 Then 'ถ้าเป็น Mac OS X
                            If AgentString.ToLower().IndexOf("iphone") <> -1 Or AgentString.ToLower().IndexOf("ipad") Or AgentString.ToLower().IndexOf("ipod") Then 'ถ้าเป็น iPhone,iPad,iPod
                                OSName = "ios"
                                If ClsKNSession.IsMaxONet Then
                                    Response.Redirect("~/MaxonetParking.htm")
                                Else
                                    If BrowserName.ToLower() <> "chrome" Then
                                        BtnSubmit.Visible = False
                                        Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                        lblHead.Text = "PointPlus Version IOS ใช้ได้กับ Google Chrome เท่านั้นค่ะ"
                                    Else
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    End If
                                End If
                            ElseIf AgentString.ToLower().IndexOf("os x") <> -1 Then
                                OSName = "mac"
                                If BrowserName.ToLower().IndexOf("chrome") = -1 Then 'ถ้า Browser ไมใช่ Chrome
                                    If CheckCookiesPassToLogIn() = True Then
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    Else
                                        Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                        lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                        BtnSubmit.Visible = True
                                    End If
                                Else 'ถ้า Browser เป็น Chrome
                                    If BrowserVersion < 30 Then ' แต่เป็น Chrome Version ที่น้อยกว่า 30 
                                        If CheckCookiesPassToLogIn() = True Then
                                            If IsDBError <> "true" Then
                                                Response.Redirect("~/LoginPage.aspx")
                                            Else
                                                BtnSubmit.Visible = True
                                            End If
                                        Else
                                            Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                            lblHead.Text = "ปรับปรุง Google Chrome Browser ให้เป็นเวอร์ชั่นใหม่เร็วล่าสุดมั้ยคะ?"
                                            BtnSubmit.Visible = True
                                        End If
                                    Else
                                        If IsDBError <> "true" Then
                                            Response.Redirect("~/LoginPage.aspx")
                                        Else
                                            BtnSubmit.Visible = True
                                        End If
                                    End If
                                End If
                            Else
                                If CheckCookiesPassToLogIn() = True Then
                                    If IsDBError <> "true" Then
                                        Response.Redirect("~/LoginPage.aspx")
                                    Else
                                        BtnSubmit.Visible = True
                                    End If
                                Else
                                    Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                    lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                    BtnSubmit.Visible = True
                                End If
                            End If
                        ElseIf AgentString.ToLower().IndexOf("android") <> -1 OrElse AgentString.IndexOf("iphone") <> -1 OrElse AgentString.IndexOf("ipad") <> -1 Then
                            If ClsKNSession.IsMaxONet Then
                                Response.Redirect("~/MaxonetParking.htm")
                            End If
                        Else 'กรณีที่เป็น OS อื่นๆ
                            OSName = "linux"
                            If CheckCookiesPassToLogIn() = True Then
                                If IsDBError <> "true" Then
                                    Response.Redirect("~/LoginPage.aspx")
                                Else
                                    BtnSubmit.Visible = True
                                End If
                            Else
                                Log.Record(Log.LogType.BrowserAgentNotChrome, AgentString & " - " & BrowserName & " - " & BrowserVersion, False)
                                lblHead.Text = "PointPlus ใช้กับ Google Chrome ดีที่สุดนะคะ, ลองติดตั้ง Google Chrome Browser เพื่อใช้งานแบบลื่นไหลเร็วกว่าเดิมมั้ยคะ?"
                                BtnSubmit.Visible = True
                            End If
                        End If
                    Else
                        If IsDBError <> "true" Then
                            Response.Redirect("~/LoginPage.aspx")
                        Else
                            BtnSubmit.Visible = True
                        End If
                    End If
                Else
                    'Redirect ไปหน้า DB Error
                    Response.Redirect("~/error/404.htm")
                End If
            Else
                Response.Redirect("~/QuickTestFirstStartPage.aspx")
            End If
        End If
    End Sub

    Private Function CheckCookiesPassToLogIn() As Boolean
        Dim CounterCheck As Integer = GetValueFromCookies()
        If CounterCheck >= 3 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetValueFromCookies(Optional ByVal AddMoreCounter As Boolean = False) As Integer
        Dim httpCookies As HttpCookie = Request.Cookies("Counter")
        If httpCookies Is Nothing Then
            httpCookies = New HttpCookie("Counter")
            If AddMoreCounter = True Then
                httpCookies.Value = 1
            Else
                httpCookies.Value = 0
            End If
            httpCookies.Expires = DateAdd(DateInterval.Day, +1, Date.Now())
        Else
            Dim PageCount As Integer = CInt(httpCookies.Value)
            If AddMoreCounter = True Then
                httpCookies.Value = (PageCount + 1).ToString()
            End If
            httpCookies.Expires = DateAdd(DateInterval.Day, +1, Date.Now())
        End If
        Response.Cookies.Add(httpCookies)
        Return CInt(httpCookies.Value)
    End Function

    Private Sub BtnDownload_Click(sender As Object, e As ImageClickEventArgs) Handles BtnDownload.Click
        Response.Redirect("~/Default2.aspx?OSName=" & OSName)
    End Sub

    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles BtnSubmit.Click
        GetValueFromCookies(True)
        Response.Redirect("~/LoginPage.aspx")
    End Sub

#Region "CheckDB"
    Private Function CheckDB() As Boolean
        'ต้องไปอ่าน ConnectionString จาก langset.bin แทน
        'Dim ConnStr As String = KNConfigData.DecryptData(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        Dim ConnStr As String = ClsLanguage.GetConStr()
        If ConnStr.ToLower().IndexOf("localdb") <> -1 Then
            'ถ้าเป็น localDb ต้องลง If นี้
            'เช็คก่อนว่ามีไฟล์ DB.bin หรือยัง ถ้ายังไม่มีให้สร้างไฟล์ + เขียน DB เริ่มแรกเป็น 1
            ClsLanguage.CheckTextFileDBIsExistAlready()
            'อ่านจากไฟล์ขึ้นมาว่าให้เริ่มวนจาก DB อะไร
            Dim IndexDB As Integer = ClsLanguage.ReadFileData()
            If IndexDB <> 0 Then
                'ถ้าเป็นก้อนที่ 1
                If IndexDB = 1 Then
                    'ConnStr = GetStrNextDB(ConnStr, IndexDB)
                    If MainCheckLocalDB(ConnStr) = True Then
                        ClsLanguage.WriteFileData(1)
                        Return True
                    Else 'ถ้าพังมาตรวจก้อนที่ 2
                        IndexDB += 1
                        ConnStr = GetStrDB(ConnStr, IndexDB)
                        If MainCheckLocalDB(ConnStr) = True Then
                            ClsLanguage.WriteFileData(2)
                            Return True
                        Else 'ถ้าพังมาตรวจก้อนที่ 3
                            IndexDB += 1
                            ConnStr = GetStrDB(ConnStr, IndexDB)
                            If MainCheckLocalDB(ConnStr) = True Then
                                ClsLanguage.WriteFileData(3)
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    End If
                    'ถ้าเป็นก้อนที่ 2
                ElseIf IndexDB = 2 Then
                    ConnStr = GetStrDB(ConnStr, IndexDB)
                    If MainCheckLocalDB(ConnStr) = True Then
                        ClsLanguage.WriteFileData(2)
                        Return True
                    Else 'ถ้าพังต้องมาตรวจก้อนที่ 3
                        IndexDB += 1
                        ConnStr = GetStrDB(ConnStr, IndexDB)
                        If MainCheckLocalDB(ConnStr) = True Then
                            ClsLanguage.WriteFileData(3)
                            Return True
                        Else
                            Return False
                        End If
                    End If
                ElseIf IndexDB = 3 Then 'ถ้าเป็นก้อนที่ 3
                    ConnStr = GetStrDB(ConnStr, IndexDB)
                    If MainCheckLocalDB(ConnStr) = True Then
                        ClsLanguage.WriteFileData(3)
                        Return True
                    Else
                        Return False
                    End If
                End If
            Else
                Return False
            End If
            
        Else
            'ถ้าเป็น DB แบบปกติ
            If CheckConnect(ConnStr) = True Then 'เช็คว่าเปิด Connection ได้หรือเปล่า ?
                If CheckQuery(ConnStr) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If
    End Function

   
    Private Function GetStrDB(ByVal CurrentConnString As String, ByVal IndexDB As Integer) As String
        'Dim NextNumberOfDB As String = GetNumberOfDB(CurrentConnString) + 1
        Dim NewConnStr As String = ""
        If CurrentConnString.IndexOf("qt") <> -1 Then
            Dim Position1 As Integer = CurrentConnString.IndexOf("qt")
            Dim DBOld As String = CurrentConnString.Substring(Position1, 3)
            NewConnStr = CurrentConnString.Replace(DBOld, "qt" & IndexDB)
        End If
        Return NewConnStr
    End Function

    Private Function GetNumberOfDB(ByVal InputConnStr As String) As Integer
        If InputConnStr.ToLower().IndexOf("qt") > -1 Then
            Dim NumberPosition As Integer = InputConnStr.ToLower().IndexOf("qt") + 2
            Dim ReturnNumber As Integer = InputConnStr.Substring(NumberPosition, 1)
            Return ReturnNumber
        End If
    End Function

    Private Function MainCheckLocalDB(ByVal ConnStr As String, Optional ByVal IsCurrentConnStrInWebConfig As Boolean = False) As Boolean
        If CheckConnect(ConnStr) = True Then
            'QueryCheck
            If CheckQuery(ConnStr) = True Then
                'If IsCurrentConnStrInWebConfig = False Then
                'เปลี่ยน ConnectionString WebConfig
                'Dim EnCryptConStr As String = KNConfigData.KnEncryption(ConnStr)
                'Dim WebCf As System.Configuration.Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
                'WebCf.ConnectionStrings.ConnectionStrings("ConnectionString").ConnectionString = EnCryptConStr
                'WebCf.Save()
                'End If
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Function CheckConnect(Optional ByVal ConnStr As String = "") As Boolean
        Dim _DB As New ClassConnectSql(False, ConnStr)
        Try
            Dim ConnCheck As New SqlConnection
            _DB.OpenExclusiveConnect(ConnCheck)
            _DB = Nothing
            Return True
        Catch ex As Exception
            _DB = Nothing
            Return False
        End Try
    End Function

    Private Function CheckQuery(Optional ByVal ConnStr As String = "") As Boolean
        Dim _DB As New ClassConnectSql(False, ConnStr)
        Try
            'Check 1
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuestion; "
            Dim CountCheck As String = _DB.ExecuteScalar(sql)
            'Check 2
            sql = " SELECT TOP 1 Quiz_Id FROM dbo.tblQuizScore; "
            CountCheck = _DB.ExecuteScalar(sql)
            'Check 3 
            sql = " SELECT TOP 1 Qset_Id FROM tblQuestionSet; "
            CountCheck = _DB.ExecuteScalar(sql)
            'Check 4 
            sql = " SELECT TOP 1 Answer_Id FROM dbo.tblAnswer ; "
            CountCheck = _DB.ExecuteScalar(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub CheckLocalDBIsError()
        Dim ConnStr As String = ClsLanguage.GetConStr()
        If ConnStr.ToLower().IndexOf("localdb") <> -1 Then
            Dim ReadIndexDB As Integer = ClsLanguage.ReadFileData()
            If ReadIndexDB <> 1 Then
                IsDBError = "true"
            End If
        End If
    End Sub

#End Region


End Class