Public Class Scheduler

    Inherits System.Web.UI.Page
    Dim ClsScheduler As New ClsScheduler(New ClassConnectSql)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If HttpContext.Current.Application("NeedEditButton") Is Nothing Then
                Try
                    KNConfigData.LoadData()
                Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
                End Try

            End If

            ClsScheduler.NextSemester(HttpContext.Current.Application("DefaultSchoolCode"))

        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
        End Try

    End Sub

End Class