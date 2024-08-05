Imports System.Runtime.InteropServices

Public Class ChangeSystemDateMaxonetCI
    Inherits System.Web.UI.Page


    <StructLayout(LayoutKind.Sequential)>
    Public Structure SYSTEMTIME
        Public Year As UShort
        Public Month As UShort
        Public DayOfWeek As UShort
        Public Day As UShort
        Public Hour As UShort
        Public Minute As UShort
        Public Second As UShort
        Public Milliseconds As UShort

        Public Sub New(dt As DateTime)

            Year = CUShort(dt.Year)
            Month = CUShort(dt.Month)
            DayOfWeek = CUShort(dt.DayOfWeek)
            Day = CUShort(dt.Day)
            Hour = CUShort(dt.Hour - 7)
            Minute = CUShort(dt.Minute)
            Second = CUShort(dt.Second)
            Milliseconds = CUShort(dt.Millisecond)
        End Sub
    End Structure

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function SetSystemTime(ByRef time As SYSTEMTIME) As Boolean
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblCurrentDate.Text = Date.Now
    End Sub

    Protected Sub btnChageSysDate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeSysDate.Click
        Dim currentDate As DateTime = Date.Now

        Dim tempDate As DateTime = New DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, 0)
        Dim setDate As DateTime

        tempDate = tempDate.AddDays(1)
        setDate = New DateTime(tempDate.Year, tempDate.Month, tempDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second, 0)
        'Response.Write($" currentDate = {currentDate}")
        'Response.Write("<br />")
        'Response.Write($" tempDate = {tempDate}")
        'Response.Write("<br />")
        'Response.Write($" setDate = {setDate}")
        'Response.Write("<br />")
        'Response.Write("<br />")
        'Response.Write("summary to set")
        'Response.Write("<br />")
        'Response.Write($"Year = {CUShort(setDate.Year)}")
        'Response.Write("<br />")
        'Response.Write($"Month = {CUShort(setDate.Month)}")
        'Response.Write("<br />")
        'Response.Write($"DayOfWeek = {CUShort(setDate.DayOfWeek)}")
        'Response.Write("<br />")
        'Response.Write($"Day = {CUShort(setDate.Day)}")
        'Response.Write("<br />")
        'Response.Write($"Hour = {CUShort(setDate.Hour)}")
        'Response.Write("<br />")
        'Response.Write($"Minute = {CUShort(setDate.Minute)}")
        'Response.Write("<br />")
        'Response.Write($"Second = {CUShort(setDate.Second)}")
        'Response.Write("<br />")
        'Response.Write($"Milliseconds = {CUShort(setDate.Millisecond)}")

        Dim systime As SYSTEMTIME = New SYSTEMTIME(setDate)
        SetSystemTime(systime)

        lblCurrentDate.Text = Date.Now
    End Sub

End Class