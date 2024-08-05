Imports System.Data.SqlClient

Public Class WordLayoutConfirmed
    Inherits System.Web.UI.Page
    'ใช้จัดการฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()
    'เก็บ QsetId เอาไว้ใช้กับหน้า Javascript ด้วย
    Public Property Qset_Id As String
        Get
            Return ViewState("_Qset_Id")
        End Get
        Set(ByVal value As String)
            ViewState("_Qset_Id") = value
        End Set
    End Property
    'Class ที่ใช้เกี่ยวกับการ select,update ข้อมูล ใช้กับหน้านี้ และ WordLayoutConfirmed
    Protected Shared clslayoutcheck As New ClsLayoutCheckConfirmed()

    ''' <summary>
    ''' ทำการ Bind Repeater ข้อสอบชุดนี้ แสดงว่าข้อนี้ มีการแก้ไข หรือ อนุมัติบ้างหรือยัง ในรูปแบบ Word
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("qsetid") IsNot Nothing Then
            Qset_Id = Request.QueryString("qsetid").ToString()
            clslayoutcheck.BindRepeater(Qset_Id, "Word", rptListQuestion)
        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้าง TestsetId ขึ้นมาใหม่สำหรับ QsetId นี้โดยเฉพาะเพื่อที่จะ Gen File Word ให้นำไปตรวจสอบ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGenWord_Click(sender As Object, e As EventArgs) Handles btnGenWord.Click
        'สร้าง Testset ขึ้นมาใหม่ก่อน
        Dim TestsetId As String = CreateNewTestset(Qset_Id)
        If TestsetId <> "" Then
            'GenWord ด้วย TestSetId ที่เพิ่งจะสร้างขึ้นมาใหม่
            Log.Record(Log.LogType.ManageExam, "ทดสอบ Print File Word (QSetId=" & Qset_Id & ")", True)
            Dim clsGenWord As New ClsGenWord(New ClassConnectSql())
            clsGenWord.GenNewDocument(QuickTest.ClsGenWord.GenType.ShowCorrectAnswer, TestsetId)
            'เมื่อ Genword เสร็จแล้วให้ลบทิ้งเลย
            DeleteTestset(TestsetId)

        End If
    End Sub

    ''' <summary>
    ''' ทำการสร้าง Testset ใหม่เลย โดยใช้ข้อมูลเฉพาะ QsetId นี้เลย
    ''' </summary>
    ''' <param name="qsetId">QsetId ที่จะนำไปสร้าง Testset ใหม่</param>
    ''' <returns>String:TestsetId</returns>
    ''' <remarks></remarks>
    Private Function CreateNewTestset(ByVal qsetId As String) As String
        Dim conn As New SqlConnection()
        _DB.OpenExclusiveConnect(conn)
        'เปิด Transaction
        _DB.OpenWithTransection(conn)
        Dim sql As String = ""
        Try
            sql = " SELECT NEWID(); "
            Dim TestsetId As String = _DB.ExecuteScalarWithTransection(sql, conn)
            'Insert tblTestset
            sql = " INSERT INTO dbo.tblTestSet ( TestSet_Id ,TestSet_Name ,UserIdOld ,SchoolId ,Level_Id ,TestSet_Time ,IsActive ,LastUpdate , " &
                  " TestSet_FontSize ,IsPracticeMode ,NeedConnectCheckmark ,UserId ,UserType ,IsHomeWorkMode , " &
                  " IsReportMode ,IsQuizMode ,IsStandard ,Calendar_Id ,GroupSubject_Name ,GroupSubject_ShortName ,ClientId) " &
                  " SELECT  TOP 1  '" & TestsetId & "' , QCategory_Name ,1 , '1000000' , NEWID() , 60 , 1 , dbo.GetThaiDate() ,NULL , " &
                  " 0 , 0,'" & Session("UserId").ToString() & "' ,1 , 0 , 1 , 0 , 1 ,NULL ,NULL ,NULL ,NULL FROM dbo.tblQuestionSet " &
                  " INNER JOIN dbo.tblQuestionCategory ON dbo.tblQuestionSet.QCategory_Id = dbo.tblQuestionCategory.QCategory_Id WHERE QSet_Id = '" & qsetId & "'; "
            _DB.ExecuteWithTransection(sql, conn)
            'Insert tblTestsetQuestionSet
            sql = " SELECT NEWID(); "
            Dim TSQSId As String = _DB.ExecuteScalarWithTransection(sql, conn)
            sql = " INSERT INTO dbo.tblTestSetQuestionSet( TSQS_Id ,TestSet_Id ,QSet_Id ,TSQS_No ,Level_Id ,IsActive ,LastUpdate ,ClientId ) " &
                  " VALUES  ( '" & TSQSId & "' ,'" & TestsetId & "' ,'" & qsetId & "' , 1 ,NULL ,1 , dbo.GetThaiDate() ,NULL ); "
            _DB.ExecuteWithTransection(sql, conn)
            'Insert tblTestsetQuestionDetail
            sql = " INSERT INTO dbo.tblTestSetQuestionDetail( TSQD_Id ,TSQS_Id ,Question_Id ,TSQD_No ,IsActive ,LastUpdate ,ClientId ) " &
                  " SELECT  NEWID() , '" & TSQSId & "' , Question_Id ,ROW_NUMBER()OVER	(ORDER BY Question_No) , 1 , dbo.GetThaiDate() ,NULL " &
                  " FROM dbo.tblQuestion WHERE IsActive = 1 AND QSet_Id = '" & qsetId & "' ORDER BY Question_No; "
            _DB.ExecuteWithTransection(sql, conn)
            'Commit transaction
            _DB.CommitTransection(conn)
            'Close Connection
            _DB.CloseExclusiveConnect(conn)
            'return NewTestsetId
            Return TestsetId
        Catch ex As Exception
            _DB.RollbackTransection(conn)
            _DB.CloseExclusiveConnect(conn)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' ทำการลบ Testset ที่สร้างเมื่อกี้ทิ้งไป เพราะจุดประสงค์ของการ Gen File Word เพราะต้องการนำไปตรวจสอบ ถ้าไม่ลบจะกลายเป็นข้อมูลขยะ
    ''' </summary>
    ''' <param name="testsetId">TestSetId ที่ต้องการจะลบ</param>
    ''' <remarks></remarks>
    Private Sub DeleteTestset(ByVal testsetId As String)
        Dim conn As New SqlConnection()
        'Open connection
        _DB.OpenExclusiveConnect(conn)
        'Open transaction
        _DB.OpenWithTransection(conn)
        Dim sql As String = ""
        Try
            'Delete tblTestsetQuestionDetail
            sql = " DELETE dbo.tblTestSetQuestionDetail WHERE TSQS_Id IN (SELECT TSQS_Id FROM dbo.tblTestSetQuestionSet " & _
                  " WHERE TestSet_Id = '" & testsetId & "' AND IsActive = 1); "
            _DB.ExecuteWithTransection(sql, conn)
            'Delete tblTestsetQuestionSet
            sql = " DELETE dbo.tblTestSetQuestionSet WHERE TestSet_Id = '" & testsetId & "' AND IsActive = 1; "
            _DB.ExecuteWithTransection(sql, conn)
            'Delete tblTestset
            sql = " DELETE dbo.tblTestSet WHERE TestSet_Id = '" & testsetId & "' AND IsActive = 1; "
            _DB.ExecuteWithTransection(sql, conn)
            'Commit transaction
            _DB.CommitTransection(conn)
            'Close connection
            _DB.CloseExclusiveConnect(conn)
        Catch ex As Exception
            _DB.RollbackTransection(conn)
            'Close connection
            _DB.CloseExclusiveConnect(conn)

        End Try
    End Sub

End Class