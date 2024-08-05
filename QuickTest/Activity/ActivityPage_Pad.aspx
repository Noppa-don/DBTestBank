<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityPage_Pad.aspx.vb" Inherits="QuickTest.ActivityPage_Pad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
    </style>
    <%--<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />--%>

    <script src="../js/jquery-1.7.1.min.js"></script>
    <%-- <script type="text/javascript" src="<%=ResolveUrl("~")%>js/fitscreen.js"></script>  --%>
    <script type="text/javascript">
        $(document).ready(function () {
            //var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
            //if (ww == 0) {
            //    setTimeout(function () {
            //        ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
            //    }, 1200);
            //    if (ww == 0) { ww = 320 };
            //}
            //var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
            //var pageLocation = window.location.href;
            //var scale;
            //if (pageLocation.toLowerCase().indexOf('activitypage_pad') != -1) {
            //    if (iOS) {
            //        if (ww < 768) {
            //            scale = ww / 580;
            //        } else {
            //            scale = ww / 742;
            //        }
            //    } else {
            //        scale = (ww / 950);
            //    }
            //}
            //if (ww < 950) {

            //    $('meta[name=viewport]').attr('content', 'width=950,user-scalable=yes,initial-scale=' + scale + ', maximum-scale=' + scale + ', minimum-scale=' + scale);
            //} else {
            //    $('meta[name=viewport]').attr('content', 'width=device-width,user-scalable=no,initial-scale=' + scale + ', maximum-scale=' + scale + ', minimum-scale=' + scale);
            //}
        });
    </script>
    <script src="../js/jquery-ui-1.8.18.js"></script>
    <script src="../js/jquery.blockUI.js"></script>
    <%--<script src="../js/MergeForActivityPage_Pad.js"></script>--%>
    <% If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <% End If %>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <%--<asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <%: System.Web.Optimization.Scripts.Render("~/bundles/ActivityPage_Pad") %>
    </asp:PlaceHolder>--%>

    <script type="text/javascript">
        var url = '<%=url%>';
        var DeviceId = '<%=DeviceId%>';
        var Groupname = '<%=GroupName%>'; //เก็บค่า Groupname เพื่อเอามา AddGroup
        var ActivityType = '<%=ActivityType%>';
        var IsToGetNext = false; // เข้าไป get next มาหรือยัง เวลาฝึกฝนอยู่แล้วไปเข้าควิซ
        var Examnum;
        var NotReplyMode;
        var AnswerState = '';
        var SignalRCheck;
        var JSQuizIdForShowScore = '<%=Session("QuizIdForShowScore") %>';//เก็บ QuizId สำหรับหน้า ShowScore
        var IsSelfPace; var IsPracticeMode;// โหมดฝึกฝนกับไปด้วยตัวเอง 
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var baseUrl = "<%= ResolveUrl("~/") %>";
        //if (isAndroid) {
        //url = url.replace(ResolveUrl("~/").substring(1), '');
        //}
        var F5 = '<%=F5%>';

        //Resize Screen
        //$(function () {
        //    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
        //    var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
        //        var mw = 480; // min width of site
        //        var ratio = ww / mw; //calculate ratio
        //        if (ww < mw) { //smaller than minimum size                   
        //            $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
        //        } else { //regular size
        //            $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + 950);
        //        }

        //        if (ua.indexOf("android 4.4.2") > -1) {
        //            $('meta[name=viewport]').attr('content', 'width=950,user-scalable=yes,initial-scale=0.1, maximum-scale=0.1, minimum-scale=0.1');
        //        } else if (ua.indexOf("android 4.2.1") > -1) {
        //            $('meta[name=viewport]').attr('content', 'width=950,user-scalable=yes,initial-scale=0.1, maximum-scale=0.1, minimum-scale=0.1');
        //        }
        //    }
        //});

        //url = url.replace(ResolveUrl("~/").substring(1), '');
        //url = url.replace(new RegExp(baseUrl.substring(1,baseUrl.length - 1), 'gi', '')).toString();

        function ResolveUrl(url) {
            if (url.indexOf("~/") == 0) {
                url = baseUrl + url;
            }
            return url;
        }

        console.log('Start = ' + DeviceId);

        <%  If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
        $.connection.hub.start(function () { //start hub
            SignalRCheck.server.addToGroup(Groupname);
        }).done(function () { });
        <% end If %>
        $(function () {
            //GetExamnum();
            console.log(Examnum);
            // On load
            //ReloadIframe();
            $('iframe').attr('src', '<%=ResolveUrl("~")%>' + url);

            //OpenBlockUI();
            <%If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
            SignalRCheck = $.connection.hubSignalR;
            SignalRCheck.client.send = function (message) {
                //console.log(message);

                if (message == 'Reload') { //สั้ง reload ให้เฉพาะที่ไปพร้อมกัน    
                    <% If F5 = "1" Then%>
                    Gettime();
                    <%End If%>
                    var Myvar = window.frames[0].window;
                    IsPracticeMode = Myvar.JvIsPracticeMode;
                    IsSelfPace = Myvar.JvIsSelfPace;
                    //console.log('Practice = ' + IsPracticeMode + ' && Selfpace = ' + IsSelfPace);
                    if (IsPracticeMode == "False" && IsSelfPace == 'False') { // Quiz แบบไปพร้อมกัน                          
                        ReloadIframe();
                    } else if (IsPracticeMode == "True" && IsSelfPace == 'True') { // ตอนฝึกฝน    

                        SetStudentPoint(); // update เหรียญ
                        ClearAppDeviceUniqueID();
                        url = "Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" + DeviceId;
                        window.parent.location = "<%=ResolveUrl("~")%>" + url;
                        //ReloadIframe();
                    }
            }
                    //else if (message.indexOf('ExamNum|') != -1) { // ท้ายเพจครู ส่งเลขข้อมาให้
                    //    if (IsPracticeMode == "False" && IsSelfPace == 'False') {
                    //        var ex = message.substring(8, message.length);
                    //        GetExamnum(); console.log(ex + ' กับ ' + Examnum);
                    //        if (ex != Examnum) {
                    //            ReloadIframe();
                    //        }
                    //    }
                    //}
            else if (message == "EndQuiz") {
                ClearAppDeviceUniqueID(); // function clear application ใน getnextaction  
                //var Myvar = window.frames[0].window;
                //var ShowScoreAfterComplete = Myvar.ShowScoreAfterComplete;
                var Myvar = window.frames[0].window;
                var IsQuiz = ActivityType; // quiz = 1
                if (IsQuiz != 1) {
                    window.location = '<%=ResolveUrl("~")%>Activity/ShowScore.aspx?DeviceUniqueID=' + DeviceId + '&QuizId=' + Myvar.JSQuizIdForShowScore;
                } else {
                    // เช็คด้วยว่ามีแสดงคะแนนตอนจบควิซรึปล่าว
                    if (Myvar.ShowScoreAfterComplete == 'True') {
                        window.location = '<%=ResolveUrl("~")%>Activity/ShowScore.aspx?DeviceUniqueID=' + DeviceId + '&QuizId=' + Myvar.JSQuizIdForShowScore;
                    } else {
                        window.location = '<%=ResolveUrl("~")%>practicemode_pad/choosetestset.aspx?DeviceUniqueID=' + DeviceId;
                    }
                    //window.location = '<%=ResolveUrl("~")%>practicemode_pad/choosetestset.aspx?DeviceUniqueID=' + DeviceId;
                }
            }
            else if (message.indexOf("QuizDuplicate|") != -1) {
                message = message.replace('QuizDuplicate|', '');
                //Gettime();
                ClearAppDeviceUniqueID(); //Clear application ก่อน
                GetNextAction(); // get nextaction ทำตามกระบวนการเดิม
                ReloadIframe(); // load iframe ใหม่    
                //DialogDuplicateQuiz(message);
            }
            };
            <% End If 'ปิด block needSignalR %>
        });

        function ReloadIframe() {
            //console.log('1');
            OpenBlockUI();
            //alert(url);
            //$('iframe').attr('src', '<%=ResolveUrl("~")%>' + url);
            //$('iframe').contentWindow.location.reload();
            //var f = document.getElementsByTagName("iframe");
            //f.contentWindow.location.reload();
            //document.getElementById('f').contentWindow.location.reload();
            //var iFrame = document.getElementById('f');
            //resizeIFrameToFitContent(iFrame);
            document.getElementById('f').contentWindow.location.reload();
        }

        function resizeIFrameToFitContent(iFrame) {
            iFrame.width = $(window).width();
            iFrame.height = $(window).height();
        }

        function GetExamnum() {
            var Myvar = window.frames[0].window;
            Examnum = Myvar.Examnum;
            if (Examnum == undefined) {
                setTimeout(function () {
                    Examnum = Myvar.Examnum;
                }, 1000);
            }
        }
        function Gettime() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>activity/activitypage_pad.aspx/Gettime",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    //alert(msg.d);
                },
                error: function myfunction(request, status) { }
            });
        }

        function GetNextAction() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/SendToGetNextAction",
                data: "{ DeviceUniqueID: '" + DeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    //var objJson = jQuery.parseJSON(msg.d);
                    //if (objJson.Param.NextURL !== '') {
                    //if (JvIsPracticeMode == "False") {
                    //console.log('GetNextAction = ' + DeviceId);
                    //BlockUiBeforeLoad();
                    //url = "Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" + DeviceId;
                    //window.parent.location = "<%=ResolveUrl("~")%>" + url;
                    //} else {
                    //$('#dialog').dialog({
                    //autoOpen: open,
                    //buttons: { 'ตกลง': function () { window.location = "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx?DeviceUniqueID=" + DeviceId; } },
                    //draggable: false, resizable: false, modal: true
                    //});
                    //}
                    //}
                },
                error: function myfunction(request, status) { }
            });
        }

        function SetStudentPoint() { // Set Score ให้เด็กก่อนจะเปลี่ยนหน้าเข้าควิซ
            var Myvar = window.frames[0].window;
            var PlayerId = Myvar.PlayerId;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetStudentPoint",
                data: "{ PlayerId: '" + PlayerId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                },
                error: function myfunction(request, status) {
                }
            });
        }

        function ClearAppDeviceUniqueID() {
            $.ajax({
                type: "POST",
                //url: "<%=ResolveUrl("~")%>activity/activitypage_pad2.aspx/ClearAppDeviceUniqueID",
                url: "<%=ResolveUrl("~")%>webservices/StudentService.asmx/ClearAppDeviceUniqueID",
                data: "{ DeviceUniqueID : '" + DeviceId + "'}",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) { },
                error: function myfunction(request, status) { }
            });
        }

    var isMaxOnet = '<%=BusinessTablet360.ClsKNSession.IsMaxONet%>';
        var responseTimeCI = "<%=BusinessTablet360.MaxOnetCI.ResponseTime%>";       
        var refreshUrl;
        var ispreview = '<%=Request.QueryString("ispreview")%>';
       
        $(function () {
            if (ispreview != "true") {
                blockUISpinner();
            }
        });

        function OpenBlockUI() {
            blockUISpinner();                  
        }

        function CloseBlockUI() {           
            $.unblockUI();         
        }

        function blockUISpinner() {
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: 'initial',
                    color: '#fff'
                },
                message: '<img src="<%=ResolveUrl("~")%>Images/waitspinner.gif" width="100" height="100" /><br /><span style="font: normal 35px THSarabunNew;">รอสักครู่นะคะ</span>'
            });            
        }       

        function DialogDuplicateQuiz(m) {
            $('#dialog').dialog({
                autoOpen: open,
                buttons: {
                    'ตกลง': function () {
                        ClearAppDeviceUniqueID(); //Clear application ก่อน
                        GetNextAction(); // get nextaction ทำตามกระบวนการเดิม
                        ReloadIframe(); // load iframe ใหม่                        
                        $(this).dialog('close');
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            }).dialog('option', 'title', 'โดนดึงเข้าควิซโดย คุณครู ' + m + ' ค่ะ');
        }


    </script>

</head>
<body style="background-color: #380604!important;">
    <form id="form1" runat="server">
        <iframe style="height: 99%; width: 100%; position: absolute; top: 0; left: 0; border:none;" id="f"></iframe>
        <div class="block" style="position: absolute; width: 100%; height: 100%; background-color: black; display: none; top: 0; left: 0;"></div>
    </form>
    <div id="dialog" title="โดนดึงเข้าควิซโดยครูคนอื่นค่ะ">
    </div>


    <script type="text/javascript">
        var isFirstTouch;

        var soundList = ['bgmusic01.mp3', 'bgmusic02.mp3', 'bgmusic03.mp3', 'bgmusic04.mp3', 'bgmusic05.mp3', 'bgmusic06.mp3', 'bgmusic07.mp3', 'bgmusic08.mp3', 'bgmusic09.mp3', 'bgmusic10.mp3'];
        var n = Math.floor((Math.random() * soundList.length));
        var mySound = new sound("<%=ResolveUrl("~")%>sounds/maxonet/" + soundList[n]);
        var timeToPlayBGMusic;

        function sound(src) {
            this.sound = document.createElement("audio");
            this.sound.src = src;
            this.sound.setAttribute("preload", "auto");
            //this.sound.setAttribute("controls", "none");
            document.body.appendChild(this.sound);
            this.play = function () {
                this.sound.pause();
                this.sound.currentTime = 0;
                this.sound.play();
            }
            this.stop = function () {
                this.sound.pause();
            }
        }
        function playBgMusic() {
            if (isFirstTouch === undefined || isFirstTouch == false) {
                mySound.play();
                timeToPlayBGMusic = setInterval(function () {
                    mySound.play();
                }, 1900);
                isFirstTouch = true;
            }
        }

        function muteBGMusic() {
            //clearInterval(timeToPlayBGMusic);
            //mySound.muted = true;
            mySound.stop();
        }

        function unmuteBGMusic() {
            mySound.muted = false;
        }

    </script>
</body>
</html>
