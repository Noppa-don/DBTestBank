<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HomeworkDetail.aspx.vb"
    Inherits="QuickTest.HomeworkDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        var DeviceId = '<%=DeviceId%>';
        $(function () { 
            //var ChecckHaveBackBtn = '<%=IsHaveBackBtn %>'; 
            //if (ChecckHaveBackBtn == 'False') { 
                //$('#btnBack').hide();
            //}

            //backclick
            $('#btnBack').click(function (e) {
                e.preventDefault();
                callBlockUI();
                BackClick();
            });


            //test bind data
            //for (var i = 0; i < 10; i++) {
            //    $('.ForHoemworkInfo').append(GetTest());
            //}
        });

        function BackClick() {
            window.location = '../Watch/HomeworkSelectStudent.aspx?DeviceId=' + DeviceId;
        }

        function GetTest() {
            var a = '<div id="DivInfo1" class="ForDivInfo"><table style="width:100%"><tr><td class="FortdStudentHomework">';
            a += '<div id="DivSubject" class="ForDivSubject">หลายวิชา</div></td> <td class="FortdStudentHomework"><span class="spnStatus" >ยังไม่ทำ</span>';
            a += '</td></tr><tr><td class="FortdStudentHomework" style="font-size:20px;">ส่ง 25/07/56 12:50</td><td class="FortdStudentHomework" style="font-size:20px;">สั่งโดย ครูไฟว ใจร้าย</td>';
            a += '</tr></table></div>';
            return a;
        }
        function callBlockUI() {
            $('body').append('<div id="imgBlockUi" style="display: none;"><img src="../Images/metroloader_white.gif" /></div>');
            $.blockUI({ css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff',
                'font-size': '50px',
                left: ($(window).width() - 300) / 2 + 'px',
                width: '300px'

            },
                message: $('#imgBlockUi')
            });
        }
    </script>
    <style type="text/css">
        body
        {
            margin: 0 !important;
            padding: 0 !important;
            border: 0 !important;
        }
        .DivStudentDetail
        {
            width: 70%;
            height: 170px;
            margin-left: auto;
            margin-right: auto;
            border-radius: 5px;
            font-size: 30px;
            padding: 20px;
            margin-top: 30px;
            background-color: #FFA032;
            color: white;
        }
        .ForHoemworkInfo
        {
            width: 85%;
            margin: 10px auto;
            border: 1px solid rgb(187, 187, 187);
            border-radius: 5px; /*height: 353px;
            overflow: auto;*/
            min-height: 100px;
            height: auto; /* padding-bottom: 10px;*/
        }
        .ForHoemworkInfo > div:nth-child(odd)
        {
            background-color: #5ECBFF;
        }
        .ForHoemworkInfo > div:nth-child(even)
        {
            background-color: rgb(161, 232, 242);
        }
        .DivForImg
        {
            width: 150px;
            float: left;
            border: 1px solid;
            margin-left: 100px;
        }
        td
        {
            padding-top: 5px;
        }
        .ForDivSubject
        {
            border: 1px solid rgb(187, 187, 187);
            border-radius: 5px;
            font-size: 30px;
            text-align: center; /*padding:10px;*/
            background-color: rgb(0, 153, 255);
            color: white;
            margin-left: 20px;
        }
        .spnStatus
        {
            font-size: 30px;
        }
        .ForDivInfo
        {
            border-bottom: 1px solid;
        }
        .imgBack
        {
            width: 75px;
            position: fixed;
            top: 5px;
            left: 5px;
            cursor: pointer;
        }
        .FortdStudentHomework
        {
            width: 50%;
            text-align: center;
        }
        .ForDivInfo
        {
            border-bottom: 1px solid #fff; /* margin-top: 10px;*/
            width: 100%;
            margin-left: auto;
            margin-right: auto;
        }
        /*.Forbtn
        {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width: 350px;
            height: 100px;
            font-size: 40px;
        }*/
    </style>
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <asp:Button ID="btnBack" CssClass="Forbtn" runat="server" Text="กลับ" Width="20%"
            Style="margin-right: 0.5%;" />
        <asp:Button ID="btnShowAll" CssClass="Forbtn" runat="server" Text="ดูประวัติการบ้าน" Width="30%" />
    </div>
    <div id="MainDiv" style="padding: 75px 0 75px 0;">
        <%--<img id='BtnBack' src="../Images/Arrow back.png" onclick="BackClick();" class="imgBack" />--%>
        <%--<div class="DivStudentDetail" id="DivStudentDetail" runat="server">
            <div class='DivForImg'>
                <img src="../Images/Student.png" style='width: 150px; margin-top: 5px;' />
            </div>
            <div id='DivStudentInfo' style='float: left; margin-left: 30px;'>
                <table>
                    <tr>
                        <td>
                            <%=StudentName %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=StudentClassRoom%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=StudentCode %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>--%>
        <div id="DivHomeworkInfo" class="ForHoemworkInfo" runat="server">
            <%--<div id="DivInfo1" class="ForDivInfo">
                <table style="width:100%">
                    <tr>
                        <td class="FortdStudentHomework">
                            <div id="DivSubject" class="ForDivSubject">
                                หลายวิชา
                            </div>
                        </td>
                        <td class="FortdStudentHomework">
                            <span class="spnStatus" >ยังไม่ทำ</span>
                        </td>
                    </tr>

                    <tr>
                        <td class="FortdStudentHomework" style="font-size:30px;">ส่ง 25/07/56 12:50</td>
                        <td class="FortdStudentHomework" style="font-size:30px;">สั่งโดย ครูไฟว ใจร้าย</td>
                    </tr>
                </table>
            </div>--%>
            <%--   <div id="Div1" class="ForDivInfo">
                <table style="width:100%">
                    <tr>
                        <td class="FortdStudentHomework">
                            <div id="Div2" class="ForDivSubject">
                                หลายวิชา
                            </div>
                        </td>
                        <td class="FortdStudentHomework">
                            <span class="spnStatus" >ยังไม่ทำ</span>
                        </td>
                    </tr>

                    <tr>
                        <td class="FortdStudentHomework" style="font-size:30px;">ส่ง 25/07/56 12:50</td>
                        <td class="FortdStudentHomework" style="font-size:30px;">สั่งโดย ครูไฟว ใจร้าย</td>
                    </tr>
                </table>
            </div>--%>
        </div>
        <%--  <div id="DivBtnAll" style="width: 55%; margin-left: auto; margin-right: auto; text-align: center;
            margin-top: 40px;">
            <asp:Button ID="btnShowAll" CssClass="Forbtn" runat="server" Text="ดูทั้งหมด" />
        </div>--%>
    </div>
    <div class="footer">
        <span>
            <%=StudentName %></span><span><%=StudentClassRoom%></span><span>
                <%=StudentCode %></span></div>
    </form>
</body>
</html>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;
    if (isAndroid) {
        $('.footer').css('min-height', '80px');        
    }    
</script>