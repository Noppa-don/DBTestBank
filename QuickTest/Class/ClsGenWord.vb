Imports System.IO
Imports System.Drawing
Imports Syncfusion.DocIO
Imports Syncfusion.DocIO.DLS
Imports Syncfusion.Pdf
Imports Syncfusion.DocToPDFConverter

Public Class ClsGenWord
    'ตัวแปรที่เก็บค่า URL เพื่อนำไป Replace กับเนื้อข้อมูลเพื่อให้แสดงรูปภาพได้ถูกต้อง
    Public GetUrl As String
    'ตัวแปรจัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As ClsConnect
    'ขนาดตัวอักษรที่จะใช้ในการ Genword ใช้ทั้งหน้า
    Dim AllFontSize As String

    Private EnablePrefixForRunningNoInWordFile As Boolean = CBool(ConfigurationManager.AppSettings("EnablePrefixForRunningNoInWordFile"))

    ''' <summary>
    ''' ทำการหา URL ที่ Run อยู่ในขณะนั้นเพื่อนำมาตัดสตริงให้เหลือแต่ท่อนหน้าสุด เช่น 
    ''' จาก "http://localhost:18615/testset/genpdf.aspx" ให้เหลือแค่ "http://localhost:18615/"
    ''' เพื่อนำมาแทนค่า '../' ในเนื้อข้อมูลที่เป็นรูปภาพเพื่อให้ไปหา Path รูปภาพได้ถูกต้อง
    ''' </summary>
    ''' <param name="DB">Class Connection</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal DB As ClassConnectSql)
        _DB = DB
        GetUrl = HttpContext.Current.Request.Url.ToString()
        Dim pos As Integer = InStrRev(GetUrl, "/")
        GetUrl = GetUrl.Substring(0, pos - 8)
    End Sub

    'เป็นการบอกว่าจะ set ให้ Paragraph นั้นอยู่ ชิดซ้าย,กึ่งกลาง,ชิดขวา
    Enum SetPositionWord
        Left = 1
        Center = 2
        Right = 3
    End Enum

    'Enum ที่บอกว่าจะ Gen File Word ให้เป็นแบบไหน บางส่วน,แบบเต็ม,แสดงเฉลย
    Enum GenType
        SomeExam = 1
        FullExam = 2
        ShowCorrectAnswer = 3
        ShowCorrectAnswerWithExplain = 4
    End Enum

    Enum QuestionType
        Choice = 1
        RightWrong = 2
        Pair = 3
        Sort = 4
    End Enum


    'Class ที่เอาไว้ใช้ในการ Query เกี่ยวกับข้อสอบ ใช้อยู่ในหน้านี้หลายจุด
    Dim ClsPDF As New ClsPDF(New ClassConnectSql())

    Private Property QuestionNo As QuestionNumber
    Private Property ColWidthType1 As Integer = 5
    Private Property ColWidthType2 As Integer = 3
    Private Property ColWidthType3 As Integer = 5

    ''' <summary>
    ''' ทำการ Gen ลายน้ำให้เป็น Logo วพ.
    ''' </summary>
    ''' <param name="document">object ไฟล์ Word ที่ต้องการ Gen ลายน้ำ</param>
    ''' <remarks></remarks>
    Public Sub GenWaterMark(ByVal document As WordDocument)
        'Function Gen ให้ Word มีลายน้ำ
        Dim dataPath As String = HttpContext.Current.Server.MapPath("~\Images\WaterMark")
        Dim picWatermark As PictureWatermark = New PictureWatermark
        document.Watermark = picWatermark
        picWatermark.Picture = System.Drawing.Image.FromFile(Path.Combine(dataPath, "WatermarkWPP.png"))
    End Sub

    ''' <summary>
    ''' ทำการส่ง Gen ไฟล์ Word ตามประเภทที่เลือกมา
    ''' </summary>
    ''' <param name="TypeToGenWord">ประเภทที่ต้องการให้ Gen ไฟล์ Word , ตัวอย่างบางส่วน,เต็ม,แสดงเฉลย</param>
    ''' <param name="TestsetId">Id ของ tblTestset ที่ต้องการให้ Gen ไฟล์ Word</param>
    ''' <remarks></remarks>
    Public Sub GenNewDocument(ByVal TypeToGenWord As GenType, Optional ByVal TestsetId As String = "")

        'ถ้ามีส่ง TestsetId เข้ามาให้ Assign เข้าไปเป็น Session เพื่อทำงานต่อไปเลย
        If TestsetId <> "" Then
            HttpContext.Current.Session("newTestSetId") = TestsetId
        End If

        'สร้าง File WordDocument ขึ้นมาใหม่
        Dim document As New WordDocument
        document.XHTMLValidateOption = DLS.XHTMLValidationType.None
        'document.EnsureMinimal()
        Dim section As WSection = TryCast(document.AddSection(), WSection)
        'ตั้งค่าหน้ากระดาษ
        section.PageSetup.Margins.Top = 42.5
        section.PageSetup.Margins.Bottom = 42.5
        section.PageSetup.Margins.Left = 58
        section.PageSetup.Margins.Right = 58

        'Get Font-Size Text
        Dim GetFontSize As String = GetTestFontSize(HttpContext.Current.Session("newTestSetId").ToString())
        If GetFontSize = "" Then 'ถ้าไม่มี Size ให้เป็น Size ปกติ
            AllFontSize = "21.5px"
        ElseIf GetFontSize = "0" Then 'Size ตัวปกติ(สำหรับเด็กมัธยม)
            AllFontSize = "20.5px"
        ElseIf GetFontSize = "1" Then 'Size ตัวใหญ่(สำหรับเด็กประถมปลาย)
            AllFontSize = "21.5px"
        ElseIf GetFontSize = "2" Then 'Size ตัวใหญ่มาก(สำหรับเด็กประถมต้น)
            AllFontSize = "23.5px"
        End If

        'GenWaterMark
        GenWaterMark(document)

        'GenHeader
        Dim CountAmount As String = GetExamAmount(HttpContext.Current.Session("newTestSetId").ToString())
        'Dim TestSetName As String = GetTestSetName(HttpContext.Current.Session("newTestSetId").ToString())
        'Dim TestSetTime As String = GetTestSetTime(HttpContext.Current.Session("newTestSetId").ToString())
        'GenHeader(document, section, TestSetName, CountAmount, TestSetTime)

        Dim dtTestset As DataTable = GetTestsetDetail(HttpContext.Current.Session("newTestSetId").ToString())

        Dim TestSetName As String = dtTestset.Rows(0)("TestSet_Name")
        Dim TestSetTime As String = dtTestset.Rows(0)("TestSet_Time")
        GenHeader(document, section, TestSetName, CountAmount, TestSetTime)

        QuestionNo = New QuestionNumber(CountAmount)
        If Not EnablePrefixForRunningNoInWordFile Then
            QuestionNo.NumberType = QuestionNoType.defaultNumber
        Else
            If dtTestset.Rows(0)("PrefixForRunningNoInWordFile") Is DBNull.Value OrElse dtTestset.Rows(0)("PrefixForRunningNoInWordFile").ToString() = "" Then
                QuestionNo.NumberType = QuestionNoType.defaultNumber
            Else
                QuestionNo.NumberType = QuestionNoType.prefixNumber
                QuestionNo.PrefixName = dtTestset.Rows(0)("PrefixForRunningNoInWordFile")

                ColWidthType1 = 8
                ColWidthType2 = 6
            End If
        End If

        'GenFooter
        GenFooter(document, section)

        If TestSetName.Length > 255 Then
            TestSetName = TestSetName.Substring(0, 255)
        End If
        Dim FileName As String = ""
        'แยกว่าเป็นการเลือกโหลดแบบไหน (บางส่วน,แบบเต็ม,แบบมีเฉลย)
        If TypeToGenWord = GenType.SomeExam Then
            GenSomeOrFullExam(document, section, True, False, False)
            FileName = "Preview_" & TestSetName & ".doc"
        ElseIf TypeToGenWord = GenType.FullExam Then
            GenSomeOrFullExam(document, section, False, False, False)
            FileName = "Full_" & TestSetName & ".doc"
        ElseIf TypeToGenWord = GenType.ShowCorrectAnswer Then
            GenSomeOrFullExam(document, section, False, True, False)
            FileName = "ShowCorrectAnswer_" & TestSetName & ".doc"
        ElseIf TypeToGenWord = GenType.ShowCorrectAnswerWithExplain Then
            GenSomeOrFullExam(document, section, False, True, True)
            FileName = "ShowCorrectAnswerWithExplain_" & TestSetName & ".doc"
        End If

        'Loop set attribute IsAutoResized = false
        LoopSetIsAutoResizedToFalse(document)

        'Save ไฟล์ Word 
        document.Save(FileName.Replace(" ", "_").Replace("/", "_"), FormatType.Doc, HttpContext.Current.Response, HttpContentDisposition.Attachment)

    End Sub

    Public Sub GenNewPDF(ByVal TypeToGenWord As GenType, Optional ByVal TestsetId As String = "")

        'ถ้ามีส่ง TestsetId เข้ามาให้ Assign เข้าไปเป็น Session เพื่อทำงานต่อไปเลย
        If TestsetId <> "" Then
            HttpContext.Current.Session("newTestSetId") = TestsetId
        End If

        'สร้าง File WordDocument ขึ้นมาใหม่
        Dim document As New WordDocument
        document.XHTMLValidateOption = DLS.XHTMLValidationType.None
        'document.EnsureMinimal()
        Dim section As WSection = TryCast(document.AddSection(), WSection)
        'ตั้งค่าหน้ากระดาษ
        section.PageSetup.Margins.Top = 42.5
        section.PageSetup.Margins.Bottom = 42.5
        section.PageSetup.Margins.Left = 58
        section.PageSetup.Margins.Right = 58 'Get Font-Size Text
        Dim GetFontSize As String = GetTestFontSize(HttpContext.Current.Session("newTestSetId").ToString())
        If GetFontSize = "" Then 'ถ้าไม่มี Size ให้เป็น Size ปกติ
            AllFontSize = "21.5px"
        ElseIf GetFontSize = "0" Then 'Size ตัวปกติ(สำหรับเด็กมัธยม)
            AllFontSize = "20.5px"
        ElseIf GetFontSize = "1" Then 'Size ตัวใหญ่(สำหรับเด็กประถมปลาย)
            AllFontSize = "21.5px"
        ElseIf GetFontSize = "2" Then 'Size ตัวใหญ่มาก(สำหรับเด็กประถมต้น)
            AllFontSize = "23.5px"
        End If

        'GenWaterMark
        GenWaterMark(document)

        'GenHeader
        Dim CountAmount As String = GetExamAmount(HttpContext.Current.Session("newTestSetId").ToString())
        Dim dtTestset As DataTable = GetTestsetDetail(HttpContext.Current.Session("newTestSetId").ToString())

        Dim TestSetName As String = dtTestset.Rows(0)("TestSet_Name")
        Dim TestSetTime As String = dtTestset.Rows(0)("TestSet_Time")
        GenHeader(document, section, TestSetName, CountAmount, TestSetTime)

        QuestionNo = New QuestionNumber(CountAmount)
        If Not EnablePrefixForRunningNoInWordFile Then
            QuestionNo.NumberType = QuestionNoType.defaultNumber
        Else
            If dtTestset.Rows(0)("PrefixForRunningNoInWordFile") Is DBNull.Value OrElse dtTestset.Rows(0)("PrefixForRunningNoInWordFile").ToString() = "" Then
                QuestionNo.NumberType = QuestionNoType.defaultNumber
            Else
                QuestionNo.NumberType = QuestionNoType.prefixNumber
                QuestionNo.PrefixName = dtTestset.Rows(0)("PrefixForRunningNoInWordFile")

                ColWidthType1 = 8
                ColWidthType2 = 6
            End If
        End If

        'GenFooter
        GenFooter(document, section)

        If TestSetName.Length > 255 Then
            TestSetName = TestSetName.Substring(0, 255)
        End If
        Dim FileName As String = ""
        'แยกว่าเป็นการเลือกโหลดแบบไหน (บางส่วน,แบบเต็ม,แบบมีเฉลย)
        If TypeToGenWord = GenType.SomeExam Then
            GenSomeOrFullExam(document, section, True, False, False)
            FileName = "Preview_" & TestSetName & ".pdf"
        ElseIf TypeToGenWord = GenType.FullExam Then
            GenSomeOrFullExam(document, section, False, False, False)
            FileName = "Full_" & TestSetName & ".pdf"
        ElseIf TypeToGenWord = GenType.ShowCorrectAnswer Then
            GenSomeOrFullExam(document, section, False, True, False)
            FileName = "ShowCorrectAnswer_" & TestSetName & ".pdf"
        ElseIf TypeToGenWord = GenType.ShowCorrectAnswerWithExplain Then
            GenSomeOrFullExam(document, section, False, True, True)
            FileName = "ShowCorrectAnswerWithExplain_" & TestSetName & ".pdf"
        End If

        'Loop set attribute IsAutoResized = false
        LoopSetIsAutoResizedToFalse(document)

        Dim pdfCoverter As New DocToPDFConverter()
        Dim pdfDoc As New PdfDocument(PdfConformanceLevel.Pdf_A1B)
        pdfDoc = pdfCoverter.ConvertToPDF(document)

        pdfDoc.Save(FileName.Replace(" ", "_").Replace("/", "_"), HttpContext.Current.Response, HttpContentDisposition.Attachment)



    End Sub

    '    Private Sub Convert_WordDoc_to_PDF(DocPath As String, sDests As String, sDestsPDFFile As String)
    '        On Error GoTo ErrorHandler
    '        Dim objWord As Word.Application
    '        Dim objWordDoc As Word.Document
    'Set objWord = CreateObject("Word.Application")
    'objWord.Visible = True
    'Set objWordDoc = objWord.Documents.Open(DocPath)
    'objWordDoc.ExportAsFixedFormat OutputFileName:=
    '        sDests & sDestsPDFFile, ExportFormat:=wdExportFormatPDF,
    '        OpenAfterExport:=False, OptimizeFor:=wdExportOptimizeForPrint, Range:=
    '        wdExportAllDocument, From:=1, To:=1, Item:=wdExportDocumentContent,
    '        IncludeDocProps:=True, KeepIRM:=True, CreateBookmarks:=
    '        wdExportCreateNoBookmarks, DocStructureTags:=True, BitmapMissingFonts:=
    '        True, UseISO19005_1:=False
    '        objWord.Quit
    '        Set objWord = Nothing
    'Exit Sub




    ''' <summary>
    ''' สำหรับ gen ดัชนีชี้วัดของแต่ละข้อ
    ''' 
    ''' </summary>
    Public Sub GenIndexIndicationsDocument()
        'สร้าง File WordDocument ขึ้นมาใหม่
        'Dim document As New WordDocument
        'document.XHTMLValidateOption = DLS.XHTMLValidationType.None
        ''document.EnsureMinimal()
        'Dim section As WSection = TryCast(document.AddSection(), WSection)
        ''ตั้งค่าหน้ากระดาษ
        'section.PageSetup.Margins.Top = 42.5
        'section.PageSetup.Margins.Bottom = 42.5
        'section.PageSetup.Margins.Left = 58
        'section.PageSetup.Margins.Right = 58

        ''Get Font-Size Text
        'Dim GetFontSize As String = GetTestFontSize(HttpContext.Current.Session("newTestSetId").ToString())
        'If GetFontSize = "" Then 'ถ้าไม่มี Size ให้เป็น Size ปกติ
        '    AllFontSize = "21.5px"
        'ElseIf GetFontSize = "0" Then 'Size ตัวปกติ(สำหรับเด็กมัธยม)
        '    AllFontSize = "20.5px"
        'ElseIf GetFontSize = "1" Then 'Size ตัวใหญ่(สำหรับเด็กประถมปลาย)
        '    AllFontSize = "21.5px"
        'ElseIf GetFontSize = "2" Then 'Size ตัวใหญ่มาก(สำหรับเด็กประถมต้น)
        '    AllFontSize = "23.5px"
        'End If

        ''GenWaterMark
        'GenWaterMark(document)

        ''GenHeader
        'Dim CountAmount As String = GetExamAmount(HttpContext.Current.Session("newTestSetId").ToString())

        'Dim dtTestset As DataTable = GetTestsetDetail(HttpContext.Current.Session("newTestSetId").ToString())
        'Dim TestSetName As String = dtTestset.Rows(0)("TestSet_Name")
        'Dim TestSetTime As String = dtTestset.Rows(0)("TestSet_Time")
        'GenHeader(document, section, TestSetName, CountAmount, TestSetTime)

        ''GenFooter
        'GenFooter(document, section)

        'If TestSetName.Length > 255 Then
        '    TestSetName = TestSetName.Substring(0, 255)
        'End If

        'GenIndicators(document, section)

        'Dim FileName As String = String.Format("IndexIndicatios_{0}.doc", TestSetName)

        ''Loop set attribute IsAutoResized = false
        'LoopSetIsAutoResizedToFalse(document)

        ''Save ไฟล์ Word 
        'document.Save(FileName.Replace(" ", "_").Replace("/", "_"), FormatType.Doc, HttpContext.Current.Response, HttpContentDisposition.Attachment)
    End Sub

    Private Sub GenIndicators(document As WordDocument, section As WSection)
        'Dim QsetNameParagraph As IWParagraph = section.AddParagraph()
        'QsetNameParagraph.ParagraphFormat.Keep = True

        'Dim dtQuestionsWithIndicators As DataTable = GetQuestionsWithIndicatos()
        'Dim QuestionAmount As String = GetExamAmount(HttpContext.Current.Session("newTestSetId").ToString())

        'Dim htmlTemp As New StringBuilder()
        'htmlTemp.Append("<table>")
        'htmlTemp.Append("<tr><td>ข้อที่</td><td>ดัชนีชี้วัดที่เกี่ยวข้อง</td></tr>")


        'For i As Integer = 1 To QuestionAmount
        '    Dim a As IEnumerable = dtQuestionsWithIndicators.AsEnumerable().Where(Function(r) r.Field(Of Integer)("QuestionNo") = i)

        '    Dim dt As DataTable '= dtQuestionsWithIndicators.AsEnumerable().Where(Function(r) r.Field(Of Integer)("QuestionNo") = i)

        '    Dim QuestionsIndicatorsTable As String = GenWordHelper.Indicators(dt)
        '    htmlTemp.Append(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", i, QuestionsIndicatorsTable))
        'Next

        'htmlTemp.Append("</table>")
        'Try
        '    QsetNameParagraph.AppendHTML(htmlTemp.ToString())
        'Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
        '    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        'End Try
    End Sub

    Private Function GetQuestionsWithIndicatos() As DataTable
        'Dim sql As New StringBuilder()
        'sql.Append("select c.qno as QuestionNo, en.EI_Code as ParentEI_Code,en.EI_Position as ParentEI_Position,en.EI_Position as ParentEI_Position,en.EI_Id As ParentEI_Id, ")
        'sql.Append("         c.eeEI_Code as Child1EI_Code,c.eeEI_Name as Child1EI_Name,c.eeEI_Position as Child1EI_Position,c.eeEI_Id as Child1EI_Id, ")
        'sql.Append(" c.eEI_Code As Child2EI_Code, c.eEI_Name As Child2EI_Name, c.eEI_Position As Child2EI_Position, c.eEI_Id As Child2EI_Id, ")
        'sql.Append(" c.EI_Code,c.EI_Name,c.EI_Position,c.EI_Id from  ")
        'sql.Append("(select b.*, ee.EI_Code As eeEI_Code, ee.EI_Name As eeEI_Name, ee.Parent_Id As eeParent_Id, ee.EI_Position As eeEI_Position, ee.EI_Id as eeEI_Id from  ")
        'sql.Append(" (select a.qno,a.EI_Code,a.EI_Name,a.EI_Id,a.Parent_Id,a.EI_Position,e.EI_Code as eEI_Code,e.EI_Id as eEI_Id,e.EI_Name as eEI_Name,e.Parent_Id as eParent_Id,e.EI_Position as eEI_Position from ")
        'sql.Append("(select tsqd.TSQD_No as qno, ein.* From tblQuestionEvaluationIndexItem qeii inner Join tblEvaluationIndexNew ein on qeii.EI_Id = ein.EI_Id inner Join tblTestSetQuestionDetail tsqd  ")
        'sql.Append(" on qeii.Question_Id = tsqd.Question_Id  where  qeii.isactive = 1 and TSQS_Id = '12C21FE2-39D7-4B5D-9A8B-C718272852F2' )  a inner join tblEvaluationIndexNew e on a.Parent_Id = e.EI_Id) b   ")
        'sql.Append("  inner join tblEvaluationIndexNew ee On b.eParent_Id = ee.EI_Id) c ")
        'sql.Append("  left join tblEvaluationIndexNew en on c.eeParent_Id = en.EI_Id  order by qno,  case when en.EI_Position is null then 2 end,eeEI_Position,eEI_Position,EI_Position;")
        'Return _DB.getdata(sql.ToString())
    End Function


    ''' <summary>
    ''' Function ที่ สร้าง Style ต่างๆให้กับ object Paragraph ของไฟล์ Word เช่น การ ตัวหนา,ขนาดตัวอักษร,สีตัวอักษร,ตำแหน่งให้ชิดซ้าย กลาง ขวา
    ''' สามารถนำ Style ที่สร้างนี้ไปใช้กับ Paragraph ต่างๆได้
    ''' </summary>
    ''' <param name="document">Object ของไฟล์ Word</param>
    ''' <param name="StyleName">ชื่อ Style</param>
    ''' <param name="FontName">ชื่อของชนิดตัวอักษร</param>
    ''' <param name="FontSize">ขนาดตัวอักษร</param>
    ''' <param name="FontColor">สีของตัวอักษร</param>
    ''' <param name="PositionWord">ตำแหน่งของข้อความ</param>
    ''' <param name="FontIsBold">ต้องการให้เป็นตัวหนาหรือเปล่า</param>
    ''' <remarks></remarks>
    Public Sub GenStyle(ByVal document As WordDocument, ByVal StyleName As String, ByVal FontName As String, ByVal FontSize As Double, _
                    ByVal FontColor As Color, ByVal PositionWord As SetPositionWord, Optional ByVal FontIsBold As Boolean = False)

        'Function สำหรับ Set Style เพื่อเอาไปใช้กับ Paragraph 
        Dim NewStyle As WParagraphStyle = TryCast(document.AddParagraphStyle(StyleName), WParagraphStyle)
        NewStyle.CharacterFormat.FontName = FontName
        NewStyle.CharacterFormat.FontSize = FontSize
        NewStyle.CharacterFormat.TextColor = FontColor
        NewStyle.ParagraphFormat.Keep = True
        If PositionWord = SetPositionWord.Left Then
            NewStyle.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left
        ElseIf PositionWord = SetPositionWord.Right Then
            NewStyle.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Right
        ElseIf PositionWord = SetPositionWord.Center Then
            NewStyle.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center
        End If

        If FontIsBold = True Then
            NewStyle.CharacterFormat.Bold = True
        End If

    End Sub

    ''' <summary>
    ''' ทำการหาข้อมูลข้อสอบต่างๆแล้วทำการส่งเข้า Function ย่อยๆที่ไป Gen คำถาม-คำตอบ ตามชนิดของข้อสอบอีกที
    ''' </summary>
    ''' <param name="document">Object ของไฟล์ Word</param>
    ''' <param name="section">Section ของ Object ไฟล์ Word</param>
    ''' <param name="IsSomeExam">เป็นแบบตัวอย่างบางส่วนใช่ไหม ?</param>
    ''' <param name="IsShowCorrectAnswer">แสดงเฉลยไหม ?</param>
    ''' <remarks></remarks>
    Public Sub GenSomeOrFullExam(ByVal document As WordDocument, ByVal section As WSection, ByVal IsSomeExam As Boolean, ByVal IsShowCorrectAnswer As Boolean)

        'Gen Style คำถามคำตอบ
        GenStyle(document, "QsetStyle", "Angsana New", 16, Color.Black, SetPositionWord.Left, True)
        GenStyle(document, "QuestionAndAnswerStyle", "Angsana New", 16, Color.Black, SetPositionWord.Left)
        Dim dt As New DataTable
        Dim SubjectName As String = ""
        dt = ClsPDF.GetTSQS(HttpContext.Current.Session("newTestSetId").ToString(), IsSomeExam)
        If dt.Rows.Count > 0 Then
            'loop เพื่อ Gen คำถาม - คำตอบ ทีละข้อ แบ่งตามชนิดของข้อสอบ , เงื่อนไขการจบ loop คือ วนจนกว่าจะครบทุกข้อคำถาม
            For row = 0 To dt.Rows.Count - 1
                SubjectName = GetSubjectNameByQsetId(dt.Rows(row)("QSet_Id").ToString())
                'ต้องเช็คไว้ก่อนว่าถ้าเป็นภาษาอังกฤษ ต้องเปลี่ยนพวก คำนำหน้าคำถามให้เป็น ภาษาอังกฤษ เช่น ก,ข,ค เป็น A,B,C
                If SubjectName = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ" Then
                    HttpContext.Current.Session("IsEngExam") = True
                Else
                    HttpContext.Current.Session("IsEngExam") = False
                End If
                'ทำการแบ่งตามชนิดข้อสอบ 1 = แบบตัวเลือก , 2 = ถูก-ผิด , 3 = จบคู่ , 6 = เรียงลำดับ
                If row = 0 Then
                    If dt.Rows(row)("QSet_Type") = 1 Then
                        GenType1(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, True)
                    ElseIf dt.Rows(row)("QSet_Type") = 2 Then
                        GenType2(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, True)
                    ElseIf dt.Rows(row)("QSet_Type") = 3 Then
                        GenType3(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, True, False) ' add value ทิ้งไว้ก่อน เพราะยกไป method ใหม่แล้ว
                    ElseIf dt.Rows(row)("QSet_Type") = 6 Then
                        GenType6(section, dt.Rows(row)("Qset_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, True)
                    End If
                Else
                    If dt.Rows(row)("QSet_Type") = 1 Then
                        GenType1(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, False)
                    ElseIf dt.Rows(row)("QSet_Type") = 2 Then
                        GenType2(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, False)
                    ElseIf dt.Rows(row)("QSet_Type") = 3 Then
                        GenType3(section, dt.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, False, False) ' add value ทิ้งไว้ก่อน เพราะยกไป method ใหม่แล้ว
                    ElseIf dt.Rows(row)("QSet_Type") = 6 Then
                        GenType6(section, dt.Rows(row)("Qset_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, False)
                    End If
                End If
            Next
        End If

    End Sub

    Public Sub GenSomeOrFullExam(ByVal document As WordDocument, ByVal section As WSection, ByVal IsSomeExam As Boolean, ByVal IsShowCorrectAnswer As Boolean, ByVal WithExplain As Boolean)

        'Gen Style คำถามคำตอบ
        GenStyle(document, "QsetStyle", "Angsana New", 16, Color.Black, SetPositionWord.Left, True)
        GenStyle(document, "QuestionAndAnswerStyle", "Angsana New", 16, Color.Black, SetPositionWord.Left)

        Dim dtQsets As DataTable = ClsPDF.GetTSQS(HttpContext.Current.Session("newTestSetId").ToString(), IsSomeExam)
        Dim SubjectName As String = ""

        If dtQsets.Rows.Count > 0 Then
            'loop qset
            For row = 0 To dtQsets.Rows.Count - 1

                SubjectName = GetSubjectNameByQsetId(dtQsets.Rows(row)("QSet_Id").ToString())

                'ต้องเช็คไว้ก่อนว่าถ้าเป็นภาษาอังกฤษ ต้องเปลี่ยนพวก คำนำหน้าคำถามให้เป็น ภาษาอังกฤษ เช่น ก,ข,ค เป็น A,B,C
                HttpContext.Current.Session("IsEngExam") = If(SubjectName = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ", True, False)

                'ทำการแบ่งตามชนิดข้อสอบ 1 = แบบตัวเลือก , 2 = ถูก-ผิด , 3 = จบคู่ , 6 = เรียงลำดับ
                Dim IsFirstRound As Boolean = If(row = 0, True, False)

                If dtQsets.Rows(row)("QSet_Type") = QuestionType.Choice Then
                    GenType1(section, dtQsets.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, IsFirstRound, WithExplain)
                ElseIf dtQsets.Rows(row)("QSet_Type") = QuestionType.RightWrong Then
                    GenType2(section, dtQsets.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, IsFirstRound, WithExplain)
                ElseIf dtQsets.Rows(row)("QSet_Type") = QuestionType.Pair Then
                    GenType3(section, dtQsets.Rows(row)("QSet_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, IsFirstRound, WithExplain)
                ElseIf dtQsets.Rows(row)("QSet_Type") = QuestionType.Sort Then
                    GenType6(section, dtQsets.Rows(row)("Qset_Id").ToString(), "QsetStyle", "QuestionAndAnswerStyle", IsSomeExam, IsShowCorrectAnswer, IsFirstRound)
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML ของข้อสอบแบบตัวเลือก
    ''' </summary>
    ''' <param name="section">Section ของ object File Word</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของชุดคำถามนี้</param>
    ''' <param name="QsetStyle">ชื่อ Style ที่จะ Set ให้กับคำอธิบายข้อสอบชุดนี้</param>
    ''' <param name="QuestionAnswerStyle">ชื่อ Style ที่จะ Set ให้กับคำถาม - คำตอบ ชุดนี้</param>
    ''' <param name="IsPreview">เป็นโหมดแบบ แสดงแค่บางส่วนหรือเปล่า ?</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="IsFirstRound">เป็น Record แรกของการ loop ? เพราะถ้าใช่ต้อง Gen คำอธิบายชุดข้อสอบนี้ด้วย</param>
    ''' <remarks></remarks>
    Public Sub GenType1(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrectAnswer As Boolean, ByVal IsFirstRound As Boolean)
        Dim dt2 As New DataTable
        dt2 = ClsPDF.GetQuestion(HttpContext.Current.Session("newTestSetId").ToString(), QsetId, IsPreview)
        If dt2.Rows.Count > 0 Then
            'loop เพื่อ Gen คำถามทุกข้อตามโครงสร้างของคำถาม แบบ ตัวเลือก , เงื่อนไขการจบ loop คือ ทำจนครบทุกคำถามที่อยู่ใน datatable
            For index = 0 To dt2.Rows.Count - 1
                'ถ้าเป็นการ loop รอบแรกต้องทำการ Gen รายละเอียดของชุดคำถามนี้ลงไปด้วย
                If index = 0 Then
                    'หาจำนวนของข้อสอบทั้งหมดในชุดคำถามนี้
                    Dim TotalQuestionInQsetId As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
                    'ทำการ Gen รายละเอียดของชุดคำถามนี้ก่อน
                    GenQsetText(section, QsetStyle, dt2.Rows(0)("QSet_Name").ToString(), TotalQuestionInQsetId, IsFirstRound)
                    Dim dt3 As New DataTable
                    dt3 = ClsPDF.GetAnswerDetails(dt2.Rows(index)("Question_Id").ToString)
                    If dt3.Rows.Count > 0 Then
                        Dim questionExpain As String = If(dt2.Rows(index)("Question_Expain") Is DBNull.Value, "", dt2.Rows(index)("Question_Expain").ToString())
                        GenQuestionAndAnswerType1(section, QuestionAnswerStyle, index + 1, dt2.Rows(index)("Question_Name"), QsetId, IsShowCorrectAnswer, dt3, questionExpain)
                    End If
                Else
                    Dim dt3 As New DataTable
                    dt3 = ClsPDF.GetAnswerDetails(dt2.Rows(index)("Question_Id").ToString)
                    If dt3.Rows.Count > 0 Then
                        Dim questionExpain As String = If(dt2.Rows(index)("Question_Expain") Is DBNull.Value, "", dt2.Rows(index)("Question_Expain").ToString())
                        GenQuestionAndAnswerType1(section, "QuestionAndAnswerStyle", index + 1, dt2.Rows(index)("Question_Name"), QsetId, IsShowCorrectAnswer, dt3, questionExpain)
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub GenType1(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrectAnswer As Boolean, ByVal IsFirstRound As Boolean, ByVal WithExplain As Boolean)
        Dim dtQuestions As DataTable = ClsPDF.GetQuestion(HttpContext.Current.Session("newTestSetId").ToString(), QsetId, IsPreview)
        If dtQuestions.Rows.Count > 0 Then
            'loop เพื่อ Gen คำถามทุกข้อตามโครงสร้างของคำถาม แบบ ตัวเลือก , เงื่อนไขการจบ loop คือ ทำจนครบทุกคำถามที่อยู่ใน datatable
            For index = 0 To dtQuestions.Rows.Count - 1
                'ถ้าเป็นการ loop รอบแรกต้องทำการ Gen รายละเอียดของชุดคำถามนี้ลงไปด้วย
                If index = 0 Then
                    'หาจำนวนของข้อสอบทั้งหมดในชุดคำถามนี้
                    Dim TotalQuestionInQsetId As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
                    'ทำการ Gen รายละเอียดของชุดคำถามนี้ก่อน
                    GenQsetText(section, QsetStyle, dtQuestions.Rows(0)("QSet_Name").ToString(), TotalQuestionInQsetId, IsFirstRound)
                End If

                Dim dt3 As DataTable = ClsPDF.GetAnswerDetails(dtQuestions.Rows(index)("Question_Id").ToString)
                If dt3.Rows.Count > 0 Then
                    Dim questionExpain As String = ""
                    If WithExplain Then
                        questionExpain = If(dtQuestions.Rows(index)("Question_Expain") Is DBNull.Value, "", dtQuestions.Rows(index)("Question_Expain").ToString())
                        questionExpain = ReplaceString(QsetId, questionExpain, GetUrl)
                    End If
                    GenQuestionAndAnswerType1(section, QuestionAnswerStyle, QuestionNo.Number(), dtQuestions.Rows(index)("Question_Name"), QsetId, IsShowCorrectAnswer, dt3, questionExpain)
                End If
                QuestionNo.IncreaseOneValue()
            Next
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML ของข้อสอบแบบ ถูก-ผิด
    ''' </summary>
    ''' <param name="section">Section ของ object File Word</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของชุดคำถามนี้</param>
    ''' <param name="QsetStyle">ชื่อ Style ที่จะ Set ให้กับคำอธิบายข้อสอบชุดนี้</param>
    ''' <param name="QuestionAnswerStyle">ชื่อ Style ที่จะ Set ให้กับคำถาม - คำตอบ ชุดนี้</param>
    ''' <param name="IsPreview">เป็นโหมดแบบ แสดงแค่บางส่วนหรือเปล่า ?</param>
    ''' <param name="IsShowCorrectAsnwer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="IsFirstRound">เป็น Record แรกของการ loop ? เพราะถ้าใช่ต้อง Gen คำอธิบายชุดข้อสอบนี้ด้วย</param>
    ''' <remarks></remarks>
    Public Sub GenType2(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrectAsnwer As Boolean, ByVal IsFirstRound As Boolean)
        Dim dt2 As New DataTable
        'ถ้าเลือกแบบแสดงเฉลยด้วยต้องหาข้อมูลแบบมีเฉลยมาด้วย
        If IsShowCorrectAsnwer = True Then
            dt2 = CreatedtType2ShowCorrect(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
        Else
            dt2 = ClsPDF.GetQuestion(HttpContext.Current.Session("newTestSetId").ToString(), QsetId, IsPreview)
        End If

        If dt2.Rows.Count > 0 Then
            'loop เพื่อทำการ Gen คำถาม-คำตอบ แบบถูก-ผิด เข้าไปใน File Word , เงื่อนไขการจบ loop คือวนจนครบทุกข้อ
            For index = 0 To dt2.Rows.Count - 1
                If index = 0 Then
                    'ทำการหาจำนวนข้อสอบทั้งหมดของชุดนี้ก่อน
                    Dim TotalQuestionInQsetId As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
                    'ทำการ Gen รายละเอียดของชุดคำถามนี้
                    GenQsetText(section, QsetStyle, dt2.Rows(0)("QSet_Name").ToString(), TotalQuestionInQsetId, IsFirstRound)
                    If IsShowCorrectAsnwer = True Then
                        Dim answerExpain As String = If(dt2.Rows(index)("Answer_Expain") Is DBNull.Value, "", dt2.Rows(index)("Answer_Expain").ToString())
                        answerExpain = ReplaceString(QsetId, answerExpain, GetUrl)
                        'GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", index + 1, dt2.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer, dt2.Rows(index)("Answer_Name"), answerExpain)
                    Else
                        GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", index + 1, dt2.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer)
                    End If
                Else
                    If IsShowCorrectAsnwer = True Then
                        Dim answerExpain As String = If(dt2.Rows(index)("Answer_Expain") Is DBNull.Value, "", dt2.Rows(index)("Answer_Expain").ToString())
                        ' GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", index + 1, dt2.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer, dt2.Rows(index)("Answer_Name"), answerExpain)
                        answerExpain = ReplaceString(QsetId, answerExpain, GetUrl)
                    Else
                        GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", index + 1, dt2.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer)
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub GenType2(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrectAsnwer As Boolean, ByVal IsFirstRound As Boolean, ByVal WithExplain As Boolean)
        Dim dtQuestions As DataTable = If(IsShowCorrectAsnwer = True, CreatedtType2ShowCorrect(QsetId, HttpContext.Current.Session("newTestSetId").ToString()), ClsPDF.GetQuestion(HttpContext.Current.Session("newTestSetId").ToString(), QsetId, IsPreview))

        If dtQuestions.Rows.Count > 0 Then

            Dim sb As New StringBuilder
            Dim QuestionAndAnswerPara As WParagraph

            'loop เพื่อทำการ Gen คำถาม-คำตอบ แบบถูก-ผิด เข้าไปใน File Word , เงื่อนไขการจบ loop คือวนจนครบทุกข้อ
            For index = 0 To dtQuestions.Rows.Count - 1
                If index = 0 Then
                    'ทำการหาจำนวนข้อสอบทั้งหมดของชุดนี้ก่อน
                    Dim TotalQuestionInQsetId As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
                    'ทำการ Gen รายละเอียดของชุดคำถามนี้
                    GenQsetText(section, QsetStyle, dtQuestions.Rows(0)("QSet_Name").ToString(), TotalQuestionInQsetId, IsFirstRound)

                    sb.Append("<table style='width:100%;'>")
                    QuestionAndAnswerPara = section.AddParagraph()
                End If

                If IsShowCorrectAsnwer = True Then
                    Dim answerExpain As String = ""
                    If WithExplain Then
                        answerExpain = If(dtQuestions.Rows(index)("Answer_Expain") Is DBNull.Value, "", dtQuestions.Rows(index)("Answer_Expain").ToString())
                        answerExpain = ReplaceString(QsetId, answerExpain, GetUrl)
                    End If
                    GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", QuestionNo.Number(), dtQuestions.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer, sb, dtQuestions.Rows(index)("Answer_Name"), answerExpain)
                Else
                    GenQuestionAndAnswerType2(section, "QuestionAndAnswerStyle", QuestionNo.Number(), dtQuestions.Rows(index)("Question_Name").ToString(), QsetId, IsShowCorrectAsnwer)
                End If
                QuestionNo.IncreaseOneValue()
            Next

            sb.Append("</table>")

            Try
                QuestionAndAnswerPara.AppendHTML(sb.ToString())
                SetKeepFollowForAppendHtml(QuestionAndAnswerPara)
                'RemoveEmptyParagraph(QuestionAndAnswerPara)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
            QuestionAndAnswerPara.ApplyStyle("QuestionAndAnswerStyle")
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML ของข้อสอบแบบ จับคู่
    ''' </summary>
    ''' <param name="section">Section ของ object File Word</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของชุดคำถามนี้</param>
    ''' <param name="QsetStyle">ชื่อ Style ที่จะ Set ให้กับคำอธิบายข้อสอบชุดนี้</param>
    ''' <param name="QuestionAnswerStyle">ชื่อ Style ที่จะ Set ให้กับคำถาม - คำตอบ ชุดนี้</param>
    ''' <param name="IsPreview">เป็นโหมดแบบ แสดงแค่บางส่วนหรือเปล่า ?</param>
    ''' <param name="IsShowCorrect">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="IsFirstRound">เป็น Record แรกของการ loop ? เพราะถ้าใช่ต้อง Gen คำอธิบายชุดข้อสอบนี้ด้วย</param>
    ''' <remarks></remarks>
    Public Sub GenType3(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrect As Boolean, ByVal IsFirstRound As Boolean, ByVal WithExplain As Boolean)
        Dim dtType3 As DataTable
        Dim dtOriginal As New DataTable

        If IsShowCorrect = True Then
            'กรณีที่มีการแสดงเฉลยต้องหา dt ของชุดที่มันคู่กันแบบถูกๆมาเลยด้วย
            dtType3 = CreatedtType3ShowCorrect(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
            dtOriginal = CreatedtForType3(QsetId, HttpContext.Current.Session("newTestSetId").ToString(), IsPreview)
        Else
            dtType3 = CreatedtForType3(QsetId, HttpContext.Current.Session("newTestSetId").ToString(), IsPreview)
        End If

        If dtType3.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            sb.Append("<table style='width:100%;'>")
            Dim QsetName As String = GetQsetName(QsetId)

            Dim questionExpain As String = ""
            If WithExplain Then
                questionExpain = GetQuestionExpainType3(QsetId) 'เพิ่มคำอธิบายคำตอบเอามาจาก tblquestion ข้อที่1 มาแปะให้ qset
            End If

            Dim TotalquestionInSet As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
            GenQsetText(section, QsetStyle, QsetName, TotalquestionInSet, IsFirstRound, questionExpain)
            'loop เพื่อ Gen คำถาม-คำตอบ สำหรับคำถามแบบ จับคู่ , เงื่อนไขการจบ loop คือ วนจนครบหมดทุกคำถาม
            For index = 0 To dtType3.Rows.Count - 1
                'ถ้าเป็นโหมดให้แสดงเฉลย ต้องทำการวน loop dt คำตอบที่ถูกต้องเพื่อหาว่าคำถามข้อนี้อยู่คู่กับ คำตอบข้อไหน
                If IsShowCorrect = True Then
                    Dim IndexLeadingText As Integer = 0
                    'loop dt คำตอบที่ถูกต้องเพื่อหาว่าคำถามข้อนี้อยู่คู่กับ คำตอบข้อไหน แล้วทำการ Gen คำตอบที่ถูกต้อง , เงื่อนไขการจบ loop คือ เจอคำตอบที่เป็นคู่กับคำถามข้อนี้
                    For row = 0 To dtOriginal.Rows.Count - 1
                        If dtOriginal.Rows(row)("Answer_Name") = dtType3.Rows(index)("Answer_Name") Then
                            IndexLeadingText = row
                            Dim answerExpain As String = ""

                            If WithExplain Then
                                answerExpain = If(dtOriginal.Rows(index)("Answer_Expain") Is DBNull.Value, "", dtOriginal.Rows(index)("Answer_Expain").ToString())
                                answerExpain = ReplaceString(QsetId, answerExpain, GetUrl)
                            End If

                            GenQuestionAndAnswerType3(section, index + 1, dtOriginal.Rows(index)("Answer_Name"), dtOriginal.Rows(index)("Question_Name"), QsetId, sb, IsShowCorrect, IndexLeadingText, answerExpain)
                            Exit For
                        End If
                    Next
                Else
                    'แต่ถ้าไม่ได้เป็นโหมดแสดงเฉลยก็ให้ Gen คำตอบที่สลับขึ้นมาจาก dtType3 ได้เลย
                    GenQuestionAndAnswerType3(section, index + 1, dtType3.Rows(index)("Answer_Name"), dtType3.Rows(index)("Question_Name"), QsetId, sb, IsShowCorrect)
                End If
                QuestionNo.IncreaseOneValue()
            Next
            sb.Append("</table>")
            Dim ParaType3 As WParagraph = section.AddParagraph()
            ParaType3.ApplyStyle(QuestionAnswerStyle)
            Try
                ParaType3.AppendHTML(sb.ToString())
                SetKeepFollowForAppendHtml(ParaType3)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML ของข้อสอบแบบ เรียงลำดับ
    ''' </summary>
    ''' <param name="section">Section ของ object File Word</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของชุดคำถามนี้</param>
    ''' <param name="QsetStyle">ชื่อ Style ที่จะ Set ให้กับคำอธิบายข้อสอบชุดนี้</param>
    ''' <param name="QuestionAnswerStyle">ชื่อ Style ที่จะ Set ให้กับคำถาม - คำตอบ ชุดนี้</param>
    ''' <param name="IsPreview">เป็นโหมดแบบ แสดงแค่บางส่วนหรือเปล่า ?</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="IsFirstRound">เป็น Record แรกของการ loop ? เพราะถ้าใช่ต้อง Gen คำอธิบายชุดข้อสอบนี้ด้วย</param>
    ''' <remarks></remarks>
    Public Sub GenType6(ByVal section As WSection, ByVal QsetId As String, ByVal QsetStyle As String, ByVal QuestionAnswerStyle As String, ByVal IsPreview As Boolean, ByVal IsShowCorrectAnswer As Boolean, ByVal IsFirstRound As Boolean)
        Dim dtType6 As DataTable = CreatedtType6(QsetId, HttpContext.Current.Session("newTestSetId").ToString(), IsPreview, IsShowCorrectAnswer)
        If dtType6.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            sb.Append("<table style='width:100%;'>")
            Dim TotalquestionInSet As String = GetQuestionQuantityInQset(QsetId, HttpContext.Current.Session("newTestSetId").ToString())
            GenQsetText(section, QsetStyle, dtType6.Rows(0)("Qset_Name"), TotalquestionInSet, IsFirstRound)
            'loop เพื่อทำการนำ QuestionName มา Gen เป็นคำตอบ , เงื่อนไขการจบ loop คือ ทำจนครบทุกข้อ
            For index = 0 To dtType6.Rows.Count - 1
                If IsShowCorrectAnswer = True Then
                    GenQuestionAndAnswerType6(section, index + 1, dtType6.Rows(index)("Question_Name"), QsetId, sb, IsShowCorrectAnswer, dtType6.Rows(index)("Answer_Name"))
                Else
                    GenQuestionAndAnswerType6(section, index + 1, dtType6.Rows(index)("Question_Name"), QsetId, sb, IsShowCorrectAnswer)
                End If
            Next
            Dim QuestionAndAnswerPara As WParagraph = section.AddParagraph()
            QuestionAndAnswerPara.ApplyStyle(QuestionAnswerStyle)
            sb.Append("</table>")
            Try
                QuestionAndAnswerPara.AppendHTML(sb.ToString())
                SetKeepFollowForAppendHtml(QuestionAndAnswerPara)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML คำถาม-คำตอบ ในรูปแบบตัวเลือก
    ''' </summary>
    ''' <param name="section">section ของ object File Word</param>
    ''' <param name="StyleName">Style ที่จะเอามาใช้กับ คำถาม-คำตอบ</param>
    ''' <param name="QuestionNumber">ข้อที่</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="QsetId">Id ของ tblQeustionSet ของคำถามข้อนี้</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="dtAnswer">Datatable คำตอบ</param>
    ''' <remarks></remarks>
    Public Sub GenQuestionAndAnswerType1(ByVal section As WSection, ByVal StyleName As String, ByVal QuestionNumber As String, ByVal QuestionName As String, ByVal QsetId As String, ByVal IsShowCorrectAnswer As Boolean, ByVal dtAnswer As DataTable, ByVal Question_Expain As String)

        'Function สำหรับ Gen ข้อสอบสำหรับข้อสอบแบบ ปรนัย
        Dim sb As New StringBuilder
        sb.Append("<table style='width:100%;'><tr><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:" & ColWidthType1 & "%;vertical-align:top;'>")
        'สร้าง Paragraph ของข้อนี้
        Dim QuestionAnswerPara As IWParagraph = section.AddParagraph()
        Dim QuestionNameComplete As String = ReplaceString(QsetId, QuestionName, GetUrl)
        QuestionNameComplete = QuestionNumber & ". </td><td colspan='2' style='font-family:Angsana New;font-size:" & AllFontSize & ";width:96%;vertical-align:top;'>" & QuestionNameComplete
        sb.Append(QuestionNameComplete)

        If Question_Expain <> "" Then
            sb.Append(String.Format("<table style='font-family:Angsana New;font-size:" & AllFontSize & ";width:600px;border:dashed;border-color: #afafaf;margin:5px;'><tr><td>{0}</td></tr></table>", Question_Expain))
        End If

        sb.Append("</td></tr>")
        Dim LeadingText As ArrayList
        'ทำการหา คำนำหน้า คำตอบ ว่าต้องเป็นภาษาอังกฤษ หรือ ภาษาไทย
        If HttpContext.Current.Session("IsEngExam") = True Then
            LeadingText = GenArrayLeadingEngText()
        Else
            LeadingText = GenArrayLeadingText()
        End If
        'เป็นตัวแปรที่นำมาหาค่าคำนำหน้าคำตอบใน Array ว่าต้องเป็น ก,ข,ค หรือ A,B,C
        Dim Count As Integer = 0
        'loop เพื่อต่อสตริงคำตอบแต่ละข้อของคำถามข้อนี้ , เงื่อนไขการจบ loop คือ วนคำตอบจนหมดทุกข้อ
        For Each r In dtAnswer.Rows
            sb.Append("<tr><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:5%;vertical-align:top;'>")
            Dim AnswerNameComplete As String = ReplaceString(QsetId, r("Answer_Name").ToString(), GetUrl)
            'ถ้ามันเป็นโหมดแบบแสดงเฉลยด้วย ต้องเทียบด้วยว่าคำตอบข้อนี้ถูกหรือเปล่า ถ้าถูกให้ทำการเติมรูปเครื่องหมายถูกเข้าไปหน้า คำตอบข้อนี้
            If IsShowCorrectAnswer = True Then
                If r("Answer_Score") > 0 Then
                    AnswerNameComplete = "<img src='" & GetUrl & "Images/right.png' /></td>" & "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & LeadingText(Count) & ".</td>" & "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:93%;vertical-align:top;'>" & AnswerNameComplete
                Else
                    AnswerNameComplete = "</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & LeadingText(Count) & ".</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:92%;vertical-align:top;'>" & AnswerNameComplete
                End If

                If Question_Expain <> "" Then
                    Dim answerExpain As String = If(r("Answer_Expain") Is DBNull.Value, "", r("Answer_Expain").ToString())
                    If answerExpain <> "" And r("Answer_Score") > 0 And BusinessTablet360.ClsKNSession.RunMode <> "wordonly" Then
                        AnswerNameComplete &= String.Format("<table style='font-family:Angsana New;font-size:" & AllFontSize & ";width:500px;border:dashed;border-color: #afafaf;margin:5px;'><tr><td>{0}</td></tr></table>", ReplaceString(QsetId, answerExpain, GetUrl))
                    End If
                End If

            Else
                AnswerNameComplete = "</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & LeadingText(Count) & ".</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:92%;vertical-align:top;'>" & AnswerNameComplete
            End If
            sb.Append(AnswerNameComplete)
            sb.Append("</td></tr>")
            Count += 1
        Next
        sb.Append("</table>")
        Try
            QuestionAnswerPara.AppendHTML(sb.ToString())
            SetKeepFollowForAppendHtml(QuestionAnswerPara)
            'RemoveEmptyParagraph(QuestionAnswerPara)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
        QuestionAnswerPara.ApplyStyle(StyleName)

    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML คำถาม-คำตอบ ในรูปแบบ ถูก-ผิด
    ''' </summary>
    ''' <param name="section">section ของ object File Word</param>
    ''' <param name="StyleName">Style ที่จะเอามาใช้กับ คำถาม-คำตอบ</param>
    ''' <param name="QuestionNumber">ข้อที่</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="QsetId">Id ของ tblQeustionSet ของคำถามข้อนี้</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="AnswerName">เป็นเฉลยเพื่อบอกให้รู้ว่าคำถามข้อนี้ต้องตอบว่า ถูก หรือ ผิด</param>
    ''' <remarks></remarks>
    Public Sub GenQuestionAndAnswerType2(ByVal section As WSection, ByVal StyleName As String, ByVal QuestionNumber As String, ByVal QuestionName As String, _
                                         ByVal QsetId As String, ByVal IsShowCorrectAnswer As Boolean, Optional ByVal AnswerName As String = "", Optional ByVal Answer_Expain As String = "")

        'Function สำหรับ Genword แบบ ถูก - ผิด
        Dim sb As New StringBuilder
        sb.Append("<table style='width:100%;'><tr><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'>")
        'สร้าง Paragraph ของข้อนี้
        Dim QuestionAndAnswerPara As WParagraph = section.AddParagraph()
        Dim QuestionNameComplete As String = ReplaceString(QsetId, QuestionName, GetUrl)
        'ถ้ามันเป็นโหมดแบบแสดงเฉลยด้วย ต้องเทียบด้วยว่าคำตอบข้อนี้ถูกหรือเปล่า ถ้าถูกให้ทำการเติมรูปเครื่องหมายถูกเข้าไปหน้า คำตอบข้อนี้
        If IsShowCorrectAnswer = True Then
            If AnswerName = "True" Then
                QuestionNameComplete = "<u><img src='" & GetUrl & "Images/right.png' /></u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:93%;vertical-align:top;'>" & QuestionNameComplete
            ElseIf AnswerName = "False" Then
                QuestionNameComplete = "<u><img src='" & GetUrl & "Images/wrong.png' /></u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:93%;vertical-align:top;'>" & QuestionNameComplete
            End If

            If Answer_Expain <> "" Then
                QuestionNameComplete &= String.Format("<table style='width:500px;border:dashed;border-color: #afafaf;margin:5px;'><tr><td>{0}</td></tr></table>", Answer_Expain)
            End If

        Else
            QuestionNameComplete = "_____</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:90%;vertical-align:top;'>" & QuestionNameComplete
        End If
        sb.Append(QuestionNameComplete)
        sb.Append("</td></tr></table>")
        Try
            QuestionAndAnswerPara.AppendHTML(sb.ToString())
            SetKeepFollowForAppendHtml(QuestionAndAnswerPara)
            'RemoveEmptyParagraph(QuestionAndAnswerPara)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
        QuestionAndAnswerPara.ApplyStyle(StyleName)

    End Sub

    Public Sub GenQuestionAndAnswerType2(ByVal section As WSection, ByVal StyleName As String, ByVal QuestionNumber As String, ByVal QuestionName As String, _
                                       ByVal QsetId As String, ByVal IsShowCorrectAnswer As Boolean, ByRef sb As StringBuilder, Optional ByVal AnswerName As String = "", Optional ByVal Answer_Expain As String = "")


        sb.Append("<tr><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'>")
        'สร้าง Paragraph ของข้อนี้
        Dim QuestionNameComplete As String = ReplaceString(QsetId, QuestionName, GetUrl)

        'ถ้ามันเป็นโหมดแบบแสดงเฉลยด้วย ต้องเทียบด้วยว่าคำตอบข้อนี้ถูกหรือเปล่า ถ้าถูกให้ทำการเติมรูปเครื่องหมายถูกเข้าไปหน้า คำตอบข้อนี้
        If IsShowCorrectAnswer = True Then
            If AnswerName = "True" Then
                QuestionNameComplete = "<u><img src='" & GetUrl & "Images/right.png' /></u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:" & ColWidthType2 & "%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:93%;vertical-align:top;'>" & QuestionNameComplete
            ElseIf AnswerName = "False" Then
                QuestionNameComplete = "<u><img src='" & GetUrl & "Images/wrong.png' /></u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:" & ColWidthType2 & "%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:93%;vertical-align:top;'>" & QuestionNameComplete
            End If

            If Answer_Expain <> "" Then
                QuestionNameComplete &= String.Format("<table style='width:500px;border:dashed;border-color: #afafaf;margin:5px;'><tr><td>{0}</td></tr></table>", Answer_Expain)
            End If

        Else
            QuestionNameComplete = "_____</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:90%;vertical-align:top;'>" & QuestionNameComplete
        End If
        sb.Append(QuestionNameComplete)
        sb.Append("</td></tr>")
        

    End Sub


    ''' <summary>
    ''' ทำการ Gen String HTML คำถาม-คำตอบ ในรูปแบบ จับคู่
    ''' </summary>
    ''' <param name="section">section ของ object File Word</param>
    ''' <param name="QuestionNumber">ข้อที่</param>
    ''' <param name="AnswerName">คำตอบ</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="QsetId">Id ของ tblQeustionSet ของคำถามข้อนี้</param>
    ''' <param name="Sb">ตัวแปร StringBuilder ที่ทำการสร้างโครงสร้างไว้ที่ Function ก่อนหน้า ให้ส่งเข้ามาด้วยเลยเพราะคำถามแบบจับคู่เป็นข้อใหญ่ข้อเดียว ไม่ได้ gen แยกเป็นข้อๆเหมือนกับ ตัวเลือก และ ถูกผิด</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="IndexLeadingText">ส่ง index ของคำตอบที่ถูกต้องเข้ามาด้วยในกรณีที่เป็นแบบแสดงเฉลย</param>
    ''' <remarks></remarks>
    Public Sub GenQuestionAndAnswerType3(ByVal section As WSection, ByVal QuestionNumber As String, ByVal AnswerName As String, ByVal QuestionName As String, ByVal QsetId As String, ByRef Sb As StringBuilder, ByVal IsShowCorrectAnswer As Boolean, Optional ByVal IndexLeadingText As Integer = 0, Optional ByVal Answer_Expain As String = "")
        'Function Gen ข้อสอบแบบ จับคู่
        Dim LeadingText As ArrayList
        If HttpContext.Current.Session("IsEngExam") = True Then
            LeadingText = GenArrayLeadingEngText()
        Else
            LeadingText = GenArrayLeadingText()
        End If
        Sb.Append("<tr>")

        'ส่วนคำถาม
        Dim QuestionNameComplete As String = ReplaceString(QsetId, QuestionName, GetUrl)

        If IsShowCorrectAnswer = True Then
            QuestionNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'><u>" & LeadingText(IndexLeadingText) & "</u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:5%;vertical-align:top;'>" & QuestionNo.Number() & ".</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:41.5%;vertical-align:top;'>" & QuestionNameComplete
        Else
            QuestionNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'>____</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:5%;vertical-align:top;'>" & QuestionNo.Number() & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:41.5%;vertical-align:top;'>" & QuestionNameComplete
        End If
        Sb.Append(QuestionNameComplete & "</td>")

        'ส่วนคำตอบ
        Dim AnswerNameComplete As String = ReplaceString(QsetId, AnswerName, GetUrl)
        AnswerNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:6%;vertical-align:top;text-align:right;'>" & LeadingText(CInt(QuestionNumber) - 1) & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:40.5%;vertical-align:top;'>" & AnswerNameComplete
        Sb.Append(AnswerNameComplete & "</td>")
        Sb.Append("</tr>")


        If IsShowCorrectAnswer = True Then
            If Answer_Expain <> "" Then Sb.Append(String.Format("<tr><td colspan='2' style='width:12%;'></td><td style='width:88%;border:dashed;border-color: #afafaf;margin:5px;' colspan='3'>{0}</td></tr>", Answer_Expain))
        End If

    End Sub

    ''' <summary>
    ''' ทำการ Gen String HTML คำถาม-คำตอบ ในรูปแบบเรียงลำดับ
    ''' </summary>
    ''' <param name="section">section ของ object File Word</param>
    ''' <param name="QuestionNumber">ข้อที่</param>
    ''' <param name="QuestionName">คำถาม</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="sb">ตัวแปร StringBuilder ที่ทำการสร้างโครงสร้างไว้ที่ Function ก่อนหน้า ให้ส่งเข้ามาด้วยเลยเพราะคำถามแบบเรียงลำดับเป็นข้อใหญ่ข้อเดียว ไม่ได้ gen แยกเป็นข้อๆเหมือนกับ ตัวเลือก และ ถูกผิด</param>
    ''' <param name="IsShowCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <param name="AnswerName">อันดับของคำถามข้อนั้น ส่งเข้ามาในกรณีเมื่อให้แสดงเฉลยด้วย</param>
    ''' <remarks></remarks>
    Public Sub GenQuestionAndAnswerType6(ByVal section As WSection, ByVal QuestionNumber As String, ByVal QuestionName As String, ByVal QsetId As String, ByRef sb As StringBuilder, ByVal IsShowCorrectAnswer As Boolean, Optional ByVal AnswerName As String = "")
        'Function สำหรับ Gen ข้อสอบแบบ เรียงลำดับ
        Dim LeadingText As ArrayList
        If HttpContext.Current.Session("IsEngExam") = True Then
            LeadingText = GenArrayLeadingEngText()
        Else
            LeadingText = GenArrayLeadingText()
        End If
        sb.Append("<tr>")

        'ส่วนคำถาม
        Dim QuestionNameComplete As String = ReplaceString(QsetId, QuestionName, GetUrl)
        If IsShowCorrectAnswer = True Then
            QuestionNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'><u>" & LeadingText(CInt(AnswerName) - 1) & "</u></td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ".</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:67%;vertical-align:top;'>" & QuestionNameComplete
        Else
            QuestionNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:7%;vertical-align:top;'>____</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & QuestionNumber & ".</td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:67%;vertical-align:top;'>" & QuestionNameComplete
        End If
        sb.Append(QuestionNameComplete & "</td>")

        'ส่วนคำตอบ
        Dim AnswerNameComplete As String = ""
        AnswerNameComplete = "<td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:3%;vertical-align:top;'>" & LeadingText(CInt(QuestionNumber) - 1) & ". </td><td style='font-family:Angsana New;font-size:" & AllFontSize & ";width:20%;vertical-align:top;'>" & QuestionNumber
        sb.Append(AnswerNameComplete & "</td>")
        sb.Append("</tr>")
    End Sub

    ''' <summary>
    ''' ทำการ Gen รายละเอียดข้อสอบชุดคำถามนี้ พวก QsetName , จำนวนข้อสอบ , คะแนนทั้งหมด
    ''' </summary>
    ''' <param name="section">section ของ object file word</param>
    ''' <param name="StyleName">ชื่อ Style ที่จะใช้กับ Paragraph นี้</param>
    ''' <param name="QsetName">รายละเอียดของชุดคำถาม</param>
    ''' <param name="TotalQuestionInQset">จำนวนข้อสอบทั้งหมด</param>
    ''' <param name="IsFirstRound">เป็นรอบแรกของการ loop ? เพราะถ้าไม่จะต้องทำการ PageBreak</param>
    ''' <remarks></remarks>
    Public Sub GenQsetText(ByVal section As WSection, ByVal StyleName As String, ByVal QsetName As String, ByVal TotalQuestionInQset As String, ByVal IsFirstRound As Boolean, Optional ByVal QuestionExpain As String = "")
        'ทำการสร้าง Paragraph ใหม่ขึ้นมา เพื่อให้รายละเอียดของชุดคำถามนี้ และ add เข้าไปใน section 
        Dim QsetNameParagraph As IWParagraph = section.AddParagraph()
        QsetNameParagraph.ParagraphFormat.Keep = True
        If IsFirstRound = False Then
            QsetNameParagraph.AppendBreak(BreakType.PageBreak)
        End If
        QsetNameParagraph.ApplyStyle(StyleName)
        Dim MergeString As String = QsetName & " (จำนวน " & TotalQuestionInQset & " ข้อ,ข้อละ 1 คะแนน) "
        MergeString = ClsPDF.RemoveDuplicationBRTag(MergeString)
        Try
            If QuestionExpain <> "" Then MergeString &= String.Format("<table style='border:dashed;border-color: #afafaf;margin:5px;'><tr><td style='width:600px;'>{0}</td></tr></table>", QuestionExpain)
            QsetNameParagraph.AppendHTML("<table><tr><td style='font-family:Angsana New;font-size:" & AllFontSize & ";'>" & MergeString & "</td></tr></table>")
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try

    End Sub

    ''' <summary>
    ''' ทำการสร้างรายละเอียดที่จะแสงอยู่ในส่วนล่างของไฟล์ Word ในทุกๆกหน้า เป็นเลขหน้า ที่ Run ไปเรื่อยๆ
    ''' </summary>
    ''' <param name="document">Document object ของไฟล์ Word</param>
    ''' <param name="section">Section ของ oeject ไฟล์ Word</param>
    ''' <remarks></remarks>
    Public Sub GenFooter(ByVal document As WordDocument, ByVal section As WSection)
        GenStyle(document, "CenterStyle", "Angsana New", 18, Color.DarkGray, SetPositionWord.Center)
        'สร้าง Paragraph ใหม่สำหรับใส่ข้อมูลที่จะไปแสดงใน Footer
        Dim NewParaPageNo As IWParagraph = section.AddParagraph()
        NewParaPageNo.AppendText("หน้าที่ ")
        NewParaPageNo.AppendField("Page2", FieldType.FieldPage)
        NewParaPageNo.AppendText(" จาก ")
        NewParaPageNo.AppendField("Page", FieldType.FieldNumPages)
        NewParaPageNo.AppendText(" หน้า")
        NewParaPageNo.ApplyStyle("CenterStyle")
        section.HeadersFooters.Footer.Paragraphs.Add(NewParaPageNo)
    End Sub

    ''' <summary>
    ''' ทำการสร้างรายละเอียดต่างๆที่จะแสดงอยู่ในส่วนบนของไฟล์ Word ในทุกๆหน้า ชื่อข้อสอบ,จำนวนข้อ,ชื่อครู,คะแนน,เวลาในการทำ ฯลฯ
    ''' </summary>
    ''' <param name="document">Document Object ของไฟล์ Word</param>
    ''' <param name="section">Section Object ของไฟล์ Word</param>
    ''' <param name="TestSetName">ชื่อชุดข้อสอบ</param>
    ''' <param name="ExamAmount">จำนวนข้อสอบทั้งหมด</param>
    ''' <param name="TestTime">เวลาในการทำข้อสอบ</param>
    ''' <remarks></remarks>
    Public Sub GenHeader(ByVal document As WordDocument, ByVal section As WSection, ByVal TestSetName As String, ByVal ExamAmount As String, ByVal TestTime As String)
        'GenHeader ให้ Word เป็นส่วนที่อยู่บนหัวกระดาษทุกๆหน้า
        GenStyle(document, "TestSetStyle", "Angsana New", 23, Color.DarkGray, SetPositionWord.Center)
        GenStyle(document, "HeaderStyle", "Angsana New", 18, Color.DarkGray, SetPositionWord.Center)
        'Paragraph ชื่อ TestSet
        Dim ParagraphTestSetName As IWParagraph = section.AddParagraph()
        ParagraphTestSetName.AppendText(TestSetName)
        ParagraphTestSetName.ApplyStyle("TestSetStyle")
        section.HeadersFooters.Header.Paragraphs.Add(ParagraphTestSetName)
        'Paragraph รายละเอียดต่างๆ
        Dim ParagraphHeaderInfo As IWParagraph = section.AddParagraph()
        ParagraphHeaderInfo.AppendText("ครูผู้สอน ...................................................  วัน .........................  เวลาสอบ .........................  ")
        ParagraphHeaderInfo.AppendBreak(BreakType.LineBreak)
        ParagraphHeaderInfo.AppendText("จำนวน " & ExamAmount & " ข้อ รวม " & ExamAmount & " คะแนน   เวลา " & TestTime & " นาที")
        ParagraphHeaderInfo.ApplyStyle("HeaderStyle")
        section.HeadersFooters.Header.Paragraphs.Add(ParagraphHeaderInfo)
    End Sub

    ''' <summary>
    ''' ทำการ replace ข้อความ คำถาม-คำตอบ พวก Path รูปภาพ , อักขระพิเศษต่างๆที่ไม่อาจทำให้โปรแกรมพังได้
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet เอามาใช้หา Path รูป</param>
    ''' <param name="QuestionOrAnswerName">เนื้อ คำถาม/คำตอบ</param>
    ''' <param name="GetUrl">URL ที่ทำการ Get ไว้ตอน Constructor</param>
    ''' <returns>เนื้อคำถาม/คำตอบ ที่ทำการ Replace ข้อมูลเรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Public Function ReplaceString(ByVal QsetId As String, ByVal QuestionOrAnswerName As String, ByVal GetUrl As String)

        Dim QuestionOrAnswerNameComplete As String = ""
        If QuestionOrAnswerName <> "" Then
            QuestionOrAnswerNameComplete = QuestionOrAnswerName.Replace("___MODULE_URL___", ClsPDF.GenFilePath(QsetId))
            QuestionOrAnswerNameComplete = Replace(QuestionOrAnswerNameComplete, "../", GetUrl).Replace("<o:p></o:p>", "").Replace("<spanstyle='mso-char-type:symbol;mso-symbol-font-family:Symbol'>", "<span>").Replace("<spanstyle='font-size:20.0pt;mso-bidi-font-family:""Times New Roman""'>", "<span>").Replace("<font size=""2"">", "").Replace("</font>", "").Replace("taglabel=""FRACTION""", "").Replace("type=""numerator""", "").Replace("type=""denominator""", "").Replace("#000", "black")
            QuestionOrAnswerNameComplete = ClsPDF.RemoveDuplicationBRTag(QuestionOrAnswerNameComplete)
        End If
        Return QuestionOrAnswerNameComplete

    End Function

    ''' <summary>
    ''' Function สำหรับ Keep ให้ส่วนที่เป็นข้อเดียวกันไม่ตกไปอยู่คนละหน้ากันตอนที่เป็น File Word
    ''' </summary>
    ''' <param name="InputParagraph">Paragraph ของคำถาม/คำตอบ แต่ละข้อ</param>
    ''' <remarks></remarks>
    Public Sub SetKeepFollowForAppendHtml(ByVal InputParagraph As WParagraph)
        Dim table As WTable = TryCast(InputParagraph.NextSibling, WTable)
        For Each row As WTableRow In table.Rows
            For Each rowpara As WParagraph In row.Cells(0).Paragraphs
                rowpara.ParagraphFormat.KeepFollow = True
            Next
        Next
    End Sub

    ''' <summary>
    ''' หาจำนวนข้อสอบทั้งหมดในชุดข้อสอบนี้
    ''' </summary>
    ''' <param name="TestSetId">Id ของ tblTestset ที่ต้องการหาจำนวนข้อสอบทั้งหมด</param>
    ''' <returns>String:จำนวนข้อสอบทั้งหมดของชุดข้อสอบนี้</returns>
    ''' <remarks></remarks>
    Public Function GetExamAmount(ByVal TestSetId As String) As String
        Dim sql As String = " SELECT TSQS_Id FROM dbo.tblTestSetQuestionSet  WHERE TestSet_Id = '" & TestSetId & "' And IsActive = '1' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            sql = "  SELECT COUNT(*) FROM dbo.tblTestSetQuestionDetail WHERE (TSQS_Id = '" & dt.Rows(0)("TSQS_Id").ToString() & "'  "
            For index = 1 To dt.Rows.Count - 1
                sql &= " OR TSQS_Id = '" & dt.Rows(index)("TSQS_Id").ToString() & "' "
            Next
            sql &= " ) And IsActive = '1'"
        End If

        Dim ExamAmount As String = _DB.ExecuteScalar(sql)
        Return ExamAmount
    End Function

    ''' <summary>
    ''' หาชื่อชุดข้อสอบ โดยใช้ TestsetId
    ''' </summary>
    ''' <param name="TestSetId">Id ของ tblTestset ที่ต้องการหาชื่อชุดข้อสอบ</param>
    ''' <returns>String:ชื่อชุดข้อสอบ</returns>
    ''' <remarks></remarks>
    Public Function GetTestSetName(ByVal TestSetId As String) As String
        Dim sql As String = " SELECT TestSet_Name FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestSetId & "' "
        Dim TestSetName As String = _DB.ExecuteScalar(sql)
        Return TestSetName
    End Function

    ''' <summary>
    ''' หาเวลาที่ให้ใช้ในการทำข้อสอบชุดนี้
    ''' </summary>
    ''' <param name="TestSetId">Id ของ tblTestset ที่ต้องการหาเวลาในการทำข้อสอบ</param>
    ''' <returns>String:เวลาที่่ให้ใช้ในการทำข้อสอบชุดนี้</returns>
    ''' <remarks></remarks>
    Public Function GetTestSetTime(ByVal TestSetId As String) As String
        Dim sql As String = " SELECT TestSet_Time FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestSetId & "' "
        Dim TestSetTime As String = _DB.ExecuteScalar(sql)
        Return TestSetTime
    End Function

    Private Function GetTestsetDetail(ByVal TestsetId As String) As DataTable
        Dim sql As String = " SELECT * FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestsetId & "'; "
        Return _DB.getdata(sql)
    End Function

    ''' <summary>
    ''' หาจำนวนข้อสอบที่อยุ่ใน Qset,Testset Id นี้
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ที่เลือกมา</param>
    ''' <param name="TestSetId">Id ของ tblTestset ที่เลือกมา</param>
    ''' <returns>String:จำนวนข้อสอบ</returns>
    ''' <remarks></remarks>
    Public Function GetQuestionQuantityInQset(ByVal QsetId As String, ByVal TestSetId As String) As String

        Dim sql As String = " SELECT TSQS_Id FROM dbo.tblTestSetQuestionSet  " & _
                            " WHERE QSet_Id = '" & QsetId & "' AND TestSet_Id = '" & TestSetId & "' And IsActive = '1' "
        Dim TSQS_Id As String = _DB.ExecuteScalar(sql)
        Dim TotalQuestion As String = ""
        If TSQS_Id <> "" Then
            sql = " SELECT COUNT(*) FROM dbo.tblTestSetQuestionDetail WHERE TSQS_Id = '" & TSQS_Id & "' And IsActive = '1' "
            TotalQuestion = _DB.ExecuteScalar(sql)
        End If
        Return TotalQuestion

    End Function

    ''' <summary>
    ''' หาคำถามและคำตอบสำหรับคำถามแบบ จับคู่ กรณีจับคู่จะใช้ คำถาม 1 ข้อกับ 1 คำตอบเป็นคู่กันเท่านั้น
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ที่เลือกมา</param>
    ''' <param name="TestSetId">Id ของ tblTestset ที่เลือกมา</param>
    ''' <param name="IsPreview">เป็นโหมดแบบแสดงบางส่วน ?</param>
    ''' <returns>Datatable:คำถาม-คำตอบ ของจับคู่</returns>
    ''' <remarks></remarks>
    Private Function CreatedtForType3(ByVal QsetId As String, ByVal TestSetId As String, ByVal IsPreview As Boolean) As DataTable
        Dim dtcomplete As New DataTable
        dtcomplete.Columns.Add("Question_Name", GetType(System.String))
        dtcomplete.Columns.Add("Answer_Name", GetType(System.String))
        dtcomplete.Columns.Add("Answer_Expain", GetType(System.String))
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim MoreSql As String = ""

        'หาคำถาม
        If IsPreview = True Then
            MoreSql = " Top 10 "
        End If
        Dim sql As String = " SELECT " & MoreSql & " q.Question_Name FROM tblQuestionSet AS qs INNER JOIN " & _
                            " tblQuestion AS q ON qs.QSet_Id = q.QSet_Id INNER JOIN " & _
                            " tblTestSetQuestionDetail AS tsqd ON q.Question_Id = tsqd.Question_Id   " & _
                            " WHERE (tsqd.TSQS_Id IN (SELECT TSQS_Id FROM tblTestSetQuestionSet AS tsqs " & _
                            " WHERE (TestSet_Id = '" & TestSetId & "') And IsActive = '1')) And tsqd.IsActive = '1' AND (q.QSet_Id = '" & QsetId & "') " & _
                            " ORDER BY q.Question_No "
        dt1 = _DB.getdata(sql)

        'หาคำตอบ
        sql = " SELECT " & MoreSql & " tblAnswer.Answer_Name,tblAnswer.Answer_Expain FROM tblAnswer INNER JOIN tblQuestion ON tblAnswer.Question_Id = " & _
              " tblQuestion.Question_Id INNER JOIN tblTestSetQuestionDetail ON tblQuestion.Question_Id =  " & _
              " tblTestSetQuestionDetail.Question_Id INNER JOIN tblTestSetQuestionSet ON  tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_Id  " & _
              " where tblTestSetQuestionSet.TestSet_Id = '" & TestSetId & "'  and " & _
              " tblTestSetQuestionSet.QSet_Id = '" & QsetId & "' And tblTestsetQuestionSet.IsActive = '1' and tblTestsetQuestionDetail.IsActive = '1' ORDER BY tblAnswer.Answer_Id "

        dt2 = _DB.getdata(sql)

        'loop เพื่อทำการ Add ข้อมูลคำถาม-คำตอบที่อยู่คนละ Datatable ให้เข้าไปอยู่ใน Datatable ด้วยกันเพื่อที่จะได้เอาไปใช้ Gen Word ต่อไป,เงื่อนไขการจบ loop คือ วนครบทุกข้อ
        For i = 0 To dt1.Rows.Count - 1
            dtcomplete.Rows.Add(dt1.Rows(i)("Question_Name").ToString(), dt2.Rows(i)("Answer_Name").ToString(), dt2.Rows(i)("Answer_Expain").ToString())
        Next

        Return dtcomplete
    End Function

    ''' <summary>
    ''' ทำการหาข้อมูลคำถามและคำตอบคำถามประเภท ถูก-ผิด แบบแสดงเฉลยมาด้วย
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของชุดคำถามนี้</param>
    ''' <param name="TestSetId">Id ของ tblTestset ของชุดคำถามนี้</param>
    ''' <returns>Datatable ของคำถาม-คำตอบ พร้อมคะแนน</returns>
    ''' <remarks></remarks>
    Private Function CreatedtType2ShowCorrect(ByVal QsetId As String, ByVal TestSetId As String) As DataTable
        Dim dtType2 As New DataTable
        Dim sql As String = " SELECT q.Question_Name,Answer_Name,qs.Qset_Name,Answer_Expain FROM tblQuestionSet AS qs INNER JOIN " & _
                            " tblQuestion AS q ON qs.QSet_Id = q.QSet_Id INNER JOIN tblTestSetQuestionDetail AS tsqd " & _
                            " ON q.Question_Id = tsqd.Question_Id INNER JOIN dbo.tblAnswer ON " & _
                            " tsqd.Question_Id = dbo.tblAnswer.Question_Id  WHERE (tsqd.TSQS_Id IN " & _
                            " (SELECT TSQS_Id FROM tblTestSetQuestionSet AS tsqs " & _
                            " WHERE (TestSet_Id = '" & TestSetId & "') And Isactive = '1')) AND (q.QSet_Id = '" & QsetId & "') " & _
                            " AND (dbo.tblAnswer.Answer_Score > 0) And tsqd.IsActive = '1' ORDER BY q.Question_No "
        dtType2 = _DB.getdata(sql)
        Return dtType2
    End Function

    ''' <summary>
    ''' หาคำถาม-คำตอบของคำถามแบบ จับคู่ โดยให้เรียง datatable ให้คำถามอยู่ record เดียวกับคำตอบที่ถูกต้องไปเลยเพื่อนำไปแสดงเฉลยได้
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="TestSetId">Id ของ tblTestset ของคำถามชุดนี้</param>
    ''' <returns>Datatable:คำถาม-คำตอบ แบบที่จับคู่กันถูกต้องแล้ว</returns>
    ''' <remarks></remarks>
    Private Function CreatedtType3ShowCorrect(ByVal QsetId As String, ByVal TestSetId As String) As DataTable
        Dim dtType3 As New DataTable
        Dim sql As String = " SELECT q.Question_Name,Answer_Name FROM tblQuestionSet AS qs INNER JOIN " & _
                            " tblQuestion AS q ON qs.QSet_Id = q.QSet_Id INNER JOIN " & _
                            " tblTestSetQuestionDetail AS tsqd ON q.Question_Id = tsqd.Question_Id INNER JOIN " & _
                            " dbo.tblAnswer ON tsqd.Question_Id = dbo.tblAnswer.Question_Id  " & _
                            " WHERE (tsqd.TSQS_Id IN (SELECT TSQS_Id FROM tblTestSetQuestionSet AS tsqs " & _
                            " WHERE (TestSet_Id = '" & TestSetId & "') And tsqs.isActive = '1' )) And tsqd.IsActive = '1' AND (q.QSet_Id = '" & QsetId & "') " & _
                            " ORDER BY q.Question_No "
        dtType3 = _DB.getdata(sql)
        Return dtType3
    End Function

    ''' <summary>
    ''' ทำการหาข้อมูลคำถามคำตอบสำหรับข้อสอบแบบเรียงลำดับ ข้อสอบแบบเรียงลำดับจะใช้ QsetName มาเป็น คำถาม และ ใช้ QuestionName มาเป็นคำตอบ ส่วนจะดูว่าแต่ละข้ออยู่ลำดับไหนให้ดูที่ Answer_Name
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="TestSetId">Id ของ tblTestset ของคำถามข้อนี้</param>
    ''' <param name="IsPreview">เป็นโหมดแสดงเฉพาะบางส่วน ? ถ้าใช่ให้ทำการ select แค่ 10 ข้อพอ</param>
    ''' <param name="IsCorrectAnswer">ต้องการให้แสดงเฉลย ?</param>
    ''' <returns>Datatable:คำถาม-คำตอบ ของข้อสอบแบบเรียงลำดับ</returns>
    ''' <remarks></remarks>
    Private Function CreatedtType6(ByVal QsetId As String, ByVal TestSetId As String, ByVal IsPreview As Boolean, ByVal IsCorrectAnswer As Boolean) As DataTable
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim MoreSql As String = ""

        If IsPreview = True Then
            MoreSql = " Top 10 "
        End If

        'Dim OrderSql As String = ""
        'If IsCorrectAnswer = True Then
        '    OrderSql = "tblAnswer.Answer_No"
        'Else
        '    OrderSql = "q.Question_No"
        'End If

        sql = " SELECT " & MoreSql & " q.Question_Name,Answer_Name ,qs.QSet_Name FROM tblQuestionSet AS qs INNER JOIN " & _
              " tblQuestion AS q ON qs.QSet_Id = q.QSet_Id INNER JOIN " & _
              " tblTestSetQuestionDetail AS tsqd ON q.Question_Id = tsqd.Question_Id INNER JOIN " & _
              " dbo.tblAnswer ON tsqd.Question_Id = dbo.tblAnswer.Question_Id " & _
              " WHERE (tsqd.TSQS_Id IN (SELECT TSQS_Id FROM tblTestSetQuestionSet AS tsqs " & _
              " WHERE (TestSet_Id = '" & TestSetId & "') And tsqs.isActive = '1')) And tsqd.IsActive = '1' AND (q.QSet_Id = '" & QsetId & "') " & _
              " AND (dbo.tblAnswer.Answer_Score > 0) ORDER BY q.Question_No  "
        dt = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' หารายละเอียดชองชุดคำถามโดยใช้ QsetId
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ที่ต้องการหารายละเอียด</param>
    ''' <returns>String:รายละเอียดของชุดคำถาม(QSetName)</returns>
    ''' <remarks></remarks>
    Private Function GetQsetName(ByVal QsetId As String)
        Dim sql As String = " SELECT QSet_Name FROM dbo.tblQuestionSet WHERE QSet_Id = '" & QsetId & "' "
        Dim QsetName As String = _DB.ExecuteScalar(sql)
        Return QsetName
    End Function

    ''' <summary>
    ''' ทำการหาขนาดตัวอักษรของชุดคำถาม
    ''' </summary>
    ''' <param name="TestSetId">Id ของ tblTestset ของชุดคำถามที่ต้องการหาขนาดตัวอักษร</param>
    ''' <returns>String:ขนาดตัวอักษร</returns>
    ''' <remarks></remarks>
    Private Function GetTestFontSize(ByVal TestSetId As String)
        Dim sql As String = " SELECT TestSet_FontSize FROM dbo.tblTestSet WHERE TestSet_Id = '" & TestSetId & "' "
        Dim TestFontSize As String = _DB.ExecuteScalar(sql)
        Return TestFontSize
    End Function

    ''' <summary>
    ''' หาชื่อวิชาโดยใช้ QsetId
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ที่ต้องการหาชื่อวิชา</param>
    ''' <returns>String:ชื่อวิชา</returns>
    ''' <remarks></remarks>
    Private Function GetSubjectNameByQsetId(ByVal QsetId As String)

        Dim sql As String = " SELECT DISTINCT tblGroupSubject.GroupSubject_Name FROM tblQuestionSet INNER JOIN " & _
                            " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN " & _
                            " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN " & _
                            " tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id " & _
                            " WHERE (tblQuestionSet.QSet_Id = '" & QsetId & "') "
        Dim SubjectName As String = ""
        SubjectName = _DB.ExecuteScalar(sql)
        Return SubjectName

    End Function


    'Private Sub RemoveEmptyParagraph(ByVal curSection As IWSection)

    '    Dim j As Integer = 0
    '    Dim paraCount As Integer = 0
    '    While j < curSection.Body.ChildEntities.Count
    '        Dim textBody As WTextBody = curSection.Body
    '        Dim TextbodyCollection As BodyItemCollection = TryCast(textBody.ChildEntities, BodyItemCollection)
    '        If TextbodyCollection(j).EntityType = EntityType.Paragraph Then
    '            Dim paragraph As IWParagraph = TryCast(TextbodyCollection(j), IWParagraph)
    '            If paragraph.Items.Count = 0 Then
    '                curSection.Paragraphs.RemoveAt(paraCount)
    '            Else
    '                paraCount += 1
    '                j += 1
    '            End If
    '        Else
    '            j += 1
    '        End If
    '    End While

    'End Sub

    ''' <summary>
    ''' Function ที่ทำมาเพื่อแก้ปัญหาบรรทัดห่าง
    ''' </summary>
    ''' <param name="paragraph">Paragraph object ของ ไฟล์ Word</param>
    ''' <remarks></remarks>
    Private Sub RemoveEmptyParagraph(paragraph As WParagraph)
        If paragraph.ChildEntities.Count = 0 Then
            TryCast(paragraph.Owner, WTextBody).ChildEntities.Remove(paragraph)
        End If
    End Sub

    ''' <summary>
    ''' สร้าง Array ที่มี Item เป็นคำนำหน้าคำตอบที่เป็นภาษาไทย
    ''' </summary>
    ''' <returns>Array คำนำหน้าคำตอบแบบภาษาอังกฤษ</returns>
    ''' <remarks></remarks>
    Private Function GenArrayLeadingText()
        Dim LeadingArray As New ArrayList
        LeadingArray.Add("ก")
        LeadingArray.Add("ข")
        LeadingArray.Add("ค")
        LeadingArray.Add("ง")
        LeadingArray.Add("จ")
        LeadingArray.Add("ฉ")
        LeadingArray.Add("ช")
        LeadingArray.Add("ซ")
        LeadingArray.Add("ฌ")
        LeadingArray.Add("ญ")
        LeadingArray.Add("ฎ")
        LeadingArray.Add("ฏ")
        LeadingArray.Add("ฐ")
        LeadingArray.Add("ฑ")
        LeadingArray.Add("ฒ")
        LeadingArray.Add("ณ")
        LeadingArray.Add("ด")
        LeadingArray.Add("ต")
        LeadingArray.Add("ถ")
        LeadingArray.Add("ท")
        LeadingArray.Add("ธ")
        LeadingArray.Add("น")
        LeadingArray.Add("บ")
        LeadingArray.Add("ป")
        LeadingArray.Add("ผ")
        LeadingArray.Add("ฝ")
        LeadingArray.Add("พ")
        LeadingArray.Add("ฟ")
        LeadingArray.Add("ภ")
        LeadingArray.Add("ม")
        LeadingArray.Add("ย")
        LeadingArray.Add("ร")
        LeadingArray.Add("ล")
        LeadingArray.Add("ว")
        LeadingArray.Add("ศ")
        LeadingArray.Add("ษ")
        LeadingArray.Add("ส")
        LeadingArray.Add("ห")
        LeadingArray.Add("ฬ")
        LeadingArray.Add("อ")
        LeadingArray.Add("ฮ")
        Return LeadingArray
    End Function

    ''' <summary>
    ''' สร้าง Array ที่มี Item เป็นคำนำหน้าคำตอบที่เป็นภาษาอังกฤษ
    ''' </summary>
    ''' <returns>Array คำนำหน้าคำตอบแบบภาษาอังกฤษ</returns>
    ''' <remarks></remarks>
    Private Function GenArrayLeadingEngText()
        Dim LeadingArray As New ArrayList
        LeadingArray.Add("A")
        LeadingArray.Add("B")
        LeadingArray.Add("C")
        LeadingArray.Add("D")
        LeadingArray.Add("E")
        LeadingArray.Add("F")
        LeadingArray.Add("G")
        LeadingArray.Add("H")
        LeadingArray.Add("I")
        LeadingArray.Add("J")
        LeadingArray.Add("K")
        LeadingArray.Add("L")
        LeadingArray.Add("M")
        LeadingArray.Add("N")
        LeadingArray.Add("O")
        LeadingArray.Add("P")
        LeadingArray.Add("Q")
        LeadingArray.Add("R")
        LeadingArray.Add("S")
        LeadingArray.Add("T")
        LeadingArray.Add("U")
        LeadingArray.Add("V")
        LeadingArray.Add("W")
        LeadingArray.Add("X")
        LeadingArray.Add("Y")
        LeadingArray.Add("Z")
        Return LeadingArray
    End Function

    ''' <summary>
    ''' Function ที่ทำการแก้ปัญหาเรื่องการแสดงตารางแล้วเพี้ยน
    ''' </summary>
    ''' <param name="document">Document Object ของไฟล์ Word</param>
    ''' <remarks></remarks>
    Private Sub LoopSetIsAutoResizedToFalse(ByRef document As WordDocument)
        For Each eachTable As Syncfusion.DocIO.DLS.IWTable In document.LastSection.Tables
            eachTable.TableFormat.IsAutoResized = False
        Next
    End Sub

    Private Function GetQuestionExpainType3(QsetId As String) As String
        Dim sql As String = String.Format("SELECT * FROM tblQuestion WHERE IsActive = 1 AND Question_No = 1 AND QSet_Id = '{0}';", QsetId)
        Dim dt As DataTable = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Question_Expain") Is DBNull.Value Then Return ""
            Return dt.Rows(0)("Question_Expain").ToString()
        End If
        Return ""
    End Function

  


End Class

Public Class QuestionNumber

    Public Property PrefixName As String
    Public Property NumberType As QuestionNoType
    Private NumberLengthToString As String = ""
    Private CurrentNumber As Integer

    Private Sub New()
    End Sub

    Public Sub New(ByVal Quantity As Integer)
        Me.CurrentNumber = 1
        For index = 1 To Quantity.ToString().Length
            Me.NumberLengthToString &= "0"
        Next
    End Sub

    Public Function Number() As String
        If NumberType = QuestionNoType.defaultNumber Then
            Return CurrentNumber
        End If
        Return PrefixName & "- <br />" & CurrentNumber.ToString(NumberLengthToString)
    End Function

    Public Sub IncreaseOneValue()
        CurrentNumber += 1
    End Sub

End Class
Public Enum QuestionNoType
    defaultNumber = 1
    prefixNumber = 2
End Enum