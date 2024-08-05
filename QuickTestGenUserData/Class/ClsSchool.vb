
Public Class clsSchool
    Dim Connection As New ClassConnectSql()
    Dim ClsValidate As New ClsValidateData

    Public Function GetProvince() As DataTable
        Dim dtProvice As DataTable
        Dim sql As String = ""
        sql = "select ProvinceId,ProvinceName from tblProvince where IsActive = 1  order by ProvinceId;"
        dtProvice = Connection.getdata(sql)

        Dim R As DataRow = dtProvice.NewRow
        R("ProvinceName") = "เลือกจังหวัด"
        R("ProvinceId") = 0
        dtProvice.Rows.Add(R)

        Return ClsValidate.OrderbyDatatable(dtProvice, "ProvinceId")

    End Function
    Public Function GetAmphur(ProviceId As String) As DataTable
        Dim dtAmphur As DataTable
        Dim sql As String = ""
        sql = "select AmphurId,AmphurName from tblAmphur where IsActive = 1 and ProvinceId = '" & ProviceId & "' order by AmphurId;"
        dtAmphur = Connection.getdata(sql)

        Dim R As DataRow = dtAmphur.NewRow
        R("AmphurName") = "เลือกอำเภอ"
        R("AmphurId") = 0
        dtAmphur.Rows.Add(R)

        Return ClsValidate.OrderbyDatatable(dtAmphur, "AmphurId")

    End Function
    Public Function GetTambol(AmphurId As String) As DataTable
        Dim dtTambol As DataTable
        Dim sql As String = ""
        sql = "select TambolId,TambolName from tblTambol where IsActive = 1 and AmphurId = '" & AmphurId & "' Order by TambolId;"
        dtTambol = Connection.getdata(sql)

        Dim R As DataRow = dtTambol.NewRow
        R("TambolName") = "เลือกตำบล"
        R("TambolId") = 0
        dtTambol.Rows.Add(R)

        Return ClsValidate.OrderbyDatatable(dtTambol, "TambolId")

    End Function


    Public Function GetSchool(Optional ProvinceId As String = "", Optional AmphurId As String = "", Optional TambolId As String = "", Optional SchoolName As String = "") As DataTable
        Dim dtSchool As DataTable
        Dim sql As String = ""

        sql = "Select SchoolId,REPLACE(SchoolName,'โรงเรียน','') as SchoolName from tblSchool Where IsActive = '1'"

        If ProvinceId <> "" Then
            sql &= " And ProvinceId = '" & ProvinceId & "'"
        End If

        If AmphurId <> "" Then
            sql &= " And AmphurId = '" & AmphurId & "'"
        End If

        If TambolId <> "" Then
            sql &= " And TambolId = '" & TambolId & "'"
        End If

        If SchoolName <> "" Then
            sql &= "And SchoolName Like '%" & SchoolName & "%'"
        End If

        sql &= ";"
        dtSchool = Connection.getdata(sql)

        Dim R As DataRow = dtSchool.NewRow
        R("SchoolName") = "เลือกโรงเรียน"
        R("SchoolId") = 0
        dtSchool.Rows.Add(R)

        Return ClsValidate.OrderbyDatatable(dtSchool, "SchoolId")

    End Function
    Public Function GetSchoolDetail(SchoolId As String) As DataTable
        Dim dtSchoolDetail As DataTable
        Dim sql As String = "select REPLACE(SchoolName,'โรงเรียน','') as SchoolName,SchoolShortName from tblSchool where SchoolId = '" & SchoolId & "'"
        dtSchoolDetail = Connection.getdata(sql)
        Return dtSchoolDetail
    End Function
    Public Function GetSchoolAddressId(SchoolId As String) As DataTable
        Dim dtSchoolDetail As DataTable
        Dim sql As String = "select ProvinceId,AmphurId,TambolId from tblSchool where SchoolId = '" & SchoolId & "'"
        dtSchoolDetail = Connection.getdata(sql)
        Return dtSchoolDetail
    End Function
    Public Function GetPrefixSchool(SchoolId As String) As String
        Dim sql As String = ""
        sql = "select case when SchoolShortName is null then '' else SchoolShortName end as SchoolShortName from tblSchool where SchoolId = '" & SchoolId & "'"
        Dim SchoolPrefix As String = Connection.ExecuteScalar(sql)
        Return SchoolPrefix
    End Function
    Public Function UpdateSchoolShortName(SchoolId As String, SchoolShortName As String) As Boolean
        Try
            Dim sql As String
            sql = "Update tblSchool Set SchoolShortName = '" & SchoolShortName & "' where SchoolId = '" & SchoolId & "';"
        Catch ex As Exception

        End Try
    End Function

End Class

Public Class DBOSchool
    Public UserId As Integer
    Public SchoolShortName As String
End Class
