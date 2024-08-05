Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils.System
Imports BusinessTablet360.Service

Public Interface IModuleManager

    'Todo เพิ่ม school code ให้ GetTeacherCreateHomeworkByTeacherIdAndCalendarIdAndGroupSubjectId, GetTeacherCreateQuizByTeacherIdAndCalendarIdAndGroupSubjectId
    Function GetModuleDetailByReferenceId(ByVal Reference_Id As Guid) As tblModuleDetail
    Function GetModuleAssignmentJoinByModuleId(ByVal Module_Id As Guid) As Object()
    Function GetModuleAssignmentByMaId(ByVal MA_Id As Guid) As tblModuleAssignment
    Function GetModuleAssignmentJoinByMaId(ByVal MA_Id As Guid) As Object()
    Function GetModuleAssignmentDetailDuplicate(ByVal StartDate As Date, ByVal EndDate As Date, ByVal TeacherId As Guid, ByVal TestSetId As Guid, ByVal MaId As Guid?) As Object()
    Function GetModuleDetailHomeworkByModuleId(ByVal Module_Id As Guid) As Object()
    Function GetStudentDoneHomework(ByVal MA_Id As Guid, ByVal ListClass As String(), ByVal ListRoomId As Guid(), ByVal ListStudentId As Guid()) As Integer
    Function GetTestSetByTestSetId(ByVal TestSet_Id As Guid) As tblTestSet
    Function GetCountTeacherCreateHomework(ByVal School_Code As String, ByVal Calendar_Id As Guid, ByVal GroupSubject_Id As Guid?) As Object()
    Function GetCountTeacherCreateQuiz(ByVal School_Code As String, ByVal Calendar_Id As Guid, ByVal GroupSubject_Id As Guid?) As Object()
    Function GetCountTeacherCreateHomeworkAndQuizBetweenDate(ByVal School_Code As String, ByVal StartDate As Date, ByVal EndDate As Date) As Object()
    Function GetCountSubjectTeacherCreateHomeworkAndQuizBetweenDate(ByVal School_Code As String, ByVal StartDate As Date, ByVal EndDate As Date) As Object()
    Function InsertHomeworkOnly(ByVal ModuleItem As tblModule, ByVal ModuleDetail As tblModuleDetail, ByVal ModuleAssignment As tblModuleAssignment, ByVal ListModuleAssignment As IEnumerable(Of tblModuleAssignmentDetail)) As Guid?
    Function UpdateHomeworkOnly(ByVal MA_Id As Guid, ByVal ModuleAssignment As tblModuleAssignment, ByVal ListModuleAssignmentDetailInsert As System.Collections.Generic.IEnumerable(Of tblModuleAssignmentDetail), ByVal ListModuleAssignmentDetailDelete As System.Collections.Generic.IEnumerable(Of System.Guid)) As Guid?
    Function GetAllQuiz(ByVal School_Code As String) As tblQuiz()

End Interface

Public Class ModuleManager
    Implements IModuleManager

#Region "Dependency"

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

