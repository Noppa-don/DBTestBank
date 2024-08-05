Public Class BookManagement

    Private Property UserId As String
    Private db As New ClassConnectSql()

    Public Sub New(userId As String)
        Me.UserId = userId
    End Sub


    ''' <summary>
    ''' function สำหรับ get qset ทั้งหมด ที่ user เพิ่มขึ้นเอง
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <returns>DataTable</returns>
    Public Function GetQsetsByUser(subjectId As String, classId As String) As DataTable
        Dim sql As String = "SELECT b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_No,qs.QSet_Name,qs.Qset_Type,COUNT(qs.qset_Id) AS QuestionAmount "
        sql &= " FROM tblQuestionCategory qc INNER JOIN tblQuestionset qs On qc.QCategory_Id = qs.QCategory_Id "
        sql &= " INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id "
        sql &= " WHERE qc.IsWpp = 0 And qs.IsActive = 1 And q.IsActive = 1 And qc.Parent_Id = '" & UserId & "' AND b.Level_Id = '" & classId & "' and b.GroupSubject_Id = '" & subjectId & "' "
        sql &= " GROUP BY b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_Name,qs.QSet_No,qs.QSet_Type  "
        sql &= " ORDER BY qc.QCategory_No,qs.QSet_No,qs.QSet_Name"
        Return db.getdata(sql)
    End Function

    ''' <summary>
    ''' function สำหรับ get qset ทั้งหมด ของ wpp
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <returns></returns>
    Public Function GetQsetsWPP(subjectId As String, classId As String) As DataTable
        Dim syllabusYear As String = "51"
        Dim sql As String = "select b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_Name,qs.QSet_No,qs.Qset_Type,Count(qs.qset_Id) as QuestionAmount from tblQuestionset qs inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id"
        sql &= " inner join tblBook b On qc.Book_Id = b.BookGroup_Id inner join tblQuestion q on qs.QSet_Id = q.QSet_Id where b.Level_Id = '" & classId & "' and GroupSubject_Id = '" & subjectId & "' and qc.IsActive = 1  "
        sql &= " and qs.IsActive = 1 and q.IsActive = 1 and b.Book_Syllabus = '" & syllabusYear & "' and (qc.IsWpp <> 0 or qc.IsWpp is null) "
        sql &= " group by b.BookGroup_Id,b.Book_Name,qc.QCategory_Name,qc.QCategory_No,qs.QSet_Id,qs.QSet_Name,qs.QSet_No,qs.QSet_Type  "
        sql &= " order by b.BookGroup_Id, qc.QCategory_No,qs.QSet_No,qc.QCategory_Name;"
        Return db.getdata(sql)
    End Function

End Class
