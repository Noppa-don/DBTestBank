Imports Telerik.Web.UI
Imports System.Web.Services

Public Class CreateEvaluationIndex
    Inherits System.Web.UI.Page
    Dim useCls As New ClassConnectSql


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            lblWarn.Visible = False
            If Not Page.IsPostBack Then
                BindDataOnLoad()
            End If
        End If

    End Sub
    '20240731 Load ตัวขี้วัด
    Private Sub BindDataOnLoad()

        'หาชื่อดัชนี ---> Index ที่ Parent เป็น null
        ListIndexName.Items.Clear()
        Dim sql1 As String = "select distinct ei.EI_Id,EI_Code,Parent_Id from tblEvaluationIndexNew ei inner join tblEvaluationIndexSubject es on EI.EI_Id = es.EI_Id
                                inner join tblUserSubjectClass usc on es.Subject_Id = usc.GroupSubjectId
                                where Parent_Id is null and usc.isactive = 1 and es.isactive = 1 And ei.IsActive = 1 
                                and usc.userId = '" & Session("UserId").ToString & "' order by ei.EI_Code;"
        Dim dt1 As New DataTable
        dt1 = useCls.getdata(sql1)

        If dt1.Rows.Count > 0 Then

            For i = 0 To dt1.Rows.Count - 1
                ListIndexName.Items.Add(New RadListBoxItem With {.Text = dt1.Rows(i)("EI_Code"), .Value = dt1.Rows(i)("EI_Id").ToString})
            Next

            If Session("IndexNameID") IsNot Nothing Then
                ListIndexName.SelectedValue = Session("IndexNameID").ToString
            Else
                ListIndexName.SelectedIndex = 0
                Session("IndexNameID") = dt1.Rows(0)("EI_Id").ToString
            End If

            'หากลุ่มดัชนี ---> Index ที่ Parent เป็น ชื่อดัชนี

            Dim sql2 As String = "select EI_Id,EI_Code from tblEvaluationIndexNew where Parent_Id = '" & ListIndexName.SelectedValue.ToString & "' And IsActive = 1 order by EI_Code;"
            Dim dt2 As New DataTable
            dt2 = useCls.getdata(sql2)

            If dt2.Rows.Count > 0 Then
                For j = 0 To dt2.Rows.Count - 1
                    ListIndexGroupName.Items.Add(New RadListBoxItem With {.Text = dt2.Rows(j)("EI_Code"), .Value = dt2.Rows(j)("EI_Id").ToString})
                Next
                ListIndexGroupName.SelectedIndex = 0
                Session("IndexGroupID") = dt2.Rows(0)("EI_Id").ToString

                'หาตัวชี้วัด ---> Index ที่ Parent เป็น กลุ่มดัชนี
                Dim sql3 As String = " select EI_Id,EI_Code from tblEvaluationIndexNew where Parent_Id  = '" & dt2.Rows(0)("EI_Id").ToString & "' And IsActive = 1 order by EI_Code; "
                Dim dt3 As New DataTable
                dt3 = useCls.getdata(sql3)
                If dt3.Rows.Count > 0 Then
                    ListIndexItem.Items.Clear()
                    For b = 0 To dt3.Rows.Count - 1
                        ListIndexItem.Items.Add(New RadListBoxItem With {.Text = dt3.Rows(b)("EI_Code"), .Value = dt3.Rows(b)("EI_Id").ToString})
                    Next
                    ListIndexItem.SelectedIndex = 0
                    Session("IndexItemId") = dt3.Rows(0)("EI_Id").ToString

                End If

            End If

        End If

    End Sub
    '20240801 Click List แล้วให้แสดงตัวชี้วัด Level ถัดไป