#End Region

    Public Function GetTestSetByTestSetId(ByVal TestSet_Id As System.Guid) As tblTestSet Implements IModuleManager.GetTestSetByTestSetId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblTestSets Where q.TestSet_Id = TestSet_Id).SingleOrDefault
        End Using
    End Function

    Public Function GetModuleAssignmentJoinByModuleId(ByVal Module_Id As System.Guid) As Object() Implements IModuleManager.GetModuleAssignmentJoinByModuleId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From ModuleAssignment In Ctx.tblModuleAssignments Join
                    ModuleAssignmentDetail In Ctx.tblModuleAssignmentDetails On ModuleAssignment.MA_Id Equals ModuleAssignmentDetail.MA_Id
                    Where ModuleAssignment.Module_Id = Module_Id And ModuleAssignment.IsActive = True And ModuleAssignmentDetail.IsActive = True
                    Select New With {.Class_Name = ModuleAssignmentDetail.Class_Name _
                                    , .Room_Id = ModuleAssignmentDetail.Room_Id _
                                    , .Class_Room = (From q In Ctx.t360_tblRooms Where q.Room_Id = ModuleAssignmentDetail.Room_Id Select q.Class_Name & q.Room_Name).SingleOrDefault _
                                    , .Student_Id = ModuleAssignmentDetail.Student_Id _
                                    , .Student_Name = (From q In Ctx.t360_tblStudents Where q.Student_Id = ModuleAssignmentDetail.Student_Id Select q.Student_FirstName & " " & q.Student_LastName).SingleOrDefault _
                                    , .Start_Date = ModuleAssignment.Start_Date _
                                    , .End_Date = ModuleAssignment.End_Date _
                                    , .MA_Id = ModuleAssignment.MA_Id _
                                    , .MAD_Id = ModuleAssignmentDetail.MAD_Id _
                                    , .Module_Id = ModuleAssignment.Module_Id}).ToArray
        End Using
    End Function

    Public Function GetModuleAssignmentByMaId(ByVal MA_Id As Guid) As tblModuleAssignment Implements IModuleManager.GetModuleAssignmentByMaId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblModuleAssignments Where q.MA_Id = MA_Id And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetModuleAssignmentJoinByMaId(ByVal MA_Id As System.Guid) As Object() Implements IModuleManager.GetModuleAssignmentJoinByMaId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From ModuleAssignment In Ctx.tblModuleAssignments Join
                    ModuleAssignmentDetail In Ctx.tblModuleAssignmentDetails On ModuleAssignment.MA_Id Equals ModuleAssignmentDetail.MA_Id
                    Where ModuleAssignment.MA_Id = MA_Id And ModuleAssignment.IsActive = True And ModuleAssignmentDetail.IsActive = True
                    Select New With {.Class_Name = ModuleAssignmentDetail.Class_Name _
                                    , .Room_Id = ModuleAssignmentDetail.Room_Id _
                                    , .Class_Room = (From q In Ctx.t360_tblRooms Where q.Room_Id = ModuleAssignmentDetail.Room_Id Select q.Class_Name & q.Room_Name).SingleOrDefault _
                                    , .Student_Id = ModuleAssignmentDetail.Student_Id _
                                    , .Student_Name = (From q In Ctx.t360_tblStudents Where q.Student_Id = ModuleAssignmentDetail.Student_Id Select q.Student_FirstName & " " & q.Student_LastName).SingleOrDefault _
                                    , .Start_Date = ModuleAssignment.Start_Date _
                                    , .End_Date = ModuleAssignment.End_Date _
                                    , .MA_Id = ModuleAssignment.MA_Id _
                                    , .MAD_Id = ModuleAssignmentDetail.MAD_Id _
                                    , .Module_Id = ModuleAssignment.Module_Id}).ToArray
        End Using
    End Function

    Public Function GetStudentDoneHomework(ByVal MA_Id As Guid, ByVal ListClass() As String, ByVal ListRoomId() As System.Guid, ByVal ListStudentId() As System.Guid) As Integer Implements IModuleManager.GetStudentDoneHomework
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r = (From ModuleAssignment In Ctx.tblModuleAssignments
                    Join ModuleDetailCompletion In Ctx.tblModuleDetailCompletions On ModuleAssignment.MA_Id Equals ModuleDetailCompletion.MA_Id
                    Join Student In Ctx.t360_tblStudents On ModuleDetailCompletion.Student_Id Equals Student.Student_Id
                    Where ModuleAssignment.MA_Id = MA_Id And _
                    ModuleDetailCompletion.Module_Status <> EnModuleStatus.NotDone And _
                    (ListClass.Contains(Student.Student_CurrentClass) Or _
                    ListRoomId.Contains(Student.Student_CurrentRoomId) Or _
                    ListStudentId.Contains(Student.Student_Id))).ToArray
            Return r.Count
        End Using
    End Function

    Public Function GetModuleDetailByReferenceId(ByVal Reference_Id As Guid) As tblModuleDetail Implements IModuleManager.GetModuleDetailByReferenceId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblModuleDetails Where q.Reference_Id = Reference_Id And q.IsActive = True).SingleOrDefault
        End Using
    End Function

    Public Function GetModuleDetailHomeworkByModuleId(ByVal Module_Id As System.Guid) As Object() Implements IModuleManager.GetModuleDetailHomeworkByModuleId
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From ModuleDetail In Ctx.tblModuleDetails Join TestSet In Ctx.tblTestSets On ModuleDetail.Reference_Id Equals TestSet.TestSet_Id
                    Where ModuleDetail.Module_Id = Module_Id And ModuleDetail.Reference_Type = EnModuleType.Homework Select ModuleDetail, TestSet).ToArray
        End Using
    End Function

    Public Function GetModuleAssignmentDetailDuplicate(ByVal StartDate As Date, ByVal EndDate As Date, ByVal TeacherId As System.Guid, ByVal TestSetId As System.Guid, ByVal MaId As Guid?) As Object() Implements IModuleManager.GetModuleAssignmentDetailDuplicate
        Using Ctx = GetLinqToSql.GetDataContext
            Dim R = (From a In Ctx.tblAssistants Join m In Ctx.tblModules On a.Assistant_id Equals m.Create_By _
                    Join md In Ctx.tblModuleDetails On m.Module_Id Equals md.Module_Id _
                    Join ModuleAssignment In Ctx.tblModuleAssignments On m.Module_Id Equals ModuleAssignment.Module_Id _
                    Join ModuleAssignmentDetail In Ctx.tblModuleAssignmentDetails On ModuleAssignment.MA_Id Equals ModuleAssignmentDetail.MA_Id _
                       Where ((StartDate >= ModuleAssignment.Start_Date And StartDate < ModuleAssignment.End_Date) Or _
                        (EndDate >= ModuleAssignment.Start_Date And EndDate < ModuleAssignment.End_Date)) And _
                        a.Assistant_id = TeacherId And md.Reference_Id = TestSetId And _
                        md.Reference_Type = EnModuleType.Homework And _
                        m.IsActive = True And md.IsActive = True And ModuleAssignmentDetail.IsActive = True
                    Select ModuleAssignment, ModuleAssignmentDetail)
            If MaId IsNot Nothing Then
                R = R.Where(Function(q) q.ModuleAssignment.MA_Id <> MaId)
            End If
            Return R.ToArray
        End Using
    End Function

    Public Function InsertHomeworkOnly(ByVal ModuleItem As tblModule, ByVal ModuleDetail As tblModuleDetail, ByVal ModuleAssignment As tblModuleAssignment, ByVal ListModuleAssignment As IEnumerable(Of tblModuleAssignmentDetail)) As Guid? Implements IModuleManager.InsertHomeworkOnly
        With GetLinqToSql
            Try
                Dim Ctx = .GetDataContextWithTransaction()
                ModuleItem.Module_Id = Guid.NewGuid
                ModuleItem.IsActive = True
                ModuleItem.LastUpdate = Now
                Ctx.tblModules.InsertOnSubmit(ModuleItem)

                Dim ModuleDetailId As Guid
                With ModuleDetail
                    .ModuleDetail_Id = Guid.NewGuid
                    .Module_Id = ModuleItem.Module_Id
                    .IsActive = True
                    .LastUpdate = Now
                    ModuleDetailId = .ModuleDetail_Id
                End With
                Ctx.tblModuleDetails.InsertOnSubmit(ModuleDetail)

                With ModuleAssignment
                    .IsActive = True
                    .Module_Id = ModuleItem.Module_Id
                    .MA_Id = Guid.NewGuid
                    .LastUpdate = Now
                End With
                Ctx.tblModuleAssignments.InsertOnSubmit(ModuleAssignment)

                For Each Detail In ListModuleAssignment
                    With Detail
                        .MAD_Id = Guid.NewGuid
                        .MA_Id = ModuleAssignment.MA_Id
                        .IsActive = True
                        .LastUpdate = Now
                    End With
                    Ctx.tblModuleAssignmentDetails.InsertOnSubmit(Detail)
                Next

                Ctx.SubmitChanges()
                .DataContextCommitTransaction()


                Dim Ctx1 = .GetDataContextWithTransaction()
                Dim Students = GetStudentIdByModuleId(UserConfig.GetCurrentContext.School_Code, ModuleAssignment.MA_Id.ToString)
                Dim QuizId = New Service.ClsHomework(New ClassConnectSql).SaveQuizHomework(ModuleDetail.Reference_Id.ToString, Students.Count.ToString)
                For Each R In Students
                    Dim Detail As New tblModuleDetailCompletion
                    With Detail
                        .MDC_Id = Guid.NewGuid
                        .ModuleDetail_Id = ModuleDetailId
                        .MA_Id = ModuleAssignment.MA_Id
                        .Student_Id = R
                        .Quiz_Id = New Guid(QuizId)
                        .Module_Status = EnModuleStatus.NotDone
                        .IsActive = True
                        .LastUpdate = Now
                        .School_Code = UserConfig.GetCurrentContext.School_Code
                    End With
                    Ctx1.tblModuleDetailCompletions.InsertOnSubmit(Detail)
                Next
                Ctx1.SubmitChanges()
                .DataContextCommitTransaction()

                Return ModuleAssignment.MA_Id
            Catch ex As Exception
                .DataContextRollbackTransaction()
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End With
    End Function

    Public Function UpdateHomeworkOnly(ByVal MA_Id As Guid, ByVal ModuleAssignment As tblModuleAssignment, ByVal ListModuleAssignmentDetailInsert As System.Collections.Generic.IEnumerable(Of tblModuleAssignmentDetail), ByVal ListModuleAssignmentDetailDelete As System.Collections.Generic.IEnumerable(Of System.Guid)) As Guid? Implements IModuleManager.UpdateHomeworkOnly
        'เนื่องจาก ModuleAssignmentDetail ในกรณีแก้ไขจะมีแค่เอาออก และ ใส่เพิ่มเท่านั้นไม่มีแก้ไข แต่จะมีการอัพเดด ModuleAssignment ทุกครั้งที่แก้ไข แต่เคสที่แก้ไขเวลาจะทำการเช็คฟิว Start_Date ถ้ามีค่าถึง update ส่วนของเวลา
        With GetLinqToSql
            Try
                Dim Ctx = .GetDataContextWithTransaction()
                Dim OldStudents = GetStudentIdByModuleId(UserConfig.GetCurrentContext.School_Code, MA_Id.ToString)

                For Each Delete In ListModuleAssignmentDetailDelete
                    Dim Item = Ctx.tblModuleAssignmentDetails.Where(Function(q) q.MAD_Id = Delete).SingleOrDefault
                    Item.IsActive = False
                Next

                For Each Detail In ListModuleAssignmentDetailInsert
                    With Detail
                        .MAD_Id = Guid.NewGuid
                        .MA_Id = MA_Id
                        .IsActive = True
                        .LastUpdate = Now
                    End With
                    Ctx.tblModuleAssignmentDetails.InsertOnSubmit(Detail)
                Next

                If ModuleAssignment.Start_Date IsNot Nothing Then
                    Dim OriModuleAssignment = (From q In Ctx.tblModuleAssignments Where q.MA_Id = MA_Id).SingleOrDefault
                    With OriModuleAssignment
                        .Start_Date = ModuleAssignment.Start_Date
                        .End_Date = ModuleAssignment.End_Date
                        .AssignTo = ModuleAssignment.AssignTo
                        .LastUpdate = Now
                    End With
                Else
                    Dim OriModuleAssignment = (From q In Ctx.tblModuleAssignments Where q.MA_Id = MA_Id).SingleOrDefault
                    With OriModuleAssignment
                        .AssignTo = ModuleAssignment.AssignTo
                        .LastUpdate = Now
                    End With
                End If

                Ctx.SubmitChanges()
                .DataContextCommitTransaction()

                Dim Ctx1 = .GetDataContextWithTransaction()
                Dim ModuleId = GetModuleAssignmentByMaId(MA_Id).Module_Id
                Dim ModuleDetailId As Guid = Ctx1.tblModuleDetails.Where(Function(q) q.Module_Id = ModuleId And q.IsActive = True).SingleOrDefault.ModuleDetail_Id
                Dim NewStudents = GetStudentIdByModuleId(UserConfig.GetCurrentContext.School_Code, MA_Id.ToString)
                Dim InsertStudent = NewStudents.Except(OldStudents)
                Dim QuizId = (From q In Ctx1.tblModuleDetailCompletions Where q.ModuleDetail_Id = ModuleDetailId Select q.Quiz_Id).First
                For Each R In InsertStudent
                    Dim Detail As New tblModuleDetailCompletion
                    With Detail
                        .IsActive = True
                        .MA_Id = MA_Id
                        .MDC_Id = Guid.NewGuid
                        .ModuleDetail_Id = ModuleDetailId
                        .Student_Id = R
                        .Quiz_Id = QuizId
                        .Module_Status = EnModuleStatus.NotDone
                        .School_Code = UserConfig.GetCurrentContext.School_Code
                    End With
                    Ctx1.tblModuleDetailCompletions.InsertOnSubmit(Detail)
                Next
                Dim DeleteStudent = OldStudents.Except(NewStudents)
                For Each R In DeleteStudent
                    Dim Detail = (From q In Ctx1.tblModuleDetailCompletions Where q.ModuleDetail_Id = ModuleDetailId And q.Student_Id = R And q.IsActive = True).SingleOrDefault
                    Detail.IsActive = False
                Next

                Ctx1.SubmitChanges()
                .DataContextCommitTransaction()

                Return MA_Id
            Catch ex As Exception
                .DataContextRollbackTransaction()
                ElmahExtension.LogToElmah(ex)
                Return Nothing
            End Try
        End With
    End Function

    Public Function GetStudentIdByModuleId(ByVal SchoolCode As String, ByVal MA_Id As String) As Guid()
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " select Class_Name,Room_Id,Student_Id from tblModuleAssignment INNER JOIN tblModuleAssignmentDetail ON tblModuleAssignment.MA_Id = tblModuleAssignmentDetail.MA_Id " & _
                            " where tblModuleAssignment.MA_Id = '" & MA_Id & "' and tblModuleAssignmentDetail.IsActive = 1 "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        Dim ListClass As New List(Of Guid)
        Dim ListRoom As New List(Of Guid)
        Dim ListStudent As New List(Of Guid)
        Dim OtherVariable As String = ""

        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                'หานักเรียนทั้งหมดในชั้น
                If dt.Rows(index)("Class_Name") IsNot DBNull.Value Then
                    OtherVariable = dt.Rows(index)("Class_Name").ToString()
                    sql = " select Student_Id from t360_tblStudent where Student_CurrentClass = '" & OtherVariable & "' and Student_IsActive = 1 and School_Code = '" & _DB.CleanString(SchoolCode) & "' "
                    Dim dtClass As New DataTable
                    dtClass = _DB.getdata(sql)
                    If dtClass.Rows.Count > 0 Then
                        For rClass = 0 To dtClass.Rows.Count - 1
                            ListClass.Add(dtClass.Rows(rClass)("Student_Id"))
                        Next
                    End If
                    'หานักเรียนทั้งหมดในห้อง
                ElseIf dt.Rows(index)("Room_Id") IsNot DBNull.Value Then
                    OtherVariable = dt.Rows(index)("Room_Id").ToString()
                    sql = " SELECT t360_tblStudent.Student_Id FROM t360_tblRoom INNER JOIN " & _
                          " t360_tblStudent ON t360_tblRoom.Class_Name = t360_tblStudent.Student_CurrentClass AND  " & _
                          " t360_tblRoom.Room_Name = t360_tblStudent.Student_CurrentRoom " & _
                          " WHERE (t360_tblRoom.Room_Id = '" & OtherVariable & "') AND (t360_tblStudent.School_Code = '" & _DB.CleanString(SchoolCode) & "') and (t360_tblStudent.Student_IsActive = 1) "
                    Dim dtRoom As New DataTable
                    dtRoom = _DB.getdata(sql)
                    If dtRoom.Rows.Count > 0 Then
                        For rRoom = 0 To dtRoom.Rows.Count - 1
                            ListRoom.Add(dtRoom.Rows(rRoom)("Student_Id"))
                        Next
                    End If
                    'นักเรียนคนนี้ Add ลงไปเลย
                ElseIf dt.Rows(index)("Student_Id") IsNot DBNull.Value Then
                    ListStudent.Add(dt.Rows(index)("Student_Id"))
                End If
            Next
        End If

        Dim CompleteList = ListClass.Union(ListRoom).Union(ListStudent)
        Return CompleteList.ToArray

    End Function

    Public Function GetCountTeacherCreateHomework(ByVal School_Code As String, ByVal Calendar_Id As Guid, ByVal GroupSubject_Id As Guid?) As Object() Implements IModuleManager.GetCountTeacherCreateHomework
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            If GroupSubject_Id Is Nothing Then
                r = (From q In Ctx.uvw_TeacherCreateHomeworks Where q.School_Code = School_Code And q.Calendar_Id = Calendar_Id
                                                                  Group By TeacherId = q.Teacher_id, _
                                                                          TeacherFirstName = q.Teacher_FirstName, _
                                                                          TeacherLastName = q.Teacher_LastName _
                                                                          Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
            Else
                r = (From q In Ctx.uvw_TeacherCreateHomeworks Where q.GroupSubject_Id = GroupSubject_Id And _
                                                                  q.School_Code = School_Code And q.Calendar_Id = Calendar_Id
                                                             Group By TeacherId = q.Teacher_id, _
                                                                         TeacherFirstName = q.Teacher_FirstName, _
                                                                         TeacherLastName = q.Teacher_LastName _
                                                                         Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
            End If


            Return r
        End Using
    End Function

    Public Function GetCountTeacherCreateQuiz(ByVal School_Code As String, ByVal Calendar_Id As System.Guid, ByVal GroupSubject_Id As System.Guid?) As Object() Implements IModuleManager.GetCountTeacherCreateQuiz
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            Dim r1 As Object()
            If GroupSubject_Id Is Nothing Then
                'r = (From q In Ctx.uvw_TeacherCreateQuizs Where q.t360_SchoolCode = School_Code
                '                                              Group By TeacherId = q.Teacher_id, _
                '                                                          TeacherFirstName = q.Teacher_FirstName, _
                '                                                          TeacherLastName = q.Teacher_LastName, _
                '                                                          QuizId = q.Quiz_Id
                '                                                          Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
                r1 = (From q In Ctx.uvw_TeacherCreateQuizs Where q.t360_SchoolCode = School_Code And q.Calendar_Id = Calendar_Id
                                                              Group By TeacherId = q.Teacher_id, _
                                                                          TeacherFirstName = q.Teacher_FirstName, _
                                                                          TeacherLastName = q.Teacher_LastName, _
                                                                          QuizId = q.Quiz_Id
                                                                          Into Group Select QuizId, TeacherId, TeacherFirstName, TeacherLastName).ToArray
                r = (From q In r1 Group By TeacherId = q.TeacherId, _
                                                                         TeacherFirstName = q.TeacherFirstName, _
                                                                         TeacherLastName = q.TeacherLastName
                                                                         Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
            Else
                r1 = (From q In Ctx.uvw_TeacherCreateQuizs Where q.GroupSubject_Id = GroupSubject_Id And _
                                                                q.t360_SchoolCode = School_Code And q.Calendar_Id = Calendar_Id
                                                              Group By TeacherId = q.Teacher_id, _
                                                                          TeacherFirstName = q.Teacher_FirstName, _
                                                                          TeacherLastName = q.Teacher_LastName, _
                                                                          QuizId = q.Quiz_Id
                                                                          Into Group Select QuizId, TeacherId, TeacherFirstName, TeacherLastName).ToArray
                r = (From q In r1 Group By TeacherId = q.TeacherId, _
                                                                          TeacherFirstName = q.TeacherFirstName, _
                                                                          TeacherLastName = q.TeacherLastName _
                                                                          Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
            End If

            Return r
        End Using
    End Function

    Public Function GetCountTeacherCreateHomeworkAndQuizBetweenDate(ByVal School_Code As String, ByVal StartDate As Date, ByVal EndDate As Date) As Object() Implements IModuleManager.GetCountTeacherCreateHomeworkAndQuizBetweenDate
        StartDate = DateAdd(DateInterval.Hour, +23, StartDate)
        StartDate = DateAdd(DateInterval.Minute, +59, StartDate)
        StartDate = DateAdd(DateInterval.Second, +59, StartDate)
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            r = (From q In Ctx.uvw_TeacherCreateTestsets Where q.t360_SchoolCode = School_Code And _
                                                                q.StartTime <= StartDate And _
                                                                q.StartTime >= EndDate And _
                                                                (q.IsQuizMode = True Or _
                                                                q.IsHomeWorkMode = True)
                                                          Group By TeacherFirstName = q.Teacher_FirstName, _
                                                                   TeacherLastName = q.Teacher_LastName _
                                                           Into Group Select Group.Count, TeacherFirstName, TeacherLastName).ToArray
            Return r
        End Using

    End Function

    Public Function GetCountSubjectTeacherCreateHomeworkAndQuizBetweenDate(ByVal School_Code As String, ByVal StartDate As Date, ByVal EndDate As Date) As Object() Implements IModuleManager.GetCountSubjectTeacherCreateHomeworkAndQuizBetweenDate
        Using Ctx = GetLinqToSql.GetDataContext
            Dim r As Object()
            r = (From q In Ctx.uvw_TeacherCreateSubjects Where q.t360_SchoolCode = School_Code And _
                                                                q.StartTime <= StartDate And _
                                                                q.StartTime >= EndDate And _
                                                                    (q.IsQuizMode = True Or _
                                                                q.IsHomeWorkMode = True)
                                                          Group By GroupSubjectShortName = q.GroupSubject_ShortName
                                                           Into Group Select Group.Count, GroupSubjectShortName).ToArray
            Return r
        End Using
    End Function

    Public Function GetAllQuiz(ByVal School_Code As String) As tblQuiz() Implements IModuleManager.GetAllQuiz
        Using Ctx = GetLinqToSql.GetDataContext
            Return (From q In Ctx.tblQuizs Where q.IsActive = True).ToArray
        End Using
    End Function

End Class
