<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainPractice.aspx.vb" Inherits="QuickTest.MainPractice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">  
    
    <meta name="description" content="QR Code scanner" />
    <meta name="keywords" content="qrcode,qr code,scanner,barcode,javascript" />
    <meta name="language" content="English" />
    <meta name="copyright" content="Lazar Laszlo (c) 2011" />
    <meta name="Revisit-After" content="1 Days"/>
    <meta name="robots" content="index, follow"/>
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    
    <script type="text/javascript" src="../js/llqrcode.js"></script>
    <script type="text/javascript" src="../js/plusone.js"></script>
    <script type="text/javascript" src="../js/webqr.js"></script>  
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />

    <title></title>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');

        $(function () {
            //hover animation
            InjectionHover('#BtnBack', 3);
            InjectionHover('#tdMainPractice', 5);
            InjectionHover('#tdScanQR', 5);
            InjectionHover('#btnBackToLogin', 5);
            
            
            //page transition
            if (!isAndroid) {
                FadePageTransition();
                $(':submit').click(function () {
                    FadePageTransitionOut();
                });
                $('tr').click(function () {
                    FadePageTransitionOut();
                });
            }

            $('#QRCode').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                width: 'auto'
            });

            $('#tdScanQR').click(function () {
                $('#Alert').text('ไม่สามารถอ่าน QR ได้ ลองอีกครั้งนะคะ');
                $('#Alert').hide();
                $('#QRCode').dialog('open').dialog('option', 'title', 'สแกนคิวอาร์โค๊ดเพื่อบันทึกประวัติฝึกฝน');
                load();
            });
        });       
       
        function ResultOK(UserData) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/MainPractice.aspx/CheckUserFromReadQR",
                data: "{ QRUserData : '" + UserData + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var a = msg.d;
                    if (a == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                            function ()
                            {
                                $('#QRCode').dialog('close');
                            },5000);
 
                    } else {
                        window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=0&DashboardMode=6';
                    }

                },
                error: function myfunction(request, status) {
                 
                }
                 });
        }
    </script>