#Region "Click List"

    Private Sub ListIndexName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListIndexName.SelectedIndexChanged

        Session("IndexNameID") = ListIndexName.SelectedValue.ToString
        ListIndexGroupName.Items.Clear()

        Dim sql2 As String = "select EI_Id,EI_Code from tblEvaluationIndexNew where Parent_Id = '" & ListIndexName.SelectedValue.ToString & "' And IsActive = 1 order by EI_Code;"

        Dim dt2 As New DataTable
        dt2 = useCls.getdata(sql2)
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1
                ListIndexGroupName.Items.Add(New RadListBoxItem With {.Text = dt2.Rows(i)("EI_Code"), .Value = dt2.Rows(i)("EI_Id").ToString})
            Next
            ListIndexGroupName.SelectedIndex = 0
            Session("IndexGroupID") = dt2.Rows(0)("EI_Id").ToString
        End If

        ListIndexItem.Items.Clear()

        If dt2.Rows.Count > 0 Then
            Dim sql3 As String = " select EI_Id,EI_Code from tblEvaluationIndexNew where Parent_Id  = '" & dt2.Rows(0)("EI_Id").ToString & "' And IsActive = 1 order by EI_Code; "
            Dim dt3 As New DataTable
            dt3 = useCls.getdata(sql3)

            For j = 0 To dt3.Rows.Count - 1
                ListIndexItem.Items.Add(New RadListBoxItem With {.Text = dt3.Rows(j)("EI_Code"), .Value = dt3.Rows(j)("EI_Id").ToString})
            Next
            ListIndexItem.SelectedIndex = 0
            Session("IndexItemId") = dt2.Rows(0)("EI_Id").ToString
        End If
    End Sub

    Private Sub ListIndexGroupName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListIndexGroupName.SelectedIndexChanged

        Session("IndexGroupID") = ListIndexGroupName.SelectedValue.ToString

        ListIndexItem.Items.Clear()

        Dim sql2 As String = " select EI_Id,EI_Code from tblEvaluationIndexNew where Parent_Id  = '" & ListIndexGroupName.SelectedValue.ToString & "' And IsActive = 1 order by EI_Code; "
        Dim dt2 As New DataTable
        dt2 = useCls.getdata(sql2)
        If dt2.Rows.Count > 0 Then
            For i = 0 To dt2.Rows.Count - 1
                ListIndexItem.Items.Add(New RadListBoxItem With {.Text = dt2.Rows(i)("EI_Code"), .Value = dt2.Rows(i)("EI_Id").ToString})
            Next

            ListIndexItem.SelectedIndex = 0
            Session("IndexItemId") = dt2.Rows(0)("EI_Id").ToString
        End If
    End Sub

    Private Sub ListIndexItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListIndexItem.SelectedIndexChanged
        Session("IndexItemId") = ListIndexItem.SelectedValue.ToString
    End Sub

#End Region
    '20240801 เพิ่มตัวชี้วัด
