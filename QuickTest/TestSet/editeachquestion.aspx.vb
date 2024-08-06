Imports Telerik.Web.UI
Imports System.IO
Imports System.Web.Services
Imports System.Web

Public Class editeachquestion
    Inherits System.Web.UI.Page
    'ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim ClsData As New ClassConnectSql
    'datatable เก็บคำถาม
    Dim dtQuestion As New DataTable
    'datatable เก็บคำตอบ ใช้ทั้งหน้า
    Dim dtAnswer As New DataTable
    'Dim dtQsetType As New DataTable
    'Class ที่ใช้เกี่ยวกับการ Gen Path ของรูปภาพ , หาข้อมูลข้อสอบต่างๆ ใช้ทั้งหน้า
    Dim pdfCls As New ClsPDF(New ClassConnectSql)
    'ตัวแปรเลขนำหน้าคำตอบ ใช้ทั้งหน้า
    Dim NumChoice As Integer = 0
    'เก็บ QuestionId เอาไปใช้ในหน้า Javascript ด้วย
    Public VBQuestionId As String
    'เก็บ QsetId เอาไปใช้ในหน้า Javascript ด้วย
    Public VBQsetId As String
    'เก็บ QsetType เอาไปใช้ในหน้า Javascript ด้วย
    Public VBQsetType As String
    'ตัวแปร check เรื่อง Validate ใช้ตอนกด save
    Dim CheckValidate As Boolean
    'ตัวแปรที่เอาไว้เก็บ Log ของการแก้ไขคำถาม กันหายหลังจาก PostBack เลยเก็บไว้ใน ViewState 
    Public SubjectId As String
    'อนุญาต Copy Paste ข้อความหรือไม่
    Public AllowPasteHtml As String

    'QsetId ของคำถามข้อที่กำลังแก้ไขนี้
    Private Property QsetId As String
        Get
            Return ViewState("_QsetId")
        End Get
        Set(value As String)
            ViewState("_QsetId") = value
        End Set
    End Property

    'QsetType ใช้เพื่อตรวจสอบการ Render และการ Save ข้อสอบลง DB
    Private Property QsetType As String
        Get
            Return ViewState("QsetType")
        End Get
        Set(value As String)
            ViewState("QsetType") = value
        End Set
    End Property

    'ใช้เก็บ String Log คำถาม
    Private Property LogQuestion As String
        Get
            LogQuestion = ViewState("_LogQuestion")
        End Get
        Set(ByVal value As String)
            ViewState("_LogQuestion") = value
        End Set
    End Property

    'เก็บคำถามก่อนจะทำการแก้ไข เพื่อตรวจสอบว่าได้เข้ามาทำการแก้ไขคำถามหรือไม่
    Private Property QuestionNameBeforeEdit As String
        Get
            QuestionNameBeforeEdit = ViewState("_QuestionNameBeforeEdit")
        End Get
        Set(ByVal value As String)
            ViewState("_QuestionNameBeforeEdit") = value
        End Set
    End Property

    'เก็บคำอธิบายคำถามก่อนจะทำการแก้ไข เพื่อตรวจสอบว่าได้เข้ามาทำการแก้ไขคำอธิบายคำถามหรือไม่
    Private Property QuestionExplainBeforeEdit As String
        Get
            QuestionExplainBeforeEdit = ViewState("_QuestionExplainBeforeEdit")
        End Get
        Set(ByVal value As String)
            ViewState("_QuestionExplainBeforeEdit") = value
        End Set
    End Property

    'เก็บสถานะว่าได้ทำการ Update ข้อสอบหรือไม่ -- ถ้าไม่มีอะไร Update ให้เก็บ Log เป็นเข้ามายืนยันข้อสอบ
    Private Property IsEditQuestion As Boolean
        Get
            IsEditQuestion = ViewState("_IsEditQuestion")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_IsEditQuestion") = value
        End Set
    End Property

    'ใช้เก็บ String Log คำตอบ
    Private Property LogAnswer As String
        Get
            LogAnswer = ViewState("_LogAnswer")
        End Get
        Set(ByVal value As String)
            ViewState("_LogAnswer") = value
        End Set
    End Property

    'ใช้ตรวจสอบการกดเปิดเครื่องมือพิเศษ -- ปุ่มสระ
    Private Property CheckOpenTool As Boolean
        Get
            CheckOpenTool = ViewState("_CheckOpenTool")
        End Get
        Set(ByVal value As Boolean)
            ViewState("_CheckOpenTool") = value
        End Set
    End Property


    'ขนาดไฟล์ที่ให้ upload ได้สูงสุดคือ 2 GB
    Const maxFileSize As Integer = 2048000

    ''' <summary>
    ''' ทำการหาข้อมูล คำถาม-คำตอบ แล้ว Bind เข้า Editor แบ่งตามประเภทของคำถาม
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            VBQuestionId = Request.QueryString("qid").ToString()

            VBQsetId = Request.QueryString("QsetId").ToString()
            QsetId = Request.QueryString("QSetId")

            GetQSetDetail()

            If Not Page.IsPostBack Then
                CheckOpenTool = False
                IsEditQuestion = False

                CreateFolderForImage()
                BindExam()
            End If

            AllowPasteHtml = ConfigurationManager.AppSettings("AllowPasteHtml")
        End If
    End Sub

    ''' <summary>
    ''' ตรวจสอบข้อมูลและเก็บค่า QSet
    ''' </summary>
    ''' <returns></returns>
    Private Function GetQSetDetail() As Boolean

        Dim sqlQSettype As String = "Select QSet_Type,QSet_IsRandomAnswer From tblQuestionSet Where QSet_Id = '" & QsetId & "'"

        Dim dtQSet As DataTable = ClsData.getdata(sqlQSettype)

        Dim QsetIsRandomAnswer As String

        If dtQSet.Rows.Count > 0 Then
            QsetType = dtQSet.Rows(0)("QSet_Type")
            QsetIsRandomAnswer = dtQSet.Rows(0)("QSet_IsRandomAnswer")
            If QsetIsRandomAnswer = "False" Then
                chkNotAllowShuffleAnswer.Checked = True
                chkNotAllowShuffleAnswer.Enabled = False

                QsetType = CInt(QsetType)
                VBQsetType = QsetType
                Return True
            End If
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' Function ที่เช็คเพื่อที่จะสร้าง Folder เก็บไฟล์รูปภาพของ QsetId นี้ สร้างในกรณีที่ยังไม่มี Folder นี้มาก่อน
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateFolderForImage()
        'Dim FullPath As String
        Dim QuestionSetId As String = Request.QueryString("QSetId")
        Dim PathProJect As String = Server.MapPath("../file/")  ' "D:\Development\QuickTest\Source\QuickTest\QuickTest\File" + "\"
        'array ที่เอาไว้เก็บ QsetId หลักต่างๆหลังจากที่ตัดมาทีละหลัก
        Dim ArrayCreateFolder As New ArrayList
        ArrayCreateFolder.Add(QuestionSetId.Substring(0, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(1, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(2, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(3, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(4, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(5, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(6, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(7, 1))
        ArrayCreateFolder.Add("{" & QuestionSetId & "}")
        Dim path As String = PathProJect
        'ทำการ loop เพื่อต่อสตริง ให้เป็น Path เพื่อจะนำ Path นี้ไปเช็คว่ามี Folder หรือยัง ถ้ายังไม่มีก็จะต้องทำการสร้าง Folder นี้ขึ้นมา , เงื่อนไขการจบ loop คือ วนจนครบ Array เพื่อต่อสตริง Path
        For Each i In ArrayCreateFolder
            path &= i & "\"
            'ตัวแปรที่ไปเช็ค Folder ว่ามี folder เกิดขึ้นมาหรือยัง
            Dim CreateFolder As New DirectoryInfo(path)
            If Not CreateFolder.Exists Then
                CreateFolder.Create()
            End If
        Next
    End Sub

    Private Sub BindExam()
        If QsetType = 2 Then
            BindTrueFalseExam()
        Else
            GetDataQuestion()
            GetDataAnswer()
            If QsetType = 1 Then
                ShowAllCheckbox()
                GetDataTickCheckbox()
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Bind ข้อมูลในคำถามแบบ ถูก-ผิด
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindTrueFalseExam()
        'ทำการซ่อน Repeater สำหรับโหมดอื่นขึ้นมา
        RptCreateAnswer.Visible = False
        lblAnswerExplain.Visible = False
        btnAddNewAnswer.Visible = False

        'ทำการ แสดง radio ถูกผิดขึ้นมา
        DivRadio.Visible = True

        GetDataQuestion()

        'Set Path รูปภาพ
        Dim FullPath As String = GetFullPath().Replace("\", "/")
        Dim fileLocationPath() As String = {"~/File" & FullPath}

        'ทำการ set Path ที่เกี่ยวข้องกับการ upload,import ไฟล์รูปภาพ ให้กับ Editor
        RadAnswerExplainTrue.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
        RadAnswerExplainTrue.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
        RadAnswerExplainTrue.ImageManager.MaxUploadFileSize = maxFileSize

        RadAnswerExplainFalse.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
        RadAnswerExplainFalse.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
        RadAnswerExplainFalse.ImageManager.MaxUploadFileSize = maxFileSize

        Dim sqlAnswer As String = "select Answer_Id,Answer_Name,Answer_Score,Answer_Expain from tblAnswer where Question_Id ='" & VBQuestionId & "' and IsActive = 1 order by Answer_No"

        Dim dtAnswer As DataTable
        dtAnswer = ClsData.getdata(sqlAnswer)

        Dim AnswerExplain As String

        'loop เพื่อดึงคำอธิบายคำถามมา Bind ใน Editor , เงื่อนไขการจบ loop คือ วนจนครบคำตอบ
        For CountLoop = 0 To dtAnswer.Rows.Count - 1

            If dtAnswer(CountLoop)("Answer_Expain") IsNot DBNull.Value Then
                AnswerExplain = dtAnswer(CountLoop)("Answer_Expain").ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
            Else
                AnswerExplain = ""
            End If

            If dtAnswer.Rows(CountLoop)("Answer_Name").ToString.ToLower > "true" Then
                RadAnswerExplainTrue.Content = AnswerExplain
                If dtAnswer.Rows(CountLoop)("Answer_Score") > 0 Then
                    RadioTrue.Checked = True
                Else
                    RadioFalse.Checked = True
                End If
            Else
                RadAnswerExplainFalse.Content = AnswerExplain
            End If
        Next

    End Sub

    ''' <summary>
    ''' ทำการต่อสตริงสร้าง Path ที่เอาไว้เก็บรูปภาพโดยแยกตามแต่ละ QsetId 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFullPath() As String
        Dim FullPath As String
        Dim QuestionSetId As String = Request.QueryString("QSetId")
        Dim PathProJect As String = Server.MapPath("../")  '"D:\Development\QuickTest\Source\QuickTest\QuickTest" + "\"
        'ทำการสร้าง Array เพื่อมาเก็บ QsetId ที่ตัดมาทีละหลักจำนวนทั้งหมด 8 หลัก แล้วต่อด้วย QsetId เต็มๆอีกทีนึง
        Dim ArrayCreateFolder As New ArrayList
        ArrayCreateFolder.Add(QuestionSetId.Substring(0, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(1, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(2, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(3, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(4, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(5, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(6, 1))
        ArrayCreateFolder.Add(QuestionSetId.Substring(7, 1))
        ArrayCreateFolder.Add("{" & QuestionSetId & "}")
        Dim path As String = PathProJect
        'loop เพื่อทำการต่อสตริง "\" เข้าไปหลังจาก QsetId ที่ตัดมาทีละหลัก , เงื่อนไขการจบ loop วนจนครบ Array คือ 9 รอบ
        For Each i In ArrayCreateFolder
            path &= i & "\"
        Next
        'Dim BackSlash As String = "\"
        'Dim Slash As String = "/"
        'Dim ReplaceSlash As String = Replace(path, BackSlash, Slash)
        Dim ReplaceSlash As String = path
        Dim RootUrl As String = "\"
        'ดึง Path ปัจจุบันขึ้นมา เช่น "D:/Development/QuickTest/Source/QuickTest/QuickTest/"
        Dim PathNotUse As String = Server.MapPath("../")
        'ทำการนำมา Replace โดยตัดส่วนหน้าทั้งหมดทิ้งให้เหลือแต่ QsetId ที่เริ่มจากหลักแรกมา เช่น \9\5\6\8\f\6\3\7\{9568f637-3230-47b4-81e1-a04e45b75132}\
        Dim CompleteFullPath As String = Replace(ReplaceSlash, PathNotUse, RootUrl)
        'สุดท้ายตัด \ ตัวสุดท้ายออกไป
        FullPath = CompleteFullPath.Remove(CompleteFullPath.Length - 1, 1)
        Return FullPath
    End Function

    ''' <summary>
    ''' ทำการหาข้อมูลของคำถาม โดยใช้ QSetid , QuestionId ที่ได้มาจาก Querystring เป็นเงื่อนไขในการ Where
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDataQuestion()


        Dim sqlQuestion As String = ""
        'ดูก่อนว่า QsetType ไม่ใช่เรียงลำดับใช่ไหม

        If QsetType <> 6 Then
            sqlQuestion = "select Question_Name,Question_No,Question_Expain from tblQuestion where Question_Id ='" & VBQuestionId & "' AND IsActive = 1 "
        Else
            'ถ้าเป็นเรียงลำดับต้องไปเอาคำถามมาจาก QsetName แทน QuestionName
            sqlQuestion = " SELECT QSet_Name AS Question_Name,Question_Expain FROM dbo.tblQuestionset INNER JOIN dbo.tblQuestion ON dbo.tblQuestionset.QSet_Id = dbo.tblQuestion.QSet_Id " &
                          " WHERE QSet_Type = 6 And dbo.tblQuestionset.IsActive = 1 And dbo.tblQuestion.IsActive = 1 AND dbo.tblQuestion.Question_Id = '" & VBQuestionId & "'; "
        End If

        dtQuestion = New DataTable
        dtQuestion = ClsData.getdata(sqlQuestion)

        If dtQuestion.Rows.Count > 0 Then
            'เนื้อคำถาม
            Dim QuestionName As String = dtQuestion.Rows(0)("Question_Name")

            'เนื้อคำอธิบายคำถาม
            Dim QuestionExplain As String
            If dtQuestion.Rows(0)("Question_Expain") IsNot DBNull.Value Then
                QuestionExplain = dtQuestion.Rows(0)("Question_Expain")
            Else
                QuestionExplain = ""
            End If

            'เนื้อคำถามหลังจากผ่านกระบวนการแปลง Path รูปให้ถูกต้อง
            Dim QuestionNameComplete As String = QuestionName.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
            'เนื้อคำอธิบายถามหลังจากผ่านกระบวนการแปลง Path รูปให้ถูกต้อง
            Dim QuestionExplainComplete As String = QuestionExplain.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
            'ทำการ Bind ข้อมูลคำถามใส่ Editor
            RadQuestion.Content = QuestionNameComplete
            RadQuestionExplain.Content = QuestionExplainComplete

            'เก็บคำถามไว้ใน QuestionNameBeforeEdit เพื่อทำการเช็คว่าได้มีการแก้ไขคำถามหรือเปล่า
            QuestionNameBeforeEdit = QuestionNameComplete
            QuestionExplainBeforeEdit = QuestionExplainBeforeEdit


            'หา Path เพื่อที่จะทำการเปิดให้ User Upload รูปภาพเข้ามาในชุดคำถามนี้ได้
            Dim FullPath As String = GetFullPath().Replace("\", "/")
            Dim fileLocationPath() As String = {"~/File" & FullPath}

            'assign Path ที่เกี่ยวข้องกับการ upload ไฟล์รูปภาพ หรือการนำรูปภาพเข้ามาใช้งาน ให้กับ Editor
            RadQuestion.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadQuestion.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadQuestion.ImageManager.DeletePaths = fileLocationPath
            RadQuestion.ImageManager.MaxUploadFileSize = maxFileSize

            RadQuestionExplain.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadQuestionExplain.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadQuestionExplain.ImageManager.DeletePaths = fileLocationPath
            RadQuestionExplain.ImageManager.MaxUploadFileSize = maxFileSize

            AddMultiControl(VBQuestionId, True, False)

            sqlQuestion = "select top 1 MFileExplain from tblmultimediaobject where ReferenceId = '" & VBQuestionId & "' 
                            and ReferenceType = 1 and MFileLevel = 1 and isActive = 1 order by lastupdate desc"

            Dim qMFiletxt As String
            qMFiletxt = ClsData.ExecuteScalar(sqlQuestion)

            If qMFiletxt <> "" Then
                Dim QMComplete As String = qMFiletxt.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
                'ทำการ Bind ข้อมูลคำถามใส่ Editor
                RadQMultiTxt.Content = QMComplete
                RadQMultiTxt.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
                RadQMultiTxt.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
                RadQMultiTxt.ImageManager.DeletePaths = fileLocationPath
                RadQMultiTxt.ImageManager.MaxUploadFileSize = maxFileSize
            End If

            AddMultiControl(VBQuestionId, True, True)

            sqlQuestion = "select top 1 MFileExplain from tblmultimediaobject where ReferenceId = '" & VBQuestionId & "' 
                            and ReferenceType = 2 and MFileLevel = 1 and isActive = 1 order by lastupdate desc"

            Dim QEMFiletxt As String
            QEMFiletxt = ClsData.ExecuteScalar(sqlQuestion)

            If QEMFiletxt <> "" Then
                Dim QEMComplete As String = qMFiletxt.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QsetId))
                'ทำการ Bind ข้อมูลคำถามใส่ Editor
                RadQMultiExplainTxt.Content = QEMComplete
                RadQMultiExplainTxt.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
                RadQMultiExplainTxt.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
                RadQMultiExplainTxt.ImageManager.DeletePaths = fileLocationPath
                RadQMultiExplainTxt.ImageManager.MaxUploadFileSize = maxFileSize
            End If

        End If

    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลของคำตอบ โดยหาจาก QsetId , QuestionId ที่ได้มาจาก Querystring 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDataAnswer()
        Dim QuestionId As String = Request.QueryString("qid")
        Dim QuestionSetId As String = Request.QueryString("QSetId")
        Dim sqlAnswer As String = ""
        'If Request.QueryString("AddNew") IsNot Nothing Then
        '    sqlAnswer = " Select Answer_Id, Answer_Name, Answer_No, Answer_Score, Answer_Expain from tblAnswer where Question_Id ='" & QuestionId & "' AND IsActive = 1  ORDER BY Answer_No desc "
        'Else
        'ถ้าเกิดว่าคำถามข้อนี้ไม่ใช่ Type 6 ก็หาข้อมูลคำตอบจาก tblAnswer ปกติ
        If QsetType <> 6 Then
            sqlAnswer = " select Answer_Id,Answer_Name,Answer_No,Answer_Score,Answer_Expain from tblAnswer where Question_Id ='" & QuestionId & "' AND IsActive = 1  ORDER BY Answer_No "
        Else
            'ถ้าเกิดเป็น Type 6 ต้องเปลี่ยนวิธีการ Select หมดเลย โดยหาเนื้อคำตอบจาก tblQuestion.QuestionName,Question_Expain แทน และตรวจคำตอบกับ AnswerName แทน
            sqlAnswer = " SELECT dbo.tblQuestion.Question_Id,Question_No AS Answer_No,Question_Name,Answer_Id,Answer_Expain FROM dbo.tblQuestion INNER JOIN dbo.tblAnswer ON dbo.tblQuestion.Question_Id = dbo.tblAnswer.Question_Id " &
                        " WHERE dbo.tblQuestion.QSet_Id = '" & QsetId & "' AND dbo.tblQuestion.IsActive = 1 AND dbo.tblAnswer.IsActive = 1  ORDER BY Question_No "
        End If
        'End If
        dtAnswer = New DataTable
        dtAnswer = ClsData.getdata(sqlAnswer)
        RptCreateAnswer.DataSource = dtAnswer
        RptCreateAnswer.DataBind()

        For Each i In dtAnswer.Rows()
            AddMultiControl(i("Answer_Id").ToString, False, False)
            AddMultiControl(i("Answer_Id").ToString, False, True)
        Next
    End Sub

    Private Sub AddMultiControl(ReferenceId As String, isQ As Boolean, IsExplain As Boolean)
        Dim FullPath As String = GetFullPath().Replace("\", "/")
        Dim sql As String = ""
        '---Normal File
        If IsExplain Then
            sql = "select MfileName from tblMultimediaObject where ReferenceId = '" & ReferenceId & "' and isactive = 1 and ReferenceType = '2' and MFileLevel = '1'; "
        Else
            sql = "select MfileName from tblMultimediaObject where ReferenceId = '" & ReferenceId & "' and isactive = 1 and ReferenceType = '1' and MFileLevel = '1'; "
        End If


        Dim dtMulti As DataTable
        dtMulti = ClsData.getdata(sql)

        Dim FPath As String
        If dtMulti.Rows.Count <> 0 Then
            For Each i In dtMulti.Rows
                FPath = "../file" & FullPath & "/" & i("MfileName").ToString

                If Not isQ Then
                    If IsExplain Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divAMultiExplain" & ReferenceId & "','" & ReferenceId & "');", True)
                    Else
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divAMulti" & ReferenceId & "','" & ReferenceId & "');", True)
                    End If
                Else
                    If IsExplain Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divQMultiExplain','" & ReferenceId & "');", True)
                    Else
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divQMulti','" & ReferenceId & "');", True)
                    End If

                End If
            Next
        End If
        '---Slow File
        If IsExplain Then
            sql = "select MfileName from tblMultimediaObject where ReferenceId = '" & ReferenceId & "' and isactive = 1 and ReferenceType = '2' and MFileLevel = '2'; "
        Else
            sql = "select MfileName from tblMultimediaObject where ReferenceId = '" & ReferenceId & "' and isactive = 1 and ReferenceType = '1' and MFileLevel = '2'; "
        End If


        Dim dtMultiSlow As DataTable
        dtMultiSlow = ClsData.getdata(sql)

        If dtMultiSlow.Rows.Count <> 0 Then
            For Each i In dtMultiSlow.Rows
                FPath = "../file" & FullPath & "/" & i("MfileName").ToString

                If Not isQ Then
                    If IsExplain Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divAMultiExplainSlow" & ReferenceId & "','" & ReferenceId & "');", True)
                    Else
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divAMultiSlow" & ReferenceId & "','" & ReferenceId & "');", True)
                    End If
                Else
                    If IsExplain Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divQMultiExplainSlow','" & ReferenceId & "');", True)
                    Else
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, GetType(Page), "Script" & i("MfileName").replace(".mp3", "").ToString & ReferenceId, "setmu(""" & i("MfileName").ToString & """,""" & i("MfileName").replace(".mp3", "").ToString & """,'" & FPath & "','divQMultiSlow','" & ReferenceId & "');", True)
                    End If

                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลคำตอบแล้วเอามา Bind ใน RepaterAnswer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RptCreateAnswer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RptCreateAnswer.ItemDataBound
        Dim QuestionSetId As String = QsetId 'Request.QueryString("QSetId")
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'ทำการประกาศตัวแปร checkbox ที่ไว้ใช้เลือกว่าคำตอบข้อนี้เป็นข้อที่ถูกต้อง
            Dim CheckBox As CheckBox = CType(e.Item.FindControl("CheckScore"), CheckBox)
            Dim txtScore As TextBox = CType(e.Item.FindControl("txtScore"), TextBox)
            If QsetType <> 6 Then
                'ทำการหาคะแนนจากว่าคำตอบข้อนี้เป็นข้อที่ถูกต้องหรือไม่
                Dim ScoreAnswer As Integer = DirectCast(e.Item.DataItem, System.Data.DataRowView).Row.ItemArray(3)
                If ScoreAnswer > 0 Then
                    'ถ้าถูกต้องก็ทำการให้ checkbox เป็น checked
                    CheckBox.Checked = True
                    txtScore.Text = ScoreAnswer
                End If
            Else 'ถ้าข้อสอบเป็น Type 6 ให้ซ่อน checkbox และ label "เลือกให้เป็นคำตอบที่ถูกต้อง"
                Dim radAnswer As RadEditor = e.Item.FindControl("RadAnswer")
                radAnswer.Attributes.Add("questionId", e.Item.DataItem("Question_Id").ToString())
                Dim lbl As Label = CType(e.Item.FindControl("lblChooseThisAnswerIsCorrect"), Label)
                lbl.Visible = False
                CheckBox.Visible = False
            End If
            'คำอธิบายคำตอบ
            Dim DataAnswerExplain As String = ""
            If DirectCast(e.Item.DataItem, System.Data.DataRowView).Row.ItemArray(4) IsNot DBNull.Value Then
                DataAnswerExplain = DirectCast(e.Item.DataItem, System.Data.DataRowView).Row.ItemArray(4)
            End If
            'คำตอบ
            Dim DataAnswerName As String = ""

            If QsetType <> 6 Then
                DataAnswerName = DirectCast(e.Item.DataItem, System.Data.DataRowView).Row.ItemArray(1)
            Else 'ถ้าเป็น Type6 ต้องดึง QuestionName มาเป็นคำตอบแทน
                DataAnswerName = e.Item.DataItem("Question_Name")
            End If

            'ทำการนำข้อมูลเข้าไป Replace Path รูป
            Dim AnswerName As String = DataAnswerName.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QuestionSetId))
            Dim AnswerExplain As String = DataAnswerExplain.ToString.Replace("___MODULE_URL___", pdfCls.GenFilePath(QuestionSetId))
            Dim RadAnswerName As RadEditor = CType(e.Item.FindControl("RadAnswer"), RadEditor)
            Dim RadAnswerExplain As RadEditor = CType(e.Item.FindControl("RadAnswerExplain"), RadEditor)

            RadAnswerName.Modules.Add(New Telerik.Web.UI.EditorModule With {.Name = "RadEditorNodeInspector"})
            RadAnswerExplain.Modules.Add(New Telerik.Web.UI.EditorModule With {.Name = "RadEditorNodeInspector"})

            'นำข้อมูลมา Bind ใส่ Editor
            RadAnswerName.Content = AnswerName.ToString
            RadAnswerExplain.Content = AnswerExplain.ToString

            'ทำการ set Path ที่เกี่ยวข้องกับรูปภาพให้กับ Editor เพื่อใช้ Upload , Import ไฟล์รูปภาพในเนื้อข้อมูลได้
            Dim FullPath As String = GetFullPath().Replace("\", "/")
            Dim fileLocationPath() As String = {"~/File" & FullPath}
            RadAnswerName.ImageManager.ViewPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadAnswerName.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadAnswerName.ImageManager.MaxUploadFileSize = maxFileSize

            RadAnswerExplain.ImageManager.ViewPaths = fileLocationPath ' New String() {"\File" & FullPath}
            RadAnswerExplain.ImageManager.UploadPaths = fileLocationPath 'New String() {"\File" & FullPath}
            RadAnswerExplain.ImageManager.MaxUploadFileSize = maxFileSize
        End If

        'ถ้าเกิดคำถามข้อนี้ไม่ได้เป็นข้อสอบแบบเรียงลำดับ ต้องทำการเพิ่ม Attr AnswerId เข้าไปให้กับ checkbox 'เลือกให้คำตอบข้อนี้แสดงเป็นลำดับสุดท้ายเสมอ' ด้วย เพื่อเอาไปใช้ตอน save ข้อมูล
        If QsetType <> 6 Then
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim CheckBox As CheckBox = CType(e.Item.FindControl("CheckThisAnswerToShowInLastRow"), CheckBox)
                CheckBox.Attributes.Add("Answer_Id", e.Item.DataItem("Answer_Id").ToString())
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการ save ช้อมูลคำถาม
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveQuestion()
        Dim QuestionId As String = Request.QueryString("qid")
        Dim QuestionSetId As String = Request.QueryString("QSetId")
        'คำถามที่ดึงขึ้นมาจาก RadEditor และผ่านการ Replace ค่าต่างๆเรียบร้อยแล้ว
        Dim QuestionName As String = GenPathForSaveImage(RadQuestion.Content.ToString()) '.Replace("<br />", "").Replace("&nbsp;", ""))
        'คำอธิบายคำถามที่ดึงขึ้นมาจาก RadEditor และผ่านการ Replace ค่าต่างๆเรียบร้อยแล้ว
        Dim QuestionExplain As String = GenPathForSaveImage(RadQuestionExplain.Content.ToString()) '.Replace("<br />", "").Replace("&nbsp;", ""))

        Dim QuestionMfiletxt As String = GenPathForSaveImage(RadQMultiTxt.Content.ToString())
        Dim QuestionExplainMfiletxt As String = GenPathForSaveImage(RadQMultiExplainTxt.Content.ToString())

        If QuestionExplain Is Nothing Then
            QuestionExplain = ""
        End If

        If QuestionName Is Nothing Then
            QuestionName = ""
        End If

        If QuestionMfiletxt Is Nothing Then
            QuestionMfiletxt = ""
        End If
        If QuestionExplainMfiletxt Is Nothing Then
            QuestionExplainMfiletxt = ""
        End If

        'check ว่ามี txt หรือเปล่าหรือว่ามีแต่ tag html
        QuestionName = CheckEmptyString(QuestionName)
        QuestionExplain = CheckEmptyString(QuestionExplain)
        QuestionMfiletxt = CheckEmptyString(QuestionMfiletxt)
        QuestionExplainMfiletxt = CheckEmptyString(QuestionExplainMfiletxt)

        If QuestionName = "" Or QuestionName = String.Empty Then
            lblWarn.Visible = True
            lblWarn.Text = "ห้ามใส่คำถามเป็นค่าว่าง"
            CheckValidate = False
            Exit Sub
        Else
            CheckValidate = True
        End If

        Dim NoChoiceShuffleAllowed As Integer = 0
        If chkNotAllowShuffleAnswer.Checked = True Then
            NoChoiceShuffleAllowed = 1
        End If

        Dim sqlUpdateQuestion As String = ""

        'ถ้าคำถามไม่ใช่ Type 6 save เหมือนเดิม
        If QsetType <> 6 Then
            sqlUpdateQuestion = " Update tblQuestion Set Question_Name = '" & ClsData.CleanString(QuestionName) & "', Question_Name_Backup = '" & ClsData.CleanString(QuestionName) & "', Question_Expain = '" & ClsData.CleanString(QuestionExplain) & "', LastUpdate = dbo.GetThaiDate(),ClientId = NULL , NoChoiceShuffleAllowed = '" & NoChoiceShuffleAllowed & "' "
            'ถ้าเลือกให้เอาคำถามข้อนี้ไปทับใน Mode Quiz
            If chkCopyThisQuestionToQuizMode.Checked = True Then
                sqlUpdateQuestion = GenStrUpdateForQuizMode(sqlUpdateQuestion, "Question_Name_Quiz", QuestionName)
            End If
            'ถ้าเลือกให้เอาอธิบายคำถามข้อนี้ไปทับใน Mode Quiz
            If chkCopyThisQuestionExplainToQuizMode.Checked = True Then
                sqlUpdateQuestion = GenStrUpdateForQuizMode(sqlUpdateQuestion, "Question_Expain_Quiz", QuestionExplain)
            End If
            sqlUpdateQuestion &= " Where Question_Id = '" & QuestionId & "' "
        Else
            'ถ้าคำถามเป็น Type 6 ให้ Save ลงที่ tblQuestionSet แทน
            sqlUpdateQuestion = " UPDATE dbo.tblQuestionset SET QSet_Name = '" & ClsData.CleanString(QuestionName) & "',LastUpdate = dbo.GetThaiDate() "
            'ถ้าเลือกให้เอาคำถามข้อนี้ไปทับใน Mode Quiz
            If chkCopyThisQuestionToQuizMode.Checked = True Then
                sqlUpdateQuestion = GenStrUpdateForQuizMode(sqlUpdateQuestion, "QSet_Name_Quiz", QuestionName)
            End If
            sqlUpdateQuestion &= " WHERE QSet_Id = '" & QsetId & "'; "
            'ถ้าเลือกให้เอาอธิบายคำถามข้อนี้ไปทับใน Mode Quiz
            sqlUpdateQuestion &= " UPDATE dbo.tblQuestion SET Question_Expain = '" & ClsData.CleanString(QuestionExplain) & "',LastUpdate = dbo.GetThaiDate() "
            If chkCopyThisQuestionExplainToQuizMode.Checked = True Then
                sqlUpdateQuestion = GenStrUpdateForQuizMode(sqlUpdateQuestion, "Question_Expain_Quiz", QuestionExplain)
            End If
            sqlUpdateQuestion &= " WHERE QSet_Id = '" & QsetId & "'; "
        End If

        Try
            'เมื่อกดตกลงแล้วให้มา update editconfirmed ด้วย
            If QsetType <> 6 Then
                ClsLayoutCheckConfirmed.UpdateEditConfirmedThisQuestion(QuestionId, ClsLayoutCheckConfirmed.LayoutType.Word)
            Else
                'ถ้าเป็นแบบเรียงลำดับต้อง update Question ทุกข้อใน Qset เลยเพราะว่า Qset นึงถือว่าเป็นข้อนึง
                ClsLayoutCheckConfirmed.UpdateEditConfirmedThisQuestion(QuestionId, ClsLayoutCheckConfirmed.LayoutType.Word, QsetId)
            End If

            ClsData.Execute(sqlUpdateQuestion)
            'LogQuestion = "Update ที่ QuestionId = " & QuestionId & " จากเดิม "" " & QuestionName & """"
            If QuestionNameBeforeEdit <> QuestionName Then

                IsEditQuestion = True
                LogQuestion = "แก้ไขคำถามเป็น """ & QuestionName

                If chkCopyThisQuestionToQuizMode.Checked Then
                    LogQuestion = LogQuestion & """ ทับที่ควิซด้วย, "
                Else
                    LogQuestion = LogQuestion & """ ไม่ทับที่ควิซ, "
                End If
                If chkNotAllowShuffleAnswer.Checked Then
                    LogQuestion = LogQuestion & "ห้ามสลับตัวเลือก"
                Else
                    LogQuestion = LogQuestion & "สลับตัวเลือกได้"
                End If

                'LogQuestion = LogQuestion & " (QuestionId=" & QuestionId & ")"

                'เก็บ Log
                'Log.Record(Log.LogType.ManageExam, ClsData.CleanString(LogQuestion), True, "", QuestionId, QsetId)
                Log.RecordLog(Log.LogCategory.EditQuestion, Log.LogAction.Update, True, LogQuestion, QuestionId)
            End If

            If QuestionExplainBeforeEdit <> QuestionExplain Then
                'Log.Record(Log.LogType.ManageExam, "แก้ไขคำอธิบายคำถามเป็น " & QuestionExplain & " (QuestionId=" & QuestionId & ")", True, "", QuestionId, QsetId)
                Log.RecordLog(Log.LogCategory.EditQuestion, Log.LogAction.Update, True, "แก้ไขคำอธิบายคำถามเป็น " & QuestionExplain, QuestionId)
            End If

            Dim sql As String
            If QuestionExplainMfiletxt <> "" Then
                Dim ClsData As New ClassConnectSql
                sql = "update tblmultimediaobject set MFileExplain = '" & QuestionExplainMfiletxt & "' where ReferenceId = '" & QuestionId & "' and ReferenceType = 2 and MFileLevel = 1"
                ClsData.Execute(sql)
            End If

            If QuestionMfiletxt <> "" Then
                Dim ClsData As New ClassConnectSql
                sql = "update tblmultimediaobject set MFileExplain = '" & QuestionMfiletxt & "' where ReferenceId = '" & QuestionId & "' and ReferenceType = 1 and MFileLevel = 1"
                ClsData.Execute(sql)
            End If


        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            CheckValidate = False
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            ShowError(ex)
        End Try

    End Sub

    ''' <summary>
    ''' ทำการ save ข้อมูลคำตอบ โดยแบ่งตามประเภทต่างๆ 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveAnswer()
        Dim QsetType As Integer
        Dim QuestionId As String = Request.QueryString("qid")
        Dim QuestionSetId As String = Request.QueryString("QSetId")

        'ดึง Datatable คำตอบไว้เพื่อเช็คการแก้ไขกับคำตอบที่จะบันทึกลงไป
        dtAnswer = GetDTAnswer(QuestionId)
        QsetType = GetQsetType(QuestionSetId)

        'ถ้าเป็น ถูก-ผิด
        If QsetType = 2 Then
            SaveTrueFalse()
        ElseIf QsetType = 6 Then
            'ถ้าเป็นเรียงลำดับ
            SaveType6()
        Else
            'ถ้าเป็น ตัวเลือก , จับคู่
            SaveType1()
        End If

        'เพิ่มคำศัพท์ใน Dictionary
        AddToWordBook(QuestionId)
    End Sub

    ''' <summary>
    ''' เมื่อกดปุ่ม save ทำการ check validation , save ข้อมูล คำถาม/คำตอบ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        SaveQuestion()
        If CheckValidate = True Then
            SaveAnswer()
        End If
    End Sub

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AnswerNumber() As String
        NumChoice += 1
        Return NumChoice
    End Function

    ''' <summary>
    ''' ทำการแปลง Path ของ Image ใหม่จาก ที่เป็นเลข QsetId หลายๆหลักให้ถูกแทนที่ด้วยคำว่า '___MODULE_URL___' แทน
    ''' </summary>
    ''' <param name="Path">เนื้อคำถาม/คำตอบ ที่จะทำการ Replace ข้อมูล</param>
    ''' <returns>String:เนื้อข้อมูลที่ได้ผ่านการ Replace Path รูปภาพด้วยคำว่า '___MODULE_URL___' ไปแล้ว</returns>
    ''' <remarks></remarks>
    Private Function GenPathForSaveImage(ByVal Path As String) As String
        If Path <> "" And Path IsNot Nothing Then
            'หาก่อนว่าคำถามข้อนี้มีรูปหรือเปล่า
            If Path.ToLower().IndexOf("<img") > -1 Then
                Path = ReplaceNeedlessString(Path)
            End If

            Dim QsetId As String = Request.QueryString("QSetId")
            'สตริง Module ที่เป็น Pattern ที่จะนำไป Replace '../file'
            Dim StrModule As String = "___MODULE_URL___"
            'ตัว root แบบต่างๆ ที่เราจะนำไปเป็นข้อมูลแทนค่าในการ Replace
            Dim rootPathHavePoint As String = "../file/"
            Dim rootPathNoPoint As String = "/File/"
            Dim filePathNoPoint As String
            Dim filePathHavePoint As String
            Dim SubStringQset As String = ""
            Dim PathComplete1 As String
            'ตัวแปร string ขั้นตอนสุดท้ายที่จะคืนค่ากลับไป
            Dim PathComplete2 As String

            filePathNoPoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) +
         "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) +
         "/" + QsetId.Substring(6, 1) + "/" + QsetId.Substring(7, 1) + "/"
            SubStringQset = filePathNoPoint
            filePathNoPoint = rootPathNoPoint + filePathNoPoint + "{" + QsetId + "}/"

            filePathHavePoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) +
             "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) +
             "/" + QsetId.Substring(6, 1) + "/" + QsetId.Substring(7, 1) + "/"
            filePathHavePoint = rootPathHavePoint + filePathHavePoint + "{" + QsetId + "}/"

            Dim StrPercent As String = rootPathNoPoint & SubStringQset & "%7B" & QsetId & "%7D" & "/"
            Dim StrPerCent2 As String = rootPathHavePoint & SubStringQset & "%7B" & QsetId & "%7D" & "/"
            Dim StrPercent3 As String = "/file/" & SubStringQset & "%7B" & QsetId & "%7D" & "/"
            Dim StrNotHavePercent As String = "/file/" & SubStringQset & "{" & QsetId & "}" & "/"


            PathComplete1 = Replace(Path, filePathHavePoint, StrModule)
            PathComplete1 = Replace(PathComplete1, StrPercent, StrModule)
            PathComplete2 = Replace(PathComplete1, filePathNoPoint, StrModule)
            PathComplete2 = Replace(PathComplete2, StrPerCent2, StrModule)
            PathComplete2 = Replace(PathComplete2, StrNotHavePercent, StrModule)
            PathComplete2 = Replace(PathComplete2, StrPercent3, StrModule)

            Return PathComplete2

        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' Function ที่ทำการแปลงเนื้อข้อมูลที่มีรูปภาพ โดยให้แปลงจากพวก '/File' , '/file' -> '../file'
    ''' </summary>
    ''' <param name="InputString">เนื้อข้อมูลที่จะเข้ามาทำการแปลงค่า</param>
    ''' <returns>String:เนื้อข้อมูลที่ถูกแปลงค่าให้ถูก Format เรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Private Function ReplaceNeedlessString(ByVal InputString As String)
        'Return InputString ถ้ามันถูกต้องตามที่ต้องการอยู่แล้ว
        If InputString.IndexOf("../file") > 0 Then
            Return InputString
        End If
        'ตัวแปรที่เอาไว้เก็บ string ที่เราจะนำไป Replace ทิ้ง
        Dim NeedlessString As String = ""
        If InputString.IndexOf("/File") >= 0 Then
            Dim a As Integer = 0
            Dim b As Integer = InputString.IndexOf("/File")
            a = b
            'For i = 1 To 500
            '    If GetChar(InputString, a) <> """" Then
            '        a -= 1
            '    Else
            '        Exit For
            '    End If
            'Next
            a = InStrRev(InputString, """", a)
            b = b - a
            NeedlessString = InputString.Substring(a, b)
            If NeedlessString <> "" Then
                InputString = InputString.Replace(NeedlessString, "")
            End If
        ElseIf InputString.IndexOf("/file") >= 0 Then
            Dim a As Integer = 0
            Dim b As Integer = InputString.IndexOf("/file")
            a = b
            'For i = 1 To 500
            '    If GetChar(InputString, a) <> """" Then
            '        a -= 1
            '    Else
            '        Exit For
            '    End If
            'Next
            a = InStrRev(InputString, """", a)
            b = b - a
            NeedlessString = InputString.Substring(a, b)
            If NeedlessString <> "" Then
                InputString = InputString.Replace(NeedlessString, "")
            End If
        End If

        Return InputString

    End Function





    ''' <summary>
    ''' ทำการ check ก่อนว่ามีการเลือกให้คำตอบสักข้อเป็นข้อถูกหรือยัง ถ้ายังจะไม่ยอมให้ save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateCheckBox() As Boolean
        'นับจำนวน item ทั้งหมดเพื่อนะไปเป็นจำนวนรอบในการวน loop
        Dim countRpt As Integer = RptCreateAnswer.Items.Count - 1
        Dim Score As Integer = 0
        'loop เพื่อดูว่ามีการเลือกให้คำตอบมีค่าเป็นถูกบ้างหรือเปล่า , เงื่อนไขการจบ loop คือ วนจนครบทุก Item ใน Repeater เพื่อหา checkbox 
        For index = 0 To countRpt
            'เปลี่ยนเป็นเก็บคะแนนจาก Textbox แทน
            Dim ScoreCheckBox As CheckBox = RptCreateAnswer.Items(index).FindControl("CheckScore")
            'If ScoreCheckBox.Checked = True Then
            '    Score += 1
            'End If
            Dim TxtScore As TextBox = RptCreateAnswer.Items(index).FindControl("txtScore")
            If TxtScore.Text <> "" Then
                Score += 1
            ElseIf ScoreCheckBox.Checked = True Then
                Score += 1
            End If
        Next
        'ถ้าวนครบหมดแล้ว score ยังเป็น 0 แสดงว่าไม่ได้เลือกข้อไหนเป็นข้อถูกสักข้อเลย จะไม่ยอมให้ save
        If Score = 0 Then
            Return False
            'ElseIf Score > 1 Then
            '    Return False
        End If
        Return True
    End Function



    ''' <summary>
    ''' ทำการ Update ข้อมูลคำตอบ แบบ ถูก-ผิด
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub SaveTrueFalse()
        'check ก่อนว่ามีการเปลี่ยนแปลงคำตอบหรือไม่
        'ถ้าคำตอบคือถูก
        Dim IsUpdateScore As Boolean = False

        If dtAnswer.Rows(0)("Answer_Name") = "True" Then
            'ตรวจสอบคะแนนของเดิมกับของใหม่ ถ้าตรงกันไม่ต้อง Update Score
            If Not (dtAnswer.Rows(0)("Answer_Score") = 1 And RadioTrue.Checked) Or (dtAnswer.Rows(0)("Answer_Score") = 0 And RadioTrue.Checked = False) Then
                IsUpdateScore = True
            End If
        Else
            If Not (dtAnswer.Rows(0)("Answer_Score") = 1 And RadioFalse.Checked) Or (dtAnswer.Rows(0)("Answer_Score") = 0 And RadioFalse.Checked = False) Then
                IsUpdateScore = True
            End If
        End If

        Dim QuestionId As String = Request.QueryString("qid")
        Dim NewAnswerScore As Integer = 0

        Dim sql As String = ""

        'เก็บรายละเอียด Log
        Dim LogAnswer As String = ""
        Dim LogAnswerExpain As String = ""

        'Update Answer_Score
        If IsUpdateScore Then
            For j = 0 To dtAnswer.Rows.Count - 1
                Dim AnswerId As Guid = dtAnswer.Rows(j)("Answer_Id")
                'Update AnswerScore
                If dtAnswer.Rows(j)("Answer_Score").ToString = "1" Then
                    NewAnswerScore = 0
                Else
                    NewAnswerScore = 1
                    LogAnswer = "แก้ไขคำตอบที่ถูกต้องเป็น '" & dtAnswer.Rows(j)("Answer_Name").ToString & "' (AnswerId='" & AnswerId.ToString & "')"
                End If

                sql = "Update tblAnswer set Answer_Score = '" & NewAnswerScore & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL where Answer_Id = '" & AnswerId.ToString & "';"
                ClsData.Execute(sql)
                If LogAnswer <> "" Then
                    Log.Record(Log.LogType.ManageExam, LogAnswer, 1, "", QuestionId, QsetId)
                End If
            Next
        End If

        'Update Answer_Expain
        For j = 0 To dtAnswer.Rows.Count - 1
            Dim AnswerId As Guid = dtAnswer.Rows(j)("Answer_Id")
            Dim OldAnswerExplain As String = GenPathForSaveImage(dtAnswer.Rows(j)("Answer_Expain"))
            Dim NewAnswerExplain As String
            Dim sqlToQuiz As String = ""

            If dtAnswer.Rows(j)("Answer_Name") = "True" Then
                NewAnswerExplain = GenPathForSaveImage(RadAnswerExplainTrue.Content.ToString())
                If chkCopyThisAnswerExplainToQuizModeTrue.Checked = True Then
                    sqlToQuiz = GenStrUpdateForQuizMode(sql, "Answer_Expain_Quiz", NewAnswerExplain)
                    LogAnswerExpain &= "ให้ไปทับที่ Quiz ด้วย"
                Else
                    LogAnswerExpain &= " ไม่นำไปทับที่ Quiz"
                End If
            Else
                NewAnswerExplain = GenPathForSaveImage(RadAnswerExplainFalse.Content.ToString())
                If chkCopyThisAnswerExplainToQuizModeFalse.Checked = True Then
                    sqlToQuiz = GenStrUpdateForQuizMode(sql, "Answer_Expain_Quiz", NewAnswerExplain)
                    LogAnswerExpain &= "ให้ไปทับที่ Quiz ด้วย"
                Else
                    LogAnswerExpain &= " ไม่นำไปทับที่ Quiz"
                End If
            End If

            If OldAnswerExplain <> NewAnswerExplain Then
                sql = "Update tblAnswer set Answer_Expain = '" & NewAnswerExplain & "'" & sqlToQuiz & ",LastUpdate = dbo.GetThaiDate(),ClientId = NULL  where Answer_Id = '" & AnswerId.ToString & "';"
                ClsData.Execute(sql)

                LogAnswerExpain = "แก้ไขคำอธิบายคำตอบ เป็น '" & NewAnswerExplain & "'" & LogAnswerExpain & " (AnswerId ='" & AnswerId.ToString & "')"
                Log.Record(Log.LogType.ManageExam, LogAnswerExpain, 1, "", QuestionId, QsetId)
            End If

        Next

        Response.Redirect("~/TestSet/editeachquestion.aspx?qid=" & VBQuestionId & "&QsetId=" & VBQsetId)
    End Sub

    ''' <summary>
    ''' ทำการ save ข้อมูลคำตอบ แบบ เรียงลำดับ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveType6()
        Try
            'Open Transaction
            ClsData.OpenWithTransection()

            Dim sql As String = ""
            'Editor ของคำตอบ
            Dim RadAnswerControl As RadEditor
            'Editor ของคำอธิบายคำตอบ
            Dim RadAnswerExplainControl As RadEditor
            'เนื้อคำถาม
            Dim QuestionName As String = ""
            'คำอธิบายคำตอบ
            Dim AnswerExplain As String = ""
            'นำตัวแปรอันนี้ไปรับค่า QuestionId ตอนวน loop
            Dim CurrentQuestionId As String = ""
            'checkbox 'เลือกให้เอาคำถามนี้ไปทับที่ modeQuiz ด้วย'
            Dim eachChkCopythisAnswerToQuizMode As CheckBox
            'checkbox 'เลือกให้เอาคำอธิบายคำตอบนี้ไปทับที่ modeQuiz ด้วย'
            Dim eachChkCopythisAnswerExplainToQuizMode As CheckBox
            'loop เพื่อเอา editor ทุกอันมาทำการ update ข้อมูลใน tblQuestion,tblAnswer , เงื่อนไขการจบ loop คือ วนครบหมดทุก control ใน RepeaterAnswer
            For Each ctrl As Control In RptCreateAnswer.Items
                RadAnswerControl = ctrl.FindControl("RadAnswer")
                RadAnswerExplainControl = ctrl.FindControl("RadAnswerExplain")
                'เนื้อคำถาม
                QuestionName = GenPathForSaveImage(RadAnswerControl.Content.ToString())
                'คำอธิบายคำตอบ
                AnswerExplain = GenPathForSaveImage(RadAnswerExplainControl.Content.ToString())
                CurrentQuestionId = RadAnswerControl.Attributes("questionId").ToString()
                eachChkCopythisAnswerToQuizMode = ctrl.FindControl("chkCopyThisAnswerToQuizMode")
                eachChkCopythisAnswerExplainToQuizMode = ctrl.FindControl("chkCopyThisAnswerExplainToQuizMode")
                If CurrentQuestionId <> "" Then
                    If QuestionName IsNot Nothing AndAlso QuestionName <> "" Then
                        'update QuestionName 
                        sql = " UPDATE dbo.tblQuestion SET Question_Name = '" & ClsData.CleanString(QuestionName) & "' , LastUpdate = dbo.GetThaiDate() "
                        'ถ้าเลือกให้เอาคำตอบข้อนี้ไปทับที่ Mode Quiz
                        If eachChkCopythisAnswerToQuizMode.Checked = True Then
                            sql = GenStrUpdateForQuizMode(sql, "Question_Name_Quiz", QuestionName)
                        End If
                        sql &= " WHERE Question_Id = '" & CurrentQuestionId & "'; "
                        ClsData.ExecuteWithTransection(sql)
                        'update AnswerExplain
                        sql = " UPDATE dbo.tblAnswer SET Answer_Expain = '" & ClsData.CleanString(AnswerExplain) & "',LastUpdate = dbo.GetThaiDate() "
                        'ถ้าเลือกให้เอาคำอธิบายคำตอบข้อนี้ไปทับที่ Mode Quiz
                        If eachChkCopythisAnswerExplainToQuizMode.Checked = True Then
                            sql = GenStrUpdateForQuizMode(sql, "Answer_Expain_Quiz", AnswerExplain)
                        End If
                        sql &= " WHERE Question_Id = '" & CurrentQuestionId & "'; "
                        ClsData.ExecuteWithTransection(sql)
                    Else
                        'rollback transaction
                        ClsData.RollbackTransection()
                        ShowError(New NullReferenceException, "ห้ามใส่คำตอบเป็นค่าว่าง")
                        Exit Sub
                    End If
                Else
                    'rollback transaction
                    ClsData.RollbackTransection()
                    lblWarn.Visible = True
                    lblWarn.Text = "ไม่มี questionId (Type6)"
                    Exit Sub
                End If
            Next
            'commit transasction
            ClsData.CommitTransection()
        Catch ex As Exception
            'rollback transaction
            ClsData.RollbackTransection()
            'show error
            ShowError(ex)
            Exit Sub
        End Try
        Response.Redirect("~/TestSet/editeachquestion.aspx?qid=" & VBQuestionId & "&QsetId=" & VBQsetId)
    End Sub

    'Private Sub UpdateReplaceBr()
    '    Dim dtselect As DataTable
    '    Dim sql As String
    '    'sql = "select Question_Id,Question_Name from tblQuestion where Question_Name like '%<table%'"
    '    'sql = "select Answer_Id,Answer_Name from tblAnswer where Answer_Name like '%<table%'"
    '    sql = "select Answer_Id,Answer_Name from tblAnswer where Answer_Name like '%MARGIN: 45px 5px 0px%' and Answer_Name like '%<table%'"
    '    'sql = "select Answer_Name,Answer_Id from tblAnswer where Answer_Name like '%<table style=""FLOAT: left;width:40px;"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">%' and Answer_Name like '%<table%'"
    '    'sql = "select Answer_Name,Answer_Id from tblAnswer where Answer_Name like '%<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">%' and Answer_Name like '%<table%'"
    '    dtselect = New DataTable
    '    dtselect = ClsData.getdata(sql)
    '    Dim sqlUpdate As String
    '    Dim QuestionIdTest As New Guid
    '    For Each r In dtselect.Rows
    '        Dim QuestionNameTest As String
    '        QuestionNameTest = r("Answer_Name")
    '        ' QuestionNameTest = QuestionNameTest.Replace("<table style=""FLOAT: left;width:40px;"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">")
    '        'QuestionNameTest = QuestionNameTest.Replace("<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""FLOAT: left;"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">")
    '        'QuestionNameTest = r("Answer_Name").ToString.Replace("<br />", "")
    '        QuestionNameTest = r("Answer_Name").ToString.Replace("MARGIN: 45px 5px 0px", "MARGIN: 15px 5px 0px")

    '        'QuestionNameTest = QuestionNameTest.Replace("margin: 15px 5px 0px", "margin: 45px 5px 0px")

    '        'QuestionNameTest = QuestionNameTest.Replace("<br>", "")
    '        'QuestionNameTest = QuestionNameTest.Replace("'times new roman'", """times new roman""")
    '        QuestionIdTest = r("Answer_Id")
    '        sqlUpdate = "Update tblAnswer set Answer_Name = '" & QuestionNameTest & "' where Answer_Id = '" & QuestionIdTest.ToString & "' "
    '        Try
    '            'ClsData.Execute(sqlUpdate)
    '        Catch ex As Exception
    '            Label1.Text = ex.ToString
    '            Label1.Visible = True
    '        End Try
    '    Next
    'End Sub

    ''' <summary>
    ''' ลบคำตอบที่เลือกมาโดยการกดปุ่มยางลบ
    ''' </summary>
    ''' <param name="AnswerId">Id ของ tblAnswer ของคำตอบที่ต้องการลบ</param>
    ''' <param name="QuestionId">Id ของ tblQuestion ของคำตอบที่ต้องการลบ</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำตอบที่ต้องการลบ</param>
    ''' <param name="QsetType">ประเภทของคำถามข้อนี้</param>
    ''' <returns>String:0:ไม่มี Session,Error:ไม่สำเร็จ,Complete:สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function DeleteAnswer(ByVal AnswerId As String, ByVal QuestionId As String, ByVal QsetId As String, ByVal QsetType As Integer) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()
        _DB.OpenWithTransection()
        Try
            Dim sql As String = " UPDATE dbo.tblAnswer SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Answer_Id = '" & AnswerId & "'; "
            _DB.ExecuteWithTransection(sql)
            'ถ้าเป็น Type 6 ต้อง update Question ด้วย
            If QsetType = 6 Then
                sql = " UPDATE dbo.tblQuestion SET IsActive = 0 ,LastUpdate = dbo.GetThaiDate() WHERE Question_Id = (SELECT Question_Id FROM tblAnswer WHERE Answer_Id = '" & AnswerId & "'); "
                _DB.ExecuteWithTransection(sql)
            End If
            Dim dt As New DataTable
            If QsetType = 6 Then
                'ถ้าเป็น Type 6 ให้ where ที่ qsetId แทน
                sql = " SELECT Answer_Id FROM dbo.tblAnswer WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1 ORDER BY Answer_No "
            Else
                sql = " SELECT Answer_Id FROM dbo.tblAnswer WHERE Question_Id = '" & QuestionId & "' AND QSet_Id = '" & QsetId & "' AND IsActive = 1 ORDER BY Answer_No "
            End If
            dt = _DB.getdataWithTransaction(sql)
            If dt.Rows.Count > 0 Then
                Dim EachAnswerId As String = ""
                Dim RunAnswerNo As Integer = 1
                'ทำการ loop เพื่อ Run เลขข้อของคำตอบใหม่ ถ้าเป็นแบบเรียงลำดับ ต้อง up AnswerName ด้วย , เงื่อนไขการจบ loop คือ วนครบทุกคำตอบของคำถามข้อนี้
                For index = 0 To dt.Rows.Count - 1
                    EachAnswerId = dt.Rows(index)("Answer_Id").ToString()
                    If QsetType = 6 Then
                        'ถ้าเป็น Type 6 ต้อง update Answer_name ด้วย
                        sql = " UPDATE dbo.tblAnswer SET Answer_No = '" & RunAnswerNo & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL,Answer_Name = '" & RunAnswerNo & "' WHERE Answer_Id = '" & EachAnswerId & "' "
                    Else
                        sql = " UPDATE dbo.tblAnswer SET Answer_No = '" & RunAnswerNo & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Answer_Id = '" & EachAnswerId & "' "
                    End If
                    Try
                        _DB.ExecuteWithTransection(sql)
                    Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                        _DB.RollbackTransection()
                        Return "Error"
                    End Try
                    RunAnswerNo += 1
                Next
            End If
            'ถ้าเป็น Type 6 ให้วน update Question_No อีกรอบ
            If QsetType = 6 Then
                sql = " SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1 ORDER BY Question_No; "
                dt.Clear()
                dt = _DB.getdataWithTransaction(sql)
                If dt.Rows.Count > 0 Then
                    Dim EachQuestionId As String = ""
                    Dim RunQuestionNo As Integer = 1
                    'loop เพื่อ Run เลขข้อของคำถามใหม่ เพราะว่า Type6 คำตอบก็คือ QuestionName,AnswerName คู่กัน , เงื่อนไขการจบ loop คือ วนจนครบทุกคำถาม
                    For index = 0 To dt.Rows.Count - 1
                        Try
                            EachQuestionId = dt.Rows(index)("Question_Id").ToString()
                            sql = " UPDATE dbo.tblQuestion SET Question_No = '" & RunQuestionNo & "',LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & EachQuestionId & "' "
                            _DB.ExecuteWithTransection(sql)
                        Catch ex As Exception
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                            _DB.RollbackTransection()
                            Return "Error"
                        End Try
                        RunQuestionNo += 1
                    Next
                End If
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "Error"
        End Try

        _DB.CommitTransection()

        Dim NewLog As String = " ลบคำตอบ (AnswerId = " & AnswerId & " , QuestionId = " & QuestionId & " "
        'เก็บ Log
        Log.Record(Log.LogType.ManageExam, _DB.CleanString(NewLog), True, "", QuestionId, QsetId)

        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function CancelEdit(ByVal QuestionId As String) As String

        Dim NewLog As String = "ยกเลิกการแก้ไขข้อสอบ (QuestionId=" & QuestionId & ")"
        'เก็บ Log
        Log.Record(Log.LogType.ManageExam, NewLog, True, "", QuestionId, HttpContext.Current.Request.QueryString("QSetId"))

        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteMultiFile(RefId As String, FileName As String) As String
        Dim _DB As New ClassConnectSql()
        _DB.OpenWithTransection()

        Try
            Dim sql As String = "update tblMultimediaObject set IsActive = 0 where ReferenceId = '" & RefId & "' and MFileName = '" & FileName & "';"
            _DB.ExecuteWithTransection(sql)
            _DB.CommitTransection()

            'เก็บ Log
            Dim NewLog As String = "ลบไฟล์เสียงที่ผูกอยู่กับ : Id=" & RefId & " ชื่อไฟล์ : " & FileName
            Log.Record(Log.LogType.ManageExam, NewLog, True, "", RefId, HttpContext.Current.Request.QueryString("QSetId"))

            Return "Complete"
        Catch ex As Exception
            _DB.RollbackTransection()
            'เก็บ Log
            Dim ErrorLog As String = "ลบไฟล์ไม่สำเร็จ (" & ex.ToString & ")"
            Log.Record(Log.LogType.ManageExam, ErrorLog, True, "", RefId, HttpContext.Current.Request.QueryString("QSetId"))
            Return "Error"
        End Try
    End Function

    <Services.WebMethod()>
    Public Shared Function testPostFromEditorTool(ByVal QuestionId As String) As String

        'Dim page As Page = DirectCast(HttpContext.Current.Handler, Page)
        ''Dim RQuest As New Telerik.Web.UI.RadEditor

        ''RQuest = DirectCast(page.FindControl("RadQuestion"), Telerik.Web.UI.RadEditor)

        'Dim upload As New RadEditor
        'upload = DirectCast(page.FindControl("RadQuestion"), RadEditor)

        'Dim ctrltlr As Telerik.Web.UI.RadEditor = fi


        'Dim ThaiTool As EditorToolGroup = New EditorToolGroup
        'Dim cuDateAjax As EditorTool = New EditorTool
        'cuDateAjax.Name = "InsertCustomDateAjax"
        'cuDateAjax.Text = "Insert Custom Date From Ajax"
        'ThaiTool.Tools.Add(cuDateAjax)
        'upload.Tools.Add(ThaiTool)

        Return "Complete"
    End Function

    ''' <summary>
    ''' ทำการเพิ่มคำตอบในคำถามข้อนี้
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddNewAnswer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddNewAnswer.Click
        Dim QuestinId As String = Request.QueryString("qid")
        Dim QsetId As String = Request.QueryString("QSetId")
        'ถ้าไม่ได้เป็นคำถามแบบเรียงลำดับ เข้าเงื่อนไข IF
        If QsetType <> 6 Then
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblAnswer WHERE Question_Id = '" & QuestinId & "' AND IsActive = 1; "
            Dim CheckIsNull As String = ClsData.ExecuteScalar(sql)
            If CheckIsNull = "0" Then
                sql = " INSERT INTO dbo.tblAnswer " &
                      " ( Answer_Id ,Question_Id ,Efficiency_Id ,QSet_Id ,Answer_No ,Answer_Name , " &
                      " Answer_Expain ,Answer_Score ,Answer_ScoreMinus , Answer_Position , " &
                      " IsActive ,Answer_Name_Bkup ,LastUpdate ) " &
                      " VALUES  ( NEWID() , '" & QuestinId & "' , NULL ,'" & QsetId & "' , " &
                      " 1 , '' ,  " &
                      " '' , 0 , 0 ,NULL , 1 , '' , dbo.GetThaiDate() ) "
            Else
                'ต้องเช็คก่อนว่าเป็นคำถาม Type3 หรือเปล่า
                If QsetType <> 3 Then
                    lblWarn.Visible = False
                    sql = " INSERT INTO dbo.tblAnswer " &
                      " ( Answer_Id ,Question_Id ,Efficiency_Id ,QSet_Id ,Answer_No ,Answer_Name , " &
                      " Answer_Expain ,Answer_Score ,Answer_ScoreMinus , Answer_Position , " &
                      " IsActive ,Answer_Name_Bkup ,LastUpdate ) " &
                      " VALUES  ( NEWID() , '" & QuestinId & "' , NULL ,'" & QsetId & "' , " &
                      " (SELECT MAX(Answer_No) FROM dbo.tblAnswer WHERE Question_Id = '" & QuestinId & "' AND QSet_Id = '" & QsetId & "' AND IsActive = 1 ) + 1 , '' ,  " &
                      " '' , 0 , 0 ,NULL , 1 , '' , dbo.GetThaiDate() ) "
                Else 'ถ้าเป็น Type3 จะต้องมีคำตอบได้แค่ข้อเดียว
                    lblWarn.Text = "ข้อสอบแบบจับคู่ คำถาม 1 ข้อ มีคำตอบได้แค่ 1 ข้อ เท่านั้น"
                    lblWarn.Visible = True
                    Exit Sub
                End If
            End If
            Try
                ClsData.Execute(sql)
                sql = " SELECT TOP 1 Answer_Id FROM dbo.tblAnswer WHERE Question_Id = '" & QuestinId & "' AND IsActive = 1 ORDER BY Answer_No desc "
                Dim LastAnswerId As String = ClsData.ExecuteScalar(sql)
                If LastAnswerId <> "" Then
                    Dim NewLog As String = " เพิ่มคำตอบ AnswerId = " & LastAnswerId & " ที่คำถาม Question = " & QuestinId & " "
                    Log.Record(Log.LogType.ManageExam, ClsData.CleanString(NewLog), True, "", QuestinId, QsetId)
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Response.Write(ex.ToString())
                Exit Sub
            End Try
            Response.Redirect("~/TestSet/editeachquestion.aspx?qid=" & QuestinId & "&QsetId=" & QsetId)
        Else
            'ถ้าเป็นคำถามแบบเรียงลำดับเข้าเงื่อนไข ELSE เพราะว่าต้องทำการเพิ่มทั้ง Question และ Answer ให้สัมพันธ์กัน
            Try
                'ถ้าเป็น Type6 เวลาเพิ่มให้เข้า function นี้แทน
                'open connection
                ClsData.OpenWithTransection()
                Dim sql As String = ""
                'หา questionId ใหม่ก่อน
                sql = " SELECT NEWID(); "
                'ต้อง Insert Question ก่อน
                Dim NewQuestionId As String = ClsData.ExecuteScalarWithTransection(sql)
                sql = " INSERT INTO dbo.tblQuestion( Question_Id ,QSet_Id ,EfficiencySet_Id ,IntroQset_Id ,Question_No , " &
                      " Question_Name ,Question_Expain ,IsActive ,Question_Name_Backup ,LastUpdate ,IsWpp ,ClientId ,NoChoiceShuffleAllowed ) " &
                      " SELECT  '" & NewQuestionId & "' , '" & QsetId & "' , NULL , NULL , CASE WHEN MAX(Question_No) + 1 IS NULL THEN 0 ELSE MAX(Question_No) + 1 END , " &
                      " 'คำถามใหม่' ,'' , 1 , 'คำถามใหม่' ,dbo.GetThaiDate() ,1 , NULL ,0  FROM dbo.tblQuestion WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1; "
                ClsData.ExecuteWithTransection(sql)
                'Insert Answer
                sql = " INSERT INTO dbo.tblAnswer( Answer_Id ,Question_Id ,Efficiency_Id ,QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score ,Answer_ScoreMinus , " &
                      " Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate ,ClientId ,IsWpp ,AlwaysShowInLastRow ) " &
                      " SELECT NEWID() , '" & NewQuestionId & "' ,NULL , '" & QsetId & "' , CASE WHEN  MAX(Answer_No) + 1 IS NULL THEN 0 ELSE MAX(Answer_No) + 1 END , " &
                      " CASE WHEN MAX(Answer_No) + 1 IS NULL THEN CAST(0 AS VARCHAR(10)) ELSE CAST(MAX(Answer_No) + 1 AS VARCHAR(20)) END, '' , 1.0 , 0.0 , NULL , 1 ,CASE WHEN MAX(Answer_No) + 1 " &
                      " IS NULL THEN CAST(0 AS VARCHAR(10)) ELSE CAST(MAX(Answer_No) + 1 AS VARCHAR(20)) END , dbo.GetThaiDate() ,NULL ,0 ,0 " &
                      " FROM dbo.tblAnswer WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1 "
                ClsData.ExecuteWithTransection(sql)
                'commit transaction
                ClsData.CommitTransection()
            Catch ex As Exception
                'rollback transaction
                ClsData.RollbackTransection()
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Response.Write(ex.ToString())
                Exit Sub
            End Try
            Response.Redirect("~/TestSet/editeachquestion.aspx?qid=" & QuestinId & "&QsetId=" & QsetId)
        End If
    End Sub

    ''' <summary>
    ''' Function ที่จะเช็คว่าเนื้อข้อมูลที่ส่งเข้ามาว่าเป็นค่าว่างหรือเปล่า มีแค่ tag html ที่ไม่ได้ใช้หรือเปล่า
    ''' </summary>
    ''' <param name="InputString">เนื้อข้อมูลที่ส่งเข้ามา</param>
    ''' <returns>String:ถ้าเกิดว่ามีแต่ Tag ที่ไม่ได้ใช้งานก็จะทำการ Return ค่าว่างกลับไป,แต่ถ้ามีข้อมูลก็จะส่งข้อมูลแบบดั้งเดิมกลับไป</returns>
    ''' <remarks></remarks>
    Private Function CheckEmptyString(ByVal InputString As String) As String
        If InputString.Contains("<img") = True Then
            Return InputString
        End If
        'ทำการ Check ข้อมูลที่ส่งเข้ามาแล้วทำการ Replace ค่าอักขระแปลกๆให้เป็นค่าว่างซะ
        Dim checkString = Regex.Replace(InputString, "<.*?>", String.Empty)
        checkString = checkString.Replace(" ", "").Replace(vbLf, "").Replace(vbCrLf, "").Replace(vbNewLine, "").Replace(vbCr, "").Replace("&nbsp;", "")
        If checkString = "" Or checkString = String.Empty Then
            Return ""
        Else
            Return InputString
        End If
    End Function

    ''' <summary>
    ''' ถ้าข้อสอบเป็นแบบ ตัวเลือก ให้ทำการ แสดง checkbox ต่างๆขึ้นมาให้หมด
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowAllCheckbox()
        'show checkbox chkNotAllowShuffleAnswer
        chkNotAllowShuffleAnswer.Visible = True
        'loop เพื่อทำการหา checkbox ใน repeaterAnswer เพื่อทำการ visible มัน , เงื่อนไขการจบ loop คือ วนจนครบทุก Item
        For Each allControl As Control In RptCreateAnswer.Items
            Dim chkboxAnswer As CheckBox = allControl.FindControl("CheckThisAnswerToShowInLastRow")
            chkboxAnswer.Visible = True
        Next
    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลว่า checkbox ไหนที่ถูกติ๊กบ้าง ถ้ามีให้ทำการติ๊กให้หมด เช่น checkbox ห้ามสลับคำตอบ , คำตอบข้อนี้แสดงเป็นลำดับสุดท้ายเสมอ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDataTickCheckbox()
        Dim sql As String = ""
        'เช็คว่าได้ tick checkbox ห้ามสลับตัวเลือกไว้หรือเปล่า
        sql = " SELECT NoChoiceShuffleAllowed FROM dbo.tblQuestion WHERE Question_Id = '" & VBQuestionId & "'; "
        Dim checkValue As Boolean = CType(ClsData.ExecuteScalar(sql), Boolean)
        If checkValue = True Then
            chkNotAllowShuffleAnswer.Checked = True
            Exit Sub
        End If

        'ถ้าไม่ได้เช็คที่คำถาม ให้มาเช็คต่อว่ามีเช็คที่คำตอบบ้างหรือเปล่า
        'sql = " SELECT Answer_Id FROM dbo.tblAnswer WHERE Question_Id = '" & VBQuestionId & "' AND AlwaysShowInLastRow = 1; "
        'Dim AnswerId As String = ClsData.ExecuteScalar(sql)
        'If AnswerId <> "" Then
        '    For Each allControl As Control In RptCreateAnswer.Items
        '        Dim chkboxAnswer As CheckBox = allControl.FindControl("CheckThisAnswerToShowInLastRow")
        '        If chkboxAnswer.Attributes("Answer_Id").ToString().ToLower() = AnswerId.ToLower() Then
        '            chkboxAnswer.Checked = True
        '        End If
        '    Next
        'End If

        'sql = " SELECT Answer_Score,Answer_Id FROM dbo.tblAnswer WHERE Question_Id = '" & VBQuestionId & "'; "
        'Dim dtAnswerScore As DataTable = ClsData.getdata(sql)

        'For Each sc In dtAnswerScore.Rows

        '    Dim AnswerScore As String = sc("Answer_Score")
        '    Dim AnsId As String = sc("Answer_Id").ToString

        '    For Each allControl As Control In RptCreateAnswer.Items
        '        Dim scoreAnswer As TextBox = allControl.FindControl("txtScore")
        '        If scoreAnswer.Attributes("answerid").ToString().ToLower() = AnsId.ToLower() Then
        '            scoreAnswer.Text = AnswerScore
        '        End If
        '    Next

        'Next

        sql = " SELECT Answer_Id,Answer_Score,AlwaysShowInLastRow FROM dbo.tblAnswer WHERE Question_Id = '" & VBQuestionId & "' And IsActive = 1;"
        Dim dtAnswerScore As DataTable = ClsData.getdata(sql)

        For Each Ans In dtAnswerScore.Rows
            Dim AnswerId As String = Ans("Answer_Id").ToString
            Dim AnswerScore As String = Ans("Answer_Score").ToString
            Dim ShowInLastRow As Boolean
            If Ans("AlwaysShowInLastRow").ToString <> "" Then
                ShowInLastRow = Ans("AlwaysShowInLastRow")
            Else
                ShowInLastRow = False
            End If


            For Each allControl As Control In RptCreateAnswer.Items
                Dim chkboxAnswer As CheckBox = allControl.FindControl("CheckThisAnswerToShowInLastRow")
                Dim txtScore As TextBox = allControl.FindControl("txtScore")

                If chkboxAnswer.Attributes("Answer_Id").ToString().ToLower() = AnswerId.ToLower() Then
                    txtScore.Text = AnswerScore
                    If ShowInLastRow Then
                        chkboxAnswer.Checked = True
                    End If
                End If
            Next
        Next




    End Sub

    ''' <summary>
    ''' Function ที่ Gen Query เพื่อไป Update ข้อมูลใน Field Quiz ด้วย สำหรับกรณีที่เลือกให้เอาข้อมูลนี้ไปทับที่ mode Quiz ด้วย
    ''' </summary>
    ''' <param name="OriginalQuery">query ที่ update Question/Answer ก่อนที่จะมาถึงขั้นตอนนี้</param>
    ''' <param name="UpdateField">Field ที่ต้องการจะ Update</param>
    ''' <param name="QuizName">เนื้อคำถาม/คำตอบ</param>
    ''' <returns>String:Query ที่ทำการต่อในส่วนที่ update ข้อมูลไปทับ Field Quiz ด้วยเรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Private Function GenStrUpdateForQuizMode(ByVal OriginalQuery As String, ByVal UpdateField As String, ByVal QuizName As String) As String
        Dim returnStr As String = OriginalQuery
        returnStr &= " ," & UpdateField & " = '" & ClsData.CleanString(QuizName) & "' "
        Return returnStr
    End Function

    ''' <summary>
    ''' Function ที่ทำการ check ว่าคำตอบข้อนี้ได้ถูกเลือกให้เป็น คำตอบที่จะแสดงอยู่ข้อสุดท้ายเสมอรึเปล่า ?
    ''' </summary>
    ''' <param name="AnswerId">Id ของคำตอบที่ต้องการ check</param>
    ''' <returns>Integer:1=ใช่,0=ไม่ใช่</returns>
    ''' <remarks></remarks>
    Private Function CheckThisAnswerIsAlwaysShowInLastRow(ByVal AnswerId As String) As Integer
        Dim returnValue As Integer = 0
        'ถ้าเป็น row สุดท้าย และ เลือกว่าห้ามสลับคำตอบ จะต้อง update AlwaysShowInLastRow = 1
        'loop เพื่อหา checkbox เพื่อดูว่า user ได้ทำการเลือกให้คำตอบข้อนี้แสดงเป็นลำดับสุดท้ายเสมอรึเปล่า , เงื่อนไขการจบ loop คือ วนให้หมดทุก Item ของ Repeater คำตอบ
        For Each eachControl As Control In RptCreateAnswer.Items
            Dim chkAnswer As CheckBox = eachControl.FindControl("CheckThisAnswerToShowInLastRow")
            'ถ้า checkbox ถูกเลือกอยู่แสดงว่าต้องการให้คำตอบข้อนี้อยู่เป็นลำดับสุดท้ายเสมอ
            If chkAnswer.Checked = True Then
                If chkAnswer.Attributes("Answer_Id").ToString().ToLower() = AnswerId.ToLower() Then
                    returnValue = 1
                End If
            End If
        Next
        Return returnValue
    End Function

    ''' <summary>
    ''' ไม่แน่ใจว่าเอาไว้ทำอะไร
    ''' </summary>
    ''' <param name="QuestionId"></param>
    ''' <remarks></remarks>
    Private Sub AddToWordBook(ByVal QuestionId As String)
        'Update 15-05-56 / Add Words To Wordbook If QuestionId Is English
        Dim ClsWordBook As New ClsWordBook()
        If ClsWordBook.QuestionIdIsEnglish(QuestionId) Then
            ClsWordBook.ClearQuestionIdInWordbook(QuestionId)
            ClsWordBook.InsertWordToWordBook(QuestionId)
        End If
    End Sub

    ''' <summary>
    ''' ทำการ update ข้อมูลคำตอบ แบบตัวเลือก , จับคู่
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveType1()
        If ValidateCheckBox() = True Then
            Dim sqlUpdateAnswer As String = ""
            'ทำการ loop เพื่อจะ update คำตอบทีละข้อ , เงื่อนไขการจบ loop คือ วนจนครบหมดทุก item คำตอบใน repeater
            For index = 0 To RptCreateAnswer.Items.Count - 1
                Dim ScoreCheckBox As CheckBox = RptCreateAnswer.Items(index).FindControl("CheckScore")
                Dim eachChkCopyThisAnswerToQuizMode As CheckBox = RptCreateAnswer.Items(index).FindControl("chkCopyThisAnswerToQuizMode")
                Dim eachChkCopyThisAnswerExplaintoQuizMode As CheckBox = RptCreateAnswer.Items(index).FindControl("chkCopyThisAnswerExplainToQuizMode")
                Dim RadAnswerControl As RadEditor = RptCreateAnswer.Items(index).FindControl("RadAnswer")
                Dim RadAnswerExplainControl As RadEditor = RptCreateAnswer.Items(index).FindControl("RadAnswerExplain")
                Dim Score As TextBox = RptCreateAnswer.Items(index).FindControl("txtScore")
                Dim scoreAnswer As Integer = Score.Text
                'If ScoreCheckBox.Checked = True Then
                'scoreAnswer = 1
                'Else
                'scoreAnswer = 0
                'End If

                Dim AnswerId As Guid = dtAnswer.Rows(index)("Answer_Id")
                Dim OriginalAnswerName As String = dtAnswer.Rows(index)("Answer_Name")

                'คำตอบ
                Dim AnswerName As String = GenPathForSaveImage(RadAnswerControl.Content.ToString)
                'อธิบายคำตอบ
                Dim AnswerExplain As String = GenPathForSaveImage(RadAnswerExplainControl.Content.ToString)

                If AnswerExplain Is Nothing Then
                    AnswerExplain = ""
                End If

                If AnswerName Is Nothing Then
                    AnswerName = ""
                End If

                AnswerName = CheckEmptyString(AnswerName)
                AnswerExplain = CheckEmptyString(AnswerExplain)

                If AnswerName = "" Or AnswerName = String.Empty Then
                    lblWarn.Visible = True
                    lblWarn.Text = "ห้ามใส่คำตอบเป็นค่าว่าง"
                    Exit Sub
                End If

                'ดูว่าข้อนี้ถูกเลือกให้เป็นคำตอบที่แสดงเป็นอันสุดท้ายรึเปล่า
                Dim AlwaysShowInLastRow As Integer = CheckThisAnswerIsAlwaysShowInLastRow(AnswerId.ToString())

                sqlUpdateAnswer = "Update tblAnswer Set Answer_Name = '" & ClsData.CleanString(AnswerName) & "', Answer_Name_Bkup = '" & ClsData.CleanString(AnswerName) & "', Answer_Score = '" & ClsData.CleanString(scoreAnswer).ToString() & "', Answer_Expain = '" & ClsData.CleanString(AnswerExplain) & "', LastUpdate = dbo.GetThaiDate(),ClientId = NULL,AlwaysShowInLastRow = '" & AlwaysShowInLastRow & "' "
                'ถ้าเลือกให้คำตอบข้อนี้ไปทับที่ mode Quiz ด้วย
                If eachChkCopyThisAnswerToQuizMode.Checked = True Then
                    sqlUpdateAnswer = GenStrUpdateForQuizMode(sqlUpdateAnswer, "Answer_Name_Quiz", AnswerName)
                End If
                'ถ้าเลือกให้อธิบายคำตอบข้อนี้ไปทับที่ mode Quiz ด้วย
                If eachChkCopyThisAnswerExplaintoQuizMode.Checked = True Then
                    sqlUpdateAnswer = GenStrUpdateForQuizMode(sqlUpdateAnswer, "Answer_Expain_Quiz", AnswerExplain)
                End If
                sqlUpdateAnswer &= " Where Answer_Id = '" & AnswerId.ToString() & "' "
                Try
                    ClsData.Execute(sqlUpdateAnswer)
                    If OriginalAnswerName <> AnswerName Then
                        'LogAnswer = LogAnswer & " แก้ไขคำตอบเป็น AnswerId = " & AnswerId.ToString() & " จาก """ & OriginalAnswerName & """"
                        'LogAnswer = LogAnswer & " เป็น """ & AnswerName & """" & "<br />"
                        LogAnswer = "แก้ไขคำตอบเป็น """ & AnswerName & """"
                        If scoreAnswer = 1 Then
                            LogAnswer = LogAnswer & " เป็นคำตอบที่ถูก,"
                        End If
                        If AlwaysShowInLastRow = 1 Then
                            LogAnswer = LogAnswer & " เป็นข้อสุดท้าย,"
                        End If
                        If eachChkCopyThisAnswerExplaintoQuizMode.Checked Then
                            LogAnswer = LogAnswer & " ทับที่ควิซด้วย"
                        End If
                        If LogAnswer.Substring(LogAnswer.Length - 1, 1) = "," Then
                            LogAnswer = LogAnswer.Substring(0, LogAnswer.Length - 1)
                        End If

                        LogAnswer = LogAnswer & " (AnswerId=" & AnswerId.ToString() & ")"

                        Log.Record(Log.LogType.ManageExam, ClsData.CleanString(LogAnswer), True, "", VBQuestionId, QsetId)

                        If AnswerExplain <> "" Then
                            Log.Record(Log.LogType.ManageExam, "แก้ไขคำอธิบายคำตอบเป็น " & AnswerExplain & " (AnswerId=" & AnswerId.ToString & ")", True, "", VBQuestionId, QsetId)
                        End If

                    End If
                    lblWarn.Visible = False
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    ShowError(ex)
                End Try
            Next

            If LogAnswer Is Nothing And IsEditQuestion = False Then
                Log.Record(Log.LogType.ManageExam, "กดบันทึกปิดหน้าจอไปโดยไม่ได้แก้ไขอะไร (QuestionId = " & VBQuestionId & ")", True, "", VBQuestionId, QsetId)
            End If
            IsEditQuestion = False

            Response.Redirect("~/TestSet/editeachquestion.aspx?qid=" & VBQuestionId & "&QsetId=" & VBQsetId)
        Else
            'MsgBox("กรุณาเลือกคำตอบที่ถูกต้อง/ห้ามเลือกมาากว่า 1 ข้อ")
            lblWarn.Visible = True
            lblWarn.Text = "กรุณาเลือกคำตอบที่ถูกต้อง/ห้ามเลือกมากกว่า 1 ข้อ"
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' ทำการหาชนิดของข้อสอบว่าเป็นประเภทไหน ตัวเลือก,ถูก-ผิด,จับคู่,เรียงลำดับ
    ''' </summary>
    ''' <param name="QuestionSetId">Id ของ tblQuestionSet</param>
    ''' <returns>Integer:ชนิดของข้อสอบ 1=ตัวเลือก,2=ถูก-ผิด,3=จับคู่,6=เรียงลำดับ</returns>
    ''' <remarks></remarks>
    Private Function GetQsetType(ByVal QuestionSetId As String) As Integer
        Dim sqlQSettype As String = " Select QSet_Type From tblQuestionSet Where QSet_Id = '" & QuestionSetId & "' "
        Dim qsetType As String = CInt(ClsData.ExecuteScalar(sqlQSettype))
        Return qsetType
    End Function

    ''' <summary>
    ''' หาข้อมูลคำตอบ
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ข้อนี้</param>
    ''' <returns>Datatable:ที่มีข้อมูลคำตอบ</returns>
    ''' <remarks></remarks>
    Private Function GetDTAnswer(ByVal QuestionId As String) As DataTable
        Dim sqlAnswer As String = " select Answer_Id,Answer_Name,Answer_No,Answer_Score,Answer_Expain from tblAnswer where Question_Id ='" & QuestionId & "' AND IsActive = 1 order by Answer_No "
        dtAnswer = New DataTable
        dtAnswer = ClsData.getdata(sqlAnswer)
        Return dtAnswer
    End Function

    ''' <summary>
    ''' ทำการแสดง Label เพื่อแสดง Error ที่เกิดขึ้น
    ''' </summary>
    ''' <param name="ex">ตัวแปร Error ที่เกิดขึ้น</param>
    ''' <param name="SpecificExceptionStr">ข้อความที่ต้องการแสดงให้ User เห็นกรณีเกิด Error ถ้าไม่ส่งเข้ามาก็จะแสดงตัวแปร ex แทน</param>
    ''' <remarks></remarks>
    Private Sub ShowError(ex As Exception, Optional ByVal SpecificExceptionStr As String = Nothing)
        Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        lblWarn.Visible = True
        If SpecificExceptionStr IsNot Nothing Then
            lblWarn.Text = SpecificExceptionStr
        Else
            lblWarn.Text = ex.ToString
        End If
    End Sub

    Private Sub btnOpenVowel_Click(sender As Object, e As EventArgs) Handles btnOpenVowel.Click

        If CheckOpenTool Then
            RemoveButton("Sara01")
            RemoveButton("Sara02")
            RemoveButton("Sara03")
            RemoveButton("Sara04")
            RemoveButton("Sara05")
            RemoveButton("Sara06")
            RemoveButton("Sara07")
            RemoveButton("Sara08")
            RemoveButton("Sara10")
            RemoveButton("Sara12")
            RemoveButton("Sara15")
            RemoveButton("Sara19")
            CheckOpenTool = False
        Else

            Dim ThaiTool As EditorToolGroup = New EditorToolGroup

            Dim cu01 As EditorTool = New EditorTool
            cu01.Name = "Sara01"
            cu01.Text = "อะ"

            Dim cu02 As EditorTool = New EditorTool
            cu02.Name = "Sara02"
            cu02.Text = "อา"

            Dim cu03 As EditorTool = New EditorTool
            cu03.Name = "Sara03"
            cu03.Text = "อิ"

            Dim cu04 As EditorTool = New EditorTool
            cu04.Name = "Sara04"
            cu04.Text = "อี"

            Dim cu05 As EditorTool = New EditorTool
            cu05.Name = "Sara05"
            cu05.Text = "อึ"

            Dim cu06 As EditorTool = New EditorTool
            cu06.Name = "Sara06"
            cu06.Text = "อื"

            Dim cu07 As EditorTool = New EditorTool
            cu07.Name = "Sara07"
            cu07.Text = "อุ"

            Dim cu08 As EditorTool = New EditorTool
            cu08.Name = "Sara08"
            cu08.Text = "อู"

            Dim cu10 As EditorTool = New EditorTool
            cu10.Name = "Sara10"
            cu10.Text = "เอ"

            Dim cu12 As EditorTool = New EditorTool
            cu12.Name = "Sara12"
            cu12.Text = "แอ"

            Dim cu15 As EditorTool = New EditorTool
            cu15.Name = "Sara15"
            cu15.Text = "เอาะ"

            Dim cu19 As EditorTool = New EditorTool
            cu19.Name = "Sara19"
            cu19.Text = "เอียะ"


            ThaiTool.Tools.Add(cu01)
            ThaiTool.Tools.Add(cu02)
            ThaiTool.Tools.Add(cu03)
            ThaiTool.Tools.Add(cu04)
            ThaiTool.Tools.Add(cu05)
            ThaiTool.Tools.Add(cu06)
            ThaiTool.Tools.Add(cu07)
            ThaiTool.Tools.Add(cu08)
            ThaiTool.Tools.Add(cu10)
            ThaiTool.Tools.Add(cu12)
            ThaiTool.Tools.Add(cu15)
            ThaiTool.Tools.Add(cu19)

            RadQuestion.Tools.Add(ThaiTool)

            CheckOpenTool = True
        End If

    End Sub

    Public Sub RemoveButton(ByVal name As String)
        For Each group As Telerik.Web.UI.EditorToolGroup In RadQuestion.Tools
            Dim tool As EditorTool = group.FindTool(name)
            If tool IsNot Nothing Then
                group.Tools.Remove(tool)
            End If
        Next
    End Sub
End Class
