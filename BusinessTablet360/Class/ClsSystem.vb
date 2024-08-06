Imports KnowledgeUtils.Database.SqlServer
Imports KnowledgeUtils.Encode.ManageEncode
Imports System.Web
Imports KnowledgeUtils

Namespace Service

    Public Class ClsSystem

        Private Property dtLevels As DataTable
        Private Property dtGroupSubjects As DataTable

        Shared _DB As ClsConnect
        Public Sub New(ByVal DB As ClsConnect)
            _DB = DB
        End Sub

        Public Shared Function GetConfig(ByVal keyName As String) As String
            Dim ConfigStr As String = ClsLanguage.GetConfigStr()
            'นำข้อมูลมาถอดรหัสออก
            Dim keyDecryptStr As String = KNConfigData.DecryptData(ConfigStr)
            'splite ข้อมูล Key ออกจากกันด้วย |
            Dim objKnConfig = keyDecryptStr.Split("|")
            'ตัวแปรที่จะเอามาเทียบค่า Key ของทุกๆ Key ใน Config
            Dim objEachKey = Nothing
            For Each k In objKnConfig
                objEachKey = k.ToString().Split("*")
                If objEachKey(0).ToLower() = keyName.ToLower() Then
                    Return objEachKey(1)
                End If
            Next
            Return ""
        End Function

        Private Sub InitialData()
            'set ค่าให้ datatable เป็น ข้อมูลจาก tbllevel
            Dim sql As String = "SELECT * FROM tblLevel;"
            dtLevels = _DB.getdata(sql)

            'set ค่าให้ datatable เป็น ข้อมูลจาก tblGroupSubject
            sql = "  SELECT * FROM tblGroupSubject;"
            dtGroupSubjects = _DB.getdata(sql)

            ' เติม 1 column ให้กับ dtGroupSubjects เป็น SubjectId จาก tblSubject
            Dim colSubjectId As New DataColumn("SubjectId", GetType(Integer))
            dtGroupSubjects.Columns.Add(colSubjectId)

            ' set ค่าตามชื่อวิชา
            For Each subject In dtGroupSubjects.Rows
                subject("SubjectId") = GetSubjectIdFromSubjectName(subject("GroupSubject_ShortName"))
            Next
        End Sub

        Private Function GetSubjectIdFromSubjectName(subjectName As String) As Integer
            Select Case subjectName
                Case "ไทย", "thai"
                    Return SubjectId.thai
                Case "ศิลปะ", "art"
                    Return SubjectId.art
                Case "การงานฯ", "home", "career"
                    Return SubjectId.home
                Case "สุขศึกษาฯ", "health"
                    Return SubjectId.health
                Case "อังกฤษ", "english", "eng"
                    Return SubjectId.english
                Case "สังคมฯ", "social"
                    Return SubjectId.social
                Case "วิทย์ฯ", "science"
                    Return SubjectId.science
                Case "PISA", "pisa"
                    Return SubjectId.pisa
                Case "คณิตฯ", "math"
                    Return SubjectId.math
            End Select
            Return 0
        End Function


        Public Function GetCalendarId(ByVal SchoolId As String) As String

            Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                                " AND Calendar_Type = 3 AND School_Code = '" & SchoolId.CleanSQL & "' AND IsActive=1;"
            Dim db As New ClassConnectSql()
            Dim CalendarId As String = db.ExecuteScalar(sql)
            If CalendarId = "" Then
                sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE Calendar_Type = '3' AND School_Code = '" & SchoolId.CleanSQL & "' " &
                      " AND dbo.GetThaiDate() >= Calendar_ToDate AND IsActive = 1 ORDER BY Calendar_ToDate DESC; "
                CalendarId = _DB.ExecuteScalar(sql)
                If CalendarId = "" Then
                    Return ""
                Else
                    Return CalendarId
                End If
            End If
            Return CalendarId

        End Function

        Public Function GetCalendarIdReal(ByVal SchoolId As String) As String
            Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                                " AND Calendar_Type = 3 AND School_Code = '" & SchoolId & "' AND IsActive=1;"
            Dim db As New ClassConnectSql()
            Dim CalendarId As String = db.ExecuteScalar(sql)
            If CalendarId = "" Then
                Return ""
            Else
                Return CalendarId
            End If

        End Function

        ''' <summary>
        ''' เช็คว่า calendar ที่ได้ใช่เทอมปัจจุบันเปล่า
        ''' </summary>
        ''' <param name="SchoolId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsCalendarCurrent(ByVal SchoolId As String) As Boolean

            Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                                " AND Calendar_Type = 3 AND School_Code = '" & SchoolId.CleanSQL & "' AND IsActive=1;"
            Dim db As New ClassConnectSql()
            Dim CalendarId As String = db.ExecuteScalar(sql)
            Return CalendarId <> ""
        End Function

        Public Function GetTableByLastUpdate(ByVal TableName As String, Optional ByVal LastUpdate As Date? = Nothing, Optional ByVal EndLastUpdate As Date? = Nothing, Optional ByVal AddFilter As String = "") As DataTable
            Dim sql As String
            If LastUpdate Is Nothing AndAlso AddFilter = "" Then
                sql = "select *  from " & TableName.CleanSQL
            Else
                If EndLastUpdate Is Nothing Then
                    sql = "select * from " & TableName.CleanSQL & " where (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "')"
                Else
                    sql = "select TOP 300000 * from " & TableName.CleanSQL & " where (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "' AND LastUpdate <= '" & ManageSqlServer.ConvertDateTimeToString(EndLastUpdate) & "')"
                End If
                If AddFilter <> "" Then
                    sql &= " AND (" & AddFilter & ")"
                End If
            End If

            Dim dt As DataTable = _DB.getdataNotDataSet(sql, TableName)
            Return dt
        End Function

        Public Function GetTableByLastUpdateForServer(ByVal TableName As String, SchoolCode As String, Optional ByVal LastUpdate As Date? = Nothing, Optional ByVal EndLastUpdate As Date? = Nothing, Optional ByVal AddFilter As String = "") As DataTable
            Dim sql As String
            If LastUpdate Is Nothing AndAlso AddFilter = "" Then
                sql = "select *  from " & TableName.CleanSQL
            Else
                If EndLastUpdate Is Nothing Then
                    sql = "select * from " & TableName.CleanSQL & " where  (ClientId is null) AND (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "')"
                Else
                    sql = "select TOP 300000 * from " & TableName.CleanSQL & " where  (ClientId is null) AND (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "' AND LastUpdate <= '" & ManageSqlServer.ConvertDateTimeToString(EndLastUpdate) & "')"
                End If
                If AddFilter <> "" Then
                    sql &= " AND (" & AddFilter & ")"
                End If
                If SchoolCode <> "" Then
                    sql &= " AND (" & SchoolCode & ")"
                End If
            End If

            Dim dt As DataTable = _DB.getdataNotDataSet(sql, TableName)
            Return dt
        End Function

        Public Function GetTableByLastUpdateForClient(ByVal TableName As String, SchoolCode As String, Optional ByVal LastUpdate As Date? = Nothing, Optional ByVal EndLastUpdate As Date? = Nothing, Optional ByVal AddFilter As String = "") As DataTable
            Dim sql As String
            If LastUpdate Is Nothing AndAlso AddFilter = "" Then
                sql = "select *  from " & TableName
            Else
                If EndLastUpdate Is Nothing Then
                    sql = "select * from " & TableName.CleanSQL & " where  (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "')"
                Else
                    sql = "select TOP 300000 * from " & TableName.CleanSQL & " where (ClientId is null) AND  (LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "' AND LastUpdate <= '" & ManageSqlServer.ConvertDateTimeToString(EndLastUpdate) & "')"
                End If
                If AddFilter <> "" Then
                    sql &= " AND (" & AddFilter & ")"
                End If
                If SchoolCode <> "" Then
                    sql &= " AND (" & SchoolCode & ")"
                End If
            End If

            Dim dt As DataTable = _DB.getdataNotDataSet(sql, TableName)
            Return dt
        End Function

        Public Function GetThaiDate() As Date
            Dim sql As String
            sql = "select dbo.GetThaiDate()"

            Dim dt As DataTable = _DB.getdataNotDataSet(sql)
            Return CType(dt.Rows(0)(0), Date)
        End Function

        Public Sub DeleteData(ByVal TableName As String)
            Dim sql As String
            sql = "truncate table " & TableName.CleanSQL
            _DB.Execute(sql)
        End Sub

        Public Sub RenameTableUnderScore(ByVal TableName As String)
            Dim sql As String
            Dim ChangeName = TableName & "_"
            sql = "sp_rename " & TableName.CleanSQL & "," & ChangeName.CleanSQL
            _DB.Execute(sql)
        End Sub

        Public Sub AlterClientIdColumn(ByVal TableName As String)
            Dim sql As String
            sql = "ALTER TABLE " & TableName.CleanSQL & " ADD ClientId	varchar(50) NULL"
            _DB.Execute(sql)
        End Sub

        Public Sub AlterEditClientIdColumn(ByVal TableName As String)
            Dim sql As String
            sql = "ALTER TABLE " & TableName.CleanSQL & " MODIFY ClientId varchar(50)"
            _DB.Execute(sql)
        End Sub

        Public Sub AlterEditLastUpdateColumn(ByVal TableName As String)
            Dim sql As String
            sql = "ALTER TABLE " & TableName.CleanSQL & " alter column LastUpdate datetime"
            _DB.Execute(sql)
        End Sub

        Public Function GetTableWithSchemaByLastUpdate(ByVal TableName As String, Optional ByVal LastUpdate As Date? = Nothing) As DataTable
            Dim sql As String
            If LastUpdate Is Nothing Then
                sql = "select *  from " & TableName
            Else
                sql = "select *  from " & TableName & " where LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "'"
            End If

            Dim dt As DataTable = _DB.getdataWithSchema(sql, TableName)
            Return dt
        End Function

        Public Function GetTableAnswerByLastUpdate(ByVal LastUpdate As Date) As DataTable
            Dim sql As String
            sql = "select top 500 Answer_Id, Question_Id, Efficiency_Id, QSet_Id, Answer_No, convert(varchar(1000), Answer_Name) AS Answer_Name, Answer_Score, Answer_ScoreMinus, Answer_Position, IsActive, " &
                   " LastUpdate  from tblAnswer where LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "'"
            Dim dt As DataTable = _DB.getdata(sql)
            Return dt
        End Function

        Public Function GetTableQuestionByLastUpdate(ByVal LastUpdate As Date) As DataTable
            Dim sql As String
            sql = "SELECT  Question_Id, QSet_Id, EfficiencySet_Id, IntroQset_Id, Question_No,convert(varchar(8000), Question_Name) AS Question_Name , convert(varchar(8000), Question_Expain) AS Question_Expain, IsActive, QId,convert(varchar(8000), Question_Name_Backup) AS Question_Name_Backup, LastUpdate " &
                  "FROM  tblQuestion where LastUpdate > '" & ManageSqlServer.ConvertDateTimeToString(LastUpdate) & "'"
            Dim dt As DataTable = _DB.getdata(sql)
            Return dt
        End Function

        Public Function KNClearDB(ByVal IsClearAll As Boolean) As Boolean
            Try
                InitialData()

                _DB.OpenWithTransection()

                ' schoolid จาก language
                Dim schoolId As String = GetConfig("DefaultSchoolCode")

                'Truncate table ที่ไม่มี Iswpp ก่อน
                Dim ArrTableName As ArrayList = GetArrTableNameToDeleteData()

                ' เอา table ที่เกียวกับการจัดชุดไว้ เก็บไว้ไม่ต้องลบ ถ้า user ไม่ต้องการ ลบทั้งหมด
                If Not IsClearAll Then
                    ArrTableName.Remove("t360_tblTeacher")
                    ArrTableName.Remove("t360_tblTeacherRoom")
                    ArrTableName.Remove("tblAssistant")
                    ArrTableName.Remove("tblUserSubjectClass")
                    ArrTableName.Remove("tblTestSet")
                    ArrTableName.Remove("tblTestSetQuestionDetail")
                    ArrTableName.Remove("tblTestSetQuestionSet")
                End If

                Dim sql As String = ""
                If ArrTableName.Count > 0 Then
                    For Each tableName As String In ArrTableName
                        sql &= " TRUNCATE TABLE dbo." & tableName & "; "
                    Next
                    _DB.ExecuteWithTransection(sql)

                    If IsClearAll Then ' ถ้าลบทั้งหมด 
                        'ลบ tbluser หมดเลย เด๋ว insert ใหม่ ด้านล่าง
                        sql = " DELETE dbo.tblUser; "
                        _DB.ExecuteWithTransection(sql)
                    End If


                    'tbl ที่มี Iswpp ให้ลบเฉพาะที่ Iswpp = 0
                    Dim ArrTableNameHaveIsWpp As ArrayList = GetArrTableNameHaveIsWpp()
                    If ArrTableName.Count > 0 Then
                        sql = ""
                        ' 27/10/58 พี่ชินบอกว่ายังไม่ให้ลบ เพราะตอนนี้เป็น bug จากฝั่งวิชาการ
                        'For Each tablenameHaveIswpp As String In ArrTableNameHaveIsWpp
                        '    sql &= " DELETE dbo." & tablenameHaveIswpp & " WHERE IsWpp = 0; "
                        'Next
                        '_DB.ExecuteWithTransection(sql)

                        'insert calendar 10 years
                        sql = " INSERT INTO t360_tblCalendar VALUES (NEWID(),'" & schoolId.CleanSQL & "','2558','เทอม 1',dbo.GetThaiDate(),DATEADD(YEAR,10,dbo.GetThaiDate()),3,dbo.GetThaiDate(),1,NULL); "
                        _DB.ExecuteWithTransection(sql)

                        't360_tblSchoolClass พี่ชินบอกไม่ต้องยุ่ง เพราะเราไม่มีส่วนไปโดนกับพวกนี้อยู่แล้ว
                        'sql = " INSERT INTO t360_tblSchoolClass VALUES(NEWID(),'schoolcode','classname',dbo.GetThaiDate(),1,NULL); "

                        'insert admin t360 t360_tbluser
                        sql = " DECLARE @Usert360 AS uniqueidentifier = NEWID();"
                        sql &= " INSERT INTO t360_tblUser VALUES('" & schoolId.CleanSQL & "',@Usert360,'admin','" & Encode("network", "qAk") & "','qAk','admin','admin','0123456789','admin@admin.com',1,dbo.GetThaiDate(),NULL); "
                        'insert t360_tblUserMenuItem, 
                        sql &= " INSERT INTO t360_tblUserMenuItem SELECT '" & schoolId.CleanSQL & "',MenuItem_Code,@Usert360,dbo.GetThaiDate(),1,NEWID(),NULL FROM t360_tblMenuItem;"
                        _DB.ExecuteWithTransection(sql)


                        If IsClearAll Then
                            'insert user pointplus
                            'insert usersubject class
                            'insert teacher
                            'insert assistant
                            Dim sqlAddUsers As New System.Text.StringBuilder()
                            Dim Users As New List(Of String)({"admin", "thai", "art", "home", "english", "social", "science", "math", "health"})
                            Dim i As Integer = 1
                            For Each user In Users
                                Dim userDeclare As String = String.Format("@{0}", user)
                                Dim pwd As String = If(user = "admin", Encryption.MD5("network"), Encryption.MD5("1234"))
                                sqlAddUsers.Append(String.Format("DECLARE {0} AS uniqueidentifier = NEWID();", userDeclare.CleanSQL))
                                sqlAddUsers.Append(String.Format("INSERT INTO tblUser VALUES ({0},'{1}','{1}','{1}','{2}','{3}',1,dbo.GetThaiDate(),NULL,{4},1,1,1,1,1,0);", i, user.CleanSQL, pwd.CleanSQL, schoolId.CleanSQL, userDeclare.CleanSQL))
                                sqlAddUsers.Append(String.Format("INSERT INTO t360_tblTeacher VALUES ({0},'{1}',{2},'ครู','{3}','{3}','0123456789',1,NULL,NULL,NULL,NULL,NULL,7984,318,1,1,dbo.GetThaiDate(),NULL);", userDeclare.CleanSQL, schoolId.CleanSQL, i, user.CleanSQL))
                                sqlAddUsers.Append(String.Format(" INSERT INTO tblAssistant VALUES (NEWID(),{0},{0},dbo.GetThaiDate(),1,NULL);", userDeclare.CleanSQL))
                                i = i + 1
                            Next
                            sqlAddUsers.Append(SQLInsertUserSubjectClass())
                            _DB.ExecuteWithTransection(sqlAddUsers.ToString())

                            'Insert New User เข้าไป อิงตาม KNConfig ว่าใช้ชั้นไหน,วิชาอะไรได้บ้าง
                            'InsertNewUserAfterDeleteData(_DB)
                        End If


                        _DB.CommitTransection()
                        _DB = Nothing
                        Return True
                    Else
                        _DB.RollbackTransection()
                        _DB = Nothing
                        Return False
                    End If
                Else
                    _DB.RollbackTransection()
                    _DB = Nothing
                    Return False
                End If
            Catch ex As Exception
                _DB.RollbackTransection()
                _DB = Nothing
                ElmahExtension.LogToElmah(ex)
                Return False
            End Try
        End Function

        Public Shared Function GetArrTableNameToDeleteData() As ArrayList
            't360_tblSchoolClass, ตัดทิ้งออกไปก่อน พี่ชินบอกไม่ต้องยุ่ง เพราะเราไม่มีส่วนไปโดนกับพวกนี้อยู่แล้ว เพราะเราไม่รู้จะ insert class ไหนบ้าง
            Dim StrtableNameToDelete As String = "t360_tblCalendar,t360_tblLog,t360_tblNetwork,t360_tblNetworkHistory,t360_tblNews,t360_tblNewsRoom,t360_tblRoom," &
                                                 "t360_tblSchool,t360_tblSetTypeRunStudentNumber,t360_tblStudent,t360_tblStudentCheckName," &
                                                 "t360_tblStudentFinish,t360_tblStudentRoom,t360_tblTablet,t360_tblTabletLog,t360_tblTabletLost,t360_tblTabletOwner," &
                                                 "t360_tblTabletRepair,t360_tblTeacher,t360_tblTeacherRoom,t360_tblTempStudent,t360_tblTempTeacher,t360_tblUpLevel,t360_tblUplevelConfirm,t360_tblUpLevelDetail," &
                                                 "t360_tblUser,t360_tblUserMenuItem,tblAssistant,tblAvatarComment,tblChatJoin,tblChatMessage,tblChatRecipient," &
                                                 "tblChatRoom,tblContact,tblLog,tblMobileAccessPassword,tblMobileRegistration,tblModule,tblModuleAssignment," &
                                                 "tblExternalApp,tblExternalAppAnswer,tblExternalAppQuestion,tblExternalAppStation,tblExternalLogAction,tblExternalLogApp,tblExternalLogAppStation,tblLayoutConfirmed," &
                                                 "tblModuleAssignmentDetail,tblModuleDetail,tblModuleDetailCompletion,tblNote,tblParent,tblQuestionSetPassword,tblQuiz," &
                                                 "tblQuizAnswer,tblQuizCommand,tblQuizQuestion,tblQuizScore,tblQuizSession,tblRequestParentApproval,tblRequestParentApprovalDetail," &
                                                 "tblSetEmail,tblStudent,tblStudentItem,tblStudentParent,tblStudentPhoto,tblStudentPoint,tblSyncLog,tblTabletLab,tblTabletLabDesk," &
                                                 "tblTeacherFavorite,tblTeacherNews,tblTeacherNewsDetail,tblTeacherNewsDetailCompletion,tblTestSet,tblTestSet_CM_temptblsetup," &
                                                 "tblTestSetQuestionDetail,tblTestSetQuestionSet,tblTestSetWinner,tblUserSetting,tblUserSubjectClass,tblUserViewPageWithTip,tmp_GetcurrentStatus,tblGenData,tblQC_User"
            Dim SpliteTableName = StrtableNameToDelete.Split(",")
            Dim ArrTableName As New ArrayList
            For Each tableName As String In SpliteTableName
                ArrTableName.Add(tableName.ToString())
            Next
            Return ArrTableName
        End Function

        Public Shared Function GetArrTableNameHaveIsWpp() As ArrayList            ' 
            Dim StrTableNameIsWpp As String = "tblAnswer,tblBook,tblQuestionset,tblQuestionCategory,tblQuestion,tblEvaluationIndexLevel,tblEvaluationIndexNew," &
                                              "tblEvaluationIndexSubject,tblIntro,tblIntroQuestionSet,tblIntroQuestionSetQuestion,tblQuestionEvaluationIndexItem," &
                                              "tblWordBook"
            Dim ArrTableName As New ArrayList
            Dim SpliteStr = StrTableNameIsWpp.Split(",")
            For Each tableName As String In SpliteStr
                ArrTableName.Add(tableName.ToString())
            Next
            Return ArrTableName
        End Function

        ''' <summary>
        ''' ทำการ Insert User Default ที่มาจาก โรงงาน โดยให้ใช้ ชั้น , วิชา ตามที่ config มา
        ''' </summary>
        ''' <param name="ObjConn">ตัวแปร Connection String</param>
        ''' <remarks></remarks>
        Public Shared Sub InsertNewUserAfterDeleteData(Optional ByRef ObjConn As ClassConnectSql = Nothing)
            'ตัวแปรที่ไว้เช็คว่าใช้ Transaction หรือเปล่า
            Dim Flag As Boolean = False
            Try
                If ObjConn Is Nothing Then
                    ObjConn = New ClassConnectSql()
                    ObjConn.OpenWithTransection()
                    Flag = True
                End If

                Dim ConfigStr As String = ClsLanguage.GetConfigStr()
                'ทำการ Get ค่าที่บอกว่าให้ใช้ ชั้น , วิชาไหนได้บ้าง จาก application
                Dim EnableUserSubjectClass As String = GetConfigEnableUserSubjectClass(ConfigStr)
                If EnableUserSubjectClass <> "" Then
                    'Get UserId Default ขึ้นมาก่อน
                    Dim sql As String = " SELECT GUID FROM dbo.tblUser WHERE UserName = 'nobeladmin' AND Password = '" & Encryption.MD5("tiger") & "' "
                    Dim UserId As String = ObjConn.ExecuteScalarWithTransection(sql)
                    If UserId <> "" Then
                        'Insert tblUserSubjectClass
                        Dim ArrSpliteObj As ArrayList = SpliteObjectUserSubjectClass(EnableUserSubjectClass)
                        If ArrSpliteObj.Count > 0 Then
                            'loop เพื่อดึงขึ้นมาทีละ ชั้น/วิชา เพื่อทำการ insert UserSubjectClass , เงื่อนไขการจบ loop คือ วนจนครบทุก Item ใน Array
                            For Each r In ArrSpliteObj
                                'ทำการ split อีกทีนึง คราวนี้จะได้เป็น ชั้น และ วิชาแล้ว
                                Dim SplitEachItem = r.ToString.Split("-")
                                Dim SubjectName As String = SplitEachItem(0).ToString().Trim()
                                Dim LevelName As String = SplitEachItem(1).ToString().Trim()
                                Dim GroupSubjectId As ArrayList = GetGroupSubjectIdBySubjectName(SubjectName, ObjConn)
                                Dim LevelId As ArrayList = GetLevelIdByLevelName(LevelName, ObjConn)
                                'เมื่อได้ข้อมูลครบก็ทำการ Insert ชั้น , วิชา , User นี้ ที่ tblUserSubjectClass
                                If GroupSubjectId(0).ToString().Trim() <> "" And GroupSubjectId(1).ToString().Trim() <> "" And LevelId(0).ToString.Trim() <> "" And LevelId(1).ToString.Trim() Then
                                    sql = " INSERT INTO dbo.tblUserSubjectClass ( USCId ,UserIdOld ,Detailid ,SubjectId ,ClassId ,GroupSubjectId ,LevelId ,IsActive " &
                                          " ,LastUpdate ,ClientId ,GUID ,UserId ) SELECT (COUNT(*) + 1) , 1 , 1 , '" & GroupSubjectId(1).ToString().Trim().CleanSQL & "' ,'" & LevelId(1).ToString().Trim().CleanSQL & "' , " &
                                          " '" & GroupSubjectId(0).ToString().Trim().CleanSQL & "' ,'" & LevelId(0).ToString.Trim().CleanSQL & "' , 1 , dbo.GetThaiDate() ,0 ,NEWID() ,'" & UserId.CleanSQL & "' FROM dbo.tblUserSubjectClass "
                                    ObjConn.ExecuteWithTransection(sql)
                                End If
                            Next
                        End If
                    End If
                    If Flag = True Then
                        ObjConn.CommitTransection()
                        ObjConn = Nothing
                    End If
                End If
            Catch ex As Exception
                If Flag = True Then
                    ObjConn.RollbackTransection()
                End If
                ElmahExtension.LogToElmah(ex)
            End Try
        End Sub


        Private Function SQLInsertUserSubjectClass() As String

            Dim EnableUserSubjectClass As String = GetConfig("enableusersubjectclass") ' GetConfigEnableUserSubjectClass("")
            Dim arrSubject As Array = EnableUserSubjectClass.Split(",")

            Dim sql As New System.Text.StringBuilder()

            Dim i As Integer = 1
            For Each subjectAndClass In arrSubject
                Dim arr As Array = subjectAndClass.ToString().Split("-")
                Dim subjectEngName As String = arr(0)
                If subjectEngName = "career" Then
                    subjectEngName = "home"
                ElseIf subjectEngName = "eng" Then
                    subjectEngName = "english"
                End If
                Dim subjectId As Integer = GetSubjectIdFromSubjectName(subjectEngName)
                Dim classId As String = arr(1).ToString().Replace("k", "").Replace("K", "")

                Dim levelRow As DataRow = dtLevels.AsEnumerable().Where(Function(t) t.Field(Of Integer)("Level") = classId).SingleOrDefault()
                Dim subjectRow As DataRow = dtGroupSubjects.AsEnumerable().Where(Function(t) t.Field(Of Integer)("SubjectId") = subjectId).SingleOrDefault()

                sql.Append(String.Format("INSERT INTO tblUserSubjectClass VALUES({0},1,1,'{1}','{2}','{3}','{4}',1,dbo.GetThaiDate(),NULL,NEWID(),@admin);", i, subjectRow("SubjectId"), classId.CleanSQL, subjectRow("GroupSubject_Id"), levelRow("Level_Id")))
                i = i + 1
                sql.Append(String.Format("INSERT INTO tblUserSubjectClass VALUES({0},1,1,'{1}','{2}','{3}','{4}',1,dbo.GetThaiDate(),NULL,NEWID(),@{5});", i, subjectRow("SubjectId"), classId.CleanSQL, subjectRow("GroupSubject_Id"), levelRow("Level_Id"), subjectEngName.CleanSQL))
                i = i + 1
            Next

            Return sql.ToString()
        End Function

        ''' <summary>
        ''' ทำการนำค่า config มา split ค่าแล้วทำการยัดใส่ Array เพื่อคืนค่ากลับไปให้ Insert ข้อมูล
        ''' </summary>
        ''' <param name="StrConfig">Config</param>
        ''' <returns>ArrayList:ที่มีชั้น/วิชา ที่สามารถใช้ได้</returns>
        ''' <remarks></remarks>
        Private Shared Function SpliteObjectUserSubjectClass(ByVal StrConfig As String) As ArrayList
            Dim ArrSplite As New ArrayList
            If StrConfig <> "" Then
                Dim SpliteStr = StrConfig.Split(",")
                If SpliteStr.Count > 0 Then
                    'loop เพื่อทำการ Add ทีละค่าเข้าไปใน Array , เงื่อนไขการจบ loop คือวนจนครบหมดใน config
                    For Each r In SpliteStr
                        ArrSplite.Add(r)
                    Next
                    Return ArrSplite
                Else
                    Return ArrSplite
                End If
            Else
                Return ArrSplite
            End If
        End Function

        ''' <summary>
        ''' นำชื่อวิชาที่ได้จากค่า config มาหาว่า GroupsubjectId,SubjectNumber
        ''' </summary>
        ''' <param name="SubjectName">ชื่อวิชาที่ได้จากค่า config</param>
        ''' <param name="ObjDB">ตัวแปร connection</param>
        ''' <returns>ArrayList:ที่มี GroupSubjectId,SubjectNumber</returns>
        ''' <remarks></remarks>
        Public Shared Function GetGroupSubjectIdBySubjectName(ByVal SubjectName As String, ByRef ObjDB As ClassConnectSql) As ArrayList
            Dim ArrReturn As New ArrayList
            Dim SubjectNumber As Integer = 0
            If SubjectName IsNot Nothing And SubjectName.Trim() <> "" Then
                Dim GroupSubjectShortName As String = ""
                If SubjectName.ToLower().Trim() = "thai" Then
                    GroupSubjectShortName = "ภาษาไทย"
                    SubjectNumber = 1
                ElseIf SubjectName.ToLower().Trim() = "art" Then
                    GroupSubjectShortName = "ศิลปะ"
                    SubjectNumber = 7
                ElseIf SubjectName.ToLower().Trim() = "career" Then
                    GroupSubjectShortName = "การงานฯ"
                    SubjectNumber = 8
                ElseIf SubjectName.ToLower().Trim() = "health" Then
                    GroupSubjectShortName = "สุขศึกษา/พละฯ"
                    SubjectNumber = 6
                ElseIf SubjectName.ToLower().Trim() = "eng" Then
                    GroupSubjectShortName = "ภาษาตปท."
                    SubjectNumber = 5
                ElseIf SubjectName.ToLower().Trim() = "social" Then
                    GroupSubjectShortName = "สังคมฯ"
                    SubjectNumber = 2
                ElseIf SubjectName.ToLower().Trim() = "science" Then
                    GroupSubjectShortName = "วิทย์ฯ"
                    SubjectNumber = 4
                ElseIf SubjectName.ToLower().Trim() = "math" Then
                    GroupSubjectShortName = "คณิตฯ"
                    SubjectNumber = 3
                End If
                Dim sql As String = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = '" & GroupSubjectShortName.CleanSQL & "' AND IsActive = 1 "
                Dim GroupSubjectId As String = ObjDB.ExecuteScalarWithTransection(sql)
                ArrReturn.Add(GroupSubjectId)
                ArrReturn.Add(SubjectNumber)
                Return ArrReturn
            Else
                Return ArrReturn
            End If
        End Function

        Public Shared Function GetSubjectMapbyValue(Optional ByVal SubjectNumber As Integer = 0,
                                                 Optional GroupSubjectShortName As String = "",
                                                 Optional SubjectNameDummy As String = "") As SubjectDTO
            Dim AllSubMap = GetAllSubjectMap()
            If SubjectNumber > 0 Then
                Return AllSubMap.Where(Function(q) q.SubjectNumber = SubjectNumber).SingleOrDefault
            End If
            If GroupSubjectShortName <> "" Then
                Return AllSubMap.Where(Function(q) q.GroupSubjectShortName = GroupSubjectShortName).SingleOrDefault
            End If
            If SubjectNameDummy <> "" Then
                Return AllSubMap.Where(Function(q) q.GroupSubjectShortName = SubjectNameDummy).SingleOrDefault
            End If
        End Function

        Public Shared Function GetAllSubjectMap() As SubjectDTO()
            Dim Tmp As New List(Of SubjectDTO)
            Dim Dt As New DataTable
            Dim Sql As String

            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'ไทย' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 1,
                                         .SubjectNameDummy = "thai",
                                         .GroupSubjectShortName = "ไทย",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'สังคมฯ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 2,
                                         .SubjectNameDummy = "social",
                                         .GroupSubjectShortName = "สังคมฯ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'คณิตฯ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 3,
                                         .SubjectNameDummy = "math",
                                         .GroupSubjectShortName = "คณิตฯ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'วิทย์ฯ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 4,
                                        .SubjectNameDummy = "science",
                                        .GroupSubjectShortName = "วิทย์ฯ",
                                        .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'อังกฤษ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 5,
                                        .SubjectNameDummy = "eng",
                                        .GroupSubjectShortName = "อังกฤษ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'สุขศึกษาฯ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 6,
                                        .SubjectNameDummy = "health",
                                        .GroupSubjectShortName = "สุขศึกษาฯ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'ศิลปะ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 7,
                                       .SubjectNameDummy = "art",
                                       .GroupSubjectShortName = "ศิลปะ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'การงานฯ' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 8,
                                       .SubjectNameDummy = "career",
                                       .GroupSubjectShortName = "การงานฯ",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})
            Sql = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = 'PISA' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New SubjectDTO With {.SubjectNumber = 9,
                                       .SubjectNameDummy = "pisa",
                                       .GroupSubjectShortName = "PISA",
                                         .GroupSubject_Id = New Guid(Dt.Rows(0)("GroupSubject_Id").ToString)})

            Return Tmp.ToArray
        End Function

        ''' <summary>
        ''' ทำการนำค่า ชื่อชั้นที่ได้มาจาก config เข้ามาหา LevelId,LevelNumber
        ''' </summary>
        ''' <param name="LevelName">ชื่อชั้นจาก config</param>
        ''' <param name="ObjDB">ตัวแปร connection</param>
        ''' <returns>ArrayList:LevelId,LevelNumber</returns>
        ''' <remarks></remarks>
        Public Shared Function GetLevelIdByLevelName(ByVal LevelName As String, ByRef ObjDB As ClassConnectSql) As ArrayList
            Dim ArrReturn As New ArrayList
            If LevelName IsNot Nothing And LevelName.Trim() <> "" Then
                Dim LevelShortName As String = ""
                Dim LevelNumber As Integer = 0
                If LevelName.Trim().ToUpper() = "K1" Then
                    LevelShortName = "อ.1"
                    LevelNumber = 1
                ElseIf LevelName.Trim().ToUpper() = "K2" Then
                    LevelShortName = "อ.2"
                    LevelNumber = 2
                ElseIf LevelName.Trim().ToUpper() = "K3" Then
                    LevelShortName = "อ.3"
                    LevelNumber = 3
                ElseIf LevelName.Trim().ToUpper() = "K4" Then
                    LevelShortName = "ป.1"
                    LevelNumber = 4
                ElseIf LevelName.Trim().ToUpper() = "K5" Then
                    LevelShortName = "ป.2"
                    LevelNumber = 5
                ElseIf LevelName.Trim().ToUpper() = "K6" Then
                    LevelShortName = "ป.3"
                    LevelNumber = 6
                ElseIf LevelName.Trim().ToUpper() = "K7" Then
                    LevelShortName = "ป.4"
                    LevelNumber = 7
                ElseIf LevelName.Trim().ToUpper() = "K8" Then
                    LevelShortName = "ป.5"
                    LevelNumber = 8
                ElseIf LevelName.Trim().ToUpper() = "K9" Then
                    LevelShortName = "ป.6"
                    LevelNumber = 9
                ElseIf LevelName.Trim().ToUpper() = "K10" Then
                    LevelShortName = "ม.1"
                    LevelNumber = 10
                ElseIf LevelName.Trim().ToUpper() = "K11" Then
                    LevelShortName = "ม.2"
                    LevelNumber = 11
                ElseIf LevelName.Trim().ToUpper() = "K12" Then
                    LevelShortName = "ม.3"
                    LevelNumber = 12
                ElseIf LevelName.Trim().ToUpper() = "K13" Then
                    LevelShortName = "ม.4"
                    LevelNumber = 13
                ElseIf LevelName.Trim().ToUpper() = "K14" Then
                    LevelShortName = "ม.5"
                    LevelNumber = 14
                ElseIf LevelName.Trim().ToUpper() = "K15" Then
                    LevelShortName = "ม.6"
                    LevelNumber = 15
                End If
                Dim sql As String = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = '" & LevelShortName.CleanSQL & "' AND IsActive = 1 "
                Dim LevelId As String = ObjDB.ExecuteScalarWithTransection(sql)
                ArrReturn.Add(LevelId)
                ArrReturn.Add(LevelNumber)
                Return ArrReturn
            Else
                Return ArrReturn
            End If
        End Function

        Public Shared Function GetLevelMapbyValue(Optional Level As String = "",
                                                  Optional Level_Name As String = "",
                                                  Optional Level_Dummy As String = "") As ClassDto
            Dim AllLevelMap = GetAllLevelMap()
            If Level <> "" Then
                Return AllLevelMap.Where(Function(q) q.Level = Level).SingleOrDefault
            End If
            If Level_Name <> "" Then
                Return AllLevelMap.Where(Function(q) q.Level_Name = Level_Name).SingleOrDefault
            End If
            If Level_Dummy <> "" Then
                Return AllLevelMap.Where(Function(q) q.Level_Name.ToUpper = Level_Dummy.ToUpper).SingleOrDefault
            End If
        End Function

        Public Shared Function GetAllLevelMap() As ClassDto()
            Dim Tmp As New List(Of ClassDto)
            Dim Dt As New DataTable
            Dim Sql As String

            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'อ.1' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 1,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "อ.1",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'อ.2' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 2,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "อ.2",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'อ.3' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 3,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "อ.3",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.1' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 4,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.1",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.2' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 5,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.2",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.3' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 6,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.3",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.4' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 7,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.4",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.5' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 8,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.5",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ป.6' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 9,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ป.6",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.1' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 10,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.1",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.2' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 11,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.2",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.3' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 12,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.3",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.4' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 13,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.4",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.5' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 14,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.5",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})
            Sql = " SELECT Level_Id FROM dbo.tblLevel WHERE Level_ShortName = 'ม.6' AND IsActive = 1 "
            Dt = _DB.getdata(Sql)
            Tmp.Add(New ClassDto With {.Level = 15,
                                       .Level_Dummy = "K" & .Level.ToString,
                                       .Level_Name = "ม.6",
                                       .Level_Id = New Guid(Dt.Rows(0)("Level_Id").ToString)})

            Return Tmp.ToArray
        End Function

        ''' <summary>
        ''' ทำการ split ค่าเพื่อดึงค่า Config ชั้น,วิชา ที่ให้ รร. นี้ใช้ได้
        ''' </summary>
        ''' <param name="InputKNConfig">ค่า config</param>
        ''' <returns>String:สตริงที่บอกว่าสามารถใช้ ชั้น,วิชาอะไรได้บ้าง</returns>
        ''' <remarks></remarks>
        Public Shared Function GetConfigEnableUserSubjectClass(ByVal InputKNConfig As String) As String
            'นำข้อมูลมาถอดรหัสออก
            InputKNConfig = KNConfigData.DecryptData(InputKNConfig)
            'splite ข้อมูล Key ออกจากกันด้วย |
            Dim objKnConfig = InputKNConfig.Split("|")
            'ตัวแปรที่จะเอามาเทียบค่า Key ของทุกๆ Key ใน Config
            Dim objEachKey = Nothing
            'สตริงที่จะทำการ Return ไป
            Dim EnableUserSubjectClassStr As String = ""
            'loop เพื่อหาค่า config enableusersubjectclass , เงื่อนไขการจบ loop คือ วน check Keyname จนกว่าจะเจอ enableusersubjectclass ถึงจะจบ loop
            For Each r In objKnConfig
                objEachKey = r.ToString().Split("*")
                Dim Eachkey As String = objEachKey(0)
                If Eachkey.ToLower() = "enableusersubjectclass" Then
                    EnableUserSubjectClassStr = objEachKey(1)
                    Exit For
                End If
            Next
            Return EnableUserSubjectClassStr
        End Function

        'Function เช็คว่า Key ที่ใส่เข้ามาถูกหรือเปล่า
        Public Function CheckKeyIsCorrect(ByVal InputKey As String, ByVal FingerPrint As String, Optional ByVal KeyType As EnLangType = EnLangType.Quick) As Boolean
            Dim sql As String = "SELECT * FROM tblActivation WHERE Key_Id = '" & InputKey.ToString().Trim().CleanSQL & "' AND Serial = '" & FingerPrint & "';"
            Dim dt As DataTable = _DB.getdata(sql)
            sql = " SELECT COUNT(*) FROM dbo.tblKey WHERE Key_Id = '" & InputKey.ToString().Trim().CleanSQL & "' AND IsActive = 1 AND KeyType = " & KeyType & " AND Remaining > 0 "
            Dim CheckKey As Integer = CInt(_DB.ExecuteScalar(sql))

            If dt.Rows.Count = 0 And CheckKey = 0 Then ' case ไม่เคยละทะเบียนกับ key นี้เลย และจำนวน Remaining = 0 แล้ว
                Return False
            End If
            Return True
        End Function

        'Function เช็ค URL ว่าเข้ามาจาก localhost หรือเปล่า
        Public Shared Function CheckIsLocalhost() As Boolean
            Dim GetUrl As String = HttpContext.Current.Request.Url.ToString()
            If GetUrl.ToLower().IndexOf("localhost") = -1 Then
                Return False
            Else
                Return True
            End If
        End Function

        ''' <summary>
        ''' function เช็คว่า url ที่เรียกมาจาก ip ที่ระบุหรือเปล่า
        ''' </summary>
        ''' <param name="Ip"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckIsCallFromIP(Ip As String) As Boolean
            Dim GetUrl As String = HttpContext.Current.Request.Url.ToString()
            If Not GetUrl.ToLower().StartsWith("http://" & Ip) Then
                Return False
            Else
                Return True
            End If
        End Function

    End Class

    Public Enum SubjectId
        thai = 1
        social = 2
        math = 3
        science = 4
        english = 5
        health = 6
        art = 7
        home = 8
        pisa = 9
    End Enum

End Namespace

