Partial Class MasterSchoolReport
    
    'NOTE: The following procedure is required by the telerik Reporting Designer
    'It can be modified using the telerik Reporting Designer.  
    'Do not modify it using the code editor.
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MasterSchoolReport))
        Me.pageHeaderSection1 = New Telerik.Reporting.PageHeaderSection()
        Me.txtAddress = New Telerik.Reporting.TextBox()
        Me.txtTelephone = New Telerik.Reporting.TextBox()
        Me.txtFax = New Telerik.Reporting.TextBox()
        Me.txtSchoolName = New Telerik.Reporting.HtmlTextBox()
        Me.Shape1 = New Telerik.Reporting.Shape()
        Me.detail = New Telerik.Reporting.DetailSection()
        Me.txtHeader = New Telerik.Reporting.TextBox()
        Me.PictureBox1 = New Telerik.Reporting.PictureBox()
        Me.txtTotalSchoolActive = New Telerik.Reporting.TextBox()
        Me.TextBox2 = New Telerik.Reporting.TextBox()
        Me.TextBox3 = New Telerik.Reporting.TextBox()
        Me.TextBox4 = New Telerik.Reporting.TextBox()
        Me.txtTotalStudentActive = New Telerik.Reporting.TextBox()
        Me.TextBox1 = New Telerik.Reporting.TextBox()
        Me.Shape5 = New Telerik.Reporting.Shape()
        Me.txtDay1 = New Telerik.Reporting.TextBox()
        Me.txtDay2 = New Telerik.Reporting.TextBox()
        Me.txtDay3 = New Telerik.Reporting.TextBox()
        Me.txtDay4 = New Telerik.Reporting.TextBox()
        Me.txtDay5 = New Telerik.Reporting.TextBox()
        Me.txtDay6 = New Telerik.Reporting.TextBox()
        Me.txtDay7 = New Telerik.Reporting.TextBox()
        Me.txtDay14 = New Telerik.Reporting.TextBox()
        Me.txtDay13 = New Telerik.Reporting.TextBox()
        Me.txtDay12 = New Telerik.Reporting.TextBox()
        Me.txtDay11 = New Telerik.Reporting.TextBox()
        Me.txtDay10 = New Telerik.Reporting.TextBox()
        Me.txtDay9 = New Telerik.Reporting.TextBox()
        Me.txtDay8 = New Telerik.Reporting.TextBox()
        Me.txtDay15 = New Telerik.Reporting.TextBox()
        Me.txtDay16 = New Telerik.Reporting.TextBox()
        Me.txtDay17 = New Telerik.Reporting.TextBox()
        Me.txtDay18 = New Telerik.Reporting.TextBox()
        Me.txtDay19 = New Telerik.Reporting.TextBox()
        Me.txtDay20 = New Telerik.Reporting.TextBox()
        Me.txtDay21 = New Telerik.Reporting.TextBox()
        Me.txtDay22 = New Telerik.Reporting.TextBox()
        Me.txtDay23 = New Telerik.Reporting.TextBox()
        Me.txtDay24 = New Telerik.Reporting.TextBox()
        Me.txtDay25 = New Telerik.Reporting.TextBox()
        Me.txtDay26 = New Telerik.Reporting.TextBox()
        Me.txtDay27 = New Telerik.Reporting.TextBox()
        Me.txtDay28 = New Telerik.Reporting.TextBox()
        Me.txtDay29 = New Telerik.Reporting.TextBox()
        Me.txtDay30 = New Telerik.Reporting.TextBox()
        Me.txtDay31 = New Telerik.Reporting.TextBox()
        Me.txtBottomDescription = New Telerik.Reporting.TextBox()
        Me.txtTotalDeviceDetail = New Telerik.Reporting.TextBox()
        Me.txtTotalDeviceDetail2 = New Telerik.Reporting.TextBox()
        Me.PictureBox4 = New Telerik.Reporting.PictureBox()
        Me.PictureBox5 = New Telerik.Reporting.PictureBox()
        Me.PictureBox2 = New Telerik.Reporting.PictureBox()
        Me.PictureBox3 = New Telerik.Reporting.PictureBox()
        Me.txtIndexPage = New Telerik.Reporting.TextBox()
        Me.txtTotalSchoolActive2 = New Telerik.Reporting.TextBox()
        Me.txtTotalStudentActive2 = New Telerik.Reporting.TextBox()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'pageHeaderSection1
        '
        Me.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(4.4589662551879883R)
        Me.pageHeaderSection1.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.txtAddress, Me.txtTelephone, Me.txtFax, Me.txtSchoolName, Me.Shape1})
        Me.pageHeaderSection1.Name = "pageHeaderSection1"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R), Telerik.Reporting.Drawing.Unit.Cm(1.4560415744781494R))
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.599900245666504R), Telerik.Reporting.Drawing.Unit.Cm(0.99979960918426514R))
        Me.txtAddress.Style.Font.Name = "Angsana New"
        Me.txtAddress.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(23.0R)
        Me.txtAddress.Value = "123/4 ถนนสุขุมวิท พระโขนง ตลองเตย กรุงเทพฯ10110"
        '
        'txtTelephone
        '
        Me.txtTelephone.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(2.4560415744781494R))
        Me.txtTelephone.Name = "txtTelephone"
        Me.txtTelephone.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.3745894432067871R), Telerik.Reporting.Drawing.Unit.Cm(0.94395840167999268R))
        Me.txtTelephone.Style.Font.Name = "Angsana New"
        Me.txtTelephone.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(23.0R)
        Me.txtTelephone.Value = "โทร : 02-1234567"
        '
        'txtFax
        '
        Me.txtFax.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.7554085254669189R), Telerik.Reporting.Drawing.Unit.Cm(2.4560415744781494R))
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643825531005859R), Telerik.Reporting.Drawing.Unit.Cm(0.9175000786781311R))
        Me.txtFax.Style.Font.Name = "Angsana New"
        Me.txtFax.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(23.0R)
        Me.txtFax.Value = "โทรสาร  : 02-1234567-1"
        '
        'txtSchoolName
        '
        Me.txtSchoolName.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R), Telerik.Reporting.Drawing.Unit.Cm(0.2560417652130127R))
        Me.txtSchoolName.Name = "txtSchoolName"
        Me.txtSchoolName.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(14.599900245666504R), Telerik.Reporting.Drawing.Unit.Cm(1.19979989528656R))
        Me.txtSchoolName.Style.Font.Bold = True
        Me.txtSchoolName.Style.Font.Name = "Angsana New"
        Me.txtSchoolName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(30.0R)
        Me.txtSchoolName.Value = "โรงเรียนอนุบาลหมีน้อย"
        '
        'Shape1
        '
        Me.Shape1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666R), Telerik.Reporting.Drawing.Unit.Cm(3.4002001285552979R))
        Me.Shape1.Name = "Shape1"
        Me.Shape1.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.399801254272461R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R))
        '
        'detail
        '
        Me.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(22.570796966552734R)
        Me.detail.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.txtHeader, Me.PictureBox1, Me.txtTotalSchoolActive, Me.TextBox2, Me.TextBox3, Me.TextBox4, Me.txtTotalStudentActive, Me.TextBox1, Me.Shape5, Me.txtDay1, Me.txtDay2, Me.txtDay3, Me.txtDay4, Me.txtDay5, Me.txtDay6, Me.txtDay7, Me.txtDay14, Me.txtDay13, Me.txtDay12, Me.txtDay11, Me.txtDay10, Me.txtDay9, Me.txtDay8, Me.txtDay15, Me.txtDay16, Me.txtDay17, Me.txtDay18, Me.txtDay19, Me.txtDay20, Me.txtDay21, Me.txtDay22, Me.txtDay23, Me.txtDay24, Me.txtDay25, Me.txtDay26, Me.txtDay27, Me.txtDay28, Me.txtDay29, Me.txtDay30, Me.txtDay31, Me.txtBottomDescription, Me.txtTotalDeviceDetail, Me.txtTotalDeviceDetail2, Me.PictureBox4, Me.PictureBox5, Me.txtIndexPage, Me.txtTotalSchoolActive2, Me.txtTotalStudentActive2})
        Me.detail.Name = "detail"
        '
        'txtHeader
        '
        Me.txtHeader.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.6489582061767578R), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317R))
        Me.txtHeader.Name = "txtHeader"
        Me.txtHeader.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.5997982025146484R), Telerik.Reporting.Drawing.Unit.Cm(1.100000262260437R))
        Me.txtHeader.Style.Font.Bold = True
        Me.txtHeader.Style.Font.Name = "Angsana New"
        Me.txtHeader.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(30.0R)
        Me.txtHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtHeader.Value = "บทวิเคราะห์ประจำเดือน สิงหาคม 2556"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(2.1000998020172119R))
        Me.PictureBox1.MimeType = "image/png"
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0000002384185791R), Telerik.Reporting.Drawing.Unit.Cm(1.9000002145767212R))
        Me.PictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureBox1.Value = CType(resources.GetObject("PictureBox1.Value"), Object)
        '
        'txtTotalSchoolActive
        '
        Me.txtTotalSchoolActive.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3999998569488525R), Telerik.Reporting.Drawing.Unit.Cm(2.1000998020172119R))
        Me.txtTotalSchoolActive.Name = "txtTotalSchoolActive"
        Me.txtTotalSchoolActive.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(1.0315397977828979R))
        Me.txtTotalSchoolActive.Style.Font.Name = "Angsana New"
        Me.txtTotalSchoolActive.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalSchoolActive.Value = ""
        '
        'TextBox2
        '
        Me.TextBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.1300029754638672R), Telerik.Reporting.Drawing.Unit.Cm(2.1000998020172119R))
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.8416709899902344R), Telerik.Reporting.Drawing.Unit.Cm(1.9000002145767212R))
        Me.TextBox2.Style.BorderColor.Default = System.Drawing.Color.White
        Me.TextBox2.Style.BorderColor.Left = System.Drawing.Color.Black
        Me.TextBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox2.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox2.Style.Color = System.Drawing.Color.Black
        Me.TextBox2.Style.Font.Name = "Angsana New"
        Me.TextBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(18.0R)
        Me.TextBox2.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(1.4500000476837158R)
        Me.TextBox2.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox2.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox2.Value = "ควรกระตุ้นให้เกิดการใช้งานมากขึ้น โดยอาจดูข้อมูลจาก Nobel Director ว่าหมวดฯใด ที่" & _
    "มีการใช้งานน้อย อาจลองสอบถามเหตุผลและโน้มน้าวให้เกิดการใช้งานให้มากขึ้น"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.1300029754638672R), Telerik.Reporting.Drawing.Unit.Cm(4.6853084564208984R))
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.8416709899902344R), Telerik.Reporting.Drawing.Unit.Cm(1.9000002145767212R))
        Me.TextBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox3.Style.Color = System.Drawing.Color.Black
        Me.TextBox3.Style.Font.Name = "Angsana New"
        Me.TextBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(18.0R)
        Me.TextBox3.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox3.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(1.4500000476837158R)
        Me.TextBox3.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox3.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox3.Value = "นักเรียนมีการสนใจเข้าใช้งานมากขึ้นเป็นแนวโน้มที่ดี ซึ่งสามารถใช้ข้อมูลจาก Nobel D" & _
    "irector เพื่อดูห้องที่ขยัน และ แสดงความชื่นชม หรือติดประกาศห้องขยันประจำเดือน เพ" & _
    "ื่อสร้างความสนใจจากห้องอื่นๆได้"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.1300029754638672R), Telerik.Reporting.Drawing.Unit.Cm(7.2664861679077148R))
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.84167194366455R), Telerik.Reporting.Drawing.Unit.Cm(1.833614706993103R))
        Me.TextBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None
        Me.TextBox4.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid
        Me.TextBox4.Style.Color = System.Drawing.Color.Black
        Me.TextBox4.Style.Font.Name = "Angsana New"
        Me.TextBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(18.0R)
        Me.TextBox4.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Cm(1.4500000476837158R)
        Me.TextBox4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox4.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806R)
        Me.TextBox4.Value = "สถานะอุปกรณ์ไอทีที่ไม่ครบ 100% อาจส่งผลให้การเรียนการสอนติดขัดได้ ควรติดตามผู้ดูแ" & _
    "ลให้รีบแก้ไข หรือเปลี่ยนอุปกรณ์ทดแทนในทันที"
        '
        'txtTotalStudentActive
        '
        Me.txtTotalStudentActive.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3999998569488525R), Telerik.Reporting.Drawing.Unit.Cm(4.6853084564208984R))
        Me.txtTotalStudentActive.Name = "txtTotalStudentActive"
        Me.txtTotalStudentActive.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(0.89458382129669189R))
        Me.txtTotalStudentActive.Style.Font.Name = "Angsana New"
        Me.txtTotalStudentActive.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalStudentActive.Value = ""
        '
        'TextBox1
        '
        Me.TextBox1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.466041088104248R), Telerik.Reporting.Drawing.Unit.Cm(10.600198745727539R))
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7631258964538574R), Telerik.Reporting.Drawing.Unit.Cm(0.99999785423278809R))
        Me.TextBox1.Style.Font.Bold = True
        Me.TextBox1.Style.Font.Name = "Angsana New"
        Me.TextBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(25.0R)
        Me.TextBox1.Style.Font.Underline = True
        Me.TextBox1.Value = "วันที่มีการใช้งานมากที่สุด"
        '
        'Shape5
        '
        Me.Shape5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.0R), Telerik.Reporting.Drawing.Unit.Cm(9.80009937286377R))
        Me.Shape5.Name = "Shape5"
        Me.Shape5.ShapeType = New Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW)
        Me.Shape5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.399900436401367R), Telerik.Reporting.Drawing.Unit.Cm(0.7998998761177063R))
        '
        'txtDay1
        '
        Me.txtDay1.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6633474826812744R), Telerik.Reporting.Drawing.Unit.Cm(12.300000190734863R))
        Me.txtDay1.Name = "txtDay1"
        Me.txtDay1.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158R))
        Me.txtDay1.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.txtDay1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay1.Style.Visible = True
        Me.txtDay1.Value = "1"
        '
        'txtDay2
        '
        Me.txtDay2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(12.300000190734863R))
        Me.txtDay2.Name = "txtDay2"
        Me.txtDay2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay2.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.txtDay2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay2.Value = "2"
        '
        'txtDay3
        '
        Me.txtDay3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.663750171661377R), Telerik.Reporting.Drawing.Unit.Cm(12.30000114440918R))
        Me.txtDay3.Name = "txtDay3"
        Me.txtDay3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay3.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.txtDay3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay3.Value = "3"
        '
        'txtDay4
        '
        Me.txtDay4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(12.300003051757812R))
        Me.txtDay4.Name = "txtDay4"
        Me.txtDay4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay4.Style.BackgroundColor = System.Drawing.Color.Empty
        Me.txtDay4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay4.Value = "4"
        '
        'txtDay5
        '
        Me.txtDay5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.663748741149902R), Telerik.Reporting.Drawing.Unit.Cm(12.300004959106445R))
        Me.txtDay5.Name = "txtDay5"
        Me.txtDay5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay5.Value = "5"
        '
        'txtDay6
        '
        Me.txtDay6.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.663750648498535R), Telerik.Reporting.Drawing.Unit.Cm(12.29999828338623R))
        Me.txtDay6.Name = "txtDay6"
        Me.txtDay6.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay6.Value = "6"
        '
        'txtDay7
        '
        Me.txtDay7.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.663749694824219R), Telerik.Reporting.Drawing.Unit.Cm(12.300004959106445R))
        Me.txtDay7.Name = "txtDay7"
        Me.txtDay7.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay7.Value = "7"
        '
        'txtDay14
        '
        Me.txtDay14.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.662941932678223R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay14.Name = "txtDay14"
        Me.txtDay14.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay14.Value = "14"
        '
        'txtDay13
        '
        Me.txtDay13.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.663145065307617R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay13.Name = "txtDay13"
        Me.txtDay13.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay13.Value = "13"
        '
        'txtDay12
        '
        Me.txtDay12.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.663345336914063R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay12.Name = "txtDay12"
        Me.txtDay12.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay12.Value = "12"
        '
        'txtDay11
        '
        Me.txtDay11.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.66354751586914R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay11.Name = "txtDay11"
        Me.txtDay11.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay11.Value = "11"
        '
        'txtDay10
        '
        Me.txtDay10.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.663750171661377R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay10.Name = "txtDay10"
        Me.txtDay10.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay10.Style.Visible = True
        Me.txtDay10.Value = "10"
        '
        'txtDay9
        '
        Me.txtDay9.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6639509201049805R), Telerik.Reporting.Drawing.Unit.Cm(13.500203132629395R))
        Me.txtDay9.Name = "txtDay9"
        Me.txtDay9.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay9.Style.Visible = True
        Me.txtDay9.Value = "9"
        '
        'txtDay8
        '
        Me.txtDay8.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(13.500000953674316R))
        Me.txtDay8.Name = "txtDay8"
        Me.txtDay8.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay8.Style.Visible = True
        Me.txtDay8.Value = "8"
        '
        'txtDay15
        '
        Me.txtDay15.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay15.Name = "txtDay15"
        Me.txtDay15.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay15.Style.Visible = True
        Me.txtDay15.Value = "15"
        '
        'txtDay16
        '
        Me.txtDay16.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6639509201049805R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay16.Name = "txtDay16"
        Me.txtDay16.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay16.Style.Visible = True
        Me.txtDay16.Value = "16"
        '
        'txtDay17
        '
        Me.txtDay17.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.663750171661377R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay17.Name = "txtDay17"
        Me.txtDay17.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay17.Style.Visible = True
        Me.txtDay17.Value = "17"
        '
        'txtDay18
        '
        Me.txtDay18.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.66354751586914R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay18.Name = "txtDay18"
        Me.txtDay18.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay18.Value = "18"
        '
        'txtDay19
        '
        Me.txtDay19.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.663345336914063R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay19.Name = "txtDay19"
        Me.txtDay19.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay19.Value = "19"
        '
        'txtDay20
        '
        Me.txtDay20.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.663145065307617R), Telerik.Reporting.Drawing.Unit.Cm(14.70040225982666R))
        Me.txtDay20.Name = "txtDay20"
        Me.txtDay20.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay20.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay20.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay20.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay20.Value = "20"
        '
        'txtDay21
        '
        Me.txtDay21.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.662941932678223R), Telerik.Reporting.Drawing.Unit.Cm(14.70000171661377R))
        Me.txtDay21.Name = "txtDay21"
        Me.txtDay21.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay21.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay21.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay21.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay21.Value = "21"
        '
        'txtDay22
        '
        Me.txtDay22.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay22.Name = "txtDay22"
        Me.txtDay22.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay22.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay22.Style.Visible = True
        Me.txtDay22.Value = "22"
        '
        'txtDay23
        '
        Me.txtDay23.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6639509201049805R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay23.Name = "txtDay23"
        Me.txtDay23.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay23.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay23.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay23.Style.Visible = True
        Me.txtDay23.Value = "23"
        '
        'txtDay24
        '
        Me.txtDay24.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.663750171661377R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay24.Name = "txtDay24"
        Me.txtDay24.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay24.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay24.Style.Visible = True
        Me.txtDay24.Value = "24"
        '
        'txtDay25
        '
        Me.txtDay25.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.66354751586914R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay25.Name = "txtDay25"
        Me.txtDay25.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay25.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay25.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay25.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay25.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay25.Value = "25"
        '
        'txtDay26
        '
        Me.txtDay26.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.663345336914063R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay26.Name = "txtDay26"
        Me.txtDay26.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay26.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay26.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay26.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay26.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay26.Value = "26"
        '
        'txtDay27
        '
        Me.txtDay27.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(12.663145065307617R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay27.Name = "txtDay27"
        Me.txtDay27.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay27.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay27.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay27.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay27.Value = "27"
        '
        'txtDay28
        '
        Me.txtDay28.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.662941932678223R), Telerik.Reporting.Drawing.Unit.Cm(15.900602340698242R))
        Me.txtDay28.Name = "txtDay28"
        Me.txtDay28.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay28.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay28.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay28.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay28.Value = "28"
        '
        'txtDay29
        '
        Me.txtDay29.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6637496948242187R), Telerik.Reporting.Drawing.Unit.Cm(17.100803375244141R))
        Me.txtDay29.Name = "txtDay29"
        Me.txtDay29.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay29.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay29.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay29.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay29.Style.Visible = True
        Me.txtDay29.Value = "29"
        '
        'txtDay30
        '
        Me.txtDay30.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.6639509201049805R), Telerik.Reporting.Drawing.Unit.Cm(17.100803375244141R))
        Me.txtDay30.Name = "txtDay30"
        Me.txtDay30.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay30.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay30.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay30.Style.Visible = True
        Me.txtDay30.Value = "30"
        '
        'txtDay31
        '
        Me.txtDay31.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.663750171661377R), Telerik.Reporting.Drawing.Unit.Cm(17.100803375244141R))
        Me.txtDay31.Name = "txtDay31"
        Me.txtDay31.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0R), Telerik.Reporting.Drawing.Unit.Cm(1.1999994516372681R))
        Me.txtDay31.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid
        Me.txtDay31.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15.0R)
        Me.txtDay31.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtDay31.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtDay31.Style.Visible = True
        Me.txtDay31.Value = "31"
        '
        'txtBottomDescription
        '
        Me.txtBottomDescription.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3937499523162842R), Telerik.Reporting.Drawing.Unit.Cm(18.850826263427734R))
        Me.txtBottomDescription.Name = "txtBottomDescription"
        Me.txtBottomDescription.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.830437660217285R), Telerik.Reporting.Drawing.Unit.Cm(2.0482432842254639R))
        Me.txtBottomDescription.Style.Font.Name = "Angsana New"
        Me.txtBottomDescription.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(20.0R)
        Me.txtBottomDescription.Value = resources.GetString("txtBottomDescription.Value")
        '
        'txtTotalDeviceDetail
        '
        Me.txtTotalDeviceDetail.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3999998569488525R), Telerik.Reporting.Drawing.Unit.Cm(7.2000999450683594R))
        Me.txtTotalDeviceDetail.Name = "txtTotalDeviceDetail"
        Me.txtTotalDeviceDetail.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(0.89458215236663818R))
        Me.txtTotalDeviceDetail.Style.Font.Name = "Angsana New"
        Me.txtTotalDeviceDetail.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalDeviceDetail.Value = ""
        '
        'txtTotalDeviceDetail2
        '
        Me.txtTotalDeviceDetail2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.3999998569488525R), Telerik.Reporting.Drawing.Unit.Cm(8.094883918762207R))
        Me.txtTotalDeviceDetail2.Name = "txtTotalDeviceDetail2"
        Me.txtTotalDeviceDetail2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(0.818640410900116R))
        Me.txtTotalDeviceDetail2.Style.Font.Name = "Angsana New"
        Me.txtTotalDeviceDetail2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalDeviceDetail2.Value = ""
        '
        'PictureBox4
        '
        Me.PictureBox4.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.98520833253860474R), Telerik.Reporting.Drawing.Unit.Cm(4.6853089332580566R))
        Me.PictureBox4.MimeType = "image/png"
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.6560418605804443R), Telerik.Reporting.Drawing.Unit.Cm(1.9000002145767212R))
        Me.PictureBox4.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureBox4.Value = CType(resources.GetObject("PictureBox4.Value"), Object)
        '
        'PictureBox5
        '
        Me.PictureBox5.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.48250001668930054R), Telerik.Reporting.Drawing.Unit.Cm(7.4103975296020508R))
        Me.PictureBox5.MimeType = "image/png"
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5820834636688232R), Telerik.Reporting.Drawing.Unit.Cm(1.3708335161209106R))
        Me.PictureBox5.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureBox5.Value = CType(resources.GetObject("PictureBox5.Value"), Object)
        '
        'PictureBox2
        '
        Me.PictureBox2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.98520833253860474R), Telerik.Reporting.Drawing.Unit.Cm(4.6853084564208984R))
        Me.PictureBox2.MimeType = "image/png"
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.708958625793457R), Telerik.Reporting.Drawing.Unit.Cm(1.9000004529953003R))
        Me.PictureBox2.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureBox2.Value = CType(resources.GetObject("PictureBox2.Value"), Object)
        '
        'PictureBox3
        '
        Me.PictureBox3.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.429583340883255R), Telerik.Reporting.Drawing.Unit.Cm(7.5175991058349609R))
        Me.PictureBox3.MimeType = "image/png"
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.476250171661377R), Telerik.Reporting.Drawing.Unit.Cm(1.3959243297576904R))
        Me.PictureBox3.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch
        Me.PictureBox3.Value = CType(resources.GetObject("PictureBox3.Value"), Object)
        '
        'txtIndexPage
        '
        Me.txtIndexPage.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.971673011779785R), Telerik.Reporting.Drawing.Unit.Cm(21.311450958251953R))
        Me.txtIndexPage.Name = "txtIndexPage"
        Me.txtIndexPage.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0R), Telerik.Reporting.Drawing.Unit.Cm(1.0R))
        Me.txtIndexPage.Style.Font.Name = "Angsana New"
        Me.txtIndexPage.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(23.0R)
        Me.txtIndexPage.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center
        Me.txtIndexPage.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle
        Me.txtIndexPage.Value = ""
        '
        'txtTotalSchoolActive2
        '
        Me.txtTotalSchoolActive2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.4000008106231689R), Telerik.Reporting.Drawing.Unit.Cm(3.1318404674530029R))
        Me.txtTotalSchoolActive2.Name = "txtTotalSchoolActive2"
        Me.txtTotalSchoolActive2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(1.0315397977828979R))
        Me.txtTotalSchoolActive2.Style.Font.Name = "Angsana New"
        Me.txtTotalSchoolActive2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalSchoolActive2.Value = ""
        '
        'txtTotalStudentActive2
        '
        Me.txtTotalStudentActive2.Location = New Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.393749475479126R), Telerik.Reporting.Drawing.Unit.Cm(5.5800929069519043R))
        Me.txtTotalStudentActive2.Name = "txtTotalStudentActive2"
        Me.txtTotalStudentActive2.Size = New Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5643830299377441R), Telerik.Reporting.Drawing.Unit.Cm(0.89458382129669189R))
        Me.txtTotalStudentActive2.Style.Font.Name = "Angsana New"
        Me.txtTotalStudentActive2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(22.0R)
        Me.txtTotalStudentActive2.Value = ""
        '
        'MasterSchoolReport
        '
        Me.Items.AddRange(New Telerik.Reporting.ReportItemBase() {Me.pageHeaderSection1, Me.detail})
        Me.Name = "MasterSchoolReport"
        Me.PageSettings.Landscape = False
        Me.PageSettings.Margins = New Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929R))
        Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
        Me.Style.BackgroundColor = System.Drawing.Color.White
        Me.Width = Telerik.Reporting.Drawing.Unit.Cm(19.400001525878906R)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents pageHeaderSection1 As Telerik.Reporting.PageHeaderSection
    Friend WithEvents detail As Telerik.Reporting.DetailSection
    Friend WithEvents Shape1 As Telerik.Reporting.Shape
    Friend WithEvents txtAddress As Telerik.Reporting.TextBox
    Friend WithEvents txtTelephone As Telerik.Reporting.TextBox
    Friend WithEvents txtFax As Telerik.Reporting.TextBox
    Friend WithEvents txtHeader As Telerik.Reporting.TextBox
    Friend WithEvents txtSchoolName As Telerik.Reporting.HtmlTextBox
    Friend WithEvents PictureBox1 As Telerik.Reporting.PictureBox
    Friend WithEvents PictureBox2 As Telerik.Reporting.PictureBox
    Friend WithEvents PictureBox3 As Telerik.Reporting.PictureBox
    Friend WithEvents txtTotalSchoolActive As Telerik.Reporting.TextBox
    Friend WithEvents TextBox2 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox3 As Telerik.Reporting.TextBox
    Friend WithEvents TextBox4 As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalStudentActive As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalDeviceDetail As Telerik.Reporting.TextBox
    Friend WithEvents TextBox1 As Telerik.Reporting.TextBox
    Friend WithEvents Shape5 As Telerik.Reporting.Shape
    Friend WithEvents txtDay1 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay2 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay3 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay4 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay5 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay6 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay7 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay14 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay13 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay12 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay11 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay10 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay9 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay8 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay15 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay16 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay17 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay18 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay19 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay20 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay21 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay22 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay23 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay24 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay25 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay26 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay27 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay28 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay29 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay30 As Telerik.Reporting.TextBox
    Friend WithEvents txtDay31 As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalDeviceDetail2 As Telerik.Reporting.TextBox
    Friend WithEvents txtBottomDescription As Telerik.Reporting.TextBox
    Friend WithEvents PictureBox4 As Telerik.Reporting.PictureBox
    Friend WithEvents PictureBox5 As Telerik.Reporting.PictureBox
    Friend WithEvents txtIndexPage As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalSchoolActive2 As Telerik.Reporting.TextBox
    Friend WithEvents txtTotalStudentActive2 As Telerik.Reporting.TextBox
End Class