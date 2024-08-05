Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class StudentService
    Inherits System.Web.Services.WebService

    '<WebMethod()> _
    'Public Function HelloWorld() As String
    '    Return "Hello World"
    'End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetFavoriteCodeStudent(studentId As String, favariteCode As Integer) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return False
        End If

        Dim userId As String = HttpContext.Current.Session("UserId").ToString()
        Dim db As New ClassConnectSql()
        Try
            Log.Record(Log.LogType.SetFavoriteStudent, db.CleanString("เพิ่ม Studentid = '" & studentId & "' ใน Favorite"), True)

            db.OpenWithTransection()
            Dim sql As String = "select * from tblteacherfavorite where teacher_id = '" & userId & "' and Student_id = '" & db.CleanString(studentId) & "';"

            Dim dtTeacherFavorite As DataTable = db.getdataWithTransaction(sql)

            If dtTeacherFavorite.Rows.Count = 0 Then
                sql = " INSERT INTO tblTeacherFavorite VALUES (NEWID(),'" & userId & "','" & studentId & "',GETDATE(),'" & favariteCode & "',1,NULL); "
            Else
                Dim tf_id As String = dtTeacherFavorite.Rows(0)("Tf_Id").ToString()
                sql = "SELECT * FROM tblTeacherFavoriteDetail WHERE Tf_Id = '" & tf_id & "' AND FavoriteScore = '" & 1 & "';"
                If db.getdataWithTransaction(sql).Rows.Count = 0 Then
                    sql = "INSERT INTO tblTeacherFavoriteDetail VALUES ("
                Else
                    sql = "UPDATE tblTeacherFavoriteDetail SET "
                    db.ExecuteScalarWithTransection("")
                End If

                sql = " UPDATE tblTeacherFavorite SET FavoriteCode = '" & favariteCode & "',IsActive = 1,LastUpdate = GETDATE() WHERE Teacher_id = '" & userId & "' AND Student_Id = '" & studentId & "'; "
            End If
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            db = Nothing
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
            Return False
        End Try
    End Function


    <WebMethod(EnableSession:=True)>
    Public Function setStudentFavorite(ByVal studentId As String, ByVal favoriteCode As Integer, ByVal favoriteScore As Integer)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return False
        End If

        Dim userId As String = HttpContext.Current.Session("UserId").ToString()
        Dim db As New ClassConnectSql()
        Try
            Log.Record(Log.LogType.SetFavoriteStudent, db.CleanString("เพิ่ม Studentid = '" & studentId & "' ใน Favorite"), True)
            Dim favoriteOrder As Integer = 0
            db.OpenWithTransection()
            Dim sql As String = "select * from tblteacherfavorite where teacher_id = '" & userId & "' and Student_id = '" & db.CleanString(studentId) & "';"

            Dim dtTeacherFavorite As DataTable = db.getdataWithTransaction(sql)

            If dtTeacherFavorite.Rows.Count = 0 Then
                Dim tf_Id As String = db.ExecuteScalarWithTransection("SELECT NEWID();").ToString()

                sql = " INSERT INTO tblTeacherFavorite VALUES ('" & tf_Id & "','" & userId & "','" & studentId & "',GETDATE(),1,NULL); "
                db.ExecuteWithTransection(sql)

                favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)

                sql = " INSERT INTO tblTeacherFavoriteDetail VALUES (NEWID(),'" & tf_Id & "'," & favoriteCode & "," & favoriteScore & "," & favoriteOrder & ",GETDATE(),1);"
                db.ExecuteWithTransection(sql)
            Else
                Dim tf_id As String = dtTeacherFavorite.Rows(0)("Tf_Id").ToString()
                sql = "Select * FROM tblTeacherFavoriteDetail WHERE Tf_Id = '" & tf_id & "' AND FavoriteCode = " & favoriteCode & ";"
                If db.getdataWithTransaction(sql).Rows.Count = 0 Then

                    sql = " SELECT * FROM tblTeacherFavoriteDetail WHERE Tf_Id = '" & tf_id & "' AND FavoriteScore <> 3 ORDER BY FavoriteOrder;"
                    Dim tmpDt As DataTable = db.getdataWithTransaction(sql)
                    If tmpDt.Rows.Count = 0 Then
                        favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)
                    Else
                        Dim q = (From t In tmpDt.AsEnumerable() Where t.Field(Of Byte)("FavoriteCode") <> 0 Select t).LastOrDefault()
                        If q Is Nothing Then
                            favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)
                        Else
                            favoriteOrder = q.Field(Of Byte)("FavoriteOrder") + 1
                        End If
                    End If

                    sql = " INSERT INTO tblTeacherFavoriteDetail VALUES (NEWID(),'" & tf_id & "'," & favoriteCode & "," & favoriteScore & "," & favoriteOrder & ",GETDATE(),1);"
                    db.ExecuteWithTransection(sql)
                Else
                    If favoriteCode = 0 Then
                        favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)
                    Else
                        sql = " SELECT * FROM tblTeacherFavoriteDetail WHERE Tf_Id = '" & tf_id & "' AND FavoriteScore <> 3 ORDER BY FavoriteOrder;"
                        Dim tmpDt As DataTable = db.getdataWithTransaction(sql)
                        If tmpDt.Rows.Count = 0 Then
                            favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)
                        Else
                            Dim q = (From t In tmpDt.AsEnumerable() Where t.Field(Of Byte)("FavoriteCode") = favoriteCode Select t).SingleOrDefault()
                            If q Is Nothing Then
                                q = (From t In tmpDt.AsEnumerable() Where t.Field(Of Byte)("FavoriteCode") <> 0 Select t).LastOrDefault()
                                If q Is Nothing Then
                                    favoriteOrder = getFavoriteOrder(favoriteCode, favoriteScore)
                                Else
                                    favoriteOrder = If((favoriteScore = 3), 0, q.Field(Of Byte)("FavoriteOrder") + 1)
                                End If
                            Else
                                If favoriteScore = 3 Then
                                    favoriteOrder = 0
                                    For Each r In tmpDt.Rows
                                        If r("FavoriteOrder") > q.Field(Of Byte)("FavoriteOrder") Then
                                            sql = "UPDATE tblTeacherFavoriteDetail SET FavoriteOrder = " & (r("FavoriteOrder") - 1) & ",LastUpdate = GETDATE() WHERE FavoriteCode = " & r("FavoriteCode") & " AND Tf_Id = '" & tf_id & "'; "
                                            db.ExecuteWithTransection(sql)
                                        End If
                                    Next
                                Else
                                    favoriteOrder = q.Field(Of Byte)("FavoriteOrder")
                                End If
                            End If
                        End If
                    End If

                    sql = "UPDATE tblTeacherFavoriteDetail SET FavoriteScore = " & favoriteScore & ", FavoriteOrder = " & favoriteOrder & ",LastUpdate = GETDATE() WHERE FavoriteCode = " & favoriteCode & " AND Tf_Id = '" & tf_id & "'; "
                    db.ExecuteWithTransection(sql)
                End If

                'sql = " UPDATE tblTeacherFavorite SET FavoriteCode = '" & favoriteCode & "',IsActive = 1,LastUpdate = GETDATE() WHERE Teacher_id = '" & userId & "' AND Student_Id = '" & studentId & "'; "
            End If
            'db.ExecuteWithTransection(sql)
            db.CommitTransection()
            db = Nothing

            Dim ClsStudent As New Service.ClsStudent(New ClassConnectSql())
            Dim dtStudentFavorite As DataTable = ClsStudent.getStudentFavoriteCode(HttpContext.Current.Session("UserId").ToString(), studentId, Nothing)
            Return FavoriteHelper.getImgStudentFavorite(dtStudentFavorite)

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Function getFavoriteOrder(favoriteCode As Integer, favoriteScore As Integer) As Integer
        If favoriteCode = 0 AndAlso favoriteScore = 3 Then
            Return 0
        ElseIf favoriteCode = 0 AndAlso favoriteScore <> 3 Then
            Return 1
        ElseIf favoriteCode <> 0 AndAlso favoriteScore = 3 Then
            Return 0
        ElseIf favoriteCode <> 0 AndAlso favoriteScore <> 3 Then
            Return 2
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function getStudentFavorite(ByVal studentId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return False
        End If

        Dim db As New ClassConnectSql()
        Try
            Dim teacherId As String = HttpContext.Current.Session("UserId").ToString()
            db.OpenWithTransection()
            Dim sql As String = "select * from tblteacherfavorite where teacher_id = '" & teacherId & "' and Student_id = '" & db.CleanString(studentId) & "';"
            Dim dt As DataTable = db.getdataWithTransaction(sql)

            If dt.Rows.Count = 0 OrElse dt.Rows(0)("IsActive") = False Then
                db.CommitTransection()
                db = Nothing
                Return ""
            End If

            Dim tf_Id As String = dt.Rows(0)("Tf_Id").ToString()
            sql = " SELECT * FROM tblTeacherFavoriteDetail WHERE Tf_Id = '" & tf_Id & "';"
            dt = db.getdataWithTransaction(sql)

            Dim result As New StringBuilder()

            For i As Integer = 0 To 9
                Dim q = (From t In dt.AsEnumerable() Where t.Field(Of Byte)("FavoriteCode") = i Select t.Field(Of Byte)("FavoriteScore")).SingleOrDefault()
                Dim favoriteScore As Integer = If((q = 0), q + 3, q)
                Dim div As String = String.Format("<div id='favorite{0}' style='background-image:url(""{1}"");' favoriteScore='{2}'></div>", i, FavoriteHelper.getIconFavorite(i, favoriteScore), favoriteScore)
                result.Append(div)
            Next

            db.CommitTransection()
            db = Nothing

            Return result.ToString()
        Catch ex As Exception
            db.RollbackTransection()
            Return "-1"
        End Try
    End Function


    'Private Function getIconFavorite(favoriteCode As Integer, favoriteScore As Integer)
    '    Dim imgPath As String
    '    Dim img As String = "../images/dashboard/student/b" & favoriteScore & ".png"
    '    Return img
    '    Select Case favoriteCode
    '        Case 0
    '            imgPath = ""
    '        Case 1
    '            imgPath = ""
    '        Case 2
    '            imgPath = ""
    '        Case 3
    '            imgPath = ""
    '        Case 4
    '            imgPath = ""
    '        Case 5
    '            imgPath = ""
    '        Case 6
    '            imgPath = ""
    '        Case 7
    '            imgPath = ""
    '        Case 8
    '            imgPath = ""
    '        Case 9
    '            imgPath = ""
    '        Case Else
    '            imgPath = ""
    '    End Select
    '    Return imgPath
    'End Function

    <WebMethod(EnableSession:=True)> _
    Public Function AddStudentFavorite(ByVal StudentId As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = "select count(*) as FavoriteAmount from tblteacherfavorite " & _
            "where teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' and Student_id = '" & _DB.CleanString(StudentId) & "' "

        Dim FavoriteAmount As String = _DB.ExecuteScalar(sql)

        If FavoriteAmount = "0" Then
            sql = " INSERT INTO dbo.tblTeacherFavorite " & _
                                        " ( Tf_id, Teacher_id, Student_Id ) " & _
                                        " VALUES  ( NEWID(),'" & HttpContext.Current.Session("UserId").ToString() & "','" & StudentId & "' ) "
        Else
            sql = " Update dbo.tblTeacherFavorite Set IsActive = '1' WHERE Student_Id = '" & _DB.CleanString(StudentId) & "' AND Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' "
        End If



        Try
            _DB.Execute(sql)
            'เก็บ Log เพิ่มนักเรียน Favorite
            Log.Record(Log.LogType.SetFavoriteStudent, _DB.CleanString("เพิ่ม Studentid = '" & StudentId & "' ใน Favorite"), True)
            _DB = Nothing
            'Return "Complete"

            Return "url(" & "MonsterID.axd?seed=" & StudentId & "&size=179" & ")"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return ""
        End Try

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function DeleteFavoriteStudent(ByVal StudentId As String) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return ""
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = " Update dbo.tblTeacherFavorite Set IsActive = '0', LastUpdate=dbo.GetThaiDate() WHERE Student_Id = '" & _DB.CleanString(StudentId) & "' AND Teacher_id = '" & HttpContext.Current.Session("UserId").ToString() & "' "
        Try
            _DB.Execute(sql)
            'เก็บ log ลบนักเรียนออกจาก Favorite
            Log.Record(Log.LogType.SetFavoriteStudent, _DB.CleanString("ลบ Studentid = " & StudentId & " ออกจาก Favorite"), True)
            _DB = Nothing
            Return "url(" & "MonsterID.axd?seed=" & StudentId & "&size=179" & ")"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB = Nothing
            Return ""
        End Try

    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetSessionCalendarId(ByVal CalendarId As String, ByVal CalendarName As String)

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        If CalendarId <> "" Then
            'Dim UserID As String = HttpContext.Current.Session("UserId").ToString()
            'HttpContext.Current.Application("CalendarID_" & UserID) = CalendarId
            'HttpContext.Current.Application("CalendarName_" & UserID) = CalendarName
            Dim KnSession As New KNAppSession()
            KnSession.StoredValue("SelectedCalendarId") = CalendarId
            KnSession.StoredValue("SelectedCalendarName") = CalendarName
            Return "Complete"
        Else
            Return ""
        End If
    End Function


    <WebMethod(EnableSession:=True)>
    Public Sub ClearAppDeviceUniqueID(ByVal DeviceUniqueID As String)
        Dim KNSession As New ClsKNSession()
        KNSession(DeviceUniqueID & "|" & "QuizId") = Nothing
        KNSession(DeviceUniqueID & "|" & "_ExmanNum") = Nothing
        KNSession(DeviceUniqueID & "|" & "CurrentAnsState") = Nothing
        KNSession(DeviceUniqueID & "|" & "IsUpdateCheckTablet") = Nothing
        HttpContext.Current.Application("uvw_GetQuizIdAndPlayerIdBySerialNumber_" & DeviceUniqueID) = Nothing
        HttpContext.Current.Session("ChooseMode") = Nothing
    End Sub

    <WebMethod(EnableSession:=True)>
    Public Function SetStudentPoint(ByVal PlayerId As String, ByRef InputConn As SqlConnection)

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim sql As String = " SELECT TOP 1 * FROM tblStudentPoint WHERE Student_Id = '" & PlayerId & ";'"
        Dim dt As DataTable = New ClassConnectSql().getdata(sql)
        Dim dtCoin As DataTable = GetSilverAndGoldCoin(PlayerId, InputConn)
        Dim Silver = dtCoin.Rows(0)("Silver")
        Dim Gold
        If IsDBNull(dtCoin.Rows(0)("Gold")) Then
            Gold = 0
        Else
            Gold = dtCoin.Rows(0)("Gold")
        End If
        If Not dt.Rows.Count = 0 Then
            sql = " UPDATE tblStudentPoint SET Silver += " & Silver & " ,Gold += " & Gold & ",TotalSilver +=" & Silver & ",TotalGold +=" & Gold & ",LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE Student_Id = '" & PlayerId & "'; "
        Else
            sql = " INSERT INTO tblStudentPoint (Student_Id,Silver,Gold,TotalSilver,TotalGold) VALUES ('" & PlayerId & "', " & Silver & "," & Gold & ", " & Silver & "," & Gold & ");"
        End If

        If HttpContext.Current.Session("ChooseMode") = EnumDashBoardType.Practice Then
            sql &= " UPDATE tblQuiz SET EndTime = dbo.GetThaiDate(),LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE IsPracticeMode = 1 AND Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' AND User_Id = '" & PlayerId & "'; "
            HttpContext.Current.Session("ChooseMode") = Nothing
            HttpContext.Current.Session("Quiz_Id") = Nothing
        End If

        Dim db As New ClassConnectSql()
        db.Execute(sql)

        Return 1
    End Function

    ' GetSilverAndGoldCoin
    <WebMethod(EnableSession:=True)> _
    Public Function GetSilverAndGoldCoin(ByVal PlayerId As String, ByRef InputConn As SqlConnection) As DataTable

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Or PlayerId = "" Then
            Return New DataTable()
        End If

        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim sql As New StringBuilder()
        sql.Append(" SELECT COUNT(*) AS Silver,CAST(SUM(tblQuizSession.TotalScore) / COUNT(*) AS TINYINT) / 10 AS Gold ")
        sql.Append(" FROM tblQuizScore INNER JOIN tblQuizSession ON tblQuizScore.Quiz_Id = tblQuizSession.Quiz_Id ")
        sql.Append(" WHERE tblQuizScore.Answer_Id IS NOT NULL AND tblQuizScore.Quiz_Id = '")
        sql.Append(Quiz_Id)
        sql.Append("' AND tblQuizScore.Student_Id = '")
        sql.Append(PlayerId)
        sql.Append("';")
        GetSilverAndGoldCoin = New ClassConnectSql().getdata(sql.ToString(), , InputConn)

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetStudentPoint(ByVal PlayerId As String, ByRef InputConn As SqlConnection) As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim db As New ClassConnectSql()
        Dim dtCoin As DataTable = GetSilverAndGoldCoin(PlayerId, InputConn)
        Dim Silver = dtCoin.Rows(0)("Silver")
        Dim Gold
        If IsDBNull(dtCoin.Rows(0)("Gold")) Then
            Gold = 0
        Else
            Gold = dtCoin.Rows(0)("Gold")
        End If
        Dim js As New JavaScriptSerializer()
        Dim JsonString As New ArrayList
        JsonString.Add(New With {.Silver = CStr(Silver), .Gold = CStr(Gold), .NewSilver = CStr(Silver), .NewGold = CStr(Gold)})
        GetStudentPoint = js.Serialize(JsonString)
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function CheckIsNewStudentPhoto() As String

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM t360_tblStudent INNER JOIN tblStudentPhoto ON t360_tblStudent.Student_Id = tblStudentPhoto.Student_Id INNER JOIN " & _
                            " t360_tblTeacherRoom ON t360_tblStudent.School_Code = t360_tblTeacherRoom.School_Code AND t360_tblStudent.Student_CurrentClass " & _
                            " = t360_tblTeacherRoom.Class_Name AND t360_tblStudent.Student_CurrentRoom = t360_tblTeacherRoom.Room_Name " & _
                            " WHERE (t360_tblTeacherRoom.Teacher_Id = '" & HttpContext.Current.Session("UserId").ToString() & "') " & _
                            " AND (t360_tblStudent.Student_IsActive = 1) AND (t360_tblTeacherRoom.TR_IsActive = 1) AND (dbo.tblStudentPhoto.Approval_Status = 0) "
        Dim IsHaveNewPhoto As Integer = CInt(_DB.ExecuteScalar(sql))
        If IsHaveNewPhoto > 0 Then
            Return True
        Else
            Return False
        End If
    End Function


End Class