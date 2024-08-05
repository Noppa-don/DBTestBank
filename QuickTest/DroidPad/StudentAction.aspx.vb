Imports System.Web.Script.Serialization
Imports KnowledgeUtils
Imports System.Web

Public Class StudentAction
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            Dim DefaultValue As String = "{""Param"": {""Lock"" : ""0"",""Mute"" : ""0"",""Visible"" : ""1"",""NextURL"" : ""/Activity/EmptySession.aspx"",""MidText"" : """",""BottomText"" : """"}}"
            If Not IsNothing(methodName) Then
                Dim DeviceUniqueID As String = Request.Form("DeviceUniqueID")
                Dim IsFirstTimeOpen As String = Request.Form("FirstTime")
                If methodName.ToLower() = "nextaction" Then
                    If DeviceUniqueID <> "" Then
                        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                        'Dim ReturnValue As String = "{""Param"": {""ActionInfoCommand"": {""Lock"" : ""0"",""Mute"" : ""0"",""Visible"" : ""1""},""NextURL"" : {""URL"" : """",""MidText"" : "" "",""BottomText"" : "" ""}}}"
                        Dim ReturnValue As String
                        ' เอามาเช็คชั่วคราวก่อน เพราะห้องแลป ยังไม่มี
                        Dim redis As New RedisStore()
                        If redis.Getkey(DeviceUniqueID & "_LabTeacher") = "True" Then
                            ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, IsFirstTimeOpen, True)
                        Else
                            ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, IsFirstTimeOpen, False)
                        End If

                        If ReturnValue = "-1" Then
                            Response.Write(DefaultValue)
                        Else
                            Response.Write(ReturnValue)
                        End If
                        Response.End()
                    End If
                End If
            End If
            Response.Write(DefaultValue)
            Response.End()
        End If



    End Sub

    '<Services.WebMethod()>
    'Public Shared Function NextAction(ByVal DeviceUniqueID As String)
    '    '    Dim UseCls As New ClassConnectSql
    '    '    Dim ClsActivity As New ClsActivity(New ClassConnectSql)
    '    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    '    Dim strNextURL As String = ""
    '    '    Dim Lock, Mute, Visible As Integer
    '    '    Lock = 0
    '    '    Mute = 0
    '    '    Visible = 1
    '    '    Dim URL As String = ""
    '    '    Dim MidText As String = ""
    '    '    Dim BottomText As String = ""


    '    '    If HttpContext.Current.Session("_QuizId") Is Nothing Then

    '    '        Dim dt As New DataTable
    '    '        dt = ClsDroidPad.GetQuizIdFromDeviceUniqueID(DeviceUniqueID)

    '    '        If dt.Rows.Count > 0 Then
    '    '            Dim QuizIds As String = dt.Rows(0)("Quiz_Id")
    '    '            HttpContext.Current.Session("_QuizId") = QuizIds
    '    '        End If


    '    '    End If


    '    '    If HttpContext.Current.Session("_QuizId") IsNot Nothing Then
    '    '        If DeviceUniqueID IsNot Nothing Then

    '    '            Dim GetTestSetId As String = ClsDroidPad.GetTestSetIdFromQuizId(HttpContext.Current.Session("_QuizId"))

    '    '            If GetTestSetId <> "" Then
    '    '                Dim dtCheckAmountQuestion As New DataTable
    '    '                dtCheckAmountQuestion = ClsActivity.GetTestset(GetTestSetId)
    '    '                If dtCheckAmountQuestion.Rows.Count > 0 Then
    '    '                    BottomText = HttpContext.Current.Session("_QQNo").ToString() & "/" & dtCheckAmountQuestion.Rows(0)("QuestionAmount").ToString()
    '    '                End If
    '    '            End If

    '    '            Dim dtLastChoice As New DataTable
    '    '            dtLastChoice = ClsDroidPad.GetLastChoiceQuestion(HttpContext.Current.Session("_QuizId"))

    '    '            If dtLastChoice.Rows.Count > 0 Then
    '    '                Dim LastChoice As Integer = dtLastChoice.Rows(0)("QQ_No")
    '    '                If HttpContext.Current.Session("_QQNo") <> LastChoice Then
    '    '                    URL = "/Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" & DeviceUniqueID
    '    '                End If
    '    '            End If



    '    '        End If

    '    '    End If


    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""ActionInfoCommand"": {""Lock"" : ""0"",""Mute"" : ""0"",""Visible"" : ""1""},""NextURL"" : {""URL"" : """",""MidText"" : """",""BottomText"" : """"}}}"
    '    ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, False)
    '    Return ReturnValue

    'End Function

    '<Services.WebMethod()>
    'Public Shared Function CheckActivityChangePage(ByVal DeviceUniqueID As String)
    '    Dim UseCls As New ClassConnectSql
    '    Dim strNextURL As String = ""


    '    If HttpContext.Current.Session("_QuizId") Is Nothing Then
    '        Dim sql1 As String = "SELECT     TOP 1 tblQuizSession.Quiz_Id " & _
    '                             " FROM t360_tblTablet INNER JOIN " & _
    '                             " t360_tblTabletOwner ON t360_tblTablet.School_Code = t360_tblTabletOwner.School_Code AND t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id INNER JOIN " & _
    '                             " tblQuizSession ON t360_tblTablet.School_Code = tblQuizSession.School_Code AND t360_tblTabletOwner.Tablet_Id = tblQuizSession.Tablet_Id " & _
    '                             " WHERE     (t360_tblTabletOwner.Owner_Type = 2) AND (t360_tblTablet.Tablet_SerialNumber = '" & DeviceUniqueID & "') " & _
    '                             " ORDER BY tblQuizSession.LastUpdate DESC "
    '        Dim dt1 As String
    '        dt1 = UseCls.ExecuteScalar(sql1)

    '        If dt1 IsNot Nothing Then
    '            Dim Quizid As String = dt1
    '            HttpContext.Current.Session("_QuizId") = Quizid
    '        End If

    '    End If


    '    If HttpContext.Current.Session("_QuizId") IsNot Nothing Then
    '        If DeviceUniqueID IsNot Nothing Then
    '            Dim sqlCheckChangePage As String = "SELECT TOP 1 QQ_No FROM dbo.tblQuizQuestion WHERE Quiz_Id = '" & HttpContext.Current.Session("_QuizId") & "' ORDER BY lastupdate DESC"
    '            Dim dtCheck As New DataTable
    '            dtCheck = UseCls.getdata(sqlCheckChangePage)

    '            If dtCheck.Rows.Count > 0 Then
    '                If HttpContext.Current.Session("_QQNo") IsNot Nothing Then
    '                    If HttpContext.Current.Session("_QQNo") <> dtCheck.Rows(0)("QQ_No") Then
    '                        strNextURL = "ActivityPage_Pad.aspx?DeviceUniqueID=" & DeviceUniqueID
    '                    End If

    '                End If

    '            End If

    '        End If

    '    End If

    '    'Dim JsonString As New ArrayList

    '    'JsonString.Add(New With {.CommandLock = "0"})
    '    'JsonString.Add(New With {.CommandMute = "0"})
    '    'JsonString.Add(New With {.CommandVisible = "0"})
    '    'JsonString.Add(New With {.NextURL = strNextURL})
    '    'JsonString.Add(New With {.MidText = ""})
    '    'JsonString.Add(New With {.BottomText = ""})


    '    '' แปลงค่าเป็น JSON
    '    'Dim js As New JavaScriptSerializer()
    '    'Dim RegInfo As String = js.Serialize(JsonString)
    '    'js = Nothing


    '    Return "{""CommandLock"":""0"",""CommandMute"":""0"",""CommandVisible"":""0"",""NextURL"":""" & strNextURL & """,""MidText"":"""",""BottomText"":""""}"
    'End Function

    <Services.WebMethod()>
    Public Shared Function SendToGetNextAction(ByVal DeviceUniqueID As String, ByVal IsFirstTime As String, ByVal IsTeacher As String)

        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim URLReturn As String = ""
        If DeviceUniqueID Is Nothing Or DeviceUniqueID = "" Then
            Return URLReturn
        End If
        URLReturn = ClsDroidPad.GetNextAction(DeviceUniqueID, IsFirstTime, IsTeacher)
        ClsDroidPad = Nothing
        Return URLReturn


    End Function

    <Services.WebMethod()>
    Public Shared Function ClosePracticeQuiz(ByVal DeviceId As String)

        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        ClsDroidPad.SetClosePracticeQuiz(DeviceId)
        Return "Complete"

    End Function

    <Services.WebMethod()>
    Public Shared Function CheckUnActiveQuiz(ByVal DeviceUniqueID As String)

        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim IsActiveQuiz As String = ClsDroidPad.GetActiveQuiz(DeviceUniqueID)

        Dim JsonString
        Dim js As New JavaScriptSerializer()
        JsonString = New With {.IsActiveQuiz = IsActiveQuiz}
        Return js.Serialize(JsonString)

    End Function


End Class