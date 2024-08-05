Imports System.Web
Public Class EditHomeworkPage
    Inherits System.Web.UI.Page
    'เก็บค่า ModuleAssignmentId เอาไว้ใช้ที่ฝั่ง Javascript
    Public MAID As String

    ''' <summary>
    ''' ทำการหาข้อมูลมาแสดงรายละเอียดใน Panel ด้านบน แสดงพวก ชื่อการบ้าน , และการตั้งค่าต่างๆของการบ้าน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("MAID") Is Nothing Then
            Response.Write("ไม่มี MAID")
            Exit Sub
        End If

        MAID = Request.QueryString("MAID").ToString()

        If Not Page.IsPostBack Then
            TestsetHeaderControl1.SetByMaId(MAID)
        End If

    End Sub


    ''' <summary>
    ''' ทำการลบการบ้านที่เลือกมา
    ''' </summary>
    ''' <param name="MAID">Id ของ tblModuleAssignment ของการบ้านที่ต้องการลบ</param>
    ''' <returns></returns>
    ''' <remarks>0 ถ้าไม่มี session , "Error" ถ้าไม่สำเร็จ , "Complete" ถ้าสำเร็จ</remarks>
    <Services.WebMethod()>
    Public Shared Function DeleteHomeworkCodeBehind(ByVal MAID As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""
        Try
            _DB.OpenWithTransection()

            'Update tblModuleAssignMent
            sql = " UPDATE dbo.tblModuleAssignment SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE MA_Id = '" & MAID & "' "
            _DB.ExecuteWithTransection(sql)

            'Update tblModuleDetailCompletion
            sql = " UPDATE dbo.tblModuleDetailCompletion SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE MA_Id = '" & MAID & "' "
            _DB.ExecuteWithTransection(sql)

            'Update tblModuleAssignmentDetail
            sql = " UPDATE dbo.tblModuleAssignmentDetail SET IsActive = 0,Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE MA_Id = '" & MAID & "' "
            _DB.ExecuteWithTransection(sql)

            _DB.CommitTransection()
            Return "Complete"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            Return "Error"
        End Try

    End Function



End Class