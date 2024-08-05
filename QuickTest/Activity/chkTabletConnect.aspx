<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="chkTabletConnect.aspx.vb"
    Inherits="QuickTest.chkTabletConnect" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: #FCFFEF;">
<head id="Head1" runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
       <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <link href="../css/jquery.qtip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         .ui-tooltip, .qtip
        {
            max-width: 800px;
        }
    </style>
    <script type="text/javascript">
        var student_id;
              

        $(document).ready(function () {

            //$('#divNotReady').qtip({
            //    content: 'กดเพื่อเปลี่ยนเครื่องสำรองงให้นักเรียน',
            //    show: { event: 'mouseover' },
            //    style: {
            //        width: 500, padding: 15, background: '#F68500', color: 'white', textAlign: 'center',
            //        border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topMiddle', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
            //    },
            //    position: { corner: { tooltip: 'topMiddle', target: 'bottomMiddle' }, adjust: { x: 0, y: -20 } },
            //    hide: { when: { event: 'mouseout' }, fixed: false }
            //});

            $('#mainDiv').qtip({
                content: 'กดเพื่อเปลี่ยนเครื่องสำรองให้นักเรียน',
                show: { ready: true },
                style: {
                    width: 500, padding: 5, background: '#F68500', color: 'white', textAlign: 'center',
                    border: { width: 7, radius: 5, color: '#F68500' }, tip: 'bottomLeft', name: 'dark', 'font-weight': 'bold', 'font-size': '25px', 'line-height': '1.5em'
                },hide:false,
                position: { corner: { tooltip: 'topLeft', target: 'topLeft' }, adjust: { x: 20, y: -70 } }//,
                //hide: { when: { event: 'mouseout' }, fixed: false }
            });

            setTimeout(function () {
                $('#mainDiv').qtip('destroy');
            }, 5000);

            //Settxt ตอนกดเปลี่ยน Tablet
            $.ajax({
                type: "POST",
                async: false,
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/GettxtCurrentTablet",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        $('#lbldefaultTablet').text(data.d);
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
        $.ajax({
            type: "POST",
            async: false,
            url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/GettxtTabletSpare",
            contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                    $('#lblchangeTablet').text(data.d);
                }
            },
            error: function myfunction(request, status) {
                alert(status);
            }
        });

        // เริ่มทำงาน
        getStudentId();
        getNoOfStudent();
        //SetDdlTabletName(idStudent);
        // get ค่าจำนวนนักเรียน
        function getNoOfStudent() {
            $.ajax({
                type: "POST",
                async: false,
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/getNoOfStudent",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    $('#lblHead').text(data.d);
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
                });
        }
        // สร้าง div สถานะนักเรียน            
        //            function creDiv(NoOfStudent) {            
        //                for (var i = 1; i <= NoOfStudent; i++) {
        //                    getStudentId(i);
        //                }
        //            }
        // get student_id มาเป็น id ของสถานะนักเรียน          
        function getStudentId() {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/getStudentID",
                    async: false,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            var Status_stu = jQuery.parseJSON(data.d);

                            for (i = 0; i < Status_stu.length; i++) {
                             
                                $('#SpnLabName').html(Status_stu[i].LabName);
                                $('#SpnDetail').html(Status_stu[i].LabDetailName);

                           
                                if (Status_stu[i].Player_Type == "1") {
                                    var newDiv = $("<div class='" + tabletIsOwner(Status_stu[i].Tablet_IsOwner, Status_stu[i].IsActive) + "' id='" + Status_stu[i].Player_Id + "' style='height:60px;width:90px;line-height:1em;'>ครู<span style='width: 50px;'>" + Status_stu[i].Tablet_TabletName + "</span></div>");
                                    //var newDiv = $("<div class='" + tabletIsOwner(Status_stu[i].Tablet_IsOwner, Status_stu[i].IsActive) + "' id='" + Status_stu[i].Student_CurrentNoInRoom + "' style='height:60px;width:90px;line-height:1em;'>ครู<span>" +  "</span></div>");
                                    $('#professorDiv').append(newDiv);

                                }
                                else if (Status_stu[i].Player_Type == "2") {

                                    var newDiv = $("<div class='" + tabletIsOwner(Status_stu[i].Tablet_IsOwner, Status_stu[i].IsActive) + "' id='" + Status_stu[i].Player_Id + "'>" + Status_stu[i].Student_CurrentNoInRoom + "<span style='width: 50px;'>" + Status_stu[i].Tablet_TabletName + "</span></div>");
                                    //var newDiv = $("<div class='" + tabletIsOwner(Status_stu[i].Tablet_IsOwner, Status_stu[i].IsActive) + "' id='" + Status_stu[i].Player_Id + "'>" + Status_stu[i].Student_CurrentNoInRoom + "<span>" + "</span></div>");
                                    $('#mainDiv').append(newDiv);

                                }
                            }

                            //                        for(i = 0 ;i<30;i++){
                            //                            var newDiv = $("<div class='divReady'><span>89</span></div>");Tablet_TabletName
                            //                                    $('#mainDiv').append(newDiv);
                            //                        }
                            //                                             
                            //                        var classDiv = "divNotReady"; 
                            //                         
                            //                        if(Status_stu.TabletIsActive == "True"){
                            //                            classDiv = "divReady";
                            //                        }
                            //                        else if(Status_stu.TabletIsActive == "False"){
                            //                            classDiv = "divChangeForReady";
                            //                        }

                            //var newDiv = $("<div class='" + classDiv + "' id='" + Status_stu.stuId + "'>" + number + "</div>");
                            //                        var newDiv = $("<div class='" + classDiv + "' id='" + number + "'>" + number + "</div>");
                            //                        $('#mainDiv').append(newDiv);

                            //sortNumber2Status();
                            var noOfUserReady = 0;
                            $('.mDiv').children().each(function () {
                                if ($(this).is('.divReady') || $(this).is('.divChangeForReady')) {
                                    noOfUserReady++;
                                }
                            });
                            //var noOfUserReady = $('.mDiv').children().length; 
                            $('#noOfUserReady').html(noOfUserReady + " คน");
                        }
                    },
                    error: function myfunction(request, status) {
                        alert(status);
                    }
                });
                }
        //check ว่า tablet ที่ใช้อยู่มีเจ้าของหรือเปล่า
        function tabletIsOwner(isOwner, IsActive) {
            //alert(isOwner);
            var classDiv = "divNotReady";

            if (isOwner == "1" && IsActive == '1') {
                classDiv = "divReady";
            }
            else if (isOwner == "0" && IsActive == '1') {
                classDiv = "divChangeForReady";
            }
            return classDiv;
        }

        // sort Div ตามสีตามลำดับเลข
        function sortNumber2Status() {
            $('.divReady').each(function () {
                $(this).remove();
                $(this).appendTo('#mainDiv');
                var numberStu = parseInt($(this).html());
                var oldPositionDiv = $(this);
                $('.divReady').each(function () {
                    var numberStuCurrent = parseInt($(this).html());
                    var currentPositionDiv = $(this);
                    if (numberStu > numberStuCurrent) {
                        $(oldPositionDiv).insertAfter($(currentPositionDiv));
                    }
                });
            });
        }
         //refresh สถานะการเชื่อมต่อทุกๆ 2 วินาที
        var refreshStatus = setInterval(function () {
            //            $('.divNotReady').remove();
            //            $('.divReady').remove();
            //            $('.divChangeForReady').remove();
            $('.mDiv').children().remove();
            $('#professorDiv').children().remove();
            getStudentId();

        }, 500);
        $.ajaxSetup({ cache: false });

        $('#dialog').dialog({
            autoOpen: false,
            buttons: { 'ใช่': function () { changeTablet(idStudent, $('#ddReserve').val(), $('#defaultTablet').attr('checked')); $(this).dialog('close'); }, 'ไม่': function () { $(this).dialog('close'); } },
            draggable: false,
            resizable: false,
            modal: true
        });
        $('#DialogNoTabletFull').dialog({
            autoOpen: false,
            buttons: { 'ตกลง': function () { $(this).dialog('close'); } },
            draggable: false,
            resizable: false,
            modal: true
        });
        $('#DialogNoTabletLab').dialog({
            autoOpen: false,
            buttons: { 'ตกลง': function () { $(this).dialog('close'); } },
            draggable: false,
            resizable: false,
            modal: true
        });
        
        $('#DialogChangeToPersonalLab').dialog({
            autoOpen: false,
            buttons: { 'ใช่': function () { changeTablet(idStudent, $('#ddReserve').val(), 'checked'); $(this).dialog('close'); }, 'ไม่': function () { $(this).dialog('close'); } },
            draggable: false,
            resizable: false,
            modal: true
        });
        $('#DialogChangeToPersonalFull').dialog({
            autoOpen: false,
            buttons: { 'ใช่': function () { changeTablet(idStudent, $('#ddReserve').val(), 'checked'); $(this).dialog('close'); }, 'ไม่': function () { $(this).dialog('close'); } },
            draggable: false,
            resizable: false,
            modal: true
        });
        var idStudent;
        $('div').live('click', function () {
            if ($(this).hasClass("divNotReady") || $(this).hasClass('divChangeForReady') || $(this).hasClass('divReady')) {
                if ($(this).parent().is("#professorDiv")) {
                   
                    $('#dialog').dialog({
                                    title: 'เปลี่ยนแท็บเล็ตของครู'
                                    //+ $(this).html()
                                    //title: TestFunction() + $(this).html()
                                });
                                $('#dialog').dialog('open');
                                idStudent = $(this).attr('id');
                     SetDdlTabletName(idStudent, 1);
                            
                } else {
                   
                $('#dialog').dialog({
                    title: 'เปลี่ยนแท็บเล็ตของนักเรียนเลขที่ '
                    //+ $(this).html()
                    //title: TestFunction() + $(this).html()
                });
                $('#dialog').dialog('open');
                  idStudent = $(this).attr('id');
                    SetDdlTabletName(idStudent, 2);
                
                }
            }
        });
        //var idTeacher;
        //$('div').live('click', function () {
        //    if ($(this).hasClass("divNotReady") || $(this).hasClass('divChangeForReady') || $(this).hasClass('divReady')) {
        //        $('#dialog').dialog({
        //            title: 'เปลี่ยนแท็บเล็ตของครู'
        //            //+ $(this).html()
        //            //title: TestFunction() + $(this).html()
        //        });
        //        $('#dialog').dialog('open');
        //        idTeacher = $(this).attr('id');
        //        SetDdlTabletName(idTeacher,1);
        //    }
        //});

        $('input[type=radio]').change(function () {
            if ($(this).attr('id') == 'defaultTablet') {
                $('#ddReserve').hide();
            }
            else {
                $('#ddReserve').show();
            }
        });

        function changeTablet(idStudent, selectVal, useMyTab) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/setChangeTablet",
                     data: "{ idStudent : '" + idStudent + "', selectValue : '" + selectVal + "', useMyTablet : '" + useMyTab + "' }",
                     async: false,
                     contentType: "application/json; charset=utf-8", dataType: "json",
                     success: function (data) {
                         //alert(data.d);           
                     },
                     error: function myfunction(request, status) {
                         alert(status);
                     }
                 });
                 }
    });

             function TestFunction() {
                 $.ajax({
                     type: "POST",
                     async: false,
                     url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/GettxtTitleChangeTablet",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        if (data.d == 'True') {
                            return 'เปลี่ยนโต๊ะ';
                        }
                        else {
                            return 'เปลี่ยนแท็บเล็ต';
                        }
                    }
                },
                error: function myfunction(request, status) {
                    alert(status);
                }
            });
             }

        function CheckEmptyDesk(idStudent) {
         
            $.ajax({
                type: "POST",
                async: false,
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/CheckEmptyDesk",
                data: "{ idStudent : '" + idStudent + "' }",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                    
                        if (data.d == 'True') {
                            $('#NotEmptyDesk').css("visibility", "hidden");
                        }
                        else {
                            $('#NotEmptyDesk').css("visibility", "visible");
                        }
                    }
                },
                error: function myfunction(request, status) {
                         alert(status);
                     }
                 });
             }

        function SetDdlTabletName(idStudent,Type) {
            $('#ddReserve').empty();
            var sel = document.getElementById('ddReserve');
            //while(sel.option.length > 0)

          
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Activity/chkTabletConnect.aspx/getDdl",
                data: "{ idStudent : '" + idStudent + "',Type : '" + Type + "' }",
                async: false,
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        var s = jQuery.parseJSON(data.d);
                        var SpareTabletName = s.detail;
                        
                        //var sel = document.getElementById('ddReserve');
                        for (var i = 1; i < SpareTabletName.length; i++) {
                            var opt = document.createElement('option');

                            opt.innerText = SpareTabletName[i];

                            sel.appendChild(opt);
                        }

                        if (SpareTabletName[0] == "1") {
                            $('#defaultTablet').prop('checked', true);
                            $('#changeTablet').prop('checked', false);
                            $('#changeTablet').attr('disabled', false);
                            CheckEmptyDesk(idStudent);
                        }
                        else if (SpareTabletName[0] == "0") {
                            $('#defaultTablet').prop('checked', false);
                            $('#changeTablet').prop('checked', true);
                            $('#changeTablet').attr('disabled', false);
                            $('#ddReserve').show();
                            CheckEmptyDesk(idStudent);
                        }
                        else if (SpareTabletName[0] == "2") { 
                            $('#dialog').dialog('close'); 
                            $('#DialogNoTabletLab').dialog('open');
                           // CheckEmptyDesk(idStudent);

                        }
                        else if (SpareTabletName[0] == "3") { 
                            $('#dialog').dialog('close');
                            $('#DialogNoTabletFull').dialog('open');
                            // CheckEmptyDesk(idStudent);

                        }
                        else if (SpareTabletName[0] == "4") {
                            $('#dialog').dialog('close');
                            $('#DialogChangeToPersonalLab').dialog('open');
                            // CheckEmptyDesk(idStudent);

                        }
                        else if (SpareTabletName[0] == "4") {
                            $('#dialog').dialog('close');
                            $('#DialogChangeToPersonalFull').dialog('open');
                            // CheckEmptyDesk(idStudent);

                        }
                    }
                }
            });
        }


    </script>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .mDiv {
            background-color: #FCFFEF;
            position: relative; /*left: 50px;*/
            top: 15px;
            width: 635px;
            height: 200px;
            font-size: 35px;
            text-align: center;
            float: left;
            margin-left: 9px;
            line-height: 1.5em;
        }

        .divNotReady {
            background-color: #F8FFF5;
            width: 50px;
            height: 50px;
            float: left;
            border: solid 1px #54E487;
            cursor: pointer;
            position: relative;
            -webkit-border-radius: 0.1em;
            margin: 5px;
            color: #424242;
            text-align: center;
        }

        .divReady {
            background-color: #25FA13;
            width: 50px;
            height: 50px;
            float: left;
            border: solid 1px #54E487;
            cursor: pointer;
            position: relative;
            -webkit-border-radius: 0.1em;
            margin: 5px;
            color: #424242;
            text-align: center;
        }

        .divReady span {
            position: absolute;
            font: bolder 15px THSarabunNew;
            left: 0px;
            bottom: -10px;
            color: orangered;
        }

        .divChangeForReady {
            /*background-color: orange; background-image: -webkit-gradient(linear,left bottom, right top,color-stop(0.17,rgb(255,255,255)),color-stop(0.35,rgb(45,196,45)),color-stop(0.20,rgb(45,196,45)));
            background-image: -webkit-gradient(linear,left bottom, right top,color-stop(0.20,rgb(255,255,255)),color-stop(0.40,#25FA13),color-stop(0.50,#25FA13));*/
            background-image: url('../Images/Activity/ReplacementBadge.png');
            width: 50px;
            height: 50px;
            float: left;
            border: solid 1px #54E487;
            cursor: pointer;
            position: relative;
            -webkit-border-radius: 0.1em;
            margin: 5px;
            color: #8B8B8B;
            background-color: #25FA13;
        }

        .divNotReady span {
            position: absolute;
            font: bolder 15px THSarabunNew;
            left: 0px;
            bottom: -10px;
            color: orangered;
        }

        .divChangeForReady span {
            position: absolute;
            font: bolder 15px THSarabunNew;
            left: 0px;
            bottom: -10px;
            color: orangered;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="text-align: center; background-color: #FCFFEF;">
        <asp:HiddenField ID="StudentChangTablet" runat="server" />
        <div style="width: 650px; margin-left: auto; margin-right: auto;">
            <div style="float: left; position: relative; margin-left: 20px">
                <span style="font: normal 24px THSarabunNew; color: #F68500;">ลงชื่อแล้ว <span id="noOfUserReady"
                    style="font: normal 24px THSarabunNew; color: #F68500;">0 </span></span>
                <br />
                <span id="lblHead" runat="server" style="font: normal 16px THSarabunNew; color: #F68500; float: left;"></span>
            </div>
            <div style="position: relative; top: 10px; text-align: center; cursor: pointer; font-size: 50px; width: 31px; height: 31px; float: left; margin-left: 20px; background-image: url('../Images/Activity/ajax_loader2.gif');">
            </div>
            <div id="professorDiv" style="position: relative; top: 10px; text-align: center; cursor: pointer; font-size: 50px; float: right; margin-right: 20px;">
            </div>
            <div id="mainDiv" class="mDiv">           
            </div>

            <div id="dialog" title="เปลี่ยนแท็บเล็ต">

                <input type="radio" id="defaultTablet" name="tablet" checked="checked" />
                <label for="defaultTablet" id="lbldefaultTablet"></label>
                <span id="NotEmptyDesk"style="font: normal 18px THSarabunNew; color: #F68500; visibility:hidden;" >โต๊ะเดิมไม่ว่างแล้วนะคะ</span>
                <br />
                <input type="radio" id="changeTablet" name="tablet" />
                <label for="defaultTablet" id="lblchangeTablet"></label>
                <asp:DropDownList runat="server" ID="ddReserve" Width="265px" Style="display: none;">
                </asp:DropDownList>


            </div>
             <div id="DialogNoTabletFull" title="ไม่มีเครื่องสำรองว่างแล้วค่ะ">
             
            </div>
            <div id="DialogNoTabletLab" title="ไม่มีโต๊ะสำรองว่างแล้วค่ะ">
             
            </div>
            <div id="DialogChangeToPersonalFull" title="ไม่มีเครื่องสำรองว่างแล้วค่ะ เปลี่ยนกลับไปใช้เครื่องส่วนตัวของนักเรียนมั้ยคะ">
             
            </div>
            <div id="DialogChangeToPersonalLab" title="ไม่มีโต๊ะสำรองว่างแล้วค่ะ เปลี่ยนกลับไปใช้โตีะตรงเลขที่ของนักเรียนมั้ยคะ">
             
            </div>
            <div style="position:absolute; margin-top : 500px;">

               <span id="SpnLabName"style="font: normal 30px THSarabunNew; color: #F68500; font-weight: bold">&nbsp;</span><span id="SpnDetail" style="font: normal 16px THSarabunNew; color: #F68500; margin-left:50px;">      ให้นักเรียนนั่งตามเลขที่ตรงกับเลขโต๊ะค่ะ</span>
               
            </div>
        </div>

    </form>
</body>
</html>
