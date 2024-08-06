Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils.System.DateTimeUtil
Imports KnowledgeUtils.Database.SqlServer

''' <summary>
''' (PK Database) Calendar_Id 
''' (PK Real) School_Code, Calendar_Year, Calendar_Name
''' </summary>
''' <remarks></remarks>
Public Interface ICalendarRepo

    Function GetCalendarByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As CalendarDTO) As T()
    Function GetCalendarForScheduler(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As CalendarDTO) As T()
    Function GetCalendarMaxYear(ByVal Ctx As DataClassesTablet360DataContext) As String
    Sub InsertCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar)
    Sub UpdateCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar, ByVal OldCalendar_Name As String)
    Sub DeleteCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar)
    Function ValidateDuplicateCalendar(ByVal Ctx As DataClassesTablet360DataContext, ByVal Calendar_Name As String, ByVal Calendar_Year As String, ByVal School_Code As String, Optional ByVal OldCalendar_Name As String = "", Optional ByVal OldCalendar_Year As String = "") As Boolean

End Interface

Public Class CalendarRepo
    Implements ICalendarRepo

    Public Function GetCalendarMaxYear(ByVal Ctx As DataClassesTablet360DataContext) As String Implements ICalendarRepo.GetCalendarMaxYear
        Return Ctx.t360_tblCalendars.Select(Function(q) q.Calendar_Year And q.IsActive = True).Max
    End Function

    Public Function GetCalendarByCrit(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As CalendarDTO) As T() Implements ICalendarRepo.GetCalendarByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblCalendar WHERE {FILTER}"
            Dim f As New SqlPart
            With Item
                f.AddPart("School_Code = {0}", .School_Code)
                If .Calendar_Id IsNot Nothing Then
                    f.AddPart("Calendar_Id = {0}", .Calendar_Id.ToString)
                End If
                f.AddPart("Calendar_Name = {0}", .Calendar_Name)
                f.AddPart("Calendar_Type = {0}", .Calendar_Type)
                f.AddPart("Calendar_Year = {0}", .Calendar_Year)
                f.AddPart("Calendar_FromDate = {0}", .Calendar_FromDate)
                f.AddPart("Calendar_ToDate = {0}", .Calendar_ToDate)
                f.AddPart("IsActive = {0}", True)
            End With
            .ApplySqlPart("FILTER", f)
            Return .DataContextExecuteObjects(Of T)(Ctx).ToArray
        End With
    End Function

    Public Function GetCalendarForScheduler(Of T)(ByVal Ctx As DataClassesTablet360DataContext, ByVal Item As CalendarDTO) As T() Implements ICalendarRepo.GetCalendarForScheduler
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblCalendar WHERE {MAIN} AND ((Calendar_FromDate <= {s1} AND Calendar_ToDate >= {s2}) " & _
                       "OR (Calendar_FromDate <= {e1} AND Calendar_ToDate >= {e2}))"
            Dim f As New SqlPart
            With Item
                f.AddPart("School_Code = {0}", .School_Code)
                f.AddPart("Calendar_Year = {0}", .Calendar_Year)
                f.AddPart("IsActive = {0}", True)
            End With
            .ApplySqlPart("MAIN", f)
            .ApplyTagWithValue("s1", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_FromDate))
            .ApplyTagWithValue("s2", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_FromDate))
            .ApplyTagWithValue("e1", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_ToDate))
            .ApplyTagWithValue("e2", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_ToDate))

            Dim Result1 As T() = .DataContextExecuteObjects(Of T)(Ctx).ToArray

            .MainSql = "SELECT * FROM t360_tblCalendar WHERE {MAIN} AND ((Calendar_FromDate > {s1} AND Calendar_ToDate < {e1}) " & _
                       "AND (Calendar_FromDate > {s2} AND Calendar_ToDate < {e2}))"
            Dim f2 As New SqlPart
            With Item
                f2.AddPart("School_Code = {0}", .School_Code)
                f2.AddPart("Calendar_Year = {0}", .Calendar_Year)
                f2.AddPart("IsActive = {0}", True)
            End With
            .ApplySqlPart("MAIN", f2)
            .ApplyTagWithValue("s1", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_FromDate))
            .ApplyTagWithValue("e1", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_ToDate))
            .ApplyTagWithValue("s2", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_FromDate))
            .ApplyTagWithValue("e2", ManageSqlServer.ConvertDateTimeToString(Item.Calendar_ToDate))

            Dim Result2 As T() = .DataContextExecuteObjects(Of T)(Ctx).ToArray

            Return Result1.Union(Result2).ToArray
        End With

    End Function

    Public Function ValidateDuplicateCalendar(ByVal Ctx As DataClassesTablet360DataContext, ByVal Calendar_Name As String, ByVal Calendar_Year As String, ByVal School_Code As String, _
                                              Optional ByVal OldCalendar_Name As String = "", _
                                              Optional ByVal OldCalendar_Year As String = "") As Boolean Implements ICalendarRepo.ValidateDuplicateCalendar
        If OldCalendar_Name = "" Then
            Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = School_Code AndAlso r.Calendar_Name = Calendar_Name AndAlso r.Calendar_Year = Calendar_Year And r.IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Dim q = (From r In Ctx.t360_tblCalendars Where r.School_Code = School_Code AndAlso r.Calendar_Name = Calendar_Name AndAlso r.Calendar_Year = Calendar_Year _
                                                      AndAlso r.Calendar_Name <> OldCalendar_Name AndAlso r.Calendar_Year <> OldCalendar_Year And r.IsActive = True).SingleOrDefault
            If q Is Nothing Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Sub DeleteCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar) Implements ICalendarRepo.DeleteCalendar
        Dim Data = (From r In ctx.t360_tblCalendars Where r.School_Code = Item.School_Code AndAlso r.Calendar_Name = Item.Calendar_Name AndAlso r.Calendar_Year = Item.Calendar_Year AndAlso r.IsActive = True).SingleOrDefault
        Data.LastUpdate = Now
        Data.IsActive = False
        'ctx.t360_tblCalendars.DeleteOnSubmit(Data)
        ctx.SubmitChanges()
    End Sub

    Public Sub InsertCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar) Implements ICalendarRepo.InsertCalendar
        Item.LastUpdate = Now
        Item.IsActive = True
        ctx.t360_tblCalendars.InsertOnSubmit(Item)
        ctx.SubmitChanges()
    End Sub

    Public Sub UpdateCalendar(ByVal ctx As DataClassesTablet360DataContext, ByVal Item As t360_tblCalendar, ByVal OldCalendar_Name As String) Implements ICalendarRepo.UpdateCalendar
        Dim Target As New t360_tblCalendar
        Using ctx1 = GetLinqToSql.GetDataContext()
            Target = (From r In ctx1.t360_tblCalendars Where r.School_Code = Item.School_Code And r.Calendar_Name = OldCalendar_Name).SingleOrDefault
        End Using
        ctx.t360_tblCalendars.Attach(Item, Target)
        ctx.SubmitChanges()
    End Sub

End Class
