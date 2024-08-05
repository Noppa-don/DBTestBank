<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatSelectTeacher.aspx.vb"
    Inherits="QuickTest.ChatSelectTeacher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        var DeviceId = '<%=DeviceId%>';
    var JsCheckIsMoreThanOne = '<%=IsMoreThanOne %>';

    $(function(){
    
        if (JsCheckIsMoreThanOne == 'True') {
            $('#ImgBack').show();
        }

        $('#btnBack').click(function(e) {
            e.preventDefault();
            callBlockUI();
            BackSelectStudent();
        });

        
        $('.DivCover').click(function () {          
            callBlockUI();
        });
    });

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


    function BackSelectStudent() {
        window.location = '<%=ResolveUrl("~")%>Watch/ChatSelectStudent.aspx?DeviceId=' + DeviceId;
    }


        function TeacherClick(InputTeacherId) {
         $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>Watch/ChatSelectTeacher.aspx/ClickTeacher",
	            data: "{ TeacherId: '" + InputTeacherId + "' }",
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    if (msg.d !== '') {
                            window.location = '../Watch/Chat.aspx?ChatRoom_Id=' + msg.d;
                        }
	                },
	                error: function myfunction(request, status)  {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                }
	            });
        }

    </script>
    <style type="text/css">
        /*.DivWidth
        {
            width: 210px;
        }
        
        .DivCover
        {
            border: 1px solid; /*width: 140px;*/
        /* height: 150px;
            float: left;
            margin: 60px 140px 30px 140px;
            border-radius: 5px;
            cursor: pointer;
        }
        .DivPicture
        {
            /*width: 140px;*/
        /* height: 80%;
            background-image: url('../Images/default-profile-image.png');
            background-size: cover;
            border-radius: 5px;
        }
        .DivName
        {
            border-top: 1px solid; /*width: 140px;*/
        /*  height: 20%;
            text-align: center;
            font-size: 20px;
            line-height: 30px; /*overflow-x:auto;
            overflow-y:hidden;
            white-space:nowrap;*/
        /*}
        
        #DivSelectTeacher
        {
            width: 75%;
            margin-left: auto;
            margin-right: auto;
            background: rgb(253, 255, 228);
            text-align: center;
            border-radius: 5px;
        }
        #ImgBack
        {
            width: 50px;
            float: left;
            margin-left: 5px;
            display: none;
            cursor: pointer;
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
    <div class="header">
        <asp:Button ID="btnBack" CssClass="Forbtn" runat="server" Text="กลับ" Width="20%" />
    </div>
    <div id="MainDiv" style="text-align: center; padding-top: 75px;">
        <%--<img id='ImgBack' src="../Images/Arrow back.png" onclick='BackSelectStudent();' />--%>
        <%--<span class="spnHead">เลือกครู </span>--%>
        <div id="DivSelectTeacher" runat="server">
        </div>
    </div>
    <div class="footer">
        เลือกครู
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
