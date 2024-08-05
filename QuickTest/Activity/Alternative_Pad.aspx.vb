Public Class Alternative_Pad
    Inherits System.Web.UI.Page
    Public GroupName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Session("selectedSession") Is Nothing Then
        '    Dim PkSession As String = Request.QueryString("PkSession")
        '    GroupName = PkSession
        'Else
        '    GroupName = Session("selectedSession").ToString()
        'End If

        Session("SchoolId") = "1000001"
        Session("UserId") = "3BEE2B4F-A667-4419-B359-4D7D35BFC238" 'player_Id สำหรับเทส
        Dim UserId As String = Session("UserId")

        If (Session("selectedSession") Is Nothing) Then
            AddNewSession(UserId) 'เอามาใช้เทสกับ browser แทน tablet
        End If

        GroupName = Session("selectedSession").ToString()

        If Not (Session("Quiz_Id") Is Nothing) Or Not (Session("Quiz_Id") = "") Then
            Dim objDroidPad As New ClassDroidPad(New ClassConnectSql)
            objDroidPad.CloseQuiz(Session("Quiz_Id").ToString())
        End If

    End Sub

    Private Sub AddNewSession(ByVal UserId As String)

        Dim ArrData As New ArrayList
        Dim objClsSessInFo As New ClsSessionInFo
        'Dim pkGuid = System.Guid.NewGuid()
        Dim clsSelectSess As New ClsSelectSession()
        Dim pkGuid = clsSelectSess.Number4Digit()

        HttpContext.Current.Session("selectedSession") = pkGuid
        objClsSessInFo.PKInfo = pkGuid
        objClsSessInFo.Index = 1
        objClsSessInFo.TimeStamp = DateTime.Now
        Dim res As String = ResolveUrl("~").ToLower()
        Dim currentPage As String = res & "testset/step1.aspx"
        objClsSessInFo.CurrentPage = currentPage
        objClsSessInFo.SchoolId = Session("SchoolId")
        ArrData.Add(objClsSessInFo)
        HttpContext.Current.Application(UserId) = ArrData

    End Sub
    
End Class