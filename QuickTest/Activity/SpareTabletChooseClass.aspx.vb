Imports System.Web
Public Class SpareTabletChooseClass
    Inherits System.Web.UI.Page

    Dim _DB As New ClassConnectSql

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim DVID As String = ""
        'If Request.QueryString("DeviceUniqueID") IsNot Nothing Then
        '    DVID = Request.QueryString("DeviceUniqueID").ToString()
        'Else
        '    Exit Sub
        'End If
        'ใช้จริงต้องปลด Comment เพื่อรับ DeviceId ******************************************

        'หา TabletId เพื่อเก็บไว้ใน Session ส่งไปให้หน้าเลือกที่นั่ง
        Dim TabletId As String = GetTabletIdByDeviceId("RealTest15") 'ใช้จริงต้องใส่ DVID แทน **********************************
        If TabletId = "" Then
            Exit Sub
        End If

        HttpContext.Current.Session("SpareTablet_Id") = TabletId

        CreateStringFortag("RealTest15") 'ใช้จริงต้องใส่ DVID แทน **********************************


    End Sub

    Private Sub CreateStringFortag(ByVal DeviceId As String) 'Function สร้าง Div ชั้นเรียนและห้อง soundlab ที่กำลังเล่นอยู่เพื่อให้กดเลือกไปเปลี่ยนเครื่องสำรอง

        If DeviceId Is Nothing Or DeviceId = "" Then
            Exit Sub
        End If

        Dim sql As String = ""
        Dim sb As New StringBuilder
        Dim dt As New DataTable
        Dim SchoolId As String = ""
        sql = " SELECT TOP 1  School_Code FROM dbo.uvw_GetQuizIdAndPlayerIdBySerialNumber WHERE tablet_serialnumber = '" & _DB.CleanString(DeviceId) & "' "

        SchoolId = _DB.ExecuteScalar(sql)
        If SchoolId <> "" Then
            Dim EachClass As String = ""
            'เริ่มสร้างจาก DivClass ก่อน
            sql = " SELECT DISTINCT  tblQuiz.t360_ClassName,tblQuiz.t360_RoomName " &
                  " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id " &
                  " WHERE (tblQuiz.t360_SchoolCode = '" & SchoolId & "') AND dbo.tblQuizScore.LastUpdate > DATEADD(MINUTE,-15,dbo.GetThaiDate()) AND dbo.tblQuiz.EndTime IS NULL "
            'dt = _DB.getdata(sql)
            dt = Testdt()
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    EachClass = dt.Rows(i)("t360_ClassName") & dt.Rows(i)("t360_RoomName")
                    sb.Append("<tr><td class='ForHover' id='" & EachClass & "' onclick=""ClickChooseLevel('" & EachClass & "','" & SchoolId & "','" & False & "')"" >" & EachClass & "</td></tr>")
                Next
            Else
                Exit Sub
            End If
            tagClass.InnerHtml = sb.ToString()
            sb.Clear()
            dt.Clear()
            'สร้าง DivSoundLab ต่อ
            sql = " SELECT DISTINCT tblTabletLab.TabletLab_Name " &
                  " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " &
                  " tblTabletLab ON tblQuiz.TabletLab_Id = tblTabletLab.TabletLab_Id " &
                  " WHERE (tblTabletLab.IsActive = 1) AND (tblQuiz.TabletLab_Id IS NOT NULL) AND (dbo.tblQuiz.t360_SchoolCode = '" & SchoolId & "') " &
                  " AND dbo.tblQuizScore.LastUpdate > DATEADD(MINUTE,-15,dbo.GetThaiDate()) "
            'dt = _DB.getdata(sql)
            dt = TestdtSL()
            If dt.Rows.Count > 0 Then
                For a = 0 To dt.Rows.Count - 1
                    EachClass = dt.Rows(a)("TabletLab_Name")
                    sb.Append("<tr><td class='ForHover' id='" & EachClass & "' onclick=""ClickChooseLevel('" & EachClass & "','" & SchoolId & "','" & True & "')"" >" & EachClass & "</td></tr>")
                Next
            Else
                Exit Sub
            End If
            tagSoundlab.InnerHtml = sb.ToString()
        Else
            Exit Sub
        End If

    End Sub

    <Services.WebMethod()>
    Public Shared Function ClickChooseLevelCB(ByVal SpareLevelName As String, ByVal SpareSchoolCode As String, ByVal SpareUseTablab As Boolean)

        If SpareLevelName Is Nothing Or SpareLevelName = "" Or SpareSchoolCode Is Nothing Or SpareSchoolCode = "" Then
            Return "Error"
        End If

        'Set ค่าให้ Session ต่างๆเพื่อเอาไปให้หน้าเลือกที่นั่งเพื่อเปลี่ยนเครื่องสำรองใช้
        HttpContext.Current.Session("SpareLevelName") = SpareLevelName 'Set ห้องที่ถูกเลือกให้กับ Session
        HttpContext.Current.Session("SpareSchoolCode") = SpareSchoolCode 'Set รหัสโรงเรียนที่ใช้งานให้กับ Session
        HttpContext.Current.Session("SpareUseLab") = SpareUseTablab 'Set ว่าเลือกห้อง SoundLab หรือเปล่าให้กับ Session
        Return "Complete"

    End Function

    Private Function GetTabletIdByDeviceId(ByVal DeviceId As String) 'Function หา Tablet_id จาก DeviceId

        Dim TabletId As String = ""
        If DeviceId Is Nothing Or DeviceId = "" Then
            Return TabletId
        End If

        Dim sql As String = " SELECT Tablet_Id FROM dbo.t360_tblTablet WHERE Tablet_SerialNumber = '" & _DB.CleanString(DeviceId) & "' "
        TabletId = _DB.ExecuteScalar(sql)
        Return TabletId

    End Function



    Public Function Testdt()

        Dim dt As New DataTable
        dt.Columns.Add("t360_ClassName")
        dt.Columns.Add("t360_RoomName")

        dt.Rows.Add("ม.1", "/1")
        dt.Rows.Add("ม.1", "/3")
        dt.Rows.Add("ม.1", "/7")
        dt.Rows.Add("ม.2", "/1")
        dt.Rows.Add("ม.2", "/3")
        dt.Rows.Add("ม.4", "/2")
        dt.Rows.Add("ม.5", "/1")
        dt.Rows.Add("ม.6", "/3")
        dt.Rows.Add("ม.6", "/2")



        Return dt

    End Function

    Public Function TestdtSL()

        Dim dt As New DataTable
        dt.Columns.Add("TabletLab_Name")
        dt.Rows.Add("SoundLab ทานตะวัน")
        dt.Rows.Add("SoundLab กุหลาบ")
        dt.Rows.Add("SoundLab ชินจัง")

        Return dt
    End Function


End Class