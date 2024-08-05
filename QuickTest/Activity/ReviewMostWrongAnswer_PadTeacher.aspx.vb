Imports System.Web
Public Class ReviewMostWrongAnswer_PadTeacher
    Inherits System.Web.UI.Page
    Dim UseCls As New ClassConnectSql
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    Dim clsPDf As New ClsPDF(New ClassConnectSql)
    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    Public DVID As String
    Public Property ExamNum As Integer ' None 0, Question 1, CorrectAnswer 2
        Get
            Return ViewState("ExamNum")
        End Get
        Set(ByVal value As Integer)
            ViewState("ExamNum") = value
        End Set
    End Property

    Public Property ExamAmount As Integer
        Get
            Return ViewState("ExamAmount")
        End Get
        Set(ByVal value As Integer)
            ViewState("ExamAmount") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            ExamNum = 1
            renderExam(ExamNum)
        End If
    End Sub

    Protected Sub BtnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        ExamNum += 1
        If (ExamNum > ExamAmount) Then
            'dialog
        Else
            renderExam(ExamNum)
        End If
    End Sub

    Protected Sub BtnPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrev.Click
        ExamNum = ExamNum - 1
        If (ExamNum >= 1) Then
            renderExam(ExamNum)
        Else
            ExamNum = 1
        End If
    End Sub
    Private Sub renderExam(ByVal ExamNum As String)

        'If Request.QueryString("DeviceUniqueID") = Nothing Then
        '    Response.Redirect("../Activity/EmptySession.aspx")
        'End If

        'Dim DeviceId As String = Request.QueryString("DeviceUniqueID").ToString()
        Dim DeviceId = "teacher1"

        'If HttpContext.Current.Application("Sess" & DeviceId) Is Nothing Then
        '    Response.Redirect("../Activity/EmptySession.aspx")
        'End If

        If DeviceId <> "" Then
            'DVID = DeviceId
            Dim dt1 As New DataTable
            Dim Quiz_Id As String = ""
            Dim Player_Id As String = ""
            'dt1 = ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceId)

            dt1 = GetQuizFromDeviceUniqueID(DeviceId)

            If (dt1.Rows.Count > 0) Then
                Quiz_Id = dt1.Rows(0)("Quiz_Id").ToString()
                'Quiz_Id = "6A35CD11-6C3D-4B84-9B80-78A9C2B49DE6"
                Player_Id = dt1.Rows(0)("Player_Id").ToString()
                Session("Quiz_Id") = Quiz_Id
                If (ExamAmount = 0) Then
                    ExamAmount = ClsActivity.GetExamAmount(Quiz_Id)
                End If
                lblTestsetName.Text = ClsActivity.GetTestsetName(Quiz_Id)
                mainQuestion.InnerHtml = ClsActivity.RenderQuestion(Quiz_Id, Player_Id, 2, ExamNum, False)
                If Session("ChooseMode") = EnumDashBoardType.PracticeFromComputer Then
                    AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Player_Id, 2, Quiz_Id, ExamNum, Session("PracticeMode"), False, Session("HomeworkMode"), True)
                Else
                    AnswerTbl.InnerHtml = ClsActivity.RenderAnswer(Player_Id, 2, Quiz_Id, ExamNum, Session("PracticeMode"), False, Session("HomeworkMode"), False)
                End If

            End If
        End If
    End Sub

    Private Function GetQuizFromDeviceUniqueID(ByVal DeviceUniqueID As String) As DataTable
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable
        Dim sql As String = " select top 1 tqs.Quiz_Id,tqs.Player_Id,tqs.School_Code from t360_tblTablet tt inner join t360_tblTabletOwner tto "
        sql &= " on tt.Tablet_Id = tto.Tablet_Id inner join tblQuizSession tqs on tqs.Tablet_Id = tto.Tablet_Id "
        sql &= " where tt.Tablet_SerialNumber = '" & db.CleanString(DeviceUniqueID.Trim()) & "'  order by tqs.LastUpdate desc ; "
        dt = db.getdata(sql)
        Return dt
    End Function


    <Services.WebMethod()>
    Public Shared Function CreateStringLeapChoice(ByVal IsNormalSort As String) 'Function สร้างปุ่มกดข้ามข้อ


        'If DeviceUniqueId Is Nothing Or DeviceUniqueId = "" or  IsNormalSort is Nothing or IsNormalSort = "" Then
        '    Return "-1"
        'End If

        Dim _DB As New ClassConnectSql
        Dim ClsKNSession As New ClsKNSession()
        'Dim AnsState As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "CurrentAnsState")
        'If AnsState Is Nothing Or AnsState = "" Then
        '    Return "-1"
        'End If

        Dim QuizId As String = "2AD4DE6E-FF20-408F-8B87-EC83DCCF4BF9" 'Test
        'Dim QuizId As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "QuizId")
        'If QuizId Is Nothing Or QuizId = "" Then
        '    Return "-1"
        'End If
        Dim StudentId As String = "C8BB6C2B-B368-4D7B-B7DD-3FA62B3DE869" 'Test
        'Dim StudentId As String = ClsKNSession.GetValueFromClsSess(DeviceUniqueId, "CurrentPlayerId")
        'If StudentId Is Nothing Or StudentId = "" Then
        '    Return "-1"
        'End If
        Dim sql As String = ""

        If IsNormalSort = "True" Then 'ถ้าเรียงแบบปกติใช้คิวรี่นี้
            sql = " SELECT Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
                            " AND Student_Id = '" & StudentId & "' ORDER BY  QQ_No "
        Else 'ถ้าเรียงจากข้อที่ยังไม่ได้ทำขึ้นก่อนใช้คิวรี่นี้
            sql = " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored  FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
                  " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NULL ORDER BY Answer_Id,QQ_No) as a " & _
                  " UNION all " & _
                  " SELECT * FROM (SELECT TOP 1000 Answer_Id,QQ_No,IsScored FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
                  " AND Student_Id = '" & StudentId & "' AND Answer_Id IS NOT NULL ORDER BY QQ_No) as b "
        End If

        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim arrImage As Array = {"veryhappy.png", "happy.png", "neutral.png", "sad.png", "verysad.png"}

        Dim CheckQuantity As Integer = 1
        Dim sb As New StringBuilder
        If dt.Rows.Count > 0 Then
            Dim EachQQNo As String = ""
            Dim EachIsScore As String = ""

            Dim rn As New Random

            For i = 0 To dt.Rows.Count - 1


                Dim rnum = rn.Next(1, 5)

                ' StudentId = dt.Rows(i)("Student_Id").ToString() 'รับ QuestionId เพื่อเอามาเป็น Id ให้แต่ละปุ่ม
                EachQQNo = dt.Rows(i)("QQ_No").ToString() 'รับ QQ_No เพื่อเอามาเป็น Text ให้ปุ่ม
                EachIsScore = dt.Rows(i)("IsScored").ToString() 'รับ IsScore เพื่อที่จะได้รู้ว่าข้อนั้นตรวจหรือยัง
                If CheckQuantity <= 10 Then 'เช็คเพื่อให้สร้างปุ่มได้แค่หน้าละ 10 ปุ่มเท่านั้น
                    If CheckQuantity = 1 Then 'ถ้าเป็นรอบแรกของการสร้างแต่ละรอบต้องสร้าง Div ขึ้นมาเพื่อครอบก่อนสร้างปุ่ม
                        sb.Append("<div style='width:500px;margin-top:95px;margin-left:83px;' class='slide'>")
                    End If
                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then 'ถ้าข้อนี้ตอบแล้วเข้า If
                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""margin-left:90px;background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        Else
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        End If
                        CheckQuantity += 1
                    Else 'ถ้าข้อนี้ยังไม่ได้ตอบเข้า Else
                        If CheckQuantity = 1 Or CheckQuantity = 6 Then 'เช็คว่าถ้าเป็นรอบแรกต้องสร้างปุ่มที่มี Margin-Left เยอะ
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""margin-left:90px;background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        Else
                            sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                        End If
                        CheckQuantity += 1
                    End If
                Else 'ถ้าเกิน 10 ปุ่มแล้วต้องขึ้นหน้าใหม่
                    sb.Append("</div>") 'ปิด Tag Div ก่อนที่จะสร้าง Div ที่ครอบปุ่มอันต่อไป
                    sb.Append("<div style='width:500px;margin-top:95px;margin-left:83px;' class='slide'>")
                    If dt.Rows(i)("Answer_Id") IsNot DBNull.Value Then
                        sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""margin-left:90px;background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                    Else
                        sb.Append("<input type='button' onclick='LeapChoiceOnclick(""" & StudentId & """);' style=""margin-left:90px;background-image:url('../Images/Activity/mostWrongFace/" & arrImage(rnum) & "');"" id='" & EachIsScore & "' class='ForBtn' value='ข้อที่" & EachQQNo & "'  />")
                    End If
                    CheckQuantity = 2
                End If
            Next
            sb.Append("</div>") 'ปิด Tag Div 
        Else
            Return "-1"
        End If

        Return sb.ToString()

    End Function
End Class