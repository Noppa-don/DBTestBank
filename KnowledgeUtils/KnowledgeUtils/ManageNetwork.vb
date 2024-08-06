Imports BusinessTablet360

Namespace Network

    Public Class ManageNetwork

        ''' <summary>
        ''' Ping IP
        ''' </summary>
        ''' <param name="IP">IP หรือ URL</param>
        ''' <param name="TimeOut">ตั้งเวลา</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Ping(ByVal IP As String, Optional ByVal TimeOut As Integer = 500) As Boolean
            Try
                If My.Computer.Network.Ping(IP, TimeOut) Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

    End Class

End Namespace

