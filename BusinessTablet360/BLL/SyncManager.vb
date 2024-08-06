Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils.System.ManageHardware
Imports System.IO
Imports System.IO.Compression
Imports KnowledgeUtils
Imports KnowledgeUtils.Database.Command
Imports KnowledgeUtils.System.DateTimeUtil
Imports KnowledgeUtils.IO
Imports System.Windows.Forms
Imports KnowledgeUtils.Database.SqlServer
Imports System.Data.SqlClient

Public Class SyncManager

#Region "Property"

    'เทเบิล sync ที่ไม่สนใจ wpp
    Public TableSync As String() = {"t360_tblCalendar", "t360_tblLog", "t360_tblNetwork", "t360_tblNetworkHistory",
                                    "t360_tblNews", "t360_tblNewsRoom", "t360_tblRoom", "t360_tblSchool", "t360_tblSchoolClass",
                                    "t360_tblSetTypeRunStudentNumber", "t360_tblStudent", "t360_tblStudentFinish",
                                    "t360_tblStudentRoom", "t360_tblTablet", "t360_tblTabletLog", "t360_tblTabletLost", "t360_tblTabletOwner",
                                    "t360_tblTabletRepair", "t360_tblTeacher", "t360_tblTeacherRoom", "t360_tblUpLevel", "t360_tblUplevelConfirm",
                                    "t360_tblUpLevelDetail", "t360_tblUser", "t360_tblUserMenuItem", "tblAssistant", "tblAvatarComment",
                                    "tblChatRoom", "tblChatJoin", "tblChatMessage", "tblChatRecipient",
                                    "tblContact", "tblLog", "tblMobileAccessPassword", "tblMobileRegistration", "tblModule",
                                    "tblModuleAssignment", "tblModuleAssignmentDetail", "tblModuleDetail", "tblModuleDetailCompletion",
                                    "tblNote", "tblParent", "tblQuestionSetPassword", "tblQuiz", "tblQuizAnswer",
                                    "tblQuizQuestion", "tblQuizScore", "tblQuizSession", "tblRequestParentApproval",
                                    "tblRequestParentApprovalDetail", "tblSetEmail", "tblStudentItem",
                                    "tblStudentParent", "tblStudentPhoto", "tblStudentPoint", "tblSyncLog", "tblTabletLab", "tblTabletLabDesk",
                                    "tblTeacherFavorite", "tblTeacherNews", "tblTeacherNewsDetail", "tblTeacherNewsDetailCompletion",
                                    "tblTestSet", "tblTestSet_CM_temptblsetup", "tblTestSetQuestionDetail", "tblTestSetQuestionSet", "tblTestSetWinner",
                                    "tblUser", "tblUserSetting", "tblUserSubjectClass", "tblTestSync", "tblTestSyncDetail"}

    'เทเบิล sync ที่สนใจ wpp และต้องเท่ากับ 0 ถ้า wpp=1 หมายความว่ามาจากโรงงาน wpp=0 เกิดจากลูกค้าทำเอง
    Public TableSyncNotWpp As String() = {"tblAnswer", "tblBook", "tblQuestionset", "tblQuestionCategory", "tblQuestion",
                                          "tblEvaluationIndexLevel", "tblEvaluationIndexNew",
                                          "tblEvaluationIndexSubject", "tblIntro", "tblIntroQuestionSet", "tblIntroQuestionSetQuestion",
                                          "tblWordBook"}

    'เทเบิลที่ sync ขึ้น และ ลง
    Public TableSyncTwoWay As String() = {"tblQuizScore", "tblQuizSession", "tblModuleDetailCompletion", "tblLog", "tblNote", "tblStudentItem",
                                          "tblTestSetWinner", "tblStudentPoint", "tblQuiz", "tblQuizAnswer",
                                          "tblQuizQuestion", "tblChatRoom", "tblChatJoin", "tblChatMessage",
                                          "tblChatRecipient", "tblAvatarComment", "tblRequestParentApproval", "tblRequestParentApprovalDetail", "tblTestSync", "tblTestSyncDetail"}

    'เทเบิลที่ sync ขึ้น และ ลง และ ตอน import ดูแค่ id
    Public TableSyncTwoWayIdPattern As String() = {"tblLog", "tblQuiz", "tblChatMessage", "tblChatRecipient", "tblRequestParentApproval", "tblRequestParentApprovalDetail"}

    'กลุ่มที่ต้องทำเป็นเทเบิลชุด
    Public TableSyncComboPattern As String() = {"tblChatRoom"}


    Public MapDataKey As New Dictionary(Of String, String) From {
                                                                 {"t360_tblCalendar", "Calendar_Name,Calendar_FromDate,Calendar_ToDate,School_Code,IsActive"},
                                                                 {"t360_tblNetwork", "School_Code,Network_IP,Network_IsActive"},
                                                                 {"t360_tblSchool", "School_Code,School_IsActive"},
                                                                 {"t360_tblSchoolClass", "School_Code,Class_Name,IsActive"},
                                                                 {"t360_tblStudent", "School_Code,Student_Code,Student_IsActive"},
                                                                 {"t360_tblStudentRoom", "School_Code,Student_Id,Calendar_Id,Room_Id,SR_IsActive"},
                                                                 {"tblQuizScore", "Quiz_Id,Student_Id,Question_Id,IsActive"},
                                                                 {"tblQuizSession", "Quiz_Id,IsActive,Player_Id"},
                                                                 {"tblModuleDetailCompletion", "ModuleDetail_Id,MA_Id,Student_Id,IsActive"},
                                                                 {"tblNote", "User_Id,IsActive"},
                                                                 {"tblTestSetWinner", "QSet_ID,TestSet_ID,IsStandard,IsActive"},
                                                                 {"tblStudentPoint", "Student_Id,IsActive"},
                                                                 {"tblQuizAnswer", "Quiz_Id,Question_Id,Answer_Id,IsActive"},
                                                                 {"tblQuizQuestion", "Quiz_Id,Question_Id,IsActive"},
                                                                 {"tblChatJoin", "ChatRoom_Id"},
                                                                 {"tblAvatarComment", "AC_From,AC_TO,AC_Message,IsActive"},
                                                                 {"tblStudentItem", "Student_Id,ShopItem_Id,SI_Position,IsActive"},
                                                                 {"tblTestSync", "testSyncName,testSyncMaster"},
                                                                 {"tblTestSyncDetail", "tsd_isPass,tsd_DetailName2"}
                                                                }

    'จะมีบางเทเบิลที่่ ฟิว schoolcode ต่างชื่อกันเลยจำเป็นที่จะต้องมีตัวแปรนี้มาเก็บ หรือ ถ้าเทเบิลไหนขึ้นอย่างเดียวแน่นอนไม่มีลงก็ไม่จำเป็นที่จะต้องนำมาใส่ตรงนี้เพราะถ้าไม่ใส่ก็จะไม่ where schoolcode ตอนจะเก็บดาต้าเผื่อส่งให้อีกฝั่งนึงเท่านั้นเองตัวอย่างเช่น tblAssistant แต่เทเบิล twoway จะต้องลงไปหาโรงเรียนของเค้าเองเพราะฉนั้นจำเป็นแน่นอนที่จะต้องอยู่ในนี้ขาดบ่อได้เลย
    Public MapSchoolCodeName As New Dictionary(Of String, String) From {
                                                                        {"t360_tblCalendar", "School_Code"},
                                                                        {"t360_tblNetwork", "School_Code"},
                                                                        {"t360_tblSchool", "School_Code"},
                                                                        {"t360_tblSchoolClass", "School_Code"},
                                                                        {"t360_tblStudent", "School_Code"},
                                                                        {"t360_tblStudentRoom", "School_Code"},
                                                                        {"t360_tblLog", "School_Code"},
                                                                        {"t360_tblNetworkHistory", "School_Code"},
                                                                        {"t360_tblNews", "School_Code"},
                                                                        {"t360_tblNewsRoom", "School_Code"},
                                                                        {"t360_tblRoom", "School_Code"},
                                                                        {"t360_tblSetTypeRunStudentNumber", "School_Code"},
                                                                        {"t360_tblTablet", "School_Code"},
                                                                        {"t360_tblTabletLog", "School_Code"},
                                                                        {"t360_tblTabletLost", "School_Code"},
                                                                        {"t360_tblTabletOwner", "School_Code"},
                                                                        {"t360_tblTabletRepair", "School_Code"},
                                                                        {"t360_tblTeacher", "School_Code"},
                                                                        {"t360_tblTeacherRoom", "School_Code"},
                                                                        {"t360_tblUpLevel", "School_Code"},
                                                                        {"t360_tblUplevelConfirm", "School_Code"},
                                                                        {"t360_tblUpLevelDetail", "School_Code"},
                                                                        {"t360_tblUser", "School_Code"},
                                                                        {"t360_tblUserMenuItem", "School_Code"},
                                                                        {"tblQuizScore", "School_Code"},
                                                                        {"tblQuizSession", "School_Code"},
                                                                        {"tblModuleDetailCompletion", "School_Code"},
                                                                        {"tblLog", "School_Code"},
                                                                        {"tblNote", "School_Code"},
                                                                        {"tblTestSetWinner", "SchoolId"},
                                                                        {"tblStudentPoint", "School_Code"},
                                                                        {"tblQuiz", "t360_SchoolCode"},
                                                                        {"tblQuizAnswer", "School_Code"},
                                                                        {"tblQuizQuestion", "School_Code"},
                                                                        {"tblAvatarComment", "School_Code"},
                                                                        {"tblStudentItem", "School_Code"},
                                                                        {"tblTestSync", "School_Code"},
                                                                        {"tblTestSyncDetail", "School_Code"}
                                                                       }

    'Public MapIsActiveName As New Dictionary(Of String, String) From {
    '                                                                  {"t360_tblCalendar", "IsActive"},
    '                                                                  {"t360_tblNetwork", "Network_IsActive"},
    '                                                                  {"t360_tblSchool", "School_IsActive"},
    '                                                                  {"t360_tblSchoolClass", "IsActive"},
    '                                                                  {"t360_tblStudent", "Student_IsActive"},
    '                                                                  {"t360_tblStudentRoom", "SR_IsActive"}
    '                                                                 }

    Public CreateBy As EnTypeSyncManager

    Public TableReName = {"coursemodules", "courses", "courses_module", "courses_G1", "excel_stu", "Sheet1$", "Sheet11$",
                          "t360_tblDistrict", "t360_tblLesson", "t360_tblProvice", "t360_tblStudent_bk_20130301", "t360_tblStudentHomeWork",
                          "t360_tblStudentRoom_bk_20130301", "t360_tblStudentTest", "t360_tblStudentTmp", "t360_tblStudentTmp2",
                          "t360_tblSubDistrict", "t360_tblSubjectClass", "t360_tblSubjectType", "t360_tblTablet_bk_20130301",
                          "t360_tblTabletOwner_bk_20130301", "t360_tblTabletOwnerTmp", "t360_tblTabletOwnerTmp2", "t360_tblTempStudent",
                          "t360_tblTempTeacher", "t360_tblTestType", "t360_tblTmpTabletDetail", "t360_tblUnit", "tblAnswer_Backup",
                          "tblAnswer_zz", "tblAppFiles_Old", "tblAppInstallLog_Old", "tblAppliance_Old", "tblApplianceAppInstalled_Old",
                          "tblApplication_Old", "tblApplicationReview_Old", "tblBackupQuestion", "tblCheckAnswer", "tblClass",
                          "tblcoursemodules", "tblEvaluationIndex", "tblEvaluationIndexNew2", "tblEvaluationIndexNew3",
                          "tblEvaluationIndexWithSubject", "tblFinaltest", "tblPicToTestMissingFile", "tblQuestion_Backup",
                          "tblQuestion_zz", "tblQuestionCategory_20130205", "tblQuestionCategory_bk", "tblQuestionCheckFileMulti",
                          "tblQuestionEvaluationIndexItem_BackUp", "tblQuestionOption", "tblQuestionStory", "tblQuestionType",
                          "tblQuizAnswerMaii", "tblQuizAnswerTmp", "tblQuizAnswerTmp2", "tblQuizMaii", "tblQuizQuestionMaii",
                          "tblQuizQuestionTmp", "tblQuizQuestionTmp2", "tblQuizScoreMaii", "tblQuizScoreTmp2", "tblQuizScoreTmpToTest",
                          "tblQuizSessionMaii", "tblQuizSessionTmp", "tblQuizSessionTmp2", "tblQuizTmp", "tblQuizTmp2",
                          "tblRoom", "tblTestSuite", "tblTmpAnswer_ReplaceChoice", "tbltmpQuizID", "tblUserAdmin",
                          "tlQuizScoreTmp", "tbltest", "tblUserContact", "tblEfficiencySet", "tblEfficiency",
                          "tblEvaluationIndexNew3", "tblMultimediaObject", "tblMultimediaQuestion",
                          "tblMultimediaQuestionSet", "tblStudentAvatar", "tblCourses"}

    ''' <summary>
    ''' property ตัวนี้ไว้ใช้ในอนาคต การ sync รอบนี้มี schoolcode ตัวไหนทำงานผ่านหรือไม่ผ่านบ้าง(ถ้าไม่ผ่านจะ roolback ทั้งโรงเรียน)
    ''' </summary>
    ''' <remarks></remarks>
    Private _IsPass As New Dictionary(Of String, Boolean)
    Public Property IsPass() As Dictionary(Of String, Boolean)
        Get
            Return _IsPass
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _IsPass = value
        End Set
    End Property

    Private _ServiceSync As Object
    Public Property ServiceSync() As Object
        Get
            Return _ServiceSync
        End Get
        Set(ByVal value As Object)
            _ServiceSync = value
        End Set
    End Property

    Private _ZipForDay As Integer = 1
    Public Property ZipForDay() As Integer
        Get
            Return _ZipForDay
        End Get
        Set(ByVal value As Integer)
            _ZipForDay = value
        End Set
    End Property

    Private _ZipFileName As String
    Public Property ZipFileName() As String
        Get
            Return _ZipFileName
        End Get
        Set(ByVal value As String)
            _ZipFileName = value
        End Set
    End Property

    Private _ZipFileFullPath As String
    Public Property ZipFileFullPath() As String
        Get
            Return _ZipFileFullPath
        End Get
        Set(ByVal value As String)
            _ZipFileFullPath = value
        End Set
    End Property

    Private _ClientId As String = ""
    Public Property ClientId() As String
        Get
            If _ClientId = "" Then
