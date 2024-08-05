Public Class purgeClass
    Inherits System.Web.UI.Page
    Dim DB As New ClassConnectSql
    Public txtDialog As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not IsPostBack Then
                '***** 28/01/2014 ปิดไว้ก่อน หน้านี้ยังไม่ได้ทดสอบ ต้องทดสอบก่อนใช้ใหม่
                'BindcbAcademicYear()
                'createClassAndRoom()
                '****************************************
            End If
        End If
        ' btnSave.Attributes.Add("onClick", "javascript: dialog('open');")
    End Sub

    Public Sub BindcbAcademicYear()
        With cbAcademicYear
            cbAcademicYear.Items.Clear()
            Dim showdata As DataTable
            showdata = DB.getdata("select distinct AcademicYear,cast(year(lastupdate) + 543 as varchar) as QuizYear from tblQuiz where TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')  order by QuizYear desc")
            Dim PID As String
            Dim PName As String

            If showdata.Rows.Count = 0 Then
                Dim Row = showdata.NewRow
                Row("QuizYear") = " ไม่มีข้อมูล"
                Row("AcademicYear") = 0
                showdata.Rows.InsertAt(Row, 0)
            End If


            For Each Name In showdata.Rows
                PID = Name.Item("AcademicYear")
                PName = Name.Item("QuizYear")
                cbAcademicYear.Items.Add(New ListItem(PName, PID))
            Next

        End With

    End Sub

    Public Sub createClassAndRoom()

        ClassAndRoom.InnerHtml = ""

        Dim SelectedYear As String = cbAcademicYear.SelectedItem.ToString

        If SelectedYear = "" Then
            Dim dtMaxYear = DB.getdata("select distinct top 1 cast(year(lastupdate) + 543 as varchar) as QuizYear from tblQuiz where TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')  order by QuizYear desc ")
            If dtMaxYear.Rows.Count = 0 Then
                Exit Sub
            Else
                SelectedYear = dtMaxYear.Rows(0)("QuizYear").ToString
            End If

        End If
        If Not SelectedYear = " ไม่มีข้อมูล" Then

            Dim sql As String = "select distinct t360_ClassName from tblquiz where year(lastupdate) + 543 = '" & SelectedYear & "' and t360_Roomname <> '/' and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "') order by t360_ClassName;"

            Dim dtclass As DataTable = DB.getdata(sql)

            Dim classTag As String = ""

            For i = 0 To dtclass.Rows.Count - 1

                classTag &= "<div class=""classRoom""><div class=""headClass""><span style=""font-size: large;""><b>"
                classTag &= dtclass.Rows(i)("t360_ClassName") & " ></b></span></div>"

                sql = "select t360_RoomName from (select distinct REPLACE(t360_RoomName,'/','') as t360_RoomName "
                sql &= " from tblquiz where year(lastupdate) + 543 = '" & SelectedYear & "' "
                sql &= " and t360_ClassName = '" & dtclass.Rows(i)("t360_ClassName") & "' and t360_Roomname <> '/' "
                sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')) as a "
                sql &= " order by dbo.ConvertRoom(t360_RoomName);"

                Dim dtroom As DataTable = DB.getdata(sql)

                Dim Room As String

                For j = 0 To dtroom.Rows.Count - 1
                    Room = dtroom.Rows(j)("t360_RoomName")
                    If Room.Length > 2 Then
                        classTag &= "<div class=""roomInClass"" id=""" & dtclass.Rows(i)("t360_ClassName") & "/" & Room & """><span style=""margin: 10px;"">" & "/" & dtroom.Rows(j)("t360_RoomName") & "</span></div>"
                    Else
                        classTag &= "<div class=""roomInClass"" style=""width: 40px;"" id=""" & dtclass.Rows(i)("t360_ClassName") & "/" & Room & """>" & "/" & dtroom.Rows(j)("t360_RoomName") & "</div>"
                    End If

                Next
                classTag &= "</div>"
            Next
            'สร้าง div ดำๆๆ ปิดด้วยอีกทีนึง
            classTag &= "<div id='divGradient' style='position: absolute; top: auto; opacity: 0.6; z-index: 9;" & _
                        " -webkit-border-radius: 0.5em; height: 100%; width: 678px; background-color: #EDEFF0; " & _
                        " top: 0px; left: 0px;' ></div>"

            ClassAndRoom.InnerHtml = classTag

        End If


    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnBack.Click
        Response.Redirect("../testset/Step1.aspx")
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        '****** 28/01/2014 ปิดไว้ก่อน ตรงนี้ยังไม่ได้ทดสอบ ต้องทดสอบก่อนเอาไปใช้ใหม่

        'If txtConfirm.Text = "แน่ใจ" Then
        '    Dim deleteClass As String
        '    Dim sql As String
        '    If hidRoomSelected.Value <> "" Then
        '        deleteClass = Right(hidRoomSelected.Value, hidRoomSelected.Value.Length - 1)
        '    End If

        '    Dim SelectedYear As Integer = CInt(cbAcademicYear.SelectedItem.ToString) - 543
        '    If SelectedYear.ToString = "" Then
        '        Dim DateNow As Date = Date.Now
        '        SelectedYear = CInt(DateNow.Year) - 543
        '    End If

        '    Dim ArrCheckBoolean() As String = {rdAllYear.Checked, rdSelectedYear.Checked, rdAllClass.Checked, rdSomeClass.Checked}
        '    Dim ArrTable() As String = {"tblQuizQuestion", "tblQuizAnswer", "tblQuizSession", "tblQuizScore"} ', "tblQuiz"
        '    Dim ArrTableStudent() As String = {"t360_tblStudent", "t360_tblTabletOwner"}

        '    '*************************************************************
        '    'If ArrCheckBoolean(0) Then 'ลบทั้งหมด

        '    '    For i = 0 To ArrTable.Length - 1
        '    '        sql = "Delete " & ArrTable(i) & " where Quiz_Id in (select Quiz_Id from tblQuiz where TestSet_Id in "
        '    '        sql &= " (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "'))"
        '    '        DB.Execute(sql)
        '    '    Next

        '    '    For i = 0 To ArrTableStudent.Length - 1
        '    '        sql = "Delete " & ArrTableStudent(i) & " where School_Code  = '" & Session("SchoolId") & "';"
        '    '        DB.Execute(sql)
        '    '    Next

        '    '    sql = "Delete tblQuiz where TestSet_Id in "
        '    '    sql &= " (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')"
        '    '    DB.Execute(sql)


        '    'End If
        '    '***********************************************

        '    If ArrCheckBoolean(1) = True And ArrCheckBoolean(2) = True Then 'เลือกปี ลบทั้งหมด

        '        'ลบ tblQuiz 

        '        sql = "Delete tblQuiz where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103)"
        '        sql &= " and  TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "') ;"
        '        DB.Execute(sql)
        '        'ลบ table Quiz อื่นๆ 
        '        For i = 0 To ArrTable.Length - 1 'วนลบข้อมูลในปีนั้นก่อน
        '            sql = "delete " & ArrTable(i) & " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) "
        '            sql &= " and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103)"
        '            sql &= " and Quiz_Id in (select quiz_id from tblQuiz where TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')) ;"
        '            DB.Execute(sql)
        '        Next
        '        'เลือกห้องเรียนที่มีในปีที่จะลบ และไม่มีในปีอื่นๆ

        '        sql = "select distinct t360_ClassName,t360_RoomName from tblquiz "
        '        sql &= " where quiz_id in(select quiz_id from tblquiz  "
        '        sql &= " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103)) "
        '        sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "') "
        '        sql &= " Except"
        '        sql &= " select distinct t360_ClassName,t360_RoomName  from tblquiz "
        '        sql &= " where quiz_id not in(select quiz_id from tblquiz  "
        '        sql &= " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103))"
        '        sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')"

        '        Dim dtStudentForDelete As DataTable = DB.getdata(sql)
        '        'ได้ชั้นและห้องมาแล้ว เอาไปวนลบ

        '        For Each a In dtStudentForDelete.Rows
        '            sql = "delete t360_tblStudent where Student_CurrentClass = '" & a("t360_ClassName") & "' and Student_CurrentRoom = '" & a("t360_RoomName") & "';"
        '            DB.Execute(sql)
        '        Next
        '        'ลบนักเรียนที่อยู่ในชั้นในห้องนั้นแล้ว ให้ลบ TabletOwner ที่ไม่มีนักเรียนแล้วทิ้ง

        '        sql = "delete t360_tblTabletOwner where Owner_Id not in (select Student_id from t360_tblStudent) and Owner_Type = '2'"
        '        DB.Execute(sql)

        '    End If

        '    If ArrCheckBoolean(1) = True And ArrCheckBoolean(3) = True Then 'ลบบางชั้น บางห้อง
        '        'วนลบตามตัวแปรที่ส่งมา
        '        '"ม.4/3,ม.4/5,ม.5/1"
        '        Dim ClassName As String
        '        Dim RoomName As String
        '        Dim ArrSelectedDelete() As String = Split(deleteClass, ",")

        '        'ตัดออกมาเป็นแต่ละห้อง
        '        For i = 0 To ArrSelectedDelete.Length - 1
        '            '"ม.4/3"
        '            ClassName = ArrSelectedDelete(i).Substring(0, 3)
        '            RoomName = ArrSelectedDelete(i).Substring(3, ArrSelectedDelete(i).Length - 3)
        '            'RoomName = ArrSelectedDelete(i).Substring(3, 2)
        '            '"ม.4" , "/3"

        '            'ลบ tblQuiz
        '            sql = "delete tblQuiz where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) "
        '            sql &= " and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103) "
        '            sql &= " and t360_ClassName = '" & ClassName & "' and t360_RoomName = '" & RoomName & "'"
        '            sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')"
        '            DB.Execute(sql)
        '            'ลบ table Quiz อื่นๆ
        '            For j = 0 To ArrTable.Length - 1 'วนลบข้อมูลในปีนั้น และ ชั้นเรียนนั้น ห้องนั้น

        '                sql = "delete " & ArrTable(j) & " where Quiz_Id in (select Quiz_Id from tblquiz "
        '                sql &= " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) "
        '                sql &= " and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103) "
        '                sql &= " and t360_ClassName = '" & ClassName & "' and t360_RoomName = '" & RoomName & "'"
        '                sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')) "

        '                DB.Execute(sql)
        '            Next

        '            'เลือกห้องเรียนที่มีในปีที่จะลบ และไม่มีในปีอื่นๆ
        '            sql = "select distinct t360_ClassName,t360_RoomName  from tblquiz"
        '            sql &= " where quiz_id in(select quiz_id from tblquiz "
        '            sql &= " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103) "
        '            sql &= " and t360_ClassName = '" & ClassName & "' and t360_RoomName = '" & RoomName & "')"
        '            sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')"
        '            sql &= " Except "
        '            sql &= " select distinct t360_ClassName,t360_RoomName  from tblquiz"
        '            sql &= " where quiz_id not in(select quiz_id from tblquiz "
        '            sql &= " where  lastupdate between convert(smalldatetime,'1/5/" & SelectedYear.ToString & "',103) and convert(smalldatetime,'1/5/" & CStr(SelectedYear + 1) & "',103) "
        '            sql &= " and t360_ClassName = '" & ClassName & "' and t360_RoomName = '" & RoomName & "')"
        '            sql &= " and TestSet_Id in (select TestSet_Id from tblTestSet where SchoolId = '" & Session("SchoolId") & "')"
        '            Dim dtStudentForDelete As DataTable = DB.getdata(sql)

        '            'ได้ชั้นและห้องมาแล้ว เอาไปวนลบ
        '            For Each a In dtStudentForDelete.Rows
        '                sql = "delete t360_tblStudent where Student_CurrentClass = '" & a("t360_ClassName") & "' and Student_CurrentRoom = '" & a("t360_RoomName") & "';"
        '                DB.Execute(sql)
        '            Next

        '            'ลบนักเรียนที่อยู่ในชั้นในห้องนั้นแล้ว ให้ลบ TabletOwner ที่ไม่มีนักเรียนแล้วทิ้ง
        '            sql = "delete t360_tblTabletOwner where Owner_Id not in (select Student_id from t360_tblStudent) and Owner_Type = '2';"
        '            DB.Execute(sql)

        '        Next

        '    End If

        'End If
        'BindcbAcademicYear()
        'createClassAndRoom()

        'rdAllClass.Checked = True
        'rdSomeClass.Checked = False
        'txtConfirm.Text = ""

    End Sub

    Protected Sub cbAcademicYear_SelectedIndexChanged1(ByVal sender As Object, ByVal e As EventArgs) Handles cbAcademicYear.SelectedIndexChanged
           createClassAndRoom()
        rdAllClass.Checked = True
        rdSomeClass.Checked = False
    End Sub
End Class
