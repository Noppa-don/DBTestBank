<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="viewReportPersonal.aspx.vb"
    Inherits="QuickTest.viewReportPersonal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<%@ Register Src="~/UserControl/SelectTermControl.ascx" TagName="UserControl" TagPrefix="myTerm" %>--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/highcharts.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/grid.js" type="text/javascript"></script>
    <link href="../css/styleViewReport.css" rel="stylesheet" type="text/css" />
    <script src="../js/json2.js" type="text/javascript"></script>
    <%--<script src="../Scripts/jquery.signalR-1.0.0.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
    <%--<script type="text/javascript">
        var Unload,CurrentPage,SignalRCheck;
        var withOutClick = true;
        var changePage = false;
        var firstClick = true;       

        $(window).bind('beforeunload', function () {
            if (withOutClick == true) {
                setUnload(true); // unload = true
            }
        });

        var groupname = '<%=GroupName %>';

        var thisPage = window.location.pathname;
        thisPage = thisPage.toLowerCase(); 
        var nextPage = '<%=ResolveUrl("~")%>viewreport/viewreportmain.aspx';
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
                window.location = CurrentPage;
            }
        }

        SignalRCheck = $.connection.hubSignalR;

        SignalRCheck.client.send = function (message) {
            if (message == thisPage) {
            }
            else {                
                withOutClick = false;
                window.location = message;
            }
        };

        SignalRCheck.client.raiseEvent = function (cmd) {
            if (cmd == nextPage) {
                firstClick = false;
                $('#imgReportRoom').trigger('click');
            }
            else {
                firstClick = false;
                $('#imgHome').trigger('click');
            }
        };

        $.connection.hub.start().done(function () {
            SignalRCheck.server.addToGroup(groupname);

            $('#imgReportRoom').click(function () {
                withOutClick = false;
                if (firstClick) {
                    setCurrentPage(nextPage);
                    SignalRCheck.server.sendCommand(groupname, nextPage);
                } else {
                    window.location = nextPage;
                }               
            });

            $('#imgHome').click(function(){
                withOutClick = false;                
                if (firstClick) {
                    setCurrentPage(step1Page);
                    SignalRCheck.server.sendCommand(groupname, step1Page);
                } else {
                    window.location = step1Page;
                }               
            });

            if (changePage) {//var today = new Date();
                SignalRCheck.server.sendCommand(groupname, thisPage);
            }

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
    </script>--%>
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');
    </script>
    <script type="text/javascript">
        var t360SchoolId = '<%=T360SchoolId %>';
        $(function () {

            if (t360SchoolId !== "") {
                $("footer").hide();
                $("#Help").hide();
                $("body").css("background", "#D3F2F7");
                $(".mainDiv").css("border", "initial")
                $(".mainDiv").css("top", "initial")
            }

            //ChartOnLoad();
        });


        //        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        //        function ChartOnLoad() {
        //            chart = new Highcharts.Chart({
        //                chart: {
        //                    renderTo: 'DivReport',
        //                    type: 'column'
        //                },
        //                title: {
        //                    text: '<b>คะแนนเฉลี่ยแต่ละวิชา</b>'
        //                },
        //                subtitle: {
        //                    text: 'ทุกปีการศึกษา ของเลขที่ 14 ม.5/3 ปีการศึกษา 2555'
        //                },
        //                xAxis: {
        //                    categories: ['ม.1/3<br />เลขที่ 20', 'ม.2/3<br />เลขที่ 20', 'ม.3/3<br />เลขที่ 20', 'ม.4/1<br />เลขที่ 9', 'ม.5/3<br />เลขที่ 14', ]
        //                },
        //                yAxis: {
        //                    title: {
        //                        text: 'คะแนนเฉลี่ย'
        //                    }
        //                },
        //                plotOptions: {
        //                    column: {
        //                        stacking: 'normal',
        //                        dataLabels: {
        //                            enabled: true
        //                        }
        //                    },
        //                    dataLabels: {
        //                        enable: true
        //                    }
        //                },
        //                series: [{
        //                    name: 'ไทย',
        //                    data: [20, 14, 16, 30, 5]
        //                }, {
        //                    name: 'อังกฤษ',
        //                    data: [20, 19, 17, 10, 5]
        //                }, {
        //                    name: 'สังคม',
        //                    data: [20, 21, 20, 15, 40]
        //                }, {
        //                    name: 'วิทย์',
        //                    data: [15, 12, 12, 25, 20]
        //                }, {
        //                    name: 'เลข',
        //                    data: [10, 7, 7, 5, 11]
        //                }, {
        //                    name: 'สุขศึกษา',
        //                    data: [10, 18, 18, 3, 5]
        //                }]

        //            });
        //        }
        //        //--------------------------------------------------------------------------------------------------------------------------------------
        //        function imgColumn() {
        //            ChartOnLoad();
        //        }

        //        //--------------------------------------------------------------------------------------------------------------------------------------

        //        function imgLine() {

        //            chart = new Highcharts.Chart({
        //                chart: {
        //                    renderTo: 'DivReport',
        //                    type: 'line'
        //                },
        //                title: {
        //                    text: '<b>คะแนนเฉลี่ยแต่ละวิชา</b>'
        //                },
        //                subtitle: {
        //                    text: 'ทุกปีการศึกษา ของเลขที่ 14 ม.5/3 ปีการศึกษา 2555'
        //                },
        //                xAxis: {
        //                    categories: ['ม.1/3<br />เลขที่ 20', 'ม.2/3<br />เลขที่ 20', 'ม.3/3<br />เลขที่ 20', 'ม.4/1<br />เลขที่ 9', 'ม.5/3<br />เลขที่ 14', ]
        //                },
        //                yAxis: {
        //                    title: {
        //                        text: 'คะแนนเฉลี่ย'
        //                    }
        //                },
        //                plotOptions: {
        //                    column: {
        //                        stacking: 'normal',
        //                        dataLabels: {
        //                            enabled: true
        //                        }
        //                    },
        //                    dataLabels: {
        //                        enable: true
        //                    }
        //                },
        //                series: [{
        //                    name: 'ไทย',
        //                    data: [20, 14, 16, 30, 5]
        //                }, {
        //                    name: 'อังกฤษ',
        //                    data: [20, 19, 17, 10, 5]
        //                }, {
        //                    name: 'สังคม',
        //                    data: [20, 21, 20, 15, 40]
        //                }, {
        //                    name: 'วิทย์',
        //                    data: [15, 12, 12, 25, 20]
        //                }, {
        //                    name: 'เลข',
        //                    data: [10, 7, 7, 5, 11]
        //                }, {
        //                    name: 'สุขศึกษา',
        //                    data: [10, 18, 18, 3, 5]
        //                }]

        //            });


        //        }
      
    </script>
    <script type="text/javascript">

     var JSChooseMode = '<%=ChooseMode %>';

        $(document).ready(function () {

        if (JSChooseMode == 1) {
            $('#imgHome').click(function(){
                window.location = '<%=ResolveUrl("~")%>Quiz/DashboardQuizPage.aspx'
            });
        }
        else if (JSChooseMode == 2) {
            $('#imgHome').click(function(){
                window.location = '<%=ResolveUrl("~")%>Homework/DashboardHomeworkPage.aspx'
            });
        }
        else if (JSChooseMode == 3) {
            $('#imgHome').click(function(){
                window.location = '<%=ResolveUrl("~")%>Practice/DashboardPracticePage.aspx'
            });
        }

        $('#imgReportRoom').click(function(){
          window.location = '<%=ResolveUrl("~")%>ViewReport/viewReportMain.aspx?DashboardMode=' + JSChooseMode;
        });

        $('#SwapLineChart').click(function(){
        ChangeChartLineRightPanel();
        });

        $('#SwapChart').click(function(){
        ChangeChartColumnRightPanel();
        });

        $('#imgTop10').click(function(){
        CreateChartTopLowOnlyRoomInPersonalMode('1');
        });

        $('#imgLow10').click(function(){
         CreateChartTopLowOnlyRoomInPersonalMode('0');
        });

           $('#imgBack').click(function () {
                window.location.href = '/Viewreport/ViewReportMain.aspx';
            });

            if ($('#HdCheckPostBack').val() == 'False') {
            $('#HdTypeChart').val('1');
            $('#HdTopLow').val('0');
             CreateChartMainPersonal(); //1
                
             }

            $('#btnClass').click(function () {
                var heightD = calPositionMenu('#btnClass','#menuShowClass');
                $('#menuShowClass').show().css('top',heightD + 'px');  
            });

            $('#btnPerson').click(function(){
                var heightD = calPositionMenu('#btnPerson','#menuShowStudentInRoom');
                $('#menuShowStudentInRoom').show().css('top',heightD + 'px');                
            });

        });

        function createStudentInRoomSelected(room) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>ViewReport/ViewReportPersonal.aspx/createStudentInRoom",
                  data:"{ room : '" + room + "',ChooseMode : '" + JSChooseMode + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {
                       var studentInRoom = data.d;
                       $('#menuShowStudentInRoom').html(studentInRoom);     
                  },
                  error: function myfunction(request, status) {   
                        //alert(status);                      
                  }
                });
        }

        // คำนวนให้แสดง div menu อยู่ตรงกลางปุ่ม
        function calPositionMenu(btn,divMenu){
            var position = $(btn).position();                
                var topBtn = position.top + 20;
                var divHeight = $(divMenu).css('height');
                divHeight = divHeight.replace('px','');
                var heightDiv = parseInt(divHeight / 2);
                var h = topBtn - heightDiv;
                return h;
        }


        
        function CreateChartMainPersonal() {
            OpenBlockUI();
             //var ValPracticeMode = $('#HdPracticeMode').val();
            var Data1 = <%=Session("SchoolID") %>
             var Data2 = $('#HdTypeChart').val();
                $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/viewReportPersonal.aspx/CreateChartMainPersonal",
	            data: "{ SchoolId: '" + Data1 + "',TypeChart: '" + Data2 + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    //alert(nextCmd);
                    if (nextCmd == '-1') {
                        $('#DivReportOut').hide();
                        $('#DivNoData').show();
                    }
                    else {
                        $('#DivNoData').hide();
                        $('#DivReportOut').show();
                        eval(nextCmd);
                    }
                    CloseBlockUI(); 
                       $('#HdRightPanel').val('1' + '$');              
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
        } //7


        function CreateChartOnlyRoomInPersonalMode(Data1) {
        OpenBlockUI();
         //var ValPracticeMode = $('#HdPracticeMode').val();
        var Data2 = $('#HdTypeChart').val();
        var InputSort = $('#HdTopLow').val();
        var ArrCheck = Data1.split(':');
        if (ArrCheck.length == 2) {
         Data1 = ArrCheck[0];
         $('#HdRoomName').val(Data1);
        }
       $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/viewReportPersonal.aspx/CreateChartOnlyRoomInPersonalMode",
	            data: "{ StrRoom: '" + Data1 + "',TypeChart: '" + Data2 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    //alert(nextCmd);
                    if (nextCmd == '-1') {
                        $('#DivReportOut').hide();
                        $('#DivNoData').show();
                    }
                    else {
                        $('#DivNoData').hide();
                        $('#DivReportOut').show();
                        eval(nextCmd);
                    }
                    CloseBlockUI();         
                      //$('#HdRightPanel').val($('#HdRightPanel').val() + '|' + '2' + '$' + classroom); 
                      $('#HdRightPanel').val('2' + '$' + Data1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
        }//8

        function CreateChartStudentOnly(Data1) {
           OpenBlockUI();
           // var ValPracticeMode = $('#HdPracticeMode').val();
           var OriginalData1 = Data1;
           var ArrCheck = Data1.split(',');
           var Data2;
           var Data3 = $('#HdTypeChart').val();
           if (ArrCheck.length == 1) {
            Data1 = $('#HdRoomName').val();
            Data2 = ArrCheck[0]; 
            }
            else
            {
            Data1 = ArrCheck[0];
            Data2 = ArrCheck[1];
            }

             $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/viewReportPersonal.aspx/CreateChartStudentOnly",
	            data: "{ StrRoom: '" + Data1 + "',NoInRoom : '" + Data2 + "',TypeChart: '" + Data3 + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    //alert(nextCmd);
                    if (nextCmd == '-1') {
                        $('#DivReportOut').hide();
                        $('#DivNoData').show();
                    }
                    else {
                        $('#DivNoData').hide();
                        $('#DivReportOut').show();
                        eval(nextCmd);
                    }
                    CloseBlockUI();          
                    //$('#HdRightPanel').val($('#HdRightPanel').val() + '|' + '3' + '$' + FullData);
                    $('#HdRightPanel').val('3' + '$' + OriginalData1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
        }//9

        function CreateChartTopLowOnlyRoomInPersonalMode(DataTopLow) {
           OpenBlockUI();
           //var ValPracticeMode = $('#HdPracticeMode').val();
            var Data1 = $('#HdRoomName').val();
            var Data2 = DataTopLow;
            var Data3 = $('#HdTypeChart').val();
                    $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/viewReportPersonal.aspx/CreateChartTopLowOnlyRoomInPersonalMode",
	            data: "{ StrRoom: '" + Data1 + "',TopOrLow : '" + Data2 + "',TypeChart:'" + Data3 + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (msg) {
                    nextCmd = msg.d;
                    if (nextCmd == '-1') {
                        $('#DivReportOut').hide();
                        $('#DivNoData').show();
                    }
                    else {
                        $('#DivNoData').hide();
                        $('#DivReportOut').show();
                        eval(nextCmd);
                    }
                    CloseBlockUI();          
                    //$('#HdRightPanel').val('3' + '$' + OriginalData1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
        }//10


        
        function ChangeChartColumnRightPanel() {
//            var Data1 = $('#HdRightPanel').val();
//            var ArrSplit = Data1.split('|');
//            var Data2 = ArrSplit[ArrSplit.length - 1];
//            var ArrData = Data2.split('$');
//            var State = ArrData[0];
              var Data1 = $('#HdRightPanel').val();
              var ArrData = Data1.split('$');
              var State = ArrData[0];
            if (State == '1') {
            $('#HdTypeChart').val('1');
            $('#HdTopLow').val('0');
                CreateChartMainPersonal();
            }
            else if (State == '2') {
            $('#HdTypeChart').val('1');
            $('#HdTopLow').val('0');
                var CompleteData1 = ArrData[1];
                CreateChartOnlyRoomInPersonalMode(CompleteData1);
            }
            else if (State == '3') {
            $('#HdTypeChart').val('1');
            $('#HdTopLow').val('0');
                var CompleteData1 = ArrData[1];
                CreateChartStudentOnly(CompleteData1);
            }
        }//ฟังก์ชั่นสลับเป็นกราฟแท่งทางขวา
        
        function ChangeChartLineRightPanel() {
//            var Data1 = $('#HdRightPanel').val();
//            var ArrSplit = Data1.split('|');
//            var Data2 = ArrSplit[ArrSplit.length - 1];
//            var ArrData = Data2.split('$');
//            var State = ArrData[0];
              var Data1 = $('#HdRightPanel').val();
              var ArrData = Data1.split('$');
              var State = ArrData[0];
            if (State == '1') {
            $('#HdTypeChart').val('2');
            $('#HdTopLow').val('0');
                CreateChartMainPersonal();
            }
            else if (State == '2') {
            $('#HdTypeChart').val('2');
            $('#HdTopLow').val('0');
                var CompleteData1 = ArrData[1];
                CreateChartOnlyRoomInPersonalMode(CompleteData1);
            }
            else if (State == '3') {
            $('#HdTypeChart').val('2');
            $('#HdTopLow').val('0');
                var CompleteData1 = ArrData[1];
                CreateChartStudentOnly(CompleteData1);
            }
        }//ฟังกชั่นสลับเป็นกราฟเส้นทางขวา

        function LoadPracticeMode() {
              var Data1 = $('#HdRightPanel').val();
              var ArrData = Data1.split('$');
              var State = ArrData[0];
              var Test =  $('#HdTypeChart').val();
              //alert(Test);
            if (State == '1') {
                CreateChartMainPersonal();
            }
            else if (State == '2') {
                var CompleteData1 = ArrData[1];
                CreateChartOnlyRoomInPersonalMode(CompleteData1);
            }
            else if (State == '3') {
                var CompleteData1 = ArrData[1];
                CreateChartStudentOnly(CompleteData1);
            }
        }//ฟังก์ชั่นสลับเมื่อกด เปิด - ปิด โหมดฝึกฝน







        function OpenBlockUI() {
            $.blockUI({
                message: '<h1><img src="../Images/prettyLoader/PackManOrange.gif" /><br /><br /><span style="color:#F85A1C;">รอสักครู่จ้า</span></h1>',
                css: {
                    border: 'none',
                    //padding:'15px',
                    backgroundColor: 'transparent'
                }
            });
        } //กาง BlockUI

        function CloseBlockUI() {
            $.unblockUI();
        }//ปิด BlockUI

    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            var selectedClassSame;
            var selectedRoomSame;
            var selectedQuizSame;

            // click on menu selected
            $('.divList').click(function () {
                // check ว่าเคยเลือกแล้วหรือยัง
                if ($(this).hasClass('divList selected')) {
                    //alert('if');
                    $(this).removeClass('selected');
                    $('.divList').removeClass('changeSelected');

                    // check ว่า div ที่กดเข้ามา คือเมนูตัวไหน
                    if ($(this).is('#classRoomSelected')) {
                        $('#menuShowClass').animate({ left: 45 }).hide(100);
                    }
                    else if ($(this).is('#noSelected')) {
                        $('#menuShowStudentInRoom').animate({ left: 45 }).hide(100);
                    }
                }
                else {
                    //alert('else');
                    $('.divList').removeClass('selected');
                    $('.divList').removeClass('changeSelected');
                    $(this).addClass('selected');

                    if ($(this).is('#classRoomSelected')) {
                        $('#menuShowStudentInRoom').animate({ left: 50 }).hide(100);
                        $('#menuShowClass').show(100).animate({ left: 245 });

                    }
                    else if ($(this).is('#noSelected')) {
                        $('#menuShowClass').animate({ left: 50 }).hide(100);
                        $('#menuShowStudentInRoom').show(100).animate({ left: 245 });
                    }
                }
            });
            // event ในการคลิกเลือกห้อง
            $('.classroom').click(function () {
                $('.classroom').each(function () {
                    $(this).attr('checked', false);
                });
                $(this).attr('checked', true);
                var classroom = $(this).attr('id');
                if (classroom != '') {
                    createStudentInRoomSelected(classroom);
                    $('#HdTypeChart').val('1');
                    $('#HdRoomName').val(classroom);
                    $('#HdTopLow').val('0');
                    CreateChartOnlyRoomInPersonalMode(classroom); //2
                    //$('#classRoomSelected').addClass('changeSelected');
                    $('#menuShowClass').animate({ left: 45 }).hide(100);
                    $('.divList').removeClass('selected');
                }
            });
            // event ในการคลิกเลือกนักเรียน
            $('.studentInRoom').live('click', function () {
                $('.studentInRoom').each(function () {
                    $(this).attr('checked', false);
                });
                $(this).attr('checked', true);
                var number = $(this).attr('id');
                number = number.replace('no', '');
                var FullData = $('#HdRoomName').val() + ',' + number;
                $('#HdTypeChart').val('1');
                $('#HdTopLow').val('0');
                CreateChartStudentOnly(FullData);
                //$('#noSelected').addClass('changeSelected');
                $('#menuShowStudentInRoom').animate({ left: 45 }).hide(100);
                $('.divList').removeClass('selected');
            });

            //$("#menuShowRoomSelected").live().alternateScroll();
            $('#practiceMode').click(function () {
                var practice = $('#HdPracticeMode').val();
                if (practice == "1") {
                    $('#HdPracticeMode').val('0');
                    $('#practiceMode').html('OFF');
                }
                else {
                    $('#HdPracticeMode').val('1');
                    $('#practiceMode').html('ON');
                }
                LoadPracticeMode(); //Function โหลดกราฟใหม่เมื่อกด เปิด - ปิดโหมดฝึกฝน
            });
        });

//            function SetSesstionCalendarId(CalendarId, CalendarName) {
//            $.ajax({
//                type: "POST",
//                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetSessionCalendarId",
//                data: "{ CalendarId: '" + CalendarId + "',CalendarName: '" + CalendarName + "'}",
//                contentType: "application/json; charset=utf-8", dataType: "json",
//                success: function (msg) {
//                    if (msg.d == 'Complete') {
//                        CloseFancyBox();
//                        window.location.reload();
//                    }
//                },
//                error: function myfunction(request, status) {
//                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
//                }
//            });            
//        }
//        function CloseFancyBox() {
//            $.fancybox.close();
//        }

    </script>
    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        button
        {
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
        .divList
        {
            background-color: #FFC76F;
            height: 50px;
            position: relative;
            padding: 15px;
            font: 18px 'THSarabunNew';
            line-height: 50px;
            font-weight: bold;
            cursor: pointer;
            background-image: url("../Images/reportGraph/ShowRightFrame-Orange.png");
            background-repeat: no-repeat;
            background-position: right;
            background-position-x: 175px;
        }
        .divList:hover
        {
            background-color: #FDA212;
            color: #F4F7FF;
            background-image: url("../Images/reportGraph/ShowRightFrame-White.png");
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        button
        {
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
        .selected
        {
            background-color: #FDA212;
            color: #F4F7FF;
            background-image: url("../Images/reportGraph/HideRightFrame-White.png");
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        .selected:hover
        {
            background-image: url("../Images/reportGraph/HideRightFrame-White.png");
        }
        .changeSelected
        {
            background-image: url("../Images/reportGraph/reloadBtn-White.png");
        }
        .changeSelected:hover
        {
            background-image: url("../Images/reportGraph/reloadBtn-White.png");
        }
        .graphSeleted
        {
            border: 1px solid white;
            height: 70px;
            width: 210px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 5px;
            -webkit-border-radius: 10px;
            -moz-border-radius: 10px;
            border-radius: 10px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        #menuShowClass, #menuShowStudentInRoom
        {
            position: absolute;
            left: 50px;
            top: 30px;
            overflow-y: auto;
            overflow-x: hidden;
            border: 2px solid #F8DDA3;
            background-color: #FFF1D3;
            display: none;
            cursor: pointer;
            z-index: 1;
            height: 497px;
            -webkit-border-radius: 0px 10px 10px 0px;
            -moz-border-radius: 0px 10px 10px 0px;
            border-radius: 0px 10px 10px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        .selectedTerm
        {
            margin-top:20px;
            }
            #DivNoData{
        border: 5px dotted;
        font-size: 70px;
        border-radius: 5px;
        border-color: orange;
        color: orange;
        position:relative;
        top:15px;
        width:675px;
        height:485px;
        background-color:#FCF2E5;
        display:none;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <%--<asp:HiddenField ID="hdPracticeMode" runat="server" Value="False" />--%>
    <div class="mainDiv" style='width: 990px; height: auto; margin-left: auto; margin-right: auto;
        position: relative; left: 0px; top: 25px;'>
     <%--   <myTerm:UserControl ID="MyCtlTerm" runat="server" />--%>
        <table id='Maintable' >
            <tr>
                <td>
                    <table id='LeftPanel'>
                        <tr>
                            <td>
                                <div class="divMenuLeft" style='position: relative; left: 6px; z-index: 2;'>
                                   <%-- <img src="../Images/reportGraph/btnHome.png" style='display:none;margin-top: 20px; margin-left: 10px;
                                        float: left;' id="imgHome" title='จัดชุดข้อสอบใหม่' />--%>
                                    <%--<img src="../Images/reportGraph/btnBack.png" style='margin-top: 20px; margin-left: 20px;'
                                        id="imgBack" title='กลับไปยังหน้าที่แล้ว' /><br />--%>
                               <%--     <div id="practiceMode" style="width: 60px; height: 60px; margin-top: 20px; text-align: center;
                                        float: left; margin-left: 10px; cursor: pointer; border: 1px solid;">
                                        OFF</div>--%>
                                    <img src="../Images/reportGraph/btnViewGroup.png" id="imgReportRoom" style='margin-top: 20px;'
                                        title='ดูคะแนนแบบรายห้อง' /><br />
                                    <br />
                                    <%--<div class="divClass" style='margin-top: -32px; height: 270px;'>
                                        <img src="../Images/freehand.png" />
                                        <button type="button" id="btnClass" style="text-align: center; font: normal 100% 'THSarabunNew';">
                                            ชั้น / ห้อง
                                        </button>
                                        <div class="divClassRoom" style="background-color: White; height: 200px">
                                            <img src="../Images/freehand.png" />
                                            <button type="button" id="btnPerson" style="text-align: center; font: normal 100% 'THSarabunNew';">
                                                เลขที่
                                            </button>
                                        </div>
                                    </div>--%>
                                    <div style="position: relative; background-color: #FFC76F;">
                                        <div class="divList" id="classRoomSelected" title='เลือกชั้นและห้อง เพื่อดูคะแนนค่ะ'>
                                            ชั้น / ห้อง
                                        </div>
                                        <div class="divList" id="noSelected" title='เลือกเลขที่ เพื่อดูคะแนนค่ะ'>
                                            เลขที่
                                        </div>
                                    </div>
                                    <br />
                                    <div class='graphSeleted'>
                                        <img id='imgTop10' src="../Images/reportGraph/btnTop10.png" style='cursor: pointer;
                                            margin-left: 6px; margin-top: 4px; float: left;' onclick='imgTop10()' title="ดูคะแนนสูงสุด 10 อันดับ" /><%--<span style='font-size: 12px;
                                                    cursor: pointer; position: relative; top: -30px;' onclick='imgTop10()'><b>สูงสุด 10</b></span>--%>
                                        <img id='imgLow10' src="../Images/reportGraph/btnBottom10.png" style='cursor: pointer;
                                            position: relative; float: left; margin-top: 4px;' onclick='imgLow10()' title="คะแนนต่ำสุด 10 อันดับ" /><%--<span style='font-size: 12px;
                                                    cursor: pointer; position: relative; top: -35px' onclick='imgLow10()'><b>ต่ำสุด 10</b></span>--%></div>
                                    <div class='graphSeleted' style="margin-top: 12px;">
                                        <img id='SwapChart' src="../Images/reportGraph/btnChartBar.png" style='cursor: pointer;
                                            margin-left: 78px; margin-top: 12px; float: left; position: relative; width: 50px;'
                                            title="คะแนนแบบกราฟแท่ง" />
                                        <img id='SwapLineChart' src="../Images/reportGraph/btnChartLine.png" style='cursor: pointer;
                                            width: 50px; margin-left: 35px; margin-top: 12px; float: left; position: relative;display:none;'
                                            title="คะแนนแบบกราฟเส้น" />
                                        <%--<img id='SwapPieChart' src="../Images/reportGraph/btnChartPie.png" style='cursor: pointer;
                                            width: 50px; margin: 10px; float: left; position: relative;' title="คะแนนแบบกราฟวงกลม" />--%></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <div id="DivReportOut" style='width: 734px; overflow-y: auto; height: 528px;'>
                        <div id='DivReport' style='width: 730px; height: 480px; left: 4px; position: relative;
                            top: 30px; z-index: 0;'>
                        </div>
                    </div>
                    <div id="DivNoData">
                        <span style="line-height:480px;">ไม่พบข้อมูล</span>
                    </div>
                </td>
                <%--<td>
                    <div id='DivRightPanel' style='position: relative; right: 12px; height: 500px; top: 13px;
                        background-color: #FFC76F; -webkit-border-radius: 0.5em; box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.7);'>
                        <div id='SwapChart'>
                            <img src="../Images/reportGraph/barchart.png" onclick='imgColumn()' style='cursor: pointer;
                                margin-top: 110px;' title='คะแนนแบบกราฟแท่ง' /></div>
                        <div id='SwapLineChart'>
                            <img src="../Images/reportGraph/linechart.png" style='cursor: pointer' onclick='imgLine()'
                                title='คะแนนแบบกราฟเส้น' /></div>
                    </div>
                </td>--%>
            </tr>
        </table>
        <div id="menuShowClass" runat="server">
        </div>
        <div id="menuShowStudentInRoom" runat="server">
        </div>
        <center>
            <footer style="position: relative; top: 11px;">
                            <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
                    </footer>
            <br />
        </center>
    </div>
    <input type="hidden" id='HdCheckPostBack' value="<%=Page.IsPostBack.Tostring() %>" />
    <input type='hidden' id='HdRoomName' />
    <input type="hidden" id='HdRightPanel' />
    <input type="hidden" id='HdTypeChart' />
    <input type="hidden" id='HdTopLow' />
    <input type="hidden" id='HdPracticeMode' value="0" />
    </form>
</body>
<%--<script type="text/javascript">
        //script ดักอีกที เมื่อโหลดเพจเสร็จ
        var CurrentPage;
        $(function () {         
            var thisPage = window.location.pathname;
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
</script>--%>
</html>
