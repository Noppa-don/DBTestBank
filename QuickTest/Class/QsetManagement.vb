Public Class QsetManagement

    Private db As New ClassConnectSql()
    Private Property QsetId As String
    Private Property ClassId As String
    Private Property SubjectId As String

    Public Sub New(qsetId As String)
        Me.QsetId = qsetId
    End Sub

    Public Sub New(classId As String, subjectId As String, qsetId As String)
        Me.QsetId = qsetId
        Me.ClassId = classId
        Me.SubjectId = subjectId
    End Sub

    Public Function DeleteQset() As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestionset SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE QSet_Id = '" & Me.QsetId & "'; "
            db.ExecuteWithTransection(sql)
            sql = "UPDATE tblQuestion SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE QSet_Id = '" & Me.QsetId & "'; "
            db.ExecuteWithTransection(sql)
            sql = "UPDATE tblAnswer SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE QSet_Id = '" & Me.QsetId & "'; "
            db.ExecuteWithTransection(sql)
            'หาข้อสอบใน testset ด้วยเผื่อมี
            DeleteQsetInTestset()
            db.CommitTransection()
            'ลบข้อสอบใน temp เผื่อกำลังจัดชุดอยู่แล้วเลือกไป
            RemoveQsetInTempTestset()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Sub DeleteQsetInTestset()
        Try
            Dim dt As DataTable = GetQsetInTestset()
            For Each r In dt.Rows
                Dim testsetId As String = r("Testset_Id").ToString()
                If GetQsetAmountInTestset(testsetId) = 1 Then
                    'ถ้า testset มี qset เดียวให้ลบชุดทิ้งไปด้วย
                    DeleteTestset(testsetId)
                End If
                Dim tsqsId As String = r("TSQS_Id").ToString()
                DeleteTestsetQuestion(tsqsId)
            Next
        Catch ex As Exception
        End Try
    End Sub

    Public Function IsQsetExistInTestset() As Boolean
        db.OpenWithTransection()
        Dim dt As DataTable = GetQsetInTestset()
        db.CommitTransection()
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function RemoveQsetInTempTestset() As Boolean
        Try
            Dim tempTestset As Testset = HttpContext.Current.Session("TempTestset")
            Dim subjectClassId As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(Me.ClassId, Me.SubjectId)
            If subjectClassId IsNot Nothing Then
                subjectClassId.RemoveTestsetQuestionset(QsetId)
            End If
            HttpContext.Current.Session("TempTestset") = tempTestset
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' หาว่า qset ที่จะทำการลบมีอยุ่ใน testset ไหนบ้าง
    ''' </summary>
    ''' <returns></returns>
    Private Function GetQsetInTestset() As DataTable
        Dim sql As String = "SELECT * FROM tblTestSetQuestionSet WHERE QSet_Id = '" & Me.QsetId & "';"
        Return db.getdataWithTransaction(sql)
    End Function

    ''' <summary>
    ''' หาจำนวน qset ที่อยู่ใน testset นั้น
    ''' </summary>
    ''' <param name="testsetId"></param>
    ''' <returns></returns>
    Private Function GetQsetAmountInTestset(testsetId As String) As Integer
        Dim sql As String = "SELECT COUNT(*) FROM tblTestSetQuestionSet WHERE TestSet_Id = '" & testsetId & "';"
        Return CInt(db.ExecuteScalarWithTransection(sql))
    End Function

    ''' <summary>
    ''' ลบ testset ทิ้ง
    ''' </summary>
    ''' <param name="testsetId"></param>
    Private Sub DeleteTestset(testsetId As String)
        Dim sql As String = "UPDATE tblTestSet SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE TestSet_Id = '" & testsetId & "';"
        db.ExecuteWithTransection(sql)
    End Sub

    ''' <summary>
    ''' ลบข้อสอบใน testsetquestionset และ testsetquestionsetdetail
    ''' </summary>
    ''' <param name="tsqsId"></param>
    Private Sub DeleteTestsetQuestion(tsqsId As String)
        Dim sql As String = "UPDATE tblTestSetQuestionSet SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE TSQS_Id = '" & tsqsId & "';"
        db.ExecuteWithTransection(sql)
        sql = "UPDATE tblTestSetQuestionDetail SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE TSQS_Id = '" & tsqsId & "';"
        db.ExecuteWithTransection(sql)
    End Sub
End Class