<%--    QRCode--%>
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
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/MainPractice.aspx/CheckUser",
                data: "{ StrBase64 : '" + oFREvent.target.result + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    var ar = msg.d;
                    if (ar == '0') {
                        $('#Alert').text('QR นี้ไม่สามารถใช้งานได้ ติดต่อเจ้าหน้าที่ศูนย์คอมนะคะ');
                        $('#Alert').show();
                        setTimeout(
                           function () {
                               $('#QRCode').dialog('close');
                           }, 5000);

                    } else {
                        window.location = '../PracticeMode_Pad/ChooseClass.aspx?UseComputer=0&DashboardMode=6';
                        //window.location.replace("../PracticeMode_Pad/ChooseClass.aspx?UseComputer=1&DashboardMode=6'");
                    }
                    
                },
                error: function myfunction(request, status) {
                    alert('error');
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
    </script>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        html {
            height:100%;
        }
        body {           
            font: normal 0.95em 'THSarabunNew';            
            color: #444;                      
        }

        #main {
            margin: 20px auto;
            width: 954px;
            background: #FFF;
            padding-bottom: 15px;
            border-radius: 15px;
        }

        #site_content {
            width: 930px;
            overflow: hidden;
            margin: 0px auto 0 auto;
            padding: 15px 0 15px 0;
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

        footer {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 930px;
            font: 150% 'THSarabunNew';
            text-shadow: 1px 1px #7E4D0E;
            height: 20px;
            padding: 5px 0 20px 0;
            text-align: center;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #DA7C0C;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            background: #faa51a; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* IE10+ */
            background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 ); /* IE6-8 */
        }

            footer a {
                color: #FFF;
                text-decoration: none;
            }

                footer a:hover {
                    color: #444;
                    text-shadow: none;
                    text-decoration: none;
                }


        .submit {
            font: 110% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em;
            behavior: url(border-radius.htc);
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            /*background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top, #63CFDF, #17B2D9);*/
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        div.form_settings {
            margin: 30px auto;
            width: 830px;
        }

        table {
            width: 100%;
        }

            table tr td {
                background: #D3F2F7;
                color: #47433F;
                border-bottom: 1px solid #FFF;
                padding: 10px !important;
                font-size: 40px;
                text-align: center !important;
                border-radius: 6px;
            }

            table tr {
                cursor: pointer;
            }

        .submit {
            font-size: 25px;
            height: 55px;
            margin-bottom: 30px;
            background: -webkit-gradient(linear, left top, left bottom, from(#EBEEEE), to(#D5D9DA));
            border: solid 1px #707070;
            text-shadow: 1px 1px #929292;
            background: #ebeeee; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ViZWVlZSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNkNWQ5ZGEiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #ebeeee 0%, #d5d9da 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ebeeee), color-stop(100%,#d5d9da)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #ebeeee 0%,#d5d9da 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #ebeeee 0%,#d5d9da 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #ebeeee 0%,#d5d9da 100%); /* IE10+ */
            background: linear-gradient(to bottom, #ebeeee 0%,#d5d9da 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ebeeee', endColorstr='#d5d9da',GradientType=0 ); /* IE6-8 */
        }
          .Forbtn {
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
    border-right: 1px solid #333333;
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
</style>

</head>
<body>
    <%If Not BusinessTablet360.ClsKNSession.IsMaxONet Then %>
    <script src="../js/bg.js" type="text/javascript"></script>
    <%End If %>
    <form id="form1" runat="server">
        <div id="main">
            <div id="site_content">
                <div class="content" style="text-align: center;">
                    <img src="../Images/LogoPractice.png" />
                    <div class="form_settings" style='width: 830px;'>
                        <table>
                            <tr>
                                <td id="tdMainPractice" onclick="window.location.href='../PracticeMode_Pad/ChooseClass.aspx?UseComputer=1&DashboardMode=6';">เข้าทบทวนตามบทเรียน</td>
                                <td id="tdScanQR">สแกนบัตรนักเรียน (QR)</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="BtnBack" runat="server" Width="180px" Text="กลับหน้าแรก" class="Forbtn" Visible="false" />
                    <asp:ImageButton ID="btnBackToLogin" runat="server" ImageUrl="~/images/btnBackToLogin.png" />
                    
                </div>
            </div>
            <div id='QRCode' title='scanner' ">
                <div>
                    <div id="Div1">
                        <tr>
                            <td>
                                <div id="header" style="display:none;">
                                    <div style="position:relative;top:+20px;left:0px;"><g:plusone size="medium"></g:plusone></div>
                                    <canvas id="Canvas1" width="300" height="300" style="margin-left:105px"></canvas>
                                    <div id="result"></div>
                                </div>
                                <div id="mainbody"></div>
                                <canvas id="qr-canvas" width="800" height="600"></canvas>
                                <div id="outdiv"></div>
                            </td>
                        </tr>
                        <tr>
                            <div id="SpanAlert" style="margin-top: 10px;text-align: center; ">
                                <span id="Alert" style="color: red; display:none;">ไม่สามารถอ่าน QR ได้ ลองอีกครั้งนะคะ</span>
                            </div>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <input id="btnSelectFile" value="ภาพถ่าย" class="Forbtn" type="button" />
                                    <input id="filePic" type="file" name="image" style="display:none;" accept="image/*"> 	
                                    <img id="fotoImg" style="visibility: hidden;width:100px;height:100px; display:none;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><img class="selector" id="webcamimg" src="vid.png" onclick="setwebcam()" align="left" style="display:none;" /></td>
                            <td><img class="selector" id="qrimg" src="cam.png" onclick="setimg()" align="right" style="display:none;" /></td>
                        </tr>
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
