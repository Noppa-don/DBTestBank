Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports KnowledgeUtils
Imports System.Web.Script.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class MaxOnetService
    Inherits System.Web.Services.WebService

    Dim redis As New RedisStore()
    Private js As New JavaScriptSerializer()

    <WebMethod()>
    Public Function RegisterParent(ByVal deviceId As String, ByVal keyCode As String, ByVal username As String) As String
        ClsLog.Record("เริ่มลงทะเบียน deviceId = " & deviceId & " keyCode = " & keyCode & " username = " & username)
        Log.Record(Log.LogType.ManageUser, "เริ่มลงทะเบียน deviceId = " & deviceId & " keyCode = " & keyCode & " username = " & username, True)
        Try
            Dim maxOnet As New MaxOnetManagement()
            maxOnet.DeviceId = deviceId
            maxOnet.KeyCode = keyCode
            maxOnet.UserName = username
            maxOnet.KCU_Type = MaxOnetRegisterType.parent
            Return maxOnet.GetToken()
        Catch ex As Exception
            ClsLog.Record(" ex = " & ex.InnerException.ToString())
            Log.Record(Log.LogType.ManageUser, " ex = " & ex.InnerException.ToString(), True)

            Return String.Format("|@#{0}#@|", "-1")
        End Try
    End Function
    ''' <summary>
    ''' ดึงข้อมูลการเข้าใช้งานของเด็กจาก Redis
    ''' </summary>
    ''' <param name="token">เลข 12 หลัก</param>
    ''' <param name="deviceId">device ของเครื่อง</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Function GetParentReportData(ByVal token As String, deviceId As String) As String
        Log.Record(Log.LogType.ParentRegister, "GetParentReportData_token = " & token & " device = " & deviceId, 0)

        Dim maxOnet As New MaxOnetManagement
        maxOnet.TokenId = token
        Dim parentNewData As String = maxOnet.GetParentReportDataFromRedis()
        Return parentNewData

    End Function


    ''' <summary>
    ''' function สำหรับ get data ของนักเรียน ส่งเป็นรูปแบบ class
    ''' </summary>
    ''' <param name="token"></param>
    ''' <param name="deviceId"></param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function GetMaxonetParentReport(ByVal token As String, deviceId As String) As String
        'Log.Record(Log.LogType.ParentRegister, "GetParentReportData_token = " & token & " device = " & deviceId, 0)
        ClsLog.Record("token = " & token & " , deviceId = " & deviceId)
        Dim maxOnet As New MaxOnetManagement
        maxOnet.TokenId = Guid.Parse(token).ToString()
        Return maxOnet.GetResultData()
    End Function


    ''' <summary>
    ''' Update ข้อมูลที่เด็กเข้าใช้งาน Max-Onet เมื่อวานให้ผู้ปกครอง
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()>
    Public Sub UpdateParentReport()

        Dim maxOnet As New MaxOnetManagement
        maxOnet.UpdateParentReport()

    End Sub

    <WebMethod()>
    Public Function RegisterSubjectMaxOnet(ByVal studentId As String, ByVal studentClass As String, ByVal subjectsId As String) As Boolean

        If HttpContext.Current.Application("RegisterSubjectMaxOnet_" & studentId) IsNot Nothing Then
            Return False
        End If

        Dim sujectList As New List(Of String)(subjectsId.Split(","c))
        Dim maxonet As New MaxOnetManagement()
        maxonet.StudentId = studentId
        maxonet.StudentClass = studentClass
        maxonet.SujectList = sujectList

        If maxonet.SaveStudentSubject() Then
            Dim dialyActivity As New DialyActivityManagement
            dialyActivity.Run(studentId)
            HttpContext.Current.Application("RegisterSubjectMaxOnet_" & studentId) = Nothing
            Return True
        End If

        HttpContext.Current.Application("RegisterSubjectMaxOnet_" & studentId) = Nothing
        Return False

    End Function

    <WebMethod()>
    Public Function RegisterMultiSubjectClassMaxonet(studentId As String, subjectClasslist As String) As Boolean
        Try
            subjectClasslist = subjectClasslist.Replace("sId", "SubjectId").Replace("value", "ClassId")
            Dim js As New JavaScriptSerializer()
            Dim sc As List(Of MaxonetSubjectClassRegister) = js.Deserialize(Of List(Of MaxonetSubjectClassRegister))(subjectClasslist)
            Dim maxonet As New MaxOnetManagement()
            maxonet.StudentId = studentId
            maxonet.StudentClass = GetRandomStudentClass()


            If maxonet.SaveStudentMultiSubjectClass(sc) Then
                ' ปิดการทำงานสร้างกิจกรรมประจำวัน ให้ไปสร้างที่ตอนเลือกจาก dialog แทน
                'Dim dialyActivity As New DialyActivityManagement
                'dialyActivity.RunDialyActivity(studentId)
                Return True
            End If


            Return False
        Catch ex As Exception
            ClsLog.Record("ปัญหาลงทะเบียนไม่ได้ --> " & ex.Message)
            Return False
        End Try
    End Function

    <WebMethod()>
    Public Function AddCreditMaxonet(studentId As String, tokenId As String, deviceId As String, userName As String, password As String) As Integer
        Dim maxonet As New MaxOnetManagement()
        maxonet.StudentId = studentId
        maxonet.TokenId = tokenId
        maxonet.DeviceId = deviceId

        maxonet.UserName = userName
        maxonet.KeyCode = password

        Return maxonet.AddCreditMaxonet()
    End Function

    Private Function GetRandomStudentClass() As String
        Dim r As Random = New Random
        Return String.Format("K{0}", r.Next(4, 15))
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function DisableGetDailyActivities() As Boolean
        HttpContext.Current.Session("DisableGetDailyActivities") = True
        Return True
    End Function

    ''' <summary>
    ''' function ในการ set SubjectId ให้กับ Session เมื่อมีการทดลองการใช้งาน TimeOut บน CI เกินเวลา 30 วินาที ตัว button จะไม่สามารถใช้งานฝั่ง server ได้
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SetChooseSubjectIdToSession(ByVal subjectId As String) As Boolean
        Try
            HttpContext.Current.Session("PSubjectName") = subjectId
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetChooseClassActivities(studentId As String, subjectId As String) As String
        Dim dam As New DialyActivityManagement
        Return dam.CreateDailyByLevelAndSubject(studentId, subjectId)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetUrlActivity(userDialyActivityJson As String) As String
        Dim uda As UserDialyActivity = js.Deserialize(Of UserDialyActivity)(userDialyActivityJson)
        Dim dam As New DialyActivityManagement
        Return dam.GetUrlDialyActivity(uda)
    End Function

    ''' <summary>
    ''' function get score ทั้งหมด ของนักเรียน
    ''' </summary>
    ''' <param name="studentId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetAllScoreData(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim subjectScore As List(Of Object) = m.GetAllScoreData()
        Return js.Serialize(subjectScore)
    End Function

    ''' <summary>
    ''' function get score กิจกรรมประจำวัน
    ''' </summary>
    ''' <param name="studentId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetScoreActivityChartData(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim subjects As List(Of ScoreChart) = m.GetScoreActivityChart()
        Return js.Serialize(subjects)
    End Function

    ''' <summary>
    ''' function get จำนวนการทำกิจกรรมประจำวัน
    ''' </summary>
    ''' <param name="studentId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetQuantityActivityChartData(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of Object) = m.GetQuantityActivityChartData()
        Return js.Serialize(data)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetScorePracticeChartData(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of ScoreChart) = m.GetScorePracticeChart()
        Return js.Serialize(data)
    End Function

    ''' <summary>
    ''' function get จำนวนการทำฝึกฝน
    ''' </summary>
    ''' <param name="studentId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetQuantityPracticeChartData(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of Object) = m.GetQuantityPracticeChartData()
        Return js.Serialize(data)
    End Function

    ''' <summary>
    ''' function สำหรับ หาว่านักเรียนลงทะเบียนวิชาอะไรไปบ้าง
    ''' </summary>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetStudentSubject(studentId As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of StudentSubjects) = m.GetStudentSubjects()
        Return js.Serialize(data)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetScoreActivityByFilter(studentId As String, subjects As String, startDate As String, endDate As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of ScoreChart) = m.GetChartActivityByFilter(subjects, startDate, endDate)
        Return js.Serialize(data)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetScorePracticeByFilter(studentId As String, subjects As String, startDate As String, endDate As String) As String
        Dim m As New MaxOnetManagement
        m.StudentId = studentId
        Dim data As List(Of ScoreChart) = m.GetChartPracticeByFilter(subjects, startDate, endDate)
        Return js.Serialize(data)
    End Function

End Class