Imports System.Web.Services
Imports System.Web

Public Class ViewReportReduce
    Inherits System.Web.UI.Page

    Public GroupName As String

    Public Property ChooseMode As Integer
        Get
            ChooseMode = ViewState("_ChooseMode")
        End Get
        Set(ByVal value As Integer)
            ViewState("_ChooseMode") = value
        End Set
    End Property

    Private _T360SchoolId As String
    Public Property T360SchoolId() As String
        Get
            Return _T360SchoolId
        End Get
        Set(ByVal value As String)
            _T360SchoolId = value
        End Set
    End Property

    Private _WidthReport As String = "730px"
    Public Property WidthReport() As String
        Get
            Return _WidthReport
        End Get
        Set(ByVal value As String)
            _WidthReport = value
        End Set
    End Property

    'Enum
    'Quiz = 1
    'Homework = 2
    'Practice = 3

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HttpContext.Current.Session("SchoolID") = "1000001" 'Test
        'Session("UserId") = "10000001"

        If Not IsPostBack Then
            '<<< ใช้กับ T360
            If Request.QueryString("T360SchoolId") IsNot Nothing OrElse Session("IsModeT360") Then
                If Session("IsModeT360") Then
                    T360SchoolId = HttpContext.Current.Session("SchoolID")
                Else
                    HttpContext.Current.Session("SchoolID") = Request.QueryString("T360SchoolId")
                    T360SchoolId = Request.QueryString("T360SchoolId")

                    HttpContext.Current.Session("UserId") = ""
                    HttpContext.Current.Session("selectedSession") = ""

                    Dim KNSession As New KNAppSession
                    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
                    KNSession("SelectedCalendarId") = ClsReport.GetCalendarId(HttpContext.Current.Session("SchoolID"))
                    KNSession("CurrentCalendarId") = KNSession("SelectedCalendarId")
                    Session("IsModeT360") = True
                    Dim dtCalendar As DataTable = GetCalendarID(T360SchoolId)

                    If dtCalendar.Rows.Count > 0 Then
                        KNSession("SelectedCalendarName") = dtCalendar.Rows(0)("Calendar_Name") & "/" & dtCalendar.Rows(0)("Calendar_Year")
                    End If
                End If
            End If
        End If

        'If Not IsPostBack Then
        '    '<<< ใช้กับ T360
        '    If Session("IsModeT360") Then
        '        T360SchoolId = HttpContext.Current.Session("SchoolID")
        '    End If
        'End If

        If Request.QueryString("DashboardMode") IsNot Nothing Then
            If Request.QueryString("DashboardMode") = 6 Then
                ChooseMode = 3
            Else
                ChooseMode = Request.QueryString("DashboardMode") ' เอาไปเช็คกับ enumdashboard ได้เลย ว่าเป็นโหมดไหน
            End If
        Else
            Response.Write("ไม่มี QueryString DashBoard Mode")
            Exit Sub
        End If

        'If Session("UserId") = Nothing Then
        '    Response.Redirect("~/LoginPage.aspx", False)
        'End If

        'GroupName = Session("selectedSession").ToString() 'GroupName

    End Sub

    <WebMethod()>
    Public Shared Function CreatePieChartOnLoad(ByVal ChooseMode As Integer) 'สร้าง Chart วงกลมตอนแรก 

        If ChooseMode = 0 Then
            Return "-1"
        End If

        Dim ChartStr As String = ""
        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim Title As String = ""
        If ChooseMode = 1 Then
            Title = "ปริมาณการควิซ"
        ElseIf ChooseMode = 2 Then
            Title = "ปริมาณการทำการบ้าน"
        ElseIf ChooseMode = 3 Then
            Title = "ปริมาณการฝึกฝน"
        End If
        Dim TeacherId = HttpContext.Current.Session("UserId").ToString()

        Dim dt As DataTable = ClsReport.dtPieQuantity(HttpContext.Current.Session("SchoolID"), ChooseMode, HttpContext.Current.Session("UserId").ToString())
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If

        'Dim DataHash As Hashtable = ClsReport.HashPieQuantityQuiz(HttpContext.Current.Session("SchoolID"), ChooseMode)
        'If DataHash.Count = 0 Then
        '    Return "-1"
        'End If
        'ChartStr = ClsReport.GenStrDrillDownPieChart(Title, DataHash, dt, "ClassName", "ชั้น ", "ครั้ง", False)
        ChartStr = ClsReport.GenStrPieChart(Title, dt)
        Return ChartStr

    End Function '****0**** กราฟวงกลม ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว


    <WebMethod()>
    Public Shared Function CreateChartMainLevel(ByVal StrLevel As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart ในชั้นที่เลือกมา เช่น (ม.4,ม.5)  ****1****

        'ข้อมูลตัวอย่าง StrLevel = "ม.5,ม.4"
        If StrLevel Is Nothing Or StrLevel = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
            Return "-1"
        End If
        Dim ChartStr As String = ""
        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim ArrLevel = StrLevel.Trim().Split(",")
        Dim Title As String = ""
        If SortType = "0" Then
            Title = "เปอร์เซ็นต์คะแนนของนักเรียน"
        ElseIf SortType = "1" Then
            Title = "เปอร์เซ็นต์คะแนน สูงสุด 10 อันดับ"
        ElseIf SortType = "2" Then
            Title = "เปอร์เซ็นต์คะแนน ต่ำสุด 10 อันดับ"
        End If
        Dim SbSubtitle As New StringBuilder
        Dim ArrCategories As New ArrayList
        For i = 0 To ArrLevel.Count - 1
            Dim EachLevel As String = ArrLevel(i).ToString()
            SbSubtitle.Append(EachLevel & ",")
            ArrCategories.Add(EachLevel)
        Next

        Dim Subtitle As String = SbSubtitle.ToString().Substring(0, SbSubtitle.Length - 1)
        Dim YAxisTitle As String = "เปอร์เซ็นต์"
        Dim XTitle As String = "ห้อง"
        Dim FunctionName As String = "NoFunction" 'เพื่อให้ไปที่กราฟแท่ง แบบทุกห้อง ตามชั้นที่กดที่แท่ง
        Dim dt As DataTable = ClsReport.dtChartMainLevel(HttpContext.Current.Session("SchoolID"), ArrCategories, SortType, ChooseMode, HttpContext.Current.Session("UserId").ToString())
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If

        ChartStr = ClsReport.GenStrBasicDrillDownColumnChart(Title, Subtitle, YAxisTitle, FunctionName, dt, XTitle, , , "เปอร์เซ็นต์คะแนนที่ทำได้")

        HttpContext.Current.Session("ArrCatChartMainLevel") = Nothing
#If ForChartDemo = "1" Then
        Return "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: 'เปอร์เซ็นต์คะแนนของนักเรียน'},subtitle:{text:'ป.1'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['ป.1'],title:{text:'ห้อง'}}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[11.43],point: { events: { click: function () { " & FunctionName & "(this.category); } } }}] });"
#Else
        Return ChartStr
#End If
    End Function '****1**** กราฟเส้น กับกราฟแท่ง หารได้แล้ว ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว

    '<WebMethod()>
    'Public Shared Function CreateChartAllRoomInClassChoose(ByVal StrChooseAllRoom As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart ทั้งชั้น ทุกห้อง ที่เลือกมา เช่น (ม.5 ทุกห้อง) ****2****

    '    If StrChooseAllRoom Is Nothing Or StrChooseAllRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return "-1"
    '    End If

    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim Title As String = ""
    '    If SortType = "0" Then
    '        Title = "เปอร์เซ็นต์รวมของแต่ละวิชา"
    '    ElseIf SortType = "1" Then
    '        Title = "เปอร์เซ็นต์รวม สูงสุด 10 อันดับ"
    '    ElseIf SortType = "2" Then
    '        Title = "เปอร์เซ็นต์รวม ต่ำสุด 10 อันดับ"
    '    End If
    '    Dim Subtitle As String = "เฉพาะ " & StrChooseAllRoom
    '    Dim YAxisTitle As String = "เปอร์เซ็นต์"
    '    Dim XTitle As String = "ห้อง"
    '    Dim ChartStr As String = ""

    '    Dim FunctionName As String = "NoFunction" 'เพื่อให้กดไปที่กราฟเส้นได้
    '    Dim ArrCategories As New ArrayList
    '    Dim HastData As Hashtable = ClsReport.HashChartAllRoomInClassChoose(HttpContext.Current.Session("SchoolID"), StrChooseAllRoom, SortType, ChooseMode, HttpContext.Current.Session("UserId").ToString())
    '    If HastData.Count = 0 Then
    '        Return "-1"
    '    End If
    '    If HttpContext.Current.Session("ArrCatAllRoomInClass") Is Nothing Then
    '        Return "-1"
    '    End If
    '    ArrCategories.Clear()
    '    ArrCategories = HttpContext.Current.Session("ArrCatAllRoomInClass")
    '    If TypeChart = "1" Then
    '        ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YAxisTitle, FunctionName, HastData, ArrCategories, XTitle) 'รอ Hashtable Data(คิวรี่)
    '    ElseIf TypeChart = "2" Then
    '        ChartStr = ClsReport.GenStrLineChart(Title, Subtitle, YAxisTitle, HastData, ArrCategories, XTitle)
    '    End If

    '    HttpContext.Current.Session("ArrCatAllRoomInClass") = Nothing
    '    Return ChartStr

    'End Function '****2**** ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว

    <WebMethod()>
    Public Shared Function CreateChartClass(ByVal StrClass As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart ห้องกับชั้นที่เลือกมา เช่น (ม.5/1,ม.5/3) ****3****

        If StrClass Is Nothing Or StrClass = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
            Return "-1"
        End If

        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim ArrClass = StrClass.Trim().Split(",")
        Dim ArrCurrentClass As New ArrayList
        Dim ArrCurrentRoom As New ArrayList
        Dim Title As String = ""
        If SortType = "0" Then
            Title = "เปอร์เซ็นต์คะแนนของนักเรียน"
        ElseIf SortType = "1" Then
            Title = "เปอร์เซ็นต์คะแนน สูงสุด 10 อันดับ"
        ElseIf SortType = "2" Then
            Title = "เปอร์เซ็นต์คะแนน ต่ำสุด 10 อันดับ"
        End If
        Dim SbSubtitle As New StringBuilder
        Dim ArrCategories As New ArrayList
        Dim ChartStr As String = ""
        Dim XTitle As String = "ห้อง"

        For i = 0 To ArrClass.Count - 1
            Dim EachClass As String = ArrClass(i).ToString()
            SbSubtitle.Append(EachClass & ",")
            ArrCategories.Add(EachClass)
        Next

        Dim subtitle As String = SbSubtitle.ToString().Substring(0, SbSubtitle.Length - 1)
        Dim yaxistitle As String = "เปอร์เซ็นต์"
        Dim functionname As String = "NoFunction"
        Dim dt As DataTable = ClsReport.dtChartClass(HttpContext.Current.Session("schoolid"), ArrCategories, SortType, ChooseMode, , HttpContext.Current.Session("UserId").ToString())
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If
        ChartStr = ClsReport.GenStrBasicDrillDownColumnChart(Title, subtitle, yaxistitle, functionname, dt, XTitle, , , "เปอร์เซ็นต์คะแนนที่ทำได้") ' รอ hashtable data(คิวรี่)
#If ForChartDemo = "1" Then
        Return "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: 'เปอร์เซ็นต์คะแนนของนักเรียน'},subtitle:{text:'ป.1/1'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['ป.1/1'],title:{text:'ห้อง'}}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[11.43],point: { events: { click: function () { " & functionname & "(this.category); } } }}] });"
#Else
        Return ChartStr
#End If
    End Function '****3**** ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว

    '<WebMethod()>
    'Public Shared Function CreateChartRoom(ByVal StrRoom As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart เส้น เฉพาะห้องที่เลือกมาเป็นกราฟเส้น เช่น (ม.5/3) ****4****

    '    If StrRoom Is Nothing Or StrRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return "-1"
    '    End If

    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim ArrRoom = StrRoom.Split("/")
    '    Dim Title As String = ""
    '    If SortType = "0" Then
    '        Title = "เปอร์เซ็นต์รวมของแต่ละวิชา"
    '    ElseIf SortType = "1" Then
    '        Title = "เปอร์เซ็นต์รวม สูงสุด 10 อันดับ"
    '    ElseIf SortType = "2" Then
    '        Title = "เปอร์เซ็นต์รวม ต่ำสุด 10 อันดับ"
    '    End If
    '    Dim Subtitle As String = "เฉพาะ " & StrRoom
    '    Dim YAxisTitle As String = "เปอร์เซ็นต์"
    '    Dim ChartStr As String = ""
    '    If TypeChart = "3" Then 'ถ้าเป็นกราฟวงกลมเข้า If
    '        Dim dt As DataTable = ClsReport.dtPieOnlyRoom(HttpContext.Current.Session("SchoolID"), StrRoom, SortType, ChooseMode)
    '        If dt.Rows.Count = 0 Then
    '            Return "-1"
    '        End If
    '        Dim DataHash As Hashtable = ClsReport.HashPieOnlyRoom(HttpContext.Current.Session("SchoolID"), StrRoom, SortType, ChooseMode)
    '        If DataHash.Count = 0 Then
    '            Return "-1"
    '        End If
    '        ChartStr = ClsReport.GenStrDrillDownPieChart(Title, DataHash, dt, "NoInRoom", "เลขที่ ", " %", False)
    '    Else 'ถ้าเป็นกราฟแท่ง กับ เส้น เข้า Else
    '        Dim DataHash As Hashtable = ClsReport.HashChartRoom(HttpContext.Current.Session("SchoolID"), StrRoom, SortType, ChooseMode)
    '        If DataHash.Count = 0 Then
    '            Return "-1"
    '        End If
    '        If HttpContext.Current.Session("ArrCatChartRoom") Is Nothing Then
    '            Return "-1"
    '        End If
    '        Dim ArrCategories As New ArrayList
    '        If HttpContext.Current.Session("ArrCatChartRoom") Is Nothing Then
    '            Return "-1"
    '        End If
    '        ArrCategories = HttpContext.Current.Session("ArrCatChartRoom")
    '        If TypeChart = "1" Then 'กราฟแท่ง
    '            ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YAxisTitle, "NoFunction", DataHash, ArrCategories, "เลขที่")
    '        ElseIf TypeChart = "2" Then 'กราฟเส้น
    '            ChartStr = ClsReport.GenStrLineChart(Title, Subtitle, YAxisTitle, DataHash, ArrCategories, "เลขที่")
    '        End If
    '    End If

    '    HttpContext.Current.Session("ArrCatChartRoom") = Nothing
    '    Return ChartStr

    'End Function '****4**** ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว

    <WebMethod()>
    Public Shared Function CreateChartActivity(ByVal StrClass As String, ByVal TestSet_Id As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้างกราฟแท่งตามกิจกรรมและห้องที่เลือก เช่น (ภาษาไทยวันนี้ - ม.5/3,ม.5/4) โดยเอาทุกครั้งที่มีในชุดนี้และห้องนี้

        'ข้อมูลตัวอย่าง StrClass = "ม.5/1,ม.5/3" , TestSet_id = "10asdasd1as-asda1sd4sad"
        If StrClass Is Nothing Or StrClass = "" Or TestSet_Id Is Nothing Or TestSet_Id = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
            Return "-1"
        End If

        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        'Dim ArrClass = StrClass.Trim().Split(",")
        Dim Title As String = ""
        If SortType = "0" Then
            Title = "เปอร์เซ็นต์คะแนนของนักเรียน"
        ElseIf SortType = "1" Then
            Title = "เปอร์เซ็นต์คะแนน สูงสุด 10 อันดับ"
        ElseIf SortType = "2" Then
            Title = "เปอร์เซ็นต์คะแนน ต่ำสุด 10 อันดับ"
        End If
        Dim ArrCategories As New ArrayList
        Dim SbSubtitle As New StringBuilder
        Dim TestSetName As String = ClsReport.GetTestSetNameByTestSetId(TestSet_Id)
        'SbSubtitle.Append("ชุด :" & TestSetName & "<br />")
        SbSubtitle.Append("ชุด :" & TestSetName)
        Dim Subtitle As String = SbSubtitle.ToString().Substring(0, SbSubtitle.Length - 1)
        Dim YAxisTitle As String = "เปอร์เซ็นต์"
        Dim ChartStr As String = ""
        Dim dt As DataTable = ClsReport.GetDtAverageScorePerStudent(TestSet_Id, StrClass, HttpContext.Current.Session("SchoolID"), SortType, ChooseMode)
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If

        Dim WidthItem = 730 / 15
        Dim Width As Double = 730
        If dt.Rows.Count > 15 Then
            Width = dt.Rows.Count * WidthItem
        End If
        ChartStr = ClsReport.GenStrBasicColumnChart(Title, SbSubtitle.ToString(), YAxisTitle, dt, Width, "เปอร์เซ็นต์คะแนนที่ทำได้")
        If ChartStr = "" Then
            Return "-1"
        End If
        'HttpContext.Current.Session("dtChartActivity") = Nothing
#If ForChartDemo = "1" Then
        Dim StrReturn As String = ""
        If SortType = "0" Then
            StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: 'เปอร์เซ็นต์คะแนนของนักเรียน'},subtitle:{text:'ชุด :แบบทดสอบ'},xAxis: {categories: ['เลขที่ 1','เลขที่ 2','เลขที่ 3','เลขที่ 4','เลขที่ 5','เลขที่ 6','เลขที่ 7']}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[5.00,6.00,4.00,10.00,7.00,5.00,3.00 ]}] });"
        ElseIf SortType = "1" Then
            StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: 'เปอร์เซ็นต์คะแนนของนักเรียน'},subtitle:{text:'ชุด :แบบทดสอบ'},xAxis: {categories: ['เลขที่ 4','เลขที่ 5','เลขที่ 2','เลขที่ 1','เลขที่ 6','เลขที่ 3','เลขที่ 7']}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[10.00,7.00,6.00,5.00,5.00,4.00,3.00 ]}] });"
        ElseIf SortType = "2" Then
            StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: 'เปอร์เซ็นต์คะแนนของนักเรียน'},subtitle:{text:'ชุด :แบบทดสอบ'},xAxis: {categories: ['เลขที่ 7','เลขที่ 3','เลขที่ 1','เลขที่ 6','เลขที่ 2','เลขที่ 5','เลขที่ 4']}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[3.00,4.00,5.00,5.00,6.00,7.00,10.00 ]}] });"
        End If
        Return StrReturn
