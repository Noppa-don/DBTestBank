Imports System.Web
Public Class viewReportPersonal
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HttpContext.Current.Session("SchoolID") = "1000001"
        'Session("UserId") = "1"

        If Not IsPostBack Then
            '<<< ใช้กับ T360
            If Session("IsModeT360") Then
                T360SchoolId = HttpContext.Current.Session("SchoolID")
            End If
        End If

        'If Session("UserId") = Nothing Then
        '    Response.Redirect("~/LoginPage.aspx")
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

        createClassRoom(HttpContext.Current.Session("SchoolID").ToString, ChooseMode)
        GroupName = Session("selectedSession").ToString() 'GroupName

    End Sub

    Private Sub createClassRoom(ByVal SchoolId As String, ByVal ChooseMode As Integer)
        Dim db As New ClassConnectSql()
        Dim KnSession As New KNAppSession()
        Dim CalendarId As String = KnSession("SelectedCalendarId").ToString()
        Dim StrWhere As String = ""

        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim sql As String = " SELECT DISTINCT(ClassName),roomName FROM uvw_Chart_ClassRoomSubjectTotalscore WHERE SchoolCode = '" & SchoolId & "' " & _
                            " AND " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' ORDER BY ClassName,RoomName ; "

        Dim dtclass As DataTable = db.getdata(sql)

        Dim classTable As String = "<table >"

        Dim classState = dtclass.Rows(0)("ClassName")
        Dim headTable = 0
        For i As Integer = 0 To dtclass.Rows.Count - 1
            Dim room = dtclass.Rows(i)("roomName")
            room = room.replace("ป.", "")
            room = room.replace("ม.", "")

            If classState = dtclass.Rows(i)("ClassName") Then
                If headTable = 0 Then
                    classTable &= "<tr><td style=""background-color: #F8DDA3;font-weight:bold;width:80px;"">" & classState & " > </td><td style=""width:80px;""><input type='checkbox' id='" & dtclass.Rows(i)("roomName") & "' class='classroom'/><label for='" & dtclass.Rows(i)("roomName") & "'>" & room & "</label></td>"
                    headTable = 1
                Else
                    classTable &= "<td style=""width:80px;"" ><input type='checkbox' id='" & dtclass.Rows(i)("roomName") & "' class='classroom'/><label for='" & dtclass.Rows(i)("roomName") & "'>" & room & "</label></td>"
                End If
            Else
                classState = dtclass.Rows(i)("ClassName")
                classTable &= "</tr><tr><td style=""background-color: #F8DDA3;font-weight:bold;width:80px;"">" & classState & " > </td><td style=""width:80px;""><input type='checkbox' id='" & dtclass.Rows(i)("roomName") & "' class='classroom'/><label for='" & dtclass.Rows(i)("roomName") & "'>" & room & "</label></td>"
            End If

        Next

        classTable &= "</tr></table>"
        'className = className.Substring(0, className.Length - 1)
        menuShowClass.InnerHtml = classTable

    End Sub

    <Services.WebMethod()>
    Public Shared Function createStudentInRoom(ByVal room As String, ByVal ChooseMode As Integer) As String
        Dim db As New ClassConnectSql()
        Dim KnSession As New KNAppSession()
        Dim CalendarId As String = KnSession("SelectedCalendarId").ToString()
        Dim StrWhere As String = ""

        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim sql As String = " SELECT DISTINCT RoomName,NoInRoom FROM uvw_Chart_RoomNoSubjectTotalscore WHERE RoomName = '" & room & "' " & _
                            " AND " & StrWhere & " = '1' AND SchoolCode = '" & HttpContext.Current.Session("SchoolID") & "' AND Calendar_Id = '" & CalendarId & "' ORDER BY NoInRoom ; "

        Dim dtclass As DataTable = db.getdata(sql)

        Dim classTable As String = "<table ><tr><td colspan='10' style='font-weight:bold;background-color: #F8DDA3;text-align:left;padding-left:25px;'>เลชที่ทั้งหมดของ ห้อง " & room & "</td></tr><tr>"
        Dim numberToRow As Integer = 1
        For i As Integer = 0 To dtclass.Rows.Count - 1
            Dim noInRoom As String = "no" & dtclass.Rows(i)("NoInRoom")
            If (numberToRow < 11) Then
                classTable &= "<td style=""width:60px;""><input type='checkbox' id='" & noInRoom & "' class='studentInRoom'/><label for='" & noInRoom & "'>" & dtclass.Rows(i)("NoInRoom") & "</label></td>"
                numberToRow = numberToRow + 1
            Else
                classTable &= "</tr><tr>"
                classTable &= "<td style=""width:60px;""><input type='checkbox' id='" & noInRoom & "' class='studentInRoom'/><label for='" & noInRoom & "'>" & dtclass.Rows(i)("NoInRoom") & "</label></td>"
                numberToRow = 1
            End If
        Next

        classTable &= "</tr>"

        Return classTable
    End Function

    <Services.WebMethod()>
    Public Shared Function CreateChartMainPersonal(ByVal SchoolId As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)
        If SchoolId Is Nothing Or SchoolId = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
            Return "-1"
        End If
        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim Title As String = "<b>เปอร์เซ็นต์คะแนนของนักเรียน</b>"
        Dim Subtitle As String = "สูงสุดอันดับ 1 ของทุกชั้น"
        Dim YTitle As String = "เปอร์เซ็นต์"
        Dim dt As DataTable = ClsReport.dtChartMainPersonal(HttpContext.Current.Session("SchoolID"), ChooseMode)
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If
        Dim ChartStr As String = ""
        'ม.5/6:เลขที 99
        Dim WidthItem = 730 / 7
        Dim Width As Double = 730
        If dt.Rows.Count > 7 Then
            Width = dt.Rows.Count * WidthItem
        End If
        If TypeChart <> "" Then
            If TypeChart = "1" Then ' 1 = กราฟแท่ง
                ChartStr = ClsReport.GenStrBasicDrillDownColumnChart(Title, Subtitle, YTitle, "CreateChartOnlyRoomInPersonalMode", dt, "ชั้น/ห้อง-เลขที่", , , "เปอร์เซ็นต์คะแนนที่ทำได้")
                'ElseIf TypeChart = "2" Then ' 2 = กราฟเส้น
                '    ChartStr = ClsReport.GenStrLineChart(Title, Subtitle, YTitle, DataHash, ArrCategories, "ห้อง", Width)
            End If
            'Else
            '    ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YTitle, "CreateChartOnlyRoomInPersonalMode", DataHash, ArrCategories, "ห้อง", Width)
        End If
