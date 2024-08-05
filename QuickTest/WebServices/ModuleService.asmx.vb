Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports KnowledgeUtils.Json.Serialization
Imports KnowledgeUtils.System
Imports BusinessTablet360
Imports Microsoft.Practices.Unity

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ModuleService
    Inherits System.Web.Services.WebService
    Dim Mn As New SchoolManager


#Region "Dependency"

    Public Sub New()
        CType(Context.ApplicationInstance, Global_asax).Container.BuildUp(Me.GetType, Me)
    End Sub

    Private _SchoolManager As ISchoolManager
    <Dependency()> Public Property SchoolManager() As ISchoolManager
        Get
            Return _SchoolManager
        End Get
        Set(ByVal value As ISchoolManager)
            _SchoolManager = value
        End Set
    End Property

    Private _StudentManager As IStudentManager
    <Dependency()> Public Property StudentManager() As IStudentManager
        Get
            Return _StudentManager
        End Get
        Set(ByVal value As IStudentManager)
            _StudentManager = value
        End Set
    End Property

    Private _ModuleManager As IModuleManager
    <Dependency()> Public Property ModuleManager() As IModuleManager
        Get
            Return _ModuleManager
        End Get
        Set(ByVal value As IModuleManager)
            _ModuleManager = value
        End Set
    End Property

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

    Private _TeacherManager As ITeacherManager
    <Dependency()> Public Property TeacherManager() As ITeacherManager
        Get
            Return _TeacherManager
        End Get
        Set(ByVal value As ITeacherManager)
            _TeacherManager = value
        End Set
    End Property