#If F5 = "1" Then
                Return "FABB262C-0D66-4165-BCD6-7C5284BF4400"
#End If
                Return ClsLanguage.GetLangIDDec
            Else
                Return _ClientId
            End If
        End Get
        Set(value As String)
            _ClientId = value
        End Set
    End Property

    Private _FromSchoolCode As String
    Public Property FromSchoolCode() As String
        Get
            Return _FromSchoolCode
        End Get
        Set(ByVal value As String)
            _FromSchoolCode = value
        End Set
    End Property

    Private _SourceFrom_ClientId As String
    Public Property SourceFrom_ClientId() As String
        Get
            Return _SourceFrom_ClientId
        End Get
        Set(ByVal value As String)
            _SourceFrom_ClientId = value
        End Set
    End Property

    Private _PathTempClient As String
    Public ReadOnly Property PathTempClient() As String
        Get
            Dim Path = System.IO.Path.Combine(Application.StartupPath, "TempFile")
            Dim FileMn As New ManageFile()
            FileMn.CreateFolder("TempFile")

            Return Path
        End Get
    End Property

    Private ListTableTest As New Dictionary(Of String, Integer)

    Public ReadOnly Property ClientLastSync(SchoolCode As String) As Date?
        Get
            Using ctx = GetLinqToSql.GetDataContext()
                Dim StartSync As Date

                Dim SyncLog = ctx.tblSyncLogs.Where(Function(q1) q1.LogType = EnSyncLog.ClientStartGetData And
                             ctx.tblSyncLogs.Where(Function(q2) q2.LogType = EnSyncLog.ClientSendDataSuccess).Select(Function(q2) q2.File_Id).Contains(q1.File_Id)) _
                             .OrderByDescending(Function(q3) q3.LastSync).FirstOrDefault

                If SyncLog Is Nothing Then
                    'ถ้าไม่เจอวันที่ใน log ครั้งแรกจะได้วันที่จาก setemail แทน
                    StartSync = (From q In ctx.tblSetEmails Where q.IsActive = True And q.SchoolId = SchoolCode).SingleOrDefault().ActivationDate
                Else
                    StartSync = SyncLog.LastSync
                End If

                Return StartSync
            End Using
        End Get
    End Property

    Public ReadOnly Property ClientEndDate() As Date
        Get
            'Return New Date(2014, 1, 31) 'todo ควรเป็น now นะ
            Dim System As New Service.ClsSystem(New ClassConnectSql(True))
            Return System.GetThaiDate
        End Get
    End Property

