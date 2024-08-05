Imports KnowledgeUtils.System

Public Class ShowTestsetDetailPage
    Inherits System.Web.UI.Page
    Dim db As New ClassConnectSql()
    Dim Cls As New ClsPDF(db)

    Private Enum EnumQuizMode
        NotDifferent = 1
        DifferentQuestion = 2
        DifferentAnswer = 3
        DifferentBoth = 4
    End Enum

    Private Enum EnumQsetType
        Choice = 1
        TrueFalse = 2
        Pair = 3
        Sort = 6
    End Enum

    Public IsPlayQuiz As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim PageName As String = System.IO.Path.GetFileName(Request.Url.ToString())
        'Log.Record(Log.LogType.PageLoad, PageName, True)

        Dim TestsetID As String = Request.QueryString("TestsetID")
        If Not TestsetID Is Nothing Then
            myCtlTestsetDetail.ProcessByTestSetId(TestsetID)
            TestsetDetail.InnerHtml = GetHtmlTestsetDetail(TestsetID)
        Else
            Dim qsetId As String = Request.QueryString("qsetid")
            If qsetId IsNot Nothing Then
                myCtlTestsetDetail.Visible = False
                TestsetDetail.InnerHtml = GetContentQsetDetail(qsetId)
            Else
                ' ทำเผื่อ quiz ของหน้า dashboard นักเรียน
                Quiz_ID = Request.QueryString("QuizID")
                Student_ID = Request.QueryString("StudentID")
                myCtlTestsetDetail.SetByQuizId(Quiz_ID, Student_ID)
                CheckIsPlayQuiz(Quiz_ID, Student_ID)
                If IsPlayQuiz Then
                    TestsetDetail.InnerHtml = GetHtmlQuizDetail(True)
                End If
                QuizDetailCorrect.InnerHtml = GetHtmlQuizDetail(False)
            End If
        End If
    End Sub

    Private Sub CheckIsPlayQuiz(Quiz_Id As String, Player_Id As String)
        Dim sql As String
        sql = " select SUM(CASE WHEN dbo.tblQuizScore.Answer_Id IS NOT NULL THEN 1 ELSE 0 END) AS ResultIsDone from tblQuizScore  " & _
              " where Quiz_Id = '" & Quiz_Id & "' and Student_Id = '" & Player_Id & "';"
        Dim AnsweredQuiz As String = db.ExecuteScalar(sql)
        If AnsweredQuiz <> "0" And AnsweredQuiz <> "" Then
            IsPlayQuiz = True
        End If
    End Sub

#Region "Testset"
    ' HTML Testset Detail
    Private Function GetHtmlTestsetDetail(ByVal TestsetID As String) As String
        Dim htmlTestset As New StringBuilder()

        Dim dtQset As DataTable = GetQsetInTestset(TestsetID)

        ExamNum = 1
        For Each row In dtQset.Rows
            Qset_ID = row("Qset_Id").ToString()
            htmlTestset.Append("<div><table style='width:100%'><tr><td style='vertical-align:top;'><qh>")
            If row("Qset_Type") = EnumQsetType.Pair Or row("Qset_Type") = EnumQsetType.Sort Then
                htmlTestset.Append(ExamNum)
                htmlTestset.Append(". ")
                ExamNum = ExamNum + 1
            End If
            htmlTestset.Append("</qh></td><td style='text-align:justify;'><qh>")
            htmlTestset.Append("<div>" & row("Qset_Name").ToString() & "</div>")
            htmlTestset.Append("</qh></tr></table>")
            htmlTestset.Append(SetHtmlFormTestset(row("Qset_Type"), row("TSQS_Id").ToString(), row("TSQS_No")))
            htmlTestset.Append("</div>")
        Next
        Return htmlTestset.ToString()
    End Function

    ''' <summary>
    ''' สำหรับสร้าง content เพื่อแสดงรายละเอียดของ qset ที่จะทำการ copy
    ''' </summary>
    ''' <returns></returns>
    Private Function GetContentQsetDetail(qsetId As String) As String
        Dim htmlQset As New StringBuilder()
        Dim dtQset As DataTable = db.getdata("SELECT * FROM tblQuestionset WHERE QSet_Id = '" & qsetId & "';")
        ExamNum = 1
        For Each row In dtQset.Rows
            Qset_ID = row("Qset_Id").ToString()
            htmlQset.Append("<div><table style='width:100%'><tr><td style='vertical-align:top;'><qh>")
            If row("Qset_Type") = EnumQsetType.Pair Or row("Qset_Type") = EnumQsetType.Sort Then
                htmlQset.Append(ExamNum)
                htmlQset.Append(". ")
                ExamNum = ExamNum + 1
            End If
            htmlQset.Append("</qh></td><td style='text-align:justify;'><qh>")
            htmlQset.Append("<div>" & row("Qset_Name").ToString() & "</div>")
            htmlQset.Append("</qh></tr></table>")
            htmlQset.Append(SetHtmlFromQset(row("Qset_Type"), qsetId))
            htmlQset.Append("</div>")
        Next
        Return htmlQset.ToString()
    End Function

    ' function แบบ testset
    Private Function SetHtmlFormTestset(ByVal QsetType As Integer, ByVal TSQS_ID As String, ByVal TSQS_No As Integer)
        Dim htmlForm As String = ""
        Dim dtQuestion As DataTable = GetQuestionAndAnswerInQset(TSQS_ID, QsetType, TSQS_No)
        Select Case QsetType
            Case EnumQsetType.Choice ' สร้าง type 1
                htmlForm = HtmlType1(dtQuestion) ' -- CHOICE
            Case EnumQsetType.TrueFalse
                htmlForm = HtmlType2(dtQuestion) ' -- T/F
            Case EnumQsetType.Pair
                htmlForm = HtmlType3() ' -- จับคู่
            Case EnumQsetType.Sort
                htmlForm = HtmlType6(dtQuestion) ' -- Sort
        End Select
        Return htmlForm
    End Function

    ' เอา qset ทั้งหมดที่อยู่ใน testset ออกมา
    Private Function GetQsetInTestset(ByVal TestsetID As String) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT * FROM tblTestSetQuestionSet INNER JOIN tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id ")
        'sql.Append(" WHERE tblTestSetQuestionSet.IsActive = 1  AND tblTestSetQuestionSet.TestSet_Id = '")
        sql.Append(" WHERE  tblTestSetQuestionSet.TestSet_Id = '")
        sql.Append(TestsetID)
        sql.Append("' AND tblTestsetQuestionSet.IsActive = '1' ORDER BY tblTestSetQuestionSet.TSQS_No; ")
        Return db.getdata(sql.ToString())
    End Function

    ' เอา question and answer ที่อยู่ใน qset ออกมา
    Private Function GetQuestionAndAnswerInQset(ByVal TSQS_ID As String, ByVal QsetType As Integer, ByVal TSQS_No As Integer) As DataTable
        Dim sql As New StringBuilder()
        sql.Append(" SELECT tblTestSetQuestionDetail.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Name,tblAnswer.Answer_Score FROM tblTestSetQuestionDetail ")
        sql.Append(" INNER JOIN tblQuestion ON tblTestSetQuestionDetail.Question_Id = tblQuestion.Question_Id INNER JOIN tblAnswer ON tblTestSetQuestionDetail.Question_Id = tblAnswer.Question_Id ")
        sql.Append(" WHERE tblTestSetQuestionDetail.IsActive = 1 AND tblAnswer.IsActive = 1")
        sql.Append(" And tblTestSetQuestionDetail.TSQS_Id = '")
        sql.Append(TSQS_ID)

        Select Case QsetType
            Case EnumQsetType.Choice
                If TSQS_No = -1 Then
                    sql.Append("'  ORDER BY tblTestSetQuestionDetail.Question_Id,tblTestSetQuestionDetail.TSQD_No,tblAnswer.Answer_No; ")
                Else
                    sql.Append("'  ORDER BY tblTestSetQuestionDetail.TSQD_No,tblAnswer.Answer_No; ")
                End If
            Case EnumQsetType.TrueFalse
                sql.Append("' AND tblAnswer.Answer_No = 1 ORDER BY tblTestSetQuestionDetail.TSQD_No; ")
            Case EnumQsetType.Pair
                sql.Append("' AND tblAnswer.Answer_Score = 1 ORDER BY tblTestSetQuestionDetail.TSQD_No; ")
            Case EnumQsetType.Sort
                sql.Append("' AND tblAnswer.Answer_Score = 1 ORDER BY CAST(tblAnswer.Answer_Name AS VARCHAR); ")
        End Select
        Return db.getdata(sql.ToString())
    End Function
