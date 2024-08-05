<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityPage_Preview.aspx.vb" Inherits="QuickTest.ActivityPage_Preview" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link href="<%=ResolveUrl("~")%>css/jquery-ui-1.8.18.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~")%>css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~")%>css/maxonetactivity.css" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~")%>css/pad2.self.css" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~")%>css/styleQuestionAnswerExplain.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .ImgCircle {
            position: absolute;
            top: 10px;
            left: -16px;
            display: none;
        }

        #panelTime {
            margin: 5px 3px !important;
        }

        #btnMoreDetail {
            top: 3px;
            right: 16px;
            cursor: pointer;
            display: none!important;
        }

        .ForSortBtn {
            font-size: 24px !important;
            height: 50px !important;
            width: 200px !important;
        }

        .MenuBackground {
            background-color: #F08080 !important;
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

        .ForDivLeapChoice {
            margin-left: 30px;
            width: 780px;
            z-index: 999;
            height: 440px;
            position: absolute;
            background-color: #DAF7A6;
            display: block;
            margin-top: 5px;
        }

        div.MainDivNormal {
            font: normal 35px 'THSarabunNew';
            background-color: #3A8B99;
            margin-left: 0.5% !important;
            margin-top: 1% !important;
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
            background-color: transparent;
            z-index: 99;
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

        .RightDiv {
            background-color: #F08080 !important;
        }

        .hidden-scrollbar {
            overflow: hidden;
        }

            .hidden-scrollbar .inner {
                overflow: auto;
                padding-right: 45px; /* Samakan dengan besar margin negatif */
            }

        .divShowData {
            top: 40%;
            left: 50%;
            margin-top: -220px;
            margin-left: -390px;
            width: 925px;
            height: 465px;
            background-color: #DAF7A6;
            position: absolute;
            display: none;
            z-index: 9999;
            font: normal 0px "THSarabunNew";
            border-radius: 20px;
            padding-bottom: 10px;
        }

        .btnsendreport {
            cursor: pointer;
            font: normal 20px 'THSarabunNew';
            font-weight: bold;
            width: 45%;
            border: 1px solid #ccc;
            background: #F6F6F6;
            color: #1C94C4;
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

    <script src="<%=ResolveUrl("~")%>js/jspad2.std.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>shadowbox/shadowbox.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~")%>js/slides.min.jquery.js" type="text/javascript"></script>

    <script type="text/javascript">

        Shadowbox.init({
            skipSetup: false
        });

        var EPageNumber = '<%=ExplainPageNumber %>';
        var PlayerId = '<%=StudentId %>';
        var Examnum = 0;

        function CreateButtonSelectExplain() {
            var AnsMode = $('#AnsweredMode').val()

            if ($('#IsSelectedExplain').val() == 'True') {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>WebServices/ActivityService.asmx/CreateStringSelectExplain",
                    data: "{ StudentId:'" + PlayerId + "',ExamNum:'" + Examnum + "',PageNumber:'" + EPageNumber + "',AnsweredMode:'" + AnsMode + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var Data = jQuery.parseJSON(data.d);
                        $('#divEachSelectExplain').html(Data.htmlExplainAnswer);

                        var JsCheckOverOnePage = Data.CheckOverOnePage;
                        var JsCheckIsLastPage = Data.IsLastPage;

                        if (JsCheckOverOnePage > '10') {
                            if (EPageNumber == '1' || EPageNumber == '0') {
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
                        EPageNumber = JsCheckPageNumber;

                        if (AnsMode == 0) {
                            $('#spnSummary').html('ได้ ' + Data.RightAmount + '/' + Data.QuizTotalScore + ' คะแนน');
                            $('#spnSummary1').html('ทั้งหมด ' + Data.QuizTotalScore + ' ข้อ');
                            $('#spnSummary2').html('ตอบถูก ' + Data.RightAmount + ' ข้อ');
                            $('#spnSummary3').html('ตอบผิด ' + Data.WrongAmount + ' ข้อ');
                            $('#spnSummary4').html('ข้ามไม่ตอบ ' + Data.SkipAmount + ' ข้อ');
                        }

                        $('#ArrNo').val(Data.ArrNo);
                        $('#LastQuestionNo').val(Data.LastQuestionNo);

                        if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                            $('#MyShadow').show();
                            $('#divSelectExplain').stop().show();
                            $('#Hider').stop().show();
                        } else {
                            $('#MyShadow').show();
                            $('#divSelectExplain').stop().show(500);
                            $('#Hider').stop().show(500);
                        }
                    },
                    error: function myfunction(request, status) { }
                });
            } else {
                CloseDivSelectExplain();
            }
        }

        function OpenDivReportExam() {
            if (/Android|webOs|ihPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
                $('#MyShadow').show();
                $('#DivReportExam').stop().show();
            } else {
                $('#MyShadow').show();
                $('#DivReportExam').stop().show(500);
            }
        }

        function CloseDivSelectExplain() {
            $('#IsSelectedExplain').val('False');
            $('#divSelectExplain').css("display", "none");
            $('#MyShadow').css("display", "none");
            $('#Hider').css("display", "none");
        }

        function CloseDivReportExam() {
            $('#DivReportExam').css("display", "none");
            $('#MyShadow').css("display", "none");
        }

        function LeapChoiceOnclick(QQ_No, IsScore, NotReplyMode, OrderNo) {
            $('#ExamNo').val(QQ_No);
            $('#OrderNo').val(QQ_No - 1);
            $('#IsSelectedExplain').val('False');
            $('#IsJumpTo').val('true');
            form1.submit();
        }

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

        (function ($) {
            $.fn.hasVerticalScrollBar = function () {
                return this.get(0) ? this.get(0).scrollHeight > this.innerHeight() : false;
            }
            $.fn.GetScrollHeight = function () {
                return this.get(0).scrollHeight;
            }
        })(jQuery);

        $(function () {
            $('#NextExplainPage').click(function () {
                EPageNumber = parseInt(EPageNumber) + 1;
                CreateButtonSelectExplain();
            });
            $('#BackExplainPage').click(function () {
                EPageNumber = parseInt(EPageNumber) - 1;
                CreateButtonSelectExplain();
            });
            $('#btnMoreDetail').toggle(function () {
                $('#DivMoreDetailMenu').stop().show(500);0
                $('#DivMoreDetailMenu').stop().hide(500);
            });
            $('#panelSendComment').click(function () {
                OpenDivReportExam();
            });
            $('#mainQuestion').each(function () {
                $(this).hover(function () {
                    return;
                });
            });
            $('#btnAllQuestion').click(function () {
                $('#AnsweredMode').val(0);
                CreateButtonSelectExplain()
                SetActiveModeButton();
            });

            $('#btnRightQuestion').click(function () {
                $('#AnsweredMode').val(1);
                CreateButtonSelectExplain()
                SetActiveModeButton();
            });

            $('#btnWrongQuestion').click(function () {
                $('#AnsweredMode').val(2);
                CreateButtonSelectExplain()
                SetActiveModeButton();
            });

            $('#btnSkipQuestion').click(function () {
                $('#AnsweredMode').val(3);
                CreateButtonSelectExplain()
                SetActiveModeButton();
            });
        });
    </script>

