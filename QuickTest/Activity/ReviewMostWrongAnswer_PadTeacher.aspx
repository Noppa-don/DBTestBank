<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReviewMostWrongAnswer_PadTeacher.aspx.vb"
    Inherits="QuickTest.ReviewMostWrongAnswer_PadTeacher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/sweet-tooltip.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <script src="../js/slides.min.jquery.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <link href="../css/prettyPhoto.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.prettyLoader.js" type="text/javascript"></script>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        ul
        {
            counter-reset: li; /*list-style-type: decimal;*/
            margin-top: 0px;
            list-style: none; /*list-style-image:url('/img/smile_1.png');*/
        }
        ul > li:before
        {
            /*content:"ลำดับที่ ";*/
            content: "ลำดับที่ " counter(li) "          "; /*list-style-type:decimal;*/
            counter-increment: li;
            color: Orange;
        }
        li
        {
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
        #MainDiv
        {
            /*font-size:35px;
            font-family: "Angsana New";*/
            font: normal 35px 'THSarabunNew';
            background-color: #7BAD18;
        }
        .ForBtn
        {
            width: 65px;
            height: 65px;
            margin-left: 7px;
            margin-bottom: 30px;
            background-repeat: no-repeat;
            background-size: 60px;
        }
    </style>
    <script type="text/javascript">

        $(function () {
            $('#BtnSwapQuestion').toggle(function () {
                $('.divReviewAns').show(500);
                CreateButtonLeapChoice();
            },
            function () {
                $('.divReviewAns').hide(500);                
            }
            );
            $('#BtnExitQuiz').click(function (e) {
                var title = 'ต้องการออกจากควิซใช่หรือไม่ ?';
                $('#dialog').dialog('option', 'title', title).dialog('open');
            });
            $('#dialog').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () { $(this).dialog('close'); }, 'ไม่': function () {
                    $(this).dialog('close');
                }
                },
                draggable: false,
                resizable: false,
                modal: true
            });
        });
        function CreateButtonLeapChoice(IsNormalSort) {
              //var DVIDJS = "<%=DVID %>"
       $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>Activity/ReviewMostWrongAnswer_PadTeacher.aspx/CreateStringLeapChoice",
	            //data: "{ DeviceUniqueId: '" + DVIDJS + "'}",  //" 
                data: "{ IsNormalSort:'" + IsNormalSort + "'}",   
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    //alert(msg.d);
                    $('#LeapChoiceDiv').html(msg.d);     
                    $('#slideDiv').slides({
     generatePagination:false
    });
                    //UseSlide();
	            },
	            error: function myfunction(request, status)  {
                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	            }
	        });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id='MainDiv' style='width: 925px; height: auto;'>
        <div id="panelReviewMode" style="background-color: #83BA1F; width: 290px; float: left;
            margin: 5px; text-align: center">
            ทบทวนข้อที่ผิดเยอะ</div>
        <div id="panelNameTestset" style="background-color: #94BD4A; width: 395px; float: left;
            height: 68px; margin: 5px; margin-left: 0px; text-align: center; overflow: hidden;">
            <asp:Label ID="lblTestsetName" runat="server" Text=""></asp:Label>
        </div>
        <div id="PanelWrongAmount" style="background-color: #94BD4A; width: 100px; float: left;
            margin: 5px; margin-left: 0px; padding-left: 10px; height: 68px;">
            <asp:Image ID="SideRed" Style='margin-left: 0px; width: 35px; margin-top: 5px; position: relative;
                top: -20px;' ImageUrl="~/Images/Activity/Red1Px.png" runat="server" />
            <asp:Image ID="SideGreen" Style='margin-left: 10px; width: 35px; margin-top: 5px;
                position: relative; top: -20px;' ImageUrl="~/Images/Activity/Green1px.png" runat="server" />
            <asp:Label ID="lblSideChart" CssClass='forSideChartTon' Style='float: left; color: Black;
                font-size: 17px; position: relative; top: -40px;' runat="server" Text="ถูก 1 ผิด 1"></asp:Label></div>
        <div id="BtnSwapQuestion" style="background-color: #94BD4A; width: 100px; float: right;
            margin: 5px; font-size: 30px; height: 68px;">
            เลือกข้อ</div>
        <div style="clear: both;">
        </div>
        <div style="float: right;">
            <div id="BtnNext" style="background-color: #94BD4A; width: 100px; height: 100px;
                margin-bottom: 5px;">
                <asp:Button ID="btnNext" runat="server" Text=">>" Height="100px" Width="90px" Style="font-size: 60px;" /></div>
            <div id="BtnPrev" style="background-color: #94BD4A; width: 100px; height: 100px;
                margin-bottom: 5px;">
                <asp:Button ID="btnPrev" runat="server" Text="<<" Height="100px" Width="90px" Style="font-size: 60px;" /></div>
            <div id="BtnExitQuiz" style="background-color: #94BD4A; width: 100px; margin-bottom: 5px;">
                <img src="../Images/upgradeClass/logout.png" style="width: 100px; height: 100px;" /></div>
        </div>
        <div id="mainQuestion" runat="server" class="Question" style="position: relative;
            -webkit-border-radius: 10px 10px 0px 0px; background-color: #ffc76f; width: 783px;
            border: 20px; padding: 20px;">
        </div>
        <div id="mainAnswer" runat="server" style="width: 783px; position: relative; background-color: #F4F7FF;
            padding: 20px; -webkit-border-radius: 0px 0px 10px 10px;">
            <table id="Table1" runat="server" style="border-collapse: collapse;">
                <tr>
                    <td runat="server" id="AnswerTbl">
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="divReviewAns" style='background-image: url(../Images/Activity/BG_Opacity/crLayer90pct.png);
        position: absolute; top: 50px; width: 675px; margin-left: 15px; height: 390px;
        display: none;'>
        <div id='slideDiv' style='width: 675px; height: 420px; position: absolute;'>
            <div id='LeapChoiceDiv' class='slides_container' style='height: 300px;'>
            </div>
            <a href='#' style='top: 130px; position: absolute;' class='prev'>
                <img src="../Images/Activity/AllNewArrow/leftBlue.png" /></a><a href='#' style='top: 130px;
                    right: 0px; position: absolute;' class='next'>
                    <img src="../Images/Activity/AllNewArrow/rightBlue.png" /></a>
            <div id='DivSortBtn'>
                <input type="button" id='BtnSortNormal' value='เรียงตามลำดับข้อ' style='margin-left: 170px;
                    width: 150px; height: 60px;' />
                <input type="button" id='BtnSortAnswerNull' value='เรียงตามข้อทีผิดเยอะ' style='margin-left: 45px;
                    width: 150px; height: 60px;' />
            </div>
        </div>
    </div>
    <div id="dialog" title="">
    </div>
    </form>
</body>
</html>
