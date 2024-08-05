﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ChooseTestset.aspx.vb" Inherits="QuickTest.ChooseTestset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%If IsAndroid = True Then%>
    <style type="text/css">
        table tr td {
            font-size: 45px;
        }

        h3 {
            font-size: 45px !important;
        }

        h2 {
            font-size: 45px !important;
        }

        #divShowData {
            margin-top: -100px !important;
        }

        .imgPlayQuiz, .imgPlayHomeWork {
            width: 160px !important;
            height: 160px !important;
            margin-top: -35px !important;
        }

        .UserImage {
            width: 70px !important;
            height: 90px !important;
            margin-right: 15px !important;
        }

        .Homework {
            padding-left: 115px !important;
        }
    </style>
    <%End If%>
    <style type="text/css">
        .imgPlayQuiz {
            position: absolute;
            right: 45%;
            margin-right: -25px;
            width: 90px;
            height: 90px;
            display: none;
            margin-top: -28px;
        }

        .imgPlayHomeWork {
            position: absolute;
            right: 45%;
            margin-right: -25px;
            width: 90px;
            height: 90px;
            display: none;
            margin-top: -28px;
        }

        .UserImage {
            float: right;
            cursor: pointer;
            display: none;
            width: 40px;
            height: 60px;
            margin-right: 20px;
        }
        /*.showCompleteHW {
            width: 155px;
            height: 40px;
            margin: 5px 20px;
            line-height: 40px;
            border-radius: 10px;
            color: white;
            position: initial;
            top: 0px;           
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            border: solid 1px #0D8AA9;
            border: 0;
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%);
            font: 100% 'THSarabunNew';
        }*/
        .showCompleteHW {
            font: 100% 'THSarabunNew';
            width: 155px;
            height: 40px;
            margin: 5px 20px;
            line-height: 40px;
            position: initial;
            border: 0;
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            -moz-border-radius: .5em;
            -webkit-border-radius: .5em;
            border-radius: .5em;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            background: #63cfdf;
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iIzYzY2ZkZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiMxN2IyZDkiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* IE10+ */
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 ); /* IE6-8 */
        }
        /*.Forbtn:hover {
        background-color:rgb(35, 233, 75) !important;
       }*/
        #site_content{
                border-radius: 10px;
                position:relative;
        }
        .btnRefresh{
            background-image:url("../images/refresh.png");
            width:64px;
            height:64px;
            position:absolute;
            right:29px;
        }
    </style>    

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');
        function refreshPage() {
            location.reload();
        }
    </script>
    <form runat="server" id='formChooseTestSet'>
        <div id="main">

            <div id="site_content">
                <div class="btnRefresh" onclick="refreshPage();"></div>
                <div class="content" style="width: 930px;padding:0;">
                    <section id="select">
		<center>
		<h2 >สมุดการบ้าน</h2>

    <%If HdChkSpareTablet.Value = False Then%>
                    <div id="div-1" class="ListingFixedHeightContainer BarHomework" style='text-align: center;'>
                            <h3 style="width: 350px; margin-left: auto; margin-right: auto;"><label class="Homework"  >ทำการบ้าน</label></h3>        
                   
        
                    <div id='divChkShowCompleteTS' style='float:right;margin-top:-58px; display:none;'>
                           <asp:Button ID="chkShowCompleteTS"  name='TestGetValue'  CssClass="showCompleteHW" runat="server" Text="การบ้าน" />
                       <%-- <input type="checkbox" name='TestGetValue' id='chkShowCompleteTS' style="z-index:55555555;" />
                        <label for='chkShowCompleteTS' style='color:black;' >แสดงประวัติการบ้าน ?</label>--%>
                    </div>


                    <div id='divShowData' style='float:right;margin-top:-84px; line-height:0em; margin-right:22px;display:block; '>
                        <table>
                            <tr>
                                <td style="width:39px" >
                                    <img id="IsValidate" src="../Images/Homework/Complete.png" style="width:25px;height:25px;"  onclick="ShowQtip('IsValidate');"/>
                                </td>
                                <td>
                                    <%= TotalNotChecked%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img id="NotIsComplete" src="../Images/Homework/Uncomplete.png" style="width:25px;height:25px;"  onclick="ShowQtip('NotIsComplete');"/>
                                </td>
                                <td> 
                                    <%= TotalNotExited%>               
                                </td>
                            </tr>
                        </table>
                </div>
       </div>                    
    <div id="divHomeWork" class="ListingContent" clientidmode="Static" style='text-align: center; position: relative;
                height: 100%; display: block; width: 90%;' runat="server" >
                
                <asp:Repeater ID="RptHomeWork" runat="server">
                    <HeaderTemplate>
                    <table class="bordered">
                        <thead>
                            <tr>
                                <th style="width: 55%;">
                                    ชื่อ
                                </th>
                                <th style="width: 20%;">
                                    วิชา
                                </th>
                                 <th style="width: 20%;">
                                    กำหนดส่ง
                                </th>
                                <th style="width: 5%;">
                                    <%= txtColum%>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="<%# Container.DataItem("Quiz_Id")%>">
                            <td style="background: #FFFFCC;">
                                <img id="play_<%# Container.DataItem("Quiz_Id")%>" src="../Images/homework/btnPlay.png"
                                    class="imgPlayHomeWork" />
                                <%# Container.DataItem("TestSet_Name")%>
                            </td>
                            <td style="background: #FFFFCC;">
                                <%# Container.DataItem("SubjectName")%>
                            </td>
                            </td>
                            <td style="background: #FFFFCC;">
                                <%# Container.DataItem("ForMatDate")%>
                            </td>
                            <td style="background: #FFFFCC;">
                                <%# Container.DataItem("PercentMade")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="<%# Container.DataItem("Quiz_Id")%>">
                            <td style="background: #FFFFFF;">
                                 <img id="play_<%# Container.DataItem("Quiz_Id")%>" src="../Images/homework/btnPlay.png"
                                    class="imgPlayHomeWork" />
                                <%# Container.DataItem("TestSet_Name")%>
                            </td>
                            <td style="background: #FFFFFF;">
                                <%# Container.DataItem("SubjectName")%>
                            </td>
                            </td>
                            <td style="background: #FFFFFF;">
                                <%# Container.DataItem("ForMatDate")%>
                            </td>
                            <td style="background: #FFFFFF;">
                                <%# Container.DataItem("PercentMade")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <div id="divWithoutHomework" runat="server" style='height: 100px; background-color: white;
                    overflow-y: initial; border: 1px dashed black; color: black; line-height: 6em;
                    font-weight: bold; margin: auto; width: 99%'>
                    <%=txtWithOutHaveHomework %></div>
            </div>
    <div id="div-2" class="ListingFixedHeightContainer BarPracticeTeacherSet" <%--onclick="ToggleBox();"--%> style='text-align: center;' <%--visibility: <%= GetTestset(HttpContext.Current.Session("ShowFullPractice"))%>'--%>>
        <h3>
            <label class="PracticeFromTeacher" >
                ฝึกตามคุณครู</label></h3>
    </div>
    <div id="divListing" class="ListingContent" style='text-align: center; position: relative;
        height: 100%; visibility: inherit'>
        <asp:Repeater ID="Listing" runat="server">
            <HeaderTemplate>
                <table class="bordered" style="width: 91%; border-spacing: 4; margin-top: 0px; margin-left: 41px;">
                    <thead>
                        <%--  <tr ><th >ชื่อ</th></tr>--%>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id="<%# Container.DataItem("TestSet_Id")%>">
                    <td id="tdItem" runat="server" style="background: #FFFFCC;">
                        <img id="play_<%# Container.DataItem("TestSet_Id")%>"  src="../Images/homework/btnPlay.png"
                            class="imgPlayQuiz" />
                        <%# Container.DataItem("TestSet_Name")%>
                        <img src="../Images/Homework/EverMade.png" style='float: right; cursor: pointer; visibility: <%# Container.DataItem("IsPracticed")%>' />

                        <img id = "User_<%# Container.DataItem("TestSet_Id")%>" src="../Images/Homework/EverMade.png" 
                            class="UserImage" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="<%# Container.DataItem("TestSet_Id")%>">
                    <td id="tdItem" runat="server"  style="background: #FFFFFF;">
                        <img id="play_<%# Container.DataItem("TestSet_Id")%>"    src="../Images/Homework/btnPlay.png"
                            class="imgPlayQuiz" />
                        <%# Container.DataItem("TestSet_Name")%>
                        <img src="../Images/Homework/EverMade.png" style='float: right; cursor: pointer; visibility: <%# Container.DataItem("IsPracticed")%>' />

                         <img id = "User_<%# Container.DataItem("TestSet_Id")%>" src="../Images/Homework/EverMade.png" 
                             class="UserImage" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
  <%End If%>
    <div id="div-3" style='text-align: center' class="BarPracticeStandard" <%--onclick="document.location.href ='../PracticeMode_Pad/ChooseClass.aspx?DashboardMode=9'"--%>>
        <h3>
            <label class="Practice" >
                ทบทวนตามบทเรียน</label></h3>
    </div>
   <%If HdChkSpareTablet.Value = False Then%>
    <div id="div-4" style='text-align: center' class="BarStudentDetail" <%--onclick="document.location.href ='../Student/StudentDetailPage.aspx'"--%>>
        <h3>
            <label class="Report" >
                ผลงานทั้งหมด</label></h3>
    </div>
            <%End If%>
    <%--       onclick="document.location.href ='../PracticeMode_Pad/ChooseClass.aspx?SchoolCode=<%#  %>'"--%>
    <%--    <%= GetYearChecked(51)%>--%>
    </center> </section>
                    <script type="text/javascript">                    var c = document.getElementById("select");
                        function resizeText(multiplier) {
                            if (c.style.fontSize == "") {
                                c.style.fontSize
        = "1.0em";
                            } c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2)
        + "em";
                        } </script>
                </div>
            </div>
            <div id="dialog" title="ต้องเข้าควิซแล้วค่ะ">
            </div>
        </div>
        <input type="hidden" id='HdChkSpareTablet' value="false" runat="server" />
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/slimScroll.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
    <script type="text/javascript">var IsPracticeFromComputer = '<%=Session("PracticeFromComputer") %>'; if (IsPracticeFromComputer == "False") { var GroupName = "<%=GroupName %>"; var DeviceId = '<%=DVID %>'; }</script>
    <script src="../js/StudentSignalR.js" type="text/javascript"></script>
    <script type="text/javascript">
        var stdId = '<%=HttpContext.Current.Session("UserId") %>';
        //        $(function () {

        //            var groupname = '<%=GroupName %>';
        //            var DeviceId = '<%=DVID %>';
        //            SignalRCheck = $.connection.hubSignalR;
        //            // alert(groupname);

        //            SignalRCheck.client.send = function (message) {

        //                if (message == '<%=ResolveUrl("~")%>testset/step1.aspx') {
        //                    window.location = '<%=ResolveUrl("~")%>TestSet/Step1_PadTeacher.aspx';
        //                }
        //                else if (message == '<%=ResolveUrl("~")%>activity/settingactivity.aspx') {
        //                    window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx';
        //                }
        //                else if (message == '<%=ResolveUrl("~")%>activity/activitypage.aspx') {
        //                    window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
        //                }
        //                else if (message == 'Reload') {
        //   
        //                    $.ajax({ type: "POST",
        //	                url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/SendToGetNextAction",
        //	                data: "{ DeviceUniqueID: '" + DeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
        //	                contentType: "application/json; charset=utf-8", dataType: "json",   
        //	                success: function (msg) {
        //                    //ถ้าค่าที่ Return กลับมาไม่เป็นค่าว่างให้ ReLoad หน้าใหม่
        //                    var objJson = jQuery.parseJSON(msg.d);
        //                    
        //                    if (objJson.Param.NextURL != '') {
        //                        //window.location = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;                        
        //                        var joinQuiz = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
        //                        dialogInterrupt(joinQuiz);
        //                    }
        //	            },
        //	            error: function myfunction(request, status)  {
        //                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	            }
        //	        });
        //             }
        //            };

        //            $.connection.hub.start().done(function () {
        //                SignalRCheck.server.addToGroup(groupname);
        //                //SignalRCheck.server.sendCommand(groupname, '../TestSet/Step1.aspx');
        //            });

        //        });
        //        function dialogInterrupt(href){        
        //            $('#dialog').dialog({
        //                autoOpen: open,
        //                buttons: { 'ตกลง': function () { window.location.href = href; } },
        //                draggable: false,
        //                resizable: false,
        //                modal: true
        //            });
        //        }


        function ShowQtip(id) {

            var ContentText;
            if (id == 'IsValidate') {
                ContentText = 'การบ้านที่ตรวจแล้ว'
            }
            else if (id == 'NotIsComplete') {
                ContentText = 'การบ้านที่ยังไม่ส่ง'
            }
            $('#divShowData').qtip({
                content: ContentText,
                show: { ready: true },
                style: {
                    width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                },
                position: { target: $('#divShowData') },
                //hide: { when: { event: 'focus', event: 'mouseover', event: 'mouseout'} }
                hide: { when: { event: 'mouseout' }, fixed: false }
            });
            setTimeout(function () { $('#divShowData').qtip("Destroy"); }, 2000);
        }

    </script>
    <%--    <title></title>--%>

    <script type="text/javascript">


        $(function () {


            //เมื่อกดที่ tr ให้มันสลับสี
            $('tr').each(function () {
                new FastButton(this, TriggerTrClick);
            });

            //เมื่อกดที่ ปุ่ม play ตอนเล่น Quiz
            $('.imgPlayQuiz').each(function () {
                new FastButton(this, TriggerClickPlayQuiz);
            });

            //เมื่อกดที่ ปุ่ม play ตอนทำการบ้าน
            $('.imgPlayHomeWork').each(function () {
                new FastButton(this, TriggerClickPlayHomework);
            });

            //กด แถบ ทำการบ้าน
            $('.BarHomework').each(function () {
                new FastButton(this, TriggerHomeBarClick);
            });

            //กด แถบ ฝึกตามคุณครู
            $('.BarPracticeTeacherSet').each(function () {
                new FastButton(this, TriggerPracticeBarClick);
            });

            //กด แถบ ฝึกฝนตามใจ
            $('#BarPracticeStandard').each(function () {
                alert(250);
                new FastButton(this, TriggerPracticeStandardBarClick);
            });

            //กด แถบ ผลงานทั้งหมด
            $('.BarStudentDetail').each(function () {
                new FastButton(this, TriggerStudentDetailBarClick);
            });

            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {

                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                //$('table tr td').css('font-size', '45px');
                //$('h3').css('font-size', '45px');
                //$('h2').css('font-size', '45px');
                //$('#divShowData').css('margin-top', '-100px');
                //$('.imgPlayQuiz').css({ 'width': '160px', 'height': '160px', 'margin-top': '-35px' });
                //$('.imgPlayHomeWork').css({ 'width': '160px', 'height': '160px', 'margin-top': '-35px' });
                //$('.UserImage').css({ 'width': '70px', 'height': '90px', 'margin-right': '15px' });
                //$('.Homework').css('padding-left', '115px');
            }

            //            $("#divListing").alternateScroll();
            //            $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />');
            ToggleBox();
            ToggleHomeWork();

            //$('.SeeFullPractice').click(function () {
            //    $.ajax({
            //        type: "POST",
            //url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/SetValueToSessionShowFullPractice",
            //        async: false,
            //        contentType: "application/json; charset=utf-8", dataType: "json",
            //        success: function (msg) {
            //            window.location = window.location.href;
            //        },
            //        error: function myfunction(request, status) {
            //        }
            //    });
            //});

            ////chkShowCompleteTS
            //$('#ChkShowCompleteTS').each(function () {
            //    alert('1');
            //    new FastButton(this, TickChkIsCompleteHM);
            //});



        });


        function ToggleBox() {

            $("#divListing").slideToggle("slow");
            if ($.browser.msie) {
                if ($.browser.version <= 7) {
                    $('#divListing').css('overflow', 'auto');
                }
            }

        }
        var isPostback = '<%=IsPostback%>';
        function ToggleHomeWork() {
            console.log(isPostback);
            if (isPostback == "False") {
                if ($('#divHomeWork').css('display') == 'block') {
                    $('#divHomeWork').stop().hide(500);
                    $('#divChkShowCompleteTS').stop().hide(300);
                    $('#divShowData').stop().show(500);
                }
                else {
                    $('#divHomeWork').stop().show(500);
                    $('#divChkShowCompleteTS').stop().show(300);
                    $('#divShowData').stop().hide(300);
                }
            } else {
                if ($('#divHomeWork').css('display') == 'block') {
                    $('#divChkShowCompleteTS').stop().show(300);
                    $('#divShowData').stop().hide(300);
                }
            }
            isPostback = "False";
        }

        function TickChkIsCompleteHM(e) {

            var frm = document.getElementById('<%=Page.Form.ClientID %>');
            frm.submit();

            //ToggleHomeWork();

            ////if ($('#divHomeWork').css('display') == 'none') {
            //$('#divHomeWork').stop().show(500);
            //$('#divChkShowCompleteTS').stop().show(300);
            //$('#divShowData').stop().hide(300);

            ////}




        }

        function ChengeColorTr() {
            var i = 1;

            $('.bordered').find('tr').each(function () {

                if (i >= 1) {
                    var td = $(this).find("td");
                    if (i % 2 == 0) {
                        $(td).css('background', '#FFFFCC');
                    } else {
                        $(td).css('background', '#FFFFFF');
                    }
                }
                i = i + 1;
            });
            $('.imgPlayQuiz').hide();
            $('.UserImage').css('display', 'none');
            $('.imgPlayHomeWork').hide();
        }

        function TriggerTrClick(e) {
            var obj1 = e.target;
            var obj = $(obj1).parent();
            //ถ้ากดคลิกที่รูป Avatar จะต้องเปิดหน้า Comment
            if ($(e.target).is('.UserImage')) {
                var winnerId = $(e.target).attr('winnerId');
                $.fancybox({
                    'autoScale': true,
                    'transitionIn': 'none',
                    'transitionOut': 'none',
                    'href': '<%=ResolveUrl("~")%>Student/AvatarComment.aspx?AVT_From=' + stdId + '&AVT_To=' + winnerId,
                    'type': 'iframe',
                    'width': 750,
                    'minHeight': 450
                });
            }
            else {
                var id = $(obj).attr('id');
                if (id != undefined) {
                    ChengeColorTr();

                    //$(this).children('td').addClass('forPlay');
                    $(obj).children('td').css('background', '#95FF62');
                    if (id !== '00000000-0000-0000-0000-000000000000') {
                        $('#play_' + id).show();

                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/GetAvatarPic",
                            data: "{ Item_Id: '" + id + "'}",
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (data) {
                                if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                                    var ShowDialog = jQuery.parseJSON(data.d);
                                    var IsShowImgWinner = ShowDialog.IsShowImgWinner;
                                    var imgWinner = ShowDialog.imgWinner;
                                    var StdWinnerId = ShowDialog.StdWinnerId;

                                    if (IsShowImgWinner == 'True') {
                                        $('#User_' + id).attr('src', imgWinner);
                                        $('#User_' + id).css('display', 'block');
                                        $('#User_' + id).attr('winnerId', StdWinnerId);
                                    }
                                }
                            },
                            error: function myfunction(request, status) {
                                //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');	                
                            }
                        });
                    } else {
                        $.ajax({
                            type: "POST",
                            url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/SetValueToSessionShowFullPractice",
                            async: false,
                            contentType: "application/json; charset=utf-8", dataType: "json",
                            success: function (msg) {
                                window.location = window.location.href;
                            },
                            error: function myfunction(request, status) {
                            }
                        });
                    }
                }
            }
        }

        function TriggerClickPlayQuiz(e) {
            var obj = e.target;
            var id = $(obj).attr('id');
            id = id.replace('play_', '');
            SaveQuiz(id);
        }

        function TriggerClickPlayHomework(e) {
            var obj = e.target;
            var id = $(obj).attr('id');
            id = id.replace('play_', '');
            var item = new Array();
            item = id.split('_');
            SaveHomework(item[0], item[1]);
        }

        function TriggerHomeBarClick(e) {
            var obj = e.target;
            var id = $(obj).attr('id');
            if (id == 'ctl00_MainContent_chkShowCompleteTS') {
                TickChkIsCompleteHM();
            } else {
                ToggleHomeWork();
            }

        }

        function TriggerPracticeBarClick() {
            ToggleBox();
        }

        function TriggerPracticeStandardBarClick() {
            alert(350);
            document.location.href = '../PracticeMode_Pad/ChooseClass.aspx?DashboardMode=9';
        }

        function TriggerStudentDetailBarClick() {
            document.location.href = '../Student/StudentDetailPage.aspx';
        }

    </script>
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyPhoto.css" type="text/css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyPhoto.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 800,
                default_height: 600,
                modal: true, social_tools: false
            });
            $.prettyLoader();
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //ToggleHomeWork()
            //$('tr').click(function (e) {
            //ถ้ากดคลิกที่รูป Avatar จะต้องเปิดหน้า Comment
            //if ($(e.target).is('.UserImage')) {
            //    var winnerId =  $(e.target).attr('winnerId');
            //    $.fancybox({
            //        'autoScale': true,
            //        'transitionIn': 'none',
            //        'transitionOut': 'none',
            //        'href': '<%=ResolveUrl("~")%>Student/AvatarComment.aspx?AVT_From=' + stdId + '&AVT_To=' + winnerId ,
            //        'type': 'iframe',
            //        'width': 750,
            //        'minHeight': 450
            //       });
            //}
            //else {
            //    var id = $(this).attr('id');
            //    if (id != undefined) {
            //        ChengeColorTr();

            //        //$(this).children('td').addClass('forPlay');
            //        $(this).children('td').css('background', '#95FF62');
            //        if (id !== '00000000-0000-0000-0000-000000000000') {
            //            $('#play_' + id).show();

            //            $.ajax({
            //                type: "POST",
            //                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/GetAvatarPic",
            //        data: "{ Item_Id: '" + id + "'}",
            //        contentType: "application/json; charset=utf-8", dataType: "json",
            //        success: function (data) {
            //            if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
            //                var ShowDialog = jQuery.parseJSON(data.d);
            //                var IsShowImgWinner = ShowDialog.IsShowImgWinner;
            //                var imgWinner = ShowDialog.imgWinner;
            //                var StdWinnerId = ShowDialog.StdWinnerId;

            //                if (IsShowImgWinner == 'True') {
            //                    $('#User_' + id).attr('src', imgWinner);
            //                    $('#User_' + id).css('display', 'block');
            //                    $('#User_' + id).attr('winnerId', StdWinnerId);
            //                }
            //            }
            //        },
            //        error: function myfunction(request, status) {
            //            //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');	                
            //        }
            //        });
            //        };
            //    }
            //}

            //});

            //$('.imgPlayQuiz').click(function(){

            //    var id = $(this).attr('id');
            //    id=id.replace('play_','');
            //     SaveQuiz(id);
            //    });

            //$('.imgPlayHomeWork').click(function(){

            //    var id = $(this).attr('id');
            //    id=id.replace('play_','');

            //    var item = new Array();
            //    item = id.split('_');
            //     SaveHomework(item[0],item[1]);
            //    });
            //getnextaction ตอนท้ายเพจ   
            //getNextAction();


            //เปิดหน้าข่าวโรงเรียน
            <%If Page.IsPostBack = False Then%>
            var NeedShowTip = '<%=NeedShowTip%>';
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/DashboardSignalRService.asmx/CheckIsHaveCurrentNews",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d == 'Show') {
                        var w_default = 800;
                        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                            var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                            if (ww > 800) {
                                w_default = 1000;
                            }
                        }
                        setTimeout(function () {
                            $.fancybox({
                                'autoScale': true,
                                'transitionIn': 'none',
                                'transitionOut': 'none',
                                'href': '<%=ResolveUrl("~")%>SchoolNewsPage.aspx?IsStudent=True',
                                'type': 'iframe',
                                'width': w_default,
                                'minHeight': 450,
                                'afterClose': function () {
                                    if (NeedShowTip == 'True') {
                                        ShowTips();
                                    }
                                }
                            });
                        }, 100)
                    } else {
                        if (NeedShowTip == 'True') {
                            setTimeout(function () {
                                ShowTips();
                            }, 1000);
                        }
                    }
                },
                error: function myfunction(request, status) {
                    alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                }
            });
                  <%End If%>

            //เมื่อกดที่ Avatar จะเปิดหน้า Comment
            //$('.UserImage').live('click', function () {
            //    alert($(this).attr('winnerId'));
            //});

        });

        function SaveQuiz(ItemId) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/SaveQuiz",
                async: false,
                data: "{ ItemId :  '" + ItemId + "',Status : '4', IsHomework :  '0'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        valReturnFromCodeBehide = msg.d;
                        window.location = msg.d;
                    }
                },
                error: function myfunction(request, status) {

                }
            });
        }

        function SaveHomework(ItemId, Status) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseTestset.aspx/SaveQuiz",
                async: false,
                data: "{ ItemId :  '" + ItemId + "',Status :  '" + Status + "', IsHomework :  '1'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    valReturnFromCodeBehide = msg.d;
                    window.location.href = msg.d;
                },
                error: function myfunction(request, status) {

                }
            });
        }

        //        function getNextAction() {       
        //         var DeviceId = '<%=DVID %>';
        //            $.ajax({ type: "POST",
        //	            url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/SendToGetNextAction",
        //	            data: "{ DeviceUniqueID: '" + DeviceId + "',IsFirstTime:'NoValue',IsTeacher:false}",
        //	            contentType: "application/json; charset=utf-8", dataType: "json",   
        //	            success: function (msg) {
        //                //ถ้าค่าที่ Return กลับมาไม่เป็นค่าว่างให้ ReLoad หน้าใหม่
        //                var objJson = jQuery.parseJSON(msg.d);
        //                if (objJson.Param.NextURL !== '') {
        //                    //window.location = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
        ////                    if (objJson.Param.NextURL == '/Activity/ActivityPage_Pad.aspx?i=t&DeviceUniqueID=' + DeviceId) { 
        ////                        var joinQuiz = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
        ////                        dialogInterrupt(joinQuiz);
        ////                    }
        //                     if (objJson.Param.NextURL == '/Activity/ActivityPage_Pad.aspx?i=t') { 
        //                        var joinQuiz = '../Activity/ActivityPage_Pad.aspx?DeviceUniqueID=' + DeviceId;
        //                        dialogInterrupt(joinQuiz);
        //                    }
        //                }
        //	            },
        //	            error: function myfunction(request, status)  {
        //                alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        //	            }
        //	        });
        //        }
    </script>
    <%--<link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        //var NeedShowTip = '<%=NeedShowTip%>';
        //$(function () {
        //    if (NeedShowTip == 'True') {
        //        setTimeout(function () {
        //            ShowTips();
        //        }, 3000);
        //    }
        //});

        function ShowTips() {
            var elm = ['label.Homework', '.PracticeFromTeacher', '.Practice', '.Report'];
            var tipPosition = ['leftMiddle'];
            var tipTarget = ['rightMiddle'];
            var tipContent = ['การบ้านที่ครูสั่ง', 'เข้าทำฝึกฝนที่ครูจัดไว้', 'ฝึกทำข้อสอบทั่วไป', 'ดูประวัติการบ้าน, ควิซ, ฝึกฝน'];
            var tipAjust = [0];
            var w = [150, 200, 180, 260];
            for (var i = 0; i < elm.length; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[0], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
                    }, hide: false,
                    position: { corner: { tooltip: tipPosition[0], target: tipTarget[0] }, adjust: { x: tipAjust[0] } },
                    fixed: false
                });
            }
            DestroyTips(elm);
        }
        function DestroyTips(elm) {
            setTimeout(function () {
                for (var i = 0; i < elm.length; i++) {
                    $(elm[i]).qtip('destroy');
                }
            }, 5000);
        }

    </script>
</asp:Content>
