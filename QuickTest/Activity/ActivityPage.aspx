<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityPage.aspx.vb"
    Inherits="QuickTest.ActivityPage" %>

<%@ Import Namespace="BusinessTablet360" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <meta id="Viewport" name="viewport" width="initial-scale=1, maximum-scale=1, minimum-scale=1, user-scalable=no">
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <%--<script src="../js/jquery.qtip.js" type="text/javascript"></script>--%>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <link href="../css/jquery.qtip.css" rel="stylesheet" type="text/css" />
    <%--<script src="../js/facescroll.js" type="text/javascript"></script>
    <script src="../js/slimScroll.js" type="text/javascript"></script>--%>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
       <script src="../js/slides.min.jquery.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleEnabledTools.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script src="../js/jsEnabledTools.js" type="text/javascript"></script>
    <script src="../js/jQueryRotateCompressed.2.2.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/fixMenuSlide.css" rel="stylesheet" />
      <script src="../js/swipe.js" type="text/javascript"></script>
    <link href="../css/styleQuestionAnswerExplain.css" rel="stylesheet" />
    <style type="text/css">
        #AnswerExp > div > div {
            font-size: 24px !important;
        }

        div#QuestionExp {
            width: 625px !important;
            font-size: 24px !important;
        }

        #btnQuestionExp {
            right: -20px !important;
            top: 12px !important;
            cursor: pointer;
        }
    </style>
        <% If IsAndroid = True Then%>
    <style type="text/css">
        #divHead {
            width: 830px !important;
            position: relative !important;
            left: 50px !important;
        }

        #DivImgTopMenu {
            position: absolute !important;
            top: 18px !important;
            z-index: 999999 !important;
            left: 775px !important;
            top: 110px !important;
        }

        #imgMenuTopSide {
            display: none !important;
        }

        #DivViewReport1, #DivExit1, .ForToolsTop {
            margin-left: -23px !important;
            z-index: 999999 !important;
        }

        body {
            height: 100% !important;
        }

        #divQuestionAndAnswer, .divQuestionAndAnswer {
            margin-left: 175px !important;
        }

        .imgShowStudentStatusPage {
            left: 5% !important;
        }
        /*.ForDivLeapChoice
        {
            list-style: none;
            z-index: 999;
            height: 100%;
            border-radius: 0.2em;
            border: 1px outset;
        }*/


    </style>
    <% End If%>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        .positionImg {
            position: relative;
        }

        .BtnNextPre {
            top: 100px;
        }

        span {
            font-size: larger;
        }

        .ForToolsLeft {
            position: relative;
            z-index: 999;
        }

        .Question {
            font-size: 30px;
            position: relative;
            -webkit-border-radius: 10px 10px 0px 0px;
            background-color: #ffc76f;
            height: auto;
            font-weight: bold;
            width: 650px;
            border: 20px;
            padding: 20px;
            margin-left: 44px;
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
            margin-left: 44px;
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

        .lblQuizName {
            cursor: pointer;
        }

        ul {
            counter-reset: li; /*list-style-type: decimal;*/
            margin-top: 0px;
            list-style: none; /*list-style-image:url('/img/smile_1.png');*/
        }

        #sortable > li:before {
            /*content:"ลำดับที่ ";*/
            content: "ลำดับที่ " counter(li) "          "; /*list-style-type:decimal;*/
            counter-increment: li;
            color: Orange;
        }

        li {
            /*height: 40px;*/
            background-color: white;
            margin: 0 5px 5px 5px;
            padding: 5px;
            width: 613px;
            text-align: left;
            padding-left: 20px;
            line-height: 40px;
            -webkit-border-radius: 5px;
            cursor: pointer;
            border: 1px solid gray;
        }

        #normal > li:before {
            content: "ลำดับที่ " counter(li) "          "; /*list-style-type:decimal;*/
            counter-increment: li;
            color: Orange;
        }

        .CorrectLi {
            color: Orange;
        }

        #ViewLimitAmount {
            background-color: rgb(255, 29, 29);
            position: absolute;
            width: 25px;
            height: 25px;
            right: 0;
            top: 0;
            z-index: 1;
            -webkit-border-radius: 1em;
            border: 2px white solid;
            text-align: center;
            color: white;
            font-weight: bolder;
            font-size: 20px;
        }

        #DivIntro {
            position: relative; /*background-color: #ffffff; */
            height: 70px;
            width: 690px;
            border: 20px;
            padding: 10px;
            margin-left: -50px;
            margin-right: auto; /*margin-bottom: 20px;*/
        }

        #mainIntro {
            position: relative;
            -webkit-border-radius: 10px 10px 10px 10px;
            background-color: #b39eb5;
            height: 50px;
            font-weight: bold;
            width: 670px;
            padding: 10px;
        }

        .prettyIntro {
            display: none;
        }

        #mainIntroAudio {
            position: relative;
            -webkit-border-radius: 10px 10px 10px 10px;
            height: auto;
            font-weight: bold;
            width: 690px;
            padding-top: 10px;
        }

        #spnTest, #spnClose {
            cursor: pointer;
        }

        #divClose {
            color: #fff;
            z-index: 555;
            position: absolute;
            right: 12%;
            top: 5%;
            font-size: medium;
            cursor: pointer;
        }

        #mainPretty {
            height: 90%;
            width: 70%;
            font-weight: bold;
            margin-left: -35%;
            margin-top: -20%;
            left: 50%;
            top: 50%;
            z-index: 100;
        }

        #introHtml {
            position: relative;
            -webkit-border-radius: 10px 10px 10px 10px;
            background-color: #fff;
            font-weight: bold;
            width: 100%;
            padding: 70px;
            margin-top: -42%;
            left: 65%;
            z-index: 100;
            border: 10px solid #b39eb5;
            overflow-y: scroll;
            height: 500px;

        }

        .ImgSkip {
            position: absolute;
            right: 235px;
            top: -45%;
        }

        .clear {
            clear: both;
            line-height: 0;
            height: 0;
            font-size: 1px;
        }

        .ForSortBtn {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            background: #1EC9F4;
            border-radius: .5em;
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            text-shadow: 1px 1px #178497;
        }

        .NotReplyEmpty {
            margin-left: 36px;
            height: 260px;
            margin-top: 30px;
            text-align: center;
            vertical-align: middle;
            display: block;
            background-color: white;
            border: 1px dashed black;
            color: black;
            line-height: 8em;
            font-weight: bold;
            width: 90%;
            border-radius: 10px;
        }

        div.PauseTime {
            width: 100%;
            height: 100%;
            background-color: red;
            position: absolute;
            top: 0;
            left: 0;
            opacity: 0.4;
            display: none;
            z-index: 9;
            background-size: cover;
            background-repeat: no-repeat;
            background-position: center center;
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
        }

        .ForBtn {
            width: 65px;
            height: 65px;
            margin-left: 17px;
            margin-top: 9px;
        }

        .ImgCircle {
            position: absolute;
            top: 3px;
            left: -18px;
            display: none;
        }

        .DivSmallInTopScore {
            background-color: rgb(28, 195, 218) !important;
            color: white;
        }

        #mainQuestion p:first-child, #mainAnswer td p:first-child {
            display: inline;
        }

        .ui-tooltip, .qtip {
            max-width: 400px;
        }
    </style>
    <link href="../css/LogoutStyle.css" rel="stylesheet" />
     <script type="text/javascript">         //เด็กกดตอบ Choice
         var PlayerId = '<%=UserId%>';

         $(document).ready(function () {

             var testsetName = '<%= TestsetName%>';
             //qtip - TestsetName
             $('#DivTestsetName').qtip({
                 content: testsetName,
                 show: { event: 'mouseover' },
                 style: {
                     width: 500, padding: 15, background: '#F68500', color: 'white', textAlign: 'center',
                     border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
                 },
                 position: { corner: { tooltip: 'bottomMiddle', target: 'bottomMiddle' }, adjust: { x: 0, y: -20 } },
                 hide: { when: { event: 'mouseout' }, fixed: false }  
             });

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
                 setTimeout(function () { $('#Answer0').qtip('destroy'); }, 3000);
             }, 10000);

             //qtip - TestsetName
             $('#divSideTestsetName').qtip({
                 content: testsetName,
                 show: { event: 'mouseover' },
                 style: {
                     width: 500, padding: 15, background: '#F68500', color: 'white', textAlign: 'center',
                     border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
                 },
                 position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' }, adjust: { x: -10, y: 0 } },
                 hide: { when: { event: 'mouseout' }, fixed: false }
             });


             $('.stuAns').each(function () {
                 new FastButton(this, TriggerSaveChoice);
             });

             $('#sortable').sortable({
                 placeholder: "ui-state-highlight",
                 stop: function (event, ui) {
                     getIdLi();
                 }
             });

             var IsLeapChoice = $('#HDNotReplyMode').val();
             if (IsLeapChoice == 'True') {
                 $('#divQuestionAndAnswer').css('background-color', 'yellow');
             } else {
                 $('#divQuestionAndAnswer').css('background-color', 'transparent');
             }
         });

         function CloseDivnot() {
             //alert('aaa');
             //var a = document.getElementById('DivNotHaveDontReplyChoice');
             //a.style.display = 'none';
             $('#DivNotHaveDontReplyChoice').css("display", "none");
         }

         //Trigger SaveChoice
         function TriggerSaveChoice(e) {
             console.log('TriggerSaveChoice');
             var obj = e.target || e.srcElement;
             //if ($('#hdStatusDict').val() == 'On') {

             //    console.log('คำศัพท์คำตอบ : ' + e.target.innerHTML);
             //    console.log('background-color : ' + obj.style.backgroundColor);
             //    if ($(obj).hasClass('ans') || ($(obj).css('background-color') == "rgb(252,252,5)" )) $(obj).css('background-color', '#FCFC05');
             //    //if($(obj).prev().hasClass('ans')) $(obj).css('background-color', '#FCFC05');
             //    //if($(obj).next().hasClass('ans')) $(obj).css('background-color', '#FCFC05');
             //    GetWordSeleter();
             //    //goSearch(e.target.innerHTML);
             //    return 0;
             //}
             console.log('next step;');
             if (!$(obj).is('td')) {
                 console.log('parent 1 ไม่ใช่ td : ' + $(obj).html());
                 obj = $(obj).parent();
                 if (!$(obj).is('td')) {
                     console.log('parent 2 ไม่ใช่ td : ' + $(obj).html());
                     obj = $(obj).parent('td');
                 }
             }
             var questionid = $(obj).attr('questionid');
             var answerid = $(obj).attr('answerid');
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
             console.log('SetAnswerChoiceQuestion');
             KeepOldAnswer(ObjControl, IsNumOne);
             $(ObjControl).addClass('ans');

             $.ajax({
                 type: "POST",
                 url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/SetAnswerChoiceQuestion",
                 data: "{ QuestionId: '" + QuestionId + "', AnswerId:'" + AnswerId + "', PlayerId:'" + PlayerId + "', NotReplyMode:'" + 'False' + "', DeviceId:'" + "" + "' }",
                 contentType: "application/json; charset=utf-8", dataType: "json",
                 success: function (data) {
                     console.log('return from services = ' + data.d);
                     if (data.d == '1') {
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

     <script type="text/javascript">
         var slider3 = new Swipe(document.getElementById('slider3'));
         var SwipeCartoon = new Swipe(document.getElementById('SwipeCartoon'));
        </script>

    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var IsTimePerQuestion = '<%=IsTimePerQuestion%>';
        $(function () {

            //ถ้าเป็นฝึกฝนจาก Computer ให้ซ่อน Tab สมุดโน็ต
            var JVCheckIsPracticeByComputer = '<%= PracticeFromComputer%>';
            if (JVCheckIsPracticeByComputer == 'True') {
                $('#tab2').hide();
                //$('#tab1').css({ 'width': '100%' });
                $('#tab1').css({ 'width': '430px', 'margin-left': '-6px' });
                $('.noteHeadTab label').css({ 'padding': '10px 35% 10px 35%' });
            }


            //ปุ่ม Back
            if (ExamNum != 1) {
                console.log('examnum = ' + ExamNum);
                if ($('#btnPrvTop').length != 0) {
                    new FastButton(document.getElementById('btnPrvTop'), TriggerServerButton);
                }
                if ($('#btnPrvSide').length != 0) {
                    new FastButton(document.getElementById('btnPrvSide'), TriggerServerButton);
                }
            }


            //ปุ่ม Next
            if ($('#btnNextTop').length != 0) {
                new FastButton(document.getElementById('btnNextTop'), TriggerServerButton);
            }
            if ($('#btnNextSide').length != 0) {
                new FastButton(document.getElementById('btnNextSide'), TriggerServerButton);
            }

            //Panel ข้อข้าม
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

            //Div ยังไม่ตอบ
            if ($('.replyAnswer').length != 0) {
                $('.replyAnswer').each(function () {
                    new FastButton(this, TriggerReplyAnswer);
                });
            }

            //กดหุบ Div ยังไม่ตอบ
            if ($('#ForActivityPage').length != 0) {
                new FastButton(document.getElementById('ForActivityPage'), TriggerDivForActivityPage);
            }

            //ปุ่มเปิด Div โชว์อันดับคะแนน
            if ($('#btnShowTopScore').length != 0) {
                new FastButton(document.getElementById('btnShowTopScore'), TriggerServerButton);
            }

            //กดหุบ Div โชว์อันดับคะแนน
            if ($('#DivTopScoreStudent').length != 0) {
                new FastButton(document.getElementById('DivTopScoreStudent'), TriggerDivTopScoreStudent);
            }

            //ปุ่มเปิดหน้า โพเดียม
            if ($('.imgShowStudentStatusPage').length != 0) {
                $('.imgShowStudentStatusPage').each(function () {
                    new FastButton(this, TriggerServerButton);
                });
            }

            //ปุ่มเปิดหน้า CheckTablet
            if ($('#DivCheckStatustablet').length != 0) {
                new FastButton(document.getElementById('DivCheckStatustablet'), CheckStatusTablet);
            }
            if ($('#imgCheckStatusTablet').length != 0) {
                new FastButton(document.getElementById('imgCheckStatusTablet'), CheckStatusTablet);
            }

            // กดหยุดนาฬิกา
            //$("div.needTimer").click(function () {                
            //    CountTime();
            //});
            //$("div.PauseTime").live('click', function () {
            //    StopCountTime();
            //    $(this).remove();
            //});
            if ($('div.needTimer').length != 0) {
                $('div.needTimer').each(function () {
                    new FastButton(this, ClickTimerToPause);
                });
            }
            if ($("div.PauseTime").length != 0) {
                $('div.PauseTime').each(function () {
                    new FastButton(this, ClickDivPauseToPlay);
                });
            }

            if (isAndroid) {
                //ปุ่มกดเปิดเมนูขึ้นมา
                if ($('.divSetting').length != 0) {
                    $('.divSetting').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
                //ปุ่มเปิดหน้ารายงาน
                if ($('#DivViewReport1').length != 0) {
                    new FastButton(document.getElementById('DivViewReport1'), TriggerServerButton);
                }
                //ปุ่มกดจบควิซ
                if ($('#DivExit1').length != 0) {
                    new FastButton(document.getElementById('DivExit1'), TriggerServerButton);
                }
                //ปุ่มเปิดเครื่องคิดเลข
                if ($('#btnCalculatorTop').length != 0) {
                    new FastButton(document.getElementById('btnCalculatorTop'), TriggerServerButton);
                }
                //ปุ่มเปิด Dictionary
                if ($('.btnDictionary').length != 0) {
                    $('.btnDictionary').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
                //ปุ่มเปิด สมุดคำศัพท์
                if ($('#btnWordBookTop').length != 0) {
                    new FastButton(document.getElementById('btnWordBookTop'), TriggerServerButton);
                }
                //ปุ่มเปิด กระดาษโน็ต
                if ($('.btnNote').length != 0) {
                    $('.btnNote').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
                //ปุ่มเปิด ไม้โปรแทรคเตอร์
                if ($('.btnProtractor').length != 0) {
                    $('.btnProtractor').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
            }


            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + 950);
                }
            }

            //$('#Help').click(function () {
            //    KeepTime();
            //    CountTime();
            //});

        });

        function TriggerReplyAnswer(e) {
            var obj = e.target;
            $(obj).trigger('click');
            if ($(obj).prop('tagName') == 'DIV') {
                $(obj).css('background-color', '#fec500');
            }
            else if ($(obj).prop('tagName') == 'SPAN') {
                $(obj).parent().css('background-color', '#fec500');
            }
        }

        function TriggerDivForActivityPage(e) {
            var obj = e.target;
            $(obj).trigger('click');
            if ($(obj).attr('id') == 'ForActivityPage') {
                $(obj).css('background-color', '#fec500');
            }
            else {
                $(obj).parent().css('background-color', '#fec500');
            }

        }

        function TriggerDivTopScoreStudent(e) {
            var obj = e.target;
            $(obj).trigger('click');
            if ($(obj).attr('id') == 'DivTopScoreStudent') {
                $(obj).css('background-color', '#12EC3E');
            }
            else {
                $(obj).parent().parent().parent().css('background-color', '#12EC3E');
            }
        }

        var arrKeepTime = [];
        var second_time;
        var second_timeN;
        function KeepTime() {
            clearInterval(second_time);
            clearInterval(second_timeN);
            arrKeepTime[0] = $("#minuteCountDown").text();
            arrKeepTime[1] = $("#secCountDown").text();
            arrKeepTime[2] = $("#minuteElapsedTime").text();
            arrKeepTime[3] = $("#secElapsedTime").text();
        }
        var TimeOnPause;
        var TimerPause;
        function CountTime() {
            TimeOnPause = 0;
            TimerPause = setInterval(function () {
                TimeOnPause++;
            }, 1000);
        }
        function StopCountTime() {
            clearInterval(TimerPause);
            SaveTimeOnPause();
            TimeOnPause = 0;
        }
        function SaveTimeOnPause() {

            if (TimeOnPause != 0 && IsTimePerQuestion != 'True') {
                $.ajax({
                    type: "POST",
                    url: '<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/SaveTimeOnPause',
                    data: "{ t : '" + TimeOnPause + "'}",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                    },
                    error: function myfunction(request, status) {
                    }
                });
            }
        }
        function ClickTimerToPause() {
            KeepTime();
            CountTime();
            var divQuestionHeight = $('#divQuestionAndAnswer').height();
            var h = divQuestionHeight + 100;
            console.log('height : ' + h + ' ----- ' + $(window).height());
            //var cssHeight = (h < $(window).height()) ? '100%' : h + 'px';
            //console.log('cssHeight : ' + cssHeight);
            $('div.needTimer').css({ 'background-color': 'transparent' });
            if (h < $(window).height()) {
                $("div.PauseTime").show();
            } else {
                h = h + 100;
                $("div.PauseTime").css({ 'height': h + 'px' }).show();
            }

            //$("div.PauseTime").show();
        }
        function ClickDivPauseToPlay() {
            StopCountTime();
            $("div.PauseTime").hide();
            timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false);
            timeNormal(arrKeepTime[2], arrKeepTime[3], true);
        }
    </script>

    <!--[if IE]>
    <script src="../js/selectivizr-min.js" type="text/javascript"></script>
    <![endif]-->
    <%If tools_Protractor = True Then%>
    <!--[if IE]>
        <script type="text/javascript" src="../js/EventHelpers.js"></script>
        <script type="text/javascript" src="../js/cssQuery-p.js"></script>
        <script type="text/javascript" src="../js/sylvester.js"></script>
        <script type="text/javascript" src="../js/cssSandpaper.js"></script>
    <![endif]-->
    <%End If%>
    <%If tools_Calculator = True Then%>
    <script src="../js/jsToolsCalculator.js" type="text/javascript"></script>
    <%End If%>
    <%--<link href="../css/video-js.min_hack.css" rel="stylesheet" type="text/css" />
    <script src="../js/video.min.js" type="text/javascript"></script>
    <script>
        _V_.options.flash.swf = "../js/video-js.swf";              
    </script>
    <script type="text/javascript">
        if ($.browser.msie && $.browser.version < 9.0) {
            $('video').remove(); // remvoe tah video ที่ใช้กัย Chorme,FF
        }
        else {
            $(document).ready(function () {
                $('#objVideo').remove(); // remove tag object ที่ใช้กับ IE <= 8.0                

                _V_("myVideoJS", {}, function () { });
                var srcFile = "<%= srcFileIntro %>";
                var typeFile = "<%= typeFileIntro %>";
                srcFile = "../MultiMedia/Audio/Maid with the Flaxen Hair.mp3";
                typeFile = "audio/mpeg";


                _V_("myVideoJS").ready(function () {
                    var myVideo = this;
                    //myVideo.src({ src: "../css/a.mp3", type: "audio/mpeg" });
                    //myVideo.src({ src: srcFile.toString(), type: typeFile.toString() });
                    //myVideo.controlBar.fadeOut();                        
                });

                $('.vjs-current-time').live().remove();
                $('.vjs-duration').live().remove();
            });
        }
    </script>--%>
    <% If Not IE = "1" Then%>
    <%If Not Session("selectedSession") = "PracticeFromComputer" Then%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script type="text/javascript">
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (IsTimePerQuestion == 'True') {
                return "ถ้า ออกจากหน้านี้ ควิซที่จับเวลาอยู่ เวลาจะหยุดเดินค่ะ";
            }
        }

        $(function () {
            $('.BtnNextPreKeepTime').click(function () {
                window.onbeforeunload = null;
            });
        });
        var SignalRCheck;
        var Unload; var CurrentPage;
        var withOutClick = true;
        var firstClick = true;
        var IsSender;
        var IsPracticePC = false;

        var Groupname = '<%=GroupName %>'; // เก็บ GroupName        
        var SelectedName = GetSelectedSession();
        var CurrentQuizId = '<%=CurrentQuizId %>';
        var ExamNum = '<%=_ExamNum %>';
        var AnswerState = '<%=_AnswerState %>';

        var ThisPage = (window.location.pathname).toLowerCase().substring(1).replace(ResolveUrl("~/").toLowerCase().substring(1), '');
        var Querystring = window.location.search;

        //console.log(ThisPage);

        $(function () {

            // console.log('ค่า hd ตอนเริ่ม' + $('#hdIsSender').val());
            IsSender = $('#hdIsSender').val();
            //console.log('IsSender ตอนรับค่า ' + IsSender);            
            if (IsSender) {
                SetCurrentPage(ThisPage);
                SetCurrentQuerystring(Querystring);
            }
        });

        window.hubReady = $.connection.hub.start();

        //$.connection.hub.start(function () {
        window.hubReady.done(function () {
            $('#backOnload').remove();
            //var dd = new Date(); console.log('conected success' + dd.toLocaleTimeString() + ' ' + dd.getMilliseconds());

            SignalRCheck.server.addToGroup(Groupname);

            if ($('#CallStudentFirst').val() == 0) {
                $('#CallStudentFirst').val(1);
                //SendReload();
            }

            SignalRCheck.server.addToGroup(SelectedName);
            SignalRCheck.server.sendCommand(SelectedName, ThisPage + window.location.search);

            // btnNext
            $('#<%=btnNextTop.ClientID %>').click(function (e) {
                //                if (firstClick) {
                //                    e.preventDefault();
                //                    IsLastQuestion('Next');
                //                }
                $('#hdIsSender').val(true);
                //SendReload();
                // SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
            $('#<%=btnNextSide.ClientID %>').click(function (e) {
                //OpenBlockUI();
                $('#hdIsSender').val(true);
                //SendReload();
                //SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
            $('#<%=btnNext.ClientID %>').click(function (e) {
                //OpenBlockUI();
                $('#hdIsSender').val(true);
                //SendReload();
                //SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
            // btn Prev
            $('#<%=btnPrvTop.ClientID%>').click(function (e) {
                //                if (firstClick) {
                //                    e.preventDefault();
                //                    SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Prev').done(function () {
                //                        console.log('success cmd');
                //                    }).fail(function (e) {
                //                        console.log('fail cmd');
                //                        console.warn(e);
                //                    });
                //                }                
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
                OpenBlockUI();
                $('#hdIsSender').val(true);
                //SendReload();
                //SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
            $('#<%=btnPrvSide.ClientID %>').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
                OpenBlockUI();
                $('#hdIsSender').val(true);
                //SendReload();
                //SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
            $('#<%=btnPrevious.ClientID %>').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
                OpenBlockUI();
                $('#hdIsSender').val(true);
                //SendReload();
                //SignalRCheck.server.cmdControlBtnPrevNext(SelectedName, 'Test');
            });
        });

        SignalRCheck = $.connection.hubSignalR;
        SignalRCheck.client.send = function (message) {
            //console.log('message ตอนรับ ' + message);
            if (CoverPageName(message) == ThisPage || message == 'EndQuiz' || message == 'Reload2' || message.indexOf('ExamNum|') != -1) {
            }
            else if (message == 'CheckAgain') {
                //  console.log('Issender ตอนรับ ' + IsSender);
                if (IsSender == "false") {
                    CheckQQNoAfterLoading();
                }
            } else if (message == 'Reload') {
                //window.location = '<%=ResolveUrl("~")%>' + ThisPage + Querystring;
                //} else if (message == 'QuizDuplicate') {
            } else if (message.indexOf('QuizDuplicate') != -1) {
                message = message.replace('QuizDuplicate|', '');
                $('#dialog').dialog({
                    autoOpen: open,
                    buttons: {
                        'ตกลง': function () {
                            window.onbeforeunload = null;
                            window.location = '<%=ResolveUrl("~")%>Quiz/DashboardQuizPage.aspx';
                        }
                    },
                    draggable: false,
                    resizable: false,
                    modal: true
                }).dialog('option', 'title', 'ควิซนี้โดนปิดไปแล้วค่ะ โดยคุณครู ' + message + ' เปิดควิซซ้อนมาค่ะ');
                if ($('.ui-button').length != 0) {
                    $('.ui-button').each(function () {
                        new FastButton(this, TriggerServerButton);
                    });
                }
            }
            else {
                window.onbeforeunload = null;
                window.location = '<%=ResolveUrl("~")%>' + message;
            }
        };
// รับค่าจากที่ส่งมา
SignalRCheck.client.cmdControl = function (cmd) {
    setTimeout(function () {
        window.location = '<%=ResolveUrl("~")%>' + ThisPage + Querystring;
    }, 500);
    //            if (cmd == 'Next') {
    //                firstClick = false;
    //                $('#<%=btnNextTop.ClientID %>').trigger('click');

    //            }
    //            else if (cmd == 'Prev') {
    //                firstClick = false;
    //                $('#<%=btnPrvTop.ClientID %>').trigger('click');
    //            }
};
// รับค่าของตัวเองหลังจากที่ส่งไป
//        SignalRCheck.client.raiseEvent = function (cmd) {
//            if (cmd == 'Next') {
//                firstClick = false;
//                SendReload();
//                $('#<%=btnNextTop.ClientID %>').trigger('click');
        //            }
        //            else if (cmd == 'Prev') {
        //                firstClick = false;
        //                SendReload();
        //                $('#<%=btnPrvTop.ClientID %>').trigger('click');
        //            }
        //        };



        // send หาเด็กให้ reload
        function SendReload() {
            //var d = new Date(); $('#testhd').val(d.toLocaleTimeString() + ' ' + d.getMilliseconds());               
            if (Groupname != "" && Groupname != null && Groupname != undefined) {
                SignalRCheck.server.sendCommand(Groupname, 'Reload').done(function () { //console.log('send Reload'); 
                });
                var a = '<%= DateTime.Now.ToString()%>';
                //alert(a);
            }
        }
        // function เช็ค ข้อสุดท้าย

        function CoverPageName(url) {
            var u = 'activity/activitypage.aspx';
            if (url.toLowerCase().indexOf(u) != -1) {
                return u;
            }
        }
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
                    //alert('GetSelectedSession');
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
                    //alert('SetUnload');
                }
            });
        }
        //        function GetUnload() {
        //            var Unload;
        //            $.ajax({ type: "POST",
        //                url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetUnload'),
        //                async: false,
        //                contentType: "application/json; charset=utf-8", dataType: "json",
        //                success: function (data) {
        //                    Unload = data.d;
        //                },
        //                error: function myfunction(request, status) {
        //                    alert('GetUnload');
        //                }
        //            });
        //            return Unload;
        //        }
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
                    //alert('SetCurrentPage');
                }
            });
        }
        //        function GetCurrentPage() {
        //            var CurrentPage;
        //            $.ajax({ type: "POST",
        //                url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/GetCurrentPage'),
        //                async: false,
        //                contentType: "application/json; charset=utf-8", dataType: "json",
        //                success: function (data) {
        //                    CurrentPage = data.d;
        //                },
        //                error: function myfunction(request, status) {
        //                    alert('GetCurrentPage');
        //                }
        //            });
        //            return CurrentPage;
        //        }
        function SetCurrentQuerystring(Querystring) {
            $.ajax({
                type: "POST",
                url: ResolveUrl('~/WebServices/DashboardSignalRService.asmx/SetCurrentQuerystring'),
                data: "{ Querystring : '" + Querystring + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                },
                error: function myfunction(request, status) {
                    //alert('SetCurrentQuerystring');
                }
            });
        }
    </script>
    <%Else%>
    <script type="text/javascript">
        var IsPracticePC = true;
        $(function () {
            $('#backOnload').remove();
            $('#<%=btnPrvTop.ClientID%>').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
            });
            $('#<%=btnPrvSide.ClientID %>').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
            });
            $('#<%=btnPrevious.ClientID %>').click(function (e) {
                if (ExamNum == 1 || ExamNum == '1') {
                    e.preventDefault();
                    return false;
                }
            });
        });
    </script>
    <% End If%>
    <%Else%>
    <script type="text/javascript" >
        var IsPracticePC = false;
        var Groupname = '<%=GroupName %>'; // เก็บ GroupName        
        var SelectedName = '0000';
        var CurrentQuizId = '<%=CurrentQuizId %>';
        var ExamNum = '<%=_ExamNum %>';
        var AnswerState = '<%=_AnswerState %>';
        $(function () {
            $('#backOnload').remove();
        });
    </script>    
    <%End If%>
    <script type="text/javascript">
        var CheckDialogState = '<%=Dialog %>';
        $(document).ready(function () {
            $('#btnNextTop').click(function (e) {
                //alert('T');
                //alert(CheckDialogState);
                if (CheckDialogState == 'True') {
                    e.preventDefault();
                    GetRenderNextStep(1);
                }
                else {
                    OpenBlockUI();
                }
            });

            $('#btnNextSide').click(function (e) {
                //alert('S');
                if (CheckDialogState == 'True') {
                    e.preventDefault();
                    GetRenderNextStep(1);
                }
                else {
                    OpenBlockUI();
                }
            });

            $('#btnNext').click(function (e) {
                //alert('N');

                if (CheckDialogState == 'True') {
                    e.preventDefault();
                    GetRenderNextStep(1);

                }
                else {
                    OpenBlockUI();

                }
            });



            $('#DivExit1').click(function () {
                GetRenderNextStep(1);
            });

            $('#DivExit2').click(function () {
                GetRenderNextStep(1);
            });

            //Dialog

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

        });



        function GetRenderNextStep(EventType) {
            var EType = EventType;
            var AnswerState = '<%=_AnswerState %>';
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/GetRenderNextStep",
                data: "{ EventType: '" + EType + "', AnswerState: '" + AnswerState + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) { //session(UserId) หายจะ return 0 - Sefety function
                        var ShowDialog = jQuery.parseJSON(data.d);
                        var DialogType = ShowDialog.DialogType;
                        var IsShowDialog = ShowDialog.IsShowDialog;
                        var txtForShowDialog = ShowDialog.txtForShowDialog;
                        var NextStep = ShowDialog.NextStep;
                        if (IsShowDialog == 'True') {
                            if (DialogType == '1') {
                                CallDialogImgBtn("เฉลยเลย", "ยังก่อน", txtForShowDialog, NextStep, 1);
                            } else if (DialogType == '2') {
                                CallDialog("ออกเลย", "ยังก่อน", txtForShowDialog, NextStep, 2);
                            }
                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

        function CallDialogImgBtn(btnConfirmName, btnCancleName, txtForShowDialog, NextStep, eventType) {
            var myBtn = {};
            myBtn = [{
                text: btnCancleName,          
                click: function () {
                    if (eventType == 1 || eventType == 2) {
                        $(this).dialog('close');
                    }
                }
            },
                    {
                        text: "",//"ออกเลย"
                        "class": "btnReplyDialog",
                        click: function () {
                            if (eventType == 1) {
                                NextToLeapChoice(1);
                            } else if (eventType == 2) {
                                $(this).dialog('close');
                                window.onbeforeunload = null;
                                window.location.href = NextStep
                            }
                        }
                    }];

            $('#LinkDialog').dialog({
                autoOpen: false,
                buttons: myBtn,
                draggable: false, resizable: false, modal: true
            }).dialog('option', 'title', txtForShowDialog).dialog('open');
            if ($('.ui-button').length != 0) {
                $('.ui-button').each(function () {
                    //new FastButton(this, TriggerServerButton);
                });
            }
        }

        function CallDialog(btnConfirmName, btnCancleName, txtForShowDialog, NextStep, eventType) {
            console.log(IsPracticePC);
            var myBtn = {};
            if (IsPracticePC) {
                myBtn = [{
                    text: "ยกเลิก",
                    //"class": "btnCancleExit",
                    click: function () {
                        if (eventType == 1 || eventType == 2) {
                            $(this).dialog('close');
                        }
                    }
                },
                        {
                            text: "ออกเลย",
                            //"class": "btnConfirmExit",
                            click: function () {
                                if (eventType == 1) {
                                    NextToLeapChoice(1);
                                } else if (eventType == 2) {
                                    $(this).dialog('close');
                                    window.onbeforeunload = null;
                                    window.location.href = NextStep
                                }
                            }
                        }];
            } else {
                myBtn[btnConfirmName] = function () {
                    if (eventType == 1) {
                        NextToLeapChoice(1);
                    } else if (eventType == 2) {
                        if (!IsPracticePC) { // สำหรับเข้า session เดิมแล้วออกไม่ได้                       
                            SetCurrentPage(NextStep.replace('../', '').toLowerCase());
                        }
                        $(this).dialog('close');
                        window.onbeforeunload = null;
                        if (!isAndroid) {
                            FadePageTransitionOut();
                        }
                        window.location.href = NextStep
                    }
                };
                myBtn[btnCancleName] = function () {
                    if (eventType == 1 || eventType == 2) {
                        $(this).dialog('close');
                    }
                };
            }

            $('#LinkDialog').dialog({
                autoOpen: false,
                buttons: myBtn,
                draggable: false, resizable: false, modal: true
            }).dialog('option', 'title', txtForShowDialog).dialog('open');
            if ($('.ui-button').length != 0) {
                $('.ui-button').each(function () {
                    //new FastButton(this, TriggerServerButton);
                });
            }
        }


        function NextToLeapChoice(IsCorrect) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/NextToLeapChoice",      //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/NextToLeapChoice",             
                data: "{ IsCorrect : '" + IsCorrect + "', PlayerId : '1'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    CheckLastQuestion = data.d;
                    $('#LinkDialog').dialog('close');
                    //alert('kk');
                    CheckDialogState = 'False'
                    $('#btnNextTop').trigger('click');
                    //alert('oo');
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

        function LeapChoiceOnclick(QQ_No, IsScore, NotReplyMode) {

            //alert(QQ_No);

            $('#HDLastChoice').val('False');
            $('#HDIsLeapChoice').val('True');


            $('#HDQQ_No').val(QQ_No);
            $('#HDIsScore').val(IsScore);

            if (NotReplyMode == 'T') {
                $('#HDNotReplyMode').val('True');

            }
            else {
                $('#HDQQ_No').val(QQ_No);
                $('#HDNotReplyMode').val('False');
            }
            //alert($('#HDIsLeapChoice').val());
            form1.submit();
        }

        //



        function OpenBlockUI() {
            $.blockUI({
                message: '<h1><span style="color:#FFC76F;">รอสักครู่นะคะ</span></h1>',
                css: {
                    border: 'none',
                    //padding:'15px',
                    backgroundColor: 'transparent'
                }
            });
        }

    </script>
    <script type="text/javascript">
        function HilightClick2(ObjControl, QuestionId, AnswerId) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/UpdateScore",
                data: "{ Questionid: '" + QuestionId + "', AnswerId:'" + AnswerId + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) { //session(UserId) หายจะ return 0 - Sefety function
                        if (data.d == '1') {
                            $('td').css('background-color', 'transparent');
                            $(ObjControl).prev().css('background-color', '#FCFC05');
                            $(ObjControl).css('background-color', '#FCFC05');
                        }
                        else {
                            //alert('เซฟไม่ผ่าน คืน "" มา');
                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                    //alert('shin' + request.statusText + status);    
                }
            });
        }
    </script>
    <script type="text/javascript">

        var s_needTimer = '<%=Session("NeedTimer")%>';
        //var QuizUseTablet = '<%=Session("QuizUseTablet")%>';
        var ExamNum = '<%=_ExamNum %>';
        $(function () {

            $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#Help > li').hover(function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-4px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
            });

            if ($('#hdTotalStudent').val() == '') {
                GetTotalNumberStudent();
            }

            if (parseInt($('#hdTotalStudent').val()) >= 13) {
                $('#DivCoverForActivityPage2').css('height', '90px');
                $('#DivCoverTopScoreStudent2').css('height', '90px');
                $('.imgShowPanelScore2').css('top', '-60px');
                $('.imgShowStudentStatusPage2').css('top', '-50px');
            }
            else {
                $('#DivCoverForActivityPage2').css('height', '50px');
                $('#DivCoverTopScoreStudent2').css('height', '50px');
                $('.imgShowPanelScore2').css('top', '-40px');
                $('.imgShowStudentStatusPage2').css('top', '-30px');
            }


            var bool = '<%=Session("QuizUseTablet") %>';
            var QuestionId = '<%= questionId %>';
            $('#hdQuestionId').val(QuestionId);
            var UserId = '<%= UserId %>';
            $('#hdUserId').val(UserId);

            //if ($('#checkStratTimer').val() == 1 || ($('#checkStratTimer').val() == 0 && s_needTimer == 'False')) {
            if ($('#checkStratTimer').val() == 1) {
                getNeedTimer();
            }

            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;
            if (!isAndroid) {
                //alert('เข้า if ที่ไม่ใช่ Andriod');
                //                    $("#ForActivityPage").alternateScroll();
                //                    $("#ForActivityPage2").alternateScroll();
                //                    $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />');
                //                    $('.alt-scroll-holder').css('width','200px');
                // $('#ForActivityPage').slimScroll();
            }

            //            $('#dialog').dialog({
            //                autoOpen: false,
            //                buttons: { 'ใช่': function () { window.location.href = '../Activity/AlternativePage.aspx'; }, 'ไม่': function () { $(this).dialog('close'); } },
            //                draggable: false,
            //                resizable: false,
            //                modal: true
            //            });

            $('.about').click(function () {
                window.onbeforeunload = null;
                window.location.href = '/Activity/ActivityReport.aspx';
            });

            $('#imgSetting1').click(function () {
                if ($('#DivExit1').css('display') == 'none') {
                    if (isAndroid) {
                        $('#DivExit1').show();
                        $('#DivViewReport1').show();
                        $('.ForToolsTop').show();
                    }
                    else {
                        $('#DivExit1').show(300);
                        $('#DivViewReport1').show(300);
                        $('.ForToolsTop').show(300);
                    }
                }
                else {
                    if (isAndroid) {
                        $('#DivExit1').hide();
                        $('#DivViewReport1').hide();
                        $('.ForToolsTop').hide();
                    }
                    else {
                        $('#DivExit1').hide(300);
                        $('#DivViewReport1').hide(300);
                        $('.ForToolsTop').hide(300);
                    }
                }
            });

            //--------------------------------------------------- ปุ่ม Setting(ดูคะแนน,ดูรายงาน)
            $('#imgSetting2').click(function () {
                if ($('#DivExit2').css('display') == 'none') {
                    $('#DivExit2').show(300);
                    $('#DivViewReport2').show(300);
                    $('.ForToolsLeft').show(300);
                }
                else {
                    $('#DivExit2').hide(300);
                    $('#DivViewReport2').hide(300);
                    $('.ForToolsLeft').hide(300);
                }
            });


            //---------------------------------------------------

            // Check Menu Position
            checkMenu();
            //alert($('#ShowBtnOnTapMenu').val());
            function checkMenu() {
                // เช็ค ปุ่มเลื่อนข้อ
                var menuState = $('#ShowBtnOnTapMenu').val();
                console.log(menuState);
                // 0 ปุ่มเลื่อนข้ออยู่บนเมนูข้างบนและข้าง
                if (menuState == 0) {
                    $('#divNextPre').hide();
                    $('.menuTopSideNextPre').show();
                } // 1 โชว์แบบที่มีคนวิ่ง
                else {
                    $('#divNextPre').show();
                    $('.menuTopSideNextPre').hide();
                }

                // เช็คเมนูอยู่ตำแหน่งไหน
                var menuOnPosition = $('#ShowMenuOnPosTop').val();
                // 0 อยู่ข้างบน
                if (menuOnPosition == 0) {
                    //$('.divMenuTop').show();
                    //$('.divMenuSide').hide();
                    $('#DivMenuTops').show();
                    $('#DivImgTopMenu').show();
                    $("div.MainReply").css("left", "-150px");
                } // 1 อยู่ด้านข้าง
                else {
                    $('.divMenuTop').hide();
                    //$('.divMenuSide').show();
                    $('#DivImgTopMenu').hide();
                    $('#DivSideMenus').show();
                    $('#DivimgSideMenu').show();
                    $("div.MainReply").css("left", "680px");
                }

                // เมนูแสดงคนตอบ
                var divShowUserReply = $('#ShowDivUserReply').val();
                if (divShowUserReply == 0) {
                    $('.replyAnswer').show();
                    $('#ForActivityPage').hide();
                    $('.divMenuTop').css("height", "100px")
                    //$('#ForActivityPage2').hide();
                    $('#DivCoverForActivityPage2').hide();
                    $('.imgShowPanelScore2').hide();
                }
                else {
                    //ถ้าเปิด Panel แสดงคะแนนไว้
                    if (divShowUserReply == 2) {
                        $('.replyAnswer').hide();
                        $('.replyAnswer2').hide();
                        //หาว่ามีเด็กเกิน 13 คนรึเปล่า
                        if ($('#hdTotalStudent').val() == '') {
                            GetTotalNumberStudent();
                        }

                        //หาว่าแถบเมนูโชว์อยู่ด้านข้างหรือเปล่า ?
                        if (menuOnPosition == 1) {
                            //$('#ForActivityPage2').show();
                            GetDataScoreStudent(true);
                            $('#DivCoverForActivityPage2').show();
                            $('.imgShowPanelScore2').show();
                            $('#DivCoverTopScoreStudent2').show();
                            $('.imgShowStudentStatusPage2').show();
                        }
                        else//ถ้าเมนูอยู่ด้านบน
                        {
                            //$('#ForActivityPage2').hide();
                            GetDataScoreStudent(false);
                            $('#DivCoverForActivityPage2').hide();
                            $('.imgShowPanelScore2').hide();
                            $('#DivCoverForActivityPage2').hide();
                            $('.imgShowPanelScore').show();
                            $('#DivTopScoreStudent').show();
                            $('.imgShowStudentStatusPage').show();
                        }

                        if (parseInt($('#hdTotalStudent').val()) >= 13) {
                            $('.divMenuTop').css("height", "265px");
                            $('#ForActivityPage').css('height', '70px');
                            $('#DivTopScoreStudent').css('height', '70px');
                            $('#ForActivityPage2').css('height', '70px');
                            $('#DivTopScoreStudent2').css('height', '70px');
                        }
                        else {
                            $('.divMenuTop').css("height", "220px");
                            $('#ForActivityPage').css('height', '50px');
                            $('#DivTopScoreStudent').css('height', '50px');
                            $('#ForActivityPage2').css('height', '50px');
                            $('#DivTopScoreStudent2').css('height', '50px');
                            //$('.imgShowStudentStatusPage').css('top', '95px')
                            $('.imgShowStudentStatusPage').css('top', '157px');
                        }

                        //$('#ForActivityPage').hide(500);

                        OnInterval();
                    }
                        //ถ้าเปิดแต่ Panel แสดงคนยังไม่ตอบอย่างเดียว
                    else {
                        $('.replyAnswer').hide();
                        $('.replyAnswer2').hide();

                        if (menuOnPosition == 1) {
                            //$('#ForActivityPage2').show();
                            $('#DivCoverForActivityPage2').show();
                            $('.imgShowPanelScore2').show();
                        }
                        else {
                            //$('#ForActivityPage2').hide();
                            $('#DivCoverForActivityPage2').hide();
                            $('#ForActivityPage').show();
                            $('.imgShowPanelScore').show();
                        }

                        if (parseInt($('#hdTotalStudent').val()) >= 13) {
                            $('.divMenuTop').css("height", "170px");
                            $('#ForActivityPage').css('height', '70px');
                            $('#DivTopScoreStudent').css('height', '70px');
                            $('#ForActivityPage2').css('height', '70px');
                            $('#DivTopScoreStudent2').css('height', '70px');
                        }
                        else {
                            $('.divMenuTop').css("height", "150px");
                            $('#ForActivityPage').css('height', '50px');
                            $('#DivTopScoreStudent').css('height', '50px');
                            $('#ForActivityPage2').css('height', '50px');
                            $('#DivTopScoreStudent2').css('height', '50px');
                            //$('.imgShowStudentStatusPage').css('top', '95px')
                            $('.imgShowStudentStatusPage').css('top', '157px')
                        }

                    }
                }
            }

            // Click Toggle Menu Top Side
            $('.imgToggle').click(function () {
                //                if ($(this).parent().is('.divMenuTop')) {
                if ($('#DivSideMenus').css('display') == 'none') {
                    $('#ShowMenuOnPosTop').val('1');
                    $('#DivMenuTops').hide('slow');
                    $('#DivImgTopMenu').hide('slow');

                    if ($('#ForActivityPage').css('display') == 'block') {
                        $('.imgShowPanelScore').show();
                        $('.replyAnswer2').hide();

                        //$('#ForActivityPage2').show();
                        $('#DivCoverForActivityPage2').show();
                        $('#ForActivityPage2').show();
                        $('.imgShowPanelScore2').show();
                        if ($('#ShowDivUserReply').val() == '2') {

                            //$('#DivTopScoreStudent2').show();
                            $('#DivCoverTopScoreStudent2').show();
                            $('#DivTopScoreStudent2').show();
                            $('.imgShowStudentStatusPage2').show();
                            OffInterval();
                            OnInterval();
                            GetDataScoreStudent(true);
                        }
                        else {
                            //$('#DivTopScoreStudent2').hide();
                            $('#DivCoverTopScoreStudent2').hide();
                            $('.imgShowStudentStatusPage2').hide();
                        }
                    }
                    else {
                        $('.replyAnswer2').show();
                        //$('#ForActivityPage2').hide();
                        $('#DivCoverForActivityPage2').hide();
                        $('.imgShowPanelScore2').hide();
                        //$('#ForActivityPage2').show();
                    }



                    //$('.divMenuTop').hide();
                    //$('.divMenuSide').show();
                    $('#DivSideMenus').fadeIn('slow');
                    $('#DivimgSideMenu').fadeIn('slow');
                    if ($('#ShowDivUserReply').val() == 0) {
                        //$('#ForActivityPage2').hide();
                        $('#DivCoverForActivityPage2').hide();
                        $('.imgShowPanelScore2').hide();
                    }
                    else {
                        //$('#ForActivityPage2').show();
                        $('#DivCoverForActivityPage2').show();
                        $('.imgShowPanelScore2').show();
                    }

                    // ปรับให้ปุ่มเฉลยไปอยู่ทางด้านขวา
                    $("div.MainReply").css("left", "680px");
                }

                else {
                    $('#ShowMenuOnPosTop').val('0');
                    //$('.divMenuTop').show();

                    $('#DivImgTopMenu').show('slow');

                    if ($('#DivCoverForActivityPage2').css('display') == 'block') {
                        $('.replyAnswer').hide();
                        $('.imgShowPanelScore').show();
                        $('#ForActivityPage').show();
                        if ($('#ShowDivUserReply').val() == '2') {
                            $('#DivTopScoreStudent').show();
                            $('.imgShowStudentStatusPage').show();
                            GetDataScoreStudent(false);
                            OffInterval();
                            OnInterval();
                            if (parseInt($('#hdTotalStudent').val()) >= 13) {
                                $('.divMenuTop').css("height", "265px")
                            }
                            else {
                                $('.divMenuTop').css("height", "220px")
                            }
                        }
                        else {
                            //$('#DivTopScoreStudent2').hide();
                            $('#DivCoverTopScoreStudent2').hide();
                            $('.imgShowStudentStatusPage2').hide();
                            $('#DivTopScoreStudent').hide();
                            $('.imgShowStudentStatusPage').hide();
                            if (parseInt($('#hdTotalStudent').val()) >= 13) {
                                $('.divMenuTop').css("height", "170px");
                                $('#ForActivityPage').css('height', '70px');
                                $('#DivTopScoreStudent').css('height', '70px');
                                $('#ForActivityPage2').css('height', '70px');
                                $('#DivTopScoreStudent2').css('height', '70px');
                            }
                            else {
                                $('.divMenuTop').css("height", "150px");
                                $('#ForActivityPage').css('height', '50px');
                                $('#DivTopScoreStudent').css('height', '50px');
                                $('#ForActivityPage2').css('height', '50px');
                                $('#DivTopScoreStudent2').css('height', '50px');
                                //$('.imgShowStudentStatusPage').css('top', '95px')
                                $('.imgShowStudentStatusPage').css('top', '157px');
                            }

                        }
                    }
                    else {
                        $('.replyAnswer').show();
                        $('#ForActivityPage').hide();
                    }

                    $('#DivMenuTops').show('slow');
                    //$('.divMenuSide').hide();
                    $('#DivSideMenus').hide('slow');
                    $('#DivimgSideMenu').hide('slow');
                    //$('#ForActivityPage2').hide();
                    $('#DivCoverForActivityPage2').hide();
                    $('.imgShowPanelScore2').hide();
                    //$('#DivTopScoreStudent2').hide();
                    $('#DivCoverTopScoreStudent2').hide();
                    $('.imgShowStudentStatusPage2').hide();

                    // ปรับให้ปุ่มเฉลยไปอยู่ทางด้านซ้ายเหมือนเดิม
                    $("div.MainReply").css("left", "-150px");
                }
            });
            //                $('.imgToggle').click(function () {
            //                if ($('#DivSideMenus').css('display') == 'none') {
            //                    $('#DivMenuTops').hide('slow');
            //                    $('#DivImgTopMenu').hide('slow');
            //                    // $('#DivMenuTops').css('display', 'none');
            //                    $('#DivSideMenus').fadeIn('slow');
            //                    $('#DivimgSideMenu').fadeIn('slow');
            //                    //$('#DivSideMenus').css('display', 'block');
            //                }
            //                else {

            //                    $('#DivSideMenus').hide('slow');
            //                    $('#DivimgSideMenu').hide('slow');

            //                    $('#DivMenuTops').fadeIn('slow');
            //                    $('#DivImgTopMenu').show('slow');


            //                    //                    $('#DivSideMenus').css('display', 'none');
            //                    //                    $('#DivMenuTops').css('display', 'block');
            //                }
            //            });


            // click hide/show btn Pre&Next Position
            $('.hideDivNextPre').click(function (e) {
                //alert(e.target.nodeName);
                if (e.target.nodeName != "INPUT") {
                    $('#ShowBtnOnTapMenu').val('0');
                    $('#divNextPre').hide(500);
                    //$('.menuTopSideNextPre').show();
                    $('.menuTopSideNextPre').fadeIn(1800);
                }
            });
            $('.positionImg').click(function () {
                $('#ShowBtnOnTapMenu').val('0');
                $('#divNextPre').hide(500);
                //$('.menuTopSideNextPre').show();
                $('.menuTopSideNextPre').fadeIn(1800);
            });

            $('.lblQuizName').click(function () {
                $('#ShowBtnOnTapMenu').val('1');
                $('#divNextPre').show(500);
                $('.menuTopSideNextPre').hide();
            });

            // Click Menu Setting
            $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            $('.divSetting').toggle(function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '5%' }, 200);
            }, function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            });

            //End Activity
            //            $('.endActivity').click(function () {
            //                $('#dialog').dialog('open');
            //            });

            var sumDontReply = $('#ForActivityPage').children('.divReplyAns').length;
            $('.sumDontReply').text(sumDontReply);

            //show/hide div Reply answer
            $('.replyAnswer').click(function () {
                if ($('#hdTotalStudent').val() == '') {
                    GetTotalNumberStudent();
                }
                if (parseInt($('#hdTotalStudent').val()) >= 13) {
                    $('.divMenuTop').css("height", "170px")
                    $('#ForActivityPage').css('height', '70px');
                    $('#DivTopScoreStudent').css('height', '70px');
                    $('#ForActivityPage2').css('height', '70px');
                    $('#DivTopScoreStudent2').css('height', '70px');
                }
                else {
                    $('.divMenuTop').css("height", "150px");
                    $('#ForActivityPage').css('height', '50px');
                    $('#DivTopScoreStudent').css('height', '50px');
                    $('#ForActivityPage2').css('height', '50px');
                    $('#DivTopScoreStudent2').css('height', '50px');
                    //$('.imgShowStudentStatusPage').css('top', '95px')
                    $('.imgShowStudentStatusPage').css('top', '157px')
                }
                $('.replyAnswer').hide(500);
                $('#ShowDivUserReply').val('1');
                $('#ForActivityPage').show(500);
                $('.imgShowPanelScore').show();


                //                if ($('#DivMenuTops').css('display') == 'block') {
                //                    $('#ForActivityPage2').hide();   
                //                }
                //                else
                //                {
                //                $('#ForActivityPage2').show();   
                //                }

            });

            $('.replyAnswer2').click(function () {
                $('#ShowDivUserReply').val('1');
                $('.replyAnswer2').hide();
                //$('#ForActivityPage2').show();
                $('#DivCoverForActivityPage2').show();
                $('#ForActivityPage2').show();
                $('.imgShowPanelScore2').show();
                $('#ForActivityPage').show(500);
                if (parseInt($('#hdTotalStudent').val()) >= 13) {
                    $('.divMenuTop').css("height", "170px")
                    $('#ForActivityPage').css('height', '70px');
                    $('#DivTopScoreStudent').css('height', '70px');
                    $('#ForActivityPage2').css('height', '70px');
                    $('#DivTopScoreStudent2').css('height', '70px');
                }
                else {
                    $('.divMenuTop').css("height", "150px");
                    $('#ForActivityPage').css('height', '50px');
                    $('#DivTopScoreStudent').css('height', '50px');
                    $('#ForActivityPage2').css('height', '50px');
                    $('#DivTopScoreStudent2').css('height', '50px');
                    //$('.imgShowStudentStatusPage').css('top', '95px')
                    $('.imgShowStudentStatusPage').css('top', '157px')
                }

            });



            $('#ForActivityPage').click(function (e) {
                if ($(e.target).is($(this))) {
                    $(this).hide(500);
                    $('#DivTopScoreStudent').hide();
                    $('.imgShowStudentStatusPage').hide();
                    $('#ShowDivUserReply').val('0');
                    $('.replyAnswer').show(500);
                    $('.divMenuTop').css("height", "100px")
                    OffInterval();
                    $('.imgShowPanelScore').hide();
                }

            });

            $('#ForActivityPage2').click(function (e) {
                if ($(e.target).is($(this))) {
                    $(this).hide();
                    //$('#DivTopScoreStudent2').hide();
                    $('#DivCoverTopScoreStudent2').hide();
                    $('.imgShowStudentStatusPage2').hide();
                    $('#DivCoverForActivityPage2').hide();
                    $('#ForActivityPage').hide();
                    $('#btnShowTopScore2').hide();
                    $('.imgShowPanelScore').hide();
                    $('#ShowDivUserReply').val('0');
                    $('.replyAnswer2').show(500);
                    $('.divMenuTop').css("height", "100px")
                    OffInterval();
                }
            });

            $('.divReplyAns').live('click', function () {
                $('#ForActivityPage').hide();
                $('.imgShowPanelScore').hide();
                //$('#ForActivityPage2').hide();
                $('#DivCoverForActivityPage2').hide();
                $('.imgShowPanelScore2').hide();
                $('#DivTopScoreStudent').hide();
                $('.imgShowStudentStatusPage').hide();
                //$('#DivTopScoreStudent2').hide();
                $('#DivCoverTopScoreStudent2').hide();
                $('.imgShowStudentStatusPage2').hide();
                $('.replyAnswer').show();
                $('.replyAnswer2').show();
                $('#ShowDivUserReply').val('0');
                $('.divMenuTop').css("height", "100px")
                OffInterval();
            });

            /////////////////////////////////////////////////////////////

            $('#btnShowTopScore').live('click', (function () {

                if (parseInt($('#hdTotalStudent').val()) >= 13) {
                    $('.divMenuTop').css("height", "265px");
                }
                else {
                    $('.divMenuTop').css("height", "220px");
                }
                //$('#ForActivityPage').hide(500);
                $('#DivTopScoreStudent').show(500);
                $('.imgShowStudentStatusPage').show();
                $('#ShowDivUserReply').val('2');
                GetDataScoreStudent(false);
                OffInterval();
                OnInterval();
            }));

            $('#btnShowTopScore2').live('click', (function () {
                //$('.divMenuTop').css("height","220px") 
                //$('#ForActivityPage').hide(500);
                //$('#DivTopScoreStudent2').show(500);
                $('#DivCoverTopScoreStudent2').show();
                $('#DivTopScoreStudent2').show();
                $('.imgShowStudentStatusPage2').show();
                $('#ShowDivUserReply').val('2');
                GetDataScoreStudent(true);
                OffInterval();
                OnInterval();
            }));

            $('#DivTopScoreStudent').click(function (e) {
                if ($(e.target).is($(this))) {
                    $(this).hide(500);
                    $('.imgShowStudentStatusPage').hide();
                    $('#ShowDivUserReply').val('1');
                    if (parseInt($('#hdTotalStudent').val()) >= 13) {
                        $('.divMenuTop').css("height", "170px")
                        $('#ForActivityPage').css('height', '70px');
                        $('#DivTopScoreStudent').css('height', '70px');
                        $('#ForActivityPage2').css('height', '70px');
                        $('#DivTopScoreStudent2').css('height', '70px');
                    }
                    else {
                        $('.divMenuTop').css("height", "150px")
                        $('#ForActivityPage').css('height', '50px');
                        $('#DivTopScoreStudent').css('height', '50px');
                        $('#ForActivityPage2').css('height', '50px');
                        $('#DivTopScoreStudent2').css('height', '50px');
                        //$('.imgShowStudentStatusPage').css('top', '95px')
                        $('.imgShowStudentStatusPage').css('top', '157px')
                    }
                    OffInterval();
                }
            });


            $('.DivShowTopScore').live('click', (function () {

                $('#DivTopScoreStudent').hide(500);
                $('.imgShowStudentStatusPage').hide();
                //$('#DivTopScoreStudent2').hide(500);
                $('#DivCoverTopScoreStudent2').hide();
                $('.imgShowStudentStatusPage2').hide();

                $('#ShowDivUserReply').val('1');
                if (parseInt($('#hdTotalStudent').val()) >= 13) {
                    $('.divMenuTop').css("height", "170px")
                    $('#ForActivityPage').css('height', '70px');
                    $('#DivTopScoreStudent').css('height', '70px');
                    $('#ForActivityPage2').css('height', '70px');
                    $('#DivTopScoreStudent2').css('height', '70px');
                }
                else {
                    $('.divMenuTop').css("height", "150px");
                    $('#ForActivityPage').css('height', '50px');
                    $('#DivTopScoreStudent').css('height', '50px');
                    $('#ForActivityPage2').css('height', '50px');
                    $('#DivTopScoreStudent2').css('height', '50px');
                    //$('.imgShowStudentStatusPage').css('top', '95px')
                    $('.imgShowStudentStatusPage').css('top', '157px');
                }
                OffInterval();
            }));

            $('#DivTopScoreStudent2').click(function (e) {
                if ($(e.target).is($(this))) {
                    $(this).hide(500);
                    $('.imgShowStudentStatusPage2').hide();
                    $('#DivTopScoreStudent').hide();
                    $('.imgShowStudentStatusPage').hide();
                    $('#ShowDivUserReply').val('1');
                    //$('.divMenuTop').css("height","150px") 
                    OffInterval();
                }
            });

            $('.DivShowDialogStudentDetail').live('click', (function () {
                OpenStudentDetailPage(CurrentQuizId, ExamNum);
            }));

            $('.imgShowStudentStatusPage').click(function () {
                OpenStudentDetailPage(CurrentQuizId, ExamNum);
            });

            $('.imgShowStudentStatusPage2').click(function () {
                OpenStudentDetailPage(CurrentQuizId, ExamNum);
            });

            $('.ShowStudentStatusPage').click(function () {
                OpenStudentDetailPage(CurrentQuizId, ExamNum);
            });

            function OpenStudentDetailPage(QuizId, InputExamNum) {
                if (isAndroid) {
                    $('#DivImgTopMenu').css('z-index', 'initial');
                    $('#DivViewReport1').css('z-index', 'initial');
                    $('#DivExit1').css('z-index', 'initial');
                    $.fancybox({
                        'autoScale': true,
                        'transitionIn': 'none',
                        'transitionOut': 'none',
                        'href': '<%=ResolveUrl("~")%>Activity/StudentStatusPage.aspx?QuizId=' + QuizId + '&CurrentExamNum=' + InputExamNum,
                        'type': 'iframe',
                        'width': 750,
                        'minHeight': 450,
                        'beforeClose': function () {
                            $('#DivImgTopMenu').css('z-index', '999999');
                            $('#DivViewReport1').css('z-index', '999999');
                            $('#DivExit1').css('z-index', '999999');
                        }
                    });
                }
                else {
                    $.fancybox({
                        'autoScale': true,
                        'transitionIn': 'none',
                        'transitionOut': 'none',
                        'href': '<%=ResolveUrl("~")%>Activity/StudentStatusPage.aspx?QuizId=' + QuizId + '&CurrentExamNum=' + InputExamNum,
                        'type': 'iframe',
                        'width': 850,
                        'minHeight': 600

                    });
                }
            }

            function GetTotalNumberStudent() {
                $.ajax({
                    type: "POST",
                    //url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/GetTotalStudentInQuiz",
                    url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetTotalStudentInQuiz",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        $('#hdTotalStudent').val(data.d);
                    },
                    error: function myfunction(request, status) {
                        //alert(status);                      
                    }
                });
            }

            //ไปเอาข้อมูล Div คะแนนของเด็ก
            function GetDataScoreStudent(IsDiv2) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/GetScoreStudent",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != 0) { // seesion(UserId) หายจะ return 0 - Sefety function
                            var StudentData = jQuery.parseJSON(data.d);
                            if (StudentData.length > 0) {
                                if (IsDiv2 == false) {
                                    $('#DivTopScoreStudent').text('');
                                    //$('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowDialogStudentDetail"><div class="DivSmallInTopScore">');
                                    $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" style="background-color:transparent;" ><div class="DivShowTopScore"><div style="background-image:url(../Images/Podium/LabelNumber.png) !important;background-size:cover !important;" class="DivSmallInTopScore StudentTopScore1"></div><div style="background-image:url(../Images/Podium/LabelScore.png) !important;background-size:cover !important;" class="DivSmallInTopScore"></div></div></div>');
                                }
                                else {
                                    $('#DivTopScoreStudent2').text('');
                                    //$('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowDialogStudentDetail"><div class="DivSmallInTopScore2">');
                                    $('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" style="background-color:transparent;" ><div class="DivShowTopScore"><div style="background-image:url(../Images/Podium/LabelNumber.png) !important;background-size:cover !important;" class="DivSmallInTopScore2 StudentTopScore1"></div><div style="background-image:url(../Images/Podium/LabelScore.png) !important;background-size:cover !important;" class="DivSmallInTopScore2"></div></div></div>');
                                }
                                for (var i = 0; i < StudentData.length; i++) {
                                    //ได้ที่ 1
                                    if (i == 0) {
                                        if (IsDiv2 == false) {
                                            if (StudentData[i].StudentScore == 0) {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                            else {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore StudentTopScore1">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                        }
                                        else {
                                            $('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore2 StudentTopScore1">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore2">' + StudentData[i].StudentScore + '</div></div></div>');
                                        }
                                    }
                                        //ได้ที่ 2
                                    else if (i == 1) {
                                        if (IsDiv2 == false) {
                                            if (StudentData[i].StudentScore == 0) {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                            else {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore StudentTopScore2">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                        }
                                        else {
                                            $('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore2 StudentTopScore2">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore2">' + StudentData[i].StudentScore + '</div></div></div>');
                                        }
                                    }
                                        //ได้ที่ 3
                                    else if (i == 2) {
                                        if (StudentData[i].StudentScore == 0) {
                                            if (true) {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                            else {
                                                $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore StudentTopScore3">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                            }
                                        }
                                        else {
                                            $('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore2 StudentTopScore3">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore2">' + StudentData[i].StudentScore + '</div></div></div>');
                                        }
                                    }
                                        //ที่เหลือ
                                    else {
                                        if (IsDiv2 == false) {
                                            $('#DivTopScoreStudent').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore">' + StudentData[i].StudentScore + '</div></div></div>');
                                        }
                                        else {
                                            $('#DivTopScoreStudent2').append('<div class="divShowTopScoreStudent DivCoverTopScore" ><div class="DivShowTopScore"><div class="DivSmallInTopScore2">' + StudentData[i].StudentNo + '</div><div class="DivSmallInTopScore2">' + StudentData[i].StudentScore + '</div></div></div>');
                                        }

                                    }
                                }
                            }
                        }
                    },
                    error: function (request, status) {
                        // alert("ต่อไม่ได้");     
                        //alert('ไม่ได้');
                    }
                });
            }

            //Set Interval ให้ Div คะแนนของเด็กขยับไปๆมาๆ
            var IntervalShowScore;
            function OnInterval() {
                var Flag = true;
                $('.DivShowTopScore').css('left', '0px');
                IntervalShowScore = setInterval(function () {
                    if (Flag == true) {
                        if ($.browser.msie) {
                            if ($.browser.version < 9.0) {
                                $('.DivShowTopScore').css({ 'left': '-40px' });
                            }
                        } else {
                            $('.DivShowTopScore').animate({
                                left: '-=40'
                            }, 1000)
                        }
                        Flag = false;
                    }
                    else {
                        if ($.browser.msie) {
                            if ($.browser.version < 9.0) {
                                $('.DivShowTopScore').css({ 'left': '0px' });
                            }
                        } else {
                            $('.DivShowTopScore').animate({
                                left: '+=40'
                            }, 1000)
                        }
                        Flag = true;
                    }

                }, 2000);
            }

            //ปิด Interval ที่ขยับ Div คะแนนเด็ก
            function OffInterval() {
                clearInterval(IntervalShowScore)
            }

            // show user don't reply
            var IsSelfPace = '<%= shareSelfPace %>';

            if (bool == "True") {
                getStudentDontReply();
            }
            function getStudentDontReply() {
                if (IsSelfPace == 'False') {
                    $.ajax({
                        type: "POST",
                        url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetStudentDontReply",
                        data: "{ ExamNum : '" + ExamNum + "'}",
                        async: false,
                        contentType: "application/json; charset=utf-8", dataType: "json",
                        success: function (data) {
                            var studentId = jQuery.parseJSON(data.d);
                            //$('#ForActivityPage').append('<div class="divReplyAns"><br/>' + studentId.stuId + '</div>');
                            if (studentId[0].stuId == 0) {
                                $('.sumDontReply').text('0');
                            }
                            else {
                                $('#ForActivityPage').append('<div class="divinfoReplyAns ShowTopScore" style="background-color:transparent;background-image:url(../Images/Podium/LabelWait.png);" ><br /></div>');
                                $('#ForActivityPage2').append('<div class="divReplyAns ShowTopScore" style="top:-12px;background-color:transparent;background-image:url(../Images/Podium/LabelWait.png);" ><br /></div>');
                                for (i = 0; i < studentId.length; i++) {
                                    $('#ForActivityPage').append('<div class="divReplyAns" style="background-color:#FFFFFF;" ><br/>' + studentId[i].stuId + '</div>');
                                    $('#ForActivityPage2').append('<div class="divReplyAns" ><br/>' + studentId[i].stuId + '</div>');
                                }
                                //sum no. user don't reply
                                var sumDontReply = $('#ForActivityPage').children('.divReplyAns').length;
                                $('.sumDontReply').text(sumDontReply);
                            }
                        },
                        error: function myfunction(request, status) {
                            //alert(status);                      
                        }
                    });
                }
            }
            // refresh สถานะการตอบของนักเรียนทุกๆ 5 วินาที
            if (bool == "True") {
                var refreshStatus = setInterval(function () {
                    $('.divReplyAns').remove();
                    $('.divinfoReplyAns').remove();
                    getStudentDontReply();
                    //},5000);        
                }, 500);
                $.ajaxSetup({ cache: false });
            }
            //จับเวลา
            var Timer;

            function countNeedTimer(second, minute, secBt, minBt, state) {

                var finishTimeMunite = minute; finishTimeSecond = second;
                var secCountDown = 60; secElapsedTime = 0;
                var minuteCountDown = parseInt(minute); minuteElapsedTime = 0;
                // เช็คว่าเป็นการปิดหน้าฟังชั่นแล้วเดินเวลาต่อ
                if (state == 1) {
                    secElapsedTime = secBt;
                    minuteElapsedTime = minBt;
                }


                if (parseInt(second) == 0) {
                    minuteCountDown = minuteCountDown - 1;
                }
                else {
                    secCountDown = parseInt(second);
                    nubSec = parseInt(second)
                }

                Timer = setInterval(function () {
                    // วินาทีถอยหลัง เดินหน้า
                    secCountDown = secCountDown - 1;
                    secElapsedTime = secElapsedTime + 1;

                    if (secCountDown.toString().length > 1 && secElapsedTime.toString().length == 1) {
                        $('.secCountDown').text(secCountDown);
                        $('.secElapsedTime').text('0' + secElapsedTime);
                    }
                    else if (secCountDown.toString().length > 1 && secElapsedTime.toString().length > 1) {
                        $('.secCountDown').text(secCountDown);
                        $('.secElapsedTime').text(secElapsedTime);
                    }
                    else if (secCountDown.toString().length == 1 && secElapsedTime.toString().length > 1) {
                        $('.secCountDown').text('0' + (secCountDown));
                        $('.secElapsedTime').text(secElapsedTime);
                    }
                    else if (secCountDown.toString().length == 1 && secElapsedTime.toString().length == 1) {
                        $('.secCountDown').text('0' + (secCountDown));
                        $('.secElapsedTime').text('0' + secElapsedTime);
                    }

                    $('.minuteCountDown').text(minuteCountDown);
                    $('.minuteElapsedTime').text(minuteElapsedTime);

                    //หยุดเวลาเมื่อครบ
                    //if(secElapsedTime == parseInt(second) && minuteElapsedTime == parseInt(minute)){
                    //alert(parseInt(secCountDown) + "    " + parseInt(minuteCountDown));
                    if (parseInt(secCountDown) == 0 && parseInt(minuteCountDown) == 0) {
                        clearInterval(Timer);
                        console.log('เมื่อหมดเวลา');
                        $('#btnNextTop').trigger('click');
                    }
                    //เช็คเมื่อเวลาลดลงไป 60 วินาทีให้เปลี่ยนเลขนาที
                    if (secCountDown == 0) {
                        $('.minuteCountDown').text(minuteCountDown--);
                        secCountDown = 60;
                    }
                    if (secElapsedTime == 60) {
                        $('.minuteElapsedTime').text(minuteElapsedTime + 1);
                        $('.secElapsedTime').text('00');
                        secElapsedTime = 0;
                        minuteElapsedTime = minuteElapsedTime + 1;
                    }

                }, 1000);
            }

            $('#tablet').click(function () {
                //window.parent.location.href = '../Activity/chkTabletConnect.aspx&iframe=true&width=80%&height=100%';
                clearInterval(Timer);
            });
            $('.ViewScore').click(function () {
                clearInterval(Timer);
            });

            // function จับเวลา
            //if($('#checkStratTimer').val() == 1){
            //    getNeedTimer();
            //}
        });

        function CloseHelpPanel() {
            $.fancybox.close();
        }

    </script>
    <%-- lightbox fancybox blockUI--%>
    <script type="text/javascript">
        var IsUseTablet = '<%=Session("QuizUseTablet") %>';
        var NeedShowTip = '<%=NeedShowTip%>';

        $(function () {
            //dialog LastQuestion
            $('#dialogIsLastQuestion').dialog({
                autoOpen: false,
                buttons: {
                    'ออกเลย': function () {
                        window.location = '<%=ResolveUrl("~")%>Activity/AlternativePage.aspx';
                    },
                    'ยังก่อน': function () {
                        $(this).dialog('close');
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            });
            if ($('.ui-button').length != 0) {
                $('.ui-button').each(function () {
                    new FastButton(this, TriggerServerButton);
                });
            }
            // startpage           

            if ($('#checkFirstPage').val() == 0 && IsUseTablet == "True") {
                CheckStatusTablet();
                //SetFirstStartPage();
            }
            else if ($('#checkFirstPage').val() == 0 && IsUseTablet == "False") {
                blackScreen();
            }
            // introduction (vdo,mp3)
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
            if (IsUseTablet == "True") {
                var t = setInterval(function () { GetTabletLost(); }, 1000);
            }
        });

        function GetTabletLost() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetTabletLost",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {
                        // ให้ div checktablet แสดงเลขที่นักเรียนที่หลุดไป
                        $('#TabletLost').show("pulsate", 1800).html(data.d);
                    } else {
                        $('#TabletLost').hide();
                    }
                },
                error: function myfunction(request, status) { }
            });
        }

        function SetFirstStartPage() {// set ค่าลง hidden เข้าครั้งแรกแล้ว           
            $('#checkStratTimer').val(1);
            $('#checkFirstPage').val(1);
        }
        function blackScreen() {// จอดำๆ ตอนโหลดเริ่มควิซ
            $.blockUI({
                css: {
                    border: 'none',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .8,
                    top: '0px',
                    left: '0px',
                    width: screen.width + 'px',
                    height: screen.height + 'px',
                    color: '#fff'

                },
                message: $('#uiCountDown')
            });

            var i = 10;
            var timeOpen = setInterval(function () {
                i--;
                $('#uiCountDown').html(i);
                if (i == 0) {
                    $.unblockUI();
                    clearInterval(timeOpen);
                    SetFirstStartPage();
                    UpdateQuizTimeStart();
                    getNeedTimer();
                    if (NeedShowTip == 'True') {
                        ShowTips();
                    }
                }
            }, 1000);

        }
        function CheckStatusTablet() { // check status tablet
            if (s_needTimer == "True") {
                KeepTime();
                if (IsTimePerQuestion == "False") {
                    CountTime();
                }
            }
            if (isAndroid) {
                $('#DivImgTopMenu').css('z-index', 'initial');
                $('#DivViewReport1').css('z-index', 'initial');
                $('#DivExit1').css('z-index', 'initial');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>activity/chkTabletConnect.aspx',
                    'type': 'iframe',
                    'width': 750,
                    'minHeight': 450,
                    'beforeClose': function () {
                        $('#DivImgTopMenu').css('z-index', '999999');
                        $('#DivViewReport1').css('z-index', '999999');
                        $('#DivExit1').css('z-index', '999999');
                        if ($('#checkStratTimer').val() == 0 && IsUseTablet == 'True') {
                            SetFirstStartPage();
                            UpdateQuizTimeStart();
                            getNeedTimer();
                        } else if ($('#checkStratTimer').val() == 1 && IsUseTablet == 'True') {
                            if (s_needTimer == "True") {
                                StopCountTime();
                                timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false);
                                timeNormal(arrKeepTime[2], arrKeepTime[3], true);
                            }
                        }
                    }
                });
            } else {
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>activity/chkTabletConnect.aspx',
                    'type': 'iframe',
                    'width': 900,
                    'minHeight': 600,
                    'beforeClose': function () {
                        //StartTimer();                        
                        if ($('#checkStratTimer').val() == 0 && IsUseTablet == 'True') {
                            SetFirstStartPage();
                            UpdateQuizTimeStart();
                            getNeedTimer();
                            if (NeedShowTip == 'True') {
                                ShowTips();
                            }
                        } else if ($('#checkStratTimer').val() == 1 && IsUseTablet == 'True') {
                            if (s_needTimer == "True") {
                                StopCountTime();
                                timeCountDown(arrKeepTime[0], arrKeepTime[1], true, false);
                                timeNormal(arrKeepTime[2], arrKeepTime[3], true);
                            }
                        }
                    }
                });
            }
        }
        function UpdateQuizTimeStart() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/UpdateQuizTimeStartOnReady",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) { },
                error: function myfunction(request, status) { }
            });
        }

        // function Start timer
        function StartTimer() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/StartTimer",
                //url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetTotalStudentInQuiz",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    //if (data.d == 1) {
                    //window.location = '<%=ResolveUrl("~")%>' + ThisPage + Querystring;
                    //}
                },
                error: function myfunction(request, status) {
                    //alert(status);                      
                }
            });
        }

        function CheckScore() { // check score
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>activity/ActivityReport.aspx?QuizId=' + CurrentQuizId + '&ReportMenu=1&ShowBtnBack=False',
                'type': 'iframe',
                'width': 900,
                'minHeight': 520
            });
        }
        function CloseDivSlide() {
            $('#slideDiv').css("display", "none");
        }

    </script>
    <script type="text/javascript">
        $(function () {

            var CorrectAnswer = '<%= CorrectAnswer %>';
            var MyAnswer = '<%= MyAnswer%>';

            $('#SwapCorrectAnswer').toggle(function () {
                $('#Answer').remove();
                $('#AnswerTbl').html(CorrectAnswer);
            }, function () {
                $('#Answer').remove();
                $('#AnswerTbl').html(MyAnswer);
            });

        });


    </script>
    <%--wordbook dictionary--%>
    <script type="text/javascript">
        $(function () {
            $('.wordsDict').live('click', function (e) {
                goSearch(e.target.innerHTML);
                $('span').removeClass('spanHilight');
                $(this).addClass('spanHilight');
            });

            //$('#mainQuestion').click(function () { if ($('#hdStatusDict').val() == "On") GetWordSeleter(); });
            //$('#mainAnswer').click(function () { if ($('#hdStatusDict').val() == "On") GetWordSeleter(); });

            var timeout = 0;
            $('#mainQuestion').mousedown(function () {
                timeout = setTimeout(GetWordSeleter, 500);
            }).bind('mouseup mouseleave', function () {
                clearTimeout(timeout);
            });

            $('#mainAnswer').mousedown(function () {
                timeout = setTimeout(GetWordSeleter, 500);
            }).bind('mouseup mouseleave', function () {
                clearTimeout(timeout);
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
                //alert(t);
                console.log(t);
                t = t.replace("?", "").replace(",", "").replace("!", "").replace(";", "");
                if (t.lastIndexOf(".", t.length - 1)) t = t.replace(".", "");
                goSearch(t);
            }
        }
        function goSearch(selection) {
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
                    //async: false,                              
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
                        // alert("ต่อไม่ได้");                      
                    }
                });
            } else {
                $('.ui-tooltip-tipped').remove();
            }
        }
        function getSelected() {
            if (window.getSelection) { return window.getSelection(); }
            else if (document.getSelection) { return document.getSelection(); }
            else {
                var selection = document.selection && document.selection.createRange();
                if (selection.text) { return selection.text; }
                return false;
            }
            return false;
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
        function playSound(soundfile) { //play mp3 dictionary/wordbook
            document.getElementById("voice").innerHTML =
        "<embed src=\"" + soundfile + "\" hidden=\"true\" autostart=\"true\" loop=\"false\" />";
        }
    </script>
    <%--คำถามแบบจับคู่ + เรียงลำดับ--%>
    <script type="text/javascript">
        $(function () {
            // จับคู่
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
                            url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/SaveAnswerPairQuestion",
                            data: "{ QuestionIdAll : '" + QuestionIdAll + "', AnswerIdAll : '" + AnswerIdAll + "'}",
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
            // เรียงลำดับ
            $('#sortable').sortable({
                placeholder: "ui-state-highlight",
                stop: function (event, ui) {
                    getIdLi();
                }
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
        function getIdLi() { // id เรียงลำดับ          
            var questionIdAll = "";
            $('#sortable').children().each(function () {
                questionIdAll += $(this).attr('id') + ",";
            });
            //return test;
            questionIdAll = questionIdAll.substring(0, questionIdAll.length - 1);
            //alert(questionIdAll);
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage.aspx/saveAnswerSortQuestion",
                data: "{  questionIdAll: '" + questionIdAll + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }
    </script>
    <%-- Timer --%>

    <script type="text/javascript">
        var typeTimer;

        function getNeedTimer() {
            if (!IsPracticePC) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetModeQuizAndTimer",
                    data: "{ _AnswerState : '" + AnswerState + "'}",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var needTimer = jQuery.parseJSON(data.d);
                        var minute; var second;

                        if (needTimer.NeedTimer == true) {
                            typeTimer = true;
                            $('.needTimer').show();
                            if (needTimer.timerType == true) {
                                minute = Math.floor(needTimer.AllTime / 60);
                                second = parseInt(needTimer.AllTime) % 60;
                                timeCountDown(minute, second, true, needTimer.noWatch);
                                timeNormal(0, 0, true);
                            }
                            else {

                                //                            if($('#hdKeepSec').val() == ""){                             
                                //                                minute = Math.floor(needTimer.AllTime/60);
                                //                                second = parseInt(needTimer.AllTime%60);
                                //                                $('#hdKeepMin').val(minute);
                                //                                $('#hdKeepSec').val(second);                                
                                //                                timeCountDown(minute,second,false);
                                //                                timeNormal(0,0,true);                                
                                //                            }
                                //                            else{  
                                //                                minute = $('#hdKeepMin').val();
                                //                                second = $('#hdKeepSec').val();                                
                                //                                timeCountDown(minute,second);
                                //                                minuteN = $('#hdKeepMinN').val();
                                //                                secondN = $('#hdKeepSecN').val();
                                //                                timeNormal(minuteN,secondN,true);                                
                                //                            }
                                minuteCountDown = Math.floor(needTimer.TimeRemain / 60);
                                secondCountDown = parseInt(needTimer.TimeRemain % 60);
                                timeCountDown(minuteCountDown, secondCountDown, false, needTimer.noWatch);

                                minute = Math.floor(needTimer.AllTime / 60);
                                second = parseInt(needTimer.AllTime % 60);
                                timeNormal(minute, second, true);
                            }
                        }
                        else {
                            typeTimer = false;
                            $('.NotTimer').show();
                            //                        if($('#hdKeepSecN').val() == ""){
                            //                            timeNormal(0,0,false);
                            //                        }
                            //                        else{
                            //                            minute = $('#hdKeepMinN').val();
                            //                            second = $('#hdKeepSecN').val();
                            //                            timeNormal(minute,second,false);
                            //                        }
                            minute = Math.floor(needTimer.AllTime / 60);
                            second = parseInt(needTimer.AllTime % 60);
                            timeNormal(minute, second, false);
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('jeng');
                    }
                });
            }
        }
        function timeNormal(minN, secondsN, type) {
            second_timeN = setInterval(function () {
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
                if (type) {
                    $('.secElapsedTime').text(secondsN);
                    $(".minuteElapsedTime").text(minN);
                }
                else {
                    $('.secElapsedNotTime').text(secondsN);
                    $(".minuteElapsedNotTime").text(minN);
                }
            }, 1000);
        }
        function timeCountDown(min, seconds, type, noWatch) {
            console.log('timeCountDown');
            console.log('timeCountDown - type : ' + type);
            console.log('timeCountDown - noWatch : ' + noWatch);
            second_time = setInterval(function () {
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
                            //alert(1);
                            $('#<%=btnNextTop.ClientID %>').trigger('click');
                        }
                        else {
                            //show dialog end quiz
                            if (!noWatch) {
                                var isShowAnswerComplete = '<%=Session("ShowCorrectAfterComplete")%>';
                                if (isShowAnswerComplete == 'True') {
                                    CallDialog("เฉลยเลย", "ยังก่อน", "ดูเฉลยเลยหรือจะทบทวนอีกสักรอบก่อนคะ", "", 1);
                                } else {
                                    $('#dialogIsLastQuestion').dialog('open');
                                }
                                //if ($('.ui-button').length != 0) {
                                //    $('.ui-button').each(function () {
                                //        new FastButton(this, TriggerServerButton);
                                //    });
                                //}
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

                $('.secCountDown').text(seconds);
                $(".minuteCountDown").text(min);

            }, 1000);
        }
    </script>
    <script type="text/javascript">
        $(function () {
            if (isAndroid) {
                //$('#divHead').css('margin-left', '-30px');
                //$('#divHead').css('width', '830px');
                //$('#divHead').css('position', 'relative');
                //$('#divHead').css('left', '50px');
                //$('#DivImgTopMenu').css('position', 'absulute');
                //$('#DivImgTopMenu').css('top', '18px');
                //$('#imgMenuTopSide').hide();
                //$('#DivViewReport1').css('margin-left', '-23px');
                //$('#DivExit1').css('margin-left', '-23px');
                //$('#DivImgTopMenu').css('z-index', '999999');
                //$('#DivViewReport1').css('z-index', '999999');
                //$('#DivExit1').css('z-index', '999999');
                //$('body').css('height', '100%');
                //$('#divQuestionAndAnswer').css('margin-left', '175px');
                //$('.divQuestionAndAnswer').css('margin-right', '175px');
                //$('.imgShowStudentStatusPage').css('left', '5%');                
            } else {
                $('#divHead').css('margin-left', 'auto');

            }
        });
    </script>
    <script type="text/javascript">
        /* css for IE */
        $(function () {
            if ($.browser.msie) {
                $('#minuteElapsedTime').css({ 'left': '12px', 'position': 'absolute', 'top': '40px' });
                $('#delimiterElapsedTime').css({ 'left': '28px', 'position': 'absolute', 'top': '40px' });
                $('#secElapsedTime').css({ 'left': '35px', 'position': 'absolute', 'top': '40px' });
                $('#minuteCountDown').css({ 'left': '12px', 'position': 'absolute', 'top': '0px' });
                $('#delimiterCountDown').css({ 'left': '28px', 'position': 'absolute', 'top': '0px' });
                $('#secCountDown').css({ 'left': '35px', 'position': 'absolute', 'top': '0px' });
            }
        });
    </script>
    <style type="text/css">
        div.MainReply {
            position: absolute;
            width: 80px;
            height: 80px;
            left: -150px;
        }

        div.ReplyMode {
            width: 80px;
            height: 80px;
            position: relative;
            cursor: pointer;
            font-size: 33px;
            text-align: center;
            border-radius: 10px;
            line-height: 80px;
            font-weight: bolder;
            z-index: 999;
            margin-left: 90px;
        }

        div.ManualReply {
            width: 63px;
            height: 35px;
            position: relative;
            background-color: rgb(27, 194, 54);
            margin-left: auto;
            margin-right: auto;
            cursor: pointer;
            display: none;
            border-radius: 0 0 10px 10px;
            font-size: 15px;
            font-weight: bold;
            text-align: center;
            color: white;
            margin-left: 100px;
        }
    </style>
    <style type="text/css">
        /*--Menu image button*/
        div.btnExit {
            /*background-image: url("../images/activity/btnExitQuizMenu.png");*/
            /*height: 49px;
            width: 109px;*/
            cursor: pointer;
            margin-bottom: 10px;
            display: none;
        }

        div.btnNote {
            background-image: url("../images/activity/btnNoteMenu.png");
            height: 49px;
            width: 109px;
            background-repeat: no-repeat;
            display: none;
            border: 0 !important;
            background-color: initial !important;
            background-position: initial !important;
        }

        .btnCancleExit {
            background-image: url("../images/activity/btnExitCancleDialog.png") !important;
            border: 0 !important;
            background-color: initial !important;
            height: 90px;
            background-repeat: no-repeat !important;
            margin: 0;
        }

        .btnConfirmExit {
            background-image: url("../images/activity/btnExitQuixDialog.png") !important;
            border: 0 !important;
            background-color: initial !important;
            height: 90px;
            background-repeat: no-repeat !important;
            margin: 0;
        }
        .btnReplyDialog{
            background-image: url("../images/activity/btnReplyDialog.png") !important;
            border: 0 !important;
            background-color: initial !important;
            height: 44px;
            background-repeat: no-repeat !important;            
        }
    </style>
    <script type="text/javascript">
        //var NeedShowTip = '<%=NeedShowTip%>';
        //$(function () {
        //    if (NeedShowTip == 'True') {
        //        ShowTips();
        //    }
        //});
        function ShowTips() {
            var elm = ['#imgMenuTopSide', '#imgSetting1', '#DivCheckStatustablet', '.replyAnswer', '#Help', '#divMenuTopNextPre'];
            var tipPosition = ['leftMiddle', 'topLeft', 'topMiddle', 'bottomMiddle', 'leftMiddle', 'topMiddle'];
            var tipTarget = ['middleRight', 'bottomRight', 'bottomMiddle', 'bottomMiddle', 'rightMiddle', 'bottomMiddle'];
            var tipContent = ['สลับเมนูไม่ให้ล้นจอ', 'ดูเมนูอื่นๆ', 'เช็คสถานะแท็บเล็ตนักเรียน', 'ดูนักเรียนที่ยังไม่ตอบ', 'ศึกษาเพิ่มเติมดูที่นี่ค่ะ', 'เปลี่ยนข้อ'];
            var tipAjust = [-15, -20, 10, 0, -70, 5];
            var w = [100, 100, 100, 100, 200, 100];
            var y = [40, -20, -20, -20, 0, -20];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 0, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '17px', 'line-height': '2em'
                    }, hide: false,
                    position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { x: tipAjust[i], y: y[i] } },
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

<body id="PageBody" runat="server" style='background: url(../Images/Activity/bg5.png) no-repeat center center fixed; height: 100%;background-repeat:no-repeat;background-size:cover;'>
    <div class='PauseTime'></div>
    <div id="backOnload" style="background-color: Black; opacity: 0.9; width: 100%; height: 100%; z-index: 99999; position: absolute;"></div>
    <span id="voice"></span>

    <form id="form1" runat="server">

        <input type="hidden" id='HDLastChoice' runat="server" value="false" />
        <input type="hidden" id='HDIsLeapChoice' runat="server" value='False' />
        <input type="hidden" id='HDIsScore' runat="server" />
        <input type="hidden" id='CheckIsHaveLeapChoice' runat="server" />
        <input type="hidden" id='HDQQ_No' runat="server" />
        <input type="hidden" id='HDLeapChoicePage' runat="server" />
        <input type="hidden" id='HDNotReplyMode' runat="server" value="false" />
        <input type="hidden" id="CallStudentFirst" runat="server" value="0" />
        <input type="hidden" id='hdTotalStudent' />
        <input type="hidden" id="hdQuestionId" runat="server" />
        <input type="hidden" id="hdUserId" runat="server" />
        <input type="hidden" id='HDCheckChangeQuestion' runat="server" />
        <input type="hidden" id='hdCmdOldSession' runat="server" />
        <input type="hidden" id="hdStatusDict" runat="server" value="Off" />
        <input type="hidden" id="hdIsGroupEng" runat="server" />
        <input type="hidden" id="hdIsSender" runat="server" value="false" title="เก็บค่าตอนว่าเป็นคนกดเปลียนข้อหรือเปล่า" />
        <asp:HiddenField ID="ShowBtnOnTapMenu" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="ShowMenuOnPosTop" runat="server" Value="0" />
        <asp:HiddenField ID="ShowDivUserReply" runat="server" Value="0" />
        <asp:HiddenField ID="minuteTime" runat="server" Value="0" />
        <asp:HiddenField ID="secTime" runat="server" Value="0" />
        <asp:HiddenField ID="checkStratTimer" runat="server" Value="0" />
        <asp:HiddenField ID="checkFirstPage" runat="server" Value="0" />    
        <asp:HiddenField ID="hdReplyMode" runat="server" ClientIDMode="Static" />

        <div id="uiCountDown" style="display: none; padding: 200px; margin-left: auto; margin-right: auto; line-height: 5em; font: normal 150px THSarabunNew;"></div>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div id="divHead" style='width: 970px; /*margin-left: auto; margin-left: -30px; */ margin-right: auto;position:relative;'>
            <div id='DivMenuTops' class="divMenuTop" style='background-image: url(../Images/Activity/BG_Opacity/whLayer40pct2.png);'>
                <div class="needTimer" style="display: none; width: 50px; background-color: transparent; font-weight: bold; font-size: 17px;">
                    <img src="../Images/Activity/hourglass.gif" style="position: relative; top: -3px; left: -8px;" />
                    <span id="minuteCountDown" class="minuteCountDown" style="position: relative; left: -4px; top: -78px;"></span>
                    <span id="delimiterCountDown" style="position: relative; left: -4px; top: -79px">:</span>
                    <span id="secCountDown" class="secCountDown" style="position: relative; left: 26px; top: -118px;"></span>
                    <br />
                    <span id="minuteElapsedTime" class="minuteElapsedTime" style="position: relative; left: -4px; top: -126px;"></span>
                    <span id="delimiterElapsedTime" style="position: relative; left: -3px; top: -128px;">: </span>
                    <span id="secElapsedTime" class="secElapsedTime" style="position: relative; left: 23px; top: -166px;"></span>
                </div>
                <div class="NotTimer" style="display: none; width: 80px; background-color: transparent;">
                    <img src="../Images/Activity/clock-small.png"  alt="" />
                    <div style="position: absolute;width: inherit;height: 30px;float: initial;margin: 0;text-align: center;padding: 0;top: 26px;background-color: initial;font: normal 0.95em 'THSarabunNew';font-weight: bold;font-size: 17px;">
                        <span id="Span5" class="minuteElapsedNotTime" ></span><span >:</span><span id="Span6" class="secElapsedNotTime"></span>
                    </div>                    
                </div>
                <div id="DivTestsetName" style="width: <%=SizeWidthForDivs %>px; background-color: #256ef4; font: bold 20px 'THSarabunNew'; line-height: 25px; word-wrap: break-word; overflow: hidden;">
                    <br />
                    <asp:Label ID="lblTestsetName" runat="server" Style='color: White;' Text="โหมดฝึกฝน วิชา"></asp:Label>
                </div>
                <div id="divMenuTopNextPre" class="menuTopSideNextPre" style='width: <%=WidthDivExamAmount %>px; background-color: #06a2dc; cursor: inherit;'>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 57px; height: 69px;">
                                <asp:ImageButton ID="btnPrvTop" runat="server" src="../images/Activity/pre-icon50.png" class="BtnNextPreKeepTime" />
                            </td>
                            <td style="width: 131px; height: 69px; font: normal 18px 'THSarabunNew';">
                                <asp:Label runat="server" Style='color: White;' ID="lblNoExam" CssClass="lblQuizName"></asp:Label>
                            </td>
                            <td style="width: 57px; height: 69px;">
                                <asp:ImageButton ID="btnNextTop" runat="server" src="../images/Activity/next-icon50.png" class="BtnNextPreKeepTime" />
                            </td>
                        </tr>
                    </table>
                </div>
                
                <%Try%>
                    <%If Session("QuizUseTablet").ToString() = "True" Then%>
                        <% If VBIsSelfPace = False Then%>
                            <div style="width: 100px; background-color: #fec500; background-image: url('../images/Activity/zoomin.png'); background-repeat: no-repeat; 
                                background-position: bottom right; background-size: 25px; font: normal 20px 'THSarabunNew';" class="replyAnswer">
                                <span style="font-size: 15px; line-height: 2;">ยังไม่ตอบ</span><br />
                                <span class="sumDontReply" style="font-size: 50px; font-weight: bolder; line-height: 0.7em">0 </span>
                            </div>
                        <%Else%>
                            <div style="width: 100px; background-color: #fec500; background-image: url('../images/Activity/zoomin.png'); background-repeat: no-repeat; 
                                background-position: bottom right; background-size: 25px; font: normal 20px 'THSarabunNew';" class="ShowStudentStatusPage">
                                <img src='../Images/group3.png' style='width: 85px;' />
                            </div>
                        <% End If%>

                        <div id="DivCheckStatustablet" style="line-height: 2; background-color: transparent; width: 61px;">
                            <img src="../Images/Activity/CheckTab.png" id="imgCheckStatusTablet" alt="" />
                            <div id="TabletLost" style="display: none;top: -85px;position: relative;width: inherit;height: inherit;left: -10px;font-size: 45px;
                                line-height: 80px;background: #F4F7FF;font-weight: bold;color: orangered;">
                            </div>
                        </div>
                        <div id='ForActivityPage' style='position: relative; width: 724px; height: 50px; background: #FEC500; -webkit-border-radius: 7px; top: -13px; left: 0px; 
                            overflow-x: auto; overflow-y: hidden; white-space: nowrap; text-align: left;'>
                            <span style="position: absolute; right: 5px; bottom: 0px; color: #FFEAA3;"></span>
                        </div>
                        <div id='DivTopScoreStudent' style='display: none; position: relative; width: 724px; height: 50px; background: #12EC3E; no-repeat right top;
                            -webkit-border-radius: 7px; top: -13px; left: 0px; overflow-x: auto; overflow-y: hidden; white-space: nowrap; text-align: left;'>
                        </div>
                        <img src='../Images/Podium/NextMode-Score.png' id='btnShowTopScore' class='imgShowPanelScore' style="top:80px;" />
                        <img src='../Images/Podium/NextMode-Podium.png' class='imgShowStudentStatusPage' style="top:190px;" />
                    <% End If%>
                
                    <%If PracticeFromComputer = True Then%>
                        <div id="BtnSwapQuestion" class="RightDiv" style="height: 75px; margin-top: 10px;background-color: #06a2dc;width: 73px; display:none"></div>
                    <% End If%>
                </div>
            <div id='DivImgTopMenu' style='width: 80px; position: relative; float: right; top: -101px; left: 15px; margin-top: -3px;z-index:999;'>
                    <img id="imgMenuTopSide" style='width: 70px; height: 52px; cursor: pointer; margin-top: 4px;' class="imgToggle" src="../Images/Activity/rotate.png" />
                    <img id='imgSetting1' class='divSetting' style='width: 63px; height: 50px;' src="../Images/Activity/features-icon5555.png" />

                    <%If Session("QuizUseTablet").ToString() = "True" Then%>
                        <div id='DivViewReport1' class='ForSetting' style='display: none; margin-top: 5px; margin-left: -38px; cursor: pointer; line-height: 42px;'>
                        <a style='text-decoration: none; color: #FDA200;' title='ดูคะแนน' onclick="CheckScore();">
                            <span class="ViewScore">ดูคะแนน</span>
                        </a>
                    </div>
                    <%End If%>
       
                    <div class="ForToolsTop btnExit" id="DivExit1">
                    <a><span>จบกิจกรรม</span></a>
                </div>

                    <% If tools_Calculator = True Then%>
                        <div class="ForToolsTop btnCalculator" id="btnCalculatorTop" style='margin-top: 3px; margin-left: -38px;'>
                        <a><span>เครื่องคิดเลข</span></a>
                    </div>
                    <% End If%>
                    <% If tools_Dictionary = True Then%>
                        <div class="ForToolsTop btnDictionary DictOff" style='margin-top: 3px; margin-left: -38px;'>
                        <a><span>แปลศัพท์</span></a>
                    </div>
                    <% End If%>
                    <% If tools_WordBook = True Then%>
                        <div class="ForToolsTop btnWordBook" id="btnWordBookTop" style='margin-top: 3px; margin-left: -38px;'>
                        <a><span>สมุดคำศัพท์</span></a>
                    </div>
                    <% End If%>
                    <% If tools_Note = True Then%>
                        <div class="ForToolsTop btnNote" style='margin-top: 5px; margin-left: -50px;'>
                        <a><span>กระดาษโน๊ต</span></a>
                    </div>
                    <% End If%>
                    <% If tools_Protractor = True Then%>
                        <div class="ForToolsTop btnProtractor" style='margin-top: 3px; margin-left: -38px;'>
                        <a><span>ไม้โปรแทรกเตอร์</span></a>
                    </div>
                    <% End If%>
                </div>
        </div>

        <div id='DivSideMenus' class="divMenuSide" style='background-image: url(../Images/Activity/BG_Opacity/whLayer40pct2.png);'>
            <div class="needTimer" style="display: none; height: 85px; background-color: transparent;">
                <img src="../Images/Activity/hourglass.gif" />
                <div style="position: relative; background-color: transparent; top: -85px; left: 6px;">
                    <span id="Span1" class="minuteCountDown"></span>
                    <span style="margin-left: -2px; margin-right: -1px;">:</span>
                    <span id="Span2" class="secCountDown"></span>
                </div>
                <div style="position: relative; background-color: transparent; top: -102px; left: 5px;">
                    <span id="Span3" class="minuteElapsedTime"></span>
                    <span style="margin-left: 0px;">:</span>
                    <span id="Span4"class="secElapsedTime" style="margin-left: -1px;"></span>
                </div>
            </div>
            <div class="NotTimer" style="display: none; height: 75px; background-color: transparent;">
                <img src="../Images/Activity/clock-small.png" />
                <div style="position: absolute;width: inherit;height: 30px;float: initial;margin: 0;text-align: center;padding: 0;top: 26px;background-color: initial;
                    font: normal 0.95em 'THSarabunNew';font-weight: bold;width:80px;left:20px;">
                    <span id="Span7" class="minuteElapsedNotTime"></span><span>:</span><span id="Span8" class="secElapsedNotTime"></span>
                </div>               
            </div>
            <div id="divSideTestsetName" style="height: <%= SideMenuDiv%>px; background-color: #256ef4; font: bold 20px 'THSarabunNew'; word-wrap: break-word; overflow: hidden;">
                <asp:Label ID="lblSideText" CssClass='forspan' Style='position: static; color: White;' runat="server" Text="โหมดฝึกฝน วิชา"></asp:Label>
            </div>
            <div id="divMenuSideNextPre" class="menuTopSideNextPre" style='height: 110px; cursor: inherit;background-color:#06a2dc;'>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <asp:ImageButton ID="btnPrvSide" runat="server" src="../images/Activity/pre-icon50.png" class="BtnNextPreKeepTime" Style="margin-top: 8px;" />
                        </td>
                        <td style="width: 50%;">
                            <asp:ImageButton ID="btnNextSide" runat="server" src="../images/Activity/next-icon50.png" class="BtnNextPreKeepTime" Style="margin-top: 8px; 
                                margin-right: 3px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; font: normal 20px 'THSarabunNew'; text-align: center;color:white;" colspan="2">
                            <asp:Label runat="server" ID="lblNoExamSide" CssClass="lblQuizName"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>

            <%If Session("QuizUseTablet").ToString() = "True" Then%>
                <% If VBIsSelfPace = False Then%>
                    <div style='height: 80px; background-image: url("../images/activity/zoomin.png"); background-color: #fec500; background-repeat: no-repeat; 
                        background-position: bottom right; font: normal 20px "THSarabunNew";' class="replyAnswer2">
                            <span style="font-size: 15px; line-height: 2;">ยังไม่ตอบ</span><br />
                            <span class="sumDontReply" style="font-size: 50px; font-weight: bolder; line-height: 0.7em">0 </span>
                    </div>
                <%Else%>
                    <div style='height: 80px; background-image: url("../images/activity/zoomin.png"); background-color: #fec500; background-repeat: no-repeat; 
                        background-position: bottom right; font: normal 20px "THSarabunNew";' class="ShowStudentStatusPage">
                            <img src='../Images/group3.png' style='width: 85px;' />
                    </div>
                <% End If%>
            <% End If%>
            
        </div>
        <div id='DivimgSideMenu' style='width: 142px; position: absolute; display: none; left: 0px;'>
            <img class='imgToggle' style='width: 70px; float: left; cursor: pointer;' src="../Images/Activity/rotate.png" />
            <img id='imgSetting2' class='divSettingTop' style='width: 70px; height: 65px; cursor: pointer;'
                src="../Images/Activity/features-icon5555.png" />
            <%If Session("QuizUseTablet").ToString() = "True" Then%>
            <div id='DivViewReport2' class='ForSetting' style='z-index: 999; display: none; margin-top: -64px; margin-left: 146px; cursor: pointer; line-height: 42px;'>
                <a style='text-decoration: none; color: #FDA200;' title='ดูคะแนน' onclick="CheckScore();">
                    <span class="ViewScore">ดูคะแนน</span></a>
            </div>
            <%End If%>
          <%--  <div id='DivExit2' class='btnExit' style='z-index: 999; position: relative; margin-top: -60px; margin-left: 150px;'>
                <%--<a style='text-decoration: none; color: #FDA200;' title="จบกิจกรรม"><span>จบกิจกรรม</span></a>--%>
           <%-- </div>--%>
                <div class="ForToolsTop btnExit" id="DivExit2">
                    <a><span>จบกิจกรรม</span></a>
                </div>
            <% If tools_Calculator = True Then%>
            <div class="ForToolsLeft btnCalculator" style='margin-top: 3px; margin-left: 146px;'>
                <a><span>เครื่องคิดเลข</span></a>
            </div>
            <% End If%>
            <% If tools_Dictionary = True Then%>
            <div class="ForToolsLeft btnDictionary DictOff" id="btnWordBookSide" style='margin-top: 3px; margin-left: 146px;'>
                <a><span>แปลศัพท์</span></a>
            </div>
            <% End If%>
            <% If tools_WordBook = True Then%>
            <div class="ForToolsLeft btnWordBook" style='margin-top: 3px; margin-left: 146px;'>
                <a><span>สมุดคำศัพท์</span></a>
            </div>
            <% End If%>
            <% If tools_Note = True Then%>
            <div class="ForToolsLeft btnNote" style='margin-top: 5px; margin-left: 150px;'>
                <%--<a><span>กระดาษโน๊ต</span></a>--%>
            </div>
            <% End If%>
            <% If tools_Protractor = True Then%>
            <div class="ForToolsLeft btnProtractor" style='margin-top: 3px; margin-left: 146px;'>
                <a><span>ไม้โปรแทรกเตอร์</span></a>
            </div>
            <% End If%>
        </div>
        <%If Session("QuizUseTablet").ToString() = "True" Then%>
            <div id='DivCoverForActivityPage2' style='width: 710px; margin-left: auto; margin-right: auto;'>
            <div id='ForActivityPage2' style='position: relative; width: 700px; height: 50px; background: #FEC500; margin-left: auto; margin-right: auto; top: 10px; -webkit-border-radius: 1em; cursor: pointer; white-space: nowrap; overflow-x: auto; overflow-y: hidden;'>
                <span style="position: absolute; right: 5px; bottom: 0px; color: #FFEAA3;"></span>
            </div>
            <img src='../Images/Podium/NextMode-Score.png' id='btnShowTopScore2' class='imgShowPanelScore2' />
        </div>
            <div id='DivCoverTopScoreStudent2' style='display: none; width: 710px; margin-left: auto; margin-right: auto;'>
            <div id='DivTopScoreStudent2' style='position: relative; width: 700px; height: 50px; background: #12EC3E; margin-left: auto; margin-right: auto; top: 15px; -webkit-border-radius: 1em; cursor: pointer; white-space: nowrap; overflow-x: auto; overflow-y: hidden;'>
            </div>
            <img src='../Images/Podium/NextMode-Podium.png' class='imgShowStudentStatusPage2' />
        </div>
        <%End If%>
        <% Catch%>
            <%Response.Redirect("~/LoginPage.aspx", False)%>
        <% End Try%>
        
        <%If PracticeFromComputer = True Then%>           
            <div id='slideDiv' style='width: 780px; height: 425px; position: absolute; background-color: #FFCFFF; 
                        margin-left: 400px; margin-right: auto; display: none; margin-top: 15px; z-index:999; border-radius: 0.5em;border: 1px outset;'>
        <div id='CloseDivSlide' style="width: 36px; height: 36px; background: url('../Images/Activity/activitypagepad2.png') -40px -280px; float: right; background-repeat: no-repeat; margin-right: -18px; margin-top: -12px;"
                    onclick="CloseDivSlide();">
        </div>
        <div id='LeapChoiceDiv' class='slides_container' style='height: 310px;'>
            <img class='imgPreNext' id="BackSlide" swap='1' src="../Images/Activity/AllNewArrow/rightBlue.png"
                        style='position: absolute; right: 1%; top: 35%; cursor: pointer; z-index: 99;'
                onclick='SwipeCartoon.next();return false;' runat="server" />
            <img class='imgPreNext' id="NextSlide"  swap='1' src="../Images/Activity/AllNewArrow/leftBlue.png"
                        style='position: absolute; left: 1%; top: 35%; cursor: pointer; z-index: 99;'
                        onclick='SwipeCartoon.prev();return false;' runat="server" />
           
            <div id="slider3" class="divReviewSwipe" style="height:75%;">
                <div>
                    <div>
                        <div id='SwipeCartoon' style='width: 900px; margin-left: auto; margin-right: auto; height: 70%;'>
                            <div id='mainReview'  runat="server">
                                <%--div สร้าง ส่วนรีวิวข้อสอบ style="display:block"--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

                <div id='DivSortBtn' style='margin-top: 40px; height: 79px; width: 710px; margin-left:43px;position: absolute;'>
                    <input type="button" class='ForSortBtn' id='BtnSortAnswerNull' value='ทวนข้อข้าม'
                        style='margin-left: 17px; width: 175px; height: 45px; font-size: 20px;' />
                    <input type="button" id='BtnSortNormal' class='ForSortBtn' value='เรียงตามลำดับ'
                        style='margin-left: 16px; width: 175px; height: 45px; font-size: 20px;' />
                    <input type="button" class='ForSortBtn' id='BtnNextToLastChoice' value='ทำต่อข้อล่าสุด'
                        style='margin-left: 87px; width: 200px; height: 45px; font-size: 20px;' />
                </div>
            </div>                   
            <div id='DivNotHaveDontReplyChoice' runat="server" style='width: 760px; z-index: 9999; margin-left: 25.5%; height: 204px; position: absolute; margin-top: 28px; background-color: skyBlue; line-height: 200px; border-radius: 7px; display: none;'>
                <div style="text-align: center;font-size: 40px; background-color: white; height: 180px; margin-top: 10px; width: 735px; margin-left: 10px; border: dashed 2px; border-radius: 5px;">
                    ไม่มีข้อข้ามแล้ว ทำข้อที่ยังไม่ทำต่อเลยค่ะ
                </div>
                <div id='CloseDivNothave' style="width: 36px; height: 36px; background: url('../Images/Activity/activitypagepad2.png') -40px -280px; float: right; margin-right: -12px;"
                    onclick="CloseDivnot();">
                </div>
            </div>            
        <%End If%>
        <div id ="divQuestionAndAnswer" class="divQuestionAndAnswer" style=" -webkit-border-radius: 0.5em; margin-left: auto; margin-right: auto;
                        margin-bottom:20px; width:780px; padding: 20px;">   
                
             <% If _AnswerState = "2" Then%>
    <div class="MainReply"><div class="ReplyMode" id="btnReplyMode" "></div><div class="ManualReply">เปิดเฉลย</div></div>
            
    <% End If%>     
            <%  If EnableIntro = True Then%>
            <div id="DivIntro">
                <div id="ViewLimitAmount" runat="server">
                </div>
                <div id="mainIntro" runat="server">
                    <span id="spnTest">Clickkkkkkkkkkkkkkkkkkkk</span>
                </div>
                <%--<div id="mainIntroAudio">
                <video controls preload="auto" width="100%" height="50" id="myvideojs" class="video-js vjs-default-skin"
                    data-setup='{"example_option":true}'><source src="../multimedia/audio/maid with the flaxen hair.mp3" type="audio/mp3">                       
                        </video>
            </div>--%>
            </div>
            <%End If%>

            <div id="btnQuestionExp"></div>
            <div id="mainQuestion" runat="server" class="Question"></div>
            <div id="qtipp" style="position: relative; width: 650px;"></div>
            <div id="mainAnswer" runat="server">
                <% If IsNoAnswer = True Then%>
                    <img id="NotAnswer" src="../Images/Activity/skip.png" class="ImgSkip" />
                <%End If%>
                <div id="btnAnswerExp"></div>
                    <table id="Table1" runat="server" style="width: 650px; border-collapse: collapse;">
                        <tr>
                            <td runat="server" id="AnswerTbl"></td>
                        </tr>
                    </table>
                    <div id="AnswerExp" runat="server" clientidmode="Static" style="font-size:24px;display:none;"></div>
                </div>
            <div id="divNextPre" style="margin-left: 13px;"> 
                <table style="width: 100%;">
                    <tr>

                        <td style="width: 371px; float: left; height: 65px; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #FFC200;">
                            <asp:ImageButton Height="50px" Width="50px" ID="btnPrevious" runat="server" src="../images/Activity/forward-icon.png"
                                class="BtnNextPreKeepTime" />
                            <asp:Image ID="ImageButton3" Class="hideDivNextPre" runat="server" src="../images/Activity/Home.png" Width="40" />

                            <asp:Image ID="imgRun" class="hideDivNextPre" runat="server" src="../images/Activity/run.gif" Height="51px"
                                Width="48px" CssClass="positionImg" />
                        </td>

                        <td class="hideDivNextPre" style="width: 371px; text-align: right; height: 65px; float: right; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: #FFC200;">
                            <asp:Image ID="ImageButton4" Class="hideDivNextPre" runat="server" src="../images/Activity/Flag.png" />
                            <asp:ImageButton Height="50px" Width="50px" ID="btnNext" runat="server" src="../images/Activity/next-icon.png"
                                class="BtnNextPreKeepTime" />
                        </td>

                    </tr>
                    <tr>
                            <td style="width: 746px; height: 69px; font: normal 18px 'THSarabunNew';">
                                <asp:Label runat="server" Style='color: White;' ID="lblRunNoExam" CssClass="lblQuizName"></asp:Label>
                            </td>
                    </tr>
                </table>              
            </div>
        </div>      
        <div id="prettyHM" style="background-color: Black; width: 100%; height: 100%; position: fixed; top: 0; left: 0; z-index: 99; opacity: 0.7;"
            class="prettyIntro">
        </div>
        <div id="mainPretty" class="prettyIntro">
            <div class="clear"></div>
            <div id="divClose">X ปิด</div>
            <div id="introHtml" style="" runat="server"></div>
            <div class="clear"></div>
        </div>
        <div id="LinkDialog" title="<%= DialogTitle %>"></div>
        <div class="divIndex">
            กระทรวงศึกษาธิการ<br />
            KPA<br />
            PISA
        </div>
        <div id="dialogIsLastQuestion" title="หมดเวลาทำควิซแล้วค่ะ จบควิซเลยไหมคะ ?"></div>
        <% If tools_Calculator = True Then%>
            <div id="tools_calculator" style="    z-index: 9999;">
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
        <% If tools_Note = True Then%>
            <script type="text/javascript">
            $(function () {
                if ($.browser.msie) {
                    $('.noteHeadTab').click(function () {
                        if ($(this).is('#tab1')) {
                            $(this).children('label').css('background', 'rgb(189, 238, 116)');
                            $('#myNote').css('z-index', '1').prev('label').css('background', '#eee');;
                            $('#myClipboard').css('z-index', '2');
                        } else {
                            $(this).children('label').css('background', 'rgb(132, 246, 255)');
                            $('#myClipboard').css('z-index', '1').prev('label').css('background', '#eee');
                            $('#myNote').css('z-index', '2');
                        }
                    });
                }
            });
        </script>
            <div id="tools_note">
            <div id="noteMain">
                <div class="noteTabs">
                    <div class='noteHeadTab' id="tab1">
                        <input type='radio' id='tab-1' name='tab-group' checked='checked' />
                        <label for='tab-1'>
                            กระดาษทด</label><div class='content' id='myClipboard'>
                                <textarea></textarea>
                            </div>
                    </div>
                    <div class='noteHeadTab' id="tab2">
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
                L
            </div>
            <div id="btnRotateR" class="btnRotate">
                R
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

        <div id="dialog" title=""></div>
        <ul id="Help" runat="server" clientidmode="Static">
            <li class="about2" style="z-index: 99;background:none;border:none;padding-left:0px;">
                <a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ" id="HelpLogin">
                    <div style="margin-top:7px;"><span style="font-size:initial;position:relative;left:10px;">ช่วย</span></div>
                <div><span style="font-size:initial;position:relative;left:8px;">เหลือ</span></div>
                </a>
            </li>
        </ul>
        
        <script type="text/javascript">
            var slider3 = new Swipe(document.getElementById('slider3'));
            var SwipeCartoon = new Swipe(document.getElementById('SwipeCartoon'));
        </script>
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
    </form>
    <script type="text/javascript">
        var qsetPath = '<%=qsetFilePath%>';
        console.log(qsetPath);
        $(function () {
            $('.imgSound').each(function () {
                var fileName = $(this).attr('alt');
                alert(qsetPath + fileName);
                $(this).after($('<audio>', { src: qsetPath + fileName, controls: '' }));
                $(this).remove();
            });
        });
    </script>
</body>
</html>
<%--<script type="text/javascript">
        //script ดักอีกที เมื่อโหลดเพจเสร็จ
        var CurrentPage;
        $(document).ready(function () {  
            var PracticeFromComputer = "<%= PracticeFromComputer %>";
            if (PracticeFromComputer == "False"){
                var ThisPage = window.location.pathname;
                ThisPage = ThisPage.toLowerCase();          
                getCurrentPage();
                if (CurrentPage != ThisPage) {
                    withOutClick = false;
                    window.location = CurrentPage;
                }
            }
        });
        function  getCurrentPage() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/getCurrentPage",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {  
                      CurrentPage = data.d;                                                  
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }

</script>--%>
<%If Not Session("selectedSession") = "PracticeFromComputer" And Not Session("ChooseMode") = 3 And Not IE = "1" Then%>
    <script type="text/javascript">
    $(function () {


        //console.log('Issender ท้ายเพจ' + IsSender);
        window.hubReady.done(function () {
            //  console.log('Issender ready ท้ายเพจ' + IsSender);
            if (IsSender == 'true') {
                //    console.log('Sender CheckAgain');
                SignalRCheck.server.sendCommand(SelectedName, 'CheckAgain');
                SignalRCheck.server.sendCommand(Groupname, 'ExamNum|' + ExamNum);
            }
            $('#hdIsSender').val(false);
            IsSender = 'false';
            //console.log('ค่า hd ตอนปรับแล้ว' + $('#hdIsSender').val());
        });
        if (IsSender == "false") {
            CheckQQNoAfterLoading();
        }
        //  console.log(ExamNum);


    });

    function CheckQQNoAfterLoading() {
        $.ajax({
            type: "POST",//ajax Post ไปเช็คว่าเป็นข้อล่าสุดหรือเปล่า
            url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/CheckQQNoAfterLoading",
            data: "{ ExamNum : '" + ExamNum + "'}",
            async: true,
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (msg) {
                //        console.log('เข้ามาเช็คข้อล่าสุด' + msg.d);
                if (msg.d == 'Reload') {//ถ้าเช็คเจอว่ามีข้อที่ใหม่กว่าให้ Reload หน้าตัวเองเพื่อ Render ข้อล่าสุด        
                    window.onbeforeunload = null;
                    window.location = "<%=ResolveUrl("~")%>Activity/activitypage.aspx" + Querystring
                }
            },
            error: function myfunction(request, status) {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง');
            }
        });
    }
</script>
<% End If%>
    <script type="text/javascript">
    $(function () {
        //hover animation
        InjectionHover('.BtnNextPreKeepTime', 3);
        InjectionHover('.imgToggle', 3);
        InjectionHover('.divSetting', 3);
        InjectionHover('.divSettingTop', 3);
        //InjectionHover('.imgShowPanelScore', 3,false);
        //InjectionHover('#imgCheckStatusTablet', 3);
        //InjectionHover('.imgShowStudentStatusPage',3,false);
        //InjectionHover('.replyAnswer', 3);
        //InjectionHover('.replyAnswer2', 3,false );
        //InjectionHover('#ForActivityPage', 3, false);
        //InjectionHover('#ForActivityPage2', 3, false);
        //InjectionHover('#btnShowTopScore2', 3, false);
        //InjectionHover('#DivTopScoreStudent', 3, false);
        //InjectionHover('#DivTopScoreStudent2', 3, false);
        //InjectionHover('.imgShowStudentStatusPage2', 3, false);
        var AnswerState = '<%=_AnswerState%>';

        if (AnswerState == '2') {
            var replyMode = parseInt($('#hdReplyMode').val());
            SetTxtReplyMode(replyMode);
            console.log(replyMode);
            Reply(replyMode);
            // ครูกดเฉลยเองงง
            $('div.ManualReply').click(function () {
                //$('td.reply').css({ 'background-color': '#2CA505', 'color': 'white' });
                //IsReply = true;
                Reply(1);
            });
            //btn สลับโหมดเฉลย
            $('#btnReplyMode').click(function () {
                $('div.ManualReply').hide();
                clearInterval(t);
                var mode = parseInt($('#hdReplyMode').val());
                //if (mode == 4) {
                //    mode = mode - 3;
                //} else {
                //    mode = mode + 1;
                //}
                mode = mode == 4 ? 1 : 4;
                if (!IsReply) {
                    Reply(mode);
                }
                SetTxtReplyMode(mode);
                $('#hdReplyMode').val(mode);
            });
        }

        //btn Open QuestionExpain
        var AnswerExpType;
        if ($('#AnswerExp').children('div').length > 0) { AnswerExpType = 1; } // $('#btnAnswerExp').show();
        if ($('ul > li').children('div.Correct').length > 0 || $('ul > li').children('div.InCorrect').length > 0) { AnswerExpType = 6; } // $('#btnAnswerExp').show();

        //ปิดอธิบายคำถาม

        //if ($('#QuestionExp').length > 0 || $('#AnswerExp').children('div').length > 0) { //$('#btnQuestionExp').show(); 
        //}
        //$('#btnQuestionExp').toggle(function () {
        //    //$('#QuestionExp').show();
        //    if (AnswerExpType == 1) { $('#Table1').hide(); $('#AnswerExp').show(); }
        //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        //}, function () {
        //    $('#QuestionExp').hide();
        //    if (AnswerExpType == 1) { $('#Table1').show(); $('#AnswerExp').hide(); }
        //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        //});

        //---------------------------------------------------------------------------------------------------------------------------------------------
        //btn Open AnswerExpain   

        //if ($('#AnswerExp').children('div').length > 0) { $('#btnAnswerExp').show(); AnswerExpType = 1; }       
        //$('#btnAnswerExp').toggle(function () {
        //    if (AnswerExpType == 1) { $('#Table1').hide(); $('#AnswerExp').show(); }
        //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        //}, function () {
        //    if (AnswerExpType == 1) { $('#Table1').show(); $('#AnswerExp').hide(); }
        //    else if (AnswerExpType == 6) { ShowOrHideAnswerExpTypeSix(); }
        //});
    });

    function ShowOrHideAnswerExpTypeSix() {
        if (resetHtml) { $('ul > li').children('div.Correct').show(); $('ul > li').children('div.InCorrect').show(); }
        else { $('ul > li').children('div.Correct').hide(); $('ul > li').children('div.InCorrect').hide(); }
        resetHtml = !resetHtml;
    }


    var IsReply = false;
    function Reply(replyMode) {
        // แสดงเฉลยตามโหมด       
        var AnswerType = $('#Table1').children().children().next().attr('class');
        if (replyMode == 1) {
            if (AnswerType == 3) {
                ReplyPair();
            }
            else if (AnswerType == 6) {
                ReplySort();
            } else {
                $('td.reply').css({ 'background-color': '#2CA505', 'color': 'white' });
            }
            IsReply = true;
        } else if (replyMode == 2) {
            TimeToReply(3, AnswerType); // รอเปิดเฉลย 3 วิ
        } else if (replyMode == 3) {
            TimeToReply(5, AnswerType); // รอเปิดเฉลย 5 วิ
        } else if (replyMode == 4) {
            //SetTxtReplyMode(replyMode);
            $('td.reply').css({ 'background-color': 'transparent', 'color': '#444' });
            $('div.ManualReply').show();
        }
    }
    function SetTxtReplyMode(m) {
        if (m == 1) {
            $('#btnReplyMode').css('background', 'url("../Images/Activity/ReplyMode/ReplyNormal.png")').html('');
        } else if (m == 2) {
            $('#btnReplyMode').css('background', 'url("../Images/Activity/ReplyMode/ReplyWithSecond.png")').html('3');
        } else if (m == 3) {
            $('#btnReplyMode').css('background', 'url("../Images/Activity/ReplyMode/ReplyWithSecond.png")').html('5');
        } else if (m == 4) {
            $('#btnReplyMode').css('background', 'url("../Images/Activity/ReplyMode/ReplyManual.png")').html('');
            $('div.ManualReply').show();
        }
    }
    var t;
    function TimeToReply(n, a) {
        $('td.reply').css({ 'background-color': 'transparent', 'color': '#444' });
        t = setInterval(function () {
            $('#btnReplyMode').html(n);
            n = n - 1;
            if (n < 0) {
                if (a == 3) {
                    ReplyPair();
                }
                else if (a == 6) {
                    ReplySort();
                } else {
                    $('td.reply').css({ 'background-color': '#2CA505', 'color': 'white' });
                }
                clearInterval(t);
                IsReply = true;
            }
        }, 1000);
    }

    // เฉลยแบบจับคู่
    function ReplyPair() {
        var Examnum = '<%=_ExamNum%>';
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetCorrectTypeThree",
            data: "{ ExamNum:'" + Examnum + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                $('#Table1').html(data.d);
            },
            error: function myfunction(request, status) {
            }
        });
    }
    // เฉลยแบบเรียงลำดับ
    function ReplySort() {
        var Examnum = '<%=_ExamNum%>';
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/GetCorrectTypeSix",
            data: "{ ExamNum:'" + Examnum + "'}",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                $('#Table1').html(data.d);
            },
            error: function myfunction(request, status) {
            }
        });
    }

    // ข้อข้าม
    function TriggerClickBtnSwapQuestion(e) {
        //if ($('#CheckIsHaveLeapChoice').val() == 1) {
        //    if ($(e.target).attr('id') == 'BtnSwapQuestion') {
        //        $(e.target).css('background-image', 'url(../Images/Activity/activitypagepad2.png)');
        //        $(e.target).css('background-position', '-180px -135px');
        //    }
        //    else {
        //        $(e.target).parent().css('background-image', 'url(../Images/Activity/activitypagepad2.png)');
        //        $(e.target).parent().css('background-position', '-180px -135px');
        //    }
        //}
        //else {
        //    if ($(e.target).attr('id') == 'BtnSwapQuestion') {
        //        $(e.target).css('background-image', 'url(../Images/Activity/activitypagepad2.png)');
        //        $(e.target).css('background-position', '-270px -135px');
        //    }
        //    else {
        //        $(e.target).parent().css('background-image', 'url(../Images/Activity/activitypagepad2.png)');
        //        $(e.target).parent().css('background-position', '-270px -135px');
        //    }
        //}
        if ($('#slideDiv').css('display') == 'none') {
            var AnswerState = '<%=_AnswerState %>';
            if ((AnswerState == '0') || (AnswerState == '1')) {
                if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                    $('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show();
                }
                else {
                    $('#slideDiv').css({ "background-color": "#FFEE2D" }).stop().show(500);
                }
            }
            CreateButtonLeapChoice("False");
        }
        else {
            if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                $('#slideDiv').stop().hide();
            }
            else {
                $('#slideDiv').stop().hide(500);
            }
        }
    }

    function TriggerClickBtnSortAnswerNull() {
        $('#slideDiv').css({ "background-color": "#FFEE2D" });
        $('#HDNotReplyMode').val("True");
        CreateButtonLeapChoice("False");
    };

    function TriggerClickBtnSortNormal() {
        $('#slideDiv').css({ "background-color": "#FFC" });
        $('#HDNotReplyMode').val("False");
        CreateButtonLeapChoice("True");
    };

    function TriggerClickBtnNextToLastChoice() {
        $('#slideDiv').css("display", "none");
        $('#HDNotReplyMode').val("True");
        $('#HDLastChoice').val("True");

        form1.submit();
    };

    function NextLeapChoicePage() {
        var CurrentPage = $('#HDLeapChoicePage').val();
        alert(CurrentPage);
        CurrentPage += 1;
        var NextPage = 'DivChoicePage' + CurrentPage
        $('#' + NextPage).css("display", "block");

    };

    function CreateButtonLeapChoice(IsNormalSort) {
        var PlayerId = '<%=PlayerId%>';
        var Examnum = '<%=_ExamNum%>';
        var AnswerState = '<%=_AnswerState %>';
        //alert(PlayerId);
        //alert(Examnum);
        //alert(AnswerState);

        if ((AnswerState == '1') || (AnswerState == '0')) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/CreateStringLeapChoice",	  //url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/CreateStringLeapChoice",          
                data: "{ IsNormalSort:'" + IsNormalSort + "',StudentId:'" + PlayerId + "',ExamNum:'" + Examnum + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    var Data = jQuery.parseJSON(data.d);
                    $('#mainReview').html(Data.HtmlLeapChoice);
                    var JsCheckOverOnePage = Data.CheckOverOnePage;
                    $('#mainReview').css('display', 'block');
                    if (JsCheckOverOnePage > '1') {
                        $('.imgPreNext').css('display', 'block');
                        $('#HDLeapChoicePage').val(JsCheckOverOnePage);
                        alert($('#HDLeapChoicePage').val());
                    } else {
                        $('.imgPreNext').css('display', 'none');
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
        }
    }

</script> 
 <%If BusinessTablet360.ClsKNSession.EnableReportQuestion Then %> 
    <style type="text/css">
    #mainQuestion .btnReportQuestion {
        position: absolute;
        cursor: pointer;
    }

    #mainQuestion .btnReportQuestion {
        right: 5px;
        top: 5px;
    }
</style>
    <script src="../js/reportProblemQuestion.js" type="text/javascript"></script>
<%End If %>
