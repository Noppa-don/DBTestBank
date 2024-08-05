Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class StudentDetailPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Dim ClsViewReport As New ClassViewReport(New ClassConnectSql())
    Dim ClsUser As New ClsUser(New ClassConnectSql())

    Protected IsMaxOnet As Boolean = ClsKNSession.IsMaxONet
    Protected TokenId As String

    Public Property StudentId As String
        Get
            StudentId = ViewState("_StudentId")
        End Get
        Set(ByVal value As String)
            ViewState("_StudentId") = value
        End Set
    End Property

    Public Property CalendarId As String
        Get
            CalendarId = ViewState("_CalendarId")
        End Get
        Set(ByVal value As String)
            ViewState("_CalendarId") = value
        End Set
    End Property

    Public Property SchoolId As String
        Get
            SchoolId = ViewState("_SchoolId")
        End Get
        Set(ByVal value As String)
            ViewState("_SchoolId") = value
        End Set
    End Property

    Protected JSDeviceId As String

    Dim redis As New RedisStore()
    Protected NeedShowTip As Boolean

    Private DVID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ดักไว้ ถ้า Application ทั้งหมด Is Nothing ให้โหลดค่าขึ้นมาใหม่ กรณีนี้เจอตอน ฝึกฝนจากคอมพิวเตอร์
        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            KNConfigData.LoadData()
        End If

        DVID = Request.QueryString("deviceuniqueid")
        TokenId = Request.QueryString("token")

        If Session("PDeviceId") Is Nothing Then
            If IsMaxOnet Then
                Response.Redirect(String.Format("ChooseTestsetMaxOnet.aspx?deviceUniqueId={0}&token={1}", DVID, TokenId))
            End If
            Response.Write("ไม่มี DeviceId")
            Exit Sub
        End If

        Dim DeviceId As String = Session("PDeviceId")
        JSDeviceId = DeviceId

        If (IsMaxOnet) Then
            btnHomework.Text = "ฝึกฝนให้ชำนาญ"
            btnQuizHistory.Visible = False

            GridDetailStudentControl1.DeviceuniqueId = DVID
            GridDetailStudentControl1.Token = TokenId
        End If

        If Not Page.IsPostBack Then
            'Open Connection
            Dim connStudentDetail As New SqlConnection
            _DB.OpenExclusiveConnect(connStudentDetail)

            'หา StudentId
            StudentId = GetStudentIdByDeviceId(DeviceId, connStudentDetail)
            If StudentId = "" Then
                Response.Write("หา StudentId ไม่ได้")
                Exit Sub
            End If
            'หา SchoolId 
            SchoolId = GetSchoolIdByStudentId(StudentId, connStudentDetail)
            If SchoolId = "" Then
                Response.Write("หารหัสโรงเรียนไม่ได้")
                Exit Sub
            End If
            'หา CalendarId
            CalendarId = ClsViewReport.GetCalendarId(SchoolId, connStudentDetail)
            If CalendarId = "" Then
                Response.Write("หาปีการศึกษาไม่ได้")
                Exit Sub
            End If
            CreateDivStudentIfo(StudentId, connStudentDetail)
            'GridDetailStudentControl1.CreateGridHomework(StudentId, CalendarId, "", True,, IsMaxOnet)
            GridDetailStudentControl1.CreateGridPracticeHistory(StudentId, CalendarId, "", True)

            'Close Connection
            _DB.CloseExclusiveConnect(connStudentDetail)

            If Not Page.IsPostBack Then
                ' ส่วนของการแสดง qtip
                If Not redis.Getkey(Of Boolean)(String.Format("{0}_IsViewAllTips", Session("UserId").ToString())) Then
                    Dim pageName As String = HttpContext.Current.Request.Url.AbsolutePath.ToString.ToLower
                    Dim ClsUserViewPageWithTip As New UserViewPageWithTip(Session("UserId").ToString())
                    NeedShowTip = ClsUserViewPageWithTip.CheckUserViewPageWithTip(pageName)
                End If
            End If
            'NeedShowTip = True
        End If

    End Sub