#If ForChartDemo = "1" Then
        Dim StrReturn As String = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: '<b>เปอร์เซ็นต์คะแนนของนักเรียน</b>'},subtitle:{text:'สูงสุดอันดับ 1 ของทุกชั้น'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['ป.1/1:เลขที่7','ป.2/1:เลขที่4','ป.3/1:เลขที่2','ป.4/1:เลขที่5'],title:{text:'ชั้น/ห้อง-เลขที่'}}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[80.00,60.00,90.00,65.00 ],point: { events: { click: function () { CreateChartOnlyRoomInPersonalMode(this.category); } } }}] });"
        Return StrReturn
#Else
        Return ChartStr
#End If
    End Function

    <Services.WebMethod()>
    Public Shared Function CreateChartOnlyRoomInPersonalMode(ByVal StrRoom As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        If StrRoom Is Nothing Or StrRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Or ChooseMode = 0 Then
            Return "-1"
        End If
        Dim Title As String
        Dim Subtitle As String = StrRoom
        Dim Ytitle As String = "<b>เปอร์เซ็นต์คะแนน</b>"
        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        If SortType = "0" Then
            Title = "<b>เปอร์เซ็นต์คะแนนของแต่ละคน</b>"
        End If

        Dim dt As DataTable = ClsReport.dtChartRoom(HttpContext.Current.Session("SchoolID"), StrRoom, SortType, ChooseMode)
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If
        Dim StrChart As String = ""
        If TypeChart = "1" Then
            'StrChart = ClsReport.GenStrStackColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", DataHash, ArrCategories, "เลขที่")
            StrChart = ClsReport.GenStrBasicDrillDownColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", dt, "เลขที", , , "เปอร์เซ็นต์คะแนนที่ทำได้")
        ElseIf TypeChart = "2" Then
            'StrChart = ClsReport.GenStrLineChart(Title, Subtitle, Ytitle, DataHash, ArrCategories, "เลขที่")
        End If
#If ForChartDemo = "1" Then
        Dim StrReturn As String = ""
        StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: '<b>เปอร์เซ็นต์คะแนนของแต่ละคน</b>'},subtitle:{text:'ป.1/1'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['1','2','3','4','5','6','7'],title:{text:'เลขที'}}, yAxis: {min: 0,title: {text: '<b>เปอร์เซ็นต์คะแนน</b>'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[60.00,80.00,70.00,20.00,50.00,40.00,80.00],point: { events: { click: function () { CreateChartStudentOnly(this.category); } } }}] });"
        Return StrReturn
