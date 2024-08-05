Public Class AddNewQuestionCatAndSet
    Inherits System.Web.UI.Page

    Dim _DB As New ClassConnectSql()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("LevelId") Is Nothing Or Request.QueryString("Subjectid") Is Nothing Or Session("SelectedYears") Is Nothing Then
            Exit Sub
        End If

        Dim LevelId As String = Request.QueryString("LevelId").ToString()
        Dim SubjectId As String = Request.QueryString("Subjectid").ToString()


        If Not Page.IsPostBack Then
         
            CreateDataBindQCat(LevelId, SubjectId)

        End If

    End Sub

    Private Sub CreateDataBindQCat(ByVal LevelId As String, ByVal GroupSubjectId As String)

        Dim YearSelect As String = Session("SelectedYears").ToString()
        'GroupSubjectId =  ChageSubjectIdToGroupSubjectId(GroupSubjectId)
        If GroupSubjectId = "" Then
            Exit Sub
        End If
        YearSelect = YearSelect.Replace("y", "")
        Dim ObjYear = YearSelect.Split(",")
        If ObjYear.Count > 1 Then
            BindDDLQuestionCategory(LevelId, GroupSubjectId)
            BindDDlBook(LevelId, GroupSubjectId)
            BindDDlChildCategory()
        Else
            BindDDlBook(LevelId, GroupSubjectId, ObjYear(0).ToString())
            BindDDLQuestionCategory(LevelId, GroupSubjectId, ObjYear(0).ToString())
            BindDDlChildCategory()
        End If

    End Sub

    Private Sub BindDDlChildCategory()

        Dim BookId As String = ddlBook.SelectedValue
        Dim sql As String = " SELECT DISTINCT QCategory_Level FROM dbo.tblQuestionCategory " & _
                            " WHERE Book_Id = '" & BookId & "' ORDER BY QCategory_Level "
        Dim dt1 As New DataTable
        dt1 = _DB.getdata(sql)

        If dt1.Rows.Count > 0 Then

            ddlChildCategory.Items.Clear()
            Dim StrUnderScore As String = ""
            Dim GenNewguid As String = System.Guid.NewGuid().ToString()
            ddlChildCategory.Items.Add("ให้อยู่ชั้นบนสุด")
            ddlChildCategory.Items(0).Value = GenNewguid & "|0"


            For i = 0 To dt1.Rows.Count - 1
                Dim Space As Integer = 1
                Space = CInt(dt1.Rows(i)("QCategory_Level")) * Space
                StrUnderScore = GenStr_(Space)

                sql = " SELECT QCategory_Id,QCategory_Name AS QCategory_Name FROM dbo.tblQuestionCategory " & _
                      " WHERE Book_Id = '" & BookId & "' AND QCategory_Level = " & dt1.Rows(i)("QCategory_Level") & " and Isactive = 1 ORDER BY QCategory_No "
                Dim dt2 As New DataTable
                dt2 = _DB.getdata(sql)

                If dt2.Rows.Count > 0 Then
                    For a = 0 To dt2.Rows.Count - 1
                        ddlChildCategory.Items.Add(StrUnderScore & dt2.Rows(a)("QCategory_Name").ToString())
                        Dim CountItem As Integer = ddlChildCategory.Items.Count - 1
                        ddlChildCategory.Items(CountItem).Value = dt2.Rows(a)("QCategory_Id").ToString() & "|" & dt1.Rows(i)("QCategory_Level")
                    Next
                End If

            Next
        Else
            ddlChildCategory.Items.Clear()
            Dim StrUnderScore As String = ""
            Dim GenNewguid As String = System.Guid.NewGuid().ToString()
            ddlChildCategory.Items.Add("ให้อยู่ชั้นบนสุด")
            ddlChildCategory.Items(0).Value = GenNewguid & "|0"
        End If


    End Sub


    Private Function GenStr_(ByVal Quantity As Integer)

        Dim Str As String = "_"


        For i = 1 To Quantity
            Str = Str & "_"
        Next

        Return Str

    End Function


    Private Sub BindDDlBook(ByVal LevelId As String, ByVal GroupSubjectId As String, Optional ByVal Book_Syllabus As String = "")

        Dim sql As String = " SELECT tblBook.BookGroup_Id, tblBook.Book_Name,Book_Syllabus FROM tblBook INNER JOIN " & _
                            " tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id INNER JOIN " & _
                            " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id " & _
                            " WHERE (tblGroupSubject.GroupSubject_Id = '" & GroupSubjectId & "') AND " & _
                            " (tblLevel.Level_Id = '" & LevelId & "') "
        If Book_Syllabus <> "" Then
            sql &= " AND (dbo.tblBook.Book_Syllabus = " & Book_Syllabus & ") "
        End If
        sql &= " ORDER BY Book_Syllabus,Book_Name "

        Dim dt As New DataTable
        dt = _DB.getdata(sql)

        If dt.Rows.Count > 0 Then

            For i = 0 To dt.Rows.Count - 1
                ddlBook.Items.Add(dt.Rows(i)("Book_Name"))
                ddlBook.Items(i).Value = dt.Rows(i)("BookGroup_Id").ToString()
            Next

        End If
                          
    End Sub


    Private Sub BindDDLQuestionCategory(ByVal LevelId As String, ByVal GroupSubjectId As String, Optional ByVal Book_Syllabus As String = "")

        Dim sql As String = " SELECT DISTINCT tblQuestionCategory.QCategory_Id, tblQuestionCategory.QCategory_Name,dbo.tblBook.Book_Name  FROM  tblQuestionCategory INNER JOIN  tblBook ON tblQuestionCategory.Book_Id = tblBook.BookGroup_Id INNER JOIN  " & _
                            " tblLevel ON tblBook.Level_Id = tblLevel.Level_Id INNER JOIN  tblGroupSubject ON tblBook.GroupSubject_Id = tblGroupSubject.GroupSubject_Id  " & _
                            " WHERE (tblLevel.Level_Id = '" & LevelId & "') AND  (dbo.tblGroupSubject.GroupSubject_Id = '" & GroupSubjectId & "') " & _
                            " AND (dbo.tblQuestionCategory.IsActive = 1) "
        If Book_Syllabus <> "" Then
            sql &= " AND (dbo.tblBook.Book_Syllabus = " & Book_Syllabus & ")"
        End If
        sql &= " ORDER BY dbo.tblQuestionCategory.QCategory_Name "
        Dim dt As New DataTable
        dt = _DB.getdata(sql)
        If dt.Rows.Count > 0 Then
            Dim ItemaName As String = ""
            For i = 0 To dt.Rows.Count - 1
                ItemaName = dt.Rows(i)("QCategory_Name") & "(" & dt.Rows(i)("Book_Name") & ")"
                ddlQCat.Items.Add(ItemaName)
                ddlQCat.Items(i).Value = dt.Rows(i)("QCategory_Id").ToString()
            Next

        End If

    End Sub

    Private Function ChageSubjectIdToGroupSubjectId(ByVal SubjectId As String) As String

        Dim GroupSubjectId As String = ""

        If SubjectId = "" Or SubjectId Is Nothing Then
            Return GroupSubjectId
        End If

        SubjectId = SubjectId.ToUpper()

        Select Case SubjectId
            Case "1"
                GroupSubjectId = "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
            Case "2"
                GroupSubjectId = "FDA224D9-CEBE-4642-ACD0-D7F7282E36AE"
            Case "3"
                GroupSubjectId = "A4B9F5CB-2D3C-4F6A-8666-FD2620E69723"
            Case "4"
                GroupSubjectId = "58802565-23BB-4F22-8238-E983AC781B0F"
            Case "5"
                GroupSubjectId = "FB677859-87DA-4D8D-A61E-8A76566D69D8"
            Case "6"
                GroupSubjectId = "6A4A7294-F5A7-4D64-ADBC-73DC14377737"
            Case "7"
                GroupSubjectId = "73C4639B-267C-4B7E-B5A4-1B4EBB428019"
            Case "8"
                GroupSubjectId = "47A224EF-3348-41B7-84D0-7250648F8706"
            Case Else
                Return GroupSubjectId
        End Select

        Return GroupSubjectId

    End Function

    Private Sub btnAddQuestionSet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddQuestionSet.Click

        SaveQuestionSet()

    End Sub


    Private Sub btnAddQuestionCat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddQuestionCat.Click

        SaveQuestionCategory()

    End Sub


    Private Sub SaveQuestionSet()

        If txtQuestionSet.Text = "" Then
            lblWarnQuestinoSet.Text = "กรุณาพิมพ์ชื่อชุดข้อสอบ"
            lblWarnQuestinoSet.Visible = True
            Exit Sub
        End If

        If rdType1.Checked = False And rdType2.Checked = False And rdType3.Checked = False And rdType6.Checked = False Then
            lblWarnQuestinoSet.Text = "กรุณาเลือกชนิดของข้อสอบ"
            lblWarnQuestinoSet.Visible = True
            Exit Sub
        End If

        Dim QcatId As String = ddlQCat.SelectedValue
        Dim QTypeId As String = ddlQtype.SelectedValue
        Dim QsetName As String = _DB.CleanString(txtQuestionSet.Text)
        Dim QsetType As Integer = 0

        If rdType1.Checked = True Then
            QsetType = 1
        ElseIf rdType2.Checked = True Then
            QsetType = 2
        ElseIf rdType3.Checked = True Then
            QsetType = 3
        ElseIf rdType6.Checked = True Then
            QsetType = 6
        End If

        Dim DisplayCorrect As Integer = 0
        Dim IsRandomQuestion As Integer = 0
        Dim IsRandomAnswer As Integer = 0

        If ChkDisplayCorrect.Checked = True Then
            DisplayCorrect = 1
        End If

        If ChkRandomQuestion.Checked = True Then
            IsRandomQuestion = 1
        End If

        If ChkRandomAnswer.Checked = True Then
            IsRandomAnswer = 1
        End If

        Dim TotalQuestion As Integer = ddlQuantityQuestion.SelectedValue

        Dim sql As String = " SELECT COUNT(*) FROM dbo.tblQuestionSet WHERE QCategory_Id = '" & QcatId & "' AND IsActive = 1 "
        Dim CheckIsExist As String = _DB.ExecuteScalar(sql)
        Dim GenNewQsetId As String = System.Guid.NewGuid().ToString()

        If CheckIsExist > 0 Then
            sql = " INSERT INTO dbo.tblQuestionSet ( QSet_Id ,QCategory_Id ,QType_Id ,QSet_No ,QSet_Name ,QSet_Type , " & _
                  " QSet_Time ,QSet_IsDisplayCorrect ,QSet_IsRandomQuestion ,QSet_IsRandomAnswer ,IsActive ,Mid) " & _
                  " VALUES  ( '" & GenNewQsetId & "' , '" & QcatId & "' , '" & QTypeId & "' , " & _
                  " (SELECT MAX(QSet_No)+1 FROM dbo.tblQuestionSet WHERE QCategory_Id = '" & QcatId & "') , " & _
                  " '" & QsetName & "' , " & QsetType & " , 0 , '" & DisplayCorrect & "' , '" & IsRandomQuestion & "' , '" & IsRandomAnswer & "' , 1 , NULL ) "
        Else
            sql = " INSERT INTO dbo.tblQuestionSet ( QSet_Id ,QCategory_Id ,QType_Id ,QSet_No ,QSet_Name ,QSet_Type , " & _
                       " QSet_Time ,QSet_IsDisplayCorrect ,QSet_IsRandomQuestion ,QSet_IsRandomAnswer ,IsActive ,Mid) " & _
                       " VALUES  ( '" & GenNewQsetId & "' , '" & QcatId & "' , '" & QTypeId & "' , " & _
                       " 1 , " & _
                       " '" & QsetName & "' , " & QsetType & " , 0 , '" & DisplayCorrect & "' , '" & IsRandomQuestion & "' , '" & IsRandomAnswer & "' , 1 , NULL ) "
        End If
        'Dim NewQsetId As String = ""
        _DB.OpenWithTransection()
        Try
            _DB.ExecuteWithTransection(sql)

            'sql = " SELECT TOP 1 QSet_Id FROM dbo.tblQuestionSet WHERE QCategory_Id = '" & QcatId & "'  ORDER BY QSet_No desc "
            'NewQsetId = _DB.ExecuteScalarWithTransection(sql)
            'If NewQsetId = "" Then
            '    _DB.RollbackTransection()
            '    lblWarnQuestinoSet.Text = " หา QsetId ที่เพิ่ง Insert ลงไปไม่ได้ "
            '    Exit Sub
            'End If

            'วน Insert คำถามตามจำนวนที่เลือกมาจาก ddlQuantity
            For i = 1 To TotalQuestion
                Dim GuidQuestion As String = System.Guid.NewGuid().ToString()
                Dim QId As String = "0"

                sql = "select cast(max(q.QId) as int) + 1 as newQId from tblquestion q inner join tblQuestionSet qs on q.QSet_Id = qs.QSet_Id
                        inner join tblQuestionCategory qc on qs.QCategory_Id = qc.QCategory_Id
                        inner join tblBook b on qc.Book_Id = b.BookGroup_Id 
                        where b.GroupSubject_Id = '" & Request.QueryString("Subjectid").ToString() & "'"

                QId = _DB.ExecuteScalarWithTransection(sql)

                If QId = "" Then
                    QId = "0"
                End If

                sql = " INSERT INTO dbo.tblQuestion(Question_Id, QSet_Id, EfficiencySet_Id, Question_No, Question_Name, Question_Expain, IsActive,QId,Question_Name_Backup, LastUpdate,
                        IsWpp, NoChoiceShuffleAllowed,Question_Name_Quiz,Question_Expain_Quiz) 
                        VALUES('" & GuidQuestion & "','" & GenNewQsetId & "',NULL," & i & ", 'คำถามใหม่','',1,0,'',dbo.GetThaiDate(),1," & QId & ",'คำถามใหม่','' ) "
                _DB.ExecuteWithTransection(sql)

                'วน Insert คำตอบตาม Type 
                If QsetType = 1 Then

                    For a = 1 To 4
                        sql = " INSERT INTO dbo.tblAnswer ( Answer_Id ,Question_Id ,Efficiency_Id ,QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score ,
                                Answer_ScoreMinus , Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate,AlwaysShowInLastRow,Answer_Name_Quiz,Answer_Expain_Quiz )
                                VALUES (NEWID() ,'" & GuidQuestion & "', NULL ,'" & GenNewQsetId & "' , " & a & " , 'คำตอบ " & a & "' , '' , 0 ,0 , NULL , 1 , '' , dbo.GetThaiDate(),0,'คำตอบ " & a & "','') "
                        _DB.ExecuteWithTransection(sql)
                    Next

                ElseIf QsetType = 2 Then
                    Dim Score As Integer = 1
                    For b = 1 To 2
                        Dim answerNameTF As String = "True"
                        If b = 2 Then
                            answerNameTF = "False"
                        End If
                        sql = " INSERT INTO dbo.tblAnswer ( Answer_Id ,Question_Id ,Efficiency_Id , " &
                          " QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score , " &
                          " Answer_ScoreMinus , Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate,AlwaysShowInLastRow ) " &
                          " VALUES  ( NEWID() , '" & GuidQuestion & "' , NULL ,  " &
                          " '" & GenNewQsetId & "' , " & b & " , '" & answerNameTF & "' , '' , " & Score & " ,  " &
                          " 0 , NULL , 1 , '' , dbo.GetThaiDate(),0 ) "
                        _DB.ExecuteWithTransection(sql)
                        Score -= 1
                    Next

                ElseIf QsetType = 3 Then
                    For c = 1 To 1
                        sql = " INSERT INTO dbo.tblAnswer ( Answer_Id ,Question_Id ,Efficiency_Id , " &
                                " QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score , " &
                                " Answer_ScoreMinus , Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate,AlwaysShowInLastRow ) " &
                                " VALUES  ( NEWID() , '" & GuidQuestion & "' , NULL ,  " &
                                " '" & GenNewQsetId & "' , " & c & " , 'คำตอบ " & c & "' , '' , 1 ,  " &
                                " 0 , NULL , 1 , '' , dbo.GetThaiDate(),0 ) "
                        _DB.ExecuteWithTransection(sql)
                    Next
                ElseIf QsetType = 6 Then
                    sql = " INSERT INTO dbo.tblAnswer ( Answer_Id ,Question_Id ,Efficiency_Id , " &
                               " QSet_Id ,Answer_No ,Answer_Name ,Answer_Expain ,Answer_Score , " &
                               " Answer_ScoreMinus , Answer_Position ,IsActive ,Answer_Name_Bkup ,LastUpdate,AlwaysShowInLastRow ) " &
                               " VALUES  ( NEWID() , '" & GuidQuestion & "' , NULL ,  " &
                               " '" & GenNewQsetId & "' , " & i & " , '" & i & "' , '' , 1 ,  " &
                               " 0 , NULL , 1 , '' , dbo.GetThaiDate(),0 ) "
                    _DB.ExecuteWithTransection(sql)
                End If

            Next

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            _DB.RollbackTransection()
            lblWarnQuestinoSet.Text = ex.ToString()
            Exit Sub
        End Try


        _DB.CommitTransection()

        'insert ลง tblLayoutConfirmed
        Dim clsLayout As New ClsLayoutCheckConfirmed()
        clsLayout.InsertQuestionInTblLayoutCheckConfirmed(GenNewQsetId)

        Dim NewLog As String = "เพิ่มชุดข้อสอบในหน่วยการเรียนรู้ " & ddlQCat.SelectedItem.ToString & " ชื่อชุด " & QsetName

        If ChkDisplayCorrect.Checked Then
            NewLog = NewLog & " แสดงคำตอบที่ถูกต้อง,"
        End If
        If ChkRandomQuestion.Checked Then
            NewLog = NewLog & " สลับคำถามได้,"
        End If
        If ChkRandomAnswer.Checked Then
            NewLog = NewLog & " สลับคำตอบได้ "
        End If
        If NewLog.Substring(NewLog.Length - 1, 1) = "," Then
            NewLog = NewLog.Substring(0, NewLog.Length - 1) & " ประเภท "
        End If

        NewLog = NewLog & ddlQtype.SelectedItem.ToString & " จำนวน "
        NewLog = NewLog & ddlQuantityQuestion.SelectedItem.ToString & " ชนิด "

        If rdType1.Checked Then
            NewLog = NewLog & "ปรนัย"
        ElseIf rdType2.Checked Then
            NewLog = NewLog & "ถูกผิด"
        ElseIf rdType3.Checked Then
            NewLog = NewLog & "จับคู่"
        Else
            NewLog = NewLog & "เรียงลำดับ"
        End If

        NewLog = NewLog & " (QsetId=" & GenNewQsetId & ")"

        ' ใหม่ QsetId = " & GenNewQsetId & " ที่ QuestionCategoryId = " & ddlQCat.SelectedItem.ToString & " "
        Log.Record(Log.LogType.ManageExam, NewLog, True)

        Response.Redirect("~/TestSet/Step3.aspx")
    End Sub

    Private Sub SaveQuestionCategory()

        If txtQuestionCat.Text = "" Or txtQuestionCat Is Nothing Then
            lblWarnQuestionCat.Text = "ต้องพิมพ์ชื่อหน่วยการเรียนรู้ก่อน"
            lblWarnQuestionCat.Visible = True
            Exit Sub
        End If
        Response.Write(ddlChildCategory.SelectedValue.ToString)

        Dim ValueDDl As String = ddlChildCategory.SelectedValue
        Dim SplitStr = ValueDDl.Split("|")
        Dim QcatId As String = ""
        Dim QCatLevel As Integer = _DB.CleanString(CInt(SplitStr(1)) + 1)
        Dim RootId As String = _DB.CleanString(SplitStr(0))
        Dim Bookid As String = ddlBook.SelectedValue
        Dim Originallevel As Integer = 0

        If SplitStr(1) = 0 Then
            QcatId = SplitStr(0)
            Originallevel = 1
        Else
            QcatId = System.Guid.NewGuid.ToString()
            Originallevel = QCatLevel
            If Originallevel = 0 Then
                Originallevel = 1
            End If
        End If

        Dim sqlcheck As String = " SELECT COUNT(QCategory_No) + 1 FROM dbo.tblQuestionCategory  " & _
                                 " WHERE Book_Id = '" & Bookid & "' AND QCategory_Level = '" & Originallevel & "' "
        Dim QcatNo As String = _DB.ExecuteScalar(sqlcheck)

        If QcatNo = "" Then
            lblWarnQuestionCat.Text = "Select Count ไม่ติด"
            lblWarnQuestionCat.Visible = True
            Exit Sub
        End If


        Dim sql As String = " INSERT INTO dbo.tblQuestionCategory ( QCategory_Id ,Root_Id ,Parent_Id ,Book_Id , " & _
                            " QCategory_Level ,QCategory_No ,QCategory_Name , " & _
                            " QCategory_Expect ,QCategory_Detail ,IsActive ,Kid ) " & _
                            " VALUES  ( '" & QcatId & "' , '" & RootId & "' , '" & RootId & "' ,  " & _
                            " '" & Bookid & "' , '" & QCatLevel & "' , '" & QcatNo & "' ,  " & _
                            " '" & _DB.CleanString(txtQuestionCat.Text) & "' , NULL , NULL , 1 , NULL )  "

        Try
            _DB.Execute(sql)
            Dim NewLog As String
            'NewLog = "เพิ่ม QuestionCategory ใหม่ QuestionCategoryId = " & QcatId & " มี CategoryName = " & _DB.CleanString(txtQuestionCat.Text) & " " & _
            '                       " ที่ BookId = " & Bookid & " "
            NewLog = "เพิ่มหน่วยการเรียนรู้ หนังสือ " & ddlBook.SelectedItem.ToString & " หน่วยที่ซ้อนอยู่ " & ddlChildCategory.SelectedItem.ToString
            NewLog = NewLog & " ชื่อหน่วย " & _DB.CleanString(txtQuestionCat.Text) & " (QCatId=" & QcatId & ")"
            Log.Record(Log.LogType.ManageExam, NewLog, True)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            lblWarnQuestionCat.Text = ex.ToString()
            lblWarnQuestionCat.Visible = True
        End Try

        Response.Redirect("AddNewQuestionCatAndSet.aspx?Subjectid=" & Request.QueryString("Subjectid") & "&LevelId=" & Request.QueryString("LevelId"))

    End Sub

    Private Sub ddlBook_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBook.SelectedIndexChanged

        BindDDlChildCategory()

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Log.Record(Log.LogType.ManageExam, "กลับไป Step 3", True)
        Response.Redirect("~/Testset/Step3.aspx")
    End Sub

End Class