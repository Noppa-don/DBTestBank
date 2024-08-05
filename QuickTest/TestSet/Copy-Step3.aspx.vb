Public Class Copy_Step3
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Dim objPdf As New ClsPDF(New ClassConnectSql)
    Dim NeedJoinQ40 As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If DEBUG Then
        'Session("userid") = "2"
#End If
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If



        NeedJoinQ40 = Session("NeedJoinQ40")

        If IsNothing(Session("objTestSet")) Then
            objTestSet = New ClsTestSet(Session("userid"))
            Session("objTestSet") = objTestSet
        Else
            objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
        End If

        If Not Page.IsPostBack() Then
            If Session("EditID") = "" Then
                If Session("newTestSetId") = "" Then
                    Dim newTestSetId As String = objTestSet.CreateNewEmptyTestSet(Session("userid"))
                    Session("newTestSetId") = newTestSetId
                End If
            Else
                Session("newTestSetId") = Session("EditID")

            End If

            ListingSubject.DataSource = objTestSet.GetSelectedSubject()
            ListingSubject.DataBind()
        End If
    End Sub

    Private Sub ListingSubject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ListingSubject.ItemDataBound
        Dim item As RepeaterItem = e.Item
        Dim ListingClass As Repeater
        If (item.ItemType = ListItemType.Item) OrElse (item.ItemType = ListItemType.AlternatingItem) Then
            ListingClass = DirectCast(item.FindControl("ListingClass"), Repeater)
            Dim drv As DataRowView = DirectCast(item.DataItem, DataRowView)
            ListingClass.DataSource = drv.CreateChildView("SubjectToClass")
            ListingClass.DataBind()
        End If
    End Sub


    Protected Friend Function CreateTestUnitList(ByVal classId As String, ByVal subjectId As String)
        Dim ds As DataSet = objTestSet.GetAllUnit(classId, subjectId, Session("NeedJoinQ40"))
        Dim sb As New System.Text.StringBuilder()

        If Not IsNothing(ds.Tables(0)) Then
            Dim qSetId As String, QuestionSet As String, numberOfQuestions As String
            For i = 0 To ds.Tables(0).Rows.Count - 1
                qSetId = ds.Tables(0).Rows(i)("qSetId").ToString()
                QuestionSet = ds.Tables(0).Rows(i)("QuestionSet").ToString()
                'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)
                numberOfQuestions = ds.Tables(0).Rows(i)("numberOfQuestions").ToString()
                Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId"), qSetId)

                sb.Append("<tr><td><input onchange=""toggleNumQstn('" & qSetId & "', " & classId & ", " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId") & "',  '" & Session("userID") & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
                'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
                Dim PositionCategory As Integer
                Dim QuestionAfTerLine As String
                Dim index As Integer = QuestionSet.IndexOf("</b> - ")
                PositionCategory = InStr(QuestionSet, "</b> - ")
                QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

                If QuestionAfTerLine.Length > 75 Then
                    Dim Strcut As String = CutString(qSetId)
                    sb.Append(Strcut)
                Else
                    sb.Append(QuestionSet)
                End If

                If ExamAmount.Equals("0") Then
                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a></td></tr>")
                Else
                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                End If



                ' sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                'sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
                '<a href=""#"" onClick=""select('" & ModID & "');"">(เลือก <span class=""spnSelectedQuestions"" id=""spnSelected_" & ModID & """>0</span> จาก <span id=""spnTotal_" & ModID & """>" & numberOfQuestions & "</span> ข้อ)</a></tr>")
            Next
        End If
        Return sb.ToString()


        '<tr><td><input type="checkbox" name="test[]" value="" id="thai1.1"><label for="thai1.1">หน่วยที่ 1 : ตอนที่ 1 สระเสียงต่ำ</label> <a href="#" onClick="selece ();">(เลือก 15 จาก 15 ข้อ)</a></tr>
        '                  <tr><td><input type="checkbox" name="test[]" value="" id="thai1.2"><label for="thai1.2">หน่วยที่ 2 : ตอนที่ 2 สระเสียงต่ำ</label> <a href="#" onClick="selece ();"> (เลือก 0 จาก 20 ข้อ)</tr>
    End Function

    <Services.WebMethod()>
    Public Shared Function OnSaveCodeBehide(ByVal needRemove As String, ByVal qSetId As String, ByVal testSetId As String, ByVal userId As String) As String
        Dim retVal As String = ""
        Dim objTestSet As New ClsTestSet(userId)
        Dim IsEdit As String
        If HttpContext.Current.Session("EditId") = "" Then
            IsEdit = "0"
        Else
            IsEdit = "1"
        End If

        If needRemove = "true" Then
            retVal = objTestSet.SaveSelectedQuestion(IsEdit, True, qSetId, testSetId, CBool(ConfigurationManager.AppSettings("NeedJoinQ40")))
        Else
            retVal = objTestSet.SaveSelectedQuestion(IsEdit, False, qSetId, testSetId, CBool(ConfigurationManager.AppSettings("NeedJoinQ40")))
        End If

        OnSaveCodeBehide = retVal
    End Function

    Protected Sub btnNextStep4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextStep4.Click
        Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนที่ 4", True)
        Response.Redirect("step4.aspx", False)
    End Sub

    Public Function GetChecked(ByVal QsetId As String) As String
        Dim Check As String
        'Dim QSId As String = Request.QueryString("qSetId")
        If Session("EditID") = "" Then
            Check = objTestSet.GetSelectedQuestionSet(Session("newTestSetId"), QsetId)
        Else
            Check = objTestSet.GetSelectedQuestionSet(Session("EditID"), QsetId)
        End If

        If Check = "True" Then
            GetChecked = "checked=""checked"""
        Else
            GetChecked = String.Empty
        End If

    End Function
    <Services.WebMethod()>
    Public Shared Function getQuestionSetName(ByVal qSetId As String) As String
        Dim strSql As String = "select qs.QSet_Name ,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id= '" & qSetId & "'"
        Dim db As New ClassConnectSql()
        Dim ds As DataTable
        ds = db.getdata(strSql)
        Dim qSetName As String = ""
        Dim qSetNameBeforeComplete As String
        Dim CategoryName As String = ""

        CategoryName = ds.Rows(0)("QCategory_Name")
        If Not IsNothing(ds) Then
            qSetNameBeforeComplete = ds.Rows(0)("QSet_Name").ToString()
            qSetName = qSetNameBeforeComplete
        End If


        Return qSetName
    End Function


    Private Function CutString(ByVal QSetId As String) As String
        Dim clsData As New ClassConnectSql
        Dim dtQuestionSet As New DataTable

        Dim QCategoryName As String
        Dim sqlQuestionSet As String = "Select qs.QSet_Name,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc Where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id = '" & QSetId & "'"
        dtQuestionSet = New DataTable
        dtQuestionSet = clsData.getdata(sqlQuestionSet)

        Dim QuestionSetName As String = dtQuestionSet.Rows(0)("QSet_Name")

        QCategoryName = dtQuestionSet.Rows(0)("QCategory_Name")
        Dim CompleteStr As String
        Dim CheckBrOld As Boolean = QuestionSetName.Contains("<br>")
        Dim CheckBrNew As Boolean = QuestionSetName.Contains("<br />")
        Dim CutQuestionSetName As String

        If QuestionSetName.ToString.Length > 50 Then

            If CheckBrNew = False And CheckBrOld = False Then
                CutQuestionSetName = QuestionSetName.Substring(0, 50) & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
                CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & CutQuestionSetName
            Else

                Dim InstrOldBr As String
                Dim CutStrNewBr As String
                Dim CutStrOldBr As String

                If CheckBrOld = True Then
                    InstrOldBr = InStr(QuestionSetName, "<br>")
                    CutStrOldBr = QuestionSetName.Substring(0, InstrOldBr - 1)
                Else
                    CutStrOldBr = QuestionSetName
                End If

                If CheckBrNew = True Then
                    Dim InstrNewBr As String = InStr(CutStrOldBr, "<br />")
                    If InstrNewBr <> 0 Then
                        CutStrNewBr = QuestionSetName.Substring(0, InstrNewBr - 1) & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
                    Else
                        CutStrNewBr = CutStrOldBr & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
                    End If
                Else
                    CutStrNewBr = CutStrOldBr & "</b><span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
                End If

                CompleteStr = "<b>" & QCategoryName & "</b>" & " - " & CutStrNewBr

            End If

        End If

        Return CompleteStr
    End Function

End Class