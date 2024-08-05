Public Class EditBookQuestionCategory
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()


    Public Property QuestionCategoryId As String
        Get
            QuestionCategoryId = ViewState("_QuestionCategoryId")
        End Get
        Set(ByVal value As String)
            ViewState("_QuestionCategoryId") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            BindDDlBook()
        End If

    End Sub


    Private Sub BindDDlBook()


        Dim QsetId As String = Request.QueryString("QsetId")
        Dim GroupSubjectId As String = Request.QueryString("GroupSubjectId")

        Dim sql As String = ""
        Dim QCatId As String = ""
        Dim QCatName As String = ""

        sql = " SELECT QCategory_Id FROM dbo.tblQuestionSet WHERE QSet_Id = '" & QsetId & "' "
        QCatId = _DB.ExecuteScalar(sql)
        QuestionCategoryId = QCatId

        sql = " SELECT QCategory_Name FROM dbo.tblQuestionCategory WHERE QCategory_Id = '" & QCatId & "' "
        QCatName = _DB.ExecuteScalar(sql)

        lblQuestionCategoryName.Text = QCatName

        sql = " SELECT Book_Id,Book_Name,Book_Syllabus,Level_Id FROM dbo.tblBook WHERE GroupSubject_Id = '" & GroupSubjectId & "' ORDER BY Book_Name "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then

            For i = 0 To dt.Rows.Count - 1
                Dim LevelName As String = ChangeLevelIdToLevelName(dt.Rows(i)("Level_Id").ToString())
                Dim BookName As String = dt.Rows(i)("Book_Name").ToString() & "(ปี " & dt.Rows(i)("Book_Syllabus").ToString() & ")(" & LevelName & ")"
                ddlBook.Items.Add(BookName)
                ddlBook.Items(i).Value = dt.Rows(i)("Book_Id").ToString()
            Next

        End If

    End Sub


    Private Function ChangeLevelIdToLevelName(ByVal LevelId As String)

        Dim LevelName As String = ""
        LevelId = LevelId.ToUpper()

        Select Case LevelId
            Case "5F4765DB-0917-470B-8E43-6D1C7B030818"
                LevelName = "ประถมศึกษาปีที่ 1"
            Case "EFA0855F-7AA5-40C1-98D0-F332F1298CEE"
                LevelName = "ประถมศึกษาปีที่ 2"
            Case "5CAF2A9B-B26B-4C16-9980-90BA760B5C43"
                LevelName = "ประถมศึกษาปีที่ 3"
            Case "DD73B147-B098-4F1D-8144-C5FCF510AEA9"
                LevelName = "ประถมศึกษาปีที่ 4"
            Case "BCBCC0C8-2A39-4AAE-9AA6-173DE86AF6AE"
                LevelName = "ประถมศึกษาปีที่ 5"
            Case "93B163B6-4F87-476D-8571-4029A6F34C84"
                LevelName = "ประถมศึกษาปีที่ 6"
            Case "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C"
                LevelName = "มัธยมศึกษาปีที่ 1"
            Case "DB95E7F8-7BF3-468D-AD9E-0AAF1B328D45"
                LevelName = "มัธยมศึกษาปีที่ 2"
            Case "14A28F3D-1AFF-429D-B7A1-927A28E010BD"
                LevelName = "มัธยมศึกษาปีที่ 3"
            Case "2E0FFC04-BCEE-45BE-9C0C-B40742523F43"
                LevelName = "มัธยมศึกษาปีที่ 4"
            Case "6736D029-6B78-4570-9DBB-991217DA8FEE"
                LevelName = "มัธยมศึกษาปีที่ 5"
            Case "6BF52DC7-314C-40ED-B7F3-BCC87F724880"
                LevelName = "มัธยมศึกษาปีที่ 6"
            Case "4BE1B530-E259-41F6-81F9-2A384B1BBC31"
                LevelName = "อนุบาล 1"
            Case "881AE7C9-C506-4305-92C9-864C49754FE9"
                LevelName = "อนุบาล 2"
            Case "EB78523F-49B1-4A7D-B2FD-F9A0446A6E8D"
                LevelName = "อนุบาล 3"
            Case Else
                LevelName = ""
        End Select

        Return LevelName

    End Function



    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        Dim BookId As String = ddlBook.SelectedValue.ToString()

        Dim sql As String = " UPDATE dbo.tblQuestionCategory SET Book_Id = '" & BookId & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE QCategory_Id = '" & QuestionCategoryId & "' "

        Try
            _DB.Execute(sql)
            Response.Redirect("~/TestSet/Step3.aspx")
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Response.Write(ex.ToString())
        End Try

    End Sub




End Class