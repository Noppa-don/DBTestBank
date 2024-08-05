Imports System.Web

Public Class RegistereQuizCreator
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <Services.WebMethod()>
    Public Shared Function CheckEmailInDB(ByVal Email As String)
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQC_User WHERE QCU_Email = '" & _DB.CleanString(Email.Trim()) & "' AND IsActive = 1 "
        Dim CheckEmail As Integer = CInt(_DB.ExecuteScalar(sql))
        If CheckEmail > 0 Then
            _DB = Nothing
            Return "AlreadyUse"
        Else
            _DB = Nothing
            Return "OK"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function GetDataProvince()
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT ProvinceName FROM dbo.tblProvince WHERE IsActive = 1 ORDER BY ProvinceId "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim StrProvince As String = ""
        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                StrProvince &= dt.Rows(index)("ProvinceName") & ","
            Next
            If StrProvince.EndsWith(",") = True Then
                StrProvince = StrProvince.Substring(0, StrProvince.Length - 1)
            End If
        End If
        Return StrProvince
    End Function

    <Services.WebMethod()>
    Public Shared Function GetDataAmphur(ByVal ProvinceName As String)
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT dbo.tblAmphur.AmphurName FROM dbo.tblProvince INNER JOIN dbo.tblAmphur ON dbo.tblProvince.ProvinceId = dbo.tblAmphur.ProvinceId " & _
                            " WHERE dbo.tblProvince.ProvinceName = '" & _DB.CleanString(ProvinceName.Trim()) & "' AND dbo.tblAmphur.IsActive = 1 AND dbo.tblProvince.IsActive = 1 ORDER BY dbo.tblAmphur.AmphurId "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim StrAmphur As String = ""
        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                StrAmphur &= dt.Rows(index)("AmphurName") & ","
            Next
            If StrAmphur.EndsWith(",") = True Then
                StrAmphur = StrAmphur.Substring(0, StrAmphur.Length - 1)
            End If
        End If
        _DB = Nothing
        Return StrAmphur
    End Function

    <Services.WebMethod()>
    Public Shared Function GetDataSchool(ByVal AmphurName As String)
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT SchoolName FROM dbo.tblAmphur INNER JOIN dbo.tblSchool ON dbo.tblAmphur.AmphurId = dbo.tblSchool.AmphurId " & _
                            " WHERE dbo.tblAmphur.AmphurName = '" & _DB.CleanString(AmphurName.Trim()) & "' AND dbo.tblAmphur.IsActive =1 AND dbo.tblSchool.IsActive = 1 ORDER BY SchoolId "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Dim StrSchool As String = ""
        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                StrSchool &= dt.Rows(index)("SchoolName") & ","
            Next
            If StrSchool.EndsWith(",") = True Then
                StrSchool = StrSchool.Substring(0, StrSchool.Length - 1)
            End If
        End If
        _DB = Nothing
        Return StrSchool
    End Function

    <Services.WebMethod()>
    Public Shared Function CheckProvinceIsExistInDB(ByVal ProvinceName As String) As String
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblProvince WHERE ProvinceName = '" & _DB.CleanString(ProvinceName.Trim()) & "' AND IsActive = 1 "
        Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql))
        If CountCheck > 0 Then
            _DB = Nothing
            Return "True"
        Else
            _DB = Nothing
            Return "False"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function CheckAmphurIsExistInDB(ByVal AmphurName As String)
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblAmphur WHERE AmphurName = '" & _DB.CleanString(AmphurName.Trim()) & "' AND IsActive = 1 "
        Dim CountCheck As Integer = CInt(_DB.ExecuteScalar(sql))
        If CountCheck > 0 Then
            _DB = Nothing
            Return "True"
        Else
            _DB = Nothing
            Return "False"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function InsertQcUser(ByVal Email As String, ByVal QCPassword As String, ByVal FirstName As String, ByVal LastName As String, ByVal ProvinceName As String, ByVal AmphurName As String, ByVal SchoolName As String, ByVal ClassName As String, ByVal GroupSubjectName As String, ByVal Phone As String) As String
        Dim _DB As New ClassConnectSql()
        Try

            Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQC_User WHERE QCU_Email = '" & _DB.CleanString(Email.Trim()) & "' AND IsActive = 1 "
            Dim CheckEmailIsAlreadyUse As Integer = CInt(_DB.ExecuteScalar(sql))
            If CheckEmailIsAlreadyUse > 0 Then
                _DB = Nothing
                Return "EmailAlreadyUse"
            End If

            sql = " SELECT ProvinceId FROM dbo.tblProvince WHERE ProvinceName = '" & _DB.CleanString(ProvinceName.Trim()) & "' AND IsActive = 1 "
            Dim ProvinceId As Integer = CInt(_DB.ExecuteScalar(sql))
            sql = " SELECT AmphurId FROM dbo.tblAmphur WHERE AmphurName = '" & _DB.CleanString(AmphurName.Trim()) & "' AND IsActive = 1 "
            Dim AmphurId As Integer = CInt(_DB.ExecuteScalar(sql))
            sql = " SELECT SchoolId FROM dbo.tblSchool WHERE SchoolName = '" & _DB.CleanString(SchoolName.Trim()) & "' AND IsActive = 1 "
            Dim SchoolId As String = _DB.ExecuteScalar(sql)
            If SchoolId = "" Then
                SchoolId = "NULL"
            End If

            sql = " INSERT INTO dbo.tblQC_User( QCU_Id ,QCU_Email ,QCU_Password ,QCU_FirstName ,QCU_LastName ," &
                  " QCU_Type ,QCU_Name ,QCU_ProvinceId ,QCU_AmphurId ,QCU_SchoolId ,QCU_SchoolName , " &
                  " QCU_ClassName ,QCU_GroupsubjectName ,QCU_Phone ,IsActive ,LastUpdate ) " &
                  " VALUES  ( NEWID() ,'" & _DB.CleanString(Email.Trim()) & "' , '" & Encryption.MD5(_DB.CleanString(QCPassword.Trim())) & "' " &
                  " ,'" & _DB.CleanString(FirstName.Trim()) & "' ,'" & _DB.CleanString(LastName.Trim()) & "' ,0 ,NULL , " & ProvinceId & "," & AmphurId & " ,  " &
                  " " & SchoolId & " , '" & _DB.CleanString(SchoolName.Trim()) & "' ,'" & _DB.CleanString(ClassName.Trim()) & "' , '" & _DB.CleanString(GroupSubjectName.Trim()) & "' ,'" & _DB.CleanString(Phone.Trim()) & "' ,1,dbo.GetThaiDate()) "
            _DB.Execute(sql)
            _DB = Nothing
            Return "Complete"
        Catch ex As Exception
            _DB = Nothing
            Return "Error"
        End Try
    End Function

    <Services.WebMethod()>
    Public Shared Function InsertQCUserOther(ByVal Email As String, ByVal QCPassword As String, ByVal FirstName As String, ByVal LastName As String, ByVal TypeName As String) As String
        Dim _DB As New ClassConnectSql()
        Try
            Dim sql As String = " INSERT INTO dbo.tblQC_User ( QCU_Id ,QCU_Email ,QCU_Password ,QCU_FirstName ,QCU_LastName , " &
                                " QCU_Type ,QCU_Name ,QCU_ProvinceId ,QCU_AmphurId ,QCU_SchoolId ,QCU_SchoolName , " &
                                " QCU_ClassName ,QCU_GroupsubjectName ,QCU_Phone ,IsActive ,LastUpdate ) " &
                                " VALUES  ( NEWID() ,'" & _DB.CleanString(Email.Trim()) & "' , " &
                                " '" & Encryption.MD5(_DB.CleanString(QCPassword.Trim())) & "' ,'" & _DB.CleanString(FirstName.Trim()) & "' ,'" & _DB.CleanString(LastName.Trim()) & "' , 1 ,'" & _DB.CleanString(TypeName.Trim()) & "' , " &
                                " NULL, NULL , NULL , NULL ,NULL , NULL ,NULL ,1,dbo.GetThaiDate()) "
            _DB.Execute(sql)
            _DB = Nothing
            Return "Complete"
        Catch ex As Exception
            _DB = Nothing
            Return "Error"
        End Try
    End Function


End Class