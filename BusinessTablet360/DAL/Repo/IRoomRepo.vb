Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database

''' <summary>
''' (PK Database) Room_Id 
''' (PK Real) School_Code, Class_Name, Room_Name
''' </summary>
''' <remarks></remarks>
Public Interface IRoomRepo

    Function GetRoomByClassName(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) As t360_tblRoom()
    Function GetRoomByRoomId(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) As t360_tblRoom
    Sub UpdateRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom)
    Sub DeleteRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom)
    Function ValidateDuplicateRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Class_Name As String, _
                                   ByVal Room_Name As String, Optional ByVal Room_Id As Guid? = Nothing) As Boolean
    Function GetStudentRoomByClassName(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) As t360_tblRoom()


End Interface

Public Class RoomRepo
    'Inherits DatabaseManager
    Implements IRoomRepo

    Public Function GetStudentRoomByClassName(Ctx As DataClassesTablet360DataContext, Item As t360_tblRoom) As t360_tblRoom() Implements IRoomRepo.GetStudentRoomByClassName
        Dim Result = (From r1 In Ctx.t360_tblRooms Join r2 In Ctx.t360_tblStudents On r1.Class_Name Equals r2.Student_CurrentClass _
                      And r1.Room_Name Equals r2.Student_CurrentRoom Where r1.Class_Name = Item.Class_Name _
                      AndAlso r1.Room_IsActive = Item.Room_IsActive AndAlso r1.School_Code = Item.School_Code _
                      AndAlso r2.Student_IsActive = True Select r1).Distinct
        Return Result.ToArray
    End Function

    Public Function GetRoomByClassName(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) As t360_tblRoom() Implements IRoomRepo.GetRoomByClassName
        Dim Result = (From r In Ctx.t360_tblRooms Where r.Class_Name = Item.Class_Name _
                      AndAlso r.Room_IsActive = Item.Room_IsActive AndAlso r.School_Code = Item.School_Code)
        Dim ListResult = Result.ToList()
        'ListResult.Sort(Function(x, y)
        '                    Dim Compare = ("00000000000000000000000000000000000000000000000000" & x.Class_Name).CompareTo(("00000000000000000000000000000000000000000000000000" & y.Class_Name))
        '                    If Compare <> 0 Then
        '                        Return Compare
        '                    End If
        '                    Dim FixedLengthXRoomName As String = ("00000000000000000000000000000000000000000000000000" + x.Room_Name.Replace("/", "")).Substring((x.Room_Name.Replace("/", "")).Length)
        '                    Dim FixedLengthYRoomName As String = ("00000000000000000000000000000000000000000000000000" + y.Room_Name.Replace("/", "")).Substring((y.Room_Name.Replace("/", "")).Length)
        '                    Return (FixedLengthXRoomName).CompareTo(FixedLengthYRoomName)
        '                End Function)
        ListResult.Sort(AddressOf CompareFixedLengthClassRoom)
        Return ListResult.ToArray
    End Function

    Private Function CompareFixedLengthClassRoom(x As t360_tblRoom, y As t360_tblRoom) As Integer
        'Dim result As Integer = x.Class_Name.CompareTo(y.Class_Name)
        'If result = 0 Then
        '    Dim FixedLengthXRoomName As String = ("00000000000000000000000000000000000000000000000000" + x.Room_Name).Substring(x.Room_Name.Length)
        '    Dim FixedLengthYRoomName As String = ("00000000000000000000000000000000000000000000000000" + y.Room_Name).Substring(y.Room_Name.Length)
        '    result = FixedLengthXRoomName.CompareTo(FixedLengthYRoomName)
        'End If
        'Return result
        Dim Compare = ("00000000000000000000000000000000000000000000000000" & x.Class_Name).CompareTo(("00000000000000000000000000000000000000000000000000" & y.Class_Name))
        If Compare <> 0 Then
            Return Compare
        End If
        Dim FixedLengthXRoomName As String = ("00000000000000000000000000000000000000000000000000" + x.Room_Name.Replace("/", "")).Substring((x.Room_Name.Replace("/", "")).Length)
        Dim FixedLengthYRoomName As String = ("00000000000000000000000000000000000000000000000000" + y.Room_Name.Replace("/", "")).Substring((y.Room_Name.Replace("/", "")).Length)
        Return (FixedLengthXRoomName).CompareTo(FixedLengthYRoomName)

    End Function

    Public Function GetRoomByRoomId(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) As t360_tblRoom Implements IRoomRepo.GetRoomByRoomId
        Dim Result = (From r In Ctx.t360_tblRooms Where r.Room_Id = Item.Room_Id AndAlso r.Room_IsActive = Item.Room_IsActive AndAlso r.School_Code = Item.School_Code).SingleOrDefault
        Return Result
    End Function

    Public Sub DeleteRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) Implements IRoomRepo.DeleteRoom
        Dim Ori = (From r In Ctx.t360_tblRooms Where r.Room_Id = Item.Room_Id AndAlso r.School_Code = Item.School_Code).SingleOrDefault
        Ori.Room_IsActive = False
        Ori.LastUpdate = Now
        Ctx.SubmitChanges()
    End Sub

    Public Sub UpdateRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblRoom) Implements IRoomRepo.UpdateRoom
        'Dim Ori As New t360_tblRoom
        'Using ctx1 = GetDataContext()
        '    Ori = (From r In Ctx.t360_tblRooms Where r.Room_Id = Item.Room_Id AndAlso r.School_Code = Item.School_Code).SingleOrDefault
        'End Using
        'Ctx.t360_tblRooms.Attach(Item, Ori)
        'Ctx.SubmitChanges()
        Dim Ori = (From r In Ctx.t360_tblRooms Where r.Room_Id = Item.Room_Id AndAlso r.School_Code = Item.School_Code).SingleOrDefault
        Ori.Room_Name = Item.Room_Name
        Ori.LastUpdate = Now
        Ctx.SubmitChanges()
    End Sub

    Public Function ValidateDuplicateRoom(ByVal Ctx As DataClassesTablet360DataContext, ByVal School_Code As String, ByVal Class_Name As String, ByVal Room_Name As String, Optional ByVal Room_Id As Guid? = Nothing) As Boolean Implements IRoomRepo.ValidateDuplicateRoom
        If Room_Id Is Nothing Then
            Dim q = (From r In Ctx.t360_tblRooms Where r.Class_Name = Class_Name AndAlso _
                                                  r.Room_Name = Room_Name AndAlso _
                                                  r.Room_IsActive = True AndAlso _
                                                  r.School_Code = School_Code).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblRooms Where r.Class_Name = Class_Name AndAlso _
                                                  r.Room_Name = Room_Name AndAlso _
                                                  r.Room_IsActive = True AndAlso _
                                                  r.School_Code = School_Code AndAlso _
                                                  r.Room_Id <> Room_Id).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

End Class
