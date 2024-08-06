Imports System.Web
Imports KnowledgeUtils.Database
Imports System.Reflection
Imports System.Configuration
Imports System.Windows.Forms
Imports System.IO
Imports KnowledgeUtils.System
Imports KnowledgeUtils

'Public Interface IApplicationManager
'    Function GetSqlConnectionString() As String
'End Interface

'Public Class ApplicationManager
'    Implements IApplicationManager

'#Region "Web Application"

'    ''' <summary>
'    ''' ก็หน้าจะดีนะแต่รู้สึกไม่จำเป็นอีกหน่อยไม่ต้องมีก็ได้นะรู้สึก
'    ''' </summary>
'    ''' <param name="Name"></param>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Function GetConfigConnectionString(ByVal Name As String) As String
'        Try
'            ดึงมาจากที่(Web.config)
'            Return System.Configuration.ConfigurationManager.ConnectionStrings(Name).ConnectionString
'        Catch ex As Exception
'            Throw New Exception("Can not connect database " & Name)
'        End Try
'    End Function

'#End Region

'    Public Function GetSqlConnectionString() As String Implements IApplicationManager.GetSqlConnectionString
'        Return GetConfigConnectionString("Tablet360ConnectionString")
'    End Function

'End Class

'Public Class WindowsApplicationManager
'    Implements IApplicationManager

'    Public Function GetSqlConnectionString() As String Implements IApplicationManager.GetSqlConnectionString
'        Dim Xdoc As XDocument = XDocument.Load(IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Config.xml"))
'        Dim ConnectionString = Xdoc.Element("appconfig").Elements("Tablet360ConnectionString").Value
'        Return ConnectionString
'    End Function

'End Class

Public Class WebApplicationManager
    Implements IApplicationManager

#Region "Web Application"

    Private Function GetConfigConnectionString(ByVal Name As String) As String
        Try
#If F5 = "1" Then
            Return System.Configuration.ConfigurationManager.ConnectionStrings(Name).ConnectionString

#Else
            Return HttpContext.Current.Application("connectionstring")
#End If
            'Return System.Configuration.ConfigurationManager.ConnectionStrings(Name).ConnectionString
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception("Can not connect database " & Name)
        End Try
    End Function

#End Region

    Public Function GetConnectionString() As String Implements IApplicationManager.GetConnectionString
        Return GetConfigConnectionString("Tablet360ConnectionString")
    End Function

    Public Function GetProductVersion() As String
        Return Assembly.GetExecutingAssembly.GetName.Version.ToString
    End Function

    Public Shared Function GetIsMultiSchool() As Boolean
#If F5 = "1" Then
        Return System.Configuration.ConfigurationManager.AppSettings("IsMultiSchool")
#Else
         Return HttpContext.Current.Application("IsMultiSchool") 
#End If
    End Function

    Public Shared Function GetSchoolId() As String
#If F5 = "1" Then
        Return System.Configuration.ConfigurationManager.AppSettings("SchoolId")
#Else
         Return HttpContext.Current.Application("SchoolId") 
#End If
    End Function

    Public Shared Function GetEnableAutomatedTestSymbol() As Boolean
#If F5 = "1" Then
        Return System.Configuration.ConfigurationManager.AppSettings("EnableAutomatedTestSymbol")
#Else
         Return HttpContext.Current.Application("EnableAutomatedTestSymbol") 
#End If
    End Function

    Public Shared Function GetUseSync() As Boolean
#If F5 = "1" Then
        Return System.Configuration.ConfigurationManager.AppSettings("UseSync")
#Else
         Return HttpContext.Current.Application("UseSync") 
#End If
    End Function

    Public Shared Function GetPointPlusURL() As String
#If F5 = "1" Then
        Return System.Configuration.ConfigurationManager.AppSettings("PointPlusURL")
#Else
         Return HttpContext.Current.Application("PointPlusURL") 