#End Region

#Region "Quiz"
    ' HTML Quiz Detail
    'Private Quiz_ID As String = "B1D32254-0DBF-4466-B246-3969F7FBB263"
    'Private Student_ID As String = "647BC646-885D-409D-B16C-3963480AB9E4"
    Private Quiz_ID As String
    Private Student_ID As String

    Private Function GetHtmlQuizDetail(ByVal IsStudent As Boolean) As String
        Dim HtmlQuiz As New StringBuilder()

        Dim dtQsetInQuiz As DataTable = GetQsetInQuiz()

        ExamNum = 1
        For Each row In dtQsetInQuiz.Rows
            Qset_ID = row("Qset_Id").ToString()

            HtmlQuiz.Append("<div><table style='width:100%'><tr><td style='vertical-align:top;'><qh>")

            If row("Qset_Type") = EnumQsetType.Pair Or row("Qset_Type") = EnumQsetType.Sort Then
                HtmlQuiz.Append("<div>" & ExamNum)
                'HtmlQuiz.Append(". " & "<div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></div>")
                HtmlQuiz.Append(". " & "</div>")
                ExamNum = ExamNum + 1
            End If

            HtmlQuiz.Append("</qh></td><td style='text-align:justify;'><qh>")
            HtmlQuiz.Append(row("Qset_Name").ToString())
            HtmlQuiz.Append("</qh></tr></table>")
            HtmlQuiz.Append(SetHtmlFormQuiz(row("Qset_Type"), IsStudent))
            HtmlQuiz.Append("</div>")
        Next
        GetHtmlQuizDetail = HtmlQuiz.ToString()
        If IsPlayQuiz = False Then
            QuizDetailCorrect.Style.Add("display", "block")
            TestsetDetail.Style.Add("display", "none")
        End If
    End Function

    Private Function SetHtmlFromQset(ByVal QsetType As Integer, qsetId As String)
        Dim htmlForm As String = ""

        'Select Case QsetType
        '    Case EnumQsetType.Choice
        '        If TSQS_No = -1 Then
        '            Sql.Append("'  ORDER BY tblTestSetQuestionDetail.Question_Id,tblTestSetQuestionDetail.TSQD_No,tblAnswer.Answer_No; ")
        '        Else
        '            Sql.Append("'  ORDER BY tblTestSetQuestionDetail.TSQD_No,tblAnswer.Answer_No; ")
        '        End If
        '    Case EnumQsetType.TrueFalse
        '        Sql.Append("' AND tblAnswer.Answer_No = 1 ORDER BY tblTestSetQuestionDetail.TSQD_No; ")
        '    Case EnumQsetType.Pair
        '        Sql.Append("' AND tblAnswer.Answer_Score = 1 ORDER BY tblTestSetQuestionDetail.TSQD_No; ")
        '    Case EnumQsetType.Sort
        '        Sql.Append("' AND tblAnswer.Answer_Score = 1 ORDER BY CAST(tblAnswer.Answer_Name AS VARCHAR); ")
        'End Select
        Dim sql As String = "SELECT * FROM tblQuestion q INNER JOIN tblAnswer a ON q.Question_Id = a.Question_Id WHERE q.QSet_Id = '" & qsetId & "' "
        If QsetType = EnumQsetType.TrueFalse Then
            sql &= " AND a.answer_no = 1 "
        End If
        sql &= " ORDER BY q.Question_No,a.Answer_No;"
        Dim dtQuestion As DataTable = db.getdata(sql)
        Select Case QsetType
            Case EnumQsetType.Choice ' สร้าง type 1
                htmlForm = HtmlType1(dtQuestion) ' -- CHOICE
            Case EnumQsetType.TrueFalse
                htmlForm = HtmlType2(dtQuestion) ' -- T/F
            Case EnumQsetType.Pair
                htmlForm = HtmlType3() ' -- จับคู่
            Case EnumQsetType.Sort
                htmlForm = HtmlType6(dtQuestion) ' -- Sort
        End Select
        Return htmlForm
    End Function

    'Private Function GetSelectFromDatatable(ByVal dtQsetQA As DataTable, ByVal QsetID As String, ByVal strAnswer As String) As DataTable
    '    Dim res = (From row In dtQsetQA.AsEnumerable() Where row("Qset_Id").ToString() = QsetID Select New With {.Question_Id = row("Question_Id"), .Question_Name = row("Question_Name"), .Answer_Name = row("Answer_Name"), .Answer_Score = row(strAnswer), .IsCorrect = row("Answer_Score"), .NoAnswer = row("NoAnswer")}).ToArray()
    '    GetSelectFromDatatable = res.ToDataTable()
    'End Function




    ' funtion แบบ quiz
    Private Function SetHtmlFormQuiz(ByVal QsetType As Integer, ByVal IsStudent As Boolean)
        Dim htmlForm As String
        Dim dtQuestion As DataTable = GetQuestionAndAnswerInQuiz(QsetType)
        Select Case QsetType
            Case EnumQsetType.Choice
                If IsStudent Then
                    htmlForm = HtmlType1Check(dtQuestion) ' -- CHOICE
                Else
                    htmlForm = HtmlType1(dtQuestion) ' -- CHOICE
                End If
            Case EnumQsetType.TrueFalse
                If IsStudent Then
                    htmlForm = HtmlType2Check(dtQuestion) ' -- T/F
                Else
                    htmlForm = HtmlType2(dtQuestion)
                End If
            Case EnumQsetType.Pair
                If IsStudent Then
                    htmlForm = HtmlType3Check(dtQuestion) ' -- จับคู่
                Else
                    htmlForm = HtmlType3() ' -- จับคู่
                End If
            Case EnumQsetType.Sort
                If IsStudent Then
                    htmlForm = HtmlType6Check(dtQuestion) ' -- Sort
                Else
                    htmlForm = HtmlType6(dtQuestion, True) ' -- Sort
                End If
        End Select
        SetHtmlFormQuiz = htmlForm
    End Function
    ' เอา question จาก quiz
    Private Function GetQuestionAndAnswerInQuiz(ByVal Qset_Type As Integer) As DataTable

        Dim QuizMode As Integer = GetQuizMode(Quiz_ID)

        Dim sql As New StringBuilder()

        Select Case Qset_Type

            Case EnumQsetType.Choice
                ' Choice / กากบาททททททททททททท
                If Not QuizMode = EnumQuizMode.DifferentAnswer Or QuizMode = EnumQuizMode.DifferentBoth Then
                    sql.Append(" SELECT CASE WHEN t.QQ_No IS NULL THEN 999 ELSE t.QQ_No END AS Q_No,tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Name,tblAnswer.Answer_Score,")
                    sql.Append(" t.Answer_Id AS AnswerStudent,tblAnswer.Answer_Id AS AnswerCorrect")
                    sql.Append(" FROM tblQuizQuestion left join (select * from tblQuizScore where Student_Id = '" & Student_ID & "'")
                    sql.Append(" and Quiz_Id = '" & Quiz_ID & "') as t ON tblQuizQuestion.Question_Id = t.Question_Id AND tblQuizQuestion.Quiz_Id = t.Quiz_Id")
                    sql.Append(" Left JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id  LEFT JOIN tblAnswer ON tblQuizQuestion.Question_Id = tblAnswer.Question_Id")
                    sql.Append(" LEFT JOIN tblQuizAnswer ON tblAnswer.Answer_Id = tblQuizAnswer.Answer_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id")
                    sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '" & Quiz_ID & "'")
                    sql.Append(" AND (tblQuestion.QSet_Id = '" & Qset_ID & "' OR tblQuestion.QSet_Id IS NULL)")
                    sql.Append(" ORDER BY Q_No,tblQuizQuestion.QQ_No,tblQuizAnswer.QA_No,tblAnswer.Answer_No;")
                Else
                    sql.Append(" SELECT CASE WHEN tblQuizScore.QQ_No IS NULL THEN 999 ELSE tblQuizScore.QQ_No END AS Q_No, ")
                    sql.Append(" tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Name,tblAnswer.Answer_Score,tblQuizScore.Answer_Id AS AnswerStudent,tblAnswer.Answer_Id AS AnswerCorrect ")
                    sql.Append(" FROM tblQuizQuestion LEFT JOIN tblQuizScore ON tblQuizQuestion.Question_Id = tblQuizScore.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizScore.Quiz_Id ")
                    sql.Append(" LEFT JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id ")
                    sql.Append(" LEFT JOIN tblAnswer ON tblQuizQuestion.Question_Id = tblAnswer.Question_Id  ")
                    sql.Append(" LEFT JOIN tblQuizAnswer ON tblAnswer.Answer_Id = tblQuizAnswer.Answer_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id ")
                    sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '")
                    sql.Append(Quiz_ID)
                    sql.Append("' AND (tblQuizScore.Quiz_Id = '")
                    sql.Append(Quiz_ID)
                    sql.Append("' OR tblQuizScore.Quiz_Id IS NULL) ")
                    sql.Append(" AND (tblQuizScore.Student_Id = '")
                    sql.Append(Student_ID)
                    sql.Append("' OR tblQuizScore.Student_Id IS NULL) ")
                    sql.Append(" AND (tblQuestion.QSet_Id = '")
                    sql.Append(Qset_ID)
                    sql.Append("' OR tblQuestion.QSet_Id IS NULL) ")
                    'If QuizMode = EnumQuizMode.DifferentAnswer Or QuizMode = EnumQuizMode.DifferentBoth Then
                    sql.Append(" AND (tblQuizAnswer.Player_Id = '")
                    sql.Append(Student_ID)
                    sql.Append("' OR tblQuizAnswer.Player_Id IS NULL)")
                    'End If
                    sql.Append(" ORDER BY Q_No,tblQuizQuestion.QQ_No,tblQuizAnswer.QA_No,tblAnswer.Answer_No; ")
                End If



                'sql.Append(" SELECT CASE WHEN tblQuizScore.QQ_No IS NULL THEN 999 ELSE tblQuizScore.QQ_No END AS Q_No, ")
                'sql.Append(" tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Name,tblAnswer.Answer_Score,tblQuizScore.Answer_Id AS AnswerStudent,tblAnswer.Answer_Id AS AnswerCorrect ")
                'sql.Append(" FROM tblQuizQuestion LEFT JOIN tblQuizScore ON tblQuizQuestion.Question_Id = tblQuizScore.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizScore.Quiz_Id ")
                'sql.Append(" LEFT JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id ")
                'sql.Append(" LEFT JOIN tblAnswer ON tblQuizQuestion.Question_Id = tblAnswer.Question_Id  ")
                'sql.Append(" LEFT JOIN tblQuizAnswer ON tblAnswer.Answer_Id = tblQuizAnswer.Answer_Id AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id ")
                'sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '")
                'sql.Append(Quiz_ID)
                'sql.Append("' AND (tblQuizScore.Quiz_Id = '")
                'sql.Append(Quiz_ID)
                'sql.Append("' OR tblQuizScore.Quiz_Id IS NULL) ")
                'sql.Append(" AND (tblQuizScore.Student_Id = '")
                'sql.Append(Student_ID)
                'sql.Append("' OR tblQuizScore.Student_Id IS NULL) ")
                'sql.Append(" AND (tblQuestion.QSet_Id = '")
                'sql.Append(Qset_ID)
                'sql.Append("' OR tblQuestion.QSet_Id IS NULL) ")
                ''If QuizMode = EnumQuizMode.DifferentAnswer Or QuizMode = EnumQuizMode.DifferentBoth Then
                'sql.Append(" AND (tblQuizAnswer.Player_Id = '")
                'sql.Append(Student_ID)
                'sql.Append("' OR tblQuizAnswer.Player_Id IS NULL)")
                'End If
                'sql.Append(" ORDER BY Q_No,tblQuizQuestion.QQ_No,tblQuizAnswer.QA_No,tblAnswer.Answer_No; ")
            Case EnumQsetType.Sort
                ' SORT / เรียงลำดับบบบบบบบบบบบบบบบบบบ
                sql.Append(" SELECT *,CAST(tblAnswer.Answer_Name AS VARCHAR) AS AnswerCorrect  FROM tblQuizQuestion ")
                sql.Append(" LEFT JOIN tblQuizAnswer on tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizAnswer.Player_Id = '")
                sql.Append(Student_ID)
                sql.Append("' LEFT JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id ")
                sql.Append(" LEFT JOIN tblAnswer ON tblQuizQuestion.Question_Id = tblAnswer.Question_Id ")
                sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '")
                sql.Append(Quiz_ID)
                sql.Append("' AND (tblQuizAnswer.Quiz_Id = '")
                sql.Append(Quiz_ID)
                sql.Append("' OR tblQuizAnswer.Quiz_Id IS NULL) ")
                sql.Append(" AND (tblQuestion.QSet_Id = '")
                sql.Append(Qset_ID)
                sql.Append("' OR tblQuestion.QSet_Id  IS NULL) ")
                sql.Append(" ORDER BY tblQuizAnswer.QA_No,AnswerCorrect DESC; ")
            Case EnumQsetType.TrueFalse
                ' TRUE / FALSE ถูกผิดดดดดดดดดดดดดดดดดดดดดดดดดดดดดดดดด
                Return GetDTTrueFalse(Quiz_ID, Student_ID, Qset_ID)
            Case EnumQsetType.Pair
                'sql.Append(" SELECT *,tblQuizAnswer.Question_Id AS AnswerStudent,tblAnswer.Question_Id AS AnswerCorrect, tblQuizScore.Answer_Id AS QuizScoreAnswerId  FROM tblQuizQuestion ")
                'sql.Append(" LEFT JOIN tblAnswer ON tblQuizQuestion.Question_Id = tblAnswer.Question_Id  ")
                'sql.Append(" LEFT JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id AND tblQuizAnswer.Player_Id = '")
                'sql.Append(Student_ID)
                'sql.Append("' LEFT JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id ")
                'sql.Append(" LEFT JOIN tblQuizScore ON tblQuizQuestion.Question_Id = tblQuizScore.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizScore.Quiz_Id ")
                'sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '")
                'sql.Append(Quiz_ID)
                'sql.Append("' AND (tblQuizAnswer.Quiz_Id = '")
                'sql.Append(Quiz_ID)
                'sql.Append("' OR tblQuizAnswer.Quiz_Id IS NULL) ")
                'sql.Append(" AND (tblQuestion.QSet_Id = '")
                'sql.Append(Qset_ID)
                'sql.Append("' OR tblQuestion.QSet_Id  IS NULL) ")
                'sql.Append(" AND (tblQuizScore.Student_Id = '")
                'sql.Append(Student_ID)
                'sql.Append(" ' OR tblQuizScore.Student_Id  IS NULL) ")
                'sql.Append(" ORDER BY tblQuizAnswer.QA_No; ")
                sql.Append(" SELECT tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id as AnswerStudent,tblAnswer.Answer_Name")
                sql.Append(" FROM tblQuizQuestion  INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id ")
                sql.Append(" AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id")
                sql.Append(" INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id")
                sql.Append(" INNER JOIN tblAnswer ON tblQuizAnswer.Answer_Id = tblAnswer.Answer_Id")
                sql.Append(" WHERE tblQuizQuestion.Quiz_Id = '")
                sql.Append(Quiz_ID)
                sql.Append("' AND tblQuizAnswer.Player_Id = '")
                sql.Append(Student_ID)
                sql.Append("' AND tblQuestion.QSet_Id = '")
                sql.Append(Qset_ID)
                sql.Append("' ORDER BY tblQuizAnswer.QA_No;")

        End Select

        GetQuestionAndAnswerInQuiz = db.getdata(sql.ToString())

    End Function

    ' get quiz mode (สลับคำถาม คำตอบ)
    Private Function GetQuizMode(ByVal Quiz_ID As String) As Integer
        Dim sql As String = " SELECT TOP 1 IsDifferentQuestion,IsDifferentAnswer FROM tblQuiz WHERE Quiz_Id = '" & Quiz_ID & "'; "
        Dim dt As DataTable = db.getdata(sql)
        Dim IsDifferentQuestion As Boolean = dt.Rows(0)("IsDifferentQuestion")
        Dim IsDifferentAnswer As Boolean = dt.Rows(0)("IsDifferentAnswer")
        Dim mode As Integer
        If IsDifferentQuestion = False And IsDifferentAnswer = False Then ' 0,0
            mode = EnumQuizMode.NotDifferent
        ElseIf IsDifferentQuestion = False And IsDifferentAnswer = True Then ' 0,1
            mode = EnumQuizMode.DifferentAnswer
        ElseIf IsDifferentQuestion = True And IsDifferentAnswer = False Then ' 1,0
            mode = EnumQuizMode.DifferentQuestion
        ElseIf IsDifferentQuestion = True And IsDifferentAnswer = True Then ' 1,1
            mode = EnumQuizMode.DifferentBoth
        End If
        GetQuizMode = mode
    End Function


    ' get qset ทั้งหมดที่อยู่ใน quiz
    Private Function GetQsetInQuiz() As DataTable
        ' todo แก้ quizid ,studentid
        Dim sql As New StringBuilder()
        'sql.Append(" SELECT MIN(tblQuizScore.QQ_No), tblQuestionSet.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type FROM tblQuizScore ")
        'sql.Append(" INNER JOIN tblQuestion ON tblQuizScore.Question_Id = tblQuestion.Question_Id ")
        'sql.Append(" INNER JOIN tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id ")
        'sql.Append(" WHERE tblquizscore.Quiz_Id = '" & Quiz_ID & "' AND tblQuizScore.Student_Id = '" & Student_ID & "' ")
        'sql.Append(" GROUP BY tblQuestionSet.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type ")
        'sql.Append(" ORDER BY MIN(tblQuizScore.QQ_No); ")
        'sql.Append(" SELECT MIN(CASE WHEN tblQuizScore.QQ_No IS NULL THEN tblQuizQuestion.QQ_No ELSE tblQuizScore.QQ_No END) AS Q_No,tblQuestionSet.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type ")
        'sql.Append(" FROM tblQuizQuestion LEFT JOIN tblQuizScore ON tblQuizQuestion.Question_Id = tblQuizScore.Question_Id AND tblQuizQuestion.Quiz_Id = tblQuizScore.Quiz_Id ")
        'sql.Append(" INNER JOIN tblQuestion ON tblQuizQuestion.Question_Id = tblQuestion.Question_Id ")
        'sql.Append(" INNER JOIN tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id ")
        'sql.Append(" WHERE (tblQuizScore.Quiz_Id = '")
        'sql.Append(Quiz_ID)
        'sql.Append("' OR tblQuizScore.Quiz_Id IS NULL)")
        'sql.Append(" AND (tblQuizScore.Student_Id = '")
        'sql.Append(Student_ID)
        'sql.Append("' OR tblQuizScore.Student_Id IS NULL) ")
        'sql.Append(" AND tblQuizQuestion.Quiz_Id = '")
        'sql.Append(Quiz_ID)
        'sql.Append("' GROUP BY tblQuestionSet.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type ")
        'sql.Append(" ORDER BY Q_No; ")

        '24/11/2015

        sql.Append("select MIN(CASE WHEN t.QQ_No IS NULL THEN tblQuizQuestion.QQ_No ELSE t.QQ_No END) AS Q_No")
        sql.Append(" ,tblQuestionSet.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type ")
        sql.Append(" from tblquizquestion left join (select Question_Id,QQ_No from tblQuizScore where Student_Id = '")
        sql.Append(Student_ID)
        sql.Append("' and Quiz_Id = '")
        sql.Append(Quiz_ID)
        sql.Append("') as t on tblQuizQuestion.Question_Id = t.Question_Id inner join tblquestion on tblquizquestion.Question_Id = tblquestion.Question_Id")
        sql.Append(" inner join tblQuestionset on tblquestion.QSet_Id = tblQuestionset.QSet_Id where tblQuizQuestion.Quiz_Id = '")
        sql.Append(Quiz_ID)
        sql.Append("' group by tblQuestionset.QSet_Id,tblQuestionSet.QSet_Name,tblQuestionSet.QSet_Type order by Q_No")



        GetQsetInQuiz = db.getdata(sql.ToString())
    End Function

    Private Function GetDTTrueFalse(Quiz_ID As String, Student_ID As String, Qset_ID As String) As DataTable
        Dim dtcomplete As New DataTable()
        Dim dtTotalOfQuestionInQuiz, dtQuestionStudentAnswered, dtQuestionStudentDidNotAnswered, dttmp1, dttmp2 As New DataTable
        Dim arrQQNotmp As New ArrayList()
        AddColumnToDT(dtQuestionStudentAnswered)
        AddColumnToDT(dtQuestionStudentDidNotAnswered)
        Dim counter As Integer = 0
        'หาจำนวนข้อทั้งหมดขึ้นมาก่อน
        'Dim sql As String = " SELECT Question_Id,QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & Quiz_ID & "' AND IsActive = 1 ORDER BY QQ_No "
        Dim sql As String = " SELECT tblquizquestion.Question_Id,QQ_No FROM dbo.tblQuizQuestion inner join tblquestion on tblQuizQuestion.Question_Id = tblQuestion.Question_Id " & _
                            " WHERE Quiz_Id = '" & Quiz_ID & "' AND tblquizquestion.IsActive = 1 and tblQuestion.QSet_Id = '" & Qset_ID & "' ORDER BY QQ_No "
        dtTotalOfQuestionInQuiz = db.getdata(Sql)
        If dtTotalOfQuestionInQuiz.Rows.Count > 0 Then
            Sql = " SELECT dbo.tblQuizQuestion.QQ_No AS Q_No ,dbo.tblQuizQuestion.Question_Id,Question_Name " & _
                  " ,dbo.tblAnswer.Answer_Name,Answer_Score,dbo.tblQuizScore.Answer_Id AS AnswerStudent, " & _
                  " (SELECT Answer_Id FROM dbo.tblAnswer a2 WHERE a2.Question_Id = dbo.tblQuestion.Question_Id AND a2.Answer_Score = 1) AS AnswerCorrect " & _
                  " FROM dbo.tblQuizQuestion INNER JOIN dbo.tblQuizScore ON dbo.tblQuizQuestion.Quiz_Id = dbo.tblQuizScore.Quiz_Id " & _
                  " AND dbo.tblQuizQuestion.Question_Id = dbo.tblQuizScore.Question_Id " & _
                  " INNER JOIN dbo.tblQuestion ON dbo.tblQuizScore.Question_Id = dbo.tblQuestion.Question_Id " & _
                  " INNER JOIN dbo.tblAnswer ON dbo.tblQuestion.Question_Id = dbo.tblAnswer.Question_Id AND dbo.tblQuizScore.Answer_Id = dbo.tblAnswer.Answer_Id " & _
                  " WHERE dbo.tblQuizQuestion.Quiz_Id = '" & Quiz_ID & "' AND dbo.tblQuizScore.Student_Id = '" & Student_ID & "'" & _
                  " AND dbo.tblQuizQuestion.IsActive = 1  and tblQuestion.QSet_Id = '" & Qset_ID & "'  ORDER BY dbo.tblQuizQuestion.QQ_No; "
            dttmp1 = db.getdata(Sql)
            'หาข้อที่นักเรียนตอบทั้งหมดเก็บใส่ dtQuestionStudentAnswered
            'If dttmp1.Rows.Count > 0 Then
            For index = 0 To dttmp1.Rows.Count - 1
                Dim dr As DataRow = dtQuestionStudentAnswered.NewRow()
                dr("Q_No") = dttmp1.Rows(index)("Q_No")
                dr("Question_Id") = dttmp1.Rows(index)("Question_Id")
                dr("Question_Name") = dttmp1.Rows(index)("Question_Name")
                dr("Answer_Name") = dttmp1.Rows(index)("Answer_Name")
                dr("Answer_Score") = dttmp1.Rows(index)("Answer_Score")
                dr("AnswerStudent") = dttmp1.Rows(index)("AnswerStudent")
                dr("AnswerCorrect") = dttmp1.Rows(index)("AnswerCorrect")
                dtQuestionStudentAnswered.Rows.Add(dr)
                arrQQNotmp.Add(dttmp1.Rows(index)("Q_No"))
                'counter += 1
            Next
            'หาข้อมูลของข้อที่เหลือเก็บใส่ dtQuestionStudentDidNotAnswered ไว้ก่อน
            For index = 0 To dtTotalOfQuestionInQuiz.Rows.Count - 1
                If arrQQNotmp.Contains(dtTotalOfQuestionInQuiz.Rows(index)("QQ_No")) = False Then
                    sql = " SELECT " & dtTotalOfQuestionInQuiz.Rows(index)("QQ_No") & " AS Q_No, dbo.tblAnswer.Question_Id,Question_Name,NULL Answer_Name,0 as Answer_Score,NULL AS AnswerStudent , Answer_Name as AnswerCorrect " & _
                          " FROM dbo.tblQuestion INNER JOIN dbo.tblAnswer ON dbo.tblQuestion.Question_Id = dbo.tblAnswer.Question_Id " & _
                          " WHERE dbo.tblQuestion.IsActive = 1 And dbo.tblAnswer.IsActive = 1 AND dbo.tblQuestion.Question_Id = '" & dtTotalOfQuestionInQuiz.Rows(index)("Question_Id").ToString() & "' " & _
                          " AND Answer_Score = 1; "
                    dttmp2.Clear()
                    dttmp2 = db.getdata(sql)
                    If dttmp2.Rows.Count > 0 Then
                        Dim dr As DataRow = dtQuestionStudentDidNotAnswered.NewRow()
                        dr("Q_No") = dttmp2.Rows(0)("Q_No")
                        dr("Question_Id") = dttmp2.Rows(0)("Question_Id")
                        dr("Question_Name") = dttmp2.Rows(0)("Question_Name")
                        dr("Answer_Name") = dttmp2.Rows(0)("Answer_Name")
                        dr("Answer_Score") = dttmp2.Rows(0)("Answer_Score")
                        dr("AnswerStudent") = dttmp2.Rows(0)("AnswerStudent")
                        dr("AnswerCorrect") = dttmp2.Rows(0)("AnswerCorrect")
                        dtQuestionStudentDidNotAnswered.Rows.Add(dr)
                    End If
                End If
            Next
            'เมื่อเสร็จแล้วก็ต้องเอาข้อมูลของ dt ที่นักเรียนตอบ และ ไม่ได้ตอบ มาใส่ใน dtcomplete และ sort ลำดับข้อให้ถูกต้อง
            dtcomplete = GetCompleteDTByDTAnsweredAndNotAnswered(dtQuestionStudentAnswered, dtQuestionStudentDidNotAnswered)

            'Else
            '    Return dtcomplete
            'End If
        Else
            Return dtcomplete
        End If
        Return dtcomplete
    End Function

    Private Sub AddColumnToDT(ByRef dt As DataTable)
        dt.Columns.Add("Q_No", GetType(Integer))
        dt.Columns.Add("Question_Id")
        dt.Columns.Add("Question_Name")
        dt.Columns.Add("Answer_Name")
        dt.Columns.Add("Answer_Score")
        dt.Columns.Add("AnswerStudent")
        dt.Columns.Add("AnswerCorrect")
    End Sub

    Private Function GetCompleteDTByDTAnsweredAndNotAnswered(dtQuestionStudentAnswered As DataTable, dtQuestionStudentDidNotAnswered As DataTable) As DataTable
        Dim dtComplete As New DataTable()
        'clone
        dtComplete = dtQuestionStudentAnswered.Copy()
        dtComplete.Merge(dtQuestionStudentDidNotAnswered)
        'sort
        Dim dv As New DataView(dtComplete)
        dv.Sort = "Q_No"
        dtComplete = dv.ToTable()
        'update data
        For index = 0 To dtComplete.Rows.Count - 1
            If dtComplete(index)("AnswerStudent") Is DBNull.Value Then
                dtComplete(index)("Q_No") = "999"
            End If
        Next
        Return dtComplete
    End Function

