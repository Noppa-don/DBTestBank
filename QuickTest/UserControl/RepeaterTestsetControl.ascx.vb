Imports BusinessTablet360
Imports System.Data.SqlClient
Imports KnowledgeUtils

Public Class RepeaterTestsetControl
    Inherits System.Web.UI.UserControl

    Public _DashboardMode As Integer
    Private _UserId As String
    Private SqlIsMode As String = ""

    Public IsEnabledSearch As Boolean

    Dim UseCls As New ClassConnectSql

    Public Sub RepeaterTestsetControl(ByVal DashboardMode As Integer, ByVal UserId As String)
        _DashboardMode = DashboardMode
        _UserId = UserId
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Dim arrStr As Array = GetArrStrLabel()
            'Dim arrMenuTestset As Array = {"", "", HW_Student, ViewResult}
            'Dim arrLabel As Array = {lblNewTestset, lblManageTestset, lblHW_Student, lblViewResult}

            'For i As Integer = 0 To arrStr.Length - 2
            '    If Not arrStr(i).ToString() = "" Then
            '        arrLabel(i).InnerText = arrStr(i).ToString()
            '    Else
            '        arrMenuTestset(i).Visible = False
            '    End If
            'Next

            Dim m As Object = GetMenuName()
            lblNewTestset.InnerText = m.Menu1
            lblManageTestset.InnerText = m.Menu2
            lblHW_Student.InnerText = m.Menu3

            If ClsKNSession.RunMode <> "standalonenotablet" Then
                lblViewResult.InnerText = m.Menu4
            End If


            If _DashboardMode = EnumDashBoardType.PrintTestset Or _DashboardMode = EnumDashBoardType.SetUp Then
                HW_Student.Visible = False
                ViewResult.Visible = False
            ElseIf _DashboardMode = EnumDashBoardType.Quiz Or _DashboardMode = EnumDashBoardType.Practice Then
                HW_Student.Visible = False
                If (_DashboardMode = EnumDashBoardType.Quiz) And (ClsKNSession.RunMode = "standalonenotablet") Then
                    ViewResult.Visible = False
                End If
                If (_DashboardMode = EnumDashBoardType.Practice) And (ClsKNSession.RunMode = "standalonenotablet") Then
                    ViewResult.Visible = False
                End If
            End If

            Log.Record(Log.LogType.PageLoad, "Get Testset Repeater", False)
            'Open Connection
            Dim connActivity As New SqlConnection
            UseCls.OpenExclusiveConnect(connActivity)
            Log.Record(Log.LogType.PageLoad, connActivity.ConnectionString, False)

            Dim dtTestset As DataTable = GetTestset(connActivity)
            Log.Record(Log.LogType.PageLoad, dtTestset.Rows.Count, False)

            If dtTestset.Rows.Count > 0 Then
                dtTestset.Columns.Add("TimeAgo", GetType(String))
                For Each row In dtTestset.Rows
                    row("TimeAgo") = Convert.ToDateTime(row("LastUpdate")).ToPointPlusTime()
                Next
                IsEnabledSearch = True
                Listing.DataSource = dtTestset
                Listing.DataBind()
                Log.Record(Log.LogType.PageLoad, "Bind Testset OK", False)
            Else
                IsEnabledSearch = False
                repeaterTestsetDiv.InnerHtml = "<div style='height:100px;background-color:white;overflow-y:initial;border:1px dashed black;color:black;font-weight: bold;cursor:auto;'><span class='MsgNoTestset'>" + m.HintTxt + "</span><br/><span class='hint'>จัดชุดข้อสอบตามใจที่ <a href='../testset/step2.aspx'>หน้าจัดชุด</a> ค่ะ</span></div>"
                repeaterTestsetDiv.Style.Add("height", "auto")
                Log.Record(Log.LogType.PageLoad, "Bind Empty Testset OK", False)
            End If
            'Close Connection
            UseCls.CloseExclusiveConnect(connActivity)
            Log.Record(Log.LogType.PageLoad, "Close Connection", False)

            ' get qset ที่ user อื่นได้สร้างขึ้นไว้ และ match กับตัว user เอง
            Log.Record(Log.LogType.PageLoad, ClsKNSession.IsAddQuestionBySchool.ToString, False)
            If ClsKNSession.IsAddQuestionBySchool Then
                RepeaterOtherUserTestset.DataSource = GetDatableOtherTestset()
                RepeaterOtherUserTestset.DataBind()

                Log.Record(Log.LogType.PageLoad, "Add Question By School", False)
            End If

            Log.Record(Log.LogType.PageLoad, "Before Update Temp Question", False)
            Dim ClsT As New ClsTestSet(_UserId)
            ClsT.UpdateTempQuestion()
            Log.Record(Log.LogType.PageLoad, "After Update Temp Question", False)
        Catch ex As Exception
            Log.Record(Log.LogType.PageLoad, ex.ToString, False)
        End Try


    End Sub

    Private Function GetArrStrLabel() As Array
        Select Case _DashboardMode
            Case EnumDashBoardType.Quiz
                GetArrStrLabel = {"ทำควิซด้วยชุดมาตรฐาน", "ทำควิซด้วยชุดที่จัดไว้", "", "ดูผลการควิซ", "ต้องจัดชุดประเภทควิซ ก่อนค่ะ"}
                SqlIsMode = " AND ts.IsQuizMode = 1 "
            Case EnumDashBoardType.Homework
                GetArrStrLabel = {"สั่งจากชุดมาตรฐาน", "สั่งการบ้าน", "การบ้านนักเรียน", "ดูผลการทำการบ้าน", "ต้องจัดชุดประเภทการบ้าน ก่อนค่ะ"}
                SqlIsMode = " AND ts.IsHomeWorkMode = 1 "
            Case EnumDashBoardType.Practice
                If ClsKNSession.RunMode <> "" Then
                    If ClsKNSession.RunMode = "standalonenotablet" Then
                        GetArrStrLabel = {"ฝึกฝนด้วยชุดมาตรฐาน", "ฝึกฝนด้วยชุดที่เตรียมไว้", "", "ต้องจัดชุดประเภทฝึกฝน ก่อนค่ะ"}
                    Else
                        GetArrStrLabel = {"ฝึกฝนด้วยชุดมาตรฐาน", "ฝึกฝนด้วยชุดที่เตรียมไว้", "", "ดูผลการฝึกฝน", "ต้องจัดชุดประเภทฝึกฝน ก่อนค่ะ"}
                    End If
                End If


                SqlIsMode = " AND ts.IsPracticeMode = 1 "
            Case EnumDashBoardType.PrintTestset
                GetArrStrLabel = {"สร้างใบงานด้วยชุดมาตรฐาน", "สร้างใบงานด้วยชุดที่จัดไว้", "", "", "ต้องจัดชุดประเภทใบงานก่อนค่ะ"}
                SqlIsMode = " AND ts.IsReportMode = 1 "
            Case EnumDashBoardType.SetUp
                GetArrStrLabel = {"จัดชุดใหม่", "แก้ไขชุดเก่า", "", "", "ต้องจัดชุดประเภทชุด ก่อนค่ะ"}
        End Select
        Return GetArrStrLabel
    End Function

    Private Function GetMenuName() As Object
        Select Case _DashboardMode
            Case EnumDashBoardType.Quiz
                SqlIsMode = " AND ts.IsQuizMode = 1 "
                If ClsKNSession.RunMode = "standalonenotablet" Then
                    GetMenuName = New With {.Menu1 = "ทำควิซด้วยชุดมาตรฐาน",
                                              .Menu2 = "ทำควิซด้วยชุดที่จัดไว้",
                                              .Menu3 = "",
                                              .Menu4 = "",
                                              .HintTxt = "ต้องจัดชุดประเภทควิซ ก่อนค่ะ"
                                             }
                Else
                    GetMenuName = New With {.Menu1 = "ทำควิซด้วยชุดมาตรฐาน",
                                               .Menu2 = "ทำควิซด้วยชุดที่จัดไว้",
                                               .Menu3 = "",
                                               .Menu4 = "ดูผลการควิซ",
                                               .HintTxt = "ต้องจัดชุดประเภทควิซ ก่อนค่ะ"
                                              }
                End If
            Case EnumDashBoardType.Homework
                SqlIsMode = " AND ts.IsHomeWorkMode = 1 "
                GetMenuName = New With {.Menu1 = "สั่งจากชุดมาตรฐาน",
                                           .Menu2 = "สั่งการบ้าน",
                                           .Menu3 = "การบ้านนักเรียน",
                                           .Menu4 = "ดูผลการทำการบ้าน",
                                           .HintTxt = "ต้องจัดชุดประเภทการบ้าน ก่อนค่ะ"
                                          }
            Case EnumDashBoardType.Practice
                If ClsKNSession.RunMode <> "" Then
                    If ClsKNSession.RunMode = "standalonenotablet" Then
                        SqlIsMode = " AND ts.IsPracticeMode = 1 "
                        GetMenuName = New With {.Menu1 = "ฝึกฝนด้วยชุดมาตรฐาน",
                                                   .Menu2 = "ฝึกฝนด้วยชุดที่เตรียมไว้",
                                                   .Menu3 = "",
                                                   .Menu4 = "",
                                                   .HintTxt = "ต้องจัดชุดประเภทฝึกฝน ก่อนค่ะ"
                                                  }
                    Else
                        SqlIsMode = " AND ts.IsPracticeMode = 1 "
                        GetMenuName = New With {.Menu1 = "ฝึกฝนด้วยชุดมาตรฐาน",
                                                   .Menu2 = "ฝึกฝนด้วยชุดที่เตรียมไว้",
                                                   .Menu3 = "",
                                                   .Menu4 = "ดูผลการฝึกฝน",
                                                   .HintTxt = "ต้องจัดชุดประเภทฝึกฝน ก่อนค่ะ"
                                                  }
                    End If
                End If


            Case EnumDashBoardType.PrintTestset
                SqlIsMode = " AND ts.IsReportMode = 1 "
                GetMenuName = New With {.Menu1 = "สร้างใบงานด้วยชุดมาตรฐาน",
                                          .Menu2 = "สร้างใบงานด้วยชุดที่จัดไว้",
                                          .Menu3 = "",
                                          .Menu4 = "",
                                          .HintTxt = "ต้องจัดชุดประเภทใบงาน ก่อนค่ะ"
                                         }
            Case EnumDashBoardType.SetUp
                GetMenuName = New With {.Menu1 = "จัดชุดใหม่",
                                          .Menu2 = "แก้ไขชุดเก่า",
                                          .Menu3 = "",
                                          .Menu4 = "",
                                          .HintTxt = "ต้องจัดชุดประเภทชุด ก่อนค่ะ"
                                         }
        End Select
        Return GetMenuName
    End Function

    Private Function GetTestset(Optional ByRef InputConn As SqlConnection = Nothing) As DataTable
        Dim strSQL As New StringBuilder()

        strSQL.Append(" SELECT DISTINCT ts.TestSet_Id, ts.TestSet_Name, ts.LastUpdate , ts.GroupSubject_ShortName as SubjectName")
        strSQL.Append(" FROM tblTestSet ts inner join tbltestsetquestionset tsqs on ts.testset_Id = tsqs.testset_Id")
        strSQL.Append(" and ts.isactive = '1'")
        strSQL.Append(" and tsqs.isactive = '1'")
        'strSQL.Append(" AND UserId IN (SELECT Teacher_id FROM tblAssistant WHERE Assistant_id = '")
        strSQL.Append(" AND UserId = '")
        strSQL.Append(_UserId)
        strSQL.Append("' and ts.testset_id not in (select testset_id from tbltestset where testset_name = 'กำลังอยู่ระหว่างการจัดชุด'")
        strSQL.Append(" AND LastUpdate < DATEADD(hour,-8,dbo.GetThaiDate()))")
        strSQL.Append(" AND ts.UserType = 1 AND IsStandard = 0 AND tsqs.IsActive = '1'  ")
        strSQL.Append(SqlIsMode)
        strSQL.Append(" ORDER BY ts.LastUpdate DESC")

        Dim db As New ClassConnectSql()
        Return db.getdata(strSQL.ToString(), , InputConn)
    End Function

    ''' <summary>
    ''' function ในการ ข้อสอบ ที่มีความสัมพันธ์กับ user ที่ใช้งานอยู่
    ''' </summary>
    ''' <returns></returns>
    Private Function GetOtherTestset() As DataTable
        Dim sql As String = "DECLARE @userId AS UNIQUEIDENTIFIER = '" & _UserId & "'; "
        sql &= " Select qs.QSet_Id,qs.QSet_Name,qs.LastUpdate,b.GroupSubject_Id,t.Teacher_FirstName FROM tblQuestionset  qs INNER JOIN tblQuestionCategory qc ON qs.QCategory_Id = qc.QCategory_Id "
        sql &= " INNER JOIN tblBook b ON b.BookGroup_Id = qc.Book_Id INNER JOIN tblUserSubjectClass uc ON uc.LevelId = b.Level_Id AND uc.GroupSubjectId = b.GroupSubject_Id "
        sql &= " INNER JOIN t360_tblTeacher t On t.Teacher_id = qc.Parent_Id "
        sql &= " WHERE qs.IsWpp = 0 AND qc.IsWpp = 0  AND b.IsWpp = 0 AND qc.Parent_Id <> @userId AND uc.UserId = @userId;"
        Dim db As New ClassConnectSql()
        Return db.getdata(sql)
    End Function

    Private Function GetDatableOtherTestset() As DataTable
        Dim dt As DataTable = GetOtherTestset()
        If dt.Rows.Count > 0 Then
            dt.Columns.Add("TimeAgo", GetType(String))
            dt.Columns.Add("SubjectName", GetType(String))
            For Each row In dt.Rows
                row("TimeAgo") = Convert.ToDateTime(row("LastUpdate")).ToPointPlusTime()
                row("SubjectName") = row("GroupSubject_Id").ToString().ToSubjectShortThName()
            Next
        End If
        Return dt
    End Function

End Class


