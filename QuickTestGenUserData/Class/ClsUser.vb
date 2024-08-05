
Public Class ClsUser
    Dim Connection As New ClassConnectSql

    Public Function CheckUserLogin(UserName As String, txtPassword As String) As String
        Dim UserId As String = ""
        Dim MD5Password As String = Encryption.MD5(txtPassword)

        Dim sql As String = "Select UserId from tblUser where UserName = '" & UserName & "' and Password = '" & MD5Password & "' and IsActive = 1;"
        UserId = Connection.ExecuteScalar(sql).ToString

        Return UserId
    End Function
    Public Function GetUserInSchool(SchoolId As String) As DataTable
        Dim dtUser As DataTable
        Dim sql As String = "select guid as UserId,FirstName + ' ' + LastName as FullName,UserName from tblUser where SchoolId = '" & SchoolId & "' and IsActive = 1"
        dtUser = Connection.getdata(sql)
        Return dtUser
    End Function
    Public Function GetUserDetail(UserId As String) As DataTable
        Dim dtUserDetail As DataTable
        Dim sql As String = "select "
        dtUserDetail = Connection.getdata(sql)
        Return dtUserDetail
    End Function
    Public Function InsertUser(DBOUser As DBOUser) As String
        Try
            Dim sql As String = ""
            Dim IsC As Integer = 0
            If DBOUser.IsContact Then
                IsC = 1
            End If

            sql = "Select newid()"

            Dim NewUserId As String = Connection.ExecuteScalar(sql).ToString

            sql = "insert into tbluser SELECT ISNULL(MAX (userId),0) + 1,'" & DBOUser.FirstName & "','" & DBOUser.LastName & "','" & DBOUser.UserName & "','" & DBOUser.Password & "',
                   '" & DBOUser.SchoolId & "',1,getdate(),NULL,'" & NewUserId & "',1,1,1,1,1,0,'" & IsC & "','" & DBOUser.PasswordChar & "' from tblUser"
            Connection.Execute(sql)
            Return NewUserId
        Catch ex As Exception
            Return "False"
        End Try
    End Function
    Public Function InsertUserSubjectClass(ClassId As String, SubjectId As String, UserId As String) As Boolean
        Try
            Dim sql As String = ""
            sql = "insert into tblUserSubjectClass SELECT top 1 ISNULL(MAX (uscid),0) + 1,1,1," & SubjectId & "," & ClassId & ",(select GroupSubject_Id from tblGroupSubject 
                where subjectId = '" & SubjectId & "'),(select Level_Id from tblLevel where Level = '" & ClassId & "'),1,getdate(),null,newid(),'" & UserId & "' 
                from tblUserSubjectClass"
            Connection.Execute(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Function UpdateUser(DBOUser As DBOUser) As Boolean
        Try
            Dim IsC As Integer = 0
            If DBOUser.IsContact Then
                IsC = 1
            End If

            Dim sql As String = ""
            sql = "Update tblUser Set
                    FirstName = '" & DBOUser.FirstName & "', LastName = '" & DBOUser.LastName & "',
                    UserName = '" & DBOUser.UserName & "', Password = '" & DBOUser.Password & "',
                    isContact = '" & DBOUser.IsContact & "', PasswordChar = '" & DBOUser.PasswordChar & "',
                    LastUpdate = Getdate() where GUID = '" & DBOUser.GUID & "'"
            Connection.Execute(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function DeleteUser(UserGUID As String) As Boolean
        Try
            Dim sql As String
            sql = "Update tblUser= set IsActive = 0 where GUID = '" & UserGUID & "';"
            Connection.Execute(sql)

            If DeleteUserSubjectClass(UserGUID) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Function DeleteUserSubjectClass(UserGUID As String) As Boolean
        Try
            Dim sql As String
            sql = "Update tblUserSubjectClass set IsActive = 0 where UserId = '" & UserGUID & "';"
            Connection.Execute(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

End Class

Public Class DBOUser
    Public UserId As Integer
    Public FirstName As String
    Public LastName As String
    Public UserName As String
    Public Password As String
    Public SchoolId As String
    Public IsActive As String
    Public LastUpdate As String
    Public ClientId As String
    Public GUID As String
    Public IsAllowMenuManageUserSchool As String
    Public IsAllowMenuManageUserAdmin As String
    Public IsAllowMenuAdminLog As String
    Public IsAllowMenuContact As String
    Public IsAllowMenuSetEmail As String
    Public IsViewAllTips As String
    Public IsContact As Boolean
    Public PasswordChar As String
End Class
