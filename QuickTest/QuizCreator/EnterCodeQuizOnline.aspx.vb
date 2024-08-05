Imports System.Web
Public Class EnterCodeQuizOnline
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <Services.WebMethod()>
    Public Shared Function CheckCodeIsCorrect(ByVal QuizCode As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuizCreatorTestset WHERE QCT_Code = '" & _DB.CleanString(QuizCode.Trim()) & "' AND QCT_IsActive = 1 "
        Dim CheckPass As Integer = CInt(_DB.ExecuteScalar(sql))
        If CheckPass > 0 Then
            _DB = Nothing
            Return "True"
        Else
            _DB = Nothing
            Return "False"
        End If

    End Function

    <Services.WebMethod()>
    Public Shared Function CheckThisCodeHavePassword(ByVal QuizCode As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT QCT_Password FROM dbo.tblQuizCreatorTestset WHERE QCT_Code = '" & _DB.CleanString(QuizCode.Trim()) & "' "
        Dim CheckPass As String = _DB.ExecuteScalar(sql)
        If CheckPass <> "" Then
            _DB = Nothing
            Return "True"
        Else
            _DB = Nothing
            Return "False"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function CheckThisCodeIsOnline(ByVal QuizCode As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuizCreatorTestset WHERE QCT_Code = '" & _DB.CleanString(QuizCode.Trim()) & "' AND QCT_IsOnline = 1 "
        Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql))
        If CountCheck > 0 Then
            Return "True"
        Else
            Return "False"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function CheckPasswordIsCorrect(ByVal QuizCode As String, ByVal QuizPassword As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuizCreatorTestset WHERE QCT_Code = '" & _DB.CleanString(QuizCode.Trim()) & "' AND QCT_Password = '" & _DB.CleanString(QuizPassword.Trim()) & "' AND QCT_IsActive = 1 "
        Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql))
        If CountCheck > 0 Then
            Return "True"
        Else
            Return "False"
        End If
    End Function



End Class