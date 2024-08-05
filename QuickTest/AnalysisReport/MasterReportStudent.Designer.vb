Partial Class MasterReportStudent
    
    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim ChartMargins7 As Telerik.Reporting.Charting.Styles.ChartMargins = New Telerik.Reporting.Charting.Styles.ChartMargins()
        Dim ChartMargins8 As Telerik.Reporting.Charting.Styles.ChartMargins = New Telerik.Reporting.Charting.Styles.ChartMargins()
        Dim ChartMargins9 As Telerik.Reporting.Charting.Styles.ChartMargins = New Telerik.Reporting.Charting.Styles.ChartMargins()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MasterReportStudent))
        Me.pageHeaderSection1 = New Telerik.Reporting.PageHeaderSection()
        Me.TextBox1 = New Telerik.Reporting.TextBox()
        Me.txtDurationDateHeader = New Telerik.Reporting.TextBox()
        Me.Shape1 = New Telerik.Reporting.Shape()
        Me.TextBox7 = New Telerik.Reporting.TextBox()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.ChartTotalUsage = New Telerik.Reporting.Chart()
        Me.TextBox3 = New Telerik.Reporting.TextBox()
        Me.ChartTotalPracticeBySubject = New Telerik.Reporting.Chart()
        Me.TextBox4 = New Telerik.Reporting.TextBox()
        Me.ChartSendHomework = New Telerik.Reporting.Chart()
        Me.TextBox5 = New Telerik.Reporting.TextBox()
        Me.Shape2 = New Telerik.Reporting.Shape()
        Me.txtStudentName = New Telerik.Reporting.TextBox()
        Me.StudentPic = New Telerik.Reporting.PictureBox()
        Me.txtStudentSchoolInfo = New Telerik.Reporting.TextBox()
        Me.txtStudentParentInfo = New Telerik.Reporting.TextBox()
        Me.txtTotalUsage = New Telerik.Reporting.TextBox()
        Me.txtQuizResult = New Telerik.Reporting.TextBox()
        Me.txtPracticeQuantity = New Telerik.Reporting.TextBox()
        Me.txtSendHomework = New Telerik.Reporting.TextBox()
        Me.TextBox2 = New Telerik.Reporting.TextBox()
        Me.txtIndexPage = New Telerik.Reporting.TextBox()
        Me.PictureQuizChart = New Telerik.Reporting.PictureBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'pageHeaderSection1
        '
        Me.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684R)
        Me.pageHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.TextBox1, Me.txtDurationDateHeader, Me.Shape1, Me.TextBox7})
        Me.pageHeaderSection1.Name = "pageHeaderSection1"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.59999805688858032R), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941R))
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.799999237060547R), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045R))
        Me.TextBox1.Style.Font.Bold = True
        Me.TextBox1.Style.Font.Name = "Angsana New"
        Me.TextBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(30.0R)
        Me.TextBox1.Style.Font.Underline = True
        Me.TextBox1.Value = "บทวิเคราะห์/พัฒนาการ/แนะนำจุดแข็งจุดอ่อน"
        '
        'txtDurationDateHeader
        '
        Me.txtDurationDateHeader.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269R), Telerik.Reporting.Drawing.Unit.Cm(1.1001999378204346R))
        Me.txtDurationDateHeader.Name = "txtDurationDateHeader"
        Me.txtDurationDateHeader.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.799999237060547R), Telerik.Reporting.Drawing.Unit.Cm(0.89999955892562866R))
        Me.txtDurationDateHeader.Style.Font.Name = "Angsana New"
        Me.txtDurationDateHeader.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(25.0R)
        Me.txtDurationDateHeader.Value = "ข้อมูลช่วง 1 - 31/8/56"
        '
        'Shape1
        '
        Me.Shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R), Telerik.Reporting.Drawing.Unit.Cm(2.0R))
        Me.Shape1.Name = "Shape1"
        Me.Shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.099899291992188R), Telerik.Reporting.Drawing.Unit.Cm(0.49960047006607056R))
        '
        'TextBox7
        '
        Me.TextBox7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.5R), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269R))
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5999982357025146R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R))
        Me.TextBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox7.Style.Font.Name = "Angsana New"
        Me.TextBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.TextBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.TextBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.TextBox7.Value = "TextBox7"
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(24.5R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.ChartTotalUsage, Me.TextBox3, Me.ChartTotalPracticeBySubject, Me.TextBox4, Me.ChartSendHomework, Me.TextBox5, Me.Shape2, Me.txtStudentName, Me.StudentPic, Me.txtStudentSchoolInfo, Me.txtStudentParentInfo, Me.txtTotalUsage, Me.txtQuizResult, Me.txtPracticeQuantity, Me.txtSendHomework, Me.TextBox2, Me.txtIndexPage, Me.PictureQuizChart})
        Me.detail.Name = "detail"
        '
        'ChartTotalUsage
        '
        Me.ChartTotalUsage.Appearance.Border.Color = System.Drawing.Color.White
        Me.ChartTotalUsage.BitmapResolution = 96.0!
        Me.ChartTotalUsage.ChartTitle.Appearance.Visible = False
        Me.ChartTotalUsage.ChartTitle.Visible = False
        Me.ChartTotalUsage.ImageFormat = System.Drawing.Imaging.ImageFormat.Emf
        Me.ChartTotalUsage.Legend.Appearance.Visible = False
        Me.ChartTotalUsage.Legend.Visible = False
        Me.ChartTotalUsage.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.59999805688858032R), Telerik.Reporting.Drawing.Unit.Cm(0.70019990205764771R))
        Me.ChartTotalUsage.Name = "ChartTotalUsage"
        Me.ChartTotalUsage.PlotArea.Appearance.Border.Color = System.Drawing.Color.White
        ChartMargins7.Bottom = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins7.Left = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins7.Right = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins7.Top = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        Me.ChartTotalUsage.PlotArea.Appearance.Dimensions.Margins = ChartMargins7
        Me.ChartTotalUsage.PlotArea.Appearance.FillStyle.FillType = Telerik.Reporting.Charting.Styles.FillType.Solid
        Me.ChartTotalUsage.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.Color.White
        Me.ChartTotalUsage.PlotArea.EmptySeriesMessage.Appearance.Visible = True
        Me.ChartTotalUsage.PlotArea.EmptySeriesMessage.Visible = True
        Me.ChartTotalUsage.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3999004364013672R), Telerik.Reporting.Drawing.Unit.Cm(3.299999475479126R))
        '
        'TextBox3
        '
        Me.TextBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.2999999523162842R), Telerik.Reporting.Drawing.Unit.Cm(4.2917594909667969R))
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7999999523162842R), Telerik.Reporting.Drawing.Unit.Cm(0.70000046491622925R))
        Me.TextBox3.Style.Font.Bold = True
        Me.TextBox3.Style.Font.Name = "Angsana New"
        Me.TextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.TextBox3.Style.Font.Underline = False
        Me.TextBox3.Value = "ผลการควิซ"
        '
        'ChartTotalPracticeBySubject
        '
        Me.ChartTotalPracticeBySubject.Appearance.Border.Color = System.Drawing.Color.White
        Me.ChartTotalPracticeBySubject.BitmapResolution = 96.0!
        Me.ChartTotalPracticeBySubject.ChartTitle.Appearance.Visible = False
        Me.ChartTotalPracticeBySubject.ChartTitle.Visible = False
        Me.ChartTotalPracticeBySubject.ImageFormat = System.Drawing.Imaging.ImageFormat.Emf
        Me.ChartTotalPracticeBySubject.Legend.Appearance.Visible = False
        Me.ChartTotalPracticeBySubject.Legend.Visible = False
        Me.ChartTotalPracticeBySubject.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.59999805688858032R), Telerik.Reporting.Drawing.Unit.Cm(9.7333335876464844R))
        Me.ChartTotalPracticeBySubject.Name = "ChartTotalPracticeBySubject"
        Me.ChartTotalPracticeBySubject.PlotArea.Appearance.Border.Color = System.Drawing.Color.White
        ChartMargins8.Bottom = New Telerik.Reporting.Charting.Styles.Unit(30.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins8.Left = New Telerik.Reporting.Charting.Styles.Unit(0.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins8.Right = New Telerik.Reporting.Charting.Styles.Unit(0.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins8.Top = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        Me.ChartTotalPracticeBySubject.PlotArea.Appearance.Dimensions.Margins = ChartMargins8
        Me.ChartTotalPracticeBySubject.PlotArea.Appearance.FillStyle.FillType = Telerik.Reporting.Charting.Styles.FillType.Solid
        Me.ChartTotalPracticeBySubject.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.Color.White
        Me.ChartTotalPracticeBySubject.PlotArea.EmptySeriesMessage.Appearance.Visible = True
        Me.ChartTotalPracticeBySubject.PlotArea.EmptySeriesMessage.Visible = True
        Me.ChartTotalPracticeBySubject.PlotArea.XAxis.Appearance.MajorGridLines.Visible = False
        Me.ChartTotalPracticeBySubject.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.399899959564209R), Telerik.Reporting.Drawing.Unit.Cm(4.6076388359069824R))
        '
        'TextBox4
        '
        Me.TextBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5R), Telerik.Reporting.Drawing.Unit.Cm(9.0331325531005859R))
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5000016689300537R), Telerik.Reporting.Drawing.Unit.Cm(0.70000046491622925R))
        Me.TextBox4.Style.Font.Bold = True
        Me.TextBox4.Style.Font.Name = "Angsana New"
        Me.TextBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.TextBox4.Style.Font.Underline = False
        Me.TextBox4.Value = "ปริมาณการฝึกฝนตัวเอง"
        '
        'ChartSendHomework
        '
        Me.ChartSendHomework.Appearance.Border.Color = System.Drawing.Color.White
        Me.ChartSendHomework.BitmapResolution = 96.0!
        Me.ChartSendHomework.ChartTitle.Appearance.Visible = False
        Me.ChartSendHomework.ChartTitle.Visible = False
        Me.ChartSendHomework.ImageFormat = System.Drawing.Imaging.ImageFormat.Emf
        Me.ChartSendHomework.Legend.Appearance.Border.Color = System.Drawing.Color.White
        Me.ChartSendHomework.Legend.Appearance.Visible = False
        Me.ChartSendHomework.Legend.Visible = False
        Me.ChartSendHomework.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60009980201721191R), Telerik.Reporting.Drawing.Unit.Cm(15.200000762939453R))
        Me.ChartSendHomework.Name = "ChartSendHomework"
        Me.ChartSendHomework.PlotArea.Appearance.Border.Color = System.Drawing.Color.White
        ChartMargins9.Bottom = New Telerik.Reporting.Charting.Styles.Unit(30.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins9.Left = New Telerik.Reporting.Charting.Styles.Unit(0.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins9.Right = New Telerik.Reporting.Charting.Styles.Unit(0.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        ChartMargins9.Top = New Telerik.Reporting.Charting.Styles.Unit(10.0R, Telerik.Reporting.Charting.Styles.UnitType.Pixel)
        Me.ChartSendHomework.PlotArea.Appearance.Dimensions.Margins = ChartMargins9
        Me.ChartSendHomework.PlotArea.Appearance.FillStyle.FillType = Telerik.Reporting.Charting.Styles.FillType.Solid
        Me.ChartSendHomework.PlotArea.Appearance.FillStyle.MainColor = System.Drawing.Color.White
        Me.ChartSendHomework.PlotArea.EmptySeriesMessage.Appearance.Visible = True
        Me.ChartSendHomework.PlotArea.EmptySeriesMessage.Visible = True
        Me.ChartSendHomework.PlotArea.XAxis.Appearance.MajorGridLines.Color = System.Drawing.Color.White
        Me.ChartSendHomework.PlotArea.XAxis.Appearance.MajorGridLines.Visible = False
        Me.ChartSendHomework.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.399899959564209R), Telerik.Reporting.Drawing.Unit.Cm(4.6076388359069824R))
        Me.ChartSendHomework.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.ChartSendHomework.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.ChartSendHomework.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        Me.ChartSendHomework.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(3.0R)
        '
        'TextBox5
        '
        Me.TextBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.2000000476837158R), Telerik.Reporting.Drawing.Unit.Cm(14.499799728393555R))
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0999999046325684R), Telerik.Reporting.Drawing.Unit.Cm(0.70000046491622925R))
        Me.TextBox5.Style.Font.Bold = True
        Me.TextBox5.Style.Font.Name = "Angsana New"
        Me.TextBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.TextBox5.Style.Font.Underline = False
        Me.TextBox5.Value = "การส่งการบ้าน"
        '
        'Shape2
        '
        Me.Shape2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R), Telerik.Reporting.Drawing.Unit.Cm(20.100000381469727R))
        Me.Shape2.Name = "Shape2"
        Me.Shape2.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.699899673461914R), Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209R))
        '
        'txtStudentName
        '
        Me.txtStudentName.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60009980201721191R), Telerik.Reporting.Drawing.Unit.Cm(21.299999237060547R))
        Me.txtStudentName.Name = "txtStudentName"
        Me.txtStudentName.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.5R), Telerik.Reporting.Drawing.Unit.Cm(1.0000002384185791R))
        Me.txtStudentName.Style.Font.Bold = True
        Me.txtStudentName.Style.Font.Name = "Angsana New"
        Me.txtStudentName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(30.0R)
        Me.txtStudentName.Value = ""
        '
        'StudentPic
        '
        Me.StudentPic.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.40000057220459R), Telerik.Reporting.Drawing.Unit.Cm(20.100000381469727R))
        Me.StudentPic.Name = "StudentPic"
        Me.StudentPic.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6999969482421875R), Telerik.Reporting.Drawing.Unit.Cm(3.3000020980834961R))
        Me.StudentPic.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        '
        'txtStudentSchoolInfo
        '
        Me.txtStudentSchoolInfo.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60009980201721191R), Telerik.Reporting.Drawing.Unit.Cm(22.299999237060547R))
        Me.txtStudentSchoolInfo.Name = "txtStudentSchoolInfo"
        Me.txtStudentSchoolInfo.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0996999740600586R), Telerik.Reporting.Drawing.Unit.Cm(0.79999959468841553R))
        Me.txtStudentSchoolInfo.Style.Font.Name = "Angsana New"
        Me.txtStudentSchoolInfo.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(25.0R)
        Me.txtStudentSchoolInfo.Value = ""
        '
        'txtStudentParentInfo
        '
        Me.txtStudentParentInfo.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.6999998092651367R), Telerik.Reporting.Drawing.Unit.Cm(22.299999237060547R))
        Me.txtStudentParentInfo.Name = "txtStudentParentInfo"
        Me.txtStudentParentInfo.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7000012397766113R), Telerik.Reporting.Drawing.Unit.Cm(0.79999804496765137R))
        Me.txtStudentParentInfo.Style.Font.Name = "Angsana New"
        Me.txtStudentParentInfo.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(25.0R)
        Me.txtStudentParentInfo.Value = ""
        '
        'txtTotalUsage
        '
        Me.txtTotalUsage.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.0999975204467773R), Telerik.Reporting.Drawing.Unit.Cm(0.70019990205764771R))
        Me.txtTotalUsage.Name = "txtTotalUsage"
        Me.txtTotalUsage.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(3.2999999523162842R))
        Me.txtTotalUsage.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.txtTotalUsage.Style.Font.Name = "Angsana New"
        Me.txtTotalUsage.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalUsage.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtTotalUsage.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtTotalUsage.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtTotalUsage.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtTotalUsage.Value = "อัตราส่วนการใช้งานของนักเรียนมีความสมดุลดี โดยมีการใช้เวลาทำการบ้านกับการฝึกฝนด้ว" & _
    "ยตนเองนอกเวลาเรียนค่อนข้างใกล้เคียงกัน"
        '
        'txtQuizResult
        '
        Me.txtQuizResult.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.0999975204467773R), Telerik.Reporting.Drawing.Unit.Cm(4.9919595718383789R))
        Me.txtQuizResult.Name = "txtQuizResult"
        Me.txtQuizResult.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.000002861022949R), Telerik.Reporting.Drawing.Unit.Cm(3.8080399036407471R))
        Me.txtQuizResult.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.txtQuizResult.Style.Font.Name = "Angsana New"
        Me.txtQuizResult.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtQuizResult.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtQuizResult.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtQuizResult.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtQuizResult.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtQuizResult.Value = resources.GetString("txtQuizResult.Value")
        '
        'txtPracticeQuantity
        '
        Me.txtPracticeQuantity.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.0999975204467773R), Telerik.Reporting.Drawing.Unit.Cm(9.7333335876464844R))
        Me.txtPracticeQuantity.Name = "txtPracticeQuantity"
        Me.txtPracticeQuantity.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.000000953674316R), Telerik.Reporting.Drawing.Unit.Cm(4.6076388359069824R))
        Me.txtPracticeQuantity.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.txtPracticeQuantity.Style.Font.Name = "Angsana New"
        Me.txtPracticeQuantity.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtPracticeQuantity.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtPracticeQuantity.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtPracticeQuantity.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtPracticeQuantity.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtPracticeQuantity.Value = resources.GetString("txtPracticeQuantity.Value")
        '
        'txtSendHomework
        '
        Me.txtSendHomework.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.0999975204467773R), Telerik.Reporting.Drawing.Unit.Cm(15.200000762939453R))
        Me.txtSendHomework.Name = "txtSendHomework"
        Me.txtSendHomework.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.000001907348633R), Telerik.Reporting.Drawing.Unit.Cm(4.6076383590698242R))
        Me.txtSendHomework.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.txtSendHomework.Style.Font.Name = "Angsana New"
        Me.txtSendHomework.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtSendHomework.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtSendHomework.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtSendHomework.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtSendHomework.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.txtSendHomework.Value = "นักเรียนทำการบ้านส่งทุกชิ้น และทำส่งครบทุกข้อภายในกำหนดเวลาเกือบทั้งหมด มีเพียง 1" & _
    " ชิ้นเท่านั้นที่ทำไม่ครบทุกข้อ อาจลองค้นหาสาเหตุที่ทำให้ทำได้ไม่ครบทุกข้อและหาแน" & _
    "วทางแก้ไขไม่ให้เกิดขึ้นอีกในอนาคต"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.2999999523162842R), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R))
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6000009775161743R), Telerik.Reporting.Drawing.Unit.Cm(0.69989949464797974R))
        Me.TextBox2.Style.Font.Bold = True
        Me.TextBox2.Style.Font.Name = "Angsana New"
        Me.TextBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.TextBox2.Style.Font.Underline = False
        Me.TextBox2.Value = "การใช้งาน"
        '
        'txtIndexPage
        '
        Me.txtIndexPage.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(16.399900436401367R), Telerik.Reporting.Drawing.Unit.Cm(23.399997711181641R))
        Me.txtIndexPage.Name = "txtIndexPage"
        Me.txtIndexPage.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9999992847442627R), Telerik.Reporting.Drawing.Unit.Cm(1.0000003576278687R))
        Me.txtIndexPage.Style.Font.Name = "Angsana New"
        Me.txtIndexPage.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(23.0R)
        Me.txtIndexPage.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtIndexPage.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtIndexPage.Value = "TextBox6"
        '
        'PictureQuizChart
        '
        Me.PictureQuizChart.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60020154714584351R), Telerik.Reporting.Drawing.Unit.Cm(4.9919605255126953R))
        Me.PictureQuizChart.MimeType = ""
        Me.PictureQuizChart.Name = "PictureQuizChart"
        Me.PictureQuizChart.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3997983932495117R), Telerik.Reporting.Drawing.Unit.Cm(3.8080394268035889R))
        Me.PictureQuizChart.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureQuizChart.Value = ""
        '
        'MasterReportStudent
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.pageHeaderSection1, Me.detail})
        Me.Name = "MasterReportStudent"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.5R), Telerik.Reporting.Drawing.Unit.Cm(0.5R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.Style.BackgroundColor = System.Drawing.Color.White
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(19.400001525878906R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents pageHeaderSection1 As Telerik.Reporting.PageHeaderSection
    Friend WithEvents detail As Telerik.Reporting.DetailSection
    Friend WithEvents TextBox1 As Telerik.Reporting.TextBox
    Friend WithEvents txtDurationDateHeader As Telerik.Reporting.TextBox
    Friend WithEvents Shape1 As Telerik.Reporting.Shape
    Friend WithEvents TextBox2 As Telerik.Reporting.TextBox
    Friend WithEvents ChartTotalUsage As Telerik.Reporting.Chart
    Friend WithEvents txtQuizResult As Telerik.Reporting.TextBox
    Friend WithEvents TextBox3 As Telerik.Reporting.TextBox
    Friend WithEvents ChartTotalPracticeBySubject As Telerik.Reporting.Chart
    Friend WithEvents TextBox4 As Telerik.Reporting.TextBox
    Friend WithEvents txtPracticeQuantity As Telerik.Reporting.TextBox
    Friend WithEvents ChartSendHomework As Telerik.Reporting.Chart
    Friend WithEvents TextBox5 As Telerik.Reporting.TextBox
    Friend WithEvents txtSendHomework As Telerik.Reporting.TextBox
    Friend WithEvents Shape2 As Telerik.Reporting.Shape
    Friend WithEvents txtStudentName As Telerik.Reporting.TextBox
    Friend WithEvents StudentPic As Telerik.Reporting.PictureBox
    Friend WithEvents txtStudentSchoolInfo As Telerik.Reporting.TextBox
    Friend WithEvents txtStudentParentInfo As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalUsage As Telerik.Reporting.TextBox
    Friend WithEvents TextBox7 As Telerik.Reporting.TextBox
    Friend WithEvents txtIndexPage As Telerik.Reporting.TextBox
    Friend WithEvents PictureQuizChart As Telerik.Reporting.PictureBox
End Class