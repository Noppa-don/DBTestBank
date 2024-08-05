Imports Telerik.Web.UI
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Public Class UserPage
    Inherits System.Web.UI.Page
    Implements IPostBackEventHandler

    Dim ListGV As New List(Of Object)
    Dim GridData As New List(Of Object)
    Dim ClsBudget As New ServerBudget(New ClassConnectSql)
    Dim CreatePDF As New ClsPDF(New ClassConnectSql)

    Public Property StateDialog() As String
        Get
            Return ViewState("StateDialog")

        End Get
        Set(ByVal value As String)
            ViewState("StateDialog") = value
        End Set
    End Property
    Public Property StateSave() As String
        Get
            Return ViewState("StateSave")

        End Get
        Set(ByVal value As String)
            ViewState("StateSave") = value
        End Set
    End Property
    Public Property ViewStateRow As Integer
        Get
            Return ViewState("Row")
        End Get
        Set(ByVal value As Integer)
            ViewState("Row") = value
        End Set
    End Property
    Public Property ViewStateData As String
        Get
            Return ViewState("data")
        End Get
        Set(ByVal value As String)
            ViewState("data") = value
        End Set
    End Property
    Public Property ViewStateNum As String
        Get
            Return ViewState("Num")
        End Get
        Set(ByVal value As String)
            ViewState("Num") = value
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
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not IsPostBack Then

                For Each i In CkbClass.Items
                    i.Selected = True
                Next

                StateDialog = ""
                BindSubject()

                Dim cls As String = ""
                Dim subject As String = ""
                Dim ClsAl As String = ""
                Dim SubjectAl As String = ""
                StateSave = ""


                If Not Request.QueryString("id") Is Nothing Then
                    Dim dtUser As DataTable = ClsBudget.SearchUserById(Request.QueryString("id"))
                    txtFirstName.Text = dtUser.Rows(0)("FirstName")
                    txtLastName.Text = dtUser.Rows(0)("LastName")
                    txtUserName.Text = dtUser.Rows(0)("UserName")
                    'txtPassword.Text = dtUser.Rows(0)("Password")
                    'txtCFPassword.Text = dtUser.Rows(0)("Password")


                    Dim dtGroupDetailUser As DataTable = ClsBudget.SelectDetail(Request.QueryString("id"))
                    Dim num As Integer = 1
                    For Each g In dtGroupDetailUser.Rows

                        Dim dtDetailSubCls As DataTable = ClsBudget.SearchOneDetailUser(Request.QueryString("id"), g("DetailId"))

                        For Each j In dtDetailSubCls.Rows
                            cls = changeClass(Trim("C" & j("ClassId").ToString))

                            If InStr(ClsAl, cls) = 0 Then
                                ClsAl = ClsAl & cls & ","
                            End If
                        Next


                        For Each k In dtDetailSubCls.Rows
                            Dim dt As DataTable = ClsBudget.SelectSubjectName(k("SubjectId"))
                            subject = dt.Rows(0)("SubjectName")
                            If InStr(SubjectAl, subject) = 0 Then
                                SubjectAl = SubjectAl & subject & ","
                            End If

                        Next

                        Dim ListDBSC = ViewStateListDB
                        For Each k In dtDetailSubCls.Rows
                            ListDBSC.Add(New ListDBSC With {.SubjectId = k("SubjectId"), .ClassId = k("ClassId"), .No = num})

                        Next
                        ViewStateListDB = ListDBSC

                        ClsAl = Left(ClsAl, ClsAl.Length - 1)
                        SubjectAl = Left(SubjectAl, SubjectAl.Length - 1)


                        ListGV.Add(New With {.a = num, .Cls = ClsAl, .Subject = SubjectAl})

                        SubjectAl = ""
                        ClsAl = ""
                        num = num + 1
                    Next

                    GvBudgetClass.DataSource = ListGV
                    GvBudgetClass.DataBind()

                Else
                    ViewStateRow = 1
                End If
                ' Log.Record(Log.LogType.Budget, "เพิ่มข้อมูล", True)
            End If
        End If
    End Sub

    Public Sub BindSubject()

        Dim Subject As DataTable = ClsBudget.SelectSubjectName("*")

        Dim r = Subject.NewRow
        r("a") = "ทั้งหมด"
        Subject.Rows.InsertAt(r, 0)

        GvCondition.DataSource = Subject
        GvCondition.DataBind()


    End Sub

    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        Dim msg As String

        If GvBudgetClass.Items.Count = 0 Then
            msg = "ต้องเพิ่มเงื่อนไขอย่างน้อย 1 เงื่อนไขนะคะ"

            WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
        ElseIf txtPassword.Text <> txtCFPassword.Text Then
            msg = "ยืนยันรหัสผ่านไม่ตรง ลองใหม่นะคะ"

            WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
        Else
            If Not Request.QueryString("id") Is Nothing Then

                ClsBudget.UnActive(Request.QueryString("id"))
                ClsBudget.UpdateUser(Request.QueryString("id"), txtFirstName.Text, txtLastName.Text, txtUserName.Text, txtPassword.Text)
                insertBudget("U")
                StateDialog = "update"
                StateSave = "OK"
                'Log.Record(Log.LogType.Budget, "บันทึกแก้ไข", True)
                Log.Record(Log.LogType.ManageUser, "บันทึกแก้ไขข้อมูล", True)
                'Response.Redirect("~/Usermanager/UserManagerPage.aspx?state=" & StateDialog,false)



            Else ' Insert

                insertBudget("I")
                StateDialog = "insert"
                StateSave = "OK"
                'Log.Record(Log.LogType.Budget, "บันทึกเพิ่ม", True)
                Log.Record(Log.LogType.ManageUser, "บันทึกเพิ่มข้อมูล", True)
                ' Response.Redirect("~/Usermanager/UserManagerPage.aspx?state=" & StateDialog,false)

            End If
        End If


    End Sub

    Public Sub insertBudget(ByVal status As String)

        Dim UserID As Integer


        ' Insert Regulation
        If status = "I" Then
            UserID = ClsBudget.MaxAuto("tblUser", "UserId")

            ClsBudget.InsertUser(UserID, txtFirstName.Text, txtLastName.Text, txtUserName.Text, txtPassword.Text, CInt(Request.QueryString("SchoolId")))
        Else
            UserID = Request.QueryString("id")
        End If

        ' Insert RegulationDetail

        Dim DetailSubjectClass = ViewStateListDB

        ' Insert RegulationDetailSubjectClass
        For Each D In DetailSubjectClass

            Dim USCId As Integer = ClsBudget.MaxAuto("tblUserSubjectClass", "USCId")
            ClsBudget.InsertUserSubjectClass(USCId, UserID, D.No, D.SubjectId, D.ClassId)

        Next


    End Sub

    Private Sub GvBudgetClass_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles GvBudgetClass.ItemCommand


        Dim NumEdit = CType(e.Item, GridDataItem)("a").Text
        ViewStateNum = NumEdit

        Dim DataOldListDB = ViewStateListDB


        If e.CommandName = "Select" Then

            ViewStateData = "Update"
            btnAddDetail.Text = "แก้ไขเงื่อนไขงบประมาณ"

            Dim r = (From q In DataOldListDB Where q.No = ViewStateNum)

            For Each a In r
                For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                    Dim DataSub As DataTable = ClsBudget.SelectSubjectName(CInt(a.SubjectId))
                    Dim Subj = DataSub.Rows(0)("SubjectName")

                    If Not InStr(Subj, dataItem("a").Text) = 0 Then

                        Dim ClsNow = "C" & a.ClassId.ToString
                        Dim c As CheckBox = CType(dataItem.FindControl(ClsNow), CheckBox)
                        c.Checked = True

                    End If
                Next
            Next



            DataOldListDB.RemoveAll(Function(ListDBSC) ListDBSC.No = ViewStateNum)


            'เมื่อกดปุ่มลบ ให้ขึ้นข้อความเตือน
        ElseIf e.CommandName = "Delete" Then

            Dim No As Integer = 1
            For Each DataOld In GvBudgetClass.Items

                ListGV.Add(New With {.a = No, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
                No = No + 1

            Next
            GvBudgetClass.DataSource = ListGV
            GvBudgetClass.DataBind()
            DataOldListDB.RemoveAll(Function(ListDBSC) ListDBSC.No = ViewStateNum)
            Dim Msg1 As String = "เมื่อต้องการลบข้อมูล ข้อมูลเงื่อนไขนี้จะถูกลบทั้งหมด แน่ใจนะคะ?"
            Dim Msg2 As String = "ลบข้อมูลเรียบร้อยคะ"
            Dim Msg As String = Msg1 & "," & Msg2
            WindowsTelerik.ShowConfirmDouble(Msg, Me, RadDialogConfirmFirst)
        End If

    End Sub

    Protected Sub btnAddDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddDetail.Click
        Dim Str As String = ""
        Dim chk As Integer = 1
        Dim SubjectAll As String = ""
        Dim ClsAll As String = ""

        ViewStateRow = 0


        'ถ้าเคยเลือกเงื่อนไขแล้ว
        'ให้เก็บข้อมูลเดิมในกริดมาใส่ใน list ก่อน

        If GvBudgetClass.Items.Count <> 0 Then
            If ViewStateData = "Update" Then



                For Each DataOld In GvBudgetClass.Items

                    If Not DataOld("a").text = ViewStateNum Then ' ต้องไม่ใช่ Row ที่กดแก้ไข
                        ListGV.Add(New With {.a = DataOld("a").text, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
                        ViewStateRow = ViewStateRow + 1
                    Else

                        MakeData(ViewStateNum)

                    End If

                Next
                ' ถ้าไม่ใช่การแก้ไขให้เก็บมาให้หมด
            Else
                For Each DataOld In GvBudgetClass.Items
                    ListGV.Add(New With {.a = DataOld("a").text, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
                    ViewStateRow = ViewStateRow + 1
                Next

                ViewStateRow = ViewStateRow + 1
                MakeData(ViewStateRow)
            End If



        Else
            ViewStateRow = ViewStateRow + 1
            MakeData(ViewStateRow)
        End If

        GvBudgetClass.DataSource = ListGV
        GvBudgetClass.DataBind()

        Log.Record(Log.LogType.ManageUser, "เพิ่มเงื่อนไข", True)

    End Sub

    Public Sub MakeData(ByVal ViewStateRow As Integer)

        Dim Str As String = ""

        Dim chk As Integer = 1
        Dim msg As String
        Dim SubjectAll As String = ""
        Dim ClsAll As String = ""

        'วนเก็บค่าในกริดมาต่อกัน

        For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items

            Dim SubjectIn = dataItem("a").Text

            If Not SubjectIn = "ทั้งหมด" Then

                Dim ClsIn As Integer  ' Id ระดับชั้นที่จะ insert

                ' วน 15 คอลัม
                For i = 4 To 15
                    Dim c As CheckBox = CType(dataItem.FindControl("C" + i.ToString), CheckBox)
                    'ถ้าคอลัมนี้ เช็คอยู่
                    If c.Checked Then

                        Dim cls = changeClass("C" + i.ToString)

                        If InStr(ClsAll, cls) = 0 Then
                            ClsAll = ClsAll + cls + ","
                        End If

                        If InStr(SubjectAll, SubjectIn) = 0 Then
                            SubjectAll = SubjectAll + SubjectIn + ","
                        End If

                        ClsIn = CInt(i.ToString)

                        Dim Data As DataTable = ClsBudget.SelectSubjectId(SubjectIn)
                        Dim SubId As Integer = Data.Rows(0)("SubjectId")

                        Dim ListDBSC = ViewStateListDB
                        ListDBSC.Add(New ListDBSC With {.SubjectId = SubId, .ClassId = ClsIn, .No = ViewStateRow})
                        ViewStateListDB = ListDBSC

                    End If

                Next  ' วน 15 คอลัม
            End If
        Next  ' วนกริดบนทีละแถว

        ' เช็คข้อมูล

        If SubjectAll = "" Then
            msg = "ต้องเลือกเงื่อนไขด้วยนะคะ"

            WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
        Else

            ' ข้อมูลครบถ้วน add ข้อมูลในลิสต์และแสดงข้อมูลบนกริด 
            SubjectAll = Left(SubjectAll, SubjectAll.Length - 1)
            ClsAll = Left(ClsAll, ClsAll.Length - 1)

            Dim CheckCount As Integer = 0

            ListGV.Add(New With {.a = ViewStateRow, .Cls = ClsAll, .Subject = SubjectAll})

            ClearCheckBox()

            If ViewStateData = "Update" Then
                ViewStateData = ""
                btnAddDetail.Text = "เพิ่มเงื่อนไข"
            End If
        End If

    End Sub

    Public Sub ClearCheckBox()
        For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
            For i = 4 To 15
                Dim c As CheckBox = CType(dataItem.FindControl("CbAll"), CheckBox)
                c.Checked = False
                Dim ci As CheckBox = CType(dataItem.FindControl("C" + i.ToString), CheckBox)
                ci.Checked = False
            Next
        Next

    End Sub

    'เปลี่ยนจากชื่อ column เป็น ชั้นเรียบ
    Private Function changeClass(ByVal Name As String) As String
        changeClass = ""
        Select Case Name
            Case "C1"
                changeClass = "อ.1"
            Case "C2"
                changeClass = "อ.2"
            Case "C3"
                changeClass = "อ.3"
            Case "C4"
                changeClass = "ป.1"
            Case "C5"
                changeClass = "ป.2"
            Case "C6"
                changeClass = "ป.3"
            Case "C7"
                changeClass = "ป.4"
            Case "C8"
                changeClass = "ป.5"
            Case "C9"
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
                changeClass = "C1"
            Case "อ.2"
                changeClass = "C2"
            Case "อ.3"
                changeClass = "C3"
            Case "ป.1"
                changeClass = "C4"
            Case "ป.2"
                changeClass = "C5"
            Case "ป.3"
                changeClass = "C6"
            Case "ป.4"
                changeClass = "C7"
            Case "ป.5"
                changeClass = "C8"
            Case "ป.6"
                changeClass = "C9"
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

    Private Sub CkbClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkbClass.SelectedIndexChanged
        BindSubject()

        GvCondition.Columns(2).Visible = False
        GvCondition.Columns(3).Visible = False
        GvCondition.Columns(4).Visible = False
        GvCondition.Columns(5).Visible = False
        GvCondition.Columns(6).Visible = False
        GvCondition.Columns(7).Visible = False
        GvCondition.Columns(8).Visible = False
        GvCondition.Columns(9).Visible = False
        GvCondition.Columns(10).Visible = False
        GvCondition.Columns(11).Visible = False
        GvCondition.Columns(12).Visible = False
        GvCondition.Columns(13).Visible = False
        GvCondition.Columns(14).Visible = False
        GvCondition.Columns(15).Visible = False
        GvCondition.Columns(16).Visible = False

        For Each i In CkbClass.Items
            If CkbClass.Items(0).Selected = True Then
                GvCondition.Columns(2).Visible = True
                GvCondition.Columns(3).Visible = True
                GvCondition.Columns(4).Visible = True
            End If
            If CkbClass.Items(1).Selected = True Then

                GvCondition.Columns(5).Visible = True
                GvCondition.Columns(6).Visible = True
                GvCondition.Columns(7).Visible = True
                GvCondition.Columns(8).Visible = True
                GvCondition.Columns(9).Visible = True
                GvCondition.Columns(10).Visible = True
            End If
            If CkbClass.Items(2).Selected = True Then

                GvCondition.Columns(11).Visible = True
                GvCondition.Columns(12).Visible = True
                GvCondition.Columns(13).Visible = True
            End If
            If CkbClass.Items(3).Selected = True Then

                GvCondition.Columns(14).Visible = True
                GvCondition.Columns(15).Visible = True
                GvCondition.Columns(16).Visible = True
            End If
        Next


    End Sub

    Public Sub RaisePostBackEvent1(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent
        Dim Msg = Strings.Split(eventArgument, ",")

        ' เมื่อตกลงลบเงื่อนไข

        If Msg(0) = "delete" Then
            Dim No As Integer = 1
            Dim chk As Integer = 1
            For Each DataOld In GvBudgetClass.Items

                If Not chk = ViewStateNum Then
                    ListGV.Add(New With {.a = No, .Cls = DataOld("Cls").text, .Subject = DataOld("Subject").text})
                    No = No + 1
                Else
                    ClsBudget.UnActive(ViewStateNum)

                End If
                chk = chk + 1
            Next
            GvBudgetClass.DataSource = ListGV
            GvBudgetClass.DataBind()
        End If

        '------------------------------------------------------------------------------------------------------------------------------

        ' ติ๊ก Checkbox
        If Msg(0) = "AllClassCheck" Then
            Dim Ch = Msg(1)
            Dim CName = Msg(2)

            'ไม่ติ๊ก เอาลูกออก

            If Not Ch = True Then

                If Not InStr(CName, "cbAll") = 0 Then
                    For index = 0 To GvCondition.MasterTableView.Items.Count - 1
                        Dim Cball As CheckBox = CType(GvCondition.MasterTableView.Items(index).FindControl("cbAll"), CheckBox)
                        Cball.Checked = False
                        For i = 4 To 15 ' วน 15 คอลัม
                            Dim cc As CheckBox = CType(GvCondition.MasterTableView.Items(index).FindControl("C" & i.ToString), CheckBox)
                            cc.Checked = False '        
                        Next
                    Next
                End If


                For index = 0 To GvCondition.MasterTableView.Items.Count - 1

                    If GvCondition.MasterTableView.Items(index)("a").Text = "ทั้งหมด" Then
                        Dim Cball As CheckBox = CType(GvCondition.MasterTableView.Items(index).FindControl("cbAll"), CheckBox)
                        Cball.Checked = False
                    End If


                    If Not GvCondition.MasterTableView.Items(index)("a").Text = "ทั้งหมด" Then
                        For i = 4 To 15
                            Dim cc As CheckBox = CType(GvCondition.MasterTableView.Items(index).FindControl("C" & i.ToString), CheckBox)
                            cc.Checked = False
                        Next
                    End If


                Next
            End If



            '------------------------------------------------------------------------------------------------------------

            For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                Dim c As CheckBox = CType(dataItem.FindControl("cbAll"), CheckBox)

                If c.Checked Then '    
                    For i = 4 To 15 ' วน 15 คอลัม
                        Dim cc As CheckBox = CType(dataItem.FindControl("C" & i.ToString), CheckBox)
                        cc.Checked = True
                    Next  ' วน 15 คอลัม 
                End If
            Next  ' วนกริดบนทีละแถว

            Dim row7 As GridDataItem = GvCondition.MasterTableView.Items(0)

            For i = 4 To 15
                Dim c1 As CheckBox = CType(row7.FindControl("cbAll"), CheckBox)
                If c1.Checked Then
                    For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                        Dim c11 As CheckBox = CType(dataItem.FindControl("cbAll"), CheckBox)
                        c11.Checked = True
                    Next
                End If
            Next
            For i = 4 To 15
                Dim c1 As CheckBox = CType(row7.FindControl("C" & i.ToString), CheckBox)

                If c1.Checked Then
                    For Each dataItem As GridDataItem In GvCondition.MasterTableView.Items
                        Dim c11 As CheckBox = CType(dataItem.FindControl("C" & i.ToString), CheckBox)
                        c11.Checked = True
                    Next
                End If
            Next

        End If



    End Sub


    Protected Sub btnExportPDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportPDF.Click


        Dim Userid As Integer
        If Not Request.QueryString("id") Is Nothing Then
            If StateSave = "OK" Then
                'Userid =
                Response.Redirect("~/UserManager/UploadPicture.aspx?id=" & Request.QueryString("id").ToString, False)
            Else
                Dim msg = "ต้องทำการบันทึกข้อมูลก่อนนะคะ"

                WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)
            End If
        Else
            If StateSave = "OK" Then
                Userid = (ClsBudget.Max("tblUser", "UserId")).ToString
                Response.Redirect("~/UserManager/UploadPicture.aspx?id =" & Userid.ToString, False)
            Else
                Dim msg = "ต้องทำการบันทึกข้อมูลก่อนนะคะ"

                WindowsTelerik.ShowAlert(msg, Me, RadDialogAlert)

            End If


        End If

        '    Log.Record(Log.LogType.ManageUser, "สร้างPDF", True)


    End Sub

    Protected Sub lbtnAddUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnAddUser.Click
        Log.Record(Log.LogType.ManageUser, "เพิ่มผู้ใช้", True)
        Response.Redirect("~/UserManager/UserPage.aspx", False)
    End Sub

    Protected Sub lbtnSearchUser_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSearchUser.Click
        Log.Record(Log.LogType.ManageUser, "ค้นหาข้อมูลผู้ใช้", True)
        Response.Redirect("~/UserManager/UserManagerPage.aspx", False)
    End Sub

    
End Class

<Serializable()>
Public Class ListDBSC

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

    Private _No As Integer
    Public Property No() As Integer
        Get
            Return _No
        End Get
        Set(ByVal value As Integer)
            _No = value
        End Set
    End Property

End Class