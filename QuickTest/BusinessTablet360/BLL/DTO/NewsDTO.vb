Public Class NewsDTO

    Public Property School_Code() As String
    Public Property News_Information() As String
    Public Property News_Announcer() As String
    Public Property News_StartDate() As Date?
    Public Property News_EndDate() As Date?
    Public Property News_ToStudent() As Boolean?
    Public Property News_ToTeacher() As Boolean?
    Public Property News_IsActive() As Boolean?
    Public Property News_Id() As Guid?
    Public Property Class_Name As String
    Public Property Room_Name As String

    Public Sub New()
        News_IsActive = True
    End Sub

End Class
