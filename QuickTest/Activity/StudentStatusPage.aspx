<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StudentStatusPage.aspx.vb" Inherits="QuickTest.StudentStatusPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <title></title>
    <script src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            if (isAndroid) {
                $('#MainDiv').css('width', '705px');
                $('#DivTop').css('width', '705px');
                $('#DivBottom').css('width', '705px');
                $('#DivTop').css('padding', '0px');
                $('#DivBottom').css('padding', '0px');
                $('#DivQNo').css('top', '-135px');
                $('#DivQNo').css('left', '584px');
            }
        });

    </script>
    <style type="text/css">

        #MainDiv {
        width:800px;
        margin-left:auto;
        margin-right:auto;
        margin-top:20px;
        }
        #DivTop {
        width:750px;
        margin-left:auto;
        margin-right:auto;
        padding:20px;
        padding-bottom:0px;
        background-color:rgb(242, 253, 218);
        border-radius:5px;
        }
        #DivBottom {
        width:750px;
        margin-left:auto;
        margin-right:auto;
        padding:20px;
        margin-top:20px;
        border-radius:5px;
        background-color:rgb(242, 253, 218);
        }
        #DivNumberOne {
            position:relative;
            top:30px;
        }
        .DivPosition {
        width:70px;
        background-color:rgb(70, 128, 216);
        border-radius:5px;
        font-size:20px;
        font-weight:bold;
        color:white;
        }
        .DivPodium {
        width:150px;
        height:40px;
        border-radius:5px;
        font-size:20px;
        font-weight:bold;
        color:white;
        background-color:rgb(40, 216, 103);
        line-height:40px;
        }
        .ForMainDivNumber2and3 {
        width:170px;
        position:relative;
        display:inline-block;
        text-align:center;
        margin-left:60px;
        }
        .DivEachStudent {
        width:150px;
        border-radius:5px;
        text-align:center;
        background-color:rgb(28, 192, 214);
        color:white;
        font-size:20px;
        display:inline-block;
        margin:15px;
        }
        .IsAnswer {
        background-color:green;
        }
        .ForDivtopEachStudent {
        height:85px;
        border-bottom:1px solid;
        }
        .ForIsComplete {
        height:85px;
        border-bottom:1px solid;
        background-image:url(../Images/right.png);
        background-size:cover;
        }
        .ForDivScore {
        width:95px;margin-left:54px;font-size:20px;border-top:1px solid;border-left:1px solid;margin-top:20px;
        }
        .ForDivScoreNotSelfPace {
        width:95px;margin-left:54px;font-size:20px;border-top:1px solid;border-left:1px solid;top:60px;position:relative;
        }
        .ForCurrentExam
        {
            float:right;
            padding:2px;
            border:1px solid;
            color:Black;
            }
            .spnStudentNo
            {
                position:relative;
                top:30px;
                left:25px;
                font-size:40px;
                /*color:Black;*/
                background: -webkit-gradient(linear, left top, left bottom, from(#FDF8F8), to(#1CC0D6));
                -webkit-background-clip:text;
                -webkit-text-fill-color:transparent;
                }
                .ForPositionOne
                {
                    background:url(../Images/Podium/Stars3.png);
                    background-repeat:no-repeat;
                    }
                    .ForPositionTwo
                    {
                    background:url(../Images/Podium/Stars2.png);
                    background-repeat:no-repeat;
                        }
                        .ForPositionThree
                        {
                    background:url(../Images/Podium/Stars1.png);
                    background-repeat:no-repeat;
                            }
                        
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv">

    <div id="DivTop">

        <div id="DivNumberOne">

        <div class='ForPositionOne' style='width:150px;display:inline-block;text-align:center;margin-left:65px;background-color: rgb(28, 192, 214);'>
            <div class="ForDivtopEachStudent"  
			style="background:url(<%= PodiumImage1%>); 
			background-size: contain; background-repeat: no-repeat; background-position: center; width:150px;display:inline-block;text-align:center;margin-top:15px;
                border-bottom-color: white;"> 
         
                <span class="spnStudentNo" style="left:5px;"><%=NoInRoomNumberOne %></span>
            </div>
            <div class="DivPodium" style="margin-left:auto;margin-right:auto;"><%=ScoreNumberOne %> คะแนน</div>
        </div>

            <div  class="ForMainDivNumber2and3 ForPositionTwo" style='margin-left:80px;background-color: rgb(28, 192, 214);'>
                 <div class="ForDivtopEachStudent"  
			style="background:url(<%= PodiumImage2%>); 
			background-size: contain; background-repeat: no-repeat; background-position: center; width:150px;display:inline-block;text-align:center;margin-top:15px;
                border-bottom-color: white;"> 
                <%--<div style='font-size:18px;line-height:65px;'>เลขที่</div>
                <div style='font-size:18px;line-height:0px;'><%=NoInRoomNumberOne %></div>--%>
                <span class="spnStudentNo" style="left:5px;"><%=NoInRoomNumberTwo%></span>
            </div>
            <div class="DivPodium" style="margin-left:auto;margin-right:auto;"><%=ScoreNumberTwo%> คะแนน</div>

            </div>

            <div class="ForMainDivNumber2and3 ForPositionThree" style='margin-left:80px;background-color: rgb(28, 192, 214);' >
               <%-- <div class="DivPosition" style="position:relative;margin-left:43px;height:50px;line-height:50px;"><div style='font-size:18px;line-height:30px;'>เลขที่</div><div style='font-size:18px;line-height:10px;'><%=NoInRoomNumberThree %></div></div>
                <div class="DivPodium"><%=ScoreNumberThree %> คะแนน</div>--%>
                 <div class="ForDivtopEachStudent"  
			style="background:url(<%= PodiumImage3%>); 
			background-size: contain; background-repeat: no-repeat; background-position: center; width:150px;display:inline-block;text-align:center;margin-top:15px;
                border-bottom-color: white;"> 
                <%--<div style='font-size:18px;line-height:65px;'>เลขที่</div>
                <div style='font-size:18px;line-height:0px;'><%=NoInRoomNumberOne %></div>--%>
                <span class="spnStudentNo" style="left:5px;"><%=NoInRoomNumberThree%></span>
            </div>
            <div class="DivPodium" style="margin-left:auto;margin-right:auto;"><%=ScoreNumberThree%> คะแนน</div>

            </div>

        </div>

        <div class="DivPosition" id="DivQNo" style="font-size:17px;height:50px;;position:relative;top:-150px;left:650px;line-height:50px;width:120px;background-color:rgb(170, 40, 40);text-align:center;"><%=CurrentExam %></div>

    </div>

        <div id="DivBottom" runat="server">
          
  <%--        <div class='DivEachStudent'>
            <div class='ForDivtopEachStudent' style='background:url(../UserData/1000001/{0e118313-07a1-41e7-8fc6-2a20699ce5d2}/Id.png);background-size:cover;'> 
            <span class='spnStudentNo'>#88</span>
            <div class='ForCurrentExam' >ข้อ 99</div>
             </div>
             <div style='padding:5px;'><img src='../Images/right.png' /> 0 คะแนน</div>
        </div>--%>
          
 
        </div>

    </div>
    </form>
</body>
</html>