#End If
    End Function

    Public Shared Function GetEnableUsersubjectclass() As String
        Return HttpContext.Current.Application("EnableUsersubjectclass")
    End Function

    Public Shared Function GetDateSchedulerUplevel() As Date?
        If HttpContext.Current.Application("DateSchedulerUplevel") Is Nothing Then
            Return Nothing
        Else
            Return HttpContext.Current.Application("DateSchedulerUplevel")
        End If
    End Function

    Public Shared Function GetConfigSetClass() As ClassDto()
        Dim Tmp As New List(Of ClassDto)
        Dim Config = GetEnableUsersubjectclass()

        If Config <> "" Then
            Dim All = Config.Split(",")
            For Each Row In All
                Dim ClassSubject = Row.Split("-")
                If Tmp.Where(Function(q) q.Level_Dummy = ClassSubject("1").ToString).Count = 0 Then
                    Dim LevelMap = Service.ClsSystem.GetLevelMapbyValue(, , ClassSubject("1"))
                    If LevelMap IsNot Nothing Then
                        Tmp.Add(New ClassDto With {.Level_Dummy = ClassSubject("1"), .Level_Name = LevelMap.Level_Name})
                    End If
                End If
            Next
        End If
        Return Tmp.ToArray
    End Function

    Public Shared Function GetConfigSetClassSubject() As UserSubjectClassDTO()
        Dim Tmp As New List(Of UserSubjectClassDTO)
        Dim Config = GetEnableUsersubjectclass()

        If Config <> "" Then
            Dim All = Config.Split(",")
            For Each Row In All
                Dim ClassSubject = Row.Split("-")
                Dim SubjectMap = Service.ClsSystem.GetSubjectMapbyValue(, , ClassSubject("0"))
                Dim LevelMap = Service.ClsSystem.GetLevelMapbyValue(, , ClassSubject("1"))
                If LevelMap IsNot Nothing Then
                    Tmp.Add(New UserSubjectClassDTO With {.Level_Dummy = LevelMap.Level_Dummy,
                                                     .SubjectNameDummy = SubjectMap.SubjectNameDummy,
                                                     .GroupSubjectShortName = SubjectMap.GroupSubjectShortName
                                                      })
                End If
            Next
        End If
        Return Tmp.ToArray
    End Function

    Public Shared Function GetKnConfigDataStringt360() As String
        Dim langFileName As String = ClsLanguage.GetFileNameOfCurrentSite(EnLangType.T360)
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

    Public Shared Function GetKnConfigDataStringQuickTest() As String
        Dim langFileName As String = ClsLanguage.GetFileNameOfCurrentSiteForQuickTest(EnLangType.Quick)
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

    Public Shared Function LoadConfig() As Boolean
        Try
            Dim FnDecryptData = Function(encString As String) As String
                                    Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
                                    Dim initVector As String = "$@1Fc3K1eTr0g9H8"

                                    Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

                                    Return rijndaelKey.Decrypt(encString)
                                End Function

            'Load ค่า config ที่อยู่ใน file t360
            Dim encryptedData As String = GetKnConfigDataStringt360()
            If encryptedData <> "" Then
                If Not IsNothing(encryptedData) Then
                    Dim decryptedData As String = FnDecryptData(encryptedData).ToLower()
                    Dim configDataArray As String() = decryptedData.Split("|")
                    If UBound(configDataArray) > 0 Then
                        For i As Integer = 0 To UBound(configDataArray)
                            Dim eachConfigurationItem As String() = configDataArray(i).Split("*")
                            'If UBound(eachConfigurationItem) = 1 Then
                            HttpContext.Current.Application(eachConfigurationItem(0)) = eachConfigurationItem(1)
                            'End If
                        Next
                    End If

                End If


                'Load ค่า config ชั้น วิชา
                Dim encryptedDataQuick = GetKnConfigDataStringQuickTest()
                HttpContext.Current.Application("enableusersubjectclass") = GetConfigFromQuick(EnTypeConfigQuick.enableusersubjectclass)
            End If

            Return True
        Catch ex As Exception
            'ElmahExtension.LogToElmah(ex)
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Shared Function GetConfigFromQuick(TypeConfig As EnTypeConfigQuick) As String
        Dim FnDecryptData = Function(encString As String) As String
                                Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
                                Dim initVector As String = "$@1Fc3K1eTr0g9H8"

                                Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

                                Return rijndaelKey.Decrypt(encString)
                            End Function

        Dim encryptedDataQuick = GetKnConfigDataStringQuickTest()
        If encryptedDataQuick <> "" Then
            If Not IsNothing(encryptedDataQuick) Then
                Dim decryptedData As String = FnDecryptData(encryptedDataQuick).ToLower()
                Dim configDataArray As String() = decryptedData.Split("|")
                If UBound(configDataArray) > 0 Then
                    For i As Integer = 0 To UBound(configDataArray)
                        Dim eachConfigurationItem As String() = configDataArray(i).Split("*")
                        If eachConfigurationItem(0) = TypeConfig.GetEnumText(Of EnumTypeConfigQuick)() Then
                            Return eachConfigurationItem(1)
                        End If
                    Next
                End If
            End If
        End If
        Return ""
    End Function

End Class

Public Enum EnTypeConfigQuick
    enableusersubjectclass
End Enum

Public Class EnumTypeConfigQuick
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnTypeConfigQuick.enableusersubjectclass, "enableusersubjectclass")
    End Sub

End Class

Public Class WindowsApplicationManager
    Implements IApplicationManager

    Public Function GetConnectionString() As String Implements KnowledgeUtils.Database.IApplicationManager.GetConnectionString
        Return ConfigurationManager.AppSettings("ConnectionString")
    End Function

