Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Public Class Report_SumUsageReport
    Inherits Telerik.Reporting.Report
    Public Sub New(ByVal Source As List(Of ClsSumUsageReport), ByVal Title As String, ByVal DetailReport As String)
        InitializeComponent()
        Me.DataSource = Source
        titleTextBox.Value = Title
        txtDetailReport.Value = DetailReport
    End Sub
End Class