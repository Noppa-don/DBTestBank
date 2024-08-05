Imports System.Data.SqlClient
Imports System.Web

Public Class PreviewAndEditExam
    Inherits System.Web.UI.Page
    'ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'เนื้อคำถามเอาไปใช้กับฝั่ง Javascript
    Protected Shared CurrentQuestionName As String
    'เลขข้อเอาไปใช้กับฝั่ง Javascript
    Protected Shared CurrentQuestionNo As String
    'เนื้อคำอธิบายคำถามเอาไปใช้กับฝั่ง Javascript
    Protected Shared CurrentQuestionExplain As String
    'คำนำหน้าคำตอบ แบบเป็นภาษาไทย
    Dim PrefixAnswer() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ", "ฒ", "ณ", "ด", "ต", ""}
    'คำนำหน้าคำตอบ แบบเป็นภาษาอังกฤษ
    Dim PrefixAnswerEng() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", ""}
    'QuestionId เอาไปใช้กับฝั่ง Javascript
    Protected Shared CurrentQuestionId As String
    'QSetType เอาไปใช้กับฝั่ง Javascript
    Protected Shared CurrentQsetType As Integer
    'URL เอาไปใช้ Replace กับพวก path รูป
    Public Shared filePathAuthority As String = "http://" & HttpContext.Current.Request.Url.Authority()

    ''' <summary>
    ''' ทำการ Bind ข้อมูล คำถาม-คำตอบ เข้าไปใน Editor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("QSetId") IsNot Nothing AndAlso Request.QueryString("qid") IsNot Nothing Then
            'get data from querystring
            HttpContext.Current.Session("CurrentQsetId") = Request.QueryString("QsetId").ToString()
            CurrentQuestionId = Request.QueryString("qid").ToString()


            'open connection
            Dim ConnEdit As New SqlConnection
            _DB.OpenExclusiveConnect(ConnEdit)

            'get QsetType
            CurrentQsetType = GetQsetTypeByQsetId(HttpContext.Current.Session("CurrentQsetId"), ConnEdit)

            'Get QuestionName
            GetQuestionNameAndQuestionExplain(CurrentQuestionId, HttpContext.Current.Session("CurrentQsetId"), CurrentQsetType, ConnEdit)

            'Render Answer
            RenderAnswerAndAnswerExplain(CurrentQuestionId, CurrentQsetType, HttpContext.Current.Session("CurrentQsetId"), ConnEdit)

            'close connection
            _DB.CloseExclusiveConnect(ConnEdit)
        End If
    End Sub

#Region "Section-Render"
    ''' <summary>
    ''' ทำการหาข้อมูลคำถาม โดยแบ่งตามแต่ละประเภทแล้วทำาการแทนค่าตัวแปรไปใช้ทางฝั่ง Javascript ได้เลย
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion คำถามข้อนี้</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet คำถามข้อนี้</param>
    ''' <param name="QsetType">ประเภทอของคำถามข้อนี้</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub GetQuestionNameAndQuestionExplain(ByVal QuestionId As String, ByVal QsetId As String, ByVal QsetType As Integer, ByRef InputConn As SqlConnection)
        If QuestionId IsNot Nothing AndAlso QuestionId.Trim() <> "" Then
            Dim sqlQuestionName As String = ""
            Dim sqlQuestionExplain As String = ""
            'ถ้าเป็นแบบตัวเลือก , ถูก-ผิด ให้หาคำถามที่ tblQuestion ได้เลย
            Dim dt As New DataTable
            If QsetType <> 3 AndAlso QsetType <> 6 Then
                sqlQuestionName = " SELECT Question_No,Question_Name_Quiz FROM dbo.tblQuestion WHERE Question_Id = '" & QuestionId & "' AND IsActive = 1; "
                dt = _DB.getdata(sqlQuestionName)
                If dt.Rows.Count <> 0 And dt.Rows(0)("Question_Name_Quiz") IsNot DBNull.Value Then
                    CurrentQuestionName = dt.Rows(0)("Question_Name_Quiz").Replace("___MODULE_URL___", GenFilePath(QsetId))
                    CurrentQuestionNo = Request.QueryString("QNo").ToString()
                End If
            Else
                'ถ้าเป็นจับคู่ , เรียงลำดับ ให้หาคำถามที่ tblQuestionSet แทน
                sqlQuestionName = "SELECT '1' as Question_No, QSet_Name_Quiz FROM dbo.tblQuestionset WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1; "
                dt = _DB.getdata(sqlQuestionName)
                If dt.Rows.Count <> 0 And dt.Rows(0)("QSet_Name_Quiz") IsNot DBNull.Value Then
                    CurrentQuestionName = dt.Rows(0)("QSet_Name_Quiz").Replace("___MODULE_URL___", GenFilePath(QsetId))
                    CurrentQuestionNo = dt.Rows(0)("Question_No")
                End If
            End If

            Dim cls As New ClsPDF(New ClassConnectSql)
            CurrentQuestionName = cls.CleanSetNameText(CurrentQuestionName).ToString

            sqlQuestionExplain = " SELECT Question_Expain_Quiz FROM dbo.tblQuestion WHERE Question_Id = '" & QuestionId & "' AND IsActive = 1; "
            CurrentQuestionExplain = _DB.ExecuteScalar(sqlQuestionExplain, InputConn).Replace("___MODULE_URL___", GenFilePath(QsetId))
        End If
    End Sub

    ''' <summary>
    ''' ทำการต่อสตริงสร้าง Path ของรูปภาพ โดยใช้ QsetId 
    ''' </summary>
    ''' <param name="QSetId">Id ของ tblQuestionset ของคำถามข้อนี้</param>
    ''' <returns>Path ของรูปภาพเอาไปแทนค่ากับเนื้อข้อมูลได้เลย</returns>
    ''' <remarks></remarks>
    Private Function GenFilePath(ByVal QSetId As String) As String
        Dim rootPath As String = "../file/"
        Dim filePath As String
        filePath = QSetId.Substring(0, 1) + "/" + QSetId.Substring(1, 1) + "/" + QSetId.Substring(2, 1) + _
            "/" + QSetId.Substring(3, 1) + "/" + QSetId.Substring(4, 1) + "/" + QSetId.Substring(5, 1) + _
            "/" + QSetId.Substring(6, 1) + "/" + QSetId.Substring(7, 1) + "/"
        filePath = filePath + "{" + QSetId + "}/"
        Return rootPath + filePath
    End Function

    ''' <summary>
    ''' ทำการหา dtAnswer เพื่อนำข้อมูลไปต่อสตริงทำเป็นคำตอบ โดยแยกตามแต่ละประเภท
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ของคำถามข้อนี้</param>
    ''' <param name="QSetType">ประเภทของคำถาม</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <remarks></remarks>
    Private Sub RenderAnswerAndAnswerExplain(ByVal QuestionId As String, ByVal QSetType As Integer, ByVal QsetId As String, ByRef InputConn As SqlConnection)
        Dim dt As DataTable = GetDTAnswerByType(QuestionId, QSetType, QsetId, InputConn)
        If dt.Rows.Count > 0 Then
            GenStringAnswerNameAndAnswerExplain(dt, QSetType, QsetId)
        End If
    End Sub
