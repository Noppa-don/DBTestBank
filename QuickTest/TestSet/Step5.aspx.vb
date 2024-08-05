Imports System.Web
Public Class Step5
    Inherits System.Web.UI.Page
    Public ChkFontSize, txtStep1, txtStep2, txtStep3, txtStep4 As String
    Public GroupName As String
    Dim ClsSelectSess As New ClsSelectSession

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If (Session("UserId") Is Nothing) Then
            Response.Redirect("~/LoginPage.aspx")
        Else

            GroupName = Session("selectedSession").ToString() 'GroupName

            ClsSelectSess.checkCurrentPage(Session("UserId").ToString(), Session("selectedSession").ToString())

            If HttpContext.Current.Application("NeedQuizMode") = True Then
                ChkFontSize = "16px"
                txtStep1 = "ขั้นที่ 1: จัดชุดควิซ -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึกชุดควิซ -->"
            Else
                ChkFontSize = "20px"
                txtStep1 = "ขั้นที่ 1: จัดชุด -->"
                txtStep2 = "ขั้นที่ 2: เลือกวิชา -->"
                txtStep3 = "ขั้นที่ 3: เลือกหน่วยการเรียนรู้ -->"
                txtStep4 = "ขั้นที่ 4: บันทึก และจัดพิมพ์"
            End If

        End If
    End Sub

    <Services.WebMethod()>
    Public Shared Function getAppsettingNeedcheckmark() As Boolean
        If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then
            getAppsettingNeedcheckmark = True
        Else
            getAppsettingNeedcheckmark = False
        End If
        Return getAppsettingNeedcheckmark
    End Function


    Private Sub GoToGenPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles GoToGenPDF.Click

        Response.Redirect("~/TestSet/GenPDF.aspx")

    End Sub


End Class