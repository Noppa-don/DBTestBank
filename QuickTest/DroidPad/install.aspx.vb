Imports System.Web.Script.Serialization
Imports KnowledgeUtils.IO
Imports System.Web
Imports KnowledgeUtils

Public Class install
    Inherits System.Web.UI.Page
    Dim redis As New RedisStore()
    ''' <summary>
    ''' เป็น API ที่ทำงานร่วมกับ Corona ในขั้นตอนการลงทะเบียนต่างๆ โดยรับ method มาแยกอีกทีว่าจะทำขั้นตอนไหน
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ClsSecurity.CheckConnectionIsSecure()

        If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
            Try
                KNConfigData.LoadData()
            Catch ex As Exception
            End Try
        End If

        If Not Page.IsPostBack Then
            Dim methodName As String = Request.Form("method")
            If Not IsNothing(methodName) Then
                'พารามิเตอร์ของ Method School
                'รหัสเครื่อง Tablet
                Dim DeviceUniqueID As String = Request.Form("DeviceUniqueID")
                'รหัสโรงเรียน
                Dim SchoolID As String = Request.Form("SchoolID")
                'รหัสลับที่กรอกเข้ามา
                Dim SchoolPassword As String = Request.Form("SchoolPassword")
                'พารามิเตอร์ของ Method Teacher
                'ชื่อ
                Dim FirstName As String = Request.Form("FirstName")
                'นามสกุล
                Dim LastName As String = Request.Form("LastName")
                'ชั้นที่ประจำชั้น
                Dim TeacherClass As String = Request.Form("Class")
                'ห้องที่ประจำชั้น
                Dim Room As String = Request.Form("Room")
                'วิชาที่สอน
                Dim Subject As String = Request.Form("Subject")
                If IsNothing(Room) Then Room = "" 'ให้เป็นค่าว่าง เพราะไม่ใช่ required field
                'พารามิเตอร์ของ Method Student
                'ชั้น
                Dim StudentClass As String = Request.Form("Class")
                'รหัสประจำตัวนักเรียน
                Dim StudentCode As String = Request.Form("StudentCode")
                'เลขที่
                Dim NumberInRoom As String = Request.Form("NumberInRoom")
                'เพศ
                Dim Gender As String = Request.Form("Gender")

                If HttpContext.Current.Application("EnableDetailLog") Is Nothing Then
                    HttpContext.Current.Application("EnableDetailLog") = CBool(ConfigurationManager.AppSettings("EnableDetailLog")) 'เก็บ Log ตอน Install Tablet
                End If

                ClsLog.CheckFileLog()

                'ลงทะเบียน Tablet กับ รร.
                If methodName.ToLower() = "registerwithschool" Then
                    ClsLog.Record(" - registerwithschool : param = " & DeviceUniqueID & "," & SchoolID & "," & SchoolPassword)

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
                    'ย้าย Tablet ไปผูกกับ รร. ใหม่
                    ClsLog.Record(" - movetonewschool : param = " & DeviceUniqueID & "," & SchoolID & "," & SchoolPassword)

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
                    'ลงทะเบียนครู
                    ClsLog.Record(" - registerteacher : param = " & DeviceUniqueID & "," & FirstName & "," & LastName & "," & TeacherClass & "," & Room & "," & Subject)

                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(FirstName) And Not IsNothing(LastName) And Not IsNothing(TeacherClass) And Not IsNothing(Room) And Not IsNothing(Subject) Then
                        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass <> "" And Subject <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""TeacherInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.GetTeacherInfo(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)
                            If ReturnValue <> "-1" Then redis.DEL(DeviceUniqueID & "_Register")
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "movetonewteacher" Then
                    'ย้าย Tablet มาผูกกับครูคนใหม่
                    ClsLog.Record(" - movetonewteacher : param = " & DeviceUniqueID & "," & FirstName & "," & LastName & "," & TeacherClass & "," & Room & "," & Subject)

                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(FirstName) And Not IsNothing(LastName) And Not IsNothing(TeacherClass) And Not IsNothing(Room) And Not IsNothing(Subject) Then
                        If DeviceUniqueID <> "" And FirstName <> "" And LastName <> "" And TeacherClass <> "" And Subject <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""TeacherInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.MoveToNewTeacher(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)

                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "registerstudent" Then
                    'ลงทะเบียนนักเรียน
                    ClsLog.Record(" - registerstudent : param = " & DeviceUniqueID & "," & StudentClass & "," & Room & "," & StudentCode & "," & NumberInRoom & "," & Gender)

                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If DeviceUniqueID <> "" And TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" And Gender <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ClsLog.Record(" - registerstudent : param = " & DeviceUniqueID & "," & FirstName & "," & LastName)
                            ReturnValue = ClsDroidPad.GetStudentInfo(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "movetonewstudent" Then
                    'ย้าย Tablet มาผูกกับนักเรียนคนใหม่
                    ClsLog.Record(" - movetonewstudent : param = " & DeviceUniqueID & "," & StudentClass & "," & Room & "," & StudentCode & "," & NumberInRoom & "," & Gender)

                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If DeviceUniqueID <> "" And TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.MoveToNewStudent(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "registerduplicatestudent" Then
                    'ลงทะเบียนนักเรียนใหม่ โดยอัพเดทให้คนเก่าที่มีรหัสนักเรียน เลขที่เหมือนกันในเทอมปัจจุบันให้ isactive = 0
                    ClsLog.Record(" - registerduplicatestudent : param = " & DeviceUniqueID & "," & StudentClass & "," & Room & "," & StudentCode & "," & NumberInRoom & "," & Gender)

                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If DeviceUniqueID <> "" And TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.AddDuplicateStudent(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "changetablet" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If DeviceUniqueID <> "" And TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.ChangeTablet(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender, False)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "changetablethasowner" Then
                    If Not IsNothing(DeviceUniqueID) And Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If DeviceUniqueID <> "" And TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.ChangeTablet(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender, True)
                            Response.Write(ReturnValue)
                            Response.End()
                        End If
                    End If
                ElseIf methodName.ToLower() = "newstudent" Then
                    If Not IsNothing(StudentClass) And Not IsNothing(Room) And Not IsNothing(StudentCode) And Not IsNothing(NumberInRoom) And Not IsNothing(Gender) Then
                        If TeacherClass <> "" And Room <> "" And StudentCode <> "" And NumberInRoom <> "" And Gender <> "" Then
                            Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
                            Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
                            ReturnValue = ClsDroidPad.NewStudentTest(FirstName, LastName, SchoolID, StudentCode, StudentClass, Room, NumberInRoom, Gender)
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

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="SchoolID"></param>
    ''' <param name="SchoolPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function RegisterWithSchool(ByVal DeviceUniqueID As String, ByVal SchoolID As String, ByVal SchoolPassword As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""RegistrationInfo"" : ""WRONGPASSWORD""}}"
        ReturnValue = ClsDroidPad.GetRegistrationInfo(DeviceUniqueID, SchoolID, SchoolPassword)
        Return ReturnValue

    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="SchoolID"></param>
    ''' <param name="SchoolPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function MoveToNewSchool(ByVal DeviceUniqueID As String, ByVal SchoolID As String, ByVal SchoolPassword As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""Result"" : ""0""}}"
        ReturnValue = ClsDroidPad.MovetoNewSchool(DeviceUniqueID, SchoolID, SchoolPassword)
        Return ReturnValue
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="TeacherClass"></param>
    ''' <param name="Room"></param>
    ''' <param name="Subject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function RegisterTeacher(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""TeacherInfo"" : """"}}"
        ReturnValue = ClsDroidPad.GetTeacherInfo(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)
        Return ReturnValue
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="TeacherClass"></param>
    ''' <param name="Room"></param>
    ''' <param name="Subject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function MoveToNewTeacher(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal TeacherClass As String, ByVal Room As String, ByVal Subject As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""TeacherInfo"" : """"}}"
        ReturnValue = ClsDroidPad.MoveToNewTeacher(DeviceUniqueID, FirstName, LastName, TeacherClass, Room, Subject)
        Return ReturnValue
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="StudentClass"></param>
    ''' <param name="Room"></param>
    ''' <param name="StudentCode"></param>
    ''' <param name="NumberInRoom"></param>
    ''' <param name="Gender"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function RegisterStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String, ByVal Gender As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
        ReturnValue = ClsDroidPad.GetStudentInfo(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender)
        Return ReturnValue
    End Function

    ''' <summary>
    ''' NA
    ''' </summary>
    ''' <param name="DeviceUniqueID"></param>
    ''' <param name="FirstName"></param>
    ''' <param name="LastName"></param>
    ''' <param name="StudentClass"></param>
    ''' <param name="Room"></param>
    ''' <param name="StudentCode"></param>
    ''' <param name="NumberInRoom"></param>
    ''' <param name="Gender"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod()>
    Public Shared Function MoveToNewStudent(ByVal DeviceUniqueID As String, ByVal FirstName As String, ByVal LastName As String, ByVal StudentClass As String, ByVal Room As String, ByVal StudentCode As String, ByVal NumberInRoom As String, ByVal Gender As String)
        Dim ClsDroidPad As New ClassDroidPad(New ClassConnectSql)
        Dim ReturnValue As String = "{""Param"": {""StudentInfo"" : """"}}"
        ReturnValue = ClsDroidPad.MoveToNewStudent(DeviceUniqueID, FirstName, LastName, StudentClass, Room, StudentCode, NumberInRoom, Gender)
        Return ReturnValue
    End Function


End Class