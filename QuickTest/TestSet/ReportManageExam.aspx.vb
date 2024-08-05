Public Class ReportManageExam
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim GId As String = Request.QueryString("GId")
            Dim LId As String = Request.QueryString("LId")
            'FillDtReport(GId, LId)
            BindGrid(GId, LId)

        End If

    End Sub

    Private Sub FillDtReport(ByVal GroupSubjectId As String, ByVal LevelId As String)

        Dim dtReport As DataTable = CreateDtReport()
        Dim dt1 As DataTable = CreateDt1(GroupSubjectId, LevelId)
        Dim dt2 As DataTable = CreateDt2(dt1)
        MergeToDtReport(dtReport, dt2)

    End Sub

    Private Function CreateDt1(ByVal GroupSubjectId As String, ByVal LevelId As String)

        Dim dtReturn As New DataTable
        dtReturn.Columns.Add("QCategory_Name", GetType(String))
        dtReturn.Columns.Add("QSet_Name", GetType(String))
        dtReturn.Columns.Add("ExamAmount", GetType(Integer))
        dtReturn.Columns.Add("QsetId", GetType(String))
        dtReturn.Columns.Add("IsExplainQuestion", GetType(String))
        dtReturn.Columns.Add("IsExplainAnswer", GetType(String))

        Dim sql As String = " SELECT tblQuestionCategory.QCategory_Name, tblQuestionSet.QSet_Name, COUNT(tblQuestion.Question_Id) AS ExamAmount, dbo.tblQuestionSet.QSet_Id " & _
                            " FROM tblGroupSubject INNER JOIN tblBook ON tblGroupSubject.GroupSubject_Id = tblBook.GroupSubject_Id INNER JOIN " & _
                            " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN tblQuestion INNER JOIN " & _
                            " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id INNER JOIN " & _
                            " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id ON tblBook.BookGroup_Id = tblQuestionCategory.Book_Id " & _
                            " WHERE (tblQuestionCategory.IsActive = 1) AND (tblQuestion.IsActive = 1) AND (tblQuestionSet.IsActive = 1) AND " & _
                            " (dbo.tblGroupSubject.GroupSubject_Id = '" & GroupSubjectId & "') AND " & _
                            " (dbo.tblLevel.Level_Id = '" & LevelId & "') " & _
                            " GROUP BY tblQuestionCategory.QCategory_Name, tblQuestionSet.QSet_Name,dbo.tblQuestionSet.QSet_Id "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            Dim dtLoop As New DataTable

            For index = 0 To dt.Rows.Count - 1
                Dim QsetID As String = dt.Rows(index)("QSet_Id").ToString()
                Dim IsExplainQ As String = ""
                Dim TotalExpA As String = ""
                Dim IsExplainA As String = ""

                sql = " SELECT Question_Id FROM dbo.tblQuestion WHERE QSet_Id = '" & QsetID & "' AND IsActive = 1 "
                dtLoop.Clear()
                dtLoop = _DB.getdata(sql)
                If dtLoop.Rows.Count > 0 Then
                    'หาอธิบายโจทย์
                    sql = " SELECT SUM(CAST(TotalExpQ AS INTEGER)) AS IsExplainQ FROM " & _
                          " (SELECT CASE WHEN Question_Expain IS NOT NULL AND Question_Expain NOT LIKE '' THEN '1' ELSE '0'  END AS TotalExpQ " & _
                          " FROM dbo.tblQuestion WHERE IsActive = 1 AND QSet_Id = '" & QsetID & "')tblxx "
                    IsExplainQ = _DB.ExecuteScalar(sql)
                    'หาจำนวนข้อที่อธิบายคำตอบไปแล้ว
                    sql = " SELECT SUM(CAST(TotalExpA AS INTEGER))AS IsExplainA FROM " & _
                          " (SELECT CASE WHEN  dbo.tblAnswer.Answer_Expain IS NOT NULL AND dbo.tblAnswer.Answer_Expain NOT LIKE '' THEN '1' ELSE '0' END AS TotalExpA " & _
                          " FROM tblAnswer INNER JOIN tblQuestion ON tblAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN " & _
                          " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " & _
                          " WHERE (tblQuestion.IsActive = 1) AND (tblAnswer.IsActive = 1) AND " & _
                          " (tblQuestionSet.QSet_Id = '" & QsetID & "'))tblxx "
                    IsExplainA = _DB.ExecuteScalar(sql)
                    'หาจำนวนคำตอบทั้งหมด
                    sql = " SELECT COUNT(Answer_Id) AS AnswerAmount FROM tblAnswer INNER JOIN " & _
                          " tblQuestion ON tblAnswer.Question_Id = tblQuestion.Question_Id INNER JOIN " & _
                          " tblQuestionSet ON tblQuestion.QSet_Id = tblQuestionSet.QSet_Id " & _
                          " WHERE (tblQuestion.IsActive = 1) AND (tblAnswer.IsActive = 1) AND (tblQuestionSet.QSet_Id = '" & QsetID & "') "
                    TotalExpA = _DB.ExecuteScalar(sql)
                    dtReturn.Rows.Add(index)("QCategory_Name") = dt.Rows(index)("QCategory_Name").ToString()
                    dtReturn.Rows(index)("QSet_Name") = dt.Rows(index)("QSet_Name").ToString()
                    dtReturn.Rows(index)("ExamAmount") = dt.Rows(index)("ExamAmount").ToString()
                    dtReturn.Rows(index)("QsetId") = dt.Rows(index)("QSet_Id").ToString()
                    dtReturn.Rows(index)("IsExplainQuestion") = IsExplainQ
                    dtReturn.Rows(index)("IsExplainAnswer") = IsExplainA & "/" & TotalExpA
                End If
            Next

        End If

        Return dtReturn

    End Function

    Private Function CreateDt2(ByVal dt1 As DataTable)

        Dim dtReturn As New DataTable
        dtReturn.Columns.Add("QCategory_Name", GetType(String))
        dtReturn.Columns.Add("QSet_Name", GetType(String))
        dtReturn.Columns.Add("ExamAmount", GetType(Integer))
        dtReturn.Columns.Add("IsExplainQuestion", GetType(String))
        dtReturn.Columns.Add("IsExplainAnswer", GetType(String))
        dtReturn.Columns.Add("NewEva", GetType(String))
        dtReturn.Columns.Add("KPA", GetType(String))
        dtReturn.Columns.Add("InterExam", GetType(String))
        dtReturn.Columns.Add("Difficult", GetType(String))
        dtReturn.Columns.Add("ProveNewEva", GetType(String))
        dtReturn.Columns.Add("ProveKPA", GetType(String))
        dtReturn.Columns.Add("ProveInterExam", GetType(String))
        dtReturn.Columns.Add("ProveDifficult", GetType(String))

        If dt1.Rows.Count > 0 Then
            Dim sql As String = ""
            sql = " SELECT  EI_Id,EI_Code FROM dbo.tblEvaluationIndexNew WHERE IsActive = 1 AND Parent_Id IS NULL "
            Dim dtEvaluation As New DataTable
            dtEvaluation = _DB.getdata(sql)

            If dtEvaluation.Rows.Count > 0 Then
                'Loop Qset
                For index = 0 To dt1.Rows.Count - 1
                    Dim QsetId As String = dt1.Rows(index)("QSetId").ToString()
                    dtReturn.Rows.Add("QCategory_Name", dt1.Rows(index)("QCategory_Name"))
                    dtReturn.Rows(index)("QSet_Name") = dt1.Rows(index)("QCategory_Name")
                    dtReturn.Rows(index)("ExamAmount") = dt1.Rows(index)("ExamAmount")
                    dtReturn.Rows(index)("IsExplainQuestion") = dt1.Rows(index)("IsExplainQuestion")
                    dtReturn.Rows(index)("IsExplainAnswer") = dt1.Rows(index)("IsExplainAnswer")
                    'Loop EI_Id
                    For z = 0 To dtEvaluation.Rows.Count - 1
                        Dim TotalEva As String = ""
                        Dim TotalProveEva As String = ""
                        If dtEvaluation(z)("EI_Code") = "ตัวชี้วัด" Then
                            'หาว่่ามีการทำดัชนี "ตัวชี้วัด" ไปแล้วกี่ข้อ
                            TotalEva = GetTotalEva(QsetId, dtEvaluation.Rows(z)("EI_Id").ToString(), True)
                            'หาว่ามีการอนุมัติดัชนี "ตัวชี้วัด" ไปแล้วกี่ข้อ
                            TotalProveEva = GetTotalEva(QsetId, dtEvaluation.Rows(z)("EI_Id").ToString(), True, True)
                            'Add เข้า datatable
                            dtReturn.Rows(index)("NewEva") = TotalEva
                            dtReturn.Rows(index)("ProveNewEva") = TotalProveEva
                        Else
                            'หาว่ามีการทำดัชนี "KPA","แบบทดสอบระดับชาติ","ระดับความยากง่าย" ไปแล้วกี่ข้อ
                            TotalEva = GetTotalEva(QsetId, dtEvaluation.Rows(z)("EI_Id").ToString(), False)
                            'หาว่ามีการอนุมัติดัชนี "KPA","แบบทดสอบระดับชาติ","ระดับความยากง่าย" ไปแล้วกี่ข้อ
                            TotalProveEva = GetTotalEva(QsetId, dtEvaluation.Rows(z)("EI_Id").ToString(), False, True)
                            If dtEvaluation.Rows(z)("EI_Code") = "KPA" Then
                                dtReturn.Rows(index)("KPA") = TotalEva
                                dtReturn.Rows(index)("ProveKPA") = TotalProveEva
                            ElseIf dtEvaluation.Rows(z)("EI_Code") = "แบบทดสอบระดับชาติ" Then
                                dtReturn.Rows(index)("InterExam") = TotalEva
                                dtReturn.Rows(index)("ProveInterExam") = TotalProveEva
                            ElseIf dtEvaluation.Rows(z)("EI_Code") = "ระดับความยากง่าย" Then
                                dtReturn.Rows(index)("Difficult") = TotalEva
                                dtReturn.Rows(index)("ProveDifficult") = TotalProveEva
                            End If
                        End If
                    Next
                Next
            End If

        End If

        Return dtReturn

    End Function

    Private Sub MergeToDtReport(ByVal dtReport As DataTable, ByVal dt2 As DataTable)

        If dt2.Rows.Count > 0 Then

            For index = 0 To dt2.Rows.Count - 1
                Dim Flag As Boolean = True
                dtReport.Rows.Add("QCategory_Name", dt2.Rows(index)("QCategory_Name"))
                dtReport.Rows(index)("QSet_Name") = dt2.Rows(index)("QSet_Name")
                dtReport.Rows(index)("QAmount") = dt2.Rows(index)("ExamAmount")
                dtReport.Rows(index)("IsExplainQuestion") = dt2.Rows(index)("IsExplainQuestion")
                dtReport.Rows(index)("IsExplainAnswer") = dt2.Rows(index)("IsExplainAnswer")
                dtReport.Rows(index)("NewEvaluation") = dt2.Rows(index)("NewEva")
                dtReport.Rows(index)("KPA") = dt2.Rows(index)("KPA")
                dtReport.Rows(index)("InterExam") = dt2.Rows(index)("InterExam")
                dtReport.Rows(index)("Difficult") = dt2.Rows(index)("Difficult")
                dtReport.Rows(index)("ProveNewEvaluation") = dt2.Rows(index)("ProveNewEva")
                dtReport.Rows(index)("ProveKPA") = dt2.Rows(index)("ProveKPA")
                dtReport.Rows(index)("ProveInterExam") = dt2.Rows(index)("ProveInterExam")
                dtReport.Rows(index)("ProveDifficult") = dt2.Rows(index)("ProveDifficult")

                If dtReport.Rows(index)("QAmount") <> dtReport.Rows(index)("IsExplainQuestion") Then
                    Flag = False
                End If

                Dim SplitStr = dtReport.Rows(index)("IsExplainAnswer").ToString().Split("/")
                If SplitStr.Count > 0 Then
                    Dim ExplainAnswer As String = SplitStr(0).ToString()
                    Dim TotalAnswer As String = SplitStr(1).ToString()
                    If ExplainAnswer <> TotalAnswer Then
                        Flag = False
                    End If
                End If
                If dtReport.Rows(index)("NewEvaluation") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("KPA") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("InterExam") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("Difficult") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("ProveNewEvaluation") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("ProveKPA") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("ProveInterExam") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If
                If dtReport.Rows(index)("ProveDifficult") <> dtReport.Rows(index)("QAmount") Then
                    Flag = False
                End If

                If Flag = True Then
                    dtReport.Rows(index)("IsComplete") = "<img src='../Images/right.png' />"
                Else
                    dtReport.Rows(index)("IsComplete") = "<img src='../Images/wrong.png'/ >"
                End If

            Next
            ReportGrid.DataSource = dtReport
            ReportGrid.DataBind()
        End If

    End Sub

    Private Function CreateDtReport()

        Dim dtReport As New DataTable
        dtReport.Columns.Add("IsComplete", GetType(String))
        dtReport.Columns.Add("QCategory_Name", GetType(String))
        dtReport.Columns.Add("QSet_Name", GetType(String))
        dtReport.Columns.Add("QAmount", GetType(Integer))
        dtReport.Columns.Add("IsExplainQuestion", GetType(String))
        dtReport.Columns.Add("IsExplainAnswer", GetType(String))
        dtReport.Columns.Add("NewEvaluation", GetType(String))
        dtReport.Columns.Add("KPA", GetType(String))
        dtReport.Columns.Add("InterExam", GetType(String))
        dtReport.Columns.Add("Difficult", GetType(String))
        dtReport.Columns.Add("ProveNewEvaluation", GetType(String))
        dtReport.Columns.Add("ProveKPA", GetType(Integer))
        dtReport.Columns.Add("ProveInterExam", GetType(String))
        dtReport.Columns.Add("ProveDifficult", GetType(String))
         
        Return dtReport

    End Function

    Private Function GetTotalEva(ByVal QsetId As String, ByVal EIId As String, ByVal IsNewEva As Boolean, Optional ByVal IsWhereApprove As Boolean = False)
        Dim sql As String = ""
        If IsNewEva = True Then
            sql = " SELECT SUM(CAST(TotalNewEva AS INTEGER)) AS TotalEva FROM " & _
                  " (SELECT  CASE WHEN COUNT(dbo.tblQuestionEvaluationIndexItem.EI_Id) > 0 THEN '1' ELSE '0' END AS TotalNewEva " & _
                  " FROM tblEvaluationIndexNew INNER JOIN tblQuestionEvaluationIndexItem " & _
                  " ON tblEvaluationIndexNew.EI_Id = tblQuestionEvaluationIndexItem.EI_Id INNER JOIN " & _
                  " tblQuestion ON tblQuestionEvaluationIndexItem.Question_Id = tblQuestion.Question_Id " & _
                  " WHERE (tblQuestion.IsActive = 1) AND (tblEvaluationIndexNew.IsActive = 1) AND (tblQuestionEvaluationIndexItem.IsActive = 1) AND " & _
                  " (tblQuestion.QSet_Id = '" & QsetId & "') AND " & _
                  " dbo.tblEvaluationIndexNew.EI_Id IN (SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " & _
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " & _
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EIId & "'))) "
            If IsWhereApprove = True Then
                sql &= " AND IsApproved = 1 "
            End If
            sql &= " GROUP BY dbo.tblQuestionEvaluationIndexItem.Question_Id)tblxx "
        Else
            sql = " SELECT SUM(CAST(TotalEva AS INTEGER)) FROM " & _
                  " (SELECT CASE WHEN COUNT(dbo.tblQuestionEvaluationIndexItem.EI_Id) > 0 THEN '1' ELSE '0' END AS TotalEva " & _
                  " FROM tblEvaluationIndexNew INNER JOIN tblQuestionEvaluationIndexItem ON " & _
                  " tblEvaluationIndexNew.EI_Id = tblQuestionEvaluationIndexItem.EI_Id INNER JOIN " & _
                  " tblQuestion ON tblQuestionEvaluationIndexItem.Question_Id = tblQuestion.Question_Id " & _
                  " WHERE (tblQuestion.IsActive = 1) AND (tblEvaluationIndexNew.IsActive = 1) AND (tblQuestionEvaluationIndexItem.IsActive = 1) AND " & _
                  " (tblQuestion.QSet_Id = '" & QsetId & "') AND dbo.tblEvaluationIndexNew.EI_Id IN  " & _
                  " (SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id IN ( " & _
                  " SELECT EI_Id FROM dbo.tblEvaluationIndexNew WHERE Parent_Id = '" & EIId & "')) "
            If IsWhereApprove = True Then
                sql &= " AND IsApproved = 1 "
            End If
            sql &= " GROUP BY dbo.tblQuestionEvaluationIndexItem.Question_Id)tblxx "
        End If
        Dim TotalNewEva As String = _DB.ExecuteScalar(sql)

        If TotalNewEva = "" Then
            TotalNewEva = 0
        End If

        Return TotalNewEva

    End Function

    Private Sub BindGrid(ByVal GroupsubjectId As String, ByVal LevelId As String)

        Dim dt As DataTable = CreateDtLongQuery(GroupsubjectId, LevelId)
        If dt.Rows.Count > 0 Then

            For index = 0 To dt.Rows.Count - 1
                Dim Flag As Boolean = True

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("IsExplainQuestion") Then
                    Flag = False
                End If

                Dim Splistr = dt.Rows(index)("IsExplainAnswer").ToString().Split("/")
                Dim ExplainAnswer As String = Splistr(0)
                Dim TotalAnswer As String = Splistr(1)

                If ExplainAnswer <> TotalAnswer Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("NewEvaluation") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("KPA") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("InterExam") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("Difficult") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("ProveNewEvaluation") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("ProveInterExam") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("ProveDifficult") Then
                    Flag = False
                End If

                If dt.Rows(index)("QAmount") <> dt.Rows(index)("ProveKPA") Then
                    Flag = False
                End If

                If Flag = True Then
                    'dt.Rows(index)("IsComplete") = "<img src='../Images/right.png' />"
                    dt.Rows(index)("IsComplete") = "/"
                Else
                    'dt.Rows(index)("IsComplete") = "<img src='../Images/wrong.png'/ >"
                    dt.Rows(index)("IsComplete") = "X"
                End If
 
            Next

            ReportGrid.DataSource = dt
            ReportGrid.DataBind()

        End If


    End Sub

    Private Function CreateDtLongQuery(ByVal GroupSubjectId As String, ByVal LevelId As String)

        Dim sql As String = " SELECT 'a' AS IsComplete,qs.QSet_Id  ,  qc.QCategory_Name,  qs.QSet_name, COUNT(DISTINCT q.Question_Id) AS QAmount, " & _
                             " COUNT(DISTINCT CASE WHEN (q.Question_Expain is NOT NULL) AND (q.Question_Expain NOT like '')  " & _
                             " THEN q.Question_Id ELSE NULL END ) AS IsExplainQuestion, " & _
                             " CAST(SUM (CASE WHEN (a.answer_Expain is NOT NULL) AND (a.answer_Expain NOT like '') " & _
                             " THEN 1 ELSE 0 END )AS VARCHAR(MAX) ) + '/' + CAST(COUNT(a.answer_id ) AS VARCHAR(max))  AS IsExplainAnswer " & _
                             " , ( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3 ,dbo.tblEvaluationIndexNew ein4 ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein4.EI_Id  " & _
                             " AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id AND ein3.EI_Id = ein4.Parent_Id " & _
                             " AND ein1.ei_id = 'EF957BC3-C463-4315-8847-8B4522CA0100'   ) AS NewEvaluation " & _
                             " , ( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3 ,dbo.tblEvaluationIndexNew ein4 ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein4.EI_Id AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id AND ein3.EI_Id = ein4.Parent_Id " & _
                             " AND ein1.ei_id = 'EF957BC3-C463-4315-8847-8B4522CA0100' AND qe.IsApproved  =1  ) AS ProveNewEvaluation " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id " & _
                             " AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = '77E41F99-FD05-4097-B835-34BF09796125' ) AS KPA " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id  AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = '77E41F99-FD05-4097-B835-34BF09796125' AND IsApproved = 1 ) AS ProveKPA " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = 'BA3A748E-AA31-473C-B037-1E1046EC983B' ) AS InterExam " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = 'BA3A748E-AA31-473C-B037-1E1046EC983B' AND IsApproved = 1 ) AS ProveInterExam " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 , " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = 'DEDFBF9F-C8F5-493B-97F6-6741BEB09EB4' ) AS Difficult " & _
                             " ,( SELECT COUNT(DISTINCT qe.question_id) FROM dbo.tblQuestionEvaluationIndexItem qe " & _
                             " , dbo.tblEvaluationIndexNew ein1 , dbo.tblEvaluationIndexNew ein2 ,  " & _
                             " dbo.tblEvaluationIndexNew ein3  ,tblquestion q2 " & _
                             " WHERE qs.QSet_Id = q2.qset_id and  q2.question_id = qe.question_id AND qe.IsActive =1 " & _
                             " AND qe.EI_Id = ein3.EI_Id AND ein1.ei_id = ein2.Parent_Id AND ein2.EI_Id = ein3.Parent_Id  " & _
                             " AND ein1.ei_id = 'DEDFBF9F-C8F5-493B-97F6-6741BEB09EB4' AND IsApproved = 1 ) AS ProveDifficult ,b.Book_Syllabus " & _
                             " from dbo.tblQuestionCategory qc,dbo.tblQuestionSet qs, tblquestion q " & _
                             " , dbo.tblAnswer a , dbo.tblBook b , dbo.tblGroupSubject gsj , dbo.tblLevel lv " & _
                             " WHERE qs.QCategory_Id  = qc.QCategory_Id AND qs.IsActive = 1 AND qc.IsActive = 1 " & _
                             " AND qs.QSet_Id = q.QSet_Id  AND q.IsActive = 1 AND q.question_id = a.Question_Id AND a.IsActive = 1 " & _
                             " AND qc.Book_Id = b.BookGroup_Id AND b.GroupSubject_Id = gsj.GroupSubject_Id " & _
                             " AND b.Level_Id = lv.Level_Id AND gsj.GroupSubject_Id = '" & GroupSubjectId & "' " & _
                             " AND lv.Level_Id = '" & LevelId & "' AND b.Book_Syllabus <> 44 " & _
                             " GROUP BY qs.QSet_Id  ,qc.QCategory_Name,  qs.QSet_name ,b.Book_Syllabus " & _
                             " ORDER BY b.Book_Syllabus , qc.QCategory_name "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , , 90)
        Return dt

    End Function


    Private Sub ReportGrid_ExcelExportCellFormatting(ByVal sender As Object, ByVal e As Telerik.Web.UI.ExcelExportCellFormattingEventArgs) Handles ReportGrid.ExcelExportCellFormatting
        e.Cell.Style("mso-number-format") = "\@"
    End Sub

End Class