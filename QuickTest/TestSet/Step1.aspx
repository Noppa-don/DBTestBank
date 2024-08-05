<%@ Page Title="QuickTest - ขั้นที่ 1: จัดชุดใหม่ หรือ เลือกชุดเก่า" Language="vb"
    AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Step1.aspx.vb"
    Inherits="QuickTest.Step1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">
        var Unload,CurrentPage,SignalRCheck;
        var withOutClick = true;
        var changePage = false;
        $('#spn').html('');
        $(window).bind('beforeunload', function () {
            if (withOutClick == true) {
                setUnload(true); // unload = true
            }
        });

        var groupname = '<%=GroupName %>';

        var thisPage = window.location.pathname;
        thisPage = thisPage.toLowerCase();
        var nextPage = '<%=ResolveUrl("~")%>testset/step2.aspx';
        nextPage = nextPage.toLowerCase();

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

        SignalRCheck.client.send = function (message) {//var today = new Date();$('#spn').append('รับแล้ว'+ today.getTime()).css('color','green');
            if (message == thisPage) {
            }
            else {                
                withOutClick = false;
                window.location = message;
            }
        };

        $.connection.hub.start().done(function () {
            SignalRCheck.server.addToGroup(groupname);

            $('#newTestset').click(function () {
                withOutClick = false;
                resetValue();
                setCurrentPage(nextPage);
                SignalRCheck.server.sendCommand(groupname, nextPage);//var today = new Date();$('#spn').append('ส่งแล้ว' + today.getTime()).css('color','blue');
                window.location = nextPage;
            });

            $('input[name=nextStep2]').click(function () {
                withOutClick = false;
                resetValue();
                setCurrentPage(nextPage);
                SignalRCheck.server.sendCommand(groupname, nextPage);
            });

            $('.aTestset').click(function () {
                withOutClick = false;
                resetValue();
                setCurrentPage(nextPage);
                SignalRCheck.server.sendCommand(groupname, nextPage);
            });

            $('#lblReport').click(function() {
                var toPage = '<%=ResolveUrl("~")%>viewreport/viewreportmain.aspx';
                toPage = toPage.toLowerCase();
                withOutClick = false;
                setCurrentPage(toPage);
                SignalRCheck.server.sendCommand(groupname, toPage);
                window.location = toPage;
            });

            if (changePage) {//var today = new Date();
                SignalRCheck.server.sendCommand(groupname, thisPage);//$('#spn').append('ส่งแล้ว' + today.getTime()).css('color','blue');
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
        function resetValue() {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/resetObjTestset",
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
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
		  

        </div>
      </div>
      <nav>
        <div id="menu_container">
        <span id='spn' style="font-size:20px;color:Black;"></span>
          <ul class="sf-menu" id="nav" stylesheet="text-align:center;" style="font-size:<%= ChkFontSize %>">
		  <li><a href="#" class="current"><%=txtStep1 %></a></li>
          <li><a href="#" style="cursor:not-allowed;"><%=txtStep2 %></a></li> 
          <li><a href="#" style="cursor:not-allowed;"><%=txtStep3 %></a></li>				
          <li><a href="#" style="cursor:not-allowed;"><%=txtStep4 %></a></li>
           <%Try%>
           <%If HttpContext.Current.Application("NeedQuizMode").ToString() = "True" Then%>
          <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 5: จัดพิมพ์,เริ่มควิซ</a></li>	
           <%End If%>
           <%Catch%>
           <%Response.Redirect("~/LoginPage.aspx", False)%>
           <%End Try%>	
           </ul>
        </div>
      </nav>
    </header>
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <section id="select">
		<center>
		<h2 style="margin: 15px 0 0 0;">ต้องการจัดข้อสอบชุดใหม่ หรือ เลือกข้อสอบชุดเก่า</h2>
        
		<div id="div-1" style='text-align:center'><h3 ><label id="newTestset" class="new" <%--onclick="document.location.href ='step2.aspx';"--%>>จัดชุดข้อสอบใหม่</label></h3> </div>
		
        <div id="div-1"  class="ListingFixedHeightContainer" style='text-align:center; '>
          <h3 ><label class="old"  onclick="ToggleBox();">แก้ไขชุดเก่า</label></h3>
	

		  <div id="divListing"  class="ListingContent" style='text-align:center; position: relative;overflow:scroll'>
          <%--<div id="divListing" style='text-align:center; width:100%; height:100%; overflow:auto;'>--%>
          <asp:Repeater id="Listing" runat="server">
            <HeaderTemplate>
                  <table class="bordered" style="width:100%; border-spacing:4; margin-top: 0px; "><thead>
                  <tr ><th >ชื่อ</th><th >วันที่สร้าง</th></tr>
                  </thead>
            </HeaderTemplate>
            <ItemTemplate>
             <tr ><td  style="background: #FFFFCC;"><a class="aTestset" href="Step2.aspx?editid=<%# Container.DataItem("TestSet_Id")%>"><%# Container.DataItem("TestSet_Name")%></a><img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style=' float:right; cursor: pointer' /></td><td style="background: #FFFFCC;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </ItemTemplate>

            <AlternatingItemTemplate>
             <tr><td style="background: #FFFFFF;"><a class="aTestset" href="Step2.aspx?editid=<%# Container.DataItem("TestSet_Id")%>"><%# Container.DataItem("TestSet_Name")%></a><img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style='float:right;cursor: pointer'/></td><td style="background: #FFFFFF;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
             </table>
            </FooterTemplate>
          </asp:Repeater>
          

              </div>   
        	</div>
            <%If HttpContext.Current.Application("NeedHomeWork") = True Then%>
             <div id="div-1"  class="ListingFixedHeightContainer" style='text-align:center; '>
          <h3 ><label class="old"  onclick="ToggleBox2();">แก้ไขการบ้านเก่า</label></h3>
		  <div id="divListing2"  class="ListingContent" style='text-align:center; position: relative;overflow:scroll'>
          <%--<div id="divListing" style='text-align:center; width:100%; height:100%; overflow:auto;'>--%>
          <asp:Repeater id="rptHomework" runat="server">
            <HeaderTemplate>
                  <table class="bordered" style="width:100%; border-spacing:4; margin-top: 0px; "><thead>
                  <tr ><th >ชื่อ</th><th >วันที่สร้าง</th></tr>
                  </thead>
            </HeaderTemplate>
            <ItemTemplate>
             <tr ><td  style="background: #FFFFCC;"><a class="aTestset" href="../Module/HomeWorkPage.aspx?Id=<%# Container.DataItem("TestSet_Id")%>&Name=<%# Container.DataItem("TestSet_Name")%>&Page=Step1"><%# Container.DataItem("TestSet_Name")%></a><%--<img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style=' float:right; cursor: pointer' />--%></td><td style="background: #FFFFCC;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </ItemTemplate>

            <AlternatingItemTemplate>
             <tr><td style="background: #FFFFFF;"><a class="aTestset" href="../Module/HomeWorkPage.aspx?Id=<%# Container.DataItem("TestSet_Id")%>&Name=<%# Container.DataItem("TestSet_Name")%>&Page=Step1"><%# Container.DataItem("TestSet_Name")%></a><%--<img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style='float:right;cursor: pointer'/>--%></td><td style="background: #FFFFFF;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
             </table>
            </FooterTemplate>
          </asp:Repeater>
              </div>   
        	</div>
              <%End If %>

            <%If HttpContext.Current.Application("NeedQuizMode").ToString() = "True" Then%>
       <div id="div-1" style='text-align:center;'><h3 ><label  id="lblReport" >ดูรายงาน</label></h3> </div>
        <%End If%>
		</center>
		  </section>
                <script type="text/javascript">                    var c = document.getElementById("select"); function resizeText(multiplier) { if (c.style.fontSize == "") { c.style.fontSize = "1.0em"; } c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2) + "em"; } </script>
                <input class="submitChangeFontSize" type="submit" name="nextStep2" value="Next - ไปต่อขั้น 2 >>"
                    style="margin-left: 730px; width: 200px;" onclick="document.location.href ='step2.aspx';" />
                <!--[if lte IE 7]><br /><![endif]-->
            </div>
        </div>
        <footer style="margin-top: 15px">
      <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
 
    </footer>
    </div>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
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
            //var today = new Date();$('#spn').append('เช็คแล้ว'+ today.getTime()).css('color','red');
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/slimScroll.js" type="text/javascript"></script>
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="../js/facescroll.js" type="text/javascript"></script>
    <script type="text/javascript">


        $("#divListing").alternateScroll();
        $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />')
      ToggleBox();
        $('.RubberHide').hide();
        $('tr').hover(function () {
            $(this).find('img').stop(true,true).fadeIn('slow');
        },
        function () {
            $(this).find('img').stop(true,true).fadeOut('slow');
        }
        );

         $("#divListing2").alternateScroll();
        $('.alt-scroll-vertical-bar').html('<img src="../Images/sprite_On.PNG" /><img src="../Images/sprite_Down.PNG" style="margin-top: 52px;" />')
      ToggleBox();
        $('.RubberHide').hide();
        $('tr').hover(function () {
            $(this).find('img').stop(true,true).fadeIn('slow');
        },
        function () {
            $(this).find('img').stop(true,true).fadeOut('slow');
        }
        );
        

        function ToggleBox() {

            $("#divListing").slideToggle("slow");
            if ($.browser.msie) {
            if ($.browser.version <= 7) {
                $('#divListing').css('overflow','auto');
                }
            }
            
      }
      function ToggleBox2() {

            $("#divListing2").slideToggle("slow");
            if ($.browser.msie) {
            if ($.browser.version <= 7) {
                $('#divListing2').css('overflow','auto');
                }
            }
            
      }

        function UpdateTestSetId(tsId,s) {

           if (confirm('ต้องการลบข้อสอบที่จัดชุดไว้ใช่หรือไม่ ?') == true) {

                $(s).closest("tr").remove();
                             $.ajax({ type: "POST",
                             url: "<%=ResolveUrl("~")%>testset/Step1.aspx/UpdateTestSetIdCodeBehind",
                             data: "{ TestsetId : '" + tsId + "'}",
                             contentType: "application/json; charset=utf-8",
                             success: function () {
                              //alert("บันทึกข้อมูลเรียบร้อย!");
                          },
                          error: function myfunction() {
                              //alert("เกิดข้อผิดพลาด!");
                        }
                        });
                        ChengeColorTr();
            }
                  
        };

         function ChengeColorTr() {
        var i = 1;
       
      $('.bordered').find('tr').each(function(){
            
            if (i > 1) {
                var td = $(this).find("td");
                if (i % 2 == 0) {
                    
                    $(td).css('background','#FFFFCC');
                }else{
                    $(td).css('background','#FFFFFF');
                }
            }
       i = i+1;
     });
     }
     
    </script>
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyPhoto.css" type="text/css" />
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyPhoto.js" type="text/javascript"
        charset="utf-8"></script>
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
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <%-- add menu slide upgradeClass--%>
    <%If HttpContext.Current.Application("NeedManageSchoolInfo") = True Then%>
    <ul id="manageStu">
        <li class="manageDataStu"><a title="จัดการข้อมูลนักเรียน" href="../ClassUpgrade/AlternativeClassUpgrade.aspx&iframe=true&width=800&height=300"
            rel="prettyPhoto">จัดการ<br />
            ข้อมูล<br />
            นักเรียน</a></li>
    </ul>
    <% End If %>
    <script type="text/javascript">
        $(function () {
            $('#manageStu a').stop().animate({ 'marginLeft': '-10px' }, 1000);
            $('#manageStu > li').hover(
            function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-66px' }, 200);
            },
                function () {
                    $('a', $(this)).stop().animate({ 'marginLeft': '-10px' }, 200);
                }
                );

            ToggleBox();
            ToggleBox2();
        });        
    </script>
</asp:Content>