#End Region

#Region "Section-GetData"
    ''' <summary>
    ''' ทำการหาประเภทของคำถาม โดยใช้ QsetId
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ที่ต้องการหา</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Integer:ประเภทของคำถาม</returns>
    ''' <remarks></remarks>
    Private Function GetQsetTypeByQsetId(ByVal QsetId As String, ByRef InputConn As SqlConnection) As Integer
        Dim sql As String = " SELECT QSet_Type FROM dbo.tblQuestionset WHERE QSet_Id = '" & QsetId & "' AND IsActive = 1; "
        Dim QsetType As String = _DB.ExecuteScalar(sql, InputConn)
        If QsetType = "" Then
            Return 0
        Else
            Return CInt(QsetType)
        End If
    End Function

    ''' <summary>
    ''' หาข้อมูลคำตอบ โดยแยกตามแต่ละประเภทของคำถาม
    ''' </summary>
    ''' <param name="QuestionId">Id ของ tblQuestion ของคำถามข้อนี้</param>
    ''' <param name="QsetType">ประเภทอของคำถามข้อนี้</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="InputConn">ตัวแปร Connection</param>
    ''' <returns>Datatable:คำตอบ</returns>
    ''' <remarks></remarks>
    Private Function GetDTAnswerByType(ByVal QuestionId As String, ByVal QsetType As Integer, ByVal QsetId As String, ByRef InputConn As SqlConnection) As DataTable
        Dim sql As String = ""
        Dim dt As New DataTable
        'ถ้าเป็นแบบ ตัวเลือก , ถูก-ผิด หาที่ tblAnswer
        If QsetType = 1 Or QsetType = 2 Then
            sql = " SELECT Answer_Id,Answer_Name_Quiz,Answer_Expain_Quiz FROM dbo.tblAnswer WHERE Question_Id = '" & QuestionId & "' AND IsActive = 1 " & _
                  " ORDER BY Answer_No; "
        ElseIf QsetType = 3 Or QsetType = 6 Then
            'ถ้าเป็นแบบ จับคู่ , เรียงลำดับ ต้องหาคำตอบจาก tblQuestion.QuestionName และ ดูเฉลยจาก tblAnswer.AnswerName
            'sql = " SELECT dbo.tblQuestion.Question_Id,Question_Name_Quiz,Answer_Id,Answer_Name_Quiz,Answer_Expain_Quiz,QSet_Name,QSet_Name_Quiz" & _
            '    " FROM dbo.tblQuestion INNER JOIN dbo.tblAnswer  inner join tblQuestionset on tblquestion.QSet_Id = tblquestionset.QSet_Id " & _
            '    " and tblanswer.QSet_Id = tblQuestionset.QSet_Id" & _
            '      " ON dbo.tblQuestion.Question_Id = dbo.tblAnswer.Question_Id WHERE dbo.tblAnswer.QSet_Id = '" & QsetId & "' " & _
            '      " AND dbo.tblAnswer.IsActive = 1 AND dbo.tblQuestion.IsActive = 1 ORDER BY Question_No "

            sql = " SELECT dbo.tblQuestion.Question_Id,Question_Name_Quiz,Answer_Id,Answer_Name_Quiz,Answer_Expain_Quiz,QSet_Name,QSet_Name_Quiz "
            sql &= " FROM dbo.tblQuestion INNER JOIN dbo.tblAnswer  ON dbo.tblQuestion.Question_Id = dbo.tblAnswer.Question_Id inner join tblQuestionset "
            sql &= " on tblquestion.QSet_Id = tblquestionset.QSet_Id and tblanswer.QSet_Id = tblQuestionset.QSet_Id"
            sql &= " WHERE dbo.tblAnswer.QSet_Id = '" & QsetId & "'  AND dbo.tblAnswer.IsActive = 1 AND dbo.tblQuestion.IsActive = 1 "
            sql &= " ORDER BY Question_No"
        End If
        dt = _DB.getdata(sql, , InputConn)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการต่อสตริง HTML ของคำตอบ และ อธิบายคำตอบ แยกกันตามประเภทของคำถาม
    ''' </summary>
    ''' <param name="dt">Datatable คำตอบ</param>
    ''' <param name="QsetType">ประเภทของคำถาม</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <remarks></remarks>
    Private Sub GenStringAnswerNameAndAnswerExplain(ByVal dt As DataTable, ByVal QsetType As Integer, ByVal QsetId As String)
        'Gen Str
        Dim sbAnswerName As New StringBuilder()
        Dim sbAnswerExplain As New StringBuilder()
        If QsetType = 1 Then 'choice
            GenStrAnswerType1(QsetId, dt, sbAnswerName, sbAnswerExplain)
        ElseIf QsetType = 2 Then 'true-false
            GenStrAnswerType2(dt, QsetId, sbAnswerName, sbAnswerExplain)
        ElseIf QsetType = 3 Then 'matching
            GenStrAnswerType3(dt, QsetId, sbAnswerName, sbAnswerExplain)
        ElseIf QsetType = 6 Then 'order
            GenStrAnswerType6(dt, QsetId, sbAnswerName, sbAnswerExplain)
        End If
        'Inner HTML
        If sbAnswerName.ToString().Trim() <> "" Then
            mainAnswer.InnerHtml = sbAnswerName.ToString()
        End If
        If sbAnswerExplain.ToString().Trim() <> "" Then
            AnswerExp.InnerHtml = sbAnswerExplain.ToString()
        End If
    End Sub

    ''' <summary>
    ''' ทำการต่อสตริง HTML คำตอบ แบบ ตัวเลือก
    ''' </summary>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="Dt">Datatable ของคำตอบ</param>
    ''' <param name="SbAnswerName">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำตอบ</param>
    ''' <param name="SbAnswerExplain">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำอธิบายคำตอบ</param>
    ''' <remarks></remarks>
    Private Sub GenStrAnswerType1(ByVal QsetId As String, ByVal Dt As DataTable, ByRef SbAnswerName As StringBuilder, ByRef SbAnswerExplain As StringBuilder)
        Dim AnswerName As String = ""
        Dim AnswerId As String = ""
        Dim AnswerExplain As String = ""
        SbAnswerName.Append("<table id='Table1' style='width: 650px; border-collapse: collapse;'>")
        SbAnswerName.Append("<tbody>")
        'loop เพื่อต่อสตริง คำตอบแบบ ตัวเลือก , เงื่อนไขการจบ loop คือ วนจนหมดทุกคำตอบ
        For i As Integer = 0 To Dt.Rows.Count - 1
            If Dt.Rows(i)("Answer_Name_Quiz") IsNot DBNull.Value Then
                AnswerName = Dt.Rows(i)("Answer_Name_Quiz").Replace("___MODULE_URL___", GenFilePath(QsetId))
            End If

            AnswerId = Dt.Rows(i)("Answer_Id").ToString()
            If Dt.Rows(i)("Answer_Expain_Quiz") IsNot DBNull.Value Then
                AnswerExplain = Dt.Rows(i)("Answer_Expain_Quiz").Replace("___MODULE_URL___", GenFilePath(QsetId))
            End If

            Dim PrefixChoice() As String

            If CheckIsEnglish(QsetId) Then
                PrefixChoice = PrefixAnswerEng
            Else
                PrefixChoice = PrefixAnswer
            End If

            'AnswerName
            If i Mod 2 = 0 Then
                'Answer ฝั่งซ้าย
                SbAnswerName.Append("<tr style='border-bottom: solid 1px #AFAFAF;vertical-align: top;'>")
                SbAnswerName.Append("<td style='height: 50px;font-weight: bold;width:35px;position:relative;'>")
                'sb.Append(PrefixChoice(i) & ".</td><td style='height: 50px;width:45%;' class='EditAnswer' editorId='Answer" & i & "'>" & AnswerName & "<textarea id='Answer" & i & "' class='height: 50px;width:45%;' style='display: none;'></textarea>" & "</td> ")
                SbAnswerName.Append(PrefixChoice(i) & ".</td><td style='height: 50px;width:45%;'><div answerid='" & AnswerId & "' class='CanEdit EditAnswer' contenteditable='true'>" & AnswerName & "</div></td> ")
            Else
                'Answer ฝั่งขวา
                SbAnswerName.Append("<td style=""height: 50px;width:30px;""></td><td style='height: 50px;font-weight: bold;width:35px;position:relative;'>")
                SbAnswerName.Append(PrefixChoice(i) & ".</td> <td style='height: 50px;width:45%;'><div answerid='" & AnswerId & "' class='CanEdit EditAnswer' contenteditable='true'>" & AnswerName & "</div></td> ")
            End If
            'AnswerExplain
            SbAnswerExplain.Append("<div><div class='NotAnswered'>")
            SbAnswerExplain.Append(PrefixChoice(i) & ". " & AnswerName)
            SbAnswerExplain.Append("<div class='CanEdit EditAnswerExplain' answerid='" & AnswerId & "' contenteditable='true'>")
            SbAnswerExplain.Append(AnswerExplain)
            SbAnswerExplain.Append("</div></div></div>")
        Next
        SbAnswerName.Append("</tr></tbody></table>")
    End Sub

    Private Function CheckIsEnglish(QsetId) As Boolean

        Dim sql As String
        sql = " select distinct GroupSubject_Id from tblbook inner join tblQuestionCategory on tblBook.BookGroup_Id = tblQuestionCategory.Book_Id  
                inner join tblQuestionset on tblQuestionCategory.QCategory_Id = tblQuestionset.QCategory_Id  where QSet_Id = '" & QsetId & "';"
        Dim GroupSubject_Id As String = _DB.ExecuteScalar(sql).ToString

        If GroupSubject_Id.ToUpper = "FB677859-87DA-4D8D-A61E-8A76566D69D8" Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' ทำการต่อสตริง HTML คำตอบ แบบ ถูก-ผิด
    ''' </summary>
    ''' <param name="Dt">Datatable ของคำตอบ</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="SbAnswer">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำตอบ</param>
    ''' <param name="SbAnswerExplain">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำอธิบายคำตอบ</param>
    ''' <remarks></remarks>
    Private Sub GenStrAnswerType2(ByVal Dt As DataTable, ByVal QsetId As String, ByRef SbAnswer As StringBuilder, ByRef SbAnswerExplain As StringBuilder)
        'AnswerName
        SbAnswer.Append("<table id='Table2' style='width: 650px; border-collapse: collapse;'><tbody><tr><td id='Td1'></td></tr>")
        SbAnswer.Append("<tr style='border-bottom: solid 1px #AFAFAF; vertical-align: top;'>")
        SbAnswer.Append("<td style='height: 50px; font-weight: bold; width: 35px; position: relative;'>ก.</td>")
        SbAnswer.Append("<td style='height: 50px; width: 45%;'>ถูก</td>")
        SbAnswer.Append("<td style='height: 50px; width: 30px;'></td>")
        SbAnswer.Append("<td style='height: 50px; font-weight: bold; width: 35px; position: relative;'>ข.</td>")
        SbAnswer.Append("<td style='height: 50px; width: 45%;'>ผิด</td>")
        SbAnswer.Append("</tr></tbody></table>")
        'AnswerExplain
        Dim AnswerName As String = ""
        Dim AnswerId As String = ""
        Dim AnswerExplain As String = ""
        'ทำการ Loop เพื่อสร้าง คำตอบ สำหรับแบบ ถูก-ผิด , เงื่อนไขการจบ loop คือ วนจนครบทุกคำตอบ
        For index = 0 To Dt.Rows.Count - 1
            If index = 0 Then
                AnswerName = "ถูก"
            Else
                AnswerName = "ผิด"
            End If
            AnswerId = Dt.Rows(index)("Answer_Id").ToString()
            If Dt.Rows(index)("Answer_Expain_Quiz") IsNot DBNull.Value Then
                AnswerExplain = Dt.Rows(index)("Answer_Expain_Quiz").ToString().Replace("___MODULE_URL___", GenFilePath(QsetId))
            End If
            SbAnswerExplain.Append("<div><div class='NotAnswered'>")
            SbAnswerExplain.Append(AnswerName)
            SbAnswerExplain.Append("<div class='CanEdit EditAnswerExplain' contenteditable='true' answerid='" & AnswerId & "' style='margin-left:0px;'>")
            SbAnswerExplain.Append(AnswerExplain)
            SbAnswerExplain.Append("</div></div></div>")
        Next
    End Sub

    ''' <summary>
    ''' ทำการต่อสตริง HTML คำถาม - คำตอบ แบบ จับคู่
    ''' </summary>
    ''' <param name="Dt">Datatable ของคำถามและคำตอบ</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="SbAnswerName">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำตอบ</param>
    ''' <param name="SbAnswerExplain">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำอธิบายคำตอบ</param>
    ''' <remarks></remarks>
    Private Sub GenStrAnswerType3(ByVal Dt As DataTable, ByVal QsetId As String, ByRef SbAnswerName As StringBuilder, ByRef SbAnswerExplain As StringBuilder)
        Dim QuestionId, QuestionName As String
        Dim AnswerId As String = ""
        Dim AnswerName As String = ""
        Dim AnswerExplain As String = ""
        SbAnswerName.Append("<table id='Table3' style='width: 650px; border-collapse: collapse;'><tbody><tr><td id='Td2'></td></tr>")
        'loop เพื่อต่อสตริง คำถาม-คำตอบ ข้อสอบแบบจับคู่ , เงื่อนไขการจบ loop คือวนจนครบทุกคำถาม-คำตอบ
        For i = 0 To Dt.Rows.Count - 1
            QuestionId = Dt.Rows(i)("Question_Id").ToString()
            Dim cls As New ClsPDF(New ClassConnectSql)
            QuestionName = cls.CleanSetNameText(Dt.Rows(i)("Question_Name_Quiz")).ToString
            QuestionName = QuestionName.Replace("___MODULE_URL___", GenFilePath(QsetId))
            AnswerId = Dt.Rows(i)("Answer_Id").ToString()
            AnswerName = Dt.Rows(i)("Answer_Name_Quiz").ToString().Replace("___MODULE_URL___", GenFilePath(QsetId))
            If Dt.Rows(i)("Answer_Expain_Quiz") IsNot DBNull.Value Then
                AnswerExplain = Dt.Rows(i)("Answer_Expain_Quiz").ToString().Replace("___MODULE_URL___", GenFilePath(QsetId))
            End If
            'Answer
            Dim CorrectAnswerId As String = Dt.Rows(i)("Answer_Id").ToString()
            Dim CorrectAnswerName As String = Dt.Rows(i)("Answer_Name_Quiz").ToString()
            SbAnswerName.Append("<tr style=""""><td style=""width:45%;border-bottom:1px solid Gray;padding-right:10px;""><div questionid='" & QuestionId & "' class='CanEdit EditQuestionType3' contenteditable='true'>" & QuestionName & "</div></td>")
            SbAnswerName.Append("<td style=""width:10%;border-bottom:1px solid Gray;text-align:center;font-weight:bold;"">คู่กับ</td><td id=""" & QuestionId & """ class=""drop"" ")
            SbAnswerName.Append("style=""width:45%;border-bottom:1px solid Gray;padding-left:10px;""><div class='CanEdit EditAnswer' answerid='" & AnswerId & "' contenteditable='true' id=""" & AnswerId & """ >" & AnswerName & "</div></td></tr>")
            'AnswerExplain
            SbAnswerExplain.Append("<div><div class='NotAnswered'><table><tr><td style='width:45%;padding-right:10px;'>")
            SbAnswerExplain.Append(QuestionName)
            SbAnswerExplain.Append("</td><td style='width:10%;text-align:center;font-weight:bold;'>คู่กับ</td><td style='width:45%;padding-left:10px;'>")
            SbAnswerExplain.Append(AnswerName)
            SbAnswerExplain.Append("</td></tr></table><div class='CanEdit EditAnswerExplain' answerid='" & AnswerId & "' contenteditable='true' style='margin-left: 0px;'>")
            SbAnswerExplain.Append(AnswerExplain)
            SbAnswerExplain.Append("</div></div></div>")
        Next
        SbAnswerName.Append("</tbody></table>")
    End Sub

    ''' <summary>
    ''' ทำการต่อสตริง HTML คำตอบ แบบเรียงลำดับ โดยใช้ Question_Name มาเป็นคำตอบแทน เพราะใช้ QsetName เป็นคำถาม ใช้ Answer_Name เป็นลำดับที่ถูกต้องของแต่ละข้อ
    ''' </summary>
    ''' <param name="Dt">Datatable ข้อมูลคำตอบ</param>
    ''' <param name="QsetId">Id ของ tblQuestionSet ของคำถามข้อนี้</param>
    ''' <param name="SbAnswer">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำตอบ</param>
    ''' <param name="SbAnswerExplain">ตัวแปร StringBuilder ที่จะนำมาต่อสตริง คำอธิบายคำตอบ</param>
    ''' <remarks></remarks>
    Private Sub GenStrAnswerType6(ByVal Dt As DataTable, ByVal QsetId As String, ByRef SbAnswer As StringBuilder, ByRef SbAnswerExplain As StringBuilder)
        SbAnswer.Append("<table id=""Table1"" style=""width: 650px; border-collapse: collapse;""><tbody><tr><td></td></tr>")
        SbAnswer.Append("<tr><td><ul>")
        Dim QuestionId, QuestionName, AnswerId As String
        Dim AnswerExplain As String = ""
        SbAnswerExplain.Append("<div><div class='NotAnswered'><ul>")
        'loop เพื่อต่อสตริง คำตอบแบบ เรียงลำดับ , เงื่อนไขการจบ loop คือ วนจนครบทุกข้อ
        For i = 0 To Dt.Rows.Count - 1
            AnswerId = Dt.Rows(i)("Answer_Id").ToString()
            QuestionId = Dt.Rows(i)("Question_Id").ToString()
            QuestionName = Dt.Rows(i)("Question_Name_Quiz").ToString().Replace("___MODULE_URL___", GenFilePath(QsetId))
            If Dt.Rows(i)("Answer_Expain_Quiz") IsNot DBNull.Value Then
                AnswerExplain = Dt.Rows(i)("Answer_Expain_Quiz").Replace("___MODULE_URL___", GenFilePath(QsetId))
            End If
            'Answer
            SbAnswer.Append("<li><div class='CanEdit EditQuestionType6' questionid='" & QuestionId & "' contenteditable='true' id=""" & QuestionId & """ >" & QuestionName & "</div></li>")
            'AnswerExplain
            SbAnswerExplain.Append("<li>")
            SbAnswerExplain.Append(QuestionName)
            SbAnswerExplain.Append("<div class='CanEdit EditAnswerExplainType6' answerid='" & AnswerId & "' contenteditable='true' style='display:block;border: 1px dashed;border-width: 2px;border-radius: 5px;margin-right: 10px;padding: 10px;'>")
            SbAnswerExplain.Append(AnswerExplain)
            SbAnswerExplain.Append("</div></li>")
        Next
        SbAnswerExplain.Append("</ul></div></div>")
        SbAnswer.Append("</ul></td></tr>")
        SbAnswer.Append("</tbody></table>")
    End Sub
#End Region

#Region "Section-Save"
    ''' <summary>
    ''' ทำการ update ข้อมูลคำถาม-คำตอบ ของ Type 1,2
    ''' </summary>
    ''' <param name="QuestionSet">สตริงข้อมูลของคำถาม มี Pattern แบบนี้ : QuestionId|QuestionName|QuestionExplain</param>
    ''' <param name="AnswerSet">สตริงข้อมูลของคำตอบ มี Pattern แบบนี้ : AnswerId|AnswerName@~@AnswerId|AnswerName@~@AnswerId|AnswerName</param>
    ''' <param name="AnswerExplainSet">สตริงข้อมูลของคำอธิบายคำตอบ มี Pattern แบบนี้ : AnswerId|AnswerExplain@~@AnswerId|AnswerExplain@~@AnswerId|AnswerExplain</param>
    ''' <returns>String:Error=ไม่สำเร็จ,Complete=สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveAfterEdit(ByVal QuestionSet As String, ByVal AnswerSet As String, ByVal AnswerExplainSet As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""
        'open transaction
        _DB.OpenWithTransection()
        Try
            Dim UpdateStatus As Integer = 0
            Dim sqlGetQuestionOld As String
            'question
            'Dim ObjQuestionSet = QuestionSet.Split("|")
            QuestionSet = System.Web.HttpUtility.UrlDecode(QuestionSet)
            Dim ObjQuestionSet = QuestionSet.Split("|")

            sqlGetQuestionOld = "Select Question_Name_Quiz,Question_Expain_Quiz from tblQuestion where Question_Id = '" & ObjQuestionSet(0).ToString().Trim() & "'"
            Dim dtOld As DataTable = _DB.getdataWithTransaction(sqlGetQuestionOld)

            'update question
            Dim QuestionNameAfterGenPath As String = GenPathForSaveImage(ObjQuestionSet(1)).Replace(filePathAuthority, "")
            Dim QuestionExplainAfterGenPath As String = GenPathForSaveImage(ObjQuestionSet(2).Replace(filePathAuthority, ""))
            sql = " UPDATE dbo.tblQuestion SET Question_Name_Quiz = '" & _DB.CleanString(QuestionNameAfterGenPath) & "' , " &
                  " Question_Expain_Quiz = '" & _DB.CleanString(QuestionExplainAfterGenPath) & "', LastUpdate = dbo.GetThaiDate() " &
                  " WHERE Question_Id = '" & ObjQuestionSet(0).ToString().Trim() & "'; "
            _DB.ExecuteWithTransection(sql)

            If dtOld.Rows(0)("Question_Name_Quiz") <> _DB.CleanString(QuestionNameAfterGenPath) Then
                Log.Record(Log.LogType.ManageExam, "แก้ไขคำถามเป็น """ & _DB.CleanString(QuestionNameAfterGenPath) & """(QuestionId=(" & ObjQuestionSet(0).ToString().Trim() & ")", True, "", ObjQuestionSet(0).ToString().Trim())
                UpdateStatus = 1
            End If
            If dtOld.Rows(0)("Question_Expain_Quiz") <> _DB.CleanString(QuestionExplainAfterGenPath) Then
                Log.Record(Log.LogType.ManageExam, "แก้ไขคำอธิบายคำถามเป็น """ & _DB.CleanString(QuestionExplainAfterGenPath) & """(QuestionId=(" & ObjQuestionSet(0).ToString().Trim() & ")", True, "", ObjQuestionSet(0).ToString().Trim())
                UpdateStatus = 1
            End If

            If AnswerSet <> "" Then
                AnswerSet = System.Web.HttpUtility.UrlDecode(AnswerSet)
                'answer
                Dim ObjMainAnswerSet = Regex.Split(AnswerSet, "@~@") 'AnswerSet.Split("@~@")
                Dim AnswerName As String = ""
                'loop เพื่อ update คำตอบทีละข้อ , เงื่อนไขการจบ loop คือ วนจนครบหมดทุกคำตอบ
                For i As Integer = 0 To ObjMainAnswerSet.Length - 1
                    'loop update answer
                    ObjMainAnswerSet(i) = System.Web.HttpUtility.UrlDecode(ObjMainAnswerSet(i))
                    Dim ObjAnswerSet = ObjMainAnswerSet(i).Split("|")
                    Dim sqlGetAnswerOld As String = "Select Answer_Name_Quiz From tblanswer where Answer_id = '" & ObjAnswerSet(0).ToString().Trim() & "'"
                    Dim AnswerOld As DataTable
                    AnswerOld = _DB.getdataWithTransaction(sqlGetAnswerOld)


                    AnswerName = GenPathForSaveImage(ObjAnswerSet(1)).Replace(filePathAuthority, "")
                    sql = " UPDATE dbo.tblAnswer SET Answer_Name_Quiz = '" & _DB.CleanString(AnswerName) & "', LastUpdate = dbo.GetThaiDate() WHERE Answer_Id = '" & ObjAnswerSet(0).ToString().Trim() & "'; "
                    _DB.ExecuteWithTransection(sql)

                    If AnswerOld.Rows(0)("Answer_Name_Quiz") <> _DB.CleanString(AnswerName) Then
                        Log.Record(Log.LogType.ManageExam, "แก้ไขคำตอบเป็น """ & _DB.CleanString(AnswerName) & """(QuestionId=(" & ObjAnswerSet(0).ToString().Trim() & ")", True, "", ObjAnswerSet(0).ToString().Trim())
                        UpdateStatus = 1
                    End If
                Next
            End If

            If AnswerExplainSet <> "" Then
                AnswerExplainSet = System.Web.HttpUtility.UrlDecode(AnswerExplainSet)
                Dim AnswerExplain As String = ""
                Dim ObjMainAnswerExplainSet = Regex.Split(AnswerExplainSet, "@~@")
                'loop เพื่อ update คำอธิบายคำตอบทีละข้อ , เงื่อนไขการจบ loop คือ วนจนครบทุกคำตอบ
                For i As Integer = 0 To ObjMainAnswerExplainSet.Length - 1
                    'loop update answerExplain
                    ObjMainAnswerExplainSet(i) = System.Web.HttpUtility.UrlDecode(ObjMainAnswerExplainSet(i))
                    Dim ObjAnswerExplainSet = ObjMainAnswerExplainSet(i).Split("|")
                    'Dim ObjAnswerExplainSet = ObjMainAnswerExplainSet(i).Split("%7C")

                    Dim sqlGetAnswerOld As String = "Select Answer_Expain_Quiz From tblanswer where Answer_id = '" & ObjAnswerExplainSet(0).ToString().Trim() & "'"
                    Dim AnswerExpainOld As DataTable
                    AnswerExpainOld = _DB.getdataWithTransaction(sqlGetAnswerOld)

                    AnswerExplain = GenPathForSaveImage(ObjAnswerExplainSet(1).Replace(filePathAuthority, ""))
                    sql = " UPDATE dbo.tblAnswer SET Answer_Expain_Quiz = '" & _DB.CleanString(AnswerExplain) & "' , LastUpdate = dbo.GetThaiDate() WHERE Answer_Id = '" & ObjAnswerExplainSet(0).ToString().Trim() & "'; "
                    _DB.ExecuteWithTransection(sql)

                    If AnswerExpainOld.Rows(0)("Answer_Expain_Quiz") <> _DB.CleanString(AnswerExplain) Then
                        Log.Record(Log.LogType.ManageExam, "แก้ไขคำอธิบายคำตอบเป็น """ & _DB.CleanString(AnswerExplain) & """(QuestionId=(" & ObjAnswerExplainSet(0).ToString().Trim() & ")", True, "", ObjAnswerExplainSet(0).ToString().Trim())
                        UpdateStatus = 1
                    End If
                Next

            End If

            If UpdateStatus = 0 Then
                Log.Record(Log.LogType.ManageExam, "กดบันทึกปิดหน้าจอไปโดยไม่ได้แก้ไขอะไร (QuestionId=" & ObjQuestionSet(0).ToString().Trim() & ")", True, "", ObjQuestionSet(0).ToString().Trim())
            End If

            'commit transaction
            _DB.CommitTransection()

            'update log
            'Dim LogName As String = "แก้ไขข้อสอบจากหน้า Preview ที่ QuestionID=" & ObjQuestionSet(0).ToString()
            'Log.Record(Log.LogType.ManageExam, _DB.CleanString(LogName), True)

            'edit confirmed
            ClsLayoutCheckConfirmed.UpdateEditConfirmedThisQuestion(ObjQuestionSet(0).ToString().Trim(), ClsLayoutCheckConfirmed.LayoutType.Quiz)

            _DB = Nothing
            Return "complete"
        Catch ex As Exception
            _DB.RollbackTransection()
            _DB = Nothing
            Return "error"
        End Try
    End Function

    ''' <summary>
    ''' ทำการรับข้อมูลแล้วส่งเข้าไป update คำตอบใน Function ต่ออีกทอดหนึ่ง สำหรับ Type 3
    ''' </summary>
    ''' <param name="QsetName">คำถาม</param>
    ''' <param name="QuestionSet">เนื้อคำตอบ update QuestonName มี Pattern แบบนี้ : QuestionId|QuestionName@~@QuestionId|QuestionName@~@QuestionId|QuestionName</param>
    ''' <param name="AnswerSet">เนื้อคำตอบ update AnswerName มี Pattern แบบนี้ : AnswerId|AnswerName@~@AnswerId|AnswerName@~@AnswerId|AnswerName</param>
    ''' <param name="AnswerExplainSet">คำอธิบายคำตอบ มี Pattern แบบนี้ : AnswerId|AnswerExplain@~@AnswerId|AnswerExplain@~@AnswerId|AnswerExplain</param>
    ''' <returns>String:Error=ไม่สำเร็จ,Complete=สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveAfterEditType3(ByVal QsetName As String, ByVal QuestionSet As String, ByVal AnswerSet As String, ByVal AnswerExplainSet As String) As String
        If HttpContext.Current.Session("CurrentQsetId") IsNot Nothing AndAlso HttpContext.Current.Session("CurrentQsetId") <> "" Then
            'รายละเอียดของ Log
            Dim LogName As String = "แก้ไขข้อสอบจากหน้า Preview ที่ เป็น Type3 ที่ QsetId=" & HttpContext.Current.Session("CurrentQsetId").ToString()
            If UpdateType3And6(HttpUtility.UrlDecode(QsetName), HttpUtility.UrlDecode(QuestionSet), HttpUtility.UrlDecode(AnswerSet), HttpUtility.UrlDecode(AnswerExplainSet), LogName, True) = True Then
                Return "complete"
            Else
                Return "error"
            End If
        Else
            Return "error"
        End If
    End Function

    ''' <summary>
    ''' ทำการรับข้อมูลแล้วส่งเข้าไป update คำตอบใน Function ต่ออีกทอดหนึ่ง สำหรับ Type 6
    ''' </summary>
    ''' <param name="QsetName">คำถาม</param>
    ''' <param name="QuestionSet">เนื้อคำตอบ update QuestonName มี Pattern แบบนี้ : QuestionId|QuestionName@~@QuestionId|QuestionName@~@QuestionId|QuestionName</param>
    ''' <param name="AnswerExplainSet">คำอธิบายคำตอบ มี Pattern แบบนี้ : AnswerId|AnswerExplain@~@AnswerId|AnswerExplain@~@AnswerId|AnswerExplain</param>
    ''' <returns>String:Error=ไม่สำเร็จ,Complete=สำเร็จ</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function SaveAfterEditType6(ByVal QsetName As String, ByVal QuestionSet As String, ByVal AnswerExplainSet As String) As String
        If HttpContext.Current.Session("CurrentQsetId") IsNot Nothing AndAlso HttpContext.Current.Session("CurrentQsetId") <> "" Then
            Dim LogName As String = "แก้ไขข้อสอบจากหน้า Preview ที่ เป็น Type6 ที่ QsetId=" & HttpContext.Current.Session("CurrentQsetId").ToString()
            If UpdateType3And6(System.Web.HttpUtility.UrlDecode(QsetName), HttpUtility.UrlDecode(QuestionSet), "", HttpUtility.UrlDecode(AnswerExplainSet), LogName, False) = True Then
                Return "complete"
            Else
                Return "error"
            End If
        Else
            Return "error"
        End If
    End Function

    ''' <summary>
    ''' ทำการ update ข้อมูลคำถาม-คำตอบ ของคำถาม Type 3,6
    ''' </summary>
    ''' <param name="QsetName">คำถาม</param>
    ''' <param name="QuestionSet">เนื้อคำตอบ update QuestonName มี Pattern แบบนี้ : QuestionId|QuestionName@~@QuestionId|QuestionName@~@QuestionId|QuestionName</param>
    ''' <param name="AnswerSet">เนื้อคำตอบ update AnswerName มี Pattern แบบนี้ : AnswerId|AnswerName@~@AnswerId|AnswerName@~@AnswerId|AnswerName</param>
    ''' <param name="AnswerExplainSet">คำอธิบายคำตอบ มี Pattern แบบนี้ : AnswerId|AnswerExplain@~@AnswerId|AnswerExplain@~@AnswerId|AnswerExplain</param>
    ''' <param name="LogStr">รายละเอียดของ Log</param>
    ''' <param name="IsType3">เป็น Type3 ?</param>
    ''' <returns>Boolean:ไม่สำเร็จ=False,สำเร็จ=True</returns>
    ''' <remarks></remarks>
    Private Shared Function UpdateType3And6(ByVal QsetName As String, ByVal QuestionSet As String, ByVal AnswerSet As String, ByVal AnswerExplainSet As String, ByVal LogStr As String, ByVal IsType3 As Boolean) As Boolean
        Dim db As New ClassConnectSql()
        'open transaction
        db.OpenWithTransection()
        Try
            Dim sql As String = ""
            'qset
            Dim objQsetName = QsetName.Split("|")
            Dim CurrentQsetName As String = GenPathForSaveImage(objQsetName(1)).Replace(filePathAuthority, "")
            Dim QuestionExplain As String = GenPathForSaveImage(objQsetName(2)).Replace(filePathAuthority, "")
            'update คำถาม qset_name 
            sql = " UPDATE dbo.tblQuestionset SET QSet_Name_Quiz = '" & db.CleanString(CurrentQsetName) & "' , LastUpdate = dbo.GetThaiDate() WHERE QSet_Id = '" & HttpContext.Current.Session("CurrentQsetId").ToString() & "'; "
            db.ExecuteWithTransection(sql)
            'update คำอธิบายคำถาม questionExplain 
            sql = " UPDATE dbo.tblQuestion SET Question_Expain_Quiz = '" & db.CleanString(QuestionExplain) & "' , LastUPdate = dbo.GetThaiDate() WHERE Qset_Id ='" & HttpContext.Current.Session("CurrentQsetId").ToString() & "'; "
            db.ExecuteWithTransection(sql)

            'question
            Dim ObjMainQuestionSet = Regex.Split(QuestionSet, "@~@") 'QuestionSet.Split("@~@")
            'loop เพื่อทำการ update ข้อมูลคำตอบเข้าไปใน tblQuestion , เงื่อนไขการจบ loop วนจนครบทุกคำตอบ
            For i As Integer = 0 To ObjMainQuestionSet.Length - 1
                'loop update question
                Dim ObjQuestionSet = ObjMainQuestionSet(i).Split("|")
                Dim QuestionNameAfterGenPath As String = GenPathForSaveImage(ObjQuestionSet(1)).Replace(filePathAuthority, "")
                sql = " UPDATE dbo.tblQuestion SET Question_Name_Quiz = '" & db.CleanString(QuestionNameAfterGenPath) & "' , LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & ObjQuestionSet(0).ToString().Trim() & "'; "
                db.ExecuteWithTransection(sql)
            Next

            'answer
            If IsType3 = True Then
                Dim ObjMainAnswerSet = Regex.Split(AnswerSet, "@~@") 'AnswerSet.Split("@~@")
                Dim AnswerName As String = ""
                'loop เพื่อทำการ update ข้อมูลคำตอบเข้าไปใน tblAnswer , เงื่อนไขการจบ loop วนจนครบทุกคำตอบ
                For i As Integer = 0 To ObjMainAnswerSet.Length - 1
                    'loop update answer
                    Dim ObjAnswerSet = ObjMainAnswerSet(i).Split("|")
                    AnswerName = GenPathForSaveImage(ObjAnswerSet(1)).Replace(filePathAuthority, "")
                    sql = " UPDATE dbo.tblAnswer SET Answer_Name_Quiz = '" & db.CleanString(AnswerName) & "' , LastUpdate = dbo.GetThaiDate() WHERE Answer_Id = '" & ObjAnswerSet(0).ToString().Trim() & "'; "
                    db.ExecuteWithTransection(sql)
                Next
            End If

            'answerexplain
            Dim ObjMainAnswerExplainSet = Regex.Split(AnswerExplainSet, "@~@") 'AnswerSet.Split("@~@")
            Dim AnswerExplain As String = ""
            'loop เพื่อทำการ update ข้อมูลคำอธิบายคำตอบเข้าไปใน tblAnswer , เงื่อนไขการจบ loop วนจนครบทุกคำตอบ
            For i As Integer = 0 To ObjMainAnswerExplainSet.Length - 1
                'loop update answer
                Dim ObjAnswerExplainSet = ObjMainAnswerExplainSet(i).Split("|")
                AnswerExplain = GenPathForSaveImage(ObjAnswerExplainSet(1)).Replace(filePathAuthority, "")
                sql = " UPDATE dbo.tblAnswer SET Answer_Expain_Quiz = '" & db.CleanString(AnswerExplain) & "' , LastUpdate = dbo.GetThaiDate() WHERE Answer_Id = '" & ObjAnswerExplainSet(0).ToString().Trim() & "'; "
                db.ExecuteWithTransection(sql)
            Next

            'commit transaction
            db.CommitTransection()

            'update log
            Dim LogName As String = "แก้ไขข้อสอบจากหน้า Preview ที่ เป็น Type3 ที่ QsetId=" & HttpContext.Current.Session("CurrentQsetId").ToString()
            Log.Record(Log.LogType.ManageExam, db.CleanString(LogStr), True)

            'edit confirmed
            ClsLayoutCheckConfirmed.UpdateEditConfirmedThisQuestion("", ClsLayoutCheckConfirmed.LayoutType.Quiz, HttpContext.Current.Session("CurrentQsetId").ToString())

            db = Nothing
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            db = Nothing
            Return False
        End Try
    End Function

#End Region

#Region "Other"
    ''' <summary>
    ''' Function ทำการแปลง Path ของ Image ใหม่จาก ที่เป็นเลข QsetId หลายๆหลักให้ถูกแทนที่ด้วยคำว่า '___MODULE_URL___' แทน
    ''' </summary>
    ''' <param name="Path">เนื้อข้อมูล</param>
    ''' <returns>String:ที่ทำการ Replace ข้อมูลเรียบร้อยแล้ว</returns>
    ''' <remarks></remarks>
    Private Shared Function GenPathForSaveImage(ByVal Path As String) As String

        If Path <> "" And Path IsNot Nothing Then

            If Path.ToLower().IndexOf("<img") > -1 Then
                Path = ReplaceNeedlessString(Path)
            End If

            Dim QsetId As String = HttpContext.Current.Session("CurrentQsetId")
            Dim StrModule As String = "___MODULE_URL___"
            Dim rootPathHavePoint As String = "../file/"
            Dim rootPathNoPoint As String = "/File/"
            Dim filePathNoPoint As String
            Dim filePathHavePoint As String
            Dim SubStringQset As String = ""
            Dim PathComplete1 As String
            Dim PathComplete2 As String

            filePathNoPoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) + _
         "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) + _
         "/" + QsetId.Substring(6, 1) + "/" + QsetId.Substring(7, 1) + "/"
            SubStringQset = filePathNoPoint
            filePathNoPoint = rootPathNoPoint + filePathNoPoint + "{" + QsetId + "}/"

            filePathHavePoint = QsetId.Substring(0, 1) + "/" + QsetId.Substring(1, 1) + "/" + QsetId.Substring(2, 1) + _
             "/" + QsetId.Substring(3, 1) + "/" + QsetId.Substring(4, 1) + "/" + QsetId.Substring(5, 1) + _
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
    Private Shared Function ReplaceNeedlessString(ByVal InputString As String)

        'Return InputString
        If InputString.IndexOf("../file") > 0 Then
            Return InputString
        End If
        'ตัวแปรที่เอาไว้เก็บ string ที่เราจะนำไป Replace ทิ้ง
        Dim NeedlessString As String = ""
        If InputString.IndexOf("/File") >= 0 Then
            Dim a As Integer = 0
            Dim b As Integer = InputString.IndexOf("/File")
            a = b

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

            a = InStrRev(InputString, """", a)
            b = b - a
            NeedlessString = InputString.Substring(a, b)
            If NeedlessString <> "" Then
                InputString = InputString.Replace(NeedlessString, "")
            End If
        End If

        Return InputString

    End Function
#End Region
     

End Class