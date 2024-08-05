Imports KnowledgeUtils

Public Class StudentStatusPage
    Inherits System.Web.UI.Page
    Public Shared NoInRoomNumberOne, ScoreNumberOne, NoInRoomNumberTwo, ScoreNumberTwo, NoInRoomNumberThree, ScoreNumberThree, PodiumImage1, PodiumImage2, PodiumImage3 As String
    Dim _DB As New ClassConnectSql()
    Dim ClsActivity As New ClsActivity(New ClassConnectSql())
    Dim ClsUser As New ClsUser(New ClassConnectSql())
    Public Shared SelfPace As Boolean
    Public Shared CurrentExam As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("QuizId") Is Nothing And Request.QueryString("CurrentExamNum") Is Nothing Then
            Exit Sub
        End If

        Dim QuizId As String = Request.QueryString("QuizId").ToString()
        SelfPace = CheckIsSelfPace(QuizId)
        If SelfPace = False Then
            CurrentExam = "อยู่ข้อที่ " & Request.QueryString("CurrentExamNum")
        Else
            CurrentExam = "ทั้งหมด " & GetTotalExamInQuiz(Request.QueryString("QuizId").ToString()) & " ข้อ"
        End If

        GetTop3StudentTopScore(QuizId)
        CreateDivStudent(QuizId, Request.QueryString("CurrentExamNum"))

    End Sub

    Private Sub GetTop3StudentTopScore(ByVal QuizId As String)

        Dim sql As String = " SELECT TOP 3 tblQuizScore.Student_Id, SUM(tblQuizScore.Score) AS TotalScore, t360_tblStudent.Student_CurrentNoInRoom,t360_tblstudent.School_Code " & _
                            " FROM tblQuizScore INNER JOIN t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (tblQuizScore.Quiz_Id = '" & QuizId & "') AND (tblQuizScore.IsActive = 1) " & _
                            " GROUP BY tblQuizScore.Student_Id, t360_tblStudent.Student_CurrentNoInRoom,t360_tblstudent.School_Code ORDER BY TotalScore DESC "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then

            If dt.Rows(0)("Student_CurrentNoInRoom") IsNot DBNull.Value Then
                NoInRoomNumberOne = dt.Rows(0)("Student_CurrentNoInRoom")
                PodiumImage1 = ""

                'ต้องมาแก้ให้เช็คก่อนว่ามีรูปเด็กมั้ย
                Dim PhotoStatus As Boolean
                PhotoStatus = ClsUser.GetStudentHasPhoto(dt.Rows(0)("Student_Id").ToString())

                Dim ImgStudent As String
                Dim BGStyle As String

                If PhotoStatus Then
                    ImgStudent = "../UserData/" & dt.Rows(0)("School_Code").ToString() & "/{" & dt.Rows(0)("Student_Id").ToString() & "}/Id.jpg"
                Else
                    ImgStudent = "MonsterID.axd?seed=" & dt.Rows(0)("Student_Id").ToString() & "&size=120"
                End If

                PodiumImage1 = ImgStudent

                If dt.Rows(0)("TotalScore") IsNot DBNull.Value Then
                    ScoreNumberOne = dt.Rows(0)("TotalScore").ToString().ToPointplusScore()
                Else
                    ScoreNumberOne = "0"
                End If
            Else
                'NoInRoomNumberOne = "ไม่มีคนที่ได้อันดับ 1"
            End If

            If dt.Rows.Count >= 2 Then
                If dt.Rows(1)("Student_CurrentNoInRoom") IsNot DBNull.Value Then
                    NoInRoomNumberOne = dt.Rows(1)("Student_CurrentNoInRoom")
                    PodiumImage2 = ""

                    Dim PhotoStatus As Boolean
                    PhotoStatus = ClsUser.GetStudentHasPhoto(dt.Rows(1)("Student_Id").ToString())
                    Dim ImgStudent As String
                    Dim BGStyle As String
                    If PhotoStatus Then
                        ImgStudent = "../UserData/" & dt.Rows(1)("School_Code").ToString() & "/{" & dt.Rows(1)("Student_Id").ToString() & "}/Id.jpg"
                    Else
                        ImgStudent = "MonsterID.axd?seed=" & dt.Rows(1)("Student_Id").ToString() & "&size=120"
                    End If

                    PodiumImage2 = ImgStudent
                Else
                    'NoInRoomNumberTwo = "ไม่มีคนที่ได้อันดับ 2"
                End If
                If dt.Rows(1)("TotalScore") IsNot DBNull.Value Then
                    ScoreNumberTwo = dt.Rows(1)("TotalScore").ToString().ToPointplusScore()
                Else
                    ScoreNumberTwo = "0"
                End If
            Else
                'NoInRoomNumberTwo = "ไม่มีคนที่ได้อันดับ 2"
                ScoreNumberTwo = "0"
            End If

            If dt.Rows.Count >= 3 Then
                If dt.Rows(2)("Student_CurrentNoInRoom") IsNot DBNull.Value Then
                    NoInRoomNumberOne = dt.Rows(2)("Student_CurrentNoInRoom")
                    PodiumImage3 = ""

                    'ต้องมาแก้ให้เช็คก่อนว่ามีรูปเด็กมั้ย
                    Dim PhotoStatus As Boolean
                    PhotoStatus = ClsUser.GetStudentHasPhoto(dt.Rows(2)("Student_Id").ToString())
                    Dim ImgStudent As String
                    Dim BGStyle As String
                    If PhotoStatus Then
                        ImgStudent = "../UserData/" & dt.Rows(2)("School_Code").ToString() & "/{" & dt.Rows(2)("Student_Id").ToString() & "}/Id.jpg"
                    Else
                        ImgStudent = "MonsterID.axd?seed=" & dt.Rows(2)("Student_Id").ToString() & "&size=120"
                    End If

                    PodiumImage3 = ImgStudent
                Else
                    'NoInRoomNumberThree = "ไม่มีคนที่ได้อันดับ 3"
                End If
                If dt.Rows(2)("TotalScore") IsNot DBNull.Value Then
                    ScoreNumberThree = dt.Rows(2)("TotalScore").ToString().ToPointplusScore()
                Else
                    ScoreNumberThree = "0"
                End If
            Else
                'NoInRoomNumberThree = "ไม่มีคนที่ได้อันดับ 3"
                ScoreNumberThree = "0"
            End If

            'If dt.Rows(0)("TotalScore") IsNot DBNull.Value Then
            '    ScoreNumberOne = dt.Rows(0)("TotalScore").ToString()
            'Else
            '    ScoreNumberOne = "0"
            'End If

        End If

    End Sub

    Private Sub CreateDivStudent(ByVal QuizId As String, ByVal CurrentExamNum As String)

        Dim sb As New StringBuilder
        Dim dtSumScore As DataTable = GetDtSumScore(QuizId)
        Dim TotalQuestion As String = ClsActivity.GetExamAmount(QuizId)
        If CheckIsSelfPace(QuizId) = True Then 'Check ก่อนว่าเป็นโหมดไปพร้อมกันหรือเปล่า
            'Dim CurrentExam As String = ""
            For index = 0 To dtSumScore.Rows.Count - 1
                'CurrentExam = ClsActivity.GetExamNum(QuizId, dtSumScore.Rows(index)("Student_Id").ToString())
                sb.Append("<div class='DivEachStudent'>")
                ' ถ้าทำเสร็จแล้วให้มีเครื่องหมายถูกขึ้น


                Dim PhotoStatus As Boolean
                PhotoStatus = ClsUser.GetStudentHasPhoto(dtSumScore.Rows(index)("Student_Id").ToString())
                Dim ImgStudent As String
                Dim BGStyle As String
                If PhotoStatus Then
                    BGStyle = "background-size:cover;"
                    ImgStudent = "../UserData/" & dtSumScore.Rows(index)("t360_SchoolCode").ToString() & "/{" & dtSumScore.Rows(index)("Student_Id").ToString() & "}/Id.jpg"

                Else

                    ImgStudent = "MonsterID.axd?seed=" & dtSumScore.Rows(index)("Student_Id").ToString() & "&size=179"
                    BGStyle = "background-size: contain; background-repeat: no-repeat; background-position: center;"
                End If
                sb.Append("<div class='ForDivtopEachStudent' style='background:url(" & ImgStudent & "); " & BGStyle & "'> ")

                sb.Append("<span class='spnStudentNo'>#" & dtSumScore.Rows(index)("Student_CurrentNoInRoom").ToString() & "</span>")
                Dim LastExamNum As String = GetLastQuestionPerStudent(dtSumScore.Rows(index)("Student_Id").ToString(), QuizId)
                Dim CheckIsAllAnswer As Boolean = CheckIsClearAllAnswer(dtSumScore.Rows(index)("Student_Id").ToString(), QuizId)
                sb.Append("<div class='ForCurrentExam'>ข้อ " & LastExamNum & "</div></div>")
                'เช็คว่านักเรียนคนนี้ทำเสร็จหรือยัง
                If CheckIsAllAnswer = True Then
                    sb.Append("<div style='padding:5px;'><img src='../Images/right.png' /> " & dtSumScore.Rows(index)("TotalScore").ToString().ToPointplusScore() & " คะแนน</div>")
                Else
                    sb.Append("<div style='padding:5px;'> " & dtSumScore.Rows(index)("TotalScore").ToString().ToPointplusScore() & " คะแนน</div>")
                End If
                sb.Append("</div>")
                'sb.Append("<div style='width:50px;font-size:16px;border-right:1px solid;border-bottom:1px solid;'>")
                'sb.Append("<div>ข้อ</div>")
                'sb.Append("<div>" & CurrentExamNum & "/" & TotalQuestion & "</div>")
                'sb.Append("</div>")
                'sb.Append(" <div class='ForDivScore' >" & dtSumScore(index)("TotalScore").ToString() & " คะแนน</div>")
                'sb.Append("</div>")
                'sb.Append("<div style='padding:5px;'>" & dtSumScore.Rows(index)("Student_CurrentNoInRoom").ToString() & "</div>")
                'sb.Append("</div>")
            Next
        Else
            For z = 0 To dtSumScore.Rows.Count - 1

                Dim CheckIsAnswer As Boolean = CheckIsAnswered(QuizId, dtSumScore.Rows(z)("Student_Id").ToString(), CurrentExamNum)
                sb.Append("<div class='DivEachStudent'>")
                'ต้องมาแก้ให้เช็คก่อนว่ามีรูปเด็กมั้ย
                Dim PhotoStatus As String = 0
                Dim ImgStudent As String
                Dim BGStyle As String
                If PhotoStatus Then

                    ImgStudent = "../UserData/" & dtSumScore.Rows(z)("t360_SchoolCode").ToString() & "/{" & dtSumScore.Rows(z)("Student_Id").ToString() & "}/Id.jpg"
                    BGStyle = "background-size:cover;"
                Else

                    ImgStudent = "MonsterID.axd?seed=" & dtSumScore.Rows(z)("Student_Id").ToString() & "&size=120"
                    BGStyle = "background-size: contain; background-repeat: no-repeat; background-position: center;"
                End If
                sb.Append("<div class='ForDivtopEachStudent' style='background:url(" & ImgStudent & "); " & BGStyle & "'> ")
                sb.Append("<span class='spnStudentNo' style='left:5px;'>#" & dtSumScore.Rows(z)("Student_CurrentNoInRoom").ToString() & "</span>")
                'sb.Append("<div class='ForCurrentExam' >ข้อ " & CurrentExamNum & "</div>")
                sb.Append("</div>")
                If CheckIsAnswer = True Then
                    sb.Append("<div class='IsAnswer' style='padding:5px;'>" & dtSumScore.Rows(z)("TotalScore").ToString().ToPointplusScore() & " คะแนน</div>")
                Else
                    sb.Append("<div style='padding:5px;'>" & dtSumScore.Rows(z)("TotalScore").ToString().ToPointplusScore() & " คะแนน</div>")
                End If
                sb.Append("</div>")
                'sb.Append("<div class='ForDivtopEachStudent'>")
                'sb.Append(" <div class='ForDivScoreNotSelfPace' >" & dtSumScore.Rows(z)("TotalScore").ToString() & " คะแนน</div>")
                'sb.Append("</div>")
                'sb.Append("<div style='padding:5px;'>" & dtSumScore.Rows(z)("Student_CurrentNoInRoom").ToString() & "</div>")
                'sb.Append("</div>")
            Next
        End If
        DivBottom.InnerHtml = sb.ToString()

    End Sub

    'Function ที่เช็คว่า Quiz นี้เป็นโหมดแบบไปพร้อมกันหรือเปล่า
    Private Function CheckIsSelfPace(ByVal QuizId As String) As Boolean
        Dim sql As String = " select Selfpace from tblQuiz where Quiz_Id = '" & QuizId & "' "
        Dim CheckSelfPace As String = _DB.ExecuteScalar(sql)
        If CheckSelfPace = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetDtSumScore(ByVal QuizId As String) As DataTable

        Dim sql As String = " SELECT tblQuiz.t360_SchoolCode,t360_tblStudent.Student_CurrentNoInRoom, SUM(tblQuizScore.Score) AS TotalScore,t360_tblStudent.Student_Id " & _
                            " FROM tblQuiz INNER JOIN tblQuizScore ON tblQuiz.Quiz_Id = tblQuizScore.Quiz_Id INNER JOIN " & _
                            " t360_tblStudent ON tblQuizScore.Student_Id = t360_tblStudent.Student_Id " & _
                            " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') GROUP BY t360_tblStudent.Student_CurrentNoInRoom,t360_tblStudent.Student_Id,tblQuiz.t360_SchoolCode " & _
                            " order by t360_tblStudent.Student_CurrentNoInRoom  "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        Return dt

    End Function

    'Function ที่หาว่านักเรียนคนนี้ตอบข้อปัจจุบันหรือยัง สำหรับโหมดแบบไปพร้อมกัน
    Private Function CheckIsAnswered(ByVal QuizId As String, ByVal StudentId As String, ByVal QQNo As String) As Boolean

        Dim sql As String = " select Answer_Id from tblQuizScore where Quiz_Id = '" & QuizId & "' " & _
                            " and QQ_No = '" & QQNo & "' and Student_Id = '" & StudentId & "' "
        Dim CheckIsAnswer As String = _DB.ExecuteScalar(sql)
        Dim ReturnValue As Boolean = False
        If CheckIsAnswer = "" Then
            ReturnValue = False
        Else
            ReturnValue = True
        End If
        Return ReturnValue

    End Function

    'Function หาจำนวนข้อทั้งหมดของ Quiz ครั้งนี้
    Private Function GetTotalExamInQuiz(ByVal QuizId As String) As String
        Dim sql As String = " SELECT COUNT(DISTINCT tblTestSetQuestionDetail.Question_Id) AS TotalQuestion " & _
                            " FROM tblQuiz INNER JOIN tblTestSet ON tblQuiz.TestSet_Id = tblTestSet.TestSet_Id INNER JOIN " & _
                            " tblTestSetQuestionSet ON tblTestSet.TestSet_Id = tblTestSetQuestionSet.TestSet_Id INNER JOIN " & _
                            " tblTestSetQuestionDetail ON tblTestSetQuestionSet.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id " & _
                            " WHERE Quiz_Id = '" & QuizId & "' and tblTestSetQuestionSet.IsActive = '1' And tblTestSetQuestionDetail.IsActive = 1"
        Dim TotalExam As String = _DB.ExecuteScalar(sql)
        Return TotalExam
    End Function

    'Function ที่หาข้อสุดท้ายที่นักเรียนคนนั้นทำถึง เพื่อเอาไปแสดงในโหมดแบบไปไม่พร้อมกัน
    Private Function GetLastQuestionPerStudent(ByVal StudentId As String, ByVal QuizId As String)
        Dim sql As String = " SELECT MAX(QQ_No) FROM dbo.tblQuizScore WHERE Quiz_Id = '" & QuizId & "' " & _
                            " AND Student_Id = '" & StudentId & "' "
        Dim LastExamNum As String = _DB.ExecuteScalar(sql)
        Return LastExamNum
    End Function

    'Function Check ในโหมดแบบไปไม่พร้อมกัน นักเรียนคนนี้ทำครบทุกข้อหรือยัง
    Private Function CheckIsClearAllAnswer(ByVal StudentId As String, ByVal QuizId As String) As Boolean
        Dim sql As String = " SELECT CASE WHEN COUNT(DISTINCT CASE WHEN dbo.tblQuizScore.Answer_Id IS NOT NULL THEN dbo.tblQuizScore.QuizScore_Id ELSE NULL END) " & _
                            " <> COUNT(DISTINCT tblTestSetQuestionDetail.Question_Id) THEN 'No' ELSE 'Yes' end  " & _
                            " FROM tblTestSetQuestionSet AS tblTestSetQuestionSet_1 INNER JOIN tblTestSetQuestionDetail " & _
                            " ON tblTestSetQuestionSet_1.TSQS_Id = tblTestSetQuestionDetail.TSQS_Id INNER JOIN tblQuiz " & _
                            " ON tblTestSetQuestionSet_1.TestSet_Id = tblQuiz.TestSet_Id INNER JOIN dbo.tblQuizScore ON dbo.tblQuiz.Quiz_Id = dbo.tblQuizScore.Quiz_Id " & _
                            " WHERE (tblQuiz.Quiz_Id = '" & QuizId & "') AND dbo.tblQuizScore.Student_Id = '" & StudentId & "' AND dbo.tblQuizScore.IsActive = 1 " & _
                            " And tblTestSetQuestionSet_1.IsActive = '1' And tblTestSetQuestionDetail.IsActive = '1'"
        Dim CheckIsClearAll As String = _DB.ExecuteScalar(sql)
        If CheckIsClearAll = "Yes" Then
            Return True
        Else
            Return False
        End If
    End Function

End Class