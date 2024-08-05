<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ShowScore.aspx.vb" Inherits="QuickTest.ShowScore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <style type="text/css">
        .coin {
            width: 70px;
            border: 1px solid #BBBBBB;
            background-color: #EFEFEF;
            text-align: right;
            padding: 0 10px 0 60px;
            background-repeat: no-repeat;
            background-size: 45px;
            background-position: center left 5px;
            border-radius: .5em;
            font-size: 30px;
        }

        #SilverCoin {
            float: left;
            background-image: url('../images/Activity/ShowScore/silver.png');
        }

        #GoldCoin {
            float: right;
            background-image: url('../images/Activity/ShowScore/gold.png');
            background-color: rgb(241, 174, 32);
        }

        .ForDivMinigame {
            width: 145px;
            height: 120px;
            background: #1EC9F4;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            border: solid 1px #0D8AA9;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top, #63CFDF, #17B2D9);
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            display: inline-block;
            margin-left: 6px;
            margin-right: 6px;
            border-radius: 6px;
            font-size: 25px;
            line-height: 120px;
            margin-top: 20px;
            position: relative;
        }

        #DivBlock {
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.8);
            position: absolute;
            top: 0;
            text-align: center;
        }

        #MainDivMinigame {
            width: 670px;
            height: 390px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 50px;
        }

        #ShowTotalReward {
            margin-left: auto;
            margin-right: auto;
            display: none;
            position: relative;
        }

        .ForRewardMinigame {
            width: 145px;
            height: 120px;
            background-color: #E78D4A;
            display: none;
            margin-left: 6px;
            border: solid;
            margin-right: 6px;
            border-color: #FF7F46;
            border-radius: 6px;
            color: white;
            font-size: 25px;
            line-height: 120px;
            margin-top: 20px;
            position: relative;
            vertical-align: top;
        }

        #DivHeadMinigame {
            font-size: 50px;
            font-weight: bold;
            color: white;
        }

        .ForDivShowTotalReward {
            text-align: center;
            width: 240px;
            height: 301px;
            display: inline-block;
            font-size: 40px;
            color: #790627;
            font-weight: bold;
            margin-left: 10px;
            border-radius: 6px;
            margin-top: 145px;
            line-height: 105px;
            position: relative;
        }

        .ForSpntxt {
            position: absolute;
            left: 53px;
            color: black;
            font-size: 40px;
            font-weight: bold;
        }

        .ForImgReward {
            position: absolute;
            left: 17px;
            width: 110px;
        }

        .ForImgStop {
            position: absolute;
            left: 17px;
            top: -20px;
            width: 110px;
        }

        .ForSpntxtBigSize {
            position: relative;
            left: 53px;
            top: -235px;
            font-size: 80px;
            left: 10px;
            color: white;
            font-weight: bold;
        }

        .ScoreDiv {
            width: 120px;
            margin-left: auto;
            margin-right: auto;
            border-color: #FF7F46;
            border-style: Solid;
            background-color: #E78D4A;
            text-align: center;
            border-radius: 10px;
            line-height: 33px;
        }

        .ScoreDivNoneBackground {
            width: 120px;
            margin-left: auto;
            margin-right: auto;
            border-color: #BF7D57;
            border-style: Dotted;
            text-align: center;
            border-radius: 10px;
            line-height: 33px;
        }

        .SectionBar {
            width: 273px;
            /*height: 370px;*/
            vertical-align: bottom;
            display: table-cell;
            border-color: #291E1D;
            color: #790627;
            font-weight: bold;
        }

        #WinPanel {
            width: 330px;
            height: 150px;
            background-image: url(../Images/Activity/ShowScore/ShowScoreWinPanel.png);
            position: absolute;
            margin-top: -50px;
            margin-left: auto;
            margin-right: auto;
            left: 0;
            right: 0;
            background-size: cover;
        }

        .AvatarBG {
            height: 150px;
            position: absolute;
        }

        .winner {
            width: 60px;
            height: 120px;
            margin-top: 8px;
        }

        .loser {
            width: 50px;
            height: 70px;
            margin-top: 35px;
        }

        .divTxtChallenge {
            margin-top: auto;
            margin-bottom: auto;
            font-size: 25px;
            color: white;
            width: 220px;
            position: absolute;
            top: 0;
            bottom: 0;
            left: 60px;
            text-align: center;
            line-height: 150px;
        }

        .divCover {
            display: flex;
            height: 250px;
        }

        .divCoverBottom {
            /*display: table;*/
            width: 25%;
            margin: 0 40px;
            height: 100%;
            position: relative;
        }

        .divBottom {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
        }
    </style>

    <%If IsMobile = True Then%>
    <style type="text/css">
        /*#WinPanel {
            margin-left: 26% !important;
        }*/

        #btnNextPage {
            height: 70px !important;
            font-size: 30px !important;
            line-height: 70px !important;
        }

        #site_content {
            /*height: 599px !important;*/
            padding: 5px 12px 0px 12px !important;
        }

        .content {
            margin: 0 0 10px 0 !important;
        }
    </style>
    <%End If%>
    <%If Not IsMaxOnet Then %>
    <style type="text/css">
        #site_content {
            border-radius: 10px;
        }
    </style>
    <%End If %>
    <style type="text/css">
        .flex {
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
        }

            .flex.flex--reverse {
                -webkit-box-orient: horizontal;
                -moz-box-orient: horizontal;
                -webkit-box-direction: reverse;
                -moz-box-direction: reverse;
                -webkit-flex-direction: row-reverse;
                -ms-flex-direction: row-reverse;
                flex-direction: row-reverse;
            }

        .flex--row {
            -webkit-box-orient: vertical;
            -moz-box-orient: vertical;
            -webkit-box-direction: normal;
            -moz-box-direction: normal;
            -webkit-flex-direction: column;
            -ms-flex-direction: column;
            flex-direction: column;
        }

            .flex--row.flex--reverse {
                -webkit-box-orient: vertical;
                -moz-box-orient: vertical;
                -webkit-box-direction: reverse;
                -moz-box-direction: reverse;
                -webkit-flex-direction: column-reverse;
                -ms-flex-direction: column-reverse;
                flex-direction: column-reverse;
            }

        .flex--justify-content--space-between {
            -webkit-box-pack: justify;
            -moz-box-pack: justify;
            -ms-flex-pack: justify;
            -webkit-justify-content: space-between;
            justify-content: space-between;
        }

        .flex--justify-content--space-around {
            -webkit-box-pack: justify;
            -moz-box-pack: justify;
            -ms-flex-pack: distribute;
            -webkit-justify-content: space-around;
            justify-content: space-around;
        }

        .flex--justify-content--center {
            -webkit-box-pack: center;
            -moz-box-pack: center;
            -ms-flex-pack: center;
            -webkit-justify-content: center;
            justify-content: center;
        }

        .flex--justify-content--start {
            -webkit-box-pack: start;
            -moz-box-pack: start;
            -ms-flex-pack: start;
            -webkit-justify-content: flex-start;
            justify-content: flex-start;
        }

        .flex--justify-content--end {
            -webkit-box-pack: end;
            -moz-box-pack: end;
            -ms-flex-pack: end;
            -webkit-justify-content: flex-end;
            justify-content: flex-end;
        }

        .flex--align-items--start {
            -webkit-box-align: start;
            -moz-box-align: start;
            -ms-flex-align: start;
            -webkit-align-items: flex-start;
            align-items: flex-start;
        }

        .flex--align-items--end {
            -webkit-box-align: end;
            -moz-box-align: end;
            -ms-flex-align: end;
            -webkit-align-items: flex-end;
            align-items: flex-end;
        }

        .flex--align-items--center {
            -webkit-box-align: center;
            -moz-box-align: center;
            -ms-flex-align: center;
            -webkit-align-items: center;
            align-items: center;
        }

        .flex--align-items--baseline {
            -webkit-box-align: baseline;
            -moz-box-align: baseline;
            -ms-flex-align: baseline;
            -webkit-align-items: baseline;
            align-items: baseline;
        }

        .flex--align-items--stretch {
            -webkit-box-align: stretch;
            -moz-box-align: stretch;
            -ms-flex-align: stretch;
            -webkit-align-items: stretch;
            align-items: stretch;
        }

        .flex--flex-grow-all > * {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex--flex-grow-last :last-child {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex--flex-grow-middle :nth-child(2) {
            -webkit-box-flex: 1;
            -moz-box-flex: 1;
            -webkit-flex-grow: 1;
            -ms-flex-grow: 1;
            flex-grow: 1;
        }

        .flex-center-wrapper {
            display: -webkit-box;
            display: -moz-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            -webkit-box-pack: center;
            -moz-box-pack: center;
            -ms-flex-pack: center;
            -webkit-justify-content: center;
            justify-content: center;
            -webkit-box-align: center;
            -moz-box-align: center;
            -ms-flex-align: center;
            -webkit-align-items: center;
            align-items: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <form runat="server" id='form1'>
        <div id="main" style="margin: 20px auto 0 auto;">
            <div id="site_content" style="width: 930px;">
                <div class="content" style="width: 930px; padding: 0;">
                    <div style="width: 910px; height: 50px; padding-left: 10px; padding-right: 10px;">
                        <span id="voice"></span>
                        <canvas id="coinAnimation" style="position: absolute; display: none; right: 25em;"></canvas>
                        <canvas id="coinSilverAnimation" style="position: absolute; display: none; left: 25em;"></canvas>
                        <script src="../js/sprite-animation-demo.js" type="text/javascript"></script>
                        <div id="SilverCoin" class="coin">
                        </div>
                        <div id="GoldCoin" class="coin">
                        </div>
                    </div>

                    <h2 style="padding: 0; margin-top: -22px;">
                        <div id="WinPanel">
                            <%-- <div id="MyAVT" style="width: <%= WidthLeftAvt%>; height: <%= HeightLeftAvt%>; margin-left: <%=MarginLLeftAvt%>; margin-top: <%=MarginTLeftAvt%>; position: absolute;">
                                <img src="<%= imgLeft%>" style="width: <%= WidthLeftAvt%>; height: <%= HeightLeftAvt%>;" />
                            </div>
                            <div id="txtAvt" style="margin-top: <%=MarginTTxtAvt%>; font-size: 25px; color: white; width: 109px; margin-left: <%=MarginLTxtAvt%>; position: absolute;">
                                <%= Wintext%>
                            </div>

                            <div id="PreAvt" style="width: <%= WidthRighttAvt%>; height: <%= HeightRightAvt%>; margin-left: <%=MarginLRightAvt%>; margin-top: <%=MarginTRightAvt%>; position: absolute;">
                                <img src="<%= imgRight%>" style="width: <%= WidthRighttAvt%>; height: <%= HeightRightAvt%>;" />
                            </div>--%>

                            <div id="MyAVT" class="AvatarBG" style="left: 10px;">
                                <img id="ImgMyAvatar" runat="server" clientidmode="Static" />
                            </div>
                            <div id="txtAvt" class="divTxtChallenge">
                                <%= Wintext%>
                            </div>

                            <div id="PreAvt" class="AvatarBG" style="right: 10px;">
                                <img id="ImgOtherAvatar" runat="server" clientidmode="Static" />
                            </div>

                        </div>


                    </h2>
                    <div id="div-1" style="width: 900px; margin-left: auto; margin-right: auto; margin: 0 10px; padding: 60px 5px 0 5px; border-radius: 0;">

                        <div class="divCover flex">
                            <div class="divCoverBottom">
                                <%If ModeOfShowScore = "3" Then%>
                                <div class="divBottom">
                                    <div style="width: 120px; height: 80px; margin-left: auto; margin-right: auto; text-align: center;">
                                        <%If IsMaxOnet Then %>
                                        <img alt="ดูคะแนน" src="../images/Activity/ShowScore/AvgScore.png" style="width: 80px; height: 80px;" />
                                        <% Else %>
                                        <img alt="ดูคะแนน" src="../images/Activity/ShowScore/avg.png" style="height: 80px;" />
                                        <%End If %>
                                    </div>
                                    <div id="Bar1" class="ScoreDiv">
                                        เฉลี่ยห้อง
                                        <br />
                                        <%= AvgScore%>
                                    </div>
                                </div>
                                <% End If%>
                            </div>
                            <div class="divCoverBottom">
                                <div class="divBottom">
                                    <div id="myScore" style="width: 120px; height: 80px; margin-left: auto; margin-right: auto; text-align: center;">
                                        <%If IsMaxOnet Then %>
                                        <img src="<%= Img%>" style="width: 80px; height: 80px;" />
                                        <% Else %>
                                        <img src="../images/Activity/ShowScore/my.png" style="height: 80px;" />
                                        <%End If %>
                                    </div>
                                    <div id="Bar2" class="ScoreDiv">
                                        ทำได้
                                        <br />
                                        <%= MyScore%>
                                    </div>
                                </div>
                            </div>
                            <div class="divCoverBottom">
                                <%If ModeOfShowScore = "3" Then%>
                                <div class="divBottom">
                                    <div id="topScore" style="width: 120px; height: 80px; margin-left: auto; margin-right: auto; text-align: center;">
                                        <%If IsMaxOnet Then %>
                                        <img alt="ดูคะแนน" src="../images/Activity/ShowScore/MaxScore.png" style="width: 80px; height: 80px;" />
                                        <% Else %>
                                        <img alt="ดูคะแนน" src="../images/Activity/ShowScore/max.png" style="height: 80px;" />
                                        <%End If %>
                                    </div>
                                    <div id="Bar3" class="ScoreDiv">
                                        สูงสุด
                                        <br />
                                        <%= MaxScore%>
                                    </div>
                                </div>
                                <% End If%>
                            </div>
                        </div>
                    </div>
                    <div style="text-align: center; font-weight: bold; font-size: 28px; color: #863D06; border-bottom: 0; padding: 0; background: #D3F2F7; width: 910px; margin-left: auto; margin-right: auto; border-radius: 0 0 0.5em 0.5em;">
                        จาก <%= FullScore %> คะแนน
                    </div>
                    <div style="margin-top: 10px; text-align: right; padding-right: 10px;">
                        <asp:Button ID="btnNextPage" ClientIDMode="Static" runat="server" class="submitChangeFontSize" Text="ไปต่อ >>"
                            Style="width: 200px; margin: 0; position: relative;"></asp:Button>
                    </div>
                </div>
            </div>
            <%--<img class="clock" src="../Images/Maxonet/clock.png" />--%>
        </div>
        <div id="DivBlock" runat="server" clientidmode="Static">
            <div id="MainDivMinigame" clientidmode="Static" runat="server">
            </div>
            <div id="ShowTotalReward">
                <div id="divRewardSilver" class="ForDivShowTotalReward">
                    <img src="../Images/Activity/Minigame/CoinSummarySilver.png" />
                    <span id="SpnRewardSilver" class="ForSpntxtBigSize"></span>
                </div>
                <div id="divRewardGold" class="ForDivShowTotalReward">
                    <img src="../Images/Activity/Minigame/CoinSummaryGold.png" />
                    <span id="SpnRewardGold" class="ForSpntxtBigSize"></span>
                </div>
                <div id="divRewardDiamond" class="ForDivShowTotalReward">
                    <img src="../Images/Activity/Minigame/CoinSummaryDiamond.png" />
                    <span id="SpnRewardDiamond" class="ForSpntxtBigSize"></span>
                </div>
            </div>
        </div>

        <input type="hidden" runat="server" id="IsSetPoint" />
        <input type="hidden" id="hdsilver" value="0" />
        <input type="hidden" id="hdgold" value="0" />
        <input type="hidden" id="hddiamond" value="0" />

        <audio id="audioOpen" style="display: none;">
            <%--<source src="<%=ResolveUrl("~")%>images/open.mp3" />--%>
            <source src="<%=ResolveUrl("~")%>sounds/maxonet/select.mp3" />
        </audio>

        <audio id="audioDie" style="display: none;">
            <source src="<%=ResolveUrl("~")%>images/die.mp3" />            
        </audio>

        <audio id="audioCoin" style="display: none;" autobuffer controls autoplay>
            <%--<source src="<%=ResolveUrl("~")%>images/coin.mp3" />--%>
        </audio>

    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/slimScroll.js" type="text/javascript"></script>
    <%--<script src="../js/jquery.mobile-1.4.2.min.js" type="text/javascript"></script>
    <link href="../css/jquery.mobile-1.4.2.min.css" rel="stylesheet" type="text/css" />--%>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script type="text/javascript">
        var PlayerId = '<%=PlayerId %>';
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var queue = [];
        var tempelement;
        $(document).ready(function () {

            //เปิดป้ายรับรางวัล Minigame
            //$('.ForDivMinigame').each(function () {
            //    new FastButton(this, GetPriceMiniGame);
            //});
            $('.ForDivMinigame').on('click', function (e) {
                GetPriceMiniGame(e);
            });
            //$('.ForDivMinigame').on('touchstart', function (e) {
            //    if (queue.length > 0) { return 0; }
            //    tempelement = $(this);
            //    queue.push(tempelement);
            //});
            //$('.ForDivMinigame').on('touchend', function (e) {
            //    if (queue.length == 0) { return 0; }
            //    if ($(this).is($(tempelement))) {
            //        GetPriceMiniGame(e);
            //        queue.shift();
            //    }                              
            //});

            //ปุ่ม ไปต่อ
            new FastButton(document.getElementById('btnNextPage'), TriggerBtnNextClick);


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Code ส่วนของเปิดป้ายของรางวัลหลังฝึกฝน
            //$('.ForDivMinigame').on('tap',function () {
            //    $(this).hide();
            //    $('#' + $(this).attr('RewardId')).css('display', 'inline-block');
            //    var reward = $('#' + $(this).attr('RewardId')).attr('reward');
            //    var splitestr = reward.split("|");
            //    if (splitestr[0] == 'gold') {
            //        playsoundOpen();
            //        var totalgold = parseInt($('#hdgold').val()) + parseInt(splitestr[1]);
            //        $('#hdgold').val(totalgold);
            //    }
            //    else if (splitestr[0] == 'silver') {
            //        playsoundOpen();
            //        var totalsilver = parseInt($('#hdsilver').val()) + parseInt(splitestr[1]);
            //        $('#hdsilver').val(totalsilver);
            //    }
            //    else if (splitestr[0] == 'diamond') {
            //        playsoundOpen();
            //        var totaldiamond = parseInt($('#hddiamond').val()) + parseInt(splitestr[1]);
            //        $('#hddiamond').val(totaldiamond);
            //    }
            //    else if (splitestr[0] == 'stop') {
            //        //เมื่อกดโดนปุ่มหยุด
            //        //เปิดทุกป้าย + รอ 2 วิ
            //        $('.ForDivMinigame').hide();
            //        $('.ForRewardMinigame').css('display', 'inline-block');
            //        //เล่นไฟล์เสียง
            //        playsoundDie();
            //        setTimeout(function () {
            //            //โชว์ยอด
            //            $('#SpnRewardSilver').text('X' + $('#hdsilver').val());
            //            $('#SpnRewardGold').text('X' + $('#hdgold').val());
            //            $('#SpnRewardDiamond').text('X' + $('#hddiamond').val());
            //            $('#MainDivMinigame').hide();
            //            $('#ShowTotalReward').show();
            //            playSound();
            //            setTimeout(function () {
            //               $('#DivBlock').hide();
            //                var studentid = '<%=PlayerId %>';
            //               $.ajax({
            //                    type: "POST",
            //                    url: "<%=ResolveUrl("~")%>Activity/ShowScore.aspx/UpdateCoinBeforeMinigameEnd",
            //                    data: "{ StudentId: '" + studentid + "',Gold: '" + $('#hdgold').val() + "',Silver: '" + $('#hdsilver').val() + "',Diamond: '" + $('#hddiamond').val() + "'}",
            //                    async: false,
            //                    contentType: "application/json; charset=utf-8", dataType: "json",
            //                    success: function (data) {
            //                        if (data.d != '' || data.d != 0) { //session(UserId) หายจะ return 0 - Sefety function
            //                            var Getcoin = jQuery.parseJSON(data.d);
            //                            $('#SilverCoin').html(Getcoin[0].Silver);
            //                            $('#GoldCoin').html(Getcoin[0].Gold);
            //                            CoinUp($('#hdsilver').val(), $('#SilverCoin'), false);
            //                            CoinUp($('#hdgold').val(), $('#GoldCoin'), true);
            //                            //init();
            //                        }
            //                    },
            //                    error: function myfunction(request, status) {
            //                    }
            //                });
            //            }, 5000);
            //        }, 3000);
            //    }
            //})
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var Mys = '<%= MyScore %>';
            var Avgs = '<%= AvgScore %>';
            var Mxs = '<%= MaxScore %>';            

            var fullScore = '<%= FullScore %>';
                        
            if (Mys == '0') {
                //alert(Mys);
                document.getElementById("Bar2").setAttribute("Class", "ScoreDivNoneBackground");
            }
            if (document.getElementById("Bar1") != null) {
                if (Avgs == '0') {
                    //alert(Avgs);     
                    document.getElementById("Bar1").setAttribute("Class", "ScoreDivNoneBackground");
                }
                if (Mxs == '0') {
                    document.getElementById("Bar3").setAttribute("Class", "ScoreDivNoneBackground");
                }
            }

            var h = ($(window).height() < window.screen.height) ? $(window).height() : window.screen.height;
            //console.log('h = ' + h);            
            h = h - 110; // 110 คือค่าที่ปรับให้ความสูงของ div แล้วพอดีกับหน้าจอ ในขนาด 360px
            h = (h > 250) ? 250 : h;            
            $('.divCover').height(h); // ความสูงของ div สีฟ้า
            var defaultHeight = 60; // ความสูงแบบไม่มีค่า ของแท่งกราฟ
            var dhg = (h - 130) - defaultHeight; // defaultHeightOfGraph
            console.log(dhg);
            //console.log('dhg = ' + dhg);
            var hBar1 = Math.round(((Avgs * dhg) / fullScore) * 2, 0) + defaultHeight; //console.log(hBar1);
            var hBar2 = Math.round(((Mys * dhg) / fullScore) * 2, 0) + defaultHeight; //console.log(hBar2);
            var hBar3 = Math.round(((Mxs * dhg) / fullScore) * 2, 0) + defaultHeight; //console.log(hBar3);

            $('#Bar1').height(hBar1);
            $('#Bar2').height(hBar2); if (hBar2 > 150) { $('#myScore').children().css({ 'margin-top': '160px' }); } //if (hBar2 > (dhg + 10)) $('#myScore').children().css({ 'margin-top': '160px' });
            $('#Bar3').height(hBar3);
        });

        var isMaxOnet = '<%=IsMaxOnet%>';
       
        function TriggerBtnNextClick(e) {
            if (isMaxOnet === 'True') {
                openBlockUI();
                nextSound.play();
                refreshUrl = '<%=NextUrl%>';                
                if (responseTimeCI != 0) { return 0; }
            }
            var obj = e.target;
            $(obj).trigger('click');
        }

        
        function GetPriceMiniGame(e) {           

            var obj = e.srcElement;
            $(obj).hide();
            $('#' + $(obj).attr('RewardId')).css('display', 'inline-block');
            var reward = $('#' + $(obj).attr('RewardId')).attr('reward');
            var splitestr = reward.split("|");
            if (splitestr[0] == 'gold') {
                playsoundOpen();
                var totalgold = parseInt($('#hdgold').val()) + parseInt(splitestr[1]);
                $('#hdgold').val(totalgold);
            }
            else if (splitestr[0] == 'silver') {
                playsoundOpen();
                var totalsilver = parseInt($('#hdsilver').val()) + parseInt(splitestr[1]);
                $('#hdsilver').val(totalsilver);
            }
            else if (splitestr[0] == 'diamond') {
                playsoundOpen();
                var totaldiamond = parseInt($('#hddiamond').val()) + parseInt(splitestr[1]);
                $('#hddiamond').val(totaldiamond);
            }
            else if (splitestr[0] == 'stop') {
                //เมื่อกดโดนปุ่มหยุด
                //เปิดทุกป้าย + รอ 2 วิ
                $('.ForDivMinigame').hide();
                $('.ForRewardMinigame').css('display', 'inline-block');
                //เล่นไฟล์เสียง
                playsoundDie();
                setTimeout(function () {
                    //โชว์ยอด
                    $('#SpnRewardSilver').text('X' + $('#hdsilver').val());
                    $('#SpnRewardGold').text('X' + $('#hdgold').val());
                    $('#SpnRewardDiamond').text('X' + $('#hddiamond').val());
                    $('#MainDivMinigame').hide();
                    $('#ShowTotalReward').show();
                    playSound();
                    setTimeout(function () {
                        $('#DivBlock').hide();
                        var studentid = '<%=PlayerId %>';
                        var NeedShowTip = '<%=NeedShowTip%>';
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>Activity/ShowScore.aspx/UpdateCoinBeforeMinigameEnd",
                            data: "{ StudentId: '" + studentid + "',Gold: '" + $('#hdgold').val() + "',Silver: '" + $('#hdsilver').val() + "',Diamond: '" + $('#hddiamond').val() + "'}",
                            async: false,
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (data) {
                                if (data.d != '' || data.d != 0) { //session(UserId) หายจะ return 0 - Sefety function
                                    var Getcoin = jQuery.parseJSON(data.d);
                                    var silver = (Getcoin[0].Silver == undefined) ? 0 : Getcoin[0].Silver;
                                    var gold = (Getcoin[0].Gold == undefined) ? 0 : Getcoin[0].Gold;
                                    $('#SilverCoin').html(silver);
                                    $('#GoldCoin').html(gold);
                                    CoinUp($('#hdsilver').val(), $('#SilverCoin'), false);
                                    CoinUp($('#hdgold').val(), $('#GoldCoin'), true);
                                    //init();
                                    if (NeedShowTip == 'True') {
                                        ShowTips();
                                    }
                                }
                            },
                            error: function myfunction(request, status) {
                            }
                        });
                    }, 5000);
                }, 3000);
            }
           
}

//function TriggerClickNextPage(e) {
//    var obj = e.srcElement;
//    $(obj).trigger('click');
//}

    </script>
    <script type="text/javascript">
        $(function () {
            var IsSetPoint = $('#IsSetPoint').val();
            if (IsSetPoint != true) {
                //GetStudentPoint();
            }
        });
        //function GetStudentPoint() {
        //$.ajax({ type: "POST",
        //url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/GetStudentPoint",
        //data: "{ PlayerId: '" + PlayerId + "'}",
        //async: true,                              
        //contentType: "application/json; charset=utf-8", dataType: "json",
        //success: function (data) {                                    
        //var StudentPoint = jQuery.parseJSON(data.d);                     
        //$('#SilverCoin').html(StudentPoint[0].Silver);
        //$('#GoldCoin').html(StudentPoint[0].Gold);  
        //setTimeout(function() {
        //SetStudentPoint();
        //$('#IsSetPoint').val(true);
        //CoinUp(StudentPoint[0].NewSilver, $('#SilverCoin'),false);
        //CoinUp(50, $('#SilverCoin'),false);
        //CoinUp(StudentPoint[0].NewGold, $('#GoldCoin'),true); 
        //CoinUp(3, $('#GoldCoin'),true);                        
        //            init();
        //        },1500);                                               
        //    },
        //    error: function myfunction(request, status) {               
        //    }
        //});
        //}
        function SetStudentPoint() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetStudentPoint",
                data: "{ PlayerId: '" + PlayerId + "'}",
                async: true,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                },
                error: function myfunction(request, status) {
                }
            });
        }
        function CoinUp(newCoin, element, IsGoldCoin) {
            var CoinPlus = parseInt(newCoin);
            var d = 50;
            if (IsGoldCoin) {
                d = d * 3;
            }
            if (CoinPlus != 0) {
                var t = setInterval(function () {
                    var Coin = parseInt($(element).html());
                    Coin++;
                    CoinPlus--;
                    if (CoinPlus == 0) {
                        clearInterval(t);
                    }
                    $(element).html(Coin);
                }, d);
            }
        }

        function init() {
            playSound();
            $('#coinAnimation').show();
            $('#coinSilverAnimation').show();

            var tGold = setInterval(draw, 30);
            var tSilver = setInterval(drawSilver, 30);
            setTimeout(function () {
                $('#coinAnimation').hide();
                $('#coinSilverAnimation').hide();
                $('#voice').html();
                clearInterval(tGold);
                clearInterval(tSilver);
            }, 3000);
        }
        var x = 50;
        //var y = 1000;
        var dx = 10;
        var dy = 10;
        // window width height
        var width = $(window).width();
        var center = (width / 2) - 50;
        var widthLR = (center) / 2; y = ((center) + widthLR);
        console.log(width); console.log(center);
        var height = $(window).height();
        function draw() {
            var coinStyle = document.getElementById("coinAnimation").style;
            coinStyle.top = x.toString().concat("px");
            coinStyle.left = y.toString().concat("px");
            // Boundary Logic            
            //if (x < 0 || x > 500) dx = -dx;
            //if (y < 750 || y > 1500) dy = -dy;
            if (x < 0 || x > 500) dx = -dx;
            if (y < center || y > (width - 50)) dy = -dy;
            x += dx;
            y -= dy;
        }
        var x2 = 50;
        //var y2 = 500;
        var y2 = widthLR;
        var dx2 = 10;
        var dy2 = 10;
        function drawSilver() {
            var coinSilverStyle = document.getElementById("coinSilverAnimation").style;
            coinSilverStyle.top = x2.toString().concat("px");
            coinSilverStyle.left = y2.toString().concat("px");
            // Boundary Logic
            //            if (x2 < 0 || x2 > 500) dx2 = -dx2;
            //            if (y2 < 400 || y2 > 700) dy2 = -dy2;
            //            x2 += dx2;            
            //            y2 -= dy2; 
            //if (x2 < 0 || x2 > 500) dx2 = -dx2;
            //if (y2 < 0 || y2 > 750) dy2 = -dy2;
            if (x2 < 0 || x2 > 500) dx2 = -dx2;
            if (y2 < 0 || y2 > center) dy2 = -dy2;
            x2 += dx2;
            y2 += dy2;
        }
        function playSound() {
            //document.getElementById('audioCoin').innerHTML = "<source src='<%=ResolveUrl("~")%>images/coin.mp3' />";
            document.getElementById('audioCoin').innerHTML = "<source src='<%=ResolveUrl("~")%>sounds/maxonet/bgmusiccoin.mp3' />";
            var audio = document.getElementById('audioCoin')
            audio.play();
            setInterval(function () {
                audio.pause();
                audio.currentTime = 0;
                audio.play();
            }, 2900);
            //var audio = document.getElementById('audioCoin');
            //audio.play();
            //document.getElementById("voice").innerHTML =
            //"<embed src='/images/coin.mp3' hidden=\"true\" autostart=\"true\" loop=\"false\" />";
        }
        function playsoundDie() {
            var audio = document.getElementById('audioDie');
            audio.play();
            //document.getElementById("voice").innerHTML =
            //"<embed src='/images/die.mp3' hidden=\"true\" autostart=\"true\" loop=\"false\" />";
        }
        function playsoundOpen() {
            var audio = document.getElementById('audioOpen');
            audio.play();
            //document.getElementById("voice").innerHTML =
            //"<embed src='/images/open.mp3' hidden=\"true\" autostart=\"true\" loop=\"false\" />";
        }
        //        var topPos = 100;
        //        var leftPos = 900;
        //        var dx = 10;
        //        var dy = 10;
        //        var a = 0; b=500;
        //        var c;i = 1;
        //        function draw() {
        //            var coinStyle = document.getElementById("coinAnimation").style;
        //            coinStyle.top = topPos.toString().concat("px");
        //            //coinStyle.left = leftPos.toString().concat("px"); 
        //                       
        //            if (topPos < a || topPos > b)  dx = -dx; if (topPos > b) { c = 100 * i; a = c; i++;}  
        //            if (leftPos < 700 || leftPos > 1000) dy = -dy;
        //            topPos += dx;
        //            leftPos += dy;     
        //                   
        //        }        
    </script>
    <script type="text/javascript">
        //var NeedShowTip = '<%=NeedShowTip%>';
        //$(function () {
        //    if (NeedShowTip == 'True') {
        //        ShowTips();
        //    }
        //});

        function ShowTips() {
            var elm = ['#myScore', '#topScore'];
            var tipPosition = ['bottomMiddle', 'bottomMiddle'];
            var tipTarget = ['topMiddle', 'topMiddle'];
            var tipContent = ['คะแนนที่ทำได้ครั้งนี้', 'คะแนนสูงสุดที่เคยมีคนทำได้ในชุดนี้'];
            var tipAjust = [0, 0];
            var w = [200, 300];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
</asp:Content>
