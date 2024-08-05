Imports System.Reflection
Imports System.Web.UI

Namespace Web

    ''' <summary>
    ''' คลาสช่วยเรื่องข้อมูลประเภท ลำดับขั้น หรือ Tree
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageHierarchy
        Dim _IdName As String
        Dim _LabelName As String
        Dim _ParentName As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Data">กลุ่มข้อมูลที่ใช้</param>
        ''' <param name="DataType">ประเภทข้อมูลควรเป็น Class Type ไม่ใช List Type</param>
        ''' <param name="IdName">ชื่อฟิวที่เก็บ Id</param>
        ''' <param name="ParentName">ชื่อฟิวที่เก็บ Id แม่</param>
        ''' <param name="LabelName">ชื่อฟิวที่เก็บคำที่จะแสดง</param>
        ''' <remarks>ตัวอย่างการใช้คลาสนี้ Dim Tree As New ManageHierarchy(value, GetType(TreeData), "Id", "IdParent", "Name") ถ้าจะนำไปใช้กับ Tree คอนโทรนเรียก Method GetRoot ด้วย</remarks>
        Sub New(Data As IEnumerable, DataType As Type, ByVal IdName As String, ByVal ParentName As String, ByVal LabelName As String)
            _IdName = IdName
            _LabelName = LabelName
            _ParentName = ParentName
            DummyTreeData.AddSource(Data, DataType, _IdName, _LabelName, _ParentName)
        End Sub

        ''' <summary>
        ''' คืนค่าทั้งหมด
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAll() As HierarchyList
            Return DummyTreeData.GetAll
        End Function

        ''' <summary>
        ''' คืนค่าข้อมูลที่เป็น แม่
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRoot() As HierarchyList
            Return DummyTreeData.GetRoot
        End Function

    End Class

    Public Class HierarchyData
        Implements IHierarchyData

        Private _Id As Object
        Public Property Id() As Object
            Get
                Return _Id
            End Get
            Set(ByVal value As Object)
                _Id = value
            End Set
        End Property

        Private _Label As String
        Public Property Label() As String
            Get
                Return _Label
            End Get
            Set(ByVal value As String)
                _Label = value
            End Set
        End Property

        Private _IdParent As Object
        Public Property IdParent() As Object
            Get
                Return _IdParent
            End Get
            Set(ByVal value As Object)
                _IdParent = value
            End Set
        End Property

        Public Function GetChildren() As IHierarchicalEnumerable Implements IHierarchyData.GetChildren
            Dim Child As New HierarchyList
            For Each C As HierarchyData In DummyTreeData.GetAll
                If C.IdParent = Me.Id Then
                    Child.Add(C)
                End If
            Next
            Return Child
        End Function

        Public Function GetParent() As IHierarchyData Implements IHierarchyData.GetParent
            Dim Parent As New HierarchyList
            For Each C As HierarchyData In DummyTreeData.GetAll
                If C.Id = Me.IdParent Then
                    Parent.Add(C)
                End If
            Next
            Return Parent
        End Function

        Public ReadOnly Property HasChildren As Boolean Implements IHierarchyData.HasChildren
            Get
                Dim Child As HierarchyList = GetChildren()
                Return Child.Count > 0
            End Get
        End Property

        Public ReadOnly Property Item As Object Implements IHierarchyData.Item
            Get
                Return Me
            End Get
        End Property

        Public ReadOnly Property Path As String Implements IHierarchyData.Path
            Get
                Return Me.Id.ToString
            End Get
        End Property

        Public ReadOnly Property Type As String Implements IHierarchyData.Type
            Get
                Return Me.GetType.ToString
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.Label.ToString()
        End Function

    End Class

    Public Class HierarchyList
        Inherits List(Of HierarchyData)
        Implements IHierarchicalEnumerable

        Public Function GetHierarchyData(ByVal enumeratedItem As Object) As IHierarchyData Implements IHierarchicalEnumerable.GetHierarchyData
            Return CType(enumeratedItem, IHierarchyData)
        End Function

    End Class

    Public Class DummyTreeData
        Shared FullList As New HierarchyList
        Shared ParentList As New HierarchyList

        Public Shared Sub AddSource(ByVal Data As IEnumerable, ByVal DataType As Type, ByVal IdName As String, ByVal LabelName As String, ByVal ParentName As String)
            FullList.Clear()
            ParentList.Clear()
            Dim ObjType As Type = DataType
            For Each Row In Data
                Dim Id = ObjType.GetProperty(IdName).GetValue(Row, {})
                Dim Label = ObjType.GetProperty(LabelName).GetValue(Row, {})
                Dim Parent = ObjType.GetProperty(ParentName).GetValue(Row, {})
                FullList.Add(New HierarchyData With {.Id = Id, .Label = Label, .IdParent = Parent})
                If ObjType.GetProperty(ParentName).GetValue(Row, {}) Is Nothing Then
                    ParentList.Add(New HierarchyData With {.Id = Id, .Label = Label, .IdParent = Parent})
                End If
            Next
        End Sub

        Public Shared Function GetAll() As HierarchyList
            Return FullList
        End Function

        Public Shared Function GetRoot() As HierarchyList
            Return ParentList
        End Function

    End Class

End Namespace

