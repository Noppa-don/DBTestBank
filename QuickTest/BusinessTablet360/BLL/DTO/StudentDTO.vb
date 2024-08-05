Public Class StudentDTO

    Public Property School_Code As String
    Public Property Student_Id() As Guid?
    Public Property Student_PrefixName() As String
    Public Property Student_FirstName() As String
    Public Property Student_LastName() As String
    Public Property Student_Code() As String
    Public Property Student_CurrentClass() As String
    Public Property Student_CurrentRoom As String
    Public Property Student_CurrentRoomId As Guid?
    Public Property Student_CurrentNoInRoom As Byte?
    Public Property Student_FatherName() As String
    Public Property Student_FatherPhone() As String
    Public Property Student_MotherName() As String
    Public Property Student_MotherPhone() As String
    Public Property Student_NickName() As String
    Public Property Student_Status() As Byte?
    Public Property Student_IsActive() As Boolean?
    Public Property Class_Name() As String
    Public Property Room_Id() As Guid?
    Public Property Room_Name() As String
    Public Property Student_Number As String
    Public Property Student_No As Integer?
    Public Property Student_Soi As String
    Public Property Student_Street As String
    Public Property SubDistrict_Id As Integer?
    Public Property District_Id As Integer?
    Public Property Province_Id As Integer?
    Public Property RowNumber As Integer?
    Public Property RoomConvert As String
    Public Property Student_NoInRoom As Byte?
    Public Property FromUpLevel As Integer?
    Public Property Calendar_Id As Guid?
    Public Property Student_FatherPhone2 As String
    Public Property UserName As String
    Public Property Password As String

    Public Sub New()
        'ระบุค่า Default
        'Student_Status = EnStudentStatus.Study
        Student_IsActive = EnIsActiveStatus.Active
    End Sub

End Class


