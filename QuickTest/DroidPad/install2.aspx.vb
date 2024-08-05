Public Class install2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                Dim DeviceUniqueID As String = Request.Form("DeviceUniqueID")
                Dim SchoolID As String = Request.Form("SchoolID")
                Dim SchoolPassword As String = Request.Form("SchoolPassword")
                Dim FirstName As String = Request.Form("FirstName")
                Dim LastName As String = Request.Form("LastName")
                Dim TeacherClass As String = Request.Form("Class")

                Dim Room As String = Request.Form("Room")
                If IsNothing(Room) Then Room = "" 'ให้เป็นค่าว่าง เพราะไม่ใช่ required field

                Dim Subject As String = Request.Form("Subject")
              
                If methodName.ToLower() = "registerwithschool" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(SchoolID) And Not IsNothing(SchoolPassword) Then
                        If DeviceUniqueID <> "" And SchoolID <> "" And SchoolPassword <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""RegistrationInfo"" : ""WRONGPASSWORD""}}"
                            ReturnValue = ClsDroidPad.GetRegistrationInfo(DeviceUniqueID, SchoolID, SchoolPassword)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "movetonewschool" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(SchoolID) And Not IsNothing(SchoolPassword) Then
                        If DeviceUniqueID <> "" And SchoolID <> "" And SchoolPassword <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : ""0""}}"
                            ReturnValue = ClsDroidPad.MovetoNewSchool(DeviceUniqueID, SchoolID, SchoolPassword)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If

                ElseIf methodName.ToLower() = "registerteacher" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(FirstName) And Not IsNothing(LastName) And Not IsNothing(TeacherClass) And Not IsNothing(Room) And Not IsNothing(Subject) Then
                        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass <> "" And Subject <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : """"}}"
                            ReturnValue = ClsDroidPad.GetTeacherInfo(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If

                ElseIf methodName.ToLower() = "movetonewteacher" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(FirstName) And Not IsNothing(LastName) And Not IsNothing(TeacherClass) And Not IsNothing(Room) And Not IsNothing(Subject) Then
                        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass <> "" And Subject <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""Result"" : """"}}"
                            ReturnValue = ClsDroidPad.MoveToNewTeacher(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)

                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If

                End If
            End If
            Response.Write("-1")
            Response.End()
        End If
    End Sub
 
 

    '<Services.WebMethod()>
    'Public Shared Function RegisterStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String)
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""Result"" : """"}}"
    '    ReturnValue = ClsDroidPad.GetStudentInfo(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom)
    '    Return ReturnValue
    'End Function

    '<Services.WebMethod()>
    'Public Shared Function MoveToNewStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String)
    '    Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
    '    Dim ReturnValue As String = "{""Param"": {""Result"" : """"}}"
    '    ReturnValue = ClsDroidPad.MoveToNewStudent(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom)
    '    Return ReturnValue
    'End Function

End Class