#End Region

    Public Sub New(WhoCreate As EnTypeSyncManager)
        CreateBy = WhoCreate
        If WhoCreate = EnTypeSyncManager.Client Then
            KnowledgeUtils.Database.DBFactory.RegisterApplicationManager(New BusinessTablet360.WindowsApplicationUpLoadDataManager)
            KnowledgeUtils.Database.DBFactory.RegisterDbCommon(New KnowledgeUtils.Database.DbCommonSqlServer)
        Else
            KnowledgeUtils.Database.DBFactory.RegisterApplicationManager(New BusinessTablet360.WebApplicationManager)
            KnowledgeUtils.Database.DBFactory.RegisterDbCommon(New KnowledgeUtils.Database.DbCommonSqlServer)
        End If

        'DeleteData()
        'CheckDataServer()
        'AlterColumn()
        ' RenameTableUnderScore()
        'AlterEditColumn()
    End Sub

    Public Function InsertSyncLog(ByVal Item As tblSyncLog) As tblSyncLog
        Try
            Dim System As Service.ClsSystem
            If CreateBy = EnTypeSyncManager.Server Then
                System = New Service.ClsSystem(New ClassConnectSql(False))
            Else
                System = New Service.ClsSystem(New ClassConnectSql(True))
            End If

            Using ctx = GetLinqToSql.GetDataContext()
                Item.LogId = Guid.NewGuid
                Item.LastUpdate = System.GetThaiDate
                Item.IsActive = True
                ctx.tblSyncLogs.InsertOnSubmit(Item)
                ctx.SubmitChanges()
            End Using

            Return Item
        Catch ex As Exception
            Throw New Exception(ex.Message)
            ElmahExtension.LogToElmah(ex)
            Return Nothing
        End Try
    End Function

    Public Function ImportDb(FullPath As String, StreamData As Byte()) As Boolean
        ManageDateTime.TraceTimeStart()

        Dim FnSqlUpdateByIdEqual = Function(Dt As DataTable) As String
                                       'gen sql update เทเบิลที่ id ตรงกัน
                                       Dim DtCol = Dt.Columns
                                       Dim DtPk = Dt.PrimaryKey
                                       Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                       Dim DtName = Dt.TableName

                                       Dim SqlSet As String = ""
                                       Dim RealPk = Dt.PrimaryKey.Select(Function(q) q.ColumnName).ToArray
                                       For Each Col As DataColumn In DtCol
                                           If Not RealPk.Contains(Col.ColumnName) Then
                                               'ไม่เอา primarykey
                                               SqlSet &= DtName & "." & Col.ColumnName & " = " & DtTempName & "." & Col.ColumnName & " ,"
                                           End If
                                       Next
                                       SqlSet = SqlSet.TrimEnd(",")

                                       Dim SqlJoin As String = ""
                                       For Each Pk In DtPk
                                           SqlJoin &= DtName & "." & Pk.ColumnName & " = " & DtTempName & "." & Pk.ColumnName & " AND "
                                       Next
                                       SqlJoin = SqlJoin.Substring(0, SqlJoin.Length - 1 - 4)


                                       Dim Sql = <s>UPDATE {0} SET {1} FROM {2} INNER JOIN {3} ON {4} </s>.Value
                                       Sql = String.Format(Sql, {DtName, SqlSet, DtName, DtTempName, SqlJoin})

                                       Return Sql

                                   End Function

        Dim FnSqlUpdateByIdEqualIfNewer = Function(Dt As DataTable) As String
                                              'gen sql update เทเบิลที่ id ตรงกัน
                                              Dim DtCol = Dt.Columns
                                              Dim DtPk = Dt.PrimaryKey
                                              Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                              Dim DtName = Dt.TableName

                                              Dim SqlSet As String = ""
                                              Dim RealPk = Dt.PrimaryKey.Select(Function(q) q.ColumnName).ToArray
                                              For Each Col As DataColumn In DtCol
                                                  If Not RealPk.Contains(Col.ColumnName) Then
                                                      'ไม่เอา primarykey
                                                      SqlSet &= DtName & "." & Col.ColumnName & " = " & DtTempName & "." & Col.ColumnName & " ,"
                                                  End If
                                              Next
                                              SqlSet = SqlSet.TrimEnd(",")

                                              Dim SqlJoin As String = ""
                                              For Each Pk In DtPk
                                                  SqlJoin &= DtName & "." & Pk.ColumnName & " = " & DtTempName & "." & Pk.ColumnName & " AND "
                                              Next
                                              SqlJoin = SqlJoin.Substring(0, SqlJoin.Length - 1 - 4)

                                              Dim SqlWhere As String = DtName & ".LASTUPDATE < " & DtTempName & ".LASTUPDATE"


                                              Dim Sql = <s>UPDATE {0} SET {1} FROM {2} INNER JOIN {3} ON {4} where {5} </s>.Value
                                              Sql = String.Format(Sql, {DtName, SqlSet, DtName, DtTempName, SqlJoin, SqlWhere})

                                              Return Sql

                                          End Function

        Dim FnSqlSelecttByIdNotEqual = Function(Dt As DataTable) As String
                                           'gen sql select เทเบิลที่ id ไม่ตรงกัน
                                           Dim DtPk = Dt.PrimaryKey
                                           Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                           Dim DtName = Dt.TableName

                                           Dim SqlWhere As String = ""
                                           For Each Pk In DtPk
                                               SqlWhere &= DtTempName & "." & Pk.ColumnName & " = " & DtName & "." & Pk.ColumnName & " AND "
                                           Next
                                           SqlWhere = SqlWhere.Substring(0, SqlWhere.Length - 1 - 4)

                                           Dim Sql = <s>SELECT {0}.* FROM {1} WHERE NOT EXISTS (SELECT * FROM {2} WHERE {3})</s>.Value
                                           Sql = String.Format(Sql, {DtTempName, DtTempName, DtName, SqlWhere})

                                           Return Sql
                                       End Function

        Dim FnSqlSelectByDataEqual = Function(Dt As DataTable, PkData As String(), SubSql As String) As String
                                         'gen select sql ที่ pk data ตรงกัน
                                         Dim DtPk = PkData
                                         Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                         Dim DtName = Dt.TableName
                                         Dim Sql = <s>SELECT {0}.* FROM ({1}) AS A INNER JOIN {2} ON </s>.Value
                                         Sql = String.Format(Sql, {"A", SubSql, DtName}) 'todo DtName หน้าจะเป็น A
                                         Dim SqlOn As String = ""
                                         For Each Pk In DtPk
                                             SqlOn &= DtName & "." & Pk & " = " & "A." & Pk & " AND "
                                         Next
                                         SqlOn = SqlOn.Substring(0, SqlOn.Length - 1 - 4)

                                         Return Sql & SqlOn
                                     End Function

        Dim FnSqlSelectIdNotEqualDataNotEqual = Function(Dt As DataTable, SubSqlIdNotEqual As String, SubSqlDataEqual As String) As String
                                                    Dim DtPk = Dt.PrimaryKey
                                                    Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                                    Dim DtName = Dt.TableName

                                                    'Dim SqlWhere As String = ""
                                                    'For Each Pk In DtPk
                                                    '    SqlWhere &= "B." & Pk.ColumnName & " = " & "C." & Pk.ColumnName & " AND "
                                                    'Next
                                                    'SqlWhere = SqlWhere.Substring(0, SqlWhere.Length - 1 - 4)

                                                    'Dim Sql = <s>SELECT B.* FROM ({0}) AS B WHERE NOT EXISTS (SELECT * FROM ({1}) AS C WHERE {2})</s>.Value
                                                    'Sql = String.Format(Sql, {SubSqlIdNotEqual, SubSqlDataEqual, SqlWhere})

                                                    Dim Sql = <s>SELECT * FROM ({0}) as aa EXCEPT SELECT * FROM ({1}) as bb</s>.Value
                                                    Sql = String.Format(Sql, {SubSqlIdNotEqual, SubSqlDataEqual})

                                                    Return Sql
                                                End Function

        'Dim FnSqlForUpdateTwoWayByData = Function(Dt As DataTable) As String
        '                                     'gen sql update เทเบิลที่ id ตรงกัน จิงๆเหมือน (FnSqlForUpdateById)
        '                                     Dim DtCol = Dt.Columns
        '                                     Dim DtPk = Dt.PrimaryKey
        '                                     Dim DtTempName = Dt.TableName & SourceFrom_ClientId.Replace("-", "")
        '                                     Dim DtName = Dt.TableName

        '                                     Dim SqlSet As String = ""
        '                                     Dim RealPk = Dt.PrimaryKey.Select(Function(q) q.ColumnName).ToArray
        '                                     For Each Col As DataColumn In DtCol
        '                                         If Not RealPk.Contains(Col.ColumnName) Then
        '                                             'ไม่เอา primarykey
        '                                             SqlSet &= DtName & "." & Col.ColumnName & " = " & DtTempName & "." & Col.ColumnName & " ,"
        '                                         End If
        '                                     Next
        '                                     SqlSet = SqlSet.TrimEnd(",")

        '                                     Dim SqlJoin As String = ""
        '                                     For Each Pk In DtPk
        '                                         SqlJoin &= DtName & "." & Pk.ColumnName & " = " & DtTempName & "." & Pk.ColumnName & " AND "
        '                                     Next
        '                                     SqlJoin = SqlJoin.Substring(0, SqlJoin.Length - 1 - 4)


        '                                     Dim Sql = <s>UPDATE {0} SET {1} FROM {2} INNER JOIN {3} ON {4} </s>.Value
        '                                     Sql = String.Format(Sql, {DtName, SqlSet, DtName, DtTempName, SqlJoin})

        '                                     Return Sql
        '                                 End Function

        Dim FnSqlUpdateIsActiveIfOlder = Function(Dt As DataTable, PkData As String()) As String
                                             'gen sql update isactive=0 ตาม view ทีถูกสร้างก่อนหน้า
                                             Dim DtCol = Dt.Columns
                                             Dim DtPk = PkData
                                             Dim DtTempName = "VIEW_" & Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                             Dim DtName = Dt.TableName

                                             Dim SqlSet As String = "IsActive=0"

                                             Dim SqlJoin As String = ""
                                             For Each Pk In DtPk
                                                 SqlJoin &= DtName & "." & Pk & " = " & DtTempName & "." & Pk & " AND "
                                             Next
                                             SqlJoin = SqlJoin.Substring(0, SqlJoin.Length - 1 - 4)

                                             Dim SqlWhere As String = DtName & ".LASTUPDATE < " & DtTempName & ".LASTUPDATE"


                                             Dim Sql = <s>UPDATE {0} SET {1} FROM {2} INNER JOIN {3} ON {4} WHERE {5} </s>.Value
                                             Sql = String.Format(Sql, {DtName, SqlSet, DtName, DtTempName, SqlJoin, SqlWhere})

                                             Return Sql
                                         End Function

        Dim FnSqlSelectIsActiveIfNewer = Function(Dt As DataTable, PkData As String()) As String
                                             'gen sql update isactive=0 ตาม view ทีถูกสร้างก่อนหน้า
                                             Dim DtCol = Dt.Columns
                                             Dim DtPk = PkData
                                             Dim DtTempName = "VIEW_" & Dt.TableName & SourceFrom_ClientId.Replace("-", "")
                                             Dim DtName = Dt.TableName

                                             Dim SqlJoin As String = ""
                                             For Each Pk In DtPk
                                                 SqlJoin &= DtTempName & "." & Pk & " = " & DtName & "." & Pk & " AND "
                                             Next
                                             SqlJoin = SqlJoin.Substring(0, SqlJoin.Length - 1 - 4)

                                             Dim SqlWhere As String = DtName & ".LASTUPDATE < " & DtTempName & ".LASTUPDATE"

                                             Dim Sql = <s>SELECT {0}.* FROM {1} INNER JOIN {2} ON {3} WHERE {4} </s>.Value
                                             Sql = String.Format(Sql, {DtTempName, DtTempName, DtName, SqlJoin, SqlWhere})

                                             Return Sql
                                         End Function


        Dim SqlExecute As String = ""
        Dim SqlSelect As String = ""

        'Trace.WriteLine("================")
        Dim TableName = ""
        Dim SqlUtil As ClassConnectSql
        If CreateBy = EnTypeSyncManager.Server Then
            Trace.WriteLine("== Sever Import")
            SqlUtil = New ClassConnectSql
        Else
            Trace.WriteLine("== Client Import")
            SqlUtil = New ClassConnectSql(True)
        End If
        Dim SqlCon As New System.Data.SqlClient.SqlConnection
        Dim NewSyncLog As tblSyncLog
        Try
            'เอา stream ทำเป็นไฟล์จริง
            Dim ms As New MemoryStream(StreamData)
            Dim fs As New FileStream(FullPath, FileMode.Create)
            ms.WriteTo(fs)
            ms.Close()
            fs.Close()
            fs.Dispose()

            SqlUtil.OpenExclusiveConnect(SqlCon)
            SqlUtil.OpenWithTransection(SqlCon)

            Dim GenDatasetUtil As New GenerateDataSet(FullPath, EnumLoad.XmlZipPath)
            Dim GenCommandUtil As New ManageCommandSql(KnowledgeUtils.Database.Command.EnDatabaseType.SqlServer)
            GenCommandUtil.AutoID = False

            'log ImportStart
            NewSyncLog = New tblSyncLog
            With NewSyncLog
                .File_Id = ZipFileName
                .LastSync = Nothing
                .Description = "เริ่ม import"
                .LogType = EnSyncLog.ImportStart
                .SourceFrom_ClientId = SourceFrom_ClientId
            End With
            InsertSyncLog(NewSyncLog)


            Dim DataSetXml = GenDatasetUtil.GetDataSet
            Dim ListTableRemove As New List(Of String) 'เก็บชื่อเทเบิลกลุ่มที่ทำงานเป็นชุดเมื่อทำงานไปแล้วเผื่อไม่ให้โดนทำซ้ำอีกครั้ง
            For Each Dt As DataTable In DataSetXml.Tables
                '1. สร้าง เทเบิลเสมือน table ที่จะ sync
                '2. copy ข้อมูลจากไฟล์ เข้าเทเลิบเสมือน
                '3. ทำการ import โดยแยกกลุ่ม ที่ทำทีละเทเบิล(oneway=เกิดการเปลี่ยนแปลงที่ ในโรงเรียนอย่างเดียว, twoway เกิดการเปลี่ยนแปลงที่ ในโรงเรียนกับนอกโรงเรียน),ทำทีละหลายเทเบิลหรือเป็นกลุ่ม (เป็น twoway แบบพิเศษ ต้อง update เป็นชุดๆ)

                TableName = Dt.TableName
                Trace.WriteLine("")
                Trace.WriteLine("import " & TableName & " " & Now.ToString("dd/MM/yyyy hh.mm.ss:fff"))

                '<<< Create Temp
                SqlExecute = <s>select top 0 * into {0} from {1} </s>.Value
                SqlExecute = String.Format(SqlExecute, {TableName & SourceFrom_ClientId.Replace("-", ""), TableName})
                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                Using BulkCopy As New SqlBulkCopy(SqlCon, SqlBulkCopyOptions.Default, SqlUtil.Tr)
                    BulkCopy.DestinationTableName = TableName & SourceFrom_ClientId.Replace("-", "")
                    BulkCopy.WriteToServer(Dt)
                End Using
                'Using BulkCopy As New SqlBulkCopy(SqlCon.ConnectionString)
                '    BulkCopy.DestinationTableName = TableName & SourceFrom_ClientId.Replace("-", "")
                '    BulkCopy.WriteToServer(Dt)
                'End Using
                '<<< Create Temp

                If Not ListTableRemove.Contains(TableName) Then
                    Dim IsOneWay = (TableSyncTwoWay.Where(Function(q) q = TableName).SingleOrDefault = "") OrElse
                               (TableSyncTwoWayIdPattern.Contains(TableName))
                    Trace.WriteLine("")

                    Dim PkData As String()
                    If Not IsOneWay AndAlso TableName <> "tblChatRoom" Then
                        'tblChatRoom พอดี table นี้สนใจแต่ id (แต่อยู่กลุ่ม two way)
                        PkData = MapDataKey.Item(TableName).Split(",")
                    End If
                    Dim IsCombo = TableSyncComboPattern.Contains(TableName)
                    If Not IsCombo Then
                        'กลุ่มรายเทเบิล

                        If IsOneWay Then
                            'one way
                            'update ที่ id ตรงกัน
                            SqlExecute = FnSqlUpdateByIdEqual(Dt)
                            Trace.WriteLine(SqlExecute)
                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                            'insert ที่ id ไม่ตรง
                            SqlSelect = FnSqlSelecttByIdNotEqual(Dt)
                            Dim DtDbInsert = SqlUtil.getdataNotDataSetWithTransection(SqlSelect, , SqlCon)
                            Using BulkCopy As New SqlBulkCopy(SqlCon, SqlBulkCopyOptions.Default, SqlUtil.Tr)
                                BulkCopy.DestinationTableName = TableName
                                BulkCopy.WriteToServer(DtDbInsert)
                            End Using
                        Else
                            'two way
                            'กลุ่มนี้ id ตรงกับของเก่านำไป update
                            SqlExecute = FnSqlUpdateByIdEqualIfNewer(Dt)
                            Trace.WriteLine(SqlExecute)
                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                            'กลุ่มนี้เป็นแบบ id ไม่ตรงแต่ pk ตรง ให้อัพเดดของเก่าออกไปแล้วทำการ insert โดยดุเงื่อนไขเวลาเก่าใหม่ด้วย
                            Dim SqlSelectByIdNotEqual = FnSqlSelecttByIdNotEqual(Dt)
                            Dim SqlSelectByDataEqual = FnSqlSelectByDataEqual(Dt, PkData, SqlSelectByIdNotEqual)
                            SqlUtil.ExecuteResultWithTransection("CREATE VIEW VIEW_" & TableName & SourceFrom_ClientId.Replace("-", "") & " AS " & SqlSelectByDataEqual, SqlCon)

                            SqlExecute = FnSqlUpdateIsActiveIfOlder(Dt, PkData)
                            Trace.WriteLine(SqlExecute)
                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                            Dim SqlSelectIsActiveIfNewer = FnSqlSelectIsActiveIfNewer(Dt, PkData)
                            Dim DtDbDeleteAndInsert = SqlUtil.getdataNotDataSetWithTransection(SqlSelectIsActiveIfNewer, , SqlCon)
                            Using BulkCopy As New SqlBulkCopy(SqlCon, SqlBulkCopyOptions.Default, SqlUtil.Tr)
                                BulkCopy.DestinationTableName = TableName
                                BulkCopy.WriteToServer(DtDbDeleteAndInsert)
                            End Using
                            SqlExecute = <s>DROP VIEW {0}</s>.Value
                            SqlExecute = String.Format(SqlExecute, {"VIEW_" & TableName & SourceFrom_ClientId.Replace("-", "")})
                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                            'กลุ่มนี้เงื่อนไข id และ pk ไม่ตรงด้วยดังนั้น insert เลย
                            Dim SqlSelectIdNotEqualDataNotEqual = FnSqlSelectIdNotEqualDataNotEqual(Dt, SqlSelectByIdNotEqual, SqlSelectByDataEqual)
                            Dim DtDbInsert = SqlUtil.getdataNotDataSetWithTransection(SqlSelectIdNotEqualDataNotEqual, , SqlCon)

                            Using BulkCopy As New SqlBulkCopy(SqlCon, SqlBulkCopyOptions.Default, SqlUtil.Tr)
                                BulkCopy.DestinationTableName = TableName
                                BulkCopy.WriteToServer(DtDbInsert)
                            End Using

                        End If
                    Else
                        'กลุ่มต้องดูเป็นชุด ดูที่ master สุด
                        Select Case TableName
                            Case "tblChatRoom"
                                For Each Dr As DataRow In Dt.Rows
                                    Dim TblChatJoin = GenDatasetUtil.GetDataSet.Tables("tblChatJoin")
                                    Dim TblChatMessage = GenDatasetUtil.GetDataSet.Tables("tblChatMessage")
                                    Dim TblChatRecipient = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient")
                                    Dim TblChatJoinFiter = GenDatasetUtil.GetDataSet.Tables("tblChatJoin").AsEnumerable.Where(Function(q) q("ChatRoom_Id") = Dr("ChatRoom_Id")).ToArray
                                    Dim TblChatMessageFiter = GenDatasetUtil.GetDataSet.Tables("tblChatMessage").AsEnumerable.Where(Function(q) q("ChatRoom_Id") = Dr("ChatRoom_Id")).ToArray

                                    'เช็คว่า idroom มีใครบ้างที่ chatjoin ใน xml
                                    Dim WhoTalks = TblChatJoin.AsEnumerable.Where(Function(q) q("ChatRoom_Id") = Dr("ChatRoom_Id")).ToArray
                                    SqlSelect = "SELECT ChatRoom_Id FROM tblChatJoin WHERE ChatUser_Id='" & WhoTalks(0)("ChatUser_Id").ToString & "' OR ChatUser_Id='" & WhoTalks(1)("ChatUser_Id").ToString & "'" & _
                                                " group by ChatRoom_Id having count(ChatRoom_Id) = 2"
                                    Dim DtDB = SqlUtil.getdataNotDataSetWithTransection(SqlSelect, , SqlCon)
                                    If DtDB.Rows.Count = 0 Then
                                        'ถ้าไม่มี insert ตัวลูกให้หมด

                                        SqlExecute = GenCommandUtil.CreateCommandSql(Dt, Dr, EnActionType.Insert)
                                        Trace.WriteLine(SqlExecute)
                                        SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                        For Each Row In TblChatJoinFiter
                                            SqlExecute = GenCommandUtil.CreateCommandSql(TblChatJoin, Row, EnActionType.Insert)
                                            Trace.WriteLine(SqlExecute)
                                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                        Next
                                        For Each Row In TblChatMessageFiter
                                            SqlExecute = GenCommandUtil.CreateCommandSql(TblChatMessage, Row, EnActionType.Insert)
                                            Trace.WriteLine(SqlExecute)
                                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)


                                            Dim TblChatRecipientFilter = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient").AsEnumerable.Where(Function(q) q("CM_Id") = Row("CM_Id")).SingleOrDefault
                                            SqlExecute = GenCommandUtil.CreateCommandSql(TblChatRecipient, TblChatRecipientFilter, EnActionType.Insert)
                                            Trace.WriteLine(SqlExecute)
                                            SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                        Next
                                    Else
                                        'ถ้ามีอยู่แล้ว
                                        If CreateBy = EnTypeSyncManager.Client Then
                                            'ทำที่ client
                                            'ดูฝั่งไหนใหม่กว่ากัน
                                            SqlSelect = "SELECT * FROM tblChatJoin WHERE ChatRoom_Id='" & DtDB(0)("ChatRoom_Id").ToString & "'"
                                            DtDB = SqlUtil.getdataNotDataSetWithTransection(SqlSelect, , SqlCon)

                                            Dim LastUpdateServer = CType(WhoTalks(0)("LastUpdate"), DateTime)
                                            Dim LastUpdateClient = CType(DtDB.Rows(0)("LastUpdate"), DateTime)
                                            If (LastUpdateServer - LastUpdateClient).TotalSeconds > 0 Then
                                                'ถ้าฝั่ง server ใหม่กว่า ปรับ room id ที่ client ด้วย room id ของ server แล้ว นำ TblChatMessage,tblChatRecipient ของ server เข้า
                                                Dim RoomIdServer = Dr("ChatRoom_Id")
                                                Dim RoomIdClient = DtDB.Rows(0)("ChatRoom_Id")
                                                SqlExecute = "UPDATE tblChatRoom SET ChatRoom_Id='" & RoomIdServer.ToString & "' WHERE ChatRoom_Id='" & RoomIdClient.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                SqlExecute = "UPDATE tblChatJoin SET ChatRoom_Id='" & RoomIdServer.ToString & "' WHERE ChatRoom_Id='" & RoomIdClient.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                SqlExecute = "UPDATE tblChatMessage SET ChatRoom_Id='" & RoomIdServer.ToString & "' WHERE ChatRoom_Id='" & RoomIdClient.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                For Each Row In TblChatMessageFiter
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatMessage, Row, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                    Dim TblChatRecipientFilter = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient").AsEnumerable.Where(Function(q) q("CM_Id") = Row("CM_Id")).SingleOrDefault
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatRecipient, TblChatRecipientFilter, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                Next
                                            Else
                                                'ถ้าฝั่ง client ใหม่กว่า ก่อน insert message  ให้ปรับ roomid ของ server ด้วย roomid ของ client
                                                Dim RoomIdClient = DtDB.Rows(0)("ChatRoom_Id")

                                                For Each Row In TblChatMessageFiter
                                                    Row("ChatRoom_Id") = RoomIdClient
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatMessage, Row, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                    Dim TblChatRecipientFilter = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient").AsEnumerable.Where(Function(q) q("CM_Id") = Row("CM_Id")).SingleOrDefault
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatRecipient, TblChatRecipientFilter, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                Next
                                            End If
                                        Else
                                            'ทำที่ server
                                            'ดูฝั่งไหนใหม่กว่ากัน
                                            SqlSelect = "SELECT * FROM tblChatJoin WHERE ChatRoom_Id='" & DtDB(0)("ChatRoom_Id").ToString & "'"
                                            DtDB = SqlUtil.getdataNotDataSetWithTransection(SqlSelect, , SqlCon)

                                            Dim LastUpdateClient = CType(WhoTalks(0)("LastUpdate"), DateTime)
                                            Dim LastUpdateServer = CType(DtDB.Rows(0)("LastUpdate"), DateTime)
                                            If (LastUpdateClient - LastUpdateServer).TotalSeconds > 0 Then
                                                'ถ้าฝั่ง client ใหม่กว่า ปรับ room id ที่ server ด้วย room id ของ client แล้ว นำ TblChatMessage,tblChatRecipient ของ client เข้า
                                                Dim RoomIdClient = Dr("ChatRoom_Id")
                                                Dim RoomIdServer = DtDB.Rows(0)("ChatRoom_Id")
                                                SqlExecute = "UPDATE tblChatRoom SET ChatRoom_Id='" & RoomIdClient.ToString & "' WHERE ChatRoom_Id='" & RoomIdServer.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                SqlExecute = "UPDATE tblChatJoin SET ChatRoom_Id='" & RoomIdClient.ToString & "' WHERE ChatRoom_Id='" & RoomIdServer.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                SqlExecute = "UPDATE tblChatMessage SET ChatRoom_Id='" & RoomIdClient.ToString & "' WHERE ChatRoom_Id='" & RoomIdServer.ToString & "'"
                                                Trace.WriteLine(SqlExecute)
                                                SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                For Each Row In TblChatMessageFiter
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatMessage, Row, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                    Dim TblChatRecipientFilter = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient").AsEnumerable.Where(Function(q) q("CM_Id") = Row("CM_Id")).SingleOrDefault
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatRecipient, TblChatRecipientFilter, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                Next
                                            Else
                                                'ถ้าฝั่ง server ใหม่กว่า ก่อน insert message ที่ client ให้ปรับ roomid ด้วย roomid ของ server
                                                Dim RoomIdClient = Dr("ChatRoom_Id")
                                                Dim RoomIdServer = DtDB.Rows(0)("ChatRoom_Id")

                                                For Each Row In TblChatMessageFiter
                                                    Row("ChatRoom_Id") = RoomIdServer

                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatMessage, Row, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                                                    Dim TblChatRecipientFilter = GenDatasetUtil.GetDataSet.Tables("tblChatRecipient").AsEnumerable.Where(Function(q) q("CM_Id") = Row("CM_Id")).SingleOrDefault
                                                    SqlExecute = GenCommandUtil.CreateCommandSql(TblChatRecipient, TblChatRecipientFilter, EnActionType.Insert)
                                                    Trace.WriteLine(SqlExecute)
                                                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)
                                                Next
                                            End If

                                        End If
                                    End If
                                Next
                                'เอาเทเบิลกลุ่มที่ทำไปแล้วออกจากที่เหลือ
                                ListTableRemove.Add("tblChatJoin") : ListTableRemove.Add("tblChatMessage") : ListTableRemove.Add("tblChatRecipient")
                        End Select
                    End If


                    Trace.WriteLine("import end " & TableName & " " & Now.ToString("dd/MM/yyyy hh.mm.ss:fff"))
                    'log ImportTableSuccess
                    NewSyncLog = New tblSyncLog
                    With NewSyncLog
                        .File_Id = ZipFileName
                        .LastSync = Nothing
                        .Description = TableName & ", จำนวนข้อมูล " & Dt.Rows.Count
                        .LogType = EnSyncLog.ImportTableSuccess
                        .SourceFrom_ClientId = SourceFrom_ClientId
                    End With
                    InsertSyncLog(NewSyncLog)

                    '<<< Delete Temp
                    SqlExecute = <s>DROP TABLE {0}</s>.Value
                    SqlExecute = String.Format(SqlExecute, {TableName & SourceFrom_ClientId.Replace("-", "")})
                    SqlUtil.ExecuteWithTransection(SqlExecute, SqlCon)

                    '<<< Delete Temp
                End If
            Next
            Trace.WriteLine("เวลาที่ใช้ไป " & ManageDateTime.TraceTimeDiff)

            SqlUtil.CommitTransection(SqlCon)
            SqlUtil.CloseExclusiveConnect(SqlCon)

            'log ImportSuccess
            NewSyncLog = New tblSyncLog
            With NewSyncLog
                .File_Id = ZipFileName
                .LastSync = Nothing
                .Description = "จบการ import"
                .LogType = EnSyncLog.ImportSuccess
                .SourceFrom_ClientId = SourceFrom_ClientId
            End With
            InsertSyncLog(NewSyncLog)
            Trace.WriteLine("================")

            Return True
        Catch ex As Exception
            SqlUtil.RollbackTransection(SqlCon)

            'log ImportTableError
            NewSyncLog = New tblSyncLog
            With NewSyncLog
                .File_Id = ZipFileName
                .LastSync = Nothing
                .Description = "err " & TableName & ", " & ex.Message
                .LogType = EnSyncLog.ImportTableError
                .SourceFrom_ClientId = SourceFrom_ClientId
            End With
            InsertSyncLog(NewSyncLog)
            Trace.TraceError(ex.Message)
            Trace.WriteLine("================")

            ElmahExtension.LogToElmah(ex)
            Return False
        End Try

    End Function

