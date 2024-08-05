Imports GdPicture10
Imports System.Drawing
Imports System.IO
Imports KnowledgeUtils

Public Class MergePNG
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()
    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql())

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ClsDroidPad.EnableDebug(("เข้ามาหน้า MergePNG<br />"))
        If Request.Form("StudentId") Is Nothing Or Request.Form("StudentId") = "" Then
            ClsDroidPad.EnableDebug("ไม่มี StudentId ส่งมา")
            HttpContext.Current.Session("StrError") &= "ไม่มี StudentId ส่งมา"
            Response.Write("GenAvatarError")
            Response.End()
        End If

        Dim StudentId As String = Request.Form("StudentId").ToString()

        If GenAvatar(StudentId) = "GenAvatarError" Then
            Response.Write("GenAvatarError")
            Response.End()
        End If

        Response.Write("Complete")
        Response.End()

    End Sub

    Private Function GenAvatar(ByVal StudentId As String) As String
        Try
            Dim dt As DataTable = GetDtStudentItem(StudentId)
            If dt.Rows.Count > 0 Then
                Dim SchoolCode As String = GetSchoolCodeByStudentId(StudentId)
                Dim ArrItemSeq As ArrayList = GenArrayThaiItemSequence()
                Dim oLicenseManager As New LicenseManager
                oLicenseManager.RegisterKEY("198459578734376851114112016468913")
                oLicenseManager.RegisterKEY("788894759717106801114143528522197")
                oLicenseManager.RegisterKEY("904333449703058531111171221964670")
                oLicenseManager.RegisterKEY("808828486749199831117143694591464")
                ClsDroidPad.EnableDebug(("Register GDpicture<br />"))
                Dim Image1 As New GdPicture10.GdPictureImaging
                Dim MainImage As Integer = Image1.CreateGdPictureImageFromFile(Server.MapPath("./Items/BG.png"))
                Dim WidthMainImage As Integer = Image1.GetWidth(MainImage)
                Dim HeightMainImage As Integer = Image1.GetHeight(MainImage)
                For Each r In ArrItemSeq
                    Dim Result() As DataRow = dt.Select("SIC_Name='" & r & "'")
                    ClsDroidPad.EnableDebug("Get รูป <br />")
                    If Result.Count > 0 Then
                        Dim ImageName As String = Result(0)("Image_FileName")
                        Dim OverlayImage As Integer = Image1.CreateGdPictureImageFromFile(Server.MapPath("./Items/" & ImageName))
                        Dim OffSetX As Integer = (WidthMainImage - Image1.GetWidth(OverlayImage)) / 2
                        Dim OffsetY As Integer = (HeightMainImage - Image1.GetHeight(OverlayImage)) / 2
                        Image1.DrawGdPictureImage(OverlayImage, MainImage, OffSetX, OffsetY, Image1.GetWidth(OverlayImage), Image1.GetHeight(OverlayImage), Drawing2D.InterpolationMode.HighQualityBicubic)
                        Image1.ReleaseGdPictureImage(OverlayImage)
                        ClsDroidPad.EnableDebug("Merge รูป <br />")
                    End If
                Next
                'Save
                CreateFolder(StudentId, SchoolCode)

                Dim dtStudentInfo As DataTable = GetStudentInfo(StudentId)
                If dtStudentInfo.Rows.Count > 0 Then
                    'Overlay StudentInfo

                    'Image1.DrawText(MainImage, "Lv." & dtStudentInfo.Rows(0)("Point_Level").ToString(),  52, 0,  20, GdPicture10.FontStyle.FontStyleBold, Color.Black, "Arial", True)
                    'Image1.DrawText(MainImage, "Lv." & dtStudentInfo.Rows(0)("Point_Level").ToString(), 50, 0, 20, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)

                    'Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("Student_FirstName").ToString(), 2, 110, 25, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)
                    'Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("Student_FirstName").ToString(), 0, 110, 25, GdPicture10.FontStyle.FontStyleBold, ColorTranslator.FromHtml("#e831e8"), "Arial", True)

                    'Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("StudentClassRoom").ToString(), 2, 140, 18, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)
                    'Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("StudentClassRoom").ToString(), 0, 140, 18, GdPicture10.FontStyle.FontStyleBold, Color.Tomato, "Arial", True)

                    Image1.DrawText(MainImage, "Lv." & dtStudentInfo.Rows(0)("Point_Level").ToString(), 142, 0, 20, GdPicture10.FontStyle.FontStyleBold, Color.Black, "Arial", True)
                    Image1.DrawText(MainImage, "Lv." & dtStudentInfo.Rows(0)("Point_Level").ToString(), 140, 0, 20, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)

                    Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("Student_FirstName").ToString(), 2, 250, 25, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)
                    Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("Student_FirstName").ToString(), 0, 250, 25, GdPicture10.FontStyle.FontStyleBold, ColorTranslator.FromHtml("#e831e8"), "Arial", True)

                    Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("StudentClassRoom").ToString(), 2, 290, 18, GdPicture10.FontStyle.FontStyleBold, Color.White, "Arial", True)
                    Image1.DrawText(MainImage, dtStudentInfo.Rows(0)("StudentClassRoom").ToString(), 0, 290, 18, GdPicture10.FontStyle.FontStyleBold, Color.Tomato, "Arial", True)

                    Dim statusSave As GdPicture10.GdPictureStatus
                    'Save รูปใหญ่ก่อน
                    statusSave = Image1.SaveAsPNG(MainImage, Server.MapPath("../UserData/" & SchoolCode & "/{" & StudentId & "}/avt_large.png"))
                    ClsDroidPad.EnableDebug("GD-SaveAsPNG-Status: " & statusSave.ToString())
                    ClsDroidPad.EnableDebug(Server.MapPath("../UserData/" & SchoolCode & "/{" & StudentId & "}/avt.png"))
                    'Resize เพื่อ Save เป็นรูปเล็ก
                    Image1.Resize(MainImage, 109, 164, Drawing2D.InterpolationMode.HighQualityBilinear)
                    Image1.SaveAsPNG(MainImage, Server.MapPath("../UserData/" & SchoolCode & "/{" & StudentId & "}/avt.png"))

                    Image1.ReleaseGdPictureImage(MainImage)
                    ClsDroidPad.EnableDebug("Merge รูปเสร็จเรียบร้อย<br />")

                    'Random SeqNo ใหม่
                    RandomNewSeqNo(StudentId)
                    Return ""
                End If
            Else
                ClsDroidPad.EnableDebug("ไม่มีข้อมูลใน dt<br />")
                HttpContext.Current.Session("StrError") &= "ไม่มีข้อมูลใน dt<br />"
                Return "GenAvatarError"
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            ClsDroidPad.EnableDebug(ex.ToString())
            HttpContext.Current.Session("StrError") &= ex.ToString() & "<br />"
            Throw New Exception("GenAvatarError")
        End Try
    End Function

    Private Function GetStudentInfo(ByVal StudentId As String) As DataTable
        Dim sql As String = " SELECT Student_FirstName,(Student_CurrentClass + Student_CurrentRoom) AS StudentClassRoom ,Point_Level " &
                            " FROM dbo.t360_tblStudent INNER JOIN dbo.tblStudentPoint ON dbo.t360_tblStudent.Student_Id = dbo.tblStudentPoint.Student_Id " &
                            " WHERE dbo.t360_tblStudent.Student_Id = '" & StudentId.CleanSQL & "' "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Sub RandomNewSeqNo(ByVal StudentId As String)
        Dim sql As String = " SELECT Student_AvatarSeqNo FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId.CleanSQL & "' "
        Dim CheckOldSeqNo As String = _DB.ExecuteScalar(sql)
        Dim OldSeqNo As Integer = 0
        If CheckOldSeqNo <> "" Then
            OldSeqNo = CInt(CheckOldSeqNo)
        End If
        Dim NewSeqNo As Integer = GetRndSeqNumber()
        Do Until NewSeqNo <> Math.Abs(OldSeqNo)
            NewSeqNo = GetRndSeqNumber()
        Loop
        sql = " UPDATE dbo.t360_tblStudent SET Student_AvatarSeqNo = '" & NewSeqNo & "',Lastupdate = dbo.GetThaiDate(),ClientId = NULL WHERE Student_Id = '" & StudentId.CleanSQL & "' "
        _DB.Execute(sql)
    End Sub

    Private Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        For i = 1 To 3
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

    Private Sub CreateFolder(ByVal StudentId As String, ByVal SchoolCode As String)
        Dim StrPath As String = Server.MapPath("../UserData/" & SchoolCode & "/{" & StudentId & "}")
        If Directory.Exists(StrPath) = False Then
            Directory.CreateDirectory(StrPath)
        End If
    End Sub

    Private Function GetDtStudentItem(ByVal StudentId As String) As DataTable
        Dim sql As String = " SELECT Image_FileName,Seq_No,dbo.tblShopItemCategory.SIC_Name FROM dbo.tblStudentItem INNER JOIN dbo.tblShopItem " &
                            " ON dbo.tblStudentItem.ShopItem_Id = dbo.tblShopItem.ShopItem_Id INNER JOIN dbo.tblShopItemCategory " &
                            " ON dbo.tblShopItem.SIC_Id = dbo.tblShopItemCategory.SIC_Id WHERE Student_Id = '" & StudentId.CleanSQL & "' " &
                            " AND dbo.tblStudentItem.IsActive = 1 ORDER BY Seq_No "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Function GetSchoolCodeByStudentId(ByVal StudentId As String) As String
        Dim sql As String = " SELECT School_Code FROM dbo.t360_tblStudent WHERE Student_Id = '" & StudentId.CleanSQL & "' AND Student_IsActive = 1 "
        Dim SchoolCode As String = _DB.ExecuteScalar(sql)
        Return SchoolCode
    End Function

    Private Function GenArrayItemSequence() As ArrayList
        Dim ArrItem As New ArrayList
        ArrItem.Add("Character")
        ArrItem.Add("shoe")
        ArrItem.Add("pants")
        ArrItem.Add("belt")
        ArrItem.Add("shirt")
        ArrItem.Add("head")
        ArrItem.Add("scarf")
        ArrItem.Add("armband")
        ArrItem.Add("watch")
        ArrItem.Add("mobilephone")
        ArrItem.Add("glasses")
        ArrItem.Add("hat")
        Return ArrItem
    End Function

    Private Function GenArrayThaiItemSequence() As ArrayList
        Dim ArrItem As New ArrayList
        ArrItem.Add("ตัว")
        ArrItem.Add("รองเท้า")
        ArrItem.Add("กางเกง")
        ArrItem.Add("belt")
        ArrItem.Add("เสื้อ")
        ArrItem.Add("หัว")
        ArrItem.Add("ผ้าพันคอ")
        ArrItem.Add("ปลอกแขน")
        ArrItem.Add("นาฬิกา")
        ArrItem.Add("มือถือ")
        ArrItem.Add("แว่น")
        ArrItem.Add("หมวก")
        Return ArrItem
    End Function

End Class