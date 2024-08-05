Imports System.Data.SqlClient
Imports System.Web
Imports KnowledgeUtils


Namespace Service
    Public Class ClsNews
        Dim _DB As New ClassConnectSql
        Public Sub New(ByVal DB As ClassConnectSql)
            _DB = DB
        End Sub

        Sub New()
            ' TODO: Complete member initialization 
        End Sub

        Public Sub InsertNewsDetailCompletion(SchoolCode As String, NewsId As String)
            Dim sql As String = ""
            sql = "select News_ToStudent,News_ToTeacher,News_ToTeacherNoRoom from t360_tblNews where News_Id = '" & NewsId & "'"
            Dim dtStatusNews As DataTable = _DB.getdata(sql)

            If dtStatusNews.Rows(0)("News_ToStudent") = "1" Then
                sql = "insert into t360_tblNewsDetailCompletion(NR_Id,SchoolCode,User_Id)"
                sql &= " select NR_Id,'" & SchoolCode & "', Student_Id from t360_tblStudent "
                sql &= " inner join t360_tblNewsRoom on t360_tblStudent.Student_CurrentClass = t360_tblNewsRoom.Class_Name "
                sql &= " and t360_tblStudent.Student_CurrentRoom = t360_tblNewsRoom.Room_Name"
                sql &= " where t360_tblStudent.Student_IsActive = '1' and t360_tblStudent.Student_Status = '1' "
                sql &= " and t360_tblStudent.School_Code = '" & SchoolCode & "' and t360_tblNewsRoom.IsActive = '1' and t360_tblNewsroom.News_Id = '" & NewsId & "' "
                _DB.Execute(sql)
            End If
            If dtStatusNews.Rows(0)("News_ToTeacher") = "1" Then
                sql = "insert into t360_tblNewsDetailCompletion(NR_Id,SchoolCode,User_Id)"
                sql &= " select NR_Id,'" & SchoolCode & "', Teacher_Id from t360_tblTeacherRoom "
                sql &= " inner join t360_tblNewsRoom on t360_tblTeacherRoom.Class_Name = t360_tblNewsRoom.Class_Name "
                sql &= " and t360_tblTeacherRoom.Room_Name = t360_tblNewsRoom.Room_Name"
                sql &= " where t360_tblTeacherRoom.TR_IsActive = '1' and t360_tblTeacherRoom.School_Code = '" & SchoolCode & "'  and t360_tblNewsRoom.IsActive = '1'  and t360_tblNewsroom.News_Id = '" & NewsId & "' "
                _DB.Execute(sql)
            End If

            If dtStatusNews.Rows(0)("News_ToTeacherNoRoom") = "1" Then
                Dim NR_id = Guid.NewGuid
                sql = "insert into t360_tblNewsRoom select '" & SchoolCode & "','" & NewsId & "',null,null,dbo.GetThaiDate(),'1','" & NR_id.ToString & "',null"
                _DB.Execute(sql)

                sql = "insert into t360_tblNewsDetailCompletion(NR_Id,SchoolCode,User_Id) "
                sql &= " select '" & NR_id.ToString & "','" & SchoolCode & "', Teacher_Id from t360_tblTeacher where Teacher_id not in "
                sql &= " (select Teacher_id from t360_tblTeacherRoom where TR_IsActive = 1 and School_Code = '" & SchoolCode & "')"
                _DB.Execute(sql)
            End If

        End Sub

        Public Sub UpdateNewsDetailcompletion(SchoolCode As String, NewsId As String)
            Dim sql As String = ""
            sql = "update t360_tblNewsDetailCompletion set isactive = 0 where NR_Id in (select NR_Id from t360_tblNewsRoom where News_Id = '" & NewsId & "')"
            _DB.Execute(sql)

            InsertNewsDetailCompletion(SchoolCode, NewsId)
        End Sub
    End Class
End Namespace