#Else
        Return ChartStr
#End If
    End Function '****5**** เสร็จแล้ว เขียน Code โหมดฝึกฝนเสร็จแล้ว

    '<WebMethod()>
    'Public Shared Function CreateChartActivityPerQuiz(ByVal StrClass As String, ByVal StrActivity As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้างกราฟแท่งตามกิจกรรม และ ห้องที่เลือกมา เช่น (ภาษาไทยวันนี้ - ครั้งที่1 - เฉพาะ ม.5/3)

    '    'ข้อมูลตัวอย่าง StrClass = "ม.5/3" , StrActivity = "ภาษาไทยวันนี้,ครั้งที่1"
    '    If StrClass Is Nothing Or StrClass = "" Or StrActivity Is Nothing Or StrActivity = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return "-1"
    '    End If

    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim ArrActivity As Object
    '    Dim TestSet_ID As String = ""
    '    Dim QuizTime As String = ""
    '    If ChooseMode <> 3 Then 'ถ้าไม่ได้เป็นโหมดฝึกฝนต้อง Split Str ออกเพื่อเก็บ TestSetId และ QuizId ว่าเป็นครั้งที่เท่าไหร่
    '        ArrActivity = StrActivity.Split(",")
    '        TestSet_ID = ArrActivity(0) 'เก็บชื่อกิจกรรม
    '        QuizTime = ArrActivity(1) 'เก็บว่าเป็นครั้งที่เท่าไหร่ 
    '    Else
    '        TestSet_ID = StrActivity 'ถ้าเป็นโหมดฝึกฝนจะส่งมาแต่ TestsetId อย่างเดียวไม่ต้อง Split Str ออก
    '    End If

    '    Dim TestSetName As String = ClsReport.GetTestSetNameByTestSetId(TestSet_ID)
    '    If TestSetName = "" Then
    '        Return "-1"
    '    End If
    '    Dim Title As String = ""
    '    If SortType = "0" Then
    '        Title = "คะแนน"
    '    ElseIf SortType = "1" Then
    '        Title = "คะแนน สูงสุด 10 อันดับ"
    '    ElseIf SortType = "2" Then
    '        Title = "คะแนน ต่ำสุด 10 อันดับ"
    '    End If
    '    QuizTime = QuizTime.Replace("ครั้งที่", "").Replace("ครั้งที่ ", "")
    '    Dim ChartCode As String = ""

    '    Dim HashData As Hashtable = ClsReport.HashChartActivityPerQuiz(HttpContext.Current.Session("SchoolID"), TestSet_ID, StrClass, QuizTime, SortType, ChooseMode)
    '    If HashData.Count = 0 Then
    '        Return "-1"
    '    End If
    '    If HttpContext.Current.Session("ArrCatActivityPerQuiz") Is Nothing Then
    '        Return "-1"
    '    End If
    '    Dim ArrCategories As New ArrayList
    '    ArrCategories = HttpContext.Current.Session("ArrCatActivityPerQuiz")

    '    Dim Subtitle As String = ""
    '    If ChooseMode <> 3 Then
    '        Subtitle = "ชุด :" & TestSetName & "<br />" & "ครั้งที่" & QuizTime & "ห้อง " & StrClass
    '    Else
    '        Subtitle = "ชุด :" & TestSetName & "ห้อง " & StrClass
    '    End If
    '    Dim YTitle As String = "คะแนน"
    '    If TypeChart = "1" Then
    '        ChartCode = ClsReport.GenStrStackColumnChart(Title, Subtitle, YTitle, "NoMoreFunction", HashData, ArrCategories, "เลขที่")
    '    ElseIf TypeChart = "2" Then
    '        ChartCode = ClsReport.GenStrLineChart(Title, Subtitle, YTitle, HashData, ArrCategories, "เลขที่")
    '    End If

    '    HttpContext.Current.Session("ArrCatActivityPerQuiz") = Nothing
    '    Return ChartCode

    'End Function '****6****

    <Services.WebMethod()>
    Public Shared Function createClassRoom(ByVal ChooseMode As Integer) As String
        Dim db As New ClassConnectSql()
        Dim ClsReport As New ClassViewReport(New ClassConnectSql())
        Dim StrWhere As String = ""

        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
    
        Dim CalendarId As String = ClsReport.GetCalendarId(HttpContext.Current.Session("SchoolID").ToString())
        'Dim sql As String = " SELECT DISTINCT(ClassName) FROM uvw_Chart_ClassSubjectTotalscore  WHERE SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ; "
        Dim sql As String = ""
        If ChooseMode <> "2" Then
            sql = " SELECT DISTINCT(t360_ClassName) AS ClassName FROM dbo.tblQuiz INNER JOIN dbo.tblQuizScore " & _
                  " ON dbo.tblQuiz.Quiz_Id = dbo.tblQuizScore.Quiz_Id WHERE t360_SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' " & _
                  " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = 1 AND dbo.tblQuiz.IsActive = 1 "
        Else
            sql = " SELECT DISTINCT dbo.t360_tblStudent.Student_CurrentClass AS ClassName FROM dbo.tblQuiz " & _
                  " INNER JOIN dbo.tblModuleDetailCompletion ON dbo.tblQuiz.Quiz_Id = dbo.tblModuleDetailCompletion.Quiz_Id " & _
                  " INNER JOIN dbo.t360_tblStudent ON dbo.tblModuleDetailCompletion.Student_Id = dbo.t360_tblStudent.Student_Id " & _
                  " WHERE dbo.tblQuiz.IsActive = 1 AND dbo.tblQuiz.Calendar_Id = '" & CalendarId & "' " & _
                  " AND dbo.tblQuiz." & StrWhere & " = '1' AND dbo.tblQuiz.t360_SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' "
        End If
        Dim dtclass As DataTable = db.getdata(sql)
        Dim className As String = ""
        For i As Integer = 0 To dtclass.Rows.Count - 1
            className &= dtclass.Rows(i)("ClassName").ToString() + ","
        Next
        className = className.Substring(0, className.Length - 1)
        ClsReport = Nothing
        Return className
    End Function

    <Services.WebMethod()>
    Public Shared Function createRoomFormSelectedClass(ByVal classSelected As String, ByVal ChooseMode As Integer) As String
        Dim db As New ClassConnectSql()
        Dim ClsReport As New ClassViewReport(New ClassConnectSql())
        'Dim _class As String = "ม.4,ม.5"
        Dim classS = classSelected.Split(",")
        Dim StrWhere As String = ""

        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
    
        Dim CalendarId As String = ClsReport.GetCalendarId(HttpContext.Current.Session("SchoolID").ToString())
        'Dim sql As String = " SELECT DISTINCT ClassName,RoomName FROM uvw_Chart_ClassRoomSubjectTotalscore  WHERE SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "

        'For i As Integer = 0 To classS.Length - 1
        '    If (i = 0) Then
        '        sql &= " AND ClassName = '" & classS(i) & "' "
        '    Else
        '        sql &= " OR ClassName = '" & classS(i) & "' "
        '    End If
        'Next
        Dim sql As String = ""
        If ChooseMode <> "2" Then
            sql = " SELECT DISTINCT dbo.tblQuiz.t360_ClassName As ClassName,(t360_ClassName + t360_RoomName) AS RoomName FROM dbo.tblQuiz WHERE t360_SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' "
            For i As Integer = 0 To classS.Length - 1
                If (i = 0) Then
                    sql &= " AND (t360_ClassName = '" & classS(i) & "' "
                Else
                    sql &= " OR t360_ClassName = '" & classS(i) & "' "
                End If
            Next
            sql &= " ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = 1 AND User_Id <> '" & HttpContext.Current.Application("DefaultUserId").ToString() & "' "
        Else
            sql = " SELECT DISTINCT dbo.t360_tblStudent.Student_CurrentClass AS ClassName, " & _
                  " (dbo.t360_tblStudent.Student_CurrentClass + dbo.t360_tblStudent.Student_CurrentRoom) AS RoomName " & _
                  " FROM dbo.tblQuiz INNER JOIN dbo.tblModuleDetailCompletion ON dbo.tblQuiz.Quiz_Id = dbo.tblModuleDetailCompletion.Quiz_Id " & _
                  " INNER JOIN dbo.t360_tblStudent ON dbo.tblModuleDetailCompletion.Student_Id = dbo.t360_tblStudent.Student_Id " & _
                  " WHERE dbo.tblQuiz.Calendar_Id = '" & CalendarId & "' " & _
                  " AND dbo.tblQuiz." & StrWhere & " = 1 "
            For i As Integer = 0 To classS.Length - 1
                If (i = 0) Then
                    sql &= " AND (dbo.t360_tblStudent.Student_CurrentClass = '" & classS(i) & "' "
                Else
                    sql &= " OR dbo.t360_tblStudent.Student_CurrentClass = '" & classS(i) & "' "
                End If
            Next
            sql &= " ) AND dbo.tblQuiz.t360_SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' "
        End If
        Dim dt = db.getdata(sql)

        'Dim tableText As String = "<table><tr style='background-color: #abc'><th>รายชื่อห้อง</th></tr>"
        'For j As Integer = 0 To dt.Rows.Count - 1
        '    Dim roomName As String = dt.Rows(j)("RoomName")
        '    If (j Mod 2 = 0) Then
        '        tableText &= "<tr><td><input type='checkbox' id='" & roomName & "' class='classRoom'/><label for='" & roomName & "'>" & roomName & "</label></td></tr>"
        '    Else
        '        tableText &= "<tr style='background-color: #abc'><td><input type='checkbox' id='" & roomName & "' class='classRoom' /><label for='" & roomName & "'>" & roomName & "</label></td></tr>"
        '    End If
        'Next
        'tableText &= "</table>"
        Dim tableText = ""
        Dim startClass As String = ""

        If classS(0) Is Nothing Or classS(0) = "" Then
            tableText = "<span style='position:relative;top:243px;margin-left:100px;margin-right:100px;font-weight: bold;'>ต้องเลือกชั้นเรียนก่อนค่ะ</span>"
        Else
            For j As Integer = 0 To dt.Rows().Count - 1
                Dim room As String = dt.Rows(j)("RoomName")
                room = room.Replace("ป.", "")
                room = room.Replace("ม.", "")
                If startClass <> dt.Rows(j)("ClassName") Then
                    If j <> 0 Then
                        tableText &= "</table>"
                    End If
                    startClass = dt.Rows(j)("ClassName")
                    tableText &= "<table style=""width:100px;float:left;""><tr style=""background-color: #F8DDA3""><th>" & startClass & "</th></tr>"
                    tableText &= "<tr><td><input type='checkbox' id='" & dt.Rows(j)("RoomName") & "' class='classRoom'/><label for='" & dt.Rows(j)("RoomName") & "'>" & room & "</label></td></tr>"
                Else
                    tableText &= "<tr><td><input type='checkbox' id='" & dt.Rows(j)("RoomName") & "' class='classRoom'/><label for='" & dt.Rows(j)("RoomName") & "'>" & room & "</label></td></tr>"
                End If
            Next
            tableText &= "</table>"
        End If
        ClsReport = Nothing
        Return tableText
    End Function

    <Services.WebMethod()>
    Public Shared Function createQuizFromRoomSelected(ByVal classRoomSelected As String, ByVal ChooseMode As Integer)
        Dim db As New ClassConnectSql()
        Dim ClsReport As New ClassViewReport(New ClassConnectSql())

        Dim CalendarId As String = ClsReport.GetCalendarId(HttpContext.Current.Session("SchoolID").ToString())
        Dim sql As String = " SELECT DISTINCT(TestSetName),TestSet_Id,IsStandard FROM dbo.uvw_Chart_AvgStudentScoreNew WHERE SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' "
        Dim room = classRoomSelected.Split(",")
        For j As Integer = 0 To room.Length - 1
            If (j = 0) Then
                sql &= " AND RoomName = '" & room(j).ToString() & "'"
            Else
                sql &= " OR RoomName = '" & room(j).ToString() & "'"
            End If
        Next
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        sql &= " AND Calendar_Id = '" & CalendarId & "' AND TestSetName <> '' AND " & StrWhere & " = '1' ORDER BY IsStandard; "

        Dim dtQuizName = db.getdata(sql)

        Dim tableQuiz As String = ""

        If room(0) Is Nothing Or room(0) = "" Then
            tableQuiz = "<span style='position:relative;top:243px;margin-left:100px;margin-right:100px;font-weight: bold;'>ต้องเลือกห้องเรียนก่อนค่ะ</span>"
        Else
            'tableQuiz = "<table style=""text-align:left;""><tr style=""background-color: #F8DDA3;text-align:center;""><th colspan='3'><input type='button' id='btnStandard' onclick='StandarFilter();' value='ชุดมาตรฐาน' /><input type='button' onclick='TeacherSetFilter();' id='btnTeacherSet' value='ชุดที่ครูจัดไว้' />ชุดควิซ<span class='imgFind'></span><input type='text' id='txtSearch' placeholder='ค้นหา' onclick='SearchTestset();' /><span class='icon_clear' onclick='ClickClear(this);'>X</span></th></tr>"
            tableQuiz = "<div style=""text-align:left;background-color: #F8DDA3;text-align:center;height:50px;""><input type='button' onclick='TeacherSetFilter();' id='btnToggleFilter' class='Forbtn' value='ชุดที่ครูจัดไว้' />ชุดควิซ<span class='imgFind'></span><input type='text' id='txtSearch' placeholder='ค้นหา' onclick='SearchTestset();' /><span class='icon_clear' onclick='ClickClear(this);'>X</span></div>"
            For i As Integer = 0 To dtQuizName.Rows.Count - 1
                Dim IsStandard As Boolean = False
                Dim ClsStandard As String = ""
                If dtQuizName.Rows(i)("IsStandard") IsNot DBNull.Value Then
                    IsStandard = dtQuizName.Rows(i)("IsStandard")
                End If

                If IsStandard = True Then
                    ClsStandard = "IsStandard"
                Else
                    ClsStandard = "IsTeacherSet"
                End If

                Dim quizName As String = dtQuizName.Rows(i)("TestSetName").ToString()
                'If i Mod 3 = 0 Then
                '    If i <> 0 Then
                '        tableQuiz &= "</tr>"
                '    End If
                '    tableQuiz &= "<tr><td style='width:200px;vertical-align: top;'><input type='checkbox' id='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' IsStandard='" & IsStandard & "' filter='" & quizName & "' class='quizName'/><label for='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' style='margin-left:10px;'>" & quizName & "</label></td>"
                'Else
                '    tableQuiz &= "<td style='width:200px;vertical-align: top;'><input type='checkbox' id='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' IsStandard='" & IsStandard & "' filter='" & quizName & "' class='quizName'/><label for='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' style='margin-left:10px;'>" & quizName & "</label></td>"
                'End If
                tableQuiz &= "<div class='" & ClsStandard & "' filter='" & quizName & "' ><input type='checkbox' id='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' IsStandard='" & IsStandard & "'  class='quizName'/><label for='" & dtQuizName.Rows(i)("TestSet_Id").ToString() & "' style='margin-left:0px;'>" & quizName & "</label></div>"
            Next
            tableQuiz &= "</div>"
        End If
        ClsReport = Nothing
        Return tableQuiz
    End Function

    Private Function GetCalendarID(ByVal SchoolID As String) As DataTable
        Dim sql As String = " SELECT TOP 1 * FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate AND Calendar_Type = 3 AND School_Code = '" & SchoolID & "'; "
        Dim db As New ClassConnectSql()
        GetCalendarID = db.getdata(sql)
        Return GetCalendarID
    End Function

