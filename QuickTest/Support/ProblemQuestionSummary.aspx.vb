Public Class ProblemQuestionSummary
    Inherits System.Web.UI.Page

    Public Shared pmtVal As String
    Public Shared userId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        pmtVal = Request.QueryString("pmt").ToString
        userId = Request.QueryString("usid").ToString
        If Not Page.IsPostBack Then

            Dim dtProblem As DataTable = GetAllQuestions(1)
            rptProblem.DataSource = dtProblem
            rptProblem.DataBind()
            lblProblem.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"

            pnProblem.Visible = True
            pnEdited.Visible = False
            pnComplete.Visible = False

            Dim dtLevel As DataTable = GetLevel(1)
            ddlLevelProblem.DataValueField = "Level_Id"
            ddlLevelProblem.DataTextField = "Level_ShortName"
            ddlLevelProblem.DataSource = dtLevel
            ddlLevelProblem.DataBind()

            Dim dtGroup As DataTable = GetGroupSubject(1)
            ddlGroupSubjectProblem.DataValueField = "GroupSubject_id"
            ddlGroupSubjectProblem.DataTextField = "GroupSubject_ShortName"
            ddlGroupSubjectProblem.DataSource = dtGroup
            ddlGroupSubjectProblem.DataBind()
        End If



    End Sub

#Region "Menu Button"
    Private Sub btnProblem_Click(sender As Object, e As EventArgs) Handles btnProblem.Click

        btnProblem.BackColor = Drawing.Color.BurlyWood
        btnEdited.BackColor = Drawing.Color.White
        btnComplete.BackColor = Drawing.Color.White

        Dim dtProblem As DataTable = GetAllQuestions(1)
        rptProblem.DataSource = dtProblem
        rptProblem.DataBind()
        lblProblem.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"

        Dim dtLevel As DataTable = GetLevel(1)
        ddlLevelProblem.DataValueField = "Level_Id"
        ddlLevelProblem.DataTextField = "Level_ShortName"
        ddlLevelProblem.DataSource = dtLevel
        ddlLevelProblem.DataBind()

        Dim dtGroup As DataTable = GetGroupSubject(1)
        ddlGroupSubjectProblem.DataValueField = "GroupSubject_id"
        ddlGroupSubjectProblem.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectProblem.DataSource = dtGroup
        ddlGroupSubjectProblem.DataBind()

        pnProblem.Visible = True
        pnEdited.Visible = False
        pnComplete.Visible = False
    End Sub
    Private Sub btnEdited_Click(sender As Object, e As EventArgs) Handles btnEdited.Click

        btnProblem.BackColor = Drawing.Color.White
        btnEdited.BackColor = Drawing.Color.BurlyWood
        btnComplete.BackColor = Drawing.Color.White

        Dim dtEdited As DataTable = GetAllQuestions(2)
        rptEdited.DataSource = dtEdited
        rptEdited.DataBind()
        lblEdited.Text = "ทั้งหมด " & dtEdited.Rows.Count & " ข้อ"

        Dim dtLevel As DataTable = GetLevel(2)
        ddlLevelProblem.DataValueField = "Level_Id"
        ddlLevelProblem.DataTextField = "Level_ShortName"
        ddlLevelProblem.DataSource = dtLevel
        ddlLevelProblem.DataBind()

        Dim dtGroup As DataTable = GetGroupSubject(2)
        ddlGroupSubjectProblem.DataValueField = "GroupSubject_id"
        ddlGroupSubjectProblem.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectProblem.DataSource = dtGroup
        ddlGroupSubjectProblem.DataBind()

        pnProblem.Visible = False
        pnEdited.Visible = True
        pnComplete.Visible = False
    End Sub
    Private Sub btnComplete_Click(sender As Object, e As EventArgs) Handles btnComplete.Click

        btnProblem.BackColor = Drawing.Color.White
        btnEdited.BackColor = Drawing.Color.White
        btnComplete.BackColor = Drawing.Color.BurlyWood

        Dim dtComplete As DataTable = GetAllQuestions(4)

        rptComplete.DataSource = dtComplete
        rptComplete.DataBind()
        lblComplete.Text = "ทั้งหมด " & dtComplete.Rows.Count & " ข้อ"

        Dim dtLevel As DataTable = GetLevel(4)
        ddlLevelProblem.DataValueField = "Level_Id"
        ddlLevelProblem.DataTextField = "Level_ShortName"
        ddlLevelProblem.DataSource = dtLevel
        ddlLevelProblem.DataBind()

        Dim dtGroup As DataTable = GetGroupSubject(4)
        ddlGroupSubjectProblem.DataValueField = "GroupSubject_id"
        ddlGroupSubjectProblem.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectProblem.DataSource = dtGroup
        ddlGroupSubjectProblem.DataBind()

        pnProblem.Visible = False
        pnEdited.Visible = False
        pnComplete.Visible = True
    End Sub
