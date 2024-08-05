
Namespace System

    Public Class ListProperty
        Inherits List(Of ObjectProperty)

        Public Sub AddProperty(ByVal PropertyName As String, ByVal PropertyValue As Object)
            Dim R = Me.Where(Function(p) p.PropertyName = PropertyName).SingleOrDefault
            If R Is Nothing Then
                Add(New ObjectProperty With {.PropertyName = PropertyName, .PropertyValue = PropertyValue})
            End If
        End Sub

    End Class

    Public Class ObjectProperty

        Private _PropertyName As String
        Public Property PropertyName() As String
            Get
                Return _PropertyName
            End Get
            Set(ByVal value As String)
                _PropertyName = value
            End Set
        End Property

        Private _PropertyValue As Object
        Public Property PropertyValue() As Object
            Get
                Return _PropertyValue
            End Get
            Set(ByVal value As Object)
                _PropertyValue = value
            End Set
        End Property

    End Class

End Namespace

