<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReviewMostWrongAnswer.aspx.vb"
    Inherits="QuickTest.ReviewMostWrongAnswer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="../css/fixMenuSlide.css" rel="stylesheet" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/swipe.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../js/highcharts.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <link href="../css/styleQuestionAnswerExplain.css" rel="stylesheet" />
    <link href="../css/LogoutStyle.css" rel="stylesheet" />
    <style type="text/css">
        #AnswerExp > div > div {
            font-size: 24px !important;
        }

        div#QuestionExp {
            width: 625px !important;
            font-size: 24px !important;
        }

        #btnQuestionExp {
            right: -120px !important;
            top: -7px !important;
            cursor: pointer;
        }
    </style>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .divQuestionAndAnswer {
            top: 30px;
        }

        .ImgCircle {
            position: absolute;
            top: 2px;
            left: -14px;
        }


        body {
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        .divBlockMostWrongAnsSkip {
            background-image: url(../Images/upgradeClass/skip.png);
            background-position: top;
            opacity: 0.8;
            width: 140px;
            height: 140px;
            border-radius: 0.9em;
            position: absolute;
        }

        .divBlockMostWrongAns, #btnSlideMenu, .lblQuizName {
            cursor: pointer;
        }

        .Question {
            font: bold 30px 'THSarabunNew';
            position: relative;
            -webkit-border-radius: 10px 10px 0px 0px;
            background-color: #ffc76f;
            height: auto;
            width: 650px;
            border: 20px;
            padding: 20px;
            margin-left: -40px;
            border-radius: 10px 10px 0px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        div#mainAnswer {
            width: 650px;
            position: relative;
            background-color: #F4F7FF;
            padding: 20px;
            -webkit-border-radius: 0px 0px 10px 10px;
            margin-left: -40px;
            border-radius: 0px 0px 10px 10px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .Answer {
            width: 283px;
            position: relative;
            float: left;
            height: 80px;
            font-size: 20px;
            border-right: solid 1px #AFAFAF;
            border-left: solid 1px #AFAFAF;
            border-bottom: solid 1px #AFAFAF;
            padding: 20px;
            background-color: #F4F7FF;
        }

        .QQ_no {
            border-radius: 15px 15px 15px 15px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .ImgSkip {
            position: absolute;
            right: 235px;
            margin-top: -10%;
            z-index: 999;
        }

        #mainQuestion p:first-child, #mainAnswer td p:first-child {
            display: inline;
        }
    </style>
    <title></title>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <%If Not IE = "1" Then%>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var SelectedName = GetSelectedSession();
        var ThisPage = (window.location.pathname).toLowerCase().substring(1).replace(ResolveUrl("~/").toLowerCase().substring(1), '');
        var SignalRCheck;

        var ExamNum = '<%=_Examnum %>';
        var ExamAmount = '<%= _ExamAmount %>';
        //console.log(ExamNum);
        window.hubReady = $.connection.hub.start();
        window.hubReady.done(function () {
            SignalRCheck.server.addToGroup(SelectedName);
            SignalRCheck.server.sendCommand(SelectedName, ThisPage + window.location.search);
            SignalRCheck.server.sendCommand(SelectedName, 'Reload');
            //            $('.btnNext').click(function () {
            //                SignalRCheck.server.sendCommand(SelectedName, 'Reload');
            //            });
            //            $('.btnPrev').click(function () {
            //                SignalRCheck.server.sendCommand(SelectedName, 'Reload');
            //            });  
            //            $('.QQ_No').click(function () {
            //                SignalRCheck.server.sendCommand(SelectedName, 'Reload');
            //            });        
        });

        SignalRCheck = $.connection.hubSignalR;
        SignalRCheck.client.send = function (message) {
            console.log(message);
            if (message == ThisPage) {
            } else if (message == 'Reload') {
                GetCurrentExamNum();
            } else {
                window.location = '<%=ResolveUrl("~")%>' + message;
            }
        };

    function GetCurrentExamNum() {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Activity/ReviewMostWrongAnswer.aspx/GetCurrentExamnum'),
            data: "{ Examnum : '" + ExamNum + "'}",
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                if (data.d == "Reload") {
                    window.location = "<%=ResolveUrl("~")%>Activity/ReviewMostWrongAnswer.aspx"
                }
            },
            error: function myfunction(request, status) {
                alert('GetCurrentPage');
            }
        });
    }

    $(function () {

        var testsetName = '<%= TestsetName%>';

        //qtip - TestsetName
        $('#divTestsetNameTop').qtip({
            content: testsetName,
            show: { event: 'mouseover' },
            style: {
                width: 500, padding: 5, background: '#F68500', color: 'white', textAlign: 'center',
                border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
            },
            position: { corner: { tooltip: 'topMiddle', target: 'bottomMiddle' }, adjust: { x: 0, y: -20 } },
            hide: { when: { event: 'mouseout' }, fixed: false }
        });

        $('#divSideTestsetName').qtip({
            content: testsetName,
            show: { event: 'mouseover' },
            style: {
                width: 500, padding: 5, background: '#F68500', color: 'white', textAlign: 'center',
                border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
            },
            position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' }, adjust: { x: -10, y: 0 } },
            hide: { when: { event: 'mouseout' }, fixed: false }
        });

        // รับค่าจากที่ส่งมา
        //            SignalRCheck.client.cmdControl = function (cmd) {
        //                console.log(cmd);
        //                if (cmd == 'Next') {
        //                    $('#<%=btnNextTop.ClientID %>').trigger('click');
        //                }
        //                else if (cmd == 'Prev') {
        //                    $('#<%=btnPrvTop.ClientID %>').trigger('click');
        //                }
        //                else if (cmd == 'SortOrder') {
        //                    $('#btnSortQuestionNormal').trigger('click');
        //                }
        //                else if (cmd == 'SortMost') {
        //                    $('#btnSortQuestionMostWrong').trigger('click');
        //                }
        //            };
        //            $.connection.hub.start().done(function () {
        //                SignalRCheck.server.addToGroup(SelectedName);
        //                SignalRCheck.server.sendCommand(SelectedName, ThisPage);
        //            });
        //            $('.btnNext').click(function () {
        //                SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Next');
        //            });
        //            $('.btnPrev').click(function () {
        //                SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Prev');
        //            });
        //            $('#btnSortQuestionNormal').click(function () {
        //                SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'SortOrder');
        //            });
        //            $('#btnSortQuestionMostWrong').click(function () {
        //                SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'SortMost');
        //            });
        $('#dialog').dialog({
            autoOpen: false,
            buttons: {
                'ใช่': function () {
                    if (!isAndroid) {
                        FadePageTransitionOut();
                    }
                    window.location = '<%=ResolveUrl("~")%>activity/AlternativePage.aspx';
                }, 'ไม่': function () { $(this).dialog('close'); }
            },
            draggable: false,
            resizable: false,
            modal: true
        });
    });

    function ResolveUrl(url) {
        if (url.indexOf("~/") == 0) {
            url = baseUrl + url.substring(2);
        }
        return url;
    }
    function GetSelectedSession() {
        var selected;
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetSelectSession'),
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                selected = data.d;
            },
            error: function myfunction(request, status) {
                alert('GetSelectedSession');
            }
        });
        return selected;
    }
    function SetUnload(unload) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetUnload'),
            data: "{ unload : '" + unload + "'}",
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
            },
            error: function myfunction(request, status) {
                alert('SetUnload');
            }
        });
    }
    function GetUnload() {
        var Unload;
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetUnload'),
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                Unload = data.d;
            },
            error: function myfunction(request, status) {
                alert('GetUnload');
            }
        });
        return Unload;
    }
    function SetCurrentPage(ThisPage) {
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetCurrentPage'),
            data: "{ page : '" + ThisPage + "'}",
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
            },
            error: function myfunction(request, status) {
                alert('SetCurrentPage');
            }
        });
    }
    function GetCurrentPage() {
        var CurrentPage;
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetCurrentPage'),
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                CurrentPage = data.d;
            },
            error: function myfunction(request, status) {
                alert('GetCurrentPage');
            }
        });
        return CurrentPage;
    }

    </script>
    <%End If%>
    <script type="text/javascript">
        $(function () {
            // Check Menu Position
            checkMenu();
            var reviewHideOrShow = $('#ShowReviewMostWrong').val();

            if (reviewHideOrShow == 0) {

                $('#ShowReviewMostWrong').val('1');
                $('.divReviewAns').stop().animate({ 'top': '96%' }, 1000);
                $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-upFlat.png');

            }
            else {

                $('#ShowReviewMostWrong').val('0');
                $('.divReviewAns').stop().animate({ 'top': '5%' }, 1000);
                $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-downFlat.png');

            }

            $('.btnPrev').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') { e.preventDefault(); return false; }
            });

            $('.btnNext').click(function (e) {
                if (ExamNum == ExamAmount) { e.preventDefault(); $('#dialog').dialog('open'); return false; }
            });

            //hover animation
            InjectionHover('.QQ_no', 3);
            InjectionHover('#btnSortQuestionNormal', 5, false);
            InjectionHover('#btnPrvTop', 3);
            InjectionHover('#btnNextTop', 3);
            InjectionHover('#btnPrevious', 3, false);
            InjectionHover('#btnNext', 3, false);
            InjectionHover('#btnPrvSide', 3);
            InjectionHover('#btnNextSide', 3);
            InjectionHover('.imgToggle', 3);
            InjectionHover('.divSetting', 3);
            InjectionHover('.divSettingTop', 3);
            InjectionHover('#btnSortQuestionMostWrong', 3, false);
            //InjectionHover('.imgPreNext', 5, false);
            //InjectionHover('#btnSlideMenu', 2);

            $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#Help > li').hover(function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-4px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
            });

            // swipe div
            $('#DivForImgTag').click(function () {
                if ($('.imgPreNext').attr('swap') == '1') {
                    $('.imgPreNext').hide();
                    $('.imgPreNext').attr('swap', '0');
                }
                else {
                    $('.imgPreNext').show(300);
                    $('.imgPreNext').attr('swap', '1');
                }
            });
            // ซ่อนรูปตอนกดเปิด Tab หน้ายิ้ม
            //            $('#btnSlideMenu').click(function(){
            //            if ($('#imgCartoonAtChart').attr('CheckState') == '1') {
            //                $('#imgCartoonAtChart').hide();
            //                $('#imgCartoonAtChart').attr('CheckState','2');
            //            }
            //            else
            //            {
            //                $('#imgCartoonAtChart').show();
            //                $('#imgCartoonAtChart').attr('CheckState','1');
            //            }
            //          
            //            });
            //-----------------------
            //$('.divReviewAns').stop().animate({ 'top': '5%' }, 1000);
            //$('#btnSlideMenu').toggle(function (e) {
            //    e.preventDefault();
            //    if ($('#ShowReviewMostWrong').val() == 0) {
            //        $('#ShowReviewMostWrong').val('1');
            //        $('.divReviewAns').stop().animate({ 'top': '96%' }, 200);
            //        $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-upFlat.png');
            //    } else {
            //        $('#ShowReviewMostWrong').val('0');
            //        $('.divReviewAns').stop().animate({ 'top': '5%' }, 200);
            //        $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-downFlat.png');
            //    }                
            //}, function (e) {
            //    e.preventDefault();
            //    $('#ShowReviewMostWrong').val('0');
            //    $('.divReviewAns').stop().animate({ 'top': '5%' }, 200);
            //    $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-downFlat.png');
            //});
            $('#btnSlideMenu').click(function (e) {
                e.preventDefault();



                if ($('#ShowReviewMostWrong').val() == 0) {

                    $('#ShowReviewMostWrong').val('1');
                    $('.divReviewAns').stop().animate({ 'top': '96%' }, 1000);
                    $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-upFlat.png');

                } else {

                    $('#ShowReviewMostWrong').val('0');
                    $('.divReviewAns').stop().animate({ 'top': '5%' }, 1000);
                    $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-downFlat.png');

                }
            });
            // Click Toggle Menu Top Side
            //            $('.imgToggle').click(function () {
            //                if ($(this).parent().is('.divMenuTop')) {
            //                    $('#ShowMenuOnPosTop').val('1');
            //                    $('.divMenuTop').hide();
            //                    $('.divMenuSide').show();
            //                }
            //                else {
            //                    $('#ShowMenuOnPosTop').val('0');
            //                    $('.divMenuTop').show();
            //                    $('.divMenuSide').hide();
            //                }
            //            });
            $('.imgToggle').click(function () {
                if ($('#DivSideMenus').css('display') == 'none') {
                    $('#ShowMenuOnPosTop').val('1');
                    $('#DivMenuTops').hide('slow');
                    $('#DivImgTopMenu').hide('slow');
                    // $('#DivMenuTops').css('display', 'none');
                    $('#DivSideMenus').fadeIn('slow');
                    $('#DivimgSideMenu').fadeIn('slow');
                    //$('#DivSideMenus').css('display', 'block');
                }
                else {
                    $('#ShowMenuOnPosTop').val('0');
                    $('#DivSideMenus').hide('slow');
                    $('#DivimgSideMenu').hide('slow');
                    $('#DivMenuTops').fadeIn('slow');
                    $('#DivImgTopMenu').show('slow');
                    //                    $('#DivSideMenus').css('display', 'none');
                    //                    $('#DivMenuTops').css('display', 'block');
                }
            });
            // Click Menu Setting
            $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            $('.divSetting').toggle(function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '5%' }, 200);
            }, function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            });

            //End Activity
            $('.endActivity').click(function () {
                $('#dialog').dialog('open');
            });
            $('.divInfo').toggle(function () {
                $(this).css('background-image', '../Images/Activity/HideRight.png');
            }, function () {
                //$(this).css('', '');
            });
            // click hide/show btn Pre&Next Position
            $('#hideDivNextPre').click(function () {
                $('#ShowBtnOnTapMenu').val('0');
                $('#divNextPre').hide(500);
                $('#divMainNextPre').hide(500);
                $('.menuTopSideNextPre').stop().fadeIn(1800);
            });
            //$('.lblQuizName').click(function () {
            //    $('#ShowBtnOnTapMenu').val('1');
            //    $('#divNextPre').show(500);
            //    $('#divMainNextPre').show(500);
            //    $('.menuTopSideNextPre').hide();
            //});
            $('#btnSortQuestionNormal').click(function () {
                $('#sortQuestion').val('0');
            });
            $('#btnSortQuestionMostWrong').click(function () {
                $('#sortQuestion').val('1');
            });
            $('.imgQuestion').click(function () {
                e.preventDefault();
                $("#ShowReviewMostWrong").val('0');
            });
            $('.btnQuestion').click(function () {

                $("#ShowReviewMostWrong").val('1');
            });
            $('#imgSetting1').click(function () {
                if ($('#DivExit1').css('display') == 'none') {
                    $('#DivExit1').show(300);
                    $('#DivViewReport1').show(300);
                }
                else {
                    $('#DivExit1').hide(300);
                    $('#DivViewReport1').hide(300);
                }
            });
            $('#imgSetting2').click(function () {
                if ($('#DivExit2').css('display') == 'none') {
                    $('#DivExit2').fadeToggle(300);
                    $('#DivViewReport2').fadeToggle(300);
                }
                else {
                    $('#DivExit2').fadeToggle(300);
                    $('#DivViewReport2').fadeToggle(300);
                }
            });
            $('#DivExit1').click(function () {
                $('#dialog').dialog('open');
            });
            $('#DivExit2').click(function () {
                $('#dialog').dialog('open');
            });
        });
        function checkMenu() {
            // เช็ค ปุ่มเลื่อนข้อ
            var menuState = $('#ShowBtnOnTapMenu').val();
            // 0 ปุ่มเลื่อนข้ออยู่บนเมนูข้างบนและข้าง
            if (menuState == 0) {
                $('#divNextPre').hide();
                $('#divMainNextPre').hide();
                $('.menuTopSideNextPre').show();
            } // 1 โชว์แบบที่มีคนวิ่ง
            else {
                $('#divNextPre').show();
                $('#divMainNextPre').show();
                $('.menuTopSideNextPre').hide();
            }
            // เช็คเมนูอยู่ตำแหน่งไหน
            var menuOnPosition = $('#ShowMenuOnPosTop').val();
            // 0 อยู่ข้างบน
            if (menuOnPosition == 0) {
                $('#DivMenuTops').show();
                $('#DivImgTopMenu').show();
            } // 1 อยู่ด้านข้าง
            else {
                $('.divMenuTop').hide();
                $('#DivImgTopMenu').hide();
                $('#DivSideMenus').show();
                $('#DivimgSideMenu').show();
            }
            //var reviewHideOrShow = $('#ShowReviewMostWrong').val();
            //if (reviewHideOrShow == 0) {
            //    $('.divReviewAns').stop().animate({ 'top': '5%' }, 200);
            //    $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-downFlat.png');
            //}
            //else {
            //    $('.divReviewAns').stop().animate({ 'top': '96%' }, 200);
            //    $('#imgUpDown').attr('src', '../Images/Activity/AllNewArrow/arrow-blue-upFlat.png');
            //}
        }

        function CloseHelpPanel() {
            $.fancybox.close();
        }



    </script>
    <script type="text/javascript">        //fancybox
        var ActivityQuizId = '<%= Session("Quiz_Id").ToString()%>';
        function CheckScore() { // check score
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>activity/ActivityReport.aspx?QuizId=' + ActivityQuizId + '&ReportMenu=1&ShowBtnBack=False',
                'type': 'iframe',
                'width': 900,
                'minHeight': 450
            });
        }
    </script>
    <script type="text/javascript">       // ปุ่มสลับเฉลย - กับที่ตอบ
        var resetHtml = true;
        $(function () {
            var CorrectAnswer = ('<%= CorrectAnswerType3%>').toString();
              var MyAnswer = ('<%= MyAnswerType3%>').toString();
              $('#SwapCorrectAnswer').toggle(function () {
                  $('#Answer').remove();
                  $('#AnswerTbl').html(CorrectAnswer);
              }, function () {
                  $('#Answer').remove();
                  $('#AnswerTbl').html(MyAnswer);
              });
              $('#SwapAnswer').toggle(function () {

                  resetHtml = true;
                  $('#Answer').remove();
                  $('#Table1').html(CorrectAnswer);
              }, function () {
                  resetHtml = true;
                  $('#Answer').remove();
                  $('#Table1').html(MyAnswer);
              });
          });
    </script>
