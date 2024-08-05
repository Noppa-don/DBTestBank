Imports KnowledgeUtils
Imports System.Web

Public Class ContactSupport
    Inherits System.Web.UI.Page
    Public WpfFingerPrint As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If HttpContext.Current.Request.QueryString("fp") IsNot Nothing Then
            WpfFingerPrint = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString("fp")).Replace(" ", "+")
        Else
            WpfFingerPrint = ""
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function PostToContactKNSupport(ByVal FingerPrint As String)
        Try
            Dim responseValue As String = ""
            Dim remoteCallId As String = PostToCreateRemoteCallAndGetRemoteCallId(FingerPrint)
            If remoteCallId <> "" Then
                Return remoteCallId
            Else
                Return "-1"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function

    Private Shared Function PostToCreateRemoteCallAndGetRemoteCallId(FingerPrint As String) As String
        Try
            Dim newManageNet As New ManageNet(ConfigurationManager.AppSettings("WebServiceURL") & "CreateNewRemoteCall")
            Dim dataHashtable As New Hashtable()
            dataHashtable.Add("FingerPrint", FingerPrint)
            Dim param As Specialized.NameValueCollection = newManageNet.GetNameValueCollectionByHashtable(dataHashtable)
            If param IsNot Nothing Then
                Dim vncId As String = ""
                Dim remoteCallId As String = newManageNet.PostDataAndGetResponse(param)
                If remoteCallId IsNot Nothing AndAlso remoteCallId <> "" Then
                    Return remoteCallId
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class