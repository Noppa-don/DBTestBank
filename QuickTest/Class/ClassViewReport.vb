Imports System.Data.SqlClient

Public Class ClassViewReport
    Dim _DB As ClsConnect
    Dim KNSession As New KNAppSession
    Public Sub New(ByVal DB As ClsConnect)
        _DB = DB
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Title">Title ตอนแสดงกราฟ</param>
    ''' <param name="Subtitle">Subtitle ตอนแสดงกราฟ</param>
    ''' <param name="YAxisTitle">Title ที่แสดงบนแกน X</param>
    ''' <param name="FunctionName">เป็นชื่อฟังก์ชั่นของ JavaScript สำหรับ DrillDown จากกราฟแท่ง</param>
    ''' <param name="DataChart">ตัวอย่าง ("ภาษาไทย","0,5,10,32,15") *เลขแต่ละหลักอิงตามลำดับ Categories</param>
    ''' <param name="ArrCategories">             ^ ^  ^
    ''' ตัวอย่าง ("เลขที่ 5")-------------------------^ ^  ^
    '''       ("เลขที่ 6")---------------------------^  ^
    '''       ("เลขที่ 7")------------------------------^
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenStrStackColumnChart(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
                                        ByVal FunctionName As String, ByVal DataChart As Hashtable, ByVal ArrCategories As ArrayList, ByVal XAxisTitle As String, Optional ByVal Width As Double = 730) As String

        If Title IsNot Nothing And Title <> "" And Subtitle IsNot Nothing And Subtitle <> "" And YAxisTitle IsNot Nothing _
    And FunctionName IsNot Nothing And FunctionName <> "" And DataChart IsNot Nothing And ArrCategories IsNot Nothing Or XAxisTitle Is Nothing Or XAxisTitle = "" Then

            Dim CodeChart As New StringBuilder
            CodeChart.Append("chart = new Highcharts.Chart({chart: {renderTo:'DivReport',width:" & Width & ",type:'column'},title: {text:'<b>$Title</b>'},")
            CodeChart.Append("exporting:{buttons:{exportButton:{enabled:false},printButton:{x:-10}}},")
            CodeChart.Append("subtitle: {text:'$Subtitle'},xAxis: {title: {text:'$XAxisTitle'},")
            CodeChart.Append("categories:[")

            If ArrCategories.Count = 0 Then
                Return "-1"
            End If

            For i = 0 To ArrCategories.Count - 1
                Dim EachCategories As String = ArrCategories(i).ToString()
                If i = ArrCategories.Count - 1 Then
                    CodeChart.Append("'" & EachCategories & "']},")
                Else
                    CodeChart.Append("'" & EachCategories & "',")
                End If
            Next

            CodeChart.Append(" yAxis: {title: {text:'$YAxisTitle'}},plotOptions: {column: {stacking:'normal',dataLabels:{enabled: false},cursor:  'pointer',")
            CodeChart.Append(" point: {events: {click: function () {$FunctionName(this.category);}}}}},")
            CodeChart.Append("series:[")

            If DataChart.Count = 0 Then
                Return "-1"
            End If

            For Each row In DataChart
                Dim EachName As String = row.Key
                Dim StrData As String = row.Value
                Dim ArrData = StrData.Split(",")
                CodeChart.Append("{name:'" & EachName & "',data:[")
                For z = 0 To ArrData.Count - 1
                    Dim EachData As Double = ArrData(z)
                    If z = ArrData.Count - 1 Then
                        CodeChart.Append("" & EachData & "]")
                    Else
                        CodeChart.Append("" & EachData & ",")
                    End If
                Next
                CodeChart.Append("},")
            Next

            Dim ForSubStr As String = CodeChart.ToString().Substring(0, CodeChart.ToString().Length - 2)
            CodeChart.Clear()
            CodeChart.Append(ForSubStr)
            CodeChart.Append("}]});")
            Dim FullStr As String = CodeChart.ToString()
            FullStr = ReplaceStrCodeChart(CodeChart.ToString(), 2, Title, Subtitle, YAxisTitle, FunctionName, XAxisTitle)

            Return FullStr

        Else
            Return "-1"
        End If

    End Function 'Function ต่อสตริง กราฟแท่ง

    Public Function GenStrLineChartNew(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
                                        ByVal dt As DataTable, ByVal XAxisTitle As String, Optional ByVal Width As Double = 730) As String

        If Title IsNot Nothing And Title <> "" And Subtitle IsNot Nothing And Subtitle <> "" And YAxisTitle IsNot Nothing _
            And YAxisTitle <> "" And XAxisTitle <> "" Then

            Dim CodeChart As New StringBuilder
            CodeChart.Append(" chart = new Highcharts.Chart({chart: {renderTo:'DivReport',width:" & Width & ",type:'line'},title: {text:'<b>$Title</b>'},")
            CodeChart.Append("exporting:{buttons:{exportButton:{enabled:false},printButton:{x:-10}}},")
            CodeChart.Append(" subtitle: {text:'$Subtitle'},xAxis: {title: {text:''},")
            CodeChart.Append("categories:[")

            For z = 0 To dt.Rows.Count - 1
                If z = dt.Rows.Count - 1 Then
                    CodeChart.Append("'" & dt.Rows(z)(0) & "']")
                Else
                    CodeChart.Append("'" & dt.Rows(z)(0) & "',")
                End If
            Next

            CodeChart.Append("},yAxis: {title: {text:'$YAxisTitle'}},plotOptions: {},")
            CodeChart.Append(" series: [")

            CodeChart.Append("{name: 'คะแนน',data:[")
            For i = 0 To dt.Rows.Count - 1
                If i = dt.Rows.Count - 1 Then
                    CodeChart.Append("" & dt.Rows(i)(1) & "]")
                Else
                    CodeChart.Append("" & dt.Rows(i)(1) & ",")
                End If
            Next
            CodeChart.Append("},")

            Dim ForSubStr As String = CodeChart.ToString.Substring(0, CodeChart.ToString().Length - 2)
            CodeChart.Clear()
            CodeChart.Append(ForSubStr)
            CodeChart.Append(" }]});")
            Dim FullStr As String = CodeChart.ToString()

            FullStr = ReplaceStrCodeChart(FullStr, 2, Title, Subtitle, YAxisTitle, "NoFunction", XAxisTitle) 'มีปัญหาเพราะว่าไม่มี Function แล้ว

            Return FullStr

        Else
            Return "-1"
        End If

    End Function 'Function ต่อสตริง กราฟเส้น

    Public Function GenStrLineChart(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
                                        ByVal DataChart As Hashtable, ByVal ArrCategories As ArrayList, ByVal XAxisTitle As String, Optional ByVal Width As Double = 730) As String

        If Title IsNot Nothing And Title <> "" And Subtitle IsNot Nothing And Subtitle <> "" And YAxisTitle IsNot Nothing _
            And YAxisTitle <> "" And DataChart IsNot Nothing And ArrCategories IsNot Nothing Or XAxisTitle Is Nothing Or XAxisTitle = "" Then

            Dim CodeChart As New StringBuilder
            CodeChart.Append(" chart = new Highcharts.Chart({chart: {renderTo:'DivReport',width:" & Width & ",type:'line'},title: {text:'<b>$Title</b>'},")
            CodeChart.Append("exporting:{buttons:{exportButton:{enabled:false},printButton:{x:-10}}},")
            CodeChart.Append(" subtitle: {text:'$Subtitle'},xAxis: {title: {text:''},")
            CodeChart.Append("categories:[")

            If ArrCategories.Count = 0 Then
                Return "-1"
            End If

            For z = 0 To ArrCategories.Count - 1
                Dim EachCategories As String = ArrCategories(z).ToString()
                If z = ArrCategories.Count - 1 Then
                    CodeChart.Append("'" & EachCategories & "']")
                Else
                    CodeChart.Append("'" & EachCategories & "',")
                End If
            Next

            CodeChart.Append("},yAxis: {title: {text:'$YAxisTitle'}},plotOptions: {},")
            CodeChart.Append(" series: [")

            If DataChart.Count = 0 Then
                Return "-1"
            End If

            For Each row In DataChart
                Dim EachName As String = row.Key
                Dim DataStr As String = row.Value
                Dim ArrData = DataStr.Split(",")
                CodeChart.Append("{name: '" & EachName & "',data:[")
                For i = 0 To ArrData.Count - 1
                    Dim Eachdata As Double = ArrData(i)
                    If i = ArrData.Count - 1 Then
                        CodeChart.Append("" & Eachdata & "]")
                    Else
                        CodeChart.Append("" & Eachdata & ",")
                    End If
                Next
                CodeChart.Append("},")
            Next

            Dim ForSubStr As String = CodeChart.ToString.Substring(0, CodeChart.ToString().Length - 2)
            CodeChart.Clear()
            CodeChart.Append(ForSubStr)
            CodeChart.Append(" }]});")
            Dim FullStr As String = CodeChart.ToString()

            FullStr = ReplaceStrCodeChart(FullStr, 2, Title, Subtitle, YAxisTitle, "NoFunction", XAxisTitle) 'มีปัญหาเพราะว่าไม่มี Function แล้ว

            Return FullStr

        Else
            Return "-1"
        End If

    End Function

    Public Function GenStrDrillDownPieChart(ByVal Title As String, ByVal HashCategories As Hashtable, ByVal dtData As DataTable, ByVal FieldName As String, ByVal ClassOrRoomOrNoinRoomName As String, ByVal UnitName As String, ByVal IsQuiz As Boolean, Optional ByVal SelectField As String = "RoomName") As String

        If Title Is Nothing Or Title = "" Or HashCategories Is Nothing Or HashCategories.Count = 0 Or dtData Is Nothing Or dtData.Rows.Count = 0 Or FieldName Is Nothing Or FieldName = "" Then
            Return "-1"
        End If

        Dim CodeChart As New StringBuilder
        Dim SpareStr As String = ""
        CodeChart.Append("var colors = Highcharts.getOptions().colors,")
        CodeChart.Append(" categories =[")
        Dim SubjectName As String = ""
        For i = 0 To HashCategories.Count - 1 'วนเอา Categories จาก HashtableCategories
            If IsQuiz = False Then
                If SelectField = "RoomName" Then
                    SubjectName = GenSubjectName(HashCategories.Keys(i).ToString())
                Else
                    SubjectName = HashCategories.Keys(i).ToString()
                End If
            Else
                If SelectField = "RoomName" Then
                    SubjectName = HashCategories.Keys(i).ToString()
                Else
                    SubjectName = HashCategories.Keys(i).ToString()
                End If
            End If
            CodeChart.Append("'" & SubjectName & "',")
        Next
        SpareStr = CutCommaLastString(CodeChart.ToString())
        CodeChart.Clear()
        CodeChart.Append(SpareStr & "],")
        CodeChart.Append(" name = '" & Title & "',")
        CodeChart.Append("data = [")
        For a = 0 To HashCategories.Count - 1 'วนตามวิชาใน Hastable เพื่อสร้าง Data
            Dim CurrentSubjectName As String = HashCategories.Keys(a).ToString()
            Dim DataY As String = HashCategories(CurrentSubjectName).ToString()
            CodeChart.Append("{y: " & DataY & ",color: colors[" & a & "],drilldown:{categories:[")
            Dim dtRow() As DataRow
            If IsQuiz = True Then
                dtRow = dtData.Select(SelectField & " = '" & CurrentSubjectName & "' ") 'หา Row จาก Datatable โดยหาจากห้อง
            Else
                dtRow = dtData.Select("SubjectName = '" & CurrentSubjectName & "' ") 'หา Row จาก Datatable โดยหาจากวิชา
            End If
            Dim EachCategories As New ArrayList
            Dim EachData As New ArrayList
            If dtRow.Length > 0 Then
                For Each objrow In dtRow
                    If IsQuiz = False Then
                        EachCategories.Add(objrow("" & FieldName & "")) 'นำห้องที่มีในวิชาที่หามา Add ลง Array
                    Else
                        EachCategories.Add(GenSubjectName(objrow("" & FieldName & "")))
                    End If

                    EachData.Add(objrow("TotalScore")) 'นำคะแนนจากห้องข้างบนมา Add ลง Array
                Next
                For z = 0 To EachCategories.Count - 1
                    CodeChart.Append("'" & EachCategories(z) & "',") 'วนต่อสตริงว่ามีห้องอะไรบ้างที่สอบวิชานี้
                Next
                SpareStr = CutCommaLastString(CodeChart.ToString())
                CodeChart.Clear()
                CodeChart.Append(SpareStr & "],data:[")
                For x = 0 To EachData.Count - 1
                    CodeChart.Append("" & EachData(x) & ",") 'วนต่อสตริงคะแนนว่าห้องที่สอบวิชานี้ได้กี่คะแนน
                Next
                SpareStr = CutCommaLastString(CodeChart.ToString())
                CodeChart.Clear()
                CodeChart.Append(SpareStr & "],color: colors[" & a & "]}},")
            End If
        Next
        SpareStr = CutCommaLastString(CodeChart.ToString())
        CodeChart.Clear()
        CodeChart.Append(SpareStr)
        CodeChart.Append("];var TotalData = [];var Classdata = [];for (var i = 0; i < data.length; i++) {TotalData.push({name: categories[i],y: data[i].y,")
        CodeChart.Append("color:data[i].color});for (var j = 0; j < data[i].drilldown.data.length; j++) {var brighhness = 0.2 - (j / data[i].drilldown.data.length) / 5;")
        CodeChart.Append("Classdata.push({name: data[i].drilldown.categories[j],y: data[i].drilldown.data[j],color: Highcharts.Color(data[i].color).brighten(brighhness).get()")
        CodeChart.Append("});}}")
        CodeChart.Append("chart = new Highcharts.Chart({chart: {renderTo: 'DivReport',type: 'pie'},title: { text: '" & Title & "'},exporting: {buttons: {exportButton: {enabled: false},printButton: {x:-10}}},plotOptions: {pie: {shadow: true,allowPointSelect:true}},")
        CodeChart.Append("tooltip: {formatter: function () {return '<b>'  + this.point.name + '</b>: ' + this.y + '" & UnitName & "';}},series: [{name: 'วิชา',data: TotalData,size: '60%',")
        CodeChart.Append("dataLabels: {formatter: function () {return this.y > 5 ? this.point.name : null;},color: 'white',distance: -30}}, { name: 'ห้อง',data: Classdata,")
        CodeChart.Append("innerSize: '60%',dataLabels: {formatter: function () {return this.y > 1 ? '<b>' + '" & ClassOrRoomOrNoinRoomName & "' + this.point.name + ':</b>' + this.y + '" & UnitName & "' : null;}}}]});")

        Return CodeChart.ToString()

    End Function 'Function ต่อสตริง กราฟวงกลม

    'Function สร้างกราฟวงกลมแบบวงเดียว ตอนแรก ***อันใหม่
    Public Function GenStrPieChart(ByVal Title As String, ByVal dt As DataTable, Optional ByVal IsFirstChart As Boolean = True)

        Dim CodeChart As New StringBuilder
        Dim KeyName As String = ""
        Dim Quantity As Double = 0

        CodeChart.Append(" chart = new Highcharts.Chart({")
        CodeChart.Append("chart:{type:'pie',renderTo:'DivReport'},title:{text:'" & Title & "'},tooltip:{pointFormat:'{series.name}:<b>{point.y}%</b>'},")
        CodeChart.Append("plotOptions:{pie:{allowPointSelect:true,cursor:'pointer',dataLabels:{enabled:true,color:'#000000',connectorColor:'#000000',format:'<b>{point.name}</b>: {this.y} %' ")
        CodeChart.Append("}}},series:[{type:'pie',name:'" & Title & "',data:[ ")

        For index = 0 To dt.Rows.Count - 1
            If IsFirstChart = True Then
                KeyName = GenSubjectName(dt.Rows(index)(0).ToString())
            Else
                KeyName = dt.Rows(index)(0).ToString()
            End If
            Quantity = dt.Rows(index)(1)
            If index = dt.Rows.Count - 1 Then
                CodeChart.Append("['" & KeyName & "'," & Quantity & "]")
            Else
                CodeChart.Append("['" & KeyName & "'," & Quantity & "],")
            End If
        Next
        CodeChart.Append("]}]});")

        Return CodeChart.ToString()

    End Function

    'Function สร้างกราฟแท่งแบบธรรมดา ไว้ใช้กับพวก คะแนนเฉลี่ย ควิซ ฝึกฝน การบ้าน
    Public Function GenStrBasicColumnChart(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, ByVal dt As DataTable, Optional ByVal Width As Double = 730, Optional ByVal SeriesName As String = "คะแนนเฉลี่ย")

        Dim CodeChart As New StringBuilder
        CodeChart.Append(" chart = new Highcharts.Chart({")
        CodeChart.Append(" chart: {type: 'column',renderTo: 'DivReport',width:" & Width & "},title: {text: '" & Title & "'},subtitle:{text:'" & Subtitle & "'},xAxis: {categories: [")
        For index = 0 To dt.Rows.Count - 1
            If index = dt.Rows.Count - 1 Then
                CodeChart.Append("'เลขที่ " & dt.Rows(index)("NoInRoom").ToString() & "'")
            Else
                CodeChart.Append("'เลขที่ " & dt.Rows(index)("NoInRoom").ToString() & "',")
            End If
        Next
        CodeChart.Append("]}, yAxis: {min: 0,title: {text: '" & YAxisTitle & "'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'" & SeriesName & "',data:[")
        For z = 0 To dt.Rows.Count - 1
            If z = dt.Rows.Count - 1 Then
                CodeChart.Append(dt.Rows(z)("Totalscore"))
            Else
                CodeChart.Append(dt.Rows(z)("Totalscore") & ",")
            End If
        Next
        CodeChart.Append(" ]}] });")
        Return CodeChart.ToString()

    End Function

    Public Function GenStrBasicDrillDownColumnChart(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
                                        ByVal FunctionName As String, ByVal dt As DataTable, ByVal XAxisTitle As String, Optional ByVal Width As Double = 730, Optional ByVal PointStr As String = "%", Optional ByVal SeriesName As String = "คะแนนเฉลี่ย")
        Dim CodeChart As New StringBuilder
        CodeChart.Append(" chart = new Highcharts.Chart({")
        CodeChart.Append(" chart: {type: 'column',renderTo: 'DivReport',width:" & Width & "},title: {text: '" & Title & "'},subtitle:{text:'" & Subtitle & "'},tooltip:{pointFormat:'{series.name}:<b>{point.y} " & PointStr & "</b>'},xAxis: {categories: [")
        For index = 0 To dt.Rows.Count - 1
            If index = dt.Rows.Count - 1 Then
                CodeChart.Append("'" & dt.Rows(index)(0).ToString() & "'")
            Else
                CodeChart.Append("'" & dt.Rows(index)(0).ToString() & "',")
            End If
        Next
        CodeChart.Append("],title:{text:'" & XAxisTitle & "'}}, yAxis: {min: 0,title: {text: '" & YAxisTitle & "'}}, plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name:'" & SeriesName & "',data:[")
        For z = 0 To dt.Rows.Count - 1
            If z = dt.Rows.Count - 1 Then
                CodeChart.Append(dt.Rows(z)(1))
            Else
                CodeChart.Append(dt.Rows(z)(1) & ",")
            End If
        Next
        CodeChart.Append(" ],point: { events: { click: function () { " & FunctionName & "(this.category); } } }}] });")
        Return CodeChart.ToString()
    End Function

    Public Function ReplaceStrCodeChart(ByVal CodeChart As String, ByVal ChartType As Integer, ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
                                       ByVal FunctionName As String, ByVal XAxisTitle As String) As String

        Dim ReplaceStr As String = CodeChart

        If ChartType = 0 Then
            Return "-1"
        ElseIf ChartType = 1 Then 'ถ้า 1 คือ กราฟเส้น
            ReplaceStr.Replace("$Type", "line")
        ElseIf ChartType = 2 Then 'ถ้า 2 คือ กราฟแท่ง
            ReplaceStr.Replace("$Type", "column")
        ElseIf ChartType = 3 Then 'ถ้า 3 คือ กราฟวงกลม
            ReplaceStr.Replace("$Type", "pie")
        End If

        ReplaceStr = ReplaceStr.Replace("$Title", Title)
        ReplaceStr = ReplaceStr.Replace("$Subtitle", Subtitle)
        ReplaceStr = ReplaceStr.Replace("$YAxisTitle", YAxisTitle)
        ReplaceStr = ReplaceStr.Replace("$XAxisTitle", XAxisTitle)
        ReplaceStr = ReplaceStr.Replace("$FunctionName", FunctionName)

        Return ReplaceStr

    End Function 'Function Replace สตริงพวก Title,Subtile,XAxisTile,YAxistitle,FunctionName(ชื่อ Function ทาง Javascript ที่ไว้ใช้กด DrillDown จากกราฟแท่ง)

    Public Function dtChartMainLevel(ByVal SchoolId As String, ByVal ClassName As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

        Dim dt As New DataTable
        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("ClassName")
        dtComplete.Columns.Add("TotalScore")
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        If SchoolId Is Nothing Or SchoolId = "" Or ClassName Is Nothing Or ClassName.Count = 0 Or SortType Is Nothing Or SortType = "" Then
            Return dtComplete
        End If

        If SortType = "0" Then 'ถ้าไม่ได้เป็นโหมด Top10 Low10 เข้าเงื่อนไขนี้
            Dim sql As New StringBuilder
            'sql.Append(" SELECT ClassName,TotalScore FROM dbo.uvw_Chart_ClassSubjectTotalscoreNew  Where SchoolCode = '")
            sql.Append(" SELECT ClassName,sum(totalscore) as TotalScore FROM dbo.uvw_Chart_ClassSubjectTotalscoreNew  Where SchoolCode = '")
            sql.Append(_DB.CleanString(SchoolId.Trim()))
            sql.Append("' ")
            Dim IsMoreOneClass As Boolean = False
            If ClassName.Count > 1 Then
                IsMoreOneClass = True
                sql.Append(" And ( ")
                For z = 0 To ClassName.Count - 1
                    sql.Append("ClassName = '" & ClassName(z) & "' OR ")
                Next
            Else
                sql.Append(" And ClassName = '" & ClassName(0) & "' ")
            End If

            Dim FullSql As String = ""
            If IsMoreOneClass = True Then
                FullSql = sql.ToString().Substring(0, sql.ToString().Length - 3) & ") And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                If TeacherId <> "" Then
                    FullSql &= " AND Assistant_id = '" & TeacherId.ToString() & "' "
                End If
                FullSql &= " group by classname ORDER BY ClassName; "       ' groupby datable ให้รวมคะแนนของครูแต่ละคน รวมกันเป็นหนึ่งเดียว
            Else
                FullSql = sql.ToString() & " And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1'  "
                If TeacherId <> "" Then
                    FullSql &= " AND Assistant_id = '" & TeacherId.ToString() & "' "
                End If
                FullSql &= " group by classname ORDER BY ClassName; "        ' groupby datable ให้รวมคะแนนของครูแต่ละคน รวมกันเป็นหนึ่งเดียว
            End If

            dtComplete = _DB.getdata(FullSql)
        Else 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไขนี้
            dt = CreateDtTopLow(SchoolId, ClassName, SortType, "ClassSubjectTotalscoreNew", "ClassName", ChooseMode, "ClassName")
            If dt.Rows.Count > 0 Then
                Dim EachClassName As String
                For t = 0 To dt.Rows.Count - 1
                    EachClassName = dt.Rows(t)("ClassName").ToString()
                    Dim StrSql As String = " SELECT ClassName,TotalScore FROM dbo.uvw_Chart_ClassSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                                           " AND ClassName = '" & _DB.CleanString(EachClassName.Trim()) & "' And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    Dim dt2 As New DataTable
                    dt2 = _DB.getdata(StrSql)
                    If dt2.Rows.Count > 0 Then
                        For Each objrow In dt2.Rows
                            dtComplete.Rows.Add(objrow(0), objrow(1))
                        Next
                    Else
                        dtComplete.Rows.Add(EachClassName, "0")
                    End If
                Next
            Else
                Return dtComplete
            End If
        End If

        
        Return dtComplete

    End Function '****1**** 

    Public Function dtChartAllRoomInClassChoose(ByVal SchoolId As String, ByVal StrChooseAllRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("RoomName")
        dtComplete.Columns.Add("TotalScore")

        If SchoolId Is Nothing Or SchoolId = "" Or StrChooseAllRoom Is Nothing Or StrChooseAllRoom = "" Or SortType Is Nothing Or SortType = "" Then
            Return dtComplete
        End If

        Dim sql As String = ""
        Dim dt As New DataTable
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        If SortType = "0" Then 'ถ้าไม่เป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
            sql = " SELECT RoomName,TotalScore FROM dbo.uvw_Chart_ClassRoomSubjectTotalscoreNew " & _
                  " WHERE SchoolCode = '" & _DB.CleanString(SchoolId) & "' AND ClassName = '" & _DB.CleanString(StrChooseAllRoom.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
            If TeacherId <> "" Then
                sql &= " AND Assistant_id = '" & TeacherId & "' "
            End If
            sql &= " ORDER BY RoomName "
            dtComplete = _DB.getdata(sql)
        ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไข query นี้
            Dim DummyArrClass As New ArrayList
            DummyArrClass.Add(StrChooseAllRoom)
            dt = CreateDtTopLow(SchoolId, DummyArrClass, SortType, "ClassRoomSubjectTotalscoreNew", "RoomName", ChooseMode, "ClassName")
            If dt.Rows.Count > 0 Then
                Dim EachRoomName As String
                For t = 0 To dt.Rows.Count - 1
                    EachRoomName = dt.Rows(t)("RoomName").ToString()
                    Dim StrSql As String = " SELECT RoomName,TotalScore FROM dbo.uvw_Chart_ClassRoomSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                                           " AND RoomName = '" & _DB.CleanString(EachRoomName.Trim()) & "' And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    If TeacherId <> "" Then
                        sql &= " AND Assistant_id = '" & TeacherId & "' "
                    End If
                    Dim dt2 As New DataTable
                    dt2 = _DB.getdata(StrSql)
                    If dt2.Rows.Count > 0 Then
                        For Each objrow In dt2.Rows
                            dtComplete.Rows.Add(objrow(0), objrow(1))
                        Next
                    Else
                        dtComplete.Rows.Add(EachRoomName, "0")
                    End If
                Next
            Else
                Return dtComplete
            End If
        End If

        Return dtComplete

    End Function '****2**** 

    Public Function dtChartClass(ByVal SchoolId As String, ByVal StrClass As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") 'Function นี้ใช้รวมกันกับกราฟ Activity (5)

        Dim dtComplete As New DataTable

        If SchoolId Is Nothing Or SchoolId = "" Or StrClass Is Nothing Or StrClass.Count = 0 Or SortType Is Nothing Or SortType = "" Then
            Return dtComplete
        End If

        Dim dt As New DataTable
        dtComplete.Columns.Add("RoomName")
        dtComplete.Columns.Add("TotalScore")
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        'Dim ViewName As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
            'ViewName = "Homework"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As New StringBuilder
        If SortType = "0" Then 'ถ้าไม่เป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
            Dim IsMoreOneRoom As Boolean = False
            If TestSetId = "" Then 'ถ้าเป็นโหมดฝึกฝนใช้ Query อีกแบบนึง
                sql.Append(" SELECT RoomName,sum(TotalScore) as TotalScore FROM dbo.uvw_Chart_RoomSubjectTotalscoreNew  WHERE SchoolCode = '" & SchoolId & "' AND ")
            Else
                sql.Append(" SELECT RoomName,CAST(SUM(ResultScore) * 100 / SUM(FullScore)AS DECIMAL(10,2)) AS TotalScore " & _
                           " FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNoSum WHERE SchoolCode = '" & SchoolId & "' And ")
            End If
            'sql.Append(" StudentCalendarId = '" & CalendarId & "' AND ")
            If StrClass.Count > 1 Then
                IsMoreOneRoom = True
                sql.Append("(")
                For index = 0 To StrClass.Count - 1
                    sql.Append(" RoomName = '" & _DB.CleanString(StrClass(index).ToString().Trim()) & "' OR ")
                Next
            Else
                sql.Append(" RoomName = '" & _DB.CleanString(StrClass(0).ToString().Trim()) & "' ")
            End If
            Dim FullStr As String
            If IsMoreOneRoom = True Then
                FullStr = sql.ToString().Substring(0, sql.ToString().Length - 3) & ")" & " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                If TeacherId <> "" Then
                    FullStr &= " AND Assistant_id = '" & TeacherId & "' "
                End If
                If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where TestsetId และ Group By เพิ่ม
                    FullStr &= " AND TestSet_Id = '" & TestSetId & "' GROUP BY SubjectName,RoomName "
                Else
                    FullStr &= " GROUP BY RoomName "
                End If
                FullStr &= " ORDER BY RoomName "
            Else
                FullStr = sql.ToString() & " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                If TeacherId <> "" Then
                    FullStr &= " AND Assistant_id = '" & TeacherId & "' "
                End If
                If TestSetId <> "" Then
                    FullStr &= " AND TestSet_Id = '" & TestSetId & "' GROUP BY RoomName "
                Else
                    FullStr &= " GROUP BY RoomName "
                End If
                FullStr &= " ORDER BY RoomName; "
            End If
            dtComplete = _DB.getdata(FullStr)
        ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
            If TestSetId = "" Then
                dt = CreateDtTopLow(SchoolId, StrClass, SortType, "RoomsubjectTotalscoreNew", "RoomName", ChooseMode, "RoomName", TestSetId) 'เข้า Function เพื่อเอาลำดับห้องตามที่ Sort
            Else
                dt = CreateDtTopLow(SchoolId, StrClass, SortType, "RoomTestsetSubjectTotalscoreNoSum ", "RoomName", ChooseMode, "RoomName", TestSetId)
            End If
            If dt.Rows.Count > 0 Then
                Dim EachRoomName As String
                For t = 0 To dt.Rows.Count - 1
                    EachRoomName = dt.Rows(t)("RoomName").ToString()
                    Dim StrSql As String = ""
                    If TestSetId = "" Then
                        StrSql = " SELECT RoomName,TotalScore FROM dbo.uvw_Chart_RoomSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                                 " AND RoomName = '" & _DB.CleanString(EachRoomName.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                        If TeacherId <> "" Then
                            StrSql &= " AND Assistant_id = '" & TeacherId & "' "
                        End If
                    Else 'ถ้าเป็นโหมดฝึกฝนต้องใช้ Query นี้
                        StrSql = "  SELECT RoomName,CAST(SUM(ResultScore) * 100 / SUM(FullScore)AS DECIMAL(10,2)) AS TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNoSum WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                                 " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' " & _
                                 " AND TestSet_Id = '" & TestSetId & "' AND RoomName = '" & EachRoomName & "' "
                        If TeacherId <> "" Then
                            StrSql &= " AND Assistant_id = '" & TeacherId & "' "
                        End If
                        StrSql &= " GROUP BY RoomName "
                    End If
                    Dim dt2 As New DataTable
                    dt2 = _DB.getdata(StrSql)
                    If dt2.Rows.Count > 0 Then
                        For Each objrow In dt2.Rows
                            dtComplete.Rows.Add(objrow(0), objrow(1))
                        Next
                    Else
                        dtComplete.Rows.Add(EachRoomName, "0")
                    End If
                Next
            Else
                Return dtComplete
            End If
        End If

        Return dtComplete

    End Function '****3**** 

    Public Function dtChartRoom(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("NoInRoom")
        dtComplete.Columns.Add("TotalScore")
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
            Return dtComplete
        End If
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As String
        If SortType = "0" Then 'ถ้าไม่มีการเลือก Top10 Low10 จะเข้าเงื่อนไขนี้
            sql = " SELECT NoInRoom,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                  " AND RoomName = '" & _DB.CleanString(StrRoom.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY NoInRoom "
            dtComplete = _DB.getdata(sql)
        ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเลือก Top10 หรือ Low10 จะเข้าเงื่อนไขนี้
            sql = " SELECT Top 10 NoInRoom,TotalScore FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                  " AND RoomName = '" & _DB.CleanString(StrRoom) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY TotalScore  "
            If SortType = "1" Then
                sql = sql & " desc "
            Else
                sql = sql & " asc "
            End If
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                Dim EachNoInRoom As String
                For z = 0 To dt.Rows.Count - 1
                    EachNoInRoom = dt.Rows(z)("NoInRoom").ToString()
                    sql = " SELECT NoInRoom,CASE WHEN TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew " & _
                          " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND RoomName = '" & _DB.CleanString(StrRoom) & "' " & _
                          " AND NoInRoom = '" & EachNoInRoom & "' AND Calendar_Id  = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    dt2 = _DB.getdata(sql)
                    If dt2.Rows.Count > 0 Then
                        For Each objrow In dt2.Rows
                            dtComplete.Rows.Add(objrow(0), objrow(1))
                        Next
                    Else
                        dtComplete.Rows.Add(EachNoInRoom, "0")
                    End If
                Next
            Else
                Return dtComplete
            End If
        End If

        Return dtComplete

    End Function '****4**** 

    Public Function dtChartActivity(ByVal SchoolId As String, ByVal TestSet_ID As String, ByVal ArrRoom As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer)
        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("RoomName")
        dtComplete.Columns.Add("TotalScore")
        dtComplete.Columns.Add("Quiz_Id")
        If SchoolId Is Nothing Or SchoolId = "" Or TestSet_ID Is Nothing Or TestSet_ID = "" Or ArrRoom.Count = 0 Or SortType Is Nothing Or SortType = "" Then
            Return dtComplete
        End If
        Dim sql As String
        Dim dt As New DataTable
        Dim RoomName As String = ""
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim ArrClassRoom As New ArrayList
        Dim ArrQuizId As New ArrayList
      
        If SortType = "0" Then 'ถ้าไม่ได้เป็น Top10 Low10 จะเข้าอันนี้เอาทุกครั้งที่ทำควิซ
            For i = 0 To ArrRoom.Count - 1 'ถ้าเป็นโหมดปกติ (SortType = 0) จะมาทำอันนี้โดยตรงเลย
                RoomName = _DB.CleanString(ArrRoom(i).ToString().Trim())
                sql = " SELECT  RoomName,TotalScore,Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                      " AND TestSet_Id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND RoomName = '" & RoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY RoomName,LastUpdate "
                dt = _DB.getdata(sql)
                If dt.Rows.Count > 0 Then
                    For Each objrow In dt.Rows
                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
                    Next
                End If
            Next
            If dtComplete.Rows.Count > 0 Then 'วน Loop เพื่อต่อสตริงครั้งที่การทำควิซ 
                Dim Counter As Integer = 1
                Dim EachRoomName As String = ""
                For index = 0 To dtComplete.Rows.Count - 1
                    If index = 0 Then
                        EachRoomName = dtComplete.Rows(index)(0)
                        dtComplete.Rows(index)(0) = dtComplete.Rows(index)(0) & "," & "ครั้งที่" & Counter
                    Else
                        If EachRoomName = dtComplete.Rows(index)(0) Then
                            Counter += 1
                        Else
                            Counter = 1
                        End If
                        EachRoomName = dtComplete.Rows(index)(0)
                        dtComplete.Rows(index)(0) = dtComplete.Rows(index)(0) & "," & "ครั้งที่" & Counter
                    End If
                Next
            End If
        End If

        If SortType = "1" Or SortType = "2" Then 'ถ้าเลือกแบบ Top10 - Low10 ต้อง Select Top 10 Quiz_id กับ ห้อง มา
            Dim dt2 As New DataTable
            dt2.Columns.Add("RoomName")
            dt2.Columns.Add("TotalScore")
            dt2.Columns.Add("Quiz_Id")
            For i = 0 To ArrRoom.Count - 1 'ถ้าเป็นโหมดปกติ (SortType = 0) จะมาทำอันนี้โดยตรงเลย
                RoomName = _DB.CleanString(ArrRoom(i).ToString().Trim())
                sql = " SELECT  RoomName,TotalScore,Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                      " AND TestSet_Id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND RoomName = '" & RoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY RoomName,LastUpdate "
                dt = _DB.getdata(sql)
                If dt.Rows.Count > 0 Then
                    For Each objrow In dt.Rows
                        dt2.Rows.Add(objrow(0), objrow(1), objrow(2))
                    Next
                End If
            Next
            If dt2.Rows.Count > 0 Then 'วน Loop เพื่อต่อสตริงครั้งที่การทำควิซ 
                Dim Counter As Integer = 1
                Dim EachRoomName As String = ""
                For index = 0 To dt2.Rows.Count - 1
                    If index = 0 Then
                        EachRoomName = dt2.Rows(index)(0)
                        dt2.Rows(index)(0) = dt2.Rows(index)(0) & "," & "ครั้งที่" & Counter
                    Else
                        If EachRoomName = dt2.Rows(index)(0) Then
                            Counter += 1
                        Else
                            Counter = 1
                        End If
                        EachRoomName = dt2.Rows(index)(0)
                        dt2.Rows(index)(0) = dt2.Rows(index)(0) & "," & "ครั้งที่" & Counter
                    End If
                Next
            End If
            'หา Top 10
            sql = " SELECT TOP 10 Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                  " AND TestSet_Id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND ( "
            For z = 0 To ArrRoom.Count - 1
                sql &= " RoomName = '" & ArrRoom(z).ToString().Trim() & "' OR"
            Next
            If sql.EndsWith("OR") = True Then
                sql = sql.Substring(0, sql.Length - 2)
            End If
            sql &= " ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1'  "
            If SortType = 1 Then
                sql &= " ORDER BY TotalScore desc "
            Else
                sql &= " ORDER BY TotalScore asc "
            End If
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                For index = 0 To dt.Rows.Count - 1
                    Dim dtrow() As DataRow = dt2.Select("Quiz_Id = '" & dt.Rows(index)(0).ToString() & "'")
                    If dtrow.Length > 0 Then
                        dtComplete.Rows.Add(dtrow(0)(0), dtrow(0)(1), dtrow(0)(2))
                    End If
                Next
            End If
        End If

        Return dtComplete

    End Function '****5**** 

    Public Function dtChartActivityPerQuiz(ByVal SchoolId As String, ByVal TestSet_Id As String, ByVal RoomName As String, ByVal QuizTime As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'หาคะแนนเฉลี่่ย ของชุด,ครั้งที่,ห้อง
        Dim dt As New DataTable
        Dim dtcomplete As New DataTable
        dtcomplete.Columns.Add("NoInRoom")
        dtcomplete.Columns.Add("TotalScore")
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As String = ""
        Dim QuizId As String = ""
        If ChooseMode = 1 Then 'ถ้าเป็นโหมด Quiz เอาคะแนนดิบของควิซเลย
            sql = " SELECT Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                  " AND TestSet_Id = '" & _DB.CleanString(TestSet_Id.Trim()) & "' AND RoomName = '" & RoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY LastUpdate "
            dt = _DB.getdata(sql)
            If dt.Rows.Count > 0 Then
                Dim TargetQuizId As String = dt.Rows((QuizTime - 1))(0).ToString()
                sql = " SELECT NoInRoom,ResultScore FROM dbo.uvw_Chart_StudentTestSetSubjectTotalScoreNew WHERE Quiz_Id = '" & TargetQuizId & "' "
                If SortType = "0" Then
                    sql &= " ORDER BY NoInRoom "
                ElseIf SortType = "1" Then
                    sql &= " ORDER BY ResultScore desc "
                ElseIf SortType = "2" Then
                    sql &= " ORDER BY ResultScore asc "
                End If
                dtcomplete = _DB.getdata(sql)
            End If
        Else 'ถ้าเป็นการบ้าน , ฝึกฝน หาแต่คะแนนเฉลี่ยของ ห้อง และ Testset_Id ที่เลือกมา
            sql = " SELECT NoInRoom,CAST(SUM( ResultScore) * 100  /SUM(FullScore) AS DECIMAL(10,2)) AS TotalScore " & _
                  " FROM dbo.uvw_Chart_StudentTestSetSubjectTotalScoreNew WHERE Calendar_Id = '" & CalendarId & "' " & _
                  " AND TestSet_Id = '" & TestSet_Id & "' AND SchoolCode = '" & SchoolId & "' AND " & StrWhere & " = 1 GROUP BY NoInRoom "
            If SortType = "0" Then
                sql &= " ORDER BY NoInRoom "
            ElseIf SortType = "1" Then
                sql &= " ORDER BY CAST(SUM( ResultScore) * 100  /SUM(FullScore) AS DECIMAL(10,2)) desc "
            ElseIf SortType = "2" Then
                sql &= " ORDER BY CAST(SUM( ResultScore) * 100  /SUM(FullScore) AS DECIMAL(10,2)) asc "
            End If
            dtcomplete = _DB.getdata(sql)
        End If

        Return dtcomplete
    End Function '****6**** 

    Public Function HashChartAvgStudentByTestSetId(ByVal SchoolId As String, ByVal TestSet_ID As String, ByVal RoomName As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim DataHash As New Hashtable
        Dim DataHash2 As New Hashtable
        Dim dt As New DataTable

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As String = ""

        sql = " SELECT NoInRoom,TotalScore,SubjectName FROM dbo.uvw_Chart_AvgStudentScoreBySubject WHERE " & _
              " TestSet_Id = '" & TestSet_ID & "' AND Calendar_Id = '" & CalendarId & " ' AND " & StrWhere & " = '1' AND RoomName = '" & _DB.CleanString(RoomName) & "' "
        If SortType = "0" Then
            sql &= " ORDER BY NoInRoom "
        ElseIf SortType = "1" Then
            sql &= " ORDER BY TotalScore DESC "
        ElseIf SortType = "2" Then
            sql &= " ORDER BY TotalScore ASC "
        End If
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            Dim ArrCategories As New ArrayList
            Dim ArrClass As New ArrayList
            For i = 0 To dt.Rows.Count - 1
                If DataHash.ContainsKey(dt.Rows(i)("SubjectName")) = False Then
                    DataHash.Add(dt.Rows(i)("SubjectName"), "")
                End If

                If ArrClass.Contains(dt.Rows(i)("NoInRoom")) = False Then
                    ArrClass.Add(dt.Rows(i)("NoInRoom"))
                    ArrCategories.Add(dt.Rows(i)("NoInRoom"))
                End If
            Next
            HttpContext.Current.Session("ArrChartNoInRoomAVG") = ArrCategories
            For Each row In DataHash.Keys
                Dim Scores As String = ""
                Dim CurrentSubject = GenSubjectName(row)
                For j = 0 To ArrClass.Count - 1
                    Dim CurrentRoom As String = ArrClass(j)
                    Dim dtrow() As DataRow = dt.Select("NoInRoom = '" & CurrentRoom & "' and SubjectName = '" & row & "' ")
                    If dtrow.Length > 0 Then
                        Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
                    Else
                        Scores = Scores & "0,"
                    End If
                Next
                If Scores.EndsWith(",") Then
                    Scores = Scores.Substring(0, Scores.Length - 1)
                End If
                DataHash2.Add(CurrentSubject, Scores)
            Next
        Else
            Return DataHash2
        End If


        Return DataHash2

    End Function

    Public Function dtChartMainPersonal(ByVal SchoolId As String, ByVal ChooseMode As Integer) As DataTable

        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("RoomNameAndNoInRoom")
        dtComplete.Columns.Add("TotalScore")

        If SchoolId Is Nothing Or SchoolId = "" Then
            Return dtComplete
        End If

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim sql As String = " SELECT RoomName,NoInRoom FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode  = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY RoomName,NoInRoom ORDER BY SUM(TotalScore) desc "
        Dim dt As New DataTable
        Dim dt2 As New DataTable

        Dim ArrCheck As New ArrayList
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                Dim FullRoomName As String = dt.Rows(index)("RoomName").ToString()
                Dim NoInRoom As String = dt.Rows(index)("NoInRoom").ToString()
                Dim ArrSplitRoomName = FullRoomName.Split("/")
                Dim ClassName As String = ArrSplitRoomName(0)
                If index = 0 Then
                    ArrCheck.Add(ClassName)
                    Dim EachRow As DataRow = dtComplete.NewRow()
                    EachRow(0) = FullRoomName & ":" & "เลขที่" & NoInRoom
                    sql = " SELECT CASE WHEN SUM(TotalScore) IS NOT NULL THEN SUM(TotalScore) ELSE 0 END AS TotalScore " & _
                          " FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                          " AND RoomName = '" & FullRoomName & "' AND NoInRoom = '" & NoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    dt2 = _DB.getdata(sql)
                    If dt2.Rows.Count > 0 Then
                        EachRow(1) = dt2.Rows(0)(0)
                        dtComplete.Rows.Add(EachRow)
                    End If
                Else
                    If ArrCheck.Contains(ClassName) = False Then
                        ArrCheck.Add(ClassName)
                        Dim EachRow As DataRow = dtComplete.NewRow()
                        EachRow(0) = FullRoomName & ":" & "เลขที่" & NoInRoom
                        sql = " SELECT CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
                              " FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                              " AND RoomName = '" & FullRoomName & "' AND NoInRoom = '" & NoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                        dt2 = _DB.getdata(sql)
                        If dt2.Rows.Count > 0 Then
                            EachRow(1) = dt2.Rows(0)(0)
                            dtComplete.Rows.Add(EachRow)
                        End If
                    End If
                End If
            Next

        Else
            Return dtComplete
        End If

        Return dtComplete

    End Function

    Public Function dtChartStudentOnly(ByVal SchoolId As String, ByVal StrClass As String, ByVal StrRoom As String, ByVal NoInRoom As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("RoomNameAndNoInRoom")
        dtComplete.Columns.Add("TotalScore")
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        If SchoolId Is Nothing Or SchoolId = "" Or StrClass Is Nothing Or StrClass = "" Or StrRoom Is Nothing Or StrRoom = "" Or NoInRoom Is Nothing Or NoInRoom = "" Or ChooseMode = 0 Then
            Return dtComplete
        End If

        Dim sql As String = " SELECT Student_Id FROM dbo.t360_tblStudent WHERE School_Code = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND Student_CurrentClass = '" & _DB.CleanString(StrClass.Trim()) & "' AND Student_CurrentRoom = '" & _DB.CleanString(StrRoom.Trim()) & "' " & _
                            " AND Student_CurrentNoInRoom = '" & NoInRoom & "' AND Student_IsActive = '1' " 'หา Student_ID จากห้องกับชั้นในปัจจุบัน
        Dim StudentId As String = _DB.ExecuteScalar(sql)
        If StudentId <> "" Then
            sql = " SELECT CalendarName + ':' +  RoomName + ':เลขที่ ' + CAST(NoInRoom AS VARCHAR(10)) AS RoomNameAndNoInRoom, " & _
                  " CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END " & _
                  " ,Calendar_Id FROM dbo.uvw_Chart_StudentAllAcademicYearNew " & _
                  " WHERE Student_Id = '" & StudentId & "' AND " & StrWhere & " = '1'  ORDER BY MoveDate "
            dtComplete = _DB.getdata(sql)
        Else
            Return dtComplete
        End If

        Return dtComplete

    End Function '****9**** 

    Public Function dtChartTopLowPerSonalMode(ByVal SchoolId As String, ByVal StrRoomName As String, ByVal StrOrder As String, ByVal ChooseMode As Integer)

        Dim dtComplete As New DataTable
        dtComplete.Columns.Add("NoInRoom")
        dtComplete.Columns.Add("TotalScore")
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoomName Is Nothing Or StrRoomName = "" Or StrOrder Is Nothing Or StrOrder = "" Or ChooseMode = 0 Then
            Return dtComplete
        End If

        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If

        Dim sql As String = " SELECT top 10 NoInRoom FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND RoomName = '" & _DB.CleanString(StrRoomName.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY NoInRoom ORDER BY SUM(TotalScore) " & StrOrder & " "
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim RoomName As String = _DB.CleanString(StrRoomName.Trim())
            For index = 0 To dt.Rows.Count - 1
                Dim CurrentNoInRoom As String = dt.Rows(index)(0).ToString()
                If index = 0 Then
                    sql = " SELECT NoInRoom,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
                          " FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                          " AND RoomName = '" & RoomName & "' AND NoInRoom = '" & CurrentNoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    dt2 = _DB.getdata(sql)
                    Dim EachRow As DataRow = dtComplete.NewRow()
                    If dt2.Rows.Count > 0 Then
                        EachRow(0) = dt2.Rows(0)(0)
                        EachRow(1) = dt2.Rows(0)(1)
                        dtComplete.Rows.Add(EachRow)
                    End If
                Else
                    sql = " SELECT NoInRoom,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
                          " FROM dbo.uvw_Chart_RoomNoSubjectTotalscoreNew WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                         " AND RoomName = '" & RoomName & "' AND NoInRoom = '" & CurrentNoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    dt2 = _DB.getdata(sql)
                    Dim EachRow As DataRow = dtComplete.NewRow()
                    If dt2.Rows.Count > 0 Then
                        EachRow(0) = dt2.Rows(0)(0)
                        EachRow(1) = dt2.Rows(0)(1)
                        dtComplete.Rows.Add(EachRow)
                    End If
                End If
            Next
        End If

        Return dtComplete

    End Function 'Top 10 - Low 10 ****10****

    Public Function CreateDtTopLow(ByVal SchoolId As String, ByVal ArrClassRoom As ArrayList, ByVal SortType As String, ByVal ViewName As String, ByVal SelectField As String, ByVal ChooseMode As Integer, Optional ByVal WhereField As String = "", Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") As DataTable

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClassRoom Is Nothing Or ArrClassRoom.Count = 0 Or SelectField Is Nothing Or SelectField = "" Or SelectField Is Nothing Or SelectField = "" Or WhereField Is Nothing _
             Or ViewName Is Nothing Or ViewName = "" Then

            Return dt
        End If

        Dim CalendarId As String = GetCalendarId(SchoolId)
        If CalendarId = "" Then
            Return dt
        End If

        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim sql As New StringBuilder
        Dim SpareStr As String
        If ChooseMode <> 1 Then
            sql.Append(" SELECT TOP 10 " & SelectField & ",CAST(SUM(ResultScore) * 100 / SUM(FullScore)AS DECIMAL(10,2)) as TotalScore FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND " & StrWhere & " = '1' ")
        Else
            sql.Append(" SELECT TOP 10 " & SelectField & ",TotalScore FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND " & StrWhere & " = '1' ")
        End If
        If WhereField <> "" Then
            sql.Append(" AND (  ")
            If ArrClassRoom.Count > 1 Then
                For u = 0 To ArrClassRoom.Count - 1
                    Dim EachRoomName As String = ArrClassRoom(u).ToString().Trim()
                    sql.Append(" " & WhereField & " = '" & _DB.CleanString(EachRoomName) & "' Or ")
                Next
                SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
                sql.Clear()
                sql.Append(SpareStr & " ) ")
            Else
                sql.Append(" " & WhereField & " ='" & _DB.CleanString(ArrClassRoom(0).ToString().Trim()) & "' ) ")
            End If
        End If
        If TeacherId <> "" Then
            sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
        End If
        If TestSetId = "" Then
            sql.Append(" AND Calendar_Id = '" & CalendarId & "' GROUP BY " & SelectField & ",TotalScore ORDER BY TotalScore ")
        Else
            sql.Append(" AND Calendar_Id = '" & CalendarId & "' AND TestSet_Id = '" & TestSetId & "' GROUP BY " & SelectField & " ORDER BY CAST(SUM(ResultScore) * 100 / SUM(FullScore)AS DECIMAL(10,2)) ")
        End If

        If SortType = "1" Then
            sql.Append(" desc ")
        Else
            sql.Append(" asc ")
        End If
        dt = _DB.getdata(sql.ToString())
        Return dt

    End Function

    Public Function GetAllRoomInClass(ByVal CurrentClass As String, ByVal SchoolId As String)

        Dim dt As New DataTable

        If CurrentClass IsNot Nothing Or CurrentClass <> "" Then
            Dim sql As String = "SELECT RoomName FROM dbo.uvw_Chart_ClassRoomSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ClassName = '" & _DB.CleanString(CurrentClass.Trim()) & "' GROUP BY RoomName"
            dt = _DB.getdata(sql)
        Else
            Return dt
        End If

        Return dt

    End Function

    Public Function GenSubjectName(ByVal SubjectName As String) As String

        If SubjectName Is Nothing Or SubjectName = "" Then
            Return "-1"
        End If

        Dim CompleteSubjectName As String
        SubjectName = SubjectName.Trim()

        Select Case SubjectName
            Case "กลุ่มสาระการเรียนรู้ภาษาไทย"
                CompleteSubjectName = "ไทย"
            Case "กลุ่่มสาระการเรียนรู้ศิลปะ"
                CompleteSubjectName = "ศิลปะ"
            Case "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี"
                CompleteSubjectName = "การงานฯ"
            Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพละศึกษา"
                CompleteSubjectName = "พละฯ"
            Case "กลุ่มสาระการเรียนรู้สุขศึกษาและพลศึกษา"
                CompleteSubjectName = "พละฯ"
            Case "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
                CompleteSubjectName = "อังกฤษ"
            Case "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม"
                CompleteSubjectName = "สังคม"
            Case "กลุ่มสาระการเรียนรู้วิทยาศาสตร์"
                CompleteSubjectName = "วิทย์"
            Case "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
                CompleteSubjectName = "คณิตฯ"
            Case Else
                CompleteSubjectName = "-1"
        End Select

        Return CompleteSubjectName

    End Function 'Function แปลงชื่อวิชา (จาก กลุ่มสาระเรียนรู้ ให้เหลือแต่ชื่อวิชา)

    Public Function GetAcademicYearStudent(ByVal SchoolId As String, ByVal StrClass As String, ByVal StrRoom As String, ByVal NoInRoom As String) As String
        Dim sql As String = " SELECT TOP 1 t360_tblStudentRoom.SR_AcademicYear FROM t360_tblStudent INNER JOIN " & _
                            " t360_tblStudentRoom ON t360_tblStudent.Student_Id = t360_tblStudentRoom.Student_Id " & _
                            " WHERE (t360_tblStudent.Student_CurrentClass = '" & StrClass & "') AND (t360_tblStudent.Student_CurrentRoom = '" & StrRoom & "') " & _
                            " AND (t360_tblStudent.Student_CurrentNoInRoom = '" & NoInRoom & "') AND (t360_tblStudent.School_Code = '" & _DB.CleanString(SchoolId.Trim()) & "') " & _
                            " ORDER BY  t360_tblStudentRoom.SR_AcademicYear desc "
        Dim AcademicYear As String = _DB.ExecuteScalar(sql)
        Return AcademicYear
    End Function

    Public Function GetTestSetNameByTestSetId(ByVal TestSet_Id As String) As String
        Dim TestSetId As String = ""
        If TestSet_Id Is Nothing Or TestSet_Id = "" Then
            Return TestSetId
        End If

        Dim sql As String = " SELECT TestSet_Name FROM dbo.tblTestSet WHERE TestSet_Id = '" & _DB.CleanString(TestSet_Id.Trim()) & "' "
        TestSetId = _DB.ExecuteScalar(sql)
        Return TestSetId
    End Function

    Public Function GetStudentName(ByVal SchoolId As String, ByVal ClassName As String, ByVal RoomName As String, ByVal NoInRoom As String) As String

        Dim StudentName As String = ""
        If SchoolId Is Nothing Or SchoolId = "" Or ClassName Is Nothing Or ClassName = "" Or RoomName Is Nothing Or RoomName = "" Or NoInRoom Is Nothing Or NoInRoom = "" Then
            Return StudentName
        End If

        Dim sql As String = " SELECT Student_FirstName FROM dbo.t360_tblStudent WHERE School_Code = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " and Student_CurrentClass = '" & _DB.CleanString(ClassName.Trim()) & "' AND Student_CurrentRoom = '" & _DB.CleanString(RoomName.Trim()) & "' AND Student_CurrentNoInRoom = '" & _DB.CleanString(NoInRoom.Trim()) & "' "
        StudentName = _DB.ExecuteScalar(sql)
        Return StudentName

    End Function

    Public Function GetStudentLastName(ByVal SchoolId As String, ByVal ClassName As String, ByVal RoomName As String, ByVal NoInRoom As String) As String

        Dim StudentLastName As String = ""

        If SchoolId Is Nothing Or SchoolId = "" Or ClassName Is Nothing Or ClassName = "" Or RoomName Is Nothing Or RoomName = "" Or NoInRoom Is Nothing Or NoInRoom = "" Then
            Return StudentLastName
        End If

        Dim sql As String = " SELECT Student_LastName FROM dbo.t360_tblStudent WHERE School_Code = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND  Student_CurrentClass = '" & _DB.CleanString(ClassName.Trim()) & "' AND Student_CurrentRoom = '" & _DB.CleanString(RoomName.Trim()) & "' AND Student_CurrentNoInRoom = '" & _DB.CleanString(NoInRoom.Trim()) & "' "
        StudentLastName = _DB.ExecuteScalar(sql)
        Return StudentLastName

    End Function

    Private Function GetQuizByTestSetIdAndRoom(ByVal SchoolId As String, ByVal TestsetId As String, ByVal StrRoom As String, ByVal QuizTime As String) As String

        Dim dt As New DataTable
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return ""
        End If
        Dim sql As String = " SELECT  quiz_id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " &
                            " AND RoomName = '" & _DB.CleanString(StrRoom.Trim()) & "' AND testset_id = '" & TestsetId & "' " & _
                            " AND Calendar_Id = '" & CalendarId & "' GROUP BY quiz_id,LastUpdate ORDER BY LastUpdate"
        dt = _DB.getdata(sql)
        Dim QuizAtTime As Integer = Integer.Parse(QuizTime) - 1
        Dim QuizId As String = ""
        If dt.Rows.Count > 0 Then
            QuizId = dt.Rows(QuizAtTime)("Quiz_Id").ToString()
        End If

        Return QuizId

    End Function

    Public Function GetQuiztTime(ByVal SchoolId As String, ByVal RoomName As String, ByVal QuizId As String, ByVal Testset_id As String) As Integer

        Dim QuizTime As Integer = 0
        If SchoolId Is Nothing Or SchoolId = "" Or RoomName Is Nothing Or RoomName = "" Or QuizId Is Nothing Or QuizId = "" Then
            Return QuizTime
        End If
        Dim sql As String = " SELECT quiz_id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND RoomName = '" & RoomName & "' AND testset_id = '" & Testset_id & "' GROUP BY quiz_id,LastUpdate ORDER BY LastUpdate  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim EachQuizId As String = dt.Rows(0)("quiz_id").ToString()
            QuizTime = 1
            For index = 0 To dt.Rows.Count - 1
                If EachQuizId = dt.Rows(index)("quiz_id").ToString() Then
                    If EachQuizId = QuizId Then
                        Exit For
                    End If
                Else
                    EachQuizId = dt.Rows(index)("quiz_id").ToString()
                    QuizTime += 1
                    If EachQuizId = QuizId Then
                        Exit For
                    End If
                End If

            Next
        Else
            Return QuizTime
        End If
        Return QuizTime

    End Function

    Public Function CutCommaLastString(ByVal InputStr As String) As String
        Dim ReturnStr As String = ""
        If InputStr.EndsWith(",") Then
            ReturnStr = InputStr.Substring(0, InputStr.Length - 1)
        End If
        Return ReturnStr
    End Function

    Private Function GetAcademicYear() As String

        Dim CurrentYear As Integer = Year(Now)
        Dim CurrentDate As New Date(Year(Now), Month(Now), Day(Now))
        Dim Fixdate As New Date(Year(Now), 3, 1)

        If DateValue(Fixdate) > DateValue(CurrentDate) Then
            CurrentYear -= 1
        End If

        If CurrentYear < 2400 Then
            CurrentYear += 543
        End If

        GetAcademicYear = CurrentYear.ToString().Substring(2)
        Return GetAcademicYear

    End Function

    Public Function dtPieQuantity(ByVal SchoolId As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "") As DataTable

        Dim dtComplete As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Then
            Return dtComplete
        End If

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dtComplete
        End If

        'Dim sql As String = " SELECT ClassName,SubjectName,TotalQuiz as TotalScore FROM dbo.uvw_Chart_QuantityQuizPerUser WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
        '                    " AND Calendar_Id = '" & CalendarId & "' "
        Dim sql As String = " SELECT SubjectName,SUM(TotalQuiz) AS TotalQuiz FROM dbo.uvw_Chart_QuantityQuizPerUser WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
                            " AND Calendar_Id = '" & CalendarId & "' "
        If TeacherId <> "" Then
            sql &= " AND Assistant_id = '" & TeacherId & "' "
        End If

        If ChooseMode = 1 Then 'Quiz
            sql &= " AND IsQuizMode = '1' "
        ElseIf ChooseMode = 2 Then 'Homework
            sql &= " AND IsHomeWorkMode = '1' "
        ElseIf ChooseMode = 3 Then 'Practice
            sql &= " AND IsPracticeMode = '1' "
        End If

        sql &= " GROUP BY SubjectName "

        dtComplete = _DB.getdata(sql)
        Dim dtReturn As New DataTable
        dtReturn = dtComplete.Clone()
        dtReturn.Columns("TotalQuiz").DataType = GetType(Double)
        If dtComplete.Rows.Count > 0 Then
            Dim SumQuantity As Integer = 0
            For index = 0 To dtComplete.Rows.Count - 1
                SumQuantity += dtComplete.Rows(index)("TotalQuiz")
            Next

            For index = 0 To dtComplete.Rows.Count - 1
                dtReturn.Rows.Add(index)("SubjectName") = dtComplete.Rows(index)("SubjectName")
                dtReturn.Rows(index)("TotalQuiz") = Format((dtComplete.Rows(index)("TotalQuiz") / SumQuantity) * 100, "0.00")
            Next
        End If

        Return dtReturn

    End Function

    Public Function HashPieQuantityQuiz(ByVal SchoolId As String, ByVal ChooseMode As Integer) As Hashtable

        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Then
            Return DataHash
        End If

        Dim CalendarId As String = GetCalendarId(SchoolId)
        If CalendarId = "" Then
            Return DataHash
        End If

        Dim sql As String = " SELECT SubjectName,SUM(TotalQuiz) as TotalQuiz FROM dbo.uvw_Chart_QuantityQuizPerUser WHERE SchoolCode = '" & SchoolId & "' " & _
                            " AND Calendar_Id = '" & CalendarId & "' "

        If ChooseMode = 1 Then 'quiz
            sql &= " AND IsQuizMode = '1' "
        ElseIf ChooseMode = 2 Then 'Homwork
            sql &= " AND IsHomeWorkMode = '1' "
        ElseIf ChooseMode = 3 Then 'Practice
            sql &= " AND IsPracticeMode = '1' "
        End If

        sql &= " GROUP BY SubjectName "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                DataHash.Add(dt.Rows(index)("SubjectName"), dt.Rows(index)("TotalQuiz"))
            Next
        End If
        Return DataHash

    End Function

    Public Function dtPieMainLeval(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "") As DataTable

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Then
            Return dt
        End If
        'If ChooseMode = 2 Then
        '    dt = QueryPieDt(SchoolId, ArrClass, "ClassSubjectTotalscoreHomework", "ClassName", "ClassName", SortType, False, False, ChooseMode, TeacherId)
        'Else
        dt = QueryPieDt(SchoolId, ArrClass, "ClassSubjectTotalscoreNew", "ClassName", "ClassName", SortType, False, False, ChooseMode, TeacherId)
        'End If
        If dt.Rows.Count > 0 Then
            Dim TotalScore As Double = 0
            For index = 0 To dt.Rows.Count - 1
                TotalScore += dt.Rows(index)(1)
            Next
            For a = 0 To dt.Rows.Count - 1
                dt.Rows(a)(1) = Math.Round(((dt.Rows(a)(1) * 100) / TotalScore), 2)
            Next
        End If

        Return dt

    End Function 'dt วงกลม MainLevel(1)

    Public Function HashPieMainlevel(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal Sortype As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "") As Hashtable

        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or Sortype Is Nothing Or Sortype = "" Then
            Return DataHash
        End If
        Dim CheckSort As Boolean
        If Sortype = "1" Or Sortype = "2" Then
            CheckSort = True
        Else
            CheckSort = False
        End If
        'If ChooseMode = 2 Then
        '    DataHash = QueryPieHashFromView(SchoolId, "ClassSubjectTotalscoreHomework", ArrClass, "ClassName", CheckSort, False, ChooseMode, , TeacherId)
        'Else
        DataHash = QueryPieHashFromView(SchoolId, "ClassSubjectTotalscore", ArrClass, "ClassName", CheckSort, False, ChooseMode, , TeacherId)
        'End If
        Return DataHash

    End Function 'Hash วงกลม MainLevel(1)

    Public Function dtPieSecondLevel(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal State As String, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal IsFromAllRoomInClass As Boolean = False)

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or State Is Nothing Or State = "" Or SortType Is Nothing Or SortType = "" Then
            Return dt
        End If
        'If State = "2" Then
        '    dt = QueryPieDt(SchoolId, ArrClass, "ClassroomSubjectTotalscorr", "RoomName", "ClassName", SortType, False, False, ChooseMode)
        'Else
        '    dt = QueryPieDt(SchoolId, ArrClass, "ClassroomSubjectTotalscore", "RoomName", "RoomName", SortType, False, False, ChooseMode)
        'End If
        If IsFromAllRoomInClass = False Then
            dt = dtChartClass(SchoolId, ArrClass, SortType, ChooseMode)
        Else
            dt = dtChartAllRoomInClassChoose(SchoolId, ArrClass(0), SortType, ChooseMode)
        End If
        If dt.Rows.Count > 0 Then
            Dim TotalScore As Double = 0
            For index = 0 To dt.Rows.Count - 1
                TotalScore += dt.Rows(index)(1)
            Next
            For a = 0 To dt.Rows.Count - 1
                dt.Rows(a)(1) = Math.Round(((dt.Rows(a)(1) * 100) / TotalScore), 2)
            Next
        End If

        Return dt

    End Function 'dt วงกลม(2-3)

    Public Function HashPieSecondLevel(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal State As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or State Is Nothing Or State = "" Or SortType Is Nothing Or SortType = "" Then
            Return DataHash
        End If
        Dim IsSort As Boolean
        If SortType = "0" Then
            IsSort = False
        ElseIf SortType = "1" Or SortType = "2" Then
            IsSort = True
        End If
        'Dim ViewName As String = ""
        'If ChooseMode = 2 Then
        '    ViewName = "Homework"
        'End If
        If State = "2" Then
            DataHash = QueryPieHashFromView(SchoolId, "ClassroomSubjectTotalscore", ArrClass, "ClassName", IsSort, False, ChooseMode)
        Else
            DataHash = QueryPieHashFromView(SchoolId, "ClassroomSubjectTotalscore", ArrClass, "RoomName", IsSort, False, ChooseMode)
        End If

        Return DataHash

    End Function 'Hash วงกลม(2-3)

    Public Function dtPieOnlyRoom(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
            Return dt
        End If
        dt = dtChartRoom(SchoolId, StrRoom, SortType, ChooseMode)
        If dt.Rows.Count > 0 Then
            Dim TotalScore As Double = 0
            For index = 0 To dt.Rows.Count - 1
                TotalScore += dt.Rows(index)(1)
            Next
            For a = 0 To dt.Rows.Count - 1
                dt.Rows(a)(1) = Math.Round(((dt.Rows(a)(1) * 100) / TotalScore), 2)
            Next
        End If

        Return dt

    End Function 'dt วงกลมเฉพาะห้อง (4)

    Public Function dtPieAVGStudentScoreByTestsetId(ByVal SchoolId As String, ByVal TestsetId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
            Return dt
        End If
        dt = QueryPieDtAVGStudentScoreByTestsetId(SchoolId, TestsetId, StrRoom, SortType, ChooseMode)
        Return dt

    End Function

    Public Function HashPieOnlyRoom(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)
        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Then
            Return DataHash
        End If
        Dim ArrClass As New ArrayList
        ArrClass.Add(StrRoom)
        Dim IsSort As Boolean
        If SortType = "0" Then
            IsSort = False
        ElseIf SortType = "1" Or SortType = "2" Then
            IsSort = True
        End If
        DataHash = QueryPieHashFromView(SchoolId, "RoomNoSubjectTotalscore", ArrClass, "RoomName", IsSort, False, ChooseMode)
        Return DataHash

    End Function 'Hash วงกลม (4)

    Public Function HashPieActivityPracticeMode(ByVal SchoolId As String, ByVal ArrRoomName As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, ByVal TestsetId As String)
        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrRoomName Is Nothing Or ArrRoomName.Count = 0 Or ChooseMode = 0 Or TestsetId Is Nothing Or TestsetId = "" Then
            Return DataHash
        End If
        Dim IsSort As Boolean
        If SortType = "0" Then
            IsSort = False
        ElseIf SortType = "1" Or SortType = "2" Then
            IsSort = True
        End If
        DataHash = QueryPieHashFromView(SchoolId, "RoomTestsetSubjectTotalscore", ArrRoomName, "RoomName", IsSort, False, ChooseMode, TestsetId)
        Return DataHash

    End Function 'Hash วงกลม (5) แบบฝึกฝน

    Public Function dtPieActivityPerQuiz(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal TestSetId As String, ByVal QuizTime As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        Dim dtComplete As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Or TestSetId Is Nothing Or TestSetId = "" Then
            Return dt
        End If
        Dim QuizId As String = ""
        If ChooseMode <> 3 Then 'ถ้าเป็นโหมดปกติจะต้องหา Quiz_Id ตามครั้งที่กดเลือกมา
            QuizId = GetQuizByTestSetIdAndRoom(SchoolId, TestSetId, StrRoom, QuizTime)
        Else 'ถ้าเป็นโหมดฝึกฝนต้องหา Quiz_id ครั้งล่าสุดที่ Testset_Id นั้น
            Dim SplitStr = StrRoom.Split("/")
            Dim ClassName As String = SplitStr(0)
            Dim RoomName As String = "/" & SplitStr(1)
            Dim CalendarId As String = KNSession("").ToString()
            If CalendarId = "" Then
                Return dt
            End If
        End If

        Dim NewArr As New ArrayList
        NewArr.Add(StrRoom)
        dtComplete = QueryPieDt(SchoolId, NewArr, "StudentTestSetSubjectTotalScore", "NoInRoom", QuizId, SortType, False, True, ChooseMode)

        Return dtComplete

    End Function 'dt วงกลมควิซ (6)



    Public Function HashPieActivityPerQuiz(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal TestsetId As String, ByVal QuizTime As String, ByVal ChooseMode As Integer)

        Dim DataHash As New Hashtable
        Dim QuizId As String = ""
        If ChooseMode <> 3 Then
            QuizId = GetQuizByTestSetIdAndRoom(SchoolId, TestsetId, StrRoom, QuizTime)
        Else
            Dim SplitStr = StrRoom.Split("/")
            Dim ClassName As String = SplitStr(0)
            Dim Roomname As String = "/" & SplitStr(1)
            Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
            If CalendarId = "" Then
                Return DataHash
            End If
            QuizId = GetLastQuizIdPracticeMode(TestsetId, ClassName, Roomname, SchoolId, CalendarId)
            If QuizId = "" Then
                Return DataHash
            End If
        End If
        Dim ArrNew As New ArrayList
        ArrNew.Add(StrRoom)
        Dim CheckSort As Boolean
        If SortType = "1" Or SortType = "2" Then
            CheckSort = True
        Else
            CheckSort = False
        End If
        DataHash = QueryPieHashFromView(SchoolId, "StudentTestSetSubjectTotalScore", ArrNew, QuizId, CheckSort, True, ChooseMode)
        Return DataHash

    End Function 'Hash วงกลมควิซ (6)


    Public Function QueryPieHashFromView(ByVal SchoolId As String, ByVal ViewName As String, ByVal ArrClass As ArrayList, ByVal WhereFieldName As String, ByVal IsSort As Boolean, ByVal IsQuiz As Boolean, ByVal ChooseMode As Integer, Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") As Hashtable

        Dim DataHash As New Hashtable
        If SchoolId Is Nothing Or SchoolId = "" Or ViewName Is Nothing Or ViewName = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Then
            Return DataHash
        End If

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return DataHash
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        If IsSort = True Then 'ถ้าเกิดมีการเลือกโหมด Top10 Low10 จะใช้ dt จาก Session เพื่อมาวนทำ Hashtable
            If HttpContext.Current.Session("dtCompleteTopLow") Is Nothing Then
                Return DataHash
            End If
            Dim dtTopLow As DataTable = HttpContext.Current.Session("dtCompleteTopLow")
            Dim ArrSubject As New ArrayList
            Dim ArrClassOrRoomOrNoInRoom As New ArrayList
            For z = 0 To dtTopLow.Rows.Count - 1
                If ArrSubject.Contains(dtTopLow.Rows(z)(1).ToString()) = False Then 'วนเพื่อเก็บวิชาทั้งหมดเข้า Array
                    ArrSubject.Add(dtTopLow.Rows(z)(1).ToString())
                End If
            Next

            For Each allobj In ArrSubject
                Dim TotalScore As Double = 0
                Dim dtRow() As DataRow = dtTopLow.Select("SubjectName = '" & allobj & "' ")
                If dtRow.Count > 0 Then
                    For Each allScore In dtRow
                        TotalScore = TotalScore + allScore(2)
                    Next
                End If
                DataHash.Add(allobj, TotalScore)
            Next
            HttpContext.Current.Session("dtCompleteTopLow") = Nothing

        Else 'ถ้าไม่ได้เลือกโหมด Top10 Low10 จะคิวรี่ข้อมูลเพื่อทำ Hashtable
            Dim sql As New StringBuilder
            Dim SpareStr As String = ""
            sql.Append(" SELECT SubjectName,CASE WHEN SUM(TotalScore) IS NULL THEN '0' ELSE SUM(TotalScore) END As SumScore FROM dbo.uvw_Chart_" & ViewName.Trim() & " ")
            If IsQuiz = False Then
                sql.Append(" WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ( ")
                If ArrClass.Count > 1 Then
                    For index = 0 To ArrClass.Count - 1
                        Dim CurrentRoomName As String = _DB.CleanString(ArrClass(index).ToString().Trim())
                        sql.Append(" " & WhereFieldName & " = '" & CurrentRoomName & "' Or ")
                    Next
                    SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
                    sql.Clear()
                    sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
                    If TeacherId <> "" Then
                        sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
                    End If
                    If TestSetId = "" Then 'ถ้าเป็นแบบฝึกฝนจะส่ง Testset_Id เข้ามาด้วย ต้อง where เพิ่มเข้าไป
                        sql.Append(" GROUP BY SubjectName ")
                    Else
                        sql.Append(" AND TestSet_Id = '" & TestSetId & "' GROUP BY SubjectName ")
                    End If
                Else
                    sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
                    If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where Testset_id ด้วย
                        sql.Append(" AND TestSet_Id = '" & TestSetId & "' ")
                    End If
                    If TeacherId <> "" Then
                        sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
                    End If
                    sql.Append(" GROUP BY SubjectName ")
                End If
            Else
                sql.Append(" WHERE Quiz_Id = '" & WhereFieldName & "' AND RoomName = '" & ArrClass(0) & "' AND " & StrWhere & " = '1' ")
                If TeacherId <> "" Then
                    sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
                End If
                'If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where TestsetId ด้วย
                '    sql.Append(" AND TestSet_Id = '" & TestSetId & "' ")
                'End If
                sql.Append(" GROUP BY SubjectName ")
            End If
            Dim dt As New DataTable
            dt = _DB.getdata(sql.ToString())
            If dt.Rows.Count > 0 Then
                For z = 0 To dt.Rows.Count - 1
                    DataHash.Add(dt.Rows(z)("SubjectName"), dt.Rows(z)("SumScore"))
                Next
            End If
        End If

        Return DataHash

    End Function 'ทำ Hashtable ของวงกลม 1-4

    Public Function QueryPieDt(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal ViewName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal SortType As String, ByVal WhereFieldIsMorethanOne As Boolean, ByVal IsQuiz As Boolean, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or SelectFieldName Is Nothing Or SelectFieldName = "" Or ViewName Is Nothing Or ViewName = "" Or SortType Is Nothing Or SortType = "" Then
            Return dt
        End If
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As New StringBuilder
        Dim SpareStr As String = ""
        sql.Append("SELECT ")
        If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าไป
            sql.Append(" Top 10 ")
        End If
        sql.Append(SelectFieldName)
        If SortType = "0" Then 'ถ้าเป็นโหมดปกติ ถึงจะ Select 3 Field
            sql.Append(" ,CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore ")
        End If
        sql.Append(" FROM dbo.uvw_Chart_" & ViewName & " WHERE ")
        If IsQuiz = False Then
            sql.Append("SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ( ")
            If ArrClass.Count > 1 Then
                For i = 0 To ArrClass.Count - 1
                    Dim CurrentClass As String = _DB.CleanString(ArrClass(i).ToString().Trim())
                    sql.Append(" " & WhereFieldName & " = '" & CurrentClass & "' Or ")
                Next
                SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
                sql.Clear()
                sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "'  AND " & StrWhere & " = '1' ")
            Else
                sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
            End If
        Else
            sql.Append(" " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' AND Quiz_Id = '" & WhereFieldName & "' AND RoomName = '" & ArrClass(0) & "' ")
        End If
        If TeacherId <> "" Then
            sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
        End If
        If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าเงื่อนไขนี้
            dt.Columns.Add(SelectFieldName)
            'dt.Columns.Add("SubjectName")
            dt.Columns.Add("TotalScore")
            'sql.Append(" AND AcademicYear = '" & AcademicYear & "' AND IsPracticeMode = '" & PracticeMode & "' GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
            sql.Append(" GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
            If SortType = "1" Then
                sql.Append(" desc ")
            Else
                sql.Append(" asc ")
            End If
            Dim dt2 As New DataTable
            dt2 = _DB.getdata(sql.ToString())
            If dt2.Rows.Count > 0 Then
                For index = 0 To dt2.Rows.Count - 1
                    Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
                    Dim sqlStr As String = " SELECT " & SelectFieldName & ",CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim) & "' " & _
                           " AND " & SelectFieldName & " = '" & ClassOrRoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
                    If WhereFieldIsMorethanOne = True Then
                        sqlStr = sqlStr & " AND " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' "
                    End If
                    If IsQuiz = True Then
                        sqlStr = sqlStr & " AND  Quiz_Id = '" & WhereFieldName & "' "
                    End If
                    If TeacherId <> "" Then
                        sqlStr &= " AND Assistant_id = '" & TeacherId & "' "
                    End If
                    Dim dt3 As New DataTable
                    dt3 = _DB.getdata(sqlStr)
                    If dt3.Rows.Count > 0 Then
                        For Each allrow As DataRow In dt3.Rows
                            dt.Rows.Add(allrow(0), allrow(1))
                        Next
                    End If
                Next
            Else
                Return dt
            End If
            HttpContext.Current.Session("dtCompleteTopLow") = dt
        End If

        If SortType = "0" Then
            dt = _DB.getdata(sql.ToString())
        End If

        Return dt

    End Function 'ทำ dt ของวงกลม 1-4

    Public Function QueryPieDtAVGStudentScoreByTestsetId(ByVal SchoolId As String, ByVal TestsetId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
            Return dt
        End If
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As String = ""
        sql = " SELECT NoInRoom,TotalScore,SubjectName FROM dbo.uvw_Chart_AvgStudentScoreBySubject WHERE " & _
            " TestSet_Id = '" & TestsetId & "' AND Calendar_Id = '" & CalendarId & " ' AND " & StrWhere & " = '1' AND RoomName = '" & _DB.CleanString(StrRoom) & "' "
        If SortType = "0" Then
            sql &= "ORDER BY NoInRoom "
        ElseIf SortType = "1" Then
            sql &= "ORDER BY TotalScore DESC"
        ElseIf SortType = "2" Then
            sql &= "ORDER BY TotalScore "
        End If

        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then
            For index = 0 To dt.Rows.Count - 1
                dt.Rows(index)("SubjectName") = GenSubjectName(dt.Rows(index)("SubjectName"))
            Next
        End If

        Return dt

    End Function

    Public Function QueryPieHashActivity(ByVal SchoolId As String, ByVal ViewName As String, ByVal ArrClass As ArrayList, ByVal WhereFieldName As String, ByVal IsSort As Boolean, ByVal TestSetId As String, ByVal ChooseMode As Integer)

        Dim DataHash As New Hashtable
        Dim dtComplete As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ViewName Is Nothing Or ViewName = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Then
            Return DataHash
        End If
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return DataHash
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        If IsSort = True Then 'ถ้าเกิดมีการเลือกโหมด Top10 Low10 จะใช้ dt จาก Session เพื่อมาวนทำ Hashtable
            If HttpContext.Current.Session("dtCompleteTopLowActivity") Is Nothing Then
                Return DataHash
            End If
            Dim dtTopLow As DataTable = HttpContext.Current.Session("dtCompleteTopLowActivity")
            Dim ArrRoomQuizTime As New ArrayList
            Dim ArrClassOrRoomOrNoInRoom As New ArrayList
            For z = 0 To dtTopLow.Rows.Count - 1
                If ArrRoomQuizTime.Contains(dtTopLow.Rows(z)(0).ToString()) = False Then 'วนเพื่อเก็บห้องและครั้งที่ทำควิซเข้า Array
                    ArrRoomQuizTime.Add(dtTopLow.Rows(z)(0).ToString())
                End If
            Next

            For Each allobj In ArrRoomQuizTime
                Dim TotalScore As Integer = 0
                Dim dtRow() As DataRow = dtTopLow.Select("RoomName = '" & allobj & "' ")
                If dtRow.Count > 0 Then 'ได้ทุก Row ของข้อมูล เช่น "ม.4/4ครั้งที่30"
                    For Each allScore In dtRow
                        TotalScore = TotalScore + CInt(allScore(2))
                    Next
                End If
                DataHash.Add(allobj, TotalScore)
            Next
            HttpContext.Current.Session("dtCompleteTopLow") = Nothing

        Else 'ถ้าไม่ได้เลือกโหมด Top10 Low10 จะคิวรี่ข้อมูลเพื่อทำ Hashtable
            Dim sql As New StringBuilder
            Dim SpareStr As String = ""
            sql.Append(" SELECT RoomName,Quiz_Id,SUM(TotalScore) FROM dbo.uvw_Chart_" & ViewName.Trim() & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND " & StrWhere & " = '1' AND Testset_Id = '" & _DB.CleanString(TestSetId) & "' AND ( ")
            If ArrClass.Count > 1 Then
                For index = 0 To ArrClass.Count - 1
                    Dim CurrentRoomName As String = _DB.CleanString(ArrClass(index).ToString().Trim())
                    sql.Append(" " & WhereFieldName & " = '" & CurrentRoomName & "' Or ")
                Next
                SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
                sql.Clear()
                sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "' GROUP BY RoomName,Quiz_Id,LastUpdate ORDER BY LastUpdate ")
            Else
                sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' GROUP BY RoomName,Quiz_Id,LastUpdate ORDER BY LastUpdate ")
            End If
            Dim dt As New DataTable
            dt = _DB.getdata(sql.ToString())
            dtComplete = CreateStrQuizTimeForDtActivity(dt, True, False, TestSetId, SchoolId)
            If dtComplete.Rows.Count > 0 Then
                For z = 0 To dtComplete.Rows.Count - 1
                    DataHash.Add(dtComplete.Rows(z)("RoomName"), dtComplete.Rows(z)("TotalScore"))
                Next
            End If
        End If

        Return DataHash

    End Function 'ทำ Hashtable วงกลม 5

    Public Function QueryPieDtActivity(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal ViewName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal SortType As String, ByVal TestSetID As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        Dim dtComplete As New DataTable
        If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or SelectFieldName Is Nothing Or SelectFieldName = "" Or ViewName Is Nothing Or ViewName = "" Or SortType Is Nothing Or SortType = "" Or ChooseMode = 0 Then
            Return dt
        End If
        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If
        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeWorkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If
        Dim sql As New StringBuilder
        Dim SpareStr As String = ""
        sql.Append("SELECT ")
        If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าไป
            sql.Append(" Top 10 ")
        End If
        If ChooseMode = 1 Then
            sql.Append(SelectFieldName & " ,Quiz_Id ")
        Else 'ถ้าเป็นแบบฝึกฝนไม่ต้อง Select Quiz_Id
            sql.Append(SelectFieldName)
        End If
        If SortType = "0" Then 'ถ้าเป็นโหมดปกติ ถึงจะ Select เพิ่ม 2 Field
            If ChooseMode = 1 Then
                sql.Append(" ,SubjectName,TotalScore ")
            Else
                sql.Append(" ,SubjectName,SUM(TotalScore)as TotalScore ")
            End If
        End If
        sql.Append(" FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND Testset_Id = '" & _DB.CleanString(TestSetID) & "' AND " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' AND ( ")
        If ArrClass.Count > 1 Then
            For i = 0 To ArrClass.Count - 1
                Dim CurrentClass As String = _DB.CleanString(ArrClass(i).ToString().Trim())
                sql.Append(" " & WhereFieldName & " = '" & CurrentClass & "' Or ")
            Next
            SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
            sql.Clear()
            sql.Append(SpareStr & " )")
        Else
            sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) ")
        End If
        If SortType = "0" Then
            If ChooseMode = 1 Then
                sql.Append(" ORDER BY LastUpdate ")
            Else 'ถ้าเป็นโหมดฝึกฝนต้อง Group By เพราะว่า Select Sum คะแนนมา
                sql.Append(" GROUP BY " & SelectFieldName & ",SubjectName ")
            End If
            dt = _DB.getdata(sql.ToString())
            If ChooseMode = 1 Then 'ถ้าไม่ใช่โหมดฝึกฝนต้องเอา Datatable ไปหาครั้งที่ควิซต่อ
                dtComplete = CreateStrQuizTimeForDtActivity(dt, False, False, TestSetID, SchoolId)
            Else 'ถ้าเป็นโหมดฝึกฝน Return Datatable กลับไปเลย
                dtComplete = dt
            End If
        End If
        If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าเงื่อนไขนี้
            dt.Columns.Add(SelectFieldName)
            If ChooseMode = 1 Then 'ถ้าไม่ได้เป็นโหมดฝึกฝนถึงจะมี Column Quiz_Id
                dt.Columns.Add("Quiz_ID")
            End If
            dt.Columns.Add("SubjectName")
            dt.Columns.Add("TotalScore")
            If ChooseMode = 1 Then 'ถ้าไม่ได้เป็นโหมดฝึกฝน Group By QuizId 
                sql.Append(" GROUP BY " & SelectFieldName & " ,Quiz_Id ORDER BY SUM(TotalScore) ")
            Else 'ถ้าเป็นโหมดฝึกฝน Group By SubjectName
                sql.Append(" GROUP BY " & SelectFieldName & " ,SubjectName ORDER BY SUM(TotalScore) ")
            End If

            If SortType = "1" Then
                sql.Append(" desc ")
            Else
                sql.Append(" asc ")
            End If
            Dim dt2 As New DataTable
            dt2 = _DB.getdata(sql.ToString())
            If dt2.Rows.Count > 0 Then
                For index = 0 To dt2.Rows.Count - 1
                    If ChooseMode = 1 Then 'ถ้าไมได้เป็นโหมดฝึกฝนเข้าเงื่อนไขนี้
                        Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
                        Dim QuizId As String = dt2.Rows(index)("Quiz_Id").ToString()
                        Dim sqlStr As String = " SELECT " & SelectFieldName & " ,Quiz_Id,SubjectName,TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore " & _
                                         " WHERE Quiz_Id = '" & QuizId & "' and RoomName = '" & ClassOrRoomName & "' "
                        'If WhereFieldIsMorethanOne = True Then
                        '    sqlStr = sqlStr & " AND " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' "
                        'End If
                        Dim dt3 As New DataTable
                        dt3 = _DB.getdata(sqlStr)
                        If dt3.Rows.Count > 0 Then
                            For Each allrow In dt3.Rows
                                dt.Rows.Add(allrow(0), allrow(1), allrow(2), allrow(3))
                            Next
                        End If
                    Else 'ถ้าเป็นโหมดฝึกฝนเข้า Else
                        Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
                        Dim sqlStr As String = " SELECT " & SelectFieldName & " ,SubjectName,SUM(TotalScore) as TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore " & _
                                         " WHERE RoomName = '" & ClassOrRoomName & "' GROUP BY " & SelectFieldName & ",SubjectName "
                        Dim dt3 As New DataTable
                        dt3 = _DB.getdata(sqlStr)
                        If dt3.Rows.Count > 0 Then
                            For Each allrow In dt3.Rows
                                dt.Rows.Add(allrow(0), allrow(1), allrow(2))
                            Next
                        End If
                    End If

                Next
                If ChooseMode = 1 Then 'ถ้าเป็นโหมดปกติต้องเข้าไปทำ dt ใหม่
                    dtComplete = CreateStrQuizTimeForDtActivity(dt, False, True, TestSetID, SchoolId)
                Else
                    dtComplete = dt
                End If
            Else
                Return dtComplete
            End If
            If ChooseMode = 1 Then 'แยกเก็บ Session คนละตัว แยกคนละโหมด แบบฝึกฝน กับไม่มีฝึกฝน
                HttpContext.Current.Session("dtCompleteTopLowActivity") = dtComplete
            Else
                HttpContext.Current.Session("dtCompleteTopLow") = dtComplete
            End If

        End If

        Return dtComplete

    End Function 'ทำ dt วงกลม 5

    Function CreateStrQuizTimeForDtActivity(ByVal dt As DataTable, ByVal IsHash As Boolean, ByVal IsTopOrLow As Boolean, ByVal TestSetId As String, ByVal SchoolId As String) As DataTable

        Dim dtComplete As New DataTable
        If dt Is Nothing Or dt.Rows.Count = 0 Then
            Return dtComplete
        End If
        Dim ArrRoom As New ArrayList
        Dim ArrQuiz As New ArrayList

        If IsHash = False Then 'ถ้า dt ที่ส่งเข้ามา มาจาก Function ทำ dt เข้่าเงื่อนไขนี้
            dtComplete.Columns.Add("RoomName")
            dtComplete.Columns.Add("SubjectName")
            dtComplete.Columns.Add("TotalScore")
            Dim QuizStr As String = ""
            Dim QuizTime As Integer = 0
            If IsTopOrLow = False Then
                For index = 0 To dt.Rows.Count - 1
                    If ArrRoom.Contains(dt.Rows(index)("RoomName")) = False Then
                        ArrRoom.Add(dt.Rows(index)("RoomName"))
                    End If
                    If ArrQuiz.Contains(dt.Rows(index)("Quiz_Id")) = False Then
                        ArrQuiz.Add(dt.Rows(index)("Quiz_Id"))
                    End If
                Next

                For Each allRoom In ArrRoom
                    QuizTime = 0
                    For Each allquiz In ArrQuiz
                        Dim dtRow() As DataRow = dt.Select("RoomName = '" & allRoom & "' and Quiz_Id = '" & allquiz.ToString() & "' ")
                        If dtRow.Count > 0 Then
                            QuizTime = QuizTime + 1
                            For Each allrow In dtRow
                                QuizStr = allRoom & "ครั้งที่" & QuizTime.ToString()
                                dtComplete.Rows.Add(QuizStr, allrow(2), allrow(3))
                            Next
                        End If
                    Next
                Next
            Else 'ถ้าเป็นโหมด Top10 Low10 ต้องมาว่าควิซแต่ละครั้งที่ครั้งที่เท่าไหร่
                Dim QuizId As String
                Dim RoomName As String
                For z = 0 To dt.Rows.Count - 1
                    RoomName = dt.Rows(z)("RoomName").ToString()
                    QuizId = dt.Rows(z)("Quiz_Id").ToString()
                    QuizTime = GetQuiztTime(SchoolId, RoomName, QuizId, TestSetId).ToString()
                    dtComplete.Rows.Add(RoomName & "ครั้งที่" & QuizTime, dt.Rows(z)("SubjectName"), dt.Rows(z)("TotalScore"))
                Next
            End If
        Else 'ถ้า dt ที่เข้ามา มาจาก Function ทำ Hashtable เข้าเงื่อนไขนี้
            dtComplete.Columns.Add("RoomName")
            dtComplete.Columns.Add("TotalScore")
            Dim CurrentRoom As String = ""
            Dim TotalScore As Double = 0
            Dim QuizId As String = ""
            Dim QuizStr As String = ""
            Dim QuizTime As Integer = 0
            For index = 0 To dt.Rows.Count - 1
                If ArrRoom.Contains(dt.Rows(index)("RoomName")) = False Then
                    ArrRoom.Add(dt.Rows(index)("RoomName"))
                End If
                If ArrQuiz.Contains(dt.Rows(index)("Quiz_Id")) = False Then
                    ArrQuiz.Add(dt.Rows(index)("Quiz_Id"))
                End If
            Next
            For Each allRoom In ArrRoom
                QuizTime = 0
                For Each allquiz In ArrQuiz
                    Dim dtRow() As DataRow = dt.Select("RoomName = '" & allRoom & "' and Quiz_Id = '" & allquiz.ToString() & "' ")
                    If dtRow.Count > 0 Then
                        QuizTime = QuizTime + 1
                        TotalScore = dtRow(0)(2)
                        QuizStr = allRoom & "ครั้งที่" & QuizTime.ToString()
                        dtComplete.Rows.Add(QuizStr, TotalScore)
                    End If
                Next
            Next
        End If

        Return dtComplete

    End Function 'แปลง Column dt สำหรับวงกลม Quiz column แรก เช่น = "ม.4/4ครั้งที่1"

    'Function หา Datatable ที่เป็นคะแนนเฉลี่ยของนักเรียนแต่ละคน เอาไปใช้กับ กราฟแท่ง ที่เป็นคะแนนเฉลี่ยของควิซทุกครั้ง ของ ห้อง ... ชุด ...
    Public Function GetDtAverageScorePerStudent(ByVal TestSetId As String, ByVal RoomName As String, ByVal SchoolId As String, ByVal SortType As String, ByVal ChooseMode As Integer)

        Dim dt As New DataTable
        If TestSetId = "" Or TestSetId Is Nothing Or RoomName = "" Or RoomName Is Nothing Or SchoolId = "" Or SchoolId Is Nothing Then
            Return dt
        End If

        Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
        If CalendarId = "" Then
            Return dt
        End If

        Dim StrWhere As String = ""
        If ChooseMode = 1 Then
            StrWhere = "IsQuizMode"
        ElseIf ChooseMode = 2 Then
            StrWhere = "IsHomeworkMode"
        ElseIf ChooseMode = 3 Then
            StrWhere = "IsPracticeMode"
        End If

        Dim sql As String = ""
        'ถ้าเป็นการบ้านใช้ Query นี้
        'If ChooseMode = 2 Then
        '    sql = " SELECT NoInRoom,CAST( ( (SUM(TotalScore) * 100 )) / SUM(FullScore) AS NUMERIC(10,2) ) AS TotalScore " & _
        '          " FROM dbo.uvw_Chart_AvgStudentScoreHomework WHERE SchoolCode = '" & SchoolId & "' AND RoomName = '" & RoomName & "' " & _
        '          " AND StudentCalendar = '" & CalendarId & "' AND Calendar_Id = '" & CalendarId & "' " & _
        '          " AND TestSet_Id = '" & TestSetId & "' GROUP BY NoInRoom "
        'Else 'ถ้าเป็นฝึกฝนกับควิซใช้ Query นี้
        sql = " SELECT NoInRoom,CASE WHEN CAST(SUM(ResultScore) * 100 / SUM(FullScore) AS DECIMAL(10,2)) IS NULL THEN 0 ELSE " & _
              " CAST(SUM(ResultScore) * 100 / SUM(FullScore) AS DECIMAL(10,2)) END AS TotalScore " & _
              " FROM dbo.uvw_Chart_AvgStudentScoreForReduceNew WHERE SchoolCode = '" & SchoolId & "' AND RoomName = '" & RoomName & "' AND " & _
              " TestSet_Id = '" & TestSetId & "' AND " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' " & _
              " GROUP BY TestSet_Id,NoInRoom "
        'End If

        If SortType <> "0" Then
            If SortType = "1" Then
                sql &= " ORDER BY CAST(SUM(ResultScore) * 100 / SUM(FullScore) AS DECIMAL(10,2)) desc "
            Else
                sql &= " ORDER BY CAST(SUM(ResultScore) * 100 / SUM(FullScore) AS DECIMAL(10,2)) asc "
            End If
        Else
            sql &= " ORDER BY NoInRoom "
        End If

        dt = _DB.getdata(sql)
        Return dt

    End Function


    Public Function GetLastQuizIdPracticeMode(ByVal TestSetId As String, ByVal ClassName As String, ByVal RoomName As String, ByVal SchoolId As String, ByVal CalendarId As String)

        Dim LastTesSetId As String = ""
        If TestSetId Is Nothing Or TestSetId = "" Or ClassName Is Nothing Or ClassName = "" Or RoomName Is Nothing Or RoomName = "" Or _
            CalendarId Is Nothing Or CalendarId = "" Or SchoolId Is Nothing Or SchoolId = "" Then
            Return LastTesSetId
        End If
        Dim sql As String = " SELECT TOP 1 Quiz_Id FROM dbo.tblQuiz WHERE TestSet_Id = '" & TestSetId & "' AND IsPracticeMode = '1' " & _
                            " AND t360_ClassName = '" & ClassName & "' AND t360_RoomName = '" & RoomName & "' " & _
                            " AND Calendar_Id = '" & CalendarId & "' AND t360_SchoolCode = '" & SchoolId & "' " & _
                            " ORDER BY LastUpdate DESC "
        LastTesSetId = _DB.ExecuteScalar(sql)
        Return LastTesSetId

    End Function 'Function หา Quiz ครั้งล่าสุดสำหรับโหมดฝึกฝน


    Public Function GetCalendarId(ByVal SchoolId As String, Optional ByRef InputConn As sqlconnection = Nothing)

        Dim sql As String = " SELECT TOP 1 Calendar_Id FROM t360_tblCalendar WHERE dbo.GetThaiDate() BETWEEN Calendar_FromDate AND Calendar_ToDate " &
                            " AND Calendar_Type = 3 AND School_Code = '" & SchoolId & "' AND IsActive = 1; "
        Dim db As New ClassConnectSql()
        Dim CalendarId As String = db.ExecuteScalar(sql, InputConn)
        Return CalendarId

    End Function

#Region "ของเก่า"
    'Public Function HashChartMainPersonal(ByVal SchoolId As String, ByVal ChooseMode As Integer)

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Then
    '        Return DataHash2
    '    End If

    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If

    '    Dim sql As String = " SELECT RoomName,NoInRoom FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode  = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                        " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY RoomName,NoInRoom ORDER BY SUM(TotalScore) desc "
    '    Dim dt As New DataTable
    '    Dim dt2 As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("RoomName")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    Dim ArrCategories As New ArrayList
    '    Dim ArrCheck As New ArrayList
    '    dt = _DB.getdata(sql)
    '    If dt.Rows.Count > 0 Then
    '        For index = 0 To dt.Rows.Count - 1
    '            Dim FullRoomName As String = dt.Rows(index)("RoomName").ToString()
    '            Dim NoInRoom As String = dt.Rows(index)("NoInRoom").ToString()
    '            Dim ArrSplitRoomName = FullRoomName.Split("/")
    '            Dim ClassName As String = ArrSplitRoomName(0)
    '            If index = 0 Then
    '                ArrCheck.Add(ClassName)
    '                ArrCategories.Add(FullRoomName & ":" & "เลขที่" & NoInRoom)
    '                sql = " SELECT RoomName,SubjectName,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
    '                      " FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                      " AND RoomName = '" & FullRoomName & "' AND NoInRoom = '" & NoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                dt2 = _DB.getdata(sql)
    '                dtComplete = dt2
    '            Else
    '                If ArrCheck.Contains(ClassName) = False Then
    '                    ArrCheck.Add(ClassName)
    '                    ArrCategories.Add(FullRoomName & ":" & "เลขที่" & NoInRoom)
    '                    sql = " SELECT RoomName,SubjectName, CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
    '                          " FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                          " AND RoomName = '" & FullRoomName & "' AND NoInRoom = '" & NoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                    dt2 = _DB.getdata(sql)
    '                    For Each objrow In dt2.Rows
    '                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                    Next
    '                End If
    '            End If
    '        Next

    '        HttpContext.Current.Session("ArrCategoriesChartMainPersonal") = ArrCategories
    '        Dim ArrClass As New ArrayList
    '        For i = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(i)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(i)("SubjectName"), "")
    '            End If

    '            If ArrClass.Contains(dtComplete.Rows(i)("RoomName")) = False Then
    '                ArrClass.Add(dtComplete.Rows(i)("RoomName"))
    '            End If
    '        Next

    '        For Each row In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(row)
    '            For j = 0 To ArrClass.Count - 1
    '                Dim CurrentRoom As String = ArrClass(j)
    '                Dim dtrow() As DataRow = dtComplete.Select("RoomName = '" & CurrentRoom & "' and SubjectName = '" & row & "' ")
    '                If dtrow.Length > 0 Then
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next

    '    Else
    '        Return DataHash2
    '    End If

    '    Return DataHash2


    'End Function

    'Public Function HashChartRoom(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return DataHash2
    '    End If
    '    Dim dt As New DataTable
    '    Dim dt2 As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("NoInRoom")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As String
    '    If SortType = "0" Then 'ถ้าไม่มีการเลือก Top10 Low10 จะเข้าเงื่อนไขนี้
    '        sql = " SELECT NoInRoom,SubjectName,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '              " AND RoomName = '" & _DB.CleanString(StrRoom.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY NoInRoom "
    '        dtComplete = _DB.getdata(sql)
    '    ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเลือก Top10 หรือ Low10 จะเข้าเงื่อนไขนี้
    '        sql = " SELECT Top 10 NoInRoom FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '              " AND RoomName = '" & _DB.CleanString(StrRoom) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY NoInRoom ORDER BY SUM(TotalScore)  "
    '        If SortType = "1" Then
    '            sql = sql & " desc "
    '        Else
    '            sql = sql & " asc "
    '        End If
    '        dt = _DB.getdata(sql)
    '        If dt.Rows.Count > 0 Then
    '            Dim EachNoInRoom As String
    '            For z = 0 To dt.Rows.Count - 1
    '                EachNoInRoom = dt.Rows(z)("NoInRoom").ToString()
    '                sql = " SELECT NoInRoom,SubjectName,CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore FROM dbo.uvw_Chart_RoomNoSubjectTotalscore " & _
    '                      " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND RoomName = '" & _DB.CleanString(StrRoom) & "' " & _
    '                      " AND NoInRoom = '" & EachNoInRoom & "' AND Calendar_Id  = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                dt2 = _DB.getdata(sql)
    '                If dt2.Rows.Count > 0 Then
    '                    For Each objrow In dt2.Rows
    '                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                    Next
    '                Else
    '                    dtComplete.Rows.Add(EachNoInRoom, "กลุ่มสาระการเรียนรู้ภาษาไทย", "0")
    '                End If
    '            Next
    '        Else
    '            Return DataHash2
    '        End If
    '    End If

    '    If dtComplete.Rows.Count > 0 Then
    '        Dim ArrNoInRoom As New ArrayList
    '        Dim ArrCategories As New ArrayList
    '        For index = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(index)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(index)("SubjectName"), "")
    '            End If
    '            If ArrNoInRoom.Contains(dtComplete.Rows(index)("NoInRoom")) = False Then
    '                ArrNoInRoom.Add(dtComplete.Rows(index)("NoInRoom"))
    '                ArrCategories.Add(dtComplete.Rows(index)("NoInRoom"))
    '            End If
    '        Next
    '        HttpContext.Current.Session("ArrCatChartRoom") = ArrCategories
    '        For Each allrow In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(allrow)
    '            For j = 0 To ArrNoInRoom.Count - 1
    '                Dim EachCurrentNoInRoom As String = ArrNoInRoom(j)
    '                Dim dtrow() As DataRow = dtComplete.Select("NoInRoom = '" & EachCurrentNoInRoom & "' and SubjectName = '" & allrow & "' ")
    '                If dtrow.Length > 0 Then
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next
    '    Else
    '        Return DataHash2
    '    End If

    '    Return DataHash2

    'End Function

    'Public Function HashChartTopLowPerSonalMode(ByVal SchoolId As String, ByVal StrRoomName As String, ByVal StrOrder As String, ByVal ChooseMode As Integer)

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or StrRoomName Is Nothing Or StrRoomName = "" Or StrOrder Is Nothing Or StrOrder = "" Or ChooseMode = 0 Then
    '        Return DataHash2
    '    End If

    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If

    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If

    '    Dim sql As String = " SELECT top 10 NoInRoom FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                        " AND RoomName = '" & _DB.CleanString(StrRoomName.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY NoInRoom ORDER BY SUM(TotalScore) " & StrOrder & " "
    '    Dim dt As New DataTable
    '    Dim dt2 As New DataTable
    '    Dim dtComplete As New DataTable
    '    dt = _DB.getdata(sql)
    '    If dt.Rows.Count > 0 Then
    '        HttpContext.Current.Session("dtChartOnlyRommInPersonalMode") = dt
    '        Dim RoomName As String = _DB.CleanString(StrRoomName.Trim())
    '        For index = 0 To dt.Rows.Count - 1
    '            Dim CurrentNoInRoom As String = dt.Rows(index)(0).ToString()
    '            If index = 0 Then
    '                sql = " SELECT RoomName,NoInRoom,SubjectName,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
    '                      " FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                      " AND RoomName = '" & RoomName & "' AND NoInRoom = '" & CurrentNoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                dt2 = _DB.getdata(sql)
    '                dtComplete = dt2
    '            Else
    '                sql = " SELECT RoomName,NoInRoom,SubjectName,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END AS TotalScore " & _
    '                      " FROM dbo.uvw_Chart_RoomNoSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                     " AND RoomName = '" & RoomName & "' AND NoInRoom = '" & CurrentNoInRoom & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                dt2 = _DB.getdata(sql)
    '                For Each obj In dt2.Rows
    '                    dtComplete.Rows.Add(obj(0), obj(1), obj(2), obj(3))
    '                Next
    '            End If
    '        Next

    '        Dim ArrNumStudent As New ArrayList
    '        For x = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(x)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(x)("SubjectName"), "")
    '            End If
    '            If ArrNumStudent.Contains(dtComplete.Rows(x)("NoInRoom")) = False Then
    '                ArrNumStudent.Add(dtComplete.Rows(x)("NoInRoom"))
    '            End If
    '        Next

    '        For Each allrow In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(allrow)
    '            For c = 0 To ArrNumStudent.Count - 1
    '                Dim CurrentNo As String = ArrNumStudent(c)
    '                Dim dtrow() As DataRow = dtComplete.Select("NoInRoom = '" & CurrentNo & "' and SubjectName = '" & allrow & "'")
    '                If dtrow.Length > 0 Then
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next

    '    Else
    '        Return DataHash2
    '    End If

    '    Return DataHash2

    'End Function 'Top 10 - Low 10 ****10****

    'Public Function HashChartStudentOnly(ByVal SchoolId As String, ByVal StrClass As String, ByVal StrRoom As String, ByVal NoInRoom As String, ByVal ChooseMode As Integer)

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("RoomName")
    '    dtComplete.Columns.Add("NoInRoom")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")

    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If

    '    If SchoolId Is Nothing Or SchoolId = "" Or StrClass Is Nothing Or StrClass = "" Or StrRoom Is Nothing Or StrRoom = "" Or NoInRoom Is Nothing Or NoInRoom = "" Or ChooseMode = 0 Then
    '        Return DataHash
    '    End If

    '    Dim sql As String = " SELECT Student_Id FROM dbo.t360_tblStudent WHERE School_Code = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                        " AND Student_CurrentClass = '" & _DB.CleanString(StrClass.Trim()) & "' AND Student_CurrentRoom = '" & _DB.CleanString(StrRoom.Trim()) & "' " & _
    '                        " AND Student_CurrentNoInRoom = '" & NoInRoom & "' AND Student_IsActive = '1' " 'หา Student_ID จากห้องกับชั้นในปัจจุบัน
    '    Dim StudentId As String = _DB.ExecuteScalar(sql)
    '    If StudentId <> "" Then
    '        sql = " SELECT RoomName,NoInRoom,SubjectName,CASE WHEN TotalScore IS NOT NULL THEN TotalScore ELSE 0 END " & _
    '            " ,Calendar_Id FROM dbo.uvw_Chart_StudentAllAcademicYear " & _
    '              " WHERE Student_Id = '" & StudentId & "' AND " & StrWhere & " = '1'  ORDER BY MoveDate "
    '        dt = _DB.getdata(sql)
    '        If dt.Rows.Count > 0 Then
    '            Dim ArrAcademicYear As New ArrayList
    '            Dim ArrRoomName As New ArrayList
    '            Dim ArrNoInRoom As New ArrayList
    '            Dim ArrSubjectName As New ArrayList
    '            For a = 0 To dt.Rows.Count - 1
    '                If ArrSubjectName.Contains(dt.Rows(a)("SubjectName")) = False Then
    '                    ArrSubjectName.Add(dt.Rows(a)("SubjectName"))
    '                End If
    '                If ArrAcademicYear.Contains(dt.Rows(a)("Calendar_Id")) = False Then 'Add ปีการศึกษาทั้งหมดลงใน Array
    '                    ArrAcademicYear.Add(dt.Rows(a)("Calendar_Id"))
    '                End If
    '                If ArrRoomName.Contains(dt.Rows(a)("RoomName")) = False Then 'Add ห้องทั้งหมดลงใน Array
    '                    ArrRoomName.Add(dt.Rows(a)("RoomName"))
    '                End If
    '                If ArrNoInRoom.Contains(dt.Rows(a)("NoInRoom")) = False Then 'Add เลขที่ทั้งหมดลงใน Array
    '                    ArrNoInRoom.Add(dt.Rows(a)("NoInRoom"))
    '                End If
    '            Next

    '            Dim dtRow() As DataRow
    '            Dim ArrCategories As New ArrayList
    '            For Each EachYear In ArrAcademicYear
    '                For Each EachRoom In ArrRoomName
    '                    For Each EachNoInRoom In ArrNoInRoom
    '                        dtRow = dt.Select(" Calendar_Id = '" & EachYear.ToString() & "' and RoomName = '" & EachRoom & "' and NoInRoom = '" & EachNoInRoom & "' ")
    '                        If dtRow.Count > 0 Then
    '                            Dim StrForArrCat As String = dtRow(0)(0) & "-เลขที่" & dtRow(0)(1)
    '                            ArrCategories.Add(StrForArrCat)
    '                        End If
    '                    Next
    '                Next
    '            Next
    '            HttpContext.Current.Session("ArrCatChartStudentOnly") = ArrCategories
    '            For Each AllSubject In ArrSubjectName
    '                Dim SplitStr1 As String
    '                Dim SplitStr2
    '                Dim Scores As String = ""
    '                For z = 0 To ArrCategories.Count - 1
    '                    SplitStr1 = ArrCategories(z)
    '                    SplitStr2 = SplitStr1.Split("-")
    '                    Dim ReplaceNoInRoomSrtr As String = SplitStr2(1).ToString().Replace("เลขที่", "")
    '                    dtRow = dt.Select(" RoomName = '" & SplitStr2(0) & "' and NoInRoom = '" & ReplaceNoInRoomSrtr & "' and SubjectName = '" & AllSubject & "' ")
    '                    If dtRow.Count > 0 Then
    '                        Scores = Scores & dtRow(0)(3).ToString() & ","
    '                    Else
    '                        Scores = Scores & "0,"
    '                    End If
    '                Next
    '                If Scores.EndsWith(",") Then
    '                    Scores = Scores.Substring(0, Scores.Length - 1)
    '                End If
    '                Dim GenSubject As String = GenSubjectName(AllSubject)
    '                DataHash.Add(GenSubject, Scores)
    '            Next
    '        Else
    '            Return DataHash
    '        End If
    '    Else
    '        Return DataHash
    '    End If

    '    Return DataHash

    'End Function

    'Public Function HashChartMainLevel(ByVal SchoolId As String, ByVal ClassName As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

    '    Dim DataHash As New Hashtable
    '    Dim Datahash2 As New Hashtable
    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("ClassName")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return "-1"
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    'Dim ViewName As String = ""
    '    'If ChooseMode = 2 Then
    '    '    ViewName = "Homework"
    '    'End If
    '    If SchoolId Is Nothing Or SchoolId = "" Or ClassName Is Nothing Or ClassName.Count = 0 Or SortType Is Nothing Or SortType = "" Then
    '        Return Datahash2
    '    End If

    '    If SortType = "0" Then 'ถ้าไม่ได้เป็นโหมด Top10 Low10 เข้าเงื่อนไขนี้
    '        Dim sql As New StringBuilder
    '        sql.Append(" SELECT ClassName,SubjectName,TotalScore FROM dbo.uvw_Chart_ClassSubjectTotalscore  Where SchoolCode = '")
    '        sql.Append(_DB.CleanString(SchoolId.Trim()))
    '        sql.Append("' ")
    '        'sql.Append(" AND StudentCalendarId = '" & CalendarId & "' ")
    '        Dim IsMoreOneClass As Boolean = False
    '        If ClassName.Count > 1 Then
    '            IsMoreOneClass = True
    '            sql.Append(" And ( ")
    '            For z = 0 To ClassName.Count - 1
    '                sql.Append("ClassName = '" & ClassName(z) & "' OR ")
    '            Next
    '        Else
    '            sql.Append(" And ClassName = '" & ClassName(0) & "' ")
    '        End If

    '        Dim FullSql As String = ""
    '        If IsMoreOneClass = True Then
    '            FullSql = sql.ToString().Substring(0, sql.ToString().Length - 3) & ") And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '            If TeacherId <> "" Then
    '                FullSql &= " AND Assistant_id = '" & TeacherId.ToString() & "' "
    '            End If
    '            FullSql &= " ORDER BY ClassName "
    '        Else
    '            FullSql = sql.ToString() & " And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1'  "
    '            If TeacherId <> "" Then
    '                FullSql &= " AND Assistant_id = '" & TeacherId.ToString() & "' "
    '            End If
    '            FullSql &= " ORDER BY SubjectName,ClassName "
    '        End If

    '        dtComplete = _DB.getdata(FullSql)
    '    Else 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไขนี้
    '        dt = CreateDtTopLow(SchoolId, ClassName, SortType, "ClassSubjectTotalscore", "ClassName", ChooseMode, "ClassName")
    '        If dt.Rows.Count > 0 Then
    '            Dim EachClassName As String
    '            For t = 0 To dt.Rows.Count - 1
    '                EachClassName = dt.Rows(t)("ClassName").ToString()
    '                Dim StrSql As String = " SELECT ClassName,SubjectName,TotalScore FROM dbo.uvw_Chart_ClassSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                                       " AND ClassName = '" & _DB.CleanString(EachClassName.Trim()) & "' And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                Dim dt2 As New DataTable
    '                dt2 = _DB.getdata(StrSql)
    '                If dt2.Rows.Count > 0 Then
    '                    For Each objrow In dt2.Rows
    '                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                    Next
    '                Else
    '                    dtComplete.Rows.Add(EachClassName, "กลุ่มสาระการเรียนรู้ภาษาไทย", "0")
    '                End If
    '            Next
    '        Else
    '            Return Datahash2
    '        End If
    '    End If

    '    If dtComplete.Rows.Count > 0 Then
    '        Dim ArrClass As New ArrayList
    '        For i = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(i)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(i)("SubjectName"), "")
    '            End If

    '            If ArrClass.Contains(dtComplete.Rows(i)("ClassName")) = False Then
    '                ArrClass.Add(dtComplete.Rows(i)("ClassName"))
    '            End If
    '        Next

    '        'ArrClass.Add("ม.91")
    '        'ArrClass.Add("ม.92")
    '        'ArrClass.Add("ม.93")
    '        'ArrClass.Add("ม.94")
    '        'ArrClass.Add("ม.95")
    '        'ArrClass.Add("ม.96")
    '        'ArrClass.Add("ม.97")
    '        HttpContext.Current.Session("ArrCatChartMainLevel") = ArrClass
    '        For Each row In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(row)
    '            'Dim CalCulateScore As Double = 0
    '            For j = 0 To ArrClass.Count - 1
    '                Dim CurrentClass As String = ArrClass(j)
    '                Dim dtrow() As DataRow = dtComplete.Select("ClassName = '" & CurrentClass & "' and SubjectName = '" & row & "' ")
    '                If dtrow.Length > 0 Then
    '                    'CalCulateScore = dtrow(0)("TotalScore")
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            Datahash2.Add(CurrentSubject, Scores)
    '        Next

    '    Else
    '        Return Datahash2
    '    End If
    '    Return Datahash2

    'End Function '****1**** 

    'Public Function HashChartClass(ByVal SchoolId As String, ByVal StrClass As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") 'Function นี้ใช้รวมกันกับกราฟ Activity (5)

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable

    '    If SchoolId Is Nothing Or SchoolId = "" Or StrClass Is Nothing Or StrClass.Count = 0 Or SortType Is Nothing Or SortType = "" Then
    '        Return DataHash2
    '    End If

    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("RoomName")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If
    '    Dim StrWhere As String = ""
    '    'Dim ViewName As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '        'ViewName = "Homework"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As New StringBuilder
    '    If SortType = "0" Then 'ถ้าไม่เป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
    '        Dim IsMoreOneRoom As Boolean = False
    '        If TestSetId = "" Then 'ถ้าเป็นโหมดฝึกฝนใช้ Query อีกแบบนึง
    '            sql.Append(" SELECT RoomName,SubjectName,TotalScore FROM dbo.uvw_Chart_RoomSubjectTotalscore  WHERE SchoolCode = '" & SchoolId & "' AND ")
    '        Else
    '            sql.Append(" SELECT RoomName,SubjectName,SUM(TotalScore) as TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & SchoolId & "' And ")
    '        End If
    '        'sql.Append(" StudentCalendarId = '" & CalendarId & "' AND ")
    '        If StrClass.Count > 1 Then
    '            IsMoreOneRoom = True
    '            sql.Append("(")
    '            For index = 0 To StrClass.Count - 1
    '                sql.Append(" RoomName = '" & _DB.CleanString(StrClass(index).ToString().Trim()) & "' OR ")
    '            Next
    '        Else
    '            sql.Append(" RoomName = '" & _DB.CleanString(StrClass(0).ToString().Trim()) & "' ")
    '        End If
    '        Dim FullStr As String
    '        If IsMoreOneRoom = True Then
    '            FullStr = sql.ToString().Substring(0, sql.ToString().Length - 3) & ")" & " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '            If TeacherId <> "" Then
    '                FullStr &= " AND Assistant_id = '" & TeacherId & "' "
    '            End If
    '            If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where TestsetId และ Group By เพิ่ม
    '                FullStr &= " AND TestSet_Id = '" & TestSetId & "' GROUP BY SubjectName,RoomName "
    '            End If
    '            FullStr &= " ORDER BY RoomName "
    '        Else
    '            FullStr = sql.ToString() & " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '            If TeacherId <> "" Then
    '                FullStr &= " AND Assistant_id = '" & TeacherId & "' "
    '            End If
    '            If TestSetId <> "" Then
    '                FullStr &= " AND TestSet_Id = '" & TestSetId & "' GROUP BY SubjectName,RoomName "
    '            End If
    '            FullStr &= " ORDER BY RoomName "
    '        End If
    '        dtComplete = _DB.getdata(FullStr)
    '    ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
    '        If TestSetId = "" Then
    '            dt = CreateDtTopLow(SchoolId, StrClass, SortType, "RoomsubjectTotalscore", "RoomName", ChooseMode, "RoomName", TestSetId) 'เข้า Function เพื่อเอาลำดับห้องตามที่ Sort
    '        Else
    '            dt = CreateDtTopLow(SchoolId, StrClass, SortType, "RoomTestsetSubjectTotalscore ", "RoomName", ChooseMode, "RoomName", TestSetId)
    '        End If
    '        If dt.Rows.Count > 0 Then
    '            Dim EachRoomName As String
    '            For t = 0 To dt.Rows.Count - 1
    '                EachRoomName = dt.Rows(t)("RoomName").ToString()
    '                Dim StrSql As String = ""
    '                If TestSetId = "" Then
    '                    StrSql = " SELECT RoomName,SubjectName,TotalScore FROM dbo.uvw_Chart_RoomSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                             " AND RoomName = '" & _DB.CleanString(EachRoomName.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                    If TeacherId <> "" Then
    '                        StrSql &= " AND Assistant_id = '" & TeacherId & "' "
    '                    End If
    '                Else 'ถ้าเป็นโหมดฝึกฝนต้องใช้ Query นี้
    '                    StrSql = "  SELECT RoomName,SubjectName,SUM(TotalScore) FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                             " AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' " & _
    '                             " AND TestSet_Id = '" & TestSetId & "' AND RoomName = '" & EachRoomName & "' "
    '                    If TeacherId <> "" Then
    '                        StrSql &= " AND Assistant_id = '" & TeacherId & "' "
    '                    End If
    '                    StrSql &= " GROUP BY RoomName,SubjectName "
    '                End If
    '                Dim dt2 As New DataTable
    '                dt2 = _DB.getdata(StrSql)
    '                If dt2.Rows.Count > 0 Then
    '                    For Each objrow In dt2.Rows
    '                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                    Next
    '                Else
    '                    dtComplete.Rows.Add(EachRoomName, "กลุ่มสาระการเรียนรู้ภาษาไทย", "0")
    '                End If
    '            Next
    '        Else
    '            Return DataHash2
    '        End If
    '    End If


    '    If dtComplete.Rows.Count > 0 Then 'ถ้าได้ dtComplete แล้วค่อยลงมาเพื่อทำ Hashtable 
    '        Dim ArrCategories As New ArrayList
    '        Dim ArrClass As New ArrayList
    '        For i = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(i)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(i)("SubjectName"), "")
    '            End If

    '            If ArrClass.Contains(dtComplete.Rows(i)("RoomName")) = False Then
    '                ArrClass.Add(dtComplete.Rows(i)("RoomName"))
    '                ArrCategories.Add(dtComplete.Rows(i)("RoomName"))
    '            End If
    '        Next

    '        'ArrClass.Add("ม.91")
    '        'ArrClass.Add("ม.92")
    '        'ArrClass.Add("ม.93")
    '        'ArrClass.Add("ม.94")
    '        'ArrClass.Add("ม.95")
    '        'ArrClass.Add("ม.96")
    '        'ArrClass.Add("ม.97")

    '        'ArrCategories.Add("ม.91")
    '        'ArrCategories.Add("ม.92")
    '        'ArrCategories.Add("ม.93")
    '        'ArrCategories.Add("ม.94")
    '        'ArrCategories.Add("ม.95")
    '        'ArrCategories.Add("ม.96")
    '        'ArrCategories.Add("ม.97")

    '        HttpContext.Current.Session("ArrChartClass") = ArrCategories
    '        For Each row In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(row)
    '            For j = 0 To ArrClass.Count - 1
    '                Dim CurrentRoom As String = ArrClass(j)
    '                Dim dtrow() As DataRow = dtComplete.Select("RoomName = '" & CurrentRoom & "' and SubjectName = '" & row & "' ")
    '                If dtrow.Length > 0 Then
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next
    '    Else
    '        Return DataHash2
    '    End If

    '    Return DataHash2

    'End Function '****3**** 
    'Function หา Datatable ที่เป็นคะแนนเฉลี่ยของนักเรียนแต่ละคน เอาไปใช้กับ กราฟแท่ง ที่เป็นคะแนนเฉลี่ยของควิซทุกครั้ง ของ ห้อง ... ชุด ...
    'Public Function GetDtAverageScorePerStudent(ByVal TestSetId As String, ByVal RoomName As String, ByVal SchoolId As String, ByVal SortType As String, ByVal ChooseMode As Integer)

    '    Dim dt As New DataTable
    '    If TestSetId = "" Or TestSetId Is Nothing Or RoomName = "" Or RoomName Is Nothing Or SchoolId = "" Or SchoolId Is Nothing Then
    '        Return dt
    '    End If

    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return dt
    '    End If

    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeworkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If

    '    Dim sql As String = ""
    '    'ถ้าเป็นการบ้านใช้ Query นี้
    '    'If ChooseMode = 2 Then
    '    '    sql = " SELECT NoInRoom,CAST( ( (SUM(TotalScore) * 100 )) / SUM(FullScore) AS NUMERIC(10,2) ) AS TotalScore " & _
    '    '          " FROM dbo.uvw_Chart_AvgStudentScoreHomework WHERE SchoolCode = '" & SchoolId & "' AND RoomName = '" & RoomName & "' " & _
    '    '          " AND StudentCalendar = '" & CalendarId & "' AND Calendar_Id = '" & CalendarId & "' " & _
    '    '          " AND TestSet_Id = '" & TestSetId & "' GROUP BY NoInRoom "
    '    'Else 'ถ้าเป็นฝึกฝนกับควิซใช้ Query นี้
    '    sql = " SELECT NoInRoom,CASE WHEN CAST(((SUM(TotalScore) * 100) / SUM(FullScore)) AS NUMERIC(10,2) ) IS NULL THEN 0 ELSE CAST(((SUM(TotalScore) * 100) / " & _
    '          " SUM(FullScore)) AS NUMERIC(10,2) ) END AS Totalscore  " & _
    '          " FROM dbo.uvw_Chart_AvgStudentScoreForReduce WHERE SchoolCode = '" & SchoolId & "' AND RoomName = '" & RoomName & "' AND " & _
    '          " TestSet_Id = '" & TestSetId & "' AND " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' " & _
    '          " GROUP BY TestSet_Id,NoInRoom "
    '    'End If

    '    If SortType <> "0" Then
    '        If SortType = "1" Then
    '            sql &= " ORDER BY CAST(((SUM(TotalScore) * 100) / SUM(FullScore)) AS NUMERIC(10,2) ) desc "
    '        Else
    '            sql &= " ORDER BY CAST(((SUM(TotalScore) * 100) / SUM(FullScore)) AS NUMERIC(10,2) ) asc "
    '        End If
    '    Else
    '        sql &= " ORDER BY NoInRoom "
    '    End If

    '    dt = _DB.getdata(sql)
    '    Return dt

    'End Function

    'Public Function GenStrLineChart(ByVal Title As String, ByVal Subtitle As String, ByVal YAxisTitle As String, _
    '                                    ByVal DataChart As Hashtable, ByVal ArrCategories As ArrayList, ByVal XAxisTitle As String, Optional ByVal Width As Double = 730) As String

    '    If Title IsNot Nothing And Title <> "" And Subtitle IsNot Nothing And Subtitle <> "" And YAxisTitle IsNot Nothing _
    '        And YAxisTitle <> "" And DataChart IsNot Nothing And ArrCategories IsNot Nothing Or XAxisTitle Is Nothing Or XAxisTitle = "" Then

    '        Dim CodeChart As New StringBuilder
    '        CodeChart.Append(" chart = new Highcharts.Chart({chart: {renderTo:'DivReport',width:" & Width & ",type:'line'},title: {text:'<b>$Title</b>'},")
    '        CodeChart.Append("exporting:{buttons:{exportButton:{enabled:false},printButton:{x:-10}}},")
    '        CodeChart.Append(" subtitle: {text:'$Subtitle'},xAxis: {title: {text:''},")
    '        CodeChart.Append("categories:[")

    '        If ArrCategories.Count = 0 Then
    '            Return "-1"
    '        End If

    '        For z = 0 To ArrCategories.Count - 1
    '            Dim EachCategories As String = ArrCategories(z).ToString()
    '            If z = ArrCategories.Count - 1 Then
    '                CodeChart.Append("'" & EachCategories & "']")
    '            Else
    '                CodeChart.Append("'" & EachCategories & "',")
    '            End If
    '        Next

    '        CodeChart.Append("},yAxis: {title: {text:'$YAxisTitle'}},plotOptions: {},")
    '        CodeChart.Append(" series: [")

    '        If DataChart.Count = 0 Then
    '            Return "-1"
    '        End If

    '        For Each row In DataChart
    '            Dim EachName As String = row.Key
    '            Dim DataStr As String = row.Value
    '            Dim ArrData = DataStr.Split(",")
    '            CodeChart.Append("{name: '" & EachName & "',data:[")
    '            For i = 0 To ArrData.Count - 1
    '                Dim Eachdata As Double = ArrData(i)
    '                If i = ArrData.Count - 1 Then
    '                    CodeChart.Append("" & Eachdata & "]")
    '                Else
    '                    CodeChart.Append("" & Eachdata & ",")
    '                End If
    '            Next
    '            CodeChart.Append("},")
    '        Next

    '        Dim ForSubStr As String = CodeChart.ToString.Substring(0, CodeChart.ToString().Length - 2)
    '        CodeChart.Clear()
    '        CodeChart.Append(ForSubStr)
    '        CodeChart.Append(" }]});")
    '        Dim FullStr As String = CodeChart.ToString()

    '        FullStr = ReplaceStrCodeChart(FullStr, 2, Title, Subtitle, YAxisTitle, "NoFunction", XAxisTitle) 'มีปัญหาเพราะว่าไม่มี Function แล้ว

    '        Return FullStr

    '    Else
    '        Return "-1"
    '    End If

    'End Function

    'Public Function GenStrDrillDownPieChart(ByVal Title As String, ByVal HashCategories As Hashtable, ByVal dtData As DataTable, ByVal FieldName As String, ByVal ClassOrRoomOrNoinRoomName As String, ByVal UnitName As String, ByVal IsQuiz As Boolean, Optional ByVal SelectField As String = "RoomName") As String

    '    If Title Is Nothing Or Title = "" Or HashCategories Is Nothing Or HashCategories.Count = 0 Or dtData Is Nothing Or dtData.Rows.Count = 0 Or FieldName Is Nothing Or FieldName = "" Then
    '        Return "-1"
    '    End If

    '    Dim CodeChart As New StringBuilder
    '    Dim SpareStr As String = ""
    '    CodeChart.Append("var colors = Highcharts.getOptions().colors,")
    '    CodeChart.Append(" categories =[")
    '    Dim SubjectName As String = ""
    '    For i = 0 To HashCategories.Count - 1 'วนเอา Categories จาก HashtableCategories
    '        If IsQuiz = False Then
    '            If SelectField = "RoomName" Then
    '                SubjectName = GenSubjectName(HashCategories.Keys(i).ToString())
    '            Else
    '                SubjectName = HashCategories.Keys(i).ToString()
    '            End If
    '        Else
    '            If SelectField = "RoomName" Then
    '                SubjectName = HashCategories.Keys(i).ToString()
    '            Else
    '                SubjectName = HashCategories.Keys(i).ToString()
    '            End If
    '        End If
    '        CodeChart.Append("'" & SubjectName & "',")
    '    Next
    '    SpareStr = CutCommaLastString(CodeChart.ToString())
    '    CodeChart.Clear()
    '    CodeChart.Append(SpareStr & "],")
    '    CodeChart.Append(" name = '" & Title & "',")
    '    CodeChart.Append("data = [")
    '    For a = 0 To HashCategories.Count - 1 'วนตามวิชาใน Hastable เพื่อสร้าง Data
    '        Dim CurrentSubjectName As String = HashCategories.Keys(a).ToString()
    '        Dim DataY As String = HashCategories(CurrentSubjectName).ToString()
    '        CodeChart.Append("{y: " & DataY & ",color: colors[" & a & "],drilldown:{categories:[")
    '        Dim dtRow() As DataRow
    '        If IsQuiz = True Then
    '            dtRow = dtData.Select(SelectField & " = '" & CurrentSubjectName & "' ") 'หา Row จาก Datatable โดยหาจากห้อง
    '        Else
    '            dtRow = dtData.Select("SubjectName = '" & CurrentSubjectName & "' ") 'หา Row จาก Datatable โดยหาจากวิชา
    '        End If
    '        Dim EachCategories As New ArrayList
    '        Dim EachData As New ArrayList
    '        If dtRow.Length > 0 Then
    '            For Each objrow In dtRow
    '                If IsQuiz = False Then
    '                    EachCategories.Add(objrow("" & FieldName & "")) 'นำห้องที่มีในวิชาที่หามา Add ลง Array
    '                Else
    '                    EachCategories.Add(GenSubjectName(objrow("" & FieldName & "")))
    '                End If

    '                EachData.Add(objrow("TotalScore")) 'นำคะแนนจากห้องข้างบนมา Add ลง Array
    '            Next
    '            For z = 0 To EachCategories.Count - 1
    '                CodeChart.Append("'" & EachCategories(z) & "',") 'วนต่อสตริงว่ามีห้องอะไรบ้างที่สอบวิชานี้
    '            Next
    '            SpareStr = CutCommaLastString(CodeChart.ToString())
    '            CodeChart.Clear()
    '            CodeChart.Append(SpareStr & "],data:[")
    '            For x = 0 To EachData.Count - 1
    '                CodeChart.Append("" & EachData(x) & ",") 'วนต่อสตริงคะแนนว่าห้องที่สอบวิชานี้ได้กี่คะแนน
    '            Next
    '            SpareStr = CutCommaLastString(CodeChart.ToString())
    '            CodeChart.Clear()
    '            CodeChart.Append(SpareStr & "],color: colors[" & a & "]}},")
    '        End If
    '    Next
    '    SpareStr = CutCommaLastString(CodeChart.ToString())
    '    CodeChart.Clear()
    '    CodeChart.Append(SpareStr)
    '    CodeChart.Append("];var TotalData = [];var Classdata = [];for (var i = 0; i < data.length; i++) {TotalData.push({name: categories[i],y: data[i].y,")
    '    CodeChart.Append("color:data[i].color});for (var j = 0; j < data[i].drilldown.data.length; j++) {var brighhness = 0.2 - (j / data[i].drilldown.data.length) / 5;")
    '    CodeChart.Append("Classdata.push({name: data[i].drilldown.categories[j],y: data[i].drilldown.data[j],color: Highcharts.Color(data[i].color).brighten(brighhness).get()")
    '    CodeChart.Append("});}}")
    '    CodeChart.Append("chart = new Highcharts.Chart({chart: {renderTo: 'DivReport',type: 'pie'},title: { text: '" & Title & "'},exporting: {buttons: {exportButton: {enabled: false},printButton: {x:-10}}},plotOptions: {pie: {shadow: true,allowPointSelect:true}},")
    '    CodeChart.Append("tooltip: {formatter: function () {return '<b>'  + this.point.name + '</b>: ' + this.y + '" & UnitName & "';}},series: [{name: 'วิชา',data: TotalData,size: '60%',")
    '    CodeChart.Append("dataLabels: {formatter: function () {return this.y > 5 ? this.point.name : null;},color: 'white',distance: -30}}, { name: 'ห้อง',data: Classdata,")
    '    CodeChart.Append("innerSize: '60%',dataLabels: {formatter: function () {return this.y > 1 ? '<b>' + '" & ClassOrRoomOrNoinRoomName & "' + this.point.name + ':</b>' + this.y + '" & UnitName & "' : null;}}}]});")

    '    Return CodeChart.ToString()

    'End Function 'Function ต่อสตริง กราฟวงกลม

    'Public Function dtPieMainLeval(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "") As DataTable

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Then
    '        Return dt
    '    End If
    '    'If ChooseMode = 2 Then
    '    '    dt = QueryPieDt(SchoolId, ArrClass, "ClassSubjectTotalscoreHomework", "ClassName", "ClassName", SortType, False, False, ChooseMode, TeacherId)
    '    'Else
    '    dt = QueryPieDt(SchoolId, ArrClass, "ClassSubjectTotalscore", "ClassName", "ClassName", SortType, False, False, ChooseMode, TeacherId)
    '    'End If
    '    Return dt

    'End Function 'dt วงกลม MainLevel(1)

    'Public Function QueryPieDt(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal ViewName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal SortType As String, ByVal WhereFieldIsMorethanOne As Boolean, ByVal IsQuiz As Boolean, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or SelectFieldName Is Nothing Or SelectFieldName = "" Or ViewName Is Nothing Or ViewName = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return dt
    '    End If
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return dt
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As New StringBuilder
    '    Dim SpareStr As String = ""
    '    sql.Append("SELECT ")
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าไป
    '        sql.Append(" Top 10 ")
    '    End If
    '    sql.Append(SelectFieldName)
    '    If SortType = "0" Then 'ถ้าเป็นโหมดปกติ ถึงจะ Select 3 Field
    '        sql.Append(" ,SubjectName,CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore ")
    '    End If
    '    sql.Append(" FROM dbo.uvw_Chart_" & ViewName & " WHERE ")
    '    If IsQuiz = False Then
    '        sql.Append("SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ( ")
    '        If ArrClass.Count > 1 Then
    '            For i = 0 To ArrClass.Count - 1
    '                Dim CurrentClass As String = _DB.CleanString(ArrClass(i).ToString().Trim())
    '                sql.Append(" " & WhereFieldName & " = '" & CurrentClass & "' Or ")
    '            Next
    '            SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
    '            sql.Clear()
    '            sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "'  AND " & StrWhere & " = '1' ")
    '        Else
    '            sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
    '        End If
    '    Else
    '        sql.Append(" " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' AND Quiz_Id = '" & WhereFieldName & "' AND RoomName = '" & ArrClass(0) & "' ")
    '    End If
    '    If TeacherId <> "" Then
    '        sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '    End If
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าเงื่อนไขนี้
    '        dt.Columns.Add(SelectFieldName)
    '        dt.Columns.Add("SubjectName")
    '        dt.Columns.Add("TotalScore")
    '        'sql.Append(" AND AcademicYear = '" & AcademicYear & "' AND IsPracticeMode = '" & PracticeMode & "' GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
    '        sql.Append(" GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
    '        If SortType = "1" Then
    '            sql.Append(" desc ")
    '        Else
    '            sql.Append(" asc ")
    '        End If
    '        Dim dt2 As New DataTable
    '        dt2 = _DB.getdata(sql.ToString())
    '        If dt2.Rows.Count > 0 Then
    '            For index = 0 To dt2.Rows.Count - 1
    '                Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
    '                Dim sqlStr As String = " SELECT " & SelectFieldName & ",SubjectName,CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim) & "' " & _
    '                       " AND " & SelectFieldName & " = '" & ClassOrRoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                If WhereFieldIsMorethanOne = True Then
    '                    sqlStr = sqlStr & " AND " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' "
    '                End If
    '                If IsQuiz = True Then
    '                    sqlStr = sqlStr & " AND  Quiz_Id = '" & WhereFieldName & "' "
    '                End If
    '                If TeacherId <> "" Then
    '                    sqlStr &= " AND Assistant_id = '" & TeacherId & "' "
    '                End If
    '                Dim dt3 As New DataTable
    '                dt3 = _DB.getdata(sqlStr)
    '                If dt3.Rows.Count > 0 Then
    '                    For Each allrow In dt3.Rows
    '                        dt.Rows.Add(allrow(0), allrow(1), allrow(2))
    '                    Next
    '                End If
    '            Next
    '        Else
    '            Return dt
    '        End If
    '        HttpContext.Current.Session("dtCompleteTopLow") = dt
    '    End If

    '    If SortType = "0" Then
    '        dt = _DB.getdata(sql.ToString())
    '    End If

    '    Return dt

    'End Function 'ทำ dt ของวงกลม 1-4

    'Public Function HashPieMainlevel(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal Sortype As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "") As Hashtable

    '    Dim DataHash As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or Sortype Is Nothing Or Sortype = "" Then
    '        Return DataHash
    '    End If
    '    Dim CheckSort As Boolean
    '    If Sortype = "1" Or Sortype = "2" Then
    '        CheckSort = True
    '    Else
    '        CheckSort = False
    '    End If
    '    'If ChooseMode = 2 Then
    '    '    DataHash = QueryPieHashFromView(SchoolId, "ClassSubjectTotalscoreHomework", ArrClass, "ClassName", CheckSort, False, ChooseMode, , TeacherId)
    '    'Else
    '    DataHash = QueryPieHashFromView(SchoolId, "ClassSubjectTotalscore", ArrClass, "ClassName", CheckSort, False, ChooseMode, , TeacherId)
    '    'End If
    '    Return DataHash

    'End Function 'Hash วงกลม MainLevel(1)

    'Public Function QueryPieHashFromView(ByVal SchoolId As String, ByVal ViewName As String, ByVal ArrClass As ArrayList, ByVal WhereFieldName As String, ByVal IsSort As Boolean, ByVal IsQuiz As Boolean, ByVal ChooseMode As Integer, Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") As Hashtable

    '    Dim DataHash As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ViewName Is Nothing Or ViewName = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Then
    '        Return DataHash
    '    End If

    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    If IsSort = True Then 'ถ้าเกิดมีการเลือกโหมด Top10 Low10 จะใช้ dt จาก Session เพื่อมาวนทำ Hashtable
    '        If HttpContext.Current.Session("dtCompleteTopLow") Is Nothing Then
    '            Return DataHash
    '        End If
    '        Dim dtTopLow As DataTable = HttpContext.Current.Session("dtCompleteTopLow")
    '        Dim ArrSubject As New ArrayList
    '        Dim ArrClassOrRoomOrNoInRoom As New ArrayList
    '        For z = 0 To dtTopLow.Rows.Count - 1
    '            If ArrSubject.Contains(dtTopLow.Rows(z)(1).ToString()) = False Then 'วนเพื่อเก็บวิชาทั้งหมดเข้า Array
    '                ArrSubject.Add(dtTopLow.Rows(z)(1).ToString())
    '            End If
    '        Next

    '        For Each allobj In ArrSubject
    '            Dim TotalScore As Double = 0
    '            Dim dtRow() As DataRow = dtTopLow.Select("SubjectName = '" & allobj & "' ")
    '            If dtRow.Count > 0 Then
    '                For Each allScore In dtRow
    '                    TotalScore = TotalScore + allScore(2)
    '                Next
    '            End If
    '            DataHash.Add(allobj, TotalScore)
    '        Next
    '        HttpContext.Current.Session("dtCompleteTopLow") = Nothing

    '    Else 'ถ้าไม่ได้เลือกโหมด Top10 Low10 จะคิวรี่ข้อมูลเพื่อทำ Hashtable
    '        Dim sql As New StringBuilder
    '        Dim SpareStr As String = ""
    '        sql.Append(" SELECT SubjectName,CASE WHEN SUM(TotalScore) IS NULL THEN '0' ELSE SUM(TotalScore) END As SumScore FROM dbo.uvw_Chart_" & ViewName.Trim() & " ")
    '        If IsQuiz = False Then
    '            sql.Append(" WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ( ")
    '            If ArrClass.Count > 1 Then
    '                For index = 0 To ArrClass.Count - 1
    '                    Dim CurrentRoomName As String = _DB.CleanString(ArrClass(index).ToString().Trim())
    '                    sql.Append(" " & WhereFieldName & " = '" & CurrentRoomName & "' Or ")
    '                Next
    '                SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
    '                sql.Clear()
    '                sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
    '                If TeacherId <> "" Then
    '                    sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '                End If
    '                If TestSetId = "" Then 'ถ้าเป็นแบบฝึกฝนจะส่ง Testset_Id เข้ามาด้วย ต้อง where เพิ่มเข้าไป
    '                    sql.Append(" GROUP BY SubjectName ")
    '                Else
    '                    sql.Append(" AND TestSet_Id = '" & TestSetId & "' GROUP BY SubjectName ")
    '                End If
    '            Else
    '                sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
    '                If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where Testset_id ด้วย
    '                    sql.Append(" AND TestSet_Id = '" & TestSetId & "' ")
    '                End If
    '                If TeacherId <> "" Then
    '                    sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '                End If
    '                sql.Append(" GROUP BY SubjectName ")
    '            End If
    '        Else
    '            sql.Append(" WHERE Quiz_Id = '" & WhereFieldName & "' AND RoomName = '" & ArrClass(0) & "' AND " & StrWhere & " = '1' ")
    '            If TeacherId <> "" Then
    '                sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '            End If
    '            'If TestSetId <> "" Then 'ถ้าเป็นโหมดฝึกฝนต้อง Where TestsetId ด้วย
    '            '    sql.Append(" AND TestSet_Id = '" & TestSetId & "' ")
    '            'End If
    '            sql.Append(" GROUP BY SubjectName ")
    '        End If
    '        Dim dt As New DataTable
    '        dt = _DB.getdata(sql.ToString())
    '        If dt.Rows.Count > 0 Then
    '            For z = 0 To dt.Rows.Count - 1
    '                DataHash.Add(dt.Rows(z)("SubjectName"), dt.Rows(z)("SumScore"))
    '            Next
    '        End If
    '    End If

    '    Return DataHash

    'End Function 'ทำ Hashtable ของวงกลม 1-4

    'Public Function CreateDtTopLow(ByVal SchoolId As String, ByVal ArrClassRoom As ArrayList, ByVal SortType As String, ByVal ViewName As String, ByVal SelectField As String, ByVal ChooseMode As Integer, Optional ByVal WhereField As String = "", Optional ByVal TestSetId As String = "", Optional ByVal TeacherId As String = "") As DataTable

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClassRoom Is Nothing Or ArrClassRoom.Count = 0 Or SelectField Is Nothing Or SelectField = "" Or SelectField Is Nothing Or SelectField = "" Or WhereField Is Nothing _
    '         Or ViewName Is Nothing Or ViewName = "" Then

    '        Return dt
    '    End If

    '    Dim CalendarId As String = GetCalendarId(SchoolId)
    '    If CalendarId = "" Then
    '        Return dt
    '    End If

    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If

    '    Dim sql As New StringBuilder
    '    Dim SpareStr As String
    '    sql.Append(" SELECT TOP 10 " & SelectField & " FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND " & StrWhere & " = '1' ")
    '    If WhereField <> "" Then
    '        sql.Append(" AND (  ")
    '        If ArrClassRoom.Count > 1 Then
    '            For u = 0 To ArrClassRoom.Count - 1
    '                Dim EachRoomName As String = ArrClassRoom(u).ToString().Trim()
    '                sql.Append(" " & WhereField & " = '" & _DB.CleanString(EachRoomName) & "' Or ")
    '            Next
    '            SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
    '            sql.Clear()
    '            sql.Append(SpareStr & " ) ")
    '        Else
    '            sql.Append(" " & WhereField & " ='" & _DB.CleanString(ArrClassRoom(0).ToString().Trim()) & "' ) ")
    '        End If
    '    End If
    '    If TeacherId <> "" Then
    '        sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '    End If
    '    If TestSetId = "" Then
    '        sql.Append(" AND Calendar_Id = '" & CalendarId & "' GROUP BY " & SelectField & " ORDER BY SUM(TotalScore) ")
    '    Else
    '        sql.Append(" AND Calendar_Id = '" & CalendarId & "' AND TestSet_Id = '" & TestSetId & "' GROUP BY " & SelectField & " ORDER BY SUM(TotalScore) ")
    '    End If

    '    If SortType = "1" Then
    '        sql.Append(" desc ")
    '    Else
    '        sql.Append(" asc ")
    '    End If
    '    dt = _DB.getdata(sql.ToString())
    '    Return dt

    'End Function

    'Public Function dtPieSecondLevel(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal State As String, ByVal SortType As String, ByVal ChooseMode As Integer)

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or State Is Nothing Or State = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return dt
    '    End If
    '    'Dim ViewName As String = ""
    '    'If ChooseMode = 2 Then
    '    '    ViewName = "Homework"
    '    'End If
    '    If State = "2" Then
    '        dt = QueryPieDt(SchoolId, ArrClass, "ClassroomSubjectTotalscore", "RoomName", "ClassName", SortType, False, False, ChooseMode)
    '    Else
    '        dt = QueryPieDt(SchoolId, ArrClass, "ClassroomSubjectTotalscore", "RoomName", "RoomName", SortType, False, False, ChooseMode)
    '    End If

    '    Return dt

    'End Function 'dt วงกลม(2-3)

    'Public Function QueryPieDt(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal ViewName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal SortType As String, ByVal WhereFieldIsMorethanOne As Boolean, ByVal IsQuiz As Boolean, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or SelectFieldName Is Nothing Or SelectFieldName = "" Or ViewName Is Nothing Or ViewName = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return dt
    '    End If
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return dt
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As New StringBuilder
    '    Dim SpareStr As String = ""
    '    sql.Append("SELECT ")
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าไป
    '        sql.Append(" Top 10 ")
    '    End If
    '    sql.Append(SelectFieldName)
    '    If SortType = "0" Then 'ถ้าเป็นโหมดปกติ ถึงจะ Select 3 Field
    '        sql.Append(" ,CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore ")
    '    End If
    '    sql.Append(" FROM dbo.uvw_Chart_" & ViewName & " WHERE ")
    '    If IsQuiz = False Then
    '        sql.Append("SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND ( ")
    '        If ArrClass.Count > 1 Then
    '            For i = 0 To ArrClass.Count - 1
    '                Dim CurrentClass As String = _DB.CleanString(ArrClass(i).ToString().Trim())
    '                sql.Append(" " & WhereFieldName & " = '" & CurrentClass & "' Or ")
    '            Next
    '            SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
    '            sql.Clear()
    '            sql.Append(SpareStr & " ) AND Calendar_Id = '" & CalendarId & "'  AND " & StrWhere & " = '1' ")
    '        Else
    '            sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ")
    '        End If
    '    Else
    '        sql.Append(" " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' AND Quiz_Id = '" & WhereFieldName & "' AND RoomName = '" & ArrClass(0) & "' ")
    '    End If
    '    If TeacherId <> "" Then
    '        sql.Append(" AND Assistant_id = '" & TeacherId & "' ")
    '    End If
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าเงื่อนไขนี้
    '        dt.Columns.Add(SelectFieldName)
    '        'dt.Columns.Add("SubjectName")
    '        dt.Columns.Add("TotalScore")
    '        'sql.Append(" AND AcademicYear = '" & AcademicYear & "' AND IsPracticeMode = '" & PracticeMode & "' GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
    '        sql.Append(" GROUP BY " & SelectFieldName & " ORDER BY SUM(TotalScore) ")
    '        If SortType = "1" Then
    '            sql.Append(" desc ")
    '        Else
    '            sql.Append(" asc ")
    '        End If
    '        Dim dt2 As New DataTable
    '        dt2 = _DB.getdata(sql.ToString())
    '        If dt2.Rows.Count > 0 Then
    '            For index = 0 To dt2.Rows.Count - 1
    '                Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
    '                Dim sqlStr As String = " SELECT " & SelectFieldName & ",CASE WHEN  TotalScore  IS NULL THEN 0 ELSE TotalScore END AS TotalScore FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim) & "' " & _
    '                       " AND " & SelectFieldName & " = '" & ClassOrRoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                If WhereFieldIsMorethanOne = True Then
    '                    sqlStr = sqlStr & " AND " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' "
    '                End If
    '                If IsQuiz = True Then
    '                    sqlStr = sqlStr & " AND  Quiz_Id = '" & WhereFieldName & "' "
    '                End If
    '                If TeacherId <> "" Then
    '                    sqlStr &= " AND Assistant_id = '" & TeacherId & "' "
    '                End If
    '                Dim dt3 As New DataTable
    '                dt3 = _DB.getdata(sqlStr)
    '                If dt3.Rows.Count > 0 Then
    '                    For Each allrow As DataRow In dt3.Rows
    '                        dt.Rows.Add(allrow(0), allrow(1))
    '                    Next
    '                End If
    '            Next
    '        Else
    '            Return dt
    '        End If
    '        HttpContext.Current.Session("dtCompleteTopLow") = dt
    '    End If

    '    If SortType = "0" Then
    '        dt = _DB.getdata(sql.ToString())
    '    End If

    '    Return dt

    'End Function 'ทำ dt ของวงกลม 1-4

    'Public Function HashChartAllRoomInClassChoose(ByVal SchoolId As String, ByVal StrChooseAllRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer, Optional ByVal TeacherId As String = "")

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or StrChooseAllRoom Is Nothing Or StrChooseAllRoom = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return DataHash2
    '    End If

    '    Dim sql As String = ""
    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("RoomName")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    If SortType = "0" Then 'ถ้าไม่เป็นโหมด Top10 Low10 เข้าเงื่อนไข Query นี้
    '        sql = " SELECT RoomName,SubjectName,TotalScore FROM dbo.uvw_Chart_ClassRoomSubjectTotalscore " & _
    '              " WHERE SchoolCode = '" & _DB.CleanString(SchoolId) & "' AND ClassName = '" & _DB.CleanString(StrChooseAllRoom.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '        If TeacherId <> "" Then
    '            sql &= " AND Assistant_id = '" & TeacherId & "' "
    '        End If
    '        sql &= " ORDER BY RoomName "
    '        dtComplete = _DB.getdata(sql)
    '    ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เข้าเงื่อนไข query นี้
    '        Dim DummyArrClass As New ArrayList
    '        DummyArrClass.Add(StrChooseAllRoom)
    '        dt = CreateDtTopLow(SchoolId, DummyArrClass, SortType, "ClassRoomSubjectTotalscore", "RoomName", ChooseMode, "ClassName")
    '        If dt.Rows.Count > 0 Then
    '            Dim EachRoomName As String
    '            For t = 0 To dt.Rows.Count - 1
    '                EachRoomName = dt.Rows(t)("RoomName").ToString()
    '                Dim StrSql As String = " SELECT RoomName,SubjectName,TotalScore FROM dbo.uvw_Chart_ClassRoomSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                                       " AND RoomName = '" & _DB.CleanString(EachRoomName.Trim()) & "' And Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' "
    '                If TeacherId <> "" Then
    '                    sql &= " AND Assistant_id = '" & TeacherId & "' "
    '                End If
    '                Dim dt2 As New DataTable
    '                dt2 = _DB.getdata(StrSql)
    '                If dt2.Rows.Count > 0 Then
    '                    For Each objrow In dt2.Rows
    '                        dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                    Next
    '                Else
    '                    dtComplete.Rows.Add(EachRoomName, "กลุ่มสาระการเรียนรู้ภาษาไทย", "0")
    '                End If
    '            Next
    '        Else
    '            Return DataHash2
    '        End If
    '    End If

    '    If dtComplete.Rows.Count > 0 Then
    '        Dim ArrClass As New ArrayList
    '        For i = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(i)("SubjectName")) = False Then
    '                DataHash.Add(dtComplete.Rows(i)("SubjectName"), "")
    '            End If

    '            If ArrClass.Contains(dtComplete.Rows(i)("RoomName")) = False Then
    '                ArrClass.Add(dtComplete.Rows(i)("RoomName"))
    '            End If
    '        Next
    '        HttpContext.Current.Session("ArrCatAllRoomInClass") = ArrClass
    '        For Each row In DataHash.Keys
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(row)
    '            For j = 0 To ArrClass.Count - 1
    '                Dim CurrentRoom As String = ArrClass(j)
    '                Dim dtrow() As DataRow = dtComplete.Select("RoomName = '" & CurrentRoom & "' and SubjectName = '" & row & "' ")
    '                If dtrow.Length > 0 Then
    '                    Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                Else
    '                    Scores = Scores & "0,"
    '                End If
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next
    '    Else
    '        Return DataHash2
    '    End If

    '    Return DataHash2

    'End Function '****2**** 

    'Public Function dtPieOnlyRoom(ByVal SchoolId As String, ByVal StrRoom As String, ByVal SortType As String, ByVal ChooseMode As Integer)

    '    Dim dt As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or StrRoom Is Nothing Or StrRoom = "" Or SortType Is Nothing Or SortType = "" Then
    '        Return dt
    '    End If

    '    Dim ArrClass As New ArrayList
    '    ArrClass.Add(StrRoom)
    '    dt = QueryPieDt(SchoolId, ArrClass, "RoomNoSubjectTotalscore", "NoInRoom", "RoomName", SortType, True, False, ChooseMode)
    '    Return dt

    'End Function 'dt วงกลมเฉพาะห้อง (4)

    'Public Function HashChartActivity(ByVal SchoolId As String, ByVal TestSet_ID As String, ByVal ArrRoom As ArrayList, ByVal SortType As String, ByVal ChooseMode As Integer)
    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    If SchoolId Is Nothing Or SchoolId = "" Or TestSet_ID Is Nothing Or TestSet_ID = "" Or ArrRoom.Count = 0 Or SortType Is Nothing Or SortType = "" Then
    '        Return DataHash2
    '    End If
    '    Dim sql As String
    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    dtComplete.Columns.Add("RoomName")
    '    dtComplete.Columns.Add("SubjectName")
    '    dtComplete.Columns.Add("TotalScore")
    '    dtComplete.Columns.Add("Quiz_Id")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return DataHash2
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim ArrClassRoom As New ArrayList
    '    Dim ArrQuizId As New ArrayList
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเลือกแบบ Top10 - Low10 ต้อง Select Top 10 Quiz_id กับ ห้อง มา
    '        sql = " SELECT Top 10 RoomName,quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '              " AND " & StrWhere & " = '1' AND ( "
    '        If ArrRoom.Count > 1 Then
    '            For p = 0 To ArrRoom.Count - 1
    '                sql = sql & " RoomName = '" & ArrRoom(p) & "' Or "
    '            Next
    '            sql = sql.Substring(0, sql.Length - 3)
    '            sql = sql & (" ) ")
    '        Else
    '            sql = sql & " RoomName = '" & ArrRoom(0).ToString() & "' ) "
    '        End If
    '        If SortType = "1" Then 'ถ้า SortType = 1 คือ Top10 ถ้า 2 คือ Low10
    '            sql = sql & " AND testset_id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' GROUP BY RoomName,quiz_Id ORDER BY SUM(TotalScore) desc "
    '        Else
    '            sql = sql & " AND testset_id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND Calendar_Id = '" & CalendarId & "' GROUP BY RoomName,quiz_Id ORDER BY SUM(TotalScore) asc "
    '        End If
    '        Dim dt2 As New DataTable
    '        dt2 = _DB.getdata(sql)
    '        Dim EachQuizId As String = ""
    '        Dim EachRoomName As String = ""
    '        Dim ArrTop10Score As New ArrayList
    '        Dim ArrSubject As New ArrayList
    '        If dt2.Rows.Count > 0 Then 'วนเพื่อเก็บห้องที่ได้มากจากการ Select Top10 และ เก็บ Quiz_id
    '            HttpContext.Current.Session("dt2SortActivity") = dt2
    '            For z = 0 To dt2.Rows.Count - 1
    '                EachQuizId = dt2.Rows(z)("quiz_Id").ToString()
    '                EachRoomName = dt2.Rows(z)("RoomName").ToString()
    '                sql = " SELECT RoomName,SubjectName,TotalScore,Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                      " AND Quiz_Id = '" & EachQuizId & "' AND RoomName = '" & EachRoomName & "' " 'วน Select คะแนนจาก Quiz_id ที่ได้จากข้างบน
    '                dt.Clear()
    '                dt = _DB.getdata(sql)
    '                If dt.Rows.Count > 0 Then
    '                    For Each allRowdt In dt.Rows 'วนเพื่อ Add ข้อมูลลง dtComplete 
    '                        dtComplete.Rows.Add(allRowdt(0), allRowdt(1), allRowdt(2), allRowdt(3))
    '                    Next
    '                End If
    '                ArrTop10Score.Add(dt2.Rows(z)("RoomName") & "|" & dt2.Rows(z)("quiz_Id").ToString()) 'วน Add ห้องกับ Quiz_id ตามลำดับที่ได้จาก Top10 ลงใน Array
    '            Next
    '            For x = 0 To dtComplete.Rows.Count - 1
    '                If ArrSubject.Contains(dtComplete.Rows(x)("SubjectName")) = False Then
    '                    ArrSubject.Add(dtComplete.Rows(x)("SubjectName")) 'วนเพื่อเก็บวิชาทั้งหมดลง Array
    '                End If
    '            Next
    '            Dim SplitStr1 As String = ""
    '            Dim SplitStr2
    '            Dim GenStrSubjectName As String = ""
    '            Dim ObjdtRow() As DataRow
    '            For Each EachSubject In ArrSubject 'วนทีละวิชา
    '                Dim Scores As String = ""
    '                For p = 0 To ArrTop10Score.Count - 1
    '                    SplitStr1 = ArrTop10Score(p).ToString()
    '                    SplitStr2 = SplitStr1.Split("|") 'นำ Str ทีได้จาก Array มาแยกออกเพื่อให้ได้ห้อง กับ Quiz_id เพื่อเอามา Select ใน dtComplete
    '                    ObjdtRow = dtComplete.Select(" RoomName = '" & SplitStr2(0) & "' and Quiz_Id = '" & SplitStr2(1) & "' and SubjectName = '" & EachSubject & "' ") 'หาคะแนนจาก dtComplete โดยได้เงื่อนไขจากห้อง,ควิซ,วิชา 
    '                    If ObjdtRow.Count > 0 Then 'ถ้ามีคะแนนก็ให้เอาคะแนนมาต่อสตริง
    '                        Scores = Scores & ObjdtRow(0)(2) & ","
    '                    Else
    '                        Scores = Scores & "0," 'ถ้าไม่มีคะแนนให้คะแนนเป็น 0
    '                    End If
    '                Next
    '                If Scores.EndsWith(",") Then 'เอาตัวแปร Scores มาตัดลูกน้ำที่เกินออกทิ้ง
    '                    Scores = Scores.Substring(0, Scores.Length - 1)
    '                End If
    '                GenStrSubjectName = GenSubjectName(EachSubject)
    '                DataHash2.Add(GenStrSubjectName, Scores)
    '            Next
    '        End If
    '    End If

    '    If SortType = "0" Then 'ถ้าไม่ได้เป็น Top10 Low10 จะเข้าอันนี้เอาทุกครั้งที่ทำควิซ
    '        For i = 0 To ArrRoom.Count - 1 'ถ้าเป็นโหมดปกติ (SortType = 0) จะมาทำอันนี้โดยตรงเลย
    '            Dim RoomName As String = _DB.CleanString(ArrRoom(i).ToString().Trim())
    '            sql = " SELECT  RoomName,SubjectName,TotalScore,Quiz_Id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscoreFix WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                  " AND TestSet_Id = '" & _DB.CleanString(TestSet_ID.Trim()) & "' AND RoomName = '" & RoomName & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY LastUpdate "
    '            dt = _DB.getdata(sql)
    '            If dt.Rows.Count > 0 Then
    '                For Each objrow In dt.Rows
    '                    dtComplete.Rows.Add(objrow(0), objrow(1), objrow(2), objrow(3))
    '                Next
    '                'Else
    '                'Dim DummyDate As Date = Now
    '                'dtComplete.Rows.Add(RoomName, "กลุ่มสาระการเรียนรู้ภาษาไทย", 0, DummyDate, "")
    '            End If

    '        Next
    '        Dim ArrDate As New ArrayList
    '        Dim ArrRoomName As New ArrayList
    '        For z = 0 To dtComplete.Rows.Count - 1
    '            If DataHash.ContainsKey(dtComplete.Rows(z)("SubjectName")) = False Then 'วนเพื่อเก็บวิชาทั้งหมดลงใน Array
    '                DataHash.Add(dtComplete.Rows(z)("SubjectName"), "")
    '            End If

    '            If ArrDate.Contains(dtComplete.Rows(z)("Quiz_Id")) = False Then 'วนเพื่อเก็บ Quiz_Id ทั้งหมดลงใน Array
    '                ArrDate.Add(dtComplete.Rows(z)("Quiz_Id"))
    '            End If
    '            If ArrRoomName.Contains(dtComplete.Rows(z)("RoomName")) = False Then 'วนเพื่อเก็บห้องทั้งหมดลงใน Array
    '                ArrRoomName.Add(dtComplete.Rows(z)("RoomName"))
    '            End If
    '        Next
    '        'ArrRoomName.Add("1")
    '        'ArrRoomName.Add("2")
    '        'ArrRoomName.Add("3")
    '        'ArrRoomName.Add("4")
    '        'ArrRoomName.Add("5")

    '        For Each row In DataHash.Keys 'วนเพื่อทำ Hashtable เริ่มจากวิชาใหญ่สุด
    '            Dim Scores As String = ""
    '            Dim CurrentSubject = GenSubjectName(row)
    '            For Each EachRoomName In ArrRoomName 'แล้ววนห้องรองลงมา
    '                For j = 0 To ArrDate.Count - 1 'ให้ Quiz_id เป็นชั้นเล็กที่สุดเพื่อที่จะได้หาว่า วิชานี้ ห้องนี้ ควิซนี้ มีคะแนนหรือเปล่า
    '                    Dim dtrowCheckDate() As DataRow = dtComplete.Select("Quiz_Id = '" & ArrDate(j) & "' and RoomName = '" & EachRoomName & "' ")
    '                    If dtrowCheckDate.Length > 0 Then
    '                        Dim CurrentDate As String = ArrDate(j)
    '                        Dim dtrow() As DataRow = dtComplete.Select("Quiz_Id = '" & CurrentDate & "' and SubjectName = '" & row & "' and RoomName = '" & EachRoomName & "' ")
    '                        If dtrow.Length > 0 Then
    '                            Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                        Else
    '                            Scores = Scores & "0,"
    '                        End If
    '                    End If
    '                Next
    '            Next
    '            If Scores.EndsWith(",") Then
    '                Scores = Scores.Substring(0, Scores.Length - 1)
    '            End If
    '            DataHash2.Add(CurrentSubject, Scores)
    '        Next
    '    End If
    '    HttpContext.Current.Session("dtChartActivity") = dtComplete


    '    Return DataHash2

    'End Function

    'Public Function QueryPieDtActivity(ByVal SchoolId As String, ByVal ArrClass As ArrayList, ByVal ViewName As String, ByVal SelectFieldName As String, ByVal WhereFieldName As String, ByVal SortType As String, ByVal TestSetID As String, ByVal ChooseMode As Integer)

    '    Dim dt As New DataTable
    '    Dim dtComplete As New DataTable
    '    If SchoolId Is Nothing Or SchoolId = "" Or ArrClass Is Nothing Or ArrClass.Count = 0 Or SelectFieldName Is Nothing Or SelectFieldName = "" Or ViewName Is Nothing Or ViewName = "" Or SortType Is Nothing Or SortType = "" Or ChooseMode = 0 Then
    '        Return dt
    '    End If
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return dt
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As New StringBuilder
    '    Dim SpareStr As String = ""
    '    sql.Append("SELECT ")
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าไป
    '        sql.Append(" Top 10 ")
    '    End If
    '    If ChooseMode = 1 Then
    '        sql.Append(SelectFieldName & " ,Quiz_Id ")
    '    Else 'ถ้าเป็นแบบฝึกฝนไม่ต้อง Select Quiz_Id
    '        sql.Append(SelectFieldName)
    '    End If
    '    If SortType = "0" Then 'ถ้าเป็นโหมดปกติ ถึงจะ Select เพิ่ม 2 Field
    '        If ChooseMode = 1 Then
    '            sql.Append(" ,SubjectName,TotalScore ")
    '        Else
    '            sql.Append(" ,SubjectName,SUM(TotalScore)as TotalScore ")
    '        End If
    '    End If
    '    sql.Append(" FROM dbo.uvw_Chart_" & ViewName & " WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' AND Testset_Id = '" & _DB.CleanString(TestSetID) & "' AND " & StrWhere & " = '1' AND Calendar_Id = '" & CalendarId & "' AND ( ")
    '    If ArrClass.Count > 1 Then
    '        For i = 0 To ArrClass.Count - 1
    '            Dim CurrentClass As String = _DB.CleanString(ArrClass(i).ToString().Trim())
    '            sql.Append(" " & WhereFieldName & " = '" & CurrentClass & "' Or ")
    '        Next
    '        SpareStr = sql.ToString().Substring(0, sql.ToString().Length - 3)
    '        sql.Clear()
    '        sql.Append(SpareStr & " )")
    '    Else
    '        sql.Append(" " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' ) ")
    '    End If
    '    If SortType = "0" Then
    '        If ChooseMode = 1 Then
    '            sql.Append(" ORDER BY LastUpdate ")
    '        Else 'ถ้าเป็นโหมดฝึกฝนต้อง Group By เพราะว่า Select Sum คะแนนมา
    '            sql.Append(" GROUP BY " & SelectFieldName & ",SubjectName ")
    '        End If
    '        dt = _DB.getdata(sql.ToString())
    '        If ChooseMode = 1 Then 'ถ้าไม่ใช่โหมดฝึกฝนต้องเอา Datatable ไปหาครั้งที่ควิซต่อ
    '            dtComplete = CreateStrQuizTimeForDtActivity(dt, False, False, TestSetID, SchoolId)
    '        Else 'ถ้าเป็นโหมดฝึกฝน Return Datatable กลับไปเลย
    '            dtComplete = dt
    '        End If
    '    End If
    '    If SortType = "1" Or SortType = "2" Then 'ถ้าเป็นโหมด Top10 Low10 เติม Top10 เข้าเงื่อนไขนี้
    '        dt.Columns.Add(SelectFieldName)
    '        If ChooseMode = 1 Then 'ถ้าไม่ได้เป็นโหมดฝึกฝนถึงจะมี Column Quiz_Id
    '            dt.Columns.Add("Quiz_ID")
    '        End If
    '        dt.Columns.Add("SubjectName")
    '        dt.Columns.Add("TotalScore")
    '        If ChooseMode = 1 Then 'ถ้าไม่ได้เป็นโหมดฝึกฝน Group By QuizId 
    '            sql.Append(" GROUP BY " & SelectFieldName & " ,Quiz_Id ORDER BY SUM(TotalScore) ")
    '        Else 'ถ้าเป็นโหมดฝึกฝน Group By SubjectName
    '            sql.Append(" GROUP BY " & SelectFieldName & " ,SubjectName ORDER BY SUM(TotalScore) ")
    '        End If

    '        If SortType = "1" Then
    '            sql.Append(" desc ")
    '        Else
    '            sql.Append(" asc ")
    '        End If
    '        Dim dt2 As New DataTable
    '        dt2 = _DB.getdata(sql.ToString())
    '        If dt2.Rows.Count > 0 Then
    '            For index = 0 To dt2.Rows.Count - 1
    '                If ChooseMode = 1 Then 'ถ้าไมได้เป็นโหมดฝึกฝนเข้าเงื่อนไขนี้
    '                    Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
    '                    Dim QuizId As String = dt2.Rows(index)("Quiz_Id").ToString()
    '                    Dim sqlStr As String = " SELECT " & SelectFieldName & " ,Quiz_Id,SubjectName,TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore " & _
    '                                     " WHERE Quiz_Id = '" & QuizId & "' and RoomName = '" & ClassOrRoomName & "' "
    '                    'If WhereFieldIsMorethanOne = True Then
    '                    '    sqlStr = sqlStr & " AND " & WhereFieldName & " = '" & _DB.CleanString(ArrClass(0).ToString().Trim()) & "' "
    '                    'End If
    '                    Dim dt3 As New DataTable
    '                    dt3 = _DB.getdata(sqlStr)
    '                    If dt3.Rows.Count > 0 Then
    '                        For Each allrow In dt3.Rows
    '                            dt.Rows.Add(allrow(0), allrow(1), allrow(2), allrow(3))
    '                        Next
    '                    End If
    '                Else 'ถ้าเป็นโหมดฝึกฝนเข้า Else
    '                    Dim ClassOrRoomName As String = dt2.Rows(index)(SelectFieldName).ToString().Trim()
    '                    Dim sqlStr As String = " SELECT " & SelectFieldName & " ,SubjectName,SUM(TotalScore) as TotalScore FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore " & _
    '                                     " WHERE RoomName = '" & ClassOrRoomName & "' GROUP BY " & SelectFieldName & ",SubjectName "
    '                    Dim dt3 As New DataTable
    '                    dt3 = _DB.getdata(sqlStr)
    '                    If dt3.Rows.Count > 0 Then
    '                        For Each allrow In dt3.Rows
    '                            dt.Rows.Add(allrow(0), allrow(1), allrow(2))
    '                        Next
    '                    End If
    '                End If

    '            Next
    '            If ChooseMode = 1 Then 'ถ้าเป็นโหมดปกติต้องเข้าไปทำ dt ใหม่
    '                dtComplete = CreateStrQuizTimeForDtActivity(dt, False, True, TestSetID, SchoolId)
    '            Else
    '                dtComplete = dt
    '            End If
    '        Else
    '            Return dtComplete
    '        End If
    '        If ChooseMode = 1 Then 'แยกเก็บ Session คนละตัว แยกคนละโหมด แบบฝึกฝน กับไม่มีฝึกฝน
    '            HttpContext.Current.Session("dtCompleteTopLowActivity") = dtComplete
    '        Else
    '            HttpContext.Current.Session("dtCompleteTopLow") = dtComplete
    '        End If

    '    End If

    '    Return dtComplete

    'End Function 'ทำ dt วงกลม 5

    'Public Function HashChartActivityPerQuiz(ByVal SchoolId As String, ByVal TestSet_Id As String, ByVal RoomName As String, ByVal QuizTime As String, ByVal SortType As String, ByVal ChooseMode As Integer) 'หาคะแนนเฉลี่่ย ของชุด,ครั้งที่,ห้อง

    '    Dim DataHash As New Hashtable
    '    Dim DataHash2 As New Hashtable
    '    Dim dt As New DataTable
    '    Dim dtcomplete As New DataTable
    '    dtcomplete.Columns.Add("NoInRoom")
    '    dtcomplete.Columns.Add("SubjectName")
    '    dtcomplete.Columns.Add("TotalScore")
    '    Dim CalendarId As String = KNSession("SelectedCalendarId").ToString()
    '    If CalendarId = "" Then
    '        Return dt
    '    End If
    '    Dim StrWhere As String = ""
    '    If ChooseMode = 1 Then
    '        StrWhere = "IsQuizMode"
    '    ElseIf ChooseMode = 2 Then
    '        StrWhere = "IsHomeWorkMode"
    '    ElseIf ChooseMode = 3 Then
    '        StrWhere = "IsPracticeMode"
    '    End If
    '    Dim sql As String = ""
    '    Dim QuizId As String = ""
    '    If ChooseMode <> 3 Then
    '        sql = " SELECT  quiz_id FROM dbo.uvw_Chart_RoomTestsetSubjectTotalscore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " &
    '                        " AND RoomName = '" & _DB.CleanString(RoomName.Trim()) & "' AND testset_id = '" & TestSet_Id & "' " & _
    '                        " AND Calendar_Id = '" & CalendarId & "' GROUP BY quiz_id,LastUpdate ORDER BY LastUpdate"
    '        dt = _DB.getdata(sql)
    '    Else
    '        Dim SplitStr = RoomName.Split("/")
    '        Dim ClassName As String = SplitStr(0)
    '        Dim QuizRoomName As String = "/" & SplitStr(1)
    '        QuizId = GetLastQuizIdPracticeMode(TestSet_Id, ClassName, QuizRoomName, SchoolId, CalendarId)
    '        If QuizId = "" Then
    '            Return DataHash2
    '        End If
    '    End If

    '    If dt.Rows.Count > 0 Or QuizId <> "" Then
    '        If ChooseMode <> 3 Then
    '            Dim QuizAtTime As Integer = Integer.Parse(QuizTime) - 1
    '            QuizId = dt.Rows(QuizAtTime)("Quiz_Id").ToString()
    '        End If
    '        HttpContext.Current.Session("ExclusiveQuizId") = QuizId
    '        'คิวรี่เพื่อหาคะแนนรวมของนักเรียนแต่ละคนในควิซครั้งนี้ 
    '        If SortType = "0" Then 'ถ้าไม่ได้เลือกโหมด Top10 Low10 ใช้คิวรี่อันนี้
    '            sql = " SELECT NoInRoom,SubjectName,TotalScore FROM dbo.uvw_Chart_StudentTestSetSubjectTotalScore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '             " AND RoomName = '" & _DB.CleanString(RoomName.Trim()) & "' AND Quiz_Id = '" & QuizId & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY SubjectName,NoInRoom "
    '            dtcomplete = _DB.getdata(sql)
    '        ElseIf SortType = "1" Or SortType = "2" Then 'ถ้าเลือก Top10 Low10 ใช้คิวรี่อันนี้
    '            sql = " SELECT TOP 10 NoInRoom FROM dbo.uvw_Chart_StudentTestSetSubjectTotalScore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                  " AND RoomName = '" & _DB.CleanString(RoomName.Trim()) & "' AND Quiz_Id =  '" & QuizId & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' GROUP BY NoInRoom ORDER BY SUM(TotalScore) "
    '            If SortType = "1" Then 'ถ้าเป็น Top10 ใช้ DESC
    '                sql = sql & " desc "
    '            Else 'ถ้าเป็น Low10 ใช้ ASC
    '                sql = sql & " asc "
    '            End If
    '            dt.Clear()
    '            dt = _DB.getdata(sql)
    '            Dim dt2 As New DataTable
    '            If dt.Rows.Count > 0 Then
    '                For z = 0 To dt.Rows.Count - 1
    '                    Dim EachNoInRoom As String = dt.Rows(z)("NoInRoom").ToString()
    '                    sql = " SELECT NoInRoom,SubjectName,TotalScore FROM dbo.uvw_Chart_StudentTestSetSubjectTotalScore WHERE SchoolCode = '" & _DB.CleanString(SchoolId.Trim()) & "' " & _
    '                          " AND NoInRoom = '" & EachNoInRoom & "' AND Quiz_Id =  '" & QuizId & "' AND Calendar_Id = '" & CalendarId & "' AND " & StrWhere & " = '1' ORDER BY SubjectName "
    '                    dt2 = _DB.getdata(sql)
    '                    If dt2.Rows.Count > 0 Then
    '                        For Each objrow In dt2.Rows
    '                            dtcomplete.Rows.Add(objrow(0), objrow(1), objrow(2))
    '                        Next
    '                    Else
    '                        dtcomplete.Rows.Add(EachNoInRoom, "กลุ่มสาระการเรียนรู้ภาษาไทย", "0")
    '                    End If
    '                Next
    '            Else
    '                Return DataHash2
    '            End If
    '        End If

    '        If dtcomplete.Rows.Count > 0 Then
    '            Dim ArrNoInRoom As New ArrayList
    '            Dim ArrCategories As New ArrayList
    '            For i = 0 To dtcomplete.Rows.Count - 1
    '                If DataHash.ContainsKey(dtcomplete.Rows(i)("SubjectName")) = False Then
    '                    DataHash.Add(dtcomplete.Rows(i)("SubjectName"), "")
    '                End If

    '                If ArrNoInRoom.Contains(dtcomplete.Rows(i)("NoInRoom")) = False Then
    '                    ArrNoInRoom.Add(dtcomplete.Rows(i)("NoInRoom"))
    '                    ArrCategories.Add("เลขที่" & dtcomplete.Rows(i)("NoInRoom"))
    '                End If
    '            Next
    '            HttpContext.Current.Session("ArrCatActivityPerQuiz") = ArrCategories
    '            For Each row In DataHash.Keys
    '                Dim Scores As String = ""
    '                Dim CurrentSubject = GenSubjectName(row)
    '                For j = 0 To ArrNoInRoom.Count - 1
    '                    Dim CurrentNoinRoom As String = ArrNoInRoom(j)
    '                    Dim dtrow() As DataRow = dtcomplete.Select("NoInRoom = '" & CurrentNoinRoom & "' and SubjectName = '" & row & "' ")
    '                    If dtrow.Length > 0 Then
    '                        Scores = Scores & dtrow(0)("TotalScore").ToString() & ","
    '                    Else
    '                        Scores = Scores & "0,"
    '                    End If
    '                Next
    '                If Scores.EndsWith(",") Then
    '                    Scores = Scores.Substring(0, Scores.Length - 1)
    '                End If
    '                DataHash2.Add(CurrentSubject, Scores)
    '            Next
    '        Else
    '            Return DataHash2
    '        End If
    '    Else
    '        Return DataHash2
    '    End If
    '    Return DataHash2

    'End Function '****6**** 

#End Region
 
End Class
