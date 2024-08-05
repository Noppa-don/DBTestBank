Imports KnowledgeUtils.IO

Public Class ClsSelectSession

    'Dim res As String = ResolveUrl("~").ToLower()
    Private UserId As String
    Private SchoolId As String
    Private selectedSession As String
    Private ArrApplication As New ArrayList
    'Private Selected As String
    'Private Arr As New ArrayList
    Private Def As Boolean = True

    Public Sub New()
        Try
            UserId = HttpContext.Current.Session("UserId").ToString()
            SchoolId = HttpContext.Current.Session("SchoolID").ToString()
        Catch ex As Exception
            'Throw New Exception("Session 'UserId' และ Session 'SchoolID' ไม่มีค่า ตอน New ClassSelectSession")
            HttpContext.Current.Response.Redirect("~/LoginPage.aspx", False)
        End Try
    End Sub

    Public Sub New(u As String, s As String, se As String, a As ArrayList)
        UserId = u
        SchoolId = s
        selectedSession = se
        ArrApplication = a
        Def = False
    End Sub

    Private Sub setValueInClass()
        If Not HttpContext.Current.Session("selectedSession") Is Nothing Then
            selectedSession = HttpContext.Current.Session("selectedSession").ToString()
        End If
        If Not HttpContext.Current.Application(UserId) Is Nothing Then
            ArrApplication = HttpContext.Current.Application(UserId)
        End If
    End Sub


    'Public Sub New(t As String)
    '    Dim ListSessionInfo As New List(Of ClsSessionInFo)
    '    Dim a As New ArrayList
    '    a = HttpContext.Current.Application(UserId)
    '    For Each s In a
    '        ListSessionInfo.Add(s)
    '    Next
    'End Sub

    'Get Arr object
    Private Function GetObjArrApplication() As Object
        If Def Then
            setValueInClass()
        End If

        'HttpContext.Current.Application.Lock()
        For i As Integer = 0 To ArrApplication.Count - 1
            Dim objArrApplication = ArrApplication.Item(i)
            If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
                'HttpContext.Current.Application.UnLock()
                Return objArrApplication
                Exit For
            End If
        Next

    End Function

    'หาหน้าปัจจุบันจากการเช็ค webconfig "Runmode"
    Public Function GetCurrentPageFromRunMode() As String
        Dim CurrentPage As String = ""
        Dim runMode As String = ClsKNSession.RunMode
        If runMode <> "" Then
            If runMode = "wordonly" Then

                If HttpContext.Current.Session("UserCheckExamType") = "certify" Then
                    CurrentPage = "testset/step2.aspx"

                Else
                    CurrentPage = "PrintTestset/DashboardPrintTestsetPage.aspx"
                End If
            ElseIf runMode = "twotests" Then
                CurrentPage = "PrintTestset/DashboardPrintTestsetPage.aspx"
            ElseIf runMode = "standalonenotablet" Or runMode = "studenttablet" Then
                CurrentPage = "Quiz/DashboardQuizPage.aspx"
                'ElseIf HttpContext.Current.Application("runmode").ToString.ToLower() = "studenttablet" Then
                '    CurrentPage = "quiz/dashboardquizpage.aspx"
                'ElseIf HttpContext.Current.Application("runmode").ToString.ToLower() = "labonly" Then
                '    CurrentPage = "quiz/dashboardquizpage.aspx"
            ElseIf runMode = "full" Or runMode = "labonly" Then
                CurrentPage = "Student/DashboardStudentPage.aspx"
            End If
        End If

        Log.Record(Log.LogType.Login, "FirstPage AfterLogin : " & CurrentPage, True)
        Return CurrentPage
    End Function

#Region "New Session"
    Public Sub NewSession(ByVal InputPage As String)
        'Dim ArrData As New ArrayList
        Dim objClsSessInFo As New ClsSessionInFo
        Dim pkGuid = Number4Digit()
        'Dim CurrentPage As String = "student/dashboardstudentpage.aspx"
        Dim CurrentPage As String = InputPage.ToLower()
        objClsSessInFo.PKInfo = pkGuid
        objClsSessInFo.TimeStamp = DateTime.Now
        objClsSessInFo.CurrentPage = CurrentPage
        objClsSessInFo.CurrentQuerystring = ""
        objClsSessInFo.SchoolId = SchoolId
        If Not HttpContext.Current.Application(UserId) Is Nothing Then
            ArrApplication = HttpContext.Current.Application(UserId)
        End If
        ArrApplication.Add(objClsSessInFo)
        HttpContext.Current.Session("selectedSession") = pkGuid
        HttpContext.Current.Application(UserId) = ArrApplication
        SetCalendarId()
    End Sub
