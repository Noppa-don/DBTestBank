Imports System.Text

Public Class StudentCheckMark

    Public Sub New()
        _TermId = 1
        _StudentPassword = "12345"
        _StudentBirth = "dbo.GetThaiDate()" 'Date.Now.ToString("yyyy-mm-dd hh:mm:ss")
    End Sub

    Private _StudentId As String
    Public Property StudentId() As String
        Get
            Return _StudentId
        End Get
        Set(ByVal value As String)
            _StudentId = value
        End Set
    End Property

    Private _StudentFName As String
    Public Property StudentFName() As String
        Get
            Return _StudentFName
        End Get
        Set(ByVal value As String)
            _StudentFName = value
        End Set
    End Property

    Private _StudentLName As String
    Public Property StudentLName() As String
        Get
            Return _StudentLName
        End Get
        Set(ByVal value As String)
            _StudentLName = value
        End Set
    End Property

    Private _StudentSex As String
    Public ReadOnly Property StudentSex() As String
        Get
            Return _StudentSex
        End Get
    End Property
    Private Sub SetStudentSex()
        Select Case _TitleId
            Case 1 Or 4 Or 5
                _StudentSex = "หญิง"
            Case Else
                _StudentSex = "ชาย"
        End Select
    End Sub

    Private _ClassId As Integer
    Public Property ClassId() As Integer
        Get
            Return _ClassId
        End Get
        Set(value As Integer)
            _ClassId = value
        End Set
    End Property
    'Public Sub SetClassId(ByVal value As Integer)
    '    _ClassId = value
    'End Sub

    Private _ClassName As String
    Public Property ClassName() As String
        Get
            Return _ClassName
        End Get
        Set(ByVal value As String)
            _ClassName = value
        End Set
    End Property

    Private _StudentBirth As String
    Public ReadOnly Property StudentBirth() As String
        Get
            Return _StudentBirth
        End Get
        'Set(ByVal value As String)
        '    _StudentBirth = value
        'End Set
    End Property

    Private _StudentPassword As String
    Public ReadOnly Property StudentPassword() As String
        Get
            Return _StudentPassword
        End Get
        'Set(ByVal value As String)
        '    _StudentPassword = value
        'End Set
    End Property

    Private _TermId As Integer 'ใน checkmark มีแค่ 1,2
    Public ReadOnly Property TermId() As Integer
        Get
            Return _TermId
        End Get
    End Property

    Private _TitleId As Integer 'เลข id ของคำนำหน้าชื่อ
    Public ReadOnly Property TitleId() As Integer
        Get
            Return _TitleId
        End Get
    End Property

    Private _StudentPrefixName As String
    Public Property StudentPrefixName() As String
        Get
            Return _StudentPrefixName
        End Get
        Set(ByVal value As String)
            _StudentPrefixName = value
            SetTitleId()
            SetStudentSex()
        End Set
    End Property
    Private Sub SetTitleId()
        Select Case _StudentPrefixName
            Case "เด็กหญิง"
                _TitleId = 1
            Case "ด.ญ."
                _TitleId = 1
            Case "เด็กชาย"
                _TitleId = 2
            Case "ด.ช."
                _TitleId = 2
            Case "นาย"
                _TitleId = 3
            Case "นางสาว"
                _TitleId = 4
            Case "นาง"
                _TitleId = 5
        End Select
    End Sub

    Private _StudentNumber As Integer
    Public Property StudentNumber() As Integer
        Get
            Return _StudentNumber
        End Get
        Set(ByVal value As Integer)
            _StudentNumber = value
        End Set
    End Property

    Private _StudentRoom As Integer
    Public Property StudentRoom() As Integer
        Get
            Return _StudentRoom
        End Get
        Set(ByVal value As Integer)
            _StudentRoom = value
        End Set
    End Property

    Private _SchoolId As String
    Public Property SchoolId() As String
        Get
            Return _SchoolId
        End Get
        Set(ByVal value As String)
            _SchoolId = value
        End Set
    End Property

    Public Function ToStringSQLInsertCheckmark() As String
        Return String.Format("INSERT INTO tblStudent VALUES('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7},{8},{9},{10});",
                                       _StudentId, _StudentFName, _StudentLName, _StudentSex, _ClassId, _StudentBirth,
                                      _StudentPassword, _TermId, _TitleId, _StudentNumber, _StudentRoom)
    End Function

    Public Function ToStringSQLUpdateCheckmark(ByVal StudentOld As StudentCheckMark) As String
        Return String.Format("UPDATE tblStudent SET Student_ID='{0}',Student_FName='{1}',Student_LName='{2}',Student_Sex='{3}',Class_ID='{4}',Title_ID='{5}',Student_Number='{6}',StudentRoom='{7}' WHERE Class_ID = {8} AND StudentRoom = '{9}' AND Student_ID = {10};  ;",
                                      _StudentId, _StudentFName, _StudentLName, _StudentSex, _ClassId,
                                      _TitleId, _StudentNumber, _StudentRoom, StudentOld.ClassId, StudentOld.StudentRoom, StudentOld.StudentId)
    End Function

    Public Function ToStringSQLDeleteCheckmark() As String
        Return String.Format("DELETE tblStudent WHERE Class_ID = {0} AND StudentRoom = '{1}' AND Student_ID = '{2}';", _ClassId, _StudentRoom, _StudentId)
    End Function

    Public Function ToStringSQLInsertT360() As String
        Dim sqlT360 As New StringBuilder()
        sqlT360.Append(String.Format("DECLARE @newstudentId{0} AS uniqueidentifier;  SET @newstudentId{0} =  NEWID();", _StudentNumber))
        sqlT360.Append(String.Format(" INSERT INTO t360_tblStudent VALUES(@newstudentId{2},'{0}','{1}','ไม่ระบุ','นักเรียน','เลขที่ {2}',NULL,NULL,NULL,NULL,NULL,1,{2},'{3}','/{4}',@RoomId,NULL,NULL,NULL,NULL,0,0,0,1,dbo.GetThaiDate(),NULL,NULL,NULL,NULL,NULL,NULL,0);",
                             _SchoolId, _StudentId, _StudentNumber, _ClassName, _StudentRoom))
        sqlT360.Append(GetSQLt360_tblRoom())
        Return sqlT360.ToString()
    End Function

    Public Function ToStringSQLDeleteT360() As String
        Dim sql As New StringBuilder()
        sql.Append(String.Format("DECLARE @studentId{0} AS uniqueidentifier;  SET @studentId{0} = (SELECT Student_Id FROM t360_tblStudent WHERE School_Code = '{1}' AND Student_Code = '{2}' AND Student_CurrentClass = '{3}' AND Student_CurrentRoom = '/{4}' AND Student_CurrentNoInRoom = '{0}' AND Student_IsActive = 1 AND Student_Status = 1);",
                                 _StudentNumber, _SchoolId, _StudentId, _ClassName, _StudentRoom))
        sql.Append(String.Format(" UPDATE t360_tblStudent SET Student_IsActive = 0  WHERE School_Code = '{0}' AND Student_CurrentRoomId = @roomid AND Student_Id = @studentId{1} ;", SchoolId, _StudentNumber))
        sql.Append(String.Format(" UPDATE t360_tblStudentRoom SET SR_IsActive = 0 WHERE School_Code = '{0}' AND Room_Id = @roomid AND Student_Id = @studentId{1};", SchoolId, _StudentNumber))
        Return sql.ToString()
    End Function

    Private Function GetSQLt360_tblRoom() As String
        Return String.Format(" INSERT INTO t360_tblStudentRoom VALUES(NEWID(),@newstudentId{0},'{1}',{0},'{2}','/{3}',dbo.GetThaiDate(),@AcademicYear,8,1,@CalendarId,dbo.GetThaiDate(),@RoomId,NULL);",
                                    _StudentNumber, SchoolId, ClassName, _StudentRoom)
    End Function

    Public Function ToStringSQLUpdateT360() As String
        Return String.Format(" UPDATE t360_tblStudent SET Student_IsActive = 0  WHERE School_Code = '{0}' AND Student_CurrentRoomId = @roomid AND Student_Code = '{1}';", _SchoolId, _StudentId)
    End Function
End Class
