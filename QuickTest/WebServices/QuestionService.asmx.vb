Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class QuestionService
    Inherits System.Web.Services.WebService

    Private db As New ClassConnectSql()
    Private jsonSerialiser = New JavaScriptSerializer()

    Private tempTestsetSessionName As String = "TempTestset"
    Dim _DB As New ClassConnectSql()

    <WebMethod(EnableSession:=True)>
    Public Function ReporterProblemQuestion(questionId As String, annotation As String) As Boolean
        Try
            Dim userId As String = HttpContext.Current.Session("UserId").ToString()
            Dim sql As String = " INSERT INTO tblReporterProblemQuestion VALUES (NEWID(),'" & questionId & "','" & annotation & "','" & userId & "',dbo.GetThaiDate(),1);"
            db.OpenWithTransection()
            db.ExecuteResultWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function


    ''' <summary>
    ''' function สร้าง content ให้เลือก qset เรียงตามหนังสือ
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetContentQsetInBook(subjectId As String, classId As String, isWpp As Boolean) As String
        Try
            Dim userId As String = Session("userID").ToString
            Dim tempTestset As Testset = Session("TempTestset")
            Dim bookManagement As New BookManagement(userId)
            ' get qset ทั้งหมด
            Dim dt As DataTable = If((isWpp), bookManagement.GetQsetsWPP(subjectId, classId), bookManagement.GetQsetsByUser(subjectId, classId))

            'Dim dtQsetsInTestset As DataTable = GetQsetsInTestset()

            Dim booksId = (From row In dt.AsEnumerable() Select row.Field(Of Guid)("BookGroup_Id")).Distinct()

            Dim content As New StringBuilder()
            content.Append("<div style='height: 400px;overflow-y: auto;'>")

            For Each item In booksId
                Dim qsets = (From row In dt.AsEnumerable() Where row.Field(Of Guid)("BookGroup_Id") = item)
                Dim bookName As String = qsets(0).Field(Of String)("Book_Name")

                'content &= "<input type='checkbox' id='" & item.ToString & "' /><label style='margin-left:10px;font-weight: bold;font-size: 18px;background-color: antiquewhite;' For='" & item.ToString & "'>" & bookName & "</label><br>"
                content.Append("<label style='margin-left:10px;font-weight: bold;font-size: 18px;background-color: antiquewhite;' >" & bookName & "</label><br>")
                content.Append("<div>")
                For Each q In qsets

                    Dim qsetId As String = q.Field(Of Guid)("QSet_Id").ToString
                    Dim qsetType As EnumQsetType = q.Field(Of Int16)("Qset_Type")
                    Dim qsetName As String = ChangePathImg(q.Field(Of String)("QSet_Name"), qsetId)
                    Dim questionAmount As Integer = If((qsetType = EnumQsetType.Pair Or qsetType = EnumQsetType.Sort), 1, q.Field(Of Int32)("QuestionAmount"))

                    Dim isChecked As String = ""
                    Dim t As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(classId, subjectId)
                    If t IsNot Nothing Then
                        If t.IsTestsetQuestionsetExist(qsetId) Then
                            isChecked = "checked='checked'"
                        End If
                    End If
                    Dim inputValueAttr As String = If((isWpp), subjectId & classId, subjectId & classId & "_user")
                    content.Append("<div>")
                    content.Append("<input type='checkbox' id='" & qsetId & "' " & isChecked & " onclick=""saveQsetToTestset(this, '" & userId & "', '" & qsetId & "', '" & qsetType & "', '" & classId & "','" & subjectId & "'," & isWpp.ToString().ToLower() & ");"" value='" & inputValueAttr & "' questionAmount='" & questionAmount & "' /><label style='margin-left:30px;' For='" & qsetId & "'><b>" & q.Field(Of String)("QCategory_Name") & "</b> - " & qsetName & " <b>(" & questionAmount & " ข้อ)</b></label>")
                    If Not isWpp Then
                        content.Append("<a style='cursor:pointer;' onclick=""editQuestion('" & qsetId & "');"">แก้ไขข้อสอบ</a>")
                        content.Append("<a style='cursor:pointer;margin-left:5px;' onclick=""deleteQset('" & classId & "','" & subjectId & "','" & qsetId & "');"">ลบข้อสอบ</a>")
                    End If

                    If t IsNot Nothing Then
                        Dim tsqs As TestSetQuestionset = t.GetQuestionsetById(qsetId)
                        If tsqs IsNot Nothing Then
                            If Not tsqs.IsSelectedAll Then
                                content.Append("<span style='color:red;margin-left:10px;font-weight:bold;'>ชุดนี้เลือกมาแค่ " & tsqs.QuestionSelectedAmount & " ข้อค่ะ (ถ้าต้องการเลือกทั้งชุด แค่ติ๊กออกแล้วติ๊กเข้าอีกทีค่ะ)</span>")
                            End If
                        End If
                    End If

                    'content.Append("<br>")
                    content.Append("</div>")
                Next
                content.Append("</div>")
            Next
            content.Append("</div>")

            Return content.ToString()
        Catch ex As Exception

            Return ex.ToString() & " ++++ " & ex.Message
        End Try
    End Function

    Private Function GetQsetsInTestset() As DataTable
        Dim sql As String = "Select * from tblTestSetQuestionSet where IsActive = 1 And TestSet_Id = '" & Session("newTestSetId").ToString & "';"
        Return db.getdata(sql)
    End Function

    ''' <summary>
    ''' function ในการ สร้าง content สำหรับการเลือกข้อสอบตาม ดัชนีชี้วัด
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetContentEvalutionIndexInBook(subjectId As String, classId As String) As String
        Dim db As New ClassConnectSql()

        Dim content As String = "<div style='height: 350px;overflow-y: auto;'>"

        'KPA
        content &= "<center>KPA</center>"

        db.OpenWithTransection()
        Dim sql As String = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id = '77E41F99-FD05-4097-B835-34BF09796125' ORDER BY EI_Position;"
        Dim dtKPA As DataTable = db.getdataWithTransaction(sql)
        For Each r In dtKPA.Rows
            content &= "<span>" & r("EI_Code").ToString() & "</span><br>"
            sql = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id = '" & r("EI_Id").ToString & "' ORDER BY EI_Position;"
            Dim dtChildKPA As DataTable = db.getdataWithTransaction(sql)
            For Each r2 In dtChildKPA.Rows
                content &= "<input type='checkbox' id='" & r2("EI_Id").ToString() & "' /><label style='margin-left:30px;' For='" & r2("EI_Id").ToString() & "'>" & r2("EI_Code").ToString & "</label><br>"
            Next
            content &= "<hr>"
        Next


        sql = "SELECT * FROM tblEvaluationIndexNew en INNER JOIN tblEvaluationIndexSubject eis ON en.EI_Id = eis.EI_Id WHERE en.Parent_Id = 'EF957BC3-C463-4315-8847-8B4522CA0100' AND eis.Subject_Id = '" & subjectId & "' ORDER BY EI_Position;"
        Dim dtParentEvalution As DataTable = db.getdataWithTransaction(sql)
        For Each r In dtParentEvalution.Rows
            sql = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id = '" & r("EI_Id").ToString() & "';"
            Dim dtChildEvalution As DataTable = db.getdataWithTransaction(sql)
            For Each r2 In dtChildEvalution.Rows
                sql = "SELECT t.EI_Id,t.EI_Code,t.EI_Name,t.Parent_Id,t.Level_No,t.EI_Position,Count(t.EI_Id) as QuestionAmount "
                sql &= " FROM tblEvaluationIndexNew t  inner join tblQuestionEvaluationIndexItem q On t.EI_Id = q.EI_Id "
                sql &= " WHERE Parent_Id = '" & r2("EI_Id").ToString & "' AND Level_Id = '" & classId & "'  and q.IsActive = 1 "
                sql &= " And q.Question_Id in ( select q.Question_Id from tblQuestionset qs inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id "
                sql &= " inner join tblBook b On qc.Book_Id = b.BookGroup_Id inner join tblQuestion q on qs.QSet_Id = q.QSet_Id "
                sql &= " where b.Level_Id =  '" & classId & "' and GroupSubject_Id =  '" & subjectId & "' and qc.IsActive = 1 and qs.IsActive = 1 and q.IsActive = 1 ) "
                sql &= " Group by t.EI_Id,t.EI_Code,t.EI_Name,t.Parent_Id,t.Level_No,t.EI_Position "
                sql &= " ORDER BY EI_Position;"

                Dim dtLastChildEvalution As DataTable = db.getdataWithTransaction(sql)
                For Each r3 In dtLastChildEvalution.Rows
                    Dim chkboxName As String = String.Format("<b>{0}({1}/{2})</b> {3}  <b>({4} ข้อ)</b>", r2("EI_Code").ToString().Replace("มาตรฐานที่ ", ""), classId.ToLevelShortThaiName(), r3("EI_Code").ToString().Replace(".", ""), r3("EI_Name").ToString(), r3("QuestionAmount"))
                    content &= "<input type='checkbox' id='" & r3("EI_Id").ToString() & "' /><label style='margin-left:30px;' For='" & r3("EI_Id").ToString() & "'>" & chkboxName & "</label><br>"
                Next
            Next
        Next
        db.CommitTransection()
        content &= "</div>"
        Return content
    End Function


    ''' <summary>
    ''' function สำหรับ เอาข้อสอบที่พึ่งสร้าง เอาเข้า tempTestset ก่อนหน้าสรุปดูบันทึก
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SaveMyQsetToTestset(qsetId As String, qsetType As EnumQsetType, subjectId As String, classId As String) As Boolean
        Try
            If HttpContext.Current.Session(tempTestsetSessionName) Is Nothing Then Return False

            Dim userId As String = HttpContext.Current.Session("UserId").ToString()
            Dim tempTestset As Testset = HttpContext.Current.Session(tempTestsetSessionName)

            Dim subjectClassId As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(classId, subjectId)
            If subjectClassId Is Nothing Then
                subjectClassId = New TestsetSubjectClassQuestion With {.SubjectId = subjectId, .ClassId = classId}
                tempTestset.ListSubjectClassQuestion.Add(subjectClassId)
            End If

            If Not subjectClassId.IsTestsetQuestionsetExist(qsetId) Then
                Dim dtQuestions As DataTable = GetTempQuestions(userId, qsetId, EnumAddBy.Qset, classId, subjectId)
                subjectClassId.AddTestsetQuestionsetWithQuestion(qsetId, qsetType, dtQuestions, userId, False)
            End If

            HttpContext.Current.Session(tempTestsetSessionName) = tempTestset
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function RemoveQuestionsTestset(addById As String, qSetId As String, classId As String, subjectId As String, isWpp As Boolean) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then Return 0

        If HttpContext.Current.Session(tempTestsetSessionName) Is Nothing Then Return False

        Dim tempTestset As Testset = HttpContext.Current.Session(tempTestsetSessionName)
        Dim subjectClassId As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(classId, subjectId)
        If subjectClassId IsNot Nothing Then
            subjectClassId.RemoveTestsetQuestionset(qSetId)
        End If

        HttpContext.Current.Session(tempTestsetSessionName) = tempTestset
        Return True
    End Function

    ''' <summary>
    ''' function save ข้อสอบลง temp object ก่อน
    ''' </summary>
    ''' <param name="addById"></param>
    ''' <param name="qsetId"></param>
    ''' <param name="classId"></param>
    ''' <param name="subjectId"></param>
    ''' <returns>Boolean</returns>
    <WebMethod(EnableSession:=True)>
    Public Function AddQuestionsTestset(addById As String, qSetId As String, qsetType As EnumQsetType, classId As String, subjectId As String, isWpp As Boolean) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then Return 0

        Dim tempTestset As Testset = HttpContext.Current.Session(tempTestsetSessionName)

        Dim subjectClassId As TestsetSubjectClassQuestion = tempTestset.GetSubjectClassQuestion(classId, subjectId)
        If subjectClassId Is Nothing Then
            subjectClassId = New TestsetSubjectClassQuestion With {.SubjectId = subjectId, .ClassId = classId}
            tempTestset.ListSubjectClassQuestion.Add(subjectClassId)
        End If

        If Not subjectClassId.IsTestsetQuestionsetExist(qSetId) Then
            Dim dtQuestions As DataTable = GetTempQuestions(addById, qSetId, EnumAddBy.Qset, classId, subjectId)
            subjectClassId.AddTestsetQuestionsetWithQuestion(qSetId, qsetType, dtQuestions, addById, isWpp)
        End If

        HttpContext.Current.Session(tempTestsetSessionName) = tempTestset
        Return True
    End Function

    ''' <summary>
    ''' function สำหรับ set Isactive กับข้อสอบที่เป็น แบบ ปรนัยและถูกผิด
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <param name="qsetId"></param>
    ''' <param name="questionId"></param>
    ''' <param name="isActive"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SetQuestionIsActive(subjectId As String, classId As String, qsetId As String, questionId As String, isActive As Boolean) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then Return 0

        Dim tempTestset As Testset = HttpContext.Current.Session(tempTestsetSessionName)

        Dim t As TestsetSubjectClassQuestion = tempTestset.ListSubjectClassQuestion.Where(Function(f) f.SubjectId = subjectId And f.ClassId = classId).SingleOrDefault()
        If t IsNot Nothing Then
            Dim qset As TestSetQuestionset = t.GetQuestionsetById(qsetId)
            Dim question As TestsetQuestion = qset.GetQuestionById(questionId)
            question.IsActive = isActive

            HttpContext.Current.Session(tempTestsetSessionName) = tempTestset

            Return True
        End If
        Return False
    End Function


    ''' <summary>
    ''' function สำหรับ set Isactive กับข้อสอบที่เป็น แบบ เรียงลำดับและจับคู่
    ''' </summary>
    ''' <param name="subjectId"></param>
    ''' <param name="classId"></param>
    ''' <param name="qsetId"></param>
    ''' <param name="isActive"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SetQuestionsIsActive(subjectId As String, classId As String, qsetId As String, isActive As Boolean) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then Return 0

        Dim tempTestset As Testset = HttpContext.Current.Session(tempTestsetSessionName)

        Dim t As TestsetSubjectClassQuestion = tempTestset.ListSubjectClassQuestion.Where(Function(f) f.SubjectId = subjectId And f.ClassId = classId).SingleOrDefault()
        If t IsNot Nothing Then
            Dim qset As TestSetQuestionset = t.GetQuestionsetById(qsetId)
            For Each question In qset.ListQuestion
                question.IsActive = isActive
            Next
            HttpContext.Current.Session(tempTestsetSessionName) = tempTestset
            Return True
        End If
        Return False
    End Function

    Private Function GetTempQuestions(addById As String, qsetId As String, addBy As EnumAddBy, classId As String, subjectId As String) As DataTable
        Select Case addBy
            Case EnumAddBy.Qset
                Return GetQuestionsFromQset(qsetId)
            Case EnumAddBy.KPA
                Return GetQuestionsFromEvalution(addById, subjectId, classId)
            Case EnumAddBy.Evalution
                Return GetQuestionsFromEvalution(addById, subjectId, classId)
        End Select
        Return New DataTable
    End Function

    Private Function GetQuestionsFromQset(qsetId As String) As DataTable
        Dim sql As String = "select * from tblQuestion q inner join tblQuestionset qs on q.QSet_Id = qs.QSet_Id where q.IsActive = 1 and qs.QSet_Id = '" & qsetId & "' order by q.Question_No;"
        Return db.getdata(sql)
    End Function

    Private Function GetQuestionsFromEvalution(eiId As String, subjectId As String, levelId As String) As DataTable
        Dim sql As String = "select qs.*,q.*,qc.QCategory_Id,b.Book_Name from tblQuestionEvaluationIndexItem qi inner join tblQuestion q on qi.Question_Id = q.Question_Id "
        sql &= " inner join tblQuestionset qs On qs.QSet_Id = q.QSet_Id inner join tblQuestionCategory qc On qs.QCategory_Id = qc.QCategory_Id "
        sql &= " inner join tblBook b on qc.Book_Id = b.BookGroup_Id "
        sql &= " where qi.EI_Id = '" & eiId & "' and qi.IsActive = 1 and b.IsActive = 1 and b.GroupSubject_Id = '" & subjectId & "' and b.Level_Id = '" & levelId & "';"
        Return db.getdata(sql)
    End Function


#Region "Add New Questions By User"
#End Region
    <WebMethod(EnableSession:=True)>
    Public Function AddQuestionCategory(ByVal objStr As String) As QuestionCategory
        Try
            Dim userId As String = HttpContext.Current.Session("UserId").ToString()
            Dim questionCategory As QuestionCategory = New JavaScriptSerializer().Deserialize(Of QuestionCategory)(objStr)
            questionCategory.Id = Guid.NewGuid().ToString()

            db.OpenWithTransection()
            Dim sql As String = "SELECT * FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id =  '" & userId & "' AND Book_Id =  '" & questionCategory.BookId & "' AND QCategory_Name =  '" & questionCategory.Name & "';"
            Dim dtQuestionCategory As DataTable = db.getdataWithTransaction(sql)
            If dtQuestionCategory.Rows.Count = 0 Then
                sql = "SELECT COUNT(*) FROM tblQuestionCategory WHERE IsWpp = 0 and Parent_Id =  '" & userId & "' AND Book_Id =  '" & questionCategory.BookId & "';"
                Dim qcatNo As Integer = CInt(db.ExecuteScalarWithTransection(sql)) + 1
                sql = "INSERT INTO tblQuestionCategory VALUES ( '" & questionCategory.Id & "',NULL, '" & userId & "', '" & questionCategory.BookId & "',1," & qcatNo & ", '" & questionCategory.Name & "',NULL,NULL,1,NULL,NULL,0,dbo.GetThaiDate(),NULL);"
                db.ExecuteScalarWithTransection(sql)
            Else
                questionCategory = Nothing
            End If
            db.CommitTransection()
            Return questionCategory
        Catch ex As Exception
            db.RollbackTransection()
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' function สำหรับ add qset ลง datatable
    ''' </summary>
    ''' <param name="questionCategoryId"></param>
    ''' <param name="objStr"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function AddQuestionSet(ByVal questionCategoryId As String, ByVal objStr As String) As QuestionSet
        Try
            Dim questionSet As QuestionSet = New JavaScriptSerializer().Deserialize(Of QuestionSet)(objStr)
            questionSet.Id = Guid.NewGuid().ToString()
            db.OpenWithTransection()
            Dim qsetNo As Integer = CInt(db.ExecuteScalarWithTransection("SELECT COUNT(*) FROM tblQuestionset WHERE QCategory_Id = '" & questionCategoryId & "';")) + 1
            Dim sql As String = "INSERT INTO tblQuestionset VALUES ( '" & questionSet.Id & "', '" & questionCategoryId & "',NULL," & qsetNo & ", '" & questionSet.Name & "'," & questionSet.Type & ",0,1,1,1,1,NULL,0,0,dbo.GetThaiDate(),NULL, '" & questionSet.Name & "');"
            db.ExecuteScalarWithTransection(sql)
            db.CommitTransection()
            Return questionSet
        Catch ex As Exception
            db.RollbackTransection()
            Return Nothing
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function AddQuestionCategoryAndQuestionSet(ByVal objStr As String) As Boolean
        Try
            Dim obj As QuestionCategoryObj = New JavaScriptSerializer().Deserialize(Of QuestionCategoryObj)(objStr)
            db.OpenWithTransection()
            Dim sql As String = "INSERT INTO tblQuestionset VALUES (NEWID(), '" & obj.QuestionCategoryId & "',NULL,1, '" & obj.QsetName & "',{obj.QsetType},0,1,1,1,1,NULL,0,0,dbo.GetThaiDate(),NULL, '" & obj.QsetName & "');"
            'obj.QuestionCategoryId = If((obj.QuestionCategoryId = ""), NewQuestionCategory(obj), obj.QuestionCategoryId)
            db.ExecuteScalarWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function UpdateQuestionCategoryAndQuestionSet(ByVal objStr As String) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = ""
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Function NewQuestionCategory(obj As QuestionCategoryObj) As String
        Dim userId As String = HttpContext.Current.Session("UserId").ToString()
        Dim qCategoryId As String = db.ExecuteScalarWithTransection("SELECT NEWID();")
        Dim sql As String = "INSERT INTO tblQuestionCategory VALUES ( '" & qCategoryId & "',NULL, '" & userId & "', '" & obj.BookId & "',1,1, '" & obj.QuestionCategoryName & "',NULL,NULL,1,NULL,NULL,0,dbo.GetThaiDate(),NULL);"
        db.ExecuteScalarWithTransection(sql)
        Return qCategoryId
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function InitialQuestion()
        Dim questions As List(Of Question) = HttpContext.Current.Session("Questions")
        Dim currentQuestion As Question
        If questions Is Nothing Then
            currentQuestion = New Question()
            questions = New List(Of Question)
            questions.Add(currentQuestion)
        Else
            currentQuestion = questions.Item(0)
        End If

        HttpContext.Current.Session("Questions") = questions
        Return currentQuestion
    End Function

    <WebMethod()>
    Public Function GetQuestions(ByVal qsetId As String) As String
        Try
            db.OpenWithTransection()
            Dim sql As String '=  "SELECT * FROM tblQuestion WHERE QSet_Id =  '" & qsetId & "' AND Question_No =  '" & questionNo & "';"
            sql = "select * from tblQuestion q inner join tblAnswer a on q.Question_Id = a.Question_Id where q.QSet_Id =  '" & qsetId & "' Order by Answer_No;"
            Dim dt As DataTable = db.getdataWithTransaction(sql)
            Dim question As Question
            If dt.Rows.Count = 0 Then
                question = New Question()
                sql = "INSERT INTO tblQuestion VALUES( '" & question.Id & "','qsetid',NULL,NULL,'1', '" & question.Name & "', '" & question.ExplainName & "',1,0, '" & question.Name & "',dbo.GetThaiDate(),1,NULL,0, '" & question.Name & "', '" & question.ExplainName & "');"
                db.ExecuteScalarWithTransection(sql)
            Else
                question = New Question With {.Id = dt.Rows(0)("Question_Id").ToString(), .Name = dt.Rows(0)("Question_Name_Quiz"), .ExplainName = dt.Rows(0)("Question_Expain_Quiz")}
                Dim answers As New List(Of Answer)
                For Each r In dt.Rows
                    Dim answer As New Answer With {.Id = r("Answer_Id").ToString(), .Name = r("Answer_Name_Quiz"), .ExplainName = r("Answer_Expain_Quiz"), .No = r("Answer_No"), .Score = r("Answer_Score"), .QsetId = r("Qset_Id").ToString(), .QuestionId = r("Question_Id").ToString()}
                    answers.Add(answer)
                Next
                question.Answers = answers
            End If
            db.CommitTransection()
            Return "question"
        Catch ex As Exception
            db.RollbackTransection()
            Return ""
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetQuestionSet(qsetId As String) As String
        Try
            Dim userId As String = HttpContext.Current.Session("UserId").ToString()
            Dim questionSet As New QuestionSet
            Dim sql As String = "SELECT * FROM tblQuestionset qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id INNER JOIN tblBook b ON qc.Book_Id = b.BookGroup_Id WHERE qs.QSet_Id = '" & qsetId & "' AND qc.Parent_Id = '" & userId & "' AND qs.IsWpp = 0;"
            Dim dt As DataTable = db.getdata(sql)
            If dt.Rows.Count > 0 Then
                questionSet.Id = dt.Rows(0)("QSet_Id").ToString().ToLower()
                questionSet.Name = ChangePathImg(dt.Rows(0)("QSet_Name").ToString(), dt.Rows(0)("QSet_Id").ToString())
                questionSet.Type = dt.Rows(0)("QSet_Type").ToString()
                questionSet.TypeName = GetQsetTypeName(questionSet.Type)
                questionSet.Questions = GetQuestions(questionSet)
            End If
            Return jsonSerialiser.Serialize(questionSet)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function GetQsetTypeName(qsetType As Integer)
        Select Case qsetType
            Case 1
                Return "ปรนัย"
            Case 2
                Return "ถูกผิด"
            Case 3
                Return "จับคู่"
            Case 6
                Return "เรียงลำดับ"
            Case Else
                Return "อัตนัย"
        End Select
    End Function

    Private Function GetQuestions(questionSet As QuestionSet) As List(Of QuestionService.Question)
        Try
            Dim questionManagement As New QuestionManagement(questionSet.Id, questionSet.Type)
            Dim dt As DataTable = questionManagement.GetQuestionsInQset()
            Dim questions As New List(Of Question)
            If dt.Rows.Count = 0 Then
                Dim question As Question = questionManagement.NewQuestion()
                questions.Add(question)
            Else
                For Each r In dt.Rows
                    Dim qsetId As String = r("Qset_Id").ToString()
                    Dim question As New Question With {
                        .Id = r("Question_Id").ToString(),
                        .Name = ChangePathImg(r("Question_Name_Quiz"), qsetId),
                        .ExplainName = ChangePathImg(r("Question_Expain_Quiz"), qsetId),
                        .No = r("Question_No")}
                    Dim dtAnswers As DataTable = questionManagement.GetAnswersInQuestion(question.Id)
                    Dim answers As New List(Of Answer)
                    For Each ans In dtAnswers.Rows
                        Dim answer As New Answer With {
                            .Id = ans("Answer_Id").ToString(),
                            .Name = ChangePathImg(ans("Answer_Name_Quiz"), qsetId),
                            .ExplainName = ChangePathImg(ans("Answer_Expain_Quiz"), qsetId),
                            .No = ans("Answer_No"),
                            .Score = ans("Answer_Score"),
                            .QsetId = qsetId,
                            .QuestionId = question.Id}
                        answers.Add(answer)
                    Next
                    question.Answers = answers
                    questions.Add(question)
                Next
            End If
            Return questions
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' function ในการ get question จากปุ่มเลือกข้อสีเขียว
    ''' </summary>
    ''' <param name="questionId"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function GetQuestion(questionId As String) As String
        Try
            db.OpenWithTransection()
            Dim sql As String = "SELECT * FROM tblQuestion q INNER JOIN tblAnswer a ON q.Question_Id = a.Question_Id WHERE q.Question_Id =  '" & questionId & "' ORDER BY a.Answer_No;"
            Dim dt As DataTable = db.getdataWithTransaction(sql)
            Dim qsetId As String = dt.Rows(0)("Qset_Id").ToString()
            Dim question As New Question With {
                .Id = dt.Rows(0)("Question_Id").ToString(),
                .Name = ChangePathImg(dt.Rows(0)("Question_Name_Quiz").ToString(), qsetId),
                .ExplainName = ChangePathImg(dt.Rows(0)("Question_Expain_Quiz").ToString(), qsetId),
                .No = dt.Rows(0)("Question_No")}
            Dim answers As New List(Of Answer)
            For Each r In dt.Rows
                Dim answer As New Answer With {
                    .Id = r("Answer_Id").ToString(),
                    .Name = ChangePathImg(r("Answer_Name_Quiz").ToString(), qsetId),
                    .ExplainName = ChangePathImg(r("Answer_Expain_Quiz").ToString(), qsetId),
                    .No = r("Answer_No"),
                    .Score = r("Answer_Score"),
                    .QsetId = r("Qset_Id").ToString(),
                    .QuestionId = r("Question_Id").ToString()}
                answers.Add(answer)
            Next
            question.Answers = answers
            db.CommitTransection()
            Return jsonSerialiser.Serialize(question)
        Catch ex As Exception
            db.RollbackTransection()
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' function ในการแปลง pathimg จาก ___MODULE_URL___ เป็นตาม format ของ qset ต้อง encode ด้วย เพื่อให้ฝั่ง javascript ทำงานได้
    ''' </summary>
    ''' <param name="txtName"></param>
    ''' <param name="qsetId"></param>
    ''' <returns></returns>
    Private Function ChangePathImg(txtName As String, qsetId As String) As String
        ' Return Web.HttpUtility.UrlEncode(txtName.Replace("___MODULE_URL___", qsetId.ToFolderFilePath()))
        Return txtName.Replace("___MODULE_URL___", qsetId.ToFolderFilePath())
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function NewQuestion(qsetId As String, qsetType As Integer) As String
        Dim questionManagement As New QuestionManagement(qsetId, qsetType)
        Dim question As Question = questionManagement.NewQuestion()
        Dim jsonSerialiser = New JavaScriptSerializer()
        Return jsonSerialiser.Serialize(question)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function DeleteQuestion(questionId As String, qsetId As String, qsetType As Integer)
        Dim questionManagement As New QuestionManagement(qsetId, qsetType)
        Return questionManagement.DeleteQuestion(questionId)
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SaveQuestion(objStr As String) As Boolean
        Try
            Dim question As Question = New JavaScriptSerializer().Deserialize(Of Question)(objStr)
            Dim pathImg As String = Session("QsetImagePath").ToString().ToFolderFilePath()
            question.Name = Web.HttpUtility.UrlDecode(question.Name)
            question.Name = question.Name.Replace(pathImg, "___MODULE_URL___")
            question.ExplainName = Web.HttpUtility.UrlDecode(question.ExplainName)
            question.ExplainName = question.ExplainName.Replace(pathImg, "___MODULE_URL___")
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestion Set Question_Name =  '" & question.Name & "',Question_Expain =  '" & question.ExplainName & "',Question_Name_Quiz =  '" & question.Name & "',Question_Expain_Quiz =  '" & question.ExplainName & "',Question_Name_Backup =  '" & question.Name & "',LastUpdate = dbo.GetThaiDate() WHERE Question_Id =  '" & question.Id.ToString() & "';"
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function


    ''' <summary>
    ''' function save ชื่อ qset (สำหรับข้อสอบแบบจับคู่ และเรียงลำดับ)
    ''' </summary>
    ''' <param name="objStr"></param>
    ''' <returns>Boolean</returns>
    <WebMethod(EnableSession:=True)>
    Public Function SaveQuestionset(objStr As String) As Boolean
        Try
            Dim qset As QuestionSet = New JavaScriptSerializer().Deserialize(Of QuestionSet)(objStr)
            Dim pathImg As String = Session("QsetImagePath").ToString().ToFolderFilePath()
            qset.Name = Web.HttpUtility.UrlDecode(qset.Name)
            qset.Name = qset.Name.Replace(pathImg, "___MODULE_URL___")
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestionset SET QSet_Name = '" & qset.Name & "',QSet_Name_Quiz = '" & qset.Name & "' WHERE QSet_Id = '" & qset.Id & "';"
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    ''' <summary>
    ''' function ในการ save คำอธิบายคำถาม สำหรับข้อสอบที่เป็นแบบ จับคู่และเรียงลำดับ
    ''' </summary>
    ''' <param name="objStr"></param>
    ''' <param name="questionExplain"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SaveQuestionExplain(objStr As String, questionExplain As String)
        Try
            Dim qset As QuestionSet = New JavaScriptSerializer().Deserialize(Of QuestionSet)(objStr)
            Dim pathImg As String = Session("QsetImagePath").ToString().ToFolderFilePath()
            questionExplain = Web.HttpUtility.UrlDecode(questionExplain)
            questionExplain = questionExplain.Replace(pathImg, "___MODULE_URL___")
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestion SET Question_Expain = '" & questionExplain & "',Question_Expain_Quiz = '" & questionExplain & "' WHERE QSet_Id = '" & qset.Id & "';"
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod()>
    Public Function NewAnswer(questionId As String, qsetId As String, answerNo As Integer) As Answer
        Try
            Dim answer As New Answer With {.QuestionId = questionId, .QsetId = qsetId, .Score = 0, .No = answerNo}
            Dim sql As String = "INSERT INTO tblAnswer VALUES( '" & answer.Id & "', '" & answer.QuestionId & "',NULL, '" & answer.QsetId & "', '" & answer.No & "', '" & answer.Name & "', '" & answer.ExplainName & "', '" & answer.Score & "',0,NULL,1, '" & answer.Name & "',dbo.GetThaiDate(),NULL,0,NULL, '" & answer.Name & "', '" & answer.ExplainName & "');"
            db.OpenWithTransection()
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return answer
        Catch ex As Exception
            db.RollbackTransection()
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' function ในการสร้างคำถามและคำตอบ แบบจับคู่และเรียงลำดับ
    ''' </summary>
    ''' <param name="qsetId"></param>
    ''' <param name="qsetType"></param>
    ''' <returns></returns>
    <WebMethod()>
    Public Function NewQuestionAnswer(qsetId As String, qsetType As EnumQsetType) As String
        Try
            Dim QuestionManagement As New QuestionManagement(qsetId, qsetType)
            Dim dt As DataTable = QuestionManagement.GetQuestionsInQset()
            'Dim questions As New List(Of QuestionService.Question)
            Dim question As QuestionService.Question = QuestionManagement.NewQuestion()
            Return jsonSerialiser.Serialize(question)
        Catch ex As Exception
            db.RollbackTransection()
            Return Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function SaveReOrderQuestionAnswer(qsetId As String, qsetType As EnumQsetType, tempQuestions As String) As String
        Try
            db.OpenWithTransection()
            Dim i As Integer = 1
            For Each question In tempQuestions.Split(",")
                Dim sql As String = "UPDATE tblQuestion SET Question_No = {i},LastUpdate = dbo.GetThaiDate() WHERE QSet_Id =  '" & qsetId & "' AND Question_Id =  '" & question & "';"
                db.ExecuteScalarWithTransection(sql)
                sql = "UPDATE tblAnswer SET Answer_No = {i},LastUpdate = dbo.GetThaiDate() WHERE QSet_Id =  '" & qsetId & "' AND Question_Id =  '" & question & "';"
                db.ExecuteScalarWithTransection(sql)
                i += 1
            Next
            db.CommitTransection()

            Dim questionManagement As New QuestionManagement(qsetId, qsetType)
            Dim dt As DataTable = questionManagement.GetQuestionsInQset()
            Dim questions As New List(Of QuestionService.Question)

            For Each r In dt.Rows
                Dim question As New QuestionService.Question With {.Id = r("Question_Id").ToString(), .Name = r("Question_Name_Quiz"), .ExplainName = r("Question_Expain_Quiz"), .No = r("Question_No")}

                Dim dtAnswers As DataTable = questionManagement.GetAnswersInQuestion(question.Id)
                Dim answers As New List(Of QuestionService.Answer)
                For Each ans In dtAnswers.Rows
                    Dim answer As New QuestionService.Answer With {.Id = ans("Answer_Id").ToString(), .Name = ans("Answer_Name_Quiz"), .ExplainName = ans("Answer_Expain_Quiz"), .No = ans("Answer_No"), .Score = ans("Answer_Score"), .QsetId = qsetId, .QuestionId = question.Id}
                    answers.Add(answer)
                Next
                question.Answers = answers
                questions.Add(question)
            Next

            Dim jsonSerialiser = New JavaScriptSerializer()
            Return jsonSerialiser.Serialize(questions)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' function ในการ save คำตอบ
    ''' </summary>
    ''' <param name="objStr"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SaveAnswer(objStr As String) As Boolean
        Try
            Dim answer As Answer = New JavaScriptSerializer().Deserialize(Of Answer)(objStr)
            Dim pathImg As String = Session("QsetImagePath").ToString().ToFolderFilePath()
            answer.Name = Web.HttpUtility.UrlDecode(answer.Name)
            answer.Name = answer.Name.Replace(pathImg, "___MODULE_URL___")
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblAnswer SET Answer_Name =  '" & answer.Name & "',Answer_Name_Quiz =  '" & answer.Name & "',Answer_Name_Bkup =  '" & answer.Name & "',LastUpdate = dbo.GetThaiDate() WHERE Answer_Id =  '" & answer.Id & "';" ',Answer_No =  '" & answer.No & "'
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    ''' <summary>
    ''' function save คำอธิบายคำตอบ
    ''' </summary>
    ''' <param name="objStr"></param>
    ''' <returns></returns>
    <WebMethod(EnableSession:=True)>
    Public Function SaveAnswerExplain(objStr As String) As Boolean
        Try
            Dim answer As Answer = New JavaScriptSerializer().Deserialize(Of Answer)(objStr)
            Dim pathImg As String = Session("QsetImagePath").ToString().ToFolderFilePath()
            answer.ExplainName = Web.HttpUtility.UrlDecode(answer.ExplainName)
            answer.ExplainName = answer.ExplainName.Replace(pathImg, "___MODULE_URL___")
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblAnswer SET Answer_Expain =  '" & answer.ExplainName & "',Answer_Expain_Quiz =  '" & answer.ExplainName & "',LastUpdate = dbo.GetThaiDate() WHERE Answer_Id =  '" & answer.Id & "';"
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod()>
    Public Function DeleteQuestionAnswer(qsetId As String, qsetType As EnumQsetType, questionId As String) As Boolean
        Try
            Dim questionManagement As New QuestionManagement(qsetId, qsetType)
            questionManagement.DeleteQuestionAnswer(questionId)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()>
    Public Function DeleteAnswer(questionId As String, qsetId As String, answerId As String) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = "SELECT * FROM tblAnswer WHERE Question_Id =  '" & questionId & "' AND QSet_Id =  '" & qsetId & "' ORDER BY Answer_No;"
            Dim dtAnswer As DataTable = db.getdataWithTransaction(sql)
            Dim isReOrder As Boolean = False

            For Each r In dtAnswer.Rows
                If isReOrder Then
                    sql = "UPDATE tblAnswer SET Answer_No = Answer_No - 1 WHERE Question_Id =  '" & questionId & "' AND QSet_Id =  '" & qsetId & "' AND Answer_Id =  '" & r("Answer_Id").ToString() & "';"
                    db.ExecuteWithTransection(sql)
                End If
                If r("Answer_Id").ToString().ToLower() = answerId.ToLower() Then
                    isReOrder = True
                    sql = "DELETE tblAnswer WHERE Answer_Id =  '" & answerId & "';"
                    db.ExecuteWithTransection(sql)
                End If
            Next
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod()>
    Public Function SaveRightAnswer(questionId As String, answerId As String, isScored As Boolean) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblAnswer SET Answer_Score = 0,LastUpdate = dbo.GetThaiDate() WHERE Question_Id =  '" & questionId & "';"
            db.ExecuteScalarWithTransection(sql)
            If isScored Then
                sql = "UPDATE tblAnswer SET Answer_Score = 1,LastUpdate = dbo.GetThaiDate() WHERE Question_Id =  '" & questionId & "' AND Answer_Id =  '" & answerId & "'; "
                db.ExecuteScalarWithTransection(sql)
            End If
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function GetHtmlEvalutionIndexItems(QuestionId As String, GroupSubjectId As String, LevelId As String) As String
        db.OpenWithTransection()
        Dim sql As String = "SELECT * FROM tblEvaluationIndexNew WHERE EI_Name IS NULL AND Parent_Id IS NULL ORDER BY EI_Position;"
        Dim htmlEvalutionIndex As New StringBuilder()
        Dim dtEvalutionParents As DataTable = db.getdataWithTransaction(sql)

        sql = "SELECT * FROM tblQuestionEvaluationIndexItem WHERE Question_Id =  '" & QuestionId & "' AND IsActive = 1;"
        Dim dtQuestionEvalutions As DataTable = db.getdataWithTransaction(sql)

        htmlEvalutionIndex.Append("<div style='width:600px;height:350px;overflow-y:auto;'>")

        For Each r In dtEvalutionParents.Rows
            htmlEvalutionIndex.Append("<span style='font-weight: bold;'>" & r("EI_Code") & "</span><hr/>")

            If r("EI_Id").ToString().ToUpper() = "EF957BC3-C463-4315-8847-8B4522CA0100" Then
                sql = "SELECT * FROM tblEvaluationIndexNew en INNER JOIN tblEvaluationIndexSubject eis ON en.EI_Id = eis.EI_Id WHERE en.Parent_Id =  '" & r("EI_Id").ToString() & "' AND eis.Subject_Id =  '" & GroupSubjectId & "' ORDER BY EI_Position;"
                Dim dtEvalutionChilds As DataTable = db.getdataWithTransaction(sql)

                For Each r2 In dtEvalutionChilds.Rows
                    htmlEvalutionIndex.Append("<span style='margin-left:10px;'>" & r2("EI_Code") & r2("EI_Name") & "</span><br>")

                    sql = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id =  '" & r2("EI_Id").ToString() & "';"
                    Dim dtEvalutionPreLastChilds As DataTable = db.getdataWithTransaction(sql)
                    For Each r3 In dtEvalutionPreLastChilds.Rows
                        htmlEvalutionIndex.Append("<span style='margin-left:20px;'>" & r3("EI_Code") & " " & r3("EI_Name") & "</span><br>")

                        sql = " SELECT * FROM tblEvaluationIndexNew  WHERE Parent_Id =  '" & r3("EI_Id").ToString() & "' AND Level_Id =  '" & LevelId & "'  ORDER BY EI_Position;"
                        Dim dtEvalutionLastChilds As DataTable = db.getdataWithTransaction(sql)
                        For Each r4 In dtEvalutionLastChilds.Rows
                            Dim result = (From q In dtQuestionEvalutions.AsEnumerable() Where q.Field(Of Guid)("EI_Id") = r4("EI_Id")).SingleOrDefault()
                            Dim checked As String = If((result Is Nothing), "", "checked='checked'")
                            htmlEvalutionIndex.Append("<input type='checkbox' {checked} id= '" & r4("EI_Id").ToString() & "' /><label style='margin-left:40px;' For= '" & r4("EI_Id").ToString() & "'>" & r4("EI_Code") & " " & r4("EI_Name") & "</label><br>")
                        Next
                    Next
                Next
            Else
                sql = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id =  '" & r("EI_Id").ToString() & "' ORDER BY EI_Position;"
                Dim dt As DataTable = db.getdataWithTransaction(sql)
                For Each r2 In dt.Rows
                    htmlEvalutionIndex.Append("<span style='margin-left:10px;'>" & r2("EI_Code") & " " & r2("EI_Name") & "</span><br>")

                    sql = "SELECT * FROM tblEvaluationIndexNew WHERE Parent_Id =  '" & r2("EI_Id").ToString() & "' ORDER BY EI_Position;"
                    Dim dt2 As DataTable = db.getdataWithTransaction(sql)
                    For Each r3 In dt2.Rows
                        Dim result = (From q In dtQuestionEvalutions.AsEnumerable() Where q.Field(Of Guid)("EI_Id") = r3("EI_Id")).SingleOrDefault()
                        Dim checked As String = If((result Is Nothing), "", "checked='checked'")
                        If r("EI_Id").ToString().ToUpper() = "BA3A748E-AA31-473C-B037-1E1046EC983B" Then
                            htmlEvalutionIndex.Append("<input type='checkbox' {checked} id= '" & r3("EI_Id").ToString() & "' /><label style='margin-left:25px;' For= '" & r3("EI_Id").ToString() & "'>" & r3("EI_Code") & " " & r3("EI_Name") & "</label>")
                        ElseIf r("EI_Id").ToString().ToUpper() = "DEDFBF9F-C8F5-493B-97F6-6741BEB09EB4" Then
                            htmlEvalutionIndex.Append("<input type='radio' {checked} id= '" & r3("EI_Id").ToString() & "' style='margin-left:40px;' name='difficulty'  /><label For= '" & r3("EI_Id").ToString() & "'>" & r3("EI_Code") & " " & r3("EI_Name") & "</label>")
                        Else
                            htmlEvalutionIndex.Append("<input type='checkbox' {checked} id= '" & r3("EI_Id").ToString() & "' /><label style='margin-left:40px;' For= '" & r3("EI_Id").ToString() & "'>" & r3("EI_Code") & " " & r3("EI_Name") & "</label><br>")
                        End If
                    Next
                    If r("EI_Id").ToString().ToUpper() = "BA3A748E-AA31-473C-B037-1E1046EC983B" Then
                        htmlEvalutionIndex.Append("<br>")
                    End If
                Next
            End If
        Next
        htmlEvalutionIndex.Append("</div>")
        db.CommitTransection()

        Return htmlEvalutionIndex.ToString()
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetQuestionEvalutionItem(QuestionId As String, EIId As String, IsActive As Boolean) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestionEvaluationIndexItem SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Question_Id =  '" & QuestionId & "' AND EI_Id =  '" & EIId & "';"
            db.ExecuteWithTransection(sql)
            If IsActive Then
                sql = "INSERT INTO tblQuestionEvaluationIndexItem VALUES(NEWID(), '" & EIId & "', '" & QuestionId & "',1,1,dbo.GetThaiDate(),0,NULL);"
                db.ExecuteWithTransection(sql)
            End If
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function SetQuestionDifficulty(QuestionId As String, EIId As String) As Boolean
        Try
            db.OpenWithTransection()
            Dim sql As String = "UPDATE tblQuestionEvaluationIndexItem SET IsActive = 0,LastUpdate = dbo.GetThaiDate() WHERE Question_Id =  '" & QuestionId & "' AND EI_Id IN ('5FFC0532-A7A4-4611-A5D3-1112162BA003','4400D235-0A13-479D-9D0B-16E99CEED017','05DD9F7D-E5CB-4A88-94B5-A1716C427812','A3B6A5AC-E195-4120-9915-B4BFC9335716','C26588C7-FC7B-4B5D-B462-CA16DB7DB8FE');"
            db.ExecuteWithTransection(sql)

            sql = "INSERT INTO tblQuestionEvaluationIndexItem VALUES(NEWID(), '" & EIId & "', '" & QuestionId & "',1,1,dbo.GetThaiDate(),0,NULL);"
            db.ExecuteWithTransection(sql)
            db.CommitTransection()
            Return True
        Catch ex As Exception
            db.RollbackTransection()
            Return False
        End Try
    End Function

    Private Class QuestionCategoryObj
        Public Property BookId As String = ""
        Public Property QuestionCategoryId As String = ""
        Public Property QuestionCategoryName As String = ""
        Public Property QsetName As String = ""
        Public Property QsetType As String = ""
    End Class

    Public Class Book
        Public Property Id As String
        Public Property Name As String

        Public Property GroupSubjectId As String
        Public Property LevelId As String

        Public Property QuestionCategories As IEnumerable(Of QuestionCategory)

        Public Overrides Function ToString() As String
            Return GroupSubjectId.ToGroupSubjectThName & " วิชา " & GroupSubjectId.ToSubjectBookThName & " " & LevelId.ToLevelShortName
        End Function
    End Class

    Public Class QuestionCategory
        Public Property Id As String
        Public Property Name As String
        Public Property BookId As String
        'Public Property QuestionSets As List(Of QuestionSet)
    End Class

    Public Class QuestionSet
        Public Property Id As String
        Public Property Name As String
        Public Property Type As Integer
        Public Property TypeName As String
        Public Property Questions As List(Of Question)
    End Class

    Public Class Question
        Public Sub New()
            Me.Id = Guid.NewGuid().ToString()
            Me.Name = ""
            Me.ExplainName = ""
            Me.Answers = Answers
        End Sub

        Public Sub New(qsetType As EnumQsetType)
            Me.Id = Guid.NewGuid().ToString()
            Me.Name = ""
            Me.ExplainName = ""
            Dim answerManager As New AnswerManager(qsetType)
            Me.Answers = answerManager.GetAnswers()
        End Sub

        Public Property Id As String
        Public Property Name As String
        Public Property ExplainName As String
        Public Property No As Integer
        Public Property Answers As List(Of Answer)
    End Class

    Public Class Answer
        Public Sub New()
            Me.Id = Guid.NewGuid().ToString()
            Me.Name = ""
            Me.ExplainName = ""
        End Sub
        Public Property Id As String
        Public Property Name As String
        Public Property ExplainName As String
        Public Property Score As Integer
        Public Property No As Integer

        Public Property QsetId As String
        Public Property QuestionId As String
    End Class

    Public Class AnswerManager
        Property QsetType As EnumQsetType
        Public Sub New(qsetType As EnumQsetType)
            Me.QsetType = qsetType
        End Sub

        Public Function GetAnswers() As List(Of Answer)
            If QsetType = EnumQsetType.Choice Then
                Return GetChoiceAnswer()
            ElseIf QsetType = EnumQsetType.TrueFalse Then
                Return GetTrueFalseAnswer()
            ElseIf QsetType = EnumQsetType.Pair Then
                Return GetPairAnswer()
            ElseIf QsetType = EnumQsetType.Sort Then
                Return GetSortAnswer()
            ElseIf QsetType = EnumQsetType.Subjective Then
                Return GetSubjectiveAnswer()
            Else
                Return Nothing
            End If
        End Function

        Private Function GetTrueFalseAnswer() As List(Of Answer)
            Dim Answers As New List(Of Answer)
            Answers.Add(New Answer() With {.No = 1, .Name = "ถูก"})
            Answers.Add(New Answer() With {.No = 2, .Name = "ผิด"})
            Return Answers
        End Function

        Private Function GetChoiceAnswer() As List(Of Answer)
            Dim Answers As New List(Of Answer)
            For i = 1 To 3
                Answers.Add(New Answer() With {.No = i})
            Next
            Return Answers
        End Function

        Private Function GetPairAnswer() As List(Of Answer)
            Dim Answers As New List(Of Answer)
            Answers.Add(New Answer())
            Return Answers
        End Function

        Private Function GetSortAnswer() As List(Of Answer)
            Return GetPairAnswer()
        End Function

        Private Function GetSubjectiveAnswer() As List(Of Answer)
            Return GetPairAnswer()
        End Function
    End Class

    <WebMethod(EnableSession:=True)>
    Public Function GetQsetContentBySort(subjectClassId As String, sortType As Integer)
        Dim ds As DataSet

        Dim subjectId As String = subjectClassId.Split("_")(0)
        Dim classid As String = subjectClassId.Split("_")(1)

        Dim objTestset As ClsTestSet = DirectCast(HttpContext.Current.Session("objTestSet"), ClsTestSet)
        Dim needJoinQ40 = HttpContext.Current.Application("NeedJoinQ40")
        Select Case sortType
            Case 1
                ds = objTestset.GetAllUnit(classid, subjectId, needJoinQ40, False)
            Case 2
                ds = objTestset.GetAllUnitByOrder(classid, subjectId, needJoinQ40)
            Case Else
                ds = New DataSet
        End Select
        Return CreateQsetContent(ds, objTestset)
    End Function

    Private Function CreateQsetContent(ByVal ds As DataSet, objTestSet As ClsTestSet)
        If ds.Tables.Count = 0 Then Return ""
        Dim sb As New System.Text.StringBuilder()
        If Not IsNothing(ds.Tables(0)) Then
            Dim qSetId As String, QuestionSet As String, numberOfQuestions As String
            For i = 0 To ds.Tables(0).Rows.Count - 1
                qSetId = ds.Tables(0).Rows(i)("qSetId").ToString()
                QuestionSet = ds.Tables(0).Rows(i)("QuestionSet").ToString()
                Dim QsetName As String = ds.Tables(0).Rows(i)("QSet_Name").ToString()
                Dim QCatName As String = ds.Tables(0).Rows(i)("QCategory_Name").ToString()
                Dim subjectId As String = ds.Tables(0).Rows(i)("GroupSubject_Id").ToString()
                Dim classId As String = ds.Tables(0).Rows(i)("Level_Id").ToString()
                'Dim CleanQuestionSet = objPdf.CleanSetNameText(QuestionSet)
                numberOfQuestions = ds.Tables(0).Rows(i)("numberOfQuestions").ToString()
                Dim ExamAmount As String = objTestSet.GetSelectedExamAmount(Session("newTestSetId").ToString, qSetId)
                Dim Book_Syllabus As String = ds.Tables(0).Rows(i)("Book_Syllabus").ToString()

                sb.Append("<tr><td><input onchange=""toggleNumQstn('" & qSetId & "', '" & classId & "', '" & subjectId & "');""" & GetChecked(qSetId, objTestSet) & " onclick=""onSave(this, '" & qSetId & "', '" & Session("newTestSetId").ToString & "',  '" & Session("userID").ToString & "', '" & classId & "' );"" type='checkbox' name='MID" & qSetId & "' value='' id='MID" & qSetId & "'><label for='MID" & qSetId & "'>")

                'เช็คว่าโจทย์ยาวเกินไปหรือป่าว
                Dim PositionCategory As Integer
                Dim QuestionAfTerLine As String
                Dim index As Integer = QuestionSet.IndexOf("</b> - ")
                PositionCategory = InStr(QuestionSet, "</b> - ")
                QuestionAfTerLine = QuestionSet.Substring((PositionCategory + 7))

                If QuestionAfTerLine.Length > 75 Then
                    Dim Strcut As String = objTestSet.CutStringAndReturn50Alphabet(qSetId)
                    sb.Append(Strcut)
                Else
                    sb.Append(QuestionSet)
                End If

                QsetName = QsetName.Replace("""", "&quot;")

                Dim ManageBtn As String = ""
                Dim MoveExamBtn As String = ""

                If (HttpContext.Current.Application("NeedAddNewQCatAndQsetButton") = True) Or (HttpContext.Current.Application("NeedDeleteQcatAndQset") = True) Then
                    ManageBtn = "<img title='จัดการชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/ManageQCateQSet.png' onclick=""EditQsetName('" & qSetId & "','" & QsetName & "')"" />"
                Else
                    ManageBtn = ""
                End If

                MoveExamBtn = "<img title='จัดการชุดข้อสอบ' style='margin-left:20px;cursor:pointer;' src='../Images/moveExam.png' />"

                Dim WIconStr As New StringBuilder
                If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
                    WIconStr.Append("<div class='MainDivSummary MainW' qsetid='" & qSetId & "'><div class='divLeft'>")
                    'แก้ไข้/ดู หมดแล้ว
                    If WordEditConfirmed(qSetId) = True Then
                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
                    End If
                    'อนุมัติหมดแล้ว
                    If WordTechnicalConfirmed(qSetId) = True Then
                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
                    End If
                    'พิสูจน์อักษรอนุมัติหมดแล้ว
                    If WordPrePressConfirmed(qSetId) = True Then
                        WIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
                    End If
                    WIconStr.Append("</div><div class='divRight'><img src='../Images/WIcon2.png' /></div></div>")
                End If

                Dim QIconStr As New StringBuilder
                If HttpContext.Current.Application("NeedEditQuestionCategory") = True Then
                    QIconStr.Append("<div class='MainDivSummary MainQ' qsetid='" & qSetId & "'><div class='divLeft'>")
                    'แก้ไข/ดูหมดแล้ว
                    If QuizEditConfirmed(qSetId) = True Then
                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>แก้/ดูหมดแล้ว</span></div>")
                    End If
                    'อนุมัติหมดแล้ว
                    If QuizTechnicalConfirmed(qSetId) = True Then
                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>อนุมัติหมดแล้ว</span></div>")
                    End If
                    'พิสูจน์อักษรอนุมัติหมดแล้ว
                    If QuizPrePreesConfirmed(qSetId) = True Then
                        QIconStr.Append("<div><img class='IconRight' src='../Images/right.png'/><span>พิสูจน์อักษรดูหมดแล้ว</span></div>")
                    End If
                    QIconStr.Append("</div><div class='divRight'><img src='../Images/QIcon2.png' /></div></div>")
                End If

                If ExamAmount.Equals("0") Then
                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ""> 
                                ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id=""spnTotal_" & qSetId & """>" & numberOfQuestions & "</span> ข้อ)</a>" & ManageBtn & MoveExamBtn & WIconStr.ToString() & QIconStr.ToString())
                Else
                    sb.Append("</label><br /><a class='aTag' style=""color: #2370FA;"" 
                                onclick=""OpenSelectEachQuestion('" & qSetId & "','" & classId & "')"" title=""เลือกเฉพาะบางข้อที่ต้องการได้ค่ะ"">
                                ชุดนี้เลือกมาแล้ว <span name='spnSelec' id=""spnSelected_" & qSetId & """>" & ExamAmount & "</span></span> จาก <span id=""spnTotal_" & qSetId & """>" &
                                numberOfQuestions & "</span> ข้อ</a>" & ManageBtn & MoveExamBtn & WIconStr.ToString() & QIconStr.ToString())
                End If
                sb.Append("</td></tr>")
            Next
        End If
        Return sb.ToString()
    End Function

#Region "LayoutConfirmed"
    Private Enum ConfirmedType
        WordEditConfirmed
        WordTechnicalConfirmed
        WordPrePressConfirmed
        QuizEditConfirmed
        QuizTechnicalConfirmed
        QuizPrePressConfirmed
    End Enum

    Private Function WordEditConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordEditConfirmed)
    End Function

    Private Function WordTechnicalConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordTechnicalConfirmed)
    End Function

    Private Function WordPrePressConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.WordPrePressConfirmed)
    End Function

    Private Function QuizEditConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizEditConfirmed)
    End Function

    Private Function QuizTechnicalConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizTechnicalConfirmed)
    End Function

    Private Function QuizPrePreesConfirmed(ByVal qSetId As String) As Boolean
        Return CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(qSetId, ConfirmedType.QuizPrePressConfirmed)
    End Function

    Private Function CheckEqualBetweenNumberOfQuestionInQsetAndNumberByCondition(ByVal qSetId As String, ByVal confirmedType As ConfirmedType) As Boolean
        Dim conn As New SqlConnection
        _DB.OpenExclusiveConnect(conn)
        Try
            Dim WhereField As String = ""
            If confirmedType = ConfirmedType.WordEditConfirmed Then WhereField = "WordEditConfirmed"
            If confirmedType = ConfirmedType.WordTechnicalConfirmed Then WhereField = "WordTechnicalConfirmed"
            If confirmedType = ConfirmedType.WordPrePressConfirmed Then WhereField = "WordPrePressConfirmed"
            If confirmedType = ConfirmedType.QuizEditConfirmed Then WhereField = "QuizEditConfirmed"
            If confirmedType = ConfirmedType.QuizTechnicalConfirmed Then WhereField = "QuizTechnicalConfirmed"
            If confirmedType = ConfirmedType.QuizPrePressConfirmed Then WhereField = "QuizPrePressConfirmed"
            Dim NumberOfQuestionInQset As Integer = GetNumberOfQuestionInQset(qSetId, conn)
            Dim NumberByCondition As Integer = GetNumberByCondition(qSetId, WhereField, conn)
            If NumberOfQuestionInQset = NumberByCondition Then
                _DB.CloseExclusiveConnect(conn)
                Return True
            Else
                _DB.CloseExclusiveConnect(conn)
                Return False
            End If
        Catch ex As Exception
            _DB.CloseExclusiveConnect(conn)
            Return False
        End Try
    End Function

    Private Function GetNumberOfQuestionInQset(ByVal qSetId As String, ByRef InputConn As SqlConnection) As Integer
        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblLayoutConfirmed WHERE Qset_Id = '" & qSetId & "' AND IsActive = 1; "
        Dim numberOfQuestion As Integer = CInt(_DB.ExecuteScalar(sql, InputConn))
        Return numberOfQuestion
    End Function

    Private Function GetNumberByCondition(ByVal qSetId As String, ByVal FieldToWhere As String, ByRef inputConn As SqlConnection) As Integer
        Dim sql As String = ""
        sql = " SELECT COUNT(*) FROM dbo.tblLayoutConfirmed WHERE Qset_Id = '" & qSetId & "' AND IsActive = 1 AND " & FieldToWhere & " = 1; "
        Dim NumberByCondition As Integer = CInt(_DB.ExecuteScalar(sql, inputConn))
        Return NumberByCondition
    End Function

#End Region

    Public Function GetChecked(ByVal QsetId As String, objTestSet As ClsTestSet) As String
        Dim testsetId As String = If((Session("EditID") = ""), Session("newTestSetId").ToString(), Session("EditID").ToString())
        Dim Check As String = objTestSet.GetSelectedQuestionSet(testsetId, QsetId)
        Return If((Check = "True"), "checked=""checked""", String.Empty)
    End Function


    <WebMethod(EnableSession:=True)>
    Public Function IsQsetInTestset(qsetId As String) As Boolean
        Dim qsetManagement As New QsetManagement(qsetId)
        Dim isQsetExist As Boolean = qsetManagement.IsQsetExistInTestset()
        Return isQsetExist
    End Function

    <WebMethod(EnableSession:=True)>
    Public Function DeleteQset(classId As String, subjectId As String, qsetId As String) As Boolean
        Dim qsetManagement As New QsetManagement(classId, subjectId, qsetId)
        Dim isDeleted As Boolean = qsetManagement.DeleteQset()
        Return isDeleted
    End Function

End Class

Public Enum EnumQsetType
    Choice = 1
    TrueFalse = 2
    Pair = 3
    Sort = 6
    Subjective = 9
End Enum