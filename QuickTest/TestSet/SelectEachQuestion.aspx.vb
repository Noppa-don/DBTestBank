Imports System.Web.Services
Imports System.Web

Public Class SelectEachQuestion
    Inherits System.Web.UI.Page

    Private Index As Integer = 0
    Public sessUserId As String
    Public TestSetId As String
    Public qSetID As String
    Public classId As String
    Dim objTestSet As ClsTestSet
    Dim IsUpdate As Boolean
    Dim cls As New ClsPDF(New ClassConnectSql)
    Public VBQsetId As String
    Private _ChoiceMath As Integer
    Protected IsAndroid As Boolean

    Protected ToTalQuestionInQset As Integer
    Protected IsUseFullQset As Boolean = False
    Protected EditTestSetWarningText As String = CommonTexts.EditTestSetWarningText

    Protected qsetFilePath As String

    Public Property ChoiceMath() As Integer
        Get
            Return _ChoiceMath
        End Get
        Set(ByVal value As Integer)
            _ChoiceMath = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If
        End If
#If DEBUG Then
        'Session("userid") = "2"
#End If
        sessUserId = Session("UserId").ToString
        TestSetId = Session("newTestSetId")
        qSetID = HttpContext.Current.Request.QueryString("qSetId")
        classId = Request.QueryString("classId")
        VBQsetId = qSetID

        qsetFilePath = qSetID.ToFolderFilePath()

        If Session("UserId").ToString = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else

            If IsNothing(Session("objTestSet")) Then
                objTestSet = New ClsTestSet(Session("userid").ToString)
                Session("objTestSet") = objTestSet
            Else
                objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
            End If


            If Not Page.IsPostBack() Then

                CreateSessionTotalEvaluationIndex()
                Dim qSetId As String = Request.QueryString("qSetId")

                Dim IsSelectByEvalutionIndex As Boolean = HttpContext.Current.Application("IsSelectByEvalutionIndex")
                Dim ds As DataSet = objTestSet.GetQuestionSet(qSetId, HttpContext.Current.Application("NeedJoinQ40"), IsSelectByEvalutionIndex, TestSetId)
                If Not IsNothing(ds.Tables(0)) Then


                    For Each i In ds.Tables(0).Rows

                        If i("QSet_Type") = 2 Then
                            Dim dt As DataTable = objTestSet.GetAnswersTrueFalse(i("Question_Id").ToString).Tables(0)
                            If dt.Rows(0)("answer_name") = "True" Then
                                i("Question_Name") = "<u> &nbsp;/&nbsp; </u> " & "&nbsp;&nbsp;" & i("Question_Name").ToString
                            Else
                                i("Question_Name") = "<u> &nbsp;X&nbsp; </u> " & "&nbsp;&nbsp;" & i("Question_Name").ToString
                            End If
                            i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(Request.QueryString("qSetId")))


                        End If

                        If i("QSet_Type") = 3 Or i("QSet_Type") = 6 Then
                            Dim dt As DataTable = objTestSet.GetAnswers(i("Question_Id").ToString).Tables(0)
                            i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(Request.QueryString("qSetId")))
                            i("Question_Name") = "<td style='border-top:0px; width:360px;'>" & i("Question_Name") & "</td>"
                        End If

                        If i("QSet_Type") = 1 Then
                            Dim dt As DataTable = objTestSet.GetAnswers(i("Question_Id").ToString).Tables(0)
                            i("Question_Name") = i("Question_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(Request.QueryString("qSetId"))).Replace("<tbody>", "<tbody style='display:-webkit-inline-box'>").Replace("<table style=""FLOAT: left;"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">").Replace("<table style=""FLOAT: left"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">").Replace("<table cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"" style=""float: left;"">", "<table cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"" style=""display:inline"">").Replace("<div style=""FLOAT: left; MARGIN: 15px 5px 0px"">", "<div style=""display:inline"">").Replace("<div style=""FLOAT: left; margin: 15px 5px 0px"">", "<div style=""display:inline"">")
                            i("Question_Name") = "<td colspan='3' style='border-top:0px;'>" & i("Question_Name") & "</td>"
                        End If




                    Next
                    ListingQuestionsInModule.DataSource = ds
                    ListingQuestionsInModule.DataBind()
                    'lblTotalQuestion.Text = ds.Tables(0).Rows.Count
                    'lblTotalQuestion2.Text = lblTotalQuestion.Text

                    ToTalQuestionInQset = ds.Tables(0).Rows.Count


                    Dim qsetType As Int16 = ds.Tables(0).Rows(0)("Qset_Type")
                    If qsetType = EnumQsetType.Sort OrElse qsetType = EnumQsetType.Pair Then
                        IsUseFullQset = True
                    Else
                        IsUseFullQset = If(ds.Tables(0).Rows(0)("IsUseFullQset") Is DBNull.Value, False, True)
                    End If



                End If
                ' Select ข้อที่เลือกไปแล้วขึ้นมา

                If Session("EditID") = "" Then
                    IsUpdate = False
                Else
                    IsUpdate = True
                End If
            End If

            _ChoiceMath = 0
        End If
    End Sub

    Public Function GetQuestionNo(IsNewNo As Boolean) As String
        If IsNewNo Then
            Index += 1
        End If
        Return Index
    End Function

    'Public Function GetQuestionDetail(ByVal questionId As String) As String
    '    Return objTestSet.GetQuestionDetail(questionId)
    'End Function

    Public Function GetAnswerLists(ByVal questionId As String, ByVal questionSetType As String) As String
        If questionSetType = 2 Then 'แบบถูกผิด
            Return " "
        End If


        If questionSetType = 3 Or questionSetType = 6 Then ' แบบจับคู่ หรือ เรียงลำดับ
            Dim dat As DataTable = objTestSet.GetAnswers(questionId).Tables(0)
            For Each i In dat.Rows
                i("answer_Name") = i("answer_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(Request.QueryString("qSetId")))
            Next
            Dim sb1 As New System.Text.StringBuilder
            ' Dim Choice1() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ณ", "ญ", "ด", "ต", "ถ", "ท", "ธ", "น", "บ", "ป", "ผ", "ฝ", "พ"}
            For i = 0 To dat.Rows.Count - 1
                If questionSetType = 3 Then
                    sb1.Append("<td style= width:60px;border-left:0px;border-top:0px;><B>คู่กับ</B></td>")
                Else
                    sb1.Append("<td style= width:90px;border-left:0px;border-top:0px;><B>เป็นลำดับที่</B></td>")
                End If
                sb1.Append("<td style=border-left:0px;border-top:0px;'>" & dat.Rows(i)("answer_name").ToString() & "</td>")

                'sb1.Append("<td style='width:40px;border-left:0px;border-top:0px;''>" & dat.Rows(i)("answer_name").ToString() & "</td>")

                _ChoiceMath = _ChoiceMath + 1
            Next
            GetAnswerLists = sb1.ToString()
        End If


        If questionSetType = 1 Then ' แบบ Choice
            Dim dt As DataTable = objTestSet.GetAnswers(questionId).Tables(0)
            For Each i In dt.Rows
                i("answer_Name") = i("answer_Name").ToString.Replace("___MODULE_URL___", cls.GenFilePath(Request.QueryString("qSetId"))).Replace("<tbody>", "<tbody style='display:-webkit-inline-box'>").Replace("<table style=""FLOAT: left;"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">").Replace("<table style=""FLOAT: left"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">", "<table style=""display:inline"" cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"">").Replace("<table cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"" style=""float: left;"">", "<table cellspacing=""0"" cellpadding=""3"" taglabel=""FRACTION"" style=""display:inline"">").Replace("<div style=""FLOAT: left; MARGIN: 15px 5px 0px"">", "<div style=""display:inline"">").Replace("<div style=""FLOAT: left; margin: 15px 5px 0px"">", "<div style=""display:inline"">")
            Next
            Dim sb As New System.Text.StringBuilder
            Dim Choice() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ณ", "ญ", "ด", "ต", "ถ", "ท", "ธ", "น", "บ", "ป", "ผ", "ฝ", "พ"}
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("answer_score").ToString() = "1" Then
                    sb.Append("</tr><tr><td style='width:40px;padding-bottom:0px;padding-top:9px;border-left:0px;border-top:0px;'><img src='../images/checked.gif' /></td><td style='width:40px;padding-bottom:3px;padding-top:3px;border-left:0px;border-top:0px;'>" & Choice(i) & "</td>")
                Else
                    sb.Append("</tr><tr><td style='width:40px;padding-bottom:3px;padding-top:3px;border-left:0px;border-top:0px;'>&nbsp;</td><td style='width:40px;padding-bottom: 3px;padding-top: 3px;border-left:0px;border-top:0px;'>" & Choice(i) & "</td>")
                End If
                sb.Append("<td class='ForTestJa' style='border-left:0px;border-top:0px;padding-bottom:3px;padding-top:3px;'>" & dt.Rows(i)("answer_name").ToString() & "</td></tr>")
            Next
            GetAnswerLists = sb.ToString()

        End If

    End Function

    <Services.WebMethod()>
    Public Shared Function OnSaveCodeBehide(ByVal questionId As String, ByVal needRemove As String, ByVal qSetId As String, ByVal testSetId As String, ByVal userId As String, ByVal classId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim retVal As String = ""
        Dim objTestSet As New ClsTestSet(userId)
        Dim isedit As String
        If HttpContext.Current.Session("EditId").ToString = "" Then
            isedit = "0"
        Else
            isedit = "1"
        End If

        If needRemove = "true" Then
            retVal = objTestSet.SaveSelectedQuestion(isedit, questionId, True, qSetId, testSetId, classId)
            'กรณีลบทีละข้อ, ต้องไปดูว่ามี record ใน tblTestSetQuestionSet ที่ไม่ได้ใช้ค้างอยู่หรือไม่, ถ้ามีก็ลบทิ้ง
            objTestSet.DeleteUnusedTestSetQuestionSet()
        Else
            retVal = objTestSet.SaveSelectedQuestion(isedit, questionId, False, qSetId, testSetId, classId)
        End If

        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Dim db As New ClassConnectSql()
            Dim sql As String = " SELECT QSet_Type FROM tblQuestionSet WHERE QSet_Id = '" & qSetId & "' ;"
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

    <Services.WebMethod()>
    Public Shared Function SaveLog(ByVal Type As String, QuestionId As String) As String
        If Type = "Word" Then
            Log.Record(Log.LogType.ManageExam, "ไปหน้าจอแก้ไขข้อสอบฝั่ง Word (QuestionId=" & QuestionId & ")", True)
        Else
            Log.Record(Log.LogType.ManageExam, "ไปหน้าจอแก้ไขข้อสอบฝั่ง Quiz (QuestionId=" & QuestionId & ")", True)
        End If

    End Function

    <Services.WebMethod()>
    Public Shared Function delAllCodeBehide(ByVal QsetId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim retVal As String = ""
        Dim objTestSet As New ClsTestSet(HttpContext.Current.Session("UserId").ToString)
        'Dim qSetId As String = HttpContext.Current.Request.QueryString("qSetId").ToString()
        Dim TestsetId As String = HttpContext.Current.Session("newTestSetId").ToString()
        objTestSet.DeleteAllWhenReChkbox(QsetId, TestsetId)
        delAllCodeBehide = retVal
    End Function

    Public Function GetChecked(ByVal QuestionId As String) As String
        Dim QSId As String = Request.QueryString("qSetId")
        Dim Check As String
        'Dim QSId As String = Request.QueryString("qSetId")
        If Session("EditID") = "" Then
            Check = objTestSet.GetSelectedQuestionID(Session("newTestSetId").ToString, QSId, QuestionId)
        Else
            Check = objTestSet.GetSelectedQuestionID(Session("EditID").ToString, QSId, QuestionId)
        End If
        If Check = "True" Then
            GetChecked = "checked=""checked"""
        Else
            GetChecked = String.Empty
        End If
    End Function

    Public Function GetExamAmount() As String
        Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId").ToString, Request.QueryString("qSetId"))
        Return ExamAmount
    End Function

    Public Function GetExplainStatusText(ByVal QuestionId As String) As String
        GetExplainStatusText = objTestSet.GetTextForlblEditEachQuestion(QuestionId)
        If GetExplainStatusText IsNot Nothing Then
            If GetExplainStatusText <> "" Then
                Return GetExplainStatusText
            Else
                Return ""
            End If
        End If

    End Function

    Public Function GetEvaluatinStatusText(ByVal QuestionId As String) As String
        GetEvaluatinStatusText = objTestSet.GetTextForlblEvaluationIndex(QuestionId)
        If GetEvaluatinStatusText IsNot Nothing Then
            If GetEvaluatinStatusText <> "" Then
                Return GetEvaluatinStatusText
            Else
                Return ""
            End If
        End If

    End Function

    Private Sub CreateSessionTotalEvaluationIndex()
        Dim _db As New ClassConnectSql
        Dim sql = " SELECT COUNT(*) FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IS NULL "
        Session("ToTalEvaluationIndex") = _db.ExecuteScalar(sql)
    End Sub

    Private Sub ListingQuestionsInModule_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles ListingQuestionsInModule.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Then
            Dim lblEditEachQuestion1 As Label = CType(e.Item.FindControl("lblEditText1"), Label)
            Dim lblEvaluatinIndex1 As Label = CType(e.Item.FindControl("lblEvaText1"), Label)

            If lblEditEachQuestion1.Text = "ยังไม่ทำ" Then
                lblEditEachQuestion1.ForeColor = Drawing.Color.Red
            ElseIf lblEditEachQuestion1.Text = "ทำแล้ว" Then
                lblEditEachQuestion1.ForeColor = Drawing.Color.Green
            End If

            If lblEvaluatinIndex1.Text = "อนุมัติแล้วทั้งหมด" Or lblEvaluatinIndex1.Text = "อนุมัติแล้วบางส่วน" Then
                lblEvaluatinIndex1.ForeColor = Drawing.Color.Green
            ElseIf lblEvaluatinIndex1.Text = "ทำแล้วทุกอัน" Then
                lblEvaluatinIndex1.ForeColor = Drawing.Color.Black
            ElseIf lblEvaluatinIndex1.Text = "ทำบางส่วน" Then
                lblEvaluatinIndex1.ForeColor = Drawing.Color.Red
            ElseIf lblEvaluatinIndex1.Text = "ยังไม่ทำ" Then
                lblEvaluatinIndex1.ForeColor = Drawing.Color.Red
            End If

        End If

        If e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblEditEachQuestion2 As Label = CType(e.Item.FindControl("lblEditText2"), Label)

            If lblEditEachQuestion2.Text = "ยังไม่ทำ" Then
                lblEditEachQuestion2.ForeColor = Drawing.Color.Red
            ElseIf lblEditEachQuestion2.Text = "ทำแล้ว" Then
                lblEditEachQuestion2.ForeColor = Drawing.Color.Green
            End If

            Dim lblEvaluatinIndex2 As Label = CType(e.Item.FindControl("lblEvaText2"), Label)

            If lblEvaluatinIndex2.Text = "อนุมัติแล้วทั้งหมด" Or lblEvaluatinIndex2.Text = "อนุมัติแล้วบางส่วน" Then
                lblEvaluatinIndex2.ForeColor = Drawing.Color.Green
            ElseIf lblEvaluatinIndex2.Text = "ทำแล้วทุกอัน" Then
                lblEvaluatinIndex2.ForeColor = Drawing.Color.Black
            ElseIf lblEvaluatinIndex2.Text = "ทำบางส่วน" Then
                lblEvaluatinIndex2.ForeColor = Drawing.Color.Red
            ElseIf lblEvaluatinIndex2.Text = "ยังไม่ทำ" Then
                lblEvaluatinIndex2.ForeColor = Drawing.Color.Red
            End If

        End If


    End Sub

    <Services.WebMethod()>
    Public Shared Function AddNewQuestionCodeBehind(ByVal StrQsetId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim NewGuid As String = System.Guid.NewGuid().ToString()
        Dim sql As String = " INSERT INTO dbo.tblQuestion(Question_Id ,QSet_Id,Question_No ,Question_Name ,Question_Expain ,IsActive,QId,Question_Name_Backup ,LastUpdate,
                              IsWpp,NoChoiceShuffleAllowed,Question_Name_Quiz,Question_Expain_Quiz) 
                              VALUES  ( '" & NewGuid & "','" & StrQsetId & "',(SELECT MAX(Question_No) + 1 FROM dbo.tblQuestion WHERE QSet_Id = '" & StrQsetId & "' AND Isactive = 1),
                              'คำถามใหม่','',1,null,'',dbo.GetThaiDate(),1,0,'คำถามใหม่','' )  "

        _DB.OpenWithTransection()
        Try
            _DB.ExecuteWithTransection(sql)
            'sql = " SELECT TOP 1 Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & StrQsetId & "' " & _
            '                              " ORDER BY Question_No DESC "
            sql = " INSERT INTO dbo.tblLayoutConfirmed ( LC_Id ,Qset_Id ,Question_Id ,WordEditConfirmed ,WordTechnicalConfirmed " &
                  " ,WordPrePressConfirmed,WordEditConfirmed_UserId ,WordEditConfirmed_LastUpdate ,WordTechnicalConfirmed_UserId , " &
                  " WordTechnicalConfirmed_LastUpdate, WordPrePressConfirmed_UserId, WordPrePressConfirmed_LastUpdate," &
                  " QuizEditConfirmed ,QuizTechnicalConfirmed ,QuizPrePressConfirmed ,QuizEditConfirmed_UserId , QuizEditConfirmed_LastUpdate " &
                  " ,QuizTechnicalConfirmed_UserId ,QuizTechnicalConfirmed_LastUpdate , QuizPrePressConfirmed_UserId ," &
                  " QuizPrePressConfirmed_LastUpdate ,LastUpdate ,IsActive ) VALUES  ( NEWID() , '" & StrQsetId & "', '" & NewGuid & "'" &
                  " , 0 , 0 , 0 , NULL ,NULL , NULL , NULL , NULL , NULL , 0 , 0 , 0 , NULL , NULL , NULL , NULL , NULL , NULL , " &
                  " dbo.GetThaiDate() ,1 ) "
            _DB.ExecuteWithTransection(sql)

            Dim NewQuestionId As String = NewGuid
            sql = " SELECT QSet_Type FROM dbo.tblQuestionSet WHERE QSet_Id = '" & StrQsetId & "' "
            Dim QsetType As String = _DB.ExecuteScalarWithTransection(sql)
            If QsetType = "2" Then
                Dim Score As Integer = 1
                Dim AnswerName As String = "True"
                Dim AnswerNo As Integer = 1
                For z = 1 To 2
                    sql = " INSERT dbo.tblAnswer ( Answer_Id ,Question_Id , Efficiency_Id ,QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score , " &
                          " Answer_ScoreMinus ,Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate,AlwaysShowInLastRow ) " &
                          " VALUES  ( NEWID() , '" & NewQuestionId & "' , NULL , '" & StrQsetId & "' , " & AnswerNo & " , '" & AnswerName & "' " &
                          " , NULL , " & Score & " , 0 , NULL , 1 , '" & AnswerName & "' ,dbo.GetThaiDate(),0 ) "
                    _DB.ExecuteWithTransection(sql)
                    Score = 0
                    AnswerName = "False"
                    AnswerNo = 2
                Next
            ElseIf QsetType = 6 Then
                'ถ้าเป็น Type 6 ต้อง Insert คำตอบไปให้เลย
                sql = " INSERT INTO dbo.tblAnswer( Answer_Id ,Question_Id ,Efficiency_Id ,QSet_Id ,Answer_No , Answer_Name ,Answer_Expain ,Answer_Score ,Answer_ScoreMinus ,Answer_Position ,IsActive , " &
                      " Answer_Name_Bkup ,LastUpdate ,ClientId ,IsWpp ,AlwaysShowInLastRow ) SELECT  NEWID() ,'" & NewGuid & "' ,NULL , '" & StrQsetId & "' ,CAST((MAX(Answer_No)+ 1) AS Varchar(20))  , " &
                      " CAST((MAX(Answer_No)+ 1) AS Varchar(20)) ,'' , 1 , 0.0 , NULL ,1 , '' ,dbo.GetThaiDate() ,NULL ,1  ,0 FROM dbo.tblAnswer WHERE QSet_Id = '" & StrQsetId & "' AND IsActive = 1; "
                _DB.ExecuteWithTransection(sql)
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "Error"
        End Try
        _DB.CommitTransection()

        sql = "select Qset_Name from tblQuestionSet where Qset_Id ='" & StrQsetId & "'"
        Dim QsetName As String = _DB.ExecuteScalar(sql)
        Dim Newlog As String = " เพิ่มคำถามข้อใหม่ในชุดข้อสอบ """ & QsetName & """ (Questionid = " & NewGuid & ") "
        Log.Record(Log.LogType.ManageExam, Newlog, True, "", NewGuid)
        Return "Complete"
    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteQuestionCodeBehind(ByVal StrQset_Id As String, ByVal StrQuestionId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " UPDATE dbo.tblQuestion SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id = '" & StrQuestionId & "'; "
        sql &= "Update tblQuestionEvaluationIndexItem set isactive = 0 , ClientId = null, LastUpdate = dbo.GetThaiDate() where  Question_Id = '" & StrQuestionId & "';"
        _DB.OpenWithTransection()
        Try
            _DB.ExecuteWithTransection(sql)
            sql = " SELECT QSet_Type FROM dbo.tblQuestionSet WHERE QSet_Id = '" & StrQset_Id & "'; "
            Dim QsetType As String = _DB.ExecuteScalarWithTransection(sql)

            'If QsetType = "2" Then
            sql = " UPDATE  dbo.tblAnswer SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id = '" & StrQuestionId & "'; "
            _DB.ExecuteWithTransection(sql)
            'End If

            Dim dt As New DataTable
            sql = " SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & StrQset_Id & "' AND IsActive = 1 ORDER BY Question_No; "
            dt = _DB.getdataWithTransaction(sql)
            If dt.Rows.Count > 0 Then
                Dim IndexQuestionNo As Integer = 1
                Dim EachQuestionId As String = ""
                For z = 0 To dt.Rows.Count - 1
                    EachQuestionId = dt.Rows(z)("Question_Id").ToString()
                    sql = " UPDATE dbo.tblQuestion SET Question_No = '" & IndexQuestionNo & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Question_Id = '" & EachQuestionId & "'; "
                    _DB.ExecuteWithTransection(sql)
                    IndexQuestionNo += 1
                Next
            End If

            'ถ้าเป็น Type 6 ต้องวน update Answer ด้วย
            If QsetType = 6 Then
                sql = " SELECT Answer_Id FROM dbo.tblAnswer WHERE IsActive = 1 AND QSet_Id = '" & StrQset_Id & "' ORDER BY Answer_No; "
                dt.Clear()
                dt = _DB.getdataWithTransaction(sql)
                If dt.Rows.Count > 0 Then
                    Dim IndexAnswerNo As Integer = 1
                    Dim EachAnswerId As String = ""
                    For z = 0 To dt.Rows.Count - 1
                        EachAnswerId = dt.Rows(z)("Answer_Id").ToString()
                        sql = " UPDATE dbo.tblAnswer SET Answer_No = '" & IndexAnswerNo & "'  , Answer_Name = '" & IndexAnswerNo & "' , LastUpdate = dbo.GetThaiDate() WHERE Answer_Id = '" & EachAnswerId & "'; "
                        _DB.ExecuteWithTransection(sql)
                        IndexAnswerNo += 1
                    Next
                End If
            End If

            Dim NewLog As String = " ลบคำถาม (QuestionId = " & StrQuestionId & ") "
            Log.Record(Log.LogType.ManageExam, NewLog, True, "", StrQuestionId)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "Error"
        End Try

        _DB.CommitTransection()
        Return "Complete"

    End Function

End Class