#Else
        Return StrChart
#End If
    End Function

    <Services.WebMethod()>
    Public Shared Function CreateChartTopLowOnlyRoomInPersonalMode(ByVal StrRoom As String, ByVal TopOrLow As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)

        If StrRoom Is Nothing Or StrRoom = "" Or TopOrLow Is Nothing Or TopOrLow = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
            Return "-1"
        End If

        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim Title As String
        Dim Subtitle As String = StrRoom
        Dim Ytitle As String = "<b>เปอร์เซ็นต์คะแนนของนักเรียน</b>"
        Dim dt As DataTable
        If TopOrLow = "1" Then 'ถ้าเป็น 1 แสดงว่า สูงสุด 0 แสดงว่าต่ำสุด
            Title = "<b>เปอร์เซ็นต์คะแนนที่ทำได้ สูงสุด 10 อันดับ</b>"
            dt = ClsReport.dtChartTopLowPerSonalMode(HttpContext.Current.Session("SchoolID"), StrRoom, "desc", ChooseMode)
        Else
            Title = "<b>เปอร์เซ็นต์คะแนนที่ทำได้ ต่ำสุด 10 อันดับ</b>"
            dt = ClsReport.dtChartTopLowPerSonalMode(HttpContext.Current.Session("SchoolID"), StrRoom, "asc", ChooseMode)
        End If

        If dt.Rows.Count = 0 Then
            Return "-1"
        End If
        Dim StrChart As String = ""
        If TypeChart = "1" Then
            'StrChart = ClsReport.GenStrStackColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", DataHash, ArrCategories, "เลขที่")
            StrChart = ClsReport.GenStrBasicDrillDownColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", dt, "เลขที่", , , "เปอร์เซ็นต์คะแนนที่ทำได้")
        ElseIf TypeChart = "2" Then
            'StrChart = ClsReport.GenStrLineChart(Title, Subtitle, Ytitle, DataHash, ArrCategories, "เลขที่")
        End If
#If ForChartDemo = "1" Then
        Dim StrReturn As String = ""
        If TopOrLow = "1" Then 'ถ้าเป็น 1 แสดงว่า สูงสุด 0 แสดงว่าต่ำสุด
            StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: '<b>เปอร์เซ็นต์คะแนนของแต่ละคน</b>'},subtitle:{text:'ป.1/1'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['4','6','5','1','3','2','7'],title:{text:'เลขที'}}, yAxis: {min: 0,title: {text: '<b>เปอร์เซ็นต์คะแนน</b>'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[20.00,40.00,50.00,60.00,70.00,80.00,80.00],point: { events: { click: function () { CreateChartStudentOnly(this.category); } } }}] });"
        Else
            StrReturn = "chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: '<b>เปอร์เซ็นต์คะแนนของแต่ละคน</b>'},subtitle:{text:'ป.1/1'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['2','7','3','1','5','6','4'],title:{text:'เลขที'}}, yAxis: {min: 0,title: {text: '<b>เปอร์เซ็นต์คะแนน</b>'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[80.00,80.00,70.00,60.00,50.00,40.00,20.00],point: { events: { click: function () { CreateChartStudentOnly(this.category); } } }}] });"
        End If
        Return StrReturn
#Else
        Return StrChart
