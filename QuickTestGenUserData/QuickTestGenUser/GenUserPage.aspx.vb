Imports Telerik.Web.UI
Imports KnowledgeUtils.System

Public Class GenUserPage
    Inherits System.Web.UI.Page
    Implements IPostBackEventHandler

    Dim ClsBudget As New ServerBudget(New ClassConnectSql)

    Public Property ViewStateData As String
        Get
            Return ViewState("data")
        End Get
        Set(ByVal value As String)
            ViewState("data") = value
        End Set
    End Property
    Public Property ViewStatePassword As String
        Get
            Return ViewState("Pass")
        End Get
        Set(ByVal value As String)
            ViewState("Pass") = value
        End Set
    End Property
    Public Property ViewStateConfirmPassword As String
        Get
            Return ViewState("Pass")
        End Get
        Set(ByVal value As String)
            ViewState("Pass") = value
        End Set
    End Property
    Public Property ViewStateListDB() As List(Of ListDBSC)
        Get
            If ViewState("ListDB") Is Nothing Then
                Return New List(Of ListDBSC)
            Else
                Return ViewState("ListDB")
            End If
        End Get
        Set(ByVal value As List(Of ListDBSC))
            ViewState("ListDB") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IsLoggedIn") = Nothing Then
            Response.Redirect("~/QuickTestGenUser/LoginGenUser.aspx", False)
        Else

            ViewStatePassword = txtPSW.Text
            ViewStateConfirmPassword = txtCFPSW.Text

            If Not IsPostBack Then

                BindSubject()

                If Not Request.QueryString("id") Is Nothing Then
                    Dim dtUser As DataTable = ClsBudget.SearchUserById(Request.QueryString("id"))
                    txtFirstName.Text = dtUser.Rows(0)("FirstName")
                    txtLastName.Text = dtUser.Rows(0)("LastName")
                    txtUserName.Text = dtUser.Rows(0)("UserName")
                    DpkStartDate.SelectedDate = dtUser.Rows(0)("Calendar_FromDate")
                    If dtUser.Rows(0)("UserExpireDate").ToString <> "" Then
                        DpkEndDate.SelectedDate = dtUser.Rows(0)("UserExpireDate")
                    Else
                        DpkEndDate.SelectedDate = dtUser.Rows(0)("Calendar_ToDate")
                    End If

                    lblSchool.Text = dtUser.Rows(0)("SchoolId").ToString
                    lblNotChangePassword.Visible = True
                    SetCurrentCondition()
                Else
                    lblSchool.Text = Request.QueryString("SchoolId").ToString
                    lblNotChangePassword.Visible = False
                End If

            End If
        End If
    End Sub

    Private Sub SetCurrentCondition()

        If ClsBudget.IsUserAllCondition(Request.QueryString("id")) Then
            CheckAllCheckbox()
        Else
            For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                Dim SubjectRow = dataItem("a").Text

                If Not SubjectRow = "ทั้งหมด" Then
                    Dim dt As DataTable = ClsBudget.GetUserSubjectClass(Request.QueryString("id"), SubjectRow)
                    For Each a In dt.Rows
                        Dim cbChild As CheckBox = CType(dataItem.FindControl(Trim("C" & a("ClassId").ToString())), CheckBox)
                        cbChild.Checked = True
                    Next

                    If dt.Rows.Count() = 12 Then
                        Dim cbAllChild As CheckBox = CType(dataItem.FindControl("cbAll"), CheckBox)
                        cbAllChild.Checked = True
                    End If

                End If
            Next
        End If
    End Sub

    Protected Sub lbtnAddUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAddUser.Click
        Log.Record(Log.LogType.GenUser, "เพิ่มผู้ใช้", True)
        Response.Redirect("~/QuickTestGenUser/GenUserPage.aspx", False)

    End Sub

    Protected Sub lbtnSearchUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSearchUser.Click
        Log.Record(Log.LogType.GenUser, "ค้นหาข้อมูลผู้ใช้", True)
        Response.Redirect("~/QuickTestGenUser/GenUserManagerPage.aspx", False)
    End Sub

    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        Dim msg As String

        If txtFirstName.Text = "" Or txtLastName.Text = "" Or txtUserName.Text = "" Then
            msg = "กรอกข้อมูลให้ครบด้วยนะคะ"
        ElseIf (Request.QueryString("id") Is Nothing) And (txtPSW.Text = "" Or txtCFPSW.Text = "") Then
            msg = "กรอกรหัสผ่านด้วยนะคะ"
        ElseIf txtPSW.Text <> txtCFPSW.Text Then
            msg = "ยืนยันรหัสผ่านไม่ตรง ลองใหม่นะคะ"
        Else

            ViewStatePassword = txtPSW.Text

            MakeData()

            If Not Request.QueryString("id") Is Nothing Then
                msg = UpdateUser(Request.QueryString("id").ToString)
            Else
                msg = insertUser()
            End If
        End If


        WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)


    End Sub

    Public Function insertUser() As String

        Try
            Dim UserID As String = Guid.NewGuid().ToString

            If ConfigurationManager.AppSettings("IsQuicktestProduction") Then
                Dim StartDateToInsert As String
                StartDateToInsert = PadZero(DpkStartDate.SelectedDate.Value.Day) & "/" & PadZero(DpkStartDate.SelectedDate.Value.Month) & "/" & ToDBYear(DpkStartDate.SelectedDate.Value.Year)

                Dim EndDateToInsert As String
                Dim EndDateAdjusted As Date = DpkEndDate.SelectedDate.Value.Date.AddDays(1).AddMinutes(-1)
                EndDateToInsert = ToDBYear(DpkEndDate.SelectedDate.Value.Year) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Month) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Day) & " 23:59:00"


                Dim EndYear As String = ToSemesterYear(DpkEndDate.SelectedDate.Value.Year)

                If ClsBudget.InsertUser(UserID.ToString, txtFirstName.Text, txtLastName.Text, txtUserName.Text, ViewStatePassword, CInt(Request.QueryString("SchoolId")), EndDateToInsert) = True Then
                    ClsBudget.SetCalendar(Request.QueryString("SchoolId").ToString, EndYear, "เทอมนี้", StartDateToInsert, EndDateToInsert)
                End If
            Else
                ClsBudget.InsertUser(UserID.ToString, txtFirstName.Text, txtLastName.Text, txtUserName.Text, ViewStatePassword, CInt(Request.QueryString("SchoolId")))
            End If

            SetUserSubjectClass(UserID)

            Log.Record(Log.LogType.GenUser, "บันทึกกข้อมุลสำเร็จ", True)

            Return "บันทึกข้อมูลเรียบร้อยค่ะ"
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Log.Record(Log.LogType.GenUser, "บันทึกข้อมูลไม่สำเร็จ", True)
            Return "บันทึกข้อมูลไม่สำเร็จ ลองอีกครั้งนะคะ-(fromInsert)"
        End Try

    End Function

    Public Function UpdateUser(ByVal UserId As String) As String
        Try


            If ConfigurationManager.AppSettings("IsQuicktestProduction") Then
                Dim StartDateToInsert As String
                StartDateToInsert = PadZero(DpkStartDate.SelectedDate.Value.Day) & "/" & PadZero(DpkStartDate.SelectedDate.Value.Month) & "/" & ToDBYear(DpkStartDate.SelectedDate.Value.Year)

                Dim EndDateToInsert As String
                Dim EndDateAdjusted As Date = DpkEndDate.SelectedDate.Value.Date.AddDays(1).AddMinutes(-1)
                EndDateToInsert = ToDBYear(DpkEndDate.SelectedDate.Value.Year) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Month) & "-" & PadZero(DpkEndDate.SelectedDate.Value.Day) & " 23:59:00"


                Dim EndYear As String = ToSemesterYear(DpkEndDate.SelectedDate.Value.Year)

                ClsBudget.UpdateUser(UserId, txtFirstName.Text, txtLastName.Text, txtUserName.Text, txtPSW.Text, EndDateToInsert)
                ClsBudget.SetCalendar(lblSchool.Text, EndYear, "เทอมนี้", StartDateToInsert, EndDateToInsert)
            Else
                ClsBudget.UpdateUser(UserId, txtFirstName.Text, txtLastName.Text, txtUserName.Text, txtPSW.Text)
            End If

            SetUserSubjectClass(UserId)

            Log.Record(Log.LogType.GenUser, "แก้ไขข้อมูลไม่สำเร็จ", True)
            Return "แก้ไขข้อมูลเรียบร้อยค่ะ"
        Catch ex As Exception
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Log.Record(Log.LogType.GenUser, "แก้ไขข้อมูลไม่สำเร็จ", True)
            Return "แก้ไขข้อมูลไม่สำเร็จ ลองอีกครั้งนะคะ-(fromUpdate)"
        End Try


    End Function

    Public Sub MakeData()

        Dim msg As String
        Dim DetailRow As Integer = 1
        Dim ListDBSC As New List(Of ListDBSC)
        'วนเก็บชั้น-วิชาที่เลือกไว้

        For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items

            Dim SubjectRow = dataItem("a").Text

            If Not SubjectRow = "ทั้งหมด" Then

                Dim ClassRow As Integer  ' Id ระดับชั้นที่จะ insert

                ' วน 15 คอลัม
                For i = 4 To 15
                    Dim chkCondition As CheckBox = CType(dataItem.FindControl("C" + i.ToString().PadLeft(2, "0")), CheckBox)
                    'ถ้าคอลัมนี้ เช็คอยู่
                    If chkCondition.Checked Then

                        ClassRow = CInt(i.ToString().PadLeft(2, "0"))

                        Dim SubjectId As Integer = CInt(ClsBudget.SelectSubjectId(SubjectRow))

                        ListDBSC.Add(New ListDBSC With {.SubjectId = SubjectId, .ClassId = ClassRow, .DetailId = DetailRow})

                        DetailRow += 1
                    End If

                Next  ' วน 15 คอลัม
            End If
        Next  ' วนกริดบนทีละแถว
        ' เช็คข้อมูล
        ' 20171025 ปรับให้สามารถปิดสิทธิ์ทุกชั้นทุกวิชาได้
        'If ListDBSC.Count = 0 Then
        '    msg = "ต้องเลือกเงื่อนไขด้วยนะคะ"
        '    WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
        'Else
        ViewStateListDB = ListDBSC
        'End If

    End Sub

    Public Sub ClearAllCheckBox()
        For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
            For i = 4 To 15
                Dim c As CheckBox = CType(dataItem.FindControl("CbAll"), CheckBox)
                c.Checked = False
                Dim ci As CheckBox = CType(dataItem.FindControl("C" + i.ToString().PadLeft(2, "0")), CheckBox)
                ci.Checked = False
            Next
        Next

    End Sub

    Private Sub CheckAllCheckbox()
        For i = 0 To GvCondition.MasterTableView.Items.Count - 1
            Dim cbAll As CheckBox = CType(GvCondition.MasterTableView.Items(i).FindControl("cbAll"), CheckBox)
            cbAll.Checked = True

            For s = 4 To 15
                Dim cbChild As CheckBox = CType(GvCondition.MasterTableView.Items(i).FindControl("C" & s.ToString().PadLeft(2, "0")), CheckBox)
                cbChild.Checked = True
            Next
        Next
    End Sub

    Private Sub SetUserSubjectClass(ByVal UserId As String)
        Try
            ClsBudget.UnActive(UserId)

            If Not ViewStateListDB.Count = 0 Then
                For Each D In ViewStateListDB
                    Dim USCId As Integer = ClsBudget.MaxAuto("tblUserSubjectClass", "USCId")
                    ClsBudget.InsertUserSubjectClass(USCId, UserId, D.DetailId, D.SubjectId, D.ClassId)
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub

    'เปลี่ยนจากชื่อ column เป็น ชั้นเรียน
    Private Function changeClass(ByVal Name As String) As String
        changeClass = ""
        Select Case Name
            Case "C01"
                changeClass = "อ.1"
            Case "C02"
                changeClass = "อ.2"
            Case "C03"
                changeClass = "อ.3"
            Case "C04"
                changeClass = "ป.1"
            Case "C05"
                changeClass = "ป.2"
            Case "C06"
                changeClass = "ป.3"
            Case "C07"
                changeClass = "ป.4"
            Case "C08"
                changeClass = "ป.5"
            Case "C09"
                changeClass = "ป.6"
            Case "C10"
                changeClass = "ม.1"
            Case "C11"
                changeClass = "ม.2"
            Case "C12"
                changeClass = "ม.3"
            Case "C13"
                changeClass = "ม.4"
            Case "C14"
                changeClass = "ม.5"
            Case "C15"
                changeClass = "ม.6"

            Case "อ.1"
                changeClass = "C01"
            Case "อ.2"
                changeClass = "C02"
            Case "อ.3"
                changeClass = "C03"
            Case "ป.1"
                changeClass = "C04"
            Case "ป.2"
                changeClass = "C05"
            Case "ป.3"
                changeClass = "C06"
            Case "ป.4"
                changeClass = "C07"
            Case "ป.5"
                changeClass = "C08"
            Case "ป.6"
                changeClass = "C09"
            Case "ม.1"
                changeClass = "C10"
            Case "ม.2"
                changeClass = "C11"
            Case "ม.3"
                changeClass = "C12"
            Case "ม.4"
                changeClass = "C13"
            Case "ม.5"
                changeClass = "C14"
            Case "ม.6"
                changeClass = "C15"



        End Select
        Return changeClass
    End Function

    Private Function ToDBYear(InputYear As Integer) As String
        If InputYear > 2500 Then
            Return (InputYear - 543).ToString
        Else
            Return InputYear.ToString
        End If
    End Function

    Private Function ToSemesterYear(InputSemeterYear As Integer) As String
        If InputSemeterYear < 2500 Then
            Return (InputSemeterYear + 543).ToString
        Else
            Return InputSemeterYear.ToString
        End If
    End Function

    ''' <summary>
    ''' เช็คว่าเป็น Checkbox ทั้งหมดหรือไม่ ถ้าใช่ให้ปรับลูกด้วย ถ้าไม่ใช่ไม่ต้องทำอะไร
    ''' </summary>
    ''' <param name="eventArgument"></param>
    ''' <remarks></remarks>
    Public Sub RaisePostBackEvent1(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent

        Dim Msg = Strings.Split(eventArgument, ",")
        Dim IsChecked = CBool(Msg(1))
        Dim CheckboxName = Msg(2)
        Dim ColName = Right(CheckboxName, 3)
        
        'Column
        'If InStr(CheckboxName, "cbAll") <> 0 Then
        For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
            Dim SubjectRow = dataItem("a").Text

            If SubjectRow = "ทั้งหมด" Then
                Dim chkAllCondition As CheckBox = CType(dataItem.FindControl("CbAll"), CheckBox)
                If chkAllCondition.Checked Then
                    CheckAllCheckbox()
                Else
                    Dim isClearAll As Boolean = True
                    For Each EachRow As GridDataItem In GvCondition.MasterTableView.Items
                        Dim chkEachSubCondition As CheckBox = CType(EachRow.FindControl("CbAll"), CheckBox)
                        If chkEachSubCondition.Checked = False And EachRow("a").Text <> "ทั้งหมด" Then
                            isClearAll = False
                        End If
                    Next
                    If isClearAll Then
                        ClearAllCheckBox()
                    End If
                End If
            Else
                Dim chkEachSubCondition As CheckBox = CType(dataItem.FindControl("CbAll"), CheckBox)
                If chkEachSubCondition.Checked Then
                    For i = 4 To 15
                        Dim chkEachSubLevelCondition As CheckBox = CType(dataItem.FindControl("C" + i.ToString().PadLeft(2, "0")), CheckBox)
                        chkEachSubLevelCondition.Checked = chkEachSubCondition.Checked
                    Next
                End If

            End If
        Next

        For i = 4 To 15
            Dim chkEachlevelCondition As CheckBox = CType(GvCondition.MasterTableView.Items(0).FindControl("C" + i.ToString().PadLeft(2, "0")), CheckBox)
            If chkEachlevelCondition.Checked Then
                For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                    Dim chkEachSublevelCondition As CheckBox = CType(dataItem.FindControl("C" + i.ToString().PadLeft(2, "0")), CheckBox)
                    chkEachSublevelCondition.Checked = chkEachlevelCondition.Checked
                Next
            End If
        Next

        txtPSW.Attributes("Value") = ViewStatePassword
        txtCFPSW.Attributes("Value") = ViewStateConfirmPassword
    End Sub

    Private Sub BindSubject()

        Dim Subject As DataTable = ClsBudget.SelectSubjectName("*")

        Dim r = Subject.NewRow
        r("a") = "ทั้งหมด"
        Subject.Rows.InsertAt(r, 0)

        GvCondition.DataSource = Subject
        GvCondition.DataBind()


    End Sub

   

    Private Function PadZero(InputNum As Integer) As String
        If InputNum < 10 Then
            Return "0" + InputNum.ToString
        Else
            Return InputNum.ToString
        End If
    End Function

End Class

<Serializable()>
Public Class ListDBSC

    Private _DetailId As Integer
    Public Property DetailId() As Integer
        Get
            Return _DetailId
        End Get
        Set(ByVal value As Integer)
            _DetailId = value
        End Set
    End Property

    Private _SubjectId As Integer
    Public Property SubjectId() As Integer
        Get
            Return _SubjectId
        End Get
        Set(ByVal value As Integer)
            _SubjectId = value
        End Set
    End Property

    Private _ClassId As Integer
    Public Property ClassId() As Integer
        Get
            Return _ClassId
        End Get
        Set(ByVal value As Integer)
            _ClassId = value
        End Set
    End Property

End Class

Public Class OldCode

    'Private Sub GvBudgetClass_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GvBudgetClass.ItemCommand

    '    ViewStateCFPassword = txtCFPSW.Text
    '    ViewStatePassword = txtPSW.Text

    '    Dim NumEdit = CType(e.Item, GridDataItem)("a").Text
    '    ViewStateNum = NumEdit

    '    Dim DataOldListDB = ViewStateListDB


    '    If e.CommandName = "Select" Then

    '        ViewStateData = "Update"
    '        'btnAddDetail.Text = "แก้เงื่อนไข"

    '        Dim r = (From q In DataOldListDB Where q.No = ViewStateNum)

    '        For Each a In r
    '            For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
    '                Dim DataSub As DataTable = ClsBudget.SelectSubjectName(CInt(a.SubjectId))
    '                Dim Subj = DataSub.Rows(0)("SubjectName")

    '                If Not InStr(Subj, dataItem("a").Text) = 0 Then

    '                    Dim ClsNow = "C" & a.ClassId.ToString
    '                    Dim c As CheckBox = CType(dataItem.FindControl(ClsNow), CheckBox)
    '                    c.Checked = True

    '                End If
    '            Next
    '        Next

    '        DataOldListDB.RemoveAll(Function(ListDBSC) ListDBSC.No = ViewStateNum)


    '        'เมื่อกดปุ่มลบ ให้ขึ้นข้อความเตือน
    '        'ElseIf e.CommandName = "Delete" Then

    '        '    DataOldListDB.RemoveAll(Function(ListDBSC) ListDBSC.No = ViewStateNum)

    '        '    Dim No As Integer = 1

    '        '    For Each DataOld In GvBudgetClass.Items

    '        '        If No <> ViewStateNum Then

    '        '            ListGV.Add(New With {.a = No, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})

    '        '        End If
    '        '        No = No + 1
    '        '    Next
    '        '    GvBudgetClass.DataSource = ListGV
    '        '    GvBudgetClass.DataBind()

    '    End If

    '    txtCFPSW.Attributes.Add("value", ViewStateCFPassword)
    '    txtPSW.Attributes.Add("value", ViewStatePassword)

    'End Sub

    'Protected Sub btnAddDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDetail.Click

    '    ViewStateCFPassword = txtCFPSW.Text
    '    ViewStatePassword = txtPSW.Text

    '    Dim Str As String = ""
    '    Dim chk As Integer = 1
    '    Dim SubjectAll As String = ""
    '    Dim ClsAll As String = ""

    '    ViewStateRow = 0


    '    'ถ้าเคยเลือกเงื่อนไขแล้ว
    '    'ให้เก็บข้อมูลเดิมในกริดมาใส่ใน list ก่อน

    '    If GvBudgetClass.Items.Count <> 0 Then
    '        If ViewStateData = "Update" Then



    '            For Each DataOld In GvBudgetClass.Items

    '                If Not DataOld("a").text = ViewStateNum Then ' ต้องไม่ใช่ Row ที่กดแก้ไข
    '                    ListGV.Add(New With {.a = DataOld("a").text, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
    '                    ViewStateRow = ViewStateRow + 1
    '                Else

    '                    MakeData(ViewStateNum)

    '                End If

    '            Next
    '            ' ถ้าไม่ใช่การแก้ไขให้เก็บมาให้หมด
    '        Else
    '            For Each DataOld In GvBudgetClass.Items
    '                ListGV.Add(New With {.a = DataOld("a").text, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
    '                ViewStateRow = ViewStateRow + 1
    '            Next

    '            ViewStateRow = ViewStateRow + 1
    '            MakeData(ViewStateRow)
    '        End If



    '    Else
    '        ViewStateRow = ViewStateRow + 1
    '        MakeData(ViewStateRow)
    '    End If

    '    GvBudgetClass.DataSource = ListGV
    '    GvBudgetClass.DataBind()

    '    Log.Record(Log.LogType.GenUser, "เพิ่มเงื่อนไข", True)

    '    txtPSW.Text = ViewStatePassword
    '    txtCFPSW.Text = ViewStateCFPassword

    'End Sub.

    'Private Sub CkbClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkbClass.SelectedIndexChanged
    'BindSubject()

    'GvCondition.Columns(2).Visible = False
    'GvCondition.Columns(3).Visible = False
    'GvCondition.Columns(4).Visible = False
    'GvCondition.Columns(5).Visible = False
    'GvCondition.Columns(6).Visible = False
    'GvCondition.Columns(7).Visible = False
    'GvCondition.Columns(8).Visible = False
    'GvCondition.Columns(9).Visible = False
    'GvCondition.Columns(10).Visible = False
    'GvCondition.Columns(11).Visible = False
    'GvCondition.Columns(12).Visible = False
    'GvCondition.Columns(13).Visible = False
    'GvCondition.Columns(14).Visible = False
    'GvCondition.Columns(15).Visible = False
    'GvCondition.Columns(16).Visible = False

    'For Each i In CkbClass.Items
    '    If CkbClass.Items(0).Selected = True Then
    '        GvCondition.Columns(2).Visible = True
    '        GvCondition.Columns(3).Visible = True
    '        GvCondition.Columns(4).Visible = True
    '    End If
    '    If CkbClass.Items(1).Selected = True Then

    '        GvCondition.Columns(5).Visible = True
    '        GvCondition.Columns(6).Visible = True
    '        GvCondition.Columns(7).Visible = True
    '        GvCondition.Columns(8).Visible = True
    '        GvCondition.Columns(9).Visible = True
    '        GvCondition.Columns(10).Visible = True
    '    End If
    '    If CkbClass.Items(2).Selected = True Then

    '        GvCondition.Columns(11).Visible = True
    '        GvCondition.Columns(12).Visible = True
    '        GvCondition.Columns(13).Visible = True
    '    End If
    '    If CkbClass.Items(3).Selected = True Then

    '        GvCondition.Columns(14).Visible = True
    '        GvCondition.Columns(15).Visible = True
    '        GvCondition.Columns(16).Visible = True
    '    End If
    'Next


    'End Sub
End Class