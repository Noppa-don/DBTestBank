Public Class ShowHowToPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HowTo.InnerHtml = "<iframe src=""../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm"" width='100%' height='500'></iframe>"
    End Sub

    '<Services.WebMethod()>
    'Public Shared Function getIframeTutorial(ByVal tutorial As String) As String
    '    If (tutorial = "introduction") Then
    '        'Return getIframeTutorial = "<iframe src=""../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm"" width='100%' height='500'></iframe>"
    '        getIframeTutorial = tutorial
    '    ElseIf (tutorial = "manageQuiz") Then
    '        'Return getIframeTutorial = "<iframe src=""../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm"" width='100%' height='500'></iframe>"
    '        getIframeTutorial = tutorial
    '    ElseIf (tutorial = "modifyQuiz") Then
    '        'Return getIframeTutorial = "<iframe src=""../HowTo/HowToSelectClassSubject/HowToSelectClassSubject.htm"" width='100%' height='500'></iframe>"
    '        getIframeTutorial = tutorial
    '    End If
    '    Return getIframeTutorial
    'End Function

End Class