<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityPage_PadTeacher.aspx.vb"
    Inherits="QuickTest.ActivityPage_PadTeacher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <link href="../css/styleActivity_pad.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.qtip.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleEnabledTools.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script src="../js/jrumble.js" type="text/javascript"></script>
    <script src="../js/jsEnabledTools.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip.js" type="text/javascript"></script>
    <script src="../js/jQueryRotateCompressed.2.2.js" type="text/javascript"></script> 
    <% If tools_Calculator = True Then%>   
    <script src="../js/jsToolsCalculator.js" type="text/javascript"></script>
    <%End If%>
    <script type="text/javascript">
        var SignalRCheck;
        var Groupname;
        var GroupOld;

        var Unload,CurrentPage,SignalRCheck;
        var withOutClick = true;
        var changePage = false;
        var firstClick = true;
       
        Groupname = '<%=GroupName %>'; // เก็บ GroupName
        GroupOld = '<%=GroupOld %>';

        var thisPage = '<%=ResolveUrl("~")%>activity/activitypage.aspx';
        thisPage = thisPage.toLowerCase();
        var step1Page = '<%=ResolveUrl("~")%>testset/step1.aspx';
        step1Page = step1Page.toLowerCase();
        var alternativePage = '<%=ResolveUrl("~")%>activity/alternativepage.aspx';
        alternativePage = alternativePage.toLowerCase();
        var settingPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx';
        settingPage = settingPage.toLowerCase();

        $(function () {
        
            ////////////////////////////////////////////////////// Code ส่วน Sycn(SignalR)

            SignalRCheck = $.connection.hubSignalR;

            SignalRCheck.client.send = function (message) {
                if (message == step1Page) {
                    window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                }
                else if (message == settingPage) {
                    window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx';
                }
                else if (message == thisPage) {
                    //window.location = '../Activity/ActivityPage_PadTeacher.aspx';
                }
                else if (message == alternativePage) {
                    window.location = '<%=ResolveUrl("~")%>Activity/Alternative_Pad.aspx';
                }
                else if (message == 'Reload') {
                }
                else {
                    window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx';
                }
            };
            //ขาส่ง ถ้า HiddenField เป็นคำว่า Reload แสดงว่าครูกด Next - Previous แล้ว
            SignalRCheck.client.cmdControl = function (cmd) {
                if (cmd == 'Next') {
                    firstClick = false;
                    $('#btnNext').trigger('click');
                }
                else if (cmd == 'Prev') {
                    firstClick = false;
                    $('#btnPrev').trigger('click');
                }
            };

            // รับค่าของตัวเองหลังจากที่ส่งไป
            SignalRCheck.client.raiseEvent = function (cmd) {
        
                if (cmd == 'Next') {
                    firstClick = false;
                    $('#btnNext').trigger('click');
                }
                else if (cmd == 'Prev') {
                    firstClick = false;
                    $('#btnPrev').trigger('click');
                }
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(Groupname);
                if ($('#HDCheckChangeQuestion').val() == 'Reload') {
                    SignalRCheck.server.sendCommand(Groupname, 'Reload');
                    $('#HDCheckChangeQuestion').val('Null');
                }

                SignalRCheck.server.addToGroup(GroupOld);
                SignalRCheck.server.sendCommand(GroupOld, thisPage);

               
                
            });
            // function เช็ค ข้อสุดท้าย
            var ShowDialog = '<%= Dialog %>';
            var titleDialog = "<%=DialogTitle %>"; 

            // btnNext
            $('#btnNext').click(function (e) {      
                if (firstClick) {             
                    e.preventDefault();
                    if (ShowDialog == "True") {                        
                        $('#dialog').dialog('option', 'title', titleDialog).dialog('open');
                    }
                    else {
                        SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Next');
                    }              
                }
            });
            $('#btnPrev').click(function (e) {
                if (firstClick) {
                    e.preventDefault();
                    SignalRCheck.server.cmdControlBtnPrevNext(GroupOld, 'Prev');                    
                }
            })  
            //////////////////////////////////////////////////////
        });


        function setUnload(unload) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setUnload", 
                  data:"{ unload : '" + unload + "'}",                 
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                              
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
            }
        function  getUnload() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/getUnload",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                 
                        Unload = data.d;                        
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
        function setCurrentPage(thisPage) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setCurrentPage",
                  data:"{ thisPage : '" + thisPage + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                              
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
        function  getCurrentPage() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/getCurrentPage",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                 
                        CurrentPage = data.d;                        
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
    </script>
    <script type="text/javascript">
        function playSound(soundfile) {
            document.getElementById("voice").innerHTML =
            //"<embed src=\"" + soundfile + "\" hidden=\"true\" autostart=\"true\" loop=\"false\" />";
            "<video id='video' width='0' height='0'><source src=\"" + soundfile + "\" /></video>";
            var video = document.getElementById("video");
            video.play();
        }

        $(document).ready(function () {
            //update 22-05-56 tools on tablet
            var QuestionId = '<%= questionId %>';
            $('#hdQuestionId').val(QuestionId);
            var UserId = '<%= UserId %>';
            $('#hdUserId').val(UserId);


            $('#BtnChkTablet').click(function () {
                getStudentCheckInClass();
                $('#DivChkTablet').show();
                $('#blockUI').show();
                setInterval(function () {
                    getStudentCheckInClass();
                }, 2000);

            });

            $('#blockUI').click(function () {
                $('#DivChkTablet').hide();
                $('#blockUI').hide();
            });

            //               $('#BtnSwapQuestion').toggle(
            //               function(){
            //    
            //                $('#slideDiv').stop().show(500);
            //                CreateButtonLeapChoice("True");
            //                //UseSlide();
            //                },
            //                function(){
            //                $('#slideDiv').stop().hide(500);
            //                }
            //                )
            //            
            //                $('#BtnSortNormal').click(function(){
            //                CreateButtonLeapChoice("True");
            //                });

            //                $('#BtnSortAnswerNull').click(function(){
            //                CreateButtonLeapChoice("False");
            //                });


            $('#BtnExitQuiz').click(function (e) {
                var title = 'ต้องการออกจากควิซใช่หรือไม่ ?';
                setCurrentPage('/testset/step1.aspx');
                $('#dialog').dialog('option', 'title', title).dialog('open');
            });
            $('#dialog').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () { window.location = '../Activity/Alternative_Pad.aspx'; }, 'ไม่': function () {
                    $(this).dialog('close');
                }
                },
                draggable: false,
                resizable: false,
                modal: true
            });

        });


        ////////////////////////////////////////////////////////////////////////
        //               function CreateButtonLeapChoice(IsNormalSort) {
        //              
        //       $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>Activity/ActivityPage_Pad.aspx/CreateStringLeapChoice",
        //	            //data: "{ DeviceUniqueId: '" + DVIDJS + "'}",  //" 
        //                data: "{ IsNormalSort:'" + IsNormalSort + "'}",   
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (msg) {
        //                    nextCmd = msg.d;
        //                    //alert(msg.d);
        //                    $('#LeapChoiceDiv').html(msg.d);     
        //                    UseSlide();
        //	            },
        //	            error: function myfunction(request, status)  {
        //                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	            }
        //	        });
        //        }

        //                function UseSlide() {
        //                    $('#slideDiv').slides({
        //                    generatePagination:false
        //                    });
        //                }


    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnNext').click(function(e){
                if (IsLastQuestion()) {
                    e.preventDefault();
                    $('#dialog').dialog('open');
                }
            });
