<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ViewReportReduce.aspx.vb"
    Inherits="QuickTest.ViewReportReduce" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControl/SelectTermControl.ascx" TagName="UserControl" TagPrefix="myTerm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/highcharts.js" type="text/javascript"></script>
    <script src="../js/grid.js" type="text/javascript"></script>
    <%--<script src="../js/exporting.js" type="text/javascript"></script>--%>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <link href="../css/styleViewReport.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
   <%-- <script src="../Scripts/jquery.signalR-1.0.0.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
<%--    <script type="text/javascript">
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
        var nextPage = '<%=ResolveUrl("~")%>viewreport/viewreportpersonal.aspx';
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
                $('#imgReportPersonal').trigger('click');
            }
            else {
                firstClick = false;
                $('#imgHome').trigger('click');
            }
        };

        $.connection.hub.start().done(function () {
            SignalRCheck.server.addToGroup(groupname);

            $('#imgReportPersonal').click(function () {
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
        //       $(function () {

        //            var colors = Highcharts.getOptions().colors,
        //        categories = ['ไทย', 'อังกฤษ', 'วิทย์', 'สังคม', 'เลข'],
        //        name = 'ปริมาณการจัดชุดกิจกรรม',
        //        data = [{
        //            y: 30,
        //            color: colors[0],
        //            drilldown: {
        //                name: 'ไทย',
        //                categories: ['ม.2', 'ม.4', 'ม.6'],
        //                data: [10, 10, 10],
        //                color: colors[0]
        //            }
        //        }, {
        //            y: 20,
        //            color: colors[1],
        //            drilldown: {
        //                name: 'อังกฤษ',
        //                categories: ['ม.1', 'ม.5'],
        //                data: [15, 5],
        //                color: colors[1]
        //            }
        //        }, {
        //            y: 20,
        //            color: colors[2],
        //            drilldown: {
        //                name: 'วิทย์',
        //                categories: ['ม.1', 'ม.3', 'ม.4', 'ม.6'],
        //                data: [5, 5, 5, 5],
        //                color: colors[2]
        //            }
        //        }, {
        //            y: 20,
        //            color: colors[3],
        //            drilldown: {
        //                name: 'วิทย์',
        //                categories: ['ม.1', 'ม.3', 'ม.4', 'ม.6'],
        //                data: [5, 5, 5, 5],
        //                color: colors[3]
        //            }
        //        }, {
        //            y: 10,
        //            color: colors[4],
        //            drilldown: {
        //                name: 'วิทย์',
        //                categories: ['ม.1', 'ม.3'],
        //                data: [5, 5],
        //                color: colors[4]
        //            }
        //        }];

        //            var TotalData = [];
        //            var Classdata = [];
        //            for (var i = 0; i < data.length; i++) {
        //                TotalData.push({
        //                    name: categories[i],
        //                    y: data[i].y,
        //                    color: data[i].color
        //                });

        //                for (var j = 0; j < data[i].drilldown.data.length; j++) {
        //                    var brighhness = 0.2 - (j / data[i].drilldown.data.length) / 5;
        //                    Classdata.push({
        //                        name: data[i].drilldown.categories[j],
        //                        y: data[i].drilldown.data[j],
        //                        color: Highcharts.Color(data[i].color).brighten(brighhness).get()
        //                    });
        //                }
        //            }

        //            chart = new Highcharts.Chart({
        //                chart: {
        //                    renderTo: 'DivReport',
        //                    type: 'pie'
        //                },
        //                title: {
        //                    text: 'ปริมาณการจัดชุด'
        //                },
        //                plotOptions: {
        //                    pie: {
        //                        shadow: true
        //                    }
        //                },
        //                tooltip: {
        //                    formatter: function () {
        //                        return '<b>' + this.point.name + '</b>: ' + this.y + '%';
        //                    }
        //                },
        //                series: [{
        //                    name: 'วิชา',
        //                    data: TotalData,
        //                    size: '60%',
        //                    dataLabels: {
        //                        formatter: function () {
        //                            return this.y > 5 ? this.point.name : null;
        //                        },
        //                        color: 'white',
        //                        distance: -30
        //                    }
        //                }, {
        //                    name: 'ห้อง',
        //                    data: Classdata,
        //                    innerSize: '60%',
        //                    dataLabels: {
        //                        formatter: function () {
        //                            return this.y > 1 ? '<b>' + this.point.name + ':</b>' + this.y + '%' : null;
        //                        }
        //                    }
        //                }
        //                ]

        //            });


        //        });


    </script>
    <script type="text/javascript">

    var JSChooseMode = '<%=ChooseMode %>';
     var t360SchoolId = '<%=T360SchoolId %>';

        $(function () {
             
               if (t360SchoolId !== "") {
            $("footer").hide();
            $("#Help").hide();
            $("body").css("background","#D3F2F7");
            $(".mainDiv").css("border","initial")
            $(".mainDiv").css("top","initial")
        }

                if (JSChooseMode == 1) {
            $('#imgHome').click(function(){
               if (t360SchoolId !== "") {
           window.location = '<%=ResolveUrl("~")%>ViewReport/ViewReportMain.aspx?DashboardMode=' + JSChooseMode;
           
            }else{
             window.location = '<%=ResolveUrl("~")%>Quiz/DashboardQuizPage.aspx'
            }
                
            });
        }
        else if (JSChooseMode == 2) {
            $('#imgHome').click(function(){
             if (t360SchoolId !== "") {
           window.location = '<%=ResolveUrl("~")%>ViewReport/ViewReportMain.aspx?DashboardMode=' + JSChooseMode;
           
            }else{
               window.location = '<%=ResolveUrl("~")%>Homework/DashboardHomeworkPage.aspx'
            }
                
             
            });
        }
        else if (JSChooseMode == 3) {
            $('#imgHome').click(function(){
              
                  if (t360SchoolId !== "") {
           window.location = '<%=ResolveUrl("~")%>ViewReport/ViewReportMain.aspx?DashboardMode=' + JSChooseMode;
           
            }else{
                window.location = '<%=ResolveUrl("~")%>Practice/DashboardPracticePage.aspx'
            }
            });
        }

        $('#Help a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#Help > li').hover(
            function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
            },
                function () {
                    $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
                }
                );

        $('#Help').click(function(){
            window.location = '<%=ResolveUrl("~")%>ViewReport/ViewReportMain.aspx?DashboardMode=' + JSChooseMode;
        });


        ////////////////////
//     Highcharts.getOptions().colors =
//          $.map(Highcharts.getOptions().colors, function (color) {
//              return {
//                  radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
//                  stops: [
//          [0, color],
//          [1,
//          Highcharts.Color(color).brighten(-0.5).get('rgb')]
//          ]
//              };
//          });
        ////////////////////

        //Test ตัวแปร PracticeMode 
        $('#HdPracticeMode').val('0');
        $('#TestPracticeMode').click(function(){
        $('#HdPracticeMode').val('1');
        LoadStatePracticeMode()
        });


        //////////////////////////////////////

        if ($('#HdCheckPostBack').val() == 'False') {
            CreatePieChartOnLoad();
        }

        $('#SwapLineChart').click(function(){
        //alert('sdfsdf');
        ChangeChartLineRightPanel();
        });

        $('#SwapChart').click(function(){
        ChangeChartColumnRightPanel();
        });

        
        $('#SwapPieChart').click(function(){
        ChangeChartPieRightPanel();
        });

        $('#imgTop10').click(function(){
        ChageChartTopOrLow('1');
        });

        $('#imgLow10').click(function(){
        ChageChartTopOrLow('2');
        });



        //            $('#imgBack').remove();
//            $('#imgBack').clcik(function () {
//                window.location.href = '/TestSet/step1.aspx';
//            });

            //disabledCheckboxClass(); // เรียกฟังชั่น disabled checkbox
            // ปุ่มระดับชั้น
        $('#btnClass').toggle(function () {
            //alert('btnclass');
                var heightD = calPositionMenu('#btnClass','#menuShow');
                $('#menuShow').show().css('top',heightD + 'px');

            }, function () {
                $('#menuShow').hide();               
                //var getClass = getClassSelected();
                //alert(getClass);
                
                if (getClass != "") {
                    $('#divClassRoom').show();
                    $('.divClassRoom').show();
                    $('#btnClass').text('บางชั้น');
                    setRoomFromSelected(getClass);
                  
                    CreateChartMainLevel(getClass);
                }
                else {
                    $('#divClassRoom').hide();
                    $('.divClassRoom').hide();
                    $('#btnClass').text('ระดับชั้น');
                }                
            });
 

            // ปุ่มระดับห้อง
        $('#btnClassRoom').toggle(function () {
            //alert('btnclassroom');
                var heightD = calPositionMenu('#btnClassRoom','#menuShowRoomSelected');
                $('#menuShowRoomSelected').show().css('top',heightD + 'px');
                $('#menuShow').hide();
            }, function () {
                $('#menuShowRoomSelected').hide();                    
                    var getRoom = getRoomSelected();
                    //alert(getRoom);
                    if (getRoom != "") {
                        $('#divClassEvent').show();
                        $('.divClassEvent').show();
                        $('#btnClassRoom').text('บางห้อง');
                        setQuizFromSelected(getRoom);
                          $('#HdTypeChart').val('1');
                        //CreateChartClass(getRoom);
                    }
                    else {
                        $('#divClassEvent').hide();
                        $('.divClassEvent').hide();
                        $('#btnClassRoom').text('ระดับห้อง');
                    }
            });
            // ปุ่มเลือก quiz
            $('#btnClassEvent').toggle(function () {
                var heightD = calPositionMenu('#btnClassEvent','#menuShowQuizSelected');
                $('#menuShowQuizSelected').show().css('top',heightD + 'px');
            }, function () {
                $('#menuShowQuizSelected').hide();                    
                    var getQuiz = getQuizSelected();
                   // alert(getQuiz);
                    if (getQuiz != "") {
                        $('#divQuizSelected').show();                        
                        $('#btnClassEvent').text('บางควิซ');
                          $('#HdTypeChart').val('1');   
                        //CreateChartActivity(getRoomSelected(),getQuiz)                     
                    }
                    else {
                        $('#divQuizSelected').hide();                        
                        $('#btnClassEvent').text('ชุดควิซ');
                    }
            });
            $('#ป.1').click(function(){
                //alert('sdg');
            });
        });
        // แสดงชั้นเรียนที่ถูกเลือก
        function getClassSelected() {
            var classSelected = "";
            $('.primaryClass:checked').each(function () {
                var numberClass = $(this).next().html();
                classSelected += 'ป.' + numberClass + ',';
            });
            $('.secondClass:checked').each(function () {
                var numberClass = $(this).next().html();
                classSelected += 'ม.' + numberClass + ',';
            });
            classSelected = classSelected.substring(0, classSelected.length - 1);
            return classSelected;
        }
        // แสดงห้องเรียนที่ถูกเลือก
        function getRoomSelected(){
            var classSelected = "";
            $('.classRoom:checked').each(function () {
                var numberClass = $(this).attr('id');
                classSelected += numberClass + ',';
            });
            classSelected = classSelected.substring(0, classSelected.length - 1);
            return classSelected;
        }
        // แสดงควิซที่ถูกเลือก
        function getQuizSelected(){
            var classSelected = "";
            $('.quizName:checked').each(function () {
                var numberClass = $(this).attr('id');
                classSelected += numberClass + ',';
            });
            classSelected = classSelected.substring(0, classSelected.length - 1);
            return classSelected;
        }
        // สร้าง div ให้เลือกห้องตามชั้นที่เลือก
        function setRoomFromSelected(classSelected) {
             $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/createRoomFormSelectedClass",
                  data:"{ classSelected : '" + classSelected + "',ChooseMode:'" + JSChooseMode + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {
                    $('#menuShowRoomSelected').html(data.d);                                                
                  },
                  error: function myfunction(request, status) {   
                        //alert(status);                      
                  }
                });
        }
        // สร้าง div ให้เลือกควิซ
        function setQuizFromSelected(classRoomSelected) {
             $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/createQuizFromRoomSelected",
                  data:"{ classRoomSelected : '" + classRoomSelected + "',ChooseMode :'" + JSChooseMode + "' }",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {
                    $('#menuShowQuizSelected').html(data.d);                                                
                  },
                  error: function myfunction(request, status) {   
                        //alert(status);                      
                  }
                });
        }
        // disbled checkbox not in db
        function disabledCheckboxClass(){
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/createClassRoom",
                  data:"{ ChooseMode : '" + JSChooseMode + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {
                       var className = data.d;
                       className = className.split(',');
                       var AllclassName = "ป.1,ป.2,ป.3,ป.4,ป.5,ป.6,ม.1,ม.2,ม.3,ม.4,ม.5,ม.6";
                       for (i=0;i<className.length;i++){
                            var reStr = className[i].toString() + ",";
                            if(i == className.length -1){
                                var reStr = "," + className[i].toString();
                            }
                            AllclassName = AllclassName.replace(reStr,"");                            
                       }
                       var newAllClassName = AllclassName.split(','); 
                       
                       for(j=0;j<newAllClassName.length;j++){ 
                            var name = newAllClassName[j].replace('.','');                                            
                            $('#'+name).attr("disabled",true);//.next().css('color','red');
                       }                                                           
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
        //--------------------------------------------------------------------------------------------------------------------------------

        function CreatePieChartOnLoad() {
         OpenBlockUI();
         //var ValPracticeMode = $('#HdPracticeMode').val();
               $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreatePieChartOnLoad",
	            data: "{ ChooseMode: '" + JSChooseMode + "'}",  //" 
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
                    $('#HdRightPanel').val('0' + '$' + 'State0');
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
        }



          function CreateChartMainLevel(Data1) {
    OpenBlockUI();
    //var ValPracticeMode = $('#HdPracticeMode').val();
    var Data2 = $('#HdTypeChart').val();
     var InputSort = $('#HdTopOrLow').val();
       $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartMainLevel",
	            data: "{ StrLevel: '" + Data1 + "',TypeChart:'" + Data2 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                    $('#HdRightPanel').val('1' + '$' + Data1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });

} //1

        function CreateChartAllRoomInClassChoose(Data1) {
         OpenBlockUI();
           //var ValPracticeMode = $('#HdPracticeMode').val();
           var Data2 = $('#HdTypeChart').val();
              var InputSort = $('#HdTopOrLow').val();
         $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartAllRoomInClassChoose",
	            data: "{ StrChooseAllRoom: '" + Data1 + "',TypeChart:'" + Data2 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                    $('#HdRightPanel').val('2' + '$' + Data1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });

} //2

        function CreateChartClass(Data1) { 
         OpenBlockUI();
           //var ValPracticeMode = $('#HdPracticeMode').val();
           var Data2 = $('#HdTypeChart').val();
            var InputSort = $('#HdTopOrLow').val();
           $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartClass",
	            data: "{ StrClass: '" + Data1 + "',TypeChart:'" + Data2 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                     $('#HdRightPanel').val('3' + '$' + Data1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });

} //3
        
        function CreateChartRoom(Data1) {
         OpenBlockUI();
           //var ValPracticeMode = $('#HdPracticeMode').val();
          var Data2 = $('#HdTypeChart').val();
          var InputSort = $('#HdTopOrLow').val();
        $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartRoom",
	            data: "{ StrRoom: '" + Data1 + "',TypeChart:'" + Data2 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                     $('#HdRightPanel').val('4' + '$' + Data1);
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
} //4

        function CreateChartActivity(Data1,Data2) {
         OpenBlockUI();
         //var ValPracticeMode = $('#HdPracticeMode').val();
        var Data3 = $('#HdTypeChart').val();
        var InputSort = $('#HdTopOrLow').val();
        $("#TestSetNameHD").val(Data2);
       $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartActivity",
	            data: "{ StrClass: '" + Data1 + "',TestSet_Id: '" + Data2 + "',TypeChart:'" + Data3 + "',SortType: '" + InputSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                     $('#HdRightPanel').val('5' + '$' + Data1 + '|' + Data2);        
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });
}//5

        function CreateChartActivityPerQuiz(DataInput) {
         OpenBlockUI();
           //var ValPracticeMode = $('#HdPracticeMode').val();
         var OriginalData = DataInput;
          var Data3 = $('#HdTypeChart').val();
          var CheckSort =  $('#HdTopOrLow').val();
      var ArrData = DataInput.split(",");
      var Data1;
      var Data2;
                Data1 = ArrData[0];
                if (ValPracticeMode == '0') {
                 Data2 = $('#TestSetNameHD').val() + "," +  ArrData[1];
                }
                else if (ValPracticeMode == '1'){
                Data2 = $('#TestSetNameHD').val();
                }
               
           $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>ViewReport/ViewReportReduce.aspx/CreateChartActivityPerQuiz",
	            data: "{ StrClass: '" + Data1 + "', StrActivity: '" + Data2 + "' ,TypeChart:'" + Data3 + "',SortType: '" + CheckSort + "',ChooseMode:'" + JSChooseMode + "'}",  //" 
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
                     $('#HdRightPanel').val('6' + '$' + OriginalData) ;
	            },
	            error: function myfunction(request, status)  {
                //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
	                //alert('shin' + request.statusText + status);    
	            }
	        });

} //6

            //ฟังกชั่นสลับเป็นกราฟเส้นทางขวา
