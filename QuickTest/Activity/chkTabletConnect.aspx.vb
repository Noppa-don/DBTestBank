Imports System.Web.Script.Serialization
Imports KnowledgeUtils
Imports System.Web

Public Class chkTabletConnect
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session("Quiz_Id") = "1a564a37-f085-4509-bc03-2933f1c8d535"
        'Session("SchoolID") = 1000001
        'Session("TotalStudent") = "15"
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        End If


    End Sub
    <Services.WebMethod()>
    Public Shared Function getDdl(idStudent As String, Type As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim db As New ClassConnectSql()
        Dim ClsActivity As New ClsActivity(New ClassConnectSql())
        Dim sql As String = ""
        Dim detail As New List(Of String)()
        Dim DefaultTabletStatus As String
        Dim ChangeTabletStatus As String
        Dim DisableStatus As Boolean
        Dim StudentNo As String = ""
        If HttpContext.Current.Session("IsSoundLab") = True Then

                sql = " SELECT  sd.Student_CurrentNoInRoom, tl.Tablet_TabletName" & _
                    " FROM tblQuizSession qs  LEFT JOIN t360_tblStudent sd  " & _
                    " ON qs.Player_Id = sd.Student_Id AND qs.School_Code = sd.School_Code LEFT JOIN t360_tblTablet tl ON qs.Tablet_Id = tl.Tablet_Id " & _
                    " AND qs.School_Code = tl.School_Code WHERE  qs.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' " & _
                    " and Player_Id = '" & idStudent & "' AND qs.School_Code = '" & HttpContext.Current.Session("SchoolID").ToString & "' ORDER BY sd.Student_CurrentNoInRoom "



            Dim dtCheckLabSpare As DataTable = db.getdata(sql)
            Dim TabletCutName As String = dtCheckLabSpare.Rows(0)("Tablet_TabletName").ToString.Replace("Desk", "")
            StudentNo = dtCheckLabSpare.Rows(0)("Student_CurrentNoInRoom").ToString
            'If dtCheckLabSpare.Rows(0)("Student_CurrentNoInRoom").ToString = Right((dtCheckLabSpare.Rows(0)("Tablet_TabletName").ToString), 1).ToString Then
            If dtCheckLabSpare.Rows(0)("Student_CurrentNoInRoom").ToString = TabletCutName Then
                detail.Add("1") 'ไม่ได้ใช้เครื่องสำรองอยู่
            Else
                If Type = "1" Then
                    detail.Add("1") 'ไม่ได้ใช้เครื่องสำรองอยู่
                Else
                    detail.Add("0") 'ใช้เครื่องสำรองอยู่ 
                    'detail.Add(dtCheckLabSpare.Rows(0)("Tablet_TabletName").ToString)
                End If


            End If
        Else
            sql = "select Tablet_IsOwner,Tablet_tabletname from t360_tblTablet inner join tblQuizSession on t360_tblTablet.Tablet_Id = tblQuizSession.Tablet_Id " & _
                     "where tblquizsession.Player_Id = '" & idStudent & "' and Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' "
            Dim dtCheckSpareTablet As DataTable = db.getdata(sql)

            If dtCheckSpareTablet.Rows.Count <> 0 Then


                If dtCheckSpareTablet.Rows(0)("Tablet_IsOwner").ToString = "False" Then
                    detail.Add("0") 'ใช้เครื่องสำรองอยู่ 
                    detail.Add(dtCheckSpareTablet.Rows(0)("Tablet_TabletName").ToString)
                Else
                    detail.Add("1") 'ไม่ได้ใช้เครื่องสำรองอยู่
                End If
            Else
                detail.Add("1")
            End If

        End If

        If HttpContext.Current.Session("IsSoundLab") = True Then
            sql = " SELECT     t360_tblTablet.Tablet_TabletName " & _
                   "FROM tblQuiz INNER JOIN tblTabletLabDesk ON tblQuiz.TabletLab_Id = tblTabletLabDesk.TabletLab_Id INNER JOIN " & _
                   "t360_tblTablet ON tblTabletLabDesk.Tablet_Id = t360_tblTablet.Tablet_Id " & _
                   "WHERE (tblTabletLabDesk.IsActive = 1) AND (t360_tblTablet.Tablet_IsActive = 1) " & _
                   "AND (tblQuiz.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "') " & _
                   "and tblTabletLabDesk.Player_Type = '" & Type & "' " & _
                   "and tblTabletLabDesk.DeskName not like '%" & StudentNo & "' " & _
                   "AND (tblTabletLabDesk.Tablet_Id NOT IN " & _
                   " (SELECT Tablet_Id FROM tblQuizSession WHERE (Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "')" & _
                   "and tblQuizsession.Tablet_id is not null)) ORDER BY tblTabletLabDesk.DeskName "
        Else
            sql = " SELECT Tablet_TabletName FROM t360_tblTablet WHERE Tablet_IsOwner = '0' AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString & "' " &
           " AND Tablet_Id NOT IN (SELECT  Distinct(t360_tblTablet.Tablet_Id) " &
           " FROM tblQuiz INNER JOIN " &
           " tblQuizSession ON tblQuiz.Quiz_Id = tblQuizSession.Quiz_Id INNER JOIN " &
           " t360_tblTablet ON tblQuizSession.Tablet_Id = t360_tblTablet.Tablet_Id " &
           " WHERE     (tblQuiz.EndTime IS NULL) AND (t360_tblTablet.Tablet_IsOwner = 0) " &
           "AND (tblQuizSession.LastUpdate > DATEADD(MINUTE, -20, dbo.GetThaiDate()))) " &
             "AND (t360_tblTablet.Tablet_Status = 1)  AND Tablet_Id not in (Select Tablet_Id from tbltabletlabdesk)"
        End If
        Dim dt As New DataTable
        dt = db.getdata(sql)

        For Each a In dt.Rows
            detail.Add(a("Tablet_TabletName").ToString)
        Next

        Dim SpareTablet() As String = detail.ToArray

        If SpareTablet.Length = 1 And SpareTablet(0) = "1" Then
            'ไม่มีเครื่องสำรองเหลือและเด็กคนนี้ไม่ได้ใช้เครื่องสำรองอยู่
            If HttpContext.Current.Session("IsSoundLab") = True Then
                SpareTablet(0) = "2"
            Else
                SpareTablet(0) = "3"
            End If
        ElseIf SpareTablet.Length = 1 And SpareTablet(0) = "0" Then
            If HttpContext.Current.Session("IsSoundLab") = True Then
                SpareTablet(0) = "4"
            Else
                SpareTablet(0) = "5"
            End If

        End If


        Dim js As New JavaScriptSerializer()
        Dim JsonString = New With {.detail = SpareTablet}
        Return js.Serialize(JsonString)

    End Function

    <Services.WebMethod()>
    Public Shared Function CheckEmptyDesk(idStudent As String)

        'เช็คว่าเครื่องของเราตอนนี้มีคนอื่นใช้งานอยู่่หรือเปล่า
        Dim sql As String
        Dim db As New ClassConnectSql()
        Dim EmptyDesk As String

        If HttpContext.Current.Session("IsSoundLab") = True Then
            sql = " select count(tblQuizSession.QuizSession_Id) as EmptyDesk from tblquizsession "
            sql &= " where Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' "
            sql &= " and Player_Id <> '" & idStudent & "' "
            sql &= " and Tablet_Id = (select Tablet_Id from tblTabletLabDesk "
            sql &= " inner join t360_tblStudent on tblTabletLabDesk.DeskName = t360_tblStudent.Student_CurrentNoInRoom "
            sql &= " inner join tblquiz on tblTabletLabDesk.TabletLab_Id = tblquiz.TabletLab_Id  "
            sql &= " where  School_Code = '" & HttpContext.Current.Session("SchoolID").ToString & "' and t360_tblStudent.Student_Id = '" & idStudent & "' "
            sql &= " and Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "')"

            Dim EmptyDeskAmount = db.ExecuteScalar(sql)

            If EmptyDeskAmount = 0 Then
                EmptyDesk = "True"
            Else
                EmptyDesk = "False"
            End If
        Else
            'ต้องมาเพิ่ม Query Check เมื่อเป็น Tablet สำรอง 
            EmptyDesk = "True"
        End If

        Return EmptyDesk

    End Function


    <Services.WebMethod()>
    Public Shared Function GettxtCurrentTablet() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' and TabletLab_Id is not null "
        Dim CheckIsSoundLab As String = _DB.ExecuteScalar(sql)
        If CInt(CheckIsSoundLab) > 0 Then
            _DB = Nothing
            Return "ใช้โต๊ะตรงเลขที่"
        Else
            _DB = Nothing
            Return "ใช้เครื่องปัจจุบัน"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function GettxtTabletSpare() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' and TabletLab_Id is not null "
        Dim CheckIsSoundLab As String = _DB.ExecuteScalar(sql)
        If CInt(CheckIsSoundLab) > 0 Then
            _DB = Nothing
            Return "ใช้โต๊ะสำรอง"
        Else
            _DB = Nothing
            Return "ใช้เครื่องสำรอง"
        End If
    End Function

    <Services.WebMethod()>
    Public Shared Function GettxtTitleChangeTablet() As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim _DB As New ClassConnectSql()
        Dim sql As String = " select COUNT(*) from tblQuiz where Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id").ToString() & "' and TabletLab_Id is not null "
        Dim CheckIsSoundLab As String = _DB.ExecuteScalar(sql)
        If CInt(CheckIsSoundLab) > 0 Then
            _DB = Nothing
            Return "True"
        Else
            _DB = Nothing
            Return "False"
        End If

    End Function

    <Services.WebMethod()>
    Public Shared Function setChangeTablet(ByVal idStudent As String, ByVal selectValue As String, ByVal useMyTablet As String) As String
        Dim db As New ClassConnectSql()
        Dim dt, dtTabletInQuiz As DataTable
        Dim ClsActivity As New ClsActivity(New ClassConnectSql())
        Dim sql, sqlTabletInQuiz As String
        Dim tabletID As String = ""
        Dim tabletStatus As Boolean = False ' เช็คแทปเล็ตว่าอยู่ใน db แล้วหรือเปล่า

        If useMyTablet = "checked" Then
            ' query เอา tabletId ของนักเรียนออกมา ถ้าเลือกใช้แทปเล็ตตัวเอง
            If HttpContext.Current.Session("IsSoundLab") = True Then
                sql = "select t360_tblTablet.Tablet_Id,t360_tblTablet.Tablet_SerialNumber from tblTabletLabDesk inner join t360_tblStudent on tblTabletLabDesk.DeskName = t360_tblStudent.Student_CurrentNoInRoom inner join t360_tblTablet on tblTabletLabDesk.Tablet_Id = t360_tblTablet.Tablet_Id where Student_Id = '" & idStudent & "' AND tblTabletLabDesk.TabletLab_Id = '" & HttpContext.Current.Session("GroupName") & "';"
            Else
                'sql = " SELECT Tablet_Id,Tablet_SerialNumber FROM t360_tblTabletOwner WHERE Owner_Id = '" & idStudent & "' AND School_Code = '" & HttpContext.Current.Session("SchoolID") & "' ;"
                sql = "  SELECT t360_tbltablet.Tablet_Id,Tablet_SerialNumber "
                sql &= " FROM t360_tblTabletOwner inner join t360_tblTablet on t360_tblTabletOwner.Tablet_Id = t360_tblTablet.Tablet_Id"
                sql &= " WHERE Owner_Id = '" & idStudent & "' AND t360_tbltablet.School_Code = '" & HttpContext.Current.Session("SchoolID") & "';"
            End If

        Else
            ' query เอา tabletId ถ้าเลือกใช้เครื่องสำรอง
            If ClsActivity.CheckQuizIsSoundLab(HttpContext.Current.Session("Quiz_Id").ToString()) = True Then
                sql = " SELECT Tablet_Id,Tablet_SerialNumber FROM t360_tblTablet WHERE  School_Code = '" & HttpContext.Current.Session("SchoolID").ToString & "' AND Tablet_TabletName = '" & selectValue & "' ;"
            Else
                sql = " SELECT Tablet_Id,Tablet_SerialNumber FROM t360_tblTablet WHERE Tablet_IsOwner = '0' AND School_Code = '" & HttpContext.Current.Session("SchoolID").ToString & "' AND Tablet_TabletName = '" & selectValue & "' ;"
            End If

        End If

        'tabletID = db.ExecuteScalar(sql)

        Dim tabletSerialNumber As String = ""
        dt = db.getdata(sql)
        If dt.Rows.Count > 0 Then
            tabletID = dt.Rows(0)("Tablet_Id").ToString()
            tabletSerialNumber = dt.Rows(0)("Tablet_SerialNumber").ToString()
        End If

        ' query เอา tablet ที่ใช้ใน quiz นี้ออกมา
        sqlTabletInQuiz = " SELECT Tablet_Id FROM tblQuizSession WHERE Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' AND School_Code = '" & HttpContext.Current.Session("SchoolID") & "' ;"
        dtTabletInQuiz = db.getdata(sqlTabletInQuiz)

        'ดึง DeviceUniqueId เดิมออกมา
        sql = " SELECT t.Tablet_SerialNumber FROM t360_tblTablet t INNER JOIN tblQuizSession qs ON t.Tablet_Id = qs.Tablet_Id AND t.School_Code = qs.School_Code"
        sql &= " WHERE qs.Player_Id = '" & idStudent & "' AND qs.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "'; "
        Dim oldDeviceUniqueId As String = db.ExecuteScalar(sql)

        For Each r In dtTabletInQuiz.Rows
            If (r("Tablet_Id").ToString = tabletID) Then
                tabletStatus = True
                Exit For
            End If
        Next
        ' ถ้าใน quizsession เป็น 
        If tabletStatus = False Then
            ' ถ้าเลือกแทปเล็ตตัวเอง แต่ไม่ได้อยู่ใน db ก็ update ให้เป็นเครื่องที่เลือกซะ
            sql = " UPDATE tblQuizSession SET Tablet_Id = '" & tabletID & "',IsActive = '0',Lastupdate = dbo.GetThaiDate(), ClientId = Null WHERE Player_Id = '" & idStudent & "' AND Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' AND School_Code = '" & HttpContext.Current.Session("SchoolID") & "' ;"
            db.Execute(sql)
        End If

        ' add ค่าลง redis ด้วยถ้ามีการเปลี่ยนค่า
        Dim redis As New RedisStore()
        Dim Quiz_Id As String = HttpContext.Current.Session("Quiz_Id").ToString()
        Dim q As Quiz = HttpContext.Current.Application("Quiz_" & Quiz_Id)
        q.PlayerId = idStudent
        redis.SetKey(Of Quiz)(tabletSerialNumber, q)
        redis.Expire(tabletSerialNumber, 900)
        redis.SAdd(Quiz_Id, tabletSerialNumber)
        redis.DEL(oldDeviceUniqueId) 'ลบ redis เครื่องเก่าที่เคยใช้

        Return "success"
    End Function

    <Services.WebMethod()>
    Public Shared Function getNoOfStudent() As String
        Dim db As New ClassConnectSql()
        Dim sql As String = ""
        Dim TabletAmount As Integer

        sql = " select count(t360_tblTablet.Tablet_Id) as StudentHaveTablet " & _
              " from t360_tblTablet inner join t360_tblTabletOwner on t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id" & _
              " inner join t360_tblStudent on t360_tblTabletOwner.Owner_Id = t360_tblStudent.Student_Id" & _
              " inner join tblQuiz on t360_tblStudent.Student_CurrentClass = tblQuiz.t360_ClassName" & _
              " and t360_tblStudent.Student_CurrentRoom = tblQuiz.t360_RoomName" & _
              " where t360_tblStudent.Student_IsActive = '1' and Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "';"
        TabletAmount = db.ExecuteScalar(sql)

        If TabletAmount < HttpContext.Current.Session("TotalStudent") Then
            Return "มีนักเรียนถือแท็บเลต " & TabletAmount & " เครื่อง จากทั้งหมด " & HttpContext.Current.Session("TotalStudent") & " คน"
        Else
            Return "จากทั้งหมด " & HttpContext.Current.Session("TotalStudent") & " คน"
        End If

    End Function

    <Services.WebMethod()>
    Public Shared Function getStudentID()
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim sql, isActive As String
        Dim dt As DataTable
        Dim studentId As String
        Dim tabletId, tabIsActive As String
        ' get ID ของนักเรียนมาเป็น ID ของ DIV
        'sql = "SELECT Student_Id FROM t360_tblStudent WHERE Student_CurrentNoInRoom = '" & number & "' AND Student_CurrentClass ='ป.5' AND Student_CurrentRoom = '/1' AND School_Code = '1000001'"
        'dt = db.getdata(sql)
        'studentId = dt.Rows(0)("Student_Id")
        '' เอา ID มาเช็คว่า Logon เข้ามาหรือยัง
        'sql2 = "SELECT Tablet_Id FROM tblQuizSession WHERE Player_Id ='" & studentId & "' AND Quiz_Id = 'ca487e8e-6a55-464b-a2cb-fc4bad101c80' AND IsActive ='1'"
        'dt2 = db.getdata(sql2)
        'isActive = "0"
        'If dt2.Rows.Count > 0 Then
        '    isActive = "True"
        '    tabletId = dt2.Rows(0)("Tablet_Id")
        'End If
        'sql2 = "SELECT Tablet_IsOwner FROM t360_tblTablet WHERE Tablet_Id ='" & tabletId & "';"
        'dt2 = db.getdata(sql2)
        'If dt2.Rows.Count > 0 Then
        '    tabIsActive = dt2.Rows(0)("Tablet_IsOwner")
        'End If

        'sql = "SELECT Tablet_IsOwner FROM t360_tblTablet WHERE Tablet_Id IN"
        'sql += "(SELECT Tablet_Id FROM tblQuizSession WHERE Quiz_Id = 'ca487e8e-6a55-464b-a2cb-fc4bad101c80' AND IsActive ='1' AND Player_Id IN"
        'sql += "(SELECT Student_Id FROM t360_tblStudent WHERE Student_CurrentNoInRoom = '" & number & "' AND Student_CurrentClass ='ป.5' "
        'sql += "AND Student_CurrentRoom = '/1' AND School_Code = '1000001'))"
        'If dt.Rows.Count > 0 Then
        '    tabIsActive = dt.Rows(0)("Tablet_IsOwner")
        'End If
        'sql = "SELECT qs.Player_Id,qs.Player_Type as Player_Type,qs.Tablet_Id as Tablet_Id,sd.Student_CurrentNoInRoom as Student_CurrentNoInRoom,tl.Tablet_IsOwner as Tablet_IsOwner,tl.Tablet_TabletName as Tablet_TabletName,qs.IsActive as IsActive FROM tblQuizSession qs "
        'sql += "LEFT JOIN t360_tblStudent sd  "
        'sql += "ON qs.Player_Id = sd.Student_Id "
        'sql += "LEFT JOIN t360_tblTablet tl "
        'sql += "ON qs.Tablet_Id = tl.Tablet_Id "
        'sql += "WHERE  qs.Quiz_Id = '" + HttpContext.Current.Session("Quiz_Id") + "'  ORDER BY sd.Student_CurrentNoInRoom" 'AND qs.School_Code = '" + HttpContext.Current.Session("SchoolID") + "'

        sql = "SELECT  DISTINCT qs.Player_Id as Player_Id,qs.Player_Type,sd.Student_CurrentNoInRoom, tl.Tablet_Id,NewestUserSubTable.Tablet_Id,"
        sql &= " tl.Tablet_IsOwner, tl.Tablet_TabletName, qs.LastUpdate, qs.IsActive ,tl.Tablet_SerialNumber "
        sql &= "FROM tblQuizSession qs "
        sql &= "left JOIN (SELECT Player_Id, MAX(tblQuizSession.lastupdate) AS NewestRow,tblQuizSession.Tablet_Id  FROM tblQuizSession INNER JOIN t360_tblTabletOwner ON t360_tblTabletOwner.Tablet_Id = tblQuizSession.Tablet_Id "
        sql &= "WHERE tblQuizSession.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' AND t360_tblTabletOwner.TabletOwner_IsActive = 1 GROUP BY Player_Id,tblQuizSession.Tablet_Id) AS NewestUserSubTable "
        sql &= "ON qs.Player_Id = NewestUserSubTable.Player_Id AND qs.lastupdate = NewestUserSubTable.NewestRow "
        sql &= "LEFT JOIN t360_tblStudent sd  "
        sql &= "ON qs.Player_Id = sd.Student_Id AND qs.School_Code = sd.School_Code "
        sql &= "LEFT JOIN t360_tblTablet tl "
        sql &= "ON qs.Tablet_Id = tl.Tablet_Id AND qs.School_Code = tl.School_Code "
        sql &= "WHERE  qs.Quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "' AND qs.School_Code = '" & HttpContext.Current.Session("SchoolID") & "' "
        sql &= " AND sd.Student_IsActive = 1 AND tl.Tablet_IsActive = 1 "
        sql &= "ORDER BY sd.Student_CurrentNoInRoom ;"

        dt = db.getdata(sql)
        Dim tabletName As String
        Dim JsonString As New ArrayList

        Dim redis As New RedisStore()

        If dt.Rows.Count > 0 Then

            Dim LabName As String
            Dim LabDetailName As String

            sql = "select TabletLab_Id from tblquiz where quiz_id = '" & HttpContext.Current.Session("Quiz_Id") & "';"
            Dim IsSoundLab As String = db.ExecuteScalar(sql).ToString

            If IsSoundLab <> "" Then

                HttpContext.Current.Session("IsSoundLab") = True

                sql = "select  TabletLab_Name from tbltabletlab inner join tblquiz on tblTabletLab.TabletLab_Id = tblQuiz.TabletLab_Id  "
                sql &= " where quiz_Id = '" & HttpContext.Current.Session("Quiz_Id") & "';"
                LabName = db.ExecuteScalar(sql)
                LabDetailName = "             ให้นักเรียนนั่งตามเลขที่ตรงกับหมายเลขโต๊ะค่ะ"
            Else
                LabName = ""
                LabDetailName = ""

            End If

            Dim tabletNameCut As String = ""

            For Each row As DataRow In dt.Rows
                tabletName = row("Tablet_TabletName").ToString


                If (HttpContext.Current.Session("IsSoundLab") = True) Then

                    tabletNameCut = tabletName.Replace("Desk", "")

                    If (tabletNameCut = row("Student_CurrentNoInRoom").ToString) Or (row("Player_Type").ToString = "1") Then
                        tabletName = ""
                    Else
                        tabletName = "โต๊ะ" & tabletNameCut
                    End If

                    'ถ้า isactive เช็คอีกทีว่ายังออนไลน์อยู่มั้ย
                    If row("IsActive") = "1" Then
                        If redis.Getkey(row("Tablet_SerialNumber").ToString() & "_status") = "" Then
                            row("IsActive") = False
                        End If
                    End If
                    'ปรับ IsOwner ของ Tablet ในห้อง Lab เพื่อให้เป็นสีเขียวได้
                    JsonString.Add(New With {.Player_Type = row("Player_Type"), .Student_CurrentNoInRoom = row("Student_CurrentNoInRoom"), .Tablet_IsOwner = "1", .Tablet_TabletName = tabletName, .IsActive = row("IsActive"), .Player_Id = row("Player_Id"), .LabName = LabName, .LabDetailName = LabDetailName})
                Else
                    If (row("Tablet_IsOwner").ToString = "True") Or (row("Tablet_IsOwner").ToString = "") Then
                        tabletName = ""
                    End If

                    'ถ้า isactive เช็คอีกทีว่ายังออนไลน์อยู่มั้ย
                    If row("IsActive") = "1" Then
                        If redis.Getkey(row("Tablet_SerialNumber").ToString() & "_status") = "" Then
                            row("IsActive") = False
                        End If
                    End If

                    JsonString.Add(New With {.Player_Type = row("Player_Type"), .Student_CurrentNoInRoom = row("Student_CurrentNoInRoom"), .Tablet_IsOwner = row("Tablet_IsOwner"), .Tablet_TabletName = tabletName, .IsActive = row("IsActive"), .Player_Id = row("Player_Id"), .LabName = LabName, .LabDetailName = LabDetailName})
                End If

            Next

        End If
        ' แปลงค่าเป็น JSON
        Dim js As New JavaScriptSerializer()
        'Dim sb As New StringBuilder()
        Dim idAndActive = js.Serialize(JsonString)

        Return idAndActive
    End Function


End Class