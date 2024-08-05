<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="QuickTest._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="max-O-net maxonet onet testbank" />
    <meta name="keywords" content="max-O-net, maxonet, onet, testbank, คลังข้อสอบ, ควิซ, สอบ, o-net" />
    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="<%=ResolveUrl("~")%>css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
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
        padding:10px !important;
        font-size:40px;
        text-align:center !important;
        border-radius:6px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="main" >
        <header>
		<div id="logo" style="padding-top: 5px">
			<div id="logo_text">
            <img class="imgLogo" style="width:224px;vertical-align: middle; margin: 0 50px 0 350px;" src="./images/logoQ.png" alt="logo" />
			</div>
		</div>	  
		</header>
        <!--[if lte IE 7]><br /><br /><br /><![endif]-->
        <div id="site_content" style="padding:0px;position:relative;top:-40px;">
            <div class="content" style="margin-left: 50px;padding:0px;margin-bottom:0px">
                <div class="form_settings" style='width:830px;'>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHead" runat="server" Font-Size="25px" style="float:none;" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="BtnDownload" style="border:none;background:none;" ToolTip="ติดตั้ง Chrome" ImageUrl="~/ChromeInstaller/Logo.png" runat="server" />
                                </td>
                            </tr>
                        </table>
                    <div style="width:830px;margin-left:auto;margin-right:auto;margin-top:20px;text-align:center;">
                        <asp:Button ID="BtnSubmit" Visible="false" runat="server" Width="100px" Text="เข้าใช้งาน" style="position:relative;width:150px;height:55px;font-size:30px;" class="submit" />
                    </div>
                </div>
            </div>
        </div>
        <footer>
      <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
    </footer>
    </div>
        <div id="DivDiaglog" title="ฐานข้อมูลมีปัญหา" style="text-align:center;">
            ฐานข้อมูลมีปัญหา 
            ตอนนี้ได้ใช้ฐานข้อมูลสำรองซึ่งเป็นฐานข้อมูลเริ่มแรกอยู่ค่ะ
        </div>
    </form>
    <script type="text/javascript">
        var JVCheckLocalDBError = '<%=IsDBError%>';
        $(function () {
            $('#DivDiaglog').dialog({
                autoOpen: false,
                modal: true,
                buttons: {
                    'รับทราบ': function () {
                        $(this).dialog('close');
                    }
                }
            })
            if (JVCheckLocalDBError == 'true') {
                $('#DivDiaglog').dialog('open');
            }
        });
    </script>
</body>
</html>
