Imports System.Net
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Web.UI
Imports BusinessTablet360

Namespace Web

    ''' <summary>
    ''' คลาสช่วยจัดการเกี่ยวกับ Web Page เช่นการ Post
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageWeb

#Region "Property"

        Private _Url As Uri
        Public Property Url() As Uri
            Get
                Return _Url
            End Get
            Set(ByVal value As Uri)
                _Url = value
            End Set
        End Property

        Private _MsgFromServer As String
        Public Property MsgFromServer() As String
            Get
                Return _MsgFromServer
            End Get
            Set(ByVal value As String)
                _MsgFromServer = value
            End Set
        End Property

#End Region

        Sub New()
        End Sub

        Sub New(UrlRequest As String)
            Url = New Uri(UrlRequest)
        End Sub

        ''' <summary>
        ''' ฟังชั่น Check ว่าสามารถต่อ Internet ได้หรือเปล่า
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckConnectInternet() As Boolean
            Dim Url = New Uri("http://www.google.com/")
            Dim request As WebRequest = WebRequest.Create(Url)
            Dim response As WebResponse
            Try
                response = request.GetResponse()
                response.Close()
                request = Nothing
                Return True
            Catch ex As Exception
                response = Nothing
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' ส่งข้อมูลไปที่ Url นั้นๆ ถ้าไม่สามารถส่งได้ หรือ ไม่ได้ระบุ url ไว้จะคืนค่า False
        ''' </summary>
        ''' <param name="Data">ข้อมูลที่จะส่ง</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PostData(Data As String) As Boolean
            Try
                If Url IsNot Nothing Then
                    'กำหนด Request
                    Dim Request As WebRequest = WebRequest.Create(Url)
                    Request.Method = "POST"
                    'เก็บข้อมูลในรูปแบบ Binary
                    Dim ByteArray As Byte() = Text.Encoding.UTF8.GetBytes(Data)
                    Request.ContentType = "application/x-www-form-urlencoded"
                    Request.ContentLength = ByteArray.Length
                    'เก็บ Binary ใส่ Stream
                    Dim DataStream As Stream = Request.GetRequestStream()
                    DataStream.Write(ByteArray, 0, ByteArray.Length)
                    DataStream.Close()
                    'ยิงไปเครื่อง Server
                    Dim Response As WebResponse = Request.GetResponse()
                    If CType(Response, HttpWebResponse).StatusDescription = "OK" Then
                        'เมื่อ Server Response กลับ
                        DataStream = Response.GetResponseStream()
                        Dim reader As New StreamReader(DataStream)
                        Dim responseFromServer As String = reader.ReadToEnd()
                        MsgFromServer = responseFromServer
                        reader.Close()
                    End If
                    DataStream.Close()
                    Response.Close()
                    Request = Nothing
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

    End Class

    Public Module ModuleManageWeb

        ''' <summary>
        ''' ใช้โชว์ Control ใน Page ว่ามีอะไรบ้างโดยแสดงเป็น Lv
        ''' </summary>
        ''' <param name="Page">Page จะโชว์ข้อมูล</param>
        ''' <param name="Controls">Control หลักเช่นถ้าส่ง Page มาก็จะหมายถึง Control ใน Page ทั้งหมด</param>
        ''' <param name="Depth">ระดับความลึก Lv ถ้าไม่ใส่อะไรหมายถึงโชว์หมด</param>
        ''' <remarks></remarks>
        <Extension()> Public Sub ShowAllControl(ByVal Page As Page, ByVal Controls As ControlCollection, Optional ByVal Depth As Integer = 0)
            For Each C As Control In Controls
                Page.Response.Write(New String("-", Depth * 4) & ">")
                Page.Response.Write(C.GetType().ToString() + " - <b>" + C.ID + "</b>" + " - " + C.ClientID + "<br />")
                If Not (C.Controls Is Nothing) Then
                    Page.ShowAllControl(C.Controls, Depth + 1)
                End If
            Next
        End Sub

        ''' <summary>
        ''' หา Control ไม่ว่าจะลึกขนาดไหน ถ้าไม่เจอจะคืนค่า Nothing
        ''' </summary>
        ''' <param name="Root">Control หลักเช่นถ้าส่ง Page มาก็จะหา Control ที่อยู่ใน Page นั้น</param>
        ''' <param name="ControlId">ชื่อ Control ไม่ใช่ ClientID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> Public Function FindDeepControl(ByVal Root As Control, ByVal ControlId As String) As Control
            Dim FoundControl As Control = Nothing
            For Each CurrentControl As Control In Root.Controls
                If (CurrentControl.ID = ControlId) Then
                    FoundControl = CurrentControl
                    Exit For
                End If
                If (CurrentControl.HasControls) Then
                    FoundControl = FindDeepControl(CurrentControl, ControlId)
                End If
            Next
            Return FoundControl
        End Function

    End Module

End Namespace


