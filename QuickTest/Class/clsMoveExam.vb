Imports System.Data.SqlClient

Public Class clsMoveExam
    Dim Db As New ClassConnectSql()

    Public Function GetNewQcatByLevel(LevelId As String, QuestionId As String) As DataTable
        Dim sql As String

        If QuestionId = "" Then
            sql = "select distinct QcatId,QCatName,null as MId from tblNewSetEng where LevelId = '" & LevelId & "' order by QCatName;"
        Else
            sql = "select distinct nse.QcatId,QCatName,MId
                from tblNewSetEng nse left join (select mid,ss.QCatId from tblMoveEngExam dd inner join tblNewSetEng ss on ss.QsetId = dd.QSetId 
                where IsActive = 1 and QuestionId = '" & QuestionId & "')m 
                on nse.QcatId = m.QcatId where nse.LevelId = '" & LevelId & "' order by QCatName;"
        End If


        Dim dt As DataTable = Db.getdata(sql)
        Return dt
    End Function
    Public Function GetNewQSetByQCat(QCatId As String, QuestionId As String) As DataTable
        Dim sql As String
        If QuestionId = "" Then
            sql = "select distinct QSetId,QSetName,null as MId from tblNewSetEng where QCatId = '" & QCatId & "' order by QSetName;"
        Else
            sql = "select distinct nse.QsetId,QSetName,MId
                    from tblNewSetEng nse left join (select mid,ss.QCatId,ss.QsetId from tblMoveEngExam dd inner join tblNewSetEng ss on ss.QsetId = dd.QSetId 
                    where IsActive = 1 and QuestionId = '" & QuestionId & "')m 
                    on nse.QsetId = m.QsetId 
                    where nse.QcatId = '" & QCatId & "' order by QSetName;"
        End If

        Dim dt As DataTable = Db.getdata(sql)
        Return dt
    End Function

    ''' <summary>
    ''' ทำการ หาข้อมูลของชุดข้อสอบ โดยแบ่งตามประเภท ควิซ,ใบงาน และทำการ Bind Repeater ที่ส่งเข้ามา
    ''' </summary>
    ''' <param name="qSetid">Id ของชุดข้อสอบ</param>
    ''' <param name="WhereField">ชื่อฟิลด์ที่จะนำไปทำการ Where ใน Query</param>
    ''' <param name="InputRepeater">Repeater Control ที่จะนำมา Bind ข้อมูล</param>
    ''' <param name="IsQuizMode">เป็นโหมดควิซ</param>
    ''' <remarks></remarks>
    Public Sub BindRepeaterMoveExam(ByVal qSetid As String, ByRef InputRepeater As Repeater)
        Dim conn As New SqlConnection
        Db.OpenExclusiveConnect(conn)
        Try
            Dim sql As String = ""
            sql = "SELECT q.Question_Id,ROW_NUMBER()OVER(ORDER BY q.Question_No) AS Question_No, 
                  case when q.Question_Name_Quiz is null then '' else q.Question_Name_Quiz end as Question_Name,mex.QSetId
                  FROM tblQuestion q left JOIN (select * from tblMoveEngExam where IsActive = 1)mex ON  mex.QuestionId = q.Question_Id 
                  where q.Qset_Id = '" & qSetid & "' and q.IsActive = 1 ORDER BY q.Question_No;"
            Dim dt As New DataTable
            dt = Db.getdata(sql, , conn)

            InputRepeater.DataSource = dt
            InputRepeater.DataBind()
            Db.CloseExclusiveConnect(conn)
        Catch ex As Exception
            Db.CloseExclusiveConnect(conn)
        End Try
    End Sub

    Public Function UpdateQuestionSet(QSetId As String, QuestionId As String) As String

        Try
            Dim sql As String
            sql = "update tblMoveEngExam set IsActive = 0 where QuestionId = '" & QuestionId & "';"
            Db.Execute(sql)

            sql = "Insert into tblMoveEngExam(QsetId,QuestionId) values ('" & QSetId & "','" & QuestionId & "');"
            Db.Execute(sql)

            Return "1"

        Catch ex As Exception
            Return "0"
        End Try
    End Function


    Public Function GetSelectedLevelbyQuestion(QuestionId As String) As String
        Try
            Dim sql As String
            sql = "select n.LevelId from tblMoveEngExam m inner join tblNewSetEng n on m.QSetId = n.QsetId where QuestionId = '" & QuestionId & "' and IsActive = 1"
            Dim levelId As String = (Db.ExecuteScalar(sql)).ToString

            If levelId = "" Then
                Return "0"
            Else
                Return levelId
            End If

        Catch ex As Exception
            Return "0"
        End Try
    End Function

End Class
