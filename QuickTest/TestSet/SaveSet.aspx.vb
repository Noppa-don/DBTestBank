Public Class SaveSet
    Inherits System.Web.UI.Page
    Dim objTestSet As ClsTestSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else

            If IsNothing(Session("objTestSet")) Then
                objTestSet = New ClsTestSet(Session("userid"))
                Session("objTestSet") = objTestSet
            Else
                objTestSet = DirectCast(Session("objTestSet"), ClsTestSet)
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim setName As String = txtName.Text
        Dim setTime As String = txtTime.Text
        If Not IsNumeric(setTime) Then setTime = "60"
        Dim setTextSize As String = ddlTextSize.SelectedValue.ToString()
        Dim levelId As String = ""
        If setTextSize = "2" Then 'ตัวใหญ่มาก
            levelId = "5F4765DB-0917-470B-8E43-6D1C7B030818" 'ป.1
        ElseIf setTextSize = "1" Then 'ตัวขนาดใหญ่กลางๆ
            levelId = "93B163B6-4F87-476D-8571-4029A6F34C84" 'ป.6
        Else 'ตัวปกติ
            levelId = "6BF52DC7-314C-40ED-B7F3-BCC87F724880" 'ม.6
        End If

        Dim IsPracticemode As String
        If IsPractice.Checked Then
            IsPracticemode = "1"
        Else
            IsPracticemode = "0"
        End If

        objTestSet.SaveTestSet(setName, setTime, levelId, Session("newTestSetId"), setTextSize, IsPracticemode)
        Session("DetailFontSize") = "15"
        Session("HeaderFontSize") = "25"

        Response.Redirect("examtemplate.aspx", False)
    End Sub
End Class