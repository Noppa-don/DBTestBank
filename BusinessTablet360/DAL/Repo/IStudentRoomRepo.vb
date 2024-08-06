Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

Public Interface IStudentRoomRepo
    'Primary key = School_COde, Student_Id, Class_Name, Room_Name, SR_AcademicYear
    Sub InsertStudentRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom)
    Sub DeleteStudentRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom)
    Sub UpdateStudentRoom(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom, ByVal ItemOld As t360_tblStudentRoom)
End Interface

Public Class StudentRoomRepo
    Implements IStudentRoomRepo

    Public Sub InsertStudentRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom) Implements IStudentRoomRepo.InsertStudentRoom
        Ctx.t360_tblStudentRooms.InsertOnSubmit(Item)
        Ctx.SubmitChanges()
    End Sub

    Public Sub DeleteStudentRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom) Implements IStudentRoomRepo.DeleteStudentRoom
        'ไม่ลบจิง
        Dim Data = (From r In Ctx.t360_tblStudentRooms Where r.Student_Id = Item.Student_Id And _
                    r.School_Code = Item.School_Code And r.Class_Name = Item.Class_Name _
                    And r.Room_Name = Item.Room_Name And r.SR_AcademicYear = Item.SR_AcademicYear).SingleOrDefault
        Data.SR_IsActive = False
        Ctx.SubmitChanges()
    End Sub

    Public Sub UpdateStudentRoom(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblStudentRoom, ByVal ItemOld As t360_tblStudentRoom) Implements IStudentRoomRepo.UpdateStudentRoom
        Dim Target As New t360_tblStudentRoom
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblStudentRooms Where r.Student_Id = ItemOld.Student_Id And r.School_Code = ItemOld.School_Code _
                                                            And r.Room_Name = ItemOld.Room_Name And r.Class_Name = ItemOld.Class_Name _
                                                            And r.SR_AcademicYear = ItemOld.SR_AcademicYear).SingleOrDefault
        End Using
        ctx.t360_tblStudentRooms.Attach(Item, Target)
        ctx.SubmitChanges()
    End Sub

End Class