#Region "Server"

    Public Function ServerGetData(SchoolCode As String) As ResultZipFile
        Dim TableNameRun As String = ""
        Try
            Trace.WriteLine("")
            Trace.WriteLine("== ServerGetData (SchoolCode:" & SchoolCode & ")")

            Dim SystemDb As New Service.ClsSystem(New ClassConnectSql(False))
            Dim StartDate As Date = SystemDb.GetThaiDate
            Dim NowDataBase As Date = StartDate
            Dim EndDate As Date
            Dim IsUpdate As Boolean = False
            FromSchoolCode = SchoolCode
            Using ctx = GetLinqToSql.GetDataContext()
                'หาวันที่ล่าสุดในการทำการกวาดดาต้าล่าสุดจาก server
                Dim LogGetDatas = ctx.tblSyncLogs.Where(Function(q) q.LogType = EnSyncLog.ServerStartGetData And q.School_Code = FromSchoolCode).OrderByDescending(Function(q) q.LastSync).ToArray
                Dim ISFound As Boolean = False

                For Each Row In LogGetDatas
                    ISFound = (ctx.tblSyncLogs.Where(Function(q) q.LogType = EnSyncLog.ServerSendDataSuccess And q.File_Id = Row.File_Id).SingleOrDefault IsNot Nothing)
                    If ISFound Then
                        StartDate = Row.LastSync
                        Exit For
                    End If
                Next
                EndDate = ServiceSystem.CalEndDateForSync(StartDate, ZipForDay)
                If EndDate > NowDataBase Then
                    Dim SystemService As New Service.ClsSystem(New ClassConnectSql(False))
                    EndDate = SystemService.GetThaiDate
                    IsUpdate = True
                End If

                Trace.Indent()
                Trace.WriteLine("StartDate: " & StartDate.ToString("dd/MM/yyyy hh.mm.ss:fff"))
                Trace.WriteLine("EndDate: " & EndDate.ToString("dd/MM/yyyy hh.mm.ss:fff"))

                ZipFileName = ClientId & "_" & StartDate.ToString("ddMMyyyyhhmmssfff") & ".zip"
                'log ServerStartGetData
                Dim NewSyncLog As New tblSyncLog
                With NewSyncLog
                    .File_Id = ZipFileName
                    .LastSync = EndDate
                    .Description = "กวาดล่าสุด " & StartDate.ToString("o")
                    .LogType = EnSyncLog.ServerStartGetData
                    .SourceFrom_ClientId = ClientId
                    .School_Code = FromSchoolCode  'ไว้เก็บให้ server รู้ว่าอันนี้เป็น log ที่กวาดให้โรงเรียนอะไร
                End With
                InsertSyncLog(NewSyncLog)

                Dim Dt As DataTable
                Dim Ds As New DataSet
                Dim HaveData As Boolean
                Dim GenXml As New GenerateXml("", EnVersionType.Database, "", "")
                Dim System As New Service.ClsSystem(New ClassConnectSql(False))
                Dim ProcessTable = Sub(TableName As String, _RunSync As Date?, _EndDaySync As Date)
                                       Dim FilterSchoolCode = ""
                                       If MapSchoolCodeName.ContainsKey(TableName) Then
                                           Dim SchoolColumn = MapSchoolCodeName.Item(TableName)
                                           FilterSchoolCode = SchoolColumn & "='" & SchoolCode & "'"
                                       End If
                                       Dt = System.GetTableByLastUpdateForServer(TableName, FilterSchoolCode, _RunSync, _EndDaySync)
                                       If Dt.Rows.Count > 0 Then
                                           For Each Row In Dt.Rows
                                               Row("ClientId") = ClientId
                                           Next

                                           HaveData = True
                                           Trace.WriteLine(TableName & "," & Dt.Rows.Count)
                                           Ds.Tables.Add(Dt)
                                           'KeepToListTest(TableName, Dt.Rows.Count)
                                       End If
                                   End Sub
                Dim ProcessTableNotWpp = Sub(TableName As String, _RunSync As Date?, _EndDaySync As Date)
                                             'Sync เฉพาะข้อมูลของโรงเรียนไม่ Sync ข้อมูลที่มาจาก วพ
                                             Dim FilterSchoolCode = ""
                                             If MapSchoolCodeName.ContainsKey(TableName) Then
                                                 Dim SchoolColumn = MapSchoolCodeName.Item(TableName)
                                                 FilterSchoolCode = SchoolColumn & "='" & SchoolCode & "'"
                                             End If
                                             Dt = System.GetTableByLastUpdateForServer(TableName, FilterSchoolCode, _RunSync, _EndDaySync, "IsWpp=0")
                                             If Dt.Rows.Count > 0 Then
                                                 For Each Row In Dt.Rows
                                                     Row("ClientId") = ClientId
                                                 Next

                                                 HaveData = True
                                                 Trace.WriteLine(TableName & "," & Dt.Rows.Count)
                                                 Ds.Tables.Add(Dt)
                                                 'KeepToListTest(TableName, Dt.Rows.Count)
                                             End If
                                         End Sub


                'ทำที่ไม่ใช่อยู่ในพวกกลุ่ม ทางเดียว
                'Dim TableSyncTwoWay1 = TableSync.Where(Function(q) TableSyncTwoWay.Contains(q))
                For Each RunTable In TableSyncTwoWay
                    TableNameRun = RunTable
                    ProcessTable(RunTable, StartDate, EndDate)
                Next
                Dim TableSyncNotWppTwoWay = TableSyncNotWpp.Where(Function(q) TableSyncTwoWay.Contains(q))
                For Each RunTable In TableSyncNotWppTwoWay
                    TableNameRun = RunTable
                    ProcessTableNotWpp(RunTable, StartDate, EndDate)
                Next

                Dim Result As New ResultZipFile
                If HaveData Then
                    'ถ้ามีข้อมูล server เก็บ log ServerSendDataSuccess จาก client จะเป็นคนบอกให้เก็บ
                    Result.DataStream = GenXml.CreateXmlZip(Ds)
                    Result.FileId = ZipFileName
                    Result.ClientId = ClientId
                Else
                    'ถ้าไม่มีข้อมูล server เก็บ log ServerSendDataSuccess
                    ServerSendSuccess(ZipFileName, True)
                End If
                Result.IsUpdated = IsUpdate

                Trace.Unindent()
                Return Result
            End Using
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return New ResultZipFile With {.IsErr = True, .ErrExceptionDetail = ex.Message,
                                           .ErrDetail = "เกิดข้อผิดพลาดจาก ServerGetData ClientId=" & ClientId & " ที่ร้องขอจาก SchoolCode=" & SchoolCode & "ที่ table=" & TableNameRun}
        Finally
            Trace.WriteLine("=============== ServerGetData")
        End Try
    End Function

    Public Function ServerSendSuccess(FileID As String, Optional IsNoData As Boolean = False) As Boolean
        'ไว้ให้ client บอกว่าฝั่ง client ได้รับข้อมูลมา import เรียบร้อยดีแล้วนะ
        Try
            Dim NewSyncLog As New tblSyncLog
            With NewSyncLog
                .File_Id = FileID
                .LastSync = Nothing
                If IsNoData Then
                    .Description = "ส่งเสร็จ ไม่มีข้อมูล"
                Else
                    .Description = "ส่งเสร็จ"
                End If
                .LogType = EnSyncLog.ServerSendDataSuccess
                .SourceFrom_ClientId = ClientId
                .School_Code = FromSchoolCode
            End With
            InsertSyncLog(NewSyncLog)

            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function ServerImportDb(DataStream As Byte(), FullPath As String, ClientId As String) As Boolean
        ZipFileName = (New ManageFile(FullPath)).FullFileName
        SourceFrom_ClientId = ClientId
        Return ImportDb(FullPath, DataStream)
    End Function

