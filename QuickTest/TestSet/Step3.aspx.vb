Imports System.Data.SqlClient
Imports System.Web
Public Class Step3
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Dim objPdf As New ClsPDF(New ClassConnectSql)
    Dim NeedJoinQ40 As Boolean
    Dim _DB As New ClassConnectSql()
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    Public GroupName As String
    'Dim ClsSelectSess As New ClsSelectSession
    Protected IsAndroid As Boolean
    Public IE As String
    Protected EditTestSetWarningText As String = CommonTexts.EditTestSetWarningText

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Log.Record(Log.LogType.PageLoad, "pageload step3", False)
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If
        End If

#If DEBUG Then
        'Session("userid") = "2"
#End If
#If IE = "1" Then
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
        Session("EditID") = "CF4B122E-E553-4258-98EF-A444071FD02E"
        Dim listSelectedSubjects As New List(Of Object)
        'SC_ไทย_6bf52dc7-314c-40ed-b7f3-bcc87f724880
        listSelectedSubjects.Add(New Selectedsubjects With {.KeyId = "6bf52dc7-314c-40ed-b7f3-bcc87f724880",
                                                    .ClassId = "6bf52dc7-314c-40ed-b7f3-bcc87f724880",
                                                    .ClassName = "ม.6",
                                                    .SubjectName = "ไทย",
                                                    .SubjectId = 1
                                                  })
        'SC_สังคมฯ_6bf52dc7-314c-40ed-b7f3-bcc87f724880
        listSelectedSubjects.Add(New Selectedsubjects With {.KeyId = "6bf52dc7-314c-40ed-b7f3-bcc87f724880",
                                                    .ClassId = "6bf52dc7-314c-40ed-b7f3-bcc87f724880",
                                                    .ClassName = "ม.6",
                                                    .SubjectName = "สังคมฯ",
                                                    .SubjectId = 2
                                                  })
        Dim c As New ClsTestSet(Session("UserId"))
        c.SelectedSubjectClass = listSelectedSubjects
        c.SelectedSyllabusYear = "y51"
        Session("objTestSet") = c
