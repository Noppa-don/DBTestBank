Imports System.Data.Common
Imports System.Data.Linq
Imports System.Runtime.CompilerServices
Imports BusinessTablet360

Namespace Database

    ''' <summary>
    ''' คลาส Linq to sql
    ''' </summary>
    ''' <typeparam name="ClassDataContext"></typeparam>
    ''' <remarks></remarks>
    Public Class LinqSqlManager(Of ClassDataContext As DataContext)
        Inherits DatabaseUtil
        Private _ApplicationManager As IApplicationManager
        Private ParamConstructor(5) As Object
        Private Tr As DbTransaction
        Private Ctx As DataContext

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
                ParamConstructor(1) = ApplicationManager.GetConnectionString
            End Set
        End Property

#End Region

        ''' <summary>
        ''' ใช้สร้าง DataContext แบบไม่ใช้ Transection
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' ปัญหาตอนแรกที่ไม่มีฟังชั่นนี้คือ StoreDataContext ใช้ ConnectionString ใน  app config จาก Project BusinessStore
        ''' เลยต้องทำให้ดึง  ConnectionString จากตัวแปรของ IAplicationManager
        ''' </remarks>
        Public Function GetDataContext() As ClassDataContext
            Dim Ctx As ClassDataContext = Activator.CreateInstance(GetType(ClassDataContext), ParamConstructor(1))
            Return Ctx
        End Function

        ''' <summary>
        ''' ใช้สร้าง DataContext แบบใช้ Transection
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataContextWithTransaction() As ClassDataContext
            Ctx = Activator.CreateInstance(GetType(ClassDataContext), ParamConstructor(1))
            Ctx.Connection.Open()
            Tr = Ctx.Connection.BeginTransaction
            Ctx.Transaction = Tr
            Return Ctx
        End Function

        ''' <summary>
        ''' ใช้คู่กับฟังชั่น GetDataContextWithTransaction ใช้เมื่อจะ Commit Database
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DataContextCommitTransaction()
            Tr.Commit()
            Tr.Dispose()
            Tr = Nothing
            If Ctx.Connection.State = ConnectionState.Open Then
                Ctx.Connection.Close()
            End If
            Ctx.Connection.Dispose()
            Ctx.Dispose()
            Ctx = Nothing
        End Sub

        ''' <summary>
        ''' ใช้คู่กับฟังชั่น GetDataContextWithTransaction ใช้เมื่อจะ Rollback Database
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DataContextRollbackTransaction()
            Tr.Rollback()
            Tr.Dispose()
            Tr = Nothing
            If Ctx.Connection.State = ConnectionState.Open Then
                Ctx.Connection.Close()
            End If
            Ctx.Connection.Dispose()
            Ctx.Dispose()
            Ctx = Nothing
        End Sub

        ''' <summary>
        ''' ประมวลผลคำสั่ง SQL ให้ได้ผลลัพธ์ในรูป Object ที่ส่งเข้ามา
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="Context"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DataContextExecuteObjects(Of T)(ByVal Context As DataContext) As IQueryable(Of T)
            If MainSql = "" Then
                Return Nothing
                Throw New Exception("ไม่มี SQL")
            End If

            If ListSqlPart.Count < 1 Then
                'ตัด WHERE ทิ้งในกรณีที่ไม่ได้มีการส่งเงื่อนไข SQL มา
                If LockWhere = False Then
                    Dim Reg As New Text.RegularExpressions.Regex("WHERE", Text.RegularExpressions.RegexOptions.IgnoreCase)
                    MainSql = Reg.Replace(MainSql, "")
                End If
            End If

            DebugUtil()
            Return Context.ExecuteQuery(Of T)(MainSql, ListSqlPart.GetValue.Where(Function(q) q IsNot Nothing).ToArray).AsQueryable
        End Function

        Public Function DataContextExecuteObjects(Of T)() As IQueryable(Of T)
            If MainSql = "" Then
                Throw New Exception("ไม่มี SQL")
            End If

            If ListSqlPart.Count < 1 Then
                'ตัด WHERE ทิ้งในกรณีที่ไม่ได้มีการส่งเงื่อนไข SQL มา
                If LockWhere = False Then
                    Dim Reg As New Text.RegularExpressions.Regex("WHERE", Text.RegularExpressions.RegexOptions.IgnoreCase)
                    MainSql = Reg.Replace(MainSql, "")
                End If
            End If

            DebugUtil()
            Dim Ctx As DataContext = Activator.CreateInstance(GetType(ClassDataContext), ParamConstructor(1))
            Return Ctx.ExecuteQuery(Of T)(MainSql, ListSqlPart.GetValue.Where(Function(q) q IsNot Nothing).ToArray).AsQueryable
        End Function

        ''' <summary>
        ''' ประมวลผลคำสั่ง Command Insert, Update, Delete ของ DataContext
        ''' </summary>
        ''' <param name="Context"></param>
        ''' <remarks></remarks>
        Public Sub DataContextExecuteCommand(ByVal Context As DataContext)
            Try
                Context.ExecuteCommand(MainSql, ListSqlPart.GetValue.ToArray)
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Throw New Exception(ex.Message)
            End Try
        End Sub

    End Class

    Public Module ModuleLinqSqlManager

        ''' <summary>
        ''' ฟั่งชั่นคืนค่า DbCommand ที่แปลงจาก IQueryable
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <param name="Ctx">DataContext ตัวที่ใช้อยู่ก็ได้ หรือ ตัวใหม่เลยก็ได้</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function GetDbCommand(Source As IQueryable, Ctx As DataContext) As DbCommand
            Return Ctx.GetCommand(Source)
        End Function

    End Module

End Namespace

