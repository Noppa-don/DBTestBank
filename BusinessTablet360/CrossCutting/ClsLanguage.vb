Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Web.Compilation
Imports System.Web
Imports System.Windows.Forms

Public Class ClsLanguage

    Public Shared TypeClsLanguage As EnLangType = EnLangType.Quick

    Public Shared ReadOnly Property FileNameLanguage() As String
        Get
            If TypeClsLanguage = EnLangType.Quick Then
                Return "langset.bin"
            Else
                Return "fontset.bin"
            End If
        End Get
    End Property

    ''' <summary>
    ''' ทำการสร้างไฟล์ langset.bin โดยมี pattern config + ' ' + fingerprint + ' ' + key + ' ' + connectionstring
    ''' </summary>
    ''' <param name="newLang">ข้อความที่จะเอาไปเขียน File langset.bin</param>
    ''' <param name="TypeApp">QuickTest,T360</param>
    ''' <returns>Boolean:True=สำเร็จ,False=ไม่สำเร็จ</returns>
    ''' <remarks></remarks>
    Public Shared Function SwitchLang(newLang As String, Optional TypeApp As EnLangType = EnLangType.Quick) As Boolean
        TypeClsLanguage = TypeApp
        Dim retVal As Boolean = False

        Call CheckDDataPathExists()

        Dim langPathOfCurrentSite As String = GetPathNameOfCurrentSite()

        If Directory.Exists(langPathOfCurrentSite) = False Then
            Try
                Directory.CreateDirectory(langPathOfCurrentSite)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                'ElmahExtension.LogToElmah(ex)
                Throw New DirectoryNotFoundException("Cannot create '" + langPathOfCurrentSite + "' folder!", ex)
            End Try
        End If

        Dim langFileNameOfCurrentSite As String = GetFileNameOfCurrentSite(TypeClsLanguage)

        If File.Exists(langFileNameOfCurrentSite) = True Then
            Try
                File.Delete(langFileNameOfCurrentSite)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                'ElmahExtension.LogToElmah(ex)
                Throw New IOException("Cannot delete old '" + langFileNameOfCurrentSite + "'!", ex)
            End Try
        End If

        'เริ่ม save จริง
        Try
            Using sr As New StreamWriter(langFileNameOfCurrentSite)
                sr.WriteLine(newLang)
                sr.Close()
            End Using
            retVal = True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            'ElmahExtension.LogToElmah(ex)
            Throw New IOException("Cannot save to newLang file '" + langFileNameOfCurrentSite + "'!", ex)
        End Try

        Return retVal
    End Function

    Public Shared Function Chklang(Optional TypeApp As EnLangType = EnLangType.Quick) As Boolean
        'Dim ClsKnConfigData As New KNConfigData()
        TypeClsLanguage = TypeApp
        Try
            Dim FingerPrint As String = ""
            Dim KNConfigDataStr As String = ""
            GetLang(FingerPrint, KNConfigDataStr)

            'ณ บรรทัดนี้ ต้องเอาให้ต้น ดึงไปเปลี่ยนแทนที่เจ้า AppSetting.KNConfigData ด้วย
            If FingerPrint.Trim() <> "" Then
                Dim DecryptFingerPrint As String = KNConfigData.DecryptData(FingerPrint)
                If DecryptFingerPrint.Trim() <> "" Then
                    Dim GetFingerPrint As String = GenerateMachineIdentification()
                    If GetFingerPrint.ToLower().Trim() = DecryptFingerPrint.ToLower().Trim() Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            '  ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Shared Function GetFileNameOfCurrentSite(Optional TypeApp As EnLangType = EnLangType.Quick) As String
        'default กรณีรันในเครื่องตัวเอง แบบกด F5 รัน, "d:\data\langset.bin"
        TypeClsLanguage = TypeApp
        GetFileNameOfCurrentSite = GetPathNameOfCurrentSite() & FileNameLanguage
    End Function

    Public Shared Function GetFileNameOfCurrentSiteForQuickTest(Optional TypeApp As EnLangType = EnLangType.Quick) As String
        'default กรณีรันในเครื่องตัวเอง แบบกด F5 รัน, "d:\data\langset.bin"
        TypeClsLanguage = TypeApp
        Dim appnamepath = System.Configuration.ConfigurationManager.AppSettings("FolderQuickTest")
        Return GetBasePath() & FileNameLanguage
    End Function
    Public Shared Function GetBasePath() As String

        If KnowledgeUtils.Azure.IsRunInAzure() Then
            Return HttpContext.Current.Server.MapPath("~/App_Data/")
        End If
        Return "d:\data\DBTestbank\"
        'Return "c:\data\"
    End Function
    Private Shared Function GetPathNameOfCurrentSite() As String
        'default กรณีรันในเครื่องตัวเอง แบบกด F5 รัน, "d:\data\langset.bin"

        'Dim path As String = AppDomain.CurrentDomain.FriendlyName
        'path = path.Substring(path.LastIndexOf("/"))
        'path = path.Substring(0, path.IndexOf("-")).Replace("/", "")
        'Dim Path = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location)
        'Dim Path = System.IO.Path.GetFileNameWithoutExtension(BuildManager.GetGlobalAsaxType().BaseType.Assembly.Location)

        Dim basePath As String = GetBasePath()


        If TypeClsLanguage = EnLangType.Quick Then
            'ถ้าเป็น quick เอาชื่อ root มาทำเป็น folder
            Dim appnamepath = HttpContext.Current.Request.ApplicationPath.Replace("/", "")
            If appnamepath = "" Then
                Return basePath
            Else
                Return basePath & appnamepath & "\"
            End If

        Else
            'ถ้าเป็น t360 เอาจาก config ของ t360
            If System.Configuration.ConfigurationManager.AppSettings("FolderT360") = "" Then
                Dim Xdoc As XDocument = XDocument.Load(IO.Path.Combine(Application.StartupPath, "Config.xml"))
                Dim r = Xdoc.Element("appconfig").Elements("FolderT360").Value
                Return basePath & r & "\"
            Else
                Return basePath & System.Configuration.ConfigurationManager.AppSettings("FolderT360") & "\"
            End If
        End If
    End Function

    Public Shared Function GetLang(ByRef langID As String, Optional ByRef KNConfigData As String = Nothing, Optional ByVal IsGetConnString As Boolean = False, Optional ByVal IsGetConfigStr As Boolean = False, Optional ByVal GetAllFile As Boolean = False) As String
        Dim langFileName As String = GetFileNameOfCurrentSite(TypeClsLanguage)
        If File.Exists(langFileName) = True Then
            Try

                Using sr As New StreamReader(langFileName)
                    Dim line As String
                    line = sr.ReadToEnd()
                    sr.Close()
                    If GetAllFile = True Then
                        Return line
                    End If
                    Dim SpliteStr = line.Split(" ")
                    If SpliteStr.Count = 4 Then
                        If KNConfigData IsNot Nothing Then KNConfigData = SpliteStr(0).ToString()
                        If IsGetConnString = True Then
                            langID = SpliteStr(3).ToString()
                        ElseIf IsGetConfigStr = True Then
                            langID = SpliteStr(0).ToString()
                        Else
                            langID = SpliteStr(1).ToString()
                        End If
                    Else
                        If KNConfigData IsNot Nothing Then KNConfigData = ""
                        langID = ""
                    End If
                End Using
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ' ElmahExtension.LogToElmah(ex)
                If KNConfigData IsNot Nothing Then KNConfigData = ""
                langID = ""
            End Try
        Else
            If KNConfigData IsNot Nothing Then KNConfigData = ""
            langID = ""
        End If
        Return langID
    End Function

    Public Shared Function GetKeyIdFromFile()
        Dim langFileName As String = GetFileNameOfCurrentSite()
        If File.Exists(langFileName) = True Then
            Try
                Using sr As New StreamReader(langFileName)
                    Dim KeyId As String = ""
                    Dim line As String
                    line = sr.ReadToEnd()
                    sr.Close()
                    Dim SpliteStr = line.Split(" ")
                    If SpliteStr.Count = 4 Then
                        KeyId = SpliteStr(2).ToString()
                    Else
                        KeyId = ""
                    End If
                    Return KeyId
                End Using
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ' ElmahExtension.LogToElmah(ex)
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function

    Public Shared Function GetLangID() As String
        Dim langID As String = ""
        Return GetLang(langID)
    End Function

    Public Shared Function GetLangIDDec() As String
        Return KNConfigData.DecryptData(GetLangID())
    End Function

    Public Shared Function GetConfigStr() As String
        Dim langID As String = ""
        Return GetLang(langID, , , True)
    End Function

    Public Shared Function GetConStr(Optional NoDecryptData As Boolean = False) As String
        Dim ConStr As String = ""
        If NoDecryptData = False Then
            Return KNConfigData.DecryptData(GetLang(ConStr, , True))
        Else
            Return GetLang(ConStr, , True)
        End If
    End Function

    Public Shared Function GetKeyId() As String
        Return GetKeyIdFromFile()
    End Function

    Public Shared Function GetKeyIdDec() As String
        Return KNConfigData.DecryptData(GetKeyId())
    End Function


    ''' <summary>
    ''' หารหัสเครื่องโดยใช้ Hardware CPU,Mainboard
    ''' หรือ กรณี รันใน Azure จะทำการหา instance_Id มาใช้งานแทน
    ''' </summary>
    ''' <param name="TypeApp">โปรแกรมที่จะทำการ Activate quicktest,t360</param>
    ''' <returns>รหัสเครื่อง</returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateMachineIdentification(Optional TypeApp As EnLangType = EnLangType.Quick) As String
        'Return "740B-4111-78BF-0BDD-886F-F2D9-FC75-9340"
        Dim sb As New StringBuilder()

        If KnowledgeUtils.Azure.IsRunInAzure() Then
            sb.AppendLine(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID "))

        Else 'รันใน Windows ปกติ


            'constants
            'Dim check As String(,) = New String(,) {{"Win32_NetworkAdapterConfiguration", "MACAddress"}, {"Win32_Processor", "UniqueId"}, {"Win32_Processor", "ProcessorId"}, {"Win32_Processor", "Name"}, {"Win32_Processor", "Manufacturer"}, {"Win32_BIOS", "Manufacturer"}, _
            '    {"Win32_BIOS", "SMBIOSBIOSVersion"}, {"Win32_BIOS", "IdentificationCode"}, {"Win32_BIOS", "SerialNumber"}, {"Win32_BIOS", "ReleaseDate"}, {"Win32_BIOS", "Version"}, {"Win32_DiskDrive", "Model"}, _
            '    {"Win32_DiskDrive", "Manufacturer"}, {"Win32_DiskDrive", "Signature"}, {"Win32_DiskDrive", "TotalHeads"}, {"Win32_BaseBoard", "Model"}, {"Win32_BaseBoard", "Manufacturer"}, {"Win32_BaseBoard", "Name"}, _
            '    {"Win32_BaseBoard", "SerialNumber"}, {"Win32_VideoController", "DriverVersion"}, {"Win32_VideoController", "Name"}}

            TypeClsLanguage = TypeApp
            'Dim check As String(,) = New String(,) {{"Win32_Processor", "UniqueId"}, {"Win32_Processor", "ProcessorId"}, {"Win32_Processor", "Name"}, {"Win32_Processor", "Manufacturer"}, {"Win32_BIOS", "Manufacturer"}, _
            '    {"Win32_BIOS", "SerialNumber"}, {"Win32_BIOS", "ReleaseDate"}, {"Win32_DiskDrive", "Model"}, {"Win32_DiskDrive", "Manufacturer"}, {"Win32_DiskDrive", "Signature"}, {"Win32_DiskDrive", "TotalHeads"}, _
            '    {"Win32_BaseBoard", "Model"}, {"Win32_BaseBoard", "Manufacturer"}, {"Win32_BaseBoard", "Name"}, {"Win32_BaseBoard", "SerialNumber"}}

            Dim check As String(,) = New String(,) {{"Win32_Processor", "UniqueId"}, {"Win32_Processor", "ProcessorId"}, {"Win32_Processor", "Name"}, {"Win32_Processor", "Manufacturer"}, {"Win32_BIOS", "Manufacturer"},
                {"Win32_BIOS", "SerialNumber"}, {"Win32_BIOS", "ReleaseDate"}, {"Win32_BaseBoard", "Model"}, {"Win32_BaseBoard", "Manufacturer"}, {"Win32_BaseBoard", "Name"}, {"Win32_BaseBoard", "SerialNumber"}}

            'WMI query
            Dim query As String = "SELECT {1} FROM {0}" ', queryex As String = " WHERE IPEnabled = 'True'"
            Dim result As String = Nothing

            For i As Integer = 0 To check.GetLength(0) - 1
                'Dim oWMI As New System.Management.ManagementObjectSearcher(String.Format(query, check(i, 0), check(i, 1)) & (If(i = 0, queryex, String.Empty)))
                Dim oWMI As New System.Management.ManagementObjectSearcher(String.Format(query, check(i, 0), check(i, 1)))
                For Each mo As System.Management.ManagementObject In oWMI.[Get]()
                    result = TryCast(mo(check(i, 1)), String)
                    If result IsNot Nothing Then
                        sb.AppendLine(result)
                        Debug.Print(result)
                    End If
                    Exit For
                Next
            Next


        End If

        'Hashing & format
        Dim sec As MD5 = New MD5CryptoServiceProvider()
        Dim enc As New ASCIIEncoding()
        Dim bt As Byte() = enc.GetBytes(sb.ToString())
        bt = sec.ComputeHash(bt)
        sb.Clear()
        For i As Integer = 0 To bt.Length - 1
            If i > 0 AndAlso i Mod 2 = 0 Then
                sb.Append("-"c)
            End If
            sb.AppendFormat("{0:X2}", bt(i))
        Next


        Return sb.ToString()
    End Function
    Private Shared Sub CheckDDataPathExists()
        'ถ้ารันใน azure ไม่ต้องเช็ค d:\data เพราะให้เก็บข้อมูลใน App_Data แทน
        If Not KnowledgeUtils.Azure.IsRunInAzure() Then
            If Directory.Exists("d:\") = False Then
                Throw New DriveNotFoundException("D:\ drive not exists!")
            End If
            If Directory.Exists("d:\data\") = False Then
                Try
                    Directory.CreateDirectory("d:\data\")
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    'ElmahExtension.LogToElmah(ex)
                    Throw New DirectoryNotFoundException("Cannot create D:\Data\ folder!", ex)
                End Try
            End If
        End If

    End Sub
    Public Shared Sub CheckTextFileDBIsExistAlready()
        Call CheckDDataPathExists()

        Dim langPathOfCurrentSite As String = GetPathNameOfCurrentSite() & "db.bin"
        If File.Exists(langPathOfCurrentSite) = False Then
            Try
                'เริ่ม save จริง
                Using sr As New StreamWriter(langPathOfCurrentSite)
                    sr.WriteLine("1")
                    sr.Close()
                End Using
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ' ElmahExtension.LogToElmah(ex)
                Throw New IOException("Cannot delete old '" + langPathOfCurrentSite + "'!", ex)
            End Try
        End If
    End Sub

    Public Shared Function ReadFileData() As Integer
        Dim ReturnValue As Integer = 0
        Dim FilePath As String = GetPathNameOfCurrentSite() & "db.bin"
        If File.Exists(FilePath) = False Then
            Throw New DriveNotFoundException("db.bin file not exists!")
        Else
            Using sr As New StreamReader(FilePath)
                ReturnValue = sr.ReadLine()
                sr.Close()
            End Using
        End If
        Return ReturnValue
    End Function

    Public Shared Sub WriteFileData(ByVal txtData As String, Optional ByVal ConfigConnStr As Boolean = False)
        Call CheckDDataPathExists()

        Dim langPathOfCurrentSite As String = ""
        If ConfigConnStr = False Then
            langPathOfCurrentSite = GetPathNameOfCurrentSite() & "db.bin"
        Else
            langPathOfCurrentSite = GetPathNameOfCurrentSite() & "langset.bin"
        End If
        If File.Exists(langPathOfCurrentSite) = True Then
            Try
                Using sr As New StreamWriter(langPathOfCurrentSite)
                    sr.WriteLine(txtData)
                    sr.Close()
                End Using
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ' ElmahExtension.LogToElmah(ex)
                Throw New IOException("Cannot Write File '" + langPathOfCurrentSite + "'!", ex)
            End Try
        End If
    End Sub

End Class

Public Enum EnLangType
    Quick
    T360
    QStep1
    QStep2
    QSurf1
    Qsurf2
    QRead
    QDo
    QPict
    QTest
End Enum