//            function ChangeChartLineRightPanel() {
//              var Data1 = $('#HdRightPanel').val();
//              var ArrData = Data1.split('$');
//              var State = ArrData[0];
//            if (State == '1') {
//            $('#HdTypeChart').val('2');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartMainLevel(CompleteData1);
//            }
//            else if (State == '2') {
//            $('#HdTypeChart').val('2');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartAllRoomInClassChoose(CompleteData1);
//            }
//            else if (State == '3') {
//            $('#HdTypeChart').val('2');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartClass(CompleteData1);
//            }
//            else if (State == '4') {
//            $('#HdTypeChart').val('2');
//             $('#HdTopOrLow').val('0');
//              var CompleteData1 = ArrData[1];
//            CreateChartRoom(CompleteData1);
//            }
//            else if (State == '5') {
//            $('#HdTypeChart').val('2');
//             $('#HdTopOrLow').val('0');
//            var SplitStr = ArrData[1].split('|');
//            var CompleteData1 = SplitStr[0];
//            var CompleteData2 = SplitStr[1];
//            CreateChartActivity(CompleteData1,CompleteData2,'0');
//            }
//            else if (State == '6') {
//         $('#HdTypeChart').val('2');
//          $('#HdTopOrLow').val('0');
//          var CompleteData1 = ArrData[1];
//            CreateChartActivityPerQuiz(CompleteData1,CompleteData2,'0');
//            }
//        }

                //ฟังกชั่นสลับเป็นกราฟแท่งทางขวา