#End Region

#Region "CheckSession TimeOut"
    Public Function IsHaveSession() As Boolean
        setValueInClass()
        Dim IsHave As Boolean = False
        If Not ArrApplication Is Nothing Then
            Dim ArrNewApp As New ArrayList
            Dim timeOut As Integer = CInt(HttpContext.Current.Application("ClassSessionIdleTimeout"))
            'Loop เพื่อหาว่ามี Session ไหนที่น้อยกว่า 120 นาทีแล้วมั่งเอาไปใส่ใน Array อันใหม่
            For i = 0 To ArrApplication.Count - 1
                If DateTime.Now > ArrApplication.Item(i).TimeStamp Then
                    If DateDiff(DateInterval.Minute, ArrApplication.Item(i).TimeStamp, DateTime.Now) < timeOut Then
                        ArrNewApp.Add(ArrApplication(i))
                    End If
                End If
            Next
            'เช็คจำนวนแถวที่เหลืออยู่ใน Array
            If ArrNewApp.Count > 0 Then
                HttpContext.Current.Application(UserId) = ArrNewApp
                IsHave = True
            Else
                'HttpContext.Current.Application(UserId) = Nothing
                HttpContext.Current.Application.Remove(UserId)
            End If
        End If
        IsHaveSession = IsHave
    End Function
#End Region

#Region "Set TimeStamp"
    Public Sub SetTimeStamp()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.TimeStamp = DateTime.Now
    End Sub
#End Region


    Public Sub setOnUnload()
        setValueInClass()
        HttpContext.Current.Application.Lock()
        For i = 0 To ArrApplication.Count - 1
            Dim objArrApplication = ArrApplication.Item(i)
            If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
                objArrApplication.OnUnload = True
                Exit For
            End If
        Next
        HttpContext.Current.Application.UnLock()
    End Sub
    Public Function getOnUnload() As Boolean
        setValueInClass()
        HttpContext.Current.Application.Lock()
        For i = 0 To ArrApplication.Count - 1
            Dim objArrApplication = ArrApplication.Item(i)
            If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
                getOnUnload = objArrApplication.OnUnload
                Exit For
            End If
        Next
        HttpContext.Current.Application.UnLock()
    End Function

#Region "Set/Get CurrentPage"
    Public Sub SetCurrentPage(ByVal CurrentPage As String)
        'Dim objArrApplication As Object = GetObjArrApplication()
        'objArrApplication.CurrentPage = CurrentPage.ToLower()
        'objArrApplication.TimeStamp = DateTime.Now
    End Sub
    Public Function GetCurrentPage() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetCurrentPage = objArrApplication.CurrentPage
    End Function
#End Region
#Region "Set/Get CurrentQuerystring"
    Public Sub SetCurrentQuerystring(ByVal CurrentQuerystring As String)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.CurrentQuerystring = CurrentQuerystring
    End Sub
    Public Function GetCurrentQuerystring() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetCurrentQuerystring = objArrApplication.CurrentQuerystring
    End Function
#End Region

#Region "Set/Get objTestset"
    Public Sub SetObjTestset()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.objTestset = HttpContext.Current.Session("objTestSet")
    End Sub
    Public Function GetObjTestset() As Object
        Dim objArrApplication As Object = GetObjArrApplication()
        GetObjTestset = objArrApplication.objTestset
    End Function
#End Region

#Region "Set/Get ClassName"
    Public Sub SetClassName(c As String)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.ClassName = c
    End Sub
    Public Function GetClassName() As Object
        Dim objArrApplication As Object = GetObjArrApplication()
        Return objArrApplication.ClassName
    End Function
#End Region

#Region "Set/Get EditId"
    Public Sub SetEditId()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.EditId = HttpContext.Current.Session("EditId")
    End Sub
    Public Function GetEditId() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetEditId = objArrApplication.EditId
    End Function
#End Region

