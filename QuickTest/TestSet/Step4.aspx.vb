Imports KnowledgeUtils
Imports BusinessTablet360
Public Class Step4
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    Public GroupName As String
    'Dim ClsSelectSess As New ClsSelectSession
    Public CheckPostback As Boolean = False
    Protected IsAndroid As Boolean
    Public IE As String
    Protected EditTestSetWarningText As String = CommonTexts.EditTestSetWarningText

    Private TempTestset As Testset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
        If AgentString.ToLower().IndexOf("android") <> -1 Then
            IsAndroid = True
        End If
#If IE = "1" Then
        Session("userid") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238"
        IE = "1"
#End If
        Log.Record(Log.LogType.PageLoad, "pageload step4", False)
        If (Session("UserId") Is Nothing) Then
            Log.Record(Log.LogType.PageLoad, "step4 session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        Else
            If ClsKNSession.IsAddQuestionBySchool Then
                TempTestset = Session("TempTestset")
                If Not IsPostBack Then
                    txtName.Text = TempTestset.Name
                    ChkIsQuiz.Checked = TempTestset.IsQuizMode
                    ChkIsHomework.Checked = TempTestset.IsHomeworkMode
                    ChkIsPractice.Checked = TempTestset.IsPracticeMode
                    ChkIsPrintTestset.Checked = TempTestset.IsPracticeMode
                End If
            Else
                If IsNothing(Session("objTestSet")) Then
                    objTestSet = New ClsTestSet(Session("userid"))
                    Session("objTestSet") = objTestSet
                Else
                    objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
                End If

                If Not IsPostBack Then
                    If Not Session("newTestSetId") Is Nothing Then
                        Dim dt As DataTable = objTestSet.GetTestsetName(Session("newTestSetId").ToString())
                        If dt.Rows.Count > 0 Then
                            Dim TestsetName As String = dt.Rows(0)("TestSet_Name").ToString()
                            If TestsetName = "กำลังอยู่ระหว่างการจัดชุด" Then
                                TestsetName = ""
                            End If
                            txtName.Text = TestsetName
                            ChkIsQuiz.Checked = dt.Rows(0)("IsQuizMode")
                            ChkIsHomework.Checked = dt.Rows(0)("IsHomeworkMode")
                            ChkIsPractice.Checked = dt.Rows(0)("IsPracticeMode")
                            ChkIsPrintTestset.Checked = If(TestsetName = "", True, dt.Rows(0)("IsReportMode"))
                        End If
                    End If
                End If
            End If

            If IsPostBack Then
                If txtName.Text = "" Then
                    CheckPostback = False
                    lblValidate.Text = "* ต้องกรอกชื่อด้วยนะคะ"
                    lblValidate.Visible = True
                ElseIf CheckDuplicateTestsetName(txtName.Text().ToString(), Session("userid").ToString) Then
                    CheckPostback = False
                    lblValidate.Text = "* ชื่อชุดซ้ำนะคะ"
                    lblValidate.Visible = True
                ElseIf Not ValidateTestsetName(txtName.Text().ToString()) Then
                    CheckPostback = False
                    lblValidate.Text = "* ห้ามตั้งชื่อชุดที่ครอบด้วย <...> นะคะ"
                    lblValidate.Visible = True
                ElseIf Not IsChooseTestsetType() Then
                    CheckPostback = False
                    lblValidate.Text = "* เลือกประเภทของชุดก่อนค่ะ (อย่างน้อย 1 ประเภท)"
                    lblValidate.Visible = True
                Else
                    CheckPostback = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        If ClsKNSession.IsAddQuestionBySchool Then
            Log.Record(Log.LogType.ExamStep, "กลับไปยังหน้าสรุปหน่วยการเรียนรู้ที่เลือกมา", True)
            Response.Redirect("../Testset/testsetsummaryPage.aspx")
        Else
            Log.Record(Log.LogType.ExamStep, "กลับไปยังหน้าเลือกหน่วยการเรียนรู้", True)
            Response.Redirect("../Testset/Step3.aspx")
        End If
    End Sub

    Private Function IsChooseTestsetType() As Boolean
        If ChkIsPractice.Checked Or ChkIsPrintTestset.Checked Or ChkIsQuiz.Checked Or ChkIsHomework.Checked Then
            Return True
        End If
        Return False
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        If (txtName.Text <> "") And (CheckDuplicateTestsetName(txtName.Text().ToString(), Session("userid").ToString) = False) And (IsChooseTestsetType()) Then
            Dim TestsetName As String = txtName.Text().ToString()
            TestsetName = Regex.Replace(TestsetName, "<.*?>", String.Empty)
            Dim TestsetID As String = Session("newTestSetId").ToString()
            Dim IsQuizMode As Boolean = ChkIsQuiz.Checked
            Dim IsHomeWorkMode As Boolean = ChkIsHomework.Checked
            Dim IsPracticeMode As Boolean = ChkIsPractice.Checked
            Dim IsReportMode As Boolean = ChkIsPrintTestset.Checked

            If ClsKNSession.IsAddQuestionBySchool Then
                TempTestset.Name = TestsetName
                TempTestset.IsHomeworkMode = IsHomeWorkMode
                TempTestset.IsPracticeMode = IsPracticeMode
                TempTestset.IsReportMode = IsReportMode
                TempTestset.IsQuizMode = IsQuizMode
                Dim KnSession As New KNAppSession()
                Dim ts As New TestsetManagement(Session("userid").ToString, KnSession.StoredValue("CurrentCalendarId").ToString(), TempTestset.Id, Session("SchoolID").ToString())
                If Not ts.SaveTestset(TempTestset) Then
                    Exit Sub
                End If
            Else
                objTestSet.SaveTestSet(TestsetName, TestsetID, IsQuizMode, IsHomeWorkMode, IsPracticeMode, IsReportMode)
                If Not objTestSet.SelectedSyllabusYear = "y51y44" Then
                    objTestSet.CheckSallyBus(Session("newTestSetId"), objTestSet.SelectedSyllabusYear)
                End If

                Log.Record(Log.LogType.ExamStep, "บันทึกข้อสอบและไปหน้าพิมพ์ข้อสอบ", True)
            End If

        Else
            Exit Sub
        End If
    End Sub


    ''' <summary>
    ''' function ในการเช็คว่า ชุดที่เรากำลังจะ save มีชื่อทับกับ ที่มีอยู่แล้วหรือเปล่า
    ''' </summary>
    ''' <param name="TestsetName"></param>
    ''' <param name="UserId"></param>
    ''' <returns>Boolean</returns>
    Private Function CheckDuplicateTestsetName(TestsetName As String, UserId As String) As Boolean
        Dim sb As New StringBuilder
        Dim dt As New DataTable
        Dim db As New ClassConnectSql()

        sb.Append("Select a.testset_Name,a.TestSet_Id from (select REPLACE(TestSet_Name,' ','') as testset_Name,TestSet_Id from tbltestset where IsStandard = '0' and UserId = '")
        sb.Append(UserId)
        sb.Append("' and IsActive = 1) a  where  a.testset_Name like replace('%")
        sb.Append(TestsetName.CleanSQL)
        sb.Append("%',' ','');")
        dt = db.getdata(sb.ToString)

        If dt.Rows.Count > 0 Then
            If ClsKNSession.IsAddQuestionBySchool Then
                If dt.Rows(0)("TestSet_Id").ToString().ToUpper() = TempTestset.Id.ToString().ToUpper() Then
                    Return False
                End If
            Else
                If dt.Rows(0)("TestSet_Id").ToString() = HttpContext.Current.Session("newTestSetId") Then
                    Return False ' ถือว่าเป็นชุดที่เราแก้ไข
                End If
            End If
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' function เช็คว่าชื่อชุดของ testset ถูกเปิดปิดด้วย <> หรือเปล่า
    ''' </summary>
    ''' <param name="TestsetName"></param>
    ''' <returns>Boolean</returns>
    Private Function ValidateTestsetName(TestsetName As String) As Boolean
        Dim FirstCharactor As String = TestsetName.Substring(0, 1)
        Dim LastCharactor As String = TestsetName.Substring(TestsetName.Length - 1, 1)

        If FirstCharactor = "<" And LastCharactor = ">" Then
            Return False
        End If
        Return True
    End Function
End Class