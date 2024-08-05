Imports KnowledgeUtils

Public Class SchoolNews
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    '<tr>
    '          <th class="ForLeft" style="width:10%">วันที่</th>
    '          <th class="ForRight" style="width:90%">เนื้อหา</th>
    '      </tr>
    '      <tr>
    '          <td class="ForLeft">
    '              <div>
    '                  <span class="spnDate">19/7/56 15:50</span>
    '                  <br />
    '                  <span class="spnTeacherAn">สุชัจจ์ วัฒนสุขกมล</span>
    '              </div>
    '          </td>
    '          <td class="ForRight tdRight">
    '              <span class="spnNewsDetail">มีข่าวประกาศจากทางโรงเรียนแจ้งให้ทราบว่ากำหนดการเปิดเทอมใกล้เข้ามาแล้วให้นักเรียนทุกคนเตรียมตัวเรียนได้แล้วจ้า</span>
    '          </td>
    '      </tr>

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dummy SchoolCode
        Dim SchoolCode As String = "1000001"

        'เช็คว่าเคยลงทะเบียนหรือยัง ถ้ายังต้องไปหน้าลงทะเบียนก่อน
        CheckRegister(Request.QueryString("DeviceId").ToString())

        Dim dt As DataTable = GetDtNews(SchoolCode)
        ' add column timeago
        dt.Columns.Add("TimeAgo", GetType(String))

        For Each row In dt.Rows
            row("TimeAgo") = Convert.ToDateTime(row("News_StartDate")).ToPointPlusTime()
        Next

        If dt.Rows.Count > 0 Then
            Repeater1.DataSource = dt
            Repeater1.DataBind()
        End If

    End Sub

    Private Sub CheckRegister(ByVal DeviceId As String)

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblParent WHERE DeviceId = '" & _DB.CleanString(DeviceId) & "' AND IsActive = 1; "
        Dim CheckCount As String = _DB.ExecuteScalar(sql)

        If CType(CheckCount, Integer) = 0 Then
            'Response.Redirect("~/Watch/RegisterParent.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
            Response.Redirect("~/Watch/AddChild2.aspx?DeviceId=" & DeviceId & "&Sendpage=Watch/SchoolNews.aspx")
        End If

    End Sub

    Private Function GetDtNews(ByVal SchoolId As String)

        Dim sql As String = " select News_Information,News_Announcer,News_StartDate from t360_tblNews where School_Code = '" & SchoolId & "' and News_ToStudent = 1 " &
                            " and News_IsActive = 1 and dbo.GetThaiDate() >= News_StartDate and dbo.GetThaiDate() <= News_EndDate "
        'Dim sql As String = " select News_Information,News_Announcer,News_StartDate from t360_tblNews"
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function


End Class