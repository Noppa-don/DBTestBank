Public Class MainScoreReport
    Inherits System.Web.UI.Page

    Dim dtSubName As New DataTable
    Dim dtStdName As New DataTable
    Dim dtScore As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ParamRoomId As String = ""
        Dim ParamQuizMode As String = "Practice"
        Dim ParamGroupSubjectId As String = ""

        Dim StdName As String() = {"Mai", "Tong", "M", "Koch"}
        Dim subName As String() = {"Thai", "Eng", "Sci", "Math"}
        Dim scoData As Double() = {5, 5, 5, 5}


        dtSubName.Columns.Add("subId", GetType(String))
        dtSubName.Columns.Add("subName", GetType(String))
        dtSubName.Rows.Add("1", "มอม")
        dtSubName.Rows.Add("2", "ชนิดของคำ")
        dtSubName.Rows.Add("3", "กลอนดอกสร้อย")
        dtSubName.Rows.Add("4", "หัวใจนักรบ")

        dtStdName.Columns.Add("stdId", GetType(String))
        dtStdName.Columns.Add("stdName", GetType(String))
        dtStdName.Rows.Add("1", "ด.ญ. กณิกา รักเรียน")
        dtStdName.Rows.Add("2", "ด.ญ. กมลพรรณ เรียนดี")
        dtStdName.Rows.Add("3", "ด.ช. กมลภพ เก่งเรียน")
        dtStdName.Rows.Add("4", "ด.ช. กมลภู เล่าเรียน")


        dtScore.Columns.Add("stdId", GetType(String))
        dtScore.Columns.Add("subId", GetType(String))
        dtScore.Columns.Add("Quizscore", GetType(String))
        dtScore.Rows.Add("1", "1", "15")
        dtScore.Rows.Add("1", "2", "10")
        dtScore.Rows.Add("1", "3", "12")
        dtScore.Rows.Add("1", "4", "24")

        dtScore.Rows.Add("2", "1", "18")
        dtScore.Rows.Add("2", "2", "20")
        dtScore.Rows.Add("2", "3", "17")
        dtScore.Rows.Add("2", "4", "12")

        dtScore.Rows.Add("3", "1", "16")
        dtScore.Rows.Add("3", "2", "20")
        dtScore.Rows.Add("3", "3", "19")
        dtScore.Rows.Add("3", "4", "21")

        dtScore.Rows.Add("4", "1", "12")
        dtScore.Rows.Add("4", "2", "23")
        dtScore.Rows.Add("4", "3", "10")
        dtScore.Rows.Add("4", "4", "16")

        rptHeader.DataSource = dtSubName
        rptHeader.DataBind()

        'ต้องเขียน Linq ตรงนี้ดึงเฉพาะชื่อออกมาใช้ทำแถว

        Dim AllstdName = (From r In dtStdName Select r("stdName")).Distinct()
        rptName.DataSource = dtStdName
        rptName.DataBind()

        'Dim item As New Dictionary(Of String, Dictionary(Of String, String))

        'For j = 0 To StdName.Length - 1
        '    Dim ItemData As New Dictionary(Of String, String)
        '    For i = 0 To StdName.Length - 1
        '        Dim eachsco As Double = scoData(i)
        '        ItemData.Add(subName(i).ToString, (CDbl(i) + eachsco).ToString)
        '    Next
        '    item.Add(StdName(j), ItemData)
        'Next
    End Sub

    Protected Sub rptName_ItemCommand(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        Dim currentItem = DirectCast(e.Item.DataItem, System.Data.DataRowView).Row.ItemArray(0)
        Dim AllstdName As EnumerableRowCollection = From r In dtScore Where r("stdId") = currentItem Select r("Quizscore")

        Dim dtEachScore As DataTable = New DataTable
        dtEachScore.Columns.Add("Quizscore", GetType(String))

        For Each EachScr In AllstdName
            dtEachScore.Rows.Add(EachScr)
        Next


        'dtEachScore = AllstdName.Cast(Of DataRow).CopyToDataTable
        'เขียน Linq ดึงคะแนนของ e.Item.DataItem.Row.itemarray(1) ออกมา แล้ว bindRepeater Score
        Dim rptScore As Repeater = CType(e.Item.FindControl("rptScore"), Repeater)
        rptScore.DataSource = dtEachScore
        rptScore.DataBind()

    End Sub

End Class