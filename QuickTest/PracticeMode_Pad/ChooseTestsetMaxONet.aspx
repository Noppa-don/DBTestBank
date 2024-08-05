<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ChooseTestsetMaxONet.aspx.vb" Inherits="QuickTest.ChooseTestsetMaxONet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <style type="text/css">
        .main {
            max-width: 100%;
        }

        .divMainMaxOnet {
            text-align: center;
            margin-left: 55px;
        }

        .menuBar {
            margin: 5px;
            width: 240px;
            height: 180px;
            position: static;
            background: #D3F2F7;
            color: #FFF;
            padding: 5px;
            text-align: left;
            border-radius: 0.5em;
            margin-left: auto;
            margin-right: auto;
            display: inline-block;
            cursor: pointer;
        }

        .Practice, .Report, .Activity {
            height: 100px;
            width: 100px;
            margin-left: auto;
            margin-right: auto;
            padding: 0;
            padding-left: 0 !important;
        }

        .DailyActivities {
            border-top: 1px solid #ddd;
            margin-top: 5px;
            padding: 15px 15px 0;
            width: 440px;
            text-align: center;
            margin-left: auto;
            margin-right: auto;
        }

            .DailyActivities > div {
                height: 65px;
                width: 65px;
                border: 1px solid #ddd;
                border-radius: 5px;
                margin: 5px 20px;
                display: inline-block;
                position: relative;
            }

                .DailyActivities > div > div {
                    width: inherit;
                    height: inherit;
                    background-size: cover;
                }

                .DailyActivities > div span {
                    font-size: 15px;
                    font-weight: bold;
                }

        .active {
            border: 1px solid #FBCB09 !important;
            background-color: #FDF5CE;
        }

        .success {
            background: #ddd;
            opacity: 0.4;
        }

        span.selected {
            background: url('../Images/Maxonet/btnChecked.png');
            background-repeat: no-repeat;
            height: 20px;
            width: 20px;
            position: absolute;
            right: -10px;
            top: -10px;
        }

        div.addSubject {
            width: 160px;
            height: 50px;
            background-color: rgba(255, 0, 0, 0.61);
            line-height: 50px;
            border-radius: 5px;
            color: whitesmoke;
            cursor: pointer;
            background-image: url(../images/maxonet/coins.png);
            background-repeat: no-repeat;
            background-position: 95px 12px;
            display: inline-block;
            float: left;
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

        div.GotoContact {
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
            padding: 15px 0 0 0 !important;
        }

        .PDPAbt {
            width: 96%; border: 1px solid #ccc; background: #F6F6F6;  overflow: hidden; height: 40px; 
        }
        
        .enableBt {
            cursor:pointer;
            color: #1C94C4;
        }

        .disableBt {
            cursor:not-allowed;
            color: #c1c1c1;
        }

    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">
        <div id="site_content">
            <input type="hidden" id='LimitAmount' clientidmode="Static" runat="server" />
            <div class="content" style="width: 930px;">
                <div style="text-align: center;">
                    <div id="addSubject" class="addSubject">
                        <span>เพิ่มวิชา</span>
                        <span id="spnCreditAmount" runat="server"></span>
                    </div>
                    <div style="display: inline-block;">
                        <h1 style="font-size: 32px; color: orange;">Max-O-Net</h1>
                    </div>

                    <div id="GotoContact" class="GotoContact"></div>
                </div>

                <div class="divMainMaxOnet">
                    <div id="BarPracticeStandard" style='text-align: center' class="menuBar">
                        <div class="Practice" style="background: url(../images/Homework/Practice.png) center left no-repeat;"></div>
                        <h3>ทำเพื่อเข้าใจ</h3>
                    </div>
                    <div id="BarDailyActivity" style='text-align: center' class="menuBar">
                        <div class="Activity" style="background: url(../images/Homework/PracticeFromTeacher.png) center left no-repeat;"></div>
                        <h3>ฝึกฝนให้ชำนาญ</h3>
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
    <div id="dialog2" class="dialogSession"></div>

    <div id="chooseClassDialog" title="">
        <div class="chooseClassContent" style="min-height: 250px;"></div>
    </div>
    <div id="PDPADialog" title="">
        <div class="PDPAContent">
            <table>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <input type="text" readonly="readonly" style="width: 90%; height: 250px;" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="checkbox" id="radio1" /><label for="radio1">ยอมรับ 1</label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="checkbox" id="radio2" /><label for="radio2">ยอมรับ 2</label></td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <input type="button" id="bcancel" value="ยกเลิก" class="PDPAbt enableBt" /></td>
                    <td style="text-align: center;">
                        <input type="button" id="bok" value="ตกลง" class="PDPAbt disableBt" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">

    <script type="text/javascript" src="../js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="../js/GFB.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <%--    <script type="text/javascript" src="../js/CheckAndSetNowDevice.js"></script>--%>
    <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>

    <link rel="stylesheet" type="text/css" href="../css/jquery-ui-1.8.18.custom.min.css" />
    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css" />

    <script type="text/javascript">
        var subjectAmount = '<%=SubjectAmount%>';
        var dailyActivitiesURL = "<%=DailyActivitiesURL %>";
        var deviceId = '<%=DVID%>';
        var tokenId = '<%=TokenId%>';
        var studentId = '<%=StudentId%>';
        var levelAmount = '<%=LevelAmount%>'
        var baseURL = "<%=ResolveUrl("~")%>";
        var uda = { DeviceId: deviceId, TokenId: tokenId, StudentId: studentId, SubjectId: '', LevelId: '', QuestionAmount: 10 };

        var selected = $("<span class='selected'></span>");
        var selected2 = $("<span class='selected'></span>");
        var selected3 = $("<span class='selected'></span>");
        var CheckPage = '<%=CheckPage%>';

        Shadowbox.init({
            skipSetup: false
        });

        $(document).ready(function () {
            NewFastButton();
           <%-- var CurrentPageStatus = '<%=Session("SessionStatus")%>'
            console.log(CurrentPageStatus);

            var ButtonName = event.path[1].id;
            if (ButtonName != "NotChange") {
                if (CurrentPageStatus == 1) {
                    CheckIsNowDevice(deviceId, tokenId, baseURL,studentId);
                }
            }--%>
        });

        //window.addEventListener("click", function (event) {
        //    var ButtonName = event.path[1].id;
        //    console.log("Clicked" + ButtonName);
        //    if (ButtonName != "NotChange") {
        //        //CheckIsNowDevice(deviceId, tokenId, baseURL, studentId);
        //    }
        //});



        $(function () {
            $('#PDPADialog').dialog({
                autoOpen: false,
                resizable: false,
                error: function myfunction(request, status) {
                }
            });

            $('#PDPADialog').dialog("open");

            var arrayTxtActivity = ["มีกิจกรรมมาใหม่ ทำเลยมั้ยคะ ?", "เพิ่มพูนความรู้ ด้วยการทำข้อสอบประจำวันค่ะ", "ฝึกซ้อมทำข้อสอบเพิ่ม สำหรับวันนี้เลยมั้ยคะ", "แนะนำแบบฝึกหัดใหม่ค่ะ เข้าทำเลย ?"];

            $('#dialog').dialog({
                autoOpen: false,
                buttons: {
                    'ไว้ก่อน': function () {
                        $(this).dialog('close');
                    }, "ทำเลย": function () {
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
                        console.log(LimitAmount);

                        if (isNaN(questionAmount)) {
                            callAlertDialog("ใส่จำนวนข้อสอบเป็นตัวเลขเท่านั้นค่ะ")
                            return 0;
                        }
                        if (questionAmount == "" || questionAmount <= 0) {
                            callAlertDialog("ใส่จำนวนข้อสอบมากกว่า 0 เท่านั้นค่ะ")
                            return 0;
                        }
                        if (parseInt(questionAmount) > LimitAmount) {
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


            $('#radio1').click(function () {
                EnableOKButton();
            });

            $('#radio2').click(function () {
                EnableOKButton();
            });

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
        });

        function EnableOKButton() {
            var r1 = $('#radio1').is(":checked");
            var r2 = $('#radio2').is(":checked");

            if (r1 == true && r2 == true) {
                $('#bok').removeClass("disableBt");
                $('#bok').addClass("enableBt");
            } else {
                $('#bok').removeClass("enableBt");
                $('#bok').addClass("disableBt");
            }
        }

        function NewFastButton() {
            $('#BarPracticeStandard').each(function () {
                new FastButton(this, TriggerPracticeStandardBarClick);
            });

            $('#BarStudentHistories').each(function () {
                new FastButton(this, TriggerStudentDetailBarClick);
            });

            $('#BarDailyActivity').each(function () {
                new FastButton(this, TriggerDailyActivityBarClick);
            });
            $('#addSubject').each(function () {
                new FastButton(this, GoToAddSubject);
            });
            $('#GotoContact').each(function () {
                new FastButton(this, GotoContact);
            });
        }

        //function DeleteFastButton() {
        //    console.log("Strat : Delete FastButton");
        //    document.getElementById("BarPracticeStandard").removeEventListener("click", TriggerPracticeStandardBarClick);
        //    document.getElementById("BarPracticeStandard").removeEventListener("touchstart", TriggerPracticeStandardBarClick);
        //    document.getElementById("BarStudentHistories").removeEventListener("click", TriggerStudentDetailBarClick);
        //    document.getElementById("BarStudentHistories").removeEventListener("touchstart", TriggerStudentDetailBarClick);
        //    document.getElementById("BarDailyActivity").removeEventListener("click", TriggerDailyActivityBarClick);
        //    document.getElementById("BarDailyActivity").removeEventListener("touchstart", TriggerDailyActivityBarClick);
        //    document.getElementById("addSubject").removeEventListener("click", GoToAddSubject);
        //    document.getElementById("addSubject").removeEventListener("touchstart", GoToAddSubject);
        //    //document.getElementById("GotoContact").removeEventListener("click", GotoContact);
        //    //document.getElementById("GotoContact").removeEventListener("touchstart", GotoContact);
        //    $("#main").off("click", "#GotoContact", GotoContact);
        //    $("#main").off("touchstart", "#GotoContact", GotoContact);
        //    console.log("End : Delete FastButton");
        //}

        function GoToAddSubject() {
            document.location.href = '../Practicemode_pad/DefaultMaxOnet.aspx' + '?deviceuniqueid=' + deviceId + '&token=' + tokenId + '&addSubject=true';
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
                        alert('SERVER ERROR!!!!');
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
                message: '<img src="<%=ResolveUrl("~")%>Images/waitspinner.gif" width="100" height="100" /><br /><span style="font-size:35px;">รอสักครู่นะคะ กำลังทำการสุ่มข้อสอบค่ะ</span>'
                    });
                }

                function TriggerPracticeStandardBarClick() {
                    console.log("triggerPractice");
                    openBlockUI();
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
                    console.log(p);
                    console.log(deviceId);
                    console.log(tokenId);
                    console.log(RedirectMode);
                    refreshUrl = '../PracticeMode_Pad/' + p + '?deviceuniqueid=' + deviceId + '&token=' + tokenId + '&DashboardMode=9&RedirectMode=' + RedirectMode;
                    //if (responseTimeCI != 0) return 0;
                    document.location.href = refreshUrl;//'../PracticeMode_Pad/' + p + '?deviceuniqueid=' + deviceId + '&token=' + tokenId + '&DashboardMode=9';
                }

                function TriggerStudentDetailBarClick() {
                    openBlockUI();
                    chooseSound.play();
                    refreshUrl = '../Student/StudentDetailPage.aspx' + '?deviceuniqueid=' + deviceId + '&token=' + tokenId;
                    if (responseTimeCI != 0) return 0;
                    document.location.href = refreshUrl;//'../Student/StudentDetailPage.aspx' + '?deviceuniqueid=' + deviceId + '&token=' + tokenId;
                }

                function TriggerDailyActivityBarClick() {
                    var arrayTxtActivity = ["มีกิจกรรมมาใหม่ ทำเลยมั้ยคะ ?", "เพิ่มพูนความรู้ ด้วยการทำข้อสอบประจำวันค่ะ", "ฝึกซ้อมทำข้อสอบเพิ่ม สำหรับวันนี้เลยมั้ยคะ", "แนะนำแบบฝึกหัดใหม่ค่ะ เข้าทำเลย ?"];
                    var n = Math.floor(Math.random() * arrayTxtActivity.length);
                    var txt = arrayTxtActivity[n];
                    $('#dialog').dialog('option', 'title', txt).dialog("open");

                }
    </script>
</asp:Content>
