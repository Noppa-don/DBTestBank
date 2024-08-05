Imports System.IO

Namespace Database

    ''' <summary>
    ''' คลาสช่วยการทำงานของดาต้าเบสกลุ่ม Command
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBManager
        Inherits DatabaseUtil

        Private _ApplicationManager As IApplicationManager
        Private _IDbCommon As IDbCommon

#Region "Not Dependency"

        ''' <summary>
        ''' ผูกกับ AplicationManager เผื่อเอาค่า connectionstring จาก Method GetSqlConnectionString
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ApplicationManager() As IApplicationManager
            Get
                Return _ApplicationManager
            End Get
            Set(ByVal value As IApplicationManager)
                _ApplicationManager = value
            End Set
        End Property

        Public Property DbCommon() As IDbCommon
            Get
                Return _IDbCommon
            End Get
            Set(ByVal value As IDbCommon)
                _IDbCommon = value
                _IDbCommon.ConnectionString = ApplicationManager.GetConnectionString
                Symbol = EnSymbolParameter.Add
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Gen Id ตามชื่อ Table
        ''' </summary>
        ''' <param name="TableName"></param>
        ''' <param name="ColumnName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNextId(ByVal TableName As String, ByVal ColumnName As String) As Long
            Return DbCommon.GetNextId(TableName, ColumnName)
        End Function

        ''' <summary>
        ''' คืนโครงสร้างเทเบิลในรูปแบบ xml
        ''' </summary>
        ''' <param name="TableName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTableSchema(TableName As String) As StringWriter
            Return DbCommon.GetTableSchema(TableName)
        End Function

        ''' <summary>
        ''' ดูข้อมูล (Query ใส่ DataTable)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectData(Optional TableName As String = "0") As DataTable
            DebugUtil()
            Return DbCommon.SelectData(MainSql, GetAllListSqlPart.ToArray, TableName)
        End Function

        ''' <summary>
        ''' ประมวลผลคำสั่ง
        ''' </summary>
        ''' <param name="ExecuteType"></param>
        ''' <remarks></remarks>
        Public Sub ExeData(Optional ExecuteType As EnExecuteType = EnExecuteType.ExecuteNonQuery)
            DebugUtil()
            DbCommon.ExeData(MainSql, GetAllListSqlPart.ToArray, ExecuteType)
        End Sub

        ''' <summary>
        ''' ประมวลผลคำสั่งแบบรับผลลัพธ์จาก data table ที่ถูกปรับปรุงแล้ว DataTable ควรมีชื่อ Table ติดมาด้วย
        ''' </summary>
        ''' <param name="ResultData"></param>
        ''' <remarks></remarks>
        Public Sub ExeDataWithCommandBuilder(ResultData As DataTable)
            DbCommon.ExeDataWithCommandBuilder(ResultData)
        End Sub

        Public Function TestConnection() As Boolean
            Return DbCommon.TestConnection
        End Function

    End Class

End Namespace

