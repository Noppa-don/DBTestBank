Imports System.IO

Namespace Database

    ''' <summary>
    ''' Interface กลางกลุ่ม Command
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IDbCommon

        Property ConnectionString As String

        Sub OpenConnection()
        Sub CloseConnection()
        Function SelectData(ByVal Sql As String, Param As Object(), Optional TableName As String = "0") As DataTable
        Function ExeData(ByVal Sql As String, Param As Object(), Optional ExecuteType As EnExecuteType = EnExecuteType.ExecuteNonQuery) As Object
        Function GetTableSchema(TableName As String) As StringWriter
        Sub ExeDataWithCommandBuilder(ResultData As DataTable)
        Function GetNextId(ByVal TableName As String, ByVal ColumnName As String) As Long
        Function TestConnection() As Boolean

    End Interface

    Public Enum EnExecuteType
        ExecuteNonQuery
        ExecuteScalar
    End Enum

End Namespace