//            
//            $('#btnPrev').click(function(){
//                keepTime();
//            });
                       
            getModeQuizAndTimer();    
        });
        
        // function ข้อสุดท้ายหรือเปล่า
        var ShowDialog = '<%= Dialog %>';
        function IsLastQuestion() {            
            if (ShowDialog == 'True') {
                return true;
            }
            else {
                return false;
            }
        }    

        function getModeQuizAndTimer() {
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx/getModeQuizAndTimer",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {                   
                    var needTimer = jQuery.parseJSON(data.d);                 
                    var minute;var second;
                    if(needTimer.NeedTimer == true){  
                        $('#TimeCountDown').show();
                        if(needTimer.timerType == true){
                            minute = Math.floor(needTimer.AllTime/60);
                            second = parseInt(needTimer.AllTime)%60;

                            var sums = parseInt((minute * 60) + second);
                            $('#keepSumseconds').val(sums);
                            timeCountDown(minute,second,true);                                              
                        }
                        else {                                                 
                            minute = Math.floor(needTimer.TimeRemain / 60);
                            second = parseInt(needTimer.TimeRemain % 60);
                            var timeTotal = needTimer.timeTotal * 60;

                            $('#keepSumseconds').val(timeTotal);
                            timeCountDown(minute,second,false,needTimer.noWatch); 
                        }                        
                    }
                    else{
                        $('#TimeNormal').show();
                        minute = Math.floor(needTimer.AllTime / 60);
                        second = parseInt(needTimer.AllTime % 60);
                        timeNormal(minute,second);
                    }
                },
                error: function myfunction(request, status) {
                    alert('jeng');
                } 
            });
        }

        function timeCountDown(min,seconds,type,noWatch) {

            var widthDiv = 0;
            var resizeDiv; var sumSeconds;

            var second_time = setInterval(function () {
                seconds = seconds - 1;
                // check second for change minute
                if (seconds == -1) {
                    seconds = 59;
                    //min = parseInt($("#minute").text());
                    min = min - 1;
                    if (min == -1) {
                        clearInterval(second_time);
                        min = 0;
                        seconds = 0;
                        //trigger Or end quiz
                        if(type){
                            $('#btnNext').trigger('click');
                        }
                        else{
                            //show dialog end quiz
                            if (!noWatch) {
                                $('#dialogIsLastQuestion').dialog('option', 'title', 'หมดเวลาสอบแล้วค่ะ ต้องส่งข้อสอบได้แล้ว').dialog('open');
                            }
                        }
                    }
                }
                // check second for add "0" when length = 1                
                if (seconds < 10) {
                    seconds = "0" + seconds;
                    if (min == 0) {
                        $('#firstBomb').jrumble({ rumbleEvent: 'constant' });
                    }
                }
                // resize div               
                if (widthDiv == 0) {
                    widthDiv = $("#secondBomb img").width();                                     
                }               
                //alert(parseInt(seconds,10));
                sumSeconds = parseInt((min * 60) + parseInt(seconds,10));
                
                var fullSeconds = $('#keepSumseconds').val();
                var lostSeconds = fullSeconds - sumSeconds; 
                resizeDiv = (((1 / fullSeconds) * 100) * lostSeconds ) * (widthDiv / 100);    

               
                //$("#secondBomb img").live().css("width", (widthDiv - resizeDiv) + "px");      
                $("#secondBomb").children('img').css("width", (widthDiv - resizeDiv) + "px");      
                         
                
                $('#second').text(seconds);
                $("#minute").text(min);

            }, 1000);
        }

        function timeNormal(minN,secondsN) {

            var second_timeN = setInterval(function () {
                secondsN++;
                // check second for change minute
                if (secondsN == 60) {
                    secondsN = 0;
                    minN = parseInt(minN) + 1;
                }
                // check second for add "0" when length = 1                
                if (secondsN < 10) {
                    secondsN = "0" + secondsN;
                }

                $('#secondN').text(secondsN);
                $("#minuteN").text(minN);

            }, 1000);
        }

        function getStudentCheckInClass() {
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/activitypage_padteacher.aspx/getStudentCheckInClass",
                async: false,                                  
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {         
                    var _html = "<span>ลงชื่อแล้ว</span><div id='divReloadChkTablet' style='border: initial;font-size:30px;'></div><span>คน</span></br>";                
                    _html += data.d;               
                    $('#DivChkTablet').html(_html);
                },
                error: function myfunction(request, status) {   
                    alert(status);                      
                }
            });
        }
        
    </script>
    <script type="text/javascript">
        $(function () {            
            $('.wordsDict').live('click', function (e) {
                getSelectedOnTablet(e.target.innerHTML);
                $('span').removeClass('spanHilight');
                $(this).addClass('spanHilight');
            });
        });

        function getSelectedOnTablet(selection) {
            selection = selection.replace(/\_/g,'');            
            selection = $.trim(selection);
            if (IsWord(selection) == true) {
                selection = '';
            }
            if (selection != "") {
                $.ajax({ type: "POST",
                    url: "<%=ResolveUrl("~")%>selectedSearch/selectedSearch.aspx/translateFromSelected",
                    data: "{ selectedText : '" + selection + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        var dict = jQuery.parseJSON(data.d);
                        var headDict = selection.toString() + dict.returnFileMp3.toString();
                        var contentDict = dict.returnValue;
                        if (contentDict == '') {
                            contentDict = 'ไม่มีคำแปลค่ะ';
                        } 
//                        $('#qtipp').qtip({
//                            content: {
//                                text: contentDict, //data.d,
//                                title: {
//                                    text: headDict, button: true
//                                }
//                            },
//                            show: { ready: true },
//                            style: { classes: 'ui-tooltip-shadow ui-tooltip-tipped myQTip', width:250 },
//                            position: { my: 'center', at: 'bottom center' },
//                            events:{ hide: function(e, api) { $('span').removeClass('spanHilight'); }}
//                        });
                        $('#tools_dictionary .headDict').html(headDict);
                        $('#tools_dictionary .contentDict').html(contentDict);  
                        $('#tools_dictionary').show();       
                    },
                    error: function myfunction(request, status) {
                        // alert("ต่อไม่ได้");                      
                    }
                });
            } else {
                $('.ui-tooltip-tipped').remove();                
            }            
        }
        function IsWord(w) {
            var a = w.split(' ');
            if (a.length > 1) {
                return true;
            }
            else {
                return false;
            }
        }        
    </script>       
    
    <%--<meta name="viewport" content="width=300,initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=300" />--%>
