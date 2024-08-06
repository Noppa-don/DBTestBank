''' <summary>
''' ฟังชั่นที่ควรมีในแต่ละ Page
''' </summary>
''' <remarks></remarks>
Public Interface IPage

    ''' <summary>
    ''' ใช้กำหนดการแสดงของ control ต่างๆของหน้านั้นๆ
    ''' </summary>
    ''' <param name="PageDetail">
    ''' ใช้ PageDetail.Index เมื่อใน page นั้นมีหลายส่วนเช่นมี แท็บอยู่ในหน้านั้น Index ถ้าไม่กำหนด Default=0
    ''' ใช้ PageDetail.Status เช็คสถานะของหน้านั้นเผื่อกำหนดรูปแบบในหน้าจอ
    ''' </param>
    ''' <remarks></remarks>
    Sub DisplayUI(ByVal PageDetail As PagePart)
    ''' <summary>
    ''' ใช้เช็คความถูกต้องของหน้านั้นๆ
    ''' 1.เช็คสิ่งที่ต้องใส่
    ''' 2.เช็คเรื่องซ้ำ
    ''' </summary>
    ''' <param name="PageDetail">
    ''' ใช้ PageDetail.Index เมื่อใน page นั้นมีหลายส่วนเช่นมี แท็บอยู่ในหน้านั้น Index ถ้าไม่กำหนด Default=0
    ''' ใช้ PageDetail.Status เช็คสถานะของหน้านั้นเผื่อกำหนดรูปแบบในหน้าจอ
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValidatePage(ByVal PageDetail As PagePart, Optional ByVal CheckData As Dictionary(Of String, Object) = Nothing) As Boolean
    ''' <summary>
    ''' ใช้เขียนการ Bind Grid ของหน้านั้นๆ
    ''' </summary>
    ''' <param name="GridDetail"></param>
    ''' <remarks></remarks>
    Sub BindGrid(ByVal GridDetail As PageGridDetail)

    ''' <summary>
    ''' ไว้เก็บ ID ของ Grid ที่เราเลือกของหน้านั้นๆ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property SelectID() As Object

End Interface

Public Class PageGridDetail
    Public GridIndex As Integer = 0
    Private _PageCommand As String
    Private _CurrentPage As Integer

    Public Sub New(Optional ByVal PageCommand As String = "", Optional ByVal CurrentPage As Integer = 1)
        _PageCommand = PageCommand
        _CurrentPage = CurrentPage + 1
    End Sub

    Public Function CalPageIndex(ByVal PageCount As Integer) As Integer
        If _PageCommand <> "" Then
            If IsNumeric(_PageCommand) Then
                Return CType(_PageCommand, Integer)
            Else
                Dim P As Integer
                Select Case _PageCommand
                    Case ""
                        P = 1
                    Case "Sort"
                        P = _CurrentPage
                    Case "First"
                        P = 1
                    Case "Last"
                        P = PageCount
                    Case "Next"
                        P = _CurrentPage + 1
                    Case "Prev"
                        P = _CurrentPage - 1
                End Select
                Return P
            End If
        Else
            Return 1
        End If
    End Function
End Class

Public Class PagePart
    Public Index As Integer = 0
    Public Status As EnStatusUI
End Class

Public Enum EnStatusUI
    Clear
    Search
    Add
    Update
    Delete
End Enum

Public Class EnumStatusUI
    Inherits EnumRegister

    Public Sub New()
        AddItem(EnStatusUI.Clear, "ล้างหน้าจอ")
        AddItem(EnStatusUI.Search, "ค้นหา")
        AddItem(EnStatusUI.Add, "เพิ่ม")
        AddItem(EnStatusUI.Update, "แก้ไข")
        AddItem(EnStatusUI.Delete, "ลบ")
    End Sub

End Class
