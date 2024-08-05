Imports KnowledgeUtils.IO

Public Class UploadPicture
    Inherits System.Web.UI.Page

    Dim ClsPdf As New ClsPDF(New ClassConnectSql)
    Public Property ViewStateId As String
        Get
            Return ViewState("VUserId")
        End Get
        Set(ByVal value As String)
            ViewState("VUserId") = value
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            ViewStateId = Request.QueryString("id")
            Image.ImageUrl = "~/UploadPic/pic.jpg"
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim id As Integer = ViewStateId
        Dim filename As String = fileupload.FileName
        If filename = "" Then
            filename = "pic.jpg"
            'WindowsTelerik.ShowAlert("กรุณาเลือกไฟล์รูปที่ตัองการค่ะ", Me, RadDialogAlert)
        End If

        If CheckFileType(Server.MapPath("~/UploadPic/" & fileupload.FileName)) Then
            If Me.fileupload.HasFile Then
                Me.fileupload.SaveAs(Server.MapPath("~/UploadPic/" & "pic.jpg"))
                WindowsTelerik.ShowAlert("บันทึกข้อมูลเรียบร้อยค่ะ", Me, RadDialogAlert)
            End If
            ' Log.Record(Log.LogType.UploadPic, "บันทึกอัพโหลดรูปภาพ", True)
        Else
            WindowsTelerik.ShowAlert("กรุณาเลือกเฉพาะไฟล์นามสกุล'.jpg'หรือ'.jpeg'เท่านั้นค่ะ", Me, RadDialogAlert)
        End If


        'ประกาศตัวแปรส่งpathให้ไหม
        'Dim PathPic As String = Server.MapPath("~/UploadPic/pic.jpg")

        Dim path As String = Server.MapPath("~")
        ClsPdf.CreateUserPassPDF(id, path)
        'Response.Redirect("~/Usermanager/ShowPDF.aspx?file=" & Server.MapPath("~/UserManager/pdf/" & id & ".pdf").ToString)
        Response.Redirect("~/Usermanager/ShowPDF.aspx?file=" & id & ".pdf", False)

    End Sub

    Public Function CheckFileType(ByVal Path As String) As Boolean
        Dim Fi As New ManageFile(Path)
        Dim En = Fi.FileExtensionName
        If En.ToUpper = ".jpg".ToUpper OrElse En.ToUpper = ".jpeg".ToUpper Then
            Return True
        End If
        Return False
    End Function

End Class