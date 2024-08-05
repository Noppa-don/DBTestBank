Public Class RaceActivity
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Shared Index As Integer = 0
    Dim QuestionId() As String = {"AB9817D6-F9A5-4B05-8086-0219DEB224DD", "89E4CC01-6360-46C4-A313-0247477B635D", _
                                 "9350EA52-7932-428A-B971-0301753B6751", "3780A1AB-E72B-4E33-9A81-070DFF0E620F", _
                                  "D5FA0D49-5357-4D78-A83A-0797C835CFB3", "69F975BD-3FFB-4AF1-A3C2-0D2DF870DA48", _
                                  "0A51C6BD-CCC0-4EBC-AC04-0E4A9BB92EB8", "575EF9EE-A483-40B3-9D58-0FDAA9BC02FB", _
                                  "5F82FEF7-9838-4D80-B999-129FC0AC1264", "AA97A64E-7AAA-4D5E-962F-1BEDCFDA63C6"}
    Dim cls As New ClassConnectSql

    Public Property _ExamNum As String
        Get
            Return ViewState("_ExamNum")
        End Get
        Set(ByVal value As String)
            ViewState("_ExamNum") = value
        End Set
    End Property
    Public Property _NextState As String
        Get
            Return ViewState("_NextState")
        End Get
        Set(ByVal value As String)
            ViewState("_NextState") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("CheckShowAnswer") = True
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else

            If Not IsPostBack Then
                Dim taganswer As String

                taganswer = GetAnswerDetails(QuestionId(0), False)

                Dim tagQuestion As String = GetQuestionDetails((QuestionId(0)))
                Question_No.InnerText = "1"
                QuestionName.InnerText = tagQuestion
                Answer.InnerHtml = taganswer
                If Session("CheckShowAnswer") = True Then

                    _NextState = "None"
                End If
                'Dim Examnum As Integer = 1
                '_ExamNum = Examnum.ToString

            End If
        End If
    End Sub

    Public Function GetQuestionNo() As String
        Index += 1
        Return "<td valign=""top"" class=""questionno"" >" & Index & ".</td>"
    End Function
    Public Function GetQuestionDetails(ByVal Question_ID As String) As String

        Dim sql As String
        sql = "select Question_name from tblQuestion where question_id = '" & Question_ID & "';"
        Dim dt As DataTable
        Dim db As New ClassConnectSql()
        dt = db.getdata(sql)

        Return dt.Rows(0)("Question_Name").ToString

    End Function

    Public Function GetAnswerDetails(ByVal Question_ID As String, ByVal ShowAnswer As Boolean) As String

        Dim sql As String
        sql = "select answer_name from tblanswer where question_id = '" & Question_ID & "' order by answer_no;"
        Dim dt As DataTable
        Dim db As New ClassConnectSql()
        dt = db.getdata(sql)
        Dim tagAnswer As String
        If ShowAnswer = True Then
            sql = "select answer_name from tblanswer where question_id = '" & Question_ID & "' and answer_score = '1' order by answer_no;"
            Dim dt2 As DataTable
            dt2 = db.getdata(sql)

            tagAnswer = "<tr><td>ก.</td> <td>" & dt.Rows(0)("Answer_Name") & "</td> "
            If dt.Rows(0)("Answer_Name") = dt2.Rows(0)("Answer_Name") Then
                tagAnswer = "<tr><td id='rightAns' style=""background-color:green;"">ก.</td> <td style=""background-color:green;"">" & dt.Rows(0)("Answer_Name") & "</td> "
            Else
                tagAnswer = "<tr><td>ก.</td> <td>" & dt.Rows(0)("Answer_Name") & "</td> "
            End If

            If dt.Rows(1)("Answer_Name") = dt2.Rows(0)("Answer_Name") Then
                tagAnswer &= "<td ></td > <td id='rightAns' style=""background-color:green;"">ข.</td><td  style=""background-color:green;"">" & dt.Rows(1)("Answer_Name") & "</td></tr> "
            Else
                tagAnswer &= "<td ></td> <td>ข.</td><td>" & dt.Rows(1)("Answer_Name") & "</td></tr> "
            End If
            If dt.Rows(2)("Answer_Name") = dt2.Rows(0)("Answer_Name") Then
                tagAnswer &= "<tr><td id='rightAns' style=""background-color:green;"">ค.</td> <td style=""background-color:green;"">" & dt.Rows(2)("Answer_Name") & "</td> "
            Else
                tagAnswer &= "<tr><td>ค.</td> <td>" & dt.Rows(2)("Answer_Name") & "</td> "
            End If
            If dt.Rows(3)("Answer_Name") = dt2.Rows(0)("Answer_Name") Then
                tagAnswer &= "<td></td> <td id='rightAns' style=""background-color:green;"">ง.</td><td style=""background-color:green;"">" & dt.Rows(3)("Answer_Name") & "</td></tr>"
            Else
                tagAnswer &= "<td></td> <td>ง.</td><td>" & dt.Rows(3)("Answer_Name") & "</td></tr>"
            End If

        Else
            tagAnswer = "<tr><td >ก.</td> <td id='rightAns'>" & dt.Rows(0)("Answer_Name") & "</td> "
            tagAnswer &= "<td ></td> <td>ข.</td><td>" & dt.Rows(1)("Answer_Name") & "</td></tr> "
            tagAnswer &= "<tr><td>ค.</td> <td>" & dt.Rows(2)("Answer_Name") & "</td> "
            tagAnswer &= "<td></td> <td>ง.</td><td>" & dt.Rows(3)("Answer_Name") & "</td></tr>"
        End If
        Return tagAnswer.ToString
    End Function

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNext.Click, btnNextTop.Click, btnNextSide.Click

        Dim ShowAnswer As Boolean = True
        Dim Examnum As Integer
        If _NextState = "None" Then
            ShowAnswer = True
            Examnum = CInt(_ExamNum)
            _ExamNum = Examnum.ToString
            If Examnum < 10 Then
                Dim QId As String = QuestionId(Examnum)
                Dim taganswer As String = GetAnswerDetails(QId, ShowAnswer)
                Dim tagQuestion As String = GetQuestionDetails(QId)
                Question_No.InnerText = Examnum + 1
                QuestionName.InnerText = tagQuestion
                Answer.InnerHtml = taganswer
                _NextState = "one"
            End If

        Else
            ShowAnswer = False
            Examnum = CInt(_ExamNum) + 1
            _ExamNum = Examnum.ToString
            If Examnum < 10 Then
                Dim QId As String = QuestionId(Examnum)
                Dim taganswer As String = GetAnswerDetails(QId, ShowAnswer)
                Dim tagQuestion As String = GetQuestionDetails(QId)
                Question_No.InnerText = Examnum + 1
                QuestionName.InnerText = tagQuestion
                Answer.InnerHtml = taganswer
                If _NextState = "one" Then
                    _NextState = "None"
                End If
            End If
        End If


        'Examnum = Examnum + 1
        '


    End Sub

    Protected Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrevious.Click, btnPrvTop.Click, btnPrvSide.Click
        Dim ShowAnswer As Boolean = True

        If _NextState = "None" Or _NextState = "one" Then
            ShowAnswer = True
        Else
            ShowAnswer = False
        End If

        Dim Examnum As Integer = CInt(_ExamNum) - 1
        _ExamNum = Examnum.ToString
        If Not Examnum < 0 Then
            Dim QId As String = QuestionId(Examnum)
            Dim taganswer As String = GetAnswerDetails(QId, ShowAnswer)
            Dim tagQuestion As String = GetQuestionDetails(QId)
            Question_No.InnerText = Examnum + 1
            QuestionName.InnerText = tagQuestion
            Answer.InnerHtml = taganswer

            'Examnum = Examnum - 1
            '
        End If
    End Sub
End Class