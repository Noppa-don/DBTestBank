Imports System.Net
Imports System.IO
Imports System.Web
Imports BusinessTablet360

Public Class ManageNet

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

#End Region

    Sub New()

    End Sub

    Sub New(UrlRequest As String)
        Url = New Uri(UrlRequest)
    End Sub

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

    Public Function PostData(Data As String) As Boolean
        Try
            'กำหนด Request
            Dim request As WebRequest = WebRequest.Create(Url)
            request.Method = "POST"
            'เก็บข้อมูลให้อยู่ในรูปแบบ Binary
            Dim byteArray As Byte() = Text.Encoding.UTF8.GetBytes(Data)
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length
            'เก็บ Binary ใส่ Stream
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            'ยิงไปเครื่อง Server
            Dim response As WebResponse = request.GetResponse()
            If CType(response, HttpWebResponse).StatusDescription = "OK" Then
                'เมื่อ server response กลับ
                dataStream = response.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                Dim responseFromServer As String = reader.ReadToEnd()
                Debug.WriteLine(responseFromServer)
                reader.Close()
            End If
            dataStream.Close()
            response.Close()
            request = Nothing
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function PostDataAndGetResponse(DataCollection As Specialized.NameValueCollection, Optional IsReturnByte As Boolean = False)
        Try
            Using Client As New Net.WebClient
                Dim responsebytes = Client.UploadValues(Url, "POST", DataCollection) 'ถ้าใช้จริงน่าจะต้องแก้ URL เพราะมันจะไม่เป็น localhost แล้ว
                If IsReturnByte Then
                    Return responsebytes
                End If
                Return (New Text.UTF8Encoding).GetString(responsebytes)
            End Using
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "-1"
        End Try
    End Function

    Public Function GetNameValueCollectionByHashtable(DataHashtable As Hashtable) As Specialized.NameValueCollection
        If DataHashtable IsNot Nothing Then
            Dim newNameValueCollection As New Specialized.NameValueCollection
            For Each eachData As DictionaryEntry In DataHashtable
                newNameValueCollection.Add(eachData.Key, eachData.Value)
            Next
            Return newNameValueCollection
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function GetRequestIPAddress() As String

        Dim context As HttpContext = HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function
End Class
