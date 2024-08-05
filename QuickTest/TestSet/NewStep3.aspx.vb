Imports System.Data.SqlClient
Imports System.Web
Public Class NewStep3
    Inherits System.Web.UI.Page

    Protected ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String

    Protected IsAndroid As Boolean
    Protected EditTestSetWarningText As String = CommonTexts.EditTestSetWarningText

    Private UserId As String
    Private TempTestset As Testset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim AgentString As String = HttpContext.Current.Request.UserAgent.ToString()
            If AgentString.ToLower().IndexOf("android") <> -1 Then
                IsAndroid = True
            End If
        End If

        Log.Record(Log.LogType.PageLoad, "pageload new step3", False)
        If (Session("UserId") Is Nothing) Then
            Log.Record(Log.LogType.PageLoad, "newstep3 session หลุด", False)
            Response.Redirect("~/LoginPage.aspx")
        Else

            UserId = Session("UserId").ToString()

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

            ' NeedJoinQ40 = HttpContext.Current.Application("NeedJoinQ40")
            If Session("TempTestset") Is Nothing Then Response.Redirect("~/DashboardSetupPage.aspx")
            TempTestset = Session("TempTestset")
            Session("newTestSetId") = TempTestset.Id

            If Not Page.IsPostBack() Then
                ' clear ข้อสอบที่ได้ทำการปลดข้อสอบทิ้งไปแล้วจากหน้าเลือกวิชา
                TempTestset.ClearQuestionsNotSelected()

                Repeater1.DataSource = GetSelectedSubject()
                Repeater1.DataBind()
            End If
        End If
    End Sub

    Private Sub ListingSubject_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Repeater1.ItemDataBound
        Dim item As RepeaterItem = e.Item
        Dim ListingClass As Repeater
        If (item.ItemType = ListItemType.Item) OrElse (item.ItemType = ListItemType.AlternatingItem) Then
            ListingClass = DirectCast(item.FindControl("ListingClass"), Repeater)
            Dim drv As DataRowView = DirectCast(item.DataItem, DataRowView)
            ListingClass.DataSource = drv.CreateChildView("SubjectToClass")
            ListingClass.DataBind()
        End If
    End Sub

    Protected Function CreateQuestionAmount(classId As String, subjectId As String) As String
        Dim syllabusYear As String = "51"
        Dim sql As String = "SELECT QuestionAmount FROM uvw_QuestionAmountOfSubjectAndClass WHERE GroupSubject_Id = '" & subjectId & "' AND Level_Id = '" & classId & "';"
        Dim db As New ClassConnectSql()
        Dim result As String = db.ExecuteScalar(sql)
        Dim questionAmount As Integer = If((result = ""), 0, CInt(result))
        Dim questionSelectedAmount As Integer = GetQuestionAmount(subjectId, classId, True)
        Return String.Format("จากทั้งหมด {0:n0} ข้อ เลือกมาแล้ว {1:n0} ข้อ", questionAmount, questionSelectedAmount)
    End Function

    Protected Function GetQuestionAmount(subjectId As String, classId As String, isWpp As Boolean) As Integer
        Return TempTestset.GetSubjectClassQuestionSelectedAmount(subjectId, classId, isWpp)
    End Function

    ''' <summary>
    ''' function ในการสร้าง content ให้เลือกข้อสอบแบบที่สร้างขึ้นเองโดย User
    ''' </summary>
    ''' <param name="classId"></param>
    ''' <param name="subjectId"></param>
    ''' <returns>String</returns>
    Protected Function CreateContentQuestionByUser(classId As String, subjectId As String) As String
        Dim bookManagement As New BookManagement(UserId)
        Dim content As String = ""
        Dim dt As DataTable = bookManagement.GetQsetsByUser(subjectId, classId)
        If dt.Rows.Count > 0 Then
            Dim questionAmount As Integer = 0
            For Each r In dt.Rows
                questionAmount += If((r("Qset_Type") = EnumQsetType.Pair Or r("Qset_Type") = EnumQsetType.Sort), 1, r("QuestionAmount"))
            Next
            Dim questionSelectedAmount As Integer = GetQuestionAmount(subjectId, classId, False)
            Dim txtQuestionAmount As String = String.Format("จากทั้งหมด {0:n0} ข้อ เลือกมาแล้ว {1:n0} ข้อ", questionAmount, questionSelectedAmount)
            content &= "<div style='padding:5px;' ></div><span>ชั้น " & classId.ToLevelShortName() & " (ข้อสอบที่โรงเรียนเพิ่มเอง)</span><table><tr>"
            content &= "<td style='text-align:left;'><input type='text' class='qtipQcategoryByUser' subjectid='" & subjectId & "' classid='" & classId & "' placeholder='เลือกหน่วยการเรียนรู้ (นำเมาส์มาลอยเพื่อเลือกค่ะ)' /></td>"
            'content &= "<td style='text-align:center;'><input type='text' class='qtipEvalutionByUser' subjectid='" & subjectId & "' classid='" & classId & "'/></td>"
            content &= "</tr><tr><td colspan = '2' style='padding-left:25px;'><span class='spnSelectQuestion " & subjectId & classId & "_user' selectQuestionAmount='" & questionSelectedAmount & "'>" & txtQuestionAmount & "</span></td></tr></table>"
        End If
        Return content
    End Function

    Protected Sub btnNextStep4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNextStep4.Click
        Dim tempTestset As Testset = Session("TempTestset")
        If tempTestset.SeletedQsetAmount > 0 Then
            Log.Record(Log.LogType.ExamStep, "ไปหน้าสรุปข้อสอบที่เลือกมา", True)
            'Response.Redirect("~/testset/TestsetSummaryPage.aspx", False)
            Response.Redirect("~/testset/SelectQuestionsByUser.aspx", False)
        Else
            Log.Record(Log.LogType.ExamStep, "ไปหน้าสรุปข้อสอบ ไม่ได้เพราะไม่ได้เลือกข้อสอบ", True)
        End If
    End Sub

    ''' <summary>
    ''' function หาว่าจำนวนข้อสอบที่เลือกมาแล้วมาทั้งหมดกี่ข้อ
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetQuestionAmount() As String
        Return TempTestset.QuestionAmount
    End Function

    ''' <summary>
    ''' function ในการหา ว่าวิชาที่เลือกมา มีชั้นอะไรบ้าง วิชาอะไรบ้าง
    ''' </summary>
    ''' <returns>dataset</returns>
    Private Function GetSelectedSubject() As DataSet
        Dim dt As New System.Data.DataTable()
        dt.TableName = "Subjects"
        dt.Columns.Add("SubjectId", GetType(String))
        dt.Columns.Add("SubjectName", GetType(String))

        Dim dt2 As New System.Data.DataTable()
        dt2.TableName = "Classes"
        dt2.Columns.Add("SubjectId", GetType(String))
        dt2.Columns.Add("ClassId", GetType(String))
        dt2.Columns.Add("ClassName", GetType(String))

        For Each subjectId In TempTestset.SubjectClassSelected.Keys
            dt.Rows.Add(subjectId, subjectId.ToSubjectShortThName)
            Dim listClassId As List(Of String) = TempTestset.SubjectClassSelected(subjectId)
            For Each classId In listClassId
                dt2.Rows.Add(subjectId, classId, classId.ToLevelShortName)
            Next
        Next

        Dim ds As New DataSet
        ds.Tables.Add(dt)
        ds.Tables.Add(dt2)

        ds.Relations.Add(New DataRelation("SubjectToClass", ds.Tables(0).Columns("SubjectId"), ds.Tables(1).Columns("SubjectId")))

        Return ds
    End Function

End Class