#End Region

#Region "Client"

    Public Sub ClientRequestDataFromServer()
        Dim SchoolCodes As String()
        Using ctx = GetLinqToSql.GetDataContext()
            SchoolCodes = (From q In ctx.tblSetEmails Where q.IsActive = True).Select(Function(q) q.SchoolId).Distinct.ToArray
        End Using
        'วนทำทุกโรงเรียนที่อยู่ใน setmail (จิงๆควรเจอโรงเรียนเดียวในเคสระดับโรงเรียนไปจังหวัดนะ ระดับจังหวัดไปกรุงเทพยังไม่คิด)
        For Each School In SchoolCodes
            IsPass.Add(School, True)
            Dim IsSuccessImport As Boolean = True
            Dim Result As Object
            Do
                Result = ServiceSync.ServerGetData(School)
                If Result.IsErr Then
                    'ถ้าพบ err ที่ server เลิกทำงาน
                    Dim MsgErr = String.Format("{0} , {1}", {Result.ErrDetail, Result.ErrExceptionDetail})
                    IsPass(School) = False
                    Trace.WriteLine(MsgErr)
                    Dim NewSyncLog As New tblSyncLog
                    With NewSyncLog
                        .Description = MsgErr
                        .LogType = EnSyncLog.ServerGetDataErr
                    End With
                    InsertSyncLog(NewSyncLog)
                End If

                'วนนำเข้า table ทั้งหมด
                If Result.DataStream IsNot Nothing Then
                    'เรียก import
                    SourceFrom_ClientId = Result.ClientId
                    ZipFileName = Result.FileId
                    ZipFileFullPath = System.IO.Path.Combine(PathTempClient, Result.FileId)
                    IsSuccessImport = ImportDb(ZipFileFullPath, Result.DataStream)
                    If IsSuccessImport Then
                        'ส่ง log
                        ServiceSync.ServerSendSuccess(Result.FileId & "," & School)
                    Else
                        IsPass(School) = False
                    End If
                End If

            Loop Until Result.IsUpdated OrElse Not IsSuccessImport 'ถ้าข้อมูลที่ server ข้อมูลที่ได้ update แล้ว หรือ import ไม่สำเร็จ ออกจากการทำงาน

            If Result.IsUpdated AndAlso Not Result.IsErr AndAlso IsSuccessImport Then
                'client ส่งดาต้าเผื่อให้ server import บ้าง เมื่อฝั่ง server ส่งข้อมูลจน update แล้ว และไม่มี err
                ClientSendData(School)
            End If
        Next
    End Sub

    Public Sub ClientSendData(SchoolCode As String)
        Dim RunSync As Date = ClientLastSync(SchoolCode)
        Dim TableNameRun As String = ""
        Dim HaveData As Boolean
        Try

            Dim GenXml As New GenerateXml("", EnVersionType.Database, "", "")
            Dim System As New Service.ClsSystem(New ClassConnectSql(True))
            Dim Dt As DataTable
            Dim Ds As DataSet

            Dim KeepToListTest = Sub(TableName As String, RowCount As Integer)
                                     ListTableTest.Item(TableName) += RowCount
                                 End Sub
            Dim ProcessTable = Sub(TableName As String, _RunSync As Date?, _EndDaySync As Date)
                                   Dim FilterSchoolCode = ""
                                   If MapSchoolCodeName.ContainsKey(TableName) Then
                                       Dim SchoolColumn = MapSchoolCodeName.Item(TableName)
                                       FilterSchoolCode = SchoolColumn & "='" & SchoolCode & "'"
                                   End If
                                   Dt = System.GetTableByLastUpdateForClient(TableName, FilterSchoolCode, _RunSync, _EndDaySync)
                                   If Dt.Rows.Count > 0 Then
                                       HaveData = True
                                       Trace.WriteLine(TableName & "," & Dt.Rows.Count)
                                       For Each Row In Dt.Rows
                                           Row("ClientId") = ClientId
                                       Next
                                       Ds.Tables.Add(Dt)
