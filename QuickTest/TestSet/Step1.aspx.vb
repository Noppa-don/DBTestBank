Imports System.Web
Imports KnowledgeUtils

Public Class Step1
    Inherits System.Web.UI.Page
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    Public GroupName As String
    Dim ClsSelectSess As New ClsSelectSession
    Dim _DB As New ClassConnectSql()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If DEBUG Then
        'Session("userid") = "2"
#End If
        If (Session("UserId") Is Nothing) Then
            Response.Redirect("~/LoginPage.aspx")
        Else

            GroupName = Session("selectedSession").ToString() 'GroupName

            If HttpContext.Current.Application("NeedQuizMode") = True Then
                ChkFontSize = "16px"
                txtStep1 = "ขั้นที่ 1: จัดชุดควิซ -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึกชุดควิซ -->"
            Else
                ChkFontSize = "20px"
                txtStep1 = "ขั้นที่ 1: จัดชุด -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึก และจัดพิมพ์"
            End If

            ClsSelectSess.checkCurrentPage(Session("UserId").ToString(), Session("selectedSession").ToString())

            If Not Page.IsPostBack() Then
                Dim objTestSet As ClsTestSet
                If IsNothing(Session("objTestSet")) Then
                    objTestSet = New ClsTestSet(Session("userid").ToString)
                    Session("objTestSet") = objTestSet
                    ClsSelectSess.setApplicationWhenChangeCurrentPage("", objTestSet)
                Else
                    objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
                End If


                Listing.DataSource = objTestSet.GetAllTestSet()
                Listing.DataBind()

                'objTestSet.SelectedSubjectClass = Nothing
                'objTestSet.SelectedSyllabusYear = "y55"
                Session("newTestSetId") = ""

                'SignalR And SelectSession
                'ClsSelectSess.resetValueInSession()
                '
                BindingRepeater()

            End If
        End If
    End Sub
    Private Sub BindingRepeater()

        Dim strSQL As String = ""
        strSQL = " SELECT tblTestSet.TestSet_Id, tblTestSet.LastUpdate, tblTestSet.TestSet_Name"
        strSQL &= " FROM tblTestSet INNER JOIN"
        strSQL &= " tblModuleDetail ON tblTestSet.TestSet_Id = tblModuleDetail.Reference_Id"
        strSQL &= " where tblTestSet.IsActive = 1"
        strSQL &= " and tbltestset.IsHomeWorkMode = 1"
        strSQL &= " and tblTestSet.UserId = '" & HttpContext.Current.Session("UserId").ToString() & "'"

        Dim ds As New DataSet
        ds = _DB.getdataset(strSQL)
        rptHomework.DataSource = ds
        rptHomework.DataBind()


    End Sub

    <Services.WebMethod()> Public Shared Sub UpdateTestSetIdCodeBehind(ByVal TestsetId As String)
        Dim ClsData As New ClassConnectSql
        Dim ClsCheckMark As New ClsCheckMark
        Dim sqlUpdateTestSetId As String = "Update tblTestSet Set testset_name = N'ถูกลบโดย user แล้ว', IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL Where TestSet_Id = '" & TestsetId.CleanSQL & "';"
        ClsData.Execute(sqlUpdateTestSetId)

        ' get reftocheckmark for update active = 2 on temptblsetup when user del testset
        'Dim sqlGetReftoCheckMark As String = " SELECT RefToCheckMark,User_Id FROM tblTestSet ts"
        'sqlGetReftoCheckMark &= " LEFT JOIN tblQuiz qu "
        'sqlGetReftoCheckMark &= " ON ts.TestSet_Id = qu.TestSet_Id "
        'sqlGetReftoCheckMark &= " WHERE ts.TestSet_Id = '" & TestsetId & "';"

        Dim sqlGetReftoCheckMark As String = " SELECT SetupAnswer_ID FROM tblTestSet_CM_temptblsetup WHERE TestSet_Id = '" & TestsetId.CleanSQL & "';"

        Dim refToCheckMark As DataTable = ClsData.getdata(sqlGetReftoCheckMark)
        ClsCheckMark.updateActiveCheckMarkWhenUserDelTestset(refToCheckMark)

    End Sub


    Public Sub save()
        Log.Record(Log.LogType.ExamStep, "จัดชุดใหม่", True)
        Response.Redirect("step2.aspx")
        Dim objTestSet As ClsTestSet
        objTestSet.SelectedSubjectClass = Nothing
    End Sub

End Class
