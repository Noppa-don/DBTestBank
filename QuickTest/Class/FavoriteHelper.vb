Public NotInheritable Class FavoriteHelper

    ''' <summary>
    ''' function return รูป icon favorite
    ''' </summary>
    ''' <returns>path icon favorite</returns>
    Public Shared Function getIconFavorite(favoriteCode As String, favoriteScore As String) As String
        Dim imgPath As String
        Select Case favoriteCode
            Case EnumfavoriteCode.favorite
                imgPath = "../images/dashboard/student/f" & favoriteScore & ".png"
            Case EnumfavoriteCode.IQ
                imgPath = "../images/dashboard/student/favorite/iq/iq" & favoriteScore & ".png"
            Case EnumfavoriteCode.EQ
                imgPath = "../images/dashboard/student/favorite/eq/eq" & favoriteScore & ".png"
            Case EnumfavoriteCode.AQ
                imgPath = "../images/dashboard/student/favorite/aq/aq" & favoriteScore & ".png"
            Case EnumfavoriteCode.MQ
                imgPath = "../images/dashboard/student/favorite/mq/mq" & favoriteScore & ".png"
            Case EnumfavoriteCode.HQ
                imgPath = "../images/dashboard/student/favorite/hq/hq" & favoriteScore & ".png"
            Case EnumfavoriteCode.PQ
                imgPath = "../images/dashboard/student/favorite/pq/pq" & favoriteScore & ".png"
            Case EnumfavoriteCode.SQ
                imgPath = "../images/dashboard/student/favorite/sq/sq" & favoriteScore & ".png"
            Case EnumfavoriteCode.OQ
                imgPath = "../images/dashboard/student/favorite/oq/oq" & favoriteScore & ".png"
            Case EnumfavoriteCode.UQ
                imgPath = "../images/dashboard/student/favorite/uq/uq" & favoriteScore & ".png"
            Case EnumfavoriteCode.mostFavorite
                imgPath = "../images/dashboard/student/medal.png"
            Case Else
                imgPath = ""
        End Select
        Return imgPath
    End Function

    Public Shared Function getImgStudentFavorite(dt As DataTable) As String
        Dim sb As New StringBuilder()
        If dt.Rows.Count = 0 Then
            sb.Append("<img src='" & FavoriteHelper.getIconFavorite(0, 3) & "' class='notfavorite' />")
        Else
            Dim i As Integer = If((dt.Rows.Count > 5), 5, dt.Rows.Count)
            For j As Integer = 0 To i - 1
                sb.Append("<img src='" & FavoriteHelper.getIconFavorite(dt.Rows(j)("FavoriteCode"), dt.Rows(j)("FavoriteScore")) & "'  favoriteCode='" & dt.Rows(j)("FavoriteCode") & "' />")
            Next
            If dt.Rows.Count > 5 Then
                sb.Append("<img src='" & FavoriteHelper.getIconFavorite(99, 0) & "' width='15' />")
            End If
        End If
        Return sb.ToString()
    End Function

    Private Enum EnumfavoriteCode
        favorite = 0
        IQ = 1
        EQ = 2
        AQ = 3
        MQ = 4
        HQ = 5
        PQ = 6
        SQ = 7
        OQ = 8
        UQ = 9

        mostFavorite = 99
    End Enum
End Class
