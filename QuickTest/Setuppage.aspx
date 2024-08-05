<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Setuppage.aspx.vb" Inherits="QuickTest.Setuppage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="./css/reset.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <link rel="stylesheet" type="text/css" href="./css/iframestyle.css" />
    <link rel="stylesheet" type="text/css" href="./css/contactstyle.css" />
    <link href="css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />

    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">

            function PostChangeIp(InputIP, InputGateWay, InputSubnetMask, InputDNS1, InputDNS2) {
                $.ajax({
                    type: "POST",
                    url: '<%=ResolveUrl("~")%>Setuppage.aspx/ChangeIP',
                    data: "{ IP : '" + InputIP + "',GateWay: '" + InputGateWay + "',SubnetMask: '" + InputSubnetMask + "',DNS1:'" + InputDNS1 + "',DNS2:'" + InputDNS2 + "'}",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                    },
                    error: function myfunction(request, status) {
                        //alert('SetUnload');
                    }
                });
            }

        </script>
    </telerik:RadCodeBlock>



    <style type='text/css'>
        table td {
            padding: 10px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
            vertical-align: bottom;
        }

        .Fortdleft {
            width: 35%;
        }

        .FortdRight {
            width: 1%;
        }

        .RadPicker_Default .rcCalPopup, .RadPicker_Default a.rcDisabled.rcCalPopup:hover {
            position: relative;
            left: -15px;
        }

        .rcTable tr td {
            background: none;
        }

        #RadDatePicker1_dateInput_text, #RadDatePicker2_dateInput_text {
            height: 50px !important;
            font-size: 35px;
        }

        .lblValidate {
            font-size: 30px;
            color: red;
        }

        #PanelStep2 span {
            float: initial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div id="main">
            <header style="height: 300px;">
                <div id="logo" style="padding-top: 5px">
                    <div id="logo_text">
                        <img class="imgLogo" style="vertical-align: middle; margin: 0 50px 0 230px;" src="./images/logoQ.png" alt="logo" />
                    </div>
                </div>
            </header>
            <!--[if lte IE 7]><br /><br /><br /><![endif]-->
            <div id="site_content">

                <div class="content" style="margin-left: 50px;">
                    <div class="form_settings" style='width: 830px;'>
                        <table>
                            <tr>
                                <td>

                                    <asp:Panel ID="PanelStep1" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="3">ตั้งค่า IP เครื่องนี้</td>
                                            </tr>
                                            <tr>
                                                <td class="Fortdleft">IP</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtIP" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td class="FortdRight">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" ControlToValidate="txtIP" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Fortdleft">Mask</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtMask" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td class="FortdRight">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" ControlToValidate="txtMask" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Fortdleft">GW</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtGW" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td class="FortdRight">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" ControlToValidate="txtGW" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Fortdleft">DNS1</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtDNS1" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td class="FortdRight">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" ControlToValidate="txtDNS1" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Fortdleft">DNS2</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtDNS2" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep2" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="2">ตั้งค่า Proxy (ถ้าต้องใช้)</td>
                                            </tr>
                                            <tr>
                                                <td>Proxy IP</td>
                                                <td style="text-align: left!important;">
                                                    <telerik:RadMaskedTextBox ID="txtProxyIP" ClientIDMode="Static" ZeroPadNumericRanges="true" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px" AutoPostBack="true"
                                                        OnTextChanged="txtProxy_Change"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">                                                       
                                                    </telerik:RadMaskedTextBox>
                                                    <asp:Label ID="lbltxtProxyIP" Visible="false" CssClass="lblValidate" runat="server" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Proxy Port</td>
                                                <td style="text-align: left!important;">
                                                    <telerik:RadMaskedTextBox ID="txtProxyPort" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px" AutoPostBack="true"
                                                        OnTextChanged="txtProxy_Change"
                                                        Mask="<0..65535>" Enabled="false">
                                                    </telerik:RadMaskedTextBox>
                                                    <asp:Label ID="lbltxtProxyPort" Visible="false" CssClass="lblValidate" runat="server" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Proxy User</td>
                                                <td style="text-align: left !important;">
                                                    <asp:TextBox ID="txtProxyUser" Width="250" Font-Size="30px" runat="server" MaxLength="20" AutoPostBack="true"
                                                        OnTextChanged="txtProxy_Change"  Enabled="false"></asp:TextBox>
                                                    <asp:Label ID="lbltxtProxyUser" Visible="false" CssClass="lblValidate" runat="server" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Proxy Password</td>
                                                <td style="text-align: left !important;">
                                                    <asp:TextBox ID="txtProxyPassword" Width="250" Font-Size="30px" MaxLength="20" runat="server"  Enabled="false"></asp:TextBox>
                                                    <asp:Label ID="lbltxtProxyPassword" Visible="false" CssClass="lblValidate" runat="server" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep3" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="2">ตั้งค่า SMTP สำหรับส่งอีเมล์</td>
                                            </tr>
                                            <tr>
                                                <td>SMTP Server IP</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtSMTPServerIP" ClientIDMode="Static" ZeroPadNumericRanges="true" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>SMTP Server Port</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtSMTPServerPort" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..65535>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>SMTP User</td>
                                                <td style="text-align: left !important;">
                                                    <asp:TextBox ID="txtSMTPUser" Width="250" Font-Size="30px" runat="server" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>SMTP Password</td>
                                                <td style="text-align: left !important;">
                                                    <asp:TextBox ID="txtSMTPPassword" Width="250" Font-Size="30px" MaxLength="50" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep4" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="2">ตั้งค่า Email ผู้รับคำถาม/ข้อสงสัย จากหน้าแรก</td>
                                            </tr>
                                            <tr>
                                                <td>ส่งคำถามไปยัง</td>
                                                <td>
                                                    <asp:TextBox ID="txtAskQuestionRecipient" Width="360" MaxLength="500" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Style="float: initial;" ForeColor="Red" Width="50" ControlToValidate="txtAskQuestionRecipient" runat="server" ErrorMessage="*" ValidationExpression="^(\s*,?\s*[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+\s*$"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep5" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="2">ตั้งค่า Email ผู้บริหารโรงเรียนสำหรับรับรายงานวิเคราะห์/แนะนำนักเรียนรายคน</td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 30px;">ส่งรายงานประจำเดือนไปยัง</td>
                                                <td>
                                                    <asp:TextBox ID="txtRptConsultantRecipient" Width="360" MaxLength="500" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Style="float: initial;" ForeColor="Red" Width="50" ControlToValidate="txtRptConsultantRecipient" runat="server" ErrorMessage="*" ValidationExpression="^(\s*,?\s*[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+\s*$"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep6" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="4">ตั้งค่าปฏิทินการศึกษา</td>
                                            </tr>
                                            <tr>
                                                <td>ภาคการศึกษา</td>
                                                <td>
                                                    <asp:TextBox ID="txtCalendarName" MaxLength="100" Width="200" runat="server"></asp:TextBox>
                                                </td>
                                                <td>ปี</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtCalendarYear" ClientIDMode="Static" runat="server" Width="200" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="####">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>ตั้งแต่วันที่</td>
                                                <td>
                                                    <telerik:RadDatePicker ID="RadDatePicker1" Width="100px" Style="position: relative !important; left: -70px !important;" runat="server"></telerik:RadDatePicker>
                                                </td>
                                                <td>ถึง</td>
                                                <td>
                                                    <telerik:RadDatePicker ID="RadDatePicker2" Width="100px" Style="position: relative !important; left: -70px !important;" runat="server"></telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Font-Size="15px" ControlToValidate="txtCalendarName" runat="server" Style="float: initial;" ForeColor="Red" ErrorMessage="ต้องใส่ภาคการศึกษาค่ะ"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Font-Size="15px" ControlToValidate="txtCalendarYear" runat="server" Style="float: initial;" ForeColor="Red" ErrorMessage="ต้องใส่ปีค่ะ"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" Font-Size="15px" ControlToValidate="RadDatePicker1" runat="server" Style="float: initial;" ForeColor="Red" ErrorMessage="ต้องเลือกวันที่เริ่มค่ะ"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Font-Size="15px" ControlToValidate="RadDatePicker2" runat="server" Style="float: initial;" ForeColor="Red" ErrorMessage="ต้องเลือกวันที่สิ้นสุดค่ะ"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Date" Font-Size="15px" ControlToCompare="RadDatePicker1" Style="float: initial;" ControlToValidate="RadDatePicker2" ForeColor="Red" ErrorMessage="รูปแบบวันที่ผิดค่ะ" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel ID="PanelStep6When360False" Visible="false" runat="server">
                                        <table>
                                            <tr>
                                                <td colspan="3">Parent Server IP</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 38%;">ParentServer IP</td>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="txtParentServerIp" ZeroPadNumericRanges="true" ClientIDMode="Static" runat="server" Width="250" SelectionOnFocus="SelectAll" Font-Size="30px"
                                                        Mask="<0..255>.<0..255>.<0..255>.<0..255>">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td class="FortdRight">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" ControlToValidate="txtParentServerIp" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <span id="spnWarning" style="float: initial; color: green; display: none;">บันทึกแล้วค่ะ</span>
                                </td>

                            </tr>
                        </table>
                        <div style="width: 830px; margin-left: auto; margin-right: auto; margin-top: 30px; text-align: center;">
                            <asp:Button ID="BtnPrev" runat="server" Width="100px" Text="กลับ" Style="position: relative; width: 150px; height: 55px; font-size: 30px; float: left;" Visible="false" class="submit" />
                            <asp:Button ID="BtnNext" runat="server" Width="100px" Text="ถัดไป" Style="position: relative; width: 150px; height: 55px; font-size: 30px; float: right;" class="submit" />
                            <asp:Button ID="BtnSave" runat="server" ClientIDMode="Static" Width="100px" Text="บันทึก" Style="position: relative; width: 150px; height: 55px; font-size: 30px; float: right;" Visible="false" class="submit" />
                        </div>
                    </div>
                </div>

            </div>
            <%--<asp:Button ID="Button1" runat="server" Text="Button" />--%>
            <footer>
                <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
            </footer>
        </div>
    </form>
    <script src="<%=ResolveUrl("~")%>js/bg.js" type="text/javascript"></script>

</body>
</html>