</head>
<body style="height: auto;">
    <form id="form1" runat="server">
    <span id="voice"></span>
    <div id='MainDiv' style='width: 980px; height: 645px; /*background-image: url(../Images/Activity/bg5.png)*/;
        z-index: 0;'>
        <div id="panelTime" style="background: rgb(255, 152, 29); width: 300px; float: left;
            height: 48px; -webkit-border-radius: 5px; margin: 5px; padding: 10px;">
            <div id="TimeCountDown" style="display: none;">
                <div id="firstBomb" style="width: 80px;">
                    <img src="../Images/Activity/Timer/bomb01.jpg" />
                    <div style="position: absolute; left: 25px; top: 8px;">
                        <span id="minute"></span><span>:</span><span id="second"></span>
                    </div>
                </div>
                <div id="secondBomb" style="">
                    <img src="../Images/Activity/Timer/slate.png" style="width: 300px;" />
                </div>
                <div id="thirdBomb" style="">
                    <img src="../Images/Activity/Timer/fragment.png" style="width: 20px;" />
                </div>
            </div>
            <div id="TimeNormal" style="display: none; background-color: Red;">
                <div style="position: absolute; left: 25px; top: 8px;">
                    <span id="minuteN"></span><span>:</span><span id="secondN"></span>
                </div>
            </div>
        </div>
        <div id="PanelTestsetName" style="background-color: rgb(255, 173, 0); width: 390px;
            float: left; padding: 10px; overflow: hidden; height: 48px; -webkit-border-radius: 5px;
            margin: 5px;">
            <asp:Label ID="lblTestsetName" runat="server"></asp:Label></div>
        <%-- Update 22/05/56 Tools On Tablet --%>
        <% If useTools = True Then%>
        <div id="PanelTools">
        </div>
        <div id="ToolsOnTablet">
            <% If tools_Calculator = True Then%>
            <div class="ForToolsTablet btnCalculator" style='margin-top: 10px; '>
                <a><span>เครื่องคิดเลข</span></a></div>
            <% End If%>
            <% If tools_Dictionary = True Then%>
            <div class="ForToolsTablet btnDictionary DictOff" style='margin-top: 10px; '>
                <a><span>แปลศัพท์</span></a></div>
            <% End If%>
            <% If tools_WordBook = True Then%>
            <div class="ForToolsTablet btnWordBook" style='margin-top: 10px; '>
                <a><span>สมุดคำศัพท์</span></a></div>
            <% End If%>
            <% If tools_Note = True Then%>
            <div class="ForToolsTablet btnNote" style='margin-top: 10px; '>
                <a><span>กระดาษโน๊ต</span></a></div>
            <% End If%>
            <% If tools_Protractor = True Then%>
            <div class="ForToolsTablet btnProtractor" style='margin-top: 10px; '>
                <a><span style="font-size: 25px;">ไม้โปรแทรคเตอร์</span></a></div>
            <% End If%>
        </div>
        <%End If%>
        <%--  <div id="BtnSwapQuestion" style="background-color: #FE7C22; width: 100px; float: right;
            margin: 5px;font-size:30px;height:68px;">
            เลือกข้อ</div>--%>
        <div style="clear: both;">
        </div>
        <div style="float: right; margin-top: -68px; margin-right: 7px;" id="panelMenuRight">
            <div id="BtnChkTablet" style="margin-bottom: 5px;">
                <img src="../Images/Activity/CheckTab.png" style="width: 90px; height: 150px; cursor: pointer;" /></div>
            <div id="BtnNext" style="background-image: url('../Images/Activity/next-icon.png');
                margin-bottom: 5px; cursor: pointer;">
                <asp:Button ID="btnNext" runat="server" Text="" Height="150px" Width="90px" Style="background: transparent;
                    border: none; cursor: pointer;" /></div>
            <div id="BtnPrev" style="background-image: url('../Images/Activity/forward-icon.png');
                margin-bottom: 5px; cursor: pointer;">
                <% If ViewState("_IsPerQuestion") = False Then%>
                <asp:Button ID="btnPrev" runat="server" Text="" Height="150px" Width="90px" Style="background: transparent;
                    border: none; cursor: pointer;" /><%End If%></div>
            <div id="BtnExitQuiz" style="background-color: rgb(255, 0, 0); margin-bottom: 5px;
                cursor: pointer;">
                <img src="../Images/upgradeClass/logout.png" style="width: 90px; height: 150px; cursor: pointer;" /></div>
        </div>
        <div id='DivMainQuestionAnswer' style='overflow: auto; height: 545px; margin-left: 5px;'>
            <div id="mainQuestion" runat="server" class="Question" style="position: relative;
                -webkit-border-radius: 10px 10px 0px 0px; background-color: #ffc76f; width: 830px;
                border: 20px; padding: 20px;">
            </div>
            <div id="qtipp" style="position: relative; width: 830px;">
            </div>
            <div id="mainAnswer" runat="server" style="width: 830px; position: relative; background-color: #F4F7FF;
                padding: 20px; -webkit-border-radius: 0px 0px 10px 10px;">
                <table id="Table1" runat="server" style="border-collapse: collapse;">
                    <tr>
                        <td runat="server" id="AnswerTbl">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="DivChkTablet">
        </div>
        <%--    <div id='slideDiv' style='width: 725px; height: 420px; background-color: Gray; position: absolute;
            margin-left: 70px;display:none;margin-top:-255px;'>
            <div id='LeapChoiceDiv' class='slides_container' style='height:300px;'>
            
            </div>

            <a href='#' style='margin-top:-115px;position:absolute;' class='prev'><img src="../Images/Activity/AllNewArrow/leftBlue.png" /></a> 
            <a href='#' style='margin-top:-115px; margin-left:655px; position:absolute;' class='next'><img  src="../Images/Activity/AllNewArrow/rightBlue.png" /></a>
            <div id='DivSortBtn'>
            <input type="button" id='BtnSortNormal' value='เรียงตามลำดับข้อ' style='margin-left:170px;;width:150px;height:60px;' />
            <input type="button" id='BtnSortAnswerNull'  value='เรียงโดยเริ่มจากข้อที่ยังไม่ได้ทำ' style='margin-left:45px;width:150px;height:60px;' />

            </div>--%>
        <div id="blockUI" style="width: 980px; height: 645px; background-color: Black; position: absolute;
            top: 0; opacity: 0.7; display: none; z-index: 1;">
        </div>
    </div>
    <div id="dialog" title="">
    </div>
    <input type="hidden" id="hdKeepMin" runat="server" />
    <input type="hidden" id="hdKeepSec" runat="server" />
    <input type="hidden" id="hdKeepMinN" runat="server" />
    <input type="hidden" id="hdKeepSecN" runat="server" />
    <input type="hidden" id="hdNum" runat="server" value="1" />
    <input type="hidden" id='HDCheckChangeQuestion' runat="server" />
    <input type="hidden" id='hdCmdOldSession' runat="server" />
    <input type="hidden" id="keepSumseconds" runat="server" />
    <%-- Update 22-05-56 Tools On Tablet --%>
    <input type="hidden" id="hdQuestionId" runat="server" />
    <input type="hidden" id="hdUserId" runat="server" />
    <input type="hidden" id="hdStatusDict" runat="server" value="Off" />
    <input type="hidden" id="hdIsGroupEng" runat="server"  />
    <% If tools_Calculator = True Then%>
    <div id="tools_calculator">
        <div id="CalculatorPanel">
            <div id="txtCalculator">
                <span id="SumNumber" style="font-size: 35px;">0</span>
            </div>
            <div id="btnCalculator">
                <div class="btnOperation btnFirstCol btnCalculatorDiv" onclick='calculator("/")'>
                    /
                </div>
                <div class="btnOperation btnSecondtCol btnCalculatorDiv" onclick='calculator("*")'>
                    *
                </div>
                <div class="btnOperation btnThirdCol btnCalculatorDiv" onclick='calculator("-")'>
                    -
                </div>
                <div class="btnClear btnForthCol btnCalculatorDiv" onclick='document.getElementById("SumNumber").innerHTML = "0"'>
                    C
                </div>
                <div class="btnNumeric btnFirstCol btnSecondRow btnCalculatorDiv" onclick='calculator("7")'>
                    7
                </div>
                <div class="btnNumeric btnSecondtCol btnSecondRow btnCalculatorDiv" onclick='calculator("8")'>
                    8
                </div>
                <div class="btnNumeric btnThirdCol btnSecondRow btnCalculatorDiv" onclick='calculator("9")'>
                    9
                </div>
                <div class="btnOperation btnForthCol btnSecondRow btnCalculatorDiv" style="height: 97px;"
                    onclick='calculator("+")'>
                    +
                </div>
                <div class="btnNumeric btnFirstCol btnThirdRow btnCalculatorDiv" onclick='calculator("4")'>
                    4
                </div>
                <div class="btnNumeric btnSecondtCol btnThirdRow btnCalculatorDiv" onclick='calculator("5")'>
                    5
                </div>
                <div class="btnNumeric btnThirdCol btnThirdRow btnCalculatorDiv" onclick='calculator("6")'>
                    6
                </div>
                <div class="btnSumary btnForthCol btnFifthRow btnCalculatorDiv" style="height: 97px;"
                    onclick='calculator("=")'>
                    =
                </div>
                <div class="btnNumeric btnFirstCol btnForthRow btnCalculatorDiv" onclick='calculator("1")'>
                    1
                </div>
                <div class="btnNumeric btnSecondtCol btnForthRow btnCalculatorDiv" onclick='calculator("2")'>
                    2
                </div>
                <div class="btnNumeric btnThirdCol btnForthRow btnCalculatorDiv" onclick='calculator("3")'>
                    3
                </div>
                <div class="btnNumeric btnFirstCol btnFifthRow btnCalculatorDiv" style="width: 148px;"
                    onclick='calculator("0")'>
                    0
                </div>
                <div class="btnNormal btnThirdCol btnFifthRow btnCalculatorDiv" onclick='calculator(".")'>
                    .
                </div>
            </div>
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_Note = True Then%>
    <div id="tools_note">
        <div id="noteMain">
            <div class="noteTabs">
                <div class='noteHeadTab'>
                    <input type='radio' id='tab-1' name='tab-group' checked='checked' />
                    <label for='tab-1'>
                        กระดาษทด</label><div class='content' id='myClipboard'>
                            <textarea></textarea></div>
                </div>
                <div class='noteHeadTab'>
                    <input type='radio' id='tab-2' name='tab-group' /><label for='tab-2'>
                        สมุดโน๊ต</label><div class='content' id='myNote'>
                        </div>
                </div>
            </div>
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_WordBook = True Then%>
    <div id="tools_wordbook">
        <div id="wordbookMain">
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    <% If tools_Protractor = True Then%>
    <div id="tools_protractor">
        <div id="divImg" >
            <img id="imgPro" src="../Images/Activity/Setting_Tools/pro.png" width="500" height="279.5"
                alt="" /></div>
        <div id="btnRotateL" class="btnRotate">
            L</div>
        <div id="btnRotateR" class="btnRotate">
            R</div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>    
    <% If tools_Dictionary = True Then%>
    <div id="tools_dictionary">
        <div class="headDict">
        </div>
        <div class="contentDict" style="font-size:21px;">
        </div>
        <a class="Aclosed">
            <div>
            </div>
        </a>
    </div>
    <%End If%>
    </form>
</body>
</html>
