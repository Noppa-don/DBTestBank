<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SettingActivity_PadTeacher.aspx.vb"
    Inherits="QuickTest.SettingActivity_PadTeacher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyLoader.js" type="text/javascript"></script>
    <link href="../css/prettyLoader.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

            var Unload,CurrentPage,SignalRCheck;
            var withOutClick = true;var changePage = false;

//            $(window).bind('beforeunload', function () {                
//                if (withOutClick == true) {
//                     setUnload(true); // unload = true          
//                }                    
//            });

            var groupname = '<%=GroupName %>';
     
            var thisPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx'; 
            thisPage = thisPage.toLowerCase();     
            var nextPage = '<%=ResolveUrl("~")%>activity/activitypage.aspx'; 
            nextPage = nextPage.toLowerCase();
            var step1Page = '<%=ResolveUrl("~")%>testset/step1.aspx';
            step1Page = step1Page.toLowerCase();

            // ถ้าเป็นการ unload ให้ทำการ save หน้าปัจจุบันลงไปที่ application selectsession ด้วย
           getUnload();
            if (Unload == true) {
                setCurrentPage(thisPage);
                setUnload(false);
                changePage = true;            
            }
            else {
                // check ว่าหน้าที่เปิดอยู่เป็นหน้าปัจจุบันหรือไม่ ถ้าไม่ใช่ให้ redirect ไปยังหน้าปัจจุบัน
                getCurrentPage();
                if (CurrentPage != thisPage) {
                    withOutClick = false;
                    if(CurrentPage == step1Page){
                         window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                    }
                    else if(CurrentPage == nextPage){
                         window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
                    }
                    else{
                        window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx';
                    }                    
                }
               
            }
                

            SignalRCheck = $.connection.hubSignalR;

            SignalRCheck.client.send = function (message) {                         
                if (message == step1Page) {
                       window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
                   }
                   else if (message == thisPage) {
                       //window.location = '../Activity/SettingActivity_PadTeacher.aspx';
                   }
                   else if (message == nextPage) {
                       window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
                   }
                   else {
                       window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx';
                   }
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(groupname);
                SignalRCheck.server.sendCommand(groupname, thisPage);
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
    <style type="text/css">
        .divAllSetting
        {
            background-color: #B4DCED;
            width: 50%;
            height: 100%;
            vertical-align: middle;
            -webkit-border-radius: 5em;
            line-height: 3;
            margin-left: auto;
            margin-right: auto;
        }
        /*span
        {
            font-size: larger;
        }*/
        .prettyLoader
        {
            left: 500px;
            top: 300px;
        }
        input[type=checkbox]:disabled + label
        {
            color: Gray;
            background-image: url(../images/bullet-disable.gif);
            font-size: 18px;
        }
    </style>
    <script type="text/javascript">
    $(document).ready(function(){
         $.prettyLoader({ animation_speed:'slow',bind_to_ajax:true});
    });
        $(function () {

            
            
            ShowHideTimechk();
            ShowHideShowAnswer();


            $('#DialogLV').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                width: 'auto'
            });

            $('#btnChageLV').click(function () {
                $('#DialogLV').dialog('open');
            });

            $('input[type=radio]').change(function(){ 

                if($('#rdbAnswerAfter').attr('checked') == 'checked'){                   
                    $('#ChkInShow1').attr('disabled',true);
                    $('#ChkInShow1').attr('checked',false);
                    $('#txtTimeShowAnswer').attr('disabled',true);
                }
                else{
                    $('#ChkInShow1').attr('disabled',false);
                    $('#txtTimeShowAnswer').attr('disabled',false);
                }
            });        
        });


    function ChangeLv(level) {
        $('#lblLevel').text(level);
        $('#HiddenTest').val(level);
        $('#DialogLV').dialog('close');
    }


    

    function ShowHideInShowAnswer() {

        if ($('#Show1').attr('checked') == 'checked') {
            $('#ForShow1').stop(true, true).show(500);
        }
        else {
            $('#ForShow1').stop(true, true).hide(500);
        }
        

    };
   
    $(function () {
        var delayID = null;
        $('#txtRoom').keyup(function (e) {            
            if (delayID == null) {
//            var ClassName = $('#lblLevel').text();
//            var RoomName = $('#txtRoom').val();
//            
//                delayID = setTimeout(function () {
//                    GetStudentAmount(ClassName,RoomName);
//                }, 500);
            }
            else if (delayID != null) {
                clearTimeout(delayID);
            
            }
            var ClassName = $('#lblLevel').text();
            var RoomName = $('#txtRoom').val();
                delayID = setTimeout(function () {
                    GetStudentAmount(ClassName,RoomName);
                }, 500);
        });
    });
    function GetStudentAmount(ClassName,RoomName) {  
                  
	                $.ajax({ type: "POST",
	                    url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetStudentAmountCodeBehide",
                        async: false, // ทำงานให้เสร็จก่อน
	                    data: "{ClassName : '" + ClassName + "', RoomName : '" + RoomName + "' }",  //" 
	                    contentType: "application/json; charset=utf-8", dataType: "json",   
	                    success: function (msg) {
	                    valReturnFromCodeBehide = msg.d;  
                        //$('#txtTotalStudent ').val(valReturnFromCodeBehide);  
                        $('#txtTotalStudent ').html(valReturnFromCodeBehide);                
	                    },
	                    error: function myfunction(request, status)  {  
                    
	                    }
	                });
                    }


    function GetTime(TimePer,StaPer) {  
                  
	                $.ajax({ type: "POST",
	                    url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/GetStudentAmountCodeBehide",
                        async: false, // ทำงานให้เสร็จก่อน
	                    data: "{Time : '" + Time + "', StaPer : '" + StaPer + "' }",  //" 
	                    contentType: "application/json; charset=utf-8", dataType: "json",   
	                    success: function (msg) {
	                    valReturnFromCodeBehide = msg.d;  
                        $('#txtTotalStudent ').val(valReturnFromCodeBehide);            
	                    },
	                    error: function myfunction(request, status)  {  
                    
	                    }
	                });
                    }              
    
    
    </script>
    <script type="text/javascript">
        $(function () {
            //$('#txtTimePerQuestion').keyup(function (e) {   
            $('.txtTime').keyup(function (e) {
                var delayID = null;
                var idTxt = $(this).attr('id');
                if (delayID == null) {
                    delayID = setTimeout(function () {
                        if (idTxt == 'txtTimePerQuestion') {
                            checkTimerPerQuestionSec($('#txtTimePerQuestion').val());
                        }
                        else if (idTxt == 'txtTimeAll') {
                            checkTimeAllQuestion($('#txtTimeAll').val());
                        }
                        else if (idTxt == 'txtTimeShowAnswer') {
                            checkTimerSolve($('#txtTimeShowAnswer').val());
                        }
                    }, 1500);
                }
                else if (delayID != null) {
                    clearTimeout(delayID);
                    delayID = setTimeout(function () {
                        if (idTxt == 'txtTimePerQuestion') {
                            checkTimerPerQuestionSec($('#txtTimePerQuestion').val());
                        }
                        else if (idTxt == 'txtTimeAll') {
                            checkTimeAllQuestion($('#txtTimeAll').val());
                        }
                        else if (idTxt == 'txtTimeShowAnswer') {
                            checkTimerSolve($('#txtTimeShowAnswer').val());
                        }
                    }, 1500);
                }
            });
        });

        function checkTimerPerQuestionSec(timerPerQuestion) {
            var timer = parseInt(timerPerQuestion); // ค่าจาก textbox เวลาต่อข้อ
            var questionAmount = parseInt($('#lblQuestionAmount').html()); // จำนวนข้อ           

            if (timer < 10) {
                $('#txtTimePerQuestion').val(10);
                $('#txtTimePerQuestion').qtip({
                    content: 'ต้องกรอกเวลาทำควิซข้อละ 10 วินาทีขึ้นไปค่ะ',
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimePerQuestion') },
                    //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                var timerPerQ = ((10 * questionAmount) / 60).toFixed(2);
                $('#lblTimeAllPerQuestion').html(timerPerQ);
            }
            else {
                var timerPerQ = ((timerPerQuestion * questionAmount) / 60).toFixed(2);
                $('#lblTimeAllPerQuestion').html(timerPerQ);
            }
        }
        function checkTimeAllQuestion(timeAll) {
            var timer = parseInt(timeAll); //ค่าจาก textbox เวลาทั้งหมด
            var questionAmount = parseInt($('#lblQuestionAmount').html()); //จำนวนข้อ
            var timerPerQuestion = ((timer * 60) / questionAmount); //เวลาเป็นนาทีมาเฉลี่ยต่อข้อ

            if (timerPerQuestion < 10) {
                var timerPerAll = Math.round((10 * questionAmount) / 60);
                $('#txtTimeAll').val(timerPerAll);
                var timerPerQ = ((timerPerAll * 60) / questionAmount).toFixed(2);
                $('#lblTimeAll').html(timerPerQ);
                // qtip
                $('#txtTimeAll').qtip({
                    content: 'ต้องกรอกเวลาทำควิซข้อละ 10 วินาทีขึ้นไปค่ะ',
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimeAll') },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
            }
            else {
                var timerPerQ = ((timer * 60) / questionAmount).toFixed(2);
                //var timerPerQ = ((timer * 60) / questionAmount);
                $('#lblTimeAll').html(timerPerQ);
            }
        }
        function checkTimerSolve(timerPerQuestion) {
            var timer = parseInt(timerPerQuestion); // ค่าจาก textbox เวลาต่อข้อ
            var questionAmount = parseInt($('#lblQuestionAmount').html()); // จำนวนข้อ           

            if (timer < 10) {
                $('#txtTimeShowAnswer').val(10);
                $('#txtTimeShowAnswer').qtip({
                    content: 'ต้องกรอกเวลาเฉลยควิซข้อละ 10 วินาทีขึ้นไปค่ะ',
                    show: { ready: true },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { target: $('#txtTimeShowAnswer') },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                var timerPerQ = ((10 * questionAmount) / 60).toFixed(2);
            }
        }             
      
    </script>
    <style type="text/css">
        .testDiv
        {
            padding: 5px;
            background-color: White;
            float: left;
            margin: 9px;
            z-index: 0;
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
        }
        .itemSettingQuiz
        {
            position: relative;
            width: 100px;
            height: 100px;
            float: left;
            color: White; /*display: none;*/
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
            margin: 1px;
            background-repeat: no-repeat; /*background-size: 60px;*/
            background-position: center;
            cursor: pointer;
            text-align: center; /*font-weight: bold;
            display: table-cell;
            vertical-align: bottom;*/
        }
        /*.itemSettingQuiz span
        {
            font-size: 14px;
            position: absolute;
            bottom: 0;
            right: 0;
            line-height: 1.2em;
        }*/
        .settingSummary
        {
        }
        .divSettingDetail
        {
            border: 1px solid;
            width: 430px;
            margin-left: 20px;
            background-color: #F4F7FF;
            -webkit-border-radius: 15px;
            border-color: #AFAFAF;
            padding-left: 10px;
            padding-right: 10px;
        }
        img
        {
            position: relative;
            margin-left: 10px;
            margin-bottom: -15px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            
            // ready
            setImageWhenChoose();            

            // Item Click
            $('.itemSettingQuiz').click(function() {
                var parentName = $(this).parent().attr('id');
                if (parentName == 'makeQuizDetailPic') {
                    $('#makeQuiz').toggleClass('settingSummary');
                    swipePanelLR($('#makeQuiz'), $('#settingQuiz'));                       
                }
                else {
                    $('#settingQuiz').toggleClass('settingSummary');
                    swipePanelLR($('#settingQuiz'), $('#makeQuiz'));
                }
            });
                       
            // div ฝั่งขวา
            $('#settingQuiz').click(function (e) {     
                if ($(e.target).is($(this))) {
                    $(this).toggleClass('settingSummary');
                    if ($(this).hasClass('settingSummary')) {
                        swipePanelLR($(this), $('#makeQuiz'));                                              
                    }
                    else {
                        swipePanelSelf($(this), $('#makeQuiz'));                                                                     
                    }
                    // update 29-04-56 Tools  
                    var checked = $('#chkUseTools').attr('checked');
                    resizeWithItemTools(checked);
                }                
            });
            
            // div ฝั่งซ้าย
            $('#makeQuiz').click(function (e) {                   
                var PicSetting = $('#settingQuizDetailPic');
                var itemSetting = $('#settingQuizDetailPic').children('div');       
                var checked = $('#chkUseTools').attr('checked');

                if ($(e.target).is($(this))) {
                    $(this).toggleClass('settingSummary');
                    if ($(this).hasClass('settingSummary')) {
                        swipePanelLR($(this), $('#settingQuiz'));
                        // update 29-04-56 Tools
                        if (checked) {
                            $(PicSetting).css('width','255px');
                            $(itemSetting).removeClass('itemSettingQuiz');  
                            $(itemSetting).addClass('itemSettingQuizWithTools'); 
                        }
                        else {
                            $(PicSetting).css('width','345px');
                            $(itemSetting).removeClass('itemSettingQuizWithTools');
                            $(itemSetting).addClass('itemSettingQuiz');
                            resizeWithItemTools(false);
                        }
                    }
                    else {                                      
                        swipePanelSelf($(this), $('#settingQuiz'));  
                        // update 29-04-56 Tools
                        if (checked) {
                            $(PicSetting).css('width','345px');
                            $(itemSetting).removeClass('itemSettingQuizWithTools');
                            $(itemSetting).addClass('itemSettingQuiz'); 
                        }                                                  
                    }
                }               
            });
            // function swipe open-closed self
            function swipePanelSelf(mySelf, Another) {

                var divWidthSelf = '315px';
                var divWidthAnother = '465px';
                if ($(mySelf).is('#settingQuiz')) {                    
                    divWidthSelf = '465px';
                    divWidthAnother = '315px';
                }

                var listChild = []; 
                $(mySelf).children().each(function() {
                    var id_child = $(this).attr('id');
                    if (id_child != undefined) {
                        listChild.push(id_child);
                    }
                });

                var divItemSelf = $('#' + listChild[0]); 
                var divDetailSelf = $('#' + listChild[1]); 

                $(mySelf).css('height', '270px').css('width', divWidthSelf).css('background-image','url("../images/activity/zoomin.png")');
                $(divDetailSelf).hide();
                $(divItemSelf).show();
                $(Another).css('width', divWidthAnother);
                       
                //imageSummary                            
                setImageWhenChoose();
            }

            // function swipe ซ้ายขวา เมนูทั้ง 2 ข้าง
            function swipePanelLR(mySelf, Another) {
                              
                if ($(Another).hasClass('settingSummary')) {
                    $(Another).toggleClass('settingSummary');                 
                }
                
                var divHeight = '270px';
                if ($(mySelf).is('#settingQuiz')) {                    
                    divHeight = 'auto';
                }

                // My Self
                var listChild = []; 
                $(mySelf).children().each(function() {
                    var id_child = $(this).attr('id');
                    if (id_child != undefined) {
                        listChild.push(id_child);
                    }
                });

                var divItemSelf = $('#' + listChild[0]); 
                var divDetailSelf = $('#' + listChild[1]); 
                
                $(mySelf).css('width', '500px').css('height', divHeight).css('background-image','url("../images/activity/zoomout.png")');
                $(divDetailSelf).show();
                $(divItemSelf).hide(); 

                // Another
                var listChildAnother = []; 
                $(Another).children().each(function() {
                    var id_child = $(this).attr('id');
                    if (id_child != undefined) {
                        listChildAnother.push(id_child);
                    }
                });

                var divItemAnother = $('#' + listChildAnother[0]); 
                var divDetailAnother = $('#' + listChildAnother[1]); 

                $(Another).css('width', '280px').css('height', '270px').css('background-image','url("../images/activity/zoomin.png")');                        
                $(divItemAnother).show();
                $(divDetailAnother).hide()

                //imageSummary
                setImageWhenChoose();
            }

            // checkbox จับเวลา
            $('#<%=chkCheckTime.clientId%>').click(function () {
                ShowHideTimechk();
            });
            // checkbox ไปไม่พร้อมกัน
            $('#<%=chkSelfPace.clientId %>').click(function () {
                if ($(this).attr('checked')) {
                    $('label[for=chkSelfPace]').qtip({
                            content: 'แต่ละคนกดไปข้อถัดไปได้ ไม่ต้องรอกัน',
                            show: { ready: true },
                            style: {
                                width: 150, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                            },
                            position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle'} },
                            hide: false
                        });
                        var destroyQtip = setTimeout(function () {
                            $('label[for=chkSelfPace]').qtip('destroy');
                        }, 4000);
                }
                else {
                    //$('label[for=chkSelfPace]').qtip('destroy');
                }
                ShowHideSelfPace();
            });
            // checkbox เฉลย
            $('#<%=chkShowAnswer.clientId %>').click(function () {
                ShowHideShowAnswer();
            });
            // checkbox คะแนน
            $('#<%=chkShowScore.clientId %>').click(function () {
                ShowHideShowScore();
            });
            // checkbox สลับข้อสอบ-คำตอบ
            $('.diffQuestion').click(function () {
                ShowHideShowDiffQuestion();
            });
            // checkbox tablet
            var chk = $('#chkQuizUseTablet').attr('checked');
            if(!chk){
                $('#divUseTemplate').show();
            }

            $('#chkQuizUseTablet').change(function () {
                // clear checkbox tabletLab
                var img = $('#imgTablet');
                if (!($(this).attr('checked'))) {
                    $('#chkQuizFromSoundlab').attr('checked',false).attr('disabled',true);  
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');                                         

                      //ซ่อน Setting แสดงคะแนนแท็บเล็ตเด็ก
                    $('#chkShowScore').hide();
                    $('#chkShowScore').prev().hide();
                    $('#chkShowScore').next().hide();
                    $('#imgShowScore').hide();
                    $('#imgShowScore').next().hide();
                    $('#DivChkShowScore').hide();
                    $('#chkShowScore').attr('checked',false);
                }
                else {
                    $('#chkQuizFromSoundlab').attr('disabled',false);
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png'); 

                    //แสดง Setting แสดงคะแนนแท็บเล็ตเด็ก
                    $('#chkShowScore').show();
                    $('#chkShowScore').prev().show();
                    $('#chkShowScore').next().show();
                    $('#imgShowScore').show();
                    $('#imgShowScore').next().show(); 
                }
                // set session UseTablet
                $.ajax({ type: "POST",
                	url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/checkQuizUseTablet",
                    async: false, // ทำงานให้เสร็จก่อน
                	data: "{checked : '" + $(this).attr('checked') + "' }",  //" 
                	contentType: "application/json; charset=utf-8", dataType: "json",   
                	success: function (data) {
                        var chk = data.d;
                	    if(chk == true){
                            $('#divUseTemplate').hide();                            
                            $('#divUseTemplate').qtip('destroy');
                            $('#chkUseTemplate').attr('checked',false);
                        }
                        else if(chk == false){
                            $('#divUseTemplate').show();
                            if($('#chkUseTemplate').attr('disabled')){
                                var content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ เพราะจำนวนข้อสอบเกิน 120 ข้อ';
                                if($('#divUseTemplate').hasClass('TypeError')){                                    
                                    content = 'ใช้กระดาษคำตอบไม่ได้ค่ะ ข้อสอบต้องเป็นแบบตัวเลือกไม่เกิน 5 ช้อยส์หรือถูกผิดเท่านั้น';
                                }
                                $('#divUseTemplate').live().qtip({
                                    content: content.toString(),
                                    show: { ready: true },
                                    style: {
                                        width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                                    },                                   
                                     position: {corner:{tooltip:'leftMiddle',target: 'rightMiddle' }},
                                     hide:false
                                });      
                            }                      
                        }                                     
                	},
                	error: function myfunction(request, status)  {                                      
                	}
                });
            });  
            // checkbox checkmark
            $('#chkUseTemplate').change(function () {
                $.ajax({ type: "POST",
                	url: "<%=ResolveUrl("~")%>Activity/SettingActivity.aspx/checkQuizUseTamplate",
                    async: false, // ทำงานให้เสร็จก่อน
                	data: "{checked : '" + $(this).attr('checked') + "' }",  //" 
                	contentType: "application/json; charset=utf-8", dataType: "json",   
                	success: function (data) {
                        var chk = data.d;                	                                     
                	},
                	error: function myfunction(request, status)  {  
                                    
                	}
                });
            });         
        });
        // แสดงจับเวลา //
        function ShowHideTimechk() {
            var item = $('#itemTime');
            var divDetail = $('#DivCheckTime');
            var img = $('#imgShowTime');
            if ($('#<%=chkCheckTime.clientId%>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();
                if ($('#IsPerQuestion').attr('checked')) {
                    //$(item).html('จับเวลาข้อต่อข้อ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Watch1-1.png")');
                    setImageInItem(item,'จับเวลาข้อต่อข้อ','Settings-btnQuizMode-Watch1-1.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch1-1_small.png');
                } 
                else {
                    //$(item).html('จับเวลาทั้งหมด').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Watch-1-N.png")');
                    setImageInItem(item,'จับเวลาทั้งหมด','Settings-btnQuizMode-Watch-1-N.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch-1-N_small.png');
                }
            }
            else {
                $(divDetail).stop(true, true).hide();               
                //$(item).html('ไม่จับเวลา').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-NoWatch.png")');
                setImageInItem(item,'ไม่จับเวลา','Settings-btnQuizMode-NoWatch.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoWatch_small.png');
            }
        }
        // ไปไม่พร้อมกัน //
        function ShowHideSelfPace() {
            var item = $('#itemSelfPace');
            var img = $('#imgSelfPace');
            if ($('#<%=chkSelfPace.clientId %>').attr('checked') == 'checked') {              
                //$(item).html('ไปไม่พร้อมกัน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-SelfPace.png")');
                setImageInItem(item,'ไปไม่พร้อมกัน','Settings-btnQuizMode-SelfPace.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-SelfPace_small.png');
            }
            else {
                //$(item).html('ไปพร้อมกัน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-SamePace.png")');
                setImageInItem(item,'ไปพร้อมกัน','Settings-btnQuizMode-SamePace.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-SamePace_small.png');
            }
        }
        // แสดงเฉลย //
        function ShowHideShowAnswer() {
            var item = $('#itemSolve');
            var divDetail = $('#DivChkShowAnswer');
            var chkShowAnswer = $('#<%=chkShowAnswer.clientId %>');
            var img = $('#imgShowAnswer');
            if ($('#<%=chkShowAnswer.clientId %>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();

                if($('#rdbAnswerAfter').attr('checked') == 'checked') {
                    $('#ChkInShow1').attr('disabled',true);
                    $('#ChkInShow1').attr('checked',false);
                    $('#txtTimeShowAnswer').attr('disabled',true);
                    //$(item).html('แสดงเฉลยเมื่อจบควิซ').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete.png")');
                    setImageInItem(item,'แสดงเฉลยเมื่อจบควิซ','Settings-btnQuizMode-AnsAfterComplete.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete_small.png');
                }       
                else {                    
                    if ($('#ChkInShow1').attr('checked')) {
                        //$(item).html('แสดงเฉลยทีละข้อมีเวลา').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-WithAnsTimer.png")');
                        setImageInItem(item,'แสดงเฉลยทีละข้อมีเวลา','Settings-btnQuizMode-WithAnsTimer.png');
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAnsTimer_small.png');
                    }
                    else {
                        //$(item).html('แสดงเฉลยทีละข้อ').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-WithAns.png")');
                        setImageInItem(item,'แสดงเฉลยทีละข้อ','Settings-btnQuizMode-WithAns.png');
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                    }
                }            
            }
            else {
                $(divDetail).stop(true, true).hide();
                //$(item).html('ไม่แสดงเฉลย').css('background-image', 'url("../images/activity/setting/Settings-btnQuizMode-NoAnswer.png")');
                setImageInItem(item,'ไม่แสดงเฉลย','Settings-btnQuizMode-NoAnswer.png');               
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoAnswer_small.png');
            }
        }
        // แสดงคะแนน //
        function ShowHideShowScore() {
            var item = $('#itemScore');
            var divDetail = $('#DivChkShowScore');
            var imgShowScore = $('#imgShowScore');
            if ($('#<%=chkShowScore.clientId %>').attr('checked') == 'checked') {
                $(divDetail).stop(true, true).show();
                if ($('#rdbByStep').attr('checked')) {
                    //$(item).html('แสดงคะแนนทีละข้อ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion.png")');
                    setImageInItem(item,'แสดงคะแนนทีละข้อ','Settings-btnQuizMode-ScoreEachQuestion.png');
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion_small.png');
                }
                else {
                    //$(item).html('แสดงคะแนนตอนจบควิซ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete.png")');
                    setImageInItem(item,'แสดงคะแนนตอนจบควิซ','Settings-btnQuizMode-ScoreWhenComplete.png');
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete_small.png');
                }                
            }
            else {
                $(divDetail).stop(true, true).hide();
                //$(item).html('ไม่แสดงคะแนน').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-ScoreNoShow.png")');
                setImageInItem(item,'ไม่แสดงคะแนน','Settings-btnQuizMode-ScoreNoShow.png');
                $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreNoShow_small.png');
            }
        }
        // soundlab
        function ShowHideSoundlab(useTablet){
            if(useTablet){
                $('#DivFromSoundlab').stop(true, true).show();
                $('#itemQuizMode').show();                
            }
            else{
                $('#DivFromSoundlab').stop(true, true).hide();
                $('#itemQuizMode').hide();
            }
        }
        // สลับคำถาม-คำตอบ //
        function ShowHideShowDiffQuestion() {   
            var isDiffQuestion;  
            var item = $('#itemDiffQuestion');  
            var divDetail = $('#DivDiffQuestion');    
            var img = $('#imgQA');
            $('.diffQuestion').each(function () {                
                var val = $(this).attr('val');                
                if ($('[id*=' + val + ']').attr('checked') == 'checked') {
                    isDiffQuestion = true;
                }
            });            
            if (isDiffQuestion) {
                var Question = $('[id*=chkRandomQuestion]').attr('checked');
                var Answer = $('[id*=chkRandomAnswer]').attr('checked');
                var Q_A = $('[id*=chkDiffQuestion]').attr('checked');                
                var textHtml;var ImageFile;

                $(divDetail).stop(true, true).show();

                if ((Question) && (Answer)) {
                    textHtml = 'สลับข้อ-คำตอบ';
                    ImageFile = 'Settings-btnQuizMode-RandomAll.png';
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll_small.png');                   
                    if ((Q_A)) {
                        //textHtml = 'สลับข้อ-คำตอบ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomAll-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll-ScreenDiff_small.png');
                    }                    
                }
                else if ((Question) && !(Answer)) {
                    textHtml = 'สลับข้อ';
                    ImageFile = 'Settings-btnQuizMode-RandomQuest.png'; 
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest_small.png');                 
                    if ((Q_A)) {
                        //textHtml = 'สลับข้อ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomQuest-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest-ScreenDiff_small.png'); 
                    }
                }
                else if (!(Question) && (Answer)) {
                    textHtml = 'สลับคำตอบ';    
                    ImageFile = 'Settings-btnQuizMode-RandomChoice.png';
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice_small.png');              
                    if ((Q_A)) {
                        //textHtml = 'สลับคำตอบ แต่ละจอแสดงต่างกัน';
                        ImageFile = 'Settings-btnQuizMode-RandomChoice-ScreenDiff.png';
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice-ScreenDiff_small.png'); 
                    }
                }           
                
                $('label[for=chkDiffQuestion]').text(textHtml + '  ให้นักเรียนแต่ละคนไม่เหมือนกัน');                  
                //$(item).html(textHtml).css('background-image','url("../images/File-Open-icon.png")');
                setImageInItem(item,textHtml, ImageFile);                
            }
            else {                
                $(divDetail).stop(true, true).hide();
                $('[id*=chkDiffQuestion]').attr('checked', false);
                //$(item).html('เหมือนกันทุกคน').css('background-image','url("../images/download.png")');
                setImageInItem(item,'เหมือนกันทุกคน','Settings-btnQuizMode-NoRandom.png');
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-NoRandom_small.png');
            }
        }
        // แสดงโหมดควิซ //
        function ShowHideModeQuiz() {
            var item = $('#itemQuizMode');
            var chkTabletLab = $('#chkQuizFromSoundlab');
            var img = $('#imgTablet');
            if($('#chkUseTemplate').attr('checked') == 'checked'){
                //$(item).html('ควิซกระดาษคำตอบ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Lab.png")');
                setImageInItem(item,'ควิซกระดาษคำตอบ','Settings-btnQuizMode-CheckMark.png'); 
                $(img).attr('src', '');                
            }
            else if($('#chkQuizUseTablet').attr('checked') == 'checked'){
                if($(chkTabletLab).attr('checked') == 'checked'){
                    //$(item).html('ควิซแล็บแท็บเล็ต').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Lab.png")');
                    setImageInItem(item,'ควิซแล็บแท็บเล็ต','Settings-btnQuizMode-Lab.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Lab_small.png');
                }
                else{
                    //$(item).html('ควิซแท็บเล็ต').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-Tablet.png")');
                    setImageInItem(item,'ควิซแท็บเล็ต','Settings-btnQuizMode-Tablet.png');
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png');
                }                            
            }                        
            else{
                $(chkTabletLab).attr('disabled',true);
                //$(item).show().html('ควิซปกติ').css('background-image','url("../images/activity/setting/Settings-btnQuizMode-QuizOnly.png")');    
                setImageInItem(item,'ควิซปกติ','Settings-btnQuizMode-QuizOnly.png');   
                $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-QuizOnly_small.png');         
            }
        }     
        // function htmlItem 
        function setImageInItem(item,textHtml,imageFile) {            
            $(item).html('').css('background-image', 'url("../images/activity/setting/' + imageFile + '")').prop('title',textHtml);            
        }  
        // function set image label
        function setImageInLabel() {
        }
        // แสดงรูป
        function setImageWhenChoose() {
            // mode left side
            ShowHideModeQuiz(); 
            ShowHideShowDiffQuestion();
            // mode right side  
            ShowHideTimechk();
            ShowHideShowAnswer();
            ShowHideShowScore();            
            ShowHideSelfPace(); 
            // update 29-04-56 Tools
            var checked = $('#chkUseTools').attr('checked');
            resizeWithItemTools(checked);   
            ShowHideUseTools(checked);                 
        }
        // update 29-04-56 Tools
        function resizeWithItemTools(checked) {        
            var PicSetting = $('#settingQuizDetailPic');
            var itemSetting = $('#settingQuizDetailPic').children('div');

            if (checked) {
                $(PicSetting).css('width','345px'); 
                $('#itemTools').show();
                $('#itemSolve').css('margin-top','0px').css('margin-left','20px');
                $('#itemScore').css('margin-left','60px');
                $(itemSetting).removeClass('itemSettingQuizWithTools');
                $(itemSetting).addClass('itemSettingQuiz');                
            }       
            else {
                $('#itemTools').hide();
                $(PicSetting).css('width','225px');
                $('#itemSolve').css('margin-top','20px').css('margin-left','0px');
                $('#itemScore').css('margin-left','20px');
                $(itemSetting).removeClass('itemSettingQuizWithTools');
                $(itemSetting).addClass('itemSettingQuiz');
            }          
        }          
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            // img 
            var imgShowTime = $('#imgShowTime');
            var imgShowAnswer = $('#imgShowAnswer');
            var imgShowScore = $('#imgShowScore');
            // radio event
            $('input[type=radio]').click(function () {
                setImageCheckboxOnIE();
                // จับเวลาข้อต่อข้อ
                if ($(this).is('#IsPerQuestion')) {
                    $(imgShowTime).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch1-1_small.png');
                }
                // จับเวลาทั้งหมด
                else if ($(this).is('#IsAll')) {
                    $(imgShowTime).attr('src', '../images/activity/setting/Settings-btnQuizMode-Watch-1-N_small.png');
                }
                // เฉลยเมื่อจบควิซ
                else if ($(this).is('#rdbAnswerAfter')) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-AnsAfterComplete_small.png');
                }
                // เฉลยข้อต่อข้อ
                else if ($(this).is('#rdbAnswerPerQuestion')) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                }
                // คะแนนข้อต่อข้อ
                else if ($(this).is('#rdbByStep')) {
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreEachQuestion_small.png');
                }
                // คะแนนเมื่อจบควิซ
                else if ($(this).is('#rdbEndQuiz')) {
                    $(imgShowScore).attr('src', '../images/activity/setting/Settings-btnQuizMode-ScoreWhenComplete_small.png');
                }
            });
            // checkbox เวลาเฉลย
            $('#ChkInShow1').click(function () {
                var checked = $(this).attr('checked');
                if (checked) {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAnsTimer_small.png');
                }
                else {
                    $(imgShowAnswer).attr('src', '../images/activity/setting/Settings-btnQuizMode-WithAns_small.png');
                }
            });
            // checkbox lab tablet
            $('#chkQuizFromSoundlab').click(function () {
                var checked = $(this).attr('checked');
                var img = $('#imgTablet');
                if (checked) {
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Lab_small.png');
                }
                else {
                    $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-Tablet_small.png');
                }
            });
            // checkbox diff question & answer
            $('#chkDiffQuestion').click(function () {
                var Question = $('[id*=chkRandomQuestion]').attr('checked');
                var Answer = $('[id*=chkRandomAnswer]').attr('checked');
                var checked = $(this).attr('checked');
                var img = $('#imgQA');

                if ((Question) && (Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomAll_small.png');
                    }
                }
                else if ((Question) && !(Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomQuest_small.png');
                    }
                }
                else if (!(Question) && (Answer)) {
                    if (checked) {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice-ScreenDiff_small.png');
                    }
                    else {
                        $(img).attr('src', '../images/activity/setting/Settings-btnQuizMode-RandomChoice_small.png');
                    }
                }

            });

            // update 29-04-56 add Tools
            // checkbox use tools #1
            $('#chkUseTools').click(function () {
                var checked = $(this).attr('checked');
                ShowHideUseTools(checked);
            });
        });
        // update 29-04-56 add Tools checkbox     
        // #2
        function ShowHideUseTools(checked) {
            var divDetail = $('#DivChkUseTools');
            if (checked) {
                $(divDetail).show();
            }
            else {
                $(divDetail).hide();
                $('#tblTools').children().find('input[type=checkbox]').each(function () {
                    $(this).attr('checked', false);
                });
            }
        }
    </script>
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');

        $(document).ready(function () {
            if ($.browser.msie) {
                // ready!!!!!!!
                setImageCheckboxOnIE();
                //checkbox tick
                $('input[type=checkbox]').click(function () {
                    setImageCheckboxOnIE();
                });
            }
        });
        function setImageCheckboxOnIE() {
            $('input[type=checkbox]').each(function () {
                var chk = $(this).attr('checked');
                if (chk) {
                    var chkT = { 'background-image': 'url(../images/bullet_checked.gif)' };
                    $(this).next().css(chkT);
                }
                else {
                    var chkF = { 'background': 'url(../images/bullet.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px', 'color': '' };
                    if ($(this).attr('disabled')) {
                        var chkDis = { 'background': ' url(../images/bullet-disable.gif) center left no-repeat', 'color': 'Gray' };
                        $(this).next().css(chkDis);
                    } else {
                        $(this).next().css(chkF);
                    }

                }
            });
            //            $('input[type=checkbox]:disabled').each(function () {
            //                var chkDis = { 'background': ' url(../images/bullet-disable.gif)' }; // 'color': 'Gray', 
            //                chkDis = { 'color': 'red' };                
            //                $(this).next().css(chkDis);
            //            });         
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#IsPerQuestion').change(function () {
                var chk = $(this).attr('checked');
                if (chk) {
                    //qtip
                    $('#lblTimeAllPerQuestion').next('span').qtip({
                        content: 'โหมดนี้ นักเรียนจะย้อนกลับไปข้อก่อนหน้าไม่ได้',
                        show: { ready: true },
                        style: {
                            width: 180, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                        },
                        position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle'} },
                        hide: false
                    });
                    var destroyQtip = setTimeout(function () {
                        $('#lblTimeAllPerQuestion').next('span').qtip('destroy');
                    }, 4000);
                }
            });

            // update 30-04-56 label for checkbox
            $('#tblTools').children().find('label').css('color', 'transparent');
        });
    </script>
    <style type="text/css">
        #btnChageLV
        {
            position: relative;
            width: 35px;
            height: 35px;
            font-weight: bolder;
            top: -5px;
            cursor: pointer;
            background: #1EC9F4;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            text-shadow: 1px 1px #178497;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        /* update 29-04-56 Add Tools */
        .imgTools
        {
            width: 40px;
            height: 40px;
            margin-left: 0px;
        }
        #tblTools tr td
        {
            background: none;
            border-bottom: none;
            padding: 0px;
        }
        .itemSettingQuizWithTools
        {
            position: relative;
            width: 70px;
            height: 70px;
            float: left;
            color: White;
            -webkit-border-radius: .5em;
            -moz-border-radius: .5em;
            border-radius: .5em;
            behavior: url(../css/PIE.htc);
            margin: 1px;
            background-repeat: no-repeat;
            background-position: center;
            cursor: pointer;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="SetupAnswer_Name" runat="server" />
    <asp:HiddenField ID="QuestionAmount" runat="server" />
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <center>
                    <h2>
                        ชื่อชุดควิซ
                        <asp:Label ID="lblTestsetName" runat="server" Text="lblTestsetName"></asp:Label></h2>
                    <div id="div-1" style="color: #47433F">
                        <%--<div id='AllSetting' class="divAllSetting">--%>
                        <center style="font-size: 25px;">
                            <span>ชั้น </span>
                            <asp:Label ID="lblLevel" runat="server" Text="lblLevel"></asp:Label>&nbsp;
                            <input type="button" id='btnChageLV' value='...' /><%--onclick='ChangeLevel()'--%>
                            / <span>ห้อง </span>
                            <asp:TextBox ID="txtRoom" Style='width: 40px' runat="server" Font-Size="25px"></asp:TextBox>&nbsp;&nbsp;
                            <span id="txtTotalStudent"></span>
                            <%--<span>นักเรียนทั้งหมด </span>
                            <asp:TextBox ID="txtTotalStudent" Style='width: 40px' runat="server" Font-Size="25px"></asp:TextBox><span>
                                คน </span>--%>
                        </center>
                        <div id="makeQuiz" class="testDiv" style="width: 315px; height: 270px; background-image: url('../images/activity/zoomin.png');
                            background-repeat: no-repeat; background-position-x: right; padding-top: 10px;
                            background-position: right top; cursor: pointer; padding-bottom: 20px;">
                            <div style="margin-left: auto; margin-right: auto; width: 220px;">
                                <center>
                                    <b><span>จากข้อสอบ จำนวน&nbsp;</span><asp:Label ID="lblQuestionAmount" runat="server"
                                        Text="QuestionAmount" Font-Underline="True"></asp:Label><span>&nbsp;ข้อ</span></b></center>
                            </div>
                            <div id="makeQuizDetailPic" style="margin-left: auto; margin-right: auto; width: 100px;
                                height: 200px; margin-top: 10px;">
                                <div id="itemQuizMode" class="itemSettingQuiz" style="background-color: #008299;">
                                    <span>ควิซแล็บแท็บเล็ต</span>'
                                </div>
                                <div id="itemDiffQuestion" class="itemSettingQuiz" style="background-color: #2E8DEF;
                                    margin-top: 20px;">
                                </div>
                            </div>
                            <div id="makeQuizDetail" style="margin-left: 20px; display: none;">
                                <%If HttpContext.Current.Application("NeedCheckmark") = True Then%>
                                <asp:CheckBox ID="chkUseTemplate" runat="server" Text="ให้เด็กทำด้วยกระดาษคำตอบคอมพิวเตอร์" /><img
                                    id="imgCheckmark" src="" /><br />
                                <%End If%>
                                <asp:CheckBox ID="chkQuizUseTablet" runat="server" Text="ทำควิซโดยใช้แท็บเล็ต ?"
                                    Checked="true" /><img id="imgTablet" src="" /><br />
                                <div id='DivFromSoundlab' style='width: 400px; border: 1px solid; margin-left: 20px;
                                    background-color: #F4F7FF; -webkit-border-radius: 15px; border-color: #AFAFAF;
                                    padding: 10px; margin-bottom: 10px;'>
                                    <asp:CheckBox ID="chkQuizFromSoundlab" runat="server" Text="ใช้กับห้องปฏิบัติการ (แท็บเล็ตแล็บ)" />
                                </div>
                                <asp:CheckBox ID="chkRandomQuestion" runat="server" Text="สลับข้อให้ใหม่" class="diffQuestion"
                                    val="chkRandomQuestion" Style="margin-top: 10px;" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkRandomAnswer" runat="server" Text="สลับคำตอบให้ใหม่" class="diffQuestion"
                                    val="chkRandomAnswer" /><img id="imgQA" src="" />
                                <div id='DivDiffQuestion' style='width: 400px; border: 1px solid; margin-left: 20px;
                                    background-color: #F4F7FF; -webkit-border-radius: 15px; border-color: #AFAFAF;
                                    padding: 10px;'>
                                    <asp:CheckBox ID="chkDiffQuestion" runat="server" Text="aas" val="chkDiffQuestion" />
                                </div>
                            </div>
                        </div>
                        <div id="settingQuiz" class="testDiv" style="width: 465px; height: 270px; background-image: url('../images/activity/zoomin.png');
                            background-repeat: no-repeat; background-position-x: right; padding-top: 10px;
                            background-position: right top; cursor: pointer; padding-bottom: 20px;">
                            <div style="margin-left: auto; margin-right: auto; width: 210px;">
                                <center>
                                    <b><span>ตั้งค่าควิซ</span></b></center>
                            </div>
                            <div id="settingQuizDetailPic" style="margin-top: 10px; margin-left: auto; margin-right: auto;
                                width: 225px; height: 270px;">
                                <div id="itemTime" class="itemSettingQuiz" style="background-color: #008299;">
                                </div>
                                <div id="itemSelfPace" class="itemSettingQuiz" style="background-color: #2E8DEF;
                                    margin-left: 20px;">
                                </div>
                                <div id="itemSolve" class="itemSettingQuiz" style="background-color: #DC572E; margin-top: 20px;">
                                </div>
                                <div id="itemScore" class="itemSettingQuiz" style="background-color: #00A600; margin-top: 20px;
                                    margin-left: 20px;">
                                </div>
                                <div id="itemTools" class="itemSettingQuiz" style="background-color: #00AA00; margin-top: 20px;
                                    margin-left: 20px; display: none; background-image: url('../Images/Activity/Setting_Tools/Tools_Icon.jpg');
                                    background-size: 60px;">
                                </div>
                            </div>
                            <div id="settingQuizDetail" style="display: none; margin-left: 20px;">
                                <asp:CheckBox ID="chkCheckTime" runat="server" Text="จับเวลา" class="chkSettingQuiz"
                                    val="chkCheckTime" /><img id="imgShowTime" /><br />
                                <div id='DivCheckTime' class="divSettingDetail">
                                    <input type="radio" id='IsPerQuestion' name='ChkTime' runat="server" /><label for='IsPerQuestion'>
                                        ข้อละ &nbsp;&nbsp;&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtTimePerQuestion" Style="width: 30px" runat="server" CssClass="txtTime">60</asp:TextBox><span>
                                        &nbsp;วินาที </span>&nbsp;&nbsp;&nbsp;(รวมทั้งหมด
                                    <asp:Label ID="lblTimeAllPerQuestion" runat="server" Text="lblTAPQ"></asp:Label>&nbsp;นาที)<span>&nbsp;</span>
                                    <br />
                                    <input type='radio' id='IsAll' name='ChkTime' runat="server" /><label for='IsAll'>
                                        ทั้งหมด &nbsp;
                                    </label>
                                    <asp:TextBox ID="txtTimeAll" Style="width: 30px" runat="server" CssClass="txtTime">30</asp:TextBox>
                                    &nbsp;นาที &nbsp;&nbsp;&nbsp;&nbsp;(เฉลี่ยข้อละ
                                    <asp:Label ID="lblTimeAll" runat="server" Text="lblTA"></asp:Label>&nbsp;วินาที)
                                </div>
                                <asp:CheckBox ID="chkSelfPace" runat="server" Text="ทำควิซไม่พร้อมกัน" class="chkSettingQuiz"
                                    val="chkSelfPace" /><img id="imgSelfPace" /><br />
                                <asp:CheckBox ID="chkShowAnswer" runat="server" Text="แสดงเฉลย" class="chkSettingQuiz"
                                    val="chkShowAnswer" /><img id="imgShowAnswer" src="" /><br />
                                <div id='DivChkShowAnswer' class="divSettingDetail">
                                    <input type="radio" name='rShowAnswer' runat="server" id='rdbAnswerPerQuestion' class='radioShowAnswer' /><label
                                        for='Show1'>
                                        แสดงทีละข้อ
                                    </label>
                                    <div id='ForShow1' style='padding-left: 30px'>
                                        <asp:CheckBox ID="chkRushMode" Style='display: none;' runat="server" Text="แข่งกันขอตอบคำถาม" />
                                        <asp:CheckBox ID="ChkInShow1" runat="server" Text="แสดงข้อละ" />&nbsp;&nbsp;<asp:TextBox
                                            ID="txtTimeShowAnswer" Style='width: 20px' runat="server" CssClass="txtTime">30</asp:TextBox>
                                        &nbsp;&nbsp; วินาที
                                    </div>
                                    <input type="radio" id='rdbAnswerAfter' runat="server" name='rShowAnswer' class='radioShowAnswer' /><label
                                        for='Show2'>
                                        แสดงเมื่อทำครบทุกข้อ</label><br />
                                </div>
                                <asp:CheckBox ID="chkShowScore" runat="server" Text="แสดงคะแนน" class="chkSettingQuiz"
                                    val="chkShowScore" /><img id="imgShowScore" src="" /><br />
                                <div id="DivChkShowScore" class="divSettingDetail" style="display: none;">
                                    <input type="radio" id="rdbByStep" runat="server" class="radioShowScore" name="rShowScore" /><label
                                        for="Show3">
                                        แสดงทีละข้อ</label><br />
                                    <input type="radio" id="rdbEndQuiz" runat="server" class="radioShowScore" name="rShowScore" /><label
                                        for="Show4">
                                        แสดงเมื่อจบควิซ</label>
                                </div>
                                <%-- update 29-04-56 Add Tools --%>
                                <asp:CheckBox ID="chkUseTools" runat="server" Text="เครื่องมือช่วยตอนทำควิซ" class="chkSettingQuiz"
                                    val="chkUseTools" />
                                <div id="DivChkUseTools" class="divSettingDetail" style="display: none;">
                                    <table id="tblTools" style="width: 0px">
                                        <tr>
                                            <% If toolsSubject_Math = True Then%>
                                            <td>
                                                <asp:CheckBox ID="chkWithCalculator" runat="server" class="chkSettingQuiz" Text="."
                                                    val="chkWithCalculator" /><img src="../Images/Activity/Setting_Tools/Calculator_Icon.png"
                                                        class="imgTools" style="margin-right: 20px" />
                                            </td>
                                            <% End If%><% If toolsSubject_Eng = True Then%>
                                            <td>
                                                <asp:CheckBox ID="chkWithDictionary" runat="server" class="chkSettingQuiz" Text="." /><img
                                                    src="../Images/Activity/Setting_Tools/Dictionary_Icon.png" class="imgTools" style="margin-right: 20px" />
                                            </td>
                                            <% End If%><% If toolsSubject_Eng = True Then%>
                                            <td>
                                                <asp:CheckBox ID="chkWithWordBook" runat="server" class="chkSettingQuiz" Text="." /><img
                                                    src="../Images/Activity/Setting_Tools/WordBook_Icon.jpg" class="imgTools" style="margin-right: 20px" />
                                            </td>
                                            <% End If%><% If toolsAllSubject = True Then%>
                                            <td>
                                                <asp:CheckBox ID="chkWithNotes" runat="server" class="chkSettingQuiz" Text="." /><img
                                                    src="../Images/Activity/Setting_Tools/Notes_Icon.png" class="imgTools" style="margin-right: 20px" />
                                            </td>
                                            <% End If%><% If toolsSubject_Math = True Then%>
                                            <td>
                                                <asp:CheckBox ID="chkWithProtractor" runat="server" class="chkSettingQuiz" Text="." /><img
                                                    src="../Images/Activity/Setting_Tools/Protractor_Icon.jpg" class="imgTools" />
                                            </td>
                                            <% End If%>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div class="form_settings">
                        <asp:Button ID="BtnBack" runat="server" Text="กลับ" class="submit" Style="margin: 0 0 0 -283px;
                            width: 200px; position: relative;" />
                        <asp:Button ID="btnOK" runat="server" Text="เริ่มกันเลย" class="submit" Style="float: Right;
                            right: 20px; width: 200px; position: relative;" />
                        <%--Style="float: right; height: 50px;width: 80px;"--%>
                    </div>
                    <%--</div>--%>
                </center>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenTest" runat="server" />
    </form>
    <div id='DialogLV' title='เลือกชั้นเรียนที่ต้องการ' style="font-size: large;">
        <table style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
            <tr style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
                <td id="PClass" runat="server" style="background: inherit; color: inherit; border-bottom: inherit;
                    padding: inherit;">
                </td>
            </tr>
            <tr style="background: inherit; color: inherit; border-bottom: inherit; padding: inherit;">
                <td id="MClass" runat="server" style="background: inherit; color: inherit; border-bottom: inherit;
                    padding: inherit;">
                </td>
            </tr>
        </table>
    </div>
</body>
<script type="text/javascript">
        //script ดักอีกที เมื่อโหลดเพจเสร็จ
        var CurrentPage;
        $(function () {         
            var thisPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx';          
            thisPage = thisPage.toLowerCase();           
            getCurrentPage();
            if (CurrentPage != thisPage) {
                withOutClick = false;
                window.location = CurrentPage;
            }           
        });
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
</html>
