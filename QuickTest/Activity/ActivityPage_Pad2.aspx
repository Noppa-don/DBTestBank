<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityPage_Pad2.aspx.vb" Inherits="QuickTest.ActivityPage_Pad2" EnableEventValidation="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%-- <%= NewRelic.Api.Agent.NewRelic.GetBrowserTimingHeader() %>--%>
    <title></title>

    <style type="text/css">
        .divDialy {
            display: none!important;
        }
        .btnsendreport {
            cursor: pointer;
            font: normal 20px 'THSarabunNew';
            font-weight: bold;
            width: 45%;
            border: 1px solid #ccc;
            background: #F6F6F6;
            color: #1C94C4;
            margin-top: 13px;
        }

        div.MainDivNormal {
            font: normal 35px 'THSarabunNew';
            background-color: #116551;
            margin-left: 0.5% !important;
            margin-top: 1% !important;
        }

        .MenuBackground {
            background-color: #ffdf30 !important;
        }

        div.MainQuestionAnswerNormal {
            overflow: auto;
            height: 100%;
            border-radius: 12px;
            overflow-x: hidden !important;
            width: auto;
        }

        div.prettyIntro {
            display: none;
        }

        div.QuestionNormal {
            position: relative;
            border-radius: 10px 10px 0px 0px;
            background-color: #ffc76f;
            width: 98%;
            border: 20px;
            padding: 20px;
        }

        .MainQuestionAnswerNormal p:first-child {
            display: inline;
        }

        #mainAnswer td p:first-child {
            display: inline;
        }

        #Hider {
            position: absolute;
            background-color: #00000094;
            z-index: 99;
            width: 100% !important;
            height: 100% !important;
            display: none;
        }

        #VS > img {
            width: 150px;
            height: 100px;
            position: relative;
            top: -180px;
        }

        #VS > div {
            width: 300px;
            height: 100px;
            background-color: white;
            position: relative;
            top: -140px;
        }

        .divDialy > img {
            width: 200px;
        }

        .divDialy > div {
            background-color: whitesmoke;
            width: 260px;
            height: 90px;
            line-height: 90px;
            border-radius: 10px;
            margin-right: auto;
            margin-left: auto;
        }

        span.drag {
            border: solid 1px;
            padding: 0 20px 0 20px;
            border-radius: 5.5px;
            box-shadow: inset 0 0 7px #06466b;
            border-color: #06466b;
            background-color: #bde2f7;
            cursor: pointer;
        }

        #DivEnemy {
            display: none !important;
        }

        #DivOwner {
            display: none !important;
        }

        #VS {
            display: none !important;
        }

        .tdMode {
            border-radius: 15px;
            line-height: 7px;
            padding: 10px;
            font-size: 21px;
            width: 199px;
            border: 2px solid #8c7a5e;
            background-color: white;
        }

        .ActiveMode {
            background-color: #fdf861 !important;
        }

        .HiddenMode {
            display: none;
        }
    </style>

    <link href="<%=ResolveUrl("~")%>css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />

    <script src="<%=ResolveUrl("~")%>js/jspad2.std.js" type="text/javascript"></script>
    <!-- jquery-1.7.1 + GFB + jquery-ui-1.8.18 -->
    <script src="<%=ResolveUrl("~")%>js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <link href="<%=ResolveUrl("~")%>css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>

    <script type="text/javascript">
        (function ($) {
            $(function () {
                var qsetPath = '<%=qsetFilePath%>';
                console.log(qsetPath);
                $('.imgSound').each(function () {
                    var fileName = $(this).attr('alt');
                    $(this).after($('<audio>', { src: qsetPath + fileName, controls: '' }));
                    $(this).remove();
                });

                $('#btnMoreDetail').toggle(function () {
                    $('#DivMoreDetailMenu').stop().show(500);
                }, function () {
                    $('#DivMoreDetailMenu').stop().hide(500);
                });
                $('#btnSendComment').click(function () {
                    OpenDivReportExam();
                });
                $('#panelSendComment').click(function () {
                    OpenDivReportExam();
                });
            });

            function OpenDivReportExam() {
                if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                    $('#MyShadow').show();
                    $('#DivReportExam').stop().show();
                } else {
                    $('#MyShadow').show();
                    $('#DivReportExam').stop().show(500);
                }
            }

        })(jQuery);

        function CloseDivReportExam() {
            $('#DivReportExam').css("display", "none");
        }
    </script>
    <style type="text/css">
        .divShowData {
            top: 50%;
            left: 50%;
            margin-top: -220px;
            margin-left: -390px;
            width: 780px;
            height: 440px;
            background-color: #DAF7A6;
            position: absolute;
            display: none;
            z-index: 9999;
            font: normal 0px "THSarabunNew";
            border-radius: 20px;
            padding-bottom: 10px;
        }

        .ForBtnChoice {
            margin-left: 6px;
            margin-bottom: 40px;
            -webkit-border-radius: 10px;
            border: 2px solid #FAA51A;
            width: 100px;
            float: left;
            height: 110px;
            position: relative;
            background-color: #F4F7FF;
            display: inline-block;
            text-align: center;
        }

            .ForBtnChoice > .ForBtn {
                margin: 0 !important;
                margin-top: 10px !important;
            }

            .ForBtnChoice > span {
                margin: 0 !important;
                font-size: 20px;
            }

        #btnMoreDetail {
            top: 3px;
            right: 16px;
            cursor: pointer;
            display: block;
        }
    </style>
    <link href="../css/pad2.self.css" rel="stylesheet" />
    <!-- styleActivity_pad + ActivityPage_Pad2  -->
    <%If JvIsSelfPace = True Then%>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/slides.min.jquery.js" type="text/javascript"></script>
    <%End If%>
    <%If EnableIntro = True Then%>
    <link href="../css/pad2.intro.css" rel="stylesheet" />
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <script type="text/javascript">// ข้อนั้นต้องแสดง intro หรือเปล่า
        $(document).ready(function () {
            var prettyIntro = $('.prettyIntro');
            var _body = $('body');
            $('#spnTest').click(function () {
                $(prettyIntro).css('display', 'block');
                $(_body).css('overflow-y', 'hidden');
            });
            $('#divClose').click(function () {
                $(prettyIntro).css('display', 'none');
                $(_body).css('overflow-y', 'scroll');
            });

        });
    </script>
    <%End If%>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .ImgCircle {
            position: absolute;
            top: 10px;
            left: -16px;
            display: none;
        }

        .ForSortBtn {
            font-size: 24px !important;
            height: 50px !important;
            width: 200px !important;
            background: -webkit-gradient(linear, left top, left bottom, from(#6395df), to(#3b30d8bd));
            border: none;
        }
    </style>
    <% If UseTools = True Then%>
    <link href="../css/styleEnabledTools.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQueryRotateCompressed.2.2.js" type="text/javascript"></script>
    <%If IsMobile Then %>
    <script type="text/javascript">
        // WORDBOOK 
        // js for control on wordbook
        $(function () {
            $('.Alphabet').live('click', function () {
                var alphabet = $(this).html();
                var QuestionId = $('#hdQuestionId').val();
                getPanelWordBook(QuestionId, alphabet, 0);
            });
            $('.backAlphabet').live('click touchstart', function () {
                var QuestionId = $('#hdQuestionId').val();
                getPanelAlphabet(QuestionId);
            });
            $('.nextAlphabet').live('click', function () {
                var alphabet = $(this).text();
                var QuestionId = $('#hdQuestionId').val();
                getPanelWordBook(QuestionId, alphabet, 1);
            });
            $('.Aclosed').on('touchstart', function () {
                $(this).parent().hide();
            });
        });
        function getPanelWordBook(QuestionId, Alphabet, ChangeAlphabet) {
            console.log('url' + ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createWordBook"));
            $.ajax({
                type: "POST",
                //url: "<%=ResolveUrl("~")%>WebForm5.aspx/createWordBook", 
                url: ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createWordBook"),
                data: "{QuestionId : '" + QuestionId + "', Alphabet : '" + Alphabet + "', ChangeAlphabet : '" + ChangeAlphabet + "' }",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    $('#wordbookMain').html(data.d);
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
        }
        function getPanelAlphabet(QuestionId) {
            $.ajax({
                type: "POST",
                //url: "<%=ResolveUrl("~")%>WebForm5.aspx/createWordBook", 
                url: ResolveUrl("ToolsActivity/WordBookAndNote.aspx/createAlphabet"),
                data: "{ QuestionId : '" + QuestionId + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    $('#wordbookMain').html(data.d);
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
        }

        function ResolveUrl(url) {
            if (url.indexOf("~/") == 0) {
                url = baseUrl + url.substring(2);
            }
            return url;
        }

        // Note
        // js for control note
        $(function () {
            $('#myNote').find('textarea').live('focusout', function () {
                var html = $(this).val();
                var UserId = $('#hdUserId').val();
                saveNote(html, UserId);
            });

        });
        function getPanelNote(UserId) {
            $.ajax({
                type: "POST",
                url: "ToolsActivity/WordBookAndNote.aspx/createNote",
                data: "{ UserId : '" + UserId + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    //$('.noteTabs').html(data.d);
                    $('#myNote').html(data.d);
                },
                error: function myfunction(request, status) {
                }
            });
        }
        function saveNote(myNote, UserId) {
            $.ajax({
                type: "POST",
                url: "ToolsActivity/WordBookAndNote.aspx/saveMyNote",
                data: "{ myNote : '" + myNote + "',UserId : '" + UserId + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                },
                error: function myfunction(request, status) {
                    alert('save Note ไม่ลง');
                }
            });
        }



        // Tools On Tablet
        $(function () {
            if ($('#PanelTools').length != 0) {
                new FastButton(document.getElementById('PanelTools'), TriggerToolClick);
            }
            //เมื่อกด
            if ($('.ForToolsTablet').length != 0) {
                $('.ForToolsTablet').each(function () {
                    new FastButton(this, getToolsForUseFromTablet);
                });
            }
            var sta = $('#hdStatusDict').val();
            if (sta == "On") {
                //turnOnDict();
                $('.btnDictionary').removeClass('DictOff').addClass('DictOn');
            }
        });

        function TriggerToolClick(e) {
            chooseSound.play();
            var obj = e.srcElement;
            if ($(obj).parent().is('#PanelTools')) {
                obj = $(obj).parent();
            } else if ($(obj).parent().parent().is('#PanelTools')) {
                obj = $(obj).parent().parent();
            }
            if ($(obj).hasClass('OpenTools')) {
                $(obj).removeClass('OpenTools');
                $('#ToolsOnTablet').hide();
            } else {
                $(obj).addClass('OpenTools');
                $('#ToolsOnTablet').show();
            }
        }

        function getToolsForUseFromTablet(e) {
            chooseSound.play();
            var obj = e.srcElement;
            if (($(obj).hasClass('btnNote'))) {
                //$('#tools_note').draggable({ cancel: '.content' }).show();
                $('#tools_note').show();
                var UserId = $('#hdUserId').val();
                getPanelNote(UserId);
                hideMenuToolsTablet();
            }
            else if (($(obj).hasClass('btnWordBook'))) {
                //$('#tools_wordbook').draggable({ cancel: '.notDraggable' }).show();
                $('#tools_wordbook').show();
                var QuestionId = $('#hdQuestionId').val();
                getPanelAlphabet(QuestionId);
                hideMenuToolsTablet();
            }
            else if (($(obj).hasClass('btnCalculator'))) {
                $('#tools_calculator').draggable({ handle: '#txtCalculator', cancel: '.btnCalculatorDiv' }).show();
                hideMenuToolsTablet();
            }
            else if (($(obj).hasClass('btnDictionary'))) {
                if ($(obj).hasClass('DictOn')) {
                    $(obj).removeClass('DictOn').addClass('DictOff');
                    turnOffDict();
                    //hideMenuToolsTablet();
                } else {
                    $(obj).removeClass('DictOff').addClass('DictOn');
                    turnOnDict();

                    var tooltip = 'right center'; var tooltipAt = 'left center';
                    $('#btnWordBookTop').qtip({
                        content: 'กดที่คำศัพท์<br/> แล้วจะเห็นคำแปลค่ะ',
                        show: { ready: true },
                        style: {
                            width: 185, padding: 0, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'rightMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '17px', 'line-height': '2em'
                        },
                        hide: false,
                        position: { corner: { tooltip: 'rightMiddle', target: 'middleRight' }, adjust: { x: -10, y: -20 } },
                        fixed: false
                    });

                    setTimeout(function () { $('#btnWordBookTop').qtip('destroy'); }, 3000);
                }
            }
            else if (($(obj).hasClass('btnProtractor'))) {
                $('#tools_protractor').draggable({ cancel: '.btnRotate' }).show();
                hideMenuToolsTablet();
            }
        }

        function hideMenuToolsTablet() {
            $('#ToolsOnTablet').hide();
            $('#PanelTools').removeClass('OpenTools');
        }
        function turnOnDict() {
            $('#hdStatusDict').val('On');
        }
        function turnOffDict() {
            $('#hdStatusDict').val('Off');
        }



        //Calculator
        var keepNum = '';
        function calculator(number) {
            var txtNum = document.getElementById('SumNumber').innerHTML;
            if (IsNumber(number)) {
                var s = keepNum.indexOf('.');
                if (txtNum == '0') {
                    if (number == '.') {
                        if (s == -1) {
                            document.getElementById('SumNumber').innerHTML += number;
                        }
                    } else {
                        NoOfNumber++;
                        document.getElementById('SumNumber').innerHTML = number;
                    }
                } else {
                    if (number == '.') {
                        if (s == -1) {
                            document.getElementById('SumNumber').innerHTML += number;
                        }
                    } else {
                        if (IsNotOverBaseCharacter(txtNum)) {
                            document.getElementById('SumNumber').innerHTML += number;
                        }
                    }
                }
                keepNum += number;
            } else {
                keepNum = '';
                txtNum = StrBeforeEval(txtNum);
                if (number == '=') {
                    document.getElementById('SumNumber').innerHTML = eval(txtNum);
                } else {
                    document.getElementById('SumNumber').innerHTML = txtNum + number;
                }
            }

        }
        function IsNumber(number) {
            if (number == '/' || number == '*' || number == '-' || number == '+' || number == '=') {
                NoOfNumber = 0;
                return false;
            } else {
                return true;
            }
        }
        function StrBeforeEval(str) {
            var s = str.substr(str.length - 1, 1);
            if (isNaN(s)) {
                str = str.substring(0, str.length - 1);
            }
            return str;
        }
        //กรอกตัวเลขแต่ละชุดได้ไม่เกิน 6 หลัก
        var NoOfNumber = 0;
        function IsNotOverBaseCharacter(Number) {

            if (NoOfNumber >= 6) {
                return false;
            }
            else {
                NoOfNumber++;
                return true;
            }
        }

        function ClearValue() {
            NoOfNumber = 0;
            document.getElementById("SumNumber").innerHTML = "0";
        }
        var times;
        window.addEventListener('load', function () {
            var btnRotateL = document.getElementById('btnRotateL')
            if (btnRotateL != null) {
                btnRotateL.addEventListener('touchstart', function (e) {
                    rotateProtractor("-")
                    e.preventDefault()
                }, false);
                btnRotateL.addEventListener('touchmove', function (e) {
                    e.preventDefault()
                }, false)
                btnRotateL.addEventListener('touchend', function (e) {
                    clearInterval(times);
                    e.preventDefault()
                }, false)

                var btnRotateR = document.getElementById('btnRotateR')
                btnRotateR.addEventListener('touchstart', function (e) {
                    rotateProtractor("+")
                    e.preventDefault()
                }, false);
                btnRotateR.addEventListener('touchmove', function (e) {
                    e.preventDefault()
                }, false);
                btnRotateR.addEventListener('touchend', function (e) {
                    clearInterval(times);
                    e.preventDefault()
                }, false);
            }
        }, false);

        var operators = {
            '+': function (a, b) { return a + b },
            '-': function (a, b) { return a - b }
        };
        function rotateProtractor(op) {
            var t = 0;
            var divImg = document.getElementById('divImg');
            var curAngle = parseInt($('#divImg').getRotateAngle());
            curAngle = isNaN(curAngle) ? 0 : curAngle;
            var val;
            times = setInterval(function () {
                if (t >= 0 && t <= 5) {
                    t = t + 1;
                }
                else if (t >= 6 && t <= 10) {
                    t = t + 5;
                }
                else {
                    t = t + 10;
                }
                val = operators[op](curAngle, t);
                $('#divImg').rotate({ angle: eval(val), easing: $.easing.easeInOutElastic });
            }, 80);
        };
    </script>
    <%Else %>
    <script src="../js/jsEnabledTools2.js" type="text/javascript"></script>
    <%End If %>
    <%--<script src="../js/JQueryMobile.js"  type="text/javascript"></script>--%>
    <script type="text/javascript">
        (function ($) {
            function startHandler(event) {
                var $elem = jQuery(this);
                if (typeof $elem.data("events") != "undefined"
                    && typeof $elem.data("events").click != "underfined") {
                    for (var c in $elem.data("events").click) {
                        if ($elem.data("events").click[c].namespace == "") {
                            var handler = $elem.data("events").click[c].handler
                            $elem.data("taphold_click_handler", handler);
                            $elem.unbind("click", handler);
                            break;
                        }
                    }
                } else if (typeof event.data != "undefined"
                    && event.data != null
                    && typeof event.data.clickHandler == "function") {
                    $elem.data("taphold_click_handler", event.data.clickHandler);
                }
                $elem.data("taphold_triggered", false);
                $elem.data("taphold_clicked", false);
                $elem.data("taphold_cancelled", false);

                $elem.data("taphold_timer",
                    setTimeout(function () {
                        if (!$elem.data("taphold_cancelled")
                            && !$elem.data("taphold_clicked")) {
                            $elem.trigger(jQuery.extend(event, jQuery.Event("taphold")));
                            $elem.data("taphold_triggered", true);
                        }
                    }, 750));

            }

            function stopHandler(event) {
                var $elem = jQuery(this);
                if ($elem.data("taphold_cancelled")) { return; }
                clearTimeout($elem.data("taphold_timer"));
                if (!$elem.data("")
                    && !$elem.data("")) {
                    if (typeof $elem.data("taphold_click_handler") == "function") {
                        $elem.data("taphold_click_handler")(jQuery.extend(event, jQuery.Event('click')));
                    }
                    $elem.data("taphold_clicked", true);
                }
            }

            function leaveHandler(event) {
                $(this).data("taphold_cancelled", true);
            }

            var taphold = $.event.special.taphold = {
                setup: function (data) {
                    $(this).bind("touchstart mousedown", data, startHandler)
                    .bind("touchend mouseup", stopHandler)
                    .bind("touchmove mouseleave", leaveHandler);
                },
                teardown: function (namespaces) {
                    $(this).unbind("touchstart mousedown", startHandler)
                    .unbind("touchend mouseup", stopHandler)
                    .unbind("touchmove mouseleave", leaveHandler);
                }
            };
        })(jQuery);
    </script>
    <script type="text/javascript">
        $(function () {
            $('.wordsDict').live('click', function (e) {
                getSelectedOnTablet(e.target.innerHTML);
                $('span').removeClass('spanHilight');
                $(this).addClass('spanHilight');
            });

            $('#mainQuestion').on('taphold', function () {
                GetWordSeleter();
            });
            $('#mainAnswer').on('taphold', function () {
                GetWordSeleter();
            });

        });
        function GetWordSeleter() {
            if ($('#hdStatusDict').val() == "On") {
                var t = '';
                if (window.getSelection && (sel = window.getSelection()).modify) {
                    var s = window.getSelection();
                    console.log("window.getSelection() = " + s);
                    if (s.isCollapsed) {
                        s.modify('move', 'forward', 'character');
                        s.modify('move', 'backward', 'word');
                        s.modify('extend', 'forward', 'word');
                        t = s.toString();
                        s.modify('move', 'forward', 'character');
                    } else {
                        t = s.toString();
                    }
                } else if ((sel = document.selection) && sel.type != "Control") {
                    var textRange = sel.createRange();
                    if (!textRange.text) {
                        textRange.expand("word");
                    }
                    while (/\s$/.test(textRange.text)) {
                        textRange.moveEnd("character", -1);
                    }
                    t = textRange.text;
                }
                //console.log(t);
                t = t.replace("?", "").replace(",", "").replace("!", "");
                if (t.lastIndexOf(".", t.length - 1)) t = t.replace(".", "");
                getSelectedOnTablet(t);
            }
        }
        function getSelectedOnTablet(selection) {
            selection = selection.replace(/\_/g, '');
            selection = $.trim(selection);
            if (IsWord(selection) == true) {
                selection = '';
            }
            if (selection != "") {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>selectedSearch/selectedSearch.aspx/translateFromSelected",
                    data: "{ selectedText : '" + selection + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var dict = jQuery.parseJSON(data.d);
                        var headDict = selection.toString() + dict.returnFileMp3.toString();
                        var contentDict = dict.returnValue;
                        if (contentDict == '') {
                            contentDict = 'ไม่มีคำแปลค่ะ';
                        }
                        $('#tools_dictionary .headDict').html(headDict);
                        $('#tools_dictionary .contentDict').html(contentDict);
                        $('#tools_dictionary').show();
                    },
                    error: function myfunction(request, status) {
                    }
                });
            } else {
                $('.ui-tooltip-tipped').remove();
            }
        }
        function IsWord(w) {
            var a = w.split(' ');
            if (a.length > 1) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <%End If%>

    <% If F5 = "1" Then%>
    <script type="text/javascript">
        $(function () {
            GetTime(1);
        });
        function GetTime(a) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad2.aspx/GetCurrentTime",
                //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/CreateStringLeapChoice",          
                data: "{ IsStart:'" + a + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    // alert(data.d);
                },
                error: function myfunction(request, status) {
                    // alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
    </script>
    <%End If%>
    <script type="text/javascript">
        var DeviceId = '<%=DeviceId %>';
        var PlayerId = '<%=PlayerId %>';
        var SupportQuestionId = '<%= questionId%>';
        var Examnum = '<%=ExamNum%>'; var NotReplyMode; var AnswerState = '<%=_AnswerState %>';
        var SignalRCheck;
        var JSQuizIdForShowScore = '<%=Session("QuizIdForShowScore") %>';//เก็บ QuizId สำหรับหน้า ShowScore
        var JvIsSelfPace = '<%=JvIsSelfPace %>';
        var JvIsPracticeMode = '<%=IsPracticeMode %>';//เก็บค่าว่าเป็นโหมดฝึกฝนหรือเปล่า  
        var ShowScoreAfterComplete = '<%=IsShowScoreAfterCompleteState %>';
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var EPage = '<%=EPageNumber %>';
    </script>
    <script type="text/javascript">        // script ใช้ทั้ง 2 โหมด

        $(function () {

            var CheckNoDataFromJs = '<%=CheckNoData %>';
            if (CheckNoDataFromJs == 1) {
                $.blockUI({
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#000',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    },
                    message: '<img src="<%=ResolveUrl("~")%>/Images/Activity/wait.png" /> รอสักครู่ค่ะ'
                });
            }

            $('#ShowScore').toggle(function () {
                $('a', $(this)).stop().animate({ 'marginTop': '35px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginTop': '0px' }, 200);
            });

            $('#NextSlide').click(function () {

                PageNumber = parseInt(PageNumber) + 1;

                if ($('#HDIsSort').val() == 'False') {
                    CreateButtonLeapChoice("False");
                } else {
                    CreateButtonLeapChoice("True");
                }

            });

            $('#BackSlide').click(function () {

                PageNumber = parseInt(PageNumber) - 1;

                if ($('#HDIsSort').val() == 'False') {
                    CreateButtonLeapChoice("False");
                } else {
                    CreateButtonLeapChoice("True");
                }

            });

            $('#NextExplainPage').click(function () {
                EPage = parseInt(EPage) + 1;
                CreateButtonSelectExplain(EPage);
            });

            $('#BackExplainPage').click(function () {

                EPage = parseInt(EPage) - 1;
                CreateButtonSelectExplain(EPage);
            });

            $('#btnAllQuestion').click(function () {
                $('#AnsweredMode').val(0);
                CreateButtonSelectExplain(1)
                SetActiveModeButton();
            });

            $('#btnRightQuestion').click(function () {
                $('#AnsweredMode').val(1);
                CreateButtonSelectExplain(1)
                SetActiveModeButton();
            });

            $('#btnWrongQuestion').click(function () {
                $('#AnsweredMode').val(2);
                CreateButtonSelectExplain(1)
                SetActiveModeButton();
            });

            $('#btnSkipQuestion').click(function () {
                $('#AnsweredMode').val(3);
                CreateButtonSelectExplain(1)
                SetActiveModeButton();
            });

        });

        function SetActiveModeButton() {
            if ($('#AnsweredMode').val() == 0) {
                $('#btnAllQuestion').addClass("ActiveMode");
                $('#btnRightQuestion').removeClass("ActiveMode");
                $('#btnWrongQuestion').removeClass("ActiveMode");
                $('#btnSkipQuestion').removeClass("ActiveMode");
            } else if ($('#AnsweredMode').val() == 1) {
                $('#btnAllQuestion').removeClass("ActiveMode");
                $('#btnRightQuestion').addClass("ActiveMode");
                $('#btnWrongQuestion').removeClass("ActiveMode");
                $('#btnSkipQuestion').removeClass("ActiveMode");
            } else if ($('#AnsweredMode').val() == 2) {
                $('#btnAllQuestion').removeClass("ActiveMode");
                $('#btnRightQuestion').removeClass("ActiveMode");
                $('#btnWrongQuestion').addClass("ActiveMode");
                $('#btnSkipQuestion').removeClass("ActiveMode");
            } else if ($('#AnsweredMode').val() == 3) {
                $('#btnAllQuestion').removeClass("ActiveMode");
                $('#btnRightQuestion').removeClass("ActiveMode");
                $('#btnWrongQuestion').removeClass("ActiveMode");
                $('#btnSkipQuestion').addClass("ActiveMode");
            }
        }

        function ContactErrorSupport(UserId, QuestionId) {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>Support/SendErrorToSupport.aspx?UserId=' + UserId + '&QuestionId=' + QuestionId,
                'type': 'iframe',
                'width': 750,
                'minHeight': 550
            });
        }

    </script>
    <script type="text/javascript">        // mode ไปพร้อมกัน
        $(function () {
            parent.CloseBlockUI(); // ปิด blockui หน้าแรก

            if (JvIsSelfPace == 'False') {
                $('#DivMainQuestionAnswer').css('height', 'auto');
                $('#mainQuestion').css('width', 'auto');
                $('#mainAnswer').css('width', 'auto');
            }
        });
    </script>
    <% If JvIsSelfPace = True Then%>
    <script type="text/javascript">
        var CheckLastQuestion = '<%=Dialog %>';
        var TokenId = '<%=TokenId%>';
        var exNum = '<%=ExamNum%>';
        console.log(CheckLastQuestion);
        $(function () {
            if (JvIsSelfPace == 'True') {
                <% If _AnswerState = 2 Then%>
                getModeQuizAndTimer(false);
                <% Else %>
                if (exNum != 1) {
                    getModeQuizAndTimer(false); // get เวลาด้วย
                }
                <% End If %>
                //ปุ่ม ข้อข้าม
                if ($('#BtnSwapQuestion').length != 0) {
                    new FastButton(document.getElementById('BtnSwapQuestion'), TriggerClickBtnSwapQuestion);
                }
                //ปุ่ม ทวนข้อข้าม ใน Panel ข้ามข้อ
                if ($('#BtnSortAnswerNull').length != 0) {
                    new FastButton(document.getElementById('BtnSortAnswerNull'), TriggerClickBtnSortAnswerNull);
                }
                //ปุ่ม เรียงตามลำดับ ใน Panel ข้ามข้อ
                if ($('#BtnSortNormal').length != 0) {
                    new FastButton(document.getElementById('BtnSortNormal'), TriggerClickBtnSortNormal);
                }
                //ปุ่ม ทำต่อข้อล่าสุด ใน Panel ข้ามข้อ
                if ($('#BtnNextToLastChoice').length != 0) {
                    new FastButton(document.getElementById('BtnNextToLastChoice'), TriggerClickBtnNextToLastChoice);
                }
                // กดปุ่ม next ข้อสุดท้ายขึ้น dialog        
                if ($('#btnNext').length != 0) {
                    new FastButton(document.getElementById("btnNext"), TriggerClickNext);
                }
                // กดปุ่ม Back
                if ($('#BtnPrev').length != 0) {
                    new FastButton(document.getElementById("BtnPrev"), TriggerClicBack);
                }
                // ปุ่มออก Quiz
                if ($('#BtnExitQuiz').length != 0) {
                    new FastButton(document.getElementById("BtnExitQuiz"), TriggerClickExitQuiz);
                }
                // ปุ่มออก Homework
                if ($('#BtnExitQuiz').length != 0) {
                    new FastButton(document.getElementById("BtnExitQuiz"), TriggerClickExitHomework);
                }
                if ($('#BtnCompleteHomework').length != 0) {
                    new FastButton(document.getElementById("BtnCompleteHomework"), TriggerClickCompleteHomework);
                }

                var CheckIsSeenIntro = '<%=IsSeenIntro %>';
                var NeedShowTip = '<%=NeedShowTip%>'; // show qtip 3 times
                if (CheckIsSeenIntro != 'True') {
                    <%If BusinessTablet360.ClsKNSession.IsMaxONet Then %>
                    JvIsPracticeMode = 'True';
                    <%End If%>
                    if (JvIsPracticeMode == 'True') {
                        var randomSound = Math.floor(Math.random() * 10) + 1;
                        $('#DivBlock').show()
                    };
                }
                $('#btnStartQuiz').click(function () {
                    getModeQuizAndTimer(true);
                    $('#DivBlock').hide()
                });

                $('.divDialy').children('div').click(function () {
                    PlaySoundIntroVS(randomSound);
                    getModeQuizAndTimer(true);
                    $('#DivBlock').hide()
                });
            }
        });

        function PlaySoundIntroVS(NumberOfFile) {
            parent.playBgMusic();
        }

        function CreateButtonLeapChoice(IsNormalSort) {
            if (IsNormalSort == "False") { $('#slideDiv').css({ "background-color": "#bdeac5" }); }
            var AnswerState = '<%=_AnswerState %>';
            if ((AnswerState == '1') || (AnswerState == '0')) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/CreateStringLeapChoice",	  //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/CreateStringLeapChoice",          
                    data: "{ IsNormalSort:'" + IsNormalSort + "',StudentId:'" + PlayerId + "',ExamNum:'" + Examnum + "',PageNumber:'" + PageNumber + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var Data = jQuery.parseJSON(data.d);
                        $('#LeapChoiceDiv').html(Data.HtmlLeapChoice);
                        UseSlide();
                        var JsCheckOverOnePage = Data.CheckOverOnePage;
                        var JsCheckIsLastPage = Data.IsLastPage;
                        if (JsCheckOverOnePage > '10') {
                            if (PageNumber == '1') {
                                $('#NextSlide').css('display', 'block');
                                $('#BackSlide').css('display', 'none');
                            } else {
                                if (JsCheckIsLastPage == 'True') {
                                    $('#NextSlide').css('display', 'none');
                                    $('#BackSlide').css('display', 'block');
                                } else {
                                    $('#NextSlide').css('display', 'block');
                                    $('#BackSlide').css('display', 'block');
                                }
                            }
                        } else {
                            $('#NextSlide').css('display', 'none');
                            $('#BackSlide').css('display', 'none');
                        }
                        var JsCheckPageNumber = Data.PageNumber;
                        PageNumber = JsCheckPageNumber;

                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        }

        function CreateButtonSelectExplain(PageNumber) {
            var AnswerState = '<%=_AnswerState %>';
            var AnsMode = $('#AnsweredMode').val()

            var isShowAnswerComplete = '<%=Session("ShowSelectExamPanel")%>';
            if (isShowAnswerComplete) {

                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/CreateStringSelectExplain",
                    data: "{ StudentId:'" + PlayerId + "',ExamNum:'" + Examnum + "',PageNumber:'" + PageNumber + "',AnsweredMode:'" + AnsMode + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var Data = jQuery.parseJSON(data.d);
                        $('#divEachSelectExplain').html(Data.htmlExplainAnswer);
                        UseSlide2();
                        $('#Hider').stop().show();
                        var JsCheckOverOnePage = Data.CheckOverOnePage;
                        var JsCheckIsLastPage = Data.IsLastPage;
                        if (JsCheckOverOnePage > '10') {
                            if (PageNumber == '1' || PageNumber == '0') {
                                $('#NextExplainPage').css('display', 'block');
                                $('#BackExplainPage').css('display', 'none');
                            } else {
                                if (JsCheckIsLastPage == 'True') {
                                    $('#NextExplainPage').css('display', 'none');
                                    $('#BackExplainPage').css('display', 'block');
                                } else {
                                    $('#NextExplainPage').css('display', 'block');
                                    $('#BackExplainPage').css('display', 'block');
                                }
                            }
                        } else {
                            $('#NextExplainPage').css('display', 'none');
                            $('#BackExplainPage').css('display', 'none');
                        }
                        var JsCheckPageNumber = Data.PageNumber;
                        EPage = JsCheckPageNumber;
                        if (AnsMode == 0) {
                            $('#spnSummary').html('ได้ ' + Data.PlayerScore + '/' + Data.QuizTotalScore + ' คะแนน');
                            $('#spnSummary1').html('ทั้งหมด ' + Data.QuizTotalScore + ' ข้อ');
                            $('#spnSummary2').html('ตอบถูก ' + Data.RightAmount + ' ข้อ');
                            $('#spnSummary3').html('ตอบผิด ' + Data.WrongAmount + ' ข้อ');
                            $('#spnSummary4').html('ข้ามไม่ตอบ ' + Data.SkipAmount + ' ข้อ');
                        }

                        $('#ArrNo').val(Data.ArrNo);
                        $('#LastQuestionNo').val(Data.LastQuestionNo);

                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        }

        function ShowNextPrevSlide() {
            var IsShowNextSlide = '<%=IsShowNextSlide %>';
            if (IsShowNextSlide == 'True') {
                $('#SlideNext').css({ "display": "block" });
                $('#SlidePrev').css({ "display": "block" });
            } else {
                $('#SlideNext').css({ "display": "none" });
                $('#SlidePrev').css({ "display": "none" });
            }
        }

        function UseSlide() {
            if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                $('#slideDiv').slides({
                    generatePagination: false,
                    slideSpeed: 0,
                    fadeSpeed: 0
                });
            }
            else {
                $('#slideDiv').slides({
                    generatePagination: false
                });
            }
        }

        function UseSlide2() {
            if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                $('#divSelectExplain').slides({
                    generatePagination: false,
                    slideSpeed: 0,
                    fadeSpeed: 0
                });
            }
            else {
                $('#divSelectExplain').slides({
                    generatePagination: false
                });
            }
        }
        function LeapChoiceOnclick(QQ_No, IsScore, NotReplyMode, OrderNo) {
             <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
            parent.OpenBlockUI();
                     <%End If%>

            $('#HDLastChoice').val('False');
            $('#HDIsLeapChoice').val('True');

            var CheckIsNeedTimerPerQuestion = '<%=VBCheckIsNeedTimerPerQuestion %>';
            if (CheckIsNeedTimerPerQuestion == 'False') {
                $('#HDQQ_No').val(QQ_No);
                $('#HDIsScore').val(IsScore);

                if (NotReplyMode == 'T') {
                    $('#HDNotReplyMode').val('True');

                }
                else {
                    $('#HDQQ_No').val(QQ_No);
                    $('#HDNotReplyMode').val('False');
                }

                var AnswerState = '<%=_AnswerState %>';
                if (AnswerState == 2) {
                    $('#OrderNo').val(OrderNo);
                }

                form1.submit();
            }
            else {
                //alert('ไม่สามารถกลับได้เพราะเป็นโหมดแบบจับเวลาข้อต่อข้อ');
            }
            //alert($('#HDIsLeapChoice').val());
        }

        function TriggerClickBtnSwapQuestion(e) {
            chooseSound.play();
            var AnswerState = '<%=_AnswerState %>';
            if ((isMaxOnet == 'True') && (AnswerState == '2')) {
                if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                    $('#divSelectExplain').stop().show();
                    $('#Hider').stop().show();
                }
                else {
                    $('#divSelectExplain').stop().show(500);
                    $('#Hider').stop().show(500);
                }
                PageNumber = 0;
                CreateButtonSelectExplain(PageNumber);
                SetActiveModeButton();
            } else {
                var CheckIsNeedTimerPerQuestion = '<%=VBCheckIsNeedTimerPerQuestion %>';
                if (CheckIsNeedTimerPerQuestion == "False") {
                    if ($('#HDStatusHomework').val() !== '1') {
                        if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                            //$('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show();
                            $('#slideDiv').stop().show();
                            $('#Hider').stop().show();
                        }
                        else {
                            //$('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show(500);
                            $('#slideDiv').stop().show(500);
                            $('#Hider').stop().show(500);
                        }
                    }
                }
                $('#slideDiv').css({ "background-color": "#b1dcf5" });
                $('#HDNotReplyMode').val("False");
                PageNumber = 0;
                $('#HDIsSort').val("True");
                CreateButtonLeapChoice("True");
            }
        }
        function TriggerClickBtnSortAnswerNull() {
            $('#slideDiv').css({ "background-color": "#bdeac5" });
            $('#HDNotReplyMode').val("True");
            PageNumber = 1;
            $('#HDIsSort').val("False");
            CreateButtonLeapChoice("False");
        };

        function TriggerClickBtnSortNormal() {
            $('#slideDiv').css({ "background-color": "#b1dcf5" });
            $('#HDNotReplyMode').val("False");
            PageNumber = 0;
            $('#HDIsSort').val("True");
            CreateButtonLeapChoice("True");
        };

        function TriggerClickBtnNextToLastChoice() {

            $('#slideDiv').css("display", "none");
            $('#Hider').css("display", "none");
            $('#HDNotReplyMode').val("True");
            $('#HDLastChoice').val("True");

            form1.submit();
        };

        function TriggerClickPanelTools(e) {

            var obj = e.target;
            if ($(obj).attr('id') == 'PanelTools') {
                $(obj).css('background', '#56C5FF');
            }
            else {
                $(obj).parent().css('background', '#56C5FF');
            }
            $(obj).trigger('click');
        }

        function TriggerbtnNextCI() {
            $('#btnNext').trigger('click');
        }
        function TriggerbtnPrevCI() {
            $('#BtnPrev').trigger('click');
        }

        function TriggerClickNext(e) {
            var obj = e.srcElement;
            if (CheckLastQuestion == 'True') {
                e.preventDefault();
                GetRenderForNextStep(1);
                return 0;
            }
            else {
                OpenBlockUI();
                if (parent.responseTimeCI == 0) { parent.OpenBlockUI(); }
                if (parent.isMaxOnet == "True") {
                    nextQuestion.play();
                    // ต้องทำให้มันกด next อีกครั้ง สำหรับ test ส่วนสถานการณ์จรืง ต้อง refresh
                    parent.refreshUrl = "N";
                    if (parent.responseTimeCI != 0 && isRefreshPage == false) { return 0; }
                }
            }
            $(obj).trigger('click');
        }
        function TriggerClicBack(e) {
            var obj = e.srcElement;
            OpenBlockUI();
            if (parent.responseTimeCI == 0) { parent.OpenBlockUI(); }
            if (parent.isMaxOnet == "True") {
                previousQuestion.play();
                parent.refreshUrl = "P";
                if (parent.responseTimeCI != 0 && isRefreshPage == false) { return 0; }
            }
            $(obj).trigger('click');
        }

        function TriggerClickExitQuiz(e) {
            chooseSound.play();
            e.preventDefault();
            GetRenderForNextStep(2);
        }

        function TriggerClickCompleteHomework(e) {
            var obj = e.target;
            $(obj).css('background-image', 'url(../Images/Activity/activitypagepad2.png)');
            $(obj).css('background-position', '-90px -255px');
            e.preventDefault();
            GetRenderForNextStep(3);
        }

        function TriggerClickExitHomework(e) {
            var obj = e.target;
            e.preventDefault();
            GetRenderForNextStep(2);
        }

        function CallDialog(btnConfirmName, btnCancleName, txtForShowDialog, NextStep, eventType) {
            var myBtn = {};
            myBtn[btnConfirmName] = function () {
                chooseSound.play();

                if (eventType == 1) {
                    $(this).dialog('close');
                } else if (eventType == 2) {
                    // NextToLeapChoice(1);
                    $(this).dialog('close');
                }
                else if (eventType == 3 || eventType == 5) {
                    //alert('confirm');
                    $('#LinkDialog').dialog('close');
                    if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                        //$('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show();
                        $('#slideDiv').stop().show();
                        $('#Hider').stop().show();
                    }
                    else {
                        //$('#slideDiv').css({ "background-color": "#FFEE2D", "width": "780px", "height": "450px", "opacity": "initial", "margin": "5px 0 0 35px", "overflow": "initial" }).stop().show(500);
                        $('#slideDiv').css({ "width": "780px", "height": "450px", "opacity": "initial", "margin": "5px 0 0 35px", "overflow": "initial" }).stop().show(500);

                        $('#Hider').stop().show(500);
                    }
                    $('#HDIsSort').val("False");
                    PageNumber = 1;
                    CreateButtonLeapChoice("False");
                } else if (eventType == 4) {
                    $('#LinkDialog').dialog('close');
                    if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                        //$('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show();
                        $('#slideDiv').stop().show();
                        $('#Hider').stop().show();
                    }
                    else {
                        // $('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show(500);
                        $('#slideDiv').stop().show(500);
                        $('#Hider').stop().show(500);
                    }
                    $('#HDIsSort').val("False");
                    PageNumber = 1;
                    CreateButtonLeapChoice("False");
                    $('#Hider').css("display", "none");
                    //UpdateHomework(NextStep);
                } else if (eventType == 6) {
                    $(this).dialog('close');
                } else if (eventType == 7) {
                    UpdateHomework(NextStep);
                }
                else if (eventType == 8) {
                    $(this).dialog('close');
                } else if (eventType == 9) {
                    $(this).dialog('close');
                } else if (eventType == 10) {
                    $(this).dialog('close');
                } else if (eventType == 11) {
                    $(this).dialog('close');
                } else if (eventType == 12 || eventType == 13) {
                    $(this).dialog('close');
                }
            };
            myBtn[btnCancleName] = function () {
                chooseSound.play();
                <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
                NextStep = '../PracticeMode_Pad/ChooseTestsetMaxOnet.aspx?deviceUniqueId=' + DeviceId + '&token=' + TokenId;
                    <%End If%>
                if (eventType == 1 || eventType == 2) {
                    <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
                    parent.OpenBlockUI();
                     <%End If%>
                    parent.location.href = NextStep;
                } else if (eventType == 6) {
                    UpdateHomework(NextStep);
                }
                else if (eventType == 3) {
                    NextToLeapChoice(1);
                } else if (eventType == 4) {
                    UpdateHomework(NextStep);
                } else if (eventType == 5 || eventType == 7) {
                    parent.location.href = NextStep;
                } else if (eventType == 8) {
                    NextToLeapChoice(1);
                } else if (eventType == 9) {
                    parent.location.href = NextStep;
                } else if (eventType == 10) {
                    parent.location.href = NextStep;
                } else if (eventType == 11) {
                    NextToLeapChoice(1);
                } else if (eventType == 12 || eventType == 13) {
                    parent.location.href = NextStep;
                }
            };

            $('#LinkDialog').dialog({
                autoOpen: false,
                buttons: myBtn,
                draggable: false, resizable: false, modal: true
            }).dialog('option', 'title', txtForShowDialog).dialog('open');
            //if ($('.ui-button').length != 0) {
            //    $('.ui-button').each(function () {
            //        new FastButton(this, TriggerServerButton);
            //    });
            //}

        }
        function GetRenderForNextStep(EventType) {
            var EType = EventType;
            var IsShowScore = '<%=IsShowScoreAfterCompleteState %>';
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetRenderForNextStep",   //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/GetRenderForNextStep",                  
                data: "{ EventType: '" + EType + "', DeviceId: '" + DeviceId + "', _AnswerState: '" + AnswerState + "', IsShowScoreAfterCompleteState: '" + IsShowScore + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    var ShowDialog = jQuery.parseJSON(data.d);
                    var DialogType = ShowDialog.DialogType;
                    var IsShowDialog = ShowDialog.IsShowDialog;
                    var txtForShowDialog = ShowDialog.txtForShowDialog;
                    var NextStep = ShowDialog.NextStep;
                    if (IsShowDialog == 'True') {
                        if (DialogType == '1') {
                            console.log(1);
                            CallDialog("ยังก่อน", "จบเลย", txtForShowDialog, NextStep, 1);
                        } else if (DialogType == '2') {
                            console.log(2);
                            CallDialog("ยังก่อน", "ออกเลย", txtForShowDialog, NextStep, 2);
                        } else if (DialogType == '3') {
                            console.log(3);
                            CallDialog("ทบทวน", "เฉลยเลย", txtForShowDialog, NextStep, 3);
                        } else if (DialogType == '4') {
                            console.log(4);
                            CallDialog("ทบทวน", "ส่งเลย", txtForShowDialog, NextStep, 4);
                        } else if (DialogType == '5') {
                            console.log(5);
                            CallDialog("ทบทวน", "ออกเลย", txtForShowDialog, NextStep, 5);
                        } else if (DialogType == '6') {
                            console.log(6);
                            CallDialog("ยังก่อน", "ส่งเลย", txtForShowDialog, NextStep, 6);
                        } else if (DialogType == '7') {
                            console.log(7);
                            CallDialog("ส่งเลย", "ออกเลย", txtForShowDialog, NextStep, 7);
                        } else if (DialogType == '8') {
                            console.log(8);
                            CallDialog("ทบทวน", "เฉลยเลย", txtForShowDialog, NextStep, 8);
                        } else if (DialogType == '9') {
                            console.log(9);
                            CallDialog("ทบทวน", "ดูคะแนนเลย", txtForShowDialog, NextStep, 9);
                        } else if (DialogType == '11') {
                            console.log(11);
                            CallDialog("ทบทวน", "เฉลยเลย", txtForShowDialog, NextStep, 11);
                        } else if (DialogType == '12') {
                            console.log(12);
                            CallDialog("ทบทวน", "ดูคะแนนเลย", txtForShowDialog, NextStep, 12);
                        } else if (DialogType == '13') {
                            console.log(13);
                            CallDialog("ทบทวน", "จบเลย", txtForShowDialog, NextStep, 13);
                        } else {
                            console.log(14);
                            NextToLeapChoice(1);
                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }
        function NextToLeapChoice(IsCorrect) {
<%--              <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
            parent.OpenBlockUI();
                     <%End If%>--%>
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/NextToLeapChoice",      //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/NextToLeapChoice",             
                data: "{ IsCorrect : '" + IsCorrect + "', PlayerId : '" + PlayerId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    CheckLastQuestion = data.d;
                    //console.log('next to leapchoice = ' + CheckLastQuestion);                 
                    $('#LinkDialog').dialog('close');
                    $('#dialogIsLastQuestion').dialog('close');
                    setTimeout(function () { $('#btnNext').trigger('click'); }, 1000);
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                    parent.CloseBlockUI();
                }
            });
        }


        function CloseDivnot() {
            $('#DivNotHaveDontReplyChoice').css("display", "none");
        }
        function CloseDivSlide() {
            $('#slideDiv').css("display", "none");
            $('#Hider').css("display", "none");
        }
        function CloseDivSelectExplain() {
            $('#divSelectExplain').css("display", "none");
            $('#Hider').css("display", "none");
        }

        function UpdateHomework(NextStep) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad2.aspx/UpdateExitByUser",
                async: false,
                data: "{ IsHomework :  '1'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) { //session(UserId) หายจะ return 0 - Sefety function
                        valReturnFromCodeBehide = msg.d;
                        $('#LinkDialog').dialog({
                            autoOpen: false,
                            buttons: {
                                'ตกลง': function () {
                                    if (NextStep == '') { $(this).dialog('close'); }
                                    else { parent.location.href = NextStep }

                                }
                            },
                            draggable: false,
                            resizable: false,
                            modal: true
                        }).dialog('option', 'title', 'ส่งการบ้านเรียบร้อยค่ะ').dialog('open');
                    }
                },
                error: function myfunction(request, status) {
                }
            });
        }
        function getModeQuizAndTimer(isStartQuiz) {
            console.log("getModeQuizAndTimer");
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetModeQuizAndTimerStudent",
                data: "{ _AnswerState : '" + AnswerState + "',IsStartQuiz  : '" + isStartQuiz + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d === "ERROR") return 0;
                    var needTimer = jQuery.parseJSON(data.d);
                    var minute; var second;
                    if (needTimer.NeedTimer == true) {
                        $('#TimeCountDown').show();
                        if (needTimer.timerType == true) {
                            minute = 0;
                            second = 0;
                            var sums = parseInt((minute * 60) + second);
                            $('#keepSumseconds').val(sums);
                            timeCountDown(minute, second, true);
                        }
                        else {
                            minute = 0;
                            second = 0;
                            var timeTotal = needTimer.timeTotal * 60;

                            $('#keepSumseconds').val(timeTotal);
                            timeCountDown(minute, second, false, needTimer.noWatch);
                        }
                    }
                    else {
                        if (needTimer.IsHomeWork == true) {
                            $('#TimeHomework').show().css({ 'margin-left': 'initial', 'font-size': '80%', 'text-align': 'center', 'line-height': '68px', 'overflow': 'hidden' });
                            $('#spnTimeHomework').html(needTimer.Deadline);
                             <%If BusinessTablet360.ClsKNSession.IsMaxONet Then%>
                            if (AnswerState == '2') {

                                $('#TimeNormal').show();
                                $('#secondN').text(parseInt(needTimer.AllTime % 60));
                                $("#minuteN").text(Math.floor(needTimer.AllTime / 60));

                            } else {
                                $('#TimeNormal').show();
                                minute = Math.floor(needTimer.AllTime / 60);
                                second = parseInt(needTimer.AllTime % 60);
                                timeNormal(minute, second);
                            }

                    <%End If%>
                        }
                        else {

                            if (AnswerState == '2') {
                                $('#TimeNormal').show();
                                $('#secondN').text(parseInt(needTimer.AllTime % 60));
                                $("#minuteN").text(Math.floor(needTimer.AllTime / 60));
                            } else {
                                $('#TimeNormal').show();
                                minute = Math.floor(needTimer.AllTime / 60);
                                second = parseInt(needTimer.AllTime % 60);
                                //minute = 0;
                                //second = 0;
                                timeNormal(minute, second);
                            }

                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('jeng');
                }
            });
        }
        function timeCountDown(min, seconds, type, noWatch) {
            var widthDiv = 0;
            var resizeDiv; var sumSeconds;
            var second_time = setInterval(function () {
                seconds = seconds - 1;
                // check second for change minute
                if (seconds == -1) {
                    seconds = 59;
                    //min = parseInt($("#minute").text());
                    min = min - 1;
                    if (min == -1) {
                        clearInterval(second_time);
                        min = 0;
                        seconds = 0;
                        //trigger Or end quiz
                        if (type) {
                            $('#btnNext').trigger('click');
                        }
                        else {
                            //show dialog end quiz
                            if (!noWatch) {
                                var isShowAnswerComplete = '<%=Session("ShowCorrectAfterComplete")%>';
                                var isShowScoreAfterComplete = '<%=Session("NeedShowScoreAfterComplete")%>';
                                console.log('isShowAnswerComplete = ' + isShowAnswerComplete);
                                if (isShowAnswerComplete == 'True') {
                                    CallDialogTimeOut("เฉลยเลย", "หมดเวลาทำควิซแล้วค่ะ มีเฉลยตอนท้ายนะคะ", "");
                                } else if (isShowScoreAfterComplete == 'True') {
                                    CallDialogTimeOut("ดูคะแนนเลย", "หมดเวลาทำควิซแล้วค่ะ มีคะแนนตอนท้ายนะคะ", "../Activity/ShowScore.aspx?DeviceUniqueID=" + DeviceId + "&QuizId=" + JSQuizIdForShowScore);
                                } else {
                                    // $('#dialogIsLastQuestion').dialog('option', 'title', '').dialog('open').dialog('option', 'width', 410);
                                    CallDialogTimeOut("ออกเลย", "หมดเวลาทำควิซแล้วค่ะ", "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" + DeviceId);
                                }
                            }
                        }
                    }
                }
                // check second for add "0" when length = 1                
                if (seconds < 10) {
                    seconds = "0" + seconds;
                    if (min == 0) {
                        //$('#firstBomb').jrumble({ rumbleEvent: 'constant' });
                    }
                }
                // resize div               
                if (widthDiv == 0) {
                    widthDiv = $("#secondBomb img").width();
                }
                sumSeconds = parseInt((min * 60) + parseInt(seconds, 10));
                var fullSeconds = $('#keepSumseconds').val();
                var lostSeconds = fullSeconds - sumSeconds;
                resizeDiv = (((1 / fullSeconds) * 100) * lostSeconds) * (widthDiv / 100);
                $("#secondBomb img").live().css("width", (widthDiv - resizeDiv) + "px");
                $('#second').text(seconds);
                $("#minute").text(min);
            }, 1000);
        }
        function timeNormal(minN, secondsN) {
            var second_timeN = setInterval(function () {
                secondsN++;
                // check second for change minute
                if (secondsN == 60) {
                    secondsN = 0;
                    minN = parseInt(minN) + 1;
                }
                // check second for add "0" when length = 1                
                if (secondsN < 10) {
                    secondsN = "0" + secondsN;
                }
                $('#secondN').text(secondsN);
                $("#minuteN").text(minN);
            }, 1000);
        }
    </script>
    <% End If%>
    <script type="text/javascript"> //เด็กกดตอบ Choice
        $(document).ready(function () {
            //$('.stuAns').each(function () {
            //    new FastButton(this, TriggerSaveChoice);
            //});
            $('#sortable').sortable({
                placeholder: "ui-state-highlight",
                stop: function (event, ui) {
                    getIdLi();
                }
            });
        });

        //Trigger SaveChoice
        function TriggerSaveChoice(e) {
            var obj = e.srcElement;
            console.log('obj = ' + obj);
            if ($('#hdStatusDict').val() == 'On') {
                console.log('คำศัพท์คำตอบ : ' + e.target.innerHTML);
                getSelectedOnTablet(e.target.innerHTML);
            }
            if (!$(obj).is('td')) {
                console.log('parent 1 ไม่ใช่ td : ' + $(obj).html());
                obj = $(obj).parent();
                if (!$(obj).is('td')) {
                    console.log('parent 2 ไม่ใช่ td : ' + $(obj).html());
                    obj = $(obj).parent('td');
                }
            }
            var questionid = $(obj).attr('questionid');
            var answerid = $(obj).attr('answerid'); console.log('answerid = ' + answerid);
            if (questionid === undefined || answerid === undefined) { console.log("click ที่รูปวงกลม"); $(obj).parent('td').css('background-color', '#FCFC05'); return 0; }
            var IsOne;
            if ($(obj).attr('IsOne') == 't') {
                IsOne = true;
            }
            else {
                IsOne = false;
            }
            if ($(obj).hasClass('ans')) {
                $(obj).css('background-color', '#FCFC05');
                return 0;
            }
            if ($(obj).prev().hasClass('ans')) {
                $(obj).css('background-color', '#FCFC05');
                return 0;
            }
            if ($(obj).next().hasClass('ans')) {
                $(obj).css('background-color', '#FCFC05');
                return 0;
            }
            SetAnswerChoiceQuestion(obj, questionid, answerid, IsOne);
        }

        // Save ChoiceQuestion
        function HilightClick2(ObjControl, QuestionId, AnswerId) {
            SetAnswerChoiceQuestion(ObjControl, QuestionId, AnswerId, false);
        }
        function HilightClick1(ObjControl, QuestionId, AnswerId) {
            SetAnswerChoiceQuestion(ObjControl, QuestionId, AnswerId, true);
        }
        function SetAnswerChoiceQuestion(ObjControl, QuestionId, AnswerId, IsNumOne) {
            KeepOldAnswer(ObjControl, IsNumOne);
            $(ObjControl).addClass('ans');
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/SetAnswerChoiceQuestion",
                data: "{ QuestionId: '" + QuestionId + "', AnswerId:'" + AnswerId + "', PlayerId:'" + PlayerId + "', NotReplyMode:'" + NotReplyMode + "', DeviceId:'" + DeviceId + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d == '1') {
                        //$('.ImgCircle').hide();
                        $('.ImgCircle').attr('style', 'display:none !important');
                        $('td').css('background-color', 'transparent');
                        if (IsNumOne) {
                            $(ObjControl).next().css('background-color', '#FCFC05');
                            $(ObjControl).children('img').attr('style', 'display:block !important');
                        } else {
                            $(ObjControl).prev().css('background-color', '#FCFC05');
                            $(ObjControl).prev().children('img').attr('style', 'display:block !important');
                        }
                        $(ObjControl).css('background-color', '#FCFC05');
                    }
                    else {
                        //alert('เซฟไม่ผ่าน คืน "" มา');
                        //console.log("เซฟไม่ผ่าน");
                        $('td').css('background-color', 'transparent');
                        if (td1 !== undefined && td2 !== undefined) {
                            $(td1).css('background-color', '#FCFC05');
                            $(td2).css('background-color', '#FCFC05');
                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }
        // Keep Old Answer
        var td1, td2;
        function KeepOldAnswer(ObjControl, IsTdFront) {
            $('#mainAnswer').children('table').children('tbody').children('tr').each(function () {
                $(this).children('td').each(function () {
                    if ($(this).hasClass('ans')) {
                        td1 = IsTdFront == true ? $(ObjControl) : $(ObjControl).prev();
                        td2 = IsTdFront == true ? $(ObjControl).next() : $(ObjControl);
                        $('td').css('background-color', 'transparent');
                        $(this).removeClass('ans')
                        return 0;
                    }
                });
            });
        }
        // Save SortQuestion
        function getIdLi() {
            var questionIdAll = "";
            $('#sortable').children().each(function () {
                questionIdAll += $(this).attr('id') + ",";
            });
            questionIdAll = questionIdAll.substring(0, questionIdAll.length - 1);
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/SetAnswerSortQuestion", //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/saveAnswerSortQuestion",
                data: "{  QuestionIdAll: '" + questionIdAll + "', PlayerId: '" + PlayerId + "', ExamNum: '" + Examnum + "', DeviceId:'" + DeviceId + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

    </script>
    <script type="text/javascript">       // ปุ่มสลับเฉลย - กับที่ตอบ
        var resetHtml = true;
        $(function () {
            var CorrectAnswer = ('<%= CorrectAnswer %>').toString();
            var MyAnswer = ('<%= MyAnswer%>').toString();
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
    <script type="text/javascript"> // เลือนคำตอบจับคู่
        $(function () {
            $('td.drop').droppable({
                drop: function (e, ui) {
                    var fromTd = ui.draggable.parent();
                    var fromSpan = ui.draggable.html()
                    var ThisTd = $(this);
                    var ThisSpan = $(this).find('span').html();
                    // id 
                    var fromQuestionId = ui.draggable.parent().attr('id');
                    var ThisQuestionId = $(this).attr('id');
                    var fromAnswerId = ui.draggable.attr('id');
                    var ThisAnswerId = $(this).find('span').attr('id');

                    $(fromTd).html('<span class="drag" id=' + ThisAnswerId + '>' + ThisSpan + '</span>');
                    $(ThisTd).html('<span class="drag" id=' + fromAnswerId + '>' + fromSpan + '</span>');
                    $('span.drag').draggable('destroy').draggable({ revert: 'invalid', helper: 'clone' });

                    if (fromQuestionId != ThisQuestionId) {
                        // POST
                        var QuestionIdAll = GetQuestionAll();
                        var AnswerIdAll = GetAnswerAll();
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/SetAnswerPairQuestion", // url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/SaveAnswerPairQuestion",
                            data: "{ QuestionIdAll : '" + QuestionIdAll + "', AnswerIdAll : '" + AnswerIdAll + "', PlayerId: '" + PlayerId + "', ExamNum: '" + Examnum + "', DeviceId:'" + DeviceId + "'}",
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (data) {
                                //alert(data.d);               
                            }, error: function myfunction(request, status) {
                                //alert("ต่อไม่ได้");                      
                            }
                        });
                    }
                }
            });
            $('span.drag').draggable({ revert: 'invalid', helper: 'clone' });

            // show btn tools แบบไปพร้อมกัน 
            $('#divToolsNotSelfpace').draggable();
            $('#divToolsNotSelfpace').toggle(function () {
                $(this).find('#divMenuToolsNotSelfpace').show();
            }, function () {
                $(this).find('#divMenuToolsNotSelfpace').hide();
            });
        });
        function GetQuestionAll() {
            var Question_All = "";
            $('td.drop').each(function () {
                Question_All += $(this).attr('id') + ",";
            });
            Question_All = Question_All.substring(0, Question_All.length - 1);
            return Question_All;
        }
        function GetAnswerAll() {
            var Answer_All = "";
            $('span.drag').each(function () {
                Answer_All += $(this).attr('id') + ",";
            });
            Answer_All = Answer_All.substring(0, Answer_All.length - 1);
            return Answer_All;
        }
    </script>
    <script type="text/javascript">        // เล่นไฟล์เสียงคำศัพท์
        var words = new Array(0, 0);
        function playSound(soundfile) {
            parent.muteBGMusic();
            // document.getElementById("voice").innerHTML = "<audio controls autoplay><source src=\"" + soundfile + "\" type='audio/mpeg'></audio>";
            //"<embed src=\"" + soundfile + "\" hidden=\"true\" autostart=\"true\" loop=\"false\" />";
            //setTimeout(function () {
            //    parent.unmuteBGMusic();
            //}, 2500);               
            var s = "";
            for (var i = 1; i < words.length; i++) {
                if (words[i][1] == soundfile) {
                    //var tempsound = words[i][0];
                    //tempsound.play();
                    s = words[i][0];
                    break;
                }
            }
            if (s == "") {
                s = new sound(soundfile);
                words.push(new Array(s, soundfile));
            }
            s.play();
        }

    </script>
    <link href="../css/styleQuestionAnswerExplain.css" rel="stylesheet" />
    <style type="text/css">
        /*#btnQuestionExp {
            top: -16px;
            right: 14px;
            cursor: pointer;
        }*/
    </style>
    <script type="text/javascript">
        // var NeedShowTip = '<%=NeedShowTip%>';
        //$(function () {
        //    if (NeedShowTip == 'True') {                
        //        ShowTips();
        //    }
        //});
        //$(function () {
        //    var pathTimeOut = "../practicemode_pad/choosetestset.aspx?DeviceUniqueID=" + DeviceId;
        //    console.log(pathTimeOut);
        //    $('#dialogIsLastQuestion').dialog({
        //        autoOpen: false,
        //        buttons: {
        //            'ออกเลย': function () {
        //                console.log(pathTimeOut);
        //                parent.location = pathTimeOut;
        //            }
        //            //},
        //            //'ยังก่อน': function () {
        //            //    $(this).dialog('close');
        //            //}
        //        },
        //         draggable: false,
        //         resizable: false,
        //         modal: true
        //     });
        //});

        function CallDialogTimeOut(btnConfirmName, txtForShowDialog, NextStep) {
            var myBtn = {};
            myBtn[btnConfirmName] = function () {
                if (NextStep !== "") {
                    parent.location.href = NextStep;
                } else {
                    NextToLeapChoice(1);
                }
            };

            $('#dialogIsLastQuestion').dialog({
                autoOpen: false,
                buttons: myBtn,
                draggable: false,
                resizable: false,
                modal: true
            }).dialog('option', 'title', txtForShowDialog).dialog('open');
        }

        function ShowTips() {
            var elm = ['#PanelMakeToDestination', '#PanelTools', '#BtnSwapQuestion', '#BtnNext', '#BtnPrev', '#BtnExitQuiz', '#Table1']; // mainAnswer
            var tipPosition = ['topMiddle', 'rightMiddle', 'rightTop', 'rightMiddle', 'rightMiddle', 'rightMiddle', 'topMiddle'];
            var tipTarget = ['bottomMiddle', 'leftMiddle', 'leftBottom', 'leftMiddle', 'leftMiddle', 'leftMiddle', 'topMiddle'];
            var tipContent = ['ทำถึงข้อไหนแล้ว จากทั้งหมดกี่ข้อ', 'เครื่องมือช่วย', 'ดูข้อที่ยังไม่ได้ทำ', 'ไปข้อถัดไป', 'ดูข้อที่แล้ว', 'จบฝึกฝน, ดูเฉลย', 'กดคำตอบที่ถูกที่สุด'];
            var tipAjust = [0, 0, -10, 0, 0, 0, 50];
            var w = [300, 150, 170, 130, 130, 170, 200];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font': 'bold 18px THSarabunNew'
                    }, hide: false,
                    position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { y: tipAjust[i] } },
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
        setTimeout(function () {

            $('#Answer0').qtip({
                content: "สามารถลากตัวเลือกด้านขวาสลับบรรทัดเพื่อตอบได้เลยค่ะ",
                show: { ready: true },
                style: {
                    width: 500, padding: 15, background: '#F68500', color: 'white', textAlign: 'center',
                    border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
                },
                position: { corner: { tooltip: 'rightTop', target: 'rightTop' }, adjust: { x: 10, y: -100 } },
                hide: false
            });
            setTimeout(function () { $('#Answer0').qtip('destroy'); }, 60000);
        }, 20000);

    </script>
    <style type="text/css">
        #panelTime {
            margin: 5px 3px !important;
        }
    </style>
    <% If BusinessTablet360.ClsKNSession.IsMaxONet Then %>
    <link href="../css/maxonetactivity.css" rel="stylesheet" />
    <%Else %>
    <link href="../css/ppactivity.css" rel="stylesheet" />
    <%End If %>
