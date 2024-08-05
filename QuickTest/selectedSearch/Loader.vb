Imports System.Data.SqlClient

Public Class Loader
    'Dim db As New ClassConnectSql()
    Public Str As String = ""
    'Dim ClsKNConfig As New KNConfigData()

    'ต้องถอดรหัส ConnectionString ก่อน
    Public Sub New()
        Dim StrDecryption As String = System.Configuration.ConfigurationManager.ConnectionStrings("DictionaryConnectionString").ConnectionString
        'Str = ClsKNConfig.DecryptData(StrDecryption)
        Str = KNConfigData.DecryptData(StrDecryption)
    End Sub

    Public conn As New SqlConnection
    Public ds As New DataSet
    Public Function LoadAllData() As Dictionary(Of String, String)
        Dim dt As New DataTable()
        Dim dictData As New Dictionary(Of String, String)()
        'dt = getdata("SELECT esearch AS keyWord, returndata ,'2' as SortBy FROM   dbo.eng2thai " & _
        ' " union  SELECT tsearch , returndata ,'1' FROM   dbo.thai2eng  order by sortBy, keyword;")

        dt = getdata("SELECT esearch AS keyWord, returndata ,'2' as SortBy , tentry , ecat , CASE esyn WHEN '' THEN '1' ELSE '2' END AS related FROM dbo.eng2thai " & _
                     " UNION  SELECT tsearch , returndata , '1' , tentry , tcat , CASE tsyn WHEN '' THEN '1' ELSE '2' END FROM dbo.thai2eng ORDER BY sortBy , keyword , ecat , related , tentry; ")

        Dim verb As String = ""

        For Each dr In dt.Rows

            If dictData.ContainsKey(dr(0)) = False Then
                dictData.Add(dr(0), dr(1))
                verb = dr(4)
            Else
                'check ก่อนว่า verb เดิมหรือเปล่า
                Dim newData As String = ""
                If (verb = dr(4)) And (dr(5) = "1") Then
                    Dim rep As String = "<b><i>" & dr(4).ToString() & "</i></b> :"
                    newData = " , " & dr(1).ToString().Replace(rep, "")
                Else
                    newData = "<br /><hr /> " + dr(1)
                    verb = dr(4)
                End If

                Dim tmpData As String = ""
                If dictData.TryGetValue(dr(0), tmpData) Then
                    dictData.Remove(dr(0))
                    'dictData.Add(dr(0), tmpData + "<br /><hr /><br /> " + dr(1))
                    dictData.Add(dr(0), tmpData + newData)
                End If

            End If
        Next

        Return dictData
    End Function

    Public Function getdata(ByVal sql As String, Optional ByVal TableName As String = "0") As System.Data.DataTable
        OpenConnect()
        ds = New DataSet()
        Dim da As New SqlDataAdapter(sql, conn)
        'da.FillSchema(ds, SchemaType.Mapped, TableName)'เอาโครงสร้าง DB มาด้วย
        da.Fill(ds, TableName)
        getdata = ds.Tables(0)
        CloseConnect()
    End Function

    Public Sub OpenConnect()
        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If

        conn.ConnectionString = Str
        conn.Open()
    End Sub

    Public Sub CloseConnect()
        conn.Close()
        conn.Dispose()
    End Sub

End Class
