<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterTabletDeskOrSpare.aspx.vb" Inherits="QuickTest.RegisterTabletDeskOrSpare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>คลังข้อสอบออนไลน์ by วัฒนาพานิช</title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
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
           .ForbtnRegister {
            width:300px !important;
            font-size:28px !important;
            font: 100% 'THSarabunNew';
            border: 0 !important;
            padding: 2px 0 3px 0 !important;
            cursor: pointer !important;
            background: #1EC9F4 !important;
            -moz-border-radius: .5em !important;
            -webkit-border-radius: .5em !important;
            border-radius: .5em !important; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF !important;
            border: solid 1px #0D8AA9 !important;
            /*background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);*/
            text-shadow: 1px 1px #178497 !important;
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
        .form_settings input, .form_settings textarea, .setupTestset input {
        width:auto;
        }
        label {
        margin-left:10px;
        }
    </style>

    <script type="text/javascript">
        //$(function () {
        //    $('#RegisterLab').click(function () {
        //        $('#DivRegisterSpare').hide();
        //        $('#DivRegisterLab').show();
        //    });
        //    $('#RegisterSpare').click(function () {
        //        $('#DivRegisterLab').hide();
        //        $('#DivRegisterSpare').show();
        //    });
        //});
        $(function () {
            if ($("#IsVisibleLogo").val() == 1) {
                $("#headerLogo").hide();
                $("#footerwpp").hide();
            }
            $('#BtnShowRegisterLab').click(function () {
                $("#IsVisibleLogo").val(1);
            });
            $('#BtnShowRegisterSpare').click(function () {
                $("#IsVisibleLogo").val(1);
            });
        });
    </script>
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
                //alert(ww);
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
    <form id="form1" runat="server">
      <input type="hidden" value="0" id="IsVisibleLogo" runat="server" />
   <div id="main">
        <header style="height:235px;" id="headerLogo">
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
                                    <asp:Button ID="BtnShowRegisterLab" CssClass="ForbtnRegister"  runat="server" Text="ลงทะเบียนกับห้อง Lab" ClientIDMode="Static" />
                                    <%--<input type="button" class="ForbtnRegister" id="RegisterLab" value="ลงทะเบียนกับห้อง Lab" />--%>
                                </td>
                                <td>
                                    <asp:Button ID="BtnShowRegisterSpare" CssClass="ForbtnRegister"  runat="server" Text="ลงทะเบียนเครื่องสำรอง" ClientIDMode="Static" />
                                    <%--<input type="button" class="ForbtnRegister" id="RegisterSpare" value="ลงทะเบียนเครื่องสำรอง" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblValidate" Visible="false" style="float:none;" runat="server" ForeColor="red" Font-Size="28px" Text="Validate"></asp:Label>
                                </td>
                            </tr>
                           
                            <tr>
                                <td colspan="2">

                                    <asp:Panel ID="DivRegisterLab" Visible="false" runat="server">
                                    <%--<div id="DivRegisterLab" style="display:none;">--%>
                                        <span style="float:none;font-size:28px;font-weight:bold;">ลงทะเบียนแท็บเลตเป็นเครื่องห้องแล็บ</span>
                                        <div>
                                            <span style="float:none;font-size:25px;">ห้องแล็บ</span>
                                                <asp:DropDownList style="font-size:25px;width:auto;" AutoPostBack="true" ID="DDlLabName" runat="server">
                                                </asp:DropDownList>  
                                        </div>
                                        <div>
                                            <asp:RadioButton ID="rdoStudent" Checked="true" style="float:none;font-size:25px;" Text="นักเรียน" GroupName="rdoRegisterLab" runat="server" />
                                            <asp:Label ID="lblStudentAmount" style="float:none;font-size:25px;" runat="server" Text="(มีแล้ว 15 ที่)"></asp:Label>
                                            <asp:RadioButton ID="rdoTeacher" style="float:none;font-size:25px;margin-left:95px;" Text="ครู" GroupName="rdoRegisterLab" runat="server" />
                                            <asp:Label ID="lblTeacherAmount" style="float:none;font-size:25px;" runat="server" Text="(มีแล้ว 1 ที่)"></asp:Label>
                                        </div>
                                        <div>
                                            <% If Request.QueryString("IsOwner").ToString() = "S" Then%>
                                            <span style="float:none;font-size:25px;">หมายเลขโต๊ะ (สำหรับนักเรียนเลขที่)</span>
                                            <asp:TextBox ID="txtDeskNumber" style="width:60px;text-align:center;font-size:25px;background:rgb(124, 124, 124);color:white;" Enabled="false" runat="server">25</asp:TextBox>
                                            <% End If%>
                                        </div>
                                        <asp:Button ID="BtnRegisterLab" CssClass="ForbtnRegister" style="width:140px !important;margin-top:10px !important;" runat="server" Text="ยืนยัน" />
                                    <%--</div>--%>
                                    </asp:Panel>

                                    <asp:Panel ID="DivRegisterSpare" Visible="false" runat="server">
                                     <%--<div id="DivRegisterSpare" style="display:none;">--%>
                                        <span style="float:none;font-size:28px;font-weight:bold;">ลงทะเบียนแท็บเลตเป็นเครื่องสำรอง</span>
                                        <div>
                                            <span style="float:none;font-size:25px;">ชื่อเครื่อง</span>
                                            <asp:TextBox ID="txtTabletName" style="font-size:25px;width:300px;" placeholder="ชื่อเครื่องแท็บเล็ต" runat="server"></asp:TextBox>
                                        </div>
                                        <asp:Button ID="BtnRegisterSpare" CssClass="ForbtnRegister" style="width:140px !important;" runat="server" Text="ยืนยัน" />
                                   <%--</div>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                            
                        </table>
                </div>
            </div>     
        </div>
        <footer id="footerwpp">
      <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
    </footer>
    </div>
    </form>
</body>
</html>
