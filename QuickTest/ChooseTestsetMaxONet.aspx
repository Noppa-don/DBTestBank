﻿<%@ Page Title="ChooseTestsetMaxOnet" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ChooseTestsetMaxONet.aspx.vb" Inherits="QuickTest.ChooseTestsetMaxONet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <style type="text/css">
        .divMainMaxOnet {
            text-align: center; margin-left: 55px;
        }

        .menuBar {
            margin: 5px; width: 240px; height: 180px; position: static; background: #D3F2F7; color: #FFF; padding: 5px; text-align: left; border-radius: 0.5em;
            margin-left: auto; margin-right: auto; display: inline-block; cursor: pointer;
        }

        .Practice, .Report {
            height: 100px; width: 100px; margin-left: auto; margin-right: auto; padding: 0; padding-left: 0 !important;
        }

        .DailyActivities {
            border-top: 1px solid #ddd; margin-top: 5px; padding: 15px 15px 0; width: 440px; text-align: center; margin-left: auto; margin-right: auto;
        }

        .DailyActivities > div {
                height: 65px; width: 65px; border: 1px solid #ddd; border-radius: 5px; margin: 5px 20px; display: inline-block; position: relative;
         }

        .DailyActivities > div > div {
            width: inherit; height: inherit; background-size: cover;
        }

        .DailyActivities > div span {
            font-size: 15px; font-weight: bold;
        }

        .active {
            border: 1px solid #FBCB09 !important; background-color: #FDF5CE;
        }

        .success {
            background: #ddd; opacity: 0.4;
        }

        span.selected {
            background: url('../Images/Maxonet/btnChecked.png'); background-repeat: no-repeat; height: 20px; width: 20px; position: absolute; right: -10px; top: -10px;
        }

        div.addSubject {
            width: 160px; height: 50px; background-color: rgba(255, 0, 0, 0.61); line-height: 50px; border-radius: 5px; color: whitesmoke; cursor: pointer; 
            background-image: url(../images/maxonet/coins.png); background-repeat: no-repeat; background-position: 95px 12px; display: inline-block; float: left;
        }

            div.addSubject span:first-child {
                margin-left: 20px;
                font-weight: bold;
                float: left;
            }

            div.addSubject span:last-child {
                float: right;
                margin-right: 9px;
            }

             div.GotoContact{
            width: 85px;
            height: 85px;
            cursor: pointer;
            background-image: url(../images/maxonet/post.png);
            background-size: 85px;
            display: inline-block;
            float: right;
        }
        .chooseClassContent > div {
            padding: 10px;
        }

        .chooseClassContent .classDiv, .chooseClassContent .success {
            display: inline-block;
            border: 1px solid #ddd;
            border-radius: 5px;
            position: relative;
            padding: 15px 20px;
            font-size: 16px;
            font-weight: bold;
            margin: 5px 1px;
        }

        .chooseClassContent .chooseClass, .chooseClassContent .chooseQuestionAmount {
            padding: 5px;
            text-align: center;
        }

        .chooseClassContent .questionAmountDiv {
            display: inline;
            border: 1px solid #ddd;
            border-radius: 5px;
            position: relative;
            padding: 15px 20px;
            font-size: 16px;
            font-weight: bold;
        }

        .chooseClassContent span.menu {
            font-size: 18px;
            font-weight: bold;
        }

        .chooseQuestionAmount input[type=text] {
            width: 30px;
            height: 30px;
            text-align: center;
        }

        h3 {
            padding: 15px 0 0 0!important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">

        <div id="site_content">
            <div class="content" style="width: 930px;">
                <div style="text-align: center;">
                    <div class="addSubject">
                        <span>เพิ่มวิชา</span>
                        <span id="spnCreditAmount" runat="server"></span>
                    </div>
                    <div style="display: inline-block;">
                        <h1 style="font-size: 32px; color: orange;">Max-O-Net</h1>
                    </div>
                    
                    <div class="GotoContact"></div>
                </div>

                <div class="divMainMaxOnet">
                    <div id="BarPracticeStandard" style='text-align: center' class="menuBar">
                        <div class="Practice" style="background: url(../images/Homework/Practice.png) center left no-repeat;"></div>
                        <h3>ทบทวนตามบทเรียน</h3>
                    </div>
                     <div id="BarDailyActivity" style='text-align: center' class="menuBar">
                        <div class="Practice" style="background: url(../images/Homework/PracticeFromTeacher.png) center left no-repeat;"></div>
                        <h3>ฝึกฝนเตรียมสอบ</h3>
                    </div>
                    <div id="BarStudentHistories" style='text-align: center' class="menuBar">
                        <div class="Report" style="background: url(../images/Homework/Report.png) center left no-repeat;"></div>
                        <h3>ผลงานทั้งหมด</h3>
                    </div>

                </div>

                <div style="text-align: center; margin-top: 20px;">
                    <asp:Label Text="" ID="lblExpireDate" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
        <img class="clock" src="../Images/Maxonet/clock.png" />
    </div>
    <div id="dialog" title="">
        <div id="daily" class="DailyActivities" runat="server" clientidmode="Static">
        </div>
    </div>
    <div id="alertDialog" title="">
    </div>
    <div id="chooseClassDialog" title="">
        <div class="chooseClassContent" style="min-height: 250px;">
           <%-- <div>
                <span class="menu">เลือกชั้น</span>c
                <div class="chooseClass">
                    <div class="classDiv">ป.1</div>
                    <div class="classDiv">ป.2</div>
                    <div class="classDiv">ป.3</div>
                    <div class="classDiv">ป.4</div>
                    <div class="classDiv">ป.5</div>
                    <div class="classDiv">ป.6</div>
                    <div class="classDiv">ม.1</div>
                    <div class="classDiv">ม.2</div>
                    <div class="classDiv">ม.3</div>
                </div>
                <span class="menu">จำนวนข้อสอบที่ต้องการทำ</span>
                <div class="chooseQuestionAmount">
                    <div class="questionAmountDiv" value="10">10 ข้อ</div>
                    <div class="questionAmountDiv" value="15">15 ข้อ</div>
                    <div class="questionAmountDiv" value="20">20 ข้อ</div>
                    <div class="questionAmountDiv" value="">
                        <input type="text" maxlength="2" disabled="disabled" onkeypress='return event.charCode >= 48 && event.charCode <= 57' />
                    </div>
                </div>
            </div>--%>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" />
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
      <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css">
        <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>
    <script type="text/javascript">
        var subjectAmount = '<%=SubjectAmount%>';
        var dailyActivitiesURL = "<%=DailyActivitiesURL %>";
        var deviceId = '<%=DVID%>';
        var tokenId = '<%=TokenId%>';
        var studentId = '<%=StudentId%>';
        var levelAmount = '<%=LevelAmount%>'

        var uda = { DeviceId: deviceId, TokenId: tokenId, StudentId: studentId, SubjectId: '', LevelId: '', QuestionAmount: 10 };

        var selected = $("<span class='selected'></span>");
        var selected2 = $("<span class='selected'></span>");
        var selected3 = $("<span class='selected'></span>");

            Shadowbox.init({
                skipSetup: false
            });

        $(function () {

            //$('.questionAmountDiv Dis').attr("disabled", "disabled").off('click');

            $('#BarPracticeStandard').each(function () {
                new FastButton(this, TriggerPracticeStandardBarClick);
            });

            $('#BarStudentHistories').each(function () {
                new FastButton(this, TriggerStudentDetailBarClick);
            });

            $('#BarDailyActivity').each(function () {
		alert(0);
                new FastButton(this, TriggerDailyActivityBarClick);
            });

            var arrayTxtActivity = ["มีกิจกรรมมาใหม่ ทำเลยมั้ยคะ ?", "เพิ่มพูนความรู้ ด้วยการทำข้อสอบประจำวันค่ะ", "ฝึกซ้อมทำข้อสอบเพิ่ม สำหรับวันนี้เลยมั้ยคะ", "แนะนำแบบฝึกหัดใหม่ค่ะ เข้าทำเลย ?"];
            $('#dialog').dialog({
                autoOpen: false,
                buttons: {
                    'ไว้ก่อน': function () {
                        //DisableGetDialyActivities($(this));
                        $(this).dialog('close');
                    }, "ทำเลย": function () {
                        //comment ไว้ก่อน รอทำ query สร้างชุดข้อสอบ
                        //var urlToActivity = (subjectAmount == 1) ? dailyActivitiesURL : $('.active').attr('urlactivity');
                        //window.location = urlToActivity + "&dialyactivities=true";
                        //DisableGetDialyActivities($(this));
                        $(this).dialog('close');
                        var subjectId = $('.subjectActivities.active').attr('id');
                        GetChooseClassActivities(subjectId);
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            });

            $('#chooseClassDialog').dialog({
                autoOpen: false,
                buttons: {
                    'ยกเลิก': function () {
                        $(this).dialog('close');
                        $('#dialog').dialog('open');
                    }, "เริ่มทำ": function () {
                        //set ค่าจาก textbox ใส่ attribute
                        var val = $('.questionAmountDiv:last-child').children().val();
                        $('.questionAmountDiv:last-child').attr('value', val);
               
                        // จำนวนข้อสอบจาก div ที่เลือกมา
                        var questionAmount = $('.questionAmountDiv.active').attr('value');
                        var LimitAmount = $('#LimitAmount').attr('value');

                        if (isNaN(questionAmount)) {
                            callAlertDialog("ใส่จำนวนข้อสอบเป็นตัวเลขเท่านั้นค่ะ")
                            return 0;
                        }
                        if (questionAmount == "" || questionAmount <= 0) {
                            callAlertDialog("ใส่จำนวนข้อสอบมากกว่า 0 เท่านั้นค่ะ")
                            return 0;
                        }
                        if (parseInt(questionAmount) > parseInt(LimitAmount)) {
                            callAlertDialog("ใส่ได้ไม่เกิน " + LimitAmount + " ข้อค่ะ")
                            return 0;
                        }

                        $(this).dialog('close');
                        GetUrlActivity();
                    }
                },
                draggable: false,
                resizable: false,
                modal: true,
            }).dialog('option', 'title', 'เลือกชั้นและจำนวนข้อค่ะ');

            //ShowActivityDialog();

            //alert(dailyActivitiesURL);
            //if (dailyActivitiesURL !== "") {
            //    var n = Math.floor(Math.random() * arrayTxtActivity.length);
            //    var txt = arrayTxtActivity[n];
            //    $('#dialog').dialog('option', 'title', txt).dialog("open");
            //}

            //ใส่ class active และเครื่องหมายถูกให้กับ div วิชาที่ยังไม่ได้ทำกิจกรรม
            $('.subjectActivities:first').addClass('active').append(selected);

            // เลือกวิชาก่อนเข้าทำกิจกรรมประจำวัน
            $('.subjectActivities').click(function () {
                $('.subjectActivities').removeClass('active');
                $('span.selected').remove();
                $(this).addClass('active').append(selected);
            });

            //เลือกชั้นก่อนเข้าทำกิจกรรมประจำวัน
            $('.classDiv').live('click', function () {
                $('.classDiv').removeClass('active');
                $(this).children('span').remove();
                $(this).addClass('active').append(selected2);
            });
                      

            //เลือกจำนวนข้อก่อนทำกิจกรรมประจำวัน       
            $('.questionAmountDiv').live('click', function (e) {
                console.log(e.target.nodeName);
                if ($(this).hasClass('active')) return 0;
                $('.questionAmountDiv').removeClass('active');
                $(this).children('span').remove();
                $(this).addClass('active').append(selected3);

                //$('.questionAmountDiv:last-child').children().attr('disabled', true);
                //เมื่อเลือก div แบบให้กรอกจำนวนข้อเอง
                //$(this).children().attr('disabled', false);
                $(this).children().focus();                
            });

            //ปุ่มเพิ่มเครดิต และไปเลือกวิชาชั้นเพิ่ม
            $('.addSubject').click(function () {
                chooseSound.play();
                CheckAndChangeDevice('../Practicemode_pad/DefaultMaxOnet.aspx' + '?deviceuniqueid=' + deviceId + '&token=' + tokenId + '&addSubject=true');
            });
            $('.GotoContact').click(function () {
                chooseSound.play();
                CheckAndChangeDevice('GoContact');
                //GotoContact();
            });
        });

       

        function callAlertDialog(titleName) {
            var $d = $('#alertDialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }

            function DisableGetDialyActivities(ele) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/DisableGetDailyActivities",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d === true) {
                            $(ele).dialog('close');
                        }
                    },
                    error: function myfunction(request, status) {
                    }
                });
            }

        
            function GetChooseClassActivities(subjectId) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/GetChooseClassActivities",
                    async: false,
                    data: "{ studentId :  '" + studentId + "',subjectId :  '" + subjectId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",      
                    success: function (data) {
                        var htmlContent = data.d;
                        if (htmlContent != "") {
                            uda.SubjectId = subjectId
                            $('.chooseClassContent').html('');
                            $('.chooseClassContent').append(htmlContent);
                            $('.classDiv:first').addClass('active').append(selected2);
                            $('.questionAmountDiv:first').addClass('active').append(selected3);                        
                            $('#chooseClassDialog').dialog("open");
                            // set ไม่ให้ focus textbox                        
                            $('input[type="text"]').blur();
                        } else {
                            //alert('');
                        }
                    },
                    error: function myfunction(request, status) {
                    }
                });
            }

            function GetUrlActivity() {
                OpenBlockUIRandomQuestion();
                var levelId = $('.classDiv.active').attr('id');
                uda.LevelId = levelId;
                var questionAmount = $('.questionAmountDiv.active').attr('value');
                uda.QuestionAmount = questionAmount;
                var dataObj = "{ userDialyActivityJson : '" + JSON.stringify(uda) + "'}";
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/GetUrlActivity",
                    async: false,
                    data: dataObj,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != "") {
                            setTimeout(function () { window.location = data.d + "&dialyactivities=true"; }, 3000);
                        } else {
                            $.unblockUI();
                            alert('SERVER ERROR!!');
                        }
                    },
                    error: function myfunction(request, status) {
                    }
                });
            }

            function OpenBlockUIRandomQuestion() {
                $.blockUI({
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: 'initial',
                        color: '#fff'
                    },
                    message: '<img src="<%=ResolveUrl("~")%>Images/waitspinner.gif" width="100" height="100" /><br /><span style="font-size:35px;">รอสักครู่นะคะ ระบบกำลังสุ่มข้อสอบสำหรับฝึกฝนเตรียมสอบค่ะ</span>'
                });
            }

            function TriggerPracticeStandardBarClick() {
                //openBlockUI();
                chooseSound.play();
                var p;
                var RedirectMode;
                if (levelAmount > 1) {
                    p = "ChooseClass.aspx";
                    RedirectMode = "3"
                } else if (subjectAmount > 1) {
                    p = "ChooseSubject.aspx";
                    RedirectMode = "2"
                } else {
                    p = "ChooseQuestionset.aspx";
                    RedirectMode = "1"
                }

                refreshUrl = '../PracticeMode_Pad/' + p + '?deviceuniqueid=' + deviceId + '&token=' + tokenId + '&DashboardMode=9&RedirectMode=' + RedirectMode;
                if (responseTimeCI != 0) return 0;

                CheckAndChangeDevice(refreshUrl);              
            }

            function TriggerStudentDetailBarClick() {
                //openBlockUI();
                chooseSound.play();
                refreshUrl = '../Student/StudentDetailPage.aspx' + '?deviceuniqueid=' + deviceId + '&token=' + tokenId;
                if (responseTimeCI != 0) return 0;
                CheckAndChangeDevice(refreshUrl);
            }

            function TriggerDailyActivityBarClick() {
                chooseSound.play();
                CheckAndChangeDevice('dailyActivity');
            }

            function CheckAndChangeDevice(RedirectPath) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/CheckIsNowDevice",
                    data: "{ DeviceId: '1'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        var result;
                        result = msg.d;
                        if (result == '0') {
                            var $dialogChangeDevice = $('<div>');
                            var diBtn = {};
                            diBtn["ไม่เปลี่ยน"] = function () {
                                $dialogChangeDevice.dialog('close');
                            };
                            diBtn["เปลี่ยน"] = function () {
                                $dialogChangeDevice.dialog('close');
                                SetNowDevice(deviceId);
                                RedirectNextStep(RedirectPath);
                            };

                            $dialogChangeDevice.dialog({ buttons: diBtn, draggable: false, resizable: false, modal: true, autoOpen: false }).dialog('option', 'title', '&nbsp;&nbsp;มีเครื่องอื่นเข้าใช้งานอยู่ ถ้าเปลี่ยนมาใช้งานเครื่องนี้ อีกเครื่องจะไม่สามารถใช้งานได้ ต้องการเปลี่ยนเลยใช่มั้ยคะ');
                            $dialogChangeDevice.dialog('open');
                        } else {
                            RedirectNextStep(RedirectPath);
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }
        function SetNowDevice(deviceId) {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/UserPwdService.asmx/SetNowDeviceToRedis",
                    data: "{ DeviceId: '" + deviceId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                    },
                    error: function myfunction(request, status) {
                        //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
            }

        function RedirectNextStep(RedirectPath) {
            if (RedirectPath == 'dailyActivity') {
                var arrayTxtActivity = ["มีกิจกรรมมาใหม่ ทำเลยมั้ยคะ ?", "เพิ่มพูนความรู้ ด้วยการทำข้อสอบประจำวันค่ะ", "ฝึกซ้อมทำข้อสอบเพิ่ม สำหรับวันนี้เลยมั้ยคะ", "แนะนำแบบฝึกหัดใหม่ค่ะ เข้าทำเลย ?"];
                var n = Math.floor(Math.random() * arrayTxtActivity.length);
                var txt = arrayTxtActivity[n];
                $('#dialog').dialog('option', 'title', txt).dialog("open");
            } else if (RedirectPath == 'GoContact') {
                GotoContact();
            } else {
                openBlockUI();
                document.location.href = RedirectPath;
            }
        }

        function GotoContact() {
        
     Shadowbox.open({
                        content: '<div style="overflow: hidden;white-space: nowrap; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="<%=ResolveUrl("~")%>Contact.aspx" frameborder="0" width="700" height="570"></iframe></div>',
                        player: "html",
                        title: "ติดต่อเรา",
                        height: 570,
                        width: 700
                    });
                };
    </script>
</asp:Content>
