Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Public Class MasterSchoolReport
    Inherits Telerik.Reporting.Report

    Public Sub New(ByVal ArrData As ArrayList, ByVal dt As DataTable, ByVal dtForCalendar As DataTable, ByVal MaxValue As Integer, ByVal IndexPage As String)

        InitializeComponent()
        Filltxt(ArrData)
        FillContent(dt)
        SetBottomCalendar(dtForCalendar, MaxValue)
        txtIndexPage.Value = IndexPage

    End Sub

    'เติมข้อมูลโรงเรียน ด้านบนของรายงาน
    Private Sub Filltxt(ByVal ArrData As ArrayList)
        If ArrData.Count > 0 Then
            Dim SchoolName As String = ArrData(0)
            Dim SchoolAddress As String = ArrData(1)
            Dim Phone As String = ArrData(2)
            Dim Fax As String = ArrData(3)
            Dim CurrentMonth As String = GetMonthName(ArrData(4))
            Dim CurrentYear As String = ArrData(5)
            If CurrentYear < 2400 Then
                CurrentYear += 543
            End If

            txtSchoolName.Value = SchoolName
            txtAddress.Value = SchoolAddress
            txtTelephone.Value = "โทร : " & Phone
            txtFax.Value = "โทรสาร : " & Fax
            txtHeader.Value = "บทวิเคราะห์ประจำเดือน " & CurrentMonth & " " & CurrentYear
        End If
    End Sub

    'เติมข้อมูลเนื้อหาที่ส่วน Content ของรายงาน
    Private Sub FillContent(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            txtTotalSchoolActive.Value = "เวลาใช้รวม " & CInt((dt.Rows(0)("TotalHour") / 60)).ToString() & " ชม. "
            txtTotalSchoolActive2.Value = dt.Rows(0)("TotalSchoolActive").ToString()
            txtTotalStudentActive.Value = "นักเรียนเข้าใช้งาน"
            txtTotalStudentActive2.Value = dt.Rows(0)("TotalStudentActive").ToString()
            txtTotalDeviceDetail.Value = "อุปกรณ์ไอที สมบูรณ์ " & dt.Rows(0)("TotalDevicePerfect").ToString() & "% "
            txtTotalDeviceDetail2.Value = "ส่งซ่อม/เสีย " & dt.Rows(0)("TotalDeviceBroken") & "%"
        End If

    End Sub

    Private Sub SetBottomCalendar(ByVal dt As DataTable, ByVal MaxValue As Integer)

        If dt.Rows.Count > 0 Then
            Dim TotalDayInMonth As Integer = GetTotalDayInMonth(dt.Rows(0)("Month"))
            If TotalDayInMonth < 31 Then txtDay31.Visible = False
            If TotalDayInMonth < 30 Then txtDay31.Visible = False

            Dim ValueCheck As Integer = 0
            For index = 1 To TotalDayInMonth
                ValueCheck = CheckDayIsHaveData(index, dt)
                FillColorInCalendarBottom(index, ValueCheck, MaxValue)
            Next
        End If

    End Sub

    'Function หาว่าเดือนที่ใส่เข้ามามีกี่วัน
    Private Function GetTotalDayInMonth(ByVal InputMonth As Integer) As Integer
        Dim TotalDay As Integer = 0
        If InputMonth = 2 Then
            'ต้องหาว่าปีนั้นเดือนกุมภามีกี่วัน
            Dim CurrentYear As Integer = DateTime.Now.Year
            If CurrentYear < 2500 Then
                CurrentYear += 543
            End If
            If DateTime.IsLeapYear(CurrentYear) = True Then
                TotalDay = 29
            Else
                TotalDay = 28
            End If
        ElseIf InputMonth = 4 Or InputMonth = 6 Or InputMonth = 9 Or InputMonth = 11 Then
            TotalDay = 30
        Else
            TotalDay = 31
        End If
        Return TotalDay
    End Function

    'Function เช็คว่าวันที่ส่งเช้ามามีค่าหรือเปล่า
    Private Function CheckDayIsHaveData(ByVal IndexCheck As Integer, ByVal dt As DataTable) As Integer
        Dim ValueOfDay As Integer = 0
        Dim dtRow() As DataRow
        dtRow = dt.Select("Day = '" & IndexCheck & "' ")
        If dtRow.Length > 0 Then
            ValueOfDay = dtRow(0)("TotalMinute")
        Else
            ValueOfDay = 0
        End If
        Return ValueOfDay
    End Function

    'Function หาชื่อเดือนจาก เลขเดือน
    Private Function GetMonthName(ByVal MonthNumber As String)

        Dim MonthName As String = ""
        Select Case MonthNumber
            Case "1"
                MonthName = "มกราคม"
            Case "2"
                MonthName = "กุมภาพันธ์"
            Case "3"
                MonthName = "มีนาคม"
            Case "4"
                MonthName = "เมษายน"
            Case "5"
                MonthName = "พฤษภาคม"
            Case "6"
                MonthName = "มิถุนายน"
            Case "7"
                MonthName = "กรกฏาคม"
            Case "8"
                MonthName = "สิงหาคม"
            Case "9"
                MonthName = "กันยายน"
            Case "10"
                MonthName = "ตุลาคม"
            Case "11"
                MonthName = "พฤศจิกายน"
            Case "12"
                MonthName = "ธันวาคม"
        End Select
        Return MonthName

    End Function

    Private Sub FillColorInCalendarBottom(ByVal IndexDay As Integer, ByVal ValueOfDay As Integer, ByVal MaxValue As Integer)

        '135, 248, 236 Level 1
        '0, 210, 255 Level 2
        '5, 158, 250 Level 3
        '2, 28, 253 Level 4

        Select Case IndexDay
            Case 1
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay1.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay1.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay1.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay1.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay1.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 2
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay2.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay2.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay2.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay2.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay2.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 3
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay3.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay3.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay3.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay3.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay3.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 4
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay4.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay4.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay4.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay4.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay4.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 5
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay5.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay5.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay5.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay5.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay5.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 6
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay6.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay6.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay6.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay6.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay6.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 7
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay7.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay7.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay7.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay7.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay7.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 8
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay8.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay8.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay8.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay8.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay8.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 9
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay9.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay9.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay9.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay9.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay9.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 10
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay10.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay10.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay10.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay10.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay10.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 11
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay11.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay11.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay11.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay11.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay11.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 12
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay12.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay12.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay12.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay12.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay12.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 13
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay13.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay13.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay13.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay13.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay13.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 14
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay14.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay14.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay14.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay14.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay14.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 15
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay15.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay15.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay15.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay15.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay15.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 16
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay16.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay16.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay16.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay16.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay16.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 17
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay17.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay17.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay17.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay17.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay17.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 18
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay18.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay18.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay18.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay18.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay18.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 19
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay19.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay19.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay19.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay19.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay19.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 20
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay20.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay20.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay20.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay20.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay20.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 21
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay21.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay21.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay21.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay21.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay21.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 22
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay22.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay22.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay22.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay22.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay22.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 23
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay23.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay23.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay23.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay23.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay23.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 24
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay24.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay24.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay24.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay24.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay24.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 25
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay25.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay25.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay25.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay25.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay25.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 26
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay26.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay26.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay26.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay26.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay26.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 27
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay27.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay27.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay27.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay27.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay27.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 28
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay28.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay28.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay28.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay28.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay28.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 29
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay29.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay29.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay29.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay29.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay29.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 30
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay30.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay30.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay30.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay30.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay30.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
            Case 31
                If CheckColorCalendar(ValueOfDay, MaxValue) = 0 Then
                    txtDay31.Style.BackgroundColor = Color.White
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 1 Then
                    txtDay31.Style.BackgroundColor = System.Drawing.Color.FromArgb(135, 248, 236)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 2 Then
                    txtDay31.Style.BackgroundColor = System.Drawing.Color.FromArgb(0, 210, 255)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 3 Then
                    txtDay31.Style.BackgroundColor = System.Drawing.Color.FromArgb(5, 158, 250)
                ElseIf CheckColorCalendar(ValueOfDay, MaxValue) = 4 Then
                    txtDay31.Style.BackgroundColor = System.Drawing.Color.FromArgb(2, 28, 253)
                End If
        End Select

    End Sub

    Private Function CheckColorCalendar(ByVal ValueOfDay As Integer, ByVal MaxValue As Integer) As Integer

        If ValueOfDay = 0 Then
            Return 0
        End If

        If (ValueOfDay / MaxValue) * 100 >= 1 And (ValueOfDay / MaxValue) * 100 <= 20 Then
            Return 1
        ElseIf (ValueOfDay / MaxValue) * 100 >= 21 And (ValueOfDay / MaxValue) * 100 <= 50 Then
            Return 2
        ElseIf (ValueOfDay / MaxValue) * 100 >= 51 And (ValueOfDay / MaxValue) * 100 <= 70 Then
            Return 3
        ElseIf (ValueOfDay / MaxValue) * 100 >= 71 And (ValueOfDay / MaxValue) * 100 <= 100 Then
            Return 4
        Else
            Return 0
        End If

    End Function



End Class
 