</head>
<body style='background: url(../Images/Activity/bg5.png) no-repeat center center fixed; height: 100%; background-repeat: no-repeat; background-size: cover;'>
    <form id="form1" runat="server">
        <div style='width: 970px; margin-left: auto; margin-right: auto;'>
            <div id='DivMenuTops' class="divMenuTop" style='background-image: url(../Images/Activity/BG_Opacity/whLayer40pct2.png); display: none;'>
                <asp:HiddenField ID="ShowBtnOnTapMenu" runat="server" Value="0" />
                <asp:HiddenField ID="ShowMenuOnPosTop" runat="server" Value="0" />
                <asp:HiddenField ID="ShowReviewMostWrong" runat="server" Value="0" />
                <asp:HiddenField ID="sortQuestion" runat="server" Value="0" />
                <div style='width: 120px; background-color: #256ef4; font: bold 17px "THSarabunNew"; height: 80px;'>
                    <span class='forspan' style='color: white; line-height: 80px'><b>ทบทวน</b></span>
                </div>
                <div id="divTestsetNameTop" style='width: <%=SizeWidthForDivs %>px; background-color: #06a2dc; height: 80px; font: bold 20px "THSarabunNew"; overflow: hidden;'>
                    <asp:Label ID="lblTestsetName" CssClass='forspan' Style='position: static; color: White;'
                        runat="server" Text="Label"></asp:Label>
                </div>
                <%Try%>
                <%If Session("QuizUseTablet").ToString() = "True" Then%>
                <div id='DivReport' style='position: relative; width: 140px; background-color: #fec500; height: 80px'>
                    <div style='position: absolute; top: 50%; height: 50px; margin-top: -35px; background-color: transparent;'>
                        <p style="text-align: left">
                            <asp:Image ID="ImgRed" Style='margin-top: -8px; height: 15px; width: 5px;' ImageUrl="~/Images/Activity/Red1Px.png"
                                runat="server" />
                        </p>
                        <p style="text-align: left">
                            <asp:Image ID="ImgGreen" Style='margin-top: -9px; height: 15px; width: 5px' ImageUrl="~/Images/Activity/Green1px.png"
                                runat="server" />
                        </p>
                        <p>
                            <asp:Label ID="lblChart" CssClass='forlblChartTon' Style='text-align: left; margin-top: -16px; color: Black;'
                                Width='145' runat="server" Text=""></asp:Label>
                        </p>
                    </div>
                </div>
                <% End If%>
                <div id="divMenuTopNextPre" class="menuTopSideNextPre" style='width: <%=WidthDivExamAmount %>px; cursor: inherit; height: 80px; background-color: #256ef4;'>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 57px; height: 74px;">
                                <asp:ImageButton ID="btnPrvTop" runat="server" src="../images/Activity/forward-icon.png"
                                    Width="50px" class="btnPrev" />
                            </td>
                            <td style="width: 131px; height: 74px; font: normal 18px 'THSarabunNew';">
                                <asp:Label runat="server" ID="lblNoExam" Style='color: White;' CssClass="lblQuizName"></asp:Label>
                            </td>
                            <td style="width: 57px; height: 74px;">
                                <asp:ImageButton ID="btnNextTop" runat="server" src="../images/Activity/next-icon.png"
                                    Width="50px" class="btnNext" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<%If HttpContext.Current.Application("NeedAddEvaluationIndex").ToString() = "True" Then%>
            <div id='EvaluationIndex' style='width: <%=SizeWidthForDivs %>px;'>
                <br />
                ดัชนี
            </div>
            <%End If%>--%>
            </div>
            <div id='DivImgTopMenu' style='width: 80px; position: relative; float: right; top: -104px; left: 15px; z-index: 999;'>
                <img id="imgMenuTopSide;" style='width: 70px; height: 52px; margin-top: 4px; cursor: pointer;'
                    class="imgToggle" src="../Images/Activity/rotate.png" />
                <img id='imgSetting1' class='divSetting' style='width: 63px; height: 50px; cursor: pointer;'
                    src="../Images/Activity/features-icon5555.png" />
                <%If Session("QuizUseTablet").ToString() = "True" Then%>
                <div id='DivViewReport1' class='ForSetting' style='display: none; margin-top: 5px; margin-left: -38px; cursor: pointer; line-height: 42px;'>
                    <a style='color: #FDA200;' title='ดูคะแนน' onclick="CheckScore();"><span>ดูคะแนน</span></a>
                </div>
                <% End If%>
                <div id='DivExit1' class='ForSetting' style='display: none; margin-top: 3px; margin-left: -38px; cursor: pointer; line-height: 42px;'>
                    <a style='text-decoration: none; color: #FDA200;' title="จบกิจกรรม"><span style="font-size: 18px;">จบกิจกรรม</span></a>
                </div>
            </div>
        </div>
        <%--<div class="divMenuSide" style='background-color:#46C4DD;'>--%>
        <div id='DivSideMenus' class="divMenuSide" style='background-image: url(../Images/Activity/BG_Opacity/whLayer40pct2.png);'>
            <%--<img class="imgToggle" src="../Images/Activity/menuTop.png" />--%>
            <div style='height: 100px; background-color: #256ef4; font: bold 17px "THSarabunNew"'>
                <span class='forspan' style='top: 12px; color: White; position: relative; line-height: 80px;'>ทบทวน</span>
            </div>
            <div id="divSideTestsetName" style='height: <%= SideMenuDiv%>px; background-color: #06a2dc; word-wrap: break-word; font: bold 20px "THSarabunNew"; overflow: hidden;'>
                <asp:Label ID="lblSideText" CssClass='forspan' Style='position: relative; color: White;'
                    runat="server" Text=""></asp:Label>
            </div>
            <%If Session("QuizUseTablet").ToString() = "True" Then%>
            <div style='height: 100px; background-color: #fec500;'>
                <asp:Image ID="SideRed" Style='margin-left: 0px; width: 35px; margin-top: 5px;' ImageUrl="~/Images/Activity/Red1Px.png"
                    runat="server" />
                <asp:Image ID="SideGreen" Style='margin-left: 10px; width: 35px; margin-top: 5px;'
                    ImageUrl="~/Images/Activity/Green1px.png" runat="server" />
                <asp:Label ID="lblSideChartWrong" CssClass='forSideChartTon' Style='float: left; color: Black; top: -11px; left: 8px;'
                    runat="server" Text=""></asp:Label>
                <asp:Label ID="lblSideChartCorrect" CssClass='forSideChartTon' Style='float: left; color: Black; top: -12px; left: 19px;'
                    runat="server" Text=""></asp:Label>
            </div>
            <% End If%>
            <%--<%If HttpContext.Current.Application("NeedAddEvaluationIndex").ToString() = "True" Then%>
        <div id='EvaluationIndexDiv' style='height: <%= SideMenuDiv%>px'>
            ดัชนี</div>
        <%End If%>--%>
            <div id="divMenuSideNextPre" style='height: 100px; background-color: #256ef4; cursor: inherit;'
                class="menuTopSideNextPre">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <asp:ImageButton ID="btnPrvSide" runat="server" src="../images/Activity/forward-icon.png"
                                class="BtnNextPreKeepTime btnPrev" Width="50px" Style="margin-top: 8px;" />
                        </td>
                        <td style="width: 50%;">
                            <asp:ImageButton ID="btnNextSide" runat="server" src="../images/Activity/next-icon.png"
                                Width="50px" class="BtnNextPreKeepTime btnNext" Style="margin-top: 8px; margin-right: 3px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; font: normal 20px 'THSarabunNew'; text-align: center; color: white;" colspan="2">
                            <asp:Label runat="server" ID="lblNoExamSide" CssClass="lblQuizName"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id='DivimgSideMenu' style='width: 142px; position: absolute; display: none; left: 0px;'>
            <img class='imgToggle' style='width: 70px; float: left; margin-top: 4px; cursor: pointer;'
                src="../Images/Activity/rotate.png" />
            <img id='imgSetting2' class='divSettingTop' style='width: 70px; height: 65px; cursor: pointer;'
                src="../Images/Activity/features-icon5555.png" />
            <%If Session("QuizUseTablet").ToString() = "True" Then%>
            <div id='DivViewReport2' class='ForSetting' style='z-index: 999; display: none; margin-top: -64px; margin-left: 146px; cursor: pointer; line-height: 42px;'>
                <a style='color: #FDA200;' title='ดูคะแนน' onclick="CheckScore();"><span>ดูคะแนน</span></a>
            </div>
            <% End If%>
            <div id='DivExit2' class='ForSetting' style='z-index: 999; position: relative; display: none; margin-top: 3px; margin-left: 146px; cursor: pointer; line-height: 42px;'>
                <a style='text-decoration: none; color: #FDA200;' title="จบกิจกรรม"><span style="font-size: 18px;">จบกิจกรรม</span></a>
            </div>
        </div>
        <div class="divQuestionAndAnswer" style="margin-bottom: 50px;">
            <div id="btnQuestionExp"></div>
            <div id="mainQuestion" runat="server" class="Question">
                <table id="QuestionTbl" runat="server" style="border-collapse: collapse;">
                    <tr>
                        <td runat="server" id="QuestionTd"></td>
                    </tr>
                    <tr>
                        <td>
                            <% If IsNoAnswer = True Then%>
                            <img id="ImgSkip" src="../Images/Activity/skip.png" class="ImgSkip" alt="" />
                            <%End If%>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="mainAnswer" runat="server">
                <div id="btnAnswerExp"></div>
                <%If IsSwapAnswerType3 Then%>
                <div id="SwapAnswer" style="line-height: 2; background-color: transparent; width: 130px; float: right; position: relative; right: -90px; cursor: pointer;">
                    <img src="../Images/Activity/swap.png" alt="" />
                </div>
                <%End If%>
                <table id="Table1" runat="server" style="width: 650px; border-collapse: collapse;">
                    <tr>
                        <td runat="server" id="AnswerTbl"></td>
                    </tr>
                </table>
                <div id="AnswerExp" runat="server" clientidmode="Static" style="font-size: 24px; display: none;">
                </div>
            </div>
            <%--<div id='divMainNextPre' style='background-image: url(../Images/Activity/BG_Opacity/whLayer40pct2.png); -webkit-border-radius: 6px; position: relative; width: 760px; height: 120px; top: 38px; left: -74px;'>
                <div id="divNextPre" style='left: -14px;'>
                    <table style="width: 100%; margin-left: 70px;">
                        <tr>
                            <td style="width: 48%; float: left; height: 120px;">
                                <asp:ImageButton ID="btnPrevious" Style='width: 50px; position: relative; left: 27px; top: 13px;'
                                    runat="server" src="../images/Activity/forward-icon.png" class="btnPrev" />--%>
            <%-- <asp:ImageButton ID="ImageButton3" runat="server" src="../images/Activity/Home.png"
                            Width="100px" Height="100px" />--%>
            <%-- <asp:Image ID="imgRun" runat="server" src="../images/Activity/run.png" Height="51px"
                            Width="48px" CssClass="positionImg" />--%>
            <%--  </td>
                            <td>
                                <div id='ForChartAndCarToon' style='position: relative; margin-left: 50px; margin-top: -90px; top: 21px; left: -11px;'
                                    runat="server">
                                    <asp:Image ID="imgCartoonAtChart" CheckState='2' Style='z-index: 99; position: absolute; bottom: 70px;'
                                        runat="server" />
                                    <highchart:ColumnChart ID='CartoonChart' Width='600' Height='110' runat="server" />
                                </div>
                            </td>
                            <td style="width: 48%; text-align: right; height: 120px; float: right;">--%>
            <%--<asp:ImageButton ID="ImageButton4" runat="server" src="../images/Activity/Flag.png"
                            Width="100px" Height="100px" />--%>
            <%--  <asp:ImageButton ID="btnNext" runat="server" Style='width: 50px; position: relative; right: 27px; top: 13px;'
                                    src="../images/Activity/next-icon.png" class="btnNext" />
                            </td>
                        </tr>
                    </table>--%>
            <%--    <div style="width: 30px; height: 33px; position: absolute; bottom: 21%; background-image: url('../images/Activity/zoomout.png'); background-repeat: no-repeat; background-position: left; left: 85px; cursor: pointer;"
                        id='hideDivNextPre'>
                    </div>
                </div>
            </div>--%>
        </div>
        <div class="divReviewAns" style='background-image: url(../Images/Activity/BG_Opacity/crLayer90pct.png);'>
            <div style="width: 100%; height: auto; text-align: center">
                <button type="submit" style="width: 20%" id="btnSlideMenu">
                    <img src="../Images/Activity/AllNewArrow/arrow-blue-downFlat.png" height="20px" id="imgUpDown" /></button>
            </div>
            <%--    <input type="button" value='TestJa' style='position:absolute;right:5%;top:35%' />
        <input type="button" value='test' style='position:absolute;left:5%;top:35%' />--%>
            <img class='imgPreNext' id="BackSlide" swap='1' src="../Images/Activity/AllNewArrow/rightBlue.png"
                style='position: absolute; right: 1%; top: 35%; cursor: pointer; z-index: 99;'
                onclick='SwipeCartoon.next();return false;' runat="server" />
            <img class='imgPreNext' id="NextSlide" swap='1' src="../Images/Activity/AllNewArrow/leftBlue.png"
                style='position: absolute; left: 1%; top: 35%; cursor: pointer; z-index: 99; visibility: hidden;'
                onclick='SwipeCartoon.prev();return false;' runat="server" />
            <div class="divReviewSwipe" id="slider3">
                <div>
                    <div>
                        <div id='SwipeCartoon' style='width: 750px; margin-left: auto; margin-right: auto;'>
                            <div id='mainReview' style="margin-top: 35px;" runat="server">
                                <%--div สร้าง ส่วนรีวิวข้อสอบ--%>
                            </div>
                        </div>
                    </div>
                    <div>
                        <div class="divSwipe" style='margin-left: 2.5%;'>
                            <input type="checkbox" id="chkDetail" style="position: absolute; top: 40%; left: 30%;" />
                            <span style="position: absolute; top: 40%; left: 33%;">เลือกถ้าต้องการแสดงเมื่อมีการกรองข้อสอบจากดัชนี
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class='OrangeBack' style="position: absolute; bottom: 0px; width: 100%;left:-1px; height: 150px; text-align: center;font-size:32px;">
                <div style="position: relative; height:150px;">     
                    <%if IsFullScore AndAlso Session("ChooseMode") = BusinessTablet360.EnumDashBoardType.PracticeFromComputer Then %>              
                    <img style="margin-top:15px;"  src="../Images/Activity/goodPracticeleft01.png" alt="" />
                    <img src="../Images/Activity/goodPracticeleft02.png" alt="" />
                    <span style="top:-20px;position:relative;"><%=Score%></span>
                    <img src="../Images/Activity/goodPracticeRight01.png" alt="" />
                    <img src="../Images/Activity/goodPracticeRight02.png" alt="" />    
                    <%Else %>
                    <span style="top:20px;position:relative;"><%=Score%></span>
                    <%End If %>                   
                </div>
                <%If Session("QuizUseTablet").ToString() = "True" Then%>
                <asp:Button ID="btnSortQuestionNormal" CssClass='forBtn' Style='font-size: 19px; position: relative; width: 200px; margin-left: 0px; margin-right: auto; height: 60px;'
                    runat="server" Text="ไล่ทบทวนทีละข้อ" />

                <asp:Button ID="btnSortQuestionMostWrong" CssClass='forBtn' Style='font-size: 19px; position: relative; width: 200px; margin-left: 105px; height: 60px;'
                    runat="server" Text="เริ่มจากข้อที่ผิดมากที่สุด" />
                <%End If%>
                <%Catch%>
                <%Response.Redirect("~/LoginPage.aspx", False)%>
                <%End Try%>
            </div>
        </div>
        <div id="dialog" title="ต้องการออกจากกิจกรรมใช่หรือไม่ ?">
        </div>
        <ul id="Help" runat="server" clientidmode="Static">
            <li class="about2" style="z-index: 99; background: none; border: none; padding-left: 0px;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
                id="HelpLogin">ช่วย<br />
                เหลือ<br />
            </a></li>
        </ul>
        <script type="text/javascript">
            var slider3 = new Swipe(document.getElementById('slider3'));
            var SwipeCartoon = new Swipe(document.getElementById('SwipeCartoon'));
        </script>
    </form>
    <script type="text/javascript">
        var $dialogLogout = $('<div>');
        var myBtn = {};
        myBtn["ยกเลิก"] = function () {
            $dialogLogout.dialog('close');
        };
        myBtn["ตกลง"] = function () {
            window.location = '../loginpage.aspx';
        };
        $dialogLogout.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true, autoOpen: false }).dialog('option', 'title', 'ต้องการออกจาก Pointplus ?');

        $(function () {
            var $btnLogout = $('<ul>', { id: 'btnLogout' }).append($('<li>').append($('<a>', { 'onclick': 'Logout();' })));
            $('body').append($btnLogout);
        });

        function Logout() {
            $dialogLogout.dialog('open');
        }
    </script>
</body>
</html>
<script type="text/javascript">
    $(function () {
        //btn Open QuestionExpain
        var AnswerExpType;
        if ($('#AnswerExp').children('div').length > 0) { AnswerExpType = 1; } // $('#btnAnswerExp').show();
        if ($('ul > li').children('div.Correct').length > 0 || $('ul > li').children('div.InCorrect').length > 0) { AnswerExpType = 6; } // $('#btnAnswerExp').show();

        if ($('#QuestionExp').length > 0 || $('#AnswerExp').children('div').length > 0) { $('#btnQuestionExp').show(); }
        $('#btnQuestionExp').toggle(function () {
            $('#QuestionExp').show();
            if (AnswerExpType == 1) { $('#Table1').hide(); $('#AnswerExp').show(); }
            else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        }, function () {
            $('#QuestionExp').hide();
            if (AnswerExpType == 1) { $('#Table1').show(); $('#AnswerExp').hide(); }
            else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        });
    });
</script>
