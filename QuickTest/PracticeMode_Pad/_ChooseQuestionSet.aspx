<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="ChooseQuestionSet.aspx.vb" Inherits="QuickTest.ChooseQuestionSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<%--    <link charset="utf-8" media="screen" type="text/css" href="<%=ResolveUrl("~")%>css/aa.css" rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~")%>css/general.css" />
    <link rel="stylesheet" type="text/css" href="../shadowbox/shadowbox.css" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css" />--%>

    <%If IsMobile = True Then%>
        <style type="text/css">
        #main {
            width: 954px !important;
            height: auto !important;
            margin: 20px auto !important;
        }

        #site_content {
            width: 930px !important;
        }

        #divListExam {
            width: 905px !important;
            height: 450px !important;
        }

        #lblCheck44 {
            margin-left: 25px !important;
        }

        table tr td {
            font-size: 30px !important;
        }

        .imgPlayQuiz {
            width: 160px !important;
            height: 160px !important;
        }

        #btnBack {
            width: 90px !important;
            height: 90px !important;
        }

        #lblCheck51, #lblCheck44 {
            font-size: 28px !important;
        }

        nav {
            display: none !important;
        }

        span.lblMaxOnet:before {
            content: "(";
        }

        span.lblMaxOnet::after {
            content: ")";
        }

        span.lblMaxOnet {
            color: orange;
            margin-left: 20px;
            font-weight: bold;
            font-size: 2em;
          
        }
    </style>
    <%End If %>

    <%If Not IsMaxOnet Then %>
        <style type="text/css">
        #site_content {
            border-radius: 10px;
        }

        nav {
            display: none !important;
        }
    </style>
    <%End If %>
       <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
        <style type="text/css">
            html {
                background-image:none !important;
            }

            body {
                background: url(../images/bg/res19201080/chooseclass.png) !important;
            }
    </style>
    <%End If %>
    <style type="text/css">
        table tr td {
            position: relative;
        }

        tbody.on {
            display: table-row-group;
        }

        tbody.off {
            display: none;
        }

        .toolsTip {
            position: absolute;
            border: 1px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            padding: 5px;
            width: 700px;
            font-size: 16px;
            margin: 0px auto;
        }

        .TopRight {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            top: 0px;
            right: 0px;
            -moz-border-radius: 15px;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            margin: 3px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .ForDescription {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            -moz-border-radius: 15px;
            color: #000000;
            padding: 10px;
            padding-bottom: 5px;
            padding-top: 5px;
            display: none;
            margin: 3px;
            top: 0px;
            left: 0px;
            -webkit-border-radius: 15px;
            border-radius: 15px;
            behavior: url(/css/PIE.htc);
        }

        .imgPlayQuiz {
            position: absolute;
            width: 70px;
            height: 70px;
            display: none;
            margin: auto;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
        }

        .UserImage {
            float: right;
            cursor: pointer;
            display: none;
            width: 40px;
            height: 60px;
            margin-right: 20px;
        }

        #site_content {
            position: relative;
        }

        .divBackBtn > input {
            width: 55px !important;
            height: 55px !important;
            top: 20px;
        }

        div.GotoContact{
                width: 70px;
                height: 70px;
                cursor: pointer;
                background-image: url(../images/maxonet/post.png);
                background-size: 70px;
                display: inline-block;
                float: right;
                margin-top: -10px;
        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <form runat="server" id='ChooseQuestionSet'>

        <nav style='position: relative; top: 50px; width: 650px; height: 50px;'>
        <div id="menu_container">
            <span id='spn' style="font-size:20px;color:Black;"></span>
            <ul class="sf-menu" id="nav" stylesheet="text-align:center;" style="font-size:<%=ChkFontSize %>;margin-left:<%=MarginSize %>;width:570px;">
                <asp:ImageButton ID="imgHome" ImageUrl="~/Images/Home.png" style='float:left;margin-left:-45px;margin-right:15px;margin-top:3px;' runat="server"></asp:ImageButton>

                <%If RedirectMode = "1" Or RedirectMode = "2" Then%>
                    <li id="liChooseClass"><a href="#" style='cursor:not-allowed;padding:<%=PaddingSize%>;' ><%=txtStep1 %></a></li>
                <%else %>
                    <li id="liChooseClass"><a onclick="ToChooseClass()" style='cursor:pointer;padding:<%=PaddingSize%>;'><%=txtStep1 %></a></li>
                <%End If%>

                <%If RedirectMode = "1" Or RedirectMode = "3" Then%>
                    <li id="liChooseSubject"><a href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep2 %></a></li> 
                <%else %>
                    <li id="liChooseSubject"><a onclick="ToChooseSubject()" style="cursor:pointer; padding:<%=PaddingSize%>;"><%=txtStep2 %></a></li> 
                <%End If%>

                <li id="liChooseQuestionset"><a class="current" href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep3 %></a></li>	
                <%If Session("ChooseMode") = 2 Or Session("ChooseMode") = 4 Then%>
                    <li id="liChooseHomework"><a href="#" style="cursor:not-allowed;padding:<%=PaddingSize%>;"><%=txtStep4 %></a></li>	
                <%End If%>
            </ul>
        </div>
    </nav>

        <div id="main" style="width: 954px; height: auto; position: relative; margin: 70px auto; text-align: center;">
            <div id="site_content">
                <div style="margin-top: 10px;">
                    <div class="GotoContact" id ="GotoContact"></div>
                    <span style="font-size: 2em; color: orange;">เลือกชุดข้อสอบ</span>
                    <%If IsMaxOnet Then %>
                    <span class="lblMaxOnet" style="display:none!important;"><%= SubjectName %></span>
                    <% End If %>
                    <div class="content" style="width: 930px; padding: 0px; margin: 0; float: initial;">
                        <div id="DivchkShow51" style="width: 78px; display: none;">
                            <input type="checkbox" <%= GetYearChecked(51)%> name='Show51' id='chkShow51' <%--onclick='CheckShow51();'--%> />
                            <label for='chkShow51' id="lblCheck51" style='color: black; margin-left: 37px;'>
                                51</label>
                        </div>
                        <div id="DivchkShow44" style="display: none;">
                            <input type="checkbox" <%= GetYearChecked(44)%> name='Show44' id='chkShow44' <%--onclick='CheckShow44();'--%> />
                            <label for='chkShow44' id="lblCheck44" style='color: black;'>
                                44  ดับเบิ้ลคลิกแถบเพื่อสร้างข้อสอบจากหน่วยที่ต้องการ
                            </label>
                        </div>

                        <div id="divListExam" clientidmode="Static" runat="server" class="" style='text-align: center; position: relative; border-color: #D3F2F7; border-style: solid; border-radius: 0.5em; visibility: visible; background: #D3F2F7; width: 900px; overflow-x: hidden; height: 450px; overflow-y: scroll; top: 5px; padding-left: 10px; padding-right: 10px;'
                            runat="server">
                        </div>
                    </div>
                    <div class="divBackBtn">
                        <%--<asp:Button ID="btnBack" ClientIDMode="Static" runat="server" class="" Text=""
                            Style="width: 70px; height: 70px; line-height: 40px; position: relative; margin: 0; border: 0;"></asp:Button>--%>
                        <input type="button" id="btnBack" style="width: 70px; height: 70px; line-height: 40px; position: relative; margin: 0; border: 0;" />
                    </div>
                </div>
            </div>
            <img class="clock" src="../Images/Maxonet/clock.png" style="display: none;" />
        </div>

        <div id='HowToDialog' style="display: none; width: 100%; height: 100%; z-index: 0; position: fixed; top: 0px; left: 0px; background-color: Black">
            <iframe id="FrameShowHowTo" scrolling="no" style="overflow: hidden; white-space: nowrap; width: 100%; height: 100%; position: relative; margin-left: auto; margin-right: auto; z-index: 0;"
                frameborder="0"></iframe>
            <ul id="CloseHelp">
                <li class="about1" style="z-index: 999;"><a title="จบการฝึกฝน" id="CloseHelp" onclick="CloseHelpSelect();">จบการ<br />
                    ฝึกฝน </a></li>
            </ul>
        </div>

        <div id="dialog" title="ต้องเข้าควิซแล้วค่ะ"> </div>

            <div id="dialog2" class="dialogSession"></div>

        <span class="TopRight" id="SpanFullDetail"></span>

    </form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~")%>js/jquery.js"></script>
<%--    <script type="text/javascript" src="../shadowbox/shadowbox.js"></script>--%>

    <% If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
      <%--  <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
        <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <% End If %>
       
    <script type="text/javascript">

        var ChooseSubjectURL = '<%=ChooseSubjectURL%>';
        var ChooseClassURL = '<%=ChooseClassURL%>';

        function toggleNumQstn(mid, classid, subjectid) {
            //              if (document.getElementById('MID' + mid).checked) {
            //                  document.getElementById('spnSelected_' + mid).innerHTML = document.getElementById('spnTotal_' + mid).innerHTML;
            //                  
            //              }
            //              else if(document.getElementById('MID' + mid).checked == false){
            //              var text = '';
            //                  document.getElementById('spnSelected_' + mid).innerHTML = text;
            //                 
            //              }
            SumQuestionSelected();
            //              document.getElementById('spnTotalSubjectsSelected').innerHTML = String($.unique(selectedSubject).length);
            //              document.getElementById('spnTotalClassesSelected').innerHTML = String($.unique(selectedClass).length);
        };

        function callQtip(id, content) {
            $('label[for="' + id + '"]').stop().qtip({
                content: content.toString(),
                show: { ready: true },
                style: {
                    width: 250, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'leftMiddle', name: 'dark', 'font-weight': 'bold'
                },
                position: { corner: { tooltip: 'leftMiddle', target: 'rightMiddle' } },
                hide: false
            });
        }

        function destroyQtip(id) {
            setTimeout(function () { $('label[for="' + id + '"]').qtip('destroy'); checkOverQuestion(id); }, 4000);
            var countQtip = $('#CountQtip').val();
            $('#CountQtip').val(parseInt(countQtip) + 1);
            //alert($('#CountQtip').val());            
        }

        function ToChooseSubject() {
            window.location = ChooseSubjectURL;
        }

        function ToChooseClass() {
            window.location = ChooseClassURL;
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
    </script>

    <script type="text/javascript">

        var isMaxOnet = '<%=IsMaxOnet%>';

        Shadowbox.init({
            skipSetup: false
        });

        $(function () {
                <% If Session("ChooseMode") = 2 Or Session("ChooseMode") = 4 Then%>
            $('#liChooseClass').css('width', '120px');
            $('#liChooseSubject').css('width', '130px');
            $('#liChooseQuestionset').css('width', '158px');
            $('#liChooseHomework').css('width', '135px');
                <%Else%>
            $('#liChooseClass').css('width', '145px');
            $('#liChooseSubject').css('width', '150px');
            $('#liChooseQuestionset').css('width', '145px');
                <% End If%>

        });

         function GotoContact() {
                    Shadowbox.open({
                        content: '<div style="overflow: hidden;white-space: nowrap; background-color:white;"><iframe scrolling="no" style="overflow: hidden;white-space: nowrap; " src="<%=ResolveUrl("~")%>Contact.aspx" frameborder="0" width="700" height="570"></iframe></div>',
                        player: "html",
                        title: "ติดต่อเรา",
                        height: 570,
                        width: 700
                    });
         };

        function TriggerBackClick(e) {
           
            if (isMaxOnet === 'True') {
                openBlockUI();
                backSound.play();
                refreshUrl = "<%=BackUrl%>";
                console.log(refreshUrl);
                //if (responseTimeCI != 0) { e.preventDefault(); return 0; }
                window.location = refreshUrl;
            } else {
                   window.location = "../PracticeMode_Pad/Choosesubject.aspx?DashboardMode=<%= Session("ChooseMode")%>";
            }
            var obj = e.target;
            $(obj).trigger('click');
        }

        function TriggerChk51() {
            if ($('#chkShow51').attr('checked') == 'checked') {
                $('#chkShow51').removeAttr('checked');
            }
            else {
                $('#chkShow51').attr('checked', 'checked');
            }
            CheckShow51();
        }

        function TriggerChk44() {
            if ($('#chkShow44').attr('checked') == 'checked') {
                $('#chkShow44').removeAttr('checked');
            }
            else {
                $('#chkShow44').attr('checked', 'checked');
            }
            CheckShow44();
        }

        function ToggleBox() {
            $("#divListing").slideToggle("slow");
            if ($.browser.msie) {
                if ($.browser.version <= 7) {
                    $('#divListing').css('overflow', 'auto');
                }
            }
        }
        function ChengeColorTr(InputId) {
            //            $('.bordered').find('tr').each(function () {
            //                var td = $(this).find("td");
            //                //$(td).css('background', '#D3F2F7');
            //            });
            $('#' + InputId).children('td').css('background-color', '#17FFBE');
            $('.imgPlayQuiz').hide();
            $('.UserImage').css('display', 'none');
        }
        //function TriggerClick(e) {
        //    var obj = e.target;
        //    $(obj).trigger('click');
        //}
        function TriggerTRClick(e) {
            //if (isMaxOnet === 'True') { chooseSound.play(); }
            var obj;
            if ($(e.target).is('td')) {
                var obj1 = $(e.target);
                obj = $(obj1).parent();
            }
            else {
                var obj1 = $(e.target);
                obj = $(obj1).parent().parent();
            }
            var id = $(obj).attr('id');
            //alert(id);
            //Get ค่าว่ามีตัวก่อนหน้าที่ต้องเปลี่ยนสีกลับเป็นสีเดิมหรือเปล่า
            var oldElement = GetColorFirstObj();
            if (oldElement !== 'Null') {
                ChangeColorOldElement(oldElement);
                SetColorFirstObj(id);
            }
            else {
                SetColorFirstObj(id);
            }
            ChengeColorTr(id);
            //$(this).children('td').addClass('forPlay');
            //$(this).children('td').css('background', '#17FFBE');
            $('#play_' + id).show();

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseQuestionSet.aspx/GetAvatarPic",
                async: false,
                data: "{ Item_Id: '" + id + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        var ShowDialog = jQuery.parseJSON(data.d);
                        var IsShowImgWinner = ShowDialog.IsShowImgWinner;
                        var imgWinner = ShowDialog.imgWinner;

                        if (IsShowImgWinner == 'True') {
                            $('#User_' + id).attr('src', imgWinner);
                            $('#User_' + id).css('display', 'block');

                        }
                    }
                },
                error: function myfunction(request, status) {
                    //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
                }
            });
        }
        function TriggerImgPlayQuiz(e) {
            var obj = e.target;
            var id = $(obj).attr('id');
            id = id.replace('play_', '');
            var urlToQuiz = SaveQuiz(id);
            if (urlToQuiz != 0) {
                if (isMaxOnet === 'True') {
                    openBlockUI();
                    //btnPlaySound.play();
                    refreshUrl = urlToQuiz;
                    if (responseTimeCI != 0) { return 0; }
                }
                window.location = urlToQuiz;
            }
        }
    </script>

    <script type="text/javascript">
        var FirstObj = 'Null';
        $(document).ready(function () {
            //$('tr').click(function () {
            //    var id = $(this).attr('id');
            //alert(id);
            //Get ค่าว่ามีตัวก่อนหน้าที่ต้องเปลี่ยนสีกลับเป็นสีเดิมหรือเปล่า
            //var oldElement = GetColorFirstObj();
            //if (oldElement !== 'Null') {
            //    ChangeColorOldElement(oldElement);
            //    SetColorFirstObj(id);
            //}
            //else {
            //    SetColorFirstObj(id);
            //}
            //ChengeColorTr(id);
            //$(this).children('td').addClass('forPlay');
            //$(this).children('td').css('background', '#17FFBE');
            //$('#play_' + id).show();

            //$.ajax({
            //    type: "POST",
            //  url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseQuestionSet.aspx/GetAvatarPic",
            //        async: false,
            //        data: "{ Item_Id: '" + id + "'}",
            //        contentType: "application/json; charset=utf-8", dataType: "json",
            //        success: function (data) {
            //            if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
            //                var ShowDialog = jQuery.parseJSON(data.d);
            //                var IsShowImgWinner = ShowDialog.IsShowImgWinner;
            //                var imgWinner = ShowDialog.imgWinner;

            //                if (IsShowImgWinner == 'True') {
            //                    $('#User_' + id).attr('src', imgWinner);
            //                    $('#User_' + id).css('display', 'block');

            //                }
            //            }
            //        },
            //        error: function myfunction(request, status) {
            //            //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
            //        }
            //    });

            //});

            //$('.imgPlayQuiz').click(function () {
            //    var id = $(this).attr('id');
            //    id = id.replace('play_', '');
            //    SaveQuiz(id);
            //});
        });
        function SetColorFirstObj(InputObj) {
            var MergeStr = $('#' + InputObj).children('td').css('background-color');
            FirstObj = InputObj + '|' + MergeStr;
        }
        function ChangeColorOldElement(InputValue) {
            var Splitestr = InputValue.split('|');
            var ElementId = Splitestr[0];
            var color = Splitestr[1];
            $('#' + ElementId).children('td').css('background-color', color);
        }
    </script>
    
    <script type="text/javascript">
        var TokenId = '<%=TokenId%>';
        function SaveQuiz(QsetId) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseQuestionSet.aspx/SaveTestset",
                async: false,
                data: "{ QsetId :  '" + QsetId + "', TokenId : '" + TokenId + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                        valReturnFromCodeBehide = data.d;
                        if (!isAndroid) {
                            FadePageTransitionOut();
                        }
                        //window.location = valReturnFromCodeBehide;
                    }
                },
                error: function myfunction(request, status) {
                    //alert('error');
                }
            });
            return valReturnFromCodeBehide;
        }
        function GetColorFirstObj() {
            return FirstObj;
        }
    </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            //alert(sumSpnSelect);
            // toolstip
            $('span.SpanMore').hover(function (e) {
                var id = $(this).parent().parent().attr('id');
                console.log('qset id = ' + id);
                if (id === undefined) return 0;
                //alert(id);
                callTooltip('#SpanFullDetail', e);
                //$('#SpanFullDetail').html(id);
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseQuestionSet.aspx/getQuestionSetName",
                    data: "{ qSetId : '" + id + "' }",  //" 
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (msg) {
                        if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            $('#SpanFullDetail').html(msg.d);
                            var widthSpan = $('#SpanFullDetail').width();
                            widthSpan = widthSpan / 2;
                            var heightSpan = $('#SpanFullDetail').height();
                            heightSpan = heightSpan / 2;
                            $('#SpanFullDetail').css('left', '50%').css('margin-left', -widthSpan + 'px').css('top', '50%').css('margin-top', -heightSpan + 'px');
                            $('#SpanFullDetail').css('width', (widthSpan * 2) + 'px');
                            $('#SpanFullDetail').html(msg.d);
                            $('#SpanFullDetail').show();
                            //alert('success'+valReturnFromCodeBehide);
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('shin' + request.statusText + status);    
                    }
                });
            }, function () {
                $("#SpanFullDetail").mouseleave(function () {
                    $('#SpanFullDetail').stop(true, true).fadeOut('slow');
                });

                $('#SpanFullDetail').stop(true, true).fadeOut('slow');
            });
            $(window).scroll(function () {
                //$('#SpanFullDetail').stop(true,true).fadeOut('slow');
                //alert('sdgsd');
            });
            //            $('body').click(function(){
            //                var d = $(this).find('qtip');
            //                alert(d);
            //            });
        });

        function callTooltip(obj, e) {
            var tip = $(this).find('toolsTip');
            var locateX = e.pageX + 20;
            var locateY = e.pageY + 20;
            locateX += 10;
            locateY -= 50;
            //           $(obj).css({ left: locateX, top: locateY }).delay(1000).fadeIn();
            $("#SpanFullDetail").mouseenter(function () {
                $('#SpanFullDetail').stop(true, true).delay(800).fadeIn('slow');
            });
            $(obj).stop(true, true).delay(800).fadeIn('slow');
            //$(obj).css({ left: locateX, top: locateY }).delay(1000).show();          
        }
        $('.aTag').hover(function () {
            $("#DescriptionDiv").stop(true, true).delay(800).fadeIn('slow');
        },
        function () {
            $("#DescriptionDiv").stop(true, true).fadeOut('slow');
        });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">

    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/CheckAndSetNowDevice.js"></script>

    <% If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
