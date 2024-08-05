Imports QuickTest
Imports System.Web.Script.Serialization

Public Class AddQuestionSetPage
    Inherits System.Web.UI.Page

    Protected Book As New QuestionService.Book
    Protected QuestionCategoryJSONStr As String = Nothing

    Private db As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Book.GroupSubjectId = Request.QueryString("subjectid")
        Book.LevelId = Request.QueryString("levelid")

        Dim dtBook As DataTable = GetBook()
        If dtBook.Rows.Count = 0 Then
            NewBook()
            Book.QuestionCategories = Nothing
        Else
            Book.Id = dtBook.Rows(0)("BookGroup_Id").ToString()
            Book.QuestionCategories = GetQuestionCategories()
            Dim jsonSerialiser = New JavaScriptSerializer()
            QuestionCategoryJSONStr = jsonSerialiser.Serialize(Book.QuestionCategories)
        End If
    End Sub

    Private Function GetQuestionCategories() As IEnumerable(Of QuestionService.QuestionCategory)
        Try
            Dim userId As String = HttpContext.Current.Session("UserId").ToString()
            Dim sql As String = "SELECT * FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id = '" & userId & "' AND Book_Id = '" & Book.Id & "';"
            Dim dt As DataTable = db.getdata(sql)
            If dt.Rows.Count = 0 Then
                Return Nothing
            End If
            Dim categories As New List(Of QuestionService.QuestionCategory)
            For Each r In dt.Rows
                categories.Add(New QuestionService.QuestionCategory With {.Id = r("QCategory_Id").ToString(), .Name = r("QCategory_Name"), .BookId = Book.Id})
            Next
            Return categories
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub NewBook()
        db.OpenWithTransection()
        Book.Id = db.ExecuteScalarWithTransection("SELECT NEWID();").ToString()
        Book.Name = Book.GroupSubjectId.ToSubjectBookThName() & " " & Book.LevelId.ToLevelShortName() & " (ข้อสอบที่โรงเรียนเพิ่มเอง)"
        Dim sql As String = "INSERT INTO tblBook VALUES(NEWID(),'" & Book.Id & "','" & Book.LevelId & "','" & Book.GroupSubjectId & "','" & Book.Name & "',NULL,NULL,1,NULL,51,GETDATE(),0,NULL);"
        db.ExecuteWithTransection(sql)
        db.CommitTransection()
    End Sub

    Private Function GetBook() As DataTable
        Dim sql As String = "SELECT * FROM tblBook WHERE Level_Id = '" & Book.LevelId & "' AND GroupSubject_Id = '" & Book.GroupSubjectId & "' AND IsWpp = 0;"
        Return db.getdata(sql)
    End Function



End Class