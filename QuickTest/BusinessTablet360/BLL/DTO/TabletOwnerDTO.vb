Public Class TabletOwnerDTO

    Public Property School_Code As String
    Public Property TON_Id As Guid?
    Public Property Tablet_Id As Guid?
    Public Property Tablet_SerialNumber As String
    Public Property Tablet_SerialNumber_Like As String
    Public Property TabletLab_Id As Guid?
    Public Property Tablet_TabletName As String
    Public Property Tablet_IsActive As Boolean?
    Public Property Tablet_Status As Integer

    ' add new prop
    Public Property OwnerCode As String
    Public Property PrefixName As String
    Public Property FirstName As String
    Public Property LastName As String

    Public Property Owner_Id As Guid?
    Public Property Owner_Type As Byte?
    Public Property Student_Id As Guid?
    Public Property Student_Code As String
    Public Property Student_Code_Like As String
    Public Property Student_PrefixName As String
    Public Property Student_FirstName As String
    Public Property Student_FirstName_Like As String
    Public Property Student_LastName As String
    Public Property Student_LastName_Like As String
    Public Property Student_FullName As String
    Public Property Student_CurrentClass As String
    Public Property Student_CurrentRoom As String
    Public Property Student_IsActive As Boolean?
    Public Property Student_ParentName As String
    Public Property Student_ParentPhone As String
    Public Property Teacher_Id As Guid?
    Public Property Teacher_Code As String
    Public Property Teacher_PrefixName As String
    Public Property Teacher_FirstName As String
    Public Property Teacher_FirstName_Like As String
    Public Property Teacher_LastName As String
    Public Property Teacher_LastName_Like As String
    Public Property Teacher_FullName As String
    Public Property Teacher_CurrentClass As String
    Public Property Teacher_CurrentRoom As String
    Public Property Teacher_IsActive As Boolean?
    Public Property Teacher_Phone As String
    Public Property FullName As String
    Public Property Phone As String
    Public Property TON_ReceiveDate As Date?
    Public Property TON_ReceiveDateString As String
    Public Property TON_ReturnDate As Date?
    Public Property TabletOwner_IsActive As Boolean?
    Public Property TLG_TimeStamp As Date?
    Public Property TLG_TimeStampString As String
    Public Property AssetNo As String

    Public Property TSD_Status As Byte
    Public Property TSD_StatusName As String
    Public Property TSD_WhereStatus As String


    Public Sub New()
        TabletOwner_IsActive = True
        Tablet_IsActive = True

    End Sub

End Class
