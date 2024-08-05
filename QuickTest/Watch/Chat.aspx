<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Chat.aspx.vb" Inherits="QuickTest.Chat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <%--<script src="../js/JQueryMobile.js" type="text/javascript"></script>    --%>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        var SignalRCheck;
        var JSChatRoomId = '<%=ChatRoomId %>';
        var JSUserId = '<%=OwnerId %>';
        var HeadText = '<%=HeaderTxt %>';
        var CheckFocusScreen = '<%=Session("FocusScreen") %>';
        var CheckSeenMsgHistory = '<%=Flag %>';
        var DeviceId = '<%=DeviceId%>';
        SignalRCheck = $.connection.hubSignalR;
        var IsFromTeacher = '<%=IsProcessFromTeacher %>';
        //Reload หน้าเมื่อฝั่งนุ้นเห็นข้อความย้อนหลังของเรา
        SignalRCheck.client.isSeenHistoryMsg = (function () {
            if (IsFromTeacher == 'True') {
                window.location = '<%=ResolveUrl("~")%>Watch/Chat.aspx?ChatRoom_Id=' + JSChatRoomId + '&FromTeacher=True';
            } else {
                window.location = '<%=ResolveUrl("~")%>Watch/Chat.aspx?ChatRoom_Id=' + JSChatRoomId;
            }
        });

        //ขารับ + ส่งกลับไปบอกว่าเห็นข้อความแล้ว
        SignalRCheck.client.recieveMsg = (function (msg, StrDatetime, CmId, CrId, username) {
            //$('#MainContent').append("<div class='MsgSetting MsgLeft'>" + username + "<span class='spnLeft'>" + msg + '</span><br /><span>' + StrDatetime + '</span></div>');
            $('#MainContent').append("<div class='MsgSetting MsgLeft'><span class='spnLeft'>" + msg + '</span><br /><span>' + StrDatetime + '</span></div>');
            $('#MainDiv').scrollTop($('#MainDiv')[0].scrollHeight);
            //$.mobile.silentScroll($('body')[0].scrollHeight);
            SignalRCheck.server.isSeenMessage(JSChatRoomId, CrId, CmId);
        });

        //เมื่อส่งสำเร็จ append ข้อความขึ้น Content
        SignalRCheck.client.sendComplete = (function (CmId, StrDateTime) {

            $('#MainContent').append("<div class='MsgSetting MsgRight'><img class='ImgEye' id='" + CmId + "' src='../Images/400_F_20499381_ktZBQQHYEaCwCQ38XpIhzwLO1OLavUrV_PXP.jpg' /><span class='spnRight'>" + $('#txtInput').val() + "</span><br /><span>" + StrDateTime + "</span></div>");
            $('#txtInput').val('');
            $('#MainDiv').scrollTop($('#MainDiv')[0].scrollHeight);
            //$.mobile.silentScroll($('body')[0].scrollHeight);
        });

        //show รูปลูกตา แสดงว่าฝั่งนุ่นเห็นข้อความเราแล้ว
        SignalRCheck.client.recipientIsSeen = (function (CMID) {
            $('#' + CMID).show();
        });

        $.connection.hub.start().done(function () {
            //Add เข้า Group
            SignalRCheck.server.addToGroup(JSChatRoomId);
            //ขาส่ง
            $('#btnSend').click(function () {

                if ($('#txtInput').val() !== '') {
                    //SignalRCheck.server.sendMessage(JSChatRoomId, JSChatRoomId, JSUserId, $('#txtInput').val());				    
                    SignalRCheck.server.sendMessage(JSChatRoomId, JSUserId, $('#txtInput').val());
                }
                if (IsFromTeacher != "True") {
                    setFocusScreen();
                    window.location.reload();
                }
            });

            //Check ถ้าเราเห็นข้อความที่คนอื่นพิมพ์มาตอนปิดเครื่องพอเห็นแล้วต้องให้ฝั่งนุ้น Reload หน้า
            if (CheckSeenMsgHistory == 'True') {
                SignalRCheck.server.seenMessageHistory(JSChatRoomId);
            }

        });


        //window.addEventListener("resize", function () {
        //      // Get screen size (inner/outerWidth, inner/outerHeight)
        //      //alert(1);
        //      //alert(window.innerHeight);
        //      $('#spnHeader').append(100);
        //  }, false);

        //	var supportsOrientationChange = "onorientationchange" in window,
        //orientationEvent = supportsOrientationChange ? "orientationchange" : "resize";

        //	window.addEventListener(orientationEvent, function () {
        //	    //alert('HOLY ROTATING SCREENS BATMAN:' + window.orientation + " " + screen.width);
        //	    $('#spnHeader').append(55555);
        //	}, false);


        $(function () {

            //ใส่ text ให้ header ว่ากำลังคุยกับใคร
            //$('#Header').append(HeadText);
            $('#spnHeader').text(HeadText);
            //$('body').silentScroll($('body')[0].scrollHeight);

            //ปรับ ScrollBar ให้อยุ่ข้างล่าง            
            if (CheckFocusScreen == 'Bottom') {
                //console.log($('#MainContent')[0].scrollHeight);
                $('#MainDiv').scrollTop($('#MainDiv')[0].scrollHeight);
                $('body').scrollTop($('body')[0].scrollHeight);
            }

            $(window).on("orientationchange", function (event) {
                if (IsFromTeacher == "False") {
                    '<% HttpContext.Current.Session("ChatRoom_Id") = ChatRoomId%>';
                }
            });
            //$(window).orientationchange();
            //            $(window).load(function() {
            //                $('body').animate({scrollTop:$(document).height()},1000);
            //            });
            //            var ScreenHeight = $(window).height();
            //            ScreenHeight = ScreenHeight - 121;

            //            $('#MainContent').height(ScreenHeight);

            //alert($(window).height());

            //$('#btnBack').click(function (e) {
            //    e.preventDefault();
            //    callBlockUI();                
            //    console.log('IsFromTeacher = ' + IsFromTeacher);
            //    if (IsFromTeacher == 'True') {
                   // window.location = '<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx';
                //}
                //else if (IsFromTeacher == 'False') {
                //    BackSelectStudent();
                //} else {
                   // window.location = '<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx';
            //    }
            //});

            //            //ขยาย textarea
            //            var txt = $('#Textarea1'),
            //            hiddenDiv = $(document.createElement('div')),
            //            content = null;
            //            
            //            txt.addClass('txtstuff');
            //            hiddenDiv.addClass('hiddenDiv common');

            //            $('body').append(hiddenDiv);

            //            txt.on('keyup',function() {
            //                content = $(this).val();
            //                content = content.replace(/\n/g,'<br>');
            //                hiddenDiv.html(content + '<br class="lbr">');
            //                $(this).css('height',hiddenDiv.height());
            //            });


        });


    function ShowMoreMessage() {
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>Watch/Chat.aspx/SetSessionWhenClickMoreMsg",
            //data: "{ EI_Id: '" + EI_Id + "',EI_Code:'" + EiName + "' }",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (msg) {
                if (msg.d !== '') {
                    if (IsFromTeacher == 'True') {
                        window.location = '<%=ResolveUrl("~")%>Watch/Chat.aspx?ChatRoom_Id=' + JSChatRoomId + '&FromTeacher=True';
                    } else {
                        window.location = '<%=ResolveUrl("~")%>Watch/Chat.aspx?ChatRoom_Id=' + JSChatRoomId;
                    }
                }
            },
            error: function myfunction(request, status) {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
            }
        });
    }

    function setFocusScreen() {
        $.ajax({
            type: "POST",
            url: "<%=ResolveUrl("~")%>Watch/Chat.aspx/SetFocusScreen",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function BackSelectStudent() {
            window.location = '<%=ResolveUrl("~")%>Watch/ChatSelectStudent.aspx?DeviceId=' + DeviceId;
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
        html, body, form, #MainDiv {
            height: 100%;
        }

        #Header {
            position: fixed;
            width: 100%;
            height: 50px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            font-size: 20px;
            font-weight: bold;
            background-color: #1D7480;
            color: black;
        }

        #MainDiv {
            display: block;
            position: absolute;
            height: auto;
            bottom: 0;
            top: 0;
            left: 0;
            right: 0;
            margin-top: 75px;
            margin-bottom: 138px;
            margin-right: 0px;
            margin-left: 0px;
            background-color: rgb(252, 255, 229);
            overflow-y: scroll;
        }

        #MainContent {
            width: 98%;
            font-size: 22px;
            background-color: rgb(252, 255, 229);
            padding: 10px 1%;
            /*height:100%;*/
        }

        #DivButtonShowMoreMsg {
            display: none;
            position: fixed;
            width: 100%;
            height: 35px;
            top: 60px;
            text-align: center; /*margin-top:10px;*/
            background-color: #12AAFF;
        }

        #DivBottom {
            position: fixed;
            width: 100%;
            bottom: 0px;
            height: 40px; /*margin-top: 10px;*/
        }

        #txtInput {
            width: 75%; /*height: 100px;*/
            font-size: 20px;
            margin-bottom: -7px;
        }

        #BtnShowMoreMsg {
            width: 10%; /*font-size: 20px;*/
            display: none;
            float: right;
            margin-right: 1%;
        }

        #btnSend {
            width: 19%; /*height: 100px;*/
            font-size: 20px;
        }

        .MsgLeft {
            text-align: left; /*padding-right: 55%;*/
        }

        .MsgRight {
            text-align: right; /*padding-left: 55%;*/
        }

        .MsgSetting {
            word-wrap: break-word;
            margin-right: 5px;
            margin-bottom: 10px;
            margin-top: 10px;
        }

        .spnLeft, .spnRight {
            padding: 3px 5px;
            border-radius: 5px;
            color: black;
        }

        .spnLeft {
            background-color: rgb(136, 209, 42); /* #18A718;*/
        }

        .spnRight {
            background-color: rgb(253, 149, 149); /* #F05D5D;*/
        }

        .ImgEye {
            width: 20px;
            margin-right: 5px;
            display: none;
        }

        .ImgEyeSeen {
            width: 20px;
            margin-right: 5px;
        }

        #ImgBack {
            width: 30px;
            float: left;
            margin-left: 5px;
            cursor: pointer;
        }

        .spnDate {
            font-size: 12px;
            margin-top: 5px;
            display: inline-block;
        }

        #spnHeader {
            color: #fff;
            font-size: 40px;
        }
    </style>

    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />

