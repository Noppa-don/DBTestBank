Imports System.DirectoryServices
Imports BusinessTablet360

Namespace ActiveDirectory

    ''' <summary>
    ''' Class ช่วยเกี่ยวกับเช็คสิทธิ์ที่อยู่ใน Active Directory ที่เซ็ตไว้ที่เครื่อง Server
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageActiveDirectory

        'ตัวแปรกลุ่มเก็บไว้เผื่อใช้
        Dim _entry As New DirectoryEntry
        Dim _path As String
        Dim _pathGen As String
        Dim _user As String
        Dim _pwd As String

        Dim _info As New Dictionary(Of String, String)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="path">ตัวอย่าง Path Ldap "LDAP://192.168.0.200", "LDAP://ppg.cementhai.com/DC=ppg,DC=cementhai,DC=com"</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal path As String)
            _path = path
        End Sub

        ''' <summary>
        ''' ฟังชั่นเช็คสิทธิ์ที่อยู่ใน Active Directory ที่เซ็ตไว้ที่เครื่อง Server
        ''' </summary>
        ''' <param name="domain">ชื่อโดเมนเช่น bz, ppg ถ้าไม่มีใส่อะไรก็ได้</param>
        ''' <param name="username">username</param>
        ''' <param name="pwd">password</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String) As Boolean

            Dim domainAndUsername As String = domain & "\" & username
            Dim entry As DirectoryEntry = New DirectoryEntry(_path, domainAndUsername, pwd)

            Try
                'Bind to the native AdsObject to force authentication.			
                Dim obj As Object = entry.NativeObject
                Dim search As DirectorySearcher = New DirectorySearcher(entry)

                search.Filter = "(SAMAccountName=" & username & ")"
                search.PropertiesToLoad.Add("memberOf")
                search.PropertiesToLoad.Add("mail")
                search.PropertiesToLoad.Add("name")
                Dim result As SearchResult = search.FindOne()

                If (result Is Nothing) Then
                    Return False
                End If

                'Update the new path to the user in the directory.
                _pathGen = result.Path
                _user = username
                _pwd = pwd

                'Add Info To Dictionary
                Dim dn As String
                Dim propertyCount As Integer
                Dim equalsIndex, commaIndex

                propertyCount = result.Properties("memberOf").Count
                For propertyCounter As Integer = 0 To 0 'บางทีอยู่หลายเมมเบอร์อยากได้เมมเบอร์ของตัวเองเลยเดาว่าหน้าจะเป็นอินเด็กที่ 0
                    dn = CType(result.Properties("memberOf")(propertyCounter), String)

                    equalsIndex = dn.IndexOf("=", 1)
                    commaIndex = dn.IndexOf(",", 1)
                    If (equalsIndex = -1) Then
                        Exit For
                    End If

                    dn = dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1)
                    _info.Add("Group", dn)
                Next
                propertyCount = result.Properties("mail").Count
                For propertyCounter As Integer = 0 To propertyCount - 1
                    dn = CType(result.Properties("mail")(propertyCounter), String)

                    If (dn = "") Then
                        Exit For
                    End If

                    _info.Add("mail", dn)
                Next
                propertyCount = result.Properties("name").Count
                For propertyCounter As Integer = 0 To propertyCount - 1
                    dn = CType(result.Properties("name")(propertyCounter), String)

                    If (dn = "") Then
                        Exit For
                    End If

                    _info.Add("FullName", dn)
                Next

            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Throw New Exception("Error authenticating user. " & ex.Message)
            End Try

            Return True
        End Function

        ''' <summary>
        ''' ฟั่งชั่นให้ค่า Info ที่อยู่ในรูปแบบ Dictionary
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetInfo() As Dictionary(Of String, String)
            Return _info
        End Function

        ''' <summary>
        ''' ฟั่งชั่นให้ค่า Info โดยมีการรับเงื่อนไข Key ที่ต้องการ
        ''' </summary>
        ''' <param name="Key">เช่น Group, FullName, Mail</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetInfoByKey(ByVal Key As String) As String
            Dim Result As String = ""
            If _info.ContainsKey(Key) Then
                Result = _info(Key)
            End If
            Return Result
        End Function

    End Class

End Namespace