End Class

#Region "ของเก่า"
'<WebMethod()>
'Public Shared Function CreateChartMainLevel(ByVal StrLevel As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart ในชั้นที่เลือกมา เช่น (ม.4,ม.5)  ****1****

'    'ข้อมูลตัวอย่าง StrLevel = "ม.5,ม.4"
'    If StrLevel Is Nothing Or StrLevel = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
'        Return "-1"
'    End If
'    Dim ChartStr As String = ""
'    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
'    Dim ArrLevel = StrLevel.Trim().Split(",")
'    Dim Title As String = ""
'    If SortType = "0" Then
'        Title = "เปอร์เซ็นต์รวมของแต่ละวิชา"
'    ElseIf SortType = "1" Then
'        Title = "เปอร์เซ็นต์รวม สูงสุด 10 อันดับ"
'    ElseIf SortType = "2" Then
'        Title = "เปอร์เซ็นต์รวม ต่ำสุด 10 อันดับ"
'    End If
'    Dim SbSubtitle As New StringBuilder
'    Dim ArrCategories As New ArrayList
'    For i = 0 To ArrLevel.Count - 1
'        Dim EachLevel As String = ArrLevel(i).ToString()
'        SbSubtitle.Append(EachLevel & ",")
'        ArrCategories.Add(EachLevel)
'    Next

