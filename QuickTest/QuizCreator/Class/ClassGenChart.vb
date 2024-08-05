Public Class ClassGenChart

    Public Function GenStrPieChart(ByVal RenderTo As String, ByVal Title As String, ByVal Name As String, ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim StrPiechart As String = "var chart = new Highcharts.Chart({chart: {renderTo: '" & RenderTo & "',plotBackgroundColor: null,plotBorderWidth: null,plotShadow: false},title: {text: '<b>" & Title & "</b>'},tooltip: {pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'},plotOptions: {pie: {allowPointSelect: true,cursor: 'pointer',dataLabels: {enabled: false,},showInLegend: true}},series: [{type: 'pie',name: '" & Name & "',data: ["
            For index = 0 To dt.Rows.Count - 1
                If index = 0 Then
                    StrPiechart &= "{name: '" & dt.Rows(index)("Testset_Name") & "',y: " & dt.Rows(index)("TotalUse") & ",sliced: true,selected: true},"
                Else
                    StrPiechart &= "['" & dt.Rows(index)("Testset_Name") & "', " & dt.Rows(index)("TotalUse") & "],"
                End If
            Next
            If StrPiechart.EndsWith(",") = True Then
                StrPiechart = StrPiechart.Substring(0, StrPiechart.Length - 1)
            End If
            StrPiechart &= "]}]});"
            Return StrPiechart
        Else
            Return ""
        End If
    End Function

    Public Function GenStrLineChart(ByVal RenderTo As String, ByVal Title As String, ByVal dt As DataTable, ByVal YTitle As String)
        If dt.Rows.Count > 0 Then
            Dim lblStep As Integer = 0
            If dt.Rows.Count > 30 Then
                lblStep = 10
            ElseIf dt.Rows.Count > 60 Then
                lblStep = 20
            ElseIf dt.Rows.Count > 120 Then
                lblStep = 30
            End If
            Dim StrLineChart As String = "var chart = new Highcharts.Chart({ chart: { renderTo: '" & RenderTo & "', }, title: { text: '" & Title & "', x: -20 }, xAxis: { categories: ["

            For index = 0 To dt.Rows.Count - 1
                StrLineChart &= "'" & dt.Rows(index)("QCL_TimeStamp").ToString() & "',"
            Next
            If StrLineChart.EndsWith(",") = True Then
                StrLineChart = StrLineChart.Substring(0, StrLineChart.Length - 1)
            End If
            StrLineChart &= "], labels: { step: " & lblStep & " } }, yAxis: { title: { text: '" & YTitle & "' }, plotLines: [{ value: 0, width: 1, color: '#808080' }] }, tooltip: { valueSuffix: ' คน' }, legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle', borderWidth: 0 }, series: [{ name: 'จำนวนคนที่เข้าทำ', data: ["

            For z = 0 To dt.Rows.Count - 1
                StrLineChart &= "" & dt.Rows(z)("TotalUse") & ","
            Next
            If StrLineChart.EndsWith(",") = True Then
                StrLineChart = StrLineChart.Substring(0, StrLineChart.Length - 1)
            End If
            StrLineChart &= "] }] });"
            Return StrLineChart
        Else
            Return ""
        End If
    End Function

    Public Function GenStrVerticalBarChart(ByVal RenderTo As String, ByVal Title As String, ByVal TotalUse As Integer, ByVal TotalDownload As Integer)
        Dim StrVerticalBarchart As String = "var chart = new Highcharts.Chart({chart: {renderTo: '" & RenderTo & "',type: 'column'},title: {text: '" & Title & "'},xAxis: {categories: ['ดาวน์โหลด','เข้าทำ']},yAxis: {min: 0,title: {text: 'คน'}},plotOptions: {column: {pointPadding: 0.2,borderWidth: 0}},series: [{name: 'ปริมาณการดาวน์โหลด/เข้าทำ',data: [" & TotalDownload & ", " & TotalUse & "]}]});"
        Return StrVerticalBarchart
    End Function

    Public Function GenStrHorizontalBarChart(ByVal RenderTo As String, ByVal Title As String, ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim StrHorizontalchart As String = "var chart = new Highcharts.Chart({chart: {renderTo: '" & RenderTo & "',type: 'bar'},title: {text: '" & Title & "'},xAxis: {categories: ["

            For index = 0 To dt.Rows.Count - 1
                StrHorizontalchart &= "'" & dt.Rows(index)("QNo") & "',"
            Next
            If StrHorizontalchart.EndsWith(",") = True Then
                StrHorizontalchart = StrHorizontalchart.Substring(0, StrHorizontalchart.Length - 1)
            End If
            StrHorizontalchart &= "],title: {text: 'ข้อที่'}},yAxis: {min: 0,title: {text: 'จำนวนครั้งที่ตอบถูก',align: 'high'},labels: {overflow: 'justify'}},credits: {enabled: false},series: [{name: 'ข้อสอบทั้งหมด',data: ["
            For z = 0 To dt.Rows.Count - 1
                StrHorizontalchart &= "" & dt.Rows(z)("TotalCorrect") & ","
            Next
            If StrHorizontalchart.EndsWith(",") = True Then
                StrHorizontalchart = StrHorizontalchart.Substring(0, StrHorizontalchart.Length - 1)
            End If
            StrHorizontalchart &= "]}]});"
            Return StrHorizontalchart
        Else
            Return ""
        End If
    End Function

    Public Function GenStrCompleteDashboard(ByVal ArrStr As ArrayList)
        Dim CompleteStr As String = ""
        Dim StrJavaScriptOpen As String = "<script type='text/javascript'>$(function (){"
        Dim StrJavaScriptClose As String = "});</script>"
        CompleteStr = StrJavaScriptOpen
        If ArrStr.Count > 0 Then
            For index = 0 To ArrStr.Count - 1
                CompleteStr &= ArrStr(index)
            Next
        End If
        CompleteStr &= StrJavaScriptClose
        'CompleteStr = StrJavaScriptOpen & StrTopUsage & StrTopDownload & StrTopRating & StrLineChart & StrJavaScriptClose
        Return CompleteStr
    End Function



End Class
