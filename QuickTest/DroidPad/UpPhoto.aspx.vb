Imports System.IO
Imports GdPicture10
Imports System.Drawing

Public Class UpPhoto
    Inherits System.Web.UI.Page
    'ตัวแปรจัดการกับฐานข้อมูล Insert,Update,Delete
    Dim _DB As New ClassConnectSql()

    ''' <summary>
    ''' ทำการ save รูปที่ user upload เข้ามา ที่มีทั้งการส่งเป็น string Base 64 และ upload ไฟล์รูปภาพเข้ามา
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ClsSecurity.CheckConnectionIsSecure()
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim ReturnValue As String = ""
                Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                Dim Mode As String = Request.Form("mode")
                Dim DeviceId As String = Request.Form("DeviceId")
                'ตัวแปรสตริงรูปภาพที่เป็น Base64
                Dim StrBase64 As String = Request.Form("Base64FileData")

                If Mode IsNot Nothing And Mode <> "" And DeviceId IsNot Nothing And DeviceId <> "" Then
                    'Upload รูปขึ้น Server
                    If Mode.ToLower() = "tabowner" Then
                        'หาข้อมูลนักเรียนจาก DeviceId
                        Dim dt As DataTable = GetStudentDataByDeviceId(DeviceId)
                        If dt.Rows.Count > 0 Then
                            'ทำการ Register STROKESCRIBE
                            Dim oLicenseManager As New LicenseManager
                            oLicenseManager.RegisterKEY("198459578734376851114112016468913")
                            oLicenseManager.RegisterKEY("788894759717106801114143528522197")
                            oLicenseManager.RegisterKEY("904333449703058531111171221964670")
                            oLicenseManager.RegisterKEY("808828486749199831117143694591464")
                            Dim SchoolCode As String = dt.Rows(0)("School_Code").ToString()
                            Dim StudentId As String = dt.Rows(0)("Student_Id").ToString()
                            'เช็คก่อนว่ามีข้อมูลของเด็กคนนี้ใน tblStudentPhoto หรือยัง
                            If CheckisHaveDataInStudentPhoto(StudentId) = False Then
                                'ถ้ายังไม่มีข้อมูลต้อง Insert
                                If InsertDataInStudentPhoto(StudentId) = -1 Then
                                    Response.Write(-2)
                                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                                    Response.End()
                                End If
                            End If
                            'Path ที่จะทำการ Save รูป
                            Dim UploadPath As String = HttpContext.Current.Server.MapPath("../UserData/" & SchoolCode & "/{" & StudentId & "}")
                            If Directory.Exists(UploadPath) = False Then
                                Directory.CreateDirectory(UploadPath)
                            End If

                            'ถ้าเป็นการส่งข้อมูลแบบ Base64 มาต้องเข้า If
                            If StrBase64 <> "" Then
                                Try
                                    StrBase64 = StrBase64.Replace(" ", "+")
                                    ProcessBase64String(StrBase64, UploadPath, StudentId)
                                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                                    Response.Write("Exception = '" & ex.ToString() & "'<br />")
                                    Response.Write("Mode = '" & Mode & "'<br/>")
                                    Response.Write("DeviceId='" & DeviceId & "'<br/>")
                                    Response.Write("Base64FileData='" & StrBase64 & "'")
                                End Try
                            Else
                                Dim UploadFile As HttpFileCollection = Request.Files
                                'เช็คว่าถ้าเป็นการส่งรูปเข้ามาแบบ Request.File ต้องเข้าทำ Sub นี้
                                If UploadFile.Keys.Count > 0 Then
                                    ProcessFileCollection(UploadFile, UploadPath, StudentId)
                                Else
                                    Response.Write(-1)
                                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                                    Response.End()
                                End If
                            End If
                        Else
                            Response.Write(-1)
                            HttpContext.Current.ApplicationInstance.CompleteRequest()
                            Response.End()
                        End If
                    End If
                Else
                    Response.Write(-1)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Response.End()
                End If
            Else
                Response.Write(-1)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                Response.End()
            End If
        End If

    End Sub

    ''' <summary>
    ''' ทำการ save รูปที่เป็น สตริง Base64
    ''' </summary>
    ''' <param name="StrBase64">สตริงรูปภาพที่เป็น Base64</param>
    ''' <param name="UploadPath">Path ที่จะ save รูป</param>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <remarks></remarks>
    Private Sub ProcessBase64String(ByVal StrBase64 As String, ByVal UploadPath As String, ByVal StudentId As String)
        Try
            'ทำการแปลงสตริงให้เป็น Byte ก่อน
            Dim Imagebyte() As Byte = Convert.FromBase64String(StrBase64)
            Dim ms As MemoryStream = New MemoryStream(Imagebyte)

            ms.Write(Imagebyte, 0, Imagebyte.Length)
            'สร้างรูปขึ้นมาจาก Byte ที่อยู่ใน MemoryStream
            Dim Image As Image = Image.FromStream(ms)

            'Save รูปแบบ Original เข้าไปก่อน
            Image.Save(UploadPath & "/IdFullSize_tmp.jpg")

            'Save ให้เป็น JPG 90%
            Dim Imaging1 As New GdPicture10.GdPictureImaging
            Dim BGImage As Integer = Imaging1.CreateGdPictureImageFromFile(Server.MapPath("../images/BGUserPhoto.jpg"))

            Dim MainImage As Integer = Imaging1.CreateGdPictureImageFromFile(UploadPath & "/IdFullSize_tmp.jpg")

            'Bounding Box
            Dim w1 As Integer = 179
            Dim h1 As Integer = 179

            'Image
            Dim h2 As Integer = Imaging1.GetHeight(MainImage)
            Dim w2 As Integer = Imaging1.GetWidth(MainImage)

            Dim r1 As Double = h2 / h1 ' = 2
            Dim r2 As Double = w2 / w1 ' = 1

            If r1 >= r2 Then
                'use ResizeHeightRatio
                Imaging1.ResizeHeightRatio(MainImage, h1, Drawing.Drawing2D.InterpolationMode.HighQualityBicubic)
            Else
                'use ResizeWidthRatio
                Imaging1.ResizeWidthRatio(MainImage, w1, Drawing.Drawing2D.InterpolationMode.HighQualityBicubic)
            End If

            'drawoverlay
            Dim OffSetX As Integer = (w1 - Imaging1.GetWidth(MainImage)) / 2
            Dim OffsetY As Integer = (h1 - Imaging1.GetHeight(MainImage)) / 2

            'ทำการสร้างรูป
            Imaging1.DrawGdPictureImage(MainImage, BGImage, OffSetX, OffsetY, Imaging1.GetWidth(MainImage),
                                        Imaging1.GetHeight(MainImage), Drawing2D.InterpolationMode.HighQualityBicubic)

            Imaging1.ReleaseGdPictureImage(MainImage)

            'save ให้เป็น tmp ไว้ก่อน
            Imaging1.SaveAsJPEG(BGImage, UploadPath & "/Id_tmp.jpg", 90)
            Imaging1.ReleaseGdPictureImage(BGImage)

            'Random SeqNo ใหม่และเช็คว่าไม่ซ้ำกับอันเก่า
            Dim NewseqNo As Integer = GetRndSeqNumber()
            Dim OldSeqPhoto As String = CInt(GetSeqStudentPhoto(StudentId))
            'loop เพื่อเช็คว่า seqno ของรูปภาพมันซ้ำกับที่ random มาใหม่รึเปล่า ถ้าซ้ำต้องไป random มาใหม่ , เงื่อนไขการจบ loop คือ seq อันใหม่ต้องไม่ซ้ำกับเลขเก่า
            Do Until NewseqNo <> OldSeqPhoto
                NewseqNo = GetRndSeqNumber()
            Loop
            'Update SeqNo และ ApprovalStatus = 0 มันจะได้ไปเด้งให้ครูเห็นว่าเด็ก UP รูปมาใหม่
            If UpdateSpSeq(StudentId, NewseqNo) = -1 Then
                Response.Write(-2)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                Response.End()
            End If
            If BusinessTablet360.ClsKNSession.IsMaxONet Then BypassApprovePhotoMaxOnet(StudentId)
            Response.Write(NewseqNo)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            Response.End()
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Throw New Exception(ex.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' ทำการ save รูปที่เป็นแบบ File ที่ upload เข้ามา
    ''' </summary>
    ''' <param name="UploadFile">object รูปภาพที่ upload เข้ามา</param>
    ''' <param name="UploadPath">Path ที่จะ save รูป</param>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <remarks></remarks>
    Private Sub ProcessFileCollection(ByVal UploadFile As HttpFileCollection, ByVal UploadPath As String, ByVal StudentId As String)
        Dim NewseqNo As Integer = 0
        'loop เพื่อวนหาไฟล์รูปภาพที่ upload เข้ามา , วนจนครบทุก item ที่อยู่ใน object รูปภาพที่ upload เข้ามา
        For Each r As String In UploadFile.AllKeys
            'ทำการเช็คว่าเป็นไฟล์รูปภาพรึเปล่า
            If (UploadFile(r).ContentLength > 0) Then
                Dim StrFullFileName As String = UploadFile(r).FileName
                Dim FileExtension As String = System.IO.Path.GetExtension(StrFullFileName)
                'Save รูปแบบ Original เข้าไปก่อน
                UploadFile(r).SaveAs(UploadPath & "/IdFullSize_tmp" & FileExtension)

                'Save ให้เป็น JPG 90%
                Dim Imaging1 As New GdPicture10.GdPictureImaging
                Dim BGImage As Integer = Imaging1.CreateGdPictureImageFromFile(Server.MapPath("../images/BGUserPhoto.jpg"))

                Dim MainImage As Integer = Imaging1.CreateGdPictureImageFromFile(UploadPath & "/IdFullSize_tmp" & FileExtension)

                ' Bounding Box
                Dim w1 As Integer = 179
                Dim h1 As Integer = 152

                'Image
                Dim h2 As Integer = Imaging1.GetHeight(MainImage)
                Dim w2 As Integer = Imaging1.GetWidth(MainImage)

                Dim r1 As Double = h2 / h1 ' = 2
                Dim r2 As Double = w2 / w1 ' = 1

                If r1 >= r2 Then
                    'use ResizeHeightRatio
                    Imaging1.ResizeHeightRatio(MainImage, h1, Drawing.Drawing2D.InterpolationMode.HighQualityBicubic)
                Else
                    'use ResizeWidthRatio
                    Imaging1.ResizeWidthRatio(MainImage, w1, Drawing.Drawing2D.InterpolationMode.HighQualityBicubic)
                End If

                'drawoverlay
                Dim OffSetX As Integer = (w1 - Imaging1.GetWidth(MainImage)) / 2
                Dim OffsetY As Integer = (h1 - Imaging1.GetHeight(MainImage)) / 2

                Imaging1.DrawGdPictureImage(MainImage, BGImage, OffSetX, OffsetY, Imaging1.GetWidth(MainImage),
                                            Imaging1.GetHeight(MainImage), Drawing2D.InterpolationMode.HighQualityBicubic)

                Imaging1.ReleaseGdPictureImage(MainImage)

                Imaging1.SaveAsJPEG(BGImage, UploadPath & "/Id_tmp.jpg", 90)
                Imaging1.ReleaseGdPictureImage(BGImage)

                'Random SeqNo ใหม่และเช็คว่าไม่ซ้ำกับอันเก่า
                NewseqNo = GetRndSeqNumber()
                Dim OldSeqPhoto As String = CInt(GetSeqStudentPhoto(StudentId))
                Do Until NewseqNo <> OldSeqPhoto
                    NewseqNo = GetRndSeqNumber()
                Loop
                'Update SeqNo และ ApprovalStatus = 0 มันจะได้ไปเด้งให้ครูเห็นว่าเด็ก UP รูปมาใหม่
                If UpdateSpSeq(StudentId, NewseqNo) = -1 Then
                    Response.Write(-2)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Response.End()
                End If
            End If
        Next
        If BusinessTablet360.ClsKNSession.IsMaxONet Then BypassApprovePhotoMaxOnet(StudentId)
        Response.Write(NewseqNo)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
        Response.End()
    End Sub

    ''' <summary>
    ''' Fucntion เช็คว่าเด็กคนนี้ถูก Insert ใน tblStudentPhoto ไปหรือยัง
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function CheckisHaveDataInStudentPhoto(ByVal StudentId As String) As Boolean
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblStudentPhoto WHERE Student_Id = '" & StudentId & "' AND IsActive = 1 "
        Dim IsHaveData As Integer = CInt(_DB.ExecuteScalar(sql))
        If IsHaveData > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Function Get SeqPhoto ของนักเรียน
    ''' </summary>
    ''' <param name="StudentId"></param>
    ''' <returns>String:SP_seq</returns>
    ''' <remarks></remarks>
    Private Function GetSeqStudentPhoto(ByVal StudentId As String) As String
        Dim sql As String = " SELECT SP_Seq FROM dbo.tblStudentPhoto WHERE Student_Id = '" & StudentId & "' "
        Dim SeqNo As String = _DB.ExecuteScalar(sql)
        Return SeqNo
    End Function

    ''' <summary>
    ''' Function Update Sp_Seq , ApprovalStatus = 0 ใน tblStudentPhoto
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <param name="NewSeqNo">SeqNo อันใหม่ที่จะ update</param>
    ''' <returns>Integer:1,-1</returns>
    ''' <remarks></remarks>
    Private Function UpdateSpSeq(ByVal StudentId As String, ByVal NewSeqNo As Integer) As Integer
        Dim sql As String = " UPDATE dbo.tblStudentPhoto SET SP_Seq = '" & NewSeqNo & "',Approval_Status = 0,LastUpDate = dbo.GetThaiDate(),ClientId = NULL WHERE Student_Id = '" & StudentId & "' "
        Try
            _DB.Execute(sql)
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' ทำการ random ตัวเลข 4 หลักเพื่อนำไปตั้งเป็นชื่อรูป
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        'loop เพื่อต่อสตริงตัวเลขที่ random มาได้ (1-9) , เงื่อนไขการจบ loop วนจนครบ 4 รอบ
        For i = 1 To 4
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

    ''' <summary>
    ''' ทำการ Insert ข้อมูลรายการรูปภาพของนักเรียนเข้าไปใน DB
    ''' </summary>
    ''' <param name="StudentId">รหัสนักเรียน</param>
    ''' <returns>Integer:1,-1</returns>
    ''' <remarks></remarks>
    Private Function InsertDataInStudentPhoto(ByVal StudentId As String) As Integer
        Dim sql As String = " INSERT INTO dbo.tblStudentPhoto( StudentPhoto_Id ,SP_Seq ,Student_Id ,Received_Date ,Approval_Status ,Approve_By ,LastUpdate ,IsActive,IsFromParentTablet) " &
                            " VALUES  ( NEWID(), 0 ,'" & StudentId & "',dbo.GetThaiDate(),0 ,NULL,dbo.GetThaiDate(),1,0) "
        Try
            _DB.Execute(sql)
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' ทำการหาข้อมูลของนักเรียน รหัส รร. , รหัสนักเรียน จากรหัสเครื่อง Tablet
    ''' </summary>
    ''' <param name="DeviceId">รหัสเครื่อง Tablet</param>
    ''' <returns>Datatable</returns>
    ''' <remarks></remarks>
    Private Function GetStudentDataByDeviceId(ByVal DeviceId As String) As DataTable
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT dbo.t360_tblStudent.School_Code,Student_Id FROM dbo.t360_tblTablet INNER JOIN dbo.t360_tblTabletOwner " &
                            " ON dbo.t360_tblTablet.Tablet_Id = dbo.t360_tblTabletOwner.Tablet_Id " &
                            " INNER JOIN dbo.t360_tblStudent ON dbo.t360_tblTabletOwner.Owner_Id = t360_tblstudent.Student_Id " &
                            " WHERE dbo.t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "' AND dbo.t360_tblTabletOwner.TabletOwner_IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt
    End Function

    Private Function BypassApprovePhotoMaxOnet(StudentId As String) As Boolean
        Dim _DB As New ClassConnectSql()
        Dim sql As String = ""
        Dim UserId As String = StudentId
        'Update Database ว่าอนุมัติรุปนี้
        Try
            _DB.OpenWithTransection()
            sql = " UPDATE dbo.tblStudentPhoto SET Approval_Status = 1 , " &
                  " Approve_By = '" & UserId & "',LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE Student_Id = '" & StudentId & "'; "
            _DB.ExecuteWithTransection(sql)

            'Update ที่ t360_tblStudent
            sql = "update t360_tblstudent set Student_HasPhoto = '1',lastupdate = dbo.GetThaiDate(),ClientId = Null where Student_Id = '" & StudentId & "';"
            _DB.ExecuteWithTransection(sql)


            'Path ของรูปของนักเรียนคนนี้
            Dim StrPath As String = HttpContext.Current.Server.MapPath("../UserData/" & BusinessTablet360.ClsKNSession.DefaultSchoolCode & "/{" & StudentId & "}")
            'สตริงที่เป็นรูปแบบ วันที่/เวลา เพื่อจะนำไปเป็นส่วนนึงในชื่อรูป
            Dim StrDatetimeinfo As String = Date.Now.Year.ToString() & Date.Now.Month.ToString() & Date.Now.Day.ToString() & "-" & TimeOfDay.Hour.ToString() & "_" & TimeOfDay.Minute.ToString()
            'Path รูปเก่า
            Dim OldImg As String = ""
            'Path รูปใหม่
            Dim NewImg As String = ""
            'ต้องเช็คก่อนว่ามีไฟล์ Id.jpg,IdFullSize.jpg อยู่แล้วหรือเปล่า ถ้ามีอยู่แล้วต้อง Copy มาเป็นชื่อใหม่ที่ต้อด้วย วันที่ แล้วค่อยลบไฟล์ต้นฉบับทิ้ง
            If System.IO.File.Exists(StrPath & "/Id.jpg") = True Then
                'ทำ Id.jpg ก่อน
                OldImg = StrPath & "\Id.jpg"
                NewImg = StrPath & "\Id" & StrDatetimeinfo & ".jpg"
                'Copy รูปที่ upload ไว้ก่อนไปเป็นชื่อใหม่
                System.IO.File.Copy(OldImg, NewImg)
                System.IO.File.SetAttributes(OldImg, IO.FileAttributes.Normal)
                'ลบรูปเก่าทิ้งเลย
                System.IO.File.Delete(OldImg)

                'ทำ IdFullSize.Ext 
                Dim dr As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
                Dim aFile As System.IO.FileInfo() = dr.GetFiles("*IdFullSize*")
                If aFile.Length > 0 Then
                    Dim StrExtension As String = aFile(0).Extension
                    System.IO.File.Copy(StrPath & "\IdFullSize" & StrExtension, StrPath & "\Id_FullSize" & StrDatetimeinfo & StrExtension)
                    System.IO.File.SetAttributes(StrPath & "\IdFullSize" & StrExtension, IO.FileAttributes.Normal)
                    System.IO.File.Delete(StrPath & "\IdFullSize" & StrExtension)
                End If

            End If
            'Rename Id_tmp.jpg ให้เป็น Id.jpg
            System.IO.File.Copy(StrPath & "\Id_tmp.jpg", StrPath & "\Id.jpg")
            System.IO.File.SetAttributes(StrPath & "\Id_tmp.jpg", IO.FileAttributes.Normal)
            System.IO.File.Delete(StrPath & "\Id_tmp.jpg")

            'Rename IdFullSize_tmp.Ext ให้เป็น IdFullSize.Ext
            Dim drInfo As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(StrPath)
            Dim bFile As System.IO.FileInfo() = drInfo.GetFiles("*IdFullSize*")
            If bFile.Length > 0 Then
                Dim StrExtension As String = bFile(0).Extension
                System.IO.File.Copy(StrPath & "\IdFullSize_tmp" & StrExtension, StrPath & "\IdFullSize" & StrExtension)
                System.IO.File.SetAttributes(StrPath & "\IdFullSize_tmp" & StrExtension, IO.FileAttributes.Normal)
                System.IO.File.Delete(StrPath & "\IdFullSize_tmp" & StrExtension)
            End If

            _DB.CommitTransection()
            _DB = Nothing
            Return True

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            _DB = Nothing
            Return False
        End Try
    End Function

End Class