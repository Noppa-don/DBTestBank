<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LabStartPage.aspx.vb" Inherits="QuickTest.LabStartPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta name="description" content="website description" />
    <meta name="keywords" content="website keywords, website keywords" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../css/reset.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
    <link rel="stylesheet" type="text/css" href="../css/iframestyle.css" />
    <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>
    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css">
    <link rel="stylesheet" type="text/css" href="../css/contactstyle.css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
        <script src="html5.js"></script>
    <![endif]-->
    <script type="text/javascript" src="../js/modernizr-1.5.min.js"></script>
    <link href="../css/fixMenuSlide.css" rel="stylesheet" type="text/css" />
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

    <div id="main">
        <header style="height:235px;">
		<div id="logo" style="padding-top: 5px">
			<div id="logo_text">
            <img class="imgLogo" style="vertical-align: middle; margin: 0 50px 0 230px;" src="../images/logoQ.png" alt="logo" />
			</div>
		</div>	  
		</header>
        <!--[if lte IE 7]><br /><br /><br /><![endif]-->
        <div id="site_content">
            
            <div class="content" style="margin-left: 50px;">
                <div class="form_settings" style='width:830px;'>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMainDetail" style="float:none;" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSecondDetail" style="float:none;" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
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