#Region "New"

    <WebMethod()>
    Public Shared Function SaveNewIndexCodeBehind(ByVal dataNewIndexName As String) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If
        Dim IndexId As Guid = Guid.NewGuid()
        Dim UseCls As New ClassConnectSql
        Dim sql As String
        sql = "Insert into tblEvaluationIndexNew select '" & IndexId.ToString & "','" & dataNewIndexName & "',
                            null,null,0,null,1,dbo.GetThaiDate(),null,max(EI_Position) + 1,null,null from tblEvaluationIndexNew 
                            where Parent_Id is null and IsActive = 1;"

        Try
            UseCls.Execute(sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try

        Try
            Dim SubjectNew As String
            sql = "select distinct GroupSubjectId from tblUserSubjectClass where UserId = '" & HttpContext.Current.Session("UserId").ToString & "';"
            Dim dt As DataTable = UseCls.getdata(sql)

            For Each r In dt.Rows()
                Dim GroupSubjectId As String = r("GroupSubjectId").ToString
                sql = "Insert into tblEvaluationIndexSubject select newid(),'" & IndexId.ToString & "','" & GroupSubjectId & "',1,dbo.GetThaiDate(),null,null"
                UseCls.Execute(sql)
                SubjectNew = GroupSubjectId & " "
            Next
            Dim StrLog As String = "เพิ่มชื่อดัชนี " & dataNewIndexName & " Id " & IndexId.ToString & " วิชา " & SubjectNew
            Log.Record(Log.LogType.ManageExam, UseCls.CleanString(StrLog), True)
            HttpContext.Current.Session("IndexNameID") = IndexId.ToString
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Return 0
        End Try



    End Function

    <WebMethod()>
    Public Shared Function SaveNewIndexGroupCodeBehind(ByVal dataNewIndexGroupName As String) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim IndexGroupId As Guid = Guid.NewGuid()
        Dim Usecls As New ClassConnectSql

        If HttpContext.Current.Session("IndexNameID") Is Nothing Then
            HttpContext.Current.Response.Write("โปรดเลือกชื่อดัชนี หรือ สร้างดัชนีก่อน")
        Else
            Dim IndexNameId As String = HttpContext.Current.Session("IndexNameID")
            Dim sql As String
            sql = "Insert into tblEvaluationIndexNew SELECT '" & IndexGroupId.ToString & "','" & dataNewIndexGroupName & "',null,
                    '" & IndexNameId.ToString & "',1,'0',1,dbo.GetThaiDate(),null,max(EI_Position) + 1,null,null 
                    FROM tblEvaluationIndexNew where Parent_Id = '" & IndexNameId.ToString & "';"
            Try
                Usecls.Execute(sql)
                HttpContext.Current.Session("IndexGroupID") = IndexGroupId.ToString
                Dim StrLog As String = "เพิ่มกลุ่มดัชนี " & dataNewIndexGroupName & " Id " & IndexGroupId.ToString
                Log.Record(Log.LogType.ManageExam, Usecls.CleanString(StrLog), True)
                Return 1
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return 0
            End Try
        End If



    End Function

    <WebMethod()>
    Public Shared Function SaveNewIndexItemCodeBehind(ByVal dataNewIndexItemName As String) As Boolean
        If HttpContext.Current.Session("UserId") Is Nothing OrElse (Guid.TryParse(HttpContext.Current.Session("UserId").ToString(), New Guid) = False) Then
            Return 0
        End If

        Dim IndexItemId As Guid = Guid.NewGuid()
        Dim UseCls As New ClassConnectSql

        If HttpContext.Current.Session("IndexGroupID") Is Nothing Then
            HttpContext.Current.Response.Write("โปรดเลือกกลุ่มดัชนีก่อน หรือ สร้างกลุ่มดัชนีก่อน")
        Else
            Dim IndexGroupId As String = HttpContext.Current.Session("IndexGroupID").ToString
            Dim sql As String
            sql = "Insert into tblEvaluationIndexNew SELECT '" & IndexItemId.ToString & "','" & dataNewIndexItemName & "'
                    ,null,'" & IndexGroupId.ToString & "',2,'0',1,dbo.GetThaiDate(),null,max(EI_Position) + 1,null,null
                    FROM tblEvaluationIndexNew where Parent_Id = '" & IndexGroupId.ToString & "';"
            Try
                UseCls.Execute(sql)
                HttpContext.Current.Session("IndexGroupID") = IndexItemId.ToString
                Dim StrLog As String = "เพิ่มตัวชี้วัด " & dataNewIndexItemName & " Id " & IndexItemId.ToString
                Log.Record(Log.LogType.ManageExam, UseCls.CleanString(StrLog), True)
                Return 1
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                Return 0
            End Try
        End If

    End Function

#End Region
    '20240802 แก้ไขและลบตัวชี้วัด
#Region "Edit"
    <WebMethod()>
    Public Shared Function UpdateNewIndexName(ByVal dataNewIndexName As String) As Boolean

        Dim selected As String = HttpContext.Current.Session("IndexNameID")

        Dim IndexId As String = selected.ToString
        Dim UseCls As New ClassConnectSql

        Dim sql As String
        sql = "insert into tblApproveEvaluationIndex select newid(),1,ei_Id,1,0,EI_Code,
               '" & dataNewIndexName & "','" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
               from tblEvaluationIndexNew where ei_Id = '" & IndexId & "';
               Update  tblevaluationIndexNew set Ei_Code = '" & dataNewIndexName & "',LastUpdate = Getdate() where EI_Id = '" & IndexId & "'; "
        Try
            UseCls.Execute(sql)
            HttpContext.Current.Session("IndexNameID") = IndexId.ToString
            Dim StrLog As String = "แก้ไขดัชนี " & dataNewIndexName & " Id =" & IndexId.ToString
            Log.Record(Log.LogType.ManageExam, UseCls.CleanString(StrLog), True)
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Log.Record(Log.LogType.ManageExam, UseCls.CleanString(ex.ToString), True)
            Return 0
        End Try
    End Function
    <WebMethod()>
    Public Shared Function UpdateIndexGroup(ByVal dataNewIndexGroupName As String) As Boolean

        Dim IndexGroupId As String = HttpContext.Current.Session("IndexGroupID")
        Dim Usecls As New ClassConnectSql

        If IndexGroupId = "" Then
            HttpContext.Current.Response.Write("โปรดเลือกชื่อดัชนี หรือ สร้างดัชนีก่อน")
        Else
            Dim sql As String
            sql = "insert into tblApproveEvaluationIndex select newid(),2,ei_Id,1,0,EI_Code,
                   '" & dataNewIndexGroupName & "','" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
                   from tblEvaluationIndexNew where ei_Id = '" & IndexGroupId & "';
                   Update  tblevaluationIndexNew set Ei_Code = '" & dataNewIndexGroupName & "',LastUpdate = Getdate() where EI_Id = '" & IndexGroupId & "';"
            Try
                Usecls.Execute(sql)
                HttpContext.Current.Session("IndexGroupID") = IndexGroupId.ToString
                Dim StrLog As String = "แก้ไขกลุ่มดัชนี " & dataNewIndexGroupName & " Id =" & IndexGroupId.ToString
                Log.Record(Log.LogType.ManageExam, Usecls.CleanString(StrLog), True)
                Return 1
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Log.Record(Log.LogType.ManageExam, Usecls.CleanString(ex.ToString), True)
                Return 0
            End Try
        End If
    End Function
    <WebMethod()>
    Public Shared Function UpdateIndexItem(ByVal dataNewIndexItemName As String) As Boolean
        Dim IndexItemId As String = HttpContext.Current.Session("IndexItemId").ToString
        Dim UseCls As New ClassConnectSql

        If IndexItemId Is Nothing Then
            HttpContext.Current.Response.Write("โปรดเลือกกลุ่มดัชนีก่อน หรือ สร้างกลุ่มดัชนีก่อน")
        Else
            Dim sql As String
            sql = "insert into tblApproveEvaluationIndex select newid(),3,ei_Id,1,0,EI_Code,
                   '" & dataNewIndexItemName & "','" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
                   from tblEvaluationIndexNew where ei_Id = '" & IndexItemId & "';
                   Update  tblevaluationIndexNew set Ei_Code = '" & dataNewIndexItemName & "',LastUpdate = Getdate() where EI_Id = '" & IndexItemId & "';"

            Try
                UseCls.Execute(sql)
                HttpContext.Current.Session("IndexItemId") = IndexItemId.ToString
                Dim StrLog As String = "แก้ไขตัวชี้วัด " & dataNewIndexItemName & " Id =" & IndexItemId.ToString
                Log.Record(Log.LogType.ManageExam, UseCls.CleanString(StrLog), True)
                Return 1
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Log.Record(Log.LogType.ManageExam, UseCls.CleanString(ex.ToString), True)
                Return 0
            End Try
        End If
    End Function
#End Region

#Region "Delete"

    Protected Sub btnDeleteIndexName_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteIndexName.Click

        Dim IndexnameSelected As String = ListIndexName.SelectedValue.ToString
        Dim IndexText As String = ListIndexName.SelectedItem.ToString

        If IndexnameSelected <> "" Then

            Dim sql As String

            sql = "Update tblevaluationIndexNew set IsActive = 0 , Lastupdate = getdate() where ei_ID = '" & IndexnameSelected & "';
                    insert into tblApproveEvaluationIndex select newid(),1,ei_Id,2,0,EI_Code,null ,'" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
                    from tblEvaluationIndexNew where ei_Id = '" & IndexnameSelected & "';"

            Try
                useCls.Execute(sql.ToString)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try

            Dim StrLog As String = "ลบดัชนี " & IndexText & " Id =" & IndexnameSelected.ToString
            Log.Record(Log.LogType.ManageExam, useCls.CleanString(StrLog), True)

            Session("IndexNameID") = Nothing
            Response.Redirect("CreateEvaluationIndex.aspx")
        Else
            lblWarn.Visible = True
            lblWarn.Text = "ต้องเลือกดัชนีก่อนจึงจะสามารถลบได้"
        End If

    End Sub

    Protected Sub btnDeleteIndexGroupName_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteIndexGroupName.Click


        Dim IndexGroupSelected As String = ListIndexGroupName.SelectedValue.ToString
        Dim IndexGroupText As String = ListIndexGroupName.SelectedItem.ToString
        If IndexGroupSelected <> "" Then

            Dim sql As String
            sql = "Update tblevaluationIndexNew set IsActive = 0 , Lastupdate = getdate() where ei_ID = '" & IndexGroupSelected & "';
                    insert into tblApproveEvaluationIndex select newid(),2,ei_Id,2,0,EI_Code,null ,'" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
                    from tblEvaluationIndexNew where ei_Id = '" & IndexGroupSelected & "';"

            Try
                useCls.Execute(sql.ToString)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            End Try
            Dim StrLog As String = "ลบกลุ่มดัชนี " & IndexGroupText & " Id =" & IndexGroupSelected.ToString
            Log.Record(Log.LogType.ManageExam, useCls.CleanString(StrLog), True)
            Session("IndexGroupID") = Nothing
            Response.Redirect("CreateEvaluationIndex.aspx")

        Else
            lblWarn.Visible = True
            lblWarn.Text = "ต้องเลือกกลุ่มดัชนีก่อนจึงจะสามารถลบได้"
        End If


    End Sub

    Private Sub btnDeleteIndexItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteIndexItem.Click

        Dim IndexItemSelected As String = ListIndexItem.SelectedValue.ToString
        Dim IndexItemText As String = ListIndexItem.SelectedItem.ToString
        If IndexItemSelected <> "" Then

            Dim sql As String
            sql = "Update tblevaluationIndexNew set IsActive = 0 , Lastupdate = getdate() where ei_ID = '" & IndexItemSelected & "';
                    insert into tblApproveEvaluationIndex select newid(),3,ei_Id,2,0,EI_Code,null ,'" & HttpContext.Current.Session("UserId").ToString & "',null,1,getdate() 
                   from tblEvaluationIndexNew where ei_Id = '" & IndexItemSelected & "';"

            Try
                useCls.Execute(sql.ToString)
            Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                lblWarn.Text = ex.ToString
            End Try
            Dim StrLog As String = "ลบดัชนี " & IndexItemText & " Id =" & IndexItemSelected.ToString
            Log.Record(Log.LogType.ManageExam, useCls.CleanString(StrLog), True)
            Session("IndexItemId") = Nothing
            Response.Redirect("CreateEvaluationIndex.aspx")

        Else
            lblWarn.Visible = True
            lblWarn.Text = "ต้องเลือกตัวชี้วัดก่อนจึงจะสามารถลบได้"
        End If



    End Sub

#End Region

End Class