#End Region

#Region "Type1"
    Private Qset_ID As String
    Private ExamNum As Integer
    Dim htmlQset As New StringBuilder()
    Dim PrefixAnswer() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ", "ฒ", "ณ", "ด", "ต"}
    Dim arrResult As Array = {"", "<img src='../Images/right.png' alt='' class='Result' />", "<img src='../Images/wrong.png' alt='' class='Result' />", "<img src='../Images/Alert.png' alt='' class='Result' />"}
    ' Type 1
    Private Function HtmlType1(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        Dim Question As String
        Dim i As Integer
        Dim arrCorrect As Array = {"", ""}
        Dim IsRight As String = ""
        For Each row In dtQuestion.Rows
            If Question Is Nothing Then
                Question = row("Question_Id").ToString()
                i = 0
                htmlQset.Append("<div class='divExamType1 divQuestion' id='" + Question + "' qsetId='" + Qset_ID + "' qsetFilePath='" + Qset_ID.ToFolderFilePath() + "' style='margin-top: 10px;position:relative;'><table><tr><td style='vertical-align:top;border:initial;'><qq>")
                htmlQset.Append(ExamNum)
                htmlQset.Append(".</qq></td>")
                htmlQset.Append("<td style='text-align:justify;border:initial;'><qq>")
                htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
                htmlQset.Append("</qq></td></table>")
                htmlQset.Append("<div>")
                htmlQset.Append("<table style='border-spacing: 0px;'>")
                ExamNum = ExamNum + 1
            ElseIf Not (Question = row("Question_Id").ToString()) Then
                Question = row("Question_Id").ToString()
                i = 0
                htmlQset.Append("</table>")
                htmlQset.Append("</div></div>")
                htmlQset.Append("<div class='divExamType1' id='" + Question + "' qsetId='" + Qset_ID + "' style='margin-top: 10px;position:relative;'><table><tr><td style='vertical-align:top;border:initial;'><qq>")
                htmlQset.Append(ExamNum)
                htmlQset.Append(".</qq></td>")
                htmlQset.Append("<td style='text-align:justify;border:initial;'><qq>")
                htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
                htmlQset.Append("</qq></td></table>")
                htmlQset.Append("<div>")
                htmlQset.Append("<table style='border-spacing: 0px;'>")
                ExamNum = ExamNum + 1
            End If
            'If row.Table.columns.contains("AnswerStudent") Then
            '    If Not IsDBNull(row("AnswerStudent")) Then
            If row("Answer_Score") Then
                arrCorrect(0) = "<span class='C'>"
                arrCorrect(1) = "</span>"
                IsRight = "style='background-color:#2CA505;color:white;position:relative;padding-right: 15px;'"
            End If
            '    End If
            'End If


            'htmlQset.Append(arrCorrect(0).ToString())
            'htmlQset.Append(PrefixAnswer(i))
            'htmlQset.Append(". ")
            'htmlQset.Append(row("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            'htmlQset.Append(arrCorrect(1).ToString())
            'htmlQset.Append("<br />")

            htmlQset.Append("<tr><td " & IsRight & ">" & PrefixAnswer(i) & ".</td><td " & IsRight & ">" & row("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)) & "</td></tr>")


            i = i + 1
            arrCorrect(0) = ""
            arrCorrect(1) = ""
            IsRight = ""
        Next
        htmlQset.Append("</table>")
        htmlQset.Append("</div></div>")
        HtmlType1 = htmlQset.ToString()
    End Function
    ' Type 1
    Private Function HtmlType1Check(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        Dim Question As String
        Dim i As Integer
        Dim arrCorrect As Array = {"", ""}

        Dim r As Integer = 3


        For Each row In dtQuestion.Rows
            If Question Is Nothing Then
                Question = row("Question_Id").ToString()
                i = 0
                htmlQset.Append("<div style='margin-top: 10px;'><table><tr><td style='vertical-align:top;border:initial;'><div style='text-align:center;'><qq>")
                htmlQset.Append(ExamNum)
                'htmlQset.Append(".</qq><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></div></td>")
                htmlQset.Append(".</qq></div></td>")
                htmlQset.Append("<td style='text-align:justify;border:initial;vertical-align:top;'><qq>")
                htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
                htmlQset.Append("</qq></td></table>")
                htmlQset.Append("<div>")
                htmlQset.Append("<table style='border-spacing: 0px;'>")
                ExamNum = ExamNum + 1
            ElseIf Not (Question = row("Question_Id").ToString()) Then
                Question = row("Question_Id").ToString()
                i = 0
                'htmlQset.Append(arrResult(r))
                r = 3
                htmlQset.Append("</table>")
                htmlQset.Append("</div></div>")
                htmlQset.Append("<div style='margin-top: 10px;'><table><tr><td style='vertical-align:top;border:initial;'><div style='text-align:center;'><qq>")
                htmlQset.Append(ExamNum)
                'htmlQset.Append(".</qq><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></div></td>")
                htmlQset.Append(".</qq></div></td>")
                htmlQset.Append("<td style='text-align:justify;border:initial;vertical-align:top;'><qq>")
                htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
                htmlQset.Append("</qq></td></table>")
                htmlQset.Append("<div>")
                htmlQset.Append("<table style='border-spacing: 0px;'>")
                ExamNum = ExamNum + 1
            End If
            If Not IsDBNull(row("AnswerStudent")) Then
                If row("AnswerStudent") = row("AnswerCorrect") Then
                    arrCorrect(0) = "<span class='C'>"
                    arrCorrect(1) = "</span>"
                    If row("Answer_Score") = 1 Then
                        r = 1
                    Else
                        r = 2
                    End If
                End If
            End If

            'htmlQset.Append(arrCorrect(0).ToString())
            'htmlQset.Append(PrefixAnswer(i))
            'htmlQset.Append(". ")
            'htmlQset.Append(row("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            'htmlQset.Append(arrCorrect(1).ToString())
            'htmlQset.Append("<br />")

            Dim ChooseChoice As String = ""
            Dim IsRight As String = ""
            If Not IsDBNull(row("AnswerStudent")) Then
                If row("AnswerStudent") = row("AnswerCorrect") Then
                    ChooseChoice = "<img src='../Images/Activity/ChooseCircle_pad.png' style='display:block !important;position:absolute;left:-20px;' />"
                    If row("Answer_Score") = 1 Then
                        'ใส่ วงกลม + สีเขียว
                        IsRight = "style='background-color:#2CA505;color:white;position:relative;padding-right: 15px;'"
                    Else
                        'ใส่ วงกลม + สีแดง
                        IsRight = "style='background-color:#FF0B00;color:white;position:relative;padding-right: 15px;'"
                    End If
                Else
                    If row("Answer_Score") = 1 Then
                        'ใส่ วงกลม + สีเขียว
                        IsRight = "style='background-color:#2CA505;color:white;position:relative;padding-right: 15px;'"
                    End If
                End If
            Else
                If Not IsDBNull(row("Answer_Score")) Then
                    If row("Answer_Score") = 1 Then
                        IsRight = "style='background-color:#2CA505;color:white;position:relative;padding-right: 15px;'"
                    End If
                End If
            End If

            htmlQset.Append("<tr><td " & IsRight & " >" & ChooseChoice & PrefixAnswer(i) & ".</td><td " & IsRight & " >" & row("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)) & "</td></tr>")

            i = i + 1
            arrCorrect(0) = ""
            arrCorrect(1) = ""
        Next
        'htmlQset.Append(arrResult(r))
        htmlQset.Append("</table>")
        htmlQset.Append("</div></div>")
        HtmlType1Check = htmlQset.ToString()
    End Function
#End Region

#Region "Type2"
    ' Type 2
    Private Function HtmlType2(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        htmlQset.Append("<div><table style='width:100%;'>")
        For Each row In dtQuestion.Rows
            htmlQset.Append("<tr class='divExamType2 divQuestion' id='" + row("Question_Id").ToString() + "' qsetId='" + Qset_ID + "' qsetFilePath='" + Qset_ID.ToFolderFilePath() + "'>")
            htmlQset.Append("<td style='text-align: center;'><qq>")
            htmlQset.Append(ExamNum)
            htmlQset.Append(".</qq></td>")
            If row("Answer_Name").ToString() = "True" Or row("Answer_Name").ToString() = "ถูก" Then
                If row("Answer_Score") = 1 Then
                    htmlQset.Append("<td><qq><span class='T'>ถูก</span></qq></td><td><qq><span class='F'>ผิด</span></qq></td>")
                Else
                    htmlQset.Append("<td><qq><span class='F'>ถูก</span></qq></td><td><qq><span class='T'>ผิด</span></qq></td>")
                End If
            ElseIf row("Answer_Name").ToString() = "False" Or row("Answer_Name").ToString() = "ผิด" Then
                If row("Answer_Score") = 1 Then
                    htmlQset.Append("<td><qq><span class='F'>ถูก</span></qq></td><td><qq><span class='T'>ผิด</span></qq></td>")
                Else
                    htmlQset.Append("<td><qq><span class='T'>ถูก</span></qq></td><td><qq><span class='F'>ผิด</span></qq></td>")
                End If
            Else
                If row("AnswerCorrect") = "True" Then
                    htmlQset.Append("<td><qq><span class='T'>ถูก</span></qq></td><td><qq><span class='F'>ผิด</span></qq></td>")
                Else
                    htmlQset.Append("<td><qq><span class='F'>ถูก</span></qq></td><td><qq><span class='T'>ผิด</span></qq></td>")
                End If
            End If
            htmlQset.Append("<td style='padding-left: 20px;width:100%;'>")
            htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</td><tr>")
            ExamNum = ExamNum + 1
        Next
        htmlQset.Append("</table></div>")
        HtmlType2 = htmlQset.ToString()
    End Function
    ' Type 2 Check ว่าถูกหรือเปล่า เด็กตอบ
    Private Function HtmlType2Check(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        htmlQset.Append("<div><table>")
        Dim S As Integer = -1
        Dim imgChooseCircle As String = "<img src='../Images/Activity/ChooseCircle_pad.png' style='position:absolute;' />"
        Dim tmpAnswerNameCorrect As String = ""
        For Each row In dtQuestion.Rows

            htmlQset.Append("<tr>")
            'htmlQset.Append("<td>")
            If Not IsDBNull(row("AnswerStudent")) Then
                If row("AnswerStudent").ToString() = row("AnswerCorrect").ToString() AndAlso row("Answer_Score") = 1 Then
                    'htmlQset.Append(arrResult(1))
                    S = 1
                Else
                    'htmlQset.Append(arrResult(2))
                    S = 0
                End If
            Else
                'htmlQset.Append(arrResult(3))
                tmpAnswerNameCorrect = row("AnswerCorrect")
            End If

            'htmlQset.Append("</td>")
            htmlQset.Append("<td style='text-align:center;'><div><qq>")
            htmlQset.Append(ExamNum)
            'htmlQset.Append(".</qq><div><img src='../Images/dotdotdot.png' id='btnErrorSupport' style='width:30px;height:15px;cursor:pointer;' /></div></div></td>")
            htmlQset.Append(".</qq></div></td>")

            If row("Answer_Name").ToString() = "True" Then
                If S = 1 Then
                    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='S correct'>ถูก</span></qq></td><td><qq><span class='F'>ผิด</span></qq></td>")
                ElseIf S = 0 Then
                    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F wrong'>ถูก</span></qq></td><td><qq><span class='T correct'>ผิด</span></qq></td>")
                Else
                    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F'>ถูก</span></qq></td><td><qq><span class='F'>ผิด</span></qq></td>")
                End If
            ElseIf row("Answer_Name").ToString() = "False" Then
                'If S = 1 Then
                '    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='S'>ผิด</span></qq></td><td><qq><span class='F'>ถูก</span></qq></td>")
                'ElseIf S = 0 Then
                '    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F'>ผิด</span></qq></td><td><qq><span class='S'>ถูก</span></qq></td>")
                'Else
                '    htmlQset.Append("<td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F'>ผิด</span></qq></td><td><qq><span class='F'>ถูก</span></qq></td>")
                'End If
                If S = 1 Then
                    htmlQset.Append("<td><qq><span class='F'>ถูก</span></qq></td><td><qq style='position=relative;'>" & imgChooseCircle & "<span class='S correct'>ผิด</span></qq></td>")
                ElseIf S = 0 Then
                    htmlQset.Append("<td><qq><span class='S correct'>ถูก</span></qq></td><td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F wrong'>ผิด</span></qq></td>")
                Else
                    htmlQset.Append("<td><qq><span class='F'>ถูก</span></qq></td><td><qq style='position=relative;'>" & imgChooseCircle & "<span class='F'>ผิด</span></qq></td>")
                End If
            Else 'ถ้าไม่ได้ตอบ
                If tmpAnswerNameCorrect = "True" Then
                    htmlQset.Append("<td><qq><span class='F correct'>ถูก</span></qq></td><td><qq><span class='S'>ผิด</span></qq></td>")
                ElseIf tmpAnswerNameCorrect = "False" Then
                    htmlQset.Append("<td><qq><span class='S'>ถูก</span></qq></td><td><qq><span class='F correct'>ผิด</span></qq></td>")
                End If
            End If

            htmlQset.Append("<td style='padding-left: 20px;'>")
            htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</td><tr>")
            ExamNum = ExamNum + 1
            S = -1
        Next
        htmlQset.Append("</table></div>")
        HtmlType2Check = htmlQset.ToString()
    End Function
#End Region

#Region "Type3"
    ' Type 3 เฉลย
    Private Function HtmlType3()
        htmlQset.Clear()
        Dim dtQuestionCorrect As New DataTable
        dtQuestionCorrect = GetType3Correct()

        htmlQset.Append("<div><table style='width:100%;'>")
        For Each row In dtQuestionCorrect.Rows
            htmlQset.Append("<tr class='divExamType3 divQuestion' id='" + row("Question_Id").ToString() + "' qsetId='" + Qset_ID + "' qsetFilePath='" + Qset_ID.ToFolderFilePath() + "'><td style='width:45%;' >")
            htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</td><td style='width:10%;text-align:center;'><qq>คู่กับ</qq></td><td style='width:45%;'><span Class=""Type3Correct"">")
            htmlQset.Append(row("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</span></td></tr>")
        Next
        htmlQset.Append("</table></div>")
        HtmlType3 = htmlQset.ToString()
    End Function
    ' Type 3 Check เด็กตอบ
    Private Function HtmlType3Check(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        'ดึงเฉลยมาก่อน
        Dim dtQuestionCorrect As New DataTable
        dtQuestionCorrect = GetType3Correct()

        'วนเทียบกับเด็กตอบ
        htmlQset.Append("<div><table style='width:100%;'>")

        For i = 0 To dtQuestion.Rows.Count - 1
            htmlQset.Append("<tr><td style='width:45%;'>")
            htmlQset.Append(dtQuestionCorrect.Rows(i)("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</td><td style='width:10%;text-align:center;'><qq>คู่กับ</qq></td><td style='width:45%;'>")

            If dtQuestion.Rows(i)("AnswerStudent") = dtQuestionCorrect.Rows(i)("AnswerCorrect") Then
                htmlQset.Append("<span Class=""Type3Correct"">")
            Else
                htmlQset.Append("<span Class=""Type3Wrong"">")
            End If
                htmlQset.Append(dtQuestion.Rows(i)("Answer_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</span></td></tr>")
        Next
        htmlQset.Append("</table></div>")
        HtmlType3Check = htmlQset.ToString()
    End Function

    Private Function GetType3Correct() As DataTable
        'เฉลย
        Dim sql As New StringBuilder()

        If Quiz_ID Is Nothing Then
            sql.Append(" SELECT  tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id as AnswerCorrect,tblAnswer.Answer_Name")
            sql.Append(" FROM  tblQuestion inner join  tblAnswer ON tblquestion.Question_Id = tblAnswer.Question_Id ")
            sql.Append("WHERE tblQuestion.QSet_Id = '" & Qset_ID & "' ORDER BY tblQuestion.Question_No;")
        Else
            sql.Append(" SELECT tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id as AnswerCorrect,tblAnswer.Answer_Name")
            sql.Append(" FROM tblQuizQuestion  INNER JOIN tblQuizAnswer ON tblQuizQuestion.Question_Id = tblQuizAnswer.Question_Id")
            sql.Append(" AND tblQuizQuestion.Quiz_Id = tblQuizAnswer.Quiz_Id")
            sql.Append(" INNER JOIN tblQuestion ON tblQuizAnswer.Question_Id = tblQuestion.Question_Id")
            sql.Append(" INNER JOIN tblAnswer ON tblQuizAnswer.Question_Id = tblAnswer.Question_Id")
            sql.Append(" WHERE tblQuestion.QSet_Id = '")
            sql.Append(Qset_ID)
            sql.Append("' AND tblQuizAnswer.Player_Id = '")
            sql.Append(Student_ID)
            sql.Append("' AND tblQuizQuestion.Quiz_Id = '")
            sql.Append(Quiz_ID)
            sql.Append("' ORDER BY tblQuizAnswer.QA_No;")
        End If



        Dim dt As New DataTable
        dt = db.getdata(sql.ToString())

        If dt.Rows.Count() = 0 Then
            'case ไม่ได้กดเข้าทำ ทำให้ไม่มี QuizQuestion
            sql.Clear()
            sql.Append("select tblQuestion.Question_Id,tblQuestion.Question_Name,tblAnswer.Answer_Id as AnswerCorrect,tblAnswer.Answer_Name")
            sql.Append(" from tblquestion inner join tblAnswer on tblquestion.Question_Id = tblanswer.Question_Id")
            sql.Append(" inner join tblTestSetQuestionDetail on tblQuestion.Question_Id = tblTestSetQuestionDetail.Question_Id")
            sql.Append(" inner join tblTestSetQuestionSet on tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_Id ")
            sql.Append(" inner join tblquiz on tblTestSetQuestionSet.TestSet_Id = tblquiz.TestSet_Id")
            sql.Append(" where Quiz_Id = '")
            sql.Append(Quiz_ID)
            sql.Append("' and tblTestSetQuestionSet.QSet_Id = '")
            sql.Append(Qset_ID)
            sql.Append("' order by tblTestSetQuestionDetail.TSQD_No;")
            dt = db.getdata(sql.ToString())
        End If

        Return dt
    End Function

#End Region

#Region "Type6"
    ' Type 6
    Private Function HtmlType6(ByVal dtQuestion As DataTable, Optional ByVal IsQuizCorrect As Boolean = False)
        htmlQset.Clear()

        If IsQuizCorrect Then
            Dim v As New DataView(dtQuestion)
            v.Sort = "AnswerCorrect ASC"
            dtQuestion = v.ToTable()
        End If

        Dim i As Integer = 1
        htmlQset.Append("<div>")
        For Each row In dtQuestion.Rows
            htmlQset.Append("<div class='divExamType6 divQuestion' id='" + row("Question_Id").ToString() + "' qsetId='" + Qset_ID + "' style='padding:0;' qsetFilePath='" + Qset_ID.ToFolderFilePath() + "'>")
            htmlQset.Append("<qq>")
            htmlQset.Append("ลำดับที่ ")
            htmlQset.Append(i)
            htmlQset.Append("</qq><span>")
            htmlQset.Append("   ")
            htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("</span></div>")
            'htmlQset.Append("<br />")
            i = i + 1
        Next
        htmlQset.Append("</div>")
        HtmlType6 = htmlQset.ToString()
    End Function
    ' Type 6 แบบ check ว่าถูกหรือเปล่า
    Private Function HtmlType6Check(ByVal dtQuestion As DataTable)
        htmlQset.Clear()

        Dim dtQuestionCorrect As DataTable
        Dim v As New DataView(dtQuestion)
        v.Sort = "AnswerCorrect ASC"
        dtQuestionCorrect = v.ToTable()

        Dim i As Integer = 1
        htmlQset.Append("<div>")
        For Each row In dtQuestion.Rows
            If row("Question_Id").ToString() = dtQuestionCorrect(i - 1)("Question_Id").ToString() Then
                htmlQset.Append(arrResult(1))
            Else
                htmlQset.Append(arrResult(2))
            End If
            htmlQset.Append("<qq>")
            htmlQset.Append("ลำดับที่ ")
            htmlQset.Append(i)
            htmlQset.Append("</qq>")
            htmlQset.Append("   ")
            htmlQset.Append(row("Question_Name").ToString().Replace("___MODULE_URL___", Cls.GenFilePath(Qset_ID)))
            htmlQset.Append("<br />")
            i = i + 1
        Next
        htmlQset.Append("</div>")
        HtmlType6Check = htmlQset.ToString()
    End Function
#End Region

End Class

