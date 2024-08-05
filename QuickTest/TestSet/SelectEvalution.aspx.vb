Public Class SelectEvalution
    Inherits System.Web.UI.Page
    Dim clsEI As New clsEvalutionIndex
    Protected LevelId As String
    Protected GroupSubjectId As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GroupSubjectId = Request.QueryString("subjectid")
        LevelId = Request.QueryString("levelid")

        Dim CurrentTestsetId = Session("newTestSetId").ToString()

        Dim pnLevelOne As New Panel
        Dim pnLevelTwo As New Panel
        Dim pnLevelThree As New Panel

        Dim dtLevelOne As DataTable = clsEI.GetEvalutionIndexLevelOne(GroupSubjectId, LevelId)

        If dtLevelOne.Rows.Count() <> 0 Then
            For Each eachLevelOne In dtLevelOne.Rows
                'ระดับ 1
                With pnLevelOne
                    .Style.Add("padding-top", "20px")
                    .Style.Add("padding-left", "5px")
                    .Style.Add("padding-right", "5px")
                    .Style.Add("background-color", "#D3F2F7")
                End With

                Dim EILevelOne As New Label
                EILevelOne.Style.Add("padding-left", "10px")
                EILevelOne.Style.Add("margin-top", "20px")
                EILevelOne.Style.Add("font-weight", "bold")
                EILevelOne.Style.Add("font-size", "25px")
                EILevelOne.ID = eachLevelOne("EI_Id").ToString
                EILevelOne.Text = eachLevelOne("EIName") & "<br><br>"
                pnLevelOne.Controls.Add(EILevelOne)

                Dim dtLevelTwo As DataTable = clsEI.GetEvalutionIndexLevelTwo(GroupSubjectId, LevelId, eachLevelOne("EI_Id").ToString)

                If dtLevelTwo.Rows.Count <> 0 Then

                    For Each eachLevelTwo In dtLevelTwo.Rows

                        Dim EILevelTwo As New Label
                        EILevelTwo.Style.Add("padding-left", "30px")
                        EILevelTwo.Style.Add("font-weight", "bold")
                        EILevelTwo.Style.Add("font-size", "20px")
                        EILevelTwo.Style.Add("line-height", "35px")
                        EILevelTwo.ID = eachLevelTwo("EI_Id").ToString
                        EILevelTwo.Text = eachLevelTwo("EIName") & "<br><br>"

                        pnLevelOne.Controls.Add(EILevelTwo)

                        Dim dtLevelThree As DataTable = clsEI.GetEvalutionIndexLevelThree(GroupSubjectId, LevelId, eachLevelTwo("EI_Id").ToString, CurrentTestsetId)

                        If dtLevelThree.Rows.Count <> 0 Then

                            For Each eachLevelThree In dtLevelThree.Rows

                                Dim EILevelThree As New CheckBox

                                EILevelThree.Style.Add("padding-left", "10px")
                                EILevelThree.ID = eachLevelThree("EI_Id").ToString
                                EILevelThree.Attributes.Add("name", "test[]")
                                EILevelThree.Attributes.Add("Onclick", "SaveTestsetEvalutionIndex('" & CurrentTestsetId & "','" & eachLevelThree("EI_Id").ToString & "')")
                                EILevelThree.Text = eachLevelThree("EIName") & "<br><br>"
                                EILevelThree.Style.Add("font-size", "18px")
                                If eachLevelThree("IsSelected") = "true" Then
                                    EILevelThree.Checked = True
                                End If
                                pnLevelOne.Controls.Add(EILevelThree)
                            Next

                            Dim UnderLineTag As New Label
                            UnderLineTag.Text = "<br>"

                            pnLevelOne.Controls.Add(UnderLineTag)
                        End If

                    Next
                End If
            Next
            'Div นอกสุด
            divSelectEvalutionIndex.Controls.Add(pnLevelOne)
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function SaveTestsetEvalutionIndex(TestsetId As String, EIId As String, IsCheck As String) As String
        Dim ClEi As New clsEvalutionIndex
        If IsCheck = "true" Then
            ClEi.AddTestsetEvalutionIndex(TestsetId, EIId)
        Else
            ClEi.DeleteTestsetEvalutionIndex(TestsetId, EIId)
        End If

        Return "True"
    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteTestsetEvalutionIndex(TestsetId As String, GroupSubjectid As String, LevelId As String) As String
        Dim ClEi As New clsEvalutionIndex
        ClEi.DeleteAllTestsetEvalutionIndex(GroupSubjectid, LevelId, HttpContext.Current.Session("newTestSetId").ToString())
        Return "True"
    End Function

End Class