Imports System.Data.SqlClient
Imports KnowledgeUtils
Public Class Student

    Private _StudentId As String
    Public Property StudentId() As String
        Get
            Return _StudentId
        End Get
        Set(ByVal value As String)
            _StudentId = value
        End Set
    End Property

    Private _RoomId As String
    Public Property RoomId() As String
        Get
            Return _RoomId
        End Get
        Set(ByVal value As String)
            _RoomId = value
        End Set
    End Property

    Private _ClassName As String
    Public Property ClassName() As String
        Get
            Return _ClassName
        End Get
        Set(ByVal value As String)
            _ClassName = value
        End Set
    End Property

    Private _RoomName As String
    Public Property RoomName() As String
        Get
            Return _RoomName
        End Get
        Set(ByVal value As String)
            _RoomName = value
        End Set
    End Property


    Private _SchoolId As String
    Public Property SchooldId() As String
        Get
            Return _SchoolId
        End Get
        Set(ByVal value As String)
            _SchoolId = value
        End Set
    End Property

    Private _DB As New ClassConnectSql()

    Public Sub New(ByRef DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim dtStudentDetail As DataTable = GetStudentDetail(DeviceId, InputConn)
        If dtStudentDetail.Rows.Count() > 0 Then
            _StudentId = dtStudentDetail.Rows(0)("Student_Id").ToString()
            _RoomId = dtStudentDetail.Rows(0)("Room_Id").ToString()
            _ClassName = dtStudentDetail.Rows(0)("ClassName").ToString()
            _RoomName = dtStudentDetail.Rows(0)("RoomName").ToString()
            _SchoolId = dtStudentDetail.Rows(0)("School_Code").ToString()
        End If
    End Sub

    Public Sub New(ByRef DeviceId As String, IsLab As Boolean, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim dtStudentDetail As DataTable = GetStudentLabDetail(DeviceId, InputConn)
        If dtStudentDetail.Rows.Count() > 0 Then
            _StudentId = dtStudentDetail.Rows(0)("Player_Id").ToString()
            _RoomId = dtStudentDetail.Rows(0)("TabletLab_Id").ToString()
            _ClassName = ""
            _RoomName = ""
            _SchoolId = dtStudentDetail.Rows(0)("School_Code").ToString()
        End If
    End Sub

    Private Function GetStudentDetail(ByRef DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        Dim dtTablet As DataTable = GetTabId(DeviceId, InputConn)
        If dtTablet.Rows.Count() > 0 Then
            Dim Tablet_Id As String = dtTablet.Rows(0)("tablet_id").ToString
            Dim tablet_Isowner As String = dtTablet.Rows(0)("Tablet_IsOwner").ToString
            '---
            sql = " Select t360_tblStudent.School_Code, t360_tblStudent.Student_CurrentClass as ClassName, t360_tblStudent.Student_CurrentRoom as RoomName, t360_tblStudent.Student_Id,r.Room_Id"
            sql &= " FROM t360_tblTabletOwner INNER JOIN"
            sql &= " t360_tblStudent ON t360_tblTabletOwner.Owner_Id = t360_tblStudent.Student_Id"
            sql &= " INNER JOIN t360_tblRoom r ON t360_tblStudent.School_Code = r.School_Code AND t360_tblStudent.Student_CurrentClass = r.Class_Name "
            sql &= " AND REPLACE(t360_tblStudent.Student_CurrentRoom,'/','') = REPLACE(r.Room_Name,'/','') AND t360_tblStudent.School_Code = r.School_Code "
            sql &= " where Tablet_Id = '" & Tablet_Id & "' and TabletOwner_IsActive = '1';"
            GetStudentDetail = _DB.getdata(sql, , InputConn)
        Else
            Return New DataTable
        End If
    End Function

    Private Function GetStudentLabDetail(ByRef DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim sql As String
        Dim dtTablet As DataTable = GetTabId(DeviceId, InputConn)
        If dtTablet.Rows.Count > 0 Then
            Dim Tablet_Id As String = dtTablet.Rows(0)("tablet_id").ToString
            Dim redis As New RedisStore()
            Dim q As Quiz = redis.Getkey(Of Quiz)(DeviceId)
            '--
            sql = " SELECT qs.School_Code, qs.Player_Id, t.TabletLab_Id "
            sql &= " FROM tblQuizSession qs INNER JOIN tblTabletLabDesk t ON  qs.Tablet_Id = t.Tablet_Id "
            sql &= " WHERE qs.Tablet_Id = '" & Tablet_Id & "' AND qs.Quiz_Id = '" & q.QuizId & "';"
            GetStudentLabDetail = _DB.getdata(sql, , InputConn)
        Else
            Return New DataTable
        End If
    End Function

    Private Function GetTabId(ByRef DeviceId As String, Optional ByRef InputConn As SqlConnection = Nothing)
        Dim sql As String = " SELECT tablet_id,Tablet_IsOwner from t360_tblTablet where Tablet_SerialNumber = '" & DeviceId & "'  and Tablet_IsActive = 1; "
        Dim dt As DataTable = _DB.getdata(sql, , InputConn)
        Return dt
    End Function

    Public Function GetLoginMaxonet(UserName As String, Password As String) As DataTable
        Dim sql As String = "select KCU_DeviceId,KCU_Token,KCU_ExpireDate from t360_tblStudent s 
                             inner join maxonet_tblKeyCodeUsage kcu on s.Student_Id = kcu.KCU_OwnerId
                             where UserName = '" & UserName & "' and Password = '" & Password & "' and kcu_type = '0' 
                             and s.Student_IsActive = 1 and kcu.KCU_IsActive = 1"
        Dim dt As DataTable = _DB.getdata(sql)
        Return dt
    End Function

End Class


