
Public Class SendUsage

    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ทำการเก็บ Log ของโปรแกรม QApp ตามสถานการณ์ต่างๆ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            'ตัวแปรที่เอาไว้เช็คค่าว่าจะต้องส่งไปเก็บ Log ในสถานการณ์ไหน
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                'รหัสเครื่อง Tablet
                Dim DeviceId As String = Request.Form("DeviceId")
                If Not IsNothing(DeviceId) And DeviceId.Trim().Length >= 10 Then
                    'เปิด App
                    If methodName.ToLower() = "runapp" Then
                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(DeviceId) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If DeviceId.Trim() <> "" And AppId.Trim() <> "" And AppKey.Trim() <> "" And AppKey.Trim().Length = 32 Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.ProcessRunApp(DeviceId, AppId, AppKey)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    ElseIf methodName.ToLower() = "enterstation" Then
                        'เข้าด่านต่างๆ
                        Dim StationId As String = Request.Form("StationId")
                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(DeviceId) And Not IsNothing(StationId) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If DeviceId.Trim() <> "" And StationId.Trim() <> "" And AppId.Trim() <> "" And AppKey.Trim() <> "" And AppKey.Trim().Length = 32 Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.ProcessEnterStation(DeviceId, AppId, AppKey, StationId)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    ElseIf methodName.ToLower() = "action" Then
                        'เมื่อกดตอบข้อต่างๆ
                        Dim StationId As String = Request.Form("StationId")
                        Dim QuestionName As String = Request.Form("QuestionName")
                        Dim AnswerName As String = Request.Form("AnswerName")
                        'Shin - 20140710 - ปรับให้รับค่าว่างได้, เพื่อรองรับข้อสอบที่กดตอบไป แต่ไม่มีคะแนน (เป็นพวกข้อสอบบันทึกพฤติกรรม, กิจกรรมไม่เก็บคะแนน)
                        'Dim Score As Double = Request.Form("Score")
                        Dim Score As Double = 0.0
                        Dim ScoreText As String = Request.Form("Score")
                        If ScoreText.Trim = "" Then
                            Score = -9999999.9999 'ถ้าส่งค่าว่าง EmptyString มา, ให้ส่งต่อ score เป็นเลขพิเศษ เพื่อไป if ต่อใน function ข้างในเพื่อ insert ลงไปเป็น NULL 
                        Else
                            If Not IsNumeric(ScoreText.Trim) Then
                                Response.Write(-1)
                                Response.End()
                            End If
                        End If
                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(DeviceId) And Not IsNothing(StationId) And Not IsNothing(QuestionName) And Not IsNothing(AnswerName) And Not IsNothing(Score) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If DeviceId.Trim() <> "" And StationId.Trim() <> "" And QuestionName.Trim() <> "" And AnswerName.Trim() <> "" And AppId.Trim() <> "" And AppKey.Trim() <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.ProcessAction(DeviceId, AppId, AppKey, StationId, QuestionName, AnswerName, Score)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    ElseIf methodName.ToLower() = "edit" Then
                        'เมื่อกดแก้ไขคำตอบ
                        Dim StationId As String = Request.Form("StationId")
                        Dim QuestionName As String = Request.Form("QuestionName")
                        Dim AnswerName As String = Request.Form("AnswerName")

                        'Shin - 20140710 - ปรับให้รับค่าว่างได้, เพื่อรองรับข้อสอบที่กดตอบไป แต่ไม่มีคะแนน (เป็นพวกข้อสอบบันทึกพฤติกรรม, กิจกรรมไม่เก็บคะแนน)
                        'Dim Score As Double = Request.Form("Score")
                        Dim Score As Double = 0.0
                        Dim ScoreText As String = Request.Form("Score")
                        If ScoreText.Trim = "" Then
                            Score = -9999999.9999 'ถ้าส่งค่าว่าง EmptyString มา, ให้ส่งต่อ score เป็นเลขพิเศษ เพื่อไป if ต่อใน function ข้างในเพื่อ insert ลงไปเป็น NULL 
                        Else
                            If Not IsNumeric(ScoreText.Trim) Then
                                Response.Write(-1)
                                Response.End()
                            End If
                        End If

                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(DeviceId) And Not IsNothing(StationId) And Not IsNothing(QuestionName) And Not IsNothing(AnswerName) And Not IsNothing(Score) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If DeviceId.Trim() <> "" And StationId.Trim() <> "" And QuestionName.Trim() <> "" And AnswerName.Trim() <> "" And AppId.Trim() <> "" And AppKey.Trim() <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.ProcessEdit(DeviceId, AppId, AppKey, StationId, QuestionName, AnswerName, Score)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    ElseIf methodName.ToLower() = "closeapp" Then
                        'เมื่อกดปิด App
                        Dim StationId As String = Request.Form("StationId")
                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(DeviceId) And Not IsNothing(StationId) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If DeviceId.Trim() <> "" And StationId.Trim() <> "" And AppId.Trim() <> "" And AppKey.Trim() <> "" And AppKey.Trim().Length = 32 Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.ProcessCloseApp(DeviceId, AppId, AppKey, StationId)
                                Response.Write(ReturnValue)
                                Response.End()
                            Else
                                Response.Write(-1)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    ElseIf methodName.ToLower() = "multiusage" Then
                        'Method นี้อยู่ในกรณีที่ส่งคำสั่งมาหลายๆคำสั่งพร้อมๆกันเป็นสตริงรูปแบบ JsonString มา ต้องนำ JsonString มาถอดให้เป็น Object แล้วไล่เข้าไปเก็บ Log ตาม Process
                        'เป็นชุด Log ของการเปิด App
                        Dim RunAppList As String = Request.Form("RunAppList")
                        'เป็นชุด Log ของการเข้าด่านต่างๆ
                        Dim EnterStationList As String = Request.Form("EnterStationList")
                        'เป็นชุด Log ของการกดตอบคำถาม
                        Dim ActionList As String = Request.Form("ActionList")
                        'เป็นชุด Log ของการแก้ไขคำตอบ
                        Dim EditList As String = Request.Form("EditList")
                        'เป็นชุด Log ของการปิด App
                        Dim CloseAppList As String = Request.Form("CloseAppList")
                        Dim AppId As String = Request.Form("AppId")
                        Dim AppKey As String = Request.Form("AppKey")
                        If Not IsNothing(RunAppList) And Not IsNothing(EnterStationList) And Not IsNothing(ActionList) And Not IsNothing(EditList) And Not IsNothing(CloseAppList) And Not IsNothing(AppId) And Not IsNothing(AppKey) Then
                            If RunAppList.Trim() = "" And EnterStationList.Trim() = "" And ActionList.Trim() = "" And EditList.Trim() = "" And CloseAppList.Trim() = "" And AppId.Trim() = "" And AppKey.Trim() = "" Then
                                Response.Write(-1)
                                Response.End()
                            Else
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                                Dim ReturnValue As String = "0"
                                ReturnValue = ClsDroidPad.GetMultiUsage(DeviceId, AppId, AppKey, RunAppList, EnterStationList, ActionList, EditList, CloseAppList)
                                Response.Write(ReturnValue)
                                Response.End()
                            End If
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    Else
                        Response.Write(-1)
                        Response.End()
                    End If
                Else
                    Response.Write("-1")
                    Response.End()
                End If
            End If
        End If
    End Sub

End Class

