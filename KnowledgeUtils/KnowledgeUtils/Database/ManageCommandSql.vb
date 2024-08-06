Imports System.Text

Namespace Database.Command

    ''' <summary>
    ''' คลาสช่วยสร้าง Command Sql
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageCommandSql
        Dim DateList As New List(Of Object)

#Region "Property"

        Private _BaseType As EnDatabaseType
        Public ReadOnly Property BaseType() As EnDatabaseType
            Get
                Return _BaseType
            End Get
        End Property

        ''' <summary>
        ''' ถ้าตั้งค่า True หมายถึงไม่ต้องมีฟิว key ในประโยคที่จะ gen แต่ถ้าเป็น false จะมีในประโยคคำสั่ง
        ''' </summary>
        ''' <remarks></remarks>
        Private _AutoID As Boolean = True
        Public Property AutoID() As Boolean
            Get
                Return _AutoID
            End Get
            Set(ByVal value As Boolean)
                _AutoID = value
            End Set
        End Property

#End Region

        Sub New(ByVal Base As EnDatabaseType)
            _BaseType = Base
        End Sub

        ''' <summary>
        ''' ระบุเพิ่มเติมให้รู้ว่า ฟิววันที่จะมีการสร้างคำสั่งให้สร้างแบบไหน Default จะนำค่าจาก RowValue มาใช้เลย
        ''' </summary>
        ''' <param name="FieldName">ชื่อฟิว</param>
        ''' <param name="DateStyle">รูปแบบ</param>
        ''' <remarks></remarks>
        Public Sub AddFieldDateModifyStyle(ByVal FieldName As String, ByVal DateStyle As EnDateStyle)
            DateList.Add(New With {.FieldName = FieldName, .DateStyle = DateStyle})
        End Sub

        ''' <summary>
        ''' สร้างคำสั่ง Sql Command
        ''' </summary>
        ''' <param name="TableStructure">รูปแบบฟิวต่างๆโดยส่งเข้ามาในรูปแบบ DataTable</param>
        ''' <param name="RowValue">ค่าที่จะนำมาสร้างคำสั่งโดยส่งเข้ามาในรูปแบบ DataRow</param>
        ''' <param name="Action">ประเภท Command</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCommandSql(ByVal TableStructure As DataTable, ByVal RowValue As DataRow, ByVal Action As EnActionType) As String
            Dim Command As String = ""
            Dim Keys = TableStructure.PrimaryKey
            If Keys.Count > 0 Then
                Command = GenerateCommand(TableStructure, Keys, RowValue, Action)
            End If
            Return Command
        End Function

        ''' <summary>
        ''' สร้างคำสั่ง Sql Command
        ''' </summary>
        ''' <param name="TableStructure">รูปแบบฟิวต่างๆโดยส่งเข้ามาในรูปแบบ DataTable</param>
        ''' <param name="RowValue">ค่าที่จะนำมาสร้างคำสั่งโดยส่งเข้ามาในรูปแบบ DataRow</param>
        ''' <param name="Action">ประเภท Command</param>
        ''' <param name="TableName">ส่งชื่อมาในกรณีที่ไม่แน่ใจว่าจะทำงานถูกมั้ยส่งมาไว้ก่อน</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCommandSql(ByVal TableStructure As DataTable, ByVal RowValue As DataRow, ByVal Action As EnActionType, ByVal TableName As String) As String
            Dim Command As String = ""
            Dim Keys = TableStructure.PrimaryKey
            If Keys.Count > 0 Then
                Command = GenerateCommand(TableStructure, Keys, RowValue, Action, TableName)
            End If
            Return Command
        End Function

        ''' <summary>
        ''' สร้างคำสั่ง Sql Command
        ''' </summary>
        ''' <param name="TableStructure">ปแบบฟิวต่างๆโดยส่งเข้ามาในรูปแบบ DataTable</param>
        ''' <param name="PrimaryKey">ในกรณีที่ TableStructure ไม่ได้มีการระบุฟิว Key ให้มาระบุที่นิแทน</param>
        ''' <param name="RowValue">ค่าที่จะนำมาสร้างคำสั่งโดยส่งเข้ามาในรูปแบบ DataRow</param>
        ''' <param name="Action">ประเภท Command</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function CreateCommandSql(ByVal TableStructure As DataTable, PrimaryKey As String(), ByVal RowValue As DataRow, ByVal Action As EnActionType) As String
            Dim Command As String = ""
            If PrimaryKey.Count > 0 Then
                Dim Keys As New List(Of DataColumn)
                For index = 0 To PrimaryKey.Length - 1
                    Dim Col As DataColumn = (From c In TableStructure.Columns Where c.ColumnName.ToString.ToUpper = PrimaryKey(index).ToUpper).SingleOrDefault
                    If Col Is Nothing Then
                        Return ""
                    End If
                    Keys.Add(Col)
                Next
                Command = GenerateCommand(TableStructure, Keys.ToArray, RowValue, Action)
            End If
            Return Command
        End Function

        Private Function GenerateCommand(ByVal TableStructure As DataTable, ByVal PrimaryKey As DataColumn(), ByVal RowValue As DataRow, ByVal Action As EnActionType, Optional ByVal _TableName As String = "") As String
            Dim Command As String = ""
            Dim Keys = PrimaryKey
            Select Case Action
                Case EnActionType.Insert
                    Command = "INSERT INTO {0} ({1}) VALUES({2});"
                    'ไม่รู้ทำไม sqlserver TableStructure.TableName ไม่คืนชื่อไม่รู้
                    Dim TableName As String = If(_TableName = "", TableStructure.TableName, _TableName)
                    Dim FieldPart As String = ""
                    Dim ValuePart As String = ""

                    Dim Cols = TableStructure.Columns
                    For Each Col As DataColumn In Cols
                        If ((Not Keys.Contains(Col)) OrElse (Keys.Contains(Col) AndAlso Not AutoID)) And Col.AutoIncrement = False Then
                            '(ต้องไมใช่ pamaryKey) หรือ (ถ้าเป็น pamaryKey  แต่ set AutoID = false) และ ต้องไม่ต้องค่าแบบเพิ่มค่าอัตโนมัติ
                            FieldPart &= Col.ColumnName & ","
                            If RowValue.IsNull(Col.ColumnName) Then
                                'รูปแบบ null
                                ValuePart &= "NULL,"
                            ElseIf Col.DataType.Name = "String" OrElse Col.DataType.Name = "Guid" OrElse Col.DataType.Name = "TimeSpan" Then
                                'รูปแบบ String, Guid
                                ValuePart &= "'" & RowValue.Item(Col.ColumnName).ToString.Replace("'", "''") & "',"
                            ElseIf Col.DataType.Name = "DateTime" Then
                                'รูปแบบ DateTime
                                Dim DatePart As String = ""
                                Select Case BaseType
                                    Case EnDatabaseType.Sqlite
                                        DatePart = "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Col.ColumnName)) & "',"
                                        Dim Style = (From d In DateList Where d.FieldName.ToString.ToUpper = Col.ColumnName.ToUpper).SingleOrDefault
                                        If Style IsNot Nothing Then
                                            If Style.DateStyle = EnDateStyle.ByNow Then
                                                DatePart = "datetime('now','localtime'),"
                                            End If
                                        End If
                                    Case EnDatabaseType.SqlServer
                                        DatePart = "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Col.ColumnName)) & "',"
                                        Dim Style = (From d In DateList Where d.FieldName.ToString.ToUpper = Col.ColumnName.ToUpper).SingleOrDefault
                                        If Style IsNot Nothing Then
                                            If Style.DateStyle = EnDateStyle.ByNow Then
                                                DatePart = "dbo.GetThaiDate(),"
                                            End If
                                        End If
                                End Select
                                ValuePart &= DatePart
                            ElseIf Col.DataType.Name = "Boolean" Then
                                'รูปแบบ Boolean
                                ValuePart &= If(RowValue.Item(Col.ColumnName), 1, 0) & ","
                            Else
                                'รูปแบบ อื่นๆ
                                ValuePart &= RowValue.Item(Col.ColumnName).ToString & ","
                            End If
                        End If
                    Next
                    FieldPart = Left(FieldPart, FieldPart.Length - 1)
                    ValuePart = Left(ValuePart, ValuePart.Length - 1)
                    Command = String.Format(Command, TableName, FieldPart, ValuePart)

                Case EnActionType.Update
                    Command = "UPDATE {0} SET {1} WHERE {2};"
                    Dim TableName As String = TableStructure.TableName
                    Dim SetPart As String = ""
                    Dim WherePart As String = ""

                    '<<< Set Part
                    Dim Cols = TableStructure.Columns
                    For Each Col As DataColumn In Cols
                        If Not Keys.Contains(Col) And Col.AutoIncrement = False Then
                            SetPart &= Col.ColumnName & "="
                            If RowValue.IsNull(Col.ColumnName) Then
                                SetPart &= "NULL,"
                            ElseIf Col.DataType.Name = "String" OrElse Col.DataType.Name = "Guid" OrElse Col.DataType.Name = "TimeSpan" Then
                                SetPart &= "'" & RowValue.Item(Col.ColumnName).ToString.Replace("'", "''") & "',"
                            ElseIf Col.DataType.Name = "DateTime" Then
                                Dim DatePart As String = ""
                                Select Case BaseType
                                    Case EnDatabaseType.Sqlite
                                        DatePart = "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Col.ColumnName)) & "',"
                                        Dim Style = (From d In DateList Where d.FieldName.ToString.ToUpper = Col.ColumnName.ToUpper).SingleOrDefault
                                        If Style IsNot Nothing Then
                                            If Style.DateStyle = EnDateStyle.ByNow Then
                                                DatePart = "datetime('now','localtime'),"
                                            End If
                                        End If
                                    Case EnDatabaseType.SqlServer
                                        DatePart = "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Col.ColumnName)) & "',"
                                        Dim Style = (From d In DateList Where d.FieldName.ToString.ToUpper = Col.ColumnName.ToUpper).SingleOrDefault
                                        If Style IsNot Nothing Then
                                            If Style.DateStyle = EnDateStyle.ByNow Then
                                                DatePart = "dbo.GetThaiDate(),"
                                            End If
                                        End If
                                End Select
                                SetPart &= DatePart
                            ElseIf Col.DataType.Name = "Boolean" Then
                                SetPart &= If(RowValue.Item(Col.ColumnName), 1, 0) & ","
                            Else
                                SetPart &= RowValue.Item(Col.ColumnName).ToString & ","
                            End If
                        End If
                    Next

                    '<<< Where Part
                    For Each Key As DataColumn In Keys
                        WherePart &= Key.ColumnName & "="
                        If Key.DataType.Name = "String" OrElse Key.DataType.Name = "Guid" Then
                            WherePart &= "'" & RowValue.Item(Key.ColumnName).ToString.Replace("'", "''") & "' AND "
                        ElseIf Key.DataType.Name = "DateTime" Then
                            Select Case BaseType
                                Case EnDatabaseType.Sqlite
                                    WherePart &= "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Key.ColumnName)) & "' AND "
                                Case EnDatabaseType.SqlServer
                                    WherePart &= "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Key.ColumnName)) & "' AND "
                            End Select
                        ElseIf Key.DataType.Name = "Boolean" Then
                            WherePart &= If(RowValue.Item(Key.ColumnName), 1, 0) & " AND "
                        Else
                            WherePart &= RowValue.Item(Key.ColumnName).ToString & " AND "
                        End If
                    Next
                    SetPart = Left(SetPart, SetPart.Length - 1)
                    WherePart = Left(WherePart, WherePart.Length - 4)
                    Command = String.Format(Command, TableName, SetPart, WherePart)

                Case EnActionType.UpdateIsActiveFalse
                    'ไว้ใช้สำหรับให้ update แต่ column พวก isactive ให้เป็น 0
                    Command = "UPDATE {0} SET {1} WHERE {2};"
                    Dim TableName As String = TableStructure.TableName
                    Dim SetPart As String = ""
                    Dim WherePart As String = ""

                    '<<< Set Part
                    Dim Cols = TableStructure.Columns
                    For Each Col As DataColumn In Cols
                        If Col.ColumnName.ToUpper.IndexOf("ISACTIVE") > -1 Then
                            SetPart &= Col.ColumnName & "="
                            If Col.DataType.Name = "Boolean" Then
                                SetPart &= 0 & ","
                            End If
                        End If
                    Next

                    '<<< Where Part
                    For Each Key As DataColumn In Keys
                        WherePart &= Key.ColumnName & "="
                        If Key.DataType.Name = "String" OrElse Key.DataType.Name = "Guid" Then
                            WherePart &= "'" & RowValue.Item(Key.ColumnName).ToString.Replace("'", "''") & "' AND "
                        ElseIf Key.DataType.Name = "DateTime" Then
                            Select Case BaseType
                                Case EnDatabaseType.Sqlite
                                    WherePart &= "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Key.ColumnName)) & "' AND "
                                Case EnDatabaseType.SqlServer
                                    WherePart &= "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Key.ColumnName)) & "' AND "
                            End Select
                        ElseIf Key.DataType.Name = "Boolean" Then
                            WherePart &= If(RowValue.Item(Key.ColumnName), 1, 0) & " AND "
                        Else
                            WherePart &= RowValue.Item(Key.ColumnName).ToString & " AND "
                        End If
                    Next
                    SetPart = Left(SetPart, SetPart.Length - 1)
                    WherePart = Left(WherePart, WherePart.Length - 4)
                    Command = String.Format(Command, TableName, SetPart, WherePart)

                Case EnActionType.Delete
                    Command = "DELETE FROM {0} WHERE {1};"
                    Dim TableName As String = TableStructure.TableName
                    Dim WherePart As String = ""

                    '<<< Where Part
                    For Each Key As DataColumn In Keys
                        WherePart &= Key.ColumnName & "="
                        If Key.DataType.Name = "String" OrElse Key.DataType.Name = "Guid" Then
                            WherePart &= "'" & RowValue.Item(Key.ColumnName).ToString.Replace("'", "''") & "' AND "
                        ElseIf Key.DataType.Name = "DateTime" Then
                            Select Case BaseType
                                Case EnDatabaseType.Sqlite
                                    WherePart &= "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Key.ColumnName)) & "' AND "
                                Case EnDatabaseType.SqlServer
                                    WherePart &= "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Key.ColumnName)) & "' AND "
                            End Select
                        ElseIf Key.DataType.Name = "Boolean" Then
                            WherePart &= If(RowValue.Item(Key.ColumnName), 1, 0) & " AND "
                        Else
                            WherePart &= RowValue.Item(Key.ColumnName).ToString & " AND "
                        End If
                    Next
                    WherePart = Left(WherePart, WherePart.Length - 4)
                    Command = String.Format(Command, TableName, WherePart)

                Case EnActionType.SelectData
                    Command = "SELECT * FROM {0} WHERE {1};"
                    Dim TableName As String = TableStructure.TableName
                    Dim WherePart As String = ""

                    '<<< Where Part
                    For Each Key As DataColumn In Keys
                        WherePart &= Key.ColumnName & "="
                        If Key.DataType.Name = "String" OrElse Key.DataType.Name = "Guid" Then
                            WherePart &= "'" & RowValue.Item(Key.ColumnName).ToString.Replace("'", "''") & "' AND "
                        ElseIf Key.DataType.Name = "DateTime" Then
                            Select Case BaseType
                                Case EnDatabaseType.Sqlite
                                    WherePart &= "'" & KnowledgeUtils.Database.Sqlite.ManageSqlite.ConvertDateTime(RowValue.Item(Key.ColumnName)) & "' AND "
                                Case EnDatabaseType.SqlServer
                                    WherePart &= "'" & KnowledgeUtils.Database.SqlServer.ManageSqlServer.ConvertDateTimeToString(RowValue.Item(Key.ColumnName)) & "' AND "
                            End Select
                        ElseIf Key.DataType.Name = "Boolean" Then
                            WherePart &= If(RowValue.Item(Key.ColumnName), 1, 0) & " AND "
                        Else
                            WherePart &= RowValue.Item(Key.ColumnName).ToString & " AND "
                        End If
                    Next
                    WherePart = Left(WherePart, WherePart.Length - 4)
                    Command = String.Format(Command, TableName, WherePart)

            End Select
            Return Command
        End Function

    End Class

    Public Enum EnDatabaseType
        SqlServer
        Sqlite
    End Enum

    Public Enum EnDateStyle
        ByNow
        ByValue
    End Enum

    Public Enum EnActionType
        Insert
        Update
        UpdateIsActiveFalse
        Delete
        SelectData
    End Enum

    Public Class TableStructure

        Private _FieldName As String
        Public Property FieldName() As String
            Get
                Return _FieldName
            End Get
            Set(ByVal value As String)
                _FieldName = value
            End Set
        End Property

        Private _FieldType As Type
        Public Property FieldType() As Type
            Get
                Return _FieldType
            End Get
            Set(ByVal value As Type)
                _FieldType = value
            End Set
        End Property

    End Class

End Namespace



