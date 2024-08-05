<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatTeacherSearchStudentParent.aspx.vb" Inherits="QuickTest.ChatTeacherSearchStudentParent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../../js/jquery-ui-1.8.18.js"></script>
    <script src="../js/jqMenuStick.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/StyleMobile.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {

            $('#btnBack').click(function () {
                window.location = '<%=ResolveUrl("~")%>Student/DashboardStudentPage.aspx'
            });

            var html = "<%=htmlClass %>";    
            //alert(html);
            $('#a').replaceWith(html);
            $("#ac").accordion({
                autoHeight: false, animate: 0, heightStyle: "content", collapsible: true, active: false
            });

            $("#ac > h3").live('click', function () {
                var c = $(this).prevAll('h3').length + 1;
                var h = (62 * c) + 80;
                //console.log(h);                
                $('.menuFixedBar').scrollTop(h);
            });

            // menu click show/hide
            $('.showMenuFixedBar').click(function () {
                var menuClass = $('#menuClassName');
                if ($(menuClass).hasClass('hideClassName')) {
                    $(this).css('background-image', "url('../images/dashboard/CloseClass.png')");
                    $(menuClass).removeClass('hideClassName').addClass('showClassName', 500, 'easeInQuart');
                } else {
                    $(this).css('background-image', "url('../images/dashboard/OpenClass.png')");
                    $(menuClass).removeClass('showClassName').addClass('hideClassName', 500, 'easeOutBounce');
                }
            });

            // selected room
            $('div.menuAcdItem').live('click', function () {
                var classRoom = $(this).html();
                SelectedClassRoom(classRoom);
            });

            // click icon clear
            $('.icon_clear').click(function () {
                $(this).delay(300).fadeTo(300, 0).prev('#txtSearhStudent').val('');
                //GetStudentSearch('');
                $('#DivSelectParent').hide();
                $('#EmptyDiv').text('เลือกนักเรียนก่อนค่ะ');
                $('#EmptyDiv').show();
            });

            //เมื่อคลิกที่ Div เพื่อจะเข้าไปสู่หน้า Chat
            $('.DivCover').live('click',function () {
                var id = $(this).attr('prid');
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx/GotoChatRoom",
                    data: "{ PR_ID : '" + id + "'}",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d != '') {
                            window.location = '<%=ResolveUrl("~")%>Watch/Chat.aspx?ChatRoom_Id=' + data.d + '&FromTeacher=True';
                        }
                },
                error: function myfunction() {
                    alert('jeng');
                }
                });
            });

        });


        // click เลือกห้อง
        function SelectedClassRoom(ClassRoom) {
            $('#headClassName').show();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx/GetChatStudentParent",
                data: "{ ClassRoom : '" + ClassRoom + "'}",
                async:false,
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d == '') {
                        $('#DivSelectParent').hide();
                        $('#EmptyDiv').show();
                    }
                    else {
                        $('#EmptyDiv').hide();
                        $('#DivSelectParent').show();
                        $('#DivSelectParent').html(msg.d);
                    }
                },
                error: function myfunction() {
                    alert('jeng');
                }
            });
        }

        // search แบบมี delay
        var delayID = null;
        function SearchStudent() {
            var prevValue = '';
            if (delayID != null) {
                clearInterval(delayID);
            }
            delayID = setInterval(function () {
                if (prevValue != txtSearhStudent.value && txtSearhStudent.value != '') {
                    GetStudentSearch(txtSearhStudent.value.toLowerCase());
                    prevValue = txtSearhStudent.value;
                }
                if (prevValue == '') {
                    $('.icon_clear').delay(300).fadeTo(300, 0);
                }
                else {
                    $('.icon_clear').stop().fadeTo(300, 1);
                }
            }, 2000);
        }

        // clear time delayID
        function ClearTimeInterval() {
            clearInterval(delayID);
        }

        function GetStudentSearch(txtSearch) {
            $('#headClassName').show();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Watch/ChatTeacherSearchStudentParent.aspx/GetSearchChatStudentParent",
                data: "{ txtSearch : '" + txtSearch + "'}",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (msg) {
                    if (msg.d == '') {
                        $('#DivSelectParent').hide();
                        $('#EmptyDiv').text('ไม่พบผู้กปกครองหรือนักเรียนที่ค้นหาค่ะ');
                        $('#EmptyDiv').show();
                    }
                    else {
                        $('#EmptyDiv').hide();
                        $('#DivSelectParent').show();
                        $('#DivSelectParent').html(msg.d);
                    }
                },
                error: function myfunction() {
                    alert('jeng');
                }
            });
        }



         
    </script>
    <title></title>
    <style type="text/css">
        .ui-accordion .ui-accordion-header
        {
            text-align: center;
            color: #444;
        }
        .ui-accordion .ui-accordion-content
        {
            height: auto;
            line-height: 16pt;
            padding: 0;
            background-color: White;
            color: #444;
        }
        .ui-state-active
        {
            border: 1px solid yellow;
            background-color: #FFA032;
            color: #444;
        }
        .menuAcdItem
        {
            line-height: 50px;
            vertical-align: middle;
            border-bottom: 1px solid;
            /*padding-left: 40px;*/
        }
        .msMainMenu
        {
            height: auto;
            position: relative; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            margin: 5px;
        }
        
        .menuFixedBar
        {
            height: 100%;
            width: 240px;
            top: 10px;
            left: 0; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            overflow-y: auto;
            border-radius: 5px;
        }
        span.icon_clear
        {
            position: absolute;
            right: 10px;
            display: none;
            cursor: pointer;
            font-weight: bold;
            font-size: 20px;
            top: -2px;
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
            top: -5px;
            left: 5px;
        }
        #txtSearhStudent
        {
            width: 120px;
            padding-left: 30px;
            padding-right: 30px;
            height: 30px;
            font: 20px 'THSarabunNew';
            margin: 13px 0 0 0;
        }
        .SearhStudent, .ToggleModeClassName
        {
            margin: 10px 5px;
            border: 1px solid rgb(235, 235, 235);
            background-color: #FFA032;
            border-radius: 5px;
            height: 62px;
            color: #444;
        }
        .ToggleModeClassName
        {
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            line-height: 62px;
            cursor: pointer;
        }
        #ctl00_MainContent_ShowStudentSearch, #ctl00_MainContent_ShowStudentSelectedRoom
        {
            border-color:orange;
            width: 821px;
            border-radius: 5px;
            margin: 0px auto 0 auto;
            padding: 10px 0 0 0;
            overflow-y: auto;
            text-align: center;
            display: none;
            height: 450px;
            /*line-height: 456px*/
        }
        #ctl00_MainContent_ShowStudentSearch :hover
        {
            background-color: #F68500;
            cursor: pointer;
        }
        .NotSelectedRoom, .NotFoundStudent
        {
            border: 1px dashed #FFA032 !important;
            line-height: 172px;
            font-size: 25px;
            font-weight: bold;
            color: rgb(122, 119, 119);
        }
        .NotSelectedRoom
        {
            background-image: url('../images/ChooseRoom.png');
            background-repeat: no-repeat;
            background-position: 50px;
            
        }
        .StudentSearch
        {
            border: 2px solid;
        }
        #ctl00_MainContent_ShowStudentSearch > div
        {
            width: 288px;
            height: 130px;
            border: 1px solid rgb(235, 235, 235);
            border-radius: 5px;
            display: inline-block;
            margin: 20px 35px;
            padding: 5px;
            overflow:hidden;
        }
        #ctl00_MainContent_ShowStudentSearch img
        {
            width: 100px;
            height: 120px;
            margin: 5px 0 0 0;
        }
        #ctl00_MainContent_ShowStudentSearch > div > div
        {
            float: right;
            width: 180px;
        }
        #ctl00_MainContent_ShowStudentSearch > div > div label
        {
            font-weight: bold;
            font-size: 22px;
        }
        /* #ctl00_MainContent_ShowStudentSearch .Left, #ctl00_MainContent_ShowStudentSearch .Middle
        {
            margin: 0 5px 5px 0;
        }
        */
        #menuClassName
        {
            width: 250px;
            background: rgb(255, 192, 119);
            height: 625px;
            position: fixed;
            top: 10px;
            z-index: 999999999;
            border-radius: 0 .5em .5em 0;
            box-shadow: #222 2px 2px 2px;
            padding: 10px 0;
            /*background: -webkit-gradient(linear, left top, right top, from(rgb(255, 242, 228)), to(rgb(255, 192, 119)));*/
        }
        .hideClassName
        {
            left: -240px;
        }
        .showClassName
        {
            left: 0;
        }
        .showMenuFixedBar
        {
            width: 40px;
            height: 80px;
            position: absolute;
            right: -35px;
            top: 50%;
            margin-top: -40px;
            background-image: url('../images/dashboard/OpenClass.png');
            background-size: 40px;
            background-repeat: no-repeat;
            cursor: pointer;
            background-position: 50%;
        }
        #ctl00_MainContent_lblClassRoom
        {
            font-size: 140%;
            color: #F68500;
            padding: 0 30px;
        }
    </style>
    <style type="text/css">
        .ForDivShowInFo
        {
            text-align: center;
            position: relative;
            top: 15px;
        }
        .spnHead
        {
            font-size: 50px;
            font-weight: bold;
        }
        .ForSpnNotification {
            background-color: red;
            position: relative;
            left: 265px;
            top: -20px;
            width:45px;
            height:30px;
            border-radius: 25px;
            padding: 10px;
            text-align: center;
            color: white;
            font-size: 30px;
        }
         #imgBack {
         position:absolute;
         left:100px;
         top:10px;
         width:80px;
         height:80px;
         cursor:pointer;
         }
         #MainDiv {
         width:1100px;
         margin-left:auto;
         margin-right:auto;
         }
        .DivName {
        height:auto;
        }
           #menuClassName
        {
            width: 250px;
            background: rgb(255, 192, 119);
            height: 625px;
            position: fixed;
            top: 10px;
            z-index: 999999999;
            border-radius: 0 .5em .5em 0;
            box-shadow: #222 2px 2px 2px;
            padding: 10px 0;
            /*background: -webkit-gradient(linear, left top, right top, from(rgb(255, 242, 228)), to(rgb(255, 192, 119)));*/
        }
        #txtSearhStudent {
            width: 120px;
            padding-left: 30px;
            padding-right: 30px;
            height: 30px;
            font: 20px 'THSarabunNew';
            margin: 13px 0 0 0;
        }
        .showMenuFixedBar
        {
            width: 40px;
            height: 80px;
            position: absolute;
            right: -35px;
            top: 50%;
            margin-top: -40px;
            background-image: url('../images/dashboard/OpenClass.png');
            background-size: 40px;
            background-repeat: no-repeat;
            cursor: pointer;
            background-position: 50%;
        }
            .menuFixedBar
        {
            height: 100%;
            width: 240px;
            top: 10px;
            left: 0; /*box-shadow: #888 2px 2px 5px;*/
            z-index: 1;
            overflow-y: auto;
            border-radius: 5px;
        }
             .hideClassName
        {
            left: -240px;
        }
              .showClassName
        {
            left: 0;
        }
        #EmptyDiv {
        border: 5px dotted;
        padding: 100px;
        font-size: 70px;
        border-radius: 5px;
        border-color: orange;
        color: orange;
        }
        .ForDivDetailStudent {
        font-size:21px;
        }
        .Forbtn {
        position:absolute;
        left:40px;
        width:150px;
        }
        .Forbtn:hover {
        background-color: rgb(35, 233, 75) !important;
       }
         .DivCover {
         margin:0 20px 20px 20px!important;
         
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <div id="MainDiv" runat="server" style="text-align: center;padding-top:20px;">

              <div id="menuClassName" class="hideClassName">
        <div class="showMenuFixedBar">
        </div>
        <div class="menuFixedBar">
            <div class="SearhStudent" style="margin-top: 0px; text-align: center;">
                <span style="position: relative;"><span class="imgFind"></span>
                    <input type="text" id="txtSearhStudent" placeholder="ค้นหานักเรียน" onfocus="SearchStudent();"
                        onblur="ClearTimeInterval();" />
                    <span class="icon_clear">X</span> </span>
            </div>
          <%--  <div class="ToggleModeClassName" id="ModeClassName" runat="server">
            </div>--%>
            <div id='mainMenu' class='msMainMenu'>
                <div id="ac">
                    <div id="a" runat="server">
                    </div>
                </div>
            </div>
        </div>
    </div>

        <div id="DivSelectParent">
               <%--<div id='DivCover1' class='DivWidth DivCover'>
                <div id='Divpicture1' class='DivWidth DivPicture' >
                     
                    <div class="ForSpnNotification">888</div>
                    </div>
                <div id='DivName" & CountLoop & "' class='DivWidth DivName' >
                    <div>zxcrmvb</div>
                    <div>สมรัก พรรคเพื่อเก้ง ป.3/1</div>
                    </div>
            </div>--%>
        </div>

          <div id="EmptyDiv">
              เลือกนักเรียนก่อนค่ะ
          </div>

    </div>
    <div class="footer">
        <input type="button" id="btnBack" value="กลับ" class="Forbtn" />
        เลือกผู้ปกครอง
    </div>
    </form>
</body>
</html>