#Region "Query & Function"

    Private Sub CreateDivStudentIfo(ByVal StudentId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sb As New StringBuilder
        If Not IsMaxOnet Then
            Dim sql As String = ""
            Dim NumberHomeworkNotDone As String = GetNumberOfHomeworkStudent(StudentId, CalendarId, True, InputConn)
            Dim NumberHomeworkNotComplete As String = GetNumberOfHomeworkStudent(StudentId, CalendarId, False, InputConn)

            sb.Append("<div style='width:200px;display:inline-block;'  >")

            Dim PhotoStatus As Boolean
            PhotoStatus = ClsUser.GetStudentHasPhoto(StudentId)
            If PhotoStatus Then

                sb.Append("<img src='../UserData/" & SchoolId & "/{" & StudentId & "}/Id.jpg' class='ForImgStudent' />")
            Else

                sb.Append("<img src='MonsterID.axd?seed=" & StudentId & "&size=120' class='ForImgStudent' />")
            End If
            sb.Append("</div>")


            sb.Append("<div style='width:440px;display:inline-block;' class='ForHomeworkInfo'>")
            sb.Append("<table>")
            sb.Append("<tr><td class='ForTdNoneBackground'>")
            sb.Append("<span>การบ้านทั้งหมดที่ยังไม่ทำ : " & NumberHomeworkNotDone & "</span>")
            sb.Append("</td></tr>")
            sb.Append("<tr><td class='ForTdNoneBackground'>")
            sb.Append("<span>การบ้านทั้งหมดที่ยังทำไม่เสร็จ : " & NumberHomeworkNotComplete & "</span>")
            sb.Append("</td></tr>")
            sb.Append("</table>")
            sb.Append("</div>")
        End If

        DivStudentInfo.InnerHtml = sb.ToString()

    End Sub

    Private Function GetStudentIdByDeviceId(ByVal DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT Owner_Id FROM dbo.t360_tblTablet INNER JOIN dbo.t360_tblTabletOwner " &
                            " ON dbo.t360_tblTablet.Tablet_Id = dbo.t360_tblTabletOwner.Tablet_Id " &
                            " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' AND dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 "
        Dim StudentId As String = _DB.ExecuteScalar(sql, InputConn)
        Return StudentId

    End Function

    Private Function GetNumberOfHomeworkStudent(ByVal StudentId As String, ByVal CalendarId As String, ByVal IsForNotDone As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim StrWhere As Integer = 0
        If IsForNotDone = True Then
            StrWhere = 0
        Else
            StrWhere = 1
        End If

        Dim sql As String = " SELECT COUNT(tblModuleDetailCompletion.Module_Status) AS HomeworkNotDone " &
                            " FROM tblModule INNER JOIN tblModuleAssignment ON tblModule.Module_Id = tblModuleAssignment.Module_Id INNER JOIN " &
                            " tblModuleDetail ON tblModule.Module_Id = tblModuleDetail.Module_Id INNER JOIN " &
                            " tblModuleDetailCompletion ON tblModuleAssignment.MA_Id = tblModuleDetailCompletion.MA_Id AND " &
                            " tblModuleDetail.ModuleDetail_Id = tblModuleDetailCompletion.ModuleDetail_Id " &
                            " WHERE (tblModuleDetailCompletion.Student_Id = '" & StudentId & "') AND " &
                            " (dbo.tblModuleAssignment.Calendar_Id = '" & CalendarId & "') " &
                            " AND (dbo.tblModuleDetailCompletion.Module_Status = " & StrWhere & ")  AND (tblModuleDetailCompletion.IsActive = 1)"

        Dim ReturnNumber As String = _DB.ExecuteScalar(sql, InputConn)
        Return ReturnNumber

    End Function

    Private Function GetSchoolIdByStudentId(ByVal StudentId As String, Optional ByRef InputConn As SqlConnection = Nothing)

        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId & "' "
        Dim SchoolId As String = _DB.ExecuteScalar(sql, InputConn)
        Return SchoolId

    End Function

#End Region

    Private Sub btnQuizHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuizHistory.Click

        GridDetailStudentControl1.CreateGridQuizHistory(StudentId, CalendarId, "", True)

    End Sub

    Private Sub btnHomework_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHomework.Click

        GridDetailStudentControl1.CreateGridHomework(StudentId, CalendarId, "", True)

    End Sub

    Private Sub btnPracticeHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPracticeHistory.Click

        GridDetailStudentControl1.CreateGridPracticeHistory(StudentId, CalendarId, "", True)

    End Sub

End Class