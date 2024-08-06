Option Strict Off
Imports Excel = Microsoft.Office.Interop.Excel
Imports Telerik.Web.UI
Imports Microsoft.Office
Imports System.Runtime.InteropServices
Imports System.Web
Imports System.Data.SqlClient
Imports System.IO



Public Class AddEvaluationIndexForQuestion
    Inherits System.Web.UI.Page
    Dim useCls As New ClassConnectSql
    'Dim Questions_ID As String = Request.QueryString("qid")
    ' Dim Questions_ID As String = "ada15ea8-f60a-4882-b4dc-7e901dbb6910"
    Dim AllArrayIndexItem As New ArrayList
    'Dim RadTabMainEvaluationIndex As New RadTabStrip
    'Dim RadAllMultiPage As New RadMultiPage
    ''' <summary>
    ''' ทำการ Bind ข้อมูลดัชนีเข้าไปใน Control ต่างๆ , Bind ข้อมูลที่ User เลือกไว้ในกรณีที่มีข้อมูลอยู่แล้ว
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else

            Dim Questions_ID As String = Request.QueryString("qid")

            If Not Page.IsPostBack Then
                Log.Record(Log.LogType.ManageExam, "ไปหน้าจอแก้ไขดัชนีชี้วัด (Questionid=" & Request.QueryString("qid") & ")", True, "", Questions_ID)
                Log.Record(Log.LogType.ManageExam, "เข้า Tab ดัชนีตัวชี้วัด", True, "", Questions_ID)
            End If

            Session("QuestionId") = Questions_ID
            'ทำการ assign เพื่อให้ตอนแก้ไขส่ง path นี้กลับไปเพื่อ reload หน้าใหม่
            Session("ForEditEIName") = "../TestSet/AddEvaluationIndexForQuestion.aspx?qid=" & Questions_ID

            'If Session("SuperUser") = True Then
            '    btnConfirmSave.Visible = True
            '    btnSave.Enabled = False
            'End If

            'RadAllMultiPage.ID = "RadAllMultiPageId"
            'RadTabMainEvaluationIndex()

            'ทำการ clear Item ใน Tab ทิ้งไปก่อนเพื่อที่จะ add เข้าไปใหม่
            RadTabMainEvaluationIndex.Tabs.Clear()
            'ทำการ clear item ใน Multipage ทิ้งไปก่อนเพื่อที่จะ add เข้าไปใหม่
            RadAllMultiPage.PageViews.Clear()
            'ทำการสร้างข้อมูลดัชนี แบบ ตัวชี้วัด ซึ่งมี 4 ระดับ เช่น ตัวชี้วัด -> ภาษาเพื่อการสื่อสาร -> นำเสนอข้อมูลข่าวสาร ความคิดรวบยอด และความคิดเห็นในเรื่องต่างๆ โดยการพูด และการเขียน -> พูด และเขียนบรรยายเกี่ยวกับตนเอง กิจวัตรประจำวัน ประสบการณ์ และสิ่งแวดล้อมใกล้ตัว , เขียนภาพแผนผัง และแผนภูมิแสดงข้อมูลต่างๆตามที่ฟังหรืออ่าน ฯลฯ เป็นต้น
            CreateTabNewEvaluation("EF957BC3-C463-4315-8847-8B4522CA0100")
            CreateTabNewEvaluation("51479817-06FB-4F99-83E4-D33D4D01F7D2")

            'ทำการสร้างข้อมูลดัชนี แบบเก่า ซึ่งมีแค่ 3 ระดับ เช่น ระดับความยากง่าย -> เลือกระดับความยากง่าย -> ง่ายมาก,ง่าย,ปานกลาง,ยาก,ยากที่สุด
            BindDataCreateTabOnLoad()
            'หาข้อมูลที่เคยเลือกเอาไว้แล้วมา bind control ในกรณีที่เคยมีข้อมูลแล้ว
            SetCheckForControl()

            '20240802 แสดงปุ่ม แก้ไขตัวชี้วัด , อนุมัติ การลบ แก้ไข ตัวชี้วัด
            CheckUserPermission()


            'If IsConfirm() = True Then
            '    If Session("SuperUser") IsNot Nothing Then
            '        If Session("SuperUser") = False Then
            '            btnSave.Enabled = False
            '            Dim Mainpanel As Panel = Nothing
            '            For Each allMainPanelID In Session("MainPanel")
            '                Dim eachMainPanel As Control = Me.FindControl(allMainPanelID)
            '                Mainpanel = eachMainPanel
            '                Mainpanel.Enabled = False
            '            Next
            '        End If
            '    End If
            'End If

        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้าง Control ของดัชนีชี้วัดที่มีระดับแค่ 3 ระดับ เช่น ระดับความยากง่าย -> เลือกระดับความยากง่าย -> ยากที่สุด,ยาก,ง่าย,ง่ายที่สุด,ปานกลาง
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindDataCreateTabOnLoad()
        Dim Questions_ID As String = Request.QueryString("qid")
        Dim SubjectId As String = GetSubjectIdByQuestionId(Questions_ID)
        Dim LevelId As String = GetLevelIdByQuestionId(Questions_ID)

        If SubjectId <> "" And LevelId <> "" Then
            'หาข้อมูลที่จะนำมาทำเป็นหัว Tab จะขอยกตัวอย่างเพื่อนำไปใช้กับ loop ถัดๆไปด้วย เช่น KPA
            Dim dtCheckIndexWithSubJect As DataTable = GetEI_IdByGroupSubjectId(SubjectId)
            'ตัวแปรที่เอาไว้เป็นเลขรันของ Item ไปเรื่อยๆเอาไว้ทำเป็น Id ของ Panel Item
            Dim TotalPanelIndexItem As Integer = 1
            'ตัวแปรที่เอาไว้นับเป็นเลขรันของ Panel ดัชนีแต่ละอัน เอาไว้ทำเป็น Id ของ Panel
            Dim TotalPanelMain As Integer = 1
            'Dim ArrayIndexItem As New ArrayList
            'Dim ArrayMainPanel As New ArrayList
            'ArrayMainPanel = Session("MainPanel")
            If dtCheckIndexWithSubJect.Rows.Count > 0 Then
                'ทำการ loop เพื่อทำการสร้างข้อมูลหัว Tab , เงื่อนไขการจบ loop คือ วนจนครบหมดทุกดัชนีที่มีในวิชานี้
                For i = 0 To dtCheckIndexWithSubJect.Rows.Count - 1
                    'เอาไว้เก็บ Id ของ Panel Item แต่ละอันเพื่อเอา Assign ให้ Session ไว้ใช้ตอน save
                    Dim ArrEvaItem As New ArrayList
                    'เอาไว้เช็คว่าคำถามข้อนี้ต้องใส่รูปที่หัว Tab รึเปล่า
                    Dim CheckForAddImage As Boolean
                    CheckForAddImage = CheckAddImage(dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString)
                    If CheckForAddImage = True Then
                        'ถ้าต้องขึ้นรูปเครื่องหมายถูก ต้องหาด้วยว่าคำถามข้อนี้ได้รับการยืนยันจากหัวหน้าแล้วรึเปล่า ถ้าใช่ต้องแสดงเครื่องหมายถูก 2 อัน ซึ่งบ่งบอกว่าได้รับการยืนยันแล้ว
                        If IsConfirm(dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString) = True Then
                            'ใช้รูปเครื่องหมายติ๊กถูก 2 อัน
                            RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dtCheckIndexWithSubJect.Rows(i)("EI_Code"), .ImageUrl = "../Images/Right Tick/20120530-164512_Imagen_5asd.png"})
                        Else
                            'ใช้รูปเครื่องหมายติ๊กถูกอันเดียว
                            RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dtCheckIndexWithSubJect.Rows(i)("EI_Code"), .ImageUrl = "../Images/Right Tick/tickIcon.gif"})
                        End If
                    Else
                        'ไม่ต้องมีรูป
                        RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dtCheckIndexWithSubJect.Rows(i)("EI_Code")})
                    End If

                    Dim dtIndexGroupname As DataTable
                    'ทำการหา Item ระดับต่อไปของ Tab นี้ เช่น K-ด้านความรู้ , P-ด้านทักษะกระบวนการ , A-ด้านคุณลักษณะ
                    dtIndexGroupname = GetEvaluationGroupByEIIdOldEvaluation(dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString())
                    Dim GroupMainPanel As New Panel
                    With GroupMainPanel
                        .ID = "GroupMainPanel" & TotalPanelMain
                    End With
                    Dim ThisPageView As New RadPageView
                    ThisPageView.CssClass = "TestClass"
                    RadAllMultiPage.PageViews.Add(ThisPageView)
                    If dtIndexGroupname.Rows.Count > 0 Then
                        'loop เพื่อทำการสร้าง Label Item ระดับที่รองมาจากหัว Tab เช่น เลือกระดับความยากง่ายก่อน , เงื่อนไขการจบ loop คือวนจนกว่า Item ระดับนี้จะหมด ต้องสร้างให้ครบ
                        For j = 0 To dtIndexGroupname.Rows.Count - 1
                            'เป็นตัวแปรที่บอกว่าจะต้องทำการสร้าง Control ของระดับ Item เป็น checkbox หรือ radio
                            Dim NeedSingleChoices As Integer = dtIndexGroupname.Rows(j)("NeedSingleChoice")
                            Dim GroupLabelPanel As New Panel
                            With GroupLabelPanel
                                .Style.Add("padding-top", "20px")
                                .Style.Add("background-color", "#2DCDFF")
                                '.Style.Add("float", "left") '----------------------
                            End With
                            Dim EachLabel As New Label
                            Dim GroupCheckBoxOrRadioPanel As New Panel
                            'เช็คว่าเป็น SuperUser หรือเปล่า
                            If Session("SuperUser") = False Then
                                'เช็คอีกว่า อนุมัติหรือยัง ถ้าอนุมัติแล้วต้องไม่ให้ checkbox,radio ติ๊กได้
                                If IsConfirm(dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString) = True Then
                                    GroupCheckBoxOrRadioPanel.Enabled = False
                                End If
                            End If

                            With GroupCheckBoxOrRadioPanel
                                .ID = "PanelIndexItem" & TotalPanelIndexItem
                                .Style.Add("border-bottom", "solid")
                                .Style.Add("padding-top", "10px")
                                .Style.Add("float", "left") '----------------------
                                .Style.Add("padding-bottom", "20px")
                                .Style.Add("background-color", "#2DCDFF")
                                .Style.Add("width", "100%")
                            End With

                            GroupLabelPanel.ID = dtIndexGroupname.Rows(j)("EI_Id").ToString
                            EachLabel.Text = dtIndexGroupname.Rows(j)("EI_Code")
                            GroupLabelPanel.Controls.Add(EachLabel)

                            'Dim sqlSelectIndexitem As String = "Select EII_Id,EII_Name From tblEvaluationIndexItem where EIG_Id = '" & dtIndexGroupname.Rows(j)("EIG_Id").ToString & "' order by EII_Code"

                            'ทำการหา Item ระดับท้ายสุด เช่น 1) รักชาติ ศาสน์ กษัตริย์,2) ซื่อสัตย์ สุจริต,3) มีวินัย,4) ใฝ่เรียนรู้,5) อยู่อย่างพอเพียง,6) มุ่งมั่นในการทำงาน,7) รักความเป็นไทย เป็นต้น
                            Dim dtIndexItem As DataTable = GetdtLastGroup(dtIndexGroupname.Rows(j)("EI_Id").ToString())

                            If dtIndexItem.Rows.Count > 0 Then
                                'ถ้าไม่ได้เลือกให้ตอบได้แค่ตัวเลือกเดียวก็สร้างให้เป็น Checkbox
                                If NeedSingleChoices = 0 Then
                                    'loop เพื่อสร้าง Control ในระดับสุดท้าย ซึ่งเป็นพวกตัวเลือกต่างๆของดัชนีนี้ แบบเป็น checkbox , เงื่อนไขการจบ loop คือ วนจนครบทุก Item
                                    For k = 0 To dtIndexItem.Rows.Count - 1
                                        Dim chk As New CheckBox
                                        With chk
                                            .Style.Add("margin-left", "15px")
                                            .Style.Add("margin-top", "20px")
                                            .InputAttributes.Add("onchange", "SetDirty();")
                                            .Style.Add("float", "left") '----------------------
                                        End With
                                        'Dim EditBtn As New System.Web.UI.WebControls.Image
                                        'With EditBtn
                                        '    .ID = "Edit" & dtIndexItem.Rows(k)("EI_Id").ToString()
                                        '    .Attributes.Add("EIID", dtIndexItem.Rows(k)("EI_Id").ToString())
                                        '    .Attributes.Add("Onclick", "EditEiName('" & dtIndexItem.Rows(k)("EI_Id").ToString() & "','" & dtIndexItem.Rows(k)("EI_Code") & "','False')")
                                        '    .ImageUrl = "../Images/freehand.png"
                                        '    .Style.Add("position", "relative")
                                        '    .Style.Add("top", "15px")
                                        '    .Style.Add("cursor", "pointer")
                                        '    .Style.Add("float", "left")
                                        'End With

                                        chk.ID = dtIndexItem.Rows(k)("EI_Id").ToString
                                        chk.Text = dtIndexItem.Rows(k)("EI_Code")
                                        GroupCheckBoxOrRadioPanel.Controls.Add(chk)
                                        'GroupCheckBoxOrRadioPanel.Controls.Add(EditBtn)
                                    Next
                                Else
                                    'ถ้าเลือกให้เลือกได้แค่ตัวเลือกเดียว ต้องสร้างเป็น Radio Control
                                    'loop เพื่อสร้าง Control Item ระดับท้ายสุด ให้เป็นแบบ Radio , เงื่อนไขการจบ loop คือ วนสร้างจนครบทุก Item
                                    For k = 0 To dtIndexItem.Rows.Count - 1
                                        Dim rdo As New RadioButton
                                        rdo.ID = dtIndexItem.Rows(k)("EI_Id").ToString
                                        rdo.Text = dtIndexItem.Rows(k)("EI_Code")
                                        rdo.GroupName = dtIndexGroupname.Rows(j)("EI_Id").ToString()
                                        With rdo
                                            .Style.Add("margin-left", "15px")
                                            .Style.Add("margin-top", "20px")
                                            .InputAttributes.Add("onchange", "SetDirty();")
                                            .Style.Add("float", "left") '----------------------
                                        End With
                                        'Dim EditBtn As New System.Web.UI.WebControls.Image
                                        'With EditBtn
                                        '    .ID = "Edit" & dtIndexItem.Rows(k)("EI_Id").ToString()
                                        '    .Attributes.Add("EIID", dtIndexItem.Rows(k)("EI_Id").ToString())
                                        '    .Attributes.Add("Onclick", "EditEiName('" & dtIndexItem.Rows(k)("EI_Id").ToString() & "','" & dtIndexItem.Rows(k)("EI_Code") & "','False')")
                                        '    .ImageUrl = "../Images/freehand.png"
                                        '    .Style.Add("position", "relative")
                                        '    .Style.Add("top", "15px")
                                        '    .Style.Add("cursor", "pointer")
                                        '    .Style.Add("float", "left")
                                        'End With
                                        GroupCheckBoxOrRadioPanel.Controls.Add(rdo)
                                        'GroupCheckBoxOrRadioPanel.Controls.Add(EditBtn)
                                    Next
                                End If
                            End If

                            GroupMainPanel.Controls.Add(GroupLabelPanel)
                            GroupMainPanel.Controls.Add(GroupCheckBoxOrRadioPanel)

                            ThisPageView.Controls.Add(GroupMainPanel)
                            'ArrayIndexItem.Add(GroupCheckBoxOrRadioPanel.ID)
                            'AllArrayIndexItem.Add(GroupCheckBoxOrRadioPanel.ID)

                            'ทำการ Add ข้อมูล Item ระดับท้ายสุดเข้าไปใน Array เพื่อเอาไป Assign ให้ Session เอาไว้ใช้ตอน save
                            ArrEvaItem.Add(GroupCheckBoxOrRadioPanel.ID)
                            'ทำการ RunId ของ Panel Item ไปเรื่อย
                            TotalPanelIndexItem += 1
                        Next
                    End If
                    'ArrayMainPanel.Add(GroupMainPanel.ID)
                    'ทำการ RunId ของ Panel หลัก ไปเรื่อย
                    TotalPanelMain += 1

                    'สร้างปุ่มสำหรับ Save โดยตรวจก่อนว่าสร้างปุ่มแบบ อนุมัติได้หรือไม่ได้
                    If Session("SuperUser") = True Then
                        Dim NewPanel As New Panel
                        Dim newBtn As New Button
                        With newBtn
                            .ID = "OldEva" & i
                            '.ID = dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString
                            .Text = "ยืนยันการบันทึก [" & dtCheckIndexWithSubJect(i)("EI_Code") & "]"
                            .Style.Add("float", "right")
                            .Attributes("EI_Id") = dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString
                            .Attributes("NewEva") = "False"
                            .Attributes("IsConfirm") = "True"
                        End With
                        'ทำการ Add Event ของปุ่มให้เป็นการ save แบบ confirm เพราะว่าเป็น SuperUser มาทำการอนุมัติแล้ว
                        AddHandler newBtn.Click, AddressOf ConfirmSave
                        NewPanel.Controls.Add(newBtn)
                        ThisPageView.Controls.Add(NewPanel)
                    Else
                        If (IsConfirm(dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString) = False) Then
                            Dim NewPanel As New Panel
                            Dim newBtn As New Button
                            With newBtn
                                .ID = "OldEva" & i
                                .Text = "บันทึก [" & dtCheckIndexWithSubJect(i)("EI_Code") & "]"
                                .Style.Add("float", "right")
                                .Attributes("EI_Id") = dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString
                                .Attributes("NewEva") = "False"
                                .Attributes("IsConfirm") = "False"
                            End With
                            'ทำการ Add Event ของปุ่มให้เป็นการ save แบบปกติ
                            AddHandler newBtn.Click, AddressOf NormalSave
                            NewPanel.Controls.Add(newBtn)
                            ThisPageView.Controls.Add(NewPanel)
                        End If
                    End If
                    'ทำการ add ข้อมูล Item ต่างๆเข้าไปใน Session เพื่อนำไปใช้ตอน save
                    Session("Arr" & dtCheckIndexWithSubJect.Rows(i)("EI_Id").ToString) = ArrEvaItem
                Next
            Else

            End If
            'Session("MainPanel") = ArrayMainPanel
            'Session("PanelIndexItemid") = ArrayIndexItem
            'Session("PanelIndexItemid") = AllArrayIndexItem

        End If

    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลว่ามีการเลือกข้อมูลไหนบ้างแล้ว ที่คำถามข้อนี้ ในกรณีที่เคยมีการเลือกไว้ก่อนแล้ว
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetCheckForControl()
        Dim Questions_ID As String = Request.QueryString("qid")
        Dim sqlQuestionEvaluationIndexItem As String = " select EI_Id,IsApproved from tblQuestionEvaluationIndexItem where Question_Id = '" & Questions_ID & "' AND IsActive = '1' "
        Dim dtQuestionEvaluationIndexItem As New DataTable
        'ทำการหา Item และ สถานะว่าอนุมัติแล้วหรือยัง ของคำถามข้อนี้
        dtQuestionEvaluationIndexItem = useCls.getdata(sqlQuestionEvaluationIndexItem)
        Dim CompareCheckbox As CheckBox = Nothing
        Dim CompareRadio As RadioButton = Nothing
        If dtQuestionEvaluationIndexItem.Rows.Count > 0 Then
            'loop เพื่อหา Control checkbox,radio เพื่อทำการ tick เลือก , เงื่อนไขการจบ loop คือ วนจนครบทุก Item ที่ได้ทำการเลือกไว้
            For i = 0 To dtQuestionEvaluationIndexItem.Rows.Count - 1
                'Id ของ Item ที่ดึงมาจากฐานข้อมูล
                Dim IndexItemId As String = dtQuestionEvaluationIndexItem.Rows(i)("EI_Id").ToString()
                'ทำการหา Checkbox,Radio ที่เป็น Id เดียวกับ Item ที่เลือกไว้
                Dim allControl As Control = Me.FindControl(IndexItemId.ToString)
                'ทำการ tick เลือก
                If TypeOf allControl Is CheckBox Then
                    CompareCheckbox = allControl
                    CompareCheckbox.Checked = True
                ElseIf TypeOf allControl Is RadioButton Then
                    CompareRadio = allControl
                    CompareRadio.Checked = True
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' ทำการ save ข้อมูลแบบยังไม่ได้อนุมัติ
    ''' </summary>
    ''' <param name="sender">object Button ที่ทำการกดตกลง</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub NormalSave(ByVal sender As Object, ByVal e As EventArgs)
        'ทำการ assign ค่าให้เป็นปุ่มที่กด save ก่อน
        Dim newBtn As New Button
        newBtn = sender
        'เริ่ม Get Attribute ที่ฝากไว้กับปุ่มตอนที่สร้างขึ้นมา
        Dim EI_Id As String = newBtn.Attributes("EI_Id").ToString()
        Dim IsNewEvaluationIndex As String = newBtn.Attributes("NewEva").ToString()
        Dim Questions_ID As String = Request.QueryString("qid")
        'นำ Id มาแทนค่าเพื่อดึงค่า Array ที่เก็บ Id ของ Panel Item เอาไว้ออกมาเพื่อส่งไป save
        Dim ArrList As ArrayList = Session("Arr" & EI_Id)
        'ส่งไป save แบบไม่อนุมัติ
        SaveData(EI_Id, Questions_ID, ArrList, False, IsNewEvaluationIndex)
    End Sub

    ''' <summary>
    ''' ทำการ save ข้อมูลแบบอนุมัติ
    ''' </summary>
    ''' <param name="sender">object Button ที่ทำการกดตกลง</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ConfirmSave(ByVal sender As Object, ByVal e As EventArgs)
        'ทำการ assign ค่าให้เป็นปุ่มที่กด save ก่อน
        Dim newBtn As New Button
        newBtn = sender
        'เริ่ม Get Attribute ที่ฝากไว้กับปุ่มตอนที่สร้างขึ้นมา
        Dim EI_Id As String = newBtn.Attributes("EI_Id").ToString()
        Dim IsNewEvaluationIndex As String = newBtn.Attributes("NewEva").ToString()
        Dim Questions_ID As String = Request.QueryString("qid")
        'นำ Id มาแทนค่าเพื่อดึงค่า Array ที่เก็บ Id ของ Panel Item เอาไว้ออกมาเพื่อส่งไป save
        Dim ArrList As ArrayList = Session("Arr" & EI_Id)
        'ส่งไป save แบบอนุมัติ
        SaveData(EI_Id, Questions_ID, ArrList, True, IsNewEvaluationIndex)
    End Sub

    ''' <summary>
    ''' ทำการ update ข้อมูลดัชนี
    ''' </summary>
    ''' <param name="EI_ID">Id หลักที่จะนำมาเป็น ParentId เพื่อหา Item ถัดๆไป</param>
    ''' <param name="Questions_ID">Id ของคำถามข้อนี้</param>
    ''' <param name="ArrEvaItem">Array ที่มี Id ของ Panel ที่เก็บ Item เอาไว้</param>
    ''' <param name="IsConfirm">อนุมัติ ?</param>
    ''' <param name="IsNewEva">เป็นตัวชี้วัด ?</param>
    ''' <remarks></remarks>
    Private Sub SaveData(ByVal EI_ID As String, ByVal Questions_ID As String, ByVal ArrEvaItem As ArrayList, ByVal IsConfirm As Boolean, ByVal IsNewEva As String)

        Dim sql As String = ""
        Dim StrLog As String = ""
        Dim StrEditId As String = ""
        'Id ระดับรองจากระดับแรกสุด(หัว Tab)
        Dim EiCode As String = GetEICodeByEIID(EI_ID)

        'txt รายละเอียด Log
        'If IsConfirm = True Then
        StrLog = "บันทึกดัชนี " & EiCode
        'Else
        'StrLog = "แก้ไขดัชนี " & EiCode
        'End If

        useCls.OpenWithTransection()
        Try
            ' Update IsActive ของเก่าให้เป็น 0
            'ถ้าเป็น ตัวชี้วัดต้อง update ระดับมากกว่า ตัวชี้วัดแบบปกติ
            If IsNewEva = True Then
                sql = " UPDATE dbo.tblQuestionEvaluationIndexItem SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id = '" & Questions_ID & "' AND EI_Id IN (  " &
                      " SELECT EI_Id FROM dbo.tblQuestionEvaluationIndexItem WHERE Question_Id = '" & Questions_ID & "' AND EI_Id IN (  " &
                      " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN (  " &
                      " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN (  " &
                      " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EI_ID & "' AND Isactive = '1' ) " &
                      " AND dbo.tblEvaluationIndexNew.IsActive = 1  ) AND dbo.tblEvaluationIndexNew.IsActive = 1)  " &
                      " AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1 )   "
            Else
                sql = " UPDATE dbo.tblQuestionEvaluationIndexItem SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE EI_Id IN ( " &
                      " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " &
                      " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EI_ID & "' AND IsActive = 1)  " &
                      " AND dbo.tblEvaluationIndexNew.IsActive = 1) AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1  " &
                      " AND Question_Id =  '" & Questions_ID & "' "
            End If
            useCls.ExecuteWithTransection(sql)

            Dim CheckboxSave As CheckBox = Nothing
            Dim RadioSave As RadioButton = Nothing
            Dim CheckRadioType As Boolean = False
            'loop เพื่อหา Control ใน Panel , เงื่อนไขการจบ loop คือ วนจนครบหมดทุก Control ที่อยู่ใน Array ที่ส่งเข้ามา
            For Each z In ArrEvaItem
                Dim GroupPanelIndexitem As Control = Me.FindControl(z)
                'ทำการ loop เพื่อหาแต่ checkbox,radio เท่านั้น เพื่อนำมาเช็คว่ามีอันไหนที่ถูกเลือกบ้าง จะได้นำมา update ข้อมูล , เงื่อนไขการจบ loop คือ วนให้ครบทุก Control ในแต่ละ Panel
                For Each r In GroupPanelIndexitem.Controls
                    If r.GetType.Name = "CheckBox" Then
                        CheckboxSave = r
                        'ถ้า Checkbox ถูกเลือกก็ Insert เป็น EI_Id ของดัชนีที่เลือก
                        If CheckboxSave.Checked = True Then

                            Dim sqlSaveQEII As String = ""
                            If IsConfirm = True Then
                                sqlSaveQEII = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " &
                                              " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " &
                                              " VALUES  ( NEWID() , '" & CheckboxSave.ID & "' , '" & Questions_ID & "' , 1 , 1 , dbo.GetThaiDate() )  "
                            Else
                                sqlSaveQEII = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " &
                                              " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " &
                                              " VALUES  ( NEWID() , '" & CheckboxSave.ID & "' , '" & Questions_ID & "' , 0 , 1 , dbo.GetThaiDate() )  "
                            End If
                            useCls.ExecuteWithTransection(sqlSaveQEII)
                            StrEditId = StrEditId & "," & CheckboxSave.ID

                        End If
                        'ถ้าเป็น Radio ถูกเลือกเข้า Else
                    ElseIf r.GetType.Name = "RadioButton" Then
                        RadioSave = r
                        'ถ้า Radio ถูกเลือกทำการ Insert ข้อมูล
                        If RadioSave.Checked = True Then
                            CheckRadioType = True
                            Dim sqlSaveQEII As String = ""
                            If IsConfirm = True Then
                                sqlSaveQEII = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " &
                                              " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " &
                                              " VALUES  ( NEWID() , '" & RadioSave.ID & "' , '" & Questions_ID & "' , 1 , 1 , dbo.GetThaiDate() )  "
                            Else
                                sqlSaveQEII = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " &
                                              " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " &
                                              " VALUES  ( NEWID() , '" & RadioSave.ID & "' , '" & Questions_ID & "' , 0 , 1 , dbo.GetThaiDate() )  "
                            End If
                            useCls.ExecuteWithTransection(sqlSaveQEII)
                            StrEditId = "บันทึกระดับความยากง่ายเป็น " & RadioSave.Text
                        End If
                    End If
                Next
            Next

            If CheckRadioType = True Then
                StrLog = StrEditId.Substring(1, StrEditId.Length - 1) & " (QuestionId=" & Questions_ID & ")"
            Else
                If StrEditId = "" Then
                    StrLog = StrLog & " ไม่เลือกดัชนีเลย"
                Else
                    StrLog = StrLog & " Id " & StrEditId.Substring(1, StrEditId.Length - 1) & " (QuestionId=" & Questions_ID & ")"
                End If

            End If

            Log.Record(Log.LogType.ManageExam, useCls.CleanString(StrLog), True, "", Questions_ID)

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            useCls.RollbackTransection()
        End Try
        useCls.CommitTransection()


        Response.Redirect("~/TestSet/AddEvaluationIndexForQuestion.aspx?qid=" & Request.QueryString("qid").ToString())

    End Sub

    ''' <summary>
    ''' ทำการหา Id Item ระดับถัดไปจากระดับแรกสุด
    ''' </summary>
    ''' <param name="EI_ID">Id ดัชนีระดับแรกสุด</param>
    ''' <returns>String:Id Item ระดับถัดไปจากแรกสุด</returns>
    ''' <remarks></remarks>
    Private Function GetEICodeByEIID(ByVal EI_ID As String) As String
        Dim sql As String = " SELECT EI_Code FROM dbo.tblEvaluationIndexNew WHERE EI_Id = '" & EI_ID & "' AND IsActive = 1 "
        Dim EICode As String = useCls.ExecuteScalar(sql)
        Return EICode
    End Function

#Region "SaveFunction CommentBlock"
    'Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

    '    Dim Questions_ID As String = Request.QueryString("qid")
    '    'Dim sqlDeleteBeforeSaveIndex As String = "Delete from tblQuestionEvaluationIndexItem where Question_Id ='" & Questions_ID & "'"
    '    Dim sqlUpdateBeforeSaveIndex As String = " UPDATE dbo.tblQuestionEvaluationIndexItem SET IsActive = 0,LastUpdate = dbo.GetThaiDate() " & _
    '                                             " WHERE Question_Id = '" & Questions_ID & "' "
    '    useCls.OpenWithTransection()
    '    Try
    '        'Update  IsActive ของเก่าให้เป็น 0 ก่อน
    '        useCls.ExecuteWithTransection(sqlUpdateBeforeSaveIndex)
    '        Dim CheckboxSave As CheckBox = Nothing
    '        Dim RadioSave As RadioButton = Nothing
    '        For Each allPanelId In Session("PanelIndexItemid")
    '            Dim GroupPanelIndexitem As Control = Me.FindControl(allPanelId)
    '            For Each GetControlInPanel In GroupPanelIndexitem.Controls
    '                'ถ้าเป็น Checkbox เข้า If 
    '                If GetControlInPanel.GetType.Name = "CheckBox" Then
    '                    CheckboxSave = GetControlInPanel
    '                    'ถ้า Checkbox ถูกเลือกก็ Insert เป็น EI_Id ของดัชนีที่เลือก
    '                    If CheckboxSave.Checked = True Then
    '                        Dim sqlSaveQEII As String = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " & _
    '                                                    " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " & _
    '                                                    " VALUES  ( NEWID() , '" & CheckboxSave.ID & "' , '" & Questions_ID & "' , 0 , 1 , dbo.GetThaiDate() )  "
    '                        useCls.ExecuteWithTransection(sqlSaveQEII)
    '                    End If
    '                    'ถ้าเป็น Radio ถูกเลือกเข้า Else
    '                ElseIf GetControlInPanel.GetType.Name = "RadioButton" Then
    '                    RadioSave = GetControlInPanel
    '                    'ถ้า Radio ถูกเลือกทำการ Insert ข้อมูล
    '                    If RadioSave.Checked = True Then
    '                        Dim sqlSaveQEII As String = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " & _
    '                                                    " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " & _
    '                                                    " VALUES  ( NEWID() , '" & RadioSave.ID & "' , '" & Questions_ID & "' , 0 , 1 , dbo.GetThaiDate() )  "
    '                        useCls.ExecuteWithTransection(sqlSaveQEII)
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Catch ex As Exception
    '        useCls.RollbackTransection()
    '    End Try
    '    useCls.CommitTransection()
    '    'Save Log
    '    Log.Record(Log.LogType.ManageExam, useCls.CleanString("แก้ไขดัชนี Question_Id = '" & Questions_ID & "' "), True)
    '    Response.Redirect("~/TestSet/AddEvaluationIndexForQuestion.aspx?qid=" & Request.QueryString("qid").ToString())

    'End Sub

    'Private Sub btnConfirmSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmSave.Click

    '    Dim Questions_ID As String = Request.QueryString("qid")
    '    Dim sqlUpdateBeforeSaveIndex As String = " UPDATE dbo.tblQuestionEvaluationIndexItem SET IsActive = 0,LastUpdate = dbo.GetThaiDate() " & _
    '                                              " WHERE Question_Id = '" & Questions_ID & "' "
    '    useCls.OpenWithTransection()
    '    Try
    '        useCls.ExecuteWithTransection(sqlUpdateBeforeSaveIndex)
    '        Dim CheckboxSave As CheckBox = Nothing
    '        Dim RadioSave As RadioButton = Nothing
    '        For Each allPanelId In Session("PanelIndexItemid")
    '            Dim GroupPanelIndexitem As Control = Me.FindControl(allPanelId)
    '            For Each GetControlInPanel In GroupPanelIndexitem.Controls
    '                If GetControlInPanel.GetType.Name = "CheckBox" Then
    '                    CheckboxSave = GetControlInPanel
    '                    If CheckboxSave.Checked = True Then
    '                        Dim sqlSaveQEII As String = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " & _
    '                                                    " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " & _
    '                                                    " VALUES  ( NEWID() , '" & CheckboxSave.ID & "' , '" & Questions_ID & "' , 1 , 1 , dbo.GetThaiDate() )  "
    '                        useCls.ExecuteWithTransection(sqlSaveQEII)
    '                    End If
    '                ElseIf GetControlInPanel.GetType.Name = "RadioButton" Then
    '                    RadioSave = GetControlInPanel
    '                    If RadioSave.Checked = True Then
    '                        Dim sqlSaveQEII As String = " INSERT INTO dbo.tblQuestionEvaluationIndexItem " & _
    '                                                    " ( QEI_Id ,EI_Id ,Question_Id , IsApproved ,IsActive ,LastUpdate ) " & _
    '                                                    " VALUES  ( NEWID() , '" & RadioSave.ID & "' , '" & Questions_ID & "' , 1 , 1 , dbo.GetThaiDate() )  "
    '                        useCls.ExecuteWithTransection(sqlSaveQEII)
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Catch ex As Exception
    '        useCls.RollbackTransection()
    '    End Try
    '    useCls.CommitTransection()
    '    Log.Record(Log.LogType.ManageExam, useCls.CleanString("Approve Question_Id = '" & Questions_ID & "' "), True)
    '    Response.Redirect("~/TestSet/AddEvaluationIndexForQuestion.aspx?qid=" & Request.QueryString("qid").ToString())

    'End Sub

#End Region

    ''' <summary>
    ''' หารหัสชั้น
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ที่ต้องการหารหัสชั้น</param>
    ''' <returns>String:รหัสชั้น</returns>
    ''' <remarks></remarks>
    Private Function GetLevelIdByQuestionId(ByVal QuestionId As String) As String
        Dim sql As String = " SELECT TOP 1 tblLevel.Level_Id FROM tblLevel INNER JOIN " &
                            " tblBook ON tblLevel.Level_Id = tblBook.Level_Id INNER JOIN tblQuestion INNER JOIN " &
                            " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " &
                            " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id " &
                            " ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id WHERE (tblQuestion.Question_Id = '" & QuestionId & "') "
        Dim LevelId As String = useCls.ExecuteScalar(sql)
        Return LevelId
    End Function

    ''' <summary>
    ''' ทำการหารหัสวิชา
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ที่ต้องการหารหัสวิชา</param>
    ''' <returns>String:รหัสวิชา</returns>
    ''' <remarks></remarks>
    Private Function GetSubjectIdByQuestionId(ByVal QuestionId As String) As String
        Dim sql As String = " select GroupSubject_Id from tblGroupSubject where GroupSubject_Id " &
                            " in(select GroupSubject_Id from tblBook where BookGroup_Id in(select Book_Id from tblQuestionCategory where QCategory_Id in " &
                            " (select QCategory_Id from tblQuestionSet where QSet_Id in(select QSet_Id from tblQuestion where Question_Id = '" & QuestionId & "')))) "
        Dim SubjectId As String = useCls.ExecuteScalar(sql)
        Return SubjectId
    End Function

    ''' <summary>
    ''' หาข้อมูลดัชนีฃี้วัดที่จะนำไปทำเป็นหัว Tab ซึ่งก็คือหัวข้อใหญ่ เช่น KPA,แบบทดสอบระดับชาติ,ระดับความยากง่าย
    ''' </summary>
    ''' <param name="GroupSubjectId">Id ของ tblGroupSubject ของคำถามข้อนี้</param>
    ''' <returns>Datatable:ข้อมูลดัชนีที่จะนำไปทำเป็นหัว Tab</returns>
    ''' <remarks></remarks>
    Private Function GetEI_IdByGroupSubjectId(ByVal GroupSubjectId As String)
        Dim sql As String = " SELECT tblEvaluationIndexNew.EI_Id,tblEvaluationIndexNew.EI_Code FROM tblEvaluationIndexNew LEFT JOIN " &
                            " tblEvaluationIndexSubject ON tblEvaluationIndexNew.EI_Id = tblEvaluationIndexSubject.EI_Id " &
                            " WHERE (tblEvaluationIndexSubject.Subject_Id = '" & GroupSubjectId & "') " &
                            " AND (tblEvaluationIndexSubject.IsActive = 1) AND " &
                            " (tblEvaluationIndexNew.IsActive = 1) And (tblEvaluationIndexNew.Parent_Id Is NULL) " &
                            "  AND (dbo.tblEvaluationIndexNew.EI_Code <> 'ตัวชี้วัด') ORDER BY EI_Position "
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' เช็คว่า tab นี้ได้รับการอนุมัติไปหรือยัง สำหรับตัวชี้วัด
    ''' </summary>
    ''' <param name="EIID">Id ของหัว Tab</param>
    ''' <param name="LevelId">Id ของชั้น</param>
    ''' <param name="GroupsubjectId">Id ของวิชา</param>
    ''' <returns>Boolean:True=อนุมัติแล้ว,False=ยังไม่ได้อนุมัติ</returns>
    ''' <remarks></remarks>
    Private Function IsConfirmNewEvaluationIndex(ByVal EIID As String, ByVal LevelId As String, ByVal GroupsubjectId As String) As Boolean
        Dim Questions_ID As String = Request.QueryString("qid")
        Dim sql As String = " SELECT  DISTINCT IsApproved FROM dbo.tblQuestionEvaluationIndexItem WHERE Question_Id = '" & Questions_ID & "' " &
                            " AND EI_Id IN ( SELECT EI_Id FROM dbo.tblEvaluationIndexLevel WHERE EI_Id IN (  SELECT e3.EI_Id FROM dbo.tblEvaluationIndexNew " &
                            " AS e3 WHERE e3.Parent_Id IN  " &
                            " (  SELECT e2.EI_Id FROM dbo.tblEvaluationIndexNew AS e2 WHERE e2.Parent_Id IN " &
                            " (  SELECT dbo.tblEvaluationIndexSubject.EI_Id FROM tblEvaluationIndexNew AS e1 INNER JOIN  tblEvaluationIndexSubject " &
                            " ON e1.EI_Id = tblEvaluationIndexSubject.EI_Id  WHERE (tblEvaluationIndexSubject.Subject_Id = '" & GroupsubjectId & "') " &
                            " AND (e1.IsActive = 1)  AND (e1.Parent_Id = '" & EIID & "') ) ) )  " &
                            " AND  dbo.tblEvaluationIndexLevel.Level_Id = '" & LevelId & "') AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1  "

        Dim CheckConfirm As String = ""
        Dim AnswerFromCheck As Boolean
        CheckConfirm = useCls.ExecuteScalar(sql)
        If CheckConfirm <> "" Then
            If CheckConfirm = True Then
                AnswerFromCheck = True
            Else
                AnswerFromCheck = False
            End If
        End If
        Return AnswerFromCheck
    End Function

    ''' <summary>
    ''' ทำการหาว่าดัชนีนี้ ที่คำถามข้อนี้ ได้รับการยืนยันตรวจสอบจากหัวหน้าแล้วรึยัง
    ''' </summary>
    ''' <param name="EIId">ParentId ของ tblEvaluationIndexNew</param>
    ''' <returns>Boolean:True=ยืนยันแล้ว,False=ยังไม่ได้ยืนยัน</returns>
    ''' <remarks></remarks>
    Private Function IsConfirm(ByVal EIId As String) As Boolean

        Dim Questions_ID As String = Request.QueryString("qid")
        Dim sqlCheckIsConfirm As String = " SELECT DISTINCT IsApproved FROM tblQuestionEvaluationIndexItem INNER JOIN  tblEvaluationIndexNew " &
                                          " ON tblQuestionEvaluationIndexItem.EI_Id = tblEvaluationIndexNew.EI_Id  WHERE  " &
                                          " (tblQuestionEvaluationIndexItem.Question_Id = '" & Questions_ID & "')  AND  " &
                                          " (dbo.tblEvaluationIndexNew.EI_Id IN (SELECT e1.EI_Id FROM dbo.tblEvaluationIndexNew AS e1  WHERE e1.Parent_Id IN  " &
                                          " (SELECT e2.EI_Id FROM dbo.tblEvaluationIndexNew  AS e2 WHERE e2.Parent_Id = '" & EIId & "') ) )  " &
                                          " AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1  "
        'sqlCheckIsConfirm = (" Select Distinct IsApproved from tblQuestionEvaluationIndexItem where Question_Id = '" & Questions_ID & "' AND IsActive = 1 ORDER BY IsApproved DESC ")

        Dim CheckConfirm As String = ""
        Dim AnswerFromCheck As Boolean
        CheckConfirm = useCls.ExecuteScalar(sqlCheckIsConfirm)
        If CheckConfirm <> "" Then
            If CheckConfirm = True Then
                AnswerFromCheck = True
            Else
                AnswerFromCheck = False
            End If
        End If
        Return AnswerFromCheck

    End Function

    ''' <summary>
    ''' ทำการ check ว่าจะต้องแสดงรูปเครื่องหมายถูกที่หัว Tab รึเปล่า สำหรับ ตัวชี้วัด เพราะว่ามีระดับเยอะกว่าดัชนีปกติ เลยต้องแยก Function ออกมา
    ''' </summary>
    ''' <param name="EI_Id">Id ของหัว Tab</param>
    ''' <param name="LevelId">Id ของชั้น</param>
    ''' <param name="GroupSubjectId">Id ของวิชา</param>
    ''' <param name="QuestionId">Id ของคำถามข้อนี้</param>
    ''' <returns>Boolean:True=มีรูป,False=ไม่ต้องมีรูป</returns>
    ''' <remarks></remarks>
    Private Function CheckAddImageNewEvaluation(ByVal EI_Id As String, ByVal LevelId As String, ByVal GroupSubjectId As String, ByVal QuestionId As String) As Boolean
        Dim CheckCount As Boolean = False
        Dim CheckResult As String = ""
        Dim Sql As String = " SELECT  COUNT(*) FROM dbo.tblQuestionEvaluationIndexItem WHERE Question_Id = '" & QuestionId & "' AND EI_Id IN ( " &
                            " SELECT EI_Id FROM dbo.tblEvaluationIndexLevel WHERE EI_Id IN ( " &
                            " SELECT e3.EI_Id FROM dbo.tblEvaluationIndexNew AS e3 WHERE e3.Parent_Id IN ( " &
                            " SELECT e2.EI_Id FROM dbo.tblEvaluationIndexNew AS e2 WHERE e2.Parent_Id IN ( " &
                            " SELECT dbo.tblEvaluationIndexSubject.EI_Id FROM tblEvaluationIndexNew AS e1 INNER JOIN " &
                            " tblEvaluationIndexSubject ON e1.EI_Id = tblEvaluationIndexSubject.EI_Id " &
                            " WHERE (tblEvaluationIndexSubject.Subject_Id = '" & GroupSubjectId & "') AND (e1.IsActive = 1) " &
                            " AND (e1.Parent_Id = '" & EI_Id & "') ) ) ) AND " &
                            " dbo.tblEvaluationIndexLevel.Level_Id = '" & LevelId & "') AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1 "

        CheckResult = useCls.ExecuteScalar(Sql)
        If CInt(CheckResult) > 0 Then
            CheckCount = True
        Else
            CheckCount = False
        End If
        Return CheckCount
    End Function

    ''' <summary>
    ''' ทำการเช็คว่าดัชนีนี้ที่คำถามข้อนี้ มีการเลือกดัชนีนี้บ้างรึเปล่าถ้ามีการเลือกแสดงว่าต้องขึ้นรูปติ๊กถูกที่หัว Tab ด้วย
    ''' </summary>
    ''' <param name="IndexNameId">ParentId ของ tblEvaluationIndexNew ที่ต้องการหา</param>
    ''' <returns>Boolean:True=ต้องขึ้นรูปเครื่องหมายถูก,False=ไม่ต้องมีรุปเครื่องหมายถูก</returns>
    ''' <remarks></remarks>
    Private Function CheckAddImage(ByVal IndexNameId As String) As Boolean

        Dim Checkcount As Boolean = False
        Dim Questions_ID As String = Request.QueryString("qid")
        Dim sql As String = ""
        'เป็นตัวแปรที่เอาไว้เช็คว่าถ้ามีค่าเกิน 1 แสดงว่ามีการเลือก item ในดัชนีนั้นๆมาต้องแสดงรูปเครื่องหมายถูก
        Dim CheckResult As String = ""
        sql = " SELECT COUNT(*) FROM tblQuestionEvaluationIndexItem INNER JOIN " &
              " tblEvaluationIndexNew ON tblQuestionEvaluationIndexItem.EI_Id = tblEvaluationIndexNew.EI_Id " &
              " WHERE (tblQuestionEvaluationIndexItem.Question_Id = '" & Questions_ID & "') " &
              " AND (dbo.tblEvaluationIndexNew.EI_Id IN (SELECT e1.EI_Id FROM dbo.tblEvaluationIndexNew AS e1 " &
              " WHERE e1.Parent_Id IN (SELECT e2.EI_Id FROM dbo.tblEvaluationIndexNew " &
              " AS e2 WHERE e2.Parent_Id = '" & IndexNameId & "') ) ) AND dbo.tblQuestionEvaluationIndexItem.IsActive = 1 "
        CheckResult = useCls.ExecuteScalar(sql)
        If CInt(CheckResult) > 0 Then
            Checkcount = True
        Else
            Checkcount = False
        End If
        Return Checkcount

    End Function

    ''' <summary>
    ''' ทำการหา Item ระดับ รองลงมาจากหัว Tab ของแบบ ตัวชี้วัด เช่น สาระที่ ๓ เรขาคณิต , สาระที่ ๒ ภาษา และวัฒนธรรม ของวิชานี้
    ''' </summary>
    ''' <param name="EiId">Id ของ tblEvaluationIndex ของตัวชี้วัด</param>
    ''' <param name="GroupsubjectId">Id ของ tblGroupSubject ของวิชาที่ต้องการหา Item นี้</param>
    ''' <returns>Datatable:ของItem ระดับ สาระ ของตัวชี้วัด</returns>
    ''' <remarks></remarks>
    Private Function GetEvaluationGroupByEIIdNewEvaluation(ByVal EiId As String, ByVal GroupsubjectId As String) As DataTable
        Dim sql As String = " SELECT EI_Id,EI_Code,EI_Name FROM dbo.tblEvaluationIndexNew WHERE EI_Id IN ( " &
                            " SELECT es.EI_Id FROM dbo.tblEvaluationIndexSubject AS es WHERE es.Subject_Id = '" & GroupsubjectId & "' AND es.EI_Id IN ( " &
                            " SELECT e1.EI_Id FROM dbo.tblEvaluationIndexNew AS e1 WHERE e1.Parent_Id = '" & EiId & "') " &
                            " ) ORDER BY dbo.tblEvaluationIndexNew.EI_Position "
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการหา Item ระดับถัดไปรองมาจาก Item หัว Tab
    ''' </summary>
    ''' <param name="EiId">ParentId ของ tblEvaluationIndexNew</param>
    ''' <returns>Datatable:ของ Item ระดับลองมาจากหัว Tab</returns>
    ''' <remarks></remarks>
    Private Function GetEvaluationGroupByEIIdOldEvaluation(ByVal EiId As String) As DataTable
        Dim sql As String = " SELECT EI_Id,EI_Code,EI_Name,NeedSingleChoice FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EiId & "' AND IsActive = '1' ORDER BY EI_Position "
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการหา Item ระดับถัดไปรองมาจาก Item ระดับ สาระ ของตัวชี้วัด
    ''' </summary>
    ''' <param name="EiId">ParentId ของ tblEvaluation ของระดับ สาระ ของ ตัวชี้วัด</param>
    ''' <returns>Datatable:Item ระดับมาตรฐานของตัวชี้วัด</returns>
    ''' <remarks></remarks>
    Private Function GetEvaluationStandard(ByVal EiId As String) As DataTable
        Dim sql As String = " SELECT EI_Id,EI_Code,EI_Name,NeedSingleChoice FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EiId & "' " &
                            " AND IsActive = 1 ORDER BY EI_Position "
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการหา Item ระดับสุดท้ายของ ดัชนีชี้วัดแบบปกติ
    ''' </summary>
    ''' <param name="EiId">ParentId ของ tblEvaluationIndexNew</param>
    ''' <returns>Datatable:Item ระดับสุดท้าย</returns>
    ''' <remarks></remarks>
    Private Function GetdtLastGroup(ByVal EiId As String) As DataTable
        Dim sql As String
        sql = " SELECT ei.EI_Id,EI_Code + ' (' + cast(count(qei.Question_Id) as varchar(7)) + ')' as EI_Code  
                FROM dbo.tblEvaluationIndexNew ei left join tblQuestionEvaluationIndexItem qei on ei.EI_Id = qei.EI_Id and qei.IsActive = 1 
                WHERE Parent_Id = '" & EiId & "' AND ei.IsActive = '1' GROUP BY ei.EI_Id,EI_Code,EI_Position ORDER BY EI_Position"
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการหา Item สุดท้ายของ ตัวชี้วัด
    ''' </summary>
    ''' <param name="EiId">ParentId ของ tblEvaluationIndexNew</param>
    ''' <param name="LevelId">Id tblLevel ของชั้น</param>
    ''' <returns>Datatable:Item ระดับสุดท้ายของตัวชี้วัด</returns>
    ''' <remarks>
    ''' 2016/11/27 ไหมเพิ่มให้ Join tblQuestionEvalutionIndex
    ''' </remarks>
    Private Function GetlastGroupNewEvaluation(ByVal EiId As String, ByVal LevelId As String) As DataTable
        'Dim sql As String = " SELECT EI_Id,EI_Code,EI_Name FROM dbo.tblEvaluationIndexNew WHERE EI_Id IN ( " &
        '                    " SELECT EI_Id FROM dbo.tblEvaluationIndexLevel WHERE Level_Id = '" & LevelId & "' AND EI_Id IN ( " &
        '                    " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EiId & "' AND IsActive = 1 ) " &
        '                    " ) ORDER BY dbo.tblEvaluationIndexNew.EI_Position "

        Dim sql As String
        sql = "select en.EI_Id,en.EI_Code,en.EI_Name + '(' + cast(case when count(qei.Question_Id) is null then 0 else count(qei.Question_Id) end as varchar(7)) + ')' as EI_Name 
                from tblEvaluationIndexLevel el  inner join tblEvaluationIndexNew en on en.EI_Id = el.EI_Id  
                left join tblQuestionEvaluationIndexItem qei on en.EI_Id = qei.EI_Id and qei.IsActive = 1
                where el.Level_Id = '" & LevelId & "' and en.Parent_Id = '" & EiId & "' and en.IsActive = 1 and el.IsActive = 1
                group by en.EI_Id,en.EI_Code,en.EI_Name"
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการสร้างดัชนี แบบ ตัวชี้วัด ที่มีระดับ 4 ระดับ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateTabNewEvaluation(EIId As String)

        Dim QuestionId As String = Request.QueryString("qid")
        Dim SubjectId As String = GetSubjectIdByQuestionId(QuestionId)
        Dim LevelId As String = GetLevelIdByQuestionId(QuestionId)
        'เอาไว้เก็บ Id ของ Panel Item ระดับสุดท้ายของตัวชี้วัด เพื่อเอาไปใช้ตอน save
        Dim ArrNewEvaluationIndex As New ArrayList

        Dim sql As String = " SELECT EI_Id,EI_Code FROM dbo.tblEvaluationIndexNew WHERE EI_Id = '" & EIId & "' AND IsActive = '1' "
        Dim dt As New DataTable
        dt = useCls.getdata(sql)
        If dt.Rows.Count > 0 Then
            'Check ว่าคำถามข้อนี้มีการ Set ค่าตัวชี้วัดไปแล้วบ้างหรือยัง
            Dim CheckForAddImage = CheckAddImageNewEvaluation(dt.Rows(0)("EI_Id").ToString(), LevelId, SubjectId, QuestionId)
            Dim TotalPanel As Integer = 1
            Dim TotalStandard As Integer = 1
            Dim Totalitem As Integer = 1
            If CheckForAddImage = True Then
                'Check ว่า SuperUser Approve หรือยัง 
                'If IsConfirm(dt.Rows(0)("EI_Id").ToString()) = True Then 'ถ้า Approve แล้วเป็นเครื่องหมายถูก 2 อัน
                If IsConfirmNewEvaluationIndex(dt.Rows(0)("EI_Id").ToString(), LevelId, SubjectId) = True Then 'ถ้า Approve แล้วเป็นเครื่องหมายถูก 2 อัน
                    RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dt.Rows(0)("EI_Code"), .ImageUrl = "../Images/Right Tick/20120530-164512_Imagen_5asd.png"})
                Else 'ถ้ายังเป็นรูปเครื่องหมายถูกอันเดียว
                    RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dt.Rows(0)("EI_Code"), .ImageUrl = "../Images/Right Tick/tickIcon.gif"})
                End If
            Else 'ถ้าคำถามข้อนี้ยังไม่ได้ตั้งค่าตัวชี้วัด ไม่มีรูปอะไรขึ้น
                RadTabMainEvaluationIndex.Tabs.Add(New RadTab With {.Text = dt.Rows(0)("EI_Code")})
            End If
            Dim ThisPageView As New RadPageView
            ThisPageView.CssClass = "TestClass"
            RadAllMultiPage.PageViews.Add(ThisPageView)

            Dim MainNewEvaluationPanel As New Panel
            With MainNewEvaluationPanel
                .ID = "GroupMainNewEvaluationPanel" & TotalPanel
            End With

            ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'หาสาระทั้งหมดจากวิชานี้
            Dim dtGroupSubject As DataTable = GetEvaluationGroupByEIIdNewEvaluation(dt.Rows(0)("EI_Id").ToString(), SubjectId)
            If dtGroupSubject.Rows.Count > 0 Then
                'วนเพื่อสร้าง Panel Item สำหรับ สาระนี้ , เงื่อนไขการจบ loop คือ สร้างให้ครบทุกสาระที่อยู่ในวิชานี้
                For i = 0 To dtGroupSubject.Rows.Count - 1
                    'เช็คก่อนว่า สาระนี้มี Item ไหม ถ้าไม่มีต้องสร้าง Panel อะไรเลย
                    If CountChild(dtGroupSubject.Rows(i)("EI_Id").ToString(), LevelId, True) > 0 Then
                        Dim GroupSubjectPanel As New Panel
                        With GroupSubjectPanel
                            .Style.Add("padding-top", "20px")
                            .Style.Add("padding-left", "5px")
                            .Style.Add("background-color", "#2DCDFF")
                            .Style.Add("border-bottom", "solid")
                        End With
                        ''''Add Text ให้ Panel กลุ่มสาระก็ต่อเมื่อสาระนั้นมี Item จริง
                        Dim EachGroupSubjectCodeNamelabel As New Label
                        GroupSubjectPanel.ID = dtGroupSubject.Rows(i)("EI_Id").ToString()
                        EachGroupSubjectCodeNamelabel.Text = dtGroupSubject(i)("EI_Code") & "   " & dtGroupSubject(i)("EI_Name")
                        GroupSubjectPanel.Controls.Add(EachGroupSubjectCodeNamelabel)
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'หา "มาตรฐาน" จากสาระแต่ละอัน
                        Dim dtStandard As DataTable = GetEvaluationStandard(dtGroupSubject.Rows(i)("EI_Id").ToString())
                        If dtStandard.Rows.Count > 0 Then
                            'วนเพื่อสร้าง Item ระดับ มาตรฐาน , เงื่อนไขการจบ loop คือ วนให้ครบหมดทุกมาตรฐาน
                            For j = 0 To dtStandard.Rows.Count - 1
                                'เช็คว่า มาตรฐานนี้ มีItemไหม ถ้าไม่มีไม่ต้องสร้าง Panel มาตรฐานนี้
                                If CountChild(dtStandard.Rows(j)("EI_Id").ToString(), LevelId, False) Then
                                    Dim NeedSingleChoices As Integer = dtStandard.Rows(j)("NeedSingleChoice")
                                    Dim StandardPanel As New Panel
                                    With StandardPanel
                                        .Style.Add("padding-top", "20px")
                                        .Style.Add("padding-left", "10px")
                                        .Style.Add("padding-right", "5px")
                                        .Style.Add("background-color", "#2DCDFF")
                                    End With
                                    Dim GroupCheckBoxOrRadioPane As New Panel

                                    'เช็คว่าเป็น SuperUser หรือเปล่า
                                    If Session("SuperUser") = False Then
                                        'เช็คอีกว่า อนุมัติหรือยัง ถ้าอนุมัติแล้วต้องไม่ให้ checkbox,radio ติ๊กได้
                                        If IsConfirmNewEvaluationIndex(dt.Rows(0)("EI_Id").ToString(), LevelId, SubjectId) = True Then
                                            GroupCheckBoxOrRadioPane.Enabled = False
                                        End If
                                    End If

                                    With GroupCheckBoxOrRadioPane
                                        .ID = "StandardItem" & TotalStandard
                                        '.Style.Add("border-bottom", "solid")
                                        .Style.Add("padding-top", "10px")
                                        '.Style.Add("float", "left") '----------------------
                                        .Style.Add("padding-bottom", "20px")
                                        .Style.Add("background-color", "#2DCDFF")
                                        .Style.Add("width", "100%")
                                    End With
                                    ''''Add Text ให้ Panel มาตรฐานก็ต่อเมื่อมาตรฐานนั้นมี Item จริง
                                    StandardPanel.ID = dtStandard.Rows(j)("EI_Id").ToString()
                                    Dim EachStandardCodeNameLabel As New Label
                                    EachStandardCodeNameLabel.Text = dtStandard.Rows(j)("EI_Code") & "   " & dtStandard.Rows(j)("EI_Name")
                                    'Dim EditStandardBtn As New System.Web.UI.WebControls.Image
                                    'With EditStandardBtn
                                    '    .ID = "Edit" & dtStandard.Rows(j)("EI_Id").ToString()
                                    '    .Attributes.Add("EIID", dtStandard.Rows(j)("EI_Id").ToString())
                                    '    .Attributes.Add("Onclick", "EditEiName('" & dtStandard.Rows(j)("EI_Id").ToString() & "','" & dtStandard.Rows(j)("EI_Name") & "','True')")
                                    '    .ImageUrl = "../Images/freehand.png"
                                    '    .Style.Add("cursor", "pointer")
                                    'End With
                                    StandardPanel.Controls.Add(EachStandardCodeNameLabel)
                                    'StandardPanel.Controls.Add(EditStandardBtn)
                                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    'หา Item ของแต่ละมาตรฐาน
                                    Dim dtIndexItem As DataTable = GetlastGroupNewEvaluation(dtStandard.Rows(j)("EI_Id").ToString(), LevelId)
                                    If dtIndexItem.Rows.Count > 0 Then
                                        'check ว่าต้องสร้าง control แบบ Radio หรือเปล่า 
                                        If NeedSingleChoices = 0 Then
                                            'loop เพื่อสร้าง Item ระดับสุดท้ายของตัวชี้วัด แบบ checkbox , เงื่อนไขการจบ loop คือ วนสร้างให้ครบทุก Item ของ มาตรฐานนี้ 
                                            For k = 0 To dtIndexItem.Rows.Count - 1
                                                Dim chk As New CheckBox
                                                'Dim EditBtn As New System.Web.UI.WebControls.Image
                                                'With EditBtn
                                                '    .ID = "Edit" & dtIndexItem.Rows(k)("EI_Id").ToString()
                                                '    .Attributes.Add("EIID", dtIndexItem.Rows(k)("EI_Id").ToString())
                                                '    .Attributes.Add("Onclick", "EditEiName('" & dtIndexItem.Rows(k)("EI_Id").ToString() & "','" & dtIndexItem.Rows(k)("EI_Name") & "','True')")
                                                '    .ImageUrl = "../Images/freehand.png"
                                                '    .Style.Add("position", "relative")
                                                '    .Style.Add("top", "35px")
                                                '    .Style.Add("cursor", "pointer")
                                                'End With
                                                'GroupCheckBoxOrRadioPane.Controls.Add(EditBtn)
                                                Dim EachItemPanel As New Panel
                                                With EachItemPanel
                                                    .ID = "EachNewEvaIndexItem" & Totalitem
                                                    .Style.Add("padding-left", "20px")
                                                    .Style.Add("padding-right", "20px")
                                                    .Style.Add("padding-top", "10px")
                                                End With
                                                With chk
                                                    .Style.Add("margin-left", "20px")
                                                    .Style.Add("margin-top", "20px")
                                                    .InputAttributes.Add("onchange", "SetDirty();")
                                                    '.Style.Add("float", "left") '----------------------
                                                End With

                                                chk.ID = dtIndexItem.Rows(k)("EI_Id").ToString()
                                                chk.Text = " " & dtIndexItem.Rows(k)("EI_Code") & "  " & dtIndexItem.Rows(k)("EI_Name")
                                                EachItemPanel.Controls.Add(chk)
                                                'Add แต่ละ Checkbox ลงใน PanelGroupCheckboxOrRadio

                                                GroupCheckBoxOrRadioPane.Controls.Add(EachItemPanel)
                                                Totalitem += 1
                                                'AllArrayIndexItem.Add(EachItemPanel.ID)
                                                ArrNewEvaluationIndex.Add(EachItemPanel.ID)
                                            Next
                                        Else
                                            'loop เพื่อสร้าง Item ระดับสุดท้ายของตัวชี้วัด แบบ radio , เงื่อนไขการจบ loop คือ วนสร้างให้ครบทุก Item ของ มาตรฐานนี้ 
                                            For k = 0 To dtIndexItem.Rows.Count - 1
                                                Dim rdo As New RadioButton
                                                Dim EditBtn As New System.Web.UI.WebControls.Image
                                                'With EditBtn
                                                '    .ID = "Edit" & dtIndexItem.Rows(k)("EI_Id").ToString()
                                                '    .Attributes.Add("EIID", dtIndexItem.Rows(k)("EI_Id").ToString())
                                                '    .Attributes.Add("Onclick", "EditEiName('" & dtIndexItem.Rows(k)("EI_Id").ToString() & "','" & dtIndexItem.Rows(k)("EI_Name") & "','True')")
                                                '    .ImageUrl = "../Images/freehand.png"
                                                '    .Style.Add("position", "relative")
                                                '    .Style.Add("top", "35px")
                                                '    .Style.Add("cursor", "pointer")
                                                'End With
                                                'GroupCheckBoxOrRadioPane.Controls.Add(EditBtn)
                                                Dim EachItemPanel As New Panel
                                                With EachItemPanel
                                                    .ID = "EachNewEvaIndexItem" & Totalitem
                                                    .Style.Add("padding-left", "20px")
                                                    .Style.Add("padding-right", "20px")
                                                    .Style.Add("padding-top", "10px")
                                                End With
                                                rdo.ID = dtIndexItem.Rows(k)("EI_Id").ToString()
                                                rdo.Text = " " & dtIndexItem.Rows(k)("EI_Code") & "  " & dtIndexItem.Rows(k)("EI_Name")
                                                rdo.GroupName = dtStandard.Rows(j)("EI_Id").ToString()
                                                With rdo
                                                    .Style.Add("margin-left", "20px")
                                                    .Style.Add("margin-top", "20px")
                                                    .InputAttributes.Add("onchange", "SetDirty();")
                                                    '.Style.Add("float", "left") '----------------------
                                                End With

                                                EachItemPanel.Controls.Add(rdo)
                                                'Add แต่ละ Radio ลงใน PanelGroupCheckboxOrRadio

                                                GroupCheckBoxOrRadioPane.Controls.Add(EachItemPanel)
                                                Totalitem += 1
                                                'AllArrayIndexItem.Add(EachItemPanel.ID)
                                                ArrNewEvaluationIndex.Add(EachItemPanel.ID)
                                            Next
                                        End If
                                        'Add PanelCheckBox ของแต่ละมาตรฐานลงใน Panel มาตรฐาน
                                        StandardPanel.Controls.Add(GroupCheckBoxOrRadioPane)
                                        TotalStandard += 1
                                    End If
                                    'Add Panel มาตรฐานลงใน Panel กลุ่มสาระ
                                    GroupSubjectPanel.Controls.Add(StandardPanel)
                                End If
                            Next
                        End If
                        'Add Panel กลุ่มสาระลงใน Panel ใหญ่สุด
                        MainNewEvaluationPanel.Controls.Add(GroupSubjectPanel)
                    End If

                Next

                'Add Panel ใหญ่สุดลงใน Pageview
                ThisPageView.Controls.Add(MainNewEvaluationPanel)

                'สร้างปุ่มสำหรับ Save โดยตรวจก่อนว่าสร้างปุ่มแบบ อนุมัติได้หรือไม่ได้
                If Session("SuperUser") = True Then
                    Dim newBtn As New Button
                    Dim NewPanel As New Panel
                    With newBtn
                        .ID = "NewEva"
                        .Text = "ยืนยันการบันทึก [" & dt.Rows(0)("EI_Code") & "]"
                        .Style.Add("float", "right")
                        .Attributes("EI_Id") = dt.Rows(0)("EI_Id").ToString
                        .Attributes("NewEva") = "True"
                        .Attributes("IsConfirm") = "True"
                    End With
                    AddHandler newBtn.Click, AddressOf ConfirmSave
                    NewPanel.Controls.Add(newBtn)
                    ThisPageView.Controls.Add(NewPanel)

                Else
                    If IsConfirmNewEvaluationIndex(dt.Rows(0)("EI_Id").ToString(), LevelId, SubjectId) = False Then
                        Dim NewPanel As New Panel
                        Dim newBtn As New Button
                        With newBtn
                            .ID = "NewEva"
                            .Text = "บันทึก [" & dt(0)("EI_Code") & "]"
                            .Style.Add("float", "right")
                            .Attributes("EI_Id") = dt.Rows(0)("EI_Id").ToString
                            .Attributes("NewEva") = "True"
                            .Attributes("IsConfirm") = "False"
                        End With
                        AddHandler newBtn.Click, AddressOf NormalSave
                        NewPanel.Controls.Add(newBtn)
                        ThisPageView.Controls.Add(NewPanel)
                    End If
                End If

                Dim ArrayMainGroup As New ArrayList
                ArrayMainGroup.Add(MainNewEvaluationPanel.ID)
                'Session("MainPanel") = ArrayMainGroup
                Session("Arr" & dt.Rows(0)("EI_Id").ToString) = ArrNewEvaluationIndex
            End If


        End If

    End Sub

    ''' <summary>
    ''' check ดัชนี้ สาระ ที่กำลังสร้างอยู่นั้น มี Item อะไรหรือเปล่า 
    ''' </summary>
    ''' <param name="EiId">ParentId ของ tblEvaluationIndexNew ตัวชี้วัด</param>
    ''' <param name="InputLevelId">Id ของ ชั้น</param>
    ''' <param name="IsLevel1">เป็นระดับที่รองลงมาจาก ตัวชี้วัดเลยรึเปล่า ?</param>
    ''' <returns>Integer:จำนวน Item</returns>
    ''' <remarks></remarks>
    Private Function CountChild(ByVal EiId As String, ByVal InputLevelId As String, ByVal IsLevel1 As Boolean) As Integer
        Dim sql As String = ""
        If IsLevel1 = True Then
            sql = " SELECT COUNT(*) FROM dbo.tblEvaluationIndexLevel WHERE Level_Id = '" & InputLevelId & "' AND EI_Id IN ( " &
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " &
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EiId & "')) "
        Else
            sql = " SELECT COUNT(*) FROM dbo.tblEvaluationIndexLevel WHERE Level_Id = '" & InputLevelId & "' AND EI_Id IN ( " &
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EiId & "') "
        End If
        Dim CountReturn As Integer = 0
        CountReturn = useCls.ExecuteScalar(sql)
        Return CInt(CountReturn)
    End Function

    Private Function CheckUserPermission()
        Dim sql As String = ""
        sql = "select Username from tbluser where guid = '" & Session("UserId").ToString & "';"

        Dim userName As String = useCls.ExecuteScalar(sql)

        If userName.Substring(0, 7).ToLower = "approve" Then
            btnApproveEvalution.Visible = True
        End If

        If userName.Substring(0, 7).ToLower = "proof" Then
            btnApproveEvalution.Visible = False
            btnAddNewEvalution.Visible = False
        End If

    End Function


    ''' <summary>
    ''' Function ที่จะทำการ update เนื้อข้อมูลสำหรับ ตัวชี้วัด เมื่อกดจากปุ่มดินสอ
    ''' </summary>
    ''' <param name="EI_Id">Id ของ Item ที่ต้องการแก้ไข</param>
    ''' <param name="EI_Name">ข้อความที่แก้ไขมา</param>
    ''' <returns>String:-1=ไม่มี session,"" ไม่สำเร็จ , Path ของหน้านี้ + Querystring ที่ assing ไว้ตอน pageload = สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveEditEIName(ByVal EI_Id As String, ByVal EI_Name As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If
        Dim _DB As New ClassConnectSql()
        EI_Name = EI_Name.Replace(vbLf, " ").Replace(vbCrLf, "").Replace(vbNewLine, "")
        Dim sql As String = " UPDATE dbo.tblEvaluationIndexNew SET EI_Name = '" & _DB.CleanString(EI_Name) & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE EI_Id = '" & EI_Id & "' "
        Try
            _DB.Execute(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

        Dim LogStr As String = "แก้ไข EI_Name ที่ EI_Id '" & EI_Id & "' "


        Dim ReturnValue As String = ""
        If HttpContext.Current.Session("ForEditEIName") IsNot Nothing Then
            ReturnValue = HttpContext.Current.Session("ForEditEIName")
            HttpContext.Current.Session("ForEditEIName") = Nothing
        Else
            ReturnValue = ""
        End If

        Log.Record(Log.LogType.ManageExam, _DB.CleanString(LogStr), True)

        Return ReturnValue

    End Function

    <Services.WebMethod()>
    Public Shared Function SaveLogWhenSelectedTab(ByVal EI_Name As String)
        Try
            Log.Record(Log.LogType.ManageExam, "เข้า Tab ดัชนี " & EI_Name, True)
            Return "Complete"
        Catch ex As Exception
            Return "-1"
        End Try
    End Function

    ''' <summary>
    ''' Function ที่จะทำการ update เนื้อข้อมูลสำหรับ ดัชนีแบบปกติ เมื่อกดจากปุ่มดินสอ
    ''' </summary>
    ''' <param name="EI_Id">Id ของ Item ที่ต้องการแก้ไข</param>
    ''' <param name="EI_Code">ข้อความที่แก้ไขมา</param>
    ''' <returns>String:-1=ไม่มี session,"" ไม่สำเร็จ , Path ของหน้านี้ + Querystring ที่ assing ไว้ตอน pageload = สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveEditEICode(ByVal EI_Id As String, ByVal EI_Code As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If
        Dim _DB As New ClassConnectSql()
        EI_Code = EI_Code.Replace(vbLf, " ").Replace(vbCrLf, "").Replace(vbNewLine, "")
        Dim sql As String = " UPDATE dbo.tblEvaluationIndexNew SET EI_Code = '" & _DB.CleanString(EI_Code) & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE EI_Id = '" & EI_Id & "' "
        Try
            _DB.Execute(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try

        Dim LogStr As String = "แก้ไข EI_Code ที่ EI_Id '" & EI_Id & "' "
        Log.Record(Log.LogType.ManageExam, _DB.CleanString(LogStr), True)
        Dim ReturnValue As String = ""
        If HttpContext.Current.Session("ForEditEIName") IsNot Nothing Then
            ReturnValue = HttpContext.Current.Session("ForEditEIName")
            HttpContext.Current.Session("ForEditEIName") = Nothing
        Else
            ReturnValue = ""
        End If

        Return ReturnValue

    End Function

    <Services.WebMethod()>
    Public Shared Function PrintEvalution(ByVal EI_Id As String)
        Dim Questions_ID As String = HttpContext.Current.Session("QuestionId").ToString
        Dim clt As New ClsTestSet(HttpContext.Current.Session("UserId").ToString)
        Try
            clt.ExportEvalution(Questions_ID, False)
            Return "Complete"
        Catch ex As Exception
            Dim a As String = ex.ToString
        End Try

    End Function

End Class