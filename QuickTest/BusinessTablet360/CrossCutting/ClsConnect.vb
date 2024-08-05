Imports System.Data
Imports System.Data.SqlClient

<Serializable()> _
Public MustInherit Class ClsConnect

    Public Sub New()

    End Sub

    Public Overridable Function getdata(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal CommandTimeOut As Integer? = Nothing) As DataTable

    End Function

    Public Overridable Function getdataNotDataSet(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

    End Function

    Public Overridable Function getdataWithTransaction(ByVal sql As String, Optional ByVal TableName As String = "0", Optional ByRef InputConn As SqlConnection = Nothing) As DataTable

    End Function

    Public Overridable Sub Execute(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing, Optional ByVal ChangeTimeoutSeconds As Integer = 30)

    End Sub

    Public Overridable Function ExecuteScalar(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

    End Function

    Public Overridable Sub ExecuteWithImage(ByVal prmSQL As String, ByVal ImageName As String, ByVal ImageByte As Byte())

    End Sub

    Public Overridable Function ExecuteReturnAutoId(ByVal prmSQL As String) As Integer

    End Function

    Public Overridable Sub ExecuteSeries(ByVal prmSQL As String, Optional ByVal needCloseConnection As Boolean = False)

    End Sub

    Public Overridable Function ExecuteSeriesWithReturnAutoId(ByVal prmSQL As String, Optional ByVal needCloseConnection As Boolean = False) As Integer

    End Function

    Public Overridable Sub RollbackTran()

    End Sub

    Public Overridable Function GetSingleValue(ByVal sql As String) As String

    End Function

    Public Overridable Function GetSingleValueByTransposeAllRows(ByVal sql As String) As String

    End Function

    Public Overridable Function ExecuteWithCommandBuilder(ByVal DtResult As DataTable) As Boolean

    End Function

    Public Overridable Sub OpenWithTransection(Optional ByRef InputConn As SqlConnection = Nothing)

    End Sub

    Public Overridable Sub ExecuteWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing)

    End Sub

    Public Overridable Function ExecuteScalarWithTransection(ByVal prmSQL As String, Optional ByRef InputConn As SqlConnection = Nothing) As String

    End Function

    Public Overridable Sub CommitTransection(Optional ByRef InputConn As SqlConnection = Nothing)

    End Sub

    Public Overridable Sub RollbackTransection(Optional ByRef InputConn As SqlConnection = Nothing)

    End Sub

    Public Overridable Function getdataset(ByVal sql As String, ByRef ds As DataSet, ByVal tablename As String) As System.Data.DataSet

    End Function

    Public Overridable Function getdataset(ByVal sql As String) As System.Data.DataSet

    End Function

    Public Overridable Function GetTableSchema(ByVal TableName As String) As DataTable

    End Function

    Public Overridable Function getdataWithSchema(ByVal sql As String, Optional ByVal TableName As String = "0") As DataTable

    End Function

    Public Overridable Sub UnActive(ByVal Table As String, ByVal field As String, ByVal id As Integer)
        Dim sql As String = "UPDATE " & Table & " SET IsActive = 0 Where " & field & " = " & id & ""
        Execute(sql)
    End Sub

    Public Overridable Function ExecInsertSqlReturnNewID(Optional ByVal InputConn As SqlConnection = Nothing) As String

    End Function


    'Public Overridable Sub FillDDL(ByVal strSql As String, ByRef ddlObj As Telerik.WinControls.UI.RadDropDownList, ByVal displayColumn As String, ByVal idColumn As String, ByVal needClear As Boolean)
    '    If needClear Then ddlObj.DataSource = Nothing
    '    Dim dt As New DataTable
    '    dt = getdata(strSql)
    '    ddlObj.DisplayMember = displayColumn
    '    ddlObj.ValueMember = idColumn
    '    ddlObj.DataSource = dt
    '    ddlObj.Tag = -1
    'End Sub

    ''' <summary>
    ''' อ่านไฟล์ทีละบรรทัด พักไว้ให้ครบ 100 บรรทัด แล้วส่งไป execute ทีละก้อน 100 บรรทัด 
    ''' ซึ่งไฟล์จะต้องเป็นบรรทัดละ 1 คำสั่ง, ปิดท้ายบรรทัดด้วย ; เสมอทุกบรรทัด
    ''' </summary>
    ''' <param name="filePath">ต้องเป็น full path to File name เลย</param>
    ''' <returns>จำนวนวินาทีที่ใช้ไป</returns>
    ''' <remarks></remarks>
    Public Overridable Function ExecuteLineFromFile(ByVal filePath As String) As Integer
        'filepath = "D:\Development\Alpha\BudgetBook55Server\trunk\source\SQLScript\script_schoolinformation.sql"
        Dim fileIn As New IO.StreamReader(filePath)
        Dim strData As New System.Text.StringBuilder()

        Dim lngCount As Long = 1
        Dim startDate As Date = DateTime.Now
        While (Not (fileIn.EndOfStream))
            strData.Append(fileIn.ReadLine())
            'Debug.Print(lngCount & ": " & strData)

            'รันทีละ 100 บรรทัด, จะได้เร็วขึ้นหน่อย
            If (lngCount Mod 100) = 0 Then
                ExecuteSeries(strData.ToString())
                strData.Clear()
            End If

            lngCount = lngCount + 1

        End While
        'กรณีมีบรรทัดที่เหลือเศษในไฟล์จะโดน exec ด้วยบรรทัดนี้
        ExecuteSeries(strData.ToString(), True)
        ExecuteLineFromFile = DateDiff(DateInterval.Second, startDate, DateTime.Now)
    End Function
    ''' <summary>
    ''' ใช้สำหรับการแทนที่ค่า (replace) ตัวอักษร ก่อนจะส่งลงไป save, execute, etc กับ DB Server เช่น SQLServer จะส่ง ' ไป save ไม่ได้ จะทำให้ error ทันที
    ''' </summary>
    ''' <param name="inString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function CleanString(ByVal inString As String) As String

    End Function
End Class


