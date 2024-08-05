<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TeacherNewsSelectTeacher.aspx.vb" Inherits="QuickTest.TeacherNewsSelectTeacher" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        var DeviceId = '<%=DeviceId%>';
        $(function () {
            $('.DivCover').click(function () {
                callBlockUI();
            });
        });
        function TeacherClick(InputTeacherId) {
            window.location = '../Watch/TeacherNewsDetail.aspx?TeacherId=' + InputTeacherId + '&MorethanOne=True&DeviceId=' + DeviceId;
        }
        function callBlockUI() {
            $('body').append('<div id="imgBlockUi" style="display: none;"><img src="../Images/metroloader_white.gif" /></div>');
            $.blockUI({
                css: {
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
        .DivWidth {
            width: 210px;
        }

        .DivCover {
            border: 1px solid;
            /*width: 140px;*/
            height: 150px;
            float: left;
            margin: 60px 140px 30px 140px;
            border-radius: 5px;
            cursor: pointer;
        }

        .DivPicture {
            /*width: 140px;*/
            height: 80%;
            background-image: url('../Images/default-profile-image.png');
            background-size: cover;
            border-radius: 5px;
        }

        .DivName {
            border-top: 1px solid;
            /*width: 140px;*/
            height: 20%;
            text-align: center;
            font-size: 20px;
            line-height: 30px;
            /*overflow-x:auto;
            overflow-y:hidden;
            white-space:nowrap;*/
        }

        .ForDivShowInFo {
            text-align: center;
            position: relative;
            top: 15px;
        }

        #DivSelectTeacher {
            width: 70%;
            margin-left: auto;
            margin-right: auto;
            /*background: rgb(253, 255, 228);*/
            text-align: center;
            border-radius: 5px;
        }

        .spnHead {
            font-size: 50px;
            font-weight: bold;
        }
    </style>
    <link href="../css/StyleMobile.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <span class="spnHead">เลือกครู
            </span>
        </div>
        <div id="MainDiv" style="text-align: center;padding-top:75px;">
            <div id="DivSelectTeacher" runat="server">

                <%-- <div id='DivCover1' class='DivWidth DivCover' onclick="TeacherClick('asdasdasd');">
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
            <span>เลือกครู เพื่อดูข่าวประกาศ</span>
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