#End Region

    <WebMethod(EnableSession:=True)> _
    Public Function GetClassInSchool(ByVal SchoolCode As String) As Object()
        Dim Result = SchoolManager.GetClassInSchool(SchoolCode)
        Dim ResultList As New List(Of Object)
        For Each r In Result
            ResultList.Add(New With {.Class_Name = r.Class_Name})
        Next
        Return ResultList.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetTeacherClass(ByVal SchoolCode As String, ByVal Teacher_id As String) As Object()
        Dim Result = TeacherManager.GetTeacherClass(SchoolCode, Teacher_id)
        Dim ResultList As New List(Of Object)
        For Each r In Result
            ResultList.Add(New With {.Class_Name = r.Class_Name})
        Next
        Return ResultList.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetTeacherClassCheckWithFile(ByVal SchoolCode As String, ByVal Teacher_id As String, ByVal AllowClass As String) As Object()
        Dim Result = SchoolManager.GetClassOfStudent(SchoolCode)
        Dim ResultList As New List(Of Object)

        AllowClass = AllowClass & ","

        For Each r In Result
            If InStr(AllowClass, r.Class_Order & ",") <> 0 Then
                ResultList.Add(New With {.Class_Name = r.Class_Name})
            End If
        Next
        Return ResultList.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetClassInStudent(ByVal SchoolCode As String) As Object()
        Return SchoolManager.GetClassInStudent(SchoolCode)
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetRoomByClassName(ByVal SchoolCode As String, ByVal ClassName As String) As Object()
        Dim Item As New t360_tblRoom
        Item.School_Code = SchoolCode
        Item.Class_Name = ClassName
        Item.Room_IsActive = True
        Dim Result = SchoolManager.GetStudentRoomByClassName(Item)

        Dim ResultList As New List(Of Object)
        For Each r In Result
            ResultList.Add(New With {.Room_Id = r.Room_Id, .Room_Name = r.Class_Name & r.Room_Name, .FormatRoom = ServiceSystem.ConvertForOrderRoom(r.Room_Name)})
        Next

        Return ResultList.OrderBy(Function(q) q.FormatRoom).ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetModuleDetailByReferenceId(ByVal Reference_Id As System.Guid) As Object
        Dim Result = ModuleManager.GetModuleDetailByReferenceId(Reference_Id)
        If Result IsNot Nothing Then
            Return New With {.Reference_Id = Result.Reference_Id, .Reference_Type = Result.Reference_Type, .Module_Id = Result.Module_Id}
        Else
            Return Nothing
        End If
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetModuleAssignmentByMaId(ByVal MA_Id As System.Guid) As tblModuleAssignment
        Return ModuleManager.GetModuleAssignmentByMaId(MA_Id)
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetModuleDetailHomeworkByModuleId(ByVal Module_Id As System.Guid) As Object()
        Dim Result = ModuleManager.GetModuleDetailHomeworkByModuleId(Module_Id)
        Dim ResultList As New List(Of Object)
        For Each r In Result
            ResultList.Add(New With {.Reference_Id = r.ModuleDetail.Reference_Id _
                                    , .Reference_Type = r.ModuleDetail.Reference_Type _
                                    , .TestSet_Name = r.TestSet.TestSet_Name _
                                    , .Md_Type = EnModuleUI.MdModule.GetEnumText(Of EnumModuleUI)() _
                                    , .ModuleDetail_Id = r.ModuleDetail.ModuleDetail_Id})
        Next
        Return ResultList.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetModuleAssignmentJoinByModuleId(ByVal Module_Id As System.Guid) As Object()
        Dim Result = ModuleManager.GetModuleAssignmentJoinByModuleId(Module_Id)
        Dim ResultList As New List(Of Object)
        Return Result.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetModuleAssignmentJoinByMaId(ByVal MA_Id As System.Guid) As Object()
        Return ModuleManager.GetModuleAssignmentJoinByMaId(MA_Id)
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetTestSetByTestSetId(ByVal TestSet_Id As System.Guid) As tblTestSet
        Return ModuleManager.GetTestSetByTestSetId(TestSet_Id)
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetStudentByRoom(ByVal SchoolCode As String, ByVal RoomName As String) As Object()
        Dim Result = StudentManager.GetStudentByRoom(RoomName, SchoolCode)
        Dim ResultList As New List(Of Object)
        For Each r In Result
            ResultList.Add(New With {.Student_Id = r.Student_Id, .Student_Name = r.Student_FirstName & " " & r.Student_LastName})
        Next
        Return ResultList.ToArray
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function CheckDuplicateModuleHomeworkOnly(ByVal MaId As String, ByVal Assignment As Object) As Object
        Dim TestSetId As Guid
        Dim StartDate As Date
        Dim EndDate As Date
        Dim MaIdGuid As Guid?
        Dim ListCheck As New Dictionary(Of Object, Object)
        Dim ListDup As New Dictionary(Of Object, Object)

        If MaId <> "" Then
            MaIdGuid = New Guid(MaId)
        End If
        For Each Row In Assignment
            Select Case Row("type")
                Case EnModuleUI.MdModule.GetEnumText(Of EnumModuleUI)()
                    TestSetId = New Guid(Row("id").ToString)
                Case EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)()
                    If Row("text") = "start" Then
                        StartDate = CType(Row("id"), System.DateTime)
                    ElseIf Row("text") = "end" Then
                        EndDate = CType(Row("id"), System.DateTime)
                    End If
                Case EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()
                    ListCheck.Add(Row("id").ToString, New With {.Type = EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)(), .Text = Row("text").ToString})
                Case EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()
                    ListCheck.Add(New Guid(Row("id").ToString), New With {.Type = EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)(), .Text = Row("text").ToString})
                Case EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()
                    ListCheck.Add(New Guid(Row("id").ToString), New With {.Type = EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)(), .Text = Row("text").ToString})
            End Select
        Next

        Dim ModuleAssignmentDetails = ModuleManager.GetModuleAssignmentDetailDuplicate(StartDate, EndDate, UserConfig.GetCurrentContext.User_Id, TestSetId, MaIdGuid)
        If ModuleAssignmentDetails.Count = 0 Then
            'เวลาไม่คล่อมก้บการสั่งการบ้านเลย
            Return New With {.IsDup = False}
        Else
            For Each Ls In ListCheck
                For Each Mad In ModuleAssignmentDetails
                    Select Case Ls.Value.Type
                        Case EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()
                            If Ls.Key = Mad.ModuleAssignmentDetail.Class_Name Then
                                ListDup.Add(Ls.Key, Ls.Value)
                                Exit For
                            End If
                        Case EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()
                            If Ls.Key = Mad.ModuleAssignmentDetail.Room_Id Then
                                ListDup.Add(Ls.Key, Ls.Value)
                                Exit For
                            End If
                        Case EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()
                            If Ls.Key = Mad.ModuleAssignmentDetail.Student_Id Then
                                ListDup.Add(Ls.Key, Ls.Value)
                                Exit For
                            End If
                    End Select
                Next
            Next

            If ListDup.Count = 0 Then
                'เวลาคล่อมแต่ไม่มีการตั้งค่าที่เหมือนเลย
                Return New With {.IsDup = False}
            ElseIf ModuleAssignmentDetails.Count = ListDup.Count Then
                'เวลาคล่อมและมีการตั้งค่าเหมือนหมด
                Dim Msg = "เคยสั่งการบ้านนี้ให้เด็กกลุ่มนี้" & ServiceSystem.HomeworkTimeToText(StartDate, EndDate) & " แล้ว จะสั่งซ้ำอีกครั้ง แน่ใจนะค่ะ"
                Msg = "<div style='padding:5px;'>" & Msg & "</div>"
                Return New With {.IsDup = True, .Msg = Msg}
            Else
                'เวลาคล่อมและมีการตั้งค่าเหมือนบางส่วน
                Dim CountMaId = (From q In ModuleAssignmentDetails Group By q.ModuleAssignment.MA_Id Into Group).Count
                If CountMaId > 1 Then
                    'กรณีเจอการบ้านที่คล่อมมากกว่าหนึ่งตัว เอา min ของ startdate, max ของ enddate
                    StartDate = (From q In ModuleAssignmentDetails).Min(Of Date)(Function(q1) q1.ModuleAssignment.Start_Date)
                    EndDate = (From q In ModuleAssignmentDetails).Max(Of Date)(Function(q1) q1.ModuleAssignment.End_Date)
                End If

                Dim Msg = "เคยสั่งการบ้านนี้ให้"
                Msg &= "<ul style='margin:0;'>"
                For Each Row In ListDup
                    Msg &= "<li>" & Row.Value.Text & "</li>"
                Next
                Msg &= "</ul>"
                Msg &= "ทำช่วง " & ServiceSystem.HomeworkTimeToText(StartDate, EndDate) & " สั่งซ้ำอีกครั้ง แน่ใจนะค่ะ"
                Msg = "<div style='padding:5px;'>" & Msg & "</div>"
                Return New With {.IsDup = True, .Msg = Msg}
            End If
        End If
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function CheckeUpdateModuleHomeworkOnly(ByVal MaId As String, ByVal Assignment As Object) As Object
        Dim MsgOut As Object()
        Dim MsgIn As Object()
        Dim CountStudentDone As Integer
        If MaId <> "" Then
            Dim StartDate As Date
            Dim EndDate As Date
            Dim NewAssignments As New List(Of ModuleUiDTO)
            For Each Row In Assignment
                Select Case Row("type")
                    Case EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)()
                        If Row("text") = "start" Then
                            StartDate = CType(Row("id"), System.DateTime)
                        ElseIf Row("text") = "end" Then
                            EndDate = CType(Row("id"), System.DateTime)
                        End If
                    Case EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)(), EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)(), EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()
                        Dim NewAssignment = (New ManageJsonSerialize).SerializeToJson(Row).Deserialize(Of ModuleUiDTO)()
                        NewAssignments.Add(NewAssignment)
                End Select
            Next
            Dim OldAssignment = ModuleManager.GetModuleAssignmentJoinByMaId(New Guid(MaId))
            Dim OutAssignment As New List(Of Object)

            For Each OldAs In OldAssignment.Where(Function(q) q.Class_Name IsNot Nothing).ToArray
                Dim Found = (From q In NewAssignments).Where(Function(q) q.type = EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()).Where(Function(q) q.id = OldAs.Class_Name).SingleOrDefault
                If Found Is Nothing Then
                    OutAssignment.Add(OldAs)
                Else
                    NewAssignments.Remove(Found)
                End If
            Next
            For Each OldAs In OldAssignment.Where(Function(q) q.Room_Id IsNot Nothing).ToArray
                Dim Found = (From q In NewAssignments).Where(Function(q) q.type = EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()).Where(Function(q) New Guid(q.id.ToString) = OldAs.Room_Id).SingleOrDefault
                If Found Is Nothing Then
                    OutAssignment.Add(OldAs)
                Else
                    NewAssignments.Remove(Found)
                End If
            Next
            For Each OldAs In OldAssignment.Where(Function(q) q.Student_Id IsNot Nothing).ToArray
                Dim Found = (From q In NewAssignments).Where(Function(q) q.type = EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()).Where(Function(q) New Guid(q.id.ToString) = OldAs.Student_Id).SingleOrDefault
                If Found Is Nothing Then
                    OutAssignment.Add(OldAs)
                Else
                    NewAssignments.Remove(Found)
                End If
            Next

            Dim OutClass = OutAssignment.Where(Function(q) q.Class_Name IsNot Nothing).Select(Function(q) q.Class_Name.ToString).ToArray
            Dim OutRoom = OutAssignment.Where(Function(q) q.Room_Id IsNot Nothing).Select(Function(q) New Guid(q.Room_Id.ToString)).ToArray
            Dim OutStudent = OutAssignment.Where(Function(q) q.Student_Id IsNot Nothing).Select(Function(q) New Guid(q.Student_Id.ToString)).ToArray
            CountStudentDone = ModuleManager.GetStudentDoneHomework(New Guid(MaId), OutClass, OutRoom, OutStudent)
            MsgOut = OutAssignment.Where(Function(q) q.Class_Name IsNot Nothing).Select(Function(q) q.Class_Name.ToString) _
                         .Union(OutAssignment.Where(Function(q) q.Room_Id IsNot Nothing).Select(Function(q) q.Class_Room.ToString)) _
                         .Union(OutAssignment.Where(Function(q) q.Student_Id IsNot Nothing).Select(Function(q) q.Student_Name.ToString)).ToArray
            MsgIn = NewAssignments.Where(Function(q) q.type = EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()).OrderBy(Function(q) q.text).Select(Function(q) q.id) _
                        .Union(NewAssignments.Where(Function(q) q.type = EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()).OrderBy(Function(q) q.text).Select(Function(q) q.text)) _
                        .Union(NewAssignments.Where(Function(q) q.type = EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()).OrderBy(Function(q) q.text).Select(Function(q) q.text)).ToArray

        End If

        If MsgOut Is Nothing Then Return New With {.MsgOut = MsgOut, .MsgIn = MsgIn, .CountStudentDone = CountStudentDone}

        Return New With {.MsgOut = MsgOut.ToArray, .MsgIn = MsgIn.ToArray, .CountStudentDone = CountStudentDone}
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function SaveModuleHomeworkOnly(ByVal MaId As String,
                                           ByVal SchoolCode As String,
                                           ByVal InsertModule As Object,
                                           ByVal InsertAssignment As Object,
                                           ByVal DeleteModule As Object,
                                           ByVal DeleteAssignment As Object,
                                           ByVal EditAssignment As Object,
                                           ByVal AssignTo As String) As String

        Dim TestSetId As Guid
        Dim TestSetName As String = ""
        Dim MaIdSuccess As Guid?

        Dim IsNew = (InsertModule.length > 0)
        If IsNew Then
            'เพิ่ม
            Dim Item As New tblModule
            With Item
                .School_Code = SchoolCode
                .Create_By = UserConfig.GetCurrentContext.User_Id
                .Title = InsertModule(0)("text")
            End With

            Dim ModuleDetail As New tblModuleDetail
            With ModuleDetail
                .Reference_Id = New Guid(InsertModule(0)("id").ToString)
                .Reference_Type = EnModuleType.Homework
            End With

            Dim ModuleAssignmentDetails As New List(Of tblModuleAssignmentDetail)

            Dim ModuleAssignment As New tblModuleAssignment With {.Calendar_Id = (New KNAppSession)("CurrentCalendarId"),
                                                                  .AssignTo = AssignTo} 'todo ของจริง
            'Dim ModuleAssignment As New tblModuleAssignment With {.Calendar_Id = New Guid("5CD20B5D-9B73-4412-8DF1-AA6602555F87")} 'todo ใช้ทดสอบ
            For Each Row In InsertAssignment
                Dim ModuleAssignmentDetail As New tblModuleAssignmentDetail
                Select Case Row("type")
                    Case EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Class_Name = Row("id")
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                    Case EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Room_Id = New Guid(Row("id").ToString)
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                    Case EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Student_Id = New Guid(Row("id").ToString)
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                    Case EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)()
                        If Row("text") = "start" Then
                            With ModuleAssignment
                                .Start_Date = CType(Row("id"), System.DateTime)
                            End With
                        ElseIf Row("text") = "end" Then
                            With ModuleAssignment
                                .End_Date = CType(Row("id"), System.DateTime)
                            End With
                        End If
                End Select
            Next

            MaIdSuccess = ModuleManager.InsertHomeworkOnly(Item, ModuleDetail, ModuleAssignment, ModuleAssignmentDetails)
        Else
            'แก้ไข
            Dim ModuleAssignmentDetails As New List(Of tblModuleAssignmentDetail)
            For Each Row In InsertAssignment
                Dim ModuleAssignmentDetail As New tblModuleAssignmentDetail
                Select Case Row("type")
                    Case EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Class_Name = Row("id")
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                    Case EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Room_Id = New Guid(Row("id").ToString)
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                    Case EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)()
                        With ModuleAssignmentDetail
                            .Student_Id = New Guid(Row("id").ToString)
                        End With
                        ModuleAssignmentDetails.Add(ModuleAssignmentDetail)
                End Select
            Next

            Dim ModuleAssignment As New tblModuleAssignment With {.AssignTo = AssignTo}
            With ModuleAssignment
                For Each Row In EditAssignment
                    Select Case Row("text")
                        Case "start"
                            With ModuleAssignment
                                .Start_Date = CType(Row("id"), System.DateTime)
                            End With
                        Case "end"
                            With ModuleAssignment
                                .End_Date = CType(Row("id"), System.DateTime)
                            End With
                    End Select
                Next
            End With

            Dim ModuleAssignmentDetailDeletes As New List(Of Guid)
            For Each Row In DeleteAssignment
                ModuleAssignmentDetailDeletes.Add(New Guid(Row.ToString))
            Next

            MaIdSuccess = ModuleManager.UpdateHomeworkOnly(New Guid(MaId), ModuleAssignment, ModuleAssignmentDetails, ModuleAssignmentDetailDeletes)
        End If

        If MaIdSuccess IsNot Nothing Then
            Dim ModuleId = ModuleManager.GetModuleAssignmentByMaId(MaIdSuccess).Module_Id
            Dim ModuleList As New List(Of Object)
            For Each Row In GetModuleDetailHomeworkByModuleId(ModuleId)
                ModuleList.Add(New With {.id = Row.Reference_Id, .text = Row.TestSet_Name, .type = Row.Md_Type, .maId = Nothing, .mdId = Row.ModuleDetail_Id})
            Next
            Dim AssignList = New List(Of Object)
            Dim AssignListHomeworkTime = New List(Of Object)
            Dim ModuleAssignments = GetModuleAssignmentJoinByMaId(MaIdSuccess)
            For Each Row In ModuleAssignments
                If Row.Class_Name IsNot Nothing Then
                    AssignList.Add(New With {.id = Row.Class_Name, .text = Row.Class_Name, .type = EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)(), .maId = Row.MAD_Id, .mdId = Nothing})
                End If
                If Row.Room_Id IsNot Nothing Then
                    AssignList.Add(New With {.id = Row.Room_Id, .text = Row.Class_Room, .type = EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)(), .maId = Row.MAD_Id, .mdId = Nothing})
                End If
                If Row.Student_Id IsNot Nothing Then
                    AssignList.Add(New With {.id = Row.Student_Id, .text = Row.Student_Name, .type = EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)(), .maId = Row.MAD_Id, .mdId = Nothing})
                End If
            Next
            Dim ModuleAssignment = GetModuleAssignmentByMaId(MaIdSuccess)
            AssignListHomeworkTime.Add(New With {.id = ModuleAssignment.Start_Date, .text = "start", .type = EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)(), .maId = ModuleAssignment.MA_Id, .mdId = Nothing})
            AssignListHomeworkTime.Add(New With {.id = ModuleAssignment.End_Date, .text = "end", .type = EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)(), .maId = ModuleAssignment.MA_Id, .mdId = Nothing})


            Return (New With {.ModuleList = ModuleList, .AssignList = AssignList, .AssignListHomeworkTime = AssignListHomeworkTime, .MdId = MaIdSuccess}).SerializeToJson
        Else
            Return ""
        End If

    End Function

End Class