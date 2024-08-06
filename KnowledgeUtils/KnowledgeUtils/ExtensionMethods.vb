Imports System.Runtime.CompilerServices

Public Module ExtensionMethods

    Public Sub PrintMe(value As String)
        Return
    End Sub

    ''' <summary>
    ''' ทำการต่อสตริงประกอบ Path รูปขึ้นมาตาม QsetId
    ''' </summary>
    ''' <param name="QSetId">QsetId ของคำถาม/คำตอบ ข้อนั้น</param>
    ''' <returns>สตริง Path รูปที่ถูกต้องตาม QsetId</returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToQsetIdPath(value As String, QsetId As String) As String
        Dim newGuid As Guid
        If Guid.TryParse(QsetId, newGuid) Then
            Dim path As String = String.Format("../file/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{{8}}/",
                                                QsetId.Substring(0, 1), QsetId.Substring(1, 1), QsetId.Substring(2, 1),
                                                 QsetId.Substring(3, 1), QsetId.Substring(4, 1), QsetId.Substring(5, 1),
                                                  QsetId.Substring(6, 1), QsetId.Substring(7, 1), QsetId)
            Return value.Replace("___MODULE_URL___", path)
        End If
        Return value
    End Function

    <Extension()>
    Public Function ToPointplusScore(value As String, Optional hasSpace As Boolean = False) As String
        Dim arr As String() = value.Split("/")
        If arr.Length > 1 Then
            If hasSpace Then Return String.Format("{0} / {1}", arr(0).Trim().ToScore(), arr(1).Trim().ToScore())
            Return String.Format("{0}/{1}", arr(0).Trim().ToScore(), arr(1).Trim().ToScore())
        End If
        Return value.ToScore()
    End Function

    <Extension()>
    Private Function ToScore(value As String) As String
        Dim arr As String() = value.Split(".")
        If arr.Length > 1 Then
            If arr(1).Replace("0", "") = "" Then Return arr(0)
            If arr(1).Substring(0, 1).Replace("0", "") = "" Then Return String.Format("{0}.{1}", arr(0), arr(1)) ' 5.01 -> 5.01
            If arr(1).Substring(1, 1).Replace("0", "") = "" Then Return String.Format("{0}.{1}", arr(0), arr(1).Replace("0", "")) ' 5.10 -> 5.1
            Return String.Format("{0}.{1}", arr(0), arr(1).Replace("0", "")) ' 5.11 -> 5.11
        End If
        Return value
    End Function

    <Extension()>
    Public Function ToPointPlusTime(value As DateTime, Optional now As DateTime = Nothing) As String
        'If now = Nothing Then now = DateTime.Now
        'Dim sec As Long = DateDiff(DateInterval.Second, value, now)
        'If sec > 0 And sec < 60 Then Return String.Format("เมื่อ {0} วินาทีที่แล้ว", sec)
        'If sec >= 60 And sec < 3600 Then Return String.Format("เมื่อ {0} นาทีที่แล้ว", Math.Floor(sec / 60))
        Return value.ToString("dd/MM/yy HH:mm")
    End Function

    <Extension()>
    Public Function ToNetworkDisconnectTime(value As DateTime) As String
        Dim min As Integer = CInt(DateDiff(DateInterval.Minute, value, DateTime.Now))
        If min < 60 Then
            Return String.Format("{0} นาที", min)
        ElseIf min < 1440 Then
            Return String.Format("{0} ชั่วโมง", DateDiff(DateInterval.Hour, value, DateTime.Now))
        End If
        Return String.Format("{0} วัน", DateDiff(DateInterval.Day, value, Date.Today))
    End Function

    <Extension()>
    Public Function ToIpAddress(value As String) As String
        Dim nums As List(Of String) = value.Split(".").ToList()
        Return String.Format("{0}.{1}.{2}.{3}", CInt(nums(0)), CInt(nums(1)), CInt(nums(2)), CInt(nums(3)))
    End Function

    ''' <summary>
    ''' ฟังชั่น replace single quote ให้กลายเป็น 2xsingle quotes แทน, เพื่อที่จะได้ส่งไปเข้า SQL Server ไม่พัง 
    ''' เช่น "select * from tblA where name ='" & inputname.CleanDBString & "';"
    ''' </summary>
    ''' <param name="Source">string ใดๆ </param>
    ''' <returns></returns>
    <Extension()>
    Public Function CleanSQL(Source As String) As String
        If Source Is Nothing Then
            Return ""
        Else
            Return Source.Replace("'", "''")
        End If
    End Function
End Module
