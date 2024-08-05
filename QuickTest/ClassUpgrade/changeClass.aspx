<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="changeClass.aspx.vb" Inherits="QuickTest.changeClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/apprise-1.5.full.js" type="text/javascript"></script>
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/apprise.css" rel="stylesheet" type="text/css" />
    <link href="../css/styleQuiz.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .divclass
        {
            background-color: #fff;
            height: 40px;
            width: 40px;
            text-align: center;
            position: relative;
            /*float: left;*/
            margin: 5px;
            line-height: 2.2;
            -webkit-border-radius: 0.5em;
            cursor: pointer;
            color: Black;
        }
        .divTabClass
        {
            height: 50px;
            background-color: #96ceed;
            -webkit-border-radius: 0.5em;
            position: relative;
            width:auto;
        }
        .tgDivClass
        {
            background-color: #FFC76F;
            color: White;
        }
        /* ถังขยะ */
        #trash
        {
            width: 400px;
            height: 150px;
            padding: 1px;
            text-align: center;
            border: 2px solid #8BFF7B;
            background-color: #AAFF7B;
            overflow: auto;
            margin-left: 30px;
            -webkit-border-radius: 5px;
        }
        /* ย้าย */
        #move
        {
            width: 400px;
            height: 150px;
            padding: 1px;
            text-align: center;
            border: 2px solid #69CFFF;
            background-color: #69E7FF;
            overflow: auto;
            margin-left: 30px;
            -webkit-border-radius: 5px;
        }
        #move .spnNumberChange, #trash .spnNumberChange
        {
            border: 1px solid;
            width: auto;
            display: block;
            color: Black;
            height: 25px;
            background-color: orangered;
            background-image: url('../Images/upgradeClass/DeleteClass-icon.png');
            background-repeat: no-repeat;
            background-position: right;
            text-align: center;
            cursor: pointer;
            font: normal 15px 'THSarabunNew';
        }
    </style>
    <style type="text/css">
        .divAccL
        {
            width: 400px;
            margin-left: 35px;
            float: left;
        }
        .divAccL div, .divAccR div
        {
            height: 150px;
        }
        .divAccR
        {
            width: 400px;
            margin-right: 35px;
            float: right;
        }
        
        #accordion div span, #accordion2 div span
        {
            border: 1px solid;
            width: auto;
            display: block;
            color: Black;
            background-color: White;
            text-align: center;
            cursor: pointer;
            height: 25px;
            font: normal 15px 'THSarabunNew';
        }
        
        #accordion div span.spnNumberChange, #accordion2 div span.spnNumberChange
        {
            border: 1px solid;
            width: auto;
            display: block;
            color: Black;
            background-color: orangered;
            background-image: url('../Images/upgradeClass/DeleteClass-icon.png');
            background-repeat: no-repeat;
            background-position: right;
            text-align: center;
            cursor: pointer;
        }
        
        .divDraggable
        {
            border: 1px solid;
            background-color: Orange;
            width: 222px;
            height: 22px;
            text-align: center;
        }
    </style>
    <%--<script type="text/javascript">
        $(function () {
            $('#dialog2').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () {
                    window.parent.location.href = '/testset/step1.aspx';
                }, 'ไม่': function () {
                    $(this).dialog('close');
                }
                },
                draggable: false,
                resizable: false,
                modal: true
            });
            
            $('#divAddRoom').live('click', function () {
                var room = '<li><a>ห้อง </a><div><ul><li></li></ul></div></li>';
                $('.ulChangeRoom').append(room).accordion('destroy').accordion();
                //$('.accordion').append($('<li/>', { 'data-role': 'list-divider' }).append($('<a/>', { 'data-transition': 'slide', 'text': 'hello' })));
            });
        });
        
    </script>
    <script type="text/javascript">
        $(function () {
            $('#accordion').accordion({
        });
        $('#accordion2').accordion({
            colapsible: true,
            active: 1
        });    
    });
    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            
            // event ปุ่มเลือกชั้น
            $('.divclass').first().addClass('tgDivClass');
            // แสดงห้องที่ปุ่มแรกสุด    
            var className = $('.tgDivClass').html();
            setRoomFromSelected($.trim(className));
            // เมื่อคลิกที่ปุ่มเลือกชั้น
            $('.divclass').click(function () {
                $('.divclass').each(function () {
                    $(this).removeClass('tgDivClass');
                });
                $(this).toggleClass('tgDivClass');
                var classRoom = $(this).text();                
                setRoomFromSelected($.trim(classRoom));
                $('#move span').remove();
                $('#trash span').remove();
            });
            // เมื่อเลือกห้องที่ accordion
            $('.divAccL a').live('click', function () {
                var selectedClass = $('.tgDivClass').html();
                var selectedRoom = $(this).text();

                createRoomForChage($.trim(selectedClass),$.trim(selectedRoom));
   
            });
            // เมื่อลบนักเรียนที่ย้ายมาแล้ว
            $('.spnNumberChange').live('click', function () {
                var id = $(this).attr('id');
                id = id.replace('_change','');
                var number = $(this).attr('number');                                           
                $('#' + id).text(number);
                $('#' + id).draggable({ disabled: false })
                //$(this).remove();
                $('.'+ id +'_g.spnNumberChange').remove();
            });          
            // เมื่อย้ายโรงเรียน
            $('#move').droppable({
                hoverClass: 'moveHover',
                drop: function (event, ui) {
                    var id = ui.draggable.attr('id');
                    number = ui.draggable.text();
                    fromRoom = ui.draggable.parent().prev().text();
                    var st = 'width: 293px;margin-left: auto;margin-right: auto;'
                    //$('<span class="spnNumberChange" id="' + id + '"></span>').text(number + ' ย้ายออกจาก ' + fromRoom).appendTo(this);
                    $('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" style="'+st+'"></span>').text(number + ' ย้ายออกจากห้อง ' + fromRoom).appendTo(this);
                    $('#'+id+'_change').addClass('spnNumberChange');

                    var numberNext = number.split('  ');
                    // add atribute number_change and changeType and Room
                    $('#'+id+'_change').attr('number_change',numberNext[1]);
                    $('#'+id+'_change').attr('changeType','3');
                    $('#'+id+'_change').attr('room','0');

                    ui.draggable.text(number + ' ย้ายโรงเรียน');
                    $('#' + id).draggable({ disabled: true });
                }
            });
            //เมื่อ ไล่ออก/ลาออก
            $('#trash').droppable({
                hoverClass: 'moveHover',
                drop: function (event, ui) {
                    var id = ui.draggable.attr('id');
                    number = ui.draggable.text();
                    fromRoom = ui.draggable.parent().prev().text();
                    var st = 'width: 293px;margin-left: auto;margin-right: auto;'
                    //$('<span class="spnNumberChange" id="' + id + '"></span>').text(number + ' ย้ายออกจาก ' + fromRoom).appendTo(this);
                    $('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" style="'+st+'"></span>').text(number + ' ย้ายออกจากห้อง ' + fromRoom).appendTo(this);
                    $('#'+id+'_change').addClass('spnNumberChange');
                    
                    var numberNext = number.split('  ');
                    // add atribute number_change and changeType and Room
                    $('#'+id+'_change').attr('number_change',numberNext[1]);
                    $('#'+id+'_change').attr('changeType','0');
                    $('#'+id+'_change').attr('room','0');

                    ui.draggable.text(number + ' ลาออก/ไล่ออก');
                    $('#' + id).draggable({ disabled: true });
                }
            });
            //ปุ่มบันทีก
            $('#btnSave').click(function (e) {
                e.preventDefault();
                //$('#dialog2').dialog('open');
                
                var studentForChange=[];
                var i = 0;
                //$('.ui-draggable-disabled').each(function(){
                $('.spnNumberChange').each(function(){
                        // get class student
                        var studentClass = $('.tgDivClass').html(); 
                                                             
                       // get guid student
                       var id = $(this).attr('id');
                       id = id.replace('_change','');

                       // get numberStudent
                       //var numberStudent = $(this).attr('number');
                       var numberStudent = $(this).attr('number_change');
                       numberStudent = numberStudent.replace('เลขที่','');
                       
                       // get ห้องที่ย้ายไป
                       var roomChange;// = $(this).html();
                       roomChange = $(this).attr('room');
//                       var last = roomChange.split(' ');
//                       roomChange = last[last.length -1];                       
//                       $.trim(roomChange);

                       //เช็ค type 
                       var typeChange;
//                       if(roomChange == 'ย้ายโรงเรียน'){typeChange = '3';roomChange = '0'}
//                       else if(roomChange == 'ลาออก/ไล่ออก'){typeChange = '0';roomChange = '0'}
//                       else{ typeChange = '1';}
                        
                       typeChange = $(this).attr('changeType');
                       // เก๊บข้อมูลแบบ object แล้วเก๊บ ลง array อีกทีนึง
                       var obj1 = new changeObject(); 
                       obj1.StudentId = id;
                       obj1.StudentNewNumber = $.trim(numberStudent);
                       obj1.StundentTypeChange = typeChange;
                       obj1.StudentNewRoom = roomChange;
                       obj1.StudentClass = $.trim(studentClass);
                       studentForChange[i] = obj1;
                       i = i +1;       
                });
//                var d = s.split(',');
//                var txt = '';
//                for (i = 0;i< d.length;i++){
//                    if(txt.indexOf(d[i]) == -1){
//                        txt += d[i] + ',';
//                    }                    
//                }      
                  var studentChange = JSON.stringify(studentForChange);      
                  updateStudentChange(studentChange);
                  $('#move span').remove();
                  $('#trash span').remove();
            });
            // ปุ่ม add room
            $('#clickAdd').live('click', function () {
                //$('#dialog').dialog('open');
                apprise('ใส่ชื่อห้องที่ต้องการค่ะ <br />',' ',{'custom':' ','textOk':'เพิ่มห้อง','textCancel':'ไม่เพิ่มละ'},function(r){
                    if(r){
                        var newRoom = r.toString();
                        creRoom(newRoom);
                        setSessionNewRoom(newRoom); 
                    }
                });
                
            });
            // dialog add room
            $('#dialog').dialog({
                autoOpen: false,
                buttons: { 'ใช่': function () {
                    var newRoom = $('#newRoom').val();
                    $('<%=HttpContext.Current.Session("newRoom")%>').val(newRoom);
                    creRoom(newRoom);
                    setSessionNewRoom(newRoom);
                    $('#newRoom').val("");                    
                    $(this).dialog('close');
                }, 'ไม่': function () {
                    $('#newRoom').val("");
                    $(this).dialog('close');
                    }
                },
                draggable: false,
                resizable: false,
                modal: true
            });            
        });
        // function object to list
        function changeObject(){
            this.StudentId;
            this.StudentNewNumber;
            this.StundentTypeChange;
            this.StudentNewRoom;
            this.StudentClass;
        }
        // function สร้าง accordion ห้อง
        function setRoomFromSelected(classRoom) {
             $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>ClassUpgrade/changeClass.aspx/createRoomInClass",
                  data:"{ classRoom : '" + classRoom + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {
                        $('#accordion').html('');
                        $('#accordion').append(data.d).accordion('destroy').accordion();                      
                        liveDraggable();
                        var room = $('#accordion span.ui-icon-triangle-1-s').next('a').html();                                              
                        createRoomForChage($.trim($('.tgDivClass').html()),$.trim(room));                         
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
                });
        }
        // สร้างห้่องสำหรับย้าย
        function createRoomForChage(classSelected, roomSelected) {
             $.ajax({ type: "POST",
                 url: "<%=ResolveUrl("~")%>ClassUpgrade/changeClass.aspx/createRoomInClassForChange",
                 data : "{classSelected : '" + classSelected + "', roomSelected : '" + roomSelected + "'}",
                 async:false,
                 contentType:"application/json;charset=utf-8",dataType:"json",
                 success:function(data){
                    $('#accordion2').html('');                    
                    $('#accordion2').append(data.d).accordion('destroy').accordion();
                    //$('#room4_change').html($('#room4').html()); 
                    $('#accordion2 div').each(function(){
                        var idChange = $(this).attr('id');
                        id = idChange.replace('_change','');
                        $('#'+idChange).html($('#'+id).html());                         
                    });                       
                    liveAccordion();                    
                 },
                 error:function myfunction(request,status){
                    alert(status);
                 }
             });
        }
        // function live accordion
        function liveAccordion(){                    
                    $('#accordion2 div').live().droppable({
                        drop: function (event, ui) {
                        var currentDrop = $(this);
                       
                        //dialog เปลี่ยนเลขที่
                        var isSort = -1;
                        try{
                            var lastNumberArr = $(currentDrop).children('span').last().html().split('  ');
                            var lastNumber = parseInt(lastNumberArr[1]) + 1; 
                        }
                        catch(err){
                            var lastNumber = 1;
                            isSort = false; 
                        }
                                               
                                                 
                        var id = ui.draggable.attr('id');// id และ number ของ span ที่ลากมา
                        var number = ui.draggable.text();  // เลขที่    
                        var fromRoom = ui.draggable.parent().prev().text(); // ย้ายมาจากห้อง
                        //var toRoom = $('#' + id + '_change' + '.spnNumberChange').parent().prev().text(); // ย้ายไปยังห้อง
                        var toRoom = $.trim($(currentDrop).prev().text()); // ย้ายไปยังห้อง
                        var spanPrepend;
                        var className = $.trim($('.tgDivClass').html());
                        var studentFrom = 'ย้ายนักเรียน' + number + ' จากห้อง ' + className + fromRoom + '<br />มาเป็นเลขที่ ';
                        var studentTo = ' ในห้อง ' + className + toRoom + '<br />ใช่มั้ยคะ ?';        
                        apprise(studentFrom.toString(),studentTo.toString(),{'custom':lastNumber.toString(),'textOk':'ตกลงย้าย','textCancel':'ยังไม่ย้าย'},function(r){
                            if(r){ 
                            
                                var newNumber = r.split('  ');
                                var allNumber = '|';
                                spanPrepend = $(currentDrop).children('span').first();                                      
                                                                                                
                                $(currentDrop).children('span').each(function(){
                                    var cur = $(this).html().split('  ');                                    
                                    var curNumber = cur[1].substring(0,2);
                                    allNumber += $.trim(curNumber) + '|';                                   
                                                                                                      
                                    if(r > parseInt(curNumber)){                                                                            
                                        spanPrepend = $(this);                                       
                                        isSort = true;
                                        if($(this).is($(currentDrop).children('span').last())){
                                            isSort = false;
                                        }
                                    }                                                                                                
                                });                                
                                if(allNumber.indexOf('|'+$.trim(r)+'|') != -1){
                                    apprise('เลขที่ซ้ำค่ะ !!');
                                }
                                else{                                   
                                    lastNumber = 'เลขที่  ' + r;
                                    // append span เมื่อลากย้ายห้อง
                                    if(isSort == false){$('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>').appendTo($(currentDrop));}
                                    else if(isSort == true){$(spanPrepend).after($('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>'));}                                    
                                    else{$(spanPrepend).before($('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>'));}
                                    $('#'+id+'_change').addClass('spnNumberChange');
                                    $('#'+id+'_change').text(lastNumber + ' ย้ายออกจากห้อง ' + fromRoom) // set text span
                                    // add atribute number_change and changeType and Room
                                    $('#'+id+'_change').attr('number_change',lastNumber);
                                    $('#'+id+'_change').attr('changeType','1');
                                    $('#'+id+'_change').attr('room',toRoom);
                                    // เพิ่ม span นักเรียนที่ย้าย ไว้ที่ห้องปัจจุบันด้วย
                                    //var idParentChange = $('#' + id + '_change' + '.spnNumberChange').parent().attr('id');
                                    var idParentChange = $(currentDrop).attr('id')                       
                                    var idParentCurrent = idParentChange.replace('_change','');
                                    $('#'+idParentCurrent).html('');                   
                                    $('#'+idParentCurrent).append($('#'+idParentChange).html()); 
                                    // ให้ span draggable ได้
                                    liveDraggable();  
                                    $('#' + id).draggable({ disabled: true }); // set disabled  
                                    ui.draggable.text(number + ' ย้ายไปห้อง ' + toRoom); // set text ตัวที่ลาก
                                 }                                
                             }
//                             else{
//                                lastNumber = 'เลขที่  '+ lastNumber;
//                                // append span เมื่อลากย้ายห้อง
//                                $('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>').appendTo($(currentDrop));
//                                $('#'+id+'_change').addClass('spnNumberChange');
//                                $('#'+id+'_change').text(lastNumber + ' ย้ายออกจากห้อง ' + fromRoom) // set text span
//                                // add atribute number_change and changeType and Room
//                                $('#'+id+'_change').attr('number_change',lastNumber);
//                                $('#'+id+'_change').attr('changeType','1');
//                                $('#'+id+'_change').attr('room',toRoom);
//                                // เพิ่ม span นักเรียนที่ย้าย ไว้ที่ห้องปัจจุบันด้วย
//                                var idParentChange = $('#' + id + '_change' + '.spnNumberChange').parent().attr('id');                        
//                                var idParentCurrent = idParentChange.replace('_change','');
//                                $('#'+idParentCurrent).html('');                   
//                                $('#'+idParentCurrent).append($('#'+idParentChange).html()); 
//                                // ให้ span draggable ได้
//                                liveDraggable();  
//                                $('#' + id).draggable({ disabled: true }); // set disabled  
//                                ui.draggable.text(number + ' ย้ายไปห้อง ' + toRoom); // set text ตัวที่ลาก
//                             }                                                         
                         });                 
                        }
                    });
        }        
        //function liveDraggable
        function liveDraggable(){
            $('#accordion div span').draggable({
                        //revert: 'invalid',       
                        helper: function(e){
                            return $("<div class='divDraggable'>" + $(this).html()  + "</div>");
                        },
                        appendTo: 'body',
                        revert: 'inital',
                        cursor: 'move'                      
                        //containment: '#divMain'
                    });                             
        }
        // ส่งค่าเพื่อไปอัพเดทสำหรับนักเรียนที่ย้ายห้อง ย้ายโรงเรียน ลาออก 
        function updateStudentChange(studentChange) {
             $.ajax({ type: "POST",
                 url: "<%=ResolveUrl("~")%>ClassUpgrade/changeClass.aspx/updateStudentChangeCodeBehind",
                 data : "{studentChange : '" + studentChange + "'}",
                 async:false,
                 contentType:"application/json;charset=utf-8",dataType:"json",
                 success:function(data){
                        var className = $('.tgDivClass').html();
                        setRoomFromSelected($.trim(className));
                        var room = $('#accordion span.ui-icon-triangle-1-s').next('a').html();                                              
                        createRoomForChage($.trim($('.tgDivClass').html()),$.trim(room));                        
                 },
                 error:function myfunction(request,status){
                    alert(status);
                 }
             });
        }
        // function cre room
        function creRoom(newClass) {
            newClass = $.trim(newClass);
            var addChange = '<h3><a href="#" style="background-color: #FFC76F;">' + newClass + '</a></h3><div id="room'+ newClass +'_change" style="background-color: #F4F7FF;"></div>';
            var add = '<h3><a href="#" style="background-color: #FFC76F;">' + newClass + '</a></h3><div id="room'+ newClass +'" style="background-color: #F4F7FF;"></div>';
            $('#accordion').append(add).accordion('destroy').accordion();
            $('#accordion2').append(addChange).accordion('destroy').accordion();
            $('#accordion2 div').droppable('destroy').droppable({
                    drop: function (event, ui) {
                        var currentDrop = $(this);
                        var isSort = -1;
                        //dialog เปลี่ยนเลขที่
                        
                        try{
                            var lastNumberArr = $(currentDrop).children('span').last().html().split('  ');
                            var lastNumber = parseInt(lastNumberArr[1]) + 1; 
                        }
                        catch(err){
                            var lastNumber = '1';
                            isSort = false; 
                        }                                              
                                                 
                        var id = ui.draggable.attr('id');// id และ number ของ span ที่ลากมา
                        var number = ui.draggable.text();  // เลขที่    
                        var fromRoom = ui.draggable.parent().prev().text(); // ย้ายมาจากห้อง
                        //var toRoom = $('#' + id + '_change' + '.spnNumberChange').parent().prev().text(); // ย้ายไปยังห้อง
                        var toRoom = $.trim($(currentDrop).prev().text()); // ย้ายไปยังห้อง
                        var spanPrepend;
                        var className = $.trim($('.tgDivClass').html());
                        var studentFrom = 'ย้ายนักเรียน' + number + ' จากห้อง ' + className + fromRoom + '<br />มาเป็นเลขที่ ';
                        var studentTo = ' ในห้อง ' + className + toRoom + '<br />ใช่มั้ยคะ ?';        
                        apprise(studentFrom.toString(),studentTo.toString(),{'custom':lastNumber.toString(),'textOk':'ตกลงย้าย','textCancel':'ยังไม่ย้าย'},function(r){
                            if(r){                                 
                                var newNumber = r.split('  ');
                                var allNumber = '|';
                                spanPrepend = $(currentDrop).children('span').first();                                      
                                                                                                
                                $(currentDrop).children('span').each(function(){
                                    var cur = $(this).html().split('  ');                                    
                                    var curNumber = cur[1].substring(0,2);
                                    allNumber += $.trim(curNumber) + '|';                                   
                                                                                                      
                                    if(r > parseInt(curNumber)){                                                                            
                                        spanPrepend = $(this);                                       
                                        isSort = true;
                                        if($(this).is($(currentDrop).children('span').last())){
                                            isSort = false;
                                        }
                                    }                                                                                                
                                });                                
                                if(allNumber.indexOf('|' + $.trim(r) + '|') != -1){
                                    apprise('เลขที่ซ้ำค่ะ !!');
                                }
                                else{
                                    lastNumber = 'เลขที่  ' + r;
                                    // append span เมื่อลากย้ายห้อง
                                    if(isSort == false){$('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>').appendTo($(currentDrop));}
                                    else if(isSort == true){$(spanPrepend).after($('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>'));}                                    
                                    else{$(spanPrepend).before($('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>'));}
                                    $('#'+id+'_change').addClass('spnNumberChange');
                                    $('#'+id+'_change').text(lastNumber + ' ย้ายออกจากห้อง ' + fromRoom) // set text span
                                    // add atribute number_change and changeType and Room
                                    $('#'+id+'_change').attr('number_change',lastNumber);
                                    $('#'+id+'_change').attr('changeType','1');
                                    $('#'+id+'_change').attr('room',toRoom);
                                    // เพิ่ม span นักเรียนที่ย้าย ไว้ที่ห้องปัจจุบันด้วย
                                    //var idParentChange = $('#' + id + '_change' + '.spnNumberChange').parent().attr('id');
                                    var idParentChange = $(currentDrop).attr('id')                       
                                    var idParentCurrent = idParentChange.replace('_change','');
                                    alert(idParentCurrent);
                                    $('#'+idParentCurrent).html('');                   
                                    $('#'+idParentCurrent).append($('#'+idParentChange).html()); 
                                    // ให้ span draggable ได้
                                    liveDraggable();  
                                    $('#' + id).draggable({ disabled: true }); // set disabled  
                                    ui.draggable.text(number + ' ย้ายไปห้อง ' + toRoom); // set text ตัวที่ลาก
                                 }                                
                             }
//                             else{
//                                lastNumber = 'เลขที่  '+ lastNumber;
//                                // append span เมื่อลากย้ายห้อง
//                                $('<span class="'+ id +'_g" number="' + number + '" id="' + id + '_change' + '" ></span>').appendTo($(currentDrop));
//                                $('#'+id+'_change').addClass('spnNumberChange');
//                                $('#'+id+'_change').text(lastNumber + ' ย้ายออกจากห้อง ' + fromRoom) // set text span
//                                // add atribute number_change and changeType and Room
//                                $('#'+id+'_change').attr('number_change',lastNumber);
//                                $('#'+id+'_change').attr('changeType','1');
//                                $('#'+id+'_change').attr('room',toRoom);
//                                // เพิ่ม span นักเรียนที่ย้าย ไว้ที่ห้องปัจจุบันด้วย
//                                var idParentChange = $('#' + id + '_change' + '.spnNumberChange').parent().attr('id');                        
//                                var idParentCurrent = idParentChange.replace('_change','');
//                                $('#'+idParentCurrent).html('');                   
//                                $('#'+idParentCurrent).append($('#'+idParentChange).html()); 
//                                // ให้ span draggable ได้
//                                liveDraggable();  
//                                $('#' + id).draggable({ disabled: true }); // set disabled  
//                                ui.draggable.text(number + ' ย้ายไปห้อง ' + toRoom); // set text ตัวที่ลาก
//                             }                                                         
                         });                 
                        }
            });
        }
        // set session remember add new room
        function setSessionNewRoom(newRoom) {
             $.ajax({ type: "POST",
                 url: "<%=ResolveUrl("~")%>ClassUpgrade/changeClass.aspx/setSessionNewRoom",
                 data : "{newRoom : '" + newRoom + "' }",
                 async:false,
                 contentType:"application/json;charset=utf-8",dataType:"json",
                 success:function(data){                       
                 },
                 error:function myfunction(request,status){
                    alert(status);
                 }
             });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--<asp:HiddenField ID="hdKeepValueChange" runat="server" Value="" />--%>
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <center>
                    <h2>
                        ย้ายห้องเรียนประจำ ปีการศึกษา 2556</h2>
                </center>
                <div class="divTabClass" runat="server" id="divAllClassInSchool">
                    <%--ส่วนแสดงชั้นเรียน--%>
                </div>
                <div style="width: 930px;">
                    <table>
                        <tr>
                            <td style="text-align: center;">
                                ห้องที่มีนักเรียนปัจจุบัน
                            </td>
                            <td>
                            </td>
                            <td style="text-align: center;">
                                ห้องที่ต้องการย้าย
                                <div class="form_settings" style="margin-top: 0px;">
                                    <input type="button" title="clickAdd" value="เพิ่มห้องเรียน" id="clickAdd" style="height: 35px;
                                        width: 120px; float: right; margin-right: 35px; position: relative; margin-bottom: 10px;"
                                        class="submit" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: White;">
                                <div id="accordion" class="divAccL">
                                    <%--<h3>
                    <a href='#'>ห้อง 3</a></h3>
                <div style="background-color: green;">
                    <span class='spnNumber'>เลขที่ 1</span> <span class='spnNumber'>เลขที่ 2</span>
                    <span class='spnNumber'>เลขที่ 3</span> <span class='spnNumber'>เลขที่ 4</span>
                </div>--%>
                                </div>
                            </td>
                            <td style="background-color: White;">
                                <img src="../Images/Activity/sq_br_next.png" />
                            </td>
                            <td style="background-color: White;">
                                <div id="accordion2" class="divAccR">
                                    <%--ส่วนแสดงห้องที่ต้องการย้าย--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 930px;">
                    <table>
                        <tr>
                            <td style="background-color: White;">
                                <div id="move" style="background-image: url('../images/upgradeClass/schoolbus.png');
                                    background-repeat: no-repeat; background-position: right;">
                                    <%--<img alt="ดูคะแนน" src="../images/upgradeClass/schoolbus.png" width="50px" height="50px" />--%>
                                    ย้ายโรงเรียน</div>
                            </td>
                            <td style="background-color: White;">
                                <div id="trash" style="background-image: url('../images/upgradeClass/logout.png');
                                    background-repeat: no-repeat; background-position: right;">
                                    <%--<img alt="ดูคะแนน" src="../images/upgradeClass/logout.png" width="50px" height="50px" />--%>
                                    ลาออก / ไล่ออก
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_settings">
                    <asp:Button ID="btnSave" runat="server" CssClass="submit" Text="บันทึก" Style="float: right;
                        width: 100px; position: relative;"></asp:Button></div>
                <br />
                <center>
                    <footer style="margin-top: 20px">
                            <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
                    </footer>
                </center>
            </div>
        </div>
    </div>
    <%--<div class="divBackToStep1">
        </div>--%><div id="dialog" title="ใส่เลขห้องที่ต้องการ">
            <input type="text" id="newRoom" />
        </div>
    <%--<div id="dialog2" title="ต้องการบันทึกข้อมูลใช่หรือไม่ ?">
    </div>--%>
    </form>
</body>
</html>
