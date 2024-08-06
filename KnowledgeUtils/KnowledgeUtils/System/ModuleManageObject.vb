Imports System.Runtime.CompilerServices

Namespace System

    Public Module ModuleManageObject

        ''' <summary>
        ''' Map Class
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="Obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()> Function ToType(Of T)(Obj As Object) As T
            Try
                Dim TmpType = GetType(T)
                Dim Tmp As Object = Activator.CreateInstance(TmpType)
                For Each p In TmpType.GetProperties
                    If Obj.GetType.GetProperty(p.Name) IsNot Nothing Then
                        p.SetValue(Tmp, Obj.GetType.GetProperty(p.Name).GetValue(Obj, Nothing), Nothing)
                    End If
                Next
                Return Tmp
            Catch ex As Exception
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Throw New Exception(ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' แปลง IEnumerable ให้เป็น DataTable
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function ToDataTable(Source As IEnumerable) As DataTable
            If Source Is Nothing Then
                Return New DataTable
            End If
            Dim Dt As DataTable
            Dim Properties As Reflection.PropertyInfo() = Nothing
            For Each Item In Source
                Dim Type = Item.GetType
                If Properties Is Nothing Then
                    'ครั้งแรกที่เข้ามาสร้างโครงสร้าง col ให้ dt
                    Dt = New DataTable(Type.Name)
                    Properties = Type.GetProperties
                    For Each p In Properties
                        Dim PType = p.PropertyType
                        If PType.IsGenericType AndAlso (PType.GetGenericTypeDefinition() = GetType(Nullable(Of ))) Then
                            PType = PType.GetGenericArguments()(0)
                        End If
                        Dt.Columns.Add(New DataColumn(p.Name, PType))
                    Next
                End If
                Dim Dr As DataRow = Dt.NewRow
                For Each p In Type.GetProperties
                    Dr(p.Name) = If(p.GetValue(Item, Nothing), DBNull.Value)
                Next
                Dt.Rows.Add(Dr)
            Next
            Return Dt
        End Function

    End Module

End Namespace


