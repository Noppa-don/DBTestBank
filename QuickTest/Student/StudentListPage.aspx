<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master"
    CodeBehind="StudentListPage.aspx.vb" Inherits="QuickTest.StudentListPage" %>

<%@ Register Src="~/UserControl/StudentListControl.ascx" TagName="UserControl" TagPrefix="myStudentList" %>
<%@ Register Src="~/UserControl/SelectTermControl.ascx" TagName="UserControl" TagPrefix="myTerm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link href="../css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />     --%>
    <link href="../css/iconFavorite.css" rel="stylesheet" type="text/css" />
  
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
        }

        .ui-state-active {
            border: 1px solid yellow;
            background-color: #FFA032;
            color: #444;
        }
        .ui-state-active .ui-icon{
            background-image:initial;
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
            background-image: url('../images/Search.png');
            display: inline-block;
            width: 25px;
            height: 25px;
            background-size: cover;
            position: absolute;
            top: -5px;
            left: 5px;
            -ms-behavior: url('../css/backgroundsize.min.htc');
        }

        #txtSearhStudent {
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
            border-color: orange;
            width: 821px;
            border-radius: 5px;
            margin: 0px auto 0 auto;
            padding: 10px 0 0 0;
            overflow-y: auto;
            text-align: center;
            display: none;
            height: 450px;
            /*line-height: 456px*/
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
            height: 130px;
            border: 1px solid rgb(235, 235, 235);
            border-radius: 5px;
            display: inline-block;
            margin: 20px 35px;
            padding: 5px;
            overflow: hidden;
        }

        #ctl00_MainContent_ShowStudentSearch img {
            width: 100px;
            height: 120px;
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
        /* #ctl00_MainContent_ShowStudentSearch .Left, #ctl00_MainContent_ShowStudentSearch .Middle
        {
            margin: 0 5px 5px 0;
        }
        */
        #menuClassName {
            width: 250px;
            height: 625px;
            position: fixed;
            top: 10px;
            z-index: 999999999;
            border-radius: 0 .5em .5em 0;
            box-shadow: #222 2px 2px 2px;
            padding: 10px 0;
            /*background: -webkit-gradient(linear, left top, right top, from(rgb(255, 242, 228)), to(rgb(255, 192, 119)));*/
            background: #ffc077;
            /* background: #fff2e4;  Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            /*background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIxMDAlIiB5Mj0iMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZmZjJlNCIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmZmMwNzciIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);*/
            background: -moz-linear-gradient(left, #fff2e4 0%, #ffc077 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, right top, color-stop(0%,#fff2e4), color-stop(100%,#ffc077)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(left, #fff2e4 0%,#ffc077 100%); /* IE10+ */
            background: linear-gradient(to right, #fff2e4 0%,#ffc077 100%); /* W3C */
            /*filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#fff2e4', endColorstr='#ffc077',GradientType=1 );  IE6-8 */
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
    <style type="text/css">
        .DivWidth {
            width: 179px;
        }

        .DivCover {
            border: 1px solid rgb(235, 235, 235);
            border-radius: 5px;
            height: 190px;
            display: inline-block;
            margin: 0 10px 20px 10px;
            position: relative;
        }

        .DivPicture {
            height: 80%;
            /*background-image: url('../Images/default-profile-image.png');*/
            background-size: cover;
            border-radius: 5px 5px 0 0;
            position:relative;
        }

        .DivName {
            height: 19.5%;
            line-height: 38px;
            text-align: center;
            background-color: #FFA032;
            border-top: 1px solid rgb(235, 235, 235);
            border-radius: 0 0 5px 5px;
            overflow: hidden;
            position: absolute;
        }

        .ForDivShowInFo {
            text-align: center;
            position: relative;
            top: 15px;
        }

        .ForDeleteFavorite, .ForGrayStar, .ForYellowStar {
            /*float: left;
            width: 40px;
            height: 40px;
            position: relative;
            bottom: -110px;*/
            cursor: pointer;
            z-index: 90;
        }

        .ForMainDivHaveDataRoom {
            width: 100%;
            height: auto;
            border-radius: 3px;
        }

        .ForMainDivHaveDataTeacher {
            width: 100%;
            height: auto; /*overflow-y: auto;*/
        }

        .ForMainDivNoData {
            width: 100%;
            border: 1px dashed;
        }

        .ForSmallDivRight {
            border-left: 1px solid rgb(235, 235, 235);
        }

        .StandardForSmallDiv {
            width: 45px;
            height: 38px;
            border-bottom: 1px solid rgb(235, 235, 235);
            text-align: center;
            line-height: 40px;
            background-color: orange;
            color: black;
        }

        div.StandardForSmallDiv:last-child {
            border-radius: 5px 0 0 0;
        }

        .ForSmallDivLeft {
            border-right: 1px solid rgb(235, 235, 235);
        }

        .ForLeftPanel {
            position: absolute; /*top:-35px;*/
            width: 45px;
            height: 95px;
            top: 0;
        }

        .ForRightPanel {
            padding-left: 55px;
            font-weight: bold;
            font-size: 23px;
            background-color: rgba(255, 215, 168, 0.74);
            color: black;
            height: 38px;
            line-height: 50px;
            border-radius: 5px 5px 0 0;
            text-align:left;
        }
    </style>
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.placeholder.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(document).ready(function () {
            $('#navigation').remove();
            $('#Chat').remove();

            if ($.browser.msie && $.browser.version <= 9.0) {
                $('#txtSearhStudent').placeholder();
            }
        });

        $(function () {

            //hover animation
            InjectionHover('.ToggleModeClassName', 3);
            InjectionHover('.imghome', 5, false);


            //DivPicture
            $('.DivPicture').each(function () {
                new FastButton(this, TriggerDivPictureClick);
            });

            //showMenuFixedBar Panel เลือกห้องช้างๆ
            $('.showMenuFixedBar').each(function () {
                new FastButton(this, TriggerPanelSelectClassRoom);
            });



            if (isAndroid) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }
            }
            else {

            }


            // เอาเมนู dashboard ออก
            $('.ulTopMenu').remove();
            $('#ExtraMenu').remove();

            var html = "<%=htmlClass %>";
            $('#a').replaceWith(html);
            $("#ac").accordion({
                autoHeight: false, animate: 0, heightStyle: "content"
            });
            $("#ac > h3").live('click', function () {
                var c = $(this).prevAll('h3').length + 1;
                var h = (62 * c) + 80;
                //console.log(h);                
                $('.menuFixedBar').scrollTop(h);
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

            // click icon clear
            $('.icon_clear').click(function () {
                $(this).delay(300).fadeTo(300, 0).prev('#txtSearhStudent').val('');
                GetStudentSearch('');
            });

            // selected room
            $('div.menuAcdItem').live('click', function () {
                $('div.menuAcdItem').css('background-color', 'transparent');
                $(this).css('background-color', '#EEFCA6');
                var classRoom = $(this).html();
                $('#lblClassRoom').text(classRoom);
                SelectedClassRoom(classRoom);
            });

            // menu click show/hide
            //$('.showMenuFixedBar').click(function () {
            //var menuClass = $('#menuClassName');
            //if ($(menuClass).hasClass('hideClassName')) {
            //    $(this).css('background-image', "url('../images/dashboard/CloseClass.png')");
            //    $(menuClass).removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
            //} else {
            //    $(this).css('background-image', "url('../images/dashboard/OpenClass.png')");
            //    $(menuClass).removeClass('showClassName').addClass('hideClassName', 500, 'easeOutBounce');
            //}
            //});

            //auto click เมื่อยังไม่ได้เลือกห้อง
            if ($('#ctl00_MainContent_ShowStudentSearch').hasClass('NotSelectedRoom')) {
                //$('.showMenuFixedBar').trigger('click');
                //var menuClass = $('#menuClassName');
                $('.showMenuFixedBar').css('background-image', "url('../images/dashboard/CloseClass.png')");
                $('#menuClassName').removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
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
            $('.HTD').qtip({
                content: 'จำนวน "การบ้าน" ที่ทำในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
                position: {
                    corner: {
                        tooltip: 'bottomLeft',
                        target: 'topRight'
                    }
                },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
                }
            });

            $('.QTD').qtip({
                content: 'จำนวน "ควิซ" ที่ทำในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
                position: {
                    corner: {
                        tooltip: 'bottomRight',
                        target: 'topLeft'
                    }
                },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomRight', name: 'dark', 'font-weight': 'bold'
                }
            });

            $('.PTD').qtip({
                content: 'จำนวนการ "ฝึกฝน" ในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
                position: {
                    corner: {
                        tooltip: 'bottomLeft',
                        target: 'topRight'
                    }
                },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
                }
            });

           

            $('.HWALL').qtip({
                content: 'จำนวนการบ้านที่ยังไม่ส่ง',
                position: {
                    corner: {
                        tooltip: 'bottomRight',
                        target: 'topLeft'
                    }
                },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomRight', name: 'dark', 'font-weight': 'bold'
                }
            });
                  
           

            ShowQTipHomeworkOn24Hour();
        });

        function ShowQTipHomeworkOn24Hour(){
            $('.HWTD').qtip({
                content: 'จำนวนการบ้านที่ยังไม่ส่งในวันนี้ (ถึงกำหนดภายใน 24ชม.)',
                position: {
                    corner: {
                        tooltip: 'bottomLeft',
                        target: 'topRight'
                    }
                },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold'
                }
            });
        }

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

        // Get รายชื่อห้อง เมื่อสลับโหมด
        function GetClassRoomWhenToggleMode(IsSelectedClassInSchool) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Student/StudentListPage.aspx/SetMenuClassRoom",
                data: "{ IsSelectedClassInSchool : '" + IsSelectedClassInSchool + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        $('#ac').html(msg.d);
                        $("#ac").accordion('destroy').accordion({
                            autoHeight: false, animate: 0, heightStyle: "content"
                        });
                    }
                },
                error: function myfunction() {
                    //alert('jeng');
                }
            });
        }

        // เลือก เทอม
        function SetSesstionCalendarId(CalendarId, CalendarName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetSessionCalendarId",
                data: "{ CalendarId: '" + CalendarId + "',CalendarName: '" + CalendarName + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        CloseFancyBox();
                        var classroom = $('#lblClassRoom').text();
                        var para = classroom == '' ? '' : '?SelectedClassRoom=' + classroom;
                        window.location = '<%=ResolveUrl("~")%>Student/StudentListPage.aspx' + para;
                        //window.location.reload();
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
        function CloseFancyBox() {
            $.fancybox.close();
        }
        // search แบบมี delay
        var delayID = null;
        function SearchStudent() {
            var prevValue = '';
            if (delayID != null) {
                clearInterval(delayID);
            }
            delayID = setInterval(function () {
                var t = $('#txtSearhStudent').val();
                if (prevValue != t) {
                    GetStudentSearch(t.toLowerCase());
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

        // Get Student From txtSearch
        function GetStudentSearch(txtSearch) {
            var ShowStudentSearch = $('#ctl00_MainContent_ShowStudentSearch');
            var ShowStudentSelectedRoom = $('#ctl00_MainContent_ShowStudentSelectedRoom');
            // show / hide div ก่อน
            $('#ctl00_MainContent_lblClassRoom').text('');
            $(ShowStudentSearch).show();
            $(ShowStudentSelectedRoom).hide();
            // search student
            if (txtSearch == "") {
                $('#headClassName').show();
                $(ShowStudentSearch).removeClass('StudentSearch').removeClass('NotFoundStudent').addClass('NotSelectedRoom');
                $(ShowStudentSearch).html('เลือกห้องก่อนค่ะ');
            } else {
                $('#headClassName').hide();
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Student/StudentListPage.aspx/GetStudentSearch",
                    data: "{ txtSearch : '" + txtSearch + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            if (msg.d == "NotHave") {
                                $(ShowStudentSearch).removeClass('StudentSearch').removeClass('NotSelectedRoom').addClass('NotFoundStudent');
                                msg.d = 'ไม่พบนักเรียนคนนี้ค่ะ'
                            } else {
                                $(ShowStudentSearch).removeClass('NotSelectedRoom').removeClass('NotFoundStudent').addClass('StudentSearch');
                            }
                            $(ShowStudentSearch).html(msg.d);
                        }
                    },
                    error: function myfunction() {
                        //alert('jeng');
                    }
                });
            }
        }

        // clear time delayID
        function ClearTimeInterval() {
            clearInterval(delayID);
        }

        // click เลือกห้อง
        function SelectedClassRoom(ClassRoom) {
            $('#headClassName').show();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Student/StudentListPage.aspx/ReloadUserControl",
                data: "{ ClassRoom : '" + ClassRoom + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        $('#ctl00_MainContent_lblClassRoom').text(ClassRoom);
                        var ShowStudentSearch = $('#ctl00_MainContent_ShowStudentSearch');
                        var ShowStudentSelectedRoom = $('#ctl00_MainContent_ShowStudentSelectedRoom');
                        $(ShowStudentSearch).hide();
                        $(ShowStudentSelectedRoom).show();
                        $(ShowStudentSelectedRoom).removeClass('NotSelectedRoom');
                        if (msg.d == 'True') {
                            msg.d = 'ห้องนี้ยังไม่มีนักเรียนค่ะ';
                            $(ShowStudentSelectedRoom).addClass('NotSelectedRoom');
                        }
                        $(ShowStudentSelectedRoom).html(msg.d);
                        //DivPicture
                        $('.DivPicture').each(function () {
                            new FastButton(this, TriggerDivPictureClick);
                        });
                        InitialIconFavorite();
                        ShowQTipHomeworkOn24Hour();
                    }
                },
                error: function myfunction() {
                    //alert('jeng');
                }
            });
        }

        // click student
        function SelectedStudent(StudentId) {
            //alert(StudentId);
            window.location = '<%=ResolveUrl("~")%>Teacher/TeacherStudentDetailPage.aspx?StudentId=' + StudentId;
        }

        function TriggerDivPictureClick(e) {
            var obj = e.target;
            var stuId = $(obj).attr('stuid');
            console.log(stuId);
            if ($(obj).is('.DivPicture')) {
                $(obj).css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
            } else if ($(obj).parent().is('.DivPicture')) {
                stuId = $(obj).parent('.DivPicture').attr('stuid');
                $(obj).parent('.DivPicture').css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
            } else {
                stuId = $(obj).parent().parent('.DivPicture').attr('stuid');
                $(obj).parent().parent('.DivPicture').css('background-image', "url('MonsterID.axd?seed=" + stuId + "')");
            }

            GotoTeacherStudentDetailPage(stuId, e, this);
        }

        function TriggerPanelSelectClassRoom(e) {
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

        // Script สำหรับ Usercontrol

        /////////////////////Function กดที่รูปเด็กเพื่อไปหน้า TeacherStudentDetailPage.aspx
        function GotoTeacherStudentDetailPage(StudentId, e, t) {
            var a = '';
            if ($.browser.msie && $.browser.version < 9.0) {
                a = e.srcElement.getAttribute('class');
                a = a != null ? a : 'DivPicture';
            }

            if ($(e.target).is('.ForDeleteFavorite') || a == 'ForDeleteFavorite') {
                //alert($(e.target).attr('stid'));  
                if ($.browser.msie && $.browser.version < 9.0) {
                    DeleteFavorite(e, t);
                } else {
                    DeleteFavorite($(e.target), t);
                }
            }
            else if ($(e.target).is('.ForGrayStar') || a.indexOf('ForGrayStar') != -1) {
                if ($.browser.msie && $.browser.version < 9.0) {

                   // AddOrRemoveStudentFavorite(e, true);
                } else {
                  //  AddOrRemoveStudentFavorite($(e.target), true);
                }
            }
            else if ($(e.target).is('.ForYellowStar') || a.indexOf('ForYellowStar') != -1) {
                if ($.browser.msie && $.browser.version < 9.0) {
                  //  AddOrRemoveStudentFavorite(e, false);
                } else {
                   // AddOrRemoveStudentFavorite($(e.target), false);
                }
            }
            else if ($(e.target).is('.DivPicture') || a.indexOf('DivPicture') != -1) {
                console.log(1);
                if (!isAndroid) {
                    FadePageTransitionOut();
                }
                window.location = '<%=ResolveUrl("~")%>Teacher/TeacherStudentDetailPage.aspx?StudentId=' + StudentId;
            }
}


//Function ลบนักเรียนออกจาก favorite 
function DeleteFavorite(InputObj, t) {
    var StudentId; var Objimg;
    if ($.browser.msie && $.browser.version < 9.0) {
        StudentId = InputObj.srcElement.getAttribute('StId');
    } else {
        Objimg = $(InputObj);
        StudentId = $(Objimg).attr('StId');
    }
    //alert(StudentId);
    $.ajax({
        type: "POST",
        url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
        data: "{ StudentId: '" + StudentId + "'}",
        contentType: "application/json; charset=utf-8", dataType: "json",
        success: function (msg) {
            if (msg.d !== '') {
                //alert(msg.d);
                $(Objimg).parent().parent().hide();
                var CheckDivResult = $('.DivCover:visible').length;
                if (CheckDivResult == 0) {
                    $('#DivHaveData').remove();
                    var ap = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo'><img src='Images/ChooseRoom.png' style='width: 130px; position: absolute; top: 40px;left:20px;' /><span style='font-size: 60px; font-weight: bold; position: relative; top: 60px;'>เลือกห้องก่อนค่ะ</span></div></div>");
                    $('#MainDiv').append(ap);
                }
            }
        },
        error: function myfunction(request, status) {
            //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        }
    });
}
    </script>
    <script src="../js/iconFavorite.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hdKeepModeRoom" runat="server" />
    <div id="main" style="height: 670px">
        <div id="site_content">
            <div class="content">
                <img src="../Images/dashboard/Home.png" alt="" class="imghome" onclick="if(!isAndroid){FadePageTransitionOut();};window.location='../Student/DashboardStudentPage.aspx';"
                    style="position: absolute; width: 80px; height: 60px; margin-left: 20px; cursor: pointer; top: 30px;" />
                <myTerm:UserControl ID="MyCtlTerm" runat="server" />
                <%--<myStudentList:UserControl ID="MyCtlStudentList" runat="server" />--%>
                <div id="headClassName" style="text-align: center;">
                    <h3>
                        <asp:Label ID="lblClassRoom" runat="server" ClientIDMode="Static"></asp:Label>
                    </h3>
                </div>
                <div id="ShowStudentSearch" runat="server">
                </div>
                <div id="ShowStudentSelectedRoom" runat="server">
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
                    <input type="text" id="txtSearhStudent" placeholder="ค้นหานักเรียน" onfocus="SearchStudent();"
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
</asp:Content>
