Imports KnowledgeUtils.Serialization
Imports System.Xml.Serialization
Imports System.Xml
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Web.Script.Serialization
Imports BusinessTablet360

Namespace Xml.Serialization

    ''' <summary>
    ''' คลาสจัดการเกี่ยวกับการแปลง Object ระหว่าง Xml
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageXmlSerialize

        ''' <summary>
        ''' ทำการแปลง Object เป็น Xml อยู่ในรูป XmlDocument
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns>XmlDocument</returns>
        ''' <remarks></remarks>
        Public Shared Function SerializeToXmlDocument(ByVal Obj As Object) As XmlDocument
            Dim Ser As XmlSerializer = New XmlSerializer(Obj.GetType())
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Ser.Serialize(Sw, Obj)
            Dim Doc As New XmlDocument
            Doc.LoadXml(Sb.ToString)
            Return Doc
        End Function

        ''' <summary>
        ''' ทำการแปลง Object เป็น Xml อยู่ในรูป XDocument
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns>XDocument</returns>
        ''' <remarks></remarks>
        Public Shared Function SerializeToXDocument(ByVal Obj As Object) As XDocument
            Dim Ser As XmlSerializer = New XmlSerializer(Obj.GetType())
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Ser.Serialize(Sw, Obj)
            Dim StrReader As New StringReader(Sb.ToString)
            Dim TxtReader As TextReader = StrReader
            Dim Doc As XDocument = XDocument.Load(TxtReader)
            Return Doc
        End Function

        ''' <summary>
        ''' ทำการแปลง Xml เป็น Object ของคลาสที่ส่งมา
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="Doc">XmlDocument</param>
        ''' <returns>คลาสที่ต้องการจะแปลง</returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Deserialize(Of T)(ByVal Doc As XmlDocument) As T
            Dim Reader As New XmlNodeReader(Doc.DocumentElement)
            Dim Ser As XmlSerializer = New XmlSerializer(GetType(T))
            Dim Obj As Object = Ser.Deserialize(Reader)
            Return CType(Obj, T)
        End Function

        ''' <summary>
        ''' ทำการแปลง Xml เป็น Object ของคลาสที่ส่งมา
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="Doc">XDocument</param>
        ''' <returns>คลาสที่ต้องการจะแปลง</returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Deserialize(Of T)(ByVal Doc As XDocument) As T
            Dim TxtReader As TextReader = New StringReader(Doc.ToString)
            Dim Ser As XmlSerializer = New XmlSerializer(GetType(T))
            Dim Obj As Object = Ser.Deserialize(TxtReader)
            Return CType(Obj, T)
        End Function

    End Class

    Public Module ModuleManageXmlSerialize

        ''' <summary>
        ''' ทำการแปลง Object เป็น Xml อยู่ในรูป XmlDocument
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns>XmlDocument</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function SerializeToXmlDocument(ByVal Obj As Object) As XmlDocument
            Dim Ser As XmlSerializer = New XmlSerializer(Obj.GetType())
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Ser.Serialize(Sw, Obj)
            Dim Doc As New XmlDocument
            Doc.LoadXml(Sb.ToString)
            Return Doc
        End Function

        ''' <summary>
        ''' ทำการแปลง Object เป็น Xml อยู่ในรูป XDocument
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns>XDocument</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function SerializeToXDocument(ByVal Obj As Object) As XDocument
            Dim Ser As XmlSerializer = New XmlSerializer(Obj.GetType())
            Dim Sb As New StringBuilder
            Dim Sw As New StringWriter(Sb)
            Ser.Serialize(Sw, Obj)
            Dim StrReader As New StringReader(Sb.ToString)
            Dim TxtReader As TextReader = StrReader
            Dim Doc As XDocument = XDocument.Load(TxtReader)
            Return Doc
        End Function

        ''' <summary>
        ''' ทำการแปลง Xml เป็น Object ของคลาสที่ส่งมา
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="Doc">XmlDocument</param>
        ''' <returns>คลาสที่ต้องการจะแปลง</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function Deserialize(Of T)(ByVal Doc As XmlDocument) As T
            Dim Reader As New XmlNodeReader(Doc.DocumentElement)
            Dim Ser As XmlSerializer = New XmlSerializer(GetType(T))
            Dim Obj As Object = Ser.Deserialize(Reader)
            Return CType(Obj, T)
        End Function

        ''' <summary>
        ''' ทำการแปลง Xml เป็น Object ของคลาสที่ส่งมา
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="Doc">XDocument</param>
        ''' <returns>คลาสที่ต้องการจะแปลง</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function Deserialize(Of T)(ByVal Doc As XDocument) As T
            Dim TxtReader As TextReader = New StringReader(Doc.ToString)
            Dim Ser As XmlSerializer = New XmlSerializer(GetType(T))
            Dim Obj As Object = Ser.Deserialize(TxtReader)
            Return CType(Obj, T)
        End Function

        ''' <summary>
        ''' ทำการแปลงเป็น Xml ในรูปของ String
        ''' </summary>
        ''' <param name="Doc">XmlDocument</param>
        ''' <returns>Xml ในรูป String</returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function ToXML(ByVal Doc As XmlDocument) As String
            Dim Sw As New StringWriter
            Dim Xw As New XmlTextWriter(Sw)
            Doc.WriteTo(Xw)
            Return Sw.ToString
        End Function

    End Module

End Namespace

Namespace Binary.Serialization

    ''' <summary>
    ''' คลาสจัดการเกี่ยวกับการแปลง Object ระหว่าง Binary
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageBinarySerialize

        ''' <summary>
        ''' จัดเก็บ Object ในรูปแบบไฟล์
        ''' </summary>
        ''' <param name="FileName">ชื่อและแฟ้มที่ต้องการจะเก็บ</param>
        ''' <param name="Obj">Instance</param>
        ''' <param name="Mode">โหมดการสร้างไฟล์ Default ที่สร้างใหม่ ถ้ามีอยู่แล้วจะทับไฟล์เก่า</param>
        ''' <remarks></remarks>
        Public Shared Sub SerializeToFile(ByVal FileName As String, ByVal Obj As Object, Optional ByVal Mode As FileMode = FileMode.Create)
            Dim FileStreamObject As FileStream = New FileStream(FileName, Mode)
            Dim BinaryFormatter As New BinaryFormatter
            Try
                BinaryFormatter.Serialize(FileStreamObject, Obj)
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            Finally
                FileStreamObject.Close()
            End Try
        End Sub

        ''' <summary>
        ''' แปลง Object ให้อยู่ในรูปแบบ Binary
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SerializeToBinary(ByVal Obj As Object) As Byte()
            Try
                Dim BinaryFormatter As New BinaryFormatter
                Dim Ms As New MemoryStream
                BinaryFormatter.Serialize(Ms, Obj)
                Return Ms.ToArray
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

        ''' <summary>
        ''' แปลงไฟล์ ให้เป็น Object
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="FileName">ชื่อและแฟ้มที่ต้องการจะแปลง</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Deserialize(Of T)(ByVal FileName As String) As T
            Dim FileStreamObject As FileStream = New FileStream(FileName, FileMode.Open)
            Dim BinaryFormatter As New BinaryFormatter
            Try
                Dim Obj = BinaryFormatter.Deserialize(FileStreamObject)
                Return CType(Obj, T)
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            Finally
                FileStreamObject.Close()
            End Try
        End Function

        ''' <summary>
        ''' แปลง Binary ให้อยู่ในรูปแบบ Object
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="ByteArray">Binary</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Deserialize(Of T)(ByVal ByteArray As Byte()) As T
            Try
                Dim BinaryFormatter As New BinaryFormatter
                Dim Ms As New MemoryStream(ByteArray)
                Dim Obj = BinaryFormatter.Deserialize(Ms)
                Return CType(Obj, T)
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

    End Class

    Public Module ModuleManageBinarySerialize

        ''' <summary>
        ''' แปลง Object ให้อยู่ในรูปแบบ Binary
        ''' </summary>
        ''' <param name="Obj">Instance</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function SerializeToBinary(ByVal Obj As Object) As Byte()
            Try
                Dim BinaryFormatter As New BinaryFormatter
                Dim Ms As New MemoryStream
                BinaryFormatter.Serialize(Ms, Obj)
                Return Ms.ToArray
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

        ''' <summary>
        ''' แปลง Binary ให้อยู่ในรูปแบบ Object
        ''' </summary>
        ''' <typeparam name="T">Type</typeparam>
        ''' <param name="ByteArray">Binary</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Deserialize(Of T)(ByVal ByteArray As Byte()) As T
            Try
                Dim BinaryFormatter As New BinaryFormatter
                Dim Ms As New MemoryStream(ByteArray)
                Dim Obj = BinaryFormatter.Deserialize(Ms)
                Return CType(Obj, T)
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

    End Module

End Namespace

Namespace Json.Serialization

    Public Class ManageJsonSerialize

        ''' <summary>
        ''' แปลง Object ให้อยู่ในรูปแบบ Json
        ''' </summary>
        ''' <param name="Obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SerializeToJson(ByVal Obj As Object) As String
            Try
                Dim Serializer As New JavaScriptSerializer
                Return Serializer.Serialize(Obj)
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

    End Class

    Public Module ModuleManageJsonSerialize

        ''' <summary>
        ''' แปลง Object ให้อยู่ในรูปแบบ Json
        ''' </summary>
        ''' <param name="Obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function SerializeToJson(ByVal Obj As Object) As String
            Try
                Dim Serializer As New JavaScriptSerializer
                Return (Serializer.Serialize(Obj))
            Catch e As SerializationException
                ElmahExtension.LogToElmah(e)
                Throw New Exception(e.Message)
            End Try
        End Function

        ''' <summary>
        ''' แปลง JsonString ให้อยู่ในรูป Object ถ้า String ไม่อยู่ใน Format Json จะทำการคืนค่า nothing
        ''' </summary>
        ''' <param name="JsonString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function DeserializeObject(ByVal JsonString As String) As Object
            Try
                Dim Serializer As New JavaScriptSerializer
                Return Serializer.DeserializeObject(JsonString)
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End Function

        ''' <summary> 
        ''' แปลง JsonString ให้อยู่ในรูป Object ตาม Type ที่ส่งเข้ามาถ้า String ไม่อยู่ใน Format Json จะทำการคืนค่า nothing หรือ ถ้า JsonString ที่ส่งเข้ามาเป็น Array ก็จะคืนค่า nothing เช่นกัน
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="JsonString"></param>
        ''' <returns></returns>
        ''' <remarks>ไม่สามารถแปลงข้อมูลที่เป็น array ได้</remarks>
        <Extension()>
        Public Function Deserialize(Of T)(ByVal JsonString As String) As T
            Try
                Dim Serializer As New JavaScriptSerializer
                Return Serializer.Deserialize(Of T)(JsonString)
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End Function

    End Module

End Namespace


