<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="RecommendPage.aspx.vb" Inherits="QuickTest.RecommendPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%If IsAndroid = True Then%>
    <style type="text/css">
        #btnRec {
            left: -200px !important;
        }

        #btnCon {
            left: -395px !important;
        }

        #btnOK {
            margin-top: -70px !important;
        }

        #main {
            height: 660px !important;
        }

        table tr td {
            font-size: 45px !important;
        }

        .imgPlayQuiz {
            width: 160px !important;
            height: 160px !important;
            margin-right: -85px !important;
        }
    </style>
    <%End If%>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);

        .ForSmallDetailDiv {
            border-radius: 3px;
            background-color: #B2DB48;
            margin: 8px 5px 5px 5px;
            font-size: 20px;
            display: inline-block;
            border: solid 1px;
            padding: 0px 15px 0px 15px;
        }

        .imgPlayQuiz {
            position: absolute;
            right: 50%;
            margin-right: -25px;
            width: 70px;
            height: 70px;
            display: none;
        }

        .ChangeReccommend {
            font: 100% 'THSarabunNew';
            font-size: 18px;
            margin-top: 10px;
            border-radius: 0.7em;
            width: 238px;
            position: relative;
            height: 60px;
            background-color: #FFA032;
        }

        #VirsualNav {
            height: 57px;
            width: 928px;
            margin: -20px auto 0 auto;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #DA7C0C;
            background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
        }

        .ForBtnAndroid {
            font-size: 25px !important;
            width: 230px !important;
            height: 70px !important;
            line-height: 70px !important;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
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

    </script>
    <form runat="server" id='formChooseTestSet'>
        <div id="main">
            <div id="site_content" style="width: 930px; height: 582px; padding: 12px;">
                <div id="logo_text" style=" margin-left: 20px;">
                    <h2>ทักษะที่ต้องปรับปรุง</h2>
                </div>
                <div class="content" style="width: 930px; margin-bottom: 25px;">
                    <section id="select">
	</section>

                    <div id='VirsualNav'>
                        <span id="spn" style="font-size: 20px; color: Black;"></span>
                        <div id='DivEvalution' style='width: 920px; overflow-x: scroll; height: 78px; overflow-y: hidden; white-space: nowrap; margin-left: 5px;' runat="server">
                        </div>

                    </div>
                </div>
                <%If ShowDivRecommend = True Then%>
                <div id="divReccommend" clientidmode="Static" runat="server" class="" style='text-align: center; position: relative; border-color: #D3F2F7; border-style: solid; border-radius: 0.5em; visibility: visible; background: #D3F2F7; width: 925px; overflow-x: hidden; height: 400px; overflow-y: scroll; white-space: nowrap; margin-top: 38px;'
                    runat="server">
                </div>
                <%End If%>
                <div id="divWithoutReccommend" runat="server" style='font-size:30px;height: 200px; background-color: white; overflow-y: initial; border: 1px dashed black; color: black; line-height: 6em; margin-top: 100px; font-weight: bold; width: 928px; text-align: center; '>
                    <%= txtWithoutReccommend%>
                </div>
            </div>
            <div class="form_settings" style="position: relative;">
                <asp:Button ID="btnRec" ClientIDMode="Static" runat="server" Text="ข้อสอบที่แนะนำ" class="submit" Style="float: left; left: -167px; width: 170px; position: relative; margin-top: -40px;" />
                <asp:Button ID="btnCon" ClientIDMode="Static" runat="server" Text="ข้อสอบที่เกี่ยวข้อง" class="submit"
                    Style="float: left; left: 16px; width: 170px; position: relative; margin-top: -40px;" />
                <asp:Button ID="btnOK" ClientIDMode="Static" runat="server" Text="กลับหน้าแรก" class="submit" Style="float: Right; margin-top: -40px; right: 50px; width: 145px; position: relative;" />
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/slimScroll.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">

        var FirstObj = 'Null';
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(document).ready(function () {
           

            //ปุ่ม ข้อสอบที่แนะนำ
            new FastButton(document.getElementById("btnRec"), TriggerServerButton);

            //ปุ่ม ข้อสอบที่เกี่ยวข้อง
            new FastButton(document.getElementById("btnCon"), TriggerServerButton);

            //ปุ่ม กลับหน้าแรก
            new FastButton(document.getElementById("btnOK"), TriggerServerButton);

            //tr click
            $('tr').each(function () {
                new FastButton(this, TriggerTrClick);
            });

            //Img ปุ่ม Play Quiz Click
            $('.imgPlayQuiz').each(function () {
                new FastButton(this, TriggerImgPlayQuizClick);
            });

            //ดักถ้าเป็น Android
            if (isAndroid) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                $('#btnRec').addClass('ForBtnAndroid');
                //$('#btnRec').css('left', '-200px');

                $('#btnCon').addClass('ForBtnAndroid');
                //$('#btnCon').css('left', '-395px');

                $('#btnOK').addClass('ForBtnAndroid');
                //$('#btnOK').css('margin-top', '-70px');

                //$('#main').css('height', '660px');
                //$('table tr td').css('font-size', '45px');
                //$('.imgPlayQuiz').live().css({ 'width': '160px', 'height': '160px','margin-right':'-85px'});

            }


            //$('tr').click(function () {
            //    var id = $(this).attr('id');

            //    // Get ค่าว่ามีตัวก่อนหน้าที่ต้องเปลี่ยนสีกลับเป็นสีเดิมหรือเปล่า
            //    var oldElement = GetColorFirstObj();

            //    if (oldElement !== 'Null') {

            //        ChangeColorOldElement(oldElement);
            //        SetColorFirstObj(id);

            //    }
            //    else {
            //        SetColorFirstObj(id);
            //    }
            //    ChengeColorTr(id);

            //    $(this).children('td').addClass('forPlay');
            //    $(this).children('td').css('background', '#17FFBE');
            //    $('#play_' + id).show();

            //});

            //$('.imgPlayQuiz').click(function () {

            //    var id = $(this).attr('id');
            //    id = id.replace('play_', '');
            //    SaveQuiz(id);
            //});

        });

        function GetColorFirstObj() {
            return FirstObj;
        }


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

        function ChengeColorTr(InputId) {
            //            $('.bordered').find('tr').each(function () {
            //                var td = $(this).find("td");

            //                //$(td).css('background', '#D3F2F7');

            //            });

            $('#' + InputId).children('td').css('background-color', '#17FFBE');
            $('.imgPlayQuiz').hide();
        }



        //function TriggerClickRec(e) {
        //    var obj = e.srcElement;
        //    $(obj).trigger('click');
        //}

        //function TriggerClickCon(e) {
        //    var obj = e.srcElement;
        //    $(obj).trigger('click');
        //}

        //function TriggerClickOk(e) {
        //    var obj = e.srcElement;
        //    $(obj).trigger('click');
        //}

        function TriggerImgPlayQuizClick(e) {
            var obj = e.target;
            var id = $(obj).attr('id');
            id = id.replace('play_', '');
            SaveQuiz(id);
        }

        function TriggerTrClick(e) {
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
            // Get ค่าว่ามีตัวก่อนหน้าที่ต้องเปลี่ยนสีกลับเป็นสีเดิมหรือเปล่า
            var oldElement = GetColorFirstObj();

            if (oldElement !== 'Null') {

                ChangeColorOldElement(oldElement);
                SetColorFirstObj(id);

            }
            else {
                SetColorFirstObj(id);
            }
            ChengeColorTr(id);

            $(obj).children('td').addClass('forPlay');
            $(obj).children('td').css('background', '#17FFBE');
            $('#play_' + id).show();
        }

        function SaveQuiz(ItemId) {
            var valReturnFromCodeBehide;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>PracticeMode_Pad/RecommendPage.aspx/SaveQuiz",
                  async: false,
                  data: "{ ItemId :  '" + ItemId + "'}",
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (msg) {
                      if (msg.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                          valReturnFromCodeBehide = msg.d;
                          window.location.href = msg.d;
                      }
                  },
                  error: function myfunction(request, status) {
                      //   alert('aaaaaa');
                  }
              });
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
             var elm = ['#btnRec', '#btnCon', '#divReccommend'];
             var tipPosition = ['bottomMiddle', 'bottomMiddle','leftMiddle'];
             var tipTarget = ['topMiddle', 'topMiddle','middleRight'];
             var tipContent = ['ดูชุดข้อสอบที่ควรทำเพิ่ม', 'ดูชุดข้อสอบที่คล้ายกัน','กดไปทำฝึกฝนเพิ่มเติม (เพราะมีข้อต้องปรับปรุง)'];
             var tipAjust = [0, 0,-150];
             var y = [0, 0, 100];
             var w = [150, 200,200];
             for (var i = 0; i < elm.length; i++) {
                 $(elm[i]).qtip({
                     content: tipContent[i],
                     show: { ready: true },
                     style: {
                         width: w[i], padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: tipPosition[i], name: 'dark', 'font-weight': 'bold', 'font-size': '18px'
                     }, hide: false,
                     position: { corner: { tooltip: tipPosition[i], target: tipTarget[i] }, adjust: { x: tipAjust[i], y: y[i] } },
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
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <script type="text/javascript">
        $(function () {
            //hover animation
            InjectionHover(':submit', 5, false);
        });
    </script>
</asp:Content>