#If ForTestUpload = "1" Then
                                       KeepToListTest(TableName, Dt.Rows.Count)
#End If
                                   End If
                               End Sub
            Dim ProcessTableNotWpp = Sub(TableName As String, _RunSync As Date?, _EndDaySync As Date)
                                         'Sync เฉพาะข้อมูลของโรงเรียนไม่ Sync ข้อมูลที่มาจาก วพ
                                         Dim FilterSchoolCode = ""
                                         If MapSchoolCodeName.ContainsKey(TableName) Then
                                             Dim SchoolColumn = MapSchoolCodeName.Item(TableName)
                                             FilterSchoolCode = SchoolColumn & "='" & SchoolCode & "'"
                                         End If
                                         Dt = System.GetTableByLastUpdateForClient(TableName, FilterSchoolCode, _RunSync, _EndDaySync, "IsWpp=0")
                                         If Dt.Rows.Count > 0 Then
                                             HaveData = True
                                             Trace.WriteLine(TableName & "," & Dt.Rows.Count)
                                             For Each Row In Dt.Rows
                                                 Row("ClientId") = ClientId
                                             Next
                                             Ds.Tables.Add(Dt)
#If ForTestUpload = "1" Then
                                             KeepToListTest(TableName, Dt.Rows.Count)
#End If
                                         End If
                                     End Sub

