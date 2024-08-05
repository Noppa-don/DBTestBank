Imports KnowledgeUtils.Web

Public Class Step2
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    Public GroupName As String
    'Dim ClsSelectSess As New ClsSelectSession
    Public currentPage As String
    Protected IsAndroid As Boolean
    Public IE As String
    Protected EditTestSetWarningText As String = CommonTexts.EditTestSetWarningText
    Protected UserSubjectAmount As Integer
    Protected ColsapnAmount As Integer

    Private UserId As String

    Private TempTestset As Testset

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
#If IE = "1" Then
        Session("userid") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
#End If
        If Not Page.IsPostBack() Then
            Log.Record(Log.LogType.PageLoad, "pageload step2", False)
        End If

        If (Session("UserId") Is Nothing) Then
            Log.Record(Log.LogType.PageLoad, "step2 session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        Else
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

            UserId = Session("UserId").ToString()
            'จำนวนวิชาที่ user สามารถใช้งานได้
            UserSubjectAmount = GetMaxNumberOfSubject()

            'If UserSubjectAmount >= 5 Then
            '    ColsapnAmount = 5
            'Else
            ColsapnAmount = UserSubjectAmount
            'End If

            If ClsKNSession.IsAddQuestionBySchool Then
                ' mode ใหม่มีการเพิ่มข้อสอบด้วยตัวเอง
                If Not IsNothing(Request.QueryString("editid")) Then
                    Dim testsetId As String = Request.QueryString("editid").ToString()
                    TempTestset = If((Session("TempTestset") Is Nothing), GetEditTestset(testsetId), Session("TempTestset"))

                Else
                    TempTestset = If((Session("TempTestset") Is Nothing), New Testset, Session("TempTestset"))
                End If
                Session("TempTestset") = TempTestset
            Else
                ' mode เดิม 
                If IsNothing(Session("objTestSet")) Then
                    objTestSet = New ClsTestSet(Session("UserId").ToString())
                    Session("objTestSet") = objTestSet
                Else
                    objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
                End If

                If Not IsNothing(Request.QueryString("editid")) Then
                    Session("EditID") = Request.QueryString("editid").ToString()
                    Dim dt As DataTable = objTestSet.GetTestsetName(Session("EditID"))
                    Session("newTestSetName") = dt.Rows(0)("testset_name").ToString()
                    Session("newTestSetTime") = dt.Rows(0)("testset_time").ToString()
                    Session("newTestSetFontSize") = dt.Rows(0)("TestSet_FontSize").ToString()
                    Session("newTestsetIsPractice") = dt.Rows(0)("IsPracticeMode").ToString()
                Else
                    Session("EditID") = ""
                End If
            End If

            ' สร้าง checkbox
            If Not Page.IsPostBack() Then
                Dim enableUserSubjectClass As String = Application("EnableUserSubjectClass").ToString
                Dim ds As DataSet = GetAllowedClass(enableUserSubjectClass) ' objTestSet.GetAllowedClass(Application("EnableUserSubjectClass").ToString)
                If ds.Tables(0).Rows.Count = 1 Then
                    If ClsKNSession.IsAddQuestionBySchool Then
                        Dim classId As String = ds.Tables(0).Rows(0)("ClassId").ToString()
                        Dim dsSubject As DataSet = GetAllowedSubject(classId, enableUserSubjectClass)
                        Dim subjectId = dsSubject.Tables(0).Rows(0)("GroupSubject_Id").ToString()

                        If dsSubject.Tables(0).Rows.Count = 1 Then 'IsOneSubject(classId, UserId)
                            If Not TempTestset.SubjectClassSelected.ContainsKey(subjectId) Then
                                TempTestset.SubjectClassSelected.Add(subjectId, New List(Of String)(New String() {classId}))
                            End If
                            Session("TempTestset") = TempTestset
                            Response.Redirect("~/testset/newstep3.aspx", True)
                        End If
                    Else
                        Dim classId As String = ds.Tables(0).Rows(0)("Level_Id").ToString()
                        Dim dsSubject As DataSet = objTestSet.GetAllowedSubject(classId, enableUserSubjectClass)
                        Dim listSelectedSubjects As New List(Of Object)
                        listSelectedSubjects.Add(New Selectedsubjects With {.KeyId = "SC_" & dsSubject.Tables(0).Rows(0)("GroupSubject_Name") & "_" & classId,
                                                        .ClassId = classId,
                                                        .ClassName = ds.Tables(0).Rows(0)("Level_Shortname"),
                                                        .SubjectName = dsSubject.Tables(0).Rows(0)("GroupSubject_Name"),
                                                        .SubjectId = dsSubject.Tables(0).Rows(0)("GroupSubject_Id").ToString()
                                                      })
                        objTestSet.SelectedSubjectClass = IIf(listSelectedSubjects.Count = 0, Nothing, listSelectedSubjects)
                        objTestSet.SelectedSyllabusYear = "y51"

                        Session("SelectedYears") = objTestSet.SelectedSyllabusYear
                        Response.Redirect("~/testset/step3.aspx", False)
                    End If
                End If
                ' source

                Dim dtClass As DataTable = ds.Tables(0)

                Dim LevelPratom = From r In dtClass Where r("Level_ShortName").ToString.IndexOf("ป.") >= 0
                If LevelPratom.Count <> 0 Then
                    ListingMaster.DataSource = LevelPratom.CopyToDataTable
                    ListingMaster.DataBind()
                End If

                Dim LevelMattayom = From r In dtClass Where r("Level_ShortName").ToString.IndexOf("ม.") >= 0
                If LevelMattayom.Count <> 0 Then
                    ListMaster2.DataSource = LevelMattayom.CopyToDataTable
                    ListMaster2.DataBind()
                End If
            End If

        End If
    End Sub

    Private Function IsOneSubject(classId As String, userId As String) As Boolean
        Dim db As New ClassConnectSql
        Dim sql As String = "SELECT * FROM tblUserSubjectClass WHERE LevelId = '" & classId & "' AND UserId = '" & userId & "';"
        Dim dt As DataTable = db.getdata(sql)
        If dt.Rows.Count = 1 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' function ในการ สร้าง testset จาก editId สำหรับ testset ที่ต้องการแก้ไข
    ''' </summary>
    ''' <param name="testsetId"></param>
    ''' <returns>Testset</returns>
    Private Function GetEditTestset(testsetId As String) As Testset
        Dim dt As DataTable = GetTestset(testsetId)
        Dim tempTestset As Testset = New Testset(dt)
        'getselected class,subject
        dt = GetGroupSubjectAndLevelIdInTestset(testsetId)
        tempTestset.SubjectClassSelected = GetSubjectClassSelected(dt)
        'getqset class,subject
        tempTestset.ListSubjectClassQuestion = GetQuestionsSelected(testsetId, dt)
        Return tempTestset
    End Function


    ''' <summary>
    ''' function ในการว่าเลือกวิชาไหนไปบ้างแล้ว เอากลับมาใส่ dictionary เหมือนเดิม เพื่อ render การเลือกวิชาตาม testset เดิม
    ''' </summary>
    ''' <param name="dtSubjectClassSelected"></param>
    ''' <returns>Dictionary</returns>
    Private Function GetSubjectClassSelected(dtSubjectClassSelected As DataTable) As Dictionary(Of String, List(Of String))
        Dim listSubjectClass As New Dictionary(Of String, List(Of String))
        For Each r In dtSubjectClassSelected.Rows
            Dim subjectId As String = r("GroupSubject_Id").ToString().ToLower()
            Dim classId As String = r("Level_Id").ToString().ToLower()
            If listSubjectClass.ContainsKey(r("GroupSubject_Id").ToString()) Then
                Dim listClassId As List(Of String) = listSubjectClass(subjectId)
                listClassId.Add(classId)
                listSubjectClass(subjectId) = listClassId
            Else
                listSubjectClass.Add(subjectId, New List(Of String)({classId}))
            End If
        Next
        Return listSubjectClass
    End Function

    ''' <summary>
    ''' function ในการหาว่ามีวิชาชั้นไหนบ้าง ที่เลือกมาอยู่ใน testset จาก database
    ''' </summary>
    ''' <param name="testsetId"></param>
    ''' <returns>DataTable</returns>
    Private Function GetGroupSubjectAndLevelIdInTestset(testsetId As String) As DataTable
        Dim sql As String = "SELECT b.GroupSubject_Id,b.Level_Id,qs.Qset_Id,qs.IsWpp,qs.Qset_Type FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionset qs ON tsqs.QSet_Id = qs.QSet_Id "
        sql &= " INNER JOIN tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id INNER JOIN tblBook b On qc.Book_Id = b.BookGroup_Id INNER JOIN tblLevel l ON l.Level_Id = b.Level_Id "
        sql &= " WHERE tsqs.TestSet_Id = '" & testsetId & "' AND tsqs.IsActive = 1 ORDER BY b.GroupSubject_Id,l.Level;"
        Dim db As New ClassConnectSql()
        Return db.getdata(sql)
    End Function

    Private Function GetQuestionsSelected(testsetId As String, dt As DataTable) As List(Of TestsetSubjectClassQuestion)
        Dim ListSubjectClassQuestion As New List(Of TestsetSubjectClassQuestion)
        For Each r In dt.Rows
            Dim classId As String = r("Level_Id").ToString()
            Dim subjectId As String = r("GroupSubject_Id").ToString()
            Dim qsetId As String = r("Qset_Id").ToString()
            Dim qsetType As EnumQsetType = r("Qset_Type")

            Dim subjectClassId = ListSubjectClassQuestion.Where(Function(t) t.ClassId = classId And t.SubjectId = subjectId).SingleOrDefault()
            If subjectClassId Is Nothing Then
                subjectClassId = New TestsetSubjectClassQuestion
                subjectClassId.SubjectId = subjectId
                subjectClassId.ClassId = classId
                ListSubjectClassQuestion.Add(subjectClassId)
            End If

            If Not subjectClassId.IsTestsetQuestionsetExist(qsetId) Then
                Dim dtQuestions As DataTable = GetTempQuestions("userid", qsetId, EnumAddBy.Qset, subjectClassId, testsetId)
                subjectClassId.AddTestsetQuestionsetWithQuestion(qsetId, qsetType, dtQuestions, "userid", r("IsWpp"))
            End If
        Next
        Return ListSubjectClassQuestion
    End Function

    Private Function GetTempQuestions(addById As String, qsetId As String, addBy As EnumAddBy, s As TestsetSubjectClassQuestion, testsetId As String) As DataTable
        Dim sql As New StringBuilder
        sql.Append(" SELECT tsqd.* FROM tblQuestion q INNER JOIN tblQuestionset qs ON q.QSet_Id = qs.QSet_Id ")
        sql.Append(" INNER JOIN tblTestSetQuestionDetail tsqd ON tsqd.Question_Id = q.Question_Id ")
        sql.Append(" INNER JOIN tblTestSetQuestionSet tsqs ON tsqd.TSQS_Id = tsqs.TSQS_Id ")
        sql.Append(" WHERE q.IsActive = 1 AND qs.QSet_Id = '" & qsetId & "' ")
        sql.Append(" AND tsqs.TestSet_Id = '" & testsetId & "' ")
        sql.Append(" ORDER BY q.Question_No;")
        Dim db As New ClassConnectSql()
        Return db.getdata(sql.ToString)

        'Return GetQuestionsFromQset(qsetId)
        'Select Case addBy
        '    Case EnumAddBy.Qset
        '        Return GetQuestionsFromQset(qsetId)
        '    Case EnumAddBy.KPA
        '        Return GetQuestionsFromEvalution(addById, s.SubjectId, s.ClassId)
        '    Case EnumAddBy.Evalution
        '        Return GetQuestionsFromEvalution(addById, s.SubjectId, s.ClassId)
        'End Select
        'Return New DataTable
    End Function

    Private Function GetQuestionsFromQset(qsetId As String) As DataTable
        Dim sql As String = "select * from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id where q.IsActive = 1 and qs.QSet_Id = '" & qsetId & "' order by q.Question_No;"
        Dim db As New ClassConnectSql()
        Return db.getdata(sql)
    End Function

    ''' <summary>
    ''' function ในการเช็คว่า user มีสิทธิ์ใช้กี่วิชา
    ''' 2017/06/12 ไหมปรับให้เช็คค่าจาก DB ไม่ต้องเช็คจาก File Config
    ''' </summary>
    ''' <param name="enableUserSubjectClass"></param>
    ''' <returns></returns>
    Private Function GetAllowedClass(enableUserSubjectClass As String) As DataSet
        Dim ds As DataSet
        Dim db As New ClassConnectSql()
        Dim sql As String = " select Level_Id,Level_ShortName from(" & _
                            " select distinct Level_Id,Level_ShortName,tblLevel.level from tblUserSubjectClass" & _
                            " inner join tblLevel on tblUserSubjectClass.LevelId = tblLevel.Level_Id " & _
                            " where UserId = '" & UserId & "' and tblUserSubjectClass.IsActive = 1 and tblLevel.IsActive = 1) a order by a.Level"
        'If ClsKNSession.IsAddQuestionBySchool Then
        '    ds = GetUserAllowedClass(enableUserSubjectClass)
        'Else
        '    ds = objTestSet.GetAllowedClass(enableUserSubjectClass)
        '    End If
        ds = db.getdataset(sql)
        Return ds
    End Function


    Private Function GetUserAllowedClass(ByVal ConfigLevel As String)
        Dim StrLevel As String = ""
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable

        Dim strSQL As String = "select * from tbllevel order by level;"
        dt = db.getdata(strSQL)

        ConfigLevel = ConfigLevel & ","

        For Each r As DataRow In dt.Rows
            If InStr(ConfigLevel.ToLower(), "k" & r("Level") & ",") <> 0 Then
                StrLevel &= "'" & r("Level_id").ToString & "',"
            End If
        Next

        StrLevel = StrLevel.Substring(0, StrLevel.Length - 1)

        strSQL = " SELECT ClassId,ClassName from uvw_getlevelbyuser  where userid = '" & UserId & "' and ClassId in (" & StrLevel & ") order by Level;"

        Return db.getdataset(strSQL)
    End Function

    ''' <summary>
    ''' เป็น function เดียวกับใน clsTestset แต่แยกออกมาเนื่องจากใช้กับโหมดเพิ่มข้อสอบในโรงเรียน ในอนาคต อาจจะยุบรวบกัน
    ''' 2017/06/12 -- ไหมปรับให้ไม่ต้องอ่านวิชาจากไฟล์ Config
    ''' </summary>
    ''' <param name="classId"></param>
    ''' <param name="ConfigSubject"></param>
    ''' <returns>dataset</returns>
    Private Function GetAllowedSubject(ByVal classId As String, ByVal ConfigSubject As String) As DataSet
        Dim strSQL As String
        Dim ds As New DataSet
        'Dim classKFormat As String = classId.ToClassKFormat()

        'Dim ArrConfigStr() As String = Split(ConfigSubject, ",")

        'Dim SelectedConfigStr As String = ""


        'For Each i As String In ArrConfigStr
        '    If InStr(i.ToLower, classKFormat.ToLower) <> 0 Then
        '        Dim ArrSub() As String = Split(i, "-")
        '        If classKFormat.Length = ArrSub(1).Length Then ' เช็คอีกที เนื่องจาก k10 กับ k101112 = True
        '            SelectedConfigStr = SelectedConfigStr & "'" & (ArrSub(0).ToSubjectId()) & "',"
        '        End If
        '    End If
        'Next

        'If SelectedConfigStr <> "" Then
        'SelectedConfigStr = SelectedConfigStr.Substring(0, SelectedConfigStr.Length - 1)
        strSQL = " SELECT tblGroupSubject.GroupSubject_Id, tblGroupSubject.GroupSubject_ShortName as GroupSubject_Name,a.QuestionAmount "
        strSQL &= " FROM tblUserSubjectClass INNER JOIN tblLevel ON tblUserSubjectClass.LevelId = tblLevel.Level_Id INNER JOIN tblGroupSubject ON tblUserSubjectClass.GroupSubjectId = tblGroupSubject.GroupSubject_Id"
        strSQL &= " left join (SELECT count(tblquestion.Question_Id) as QuestionAmount,"
        strSQL &= " Level_Id,GroupSubject_Id  FROM tblbook left join tblQuestionCategory on tblQuestionCategory.Book_Id = tblbook.BookGroup_Id "
        strSQL &= " left join tblquestionset on tblQuestionCategory.QCategory_Id = tblquestionset.QCategory_Id"
        strSQL &= " left join tblquestion on tblQuestionset.QSet_Id = tblQuestion.QSet_Id where Book_Syllabus = '51' and tblbook.IsActive = 1 group by Level_Id,GroupSubject_Id)a"
        strSQL &= " on a.Level_Id = tblUserSubjectClass.LevelId and a.GroupSubject_Id = tblUserSubjectClass.GroupSubjectId"
        strSQL &= " where  tblUserSubjectClass.isactive = '1'  and tblUserSubjectClass.LevelId = '" & classId.ToString() & "' and tblUserSubjectClass.userid = '" & UserId & "'  "
        ' strSQL &= " and tblGroupSubject.GroupSubject_Id in (" & SelectedConfigStr & ")"
        strSQL &= " ORDER BY tblUserSubjectClass.SubjectId "
        Dim db As New ClassConnectSql()
        ds = db.getdataset(strSQL)

        If ds.Tables(0) Is Nothing Then
            ds.Tables.Add("0")
        End If

        Return ds
    End Function

    ''' <summary>
    ''' function ในการหาว่าจำนวนวิชาทั้งหมดที่ user สามารถใช้งานได้
    ''' </summary>
    ''' <returns>จำนวนวิชา</returns>
    Private Function GetMaxNumberOfSubject() As Integer
        Dim strSQL As String = "select isnull(count(groupsubjectId),0) from tblUserSubjectClass where userid = '" & UserId & "' and LevelId = '5F4765DB-0917-470B-8E43-6D1C7B030818';"
        Dim db As New ClassConnectSql()
        Return CInt(db.ExecuteScalar(strSQL))
    End Function

    Protected Friend Function CreateSubjectList(ByVal classId As String)
        Dim enableUserSubjectClass As String = Application("EnableUserSubjectClass").ToString

        Dim dtSavedData As DataTable
        If Not IsNothing(Request.QueryString("editid")) Then
            If Request.QueryString("editid").ToString() <> "" AndAlso Not ClsKNSession.IsAddQuestionBySchool Then
                dtSavedData = objTestSet.GetSavedSubjectId(classId, Request.QueryString("editid").ToString())
            End If
        End If
        Dim ds As DataSet = If((ClsKNSession.IsAddQuestionBySchool), GetAllowedSubject(classId, enableUserSubjectClass), objTestSet.GetAllowedSubject(classId, enableUserSubjectClass))
        Dim sb As New System.Text.StringBuilder()

        If Not IsNothing(ds.Tables(0)) Then
            Dim subjName As String
            Dim QuestionAmount As String
            Dim subjectId As String
            Dim chkboxIdName As String

            'If ds.Tables(0).Rows.Count >= 5 Then
            '    ColsapnAmount = 5
            'Else
            ColsapnAmount = ds.Tables(0).Rows.Count
            'End If

            For i = 0 To ds.Tables(0).Rows.Count - 1
                subjName = ds.Tables(0).Rows(i)("GroupSubject_Name").ToString()
                QuestionAmount = ds.Tables(0).Rows(i)("QuestionAmount").ToString()
                subjectId = ds.Tables(0).Rows(i)("GroupSubject_Id").ToString()

                chkboxIdName = "SC_" & subjName & "_" & classId & "_" & subjectId

                'If i = 5 Then
                '    sb.Append("</tr><tr><td></td>")
                'End If

                If (i = ds.Tables(0).Rows.Count - 1) And (i < (UserSubjectAmount - 1)) Then
                    sb.Append("<td colspan='" & (UserSubjectAmount - i).ToString() & "'><div class='divchk'><input type='checkbox' name='")
                Else
                    sb.Append("<td><div class='divchk'><input type='checkbox' name='")
                End If
                sb.Append(chkboxIdName)

                If QuestionAmount = "" Then
                    sb.Append("' disabled=""disabled""")
                Else
                    sb.Append("'")
                End If

                sb.Append(" value='' id='")
                sb.Append(chkboxIdName)
                sb.Append("' ")

                If ClsKNSession.IsAddQuestionBySchool Then
                    If TempTestset.IsSubjectClassSelected(subjectId, classId) Then
                        sb.Append(" checked='checked' ")
                    End If
                Else
                    If Not IsNothing(objTestSet.SelectedSubjectClass) Then
                        If objTestSet.SelectedSubjectClass.Count > 0 Then
                            For Each g In objTestSet.SelectedSubjectClass
                                If g.Keyid = chkboxIdName Then
                                    sb.Append(" checked='checked' ")
                                End If
                            Next
                        End If
                    End If

                    If Not IsNothing(dtSavedData) Then
                        If dtSavedData.Rows.Count > 0 Then
                            Dim drows() As DataRow = dtSavedData.Select("subject_id = '" & ds.Tables(0).Rows(i)("GroupSubject_Id").ToString() & "'")
                            If (drows.Count > 0) Then
                                sb.Append(" checked='checked' ")
                            End If
                        End If
                    End If
                End If

                sb.Append("><label for='")
                sb.Append(chkboxIdName)

                If QuestionAmount = "" Then
                    sb.Append("' Style = ""color: gray;  background-image: url(../images/bullet-disable.gif);""")
                Else
                    sb.Append("'")
                End If

                sb.Append(">" & subjName)
                sb.Append("</label>")
                sb.Append("<div style='text-align:center;' ><a href='../Testset/ReportManageExam.aspx?GId=" & ds.Tables(0).Rows(i)("GroupSubject_Id").ToString() & "&LId=" & classId & "' target='_blank'>")

                If HttpContext.Current.Application("NeedReportButton") = True Then
                    sb.Append("<img src='../Images/preview.png' style='width:30px;height:30px;' /></a></div>")
                End If

                sb.Append("</div></td>")
            Next
        End If
        Return sb.ToString()
    End Function

    Protected Sub btnNextStep3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextStep3.Click
        If ClsKNSession.IsAddQuestionBySchool Then
            AddSubjectClassSelected()
            If TempTestset.SubjectClassSelected.Count > 0 Then
                Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนที่ 3 หน้าจอแบบใหม่", True)
                Response.Redirect("~/testset/NewStep3.aspx")
            End If
        Else
            ProcessSelectedCheckBoxes(Request.Form)
            'ต้องมีค่าทั้ง 2 ส่วน, (คือต้องเลือก หลักสูตร และวิชา) ไม่งั้นไม่ redirect ไปหน้าถัดไปให้
            If (Not IsNothing(objTestSet.SelectedSubjectClass)) And (Not IsNothing(objTestSet.SelectedSyllabusYear)) Then
                Session("SelectedYears") = objTestSet.SelectedSyllabusYear
                Log.Record(Log.LogType.ExamStep, "ไปขั้นตอนที่ 3", True)
                Response.Redirect("~/testset/step3.aspx", False)
            End If
        End If
    End Sub


    Private Sub ProcessSelectedCheckBoxes(ByVal formsValue As System.Collections.Specialized.NameValueCollection)
        'Session("SelectedSubjectClass") = Nothing
        'Session("SelectedSyllabusYear") = Nothing
        objTestSet.SelectedSubjectClass = Nothing
        objTestSet.SelectedSyllabusYear = Nothing
        Dim listSelectedSubjects As New List(Of Object)
        Dim strYear As String = ""
        For Each item In formsValue
            If item.StartsWith("SC_") Then
                Dim sp As String() = item.Split("_")
                listSelectedSubjects.Add(New Selectedsubjects With {.KeyId = item,
                                                    .ClassId = sp(2),
                                                    .ClassName = objTestSet.GetClassNameById(sp(2)),
                                                    .SubjectName = sp(1),
                                                    .SubjectId = objTestSet.GetSubjectIdByName(sp(1))
                                                  })
            End If
            If item.StartsWith("y") Then
                If strYear = "" Then
                    strYear = item
                Else
                    strYear = strYear + "," + item
                End If
            End If
        Next
        objTestSet.SelectedSubjectClass = IIf(listSelectedSubjects.Count = 0, Nothing, listSelectedSubjects)
        objTestSet.SelectedSyllabusYear = IIf(strYear = "", Nothing, strYear)

    End Sub

    Private Sub AddSubjectClassSelected()
        Dim formsValue As System.Collections.Specialized.NameValueCollection = Request.Form
        'clear วิชาที่เลือกมาออกให้หมดก่อน ค่อยใส่เข้าไปใหม่ เจอใน case ที่ เอาชุดเก่ากลับมาแก้ใหม่
        TempTestset.ClearSubjectClassSelected()
        'วนลูป เพื่อเอาวิชาที่เลือกเข้าไปใหม่
        For Each item In formsValue
            If item.StartsWith("SC_") Then
                Dim sp As String() = item.Split("_")
                Dim classId As String = sp(2).ToLower()
                Dim subjectId As String = sp(3).ToLower()
                If TempTestset.SubjectClassSelected.ContainsKey(subjectId) Then
                    Dim listClassId As List(Of String) = TempTestset.SubjectClassSelected(subjectId)
                    If Not listClassId.Contains(classId) Then
                        listClassId.Add(classId)
                    End If
                    TempTestset.SubjectClassSelected(subjectId) = listClassId
                Else
                    TempTestset.SubjectClassSelected.Add(subjectId, New List(Of String)(New String() {classId}))
                End If
            End If
        Next
        Session("TempTestset") = TempTestset
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

    Protected Friend Function GetYearChecked(ByVal checkedYear As String) As String
        If ClsKNSession.IsAddQuestionBySchool Then
            Return "checked=""checked"""
        Else
            If Not Session("EditID") = "" Then
                If objTestSet.GetSelectedYear(Session("EditID"), checkedYear) Then
                    Return "checked=""checked"""
                End If
            Else
                If Right(checkedYear, 2) = "51" Then
                    Return "checked=""checked"""
                End If
            End If

            If objTestSet.SelectedSyllabusYear = "y51,y44" Then
                Return "checked=""checked"""
            End If

            If Right(objTestSet.SelectedSyllabusYear, 2) = checkedYear Then
                Return "checked=""checked"""
            End If
        End If
        Return ""
    End Function


    ''' <summary>
    ''' function get testset data มาจาก tbltestset
    ''' </summary>
    ''' <param name="Testset_Id"></param>
    ''' <returns>DataTable TblTestset</returns>
    Private Function GetTestset(ByVal Testset_Id As String) As DataTable
        Dim sql As String = "select * from tbltestset where testset_Id = '" & Testset_Id.ToString & "' AND UserId = '" & UserId & "';"
        Dim db As New ClassConnectSql()
        Return db.getdata(sql)
    End Function

End Class