//            function ChangeChartColumnRightPanel() {
//              var Data1 = $('#HdRightPanel').val();
//              var ArrData = Data1.split('$');
//              var State = ArrData[0];
//            if (State == '1') {
//            $('#HdTypeChart').val('1');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartMainLevel(CompleteData1);
//            }
//            else if (State == '2') {
//            $('#HdTypeChart').val('1');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartAllRoomInClassChoose(CompleteData1);
//            }
//            else if (State == '3') {
//            $('#HdTypeChart').val('1');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartClass(CompleteData1);
//            }
//            else if (State == '4') {
//            $('#HdTypeChart').val('1');
//             $('#HdTopOrLow').val('0');
//              var CompleteData1 = ArrData[1];
//            CreateChartRoom(CompleteData1);
//            }
//            else if (State == '5') {
//            $('#HdTypeChart').val('1');
//             $('#HdTopOrLow').val('0');
//            var SplitStr = ArrData[1].split('|');
//            var CompleteData1 = SplitStr[0];
//            var CompleteData2 = SplitStr[1];
//            CreateChartActivity(CompleteData1,CompleteData2,'0');
//            }
//            else if (State == '6') {
//         $('#HdTypeChart').val('1');
//          $('#HdTopOrLow').val('0');
//          var CompleteData1 = ArrData[1];
//            CreateChartActivityPerQuiz(CompleteData1,CompleteData2,'0');
//            }
//        }
        
          //ฟังกชั่นสลับเป็นกราฟวงกลมทางขวา
