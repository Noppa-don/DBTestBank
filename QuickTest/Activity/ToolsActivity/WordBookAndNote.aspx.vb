Imports System.Web
Public Class WordBook
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ' สร้าง หน้าที่เป็นสมุดคำศัพท์
    <Services.WebMethod()>
    Public Shared Function createWordBook(ByVal QuestionId As String, ByVal Alphabet As String, ByVal ChangeAlphabet As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Try
            If ChangeAlphabet = "1" Then
                Alphabet = getNextAlphabet(QuestionId, Alphabet)
            End If

            Dim dtWords = getWords(QuestionId, Alphabet)

            Dim words As String = ""
            Dim returnData As String = ""
            Dim verb As String = ""
            Dim ArrWords As New Dictionary(Of String, String)()

            Dim removeTagHTML As String = ""
            Dim dataHtml As String = ""

            For Each row In dtWords.Rows
                words = row("words").ToString()
                returnData = row("ReturnData").ToString().Replace("$F$", "")

                If ArrWords.ContainsKey(words) = False Then
                    removeTagHTML = StripTags(returnData)
                    If InStr(removeTagHTML, "<br />คำตรงข้าม") Then
                        removeTagHTML = removeTagHTML.Replace("<br />คำตรงข้าม", "| คำตรงข้าม")
                        removeTagHTML = removeTagHTML.Replace("<br />", "<br /><f>")
                        removeTagHTML = removeTagHTML.Replace("|", "</f><br /><f>")
                    Else
                        removeTagHTML = removeTagHTML.Replace("<br />", "<br /><f>")
                    End If
                    dataHtml = removeTagHTML.Insert(removeTagHTML.Length, "</f>")
                    ArrWords.Add(words, dataHtml)
                    verb = row("ecat")
                Else
                    'check ก่อนว่า verb เดิมหรือเปล่า
                    Dim newData As String = ""
                    If (row("ecat") = verb) And (row("esynAsRelated") = "1") Then
                        Dim rep As String = "<b><i>" & row("ecat").ToString() & "</i></b> :"
                        newData = " , " & returnData.ToString().Replace(rep, "")
                    ElseIf (row("ecat") <> verb) And (row("esynAsRelated") = "1") Then
                        newData = "<br />" + StripTags(returnData)
                        verb = row("ecat")
                    Else
                        removeTagHTML = StripTags(returnData)
                        If InStr(removeTagHTML, "<br />คำตรงข้าม") Then
                            removeTagHTML = removeTagHTML.Replace("<br />คำตรงข้าม", "| คำตรงข้าม")
                            removeTagHTML = removeTagHTML.Replace("<br />", "<br /><f>")
                            removeTagHTML = removeTagHTML.Replace("|", "</f><br /><f>")
                        Else
                            removeTagHTML = removeTagHTML.Replace("<br />", "<br /><f>")
                        End If
                        dataHtml = removeTagHTML.Insert(removeTagHTML.Length, "</f>")
                        newData = "<br /> " + dataHtml
                        verb = row("ecat")
                    End If

                    Dim tmpData As String = ""
                    If ArrWords.TryGetValue(words, tmpData) Then
                        ArrWords.Remove(words)
                        ArrWords.Add(words, tmpData + newData)
                    End If
                End If
            Next

            Return htmlWordBook(ArrWords, Alphabet)
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    ' สร้าง html wordbook
    Private Shared Function htmlWordBook(ByVal ArrWords As Dictionary(Of String, String), ByVal Alphabet As String) As String

        Dim htmlWords As New StringBuilder()
        htmlWords.Append("<div class='headAlphabet'><div class='backAlphabet notDraggable'></div><div class='nextAlphabet notDraggable'><span>")
        htmlWords.Append(Alphabet)
        htmlWords.Append("</span></div><div class='clear'></div></div>")
        htmlWords.Append("<div class='wordsBook notDraggable'>")

        Dim tblLeft As New StringBuilder()
        Dim tblRight As New StringBuilder()
        tblLeft.Append("<table width='50%' style='float:left;'>")
        tblRight.Append("<table width='50%' style='float:right;'>")

        Dim i As Integer = 1
        For Each words In ArrWords
            'Dim mp3 = "<a href=""#"" style='margin-left:5px;margin-rigth:10px;' ""><img style='width:20px;height:20px;' src='../images/Dictionary/realvoice.png' /></a>"
            Dim mp3 = getFileMp3(words.Key().ToString())
            Dim htmlData As String = "<tr><td width='50' align='right' valign='top'><b>" + words.Key() + "</b><br/>" + mp3 + "</td><td valign='top'>" + words.Value.ToString() + "</td></tr>"
            If i Mod 2 = 1 Then
                tblLeft.Append(htmlData)
            ElseIf i Mod 2 = 0 Then
                tblRight.Append(htmlData)
            End If
            i += 1
        Next
        tblLeft.Append("</table>")
        tblRight.Append("</table>")
        'Dim i As Integer = 1
        'For Each w In ArrWords
        '    If i Mod 2 = 1 Then
        '        htmlWords.Append("<tr>")
        '    End If
        '    htmlWords.Append("<td width='50' align='right' valign='top'><b>" + w.Key() + "</b><br/>" + mp3 + "</td>")
        '    htmlWords.Append("<td valign='top'>" + w.Value.ToString() + "</td>")
        '    If i Mod 2 = 0 Then
        '        htmlWords.Append("</tr>")
        '    End If
        '    i += 1
        'Next
        'htmlWords.Append("</table></div>")
        htmlWords.Append(tblLeft.ToString() + tblRight.ToString() + "</div>")
        htmlWordBook = htmlWords.ToString()
    End Function

    ' get filePath mp3
    Private Shared Function getFileMp3(ByVal words As String) As String
        Dim fileName As String = StrToHex(words) + ".mp3"
        Dim mp3PathName As String = HttpContext.Current.Server.MapPath("../../dictionary/mp3")
        Dim fullFileName As String = System.IO.Path.Combine(mp3PathName, fileName)
        Dim htmlMp3 As String = ""
        If (System.IO.File.Exists(fullFileName)) Then
            htmlMp3 = "<a href=""#"" style='margin-left:5px;margin-rigth:10px;' onclick=""playSound('../dictionary/mp3/" & fileName & "');""><img style='width:70px;height:70px;' src='../images/dictionary/realvoice.png' /></a>"
        End If
        getFileMp3 = htmlMp3
    End Function

    ' remove tag <b>
    Private Shared Function StripTags(ByVal html As String) As String
        Return Regex.Replace(html, "</?b>", "")
    End Function

    ' หาคำศัพท์ทั้งหมดตามตัวอักษร ที่เลือกมา
    Private Shared Function getWords(ByVal QuestionId As String, ByVal Alphabet As String) As DataTable
        'เพิ่ม Index ,ฟิลด์ esynAsRelated แล้วเอา Case กับ Order By ออก : Tune Performance 22/11/2556 ต้น
        'Dim sql As String = " SELECT  e.esearch As words, ReturnData, ecat , CASE esyn WHEN '' THEN '1' ELSE '2' END AS related FROM tblWordBook wb INNER JOIN "
        'sql &= " Dict.dbo.eng2thai e ON wb.WordBook_Word = e.esearch "
        'sql &= " WHERE wb.Question_Id = '" & QuestionId & "' AND wb.WordBook_Word Like '" & Alphabet & "%' ORDER BY words,ecat,related "

        Dim sql As String = " SELECT e.esearch AS words, ReturnData, ecat , esynAsRelated " &
                            " FROM tblWordBook wb INNER JOIN eng2thai e ON wb.WordBook_Word = e.esearch " &
                            " WHERE wb.Question_Id = '" & QuestionId & "' AND wb.IsActive = '1' AND wb.WordBook_Word LIKE '" & Alphabet & "%' ORDER BY words,ecat,esynAsRelated "
        Dim db As New ClassConnectSql()
        getWords = db.getdata(sql)
    End Function

    ' สร้างหน้าจำนวนของ ตัวอักษร ที่มีอยู่ในข้อนั้น ๆ
    <Services.WebMethod()>
    Public Shared Function createAlphabet(ByVal QuestionId As String) As String
        Dim dtAlphabet As New DataTable
        dtAlphabet = getAlphabet(QuestionId)
        Dim htmlAlphabet As New StringBuilder
        htmlAlphabet.Append("<div class='headAlphabet' style='padding-right: 20px;padding-top: 5px;'><center>สมุดคำศัพท์</center><div class='clear'></div></div><div style='width:770px;margin-left:auto;margin-right:auto;height: 365px;overflow-y: auto;'>")
        For Each row In dtAlphabet.Rows
            htmlAlphabet.Append("<div Class='Alphabet notDraggable'>")
            htmlAlphabet.Append(row("Alphabet").ToString())
            htmlAlphabet.Append("</div>")
        Next
        htmlAlphabet.Append("<div Class='clear'></div></div>")
        Return htmlAlphabet.ToString()
    End Function

    ' get จำนวนตัวอักษรที่อยู่ในข้อนั้นๆ
    Private Shared Function getAlphabet(ByVal QuestionId As String) As DataTable
        Dim sql As String = " SELECT Alphabet FROM (SELECT UPPER(SUBSTRING(WordBook_Word,1,1)) AS Alphabet FROM tblWordBook "
        sql &= " WHERE Question_Id = '" & QuestionId & "' AND tblWordbook.IsActive = '1') tblAlphabet GROUP BY Alphabet ORDER BY Alphabet ;"
        Dim db As New ClassConnectSql()
        getAlphabet = db.getdata(sql)
    End Function

    ' get ตัวอักษรถัดไป
    Private Shared Function getNextAlphabet(ByVal QuestionId As String, ByVal Alphabet As String) As String
        Dim ArrAlPhabet = getAlphabet(QuestionId)
        Dim nextAlphabet As String = ""
        For i As Integer = 0 To ArrAlPhabet.Rows.Count - 1
            If ArrAlPhabet.Rows(i)("Alphabet") = Alphabet Then
                If i = ArrAlPhabet.Rows.Count - 1 Then
                    nextAlphabet = ArrAlPhabet.Rows(0)("Alphabet")
                Else
                    nextAlphabet = ArrAlPhabet.Rows(i + 1)("Alphabet")
                End If
                Exit For
            End If
        Next
        getNextAlphabet = nextAlphabet
    End Function

    Private Shared Function StrToHex(ByVal Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        While Data.Length > 0
            sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
            Data = Data.Substring(1, Data.Length - 1)
            sHex = sHex & sVal & "-"
        End While
        Return sHex.Substring(0, sHex.Length - 1)
    End Function

    ' NOTE
    ' create note 
    <Services.WebMethod()>
    Public Shared Function createNote(ByVal UserId As String)
        Dim HtmlNote As String = getNote(UserId)
        createNote = HtmlNote
    End Function

    'get note
    Private Shared Function getNote(ByVal UserId As String) As String
        Dim sql As String = " SELECT Note_Text FROM tblNote WHERE User_Id = '" & UserId & "' ;"
        Dim has As New DataTable
        Dim db As New ClassConnectSql()
        has = db.getdata(sql)
        Dim myNote As String = ""
        If has.Rows.Count > 0 Then
            myNote = has.Rows(0)("Note_Text").ToString()
        Else
            addNoteUser(UserId)
            myNote = ""
        End If

        Dim htmlNote As New StringBuilder()
        'htmlNote.Append("<div class='noteHeadTab'><input type='radio' id='tab-1' name='tab-group' checked='checked' />")
        'htmlNote.Append("<label for='tab-1'>กระดาษทด</label><div class='content' id='myClipboard'><textarea></textarea></div></div>")
        'htmlNote.Append("<div class='noteHeadTab'><input type='radio' id='tab-2' name='tab-group' /><label for='tab-2'>")
        'htmlNote.Append("สมุดโน๊ต</label><div class='content' id='myNote'><textarea>")
        'htmlNote.Append(myNote.ToString() + "</textarea></div></div>")
        htmlNote.Append("<textarea>")
        htmlNote.Append(myNote.ToString())
        htmlNote.Append("</textarea>")
        Return htmlNote.ToString()
    End Function
    ' add new note
    Private Shared Sub addNoteUser(ByVal UserId As String)
        Dim PlayerType As String = getPlayerType(UserId)
        Dim sql As String = " INSERT INTO tblNote(User_Id,User_Type,School_Code) VALUES('" & UserId & "','" & PlayerType & "','" & HttpContext.Current.Session("SchoolCode") & "');"
        Try
            Dim db As New ClassConnectSql()
            db.Execute(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub
    ' get player type
    Private Shared Function getPlayerType(ByVal UserId As String) As String
        Dim sql As String = " SELECT Owner_Type FROM t360_tblTabletOwner Where Owner_Id = '" & UserId & "'; "
        Dim db As New ClassConnectSql()
        Dim PlayerType As String = db.ExecuteScalar(sql)
        If PlayerType <> "" Then
            Return PlayerType
        Else
            Return "0"
        End If
    End Function
    ' save note
    <Services.WebMethod()>
    Public Shared Function saveMyNote(ByVal myNote As String, ByVal UserId As String)
        Dim sql As String = " UPDATE tblNote SET School_Code = '" & HttpContext.Current.Session("SchoolCode") & "', Note_Text = N'" & myNote & "',Lastupdate = dbo.GetThaiDate(), ClientId = Null WHERE User_Id = '" & UserId & "'; "
        Try
            Dim db As New ClassConnectSql()
            db.Execute(sql)
            Return "Success"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "Fail"
        End Try
    End Function
End Class
