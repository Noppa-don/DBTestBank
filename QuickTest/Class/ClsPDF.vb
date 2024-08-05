Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System
Imports iTextSharp.text.html.simpleparser

<Serializable()> _
Public Class ClsPDF
    Dim _DB As ClsConnect
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Enum FontSize
        ExtraLarge
        Large
        Medium
        Small
    End Enum

    Public Enum QsetType
        Choice
        TrueFalse
        Match
    End Enum

    Public Function GetFontSize(ByVal newTestSetId As String) As String
        Dim sql As String = " select convert(char(36),level_id) as level_id from tbltestset where testset_id = '" & newTestSetId & "';"
        Dim levelId As String = _DB.ExecuteScalar(sql).ToString()

        'If levelId = "5F4765DB-0917-470B-8E43-6D1C7B030818" Then 'ตัวใหญ่มาก 'ป.1
        '    Return "56,52,110"
        'ElseIf levelId = "93B163B6-4F87-476D-8571-4029A6F34C84" Then 'ตัวขนาดใหญ่กลางๆ  'ป.6
        '    Return "170,160,150"
        'Else 'ตัวปกติ "6BF52DC7-314C-40ED-B7F3-BCC87F724880" 'ม.6
        'Return "110,100,90"
        'End If

        Return "50,50,50"


    End Function
    Public Sub CreateUserPassPDF(ByVal UserId As Integer, ByVal path As String)

        Dim sql As String = "SELECT dbo.tblUser.FirstName, dbo.tblUser.LastName, dbo.tblSchool.SchoolName, dbo.tblSchool.SchoolId, dbo.tblUser.UserName, dbo.tblUser.Password "
        sql &= "FROM dbo.tblUser INNER JOIN dbo.tblSchool ON dbo.tblUser.SchoolId = dbo.tblSchool.SchoolId "
        sql &= "WHERE dbo.tblUser.UserId = " & UserId

        Dim a As DataTable = _DB.getdata(sql)
        Dim SchoolName As String = a.Rows(0)("SchoolName")
        Dim Name As String = a.Rows(0)("FirstName") & "  " & a.Rows(0)("LastName")
        Dim SchoolId As String = a.Rows(0)("SchoolId").ToString
        Dim User As String = a.Rows(0)("UserName")
        Dim Password As String = a.Rows(0)("Password")

        Dim Document As New Document(PageSize.A4, 20, 20, 20, 20)
        Dim fileName As String = path & "Usermanager/pdf/" & UserId & ".pdf"
        PdfWriter.GetInstance(Document, New FileStream(fileName, FileMode.Create))

        Document.AddAuthor("QuickTest")
        Document.AddSubject("UserReport")

        Dim Logo As String = ""
        Logo = path & "UploadPic/pic.jpg"
        Dim ImgLogo As Image = Image.GetInstance(Logo)
        ImgLogo.ScaleAbsolute(300, 300)
        ImgLogo.Alignment = iTextSharp.text.Image.ALIGN_CENTER

        Dim BG As String = ""
        BG = path & "Images/Border.png"
        Dim imgBG As Image = Image.GetInstance(BG)
        imgBG.ScaleAbsolute(500, 270)
        imgBG.Alignment = iTextSharp.text.Image.UNDERLYING
        imgBG.SetAbsolutePosition(50, 10)

        Dim EnCodeFont As BaseFont = BaseFont.CreateFont((path & "fonts/DB ThaiText X.ttf").ToString, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)

        Dim SmallerWhiteFont As New iTextSharp.text.Font(EnCodeFont, 8, iTextSharp.text.Font.NORMAL, New BaseColor(255, 255, 255))

        Dim SmallFont As New iTextSharp.text.Font(EnCodeFont, 10, iTextSharp.text.Font.NORMAL, New BaseColor(0, 0, 255))
        Dim SmallWhiteFont As New iTextSharp.text.Font(EnCodeFont, 10, iTextSharp.text.Font.BOLD, New BaseColor(255, 255, 255))

        Dim MediumFont As New iTextSharp.text.Font(EnCodeFont, 14, iTextSharp.text.Font.NORMAL, New BaseColor(0, 0, 255))
        Dim MediumBoldFont As New iTextSharp.text.Font(EnCodeFont, 14, iTextSharp.text.Font.BOLD, New BaseColor(0, 0, 255))
        Dim MediumWhiteFont As New iTextSharp.text.Font(EnCodeFont, 13, iTextSharp.text.Font.NORMAL, New BaseColor(255, 255, 255))

        Dim BigFont As New iTextSharp.text.Font(EnCodeFont, 28, iTextSharp.text.Font.BOLD, New BaseColor(0, 0, 255))



        Dim BigWhiteFont As New iTextSharp.text.Font(EnCodeFont, 20, iTextSharp.text.Font.NORMAL, New BaseColor(255, 255, 255))

        Document.Open()
        Document.NewPage()
        Document.Add(ImgLogo)

        'Document.Add(imBGSpace)
        Document.Add(imgBG)


        Document.Add(New Paragraph(".", BigWhiteFont))

        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", BigWhiteFont))
        Document.Add(New Paragraph(".", SmallerWhiteFont))
        Document.Add(New Paragraph(".", SmallerWhiteFont))




        Document.Add(New Paragraph("                       QuickTest", BigFont))
        Document.Add(New Paragraph(".", MediumWhiteFont))
        Document.Add(New Paragraph("                                                 สำหรับโรงเรียน " & SchoolName & "    " & "อาจารย์ " & Name, SmallFont))
        Document.Add(New Paragraph(".", MediumWhiteFont))

        Document.Add(New Paragraph("                                            รหัสโรงเรียน " & "     " & SchoolId, MediumBoldFont))

        Document.Add(New Paragraph("                                            ชื่อ " & "                 " & User, MediumBoldFont))

        Document.Add(New Paragraph("                                            รหัสผ่าน " & "          " & Password, MediumBoldFont))
        Document.Add(New Paragraph(".", SmallWhiteFont))


        Document.Add(New Paragraph("                                                 By   วัฒนาพานิช ", MediumFont))

        Document.Close()

    End Sub

    ''' <summary>
    ''' หาคำถามสำหรับนำไป Gen ไฟล์ Word โดยถ้าเป็นโหมดแบบ Preview จะเลือกไปแค่ Qset เดียว เอาไปเป็นตัวอย่างเฉยๆ
    ''' </summary>
    ''' <param name="testSetId">Id ของ tblTestset ชุดข้อสอบที่เลือกมา</param>
    ''' <param name="IsPreview">เป็นโหมดแบบแสดงแค่บางส่วนใช่ไหม ?</param>
    ''' <returns>Datatable คำถาม</returns>
    ''' <remarks></remarks>
    Public Function GetTSQS(ByVal testSetId As String, ByVal IsPreview As Boolean) As DataTable
        Dim sql As String
        If IsPreview = True Then
            'sql = " select top 1 tsqs.QSet_Id, qs.QSet_Type from tbltestsetquestionset tsqs, tblQuestionSet qs where testset_id in (select testset_id from tblTestSet ) "
            'sql &= " and tsqs.QSet_Id = qs.QSet_Id and  TestSet_Id = '" & testSetId & "' order by tsqs.TSQS_No "
            'GetTSQS = _DB.getdata(sql)

            sql = "  SELECT TOP 1 qs.QSet_Id,qs.QSet_Type FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionset qs ON tsqs.QSet_Id = qs.QSet_Id "
            sql &= " WHERE tsqs.TestSet_Id = '" & testSetId & "' AND tsqs.IsActive = 1 ORDER BY tsqs.TSQS_No; "
        Else
            'sql = " select tsqs.QSet_Id, qs.QSet_Type from tbltestsetquestionset tsqs, tblQuestionSet qs where testset_id in (select testset_id from tblTestSet ) "
            'sql &= " and tsqs.QSet_Id = qs.QSet_Id and  TestSet_Id = '" & testSetId & "'  order by tsqs.TSQS_No "

            sql = "  SELECT qs.QSet_Id,qs.QSet_Type FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionset qs ON tsqs.QSet_Id = qs.QSet_Id "
            sql &= " WHERE tsqs.TestSet_Id = '" & testSetId & "' AND tsqs.IsActive = 1 ORDER BY tsqs.TSQS_No; "
        End If
        'and qs.qset_id = 'eff55910-87d8-4d08-9ebd-2f792fa67b0b'
        'GetTSQS = _DB.getdata(sql)
        'Return GetTSQS
        Return _DB.getdata(sql)
    End Function

    ''' <summary>
    ''' ทำการหาคำถามจากชุดข้อสอบที่เลือกมา
    ''' </summary>
    ''' <param name="TestSetId">Id ของ tblTestset ชุดข้อสอบที่เลือกมา</param>
    ''' <param name="QuestionSetId">Id ของ tblQuestionSet ชุดคำถามที่เลือกมา</param>
    ''' <param name="IsPreview">เป็นโหมดแสดงเฉพาะบางส่วน ?</param>
    ''' <returns>Datatable คำถาม</returns>
    ''' <remarks></remarks>
    Public Function GetQuestion(ByVal TestSetId As String, ByVal QuestionSetId As String, ByVal IsPreview As Boolean) As DataTable
        Dim sql As String = ""
        'ถ้าเป็นโหมดแสดงแค่บางส่วน ให้ Select ไปแค่ 10 ข้อพอ เอาไปเป็นตัวอย่างเฉยๆ
        If IsPreview Then
            sql = " select top 10 tsqd.Question_Id,q.Question_Name,qs.QSet_Name from tbltestsetquestiondetail  tsqd,tblQuestion q, tblQuestionSet qs  where tsqd.tsqs_id in ("
            sql &= " select tsqs_id from tbltestsetquestionset tsqs where testset_id = '" & TestSetId & "' ) "
            sql &= " and qs.QSet_Id = q.QSet_Id     and tsqd.Question_Id = q.Question_Id and q.qset_id = '" & QuestionSetId & "' AND q.IsActive = 1 AND tsqd.IsActive = 1 order by tsqd.tsqd_no ;"
        Else
            sql = " select tsqd.Question_Id,q.Question_Name,qs.QSet_Name,q.Question_Expain from tbltestsetquestiondetail  tsqd,tblQuestion q, tblQuestionSet qs  where tsqd.tsqs_id in ("
            sql &= " select tsqs_id from tbltestsetquestionset tsqs where testset_id = '" & TestSetId & "' ) "
            sql &= " and qs.QSet_Id = q.QSet_Id     and tsqd.Question_Id = q.Question_Id and q.qset_id = '" & QuestionSetId & "' AND q.IsActive = 1 AND tsqd.IsActive = 1 order by tsqd.tsqd_no ;"
            ' and q.question_id <> 'FBAB584A-39E9-41CA-8D94-367545A8C481'   
        End If

        Dim dt As DataTable = _DB.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' หาข้อมูลคำตอบของคำถามข้อนี้
    ''' </summary>
    ''' <param name="Question_Id">Id ของ tblQuestion คำถาม</param>
    ''' <returns>Datatable ของคำตอบ ของคำถามที่เลือกมา</returns>
    ''' <remarks></remarks>
    Public Function GetAnswerDetails(ByVal Question_Id As String) As DataTable 'Choice, TrueFalse
        Dim sql As String
        sql = "select a.Answer_Name, a.Answer_Score,Answer_Expain from tblAnswer a "
        sql &= " where A.Question_Id = '" & Question_Id & "' AND a.IsActive = 1 "
        sql &= " order by Answer_No;"
        Dim dt As DataTable = _DB.getdata(sql)
        Return dt
    End Function

    Public Function GetMatchAnswerDetails(ByVal QuestionSetId As String, ByVal TestSetId As String) As DataTable

        Dim sql As String
        'พี่ชินปิดไป 18/6/55
        ' ''sql = " select a.Answer_Name from tbltestsetquestiondetail  tsqd,tblQuestion q, tblQuestionSet qs ,tblAnswer a"
        ' ''sql &= " where tsqd.tsqs_id in ( select top 10 tsqs_id from tbltestsetquestionset tsqs where testset_id = '7A91A026-5E4D-46F2-ABBE-E3CCE69AFEDF' )  "
        ' ''sql &= " and qs.QSet_Id = q.QSet_Id     "
        ' ''sql &= " and tsqd.Question_Id = q.Question_Id and q.qset_id = '" & QuestionSetId & "' and a.Question_Id = tsqd.Question_Id"
        ' ''sql &= " order by tsqd_no;"

        'พี่ชินเขียนใหม่ 18/6
        sql = " SELECT tblAnswer.Answer_Name FROM tblAnswer INNER JOIN tblQuestion ON tblAnswer.Question_Id = "
        sql = sql + " tblQuestion.Question_Id INNER JOIN tblTestSetQuestionDetail ON tblQuestion.Question_Id = "
        sql = sql + " tblTestSetQuestionDetail.Question_Id INNER JOIN tblTestSetQuestionSet ON "
        sql = sql + " tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_Id "
        sql = sql + " where tblTestSetQuestionSet.TestSet_Id = '" & TestSetId & "' "
        sql = sql + " and tblTestSetQuestionSet.QSet_Id = '" & QuestionSetId & "';"

        ''sql = "select Answer_Name from tblanswer a,tblQuestionSet Qs,tblQuestion Q where(Qs.QSet_Id = Q.QSet_Id)"
        ''sql &= "And Q.Question_Id = a.Question_Id And Qs.QSet_Type = '3' And Qs.QSet_Id = '" & QuestionSetId & "' order by newid();"

        'sql = "select Answer_Name from tblanswer a,tblQuestionSet Qs,tblQuestion Q where(Qs.QSet_Id = Q.QSet_Id)"
        'sql &= "And Q.Question_Id = a.Question_Id And Qs.QSet_Type = '3' And Qs.QSet_Id = '" & QuestionSetId & "';"
        Dim dt As DataTable = _DB.getdata(sql)
        Return dt
    End Function
    Public Function GetQSetName(ByVal QSetId As String) As String
        Dim sql As String = ""
        sql = " Select Qset_Name from tblQuestionSet where QSet_Id = '" & QSetId & "';"
        Dim dt As DataTable = _DB.getdata(sql)
        GetQSetName = dt.Rows(0)("Qset_Name")
        Return GetQSetName
    End Function
    Public Function GetHeader(ByVal TestSetId As String) As DataTable
        Dim sql As String
        sql = " select t.TestSet_Name,s.SchoolName,t.TestSet_Time from tblTestSet t , tblSchool s "
        sql &= " where(t.SchoolId = s.SchoolId) and t.TestSet_Id = '" & TestSetId & "';"
        GetHeader = _DB.getdata(sql)
    End Function
    Public Function GetExamAmount(ByVal TestSetId As String) As String
        Dim sql As String
        sql = " select count(*) as ExamAmount from tbltestsetquestiondetail  tsqd, tblTestSetQuestionSet tsqs , tblTestSet t "
        sql &= " where  t.TestSet_Id = '" & TestSetId & "' and t.isactive = 1 and tsqs.testset_id = t.testset_id and tsqs.isactive = 1 and tsqs.tsqs_id = tsqd.tsqs_id and tsqd.isactive=1;"

        Dim dt As DataTable = _DB.getdata(sql)
        GetExamAmount = dt.Rows(0)("ExamAmount")
    End Function

    ''' <summary>
    ''' ทำการต่อสตริงประกอบ Path รูปขึ้นมาตาม QsetId
    ''' </summary>
    ''' <param name="QSetId">QsetId ของคำถาม/คำตอบ ข้อนั้น</param>
    ''' <returns>สตริง Path รูปที่ถูกต้องตาม QsetId</returns>
    ''' <remarks></remarks>
    Public Function GenFilePath(ByVal QSetId As String) As String
        Dim rootPath As String = "../file/"
        Dim filePath As String
        filePath = QSetId.Substring(0, 1) + "/" + QSetId.Substring(1, 1) + "/" + QSetId.Substring(2, 1) + _
            "/" + QSetId.Substring(3, 1) + "/" + QSetId.Substring(4, 1) + "/" + QSetId.Substring(5, 1) + _
            "/" + QSetId.Substring(6, 1) + "/" + QSetId.Substring(7, 1) + "/"
        filePath = filePath + "{" + QSetId + "}/"
        Return rootPath + filePath
    End Function


    Public Function CleanSetNameText(ByVal inString As String) As String
        Dim tmpOutput As String = ""
        tmpOutput = RemoveUnitAndNumberTextStringFromQuestion(inString)
        Return RemoveDuplicationBRTag(tmpOutput)
    End Function
    Public Function CleanQuestionText(ByVal inString As String) As String
        Dim tmpOutput As String = ""
        tmpOutput = TrimSpaceAfter(inString)
        Return RemoveDuplicationBRTag(tmpOutput)
    End Function
    Public Function CleanAnswerText(ByVal inString As String) As String
        Dim tmpOutput As String = ""
        tmpOutput = TrimSpaceAfter(inString)
        Return RemoveDuplicationBRTag(tmpOutput)
    End Function
    ''' <summary>
    ''' ทำการ replace string Tag Br ให้ถูกต้อง
    ''' </summary>
    ''' <param name="inString">ข้อมูลที่ต้องการให้ Replace</param>
    ''' <returns>ข้อมูลที่ Replace Tag BR เรียบร้อย</returns>
    ''' <remarks></remarks>
    Public Function RemoveDuplicationBRTag(ByVal inString As String) As String
        Return inString.Replace("<br /><br />", "<br />").Replace("<br><br>", "<br>").Replace("<br>  <p>&nbsp;</p><br>", "<br />").Replace("<br>", "<br />")
    End Function

    Private Function TrimSpaceAfter(ByVal inString As String) As String

        '<br /><br />&nbsp;</p>
        If inString.EndsWith("<br /><br />&nbsp;</p>") Then
            inString = inString.Substring(0, inString.Length - 22) + "</p>"
        End If
        '<br><br>
        If inString.EndsWith("<br><br>") Then
            inString = inString.Substring(0, inString.Length - 8)
        End If
        '<br /><br />
        If inString.EndsWith("<br /><br />") Then
            inString = inString.Substring(0, inString.Length - 12)
        End If
        '<br><br><stong>
        If inString.EndsWith("<br><br></stong>") Then
            inString = inString.Substring(0, inString.Length - 16) + "</stong>"
        End If

        Return inString
    End Function

    Private Function RemoveUnitAndNumberTextStringFromQuestion(ByVal inString As String) As String

        If inString Is Nothing Then
            inString = ""
        End If

        Dim posNumberText As Integer = -1
        posNumberText = inString.IndexOf("ข้อ ")
        Dim posCutText As Integer = -1
        If posNumberText >= 0 Then
            posCutText = inString.IndexOf(" ", posNumberText + 4)

            If posCutText > 0 Then
                If inString.Length > (posCutText + 1) Then
                    Return inString.Substring(posCutText + 1)
                End If
            End If
        End If


        Return inString
    End Function
End Class
