﻿Imports GdPicture10
Imports System.IO
Imports System.Drawing
Imports ZXing
Public Class LoginPageQR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <Services.WebMethod()>
    Public Shared Function CheckUser(ByVal StrBase64 As String) As String
        Dim JsonData As String
        StrBase64 = StrBase64.Replace(" ", "+")
        StrBase64 = StrBase64.Replace("data:image/png;base64,", "")
        StrBase64 = StrBase64.Replace("data:image/jpeg;base64,", "")
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
        ClsLog.Log("After UpLoad Pic")
        'ตัวแปรที่จะนำไปอ่าน QR-Code

        Dim readCode As New ZXing.QrCode.QRCodeReader()
        Dim bitMap As New Bitmap("D:\data\tmp\DroidPad\QrReader\" & FileName & ".png")
        bitMap = Convert1bpp(bitMap)

        Dim RbitMap = ResizeImage(bitMap, 239, 217)

        RbitMap.Save("D:\data\tmp\DroidPad\QrReader\" & FileName & "AfterResize.png", Imaging.ImageFormat.Png)

        Dim lumianaceSource = New ZXing.BitmapLuminanceSource(RbitMap)
        Dim binarizer = New ZXing.Common.HybridBinarizer(lumianaceSource)

        Dim mapa As New ZXing.BinaryBitmap(binarizer)

        Dim reader = New MultiFormatReader()

        Dim Result = reader.decode(mapa)


        JsonData = Result.Text

        Return JsonData
    End Function

    ''' <summary>
    ''' ทำการสุ่มเลข 5 หลักเพื่อนำไปตั้งเป็นชื่อรูป
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Shared Function GetRndSeqNumber() As Integer
        Dim TxtSeqNo As String = ""
        Dim r As Random = New Random()
        'loop เพื่อทำการสุ่มเลข 1-9 เพื่อนำไปต่อสตริงให้เป็นชื่อรูปภาพ , เงื่อนไขการจบ loop คือ วน 5 รอบ
        For i = 1 To 5
            TxtSeqNo &= r.Next(1, 9)
        Next
        Dim NewRandomSeq As Integer = CInt(TxtSeqNo)
        Return NewRandomSeq
    End Function

    Private Shared Function Convert1bpp(Img As Bitmap) As Bitmap

        For i As Integer = 0 To Img.Width - 1
            For j As Integer = 0 To Img.Height - 1
                Dim col As Color = Img.GetPixel(i, j)
                Dim gray As Integer = (CInt(col.R) + CInt(col.G) + CInt(col.B)) / 3
                Img.SetPixel(i, j, Color.FromArgb(gray, gray, gray))
            Next
        Next
        Return Img
    End Function

    Private Shared Function ResizeImage(originalImage As Bitmap, maxWidth As Integer, maxHeight As Integer) As Bitmap
        'Dim originalImage As New Bitmap(streamImage)
        Dim newWidth As Integer = originalImage.Width
        Dim newHeight As Integer = originalImage.Height
        Dim aspectRatio As Double = CDbl(originalImage.Width) / CDbl(originalImage.Height)
        If aspectRatio <= 1 AndAlso originalImage.Width > maxWidth Then
            newWidth = maxWidth
            newHeight = CInt(Math.Round(newWidth / aspectRatio))
        ElseIf aspectRatio > 1 AndAlso originalImage.Height > maxHeight Then
            newHeight = maxHeight
            newWidth = CInt(Math.Round(newHeight * aspectRatio))
        End If
        Dim newImage As New Bitmap(originalImage, newWidth, newHeight)
        Dim g As Graphics = Graphics.FromImage(newImage)
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
        g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height)
        originalImage.Dispose()
        Return newImage
    End Function



End Class