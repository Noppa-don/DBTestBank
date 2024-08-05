Option Strict Off
Imports Excel = Microsoft.Office.Interop.Excel
Imports Microsoft.Office
Imports System.Runtime.InteropServices
Imports System.Web
Imports System.Data.SqlClient
Imports System.IO

Public Class Cleanuppage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Service.ClsSystem.CheckIsLocalhost() = False Then
            Response.Redirect("~/Default.aspx")
        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function CheckKeyIsCorrect(ByVal InputKey As String) As String
        If InputKey Is Nothing AndAlso InputKey.Length <> 39 Then
            Return ""
        End If
        If InputKey IsNot Nothing And InputKey.ToString() <> "" Then
            Try
                InputKey = InputKey.Replace("-", "")
                InputKey = InputKey.Insert(8, "-")
                InputKey = InputKey.Insert(13, "-")
                InputKey = InputKey.Insert(18, "-")
                InputKey = InputKey.Insert(23, "-")
                Dim CheckKey As String = ClsLanguage.GetKeyIdDec()

                If CheckKey.Length <> 36 Then
                    Return ""
                ElseIf CheckKey.ToLower() = InputKey.ToLower() Then
                    System.Web.HttpContext.Current.Session("authenticatefordelete") = Date.Now
                    Return "Correct"
                Else
                    Return ""
                End If
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                ElmahExtension.LogToElmah(ex)
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function DeleteAllData(ByVal IsAll As Boolean) As String
        Try
            If System.Web.HttpContext.Current.Session("authenticatefordelete") Is Nothing Then
                Return ""
            Else
                Dim authenticatetime As Date = System.Web.HttpContext.Current.Session("authenticatefordelete")
                Dim sec As Integer = DateDiff(DateInterval.Second, authenticatetime, Date.Now)
                If sec > 300 Then
                    Return ""
                End If
            End If
            Dim ClsSystem As New Service.ClsSystem(New ClassConnectSql)
            If ClsSystem.KNClearDB(IsAll) = True Then
                Return "Complete"
            Else
                Return ""
            End If
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            ElmahExtension.LogToElmah(ex)
            Return ""
        End Try

        'Dim _DB As New ClassConnectSql()
        'Try
        '    _DB.OpenWithTransection()

        '    'Truncate table ที่ไม่มี Iswpp ก่อน
        '    Dim ArrTableName As ArrayList = GetArrTableNameToDeleteData()
        '    Dim sql As String = ""
        '    If ArrTableName.Count > 0 Then
        '        For Each tableName As String In ArrTableName
        '            sql &= " TRUNCATE TABLE dbo." & tableName & "; "
        '        Next
        '        _DB.ExecuteWithTransection(sql)

        '        'ลบ tblUser โดยการ Where ไม่ให้ลบ UserDefault ออกไป
        '        sql = " DELETE dbo.tblUser WHERE UserName <> 'nobeladmin' AND Password <> '" & Encryption.MD5("tiger") & "' "

        '        'tbl ที่มี Iswpp ให้ลบเฉพาะที่ Iswpp = 0
        '        Dim ArrTableNameHaveIsWpp As ArrayList = GetArrTableNameHaveIsWpp()
        '        If ArrTableName.Count > 0 Then
        '            sql = ""
        '            For Each tablenameHaveIswpp As String In ArrTableNameHaveIsWpp
        '                sql &= " DELETE dbo." & tablenameHaveIswpp & " WHERE IsWpp = 0; "
        '            Next
        '            _DB.ExecuteWithTransection(sql)

        '            'Insert New User เข้าไป อิงตาม KNConfig ว่าใช้ชั้นไหน,วิชาอะไรได้บ้าง
        '            InsertNewUserAfterDeleteData(_DB)

        '            _DB.CommitTransection()
        '            _DB = Nothing
        '            Return "Complete"
        '        Else
        '            _DB.RollbackTransection()
        '            _DB = Nothing
        '            Return ""
        '        End If
        '    Else
        '        _DB.RollbackTransection()
        '        _DB = Nothing
        '        Return ""
        '    End If
        'Catch ex As Exception
        '    _DB.RollbackTransection()
        '    _DB = Nothing
        '    Return ""
        'End Try
    End Function

    'Public Shared Function GetArrTableNameHaveIsWpp() As ArrayList
    '    Dim StrTableNameIsWpp As String = "tblBook,tblQuestionset,tblQuestionCategory,tblQuestion,tblEvaluationIndexLevel,tblEvaluationIndexNew," & _
    '                                      "tblEvaluationIndexSubject,tblIntro,tblIntroQuestionSet,tblIntroQuestionSetQuestion,tblQuestionEvaluationIndexItem," & _
    '                                      "tblWordBook,tblAnswer"
    '    Dim ArrTableName As New ArrayList
    '    Dim SpliteStr = StrTableNameIsWpp.Split(",")
    '    For Each tableName As String In SpliteStr
    '        ArrTableName.Add(tableName.ToString())
    '    Next
    '    Return ArrTableName
    'End Function

    'Public Shared Function GetArrTableNameToDeleteData() As ArrayList
    '    Dim StrtableNameToDelete As String = "t360_tblCalendar,t360_tblLog,t360_tblNetwork,t360_tblNetworkHistory,t360_tblNews,t360_tblNewsRoom,t360_tblRoom," & _
    '                                         "t360_tblSchool,t360_tblSchoolClass,t360_tblSetTypeRunStudentNumber,t360_tblStudent,t360_tblStudentCheckName," & _
    '                                         "t360_tblStudentFinish,t360_tblStudentRoom,t360_tblTablet,t360_tblTabletLog,t360_tblTabletLost,t360_tblTabletOwner," & _
    '                                         "t360_tblTabletRepair,t360_tblTeacher,t360_tblTeacherRoom,t360_tblUpLevel,t360_tblUplevelConfirm,t360_tblUpLevelDetail," & _
    '                                         "t360_tblUser,t360_tblUserMenuItem,tblAssistant,tblAvatarComment,tblChatJoin,tblChatMessage,tblChatRecipient," & _
    '                                         "tblChatRoom,tblContact,tblLog,tblMobileAccessPassword,tblMobileRegistration,tblModule,tblModuleAssignment," & _
    '                                         "tblModuleAssignmentDetail,tblModuleDetail,tblModuleDetailCompletion,tblNote,tblParent,tblQuestionSetPassword,tblQuiz," & _
    '                                         "tblQuizAnswer,tblQuizQuestion,tblQuizScore,tblQuizSession,tblRequestParentApproval,tblRequestParentApprovalDetail," & _
    '                                         "tblSetEmail,tblStudentItem,tblStudentParent,tblStudentPhoto,tblStudentPoint,tblSyncLog,tblTabletLab,tblTabletLabDesk," & _
    '                                         "tblTeacherFavorite,tblTeacherNews,tblTeacherNewsDetail,tblTeacherNewsDetailCompletion,tblTestSet,tblTestSet_CM_temptblsetup," & _
    '                                         "tblTestSetQuestionDetail,tblTestSetQuestionSet,tblTestSetWinner,tblUserSetting,tblUserSubjectClass "
    '    Dim SpliteTableName = StrtableNameToDelete.Split(",")
    '    Dim ArrTableName As New ArrayList
    '    For Each tableName As String In SpliteTableName
    '        ArrTableName.Add(tableName.ToString())
    '    Next
    '    Return ArrTableName
    'End Function

    'Public Shared Function SpliteObjectUserSubjectClass(ByVal StrConfig As String) As ArrayList
    '    Dim ArrSplite As New ArrayList
    '    If StrConfig <> "" Then
    '        Dim SpliteStr = StrConfig.Split(",")
    '        If SpliteStr.Count > 0 Then
    '            For Each r In SpliteStr
    '                ArrSplite.Add(r)
    '            Next
    '            Return ArrSplite
    '        Else
    '            Return ArrSplite
    '        End If
    '    Else
    '        Return ArrSplite
    '    End If
    'End Function

    'Public Shared Function GetGroupSubjectIdBySubjectName(ByRef ObjConn As ClassConnectSql, ByVal SubjectName As String) As ArrayList
    '    Dim ArrReturn As New ArrayList
    '    Dim SubjectNumber As Integer = 0
    '    If SubjectName IsNot Nothing And SubjectName.Trim() <> "" Then
    '        Dim GroupSubjectShortName As String = ""
    '        If SubjectName.ToLower().Trim() = "thai" Then
    '            GroupSubjectShortName = "ภาษาไทย"
    '            SubjectNumber = 1
    '        ElseIf SubjectName.ToLower().Trim() = "art" Then
    '            GroupSubjectShortName = "ศิลปะ"
    '            SubjectNumber = 7
    '        ElseIf SubjectName.ToLower().Trim() = "career" Then
    '            GroupSubjectShortName = "การงานฯ"
    '            SubjectNumber = 8
    '        ElseIf SubjectName.ToLower().Trim() = "health" Then
    '            GroupSubjectShortName = "สุขศึกษา/พละฯ"
    '            SubjectNumber = 6
    '        ElseIf SubjectName.ToLower().Trim() = "eng" Then
    '            GroupSubjectShortName = "ภาษาตปท."
    '            SubjectNumber = 5
    '        ElseIf SubjectName.ToLower().Trim() = "social" Then
    '            GroupSubjectShortName = "สังคมฯ"
    '            SubjectNumber = 2
    '        ElseIf SubjectName.ToLower().Trim() = "science" Then
    '            GroupSubjectShortName = "วิทย์ฯ"
    '            SubjectNumber = 4
    '        ElseIf SubjectName.ToLower().Trim() = "math" Then
    '            GroupSubjectShortName = "คณิตฯ"
    '            SubjectNumber = 3
    '        End If
    '        Dim sql As String = " SELECT GroupSubject_Id FROM dbo.tblGroupSubject WHERE GroupSubject_ShortName = '" & GroupSubjectShortName & "' AND IsActive = 1 "
    '        Dim GroupSubjectId As String = ObjConn.ExecuteScalarWithTransection(sql)
    '        ArrReturn.Add(GroupSubjectId)
    '        ArrReturn.Add(SubjectNumber)
    '        Return ArrReturn
    '    Else
    '        Return ArrReturn
    '    End If
    'End Function

    'Public Shared Function GetLevelIdByLevelName(ByRef ObjConn As ClassConnectSql, ByVal LevelName As String) As ArrayList
    '    Dim ArrReturn As New ArrayList
    '    If LevelName IsNot Nothing And LevelName.Trim() <> "" Then
    '        Dim LevelShortName As String = ""
    '        Dim LevelNumber As Integer = 0
    '        If LevelName.Trim().ToUpper() = "K1" Then
    '            LevelShortName = "อ.1"
    '            LevelNumber = 1
    '        ElseIf LevelName.Trim().ToUpper() = "K2" Then
    '            LevelShortName = "อ.2"
    '            LevelNumber = 2
    '        ElseIf LevelName.Trim().ToUpper() = "K3" Then
    '            LevelShortName = "อ.3"
    '            LevelNumber = 3
    '        ElseIf LevelName.Trim().ToUpper() = "K4" Then
    '            LevelShortName = "ป.1"
    '            LevelNumber = 4
    '        ElseIf LevelName.Trim().ToUpper() = "K5" Then
    '            LevelShortName = "ป.2"
    '            LevelNumber = 5
    '        ElseIf LevelName.Trim().ToUpper() = "K6" Then
    '            LevelShortName = "ป.3"
    '            LevelNumber = 6
    '        ElseIf LevelName.Trim().ToUpper() = "K7" Then
    '            LevelShortName = "ป.4"
    '            LevelNumber = 7
    '        ElseIf LevelName.Trim().ToUpper() = "K8" Then
    '            LevelShortName = "ป.5"
    '            LevelNumber = 8
    '        ElseIf LevelName.Trim().ToUpper() = "K9" Then
    '            LevelShortName = "ป.6"
    '            LevelNumber = 9
    '        ElseIf LevelName.Trim().ToUpper() = "K10" Then
    '            LevelShortName = "ม.1"
    '            LevelNumber = 10
    '        ElseIf LevelName.Trim().ToUpper() = "K11" Then
    '            LevelShortName = "ม.2"
    '            LevelNumber = 11
    '        ElseIf LevelName.Trim().ToUpper() = "K12" Then
    '            LevelShortName = "ม.3"
    '            LevelNumber = 12
    '        ElseIf LevelName.Trim().ToUpper() = "K13" Then
    '            LevelShortName = "ม.4"
    '            LevelNumber = 13
    '        ElseIf LevelName.Trim().ToUpper() = "K14" Then
    '            LevelShortName = "ม.5"
    '            LevelNumber = 14
    '        ElseIf LevelName.Trim().ToUpper() = "K15" Then
    '            LevelShortName = "ม.6"
    '            LevelNumber = 15
    '        End If
    '        Dim sql As String = " SELECT * FROM dbo.tblLevel WHERE Level_ShortName = '" & LevelShortName & "' AND IsActive = 1 "
    '        Dim LevelId As String = ObjConn.ExecuteScalarWithTransection(sql)
    '        ArrReturn.Add(LevelId)
    '        ArrReturn.Add(LevelNumber)
    '        Return ArrReturn
    '    Else
    '        Return ArrReturn
    '    End If
    'End Function

    ''Match-K4,Match-K5,Match-K6
    'Public Shared Sub InsertNewUserAfterDeleteData(ByRef ObjConn As ClassConnectSql)
    '    If HttpContext.Current.Application("EnableUserSubjectClass") IsNot Nothing And HttpContext.Current.Application("EnableUserSubjectClass").ToString() <> "" Then
    '        Try
    '            'Insert tblUser
    '            Dim sql As String = " SELECT NEWID() "
    '            Dim UserId As String = ObjConn.ExecuteScalarWithTransection(sql)
    '            sql = " INSERT INTO dbo.tblUser( UserId ,FirstName ,LastName ,UserName ,Password ,SchoolId ,IsActive ,LastUpdate " & _
    '                  " ,ClientId ,GUID ,IsAllowMenuManageUserSchool ,IsAllowMenuManageUserAdmin ,IsAllowMenuAdminLog ,IsAllowMenuContact " & _
    '                  " ,IsAllowMenuSetEmail) SELECT  (COUNT(*) + 1) , 'teacher' ,'teacher' ,'teacher' ,'" & Encryption.MD5("1234") & "' , " & _
    '                  " '" & HttpContext.Current.Session("SchoolID").ToString() & "' ,1 " & _
    '                  " , dbo.GetThaiDate() ,0 , '" & UserId & "' , 1 , 1 , 1 , 1 ,1   FROM dbo.tblUser  "
    '            ObjConn.ExecuteWithTransection(sql)

    '            'Insert tblUserSubjectClass
    '            Dim ArrSpliteObj As ArrayList = SpliteObjectUserSubjectClass(HttpContext.Current.Application("EnableUserSubjectClass").ToString())
    '            If ArrSpliteObj.Count > 0 Then
    '                For Each r In ArrSpliteObj
    '                    Dim SplitEachItem = r.ToString.Split("-")
    '                    Dim SubjectName As String = SplitEachItem(0).ToString().Trim()
    '                    Dim LevelName As String = SplitEachItem(1).ToString().Trim()
    '                    Dim GroupSubjectId As ArrayList = GetGroupSubjectIdBySubjectName(ObjConn, SubjectName)
    '                    Dim LevelId As ArrayList = GetLevelIdByLevelName(ObjConn, LevelName)
    '                    If GroupSubjectId(0).ToString().Trim() <> "" And GroupSubjectId(1).ToString().Trim() <> "" And LevelId(0).ToString.Trim() <> "" And LevelId(1).ToString.Trim() Then
    '                        sql = " INSERT INTO dbo.tblUserSubjectClass ( USCId ,UserIdOld ,Detailid ,SubjectId ,ClassId ,GroupSubjectId ,LevelId ,IsActive " & _
    '                              " ,LastUpdate ,ClientId ,GUID ,UserId ) SELECT (COUNT(*) + 1) , 1 , 1 , '" & GroupSubjectId(1).ToString().Trim() & "' ,'" & LevelId(1).ToString().Trim() & "' , " & _
    '                              " '" & GroupSubjectId(0).ToString().Trim() & "' ,'" & LevelId(0).ToString.Trim() & "' , 1 , dbo.GetThaiDate() ,0 ,NEWID() ,'" & UserId & "' FROM dbo.tblUserSubjectClass "
    '                        ObjConn.ExecuteWithTransection(sql)
    '                    Else
    '                        Throw New Exception("")
    '                    End If
    '                Next
    '            Else
    '                Throw New Exception("ไม่มีค่า EnableUserSubjectClass ใน WebConfig")
    '            End If
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End If
    'End Sub


    <Services.WebMethod()>
    Public Shared Function ExportBackUpFile(IsAll As String) As String


        Dim ExportTable() As String = {"t360_tblCalendar", "t360_tblRoom", "t360_tblStudent", "t360_tblStudentRoom", "tblNote", "tblQuiz" _
                                      , "tblQuizAnswer", "tblQuizQuestion", "tblQuizSession", "tblTestSet", "tblTestSetQuestionDetail", "tblTestSetQuestionSet" _
                                      , "tblUser", "tblUserSubjectClass", "t360_tblTeacher", "t360_tblTeacherRoom", "tblAssistant"}

        Dim clt As New ClsTestSet(HttpContext.Current.Session("UserId"))

        Dim ExcelApp As New Excel.Application
        Dim WB As Excel.Workbook
        Dim WS As Excel.Worksheet

        WB = ExcelApp.Workbooks.Add()

        Dim dt As DataTable

        For Each EPT As String In ExportTable
            dt = clt.CreateExportDataTable(EPT)

            WS = WB.Sheets.Add
            WS.Name = EPT

            Dim colIndex As Integer = 0
            Dim rowIndex As Integer = 1
            Dim dc As DataColumn
            Dim dr As DataRow

            For Each dc In dt.Columns
                colIndex += 1
                WS.Cells(rowIndex, colIndex) = dc.ColumnName
            Next

            For Each dr In dt.Rows

                colIndex = 0
                rowIndex += 1
                For Each dc In dt.Columns
                    colIndex += 1
                    WS.Cells(rowIndex, colIndex) = dr(dc.ColumnName).ToString
                Next
            Next

            WS.Columns.AutoFit()

        Next

        Dim strFileName As String = "D:\Development2\Document\BackUpPointplus_" & Now.Year & Now.Month & Now.Day & "_" & Now.Hour & Now.Minute & ".xlsx"
        WB.SaveAs(strFileName)
        WB.Close()
        ExcelApp.Quit()

        Return "Complete"

    End Function

End Class