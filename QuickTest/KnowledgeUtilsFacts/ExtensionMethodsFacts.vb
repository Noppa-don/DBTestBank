Imports Xunit
Imports KnowledgeUtils


Public Class ExtensionMethodsFacts

    <Fact>
    Public Sub NumberToScore()
        Assert.Equal("0".ToPointplusScore(), "0")
        Assert.Equal("10".ToPointplusScore(), "10")

        Assert.Equal("0.00".ToPointplusScore(), "0")

        Assert.Equal("10.00".ToPointplusScore(), "10")
        Assert.Equal("10.01".ToPointplusScore(), "10.01")
        Assert.Equal("10.10".ToPointplusScore(), "10.1")

        Assert.Equal("100.00".ToPointplusScore(), "100")
        Assert.Equal("100.01".ToPointplusScore(), "100.01")
        Assert.Equal("100.10".ToPointplusScore(), "100.1")

        Assert.Equal("110.00".ToPointplusScore(), "110")
        Assert.Equal("110.01".ToPointplusScore(), "110.01")
        Assert.Equal("110.10".ToPointplusScore(), "110.1")
    End Sub

    <Fact>
    Public Sub StringToScore()
        'Assert.Throws("score".ToScore(), Exception(""))
        'Dim ex As Exception = Assert.Throws(Of InvalidExpressionException,"score".ToScore())
        'Assert.
        'Assert.Equal("not use", ex.Message())

    End Sub

    <Fact>
    Public Sub NumbersToPointplusScore()

        Assert.Equal("0.00/20.00".ToPointplusScore(), "0/20")
        Assert.Equal("0.00 / 20.00".ToPointplusScore(), "0/20")

        Assert.Equal("1.00/20.00".ToPointplusScore(), "1/20")
        Assert.Equal("10.00/20.00".ToPointplusScore(), "10/20")

        Assert.Equal("10.10/20.00".ToPointplusScore(), "10.1/20")
        Assert.Equal("10.01/20.00".ToPointplusScore(), "10.01/20")
    End Sub

    <Fact>
    Public Sub NumbersToPointplusScoreWithSpace()

        Assert.Equal("0.00/20.00".ToPointplusScore(True), "0 / 20")
        Assert.Equal("0.00 / 20.00".ToPointplusScore(True), "0 / 20")
        Assert.Equal("0.00 /20.00".ToPointplusScore(True), "0 / 20")
        Assert.Equal("0.00/ 20.00".ToPointplusScore(True), "0 / 20")

    End Sub

    <Fact>
    Public Sub TimeToPointplusTimeAgo()

        Dim now As New DateTime(2015, 8, 27, 9, 0, 0)

        Dim time1 As New DateTime(2015, 8, 27, 8, 59, 57)
        Assert.Equal(time1.ToPointPlusTime(now), "เมื่อ 3 วินาทีที่แล้ว")

        Dim time2 As New DateTime(2015, 8, 27, 8, 47, 0)
        Assert.Equal(time2.ToPointPlusTime(now), "เมื่อ 13 นาทีที่แล้ว")

        Dim time3 As New DateTime(2015, 8, 27, 8, 46, 29) ' 13 นาที 29 วิ มั้ง
        Assert.Equal(time3.ToPointPlusTime(now), "เมื่อ 13 นาทีที่แล้ว")

        Dim time4 As New DateTime(2015, 8, 27, 8, 0, 29)
        Assert.Equal(time4.ToPointPlusTime(now), "เมื่อ 59 นาทีที่แล้ว")

        Dim time5 As New DateTime(2015, 8, 27, 8, 0, 0)
        Assert.Equal(time5.ToPointPlusTime(now), "27/08/2558 08:00")

        Dim time6 As New DateTime(2015, 8, 27, 7, 2, 29)
        Assert.Equal(time6.ToPointPlusTime(now), "27/08/2558 07:02")

    End Sub

    <Fact>
    Public Sub TimeToPointplusTimeCreepOn()

        Dim now As New DateTime(2015, 8, 27, 9, 0, 0)

        Dim time1 As New DateTime(2015, 8, 27, 9, 10, 57)
        Assert.Equal(time1.ToPointPlusTime(now), "27/08/2558 09:10")

        time1 = New DateTime(2015, 8, 29, 9, 30, 22)
        Assert.Equal(time1.ToPointPlusTime(now), "29/08/2558 09:30")

        time1 = New DateTime(2015, 8, 31, 10, 30, 37)
        Assert.Equal(time1.ToPointPlusTime(now), "31/08/2558 10:30")

    End Sub

End Class
