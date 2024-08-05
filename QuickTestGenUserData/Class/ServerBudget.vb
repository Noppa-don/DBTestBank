Public Class ServerBudget
    Dim _DB As ClsConnect

    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Function SelectSubjectId(ByVal SubjectName As String) As String
        Dim sql As String = "Select SubjectId From tblSubject Where SubjectName like '%" & SubjectName & "%'"

        SelectSubjectId = _DB.ExecuteScalar(sql)
    End Function
    Public Function SelectSubjectName(ByVal SubjectId As String) As DataTable
        Dim sql As String
        If SubjectId = "*" Then
            sql = "Select SubjectName as a From tblSubject Where IsActive = 1"
        Else
            sql = "Select SubjectName From tblSubject Where SubjectId = " & SubjectId & " And IsActive = 1"
        End If
        SelectSubjectName = _DB.getdata(Sql)
    End Function

    Public Function SearchDetailUser(ByVal UserId As String) As DataTable
        Dim sql As String = "Select * from tblUserSubjectClass where UserId = '" & UserId.ToString & "' And IsActive = 1"
        SearchDetailUser = _DB.getdata(sql)
    End Function


    Public Function IsUserAllCondition(userId As String) As Boolean
        Dim sql As String = "select case when UserSubjectAmount = subjectAmount then 'true' else 'false' end as IsAll
                             from (select count(*) as UserSubjectAmount from tblUserSubjectClass where UserId = '" & userId & "' And tblUserSubjectClass.IsActive = 1)USA
                             ,(select count(*)  * 12 as subjectAmount from tblGroupSubject where IsActive = 1)SA "

        IsUserAllCondition = CBool(_DB.ExecuteScalar(sql))
    End Function

    Public Function GetUserSubjectClass(ByVal UserId As String, SubjectName As String) As DataTable
        Dim sql As String = " Select tblUserSubjectClass.SubjectId,replicate('0', 2-len(ClassId)) + ClassId as ClassId " & _
                            " from tblUserSubjectClass inner join tblsubject on tblUserSubjectClass.SubjectId = tblSubject.subjectId " & _
                            " where UserId = '" & UserId & "' And tblUserSubjectClass.IsActive = 1 and subjectName like '%" & SubjectName & "%' " & _
                            " order by tblUserSubjectClass.SubjectId,replicate('0', 2-len(ClassId)) + ClassId "

        GetUserSubjectClass = _DB.getdata(sql)
    End Function

    Public Function SearchOneDetailUser(ByVal UserId As String, ByVal detailid As Integer) As DataTable
        Dim sql As String = "Select * from tblUserSubjectClass where UserId = '" & UserId.ToString & "' And DetailId = " & detailid & " And IsActive = 1"
        SearchOneDetailUser = _DB.getdata(sql)
    End Function
    Public Function SearchUserById(ByVal UserId As String) As DataTable

        Dim sql As String = "select * from tbluser inner join t360_tblCalendar on tbluser.SchoolId = t360_tblCalendar.School_Code "
        sql &= " where tbluser.GUID = '" & UserId & "' And tblUser.IsActive = 1"
        SearchUserById = _DB.getdata(sql)

    End Function
    Public Function SearchSchoolByUserId(ByVal UserId As String) As Integer
        Dim sql As String = "Select SchoolId from tblUser where Guid = '" & UserId.ToString & "' And IsActive = 1"
        Dim dt As DataTable
        dt = _DB.getdata(sql)
        SearchSchoolByUserId = dt.Rows(0)("SchoolId")
    End Function

    Public Function SelectSchoolName(ByVal SchoolId As String) As String
        Dim sql As String = "Select SchoolName from tblSchool where SchoolId =" & SchoolId & " And IsActive = 1"
        Dim dt As DataTable
        dt = _DB.getdata(sql)
        SelectSchoolName = dt.Rows(0)("SchoolName")
    End Function
    Public Function SelectAllSchool() As DataTable
        Dim sql As String = "Select * from tblSchool where IsActive = 1"
        SelectAllSchool = _DB.getdata(sql)
    End Function

    Public Sub UnActive(ByVal UserId As String)
        Dim sql As String = "Update tblUserSubjectClass Set IsActive = 0,LastUpdate = GetDate() "
        sql = sql & "where UserId = '" & UserId & "';"
        _DB.Execute(sql)
    End Sub
    Public Sub UpdateUser(ByVal UserId As String, ByVal FirstName As String, ByVal LastName As String, ByVal UserName As String, ByVal Password As String, Optional UserExpireDate As String = Nothing)
        Dim strExpireDate As String

        If UserExpireDate IsNot Nothing Then
            strExpireDate = "'" & UserExpireDate & "'"
        Else
            strExpireDate = "null"
        End If

        Dim sql As String = ""
        sql = "Update tblUser Set FirstName = '" & _DB.CleanString(FirstName) & "', LastName = '" & _DB.CleanString(LastName) & "'"
        If Password <> "" Then
            sql &= ", Password = '" & Encryption.MD5(Password) & "'"
        End If
        sql &= ", UserName = '" & _DB.CleanString(UserName) & "', UserExpireDate = " & strExpireDate
        sql &= ",LastUpdate = GetDate() Where GUID = '" & UserId & "';"
        _DB.Execute(sql)
    End Sub

    Public Function InsertUser(ByVal UserId As String, ByVal FirstName As String, ByVal LastName As String, ByVal UserName As String, ByVal Password As String, ByVal SchoolId As Integer, Optional UserExpireDate As String = Nothing)
        Try
            Dim strExpireDate As String

            If UserExpireDate IsNot Nothing Then
                strExpireDate = "'" & UserExpireDate & "'"
            Else
                strExpireDate = "null"
            End If
            Dim Sql As String = "Insert Into tblUser (UserId,FirstName,LastName,UserName,Password,SchoolId,Guid,UserExpireDate)"
            Sql &= " Values (1,'" & _DB.CleanString(FirstName) & "' , '" & _DB.CleanString(LastName) & "', '" & _DB.CleanString(UserName) & "' , '" & Encryption.MD5(Password) & "'," & _DB.CleanString(SchoolId) & ",'" & UserId & "'," & strExpireDate & ")"
            _DB.Execute(Sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function InsertUserSubjectClass(ByVal USCID As Integer, ByVal UserId As String, ByVal DetailId As Integer, ByVal SubjectId As Integer, ByVal ClassId As Integer)

        Try
            Dim sql As String

            sql = " Insert Into tblUserSubjectClass(USCID,UserIdOld,DetailId,SubjectId,ClassId,IsActive, UserId, GroupSubjectId,Levelid)Values "
            sql &= " (" & USCID & ",1," & DetailId & "," & SubjectId & "," & ClassId & ",1,'" & UserId & "'," & _
                   " (select GroupSubject_Id from tblGroupSubject inner join tblSubject on tblGroupSubject.GroupSubject_ShortName = tblsubject.subjectName " & _
                   " where subjectId = '" & SubjectId & "')" & ",(select level_id from tbllevel where level = '" & ClassId & "')) "
            _DB.Execute(sql)

            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function

    Public Function SelectDetail(ByVal UserId As String) As DataTable
        Dim sql = "Select Distinct DetailId From tblUserSubjectClass Where UserId = '" & UserId.ToString & "' And IsActive = 1"
        SelectDetail = _DB.getdata(sql)
    End Function


    Public Function MaxAuto(ByVal Table As String, ByVal Field As String) As Integer

        Dim sql As String = "Select max(" & Field & ") as Maxid from " & Table
        Dim dtMax As DataTable = _DB.getdata(sql)
        If dtMax.Rows(0)("Maxid") Is DBNull.Value Then
            MaxAuto = 1

        Else
            MaxAuto = CInt(dtMax.Rows(0)("Maxid")) + 1
        End If


    End Function


    Public Function SetCalendar(SchoolCode As String, CalendarYear As String, CalendarName As String, CalendarFrom As String, CalendarTo As String) As Boolean
        Dim sql As String

        Try
            sql = "select Calendar_Id,case when Calendar_ToDate < '" & CalendarTo & "' then 'true' else 'false' end as IsMoreExpire from t360_tblCalendar where School_Code = '" & SchoolCode & "' ;"
            Dim oldCalendar As DataTable = _DB.getdata(sql)

            If oldCalendar.Rows.Count = 0 Then
                sql = "insert into t360_tblCalendar"
                sql &= " select '" & SchoolCode & "','" & CalendarYear & "','" & CalendarName & "',convert(smalldatetime,'" & CalendarFrom & "',103), convert(smalldatetime,'" & CalendarTo & "',120),'3',newid();"

            Else
                If oldCalendar.Rows(0)("IsMoreExpire").ToString = "true" Then
                    sql = "Update t360_tblCalendar set Calendar_FromDate = convert(smalldatetime,'" & CalendarFrom & "',103) ,Calendar_ToDate = convert(smalldatetime,'" & CalendarTo & "',120) where School_Code = '" & SchoolCode & "';"
                End If
            End If
            _DB.Execute(sql)
        Catch ex As Exception

        End Try

        Return True

    End Function

    'Public Function UpdateCalendar(SchoolCode As String, CalendarFrom As String, CalendarTo As String, CalendarYear As String) As Boolean
    '    Dim sql As String
    '    Try
    '        sql = "select Calendar_Id,case when Calendar_ToDate < '" & CalendarTo & "' then 'true' else 'false' end as IsMoreExpire from t360_tblCalendar where School_Code = '" & SchoolCode & "' ;"
    '        Dim oldCalendar As DataTable = _DB.getdata(sql)

    '        If oldCalendar.Rows.Count = 0 Then
    '            sql = "insert into t360_tblCalendar"
    '            sql &= " select '" & SchoolCode & "','" & CalendarYear & "','" & CalendarName & "',convert(smalldatetime,'" & CalendarFrom & "',103), convert(smalldatetime,'" & CalendarTo & "',120),'3',newid();"

    '        Else
    '            If oldCalendar.Rows(0)("IsMoreExpire").ToString = "true" Then
    '                sql = "Update t360_tblCalendar set Calendar_FromDate = convert(smalldatetime,'" & CalendarFrom & "',103) ,Calendar_ToDate = convert(smalldatetime,'" & CalendarTo & "',120) where School_Code = '" & SchoolCode & "';"
    '            End If
    '        End If
    '        _DB.Execute(sql)
    '    Catch ex As Exception

    '    End Try

    '    Return True
    'End Function

    Public Function SetDate(fDate As Date) As String

        Dim SDate As String = fDate.Month & "/" & fDate.Day & "/" & fDate.Year & " " & fDate.Hour & ":" & fDate.Minute & ":" & fDate.Second
        Return SDate

    End Function

    Public Sub DeleteUser(ByVal UserId As String)
        UnActive(UserId)
        Dim sql As String = ""
        sql = "Update tbluser set isactive = '0' where GUID = '" & UserId & "'; "
        _DB.Execute(sql)

        sql = "Select * From tblUser where IsActive = '1' And SchoolId in (select SchoolId from tbluser where guid = '" & UserId & "'); "

        Dim dt As DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count = 0 Then
            sql = "Delete t360_tblCalendar where SchoolId in (select SchoolId from tbluser where guid = '" & UserId & "');"
        End If

    End Sub

End Class