</head>
<body style='margin: 0px;'>
    <form id="form1" runat="server">
        <div class="header" style="line-height: 55px;">
            <asp:Button ID="btnBack" CssClass="Forbtn" runat="server" Text="กลับ" Width="20%"
                Style="margin-left: 1%; float: left;" data-role="none" />
            <span id='spnHeader'></span>
            <input type="button" id='BtnShowMoreMsg' runat="server" value='...' onclick='ShowMoreMessage()'
                class="Forbtn" data-role="none" />
        </div>
        <div id='MainDiv'>
            <%--<div id='Header'>
            <img id='ImgBack' src="../Images/Arrow back.png" onclick='BackSelectStudent();' />
            <span id='spnHeader'></span>
            <input type="button" id='BtnShowMoreMsg' runat="server" value='...' onclick='ShowMoreMessage()' />
        </div>--%>
            <%--<div id='DivButtonShowMoreMsg' runat="server">
	
		</div>--%>
            <div id='MainContent' runat="server">
            </div>
            <%--<div id='DivBottom'>
            <input type="text" id='txtInput' />
            <textarea id='txtInput' rows='1' cols='10'></textarea>
            <input type="button" id='btnSend' value='ส่ง' />
        </div>--%>
        </div>
        <div class="footer">
            <textarea id='txtInput' rows='1' cols='10' data-role="none"></textarea>
            <input type="button" id='btnSend' value='ส่ง' class="Forbtn" style="float: left; width: 10%; margin-top: 5px;" data-role="none" />
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1;
    if (isAndroid) {
        console.log(1);
        $('.footer').css('min-height', '80px');
        $('#MainDiv').css('margin-bottom', '80px');
    }
    //$.mobile.loadingMessage = false;   
    //if (CheckFocusScreen == 'Bottom') {
    //    setTimeout(function () {
    //        $.mobile.silentScroll($('body')[0].scrollHeight);
    //    }, 500);
    //}
</script>
