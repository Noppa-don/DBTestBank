Public Class FilterControl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            FilterMode.SelectedIndex = 0
            PanelFilter2.Visible = False
            PanelFilter1.Visible = True
        End If
    End Sub

    Private Sub FilterMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilterMode.SelectedIndexChanged
        If FilterMode.SelectedValue = 1 Then
            PanelFilter2.Visible = False
            PanelFilter1.Visible = True
        ElseIf FilterMode.SelectedValue = 2 Then
            PanelFilter1.Visible = False
            PanelFilter2.Visible = True
        End If
    End Sub

    Public Function GetDuratoinDate() As ArrayList
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        Dim ArrDurationDate As New ArrayList
        If FilterMode.SelectedValue = 1 Then
            Dim ValueOfDay As Integer = SelectedDate.SelectedValue
            Dim Date1 As String = DateAdd(DateInterval.Day, -ValueOfDay, Date.Now()).Date.ToString("yyyy/MM/dd")
            Dim Date2 As String = Date.Now.Date.ToString("yyyy/MM/dd")
            ArrDurationDate.Add(Date1)
            ArrDurationDate.Add(Date2)
        ElseIf FilterMode.SelectedValue = 2 Then
            Dim Date1 As Date = RadDatePicker1.SelectedDate.Value.Date
            Dim Date2 As Date = RadDatePicker2.SelectedDate.Value.Date
            If Date1.Year > Date2.Year Then
                Return ArrDurationDate
            End If
            If Date1.Month > Date2.Month Then
                Return ArrDurationDate
            End If
            If Date1.Day > Date2.Day Then
                Return ArrDurationDate
            End If
            ArrDurationDate.Add(Date1.ToString("yyyy/MM/dd"))
            ArrDurationDate.Add(Date2.ToString("yyyy/MM/dd"))
        End If
        Return ArrDurationDate
    End Function
 

End Class