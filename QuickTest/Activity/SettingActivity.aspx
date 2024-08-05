<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SettingActivity.aspx.vb"
    Inherits="QuickTest.SettingActivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/prettyLoader.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script src="../js/json2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>"; var FontSize = '<%=Session("FontSize") %>';</script>
    <%If Not IE = "1" Then%>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>
    <script src="../js/ResizeText.js" type="text/javascript"></script>
    <%End If%>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyLoader.js" type="text/javascript"></script>
    <link href="../css/fixMenuSlide.css" rel="stylesheet" />
    <% If BusinessTablet360.ClsKNSession.RunMode = "standalonenotablet" Then%>
    <link href="../css/RoomManagement.css" rel="stylesheet" />
    <script src="../js/RoomManagement.js" type="text/javascript"></script>
    <link href="../css/toggleswitch.css" rel="stylesheet" />
    <script src="../js/jquery.toggleswitch.min.js"></script>
    <script type='text/javascript'>
        jQuery(document).ready(function ($) {
            $('.toggleswitch').toggleSwitch();
        });
    </script>
    <%End If%>
    <%--<style type="text/css">
        ul#ChangeFontSize {
            position: fixed;
            margin: 0px;
            padding: 0px; /*top: 10px;*/
            left: -1px;
            bottom: 190px;
            list-style: none;
            z-index: 9999;
            margin-left: -52px;
        }

            ul#ChangeFontSize li {
                width: 110px;
                height: 150px;
                background-color: #CFCFCF;
                border: 1px solid #AFAFAF;
                -moz-border-radius: 0px 10px 10px 0px;
                -webkit-border-bottom-right-radius: 10px;
                -webkit-border-top-right-radius: 10px;
                -khtml-border-bottom-right-radius: 10px;
                -khtml-border-top-right-radius: 10px;
                border-radius: 0px 10px 10px 0px;
                behavior: url('../css/PIE.htc');
                -pie-track-active: false;
                opacity: 0.6;
                filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
            }

        #btnChangefontsize {
            width: 50px;
            position: absolute;
            text-align: center;
            top: 0;
            right: 0;
        }

            #btnChangefontsize input[type="button"] {
                width: 40px;
                height: 40px;
            }
    </style>--%>
    <%-- <script type="text/javascript">
        $(function () {
            $('#ChangeFontSize').hover(function () {
                $(this).stop().animate({ 'marginLeft': '-2px' }, 200);
            }, function () {
                $(this).stop().animate({ 'marginLeft': '-52px' }, 200);
            });
        });
    </script>--%>
    <link href="../css/LogoutStyle.css" rel="stylesheet" />
    <style type="text/css">
        .selectClass {
            border: 1px solid #CCC;
            background: #F6F6F6;
            font-weight: bold;
            color: #1C94C4;
            border-radius: 6px;
            cursor: pointer;
        }

        .SelectRoom {
            border: 1px solid #CCC;
            background: #F6F6F6;
            font-weight: bold;
            color: #1C94C4;
            border-radius: 6px;
            cursor: pointer;
            margin-left: 5px;
            margin-right: 5px;
        }

        .divAllSetting {
            background-color: #B4DCED;
            width: 50%;
            height: 100%;
            vertical-align: middle;
            -webkit-border-radius: 5em;
            line-height: 3;
            margin-left: auto;
            margin-right: auto;
        }

        /*span
        {
            font-size: larger;
        }*/
        .prettyLoader {
            left: 500px;
            top: 300px;
        }

        input[type=checkbox]:disabled + label {
            color: Gray;
            background-image: url(../images/bullet-disable.gif);
            font-size: 18px;
        }

        .form_settings .submit {
            height: 40px;
            line-height: 40px;
        }

        .txtAddRoom {
            width: 100px;
        }

        /*#DialogAddRoom table td:first-child, #DialogAddRoom div {
            text-align: right;
            padding-right: 20px;
        }

        #DialogAddRoom div {
            padding-bottom: 5px;
        }*/

        #DialogAddRoom table tr td {
            background: none !important;
        }

        #DialogAddRoom .btnAddRoom, #DialogManageRoom .manageRoom, #DialogDeleteRoom .deleteRoom, #DialogChangeNoOfStudent .btnAddRoom {
            font: 20px 'THSarabunNew';
            line-height: 2em;
            border: 0;
            height: 40px;
            line-height: 2em;
            width: 100px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            text-shadow: 1px 1px #178497;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('PIE.htc');
            -pie-track-active: false;
            background: #63cfdf;
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJod…EiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9));
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 );
        }

        .EditRoom {
            width: 120px;
            height: 40px;
            /*float: right;*/
        }

        #DialogManageRoom div, #DialogDeleteRoom div {
            padding: 20px;
            text-align: center;
        }

        .editedRoom {
            background-color: yellow;
        }

        .ui-dialog {
            min-width: 400px;
            width: auto !important;
        }

        .duplicateNumber {
            background: rgb(245, 230, 230);
            border: 1px solid red;
        }

        #site_content {
            padding: 15px 12px 10px 0;
            border-radius: 10px;
        }

        #main {
            background-color: white;
            border-radius: 10px;
            padding-bottom: 10px;
        }
    </style>

    <% If IsAndroid = True Then%>
    <style type="text/css">
        #main {
            width: 885px !important;
            padding-bottom: 0px !important;
        }

        .content, #site_content {
            width: 885px !important;
        }

        #div-1 {
            width: 840px !important;
        }

        footer {
            /*width:850px !important;*/
            display: none;
        }

        #btnOK {
            right: 18px !important;
            width: 200px !important;
            height: 70px !important;
            font-size: 30px !important;
        }

        #BtnBack {
            width: 200px !important;
            height: 70px !important;
            font-size: 30px !important;
            left: 45px !important;
        }

        #btnChageLV, #btnChangeRoom {
            min-width: 60px !important;
            height: 60px !important;
            font-size: 25px !important;
            padding: 0px 5px 0px 5px !important;
        }

        #settingQuizDetail, #spnQuizSetting, #makeQuizDetail, #spnClass, #lblLevel, #spnRoom {
            font-size: 30px !important;
        }

        #DivchkRandomQuestion {
            width: 185px !important;
        }

        #DivchkRandomAnswer {
            width: 255px !important;
        }

        #DivChkInShow1 {
            width: 160px !important;
        }

        #DivIsPerQuestion, #DivIsAll {
            width: 133px !important;
        }
    </style>
    <% End If%>

    <script type="text/javascript">
        var RunModeConfig = '<%= BusinessTablet360.ClsKNSession.RunMode %>';
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var JVDefaultClass = '<%=DefaultClass%>';
        var JVDefaultRoom = '<%=DefaultRoom%>';

        $(document).ready(function () {
            $.prettyLoader({ animation_speed: 'slow', bind_to_ajax: true });
        });
        $(function () {

            //qtip - Tools
            $('#imgCalculator').qtip({
                content: 'เครื่องคิดเลข',
                show: { event: 'mouseover' },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#imgCalculator') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });

            $('#imgNote').qtip({
                content: 'สมุดโน็ต/กระดาษทด',
                show: { event: 'mouseover' },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#imgNote') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });

            $('#imgProtractor').qtip({
                content: 'ไม้โปรแทรคเตอร์',
                show: { event: 'mouseover' },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#imgProtractor') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });

            $('#imgWordBook').qtip({
                content: 'สมุดคำศัพท์',
                show: { event: 'mouseover' },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#imgWordBook') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });

            $('#imgDictionary').qtip({
                content: 'ดิกชันนารี',
                show: { event: 'mouseover' },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#imgDictionary') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });




            //แก้ Layout ถ้าเปลี่ยน Font-size
            //medium size
            if (FontSize == 1) {
                //left panel
                $('.divDetailLeftPanel').css('width', '260px');
                $('#DivchkRandomQuestion').css('width', '180px');
                $('#DivchkRandomAnswer').css('width', '245px');
                //right panel
                $('#DivIsPerQuestion').css('display', 'inline');
                $('#lblForIsPerQuestion').text(' ข้อละ ');
                $('#DivIsAll').css('display', 'inline');
                //$('#spnTimeDetail').text(' นาที (เฉลี่ยข้อละ ');
                $('#spnInDivIsAll').text('ทั้งหมด ');
                //$('#spnTimeDetail2').text('วินาที');
                //$('#spnInTimePerQuestion').text('(รวมทั้งหมด ');
                $('#DivChkInShow1').css('display', 'inline');
                $('#spnSpaceInDivchkRandomQuestion').remove();
            }
                //Large size
            else if (FontSize == 2) {
                //left panel
                $('.divDetailLeftPanel').css('width', '295px');
                $('#DivchkRandomQuestion').css('display', 'inline');
                $('#DivchkRandomAnswer').css('display', 'inline');
                $('#spnSpaceInDivchkRandomQuestion').remove();
                //right panel
                $('#DivIsPerQuestion').css('display', 'inline');
                $('#lblForIsPerQuestion').text('ข้อละ');
                $('#spnSpaceIntxtTimePerQuestion').text('วินาที');
                //$('#spnInTimePerQuestion').text('(รวมทั้งหมด');
                $('#spnMinuteTxtInDivCheckTime').text('นาที');
                $('#spnMinuteTxtInDivCheckTime').next().remove();
                $('#DivIsAll').css('display', 'inline');
                $('#spnInDivIsAll').text('ทั้งหมด');
                //$('#spnTimeDetail').text('นาที (เฉลี่ยข้อละ');
                $('#DivChkInShow1').css('display', 'inline');
            }
            else {
                $('#spnSpaceInDivchkRandomQuestion').remove();
                $('#DivIsAll').css('display', 'inline');
                $('#DivChkInShow1').css('display', 'inline');
            }

            //page transition
            if (!isAndroid) {
                FadePageTransition();
                $(':submit').click(function () {
                    FadePageTransitionOut();
                });
            }

            //hover animation
            InjectionHover('#BtnBack', 5);
            InjectionHover('#btnOK', 5, false);
            InjectionHover('.btnChoose', 3, false);

            //Set ห้อง - ชั้น ถ้าเคยใช้ Quiz นี้แล้ว
            if (JVDefaultClass != '' && JVDefaultRoom != '') {
                $('#lblLevel').text(JVDefaultClass);
                $('#lblRoom').text(JVDefaultRoom);
                $('#HiddenTest').val(JVDefaultClass);
                $('#HiddenRoom').val(JVDefaultRoom);
                GetStudentAmount(JVDefaultClass, JVDefaultRoom);
            }

            //ปุ่มเลือกชั้น
            new FastButton(document.getElementById('btnChageLV'), TriggerChooseLv);

            //ปุ่มเลือกห้อง
            new FastButton(document.getElementById('btnChangeRoom'), TriggerChooseRoom);

            // ปุ่มเพิ่มห้อง
            new FastButton(document.getElementById('btnConfirmAddRoom'), AddNewRoomInClass);
            new FastButton(document.getElementById('btnEditAddRoom'), EditRoom);
            new FastButton(document.getElementById('btnCloseAddRoom'), function () { $('#DialogAddRoom').dialog('close'); });
            // ปุ่มยันยันลบห้อง            
            new FastButton(document.getElementById('btnConfirmDelRoom'), DeleteRoomInClass);
            new FastButton(document.getElementById('btnCancleDelRoom'), function () { $('#DialogDeleteRoom').dialog('close'); });

            //ปุ่มแว่นขยายทางซ้าย
            //new FastButton(document.getElementById('makeQuiz'), TriggerLeftMagnifierOpen);

            //ปุ่มแว่นขยายทางขวา
            //new FastButton(document.getElementById('settingQuiz'), TriggerRightMagnifierOpen)

            //Checkbox
            if (isAndroid) {
                new FastButton(document.getElementById('DivchkQuizUseTablet'), TriggerCheckQuizUseTablet);
                if ($('#DivchkQuizFromSoundlab').length != 0) {
                    new FastButton(document.getElementById('DivchkQuizFromSoundlab'), TriggerCheckQuizFromSoundlab);
                }
                new FastButton(document.getElementById('DivchkRandomQuestion'), TriggerCheckRandomQuestion);
                new FastButton(document.getElementById('DivchkRandomAnswer'), TriggerCheckRandomAnswer);
                new FastButton(document.getElementById('DivDiffQuestion'), TriggerCheckDiffQuestion);
                new FastButton(document.getElementById('DivchkCheckTime'), TriggerCheckTime);
                new FastButton(document.getElementById('DivchkSelfPace'), TriggerCheckSelfPace);
                new FastButton(document.getElementById('DivchkShowAnswer'), TriggerCheckShowAnswer);
                new FastButton(document.getElementById('DivchkShowScore'), TriggerCheckShowScore);
                new FastButton(document.getElementById('DivchkUseTools'), TriggerCheckUseTools);
                //เครื่องคิดเลข
                if ($('#DivchkWithCalculator').length != 0) {
                    new FastButton(document.getElementById('DivchkWithCalculator'), TriggerCheckCalculator);
                }
                //Dictionary
                if ($('#DivchkWithDictionary').length != 0) {
                    new FastButton(document.getElementById('DivchkWithDictionary'), TriggerCheckDictionary);
                }
                //WordBook
                if ($('#DivchkWithWordBook').length != 0) {
                    new FastButton(document.getElementById('DivchkWithWordBook'), TriggerCheckWordBook);
                }
                //Note
                if ($('#DivchkWithNotes').length != 0) {
                    new FastButton(document.getElementById('DivchkWithNotes'), TriggerCheckNote);
                }
                //Note
                if ($('#DivchkWithProtractor').length != 0) {
                    new FastButton(document.getElementById('DivchkWithProtractor'), TriggerCheckProtractor);
                }
                new FastButton(document.getElementById('DivrdbAnswerPerQuestion'), TriggerRDAnswerPerQuestion);
                new FastButton(document.getElementById('DivChkInShow1'), TriggerChkShow1);
                new FastButton(document.getElementById('DivrdbAnswerAfter'), TriggerRDAnswerAfter);
                new FastButton(document.getElementById('DivrdbByStep'), TriggerRDByStep);
                new FastButton(document.getElementById('DivrdbEndQuiz'), TriggerRDEndQuiz);
                new FastButton(document.getElementById('DivIsPerQuestion'), TriggerRDIsPerQuestion);
                new FastButton(document.getElementById('DivIsAll'), TriggerRDIsAll);
            }

            //ปุ่ม กลับ,เริ่มกันเลย
            $('.submit').each(function () {
                new FastButton(this, TriggerClick);
            });

            $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#Help > li').hover(function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
            });

            //Check WebConfig RunMode
            if (RunModeConfig == 'standalonenotablet') {
                $('#chkQuizUseTablet').attr('checked', false);
                $("label[for=chkQuizUseTablet]").hide();
                $('#imgTablet').hide();
                $('#DivFromSoundlab').css('display', 'none');
                $('#chkSelfPace').attr('checked', false);
                $('#chkShowScore').attr('checked', false);
                $('#chkUseTools').attr('checked', false);
                $("label[for=chkSelfPace]").hide();
                $('#imgSelfPace').hide();
                $('#imgSelfPace').next().hide();
                $("label[for=chkShowScore]").hide();
                $('#imgShowScore').hide();
                $('#imgShowScore').next().hide();
                $("label[for=chkUseTools]").hide();
                $('#imgToolsQuiz').hide();
                $('#imgToolsQuiz').next().hide();
            }
            else if (RunModeConfig == 'studenttablet') {
                $('#chkQuizFromSoundlab').attr('checked', false);
                $('#DivFromSoundlab').remove();
            }
            else if (RunModeConfig == 'labonly') {
                $('#chkQuizUseTablet').attr('checked', 'checked');
                $('#chkQuizFromSoundlab').attr('checked', 'checked');
                $('#DDLSoundLabName').show();
                ChkTabletLabAmount();
            }

            //ดักถ้าเข้ากับ Tablet ของครู
            if (isAndroid) {

                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                //$('#main').css('width', '885px');
                //$('.content').css('width', '885px');
                //$('#site_content').css('width', '885px');
                //$('#div-1').css('width', '840px');
                //$('footer').css('width', '850px');
                //$('#btnOK').css({'right': '18px','width':'200px','height':'70px','font-size':'30px'});
                //$('#BtnBack').css({ 'width': '200px', 'height': '70px', 'font-size': '30px', 'left': '45px' });
                //$('#btnChageLV').css({ 'width': '60px', 'height': '60px', 'font-size': '25px' });
                //$('#btnChangeRoom').css({ 'width': '60px', 'height': '60px', 'font-size': '25px' });
                //$('#settingQuizDetail').css('font-size', '30px');
                //$('#spnQuizSetting').css('font-size', '30px');
                //$('#makeQuizDetail').css('font-size', '23px');
                //$('#spnClass').css('font-size', '30px');
                //$('#lblLevel').css('font-size', '30px');
                //$('#spnRoom').css('font-size', '30px');
                //$('#DivchkRandomQuestion').css('width', '185px');
                //$('#DivchkRandomAnswer').css('width', '255px');
                //$('#DivChkInShow1').css('width', '160px');
                //$('#DivIsPerQuestion').css('width', '133px');
                //$('#DivIsAll').css('width', '133px');

                //$('#spnQuizDetail').css('font-size', '22px');
                //$('#lblQuestionAmount').css('font-size', '21px');
                //$('#spnQuestionAmount').css('font-size', '22px');
            }

            ShowHideTimechk();
            ShowHideShowAnswer();

            $('#DialogLV').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                width: 'auto'
            });

            $('#DialogRoom').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                width: 'auto'
            });

            $('#DialogAddRoom').dialog({ autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto' });
            $('#DialogManageRoom').dialog({ autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto' });
            $('#DialogDeleteRoom').dialog({ autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto' });

            //$('#btnChageLV').click(function () {
            //    $('#DialogLV').dialog('open');
            //    $('#lblRoom').text('');
            //});

            //$('#btnChangeRoom').click(function () {

            //    $('#DialogRoom').dialog('open');

            //});

            //            $('input[type=radio]').change(function(){ 

            //                if($('#rdbAnswerAfter').attr('checked') == 'checked'){                   
            //                    $('#ChkInShow1').attr('disabled',true);
            //                    $('#ChkInShow1').attr('checked',false);
            //                    $('#txtTimeShowAnswer').attr('disabled',true);
            //                }
            //                else{
            //                    $('#ChkInShow1').attr('disabled',false);
            //                    $('#txtTimeShowAnswer').attr('disabled',false);
            //                }
            //            });        

            $('.radioShowAnswer').change(function () {
                if ($('#rdbAnswerAfter').attr('checked') == 'checked') {
                    $('#ChkInShow1').attr('disabled', true);
                    $('#ChkInShow1').attr('checked', false);
                    $('#txtTimeShowAnswer').attr('disabled', true);
                } else {
                    $('#ChkInShow1').attr('disabled', false);
                    $('#txtTimeShowAnswer').attr('disabled', true);
                }
            });

            // เมื่อแก้เลขที่ช่องที่เป็นนักเรียนเลขซ้ำ
        });

        function TriggerChooseLv() {
            var divClose = '<div id="closeDialog" style= "position:absolute;right:5px;top:5px;width: 25px;height: 25px;background-color: white;float:right;border-radius: 50%;text-align: center;font-size: 15px;color: orange;font-weight: bold;cursor:pointer;" >X</div>';
            $('#DialogLV').dialog('open').dialog('option', 'title', 'เลือกชั้นเรียนที่ต้องการ ' + divClose);
            if ($('.selectClass').length != 0) {
                $('.selectClass').each(function () {
                    new FastButton(this, TriggerButtonChooseLV);
                });
            }
            $('#lblRoom').text('');
        }

        function TriggerButtonChooseLV(e) {
            var obj = e.target;
            var lv = $(obj).attr('lv');
            ChangeLv(lv);
        }

        function TriggerChooseRoom() {
            SetRoomDialog($('#lblLevel').text());
            var divClose = '<div id="closeDialog" style= "position:absolute;right:5px;top:5px;width: 25px;height: 25px;background-color: white;float:right;border-radius: 50%;text-align: center;font-size: 15px;color: orange;font-weight: bold;cursor:pointer;" >X</div>';
            if ($('#btnAddRoom').length != 0) {
                new FastButton(document.getElementById('btnAddRoom'), TriggerAddRoom);
                //var toggleSwitch = "<span style='margin-left: 90px;'>แก้ไข</span><input type='checkbox' name='opt1' id='opt1' value='1' class='toggleswitch' checked='checked' /></div>";
                var toggleSwitch = "<span style='float: right;right: 100px;position: absolute;font-size: 18px;font-weight: initial;top: 20px;'>แก้ไข</span><input type='checkbox' name='opt1' id='opt1' value='0' class='toggleswitch' /></div>";
                //$('#DialogRoom').dialog('option', 'title', 'เลือกห้องเรียนที่ต้องการ <input type="button" value="แก้ไขห้อง" class="EditRoom" id="btnManageRoom" />' + divClose);
                $('#DialogRoom').dialog('option', 'title', '<div style="width:350px"><span id="spnMode">เลือกห้องทำควิซ</span>' + toggleSwitch + divClose);
                //new FastButton(document.getElementById('btnManageRoom'), TriggerMangeRoom);

                $('.toggleswitch').toggleSwitch({
                    onChangeOn: function () {
                        $('#spnMode').text('เลือกห้องที่จะแก้ไข');
                        $('.switched').css('left', '2px!important');
                        SetRoomDialog($('#lblLevel').text());
                        new FastButton(document.getElementById('btnAddRoom'), TriggerAddRoom);
                        TriggerMangeRoomToggle(1);
                        console.log('on');
                    }, onChangeOff: function () {
                        $('#spnMode').text('เลือกห้องทำควิซ');
                        SetRoomDialog($('#lblLevel').text());
                        new FastButton(document.getElementById('btnAddRoom'), TriggerAddRoom);
                        TriggerMangeRoomToggle(0);
                        console.log('off');
                    }
                });
            } else {
                $('#DialogRoom').dialog('option', 'title', '<div style="width:350px"><span id="spnMode">เลือกห้องทำควิซ</span>' + divClose);
            }
            $('#DialogRoom').dialog('open');
            if ($('.SelectRoom').length != 0) {
                $('.SelectRoom').each(function () {
                    new FastButton(this, TriggerButtonChooseRoom);
                });
                //$('#btnManageRoom').show();                
            } else {
                //$('#btnManageRoom').hide();
                $('.switch').hide();
                $('.switch').prev().hide();
            }
            isRoomManagementCallback = false;
        }

        // ปุ่มกด ปิด dialog สุ่มข้อสอบ
        $('#closeDialog').live('click', function () {
            $('#DialogRoom').dialog('close');
            $('#DialogLV').dialog('close');
        });

        // function เพิ่มห้อง          
        function TriggerAddRoom() {
            $('#txtRoomName').val('');
            //$('#txtNoOfStudent').val('');
            $('#btnEditAddRoom').hide();
            $('#btnConfirmAddRoom').show();
            $('#DialogAddRoom').dialog('open').dialog('option', 'title', 'เพิ่มห้องเรียนในชั้น ' + $('#lblLevel').text() + " / <input type='text' id='txtRoomName' class='txtAddRoom' maxlength='2' onkeypress='return event.charCode >= 48 && event.charCode <= 57' />");
            NewStudents(60);
            $('#DialogRoom').dialog('close');
        }
        function AddNewRoomInClass(e) {
            e.preventDefault();
            var ClassName = $('#lblLevel').text();
            var RoomName = $('#txtRoomName').val();
            var students = GetJsonStudents();
            if (RoomName == "") {
                //alert("ใส่เลขห้องก่อนค่ะ");
                callDialogAlert("ใส่เลขห้องก่อนค่ะ");
                return false;
            }
            if (students == null) {
                //alert("ใส่รหัสนักเรียนก่อนค่ะ! (อย่างน้อย 1 คนค่ะ)");
                callDialogAlert("ใส่รหัสนักเรียนก่อนค่ะ! (อย่างน้อย 1 คนค่ะ)");
                return false;
            }
            if (CheckStudentsOrder(students)) {
                callDialogAlert("ใส่รหัสนักเรียนให้ครบด้วยค่ะ โดยต้องเรียงและห้ามข้ามเลขที่ค่ะ");
                return false;
            }
            if (CheckDuplicateStudent(students)) {
                callDialogAlert("มีรหัสนักเรียนซ้ำกันในห้องนี้ค่ะ");
                return false;
            }
            students = JSON.stringify(students);
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/AddNewRoom",
                async: false,
                data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "' , NoOfStudent : '" + students + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    isRoomManagementCallback = true;
                    console.log(msg.d);
                    if (msg.d === "NOT") {
                        callDialogAlert('เกิดข้อผิดพลาดค่ะ');
                        return false;
                    }
                    if (msg.d === "ExistRoom") {
                        callDialogAlert("มีห้องเรียนนี้อยู่ในระบบแล้วค่ะ!");
                        return false;
                    }
                    if (msg.d === "False") { // save ไม่ผ่าน
                        alert('save ไม่ผ่าน');
                        return false;
                    } else if (msg.d === "True") { // success
                        $('#txtRoomName').val('');
                        $('#txtNoOfStudent').val('');
                        $('#DialogAddRoom').dialog('close');
                        callDialogAlert("เพิ่มห้องและนักเรียนเรียบร้อยแล้วค่ะ");
                        ChangeLv(ClassName);
                        ChangeRoom(RoomName);
                        //$('#lblRoom').text('');
                    } else { // case studentid ซ้ำ
                        var data = JSON.parse(msg.d);
                        var j = GetJsonLength(data);
                        console.log(j);
                        for (var i = 0; i < j; i++) {
                            var txt = "#txtnumber" + data.students[i].number;
                            $(txt).addClass('duplicateNumber');
                        }
                        callDialogAlert("มีรหัสนักเรียนซ้ำกันในห้องอื่นค่ะ!");
                        return false;
                    }
                },
                error: function myfunction(request, status) {
                }
            });
        }
        // เช็คว่ากรอกเลขที่ ครบมั้ย เช่น เลขที่ 1-n ห้ามเว้น
        function CheckStudentsOrder(data) {
            var j = GetJsonLength(data);
            console.log(' number = ' + data.students[j - 1].number + ' , j = ' + j);
            if (j != data.students[j - 1].number) {
                return true;
            } //else {
            //    
            //    return true;
            //}
            return false;
        }

        // เช็คเลขที่ ซ้ำกันตอนกรอก ก่อนไป save ลง DB
        function CheckDuplicateStudent(data) {
            var j = GetJsonLength(data);
            var arr = [];
            for (var i = 0; i < j; i++) {
                var txt = "#txtnumber" + data.students[i].number;
                $(txt).removeClass('duplicateNumber');
                var sid = data.students[i].studentid;
                for (var k = i + 1; k < j; k++) {
                    if (sid == data.students[k].studentid) {
                        arr.push(sid);
                    }
                }
            }
            if (arr.length > 0) {
                SetDuplicateStudentTextbox(data, arr);
                return true;
            }
            return false;
        }
        // ทำ textbox ให้เป็นขอบสีแดง ถ้ารหัสนักเรียนซ้ำกัน
        function SetDuplicateStudentTextbox(data, s) {
            var j = GetJsonLength(data);
            for (var i = 0; i < j; i++) {
                var sid = data.students[i].studentid;
                for (var k = 0; k < s.length; k++) {
                    if (sid == s[k]) {
                        var txt = "#txtnumber" + data.students[i].number;
                        $(txt).addClass('duplicateNumber');
                    }
                }
            }
        }
        // เรียก dialog ที่เกียวกับการ alert
        function callDialogAlert(title) {
            $('#DialpgAlert').dialog('option', 'title', title).dialog('open');
        }
        // Is number
        function IsNumeric(input) {
            return (input - 0) == input && ('' + input).replace(/^\s+|\s+$/g, "").length > 0;
        }
        // เปลี่ยนปุ่มเลือกห้องเป็นปุ่มแก้ไข
        function TriggerMangeRoom() {
            if ($('#btnManageRoom').val() == "แก้ไขห้อง") {
                $('#btnManageRoom').val("โหมดปกติ");
                if ($('.SelectRoom').length != 0) {
                    $('.SelectRoom').each(function () {
                        $(this).addClass('editedRoom');
                        new FastButton(this, TriggerManageRoom);
                    });
                }
                $('#btnAddRoom').hide();
            } else {
                $('#btnManageRoom').val("แก้ไขห้อง");
                if ($('.SelectRoom').length != 0) {
                    $('.SelectRoom').each(function () {
                        $(this).removeClass('editedRoom');
                        new FastButton(this, TriggerButtonChooseRoom);
                    });
                }
                $('#btnAddRoom').show();
            }
        }
        function TriggerMangeRoomToggle(mode) {
            if ($('.SelectRoom').length != 0) {
                console.log(mode);
                if (mode == 1) {
                    $('#btnAddRoom').hide();
                    $('.SelectRoom').each(function () {
                        $(this).addClass('editedRoom');
                        new FastButton(this, TriggerManageRoom);
                    });
                } else {
                    $('#btnAddRoom').show();
                    $('.SelectRoom').each(function () {
                        $(this).removeClass('editedRoom');
                        this.removeEventListener('touchstart', this, false);
                        this.removeEventListener('click', this, false);
                        new FastButton(this, TriggerButtonChooseRoom);
                    });
                }
            }
        }
        // เปิด Dialog ManageRoom
        var RoomName;
        function TriggerManageRoom(e) {
            $('#DialogRoom').dialog('close');
            var obj = e.target;
            RoomName = $(obj).attr('room');
            $('#divMangeRoom').html('');
            var btnEdit = "<input type='button' id='btnEditRoom' value='แก้ไข' class='manageRoom' style='margin-right: 50px;' />";
            var btnDel = "<input type='button' id='btnDeleteRoom' value='ลบ' class='manageRoom' />";
            $('#divMangeRoom').append(btnEdit).append(btnDel);
            new FastButton(document.getElementById('btnEditRoom'), TriggerEditRoom);
            new FastButton(document.getElementById('btnDeleteRoom'), TriggerDeleteRoom);
            var ClassName = $('#lblLevel').text();
            $('#DialogManageRoom').dialog('open').dialog('option', 'title', 'จัดการห้อง ' + ClassName + '/' + RoomName);
        }
        // Edit Room
        var NoOfStudent;
        function TriggerEditRoom() {
            $('#txtRoomName').val(RoomName);
            NoOfStudent = GetNoOfStudent();
            //$('#txtNoOfStudent').val(NoOfStudent);
            $('#btnConfirmAddRoom').hide();
            $('#btnEditAddRoom').show();
            $('#DialogAddRoom').dialog('open').dialog('option', 'title', 'แก้ไขห้องเรียนชั้น ' + $('#lblLevel').text() + ' / ' + "<input type='text' id='txtRoomName' class='txtAddRoom' maxlength='2' onkeypress='return event.charCode >= 48 && event.charCode <= 57' value='" + RoomName + "' />");
            console.log(NoOfStudent);
            var data = JSON.parse(NoOfStudent);
            var n = GetJsonLength(data);
            var lastnumber = data.students[n - 1].number;
            //if (lastnumber < 20) { n = 20; } else if (lastnumber > 20 && lastnumber <= 40) { n = 40; } else { n = 60; }
            n = 60;
            UpdateStudent(n, data);
            $('#DialogManageRoom').dialog('close');
        }
        function EditRoom() {
            var ClassName = $('#lblLevel').text();
            var NewRoomName = $('#txtRoomName').val();
            var NewNoOfStudent = GetJsonStudents();
            if (NewRoomName == "") {
                callDialogAlert("ใส่เลขห้องก่อนค่ะ");
                return false;
            }
            if (NewNoOfStudent == null) {
                callDialogAlert("ใส่รหัสนักเรียนก่อนค่ะ! (อย่างน้อย 1 คนค่ะ)");
                return false;
            }
            if (CheckStudentsOrder(NewNoOfStudent)) {
                callDialogAlert("ใส่รหัสนักเรียนให้ครบด้วยค่ะ โดยต้องเรียงและห้ามข้ามเลขที่ค่ะ");
                return false;
            }
            if (CheckDuplicateStudent(NewNoOfStudent)) {
                callDialogAlert("มีรหัสนักเรียนซ้ำกันในห้องนี้ค่ะ");
                return false;
            }
            NewNoOfStudent = JSON.stringify(NewNoOfStudent);
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/UpdateRoom",
                async: false,
                data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "', NoOfStudent : '" + NoOfStudent + "', NewNoOfStudent : '" + NewNoOfStudent + "', NewRoomName : '" + NewRoomName + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    console.log('return (UpdateRoom) = ' + msg.d);
                    isRoomManagementCallback = true;
                    if (msg.d === "False") {
                        alert('save ไม่ผ่าน');
                        return false;
                    }
                    if (msg.d === "ExistRoom") {
                        callDialogAlert("มีห้องเรียนนี้อยู่ในระบบแล้วค่ะ!");
                        return false;
                    }
                    if (msg.d === "True") {
                        $('#DialogAddRoom').dialog('close');
                        ChangeLv(ClassName);
                        ChangeRoom(NewRoomName);
                        //$('#lblRoom').text('');
                        callDialogAlert("แก้ไขเรียบร้อยแล้วค่ะ");
                    } else { // นักเรียนที่แก้ไขไปใหม่ มีรหัสซ่้ำ
                        var data = JSON.parse(msg.d);
                        var j = GetJsonLength(data);
                        for (var i = 0; i < j; i++) {
                            var txt = "#txtnumber" + data.students[i].number;
                            $(txt).addClass('duplicateNumber');
                        }
                        callDialogAlert("มีรหัสนักเรียนซ้ำกันในห้องอื่นค่ะ!");
                        return false;
                    }
                },
                error: function myfunction(request, status) {
                }
            });
        }
        // หาจำนวนนักเรัยน
        function GetNoOfStudent() {
            var n;
            var ClassName = $('#lblLevel').text();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetNoOfStudentInRoom",
                async: false,
                data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    n = msg.d;
                },
                error: function myfunction(request, status) {
                }
            });
            return n;
        }
        // เปิด Dialog Delete Room
        function TriggerDeleteRoom() {
            var ClassName = $('#lblLevel').text();
            $('#DialogDeleteRoom').dialog('open').dialog('option', 'title', 'ต้องการลบห้อง  ' + ClassName + '/' + RoomName);
            $('#DialogManageRoom').dialog('close');
        }
        // ลบ Room
        function DeleteRoomInClass() {
            var ClassName = $('#lblLevel').text();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/DeleteRoom",
                async: false,
                data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d) {
                        //alert('ลบแล้ว');                       
                        $('#DialogDeleteRoom').dialog('close');
                        ChangeLv(ClassName);
                        $('#lblRoom').text('');
                    }
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function TriggerButtonChooseRoom(e) {
            var obj = e.target;
            var room = $(obj).attr('room');
            ChangeRoom(room);
        }

        function TriggerClick(e) {
            var obj = e.target;
            $(obj).trigger('click');
        }

        function TriggerLeftMagnifierOpen(e) {
            var PicSetting = $('#settingQuizDetailPic');
            var itemSetting = $('#settingQuizDetailPic').children('div');
            var checked = $('#chkUseTools').attr('checked');

            if ($(e.target).is($('#makeQuiz'))) {
                $('#makeQuiz').toggleClass('settingSummary');
                if ($('#makeQuiz').hasClass('settingSummary')) {
                    swipePanelLR($('#makeQuiz'), $('#settingQuiz'));
                    // update 29-04-56 Tools
                    if (checked) {
                        $(PicSetting).css('width', '100%');
                        $(itemSetting).removeClass('itemSettingQuiz');
                        $(itemSetting).addClass('itemSettingQuizWithTools');
                    }
                    else {
                        $(PicSetting).css('width', '345px');
                        $(itemSetting).removeClass('itemSettingQuizWithTools');
                        $(itemSetting).addClass('itemSettingQuiz');
                        resizeWithItemTools(false);
                    }
                }
                else {
                    swipePanelSelf($('#makeQuiz'), $('#settingQuiz'));
                    // update 29-04-56 Tools
                    if (checked) {
                        $(PicSetting).css('width', '345px');
                        $(itemSetting).removeClass('itemSettingQuizWithTools');
                        $(itemSetting).addClass('itemSettingQuiz');
                    }
                }
            }
        };

        function TriggerRightMagnifierOpen(e) {
            if ($(e.target).is($('#settingQuiz'))) {
                $('#settingQuiz').toggleClass('settingSummary');
                if ($('#settingQuiz').hasClass('settingSummary')) {
                    swipePanelLR($('#settingQuiz'), $('#makeQuiz'));
                }
                else {
                    swipePanelSelf($('#settingQuiz'), $('#makeQuiz'));
                }
                // update 29-04-56 Tools  
                var checked = $('#chkUseTools').attr('checked');
                resizeWithItemTools(checked);
            }
        }

        function TriggerCheckQuizUseTablet() {
            if ($('#chkQuizUseTablet').attr('checked') == 'checked') {
                $('#chkQuizUseTablet').removeAttr('checked');
                $('#chkQuizUseTablet').trigger('change');
            }
            else {
                $('#chkQuizUseTablet').attr('checked', 'checked');
                $('#chkQuizUseTablet').trigger('change');
            }
        }

        function TriggerCheckQuizFromSoundlab() {
            if ($('#chkQuizFromSoundlab').attr('checked') == 'checked') {
                $('#chkQuizFromSoundlab').removeAttr('checked');
                $('#chkQuizFromSoundlab').trigger('click');
            }
            else {
                $('#chkQuizFromSoundlab').attr('checked', 'checked');
                $('#chkQuizFromSoundlab').trigger('click');
            }
        }

        function TriggerCheckRandomQuestion() {
            if ($('#chkRandomQuestion').attr('checked') == 'checked') {
                $('#chkRandomQuestion').removeAttr('checked');
                ShowHideShowDiffQuestion();
            }
            else {
                $('#chkRandomQuestion').attr('checked', 'checked');
                ShowHideShowDiffQuestion();
            }
        }

        function TriggerCheckRandomAnswer() {
            if ($('#chkRandomAnswer').attr('checked') == 'checked') {
                $('#chkRandomAnswer').removeAttr('checked');
                ShowHideShowDiffQuestion();
            }
            else {
                $('#chkRandomAnswer').attr('checked', 'checked');
                ShowHideShowDiffQuestion();
            }
        }

        function TriggerCheckDiffQuestion() {
            if ($('#chkDiffQuestion').attr('checked') == 'checked') {
                $('#chkDiffQuestion').removeAttr('checked');
                $('#chkDiffQuestion').trigger('click');
            }
            else {
                $('#chkDiffQuestion').attr('checked', 'checked');
                $('#chkDiffQuestion').trigger('click');
            }
        }

        function TriggerCheckTime() {
            if ($('#chkCheckTime').attr('checked') == 'checked') {
                $('#chkCheckTime').removeAttr('checked');
                $('#chkCheckTime').trigger('click');

            }
            else {
                $('#chkCheckTime').attr('checked', 'checked');
                $('#chkCheckTime').trigger('click');
            }
        }

        function TriggerCheckSelfPace() {
            if ($('#chkSelfPace').attr('checked') == 'checked') {
                $('#chkSelfPace').removeAttr('checked');
                $('#chkSelfPace').trigger('click');
            }
            else {
                $('#chkSelfPace').attr('checked', 'checked');
                $('#chkSelfPace').trigger('click');
            }
        }

        function TriggerCheckShowAnswer() {
            if ($('#chkShowAnswer').attr('checked') == 'checked') {
                $('#chkShowAnswer').removeAttr('checked');
                $('#chkShowAnswer').trigger('click');
            }
            else {
                $('#chkShowAnswer').attr('checked', 'checked');
                $('#chkShowAnswer').trigger('click');
            }
        }

        function TriggerCheckShowScore() {
            if ($('#chkShowScore').attr('checked') == 'checked') {
                $('#chkShowScore').removeAttr('checked');
                $('#chkShowScore').trigger('click');
            }
            else {
                $('#chkShowScore').attr('checked', 'checked');
                $('#chkShowScore').trigger('click');
            }
        }

        function TriggerCheckUseTools() {
            if ($('#chkUseTools').attr('checked') == 'checked') {
                $('#chkUseTools').removeAttr('checked');
                $('#chkUseTools').trigger('click');
            }
            else {
                $('#chkUseTools').attr('checked', 'checked');
                $('#chkUseTools').trigger('click');
            }
        }

        function TriggerCheckCalculator() {
            if ($('#chkWithCalculator').attr('checked') == 'checked') {
                $('#chkWithCalculator').removeAttr('checked');
            }
            else {
                $('#chkWithCalculator').attr('checked', 'checked');
            }
        }

        function TriggerCheckDictionary() {
            if ($('#chkWithDictionary').attr('checked') == 'checked') {
                $('#chkWithDictionary').removeAttr('checked');
            }
            else {
                $('#chkWithDictionary').attr('checked', 'checked');
            }
        }

        function TriggerCheckWordBook() {
            if ($('#chkWithWordBook').attr('checked') == 'checked') {
                $('#chkWithWordBook').removeAttr('checked');
            }
            else {
                $('#chkWithWordBook').attr('checked', 'checked');
            }
        }

        function TriggerCheckNote() {
            if ($('#chkWithNotes').attr('checked') == 'checked') {
                $('#chkWithNotes').removeAttr('checked');
            }
            else {
                $('#chkWithNotes').attr('checked', 'checked');
            }
        }

        function TriggerCheckProtractor() {
            if ($('#chkWithProtractor').attr('checked') == 'checked') {
                $('#chkWithProtractor').removeAttr('checked');
            }
            else {
                $('#chkWithProtractor').attr('checked', 'checked');
            }
        }

        function TriggerRDAnswerPerQuestion() {
            if ($('#rdbAnswerPerQuestion').attr('checked') != 'checked') {
                $('#rdbAnswerPerQuestion').attr('checked', 'checked');
                $('#rdbAnswerPerQuestion').trigger('click');
                $('.radioShowAnswer').trigger('change');
            }

        }

        function TriggerChkShow1() {
            if ($('#ChkInShow1').attr('checked') == 'checked') {
                $('#ChkInShow1').removeAttr('checked');
                $('#ChkInShow1').trigger('click');
            }
            else {
                $('#ChkInShow1').attr('checked', 'checked');
                $('#ChkInShow1').trigger('click');
            }
        }

        function TriggerRDAnswerAfter() {
            if ($('#rdbAnswerAfter').attr('checked') != 'checked') {
                $('#rdbAnswerAfter').attr('checked', 'checked');
                $('#rdbAnswerAfter').trigger('click');
                $('.radioShowAnswer').trigger('change');
            }
        }

        function TriggerRDByStep() {
            if ($('#rdbByStep').attr('checked') != 'checked') {
                $('#rdbByStep').attr('checked', 'checked');
                $('#rdbByStep').trigger('click');
            }
        }

        function TriggerRDEndQuiz() {
            if ($('#rdbEndQuiz').attr('checked') != 'checked') {
                $('#rdbEndQuiz').attr('checked', 'checked');
                $('#rdbEndQuiz').trigger('click');
            }
        }

        function TriggerRDIsPerQuestion() {
            if ($('#IsPerQuestion').attr('checked') != 'checked') {
                $('#IsPerQuestion').attr('checked', 'checked');
                $('#IsPerQuestion').trigger('click');

            }
        }

        function TriggerRDIsAll() {
            if ($('#IsAll').attr('checked') != 'checked') {
                $('#IsAll').attr('checked', 'checked');
                $('#IsAll').trigger('click');
            }
        }

        function ChangeLv(level) {
            //console.log(level);
            $('#txtTotalStudent').html('');
            $('#lblLevel').text(level);
            $('#HiddenTest').val(level);
            $('#DialogLV').dialog('close');
            SetRoomDialog(level);
        }

        function ChangeRoom(Room) {
            $('#<%= lblRoom.ClientID %>').text(Room);
            $('#DialogRoom').dialog('close');
            var ClassName = $('#lblLevel').text();
            var RoomName = $('#lblRoom').text();
            $('#HiddenRoom').val(RoomName);
            GetStudentAmount(ClassName, RoomName);
        }




        function ShowHideInShowAnswer() {

            if ($('#Show1').attr('checked') == 'checked') {
                $('#ForShow1').stop(true, true).show(500);
            }
            else {
                $('#ForShow1').stop(true, true).hide(500);
            }


        };


        function SetRoomDialog(ClassName) {

            var a = document.getElementById('TRRoom');
            a.innerHTML = '&nbsp';
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetHtmlRoom",
                async: false, // ทำงานให้เสร็จก่อน
                data: "{ClassName : '" + ClassName + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        valReturnFromCodeBehide = msg.d;
                        a.innerHTML = valReturnFromCodeBehide;
                    }
                },
                error: function myfunction(request, status) {

                }
            });
        }


        function GetStudentAmount(ClassName, RoomName) {
            $('#DDLSoundLabName').empty();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetStudentAmountCodeBehide",
                    async: false, // ทำงานให้เสร็จก่อน
                    data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            var s = jQuery.parseJSON(msg.d);

                            var sel = document.getElementById('DDLSoundLabName');

                            var TabletLab = s.Detail;

                            if (TabletLab[0] == '1') {

                                for (var i = 1; i < TabletLab.length; i++) {
                                    var opt = document.createElement('option');
                                    opt.value = TabletLab[i];
                                    opt.innerText = TabletLab[i + 1];

                                    i += 1;

                                    sel.appendChild(opt);
                                }
                            }
                            else {
                                $('#DDLSoundLabName').hide
                                $('#chkQuizFromSoundlab').next("Label:first").text('ใช้กับห้องปฏิบัติการ (แท็บเล็ตแล็บ) ไม่มีห้องที่มีจำนวนโต๊ะพอ');
                                $('#chkQuizFromSoundlab').attr('checked', false).attr('disabled', true);
                            }

                            if (s.IsDuplicate == "True") {
                                $('#DialogDuplicateQuiz').html(s.TextHtml).dialog({
                                    autoOpen: false,
                                    resizable: false,
                                    modal: true,
                                    buttons: {
                                        "เปิดใหม่": function () {
                                            $('#txtTotalStudent ').html(s.NoOfStudent);
                                            $(this).dialog('close');
                                        },
                                        "ยกเลิก": function () {
                                            $('#lblRoom').text('');
                                            $('#HiddenRoom').val('');
                                            $('#txtTotalStudent ').html('');
                                            $(this).dialog('close');
                                        }
                                    }
                                }).dialog('open');
                            } else {
                                $('#txtTotalStudent ').html(s.NoOfStudent);
                            }
                        }
                    },
                    error: function myfunction(request, status) {

                    }
                });
            }
            function GetTime(TimePer, StaPer) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetStudentAmountCodeBehide",
                async: false, // ทำงานให้เสร็จก่อน
                data: "{Time : '" + Time + "', StaPer : '" + StaPer + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    valReturnFromCodeBehide = msg.d;
                    $('#txtTotalStudent ').val(valReturnFromCodeBehide);
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function CloseHelpPanel() {
            $.fancybox.close();
        }

    </script>
    <script type="text/javascript">
        $(function () {
            //$('#txtTimePerQuestion').keyup(function (e) {   
            $('.txtTime').keyup(function (e) {
                var delayID = null;
                var idTxt = $(this).attr('id');
                if (delayID == null) {
                    delayID = setTimeout(function () {
                        if (idTxt == 'txtTimePerQuestion') {
                            checkTimerPerQuestionSec($('#txtTimePerQuestion').val());
                        }
                        else if (idTxt == 'txtTimeAll') {
                            checkTimeAllQuestion($('#txtTimeAll').val());
                        }
                        else if (idTxt == 'txtTimeShowAnswer') {
                            checkTimerSolve($('#txtTimeShowAnswer').val());
                        }
                    }, 2000);
                }
                else if (delayID != null) {
                    clearTimeout(delayID);
                    delayID = setTimeout(function () {
                        if (idTxt == 'txtTimePerQuestion') {
                            checkTimerPerQuestionSec($('#txtTimePerQuestion').val());
                        }
                        else if (idTxt == 'txtTimeAll') {
                            checkTimeAllQuestion($('#txtTimeAll').val());
                        }
                        else if (idTxt == 'txtTimeShowAnswer') {
                            checkTimerSolve($('#txtTimeShowAnswer').val());
                        }
                    }, 2000);
                }
            });

            $('#txtTimePerQuestion').focusout(function () {
                if ($(this).val() == "") {
                    $(this).val(10);
                    checkTimerPerQuestionSec($(this).val());
                }
            });
            $('#txtTimeAll').focusout(function () {
                if ($(this).val() == "") {
                    $(this).val(30);
                    checkTimeAllQuestion($(this).val());
                }
            });
            $('#txtTimeShowAnswer').focusout(function () {
                if ($(this).val() == "") {
                    $(this).val(30);
                }
            });

        });

        function checkTimerPerQuestionSec(timerPerQuestion) {
            var timer = parseInt(timerPerQuestion); // ค่าจาก textbox เวลาต่อข้อ
            if (isNaN(timer)) { $('#lblTimeAllPerQuestion').html(""); return false; }
            var questionAmount = parseInt($('#lblQuestionAmount').html()); // จำนวนข้อ          
            if (timer < 10) {
                $('#txtTimePerQuestion').val(10);
                $('#txtTimePerQuestion').qtip({
                    content: 'ต้องกรอกเวลาทำควิซข้อละ 10 วินาทีขึ้นไปค่ะ',
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimePerQuestion') },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                //var timerPerQ = ((10 * questionAmount) / 60);//.toFixed(2);                
                //$('#lblTimeAllPerQuestion').html(timerPerQ);
                //DestroyQtip($('#txtTimePerQuestion'));
                timer = 10;
                DestroyQtip($('#txtTimePerQuestion'));
            }
            //else {
            //    var timerPerQ = ((timerPerQuestion * questionAmount) / 60);//.toFixed(2);
            //    $('#lblTimeAllPerQuestion').html(timerPerQ);
            //}
            var t = questionAmount * timer;
            $('#lblTimeAllPerQuestion').html("(ทั้งหมด " + GetTimeString(t) + ")");

            $('#hdTimePerQuestion').val(timer);
        }
        // return string โดยหาจากวินาที
        function GetTimeString(sec) {
            if (sec >= 60) {
                var m = Math.floor(sec / 60);
                var s = sec % 60;
                return (s == 0) ? m + " นาที" : m + " นาที " + s + " วินาที";
            }
            return sec + " วินาที";
        }
        function checkTimeAllQuestion(timeAll) {
            var timer = parseInt(timeAll); //ค่าจาก textbox เวลาทั้งหมด
            if (isNaN(timer)) { $('#lblTimeAll').html(""); return false; }
            var questionAmount = parseInt($('#lblQuestionAmount').html()); //จำนวนข้อ
            var timerPerQuestion = Math.floor((timer * 60) / questionAmount); //เวลาเป็นนาทีมาเฉลี่ยต่อข้อ
            console.log(timerPerQuestion);
            if (timerPerQuestion < 10) {
                var timerPerAll = Math.ceil((10 * questionAmount) / 60);
                console.log("เวลาต่อข้อ = " + timerPerAll);
                $('#txtTimeAll').val(timerPerAll);
                //var timerPerQ = ((timerPerAll * 60) / questionAmount);//.toFixed(2);
                //$('#lblTimeAll').html(timerPerQ);
                // qtip
                $('#txtTimeAll').qtip({
                    content: 'ต้องกรอกเวลาทำควิซทั้งหมดให้เฉลี่ยข้อละ 10 วินาทีขึ้นไปค่ะ ปรับค่าให้เหมาะสมแล้วค่ะ',
                    show: { ready: true },
                    style: {
                        width: 400, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimeAll') },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                timer = timerPerAll;
                DestroyQtip($('#txtTimeAll'));
            }
            //else {
            //    var timerPerQ = ((timer * 60) / questionAmount);//.toFixed(2);
            //    //var timerPerQ = ((timer * 60) / questionAmount);
            //    $('#lblTimeAll').html(timerPerQ);
            //}
            var t = 60 * timer;
            var sec = Math.floor(t / questionAmount);
            $('#lblTimeAll').html("(ข้อละ " + GetTimeString(sec) + ")");
            $('#hdTimeAll').val(timer);
        }
        function checkTimerSolve(timerPerQuestion) {
            var timer = parseInt(timerPerQuestion); // ค่าจาก textbox เวลาต่อข้อ
            var questionAmount = parseInt($('#lblQuestionAmount').html()); // จำนวนข้อ           

            if (timer < 10) {
                $('#txtTimeShowAnswer').val(10);
                $('#txtTimeShowAnswer').qtip({
                    content: 'ต้องกรอกเวลาเฉลยควิซข้อละ 10 วินาทีขึ้นไปค่ะ',
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimeShowAnswer') },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                var timerPerQ = ((10 * questionAmount) / 60);//.toFixed(2);
                DestroyQtip($('#txtTimeShowAnswer'));
            }
        }
        function DestroyQtip(id) {
            setTimeout(function () {
                $(id).qtip('destroy');
            }, 4000);
        }
    </script>
    <style type="text/css">
        .testDiv {
            padding: 5px;
            background-color: White;
            float: left;
            margin: 9px;
            z-index: 0;
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
        }

        .itemSettingQuiz {
            position: relative;
            width: 100px;
            height: 100px;
            /*float: left;*/
            color: White; /*display: none;*/
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
            background-repeat: no-repeat; /*background-size: 60px;*/
            background-position: center;
            cursor: pointer;
            text-align: center;
            display: inline-block;
            /*font-weight: bold;
            display: table-cell;
            vertical-align: bottom;*/
        }
        /*.itemSettingQuiz span
        {
            font-size: 14px;
            position: absolute;
            bottom: 0;
            right: 0;
            line-height: 1.2em;
        }*/
        .settingSummary {
        }

        .divSettingDetail {
            border: 1px solid;
            width: 400px;
            margin-left: 20px;
            background-color: #F4F7FF;
            -webkit-border-radius: 15px;
            border-color: #AFAFAF;
            padding-left: 10px;
            padding-right: 10px;
        }

        img {
            position: relative;
            margin-left: 10px;
            margin-bottom: -15px;
        }

        .txtTime {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            // ready
            setImageWhenChoose();

            // Item Click
            $('.itemSettingQuiz').click(function () {
                var parentName = $(this).parent().attr('id');
                if (parentName == 'makeQuizDetailPic') {
                    $('#makeQuiz').toggleClass('settingSummary');
                    swipePanelLR($('#makeQuiz'), $('#settingQuiz'));
                }
                else {
                    $('#settingQuiz').toggleClass('settingSummary');
                    swipePanelLR($('#settingQuiz'), $('#makeQuiz'));
                }
            });

            // div ฝั่งขวา
            $('#settingQuiz').click(function (e) {
                if ($(e.target).is($(this))) {
                    $(this).toggleClass('settingSummary');
                    if ($(this).hasClass('settingSummary')) {
                        swipePanelLR($(this), $('#makeQuiz'));
                    }
                    else {
                        swipePanelSelf($(this), $('#makeQuiz'));
                    }
                    // update 29-04-56 Tools  
                    var checked = $('#chkUseTools').attr('checked');
                    resizeWithItemTools(checked);
                }
            });

            // div ฝั่งซ้าย
            $('#makeQuiz').click(function (e) {
                var PicSetting = $('#settingQuizDetailPic');
                var itemSetting = $('#settingQuizDetailPic').children('div');
                var checked = $('#chkUseTools').attr('checked');

                if ($(e.target).is($(this))) {
                    $(this).toggleClass('settingSummary');
                    if ($(this).hasClass('settingSummary')) {
                        swipePanelLR($(this), $('#settingQuiz'));
                        // update 29-04-56 Tools
                        if (checked) {
                            $(PicSetting).css('width', '100%');
                            $(itemSetting).removeClass('itemSettingQuiz');
                            $(itemSetting).addClass('itemSettingQuizWithTools');
                        }
                        else {
                            $(PicSetting).css('width', '345px');
                            $(itemSetting).removeClass('itemSettingQuizWithTools');
                            $(itemSetting).addClass('itemSettingQuiz');
                            resizeWithItemTools(false);
                        }
                    }
                    else {
                        swipePanelSelf($(this), $('#settingQuiz'));
                        // update 29-04-56 Tools
                        if (checked) {
                            $(PicSetting).css('width', '345px');
                            $(itemSetting).removeClass('itemSettingQuizWithTools');
                            $(itemSetting).addClass('itemSettingQuiz');
                        }
                    }
                }

            });



            // checkbox จับเวลา
            $('#<%=chkCheckTime.clientId%>').click(function () {
                ShowHideTimechk();
            });
            // checkbox ไปไม่พร้อมกัน
            var destroyQtip;
            $('#<%=chkSelfPace.clientId %>').click(function () {
                clearTimeout(destroyQtip);
                if ($(this).attr('checked') == 'checked') {
                    $('label[for=chkSelfPace]').qtip({
                        content: 'แต่ละคนกดไปข้อถัดไปได้ ไม่ต้องรอกัน',
                        show: { ready: true },
                        style: {
                            width: 150, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                        },
                        position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' } },
                        hide: false
                    });
                    destroyQtip = setTimeout(function () {
                        $('label[for=chkSelfPace]').qtip('destroy');
                    }, 4000);
                }
                else {
                    $('label[for=chkSelfPace]').qtip('destroy');
                }

                ShowHideSelfPace();
            });
            // checkbox เฉลย
            $('#<%=chkShowAnswer.clientId %>').click(function () {
                ShowHideShowAnswer();
            });
            // checkbox คะแนน
            $('#<%=chkShowScore.clientId %>').click(function () {
                ShowHideShowScore();
            });
            // checkbox สลับข้อสอบ-คำตอบ
            $('.diffQuestion').click(function () {
                ShowHideShowDiffQuestion();
            });
            // checkbox tablet
            var chk = $('#chkQuizUseTablet').attr('checked');
            if (!chk) {
                $('#divUseTemplate').show();
            }


            $('#chkQuizUseTablet').change(function () {
                // clear checkbox tabletLab
                var img = $('#imgTablet');
                if (!($(this).attr('checked'))) {
                    //Check KNConfig
                    if (RunModeConfig == 'labonly') {
                        $(this).attr('checked', 'checked');
                    }
                    else {
                        $('#DivFromSoundlab').stop(true, true).hide();
                        $('#itemQuizMode').hide();
                        $('#DivDiffQuestion').hide();

                        $('#chkQuizFromSoundlab').attr('checked', false).attr('disabled', true);
                        $('#DDLSoundLabName').hide();
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');

                        $('#chkSelfPace').attr('checked', false).attr('disabled', true);
                        $('#chkUseTools').attr('checked', false).attr('disabled', true);
                        $('#itemSelfPace').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-SamePace.png")');
                        resizeWithItemTools(false);
                        $('#DivDiffQuestion').hide();
                        $('#chkDiffQuestion').attr('checked', false);
                        //ซ่อน Setting แสดงคะแนนแท็บเล็ตเด็ก
                        //$('#chkShowScore').hide();
                        //$('#chkShowScore').prev().hide();
                        //$('#chkShowScore').next().hide();
                        //$('#imgShowScore').hide();
                        //$('#imgShowScore').next().hide();
                        $('#DivChkShowScore').hide();
                        $('#chkShowScore').attr('checked', false).attr('disabled', true);
                    }
                }
                else {

                    ChkTabletLabAmount();

                    $('#chkQuizFromSoundlab').attr('disabled', false);
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png');

                    $('#chkSelfPace').attr('checked', false).attr('disabled', false);
                    $('#chkUseTools').attr('checked', false).attr('disabled', false);
                    ShowHideShowDiffQuestion();
                    //แสดง Setting แสดงคะแนนแท็บเล็ตเด็ก
                    //$('#chkShowScore').show();
                    //$('#chkShowScore').prev().show();
                    //$('#chkShowScore').next().show();
                    //$('#imgShowScore').show();
                    //$('#imgShowScore').next().show();
                    $('#chkShowScore').attr('checked', false).attr('disabled', false);
                }
                // set session UseTablet
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/checkQuizUseTablet",
                    async: false, // ทำงานให้เสร็จก่อน
                    data: "{checked : '" + $(this).attr('checked') + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var chk = data.d;
                        if (chk == true) {
                            $('#divUseTemplate').hide();
                            $('#divUseTemplate').qtip('destroy');
                            $('#chkUseTemplate').attr('checked', false);
                        }
                        else if (chk == false) {
                            $('#divUseTemplate').show();
                            if ($('#chkUseTemplate').attr('disabled')) {
                                var content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ เพราะจำนวนข้อสอบเกิน 120 ข้อ';
                                if ($('#divUseTemplate').hasClass('TypeError')) {
                                    content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ ข้อสอบต้องเป็นแบบตัวเลือกไม่เกิน 5 ช้อยส์หรือถูกผิดเท่านั้น';
                                }
                                $('#divUseTemplate').live().qtip({
                                    content: content.toString(),
                                    show: { ready: true },
                                    style: {
                                        width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                                    },
                                    position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' } },
                                    hide: false
                                });
                            }
                        }
                    },
                    error: function myfunction(request, status) {
                    }
                });
            });

            // checkbox checkmark
            $('#chkUseTemplate').change(function () {
                if ($(this).attr('checked')) {
                    $('#chkQuizUseTablet').attr('checked', false);
                    $('#chkQuizFromSoundlab').attr('checked', false).attr('disabled', true);
                    $('#chkSelfPace').attr('checked', false).attr('disabled', true);
                    $('#chkUseTools').attr('checked', false).attr('disabled', true);
                    $('#chkShowScore').attr('checked', false).attr('disabled', true);
                    $('#itemSelfPace').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-SamePace.png")');
                    $('#imgTablet').attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');
                    resizeWithItemTools(false);
                    ShowHideShowDiffQuestion();
                    $('#DDLSoundLabName').hide();
                } else {
                    $('#DivDiffQuestion').hide();
                    $('#chkDiffQuestion').attr('checked', false);
                }
                //$.ajax({
                //  type: "POST",
                //  url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/checkQuizUseTamplate",
                //  async: false, 
                //  data: "{checked : '" + $(this).attr('checked') + "' }",  //" 
                //  contentType: "application/json; charset=utf-8", dataType: "json",
                //  success: function (data) {
                //      var chk = data.d;
                //  },
                //  error: function myfunction(request, status) {
                //  }
                //});
            });
        });

        // function swipe open-closed self
        function swipePanelSelf(mySelf, Another) {

            var divWidthSelf = '315px';
            var divWidthAnother = '465px';
            if ($(mySelf).is('#settingQuiz')) {
                divWidthSelf = '465px';
                divWidthAnother = '315px';
            }

            var listChild = [];
            $(mySelf).children().each(function () {
                var id_child = $(this).attr('id');
                if (id_child != undefined) {
                    listChild.push(id_child);
                }
            });

            var divItemSelf = $('#' + listChild[0]);
            var divDetailSelf = $('#' + listChild[1]);

            $(mySelf).css('height', 'auto').css('width', divWidthSelf).css('background-image', 'url("../images/activity/zoomin.png")');
            $(divDetailSelf).hide();
            $(divItemSelf).show();
            $(Another).css('width', divWidthAnother);

            //imageSummary                            
            setImageWhenChoose();
        }

        // function swipe ซ้ายขวา เมนูทั้ง 2 ข้าง
        function swipePanelLR(mySelf, Another) {
            // ดูว่า div ที่ไม่ได้กดแว่นขยาย มีคลาส settingSummary?               
            if ($(Another).hasClass('settingSummary')) {
                $(Another).toggleClass('settingSummary');
            }

            var divHeight = 'auto';
            var divWidth = '470px';
            //                if ($(mySelf).is('#settingQuiz')) {                  
            //                    //divWidth = '500px';
            //                }

            // My Self
            var listChild = [];
            $(mySelf).children().each(function () {
                var id_child = $(this).attr('id');
                if (id_child != undefined) {
                    listChild.push(id_child);
                }
            });

            var divItemSelf = $('#' + listChild[0]);
            var divDetailSelf = $('#' + listChild[1]);

            $(mySelf).css('width', divWidth).css('height', divHeight).css('background-image', 'url("../images/activity/zoomout.png")');
            $(divDetailSelf).show();
            $(divItemSelf).hide();

            // Another
            var listChildAnother = [];
            $(Another).children().each(function () {
                var id_child = $(this).attr('id');
                if (id_child != undefined) {
                    listChildAnother.push(id_child);
                }
            });

            var divItemAnother = $('#' + listChildAnother[0]);
            var divDetailAnother = $('#' + listChildAnother[1]);

            $(Another).css('width', '310px').css('height', 'auto').css('background-image', 'url("../images/activity/zoomin.png")');
            $(divItemAnother).show();
            $(divDetailAnother).hide()

            //imageSummary
            setImageWhenChoose();
        }

        // แสดงจับเวลา //
        function ShowHideTimechk() {
            console.log('ShowHideTimechk');
            var item = $('#itemTime');
            var divDetail = $('#DivCheckTime');
            var img = $('#imgShowTime');
            if ($('#<%=chkCheckTime.clientId%>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();
                if ($('#IsPerQuestion').attr('checked')) {
                    //$(item).html('จับเวลาข้อต่อข้อ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Watch1-1.png")');
                    setImageInItem(item, 'จับเวลาข้อต่อข้อ', 'Settings-btnQuizMode-Watch1-1.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch1-1_small.png');
                }
                else {
                    //$(item).html('จับเวลาทั้งหมด').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Watch-1-N.png")');
                    setImageInItem(item, 'จับเวลาทั้งหมด', 'Settings-btnQuizMode-Watch-1-N.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch-1-N_small.png');
                }
            }
            else {
                if ($('#IsPerQuestion').attr('checked') != 'checked' && $('#IsAll').attr('checked') != 'checked') {
                    $('#IsPerQuestion').attr('checked', true);
                    $('#txtTimeAll').attr('disabled', 'disabled');
                }
                $(divDetail).stop(true, true).hide();
                //$(item).html('ไม่จับเวลา').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-NoWatch.png")');
                setImageInItem(item, 'ไม่จับเวลา', 'Settings-btnQuizMode-NoWatch.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoWatch_small.png');
            }
        }
        // ไปไม่พร้อมกัน //
        function ShowHideSelfPace() {
            var item = $('#itemSelfPace');
            var img = $('#imgSelfPace');
            if ($('#<%=chkSelfPace.clientId %>').attr('checked') == 'checked') {
                //$(item).html('ไปไม่พร้อมกัน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-SelfPace.png")');
                setImageInItem(item, 'ไปไม่พร้อมกัน', 'Settings-btnQuizMode-SelfPace.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-SelfPace_small.png');
            }
            else {
                //$(item).html('ไปพร้อมกัน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-SamePace.png")');
                setImageInItem(item, 'ไปพร้อมกัน', 'Settings-btnQuizMode-SamePace.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-SamePace_small.png');
            }
        }
        // แสดงเฉลย //
        function ShowHideShowAnswer() {
            var item = $('#itemSolve');
            var divDetail = $('#DivChkShowAnswer');
            var chkShowAnswer = $('#<%=chkShowAnswer.clientId %>');
            var img = $('#imgShowAnswer');
            if ($('#<%=chkShowAnswer.clientId %>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();
                if ($('#rdbAnswerAfter').attr('checked') == 'checked') {
                    $('#ChkInShow1').attr('disabled', true);
                    $('#ChkInShow1').attr('checked', false);
                    $('#txtTimeShowAnswer').attr('disabled', true);
                    //$(item).html('แสดงเฉลยเมื่อจบควิซ').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete.png")');
                    setImageInItem(item, 'แสดงเฉลยเมื่อจบควิซ', 'Settings-btnQuizMode-AnsAfterComplete.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete_small.png');
                }
                else {
                    if ($('#ChkInShow1').attr('checked')) {
                        //$(item).html('แสดงเฉลยทีละข้อมีเวลา').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-WithAnsTimer.png")');
                        setImageInItem(item, 'แสดงเฉลยทีละข้อมีเวลา', 'Settings-btnQuizMode-WithAnsTimer.png');
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAnsTimer_small.png');
                    }
                    else {
                        //$(item).html('แสดงเฉลยทีละข้อ').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-WithAns.png")');
                        setImageInItem(item, 'แสดงเฉลยทีละข้อ', 'Settings-btnQuizMode-WithAns.png');
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                    }
                }
            }
            else {
                $(divDetail).stop(true, true).hide();
                //$(item).html('ไม่แสดงเฉลย').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-NoAnswer.png")');
                setImageInItem(item, 'ไม่แสดงเฉลย', 'Settings-btnQuizMode-NoAnswer.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoAnswer_small.png');
            }
        }
        // แสดงคะแนน //
        function ShowHideShowScore() {
            var item = $('#itemScore');
            var divDetail = $('#DivChkShowScore');
            var imgShowScore = $('#imgShowScore');
            if ($('#<%=chkShowScore.clientId %>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();
                if ($('#rdbByStep').attr('checked')) {
                    //$(item).html('แสดงคะแนนทีละข้อ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion.png")');
                    setImageInItem(item, 'แสดงคะแนนทีละข้อ', 'Settings-btnQuizMode-ScoreEachQuestion.png');
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion_small.png');
                }
                else {
                    //$(item).html('แสดงคะแนนตอนจบควิซ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete.png")');
                    setImageInItem(item, 'แสดงคะแนนตอนจบควิซ', 'Settings-btnQuizMode-ScoreWhenComplete.png');
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete_small.png');
                }
            }
            else {
                $(divDetail).stop(true, true).hide();
                //$(item).html('ไม่แสดงคะแนน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreNoShow.png")');
                setImageInItem(item, 'ไม่แสดงคะแนน', 'Settings-btnQuizMode-ScoreNoShow.png');
                $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreNoShow_small.png');
            }
        }
        // soundlab
        function ShowHideSoundlab(useTablet) {
            if (useTablet) {
                $('#DivFromSoundlab').stop(true, true).show();
                $('#itemQuizMode').show();
            }
            else {
                $('#DivFromSoundlab').stop(true, true).hide();
                $('#itemQuizMode').hide();
            }
        }
        // สลับคำถาม-คำตอบ //
        function ShowHideShowDiffQuestion() {
            var isDiffQuestion;
            var item = $('#itemDiffQuestion');
            var divDetail = $('#DivDiffQuestion');
            var divWarning = $('#DivRandomWarning');
            var img = $('#imgQA');
            $('.diffQuestion').each(function () {
                var val = $(this).attr('val');
                //if ($('[id*=' + val + ']').attr('checked') == 'checked') {
                //    alert('if');
                //    isDiffQuestion = true;
                //}
                if ($('#' + val).attr('checked') == 'checked') {
                    isDiffQuestion = true;
                }
            });
            if (isDiffQuestion) {
                //var Question = $('[id*=chkRandomQuestion]').attr('checked');
                var Question = $('#chkRandomQuestion').attr('checked');
                //var Answer = $('[id*=chkRandomAnswer]').attr('checked');
                var Answer = $('#chkRandomAnswer').attr('checked');
                //var Q_A = $('[id*=chkDiffQuestion]').attr('checked');
                var Q_A = $('#chkDiffQuestion').attr('checked');
                var textHtml; var ImageFile;

                if ($('#chkQuizUseTablet').attr('checked')) {
                    $(divDetail).stop(true, true).show();
                } else {
                    $('#chkDiffQuestion').attr('checked', false);
                    $(divDetail).stop(true, true).hide();
                }

                if ((Question) && (Answer)) {
                    textHtml = 'สลับข้อ-คำตอบ';
                    ImageFile = 'Settings-btnQuizMode-RandomAll.png';
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll_small.png');
                    if ((Q_A) && $('#chkQuizUseTablet').attr('checked')) {
                        //textHtml = 'สลับข้อ-คำตอบ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomAll-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll-ScreenDiff_small.png');
                    }

                    <% If RandomQuestion And RandomAnswer Then%>
                    $(divWarning).css('display', 'none');
                    <%Else%>
                    $(divWarning).css('display', 'inline-block');
                    <% End If%>
                }
                else if ((Question) && !(Answer)) {
                    textHtml = 'สลับข้อ';
                    ImageFile = 'Settings-btnQuizMode-RandomQuest.png';
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest_small.png');
                    if ((Q_A) && $('#chkQuizUseTablet').attr('checked')) {
                        //textHtml = 'สลับข้อ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomQuest-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest-ScreenDiff_small.png');
                    }

                    <% If RandomQuestion Then%>
                    $(divWarning).css('display', 'none');
                    <%Else%>
                    $(divWarning).css('display', 'inline-block');
                    <% End If%>

                }
                else if (!(Question) && (Answer)) {
                    console.log('bug');
                    textHtml = 'สลับคำตอบ';
                    ImageFile = 'Settings-btnQuizMode-RandomChoice.png';
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice_small.png');
                    if ((Q_A) && $('#chkQuizUseTablet').attr('checked')) {
                        //textHtml = 'สลับคำตอบ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomChoice-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice-ScreenDiff_small.png');
                    }
                    <% If RandomAnswer Then%>
                    $(divWarning).css('display', 'none');
                    <%Else%>
                    $(divWarning).css('display', 'inline-block');
                    <% End If%>

                }

                $('label[for=chkDiffQuestion]').text(textHtml + '  ให้นักเรียนแต่ละคนไม่เหมือนกัน');
                //$(item).html(textHtml).css('background-image','url("../images/File-Open-icon.png")');
                setImageInItem(item, textHtml, ImageFile);
            }
            else {
                $(divDetail).stop(true, true).hide();
                $('[id*=chkDiffQuestion]').attr('checked', false);
                //$(item).html('เหมือนกันทุกคน').css('background-image','url("../images/download.png")');
                setImageInItem(item, 'เหมือนกันทุกคน', 'Settings-btnQuizMode-NoRandom.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoRandom_small.png');
                $(divWarning).css('display', 'none');
            }
        }
        // แสดงโหมดควิซ //
        function ShowHideModeQuiz() {
            var item = $('#itemQuizMode');
            var chkTabletLab = $('#chkQuizFromSoundlab');
            var img = $('#imgTablet');
            if ($('#chkUseTemplate').attr('checked') == 'checked') {
                //$(item).html('ควิซกระดาษคำตอบ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Lab.png")');
                setImageInItem(item, 'ควิซกระดาษคำตอบ', 'Settings-btnQuizMode-CheckMark.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');
            }
            else if ($('#chkQuizUseTablet').attr('checked') == 'checked') {
                if ($(chkTabletLab).attr('checked') == 'checked') {
                    //$(item).html('ควิซแล็บแท็บเล็ต').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Lab.png")');
                    setImageInItem(item, 'ควิซแล็บแท็บเล็ต', 'Settings-btnQuizMode-Lab.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Lab_small.png');
                }
                else {
                    //$(item).html('ควิซแท็บเล็ต').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Tablet.png")');
                    setImageInItem(item, 'ควิซแท็บเล็ต', 'Settings-btnQuizMode-Tablet.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png');
                }
            }
            else {
                $(chkTabletLab).attr('disabled', true);
                //$(item).show().html('ควิซปกติ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-QuizOnly.png")');    
                setImageInItem(item, 'ควิซปกติ', 'Settings-btnQuizMode-QuizOnly.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');
            }
        }
        // function htmlItem 
        function setImageInItem(item, textHtml, imageFile) {
            $(item).html('').css('background-image', 'url("../images/activity/setting/' + imageFile + '")').prop('title', textHtml);
        }
        // function set image label
        function setImageInLabel() {
        }
        // แสดงรูป
        function setImageWhenChoose() {
            // mode left side
            ShowHideModeQuiz();
            ShowHideShowDiffQuestion();
            // mode right side  
            ShowHideTimechk();
            ShowHideShowAnswer();
            ShowHideShowScore();
            ShowHideSelfPace();
            // update 29-04-56 Tools
            var checked = $('#chkUseTools').attr('checked');
            resizeWithItemTools(checked);
            ShowHideUseTools(checked);
        }
        // update 29-04-56 Tools
        function resizeWithItemTools(checked) {
            var PicSetting = $('#settingQuizDetailPic');
            var itemSetting = $('#settingQuizDetailPic').children('div');

            if (checked) {
                $(PicSetting).css('width', '345px');
                $('#itemTools').show();
                //                $('#itemSolve').css('margin-top','0px').css('margin-left','20px');
                //                $('#itemScore').css('margin-left','60px');
                $(itemSetting).removeClass('itemSettingQuizWithTools');
                $(itemSetting).addClass('itemSettingQuiz');
            }
            else {
                $('#itemTools').hide();
                $(PicSetting).css('width', '225px');
                //                $('#itemSolve').css('margin-top','20px').css('margin-left','0px');
                //                $('#itemScore').css('margin-left','20px');
                $(itemSetting).removeClass('itemSettingQuizWithTools');
                $(itemSetting).addClass('itemSettingQuiz');
            }
        }

        function ChkTabletLabAmount() {
            var Room;
            var Level;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetTabletLab",
                async: false, // ทำงานให้เสร็จก่อน
                data: "{Room : '" + Room + "',Level : '" + Level + "'}",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        var chk = data.d;
                        if (chk == '0') {
                            $('#DivFromSoundlab').stop(true, true).hide();
                            $('#itemQuizMode').hide();
                            $('#DivDiffQuestion').hide();
                        } else {
                            console.log('usetablet');
                            $('#DivFromSoundlab').stop(true, true).show();
                            $('#itemQuizMode').show();
                            $('#DivDiffQuestion').show();
                        }
                    }
                },
                error: function myfunction(request, status) {

                }
            });
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            // img 
            var imgShowTime = $('#imgShowTime');
            var imgShowAnswer = $('#imgShowAnswer');
            var imgShowScore = $('#imgShowScore');
            // radio event
            $('input[type=radio]').click(function () {
                setImageCheckboxOnIE();
                // จับเวลาข้อต่อข้อ
                if ($(this).is('#IsPerQuestion')) {
                    $(imgShowTime).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch1-1_small.png');
                }
                    // จับเวลาทั้งหมด
                else if ($(this).is('#IsAll')) {
                    $(imgShowTime).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch-1-N_small.png');
                }
                    // เฉลยเมื่อจบควิซ
                else if ($(this).is('#rdbAnswerAfter')) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete_small.png');
                }
                    // เฉลยข้อต่อข้อ
                else if ($(this).is('#rdbAnswerPerQuestion')) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                }
                    // คะแนนข้อต่อข้อ
                else if ($(this).is('#rdbByStep')) {
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion_small.png');
                }
                    // คะแนนเมื่อจบควิซ
                else if ($(this).is('#rdbEndQuiz')) {
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete_small.png');
                }
            });
            // checkbox เวลาเฉลย
            $('#ChkInShow1').click(function () {
                var checked = $(this).attr('checked');
                if (checked) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAnsTimer_small.png');
                }
                else {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                }
            });
            // checkbox lab tablet
            $('#chkQuizFromSoundlab').click(function () {
                var checked = $(this).attr('checked');
                var img = $('#imgTablet');
                if (checked) {
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Lab_small.png');
                    $('#DDLSoundLabName').show();
                }
                else {
                    if (RunModeConfig == 'labonly') {
                        $(this).attr('checked', 'checked');
                        $('#DDLSoundLabName').show();
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png');
                        $('#DDLSoundLabName').hide();
                    }
                }
            });
            // checkbox diff question & answer
            $('#chkDiffQuestion').click(function () {
                var Question = $('[id*=chkRandomQuestion]').attr('checked');
                var Answer = $('[id*=chkRandomAnswer]').attr('checked');
                var checked = $(this).attr('checked');
                var img = $('#imgQA');

                if ((Question) && (Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll_small.png');
                    }
                }
                else if ((Question) && !(Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest_small.png');
                    }
                }
                else if (!(Question) && (Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice_small.png');
                    }
                }

            });

            // update 29-04-56 add Tools
            // checkbox use tools #1
            $('#chkUseTools').click(function () {
                var checked = $(this).attr('checked');
                ShowHideUseTools(checked);
            });


            $('#chkWithCalculator').click(function () {
                CheckUsageTools();
            });
            $('#chkWithDictionary').click(function () {
                CheckUsageTools();
            });
            $('#chkWithWordBook').click(function () {
                CheckUsageTools();
            });
            $('#chkWithNotes').click(function () {
                CheckUsageTools();
            });
            $('#chkWithProtractor').click(function () {
                CheckUsageTools();
            });

        });
        // update 29-04-56 add Tools checkbox     
        // #2
        function ShowHideUseTools(checked) {
            var divDetail = $('#DivChkUseTools');
            if (checked) {
                $(divDetail).show();
                $('#tblTools').children().find('input[type=checkbox]').each(function () {
                    $(this).attr('checked', true);
                });
            }
            else {
                $(divDetail).hide();
                $('#tblTools').children().find('input[type=checkbox]').each(function () {
                    $(this).attr('checked', false);
                });
            }
        }
        function CheckUsageTools() {
            var i = 0;
            $('#tblTools').children().find('input[type=checkbox]').each(function () {
                if (($(this).attr('checked') == 'checked')) {
                    i++;
                }
            });
            if (i == 0) { $('#chkUseTools').attr('checked', false); $('#DivChkUseTools').hide(); }
        }
    </script>
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');

        $(document).ready(function () {
            // ready!!!!!!!
            setImageCheckboxOnIE();
            //checkbox tick
            $('input[type=checkbox]').click(function () {
                setImageCheckboxOnIE();
            });
        });
        function setImageCheckboxOnIE() {
            if ($.browser.msie) {
                $('input[type=checkbox]').each(function () {
                    var chk = $(this).attr('checked');
                    if (chk) {
                        var chkT = { 'background-image': 'url(../images/bullet_checked.gif)' };
                        $(this).next().css(chkT);
                    }
                    else {
                        var chkF = { 'background': 'url(../images/bullet.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px', 'color': '' };
                        if ($(this).attr('disabled')) {
                            var chkDis = { 'background': ' url(../images/bullet-disable.gif) center left no-repeat', 'color': 'Gray' };
                            $(this).next().css(chkDis);
                        } else {
                            $(this).next().css(chkF);
                        }

                    }
                });
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#txtTimePerQuestion').attr('disabled', 'disabled');
            //$('#txtTimeShowAnswer').attr('disabled', 'disabled');
            var destroyQtip;
            $('#IsPerQuestion').change(function () {
                $('.qtip').remove();
                clearTimeout(destroyQtip);
                var chk = $(this).attr('checked');
                if (chk) {
                    //qtip
                    $('#lblTimeAllPerQuestion').next('span').qtip({
                        content: 'โหมดนี้ นักเรียนจะย้อนกลับไปข้อก่อนหน้าไม่ได้',
                        show: { ready: true },
                        style: {
                            width: 180, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                        },
                        position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' } },
                        hide: false
                    });
                    destroyQtip = setTimeout(function () {
                        $('#lblTimeAllPerQuestion').next('span').qtip('destroy');
                    }, 4000);
                    $('#txtTimeAll').attr('disabled', 'disabled');
                    $('#txtTimePerQuestion').removeAttr('disabled');
                }
            });
            $('#IsAll').change(function () {
                var chk = $(this).attr('checked');
                if (chk == 'checked') {
                    $('#txtTimePerQuestion').attr('disabled', 'disabled');
                    $('#txtTimeAll').removeAttr('disabled');
                }
            });
            $('#ChkInShow1').change(function () {
                var chk = $(this).attr('checked');
                if (chk == 'checked') {
                    $('#txtTimeShowAnswer').removeAttr('disabled');
                } else {
                    $('#txtTimeShowAnswer').attr('disabled', 'disabled');
                }
            });

            // update 30-04-56 label for checkbox
            $('#tblTools').children().find('label').css('color', 'transparent');
        });
    </script>
    <style type="text/css">
        .btnChoose {
            position: relative;
            width: 35px;
            height: 35px;
            font-weight: bolder;
            top: -5px;
            cursor: pointer;
            background: #1EC9F4;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            text-shadow: 1px 1px #178497;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        /* update 29-04-56 Add Tools */
        .imgTools {
            width: 35px;
            height: 35px;
            margin-left: 0px;
        }

        #tblTools tr td {
            background: none;
            border-bottom: none;
            padding: 0px;
        }

        .itemSettingQuizWithTools {
            position: relative;
            width: 90px;
            height: 90px;
            display: inline-block;
            /*float: left;*/
            color: White;
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
            background-repeat: no-repeat;
            background-position: center;
            cursor: pointer;
            text-align: center;
        }

        .ddl {
            height: 50px;
            font: 90% 'THSarabunNew';
            border: 1px solid #C6E7F0;
            background: #FFFFFF;
            color: #444;
            border-radius: 7px 0 0 7px;
            width: 115px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            if ($.browser.msie && $.browser.version == 9.0) {
                $('#BtnBack').css({ 'margin-left': '-240px' });
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
            var elm = ['#makeQuiz', '#settingQuiz', '#btnChageLV', '#btnChangeRoom', '#Help', '#btnOK'];
            var tipPosition = ['leftMiddle', 'leftMiddle', 'bottomRight', 'bottomLeft', 'leftMiddle', 'leftMiddle'];
            var tipTarget = ['rightMiddle', 'rightMiddle', 'topMiddle', 'topMiddle', 'rightMiddle', 'rightMiddle'];
            var tipContent = ['ตั้งค่าข้อสอบกดที่นี้', 'ตั้งค่าคุณสมบัติกดที่นี้', 'เลือกชั้นที่จะทำควิซ', 'เลือกห้องที่จะทำควิซ', 'ศึกษาเพิ่มเติมดูที่นี่ค่ะ', 'ตั้งค่าเสร็จกดที่นี่เพื่อเริ่มควิซ'];
            var tipAjust = [-50, -50, 0, 0, -50];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold'
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

</head>
<body>
    <%If Not BusinessTablet360.ClsKNSession.IsMaxONet Then %>
    <script src="<%=ResolveUrl("~")%>js/bg.js" type="text/javascript"></script>
    <%End If %>
    <form id="form1" runat="server">
        <asp:HiddenField ID="SetupAnswer_Name" runat="server" />
        <asp:HiddenField ID="QuestionAmount" runat="server" />
        <div id="main">
            <div id="site_content">
                <div class="content" style="width: 930px; margin: 0;">
                    <center>
                        <h2>ชื่อชุดควิซ
                        <asp:Label ID="lblTestsetName" runat="server" Text="lblTestsetName"></asp:Label></h2>
                        <div id="div-1" style="color: #47433F">
                            <%--<div id='AllSetting' class="divAllSetting">--%>
                            <center style="font-size: 25px;">
                                <span id="spnClass">ชั้น </span>
                                <asp:Label ID="lblLevel" ClientIDMode="Static" runat="server" Text="lblLevel"></asp:Label>&nbsp;
                            <input type="button" class='btnChoose' id='btnChageLV' value='...' /><%--onclick='ChangeLevel()'--%>
                                <span id="spnRoom">/ ห้อง </span>
                                <asp:Label ID="lblRoom" runat="server" Text=""></asp:Label>&nbsp;
                            <input type="button" class='btnChoose' id='btnChangeRoom' value='...' />
                                <span id="txtTotalStudent"></span>
                                <%--<span>นักเรียนทั้งหมด </span>
                            <asp:TextBox ID="txtTotalStudent" Style='width: 40px' runat="server" Font-Size="25px"></asp:TextBox><span>
                                คน </span>--%>
                            </center>
                            <div id="makeQuiz" class="testDiv" style="width: 315px; height: auto; background-image: url('../images/activity/zoomin.png'); background-repeat: no-repeat; background-position-x: right; padding-top: 10px; background-position: right top; cursor: pointer; padding-bottom: 20px;">
                                <div class="divDetailLeftPanel" style="margin-left: auto; margin-right: auto; width: 220px;">
                                    <center>
                                        <b><span id="spnQuizDetail">จากข้อสอบ จำนวน&nbsp;</span><asp:Label ID="lblQuestionAmount" runat="server"
                                            Text="QuestionAmount" Font-Underline="True"></asp:Label><span id="spnQuestionAmount">&nbsp;ข้อ</span></b></center>
                                </div>
                                <div id="makeQuizDetailPic" style="margin-left: auto; margin-right: auto; width: 100px; height: 224px; margin-top: 10px;">
                                    <div id="itemQuizMode" class="itemSettingQuiz" style="background-color: #008299;">
                                        <span>ควิซแล็บแท็บเล็ต</span>'
                                    </div>
                                    <div id="itemDiffQuestion" class="itemSettingQuiz" style="background-color: #2E8DEF; margin-top: 12px;">
                                    </div>
                                </div>
                                <div id="makeQuizDetail" style="margin-left: 20px; display: none;">
                                    <%If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then%>
                                    <asp:CheckBox ID="chkUseTemplate" runat="server" Text="ให้เด็กทำด้วยกระดาษคำตอบคอมพิวเตอร์" /><img
                                        id="imgCheckmark" src="../images/activity/setting/Settings-btnQuizMode-CheckMark_small.png" alt="" /><br />
                                    <%End If%>
                                    <div id="DivchkQuizUseTablet">
                                        <asp:CheckBox ID="chkQuizUseTablet" runat="server" Text="ทำควิซโดยใช้แท็บเล็ต ?"
                                            Checked="true" /><img id="imgTablet" src="" />
                                    </div>
                                    <%--<br />--%>
                                    <div id='DivFromSoundlab' style='width: 400px; border: 1px solid; margin-left: 20px; background-color: #F4F7FF; -webkit-border-radius: 15px; border-color: #AFAFAF; padding: 10px; margin-bottom: 10px; display: none;'>
                                        <div id="DivchkQuizFromSoundlab">
                                            <asp:CheckBox ID="chkQuizFromSoundlab" runat="server" Text="ใช้กับห้องปฏิบัติการ (แท็บเล็ตแล็บ)" />
                                        </div>
                                        <asp:DropDownList ID="DDLSoundLabName" runat="server" ClientIDMode="Static" Style="display: none; width: initial; max-width: 400px"
                                            CssClass="ddl">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="DivchkRandomQuestion" style="width: 150px; display: inline-block;">
                                        <asp:CheckBox ID="chkRandomQuestion" runat="server" Text="สลับข้อให้ใหม่" class="diffQuestion"
                                            val="chkRandomQuestion" Style="margin-top: 10px;" />
                                        <span id="spnSpaceInDivchkRandomQuestion">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </div>
                                    <div id="DivchkRandomAnswer" style="width: 225px; display: inline-block;">
                                        <asp:CheckBox ID="chkRandomAnswer" runat="server" Text="สลับคำตอบให้ใหม่" class="diffQuestion"
                                            val="chkRandomAnswer" /><img id="imgQA" src="" />
                                    </div>
                                    <div id='DivDiffQuestion' style='width: 400px; border: 1px solid; margin-left: 20px; background-color: #F4F7FF; -webkit-border-radius: 15px; border-color: #AFAFAF; padding: 10px;'>
                                        <asp:CheckBox ID="chkDiffQuestion" runat="server" Text="aas" val="chkDiffQuestion" />
                                    </div>
                                    <div id='DivRandomWarning' style='width: 400px; padding: 10px; color: red; font-size: 14px; margin-left: 20px; display: none;'>
                                        <span id="SpnRandomWarning">ข้อสอบชุดนี้มีข้อที่ห้ามสลับคำถาม-คำตอบอยู่ด้วย (เช่น คำถามต่อเนื่อง) แม้จะเลือกสลับข้อ-คำตอบ ข้อเหล่านั้นจะไม่สลับให้ค่ะ</span>
                                    </div>
                                </div>
                            </div>
                            <div id="settingQuiz" class="testDiv" style="width: 465px; height: 270px; background-image: url('../images/activity/zoomin.png'); background-repeat: no-repeat; background-position-x: right; padding-top: 10px; background-position: right top; cursor: pointer; padding-bottom: 20px; min-height: 270px;">
                                <div style="margin-left: auto; margin-right: auto; width: 210px;">
                                    <center>
                                        <b><span id="spnQuizSetting">ตั้งค่าควิซ</span></b></center>
                                </div>
                                <div id="settingQuizDetailPic" style="margin-top: 10px; margin-left: auto; margin-right: auto; width: 100%; height: 220px; text-align: center;">
                                    <div id="itemTime" class="itemSettingQuiz" style="background-color: #008299;">
                                    </div>
                                    <div id="itemSelfPace" class="itemSettingQuiz" style="background-color: #2E8DEF;">
                                    </div>
                                    <div id="itemSolve" class="itemSettingQuiz" style="background-color: #DC572E;">
                                    </div>
                                    <div id="itemScore" class="itemSettingQuiz" style="background-color: #00A600;">
                                    </div>
                                    <div id="itemTools" class="itemSettingQuiz" style="background-color: #00AA00; display: none; background-image: url('../Images/Activity/Setting_Tools/Tools.png'); background-size: 60px;">
                                    </div>
                                </div>
                                <div id="settingQuizDetail" style="display: none; margin-left: 20px;">
                                    <div id="DivchkCheckTime">
                                        <asp:CheckBox ID="chkCheckTime" runat="server" Text="จับเวลา" class="chkSettingQuiz"
                                            val="chkCheckTime" /><img id="imgShowTime" />
                                    </div>
                                    <%--<br />--%>
                                    <div id='DivCheckTime' class="divSettingDetail">
                                        <div id="DivIsPerQuestion" style="width: 85px; display: inline-block;">
                                            <input type="radio" id='IsPerQuestion' name='ChkTime' runat="server" /><label id="lblForIsPerQuestion" for='IsPerQuestion'>
                                                ข้อละ &nbsp;&nbsp;&nbsp;
                                            </label>
                                        </div>
                                        <asp:TextBox ID="txtTimePerQuestion" Style="width: 30px" runat="server" CssClass="txtTime" MaxLength="3" onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
                                        <span id="spnSpaceIntxtTimePerQuestion">&nbsp;วินาที </span>
                                        <%--<span id="spnInTimePerQuestion">&nbsp;(รวมทั้งหมด</span>--%>
                                        <asp:Label ID="lblTimeAllPerQuestion" runat="server" Text="lblTAPQ"></asp:Label>
                                        <%-- <span id="spnMinuteTxtInDivCheckTime">)</span>--%>
                                        <br />
                                        <div id="DivIsAll" style="width: 85px; display: inline-block;">
                                            <input type='radio' id='IsAll' name='ChkTime' runat="server" /><label for='IsAll'>
                                                <span id="spnInDivIsAll">ทั้งหมด &nbsp;</span>
                                            </label>
                                        </div>
                                        <asp:TextBox ID="txtTimeAll" Style="width: 30px" runat="server" CssClass="txtTime" MaxLength="3" onkeypress="return event.charCode >= 48 && event.charCode <= 57"></asp:TextBox>
                                        <span id="spnTimeDetail">&nbsp;นาที</span>
                                        <asp:Label ID="lblTimeAll" runat="server" Text="lblTA" Style="margin-left: 9px;"></asp:Label>
                                        <%--<span id="spnTimeDetail2">)</span>--%>
                                    </div>
                                    <div id="DivchkSelfPace">
                                        <asp:CheckBox ID="chkSelfPace" runat="server" Text="ทำควิซไม่พร้อมกัน" class="chkSettingQuiz"
                                            val="chkSelfPace" /><img id="imgSelfPace" />
                                    </div>
                                    <%--<br />--%>
                                    <div id="DivchkShowAnswer">
                                        <asp:CheckBox ID="chkShowAnswer" runat="server" Text="แสดงเฉลย" class="chkSettingQuiz"
                                            val="chkShowAnswer" /><img id="imgShowAnswer" src="" />
                                    </div>
                                    <%--<br />--%>
                                    <div id='DivChkShowAnswer' class="divSettingDetail">
                                        <div id="DivrdbAnswerPerQuestion">
                                            <input type="radio" name='rShowAnswer' runat="server" id='rdbAnswerPerQuestion' class='radioShowAnswer' /><label
                                                for='Show1'>
                                                แสดงทีละข้อ
                                            </label>
                                        </div>

                                        <div id='ForShow1' style='padding-left: 30px'>
                                            <asp:CheckBox ID="chkRushMode" Style='display: none;' runat="server" Text="แข่งกันขอตอบคำถาม" />
                                            <div id="DivChkInShow1" style="display: inline-block; width: 102px;">
                                                <asp:CheckBox ID="ChkInShow1" runat="server" Text="แสดงข้อละ" />
                                            </div>

                                            &nbsp;&nbsp;<asp:TextBox
                                                ID="txtTimeShowAnswer" Style='width: 25px' runat="server" CssClass="txtTime" MaxLength="3" onkeypress="return event.charCode >= 48 && event.charCode <= 57">30</asp:TextBox>
                                            &nbsp;&nbsp; วินาที
                                        </div>
                                        <div id="DivrdbAnswerAfter">
                                            <input type="radio" id='rdbAnswerAfter' runat="server" name='rShowAnswer' class='radioShowAnswer' /><label
                                                for='Show2'>
                                                แสดงเมื่อทำครบทุกข้อ</label>
                                        </div>
                                        <%--<br />--%>
                                    </div>
                                    <div id="DivchkShowScore">
                                        <asp:CheckBox ID="chkShowScore" runat="server" Text="แสดงคะแนนที่แท็บเล็ตเด็ก" class="chkSettingQuiz"
                                            val="chkShowScore" /><img id="imgShowScore" src="" />
                                    </div>
                                    <%--<br />--%>
                                    <div id="DivChkShowScore" class="divSettingDetail" style="display: none;">
                                        <div id="DivrdbByStep">
                                            <input type="radio" id="rdbByStep" runat="server" class="radioShowScore" name="rShowScore" /><label
                                                for="Show3">
                                                แสดงทีละข้อ</label>
                                        </div>
                                        <%--<br />--%>
                                        <div id="DivrdbEndQuiz">
                                            <input type="radio" id="rdbEndQuiz" runat="server" class="radioShowScore" name="rShowScore" /><label
                                                for="Show4">
                                                แสดงเมื่อจบควิซ</label>
                                        </div>
                                    </div>
                                    <%-- update 29-04-56 Add Tools --%>
                                    <div id="DivchkUseTools" style="display:none;">
                                        <asp:CheckBox ID="chkUseTools" runat="server" Text="เครื่องมือช่วยตอนทำควิซ" class="chkSettingQuiz"
                                            val="chkUseTools" /><img id="imgToolsQuiz" src="../Images/Activity/Setting_Tools/Tools2.png" width="44px"
                                                height="44px" />
                                    </div>
                                    <div id="DivChkUseTools" class="divSettingDetail" style="display: none;">
                                        <table id="tblTools" style="width: 0px">
                                            <tr>
                                                <% If toolsSubject_Math = True Then%>
                                                <td>
                                                    <div id="DivchkWithCalculator">
                                                        <asp:CheckBox ID="chkWithCalculator" runat="server" class="chkSettingQuiz" Text="."
                                                            val="chkWithCalculator" /><img id="imgCalculator" src="../Images/Activity/Setting_Tools/Calculator_Icon.png"
                                                                class="imgTools" style="margin-right: 20px" />
                                                    </div>
                                                </td>
                                                <% End If%><% If toolsSubject_Eng = True Then%>
                                                <td>
                                                    <div id="DivchkWithDictionary">
                                                        <asp:CheckBox ID="chkWithDictionary" runat="server" class="chkSettingQuiz" Text="." /><img
                                                            src="../Images/Activity/Setting_Tools/Dictionary_Icon.png" id="imgDictionary" class="imgTools" style="margin-right: 20px" />
                                                    </div>
                                                </td>
                                                <% End If%><% If toolsSubject_Eng = True Then%>
                                                <td>
                                                    <div id="DivchkWithWordBook">
                                                        <asp:CheckBox ID="chkWithWordBook" runat="server" class="chkSettingQuiz" Text="." /><img
                                                            src="../Images/Activity/Setting_Tools/WordBook_Icon.jpg" id="imgWordBook" class="imgTools" style="margin-right: 20px" />
                                                    </div>
                                                </td>
                                                <% End If%><% If toolsAllSubject = True Then%>
                                                <td>
                                                    <div id="DivchkWithNotes">
                                                        <asp:CheckBox ID="chkWithNotes" runat="server" class="chkSettingQuiz" Text="." /><img
                                                            src="../Images/Activity/Setting_Tools/Notes_Icon.png" id="imgNote" class="imgTools" style="margin-right: 20px" />
                                                    </div>
                                                </td>
                                                <% End If%><% If toolsSubject_Math = True Then%>
                                                <td>
                                                    <div id="DivchkWithProtractor">
                                                        <asp:CheckBox ID="chkWithProtractor" runat="server" class="chkSettingQuiz" Text="." /><img
                                                            src="../Images/Activity/Setting_Tools/Protractor_Icon.jpg" id="imgProtractor" class="imgTools" />
                                                    </div>
                                                </td>
                                                <% End If%>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div class="form_settings" style="position: relative; margin: 15px 0 0 0;">
                            <asp:Button ID="BtnBack" runat="server" Text="กลับ" class="submit" Style="margin: 0 0 0 -320px; width: 160px; position: relative;" ClientIDMode="Static" />
                            <%--<div id="btnCheckUseQuiz" style="position:absolute;right:40px;float:right;width:160px;background-color:transparent;height:45px;top:-5px;z-index:5;cursor:pointer;"></div>--%>
                            <asp:Button ID="btnOK" runat="server" Text="เริ่มกันเลย" ClientIDMode="Static" class="submit" Style="float: Right; right: 40px; width: 160px; position: relative;" />
                        </div>
                    </center>
                </div>
            </div>
            <footer> <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์
                    &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด </footer>
        </div>
        <asp:HiddenField ID="HiddenTest" runat="server" />
        <asp:HiddenField ID="HiddenRoom" runat="server" />
        <asp:HiddenField ID="hdTimePerQuestion" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdTimeAll" runat="server" ClientIDMode="Static" />
    </form>
    <div id='DialogLV' title='เลือกชั้นเรียนที่ต้องการ' style="font-size: large;">
        <table style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
            <tr style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
                <td id="PClass" runat="server" style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;"></td>
            </tr>
            <tr style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
                <td id="MClass" runat="server" style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;"></td>
            </tr>
        </table>
    </div>
    <div id='DialogRoom' title='เลือกห้องทำควิซ ' style="font-size: large;">
        <table style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
            <tr id="TRRoom" style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
                <td id="TRoom" runat="server" style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;"></td>
            </tr>
        </table>
    </div>
    <div id="DialogAddRoom">
        <%-- <table>
            <tr>
                <td style="width: 230px;">เลขห้อง</td>
                <td style="width: 120px;">
                    <input type="text" id="txtRoomName" class="txtAddRoom" /></td>
                <td></td>
            </tr>
            <tr>
                <td>จำนวนนักเรียน</td>
                <td>
                    <input type="text" id="txtNoOfStudent" class="txtAddRoom" /></td>
                <td>คน</td>
            </tr>
        </table>
        <div>
            <input type="button" value="ตกลง" class="btnAddRoom" id="btnConfirmAddRoom" />
            <input type="button" value="แก้ไข" class="btnAddRoom" id="btnEditAddRoom" />
            <input type="button" value="ยกเลิก" class="btnAddRoom" id="btnCloseAddRoom" />
        </div>--%>
        <div class="StudentsManagement">
            <center>
                <label>รหัสนักเรียน</label></center>
            <div class="Students">
            </div>
            <div class="NoOfStudent">จำนวนนักเรียน 0 คน</div>
            <div class="StudentManagementBtn">
                <%--<input type="button" value="เพิ่ม/ลด จำนวนนักเรียน" style="width: 230px;" class="btnAddRoom" id="btnChangeNoOfStudent" />--%>
                <input type="button" value="ตกลง" class="btnAddRoom" id="btnConfirmAddRoom" />
                <input type="button" value="ตกลง" class="btnAddRoom" id="btnEditAddRoom" />
                <input type="button" value="ยกเลิก" class="btnAddRoom" id="btnCloseAddRoom" />
            </div>
        </div>
    </div>
    <% If BusinessTablet360.ClsKNSession.RunMode = "standalonenotablet" Then%>
    <div id='DialogChangeNoOfStudent' title='' style="font-size: large;">
        <div style="text-align: center; margin: 10px 0;">
            <label id="lblNoOfStudent"></label>
            <input type="text" id="txtNoOfStudent" style="width: 70px; margin: 0 5px;" maxlength="3" onkeypress="return event.charCode >= 48 && event.charCode <= 57" /><label> คน</label>
        </div>
        <div style="padding: 10px 0;">
            <input type="button" value="ตกลง" class="btnAddRoom" id="btnConfirmChangeNoOfStudent" />
            <input type="button" value="ยกเลิก" class="btnAddRoom" id="btnCancleChangeNoOfStudent" />
        </div>
    </div>
    <%End If%>
    <div id='DialogManageRoom' title='แก้ไขจัดการห้องเรียน' style="font-size: large;">
        <div id="divMangeRoom"></div>
    </div>
    <div id='DialogDeleteRoom' title='ต้องการลบห้องเรียน' style="font-size: large;">
        <div id="divDeleteRoom">
            <input type='button' id='btnCancleDelRoom' value='ยกเลิก' class='deleteRoom' style='margin-right: 50px;' />
            <input type='button' id='btnConfirmDelRoom' value='ตกลง' class='deleteRoom' />
        </div>
    </div>
    <div id="DialogDuplicateQuiz" title="ห้องที่เลือกกำลังทำควิซอยู่ค่ะ"></div>
    <div id="DialpgAlert" title="เลือกห้องเรียนก่อนเข้าทำควิซค่ะ"></div>
    <div id="DialogWarningOpenQuiz" title=""></div>
    <ul id="Help" runat="server" clientidmode="Static">
        <li class="about2" style="z-index: 99;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
            id="HelpLogin">
            <%--ช่วย<br />
            เหลือ<br />--%>
            <div style="margin-top: 7px;">
                <span style="font-size: initial; position: relative; left: 10px;">ช่วย</span>
            </div>
            <div>
                <span style="font-size: initial; position: relative; left: 8px;">เหลือ</span>
            </div>
        </a></li>
    </ul>
    <%--<ul id="ChangeFontSize">
        <li class="about" style="z-index: 99;">
            <div id="btnChangefontsize">
                <input type="button" value="0" onclick="SetFontSize(0)" id="btnFontSize0" /><input
                    type="button" value="1" onclick="SetFontSize(1)" id="btnFontSize1" /><input type="button"
                        value="2" onclick="SetFontSize(2)" id="btnFontSize2" /></div>
        </li>
    </ul>--%>
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
            $('form').append($btnLogout);
        });

        function Logout() {
            $dialogLogout.dialog('open');
        }
    </script>
