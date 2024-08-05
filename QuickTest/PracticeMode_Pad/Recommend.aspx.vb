Imports System.Web
Public Class Recommend
    Inherits System.Web.UI.Page


    Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
    Public DVID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Quiz_Id As String = "C8CCB2BB-420E-4E0D-A48A-A86D2E5134D3"
        'Dim Player_Id As String = "4C12E915-84DB-4E5C-A9B5-00E57A84CF02"
        'Session("PracticeFromComputer") = True
        'Session("PracticeMode") = True

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

        Dim Quiz_Id As String = Session("Quiz_ID")
        Dim Player_Id As String = ClsPracticeMode.GetPlayerInQuiz(Quiz_Id)
        Dim SubjectId As String = ClsPracticeMode.GetSubjectInQuiz(Quiz_Id)
        Dim dt As DataTable = ClsPracticeMode.GetQSetNearQuiz(Quiz_Id, Player_Id, SubjectId)
        Dim PositionCategory As Integer
        Dim QuestionAfTerLine As String
        Dim QuestionSet As String
        Dim Strcut As String
        For Each a In dt.Rows

            If HttpContext.Current.Application("Sess" & Session("PDeviceId")) IsNot Nothing Then
                HttpContext.Current.Application("Sess" & Session("PDeviceId")) = Nothing
            End If

            QuestionSet = a("QSet_Name")
            Dim index As Integer = QuestionSet.IndexOf("</b> - ")
            PositionCategory = InStr(QuestionSet, "</b> - ")
            QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

            'If QuestionAfTerLine.Length > 75 Then
            '    Strcut = CutString(a("QSet_Id").ToString)
            'Else
            Strcut = a("QSet_Name")
            'End If

            a("QSet_Name") = Strcut
            Strcut = ""
        Next

        Listing.DataSource = dt
        Listing.DataBind()

    End Sub

    ''Private Function CutString(ByVal QSetId As String) As String
    'Dim clsData As New ClassConnectSql
    'Dim dtQuestionSet As New DataTable

    ''Dim QCategoryName As String
    'Dim sqlQuestionSet As String = "Select qs.QSet_Name,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc Where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id = '" & QSetId & "'"
    '    dtQuestionSet = New DataTable
    '    dtQuestionSet = clsData.getdata(sqlQuestionSet)

    'Dim QuestionSetName As String = dtQuestionSet.Rows(0)("QSet_Name")

    '' QCategoryName = dtQuestionSet.Rows(0)("QCategory_Name")
    'Dim CompleteStr As String
    'Dim CheckBrOld As Boolean = QuestionSetName.Contains("<br>")
    'Dim CheckBrNew As Boolean = QuestionSetName.Contains("<br />")
    'Dim CutQuestionSetName As String

    '    If QuestionSetName.ToString.Length > 50 Then

    '        If CheckBrNew = False And CheckBrOld = False Then
    '            CutQuestionSetName = QuestionSetName.Substring(0, 50) & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
    ''CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & 
    '            CompleteStr = CutQuestionSetName
    '        Else

    'Dim InstrOldBr As String
    'Dim CutStrNewBr As String
    'Dim CutStrOldBr As String

    '            If CheckBrOld = True Then
    '                InstrOldBr = InStr(QuestionSetName, "<br>")
    '                CutStrOldBr = QuestionSetName.Substring(0, InstrOldBr - 1)
    '            Else
    '                CutStrOldBr = QuestionSetName
    '            End If

    '            If CheckBrNew = True Then
    'Dim InstrNewBr As String = InStr(CutStrOldBr, "<br />")
    '                If InstrNewBr <> 0 Then
    '                    CutStrNewBr = QuestionSetName.Substring(0, InstrNewBr - 1) & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
    '                Else
    '                    CutStrNewBr = CutStrOldBr & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
    '                End If
    '            Else
    '                CutStrNewBr = CutStrOldBr & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
    '            End If

    ''CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & 
    '            CompleteStr = CutStrNewBr

    '        End If

    '    End If

    '    Return CompleteStr
    'End Function

    <Services.WebMethod()>
    Public Shared Function SaveTestsetAndQuiz(ByVal QsetId As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim ClsPracticeMode As New ClsPracticeMode(New ClassConnectSql)
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim KnSession As New ClsKNSession()
        Dim ClassName As String
        Dim RoomName As String
        Dim SchoolCode As String
        Dim player_Id As String
        Dim IsUseTablet As String

        If HttpContext.Current.Session("PracticeFromComputer") Then
            ClassName = "ม.1"
            RoomName = "/1"
            SchoolCode = HttpContext.Current.Application("DefaultSchoolCode")
            player_Id = HttpContext.Current.Application("DefaultUserId")
            IsUseTablet = "0"
        Else
            Dim dtPlayer As DataTable = ClsPracticeMode.GetPlayerDetail(HttpContext.Current.Session("PDeviceId")) 'หาข้อมูลนักเรียนคนนั้น
            ClassName = dtPlayer.Rows(0)("ClassName").ToString
            RoomName = dtPlayer.Rows(0)("RoomName").ToString
            SchoolCode = dtPlayer.Rows(0)("School_Code").ToString
            player_Id = dtPlayer.Rows(0)("Student_Id").ToString
            IsUseTablet = "1"
        End If




        HttpContext.Current.Session("UserId") = player_Id


        Dim testsetId As String = ClsPracticeMode.GetTestsetId(QsetId, player_Id, SchoolCode, HttpContext.Current.Session("PClassId"))

        Dim KNS As New KNAppSession()
        Dim Calendar_Id = KNS.StoredValue("CurrentCalendarId").ToString()

        HttpContext.Current.Session("Quiz_Id") = ClsPracticeMode.SaveQuizDetail(testsetId, ClassName, RoomName, SchoolCode, player_Id, IsUseTablet, False, Calendar_Id) 'เปิด Quiz


        Dim testsetValue() As String = Split(testsetId, ",")

        If testsetValue(1) = "New" Then 'Testset ใหม่ต้องเอาข้อสอบใส่ Table TestsetQuestionSet และ TestsetQuestionDetail ด้วย
            ClsPracticeMode.SetTSQSAndSetTSQD(testsetValue(0), QsetId)
        End If

        ClsActivity.getQsetInQuiz(testsetId, HttpContext.Current.Session("Quiz_Id")) 'insertQuestionToQuizQuestion

        If Not HttpContext.Current.Session("PracticeFromComputer") Then
            ClsActivity.InsertQuizScorePracticeMode(HttpContext.Current.Session("Quiz_Id"), SchoolCode, 1, HttpContext.Current.Session("PracticeFromComputer"), ClsPracticeMode.GetQSetTypeFromQSetId(QsetId))
        End If

        ClsPracticeMode.SaveQuiznswer(HttpContext.Current.Session("Quiz_Id"), HttpContext.Current.Session("UserId"), SchoolCode, HttpContext.Current.Session("PDeviceId"), HttpContext.Current.Session("PracticeFromComputer"))


        KnSession(HttpContext.Current.Session("Quiz_Id") & "|" & "SelfPace") = True

        If HttpContext.Current.Session("PracticeFromComputer") Then
            HttpContext.Current.Session("SchoolId") = SchoolCode
            HttpContext.Current.Session("QuizUseTablet") = "False"
            Return "../Activity/ActivityPage.aspx"
        Else
            Return "../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & HttpContext.Current.Session("PDeviceId")
        End If

    End Function

    'Protected Friend Function CreateQsetUnit(ByVal AAA As String)

    '    Dim Quiz_Id As String = "C8CCB2BB-420E-4E0D-A48A-A86D2E5134D3"
    '    Dim Player_Id As String = "4C12E915-84DB-4E5C-A9B5-00E57A84CF02"
    '    Dim SubjectId As String = ClsPracticeMode.GetSubjectInQuiz(Quiz_Id)

    '    Dim dt As DataTable = ClsPracticeMode.GetQSetNearQuiz(Quiz_Id, Player_Id, SubjectId)
    '    'Dim PositionCategory As Integer
    '    'Dim QuestionAfTerLine As String
    '    'Dim QuestionSet As String
    '    'Dim Strcut As String
    '    'For Each a In dt.Rows

    '    '    QuestionSet = a("QSet_Name")
    '    '    Dim index As Integer = QuestionSet.IndexOf("</b> - ")
    '    '    PositionCategory = InStr(QuestionSet, "</b> - ")
    '    '    QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

    '    '    'If QuestionAfTerLine.Length > 75 Then
    '    '    '    Strcut = CutString(a("QSet_Id").ToString)
    '    '    'Else
    '    '    Strcut = a("QSet_Name")
    '    '    'End If

    '    '    a("QSet_Name") = Strcut
    '    '    Strcut = ""
    '    'Next

    '    Dim sb As New System.Text.StringBuilder()



    '    If Not IsNothing(dt) Then
    '        Dim qSetId, QuestionSet, Ispracticed As String
    '        For i = 0 To dt.Rows.Count - 1
    '            qSetId = dt.Rows(i)("Qset_Id").ToString()
    '            QuestionSet = dt.Rows(i)("QSet_Name").ToString()
    '            Ispracticed = dt.Rows(i)("IsPracticed").ToString
    '            'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)

    '            sb.Append("<tr id=" & qSetId & " ><td>")
    '            sb.Append(" <img id=""play_" & qSetId & """ src=""../Images/upgradeClass/Actions-arrow-right-icon.png"" class=""imgPlayQuiz"" />")


    '            'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
    '            'Dim PositionCategory As Integer
    '            'Dim QuestionAfTerLine As String
    '            'Dim index As Integer = QuestionSet.IndexOf("</b> - ")
    '            '        PositionCategory = InStr(QuestionSet, "</b> - ")
    '            '        QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

    '            '        If QuestionAfTerLine.Length > 75 Then
    '            'Dim Strcut As String = CutString(qSetId)
    '            '            sb.Append(Strcut)
    '            '        Else
    '            sb.Append(QuestionSet)
    '            'End If

    '            'If ExamAmount.Equals("0") Then
    '            '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a></td></tr>")
    '            'Else
    '            '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
    '            'End If
    '            sb.Append(" <img src=""../Images/Delete-icon.png"" style='float:right; cursor: pointer; visibility:" & Ispracticed & "' />")
    '            sb.Append("</td></tr>")

    '        Next
    '    End If
    '    Return sb.ToString()

    'End Function

    'Protected Friend Function CreateCategoryUnit(ByVal Qcategory_Id As String)

    '    Dim Quiz_Id As String = "C8CCB2BB-420E-4E0D-A48A-A86D2E5134D3"
    '    Dim Player_Id As String = "4C12E915-84DB-4E5C-A9B5-00E57A84CF02"
    '    Dim SubjectId As String = ClsPracticeMode.GetSubjectInQuiz(Quiz_Id)

    '    Dim dt As DataTable = ClsPracticeMode.GetQSetByQcategory(Qcategory_Id, player_Id, Session("PracticeFromComputer"))
    '    Dim sb As New System.Text.StringBuilder()



    '    If Not IsNothing(dt) Then
    '        Dim qSetId, QuestionSet, Ispracticed As String
    '        For i = 0 To dt.Rows.Count - 1
    '            qSetId = dt.Rows(i)("Qset_Id").ToString()
    '            QuestionSet = dt.Rows(i)("QSet_Name").ToString()
    '            Ispracticed = dt.Rows(i)("IsPracticed").ToString
    '            'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)

    '            sb.Append("<tr id=" & qSetId & " ><td>")
    '            sb.Append(" <img id=""play_" & qSetId & """ src=""../Images/upgradeClass/Actions-arrow-right-icon.png"" class=""imgPlayQuiz"" />")


    '            'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
    '            'Dim PositionCategory As Integer
    '            'Dim QuestionAfTerLine As String
    '            'Dim index As Integer = QuestionSet.IndexOf("</b> - ")
    '            '        PositionCategory = InStr(QuestionSet, "</b> - ")
    '            '        QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

    '            '        If QuestionAfTerLine.Length > 75 Then
    '            'Dim Strcut As String = CutString(qSetId)
    '            '            sb.Append(Strcut)
    '            '        Else
    '            sb.Append(QuestionSet)
    '            'End If

    '            'If ExamAmount.Equals("0") Then
    '            '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a></td></tr>")
    '            'Else
    '            '    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
    '            'End If
    '            sb.Append(" <img src=""../Images/Delete-icon.png"" style='float:right; cursor: pointer; visibility:" & Ispracticed & "' />")
    '            sb.Append("</td></tr>")

    '        Next
    '    End If
    '    Return sb.ToString()

    'End Function

End Class