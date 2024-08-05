Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.Reporting
Imports Telerik.Reporting.Drawing

Partial Public Class Report1
    Inherits Telerik.Reporting.Report
    Public Sub New()
        InitializeComponent()
        TestTest()
    End Sub

    Private Sub TestTest()

        'Dim dt As New DataTable
        'dt.Columns.Add("Test1", GetType(String))
        'dt.Columns.Add("Test2", GetType(String))

        'For index = 0 To 10
        '    dt.Rows.Add(index)("Test1") = index
        '    dt.Rows(index)("Test2") = index
        'Next

        For index = 1 To 10
            Dim newTxtbox As New Telerik.Reporting.TextBox
            newTxtbox.Name = "TxtBox" & index
            newTxtbox.Value = index
        Next

    End Sub

End Class