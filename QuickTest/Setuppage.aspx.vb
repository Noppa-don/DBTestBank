Imports KnowledgeUtils
Imports System.Net
Imports System.Data.SqlClient
Imports System.Web

Public Class Setuppage
    Inherits System.Web.UI.Page
    'ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()

    'ตัวแปรที่ไว้บอกว่าตอนนี้อยู่ในขั้นตอนไหน เพื่อที่จะได้เอาไว้ดักที่ปุ่ม ถัดไป และ ย้อนกลับ ว่าควรจะทำอะไรต่อหลังจากกดปุ่ม และใช้หลังจาก PostBack ได้ เพราะเก็บไว้ใน ViewState
    Public Property CurrentStep As Integer
        Get
            CurrentStep = ViewState("_CurrentStep")
        End Get
        Set(value As Integer)
            ViewState("_CurrentStep") = value
        End Set
    End Property

    ''' <summary>
    ''' ทำการ Bind ข้อมูลลงใน Control ในกรณีที่มีการกรอกข้อมูลไว้แล้ว
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CurrentStep = 1
            'HttpContext.Current.Application("ConnectedWithT360") = True
            'Session("SchoolID") = "1000002"
            'ต้องหาค่าก่อนว่า รร. นี้เคยมีข้อมูหรือยัง ถ้ามีข้อมูลแล้วให้ Select ข้อมูลมาใส่ txtbox และ Update แทน
            If CheckIsHaveData() = True Then
                BinDataToAlltxt()
                HttpContext.Current.Session("IsUpdate") = False    'True
            Else
                HttpContext.Current.Session("IsUpdate") = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Function ที่ทำการรับ InputIP ที่เป็น Pattern  'InputStrIP = "010.100.001.116" แบบนี้เข้าไปแปลงให้เป็นตัวเลขเพื่อนำไปแปลงเป็นค่า IpAddress อีกทีนึง
    ''' </summary>
    ''' <param name="InputStrIP">Ipที่ต้องการแปลงค่า</param>
    ''' <returns>String:ตัวเลขเพื่อนำไปเข้า System.Net.IPAddress.Parse() อีกทีนึง</returns>
    ''' <remarks></remarks>
    Private Shared Function ChangeFormatStringToLong(ByVal InputStrIP As String) As String
        Dim octests As New List(Of String)(InputStrIP.Split("."))
        Dim ipAsHex As New StringBuilder()
        octests.ForEach(Sub(value As String) ipAsHex.AppendFormat("{0:X2}", Integer.Parse(value)))
        Dim decimalIP As Long = Long.Parse(ipAsHex.ToString(), Globalization.NumberStyles.HexNumber)
        Return decimalIP.ToString()
    End Function

    ''' <summary>
    ''' ทำการแสดง Panel ถัดไปเรื่อยๆ แต่ถ้าครบหมดแล้วให้ทำการ save ข้อมูลแทน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnNext_Click(sender As Object, e As EventArgs) Handles BtnNext.Click
        CurrentStep += 1
        BtnPrev.Visible = True

        If CurrentStep = 2 Then
            PanelStep1.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep2.Visible = True
        ElseIf CurrentStep = 3 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep3.Visible = True
        ElseIf CurrentStep = 4 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep4.Visible = True
        ElseIf CurrentStep = 5 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep5.Visible = True
            'If HttpContext.Current.Application("ConnectedWithT360") = True Then
            '    BtnNext.Visible = False
            '    BtnSave.Visible = True
            'End If
        ElseIf CurrentStep = 6 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False

            'ตรงนี้จะแสดงคนละ Panel กันถ้าไม่ได้เชื่อมต่อกับ T360 จะแสดง Panel ให้ set ปีการศึกษา แต่ถ้าเชื่อมต่อจะแสดง Panel ให้ Set ParentServer IP
            If HttpContext.Current.Application("ConnectedWithT360") = True Then
                PanelStep6When360False.Visible = True
            Else
                PanelStep6.Visible = True
            End If
            BtnNext.Visible = False
            BtnSave.Visible = True
        End If

    End Sub

    ''' <summary>
    ''' ทำการซ่อน แสดง Panel เช่นเดียวกับปุ่ม ถัดไป แต่เมื่อถอยกลับมาจนสุดแล้วต้องซ่อนปุ่มถอยไป เพื่อไม่ให้กดได้อีก
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnPrev_Click(sender As Object, e As EventArgs) Handles BtnPrev.Click
        CurrentStep -= 1

        If CurrentStep = 1 Then
            BtnPrev.Visible = False

            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep1.Visible = True
        ElseIf CurrentStep = 2 Then
            PanelStep1.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep2.Visible = True
        ElseIf CurrentStep = 3 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep4.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            PanelStep3.Visible = True
        ElseIf CurrentStep = 4 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep5.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False
            'If HttpContext.Current.Application("ConnectedWithT360") = True Then
            '    BtnSave.Visible = False
            '    BtnNext.Visible = True
            'End If
            PanelStep4.Visible = True
        ElseIf CurrentStep = 5 Then
            PanelStep1.Visible = False
            PanelStep2.Visible = False
            PanelStep3.Visible = False
            PanelStep4.Visible = False
            PanelStep6.Visible = False
            PanelStep6When360False.Visible = False

            BtnSave.Visible = False
            BtnNext.Visible = True
            PanelStep5.Visible = True
            'ElseIf CurrentStep = 6 Then
            '    PanelStep1.Visible = False
            '    PanelStep2.Visible = False
            '    PanelStep3.Visible = False
            '    PanelStep4.Visible = False
            '    PanelStep5.Visible = False

            '    PanelStep6.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Update/Insert ข้อมูล 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If HttpContext.Current.Application("ConnectedWithT360") = False Then
            If Page.IsValid = False Then
                Exit Sub
            End If
        End If
        'ทำการแปลงค่าข้อมูลต่างๆให้ถูกต้อง เพื่อเตรียมที่จะนำไป Insert อีกทอดนึง
        Dim IP As String = TranfromToIPFormat(txtIP.Text)
        Dim GW As String = TranfromToIPFormat(txtGW.Text)
        Dim DNS1 As String = TranfromToIPFormat(txtDNS1.Text)
        Dim DNS2 As String = TranfromToIPFormat(txtDNS2.Text)
        Dim ProxyIP As String = ReformStrToInsert(txtProxyIP.Text, True)
        Dim ProxyPort As String = CInt(ReformIntToInsert(txtProxyPort.Text))
        Dim ProxyUser As String = ReformStrToInsert(txtProxyUser.Text)
        Dim ProxyPassword As String = ReformStrToInsert(txtProxyPassword.Text)
        Dim SMTPServerIP As String = ReformStrToInsert(txtSMTPServerIP.Text, True)
        Dim SMTPServerPort As String = CInt(ReformIntToInsert(txtSMTPServerPort.Text))
        Dim SMTPServerUser As String = ReformStrToInsert(txtSMTPUser.Text)
        Dim SMTPServerPassword As String = ReformStrToInsert(txtSMTPPassword.Text)
        Dim AskQuestionRecipient As String = ReformStrToInsert(txtAskQuestionRecipient.Text)
        Dim RptConsultantRecipient As String = ReformStrToInsert(txtRptConsultantRecipient.Text)
        Dim ParentServerIP As String = TranfromToIPFormat(txtParentServerIp.Text).Replace("'", "")
        Dim StartDate As String = "dbo.GetThaiDate()"
        If RadDatePicker1.SelectedDate.HasValue = True Then
            StartDate = "'" & RadDatePicker1.SelectedDate.Value.Date.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("EN")) & "'"
        End If
        Dim EndDate As String = "NULL"
        If RadDatePicker2.SelectedDate.HasValue = True Then
            EndDate = "'" & RadDatePicker2.SelectedDate.Value.Date.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.CreateSpecificCulture("EN")) & "'"
        End If
        Dim CalendarName As String = txtCalendarName.Text
        Dim CalendarYear As String = txtCalendarYear.Text
        Dim Mask As String = TranfromToIPFormat(txtMask.Text)
        'ดูว่าต้องทำการ Insert หรือ Update ข้อมูล
        If HttpContext.Current.Session("IsUpdate") = True Then
            UpdateData(IP, GW, DNS1, DNS2, ProxyIP, ProxyPort, ProxyUser, ProxyPassword, SMTPServerIP, SMTPServerPort, SMTPServerUser, SMTPServerPassword, AskQuestionRecipient, RptConsultantRecipient, StartDate, EndDate, CalendarName, CalendarYear, Mask, ParentServerIP)
        Else
            SaveData(IP, GW, DNS1, DNS2, ProxyIP, ProxyPort, ProxyUser, ProxyPassword, SMTPServerIP, SMTPServerPort, SMTPServerUser, SMTPServerPassword, AskQuestionRecipient, RptConsultantRecipient, StartDate, EndDate, CalendarName, CalendarYear, Mask, ParentServerIP)
        End If

    End Sub

    ''' <summary>
    ''' Javscript ทำการ POST มาเพื่อเรียกให้เปลี่ยน IP
    ''' </summary>
    ''' <param name="IP">IPAddress ที่ต้องการเปลี่ยน</param>
    ''' <param name="GateWay">GateWay ที่ต้องการเปลี่ยน</param>
    ''' <param name="SubnetMask">SubnetMask ที่ต้องการเปลี่ยน</param>
    ''' <param name="DNS1">DNS1 ที่ต้องการเปลี่ยน</param>
    ''' <param name="DNS2">DNS2 ที่ต้องการเปลี่ยน</param>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Sub ChangeIP(ByVal IP As String, ByVal GateWay As String, ByVal SubnetMask As String, ByVal DNS1 As String, ByVal DNS2 As String)
        'IP = ChangeFormatStringToLong(IP)
        'GateWay = ChangeFormatStringToLong(GateWay)
        'SubnetMask = ChangeFormatStringToLong(SubnetMask)
        'DNS1 = ChangeFormatStringToLong(DNS1)
        If DNS2 <> "" Then
            DNS2 = ChangeFormatStringToLong(DNS2)
            KnowledgeUtils.ManageIP.SetNewIP("ton", "12345", "", IPAddress.Parse(IP), IPAddress.Parse(GateWay), IPAddress.Parse(SubnetMask), IPAddress.Parse(DNS1), "Local Area Connection", IPAddress.Parse(DNS2))
        Else
            KnowledgeUtils.ManageIP.SetNewIP("ton", "12345", "", IPAddress.Parse(IP), IPAddress.Parse(GateWay), IPAddress.Parse(SubnetMask), IPAddress.Parse(DNS1), "Local Area Connection")
        End If
    End Sub

    ''' <summary>
    ''' ทำการเรียกโปรแกรม SetProxy.exe โดยดู Path ของโปรแกรมได้จาก Webconfig ที่ AppSetting "SetProxyProgramPath" เพื่อทำการ Set Proxy ใหม่
    ''' </summary>
    ''' <param name="Address">Proxy IP Address</param>
    ''' <param name="Port">Proxy Port</param>
    ''' <remarks></remarks>
    Private Sub SetProxy(ByVal Address As String, Optional ByVal Port As Integer = 80)
        Try
            'set by program setproxy.exe
            'clear current proxy
            Shell(ConfigurationManager.AppSettings("SetProxyProgramPath") & " " & """", AppWinStyle.Hide, True)
            'set new proxy setting
            Shell(ConfigurationManager.AppSettings("SetProxyProgramPath") & " " & Address & ":" & Port.ToString(), AppWinStyle.Hide, True)
            'set netsh winhttp proxy
            Shell("netsh winhttp set proxy " & Address & ":" & Port.ToString(), AppWinStyle.Hide, True)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ select ข้อมูลเพื่อนำมา Bind ใส่ Control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BinDataToAlltxt()
        Dim sql As String = " SELECT * FROM dbo.tblSetEmail WHERE SchoolId = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("IP") IsNot DBNull.Value Then
                txtIP.Text = dt.Rows(0)("IP")
            End If
            If dt.Rows(0)("Mask") IsNot DBNull.Value Then
                txtMask.Text = dt.Rows(0)("Mask")
            End If
            If dt.Rows(0)("GW") IsNot DBNull.Value Then
                txtGW.Text = dt.Rows(0)("GW")
            End If
            If dt.Rows(0)("DNS1") IsNot DBNull.Value Then
                txtDNS1.Text = dt.Rows(0)("DNS1")
            End If
            If dt.Rows(0)("DNS2") IsNot DBNull.Value Then
                txtDNS2.Text = dt.Rows(0)("DNS2")
            End If
            If dt.Rows(0)("ProxyIP") IsNot DBNull.Value Then
                txtProxyIP.Text = dt.Rows(0)("ProxyIP")
            End If
            If dt.Rows(0)("ProxyPort") IsNot DBNull.Value Then
                txtProxyPort.Text = dt.Rows(0)("ProxyPort")
            End If
            If dt.Rows(0)("ProxyUser") IsNot DBNull.Value Then
                txtProxyUser.Text = dt.Rows(0)("ProxyUser")
            End If
            If dt.Rows(0)("ProxyPassword") IsNot DBNull.Value Then
                txtProxyPassword.Text = dt.Rows(0)("ProxyPassword")
            End If
            If dt.Rows(0)("SMTPServerIP") IsNot DBNull.Value Then
                txtSMTPServerIP.Text = dt.Rows(0)("SMTPServerIP")
            End If
            If dt.Rows(0)("SMTPServerPort") IsNot DBNull.Value Then
                txtSMTPServerPort.Text = dt.Rows(0)("SMTPServerPort")
            End If
            If dt.Rows(0)("SMTPServerUser") IsNot DBNull.Value Then
                txtSMTPUser.Text = dt.Rows(0)("SMTPServerUser")
            End If
            If dt.Rows(0)("SMTPServerPassword") IsNot DBNull.Value Then
                txtSMTPPassword.Text = dt.Rows(0)("SMTPServerPassword")
            End If
            If dt.Rows(0)("AskQuestionRecipient") IsNot DBNull.Value Then
                txtAskQuestionRecipient.Text = dt.Rows(0)("AskQuestionRecipient")
            End If
            If dt.Rows(0)("RptConsultantRecipient") IsNot DBNull.Value Then
                txtRptConsultantRecipient.Text = dt.Rows(0)("RptConsultantRecipient")
            End If
        End If
        'ถ้าไม่ได้ต่อกับ t360 ถึงจะเอาเทอมขึเนมาโชว์
        If HttpContext.Current.Application("ConnectedWithT360") = False Then
            sql = " SELECT TOP 1 Calendar_Name,Calendar_Year,Calendar_FromDate,Calendar_ToDate FROM dbo.t360_tblCalendar " &
                  " WHERE School_Code = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND Calendar_Type = 3 AND IsActive = 1 ORDER BY Calendar_ToDate DESC "
            dt.Clear()
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Calendar_Name") IsNot DBNull.Value Then
                    txtCalendarName.Text = dt.Rows(0)("Calendar_Name")
                End If
                If dt.Rows(0)("Calendar_Year") IsNot DBNull.Value Then
                    txtCalendarYear.Text = dt.Rows(0)("Calendar_Year")
                End If
                RadDatePicker1.SelectedDate = dt.Rows(0)("Calendar_FromDate")
                RadDatePicker2.SelectedDate = dt.Rows(0)("Calendar_ToDate")
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการ check ข้อมูลสตริง ถ้าเป็นค่าว่างจะทำการแปลงให้เป็น Null แทน
    ''' </summary>
    ''' <param name="InputStr">ข้อความที่นำเข้ามาเช็ค</param>
    ''' <param name="IsIP">เป็น IpAddress ?</param>
    ''' <returns>String:ข้อมูลที่ถูกต้องแล้ว</returns>
    ''' <remarks></remarks>
    Private Function ReformStrToInsert(ByVal InputStr As String, Optional ByVal IsIP As Boolean = False) As String
        If InputStr = "" Or InputStr = "000000000000" Then
            Return "NULL"
        Else
            'ถ้าเป็น IP ก็ต้องเข้าไปทำการแปลงค่าอีกทีนึง
            If IsIP = True Then
                InputStr = TranfromToIPFormat(InputStr)
                Return InputStr
            Else
                Return "'" & InputStr & "'"
            End If
        End If
    End Function

    ''' <summary>
    ''' ทำการเช็คค่าตัวแปรที่จะต้องเป็นตัวเลข ถ้าเป็นค่าว่างต้องแปลงให้เป็น 0 ไม่งั้น Insert ไม่ได้
    ''' </summary>
    ''' <param name="InputStr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReformIntToInsert(ByVal InputStr As String) As String
        If InputStr = "" Then
            Return "0"
        Else
            Return InputStr
        End If
    End Function

    ''' <summary>
    ''' ทำการแปลงค่า IPAddress ที่ดึงขึ้นมาจาก RadMaskEdit ให้ถูกต้อง
    ''' </summary>
    ''' <param name="InputStr">Ipที่ส่งเข้ามา</param>
    ''' <returns>String:Ipที่ทำการแปลงเรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Private Function TranfromToIPFormat(ByVal InputStr As String) As String
        If InputStr IsNot Nothing AndAlso InputStr <> "" Then
            If InputStr = "000000000000" Then Return "'0.0.0.0'"
            '000.000.000.000
            Dim result As String = ""
            'loop เพื่อนำ string มาเช็คทีละชุด เพื่อทำให้เป็น Format ที่ถูกต้อง โดย loop index เพิ่มครั้งละ 3 , เงื่อนไขการจบ loop คือวนจนครบ ทั้ง 4 ชุด
            For i As Integer = 0 To InputStr.Length - 1 Step 3
                Dim eachIpRange As Integer = CInt(InputStr.Substring(i, 3))
                'ถ้ามีค่า = 0 ให้เป็น 0 ไปเลย
                If eachIpRange = 0 Then
                    result &= "0."
                ElseIf eachIpRange < 10 Then
                    'เช่นมีค่าเป็น 009 ให้ทำเป็นสตริงหลักเดียว -> 9
                    result &= eachIpRange.ToString("#") & "."
                ElseIf eachIpRange < 100 Then
                    'เช่นมีค่าเป็น 099 ให้ทำเป็นสตริง 2 หลัก -> 99
                    result &= eachIpRange.ToString("##") & "."
                Else
                    'เช่นมีค่าเป็น 116 ก็ให้ทำเป็นสตริง 3 หลัก -> 116
                    result &= eachIpRange.ToString("###") & "."
                End If
            Next
            'ทำการลบ . ตัวสุดท้ายออกไป
            If result.EndsWith(".") Then
                result = result.Substring(0, result.Length - 1)
            End If
            Return "'" & result & "'"
            'InputStr = InputStr.Insert(3, ".")
            'InputStr = InputStr.Insert(7, ".")
            'InputStr = InputStr.Insert(11, ".")
            'Return "'" & InputStr & "'"
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูลใหม่เข้าไปที่ tblSetEmail และทำการ SetProxy , IPAddress ตามที่ Set มา
    ''' </summary>
    ''' <param name="IP">IP ที่จะทำการ Insert</param>
    ''' <param name="GW">GateWay ที่จะทำการ Insert</param>
    ''' <param name="DNS1">DNS1 ที่จะทำการ Insert</param>
    ''' <param name="DNS2">DNS2 ที่จะทำการ Insert</param>
    ''' <param name="ProxyIP">Proxy IPAddress ที่จะทำการ Insert</param>
    ''' <param name="ProxyPort">Proxy Port ที่จะทำการ Insert</param>
    ''' <param name="ProxyUser">ProxyUser ที่จะทำการ Insert</param>
    ''' <param name="ProxyPassword">ProxyPassword ที่จะทำการ Insert</param>
    ''' <param name="SMTPServerIp">SMTP ServerIP ที่จะทำการ Insert</param>
    ''' <param name="SMTPServerPort">SMTP ServerPort ที่จะทำการ Insert</param>
    ''' <param name="SMTPServerUser">SMTP Server User ที่จะทำการ Insert</param>
    ''' <param name="SMTPServerPassword">SMTP Server Password ที่จะทำการ Insert</param>
    ''' <param name="AskQuestionRecipient">Email ที่จะทำการรับเรื่องปัญหา</param>
    ''' <param name="RptConsultantRecipient">Email ที่จะส่ง Report ของ รร. ไปให้</param>
    ''' <param name="StartDate">วันที่ติดตั้ง</param>
    ''' <param name="EndDate">วันที่หมดอายุ</param>
    ''' <param name="CalendarName">ชื่อเทอม</param>
    ''' <param name="CalendarYear">ปีการศึกษา</param>
    ''' <param name="Mask">SubnetMask ที่จะทำการ Insert</param>
    ''' <param name="ParentServerIP">Server IP ที่จะทำการ Insert</param>
    ''' <remarks></remarks>
    Private Sub SaveData(ByVal IP As String, ByVal GW As String, ByVal DNS1 As String, ByVal DNS2 As String, ByVal ProxyIP As String, ByVal ProxyPort As String,
                         ByVal ProxyUser As String, ByVal ProxyPassword As String, ByVal SMTPServerIp As String, ByVal SMTPServerPort As String,
                         ByVal SMTPServerUser As String, ByVal SMTPServerPassword As String, ByVal AskQuestionRecipient As String, ByVal RptConsultantRecipient As String,
                         ByVal StartDate As String, ByVal EndDate As String, ByVal CalendarName As String, ByVal CalendarYear As String, ByVal Mask As String, ByVal ParentServerIP As String)
        Try
            'ถ้าเกิดใช้ Server ส่วนกลาง จะต้องใช้ DB ของส่วนกลาง ต้องทำการเปลี่ยน IP ของ Connection-String ในไฟล์ langset.bin ให้เป็น IP ของ Server ส่วนกลาง
            If HttpContext.Current.Application("ConnectedWithT360") = True Then
                ChangeConnectionStringToParentServerIP(ParentServerIP)
            End If
            _DB.OpenWithTransection()
            'Insert ข้อมูลลง tblSetEmail
            Dim sql As String = " INSERT INTO dbo.tblSetEmail ( SE_Id ,IP ,GW ,DNS1 ,DNS2 ,ProxyIP ,ProxyPort ,ProxyUser , " &
                                " ProxyPassword ,SMTPServerIP ,SMTPServerPort ,SMTPServerUser ,SMTPServerPassword , " &
                                " AskQuestionRecipient ,RptConsultantRecipient ,IsActive ,LastUpdate ,ClientId,SchoolId,Mask,ActivationDate) " &
                                " VALUES  ( NEWID() , " & IP & " , " & GW & " , " & DNS1 & " , " & DNS2 & " , " &
                                " " & ProxyIP & " , " & ProxyPort & " , " & ProxyUser & " , " & ProxyPassword & " ," & SMTPServerIp & " , " & SMTPServerPort & " ," & SMTPServerUser & " , " &
                                " " & SMTPServerPassword & " , " & AskQuestionRecipient & " , " & RptConsultantRecipient & " , 1 , dbo.GetThaiDate() ,NULL,'" & HttpContext.Current.Application("DefaultSchoolCode") & "'," & Mask & ",dbo.GetThaiDate() ) "
            _DB.ExecuteWithTransection(sql)
            'Insert ปีการศึกษาที่ t360_tblCalendar
            If HttpContext.Current.Application("ConnectedWithT360") = False Then
                sql = " INSERT INTO dbo.t360_tblCalendar( Calendar_Id ,School_Code ,Calendar_Year ,Calendar_Name , " &
                  " Calendar_FromDate ,Calendar_ToDate ,Calendar_Type ,LastUpdate,IsActive) VALUES  ( NEWID() , '" & HttpContext.Current.Application("DefaultSchoolCode") & "' , '" & CalendarYear & "' , " &
                  " '" & CalendarName & "' , " & StartDate & " , " & EndDate & " , 3 , dbo.GetThaiDate(),1 ) "
                _DB.ExecuteWithTransection(sql)
            End If
            If DNS2 = "'000.000.000.000'" Then
                DNS2 = "''"
            End If

            _DB.CommitTransection()
            'ทำการเปลี่ยน IP และ Set Proxy ตามที่เลือกมา
            SetProxyAndIPAddress(ProxyIP, ProxyPort, IP, GW, Mask, DNS1, DNS2)
            ''set proxy
            'If ProxyIP <> "NULL" Then SetProxy(ProxyIP.Replace("'", ""), ProxyPort.Replace("'", ""))
            ''set ip address
            'TriggerScript(IP, GW, Mask, DNS1, DNS2)
        Catch ex As Exception
            If ex.Message <> "Cannot Open Connection" Then
                _DB.RollbackTransection()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ Set Proxy ให้ในกรณีที่เลือกให้มี Proxy และ เปลี่ยน IPAddress ให้เป็นตามที่ Set เข้ามา
    ''' </summary>
    ''' <param name="ProxyIP">Proxy IPAddress</param>
    ''' <param name="ProxyPort">Proxy Port</param>
    ''' <param name="IP">IpAddress ของเครื่อง</param>
    ''' <param name="GW">Gateway ของเครื่อง</param>
    ''' <param name="Mask">SubnetMask ของเครื่อง</param>
    ''' <param name="DNS1">DNS1 ของเครื่อง</param>
    ''' <param name="DNS2">DNS2 ของเครื่อง</param>
    ''' <remarks></remarks>
    Private Sub SetProxyAndIPAddress(ProxyIP As String, ProxyPort As String, IP As String, GW As String, Mask As String, DNS1 As String, DNS2 As String)
        'set proxy
        If ProxyIP <> "NULL" Then SetProxy(ProxyIP.Replace("'", ""), ProxyPort.Replace("'", ""))
        'set ip address
        TriggerScript(IP, GW, Mask, DNS1, DNS2)
    End Sub

    ''' <summary>
    ''' Update ข้อมูลลง tblSetEmail ในกรณีที่เคยมีข้อมูลอยู่แล้ว
    ''' </summary>
    ''' <param name="IP">Ip ที่จะทำการ Update</param>
    ''' <param name="GW">GateWay ที่จะทำการ Update</param>
    ''' <param name="DNS1">DNS1 ที่จะทำการ Update</param>
    ''' <param name="DNS2">DNS2 ที่จะทำการ Update</param>
    ''' <param name="ProxyIP">Proxy IP ที่จะทำการ Update</param>
    ''' <param name="ProxyPort">Proxy Port ที่จะทำการ Update</param>
    ''' <param name="ProxyUser">Proxy User ที่จะทำการ Update</param>
    ''' <param name="ProxyPassword">ProxyPassword ที่จะทำการ Update</param>
    ''' <param name="SMTPServerIp">SMTPServerIP ที่จะทำการ Update</param>
    ''' <param name="SMTPServerPort">SMTPServerPort ที่จะทำการ Update</param>
    ''' <param name="SMTPServerUser">SMTPServerUser ที่จะทำการ Update</param>
    ''' <param name="SMTPServerPassword">SMTPServerPassword ที่จะทำการ Update</param>
    ''' <param name="AskQuestionRecipient">Email ของผู้รับเรื่องปัญหา</param>
    ''' <param name="RptConsultantRecipient">Email ที่จะทำการส่ง Report ของ รร. ไป</param>
    ''' <param name="StartDate">วันที่ติดตั้ง</param>
    ''' <param name="EndDate">วันที่หมดอายุ</param>
    ''' <param name="CalendarName">ชื่อเทอม</param>
    ''' <param name="CalendarYear">ปีการศึกษา</param>
    ''' <param name="Mask">SubNetMask ที่จะทำการ Update</param>
    ''' <param name="ParentServerIP">IP ของเครื่อง server ในกรณีที่มีการซื้อเครื่อง server ไปด้วย</param>
    ''' <remarks></remarks>
    Private Sub UpdateData(ByVal IP As String, ByVal GW As String, ByVal DNS1 As String, ByVal DNS2 As String, ByVal ProxyIP As String, ByVal ProxyPort As String,
                         ByVal ProxyUser As String, ByVal ProxyPassword As String, ByVal SMTPServerIp As String, ByVal SMTPServerPort As String,
                         ByVal SMTPServerUser As String, ByVal SMTPServerPassword As String, ByVal AskQuestionRecipient As String, ByVal RptConsultantRecipient As String,
                         ByVal StartDate As String, ByVal EndDate As String, ByVal CalendarName As String, ByVal CalendarYear As String, ByVal Mask As String, ByVal ParentServerIP As String)
        Try
            'ถ้ามี Server ส่วนกลางต้องเปลี่ยนไปใช้ DB ส่วนกลาง
            If HttpContext.Current.Application("ConnectedWithT360") = True Then
                'todo เดียวต้องแก้ function นี้เมื่อเปลี่ยน ip ต้องไปเปลี่ยน ip ของ file langset.bin วรรคที่ 4 แทน connectionstring (webconfig)
                ChangeConnectionStringToParentServerIP(ParentServerIP)
            End If
            _DB.OpenWithTransection()
            'Update tblSetEmail
            Dim sql As String = " UPDATE dbo.tblSetEmail SET IP = " & IP & ",Mask = " & Mask & ",GW = " & GW & ",DNS1 = " & DNS1 & ",DNS2 = " & DNS2 & "," &
                                " ProxyIP = " & ProxyIP & ",ProxyPort = " & ProxyPort & ", " &
                                " ProxyUser = " & ProxyUser & ",ProxyPassword = " & ProxyPassword & ",SMTPServerIP = " & SMTPServerIp & ",SMTPServerPort = " & SMTPServerPort & "," &
                                " SMTPServerUser = " & SMTPServerUser & ",SMTPServerPassword = " & SMTPServerPassword & ", " &
                                " AskQuestionRecipient = " & AskQuestionRecipient & ",RptConsultantRecipient = " & RptConsultantRecipient & ",LastUpdate = dbo.GetThaiDate() " &
                                " WHERE SchoolId = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' "
            _DB.ExecuteWithTransection(sql)

            If HttpContext.Current.Application("ConnectedWithT360") = False Then
                'Update t360_tblcalendar
                sql = " SELECT TOP 1 Calendar_Id FROM dbo.t360_tblCalendar WHERE School_Code = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' " &
                      " AND Isactive = 1 AND Calendar_Type = 3 ORDER BY Calendar_ToDate DESC "
                Dim CalendarId As String = _DB.ExecuteScalarWithTransection(sql)
                If CalendarId <> "" Then
                    sql = " UPDATE dbo.t360_tblCalendar SET Calendar_Name = '" & CalendarName & "' , Calendar_Year = '" & CalendarYear & "',Calendar_FromDate = " & StartDate & " " &
                          " ,Calendar_ToDate = " & EndDate & ",LastUpdate = dbo.GetThaiDate() WHERE Calendar_Id = '" & CalendarId & "' "
                    _DB.ExecuteWithTransection(sql)
                Else
                    _DB.RollbackTransection()
                    Exit Sub
                End If
            End If

            _DB.CommitTransection()
            If DNS2 = "'000.000.000.000'" Then
                DNS2 = "''"
            End If
            SetProxyAndIPAddress(ProxyIP, ProxyPort, IP, GW, Mask, DNS1, DNS2)
            'TriggerScript(IP, GW, Mask, DNS1, DNS2)
        Catch ex As Exception
            If ex.Message <> "Cannot Open Connection" Then
                _DB.RollbackTransection()
            End If
        End Try

    End Sub

    ''' <summary>
    ''' ทำการเรียกให้ Function Javascript ทำงานหลังจากทำการ Insert/Update ข้อมูลเสร็จเรียบร้อยแล้ว เพื่อให้เปลี่ยน IP และทำการ Redirect ไปหน้า LogInpage.aspx โดยอัตโนมัติ
    ''' </summary>
    ''' <param name="InputIp">IP ที่จะทำการเปลี่ยน</param>
    ''' <param name="InputGW">GateWay ที่จะทำการเปลี่ยน</param>
    ''' <param name="InputSubnetMask">SubNet Mask ที่จะทำการเปลี่ยน</param>
    ''' <param name="InputDNS1">DNS1 ที่จะทำการเปลี่ยน</param>
    ''' <param name="InputDNS2">DNS2 ที่จะทำการเปลี่ยน</param>
    ''' <remarks></remarks>
    Private Sub TriggerScript(ByVal InputIp As String, ByVal InputGW As String, ByVal InputSubnetMask As String, ByVal InputDNS1 As String, ByVal InputDNS2 As String)
        'Dim StrScript As String = "<script type='text/javascript'>$(function () {$('#spnWarning').show();setTimeout(function () {window.location = 'Loginpage.aspx';}, 10000)});</script>"
        Dim StrScript As String = "<script type='text/javascript'>$(function () {$('#spnWarning').show();PostChangeIp(" & InputIp & ", " & InputGW & ", " & InputSubnetMask & ", " & InputDNS1 & ", " & InputDNS2 & ");setTimeout(function () {window.location = 'Loginpage.aspx';}, 5000)});</script>"
        'เป็นคำสั่งให้ Inject Javascript นี้ลงไปหลังจาก pageload เสร็จเรียบร้อยแล้ว
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Test", StrScript)
    End Sub

    'Private Sub ChangeIPToParentServerIP(ByVal NewIpAddress As String)
    '    Try
    '        Dim GetConStr As System.Configuration.Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
    '        If GetConStr IsNot Nothing Then
    '            NewIpAddress = IPAddress.Parse(ChangeFormatStringToLong(NewIpAddress)).ToString()
    '            Dim ConStr As String = KNConfigData.DecryptData(GetConStr.ConnectionStrings.ConnectionStrings("ConnectionString").ConnectionString)
    '            If ConStr.IndexOf(";") <> -1 Then
    '                Dim StartPosition As Integer = ConStr.IndexOf("=") + 1
    '                Dim PositionSemicolon As Integer = ConStr.IndexOf(";")
    '                Dim EndPosition As Integer = PositionSemicolon - StartPosition
    '                Dim StrIsReplace As String = ConStr.Substring(StartPosition, EndPosition)
    '                Dim NewConStr As String = ConStr.Replace(StrIsReplace, NewIpAddress)
    '                If CheckIsCanOpenConnection(NewConStr) = True Then
    '                    NewConStr = KNConfigData.KnEncryption(NewConStr)
    '                    GetConStr.ConnectionStrings.ConnectionStrings("ConnectionString").ConnectionString = NewConStr
    '                    GetConStr.Save()
    '                Else
    '                    Throw New Exception("Cannot Open Connection")
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If ex.Message = "Cannot Open Connection" Then
    '            Throw New Exception("Cannot Open Connection")
    '        End If
    '    End Try
    'End Sub

    ''' <summary>
    ''' ทำการดึงค่า ConnectionString ขึ้นมาเปลี่ยน IP ให้ยิงไปหาเครื่อง Server แล้วทำการทดสอบเปิด Connection ดูว่าสามารถใช้งานได้รึเปล่า
    ''' </summary>
    ''' <param name="NewIpAddress">IP ของเครื่อง Server</param>
    ''' <remarks></remarks>
    Private Sub ChangeConnectionStringToParentServerIP(ByVal NewIpAddress As String)
        Try
            Dim tmpStr As String = ""
            'Get txt ข้อมูลทั้งหมดขึ้นมาจากไฟล์ langset.bin
            Dim txtData As String = ClsLanguage.GetLang(tmpStr, , , , True)
            'ทำการหาเฉพาะ ConnectionString ขึ้นมาแบบไม่เข้ารหัส
            Dim currentConStrNoDecrypt As String = ClsLanguage.GetConStr(True)
            'ทำการหา ConnectionString แบบเข้ารหัสเพื่อเอาไว้ Replace ตอนท้าย
            Dim currentConnStr As String = ClsLanguage.GetConStr()
            'ทำการตัดสตริงเพื่อหาเฉพาะ IP เพื่อเตรียมนำมา Replace ให้เป็น IP ใหม่
            Dim posStart As Integer = (currentConnStr.ToLower().IndexOf("=") + 1)
            Dim posEnd As Integer = currentConnStr.ToLower().IndexOf(";", posStart)
            Dim currentDatasource As String = currentConnStr.Substring(posStart, posEnd - posStart)

            'ทำการ Replace ให้เป็น Connection String ที่เป็น IP ของ Server อันใหม่
            Dim newConnStr As String = currentConnStr.Replace(currentDatasource, NewIpAddress)
            'ทำการทดสอบเปิด Connection โดยใช้ ConnectionString ใหม่นี้
            If CheckIsCanOpenConnection(newConnStr) = True Then
                'ถ้าเปิด Connection ได้ก็ให้ทำการ Replace ส่วนเฉพาะที่เป็น ConnectionString แล้วเขียนไฟล์ใหม่
                txtData = txtData.Replace(currentConStrNoDecrypt, KNConfigData.KnEncryption(newConnStr))
                ClsLanguage.WriteFileData(txtData, True)
            Else
                Throw New Exception("Cannot Open Connection")
            End If
        Catch ex As Exception
            If ex.Message = "Cannot Open Connection" Then
                Throw New Exception("Cannot Open Connection")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Function ที่เอาไว้ check ว่า ConnetionString ที่ส่งเข้ามาใช้งานได้จริงหรือไม่
    ''' </summary>
    ''' <param name="NewConStr">ConnectionString ที่ต้องการทดสอบ</param>
    ''' <returns>Boolean:True=ใช้งานได้,False=ใช้งานไม่ได้</returns>
    ''' <remarks></remarks>
    Private Function CheckIsCanOpenConnection(ByVal NewConStr As String) As Boolean
        Dim NewDB As New ClassConnectSql(False, NewConStr)
        Try
            Dim ConnCheck As New SqlConnection
            NewDB.OpenExclusiveConnect(ConnCheck)
            NewDB.CloseExclusiveConnect(ConnCheck)
            NewDB = Nothing
            Return True
        Catch ex As Exception
            NewDB = Nothing
            Return False
        End Try
    End Function

    ''' <summary>
    ''' หาว่าเคยมีการกรอกรข้อมูลไว้แล้วหรือยัง
    ''' </summary>
    ''' <returns>Boolean:True=มีแล้ว,False=ไม่มีข้อมูล</returns>
    ''' <remarks></remarks>
    Private Function CheckIsHaveData() As Boolean
        'Dim sql As String = " SELECT COUNT(*) FROM dbo.t360_tblStudent WHERE School_Code = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' "
        'Dim CountCheckDataStudent As Integer = CInt(_DB.ExecuteScalar(sql))
        'sql = " SELECT COUNT(*) FROM dbo.t360_tblTeacher WHERE School_Code = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND Teacher_IsActive = 1 "
        'Dim CountCheckTeacher As Integer = CInt(_DB.ExecuteScalar(sql))
        'sql = " SELECT COUNT(*) FROM dbo.tblQuiz WHERE t360_SchoolCode = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND IsActive = 1 "
        'Dim CountCheckQuiz As Integer = CInt(_DB.ExecuteScalar(sql))
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblSetEmail WHERE SchoolId = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND IsActive = 1 "
        Dim CountCheckSetEmail As Integer = CInt(_DB.ExecuteScalar(sql))
        sql = " SELECT COUNT(*) FROM dbo.t360_tblCalendar WHERE School_Code = '" & HttpContext.Current.Application("DefaultSchoolCode") & "' AND IsActive = 1 "
        Dim CountCheckCalendar As Integer = CInt(_DB.ExecuteScalar(sql))
        If (CountCheckCalendar + CountCheckSetEmail) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub txtProxy_Change(ByVal sender As Object, ByVal e As EventArgs)
        If sender Is txtProxyIP Then
            If txtProxyIP.TextWithLiterals = "000.000.000.000" Then
                txtProxyPort.Enabled = False
                txtProxyUser.Enabled = False
                txtProxyPassword.Enabled = False

                txtProxyPort.Text = "0"
                txtProxyUser.Text = ""
                txtProxyPassword.Text = ""
            Else
                txtProxyPort.Enabled = True
            End If
        ElseIf sender Is txtProxyPort Then
            If txtProxyPort.TextWithLiterals = "00000" Then
                txtProxyUser.Enabled = False
                txtProxyPassword.Enabled = False

                txtProxyUser.Text = ""
                txtProxyPassword.Text = ""
            Else
                txtProxyUser.Enabled = True
            End If
        ElseIf sender Is txtProxyUser Then
            If txtProxyUser.Text = "" Then
                txtProxyPassword.Enabled = False
                txtProxyPassword.Text = ""
            Else
                txtProxyPassword.Enabled = True
            End If
        End If
    End Sub

End Class