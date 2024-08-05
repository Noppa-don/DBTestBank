<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginPage.aspx.vb" Inherits="QuickTest.LoginPage"
    UICulture="th-TH" Culture="th-TH" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />

        <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>

        <meta name="description" content="website description" />
        <meta name="keywords" content="website keywords, website keywords" />
        <meta name="language" content="English" />
        <meta name="copyright" content="Lazar Laszlo (c) 2011" />
        <meta name="Revisit-After" content="1 Days"/>
        <meta name="robots" content="index, follow"/>
        <meta http-equiv="Content-type" content="text/html;charset=UTF-8"/>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

        <script src="js/jquery-1.7.1.js" type="text/javascript"></script>
        <script src="js/Animation.js" type="text/javascript"></script>
        <script src="js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
        <script src="js/GFB.js" type="text/javascript"></script>
        <script src="js/modernizr-1.5.min.js" type="text/javascript"></script>
        <script src="shadowbox/shadowbox.js" type="text/javascript"></script>
        <script src="js/jquery.fancybox.js" type="text/javascript"></script>

        <link rel="stylesheet" href="../css/jquery.fancybox.css" />
        <link rel="stylesheet" href="./css/reset.css" type="text/css" />
        <link rel="stylesheet" href="./css/style.css"  type="text/css" />
        <link rel="stylesheet" href="./css/iframestyle.css"  type="text/css" />
        <link rel="stylesheet" href="./css/contactstyle.css"  type="text/css" />
        <link rel="stylesheet" href="css/menuFixReviewAns.css" type="text/css" />
        <link rel="stylesheet" href="./shadowbox/shadowbox.css" type="text/css" />
        <link rel="stylesheet" href="css/fixMenuSlide.css" type="text/css" />

        <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
        <link rel="icon" href="favicon.ico" type="image/x-icon" />

        <script type="text/javascript">
            $(function () {
                //ปุ่ม Log-In
                if ($('#btnContact').length > 0) {
                    new FastButton(document.getElementById('btnContact'), TriggerServerButton);
                }

                //Tag a ทั้งหมด

                $('a').each(function () {
                    new FastButton(this, TriggerServerButton);
                });

                $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
                $('#Help > li').hover(function () {
                    $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
                }, function () {
                    $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
                });
            });
        </script>
        <script type="text/javascript">
            Shadowbox.init({
                skipSetup: false
            });
           
            window.onload = function () {
                
                //// welcome box
                //Shadowbox.open({
                //    content: '<div  scrolling="no" style="overflow: hidden;white-space: nowrap;"><img src="./images/welcome/shoutbox.png" border="0" /></div>',
                //    player: "html",
                //    title: "เริ่มกันเลยค่ะ (คลิกข้างนอกที่ใดก็ได้เพื่อปิด)",
                //    height: 290,
                //    modal: true,
                //    width: 400
                //});
            };

            function CloseHelp() {
                $('#HowToDialog').hide();
                $('#Help').show();
            }

            function WannaHave() {
                Shadowbox.open({
                    content: '<div  scrolling="no" style="overflow: hidden;white-space: nowrap;"><img src="./images/BGcallcenter2.png" border="0" /></div>',
                    player: "html",
                    /*title: "สำหรับโรงเรียน ที่ยังไม่มีรหัสเข้าใช้งาน",*/
                    height: 484,
                    width: 612
                });
            };
            function ForgotPass() {
                Shadowbox.open({
                    content: '<div  scrolling="no" style="overflow: hidden;white-space: nowrap;"><img src="./images/BGcallcenter1.png" border="0" /></div>',
                    player: "html",
                    title: "หากลืมรหัสผ่าน หรือ ป้อนแล้วเข้าไม่ได้ค่ะ",
                    height: 485,
                    width: 616
                });
            };
            function HowtoClip() {
                Shadowbox.open({
                    content: '<iframe width="640" height="360" src="http://www.youtube.com/embed/fJLBvpgwLAs" frameborder="0" allowfullscreen></iframe>',
                    player: "html",
                    title: "วิธีการใช้งาน 4 ขั้นตอนง่ายสุดๆ ",
                    height: 360,
                    width: 640
                });
            };
            function Overview() {
                Shadowbox.open({
                    content: '<iframe width="640" height="360" src="http://www.youtube.com/embed/27kGW1giNUM?rel=0" frameborder="0" allowfullscreen></iframe>',
                    player: "html",
                    title: "แนะนำการประยุกต์ใช้ในห้องเรียน ในการเรียนการสอนทั่วไป",
                    height: 360,
                    width: 640
                });
            };
            function Contact() {
                Shadowbox.open({
                    content: '<div style="overflow: hidden;white-space: nowrap; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="Contact.aspx" frameborder="0" width="700" height="570"></iframe></div>',
                    player: "html",
                    title: "ติดต่อเรา",
                    height: 570,
                    width: 700
                });
            };

            function HowTo() {
                Shadowbox.open({
                    content: '<div  scrolling="no" style="overflow: hidden;white-space: nowrap;z-index: 9999; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="ShowHowToPage.aspx" frameborder="0" width="100%" height="100%"  ></iframe></div>',
                    player: "html",
                    height: 850,
                    width: 1500
                });
            };
    </script>
    
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .MenuList {
            font: 18px 'THSarabunNew';
            color: #362C20;
            line-height: 7.5em;
            top: 0;
        }

        .top {
            float: left;
            width: 100%;
            height: 100%;
            position: relative;
            top: 0;
            background-color: #FFC76F;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.7);
        }

        .menuItem {
            position: relative;
            float: left;
            padding: 5px 15px 5px 15px;
            height: 60px;
            font-size: 18px;
        }

            .menuItem:hover {
                background-color: #FDA212;
                -webkit-border-radius: 5px;
                padding: 10px 15px 5px 15px;
                height: 80px;
                font-weight: bold;
                cursor: pointer;
            }

        .selected {
            background-color: #FDA212;
            -webkit-border-radius: 5px;
            height: 80px;
            color: #F4F7FF;
        }

        .clearAll {
            clear: both;
            line-height: 0;
            height: 0;
        }

        #HowTo {
            width: 100%;
            height: 100%;
            position: relative;
            background-color: #FFFFFF;
            float: left;
        }

        .style3 {
            font-size: medium;
            width: 188px;
            background-color: #FFFFFF;
        }

        .style4 {
            width: 62px;
            background-color: #FFFFFF;
        }

        .style5 {
            width: 119px;
            background-color: #FFFFFF;
        }

        .style6 {
            height: 38px;
            background-color: #FFFFFF;
        }

        .style7 {
            height: 20px;
            background-color: #FFFFFF;
        }

        #btnPractice {
            /*background-image: url('/Images/btnGoPractice.png');*/
            
            width: 180px;
            margin: 0px 0px 0px 50px;
            position: relative;
            cursor: pointer;
        }

        .btnImageLogin {
            padding: initial !important;
            width: initial !important;
            font: initial !important;
            border: initial !important;
            background: initial !important;
            color: initial !important;
            border-radius: initial !important;
            position: relative;
        }

                .ui-dialog
        {
            top: 100px!important;
            width: 620px!important
        }
    </style>

    <style type="text/css">

        #header{
            background:white;
            margin-bottom:15px;
        }
        #mainbody{
            background: white;
            width:100%;
        	
        }
        #footer{
            background:white;
        }
        #v{
            width:320px;
            height:240px;
        }
        #qr-canvas{
            display:none;
        }
        #qrfile{
            width:320px;
            height:240px;
        }
        #mp1{
            text-align:center;
            font-size:35px;
        }
        #imghelp{
            position:relative;
            left:0px;
            top:-160px;
            z-index:100;
            font:18px arial,sans-serif;
            background:#f0f0f0;
        	margin-left:35px;
        	margin-right:35px;
        	padding-top:10px;
        	padding-bottom:10px;
        	border-radius:20px;
        }
        .selector{
            margin:0;
            padding:0;
            cursor:pointer;
            margin-bottom:-5px;
        }
        #outdiv
        {
            width:320px;
            height:240px;
        	border: solid;
        	border-width: 3px 3px 3px 3px;
            margin-top: 10px;
            margin-left: auto;
            margin-right: auto;
        }
        #result{
            border: solid;
        	border-width: 1px 1px 1px 1px;
        	padding:20px;
        	width:70%;
        }
        
        ul{
            margin-bottom:0;
            margin-right:40px;
        }
        li{
            display:inline;
            padding-right: 0.5em;
            padding-left: 0.5em;
            font-weight: bold;
       margin-top: 2px;
        }
        li a{
            text-decoration: none;
            color: black;
        }
        
        #footer a{
        	color: black;
        }
        .tsel{
            padding:0;
        }
        .Forbtn {
            margin-left: 240px;
            width: 130px;
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            /*background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);*/
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            background: #63cfdf; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iIzYzY2ZkZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiMxN2IyZDkiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* IE10+ */
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 ); /* IE6-8 */
        }
    </style>
    <script type="text/javascript">
            $(document).ready(function () {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                if (ww == 0) {
                    setTimeout(function () {
                        ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                    }, 1200);
                    if (ww == 0) { ww = 640 };
                }
                var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
                var pageLocation = window.location.href;
                var scale;                
                if (pageLocation.toLowerCase().indexOf('activitypage_pad') != -1) {
                    scale = ww / 942;
                    if (iOS) {
                        if (ww < 768) {
                            scale = ww / 580;
                        } else {
                            scale = ww / 742;
                        }
                    } else {
                        scale = ww / 942;
                    }
                } else {
                    if (iOS) {
                        if (ww < 768) { // iphone 5,6
                            scale = ww / 750;
                        } else { //ipad mini
                            scale = ww / 950;
                        }
                    } else {
                        scale = ww / 1200;
                    }                    
                }                
                $('meta[name=viewport]').attr('content', 'width=device-width,user-scalable=no,initial-scale=' + scale + ', maximum-scale=' + scale + ', minimum-scale=' + scale);                
            });
        </script>

    </head>

    <body>
        
        <script src="js/bg.js" type="text/javascript"></script>

        <form id="contact" runat="server" defaultbutton="btnLogin">
        
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <telerik:RadWindowManager ID="RadWindowManager1" VisibleStatusbar="false" runat="server" Skin="Outlook" EnableShadow="true">
                <Windows>
                    <telerik:RadWindow ID="RadDialogAlert" runat="server" Behaviors="Move" Modal="false" Height="200" Width="500" EnableShadow="true" VisibleOnPageLoad="false"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>

            <div id="main">
                <div id="logo">
                    <img class="imgLogo" style="float: left; vertical-align: middle; margin: 50px 50px 0 100px;" src="./images/Logo.png" alt="logo" />

                    <h2>ครบถ้วน</h2>&nbsp;ที่สุด พร้อมสรรพหลักสูตร 51 และ 44
                    <h2>ถูกต้อง</h2>&nbsp;ด้วยมาตรฐานทางวิชาการ
                    <h2>ฉับไว</h2>&nbsp;ภายใน 4 ขั้นตอนง่ายๆ
                </div>  
                
                <div id="site_content" style="padding: 15px 12px 0px 0;">
                    <div class="content" style="margin: 0; padding: 0;">
                            <div class="form_settings" style='width: 100%; display: inline-flex;'>

                                
                                <div style="width: 60%;">
                                    <table>
                                        <tr>
                                            <td colspan="3" class="style7">
                                                <asp:Label ID="txtvalidate" runat="server" Text="* รหัสโรงเรียน, ชื่อผู้ใช้หรือรหัสผ่านผิด ลองอีกครั้งนะคะ"
                                                    Width="307px" ForeColor="#FF3300"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trSchoolCode" runat="server" clientidmode="Static">
                                            <td class="style3">รหัสโรงเรียน</td>
                                            <td class="style5">
                                                <asp:TextBox ID="txtSchoolid" runat="server" class="contact" value="" Width="249px"></asp:TextBox>
                                            </td>
                                            <td class="style4" style="vertical-align: middle;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSchoolid"
                                                    ErrorMessage="*" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style3" style="text-align: right; padding-right: 30px; width: 50px;">ชื่อผู้ใช้</td>
                                            <td class="style5" style="display: block;">
                                                <asp:TextBox ID="txtusername" runat="server" class="conta ct" value="" Width="249px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td class="style4" style="vertical-align: middle;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtUserName"
                                                    ErrorMessage="*" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700" Width="5px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style3" style="text-align: right; padding-right: 30px; width: 50px;">รหัสผ่าน</td>
                                            <td class="style5">
                                                <asp:TextBox ID="txtpassword" runat="server" class="contact" value="" Width="249px"
                                                TextMode="Password" Text="cat" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <td class="style4" style="vertical-align: middle;">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtUserName"
                                                    ErrorMessage="*" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700" Width="5px"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style3"></td>
                                            <td colspan="2" class="style6">
                                                <%-- <asp:Button ID="BtnSubmit" ClientIDMode="Static" runat="server" Style="display: none;" />--%>
                                                <asp:Button ID="BtnSubmit" runat="server" CssClass="btnImageLogin" ClientIDMode="Static" Style="display: none;"  />
                                                <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="~/Images/btnLogin.png" CssClass="btnImageLogin" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                       
                                                <div class="sidebar">
                            <h3>มีคำถาม ?</h3>
                            <ul>
                                <li><a href="#" onclick="WannaHave();">ทำอย่างไรถึงจะได้ใช้บ้าง ?</a></li>
                                <li><a href="#" onclick="ForgotPass();">ลืมรหัสผ่าน ทำยังไง ?</a></li>
                                <li><a href="manual.pdf" target="_blank">ใช้อย่างไร ? ยากมั้ย ?</a></li>
                                <li><a href="#" onclick="Contact();">ติดต่อสอบถามเจ้าหน้าที่ </a></li>
                            </ul>
                       </div> 

                        
                    
                          </div>

                <footer> สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด </footer>
            </div>


        <script type="text/javascript" src="js/jquery.js"></script>
        <script type="text/javascript" src="js/jquery.easing-sooper.js"></script>
