Imports System.Web

Namespace Service


    Public Class ClsSchool

        Dim _DB As ClsConnect
        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB
        End Sub

        Public Function GetTotalHour(ByVal SchoolId As String)

            Dim sql As String = " SELECT  SUM(  DATEDIFF( SECOND,tblQuizScore.FirstResponse, tblQuizScore.LastUpdate) ) / 3600 as TotalHour " &
                                " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id WHERE (tblQuiz.t360_SchoolCode = '" & SchoolId & "') " &
                                " AND (tblQuizScore.IsActive = 1) AND (tblQuiz.IsActive = 1) and (DATEDIFF(MINUTE,tblQuizScore.FirstResponse,tblQuizScore.LastUpdate) between 0 and 10 ) "
            Dim TotalHour As Integer = CType(_DB.ExecuteScalar(sql), Integer)

            Return TotalHour.ToString("#,##0")

        End Function

        Public Function GetLevelShortname(LevelId As String) As String
            Dim sql As String = "select Level_ShortName from tblLevel where Level_Id = '" & LevelId & "'"
            Dim LevelShortname As String = _DB.ExecuteScalar(sql)
            Return LevelShortname

        End Function
    End Class

End Namespace



