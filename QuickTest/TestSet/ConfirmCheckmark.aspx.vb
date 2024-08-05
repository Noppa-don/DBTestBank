Imports System.Globalization
Imports System.Web

Public Class ConfirmCheckmark
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            setTextToControl()
        End If

    End Sub

    Private Sub setTextToControl()
        Dim db As New ClassConnectSql()
        Dim TestsetId As String = Session("newTestSetId").ToString
        'Dim TestsetId As String = "608D869A-7043-4490-ADC7-A5261FD433DE"

        Dim Testset As DataTable = db.getdata(" SELECT TestSet_Name,NeedConnectCheckmark FROM tblTestSet WHERE TestSet_Id = '" & TestsetId & "'; ")

        Dim sql As String = " SELECT ClassId FROM tblUserSubjectClass WHERE UserId = '" & Session("UserId") & "' GROUP BY ClassId ORDER BY CONVERT(INT,ClassId); "
        Dim ClassId As DataTable = db.getdata(sql)
        ddClassName.Items.Clear()
        For i As Integer = 0 To ClassId.Rows.Count - 1
            Dim intClassName As Integer = CInt(ClassId(i)("ClassId"))
            Dim className As String = setClassName(intClassName)
            'ddClassName.Items.Insert(i, New ListItem(className))
            ddClassName.Items.Add(New ListItem(className, className))
        Next

        'Dim sqlLevelTest As String = " SELECT tl.Level FROM tblTestSet ts JOIN tblLevel tl "
        'sqlLevelTest &= " ON ts.Level_Id = tl.Level_Id WHERE ts.TestSet_Id = '" & TestsetId & "'; "
        Dim sqlLevelTest As String = " SELECT  TOP 1(tblLevel.Level) FROM  tblQuestionCategory INNER JOIN "
        sqlLevelTest &= " tblQuestionSet ON tblQuestionCategory.QCategory_Id = tblQuestionSet.QCategory_Id INNER JOIN "
        sqlLevelTest &= " tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN "
        sqlLevelTest &= " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN "
        sqlLevelTest &= " tblTestSetQuestionSet ON tblQuestionSet.QSet_Id = tblTestSetQuestionSet.QSet_Id "
        sqlLevelTest &= " WHERE TestSet_Id = '" & TestsetId & "' AND tblTestsetQuestionSet.IsActive = '1' ORDER BY Level DESC "

        Dim levelClass As String = db.ExecuteScalar(sqlLevelTest)
        levelClass = setClassName(CInt(levelClass) + 3)
        ddClassName.Items.FindByValue(levelClass).Selected = True


        'Dim formatTh As DateTimeFormatInfo = New DateTimeFormatInfo
        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            Dim getTemplateAndAmount = getTemplateAndQuizAmount()
            Dim qAmount As String = getTemplateAndAmount.quizAmount
            'ถ้าข้อสอบเกิน 120 ให้ checkbox-checkmark disabled
            If (CInt(qAmount) > 120) Then
                chkUseTemplate.Enabled = False
                divUseTemplate.Attributes("class") = "Over"
            Else
                Dim sqlQsetType As String = " SELECT tblQuestionSet.QSet_Type FROM tblTestSetQuestionSet INNER JOIN "
                sqlQsetType &= " tblQuestionSet ON tblTestSetQuestionSet.QSet_Id = tblQuestionSet.QSet_Id"
                sqlQsetType &= " WHERE tblTestSetQuestionSet.TestSet_Id = '" & TestsetId & "' AND tblQuestionSet.QSet_Type <> '1' "
                sqlQsetType &= " AND tblQuestionSet.QSet_Type <> '2' AND tblTestsetQuestionSet.IsActive = '1';"
                Dim dt As DataTable = db.getdata(sqlQsetType)
                ' เช็คว่าชุดนั้นมีข้อสอบที่ไม่สามารถใช้ checkmark ได้หรือไม่
                If (dt.Rows.Count > 0) Then
                    chkUseTemplate.Enabled = False
                    divUseTemplate.Attributes("class") = "TypeError"
                Else
                    If (Testset(0)("NeedConnectCheckmark").ToString() = "True") Then
                        chkUseTemplate.Checked = True
                    Else
                        chkUseTemplate.Checked = False
                    End If
                End If
            End If
        End If

        lblTestsetName.Text = Testset(0)("TestSet_Name").ToString()
        txtQuizdate.Text = Date.Now.ToString("dd/MM/yy", DateTimeFormatInfo.CurrentInfo)
        txtQuiztime.Text = Date.Now.ToString("HH:00", DateTimeFormatInfo.CurrentInfo)

    End Sub
    <Services.WebMethod()>
    Public Shared Function saveToChekmark(ByVal setupName As String, ByVal className As String) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return False
        End If

        Dim clsCheckmark As New ClsCheckMark()

        Dim getTemplateAndAmount = getTemplateAndQuizAmount()
        Dim TemplateName As String = getTemplateAndAmount.template
        Dim qAmount As String = getTemplateAndAmount.quizAmount

        Try
            clsCheckmark.saveQuizToCheckmark(setupName, TemplateName, qAmount, className, HttpContext.Current.Session("newTestSetId").ToString())
            clsCheckmark.InsertRefToCheckMarkIntblCM(HttpContext.Current.Session("newTestSetId").ToString)
            clsCheckmark.saveAllCorrectAnswerToCheckmark()
            clsCheckmark.updateConnectCheckmark("1")
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return False
        End Try

    End Function

    <Services.WebMethod()>
    Public Shared Function updateNeedConnectCheckmarkWhenNotNeed() As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return False
        End If
        Dim clsCheckmark As New ClsCheckMark()

        Try
            clsCheckmark.updateConnectCheckmark("0")
            Return True
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return False
        End Try

    End Function

    Private Shared Function getTemplateAndQuizAmount()
        Dim db As New ClassConnectSql()
        Dim sql As String = " SELECT COUNT(td.TSQD_Id) as qAmount FROM tblTestSetQuestionSet  ts "
        sql &= " LEFT JOIN tblTestSetQuestionDetail td "
        sql &= " ON ts.TSQS_Id = td.TSQS_Id "
        sql &= " WHERE ts.TestSet_Id = '" & HttpContext.Current.Session("newTestSetId").ToString() & "' And ts.IsActive = '1' and td.IsActive= = '1' ; "
        Dim qAmount As Integer = CInt(db.ExecuteScalar(sql))
        Dim templateAndAmount As setTemplateAndAmount
        If (qAmount <= 60) Then
            templateAndAmount.template = "Wpp02 QR 120 Choice"
        ElseIf (qAmount > 60) Then
            templateAndAmount.template = "Wpp02 QR 120 Choice"
        End If
        templateAndAmount.quizAmount = CStr(qAmount)
        Return templateAndAmount
    End Function


    Private Structure setTemplateAndAmount
        Public template As String
        Public quizAmount As String
    End Structure

    Private Function setClassName(ByVal classid As Integer) As String
        Dim className As Array = {"", "อ.1", "อ.2", "อ.3", "ป.1", "ป.2", "ป.3", "ป.4", "ป.5", "ป.6", "ม.1", "ม.2", "ม.3", "ม.4", "ม.5", "ม.6"}
        setClassName = className(classid)
        Return setClassName
    End Function
End Class