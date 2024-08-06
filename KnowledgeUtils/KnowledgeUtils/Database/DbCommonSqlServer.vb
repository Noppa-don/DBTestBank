Imports System.Data.SqlClient
Imports System.IO

Namespace Database

    ''' <summary>
    ''' กลุ่ม SqlServer Command
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DbCommonSqlServer
        'todo ขาด Rollback
        Implements IDbCommon
        Dim _ConnectionString As String
        Dim Cn As SqlConnection
        Dim Da As SqlDataAdapter

        Public Property ConnectionString As String Implements IDbCommon.ConnectionString
            Get
                Return _ConnectionString
            End Get
            Set(value As String)
                _ConnectionString = value
            End Set
        End Property

        Public Sub OpenConnection() Implements IDbCommon.OpenConnection
            Cn = New SqlConnection
            Cn.ConnectionString = ConnectionString
            Cn.Open()
        End Sub

        Public Sub CloseConnection() Implements IDbCommon.CloseConnection
            If Cn.State = ConnectionState.Open Then
                Cn.Close()
            End If
            Cn.Dispose()
            Cn = Nothing
        End Sub

        Public Function SelectData(Sql As String, Param As Object(), Optional TableName As String = "0") As DataTable Implements IDbCommon.SelectData
            OpenConnection()
            Using Da = New SqlDataAdapter()
                Dim Ds As DataSet = New DataSet
                Dim Cm As New SqlCommand(Sql)
                Cm.Connection = Cn
                For Each i In Param
                    Cm.Parameters.AddWithValue(i.PropertyName, i.PropertyValue)
                Next
                Da.SelectCommand = Cm
                Da.FillSchema(Ds, SchemaType.Mapped, TableName)
                Da.Fill(Ds, TableName)
                SelectData = Ds.Tables(0)
            End Using
            CloseConnection()
        End Function

        Public Function ExeData(ByVal Sql As String, Param As Object(), Optional ExecuteType As EnExecuteType = EnExecuteType.ExecuteNonQuery) As Object Implements IDbCommon.ExeData
            OpenConnection()
            Using Cm As New SqlCommand
                Dim Result As Object
                Cm.CommandText = Sql
                Cm.Connection = Cn
                For Each i In Param
                    Cm.Parameters.AddWithValue(i.PropertyName, i.PropertyValue)
                Next
                Select Case ExecuteType
                    Case EnExecuteType.ExecuteNonQuery
                        Result = Cm.ExecuteNonQuery()
                    Case EnExecuteType.ExecuteScalar
                        Result = Cm.ExecuteScalar()
                End Select
                ExeData = Result
            End Using
            CloseConnection()
        End Function

        Public Function GetTableSchema(TableName As String) As StringWriter Implements IDbCommon.GetTableSchema
            OpenConnection()
            Da = New SqlDataAdapter("SELECT * FROM " & TableName, Cn)
            Dim Dt As New DataTable
            Da.FillSchema(Dt, SchemaType.Source)
            Dim StWr As New StringWriter
            Dt.WriteXmlSchema(StWr)
            CloseConnection()
            Return StWr
        End Function

        Public Sub ExeDataWithCommandBuilder(ResultData As DataTable) Implements IDbCommon.ExeDataWithCommandBuilder
            Dim Sql As String = "SELECT * FROM " & ResultData.TableName
            Dim MyDa As New SqlDataAdapter(Sql, ConnectionString)
            Dim MyCm As New SqlCommandBuilder(MyDa)
            MyDa.Update(ResultData)
        End Sub

        Public Function GetNextId(TableName As String, ColumnName As String) As Long Implements IDbCommon.GetNextId
            Dim Sql = String.Format("SELECT ISNULL(MAX({0}),0) FROM {1}", ColumnName, TableName)
            Dim Result = SelectData(Sql, {}).Rows(0).Item(0) + 1
            Return Result
        End Function

        Public Function TestConnection() As Boolean Implements IDbCommon.TestConnection
            Try
                Cn = New SqlConnection
                Cn.ConnectionString = ConnectionString
                Cn.Open()
                Cn.Close()
                Cn.Dispose()
                Cn = Nothing
                Return True
            Catch ex As Exception
                Cn = Nothing
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

    End Class

End Namespace