#Region "Set/Get StartTimer"
    Public Sub SetStartTimer()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.StarTimer = HttpContext.Current.Session("StartTime")
    End Sub
    Public Function GetStartTimer() As Boolean
        Dim objArrApplication As Object = GetObjArrApplication()
        GetStartTimer = objArrApplication.StarTimer
    End Function
#End Region

#Region "Set/Get t"
    Public Sub SetT()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.timerQuiz = HttpContext.Current.Session("t")
    End Sub
    Public Function GetT() As System.Timers.Timer
        Dim objArrApplication As Object = GetObjArrApplication()
        GetT = objArrApplication.timerQuiz
    End Function
#End Region

#Region "OnRecieved"
    Public Sub SetOnRecieved(ByVal CmdRecieved As String)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.EditId = CmdRecieved
    End Sub
    Public Function GetOnRecieved() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetOnRecieved = objArrApplication.OnRecieved
    End Function
#End Region

    Public Property TestsetId() As String
        Get
            Dim s As Object = GetObjArrApplication()
            Return s.TestsetId.ToString()
        End Get
        Set(ByVal value As String)
            Dim s As Object = GetObjArrApplication()
            s.TestsetId = value
        End Set
    End Property


#Region "QuizId"
    ' Set QuizId
    Public Sub SetSessionQuizId(ByVal QuizId As String)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.QuizId = QuizId
    End Sub
    ' Get QuizId
    Public Function GetSessionQuizId() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetSessionQuizId = objArrApplication.QuizId
    End Function
#End Region

#Region "ChooseMode"
    ' Set Mode
    Public Sub SetSessionChooseMode(ByVal ChooseMode As String)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.ChooseMode = ChooseMode
    End Sub
    ' Get Mode
    Public Function GetSessionChooseMode() As String
        Dim objArrApplication As Object = GetObjArrApplication()
        GetSessionChooseMode = objArrApplication.ChooseMode
    End Function
#End Region

#Region "QuizUseTablet"
    ' Set QuizUseTablet
    Public Sub SetSessionQuizUseTablet(ByVal QuizUseTablet As Boolean)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.QuizUseTablet = QuizUseTablet
    End Sub
    ' Get QuizUseTablet
    Public Function GetSessionQuizUseTablet()
        Dim objArrApplication As Object = GetObjArrApplication()
        GetSessionQuizUseTablet = objArrApplication.QuizUseTablet
    End Function
#End Region

#Region "Set/Get PClassId"
    Public Sub SetPClassId()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.PClassId = HttpContext.Current.Session("PClassId")
    End Sub
    Public Function GetPClassId() As Object
        Dim objArrApplication As Object = GetObjArrApplication()
        GetPClassId = objArrApplication.PClassId
    End Function
#End Region

#Region "Set/Get PSubjectName"
    Public Sub SetPSubjectName()
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.PSubjectName = HttpContext.Current.Session("PSubjectName")
    End Sub
    Public Function GetPSubjectName() As Object
        Dim objArrApplication As Object = GetObjArrApplication()
        GetPSubjectName = objArrApplication.PSubjectName
    End Function
#End Region

#Region "ExamNum"
    ' Set ExamNum
    Public Sub SetExamNum(ByVal ExamNum As Integer)
        Dim objArrApplication As Object = GetObjArrApplication()
        objArrApplication.ExamNum = ExamNum
    End Sub
    ' Get Mode
    Public Function GetExamNum() As Integer
        Dim objArrApplication As Object = GetObjArrApplication()
        GetExamNum = objArrApplication.ExamNum
    End Function
#End Region

