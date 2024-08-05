Public Class TestAction
    Inherits System.Web.UI.Page

    'StudentId = 6902A943-75FE-48E1-8263-7610074019F6 
    Dim _DB As New ClassConnectSql()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim ItemArr As New ArrayList
        'ItemArr.Add("8AE519A2-DE16-499D-BF4D-53C53D1B8431")
        'ItemArr.Add("7F3ED80C-8BD3-45F0-9707-8C69CF69246E")
        'ItemArr.Add("1F119A00-9267-4385-ACB2-C7AB2C96B29F")
        'GetCurrentStatus("06ac1670-f48a-4325-9675-e43030684a20", ItemArr, 11, 15)

        Dim ClsDP As New ClassDroidPad(New ClassConnectSql())
        ClsDP.GetSchoolRank("7ce16cb0-4bc3-41b5-b16a-37cf4368ca22")

    End Sub

#Region "GetCurrentStatus"
    Public Function GetCurrentStatus(ByVal DeviceId As String, ByVal ItemId As ArrayList, ByVal ResultGold As Integer, ByVal ResultSilver As Integer)

        If DeviceId = "" Or DeviceId Is Nothing Or ItemId.Count = 0 Or ItemId Is Nothing Then
            Return -1
        End If

        Dim StudentId As String = GetStudentIdByDeviceId(_DB.CleanString(DeviceId))
        Dim SchoolId As String = GetSchoolIdByDeviceId(_DB.CleanString(DeviceId))

        If StudentId <> "" And SchoolId <> "" Then
            Try
                'เปิด Transaction
                _DB.OpenWithTransection()
                Dim sql As String = " select COUNT(*) from tblStudentPoint where Student_Id = '" & StudentId & "' "
                Dim CheckIsInStudentPoint As String = _DB.ExecuteScalarWithTransection(sql)
                If CInt(CheckIsInStudentPoint) = 0 Then
                    'Insert tblStudentPoint
                    If InsertStudentPoint(StudentId, _DB) = -2 Then
                        _DB.RollbackTransection()
                        Return -2
                    End If
                End If

                Dim dt As DataTable = GetDtSilverAndGoldCoin(StudentId, _DB)
                Dim TotalSilver As Integer = 0
                Dim TotalGold As Integer = 0

                If dt.Rows.Count > 0 Then
                    TotalSilver = dt.Rows(0)("Silver") + ResultSilver
                    TotalGold = dt.Rows(0)("Gold") + ResultGold
                    'Update tblStudentPoint
                    If UpdateCurrentGoldAndSilver(StudentId, TotalSilver, TotalGold, _DB) = -2 Then
                        _DB.RollbackTransection()
                        Return -2
                    End If

                    'Update tblStudentItem set IsActive = 0 ก่อนที่จะ Insert อันล่าสุดลงไป
                    If SetIsActiveFalseStudentItem(StudentId, _DB) = -2 Then
                        _DB.RollbackTransection()
                        Return -2
                    End If

                    'Loop Insert tblStudentItem
                    For Each r In ItemId
                        If InsertLastStudentItem(StudentId, r, _DB) = -2 Then
                            _DB.RollbackTransection()
                            Return -2
                        End If
                    Next

                    'จบ ปิด Transaction
                    _DB.CommitTransection()
                Else
                    Return -1
                End If
                Return 0
            Catch ex As Exception
                'Error RollBackTransaction
                _DB.RollbackTransection()
                Return -2
            End Try
        Else
            Return -1
        End If

    End Function

    Public Function InsertLastStudentItem(ByVal StudentId As String, ByVal ItemId As String, ByRef _ObjDb As ClassConnectSql)
        Dim sql As String = " insert into tblStudentItem(SI_Id,Student_Id,ShopItem_Id,IsActive,LastUpdate) " & _
                            " values(NEWID(),'" & _ObjDb.CleanString(StudentId) & "','" & _ObjDb.CleanString(ItemId) & "',1,GETDATE()) "
        Try
            _ObjDb.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception
            Return -2
        End Try
    End Function

    Public Function SetIsActiveFalseStudentItem(ByVal StudentId As String, ByRef _ObjDB As ClassConnectSql)
        Dim sql As String = " update tblStudentItem set IsActive = 0 where Student_Id = '" & StudentId & "' "
        Try
            _ObjDB.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception
            Return -2
        End Try
    End Function

    Public Function UpdateCurrentGoldAndSilver(ByVal StudentId As String, ByVal TotalSilver As Integer, ByVal TotalGold As Integer, ByRef _ObjDB As ClassConnectSql)
        Dim sql As String = " update tblStudentPoint set Silver = '" & TotalSilver & "', Gold = '" & TotalGold & "' where Student_Id = '" & StudentId & "' "
        Try
            _ObjDB.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception
            Return -2
        End Try
    End Function

    Public Function GetDtSilverAndGoldCoin(ByVal StudentId As String, ByRef _ObjDB As ClassConnectSql)
        Dim sql As String = " select Silver,Gold from tblStudentPoint where Student_Id = '" & StudentId & "' "
        Dim dt As New DataTable
        dt = _ObjDB.getdataWithTransaction(sql)
        Return dt
    End Function

    Public Function InsertStudentPoint(ByVal StudentId As String, ByRef _ObjDb As ClassConnectSql)
        Dim sql As String = " insert into tblStudentPoint(StudentPoint_Id,Student_Id,Silver,Gold) " & _
                            " values(NEWID(),'" & StudentId & "',0,0) "
        Try
            _ObjDb.ExecuteWithTransection(sql)
            Return 0
        Catch ex As Exception
            Return -2
        End Try
    End Function

    Public Function GetStudentIdByDeviceId(ByVal DeviceId As String)
        Dim sql As String = " Select t360_tblTabletOwner.Owner_Id FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " & _
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) "
        Dim StudentId As String = _DB.ExecuteScalar(sql)
        Return StudentId
    End Function

    Public Function GetSchoolIdByDeviceId(ByVal DeviceId As String)
        Dim sql As String = " Select t360_tblTabletOwner.School_Code FROM t360_tblTablet INNER JOIN t360_tblTabletOwner ON t360_tblTablet.Tablet_Id = t360_tblTabletOwner.Tablet_Id " & _
                            " WHERE (t360_tblTablet.Tablet_SerialNumber = '" & DeviceId & "') AND (t360_tblTabletOwner.TabletOwner_IsActive = 1) "
        Dim SchoolId As String = _DB.ExecuteScalar(sql)
        Return SchoolId
    End Function

#End Region

    
End Class