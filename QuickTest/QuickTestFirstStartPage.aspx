<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuickTestFirstStartPage.aspx.vb" Inherits="QuickTest.QuickTestFirstStartPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <telerik:RadCodeBlock ID='RadcodeBlock1' runat="server">

        <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.js" type="text/javascript"></script>
        <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/reset.css" type="text/css" />
        <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>css/style.css" />
        <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>css/iframestyle.css" />
        <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>css/contactstyle.css" />
        <link href="<%=ResolveUrl("~")%>css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
        <!--[if lt IE 9]>
        <script src="html5.js"></script>
    <![endif]-->
        <script type="text/javascript" src="<%=ResolveUrl("~")%>js/modernizr-1.5.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css">
        <link href="<%=ResolveUrl("~")%>css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%=ResolveUrl("~")%>shadowbox/shadowbox.js"></script>
        
    </telerik:RadCodeBlock>
    <style type="text/css">
        table td {
            padding: 10px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
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
                                <td>ใส่ Key ได้เลยค่ะ</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadMaskedTextBox ID="RadMaskedTextBox1" runat="server" Width="750" SelectionOnFocus="SelectAll" Label="KEY:" Font-Size="30px"
                                        Mask="aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa-aaaa">
                                    </telerik:RadMaskedTextBox>
                                    <div>
                                        <asp:Label ID="lblWarning" Style="float: initial;" ForeColor="Red" runat="server" Text="" Visible="false"></asp:Label></div>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 830px; margin-left: auto; margin-right: auto; margin-top: 30px; text-align: center;">
                            <asp:Button ID="BtnSubmit" runat="server" Width="100px" Text="ลงทะเบียน" Style="position: relative; width: 150px; height: 55px; font-size: 30px;" class="submit" />
                        </div>
                    </div>
                </div>
                <% If HttpContext.Current.Application("NeedQuizMode") = True Then%>
                <ul id="Help">
                    <li class="about2" style="z-index: 99;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
                        id="HelpLogin" onclick="ShowHelp();">
                        <%--ช่วย<br />
                    เหลือ<br />--%>
                        <div style="margin-top: 7px;">
                            <span style="font-size: initial; position: relative; left: 10px;">ช่วย</span>
                        </div>
                        <div>
                            <span style="font-size: initial; position: relative; left: 8px;">เหลือ</span>
                        </div>
                    </a></li>
                </ul>
                <% End If%>
                <script type="text/javascript">
                    $(function () {
                        $('#CloseHelp a').stop().animate({ 'marginLeft': '-10px' }, 1000);
                        $('#CloseHelp > li').hover(function () {
                            $('a', $(this)).stop().animate({ 'marginLeft': '-66px' }, 200);
                        }, function () {
                            $('a', $(this)).stop().animate({ 'marginLeft': '-10px' }, 200);
                        });
                    });
                </script>
                <div id='HowToDialog' style="display: none; width: 100%; height: 100%; z-index: 0; position: fixed; top: 0px; left: 0px; background-color: Black">
                    <table style="height: 100%;">
                        <tr>
                            <td style="width: 150px; height: 100%;">
                                <div class='menuItem' id='Report'>
                                    ดูรายงาน
                                </div>
                                <div class='menuItem' id='manageQuiz'>
                                    การจัดชุดข้อสอบ
                                </div>
                                <div class='menuItem' id='modifyQuiz'>
                                    การแก้ไขชุดข้อสอบ
                                </div>
                                <div class='menuItem' id='introduction'>
                                    วิธีการใช้งานเบื้องต้น
                                </div>
                            </td>
                            <td style="height: 100%;">
                                <iframe id="FrameShowHowTo" scrolling="no" style="overflow: hidden; white-space: nowrap; width: 100%; height: 100%; position: relative; margin-left: auto; margin-right: auto; z-index: 0;"
                                    frameborder="0"></iframe>
                                <ul id="CloseHelp">
                                    <li class="about1" style="z-index: 999;"><a title="จบการฝึกฝน" id="A2" onclick="CloseHelp();">จบการ<br />
                                        ฝึกฝน </a></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
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
