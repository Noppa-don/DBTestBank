Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Web

Public Class ShowImgHelpPage
    Inherits System.Web.UI.Page
    'เก็บชื่อ Folder ของรูปภาพ ใช้ทางฝั่ง Javascript ด้วย
    Public FolderName As String
    'เก็บชื่อของหน้าจอ ใช้ทางฝั่ง Javascript ด้วย
    Public PageName As String
    'ตัวแปรที่เช็คว่าเป็นรูปสุดท้ายหรือเปล่า เพื่อใช้ เปลี่ยน state ของปุ่ม ถัดไป ให้เป็น ปุ่มจบ ใช้ทางฝั่ง Javascript ด้วย
    Public IsLastImage As String
    'ตัวแปรที่บอกว่าตอนนี้อยู่ Mode อะไร เช่น Full,StandAlone,TabletLab ใช้ทางฝั่ง Javascript ด้วย
    Public RunMode As String
    'Public appnamepath As String

    ''' <summary>
    ''' ทำการนำ querystring ชื่อ folder,ชื่อหน้า ต่างๆมาต่อกันให้เป็น Path รูป โดยรูปจะเก็บอยู่ตาม Pattern นี้ คือ 
    ''' /HowTo/helpImg/ชื่อโหมด/ชื่อFolder_ชื่อหน้า00.png
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If HttpContext.Current.Request.QueryString("FolderName") IsNot Nothing And HttpContext.Current.Request.QueryString("PageName") IsNot Nothing And ClsKNSession.RunMode <> "" Then
                FolderName = HttpContext.Current.Request.QueryString("FolderName")
                PageName = HttpContext.Current.Request.QueryString("PageName")
                RunMode = ClsKNSession.RunMode

                'ตัวแปรนี้จะถูกทำให้แทนค่ารูปถัดไปเพื่อนำไปเช็คว่า มีรูปถัดไปหรือเปล่า หรือว่ามีรูปเดียวเลย ถ้ามีรูปเดียวจะได้ให้ปุ่ม ถัดไปแสดงเป็นปุ่มจบเลย
                Dim SecondImg As String = ""

                'appnamepath = HttpContext.Current.Request.ApplicationPath
                'If appnamepath.Trim() = "/" Then
                '    appnamepath = ""
                'End If

                'If RunMode.ToLower() = "standalonenotablet" Then
                '    SecondImg = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/Howto/helpImg/" & RunMode & "/" & FolderName & "_" & PageName & "01.png")
                'Else
                SecondImg = HttpContext.Current.Server.MapPath("./Howto/helpImg/" & RunMode & "/" & FolderName & "_" & PageName & "01.png")
                'End If
                'เช็คว่า Img นี้มีรูปเดียวหรือเปล่า
                If File.Exists(SecondImg) = True Then
                    IsLastImage = "False"
                Else
                    IsLastImage = "True"
                End If
                'เป็นตัวแปรทีให้ JavaScript รู้ว่าต้องแสดงรูปแรก เพราะเป็นการโหลดครั้งแรก
                'ClientScript.RegisterHiddenField("ImgHdNo", "0")
            End If
        End If

    End Sub

    ''' <summary>
    ''' เมื่อกดปุ่มถัดไปทางฝั่ง Javascript จะ POST มาทำการ Get Path ของรูปต่อไป
    ''' </summary>
    ''' <param name="FolderName">ชื่อ-Folder</param>
    ''' <param name="PageName">ชื่อหน้า</param>
    ''' <param name="ImgNo">ลำดับของรูปปัจจุบัน</param>
    ''' <returns>String:{Path-ของรูปถัดไป,ตัวแปรที่บอกว่ารูปถัดไปเนี่ยเป็นรูปสุดท้ายหรือเปล่า}</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GetNextImgHelpSrc(ByVal FolderName As String, ByVal PageName As String, ByVal ImgNo As Integer) As String
        'ตัวแปร object ที่จะเอาไว้ทำ JsonString เพื่อส่งค่ากลับไปทาง Javascript
        Dim js As New JavaScriptSerializer()
        'Path ของรูปต่อไปที่เราจะคืนค่าไป
        Dim ImgNoString1 As String = ""
        'Path ของรูปถัด + ถัดไป เพื่อที่จะเช็คว่าจะยังมีรูปถัดไปอีกรึเปล่า หรือว่าเป็นรูปสุดท้ายแล้ว จะได้ assign ค่าตัวแปร IsLastImg ต่อไป
        Dim ImgNoString2 As String = ""
        ImgNo += 1
        If ImgNo < 10 Then
            ImgNoString1 = "0" & ImgNo.ToString()
        Else
            ImgNoString1 = ImgNo.ToString()
        End If
        'ตัวแปรที่ไว้ใช้เช็คว่ารูปถัดไปมีหรือเปล่า
        Dim NextImg As String = ""
        'Dim appnamepath = HttpContext.Current.Request.ApplicationPath
        'If appnamepath.Trim() = "/" Then
        '    appnamepath = ""
        'End If
        'If HttpContext.Current.Application("RunMode").ToString().Trim().ToLower() = "standalonenotablet" Then
        '    NextImg = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/Howto/helpImg/" & HttpContext.Current.Application("RunMode").ToString().Trim() & "/" & FolderName & "_" & PageName & ImgNoString1 & ".png")
        'Else
        NextImg = HttpContext.Current.Server.MapPath("./Howto/helpImg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & ImgNoString1 & ".png")
        'End If
        'HttpContext.Current.Server.MapPath("Howto/helpImg/" & HttpContext.Current.Application("RunMode").ToString().Trim() & "/" & FolderName & "_" & PageName & ImgNoString1 & ".png")
        'ตัวแปร Json String ที่จะ Return ค่ากลับไป
        Dim JsonString
        'เช็คว่ารูปถัดไปมีหรือเปล่า
        If File.Exists(NextImg) = True Then
            'เช็คอีกว่ารูปนี้เป็นรูปสุดท้ายหรือยัง
            ImgNo += 1
            If ImgNo < 10 Then
                ImgNoString2 = "0" & ImgNo.ToString()
            Else
                ImgNoString2 = ImgNo.ToString()
            End If
            Dim CheckLastImg As String = ""
            'If HttpContext.Current.Application("RunMode").ToString().Trim().ToLower() = "standalonenotablet" Then
            '    CheckLastImg = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/Howto/helpImg/" & HttpContext.Current.Application("RunMode").ToString().Trim() & "/" & FolderName & "_" & PageName & ImgNoString2 & ".png")
            'Else
            CheckLastImg = HttpContext.Current.Server.MapPath("./Howto/helpImg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & ImgNoString2 & ".png")
            'End If
            If File.Exists(CheckLastImg) = True Then 'ถ้ายังไม่ใช่รูปสุดท้าย
                JsonString = New With {.SrcImg = ImgNoString1, .IsLastImg = "False"}
            Else 'ถ้าเป็นรูปสุดท้ายแล้ว
                JsonString = New With {.SrcImg = ImgNoString1, .IsLastImg = "True"}
            End If
        Else
            'ไม่มีรูปถัดไป
            JsonString = New With {.SrcImg = "", .IsLastImg = "False"}
        End If
        Return js.Serialize(JsonString)
    End Function

    ''' <summary>
    ''' เมื่อกดปุ่มถอยกลับ จะต้องมาหา Path ของรูปก่อนหน้าด้วย
    ''' </summary>
    ''' <param name="FolderName">ชื่อ-Folder</param>
    ''' <param name="PageName">ชื่อหน้า</param>
    ''' <param name="ImgNo">ลำดับของรูปปัจจุบัน</param>
    ''' <returns>String:{Path-ของรูปก่อนหน้า,ตัวแปรที่บอกว่ารูปถัดไปเนี่ยเป็นรูปสุดท้ายหรือเปล่า}</returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function GetBackImgHelpSrc(ByVal FolderName As String, ByVal PageName As String, ByVal ImgNo As Integer) As String
        'ตัวแปร object ที่จะเอาไว้ทำ JsonString เพื่อส่งค่ากลับไปทาง Javascript
        Dim js As New JavaScriptSerializer()
        'Path ของรูปต่อไปที่เราจะคืนค่าไป
        Dim ImgNoString1 As String = ""
        'Path ของรูปถัด + ถัดไป เพื่อที่จะเช็คว่าจะยังมีรูปถัดไปอีกรึเปล่า หรือว่าเป็นรูปสุดท้ายแล้ว จะได้ assign ค่าตัวแปร IsLastImg ต่อไป
        Dim ImgNoString2 As String = ""
        ImgNo -= 1
        If ImgNo < 10 Then
            ImgNoString1 = "0" & ImgNo.ToString()
        Else
            ImgNoString1 = ImgNo.ToString()
        End If
        Dim NextImg As String = ""
        'If HttpContext.Current.Application("RunMode").ToString().Trim().ToLower() = "standalonenotablet" Then
        '    NextImg = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/Howto/helpImg/" & HttpContext.Current.Application("RunMode").ToString().Trim() & "/" & FolderName & "_" & PageName & ImgNoString1 & ".png")
        'Else
        'Dim appnamepath = HttpContext.Current.Request.ApplicationPath
        'If appnamepath.Trim() = "/" Then
        '    appnamepath = ""
        'End If
        NextImg = HttpContext.Current.Server.MapPath("./Howto/helpImg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & ImgNoString1 & ".png")
        'End If
        'ตัวแปร Json String ที่จะ Return ค่ากลับไป
        Dim JsonString
        If File.Exists(NextImg) = True Then
            'เช็คอีกว่ารูปนี้เป็นรูปสุดท้ายหรือยัง
            ImgNo -= 1
            If ImgNo = -1 Then
                JsonString = New With {.SrcImg = ImgNoString1, .IsLastImg = "True"}
                Return js.Serialize(JsonString)
            ElseIf ImgNo < 10 Then
                ImgNoString2 = "0" & ImgNo.ToString()
            Else
                ImgNoString2 = ImgNo.ToString()
            End If
            Dim CheckLastImg As String = ""
            'If HttpContext.Current.Application("RunMode").ToString().Trim().ToLower() = "standalonenotablet" Then
            '    CheckLastImg = HttpContext.Current.Server.MapPath("/quicktest_test_standalone/Howto/helpImg/" & HttpContext.Current.Application("RunMode").ToString().Trim() & "/" & FolderName & "_" & PageName & ImgNoString2 & ".png")
            'Else
            CheckLastImg = HttpContext.Current.Server.MapPath("./Howto/helpImg/" & ClsKNSession.RunMode & "/" & FolderName & "_" & PageName & ImgNoString2 & ".png")
            'End If

            If File.Exists(CheckLastImg) = True Then 'ถ้ายังไม่ใช่รูปสุดท้าย
                JsonString = New With {.SrcImg = ImgNoString1, .IsLastImg = "False"}
            Else 'ถ้าเป็นรูปสุดท้ายแล้ว
                JsonString = New With {.SrcImg = ImgNoString1, .IsLastImg = "True"}
            End If
        Else
            JsonString = New With {.SrcImg = "", .IsLastImg = "False"}
        End If
        Return js.Serialize(JsonString)
    End Function

End Class