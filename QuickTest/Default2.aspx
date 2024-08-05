<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default2.aspx.vb" Inherits="QuickTest.Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <%--<script src="js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" href="./css/reset.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="./css/style.css" />
    <link rel="stylesheet" type="text/css" href="./css/iframestyle.css" />
    <link rel="stylesheet" type="text/css" href="./css/contactstyle.css" />
    <link href="css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
        <script src="html5.js"></script>
    <![endif]-->
    <script type="text/javascript" src="./js/modernizr-1.5.min.js"></script>
    <link rel="stylesheet" type="text/css" href="./shadowbox/shadowbox.css">
    <link href="css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="./shadowbox/shadowbox.js"></script>
    <style type="text/css">
        table td {
            padding: 10px !important;
            font-size: 40px;
            text-align: center !important;
            border-radius: 6px;
        }
        .ForPanel {
        width:1000px;
        position:relative;
        left:-10px;
        }
    </style>

    <script type="text/javascript">
        var JVStrhref = '<%=Strhref %>';
        var JSOSName = '<%=OSCheck %>';
        function startDownload() {
            document.getElementById("download").src = JVStrhref;
        }
        if (JSOSName != 'ios') {
            setTimeout(startDownload, 1000);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <iframe id="download" width="1" height="1"  style="display:none"></iframe>
        <div id="main">
            <header style="height: 255px;">
                <div id="logo" style="padding-top: 5px">
                    <div id="logo_text">
                        <img class="imgLogo" style="vertical-align: middle; margin: 0 50px 0 230px;" src="./images/logoQ.png" alt="logo" />
                    </div>
                </div>
            </header>
            <!--[if lte IE 7]><br /><br /><br /><![endif]-->
            <div id="site_content" style="padding:0px 0 15px 0">

                <div class="content" style="margin-left: 50px;">
                    <div class="form_settings" style='width: 830px;'>
                        <div style="width:800px;margin-left:auto;margin-right:auto;text-align:center;">
                            <div>
                                <asp:Label ID="lblThankYou" style="float:none;font-size:25px;font-weight:bold;" runat="server" Text="วิธีการดาวน์โหลด และติดตั้ง"></asp:Label>
                            </div>
                            <asp:Label ID="lblHead" runat="server" Style="float:none;" Text="ไฟล์จะเริ่มดาวน์โหลดเอง โดยทำตาม 3 ขั้นตอนง่ายๆ นี้ค่ะ"></asp:Label>
                            <div id="DivLinkDownload" runat="server">
                                <a style="cursor:pointer;" href="<%=Strhref %>">โปรดคลิกที่นี่เพื่อลองอีกครั้ง</a>
                            </div>
                        </div>

                        <asp:Panel ID="PanelWindow" Visible="false" CssClass="ForPanel" runat="server">
                            <img src="ChromeInstaller/windows/1.png" />
                            <img src="ChromeInstaller/windows/2.png" />
                            <img src="ChromeInstaller/windows/3.png" />
                        </asp:Panel>

                        <asp:Panel ID="PanelMac" Visible="false" CssClass="ForPanel" runat="server">
                            <img src="ChromeInstaller/mac/1.png" />
                            <img src="ChromeInstaller/mac/2.png" />
                            <img src="ChromeInstaller/mac/3.png" />
                        </asp:Panel>

                        <asp:Panel ID="PanelLinux" Visible="false" style="width:600px;margin-left:auto;margin-right:auto;margin-top:20px;position:relative;left:20px;" runat="server">
                            <a href="ChromeInstaller/ChromeDownloadHandler.ashx?OSName=linux&FileVersion=Chrome_32.deb"><img src="ChromeInstaller/linux/1.png" style="cursor:pointer;" /></a>
                            <a href="ChromeInstaller/ChromeDownloadHandler.ashx?OSName=linux&FileVersion=Chrome_64.deb"><img src="ChromeInstaller/linux/2.png" style="cursor:pointer;" /></a>
                            <a href="ChromeInstaller/ChromeDownloadHandler.ashx?OSName=linux&FileVersion=Chrome_32.rpm"><img src="ChromeInstaller/linux/3.png" style="cursor:pointer;" /></a>
                            <a href="ChromeInstaller/ChromeDownloadHandler.ashx?OSName=linux&FileVersion=Chrome_64.rpm"><img src="ChromeInstaller/linux/4.png" style="cursor:pointer;" /></a>
                        </asp:Panel>

                        <asp:Panel ID="PanelIOS" Visible="false" CssClass="ForPanel" runat="server">
                            <img src="ChromeInstaller/windows/1.png" />
                            <img src="ChromeInstaller/windows/2.png" />
                            <img src="ChromeInstaller/windows/3.png" />
                        </asp:Panel>

                    </div>
                </div>
            </div>
            <footer>
                <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
            </footer>
        </div>
    </form>
</body>
</html>
