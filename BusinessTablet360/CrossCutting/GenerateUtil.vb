Imports System.IO
Imports System.Xml
Imports System.Text
Imports ICSharpCode.SharpZipLib.Zip
Imports System.IO.Compression
Imports KnowledgeUtils

''' <summary>
''' คลาสช่วย Generate XML จาก DataTable
''' </summary>
''' <remarks></remarks>
Public Class GenerateXml
    Dim Root As XElement
    Dim _Version As String
    Dim _VersionType As EnVersionType
    Dim _ClientId As String
    Dim _Description As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Version">ระบุ Version ที่จะทำการสร้าง</param>
    ''' <param name="VersionType"></param>
    ''' <param name="ClientId"></param>
    ''' <param name="Description"></param>
    ''' <remarks></remarks>
    Sub New(ByVal Version As String, ByVal VersionType As EnVersionType, ByVal ClientId As String, ByVal Description As String)
        _Version = Version
        _ClientId = ClientId
        _VersionType = VersionType
        _Description = Description
    End Sub

    ''' <summary>
    ''' เปิดหัว xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRoot() As XElement
        Root = New XElement("Package", New XAttribute("Version", _Version), New XAttribute("VersionType", _VersionType), _
                            New XAttribute("ClientId", _ClientId), New XAttribute("SendTime", Date.Now), New XAttribute("Description", _Description))
        Return Root
    End Function

    ''' <summary>
    ''' เพิ่ม DataTable ที่จะทำการสร้าง XML ในกรณีที่ Dt ที่ส่งมามีชื่อ DataTable แล้วก็ไม่จำเป็นที่จะต้องส่ง TableName มาก็ได้
    ''' </summary>
    ''' <param name="Dt"></param>
    ''' <param name="TableName"></param>
    ''' <param name="MustCreate">บังคับสร้าง XML ต่อให้ไม่มีข้อมูลใน DataTable เผื่อจะนำโครงสร้างมาใช้อย่างเดียว</param>
    ''' <remarks></remarks>
    Public Sub AddDataTable(ByVal Dt As DataTable, Optional ByVal TableName As String = "", Optional ByVal MustCreate As Boolean = False)
        If Root Is Nothing Then
            Root = CreateRoot()
        End If

        If Dt.Rows.Count > 0 OrElse MustCreate Then
            If TableName = "" Then
                TableName = Dt.TableName
            End If
            Dim TableEl = New XElement("Tables", New XAttribute("Name", TableName)) 'สร้าง tag ชื่อ table คลุมเนื้อหา xml ที่ได้จาก datatable

            'Dim DataTableStream As New MemoryStream
            'Dt.WriteXml(DataTableStream, System.Data.XmlWriteMode.WriteSchema, False)
            'DataTableStream.Position = 0
            'Dim ResultEl As XElement = XElement.Load(DataTableStream)
            'Dim xr = XmlReader.Create(DataTableStream)
            'xr.
            'Dim Doc As XDocument = XDocument.Load(xr)

            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Dt.WriteXml(Sw, System.Data.XmlWriteMode.WriteSchema, False)
            Dim Doc As XDocument = XDocument.Parse(Sb.ToString)
            Dim ResultEl = Doc.Elements
            TableEl.Add(ResultEl) 'เพิ่ม xml เนื้อดาต้าเทเบิลเข้า element ชื่อ table
            Root.Add(TableEl) 'เพิ่ม เนื้อทั้งหมดเข้า root
        End If
    End Sub

    Public Sub AddDataTableLimitRow(ByVal Dt As DataTable, Optional ByVal TableName As String = "", Optional ByVal MustCreate As Boolean = False)
        If Root Is Nothing Then
            Root = CreateRoot()
        End If

        If Dt.Rows.Count > 0 OrElse MustCreate Then
            If TableName = "" Then
                TableName = Dt.TableName
            End If

            Dim SkipRow As Integer = 0
            Dim Rows = Dt.AsEnumerable
            Do While SkipRow < Dt.Rows.Count
                Dim DtPart = Rows.Skip(SkipRow).Take(100000).CopyToDataTable
                DtPart.TableName = "0"
                SkipRow += 100000

                Dim TableEl = New XElement("Tables", New XAttribute("Name", TableName)) 'สร้าง tag ชื่อ table คลุมเนื้อหา xml ที่ได้จาก datatable
                Dim Sb As New StringBuilder
                Dim Sw As New StringWriter(Sb)
                DtPart.WriteXml(Sw, System.Data.XmlWriteMode.WriteSchema, False)
                Dim Doc As XDocument = XDocument.Parse(Sb.ToString)
                Dim ResultEl = Doc.Elements
                TableEl.Add(ResultEl) 'เพิ่ม xml เนื้อดาต้าเทเบิลเข้า element ชื่อ table
                Root.Add(TableEl) 'เพิ่ม เนื้อทั้งหมดเข้า root
            Loop
        End If
    End Sub

    'Public Sub AddDataTable1(ByVal Dt As DataTable, Optional ByVal TableName As String = "", Optional ByVal MustCreate As Boolean = False)
    '    If Root Is Nothing Then
    '        Root = CreateRoot()
    '    End If

    '    If Dt.Rows.Count > 0 OrElse MustCreate Then
    '        If TableName = "" Then
    '            TableName = Dt.TableName
    '        End If
    '        Dim TableEl = New XElement("Tables", New XAttribute("Name", TableName)) 'สร้าง tag ชื่อ table คลุมเนื้อหา xml ที่ได้จาก datatable
    '        Dim Sb As New StringBuilder
    '        Dim Sw As New StringWriter(Sb)
    '        Dt.WriteXml(Sw, System.Data.XmlWriteMode.WriteSchema, False)
    '        Dim Doc As XDocument = XDocument.Parse(Sb.ToString)
    '        Dim ResultEl = Doc.Elements
    '        TableEl.Add(ResultEl) 'เพิ่ม xml เนื้อดาต้าเทเบิลเข้า element ชื่อ table
    '        Root.Add(TableEl) 'เพิ่ม เนื้อทั้งหมดเข้า root
    '    End If
    'End Sub

    ''' <summary>
    ''' สร้าง XML File
    ''' </summary>
    ''' <param name="Path">ที่อยู่ที่เก็บไฟล์ระบุชื่อเต็ม</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateXml(ByVal Path As String) As Boolean
        Try
            Dim Doc As New XDocument
            Doc.Add(Root)
            Doc.Save(Path, SaveOptions.DisableFormatting)
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' สร้าง XML File ที่ถูก Zip ใช้ SharpZipLib
    ''' </summary>
    ''' <param name="Path">ที่อยู่ที่เก็บไฟล์ระบุชื่อเต็ม</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CreateXmlZip(ByVal Path As String) As Boolean
        Try
            Dim Doc As New XDocument
            Doc.Add(Root)
            Using Ms As New MemoryStream
                Doc.Save(Ms, SaveOptions.DisableFormatting)
                Using MsOutput As New MemoryStream
                    Using ZipOutput As New ZipOutputStream(MsOutput)
                        Dim Ze As New ZipEntry("Data.xml")
                        With ZipOutput
                            .PutNextEntry(Ze)
                            .Write(Ms.ToArray, 0, Ms.Length)
                            .Finish()
                            .Close()
                        End With

                        Dim Fs As New FileStream(Path, FileMode.Create, FileAccess.Write)
                        Fs.Write(MsOutput.ToArray, 0, MsOutput.ToArray.Length)
                        Fs.Close()
                    End Using
                    MsOutput.Close()
                End Using
                Ms.Close()
            End Using

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' แบบคืนเป็น Byte() ใช้ gzip
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CreateXmlZip(ByVal Source As DataSet) As Byte()
        Try
            Using FileStream As New MemoryStream
                Using ZipStream = New GZipStream(FileStream, CompressionMode.Compress)
                    Source.WriteXml(ZipStream, XmlWriteMode.WriteSchema)
                End Using
                'Dim a As New DataSet
                'Using FileStream1 As New MemoryStream(FileStream.ToArray())
                '    Using ZipStream = New GZipStream(FileStream1, CompressionMode.Decompress)
                '        a.ReadXml(ZipStream, XmlWriteMode.WriteSchema)
                '    End Using
                'End Using
                Return FileStream.ToArray()
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' แบบเขียนใส่ไฟล์ ใช้ gzip
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function CreateXmlZip(ByVal Source As DataSet, ByVal Path As String) As Boolean
        Try
            'Using FileStream = File.Create(Path)
            '    Using ZipStream = New GZipStream(FileStream, CompressionMode.Compress)

            '        Dt.WriteXml(ZipStream, XmlWriteMode.WriteSchema)
            '    End Using
            'End Using

            'Dim Doc As New XDocument
            'Doc.Add(Root)
            'Using Ms As New MemoryStream
            '    Doc.Save(Ms, SaveOptions.DisableFormatting)
            '    Using FileStream = File.Create(Path)
            '        Using ZipStream = New GZipStream(FileStream, CompressionMode.Compress)
            '            Doc.Save(ZipStream, SaveOptions.DisableFormatting)
            '        End Using
            '    End Using
            '    Ms.Close()
            'End Using

            Using FileStream = File.Create(Path)
                Using ZipStream = New GZipStream(FileStream, CompressionMode.Compress)
                    Source.WriteXml(ZipStream, XmlWriteMode.WriteSchema)
                End Using
            End Using

            'Dim a As New DataSet
            'Using FileStream = File.OpenRead(Path)
            '    Using ZipStream = New GZipStream(FileStream, CompressionMode.Decompress)
            '        a.ReadXml(ZipStream, XmlWriteMode.WriteSchema)
            '    End Using
            'End Using

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' สร้างไฟล์ zip ใช้ SharpZipLib
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Path"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateZip(ByVal Source As DataSet, ByVal Path As String) As Boolean
        Try
            Using Ms As New MemoryStream
                Source.WriteXml(Ms, XmlWriteMode.WriteSchema)
                Using MsOutput As New MemoryStream
                    Using ZipOutput As New ZipOutputStream(MsOutput)
                        Dim Ze As New ZipEntry("Data.xml")
                        With ZipOutput
                            .PutNextEntry(Ze)
                            .Write(Ms.ToArray, 0, Ms.Length)
                            .Finish()
                            .Close()
                        End With

                        Dim Fs As New FileStream(Path, FileMode.Create, FileAccess.Write)
                        Fs.Write(MsOutput.ToArray, 0, MsOutput.ToArray.Length)
                        Fs.Close()
                    End Using
                    MsOutput.Close()
                End Using
                Ms.Close()
            End Using

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' สร้าง XML ในรูปแบบ String
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateXmlString() As String
        Try
            Dim Doc As New XDocument
            Doc.Add(Root)
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Dim XmlWriter As New XmlTextWriter(Sw)
            Doc.Save(XmlWriter)
            Return Sb.ToString
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

End Class

''' <summary>
''' คลาสช่วย Generate DataTable จาก XML
''' </summary>
''' <remarks></remarks>
Public Class GenerateDataSet
    Dim Doc As XDocument
    Dim MainDs As New DataSet

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="XmlSource">แหล่งที่เก็บ XML ถ้าเป็น File ให้ระบุชื่อที่อยู่เต็ม</param>
    ''' <param name="XmlType">รูปแบบ XML ที่ส่งเข้ามาเป็น File หรือ เป็น String</param>
    ''' <remarks></remarks>
    Sub New(ByVal XmlSource As String, ByVal XmlType As EnumLoad)
        Try
            If XmlType = EnumLoad.XmlPath Then
                Doc = XDocument.Load(XmlSource)
            ElseIf XmlType = EnumLoad.XmlZipPath Then
                'ตอนหลังใช้ gzip แล้วทำการโหลดใส่ ds แทนเลย
                Using FileStream = File.OpenRead(XmlSource)
                    Using ZipStream = New GZipStream(FileStream, CompressionMode.Decompress)
                        MainDs.ReadXml(ZipStream, XmlWriteMode.WriteSchema)
                    End Using
                End Using

                ''แบบให้ gzip เขียนแตก zip ที่ เอาไฟล์ xml ที่นิ 
                'Dim Zf As New ZipFile(XmlSource)
                'Dim Ze = Zf.GetEntry("Data.xml")
                'Doc = XDocument.Load(Zf.GetInputStream(Ze))
            Else
                Doc = XDocument.Parse(XmlSource)
            End If
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception("โหลดไม่สำเร็จ")
        End Try
    End Sub


    ''' <summary>
    ''' รับรูปแบบ XDocument
    ''' </summary>
    ''' <param name="DocSouce"></param>
    ''' <remarks></remarks>
    Sub New(ByVal DocSouce As XDocument)
        Try
            Doc = DocSouce
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Throw New Exception("โหลดไม่สำเร็จ")
        End Try
    End Sub

    ''' <summary>
    ''' คืนค่า ชื่อ Table ทั้งหมดที่อยู่ใน XML
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTablesName() As IEnumerable(Of String)
        Dim Tables = Doc.Root.Elements("Tables").Attributes("Name").Select(Function(x) New String(x.Value))
        Return Tables
    End Function

    ''' <summary>
    ''' คืนค่าเวอร์ชั่นของ XML
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVersion() As String
        Try
            Return Doc.Root.Attribute("Version").Value
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่าประเภทข้อมูล
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVersionType() As EnVersionType
        Try
            Return Doc.Root.Attribute("VersionType").Value
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่ารหัสเครื่องลูกข่าย
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClientId() As String
        Try
            Return Doc.Root.Attribute("ClientId").Value
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่ารายละเอียด
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDescription() As String
        Try
            Return Doc.Root.Attribute("Description").Value
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่าวันเวลาในของไฟล์ XML ที่ส่งมา
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSendTime() As DateTime
        Try
            Return Doc.Root.Attribute("SendTime").Value
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่า XML กลับเป็น Type XDocument
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToXDocument() As XDocument
        Return Doc
    End Function

    ''' <summary>
    ''' สร้าง DataTable ที่อยู่ใน XML ตามชื่อ TableName ที่ส่งมา
    ''' </summary>
    ''' <param name="TableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateDataTable(ByVal TableName As String) As DataTable
        Dim Table = (From d In Doc.Root.Elements("Tables") _
                     Where d.Attribute("Name").Value = TableName).Elements("NewDataSet")
        Dim Ds As New DataSet
        Dim Dt As New DataTable
        If Table.Count > 0 Then
            Dim StrXml As String = String.Concat(Table)
            Dim StrReader As New StringReader(StrXml)
            Dim XmlReader As New XmlTextReader(StrReader)
            Ds = New DataSet
            Ds.ReadXml(XmlReader)
            Dt = Ds.Tables(0)
        End If
        Return Dt
    End Function

    ''' <summary>
    ''' คืนค่า XML ในรูปแบบ String
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateXmlString() As String
        Try
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Dim XmlWriter As New XmlTextWriter(Sw)
            Doc.Save(XmlWriter)
            Return Sb.ToString
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' คืนค่า Dataset
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet() As DataSet
        Try
            Return MainDs
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

End Class

Public Enum EnumLoad
    XmlPath
    XmlZipPath
    XmlString
End Enum

Public Enum EnVersionType
    Database
    Program
End Enum