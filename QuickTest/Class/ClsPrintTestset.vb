Imports KnowledgeUtils
Public Class ClsPrintTestset
    Dim db As New ClassConnectSql()

    Private _UserID As String
    Private _QsetID As String
    Private _QsetName As String
    Private _CalendarId As String
    Private _IsTeacher As Integer
    Private sql As New StringBuilder()
    Private TestsetID As String
    Private TSQS_ID As String

    Public Sub New(ByVal UserID As String, ByVal QsetID As String, ByVal CalendarId As String, ByVal IsTeacher As String)
        _UserID = UserID
        _QsetID = QsetID
        _QsetName = GetTestsetName(_QsetID)
        _CalendarId = CalendarId
        If IsTeacher = "True" Then
            _IsTeacher = 1
        Else
            _IsTeacher = 2
        End If
        '_IsTeacher = IsTeacher
    End Sub

    'หาชื่อ testset จากการตัด QsetName

    Public Function GetTestsetName(ByVal QsetId As String) As String
        sql.Clear()
        sql.Append(" select Qc.QCategory_Name,qs.QSet_Name from tblQuestionSet qs,tblQuestionCategory qc where qs.QCategory_Id = qc.QCategory_Id and QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("'")


        Dim dt As DataTable = db.getdata(sql.ToString)
        Dim QsetName As String = dt.Rows(0)("Qset_Name").ToString
        Dim QcategoryName As String = dt.Rows(0)("QCategory_Name").ToString
        If InStr(QsetName, "(") <> 0 Then
            GetTestsetName = QcategoryName & "-" & QsetName.Substring(0, QsetName.IndexOf("(") - 1)
        Else
            GetTestsetName = QcategoryName & "-" & QsetName
        End If

    End Function

    ' check ว่า qset นั้น มี testset หรือยัง  get testset ออกมา
    Public Function GetTestSetID() As String
        sql.Clear()
        sql.Append(" SELECT ts.TestSet_Id FROM tblTestSet ts INNER JOIN tblTestSetQuestionSet tsqs ON ts.TestSet_Id = tsqs.TestSet_Id ")
        sql.Append(" WHERE ts.UserId = '")
        sql.Append(_UserID.CleanSQL)
        sql.Append("' AND tsqs.QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' AND ts.IsStandard = 1 AND ts.IsActive = 1 AND tsqs.IsActive = '1'; ")
        TestsetID = db.ExecuteScalar(sql.ToString())
        If TestsetID = "" Then
            'insert testsetID
            SetNewTestsetStandard()
        Else
            'check question มีการเปลี่ยนแปลง
            CheckQsetToUpdateQuestion()
        End If
        GetTestSetID = TestsetID
    End Function

    ' สร้าง testset ใหม่
    Private Sub SetNewTestsetStandard()
        'TestsetID = Guid.NewGuid().ToString()
        Dim _DB As New ClassConnectSql()
        TestsetID = New ClassConnectSql().ExecuteScalar(" SELECT NEWID();")
        ' insert qset ก่อน
        SetNewTestsetQuestionSet()
        ' สร้าง testset question detail ต่อ 
        TSQS_ID = GetTSQS_ID()
        InsertAllNewQuestionsToTestset()
        Dim GSName As Object = New ClsTestSet(_UserID).GetGroupSubjectName(TestsetID)
        ' insert tbltestset ท้ายสุด 
        sql.Clear()
        sql.Append(" INSERT INTO tblTestSet ( TestSet_Id ,TestSet_Name ,UserIdOld ,SchoolId ,Level_Id ,TestSet_Time ,IsActive , " &
                   " LastUpdate ,TestSet_FontSize ,IsPracticeMode ,IsHomeWorkMode ,IsReportMode ,IsQuizMode ,IsStandard ,NeedConnectCheckmark ," &
                   " UserId ,UserType ,Calendar_Id ,GroupSubject_Name ,GroupSubject_ShortName ,ClientId) SELECT '")
        sql.Append(TestsetID)
        sql.Append("',N'")
        Dim qsetName As String = _DB.CleanString(_QsetName)
        qsetName = Regex.Replace(qsetName, "<.*?>", String.Empty)
        If qsetName.Length > 250 Then
            qsetName = qsetName.Substring(0, 249) & "..."
        End If
        sql.Append(qsetName)
        sql.Append("',0,'")
        sql.Append(HttpContext.Current.Session("SchoolID").ToString()) ' SCHoolID
        sql.Append("','93B163B6-4F87-476D-8571-4029A6F34C84','60',1,dbo.GetThaiDate(),0,1,1,1,1,1,0,'") ' เลข GUID ของ ตัวหนังสือ ป6 ตัวหนังสือแบบ กลางๆๆ
        sql.Append(_UserID.CleanSQL)
        sql.Append("','")
        sql.Append(_IsTeacher)
        sql.Append("','")
        sql.Append(_CalendarId.CleanSQL)
        sql.Append("',N'")
        sql.Append(GSName.GroupSubject_Name)
        sql.Append("',N'")
        sql.Append(GSName.GroupSubject_ShortName)
        sql.Append("',NULL;")
        db.Execute(sql.ToString())
        _DB = Nothing
    End Sub
    ' สร้าง testsetqset ใหม่
    Private Sub SetNewTestsetQuestionSet()
        sql.Clear()
        sql.Append(" INSERT INTO tblTestSetQuestionSet SELECT NEWID(),'")
        sql.Append(TestsetID.CleanSQL)
        sql.Append("','")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("',1,NULL,1,dbo.GetThaiDate(),NULL")
        db.Execute(sql.ToString())
    End Sub

    ' check ว่า qset นี่มีคำถามเปลี่ยนแปลงหรือเปล่า (เพิ่มหรือลดลง)
    Private Sub CheckQsetToUpdateQuestion()
        sql.Clear()
        sql.Append(" SELECT SUM(tblDiffQuestion.Qset) AS NoOfDiffQuestion FROM ( ")
        sql.Append(" SELECT COUNT(*) AS Qset FROM tblQuestionSet qs INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id ")
        sql.Append(" WHERE qs.QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' AND q.IsActive = 1 ")
        sql.Append(" AND q.Question_Id NOT IN (SELECT  tsqd.Question_Id FROM tblTestSetQuestionSet tsqs INNER JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id  ")
        sql.Append(" WHERE tsqs.QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' AND tsqs.TestSet_Id = '")
        sql.Append(TestsetID.CleanSQL)
        sql.Append("') UNION ")
        sql.Append(" SELECT COUNT(*) AS TQset FROM tblTestSetQuestionSet tsqs INNER JOIN tblTestSetQuestionDetail tsqd ON tsqs.TSQS_Id = tsqd.TSQS_Id ")
        sql.Append(" WHERE tsqs.QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' AND tsqs.TestSet_Id = '")
        sql.Append(TestsetID.CleanSQL)
        sql.Append("' AND tsqd.IsActive = 1 AND tsqd.Question_Id NOT IN ( ")
        sql.Append(" SELECT q.Question_Id FROM tblQuestionSet qs INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id ")
        sql.Append(" WHERE qs.QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' ) ")
        sql.Append(" ) AS tblDiffQuestion ")
        Dim DiffQuestion As String = db.ExecuteScalar(sql.ToString())
        ' ถ้ามีมากว่า 0 ให้ลบ question ทังหมดทิ้ง
        If Not DiffQuestion = "0" Then
            TSQS_ID = GetTSQS_ID()
            DelAllQuestionsInTestset()
            InsertAllNewQuestionsToTestset()
        End If
    End Sub
    ' get tsqs_id ไว้ลบ question และ เพิ่ม question
    Private Function GetTSQS_ID() As String
        sql.Clear()
        sql.Append(" SELECT TSQS_ID FROM tblTestsetQuestionSet WHERE Testset_Id = '" & TestsetID.CleanSQL & "' And tblTestsetQuestionSet.IsActive = '1';")
        GetTSQS_ID = db.ExecuteScalar(sql.ToString())
    End Function
    ' delete question
    Private Sub DelAllQuestionsInTestset()
        sql.Clear()
        sql.Append(" Update tblTestSetQuestionDetail Set IsActive = '0' WHERE TSQS_ID = '" & TSQS_ID.CleanSQL & "' And IsActive = '1';")
        db.Execute(sql.ToString())
    End Sub
    ' insert question
    Private Sub InsertAllNewQuestionsToTestset()
        sql.Clear()
        sql.Append(" INSERT INTO tblTestSetQuestionDetail ")
        sql.Append(" SELECT NEWID(),'")
        sql.Append(TSQS_ID.CleanSQL)
        sql.Append("',Question_Id,ROW_NUMBER() OVER(ORDER BY Question_No),1,dbo.GetThaiDate(),NULL FROM tblQuestion WHERE QSet_Id = '")
        sql.Append(_QsetID.CleanSQL)
        sql.Append("' AND IsActive = 1;")
        db.Execute(sql.ToString())
    End Sub

End Class
