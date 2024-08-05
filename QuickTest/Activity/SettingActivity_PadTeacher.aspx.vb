Imports System.Web
Public Class SettingActivity_PadTeacher
    Inherits System.Web.UI.Page
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Public GroupName As String
    Dim ClsSelectedSession As New ClsSelectSession
    ' 02-05-56 update variable tools
    Public toolsAllSubject As Boolean = True
    Public toolsSubject_Eng As Boolean = False
    Public toolsSubject_Math As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx")
        End If

        If (Request.QueryString("editid") <> "") Then
            Session("newTestSetId") = Request.QueryString("editid")
        End If

        If Not Page.IsPostBack Then
            Session("QuizUseTablet") = True ' default use tablet

            Dim TestsetId As String = Session("newTestSetId").ToString
            Dim dtQuizDetail As DataTable = ClsActivity.GetTestset(TestsetId)
            Dim QuizClass As String = ClsActivity.GetMaxLevel(TestsetId).ToString
            Dim dtUserSettingDetail As DataTable = ClsActivity.GetUserSettingDetail(Session("UserId"), TestsetId)

            lblTestsetName.Text = dtQuizDetail.Rows(0)("Testset_Name").ToString
            SetupAnswer_Name.Value() = dtQuizDetail.Rows(0)("Testset_Name").ToString ' ใช้กับ ClsCheckMark
            lblQuestionAmount.Text = dtQuizDetail.Rows(0)("QuestionAmount").ToString
            QuestionAmount.Value() = dtQuizDetail.Rows(0)("QuestionAmount").ToString ' ใช้กับ ClsCheckMark
            lblLevel.Text = QuizClass
            HiddenTest.Value = QuizClass

            If Not dtUserSettingDetail.Rows.Count = 0 Then
                chkCheckTime.Checked = dtUserSettingDetail.Rows(0)("NeedTimer")
                chkShowAnswer.Checked = dtUserSettingDetail.Rows(0)("NeedCorrectAnswer")
                If dtUserSettingDetail.Rows(0)("IsPerQuestionMode") Then
                    IsPerQuestion.Checked = True
                    IsAll.Checked = False
                Else
                    IsPerQuestion.Checked = False
                    IsAll.Checked = True
                End If

                txtTimePerQuestion.Text = dtUserSettingDetail.Rows(0)("TimePerQuestion")
                txtTimeAll.Text = dtUserSettingDetail.Rows(0)("TimePerTotal")
                lblTimeAllPerQuestion.Text = Math.Round(CInt(lblQuestionAmount.Text) * CInt(txtTimePerQuestion.Text) / 60).ToString
                lblTimeAll.Text = Math.Round((CInt(txtTimeAll.Text) * 60) / CInt(lblQuestionAmount.Text), 2).ToString

                chkShowAnswer.Checked = dtUserSettingDetail.Rows(0)("NeedCorrectAnswer")
                chkRushMode.Checked = dtUserSettingDetail.Rows(0)("IsRushMode")

                If dtUserSettingDetail.Rows(0)("IsShowCorrectAfterComplete") Then
                    rdbAnswerAfter.Checked = True
                    rdbAnswerPerQuestion.Checked = False
                Else
                    rdbAnswerAfter.Checked = False
                    rdbAnswerPerQuestion.Checked = True
                End If

                txtTimeShowAnswer.Text = dtUserSettingDetail.Rows(0)("TimePerCorrectAnswer")
                chkRandomAnswer.Checked = dtUserSettingDetail.Rows(0)("NeedRandomAnswer")
                chkRandomQuestion.Checked = dtUserSettingDetail.Rows(0)("NeedRandomQuestion")

                If (dtUserSettingDetail.Rows(0)("IsDifferentQuestion")) Then
                    chkDiffQuestion.Checked = dtUserSettingDetail.Rows(0)("IsDifferentQuestion")
                Else
                    chkDiffQuestion.Checked = dtUserSettingDetail.Rows(0)("IsDifferentAnswer")
                End If

                chkSelfPace.Checked = dtUserSettingDetail.Rows(0)("Selfpace")
                chkShowScore.Checked = dtUserSettingDetail.Rows(0)("NeedShowScore")
                rdbEndQuiz.Checked = dtUserSettingDetail.Rows(0)("NeedShowScoreAfterComplete")

                If (HttpContext.Current.Application("NeedCheckmark") = True) Then
                    setElementCheckmarkOnPageLoad(TestsetId, dtQuizDetail.Rows(0)("QuestionAmount").ToString)
                End If

                setCheckboxUseTools(dtUserSettingDetail.Rows(0)("EnabledTools"))
            Else
                If (HttpContext.Current.Application("NeedCheckmark") = True) Then
                    setElementCheckmarkOnPageLoad(TestsetId, dtQuizDetail.Rows(0)("QuestionAmount").ToString)
                End If

                chkCheckTime.Checked = False
                chkShowAnswer.Checked = True

                IsPerQuestion.Checked = False
                IsAll.Checked = True

                Dim timeAll As String = dtQuizDetail.Rows(0)("TestSet_Time").ToString
                Dim quizAmount As String = dtQuizDetail.Rows(0)("QuestionAmount").ToString()
                Dim timePerQuestion As Integer = (CInt(timeAll) * 60) / CInt(quizAmount)

                If (timePerQuestion < 10) Then
                    timePerQuestion = 10
                End If

                txtTimePerQuestion.Text = timePerQuestion.ToString()
                txtTimeAll.Text = timeAll

                lblTimeAllPerQuestion.Text = Math.Round(CInt(lblQuestionAmount.Text) * CInt(txtTimePerQuestion.Text) / 60).ToString
                lblTimeAll.Text = Math.Round((CInt(txtTimeAll.Text) * 60) / CInt(lblQuestionAmount.Text), 2).ToString

                chkRushMode.Checked = False


                rdbAnswerAfter.Checked = True
                rdbAnswerPerQuestion.Checked = False


                txtTimeShowAnswer.Text = "30"
                chkRandomAnswer.Checked = False
                chkRandomQuestion.Checked = False


            End If



        End If

        Dim dtClass As DataTable = ClsActivity.GetClassName(Session("UserId"), "ป")

        Dim styleBtn As String = "width:60px;height:60px;"
        Dim styleBtnClass As String = "ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"

        Dim tagClass As String
        tagClass = "<tr><td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;padding-right:20px;'>ประถมศึกษา</td>"

        For Each a In dtClass.Rows
            tagClass &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'><input id='btnChangeLv' type=""button"" value='" & (a("Class_Order") - 3) & "' onclick=""ChangeLv('" & a("className") & "')"" style='" & styleBtn & "' class='" & styleBtnClass & "'/></td>"
        Next
        tagClass &= "</tr>"
        PClass.InnerHtml = tagClass

        Dim dtClass2 As DataTable = ClsActivity.GetClassName(Session("UserId"), "ม")

        Dim tagClass2 As String
        tagClass2 = "<tr><td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'>มัธยมศึกษา</td>"

        For Each b In dtClass2.Rows
            Select Case b("Class_Order")
                Case "10"
                    b("Class_Order") = "1"
                Case "11"
                    b("Class_Order") = "2"
                Case "12"
                    b("Class_Order") = "3"
                Case "13"
                    b("Class_Order") = "4"
                Case "14"
                    b("Class_Order") = "5"
                Case "15"
                    b("Class_Order") = "6"

            End Select
            tagClass2 &= "<td style='background:inherit;color:inherit;border-bottom:inherit;padding:inherit;font-size: larger;'><input id='btnChangeLv' type=""button"" value='" & b("Class_Order") & "' onclick=""ChangeLv('" & b("className") & "')""  style='" & styleBtn & "' class='" & styleBtnClass & "'/></td>"
        Next
        tagClass2 &= "</tr>"
        MClass.InnerHtml = tagClass2

        ' 02-05-56 update หาวิชาที่อยู่ใน testset เพื่อแสดง tools ให้เลือกใช้
        getSubjectInTestsetAndSetTools(Session("newTestSetId").ToString())

    End Sub

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click

        Dim ArrCheckBoolean() As String = {chkCheckTime.Checked, IsPerQuestion.Checked, chkShowAnswer.Checked, rdbAnswerAfter.Checked, _
                                          chkRushMode.Checked, chkRandomQuestion.Checked, chkRandomAnswer.Checked, ChkInShow1.Checked, _
                                          chkShowScore.Checked, rdbEndQuiz.Checked, chkDiffQuestion.Checked, chkSelfPace.Checked}

        For j = 0 To ArrCheckBoolean.Length - 1
            If ArrCheckBoolean(j) = True Then
                ArrCheckBoolean(j) = "1"
            Else
                ArrCheckBoolean(j) = "0"
            End If
        Next

        ' แก้ Bug ปุ่ม back ไม่ขึ้น 
        If ArrCheckBoolean(0) = "0" Then
            ArrCheckBoolean(1) = "0"
        End If

        Dim totalStudent As String
        If Session("QuizUseTablet") = False Then
            totalStudent = "0"
        Else
            totalStudent = Session("TotalStudent").ToString()
        End If

        'คำถามแต่ละคนเหมือนกันหรือเปล่า
        Dim differentQuestion As String = ""
        If (chkRandomQuestion.Checked AndAlso chkDiffQuestion.Checked) Then
            differentQuestion = "1"
        Else
            differentQuestion = "0"
        End If
        'คำตอบแต่ละคนเหมือนกันหรือเปล่า
        Dim differentAnswer As String = ""
        If (chkRandomAnswer.Checked AndAlso chkDiffQuestion.Checked) Then
            differentAnswer = "1"
        Else
            differentAnswer = "0"
        End If

        Dim AcademicYear As String = ClsActivity.GetAcademicYear()
        Dim LvName As String = HiddenTest.Value
        Dim roomName As String = "/" & txtRoom.Text.ToString()

        ' 02-05-56 update use tools
        Dim EnabledTools As Integer = 0
        If chkUseTools.Checked Then
            EnabledTools = setToolsInQuiz()
        End If

        ' ทำการ update endtime ที่ยังเป็น null โดยเป็นของห้องที่กำลังจะจัดควิซ
        'ClsActivity.setEndTimeNotNullBeforeStartQuiz(LvName, roomName, Session("SchoolId").ToString())

        Dim QuizId As String = ClsActivity.SaveQuizDetail(Session("newTestSetId").ToString(), LvName, "/" & txtRoom.Text, totalStudent, AcademicYear, ArrCheckBoolean(0), _
                                   ArrCheckBoolean(1), txtTimePerQuestion.Text, txtTimeAll.Text, ArrCheckBoolean(2), txtTimeShowAnswer.Text, _
                                  ArrCheckBoolean(3), ArrCheckBoolean(4), ArrCheckBoolean(5), ArrCheckBoolean(6), Session("UserId").ToString(), _
                                   Session("SchoolId").ToString(), Session("SchoolId").ToString(), ArrCheckBoolean(7), chkQuizUseTablet.Checked, ArrCheckBoolean(8), _
                                  ArrCheckBoolean(9), differentQuestion, ArrCheckBoolean(11), differentAnswer, EnabledTools)
        Session("Quiz_Id") = QuizId
        'Session("TotalStudent") = txtTotalStudent.Text        

        If Session("QuizUseTablet") = True Then
            ClsActivity.SetStudent(QuizId, Session("SchoolId").ToString(), LvName, "/" & txtRoom.Text)
            ClsActivity.setTeacher(Session("UserId").ToString(), QuizId, Session("SchoolId").ToString()) ' เพิ่ม tablet ครู เข้าไปใน quizsession ด้วย
        End If

        ' save question ลง tblQuizQuestion
        Dim swapQuestion As Boolean = chkRandomQuestion.Checked
        Dim swapAnswer As Boolean = chkRandomAnswer.Checked
        getQsetInQuiz(swapQuestion, swapAnswer)

        ' save ข้อมูล ลงไปใน table checkAnswer2
        If chkUseTemplate.Checked = True Then
            Dim ClsCheckMark As New ClsCheckMark
            Dim TemplateName = setAmountChoice(QuestionAmount.Value().ToString()) 'get tamplate from questionAmount
            Dim qAmount = 0 ' บันทึกจำนวนข้อตามจำนวนที่กด next เลยให้เป็น 0 ก่อน
            Dim detail As String = setAnserNameCheckmark(LvName)
            Dim setupName As String = SetupAnswer_Name.Value().ToString() & detail
            ClsCheckMark.saveQuizToCheckmark(setupName, TemplateName, qAmount, LvName, Session("newTestSetId").ToString) 'save data to db checkAnswer2
            ClsCheckMark.InsertRefToCheckMarkIntblCM(Session("newTestSetId").ToString)
            ClsCheckMark.updateConnectCheckmark("1")
        Else
            Dim ClsCheckMark As New ClsCheckMark
            ClsCheckMark.updateConnectCheckmark("0")
        End If

        Dim clsSelectSess As New ClsSelectSession()
        clsSelectSess.SetSessionQuizId(QuizId)

        If chkShowAnswer.Checked Then
            If chkRushMode.Checked Then
                Response.Redirect("RaceActivity.aspx")
            Else
                Response.Redirect("ActivityPage_PadTeacher.aspx")
            End If
        Else
            Response.Redirect("ActivityPage_PadTeacher.aspx")
        End If


    End Sub

    ' <<< หาจำนวนนักเรียนในห้อง >>>
    <Services.WebMethod()>
    Public Shared Function GetStudentAmountCodeBehide(ByVal ClassName As String, ByVal RoomName As String) As String
        RoomName = "/" & RoomName
        Dim ClsActivity As New ClsActivity(New ClassConnectSql)
        Dim SchoolCode As String = HttpContext.Current.Session("SchoolID").ToString
        Dim studentAmount = ClsActivity.GetStudentAmount(ClassName, RoomName, SchoolCode)
        HttpContext.Current.Session("TotalStudent") = studentAmount
        GetStudentAmountCodeBehide = "นักเรียนทั้งหมด " & studentAmount & " คน"
    End Function

    ' <<< session ของ useTablet >>>
    <Services.WebMethod()>
    Public Shared Function checkQuizUseTablet(ByVal checked As String)
        Dim chk As Boolean
        If (checked = "checked") Then
            HttpContext.Current.Session("QuizUseTablet") = True
            chk = True
        Else
            HttpContext.Current.Session("QuizUseTablet") = False
            chk = False
        End If

        Return chk
    End Function

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnBack.Click
        Response.Redirect("../testset/Step1_PadTeacher.aspx")
    End Sub

    ' <<< set ชื่อของกระดาษคำตอบ checkmark >>>
    Private Function setAmountChoice(ByVal questionAmount As String) As String
        Dim template As String = ""
        If (CInt(questionAmount) <= 60) Then
            template = "Wpp02 QR 120 Choice"
        ElseIf (CInt(questionAmount) > 60) Then
            template = "Wpp02 QR 120 Choice"
        End If
        Return template
    End Function

    ' <<< session ของ checkmark >>>
    <Services.WebMethod()>
    Public Shared Function checkQuizUseTamplate(ByVal checked As String)
        If (checked = "checked") Then
            HttpContext.Current.Session("QuizUseTamplate") = True
        Else
            HttpContext.Current.Session("QuizUseTamplate") = False
        End If
        Return "success"
    End Function

    ' <<< set รายละเอียดต่างๆๆ ของ checkmark >>>
    Private Function setAnserNameCheckmark(ByVal LvName As String) As String
        Dim className As String = LvName & "/" & txtRoom.Text.Trim()
        Dim currentDate As String = Date.Now.ToString("dd/MM/yy HH:mm")
        setAnserNameCheckmark = "(" & className & " - " & currentDate & ")"
        Return setAnserNameCheckmark
    End Function

    ' <<< set checkbox ของ checkmark ว่าชุดก่อนถูกใช้หรือเปล่า >>>
    Private Sub setElementCheckmarkOnPageLoad(ByVal testsetId As String, ByVal qAmount As String)
        'ถ้าข้อสอบเกิน 120 ให้ checkbox-checkmark disabled
        If (CInt(qAmount) > 120) Then
            chkUseTemplate.Enabled = False
            'divUseTemplate.Attributes("class") = "Over"
        Else
            Dim db As New ClassConnectSql()
            Dim sql As String = " SELECT tblQuestionSet.QSet_Type FROM tblTestSetQuestionSet INNER JOIN "
            sql &= " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id"
            sql &= " WHERE tblTestSetQuestionSet.TestSet_Id = '" & testsetId & "' AND tblQuestionSet.QSet_Type <> '1' "
            sql &= " AND tblQuestionSet.QSet_Type <> '2' and tblTestSetQuestionSet.IsActive = '1';"
            Dim dt As DataTable = db.getdata(sql)
            ' เช็คว่าชุดนั้นมีข้อสอบที่ไม่สามารถใช้ checkmark ได้หรือไม่
            If (dt.Rows.Count > 0) Then
                chkUseTemplate.Enabled = False
                'divUseTemplate.Attributes("class") = "TypeError"
            Else
                Dim needCheckmark As String = db.ExecuteScalar(" SELECT NeedConnectCheckmark FROM tblTestSet WHERE TestSet_Id = '" & testsetId & "'; ")
                ' ถ้าชุดข้อสอบนี้ เคยใช้ checkmark แล้วในครั้งก่อน
                If (needCheckmark = "True") Then
                    chkQuizUseTablet.Checked = False
                    chkUseTemplate.Checked = True
                    Session("QuizUseTablet") = False
                Else
                    Session("QuizUseTablet") = True
                End If
            End If
        End If
    End Sub

    ' <<< หา qset ที่อยู่ใน quiz >>>
    Private Sub getQsetInQuiz(ByVal isDifferentQuestion As Boolean, ByVal isDiffAnswer As Boolean)

        'สุ่มคำถาม (สุ่ม Qset) ทำให้ข้อสอบของแต่ละ Qset ไม่สามารถปนกันได้
        Dim db As New ClassConnectSql()

        Dim sqlGetQuestionInTestset As String = " SELECT tqs.QSet_Id,qs.QSet_Type,qs.QSet_IsRandomQuestion,qs.QSet_IsRandomAnswer "
        sqlGetQuestionInTestset &= " FROM tblTestSetQuestionSet tqs LEFT JOIN tblQuestionSet qs "
        sqlGetQuestionInTestset &= " ON tqs.QSet_Id = qs.QSet_Id "
        sqlGetQuestionInTestset &= " WHERE tqs.TestSet_Id = '" & Session("newTestSetId").ToString & "' "
        sqlGetQuestionInTestset &= " And tqs.IsActive = '1'"
        Dim dtQset As New DataTable()

        If (isDifferentQuestion) Then
            sqlGetQuestionInTestset &= " ORDER BY NEWID(); "
            dtQset = db.getdata(sqlGetQuestionInTestset)
            insertQuestionToQuizQuestion(dtQset, True, isDiffAnswer)
        Else
            dtQset = db.getdata(sqlGetQuestionInTestset)
            insertQuestionToQuizQuestion(dtQset, False, isDiffAnswer)
        End If

    End Sub

    ' <<< insert ข้อสอบ จาก qset >>>
    Private Sub insertQuestionToQuizQuestion(ByVal dtQset As DataTable, ByVal isDiffQuestion As Boolean, ByVal isDiffAnswer As Boolean)

        Dim db As New ClassConnectSql()
        Dim qq_no As Integer = 1

        For i As Integer = 0 To dtQset.Rows.Count - 1
            Dim sqlQuestionInQset As String = " SELECT tsqd.Question_Id FROM tblTestSetQuestionSet tsqs LEFT JOIN tblTestSetQuestionDetail tsqd "
            sqlQuestionInQset &= " ON tsqs.TSQS_Id = tsqd.TSQS_Id "
            sqlQuestionInQset &= " WHERE tsqs.TestSet_Id = '" & Session("newTestSetId").ToString & "' "
            sqlQuestionInQset &= " AND tsqs.QSet_Id = '" & dtQset(i)("QSet_Id").ToString() & "' "
            sqlQuestionInQset &= " And tsqs.isActive = '1' And tsqd.IsActive = '1'"
            'sql get question in qset

            Dim dtQuestionInQset As DataTable

            'ถ้าเป็น Type 6 ต้องดูด้วยว่าสุ่มคำตอบมั้ย ถ้าสุ่มคำตอบต้องสุ่มคำถาม เพราะเราเอาคำถามไปเป็นคำตอบ ถ้าไม่เช็ค เวลาเลือกสุ่มคำตอบอย่างเดียว จะไม่สุ่มให้

            If dtQset.Rows(i)("QSet_Type") = "6" Then
                If (isDiffAnswer) AndAlso dtQset.Rows(i)("QSet_IsRandomQuestion") Then
                    sqlQuestionInQset &= " ORDER BY NEWID(); "
                Else
                    sqlQuestionInQset &= " ORDER BY tsqd.TSQD_No; "
                End If
            Else
                If (isDiffQuestion) AndAlso dtQset.Rows(i)("QSet_IsRandomQuestion") Then 'question ใน qset ต้องสุ่มหรือเปล่า
                    sqlQuestionInQset &= " ORDER BY NEWID(); "
                Else
                    sqlQuestionInQset &= " ORDER BY tsqd.TSQD_No; "
                End If
            End If

            dtQuestionInQset = db.getdata(sqlQuestionInQset)

            db.OpenWithTransection()
            Dim sqlInsertQuestion As String = ""
            If dtQset.Rows(i)("QSet_Type") = "6" Then
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Session("Quiz_Id").ToString & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion)
                Next
                qq_no = qq_no + 1
            Else
                For Each question As DataRow In dtQuestionInQset.Rows()
                    sqlInsertQuestion = " INSERT INTO tblQuizQuestion (Quiz_Id,Question_Id,QQ_No,School_Code) VALUES('" & Session("Quiz_Id").ToString & "','" & question.Item("Question_Id").ToString() & "','" & qq_no & "','" & HttpContext.Current.Session("SchoolID").ToString() & "'); "
                    db.ExecuteWithTransection(sqlInsertQuestion)
                    qq_no = qq_no + 1
                Next
            End If
            db.CommitTransection()
        Next

    End Sub

    ' 02-05-56 update หาวิชาที่อยู่ใน testset เพื่อเปิดการใช้งาน tools
    Private Sub getSubjectInTestsetAndSetTools(ByVal Testset_Id As String)

        Dim db As New ClassConnectSql()
        Dim sqlSubject As String = " SELECT tgs.GroupSubject_Name FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionSet tqs "
        sqlSubject &= " ON tsqs.QSet_Id = tqs.QSet_Id INNER JOIN tblQuestionCategory tqc "
        sqlSubject &= "ON tqs.QCategory_Id = tqc.QCategory_Id INNER JOIN tblbook tb "
        sqlSubject &= "ON tqc.Book_Id = tb.Book_Id INNER JOIN tblGroupSubject tgs "
        sqlSubject &= "ON tb.GroupSubject_Id = tgs.GroupSubject_Id "
        sqlSubject &= "WHERE tsqs.TestSet_Id = '" & Testset_Id & "' And tsqs.IsActive = '1';"

        Dim dtSubject As DataTable = db.getdata(sqlSubject)

        For Each subject As DataRow In dtSubject.Rows()
            Select Case subject.Item("GroupSubject_Name").ToString()
                Case "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                    toolsSubject_Eng = True
                Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                    toolsSubject_Math = True
            End Select
        Next
    End Sub
    ' update 02-05-56 tools ที่ใช้ใน quiz มีอะไรบ้าง
    Private Function setToolsInQuiz() As Integer
        Dim arrTools As Array = {chkWithCalculator.Checked, chkWithDictionary.Checked, chkWithWordBook.Checked, chkWithNotes.Checked, chkWithProtractor.Checked}
        Dim EnabledTools As Integer = 0
        Dim Tools As Array = {2, 4, 8, 16, 32}
        Dim num As Integer = 0

        For Each arrChecked As Boolean In arrTools
            If arrChecked = True Then
                EnabledTools = EnabledTools + Tools(num)
            End If
            num = num + 1
        Next

        Return EnabledTools
    End Function
    ' set checkbox Tools T/F on page load
    Private Sub setCheckboxUseTools(ByVal EnabledTools As Integer)
        If Not EnabledTools = 0 Then
            chkUseTools.Checked = True
            ' calculator
            If (EnabledTools And 2) = 2 Then
                chkWithCalculator.Checked = True
            End If
            ' dictionary
            If (EnabledTools And 4) = 4 Then
                chkWithDictionary.Checked = True
            End If
            ' wordbook
            If (EnabledTools And 8) = 8 Then
                chkWithWordBook.Checked = True
            End If
            ' note
            If (EnabledTools And 16) = 16 Then
                chkWithNotes.Checked = True
            End If
            ' protractor
            If (EnabledTools And 32) = 32 Then
                chkWithProtractor.Checked = True
            End If
        End If
    End Sub
End Class