</head>

<body style="height: 100%; color: #444;">
    <form id="form1" runat="server" style='margin: -8px; height: 100%;'>
        <asp:HiddenField ID="ExamNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="IsSelectedExplain" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="IsJumpTo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="AnsweredMode" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="ArrNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="OrderNo" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="LastQuestionNo" runat="server" ClientIDMode="Static" />
        <span id="voice" style="display: none;"></span>
        <div id="MyShadow" style="background-color: gray; opacity: 0.7; width: 100%; height: 120%; position: absolute; z-index: 9998; display: none; }"></div>
        <div id="dialog"></div>

        <div id='MainDivPad' class='MainDivNormal' runat='server' style='width: 100%; height: 100%; position: absolute;'>

            <div id="DivPlayground" style="position: relative; width: 89%; height: 100%;">

                <div id="DivTopMenu">
                    <div>
                        <table>
                            <tr>
                                <td style="width:15%;">
                                    <div id="panelTime" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center; font-weight: bold;">
                                        <span style="font-size: 30px;">ทบทวน</span>
                                    </div>
                                </td>
                                <td style="width:auto;">
                                    <div id="PanelMakeToDestination" class="MenuBackground" style="padding-left: 10px;height: 68px;border-radius: 5px;">
                                        <asp:Label ID="lblTestsetName" runat="server" Font-Size="22px"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:5%;">
                                    <div id="panelShowExplain" class="MenuBackground" style="height: 68px; border-radius: 5px; text-align: center;">
                                        <div id="btnAnswerExpNew">
                                            <img src="../Images/Activity/SelectExplain/ShowExplain.png" style="width: 80%; padding-top: 10px; cursor: pointer;" />
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

                        <div style="clear: both;"></div>
                    </div>

                    <div id='divSelectExplain' runat="server" class="divShowData">

                        <div id='CloseDivSelectExplain' onclick="CloseDivSelectExplain();"></div>

                        <div id='divEachSelectExplain' class='slides_container' style='height: 300px; font: normal 0px "THSarabunNew";'></div>

                        <div id="divSummary">
                            <table style="margin-left: 12px; position: absolute;">
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

                    <div id='DivReportExam' runat="server" class="divShowData" style="background-color: #fbefe6; width: 600px; margin-left: -235px;">

                        <div id='CloseDivReportExam' onclick="CloseDivReportExam();"></div>

                        <div id="divReportDetail" style="font-size: 20px;">
                            <table style="width: 98%; padding-left: 20px; padding-top: 11px;">
                                <tr>
                                    <td><span>ปัญหา</span></td>
                                    <td>
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
                                    <td><span>รายละเอียด</span></td>
                                    <td id="tdReportDetail" runat="server">
                                        <textarea rows="4" cols="1" id="txtHidden" name="HiddenId" style="display: none;" runat="server"></textarea>
                                        <textarea rows="4" cols="1" id="txtDescript" name="descript" style="resize: none; width: 375px; height: 270px; font: normal 18px THSarabunNew;"></textarea>
                                    </td>
                                    <td style="color: red;">****</td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Button ID="btnSubmit" runat="server" Text="แจ้งปัญหา" class="btnsendreport" />
                                    </td>
                                </tr>
                            </table>
                        </div>


                    </div>
                </div>

                <div id="DivMainQuestionAndAnswer" style="position: absolute; height: 88%; width: 100%;">

                    <div id='DivMainQuestionAnswer' runat="server" class='MainQuestionAnswerNormal'>

                        <div id="mainQuestion" runat="server" class='QuestionNormal QuestionBackground'>
                            <div id="btnMoreDetail" style="display:none!important;"></div>
                            <div id="DivMoreDetailMenu" style="width: 100px; height: 200px; z-index: 1; position: absolute; right: 12px; border-radius: 5px; top: 7px; display: none;">
                                <div id="btnQuestionExp"></div>
                                <div id="btnSendComment"></div>
                            </div>
                            <table id="QuestionTbl" runat="server" style="border-collapse: collapse; padding-top: 10px;">
                                <tr>
                                    <td runat="server" id="QuestionTd"></td>
                                </tr>
                            </table>
                        </div>

                        <div id="mainAnswer" runat="server" style="width: 98%; position: relative; background-color: #F4F7FF; padding: 20px; border-radius: 0 0 10px 10px; min-height: 65%;">

                            <% If IsNoAnswer Then%>
                            <img id="ImgSkip" src="../Images/Activity/skip.png" class="ImgSkip" />
                            <%End If%>

                            <div id="btnAnswerExp"></div>

                            <div id="SwapAnswer" style="line-height: 2; background-color: transparent; width: 130px; float: right; display: none;">
                                <img src="../Images/Activity/swap.png" alt="" />
                            </div>

                            <table id="Table1" runat="server" style="border-collapse: collapse; width: 100%;">
                                <tr>
                                    <td runat="server" id="AnswerTbl"></td>
                                </tr>
                            </table>
                            <div id="AnswerExp" runat="server" clientidmode="Static" style="font-size: 24px; display: none;">
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <div id="DivRightMenu" style="right: 0; margin-top: 5px; position: absolute; height: 100%; top: 0; width: 9%; margin-right: 18px;">
                <%--        <div style="float: right; margin-top: -73px; z-index: 999; position: relative; background-color: #3A8B99;">--%>

                <div id='DivScored' class="RightDiv MenuBackground" style="height: 18%; cursor: pointer; font-weight: bold; margin-right: 10px;">
                    <div>
                        <asp:Image ID="imgAnsweredMode" runat="server" AlternateText="" ImageAlign="Middle" Width="40" Height="40" ImageUrl="../Images/Activity/SelectExplain/AllQuestion.png" />

                        <%--<img src="../Images/Activity/SelectExplain/AllQuestion.png" runat="server" id="imgAnsweredMode" style="width: 40%; padding-top: 10px;" />--%>
                    </div>
                    <div style="border-top: 1px solid; padding-top: 10px">
                        <asp:Label ID="lblScore" runat="server" Style="position: relative;">5</asp:Label>/
                        <asp:Label ID="lblSumScore" runat="server" Style="position: relative;">10</asp:Label>
                    </div>
                </div>

                <div id="BtnNext" class="RightDiv MenuBackground" style="height: 30%; background: url(../Images/Activity/bnext.png) center center no-repeat; background-size: 70%; margin-right: 10px;">
                    <asp:Button ID="btnNext" runat="server" Height="100%" Width="100%" ClientIDMode="Static" Style="background: transparent; border: none; cursor: pointer;" />
                </div>


                <div id="BtnPrev" class="RightDiv MenuBackground" style="height: 30%; background: url(../Images/Activity/bback.png) center center no-repeat; background-size: 70%; margin-right: 10px;">
                    <asp:Button ID="btnPrev" runat="server" Height="100%" Width="100%" Style='background: transparent; border: none; cursor: pointer;' />
                </div>


                <div id="BtnExitQuiz" class="RightDiv MenuBackground" style="margin-right: 7px; cursor: pointer; height: 16%; background-size: 70%; margin-right: 10px;">
                    <%--<img src="../Images/activity/logout.png" style="margin-top: <%=SizeTopForImgExit%>px; margin-left: 9px;" />--%>
                </div>
            </div>

        </div>

    </form>

    <script type="text/javascript">       // ปุ่มสลับเฉลย - กับที่ตอบ
        var resetHtml = true;
        $(function () {

            var CorrectAnswer = ('<%= CorrectAnswer %>').toString();
            var MyAnswer = ('<%= MyAnswer%>').toString();

            if (CorrectAnswer != "") {
                $('#SwapAnswer').show();
            }



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

            $('#DivScored').click(function () {
                PageNumber = 0;
                $('#IsSelectedExplain').val('True');
                CreateButtonSelectExplain();
                SetActiveModeButton();
            });

        });


    </script>

    <script type="text/javascript">

        var deviceId = '<%=DeviceUinqueId%>';
        var token = '<%=Token%>';

        //var examNo = parseInt(")
        $(function () {
            var examNo = parseInt($('#ExamNo').attr('value'));
            var lastExamNo = $('#LastQuestionNo').val();

            CreateButtonSelectExplain();

            $('#btnPrev').click(function (e) {
                if (examNo === 1) {
                    e.preventDefault();
                    callAlertDialog('ข้อที่ 1 แล้วค่ะ ไม่มีข้อก่อนหน้าค่ะ');
                }
            });

            $('#btnNext').click(function (e) {
                if (examNo == lastExamNo) {
                    e.preventDefault();
                    callConfirmDialog('ข้อสุดท้ายแล้วค่ะ จะออกเลยหรือว่าทบทวนต่อคะ');
                }
            });

            $('#BtnExitQuiz').click(function () {
                callConfirmDialog('ต้องการออกจาก preview ข้อสอบเลยหรือเปล่าคะ');
            });



            var AnswerExpType;
            if ($('#AnswerExp').children('div').length > 0) { AnswerExpType = 1; }
            if ($('ul > li').children('div.Correct').length > 0 || $('ul > li').children('div.InCorrect').length > 0) { AnswerExpType = 6; }

            if ($('#QuestionExp').length > 0 || $('#AnswerExp').children('div').length > 0) { $('#btnQuestionExp').show(); }

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

        function callAlertDialog(titleName) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }

        function callConfirmDialog(titleName) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ยังก่อน"] = function () {
                $d.dialog('close');
            };
            myBtn["ออกเลย"] = function () {
                $d.dialog('close');
                parent.location.href = '../Student/StudentDetailPage.aspx?deviceuniqueid=' + deviceId + '&token=' + token;
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }
    </script>
</body>

</html>
