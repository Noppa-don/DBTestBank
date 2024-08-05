<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="HomewokNowAndHistoryPage.aspx.vb"
    Inherits="QuickTest.HomewokNowAndHistoryPage" %>

<%@ Register Src="~/UserControl/SelectTermControl.ascx" TagName="UserControl" TagPrefix="myTerm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //ดักถ้าเข้าด้วย Tablet ของครู
            if (isAndroid) {
                $('.ForMainDiv').css('width', '850px');
                $('#site_content').css('width', '850px');
            }
        });
    </script>
    <script type="text/javascript">
        // Css for IE
        $(function () {
            if ($.browser.msie && $.browser.version < 9.0) {
                $('div.showMenuFixedBar').css({ 'right': '-38px' });
            }
            if ($.browser.msie && $.browser.version <= 9.0) {
                $('#txtSearchHomework').placeholder();
            }
        });

        function HandlerEnterKey(e) {
            if (e.keyCode === 13) { e.preventDefault(); };
            return false
        }
    </script>
    <style type="text/css">
        .ForMainDiv {
            margin: 20px auto;
            width: 984px;
            background: #FFF;
            padding-bottom: 15px;
            -webkit-border-radius: 15px;
            -moz-border-radius: 15px;
            border-radius: 15px;
            behavior: url('../css/PIE.htc');
        }

        .ForDivWidth {
            width: 249px;
        }

        .ForDivHomework {
            float: left;
            height: 190px;
            border: 1px solid #EBEBEB;
            border-radius: 3px;
            margin: 5px 5px 5px 5px;
        }

        .ForDivTop {
            height: 80%;
            position: relative;
        }

        .ForDivBottom {
            height: 20%;
            border-top: 1px solid #EBEBEB;
            position: relative;
            background-color: #FFA032;
            border-bottom-left-radius: 3px;
            border-bottom-right-radius: 3px;
            line-height: 40px;
        }

        .ForDivRightPanel {
            width: 70px;
            line-height: 32px;
            position: absolute;
            top: -1px;
            right: -1px;
        }

        .ForSmallDivRightPanel {
            border: 1px solid #EBEBEB;
            text-align: center;
            height: 30px;
            background: rgba(255, 160, 50, 0.36);
            border-top-right-radius: 3px;
        }

        .ForSpanTeacherName {
            position: absolute;
            top: 27px;
            left: 5px; /*font-size: 15px;*/
            width: 240px;
            height: 80px;
            height: auto;
            overflow: hidden;
            z-index: 999;
            cursor: pointer;
        }

        .ForSpanEndTime {
            position: absolute;
            bottom: 0px;
            margin-left: 10px;
            font-size: 15px;
        }

        .ForSpanHomeworkName {
            margin-left: 5px;
            position: absolute; /*top:5px;*/
            font-size: 90%;
        }

        .ImgEdit {
            float: right;
            margin-right: 5px;
            cursor: pointer;
            margin-top: 3px;
        }

        .ImgShowInfo {
            position: relative;
            top: 35px;
            left: 66px;
            cursor: pointer;
        }

        .Forbtn {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em;
            color: #444;
            font-weight: bold;
            background: #FFA032;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width: 395px;
            height: 50px;
            font-size: 25px;
        }

        .btnCurrentImg {
            background-image: url('../images/homework/CurrentHomework.png');
            background-repeat:no-repeat;
                background-position-x: 100px;
        }

        .btnHistoryImg {
            background-image: url('../images/homework/HistoryHomework.png');
            background-repeat:no-repeat;
                background-position-x: 100px;
        }

        .ForMainDivNoData {
            width: 750px;
            height: 244px;
            overflow-y: auto;
            margin-left: auto;
            margin-right: auto;
        }

        .ForDivShowInFo {
            text-align: center;
            position: relative;
            top: 15px;
            background-image: url("../images/ChooseRoom.png");
            background-repeat: no-repeat;
            background-position: 50px;
            margin-top: 165px;
        }
    </style>
    <style type="text/css">
        .ui-accordion .ui-accordion-header {
            text-align: center;
            color: #444;
            padding: .5em .5em .5em .7em;
        }

        .ui-accordion .ui-accordion-content {
            height: auto;
            line-height: 16pt;
            padding: 0;
            background-color: White;
            color: #444;
            padding: 0px !important;
        }

        .ui-state-active {
            border: 1px solid yellow;
            background-color: #FFA032 !important;
            color: #444;
        }

            .ui-state-active .ui-icon {
                background-image: initial;
            }

        .menuAcdItem {
            line-height: 50px;
            vertical-align: middle;
            border-bottom: 1px solid;
            padding-left: 40px;
        }

        .msMainMenu {
            height: auto;
            position: relative; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            margin: 5px;
        }

        .menuFixedBar {
            height: 100%;
            width: 240px;
            top: 10px;
            left: 0; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            overflow-y: auto;
            border-radius: 5px;
        }

        span.icon_clear {
            position: absolute;
            right: 10px;
            display: none;
            cursor: pointer;
            font-weight: bold;
            font-size: 20px;
            top: -10px;
            color: Red;
        }

            span.icon_clear:hover {
                color: Blue;
            }

        span.imgFind {
            background-image: url('../images/homework/Search.png');
            display: inline-block;
            width: 25px;
            height: 25px;
            background-size: cover;
            position: absolute;
            top: 0;
            left: 5px;
            -ms-behavior: url('../css/backgroundsize.min.htc');
        }

        #txtSearchHomework {
            width: 120px;
            padding-left: 30px;
            padding-right: 30px;
            height: 30px;
            font: 20px 'THSarabunNew';
            margin: 13px 0 0 0;
        }

        .SearhStudent, .ToggleModeClassName {
            margin: 10px 5px;
            border: 1px solid rgb(235, 235, 235);
            background-color: #FFA032;
            border-radius: 5px;
            height: 62px;
            color: #444;
        }

        .ToggleModeClassName {
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            line-height: 62px;
            cursor: pointer;
        }

        #ctl00_MainContent_ShowStudentSearch, #ctl00_MainContent_ShowStudentSelectedRoom {
            width: 821px;
            border-radius: 5px;
            margin: -79px auto 0 auto;
            padding: 10px 0 0 0;
            overflow-y: auto;
            text-align: center;
            display: none;
            height: 456px;
            line-height: 456px;
        }

            #ctl00_MainContent_ShowStudentSearch :hover {
                background-color: #F68500;
                cursor: pointer;
            }

        .NotSelectedRoom, .NotFoundStudent {
            border: 1px dashed #FFA032 !important;
            line-height: 172px;
            font-size: 25px;
            font-weight: bold;
            color: rgb(122, 119, 119);
        }

        .NotSelectedRoom {
            background-image: url('../images/ChooseRoom.png');
            background-repeat: no-repeat;
            background-position: 50px;
        }

        .StudentSearch {
            border: 2px solid;
        }

        #ctl00_MainContent_ShowStudentSearch > div {
            width: 288px;
            height: 110px;
            border: 1px solid rgb(235, 235, 235);
            border-radius: 5px;
            display: inline-block;
            margin: 20px 35px;
            padding: 5px;
        }

        #ctl00_MainContent_ShowStudentSearch img {
            width: 100px;
            height: 100px;
            margin: 5px 0 0 0;
        }

        #ctl00_MainContent_ShowStudentSearch > div > div {
            float: right;
            width: 180px;
        }

            #ctl00_MainContent_ShowStudentSearch > div > div label {
                font-weight: bold;
                font-size: 22px;
            }

        #menuClassName {
            width: 250px;
            /*background: rgb(255, 255, 203);*/
            height: 625px;
            position: fixed;
            top: 10px;
            z-index: 999999999;
            border-radius: 0 .5em .5em 0;
            box-shadow: #222 2px 2px 2px;
            padding: 10px 0;
            background: #ffc077; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            /*background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIxMDAlIiB5Mj0iMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZmZjJlNCIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmZmMwNzciIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);*/
            background: -moz-linear-gradient(left, #fff2e4 0%, #ffc077 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, right top, color-stop(0%,#fff2e4), color-stop(100%,#ffc077)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* IE10+ */
            background: linear-gradient(to right, #fff2e4 0%,#ffc077 100%); /* W3C */
            /*filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fff2e4', endColorstr='#ffc077',GradientType=1 ); /* IE6-8 */
        }

        .hideClassName {
            left: -240px;
        }

        .showClassName {
            left: 0;
        }

        .showMenuFixedBar {
            width: 40px;
            height: 80px;
            position: absolute;
            right: -35px;
            top: 50%;
            margin-top: -40px;
            background-image: url('../images/dashboard/OpenClass.png');
            background-size: 40px;
            background-repeat: no-repeat;
            cursor: pointer;
            background-position: 50%;
        }

        #ctl00_MainContent_lblClassRoom {
            font-size: 140%;
            color: #F68500;
            padding: 0 30px;
        }
    </style>
    <!--[if gte IE 9]>
        <style type="text/css">
        .gradient{filter:none;}
        </style>
        <![endif]-->
    <script type="text/javascript">
        function SetSesstionCalendarId(CalendarId, CalendarName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetSessionCalendarId",
                data: "{ CalendarId: '" + CalendarId + "',CalendarName: '" + CalendarName + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        CloseFancyBox();
                        window.location.reload();
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
        function CloseFancyBox() {
            $.fancybox.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="FormHowworkNowAndHistory" runat="server">
        <div id="MainDiv" class="ForMainDiv">
            <div id="site_content" style="margin: auto;">
                <div class="content" style="width: 100%; float: initial; padding: 0;">
                    <img id='imgBack' src="../Images/dashboard/Home.png" style='position: absolute; margin-left: 5px; margin-top: 10px; cursor: pointer; width: 80px; height: 60px; top: 25px;' />
                    <myTerm:UserControl ID="MyCtlTerm" runat="server" />
                    <div style="text-align: center;">
                        <asp:Button ID="BtnCurrent" ClientIDMode="Static" CssClass="Forbtn btnCurrentImg" runat="server"
                            Text="ปัจจุบัน" />
                        <asp:Button ID="BtnHistory" ClientIDMode="Static" CssClass="Forbtn btnHistoryImg" runat="server"
                            Text="ประวัติ" />
                    </div>
                    <div id="MainDivHomework" clientidmode="Static" runat="server" style="width: 800px; height: 400px; overflow: auto; margin-left: auto; margin-right: auto; padding: 10px; border: 1px dashed #FFA032; border-radius: 15px; margin-top: 15px;">
                    </div>
                </div>
            </div>
        </div>
        <div id="menuClassName" class="hideClassName">
            <div class="showMenuFixedBar">
            </div>
            <div class="menuFixedBar">
                <div class="SearhStudent" style="margin-top: 0px; text-align: center;">
                    <span style="position: relative;"><span class="imgFind"></span>
                        <input type="text" id="txtSearchHomework" placeholder="ค้นหาการบ้าน" onfocus="SearchHomework();" onkeydown="HandlerEnterKey(event);"
                            onblur="ClearTimeInterval();" />
                        <span class="icon_clear">X</span> </span>
                </div>
                <div class="ToggleModeClassName" id="ModeClassName" runat="server">
                </div>
                <div id='mainMenu' class='msMainMenu'>
                    <div id="ac">
                        <div id="a">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdKeepModeRoom" runat="server" />
        <asp:HiddenField runat="server" ID="IsSelected" Value="" ClientIDMode="Static" />
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.placeholder.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <%If Not IE = "1" Then%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>
    <%End If%>
    <%-- <link href="../css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />--%>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script type="text/javascript">

        var ClassName = '<%=JSClassName %>';
        var IsCurrent = '<%=JSIsCurrent %>';

        $(function () {

            //hover animation
            InjectionHover('#imgBack', 5, false);
            InjectionHover('.ToggleModeClassName', 3);


            //ปุ่ม Back
            new FastButton(document.getElementById('imgBack'), TriggerImgBackClick);

            //ปุ่ม ปัจจุบัน
            if ($('#BtnCurrent').length != 0) {
                new FastButton(document.getElementById('BtnCurrent'), TriggerServerButton);
            }

            //ปุ่ม ประวัติ
            if ($('#BtnHistory').length != 0) {
                new FastButton(document.getElementById('BtnHistory'), TriggerServerButton);
            }

            //ปุ่ม เปิด - ปิด Panel เลือกห้อง
            $('.showMenuFixedBar').each(function () {
                new FastButton(this, TriggerPanelClassRoomClick);
            });

            //ปุ่ม ImgShowInfo
            if ($('.ImgShowInfo').length != 0) {
                $('.ImgShowInfo').each(function () {
                    new FastButton(this, TriigerImgShowInfoClick);
                });
            }

            //ForSpanTeacherName
            if ($('.ForSpanTeacherName').length != 0) {
                $('.ForSpanTeacherName').each(function () {
                    new FastButton(this, TriigerImgShowInfoClick);
                });
            }

            //ปุ่ม ImgEdit
            if ($('.ImgEdit').length != 0) {
                $('.ImgEdit').each(function () {
                    new FastButton(this, TriigerImgEditClick);
                });
            }



            //$('#imgBack').click(function () {
            //    window.location = '<%=ResolveUrl("~")%>Homework/DashboardHomeworkPage.aspx'
            //});

            //$('#Help').hide();
            //$('#navigation').hide();

            //            $('.SR').qtip({
            //                content: 'จำนวนเด็กที่เกี่ยวข้องกับการบ้านนี้',
            //                position: {
            //                    corner: {
            //                        tooltip: 'bottomLeft',
            //                        target: 'topRight'
            //                    }
            //                },
            //                style: {
            //                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
            //                }
            //            });

            //            $('.SN').qtip({
            //                content: 'จำนวนนักเรีนยที่ยังไม่ส่งการบ้าน',
            //                position: {
            //                    corner: {
            //                        tooltip: 'bottomRight',
            //                        target: 'topLeft'
            //                    }
            //                },
            //                style: {
            //                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomRight', name: 'dark', 'font-weight': 'bold'
            //                }
            //            });

            //            $('.RS').qtip({
            //                content: 'จำนวนนักเรียนที่ยังไม่ทำ และ ยังไม่เสร็จ',
            //                position: {
            //                    corner: {
            //                        tooltip: 'bottomLeft',
            //                        target: 'topRight'
            //                    }
            //                },
            //                style: {
            //                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
            //                }
            //            });

            $('.TxtSmallDiv').qtip({
                content: 'จำนวนนักเรียนที่ส่งแล้ว / ทั้งหมด',
                position: {
                    corner: {
                        tooltip: 'bottomLeft',
                        target: 'topRight'
                    }
                },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
                }
            })



            //ปุ่ม Info กดเพื่อไปหน้า UsageScore
            //$('.ImgShowInfo').live('click', (function () {
            //ShowSelectTermPage($(this).attr('MAID'));
            //}));

            //$("#mainMenu").jqMenuStick();
            var html = "<%=htmlClass %>";
            $('#a').replaceWith(html);
            $("#ac").accordion({ autoHeight: false, animate: 0, heightStyle: "content" });
            $("#ac > h3").live('click', function () {
                var c = $(this).prevAll('h3').length + 1;
                var h = (62 * c) + 80;
                //console.log(h);                
                $('.menuFixedBar').scrollTop(h);
            });

            // click icon clear
            $('.icon_clear').click(function () {
                $(this).delay(300).fadeTo(300, 0).prev('#txtSearchHomework').val('');
                GetHomeworkSearch('');
            });

            // selected room
            $('div.menuAcdItem').live('click', function () {
                $('div.menuAcdItem').css('background-color', 'transparent');
                $(this).css('background-color', '#EEFCA6');
                var classRoom = $(this).html();
                SelectedClassRoom(classRoom);
            });

            //Accordion
            if (isAndroid) {
                $('.menuAcdHeadItem').each(function () {
                    new FastButton(this, TriggerAccClick);
                });
                if ($('div.menuAcdItem').length != 0) {
                    $('div.menuAcdItem').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
            }

            // menu click show/hide
            //$('.showMenuFixedBar').click(function () {
            //    var menuClass = $('#menuClassName');
            //    if ($(menuClass).hasClass('hideClassName')) {
            //        $(this).css('background-image', "url('../images/dashboard/CloseClass.png')");
            //        $(menuClass).removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
            //    } else {
            //        $(this).css('background-image', "url('../images/dashboard/OpenClass.png')");
            //        $(menuClass).removeClass('showClassName').addClass('hideClassName', 500, 'easeOutBounce');
            //    }
            //});


            if ($('#IsSelected').val() == 'false') {
                $('.showMenuFixedBar').trigger('click');
            }

            // สลับโหมด ห้อง
            $('#ctl00_MainContent_ModeClassName').click(function () {
                var ModeRoom = $('#ctl00_MainContent_hdKeepModeRoom').val();
                if (ModeRoom == "True") {
                    $('#ctl00_MainContent_hdKeepModeRoom').val('False');
                    GetClassRoomWhenToggleMode('False');
                    $(this).html('ห้องที่สอน');
                } else {
                    $('#ctl00_MainContent_hdKeepModeRoom').val('True');
                    GetClassRoomWhenToggleMode('True');
                    $(this).html('ห้องทั้งหมด');
                }
            });


        });

        function TriggerAccClick(e) {
            var obj = e.target;
            var divTarget = $(obj).next();
            if ($(divTarget).css('display') == 'block') {
                $(divTarget).hide();
            }
            else {
                $('.ui-accordion-content').hide();
                $(divTarget).show();
            }
        }

        function TriggerImgBackClick() {
            if (!isAndroid) {
                FadePageTransitionOut();
            }
            window.location = '<%=ResolveUrl("~")%>Homework/DashboardHomeworkPage.aspx'
        }

        //function TriggerClick(e) {
        //    var obj = e.target;
        //    $(obj).trigger('click');
        //}

        function TriggerPanelClassRoomClick(e) {
            var obj = e.target;
            var menuClass = $('#menuClassName');
            if ($(menuClass).hasClass('hideClassName')) {
                $(obj).css('background-image', "url('../images/dashboard/CloseClass.png')");
                $(menuClass).removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
            } else {
                $(obj).css('background-image', "url('../images/dashboard/OpenClass.png')");
                $(menuClass).removeClass('showClassName').addClass('hideClassName', 500, 'easeOutBounce');
            }
        }

        function TriigerImgShowInfoClick(e) {
            var obj = e.target;
            ShowSelectTermPage($(obj).attr('MAID'));
        }

        function TriigerImgEditClick(e) {
            var obj = e.target;
            var maid = $(obj).attr('maid');
            EditHomework(maid);
        }

        function ShowSelectTermPage(MAID) {
            if (isAndroid) {
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    //'href': '../Student/SelectTermPage.aspx',
                    'href': '../Student/UsageScore.aspx?MA_Id=' + MAID,
                    'type': 'iframe',
                    'width': 750,
                    'minHeight': 450
                });
            }
            else {
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    //'href': '../Student/SelectTermPage.aspx',
                    'href': '../Student/UsageScore.aspx?MA_Id=' + MAID,
                    'type': 'iframe',
                    'width': 864,
                    'minHeight': 540
                });
            }

        };

        // Get รายชื่อห้อง เมื่อสลับโหมด
        function GetClassRoomWhenToggleMode(IsSelectedClassInSchool) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Student/HomewokNowAndHistoryPage.aspx/SetMenuClassRoom",
                data: "{ IsSelectedClassInSchool : '" + IsSelectedClassInSchool + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        $('#ac').html(msg.d);
                        $("#ac").accordion('destroy').accordion({ autoHeight: false, animate: 0, heightStyle: "content" });
                    }
                },
                error: function myfunction() {
                    alert('jeng');
                }
            });
        }


        function SelectedClassRoom(InputClass) {
            window.location = '<%=ResolveUrl("~")%>Student/HomewokNowAndHistoryPage.aspx?IsCurrent=True&ClassName=' + InputClass;
        }

        //ปุ่มดินสอที่ไว้กดแก้ไข
        function EditHomework(MAID) {
            if (isAndroid) {
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>Module/EditHomeworkPage.aspx?MAID=' + MAID,
                    'type': 'iframe',
                    'width': 750,
                    'minHeight': 450
                });
            }
            else {
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>Module/EditHomeworkPage.aspx?MAID=' + MAID,
                        'type': 'iframe',
                        'width': 864,
                        'minHeight': 540
                    });
                }
            }

            function CloseFancyBoxEditHomework() {
                $.fancybox.close();
                window.location = '<%=ResolveUrl("~")%>Student/HomewokNowAndHistoryPage.aspx?IsCurrent=' + IsCurrent + '&ClassName=' + ClassName;
        }

        function GotoHomeworkAssignmentPage(MAID, Isnew) {
            window.location = '<%=ResolveUrl("~")%>Module/HomeworkAssignPage.aspx?MAId=' + MAID + '&PageName=../Student/HomewokNowAndHistoryPage.aspx&IsNew=' + Isnew + '&IsCurrent=' + IsCurrent + '&ClassName=' + ClassName;
        }

        //Search Homework
        var delayID = null;
        function SearchHomework() {
            var prevValue = '';
            //var txtSearhHomework = $('#txtSearhHomework').val();
            //txtSearhHomework = txtSearhHomework.toLowerCase();
            if (delayID != null) {
                clearInterval(delayID);
            }
            delayID = setInterval(function () {
                var t = $('#txtSearchHomework').val();
                if (prevValue != t) {
                    GetHomeworkSearch(t.toLowerCase());
                    prevValue = t;
                }
                if (prevValue == '') {
                    $('.icon_clear').delay(300).fadeTo(300, 0);
                }
                else {
                    $('.icon_clear').stop().fadeTo(300, 1);
                }
            }, 2000);
        }

        // clear time delayID
        function ClearTimeInterval() {
            clearInterval(delayID);
        }

        function GetHomeworkSearch(txtSearch) {
            if (txtSearch == "") {
                $('#MainDivHomework').html('<div id="DivNoData" class="ForMainDivNoData"><div id="DivShowInfo" class="ForDivShowInFo" style="background-image: url("../images/ChooseRoom.png");background-repeat: no-repeat;background-position: 50px;"><span style="font-size: 25px;font-weight: bold;color: rgb(122, 119, 119);">เลือกห้องก่อนค่ะ</span></div></div>');
            } else {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Student/HomewokNowAndHistoryPage.aspx/SearchHomework",
                    data: "{ txtSearchHomework : '" + txtSearch + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            $('#MainDivHomework').html(msg.d);
                        }
                    },
                    error: function myfunction() {
                        alert('jeng');
                    }
                });
                $('#BtnHistory').hide();
                $('#BtnCurrent').hide();
            }
        }

    </script>
</asp:Content>
