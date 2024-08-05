<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="SelectEachQuestion.aspx.vb" Inherits="QuickTest.SelectEachQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <%If IsAndroid = True Then%>
    <style type="text/css">
        #site_content {
            width: 950px !important;
        }

        #div1 {
            width: 810px !important;
        }

        #footer-back-to-top, #HelpSelect {
            display: none !important;
        }

        table tr td, #thHeader, #lblforchkrandom, #thTotalChoose, #tdRandom {
            font-size: 25px !important;
        }

        body {
            background: #2AA0B3 !important;
        }

        #lblforchkAll {
            font-size: 20px !important;
        }

        #DivFaceScroll {
            height: 450px !important;
        }

        #DivchkRandomFixAmount, #DivchkRandomPercent {
            width: 465px !important;
        }
    </style>
    <%End If%>

    <style type="text/css">
        #btnLogout, #btnContact {
            display: none;
        }

        #dialogRandomQuestion #btnRandomQuestion {
            font: 20px 'THSarabunNew';
            line-height: 2em;
            border: 0;
            height: 40px;
            line-height: 2em;
            width: 100px;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            text-shadow: 1px 1px #178497;
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('PIE.htc');
            -pie-track-active: false;
            background: #63cfdf;
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJod…EiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%);
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9));
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%);
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 );
        }
    </style>

    <script type="text/javascript" src="../js/jquery-1.7.1.js"></script>
    <script src="../js/jutil.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        $(function () {                
            var getLengthChk = document.getElementsByName("test[]").length;
            var getEleChk = document.getElementsByName("test[]");  

            var t = '<%=ToTalQuestionInQset%>';
            $('#spnTotalQuestionInQset').text(t);
            $('#spnTotalQuestionInQset2').text(t);
            var p = ((1 / t) * 100);
            $('#spnTotalPercent').text('(ระบุตัวเลข 1 - 100 เท่านั้น)');
            if ($.browser.msie) {          
                if ($.browser.version >= 8) {
                    $('table').width(0);
                    $('.ForTestJa').css('width','100%');
                    $('#Toptbl').css('width','100%');
                    $('.ForIe8').css('width','100%');
                    $('#QuestionAnswer').css('width','100%');
                    $('#TestForie8').css('width','80%');
                }
                else {
                    $('table').width(0);
                    $('#Toptbl').css('width','100%');
                    $('.ForTestJa').css('width','100%');
                    $('#QuestionAnswer').css('width','100%');
                }
            }     

            // event checkbox เลือกทั้งหมด
            $("#chkAll").click(function() {         
                if ($("#chkAll").attr('checked') == 'checked') {  
                    for (var i=0;i<getLengthChk;i++) {
                        if ($(getEleChk.item(i)).attr("checked") != 'checked') {
                            $(getEleChk.item(i)).attr("checked", true);
                            $(getEleChk.item(i)).triggerHandler('click');
                        }
                    }
                } else {
                    for (var i=0;i<getLengthChk;i++) {
                        if ($(getEleChk.item(i)).attr("checked") == 'checked') {
                            $(getEleChk.item(i)).attr("checked", false);
                            $(getEleChk.item(i)).triggerHandler('click');
                        }
                    }
                }
                SumQuestionSelected();               
            });
        });

        function SumQuestionSelected() {           
            var sum = $(":checkbox[class^=chkQuestion]:checked").length;
            $("#spnTotalQuestionsSelected").html(sum);
            $("#spnTotalQuestionsSelected1").html(sum);            
            $('#txtFixAmount').val('');
            $('#txtPercent').val('');            
        } 
        
        function toggleOtherChkbox(selectedChk) {
            if (selectedChk == "FixAmount") {
                if (document.getElementById('chkRandomFixAmount').checked) {
                    document.getElementById('chkRandomPercent').checked = false;
                }
            }
            else {
                if (document.getElementById('chkRandomPercent').checked) {
                    document.getElementById('chkRandomFixAmount').checked = false;
                }
            }
        };

        function SaveLog(Type,QuestionId){
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>testset/SelectEachQuestion.aspx/SaveLog",
                async: false, // ทำงานให้เสร็จก่อน
                data: "{ Type : '" + Type + "', QuestionId : '" + QuestionId + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",   
                success: function (msg) { 
                },
                error: function myfunction(request, status)  {
                }
            });
            };

            function onSave(questionId, chkBox, qSetId, testSetId, userId, classId) {

                var valReturnFromCodeBehide;

                $.ajax({ type: "POST",
                    url: "<%=ResolveUrl("~")%>testset/SelectEachQuestion.aspx/OnSaveCodeBehide",
                    async: false, // ทำงานให้เสร็จก่อน
                    data: "{ questionId : '" + questionId + "', needRemove : '" + !(chkBox.checked) + "', qSetId : '" + qSetId + "', testSetId : '" + testSetId + "', userId : '" + userId + "', classId : '" + classId + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",   
                    success: function (msg) {
                        if (msg.d != 0) {
                            valReturnFromCodeBehide = msg.d;   
                        
                            if(valReturnFromCodeBehide == "1"){
                            
                                if(chkBox.checked && $('#CountQtip').val() < 4){
                               
                                } 
                            }
                            else if(valReturnFromCodeBehide == "0"){
                                if(chkBox.checked){
                                    checkOverQuestion(chkBox.id);
                                }  
                            }                   
                        }
                    },
                    error: function myfunction(request, status)  { 
                    }
                });
                updateSpnSelect(window.self.document.location.href);
                }
                function checkOverQuestion(id){
                    var sumQuestionStep3 = $('#spnTotalQuestionsSelected1',window.parent.document).text();
                    var sumQuestionSelected = $('#spnTotalQuestionsSelected1').text();
                    var sumQuestion = parseInt(sumQuestionStep3) + parseInt(sumQuestionSelected);
                    if(parseInt(sumQuestion) > 120){
                        $('label[for="'+ id +'"]').qtip({
                            content: 'จำนวนข้อสอบที่เลือกมาเกิน 120 ข้อ จะใช้ร่วมกับกระดาษคำตอบไม่ได้นะคะ',
                            show: { ready: true },
                            style: {
                                width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                            },                             
                            position: {corner:{tooltip:'leftMiddle',target: 'rightMiddle' }},
                            hide:false                                                                    
                        });                    
                        setTimeout(function(){$('label[for="'+ id +'"]').qtip('destroy');},4000);
                    } 
                }
                function destroyQtip(id){
                    setTimeout(function(){$('label[for="'+ id +'"]').qtip('destroy');checkOverQuestion(id)},4000);
                    var countQtip = $('#CountQtip').val();              
                    $('#CountQtip').val(parseInt(countQtip) + 1);             
                }         
            
                // random FixAmount
                function randomFixAmount() {                 
                    //$(window).append('<div><h1><span style="color:white;font-size:30px;">รอสักครู่... กำลังสุ่มข้อค่ะ</span></h1></div>');
                    var getLengthChk = document.getElementsByName("test[]").length; //จำนวนทั้งหมดของ checkbox
                    var getEleChk = document.getElementsByName("test[]"); 
                    var getValtxtFixAmount = document.getElementById("txtFixAmount").value; // ค่าจาก textbox fixamount                
                    var chkTxt = checkNum("txtFixAmount",1); 
                    console.log('chkTxt : ' + chkTxt);                
                    if (chkTxt != true) {
                        OpenBlackScreen();
                        reChkbox(); // All chkbox = false
                        var noOfChkQuestion = parseInt(getLengthChk); // จำนวนของข้อสอบ                
                        //$("#chkAll").attr('checked',false);
                        var noOfRand = new Array(); // array เก็บลำดับ element ของข้อสอบ
                        var j = 0; // chkbox ของข้อสอบข้อแรก = 0
                        for (i = 0; i < noOfChkQuestion; i++) {
                            noOfRand[i] = j;
                            j++;
                        }
                        // loop random chkbox
                        for (var i = 1; i <= getValtxtFixAmount; i++) {
                            var Testrandom = (Math.floor(Math.random() * (noOfRand.length )));
                            var r = noOfRand[Testrandom];      
                            $(getEleChk.item(r)).attr("checked",true);
                            $(getEleChk.item(r)).triggerHandler('click');
                            noOfRand.splice(Testrandom, 1);                    
                        }                     
                        SumQuestionSelected();  
                        CloseBlackScreen();
                    }               
                }       
                //ALL CHECKBOX = FALSE
                var QsetId = '<%=qSetID %>';
                function reChkbox() {
                    var getLengthChk = document.getElementsByName("test[]").length; //console.log('reChkbox getLengthChk : ' + getLengthChk);
                    var getEleChk = document.getElementsByName("test[]");            
                    console.log('before inside recheckbox : ' + $("#chkAll").attr('checked'));
                    for (i = 0; i < getLengthChk; i++) {                      
                        if ($(getEleChk.item(i)).attr("checked") == 'checked') {                                         
                            $(getEleChk.item(i)).attr("checked",false); 
                            $(getEleChk.item(i)).triggerHandler('click');                     
                        }
                    }        
                    console.log('after inside recheckbox : ' + $("#chkAll").attr('checked'));
                    $.ajax({ type: "POST",
                        url: "<%=ResolveUrl("~")%>testset/SelectEachQuestion.aspx/delAllCodeBehide",
                        async: false, // ทำงานให้เสร็จก่อน
                        data: "{QsetId : '" + QsetId + "' }",  //" 
                        contentType: "application/json; charset=utf-8", dataType: "json",   
                        success: function (msg) {
                            if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                                valReturnFromCodeBehide = msg.d;  
                            }
                        },
                        error: function myfunction(request, status)  {  
                        }
                    });
                    }       
                    // random Percent
                    function randomPercent() {
                        var getLengthChk = document.getElementsByName("test[]").length; //จำนวนทั้งหมดของ checkbox
                        var getEleChk = document.getElementsByName("test[]"); 
                        var getValTxtPercent = document.getElementById("txtPercent").value; // ค่าจาก textbox Percent
                        var noOfChkQuestion = parseInt(getLengthChk); // จำนวนของข้อสอบ                    
                        var chkTxt = checkNum("txtPercent",2);    
                        var t = '<%=ToTalQuestionInQset%>';
                        var p = (1 / t) * 100;
                        getValTxtPercent = parseInt(getValTxtPercent) >= p ? getValTxtPercent : p;
                        if (chkTxt != true) {
                            OpenBlackScreen();
                            reChkbox(); // All chkbox = false
                            var noOfRand = new Array(); // array เก็บลำดับ element ของข้อสอบ
                            var j = 0; // chkbox ของข้อสอบข้อแรก = 0
                            for (i = 0; i < noOfChkQuestion; i++) {
                                noOfRand[i] = j;
                                j++;
                            }
                            // loop random chkbox
                            var per =  (parseInt(getValTxtPercent) * noOfChkQuestion )/100; // คำนวนจำนวนข้อจาก percent
                            for (var i = 1; i <= per; i++) {
                                var Testrandom = (Math.floor(Math.random() * (noOfRand.length)));
                                var r = noOfRand[Testrandom];               
                                $(getEleChk.item(r)).attr("checked",true);
                                $(getEleChk.item(r)).triggerHandler('click');
                                noOfRand.splice(Testrandom, 1);  
                            }
                            SumQuestionSelected();
                            CloseBlackScreen();
                        }
                        //updateSpnSelect(window.self.document.location.href);
                    }        
                    // check ค่าใน txtbox
                    function checkNum(txtbox,ty){
                        var valOfTxt = $("#"+txtbox).val();
                        var getLengthChk = document.getElementsByName("test[]").length;
                        var noOfChkQuestion = parseInt(getLengthChk); 
                        console.log(parseInt(valOfTxt));
                        var t = '<%=ToTalQuestionInQset%>';
                    var p = (1 / t) * 100;
                    if (isNaN(valOfTxt)) {
                        return true;
                    }
                    if (valOfTxt > noOfChkQuestion && ty == 1) {                       
                        callDialogAlert("กรอกค่ามากกว่าจำนวนข้อที่มีอยู่ (จำนวนข้อทั้งหมด " + noOfChkQuestion + " ข้อ)");
                        $("#"+txtbox).val('').focus();
                        return true;
                    } else if (valOfTxt <= 0 && ty == 1) {                        
                        callDialogAlert("สุ่มข้อสอบได้ระหว่าง 1 - " + t + " ค่ะ");
                        $("#"+txtbox).val('').focus();
                        return true;
                    } else if (parseInt(valOfTxt) <=0 && ty == 2) {  
                        callDialogAlert("สุ่มข้อสอบได้ระหว่าง 1 - 100 % ค่ะ");
                        $("#"+txtbox).val('').focus();
                        return true;
                    } else if (parseInt(valOfTxt) > 100 && ty == 2) {                        
                        callDialogAlert("สุ่มข้อสอบได้ระหว่าง 1 - 100 % ค่ะ");
                        $("#"+txtbox).val('').focus();
                        return true;
                    }
                    else{$('#dialogRandomQuestion').dialog('close');return false;
                    }
                }
                function updateSpnSelect(strLink){// update จำนวนข้อที่ถูกเลือก
                    var spnId = strLink;
                    //spnId = spnId.substring(spnId.indexOf("=")+1,spnId.length); // id ของ span ตัวที่จะถูกเปลียนค่า
                    var Str1 = (strLink.indexOf("=")+1);
                    var Str2 = (strLink.indexOf("&"));
                    spnId = strLink.substring(Str1,Str2);
                    //var spnTotalQselect = $("#spnTotalQuestionsSelected").html(); // span ผลรวมของหน้า lightbox
                    //console.log('spnTotalQselect : ' + spnTotalQselect);
                    //$("#spnSelected_"+spnId, window.parent.document).html(spnTotalQselect); // update span
                    var totalSpan = $(":checkbox[class^=chkQuestion]:checked").length; //parseInt($("#spnTotalQuestionsSelected").text(), 10);
                    var sumChkQusetion = $(":checkbox[class^=chkQuestion]").length;// จำนวนคำถามทั้งหมดในหน้า lightbox
                    if($.browser.msie){
                        var chkF = { 'background': 'url(../images/bullet.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px' };
                        var chkT = { 'background': 'url(../images/bullet_checked.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px' };
                        if(totalSpan == 0){ // check ถ้าผลรวมน้อยกว่า 0 ไม่ต้อง check ในหน้า step3
                            $('#MID'+spnId, window.parent.document).attr('checked',false);
                            $('#MID'+spnId, window.parent.document).next().css(chkF);
                            var str = "ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id='spnTotal_"+spnId + "'>"+sumChkQusetion+"</span> ข้อ)";
                            $('#MID'+spnId, window.parent.document).next().next().next().html(str);
                        }
                        else if(totalSpan > 0){
                            $('#MID'+spnId, window.parent.document).attr('checked','checked');
                            $('#MID'+spnId, window.parent.document).next().css(chkT);
                            var str = "ชุดนี้เลือกมาแล้ว <span id='spnSelected_"+spnId+"' name='spnSelec'>"+totalSpan+"</span> จาก <span id='spnTotal_"+spnId + "'>"+sumChkQusetion+"</span> ข้อ";
                            $('#MID'+spnId, window.parent.document).next().next().next().html(str);
                        }
                    }
                    else{                        
                        if(totalSpan == 0){ // check ถ้าผลรวมน้อยกว่า 0 ไม่ต้อง check ในหน้า step3
                            $('#MID'+spnId, window.parent.document).attr('checked',false);
                            var str = "ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id='spnTotal_"+spnId + "'>"+sumChkQusetion+"</span> ข้อ)";
                            $('#MID'+spnId, window.parent.document).next().next().next().html(str);
                        }
                        else if(totalSpan > 0){
                            $('#MID'+spnId, window.parent.document).attr('checked','checked');
                            var str = "ชุดนี้เลือกมาแล้ว <span id='spnSelected_"+spnId+"' name='spnSelec'>"+totalSpan+"</span> จาก <span id='spnTotal_"+spnId + "'>"+sumChkQusetion+"</span> ข้อ";
                            $('#MID'+spnId, window.parent.document).next().next().next().html(str);
                        }
                    }
                    window.parent.SumQuestionSelected();
                }
                function chkAllIsLoad() {                    
                    var getLengthChk = document.getElementsByName("test[]").length; //จำนวนทั้งหมดของ checkbox
                    var getEleChk = document.getElementsByName("test[]"); 
                    if ($.browser.msie) {//IE
                        var inputs = document.getElementsByTagName('input');
                        var chkbox = [];
                        for (var i = 0; i < inputs.length; i++) {
                            if (inputs.item(i).getAttribute('class') == 'chkQuestion') {
                                chkbox.push(inputs.item(i));
                            }
                        }
                        getLengthChk = chkbox.length;
                        var chkF = { 'background': 'url(../images/bullet.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px' };
                        var chkT = { 'background': 'url(../images/bullet_checked.gif) center left no-repeat', 'height': '21px', 'padding-left': '21px' };
                        for(i=0;i<getLengthChk;i++){
                            if(chkbox[i].checked == false){
                                $("#chkAll").attr('checked',false);
                                $("#chkAll").next().css(chkF)
                                break;}      
                            else{
                                $("#chkAll").attr('checked',true);
                                $("#chkAll").next().css(chkT);
                            }
                        }
                    }
                    else{//FF CHROME      
                        var countChecked = 0;
                        for(i=0;i<getLengthChk;i++) {
                            if ($(getEleChk.item(i)).attr('checked') == 'checked') {
                                countChecked++;
                            }                                                     
                        }
                        if (countChecked == getLengthChk) {
                            $("#chkAll").attr('checked', true);
                        } else {
                            if ($("#chkAll").attr('checked') == 'checked') {
                                $("#chkAll").attr('checked', false);
                            }
                        }    
                        console.log('CheckAllIsLoad = ' + $("#chkAll").attr('checked'));
                    }     
                }
                
                $(function () {
                    $('#HelpSelect a').stop().animate({ 'marginLeft': '-52px' }, 1000);
                    $('#HelpSelect > li').hover(
                    function () {
                        $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
                    },
                        function () {
                            $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
                        }
                        );
                });
                function ShowHelpSelect() {
                    parent.$.fancybox.close();
                    window.parent.ShowHelpSelect();                   
                }
    </script>

    <link charset="utf-8" media="screen" type="text/css" href="<%=ResolveUrl("~")%>css/aa.css"
        rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~")%>css/general.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <input type="hidden" id="CountQtip" value="0" />

    <div id="DialpgAlert" title="เลือกห้องเรียนก่อนเข้าทำควิซค่ะ"></div>

    <ul id="HelpSelect" style="display: none;">
        <li class="HelpSelect" style="z-index: 99;"><a title="สงสัยในการใช้งาน ทำตามขั้นตอนตัวอย่างนี้นะคะ"
            id="HelpSelect" onclick="ShowHelpSelect();">ช่วย<br />
            เหลือ<br />
        </a></li>
    </ul>

    <div id='DivFaceScroll'>
        <div id="site_content">
            <center>

                 <%If HttpContext.Current.Session("EditID") IsNot Nothing And HttpContext.Current.Session("EditID") <> "" Then %>
                    <div style="text-align:center;background-color:#FFF;padding:10px;border-radius:5px;width: 825px;">
                    <span style="color: red; font-size: 20px;"><%= EditTestSetWarningText %></span></div>
                <%End If %>

                <div id="div-2" style='text-align: left;    width: 100%;' classname="opacity85">
                    <asp:Repeater ID="ListingQuestionsInModule" runat="server">
                        <HeaderTemplate>
                            <table class="bordered" id='QuestionAnswer' style="width: 100%; border-spacing: 0;">
                                <thead>
                                    <tr>
                                        <th colspan="2" id="thTotalChoose">
                                            <div style="display:table;width:100%;">
                                                <div style="display:table-cell;width:50%;text-align:left;">
                                                    <% If HttpContext.Current.Application("NeedAddNewQuestionButton") = True Then%>
                                                        <img id='ImgAddNewQuestion' title='เพิ่มคำถาม' onclick='AddNewQuestion()' src="../Images/New.png" style='float: left; cursor: pointer;' />
                                                    <% End If%>

                                                    <span style="margin-left:10px;">ข้อสอบจำนวนทั้งหมด <span style="color: Black; font-size: x-large; font-weight: bold;" id="spnTotalQuestionInQset"></span> ข้อ</span>
                                                </div>
                                                <div style="display:table-cell;text-align:right;padding-right:10px;">
                                                    <%If IsUseFullQset Then%>
                                                        <span>ข้อสอบชุดนี้ต้องเลือกทั้งชุดเท่านั้นค่ะ</span>
                                                    <%Else%>
                                                        <span>เลือกไปแล้ว <span style="color: Black; font-size: x-large; font-weight: bold;" id='spnTotalQuestionsSelected'>0</span> ข้อ</span>
                                                    <%End If%>
                                                </div>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th colspan="2">
                                            <div id="DivchkAll" style="text-align:left;width:66%;">
                                                <input type="checkbox" value="" id="chkAll"><label for="chkAll" id="lblforchkAll">เลือกทั้งหมด</label>
                                                
                                                <%If Not IsUseFullQset Then%>
                                                    <div style="float:right;margin-top: -7px;">
                                                        <div id="divRandomQuestion" class="submitChangeFontSize" style="
                                                            width: 90px;
                                                            height: 40px;
                                                            cursor: pointer;
                                                            border-radius: 0.5em;
                                                            background: #cf8a0c;
                                                            color: white;
                                                            font-size: 120%;
                                                            border: brown;
                                                            line-height: 2.5em;
                                                            text-align: center;">สุ่มข้อสอบ
                                                        </div>
                                                    </div>
                                                <%End If%>                                      
                                            </div>   
                                        </th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="background: #FFFFCC; width: 80px; padding-bottom: 3px; padding-top: 3px;text-align: center;">
                                    <input onchange="SumQuestionSelected();" type="checkbox" <%# GetChecked(Eval("question_id").ToString())%>
                                        onclick="onSave('<%# Eval("question_id")%>    ', this, '<%# Eval("qset_id")%>    ', '<%# session("newTestSetId") %>    ', '<%# session("userId") %>    ', '<%= classId%>    ' );"
                                        name="test[]" class="chkQuestion" value="" id="<%# Eval("question_id")%>">
                                    
                                    <label for="<%# Eval("question_id")%>">ข้อที่<%# GetQuestionNo(True)%></br>(<%# Eval("qid")%>)</label>
                                    <br />
                                    <%If HttpContext.Current.Application("NeedEditButton") = True Then%>
                                    <br />
                                        <a href="editeachquestion.aspx?qid=<%# Eval("question_id")%>&QSetId=<%# Eval("qset_id")%>" target="_blank">
                                            <img title="Word" style="width:30px;" src="../Images/WIcon2.png" onclick="SaveLog('Word','<%# Eval("question_id")%>')" />
                                        </a>
                                        <a href="PreviewAndEditExam.aspx?qid=<%# Eval("question_id")%>&QSetId=<%# Eval("qset_id")%>&QNo=<%# GetQuestionNo(False)%>" target="_blank">
                                            <img title="Quiz" style="width: 30px;" src="../Images/QIcon2.png" onclick="SaveLog('Quiz','<%# Eval("question_id")%>')" />
                                        </a>
                                        <br />
                                        ---------------
                                        <asp:Label ID="lblHeadEdit" runat="server" Text="อธิบายโจทย์" Font-Bold="True"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblEditText1" runat="server" Text='<%# GetExplainStatusText(Eval("question_id").ToString()) %>'></asp:Label>
                                        <br />
                                    <%End If%>
                                        ---------------
                                    <%If HttpContext.Current.Application("NeedAddEvaluationIndex") = True Then%>
                                        <br />
                                        <a href="AddEvaluationIndexForQuestion.aspx?qid=<%# Eval("question_id")%>" target="_blank">
                                            <img src="../Images/Alert.png" />
                                        </a>
                                        <br />
                                        <asp:Label ID="lblHeadEva" runat="server" Text="ดัชนี" Font-Bold="True"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblEvaText1" runat="server" Text='<%# GetEvaluatinStatusText(Eval("question_id").ToString()) %>'></asp:Label>
                                        <br />
                                    <%End If%>
                                    ---------------
                                    <%If HttpContext.Current.Application("NeedDeleteQuestionButton") = True Then%>
                                        <img src="../Images/Delete-icon.png" style='cursor: pointer; margin-top:10px;' title='ลบคำถาม' onclick="DeleteQuestion('<%# Eval("question_id")%>')" />
                                    <%End If%>
                                </td>
                                <td style="padding-bottom: 3px; padding-top: 9px;">
                                    <table border="0" class="ForIe8" style="margin-top: 3px; margin-bottom: 3px; width: 100%">
                                        <tr>
                                            <%# Eval("question_name")%>
                                            <%# GetAnswerLists(Eval("question_id").ToString(), Eval("qSet_Type").ToString())%>
                                         </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td style="background: #FFFFFF; width: 80px; padding-bottom: 3px; padding-top: 3px;text-align: center;">
                                    <input onchange="SumQuestionSelected();" type="checkbox" <%# GetChecked(Eval("question_id").ToString())%>
                                        onclick="onSave('<%# Eval("question_id")%>    ', this, '<%# Eval("qset_id")%>    ', '<%# session("newTestSetId") %>    ', '<%# session("userId") %>    ', '<%= classId%>    '  );"
                                        name="test[]" class="chkQuestion" value="" id="<%# Eval("question_id")%>">

                                    <label for="<%# Eval("question_id")%>">ข้อที่<%# GetQuestionNo(True)%></br>(<%# Eval("qid")%>)</label>
                                    <br />
                                    <%If HttpContext.Current.Application("NeedEditButton") = True Then%>
                                        <br />
                                         <a href="editeachquestion.aspx?qid=<%# Eval("question_id")%>&QSetId=<%# Eval("qset_id")%>" target="_blank">
                                            <img title="Word" style="width:30px;" src="../Images/WIcon2.png" onclick="SaveLog('Word','<%# Eval("question_id")%>')" />
                                        </a>
                                        <a href="PreviewAndEditExam.aspx?qid=<%# Eval("question_id")%>&QSetId=<%# Eval("qset_id")%>&QNo=<%# GetQuestionNo(False)%>" target="_blank">
                                            <img title="Quiz" style="width: 30px;" src="../Images/QIcon2.png" onclick="SaveLog('Quiz','<%# Eval("question_id")%>')" />
                                        </a>
                                         <br />
                                        ---------------
                                        <asp:Label ID="lblHeadEdit2" runat="server" Text="อธิบายโจทย์" Font-Bold="True"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblEditText2" runat="server" Text='<%# GetExplainStatusText(Eval("question_id").ToString()) %>'></asp:Label>
                                        <br />
                                    <%End If%>
                                    ---------------
                                    <%If HttpContext.Current.Application("NeedAddEvaluationIndex") = True Then%>
                                    <br />
                                        <a href="AddEvaluationIndexForQuestion.aspx?qid=<%# Eval("question_id")%>" target="_blank">
                                            <img src="../Images/Alert.png" />
                                        </a>
                                        <br />
                                        <asp:Label ID="lblHeadEva2" runat="server" Text="ดัชนี" Font-Bold="True"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblEvaText2" runat="server" Text='<%# GetEvaluatinStatusText(Eval("question_id").ToString()) %>'></asp:Label>
                                    <br />
                                    <%End If%>
                                    ---------------
                                    <%If HttpContext.Current.Application("NeedDeleteQuestionButton") = True Then%>
                                        <img src="../Images/Delete-icon.png" style='cursor: pointer;' title='ลบคำถาม' onclick="DeleteQuestion('<%# Eval("question_id")%>')" />
                                    <%End If%>
                                </td>
                                <td style="padding-bottom: 3px; padding-top: 9px;">
                                    <table border="0" class="ForIe8" style="margin-top: 3px; margin-bottom: 3px;">
                                        <tr>
                                            <%# Eval("question_name")%>
                                            <%# GetAnswerLists(Eval("question_id").ToString(), Eval("qSet_Type").ToString())%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

                <%If Not IsUseFullQset Then%>
                    <a id="footer-back-to-top" style='cursor: not-allowed;width: 120px;' class="WhiteButton badge-back-to-top ">
                    <strong>เลือกไปแล้ว<span id='spnTotalQuestionsSelected1'>0</span> ข้อ</strong></a>
                <%End If%>
            </center>
        </div>
    </div>

    <div id="dialogRandomQuestion" title="">
        <div style="text-align: center; padding: 10px; margin-top: 5px; border-radius: 5px; border: 1px solid #E78F08; font-size: 22px;">
            <div>
                <span>เลือกสุ่มจำนวน </span>
                <input type="text" value="" maxlength="3" size="3" id="txtFixAmount" style="width: 40px; height: 35px;"
                    class="rightJustified" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                <span>ข้อ</span>
                <br />
                <span>(ระบุตัวเลขห้ามเกิน <span id="spnTotalQuestionInQset2"></span>ข้อ)</span>
            </div>
            <div style="display: none;">หรือ</div>
            <div style="display: none;">
                <span>เลือกสุ่ม % </span>
                <input type="text" value="" maxlength="3" size="3" id="txtPercent" style="width: 40px; height: 35px;"
                    class="rightJustified" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                <span>%</span>
                <br />
                <span id="spnTotalPercent">(ระบุตัวเลข 0-100 เท่านั้น)</span>
            </div>
            <input type="button" id="btnRandomQuestion" value="ตกลง" />
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" />
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;
        var IOS = /(iPad|iPhone|iPod)/g.test(navigator.userAgent);
        if (!isAndroid && !IOS) {
            $("#DivFaceScroll").alternateScroll();
        }
        
        $(document).ready(function () {
            //$('#dialogRandomQuestion').dialog({
            //    autoOpen: false,
            //    buttons: myBtn,
            //    draggable: false, resizable: false, modal: true
            //});
            $('#dialogRandomQuestion').dialog({ autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto' });
            // dialog alert
            $('#DialpgAlert').dialog({
                autoOpen: false, draggable: false, resizable: false, modal: true, width: 'auto', buttons: {
                    "ตกลง": function () {
                        $(this).dialog('close');
                    }
                }
            });

            $('#divRandomQuestion').click(function() {
                $('#dialogRandomQuestion').dialog('option', 'title', "เลือกสุ่มข้อ").dialog('open');
                if ($('#closeDialog').length == 0) {
                    $('.ui-dialog').append('<div id="closeDialog" style= "width: 25px;height: 25px;background-color: white;right: 20px;top: 30px;position: absolute;border-radius: 50%;text-align: center;font-size: 15px;color: orange;font-weight: bold;cursor:pointer;" >X</div>');
                }
            });
            // ปุ่มกด ปิด dialog สุ่มข้อสอบ
            $('#closeDialog').live('click',function() {
                clearValueInTextbox();
                $('#dialogRandomQuestion').dialog('close');                    
            });
            // ตอนกดปุ่ม ตกลง ตอนสุ่มข้อสอบ
            $('#btnRandomQuestion').click(function() {
                var fixAmount = $('#txtFixAmount').val();
                var percent = $('#txtPercent').val();
                if (fixAmount == '' && percent == ''){
                    callDialogAlert('ใส่จำนวนเลขเพื่อสุ่มข้อสอบค่ะ');
                    return false;
                }
                if (fixAmount != '' && percent != '') {
                    clearValueInTextbox();
                    callDialogAlert('สุ่มข้อสอบได้อย่างใดอย่างหนึ่งค่ะ');
                    return false;
                }
                if (fixAmount != '') {                   
                    randomFixAmount();
                } else {
                    randomPercent();
                }                
            });

            //chkbox ใส่จำนวนทีต้องการสุ่ม
            //new FastButton(document.getElementById('DivchkRandomFixAmount'),TriggerchkRandomFixAmount);

            //chkbox ระบุจำนวนเปอร์เซ็นต์ที่ต้องการสุ่ม
            //new FastButton(document.getElementById('DivchkRandomPercent'),TriggerchkRandomPercent);
            
            //chkbox เลือกทั้งหมด
            new FastButton(document.getElementById('DivchkAll'),TriggerchkAll);
            
            //ดักถ้าเข้าจาก Tablet ของครู
            if (isAndroid) {
                //$('#site_content').css('width','950px');
                //$('#div1').css('width','810px');
                //$('#footer-back-to-top').hide();
                //$('#HelpSelect').remove();
                //$('table tr td').css('font-size','25px');
                //$('body').css('background','#2AA0B3');
                //$('#thHeader').css('font-size','25px');
                //$('#lblforchkrandom').css('font-size','25px');
                //$('#thTotalChoose').css('font-size','25px');
                //$('#lblforchkAll').css('font-size','20px');
                //$('#tdRandom').css('font-size','25px;');
                //$('#DivFaceScroll').height(450);
            }
            else {
                $('#DivFaceScroll').height(600);
            }
             
            $('#navigation').remove();
            $('#Help').remove();

            chkAllIsLoad(); // เช็คว่า checkbox ข้อทุกข้อเลือกครบหรือเปล่า ถ้าครบให้ checkbox เลือกทั้งหมด ถูกติ๊กด้วย          
            $(':checkbox[class^=chkQuestion]').click(function () { // เช็คทุกครั้งที่กดเลือกข้อสอบ ถ้าเลือกครบทุกข้อในชุด  ให้ checkbox เลือกทั้งหมด ถูกติ๊กด้วย
                chkAllIsLoad();
            });

            SumQuestionSelected();

            $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 54px;" />')
            //        var GetScreenHeight = Position.screenHeight();
            //        alert(GetScreenHeight);
            //$('#DivFaceScroll').height(600);

            var IsUseFullQset = '<%=IsUseFullQset%>'; console.log(IsUseFullQset);
            if (IsUseFullQset == 'True') { 
                $('#lblforchkAll').text('เลือกชุดนี้'); 
                $(":checkbox[class^=chkQuestion] + label").css('background-image','none');
                $(':checkbox[class^=chkQuestion]').attr('disabled','disabled');
            }
        });

        function clearValueInTextbox(){
            $('#txtFixAmount').val('');
            $('#txtPercent').val('');
        }
        //function TriggerchkRandomFixAmount() {
        //    if ($('#chkRandomFixAmount').attr('checked') == 'checked') {
        //        $('#chkRandomFixAmount').removeAttr('checked');
        //        $('#chkRandomFixAmount').trigger('change');
        //    }
        //    else {
        //        $('#chkRandomFixAmount').attr('checked', 'checked');
        //        $('#chkRandomFixAmount').trigger('change');
        //    }
        //}

        //function TriggerchkRandomPercent() {
        //    if ($('#chkRandomPercent').attr('checked') == 'checked') {
        //        $('#chkRandomPercent').removeAttr('checked');
        //        $('#chkRandomPercent').trigger('change');
        //    }
        //    else {
        //        $('#chkRandomPercent').attr('checked', 'checked');
        //        $('#chkRandomPercent').trigger('change');
        //    }
        //}

        function TriggerchkAll() {
            //if ($('#chkAll').attr('checked') == 'checked') {            
            //    $('#chkAll').removeAttr('checked');
            //    $('#chkAll').trigger('click');
            //}
            //else {            
            //    $('#chkAll').attr('checked', 'checked');
            //    $('#chkAll').trigger('click');
            //}
        }

        function AddNewQuestion() {
            var JVQsetId = '<%=VBQsetId %>';

            if (confirm('ต้องการเพิ่มคำถาม ?') == true) {
                $.ajax({ type: "POST",
                    url: "<%=ResolveUrl("~")%>TestSet/SelectEachQuestion.aspx/AddNewQuestionCodeBehind",
                    data: "{ StrQsetId: '" + JVQsetId + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",   
                    success: function (msg) {
                        if (msg.d == 'Complete') {
                            window.location.reload();
                        }
                    },
                    error: function myfunction(request, status)  {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                    }
                });
                }
            }
            function DeleteQuestion(InputQuestionId) {    
                var JVQsetId = '<%=VBQsetId %>';
                if (confirm('ต้องการลบคำถามข้อนี้ ?') == true) {
                    $.ajax({ type: "POST",
                        url: "<%=ResolveUrl("~")%>TestSet/SelectEachQuestion.aspx/DeleteQuestionCodeBehind",
                        data: "{ StrQset_Id: '" + JVQsetId + "',StrQuestionId:'" + InputQuestionId + "'}",
                        contentType: "application/json; charset=utf-8", dataType: "json",   
                        success: function (msg) {
                            if (msg.d == 'Complete') {
                                window.location.reload();
                            }
                        },
                        error: function myfunction(request, status)  {
                            alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                        }
                    });
                    }
                }  
                function OpenBlackScreen() {// จอดำๆ ตอนกำลังสุ่ม
                    //$('#dialogRandomQuestion').dialog('close');
                    $.blockUI({
                        message: '<h1><span style="color:white;font-size:30px;">รอสักครู่... กำลังสุ่มข้อค่ะ</span></h1>',
                        css: {
                            border: 'none',                           
                            backgroundColor: 'transparent'
                        }
                    });           
                }
                function CloseBlackScreen() {
                    setTimeout(function(){
                        $.unblockUI();
                    },1000);             
                    // $('#dialogRandomQuestion').dialog('close');
                }
                // เรียก dialog ที่เกียวกับการ alert
                function callDialogAlert(title) {
                    $('#DialpgAlert').next().next().remove();
                    $('#DialpgAlert').dialog('open').dialog('option', 'title', title);
                }
    </script>
    <script type="text/javascript">
        var qsetPath = '<%=qsetFilePath%>';       
        $(function () {
            $('.imgSound').each(function () {               
                var fileName = $(this).attr('alt');                
                $(this).after($('<audio>', { src: qsetPath + fileName, controls: '' }));
                $(this).remove();
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
