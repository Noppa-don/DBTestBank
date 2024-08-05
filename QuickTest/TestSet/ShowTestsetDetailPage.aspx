<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowTestsetDetailPage.aspx.vb"
    Inherits="QuickTest.ShowTestsetDetailPage" %>

<%@ Register Src="~/UserControl/TestsetHeaderControl.ascx" TagName="UserControl"
    TagPrefix="myTestsetDetail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        body {
            font: normal 0.95em 'THSarabunNew';
            color: #444;
        }

        #TestsetDetail, .Detail {
            margin: 10px 10px 0 0;
            /*position: absolute;*/
            width: 95.5%;
            border: 2px solid #FFA032;
            background-color: wheat;
            border-radius: 5px;
            margin-left: auto;
            margin-right: auto;
            padding: 10px;
        }

            #TestsetDetail > div, .Detail > div {
                /*border: 1px solid;*/
                padding: 10px;
                border-radius: .5em;
                font-size: 20px;
                margin-top: 10px;
                border: 1px dashed #FFA032;
            }

                #TestsetDetail > div > div, .Detail > div > div {
                    /*border: 1px solid;*/
                    border: 1px dashed #FFA032;
                    padding: 10px; /*margin-top: 10px;*/
                    border-radius: .5em;
                }

                    #TestsetDetail > div > div > div, .Detail > div > div > div {
                        padding-left: 2em;
                    }
        /*#TestsetDetail > div > div td, .Detail > div > div td
        {
            border-bottom: 1px solid white;
        }*/
        qh, qq {
            font-weight: bold;
        }

        span.T, span.F, span.S {
            padding-left: 15px;
            padding-right: 15px;
            border: 1px solid;
        }

        .wrong {
            background-color: red;
            color: white;
        }

        span.T, span.C {
            /*background-color: rgb(131, 252, 131);*/
            background-color: #2CA505;
            color: white;
        }

        span.S {
            /*background-color: yellow;*/
        }

        .correct {
            background-color: #2CA505;
            color: white;
        }

        /*.toggleDetail
        {
            background-image: url('../images/ModeChangeBtn-Answer-Selected.png');
            width: 44px;
            height: 44px;
            position: absolute;
            right: 0;
            cursor: pointer;
            z-index: 55;
        }*/
        ul#ToggleModeDetail {
            position: fixed;
            margin: 0px;
            padding: 0px;
            right: -75px; /*-50px */
            top: 90px;
            list-style: none;
            z-index: 9999;
            cursor: pointer;
            /*display:none;*/
        }

            ul#ToggleModeDetail li {
                width: 120px;
                height: 70px;
                text-decoration: none;
                line-height: 1.7em;
                list-style: none;
                display: block;
                background-image: url('../images/ModeChangeBtn-Answer-Selected.png');
                background-repeat: no-repeat;
                background-position: left center;
                background-color: #CFCFCF;
                border: 1px solid #AFAFAF;
                border-radius: 10px 0 0 10px;
                opacity: 0.6;
            }

                ul#ToggleModeDetail li a {
                    position: absolute;
                    right: 5px;
                    font-size: 20px;
                    margin-top: 10px;
                }

                ul#ToggleModeDetail li:hover {
                    color: #0D0914;
                    text-decoration: none;
                    font-weight: bold;
                }

        html {
            background-color: #D3F2F7 !important;
        }

        .Type3Correct {
            background-color: #2CA505;
            padding: 0px 15px;
            color: white;
        }

        .Type3Wrong {
            background-color: red;
            padding: 0px 15px;
            color: white;
        }
    </style>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //ดักถ้าเข้าจาก Tablet ครู
            if (isAndroid) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio

                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                $('table tr td').css('font-size', '30px');
                $('#TestsetDetail div div').css('font-size', '25px');

            }


            // click สลับโหมด
            $('.toggleDetail').toggle(function () {
                $('#TestsetDetail').hide();
                $('#QuizDetailCorrect').show();
                $(this).children().find('a').html('สลับไปดู<br />เด็กตอบ');
            }, function () {
                $('#TestsetDetail').show();
                $('#QuizDetailCorrect').hide();
                $(this).children().find('a').html('สลับไป<br />ดูเฉลย');
            });

            // show/hide menu side bar
            //            $('#ToggleModeDetail li a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#ToggleModeDetail').hover(function () {
                $(this).stop().animate({ 'right': '0px' }, 200);
            },
            function () {
                $(this).stop().animate({ 'right': '-75px' }, 200);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <% If Request.QueryString("TestsetID") = Nothing And IsPlayQuiz = True Then%>
            <%--<div class="toggleDetail">
            </div>--%>
            <ul id="ToggleModeDetail" class="toggleDetail">
                <li class="ToggleMode"><a>สลับไป<br />
                    ดูเฉลย</a></li>
            </ul>
            <%End If%>
            <myTestsetDetail:UserControl ID="myCtlTestsetDetail" runat="server"></myTestsetDetail:UserControl>
            <div>
                <div id="TestsetDetail" class="Detail" runat="server">
                </div>
                <div id="QuizDetailCorrect" class="Detail" runat="server" style="display: none;">
                </div>
            </div>
        </div>
    </form>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <%If BusinessTablet360.ClsKNSession.EnableReportQuestion Then %>
    <style type="text/css">
        .btnReportQuestion {
            position: absolute;
            cursor: pointer;
        }

        .divExamType1 .btnReportQuestion {
            right: 5px;
            top: 5px;
        }

        .divExamType2, .divExamType6 {
            position: relative;
        }

            .divExamType2 td {
                width: 1px;
            }

            .divExamType2 .btnReportQuestion, .divExamType3 .btnReportQuestion {
                right: 45px;
            }

            .divExamType6 .btnReportQuestion {
                right: 0;
            }
    </style>    
    <script src="../js/reportProblemQuestion.js" type="text/javascript"></script>
    <%End If %>
    <script type="text/javascript">
        $(function () {
            $('.imgSound').each(function () {
                var parent = $(this).closest('.divQuestion');
                var qsetPath = $(parent).attr('qsetFilePath');
                var fileName = $(this).attr('alt');                
                $(this).after($('<audio>', { src: qsetPath + fileName, controls: '' }));
                $(this).remove();
            });
        });
    </script>   
</body>
</html>