#End If
    End Function '****10**** Top10 - Low10

    <Services.WebMethod()>
    Public Shared Function CreateChartStudentOnly(ByVal StrRoom As String, ByVal NoInRoom As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)
        If StrRoom Is Nothing Or StrRoom = "" Or NoInRoom Is Nothing Or NoInRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
            Return "-1"
        End If

        Dim KNSession As New KNAppSession()
        Dim CalendarName As String = KNSession("SelectedCalendarName")
        Dim ClsReport As New ClassViewReport(New ClassConnectSql)
        Dim ArrSplit = StrRoom.Split("/")
        Dim CurrentClass As String = ArrSplit(0)
        Dim CurrentRoom As String = "/" & ArrSplit(1)
        Dim Title As String = "<b>เปอร์เซ็นต์คะแนนของนักเรียน ทุกปีการศึกษา</b>"
        Dim StudentName As String = ClsReport.GetStudentName(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom)
        If StudentName = "" Then
            Return "-1"
        End If
        Dim StudentLastName As String = ClsReport.GetStudentLastName(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom)
        If StudentLastName = "" Then
            Return "-1"
        End If

        Dim Subtitle As String = " ของ " & StudentName & " " & StudentLastName & " " & StrRoom & " เลขที่ " & NoInRoom & " ปีการศึกษา " & CalendarName
        Dim YTitle As String = "เปอร์เซ็นต์"
        Dim dt As DataTable = ClsReport.dtChartStudentOnly(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom, ChooseMode)
        If dt.Rows.Count = 0 Then
            Return "-1"
        End If

        Dim StrChart As String = ""
        If TypeChart = "1" Then
            StrChart = ClsReport.GenStrBasicDrillDownColumnChart(Title, Subtitle, YTitle, "NoFunction", dt, "ห้อง/เลขที่", , , "เปอร์เซ็นต์คะแนนที่ทำได้")
        ElseIf TypeChart = "2" Then
            'StrChart = ClsReport.GenStrLineChart(Title, Subtitle, YTitle, DataHash, ArrCategories, "ห้อง/เลขที่")
        End If

#If ForChartDemo = "1" Then
        Dim StrReturn As String = " chart = new Highcharts.Chart({ chart: {type: 'column',renderTo: 'DivReport',width:730},title: {text: '<b>เปอร์เซ็นต์คะแนนของนักเรียน ทุกปีการศึกษา</b>'},subtitle:{text:' ของ ไก่ อย่าลืมฉัน ป.1/1 เลขที่ 1 ปีการศึกษา เทอม 2/2557'},tooltip:{pointFormat:'{series.name}:<b>{point.y} %</b>'},xAxis: {categories: ['2557 เทอม 1 ป.1/1 เลขที่ 1','2557 เทอม 2:ป.1/1:เลขที่ 1'],title:{text:'ห้อง/เลขที่'}}, yAxis: {min: 0,title: {text: 'เปอร์เซ็นต์'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'เปอร์เซ็นต์คะแนนที่ทำได้',data:[25.00,16.00 ],point: { events: { click: function () { NoFunction(this.category); } } }}] });"
        Return StrReturn
#Else
           Return StrChart
#End If
    End Function '****9**** เสร็จแล้ว