#End Region

#Region "DDL"
    Private Sub ddlLevelProblem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLevelProblem.SelectedIndexChanged
        Dim dtGroup As DataTable = GetGroupSubject(1, ddlLevelProblem.SelectedValue)
        ddlGroupSubjectProblem.DataValueField = "GroupSubject_id"
        ddlGroupSubjectProblem.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectProblem.DataSource = dtGroup
        ddlGroupSubjectProblem.DataBind()

        Dim dtProblem As DataTable = GetAllQuestions(1, ddlLevelProblem.SelectedValue, ddlGroupSubjectProblem.SelectedValue)
        rptProblem.DataSource = dtProblem
        rptProblem.DataBind()
        lblProblem.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"

    End Sub
    Private Sub ddlGroupSubjectProblem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroupSubjectProblem.SelectedIndexChanged

        Dim dtProblem As DataTable = GetAllQuestions(1, ddlLevelProblem.SelectedValue, ddlGroupSubjectProblem.SelectedValue)
        rptProblem.DataSource = dtProblem
        rptProblem.DataBind()
        lblProblem.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"

    End Sub
    Private Sub ddlLevelEdited_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLevelEdited.SelectedIndexChanged
        Dim dtGroup As DataTable = GetGroupSubject(2, ddlLevelEdited.SelectedValue)
        ddlGroupSubjectEdited.DataValueField = "GroupSubject_id"
        ddlGroupSubjectEdited.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectEdited.DataSource = dtGroup
        ddlGroupSubjectEdited.DataBind()

        Dim dtProblem As DataTable = GetAllQuestions(2, ddlLevelEdited.SelectedValue, ddlGroupSubjectEdited.SelectedValue)
        rptEdited.DataSource = dtProblem
        rptEdited.DataBind()
        lblEdited.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"
    End Sub
    Private Sub ddlGroupSubjectEdited_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroupSubjectEdited.SelectedIndexChanged
        Dim dtProblem As DataTable = GetAllQuestions(2, ddlLevelEdited.SelectedValue, ddlGroupSubjectEdited.SelectedValue)
        rptEdited.DataSource = dtProblem
        rptEdited.DataBind()
        lblEdited.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"
    End Sub
    Private Sub ddlLevelComplete_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLevelComplete.SelectedIndexChanged
        Dim dtGroup As DataTable = GetGroupSubject(4, ddlLevelComplete.SelectedValue)
        ddlGroupSubjectComplete.DataValueField = "GroupSubject_id"
        ddlGroupSubjectComplete.DataTextField = "GroupSubject_ShortName"
        ddlGroupSubjectComplete.DataSource = dtGroup
        ddlGroupSubjectComplete.DataBind()

        Dim dtProblem As DataTable = GetAllQuestions(4, ddlLevelComplete.SelectedValue, ddlGroupSubjectComplete.SelectedValue)
        rptComplete.DataSource = dtProblem
        rptComplete.DataBind()
        lblComplete.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"
    End Sub
    Private Sub ddlGroupSubjectComplete_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGroupSubjectComplete.SelectedIndexChanged
        Dim dtProblem As DataTable = GetAllQuestions(4, ddlLevelComplete.SelectedValue, ddlGroupSubjectComplete.SelectedValue)
        rptComplete.DataSource = dtProblem
        rptComplete.DataBind()
        lblComplete.Text = "ทั้งหมด " & dtProblem.Rows.Count & " ข้อ"
    End Sub
#End Region