'    Dim Subtitle As String = SbSubtitle.ToString().Substring(0, SbSubtitle.Length - 1)
'    Dim YAxisTitle As String = "เปอร์เซ็นต์"
'    Dim XTitle As String = "ห้อง"
'    Dim FunctionName As String = "NoFunction" 'เพื่อให้ไปที่กราฟแท่ง แบบทุกห้อง ตามชั้นที่กดที่แท่ง
'    Dim HastData As Hashtable = ClsReport.HashChartMainLevel(HttpContext.Current.Session("SchoolID"), ArrCategories, SortType, ChooseMode, HttpContext.Current.Session("UserId").ToString())
'    If HastData.Count = 0 Then
'        Return "-1"
'    End If
'    If HttpContext.Current.Session("ArrCatChartMainLevel") Is Nothing Then
'        Return "-1"
'    End If
'    ArrCategories.Clear()
'    ArrCategories = HttpContext.Current.Session("ArrCatChartMainLevel")
'    If TypeChart = "1" Then 'กราฟแท่ง
'        ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YAxisTitle, FunctionName, HastData, ArrCategories, XTitle)
'    ElseIf TypeChart = "2" Then 'กราฟเส้น
'        ChartStr = ClsReport.GenStrLineChart(Title, Subtitle, YAxisTitle, HastData, ArrCategories, XTitle)
'    End If