#Region "Random Number Pk SelectedSession"
    Private Function randomNumber4Digit() As String
        Dim rand As New Random()
        Dim num As Integer = rand.Next(1000, 9999)
        Return num
    End Function

    Public Function Number4Digit() As String
        Dim arrApplication As New ArrayList
        Dim numberPass As Boolean = False 'check ว่า ตัวเลขที่ random มาผ่านมั้ย
        Dim newRand As Boolean = False ' check ว่า มีการ random เลขใหม่หรือเปล่า 
        Dim number As String
        number = randomNumber4Digit()
        Dim g As Guid ' เอาเช็คว่าชื่อ application เป็น guid หรือเปล่า
        'Dim f As New ManageFile
        'Dim fpath As String = "C:\temp\logGen4digit.txt"
        'f.CreateFile(fpath, " ----- ", True)
        'f.CreateFile(fpath, " Login @ " & Date.Today.ToString(), True)
        If HttpContext.Current.Application.Count > 0 Then
            HttpContext.Current.Application.Lock()
            While (Not (numberPass))
                ' loop ชื่อ app ทั้งหมด
                For Each allApp In HttpContext.Current.Application.AllKeys
                    ' check ว่า application มีคำว่า Sess และ Quiz อยู่หรือเปล่า เพราะจะถือว่าไม่ใช้ application selectedSession
                    ' If allApp.ToString().IndexOf("Sess") = -1 And allApp.ToString().IndexOf("Quiz") = -1 And allApp.ToString() <> "DictData" And allApp.ToString() <> "UnityContainer" And allApp.ToString().IndexOf("|") = -1 And allApp.ToString().IndexOf("Calendar") = -1 And allApp.ToString().IndexOf("tab") = -1 And allApp.ToString().IndexOf("Review") = -1 Then
                    If Guid.TryParse(allApp.ToString(), g) Then ' เช็คว่า name เป็น guid หรือเปล่า
                        'f.CreateFile(fpath, "Application เป็น Guid = " & allApp.ToString(), True)
                        If Not HttpContext.Current.Application(allApp.ToString()) Is Nothing Then ' ถ้า application ไม่เป็นค่าว่าง
                            'f.CreateFile(fpath, "Application ไม่ nothing ", True)
                            arrApplication = HttpContext.Current.Application(allApp.ToString())
                            'If TypeOf arrApplication Is ArrayList Then 'check type ของ appp
                            For i = 0 To arrApplication.Count - 1
                                If TypeOf arrApplication.Item(i) Is ClsSessionInFo Then
                                    'f.CreateFile(fpath, "Type เป็น ClsSessionInFo ", True)
                                    Dim objArrApplication As ClsSessionInFo = arrApplication.Item(i)
                                    If (objArrApplication.PKInfo.ToString() = number) Then
                                        number = randomNumber4Digit()
                                        numberPass = False
                                        newRand = True
                                        Exit For
                                    Else
                                        numberPass = True
                                    End If
                                End If
                            Next
                            'End If
                        End If
                        ' check ว่ามีการ random เลขใหม่หรือเปล่า
                        If (newRand) Then
                            newRand = False
                            Exit For
                        End If
                    Else
                        numberPass = True
                    End If
                Next
            End While
            HttpContext.Current.Application.UnLock()
        End If
        Number4Digit = number
    End Function
#End Region

#Region "Get ScreenName"
    Public Function ScreenName(ByVal url As String) As String
        'Dim quizMode As Boolean = HttpContext.Current.Application("NeedQuizMode")
        url = url.ToLower()
        Select Case url
            'Case "testset/step1.aspx"
            '    ScreenName = "ขั้น 1: จัดชุดควิซ"
            Case "testset/step2.aspx"
                ScreenName = "จัดชุดข้อสอบ(เลือกชั้น/ห้อง)"
            Case "testset/step3.aspx"
                ScreenName = "จัดชุดข้อสอบ(เลือกหน่วย)"
            Case "testset/step4.aspx"
                ScreenName = "จัดชุดข้อสอบ(บันทึก)"
            Case "activity/settingactivity.aspx"
                ScreenName = "ตั้งค่าควิซ"
            Case "testset/genpdf.aspx"
                ScreenName = "ดู/โหลด ใบงาน"
            Case "activity/activitypage.aspx"
                ScreenName = "ทำควิซ"
            Case "activity/reviewmostwronganswer.aspx"
                ScreenName = "ทบทวนข้อผิด"
            Case "student/dashboardstudentpage.aspx"
                ScreenName = "หน้าหลัก นักเรียน"
            Case "quiz/dashboardquizpage.aspx"
                ScreenName = "หน้าหลัก ควิซ"
            Case "homework/dashboardhomeworkpage.aspx"
                ScreenName = "หน้าหลัก การบ้าน"
            Case "practice/dashboardpracticepage.aspx"
                ScreenName = "หน้าหลัก ฝึกฝน"
            Case "printtestset/dashboardprinttestsetpage.aspx"
                ScreenName = "หน้าหลัก ใบงาน"
            Case "testset/dashboardsetuppage.aspx"
                ScreenName = "หน้าหลัก จัดชุดข้อสอบ"
            Case "activity/alternativepage.aspx"
                ScreenName = "สิ้นสุดการควิซ"
            Case "module/homeworkassignpage.aspx"
                ScreenName = "สั่งการบ้าน"
            Case "teacher/teacherstudentdetailpage.aspx"
                ScreenName = "ดูประวัตินักเรียน"
            Case "student/studentlistpage.aspx"
                ScreenName = "ดูนักเรียนในห้อง"
            Case "student/homewoknowandhistorypage.aspx"
                ScreenName = "ดูประวัติการบ้าน"
            Case "practicemode_pad/chooseclass.aspx"
                ScreenName = "จัดชุดมาตรฐาน(เลือกชั้น)"
            Case "practicemode_pad/choosesubject.aspx"
                ScreenName = "จัดชุดมาตรฐาน(เลือกวิชา)"
            Case "practicemode_pad/choosequestionset.aspx"
                ScreenName = "จัดชุดมาตรฐาน(เลือกหน่วย)"
            Case Else
                ScreenName = "Unknown"
        End Select

    End Function
