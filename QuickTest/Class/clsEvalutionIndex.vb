Public Class clsEvalutionIndex

    Dim db As New ClassConnectSql()
    Public Function GetEvalutionIndexLevelOne(GroupSubjectId As String, LevelId As String) As DataTable
        Dim sql As String = ""
        Dim dt As DataTable
        sql = "select distinct en3.EI_Id,en3.EI_Code + ' ' + en3.EI_Name as EIName,en3.EI_Position
                from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id inner join tblBook b on qc.Book_Id = b.BookGroup_Id
                inner join tblQuestionEvaluationIndexItem qe on q.Question_Id = qe.Question_Id inner join tblEvaluationIndexNew en1 on qe.EI_Id = en1.EI_Id
                inner join tblEvaluationIndexNew en2 on en1.Parent_Id = en2.EI_Id inner join tblEvaluationIndexNew en3 on en2.Parent_Id = en3.EI_Id
                where q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 and qe.IsActive = 1 and en1.IsActive = 1
                and b.GroupSubject_Id = '" & GroupSubjectId & "' and b.Level_Id = '" & LevelId & "' and en1.Level_No = 3 order by en3.EI_Position"
        dt = db.getdata(sql)
        Return dt
    End Function
    Public Function GetEvalutionIndexLevelTwo(GroupSubjectId As String, LevelId As String, ParentId As String) As DataTable
        Dim sql As String
        sql = "select distinct en2.EI_Id,en2.EI_Code + ' ' + en2.EI_Name as EIName,en2.EI_Position
                from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id
                inner join tblBook b on qc.Book_Id = b.BookGroup_Id inner join tblQuestionEvaluationIndexItem qe on q.Question_Id = qe.Question_Id
                inner join tblEvaluationIndexNew en1 on qe.EI_Id = en1.EI_Id inner join tblEvaluationIndexNew en2 on en1.Parent_Id = en2.EI_Id
                where q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 and qe.IsActive = 1 and en1.IsActive = 1
                and b.GroupSubject_Id = '" & GroupSubjectId & "' and b.Level_Id = '" & LevelId & "'
                and en1.Level_No = 3 and en2.Parent_Id = '" & ParentId & "' order by en2.EI_Position"
        Dim dt As DataTable
        dt = db.getdata(sql)
        Return dt
    End Function
    Public Function GetEvalutionIndexLevelThree(GroupSubjectId As String, LevelId As String, ParentId As String,TestsetId As string) As DataTable
        Dim sql As String
        sql = "select distinct en1.EI_Id,en1.EI_Code + ' ' + en1.EI_Name as EIName,en1.EI_Position,case when tei.TEIId is null then 'false' else 'true' end as IsSelected 
                from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id inner join tblBook b on qc.Book_Id = b.BookGroup_Id
                inner join tblQuestionEvaluationIndexItem qe on q.Question_Id = qe.Question_Id inner join tblEvaluationIndexNew en1 on qe.EI_Id = en1.EI_Id
				left join (select teiid,eiid from tblTestsetEvalutionIndex where TestsetId = '" & TestsetId & "') tei on tei.EIID = en1.EI_Id
                where q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 and qe.IsActive = 1 and en1.IsActive = 1
                and b.GroupSubject_Id = '" & GroupSubjectId & "' and b.Level_Id = '" & LevelId & "'
                and en1.Level_No = 3 and en1.Parent_Id = '" & ParentId & "' order by en1.EI_Position"

        Dim dt As DataTable
        dt = db.getdata(sql)
        Return dt
    End Function

    Public Function AddTestsetEvalutionIndex(TestsetId As String, EI_ID As String) As Boolean
        Try
            Dim sql As String
            sql = "insert into tblTestsetEvalutionIndex(TestsetId,EIID) values('" & TestsetId & "','" & EI_ID & "');"
            db.ExecuteScalar(sql)

            DeleteQuestionDetailNotEIID(TestsetId)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Function DeleteTestsetEvalutionIndex(TestsetId As String, EI_ID As String) As Boolean
        Try
            Dim sql As String
            sql = "Delete tblTestsetEvalutionIndex where TestsetId = '" & TestsetId & "' And EIID = '" & EI_ID & "';"
            db.ExecuteScalar(sql)

            DeleteQuestionDetailNotEIID(TestsetId)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function DeleteAllTestsetEvalutionIndex(GroupSubjectId As String, LevelId As String, TestsetId As String) As Boolean
        Try
            Dim sql As String
            sql = "delete tblTestsetEvalutionIndex where EIID in(select distinct en1.EI_Id
                   from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                   inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id inner join tblBook b on qc.Book_Id = b.BookGroup_Id
                   inner join tblQuestionEvaluationIndexItem qe on q.Question_Id = qe.Question_Id inner join tblEvaluationIndexNew en1 on qe.EI_Id = en1.EI_Id
                   inner join tblEvaluationIndexNew en2 on en1.Parent_Id = en2.EI_Id inner join tblEvaluationIndexNew en3 on en2.Parent_Id = en3.EI_Id
                   inner join tblTestsetEvalutionIndex tei on en1.EI_Id = tei.EIID
                   where q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 and qe.IsActive = 1 and en1.IsActive = 1
                   and b.GroupSubject_Id = '" & GroupSubjectId & "' and b.Level_Id = '" & LevelId & "' and en1.Level_No = 3) and TestsetId = '" & TestsetId & "';"
            db.ExecuteScalar(sql)

            DeleteQuestionDetailNotEIID(TestsetId)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function DeleteQuestionDetailNotEIID(TestsetId As String) As Boolean
        Try
            Dim sql As String
            sql = "Update tqd set isactive = 0 from tblTestSetQuestionDetail tqd inner join tblTestSetQuestionSet tqs on tqd.TSQS_Id = tqs.TSQS_Id 
                    where tqs.TestSet_Id = '" & TestsetId & "' and TSQD_Id not in(
	                    select distinct tqd.TSQD_Id from tblTestSetQuestionDetail tqd 
	                    inner join tblQuestionEvaluationIndexItem qei on tqd.Question_Id = qei.Question_Id
	                    inner join tblTestsetEvalutionIndex tei on qei.EI_Id = tei.EIID 
	                    where tqd.IsActive = 1 and qei.IsActive = 1 and tei.IsActive = 1 
                        and tei.TestsetId = '" & TestsetId & "')"
            db.ExecuteScalar(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
