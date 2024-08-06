Public Class TabletStatusDTO

    Public Property School_Code As String
    Public Property Tablet_Status As Byte
    Public Property Count_Status As Integer
    Public Property Percent_Status As Decimal
    Public Property Tablet_StatusName As String


    Public Property TabletId As Guid
    Public Property TSD_Status As EnumTabletStatus
    Public Property TSD_SendToFixOrLostDate As String
    Public Property TSD_FollowUp As String
    Public Property TSD_FollowUpDate As String
    Public Property TSD_SendToFixOrLostReportAt As String
    Public Property TSD_SendToFixOrLostReportDocNo As String
    Public Property TSD_SendToFixTel As String
    Public Property TSD_Remark As String
    Public Property SchoolCode As String
    Public Property LastUpdate As String

End Class
