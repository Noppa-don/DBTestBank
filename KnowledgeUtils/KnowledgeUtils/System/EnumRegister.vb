Imports System.Runtime.CompilerServices

Namespace System

    Public MustInherit Class EnumRegister
        Inherits List(Of EnumItem)

        ''' <summary>
        ''' เพิ่ม Item
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <param name="Text"></param>
        ''' <remarks></remarks>
        Public Sub AddItem(ByVal Value As Integer, ByVal Text As String)
            Add(New EnumItem With {.Value = Value, .Text = Text})
        End Sub

        ''' <summary>
        ''' หาค่า Text โดยระบุค่า Value
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetText(ByVal Value As Integer) As String
            Dim R = (From e In Me Where e.Value = Value Select e.Text).SingleOrDefault
            Return R
        End Function

        ''' <summary>
        ''' หาค่า Value โดยระบุ Text
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetValue(ByVal Text As String) As Integer
            Dim R = (From e In Me Where e.Text = Text Select e.Value).SingleOrDefault
            Return R
        End Function

        ''' <summary>
        ''' คืนค่าทั้งหมด
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetList() As IEnumerable(Of EnumItem)
            Return Me
        End Function

    End Class

    Public Class EnumItem

        Private _Value As Integer
        Public Property Value() As Integer
            Get
                Return _Value
            End Get
            Set(ByVal value As Integer)
                _Value = value
            End Set
        End Property

        Private _text As String
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

    End Class

    Public Module ModuleEnumRegister

        ''' <summary>
        ''' คืนค่าข้อความที่ได้จาก Class EnumRegister
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="Source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function GetEnumText(Of T)(ByVal Source As [Enum]) As String
            Dim EnumSource = Activator.CreateInstance(GetType(T))
            Return EnumSource.GetText(Source)
        End Function

    End Module

End Namespace

