Imports KnowledgeUtils.Database.DBFactory.LinqToSql(Of BusinessTablet360.DataClassesTablet360DataContext)
Imports KnowledgeUtils.Database.DBFactory
Imports KnowledgeUtils.Database
Imports KnowledgeUtils
Imports System.Web
Imports System.Text
Imports BusinessTablet360

''' <summary>
''' TabletOwner
''' (PK Database) School_Code, TON_Id 
''' (PK Real) School_Code, Tablet_Id, Owner_Id, OwnerType
''' 
''' TabletLost
''' (PK Database) School_Code, TON_Id, TLT_LostDate
''' </summary>
''' <remarks></remarks>
Public Interface ITabletManager

    'todo t360_tblTabletLog ไม่มีตอน insert

    '<<< Tablet
    Function GetTabletBySerialNumber(ByVal SerialNumber As String) As t360_tblTablet
    Function GetTabletById(ByVal TabletId As Guid) As t360_tblTablet
    Function GetAllTablet(ByVal School_Code As String) As t360_tblTablet()
    Function CalPercentUseTablet(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Single
    Function CalDayLastFound(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer
    Function CalUseTabletInClass(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalReadBook(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalUseBookOut(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalTabletStatus(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer
    Function CalStorageAvailable(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalStorageTotal(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalDayPassInSchool(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer
    Function CalTotalTimeUse(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalTotalTimeCharge(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String
    Function CalProgramStatus(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer


    '<<< TabletOwner
    Function GetStudentHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetStudentNotHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetStudentHaveAndNotTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTeacherHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTeacherNotHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTeacherHaveAndNotTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTabletOwnerStudentByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTabletOwnerTeacherByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function GetTabletOwnerByCrit(Of T)(ByVal Item As TabletOwnerDTO) As T()
    Function UpdateReturnTablet(ByVal TON_Id As Guid, ByVal TON_ReturnDate As Date, ByVal Tablet_Status As EnTabletStatus) As Boolean

    '<<< TabletLost
    Function InsertTabletLost(ByVal Item As t360_tblTabletLost, ByVal TON_Id As Guid) As Boolean
    Function UpdateTabletLost(ByVal Item As t360_tblTabletLost, ByVal TON_Id As Guid) As Boolean
    Function GetTabletLostByTON_Id(ByVal TON_Id As Guid) As t360_tblTabletLost()

    '<<< TabletRepair
    Function InsertTabletRepair(ByVal Item As t360_tblTabletRepair, ByVal TON_Id As Guid) As Boolean
    Function UpdateTabletRepair(ByVal Item As t360_tblTabletRepair, ByVal TON_Id As Guid) As Boolean
    Function GetTabletRepairByTON_Id(ByVal TON_Id As Guid) As t360_tblTabletRepair()

    '<<< Lab
    Function GetAllTabletLabFull(ByVal SchoolCode As String) As tblTabletLab()
    Function GetTabletLabById(ByVal TabletLabId As Guid) As tblTabletLab
    Function GetTabletLabFullById(ByVal TabletLabId As Guid) As tblTabletLab
    Function GetTabletLabByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()
    Function ValidateDuplicateLab(ByVal LabName As String, SchoolCode As String, Optional Id As Guid? = Nothing) As Boolean
    Function IsFoundTabletInLabname(ByVal SerialNumber As String, LabName As String) As Boolean
    Function CheckLabNameByTabletSerialNumber(ByVal SerialNumber As String, LabName As String) As String
    Function InsertTabletLab(ByVal Item As tblTabletLab) As Boolean
    Function UpDateTabletLab(ByVal Item As tblTabletLab) As Boolean
    Function DeleteTabletLab(ByVal TabletLab_Id As Guid) As Boolean
    Function InsertTabletLabDesk(ByVal Item As tblTabletLabDesk) As Boolean
    Function UpDateTabletLabDesk(ByVal Item As tblTabletLabDesk) As Boolean


    '<<< View
    Function GetTabletDetailByCrit(ByVal Item As UseTabletDetailDTO) As t360_uvwTabletDetail()
    Function GetTabletDetailSummaryByCrit(ByVal Item As UseTabletDetailDTO) As UseTabletDetailDTO()
    Function GetTabletStatusAll() As TabletStatusDTO()
    Function GetFoundTabletByCrit(ByVal Item As uvwFoundTablet) As uvwFoundTablet()
    Function GetUseLocalByCrit(ByVal Item As uvwUseLocation) As uvwUseLocation()
    Function GetStatisticReportByCrit(ByVal Item As uvwStatisticReport) As uvwStatisticReport()
    Function GetPercentClassByCrit(ByVal Item As uvwPercentClass) As uvwPercentClass()

    '<< SpareTablet
    Function GetTabletSpareByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()

    '<<< Tablet AssetNo
    Function GetOwnerNameByTablet(ByVal Item As t360_tblTablet, ByVal tabletType As Integer) As t360_tblTabletStatusDetail
    Function UpdateTabletAssetNo(ByVal AssetNo As String, ByVal TabletId As Guid) As Boolean

    '<<< Update Tablet Status
    Function UpdateTabletStatus(TabletStatusDetail As t360_tblTabletStatusDetail) As Boolean


    '<< tablet mobile director registered
    Function GetMobileDirectorRegister(ByVal DeviceId As String) As Boolean

    Function GetAllTabletOwner(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO()

    Function GetTabletHistory(ByVal TabletId As String) As t360_tblTabletStatusDetail()

    Function GetTabletUseInToDay() As Integer
    Function GetTabletUseInLastWeek() As Integer

    Function GetDamagedTablet() As DataTable
End Interface

Public Class TabletManager
    Implements ITabletManager

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

    Private _UserManager As IUserManager
    <Dependency()> Public Property UserManager() As IUserManager
        Get
            Return _UserManager
        End Get
        Set(ByVal value As IUserManager)
            _UserManager = value
        End Set
    End Property

#End Region

#Region "Tablet"

    Public Function GetTabletBySerialNumber(SerialNumber As String) As t360_tblTablet Implements ITabletManager.GetTabletBySerialNumber
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From q In Ctx.t360_tblTablets Where q.Tablet_SerialNumber = SerialNumber And q.Tablet_IsActive = True).SingleOrDefault
            End Using
        End With
    End Function

    Public Function GetTabletById(TabletId As Guid) As t360_tblTablet Implements ITabletManager.GetTabletById
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From q In Ctx.t360_tblTablets Where q.Tablet_Id = TabletId And q.Tablet_IsActive = True).SingleOrDefault
            End Using
        End With
    End Function

    Public Function GetAllTablet(ByVal School_Code As String) As t360_tblTablet() Implements ITabletManager.GetAllTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From q In Ctx.t360_tblTablets Where q.School_Code = School_Code And q.Tablet_IsActive = True).ToArray
            End Using
        End With
    End Function

    Public Function CalDayPassInSchool(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer Implements ITabletManager.CalDayPassInSchool
        Return 3
    End Function

    Public Function CalPercentUseTablet(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Single Implements ITabletManager.CalPercentUseTablet
        Return 100
    End Function

    Public Function CalReadBook(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalReadBook
        Return (New EnumStatusTabletUse).GetText(EnStatusTabletUse.Always)
    End Function

    Public Function CalStorageAvailable(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalStorageAvailable
        Return "10 G"
    End Function

    Public Function CalTabletStatus(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer Implements ITabletManager.CalTabletStatus
        Return 100
    End Function

    Public Function CalProgramStatus(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer Implements ITabletManager.CalProgramStatus
        Return 100
    End Function

    Public Function CalTotalTimeCharge(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalTotalTimeCharge
        Return "3:00"
    End Function

    Public Function CalTotalTimeUse(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalTotalTimeUse
        Return "10:00"
    End Function

    Public Function CalUseTabletInClass(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalUseTabletInClass
        Return (New EnumStatusTabletUse).GetText(EnStatusTabletUse.Always)
    End Function

    Public Function CalDayLastFound(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As Integer Implements ITabletManager.CalDayLastFound
        Return 5
    End Function

    Public Function CalStorageTotal(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalStorageTotal
        Return "16 G"
    End Function

    Public Function CalUseBookOut(ByVal TON_Id As Integer, ByVal Owner_Type As Byte) As String Implements ITabletManager.CalUseBookOut
        Return (New EnumStatusTabletUse).GetText(EnStatusTabletUse.Always)
    End Function

#End Region

#Region "TabletOwner"

    Public Function GetStudentHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetStudentHaveTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                .MainSql = "SELECT t.Tablet_Id,T.Tablet_SerialNumber, S.Student_Id, S.Student_Code, S.Student_PrefixName, S.Student_FirstName, S.Student_LastName, TOW.School_Code, " &
                                 "S.Student_CurrentClass, S.Student_CurrentRoom, TOW.TON_ReceiveDate, Owner_Type " &
                                 "FROM t360_tblTabletOwner AS TOW INNER JOIN " &
                                 "t360_tblTablet AS T ON TOW.Tablet_Id = T.Tablet_Id INNER JOIN " &
                                 "t360_tblStudent AS S ON TOW.School_Code = S.School_Code AND TOW.Owner_Id = S.Student_Id AND TOW.Owner_Type = 2 " &
                                 "WHERE {F}"

                Dim f As New SqlPart
                f.AddPart("Tablet_IsActive={0}", True)
                f.AddPart("TabletOwner_IsActive={0}", True)
                f.AddPart("Student_IsActive={0}", True)
                f.AddPart("TOW.School_Code={0}", UserConfig.GetCurrentContext.School_Code)
                f.AddPart("Student_CurrentClass={0}", Item.Student_CurrentClass)
                f.AddPart("Student_CurrentRoom={0}", Item.Student_CurrentRoom)
                .ApplySqlPart("F", f)

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With
    End Function

    Public Function GetStudentNotHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetStudentNotHaveTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                .MainSql = "SELECT School_Code, Student_Id, Student_Code, Student_PrefixName, Student_FirstName, Student_LastName, Student_CurrentClass, " &
                          "Student_CurrentRoom FROM t360_tblStudent " &
                          "WHERE NOT EXISTS(SELECT Owner_Id FROM t360_tblTabletOwner WHERE t360_tblStudent.Student_Id = Owner_Id AND t360_tblStudent.School_Code = {School_Code} AND Owner_Type=2 AND t360_tblTabletOwner.TabletOwner_IsActive=1) " &
                          "AND {F}"

                .ApplyTagWithValue("School_Code", UserConfig.GetCurrentContext.School_Code)

                Dim f As New SqlPart
                f.AddPart("Student_IsActive={0}", True)
                f.AddPart("Student_CurrentClass={0}", Item.Student_CurrentClass)
                f.AddPart("Student_CurrentRoom={0}", Item.Student_CurrentRoom)
                f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
                .ApplySqlPart("F", f)

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With
    End Function

    Public Function GetStudentHaveAndNotTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetStudentHaveAndNotTablet
        Dim ListStudentHave = GetStudentHaveTablet(Item)
        Dim ListStudentNotHave = GetStudentNotHaveTablet(Item)
        Dim r = (From q In ListStudentHave).Union(From q In ListStudentNotHave)
        Return r.ToArray
    End Function

    Public Function GetTeacherHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTeacherHaveTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                .MainSql = "SELECT T.Tablet_SerialNumber, TOW.School_Code, TOW.TON_ReceiveDate, TEA.Teacher_Id, TEA.Teacher_Code, TEA.Teacher_PrefixName, TEA.Teacher_FirstName, " &
                          "TEA.Teacher_LastName, TEA.Teacher_CurrentClass, TEA.Teacher_CurrentRoom, Owner_Type " &
                          "FROM t360_tblTabletOwner AS TOW INNER JOIN t360_tblTablet AS T ON TOW.Tablet_Id = T.Tablet_Id  INNER JOIN " &
                          "t360_tblTeacher AS TEA ON TOW.School_Code = TEA.School_Code AND TOW.Owner_Id = TEA.Teacher_Id AND TOW.Owner_Type = 1 " &
                          "WHERE {F}"

                Dim f As New SqlPart
                f.AddPart("Tablet_IsActive={0}", True)
                f.AddPart("TabletOwner_IsActive={0}", True)
                f.AddPart("Teacher_IsActive={0}", True)
                f.AddPart("TOW.School_Code={0}", UserConfig.GetCurrentContext.School_Code)
                f.AddPart("Teacher_CurrentClass={0}", Item.Teacher_CurrentClass)
                f.AddPart("Teacher_CurrentRoom={0}", Item.Teacher_CurrentRoom)
                .ApplySqlPart("F", f)

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With

    End Function

    Public Function GetTeacherNotHaveTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTeacherNotHaveTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                .MainSql = "SELECT Teacher_Code, Teacher_Id, Teacher_Code, Teacher_PrefixName, Teacher_FirstName, Teacher_LastName, Teacher_CurrentClass, " &
                                  "Teacher_CurrentRoom FROM t360_tblTeacher " &
                                  "WHERE NOT EXISTS(SELECT Owner_Id FROM t360_tblTabletOwner WHERE t360_tblTeacher.Teacher_Id = Owner_Id AND t360_tblTeacher.School_Code = {School_Code} AND Owner_Type=1 AND t360_tblTabletOwner.TabletOwner_IsActive=1) " &
                                  "AND {F}"

                .ApplyTagWithValue("School_Code", UserConfig.GetCurrentContext.School_Code)

                Dim f As New SqlPart
                f.AddPart("Teacher_IsActive={0}", True)
                f.AddPart("t360_tblTeacher.School_Code={0}", UserConfig.GetCurrentContext.School_Code)
                f.AddPart("Teacher_CurrentClass={0}", Item.Teacher_CurrentClass)
                f.AddPart("Teacher_CurrentRoom={0}", Item.Teacher_CurrentRoom)
                .ApplySqlPart("F", f)

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With

    End Function

    Public Function GetTeacherHaveAndNotTablet(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTeacherHaveAndNotTablet
        Dim ListTeachertHave = GetTeacherHaveTablet(Item)
        Dim ListTeacherNotHave = GetTeacherNotHaveTablet(Item)
        Dim r = (From q In ListTeachertHave).Union(From q In ListTeacherNotHave)
        Return r.ToArray
    End Function

    Public Function GetAllTabletOwner(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetAllTabletOwner
        With GetLinqToSql
            .MainSql = " SELECT * FROM ( SELECT TON_Id,TL.AssetNo, TL.Tablet_Id, TL.Tablet_SerialNumber, ST.Student_Id AS OwnerId, ST.Student_Code AS OwnerCode, ST.Student_PrefixName AS PrefixName, " &
                       " ST.Student_FirstName AS FirstName, ST.Student_LastName AS LastName ,  ST.Student_FatherPhone AS Phone,case when TSD_Status = 1 " &
                       " then ST.Student_PrefixName + ' ' + ST.Student_FirstName + ' ' + ST.Student_LastName else 'ไม่มีคนใช้งาน' end AS FullName, " &
                       " TON.TON_ReceiveDate,(SELECT MAX(TLG_TimeStamp) AS t FROM dbo.t360_tblTabletLog WHERE (School_Code = TON.School_Code) " &
                       " AND (TON_Id = TON.TON_Id) AND (TLG_InSchool = 1)) AS TLG_TimeStamp, " &
                       " case when TSD_Status = 1 or TSD_Status = 2 then 'ปกติ' when TSD_Status = 3 or TSD_Status = 4 then 'เสียหาย'  " &
                       " when TSD_Status = 5 then 'ส่งซ่อม' else 'สูญหาย' End As TSD_StatusName,TSD_Status,Owner_Type  " &
                       " FROM t360_tblTablet AS TL INNER JOIN t360_tblTabletOwner AS TON ON TL.Tablet_Id = TON.Tablet_Id   " &
                       " INNER JOIN t360_tblStudent AS ST ON TON.School_Code = ST.School_Code AND TON.Owner_Id = ST.Student_Id " &
                       " AND TON.Owner_Type = 2 INNER JOIN t360_tblTabletStatusDetail TS on TL.Tablet_Id = TS.Tablet_Id " &
                       " WHERE TON.TabletOwner_IsActive = 1 " &
                       " UNION " 'student union teacher
            .MainSql &= " SELECT TON_Id,TL.AssetNo,TL.Tablet_Id, TL.Tablet_SerialNumber, TH.Teacher_Id AS OwnerId, TH.Teacher_Code AS OwnerCode, TH.Teacher_PrefixName  AS PrefixName,  " &
                         "  TH.Teacher_FirstName AS FirstName, TH.Teacher_LastName AS LastName, TH.Teacher_Phone AS Phone,TH.Teacher_PrefixName + ' ' + TH.Teacher_FirstName + ' ' + TH.Teacher_LastName AS FullName, TON.TON_ReceiveDate,  " &
                         "  (SELECT MAX(TLG_TimeStamp) AS t FROM dbo.t360_tblTabletLog WHERE (School_Code = TON.School_Code)  " &
                         "  And (TON_Id = TON.TON_Id) And (TLG_InSchool = 1)) AS TLG_TimeStamp ,'ปกติ' AS TSD_StatusName ,TL.Tablet_Status AS TSD_Status,Owner_Type " &
                         "  FROM t360_tblTablet AS TL INNER JOIN t360_tblTabletOwner AS TON ON TL.Tablet_Id = TON.Tablet_Id INNER JOIN  " &
                         "  t360_tblTeacher AS TH ON TON.School_Code = TH.School_Code AND TON.Owner_Id = TH.Teacher_Id AND TON.Owner_Type = 1 " &
                         " WHERE TON.TabletOwner_IsActive = 1 " &
                         " UNION " 'teacher union tabletlab
            .MainSql &= " Select t360_tblTablet.Tablet_Id As TON_Id,t360_tblTablet.AssetNo, t360_tblTablet.Tablet_Id,t360_tblTablet.Tablet_SerialNumber,tblTabletLab.TabletLab_Id As OwnerId , " &
                        " 'LAB' AS OwnerCode,'' AS PrefixName,'LabRoom' AS FirstName,'LabRoom' AS LastName,NULL AS Phone, " &
                        "  tblTabletLab.TabletLab_Name + ' / โต๊ะ ' + cast(tblTabletLabDesk.DeskName as varchar) as FullName,  " &
                        "  tblTabletLabDesk.LastUpdate as TON_ReceiveDate, NULL AS TLG_TimeStamp, 'ปกติ' AS TSD_StatusName,t360_tblTablet.Tablet_Status AS TSD_Status,CAST(3 as tinyint) AS Owner_Type " &
                        "  FROM tblTabletLab INNER JOIN tblTabletLabDesk ON tblTabletLab.TabletLab_Id = tblTabletLabDesk.TabletLab_Id  " &
                        "  INNER JOIN t360_tblTablet ON tblTabletLabDesk.Tablet_Id = t360_tblTablet.Tablet_Id " &
                        " WHERE tblTabletLabDesk.IsActive = 1 " &
                        " UNION "  'tabletlab union สำรอง
            .MainSql &= " SELECT t.Tablet_Id as TON_Id,t.AssetNo,  t.Tablet_Id, t.Tablet_SerialNumber,NULL AS OwnerId,'' AS OwnerCode,'' AS PrefixName,'สำรอง' AS FirstName,'สำรอง' AS LastName,NULL AS Phone, " &
                       "  t.Tablet_TabletName AS FullName, t.Tablet_LastUpdate AS TON_ReceiveDate, MAX(q.LastUpdate) AS TLG_TimeStamp , " &
                       " case when TSD_Status = 1 or TSD_Status = 2 then 'ปกติ' when TSD_Status = 3 or TSD_Status = 4 then 'เสียหาย'  when TSD_Status = 5 then 'ส่งซ่อม' else 'สูญหาย' End As TSD_StatusName " &
                       " ,TSD_Status,CAST(4 as tinyint) AS Owner_Type " &
                       " FROM t360_tblTablet t LEFT JOIN tblQuizSession q ON t.Tablet_Id = q.Tablet_Id left JOIN t360_tblTabletStatusDetail td ON t.Tablet_Id = td.Tablet_Id   " &
                       " WHERE t.Tablet_IsActive = 1  And t.Tablet_IsOwner = 0  and td.IsActive = 1 " &
                       " AND t.Tablet_Id NOT IN (SELECT Tablet_Id FROM tblTabletLabDesk WHERE IsActive = 1)  " &
                       " GROUP BY t.Tablet_Id,t.AssetNo, t.Tablet_SerialNumber, t.Tablet_TabletName,t.Tablet_LastUpdate,t.Tablet_Status,TSD_Status) AS t " &
                       " ORDER BY CASE WHEN t.AssetNo IS NULL THEN 1 ELSE 0 END,t.AssetNo,t.FirstName "
            .LockWhere = True
            Return .DataContextExecuteObjects(Of TabletOwnerDTO)().ToArray
        End With
    End Function

    Public Function GetTabletOwnerStudentByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTabletOwnerStudentByCrit
        With GetLinqToSql
            .MainSql = "SELECT TON_Id,TL.AssetNo, TL.Tablet_Id, TL.Tablet_SerialNumber, ST.Student_Id, ST.Student_Code, ST.Student_PrefixName, " &
                             "ST.Student_FirstName, ST.Student_LastName, ST.Student_MotherName,ST.Student_MotherPhone, " &
                             "ST.Student_FatherName ,ST.Student_FatherPhone AS Phone,case when TSD_Status = 1 " &
                             "then ST.Student_PrefixName + ' ' + ST.Student_FirstName + ' ' + ST.Student_LastName else 'ไม่มีคนใช้งาน' end AS FullName, " &
                             "TON.TON_ReceiveDate,(SELECT MAX(TLG_TimeStamp) AS t FROM dbo.t360_tblTabletLog WHERE (School_Code = TON.School_Code) " &
                             "AND (TON_Id = TON.TON_Id) AND (TLG_InSchool = 1)) AS TLG_TimeStamp, " &
                             "case when TSD_Status = 1 or TSD_Status = 2 then 'ปกติ' when TSD_Status = 3 or TSD_Status = 4 then 'เสียหาย' " &
                             "when TSD_Status = 5 then 'ส่งซ่อม' else 'สูญหาย' End As TSD_StatusName,TSD_Status " &
                             "FROM t360_tblTablet AS TL INNER JOIN t360_tblTabletOwner AS TON ON TL.Tablet_Id = TON.Tablet_Id  " &
                             "INNER JOIN t360_tblStudent AS ST ON TON.School_Code = ST.School_Code AND TON.Owner_Id = ST.Student_Id " &
                             "AND TON.Owner_Type = 2 INNER JOIN t360_tblTabletStatusDetail TS on TL.Tablet_Id = TS.Tablet_Id " &
                             "WHERE {F}"

            Dim Fs As New SqlPart
            Fs.AddPart("TON.School_Code = {0}", UserConfig.GetCurrentContext.School_Code)
            Fs.AddPart("TL.Tablet_IsActive = {0}", Item.Tablet_IsActive)
            Fs.AddPart("TON.TabletOwner_IsActive = {0}", Item.TabletOwner_IsActive)
            Fs.AddPart("TS.IsActive = {0}", True)
            Fs.AddPart("TON.TON_Id = {0}", Item.TON_Id)
            Fs.AddPart("TL.AssetNo LIKE {0}", Item.AssetNo.FusionText("%"), EnJoinType.JoinByOr)
            Fs.AddPart("ST.Student_FirstName LIKE {0}", Item.Student_FirstName_Like.FusionText("%"), EnJoinType.JoinByOr)
            Fs.AddPart("ST.Student_LastName LIKE {0}", Item.Student_LastName_Like.FusionText("%"), EnJoinType.JoinByOr)
            Fs.AddPart("ST.Student_Code LIKE {0}", Item.Student_Code_Like.FusionText("%"), EnJoinType.JoinByOr)
            Fs.AddPart("ST.Student_CurrentClass={0}", Item.Student_CurrentClass, EnJoinType.JoinByOr)
            Fs.AddPart("ST.Student_CurrentRoom={0}", Item.Student_CurrentRoom, EnJoinType.JoinByOr)
            .ApplySqlPart("F", Fs)

            Return .DataContextExecuteObjects(Of TabletOwnerDTO)().ToArray
        End With

    End Function

    Public Function GetTabletOwnerTeacherByCrit(ByVal Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTabletOwnerTeacherByCrit
        With GetLinqToSql
            .MainSql = "SELECT TON_Id,TL.AssetNo,TL.Tablet_Id, TL.Tablet_SerialNumber, TH.Teacher_Id, TH.Teacher_Code, TH.Teacher_PrefixName,TL.Tablet_Status," &
                           "TH.Teacher_FirstName, TH.Teacher_LastName, TH.Teacher_Phone AS Phone, TON.TON_ReceiveDate,TH.Teacher_PrefixName + ' ' + TH.Teacher_FirstName + ' ' + TH.Teacher_LastName AS FullName, " &
                           "(SELECT MAX(TLG_TimeStamp) AS t FROM dbo.t360_tblTabletLog WHERE (School_Code = TON.School_Code) " &
                           "AND (TON_Id = TON.TON_Id) AND (TLG_InSchool = 1)) AS TLG_TimeStamp,'' " &
                           "FROM t360_tblTablet AS TL INNER JOIN t360_tblTabletOwner AS TON ON TL.Tablet_Id = TON.Tablet_Id INNER JOIN " &
                           "t360_tblTeacher AS TH ON TON.School_Code = TH.School_Code AND TON.Owner_Id = TH.Teacher_Id AND TON.Owner_Type = 1 " &
                           "WHERE {F}"

            Dim Fs As New SqlPart
            Fs.AddPart("TON.School_Code = {0}", UserConfig.GetCurrentContext.School_Code)
            Fs.AddPart("TL.Tablet_IsActive = {0}", Item.Tablet_IsActive)
            Fs.AddPart("TON.TabletOwner_IsActive = {0}", Item.TabletOwner_IsActive)
            'Fs.AddPart("TON.TON_Id = {0}", Item.TON_Id)
            'Fs.AddPart("TL.AssetNo LIKE {0}", Item.AssetNo.FusionText("%"))
            'Fs.AddPart("TH.Teacher_FirstName LIKE {0}", Item.Teacher_FirstName_Like.FusionText("%"))
            'Fs.AddPart("TH.Teacher_LastName LIKE {0}", Item.Teacher_LastName_Like.FusionText("%"))
            'Fs.AddPart("TH.Teacher_CurrentClass={0}", Item.Teacher_CurrentClass)
            'Fs.AddPart("TH.Teacher_CurrentRoom={0}", Item.Teacher_CurrentRoom)
            .ApplySqlPart("F", Fs)

            Return .DataContextExecuteObjects(Of TabletOwnerDTO)().ToArray
        End With

    End Function

    Public Function GetTabletOwnerByCrit(Of T)(ByVal Item As TabletOwnerDTO) As T() Implements ITabletManager.GetTabletOwnerByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_tblTabletOwner WHERE {F}"

            Dim f As New SqlPart
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            f.AddPart("TON_Id={0}", Item.TON_Id)
            .ApplySqlPart("F", f)

            Return .DataContextExecuteObjects(Of T)().ToArray
        End With

    End Function

    Public Function UpdateReturnTablet(ByVal TON_Id As Guid, ByVal TON_ReturnDate As Date, ByVal Tablet_Status As EnTabletStatus) As Boolean Implements ITabletManager.UpdateReturnTablet
        Dim LogDetail As New StringBuilder
        Try
            'ปรับให้ความเป็นเจ้าของที่ TabletOwner หายไป
            'ปรับสถานะของ Tablet เป็นไม่มีเจ้าของ
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim TabletOwner = (From r In Ctx.t360_tblTabletOwners Where r.TON_Id = TON_Id AndAlso r.School_Code = UserConfig.GetCurrentContext.School_Code).SingleOrDefault
                Dim Tablets = (From r In Ctx.t360_tblTablets Where r.Tablet_Id = TabletOwner.Tablet_Id).SingleOrDefault
                Dim TabletStatusName As String
                If Tablet_Status = EnTabletStatus.Normal Then
                    TabletStatusName = "ปกติ"
                Else
                    TabletStatusName = "ส่งซ่อม"
                End If
                LogDetail.Append("คืน -แท็บเลตเลขเครื่อง: ")
                LogDetail.Append(Tablets.Tablet_SerialNumber)
                LogDetail.Append(" -สถานะ: ")
                LogDetail.Append(TabletStatusName)
                LogDetail.Append(" t360_tblTablet.Id=")
                LogDetail.Append(Tablets.Tablet_Id.ToString)

                TabletOwner.TON_ReturnDate = TON_ReturnDate
                TabletOwner.TabletOwner_IsActive = False
                TabletOwner.LastUpdate = GetTime
                TabletOwner.ClientId = Nothing

                Tablets.Tablet_Status = Tablet_Status
                Tablets.Tablet_IsOwner = EnTabletOwnerStatus.NotOwner
                Tablets.LastUpdate = GetTime
                Tablets.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            UserManager.Log(EnLogType.ManageTabletStatus, LogDetail.ToString)
        End Try
    End Function
#End Region

#Region "TabletLost"

    Public Function InsertTabletLost(ByVal Item As t360_tblTabletLost, ByVal TON_Id As Guid) As Boolean Implements ITabletManager.InsertTabletLost
        Dim LogDetail As New StringBuilder
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim TabletOwner = (From r In Ctx.t360_tblTabletOwners Where r.TON_Id = TON_Id).SingleOrDefault
                Dim Tablet = (From r In Ctx.t360_tblTablets Where r.Tablet_Id = TabletOwner.Tablet_Id).SingleOrDefault
                LogDetail.Append("หาย -แท็บเลตเลขเครื่อง: ")
                LogDetail.Append(Tablet.Tablet_SerialNumber)
                LogDetail.Append(" -เมื่อ:")
                LogDetail.Append(Item.TLT_LostDate)
                LogDetail.Append(" -เลขที่ใบแจ้งความ: ")
                LogDetail.Append(Item.TLT_DocNumber)
                LogDetail.Append(" ,t360_tblTablet.Id=")
                LogDetail.Append(Tablet.Tablet_Id.ToString)

                TabletOwner.TabletOwner_IsActive = False
                TabletOwner.LastUpdate = GetTime
                TabletOwner.ClientId = Nothing

                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                Item.LastUpdate = GetTime
                Item.IsActive = True
                TabletOwner.t360_tblTabletLosts.Add(Item)


                Tablet.Tablet_Status = EnTabletStatus.Lost
                Tablet.LastUpdate = GetTime
                Tablet.ClientId = Nothing


                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            UserManager.Log(EnLogType.ManageTabletStatus, LogDetail.ToString)
        End Try
    End Function

    Public Function UpdateTabletLost(ByVal Item As t360_tblTabletLost, ByVal TON_Id As Guid) As Boolean Implements ITabletManager.UpdateTabletLost
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim TabletLost = (From r In Ctx.t360_tblTabletLosts Where r.TON_Id = TON_Id And r.IsActive = True).SingleOrDefault
                TabletLost.LastUpdate = GetTime
                TabletLost.IsActive = False

                Dim TabletOwner = (From r In Ctx.t360_tblTabletOwners Where r.TON_Id = TON_Id).SingleOrDefault
                Item.LastUpdate = GetTime
                Item.IsActive = True


                TabletOwner.t360_tblTabletLosts.Add(Item)
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.ManageTabletStatus
                .Log_Page = "หาย"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function GetTabletLostByTON_Id(ByVal TON_Id As Guid) As t360_tblTabletLost() Implements ITabletManager.GetTabletLostByTON_Id
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTabletLosts.Where(Function(q) q.TON_Id = TON_Id And q.IsActive = True).ToArray
        End Using
    End Function

#End Region

#Region "TabletRepair"

    Public Function GetTabletRepairByTON_Id(ByVal TON_Id As System.Guid) As t360_tblTabletRepair() Implements ITabletManager.GetTabletRepairByTON_Id
        Using Ctx = GetLinqToSql.GetDataContext()
            Return Ctx.t360_tblTabletRepairs.Where(Function(q) q.TON_Id = TON_Id And q.IsActive = True).ToArray
        End Using
    End Function

    Public Function InsertTabletRepair(ByVal Item As t360_tblTabletRepair, ByVal TON_Id As System.Guid) As Boolean Implements ITabletManager.InsertTabletRepair
        Dim LogDetail As New StringBuilder
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim TabletOwner = (From r In Ctx.t360_tblTabletOwners Where r.TON_Id = TON_Id).SingleOrDefault
                Dim Tablet = (From r In Ctx.t360_tblTablets Where r.Tablet_Id = TabletOwner.Tablet_Id).SingleOrDefault

                LogDetail.Append("ส่งซ่อม -แท็บเลตเลขเครื่อง: ")
                LogDetail.Append(Tablet.Tablet_SerialNumber)
                LogDetail.Append(" ,t360_tblTablet.Id=")
                LogDetail.Append(Tablet.Tablet_Id.ToString)

                Item.LastUpdate = GetTime
                Item.IsActive = True
                Item.School_Code = UserConfig.GetCurrentContext.School_Code
                TabletOwner.t360_tblTabletRepairs.Add(Item)

                Tablet.Tablet_Status = EnTabletStatus.SendToFix
                Tablet.Tablet_LastUpdate = GetTime
                Tablet.LastUpdate = GetTime
                Tablet.ClientId = Nothing
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            UserManager.Log(EnLogType.ManageTabletStatus, LogDetail.ToString)
        End Try
    End Function

    Public Function UpdateTabletRepair(ByVal Item As t360_tblTabletRepair, ByVal TON_Id As System.Guid) As Boolean Implements ITabletManager.UpdateTabletRepair
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate
            Using Ctx = GetLinqToSql.GetDataContext()
                Dim TabletRepair = (From q In Ctx.t360_tblTabletRepairs Where q.TON_Id = TON_Id And q.IsActive = True).SingleOrDefault
                If Item.TLR_ReturnDate Is Nothing Then
                    With TabletRepair
                        .TLR_DocNumber = Item.TLR_DocNumber
                        .TLR_Location = Item.TLR_Location
                        .TLR_RepairDate = Item.TLR_RepairDate
                        .LastUpdate = GetTime
                        .IsActive = True
                        .ClientId = Nothing
                    End With
                Else
                    With TabletRepair
                        .TLR_ReturnDate = Item.TLR_ReturnDate
                        .LastUpdate = GetTime
                        .IsActive = False
                        .ClientId = Nothing
                    End With
                End If

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.ManageTabletStatus
                .Log_Page = "ซ่อม"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

#End Region

#Region "View"

    Public Function GetTabletDetailByCrit(ByVal Item As UseTabletDetailDTO) As t360_uvwTabletDetail() Implements ITabletManager.GetTabletDetailByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM t360_uvwTabletDetail WHERE  {PartType} {PartCrit} {PartClass}"

            .LockWhere = True
            If Item.Condition_Check_Name IsNot Nothing Then
                .ApplyTextPart("PartType", "NOT Check_Name IS NULL")
            End If
            If Item.Condition_UseOutClass IsNot Nothing Then
                .ApplyTextPart("PartType", "NOT Use_OutClassName IS NULL")
            End If
            If Item.Condition_ReadOutClass IsNot Nothing Then
                .ApplyTextPart("PartType", "NOT Use_OutClassName IS NULL AND Use_OutClassType = " & EnUseTabletType.ReadBook)
            End If
            If Item.Condition_Test IsNot Nothing Then
                .ApplyTextPart("PartType", "Score_Type = " & enScoreType.TestInClass)
            End If
            If Item.Condition_Homework IsNot Nothing Then
                .ApplyTextPart("PartType", "Score_Type = " & enScoreType.Homework)
            End If

            Dim f As New SqlPart("AND")
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            f.AddPart("Student_Code={0}", Item.Student_Code)
            f.AddPart("Student_Id={0}", Item.Student_Id)
            f.AddPart("Student_Code LIKE {0}", Item.Student_Code_Like.FusionText("%"))
            f.AddPart("Student_FirstName={0}", Item.Student_FirstName)
            f.AddPart("Student_FirstName LIKE {0}", Item.Student_FirstName_Like.FusionText("%"))
            f.AddPart("Student_LastName={0}", Item.Student_LastName)
            If Item.Start_Time IsNot Nothing AndAlso Item.End_Time Is Nothing Then
                f.AddPart("Start_Time={0}", Item.Start_Time)
            End If
            If Item.Start_Time Is Nothing AndAlso Item.End_Time IsNot Nothing Then
                f.AddPart("End_Time={0}", Item.End_Time)
            End If
            If Item.Start_Time IsNot Nothing AndAlso Item.End_Time IsNot Nothing Then
                f.AddPart("Start_Time >= {0}", Item.Start_Time)
                f.AddPart("End_Time <= {0}", Item.End_Time)
            End If
            .ApplySqlPart("PartCrit", f)

            Dim WhereClass = " Class_Room IN (" & Item.Class_Room & ")"
            .ApplyTextPart("PartClass", WhereClass, Item.Class_Room, WordLink:="AND")

            Return .DataContextExecuteObjects(Of t360_uvwTabletDetail)().ToArray
        End With

    End Function

    Public Function GetTabletDetailSummaryByCrit(ByVal Item As UseTabletDetailDTO) As UseTabletDetailDTO() Implements ITabletManager.GetTabletDetailSummaryByCrit
        With GetLinqToSql
            .MainSql = "SELECT ROW_NUMBER() OVER(ORDER BY Student_Code DESC) as RowNumber, School_Code, Student_Code, Student_PrefixName, Student_FirstName, Student_LastName, Class_Name, Room_Name, " &
                            "COUNT(Check_Name) AS Count_AllCheckName , " &
                            "COUNT(CASE WHEN Check_Name=1 THEN 1 END) AS Count_CheckName , " &
                            "CONVERT(decimal(5, 2), (CONVERT(decimal(5, 2), COUNT(CASE WHEN Check_Name = 1 THEN 1 END)) /CASE WHEN COUNT(Check_Name)=0 THEN 1 ELSE COUNT(Check_Name)  END)*100) AS Percent_CheckName, " &
                            "COUNT(Use_OutClassType) as Count_UseOutClass,  " &
                            "SUM(CASE WHEN not Use_OutClassType is null THEN DiffMin END) as SumMin_UseOutClass, " &
                            "SUM(CASE WHEN not Use_OutClassType is null THEN DiffMin END)/60 as Hr_UseOutClass, " &
                            "SUM(CASE WHEN not Use_OutClassType is null THEN DiffMin END)%60 as Mn_UseOutClass, " &
                            "COUNT(CASE WHEN Use_OutClassType=1 THEN 1 END) as Count_ReadOutClass, " &
                            "SUM(CASE WHEN Use_OutClassType=1 THEN DiffMin END) as SumMin_ReadOutClass, " &
                            "SUM(CASE WHEN Use_OutClassType=1 THEN DiffMin END)/60 as Hr_ReadOutClass, " &
                            "SUM(CASE WHEN Use_OutClassType=1 THEN DiffMin END)%60 as Mn_ReadOutClass, " &
                            "SUM(CASE WHEN Score_Type=1 THEN Score_Full END) as Sum_ScoreFullTest, " &
                            "SUM(CASE WHEN Score_Type=1 THEN Score_Available END) as Sum_ScoreAvailableTest, " &
                            "CONVERT(decimal(5,2),(SUM(CASE WHEN Score_Type=1 THEN Score_Available END)/SUM(CASE WHEN Score_Type=1 THEN Score_Full END))*100) as Percent_Test, " &
                            "SUM(CASE WHEN Score_Type=2 THEN Score_Full END) as Sum_ScoreFullHomework, " &
                            "SUM(CASE WHEN Score_Type=2 THEN Score_Available END) as Sum_ScoreAvailableHomework, " &
                            "CONVERT(decimal(5,2),(SUM(CASE WHEN Score_Type=2 THEN Score_Available END)/SUM(CASE WHEN Score_Type=2 THEN Score_Full END))*100) as Percent_Homework " &
                            "FROM t360_uvwTabletDetail WHERE {F} {PartClass} GROUP BY School_Code, Student_Code, Student_PrefixName, Student_FirstName, Student_LastName, Class_Name, Room_Name"

            Dim f As New SqlPart()
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            f.AddPart("Student_Code={0}", Item.Student_Code)
            f.AddPart("Student_Code LIKE {0}", Item.Student_Code_Like.FusionText("%"))
            f.AddPart("Student_FirstName={0}", Item.Student_FirstName)
            f.AddPart("Student_FirstName LIKE {0}", Item.Student_FirstName_Like.FusionText("%"))
            f.AddPart("Student_LastName={0}", Item.Student_LastName)
            f.AddPart("Student_LastName LIKE {0}", Item.Student_LastName_Like.FusionText("%"))
            f.AddPart("Class_Name={0}", Item.Class_Name)
            f.AddPart("Room_Name={0}", Item.Room_Name)
            If Item.Start_Time IsNot Nothing AndAlso Item.End_Time Is Nothing Then
                f.AddPart("Start_Time={0}", Item.Start_Time)
            End If
            If Item.Start_Time Is Nothing AndAlso Item.End_Time IsNot Nothing Then
                f.AddPart("End_Time={0}", Item.End_Time)
            End If
            If Item.Start_Time IsNot Nothing AndAlso Item.End_Time IsNot Nothing Then
                f.AddPart("Start_Time >= {0}", Item.Start_Time)
                f.AddPart("End_Time <= {0}", Item.End_Time)
            End If
            .ApplySqlPart("F", f)

            Dim WhereClass As String = ""
            If Item.Class_Room IsNot Nothing Then
                .LockWhere = True
                If .CountSqlPart > 0 Then
                    WhereClass &= " AND "
                End If
                WhereClass &= " Class_Room IN (" & Item.Class_Room & ")"
            End If
            .ApplyTextPart("PartClass", WhereClass)

            Return .DataContextExecuteObjects(Of UseTabletDetailDTO)().ToArray
        End With

    End Function

    Public Function GetTabletStatusAll() As TabletStatusDTO() Implements ITabletManager.GetTabletStatusAll
        With GetLinqToSql
            .MainSql = "EXEC spTabletStatus @School_Code=" & UserConfig.GetCurrentContext.School_Code
            Return .DataContextExecuteObjects(Of TabletStatusDTO)().ToArray
        End With

    End Function

    Public Function GetFoundTabletByCrit(ByVal Item As uvwFoundTablet) As uvwFoundTablet() Implements ITabletManager.GetFoundTabletByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM uvwFoundTablet WHERE {f}"

            Dim f As New SqlPart()
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            .ApplySqlPart("f", f)

            Return .DataContextExecuteObjects(Of uvwFoundTablet)().ToArray
        End With

    End Function

    Public Function GetPercentClassByCrit(ByVal Item As uvwPercentClass) As uvwPercentClass() Implements ITabletManager.GetPercentClassByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM uvwPercentClass WHERE {f}"

            Dim f As New SqlPart()
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            .ApplySqlPart("f", f)

            Return .DataContextExecuteObjects(Of uvwPercentClass)().ToArray
        End With

    End Function

    Public Function GetStatisticReportByCrit(ByVal Item As uvwStatisticReport) As uvwStatisticReport() Implements ITabletManager.GetStatisticReportByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM uvwStatisticReport WHERE {f}"

            Dim f As New SqlPart()
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            .ApplySqlPart("f", f)

            Return .DataContextExecuteObjects(Of uvwStatisticReport)().ToArray
        End With

    End Function

    Public Function GetUseLocalByCrit(ByVal Item As uvwUseLocation) As uvwUseLocation() Implements ITabletManager.GetUseLocalByCrit
        With GetLinqToSql
            .MainSql = "SELECT * FROM uvwUseLocation WHERE {f}"

            Dim f As New SqlPart()
            f.AddPart("School_Code={0}", UserConfig.GetCurrentContext.School_Code)
            .ApplySqlPart("f", f)

            Return .DataContextExecuteObjects(Of uvwUseLocation)().ToArray
        End With

    End Function

#End Region

#Region "Lab"

    Public Function GetAllTabletLabFull(SchoolCode As String) As tblTabletLab() Implements ITabletManager.GetAllTabletLabFull
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Dim l As New System.Data.Linq.DataLoadOptions
                l.LoadWith(Of tblTabletLab)(Function(q) q.tblTabletLabDesks)
                Ctx.LoadOptions = l

                Return (From q In Ctx.tblTabletLabs Where q.School_Code = SchoolCode And q.IsActive = True).ToArray
            End Using
        End With
    End Function

    Public Function GetTabletLabById(TabletLabId As Guid) As tblTabletLab Implements ITabletManager.GetTabletLabById
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Return (From q In Ctx.tblTabletLabs Where q.TabletLab_Id = TabletLabId).SingleOrDefault
            End Using
        End With
    End Function

    Public Function GetTabletLabFullById(TabletLabId As Guid) As tblTabletLab Implements ITabletManager.GetTabletLabFullById
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Dim l As New System.Data.Linq.DataLoadOptions
                l.LoadWith(Of tblTabletLab)(Function(q) q.tblTabletLabDesks)
                l.LoadWith(Of tblTabletLabDesk)(Function(q) q.t360_tblTablet)
                Ctx.LoadOptions = l

                Dim Data = (From q In Ctx.tblTabletLabs Where q.TabletLab_Id = TabletLabId).SingleOrDefault
                Dim DeskNotActives = Data.tblTabletLabDesks.Where(Function(q) q.IsActive = False).ToArray
                For Each Row In DeskNotActives
                    Data.tblTabletLabDesks.Remove(Row)
                Next

                Return Data
            End Using
        End With
    End Function

    Public Function ValidateDuplicateLab(LabName As String, SchoolCode As String, Optional Id As Guid? = Nothing) As Boolean Implements ITabletManager.ValidateDuplicateLab
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                If Id Is Nothing Then
                    Dim Found = (From q In Ctx.tblTabletLabs Where q.School_Code = SchoolCode And q.TabletLab_Name = LabName And q.IsActive = True).SingleOrDefault
                    Return (Found Is Nothing)
                Else
                    Dim Found = (From q In Ctx.tblTabletLabs Where q.School_Code = SchoolCode And q.TabletLab_Name = LabName And q.IsActive = True And q.TabletLab_Id <> Id).SingleOrDefault
                    Return (Found Is Nothing)
                End If
            End Using
        End With
    End Function

    ''' <summary>
    ''' ถ้าเจอในห้องเดียวกันที่ส่งมาคืนค่าช่องว่าง ถ้าเจอห้องอื่นคืนชื่อห้องที่เจอ
    ''' </summary>
    ''' <param name="SerialNumber"></param>
    ''' <param name="LabName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckLabNameByTabletSerialNumber(SerialNumber As String, LabName As String) As String Implements ITabletManager.CheckLabNameByTabletSerialNumber
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Dim Tablet = Ctx.t360_tblTablets.Where(Function(q) q.Tablet_SerialNumber = SerialNumber And q.Tablet_IsActive = True).SingleOrDefault
                Dim LabDesk = (From q In Ctx.tblTabletLabDesks Where q.Tablet_Id = Tablet.Tablet_Id And q.IsActive = True).SingleOrDefault
                If LabDesk Is Nothing Then
                    Return ""
                Else
                    If LabDesk.tblTabletLab.TabletLab_Name = LabName Then
                        Return ""
                    Else
                        Return LabDesk.tblTabletLab.TabletLab_Name
                    End If
                End If
            End Using
        End With
    End Function

    Public Function IsFoundTabletInLabname(SerialNumber As String, LabName As String) As Boolean Implements ITabletManager.IsFoundTabletInLabname
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                Dim Tablet = Ctx.t360_tblTablets.Where(Function(q) q.Tablet_SerialNumber = SerialNumber And q.Tablet_IsActive = True).SingleOrDefault
                Dim LabDesk = (From q In Ctx.tblTabletLabDesks Where q.Tablet_Id = Tablet.Tablet_Id And q.IsActive = True).SingleOrDefault
                If LabDesk Is Nothing Then
                    Return False
                Else
                    If LabDesk.tblTabletLab.TabletLab_Name = LabName Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End Using
        End With
    End Function

    Public Function InsertTabletLab(Item As tblTabletLab) As Boolean Implements ITabletManager.InsertTabletLab
        Try
            Using Ctx = GetLinqToSql.GetDataContext


                Ctx.tblTabletLabs.InsertOnSubmit(Item)

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim LogDetail As New StringBuilder
            LogDetail.Append("ห้องแล็บ- ชื่อ:")
            LogDetail.Append(Item.TabletLab_Name)
            LogDetail.Append(",tblTabletLab.Id=")
            LogDetail.Append(Item.TabletLab_Id.ToString)

            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.Insert
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = LogDetail.ToString
            End With
            UserManager.InsertLog(ItemLog)
        End Try

    End Function

    Public Function UpDateTabletLab(Item As tblTabletLab) As Boolean Implements ITabletManager.UpDateTabletLab
        Dim LogDetail As New StringBuilder
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Ori As tblTabletLab
                Using ctx1 = GetLinqToSql.GetDataContext()
                    Ori = ctx1.tblTabletLabs.Where(Function(q) q.TabletLab_Id = Item.TabletLab_Id).SingleOrDefault
                    Dim AllTldIdActive = Ori.tblTabletLabDesks.Where(Function(q) q.IsActive = True).Select(Function(q) q.TLD_Id).ToArray

                    'ลบ labdesk
                    For Each Id In AllTldIdActive
                        Dim Found = (Item.tblTabletLabDesks.Where(Function(q) q.TLD_Id = Id).SingleOrDefault IsNot Nothing)
                        If Not Found Then
                            Dim Desk = Ctx.tblTabletLabDesks.Where(Function(q) q.TLD_Id = Id).SingleOrDefault
                            Dim DeskType As String
                            If Desk.Player_Type = 1 Then
                                DeskType = "ครู"
                            Else
                                DeskType = "นักเรียน"
                            End If

                            LogDetail.Append("แท็บเลตในห้องแล็บ -ห้อง: ")
                            LogDetail.Append(Item.TabletLab_Name)
                            LogDetail.Append(" -โต๊ะ: ")
                            LogDetail.Append(Desk.DeskName)
                            LogDetail.Append(" ,tblTabletLabDesk.Id=")
                            LogDetail.Append(Desk.TLD_Id.ToString)
                            UserManager.Log(EnLogType.ImportantDelete, LogDetail.ToString)
                            LogDetail.Clear()

                            Desk.LastUpdate = Now
                            Desk.IsActive = False
                            Desk.ClientId = Nothing
                        End If

                    Next

                    For Each Desk In Item.tblTabletLabDesks
                        Desk.IsActive = True
                        Desk.ClientId = Nothing
                        Dim OriDesk As tblTabletLabDesk
                        Using ctx2 = GetLinqToSql.GetDataContext()
                            OriDesk = ctx2.tblTabletLabDesks.Where(Function(q) q.TLD_Id = Desk.TLD_Id).SingleOrDefault
                        End Using
                        If OriDesk Is Nothing Then
                            'เพิ่ม labdesk
                            Ctx.tblTabletLabDesks.InsertOnSubmit(Desk)
                        Else
                            'แก้ labdesk

                            Dim DeskType As String
                            If Desk.Player_Type = 1 Then
                                DeskType = "ครู"
                            Else
                                DeskType = "นักเรียน"
                            End If
                            Ctx.tblTabletLabDesks.Attach(Desk, OriDesk)

                            LogDetail.Append("แท็บเลตในห้องแล็บ -ห้อง: ")
                            LogDetail.Append(Item.TabletLab_Name)
                            LogDetail.Append(" -โต๊ะ: ")
                            LogDetail.Append(Desk.DeskName)
                            LogDetail.Append(" -เปลี่ยนสถานะเป็น: ")
                            LogDetail.Append(DeskType)
                            LogDetail.Append(" ,tblTabletLabDesk.Id=")
                            LogDetail.Append(Desk.TLD_Id.ToString)
                            UserManager.Log(EnLogType.ImportantUpdate, LogDetail.ToString)
                            LogDetail.Clear()
                        End If
                    Next

                    Ctx.tblTabletLabs.Attach(Item, Ori)
                End Using


                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'Dim LogDetail As New StringBuilder
            'LogDetail.Append("แท็บเลตในห้องแล็บ")
            'Dim ItemLog As New t360_tblLog
            'With ItemLog
            '    .Log_Type = 
            '    .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            '    .Log_Description = "ห้องแล็บ"
            'End With

        End Try
    End Function

    Public Function DeleteTabletLab(TabletLab_Id As Guid) As Boolean Implements ITabletManager.DeleteTabletLab
        Dim Logdetail As New StringBuilder
        Try
            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx = GetLinqToSql.GetDataContext

                Dim DataTabletLab = Ctx.tblTabletLabs.Where(Function(q) q.TabletLab_Id = TabletLab_Id).SingleOrDefault

                Logdetail.Append("ห้องแล็บ -ชื่อ: ")
                Logdetail.Append(DataTabletLab.TabletLab_Name)
                Logdetail.Append(" ,t360_tblTabletLab.Id=")
                Logdetail.Append(DataTabletLab.TabletLab_Id.ToString)
                DataTabletLab.IsActive = False
                DataTabletLab.LastUpdate = GetTime
                DataTabletLab.ClientId = Nothing

                For Each Row In DataTabletLab.tblTabletLabDesks
                    Row.IsActive = False
                    Row.LastUpdate = GetTime
                    Row.ClientId = Nothing
                Next

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            UserManager.Log(EnLogType.Delete, Logdetail.ToString)
        End Try
    End Function

    Public Function InsertTabletLabDesk(Item As tblTabletLabDesk) As Boolean Implements ITabletManager.InsertTabletLabDesk
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Ctx.tblTabletLabDesks.InsertOnSubmit(Item)

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        End Try
    End Function

    Public Function UpDateTabletLabDesk(Item As tblTabletLabDesk) As Boolean Implements ITabletManager.UpDateTabletLabDesk
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim Ori As tblTabletLabDesk
                Using ctx1 = GetLinqToSql.GetDataContext()
                    Ori = ctx1.tblTabletLabDesks.Where(Function(q) q.TLD_Id = Item.TLD_Id).SingleOrDefault
                End Using
                Ctx.tblTabletLabDesks.Attach(Item, Ori)

                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            Dim ItemLog As New t360_tblLog
            With ItemLog
                .Log_Type = EnLogType.ImportantUpdate
                .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
                .Log_Description = "แท็บเลตในห้องเรียน"
            End With
            UserManager.InsertLog(ItemLog)
        End Try
    End Function

    Public Function GetTabletLabByCrit(Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTabletLabByCrit
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                '.MainSql = " SELECT t360_tblTablet.Tablet_SerialNumber, tblTabletLab.TabletLab_Name + ' / โต๊ะ ' + cast(tblTabletLabDesk.DeskName as varchar) as FullName, " & _
                '            "MAX(tblLog.LastUpdate) AS TLG_TimeStamp, tblTabletLabDesk.LastUpdate as TON_ReceiveDate " & _
                '            "FROM tblTabletLab INNER JOIN tblTabletLabDesk ON tblTabletLab.TabletLab_Id = tblTabletLabDesk.TabletLab_Id " & _
                '            "INNER JOIN t360_tblTablet ON tblTabletLabDesk.Tablet_Id = t360_tblTablet.Tablet_Id " & _
                '            "LEFT OUTER JOIN tblQuizSession on t360_tblTablet.Tablet_Id = tblQuizSession.tablet_Id " & _
                '            "INNER JOIN tblLog ON tblQuizSession.Player_Id = tblLog.UserId " & _
                '            "WHERE {F} GROUP BY tblTabletLabDesk.DeskName, tblTabletLab.TabletLab_Name, tblTabletLabDesk.LastUpdate, t360_tblTablet.Tablet_SerialNumber "
                .MainSql = " SELECT t360_tblTablet.Tablet_Id as TON_Id, t360_tblTablet.Tablet_Id,t360_tblTablet.AssetNo ,t360_tblTablet.Tablet_SerialNumber, tblTabletLab.TabletLab_Name + ' / โต๊ะ ' + cast(tblTabletLabDesk.DeskName as varchar) as FullName, " &
                            " tblTabletLabDesk.LastUpdate as TON_ReceiveDate,''  " &
                            " FROM tblTabletLab INNER JOIN tblTabletLabDesk ON tblTabletLab.TabletLab_Id = tblTabletLabDesk.TabletLab_Id " &
                            " INNER JOIN t360_tblTablet ON tblTabletLabDesk.Tablet_Id = t360_tblTablet.Tablet_Id "

                Dim f As New SqlPart
                f.AddPart("tbltabletlab.school_code={0}", Item.School_Code.ToString.TextOrNothing)
                f.AddPart("AssetNo LIKE {0}", Item.AssetNo.FusionText("%"))
                f.AddPart("tblTabletLab.TabletLab_Id={0}", Item.TabletLab_Id.ToString.TextOrNothing)
                'f.AddPart("t360_tblTabletOwner.TabletOwner_IsActive = {0}", Item.TabletOwner_IsActive)
                f.AddPart("t360_tblTablet.Tablet_IsActive = {0}", Item.Tablet_IsActive)
                .ApplySqlPart("F", f)

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With
    End Function


#End Region

#Region "SpareTablet"

    Public Function GetTabletSpareByCrit(Item As TabletOwnerDTO) As TabletOwnerDTO() Implements ITabletManager.GetTabletSpareByCrit
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                '.MainSql = " SELECT t360_tblTablet.Tablet_SerialNumber, t360_tblTablet.Tablet_TabletName as FullName, " & _
                '            "MAX(tblLog.LastUpdate) AS TLG_TimeStamp, t360_tblTablet.Tablet_LastUpdate as TON_ReceiveDate " & _
                '            "FROM tblLog INNER JOIN tblQuizSession ON tblLog.UserId = tblQuizSession.Player_Id INNER JOIN " & _
                '            "t360_tblTablet ON tblQuizSession.Tablet_Id = t360_tblTablet.Tablet_Id " & _
                '            "LEFT OUTER JOIN tblTabletLabDesk ON t360_tblTablet.Tablet_Id = tblTabletLabDesk.Tablet_Id " & _
                '            "WHERE t360_tblTablet.Tablet_Id not in (select Tablet_Id from t360_tblTabletOwner where Tablet_Id is not null) " & _
                '            "AND (tblTabletLabDesk.Tablet_Id IS NULL) And {F}  GROUP BY t360_tblTablet.Tablet_SerialNumber, t360_tblTablet.Tablet_TabletName, t360_tblTablet.Tablet_LastUpdate  "

                'Dim f As New SqlPart
                'f.AddPart("t360_tbltablet.school_code={0}", Item.School_Code.ToString.TextOrNothing)
                'f.AddPart("Tablet_SerialNumber LIKE {0}", Item.Tablet_SerialNumber.FusionText("%"))
                'f.AddPart("t360_tbltablet.Tablet_TabletName LIKE {0}", Item.Tablet_TabletName.FusionText("%"))
                ''f.AddPart("t360_tblTabletOwner.TabletOwner_IsActive = {0}", Item.TabletOwner_IsActive)
                'f.AddPart("t360_tblTablet.Tablet_IsActive = {0}", Item.Tablet_IsActive)

                '.ApplySqlPart("F", f)


                Dim sql As New StringBuilder()
                'sql.Append(" SELECT t.Tablet_Id,t.AssetNo, t.Tablet_SerialNumber, t.Tablet_TabletName as FullName, l.LastUpdate AS TLG_TimeStamp, t.Tablet_LastUpdate as TON_ReceiveDate FROM (")
                'sql.Append(" SELECT Tablet_Id,Tablet_SerialNumber,Tablet_TabletName,Tablet_LastUpdate,AssetNo ")
                'sql.Append(" FROM t360_tblTablet WHERE Tablet_IsActive = 1 AND School_Code = '{0}' ")
                'sql.Append(" AND Tablet_IsOwner = 0 AND Tablet_Id NOT IN (SELECT Tablet_Id FROM tblTabletLabDesk WHERE IsActive = 1)) AS t ")
                'sql.Append(" LEFT JOIN tblQuizSession q ON t.Tablet_Id = q.Tablet_Id ")
                'sql.Append(" LEFT JOIN tblLog l ON q.Player_Id = l.UserId ORDER BY l.LastUpdate DESC;")


                sql.Append(" SELECT t.Tablet_Id as TON_Id,  t.Tablet_Id,t.AssetNo, t.Tablet_SerialNumber, t.Tablet_TabletName AS FullName, ")
                sql.Append(" MAX(q.LastUpdate) AS TLG_TimeStamp, t.Tablet_LastUpdate AS TON_ReceiveDate,''  ")
                sql.Append(" FROM t360_tblTablet t LEFT JOIN tblQuizSession q ON t.Tablet_Id = q.Tablet_Id ")
                'sql.Append(" INNER JOIN tblLog l ON tblQuizSession.Player_Id = l.UserId ")
                sql.Append(" WHERE t.Tablet_IsActive = 1 AND t.School_Code = '{0}' AND t.Tablet_IsOwner = 0 ")
                sql.Append(" AND t.Tablet_Id NOT IN (SELECT Tablet_Id FROM tblTabletLabDesk WHERE IsActive = 1) ")
                sql.Append(" GROUP BY t.Tablet_Id,t.AssetNo, t.Tablet_SerialNumber, t.Tablet_TabletName,t.Tablet_LastUpdate; ")


                .MainSql = String.Format(sql.ToString(), Item.School_Code.ToString.TextOrNothing)
                .LockWhere = True

                Return .DataContextExecuteObjects(Of TabletOwnerDTO)(Ctx).ToArray
            End Using
        End With
    End Function

#End Region

#Region "Tablet AssetNo"
    Public Function GetOwnerNameByTablet(Item As t360_tblTablet, tabletType As Integer) As t360_tblTabletStatusDetail Implements ITabletManager.GetOwnerNameByTablet
        With GetLinqToSql
            Using Ctx = .GetDataContext()
                'Select Case tabletType
                '    Case 1
                '        Dim tabletOwner = (From q In Ctx.t360_tblTabletOwners Where q.Tablet_Id = Item.Tablet_id And q.TabletOwner_IsActive = True).SingleOrDefault
                '        If tabletOwner IsNot Nothing Then
                '            If tabletOwner.Owner_Type = 1 Then
                '                Dim teacher = (From q In Ctx.t360_tblTeachers Where q.Teacher_id = tabletOwner.Owner_Id).SingleOrDefault
                '                Return teacher.Teacher_FirstName & " " & teacher.Teacher_LastName
                '            End If
                '            Dim student = (From q In Ctx.t360_tblStudents Where q.Student_Id = tabletOwner.Owner_Id).SingleOrDefault
                '            Return student.Student_FirstName & " " & student.Student_LastName
                '        End If

                '    Case 2
                '        Dim tabletLab = (From q In Ctx.tblTabletLabs Join qq In Ctx.tblTabletLabDesks On qq.TabletLab_Id Equals q.TabletLab_Id Where qq.Tablet_Id = Item.Tablet_id Select q).SingleOrDefault
                '        Return tabletLab.TabletLab_Name

                '    Case 3
                '        Return "ไม่มีเจ้าของ(เครื่องสำรอง)"
                'End Select

                'Return ""
                Dim TabletStatus = (From q In Ctx.t360_tblTabletStatusDetails Where q.Tablet_Id = Item.Tablet_Id And q.IsActive = True).SingleOrDefault
                If TabletStatus IsNot Nothing Then
                    Return TabletStatus
                End If
            End Using
        End With
    End Function

    Public Function UpdateTabletAssetNo(AssetNo As String, TabletId As Guid) As Boolean Implements ITabletManager.UpdateTabletAssetNo

        Dim LogDetail As New StringBuilder
        Try
            Using Ctx = GetLinqToSql.GetDataContext
                Dim tablet = (From q In Ctx.t360_tblTablets Where q.Tablet_Id = TabletId And q.Tablet_IsActive = True).SingleOrDefault
                tablet.AssetNo = AssetNo
                tablet.LastUpdate = Now
                Ctx.SubmitChanges()
            End Using
            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'Dim LogDetail As New StringBuilder
            'LogDetail.Append("แท็บเลตในห้องแล็บ")
            'Dim ItemLog As New t360_tblLog
            'With ItemLog
            '    .Log_Type = 
            '    .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            '    .Log_Description = "ห้องแล็บ"
            'End With
        End Try
    End Function
#End Region


    Public Function GetMobileDirectorRegister(DeviceId As String) As Boolean Implements ITabletManager.GetMobileDirectorRegister
        Using Ctx = GetLinqToSql.GetDataContext
            If (From q In Ctx.tblMobileRegistrations Where q.Device_Id = DeviceId And q.IsActive = True).SingleOrDefault Is Nothing Then
                Return False
            End If
            Return True
        End Using
    End Function

    Public Function UpdateTabletStatus(TabletStatusDetail As t360_tblTabletStatusDetail) As Boolean Implements ITabletManager.UpdateTabletStatus

        Dim LogDetail As New StringBuilder
        Try
            'Update Row เดิม
            Using Ctx = GetLinqToSql.GetDataContext
                Dim tabletoldStatus = (From q In Ctx.t360_tblTabletStatusDetails Where q.Tablet_Id = TabletStatusDetail.Tablet_Id And q.IsActive = True).SingleOrDefault
                If tabletoldStatus Is Nothing Then
                    tabletoldStatus = TabletStatusDetail
                End If
                tabletoldStatus.IsActive = False
                tabletoldStatus.LastUpdate = Now
                tabletoldStatus.ClientId = Nothing

                Dim tablet = (From q In Ctx.t360_tblTablets Where q.Tablet_Id = TabletStatusDetail.Tablet_Id).SingleOrDefault()
                If tablet IsNot Nothing Then
                    tablet.Tablet_IsOwner = False
                    Dim tabletName As String = ""
                    If TabletStatusDetail.TSD_Status = EnTabletStatus.TabletReturn Then
                        tabletName = "ไม่มีคนใช้งาน (รับคืน)"
                    ElseIf TabletStatusDetail.TSD_Status = EnTabletStatus.Damage Then
                        tabletName = "ไม่มีคนใช้งาน (รับคืนเสียหายใช้งานได้)"
                    ElseIf TabletStatusDetail.TSD_Status = EnTabletStatus.Collapse Then
                        tabletName = "ไม่มีคนใช้งาน (รับคืนเสียหายใช้งานไม่ได้)"
                    ElseIf TabletStatusDetail.TSD_Status = EnTabletStatus.SendToFix Then
                        tabletName = "ไม่มีคนใช้งาน (ส่งซ่อม)"
                    ElseIf TabletStatusDetail.TSD_Status = EnTabletStatus.Lost Then
                        tabletName = "ไม่มีคนใช้งาน (สูญหาย)"
                    End If
                    tablet.Tablet_TabletName = tabletName
                End If


                Dim tabletOwner = (From q In Ctx.t360_tblTabletOwners Where q.Tablet_Id = TabletStatusDetail.Tablet_Id And q.TabletOwner_IsActive = True).SingleOrDefault()
                If tabletOwner IsNot Nothing Then
                    tabletOwner.TabletOwner_IsActive = False
                Else
                    Dim tabletLabDesk = (From q In Ctx.tblTabletLabDesks Where q.Tablet_Id = TabletStatusDetail.Tablet_Id And q.IsActive = True).SingleOrDefault()
                    tabletLabDesk.IsActive = False
                End If

                Ctx.SubmitChanges()
            End Using

            Dim System As New Service.ClsSystem(New ClassConnectSql())
            Dim GetTime = System.GetThaiDate

            Using Ctx2 = GetLinqToSql.GetDataContext()


                TabletStatusDetail.LastUpdate = GetTime
                TabletStatusDetail.ClientId = Nothing
                TabletStatusDetail.School_Code = UserConfig.GetCurrentContext.School_Code
                TabletStatusDetail.IsActive = True

                Ctx2.t360_tblTabletStatusDetails.InsertOnSubmit(TabletStatusDetail)
                Ctx2.SubmitChanges()
            End Using


            Return True
        Catch ex As Exception
            ElmahExtension.LogToElmah(ex)
            Return False
        Finally
            'Dim LogDetail As New StringBuilder
            'LogDetail.Append("แท็บเลตในห้องแล็บ")
            'Dim ItemLog As New t360_tblLog
            'With ItemLog
            '    .Log_Type = 
            '    .Log_Page = HttpContext.Current.Request.Url.AbsoluteUri
            '    .Log_Description = "ห้องแล็บ"
            'End With
        End Try

        Return True
    End Function

    Public Function GetTabletHistory(TabletId As String) As t360_tblTabletStatusDetail() Implements ITabletManager.GetTabletHistory
        With GetLinqToSql
            .MainSql = " SELECT * FROM t360_tblTabletStatusDetail WHERE Tablet_Id = '" & TabletId & "' ORDER BY LastUpdate DESC "
            .LockWhere = True
            Return .DataContextExecuteObjects(Of t360_tblTabletStatusDetail)().ToArray
        End With
    End Function

    Public Function GetTabletUseInToDay() As Integer Implements ITabletManager.GetTabletUseInToDay
        With GetLinqToSql
            .MainSql = " SELECT * FROM tblLog WHERE LogType = 20 AND School_Code = '" & UserConfig.GetCurrentContext.School_Code & "' AND LastUpdate BETWEEN CAST(CAST(GETDATE() AS DATE) AS DATETIME) " &
                        "AND DATEADD(DAY, 1,  CAST(CAST(GETDATE() AS DATE) AS DATETIME)); "
            .LockWhere = True
            Return .DataContextExecuteObjects(Of tblLog)().ToArray.Count
        End With
    End Function

    Public Function GetTabletUseInLastWeek() As Integer Implements ITabletManager.GetTabletUseInLastWeek
        With GetLinqToSql
            .MainSql = " SELECT * FROM tblLog WHERE LogType = 20 AND School_Code = '" & UserConfig.GetCurrentContext.School_Code & "' " &
                        " And LastUpdate BETWEEN DATEADD(DAY, (1-DATEPART(WEEKDAY,  CAST(CAST(GETDATE() As Date) As DATETIME))) - 7,  CAST(CAST(GETDATE() As Date) As DATETIME)) " &
                        " AND DATEADD(DAY, ((1-DATEPART(WEEKDAY,  CAST(CAST(GETDATE() AS DATE) AS DATETIME))) - 7) + 7,  CAST(CAST(GETDATE() AS DATE) AS DATETIME)); "
            .LockWhere = True
            Return .DataContextExecuteObjects(Of tblLog)().ToArray.Count
        End With
    End Function

    Public Function GetDamagedTablet() As DataTable Implements ITabletManager.GetDamagedTablet
        With GetLinqToSql
            .MainSql = " SELECT s.Student_CurrentClass,COUNT(s.Student_CurrentClass) AS tabletAmount FROM t360_tblTabletStatusDetail td  " &
                        " INNER JOIN t360_tblTabletOwner tw ON td.Tablet_Id = tw.Tablet_Id INNER JOIN t360_tblStudent s ON tw.Owner_Id = s.Student_Id  " &
                        " WHERE td.TSD_Status IN (3,4,5) AND td.IsActive = 1 AND td.School_Code = '" & UserConfig.GetCurrentContext.School_Code & "' GROUP BY s.Student_CurrentClass; "
            .LockWhere = True
            Return .DataContextExecuteObjects(Of DataTable)()
        End With
    End Function
End Class