<%--        <script type="text/javascript" src="js/jquery.sooperfish.js"></script>--%>
    <script type="text/javascript" src="js/llqrcode.js"></script>
    <%--<script type="text/javascript" src="js/plusone.js"></script>--%>
    <script type="text/javascript" src="js/webqr.js"></script>  
    <script src="js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="js/Animation.js" type="text/javascript"></script>
    <script src="js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="css/jquery.fancybox.css" rel="stylesheet" />
    <link href="css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />

        <script type="text/javascript">
            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;

            $(document).ready(function () {
                //hover animation
                if (navigator.userAgent.indexOf('Firefox') < 0) {0
                    InjectionHover(':submit', 5);
                }
                InjectionHover('#btnPractice', 5, false);
                InjectionHover('#btnLogin', 5, false);

                $('#btnPractice').click(function () {
                    window.location = '<%=ResolveUrl("~")%>PracticeMode_Pad/MainPractice.aspx';
                });

                //ดักถ้าเข้าจาก Tablet ครู
                if (isAndroid) {}
                else {
                    //page transition
                    FadePageTransition();
                    $(':submit').click(function () {
                        FadePageTransitionOut();
                    });
                    $('#btnPractice').click(function () {
                        FadePageTransitionOut();
                    });
                }

                // Login With QR Code
                $('#ImgBtnQRLogin').click(function (e) {
                    $('#Alert').text('ไม่สามารถอ่าน QR ได้ ลองอีกครั้งนะคะ');
                    $('#Alert').hide();
                    $('#blockQRLogin').dialog('open').dialog('option', 'title', 'สแกนคิวอาร์โค๊ดเพื่อเข้าระบบ');
                    load();
                });

                $('#blockQRLogin').dialog({
                    autoOpen: false,
                    draggable: false,
                    resizable: false,
                    modal: true,
                    width: 'auto'
                });
            });

        </script>

  <script type="text/javascript">

        $(function () {
            $('#btnSelectFile').click(function () {
                $('#filePic').click();
                
            });
        });
        
        oFReader = new FileReader();

            oFReader.onload = function (oFREvent) {
 
            document.getElementById("fotoImg").src = oFREvent.target.result;
            document.getElementById("fotoImg").style.visibility = "visible";
            var screenHeight = screen.availHeight;
            screenHeight = screenHeight - 220;
            document.getElementById("fotoImg").style.height = screenHeight;

                      $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckUser",
                data: "{ StrBase64 : '" + UserData + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var a = msg.d;
                    if (a == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                            function ()
                            {
                                $('#blockQRLogin').dialog('close');
                            },5000);
 
                    } else {
                        var arr = msg.d.split(',');
                        var userName = arr[0];
                        var pwd = arr[1];
                        $('#txtusername').val(userName);
                        $('#txtpassword').val(pwd);
                        $('#blockQRLogin').dialog('close');
                        document.getElementById('<%= BtnSubmit.ClientID %>').click();
                    }
                },
                error: function myfunction(request, status) {
                 
                }
            });
        };
       
        $(function () {
            $("input:file").change(function () {
                var input = document.querySelector('input[type=file]');
                var oFile = input.files[0];
                oFReader.readAsDataURL(oFile);
            });
        });

      function ResultOK(UserData) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckUser",
                data: "{ StrBase64 : '" + UserData + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var a = msg.d;
                    if (a == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                            function ()
                            {
                                $('#blockQRLogin').dialog('close');
                            },5000);
 
                    } else {
                        var arr = msg.d.split(',');
                        var userName = arr[0];
                        var pwd = arr[1];
                        $('#txtusername').val(userName);
                        $('#txtpassword').val(pwd);
                        $('#blockQRLogin').dialog('close');
                        document.getElementById('<%= BtnSubmit.ClientID %>').click();
                    }
                },
                error: function myfunction(request, status) {
                 
                }
            });
        }
    </script>
    </form>
</body>
</html>