#End If

        If (Session("UserId") Is Nothing) Then
            Log.Record(Log.LogType.PageLoad, "step3 session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        Else
            'GroupName = Session("selectedSession").ToString()

            If HttpContext.Current.Application("NeedQuizMode") = True Then
                ChkFontSize = "16px"
                txtStep1 = "ขั้นที่ 1: จัดชุดควิซ -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึกชุดควิซ -->"
            Else
                ChkFontSize = "20px"
                txtStep1 = "ขั้นที่ 1: จัดชุด -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึก และจัดพิมพ์"
            End If

            ' Dim ClsSelectedSession As New ClsSelectSession
            'ClsSelectedSession.checkCurrentPage(Session("UserId").ToString(), Session("selectedSession").ToString())

            NeedJoinQ40 = HttpContext.Current.Application("NeedJoinQ40")

            If IsNothing(Session("objTestSet")) Then
                objTestSet = New ClsTestSet(Session("userid").ToString)
                Session("objTestSet") = objTestSet
            Else
                objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
            End If

            If Not Page.IsPostBack() Then

                If Session("EditID").ToString = "" Then

                    Dim s As New ClsSelectSession()
                    If Not s.TestsetId = "" Then
                        Session("newTestSetId") = s.TestsetId
                    End If

                    If Session("newTestSetId") = "" Then
                        Dim newTestSetId As String = objTestSet.CreateNewEmptyTestSet(Session("userid").ToString)
                        Session("newTestSetId") = newTestSetId
                        s.TestsetId = newTestSetId
                        ' ClsSelectSess.setApplicationWhenChangeCurrentPage(Session("newTestSetId").ToString(), objTestSet)
                        ' ClsSelectSess.setApplicationWhenChangeCurrentPage(Session("EditID").ToString(), objTestSet)
                    End If
                Else

                    Session("newTestSetId") = Session("EditID").ToString
                    objTestSet.DeleteQuestionforDeletedSubject(Session("newTestSetId").ToString)

                    ' ClsSelectSess.setApplicationWhenChangeCurrentPage(Session("newTestSetId").ToString(), objTestSet)
                    ' ClsSelectSess.setEditId("")
                    ' เก็บ session จากชุดเก่าเพื่อไปดูว่ามีการเปลี่ยนแปลงหรือไม่
                    'Dim clsCheckmark As New ClsCheckMark
                    'clsCheckmark.setSessionForCheckEditTestset(Session("EditID").ToString())
                End If

                ListingSubject.DataSource = objTestSet.GetSelectedSubject()
                ListingSubject.DataBind()
                ' clear ข้อสอบที่ถูกเอาวิชาออกไป   

                Repeater1.DataSource = objTestSet.GetSelectedSubject()
                Repeater1.DataBind()
            Else
                Repeater1.DataSource = objTestSet.GetSelectedSubject()
                Repeater1.DataBind()
            End If



        End If
    End Sub

    Private Sub ListingSubject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ListingSubject.ItemDataBound, Repeater1.ItemDataBound
        Dim item As RepeaterItem = e.Item
        Dim ListingClass As Repeater
        If (item.ItemType = ListItemType.Item) OrElse (item.ItemType = ListItemType.AlternatingItem) Then
            ListingClass = DirectCast(item.FindControl("ListingClass"), Repeater)
            Dim drv As DataRowView = DirectCast(item.DataItem, DataRowView)
            ListingClass.DataSource = drv.CreateChildView("SubjectToClass")
            ListingClass.DataBind()
        End If
    End Sub

    Protected Friend Function CreateQuestionAmount(classId As String, subjectId As String) As String
        Dim syllabusYear As String = "51"

        Dim sql As String = "select count(*) as questionAmount from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                                inner Join tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id 
                                inner Join tblBook b on qc.Book_Id = b.BookGroup_Id 
                                where b.GroupSubject_Id = '" & subjectId & "' and b.Level_Id = '" & classId & "'  
                                and q.IsActive = 1 and qs.IsActive = 1 and b.Book_Syllabus = '" & syllabusYear & "' 
                                and (qc.IsWpp <> 0 or qc.IsWpp is null);"

        Dim db As New ClassConnectSql()
        Dim questionAmount As Integer = db.ExecuteScalar(sql)

        Dim sql2 As String = "select count(*) from tblTestSetQuestionSet tsqs inner join tblTestSetQuestionDetail tsqd on tsqs.TSQS_Id = tsqd.TSQS_Id "
        sql2 &= " inner join tblQuestionset qs On tsqs.QSet_Id = qs.QSet_Id inner join tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id "
        sql2 &= " inner join tblBook b on qc.Book_Id = b.BookGroup_Id "
        sql2 &= " where tsqs.TestSet_Id = '" & Session("newTestSetId").ToString() & "' and tsqs.IsActive = 1 and tsqd.IsActive = 1 and  "
        sql2 &= " b.GroupSubject_Id = '" & subjectId & "' and b.Level_Id = '" & classId & "';"
        Dim questionSelectedAmount As Integer = db.ExecuteScalar(sql2)

        Return String.Format("จากทั้งหมด {0:n0} ข้อ เลือกมาแล้ว {1:n0} ข้อ", questionAmount, questionSelectedAmount)
    End Function


    Protected Friend Function CreateQuestionByUser(classId As String, subjectId As String, className As String) As String
        Dim content As String = ""
        content &= "<div style='padding:5px;' ></div><span>ชั้น " & className & " (ข้อสอบที่โรงเรียนเพิ่มเอง)</span><table><tr>"
        content &= "<td style='text-align:center;'><input type='text' class='qtipQcategory' subjectid='' classid=''/></td>"
        content &= "<td style='text-align:center;'><input type='text' class='qtipEvalution' subjectid='' classid=''/></td>"
        content &= "</tr><tr><td colspan = '2' style='padding-left:25px;'><span class='" & subjectId & classId & "'></span></td></tr></table>"

        Return content
    End Function

    Private Function GetQsetInBookByUser(classId As String, subjectId As String) As DataTable
        Dim syllabusYear As String = "51"
        Dim userId As String = Session("userid").ToString()
        Dim sql As String = "select b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_Name,qs.QSet_No,Count(qs.qset_Id) as QuestionAmount from tblQuestionset qs inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id"
        sql &= " inner join tblBook b On qc.Book_Id = b.BookGroup_Id inner join tblQuestion q on qs.QSet_Id = q.QSet_Id where b.Level_Id = '" & classId & "' and GroupSubject_Id = '" & subjectId & "' and qc.IsActive = 1  "
        sql &= " and qs.IsActive = 1 and q.IsActive = 1 and b.Book_Syllabus = '" & syllabusYear & "' and qc.IsWpp = 0 and qc.Parent_Id = '" & userId & "' "
        sql &= " group by b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_Name,qs.QSet_No "
        sql &= " order by b.BookGroup_Id, qc.QCategory_No,qs.QSet_No,qc.QCategory_Name;"
        Dim db As New ClassConnectSql()
        Return db.getdata(sql)
    End Function

    Protected Friend Function CreateTestUnitList(ByVal classId As String, ByVal subjectId As String)

        Dim log As New Log
        Log.Record(Log.LogType.ExamStep, classId, False)
        Log.Record(Log.LogType.ExamStep, subjectId, False)
        Try
            Dim IsSelectByEvalutionIndex As Boolean = HttpContext.Current.Application("IsSelectByEvalutionIndex")

            Dim ds As DataSet = objTestSet.GetAllUnit(classId, subjectId, HttpContext.Current.Application("NeedJoinQ40"), IsSelectByEvalutionIndex, Session("newTestSetId").ToString)
            Dim sb As New System.Text.StringBuilder()
            Dim _DB As New ClassConnectSql()

            If Not IsNothing(ds.Tables(0)) Then
                Dim qSetId As String, QuestionSet As String, numberOfQuestions As String
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    qSetId = ds.Tables(0).Rows(i)("qSetId").ToString()
                    QuestionSet = ds.Tables(0).Rows(i)("QuestionSet").ToString()
                    Dim QsetName As String = ds.Tables(0).Rows(i)("QSet_Name").ToString()
                    Dim QCatName As String = ds.Tables(0).Rows(i)("QCategory_Name").ToString()
                    Dim GroupSubjectId As String = ds.Tables(0).Rows(i)("GroupSubject_Id").ToString()
                    numberOfQuestions = ds.Tables(0).Rows(i)("numberOfQuestions").ToString()
                    Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId").ToString, qSetId)
                    Dim Book_Syllabus As String = ds.Tables(0).Rows(i)("Book_Syllabus").ToString()

                    'sb.Append("<tr><td><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
                    If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
                        'sb.Append("<tr><td><label>[ " & Book_Syllabus & " ]</label><img style='cursor:pointer;' src='../Images/freehand.png' onclick=""EditQCatName('" & qSetId & "','" & QCatName & "')"" /><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
                        sb.Append("<tr><td><img style='cursor:pointer;' src='../Images/freehand.png' onclick=""EditQCatName('" & qSetId & "','" & QCatName & "')"" /><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "' QCatId=5f4765db-0917-470b-8e43-6d1c7b030818 QSetId=" & qSetId & ">")
                    Else
                        'sb.Append("<tr><td><label>[ " & Book_Syllabus & " ]</label><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
                        sb.Append("<tr><td><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
                    End If


                    Dim PositionCategory As Integer
                    Dim QuestionAfTerLine As String
                    Dim index As Integer = QuestionSet.IndexOf("</b> - ")
                    PositionCategory = InStr(QuestionSet, "</b> - ")
                    QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

                    If QuestionAfTerLine.Length > 75 Then
                        Dim Strcut As String = objTestSet.CutStringAndReturn50Alphabet(qSetId)
                        sb.Append(Strcut)
                    Else
                        sb.Append(QuestionSet)
                    End If

                    QsetName = QsetName.Replace("""", "&quot;")

                    Dim ManageBtn As String = ""
                    Dim MoveExamBtn As String = ""

                    If (HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = True) Or (HttpContext.Current.Application("NeedDeleteQcatAndQset") = True) Then
                        ManageBtn = "<img title='จัดการชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/ManageQCateQSet.png' onclick=""EditQsetName('" & qSetId & "','" & QsetName & "')"" />"
                    Else
                        ManageBtn = ""
                    End If

                    'MoveExamBtn = "<img title='จัดการชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/moveExam.png' />"

                    Dim WIconStr As New StringBuilder
                    If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
                        WIconStr.Append("<div class='MainDivSummary MainW' qsetid='" & qSetId & "'><div class='divLeft'>")
                        'แก้ไข้/ดู หมดแล้ว
                        If WordEditConfirmed(qSetId) = True Then
                            WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
                        End If
                        'อนุมัติหมดแล้ว
                        If WordTechnicalConfirmed(qSetId) = True Then
                            WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
                        End If
                        'พิสูจน์อักษรอนุมัติหมดแล้ว
                        If WordPrePressConfirmed(qSetId) = True Then
                            WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
                        End If
                        WIconStr.Append("</div><div class='divRight'><img src='../Images/WIcon2.png' /></div></div>")
                    End If

                    Dim QIconStr As New StringBuilder
                    If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
                        QIconStr.Append("<div class='MainDivSummary MainQ' qsetid='" & qSetId & "'><div class='divLeft'>")
                        'แก้ไข/ดูหมดแล้ว
                        If QuizEditConfirmed(qSetId) = True Then
                            QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
                        End If
                        'อนุมัติหมดแล้ว
                        If QuizTechnicalConfirmed(qSetId) = True Then
                            QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
                        End If
                        'พิสูจน์อักษรอนุมัติหมดแล้ว
                        If QuizPrePreesConfirmed(qSetId) = True Then
                            QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
                        End If
                        QIconStr.Append("</div><div class='divRight'><img src='../Images/QIcon2.png' /></div></div>")
                    End If

                    Dim ManageEngExam As New StringBuilder
                    If subjectId = "fb677859-87da-4d8d-a61e-8a76566d69d8" Then
                        ManageEngExam.Append("<div class='MainDivSummary MainEM' qsetid='" & qSetId & "'><img src='../Images/ManageQuestionSet.png' /></div>")
                    End If

                    Dim IsMove As Boolean = CBool(ConfigurationManager.AppSettings("IsMoveExam"))
                    If ExamAmount.Equals("0") Then

                        If IsMove Then
                            sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> 
                                ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a>" & ManageBtn & MoveExamBtn & WIconStr.ToString() & QIconStr.ToString() & ManageEngExam.ToString())
                        Else
                            sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> 
                                ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a>" & ManageBtn & WIconStr.ToString() & QIconStr.ToString() & ManageEngExam.ToString())
                        End If

                    Else
                        If IsMove Then
                            sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">
                                ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" &
                                   numberOfQuestions & "</span> ข้อ</a>" & ManageBtn & MoveExamBtn & WIconStr.ToString() & QIconStr.ToString() & ManageEngExam.ToString())
                        Else
                            sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">
                                ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" &
                                   numberOfQuestions & "</span> ข้อ</a>" & ManageBtn & WIconStr.ToString() & QIconStr.ToString() & ManageEngExam.ToString())
                        End If

                    End If
                    sb.Append("</td></tr>")
                Next
            End If
            Return sb.ToString()
        Catch ex As Exception
            Log.Record(Log.LogType.ExamStep, ex.ToString, False)
        End Try

    End Function

    Private Function GetQsetTypeName(ByVal QsetType As Integer) As String
        Select Case QsetType
            Case 1
                Return "ช๊อยส์"
            Case 2
                Return "ถูกผิด"
            Case 3
                Return "จับคู่"
            Case 6
                Return "ถูกผิด"
        End Select
        Return "ไม่ระบุ"
    End Function

    <Services.WebMethod()>
    Public Shared Function OnSaveCodeBehide(ByVal needRemove As String, ByVal qSetId As String, ByVal testSetId As String, ByVal userId As String, ByVal classId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim retVal As String = ""
        Dim objTestSet As New ClsTestSet(userId)
        Dim IsEdit As String
        If HttpContext.Current.Session("EditId").ToString = "" Then
            IsEdit = "0"
        Else
            IsEdit = "1"
        End If

        If needRemove = "true" Then
            retVal = objTestSet.SaveSelectedQuestion(IsEdit, True, qSetId, testSetId, CBool(HttpContext.Current.Application("NeedJoinQ40")), classId)
        Else
            retVal = objTestSet.SaveSelectedQuestion(IsEdit, False, qSetId, testSetId, CBool(HttpContext.Current.Application("NeedJoinQ40")), classId)
        End If

        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Dim db As New ClassConnectSql()
            Dim sql As String = " SELECT QSet_Type FROM tblQuestionSet WHERE QSet_Id = '" & qSetId & "';"
            Dim type As String = db.ExecuteScalar(sql)
            If (type = "1" Or type = "2") Then
                'ถ้าข้อสอบเป็น choice and true/false
                OnSaveCodeBehide = "0"
            Else
                'ถ้าข้อสอบเป็นแบบอื่น ที่ checkmark ไม่สามารถใช้ได้
                OnSaveCodeBehide = "1"
            End If
        Else
            OnSaveCodeBehide = retVal
        End If

    End Function

    Protected Sub btnNextStep4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextStep4.Click
        'If Session("EditID") <> "" Then
        '    ' ไปเช็คอีกรอบว่า ชุดที่เลือกมาแก้ไขและหลังจาก nextstep ไปแล้ว มีการเปลี่ยนแปลงหรือไม่
        '    Dim clsCheckmark As New ClsCheckMark
        '    clsCheckmark.setSessionForCheckEditTestsetIsChange(Session("newTestSetId"), Session("testSetIsEdit"))
        'End If  

        Dim CheckQuestionAmount As String = GetQuestionAmount()

        If CheckQuestionAmount <> "0" Then
            If Not ClsKNSession.IsAddQuestionBySchool Then
                Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนที่ 4", True)
                Response.Redirect("~/testset/step4.aspx", False)
            Else
                Log.Record(Log.LogType.ExamStep, "ไปหน้าสรุปข้อสอบที่เลือกมา", True)
                Response.Redirect("~/testset/TestsetSummaryPage.aspx", False)
            End If
        Else
            Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนที่ 4 ไม่ได้เพราะไม่ได้เลือกข้อสอบ", True)
        End If

    End Sub

    Public Function GetChecked(ByVal QsetId As String) As String
        Dim Check As String
        'Dim QSId As String = Request.QueryString("qSetId")
        If Session("EditID") = "" Then
            Check = objTestSet.GetSelectedQuestionSet(Session("newTestSetId").ToString, QsetId)
        Else
            Check = objTestSet.GetSelectedQuestionSet(Session("EditID").ToString, QsetId)
        End If

        If Check = "True" Then
            GetChecked = "checked=""checked"""
        Else
            GetChecked = String.Empty
        End If

    End Function
    <Services.WebMethod()>
    Public Shared Function getQuestionSetName(ByVal qSetId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
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

    Public Function GetQuestionAmount() As String

        Dim SelectedQuestionAmount
        If Session("EditID") = "" Then
            SelectedQuestionAmount = objTestSet.GetSelectedAmount(Session("newTestSetId").ToString)
        Else
            SelectedQuestionAmount = objTestSet.GetSelectedAmount(Session("EditID").ToString)
        End If

        Return SelectedQuestionAmount

    End Function


    <Services.WebMethod()>
    Public Shared Function DeleteQuestionCategoryCB(ByVal VbQsetId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        If VbQsetId Is Nothing Or VbQsetId = "" Then
            Return ""
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""
        sql = " SELECT QCategory_Id FROM dbo.tblQuestionSet WHERE QSet_Id = '" & VbQsetId & "' "
        Dim QcatId As String = _DB.ExecuteScalar(sql)
        If QcatId = "" Then
            Return ""
        End If

        _DB.OpenWithTransection()
        Try
            'ลบ QuestionCategory
            sql = " UPDATE dbo.tblQuestionCategory SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCategory_Id = '" & QcatId & "' "
            _DB.ExecuteWithTransection(sql)

            'ลบ QuestionSet
            sql = " UPDATE  dbo.tblQuestionSet  SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCategory_Id = '" & QcatId & "' "
            _DB.ExecuteWithTransection(sql)

            'ลบคำถาม
            sql = " UPDATE  dbo.tblQuestion SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QSet_Id IN (SELECT QSet_Id FROM dbo.tblQuestionSet WHERE QCategory_Id = '" & QcatId & "') "
            _DB.ExecuteWithTransection(sql)

            'ลบคำตอบ
            sql = " UPDATE  dbo.tblAnswer SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id IN (SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id IN " &
                  " (SELECT QSet_Id FROM dbo.tblQuestionSet WHERE QCategory_Id = '" & QcatId & "' ) ) "
            _DB.ExecuteWithTransection(sql)

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return ""
        End Try

        _DB.CommitTransection()

        Dim Newlog As String = "ลบหน่วยการเรียนรู้ (QCatId=" & QcatId & ")"
        '" ลบ QuestionCategory ที่ QuestionCategoryId = " & QcatId & " "
        Log.Record(Log.LogType.ManageExam, Newlog, True)
        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteQuestionSet(ByVal VbQsetId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        If VbQsetId Is Nothing Or VbQsetId = "" Then
            Return ""
        End If

        Dim sql As String = ""
        Dim _DB As New ClassConnectSql()

        _DB.OpenWithTransection()
        Try

            'ลบคำตอบก่อน
            sql = " UPDATE  dbo.tblAnswer SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id IN (SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & VbQsetId & "') "
            _DB.ExecuteWithTransection(sql)

            'ลบคำถาม
            sql = " UPDATE  dbo.tblQuestion SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QSet_Id = '" & VbQsetId & "' "
            _DB.ExecuteWithTransection(sql)

            'ลบ Qsetid
            sql = " UPDATE  dbo.tblQuestionSet SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QSet_Id = '" & VbQsetId & "' "
            _DB.ExecuteWithTransection(sql)

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return ""
        End Try

        _DB.CommitTransection()
        Dim Newlog As String = "ลบชุดข้อสอบ (QSetId=" & VbQsetId & ")"
        ' "ลบ Qset Qsetid = " & VbQsetId & " "
        Log.Record(Log.LogType.ManageExam, Newlog, True)
        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function SaveNewQsetNameCodeBehind(ByVal QsetId As String, ByVal QsetName As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _Db As New ClassConnectSql()
        QsetName = QsetName.Replace("<br>", "<br />")
        Dim CheckEmptystring As String = Regex.Replace(QsetName, "<.*?>", String.Empty)
        CheckEmptystring = CheckEmptystring.Replace(" ", "").Replace(vbLf, "").Replace(vbCrLf, "").Replace(vbNewLine, "").Replace(vbCr, "").Replace("&nbsp;", "")
        If CheckEmptystring = "" Then
            Return "False"
        End If
        Dim sql As String = " UPDATE dbo.tblQuestionSet SET QSet_Name = '" & _Db.CleanString(QsetName) & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QSet_Id = '" & QsetId & "'; "
        Try
            _Db.Execute(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
        Dim LogStr As String = "แก้ไขชื่อชุดข้อสอบเป็น " & _Db.CleanString(QsetName) & "(QSetId=" & QsetId & ")"
        '"แก้ QsetName เป็น '" & _Db.CleanString(QsetName) & "' ที่ QsetId = '" & QsetId & "' "
        Log.Record(Log.LogType.ManageExam, _Db.CleanString(LogStr), True)
        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function EditQCatName(ByVal QsetId As String, ByVal QCatName As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT QCategory_Id FROM dbo.tblQuestionset WHERE QSet_Id = '" & QsetId & "' "
        Dim QCatId As String = _DB.ExecuteScalar(sql)
        If QCatId <> "" Then
            If QCatName = "CancelEditQCat" Then
                Log.Record(Log.LogType.ManageExam, "ยกเลิก(ปิดหน้าจอ)แก้ไขชื่อหน่วยการเรียนรู้ " & "(QCatId = " & QCatId & ")", True)
                Return "Complete"
            ElseIf QCatName = "CancelEditQSet" Then
                Log.Record(Log.LogType.ManageExam, "ยกเลิก(ปิดหน้าจอ)แก้ไขชื่อชุดข้อสอบ " & "(QSetId = " & QsetId & ")", True)
                Return "Complete"
            Else
                sql = " UPDATE dbo.tblQuestionCategory SET QCategory_Name = '" & _DB.CleanString(QCatName) & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCategory_Id = '" & QCatId & "' "
                Try
                    _DB.Execute(sql)
                    Log.Record(Log.LogType.ManageExam, "แก้ไขชื่อหน่วยการเรียนรู้เป็น " & _DB.CleanString(QCatName) & " (QCatId = " & QCatId & ")", True)
                    Return "Complete"
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    Return "Error"
                End Try

            End If
        Else
            Return "Error"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Sub SetLog(ByVal LogText As String, LogType As Integer)
        Log.Record(LogType, LogText, 1)
    End Sub

    <Services.WebMethod()>
    Public Shared Sub SetLogAddQcatOrQset(ByVal SubjectId As String, LevelId As String)
        Dim sql As String = " select * from (select GroupSubject_ShortName from tblGroupSubject where GroupSubject_Id = '" & SubjectId & "')s," & _
                            "(select  Level_ShortName from tblLevel where Level_Id = '" & LevelId & "')l"

        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)
        If dt.Rows.Count <> 0 Then
            Dim strLog As String = "ไปหน้าเพิ่มหน่วยการเรียนรู้/ชุดข้อสอบ วิชา " & dt.Rows(0)("GroupSubject_ShortName") & " ชั้น " & dt.Rows(0)("Level_ShortName")
            Log.Record(Log.LogType.ManageExam, strLog, True)
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Sub SetLogEditQCatName(QsetId As String)
        Dim sql As String = "Select QCategory_Id From tblQuestionSet Where QSet_Id = '" & QsetId & "';"

        Dim db As New ClassConnectSql()
        Dim QcatId As String = db.ExecuteScalar(sql)
        Dim strLog As String = "ไปหน้าแก้ไขหน่วยการเรียนรู้ (QCatId=" & QcatId & ")"
        Log.Record(Log.LogType.ManageExam, strLog, True)


    End Sub

#Region "LayoutConfirmed"
    Private Enum ConfirmedType
        WordEditConfirmed
        WordTechnicalConfirmed
        WordPrePressConfirmed
        QuizEditConfirmed
        QuizTechnicalConfirmed
        QuizPrePressConfirmed
    End Enum

    Private Function WordEditConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordEditConfirmed)
    End Function

    Private Function WordTechnicalConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordTechnicalConfirmed)
    End Function

    Private Function WordPrePressConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordPrePressConfirmed)
    End Function

    Private Function QuizEditConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizEditConfirmed)
    End Function

    Private Function QuizTechnicalConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizTechnicalConfirmed)
    End Function

    Private Function QuizPrePreesConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizPrePressConfirmed)
    End Function

    Private Function CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(ByVal qSetId As String, ByVal confirmedType As ConfirmedType) As Boolean
        Dim conn As New SqlConnection
        _DB.OpenExclusiveConnect(conn)
        Try
            Dim WhereField As String = ""
            If confirmedType = Step3.ConfirmedType.WordEditConfirmed Then WhereField = "WordEditConfirmed"
            If confirmedType = Step3.ConfirmedType.WordTechnicalConfirmed Then WhereField = "WordTechnicalConfirmed"
            If confirmedType = Step3.ConfirmedType.WordPrePressConfirmed Then WhereField = "WordPrePressConfirmed"
            If confirmedType = Step3.ConfirmedType.QuizEditConfirmed Then WhereField = "QuizEditConfirmed"
            If confirmedType = Step3.ConfirmedType.QuizTechnicalConfirmed Then WhereField = "QuizTechnicalConfirmed"
            If confirmedType = Step3.ConfirmedType.QuizPrePressConfirmed Then WhereField = "QuizPrePressConfirmed"
            Dim NumberOfQuestionInQset As Integer = GetNumberOfQuestionInQset(qSetId, conn)
            Dim NumberByCondition As Integer = GetNumberByCondition(qSetId, WhereField, conn)
            If NumberOfQuestionInQset = NumberByCondition Then
                _DB.CloseExclusiveConnect(conn)
                Return True
            Else
                _DB.CloseExclusiveConnect(conn)
                Return False
            End If
        Catch ex As Exception
            _DB.CloseExclusiveConnect(conn)
            Return False
        End Try
    End Function

    Private Function GetNumberOfQuestionInQset(ByVal qSetId As String, ByRef InputConn As SqlConnection) As Integer
        Dim sql As String = "SELECT COUNT(*) FROM dbo.tblLayoutConfirmed lcf inner join tblQuestion q on lcf.Question_Id = q.Question_Id WHERE q.Qset_Id = '" & qSetId & "' AND q.IsActive = 1 and lcf.IsActive = 1; "
        Dim numberOfQuestion As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
        Return numberOfQuestion
    End Function

    Private Function GetNumberByCondition(ByVal qSetId As String, ByVal FieldToWhere As String, ByRef inputConn As SqlConnection) As Integer
        Dim sql As String = ""
        sql = " SELECT COUNT(*) FROM dbo.tblLayoutConfirmed WHERE Qset_Id = '" & qSetId & "' AND IsActive = 1 AND " & FieldToWhere & " = 1; "
        Dim NumberByCondition As Integer = CInt(_DB.ExecuteScalar(sql, inputConn))
        Return NumberByCondition
    End Function

#End Region

    '    Protected Friend Function CreateTestUnitList(ByVal classId As String, ByVal subjectId As String)
    '        Dim IsSelectByEvalutionIndex As Boolean = HttpContext.Current.Application("IsSelectByEvalutionIndex")

    '        Dim ds As DataSet = objTestSet.GetAllUnit(classId, subjectId, HttpContext.Current.Application("NeedJoinQ40"), IsSelectByEvalutionIndex, Session("newTestSetId").ToString)
    '        Dim sb As New System.Text.StringBuilder()
    '        Dim _DB As New ClassConnectSql()

    '        If Not IsNothing(ds.Tables(0)) Then
    '            Dim qSetId As String, QuestionSet As String, numberOfQuestions As String
    '            For i = 0 To ds.Tables(0).Rows.Count - 1
    '                qSetId = ds.Tables(0).Rows(i)("qSetId").ToString()
    '                QuestionSet = ds.Tables(0).Rows(i)("QuestionSet").ToString()
    '                Dim QsetName As String = ds.Tables(0).Rows(i)("QSet_Name").ToString()
    '                Dim QCatName As String = ds.Tables(0).Rows(i)("QCategory_Name").ToString()
    '                Dim GroupSubjectId As String = ds.Tables(0).Rows(i)("GroupSubject_Id").ToString()
    '                'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)
    '                numberOfQuestions = ds.Tables(0).Rows(i)("numberOfQuestions").ToString()
    '                Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId").ToString, qSetId)
    '                Dim Book_Syllabus As String = ds.Tables(0).Rows(i)("Book_Syllabus").ToString()

    '                If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
    '                    'sb.Append("<tr><td><label>[ " & Book_Syllabus & " ]</label><img style='cursor:pointer;' src='../Images/freehand.png' onclick=""EditQCatName('" & qSetId & "','" & QCatName & "')"" /><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
    '                    sb.Append("<tr><td><img style='cursor:pointer;' src='../Images/freehand.png' onclick=""EditQCatName('" & qSetId & "','" & QCatName & "')"" /><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "' QCatId=5f4765db-0917-470b-8e43-6d1c7b030818 QSetId=" & qSetId & ">")
    '                Else
    '                    'sb.Append("<tr><td><label>[ " & Book_Syllabus & " ]</label><br/><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
    '                    sb.Append("<tr><td><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")
    '                End If
    '                'sb.Append("<tr><td><label>[ " & Book_Syllabus & " ]</label><br/><input onchange=""toggleNumQstn('" & qSetId & "', " & classId & ", " & subjectId & ");""" & GetChecked(qSetId) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")

    '                'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
    '                Dim PositionCategory As Integer
    '                Dim QuestionAfTerLine As String
    '                Dim index As Integer = QuestionSet.IndexOf("</b> - ")
    '                PositionCategory = InStr(QuestionSet, "</b> - ")
    '                QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

    '                If QuestionAfTerLine.Length > 75 Then
    '                    Dim Strcut As String = objTestSet.CutStringAndReturn50Alphabet(qSetId)
    '                    sb.Append(Strcut)
    '                Else
    '                    sb.Append(QuestionSet)
    '                End If

    '                'Dim IsHaveHomeWork As String = ""
    '                'If HttpContext.Current.Application("NeedHomeWork") = True Then
    '                '    IsHaveHomeWork = "<img style='width:80px;height:45px;margin-left:35px;cursor:pointer;' src='../Images/HomeWork/homework_0.jpg' onclick=""GoToHomeWork('" & qSetId & "','" & _DB.CleanString(QsetName) & "')"" />"
    '                'Else
    '                '    IsHaveHomeWork = ""
    '                'End If

    '                QsetName = QsetName.Replace("""", "&quot;")

    '                'Dim DecodeQsetName As String = Server.UrlDecode(QsetName)
    '                'Dim EditBtn As String = "<img title='แก้ไขชื่อชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/freehand.png' onclick=""EditQsetName('" & qSetId & "',escape('" & QsetName & "'))"" />"
    '                Dim EditBtn As String = ""
    '                Dim DeleteBtn As String = ""

    '                If HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = True Then
    '                    EditBtn = "<img title='แก้ไขชื่อชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/freehand.png' onclick=""EditQsetName('" & qSetId & "','" & QsetName & "')"" />"
    '                Else
    '                    EditBtn = ""
    '                End If

    '                If HttpContext.Current.Application("NeedDeleteQcatAndQset") = True Then
    '                    DeleteBtn = "<img style='margin-left:45px;cursor:pointer;' onclick=""DeleteQcatOrQset('" & qSetId & "','" & QsetName & "','" & QCatName & "')"" src='../Images/Delete-icon.png' />"
    '                Else
    '                    DeleteBtn = ""
    '                End If

    '                Dim WIconStr As New StringBuilder
    '                If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
    '                    WIconStr.Append("<div class='MainDivSummary MainW' qsetid='" & qSetId & "'><div class='divLeft'>")
    '                    'แก้ไข้/ดู หมดแล้ว
    '                    If WordEditConfirmed(qSetId) = True Then
    '                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
    '                    End If
    '                    'อนุมัติหมดแล้ว
    '                    If WordTechnicalConfirmed(qSetId) = True Then
    '                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
    '                    End If
    '                    'พิสูจน์อักษรอนุมัติหมดแล้ว
    '                    If WordPrePressConfirmed(qSetId) = True Then
    '                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
    '                    End If
    '                    WIconStr.Append("</div><div class='divRight'><img src='../Images/WIcon.png' /></div></div>")
    '                End If

    '                Dim QIconStr As New StringBuilder
    '                If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
    '                    QIconStr.Append("<div class='MainDivSummary MainQ' qsetid='" & qSetId & "'><div class='divLeft'>")
    '                    'แก้ไข/ดูหมดแล้ว
    '                    If QuizEditConfirmed(qSetId) = True Then
    '                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
    '                    End If
    '                    'อนุมัติหมดแล้ว
    '                    If QuizTechnicalConfirmed(qSetId) = True Then
    '                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
    '                    End If
    '                    'พิสูจน์อักษรอนุมัติหมดแล้ว
    '                    If QuizPrePreesConfirmed(qSetId) = True Then
    '                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
    '                    End If
    '                    QIconStr.Append("</div><div class='divRight'><img src='../Images/QIcon.png' /></div></div>")
    '                End If

    '                Dim IsMove As Boolean = CBool(ConfigurationManager.AppSettings("IsMoveExam"))
    '                Dim MIconStr As New StringBuilder
    '                If IsMove Then
    '                    MIconStr.Append("<div class='MainDivSummary MainM' qsetid='" & qSetId & "'><div class='divLeft'></div><div class='divRight'><img style='width: 45px;' src='../Images/move.png' /></div></div>")
    '                End If

    '                If ExamAmount.Equals("0") Then
    '                    'sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&classId=" & classId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a>" & EditBtn & DeleteBtn & "</td></tr>")
    '                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a>" & EditBtn & DeleteBtn & WIconStr.ToString() & QIconStr.ToString() & MIconStr.ToString())
    '                Else
    '                    'sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&classId=" & classId & "&iframe=true&width=95%&height=95%&z-index=9"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a>" & EditBtn & DeleteBtn & "</td></tr>")
    '                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a>" & EditBtn & DeleteBtn & WIconStr.ToString() & QIconStr.ToString() & MIconStr.ToString())
    '                End If

    '#If ShowQsetTypeName = "1" Then
    '                sb.Append("<span style=""margin-left: 30px;color: red;"">" & GetQsetTypeName(ds.Tables(0).Rows(i)("QSet_Type")) & "</span>")
    '#End If
    '                sb.Append("</td></tr>")

    '                ' sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
    '                'sb.Append("</label><br /><a style=""color: #2370FA;"" href=""SelectEachQuestion.aspx?qSetId=" & qSetId & "&iframe=true&width=95%&height=95%"" rel=""prettyPhoto"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ</a></td></tr>")
    '                '<a href=""#"" onClick=""select('" & ModID & "');"">(เลือก <span class=""spnSelectedQuestions"" id=""spnSelected_" & ModID & """>0</span> จาก <span id=""spnTotal_" & ModID & """>" & numberOfQuestions & "</span> ข้อ)</a></tr>")
    '            Next
    '        End If
    '        Return sb.ToString()


    '        '<tr><td><input type="checkbox" name="test[]" value="" id="thai1.1"><label for="thai1.1">หน่วยที่ 1 : ตอนที่ 1 สระเสียงต่ำ</label> <a href="#" onClick="selece ();">(เลือก 15 จาก 15 ข้อ)</a></tr>
    '        '                  <tr><td><input type="checkbox" name="test[]" value="" id="thai1.2"><label for="thai1.2">หน่วยที่ 2 : ตอนที่ 2 สระเสียงต่ำ</label> <a href="#" onClick="selece ();"> (เลือก 0 จาก 20 ข้อ)</tr>
    '    End Function


End Class
