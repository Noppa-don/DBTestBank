Imports GdPicture10
Imports System.IO
Imports System.Drawing


Public Class QRReaderPage
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' ทำการอ่านค่า QRCode ออกมาเป็น JsonString โดยอ่านทั้งแบบสตริงที่เป็น Base64 และ ไฟล์รูปภาพที่เลือกมา
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ClsSecurity.CheckConnectionIsSecure()
        ClsLog.Record("QRReaderPage = Page_Load")
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            'สตริง Base64 ในกรณีที่ส่งเป็นสตริงเข้ามา
            Dim StrBase64 As String = Request.Form("Base64FileData")

            ClsLog.Record("methodname = " & methodName)
            ClsLog.Record("StrBase64 = " & StrBase64)
            ClsLog.Record("????")
            If Not IsNothing(methodName) Then
                If methodName.ToLower() = "qrreade" Then
                    'ทำการ Register STROKESCRIBE โปรแกรมที่อ่าน QR-Code ก่อน
                    Dim oLicenseManager As New GdPicture10.LicenseManager
                    oLicenseManager.RegisterKEY("198459578734376851114112016468913")
                    oLicenseManager.RegisterKEY("788894759717106801114143528522197")
                    oLicenseManager.RegisterKEY("904333449703058531111171221964670")
                    oLicenseManager.RegisterKEY("808828486749199831117143694591464")

                    'ถ้าเป็นการส่งข้อมูลแบบ Base64 มาต้องเข้า If
                    If StrBase64 <> "" Then
                        ClsLog.Record("StrBase64 <> """)
                        StrBase64 = StrBase64.Replace(" ", "+")
                        'ไป Process ในแบบ Base64
                        ProcessBase64String(StrBase64)
                    Else
                        'ถ้าเป็นการส่งข้อมูลแบบ FileUpload
                        Dim UploadFile As HttpFileCollection = Request.Files
                        If UploadFile.Keys.Count > 0 Then
                            'ไป Process ในแบบไฟล์รูปภาพ
                            ProcessPostedFile(UploadFile)
                        Else
                            Response.Write(-1)
                            Response.End()
                        End If
                    End If
                End If
            Else
                Response.Write(-1)
                Response.End()
            End If
        End If
    End Sub

    ''' <summary>
    ''' ทำการหา JsonString จากรูปภาพที่เลือกมา
    ''' </summary>
    ''' <param name="UploadFile">ไฟล์รูปภาพที่เลือกมา</param>
    ''' <remarks></remarks>
    Private Sub ProcessPostedFile(ByVal UploadFile As HttpFileCollection)
        'loop เพื่อหารูปทั้งหมดที่เลือกมา(ควรจะมีแค่รูปเดียว) เพื่อนำไปหา JsonStirng , เงื่อนไขการจบ loop คือ วนจนครบทุก Item 
        For Each r As String In UploadFile.AllKeys
            'ถ้ามีข้อมูล
            If (UploadFile(r).ContentLength > 0) Then
                'UploadFile(r).SaveAs(Server.MapPath("../DroidPad/temp/" & UploadFile(r).FileName))

                'ทำการ save รูปลง tmp ไว้ก่อน
                UploadFile(r).SaveAs("D:\data\tmp\DroidPad\QrReader\" & UploadFile(r).FileName)
                'สร้างตัวแปรที่จะเอามาอ่าน QR-Code
                Dim GdPic As New GdPicture10.GdPictureImaging
                'Dim QrImage As Integer = GdPic.CreateGdPictureImageFromFile(Server.MapPath("../DroidPad/temp/" & UploadFile(r).FileName))
                Dim QrImage As Integer = GdPic.CreateGdPictureImageFromFile("D:\data\tmp\DroidPad\QrReader\" & UploadFile(r).FileName)
                If QrImage > 0 Then
                    GdPic.BarcodeQRReaderDoScan(QrImage)
                    GdPic.ReleaseGdPictureImage(QrImage)
                    'ทำการอ่านค่า JsonStirng จากรูปภาพ
                    Dim Jsondata As String = GdPic.BarcodeQRReaderGetBarcodeValue(1)
                    GdPic.BarcodeQRReaderClear()
                    'Dim tempFile As New FileInfo(Server.MapPath("../DroidPad/temp/" & UploadFile(r).FileName))
                    Dim tempFile As New FileInfo("D:\data\tmp\DroidPad\QrReader\" & UploadFile(r).FileName)
                    If tempFile.Exists Then
                        Try
                            'ทำการลบรูปทิ้งเมื่ออ่านข้อมูลเสร็จเรียบร้อยแล้ว
                            'File.SetAttributes(Server.MapPath("../DroidPad/temp/" & UploadFile(r).FileName), FileAttributes.Normal)
                            File.SetAttributes("D:\data\tmp\DroidPad\QrReader\" & UploadFile(r).FileName, FileAttributes.Normal)
                            tempFile.Delete()
                        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                            Response.Write(Jsondata)
                            Response.End()
                        End Try
                    End If
                    Response.Write(Jsondata)
                    Response.End()
                Else
                    Response.Write(-2)
                    Response.End()
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' ทำการหา JsonString จากสตริง Base64
    ''' </summary>
    ''' <param name="StrBase64">สตริง Base64 ที่ต้องการแปลงเป็น JsonString</param>
    ''' <remarks></remarks>
    Private Sub ProcessBase64String(ByVal StrBase64 As String)
        'ทำการแปลงสตริง Base64 ให้เป็น Byte ก่อน
        Dim Imagebyte() As Byte = Convert.FromBase64String(StrBase64)
        Dim ms As MemoryStream = New MemoryStream(Imagebyte)
        ms.Write(Imagebyte, 0, Imagebyte.Length)
        'ทำการสร้างรูปจาก Base64
        Dim Image As Image = Image.FromStream(ms)
        'นำเลขที่ random มาได้มาตั้งเป็นชื่อรูป
        Dim FileName As String = GetRndSeqNumber().ToString()

        'Do Until File.Exists(Server.MapPath("../DroidPad/temp/" & FileName & ".png")) = False
        '    FileName = GetRndSeqNumber().ToString()
        'Loop

        'ทำการเช็คก่อนว่าชื่อรูปที่ Random มาได้มันซ้ำหรือยัง ถ้าซ้ำแล้วต้องไป Random มาใหม่ , เงื่อนไขการจบ loop คือ ชื่อต้องไม่ซ้ำ
        Do Until File.Exists("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png") = False
            FileName = GetRndSeqNumber().ToString()
        Loop
        'Image.Save(Server.MapPath("../DroidPad/temp/" & FileName & ".png"))
        'ทำการ save รูปลงไปที่ tmp ก่อน
        Image.Save("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
        ClsLog.Record("After UpLoad Pic")
        'ตัวแปรที่จะนำไปอ่าน QR-Code
        Dim GdPic As New GdPicture10.GdPictureImaging
        'Dim QrImage As Integer = GdPic.CreateGdPictureImageFromFile(Server.MapPath("../DroidPad/temp/" & FileName & ".png"))
        Dim QrImage As Integer = GdPic.CreateGdPictureImageFromFile("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
        If QrImage > 0 Then
            GdPic.BarcodeQRReaderDoScan(QrImage)
            GdPic.ReleaseGdPictureImage(QrImage)
            'ทำการหา JsonStirng จากการอ่าน QR-Code
            Dim Jsondata As String = GdPic.BarcodeQRReaderGetBarcodeValue(1)
            ClsLog.Record(" " + Jsondata)
            GdPic.BarcodeQRReaderClear()
            'Dim tempFile As New FileInfo(Server.MapPath("../DroidPad/temp/" & FileName & ".png"))
            Dim tempFile As New FileInfo("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
            If tempFile.Exists Then
                'ทำการลบรูปทิ้งหลังจากอ่านค่าได้เรียบร้อยแล้ว
                Try
                    'File.SetAttributes(Server.MapPath("../DroidPad/temp/" & FileName & ".png"), FileAttributes.Normal)
                    File.SetAttributes("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png", FileAttributes.Normal)
                    tempFile.Delete()
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                    Response.Write(Jsondata)
                    Response.End()
                End Try
            End If
            Response.Write(Jsondata)
            Response.End()
        Else
            Response.Write(-2)
            Response.End()
        End If
    End Sub

    ''' <summary>
    ''' ทำการสุ่มเลข 5 หลักเพื่อนำไปตั้งเป็นชื่อรูป
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        'loop เพื่อทำการสุ่มเลข 1-9 เพื่อนำไปต่อสตริงให้เป็นชื่อรูปภาพ , เงื่อนไขการจบ loop คือ วน 5 รอบ
        For i = 1 To 5
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

End Class