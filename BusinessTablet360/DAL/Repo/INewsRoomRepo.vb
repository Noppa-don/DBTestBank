Imports System.Web
Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface INewsRoomRepo

    Function GetNewsRoomByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO) As T()
    Function GetNewsRoomByNewsId(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal News_Id As Guid) As t360_tblNewsRoom()
    Sub DeleteNewsRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO)

End Interface

Public Class NewsRoomRepo
    Implements INewsRoomRepo

    Public Function GetNewsRoomByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO) As T() Implements INewsRoomRepo.GetNewsRoomByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblNewsRoom WHERE {F}"
            Dim f As New SqlPart
            f.AddPart("School_Code={0}", Item.School_Code)
            f.AddPart("News_Id={0}", Item.News_Id)
            f.AddPart("Class_Name={0}", Item.Class_Name)
            f.AddPart("Room_Name={0}", Item.Room_Name)
            f.AddPart("IsActive={0}", True)
            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of T)(Ctx).ToArray
        End With

    End Function

    Public Function GetNewsRoomByNewsId(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal News_Id As Guid) As t360_tblNewsRoom() Implements INewsRoomRepo.GetNewsRoomByNewsId
        Return Ctx.t360_tblNewsRooms.Where(Function(n) n.School_Code = School_Code AndAlso n.News_Id = News_Id).ToArray()
    End Function

    Public Sub DeleteNewsRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As NewsDTO) Implements INewsRoomRepo.DeleteNewsRoom
        With GetLinqToSql
            .MainSql = "DELETE FROM t360_tblNewsRoom WHERE {F}"
            Dim f As New SqlPart
            f.AddPart("School_Code={0}", Item.School_Code)
            f.AddPart("News_Id={0}", Item.News_Id)
            .ApplySqlPart("F", f)

            .DataContextExecuteCommand(Ctx)
        End With
    End Sub

End Class
