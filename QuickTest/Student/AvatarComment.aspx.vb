Imports System.Data.SqlClient

Public Class AvatarComment
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Private Property dtBadword As DataTable
        Get
            dtBadword = ViewState("_dtBadword")
        End Get
        Set(value As DataTable)
            ViewState("_dtBadword") = value
        End Set
    End Property

    Private Property AVT_From As String
        Get
            AVT_From = ViewState("_AVT_From")
        End Get
        Set(value As String)
            ViewState("_AVT_From") = value
        End Set
    End Property

    Private Property AVT_To As String
        Get
            AVT_To = ViewState("_AVT_To")
        End Get
        Set(value As String)
            ViewState("_AVT_To") = value
        End Set
    End Property

    Private Property School_Code As String
        Get
            School_Code = ViewState("School_Code")
        End Get
        Set(value As String)
            ViewState("School_Code") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If HttpContext.Current.Request.QueryString("AVT_From") IsNot Nothing And HttpContext.Current.Request.QueryString("AVT_From") <> "" And HttpContext.Current.Request.QueryString("AVT_To") IsNot Nothing And HttpContext.Current.Request.QueryString("AVT_To") <> "" Then
            If Not Page.IsPostBack Then
                'Open Connection
                Dim connAvatarComment As New SqlConnection
                _DB.OpenExclusiveConnect(connAvatarComment)
                AVT_To = HttpContext.Current.Request.QueryString("AVT_To").ToString()
                AVT_From = HttpContext.Current.Request.QueryString("AVT_From").ToString()
                If dtBadword Is Nothing Then
                    BindDtBadword(connAvatarComment)
                End If
                RenderComment(connAvatarComment)
                'CloseConnection
                _DB.CloseExclusiveConnect(connAvatarComment)
            End If
        End If
    End Sub

    Private Sub BindDtBadword(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " SELECT BW_From,BW_To FROM dbo.tblBadword ORDER BY LEN(BW_From) DESC "
        dtBadword = _DB.getdata(sql, , InputConn)
    End Sub

    Private Sub RenderComment(Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " SELECT AC_From,AC_Message,LastUpdate FROM dbo.tblAvatarComment  WHERE AC_To = '" & AVT_To & "' AND IsActive = 1 ORDER BY LastUpdate DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql, , InputConn)
        If dt.Rows.Count > 0 Then
            Dim sb As New StringBuilder
            Dim CommentStr As String = ""
            Dim SchoolCode As String = GetSchoolCode(AVT_To, InputConn)
            School_Code = SchoolCode
            For index = 0 To dt.Rows.Count - 1
                CommentStr = ReplaceBadword(dt.Rows(index)("AC_Message").ToString())
                sb.Append("<div id='Div" & (index + 1) & "'  class='ForDivCommentCover'>")
                sb.Append("<div id='comment' class='ForDivComment'><div>")
                sb.Append("<span>" & dt.Rows(index)("LastUpdate").ToString() & "</span></div>")
                sb.Append("<div style='margin-left:15px;margin-top:10px;'>")
                sb.Append(CommentStr)
                Dim AVTPath As String = HttpContext.Current.Server.MapPath("../UserData/" & SchoolCode & "/{" & dt.Rows(index)("AC_From").ToString() & "}/avt.png")
                Dim ImgPath As String = ""
                If System.IO.File.Exists(AVTPath) = True Then
                    sb.Append("</div></div><div id='picture" & (index + 1) & "' style=""background:url('../UserData/" & SchoolCode & "/{" & dt.Rows(index)("AC_From").ToString() & "}/avt.png');background-size:contain;"" class='ForDivAvt'>")
                Else
                    sb.Append("</div></div><div id='picture" & (index + 1) & "' style=""background:url('../UserData/dummy.png');background-size:contain;"" class='ForDivAvt'>")
                End If
                sb.Append("</div></div>")
            Next
            ContentDiv.InnerHtml = sb.ToString()
        End If
    End Sub

    Private Function ReplaceBadword(ByVal InputString As String) As String
        Dim ReturnStr As String = ""
        If dtBadword.Rows.Count > 0 Then
            ReturnStr = InputString
            For index = 0 To dtBadword.Rows.Count - 1
                ReturnStr = ReturnStr.Replace(dtBadword.Rows(index)("BW_From"), dtBadword.Rows(index)("BW_To"))
            Next
        Else
            Return InputString
        End If
        Return ReturnStr
    End Function

    Private Function GetSchoolCode(ByVal InputId As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblStudent WHERE Student_Id = '" & InputId & "' "
        Dim SchoolCode As String = _DB.ExecuteScalar(sql, InputConn)
        Return SchoolCode
    End Function

    Private Sub BtnComment_Click(sender As Object, e As EventArgs) Handles BtnComment.Click
        'Open Connection
        Dim connAvatarComment As New SqlConnection
        _DB.OpenExclusiveConnect(connAvatarComment)

        If txtComment.Text.Trim() <> "" Then
            Try
                Dim sql As String = " INSERT INTO dbo.tblAvatarComment( AC_Id ,AC_From ,AC_To ,AC_Message ,LastUpdate ,IsActive,School_Code) " &
                                    " VALUES  ( NEWID(),'" & AVT_From & "','" & AVT_To & "' ,'" & _DB.CleanString(txtComment.Text.Trim()) & "',dbo.GetThaiDate(),1,'" & School_Code & "') "
                _DB.Execute(sql, connAvatarComment)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
        End If
        RenderComment(connAvatarComment)
        txtComment.Text = ""
        'CloseConnection
        _DB.CloseExclusiveConnect(connAvatarComment)
    End Sub

End Class