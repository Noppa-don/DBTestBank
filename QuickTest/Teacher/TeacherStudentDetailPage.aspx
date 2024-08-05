<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master"
    CodeBehind="TeacherStudentDetailPage.aspx.vb" Inherits="QuickTest.TeacherStudentDetailPage" %>

<%@ Register Src="../UserControl/SelectTermControl.ascx" TagName="SelectTermControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Reference Control="~/UserControl/GridDetailStudentControl.ascx" %>
<%@ Reference Control="~/UserControl/CompareChartControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <%--<link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />--%>
    <script src="<%=ResolveUrl("~")%>js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <link href="../css/iconFavorite.css" rel="stylesheet" type="text/css" />
<style type="text/css">
        .AllMainDiv {
            width: 984px;
            height: 560px;
            margin-left: auto;
            margin-right: auto;
        }

        .SecondMainDiv {
            width: 979px;
            height: 555px;
            margin-left: auto;
            margin-right: auto;
        }

        .DivStudentDetail {
            width: 800px;
            height: 80px;
            margin-left: auto;
            margin-right: auto;
        }

        table tr td {
            background: initial;
            padding: initial;
            border-bottom: initial;
        }

        .DivStudentInfo table {
            margin: initial;
        }

        .DivStudentInfo {
            width: 100%;
            float: left;
        }

            .DivStudentInfo table tr:first-child td:first-child {
                font-size: 30px;
                width: 50%;
            }

            .DivStudentInfo table tr:first-child td:last-child {
                font-size: 18px;
            }

            .DivStudentInfo table tr:last-child td {
                font-size: 16px;
            }

        .DivImg {
            width: 150px;
            float: left;
            border: 1px solid;
        }

        .ForYellowStar, .ForGrayStar {
            /*width: 50px;
            height: 45px;*/
            position: relative;
            bottom: 40px;
            left: 5px;
            cursor: pointer;
        }

        .DivForImg {
            /*width: 150px;
            height: 170px;*/
            width: 179px;
            height: 128px;
            border: 1px solid rgb(235, 235, 235);
            border-radius: 5px 15px 5px 5px;
            position: absolute;
            top: 0;
            right: 0;
        }

        /*.Forbtn
        {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/ /*
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width: 128px;
        }*/
        .ForDivBtn {
            margin-left: 30px;
            width: 480px;
            border: solid 1px #DA7C0C;
            text-align: center;
            display: table;
            margin-top: 20px;
            /*background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);*/
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

        .ForDivBtnBottom {
            border: initial;
            background: initial;
        }

            .ForDivBtnBottom #divBtn {
                border: 1px solid #FFA032;
                border-radius: .5em;
                margin-top: initial !important;
                text-align: center !important;
            }

        .ForbtnMenu {
            font: 100% 'THSarabunNew';
            /*width: 160px;*/ width: 120px;
            height: 45px;
            line-height: 45px;
            display: inline-block;
            text-align: center;
            cursor: pointer;
            color: #fff;
            border: 0;
            background: transparent;
            text-transform: uppercase;
            text-decoration: none;
            border-right: 1px solid;
        }

        .ForTimebtnMenu {
            font: 100% 'THSarabunNew';
            /*width: 160px;*/ width: 85px;
            height: 45px;
            line-height: 45px;
            display: inline-block;
            text-align: center;
            cursor: pointer;
            color: #fff;
            border: 0;
            background: transparent;
            text-transform: uppercase;
            text-decoration: none;
            border-right: 1px solid;
        }

        .ForDivBottom {
            width: 800px;
            height: 300px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 30px;
        }

        /* Accordion*/
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
            background: #ffc077; /* Old browsers */
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
            right: -30px;
            top: 50%;
            margin-top: -40px;
            background-image: url('../images/dashboard/OpenClass.png');
            background-size: 40px;
            background-repeat: no-repeat;
            cursor: pointer;
            background-position: 50%;
        }

        .msMainMenu {
            height: auto;
            position: relative; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            margin: 5px;
            margin-right: 10px;
        }

        .itmAcd {
            border-bottom: 1px solid;
            padding-top: 5px;
            padding-bottom: 5px;
            font-size: 18px;
            cursor: pointer;
            /*text-align: center;*/
            height: 35px;
            padding-left: 10px;
            position: relative;
            line-height: 2em; /* น่าจะคือ 2 เท่าของ font-size: 18px; */
        }

        .ui-accordion .ui-accordion-header {
            text-align: center;
            color: #444;
            height: 40px;
            line-height: 15px;
            font-size: 30px;
        }

        .ui-accordion .ui-accordion-content {
            height: auto;
            line-height: 16pt;
            padding: 0;
            background-color: White;
            color: #444;
        }

        .ui-state-active {
            /*border: 1px solid yellow;*/
            background-color: #FFA032;
            color: #444;
        }

        .ImgSmallPic {
            /*float: right;
            margin-right: 5px;*/
            height: 100%;
            position: absolute;
            margin: -5px;
            right: 0;
        }
        /* Accordion*/
        .btnFixSlide {
            position: fixed;
            margin: 0px;
            display: block;
            margin-left: -2px;
            width: 160px;
            height: 80px;
            background-color: #CFCFCF;
            background-repeat: no-repeat;
            background-position: right center;
            border: 1px solid #AFAFAF;
            -moz-border-radius: 0px 10px 10px 0px;
            -webkit-border-top-left-radius: 10px;
            -webkit-border-top-right-radius: 10px; /*-khtml-border-bottom-right-radius: 10px;
            -khtml-border-top-right-radius: 10px;*/
            border-radius: 10px 10px 0px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            opacity: 0.6;
            filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
            font-size: 18px;
        }

        .MainFixSlide {
            position: fixed;
            margin: 0px;
            padding: 0px;
            right: 280px;
            bottom: 30px;
            list-style: none;
            z-index: 9999;
        }

        #ChangeFontSize {
            display: none !important;
        }

        #navigation, #ExtraMenu, .ulTopMenu {
            display: none !important;
        }

        .ui-accordion .ui-accordion-content {
            overflow-y: auto;
            overflow-x: hidden;
            height: auto;
            max-height: 460px;
        }

        #OtherClass {
            position: initial;
            top: 0px;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            line-height: 62px;
            cursor: pointer;
            margin: 10px 5px;
            border: 1px solid rgb(235, 235, 235);
            background-color: #FFA032;
            border-radius: 5px;
            height: 62px;
            color: #444;
        }

        #DivInfo
        {
            width: 876px;
            border: solid 1px #DA7C0C;
            margin-left: 30px;
            margin-bottom: 27px;
        }
        #MainDiv
        {
            height: 360px;
            overflow-y: auto;
            width: 780px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 32px;
        }

        #RadGrid1
        {
            width: 777px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 10px;
            margin-bottom: 10px;
        }

        .comiseo-daterangepicker-presets
        {
            display:none;
        }
    </style>


    <%If IsAndroid = True Then%>
    <style type="text/css">
        .ForbtnMenu {
            width: 190px !important;
        }

        #btnHomework, #btnQuizHistory, #btnPracticeHistory, #btnLog {
            height: 70px !important;
        }

        #tdStudentName, #tdStudentRoomDetail {
            font-size: 30px !important;
            padding-top: 45px !important;
        }

        #tdStudentDetail {
            font-size: 30px !important;
        }
    </style>
    <%End If%>  
    <!--[if gte IE 9]>
        <style type="text/css">
        .gradient{filter:none;}
        </style>
        <![endif]-->
    <script type="text/javascript">
        /* css for IE 8-11,Safari */
        $(function () {
            if ($.browser.msie || !!navigator.userAgent.match(/Trident\/7\./)) {
                $('.ForbtnMenu').css('width', '195px');
                $('td').css({ 'background-color': 'white!important', 'background': '#FFFFFF!important' });
                $('.ForDivBtnBottom').css({ 'background': 'white', 'border': '0px' });
                $('tr.rgRow').children('td').css('background-color', '#FFFFCC !important');
            }
            if (navigator.userAgent.indexOf('Firefox') > -1) {
                $('.ForbtnMenu').css('width', '195px');
                $('td').css({ 'background-color': 'white', 'background': '#FFFFFF' });
                $('.ForDivBtnBottom').css({ 'background': 'white', 'border': '0px' });
                $('tr.rgRow').children('td').css('background-color', '#FFFFCC');
                $('.ForYellowStar').css({ 'top': '90px', 'bottom': '0' });
                $('.ForGrayStar').css({ 'top': '90px', 'bottom': '0' });
            }
            if (navigator.userAgent.indexOf('Safari') > -1) {
                $('.ForbtnMenu').css('width', '195px');
            }
        });
    </script>
    <telerik:RadCodeBlock ID='RadCodeBlock1' runat="server">

        <script type="text/javascript">
            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;

            $(document).ready(function () {
                //hover animation
                InjectionHover('.ForbtnMenu', 3);
                InjectionHover('#ImgBackHome', 3, false);
            });

            $(function () {

                // ดักว่าถ้าหาห้องไม่ได้ให้ซ่อน Panel Accordion เลือกห้องไป
                var JSCheckHaveInfo = '<%=CheckIsHaveInfo %>';

                if (JSCheckHaveInfo == 'False') {
                    $('#menuClassName').hide();
                }
                else {
                    $('#menuClassName').show();
                }

                // เอาเมนู dashboard ออก
                //$('.ulTopMenu').remove();

                // สร้าง Accordion 
                $("#ac").accordion('destroy').accordion({ autoHeight: false, animate: 0, heightStyle: "content" });

                //function กดติดดาวเพิ่มนักเรียนเข้าไปใน favorite
                //$('.imgStar').click(function () {
                //    if ($(this).hasClass('ForGrayStar')) {
                //        var ImgId = $(this);
                //        var StudentId = $(this).attr('StId');
                //        $.ajax({
                //            type: "POST",
                //        url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/AddStudentFavorite",
                //        data: "{ StudentId: '" + StudentId + "'}",
                //        contentType: "application/json; charset=utf-8", dataType: "json",
                //        success: function (msg) {
                //            if (msg.d !== '') {
                //                //alert(msg.d);
                //                $(ImgId).attr('src', '../Images/dashboard/student/Unfavorite.png');
                //                $(ImgId).removeClass('ForGrayStar');
                //                $(ImgId).addClass('ForYellowStar');
                //            }
                //        },
                //        error: function myfunction(request, status) {
                //            //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                //        }
                //    });
                //}
                //else {
                //    var ImgId = $(this);
                //    var StudentId = $(this).attr('StId');
                //    $.ajax({
                //        type: "POST",
                //            url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/DeleteFavoriteStudent",
                //            data: "{ StudentId: '" + StudentId + "'}",
                //            contentType: "application/json; charset=utf-8", dataType: "json",
                //            success: function (msg) {
                //                if (msg.d !== '') {
                //                    //alert(msg.d);
                //                    $(ImgId).attr('src', '../Images/dashboard/student/Favorite.png');
                //                    $(ImgId).removeClass('ForYellowStar');
                //                    $(ImgId).addClass('ForGrayStar');
                //                }
                //            },
                //            error: function myfunction(request, status) {
                //                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                //            }
                //        });
                //    }
                //});
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Function ลบนักเรียนออกจาก favorite 
                $('.ForDeleteFavorite').click(function () {
                    var Objimg = $(this);
                    var StudentId = $(this).attr('StId');
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
                                    var ap = $("<div id='DivNoData' class='ForMainDivNoData' ><div id='DivShowInfo' class='ForDivShowInFo'><img src='Images/blue-arrow-pointing-left-hi.png' style='width: 130px; position: absolute; top: 40px;left:20px;' /><span style='font-size: 60px; font-weight: bold; position: relative; top: 60px;'>เลือกห้องก่อนค่ะ</span></div></div>");
                                    $('#MainDiv').append(ap);
                                }
                            }
                        },
                        error: function myfunction(request, status) {
                            //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                        }
                    });
                });

                $('#OtherClass').click(function () {
                    window.location = '../Student/StudentListPage.aspx'
                });


                //ปุ่ม Hide Show PanelAccordion แสดงนักเรียนในห้อง
                $('.showMenuFixedBar').click(function () {
                    var menuClass = $('#menuClassName');
                    if ($(menuClass).hasClass('hideClassName')) {
                        $(this).css('background-image', "url('../images/dashboard/CloseClass.png')");
                        $(menuClass).removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
                    } else {
                        $(this).css('background-image', "url('../images/dashboard/OpenClass.png')");
                        $(menuClass).removeClass('showClassName').addClass('hideClassName', 500, 'easeOutBounce');
                    }
                });

                ////////////////////////////////////////////////////////////////////// Fix Slide
                $('#btnToggle').stop().animate({ 'bottom': '-52px' }, 1000);
                $('#DivBtnToggle').hover(
                function () {
                    $('#btnToggle', $(this)).stop().animate({ 'bottom': '2px' }, 200);
                },
                    function () {
                        $('#btnToggle', $(this)).stop().animate({ 'bottom': '-52px' }, 200);
                    }
                    );

            });


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Function เมื่อกดเลือกเด็กจาก Accordion จะต้องไป SetStudentId ให้เป็นเด็กคนที่เลือก และ Reload หน้าใหม่
            function ChooseStudentAccordion(InputStudentId, InputTeacherId) {

                if (InputTeacherId == '') {
                    //redirect กลับหน้าตัวเองโดยไม่ต้องมี querystring teacherid และปุ่มเป็นคำว่า ดูทั้งโรงเรียน
                    window.location = '../Teacher/TeacherStudentDetailPage.aspx?StudentId=' + InputStudentId;
                }
                else {
                    //redirect กลับหน้าตัวเองโดยมี querystring teacherid และปุ่มเป็นคำว่า ดูเฉพาะของตัวเอง
                    window.location = '../Teacher/TeacherStudentDetailPage.aspx?StudentId=' + InputStudentId + '&TeacherId=' + InputTeacherId;
                }

            };

            //Set CalendarId
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
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
            // Close Lightbox Calendar
            function CloseFancyBox() {
                $.fancybox.close();
            }

            // ปุ่มกลับหน้าแรก
            function GoHome() {
                FadePageTransitionOut();
                window.location = '<%=ResolveUrl("~")%>Student/DashboardStudentPage.aspx'
            }

        </script>

    </telerik:RadCodeBlock>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">
        <div id="site_content" style="position: relative;">
            <div class="content">
                <img src="../Images/dashboard/Home.png" <%--onclick='GoHome();'--%> id="ImgBackHome" style='top: 15px; cursor: pointer; position: absolute; width: 70px; margin-left: 20px;' />
                <div id='DivSelectTerm'>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
                    <uc1:SelectTermControl ID="SelectTermControl1" runat="server" />
                </div>
                <div id='DivStudentDetail' class='DivStudentDetail' runat="server">
                    
                </div>
                <div id='DivTimeFilter' class='ForDivBtn' style="width: 428px!important; margin-left: 20px;margin-top: -20px;">
                    <asp:Button ID="btnNow" ClientIDMode="Static" runat="server" Text="วันนี้" CssClass='ForTimebtnMenu' />
                    <asp:Button ID="btnSevenDay" ClientIDMode="Static" runat="server" Text="7 วัน" CssClass='ForTimebtnMenu' />
                    <asp:Button ID="btnFifteenDay" ClientIDMode="Static" runat="server" Text="15 วัน" CssClass='ForTimebtnMenu' />
                    <asp:Button ID="btnMonth" ClientIDMode="Static" runat="server" Text="30 วัน" CssClass='ForTimebtnMenu' />
                    <asp:Button ID="btnTerm" runat="server" ClientIDMode="Static" Text="เทอมนี้" CssClass='ForTimebtnMenu' Style="border-right: 0;" />
                </div>
                <div id="Div2" style="margin-left: 20px; width: 937px; border: solid 1px #DA7C0C;">
                    <div id='DivBtnChooseMode' class='ForDivBtn' style="width: 780px;">
                        <%--<asp:Button ID="btnCompare" runat="server" Text="เปรียบเทียบ" CssClass='ForbtnMenu' />--%>
                        <asp:Button ID="btnHomework" ClientIDMode="Static" runat="server" Text="การบ้าน" CssClass='ForbtnMenu' />
                        <asp:Button ID="btnQuizHistory" ClientIDMode="Static" runat="server" Text="ประวัติควิซ" CssClass='ForbtnMenu' />
                        <asp:Button ID="btnPracticeHistory" ClientIDMode="Static" runat="server" Text="ประวัติฝึกฝน" CssClass='ForbtnMenu' />
                        <asp:Button ID="btnLog" runat="server" ClientIDMode="Static" Text="กิจกรรม" CssClass='ForbtnMenu' Style="border-right: 0;" />
                        
                    </div>
           
                   
                    <div id="DivInfo">
                        <div id='DivBottomInfo' class='ForDivBtnBottom' runat="server">
                        </div>
                    </div>
                </div>
                 
                <div id='DivBtnToggle' class='MainFixSlide'>
                    <asp:Button ID="btnToggle" runat="server" ClientIDMode="Static" Text="ข้อมูลเฉพาะที่สอน"
                        CssClass='btnFixSlide' />
                </div>
              
            </div>
        </div>
    </div>
    <div id="menuClassName" class="hideClassName">
        <div class="showMenuFixedBar" style="right: -35px;">
        </div>
        <div class="menuFixedBar">
            <div id="OtherClass">ดูห้องอื่น</div>
            <div id='mainMenu' class='msMainMenu'>
                <div id="ac" clientidmode="Static" runat="server">
                </div>
            </div>
        </div>
    </div>

    <%--  <div class="divSocial" style="position:absolute;width:184px;height:90px;background-color:grey;top:146px;right:360px;border-radius: 10px;padding:10px;">
        <div style="background-image:url('../images/dashboard/student/unfavorite.png');"></div>
        <div style="background-image:url('../images/dashboard/student/kappa.png');"></div>
        <div style="background-image:url('../images/dashboard/student/rho.png');"></div>
        <div style="background-image:url('../images/dashboard/student/alpha.png');"></div>
        <div style="background-image:url('../images/dashboard/student/medal.png');"></div>
        <div style="background-image:url('../images/dashboard/student/mortarboard.png');"></div>
        <div style="background-image:url('../images/dashboard/student/palette.png');"></div>
        <div style="background-image:url('../images/dashboard/student/basketball.png');"></div>
    </div>--%>
    <script type="text/javascript">
        var RunModeConfig = '<%= BusinessTablet360.ClsKNSession.RunMode %>';
        $(function () {

            //เช็ค RunMode เพื่อ ซ่อน - โชว์ ปุ่ม
            if (RunModeConfig == 'labonly') {
                $('#btnHomework').hide();
                $('#btnPracticeHistory').hide();
                $('#btnLog').hide();
                $('#btnQuizHistory').css({ 'width': '100%', 'border': 'initial' });
            }

            //ปุ่ม กลับ (รูปบ้าน)
            new FastButton(document.getElementById('ImgBackHome'), GoHome)

            //ปุ่มการบ้าน,ประวัติควิซ,ประวัติฝึกฝน,กิจกรรม
            $('.ForbtnMenu').each(function () {
                new FastButton(this, TriggerServerButton);
            });
                                           

            if ($('.itmAcd').length != 0) {
                $('.itmAcd').each(function () {
                    new FastButton(this, TriggerItmAcd);
                });
            }

            //ดักสำหรับ Tablet ของครู
            //if (isAndroid) {
            //$('.ForbtnMenu').css('width', '190px');
            //$('#btnHomework').css('height', '70px');
            //$('#btnQuizHistory').css('height', '70px');
            //$('#btnPracticeHistory').css('height', '70px');
            //$('#btnLog').css('height', '70px');
            //$('#tdStudentName').css({ 'font-size': '30px', 'paddint-top': '45px' });
            //$('#tdStudentRoomDetail').css({ 'font-size': '30px', 'paddint-top': '45px' });
            //$('#tdStudentDetail').css({ 'font-size': '30px' });
            //}
        });
              
        //function TriggerClick(e) {
        //    var obj = e.target;
        //    $(obj).trigger('click');
        //}

        function TriggerItmAcd(e) {
            var obj = e.target;
            var stdId = $(obj).attr('stdId');
            var tcId = $(obj).attr('tcId');
            if (!isAndroid) {
                FadePageTransitionOut();
            }

            ChooseStudentAccordion(stdId, tcId);
        }
       
    </script>    
    <script src="../js/iconFavorite.js" type="text/javascript"></script>
</asp:Content>
