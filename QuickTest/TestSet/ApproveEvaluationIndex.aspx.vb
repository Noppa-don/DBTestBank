Public Class ApproveEvaluationIndex
    Inherits System.Web.UI.Page
    Dim db As New ClassConnectSql
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") = Nothing Then
            Response.Redirect("~/LoginPage.aspx", False)
        Else
            If Not Page.IsPostBack Then
                BindDataOnLoad()
            End If
        End If
    End Sub

    '20240802 Load ตัวชี้วัดที่มีการเปลี่ยนแปลง
    Private Sub BindDataOnLoad()
        Dim sql As String
        sql = "select AEId,EiId,case when EIStatusType = 1 then 'แก้ไข' else 'ลบ' end + case when eilevel = 1 then 'ดัชนี' when EILevel = 2 then 'กลุ่มดัชนี' else 'ตัวชี้วัด' end 
                + case when EIStatusType = 1 then '  จาก ' + eiold + ' เป็น ' + einew else '  ' + eiold end as EIDetial
                from tblApproveEvaluationIndex where ApproveStatus = 0 order by lastupdate desc;"

        Dim dt As New DataTable
        dt = db.getdata(sql)

        Dim detailTxt As String = ""

        If dt.Rows.Count <> 0 Then
            detailTxt &= "<div class='dtContainer'>"
            For i = 0 To dt.Rows.Count - 1
                If i Mod 2 = 0 Then detailTxt &= "<div class='dtDetail1'>" Else detailTxt &= "<div class='dtDetail2'>"

                detailTxt &= "<div class='dtChild'>" & dt.Rows(i)("EIDetial") & "</div>
                              <div id='Approve'><div class='btn btnApproved' Eiid='" & dt.Rows(i)("EiId").ToString & "' AEId='" & dt.Rows(i)("AEId").ToString & "'>อนุมัติ</div></div>
                              <div id='NotApprove'><div class='btn btnNotApproved' Eiid='" & dt.Rows(i)("EiId").ToString & "' AEId='" & dt.Rows(i)("AEId").ToString & "'>ไม่อนุมัติ</div></div></div>"
            Next
            detailTxt &= "</div>"
        End If

        ContentDiv.innerhtml = detailTxt
    End Sub

    '20240802 บันทึกผลการอนุมัติ
    <Services.WebMethod()>
    Public Shared Function UpdateApproveEI(ApproveType As String, EIId As String, AEId As String)
        Dim sql As String
        Dim db As New ClassConnectSql
        Dim Result As String
        Try
            sql = "select EIStatusType from tblApproveEvaluationIndex where AEId = '" & AEId & "';"

            Dim EiStatus As String = db.ExecuteScalar(sql)

            If ApproveType = "1" Then
                If EiStatus = "1" Then
                    sql = "Update tblEvaluationIndexnew set EI_Code = (select einew from tblApproveEvaluationIndex where AEId = '" & AEId & "') where EI_Id = '" & EIId & "';
                    Update tblApproveEvaluationIndex set ApproveStatus = 1 , approveuser = '" & HttpContext.Current.Session("UserId").ToString & "' where AEId = '" & AEId & "';"
                Else
                    sql = "Update tblApproveEvaluationIndex set ApproveStatus = 1 , approveuser = '" & HttpContext.Current.Session("UserId").ToString & "' where AEId = '" & AEId & "';"
                End If
            Else
                If EiStatus = "1" Then
                    sql = "Update tblApproveEvaluationIndex set ApproveStatus = 2 , approveuser = '" & HttpContext.Current.Session("UserId").ToString & "'  where AEId = '" & AEId & "'"
                Else
                    sql = "Update tblEvaluationIndexnew set IsActive = 1 where EI_Id = '" & EIId & "';
                        Update tblApproveEvaluationIndex set ApproveStatus = 2 , approveuser = '" & HttpContext.Current.Session("UserId").ToString & "'  where AEId = '" & AEId & "'"
                End If
            End If
            db.Execute(sql)

            Result = "complete"
        Catch ex As Exception
            Result = ex.ToString
        End Try
        Return Result
    End Function

End Class