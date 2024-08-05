Imports System.Runtime.CompilerServices
Imports System.IO

Namespace System.String

    ''' <summary>
    ''' คลาสจัดการ String
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManageString

        ''' <summary>
        ''' ฟังชั่นใช้ตัดคำที่อยู่ระหว่างคำสองคำ ถ้าไม่เจอหรือมีปัญหาส่งค่าช่องว่างออกไป
        ''' </summary>
        ''' <param name="Source">ประโยคที่จะถูกตัด</param>
        ''' <param name="FirstChar">ตัวอักษรตัวแรกที่จะตัด</param>
        ''' <param name="LastChar">ตัวอักษรตัวสุดท้ายที่จะตัด</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CutWord(ByVal Source As String, ByVal FirstChar As String, ByVal LastChar As String) As String
            Dim Result As String = ""
            'IndexOf ตำแหน่งแรกที่เจอคือ 0
            Dim F = Source.IndexOf(FirstChar)
            Dim L = Source.IndexOf(LastChar)
            If F > -1 AndAlso L > -1 AndAlso L > F Then
                'Mid ตำแหน่งแรกคือ 1
                Result = Strings.Mid(Source, F + 1, L - F + 1)
            End If
            Return Result
        End Function

        ''' <summary>
        ''' ฟังชั่นต่อคำที่ต้องการไปไว้ตามตำแหน่งที่ระบุ
        ''' </summary>
        ''' <param name="Source">ประโยชคต้นฉบับ</param>
        ''' <param name="InsertText">คำที่ต้องการจะแทรกเข้าไป</param>
        ''' <param name="Position">ตำแหน่งที่ต้องการจะแทกโดยถ้าไม่ระบุจะหมายถึงทั้งสองข้าง</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FusionText(ByVal Source As String, ByVal InsertText As String, Optional ByVal Position As EnumTextPosition = EnumTextPosition.Both) As String
            If Not Source Is Nothing AndAlso Source <> "" Then
                Select Case Position
                    Case EnumTextPosition.Both
                        Source = InsertText & Source & InsertText
                    Case EnumTextPosition.Left
                        Source = InsertText & Source
                    Case EnumTextPosition.Rigth
                        Source = Source & InsertText
                End Select
            End If
            Return Source
        End Function

        ''' <summary>
        ''' ฟังชั่นเช็ค String ถ้ามีค่าจะคืนค่าตัวเอง ถ้าเป็นช่องว่าง หรือ Nothing จะทำการคืนค่า Nothing
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TextOrNothing(ByVal Source As String) As String
            If Source Is Nothing OrElse Source = "" Then
                Source = Nothing
            End If
            Return Source
        End Function

        ''' <summary>
        ''' ฟังชั่นรับสตริงที่เป็นรูปแบบ Tag จะทำการตัด Tag ทั้งหมดทิ้งทำให้เหลือแต่ข้อความ
        ''' </summary>
        ''' <param name="HTML"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function StripTags(ByVal HTML As String) As String
            ' Removes tags from passed HTML
            Dim Rx As New Text.RegularExpressions.Regex("<[^>]*>")
            Return Rx.Replace(HTML, "")
        End Function

        ''' <summary>
        ''' ฟั่งชั่นตัดเอกสาร ในรูปแบบ Html ทิ้งทั้งหมด เหมาะกับการนำค่าจาก Response.Write ไปใช้
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <param name="TagName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function StripSpecialTags(ByVal Source As String, ByVal TagName As String) As String
            Dim P  = Source.IndexOf("<!DOCTYPE")
            Return Trim(Source.Substring(0, P - 1))
        End Function

        Public Shared Function StripHtmlDocument(ByVal HTML As String, ByVal TagName As String) As String
            Dim Rx As New Text.RegularExpressions.Regex("<" & TagName & " [^>]*>")
            Return Rx.Replace(HTML, "")
        End Function

        ''' <summary>
        ''' ฟั่งชั่นแปลง String ให้อยู่ในรูปแบบ StringReader
        ''' </summary>
        ''' <param name="Souce"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetTextReader(Souce As String) As StringReader
            Dim Tr As StringReader
            If Souce IsNot Nothing Then
                Tr = New StringReader(Souce)
            End If
            Return Tr
        End Function

    End Class

    Public Module ModuleManageString

        ''' <summary>
        ''' ฟังชั่นใช้ตัดคำที่อยู่ระหว่างคำสองคำ ถ้าไม่เจอหรือมีปัญหาส่งค่าช่องว่างออกไป
        ''' </summary>
        ''' <param name="FirstChar">ตัวอักษรตัวแรกที่จะตัด</param>
        ''' <param name="LastChar">ตัวอักษรตัวสุดท้ายที่จะตัด</param>
        ''' <param name="Source">ประโยคที่จะถูกตัด</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function CutWord(ByVal Source As String, ByVal FirstChar As String, ByVal LastChar As String) As String
            Dim Result As String = ""
            'IndexOf ตำแหน่งแรกที่เจอคือ 0
            Dim F = Source.IndexOf(FirstChar)
            Dim L = Source.IndexOf(LastChar)
            If F > -1 AndAlso L > -1 AndAlso L > F Then
                'Mid ตำแหน่งแรกคือ 1
                Result = Strings.Mid(Source, F + 1, L - F + 1)
            End If
            Return Result
        End Function

        ''' <summary>
        ''' ฟังชั่นต่อคำที่ต้องการไปไว้ตามตำแหน่งที่ระบุ
        ''' </summary>
        ''' <param name="Source">ประโยชคต้นฉบับ</param>
        ''' <param name="InsertText">คำที่ต้องการจะแทรกเข้าไป</param>
        ''' <param name="Position">ตำแหน่งที่ต้องการจะแทกโดยถ้าไม่ระบุจะหมายถึงทั้งสองข้าง</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function FusionText(ByVal Source As String, ByVal InsertText As String, Optional ByVal Position As EnumTextPosition = EnumTextPosition.Both) As String
            If Not Source Is Nothing AndAlso Source <> "" Then
                Select Case Position
                    Case EnumTextPosition.Both
                        Source = InsertText & Source & InsertText
                    Case EnumTextPosition.Left
                        Source = InsertText & Source
                    Case EnumTextPosition.Rigth
                        Source = Source & InsertText
                End Select
            End If
            Return Source
        End Function

        ''' <summary>
        ''' ฟังชั่นเช็ค String ถ้ามีค่าจะคืนค่าตัวเอง ถ้าเป็นช่องว่าง หรือ Nothing จะทำการคืนค่า Nothing
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function TextOrNothing(ByVal Source As String) As String
            If Source Is Nothing OrElse Source = "" Then
                Source = Nothing
            End If
            Return Source
        End Function

        <Extension()>
        Public Function ToGuid(ByVal Source As String) As Guid?
            If Source Is Nothing OrElse Source = "" Then
                Return Nothing
            End If
            Return New Guid(Source)
        End Function

        ''' <summary>
        ''' ฟั่งชั่นตัดเอกสาร ในรูปแบบ Html ทิ้งทั้งหมด เหมาะกับการนำค่าจาก Response.Write ไปใช้
        ''' </summary>
        ''' <param name="Source"></param>
        ''' <param name="TagName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function StripSpecialTags(ByVal Source As String, ByVal TagName As String) As String
            Dim P = Source.IndexOf("<!DOCTYPE")
            Return Trim(Source.Substring(0, P - 1))
        End Function

        ''' <summary>
        ''' ฟั่งชั่นแปลง String ให้อยู่ในรูปแบบ StringReader
        ''' </summary>
        ''' <param name="Souce"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function GetTextReader(Souce As String) As StringReader
            Dim Tr As StringReader
            If Souce IsNot Nothing Then
                Tr = New StringReader(Souce)
            End If
            Return Tr
        End Function

        ''' <summary>
        ''' ฟังชั่นแปลง Integer เป็น String ถ้าค่าเป็น 0 จะคืนค่าช่องว่างแทน และสามารถกำหนดรูปแบบได้
        ''' </summary>
        ''' <param name="Souce"></param>
        ''' <param name="Format"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function NumberToString(Souce As Integer, Optional Format As String = "") As String
            If Souce = 0 Then
                Return ""
            Else
                Return Souce.ToString(Format)
            End If
        End Function

    End Module

    Public Enum EnumTextPosition
        Both
        Left
        Rigth
    End Enum

End Namespace


