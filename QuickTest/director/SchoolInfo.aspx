<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SchoolInfo.aspx.vb" Inherits="QuickTest.SchoolInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css">
        #MainDiv {
           background:url(../Images/SchoolInfoBG.png);
        background-size:cover;
        margin-left:auto;
        margin-right:auto;
        width:350px;
        height:800px;
        }
        span {
            color:white;
            position:relative;
        }
        .ForspnTotalHour1Digit {
        font-size:80px;
        top:125px;
        left:162px;
        }
        .ForspnTotalHour2Digit {
        font-size:90px;
        top:120px;
        left:140px;
        }
        .ForspnTotalHour3Digit {
        font-size:80px;
        top:125px;
        left:121px;
        }
        .ForspnTotalHour4Digit {
        font-size:70px;
        top:130px;
        left:105px;
        }
        .ForspnTotalHour5Digit {
        font-size:60px;
        top:135px;
        left:100px;
        }
        .ForspnTotalHour6Digit {
        font-size:45px;
        top:140px;
        left:108px;
        }
        #spnMaleActive {
        font-size:70px;
        /*top:325px;
        left:-35px;*/
        }
        #spnFemaleActive {
        font-size:70px;
        /*top:237px;
        left:208px;*/
        }
        #spnQuiz {
        font-size:30px;
        margin-left:30px;
        }
        #spnTotalQuiz {
        font-size:30px;
        }
        .ForspnTotalQuiz1Digit {
        margin-left:50px;
        }
        .ForspnTotalQuiz2Digit {
            margin-left:43px;
        }
        .ForspnTotalQuiz3Digit {
        margin-left:35px;
        }
        .ForspnTotalQuiz4Digit {
        margin-left:23px;
        }
        #spnHomework {
        font-size:30px;
        }
        #spnTotalHomework {
        font-size:30px;
        }
        .ForspnTotalHomework1Dgit {
        margin-left:38px;
        }
        .ForspnTotalHomework2Dgit {
        margin-left:30px;
        }
        .ForspnTotalHomework3Dgit {
        margin-left:23px;
        }
        .ForspnTotalHomework4Dgit {
        margin-left:15px;
        }
        #spnPractice {
        font-size:30px;
        margin-left:11px;
        }
        #spnTotalPractice {
        font-size:30px;
        }
        .ForspnTotalPractice1Digit {
        margin-left:40px;
        }
        .ForspnTotalPractice2Digit {
        margin-left:34px;
        }
        .ForspnTotalPractice3Digit {
        margin-left:24px;
        }
        .ForspnTotalPractice4Digit {
        margin-left:14px;
        }
        .ForDivBottom {
        width:110px;
        display:inline-block;
        }
        .ForDivMiddle {
        width:130px;
        display:inline-block;
        position:relative;
        top:240px;

        }

    </style>

</head>
<body style='background-color:#009CC4;'>
    <form id="form1" runat="server">
    <div id="MainDiv">
 <span id="spnTotalHour" class='<%=spnTotalHourClass %>' ><%=TotalHour %></span>
        <div style="position:relative;">
            <div class="ForDivMiddle" style="margin-left:48px;">
        <span id="spnMaleActive"><%=MaleActive %></span>
                </div>
            <div class="ForDivMiddle" style="margin-left:20px;">
        <span id="spnFemaleActive"><%=FemaleActive %></span>
                </div>
            </div>
        <div style="position:relative;top:415px;">
       <div class="ForDivBottom">
        <span id="spnQuiz">ควิซ</span>
           <br />
        <span id="spnTotalQuiz" class="<%=spnTotalQuizClass %>"><%=TotalQuiz %></span>
           </div>
        <div class="ForDivBottom">
        <span id="spnHomework">การบ้าน</span>
             <br />
        <span id="spnTotalHomework" class="<%=spnTotalHomeworkClass %>"><%=TotalHomework %></span>
            </div>
        <div class="ForDivBottom">
        <span id="spnPractice">ฝึกฝน</span>
             <br />
        <span id="spnTotalPractice" class="<%=spnTotalPracticeClass %>"><%=TotalPractice %></span>
            </div>
           </div>
    </div>
    </form>
</body>
</html>
