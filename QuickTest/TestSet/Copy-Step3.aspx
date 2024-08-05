<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Copy-Step3.aspx.vb" Inherits="QuickTest.Copy_Step3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        tbody.on
        {
            display: table-row-group;
        }
        tbody.off
        {
            display: none;
        }
        
        .toolsTip
        {
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
        .TopRight
        {
            position: fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            color: #000000;
            display: none;
            top: 0px;
            right: 0px;
            -moz-border-radius:15px;
            padding:10px;
            padding-bottom:5px;
            padding-top:5px;
            margin:3px;
            -webkit-border-radius:15px;
            border-radius:15px;
            behavior:url(/css/PIE.htc);
        }
        .ForDescription
        {
            position:fixed;
            border: 3px solid #FFCC66;
            background-color: #FFFFCC;
            -moz-border-radius:15px;
            color: #000000;
            padding:10px;
            padding-bottom:5px;
            padding-top:5px;
            display:none;
            margin:3px;
            top: 0px;
            left:0px;
            -webkit-border-radius:15px;
            border-radius:15px;
            behavior:url(/css/PIE.htc);
            }
    </style>
    <%--
 

    <script type="text/javascript" src="<%=ResolveUrl("~")%>js/jquery.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~")%>shadowbox/shadowbox.css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyPhoto.css" type="text/css"
        media="screen" charset="utf-8" />
    <link charset="utf-8" media="screen" type="text/css" href="<%=ResolveUrl("~")%>css/aa.css"
        rel="stylesheet" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~")%>css/general.css" />
    
    <%--<script type="text/javascript" src="<%=ResolveUrl("~")%>shadowbox/shadowbox.js"></script>--%>
    <script type="text/javascript">
 
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
 

          function SumQuestionSelected() {
              
//              var sum = 0;
//              //iterate through each textboxes and add the values
//              $(".spnSelectedQuestions").each(function () {
//                  //add only if the value is number
//                  if (!isNaN($(this).text()) && $(this).text().length != 0) {
//                      sum += parseFloat($(this).text());
//                  }

//              });
//               var spnSelectLegth = document.getElementsByName('spnSelec').length;
//                    var spnSelect = document.getElementsByName('spnSelec');
//                    var sumSpnSelect = 0;
//                    for (i = 0; i < spnSelectLegth; i++) {
//                        var s = parseInt($(spnSelect.item(i)).text(), 10);
//                        sumSpnSelect = sumSpnSelect + s;
//                    }
//                    $('#spnTotalQuestionsSelected').html(sumSpnSelect);
//                    $('#spnTotalQuestionsSelected1').html(sumSpnSelect);
                if ($.browser.msie) {//IE
                    var inputs = document.getElementsByTagName('span');
                    var spanA = [];
                    for (var i = 0; i < inputs.length; i++) {
                    if (inputs.item(i).getAttribute('name') == 'spnSelec') {
                            spanA.push(inputs.item(i));
                        }
                    }
                    var sumspan = 0;
                    for (i = 0; i < spanA.length; i++) {
                        var s3 = parseInt($(spanA[i]).text(), 10);
                        sumspan = sumspan + s3;
                    }
                    $('#spnTotalQuestionsSelected').html(sumspan);
                    $('#spnTotalQuestionsSelected1').html(sumspan);
               }
               else{//FF CHROME
                    var spnSelectLegth = document.getElementsByName('spnSelec').length;
                    var spnSelect = document.getElementsByName('spnSelec');
                    var sumSpnSelect = 0;
                    for (i = 0; i < spnSelectLegth; i++) {
                        var s = parseInt($(spnSelect.item(i)).text(), 10);
                        sumSpnSelect = sumSpnSelect + s;
                    }
                    $('#spnTotalQuestionsSelected').html(sumSpnSelect);
                    $('#spnTotalQuestionsSelected1').html(sumSpnSelect);}
              //.toFixed() method will roundoff the final sum to 2 decimal places
              //$("#spnTotalQuestionsSelected").html(sum);
          }

          function onSave(chkBox, qSetId, testSetId, userId) {

              var valOftotal = $('#spnTotal_'+qSetId).html();
              var CreateStr = "ชุดนี้เลือกมาแล้ว <span id='spnSelected_"+qSetId+"' name='spnSelec'>"+valOftotal+"</span> จาก <span id='spnTotal_"+qSetId + "'>"+valOftotal+"</span> ข้อ";
              if(chkBox.checked){$(chkBox).next().next().next().html(CreateStr);}
              else{
              var CreateStr = "ชุดนี้ยังไม่ถูกเลือก (มีทั้งหมด <span id='spnTotal_"+qSetId + "'>"+valOftotal+"</span> ข้อ)";
              $(chkBox).next().next().next().html(CreateStr);
              }
              SumQuestionSelected();

              
              var valReturnFromCodeBehide;
              $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>testset/Step3.aspx/OnSaveCodeBehide",
                  data: "{ needRemove : '" + !(chkBox.checked) + "', qSetId : '" + qSetId + "', testSetId : '" + testSetId + "', userId : '" + userId + "'}",  //" 
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (msg) {
                      valReturnFromCodeBehide = msg.d;
                      //alert('success'+valReturnFromCodeBehide);
                  },
                  error: function myfunction(request, status) {
                      //alert('shin' + request.statusText + status);    
                  }
              });
          }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--     <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
   
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
    <div id="main">
        <header class="thinheader">
      <div id="logo" class="slogantext">
        <div id="logo_text">
          <h2>ครบถ้วน ถูกต้อง ฉับไว </h2>
		  
		   <input class="submitChangeFontSize" type="image" src="<%=ResolveUrl("~")%>images/minsize.png" name="largerFont" id="largerFont" value="ก+" onclick="resizeText(1)" style="margin-top: -40px;margin-left: 702px;"/> 
		  <input class="submitChangeFontSize" type="image" src="<%=ResolveUrl("~")%>images/maxsize.png" name="smallerFont"  id="smallerFont" value="ก-" onclick="resizeText(-1)" style="margin-top: -40px;margin-left: 832px;"/> 
		  <!--/div-->

        </div>
      </div>
      <nav>
        <div id="menu_container">
          <ul class="sf-menu" id="nav" stylesheet="text-align:center;"style="font-size:20px">
		  <li><a href="step1.aspx" class="current">ขั้นที่ 1: จัดชุด --></a></li>
          <li><a href="step2.aspx" class="current">ขั้นที่ 2: เลือกวิชา --> </a></li> 
          <li><a href="#" class="current">ขั้นที่ 3: เลือกหน่วยการเรียนรู้ --></a></li>	
          <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 4: บันทึก และจัดพิมพ์</a></li>	

          
          					
           </ul>
        </div>
      </nav>
    </header>
        <div id="site_content">
            <div class="content ListingFixedHeightContainer" style="width: 930px;">
                <section id="select">
		
          <asp:Repeater id="ListingSubject" runat="server">
            <ItemTemplate>
                <h3 id="Subject_<%# Container.DataItem("SubjectId") %>" style="margin: 15px 0 0 0;"><%# Container.DataItem("SubjectName")%></h3>
		        <center>
                    <div id="div-1" style='text-align:left' class="ListingContent"> 
                        <asp:Repeater id="ListingClass" runat="server">
                            <ItemTemplate>
		                       <table style="width:100%; border-spacing:0;">
                                <tbody id="ShowHide" >
                                    <tr onclick="return toggleTbody('<%# Container.DataItem("SubjectId") & "_" &  Container.DataItem("ClassId")%>');;"><td><b>ชั้น<%# Container.DataItem("ClassName")%></b></td></tr>
                                </tbody>
                                <tbody  class='off' id="Notice<%# Container.DataItem("SubjectId") & "_" &  Container.DataItem("ClassId")%>"><tr><td colspan="'2'">มีรายชื่อหน่วยการเรียนรู้ซ่อนอยู่</td></tr></tbody>
                                <tbody id="<%# Container.DataItem("SubjectId") & "_" &  Container.DataItem("ClassId")%>">
                                <%# CreateTestUnitList(Eval("ClassId"), Eval("SubjectId"))%>
		                        </tbody>
                              </table>
                            </ItemTemplate>
                        </asp:Repeater>

		            </div>
		        </center>
            </ItemTemplate>
          </asp:Repeater>