</head>
<body style="height: 100%; color: #444;">

    <form id="form1" runat="server" style='margin: -8px; height: 100%;'>
        <asp:HiddenField ID="AnsweredMode" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="ArrNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="OrderNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="LastQuestionNo" runat="server" ClientIDMode="Static" />

        <input type="hidden" id="hdQuestionId" runat="server" />
        <span id="voice" style="display: none;"></span>
        <div id='Hider' runat='server'></div>

        <div id='MainDivPad' class='MainDivNormal' runat='server' style='width: 100%; height: 100%; position: absolute;'>

            <div id="DivPlayground" style="position: relative; width: 89%; height: 100%;">

                <div id="DivTopMenu" style="width: 100%;">
                    <div id='DivReportExam' runat="server" class="divShowData" style="background-color: #fbefe6; width: 600px; margin-left: -235px;">

                        <div id='CloseDivReportExam' onclick="CloseDivReportExam();"></div>

                        <div id="divReportDetail" style="font-size: 20px; position: inherit;">
                            <table style="width: 98%; padding-left: 20px; padding-top: 10px; padding-bottom: 10px;">
                                <tr style="display: none;">
                                    <td><span><b>QuestionId</b></span></td>
                                    <td colspan="2" style="padding-left: 20px;"><%= questionId %></td>
                                </tr>
                                <tr>
                                    <td><span><b>ปัญหา</b></span></td>
                                    <td style="padding-left: 20px;">
                                        <select id="id" name="dropdown" style="width: 382px; height: 40px; border-radius: 5px; font: normal 18px THSarabunNew; margin-bottom: 5px;">

                                            <option value="0" selected="selected">เลือกหัวข้อปัญหาค่ะ...</option>
                                            <option value="1">คำถาม/คำตอบ ไม่ชัดเจน</option>
                                            <option value="2">คำอธิบายคำถาม/คำอธิบายคำตอบ ไม่ชัดเจน</option>
                                            <option value="3">เฉลยผิด</option>
                                            <option value="4">สับสน ไม่เข้าใจว่าใช้งานยังไง</option>
                                            <option value="5">ปัญหาอื่นๆ</option>
                                        </select>
                                    </td>
                                    <td style="color: red;">****</td>
                                </tr>
                                <tr>
                                    <td><span><b>รายละเอียด</b></span></td>
                                    <td id="tdReportDetail" runat="server" style="padding-left: 20px;">
                                        <textarea rows="4" cols="1" id="txtHidden" name="HiddenId" style="display: none;" runat="server"></textarea>
                                        <textarea rows="4" cols="1" id="txtDescript" name="descript" style="resize: none; width: 375px; height: 270px; font: normal 18px THSarabunNew;"></textarea>
                                    </td>
                                    <td style="color: red;">****</td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Button ID="btnSubmit" runat="server" Text="แจ้งปัญหา" class="btnsendreport" />
                                        <%--    <asp:Button ID="btnPrint" runat="server" text="พิมพ์" class="btnsendreport" />--%>
                                    </td>
                                </tr>
                            </table>
                        </div>


                    </div>
                    <%If JvIsSelfPace = True Then%>
                    <div>
                        <table>
                            <tr>
                                <td style="width: 5%;">
                                    <div id="panelLogo" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center;">
                                        <%If IsHomeWork Then %>
                                        <img src="../Images/Homework/PracticeFromTeacher.png" style="width: 70%; padding-top: 3px;" />
                                        <%Else %>
                                        <img src="../Images/Homework/Practice.png" style="width: 70%; padding-top: 3px;" />
                                        <%End If %>
                                    </div>
                                </td>
                                <td style="width: 10%;">
                                    <div id="panelTime" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center;">
                                        <div id="TimeCountDown" style="display: none;">
                                            <div id="firstBomb" style="width: 50px; margin-top: 15px; margin-left: -29px;">
                                                <img src="../Images/Activity/Timer/bomb01.png" />
                                                <div style="position: absolute; left: 60px; top: 0px;">
                                                    <span id="minute"></span><span>:</span><span id="second"></span>
                                                </div>
                                            </div>
                                            <div id="secondBomb" style="margin-top: 15px;">
                                                <img src="../Images/Activity/Timer/slate.png" style="width: 100px;" />
                                            </div>
                                            <div id="thirdBomb" style="margin-top: 15px">
                                                <img src="../Images/Activity/Timer/fragment.gif" style="width: 20px;" />
                                            </div>
                                        </div>
                                        <div id="TimeNormal" style="display: none; text-align: center;">
                                            <div>
                                                <span id="minuteN"></span><span>:</span><span id="secondN"></span>
                                            </div>
                                        </div>
                                        <div id="TimeHomework" style="display: none;">
                                            <span id="spnTimeHomework"></span>
                                        </div>
                                    </div>
                                </td>
                                <td style="width:auto;">
                                    <div id="PanelMakeToDestination" class="MenuBackground" style="height: 68px; border-radius: 5px; position: relative;">
                                        <img id='imgHere' runat="server" src="../images/Activity/NowChoice.PNG" style='position: absolute;' />

                                        <span id='ExamNumSpan' runat="server" style='position: absolute; font-size: 13px; margin-top: 10px;'></span>
                                        <div class="startPoint"></div>
                                        <div class="endPoint"></div>


                                        <div id='MainDivShowInfo' style='width: 85%; margin-left: 9%; height: 68px; position: relative; text-align: center;'>
                                            <div id='DivDone' runat="server" style='background-color: #f4502a; float: left; width: 100%; height: 26px; margin-top: 30px; text-align: center; border-bottom-left-radius: 3px; border-top-left-radius: 3px; position: absolute; z-index: 2;'>
                                            </div>
                                            <div id='DivResult' runat="server" style='background-color: #DFD2D2; float: left; width: 100%!important; height: 26px; font-size: 13px; margin-top: 30px; text-align: center; border-bottom-right-radius: 3px; border-top-right-radius: 3px; position: absolute;'>
                                            </div>
                                            <span id='spnTotalQuestion' runat="server" style='position: relative; font-size: 23px; top: 22px; z-index: 3;'></span>
                                        </div>
                                    </div>
                                </td>
                                <td style="width:5%;">
                                    <div id="panelShowExplain" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center;">
                                        <div id="btnAnswerExpNew">
                                            <% If _AnswerState = 2 Then%>
                                            <img src="../Images/Activity/SelectExplain/ShowExplain.png" style="width: 80%; padding-top: 10px; cursor: pointer;" />
                                            <%else %>
                                            <img src="../Images/Activity/SelectExplain/ShowExplain2.png" style="width: 80%; padding-top: 10px; cursor: not-allowed;" />
                                            <%End If%>
                                        </div>
                                    </div>
                                </td>
                                <td style="width:5%;">
                                    <div id="panelSendComment" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center;">
                                        <img src="../Images/Activity/SelectExplain/SendComment.png" style="width: 80%; padding-top: 8px; cursor:pointer;" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <%If Session("showScore") = True And Session("NeedShowScoreAfterComplete") = False Then%>
                        <div id='DivScored' style="background-color: #56C5FF; width: 70px; float: left; margin: 5px; margin-left: 2px; height: 48px; border-radius: 5px; padding: 10px;">
                            <div>
                                <asp:Label ID="lblScore" runat="server" Style="position: relative; top: -15px;">5</asp:Label>
                            </div>
                            <div style="border-top: 1px solid;">
                                <asp:Label ID="lblSumScore" runat="server" Style="position: relative; top: -5px;">10</asp:Label>
                            </div>
                        </div>
                        <%End If%>

                        <% If UseTools = True Then%>
                        <div id="PanelTools" style="margin-left: 2px; background-color: #56C5FF; padding: 0; width: 90px; height: 68px;">
                            <div style='background-color: #56C5FF; padding: 10px; width: 70px; height: 48px;'>
                                <div></div>
                            </div>
                        </div>

                        <div id="ToolsOnTablet" style="display: none;">

                            <% If tools_Calculator = True Then%>
                            <div class="ForToolsTablet btnCalculator" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>
                                <%-- <a><span>เครื่องคิดเลข</span></a>--%>
                     เครื่องคิดเลข
                            </div>
                            <% End If%>
                            <% If tools_Dictionary = True Then%>
                            <%-- <div class="ForToolsTablet btnDictionary DictOff" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>--%>
                            <div class="ForToolsTablet btnDictionary DictOff" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>
                                <%--<a><span>แปลศัพท์</span></a>--%>
                    แปลศัพท์
                            </div>
                            <% End If%>
                            <% If tools_WordBook = True Then%>
                            <div class="ForToolsTablet btnWordBook" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>
                                <%--<a><span>สมุดคำศัพท์</span></a>--%>
                    สมุดคำศัพท์
                            </div>
                            <% End If%>
                            <% If tools_Note = True Then%>
                            <div class="ForToolsTablet btnNote" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>
                                <%--<a><span>กระดาษโน๊ต</span></a>--%>
                    กระดาษโน๊ต
                            </div>
                            <% End If%>
                            <% If tools_Protractor = True Then%>
                            <div class="ForToolsTablet btnProtractor" style='margin-top: 10px; color: #FDA200; font-size: 25px; line-height: 70px;'>
                                <%--<a><span style="font-size: 25px;">ไม้โปรแทรกเตอร์</span></a>--%>
                    ไม้โปรแทรกเตอร์
                            </div>
                            <% End If%>
                        </div>
                        <% End If%>

                        <div style="clear: both;"></div>

                    </div>

                    <div id='slideDiv' class='ForDivLeapChoice' style='width: 780px; height: 440px; background-color: rgb(177, 220, 245); position: fixed; margin-left: 30px; margin-top: 5px; z-index: 999; top: 50%; left: 50%; margin-top: -220px; margin-left: -390px; display: none;'>
                        <div id='CloseDivSlide' onclick="CloseDivSlide();"></div>
                        <div id='LeapChoiceDiv' class='slides_container' style='height: 300px;'></div>

                        <img class='imgPreNext' id="NextSlide" src="../Images/Activity/AllNewArrow/rightBlue.png"
                            style='position: absolute; right: 1%; top: 35%; cursor: pointer; z-index: 99; display: none;' />

                        <img class='imgPreNext' id="BackSlide" src="../Images/Activity/AllNewArrow/leftBlue.png"
                            style='position: absolute; left: 1%; top: 35%; cursor: pointer; z-index: 99; display: none;' />

                        <div id='DivSortBtn' class='OrangeBack' style='margin-top: 14px; height: 79px; width: 710px;'>
                            <input type="button" class='ForSortBtn' id='BtnSortAnswerNull' value='ทวนข้อข้าม' />
                            <input type="button" id='BtnSortNormal' class='ForSortBtn' value='เรียงตามลำดับ'
                                style='margin-left: 15px;' />
                            <input type="button" class='ForSortBtn' id='BtnNextToLastChoice' value='ทำต่อข้อล่าสุด'
                                style='margin-left: 70px;' />
                        </div>
                    </div>

                    <div id='divSelectExplain' runat="server" class='ForDivLeapChoice'
                        style='display: none; background-color: #DAF7A6; position: absolute; font: normal 0px "THSarabunNew"; width: 925px; height: fit-content; z-index: 999; top: 50%; left: 50%; margin-top: -220px; margin-left: -390px; border-radius: 16px;'>

                        <div id='CloseDivSelectExplain' onclick="CloseDivSelectExplain();"></div>
                        <div id='divEachSelectExplain' class='slides_container' style='height: 300px;'></div>
                        <div id="divSummary">
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="4" style="text-align: center; font-size: 35px; font-weight: bold;"><span id='spnSummary'>ได้ 10/20 คะแนน</span></td>
                                </tr>
                                <tr>
                                    <td class="tdMode ActiveMode" id="btnAllQuestion">
                                        <table>
                                            <tr>
                                                <td>
                                                    <img src="../Images/Activity/SelectExplain/AllQuestion.png" style="width: 25px;"></td>
                                                <td><span id='spnSummary1' style="padding-left: 10px;">ทั้งหมด 10 ข้อ</span></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="tdMode" id="btnRightQuestion">
                                        <table>
                                            <tr>
                                                <td>
                                                    <img src="../Images/Activity/SelectExplain/RightAnswer.png" style="width: 25px;"></td>
                                                <td><span id='spnSummary2' style="padding-left: 10px;">ตอบถูก 10 ข้อ</span></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="tdMode" id="btnWrongQuestion">
                                        <table>
                                            <tr>
                                                <td>
                                                    <img src="../Images/Activity/SelectExplain/WrongAnswer.png" style="width: 25px;"></td>
                                                <td><span id='spnSummary3' style="padding-left: 10px;">ตอบผิด 3 ข้อ</span></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="tdMode" id="btnSkipQuestion">
                                        <table>
                                            <tr>
                                                <td>
                                                    <img src="../Images/Activity/SelectExplain/SkipAnswer.png" style="width: 25px;"></td>
                                                <td><span id='spnSummary4'>ข้ามไม่ตอบ 7 ข้อ</span></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <img class='imgPreNext' id="NextExplainPage" src="../Images/Activity/AllNewArrow/rightBlue.png"
                            style='position: absolute; right: 1%; top: 35%; cursor: pointer; z-index: 99; display: none;' />

                        <img class='imgPreNext' id="BackExplainPage" src="../Images/Activity/AllNewArrow/leftBlue.png"
                            style='position: absolute; left: 1%; top: 35%; cursor: pointer; z-index: 99; display: none;' />
                    </div>

                    <div id='DivNotHaveDontReplyChoice' runat="server" style='width: 760px; z-index: 9999; margin-left: 35px; height: 204px; position: absolute; margin-top: 28px; background-color: skyBlue; line-height: 200px; border-radius: 7px; display: none;'>
                        <div style="text-align: center; background-color: white; height: 180px; margin-top: 10px; width: 735px; margin-left: 10px; border: dashed 2px; border-radius: 5px;">
                            ไม่มีข้อข้ามแล้ว ทำข้อที่ยังไม่ทำต่อเลยค่ะ
                        </div>
                        <div id='CloseDivNothave' style="width: 50px; height: 50px; background: url('../Images/Activity/activitypagepad2.png') -250px -265px; float: right; margin-right: -30px; margin-top: -10px;"
                            onclick="CloseDivnot();">
                        </div>
                    </div>
                    <%Else%>
                    <% If UseTools = True Then%>
                    <div id="divToolsNotSelfpace" style="margin-left: 2px; background-color: #56C5FF; width: 70px; height: 50px; position: absolute; border-radius: 5px; z-index: 9999; top: 15px; left: 865px;">
                        <div style="background: url('../Images/Activity/activitypagepad2.png') -178px -228px; width: 100%; height: 100%; border-radius: 5px;"></div>
                        <div id="divMenuToolsNotSelfpace" style="display: none;">
                            <% If tools_Calculator = True Then%>
                            <div class="ForToolsTablet btnCalculator" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>เครื่องคิดเลข</div>
                            <% End If%>

                            <% If tools_Dictionary = True Then%>
                            <div class="ForToolsTablet btnDictionary DictOff" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>แปลศัพท์</div>
                            <% End If%>

                            <% If tools_WordBook = True Then%>
                            <div class="ForToolsTablet btnWordBook" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>สมุดคำศัพท์</div>
                            <% End If%>

                            <% If tools_Note = True Then%>
                            <div class="ForToolsTablet btnNote" style='margin-top: 10px; color: #FDA200; line-height: 70px;'>กระดาษโน๊ต</div>
                            <% End If%>

                            <% If tools_Protractor = True Then%>
                            <div class="ForToolsTablet btnProtractor" style='margin-top: 10px; color: #FDA200; font-size: 25px; line-height: 70px;'>ไม้โปรแทรกเตอร์</div>
                            <% End If%>
                        </div>
                    </div>
                    <% End If%>
                    <%End If%>
                </div>
                <div id="divQid" style="color: aliceblue; display: none;"><%=questionId%> </div>

                <div id="DivMainQuestionAndAnswer" style="position: absolute; height: 88%; width: 100%;">

                    <%  If EnableIntro = True Then%>
                    <div id="DivIntro">
                        <div id="ShowLimitAmount" runat="server"></div>
                        <div id="mainIntro" runat="server"><span id="spnTest">Clickkkkkkkkkkkkkkkkkkkk</span></div>
                        <%--<div id="mainIntroAudio">
                            <video controls preload="auto" width="100%" height="50" id="myvideojs" class="video-js vjs-default-skin"
                                data-setup='{"example_option":true}'><source src="../multimedia/audio/maid with the flaxen hair.mp3" type="audio/mp3">                       
                            </video>
                    </div>--%>
                    </div>
                    <%End If%>

                    <div id='DivMainQuestionAnswer' runat="server" class='MainQuestionAnswerNormal'>
                        <%  If EnableIntro = True Then%>
                        <div id="SPmainIntro" runat="server" style="position: relative; -webkit-border-radius: 10px 10px 0px 0px; background-color: #ffc76f; height: auto; font-weight: bold; width: 650px; border: 20px; padding: 20px; margin-left: -40px;">
                            <table id="Table4" runat="server" style="width: 650px; border-collapse: collapse;">
                                <tr>
                                    <td runat="server" id="SPIntroDetail"></td>
                                    <td runat="server" id="SPViewLimitAmount"></td>
                                </tr>
                            </table>
                        </div>
                        <%End If%>
                        <div id="mainQuestion" runat="server" class='QuestionNormal QuestionBackground'>
                            <%--<div runat="server" id="btnMoreDetail"></div>--%>
                            <%--                            <div id="DivMoreDetailMenu" style="width: 100px; height: 200px; z-index: 1; position: absolute; right: 12px; border-radius: 5px; top: 17px; display: none!important;">
                                <div id="btnQuestionExp"></div>
                                <div id="btnSendComment"></div>
                            </div>--%>
                            <table id="QuestionTbl" runat="server" style="border-collapse: collapse; width: 100%;">
                                <tr>
                                    <td style="padding-top: 10px;" runat="server" id="QuestionTd"></td>
                                </tr>
                            </table>
                        </div>
                        <div id="mainAnswer" runat="server" style="width: 98%; position: relative; background-color: #F4F7FF; padding: 20px; border-radius: 0 0 10px 10px; min-height: 65%;">
                            <% If IsNoAnswer = True Then%>
                            <img id="ImgSkip" src="../Images/Activity/skip.png" class="ImgSkip" />
                            <%End If%>

                            <div id="btnAnswerExp"></div>
                            <%If SwapStatus = True Then%>
                            <div id="SwapAnswer" style="line-height: 2; background-color: transparent; width: 130px; float: right;">
                                <img src="../Images/Activity/swap.png" alt="" />
                            </div>
                            <%End If%>

                            <table id="Table1" runat="server" style="border-collapse: collapse; width: 100%;">
                                <tr>
                                    <td runat="server" id="AnswerTbl"></td>
                                </tr>
                            </table>

                            <div id="AnswerExp" runat="server" clientidmode="Static" style="font-size: 24px; display: none;"></div>
                        </div>
                    </div>
                    <div style="clear: both; line-height: 0; height: 0;"></div>
                </div>

            </div>

            <div id="DivRightMenu" style="right: 0; margin-top: 5px; position: absolute; height: 100%; top: 0; width: 9%; margin-right: 18px;">

                <div id="BtnSwapQuestion" class="RightDiv MenuBackground" style="height: 18%; cursor: pointer; margin-right: 10px; background: url(../Images/Activity/writing.png) center center no-repeat;">
                    <span id='spnLeap1' style='font-size: 20px; position: absolute; margin-left: 30px; font-weight: bold; margin-top: 12px;'>ข้าม</span>
                    <span id='spnStatusLeapChoice' style='font-size: 40px; position: absolute; width: 90px; text-align: center; margin-top: 20px; font-weight: bold;'></span>
                </div>

                <div id="BtnNext" class="RightDiv MenuBackground" style="height: 30%; background: url(../Images/Activity/bnext.png) center center no-repeat; background-size: 70%; margin-right: 10px;">
                    <asp:Button ID="btnNext" runat="server" Height="100%" Width="100%" ClientIDMode="Static" Style="background: transparent; border: none; cursor: pointer;" />
                </div>

                <% If ViewState("_IsPerQuestion") = False Then%>
                <div id="BtnPrev" class="RightDiv MenuBackground" style="height: 30%; background: url(../Images/Activity/bback.png) center center no-repeat; background-size: 70%; margin-right: 10px;">

                    <asp:Button ID="btnPrev" runat="server" Height="100%" Width="100%" Style='background: transparent; border: none; cursor: pointer;' />

                </div>
                <%End If%>

                <%If IsShowBtnCompleteHomework = True Then%>
                <div id="BtnCompleteHomework" class="RightDiv" style="height: 60px; text-align: center; background: url('../Images/Activity/activitypagepad2.png') -90px -255px;"></div>
                <%End If%>

                <div id="BtnExitQuiz" class="RightDiv MenuBackground" style="margin-right: 7px; cursor: pointer; height: 16%; background-size: 70%; margin-right: 10px;">
                    <%--<img src="../Images/activity/logout.png" style="margin-top: <%=SizeTopForImgExit%>px; margin-left: 9px;" />--%>
                </div>
            </div>

            <% If JvIsSelfPace = True Then%>
            <div id="prettyHM" style="background-color: Black; width: 100%; height: 100%; position: fixed; top: 0; left: 0; z-index: 99; opacity: 0.7;" class="prettyIntro"></div>

            <div id="mainPretty" class="prettyIntro">
                <div class="clear"></div>
                <div id="divClose">X ปิด</div>
                <div id="introHtml" style="" runat="server"></div>
                <div class="clear"></div>
            </div>

            <div id="LinkDialog" title=""></div>

            <div id="CompleteHomeworkDialog" title=""></div>

            <div id="UnCompleteDialog" title=""></div>
            <% End If%>

            <div id="dialog" title="ต้องเข้าควิซแล้วค่ะ"></div>

            <div id="DivBlock" style="z-index: 9999999999;">
                <div id="DivVs" runat="server"></div>
                <div id="DivStartQuiz">
                    <img id="btnStartQuiz" src="../Images/Start.gif" style="position: relative; top: 150px;">
                </div>
            </div>

            <div id="dialogIsLastQuestion" class="dialogIsLastQuestion" title=""></div>
        </div>

        <input type="hidden" id='HDIsScore' runat="server" />
        <input type="hidden" id='HDStatusHomework' runat="server" />
        <input type="hidden" id='HDQQ_No' runat="server" />
        <input type="hidden" id='HDIsLeapChoice' runat="server" value='False' />
        <%--<input type="hidden" id="hdKeepMin" runat="server" />
        <input type="hidden" id="hdKeepSec" runat="server" />
        <input type="hidden" id="hdKeepMinN" runat="server" />
        <input type="hidden" id="hdKeepSecN" runat="server" />--%>
        <input type="hidden" id="hdNum" runat="server" value="1" />
        <input type="hidden" id="keepSumseconds" runat="server" />
        <input type="hidden" id='CheckIsHaveLeapChoice' runat="server" />
        <input type="hidden" id='QuantityLeapChoice' runat="server" />
        <input type="hidden" id="hdStatusDict" runat="server" value="Off" />
        <input type="hidden" id='HDNotReplyMode' runat="server" value="false" />
        <input type="hidden" id='HDIsSort' runat="server" value="false" />
        <input type="hidden" id='HDLastChoice' runat="server" value="false" />

        <audio id="audioIntro" style="display: none;" autobuffer controls autoplay>
            <%--<source src="/images/coin.mp3" />--%>
        </audio>
        <% If tools_Calculator = True Then%>
        <div id="tools_calculator" style="z-index: 99;">
            <div id="CalculatorPanel">
                <div id="txtCalculator">
                    <span id="SumNumber" style="font-size: 35px;">0</span>
                </div>
                <div id="btnCalculator">
                    <div class="btnOperation btnFirstCol btnCalculatorDiv" onclick='calculator("/")'>
                        /
                    </div>
                    <div class="btnOperation btnSecondtCol btnCalculatorDiv" onclick='calculator("*")'>
                        *
                    </div>
                    <div class="btnOperation btnThirdCol btnCalculatorDiv" onclick='calculator("-")'>
                        -
                    </div>
                    <div class="btnClear btnForthCol btnCalculatorDiv" onclick='ClearValue()'>
                        C
                    </div>
                    <%--<input type="button" onclick='calculator("7")' value="7" class="btnNumeric btnFirstCol btnSecondRow btnCalculatorDiv" style="width: 72px; height: 47px; position: absolute;" />--%>
                    <div class="btnNumeric btnFirstCol btnSecondRow btnCalculatorDiv" onclick='calculator("7")'>
                        7
                    </div>
                    <div class="btnNumeric btnSecondtCol btnSecondRow btnCalculatorDiv" onclick='calculator("8")'>
                        8
                    </div>
                    <div class="btnNumeric btnThirdCol btnSecondRow btnCalculatorDiv" onclick='calculator("9")'>
                        9
                    </div>
                    <div class="btnOperation btnForthCol btnSecondRow btnCalculatorDiv" style="height: 97px;"
                        onclick='calculator("+")'>
                        +
                    </div>
                    <div class="btnNumeric btnFirstCol btnThirdRow btnCalculatorDiv" onclick='calculator("4")'>
                        4
                    </div>
                    <div class="btnNumeric btnSecondtCol btnThirdRow btnCalculatorDiv" onclick='calculator("5")'>
                        5
                    </div>
                    <div class="btnNumeric btnThirdCol btnThirdRow btnCalculatorDiv" onclick='calculator("6")'>
                        6
                    </div>
                    <div class="btnSumary btnForthCol btnFifthRow btnCalculatorDiv" style="height: 97px;"
                        onclick='calculator("=")'>
                        =
                    </div>
                    <div class="btnNumeric btnFirstCol btnForthRow btnCalculatorDiv" onclick='calculator("1")'>
                        1
                    </div>
                    <div class="btnNumeric btnSecondtCol btnForthRow btnCalculatorDiv" onclick='calculator("2")'>
                        2
                    </div>
                    <div class="btnNumeric btnThirdCol btnForthRow btnCalculatorDiv" onclick='calculator("3")'>
                        3
                    </div>
                    <div class="btnNumeric btnFirstCol btnFifthRow btnCalculatorDiv" style="width: 148px;"
                        onclick='calculator("0")'>
                        0
                    </div>
                    <div class="btnNormal btnThirdCol btnFifthRow btnCalculatorDiv" onclick='calculator(".")'>
                        .
                    </div>
                </div>
            </div>
            <a class="Aclosed">
                <div>
                </div>
            </a>
        </div>
        <%End If%>
    </form>

    <script type="text/javascript">
        // Script สำหรับโหมด Selfpace
        var isMaxOnet = '<%=BusinessTablet360.ClsKNSession.IsMaxONet%>';

        $(function () {
            NotReplyMode = '<%=NotReplyMode %>';

            if (JvIsSelfPace == "True") {


                var AnswerStatemode = '<%=_AnswerState %>';

                if (AnswerStatemode == 2) {
                    $('#BtnSwapQuestion').css('background', 'url("../images/Activity/checklist.png")');
                } else {
                    if ($('#CheckIsHaveLeapChoice').val() == 1) {
                        $('#BtnSwapQuestion').css('background', 'url("../images/Activity/Explain.gif")');
                        $('#BtnSwapQuestion').addClass('leapQuestion');
                        //$('#spnStatusLeapChoice').html($('#QuantityLeapChoice').val());
                    } else {
                        $('#BtnSwapQuestion').css('background', 'url("../Images/Activity/writing.png")');
                        $('#BtnSwapQuestion').addClass('noleapQuestion');
                    }
                }

                $('#spnLeap1').hide();
                $('#spnLeap2').hide();
                $('#spnStatusLeapChoice').hide();
                $('#BtnSwapQuestion').css('background-repeat', 'no-repeat');
                $('#BtnSwapQuestion').css('background-position', 'center');

                var IsHomework = '<%=IsShowBtnCompleteHomework%>'; // IsHomework = show btn send homework?                 
                if (IsHomework == 'True' && isMaxOnet == "True") {
                    //$('#BtnNext').css('background', 'url("../Images/Activity/activitypagepad2.png") -270px 0px');
                    //$('#BtnPrev').css('background', 'url("../Images/Activity/activitypagepad2.png") -180px 0px');
                    //$('#BtnExitQuiz').css('background', 'url("../Images/Activity/activitypagepad2.png") -90px -155px');
                    // $('#BtnExitQuiz').css("background-size", "70px"); //background-position-y: 150px;
                    $('#BtnNext').css('background-position-y', '305px');
                    $('#BtnPrev').css('background-position-y', '305px');
                    $('#BtnExitQuiz').css("background-position-y", "150px");
                } else {
                    var CheckIsNeedTimerPerQuestion = '<%=VBCheckIsNeedTimerPerQuestion %>';
                    if (CheckIsNeedTimerPerQuestion == "True") {
                        //$('#BtnNext').css({ 'background': 'url("../Images/Activity/next-icon.png") 0px 100px', 'background-color': '#56C5FF', 'background-repeat': 'no-repeat' });
                    } else {
                        // $('#BtnNext').css('background', 'url("../Images/Activity/activitypagepad2.png") -90px 0px');
                    }
                    //$('#BtnPrev').css('background', 'url("../Images/Activity/activitypagepad2.png") 0px 0px');
                    //$('#BtnExitQuiz').css('background', 'url("../Images/Activity/activitypagepad2.png") 0px -155px');
                }
            }
            //update 16-06-56 tools on tablet
            var QuestionId = '<%= questionId %>';
            $('#hdQuestionId').val(QuestionId);
            $('#hdUserId').val(PlayerId);

            //btn Open QuestionExpain
            var AnswerExpType;
            if ($('#AnswerExp').children('div').length > 0) { AnswerExpType = 1; }
            if ($('ul > li').children('div.Correct').length > 0 || $('ul > li').children('div.InCorrect').length > 0) { AnswerExpType = 6; }

            if ($('#QuestionExp').length > 0 || $('#AnswerExp').children('div').length > 0) { $('#btnQuestionExp').show(); }


            //$('#btnQuestionExp').toggle(function () {
            //    chooseSound.play();
            //    $('#QuestionExp').show();
            //    if (AnswerExpType == 1) { $('#Table1').hide(); $('#AnswerExp').show(); }
            //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
            //}, function () {
            //    chooseSound.play();
            //    $('#QuestionExp').hide();
            //    if (AnswerExpType == 1) { $('#Table1').show(); $('#AnswerExp').hide(); }
            //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
            //});

            //btn Open AnswerExpain   

            $('#btnAnswerExpNew').toggle(function () {
                $('#QuestionExp').show();
                if (AnswerExpType == 1) { $('#Table1').hide(); $('#AnswerExp').show(); }
                else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
            }, function () {
                $('#QuestionExp').hide();
                if (AnswerExpType == 1) { $('#Table1').show(); $('#AnswerExp').hide(); }
                else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
            });

        });

        function ShowOrHideAnswerExpTypeSix() {
            if (resetHtml) { $('ul > li').children('div.Correct').show(); $('ul > li').children('div.InCorrect').show(); }
            else { $('ul > li').children('div.Correct').hide(); $('ul > li').children('div.InCorrect').hide(); }
            resetHtml = !resetHtml;
        }
    </script>

    <%--  <input type="hidden" id="hdQuestionId" runat="server" />--%>
    <input type="hidden" id="hdUserId" runat="server" />
    <input type="hidden" id="hdIsGroupEng" runat="server" />

    <% If tools_Note = True Then%>
    <div id="tools_note" style="display: none;">
        <div id="noteMain">
            <div class="noteTabs">
                <div class='noteHeadTab'>
                    <input type='radio' id='tab-1' name='tab-group' checked='checked' />
                    <label for='tab-1'>
                        กระดาษทด</label><div class='content' id='myClipboard'>
                            <textarea></textarea>
                        </div>
                </div>
                <div class='noteHeadTab'>
                    <input type='radio' id='tab-2' name='tab-group' /><label for='tab-2'>
                        สมุดโน๊ต</label><div class='content' id='myNote'>
                        </div>
                </div>
            </div>
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_WordBook = True Then%>
    <div id="tools_wordbook">
        <div id="wordbookMain">
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_Protractor = True Then%>
    <div id="tools_protractor">
        <div id="divImg">
            <img id="imgPro" src="../Images/Activity/Setting_Tools/pro.png" width="500" height="279.5"
                alt="" />
        </div>
        <div id="btnRotateL" class="btnRotate">
        </div>
        <div id="btnRotateR" class="btnRotate">
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_Dictionary = True Then%>
    <div id="tools_dictionary">
        <div class="headDict">
        </div>
        <div class="contentDict">
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <%If ShowDivScoreChoiceToChoice = True Then%>
    <ul id="ShowScorePanel">
        <li id="ShowScore" class="ShowScore"><a style="margin-left: -52px;"><font size="6.5">
            <br />
            <%= ScoreChoiceToChoice%></font>
            <br />
            <br />
            คะแนน</a></li>
    </ul>
    <%End If%>

    <div id="hasScrollbar" style="display: none; width: 75px; height: 75px; position: absolute; margin-left: 700px; top: 470px; background-image: url('../images/btnScrollDown50.png');"></div>

    <script type="text/javascript">
        (function () {
            var dictionaryStatus = false;
            var obj = {
                init: function () {
                    var a = document.getElementsByClassName("stuAns");
                    for (var i = 0; i < a.length; i++) {
                        on(a[i], "click", this, false);
                        on(a[i], 'touchstart', this, false);
                    }
                    //var b = document.getElementsByClassName("btnDictionary");
                    //for (var i = 0; i < b.length; i++) {
                    //    on(b[i], 'click', this, false);
                    //}
                },
                handleEvent: function (e) {
                    var evt = e || window.event,
                        t = evt.target || evt.srcElement;
                    console.log(t);
                    console.log('t.className = ' + t.className);
                    if (t.className === '') {
                        t = t.parentElement;
                        console.log('t.parent.className  = ' + t.className);
                    }
                    console.log(t.className);
                    switch (evt.type) {
                        case "click":
                            if (t.className === "stuAns") {
                                this.button(e, t);
                            } else if (t.className === "btnDictionary DictOff") {
                                this.changeHandleEvent(evt);
                                turnOnDict();
                                $(t).removeClass('DictOff').addClass('DictOn');
                            }
                            else if (t.className === "btnDictionary DictOn") {
                                this.revertHandleEvent();
                                turnOffDict();
                                $(t).removeClass('DictOn').addClass('DictOff');
                            }
                            break;
                        case "touchstart":
                            if (t.className === "stuAns") {
                                this.buttonTouch(e, t);
                            } break;
                        case "touchend":
                            if (t.className === "stuAns") {
                                this.button(e);
                            } break;
                    }
                },
                dude: "hello",
                button: function (e, t) {
                    //TriggerSaveChoice(e);
                    //new FastButton(e, TriggerSaveChoice);
                    answered.play();
                    e.stopPropagation();
                    this.handler = TriggerSaveChoice;
                    this.handler(e);
                    if (e.type == 'touchend') {
                        preventGhostClick(this.startX, this.startY);
                    }
                },
                buttonTouch: function (e, t) {
                    answered.play();
                    e.stopPropagation();
                    t.addEventListener('touchend', this, false);
                    document.body.addEventListener('touchmove', this, false);
                    this.handler = TriggerSaveChoice;
                    this.handler(e);
                    this.startX = e.touches[0].clientX;
                    this.startY = e.touches[0].clientY;
                    t.style.background = "rgba(0,0,0,.7)";
                },
                changeHandleEvent: function (evt) {
                    this._handleEvent = this.handleEvent;
                    this.handleEvent = function (e) {
                        var evt = e || window.eval,
                            t = evt.target || evt.srcElement;
                        console.log(t.className);
                        if (t.className === "stuAns") {
                        }
                        else if (t.className === "btnDictionary DictOn") {
                            this.revertHandleEvent();
                            turnOffDict();
                            $(t).removeClass('DictOn').addClass('DictOff');
                        }
                    }
                },
                revertHandleEvent: function () {
                    if (!this._handleEvent) return;
                    this.handleEvent = this._handleEvent;
                    delete this._handleEvent;
                },
                other: function (e) {
                    alert(e.type);
                }
            };

            function preventGhostClick(x, y) {
                coordinates.push(x, y);
                window.setTimeout(gpop, 2500);
            };
            function gpop() {
                coordinates.splice(0, 2);
            };

            function on(el, evt, fn, bubble) {
                console.log(("addEventListener" in el));
                if ("addEventListener" in el) {
                    try {
                        el.addEventListener(evt, fn, bubble);
                    } catch (e) {
                        console(e);
                        if (typeof fn == "object" && fn.handleEvent) {
                            el.addEventListener(evt, function (e) {
                                fn.handleEvent.call(fn, e);
                            }, bubble);
                        } else {
                            throw e;
                        }
                    }
                } else if ("attachEvent" in el) {
                    if (typeof fn == "object" && fn.handleEvent) {
                        el.attachEvent("on" + evt, function () {
                            fn.handleEvent.call(fn);
                        });
                    } else {
                        el.attachEvent("on" + evt, fn);
                    }
                }
            }
            obj.init();
        })();
    </script>