'    HttpContext.Current.Session("ArrCatChartMainLevel") = Nothing
'    Return ChartStr

'End Function '****1**** กราฟเส้น กับกราฟแท่ง หารได้แล้ว ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว

'<WebMethod()>
'Public Shared Function CreateChartClass(ByVal StrClass As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'สร้าง Chart ห้องกับชั้นที่เลือกมา เช่น (ม.5/1,ม.5/3) ****3****

'    If StrClass Is Nothing Or StrClass = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Then
'        Return "-1"
'    End If

'    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
'    Dim ArrClass = StrClass.Trim().Split(",")
'    Dim ArrCurrentClass As New ArrayList
'    Dim ArrCurrentRoom As New ArrayList
'    Dim Title As String = ""
'    If SortType = "0" Then
'        Title = "เปอร์เซ็นต์รวมของแต่ละวิชา"
'    ElseIf SortType = "1" Then
'        Title = "เปอร์เซ็นต์รวม สูงสุด 10 อันดับ"
'    ElseIf SortType = "2" Then
'        Title = "เปอร์เซ็นต์รวม ต่ำสุด 10 อันดับ"
'    End If
'    Dim SbSubtitle As New StringBuilder
'    Dim ArrCategories As New ArrayList
'    Dim ChartStr As String = ""
'    Dim XTitle As String = "ห้อง"

