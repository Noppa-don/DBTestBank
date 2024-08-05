Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.IO
Imports KnowledgeUtils.Database.Command
Imports KnowledgeUtils.IO

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SyncService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function UploadFile(ByVal f As Byte(), ByVal fileName As String) As String
        Dim IO As New ManageFile()
        IO.CreateFolder("TempData", System.Web.Hosting.HostingEnvironment.MapPath("~/"))
        Dim ms As New MemoryStream(f)
        Dim NameArr = Strings.Split(fileName, ".")
        Dim FullPath As String = System.Web.Hosting.HostingEnvironment.MapPath("~/TempData/") & NameArr(0).ToString & Now.Day.ToString & Now.Month.ToString & Now.Year.ToString & Now.Hour.ToString & Now.Minute.ToString & "." & NameArr(1).ToString
        Dim fs As New FileStream(FullPath, FileMode.Create)
        ms.WriteTo(fs)
        ' clean up
        ms.Close()
        fs.Close()
        fs.Dispose()

        Dim SystemMn As New Service.ClsSystem(New ClassConnectSql)
        Dim SqlMn As New ClassConnectSql
        Dim CmUtil As New ManageCommandSql(KnowledgeUtils.Database.Command.EnDatabaseType.SqlServer)
        CmUtil.AutoID = False
        Dim gen As New GenerateDataSet(FullPath, EnumLoad.XmlZipPath)
        Dim tables = gen.GetTablesName
        Try
            SqlMn.OpenWithTransection()
            For Each Table In tables
                Dim DtServer = SystemMn.GetTableWithSchemaByLastUpdate(Table)
                Dim DtClient = gen.CreateDataTable(Table)
                Dim Sql As String
                Dim Sb As New StringBuilder
                For Each Row In DtClient.Rows
                    Dim Keys = DtServer.PrimaryKey
                    Dim KeyName = Keys(0).ColumnName
                    Dim KeyValue = Row(KeyName)

                    Dim RowServer = DtServer.Rows.Find(KeyValue)
                    Dim IsAdd As Boolean = If(RowServer Is Nothing, True, False)
                    If IsAdd Then
                        Sql = CmUtil.CreateCommandSql(DtServer, Row, EnActionType.Insert, Table)
                    Else
                        Sql = CmUtil.CreateCommandSql(DtServer, Row, EnActionType.Update, Table)
                    End If
                    Sb.Append(Sql)
                Next
                If Sb.Length > 0 Then
                    SqlMn.ExecuteWithTransection(Sb.ToString)
                End If
            Next
            'SqlMn.RollbackTransection()
            SqlMn.CommitTransection()
            Return "ok"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            SqlMn.RollbackTransection()
            Return "not"
        End Try
    End Function


End Class