</body>

<%--<% If F5 = "1" Then%>
<script type="text/javascript">
    $(function () {
        GetTime(0);

    });
    function GetTime(a) {
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad2.aspx/GetCurrentTime",
            //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/CreateStringLeapChoice",          
            data: "{ IsStart:'" + a + "'}",
            async: false,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                // alert(data.d);
            },
            error: function myfunction(request, status) {
                // alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
            }
        });
    }
</script>
<% End If%>--%>
</html>
<%--<script src="../js/jquery.nicescroll.min.js"></script>--%>
<script type="text/javascript">
    $(function () {
        //alert($("#DivMainQuestionAnswer").GetScrollHeight());
        //if ($("#DivMainQuestionAnswer").hasVerticalScrollBar()) {
        //    $("#hasScrollbar").show();
        //    //$("#DivMainQuestionAnswer").niceScroll();
        //}
        var imgUp = "url(../images/btnScrollUp50.png)";
        var imgDown = "url(../images/btnScrollDown50.png)";
        var stateBtnDown = true;
        $("#hasScrollbar").click(function () {
            var h = 0;
            if ($("#DivMainQuestionAnswer").scrollTop() == 0 || stateBtnDown) {
                h = $("#DivMainQuestionAnswer").GetScrollHeight();
                $(this).css('top', '85px').css("background-image", imgUp);
                stateBtnDown = false;
            } else {
                $(this).css('top', '470px').css("background-image", imgDown);
                stateBtnDown = true;
            }
            $("#DivMainQuestionAnswer").scrollTop(h);
        });
        $("#DivMainQuestionAnswer").scroll(function () {
            if ($(this).scrollTop() == 0) {
                $("#hasScrollbar").css('top', '470px').css("background-image", imgDown);
                stateBtnDown = true;
            }
            else if (($(this).scrollTop() + $(this).height()) >= $(this).GetScrollHeight()) {
                $("#hasScrollbar").css('top', '85px').css("background-image", imgUp);
                stateBtnDown = false;
            }
        });
    });
