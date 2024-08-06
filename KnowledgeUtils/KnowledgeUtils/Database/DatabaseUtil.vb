Imports KnowledgeUtils.System
Imports System.Data.Common

Namespace Database

    ''' <summary>
    ''' คลาสช่วยเรื่อง Sql Command ต้องนำไป Inherit
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class DatabaseUtil
        ''' <summary>
        ''' Defualt ที่ false ถ้ามีการกำหนดเป็น true จะหมายถึงไม่มีการตัด WHERE ไม่ว่าจะคำสั่ง ApplySqlPart จะไม่มี parameter เลยก็ตาม
        ''' </summary>
        ''' <remarks></remarks>
        Public LockWhere As Boolean = False
        ''' <summary>
        ''' ไว้ใช้เก็บเงื่อนไขในประโยค
        ''' </summary>
        ''' <remarks></remarks>
        Protected ListSqlPart As New ListSqlPart

#Region "Property"

        ''' <summary>
        ''' เก็บประโยค SQL หลัก
        ''' </summary>
        ''' <remarks></remarks>
        Private _MainSql As String
        Public Property MainSql() As String
            Get
                Return _MainSql
            End Get
            Set(ByVal value As String)
                _MainSql = value
            End Set
        End Property

        ''' <summary>
        ''' คืนค่าจำนวนของ SqlPart ที่ถูกเพิ่มเข้าไป ควรจะทำการ ApplySqlPart หรือ ApplyTagWithValue ก่อนเพราะจะทำให้เกิดจำนวน SqlPart
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CountSqlPart() As Integer
            Get
                Return ListSqlPart.Count
            End Get
        End Property

        ''' <summary>
        ''' กำหนดรูปแบบสัญญาลักษณ์ของ Parameter
        ''' </summary>
        ''' <remarks></remarks>
        Private _Symbol As EnSymbolParameter = EnSymbolParameter.Tag
        Public Property Symbol() As EnSymbolParameter
            Get
                Return _Symbol
            End Get
            Set(ByVal value As EnSymbolParameter)
                _Symbol = value
            End Set
        End Property

