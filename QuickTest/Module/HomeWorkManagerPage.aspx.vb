Public Class HomeWorkManagerPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("NewHomeWorkQsetId") = Request.QueryString("QsetId").ToString()

        If Not Page.IsPostBack Then
            BindingRepeater()
        End If

    End Sub


    Private Sub BindingRepeater()

        Dim strSQL As String = ""
        'strSQL = " select distinct t.TestSet_Id, t.TestSet_Name, t.LastUpdate  from tbltestset t, tbltestsetquestionset tsqs  " & _
        '                       " where  t.testset_id = tsqs.testset_id and t.userid = '" & HttpContext.Current.Session("UserId").ToString() & "' " & _
        '                       " and (t.IsForHomeWork = 1) and (t.IsActive = 1 ) order by t.lastupdate desc  "

        strSQL = " SELECT tblTestSet.TestSet_Id, tblTestSet.LastUpdate, tblTestSet.TestSet_Name"
        strSQL &= " FROM tblTestSet INNER JOIN"
        strSQL &= " tblModuleDetail ON tblTestSet.TestSet_Id = tblModuleDetail.Reference_Id"
        strSQL &= " where tblTestSet.IsActive = 1"
        strSQL &= " and tbltestset.IsForHomeWork = 1"
        strSQL &= " and tblTestSet.UserId = '" & HttpContext.Current.Session("UserId").ToString() & "'"

        Dim ds As New DataSet
        ds = _DB.getdataset(strSQL)
        Listing.DataSource = ds
        Listing.DataBind()


    End Sub


    <Services.WebMethod()>
    Public Shared Function InsertNewTestSetForHomeWork()

        Dim VbQsetId As String = HttpContext.Current.Session("NewHomeWorkQsetId").ToString()
        Dim _DB As New ClassConnectSql()

        Try
            _DB.OpenWithTransection()
            'หา LevelId
            Dim sql As String = " SELECT TOP 1 tblLevel.Level_Id FROM tblQuestionSet INNER JOIN " & _
                                " tblQuestionCategory ON tblQuestionSet.QCategory_Id = tblQuestionCategory.QCategory_Id INNER JOIN " & _
                                " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN " & _
                                " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id WHERE (tblQuestionSet.QSet_Id = '" & VbQsetId & "') "
            Dim LevelId As String = _DB.ExecuteScalarWithTransection(sql)

            'หา QuestionCategoryName
            sql = " SELECT QCategory_Name FROM dbo.tblQuestionCategory WHERE " & _
                  " QCategory_Id = (SELECT QCategory_Id FROM dbo.tblQuestionSet WHERE QSet_Id ='" & VbQsetId & "') "
            Dim CategoryName As String = _DB.ExecuteScalarWithTransection(sql)

            'Insert tblTestset
            sql = " SELECT NEWID() "
            Dim TestSetId As String = _DB.ExecuteScalarWithTransection(sql)
            sql = " INSERT INTO dbo.tblTestSet ( TestSet_Id ,TestSet_Name ,SchoolId ,Level_Id ,IsActive , " & _
                  " LastUpdate, TestSet_FontSize, IsPracticeMode, " & _
                  " IsForHomeWork, NeedConnectCheckmark, UserId, UserType ) " & _
                  " VALUES  ( '" & TestSetId & "' , '" & _DB.CleanString(CategoryName) & "' ,  " & _
                  " '" & HttpContext.Current.Session("SchoolID") & "' , '" & LevelId & "' , 1 , GETDATE() ,  " & _
                  " 0 , 0 , 1 , 0 , '" & HttpContext.Current.Session("UserId").ToString() & "' , 1 ) "
            _DB.ExecuteWithTransection(sql)

            'Insert tblTestSetQuestionSet
            sql = " SELECT NEWID() "
            Dim TSQSID As String = _DB.ExecuteScalarWithTransection(sql)
            sql = " INSERT INTO dbo.tblTestSetQuestionSet( TSQS_Id ,TestSet_Id , " & _
                  " QSet_Id ,TSQS_No ,IsActive ,LastUpdate ) VALUES  " & _
                  " ( '" & TSQSID & "' , '" & TestSetId & "' , '" & VbQsetId & "' , 1 , 1 , GETDATE() ) "
            _DB.ExecuteWithTransection(sql)

            'Insert tblTestSetQuestionDetail
            sql = " INSERT INTO dbo.tblTestSetQuestionDetail  ( TSQD_Id ,TSQS_Id ,Question_Id , " & _
                  " TSQD_No ,IsActive ,LastUpdate ) " & _
                  " SELECT NEWID(),'" & TSQSID & "', Question_Id,Question_No,'1',GETDATE() " & _
                  " FROM dbo.tblQuestion WHERE QSet_Id = '" & VbQsetId & "' ORDER BY Question_No "
            _DB.ExecuteWithTransection(sql)

            _DB.CommitTransection()
            Return New With {.TestSetId = TestSetId, .CategoryName = CategoryName}
        Catch ex As Exception
            _DB.RollbackTransection()
            Return ""
        End Try

    End Function


End Class