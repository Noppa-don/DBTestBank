Imports System.IO
Imports System.Net
Imports KnowledgeUtils

Public Class RegisterMaxonet
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Private Sub btnRegister_ServerClick(sender As Object, e As EventArgs) Handles btnRegister.ServerClick
    '    If txtUsername.Value.ToString = "" Or txtPassword.Value.ToString = "" Then
    '        MsgBox("กรอกข้อมูลให้ครบก่อนนะคะ")
    '    Else
    '        Dim sGUID = System.Guid.NewGuid.ToString()
    '        'Dim url As String = Server.MapPath("~/droidpad/installmaxonet.aspx")
    '        'Dim postData As StringBuilder = New StringBuilder()
    '        'postData.Append("methodName=registerstudent&")
    '        'postData.Append("DeviceUniqueID=" & HttpUtility.UrlEncode(sGUID.ToString) & "&")
    '        'postData.Append("username=" & HttpUtility.UrlEncode(txtUsername.Value) & "&")
    '        'postData.Append("password=" & HttpUtility.UrlEncode(txtPassword.Value))

    '        Dim maxOnet As New MaxOnetManagement()
    '        maxOnet.DeviceId = sGUID.ToString
    '        maxOnet.KCU_Type = MaxOnetRegisterType.student
    '        maxOnet.UserName = txtUsername.Value
    '        maxOnet.KeyCode = txtPassword.Value
    '        Dim LogGuid As String = System.Guid.NewGuid().ToString()
    '        ClsLog.Record("-------------------------------------------")
    '        ClsLog.Record(LogGuid & " : Start InstallMaxOnet PC UserName = " & txtUsername.Value & " ,KeyCode = " & txtPassword.Value)

    '        Dim ReturnValue = maxOnet.GetToken()
    '        If ReturnValue.ToString = "-1" Then
    '            MsgBox("ลงทะเบียนไม่สำเร็จค่ะ")
    '        Else
    '            Response.Redirect("../MaxOnet/RegisterStudent.aspx?Token=" & ReturnValue.ToString & "&DeviceId=" & sGUID.ToString & "&keycode=" & txtPassword.Value)
    '        End If
    '    End If
    'End Sub

    <Services.WebMethod()>
    Public Shared Function InstallDevice(ByVal txtUserName As String, ByVal txtPassword As String) As String
        Dim maxOnet As New MaxOnetManagement()
        Dim sGUID = System.Guid.NewGuid.ToString()
        maxOnet.DeviceId = sGUID.ToString
        maxOnet.KCU_Type = MaxOnetRegisterType.student
        maxOnet.UserName = txtUserName
        maxOnet.KeyCode = txtPassword
        Dim LogGuid As String = System.Guid.NewGuid().ToString()
        ClsLog.Record(LogGuid & " : Start InstallMaxOnet PC UserName = " & txtUserName & " ,KeyCode = " & txtPassword)

        Dim TokenId As String = maxOnet.GetToken()
        Dim ReturnValue = ""

        If ReturnValue <> "-1" Then
            ReturnValue = "RegisterStudent.aspx?Token=" & TokenId.ToString & "&DeviceId=" & sGUID.ToString & "&keycode=" & txtPassword
            Dim newGuid As Guid = Guid.Parse(TokenId)
            maxOnet.UpdatePCType(newGuid.ToString)
        End If

        Return ReturnValue

    End Function

End Class