<%--        	<div style="margin-left: 500px; width:300px;">รวมทั้งหมด <span id='spnTotalSubjectSelected'>0</span> วิชา <span id='spnTotalQuestionsSelected'>0</span> ข้อ จาก <span id='spnTotalClassesSelected'>0</span> ระดับชั้น</div>
--%>        	<div style="margin-left: 700px; width:300px;">รวมทั้งหมด <span id='spnTotalQuestionsSelected'>0</span> ข้อ</div>
		 </section>
                <script type="text/javascript">                    var c = document.getElementById("select"); function resizeText(multiplier) { if (c.style.fontSize == "") { c.style.fontSize = "1.0em"; } c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2) + "em"; } </script>
                <%--<a href="SaveSet.aspx?iframe=true&width=800&height=600"" rel="prettyPhoto" title="ตั้งชื่อแบบทดสอบที่จัดชุดมาค่ะ" style="text-decoration:none">
            <input class="submitChangeFontSize" type="submit" name="smallerFont" value="บันทึก -> พิมพ์"  style="margin-left: 670px; width:220px; margin-top: 15px" />
        </a>--%>
                <form id="toNextPage" runat="server">
                <asp:Button ID="btnNextStep4" runat="server" class="submitChangeFontSize" Text="Next - ไปต่อขั้น 4 >>"
                    Style="margin-left: 730px; width: 200px;"></asp:Button>
                <a id="footer-back-to-top" class="WhiteButton badge-back-to-top "><strong>รวม <span
                    id='spnTotalQuestionsSelected1'>0</span> ข้อ</strong></a>
                </form>
                <footer style="margin-top: 100px">
      <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
    </footer>
            </div>
        </div>
    </div>
    <div id="tip">
    </div>
    <div id='DescriptionDiv' class="ForDescription">เมื่อคลิกสามารถดูรายละเอียดข้อสอบและ<br />&nbspเลือกข้อสอบเฉพาะบางข้อที่ต้องการได้</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css"
        media="screen" charset="utf-8" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyPhoto.js" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("a[rel^='prettyPhoto']").prettyPhoto({
                default_width: 800,
                default_height: 600,
                modal: true,
                social_tools: '<div class="pp_social"><div class="facebook" style="width: 300px;"><iframe  src="http://www.facebook.com/plugins/like.php?locale=th_TH&href=http://www.wpp.co.th/quicktest/&layout=button_count&amp;show_faces=true&amp;width=300&amp;action=like&amp;font&amp;colorscheme=light&amp;height=23" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:300px; height:23px;" allowTransparency="true"></iframe></div></div>',
                callback: function () {
                    SumQuestionSelected();

                }
            });
            $.prettyLoader();

        });
    </script>
    <script type="text/javascript">

        function toggleTbody(id) {
            if (document.getElementById) {
                var tbod = document.getElementById(id);
                var tbodNotice = document.getElementById('Notice' + id);
                if (tbod && typeof tbod.className == 'string') {
                    if (tbod.className == 'off') {
                        tbod.className = 'on';

                        tbodNotice.className = 'off';

                    } else {
                        tbod.className = 'off';
                        tbodNotice.className = 'on';
                    }
                }
            }
            return false;
        }
    </script>

     <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
     <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    
    
    <script type="text/javascript">



        $(".ListingContent").alternateScroll();
      
      //  $("#Testdoo").alternateScroll();    


        $(document).ready(function () {
        $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />')
            SumQuestionSelected();
            //alert(sumSpnSelect);
            // toolstip

            $('label').hover(function (e) {
              callTooltip('#SpanFullDetail', e);
                var id = $(this).prev().attr('id');
                id = id.substring(3,id.length);
                $('#SpanFullDetail').html(id);
                $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>testset/Step3.aspx/getQuestionSetName",
                  data: "{ qSetId : '" + id + "' }",  //" 
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (msg) {
                        $('#SpanFullDetail').html(msg.d);

//                      $('#SpanFullDetail').html(msg.d);
//                      $('#SpanFullDetail').show();
                      //alert('success'+valReturnFromCodeBehide);
                  },
                  error: function myfunction(request, status) {
                      //alert('shin' + request.statusText + status);    
                  }
              });
            }, function () {
            $("#SpanFullDetail").mouseleave(function(){
             $('#SpanFullDetail').stop(true,true).fadeOut('slow');
            });

            $('#SpanFullDetail').stop(true,true).fadeOut('slow');
            });




        });

        function callTooltip(obj, e) {
        var tip = $(this).find('toolsTip');
            var locateX = e.pageX + 20;
            var locateY = e.pageY + 20;
            locateX +=10;
            locateY -=50;
//           $(obj).css({ left: locateX, top: locateY }).delay(1000).fadeIn();
  $("#SpanFullDetail").mouseenter(function(){
   $('#SpanFullDetail').stop(true,true).delay(800).fadeIn('slow');
  });
           $(obj).stop(true,true).delay(800).fadeIn('slow');
            //$(obj).css({ left: locateX, top: locateY }).delay(1000).show();
          
        }

        $('.aTag').hover(function(){
         $("#DescriptionDiv").stop(true,true).delay(800).fadeIn('slow');
        },
        function(){
          $("#DescriptionDiv").stop(true,true).fadeOut('slow');
        });

    

    </script>
    <span class="TopRight" id="SpanFullDetail"></span>
 
</asp:Content>