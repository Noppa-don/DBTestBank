Imports System.Web
Imports System.Configuration

<Serializable()> _
Public Class UserConfig

    '<<< ใช้เทส
    'Public School_Code As String = "1000001"
    'Public User_Id As Guid? = New Guid("c334aa41-5026-41a8-8b05-7d238bc74f84")
    'Public User_Name As String = "test"
    'Public User_FirstName As String = "ฐิติพงษ์"
    'Public User_LastName As String = "หาญวงศ์ไพบุลย์"
    'Public CurrentCalendar As Guid? = New Guid("184d8614-1121-4e67-9e7d-02db59ba771c")
    'Public GroupSubjectId As Guid? = New Guid("E7EDF837-4A6A-4E69-A62D-158F26A2BB7D")
    'Public OrderBy(5) As Object
    'Public Where(5) As Object
    '<<< ใช้เทส

    Public School_Code As String '= "1000001"
    Public User_Id As Guid? '= New Guid("c334aa41-5026-41a8-8b05-7d238bc74f84")
    Public User_Name As String ' = "test"
    Public GUID As Guid? 'เก็บฟิว GUID ของ tblUser
    Public User_FirstName As String '= "ฐิติพงษ์"
    Public User_LastName As String '= "หาญวงศ์ไพบุลย์"
    Public CurrentCalendar As Guid? '= New Guid("184d8614-1121-4e67-9e7d-02db59ba771c")
    Public CalendarName As String
    Public CalendarYear As String
    Public GroupSubjectId As Guid? '= New Guid("E7EDF837-4A6A-4E69-A62D-158F26A2BB7D")
    Public MobileAccessType As Integer
    Public UseSync As Boolean?
    Public OrderBy(5) As Object
    Public Where(5) As Object

    Public Sub New()

    End Sub
    
End Class

Public Interface IUserConfigManager

    Function GetCurrentContext() As UserConfig
    Sub AddSchoolCode(ByVal Code As String)
    Sub AddUserId(ByVal User_Id As Guid)
    Sub AddUserName(ByVal User_Name As String)
    Sub AddCurrentCalendar(ByVal Id As Guid)
    Sub SetCurrentContext(ByVal Item As UserConfig)
    Function GetVirtualPathTempFile() As String
    Function GetPhysicalPathTempFile() As String

End Interface

Public Class UserConfigManager
    Implements IUserConfigManager

    'Public Sub New(ByVal webContext As HttpContext)
    '    MyWebContext = webContext
    'End Sub

    Private _webContext As System.Web.HttpContext
    Public Property MyWebContext() As System.Web.HttpContext
        Get
            Return _webContext
        End Get
        Set(ByVal value As System.Web.HttpContext)
            _webContext = value
        End Set
    End Property

    Public Sub AddSchoolCode(ByVal Code As String) Implements IUserConfigManager.AddSchoolCode
        Dim Item As UserConfig = GetCurrentContext()
        With Item
            .School_Code = Code
        End With
        SetCurrentContext(Item)
    End Sub

    Public Sub AddUserId(ByVal User_Id As Guid) Implements IUserConfigManager.AddUserId
        Dim Item As UserConfig = GetCurrentContext()
        With Item
            .User_Id = User_Id
        End With
        SetCurrentContext(Item)
    End Sub

    Public Sub AddUserName(ByVal User_Name As String) Implements IUserConfigManager.AddUserName
        Dim Item As UserConfig = GetCurrentContext()
        With Item
            .User_Name = User_Name
        End With
        SetCurrentContext(Item)
    End Sub

    Public Sub AddCurrentCalendar(Id As Guid) Implements IUserConfigManager.AddCurrentCalendar
        Dim Item As UserConfig = GetCurrentContext()
        With Item
            .CurrentCalendar = Id
        End With
        SetCurrentContext(Item)
    End Sub

    Public Sub SetCurrentContext(ByVal Item As UserConfig) Implements IUserConfigManager.SetCurrentContext
        'MyWebContext.Session("UserConText") = Item
        HttpContext.Current.Session("UserConText") = Item
    End Sub

    Public Function GetCurrentContext() As UserConfig Implements IUserConfigManager.GetCurrentContext
        If HttpContext.Current.Session("UserConText") Is Nothing Then
            'Todo ใช้เทสไปก่อน
            'WebContext.Session("UserConText") = New UserConfig
            Return New UserConfig
            'Return New UserConfig With {.School_Code = "1000001", .User_Id = New Guid("c334aa41-5026-41a8-8b05-7d238bc74f84"), .User_Name = "test", .User_FirstName = "ฐิติพงษ์", .User_LastName = "หาญวงศ์ไพบุลย์", .CurrentCalendar = Guid.NewGuid}
        Else
            'Todo ใช้เทสไปก่อน
            Return HttpContext.Current.Session("UserConText")
            'Return New UserConfig With {.School_Code = "1000001", .User_Id = New Guid("c334aa41-5026-41a8-8b05-7d238bc74f84"), .User_Name = "test", .User_FirstName = "ฐิติพงษ์", .User_LastName = "หาญวงศ์ไพบุลย์", .CurrentCalendar = Guid.NewGuid}
        End If
    End Function

    Public Function GetVirtualPathTempFile() As String Implements IUserConfigManager.GetVirtualPathTempFile
        Return "~/TempFile"
    End Function

    Public Function GetPhysicalPathTempFile() As String Implements IUserConfigManager.GetPhysicalPathTempFile
        Return HttpContext.Current.Server.MapPath("~/TempFile/")
    End Function

End Class

Public Class UserConfigApplicationManager
    Implements IUserConfigManager


    'Dim ItemUserConfig As UserConfig
    'Sub New(ByVal Item As UserConfig)
    '    ItemUserConfig = Item
    'End Sub

    Public Sub AddSchoolCode(ByVal Code As String) Implements IUserConfigManager.AddSchoolCode

    End Sub

    Public Sub AddUserId(ByVal User_Id As Guid) Implements IUserConfigManager.AddUserId

    End Sub


    Public Function GetCurrentContext() As UserConfig Implements IUserConfigManager.GetCurrentContext
        'Return ItemUserConfig
    End Function

    Public Function GetPhysicalPathTempFile() As String Implements IUserConfigManager.GetPhysicalPathTempFile

    End Function

    Public Function GetVirtualPathTempFile() As String Implements IUserConfigManager.GetVirtualPathTempFile

    End Function

    Public Sub SetCurrentContext(ByVal Item As UserConfig) Implements IUserConfigManager.SetCurrentContext

    End Sub

    Public Sub AddUserName(ByVal User_Name As String) Implements IUserConfigManager.AddUserName

    End Sub

    Public Sub AddCurrentCalendar(Id As Guid) Implements IUserConfigManager.AddCurrentCalendar

    End Sub
End Class