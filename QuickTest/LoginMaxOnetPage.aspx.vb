Public Class LoginMaxOnetPage
    Inherits System.Web.UI.Page

    Dim clsStudent As Student

    Public Property runmode As String
        Get
            Return ViewState("_runmode")
        End Get
        Set(ByVal value As String)
            ViewState("_runmode") = value
        End Set
    End Property
    Public Property cssName As String
        Get
            Return ViewState("_cssName")
        End Get
        Set(ByVal value As String)
            ViewState("_cssName") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Application("runmode").ToString.ToLower = "wetest" Then
            runmode = "wetest"

        Else
            runmode = "maxonet"
        End If

        cssName = "<link rel=""stylesheet"" href=""css/" + runmode + "Style.css"" type=""text/css"" />"
        HttpContext.Current.Session("cssName") = cssName

    End Sub
End Class