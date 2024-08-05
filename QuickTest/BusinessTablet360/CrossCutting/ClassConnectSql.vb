Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Configuration
Imports System.Web

<Serializable()> _
Public Class ClassConnectSql
    Inherits ClsConnect

    Public Str As String = "" 'ConfigurationSettings.AppSettings("ConnectionString")
    <NonSerialized()> _
    Public conn As New SqlConnection
    Public ds As New DataSet
    <NonSerialized()> _
    Public da As SqlDataAdapter
    <NonSerialized()> _
    Public Tr As SqlTransaction

    Dim ClsKNConfig As New KNConfigData()
    ''' <summary>
    ''' ถ้าส่ง false มา , หรือ ไม่ได้ส่งมาเลย แปลว่าเป็น webform ก็จะเรียก หา connectionstring แบบนึง
    ''' </summary>
    ''' <param name="IsWinFormApp"></param>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal IsWinFormApp As Boolean = False, Optional ByVal strConnectionString As String = "")
        If Not IsWinFormApp Then
            If strConnectionString <> "" Then
                Str = strConnectionString
            Else
                '#If F5 = "1" Then
                '                Str = "Data Source=(local);Initial Catalog=PP_Tester;Persist Security Info=True;User ID=sa;Password=P@ssw0rd;Max Pool Size = 50000"
                '#Else
                If HttpContext.Current.Application("PointPlusURL") IsNot Nothing Then
                        ClsLanguage.TypeClsLanguage = EnLangType.T360
                    Else
                        ClsLanguage.TypeClsLanguage = EnLangType.Quick
                    End If
                    Str = ClsLanguage.GetConStr()
                '#End If
            End If
        Else
            If strConnectionString <> "" Then
                Str = strConnectionString
            Else
                If ConfigurationManager.AppSettings("ConnectionString") <> "" Then
                    Str = ConfigurationManager.AppSettings("ConnectionString")
                Else
                    Str = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                End If
            End If

        End If
    End Sub

    Public Sub OpenExclusiveConnect(ByRef InputConn As SqlConnection)
        If InputConn.State = ConnectionState.Open Then
            InputConn.Close()
        End If

        InputConn.ConnectionString = Str
        InputConn.Open()
    End Sub


    Public Sub OpenConnect()
        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If

        conn.ConnectionString = Str
        conn.Open()
    End Sub

    Public Sub CloseExclusiveConnect(ByRef InputConn As SqlConnection)
        InputConn.Close()
        InputConn.Dispose()
    End Sub

    Public Sub CloseConnect()
        conn.Close()
        conn.Dispose()
    End Sub

    Public Overrides Sub Execute(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal ChangeTimeoutSeconds As Integer = 30)
        If IsNothing(InputConn) Then
            OpenConnect()
        End If
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            .CommandTimeout = ChangeTimeoutSeconds
            If IsNothing(InputConn) Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
            .ExecuteNonQuery()
        End With
        If IsNothing(InputConn) Then
            CloseConnect()
        End If
    End Sub

    Public Overrides Function ExecuteScalar(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        If IsNothing(InputConn) Then
            OpenConnect()
        End If
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If IsNothing(InputConn) Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
        End With
        Dim Result As Object
        Try
            Result = cmd.ExecuteScalar
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception(ex.Message)
        End Try


        If Result Is Nothing Then
            Result = ""
        End If
        'shin 20160415,ย้าย if และ closeconnect ขึ้นข้างบน ก่อนจะ return , เพื่อปิด conn ก่อนจะจบ func
        If IsNothing(InputConn) Then
            CloseConnect()
        End If
        'shin 20160415,ย้าย if และ closeconnect ขึ้นข้างบน ก่อนจะ return , เพื่อปิด conn ก่อนจะจบ func
        Return Result.ToString()

    End Function
    Public Function ExecuteScalarAsObject(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As Object
        If IsNothing(InputConn) Then
            OpenConnect()
        End If
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If IsNothing(InputConn) Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
        End With

        Dim Result As Object = cmd.ExecuteScalar
        'shin 20160415,ย้าย if และ closeconnect ขึ้นข้างบน ก่อนจะ return , เพื่อปิด conn ก่อนจะจบ func
        If IsNothing(InputConn) Then
            CloseConnect()
        End If
        'shin 20160415,ย้าย if และ closeconnect ขึ้นข้างบน ก่อนจะ return , เพื่อปิด conn ก่อนจะจบ func
        Return Result
    End Function

    Public Overrides Function ExecuteScalarWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If IsNothing(InputConn) Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
            .Transaction = Tr
        End With

        'Dim Result As String
        'If cmd.ExecuteScalar Is Nothing Then
        '    Result = ""
        'Else
        '    Result = cmd.ExecuteScalar.ToString()
        'End If
        Dim Result As Object = cmd.ExecuteScalar
        If Result Is Nothing Then
            Return ""
        End If
        Return Result.ToString()

    End Function

    Public Overrides Sub ExecuteWithImage(ByVal prmSQL As String, ByVal ImageName As String, ByVal ImageByte() As Byte)
        OpenConnect()
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .Parameters.Add(ImageName, DbType.Binary, ImageByte.Length).Value = ImageByte
            .CommandText = prmSQL
            .Connection = conn
            .ExecuteNonQuery()
        End With
        CloseConnect()
    End Sub

    Public Overrides Sub OpenWithTransection(Optional ByRef InputConn As SqlConnection = Nothing)
        If IsNothing(InputConn) Then
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            conn.ConnectionString = Str
            conn.Open()
            Tr = conn.BeginTransaction
        Else
            Tr = InputConn.BeginTransaction
        End If

    End Sub

    Public Overrides Sub ExecuteWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Try
            Dim cmd As New SqlCommand
            With cmd
                .CommandType = CommandType.Text
                .CommandText = prmSQL
                If IsNothing(InputConn) Then
                    .Connection = conn
                Else
                    .Connection = InputConn
                End If
                .Transaction = Tr
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ExecuteResultWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As Integer
        Try
            Dim cmd As New SqlCommand
            With cmd
                .CommandType = CommandType.Text
                .CommandText = prmSQL
                If IsNothing(InputConn) Then
                    .Connection = conn
                Else
                    .Connection = InputConn
                End If
                .Transaction = Tr
                Return .ExecuteNonQuery()
            End With
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Overrides Sub CommitTransection(Optional ByRef InputConn As SqlConnection = Nothing)
        Tr.Commit()
        Tr.Dispose()

        If IsNothing(InputConn) Then
            conn.Close()
            conn.Dispose()
        End If
    End Sub

    Public Overrides Sub RollbackTransection(Optional ByRef InputConn As SqlConnection = Nothing)
        If Not (IsNothing(InputConn) And IsNothing(conn)) Then
            Tr.Rollback()
            Tr.Dispose()
        End If

        If IsNothing(InputConn) Then
            conn.Close()
            conn.Dispose()
        End If
    End Sub

    Public Overrides Function getdata(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal CommandTimeOut As Integer? = Nothing) As System.Data.DataTable
        ds = New DataSet()
        Dim da As SqlDataAdapter
        If IsNothing(InputConn) Then
            OpenConnect()
            da = New SqlDataAdapter(sql, conn)
        Else
            da = New SqlDataAdapter(sql, InputConn)
        End If
        If CommandTimeOut IsNot Nothing Then
            da.SelectCommand.CommandTimeout = CommandTimeOut
        End If

        'da.FillSchema(ds, SchemaType.Mapped, TableName)'เอาโครงสร้าง DB มาด้วย
        da.Fill(ds, TableName)
        getdata = ds.Tables(0)
        If IsNothing(InputConn) Then
            CloseConnect()
        End If
    End Function

    Public Overrides Function getdataNotDataSet(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As System.Data.DataTable
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        If IsNothing(InputConn) Then
            OpenConnect()
            da = New SqlDataAdapter(sql, conn)
        Else
            da = New SqlDataAdapter(sql, InputConn)
        End If
        dt.TableName = TableName
        da.FillSchema(dt, SchemaType.Mapped)
        da.Fill(dt)
        getdataNotDataSet = dt
        If IsNothing(InputConn) Then
            CloseConnect()
        End If
    End Function

    Public Function getdataNotDataSetWithTransection(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As System.Data.DataTable
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        If IsNothing(InputConn) Then
            da = New SqlDataAdapter(sql, conn)
        Else
            da = New SqlDataAdapter(sql, InputConn)
        End If
        da.SelectCommand.Transaction = Tr
        dt.TableName = TableName
        da.FillSchema(dt, SchemaType.Mapped)
        da.Fill(dt)
        getdataNotDataSetWithTransection = dt

    End Function

    Public Overrides Function GetTableSchema(ByVal TableName As String) As DataTable
        OpenConnect()

        Dim MyDs As New DataSet
        Dim da As New SqlDataAdapter(TableName, conn)
        da.FillSchema(MyDs, SchemaType.Mapped, TableName)
        da.Dispose()
        GetTableSchema = MyDs.Tables(0)

        CloseConnect()
    End Function

    Public Overrides Function getdataWithSchema(ByVal sql As String, Optional ByVal TableName As String = "0") As DataTable
        OpenConnect()

        Dim MyDs As New DataSet
        Dim da As New SqlDataAdapter(sql, conn)
        da.FillSchema(MyDs, SchemaType.Mapped, TableName)
        da.Fill(MyDs, TableName)
        da.Dispose()
        getdataWithSchema = MyDs.Tables(0)

        CloseConnect()
    End Function

    Public Overrides Function getdataWithTransaction(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As System.Data.DataTable

        'OpenConnect()
        ds = New DataSet()
        Dim da As New SqlDataAdapter
        If IsNothing(InputConn) Then
            da = New SqlDataAdapter(sql, conn)
        Else
            da = New SqlDataAdapter(sql, InputConn)
        End If
        da.SelectCommand.Transaction = Tr
        'da.FillSchema(ds, SchemaType.Mapped, TableName)'เอาโครงสร้าง DB มาด้วย
        da.Fill(ds, TableName)
        getdataWithTransaction = ds.Tables(0)
        'CloseConnect()
    End Function

    Public Overrides Function ExecuteWithCommandBuilder(ByVal DtResult As DataTable) As Boolean
        Try
            Dim Sql As String = "SELECT * FROM " & DtResult.TableName
            da = New SqlDataAdapter(Sql, Str)
            Dim bd As SqlCommandBuilder = New SqlCommandBuilder(da)

            da.Update(DtResult)
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Overrides Function getdataset(ByVal sql As String, ByRef ds As DataSet, ByVal tablename As String) As System.Data.DataSet
        OpenConnect()
        Dim da As New SqlDataAdapter(sql, conn)
        If IsNothing(ds) Then ds = New DataSet
        da.Fill(ds, tablename)
        getdataset = ds
        CloseConnect()
    End Function
    Public Overrides Function getdataset(ByVal sql As String) As System.Data.DataSet
        OpenConnect()
        Dim da As New SqlDataAdapter(sql, conn)
        Dim ds = New DataSet
        da.Fill(ds)
        getdataset = ds
        CloseConnect()
    End Function
    Public Overrides Function CleanString(ByVal inString As String) As String
        If inString Is Nothing Then
            Return ""
        End If
        Return inString.Replace("'", "''")
    End Function

    ' Get NewID
    ' รอพี่ชิน มาปรับให้ใช้เป็น store ที่ชื่อว่า 
    Public Overrides Function ExecInsertSqlReturnNewID(Optional ByVal InputConn As SqlConnection = Nothing) As String
        Dim sql As String = " SELECT NEWID()"
        ExecInsertSqlReturnNewID = ExecuteScalar(sql, InputConn)
    End Function

    Public Function ExecuteReader(strSQL As String) As SqlDataReader
        Dim dr As SqlDataReader
        OpenConnect()

        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = strSQL
            .Connection = conn

        End With

        dr = cmd.ExecuteReader()

        Return dr
    End Function


End Class



