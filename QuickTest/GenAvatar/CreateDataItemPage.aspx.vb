Imports System.IO


Public Class CreateDataItemPage
    Inherits System.Web.UI.Page
    Dim _DB As New ClassConnectSql()

    Private Sub InsertDataProcess()
        If TextBox1.Text <> "" Then
            Dim MydirInfo As DirectoryInfo = New DirectoryInfo(TextBox1.Text)
            _DB.OpenWithTransection()
            If MydirInfo.GetFiles.Count > 0 Then
                If TruncateShopItem(_DB) = -2 Then
                    Exit Sub
                End If
                For Each f As FileInfo In MydirInfo.GetFiles
                    Dim EachFileName As String = f.Name
                    Dim DotIndex As Integer = EachFileName.IndexOf(".")
                    Dim EachSICId As String = ""
                    If DotIndex <> -1 Then
                        EachFileName = EachFileName.Substring(0, DotIndex)
                        EachSICId = GetItemCategory(EachFileName)
                        If EachSICId <> "" Then
                            InsertItem(EachSICId, EachFileName, f.Name, _DB)
                        Else
                            InsertItem(EachSICId, EachFileName, f.Name, _DB, True)
                        End If
                    End If
                Next
                _DB.CommitTransection()
                Response.Write("Complete")
            Else
                Response.Write("ไม่เจอ File ใน Folder ที่ระบุมา")
                _DB.RollbackTransection()
            End If
        Else
            Response.Write("ต้องใส่ path ของ Folder ที่เก็บรูป")
        End If
    End Sub

    Private Function TruncateShopItem(ByRef Objdb As ClassConnectSql) As Integer
        Dim sql As String = " TRUNCATE TABLE dbo.tblShopItem "
        Try
            Objdb.ExecuteWithTransection(sql)
            Return 1
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Response.Write("Error ตอน Truncate ShopItem : " & ex.ToString() & "<br />")
            Objdb.RollbackTransection()
            Return -2
        End Try
    End Function

    Private Sub InsertItem(ByVal SICID As String, ByVal ShopItemName As String, ByVal ImageFileName As String, ByRef Objdb As ClassConnectSql, Optional ByVal SICIsNull As Boolean = False)
        Try
            Dim sql As String = ""
            If SICIsNull = False Then
                sql = " INSERT INTO dbo.tblShopItem (ShopItem_Id ,SIC_Id ,ShopItem_Name ,Image_FileName ,Seq_No ,IsActive ,LastUpdate ) " &
                                " VALUES  ( NEWID() , '" & SICID & "' , '" & ShopItemName & "' ,'" & ImageFileName & "' ,1 , 1 ,dbo.GetThaiDate() ) "
            Else
                sql = " INSERT INTO dbo.tblShopItem (ShopItem_Id ,SIC_Id ,ShopItem_Name ,Image_FileName ,Seq_No ,IsActive ,LastUpdate ) " &
                                " VALUES  ( NEWID() , '00000000-0000-0000-0000-000000000000' , '" & ShopItemName & "' ,'" & ImageFileName & "' ,1 , 1 ,dbo.GetThaiDate() ) "
            End If
            Objdb.ExecuteWithTransection(Sql)
        Catch ex As Exception When Not TypeOf ex Is Threading.ThreadAbortException
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex)
            Response.Write("Error ตอน InsertItem : " & ex.ToString() & "<br />")
            Objdb.RollbackTransection()
            Exit Sub
        End Try
    End Sub
 

    Private Function GetItemCategory(ByVal ItemName As String)

        Dim CategoryId As String = ""
        ItemName = ItemName.ToLower()

        If ItemName.IndexOf("body") <> -1 Then
            CategoryId = "3D107CAA-479D-4576-AA5B-5597A7E3D349"
        ElseIf ItemName.IndexOf("glass") <> -1 Then
            CategoryId = "C30984E7-6AA1-4D91-AFFF-A928D41852D3"
        ElseIf ItemName.IndexOf("scarf") <> -1 Then
            CategoryId = "6984E5B1-8883-4F27-A880-04C41525321F"
        ElseIf ItemName.IndexOf("pet") <> -1 Or ItemName.IndexOf("cat") <> -1 Or ItemName.IndexOf("dog") <> -1 Then
            CategoryId = "BD26B443-66FE-46D0-BE95-1520C557C568"
        ElseIf ItemName.IndexOf("watch") <> -1 Then
            CategoryId = "8EF6C59F-CDB8-4166-9750-3CAB72080592"
        ElseIf ItemName.IndexOf("hat") <> -1 Then
            CategoryId = "ED3D4633-361F-4680-BB05-8F12C9BF6AED"
        ElseIf ItemName.IndexOf("pant") <> -1 Or ItemName.IndexOf("skirt") <> -1 Then
            CategoryId = "F1B13930-78AA-4C57-BE94-7CCFF5BCD970"
        ElseIf ItemName.IndexOf("shoe") <> -1 Then
            CategoryId = "2203DAA6-854E-41EE-91B3-C2CAA7964A16"
        ElseIf ItemName.IndexOf("shirt") <> -1 Then
            CategoryId = "A4E647CF-9627-41C5-9DDF-D82489199B99"
        ElseIf ItemName.IndexOf("mobile") <> -1 Then
            CategoryId = "BECE9432-973E-480A-AF08-DAECAB979E12"
        ElseIf ItemName.IndexOf("armband") <> -1 Then
            CategoryId = "ADDE003E-D0B7-4504-868E-FA307B3AAF58"
        ElseIf ItemName.IndexOf("head") <> -1 Then
            CategoryId = "FC643000-AE5F-4406-A45B-4CEF7F29C592"
        ElseIf ItemName.IndexOf("room") <> -1 Then
            CategoryId = "E75E08E7-06D1-45E2-B03E-016D409FDC0B"
        End If
        Return CategoryId

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        InsertDataProcess()
    End Sub

End Class