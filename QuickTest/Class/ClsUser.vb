Public Class ClsUser

    Dim _DB As ClsConnect

    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub

    Public Function SelectAllUser() As DataTable

        Dim sql As String = "Select UserId,FirstName,LastName,UserName From tblUser Where IsActive = 1"

        SelectAllUser = _DB.getdata(sql)

    End Function

    Public Function SearchUser(ByVal Name As String) As DataTable

        Dim sql As String = "Select UserId,FirstName,LastName,UserName From tblUser Where IsActive = 1 and (FirstName like '%" & Name & "%' or UserName like '%" & Name & "%') "

        SearchUser = _DB.getdata(sql)

    End Function

    Public Function SearchByID(ByVal ID As Integer) As DataTable

        Dim sql As String = "Select FirstName,LastName,UserName,Password,PermissionSets FROM tblUser WHERE UserID = " & ID & " AND IsActive = 1 "
        SearchByID = _DB.getdata(sql)

    End Function

    Public Sub InsertUser(ByVal FName As String, ByVal LName As String, ByVal UName As String, ByVal Pass As String, ByVal Permission As String, Optional ByVal timeupdate As String = "")

        Dim sql As String = "Insert into tblUser (FirstName,LastName,UserName,Password,IsActive,PermissionSets,LastUpdate) "
        sql = sql & "VALUES ('" & _DB.CleanString(FName) & "','" & _DB.CleanString(LName) & "','" & _DB.CleanString(UName) & "','" & _DB.CleanString(Pass) & "',1,'" & Permission & "', datetime('now','localtime') );"
        _DB.Execute(sql)

    End Sub

    Public Sub InsertUserServer(ByVal Id As Integer, ByVal FName As String, ByVal LName As String, ByVal UName As String, ByVal Pass As String, ByVal Permission As String, ByVal timeupdate As String)

        Dim sql As String = "Insert into tblUser (UserId,FirstName,LastName,UserName,Password,IsActive,PermissionSets,LastUpdate) "
        sql = sql & "VALUES (" & Id & ",'" & _DB.CleanString(FName) & "','" & _DB.CleanString(LName) & "','" & _DB.CleanString(UName) & "','" & _DB.CleanString(Pass) & "',1,'" & Permission & "','" & timeupdate & "')"
        _DB.Execute(sql)

    End Sub

    Public Sub UpdateUser(ByVal Id As Integer, ByVal FName As String, ByVal LName As String, ByVal UName As String, ByVal Pass As String, ByVal PermissionSets As String, Optional ByVal timeupdate As String = "")

        Dim sql As String = "Update tblUser Set FirstName = '" & _DB.CleanString(FName) & "',LastName = '" & _DB.CleanString(LName) & "',UserName = '" & _DB.CleanString(UName) & "', "
        sql = sql & " Password = '" & _DB.CleanString(Pass) & "',PermissionSets = '" & PermissionSets & "',LastUpdate = datetime('now','localtime'),ClientId = NULL "
        sql = sql & " Where UserId = '" & Id & "';"
        _DB.Execute(sql)

    End Sub

    Public Sub UpdateStatus(ByVal id As Integer)

        Dim sql As String = "Update tblUser Set IsActive = '0', LastUpdate = datetime('now','localtime'),ClientId = NULL "
        sql = sql & " Where UserId = '" & id & "';"
        _DB.Execute(sql)

    End Sub

    Public Function GetStudentHasPhoto(ByVal Student_id As String) As Boolean

        Dim sql As String
        sql = "select Student_HasPhoto from t360_tblStudent where Student_Id = '" & Student_id & "' and Student_IsActive = '1'"

        Dim Student_HasPhoto As String = _DB.ExecuteScalar(sql)

        Dim HasPhoto As Boolean
        If Student_HasPhoto = "1" Then HasPhoto = True Else HasPhoto = False

        Return HasPhoto

    End Function

    Public Function CheckEmptySession(UserId As String, ClientIPAddress As String) As Boolean
        'ทุกครั้งที่ลงทะเบียน 
        '    ถ้าไม่มี User + IP นี้ ให้ CreateLoginSession ใหม่ และ Return True ได้เลย
        '    ถ้ามี User นี้ แต่ Session_Out Is Not null ให้ CreateLoginSession ใหม่ และ Return True ได้เลย 
        '    ถ้ามี User นี้ และ Session_Out Is Null ให้ Return False (แสดง Dialog ถามว่าจะ LogOut หรือไม่)
    End Function
    Public Sub CreateLoginSession(UserId As String, ClientIPAddress As String)
        'สร้าง Session ใหม่ให้ User
    End Sub

    Public Sub CloseLoginSession(UserId As String, ClentIPAddress As String)
        'ปิด Session ที่ค้างอยู่
    End Sub

End Class
