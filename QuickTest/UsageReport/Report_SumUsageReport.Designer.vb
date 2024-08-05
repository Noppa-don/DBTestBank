Partial Class Report_SumUsageReport
    
    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim Group1 As Telerik.Reporting.Group = New Telerik.Reporting.Group()
        Dim StyleRule1 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule2 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule3 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Dim StyleRule4 As Telerik.Reporting.Drawing.StyleRule = New Telerik.Reporting.Drawing.StyleRule()
        Me.labelsGroupFooterSection = New Telerik.Reporting.GroupFooterSection()
        Me.labelsGroupHeaderSection = New Telerik.Reporting.GroupHeaderSection()
        Me.userNameCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.totalLogInCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.totalSpentTimeCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.averageTimeCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.minTimeCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.maxTimeCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.zeroToFive_MinuteCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.fiveToTen_MinuteCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.tenToFifteen_MinuteCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.fifteenToThirty_MinuteCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.thirtyPlus_MinuteCaptionTextBox = New Telerik.Reporting.TextBox()
        Me.ObjectDataSource1 = New Telerik.Reporting.ObjectDataSource()
        Me.pageFooter = New Telerik.Reporting.PageFooterSection()
        Me.currentTimeTextBox = New Telerik.Reporting.TextBox()
        Me.pageInfoTextBox = New Telerik.Reporting.TextBox()
        Me.reportHeader = New Telerik.Reporting.ReportHeaderSection()
        Me.titleTextBox = New Telerik.Reporting.TextBox()
        Me.txtDetailReport = New Telerik.Reporting.HtmlTextBox()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.userNameDataTextBox = New Telerik.Reporting.TextBox()
        Me.totalLogInDataTextBox = New Telerik.Reporting.TextBox()
        Me.totalSpentTimeDataTextBox = New Telerik.Reporting.TextBox()
        Me.averageTimeDataTextBox = New Telerik.Reporting.TextBox()
        Me.minTimeDataTextBox = New Telerik.Reporting.TextBox()
        Me.maxTimeDataTextBox = New Telerik.Reporting.TextBox()
        Me.zeroToFive_MinuteDataTextBox = New Telerik.Reporting.TextBox()
        Me.fiveToTen_MinuteDataTextBox = New Telerik.Reporting.TextBox()
        Me.tenToFifteen_MinuteDataTextBox = New Telerik.Reporting.TextBox()
        Me.fifteenToThirty_MinuteDataTextBox = New Telerik.Reporting.TextBox()
        Me.thirtyPlus_MinuteDataTextBox = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'labelsGroupFooterSection
        '
        Me.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155R)
        Me.labelsGroupFooterSection.Name = "labelsGroupFooterSection"
        Me.labelsGroupFooterSection.Style.Visible = False
        '
        'labelsGroupHeaderSection
        '
        Me.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1999998092651367R)
        Me.labelsGroupHeaderSection.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.userNameCaptionTextBox, Me.totalLogInCaptionTextBox, Me.totalSpentTimeCaptionTextBox, Me.averageTimeCaptionTextBox, Me.minTimeCaptionTextBox, Me.maxTimeCaptionTextBox, Me.zeroToFive_MinuteCaptionTextBox, Me.fiveToTen_MinuteCaptionTextBox, Me.tenToFifteen_MinuteCaptionTextBox, Me.fifteenToThirty_MinuteCaptionTextBox, Me.thirtyPlus_MinuteCaptionTextBox})
        Me.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection"
        Me.labelsGroupHeaderSection.PrintOnEveryPage = True
        Me.labelsGroupHeaderSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid
        Me.labelsGroupHeaderSection.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.labelsGroupHeaderSection.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid
        '
        'userNameCaptionTextBox
        '
        Me.userNameCaptionTextBox.CanGrow = True
        Me.userNameCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.userNameCaptionTextBox.Name = "userNameCaptionTextBox"
        Me.userNameCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.userNameCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.userNameCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.userNameCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.userNameCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.userNameCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.userNameCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.userNameCaptionTextBox.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0.0R)
        Me.userNameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.userNameCaptionTextBox.StyleName = "Caption"
        Me.userNameCaptionTextBox.Value = "ชื่อ UserName"
        '
        'totalLogInCaptionTextBox
        '
        Me.totalLogInCaptionTextBox.CanGrow = True
        Me.totalLogInCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.8021211624145508R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.totalLogInCaptionTextBox.Name = "totalLogInCaptionTextBox"
        Me.totalLogInCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.totalLogInCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.totalLogInCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.totalLogInCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.totalLogInCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.totalLogInCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.totalLogInCaptionTextBox.StyleName = "Caption"
        Me.totalLogInCaptionTextBox.Value = "จำนวนครั้งเข้าระบบ"
        '
        'totalSpentTimeCaptionTextBox
        '
        Me.totalSpentTimeCaptionTextBox.CanGrow = True
        Me.totalSpentTimeCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.551325798034668R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.totalSpentTimeCaptionTextBox.Name = "totalSpentTimeCaptionTextBox"
        Me.totalSpentTimeCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.totalSpentTimeCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.totalSpentTimeCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.totalSpentTimeCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.totalSpentTimeCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.totalSpentTimeCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.totalSpentTimeCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.totalSpentTimeCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.totalSpentTimeCaptionTextBox.StyleName = "Caption"
        Me.totalSpentTimeCaptionTextBox.Value = "ใช้เวลารวม (ชม:นาที)"
        '
        'averageTimeCaptionTextBox
        '
        Me.averageTimeCaptionTextBox.CanGrow = True
        Me.averageTimeCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3005304336547852R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.averageTimeCaptionTextBox.Name = "averageTimeCaptionTextBox"
        Me.averageTimeCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.averageTimeCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.averageTimeCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.averageTimeCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.averageTimeCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.averageTimeCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11.0R)
        Me.averageTimeCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.averageTimeCaptionTextBox.StyleName = "Caption"
        Me.averageTimeCaptionTextBox.Value = "เฉลี่ยครั้งละ (นาที:วินาที)"
        '
        'minTimeCaptionTextBox
        '
        Me.minTimeCaptionTextBox.CanGrow = True
        Me.minTimeCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.0497350692749023R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.minTimeCaptionTextBox.Name = "minTimeCaptionTextBox"
        Me.minTimeCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.minTimeCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.minTimeCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.minTimeCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.minTimeCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.minTimeCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.minTimeCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.minTimeCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11.0R)
        Me.minTimeCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.minTimeCaptionTextBox.StyleName = "Caption"
        Me.minTimeCaptionTextBox.Value = "Min" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(นาที:วินาที)"
        '
        'maxTimeCaptionTextBox
        '
        Me.maxTimeCaptionTextBox.CanGrow = True
        Me.maxTimeCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.79893970489502R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.maxTimeCaptionTextBox.Name = "maxTimeCaptionTextBox"
        Me.maxTimeCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.maxTimeCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.maxTimeCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.maxTimeCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.maxTimeCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.maxTimeCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11.0R)
        Me.maxTimeCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.maxTimeCaptionTextBox.StyleName = "Caption"
        Me.maxTimeCaptionTextBox.Value = "Max" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(นาที:วินาที)"
        '
        'zeroToFive_MinuteCaptionTextBox
        '
        Me.zeroToFive_MinuteCaptionTextBox.CanGrow = True
        Me.zeroToFive_MinuteCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.548144340515137R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.zeroToFive_MinuteCaptionTextBox.Name = "zeroToFive_MinuteCaptionTextBox"
        Me.zeroToFive_MinuteCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.zeroToFive_MinuteCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.zeroToFive_MinuteCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.zeroToFive_MinuteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.zeroToFive_MinuteCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.zeroToFive_MinuteCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.zeroToFive_MinuteCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.zeroToFive_MinuteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.zeroToFive_MinuteCaptionTextBox.StyleName = "Caption"
        Me.zeroToFive_MinuteCaptionTextBox.Value = "0-5 นาที"
        '
        'fiveToTen_MinuteCaptionTextBox
        '
        Me.fiveToTen_MinuteCaptionTextBox.CanGrow = True
        Me.fiveToTen_MinuteCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.297348022460937R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.fiveToTen_MinuteCaptionTextBox.Name = "fiveToTen_MinuteCaptionTextBox"
        Me.fiveToTen_MinuteCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.fiveToTen_MinuteCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.fiveToTen_MinuteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.fiveToTen_MinuteCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.fiveToTen_MinuteCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.fiveToTen_MinuteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.fiveToTen_MinuteCaptionTextBox.StyleName = "Caption"
        Me.fiveToTen_MinuteCaptionTextBox.Value = "5-10 นาที"
        '
        'tenToFifteen_MinuteCaptionTextBox
        '
        Me.tenToFifteen_MinuteCaptionTextBox.CanGrow = True
        Me.tenToFifteen_MinuteCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.046552658081055R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.tenToFifteen_MinuteCaptionTextBox.Name = "tenToFifteen_MinuteCaptionTextBox"
        Me.tenToFifteen_MinuteCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.tenToFifteen_MinuteCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.tenToFifteen_MinuteCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.tenToFifteen_MinuteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.tenToFifteen_MinuteCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.tenToFifteen_MinuteCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.tenToFifteen_MinuteCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.tenToFifteen_MinuteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.tenToFifteen_MinuteCaptionTextBox.StyleName = "Caption"
        Me.tenToFifteen_MinuteCaptionTextBox.Value = "10-15 นาที"
        '
        'fifteenToThirty_MinuteCaptionTextBox
        '
        Me.fifteenToThirty_MinuteCaptionTextBox.CanGrow = True
        Me.fifteenToThirty_MinuteCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.795757293701172R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.fifteenToThirty_MinuteCaptionTextBox.Name = "fifteenToThirty_MinuteCaptionTextBox"
        Me.fifteenToThirty_MinuteCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.fifteenToThirty_MinuteCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.fifteenToThirty_MinuteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.fifteenToThirty_MinuteCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.fifteenToThirty_MinuteCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.fifteenToThirty_MinuteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.fifteenToThirty_MinuteCaptionTextBox.StyleName = "Caption"
        Me.fifteenToThirty_MinuteCaptionTextBox.Value = "15-30 นาที"
        '
        'thirtyPlus_MinuteCaptionTextBox
        '
        Me.thirtyPlus_MinuteCaptionTextBox.CanGrow = True
        Me.thirtyPlus_MinuteCaptionTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.544961929321289R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.thirtyPlus_MinuteCaptionTextBox.Name = "thirtyPlus_MinuteCaptionTextBox"
        Me.thirtyPlus_MinuteCaptionTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.thirtyPlus_MinuteCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.thirtyPlus_MinuteCaptionTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None
        Me.thirtyPlus_MinuteCaptionTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.thirtyPlus_MinuteCaptionTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None
        Me.thirtyPlus_MinuteCaptionTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.thirtyPlus_MinuteCaptionTextBox.Style.Color = System.Drawing.Color.Black
        Me.thirtyPlus_MinuteCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.thirtyPlus_MinuteCaptionTextBox.StyleName = "Caption"
        Me.thirtyPlus_MinuteCaptionTextBox.Value = "30+ นาที"
        '
        'ObjectDataSource1
        '
        Me.ObjectDataSource1.DataSource = GetType(QuickTest.ClsSumUsageReport)
        Me.ObjectDataSource1.Name = "ObjectDataSource1"
        '
        'pageFooter
        '
        Me.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567R)
        Me.pageFooter.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.currentTimeTextBox, Me.pageInfoTextBox})
        Me.pageFooter.Name = "pageFooter"
        '
        'currentTimeTextBox
        '
        Me.currentTimeTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.currentTimeTextBox.Name = "currentTimeTextBox"
        Me.currentTimeTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.5677080154418945R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.currentTimeTextBox.StyleName = "PageInfo"
        Me.currentTimeTextBox.Value = "=NOW()"
        '
        'pageInfoTextBox
        '
        Me.pageInfoTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.6735420227050781R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.pageInfoTextBox.Name = "pageInfoTextBox"
        Me.pageInfoTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.5677080154418945R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right
        Me.pageInfoTextBox.StyleName = "PageInfo"
        Me.pageInfoTextBox.Value = "=PageNumber"
        '
        'reportHeader
        '
        Me.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5R)
        Me.reportHeader.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.titleTextBox, Me.txtDetailReport})
        Me.reportHeader.Name = "reportHeader"
        '
        'titleTextBox
        '
        Me.titleTextBox.Angle = 0.0R
        Me.titleTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(0.0R))
        Me.titleTextBox.Name = "titleTextBox"
        Me.titleTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.241250991821289R), Telerik.Reporting.Drawing.Unit.Cm(1.3998004198074341R))
        Me.titleTextBox.StyleName = "Title"
        Me.titleTextBox.Value = ""
        '
        'txtDetailReport
        '
        Me.txtDetailReport.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R), Telerik.Reporting.Drawing.Unit.Cm(1.5R))
        Me.txtDetailReport.Name = "txtDetailReport"
        Me.txtDetailReport.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.547080993652344R), Telerik.Reporting.Drawing.Unit.Cm(1.0000001192092896R))
        Me.txtDetailReport.Style.Font.Name = "Tahoma"
        Me.txtDetailReport.Value = ""
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.userNameDataTextBox, Me.totalLogInDataTextBox, Me.totalSpentTimeDataTextBox, Me.averageTimeDataTextBox, Me.minTimeDataTextBox, Me.maxTimeDataTextBox, Me.zeroToFive_MinuteDataTextBox, Me.fiveToTen_MinuteDataTextBox, Me.tenToFifteen_MinuteDataTextBox, Me.fifteenToThirty_MinuteDataTextBox, Me.thirtyPlus_MinuteDataTextBox})
        Me.detail.Name = "detail"
        '
        'userNameDataTextBox
        '
        Me.userNameDataTextBox.CanGrow = True
        Me.userNameDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.userNameDataTextBox.Name = "userNameDataTextBox"
        Me.userNameDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.userNameDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.userNameDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.userNameDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.userNameDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.userNameDataTextBox.StyleName = "Data"
        Me.userNameDataTextBox.Value = "=Fields.UserName"
        '
        'totalLogInDataTextBox
        '
        Me.totalLogInDataTextBox.CanGrow = True
        Me.totalLogInDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.8021211624145508R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.totalLogInDataTextBox.Name = "totalLogInDataTextBox"
        Me.totalLogInDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.totalLogInDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.totalLogInDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.totalLogInDataTextBox.StyleName = "Data"
        Me.totalLogInDataTextBox.Value = "=Fields.TotalLogIn"
        '
        'totalSpentTimeDataTextBox
        '
        Me.totalSpentTimeDataTextBox.CanGrow = True
        Me.totalSpentTimeDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.551325798034668R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.totalSpentTimeDataTextBox.Name = "totalSpentTimeDataTextBox"
        Me.totalSpentTimeDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.totalSpentTimeDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.totalSpentTimeDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.totalSpentTimeDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.totalSpentTimeDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.totalSpentTimeDataTextBox.StyleName = "Data"
        Me.totalSpentTimeDataTextBox.Value = "=Fields.TotalSpentTime"
        '
        'averageTimeDataTextBox
        '
        Me.averageTimeDataTextBox.CanGrow = True
        Me.averageTimeDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3005304336547852R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.averageTimeDataTextBox.Name = "averageTimeDataTextBox"
        Me.averageTimeDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.averageTimeDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.averageTimeDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.averageTimeDataTextBox.StyleName = "Data"
        Me.averageTimeDataTextBox.Value = "=Fields.AverageTime"
        '
        'minTimeDataTextBox
        '
        Me.minTimeDataTextBox.CanGrow = True
        Me.minTimeDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.0497350692749023R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.minTimeDataTextBox.Name = "minTimeDataTextBox"
        Me.minTimeDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.minTimeDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.minTimeDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.minTimeDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.minTimeDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.minTimeDataTextBox.StyleName = "Data"
        Me.minTimeDataTextBox.Value = "=Fields.MinTime"
        '
        'maxTimeDataTextBox
        '
        Me.maxTimeDataTextBox.CanGrow = True
        Me.maxTimeDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.79893970489502R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.maxTimeDataTextBox.Name = "maxTimeDataTextBox"
        Me.maxTimeDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.maxTimeDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.maxTimeDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.maxTimeDataTextBox.StyleName = "Data"
        Me.maxTimeDataTextBox.Value = "=Fields.MaxTime"
        '
        'zeroToFive_MinuteDataTextBox
        '
        Me.zeroToFive_MinuteDataTextBox.CanGrow = True
        Me.zeroToFive_MinuteDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.548144340515137R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.zeroToFive_MinuteDataTextBox.Name = "zeroToFive_MinuteDataTextBox"
        Me.zeroToFive_MinuteDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.zeroToFive_MinuteDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.zeroToFive_MinuteDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.zeroToFive_MinuteDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.zeroToFive_MinuteDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.zeroToFive_MinuteDataTextBox.StyleName = "Data"
        Me.zeroToFive_MinuteDataTextBox.Value = "=Fields.ZeroToFive_Minute"
        '
        'fiveToTen_MinuteDataTextBox
        '
        Me.fiveToTen_MinuteDataTextBox.CanGrow = True
        Me.fiveToTen_MinuteDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.297348022460937R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.fiveToTen_MinuteDataTextBox.Name = "fiveToTen_MinuteDataTextBox"
        Me.fiveToTen_MinuteDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.fiveToTen_MinuteDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.fiveToTen_MinuteDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.fiveToTen_MinuteDataTextBox.StyleName = "Data"
        Me.fiveToTen_MinuteDataTextBox.Value = "=Fields.FiveToTen_Minute"
        '
        'tenToFifteen_MinuteDataTextBox
        '
        Me.tenToFifteen_MinuteDataTextBox.CanGrow = True
        Me.tenToFifteen_MinuteDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.046552658081055R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.tenToFifteen_MinuteDataTextBox.Name = "tenToFifteen_MinuteDataTextBox"
        Me.tenToFifteen_MinuteDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.tenToFifteen_MinuteDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.tenToFifteen_MinuteDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.tenToFifteen_MinuteDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.tenToFifteen_MinuteDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.tenToFifteen_MinuteDataTextBox.StyleName = "Data"
        Me.tenToFifteen_MinuteDataTextBox.Value = "=Fields.TenToFifteen_Minute"
        '
        'fifteenToThirty_MinuteDataTextBox
        '
        Me.fifteenToThirty_MinuteDataTextBox.CanGrow = True
        Me.fifteenToThirty_MinuteDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.795757293701172R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.fifteenToThirty_MinuteDataTextBox.Name = "fifteenToThirty_MinuteDataTextBox"
        Me.fifteenToThirty_MinuteDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.fifteenToThirty_MinuteDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.fifteenToThirty_MinuteDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.fifteenToThirty_MinuteDataTextBox.StyleName = "Data"
        Me.fifteenToThirty_MinuteDataTextBox.Value = "=Fields.FifteenToThirty_Minute"
        '
        'thirtyPlus_MinuteDataTextBox
        '
        Me.thirtyPlus_MinuteDataTextBox.CanGrow = True
        Me.thirtyPlus_MinuteDataTextBox.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.544961929321289R), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637R))
        Me.thirtyPlus_MinuteDataTextBox.Name = "thirtyPlus_MinuteDataTextBox"
        Me.thirtyPlus_MinuteDataTextBox.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6962878704071045R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.thirtyPlus_MinuteDataTextBox.Style.BackgroundColor = System.Drawing.Color.White
        Me.thirtyPlus_MinuteDataTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Cm(1.0R)
        Me.thirtyPlus_MinuteDataTextBox.Style.Color = System.Drawing.Color.Black
        Me.thirtyPlus_MinuteDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.thirtyPlus_MinuteDataTextBox.StyleName = "Data"
        Me.thirtyPlus_MinuteDataTextBox.Value = "=Fields.ThirtyPlus_Minute"
        '
        'Report_SumUsageReport
        '
        Me.DataSource = Me.ObjectDataSource1
        Group1.GroupFooter = Me.labelsGroupFooterSection
        Group1.GroupHeader = Me.labelsGroupHeaderSection
        Group1.Name = "labelsGroup"
        Me.Groups.AddRange(New Telerik.Reporting.Group() {Group1})
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.labelsGroupHeaderSection, Me.labelsGroupFooterSection, Me.pageFooter, Me.reportHeader, Me.detail})
        Me.Name = "Report_SumUsageReport"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.5R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.Style.BackgroundColor = System.Drawing.Color.White
        StyleRule1.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Title")})
        StyleRule1.Style.Color = System.Drawing.Color.Black
        StyleRule1.Style.Font.Bold = True
        StyleRule1.Style.Font.Italic = False
        StyleRule1.Style.Font.Name = "Tahoma"
        StyleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18.0R)
        StyleRule1.Style.Font.Strikeout = False
        StyleRule1.Style.Font.Underline = False
        StyleRule2.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Caption")})
        StyleRule2.Style.Color = System.Drawing.Color.Black
        StyleRule2.Style.Font.Name = "Tahoma"
        StyleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10.0R)
        StyleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule3.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("Data")})
        StyleRule3.Style.Font.Name = "Tahoma"
        StyleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9.0R)
        StyleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        StyleRule4.Selectors.AddRange(New Telerik.Reporting.Drawing.ISelector() {New Telerik.Reporting.Drawing.StyleSelector("PageInfo")})
        StyleRule4.Style.Font.Name = "Tahoma"
        StyleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.0R)
        StyleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.StyleSheet.AddRange(New Telerik.Reporting.Drawing.StyleRule() {StyleRule1, StyleRule2, StyleRule3, StyleRule4})
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(19.241249084472656R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents ObjectDataSource1 As Telerik.Reporting.ObjectDataSource
    Friend WithEvents labelsGroupHeaderSection As Telerik.Reporting.GroupHeaderSection
    Friend WithEvents userNameCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents totalLogInCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents totalSpentTimeCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents averageTimeCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents minTimeCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents maxTimeCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents zeroToFive_MinuteCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents fiveToTen_MinuteCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents tenToFifteen_MinuteCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents fifteenToThirty_MinuteCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents thirtyPlus_MinuteCaptionTextBox As Telerik.Reporting.TextBox
    Friend WithEvents labelsGroupFooterSection As Telerik.Reporting.GroupFooterSection
    Friend WithEvents pageFooter As Telerik.Reporting.PageFooterSection
    Friend WithEvents currentTimeTextBox As Telerik.Reporting.TextBox
    Friend WithEvents pageInfoTextBox As Telerik.Reporting.TextBox
    Friend WithEvents reportHeader As Telerik.Reporting.ReportHeaderSection
    Friend WithEvents titleTextBox As Telerik.Reporting.TextBox
    Friend WithEvents detail As Telerik.Reporting.DetailSection
    Friend WithEvents userNameDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents totalLogInDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents totalSpentTimeDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents averageTimeDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents minTimeDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents maxTimeDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents zeroToFive_MinuteDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents fiveToTen_MinuteDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents tenToFifteen_MinuteDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents fifteenToThirty_MinuteDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents thirtyPlus_MinuteDataTextBox As Telerik.Reporting.TextBox
    Friend WithEvents txtDetailReport As Telerik.Reporting.HtmlTextBox
End Class