'    For i = 0 To ArrClass.Count - 1
'        Dim EachClass As String = ArrClass(i).ToString()
'        SbSubtitle.Append(EachClass & ",")
'        ArrCategories.Add(EachClass)
'    Next

'    Dim subtitle As String = SbSubtitle.ToString().Substring(0, SbSubtitle.Length - 1)
'    Dim yaxistitle As String = "เปอร์เซ็นต์"
'    Dim functionname As String = "NoFunction"
'    Dim datahash As Hashtable = ClsReport.HashChartClass(HttpContext.Current.Session("schoolid"), ArrCategories, SortType, ChooseMode, , HttpContext.Current.Session("UserId").ToString())
'    If datahash.Count = 0 Then
'        Return "-1"
'    End If
'    If HttpContext.Current.Session("ArrChartClass") Is Nothing Then
'        Return "-1"
'    End If
'    ArrCategories.Clear()
'    ArrCategories = HttpContext.Current.Session("ArrChartClass")
'    If TypeChart = "1" Then
'        ChartStr = ClsReport.GenStrStackColumnChart(Title, subtitle, yaxistitle, functionname, datahash, ArrCategories, XTitle) ' รอ hashtable data(คิวรี่)
'    ElseIf TypeChart = "2" Then
'        ChartStr = ClsReport.GenStrLineChart(Title, subtitle, yaxistitle, datahash, ArrCategories, XTitle)
'    End If

'    HttpContext.Current.Session("ArrChartClass") = Nothing
'    Return ChartStr

'End Function '****3**** ใส่ Parameter PracticeMode และ แก้ Function ใน Cls แล้ว
#End Region

