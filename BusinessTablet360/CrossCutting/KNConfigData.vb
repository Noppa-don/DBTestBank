Option Strict Off

Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports KnowledgeUtils

<Serializable()> _
Public Class KNConfigData
    Public Sub New()

    End Sub

    Private Shared isWorkingAsNobelWorld As Boolean = False 'ค่า default จะเป็น NobelOne = ใช้งานได้จำกัด
    Private Shared appStorageBasePath As String = "d:\storage\" 'ค่า default จะเป็น d:\storage  

    Private Shared hasDataLoaded As Boolean = False
    Private Shared Sub KNConfigData()

    End Sub
    Public Shared Function DecryptData(encString As String) As String
        Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
        Dim initVector As String = "$@1Fc3K1eTr0g9H8"

        Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

        Return rijndaelKey.Decrypt(encString)
    End Function

    ''' <summary>
    ''' ฟังก์ชั่นนี้ ต้อง ลบ ก่อนจะเอาไปขายจริงจัง, และต้องย้ายไปอยู่ในหน้า KNSuperAdmin ด้วย 
    ''' </summary>
    ''' <param name="stringToEnc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function KnEncryption(stringToEnc As String) As String
        Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
        Dim initVector As String = "$@1Fc3K1eTr0g9H8" '

        Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

        Return rijndaelKey.Encrypt(stringToEnc)
    End Function
    Public Shared Function ConvertToBoolean(keyname As String, value As String) As String
        Dim retVal As Boolean = False
        Try
            retVal = Convert.ToBoolean(value)
        Catch e As FormatException
            ElmahExtension.LogToElmah(e)
            'Log.Record("Unable to convert app.config '" & keyname & "' to a Boolean. DataToConvert = " & value)
        End Try
        ConvertToBoolean = retVal
    End Function
    Public ReadOnly Property IsNobelWorld As Boolean
        Get
            If Not hasDataLoaded Then LoadData()
            Return isWorkingAsNobelWorld
        End Get
    End Property
    Public ReadOnly Property StorageBasePath As String
        Get
            If Not hasDataLoaded Then LoadData()
            Return appStorageBasePath
        End Get
    End Property
    Public Shared Sub AppendF5DebugSession(ByVal messageText As String)
        'System.Web.HttpContext.Current.Session("F5Debug") = System.Web.HttpContext.Current.Session("F5Debug") + vbNewLine + messageText + " at Time: " + Now.ToShortTimeString()
    End Sub
    Public Shared Sub LoadData()
#If F5 = "1" Then
        AppendF5DebugSession("KNConfigData Entering LoadData())")
#End If
        If System.Web.HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            'อ่านค่า redis host จาก web.config มาถือไว้
            System.Web.HttpContext.Current.Application("RedisConnectionString") = ConfigurationManager.ConnectionStrings("RedisConnectionString").ConnectionString
            Dim KnApplication As New ClsKNSession()
            Dim encryptedData As String = GetKnConfigDataString()

            If encryptedData <> "" Then
                If Not IsNothing(encryptedData) Then
                    Dim decryptedData As String = DecryptData(encryptedData).ToLower()
                    Dim configDataArray As String() = decryptedData.Split("|")
                    If UBound(configDataArray) > 0 Then
                        For i As Integer = 0 To UBound(configDataArray)

                            Dim eachConfigurationItem As String() = configDataArray(i).Split("*")

                            If UBound(eachConfigurationItem) = 1 Then
                                KnApplication(eachConfigurationItem(0)) = eachConfigurationItem(1)

                            End If
                        Next
                    End If

                End If
                hasDataLoaded = True
            End If
        End If
    End Sub

    '    Public Shared Sub LoadData()
    '#If F5 = "1" Then
    '        AppendF5DebugSession("KNConfigData Entering LoadData())")
    '#End If
    '        If System.Web.HttpContext.Current.Application("NeedEditButton") Is Nothing Then
    '            'อ่านค่า redis host จาก web.config มาถือไว้
    '            System.Web.HttpContext.Current.Application("RedisConnectionString") = ConfigurationManager.ConnectionStrings("RedisConnectionString").ConnectionString

    '            'isWorkingAsNobelWorld = False
    '            Dim KnApplication As New ClsKNSession()
    '            'Dim encryptedData As String = ConfigurationManager.AppSettings.Get("KNConfig")
    '            Dim encryptedData As String = GetKnConfigDataString()

    '            '#If F5 = "1" Then
    '            '        AppendF5DebugSession("KNConfigData After Call GetKnConfigDataString()")
    '            '#End If

    '            If encryptedData <> "" Then
    '                If Not IsNothing(encryptedData) Then

    '                    Dim decryptedData As String = DecryptData(encryptedData).ToLower()
    '                    '#If F5 = "1" Then
    '                    '                AppendF5DebugSession("KNConfigData After Call DecryptData(encryptedData).ToLower()")
    '                    '#End If
    '                    Dim configDataArray As String() = decryptedData.Split("|")
    '                    '#If F5 = "1" Then
    '                    '                AppendF5DebugSession("KNConfigData After Call decryptedData.Split()")
    '                    '#End If
    '                    If UBound(configDataArray) > 0 Then
    '                        For i As Integer = 0 To UBound(configDataArray)

    '                            Dim eachConfigurationItem As String() = configDataArray(i).Split("*")
    '                            '#If F5 = "1" Then
    '                            '                        AppendF5DebugSession("KNConfigData After Call configDataArray(" + i.ToString + ").Split()")
    '                            '#End If
    '                            If UBound(eachConfigurationItem) = 1 Then
    '                                KnApplication(eachConfigurationItem(0)) = eachConfigurationItem(1)
    '                                '#If F5 = "1" Then
    '                                '                            AppendF5DebugSession("KNConfigData After Call KnApplication(eachConfigurationItem(0)) = eachConfigurationItem(1) ")
    '                                '#End If
    '                            End If
    '                        Next
    '                    End If

    '                End If
    '                hasDataLoaded = True
    '                '#If F5 = "1" Then
    '                '            AppendF5DebugSession("KNConfigData Leaving LoadData() at Time: " + Now.ToShortTimeString())
    '                '#End If

    '            End If
    '        End If
    '    End Sub

    Public Shared Function GetKnConfigDataString() As String
        Dim langFileName As String = ClsLanguage.GetFileNameOfCurrentSite
        If File.Exists(langFileName) = True Then
            Try
                Dim KnConfigData As String = ""
                Using sr As New StreamReader(langFileName)
                    Dim line As String
                    line = sr.ReadToEnd()
                    Dim SpliteStr = line.Split(" ")
                    If SpliteStr.Count = 4 Then
                        KnConfigData = SpliteStr(0).ToString()
                    Else
                        Return ""
                    End If
                End Using
                Return KnConfigData
            Catch
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function

End Class

