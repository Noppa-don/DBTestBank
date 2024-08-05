Public Class CreateDialyActivities
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim db As New ClassConnectSql
        'สร้าง temp table เก็บข้อสอบของเด็กแต่ละคนที่ทำหลัง ตี 2 โดยใช้ store
        'db.Execute(" EXEC dbo.GetQuestionAfterAttwo", , 600)

        'สร้าง temp table2 เก็บข้อสอบที่เกี่ยวข้องกับ table1 โดยใช้ store
        'db.Execute(" EXEC dbo.RelationshipQuestion",, 600)

        ' สร้างกิจกรรมประจำวัน random ข้อสอบจาก table2
        Dim dialy As New DialyActivityManagement
        dialy.Run()

    End Sub

End Class