#Region "Function"
    Private Function GetAllQuestions(ProblemStatus As Integer, Optional LevelId As String = "5B882FD4-4D06-4CF4-B7BD-E18CF1228D4E", Optional GroupSubjectId As String = "B453BE26-3CD5-4239-A14F-A373AEC48969") As DataTable
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = "select * from (select RANK() OVER (PARTITION BY qp.QPId ORDER BY qpd.LastUpdate desc) DateRank ,ROW_NUMBER() over (order by QP.lastupdate) as AnnoNo,g.GroupSubject_ShortName as SubjectName,
                Level_ShortName as LevelName,QCategory_Name as QCName,QSet_Name as QSName,Question_No as QNo,'<b>หัวข้อ</b> :: ' + ProblemTopic + '<br><b>รายละเอียด</b> :: ' + ProblemDetail as Annotation,
                QP.LastUpdate as ReportTime,s.Student_FirstName as UserName,QPD.ProblemStatus as ReportStatus,QP.QuestionId,qp.QPId,ProblemRemark,qpd.LastUpdate
                from t360_tblStudent s inner join tblQuestionProblemDetail QPD on s.Student_Id = QPD.ReporterId inner join tblQuestionProblem QP on QPD.QPId = QP.QPId and QPD.ProblemStatus = qp.CurrentStatus 
                inner join tblQuestion q on qp.QuestionId = q.Question_Id inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id 
                inner join tblbook b on qc.Book_Id = b.BookGroup_Id inner join tblGroupSubject g on b.GroupSubject_Id = g.GroupSubject_Id
                inner join tblLevel l on b.Level_Id = l.Level_Id
                where (QP.CurrentStatus = " & ProblemStatus

        If ProblemStatus = 1 Then
            sql &= " Or QP.CurrentStatus = 3)"
        Else
            sql &= ") "
        End If

        If (pmtVal = 1) Then
            sql &= " And QP.ReporterId = '" & userId & "'"
        End If

        If LevelId.ToUpper <> "5B882FD4-4D06-4CF4-B7BD-E18CF1228D4E" Then
            sql &= " And l.Level_Id = '" & LevelId & "'"
        End If

        If GroupSubjectId.ToUpper <> "B453BE26-3CD5-4239-A14F-A373AEC48969" Then
            sql &= " And g.GroupSubject_Id = '" & GroupSubjectId & "'"
        End If

        sql &= ")d where d.DateRank = 1  ORDER BY d.LastUpdate desc"

        Return db.getdata(sql.ToString())
    End Function
    Private Function GetLevel(ProblemStatus As Integer) As DataTable
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = "select distinct l.Level_ShortName,l.Level_Id
                from t360_tblStudent s inner join tblQuestionProblemDetail QPD on s.Student_Id = QPD.ReporterId 
                inner join tblQuestionProblem QP on QPD.QPId = QP.QPId and QPD.ProblemStatus = qp.CurrentStatus 
                inner join tblQuestion q on qp.QuestionId = q.Question_Id inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id inner join tblbook b on qc.Book_Id = b.BookGroup_Id 
                inner join tblGroupSubject g on b.GroupSubject_Id = g.GroupSubject_Id inner join tblLevel l on b.Level_Id = l.Level_Id
                where (QP.CurrentStatus = " & ProblemStatus

        If ProblemStatus = 1 Then
            sql &= " Or QP.CurrentStatus = 3)"
        Else
            sql &= ") "
        End If

        If (pmtVal = 1) Then
            sql &= " And QP.ReporterId = '" & userId & "'"
        End If

        sql &= " union Select 'ทั้งหมด','5B882FD4-4D06-4CF4-B7BD-E18CF1228D4E'"

        Return db.getdata(sql.ToString())
    End Function
    Private Function GetGroupSubject(ProblemStatus As Integer, Optional LevelId As String = "5B882FD4-4D06-4CF4-B7BD-E18CF1228D4E") As DataTable
        Dim db As New ClassConnectSql()
        Dim sql As String
        sql = "Select * from ( Select *,ROW_NUMBER() over (order by d.GroupSubject_shortname) As level 
                from (select distinct g.GroupSubject_ShortName,g.GroupSubject_id
                from t360_tblStudent s inner join tblQuestionProblemDetail QPD on s.Student_Id = QPD.ReporterId 
                inner join tblQuestionProblem QP on QPD.QPId = QP.QPId and QPD.ProblemStatus = qp.CurrentStatus 
                inner join tblQuestion q on qp.QuestionId = q.Question_Id inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id 
                inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id inner join tblbook b on qc.Book_Id = b.BookGroup_Id 
                inner join tblGroupSubject g on b.GroupSubject_Id = g.GroupSubject_Id inner join tblLevel l on b.Level_Id = l.Level_Id
                where (QP.CurrentStatus = " & ProblemStatus

        If ProblemStatus = 1 Then
            sql &= " Or QP.CurrentStatus = 3)"
        Else
            sql &= ") "
        End If

        If (pmtVal = 1) Then
            sql &= " And QP.ReporterId = '" & userId & "'"
        End If

        If LevelId.ToUpper <> "5B882FD4-4D06-4CF4-B7BD-E18CF1228D4E" Then
            sql &= " And l.Level_Id = '" & LevelId & "'"
        End If

        sql &= ")d union select 'ทั้งหมด','B453BE26-3CD5-4239-A14F-A373AEC48969',0)c order by c.level"

        Return db.getdata(sql.ToString())
    End Function
    Protected Sub GetProblemDetail(ByVal sender As Object, ByVal e As EventArgs)
        'Reference the Repeater Item using Button.
        Dim item As RepeaterItem = TryCast((TryCast(sender, Button)).NamingContainer, RepeaterItem)

        'Reference the Label and TextBox.
        Dim message As String = "QuestionId: " & (TryCast(item.FindControl("QuestionId"), Label)).Text

        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('" & message & "');", True)
    End Sub
#End Region

#Region "WebServices"
    <Services.WebMethod()>
    Public Shared Function UpdateProblem(ByVal QPId As String, RemarkDetail As String, ProblemType As String, UsId As String) As Boolean

        Dim db As New ClassConnectSql()
        Dim sql As String

        sql = "update tblQuestionProblem set CurrentStatus = " & ProblemType & " , LastUpdate = GETDATE() where QPId = '" & QPId & "';
                insert into tblQuestionProblemDetail(QPId,ProblemRemark,ProblemStatus,ReporterId)
                values('" & QPId & "','" & RemarkDetail & "','" & ProblemType & "','" & UsId & "')"

        db.Execute(sql.ToString())

        Return True
    End Function
    <Services.WebMethod()>
    Public Shared Function GetHistory(ByVal QPId As String) As String

        Dim db As New ClassConnectSql()
        Dim sql As String

        sql = "select ROW_NUMBER() over (order by qpd.lastupdate) as RowNo,qpd.LastUpdate,Student_FirstName,
                case when qpd.ProblemStatus = 1 then '<b>หัวข้อ</b> :: ' + ProblemTopic + '<br><br><b>รายละเอียด</b> :: ' + ProblemDetail 
                when qpd.ProblemStatus = 4 then 'อนุมัติเรียบร้อย' else ProblemRemark end as ProblemDetail
                from tblQuestionProblem qp inner join tblQuestionProblemDetail qpd on qp.QPId = qpd.QPId inner join t360_tblstudent on qpd.ReporterId = Student_Id 
                where qp.QPId = '" & QPId & "' order by qpd.LastUpdate"

        Dim dtHistory As DataTable = db.getdata(sql.ToString())

        Dim RowColour As String = "#dad8d8"

        Dim strHistory As String = "<table>"
        strHistory &= "<tr><td class=""tdHeader"" colspan=""3"">" & dtHistory.Rows(0)("ProblemDetail").ToString & "</td></tr>"
        strHistory &= "<tr style=""height:20px;""><td colspan=""3""></td></tr><tr><td style=""background-color:" & RowColour & ";height: 40px;text-align:center;"">วันที่</td>"
        strHistory &= "<td style=""background-color:" & RowColour & ";height: 40px;text-align:center;"">ผู้แจ้ง</td>"
        strHistory &= "<td style=""background-color:" & RowColour & ";height: 40px;text-align:center;"">รายละเอียด</td></tr>"

        dtHistory.Rows(0)("ProblemDetail") = "แจ้งปัญหา"


        For Each eachHis In dtHistory.Rows
            If CInt(eachHis("RowNo")) Mod 2 = 1 Then
                RowColour = "#e4a5c5"
            Else
                RowColour = "#dad8d8"
            End If
            strHistory &= "<tr><td style=""height: 40px;padding-left: 10px;padding-right: 10px;background-color:" & RowColour & ";"">" & eachHis("LastUpdate").ToString & "</td>"
            strHistory &= "<td style=""height: 40px;padding-left: 10px;padding-right: 10px;background-color:" & RowColour & ";"">" & eachHis("Student_FirstName").ToString & "</td>"
            strHistory &= "<td style=""height: 40px;padding-left: 10px;padding-right: 10px;background-color:" & RowColour & ";"">" & eachHis("ProblemDetail").ToString & "</td></tr>"
        Next

        strHistory &= "</table>"

        Return strHistory
    End Function






#End Region

End Class