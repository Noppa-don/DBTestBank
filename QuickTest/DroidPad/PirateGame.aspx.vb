Public Class PirateGame
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim DeviceId As String = Request.Form("DeviceId")

                If methodName.ToLower() = "runapp" Then
                    Dim AppId As String = Request.Form("AppId")
                    If Not IsNothing(DeviceId) And IsNothing(AppId) Then
                        If DeviceId <> "" And AppId <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            'ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, IsFirstTimeOpen, True)
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
                    Dim StationId As String = Request.Form("StationId")
                    If Not IsNothing(DeviceId) And Not IsNothing(StationId) Then
                        If DeviceId <> "" And StationId <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            ' ReturnValue  = ClsDroidPad.SendMuteAll(DeviceUniqueID, NeedMute)
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
                    Dim AppId As String = Request.Form("AppId")
                    If Not IsNothing(DeviceId) And Not IsNothing(AppId) Then
                        If DeviceId <> "" And AppId <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            'ReturnValue = ClsDroidPad.SendHideAll(DeviceUniqueID, NeedHide)
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
                    Dim StationId As String = Request.Form("StationId")
                    Dim QuestionName As String = Request.Form("QuestionName")
                    Dim AnswerName As String = Request.Form("AnswerName")
                    If Not IsNothing(DeviceId) And Not IsNothing(StationId) And IsNothing(QuestionName) And IsNothing(AnswerName) Then
                        If DeviceId <> "" And StationId <> "" And QuestionName <> "" And AnswerName <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            'ReturnValue = ClsDroidPad.SendLockAll(DeviceUniqueID, NeedLock)
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
                    Dim StationId As String = Request.Form("StationId")
                    Dim AppId As String = Request.Form("AppId")
                    If Not IsNothing(DeviceId) And Not IsNothing(StationId) Then
                        If DeviceId <> "" And StationId <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = ""
                            'ReturnValue = ClsDroidPad.SendLockAll(DeviceUniqueID, NeedLock)
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
                End If
            End If
            Response.Write("-1")
            Response.End()
        End If

    End Sub

End Class