#If ForTestUpload = "1" Then

            For Each RunTable In TableSync
                ListTableTest.Add(RunTable, 0)
            Next
            For Each RunTable In TableSyncNotWpp
                ListTableTest.Add(RunTable, 0)
            Next
#End If
            'Trace.WriteLine("=============== ClientSendData (" & SchoolCode & ") Start " & Now.ToString)
            ManageDateTime.TraceTimeStart()
            Dim _EndDate = ClientEndDate 'เก็บไว้ตัวแปรนี้ก่อนเฝื่อจะได้เวลาแรกของการวนลูป
            'Trace.WriteLine(RunSync.ToString("dd/MM/yyyy hh.mm.ss:fff") & " vs now = " & _EndDate.ToString("dd/MM/yyyy hh.mm.ss:fff") & " ," & DateDiff(DateInterval.Second, RunSync, _EndDate).ToString)
            Do While DateDiff(DateInterval.Second, RunSync, _EndDate) > 0
                HaveData = False
                Trace.WriteLine("== ClientSendData (" & SchoolCode & ")")
                Trace.Indent()
                Ds = New DataSet
                Dim EndDaySync As Date
                EndDaySync = ServiceSystem.CalEndDateForSync(RunSync, ZipForDay)
                If (EndDaySync - _EndDate).TotalMilliseconds > 0 Then
                    'ถ้าวันที่สิ้นสุด sync มากกว่าเวลาเริ่ม sync ต้องปรับให้ทำกัน EndDaySync > _EndDate
                    EndDaySync = _EndDate
                End If
                'Trace.WriteLine("รอบ " & RunSync.ToString("dd/MM/yyyy hh.mm.ss:fff") & " vs " & EndDaySync.ToString("dd/MM/yyyy hh.mm.ss:fff") & " ," & DateDiff(DateInterval.Second, EndDaySync, _EndDate).ToString)
                Trace.WriteLine("StartDate: " & RunSync.ToString("dd/MM/yyyy hh.mm.ss:fff"))
                Trace.WriteLine("EndDate: " & EndDaySync.ToString("dd/MM/yyyy hh.mm.ss:fff"))

                ZipFileName = ClientId & "_" & RunSync.ToString("ddMMyyyyhhmmssfff") & ".zip"

                'log ClientStartGetData
                Dim NewSyncLog As New tblSyncLog
                With NewSyncLog
                    .File_Id = ZipFileName
                    .LastSync = EndDaySync
                    .Description = "กวาดล่าสุด " & RunSync.ToString("o")
                    .LogType = EnSyncLog.ClientStartGetData
                    .SourceFrom_ClientId = ClientId
                End With
                InsertSyncLog(NewSyncLog)

                'TableSync = TableSyncOneWay todo
                For Each RunTable In TableSync
                    TableNameRun = RunTable
                    ProcessTable(RunTable, RunSync, EndDaySync)
                Next
                'TableSyncNotWpp = {} 'todo
                For Each RunTable In TableSyncNotWpp
                    TableNameRun = RunTable
                    ProcessTableNotWpp(RunTable, RunSync, EndDaySync)
                Next

                If HaveData Then
                    Dim DataStream As Byte() = GenXml.CreateXmlZip(Ds)
                    If ServiceSync.ServerImportDb(DataStream, ZipFileName, ClientId) Then
                        'log  ClientSendDataSuccess
                        NewSyncLog = New tblSyncLog
                        With NewSyncLog
                            .File_Id = ZipFileName
                            .LastSync = Nothing
                            .Description = "ส่งเสร็จ"
                            .LogType = EnSyncLog.ClientSendDataSuccess
                            .SourceFrom_ClientId = ClientId
                        End With
                        InsertSyncLog(NewSyncLog)
                    Else
                        'server import ไม่สำเร็จ
                        IsPass(SchoolCode) = False
                        Exit Do
                    End If
                Else
                    NewSyncLog = New tblSyncLog
                    With NewSyncLog
                        .File_Id = ZipFileName
                        .LastSync = Nothing
                        .Description = "ส่งเสร็จ ไม่มีข้อมูล"
                        .LogType = EnSyncLog.ClientSendDataSuccess
                        .SourceFrom_ClientId = ClientId
                    End With
                    InsertSyncLog(NewSyncLog)
                End If

                RunSync = ClientLastSync(SchoolCode)
                'Trace.WriteLine("เวลาที่ใช้ไป " & ManageDateTime.TraceTimeDiff)
                'Trace.WriteLine("New RunSync " & RunSync.ToString("dd/MM/yyyy hh.mm.ss:fff"))
                'RunSync = New Date(RunSync.Value.Year, RunSync.Value.Month, RunSync.Value.Day).AddDays(ZipForDay)
                Trace.Unindent()
            Loop
            Trace.Unindent()
            Trace.WriteLine("=============== End ClientSendData " & "เวลาที่ใช้ไป " & ManageDateTime.TraceTimeDiff)
            Trace.WriteLine("")
            Trace.WriteLine("")

