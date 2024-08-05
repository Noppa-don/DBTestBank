<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master"
    CodeBehind="DashboardStudentPage.aspx.vb" Inherits="QuickTest.DashboardStudentPage" %>

<%@ Register Src="~/UserControl/StudentListControl.ascx" TagName="UserControl" TagPrefix="myStudentList" %>
<%@ Register Src="~/UserControl/SelectTermControl.ascx" TagName="UserControl" TagPrefix="myTerm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .ClassRoomMenu {
            position: relative;
            text-align: center;
            width: 400px;
            margin: 10px auto 0 auto;
            font-size: 190%;
            color: #F68500;
        }

        .NotHaveRoom {
            width: 821px;
            height: 150px;
            border: 1px dashed #FFA032;
            margin: auto;
            text-align: center;
            line-height: 150px;
            font-weight: bold;
            color: #444;
            border-radius: .5em;
        }

        .HaveRoom {
            width: 821px;
            height: auto;
            max-height: 170px;
            border: 1px solid #FFA032;
            margin: auto;
            overflow-y: auto;
            border-radius: .5em;
            padding: 10px 0;
            text-align: center;
        }

            .HaveRoom div {
                width: 113px;
                height: 38px;
                line-height: 38px;
                margin: 10px 20px;
                display: inline-block;
                text-align: center;
                font-weight: bold;
                border-radius: .5em;
                cursor: pointer;
                color: #444;
                border-top: 1px solid rgb(235, 235, 235);
                background-color: #FFA032;
            }

        a.hint {
            color: #444;
        }

            a.hint:hover {
                color: #09D4FF;
            }
   

    </style>
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <link href="../css/iconFavorite.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ForDeleteFavorite, .ForGrayStar, .ForYellowStar{
            top:115px!important;
        }

    </style>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {

            //ดักไว้ถ้าเข้าจาก Tablet ครู
            if (isAndroid) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                $('.ClassRoomMenu').css('font-size', '45px');
                $('.ClassRoomMenu img').css({'width': '60px','top':'10px','right':'90px'});
                $('#MainDiv').css('font-size', '20px');
                $('#ctl00_MainContent_RoomsHaveQuizAndHomework').css('font-size', '22px');
                $('.HaveRoom div').css({ 'width': '200px', 'height': '70px', 'line-height': '70px', 'font-size': '35px' })
            }

            //เปิดหน้าข่าวโรงเรียน
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/DashboardSignalRService.asmx/CheckIsHaveCurrentNews",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Show') {
                        setTimeout(function () {
                            if (isAndroid) {
                                $.fancybox({
                                    'autoScale': true,
                                    'transitionIn': 'none',
                                    'transitionOut': 'none',
                                    'href': '<%=ResolveUrl("~")%>SchoolNewsPage.aspx',
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
                                    'href': '<%=ResolveUrl("~")%>SchoolNewsPage.aspx',
                                    'type': 'iframe',
                                    'width': 1000,
                                    'minHeight': 650
                                });
                            }
                        }, 100)
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });

            //เช็คว่าเด็กมี Up รุปเข้ามาใหม่หรือเปล่า
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/CheckIsNewStudentPhoto",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'True') {
                        $.fancybox({
                            'autoScale': true,
                            'transitionIn': 'none',
                            'transitionOut': 'none',
                            'href': '<%=ResolveUrl("~")%>DroidPad/CheckPhotoApprovalStatusPage.aspx',
                                'type': 'iframe',
                                'width': 1000,
                                'minHeight': 700
                            });
                        }
                    },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });

        });

            function SetSesstionCalendarId(CalendarId, CalendarName) {
                $.ajax({
                    type: "POST",
                    async:false,
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

            // img click to studentlistpage
            function ToStudentListPage() {
                window.location = '<%=ResolveUrl("~")%>Student/StudentListPage.aspx';
            }
    </script>
    <script type="text/javascript">
        $(function () {
            //กดปุ่ม เลือกห้อง
            new FastButton(document.getElementById('imgChooseClass'), TriggerClickImgChooseClass);

            //เมื่อกดคลิก ห้อง
            $('.HaveRoom div').each(function () {
                new FastButton(this, TriggerClickClassRoom);
            });

            //$(".HaveRoom div").click(function () {
                //alert($(this).html());
                //var classroom = $(this).html();
                //window.location = '<%=ResolveUrl("~")%>Student/StudentListPage.aspx?SelectedClassRoom=' + classroom;
            //});
        });

        function TriggerClickImgChooseClass(e) {
            //page Transition
            if (!isAndroid) {
                FadePageTransitionOut();
            }
            ToStudentListPage();
        }

        function TriggerClickClassRoom(e) {
            var obj = e.target;
            var classroom = $(obj).html();
            FadePageTransitionOut();
            window.location = '<%=ResolveUrl("~")%>Student/StudentListPage.aspx?SelectedClassRoom=' + classroom;
        }
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">        
        <div id="site_content">
            <div class="content">
                <myTerm:UserControl ID="MyCtlTerm" runat="server" />
                <myStudentList:UserControl ID="MyCtlStudentList" runat="server" />
                <div id="" class="ClassRoomMenu">
                    ห้อง
                    <img src="../Images/dashboard/SeeAllClass.png" id="imgChooseClass" alt="" style="position: absolute; top: 15px; right: 120px; cursor: pointer;"
                        <%--onclick="ToStudentListPage();"--%> />
                </div>
                <div id="RoomsHaveQuizAndHomework" runat="server">
                </div>
            </div>
        </div>
    </div>

</asp:Content>
