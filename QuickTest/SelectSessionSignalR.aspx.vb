Imports System.Web.Script.Serialization

Public Class SelectSessionSignalR
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    '<Services.WebMethod()>
    'Public Shared Function setCurrentPageWhenClickCodeBehind(ByVal gotoPage As String)
    '    Dim ClsSelectSess As New ClsSelectSession()
    '    'Dim gotoPage As String = "~/testset/step2.aspx"
    '    ClsSelectSess.setCurrentPageWhenClickBtn(gotoPage)
    '    Return "success"
    'End Function

    <Services.WebMethod()>
    Public Shared Function setUnload(ByVal unload As Boolean)
        'ClsSess.setOnUnload()
        HttpContext.Current.Session("UnLoad") = unload
        setUnload = "OK"
    End Function
    <Services.WebMethod()>
    Public Shared Function getUnload() As Boolean
        'getUnload = ClsSess.getOnUnload()
        getUnload = HttpContext.Current.Session("UnLoad")
    End Function

    <Services.WebMethod()>
    Public Shared Function setCurrentPage(ByVal thisPage As String)
        Dim ClsSess As New ClsSelectSession()
        ClsSess.setCurrentPage(thisPage)
        setCurrentPage = "OK"
    End Function
    <Services.WebMethod()>
    Public Shared Function getCurrentPage() As String
        Dim ClsSess As New ClsSelectSession()
        getCurrentPage = ClsSess.getCurrentPage()
    End Function

    <Services.WebMethod()>
    Public Shared Function setNewTestSetId(ByVal testsetId As String) As String
        Dim ClsSess As New ClsSelectSession()
        ClsSess.setTestSetId(testsetId)
        setNewTestSetId = "OK"
    End Function

    <Services.WebMethod()>
    Public Shared Function resetObjTestset() As String
        Dim ClsSess As New ClsSelectSession()
        ClsSess.resetValueInSession()
        resetObjTestset = "OK"
    End Function
End Class