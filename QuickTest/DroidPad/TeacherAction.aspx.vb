Public Class TeacherAction
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim DeviceUniqueID As String = Request.Form("DeviceUniqueID")
                Dim IsFirstTimeOpen As String = Request.Form("FirstTime")
                Dim NeedLock As String = Request.Form("NeedLock")
                Dim NeedMute As String = Request.Form("NeedMute")
                Dim NeedHide As String = Request.Form("NeedHide")

                If methodName.ToLower() = "nextaction" Then
                    If Not IsNothing(DeviceUniqueID) Then
                        If DeviceUniqueID <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim DefaultValue As String = "{""Param"": {""Lock"" : ""0"",""Mute"" : ""0"",""Visible"" : ""1"",""NextURL"" : ""/Activity/EmptySession.aspx"",""MidText"" : """",""BottomText"" : """"}}"
                            Dim ReturnValue As String
                            ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, IsFirstTimeOpen, True)
                            If ReturnValue = "-1" Then
                                Response.Write(DefaultValue)
                            Else
                                Response.Write(ReturnValue)
                            End If
                            ClsLog.Record("TeacherAction : ReturnValue = " & ReturnValue & ", DefaultValue = " & DefaultValue)
                            Response.End()
                        End If
                        End If
                    ElseIf methodName.ToLower() = "muteall" Then
                        If Not IsNothing(DeviceUniqueID) And Not IsNothing(NeedMute) Then
                            If DeviceUniqueID <> "" And NeedMute <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : ""-1""}}"
                                ReturnValue = ClsDroidPad.SendMuteAll(DeviceUniqueID, NeedMute)
                                Response.Write(ReturnValue)
                                Response.End()
                            End If
                        End If
                    ElseIf methodName.ToLower() = "hideall" Then
                        If Not IsNothing(DeviceUniqueID) And Not IsNothing(NeedHide) Then
                            If DeviceUniqueID <> "" And NeedHide <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : ""-1""}}"
                                ReturnValue = ClsDroidPad.SendHideAll(DeviceUniqueID, NeedHide)
                                Response.Write(ReturnValue)
                                Response.End()
                            End If
                        End If
                    ElseIf methodName.ToLower() = "lockall" Then
                        If Not IsNothing(DeviceUniqueID) And Not IsNothing(NeedLock) Then
                            If DeviceUniqueID <> "" And NeedLock <> "" Then
                                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : ""-1""}}"
                                ReturnValue = ClsDroidPad.SendLockAll(DeviceUniqueID, NeedLock)
                                Response.Write(ReturnValue)
                                Response.End()
                            End If
                        End If
                    End If
                End If
                Response.Write("-1")
                Response.End()
            End If


    End Sub
    '<Services.WebMethod()>
    'Public Shared Function NextAction(ByVal DeviceUniqueID As String)

    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""ActionInfoCommand"": {""Lock"" : ""0"",""Mute"" : ""0"",""Visible"" : ""1""},""NextURL"" : {""URL"" : """",""MidText"" : "" "",""BottomText"" : "" ""}}}"
    '    ReturnValue = ClsDroidPad.GetNextAction(DeviceUniqueID, True)
    '    Return ReturnValue

    'End Function

    '<Services.WebMethod()>
    'Public Shared Function MuteAll(ByVal DeviceUniqueID As String, ByVal NeedMute As String)
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""Result"" : ""0""}}"
    '    ReturnValue = ClsDroidPad.SendMuteAll(DeviceUniqueID, NeedMute)
    '    Return ReturnValue
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function HideAll(ByVal DeviceUniqueID As String, ByVal NeedHide As String)
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""Result"" : ""0""}}"
    '    ReturnValue = ClsDroidPad.SendHideAll(DeviceUniqueID, NeedHide)
    '    Return ReturnValue
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function LockAll(ByVal DeviceUniqueID As String, ByVal NeedLock As String)
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""Result"" : ""0""}}"
    '    ReturnValue = ClsDroidPad.SendLockAll(DeviceUniqueID, NeedLock)
    '    Return ReturnValue
    'End Function







End Class