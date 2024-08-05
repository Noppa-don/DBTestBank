Imports System.Runtime.CompilerServices

Namespace System.Numeric

    Public Module ModuleManageNumeric

        ''' <summary>
        ''' ฟังชั่น + ค่าตัวแปรตามจำนวนเลขที่ส่งเข้ามา
        ''' </summary>
        ''' <param name="Source">เลข integer ที่ต้องการจะบวก</param>
        ''' <param name="Plus">จำนวนค่าที่จะบวก</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Extension()>
        Public Function IntegerPlus(ByRef Source As Integer, ByVal Plus As Integer) As Integer
            Source = Source + Plus
            Return Source
        End Function

    End Module

End Namespace