</body>
</html>
<script type="text/javascript">
    $(function () {
        ChangeLv($('#lblLevel').text());
        GetStudentAmount($('#lblLevel').text(), $('#lblRoom').text());

        $('#DialpgAlert').dialog({
            autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                "ตกลง": function () {
                    $(this).dialog('close');
                }
            }
        });

        // button มาครอบหน้า btnOk เพื่อเช็คก่อนว่าจะเปิดควิซมั้ยถ้าห้องที่ทำควิซระดับน่้อยกว่าชุดควิซ        
        var IsCanQuiz = false;

        $("#btnOK").click(function (e) {
            if ($('#lblRoom').text() == "") {
                e.preventDefault();
                callDialogAlert("เลือกห้องเรียนก่อนเข้าทำควิซค่ะ");
                //$('#DialpgAlert').dialog('open');
                return false;
            }
            if (!CheckIsCanQuiz() && !IsCanQuiz) {
                e.preventDefault();
                $('#DialogWarningOpenQuiz').dialog("option", "title", ("<div style='width:500px;'>ชุดควิซที่เลือกมามีข้อสอบคนละระดับชั้นกับห้อง " + $('#lblLevel').text() + '/' + $('#lblRoom').text() + " อยู่ด้วยค่ะ ต้องการเริ่มควิซแน่นะคะ?</div>")).dialog('open');
                return false;
            }
            return true;
        });

        $('#DialogWarningOpenQuiz').dialog({
            autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                "ยกเลิก": function () {
                    $(this).dialog('close');
                },
                "ตกลง": function () {
                    IsCanQuiz = true;
                    $(this).dialog('close');
                    $("#btnOK").trigger('click');
                }
            }
        });

        //$("#btnCheckUseQuiz").hover(function () {
        //    $('#btnOK').trigger("mouseover");
        //}, function () {
        //    $('#btnOK').trigger("mouseout");
        //});
        //$("#btnCheckUseQuiz").click(function () {
        //    var ClassName = $('#lblLevel').text();
        //    if (classRoomCanMakeQuiz.indexOf(ClassName) = -1) {
        //        $('#btnOK').trigger("click");
        //    } else {
        //        //show dialog
        //        alert(1);
        //    }
        //});

    });
    function CheckIsCanQuiz() {
        var classRoomCanMakeQuiz = '<%=ClassRoomCanMakeQuiz%>';
        var ClassName = $('#lblLevel').text();
        console.log(classRoomCanMakeQuiz);
        console.log(ClassName);
        if (classRoomCanMakeQuiz.indexOf(ClassName) > -1) {
            return true;
        }
        return false;
    }
</script>

