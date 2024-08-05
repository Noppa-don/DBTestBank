Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports KnowledgeUtils
Imports System.Web.Script.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class TestsetService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function CheckTestsetUseNow(ByVal TestsetId As String) As String
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim sqlCheckTestset As New StringBuilder()
        sqlCheckTestset.Append(" SELECT COUNT(*) AS a FROM tblQuizScore INNER JOIN tblQuiz ON tblQuizScore.Quiz_Id = tblQuiz.Quiz_Id ")
        sqlCheckTestset.Append(" WHERE tblQuizScore.LastUpdate > DATEADD(hour,-1,dbo.GetThaiDate()) AND tblQuiz.IsActive = 1 AND tblQuiz.TestSet_Id = '")
        sqlCheckTestset.Append(TestsetId)
        sqlCheckTestset.Append("' UNION ALL SELECT COUNT(*) FROM tblModuleDetail INNER JOIN tblModuleAssignment ON tblModuleDetail.Module_Id = tblModuleAssignment.Module_Id ")
        sqlCheckTestset.Append(" WHERE dbo.GetThaiDate() BETWEEN tblModuleAssignment.Start_Date AND tblModuleAssignment.End_Date AND tblModuleDetail.IsActive = 1 AND tblModuleDetail.Reference_Id = '")
        sqlCheckTestset.Append(TestsetId)
        sqlCheckTestset.Append("';")

        Dim db As New ClassConnectSql()
        Dim dt As DataTable = db.getdata(sqlCheckTestset.ToString())

        Dim AtThatTimeQuiz As Integer = dt.Rows(0)("a")
        Dim AtThatTimeHomeWork As Integer = dt.Rows(1)("a")

        If AtThatTimeQuiz = 0 And AtThatTimeHomeWork = 0 Then
            Return "ลบชุดที่จัดไว้แน่นะคะ ?"
        ElseIf AtThatTimeQuiz = 0 And AtThatTimeHomeWork > 0 Then
            Return "ชุดนี้กำลังใช้เป็นการบ้านอยู่จำนวน " & AtThatTimeHomeWork & " การบ้าน ยืนยันลบชุดนี้ทันที แน่นะคะ ?"
        ElseIf AtThatTimeQuiz > 0 And AtThatTimeHomeWork = 0 Then
            Return "มีนักเรียนใช้ชุดนี้ทำควิซหรือฝึกฝนอยู่ค่ะ ยืนยันลบชุดนี้ทันที แน่นะคะ ?"
        Else
            Return "ชุดนี้กำลังใช้เป็นการบ้านอยู่จำนวน " & AtThatTimeHomeWork & " การบ้าน และ มีนักเรียนใช้ชุดนี้ทำควิซหรือฝึกฝนอยู่ค่ะ ยืนยันลบชุดนี้ทันที แน่นะคะ ?"
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function UpdateTestSetIdCodeBehind(ByVal TestsetId As String)

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim db As New ClassConnectSql()
        Dim ClsCheckMark As New ClsCheckMark
        ' Dim sqlUpdateTestSetId As String = "Update tblTestSet Set testset_name = 'ถูกลบโดย user แล้ว', IsActive = 0 Where TestSet_Id = '" & TestsetId & "';"
        Dim sqlUpdateTestSetId As String = "Update tblTestSet Set IsActive = 0, Lastupdate = dbo.GetThaiDate(),ClientId = NULL Where TestSet_Id = '" & TestsetId.CleanSQL & "';"
        db.Execute(sqlUpdateTestSetId)
        'CHECKMARK
        'Dim sqlGetReftoCheckMark As String = " SELECT SetupAnswer_ID FROM tblTestSet_CM_temptblsetup WHERE TestSet_Id = '" & TestsetId & "';"
        'Dim refToCheckMark As DataTable = ClsData.getdata(sqlGetReftoCheckMark)
        'ClsCheckMark.updateActiveCheckMarkWhenUserDelTestset(refToCheckMark)
        Return 1
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetFontSize(ByVal FontSize As Integer)

        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        HttpContext.Current.Session("FontSize") = FontSize
        Dim db As New ClassConnectSql()
        Dim sql As String = " UPDATE tblUserSetting SET FontSize = " & FontSize & ",LastUpdate = dbo.GetThaiDate(),ClientId = NULL WHERE User_Id = '" & HttpContext.Current.Session("UserId").ToString() & "';"
        db.Execute(sql)
        Return 1
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function UpdateLayoutConfirmed(ByVal UpdateField As String, ByVal Tick As Boolean, ByVal QuestionId As String) As Boolean
        If Session("UserId") IsNot Nothing Then
            Dim _DB As New ClassConnectSql()
            Dim conn As New SqlConnection()
            Dim sql As String = ""
            _DB.OpenExclusiveConnect(conn)
            Try
                sql = " UPDATE dbo.tblLayoutConfirmed SET " & UpdateField & " = '" & Tick & "' , " & UpdateField & "_UserId = '" & Session("UserId").ToString() & "' , " &
                      " " & UpdateField & "_LastUpdate = dbo.GetThaiDate() , LastUpdate = dbo.GetThaiDate() WHERE Question_Id = '" & QuestionId & "'; "
                _DB.Execute(sql, conn)
                _DB.CloseExclusiveConnect(conn)
                If UpdateField = "WordTechnicalConfirmed" Then
                    Log.Record(Log.LogType.ManageExam, "วิชาการอนุมัติข้อสอบฝั่ง Word (QuestionId=" & QuestionId & ")", True, "", QuestionId)
                ElseIf UpdateField = "QuizTechnicalConfirmed" Then
                    Log.Record(Log.LogType.ManageExam, "วิชาการอนุมัติข้อสอบฝั่ง Quiz (QuestionId=" & QuestionId & ")", True, "", QuestionId)
                ElseIf UpdateField = "WordPrePressConfirmed" Then
                    Log.Record(Log.LogType.ManageExam, "Prepress อนุมัติข้อสอบฝั่ง Word (QuestionId=" & QuestionId & ")", True, "", QuestionId)
                Else
                    Log.Record(Log.LogType.ManageExam, "Prepress อนุมัติข้อสอบฝั่ง Quiz (QuestionId=" & QuestionId & ")", True, "", QuestionId)
                End If
                Return True
            Catch ex As Exception
                _DB.CloseExclusiveConnect(conn)
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SaveLog(ByVal LogText As String)
        Log.Record(Log.LogType.ManageExam, LogText, True)
        Return True
    End Function

    ''' <summary>
    ''' function สำหรับ copy testset ของคนอื่นมาเป็นของตัวเอง
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <param name="newTestsetName"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function CopyOtherTestset(qsetId As String, newTestsetName As String, isQuiz As Boolean, isHomework As Boolean, isPractice As Boolean, isReport As Boolean) As Boolean
        Try
            Dim userId As String = Session("UserId").ToString()
            Dim KnSession As New KNAppSession()
            Dim calendarId As String = KnSession.StoredValue("CurrentCalendarId").ToString()
            Dim schoolId As String = Session("SchoolID").ToString()
            Dim testset As New Testset With {.Id = Guid.NewGuid().ToString(), .Name = newTestsetName, .IsQuizMode = isQuiz,
                .IsHomeworkMode = isHomework, .IsPracticeMode = isPractice, .IsReportMode = isReport}
            Dim ts As New TestsetManagement(userId, calendarId, schoolId, testset)
            Return ts.CopyOtherQsetToMyTestset(qsetId)
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetQSetSelected(subjectId As String, classId As String) As String
        Dim tempTestset As Testset = HttpContext.Current.Session("TempTestset")
        Dim obj As New List(Of Object)
        Dim tc As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(classId, subjectId)
        If tc IsNot Nothing Then
            For Each qset In tc.ListQset
                obj.Add(New With {.qsetId = qset.QsetId})
            Next
        End If
        Dim jsonSerialiser = New JavaScriptSerializer()
        Return jsonSerialiser.Serialize(obj)
    End Function

End Class