#Region "ของเก่า"
    '<Services.WebMethod()>
    'Public Shared Function CreateChartMainPersonal(ByVal SchoolId As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)

    '    If SchoolId Is Nothing Or SchoolId = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
    '        Return "-1"
    '    End If

    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim Title As String = "<b>เปอร์เซ็นต์แต่ละวิชา</b>"
    '    Dim Subtitle As String = "สูงสุดอันดับ 1 ของทุกชั้น"
    '    Dim YTitle As String = "เปอร์เซ็นต์"
    '    Dim DataHash As Hashtable = ClsReport.HashChartMainPersonal(HttpContext.Current.Session("SchoolID"), ChooseMode)
    '    If DataHash.Count = 0 Then
    '        Return "-1"
    '    End If
    '    Dim ArrCategories As ArrayList = HttpContext.Current.Session("ArrCategoriesChartMainPersonal")
    '    If ArrCategories.Count = 0 Then
    '        Return "-1"
    '    End If
    '    HttpContext.Current.Session("ArrCategoriesChartMainPersonal") = Nothing

    '    Dim ChartStr As String = ""
    '    'ม.5/6:เลขที 99
    '    Dim WidthItem = 730 / 7
    '    Dim Width As Double = 730
    '    If ArrCategories.Count > 7 Then
    '        Width = ArrCategories.Count * WidthItem
    '    End If
    '    If TypeChart <> "" Then
    '        If TypeChart = "1" Then ' 1 = กราฟแท่ง
    '            ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YTitle, "CreateChartOnlyRoomInPersonalMode", DataHash, ArrCategories, "ห้อง", Width)
    '        ElseIf TypeChart = "2" Then ' 2 = กราฟเส้น
    '            ChartStr = ClsReport.GenStrLineChart(Title, Subtitle, YTitle, DataHash, ArrCategories, "ห้อง", Width)
    '        End If
    '    Else
    '        ChartStr = ClsReport.GenStrStackColumnChart(Title, Subtitle, YTitle, "CreateChartOnlyRoomInPersonalMode", DataHash, ArrCategories, "ห้อง", Width)
    '    End If

    '    Return ChartStr

    'End Function '****7**** เสร็จแล้ว เพิ่ม Parameter โหมดฝึกฝนแล้ว

    '<Services.WebMethod()>
    'Public Shared Function CreateChartOnlyRoomInPersonalMode(ByVal StrRoom As String, ByVal TypeChart As String, ByVal SortType As String, ByVal ChooseMode As Integer)

    '    If StrRoom Is Nothing Or StrRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or SortType Is Nothing Or SortType = "" Or ChooseMode = 0 Then
    '        Return "-1"
    '    End If
    '    Dim Title As String
    '    Dim Subtitle As String = StrRoom
    '    Dim Ytitle As String = "<b>เปอร์เซ็นต์</b>"
    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    If SortType = "0" Then
    '        Title = "<b>เปอร์เซ็นต์แต่ละวิชา</b>"
    '    End If

    '    Dim DataHash As Hashtable = ClsReport.HashChartRoom(HttpContext.Current.Session("SchoolID"), StrRoom, SortType, ChooseMode)
    '    If DataHash.Count = 0 Then
    '        Return "-1"
    '    End If
    '    If HttpContext.Current.Session("ArrCatChartRoom") Is Nothing Then
    '        Return "-1"
    '    End If
    '    Dim ArrCategories As New ArrayList
    '    ArrCategories = HttpContext.Current.Session("ArrCatChartRoom")
    '    HttpContext.Current.Session("dtChartOnlyRommInPersonalMode") = Nothing
    '    Dim StrChart As String = ""

    '    If TypeChart = "1" Then
    '        StrChart = ClsReport.GenStrStackColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", DataHash, ArrCategories, "เลขที่")
    '    ElseIf TypeChart = "2" Then
    '        StrChart = ClsReport.GenStrLineChart(Title, Subtitle, Ytitle, DataHash, ArrCategories, "เลขที่")
    '    End If

    '    Return StrChart

    'End Function

    '<Services.WebMethod()>
    'Public Shared Function CreateChartTopLowOnlyRoomInPersonalMode(ByVal StrRoom As String, ByVal TopOrLow As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)

    '    If StrRoom Is Nothing Or StrRoom = "" Or TopOrLow Is Nothing Or TopOrLow = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
    '        Return "-1"
    '    End If

    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim Title As String
    '    Dim Subtitle As String = StrRoom
    '    Dim Ytitle As String = "<b>เปอร์เซ็นต์</b>"
    '    Dim DataHash As Hashtable
    '    If TopOrLow = "1" Then 'ถ้าเป็น 1 แสดงว่า สูงสุด 0 แสดงว่าต่ำสุด
    '        Title = "<b>เปอร์เซ็นต์แต่ละวิชา สูงสุด 10 อันดับ</b>"
    '        DataHash = ClsReport.HashChartTopLowPerSonalMode(HttpContext.Current.Session("SchoolID"), StrRoom, "desc", ChooseMode)
    '    Else
    '        Title = "<b>เปอร์เซ็นต์แต่ละวิชา ต่ำสุด 10 อันดับ</b>"
    '        DataHash = ClsReport.HashChartTopLowPerSonalMode(HttpContext.Current.Session("SchoolID"), StrRoom, "asc", ChooseMode)
    '    End If

    '    If DataHash.Count = 0 Then
    '        Return "-1"
    '    End If
    '    Dim ArrCategories As New ArrayList
    '    Dim dt As DataTable = HttpContext.Current.Session("dtChartOnlyRommInPersonalMode")
    '    If dt.Rows.Count > 0 Then
    '        For index = 0 To dt.Rows.Count - 1
    '            ArrCategories.Add(dt.Rows(index)("NoInRoom"))
    '        Next
    '    Else
    '        Return "-1"
    '    End If
    '    HttpContext.Current.Session("dtChartOnlyRommInPersonalMode") = Nothing
    '    Dim StrChart As String = ""
    '    If TypeChart = "1" Then
    '        StrChart = ClsReport.GenStrStackColumnChart(Title, Subtitle, Ytitle, "CreateChartStudentOnly", DataHash, ArrCategories, "เลขที่")
    '    ElseIf TypeChart = "2" Then
    '        StrChart = ClsReport.GenStrLineChart(Title, Subtitle, Ytitle, DataHash, ArrCategories, "เลขที่")
    '    End If



    '    Return StrChart

    'End Function '****10**** Top10 - Low10

    '<Services.WebMethod()>
    'Public Shared Function CreateChartStudentOnly(ByVal StrRoom As String, ByVal NoInRoom As String, ByVal TypeChart As String, ByVal ChooseMode As Integer)

    '    If StrRoom Is Nothing Or StrRoom = "" Or NoInRoom Is Nothing Or NoInRoom = "" Or TypeChart Is Nothing Or TypeChart = "" Or ChooseMode = 0 Then
    '        Return "-1"
    '    End If

    '    Dim KNSession As New KNAppSession()
    '    Dim CalendarName As String = KNSession("SelectedCalendarName")
    '    Dim ClsReport As New ClassViewReport(New ClassConnectSql)
    '    Dim ArrSplit = StrRoom.Split("/")
    '    Dim CurrentClass As String = ArrSplit(0)
    '    Dim CurrentRoom As String = "/" & ArrSplit(1)
    '    'Dim AcademicYear As String = ClsReport.GetAcademicYearStudent(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom)
    '    'If AcademicYear = "" Then
    '    '    Return "-1"
    '    'End If
    '    Dim Title As String = "<b>เปอร์เซ็นต์แต่ละวิชา ทุกปีการศึกษา</b>"
    '    Dim StudentName As String = ClsReport.GetStudentName(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom)
    '    If StudentName = "" Then
    '        Return "-1"
    '    End If
    '    Dim StudentLastName As String = ClsReport.GetStudentLastName(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom)
    '    If StudentLastName = "" Then
    '        Return "-1"
    '    End If
    '    'Dim Subtitle As String = "ของ " & StudentName & " & StrRoom & " & " เลขที่ " & NoInRoom & " ปีการศึกษา " & AcademicYear
    '    Dim Subtitle As String = " ของ " & StudentName & " " & StudentLastName & " " & StrRoom & " เลขที่ " & NoInRoom & " ปีการศึกษา " & CalendarName
    '    Dim YTitle As String = "เปอร์เซ็นต์"
    '    Dim DataHash As Hashtable = ClsReport.HashChartStudentOnly(HttpContext.Current.Session("SchoolID"), CurrentClass, CurrentRoom, NoInRoom, ChooseMode)
    '    If DataHash.Count = 0 Then
    '        Return "-1"
    '    End If
    '    Dim ArrCategories As ArrayList = HttpContext.Current.Session("ArrCatChartStudentOnly")
    '    If ArrCategories.Count = 0 Then
    '        Return "-1"
    '    End If
    '    Dim StrChart As String = ""
    '    If TypeChart = "1" Then
    '        StrChart = ClsReport.GenStrStackColumnChart(Title, Subtitle, YTitle, "NoFunction", DataHash, ArrCategories, "ห้อง/เลขที่")
    '    ElseIf TypeChart = "2" Then
    '        StrChart = ClsReport.GenStrLineChart(Title, Subtitle, YTitle, DataHash, ArrCategories, "ห้อง/เลขที่")
    '    End If

    '    Return StrChart

    'End Function
#End Region





End Class