#If ForTestUpload = "1" Then
            Trace.WriteLine("======= Summary Source")
            Trace.Indent()
            For Each Row In ListTableTest
                Trace.WriteLine(Row.Key & "," & Row.Value)
            Next
            Trace.Unindent()
            Trace.WriteLine("======= Summary Definition")
            Trace.Indent()
            For Each Row In ListTableTest
                Dim System1 As New Service.ClsSystem(New ClassConnectSql(True, "Data Source=10.100.1.72;Initial Catalog=QuickTest_9_MainServer;Persist Security Info=True;User ID=sa;Password=kl123;Max Pool Size = 50000"))
                Dim Dt1 = System1.GetTableByLastUpdate(Row.Key)
                Trace.WriteLine(Row.Key & "," & Dt1.Rows.Count)
            Next
#End If
        Catch ex As Exception
            IsPass(SchoolCode) = False
            Trace.WriteLine(ex.Message)
            Dim NewSyncLog As New tblSyncLog
            With NewSyncLog
                Dim MsgErr = String.Format("{0} , {1}", {"เกิดข้อผิดพลาดจาก ClientData ClientId=" & ClientId & "ที่ table=" & TableNameRun, ex.Message})
                .Description = MsgErr
                .LogType = EnSyncLog.ClientGetDataErr
            End With
            InsertSyncLog(NewSyncLog)
            ElmahExtension.LogToElmah(ex)
        End Try
    End Sub

#End Region

#Region "TestData"

    Private Function DeleteData() As Boolean
        'ใช้ตอนทำเทสดาต้า
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql(True))
            For Each RunTable In TableSync
                System.DeleteData(RunTable)
            Next
            For Each RunTable In TableSyncNotWpp
                System.DeleteData(RunTable)
            Next
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Private Sub CheckDataServer()
        Dim Dt As New DataTable
        Dim System As New Service.ClsSystem(New ClassConnectSql(True))
        Dim ProcessTable = Sub(TableName As String, _RunSync As Date?, _EndDaySync As Date)
                               Dt = System.GetTableByLastUpdate(TableName, _RunSync, _EndDaySync)
                               Trace.WriteLine(TableName & "," & Dt.Rows.Count)
                           End Sub
        'Dim Sd As New Date(2013, 1, 1)
        'Dim Ed As New Date(2014, 1, 31)
        Dim Sd As New Date(2013, 9, 1)
        Dim Ed As New Date(2013, 11, 30)


        For Each Row In TableSync
            ProcessTable(Row, Sd, Ed)
        Next
        For Each Row In TableSyncNotWpp
            ProcessTable(Row, Sd, Ed)
        Next
    End Sub

    Private Sub AlterColumn()
        'ใช้สร้างคอลัม
        Try
            Dim Dt As New DataTable
            Dim System As New Service.ClsSystem(New ClassConnectSql(True))
            Dim ProcessTable = Sub(TableName As String)
                                   System.AlterClientIdColumn(TableName)
                                   Trace.WriteLine(TableName)
                               End Sub

            For Each Row In TableSync
                ProcessTable(Row)
            Next
            For Each Row In TableSyncNotWpp
                ProcessTable(Row)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
            ElmahExtension.LogToElmah(ex)
        End Try
    End Sub

    Private Sub AlterEditColumn()
        'Try
        Dim Dt As New DataTable
        Dim System As New Service.ClsSystem(New ClassConnectSql(True))
        Dim ProcessTable = Sub(TableName As String)
                               System.AlterEditLastUpdateColumn(TableName)
                               Trace.WriteLine(TableName)
                           End Sub

        For Each Row In TableSync
            ProcessTable(Row)
        Next
        For Each Row In TableSyncNotWpp
            ProcessTable(Row)
        Next
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Private Sub RenameTableUnderScore()
        'ใช้สร้างคอลัม
        Dim Dt As New DataTable
        Dim System As New Service.ClsSystem(New ClassConnectSql(True))
        Dim ProcessTable = Sub(TableName As String)
                               Try
                                   Trace.WriteLine(TableName)
                                   System.RenameTableUnderScore(TableName)
                               Catch ex As Exception
                                   Trace.WriteLine(ex.Message)
                                   ElmahExtension.LogToElmah(ex)
                               End Try
                           End Sub

        For Each Row In TableReName
            ProcessTable(Row)
        Next

    End Sub

#End Region

End Class

Public Enum EnSyncLog
    ServerStartGetData
    ServerSendDataSuccess
    ClientStartGetData
    ClientSendDataSuccess
    ImportStart
    ImportTableSuccess
    ImportTableError
    ImportSuccess
    ServerGetDataErr
    ClientGetDataErr

    'todo เพิ่ม type log err ตอน เก็บของ
End Enum

Public Enum EnTypeSyncManager
    Client
    Server
End Enum

<Serializable()> _
Public Class ResultZipFile

    Public DataStream As Byte()
    Public FileId As String
    Public ClientId As String
    Public IsErr As Boolean = False
    Public ErrExceptionDetail As String
    Public ErrDetail As String
    Public IsUpdated As Boolean = False

    Public Sub New()

    End Sub

End Class

'Public Class ManageCommandSql1
'    Inherits ManageCommandSql

'    Sub New(ByVal Base As EnDatabaseType)

'    End Sub

'    Public Function Hello() As String

'    End Function

'End Class


