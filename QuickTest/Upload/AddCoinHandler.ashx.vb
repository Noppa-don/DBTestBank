Imports System.Web
Imports System.Web.Services

Public Class AddCoinHandler
    Implements System.Web.IHttpHandler

    Private db As New ClassConnectSql

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim returnValue As String = ""
        Dim deviceId As String = context.Request.Form("method")

        Dim owner As DataTable = GetOwner(deviceId)
        If owner.Rows.Count > 0 Then
            Dim studentId As String = owner.Rows(0)("Student_Id").ToString()
            Dim schoolCode As String = owner.Rows(0)("School_code").ToString()
            'If Not IsStudentPointIsExist(studentId) Then
            '    AddTwentyCoin(studentId, schoolCode)
            '    returnValue = "Add 20 Every thing Coin Success!!"
            'Else
            '    returnValue = "Coin Has Ready!, No Add Coin Again!"
            'End If
            If UpdateCoin(studentId) Then
                returnValue = "Add 20 Every thing Coin Success!!"
            Else
                returnValue = "Add Coin Fail!!"
            End If
        Else
            returnValue = "No Owner!, No Add Coin!"
        End If

        context.Response.Write(returnValue)
        context.Response.End()
    End Sub

    Private Function GetOwner(deviceId As String) As DataTable
        Dim sql As String = "SELECT * FROM t360_tblTablet t INNER JOIN t360_tblTabletOwner o ON t.Tablet_Id = o.Tablet_Id INNER JOIN t360_tblStudent s ON o.Owner_Id = s.Student_Id "
        sql &= " WHERE t.Tablet_SerialNumber = '" & deviceId & "' AND t.Tablet_IsActive = 1 AND o.TabletOwner_IsActive = 1;"
        Return db.getdata(sql)
    End Function

    Private Function IsStudentPointIsExist(studentId As String) As Boolean
        Dim sql As String = "SELECT * FROM tblStudentPoint WHERE Student_Id = '" & studentId & "';"
        Dim dt As DataTable = db.getdata(sql)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Sub AddTwentyCoin(studentId As String, schoolCode As String)
        Try
            Dim sql As String = "INSERT INTO tblStudentPoint VALUES (NEWID(),'" & studentId & "',20,20,20,20,20,20,dbo.GetThaiDate(),1,0,0,NULL,'" & schoolCode & "');"
            db.Execute(sql)
        Catch ex As Exception

        End Try
    End Sub

    Private Function UpdateCoin(studentId As String) As String
        Try
            Dim sql As String = "UPDATE tblStudentPoint SET Silver = 20,Gold = 20,Diamond = 20,TotalSilver = 20,TotalGold = 20,TotalDiamond = 20 WHERE Student_Id = '" & studentId & "';"
            db.Execute(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class