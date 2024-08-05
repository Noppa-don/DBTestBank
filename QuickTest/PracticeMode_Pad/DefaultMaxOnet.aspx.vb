Imports System.Web.Script.Serialization
Imports KnowledgeUtils
Public Class DefaultMaxOnet
    Inherits System.Web.UI.Page

    Private db As New ClassConnectSql()
    Protected SubjectsIdStr As String = ""
    Protected EnableSelect As Boolean = False

    Private SubjectsId As New List(Of String)({CommonSubjectsText.EnglishSubjectId, CommonSubjectsText.ThaiSubjectId, CommonSubjectsText.SocialSubjectId, CommonSubjectsText.MathSubjectId,
                                                    CommonSubjectsText.ScienceSubjectId, CommonSubjectsText.HomeSubjectId, CommonSubjectsText.HealthSubjectId, CommonSubjectsText.ArtSubjectId})
    Protected ThaiId As String = CommonSubjectsText.ThaiSubjectId
    Protected MathId As String = CommonSubjectsText.MathSubjectId
    Protected EngId As String = CommonSubjectsText.EnglishSubjectId
    Protected SocialId As String = CommonSubjectsText.SocialSubjectId
    Protected ScienceId As String = CommonSubjectsText.ScienceSubjectId
    Protected HomeId As String = CommonSubjectsText.HomeSubjectId
    Protected HealthId As String = CommonSubjectsText.HealthSubjectId
    Protected ArtId As String = CommonSubjectsText.ArtSubjectId

    Protected StudentId As String = ""
    Protected StudentClass As String = ""

    Protected DeviceId As String
    Protected TokenId As String

    Protected CreditAmount As Integer = 0

    Private SubjectRegistered As New List(Of MaxonetSubjectClassRegister)
    Protected SubjectRegisteredJson As String

    Private js As New JavaScriptSerializer()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'หน้าเลือกวิชา
        'ตรวจสอบว่าหมดอายุหรือยัง (ตรวจสอบทั้งลงทะเบียนและเติมเครดิต) -- ไปหน้า KeycodeExpired
        'ตรวจสอบว่ามีเครื่องอื่น Active อยู่หรือไม่ -- ทำการ UnActive Device อื่นทิ้ง (keycodeUsage , Tablet) ยกเว้น Keycode Type 3 เพราะ เครดิตก็ควรจะยังอยู่
        'ถ้าเอา Keycode ที่เคยลง Type 3 ไปลงทะเบียนใหม่ล่ะ !!!!!

        'เก็บ Data

        If Not Page.IsPostBack Then
            Session("SessionStatus") = "1"
        End If

        Page.Header.DataBind()
        HttpContext.Current.Session.Abandon()
        HttpContext.Current.Session.Clear()

        DeviceId = Request.QueryString("DeviceUniqueID")
        TokenId = Request.QueryString("token")
        HttpContext.Current.Session("DeviceId") = DeviceId

        If DeviceId Is Nothing OrElse TokenId Is Nothing OrElse DeviceId = "" OrElse TokenId = "" Then
            deviceDisable.Style.Item("display") = "block"
            Exit Sub
        End If

        'เช็คว่ายัง Active หรือไม่
        TokenId = Guid.Parse(TokenId).ToString("D")

        Dim dt As DataTable = CheckActiveKeycode()

        If dt.Rows.Count = 0 Then
            'case ไม่น่าจะเป็นไปได้ เพราะถ้ายังไม่เคยลงทะเบียนจะไม่มาหน้านี้
            Exit Sub
        Else
            StudentId = dt.Rows(0)("KCU_OwnerId").ToString()
            Session("UserId") = StudentId
            StudentClass = ChangeStudentClass(dt.Rows(0)("Student_CurrentClass").ToString())
            If CBool(dt.Rows(0)("KCU_IsActive")) Then
                'ยัง Active ทำงานต่อเลยจ้า
            Else
                'น่าจะโดนเครื่องอื่นเตะมาก่อนหน้านี้ ก็เตะเครื่องอื่น แล้ว Active ตัวเอง
                KickOtherDeviceAndActiveMe()
            End If
        End If

        Dim keyCode_Credit As Integer = GetKeyCodeCredit()
        Dim keyCode_Type As Integer = GetKeyCodeType()

        ' case keycode ที่ลงทะเบียนใหม่ หมดอายุแล้ว
        If keyCode_Type = EnumMaxOnetKeyCodeType.keyCodeExpire Then
            Response.Redirect("~/MaxOnet/KeyCodeExpiredPage.aspx?TokenId=" & TokenId.CleanSQL & "&DeviceId=" & DeviceId.CleanSQL & "&SubjectsIdStr=" & StudentId)
        End If

        ' assign value
        initialDataSubjectRegistered()

        CreditAmount = (keyCode_Credit - GetStudentSubjectAmount())

        'case ลงทะเบียนใหม่แล้วจะเลือกวิชา
        If Request.QueryString("addSubject") IsNot Nothing Then
            ' จำนวนวิชาที่สามารถลงได้
            If (CreditAmount > 0) Then
                ' change object to json string
                SubjectRegisteredJson = js.Serialize(SubjectRegistered)

                ' ให้เพิมวิชาได้เลย
                'ยังไงก็ต้องเพิ่ม
                deviceEnable.Style.Item("display") = "block"
                'If keyCode_Type = EnumMaxOnetKeyCodeType.OneSubject Then
                '    EnableSelect = True
                SetSubjectsIdStr(8)
                'ElseIf keyCode_Type = EnumMaxOnetKeyCodeType.ThreeSubjects Then
                '    SetSubjectsIdStr(3)
                'ElseIf keyCode_Type = EnumMaxOnetKeyCodeType.FiveSubjects Then
                '    SetSubjectsIdStr(5)
                'Else
                'SetSubjectsIdStr(1)
                'End If
                deviceEnable.Style.Item("display") = "block"
            Else
                ' แสดงหน้าเพื่อให้เพิ่ม โควต้าลงทะเบียน
                divAddCreditMaxoent.Style.Item("display") = "block"
            End If
        Else
            If Not IsRegisterSubjectMaxonet() Then
                'check ว่าลงทะเบียนเลือกวิชาหรือยัง
                If keyCode_Type = EnumMaxOnetKeyCodeType.OneSubject Then
                    EnableSelect = True
                    SetSubjectsIdStr(1)
                ElseIf keyCode_Type = EnumMaxOnetKeyCodeType.ThreeSubjects Then
                    SetSubjectsIdStr(3)
                ElseIf keyCode_Type = EnumMaxOnetKeyCodeType.FiveSubjects Then
                    SetSubjectsIdStr(5)
                Else
                    SetSubjectsIdStr(8)
                End If
                deviceEnable.Style.Item("display") = "block"
            Else
                HttpContext.Current.Session("DisableGetDailyActivities") = Nothing
                Response.Redirect(String.Format("ChooseTestsetMaxOnet.aspx?deviceUniqueId={0}&token={1}", DeviceId, TokenId))
            End If
        End If


    End Sub

    Private Function CheckActiveKeycode() As DataTable
        Dim sql As String = "SELECT k.KCU_OwnerId,s.Student_CurrentClass,KCU_IsActive FROM maxonet_tblKeyCodeUsage k INNER JOIN t360_tblStudent s ON k.KCU_OwnerId = s.Student_Id 
                                WHERE k.KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND k.KCU_Token = '" & TokenId.CleanSQL & "';"
        Dim dt As DataTable = db.getdata(sql)
        Return dt
    End Function

    Private Sub KickOtherDeviceAndActiveMe()
        Dim sql As String = "Update maxonet_tblKeyCodeUsage set KCU_IsActive = '0' where KCU_Type = 0 and KCU_OwnerId = '" & StudentId.CleanSQL & "';"
        db.Execute(sql)

        sql = "Update maxonet_tblKeyCodeUsage set KCU_IsActive = '1' where KCU_Type = 0 and KCU_DeviceId = '" & DeviceId.CleanSQL & "';"
        db.Execute(sql)
    End Sub

    Private Function GetKeyCodeCredit() As Integer
        Dim sql As String = "Select sum(KCU_CreditAmount) As TotalCredit from maxonet_tblKeyCodeUsage where KCU_Token = '" & TokenId.CleanSQL & "'
                                And (KCU_ExpireDate > dbo.GetThaiDate() Or KCU_ExpireDate Is null)"
        Dim result As String = db.ExecuteScalar(sql)
        Dim KeyCodeCredit As Integer = If((result = ""), -1, CInt(result))
        Return KeyCodeCredit
    End Function
    Private Function GetKeyCodeType() As Integer
        Dim sql As String
        sql = "Select top 1 keycode_Code from ( Select KeyCode_Code,KCU_ExpireDate FROM maxonet_tblKeyCodeUsage   
                WHERE KCU_DeviceId = '" & DeviceId.CleanSQL & "' AND KCU_Token = '" & TokenId.CleanSQL & "' and KCU_IsActive = 1) kcu order by KCU_ExpireDate desc"
        Dim keyCode As String = db.ExecuteScalar(sql)

        sql = "SELECT KeyCode_Type FROM maxonet_tblKeyCode  WHERE KeyCode_Code = '" & keyCode.CleanSQL & "' AND (GETDATE() < KeyCode_ExpireDate or KeyCode_ExpireDate is null);"
        Dim dbLicenseKey As New ClassConnectSql(False, ConfigurationManager.ConnectionStrings("LicensKeyConnectionString").ConnectionString)
        Dim result As String = dbLicenseKey.ExecuteScalar(sql)
        Dim keyCodeType As Integer = If((result = ""), -1, CInt(result))
        Return keyCodeType
    End Function
    Private Sub SetSubjectsIdStr(subjectAmount As Integer)
        subjectAmount = subjectAmount - 2
        For index = 0 To subjectAmount
            SubjectsIdStr &= SubjectsId(index) & ","
        Next
        SubjectsIdStr &= SubjectsId(subjectAmount + 1)
    End Sub

    Private Function IsRegisterSubjectMaxonet() As Boolean
        Dim sql As String = "SELECT * FROM maxonet_tblStudentSubject WHERE SS_StudentId = '" & StudentId.CleanSQL & "';"
        Dim dt As DataTable = db.getdata(sql)
        If dt.Rows.Count > 0 Then Return True
        Return False
    End Function

    Private Sub initialDataSubjectRegistered()

        Dim sql As String = "SELECT ss.SS_SubjectId,ss.SS_LevelId FROM maxonet_tblStudentSubject ss inner join maxonet_tblKeyCodeUsage kcu 
                on kcu.KeyCode_Code = ss.SS_KeyCode and ss.SS_StudentId = kcu.KCU_OwnerId
                WHERE ss.SS_StudentId = '" & StudentId.CleanSQL & "' and (KCU_ExpireDate >= dbo.GetThaiDate() or KCU_ExpireDate is null) 
                and ss.SS_IsActive = 1 and kcu.KCU_IsActive = 1 ORDER BY SS_SubjectId;"

        Dim dt As DataTable = db.getdata(sql)
        For Each r In dt.Rows
            Dim subjectId As String = r("SS_SubjectId").ToString().ToUpper
            Dim classId As String = r("SS_LevelId").ToString().ToUpper
            Dim subject As MaxonetSubjectClassRegister = (From t In SubjectRegistered Where t.SubjectId.ToUpper = subjectId.ToUpper).SingleOrDefault()
            If subject Is Nothing Then
                Dim tsc As New MaxonetSubjectClassRegister With {.SubjectId = subjectId.ToUpper, .ClassId = New List(Of String)({classId.ToUpper}), .Registered = New List(Of Integer)({1})}
                SubjectRegistered.Add(tsc)
            Else
                subject.ClassId.Add(classId)
                subject.Registered.Add(1)
            End If
        Next
        'Return db.getdata(sql).Rows.Count
    End Sub

    Private Function GetStudentSubjectAmount() As Integer
        Dim i As Integer = 0
        For Each subject In SubjectRegistered
            i += subject.ClassId.Count
        Next
        Return i
    End Function

    Private Function ChangeStudentClass(className As String) As String
        Dim c As Array = className.Split(".")
        Dim addNumber As Integer = If((c(0) = "ป"), 3, 9)
        Return String.Format("K{0}", CInt(c(1)) + addNumber)
    End Function

    Private Sub setLevelPanel(subjectId As String)
        'Set Level from db
        'Dim sql As String = "select l.Level_Id,l.Level_Shortname from tblLevel l 
        '                    inner join tblBook  b on l.Level_Id = b.Level_Id where b.GroupSubject_Id = '" & subjectId & "'
        '                    and b.IsActive = 1 and b.Book_Syllabus = '51' order by l.level;"

        'Dim dt As DataTable = db.getdata(sql)
        'For Each r In dt.Rows

        'Next
        'tdk4.InnerHtml = "<input type=""checkbox"" id=""K4"" value=""5F4765DB-0917-470B-8E43-6D1C7B030818"" name=""radio"" />
        '<label for=""K4""><span style=""background: url('../Images/MaxOnet/Levels/K4.png');""></span>Portables</label>"
    End Sub

    <Services.WebMethod()>
    Public Shared Function GetLevelThisSubject(subjectId As String) As String
        Dim db As New ClassConnectSql()
        Dim sql As String = "select l.Level_Id,Level_Shortname,'K' + cast(Level as varchar)as Level from tblLevel l inner join tblBook  b on l.Level_Id = b.Level_Id 
                                where b.GroupSubject_Id = '" & subjectId & "'
                                and b.IsActive = 1 and b.Book_Syllabus = '51' order by l.level"

        Dim dt As DataTable = db.getdata(sql)
        Dim LevelTag As String = ""
        LevelTag = "<tr>"
        For Each eachLevel In dt.Rows
            LevelTag &= "<td><input type=""checkbox"" id=""" & eachLevel("Level").ToString & """ value=""" & eachLevel("Level_Id").ToString.ToUpper & """ name=""radio"" />
             <label for=""" & eachLevel("Level").ToString & """><s></s><span style=""background: url('../Images/MaxOnet/Levels/" & eachLevel("Level").ToString & ".png');""></span>" & eachLevel("Level_Shortname").ToString & "</label></td>"
        Next
        LevelTag &= "/<tr>"

        Return LevelTag
    End Function

    Private Enum EnumMaxOnetKeyCodeType
        OneSubject
        ThreeSubjects
        FiveSubjects
        EightSubjects

        keyCodeExpire = -1
    End Enum

End Class