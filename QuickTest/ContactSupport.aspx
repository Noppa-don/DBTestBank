<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContactSupport.aspx.vb" Inherits="QuickTest.ContactSupport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="<%=ResolveUrl("~")%>js/jquery-1.7.1.min.js"></script>
    <script src="<%=ResolveUrl("~")%>js/firebase.js"></script>
    <script src="<%=ResolveUrl("~")%>js/RTCMultiConnection.js"></script>
    <script type="text/javascript" src="https://static.twilio.com/libs/twiliojs/1.1/twilio.min.js"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.blockUI.js"></script>

    <script type="text/javascript">
        var caller;
        var sessions = {};
        var wpfFingerPrint = '<%=WpfFingerPrint%>';

        $(function () {
            if (wpfFingerPrint !== '') {
                ContactSupport(wpfFingerPrint);
            }
        });

        function ContactSupport(inputFingerPrint) {
            $.ajax({
                type: "POST",
                async: false,
                url: "<%=ResolveUrl("~")%>ContactSupport.aspx/PostToContactKNSupport",
                data: "{ FingerPrint: '" + inputFingerPrint + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d !== '-1') {
                        caller = new RTCMultiConnection(data.d);
                        caller.session = { audio: true };
                        caller.connect();
                        caller.open();
                        OpenBlockUI();
                        OnStream();
                        OnMute();
                        OnUnMute();
                    }
                },
                error: function myfunction(request, status) {
                    alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }

        function OpenBlockUI() {
            $.blockUI({
                message: '<h1><img src="./Images/reportGraph/ajax-loader.gif" /><br /><br /><span style="color:#FFC76F;">กำลังติดต่อเจ้าหน้าที่ รอสักครู่นะค่ะ</span></h1>',
                css: {
                    border: 'none',
                    backgroundColor: 'transparent'
                }
            });
        }

        function CloseBlockUI() {
            $.unblockUI();
        }

        function OnStream() {
            caller.onstream = function (e) {
                if (e.type == 'remote') {
                    if (e.isVideo) {
                        $('#DivVideo').append(e.mediaElement);
                        $('video').css('width', 200);
                        $('video').css('height', 200);
                        $('video').css('margin-top', '-25px');
                    }
                    CloseBlockUI();
                }
                else {
                    $('#DivAudio').append(e.mediaElement);
                    $('audio').hide();
                }
            }
        }

        function OnMute() {
            caller.onmute = function (e) {
                caller.streams.mute({
                    audio: true
                })
            }
        }

        function OnUnMute() {
            caller.onunmute = function (e) {
                caller.streams.unmute({
                    audio: true
                })
            }
        }

    </script>

    <title></title>
    <style type="text/css">
        #Main {
            /*width: 980px;
            background-color: #fff;
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            padding: 5px;
            border-radius: 6px;
            text-align: center;*/
            margin:-8px;
        }

        /*h2 {
            font-size: 35px;
            margin-top: 10px;
        }*/

        .media {
            /*border: 1px dashed #FFA032;
            border-radius: 5px;
            width: 760px;
            margin-left: auto;
            margin-right: auto;*/
        }

        /*div {
            margin-top: 20px;
        }*/

        /*.Forbtn {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em;  
            color: white;
            font-weight: bold;
            background: #FFA032;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width: 300px;
            height: 50px;
            font-size: 25px;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Main">
            <%--<center>
                <h2>ติดต่อเจ้าหน้าที่เพื่อขอความช่วยเหลือ</h2>
            </center>--%>
            <div id="DivVideo" class="media">
            </div>
            <div id="DivAudio" class="media">
            </div>
            <%-- <div id="divBtn" style="margin-bottom:10px;">
                <input id="btnClose" class="Forbtn" type="button" value="ปิดหน้านี้" />
                <input id="btnCall" class="Forbtn" type="button" style="margin-left:150px;" value="โทรติดต่อเจ้าหน้าที่" />
            </div>--%>
        </div>
    </form>
</body>
</html>
