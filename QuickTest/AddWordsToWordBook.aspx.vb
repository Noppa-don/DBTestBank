Public Class AddWordsToWordBook
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnAddWords_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddWords.Click
        Dim db As New ClassConnectSql()
        Dim addWords As New ClsWordBook()

        Dim sqlEng As String = ""
        sqlEng &= " SELECT q.QSet_Id,q.Question_Id,qs.QSet_Type FROM tblbook bk "
        sqlEng &= " INNER JOIN tblQuestionCategory qc ON bk.BookGroup_Id = qc.Book_Id "
        sqlEng &= " INNER JOIN tblQuestionSet qs ON qc.QCategory_Id = qs.QCategory_Id "
        sqlEng &= " INNER JOIN tblQuestion q ON qs.QSet_Id = q.QSet_Id "
        sqlEng &= " WHERE GroupSubject_Id = 'FB677859-87DA-4D8D-A61E-8A76566D69D8' "
        sqlEng &= " AND q.Question_Id NOT IN (SELECT Question_Id FROM tblWordBook) "
        sqlEng &= " ORDER BY q.QSet_Id; "

        Dim dt As DataTable
        dt = db.getdata(sqlEng,,, 50000)

        'Response.Write("Start : " + Date.Now().ToString("hh:mm:ss:fff") + "</br>")
        For Each r In dt.Rows
            Dim QuestionId As String = r("Question_Id").ToString()
            Dim Result As String = addWords.InsertWordToWordBook(QuestionId)
        Next
        'Response.Write("END : " + Date.Now().ToString("hh:mm:ss:fff"))
        'Dim QuestionId As Guid = Guid.NewGuid()
        'Dim Result As String = addWords.InsertWordToWordBook(QuestionId.ToString())
        'Response.Write(Result)
    End Sub
End Class