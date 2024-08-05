<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RaceActivity.aspx.vb"
    Inherits="QuickTest.RaceActivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <script src="../js/jrumble.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <link href="../css/prettyPhoto.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            //cre div raceActivity
            creDivRaceActivity(20);
            function creDivRaceActivity(number) {
                for (var i = 1; i <= number; i++) {
                    var randomStuId = Math.floor(Math.random() * 5) + 1;
                    var divStuId = '<div class="divStuId"><br/>เลขที่<br />' + randomStuId + '</div>';
                    var divStuOrder = '<div class="divStuOrder"><br/>ลำดับ<br />' + i + '</div>';
                    $('#stuID').append(divStuId);
                    $('#stuOrder').append(divStuOrder);
                }
            }
            // check answer for raceActivity      
            var randomAns = Math.floor(Math.random() * 4) + 1;
            $('div .divStuId:first-child').live('click', function () {
                var answer = $(this).html();
                answer = answer.replace('<br>เลขที่<br>', '');
                if (answer == randomAns) {
                    $(this).effect('highlight', 1000);
                    highLightAnswer(true);
                }
                else {
                    $(this).effect('explode', 500, callBack);
                    //$('div .divStuOrder:last-child').effect('clip', 500, callBack); 
                    highLightAnswer(false);
                }
            });
            function callBack() {
                $('div .divStuId:first-child').remove();
                $('div .divStuOrder:last-child').remove();
            }
            function highLightAnswer(isRight) {
                if (isRight == false) {
                    $('#rightAns').effect('highlight', 500);
                }
                else if (isRight == true) {
                    $('#rightAns').css('background-color', 'green');
                }
            }

            $('#btnStuClick').click(function (e) {
                var answer = $('div .divStuId:first-child').html();
                answer = answer.replace('<br>เลขที่<br>', '');
                if (answer == randomAns) {
                    $('div .divStuId:first-child').css('position', 'absolute').css('width', '300px').css('font-size', '50px').css('height', '300px').css('z-index', '9999').jrumble({
                        rumbleEvent: 'constant',
                        rangeX: 0,
                        rangeY: 0,
                        rangeRot: 5
                    });
                    highLightAnswer(true);
                }
                else {
                    $('div .divStuId:first-child').css('position', 'absolute').css('width', '300px').css('height', '300px').css('z-index', '9999').effect('explode', 500, callBack);
                    //$('div .divStuOrder:last-child').effect('clip', 500, callBack); 
                    highLightAnswer(false);
                }
            });



        });
        
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            //End Activity
            $('.endActivity').click(function () {
                $('#dialog').dialog('open');
            });

            // Click Menu Setting
            $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            $('.divSetting').toggle(function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '5%' }, 200);
            }, function () {
                $('#navSetting a').stop().animate({ 'marginLeft': '1000px' }, 1000);
            });

            // Check Menu Position
            checkMenu();
            function checkMenu() {
                // เช็ค ปุ่มเลื่อนข้อ
                var menuState = $('#ShowBtnOnTapMenu').val();
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
                    $('.divMenuTop').show();
                    $('.divMenuSide').hide();
                } // 1 อยู่ด้านข้าง
                else {
                    $('.divMenuTop').hide();
                    $('.divMenuSide').show();
                }
            }

            // Click Toggle Menu Top Side
            $('.imgToggle').click(function () {
                if ($(this).parent().is('.divMenuTop')) {
                    $('#ShowMenuOnPosTop').val('1');
                    $('.divMenuTop').hide();
                    $('.divMenuSide').show();
                }
                else {
                    $('#ShowMenuOnPosTop').val('0');
                    $('.divMenuTop').show();
                    $('.divMenuSide').hide();
                }
            });

            // click hide/show btn Pre&Next Position
            $('#hideDivNextPre').click(function () {
                $('#ShowBtnOnTapMenu').val('0');
                $('#divNextPre').hide();
                $('.menuTopSideNextPre').show();
            });
            $('.showDivNextPre').click(function () {
                $('#ShowBtnOnTapMenu').val('1');
                $('#divNextPre').show();
                $('.menuTopSideNextPre').hide();
            });

            // show ดัชนี
            $('.Dindex').toggle(function () {
                $('.divIndex').show();
            }, function () {
                $('.divIndex').hide();
            });

            // dialog
            $('#dialog').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () { window.location.href = '/Activity/AlternativePage.aspx'; }, 'ไม่': function () { $(this).dialog('close'); } },
                draggable: false,
                resizable: false,
                modal: true
            });
            $('.about').click(function () {
                window.location.href = '/Activity/ActivityReport.aspx';
            });

            //จับเวลา
            var secElapsedTime = parseInt($('#secTime').val());
            var minuteElapsedTime = parseInt($('#minuteTime').val());
            var Timer = setInterval(function () {
                // วินาทีถอยหลัง เดินหน้า                
                secElapsedTime = secElapsedTime + 1;

                if (secElapsedTime.toString().length == 1) {
                    $('.secElapsedTime').text('0' + secElapsedTime);
                }
                else if (secElapsedTime.toString().length > 1) {
                    $('.secElapsedTime').text(secElapsedTime);
                }

                $('.minuteElapsedTime').text(minuteElapsedTime);

                if (secElapsedTime == 60) {
                    $('.minuteElapsedTime').text(minuteElapsedTime + 1);
                    $('.secElapsedTime').text('00');
                    secElapsedTime = 0;
                    minuteElapsedTime = minuteElapsedTime + 1;
                }
            }, 1000);
            $('.BtnNextPreKeepTime').click(function () {
                $('#minuteTime').val(minuteElapsedTime);
                $('#secTime').val(secElapsedTime);
            });

            // Frame เช็คสถานะแทปเล็ต
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 800,
                default_height: 600,
                modal: true
            });
            $.prettyLoader();
        });  
    </script>
    <style type="text/css">
        .BtnNextPre
        {
            top: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="divMenuTop">
        <asp:HiddenField ID="ShowBtnOnTapMenu" runat="server" Value="0" />
        <asp:HiddenField ID="ShowMenuOnPosTop" runat="server" Value="0" />
        <asp:HiddenField ID="minuteTime" runat="server" Value="0" />
        <asp:HiddenField ID="secTime" runat="server" Value="0" />
        <img id="imgMenuTopSide;" class="imgToggle" src="../Images/Activity/menuTop.png" />
        <div>
            <img src="../Images/Activity/clock2.png" style="margin: 2%; width: 40px; height: 80px" />
            <span id="minuteElapsedTime" class="minuteElapsedTime"></span><span>: </span><span
                id="secElapsedTime" class="secElapsedTime"></span>
        </div>
        <div>
            <span>
                <br />
                <b>สังคม<br />
                    บท ประชาธิปไตย</b></span>
        </div>
        <div>
            ดัชนี</div>
        <div id="divMenuTopNextPre" class="menuTopSideNextPre">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%; float: left; height: 120px;">
                        <asp:ImageButton ID="btnPrvTop" runat="server" src="../images/forward-icon.png" Width="40px"
                            class="BtnNextPreKeepTime" />
                    </td>
                    <td style="width: 100%;">
                        <asp:Label runat="server" ID="lblNoExam"></asp:Label>
                    </td>
                    <td style="width: 100%; text-align: right; height: 120px; float: left;">
                        <asp:ImageButton ID="btnNextTop" runat="server" src="../images/next-icon.png" Width="40px"
                            class="BtnNextPreKeepTime" />
                    </td>
                </tr>
            </table>
            <div style="background-color: Gray; border: 1px outset; width: 30px; height: 30px;
                position: absolute; bottom: 0px; left: 0px; margin: 0px; background-image: url('../images/Activity/run2.png');"
                class='showDivNextPre'>
            </div>
        </div>
        <div id="tablet" style="line-height: 2; background-color: #B4DCED; width: 90px;">
            <a href="chkTabletConnect.aspx&iframe=true&width=80%&height=100%" rel="prettyPhoto">
                ดูสถานะการเชื่อมต่อของแทปเล็ต</a></div>
    </div>
    <div class="divMenuSide">
        <img class="imgToggle" src="../Images/Activity/menuTop.png" />
        <div>
            <img src="../Images/Activity/clock2.png" style="margin: 10px; width: 80px; height: 80px" /><br />
            <span id="Span1" class="minuteElapsedTime"></span><span>: </span><span id="Span2"
                class="secElapsedTime"></span>
        </div>
        <div>
            <span>
                <br />
                <br />
                <b>สังคม<br />
                    บท ประชาธิปไตย</b></span>
        </div>
        <div id="divMenuSideNextPre" class="menuTopSideNextPre">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%; float: left; height: 120px;">
                        <asp:ImageButton ID="btnPrvSide" runat="server" src="../images/forward-icon.png"
                            class="BtnNextPreKeepTime" Width="40px" />
                    </td>
                    <td style="width: 100%;">
                        <asp:Label runat="server" ID="lblNoExamSide"></asp:Label>
                    </td>
                    <td style="width: 100%; text-align: right; height: 120px; float: left;">
                        <asp:ImageButton ID="btnNextSide" runat="server" src="../images/next-icon.png" Width="40px"
                            class="BtnNextPreKeepTime" />
                    </td>
                </tr>
            </table>
            <div style="background-color: Gray; border: 1px outset; width: 30px; height: 30px;
                position: absolute; bottom: 0px; top: initial; left: 0px; margin: 0px; background-image: url('../images/Activity/run2.png');"
                class='showDivNextPre'>
            </div>
        </div>
        <div style="line-height: 5;" class="Dindex">
            ดัชนี</div>
    </div>
    <div id='DivRace' style="position: absolute; width: 60%; left: 20%; top: 18%; text-align: center;
        height: 20%;">
        <div id='stuID' style="width: 99%; height: 47%; border: solid 1px; padding: 5px;
            overflow: hidden;">
        </div>
        <div id='stuOrder' style="width: 99%; height: 47%; border: solid 1px; padding: 5px;
            overflow: hidden;">
        </div>
    </div>
    <div class="divQuestionAndAnswer" style="top: 50%;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 5%;" id="Question_No" runat="server">
                </td>
                <td style="width: 95%;" colspan="4" id="QuestionName" runat="server">
                </td>
            </tr>
            <tr>
                <td id="Answer" runat="server">
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <div id="divNextPre">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 48%; float: left; height: 120px; border-bottom-width: 2px; border-bottom-style: solid;
                        border-bottom-color: Black;">
                        <asp:ImageButton ID="btnPrevious" runat="server" src="../images/forward-icon.png"
                            class="BtnNextPreKeepTime" />
                        <asp:ImageButton ID="ImageButton3" runat="server" src="../images/Activity/Home.png"
                            Width="100px" Height="100px" />
                        <asp:Image ID="imgRun" runat="server" src="../images/Activity/run.png" Height="51px"
                            Width="48px" CssClass="positionImg" />
                    </td>
                    <td style="width: 48%; text-align: right; height: 120px; float: right; border-bottom-width: 2px;
                        border-bottom-style: solid; border-bottom-color: Black;">
                        <asp:ImageButton ID="ImageButton4" runat="server" src="../images/Activity/Flag.png"
                            Width="100px" Height="100px" />
                        <asp:ImageButton ID="btnNext" runat="server" src="../images/next-icon.png" class="BtnNextPreKeepTime" />
                    </td>
                </tr>
            </table>
            <div style="border-style: outset; border-color: inherit; border-width: 1px; width: 30px;
                height: 30px; position: absolute; bottom: 5%; background-image: url('../images/Activity/run2.png');
                background-repeat: no-repeat; background-position: left; right: 898px;" id='hideDivNextPre'>
            </div>
        </div>
    </div>
    <div class="divSetting">
    </div>
    <ul id="navSetting">
        <li class="about"><a title="ดูคะแนน">ดูคะแนน</a></li>
        <li class="endActivity"><a title="จบกิจกรรม">จบกิจกรรม</a></li>
    </ul>
    <div id="dialog" title="ต้องการออกจากกิจกรรมใช่หรือไม่ ?">
    </div>
    </form>
</body>
</html>