#End Region

#Region "Calendar" 'get calendar from date now 
    Public Function GetCalendarID(ByVal SchoolID As String) As DataTable
        Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "' AND IsActive = 1; "
        Dim db As New ClassConnectSql()
        Dim dt As New DataTable
        dt = db.getdata(sql)
        If dt.Rows.Count > 0 Then
            GetCalendarID = dt
        Else
            'sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE School_Code = '" & SchoolID & "' AND Calendar_Type = 3 " & _
            '      " ORDER BY Calendar_Year DESC, Calendar_Name DESC "
            sql = " SELECT TOP 1 * FROM dbo.t360_tblCalendar WHERE Calendar_Type = '3' AND School_Code = '" & SchoolID & "' " &
                  " AND dbo.GetThaiDate() >= Calendar_ToDate AND IsActive = 1 ORDER BY Calendar_ToDate DESC; "
            dt.Clear()
            dt = db.getdata(sql)
            If dt.Rows.Count = 0 Then
                Throw New ArgumentException("ไม่มี Calendar หรือ Getdate น้อยกว่า Calendar_Fromdate ที่น้อยที่สุด , SchoolCode = " & SchoolID)
            Else
                Return dt
            End If
        End If

    End Function

    Public Sub SetCalendarId()
        'Session CalendarID,CalendarName (Cuurent,Selected)
        Dim KnSession As New KNAppSession()
        If KnSession.StoredValue("CurrentCalendarId") Is Nothing Then
            Dim dtCalendar As DataTable = GetCalendarID(SchoolId)
            'ค่าถาวร
            KnSession.StoredValue("CurrentCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("CurrentCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
            'ค่ามีการเปลี่ยนแปลงเมื่อเลือกเทอม
            KnSession.StoredValue("SelectedCalendarId") = dtCalendar.Rows(0)("Calendar_Id")
            KnSession.StoredValue("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
        End If

#If DEBUG Then
        'KnSession.StoredValue("SelectedCalendarId") = "5CD20B5D-9B73-4412-8DF1-AA6602555F87"
        'KnSession.StoredValue("SelectedCalendarName") = "เทอม 1/2556"

#End If

    End Sub
#End Region

#Region "Dirty Code"

    'Public Sub setTestSetId(ByVal TestSetId As String)
    '    setValueInClass()
    '    Dim i As Integer
    '    For i = 0 To ArrApplication.Count - 1
    '        Dim objArrApplication = ArrApplication.Item(i)
    '        If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
    '            objArrApplication.TestsetId = TestSetId
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Public Sub resetValueInSession()
    '    setValueInClass()
    '    Dim i As Integer
    '    'Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
    '    'Dim selectedSession As String = HttpContext.Current.Session("selectedSession").ToString()
    '    'Dim arrApplication As ArrayList = HttpContext.Current.Application(UserId)
    '    For i = 0 To ArrApplication.Count - 1
    '        Dim objArrApplication = ArrApplication.Item(i)
    '        If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
    '            objArrApplication.objTestset = Nothing
    '            objArrApplication.TestsetId = Nothing
    '            objArrApplication.TestSetName = Nothing
    '            objArrApplication.TestSetTime = Nothing
    '            objArrApplication.OutputFileName = Nothing
    '            objArrApplication.QuizId = Nothing
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Public Sub setApplicationWhenChangeCurrentPage(ByVal testsetId As String)
    '    setValueInClass()
    '    Dim i As Integer
    '    'Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
    '    'Dim selectedSession As String = HttpContext.Current.Session("selectedSession").ToString()
    '    'Dim arrApplication As ArrayList = HttpContext.Current.Application(UserId)
    '    For i = 0 To ArrApplication.Count - 1
    '        Dim objArrApplication = ArrApplication.Item(i)
    '        If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
    '            objArrApplication.TestsetId = testsetId
    '            objArrApplication.OutputFileName = ""
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Public Sub setApplicationGenPDF(ByVal currentPage As String, ByVal OutputFileName As String)
    '    setValueInClass()
    '    Dim i As Integer
    '    'Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
    '    'Dim selectedSession As String = HttpContext.Current.Session("selectedSession").ToString()
    '    'Dim arrApplication As ArrayList = HttpContext.Current.Application(UserId)
    '    For i = 0 To ArrApplication.Count - 1
    '        Dim objArrApplication = ArrApplication.Item(i)
    '        If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
    '            objArrApplication.CurrentPage = currentPage
    '            objArrApplication.OutputFileName = OutputFileName
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Public Sub setApplicationTestsetName(ByVal testsetName As String)
    '    setValueInClass()
    '    Dim i As Integer
    '    'Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
    '    'Dim selectedSession As String = HttpContext.Current.Session("selectedSession").ToString()
    '    'Dim arrApplication As ArrayList = HttpContext.Current.Application(UserId)
    '    For i = 0 To ArrApplication.Count - 1
    '        Dim objArrApplication = ArrApplication.Item(i)
    '        If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
    '            objArrApplication.TestSetName = testsetName
    '            Exit For
    '        End If
    '    Next
    'End Sub
    Public Sub setApplicationWhenChangeCurrentPage(ByVal testsetId As String, ByVal objTestset As Object)
        setValueInClass()
        Dim i As Integer
        'Dim UserId As String = HttpContext.Current.Session("UserId").ToString()
        'Dim selectedSession As String = HttpContext.Current.Session("selectedSession").ToString()
        Dim testSetName As String
        Dim testSetTime As String
        Dim testSet_FontSize As String
        Try
            testSetName = HttpContext.Current.Session("newTestSetName").ToString()
            testSetTime = HttpContext.Current.Session("newTestSetTime").ToString()
            testSet_FontSize = HttpContext.Current.Session("newTestSetTime").ToString()
        Catch ex As Exception
            testSetName = ""
            testSetTime = "50"
            testSet_FontSize = "0"
        End Try
        'Dim arrApplication As ArrayList = HttpContext.Current.Application(UserId)
        For i = 0 To ArrApplication.Count - 1
            Dim objArrApplication = ArrApplication.Item(i)
            If (objArrApplication.PKInfo.ToString() = selectedSession.ToLower()) Then
                'objArrApplication.CurrentPage = currentPage
                objArrApplication.objTestset = objTestset
                objArrApplication.TestsetId = testsetId
                objArrApplication.TestSetName = testSetName
                objArrApplication.TestSetTime = testSetTime
                objArrApplication.OutputFileName = ""
                Exit For
            End If
        Next
    End Sub

    Public Function checkCurrentPage(ByVal UserId As String, ByVal selectedSession As String) As String
        Dim currentPage As String = ""
        Dim objArrApplication As Object = GetObjArrApplication()
        HttpContext.Current.Session("EditID") = objArrApplication.EditId
        HttpContext.Current.Session("newTestSetId") = objArrApplication.TestsetId
        HttpContext.Current.Session("objTestSet") = objArrApplication.objTestset
        HttpContext.Current.Session("newTestSetName") = objArrApplication.TestSetName
        HttpContext.Current.Session("newTestSetTime") = objArrApplication.TestSetTime
        HttpContext.Current.Session("OutputFileName") = objArrApplication.OutputFileName
        HttpContext.Current.Session("Quiz_Id") = objArrApplication.QuizId
        HttpContext.Current.Session("SchoolId") = objArrApplication.SchoolId
        'update
        HttpContext.Current.Session("QuizUseTablet") = objArrApplication.QuizUseTablet
        HttpContext.Current.Session("ChooseMode") = objArrApplication.ChooseMode
        'retrun หน้าปัจจุบัน กลับไป
        checkCurrentPage = objArrApplication.CurrentPage
    End Function
#End Region

End Class
