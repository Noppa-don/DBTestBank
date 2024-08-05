<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HomeworkSelectStudent.aspx.vb"
    Inherits="QuickTest.HomeworkSelectStudent" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            
            $('.DivCover').click(function () {
                callBlockUI();
            });
        });

        function ChooseStudent(InputStudentId, DeviceId) {
            window.location = '../Watch/HomeworkDetail.aspx?StudentId=' + InputStudentId + '&MorethanOne=True&DeviceId=' + DeviceId;
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
        /*#DivSelectStudent
        {
            width: 100%; background: rgb(253, 255, 228);
            text-align: center;
            padding-top: 30px;
            padding-bottom: 66px;
        }
        .DivWidth
        {
            width: 300px;
        }
        .DivCover
        {
            height: 250px;
            cursor: pointer;
            border: 1px solid rgb(187, 187, 187);
            border-radius: 5px;
            display: inline-block;
            margin: 0 30px 30px 30px;
        }
        .DivPicture
        {            
            height: 200px;
            background-image: url('../Images/default-profile-image.png');
            background-size: cover;
            border-radius: 4px 4px 0 0;
        }
        .DivName
        {         
            height: 49px;
            text-align: center;
            font-size: 25px;
            line-height: 49px;             
            background-color: #FFA032;
            border-top: 1px solid rgb(187, 187, 187);
            border-radius: 0 0 4px 4px;
        }*/
        .ForDivShowInFo
        {
            text-align: center;
            position: relative;
            top: 15px;
        }
        .spnHead
        {
            font-size: 50px;
            font-weight: bold;
        }
    </style>
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="MainDiv" style="text-align: center; padding-top: 10px;">
        <div id="DivSelectStudent" runat="server">
            <%-- <div id='DivCover1' class='DivWidth DivCover' onclick="ChooseStudent('asdasdasd');">
                <div id='Divpicture1' class='DivWidth DivPicture' >
                    </div>
                <div id='DivName1' class='DivWidth DivName' >
                    สมรัก พรรคเพื่อเก้ง
                    </div>
                </div>


                    <div id='Div1' class='DivWidth DivCover'>
                <div id='Div2' class='DivWidth DivPicture' >
                    </div>
                <div id='Div3' class='DivWidth DivName' >
                    สมรัก พรรคเพื่อเก้ง
                    </div>
                </div>--%>
        </div>
    </div>
    <div class="footer">
        เลือกนักเรียน
    </div>
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