//         function ChangeChartPieRightPanel() {
//              var Data1 = $('#HdRightPanel').val();
//              var ArrData = Data1.split('$');
//              var State = ArrData[0];
//            if (State == '1') {
//            $('#HdTypeChart').val('3');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartMainLevel(CompleteData1);
//            }
//            else if (State == '2') {
//            $('#HdTypeChart').val('3');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartAllRoomInClassChoose(CompleteData1);
//            }
//            else if (State == '3') {
//            $('#HdTypeChart').val('3');
//             $('#HdTopOrLow').val('0');
//                var CompleteData1 = ArrData[1];
//                CreateChartClass(CompleteData1);
//            }
//            else if (State == '4') {
//            $('#HdTypeChart').val('3');
//             $('#HdTopOrLow').val('0');
//              var CompleteData1 = ArrData[1];
//            CreateChartRoom(CompleteData1);
//            }
//            else if (State == '5') {
//            $('#HdTypeChart').val('3');
//            var SplitStr = ArrData[1].split('|');
//            var CompleteData1 = SplitStr[0];
//            var CompleteData2 = SplitStr[1];
//             $('#HdTopOrLow').val('0')
//            CreateChartActivity(CompleteData1,CompleteData2);
//            }
//            else if (State == '6') {
//         $('#HdTypeChart').val('3');
//          $('#HdTopOrLow').val('0');
//          var CompleteData1 = ArrData[1];
//            CreateChartActivityPerQuiz(CompleteData1,CompleteData2);
//            }
//        }

      
      //ฟังกชั่นสลับ Top10 - Low10
            function ChageChartTopOrLow(TopOrLow) {
              var Data1 = $('#HdRightPanel').val();
              $('#HdTopOrLow').val(TopOrLow);//เก็บค่าว่า Sort แบบ Top10 หรือ Low10
              var ArrData = Data1.split('$');
              var State = ArrData[0];
            if (State == '1') {
            //$('#HdTypeChart').val('1');
                var CompleteData1 = ArrData[1];
                CreateChartMainLevel(CompleteData1);
            }
             if (State == '2') {
            //$('#HdTypeChart').val('1');
                var CompleteData1 = ArrData[1];
                CreateChartAllRoomInClassChoose(CompleteData1);
            }
            else if (State == '3') {
            //$('#HdTypeChart').val('1');
                var CompleteData1 = ArrData[1];
                CreateChartClass(CompleteData1);
            }
            else if (State == '4') {
           // $('#HdTypeChart').val('1');
              var CompleteData1 = ArrData[1];
            CreateChartRoom(CompleteData1);
            }
            else if (State == '5') {
            //$('#HdTypeChart').val('1');
            var SplitStr = ArrData[1].split('|');
            var CompleteData1 = SplitStr[0];
            var CompleteData2 = SplitStr[1];
            CreateChartActivity(CompleteData1,CompleteData2,TopOrLow);
            }
            else if (State == '6') {
         //$('#HdTypeChart').val('1');
            var CompleteData1 = ArrData[1];
            CreateChartActivityPerQuiz(CompleteData1,CompleteData2,TopOrLow);
            }
        }
        
        
           //------------------------------------------------------------------------------------
            //---- Function ตอนกดเปิดโหมดฝึกฝนต้องเช็คว่าอยู่ State ไหนเพื่อทำการโหลดกราฟใน State ปัจจุบัน ด้วยคะแนนในโหมดฝึกฝน
