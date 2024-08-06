Imports System.Web.UI.WebControls

Public Class MenuProperty
    Inherits List(Of MenuProperty)

    Private _TextDisplay As String
    Public Property TextDisplay() As String
        Get
            Return _TextDisplay
        End Get
        Set(ByVal value As String)
            _TextDisplay = value
        End Set
    End Property

    Private _PageUrl As String
    Public Property PageUrl() As String
        Get
            Return _PageUrl
        End Get
        Set(ByVal value As String)
            _PageUrl = value
        End Set
    End Property

    Private _CodeId As String
    Public Property CodeId() As String
        Get
            Return _CodeId
        End Get
        Set(ByVal value As String)
            _CodeId = value
        End Set
    End Property

    Private _ParentId As String
    Public Property ParentId() As String
        Get
            Return _ParentId
        End Get
        Set(ByVal value As String)
            _ParentId = value
        End Set
    End Property

    Public Sub AddSource(ByVal TextDisplay As String, ByVal PageUrl As String, ByVal CodeId As String, ByVal ParentId As String)
        Add(New MenuProperty With {.TextDisplay = TextDisplay, .PageUrl = PageUrl, .CodeId = CodeId, .ParentId = ParentId})
    End Sub

End Class

''' <summary>
''' คลาสช่วยจัดการเรื่อง Menu ของ Web แต่ต้องใส่ข้อมูลเมนูเข้ามาที่คลาสก่อน
''' </summary>
''' <remarks></remarks>
Public Class MenuManager
    Private ListMenuProperty As New MenuProperty

    ''' <summary>
    ''' ใส่ข้อมูล Menu
    ''' </summary>
    ''' <param name="TextDisplay">ข้อความที่จะแสดงบนเมนู</param>
    ''' <param name="PageUrl">ตำแหน่ง page</param>
    ''' <param name="CodeId">รหัสเมนู</param>
    ''' <param name="ParentId">รหัสแม่เนนู</param>
    ''' <remarks></remarks>
    Public Sub AddSource(ByVal TextDisplay As String, ByVal PageUrl As String, ByVal CodeId As String, ByVal ParentId As String)
        ListMenuProperty.AddSource(TextDisplay, PageUrl, CodeId, ParentId)
    End Sub

    ''' <summary>
    ''' สร้าง Menu
    ''' </summary>
    ''' <param name="MainMenu">ชื่อ Menu ฝั่ง Web</param>
    ''' <remarks></remarks>
    Public Sub CreateMenu(ByVal MainMenu As Menu)
        Dim Parent = (From r In ListMenuProperty Where r.ParentId Is Nothing)
        With MainMenu
            .Items.Clear()
            'Parent
            For Each r In Parent
                .Items.Add(New MenuItem With {.Text = r.TextDisplay, .NavigateUrl = r.PageUrl, .Value = r.CodeId})
            Next

            'Child
            For Each m As MenuItem In MainMenu.Items
                Dim Code = m.Value
                Dim child = (From r In ListMenuProperty Where r.ParentId = Code)
                For Each r In child
                    m.ChildItems.Add(New MenuItem With {.Text = r.TextDisplay, .NavigateUrl = r.PageUrl, .Value = r.CodeId})
                Next
            Next
        End With
    End Sub

End Class