<%--        <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
        <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <% End If %>

    <%If Not IE = "1" Then%>
        <%If Not Session("selectedSession") = "PracticeFromComputer" Then%>
            <%If Session("IsTeacher") = "1" Then%>
                <% If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
                 <%--   <script src='../js/DashboardSignalR.js' type="text/javascript"></script>--%>
                <% end If %>
            <%Else%>
                <script type="text/javascript">

                    var IsPracticeFromComputer = '<%=Session("PracticeFromComputer") %>';

                    if (IsPracticeFromComputer == "False") {
                        var GroupName = "<%=GroupName %>";
                        var DeviceId = '<%=DVID %>';
                    }
                </script>

                <% If BusinessTablet360.ClsKNSession.NeedSignalR Then %>
                  <%--  <script src="../js/StudentSignalR.js" type="text/javascript"></script>--%>
                <%End If %>
            <%End If%>
        <%End If%>
    <%End If%>

    <script type="text/javascript">
        $(document).ready(function () {
            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;
            if (isAndroid) {
                //var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                //var mw = 480; // min width of site
                //var ratio = ww / mw; //calculate ratio
                //if (ww < mw) { //smaller than minimum size
                //    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                //} else { //regular size
                //    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                //}

                //$('#main').css({'width' : '880px','height': '530px'});
                //$('#site_content').css('width', '840px');
                //$('#divListExam').css({'width':'805px','height': '350px'});
                //$('#lblCheck44').css('margin-left', '25px');
                //$('table tr td').css('font-size', '32px');
                //$('.imgPlayQuiz').css({ 'width': '160px', 'height': '160px', 'right': '42%' });
                //$('#btnBack').css({ 'width': '230px', 'font-size': '35px', 'height': '70px' });
                //$('#lblCheck51').css('font-size', '28px');
                //$('#lblCheck44').css('font-size', '28px');
                //$('nav').hide();
            }

            var DeId = '<%=DVID %>';
            var TokenId = '<%=TokenId%>';
            var baseURL = "<%=ResolveUrl("~")%>";
            var UserId = '<%=player_Id%>'
            console.log(DeId);
            console.log(TokenId);
            console.log(baseURL);

            $(document).ready(function () {

            var CurrentPageStatus = '<%=Session("SessionStatus")%>'
            console.log(CurrentPageStatus);
            var ButtonName = event.path[1].id;
            if (ButtonName != "NotChange") {
                if (CurrentPageStatus == 1) {
                    CheckIsNowDevice(DeId, TokenId, baseURL, UserId);
                }
            }
        });

        window.addEventListener("click", function (event) {
            var ButtonName = event.path[1].id;
            console.log("Clicked" + ButtonName);
            if (ButtonName != "NotChange") {
                CheckIsNowDevice(DeId, TokenId, baseURL, UserId);
            }
        });
        });

        function NewFastButton() {
            //ปุ่ม กลับ
            new FastButton(document.getElementById('btnBack'), TriggerBackClick);

            //chkbox
            new FastButton(document.getElementById('chkShow51'), CheckShow51);
            new FastButton(document.getElementById('chkShow44'), CheckShow44);

            //tr Click 
            $('tr').each(function () {
                new FastButton(this, TriggerTRClick);
            });

            //ปุ่ม Play เล่น Quiz
            $('.imgPlayQuiz').each(function () {
                new FastButton(this, TriggerImgPlayQuiz);
            });

            //chkbox 51 , 44
            if (isAndroid) {
                new FastButton(document.getElementById('DivchkShow51'), TriggerChk51);
                new FastButton(document.getElementById('DivchkShow44'), TriggerChk44);
            }

            $('#GotoContact').each(function () {
                new FastButton(document.getElementById('GotoContact'), GotoContact);
            });
        }

        function DeleteFastButton() {
            $("#ChooseQuestionSet").off("click", "#GotoContact", GotoContact);
            $("#ChooseQuestionSet").off("touchstart", "#GotoContact", GotoContact);
            $("#main").off("click", "#btnBack", TriggerBackClick);
            $("#main").off("touchstart", "#btnBack", TriggerBackClick);
            $("#ChooseQuestionSet").off("click", "tr", TriggerTRClick);
            $("#ChooseQuestionSet").off("touchstart", "tr", TriggerTRClick);
            $(".imgPlayQuiz").off("click", ".imgPlayQuiz", TriggerImgPlayQuiz);
            $(".imgPlayQuiz").off("touchstart", ".imgPlayQuiz", TriggerImgPlayQuiz);
        }

        function CheckShow51() {
            var frm = document.getElementById('<%=Page.Form.ClientID %>');
            if ($('#chkShow51').attr('checked') == 'checked') {
                frm.submit();
                //                $('#HdChkOnLoad').val('chkTrue');
            }
            else {
                frm.submit();
                ChooseQuestionSet.submit();
                //                $('#HdChkOnLoad').val('chkFalse'); 
            }
        }

        function CheckShow44() {
            var frm = document.getElementById('<%=Page.Form.ClientID %>');
            if ($('#CheckShow44').attr('checked') == 'checked') {
                frm.submit();
                //                $('#HdChkOnLoad').val('chkTrue');
            }
            else {
                frm.submit();
                ChooseQuestionSet.submit();
                //                $('#HdChkOnLoad').val('chkFalse'); 
            }
        }
    </script>
    <script type="text/javascript">
        var NeedShowTip = '<%=NeedShowTip%>';
        $(function () {
            if (NeedShowTip == 'True') {
                ShowTips();
            }
        });

        function ShowTips() {
            var elm = ['#divListExam', '#Help'];
            var tipPosition = ['topMiddle', 'leftMiddle'];
            var tipTarget = ['bottomMiddle', 'rightMiddle'];
            var tipContent = ['เลือกข้อสอบหน่วยที่จะเริ่มฝึกฝน', 'ศึกษาเพิ่มเติมดูที่นี่ค่ะ'];
            var tipAjust = [0, -50];
            var w = [280, 200];
            for (var i = 0; i < elm.length - 1; i++) {
                $(elm[i]).qtip({
                    content: tipContent[i],
                    show: { ready: true },
                    style: {
                        width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
                    }, hide: false,
                    position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { x: tipAjust[i] } },
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
