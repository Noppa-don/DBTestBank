Option Strict Off
Imports Excel = Microsoft.Office.Interop.Excel
Imports Telerik.Web.UI
Imports Microsoft.Office
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports iTextSharp.text
Imports Syncfusion.DocIO
Imports System.Net.Mail
Imports System.Net

<Serializable()> _
Public Class ClsTestSet
    Private ds As DataSet
    Private currentUserId As String
    Public SelectedSubjectClass As List(Of Object)
    Public SelectedSyllabusYear As String
    Dim SelectedYear As String
    Dim objPDF As New ClsPDF(New ClassConnectSql)
    Dim RecordLog As New ClsLog

    Public Sub New(ByVal userId As String)
        currentUserId = userId

    End Sub
    Public Function GetAllTestSet() As DataSet
        Dim strSQL As String = "select distinct t.TestSet_Id, t.TestSet_Name, t.LastUpdate  from tbltestset t, tbltestsetquestionset tsqs "
        strSQL = strSQL + " where  t.testset_id = tsqs.testset_id and t.userid = '" & currentUserId.CleanSQL & "' and IsHomeworkMode <> 1"
        strSQL = strSQL + "  and ((t.IsActive = 0 and t.testset_name = 'กำลังอยู่ระหว่างการจัดชุด' and t.LastUpdate >   DATEADD(hour,-8,dbo.GetThaiDate()) )"
        strSQL = strSQL + "  or (t.IsActive = 1 )) order by t.lastupdate desc ;"
        Dim db As New ClassConnectSql()
        GetAllTestSet = db.getdataset(strSQL)
    End Function
    Public Function GetAllQuestions(ByVal moduleId As Integer) As DataSet

        Dim strSQL As String = "select distinct s.subjectid, s.subjectname from tblsubject s, tblusersubjectclass usc where s.subjectid = usc.subjectid and usc.isactive = '1' and usc.classid = '" & moduleId.ToString().CleanSQL & "' and usc.userid = '" & currentUserId.CleanSQL & "' order by s.subjectid;"
        Dim db As New ClassConnectSql()
        GetAllQuestions = db.getdataset(strSQL)
    End Function
    Public Function GetAllowedSubject(ByVal classId As String, ByVal ConfigSubject As String) As DataSet

        Dim strSQL As String
        Dim ds As New DataSet
        'Dim ClassNickName As String = GetClassNickName(classId)

        ''ConfigSubject = ConfigSubject & BookSpecial()

        'Dim ArrConfigStr() As String = Split(ConfigSubject, ",")

        'Dim SelectedConfigStr As String = ""


        'For Each i As String In ArrConfigStr

        '    If InStr(i.ToLower, ClassNickName.ToLower) <> 0 Then
        '        Dim ArrSub() As String = Split(i, "-")
        '        If ClassNickName.Length = ArrSub(1).Length Then ' เช็คอีกที เนื่องจาก k10 กับ k101112 = True
        '            SelectedConfigStr = SelectedConfigStr & "'" & GetSubjectId(ArrSub(0)) & "',"
        '        End If
        '    End If
        'Next

        'If SelectedConfigStr <> "" Then

        strSQL = " SELECT tblGroupSubject.GroupSubject_Id, tblGroupSubject.GroupSubject_ShortName as GroupSubject_Name,case when a.QuestionAmount is null then 0 else a.QuestionAmount end as QuestionAmount "
        strSQL &= " FROM tblUserSubjectClass INNER JOIN tblLevel ON tblUserSubjectClass.LevelId = tblLevel.Level_Id INNER JOIN tblGroupSubject ON tblUserSubjectClass.GroupSubjectId = tblGroupSubject.GroupSubject_Id"
        strSQL &= " left join (SELECT count(tblquestion.Question_Id) as QuestionAmount,"
        strSQL &= " Level_Id,GroupSubject_Id  FROM tblbook left join tblQuestionCategory on tblQuestionCategory.Book_Id = tblbook.BookGroup_Id "
        strSQL &= " left join tblquestionset on tblQuestionCategory.QCategory_Id = tblquestionset.QCategory_Id"
        strSQL &= " left join tblquestion on tblQuestionset.QSet_Id = tblQuestion.QSet_Id where Book_Syllabus = '51' and tblbook.IsActive = 1 group by Level_Id,GroupSubject_Id)a"
        strSQL &= " on a.Level_Id = tblUserSubjectClass.LevelId and a.GroupSubject_Id = tblUserSubjectClass.GroupSubjectId"

        strSQL &= " where  tblUserSubjectClass.isactive = '1'  and tblUserSubjectClass.LevelId = '" & classId.ToString().CleanSQL & "' and tblUserSubjectClass.userid = '" & currentUserId.CleanSQL & "'  "

        'ตัด Query ส่วนที่เปรียบเทียบไฟล์ Config ออก

        'SelectedConfigStr = SelectedConfigStr.Substring(0, SelectedConfigStr.Length - 1)

        'strSQL &= " and tblGroupSubject.GroupSubject_Id in (" & SelectedConfigStr & ")"

        strSQL &= " ORDER BY tblUserSubjectClass.SubjectId "

        'strSQL = " SELECT tblGroupSubject.GroupSubject_Id, tblGroupSubject.GroupSubject_Name"
        'strSQL &= " FROM tblUserSubjectClass INNER JOIN"
        'strSQL &= " tblLevel ON tblUserSubjectClass.LevelId = tblLevel.Level_Id INNER JOIN"
        'strSQL &= " tblGroupSubject ON tblUserSubjectClass.GroupSubjectId = tblGroupSubject.GroupSubject_Id "
        'strSQL &= " where  tblUserSubjectClass.isactive = '1' "
        'strSQL &= " and tblUserSubjectClass.LevelId = '" & classId.ToString() & "' and tblUserSubjectClass.userid = '" & currentUserId & "' "

        'SelectedConfigStr = SelectedConfigStr.Substring(0, SelectedConfigStr.Length - 1)

        'strSQL &= " and tblGroupSubject.GroupSubject_Id in (" & SelectedConfigStr & ")"

        'strSQL &= " ORDER BY tblUserSubjectClass.SubjectId "
        Dim db As New ClassConnectSql()
        ds = db.getdataset(strSQL)

        If ds.Tables(0).Rows.Count = 0 Then
            ds.Tables.Add("0")
        End If

        Return ds
        'Else
        'ds.Tables.Add("0")
        'Return ds
        'End If

    End Function

    Private Function GetClassNickName(ByVal Class_Id As String) As String

        Dim sql As String = "Select Level From tblLevel Where Level_id = '" & Class_Id.CleanSQL & "'"
        Dim db As New ClassConnectSql()

        Dim ClassNickName As String = db.ExecuteScalar(sql)

        Return "k" & ClassNickName

    End Function

    Private Function GetSubjectId(ByVal Subject As String) As String

        Select Case Subject
            Case "thai"
                Return "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
            Case "social"
                Return "FDA224D9-CEBE-4642-ACD0-D7F7282E36AE"
            Case "math"
                Return "A4B9F5CB-2D3C-4F6A-8666-FD2620E69723"
            Case "science"
                Return "58802565-23BB-4F22-8238-E983AC781B0F"
            Case "eng"
                Return "FB677859-87DA-4D8D-A61E-8A76566D69D8"
            Case "health"
                Return "6A4A7294-F5A7-4D64-ADBC-73DC14377737"
            Case "art"
                Return "73C4639B-267C-4B7E-B5A4-1B4EBB428019"
            Case "career"
                Return "47A224EF-3348-41B7-84D0-7250648F8706"
            Case "pisa"
                Return "7F2C522A-FF65-4CA7-9DCD-EC5102A38731"
        End Select



    End Function

    Public Function GetSavedSubjectId(ByVal classId As String, ByVal TestSet_Id As String) As DataTable
        Dim strSQL As String
        strSQL = "SELECT DISTINCT tblGroupSubject.GroupSubject_Id AS Subject_Id"
        strSQL &= " FROM tblTestSetQuestionSet INNER JOIN"
        strSQL &= " tblTestSet ON tblTestSetQuestionSet.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN"
        strSQL &= " tblQuestionset ON tblTestSetQuestionSet.QSet_Id = tblQuestionset.QSet_Id INNER JOIN"
        strSQL &= " tblQuestionCategory ON tblQuestionset.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
        strSQL &= " tblBook INNER JOIN tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id ON"
        strSQL &= "  tblBook.BookGroup_Id = tblQuestionCategory.Book_Id INNER JOIN"
        strSQL &= " tblLevel ON tblTestSetQuestionSet.Level_Id = tblLevel.Level_Id"
        strSQL &= " WHERE (tblLevel.Level_Id = '" & classId.CleanSQL & "') AND (tblTestSet.TestSet_Id = '" & TestSet_Id.CleanSQL & "') And (tblTestSetQuestionSet.IsActive = '1')"


        Dim db As New ClassConnectSql()
        GetSavedSubjectId = db.getdata(strSQL)
    End Function


    'Test pap
    'Private Function BookSpecial() As String
    '    Dim listClass As New List(Of String)({"k456", "k789", "k101112", "k131415"})
    '    Dim listSubject As New List(Of String)({"thai", "social", "math", "science", "eng", "health", "art", "career", "pisa"})
    '    Dim a As New StringBuilder()
    '    For Each c In listClass
    '        For Each s In listSubject
    '            a.Append(String.Format(",{0}-{1}", s, c))
    '        Next
    '    Next
    '    a.Append(String.Format(",{0}-{1}", "pisa", "k8"))
    '    Return a.ToString()
    'End Function

    Public Function GetAllowedClass(ByVal ConfigLevel As String)

        Dim strSQL As String
        Dim StrLevel As String = ""
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable

        strSQL = "select * from tbllevel order by level;"
        dt = db.getdata(strSQL)

        ConfigLevel = ConfigLevel & ","

        For Each r As DataRow In dt.Rows
            If InStr(ConfigLevel.ToLower(), "k" & r("Level") & ",") <> 0 Then
                StrLevel &= "'" & r("Level_id").ToString.CleanSQL & "',"
            End If
        Next

        StrLevel = StrLevel.Substring(0, StrLevel.Length - 1)

        strSQL = " SELECT ClassId,ClassName from uvw_getlevelbyuser  where userid = '" & currentUserId.CleanSQL & "' and ClassId in (" & StrLevel & ") order by Level;"

        Return db.getdataset(strSQL)
    End Function

    Public Function GetQuestionSet(ByVal questionSetId As String, ByVal NeedJoinQ40 As Boolean, IsSelectedEvalutionIndex As Boolean, Optional TestsetId As String = "") As DataSet
        Dim sql As String
        If NeedJoinQ40 Then
            sql = "select q40.qset_id, q.question_id,qs.QSet_Type, q.question_name,qs.IsUseFullQset from tblquestion q, tblquestionset qs, tblQuestion40 q40"
            sql &= " where q.qset_id = qs.qset_id and q.qset_id = '" & questionSetId.CleanSQL & "' "
            sql &= " and q40.Question_Id = q.question_Id and q.Isactive = 1   order by q.question_no; "
        Else
            If IsSelectedEvalutionIndex Then
                If CountTestsetEvalutionIndex(TestsetId) <= 0 Then
                    IsSelectedEvalutionIndex = False
                End If
            End If


            If IsSelectedEvalutionIndex Then

                sql = "Select q.qset_id, q.question_id,qs.QSet_Type, q.question_name,qs.IsUseFullQset  
                        From tblQuestion q
                        inner Join tblQuestionset qs on q.QSet_Id = qs.QSet_Id
                        inner Join tblQuestionEvaluationIndexItem qei on qei.Question_Id = q.Question_Id
                        inner Join tblTestsetEvalutionIndex tei on qei.EI_Id = tei.EIID
                        where q.IsActive = 1 And qs.IsActive = 1 And qei.IsActive = 1 And tei.IsActive = 1
                        And tei.TestsetId = '" & TestsetId & "'
                        And q.QSet_Id = '" & questionSetId.CleanSQL & "'
                        order by q.question_no;"
            Else
                sql = "Select q.qset_id, q.question_id,qs.QSet_Type, q.question_name,qs.IsUseFullQset,q.Qid
                        From tblQuestion q
                        inner Join tblQuestionset qs on q.QSet_Id = qs.QSet_Id
                        where q.IsActive = 1 And qs.IsActive = 1
                        And q.QSet_Id = '" & questionSetId.CleanSQL & "'
                        order by q.question_no;"
            End If
        End If

        Dim db As New ClassConnectSql()

        GetQuestionSet = db.getdataset(sql)
    End Function

    Public Function GetAnswers(ByVal questionId As String) As DataSet
        Dim sql As String
        sql = "Select answer_name, answer_score from tblanswer where question_id = '" & questionId.CleanSQL & "' and IsActive = 1 order by answer_no;"

        Dim db As New ClassConnectSql()
        GetAnswers = db.getdataset(sql)
    End Function

    Public Function GetAnswersMatch(ByVal questionSetId As String) As DataSet
        Dim sql As String
        sql = "select a.answer_name, a.answer_score from tblanswer a,tblQuestion q where q.qSet_id = '" & questionSetId.CleanSQL & "' and a.Question_Id = q.Question_Id order by answer_no;"

        Dim db As New ClassConnectSql()
        GetAnswersMatch = db.getdataset(sql)
    End Function
    Public Function GetAnswersTrueFalse(ByVal questionId As String) As DataSet
        Dim sql As String
        sql = "select answer_name, answer_score from tblanswer where question_id = '" & questionId.CleanSQL & "' and Answer_Score = 1 and IsActive = 1 order by answer_no;"

        Dim db As New ClassConnectSql()
        GetAnswersTrueFalse = db.getdataset(sql)
    End Function


    'Public Function GetQuestionDetail(ByVal questionId As String) As String
    '    Dim sql As String
    '    sql = "select questiondetail from tblquestion where questionid = '" & questionId & "' order by questionid;"
    '    Dim dt As System.Data.DataTable
    '    dt = _DB.getdata(sql)
    '    If dt.Rows.Count > 0 Then
    '        Return dt.Rows(0)("answerdetail").ToString()
    '    End If
    '    Return ""
    'End Function
    Public Function CreateNewEmptyTestSet(ByVal userId As String) As String
        Dim sql As String
        Dim newTestSetID As String = ""
        sql = "select newid()"

        Dim db As New ClassConnectSql()
        Try
            newTestSetID = db.ExecuteScalar(sql).ToString()

            'logsuccess

            ' ให้ default level = ป.4 ไว้ก่อน, เพราะใช้ font size ขนาด กลางๆ 
            Dim KnSession As New KNAppSession()
            Dim CurrentCalendarId As String = KnSession("CurrentCalendarId").ToString()

            sql = " INSERT INTO tblTestSet (testset_id, testset_name, userid, schoolid, level_id, testset_time,isactive,IsPracticeMode,IsHomeworkMode,IsReportMode,IsQuizMode,IsStandard,NeedConnectCheckmark,Calendar_Id) values ("
            sql = sql + " '" & newTestSetID.CleanSQL & "', N'กำลังอยู่ระหว่างการจัดชุด', '" & userId.CleanSQL & "', (select top 1 schoolid from tbluser where Guid = '" & userId.CleanSQL & "' and isactive='1'), 'DD73B147-B098-4F1D-8144-C5FCF510AEA9', '60','1',0,0,0,0,0,0,'" & CurrentCalendarId.CleanSQL & "'); "

            Try
                db.Execute(sql)
                'logsuccess
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return ""
                'logerror
            End Try
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
            'logerror
        End Try



        Return newTestSetID
    End Function
    Public Sub DeleteUnusedTestSetQuestionSet()
        'Dim strSQL As String = "delete from tblTestSetQuestionSet where TSQS_Id not in (select TSQS_Id from tblTestSetQuestionDetail )"
        'Dim strSQL As String = "Update tblTestSetQuestionSet Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where TSQS_Id not in (select TSQS_Id from tblTestSetQuestionDetail where IsActive = '0' )"

        Dim strSQL As String = "update tbltestsetQuestionset set IsActive = 0 where tsqs_Id in ("

        strSQL &= " select AllQuestion.tsqs_id from (select tsqs_id , count(tsqd_id) as AllQuestionAmount from  tbltestsetquestiondetail group by tsqs_id ) "
        strSQL &= " as AllQuestion,(select tsqs_id , count(tsqd_id) as UnActiveAmount from  tbltestsetquestiondetail where isactive = 0 group by tsqs_id) "
        strSQL &= " as UnActiveQuestion where AllQuestion.tsqs_id = UnActiveQuestion.tsqs_id"
        strSQL &= " Group by AllQuestionAmount,UnActiveAmount,AllQuestion.tsqs_id having AllQuestionAmount = UnActiveAmount) and isactive = 1"

        Dim db As New ClassConnectSql()

        Try
            db.Execute(strSQL)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try
    End Sub
    Public Sub DeleteAllWhenReChkbox(ByVal QSetId As String, ByVal testSetId As String)
        Dim db As New ClassConnectSql()

        Dim strSQL As String = ""
        Dim TSQS_Id As String = ""

        strSQL = "select top 1 tsqs_id from tblTestSetQuestionSet where QSet_Id = '" & QSetId.CleanSQL & "' and TestSet_Id = '" & testSetId.CleanSQL & "';"
        TSQS_Id = db.ExecuteScalar(strSQL).ToString()

        If TSQS_Id <> "" Then
            'strSQL = "delete from tblTestSetQuestionDetail where tsqs_id = '" & TSQS_Id & "' ;"
            'strSQL &= "delete from tblTestSetQuestionSet where tsqs_id = '" & TSQS_Id & "' ;"

            strSQL = "Update tblTestSetQuestionDetail Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where tsqs_id = '" & TSQS_Id.CleanSQL & "' ;"
            strSQL &= "Update tblTestSetQuestionSet Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where tsqs_id = '" & TSQS_Id.CleanSQL & "' ;"

            Try
                db.Execute(strSQL)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End If

    End Sub

    'Public Function SaveTestSet(ByVal setName As String, ByVal setTime As String, ByVal levelId As String, ByVal testSetID As String, ByVal newTestSetFontSize As String, ByVal IsPracticeMode As String)
    Public Function SaveTestSet(ByVal setName As String, ByVal testSetID As String, ByVal IsQuizMode As Boolean, ByVal IsHomeWorkMode As Boolean, ByVal IsPracticeMode As Boolean, ByVal IsReportMode As Boolean)
        Dim strSQL As String = ""
        Dim db As New ClassConnectSql()
        Dim c As New SqlConnection
        db.OpenExclusiveConnect(c)

        Dim GSName As Object = GetGroupSubjectName(testSetID, c)
        'strSQL = "update tbltestset set testset_name = '" & db.CleanString(setName) & "', level_id='" & levelId & "', testset_time = '" & db.CleanString(setTime) & "' , TestSet_FontSize = '" & newTestSetFontSize & "', IsPracticeMode = '" & IsPracticeMode & "', isactive='1', lastupdate=dbo.GetThaiDate() where testset_id = '" & testSetID & "'"
        If ClsKNSession.RunMode = "wordonly" Then
            strSQL = " update tbltestset set testset_name = N'" & setName.CleanSQL & "', isactive='1', lastupdate=dbo.GetThaiDate(),ClientId = NULL,IsQuizMode = '0' , IsHomeWorkMode = '0' , IsPracticeMode = '0' , IsReportMode = '1', "
            strSQL &= " GroupSubject_Name = N'" & GSName.GroupSubject_Name.ToString.CleanSQL & "' ,GroupSubject_ShortName = N'" & GSName.GroupSubject_ShortName.ToString.CleanSQL & "' "
            strSQL &= " WHERE testset_id = '" & testSetID.CleanSQL & "';"
        Else
            strSQL = " update tbltestset set testset_name = N'" & setName.CleanSQL & "', isactive='1', lastupdate=dbo.GetThaiDate(),ClientId = NULL,IsQuizMode = '" & IsQuizMode & "' , IsHomeWorkMode = '" & IsHomeWorkMode & "' , IsPracticeMode = '" & IsPracticeMode & "' , IsReportMode = '" & IsReportMode & "', "
            strSQL &= " GroupSubject_Name = N'" & GSName.GroupSubject_Name.ToString.CleanSQL & "' ,GroupSubject_ShortName = N'" & GSName.GroupSubject_ShortName.ToString.CleanSQL & "' "
            strSQL &= " WHERE testset_id = '" & testSetID.CleanSQL & "';"
        End If
        db.Execute(strSQL, c)

        strSQL = "select tsqs_id from tblTestSetQuestionSet where testset_id = '" & testSetID.CleanSQL & "' and isactive='1' and tsqs_no = '-1' order by lastupdate asc; "
        Dim dt As DataTable
        dt = db.getdata(strSQL, , c)
        Dim tsqs_no As Integer = 1
        For Each r In dt.Rows
            strSQL = "update tblTestSetQuestionSet set tsqs_no = '" & tsqs_no.ToString().CleanSQL & "' , isactive = '1', lastupdate = dbo.GetThaiDate(),ClientId = NULL where tsqs_id = '" & r(0).ToString().CleanSQL & "'; "
            db.Execute(strSQL, c)
            tsqs_no = tsqs_no + 1
        Next
        Dim tsqd_no As Integer = 1
        Dim strSQL2 As String = ""
        Dim dt2 As DataTable
        For Each r In dt.Rows
            'strSQL2 = "select tsqd_id from tblTestSetQuestionDetail where tsqs_id = '" & r(0).ToString() & "' and tsqd_no = '-1' and isactive = '0' order by lastupdate"
            strSQL2 = " SELECT tsqd.TSQD_Id FROM tblTestSetQuestionDetail tsqd LEFT JOIN tblQuestion tq ON tsqd.Question_Id = tq.Question_Id  "
            strSQL2 &= " WHERE tsqd.TSQS_Id = '" & r(0).ToString().CleanSQL & "' AND tsqd.TSQD_No = '-1' AND tsqd.IsActive = '1' ORDER BY tq.Question_No "
            dt2 = db.getdata(strSQL2, , c)
            For Each r2 In dt2.Rows
                strSQL = "update tblTestSetQuestionDetail set tsqd_no = '" & tsqd_no.ToString().CleanSQL & "' , isactive = '1', lastupdate = dbo.GetThaiDate() where tsqd_id = '" & r2(0).ToString().CleanSQL & "'; "
                db.Execute(strSQL, c)
                tsqd_no = tsqd_no + 1
            Next
        Next

        db.CloseExclusiveConnect(c)
        'ยังมีบั๊ก , ถ้าเค้ากดดู pdf แล้ว, มาเพิ่ม - ลบ choice ใหม่, จะต้อง รันเลขที่ใหม่หมดจด เลย ตั้งแต่ต้นเลย ดังนั้นพวก where QD/QS_NO = -1 จะใช้ไม่ได้แล้ว เนื่องจากมันเกิดการ update เลขรันลงไปแล้ว

    End Function

    Public Function GetGroupSubjectName(ByVal TestsetId As String, Optional ByRef InputConn As SqlConnection = Nothing) As Object
        Dim sql As New StringBuilder()
        sql.Append(" SELECT Distinct tgs.GroupSubject_Name, tgs.GroupSubject_ShortName,tgs.GroupSubject_Id ")
        sql.Append(" FROM tblTestSetQuestionSet tsqs INNER JOIN tblQuestionSet tqs ")
        sql.Append(" ON tsqs.QSet_Id = tqs.QSet_Id INNER JOIN tblQuestionCategory tqc ")
        sql.Append(" ON tqs.QCategory_Id = tqc.QCategory_Id INNER JOIN tblBook tb ")
        sql.Append(" ON tqc.Book_Id = tb.BookGroup_Id INNER JOIN tblGroupSubject tgs ")
        sql.Append(" ON tb.GroupSubject_Id = tgs.GroupSubject_Id ")
        sql.Append(" WHERE tsqs.Testset_id = '")
        sql.Append(TestsetId)
        sql.Append("' and tsqs.IsActive = '1';")
        Dim dt As DataTable = New ClassConnectSql().getdata(sql.ToString(), , InputConn)
        Dim GroupName As Object
        If dt.Rows.Count = 1 Then
            'GroupName = New With {.GroupSubject_Name = dt.Rows(0)("GroupSubject_Name").ToString(), .GroupSubject_ShortName = dt.Rows(0)("GroupSubject_ShortName").ToString()}
            GroupName = New With {.GroupSubject_Name = dt.Rows(0)("GroupSubject_Id").ToString().ToGroupSubjectThName, .GroupSubject_ShortName = dt.Rows(0)("GroupSubject_Id").ToString().ToSubjectShortThName}
        ElseIf dt.Rows.Count > 1 Then
            GroupName = New With {.GroupSubject_Name = "รวมวิชา", .GroupSubject_ShortName = "รวมวิชา"}
        Else
            GroupName = New With {.GroupSubject_Name = "", .GroupSubject_ShortName = ""}
        End If
        Return GroupName
    End Function


    Public Function SaveSelectedQuestion(ByVal isEdit As String, ByVal needRemove As Boolean, ByVal qSetId As String, ByVal testSetID As String, ByVal NeedJoinQ40 As Boolean, ByVal classId As String) As String

        Dim strSQL As String
        If NeedJoinQ40 Then
            strSQL = "select q4.question_id from tblquestion40 q4,tblQuestion q where q.qset_id = '" & qSetId.CleanSQL & "' and q4.Question_Id = q.Question_Id"
            strSQL &= " and q4.isactive = '1';"
        Else
            strSQL = "select q.question_id from tblQuestion q where q.qset_id = '" & qSetId.CleanSQL & "' and q.isactive = '1';"
        End If



        Dim db As New ClassConnectSql()
        Dim dt As DataTable
        Dim retval As String = "OK"
        Try

            dt = db.getdata(strSQL)
            If dt.Rows.Count > 0 Then
                For Each r In dt.Rows
                    If SaveSelectedQuestion(isEdit, r(0).ToString(), needRemove, qSetId, testSetID, classId) <> "OK" Then
                        retval = "failed"
                    End If
                Next
            End If

            Call DeleteUnusedTestSetQuestionSet()
            SaveSelectedQuestion = retval

            Log.Record(Log.LogType.ExamStep, "เลือกหน่วยการเรียนรู้ทั้งชุด (QsetId=" & qSetId & ")", True)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            SaveSelectedQuestion = retval
        End Try
    End Function

    Public Sub DeleteQuestionforDeletedSubject(ByVal testsetId As String)

        Dim sql As String
        Dim db As New ClassConnectSql()
        Dim SubjectId As String
        Dim ClassId As String
        Dim CheckFirstStep As Boolean = True
        For Each a In SelectedSubjectClass
            ClassId = a.ClassId.ToString
            SubjectId = a.SubjectId.ToString
            'ClassId = ChangeClassAndSubject(a.ClassId.ToString, "None")
            'SubjectId = ChangeClassAndSubject("None", a.SubjectId.ToString)

            If CheckFirstStep Then
                'sql = "delete tblTestSetQuestionDetail where TSQS_Id in("
                sql = "Update tblTestSetQuestionDetail Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where TSQS_Id in("
                sql &= " select tsqs_Id from tblTestSetQuestionSet where TestSet_Id = '" & testsetId.CleanSQL & "' "
                sql &= " and QSet_Id  in (select QSet_Id from tblQuestionSet where QCategory_Id in ("
                sql &= " select QCategory_Id from tblQuestionCategory "
                sql &= " where Book_Id not in (select BookGroup_Id from tblbook where GroupSubject_Id in ('" & SubjectId.CleanSQL & "')"
                sql &= " and Level_Id in('" & ClassId.CleanSQL & "'))"
                CheckFirstStep = False
            Else
                sql &= " and Book_Id not in (select BookGroup_Id from tblbook where GroupSubject_Id in ('" & SubjectId.CleanSQL & "')"
                sql &= " and Level_Id in('" & ClassId.CleanSQL & "')) "
            End If
        Next

        sql &= ")))"

        db.Execute(sql)

        Dim sqlUpdateDetail = sql.Replace("tblTestSetQuestionDetail", "tblTestSetQuestionset")
        db.Execute(sqlUpdateDetail)



    End Sub

    Public Function ChangeClassAndSubject(ByVal ClassID As String, ByVal SubjectId As String) As String

        '    If Not ClassID = "None" Then
        '        Dim levelId As String = ""
        '        If ClassID = "4" Then
        '            levelId = "5F4765DB-0917-470B-8E43-6D1C7B030818"
        '        ElseIf ClassID = "5" Then
        '            levelId = "EFA0855F-7AA5-40C1-98D0-F332F1298CEE"
        '        ElseIf ClassID = "6" Then
        '            levelId = "5CAF2A9B-B26B-4C16-9980-90BA760B5C43"
        '        ElseIf ClassID = "7" Then
        '            levelId = "DD73B147-B098-4F1D-8144-C5FCF510AEA9"
        '        ElseIf ClassID = "8" Then
        '            levelId = "BCBCC0C8-2A39-4AAE-9AA6-173DE86AF6AE"
        '        ElseIf ClassID = "9" Then
        '            levelId = "93B163B6-4F87-476D-8571-4029A6F34C84"
        '        ElseIf ClassID = "10" Then
        '            levelId = "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C"
        '        ElseIf ClassID = "11" Then
        '            levelId = "DB95E7F8-7BF3-468D-AD9E-0AAF1B328D45"
        '        ElseIf ClassID = "12" Then
        '            levelId = "14A28F3D-1AFF-429D-B7A1-927A28E010BD"
        '        ElseIf ClassID = "13" Then
        '            levelId = "2E0FFC04-BCEE-45BE-9C0C-B40742523F43"
        '        ElseIf ClassID = "14" Then
        '            levelId = "6736D029-6B78-4570-9DBB-991217DA8FEE"
        '        ElseIf ClassID = "15" Then
        '            levelId = "6BF52DC7-314C-40ED-B7F3-BCC87F724880"
        '        Else
        '            'ดักไว้ก่อน, กรณีเลือก อ.1-2-3 มา มันจะไม่มีหนังสือแบบทดสอบให้ใช้ , มันจะส่งค่า levelid = '' ค่าว่างไป ซึ่งจะเกิด error 
        '            levelId = "6BF52DC7-314C-40ED-B7F3-BCC87F724880"
        '        End If

        '        Return levelId
        '    End If

        If Not SubjectId = "None" Then
            Dim groupSubjectId As String = ""
            If SubjectId = "1" Then
                groupSubjectId = "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
            ElseIf SubjectId = "2" Then
                groupSubjectId = "FDA224D9-CEBE-4642-ACD0-D7F7282E36AE"
            ElseIf SubjectId = "3" Then
                groupSubjectId = "A4B9F5CB-2D3C-4F6A-8666-FD2620E69723"
            ElseIf SubjectId = "4" Then
                groupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F"
            ElseIf SubjectId = "5" Then
                groupSubjectId = "FB677859-87DA-4D8D-A61E-8A76566D69D8"
            ElseIf SubjectId = "6" Then
                groupSubjectId = "6A4A7294-F5A7-4D64-ADBC-73DC14377737"
            ElseIf SubjectId = "7" Then
                groupSubjectId = "73C4639B-267C-4B7E-B5A4-1B4EBB428019"
            ElseIf SubjectId = "8" Then
                groupSubjectId = "47A224EF-3348-41B7-84D0-7250648F8706"
            Else
                'ดักไว้ก่อน, กรณีเลือก วิชา ที่ไม่เคย set ค่าไว้ (วิชาที่ 9?) ถ้าหลุด case มาได้ เดี๋ยวจะ error ใน query  เพราะ groupsubjectid ='' 
                groupSubjectId = "47A224EF-3348-41B7-84D0-7250648F8706"
            End If
            Return groupSubjectId
        End If
    End Function

    Public Function SaveSelectedQuestion(ByVal IsEdit As String, ByVal questionId As String, ByVal needRemove As Boolean, ByVal qSetId As String, ByVal testSetID As String, ByVal classId As String) As String

        Dim strSQL As String = ""
        Dim db As New ClassConnectSql()
        Dim TSQS_Id As String = ""
        'Dim currentTSQS_Id As String = ""
        Dim recordCount As Integer = 0

        If Not needRemove Then
            'เช็คก่อน ว่าเคยเลือก qsetid นี้มาก่อนในการจัดชุดครั้งนี้แล้วหรือยัง 
            strSQL = "select tsqs_id from tbltestsetquestionset where qset_id = '" & qSetId.CleanSQL & "' and testset_id = '" & testSetID.CleanSQL & "';"

            Dim dt As DataTable
            dt = db.getdata(strSQL)
            If dt.Rows.Count = 0 Then
                'ยังไม่มี ก็สร้าง รหัสใหม่ก่อน, 
                strSQL = "select newid();"
                TSQS_Id = db.ExecuteScalar(strSQL).ToString()
                'แล้วเพิ่ม row ใหม่
                strSQL = "insert into tbltestsetquestionset (tsqs_id, testset_id, qset_id, tsqs_no, isactive, Level_Id) values ("
                strSQL = strSQL + " '" & TSQS_Id.CleanSQL & "', '" & testSetID.CleanSQL & "', '" & qSetId.CleanSQL & "', '-1', '1','" & classId.CleanSQL & "');"
                db.Execute(strSQL)
            Else

                TSQS_Id = dt.Rows(0)(0).ToString()
                strSQL = "Update tblTestsetQuestionSet Set IsActive = '1',LastUpdate = dbo.GetThaiDate(), ClientId = NULL where TSQS_Id = '" & TSQS_Id.CleanSQL & "'"
                db.Execute(strSQL)

            End If

            strSQL = "select top 1 tsqd_id from tblTestsetQuestionDetail where tsqs_id = (select top 1 tsqs_id from tbltestsetQuestionset where testset_id = '" & testSetID.CleanSQL & "' )  and Question_id = '" & questionId.CleanSQL & "' ;"

            Dim dt2 As DataTable
            dt2 = db.getdata(strSQL)
            If dt2.Rows.Count = 0 Then
                strSQL = "insert into tblTestSetQuestionDetail (tsqd_id, TSQS_Id, Question_Id, tsqd_no, isactive) values (NEWID(), '" & TSQS_Id.CleanSQL & "', '" & questionId.CleanSQL & "','-1', '1')"
            Else
                Dim TSQD_Id As String = dt2.Rows(0)(0).ToString()
                strSQL = "Update tblTestsetQuestionDetail Set IsActive = '1',LastUpdate = dbo.GetThaiDate(), ClientId = NULL where TSQD_Id = '" & TSQD_Id.CleanSQL & "'"
                db.Execute(strSQL)
            End If

        Else ' เคยเลือกแล้วต้องการลบออก

            strSQL = "select top 1 tsqs_id from tbltestsetquestionset where qset_id = '" & qSetId.CleanSQL & "' and testset_id = '" & testSetID.CleanSQL & "';"
            TSQS_Id = db.ExecuteScalar(strSQL).ToString()
            If TSQS_Id = "" Then
                IsEdit = 0
                strSQL = "select top 1 tsqs_id from tbltestsetquestionset where qset_id = '" & qSetId.CleanSQL & "' and testset_id = '" & testSetID.CleanSQL & "';"
                TSQS_Id = db.ExecuteScalar(strSQL).ToString()
            End If

            'strSQL = "delete from tblTestSetQuestionDetail where question_id = '" & questionId & "' and tsqs_id = '" & TSQS_Id & "';"

            strSQL = "Update tblTestSetQuestionDetail Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where question_id = '" & questionId.CleanSQL & "' and tsqs_id = '" & TSQS_Id.CleanSQL & "';"

        End If

        Try

            db.Execute(strSQL)
            SaveSelectedQuestion = "OK"

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            SaveSelectedQuestion = "failed"
        End Try
    End Function
    Public Function GetAllUnit(ByVal classId As String, ByVal subjectId As String, ByVal NeedJoinQ40 As Boolean, IsSelectEvalutionIndex As Boolean, Optional ByVal TestsetId As String = "") As DataSet

        Dim groupSubjectId As String = subjectId
        Dim db As New ClassConnectSql()
        If Not SelectedSyllabusYear = "" Then
            If Not SelectedSyllabusYear = "y51,y44" Then
                SelectedSyllabusYear = Right(SelectedSyllabusYear, 2)
                SelectedYear = SelectedSyllabusYear
            Else : SelectedYear = ""

            End If
        Else : SelectedYear = ""
        End If

        Dim Table As String
        If NeedJoinQ40 = True Then
            Table = "tblQuestion40"
        Else
            Table = "tblQuestion"
        End If

        If IsSelectEvalutionIndex Then
            If CountTestsetEvalutionIndex(TestsetId, groupSubjectId, classId) <= 0 Then
                IsSelectEvalutionIndex = False
            End If
        End If

        Dim strSQL As String

        If IsSelectEvalutionIndex Then
            strSQL = "SELECT 'a' AS QuestionSet, qc.QCategory_Name ,qs.QSet_Name, COUNT(q.Question_Id) AS NumberOfQuestions,
                        qs.QSet_Id AS QSetId, b.GroupSubject_Id, b.Level_Id,b.Book_Syllabus,qs.QSet_Type 
                        FROM " & Table & " q 
                        INNER JOIN tblQuestionSet qs ON q.QSet_Id = qs.QSet_Id 
                        INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id 
                        INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id 
                        INNER JOIN tblQuestionEvaluationIndexItem qei on q.Question_Id = qei.Question_Id 
                        INNER JOIN tblTestsetEvalutionIndex tei on qei.EI_Id = tei.EIID 
                        WHERE (b.Level_Id = '" & classId & "') AND (b.GroupSubject_Id = '" & groupSubjectId & "') 
                        AND (q.IsActive = '1') AND (qs.IsActive = 1) AND (qc.IsActive = 1) AND (b.IsActive = 1) 
                        And (qei.IsActive = 1) And (tei.IsActive = 1) And tei.TestsetId = '" & TestsetId & "' "
            If Not SelectedYear = "" Then
                strSQL &= " AND (b.Book_Syllabus = " & SelectedYear.CleanSQL & ")"
            End If

            strSQL = strSQL + " GROUP BY qc.QCategory_Name ,qs.QSet_Name, qs.QSet_Id, b.GroupSubject_Id, b.Level_Id,b.Book_Syllabus,qs.QSet_Type "
            If groupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F" Then
                strSQL &= " ,qc.QCategory_No "
            End If
            strSQL = strSQL + " ORDER BY "
            If groupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F" Then
                strSQL &= " qc.QCategory_No, "
            End If
            strSQL &= " qc.QCategory_Name ,qs.QSet_Name "
        Else
            strSQL = "SELECT 'a' AS QuestionSet, qc.QCategory_Name ,qs.QSet_Name, COUNT(q.Question_Id) AS NumberOfQuestions,
                        qs.QSet_Id AS QSetId, b.GroupSubject_Id, b.Level_Id,b.Book_Syllabus,qs.QSet_Type 
                        FROM " & Table & " q 
                        INNER JOIN tblQuestionSet qs ON q.QSet_Id = qs.QSet_Id 
                        INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id 
                        INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id 
                        WHERE (b.Level_Id = '" & classId & "') AND (b.GroupSubject_Id = '" & groupSubjectId & "') 
                        AND (q.IsActive = '1') AND (qs.IsActive = 1) AND (qc.IsActive = 1) AND (b.IsActive = 1)"

            If Not SelectedYear = "" Then
                strSQL &= " AND (b.Book_Syllabus = " & SelectedYear.CleanSQL & ")"
            End If

            strSQL = strSQL + " GROUP BY qc.QCategory_Name ,qs.QSet_Name, qs.QSet_Id, b.GroupSubject_Id, b.Level_Id,b.Book_Syllabus,qs.QSet_Type "
            If groupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F" Then
                strSQL &= " ,qc.QCategory_No "
            End If
            strSQL = strSQL + " ORDER BY "
            If groupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F" Then
                strSQL &= " qc.QCategory_No, "
            End If
            strSQL &= " qc.QCategory_Name ,qs.QSet_Name "
        End If


        Dim ds As DataSet
        ds = db.getdataset(strSQL)
        For Each r In ds.Tables(0).Rows
            r("QSet_Name") = objPDF.CleanSetNameText(r("QSet_Name"))
            r("QuestionSet") = "<b>" & r("QCategory_Name") & "</b> - " & r("QSet_Name")
        Next

        GetAllUnit = ds
    End Function

    Public Function CountTestsetEvalutionIndex(TestsetId As String, Optional ByVal GroupSubjectId As String = "", Optional ByVal classId As String = "") As Integer
        Dim db As New ClassConnectSql()
        Dim sql As String = ""

        If GroupSubjectId = "" Then
            sql = "select count(*) from tblTestsetEvalutionIndex tei where tei.TestsetId = '" & TestsetId & "' and tei.IsActive = 1"
        Else
            sql = "select count(*) from (select EI_Id from tblEvaluationIndexNew where Parent_Id in(select EI_Id from tblEvaluationIndexNew 
                                    where Parent_Id in (select EI_Id from tblEvaluationIndexNew 
                                    where EI_Id in(select EI_Id from tblEvaluationIndexSubject where Subject_Id = '" & GroupSubjectId & "' and IsActive = 1)))
                                    and Level_No = 3 and Level_Id = '" & classId & "')eiall inner join tblTestsetEvalutionIndex tei on eiall.EI_Id = tei.EIID 
                                    where tei.TestsetId = '" & TestsetId & "' and tei.IsActive = 1"
        End If

        Dim TestsetEvalutionIndexAmount As Integer = CInt(db.ExecuteScalar(sql))

        Return TestsetEvalutionIndexAmount
    End Function

    Public Function GetAllUnitByOrder(ByVal classId, ByVal groupSubjectId, ByVal NeedJoinQ40) As DataSet

        If Not SelectedSyllabusYear = "" Then
            If Not SelectedSyllabusYear = "y51,y44" Then
                SelectedSyllabusYear = Right(SelectedSyllabusYear, 2)
                SelectedYear = SelectedSyllabusYear
            Else : SelectedYear = ""
            End If
        Else : SelectedYear = ""
        End If

        Dim Table As String = If((NeedJoinQ40 = True), "tblQuestion40", "tblQuestion")

        Dim strSQL As String = "SELECT 'a' AS QuestionSet, tblQuestionCategory.QCategory_Name ,tblQuestionSet.QSet_Name, COUNT(" & Table & ".Question_Id) AS NumberOfQuestions, "
        strSQL = strSQL + " tblQuestionSet.QSet_Id AS QSetId, tblBook.GroupSubject_Id, tblBook.Level_Id,tblBook.Book_Syllabus,tblQuestionset.QSet_Type "
        strSQL = strSQL + " ,tblQuestionCategory.QCategory_No,tblQuestionset.QSet_No "
        strSQL = strSQL + " FROM " & Table & " INNER JOIN"
        strSQL = strSQL + " tblQuestionSet ON " & Table & ".QSet_Id = tblQuestionSet.QSet_Id INNER JOIN"
        strSQL = strSQL + " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN"
        strSQL = strSQL + " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id"
        strSQL = strSQL + " WHERE (tblBook.Level_Id = '" & classId & "') AND (tblBook.GroupSubject_Id = '" & groupSubjectId & "')"
        'strSQL = strSQL + " And (tblQuestionSet.QSet_IsRandomQuestion = '1')"
        strSQL = strSQL + " And (tblQuestionSet.IsActive = '1') AND (" & Table & ".IsActive = 1)"

        If Not SelectedYear = "" Then
            strSQL = strSQL + " And (tblBook.Book_Syllabus = " & SelectedYear.CleanSQL & ")"
        End If

        strSQL = strSQL + " GROUP BY tblQuestionCategory.QCategory_Name ,tblQuestionSet.QSet_Name, tblQuestionSet.QSet_Id, tblBook.GroupSubject_Id, tblBook.Level_Id,tblBook.Book_Syllabus,tblQuestionset.QSet_Type "
        strSQL &= ",tblQuestionCategory.QCategory_No,tblQuestionset.QSet_No"
        strSQL = strSQL + " ORDER BY tblQuestionCategory.QCategory_No,tblQuestionCategory.QCategory_Name,tblQuestionset.QSet_No ,tblQuestionSet.QSet_Name;"

        Dim db As New ClassConnectSql()
        Dim ds As DataSet
        ds = db.getdataset(strSQL)
        For Each r In ds.Tables(0).Rows
            r("QSet_Name") = objPDF.CleanSetNameText(r("QSet_Name"))
            r("QuestionSet") = "<b>" & r("QCategory_Name") & "</b> - " & r("QSet_Name")
        Next
        Return ds
    End Function

    Public Function GetSelectedExamAmount(ByVal testsetid As String, ByVal QSetId As String) As String
        'Dim sql As String = " select COUNT(Question_ID) as Amount from tblquestion where Question_Id in (select question_Id from tbltestsetquestiondetail "
        'sql &= " where tsqs_id in (select tsqs_id from tbltestsetquestionset where testset_id in (select testset_id from tblTestSet "
        'sql &= "where TestSet_Id = '" & testsetid & "' ))and QSet_Id = '" & QSetId & "')"
        'Optimize sql
        Dim sql As String = "select COUNT(TSQS_Id) as Amount from tbltestsetquestiondetail where tsqs_id in("
        sql &= " select tsqs_id from tbltestsetquestionset where testset_id = '" & testsetid.CleanSQL & "' and QSet_Id = '" & QSetId.CleanSQL & "') and IsActive = '1'"
        'Use Left Join
        'Dim sql As String = "SELECT COUNT(tbltestsetquestiondetail.TSQS_Id) AS Amount FROM tblTestSetQuestionDetail inner Join tblTestSetQuestionSet "
        'Sql &= "ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id "
        'sql &= "WHERE tblTestSetQuestionSet.TestSet_Id = '" & testsetid & "' AND tblTestSetQuestionSet.QSet_Id = '" & QSetId & "'"

        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)
        GetSelectedExamAmount = dt.Rows(0)("Amount")

    End Function

    Public Function GetSelectedSubject() As DataSet
        Dim dt As New System.Data.DataTable()
        dt.TableName = "Subjects"
        dt.Columns.Add("SubjectId", GetType(String))
        dt.Columns.Add("SubjectName", GetType(String))

        Dim alreadyAddedSubject As String = ""

        For Each j In SelectedSubjectClass

            'เคย add เข้า DT แล้ว ก็ไม่ต้อง add ซ้ำ                
            If Not alreadyAddedSubject.Contains("[" & j.SubjectId.ToString() & "]") Then
                dt.Rows.Add(j.SubjectId, j.SubjectName)
            End If
            alreadyAddedSubject = alreadyAddedSubject & "[" & j.SubjectId.ToString() & "]"
        Next

        Dim dt2 As New System.Data.DataTable()
        dt2.TableName = "Classes"
        dt2.Columns.Add("SubjectId", GetType(String))
        dt2.Columns.Add("ClassId", GetType(String))
        dt2.Columns.Add("ClassName", GetType(String))

        Dim alreadyAddedClass As String = ""
        For Each j In SelectedSubjectClass

            'เคย add เข้า DT แล้ว ก็ไม่ต้อง add ซ้ำ                
            If Not alreadyAddedClass.Contains("[" & j.SubjectId.ToString() & "-" & j.ClassId.ToString() & "]") Then
                dt2.Rows.Add(j.SubjectId, j.ClassId, j.ClassName)
            End If
            alreadyAddedClass = alreadyAddedClass & "[" & j.SubjectId.ToString() & "-" & j.ClassId.ToString() & "]"
        Next


        Dim ds As New DataSet
        ds.Tables.Add(dt)
        ds.Tables.Add(dt2)

        ds.Relations.Add(New DataRelation("SubjectToClass", ds.Tables(0).Columns("SubjectId"), ds.Tables(1).Columns("SubjectId")))

        Return ds
    End Function

    ''' <summary>
    ''' มีเวลา มาปรับ ให้ lookup จาก DB แล้ว built เป็น array ใช้ใน function นี้แทน hardcoded นะ
    ''' </summary>
    ''' <param name="SubjectName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSubjectIdByName(ByVal SubjectName As String) As String

        'If SubjectName = "ไทย" Then
        '    Return 1
        'ElseIf SubjectName = "สังคมฯ" Then
        '    Return 2
        'ElseIf SubjectName = "คณิตฯ" Then
        '    Return 3
        'ElseIf SubjectName = "วิทยาศาสตร์" Then
        '    Return 4
        'ElseIf SubjectName = "ภาษาอังกฤษ" Then
        '    Return 5
        'ElseIf SubjectName = "สุขศึกษาฯ" Then
        '    Return 6
        'ElseIf SubjectName = "ศิลปะ" Then
        '    Return 7
        'ElseIf SubjectName = "การงานฯ" Then
        '    Return 8
        'ElseIf SubjectName = "อังกฤษ" Then
        '    Return 5
        'Else
        '    Return 0
        'End If
        'Return 0

        Dim sql As String
        sql = "select tblGroupSubject.GroupSubject_Id from tblGroupSubject where GroupSubject_ShortName = '" & SubjectName & "';"
        Dim db As New ClassConnectSql()
        Dim GroupSubjectId As String = db.ExecuteScalar(sql)

        Return GroupSubjectId

    End Function
    ''' <summary>
    ''' มีเวลา มาปรับ ให้ lookup จาก DB แล้ว built เป็น array ใช้ใน function นี้แทน hardcoded นะ
    ''' </summary>
    ''' <param name="classId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClassNameById(ByVal classId As String) As String

        Dim sql As String
        sql = "SELECT Level_ShortName FROM  tblLevel  where Level_Id = '" & classId.CleanSQL & "' "
        Dim db As New ClassConnectSql()
        GetClassNameById = db.ExecuteScalar(sql)

    End Function
    Public Function MaxNumberOfSubject() As Integer

        Dim strSQL As String = "select isnull(max(countsubject),0) from uvw_CountSubjectPerClass where userid = '" & currentUserId.CleanSQL & "' ;"
        Dim db As New ClassConnectSql()
        MaxNumberOfSubject = db.ExecuteScalar(strSQL)
    End Function

    Public Function GetSelectedQuestionID(ByVal TestSetId As String, ByVal QuestionSetID As String, ByVal QuestionId As String) As String
        Dim sql As String
        If Not TestSetId = "" Then


            sql = "select count(tsqd.Question_Id) as id from tblTestSetQuestionDetail tsqd where tsqs_id in "
            sql &= "( select tsqs_id from tbltestsetquestionset where testset_id in (select testset_id from tblTestSet "
            sql &= "where TestSet_Id = '" & TestSetId.CleanSQL & "') and QSet_Id = '" & QuestionSetID.CleanSQL & "') "
            sql &= "and tsqd.Question_Id = '" & QuestionId.CleanSQL & "' and IsActive = '1'"

            Dim db As New ClassConnectSql()
            Dim dt As DataTable = db.getdata(sql)

            If dt.Rows(0)("id") = 0 Then
                GetSelectedQuestionID = String.Empty

            Else
                GetSelectedQuestionID = "True"

            End If
        Else
            Return String.Empty
        End If
    End Function

    Public Function GetSelectedYear(ByVal TestSetId As String, ByVal CheckYear As String) As Boolean
        Dim sql As String
        sql = " select distinct b.Book_Syllabus as Year from tblBook b where b.BookGroup_Id in (select Book_Id from tblQuestionCategory qc where "
        sql &= " qc.QCategory_Id in (select qs.QCategory_Id from tblQuestionSet qs where qs.isactive = 1 and qs.QSet_Id in (select QSet_Id from tblTestSetQuestionSet  "
        sql &= "where TestSet_Id = '" & TestSetId.CleanSQL & "'))) and b.Book_Syllabus = " & CheckYear.CleanSQL

        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sql)

        If dt.Rows.Count = 0 Then
            GetSelectedYear = "False" ' String.Empty
            Return False
        Else
            GetSelectedYear = "True"

        End If
        Return True
    End Function


    Public Function GetSelectedQuestionSet(ByVal TestSetId As String, ByVal QuestionSetID As String) As String

        If Not TestSetId = "" Then
            Dim sql As String
            Dim sqlUpdate As String
            Dim db As New ClassConnectSql()
            'เอา lastupdate ออก เนื่องจากไปกระทบกับ ลำดับของ qset ตอนเลือกทีละ qset ที่ต้องเรียงแบบ lastupdate
            sqlUpdate = "update tblTestSetQuestionDetail set TSQD_No='-1',ClientId = NULL where TSQS_Id in (select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & TestSetId & "' and IsActive = '1') and IsActive = '1'; "
            sqlUpdate &= "update tblTestSetQuestionSet set TSQS_No='-1',ClientId = NULL where TestSet_Id = '" & TestSetId.CleanSQL & "' and IsActive = '1';"
            db.Execute(sqlUpdate)
            sql = " select count(tsqs.QSet_Id) as id from tblTestSetQuestionSet tsqs where tsqs_id in "
            sql &= " (select tsqs_id from tbltestsetquestionset where testset_id in (select testset_id from tblTestSet "
            sql &= " where TestSet_Id = '" & TestSetId.CleanSQL & "')  "
            sql &= " and QSet_Id = '" & QuestionSetID.CleanSQL & "') and IsActive = '1';"


            Dim dt As DataTable = db.getdata(sql)

            If dt.Rows(0)("id") = 0 Then
                GetSelectedQuestionSet = String.Empty

            Else
                GetSelectedQuestionSet = "True"
            End If

        Else
            Return String.Empty
        End If

    End Function

    Public Sub CheckSallyBus(ByVal Testsetid As String, ByVal Syllabus As String)

        If Not Syllabus = "y51,y44" Then
            Dim selectedyear As String = Right(Syllabus, 2)
            Dim sql As String = "select distinct d.Question_Id from tblTestSetQuestionDetail d,tblBook  where TSQS_Id in (select TSQS_Id "
            sql &= " from tblTestSetQuestionSet where TestSet_Id = '" & Testsetid.CleanSQL & "' )"

            Dim db As New ClassConnectSql()
            Dim c As New SqlConnection
            db.OpenExclusiveConnect(c)
            Dim dt As DataTable = db.getdata(sql, , c)

            For Each r In dt.Rows
                sql = " select Book_Syllabus from tblBook b,tblQuestionCategory qc, tblQuestionSet qs, tblQuestion q"
                sql &= " where(qc.Book_Id = b.BookGroup_Id)"
                sql &= " and qc.QCategory_Id = qs.QCategory_Id"
                sql &= " and qs.QSet_Id = q.QSet_Id "
                sql &= " and q.Question_Id = '" & r("Question_Id").ToString.CleanSQL & "'"

                Dim dts As DataTable = db.getdata(sql, , c)

                If Not dts.Rows(0)("Book_Syllabus") Is DBNull.Value Then
                    If Not dts.Rows(0)("Book_Syllabus") = selectedyear Then

                        'sql = " delete tblTestSetQuestionDetail where TSQS_Id in "
                        'sql &= " (select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & Testsetid & "' ) and Question_Id = '" & r("Question_Id").ToString & "'"

                        sql = " Update tblTestSetQuestionDetail Set IsActive = '0',Lastupdate = dbo.GetThaiDate(),ClientId = NULL where TSQS_Id in "
                        sql &= " (select TSQS_Id from tblTestSetQuestionSet where TestSet_Id = '" & Testsetid.CleanSQL & "' ) and Question_Id = '" & r("Question_Id").ToString.CleanSQL & "'"
                        db.Execute(sql, c)

                    End If
                End If

            Next

            db.CloseExclusiveConnect(c)
        End If

    End Sub

    Public Function GetSelectedAmount(ByVal TestSetId As String) As String
        Dim sb As New StringBuilder
        sb.Append("select count(TSQD_Id) as QuestionAmount from tblTestSetQuestionDetail inner join tblTestSetQuestionSet ")
        sb.Append("on tblTestSetQuestionDetail.TSQS_Id = tblTestSetQuestionSet.TSQS_id ")
        sb.Append(" where TestSet_Id = '" & TestSetId & "' and tblTestSetQuestionDetail.IsActive = '1'")
        Dim db As New ClassConnectSql
        GetSelectedAmount = db.ExecuteScalar(sb.ToString)
    End Function

    Public Function GetTextForlblEditEachQuestion(ByVal QuestionId As String) As String

        Dim db As New ClassConnectSql
        Dim sql As String
        sql = " SELECT COUNT(*) FROM dbo.tblQuestion WHERE Question_Id = '" & QuestionId.CleanSQL & "' AND " &
               " ((Question_Expain IS NULL) OR (CAST(Question_Expain AS VARCHAR(max)) = '')) "
        '
        Dim CheckExplainQuestion As Integer = db.ExecuteScalar(sql)

        If CheckExplainQuestion = 0 Then
            GetTextForlblEditEachQuestion = "ทำแล้ว"
            HttpContext.Current.Session("currentcolor") = "Green"
            Return GetTextForlblEditEachQuestion
        End If

        sql = " SELECT COUNT(*) FROM dbo.tblAnswer WHERE Question_Id = '" & QuestionId.CleanSQL & "' " &
              " AND ((Answer_Expain IS not null) and (CAST(Answer_Expain AS VARCHAR(max)) <> ''))"
        '
        Dim CheckExplainAnswer As Integer = db.ExecuteScalar(sql)
        If CheckExplainAnswer >= 1 Then
            GetTextForlblEditEachQuestion = "ทำแล้ว"
            HttpContext.Current.Session("currentcolor") = "Green"
            Return GetTextForlblEditEachQuestion
        End If


        GetTextForlblEditEachQuestion = "ยังไม่ทำ"
        HttpContext.Current.Session("currentcolor") = "Red"

        Return GetTextForlblEditEachQuestion

    End Function

    Private Function GetLevelParent(ByVal EI_Id As String)

        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT Level_No FROM dbo.tblEvaluationIndexNew WHERE EI_Id = '" & EI_Id.CleanSQL & "' "
        Dim Level As String = _DB.ExecuteScalar(sql)
        _DB = Nothing
        Return Level

    End Function

    Public Function GetTextForlblEvaluationIndex(ByVal QuestionId As String) As String

        If HttpContext.Current.Session("ToTalEvaluationIndex") IsNot Nothing Then

            Dim db As New ClassConnectSql
            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuestionEvaluationIndexItem  WHERE Question_Id = '" & QuestionId.CleanSQL & "' And IsApproved = '1' AND IsActive = '1' "
            Dim IsArrove As Integer = db.ExecuteScalar(sql)

            If IsArrove > 0 Then
                Dim ResultAfterCheck As Boolean = False
                sql = " SELECT Count(*) FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IS NULL AND IsActive = 1 "
                Dim ResultEva As String = db.ExecuteScalar(sql)
                sql = " SELECT EI_Id FROM dbo.tblQuestionEvaluationIndexItem  WHERE Question_Id = '" & QuestionId.CleanSQL & "'  And IsApproved = '1' AND IsActive = '1' "
                Dim dtApprove As New DataTable
                dtApprove = db.getdata(sql)
                Dim ArrayCheck As New ArrayList
                If dtApprove.Rows.Count > 0 Then
                    For index = 0 To dtApprove.Rows.Count - 1
                        Dim CheckLevelParent As String = GetLevelParent(dtApprove.Rows(index)("EI_Id").ToString())
                        If CheckLevelParent = "2" Then
                            sql = " SELECT EI_Code FROM dbo.tblEvaluationIndexNew WHERE EI_Id IN ( " &
                                  " SELECT Parent_Id FROM dbo.tblEvaluationIndexNew WHERE EI_Id  IN ( " &
                                  " SELECT Parent_Id FROM dbo.tblEvaluationIndexNew WHERE EI_Id = '" & dtApprove.Rows(index)("EI_Id").ToString().CleanSQL & "')) " &
                                  " AND dbo.tblEvaluationIndexNew.IsActive = 1 "
                        ElseIf CheckLevelParent = "3" Then
                            sql = " SELECT EI_Code FROM dbo.tblEvaluationIndexNew WHERE EI_Id IN ( " &
                                  " SELECT Parent_Id FROM dbo.tblEvaluationIndexNew WHERE EI_Id IN ( " &
                                  " SELECT Parent_Id FROM dbo.tblEvaluationIndexNew WHERE EI_Id  IN ( " &
                                  " SELECT Parent_Id FROM dbo.tblEvaluationIndexNew WHERE EI_Id = '" & dtApprove.Rows(index)("EI_Id").ToString().CleanSQL & "' ))) " &
                                  " AND dbo.tblEvaluationIndexNew.IsActive = 1 "
                        End If
                        Dim EICode As String = db.ExecuteScalar(sql)
                        If index = 0 Then
                            ArrayCheck.Add(EICode)
                        Else
                            If ArrayCheck.Contains(EICode) = False Then
                                ArrayCheck.Add(EICode)
                            End If
                        End If
                    Next
                    If ResultEva = ArrayCheck.Count Then
                        ResultAfterCheck = True
                    Else
                        ResultAfterCheck = False
                    End If
                End If
                If ResultAfterCheck = True Then
                    GetTextForlblEvaluationIndex = "อนุมัติแล้วทั้งหมด"
                Else
                    GetTextForlblEvaluationIndex = "อนุมัติแล้วบางส่วน"
                End If
                HttpContext.Current.Session("currentcolor2") = "Green"
                Return GetTextForlblEvaluationIndex
            End If
            Dim IsEditAll As Integer = 0
            sql = " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IS NULL ORDER BY EI_Position "
            Dim dt As New DataTable
            dt = db.getdata(sql)
            If dt.Rows.Count > 0 Then
                Dim EachEIID As String = ""

                For index = 0 To dt.Rows.Count - 1
                    EachEIID = dt.Rows(index)("EI_Id").ToString()
                    If index = 0 Then
                        sql = " SELECT COUNT(*) FROM dbo.tblQuestionEvaluationIndexItem WHERE Question_Id = '" & QuestionId.CleanSQL & "' AND EI_Id IN ( " &
                              " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " &
                              " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " &
                              " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EachEIID.CleanSQL & "' ) ) ) AND IsActive = 1 "
                        Dim CheckResult As String = db.ExecuteScalar(sql)
                        If CInt(CheckResult) > 0 Then
                            IsEditAll += 1
                        End If
                    Else
                        sql = " SELECT COUNT(*) FROM dbo.tblQuestionEvaluationIndexItem WHERE " &
                              " Question_Id = '" & QuestionId.CleanSQL & "' AND EI_Id IN ( " &
                              " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN  " &
                              " (SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EachEIID.CleanSQL & "') ) AND IsActive = 1 "
                        Dim CheckResult As String = db.ExecuteScalar(sql)
                        If CInt(CheckResult) > 0 Then
                            IsEditAll += 1
                        End If
                    End If
                Next
            End If
            'Dim IsEditAll As Integer = db.ExecuteScalar(sql)
            If IsEditAll = HttpContext.Current.Session("ToTalEvaluationIndex") Then
                GetTextForlblEvaluationIndex = "ทำแล้วทุกอัน"
                HttpContext.Current.Session("currentcolor2") = "Black"
                Return GetTextForlblEvaluationIndex
            End If

            If IsEditAll > 0 Then
                GetTextForlblEvaluationIndex = "ทำบางส่วน"
                HttpContext.Current.Session("currentcolor2") = "Red"
                Return GetTextForlblEvaluationIndex
            Else
                GetTextForlblEvaluationIndex = "ยังไม่ทำ"
                HttpContext.Current.Session("currentcolor2") = "Red"
                Return GetTextForlblEvaluationIndex
            End If

            Return ""

        End If
    End Function

    ''' <summary>
    ''' ทำการหาชื่อชุดข้อสอบ
    ''' </summary>
    ''' <param name="Testset_Id">Id ของ tblTestset ที่ต้องการหาชื่อชุดข้อสอบ</param>
    ''' <returns>ชื่อชุดข้อสอบ</returns>
    ''' <remarks></remarks>
    Public Function GetTestsetName(ByVal Testset_Id As String) As DataTable
        Dim sql As String
        sql = "select * from tbltestset where testset_Id = '" & Testset_Id.ToString & "' AND UserId = '" & currentUserId & "';"

        Dim db As New ClassConnectSql()
        GetTestsetName = db.getdata(sql)
    End Function

    ''' <summary>
    ''' Update Testset
    ''' </summary>
    ''' <param name="Testset_ID">Id ของ tblTestset ของชุดที่เลือกมา</param>
    ''' <param name="TestsetName">ชื่อชุดข้อสอบ</param>
    ''' <param name="TestsetTime">เวลาในการทำข้อสอบ</param>
    ''' <param name="Level_Id">ชั้นที่เลือกมา</param>
    ''' <param name="NeedConnectCheckmark">ต้องการใช้กับ checkmark ด้วยรึเปล่า</param>
    ''' <remarks></remarks>
    Public Sub UpdateTestset(ByVal Testset_ID As String, ByVal TestsetName As String, ByVal TestsetTime As String, ByVal Level_Id As String, ByVal NeedConnectCheckmark As Integer, ByVal PrefixForRunningNoInWordFile As String, ByVal EnablePrefixForRunningNoInWordFile As Boolean)
        Dim db As New ClassConnectSql()
        Dim fontSize As Integer = GetFontSizeForGenWord(Level_Id)

        Dim prefixRunningNo As String = If(PrefixForRunningNoInWordFile = "", "AND PrefixForRunningNoInWordFile IS NULL", "AND PrefixForRunningNoInWordFile = '" & PrefixForRunningNoInWordFile.CleanSQL & "'")
        Dim queryWithPrefix As String = If(EnablePrefixForRunningNoInWordFile, prefixRunningNo, "")

        Dim dt As DataTable = db.getdata(String.Format("SELECT * FROM tblTestSet WHERE TestSet_Id = '{0}' AND TestSet_Name = '{1}' AND TestSet_FontSize = {2} AND TestSet_Time = {3}  {4};", Testset_ID.CleanSQL, TestsetName.CleanSQL, fontSize, TestsetTime.CleanSQL, queryWithPrefix))

        If dt.Rows.Count = 0 Then
            PrefixForRunningNoInWordFile = If(PrefixForRunningNoInWordFile = "", ",PrefixForRunningNoInWordFile = NULL", ",PrefixForRunningNoInWordFile = '" & PrefixForRunningNoInWordFile & "'")
            Dim updateWithPrefix As String = If(EnablePrefixForRunningNoInWordFile, PrefixForRunningNoInWordFile, "")
            Dim sql As String = " UPDATE tblTestset SET Testset_Name = N'" & TestsetName.CleanSQL & "',Testset_Time = '" & TestsetTime.CleanSQL & "',Level_Id = '" & Level_Id.CleanSQL & "',NeedConnectCheckmark = " & NeedConnectCheckmark & ",LastUpdate = dbo.GetThaiDate(),ClientId = NULL,TestSet_FontSize = " & fontSize & " " & updateWithPrefix & "  WHERE testset_Id = '" & Testset_ID.ToString.CleanSQL & "' AND UserId = '" & currentUserId.CleanSQL & "';"
            db.Execute(sql)
        End If
    End Sub

    Private Function GetFontSizeForGenWord(levelId As String) As Integer
        Select Case levelId
            Case "6BF52DC7-314C-40ED-B7F3-BCC87F724880"
                Return 0
            Case "93B163B6-4F87-476D-8571-4029A6F34C84"
                Return 1
            Case "5F4765DB-0917-470B-8E43-6D1C7B030818"
                Return 2
            Case Else
                Return 0
        End Select
    End Function

    Public Function CheckSelectedExam(ByVal Testset_Id As String)
        Dim sql = "select Count(tsqs_id) as SelectedAmount from tbltestsetquestionset where isactive = 1 "
        sql &= " and testset_Id = '" & Testset_Id & "'"

        Dim db As New ClassConnectSql()
        Dim SelectedAmount = db.ExecuteScalar(sql)

        If SelectedAmount = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function CutStringAndReturn50Alphabet(ByVal QSetId) As String
        Dim clsData As New ClassConnectSql
        Dim dtQuestionSet As New DataTable

        Dim sqlQuestionSet As String = "Select qs.QSet_Name,qc.QCategory_Name from tblQuestionSet qs,tblQuestionCategory qc Where qs.QCategory_Id = qc.QCategory_Id and qs.QSet_Id = '" & QSetId.ToString().CleanSQL & "'"
        'dtQuestionSet = New DataTable
        dtQuestionSet = clsData.getdata(sqlQuestionSet)

        Dim QCategoryName As String = dtQuestionSet.Rows(0)("QCategory_Name")
        Dim QuestionSetName As String = dtQuestionSet.Rows(0)("QSet_Name")

        Dim CheckBrOld As Boolean = QuestionSetName.Contains("<br>")
        Dim CheckBrNew As Boolean = QuestionSetName.Contains("<br />")

        Dim strFormatReturn As String = "<b>{0}</b> - {1}<span id='SpanMore' style='color: #2370FA;'> ...ดูเพิ่มเติม</span>"
        If QuestionSetName.ToString.Length > 50 Then
            If CheckBrNew = False And CheckBrOld = False Then  ' ไม่มี <br> และ <br />     
                QuestionSetName = Regex.Replace(QuestionSetName, "<.*?>", "")
                Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
            ElseIf CheckBrNew = False And CheckBrOld = True Then ' มี <br>
                If QuestionSetName.IndexOf("<br>") < 50 Then
                    QuestionSetName = QuestionSetName.Replace("<br>", "&nbsp;")
                    If QuestionSetName.IndexOf("&nbsp;") > 44 And QuestionSetName.IndexOf("&nbsp;") < 51 Then
                        QuestionSetName = QuestionSetName.Replace("&nbsp;", "      ")
                    End If
                End If
                QuestionSetName = Regex.Replace(QuestionSetName, "<.*?>", "")
                Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
            ElseIf CheckBrNew = True And CheckBrOld = False Then 'มี <br />
                If QuestionSetName.IndexOf("<br />") < 50 Then
                    QuestionSetName = QuestionSetName.Replace("<br />", "&nbsp;")
                    If QuestionSetName.IndexOf("&nbsp;") > 44 And QuestionSetName.IndexOf("&nbsp;") < 51 Then
                        QuestionSetName = QuestionSetName.Replace("&nbsp;", "      ")
                    End If
                End If
                QuestionSetName = Regex.Replace(QuestionSetName, "<.*?>", "")
                Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
            Else ' มี <br> และ <br />
                QuestionSetName = QuestionSetName.Replace("<br>", "&nbsp;").Replace("<br />", "&nbsp;")
                If QuestionSetName.IndexOf("&nbsp;") > 44 And QuestionSetName.IndexOf("&nbsp;") < 51 Then
                    QuestionSetName = QuestionSetName.Replace("&nbsp;", "      ")
                End If
                QuestionSetName = Regex.Replace(QuestionSetName, "<.*?>", "")
                Return String.Format(strFormatReturn, QCategoryName, QuestionSetName.Substring(0, 50))
            End If
        End If
        Return String.Format(strFormatReturn, QCategoryName, QuestionSetName)
    End Function

    Public Function GetClassAndSubjectFromQuizId(Quiz_Id As String) As DataTable
        Dim dt As New DataTable
        Dim db As New ClassConnectSql()
        Dim sb As New StringBuilder
        '        select tbllevel.Level_Id , tblGroupSubject.GroupSubject_Id from tbllevel inner join tblbook on tbllevel.Level_Id = tblBook.Level_Id 
        'inner join tblGroupSubject on tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id inner join tblQuestionCategory on tblbook.Bookgroup_Id = tblQuestionCategory.Book_Id
        'inner join tblquestionset on tblQuestionCategory.QCategory_Id = tblquestionset.QCategory_Id inner join tblTestSetQuestionSet on tblquestionset.QSet_Id = tblTestSetQuestionSet.QSet_Id
        'inner join tblquiz on tblTestSetQuestionSet.TestSet_Id = tblQuiz.TestSet_Id where Quiz_Id = 'E901A585-5DCA-4F86-A118-05B45FD79998'

        sb.Append("select tbllevel.Level_Id , tblGroupSubject.GroupSubject_Id from tbllevel inner join tblbook on tbllevel.Level_Id = tblBook.Level_Id ")
        sb.Append(" inner join tblGroupSubject on tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id")
        sb.Append(" inner join tblQuestionCategory on tblbook.Bookgroup_Id = tblQuestionCategory.Book_Id")
        sb.Append(" inner join tblquestionset on tblQuestionCategory.QCategory_Id = tblquestionset.QCategory_Id")
        sb.Append(" inner join tblTestSetQuestionSet on tblquestionset.QSet_Id = tblTestSetQuestionSet.QSet_Id")
        sb.Append(" inner join tblquiz on tblTestSetQuestionSet.TestSet_Id = tblQuiz.TestSet_Id")
        sb.Append(" where Quiz_Id = '" & Quiz_Id.CleanSQL & "'")

        dt = db.getdata(sb.ToString)

        Return dt
    End Function

    Public Sub UpdateTempQuestion()
        Try
            Dim sql As String = "Update tblquestion set isactive = 0 where (Question_Name like '' or Question_Name like 'คำถามใหม่')"
            sql &= " and IsActive = 1 and LastUpdate < DATEADD(minute,-30,dbo.getthaidate());"
            Dim db As New ClassConnectSql()
            db.Execute(sql)
        Catch ex As Exception
            Log.Record(Log.LogType.ExamStep, "Update ข้อสอบที่ค้างไม่สำเร็จ", False)
        End Try
    End Sub

    Public Function CreateExportDataTable(TableName As String) As DataTable
        Dim ClsConnect As New ClassConnectSql()
        Dim sql As String = "Select * from " & TableName & ";"
        Dim dt As DataTable = ClsConnect.getdata(sql)
        Return dt
    End Function

    Public Function GetEvalutionByQuestion(Question_Id As String) As DataTable
        Dim ClsConnect As New ClassConnectSql()
        Dim sql As String
        sql = " select Evalution,EName1, EName2,EName3 from (select 'ตัวชี้วัด' as Evalution, " &
            " (case when EN1.EI_Code is null then '' else EN1.EI_Code end) + ' ' + (case when EN1.EI_Name is null then '' else EN1.EI_Name end) as EName1," &
            " EName2,EName3,'1' as EI_Position from tblEvaluationIndexNew EN1 inner join  (" &
            " select (case when EN2.EI_Code is null then '' else EN2.EI_Code end) + ' ' + (case when EN2.EI_Name is null then '' else EN2.EI_Name end) as EName2," &
            " EName3,en2.Parent_Id from tblEvaluationIndexNew EN2 inner join (" &
            " select (case when EN.EI_Code is null then '' else EN.EI_Code end) + ' ' + (case when EN.EI_Name is null then '' else EN.EI_Name end) as EName3,Parent_Id" &
            " from tblQuestionEvaluationIndexItem EI inner join tblEvaluationIndexNew EN on EI.EI_Id = EN.EI_Id" &
            " inner join tblQuestion Q on EI.Question_Id = Q.Question_Id where EI.Question_Id = '" & Question_Id & "' and EN.IsActive = 1 and EI.IsActive = 1)EN3" &
            " on en2.EI_Id = en3.Parent_Id)EN2 on EN1.EI_Id = en2.Parent_Id where EN1.Parent_Id Is Not null" &
            " union" &
            " select (case when EN1.EI_Code is null then '' else EN1.EI_Code end) + ' ' + (case when EN1.EI_Name is null then '' else EN1.EI_Name end) as Evalution," &
            " EName2 as EName1,EName3 as EName2, Null as EName3,EI_Position from tblEvaluationIndexNew EN1 inner join (" &
            " select (case when EN2.EI_Code is null then '' else EN2.EI_Code end) + ' ' + (case when EN2.EI_Name is null then '' else EN2.EI_Name end) as EName2,EName3,en2.Parent_Id" &
            " from tblEvaluationIndexNew EN2 inner join (select (case when EN.EI_Code is null then '' else EN.EI_Code end) + ' ' + (case when EN.EI_Name is null then '' else EN.EI_Name end) as EName3,Parent_Id" &
            " from tblQuestionEvaluationIndexItem EI inner join tblEvaluationIndexNew EN on EI.EI_Id = EN.EI_Id" &
            " inner join tblQuestion Q on EI.Question_Id = Q.Question_Id where EI.Question_Id = '" & Question_Id & "'" &
            " and EN.IsActive = 1 and EI.IsActive = 1)EN3 on en2.EI_Id = en3.Parent_Id)EN2 on EN1.EI_Id = en2.Parent_Id" &
            " where EN1.Parent_Id is null)AllEvalution order by EI_Position"

        Dim dt As DataTable = ClsConnect.getdata(sql)
        Return dt
    End Function

    Public Function GetDetailQuestionName(Question_Id As String) As DataTable
        Dim ClsConnect As New ClassConnectSql()
        Dim sql As String
        sql = " select Question_No,QSet_Name,QCategory_Name,Question_Name from tblquestion Q inner join tblQuestionSet QS on Q.QSet_Id = QS.QSet_Id " &
              "inner join tblQuestionCategory QC on QC.QCategory_Id = QS.QCategory_Id where Question_Id = '" & Question_Id & "';"
        Dim dt As DataTable = ClsConnect.getdata(sql)
        Return dt
    End Function

    Public Function SaveMultimediaFile(QsetId As String, RefId As String, MFileName As String, MFileType As String, RefType As String) As Boolean
        Try
            Dim db As New ClassConnectSql()
            Dim sql As String = "select top 1 MultimediaObjId from tblMultimediaObject where referenceId = '" & RefId & "' and MFileNAme = '" & MFileName & "' and IsActive= 1;"

            Dim omId As String = db.ExecuteScalar(sql)

            If omId <> "" Then
                sql = "update tblMultimediaObject set IsActive = 0 where MultimediaObjId = '" & omId & "';"
                db.Execute(sql)
            End If

            sql = "insert into tblMultimediaObject(QSetId,MFileName,MFileType,ReferenceId,ReferenceType) values('" & QsetId & "','" & MFileName & "','" & MFileType & "','" & RefId & "','" & RefType & "');"

            db.Execute(sql)
        Catch ex As Exception
            Log.Record(Log.LogType.ExamStep, "บันทึกไฟล์เสียงไม่สำเร็จ", False)
        End Try
    End Function

    Public Function ExportEvalution(QuestionId As String, IsFullQset As Boolean) As Boolean

        Dim QuestionDetailTable As DataTable
        QuestionDetailTable = GetDetailQuestionName(QuestionId)

        Dim EvalutionTable As DataTable
        EvalutionTable = GetEvalutionByQuestion(QuestionId)

        Dim ExcelApp As New Excel.Application
        Dim WB As Excel.Workbook
        Dim WS As Excel.Worksheet

        WB = ExcelApp.Workbooks.Add()

        For Each QDT In QuestionDetailTable.Rows

            WS = WB.Sheets.Add
            WS.Name = QDT("Question_No").ToString

            WS.Cells(1, 1) = "หน่วยการเรียนรู้"
            WS.Cells(2, 1) = "ชุดข้อสอบ"
            WS.Cells(3, 1) = "คำถาม"

            WS.Cells(1, 2) = QDT("QCategory_Name").ToString
            WS.Cells(2, 2) = QDT("QSet_Name").ToString
            WS.Cells(3, 2) = "ข้อที่ " & QDT("Question_No").ToString

            WS.Range("B1:D1").MergeCells = True
            WS.Range("B2:D2").MergeCells = True

            WS.Cells(4, 1) = "ดัชนีชี้วัด"
            WS.Range("A4:D4").MergeCells = True
            WS.Cells.WrapText = True

            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 4

            Dim dc As DataColumn
            Dim dr As DataRow

            For Each dr In EvalutionTable.Rows
                colIndex = 0
                rowIndex += 1
                For Each dc In EvalutionTable.Columns
                    colIndex += 1
                    WS.Cells(rowIndex, colIndex) = dr(dc.ColumnName).ToString
                Next
            Next

        Next

        Dim PathName As String = "~/QRReport/EvalutionFile/"
        Dim FileName As String = "Evalution_" & Now.Year & Now.Month & Now.Day & "_" & Now.Hour & Now.Minute & ".xlsx"
        Dim Path = HttpContext.Current.Server.MapPath(PathName & FileName)

        WB.SaveAs(Path)
        WB.Close()
        ExcelApp.Quit()

        Return True

    End Function

    Public Function sendEmailToAdmin(mailSubject As String, ByVal strBody As String) As Boolean
        Dim InLog As New ClsLog
        InLog.Record("เริ่มส่ง Mail")

        Try

            Dim mail As New MailMessage()
            mail.To.Add(New MailAddress("2tests2017@gmail.com")) ' send to email
            mail.From = New MailAddress("2tests2017@gmail.com") ' from email
            mail.Subject = mailSubject
            mail.IsBodyHtml = True ' แทรกข้อความแบบ html
            mail.Body = strBody

            Dim Smtp As New SmtpClient("smtp.gmail.com") ' ip ของ server mail
            Smtp.Port = 587
            Smtp.UseDefaultCredentials = False
            'Smtp.Credentials = New NetworkCredential("2tests2017", "ITsupp0rt")
            Smtp.Credentials = New NetworkCredential("2tests2017", "ITsupport")
            Smtp.EnableSsl = True
            Smtp.Send(mail)
            Return True
        Catch ex As Exception
            InLog.Record(ex.ToString)
            Return False
        End Try

    End Function




End Class