End Class

Public Class WindowsApplicationCheckNetworkManager
    Implements IApplicationManager

    Public Function GetConnectionString() As String Implements KnowledgeUtils.Database.IApplicationManager.GetConnectionString
        Return GetKnConfigDataConnectionStringt360()
        'Dim Xdoc As XDocument = XDocument.Load(IO.Path.Combine(Application.StartupPath, "Config.xml"))
        'Dim r = Xdoc.Element("appconfig").Elements("Tablet360ConnectionString").Value
        'Return r
    End Function

    Public Function GetKnConfigDataConnectionStringt360() As String
        Dim FnDecryptData = Function(encString As String) As String
                                Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
                                Dim initVector As String = "$@1Fc3K1eTr0g9H8"

                                Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

                                Return rijndaelKey.Decrypt(encString)
                            End Function

        'Load ค่า config ที่อยู่ใน file t360
        Dim encryptedData As String = GetKnConfigDataStringt360()
        If encryptedData <> "" Then
            If Not IsNothing(encryptedData) Then
                Dim decryptedData As String = FnDecryptData(encryptedData).ToLower()
                Dim configDataArray As String() = decryptedData.Split("|")
                If UBound(configDataArray) > 0 Then
                    For i As Integer = 0 To UBound(configDataArray)
                        Dim eachConfigurationItem As String() = configDataArray(i).Split("*")
                        If eachConfigurationItem(0).ToUpper = "ConnectionString".ToUpper Then
                            Return eachConfigurationItem(1)
                        End If
                    Next
                End If
            End If
        End If
        Return ""
    End Function

    Public Function GetKnConfigDataStringt360() As String
        Dim langFileName As String = ClsLanguage.GetFileNameOfCurrentSite(EnLangType.T360)
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

Public Class WindowsApplicationUpLoadDataManager
    Implements IApplicationManager

    Public Function GetConnectionString() As String Implements KnowledgeUtils.Database.IApplicationManager.GetConnectionString
#If F5 = "1" Then
        Return ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
#Else
       Return GetKnConfigDataConnectionStringt360()
#End If
    End Function

    Public Function GetKnConfigDataConnectionStringt360() As String
        Dim FnDecryptData = Function(encString As String) As String
                                Dim passPhrase As String = "=bor'LNditlboT6N"        ' can be any string  สำคัญ, ต้องตรงกัน ไม่งั้นถอดไม่ออก
                                Dim initVector As String = "$@1Fc3K1eTr0g9H8"

                                Dim rijndaelKey As RijndaelEnhanced = New RijndaelEnhanced(passPhrase, initVector)

                                Return rijndaelKey.Decrypt(encString)
                            End Function

        'Load ค่า config ที่อยู่ใน file t360
        Dim encryptedData As String = GetKnConfigDataStringt360()
        If encryptedData <> "" Then
            If Not IsNothing(encryptedData) Then
                Dim decryptedData As String = FnDecryptData(encryptedData).ToLower()
                Dim configDataArray As String() = decryptedData.Split("|")
                If UBound(configDataArray) > 0 Then
                    For i As Integer = 0 To UBound(configDataArray)
                        Dim eachConfigurationItem As String() = configDataArray(i).Split("*")
                        If eachConfigurationItem(0).ToUpper = "ConnectionString".ToUpper Then
                            Return eachConfigurationItem(1)
                        End If
                    Next
                End If
            End If
        End If
        Return ""
    End Function

    Public Function GetKnConfigDataStringt360() As String
        Dim langFileName As String = ClsLanguage.GetFileNameOfCurrentSite(EnLangType.T360)
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

Public Class WebApplicationQuickTestManager
    Implements IApplicationManager

#Region "Web Application"

    Private Function GetConfigConnectionString(ByVal Name As String) As String
        Try
            Return System.Configuration.ConfigurationManager.ConnectionStrings(Name).ConnectionString
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception("Can not connect database " & Name)
        End Try
    End Function

#End Region

    Public Function GetConnectionString() As String Implements IApplicationManager.GetConnectionString
        'todo แก้ของจิงให้ดึงจากไฟล์ bin
        'Dim KnClsConfigData As New KNConfigData
        'Dim EncodeConnectionString As String = GetConfigConnectionString("ConnectionString")
#If F5 = "1" Then
        Return "Data Source=10.100.1.90;Initial Catalog=Pointplus_Present;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"

#Else
        'Return KnClsConfigData.DecryptData(EncodeConnectionString)
        Return ClsLanguage.GetConStr()
#End If

    End Function

    Public Function GetProductVersion() As String
        Return Assembly.GetExecutingAssembly.GetName.Version.ToString
    End Function

End Class

