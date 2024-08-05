Imports System.IO

Public Class ExamTemplate
    Inherits System.Web.UI.Page
    Shared Index As Integer = 0
    Dim cls As New ClsPDF(New ClassConnectSql)
    Dim Size() As String
    Dim newTestSetId As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not IsPostBack Then

                '#If DEBUG Then
                '            Session("newTestSetId") = "C3FF56C5-9927-4D2E-A762-5BFB94838038"
                '            Session("IsPreviewPage") = False
                '            Session("IsAnswerSheet") = True
                '#End If

                newTestSetId = Session("newTestSetId").ToString()



                Dim dt As DataTable = cls.GetHeader(newTestSetId)

                litSchoolName.Text = "<B>" & dt.Rows(0)("SchoolName") & " </b>"
                litExamDetail.Text = "<B>" & dt.Rows(0)("TestSet_Name") & " </b>"
                litTotalTime.Text = "<B>เวลา  " & dt.Rows(0)("TestSet_Time") & "  นาที </b>"
                litExamAmount.Text = "<B>จำนวน  " & cls.GetExamAmount(newTestSetId) & "  ข้อ </b>"

                Dim dtQuestionSet As DataTable
                dtQuestionSet = cls.GetTSQS(newTestSetId, Session("IsPreviewPage"))

                Dim count As Integer = 1

                For Each a In dtQuestionSet.Rows

                    Select Case a("QSet_Type")
                        Case 1
                            Dim C = LoadControl("~/UserControl/ChoiceControl.ascx")
                            Dim Uc As ChoiceControl = DirectCast(C, ChoiceControl)
                            Uc.QSetId = a("QSet_Id").ToString
                            Uc.TestSetId = newTestSetId
                            If count = 1 Then
                                Uc.IsFirstControl = True
                            End If
                            Controls.Add(Uc)
                        Case 2
                            Dim T = LoadControl("~/UserControl/TrueFalseControl.ascx")
                            Dim UT As TrueFalseControl = DirectCast(T, TrueFalseControl)
                            UT.QSetId = a("QSet_Id").ToString
                            UT.TestSetId = newTestSetId
                            If count = 1 Then
                                UT.IsFirstControl = True
                            End If
                            Controls.Add(UT)

                        Case 6
                            Dim M = LoadControl("~/UserControl/MatchControl.ascx")
                            Dim UM As MatchControl = DirectCast(M, MatchControl)
                            UM.QSetId = a("QSet_Id").ToString
                            UM.TestSetId = newTestSetId
                            If count = 1 Then
                                UM.IsFirstControl = True
                            End If
                            Controls.Add(UM)

                        Case 3
                            Dim M = LoadControl("~/UserControl/MatchControl.ascx")
                            Dim UM As MatchControl = DirectCast(M, MatchControl)
                            UM.QSetId = a("QSet_Id").ToString
                            UM.TestSetId = newTestSetId
                            If count = 1 Then
                                UM.IsFirstControl = True
                            End If
                            Controls.Add(UM)


                    End Select
                    count = count + 1
                Next


            End If
        End If
    End Sub

    Function GetFont(ByVal s As String) As String
        Dim FontSize As String = cls.GetFontSize(newTestSetId)
        Size = Split(FontSize, ",")
        Return Size(s) & "pt"
    End Function

End Class