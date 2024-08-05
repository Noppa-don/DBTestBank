Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.Configuration
Imports BusinessTablet360

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
    Dim Tr As SqlTransaction
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
                'Dim StrDecryption As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                'Str = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                'Str = KNConfigData.DecryptData(StrDecryption)
#If F5 = "1" Then
                'Str = "Data Source=10.100.1.72;Initial Catalog= QuickTest_9_fromSupport_20130214_noon;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"
                'Dim StrDecryption As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                'Str = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                'Str = KNConfigData.DecryptData(StrDecryption)
                ''Str = "Data Source=10.100.1.72;Initial Catalog= QuickTest_9_fromSupport_20130214_noon;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"
                'Str = "Data Source=10.100.1.72;Initial Catalog= QuickTest_9_Empty;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"
                Str = "Data Source=(local)\sql2012;Initial Catalog= Pointplus_Present;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"
#ElseIf GetPlainTextConnectionStringFromWebConfig = "1" Then
                Str = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
#Else
                'Str = ClsLanguage.GetConStr()
                Str = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
                'Str = "Data Source=KN-SERVER\KNSERVER;Initial Catalog=QuickTest_Production;Persist Security Info=True;User ID=sa;Password=sql.123kl;Max Pool Size = 50000"
#End If

            End If
        Else
            If strConnectionString <> "" Then
                Str = strConnectionString
            Else
                Str = ConfigurationManager.AppSettings("ConnectionString")
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

    Public Overrides Sub Execute(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing)
        If InputConn Is Nothing Then
            OpenConnect()
        End If
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If InputConn Is Nothing Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
            .ExecuteNonQuery()
        End With
        If InputConn Is Nothing Then
            CloseConnect()
        End If
    End Sub

    Public Overrides Function ExecuteScalar(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String
        If InputConn Is Nothing Then
            OpenConnect()
        End If
        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If InputConn Is Nothing Then
                .Connection = conn
            Else
                .Connection = InputConn
            End If
        End With

        Dim Result As Object = cmd.ExecuteScalar
        If Result Is Nothing Then
            Result = ""
            'Else
            '    Result = cmd.ExecuteScalar.ToString()
        End If
        Return Result.ToString()
        If InputConn Is Nothing Then
            CloseConnect()
        End If
    End Function

    Public Overrides Function ExecuteScalarWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

        Dim cmd As New SqlCommand
        With cmd
            .CommandType = CommandType.Text
            .CommandText = prmSQL
            If InputConn Is Nothing Then
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

        Dim Result As String = cmd.ExecuteScalar.ToString()
        If Result Is Nothing Then
            Result = ""
        End If
        Return Result

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
        If InputConn Is Nothing Then
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            conn.ConnectionString = Str
            conn.Open()
            Tr = conn.BeginTransaction
        Else
            'InputConn.ConnectionString = Str
            'InputConn.Open()
            Tr = InputConn.BeginTransaction
        End If
    End Sub

    Public Overrides Sub ExecuteWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Try
            Dim cmd As New SqlCommand
            With cmd
                .CommandType = CommandType.Text
                .CommandText = prmSQL
                If InputConn Is Nothing Then
                    .Connection = conn
                Else
                    .Connection = InputConn
                End If
                .Transaction = Tr
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            'Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Overrides Sub CommitTransection(Optional ByRef InputConn As SqlConnection = Nothing)
        Tr.Commit()
        Tr.Dispose()

        If InputConn Is Nothing Then
            conn.Close()
            conn.Dispose()
        End If
    End Sub

    Public Overrides Sub RollbackTransection(Optional ByRef InputConn As SqlConnection = Nothing)
        Tr.Rollback()
        Tr.Dispose()

        If InputConn Is Nothing Then
            conn.Close()
            conn.Dispose()
        End If
    End Sub

    Public Overrides Function getdata(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal CommandTimeOut As Integer? = Nothing) As System.Data.DataTable
        ds = New DataSet()
        Dim da As SqlDataAdapter
        If InputConn Is Nothing Then
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
        If InputConn Is Nothing Then
            CloseConnect()
        End If
    End Function

    Public Overrides Function getdataWithTransaction(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As System.Data.DataTable

        'OpenConnect()
        ds = New DataSet()
        'Dim da As New SqlDataAdapter(sql, conn)
        Dim da As New SqlDataAdapter
        If InputConn Is Nothing Then
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
            'Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
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
End Class