</script>
<script type="text/javascript">
    var isRefreshPage = false;
    var refreshUrl;

    function OpenBlockUI() {
        blockUISpinner();
        countTimeToRefresh();
    }

    function CloseBlockUI() {
        $.unblockUI();
    }

    function blockUISpinner() {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: 'initial',
                color: '#fff'
            },
            message: '<img src="<%=ResolveUrl("~")%>Images/waitspinner.gif" width="100" height="100" /><br /><span style="font: normal 35px THSarabunNew;">รอสักครู่นะคะ</span>'
        });
    }

    function countTimeToRefresh() {
        setTimeout(function () {
            $.unblockUI();
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: 'initial',
                    color: '#fff'
                },
                message: '<input type="button" value="โหลดใหม่" onclick="refreshPage();" style="width: 150px;height: 50px;font: normal 25px THSarabunNew;" />'
            });
        }, 30000);
    }
    function refreshPage() {
        isRefreshPage = !isRefreshPage;
        console.log("When refreshPage = " + isRefreshPage);
        if (parent.refreshUrl == "N") {
            TriggerbtnNextCI();
        } else if (parent.refreshUrl == "P") {
            TriggerbtnPrevCI();
        }
        $.unblockUI();
        blockUISpinner();
        parent.OpenBlockUI();
    }
</script>
<script type="text/javascript">
    var chooseSound = new sound("<%=ResolveUrl("~")%>sounds/maxonet/select.mp3");
    var backSound = new sound("<%=ResolveUrl("~")%>sounds/maxonet/back.mp3");
    var nextSound = new sound("<%=ResolveUrl("~")%>sounds/maxonet/nextstep.mp3");
    var previousQuestion = new sound("<%=ResolveUrl("~")%>sounds/maxonet/previous.mp3");
    var nextQuestion = new sound("<%=ResolveUrl("~")%>sounds/maxonet/next.mp3");
    var answered = new sound("<%=ResolveUrl("~")%>sounds/maxonet/answer.mp3");

    function sound(src) {
        this.sound = document.createElement("audio");
        this.sound.src = src;
        this.sound.setAttribute("preload", "auto");
        //this.sound.setAttribute("controls", "none");

        document.body.appendChild(this.sound);
        this.play = function () {
            this.sound.pause();
            this.sound.currentTime = 0;
            this.sound.play();
        }
    }
</script>
