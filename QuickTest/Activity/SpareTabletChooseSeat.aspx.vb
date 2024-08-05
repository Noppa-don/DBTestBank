Imports System.Web
Public Class SpareTabletChooseSeat
    Inherits System.Web.UI.Page
    Dim ClsActivity As New ClsActivity(New ClassConnectSql)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("spareLevelName") = "ม.1/1"
        Session("spareSchoolCode") = "1000001"
        Session("spareUseLab") = False
        Session("spareTablet_Id") = "eab08020-042e-4577-a545-95a00aee47df"
        setElementOnPage(Session("spareLevelName"), Session("spareSchoolCode"), Session("spareUseLab"))

    End Sub

    Private Sub setElementOnPage(ByVal levelName As String, ByVal SchoolCode As String, ByVal useLab As Boolean)

        Dim arrQuiz As Array = getArrQuizDetail(levelName, SchoolCode, useLab) 'return arr[0] = quizid, arr[1] = classlevel, arr[2] = room
        Session("spareQuizId") = arrQuiz(0).ToString()

        lblTestName.Text = "ชื่อชุดควิซ " & ClsActivity.GetTestsetName(arrQuiz(0).ToString())
        lblClassRoom.Text = levelName

        Dim strBtn As String = ""
        'Dim dtStudentInRoom As DataTable = getNoOfStudent(arrQuiz(1).ToString(), arrQuiz(2).ToString(), SchoolCode)
        Dim dtStudentInRoom As DataTable = getStatusTabletStudent(arrQuiz(0).ToString(), SchoolCode)

        Try
            For i As Integer = 0 To dtStudentInRoom.Rows.Count - 1
                Dim bgColorStatus As String = "divSeat"
                If (dtStudentInRoom(i)("IsActive") = "1") Then
                    bgColorStatus = "divSeatActive"
                End If
                strBtn &= "<div class='" & bgColorStatus & "' id='" & dtStudentInRoom(i)("Player_Id").ToString() & "' >" & dtStudentInRoom(i)("Student_CurrentNoInRoom").ToString() & "</div>"
            Next
            strBtn &= "<div style='clear:both;'></div>"
            divSeatForChoose.InnerHtml = strBtn
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            divSeatForChoose.InnerHtml = "ERROR "
        End Try

    End Sub

    Private Function getArrQuizDetail(ByVal levelName As String, ByVal SchoolCode As String, ByVal useLab As Boolean) As Array

        'return arr[0] = quizid, arr[1] = classlevel, arr[2] = room
        Dim db As New ClassConnectSql()
        Dim sql As String = ""
        Dim arrLevelName As Array

        If (useLab) Then
            sql = " SELECT TOP 1 q.Quiz_Id as Quiz_Id,q.t360_ClassName as ClassName,q.t360_RoomName as RoomName FROM tblQuiz q INNER JOIN tblTabletLab tl "
            sql &= " ON q.TabletLab_Id = tl.TabletLab_Id WHERE tl.TabletLab_Name = '" & levelName & "' ORDER BY q.LastUpdate DESC ; "
            Dim dt As DataTable = db.getdata(sql)
            getArrQuizDetail = {dt(0)("Quiz_Id"), dt(0)("ClassName"), dt(0)("RoomName")}
        Else
            arrLevelName = levelName.Split("/")
            Dim roomName = "/" & arrLevelName(1).ToString()
            sql = " SELECT TOP 1 Quiz_Id FROM tblQuiz WHERE t360_ClassName = '" & arrLevelName(0).ToString() & "' AND t360_RoomName = '" & roomName & "' AND t360_SchoolCode = '" & SchoolCode & "' ORDER BY LastUpdate DESC ; "
            Dim dt As DataTable = db.getdata(sql)
            getArrQuizDetail = {dt(0)("Quiz_Id"), arrLevelName(0).ToString(), roomName}
        End If

    End Function

    Private Function getNoOfStudent(ByVal classLevel As String, ByVal roomNo As String, ByVal schoolCode As String) As DataTable

        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT Student_CurrentNoInRoom,Student_Id FROM t360_tblStudent WHERE Student_CurrentClass = '" & classLevel & "' "
        sql &= " AND Student_CurrentRoom = '" & roomNo & "' AND School_Code = '" & schoolCode & "' AND Student_IsActive = '1' ORDER BY Student_CurrentNoInRoom ; "
        getNoOfStudent = db.getdata(sql)

    End Function

    Private Function getStatusTabletStudent(ByVal Quiz_Id As String, ByVal schoolCode As String) As DataTable

        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT DISTINCT qs.Player_Id,ts.Student_CurrentNoInRoom,qs.IsActive FROM tblQuizSession qs INNER JOIN "
        sql &= " t360_tblStudent ts ON qs.Player_Id = ts.Student_Id WHERE qs.Quiz_Id = '" & Quiz_Id & "' "
        sql &= " AND qs.School_Code = '" & schoolCode & "' AND qs.Player_Type = '2' ORDER BY ts.Student_CurrentNoInRoom; "
        getStatusTabletStudent = db.getdata(sql)

    End Function

    <Services.WebMethod()>
    Public Shared Function updateChangeTabletInQuiz(ByVal playerId As String)
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Try
            ' เอามาupdate ที่ playerId  quizid schoolcode 
            Dim db As New ClassConnectSql()
            Dim tabletId As String = HttpContext.Current.Session("spareTablet_Id")
            Dim sql As String = " UPDATE tblQuizSession SET Tablet_Id = '" & tabletId & "',IsActive = '1',LastUpdate = dbo.GetThaiDate(), ClientId = Null"
            sql &= " WHERE Player_Id = '" & playerId & "' AND Quiz_Id = '" & HttpContext.Current.Session("spareQuizId") & "' AND School_Code = '" & HttpContext.Current.Session("spareSchoolCode") & "' ;"
            'db.Execute(sql)
            Return "success"
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "error"
        End Try

    End Function

    <Services.WebMethod()>
    Public Shared Function refreshStatusConnectionTablet()
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim db As New ClassConnectSql()
        Dim sql As String = ""
        Try
            sql = "SELECT  DISTINCT qs.Player_Id as Player_Id,qs.Player_Type,sd.Student_CurrentNoInRoom, tl.Tablet_Id,"
            sql &= " tl.Tablet_IsOwner, tl.Tablet_TabletName, qs.LastUpdate, qs.IsActive "
            sql &= "FROM tblQuizSession qs "
            sql &= "INNER JOIN (SELECT Player_Id, MAX(lastupdate) AS NewestRow FROM tblQuizSession "
            sql &= "WHERE tblQuizSession.Player_Type = '2' AND tblQuizSession.Quiz_Id = '" & HttpContext.Current.Session("spareQuizId") & "' GROUP BY Player_Id) AS NewestUserSubTable "
            sql &= "ON qs.Player_Id = NewestUserSubTable.Player_Id AND qs.lastupdate = NewestUserSubTable.NewestRow "
            sql &= "LEFT JOIN t360_tblStudent sd  "
            sql &= "ON qs.Player_Id = sd.Student_Id AND qs.School_Code = sd.School_Code "
            sql &= "LEFT JOIN t360_tblTablet tl "
            sql &= "ON qs.Tablet_Id = tl.Tablet_Id AND qs.School_Code = tl.School_Code "
            sql &= "WHERE  qs.Quiz_Id = '" & HttpContext.Current.Session("spareQuizId") & "' AND qs.School_Code = '" & HttpContext.Current.Session("spareSchoolCode") & "' "
            'sql &= "--AND qs.IsActive = '1'  "
            sql &= "ORDER BY sd.Student_CurrentNoInRoom "

            Dim strBtn As String = ""
            Dim dtStudentInRoom As DataTable = db.getdata(sql)
            For i As Integer = 0 To dtStudentInRoom.Rows.Count - 1
                Dim bgColorStatus As String = "divSeat"
                If (dtStudentInRoom(i)("IsActive") = "1") Then
                    bgColorStatus = "divSeatActive"
                End If
                strBtn &= "<div class='" & bgColorStatus & "' id='" & dtStudentInRoom(i)("Player_Id").ToString() & "' >" & dtStudentInRoom(i)("Student_CurrentNoInRoom").ToString() & "</div>"
            Next
            strBtn &= "<div style='clear:both;'></div>"


            Return strBtn
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return "error"
        End Try

    End Function
End Class