#End Region

        Public Sub DebugUtil()
            '<<< debug ดูค่า
            Dim L = ListSqlPart.Where(Function(q) q.Value IsNot Nothing).ToList
            Dim MockCondition(L.Count - 1) As PropertySqlPart
            For index = 0 To L.Count - 1
                MockCondition(index) = New PropertySqlPart With {.SqlCondition = L(index).SqlCondition, _
                                                                 .Value = L(index).Value}
            Next

            Trace.WriteLine(MainSql)
            Trace.Indent()
            For Each i In MockCondition
                Trace.WriteLine(i.SqlCondition & "," & i.Value.ToString)
            Next

            'เตรียมคิวรี่แบบเต็มเผื่อใช้สำหรับเช็คได้เลย
            Dim MockMainSql = MainSql
            Trace.Unindent()
            Trace.WriteLine("")
            For Each i In MockCondition
                If i.Value.GetType.Name = "String" Then
                    i.Value = "'" & i.Value & "'"
                End If
            Next
            If Symbol = EnSymbolParameter.Add Then
                For i = 0 To MockCondition.Length - 1
                    MockMainSql = Strings.Replace(MockMainSql, MockCondition(i).SqlCondition, "{" & i.ToString & "}")
                Next
            End If
            Trace.WriteLine(String.Format(MockMainSql, MockCondition.Select(Function(q) q.Value).ToArray))
            Trace.WriteLine("==========")
            Trace.Flush()
        End Sub

        ''' <summary>
        ''' คืนค่า Value ของเงื่อนไขทั้งหมด
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetValueListSqlPart() As IEnumerable(Of Object)
            Return ListSqlPart.GetValue.Where(Function(q) q IsNot Nothing)
        End Function

        ''' <summary>
        ''' คืนค่า SqlCondition, Value ของเงื่อนไขทั้งหมด
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllListSqlPart() As IEnumerable(Of Object)
            Return ListSqlPart.GetAll.Where(Function(q) q.PropertyValue IsNot Nothing)
        End Function

        ''' <summary>
        ''' Add Parameter ของ DbCommand
        ''' </summary>
        ''' <param name="Params"></param>
        ''' <remarks></remarks>
        Public Sub MapDbCommandParameters(ByVal Params As DbParameterCollection)
            For Each Param As DbParameter In Params
                ListSqlPart.Add(New PropertySqlPart With {.SqlCondition = Param.ParameterName, .Value = Param.Value})
            Next
        End Sub

        ''' <summary>
        ''' ฟั่งชั่นปรับ Tag ที่อยู่ใน MainSql ให้ใส่ค่า String ที่ส่งเข้ามาไปแปะใส่ MainSql ช่วยในกรณีที่ต้องการแทนที่ Tag ด้วย String ล้วนๆ ประมาณว่า String ได้ปรับรูปมาแล้ว
        ''' ยกตัวอย่าง กรณี Field1 IN ('a','b','c') กรณีนี้ควรใช้แบบนี้
        ''' </summary>
        ''' <param name="TagName">ชื่อ Tag ที่จะโดนแปะ</param>
        ''' <param name="TextValue">ค่าที่จะไปแปะ tag</param>
        ''' <param name="FieldData">ถ้า FieldData = Nothing ค่าของ TextValue = "" โดยอัตโนมัติ</param>
        ''' <param name="WordLink">ตัวเชื่อมถ้าต้องการให้มีจะไว้ที่ข้างหน้า</param>
        ''' <remarks></remarks>
        Public Sub ApplyTextPart(ByVal TagName As String, ByVal TextValue As String, Optional ByVal FieldData As Object = "", Optional ByVal WordLink As String = "")
            TagName = "{" & TagName & "}"
            If FieldData Is Nothing Then
                'ถ้าไม่มีข้อมูลปรับ Tag ให้หายไปจาก MainSql
                If Strings.InStr(MainSql, TagName) > 0 Then
                    '<<< ปรับตัว TagName ที่อยู่ที่ MainSql
                    MainSql = Strings.Replace(MainSql, TagName, "")
                End If
            Else
                'ถ้ามีข้อมูลปรับรูปที่ MainSql และ เก็บใส่ ListSqlPart
                If Strings.InStr(MainSql, TagName) > 0 Then
                    '<<< ปรับ MainSql
                    MainSql = Strings.Replace(MainSql, TagName, WordLink & " " & TextValue)
                    ListSqlPart.Add(New PropertySqlPart With {.SqlCondition = "", .Value = Nothing})
                End If
            End If
        End Sub

        ''' <summary>
        ''' เหมาะกับฟิวเงื่อนไขที่รอรับค่าแน่นอนคือมีเงื่อนไขนี้แน่นอน
        ''' ที่ MainSql เขียน field1 = {a} ตอนจะระบุค่าเรียก MapTagWithValue("a","test")
        ''' </summary>
        ''' <param name="TagName">ชื่อ Tag ที่ระบุไว้ที่ MainSql</param>
        ''' <param name="TagValue">ค่าที่จะใส่ให้ Tag ตัวนี้</param>
        ''' <remarks></remarks>
        Public Sub ApplyTagWithValue(ByVal TagName As String, ByVal TagValue As Object)
            'ฟังชั่นนี้ปรับรูปที่  MainSql ได้เลย
            TagName = "{" & TagName & "}"
            If Strings.InStr(MainSql, TagName) > 0 Then
                '<<< ปรับ MainSql ใหม่
                Select Case Symbol
                    Case EnSymbolParameter.Tag
                        MainSql = Strings.Replace(MainSql, TagName, "{" & ListSqlPart.Where(Function(q) q.Value IsNot Nothing).Count.ToString & "}")
                    Case EnSymbolParameter.Add
                        MainSql = Strings.Replace(MainSql, TagName, "@" & ListSqlPart.Where(Function(q) q.Value IsNot Nothing).Count.ToString)
                End Select
                '<<< เก็บใส่ List
                Select Case Symbol
                    Case EnSymbolParameter.Tag
                        ListSqlPart.Add(New PropertySqlPart With {.SqlCondition = "", .Value = TagValue})
                    Case EnSymbolParameter.Add
                        ListSqlPart.Add(New PropertySqlPart With {.SqlCondition = "@" & ListSqlPart.Where(Function(q) q.Value IsNot Nothing).Count.ToString, .Value = TagValue})
                End Select
            End If
        End Sub

        ''' <summary>
        ''' ใส่เงื่อนใขที่คลาส SqlFilter เข้าไปใน MainSql
        ''' </summary>
        ''' <param name="TagName"></param>
        ''' <param name="SP"></param>
        ''' <remarks></remarks>
        Public Sub ApplySqlPart(ByVal TagName As String, ByVal SP As SqlPart)
            '<<< ฟังชั่นนี้ทำการปรับ SqlCondition ให้ Tag ถูกต้องเรียงตามตัวเลขก่อนแล้วค่อยไปแปะทับ TagName ที่อยุ่ใน MainSql 
            If Strings.InStr(MainSql, TagName) > 0 Then
                '<<< ทำให้สิ่งที่ add เข้าไปที่ SqlPart อยู่ในตัวแปร WhereSql เผื่อไปปรับ MainSql ใหม่
                Dim WhereSql As String = ""
                Dim i As Integer = ListSqlPart.Where(Function(q) q.Value IsNot Nothing).Count
                For Each Item In SP.ListPart
                    'ตัดเฉพาะ Tag มาเผื่อจะเอามาตัดทิ้งแล้วจะทำเป็น Tag ตัวเลขที่ทำการเรียงเองไปใส่แทน
                    Dim Rx As New Text.RegularExpressions.Regex("{[^{}]*}")
                    Dim MatchTag = Rx.Match(Item.SqlCondition)
                    Select Case Symbol
                        Case EnSymbolParameter.Tag
                            WhereSql &= " " & Strings.Replace(Item.SqlCondition, MatchTag.Value, "{" & i.ToString & "} ") & (New EnumJoinType).GetText(Item.JoinType)
                        Case EnSymbolParameter.Add
                            WhereSql &= " " & Strings.Replace(Item.SqlCondition, MatchTag.Value, "@" & i.ToString & " ") & (New EnumJoinType).GetText(Item.JoinType)
                            Item.SqlCondition = "@" & i.ToString
                    End Select
                    SP.joinType = Item.JoinType
                    i += 1
                Next
                If SP.ListPart.Count > 0 Then
                    Dim LenJoinType = Len((New EnumJoinType).GetText(SP.joinType)) + 1
                    WhereSql = Left(WhereSql, Len(WhereSql) - LenJoinType)
                End If

                'เก็บ ListPart ของ Class SqlPart ใส่เข้าไปใน ListSqlPart ของ Class DatabaseManager
                ListSqlPart.AddRange(SP.ListPart)

                If SP.ListPart.Count > 0 AndAlso SP.LinkString <> String.Empty Then
                    'ถ้ามีการกำหนดตัว Link ให้มาต่อ Link ก่อนที่จะทำการ Replace Tag
                    MainSql = Strings.Replace(MainSql, "{" & TagName & "}", " " & SP.LinkString & " {" & TagName & "}")
                End If
                '<<< ปรับ MainSql ใหม่โดยการนำ WhereSql ไปแทน TagName
                MainSql = Strings.Replace(MainSql, "{" & TagName & "}", WhereSql)
            End If
        End Sub

    End Class

#Region "Type"

    ''' <summary>
    ''' ใช้กำหนดให้กับ Class SqlFilter เป็นจะใช้ Join แบบไหน
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EnJoinType
        JoinByAnd
        JoinByOr
        JoinByComma
    End Enum

    Public Class EnumJoinType
        Inherits EnumRegister

        Public Sub New()
            AddItem(EnJoinType.JoinByAnd, "AND")
            AddItem(EnJoinType.JoinByOr, "OR")
            AddItem(EnJoinType.JoinByComma, ",")
        End Sub
    End Class

    Public Class PropertySqlPart
        Public SqlCondition As String
        Public Value As Object
        Public JoinType As EnJoinType
    End Class

    ''' <summary>
    ''' เป็น List Type ประเภท PropertySqlFilter ไว้ใช้คู่กับ ClassSqlFilter
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ListSqlPart
        Inherits List(Of PropertySqlPart)

        Function GetCondition() As IEnumerable(Of String)
            Dim l As New List(Of String)
            For Each i In Me
                l.Add(i.SqlCondition)
            Next
            Return l.AsEnumerable
        End Function

        Function GetValue() As IEnumerable(Of Object)
            Dim l As New List(Of Object)
            For Each i In Me
                l.Add(i.Value)
            Next
            Return l.AsEnumerable
        End Function

        Function GetAll() As ListProperty
            Dim l As New ListProperty
            For Each i In Me
                l.AddProperty(i.SqlCondition, i.Value)
            Next
            Return l
        End Function

    End Class

    Public Enum EnSymbolParameter
        Add
        Tag
    End Enum

#End Region

    ''' <summary>
    ''' คลาสเก็บเงื่อนไขของประโยค SQL ในรูป Lst String คู่กับ Object
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SqlPart
        Public LinkString As String 'ใช้เก็บตัวเชื่อมตอนรวมเข้า SQL MAIN
        Public ListPart As New ListSqlPart

        Private _joinType As EnJoinType
        Public Property joinType() As EnJoinType
            Get
                Return _joinType
            End Get
            Set(ByVal value As EnJoinType)
                _joinType = value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="WordLink">ตัวเชื่อมถ้าต้องการให้มีจะไว้ที่ข้างหน้า</param>
        ''' <remarks></remarks>
        Public Sub New(Optional ByVal WordLink As String = "")
            LinkString = WordLink
            'Default AND
            joinType = EnJoinType.JoinByAnd
        End Sub

        ''' <summary>
        ''' เพิ่มเงื่อนไข Sql ถ้า Value เป็น Nothing จะไม่ถูกเพิ่มเข้ามา
        ''' </summary>
        ''' <param name="SqlCondition">เงื่อนไขหนึ่งเงื่อนไขเช่น Field1={0}</param>
        ''' <param name="Value">ค่าที่จะใส่ลงไปใน Tag</param>
        ''' <remarks></remarks>
        Public Sub AddPart(ByVal SqlCondition As String, ByVal Value As Object, Optional ByVal JoinType As EnJoinType = EnJoinType.JoinByAnd)
            'todo รับ param ได้ก็ดี
            If Value IsNot Nothing Then
                ListPart.Add(New PropertySqlPart With {.SqlCondition = SqlCondition, .Value = Value, .JoinType = JoinType})
            End If
        End Sub

    End Class

End Namespace


