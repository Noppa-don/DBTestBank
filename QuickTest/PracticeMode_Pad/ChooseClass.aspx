<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ChooseClass.aspx.vb" Inherits="QuickTest.ChooseClass" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../css/jquery.fancybox.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.18.custom.css" />
    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css" />

    <%If IsAndroid = True Then%>
        <style type="text/css">
        #main {
            margin: 20px auto !important;
        }

        #site_content {
            width: 930px !important;
        }

        #MainPanel {
            padding: 0 !important;
            width: 930px !important;
            background-color: transparent !important;
            margin-top: 10px !important;
        }

        #BackButton {
            width: 90px !important;
            height: 90px !important;
        }
        
        nav {
            display: none !important;
        }
    </style>
    <%Else%>
        <style type="text/css">
        #MainPanel {
            margin-top: 10px !important;
        }
    </style>
    <%End If%>

    <%If Not BusinessTablet360.ClsKNSession.IsMaxONet Then %>
        <style type="text/css">
        #site_content {
            border-radius: 10px;
            position: relative;
        }

        nav {
            display: none !important;
        }
    </style>
    <%End If %>

    <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "wordonly" Then %>
        <style type="text/css">
            html {
                background: url(../images/bg/res19201080/3.png) !important;
            }

            body {
                background-image:none !important;
            }
    </style>
    <%End If %>

        <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
        <style type="text/css">
            html {
                background-image:none !important;
            }

            body {
                background: url(../images/bg/res19201080/chooseclass.png) !important;
            }
    </style>
    <%End If %>
    
    <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
        <style type="text/css">
        #site_content {
            height: 380px;
        }        

        .ImgBtnSubject {
            margin-left: 30px !important;
            margin-right: 30px !important;
        }

        span.lblMaxOnet:before {
            content: "(";
        }

        span.lblMaxOnet::after {
            content: ")";
        }

        span.lblMaxOnet {
            color: orange;
            margin-left: 20px;
            font-weight: bold;
        }
    </style>
    <%End If %>

    <style type="text/css">
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

        body {
            font-size: 110% !important;
        }

            body #main span {
                font-size: 2em;
            }

        #site_content {
            position: relative;
            height:auto;
                padding-bottom: 10px!important;
        }

        .divBackBtn {
            top:20px!important;
        }
         .divBackBtn > input {
            width: 55px!important;
            height: 55px!important;
        }
         div.GotoContact{
                width: 70px;
                height: 70px;
                cursor: pointer;
                background-image: url(../images/maxonet/post.png);
                background-size: 70px;
                display: inline-block;
                float: right;
                margin-top: -10px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id='formclass' runat="server">
        <%If IsAndroid = False Then%>
            <nav style='position: relative; top: 50px; width: 650px; height: 50px;'>
                <div id="menu_container">
                    <span id='spn' style="font-size:20px;color:Black;"></span>
                    <ul class="sf-menu" id="nav" stylesheet="text-align:center;" style="font-size:<%=ChkFontSize %>;margin-left:<%=MarginSize %>;width:570px;">
                        <asp:ImageButton ID="imgHome" ImageUrl="~/Images/Home.png" style='float:left;margin-left:-45px;margin-right:15px;margin-top:3px;' runat="server"></asp:ImageButton>
		                <li id="liChooseClass"><a href="#" class="current" style='padding:<%=PaddingSize%>;' ><%=txtStep1 %></a></li>
                        <li id="liChooseSubject"><a href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep2 %></a></li> 
                        <li id="liChooseQuestionset"><a href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep3 %></a></li>	
                        <%If Session("ChooseMode") = 2 Or Session("ChooseMode") = 4 Then%>
                            <li id="liChooseHomework"><a href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep4 %></a></li>	
                        <%End If%>
                    </ul>
                </div>
            </nav>
        <%End If%>

        <div id="main" style="margin: 70px auto;">
            <div id="site_content">
                <div id='MainDiv' runat="server" clientidmode="Static">
                    <div class="GotoContact"></div>
                </div>
            </div>
        </div>

        <div id="dialog" title="ต้องเข้าควิซแล้วค่ะ"></div>

        <div id="dialog2" class="dialogSession"></div>
    </form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <telerik:RadCodeBlock ID='RadCodeblock1' runat="server">
        <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
        <script type="text/javascript" src="../js/GFB.js"></script>
        <script type="text/javascript" src="../js/jquery.fancybox.js"></script>
        <script type="text/javascript" src="../js/jquery-ui-1.8.18.js"></script>
        <script type="text/javascript" src="../js/jquery.qtip-1.0.0-rc3.js"></script>
<%--        <script type="text/javascript" src="../Scripts/jquery.signalR-2.0.2.min.js"></script>
        <script type="text/javascript" src='<%=ResolveClientUrl("~/signalr/hubs") %>'></script>--%>
        <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>
        <script type="text/javascript" src="../js/CheckAndSetNowDevice.js"></script>

        <script type="text/javascript">
            Shadowbox.init({
                skipSetup: false
            });

            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;

            //ปุ่มเลือกห้อง
            $('.ImgbtnCls').each(function () {
                new FastButton(this, TriggerServerButton);
            });
            $('.Forbtn').each(function () {
                new FastButton(this, TriggerServerButton);
            });

            //ปุ่ม กลับ
            new FastButton(document.getElementById('BackButton'), TriggerServerButton);

            $(function () {
                //hover animation
                InjectionHover('.ImgbtnCls', 5);
                InjectionHover('#BackButton', 5, false);

                <% If Session("ChooseMode") = 2 Or Session("ChooseMode") = 4 Then%>
                $('#liChooseClass').css('width', '125px');
                $('#liChooseSubject').css('width', '130px');
                $('#liChooseQuestionset').css('width', '155px');
                $('#liChooseHomework').css('width', '135px');
                <%Else%>
                $('#liChooseClass').css('width', '145px');
                $('#liChooseSubject').css('width', '150px');
                $('#liChooseQuestionset').css('width', '145px');
                <% End If%>

                if (isAndroid) {
                    var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                    var mw = 480; // min width of site
                    var ratio = ww / mw; //calculate ratio
                    if (ww < mw) { //smaller than minimum size
                        $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                    } else { //regular size
                        $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                    }

                    //$('#site_content').css('width', '1200px');
                    //$('#MainPanel').css({ 'padding-left': '25px', 'padding-right': '25px', 'margin-top': '23px', 'height': '580px' });
                    //$('#BackButton').css({ 'top': '10px', 'font-size': '30px', 'width': '200px', 'height': '80px', 'margin-left': '-82%' });
                    //$('nav').hide();
                    //$('nav').css('top', '40px');

                }
                else {
                    //page transition
                    $('.ImgbtnCls').click(function () {
                        FadePageTransitionOut();
                    });
                }

                $('.GotoContact').click(function () {
                    GotoContact();
                });
            });

            function GotoContact() {
                    Shadowbox.open({
                        content: '<div style="overflow: hidden;white-space: nowrap; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="<%=ResolveUrl("~")%>Contact.aspx" frameborder="0" width="700" height="570"></iframe></div>',
                        player: "html",
                        title: "ติดต่อเรา",
                        height: 570,
                        width: 700
                    });
            };

            //function TriggerClick(e) {
            //    var obj = e.target;
            //    $(obj).trigger('click');
            //}
        </script>
        <%If Not IE = "1" Then%>
        <%If Not Session("selectedSession") = "PracticeFromComputer" Then%>
        <%If Session("IsTeacher") = "1" Then%>
<%--        <script src='../js/DashboardSignalR.js' type="text/javascript"></script>--%>
        <%Else%>
        <script type="text/javascript">            var IsPracticeFromComputer = '<%=Session("PracticeFromComputer") %>'; if (IsPracticeFromComputer == "False") { var GroupName = "<%=GroupName %>"; var DeviceId = '<%=DVID %>'; }</script>
<%--        <script src="../js/StudentSignalR.js" type="text/javascript"></script>--%>
        <%End If%>
        <%End If%>
        <%End If%>
        <script type="text/javascript">
            $(function () {
                if ($.browser.msie && $.browser.version == 9.0) {
                    $('#BackButton').css({ 'margin-left': '-50%' });
                }
            });
        </script>
        <script type="text/javascript">
            var NeedShowTip = '<%=NeedShowTip%>';
            $(function () {
                if (NeedShowTip == 'True') {
                    ShowTips();
                }
            });

            function ShowTips() {
                var elm = ['#ctl00_MainContent_SecondPanel', '#Help'];
                var tipPosition = ['topMiddle', 'leftMiddle'];
                var tipTarget = ['bottomMiddle', 'rightMiddle'];
                var tipContent = ['เลือกชั้นเพื่อดูข้อสอบ', 'ศึกษาเพิ่มเติมดูที่นี่ค่ะ'];
                var tipAjust = [0, -50];
                for (var i = 0; i < elm.length; i++) {
                    $(elm[i]).qtip({
                        content: tipContent[i],
                        show: { ready: true },
                        style: {
                            width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
                        }, hide: false,
                        position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { x: tipAjust[i] } },
                        fixed: false
                    });
                }
                DestroyTips(elm);
            }
            function DestroyTips(elm) {
                setTimeout(function () {
                    for (var i = 0; i < elm.length; i++) {
                        $(elm[i]).qtip('destroy');
                    }
                }, 5000);
            }
        </script>

    </telerik:RadCodeBlock>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server"></asp:Content>