//            function LoadStatePracticeMode() {
//            var Data1 = $('#HdRightPanel').val();
//            var ArrData = Data1.split('$');
//            var State = ArrData[0];
//            alert(Data1);
//            alert(State);
//            if (State == '0') {
//                var CompleteData1 = ArrData[1];
//                CreatePieChartOnLoad();
//            }
//            else if (State == '1') {
//                var CompleteData1 = ArrData[1];
//                CreateChartMainLevel(CompleteData1);
//            }
//             else if (State == '2') {
//                var CompleteData1 = ArrData[1];
//                CreateChartAllRoomInClassChoose(CompleteData1);
//            }
//            else if (State == '3') {
//                var CompleteData1 = ArrData[1];
//                CreateChartClass(CompleteData1);
//            }
//            else if (State == '4') {
//              var CompleteData1 = ArrData[1];
//            CreateChartRoom(CompleteData1);
//            }
//            else if (State == '5') {
//            var SplitStr = ArrData[1].split('|');
//            var CompleteData1 = SplitStr[0];
//            var CompleteData2 = SplitStr[1];
//            CreateChartActivity(CompleteData1,CompleteData2);
//            }
//            else if (State == '6') {
//            var CompleteData1 = ArrData[1];
//            CreateChartActivityPerQuiz(CompleteData1,CompleteData2);
//            }
//            }


         function OpenBlockUI() {
            $.blockUI({
            message:'<h1><img src="../Images/reportGraph/ajax-loader Ja.gif" /><br /><br /><span style="color:#FFC76F;">รอสักครู่จ้า</span></h1>',
            css:{
            border:'none',
            //padding:'15px',
            backgroundColor:'transparent'
            }
            });
        }

        function CloseBlockUI(){
            $.unblockUI();
        }

        

    </script>
    <script type="text/javascript">

        // ตัวแปรเช็คค่า ที่เลือกว่าเป็นค่าเดิมที่เลือกก่อนหน้าหรือเปล่า
        var selectedClassSame;
        var selectedRoomSame;
        var selectedQuizSame;

        $(document).ready(function () {
            disabledCheckboxClass();

            // click on menu selected
            $('.divList').click(function () {

                // check ว่าเคยเลือกแล้วหรือยัง
                if ($(this).hasClass('divList selected')) {
                    $(this).removeClass('selected');
                    $('.divList').removeClass('changeSelected');
                    // check ว่า div ที่กดเข้ามา คือเมนูตัวไหน
                    if ($(this).is('#classSelected')) {
                        $('#menuShow').animate({ left: 45 }).hide(100);
                        //var getClass = getClassSelected();
                        //// check ค่าที่เลือกแล้ว return กลับมาเป็นค่าว่างหรือเปล่า
                        ////if (getClass != "") {
                        //if (selectedClassSame != getClass) {
                        //    selectedClassSame = getClass;
                        //    setRoomFromSelected(getClass);
                        //    $('#HdTypeChart').val('1');
                        //    $('#HdTopOrLow').val('0');
                        //    CreateChartMainLevel(getClass);
                        //}
                        //}
                    }
                    else if ($(this).is('#roomSelected')) {
                        $('#menuShowRoomSelected').animate({ left: 45 }).hide(100);
                        //var getRoom = getRoomSelected();
                        ////if (getRoom != "") {
                        //if (selectedRoomSame != getRoom) {
                        //    selectedRoomSame = getRoom;
                        //    //var practice = $('[id*=HdPracticeMode]').val();
                        //    //setQuizFromSelected(getRoom, practice);
                        //    setQuizFromSelected(getRoom);
                        //    $('#HdTypeChart').val('1');
                        //    $('#HdTopOrLow').val('0');
                        //    CreateChartClass(getRoom);
                        //}
                        //}
                    }
                    else if ($(this).is('#quizSelected')) {
                        $('#menuShowQuizSelected').animate({ left: 45 }).hide(100);
                        //var getQuiz = getQuizSelected();
                        //if (getQuiz != "") {
                        //    $('#HdTypeChart').val('1');
                        //    $('#HdTopOrLow').val('0');
                        //    alert(getQuiz);
                        //    CreateChartActivity(getRoomSelected(), getQuiz, '0')
                        //}
                    }
                }
                else {
                    $('.divList').removeClass('selected');
                    $('.divList').removeClass('changeSelected');
                    $(this).addClass('selected');

                    if ($(this).is('#classSelected')) {
                        $('#menuShowRoomSelected').animate({ left: 50 }).hide(100);
                        $('#menuShowQuizSelected').animate({ left: 50 }).hide(100);
                        $('#menuShow').show(100).animate({ left: 245 });

                    }
                    else if ($(this).is('#roomSelected')) {
                        $('#menuShow').animate({ left: 50 }).hide(100);
                        $('#menuShowQuizSelected').animate({ left: 50 }).hide(100);
                        $('#menuShowRoomSelected').show(100).animate({ left: 245 });
                        //var getClass = getClassSelected();
                        //if (selectedClassSame != getClass) {
                        //    selectedClassSame = getClass;
                        //    setRoomFromSelected(getClass);
                        //    $('#HdTypeChart').val('1');
                        //    CreateChartMainLevel(getClass);
                        //}

                    }
                    else if ($(this).is('#quizSelected')) {
                        $('#menuShow').animate({ left: 50 }).hide(100);
                        $('#menuShowRoomSelected').animate({ left: 50 }).hide(100);
                        $('#menuShowQuizSelected').show(100).animate({ left: 245 });
                        //var getRoom = getRoomSelected();
                        //if (selectedRoomSame != getRoom) {
                        //    selectedRoomSame = getRoom;
                        //    var practice = $('[id*=HdPracticeMode]').val();
                        //    //alert(practice);
                        //    setQuizFromSelected(getRoom, practice);
                        //    $('#HdTypeChart').val('1');
                        //    CreateChartClass(getRoom);
                        //}
                    }
                }

            });

            $('input[type=checkbox]').live('click', function () {
                //เมื่อกด checkbox เลือกชั้น
                if ($(this).hasClass('primaryClass') || $(this).hasClass('secondClass')) {
                    //alert('primaryclass');
                    //                    $('.primaryClass').each(function () {
                    //                        $(this).attr('checked', false);
                    //                        alert('asd');
                    //                    });
                    $('.primaryClass').attr('checked', false);
                    $('.secondClass').attr('checked', false);
                    $(this).attr('checked', true);
                    SetPrimaryClass();
                }
                //เมื่อกด checkbox เลือกห้อง
                else if ($(this).hasClass('classRoom')) {
                    //                    $('.classRoom').each(function () {
                    //                        $(this).attr('checked', false);
                    //                    });
                    $('.classRoom').attr('checked', false);
                    $(this).attr('checked', true);
                    SetClassRoom();
                }
                //เมื่อกด checkbox เลือกควิซ
                else if ($(this).hasClass('quizName')) {
                    //alert('asdasdasd');
                    //                        $('.quizName').each(function () {
                    //                            $(this).attr('checked', false);
                    //                            alert('zzzzzzzzzz');
                    //                        });
                    $('.quizName').attr('checked', false);
                    $(this).attr('checked', true);
                    SetSelectQuiz();
                    //$('#quizSelected').addClass('changeSelected');
                }

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
                LoadStatePracticeMode(); //Function สลับกราฟเมื่อกดเปลี่ยนโหมด
            });





        });


        //เมื่อกดเลือกชั้น
        function SetPrimaryClass() {
            $('#menuShow').animate({ left: 45 }).hide(100);
            var getClass = getClassSelected();
            if (selectedClassSame != getClass) {
                selectedClassSame = getClass;
                setRoomFromSelected(getClass);
                $('#HdTypeChart').val('1');
                $('#HdTopOrLow').val('0');
                CreateChartMainLevel(getClass);
            }
        }

        function SetClassRoom() {
            $('#menuShowRoomSelected').animate({ left: 45 }).hide(100);
            var getRoom = getRoomSelected();
            if (selectedRoomSame != getRoom) {
                selectedRoomSame = getRoom;
                setQuizFromSelected(getRoom);
                $('#HdTypeChart').val('1');
                $('#HdTopOrLow').val('0');
                CreateChartClass(getRoom);
            }
        }

        function SetSelectQuiz() {
            $('#menuShowQuizSelected').animate({ left: 45 }).hide(100);
            var getQuiz = getQuizSelected();
            if (getQuiz != "") {
                $('#HdTypeChart').val('1');
                $('#HdTopOrLow').val('0');
                CreateChartActivity(getRoomSelected(), getQuiz, '0')
            }
        }

        function NoFunction() {

        }


        function StandarFilter() {

            if ($('#HdFilterBtn').val() == '0') {
                $('.quizName').each(function () {
                    if ($(this).attr('IsStandard') == 'False') {
                        $(this).next().hide();
                    }
                    else {
                        $(this).next().show();
                    }
                });
                $('#HdFilterBtn').val('1');
            }
            else {
                $('.quizName').each(function () {
                        $(this).next().show();
                });
                $('#HdFilterBtn').val('0');
            }
        }

        function TeacherSetFilter() {
            //alert($('#HdFilterBtn').val());
            if ($('#HdFilterBtn').val() == 'IsTeacherSet') {
                $('.IsTeacherSet').each(function () {
                    $(this).hide();
                });

                $('.IsStandard').each(function () {
                    //$(this).show();
                    $(this).css('display', 'inline-block');
                });

                $('#HdFilterBtn').val('IsStandard');
                $('#btnToggleFilter').val('ชุดมาตรฐาน');
            }
            else {
                $('.IsStandard').each(function () {
                        $(this).hide();
                });

                $('.IsTeacherSet').each(function () {
                    //$(this).show();
                    $(this).css('display', 'inline-block');
                    });

                    $('#HdFilterBtn').val('IsTeacherSet');
                    $('#btnToggleFilter').val('ชุดที่ครูจัดไว้');
            }

        }

        var delayID = null;
        function SearchTestset() {
//            if (!($('.TestsetDiv').hasClass('slide_True'))) {
//                $(".TestsetDiv").slideToggle();
//                $(".TestsetDiv").addClass('slide_True');
//            }
            var prevValue = '';
            if (delayID != null) {
                clearInterval(delayID);
            }
            delayID = setInterval(function () {
                if (prevValue != $('#txtSearch').val()) {
                    //alert($('#txtSearch').val());
                    ClearNotMatchSearch($('#txtSearch').val().toLowerCase());
                    prevValue = $('#txtSearch').val();
                }
                if (prevValue == '') {
                    $('.icon_clear').delay(300).stop().fadeTo(300, 0);
                }
                else {
                    $('.icon_clear').stop().fadeTo(300, 1);
                }
            }, 500);
        }

        function ClearNotMatchSearch(txtSearch) {
            //alert(txtSearch);
            //var table = $('.quizName');
            if ($('#HdFilterBtn').val() == 'IsTeacherSet') {
                $('.IsTeacherSet').each(function () {
                    var tag = $(this).attr('filter');
                    var IsShow = $(this).css('display');
                    if (tag.toLowerCase().indexOf(txtSearch) == -1) {
                        if (IsShow != "none") {
                            $(this).hide();
                        }
                    }
                    else {
                        if (IsShow == "none") {
                            $(this).css('display', 'inline-block');
                        }
                    }
                });
            }
            else {
                $('.IsStandard').each(function () {
                    var tag = $(this).attr('filter');
                    var IsShow = $(this).css('display');
                    if (tag.toLowerCase().indexOf(txtSearch) == -1) {
                        if (IsShow != "none") {
                            $(this).hide();
                        }
                    }
                    else {
                        if (IsShow == "none") {
                            $(this).css('display', 'inline-block');
                        }
                    }
                });
            }
         
        }

        function ClickClear(e) {
            $(e).delay(300).stop().fadeTo(300, 0).prev('#txtSearch').val('');
            ShowAllMatchSearch();
        }

        function ShowAllMatchSearch() {
            //var table = $('.bordered');
            if ($('#HdFilterBtn').val() == 'IsTeacherSet') {
                $('.IsTeacherSet').each(function () {
                    var IsShow = $(this).css('display');
                    if (IsShow == 'none') {
                        $(this).css('display','inline-block');
                    }
                });
            }
            else {
                $('.IsStandard').each(function () {
                    var IsShow = $(this).css('display');
                    if (IsShow == 'none') {
                        $(this).css('display', 'inline-block');
                    }
                });
            }
        }


    </script>

    <script type="text/javascript">

    function SetSesstionCalendarId(CalendarId, CalendarName) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/StudentService.asmx/SetSessionCalendarId",
                data: "{ CalendarId: '" + CalendarId + "',CalendarName: '" + CalendarName + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Complete') {
                        CloseFancyBox();
                        window.location.reload();
                    }
                },
                error: function myfunction(request, status) {
                    //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });            
        }
        function CloseFancyBox() {
            $.fancybox.close();
        }

    </script>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        .divList
        {
            background-color: #B3F868;
            height: 70px;
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
            background-color: #539110;
            color: #F4F7FF;
            background-image: url("../Images/reportGraph/ShowRightFrame-White.png");
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
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
            background-color: #488506;
            -webkit-border-radius: 5px;
            color: #F4F7FF;
            background-image: url("../Images/reportGraph/HideRightFrame-White.png");
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
        #menuShow
        {
            width: 200px;
            position: absolute;
            left: 50px;
            top: 66px;
            border: 2px solid #F8DDA3;
            z-index: 1;
            height: 497px;
            background-color: #FFF1D3;
            cursor: pointer;
            display: none;
            -webkit-border-radius: 0px 10px 10px 0px;
            -moz-border-radius: 0px 10px 10px 0px;
            border-radius: 0px 10px 10px 0px;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }
        #menuShowRoomSelected
        {
            position: absolute;
            left: 50px;
            top: 66px;
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
        
         #menuShowQuizSelected
         {
            width:615px;
            position: absolute;
            left: 50px;
            top: 66px;
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
            text-align:left;
             }
        
        .selectedTerm
    {
        width: 400px;
        margin-left: auto;
        margin-right: auto;
        cursor:pointer;
        position:relative;
        margin-top:10px
    }
    .spnSelected
    {
        position:relative;
        margin-left:-45px;
        top:15px;
        font-size:30px;
        }       ul#Help
{
    position: fixed;
    margin: 0px;
    padding: 0px; /*top: 10px;*/
    left: -1px;
    bottom: 20px;
    list-style: none;
    z-index: 9999;
    cursor:pointer;
}
ul#Help li
{
    width: 110px;
    text-decoration: none;
    line-height: 1.7em;
    list-style: none;
}
ul#Help .about2 a
{
    background-image: url(../Images/ModeChangeBtn-Answer-Selected.png);
}
ul#Help li a
{
    display: block;
    margin-left: -2px;
    width: 110px;
    height: 70px;
    background-color: #CFCFCF;
    background-repeat: no-repeat;
    background-position: right center;
    border: 1px solid #AFAFAF;
    -moz-border-radius: 0px 10px 10px 0px;
    -webkit-border-bottom-right-radius: 10px;
    -webkit-border-top-right-radius: 10px;
    -khtml-border-bottom-right-radius: 10px;
    -khtml-border-top-right-radius: 10px;
    border-radius: 0px 10px 10px 0px;
    behavior: url('../css/PIE.htc');
    -pie-track-active: false;
    opacity: 0.6;
    filter: progid:DXImageTransform.Microsoft.Alpha(opacity=60);
}
    
        #txtSearch
        {
            position:absolute;
            right:10px;
            height:25px;
            top:9px;
            padding-left:30px;
            padding-right:20px;
            }
            span.icon_clear
    {
        position: absolute;
        right: 20px;
        top:10px;
        display: none;
        cursor: pointer;
        font: bold 1em;
        color: Red;
    }
    span.icon_clear:hover
    {
        color: Blue;
    }
       span.imgFind
    {
        background-image: url('../images/Search.png');
        display: inline-block;
        width: 25px;
        height: 25px;
        background-size: cover;
        position: absolute;
        top: 12px;
        right: 185px;
        z-index:2;
    }
       .IsStandard
    {
        display:none;
        width:195px;
        vertical-align: top;
        word-wrap:break-word;
        text-align:left;
        }
        .IsTeacherSet
        {
            display:inline-block;
            width:175px;
        vertical-align: top;
        word-wrap:break-word;
        text-align:left;
        padding:5px;
        margin-left:20px;
            }
     .Forbtn
        {
            font: 100% 'THSarabunNew';
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em; /*behavior:url(border-radius.htc);*/
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            width:128px;
            top:5px;
            left:15px;
            position:absolute;
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
</head>
<body>
    <form id="form1" runat="server">
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
    <div class="mainDiv" style='width: 990px; height: auto; margin-left: auto; margin-right: auto;
        position: relative; left: 0px; top: 25px;'>
        <myTerm:UserControl ID="MyCtlTerm"   runat="server" />
        <table id='Maintable' style='position:relative;top:-20px;'>
            <tr>
                <td>
                    <table id='LeftPanel'>
                        <tr>
                            <td>
                                <div class="divMenuLeft" style='position: relative; left: 6px; z-index: 2;background-color:#B3F868;'>
                                    <img src="../Images/reportGraph/btnHome-Detail.png" style='margin-top: 10px; margin-left: 10px;' id="imgHome" title='จัดชุดข้อสอบใหม่' />
                                    <%--<img src="../Images/reportGraph/btnBack.png" style='margin-top: 20px;margin-left:20px;' id="imgBack"
                                        title='กลับไปยังหน้าที่แล้ว' /><br />--%>
                          <%--          <div id="practiceMode" style="width: 60px; height: 60px; margin-top: 10px; text-align: center;
                                        float: left; margin-left: 10px; cursor: pointer; border: 1px solid;">
                                        OFF</div>--%>
                                   <%-- <img src="../Images/reportGraph/btnViewSingle.png" style="width: 50px; height: 50px;
                                        margin-top: 10px;position:relative;right:-50px;top:7px;" title='ดูคะแนนแบบรายตัว' id="imgReportPersonal" /><br />--%>
                                    <%--<div class="divClass" style='margin-top: -32px;'>
                                        <img src="../Images/freehand.png" id="divClassRoom" style="display: none" />
                                        <button type="button" id="btnClass" style="text-align: center; font: normal 100% 'THSarabunNew';">
                                            ระดับชั้น
                                        </button>
                                        <div class="divClassRoom" style="display: none;">
                                            <img src="../Images/freehand.png" id="divClassEvent" style="display: none" />
                                            <button type="button" id="btnClassRoom" style="text-align: center; font: normal 100% 'THSarabunNew';">
                                                ระดับห้อง
                                            </button>
                                            <div class="divClassEvent" style="display: none;">
                                                <img src="../Images/freehand.png" id="divQuizSelected" style="display: none" />
                                                <button type="button" id="btnClassEvent" style="text-align: center; font: normal 100% 'THSarabunNew';">
                                                    ชุดควิซ
                                                </button>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div style="position: relative; background-color: #B3F868; margin-top: 20px;">
                                        <div class="divList" id="classSelected" title="เลือกชั้นเรียนค่ะ"><span class='spnSelected'>ระดับชั้น</span><%--<span style="float: right;">→</span>--%><%--<img src="../Images/reportGraph/ShowRightFrame-Orange.png" style="float:right;"  />--%>
                                        </div>
                                        <div class="divList" id="roomSelected" title="เลือกห้องเรียนค่ะ"><span class='spnSelected'>ระดับห้อง</span><%--<span style="float: right;">→</span>--%>
                                        </div>
                                        <div class="divList" id="quizSelected"  title="เลือกควิซค่ะ"><span class='spnSelected'>ชุดควิซ</span><%--<span style="float: right;">→</span>--%>
                                        </div>
                                        <div class='graphSeleted'>
                                            <img id='imgTop10' src="../Images/reportGraph/btnTop10.png" style='cursor: pointer;
                                                margin-left: 6px; margin-top: 4px; float: left;' onclick='imgTop10()' title="ดูคะแนนสูงสุด 10 อันดับ" /> 
                                            <img id='imgLow10' src="../Images/reportGraph/btnBottom10.png" style='cursor: pointer;
                                                position: relative; float: left; margin-top: 4px;' onclick='imgLow10()' title="คะแนนต่ำสุด 10 อันดับ" /></div>
                                      <%--  <div class='graphSeleted' style="margin-top: 12px;">
                                            <img id='SwapChart' src="../Images/reportGraph/btnChartBar.png" style='cursor: pointer;
                                                margin: 5px; margin: 10px; float: left; position: relative; width: 50px;' title="คะแนนแบบกราฟแท่ง" />
                                            <img id='SwapLineChart' src="../Images/reportGraph/btnChartLine.png" style='cursor: pointer;
                                                width: 50px; margin: 10px; float: left; position: relative;' title="คะแนนแบบกราฟเส้น" />
                                            <img id='SwapPieChart' src="../Images/reportGraph/btnChartPie.png" style='cursor: pointer;
                                                width: 50px; margin: 10px; float: left; position: relative;' title="คะแนนแบบกราฟวงกลม" /></div>--%>
                                    </div>
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
                        <div id='TopLow10'>
                            <img src="../Images/reportGraph/top10Btn.png" style='cursor: pointer' onclick='imgTop10()'
                                title="ดูคะแนนสูงสุด 10 อันดับ" /><span style='font-size: 12px; cursor: pointer;
                                    position: relative; top: -20px;' onclick='imgTop10()'><b> คะแนนสูงสุด 10 อันดับ</b></span>
                            <br />
                            <img src="../Images/reportGraph/bottom10Btn.png" style='cursor: pointer; position: relative;'
                                onclick='imgLow10()' title="คะแนนต่ำสุด 10 อันดับ" /><span style='font-size: 12px;
                                    cursor: pointer; position: relative; top: -35px' onclick='imgLow10()'><b> คะแนนต่ำสุด
                                        10 อันดับ</b></span></div>
                        <div id='SwapChart'>
                            <img src="../Images/reportGraph/barchart.png" onclick='imgColumn()' style='cursor: pointer;
                                width: 120px;' title="คะแนนแบบกราฟแท่ง" /></div>
                        <div id='SwapLineChart'>
                            <img src="../Images/reportGraph/linechart.png" style='cursor: pointer; width: 120px;'
                                onclick='imgLine()' title="คะแนนแบบกราฟเส้น" /></div>
                        <div id='SwapPieChart'>
                            <img src="../Images/reportGraph/piechart.png" style='cursor: pointer; width: 120px;'
                                onclick='imgPie()' title="คะแนนแบบกราฟวงกลม" /></div>
                    </div>
                </td>--%>
            </tr>
        </table>
        <div id="menuShow" style="" class="showSelected">
            <table>
                <tr style="background-color: #F8DDA3">
                    <th>
                        ประถม
                    </th>
                    <th style="-webkit-border-radius: 0px 10px 0px 0px;">
                        มัธยม
                    </th>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="ป1" class="primaryClass" /><label for="ป1">1</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม1" class="secondClass" /><label for="ม1">1</label>
                    </td>
                </tr>
                <tr style="background-color: #FFF1D3">
                    <td>
                        <input type="checkbox" id="ป2" class="primaryClass" /><label for="ป2">2</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม2" class="secondClass" /><label for="ม2">2</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="ป3" class="primaryClass" /><label for="ป3">3</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม3" class="secondClass" /><label for="ม3">3</label>
                    </td>
                </tr>
                <tr style="background-color: #FFF1D3">
                    <td>
                        <input type="checkbox" id="ป4" class="primaryClass" /><label for="ป4">4</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม4" class="secondClass" /><label for="ม4">4</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="checkbox" id="ป5" class="primaryClass" /><label for="ป5">5</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม5" class="secondClass" /><label for="ม5">5</label>
                    </td>
                </tr>
                <tr style="background-color: #FFF1D3">
                    <td>
                        <input type="checkbox" id="ป6" class="primaryClass" /><label for="ป6">6</label>
                    </td>
                    <td>
                        <input type="checkbox" id="ม6" class="secondClass" /><label for="ม6">6</label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="menuShowRoomSelected">
        </div>
        <div id="menuShowQuizSelected">
        
        </div>
        <center>
            <footer style="position: relative; top: -10px">
                            <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
                    </footer>
        </center>
              <ul id="Help">
            <li class="about2" style="z-index: 99;"><a title="ดูรายงานแบบละเอียด"
                id="HelpLogin" onclick="ShowHelp();">วิเคราะห์
            </a></li>
        </ul>
    </div>
    <input type="hidden" id='TestSetNameHD' />
    <input type="hidden" id='HdTypeChart' />
    <input type="hidden" id='HdRightPanel' />
    <input type="hidden" id='HdTopOrLow' />
    <input type="hidden" id='HdCheckPostBack' value="<%=Page.IsPostBack.Tostring() %>" />
    <input type="hidden" id='HdPracticeMode' value="0" />
    <input type="hidden" id="HdFilterBtn" value="IsTeacherSet" />
    <%--<input type="button" id='TestPracticeMode' value='TestPracticeMode' />--%>
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
