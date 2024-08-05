Imports System
Imports System.Web
Imports Microsoft.AspNet.SignalR
'Imports Microsoft.AspNet.SignalR.Hub
'Imports Microsoft.AspNet.SignalR.Hubs

 
Public Class hubSignalR
    Inherits Hub

    Public Sub SendCommand(ByVal GroupName As String, ByVal Cmd As String)
        'Clients.All.send(GroupName)
        'Clients.Group(GroupName).send("Reload")
        Clients.OthersInGroup(GroupName).send(Cmd)
        Clients.Caller.raiseEvent(Cmd)
    End Sub

    Public Sub AddToGroup(ByVal GroupName As String)
        Groups.Add(Context.ConnectionId, GroupName)
    End Sub

    Public Overrides Function OnConnected() As System.Threading.Tasks.Task
        Return MyBase.OnConnected()
    End Function

    Public Overrides Function OnDisconnected() As System.Threading.Tasks.Task
        Return MyBase.OnDisconnected()
    End Function

    Public Overrides Function OnReconnected() As System.Threading.Tasks.Task
        Return MyBase.OnReconnected()
    End Function


    'Public Sub setCheckboxWhenClick(ByVal GroupName As String, ByVal checkboxName As String, ByVal IsChecked As String)
    '    Clients.OthersInGroup(GroupName).setCheckbox(checkboxName, IsChecked)
    'End Sub

    'Public Sub setDropdownWhenChange(ByVal GroupName As String, ByVal dropdownName As String, ByVal val As Integer)
    '    Clients.OthersInGroup(GroupName).setDropdown(dropdownName, val)
    'End Sub

    'Public Sub setTextboxWhenKeyup(ByVal GroupName As String, ByVal textboxName As String, ByVal txt As Integer)
    '    Clients.OthersInGroup(GroupName).setTextbox(textboxName, txt)
    'End Sub

    Public Sub cmdControlBtnPrevNext(ByVal GroupName As String, ByVal cmd As String)
        Clients.OthersInGroup(GroupName).cmdControl(cmd)
        Clients.Caller.raiseEvent(cmd)
    End Sub

    Public Sub SendMessage(ByVal ChatRoomId As String, ByVal ChatUserId As String, ByVal Message As String)

        Dim ClsChat As New ClassChat(New ClassConnectSql())
        Dim GetDataChat As String = ClsChat.SaveMessage(ChatRoomId, ChatUserId, Message)
        Dim SplitStr = GetDataChat.Split("|")
        Dim CMID As String = SplitStr(0)
        Dim StrDateTime As String = SplitStr(1)
        Dim CRID As String = SplitStr(2)
        Clients.Caller.sendComplete(CMID, StrDateTime)
        Clients.OthersInGroup(ChatRoomId).recieveMsg(Message, StrDateTime, CMID, CRID)
        ClsChat = Nothing

    End Sub

    Public Sub IsSeenMessage(ByVal ChatRoomId As String, ByVal CrId As String, ByVal CmId As String)

        Dim ClsChat As New ClassChat(New ClassConnectSql())
        ClsChat.UpdateRecipient(CrId)
        Clients.OthersInGroup(ChatRoomId).RecipientIsSeen(CmId)

    End Sub

    Public Sub SeenMessageHistory(ByVal ChatRoomId As String)

        Clients.OthersInGroup(ChatRoomId).IsSeenHistoryMsg()

    End Sub


End Class
