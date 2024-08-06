Imports System.Data.Linq

Namespace Database

    Public Class RepositoryLinq(Of T As Class)

        Private Ctx As DataContext

        Sub New(Context As DataContext)
            Ctx = Context
        End Sub

        Public Function GetAll() As IEnumerable(Of T)
            Return Ctx.GetTable(Of T)()
        End Function

        Public Function GetSingle(Fn As Global.System.Func(Of T, Boolean)) As T
            Return Ctx.GetTable(Of T).Where(Fn).SingleOrDefault
        End Function

        Public Function GetMany(Fn As Global.System.Func(Of T, Boolean)) As IEnumerable(Of T)
            Return Ctx.GetTable(Of T).Where(Fn)
        End Function

        Public Sub InsertSingle(Entity As T)
            Ctx.GetTable(Of T).InsertOnSubmit(Entity)
            'Ctx.SubmitChanges()
        End Sub

        Public Sub InsertMany(Entity As IEnumerable(Of T))
            Ctx.GetTable(Of T).InsertAllOnSubmit(Entity)
            'Ctx.SubmitChanges()
        End Sub

        Public Sub DeleteSingle(Entity As T)
            Ctx.GetTable(Of T).DeleteOnSubmit(Entity)
            'Ctx.SubmitChanges()
        End Sub

        Public Sub DeleteMany(Entity As IEnumerable(Of T))
            Ctx.GetTable(Of T).DeleteAllOnSubmit(Entity)
            'Ctx.SubmitChanges()
        End Sub

        Public Sub Update(Entity As T, OriginalEntity As T)
            Ctx.GetTable(Of T).Attach(Entity, OriginalEntity)
            'Ctx.SubmitChanges()
        End Sub

        Public Sub Save()
             Ctx.SubmitChanges()
        End Sub

    End Class

End Namespace


