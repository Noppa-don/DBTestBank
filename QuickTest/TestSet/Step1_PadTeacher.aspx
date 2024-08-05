<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Step1_PadTeacher.aspx.vb" Inherits="QuickTest.Step1_PadTeacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">
        var Unload,CurrentPage,SignalRCheck;
        var withOutClick = true;
        var changePage = false;
       
//        $(window).bind('beforeunload', function () {
//            if (withOutClick == true) {
//                setUnload(true); // unload = true
//            }
//        });

        var groupname = '<%=GroupName %>';

        var thisPage = '<%=ResolveUrl("~")%>testset/step1.aspx';
        thisPage = thisPage.toLowerCase();
        var nextPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx';
        nextPage = nextPage.toLowerCase();
        var activityPage = '<%=ResolveUrl("~")%>activity/activitypage.aspx';
        activityPage = activityPage.toLowerCase();
        var alternativePage = '<%=ResolveUrl("~")%>activity/alternativepage.aspx';
        alternativePage = alternativePage.toLowerCase();

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
                if (CurrentPage == nextPage) {
                        window.location = '<%=ResolveUrl("~")%>activity/settingactivity_padteacher.aspx';
                }
                else if (CurrentPage == activityPage) {
                        window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
                }
                else {
                    window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx';
                }                    
            }
               
        }

        SignalRCheck = $.connection.hubSignalR;

        SignalRCheck.client.send = function (message) {        
            //ถ้า URL ที่ส่งมาเป็นหน้า Step1 ให้เปลี่ยนหน้าไปหน้า Step1 ของ Tablet
            //alert(message);
            if (message == thisPage) {
            }
            //ถ้า URL ที่ส่งมาเป็นหน้า Setting ให้เปลี่ยนหน้าไปหน้า Setting ของ Tablet
            else if (message == nextPage) {
                withOutClick = false;
                window.location = '<%=ResolveUrl("~")%>activity/settingactivity_padteacher.aspx';
            }
            //ถ้า URL ที่ส่งมาเป็นหน้า Activity ให้เปลี่ยนหน้าไปหน้า Activity ของ Tablet
            else if (message == activityPage) {
                withOutClick = false;
                window.location = '<%=ResolveUrl("~")%>Activity/ActivityPage_PadTeacher.aspx';
            }
            else if (message == alternativePage) {
                window.location = '<%=ResolveUrl("~")%>Activity/alternative_pad.aspx';
            }
            //นอกนั้นต้องเปลี่ยนหน้าเป็น หน้ารอจัดข้อสอบ
            else {
                withOutClick = false;
                window.location = '<%=ResolveUrl("~")%>Activity/WaitPageTeacherPad.aspx';
            }
        };

        $.connection.hub.start().done(function () {
            SignalRCheck.server.addToGroup(groupname);
            SignalRCheck.server.sendCommand(groupname, thisPage);
            
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
        function  setTestsetId(testsetId) {
            $.ajax({ type: "POST",
                  url: "<%=ResolveUrl("~")%>SelectSessionSignalR.aspx/setNewTestSetId",
                  data:"{ testsetId : '" + testsetId + "'}",
                  async: false,                              
                  contentType: "application/json; charset=utf-8", dataType: "json",
                  success: function (data) {                                                                        
                  },
                  error: function myfunction(request, status) {   
                        alert(status);                      
                  }
            });
        }
        function gotoSettingPad(editId) {
            setTestsetId(editId);
            setCurrentPage(nextPage);
            window.location = '<%=ResolveUrl("~")%>Activity/SettingActivity_PadTeacher.aspx?editid=' + editId;
        }
    </script>
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
		  
        </div>
      </div>
    </header>
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <section id="select">
		<center>
		<h2 style="margin: 15px 0 0 0;">เลือกชุดข้อสอบเพื่อทำควิซ</h2>
		
        <div id="div-1"  class="ListingFixedHeightContainer" style='text-align:center; '>

		  <div id="divListing"  class="ListingContent" style='text-align:center; position: relative;overflow:scroll'>
          <%--<div id="divListing" style='text-align:center; width:100%; height:100%; overflow:auto;'>--%>
          <asp:Repeater id="Listing" runat="server">
            <HeaderTemplate>
                  <table class="bordered" style="width:100%; border-spacing:4; margin-top: 0px; "><thead>
                  <tr ><th >ชื่อ</th><th >วันที่สร้าง</th></tr>
                  </thead>
            </HeaderTemplate>
            <ItemTemplate>
             <tr ><td  style="background: #FFFFCC;"><a style="cursor:pointer;" onclick="gotoSettingPad('<%# Container.DataItem("TestSet_Id")%>')"<%--href="../Activity/SettingActivity_PadTeacher.aspx?editid=<%# Container.DataItem("TestSet_Id")%>"--%>><%# Container.DataItem("TestSet_Name")%></a>
             <img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style=' float:right; cursor: pointer' /></td><td style="background: #FFFFCC;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </ItemTemplate>

            <AlternatingItemTemplate>
             <tr><td style="background: #FFFFFF;"><a style="cursor:pointer;" onclick="gotoSettingPad('<%# Container.DataItem("TestSet_Id")%>')"<%--href="../Activity/SettingActivity_PadTeacher.aspx?editid=<%# Container.DataItem("TestSet_Id")%>"--%>><%# Container.DataItem("TestSet_Name")%></a>
             <img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style='float:right;cursor: pointer'/></td><td style="background: #FFFFFF;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
             </table>
            </FooterTemplate>
          </asp:Repeater>


              </div>   
        	</div>
		</center>
		  </section>
            </div>
        </div>
    </div>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        //script ดักอีกที เมื่อโหลดเพจเสร็จ
        var CurrentPage;
        $(function () {         
            var thisPage = '<%=ResolveUrl("~")%>testset/step1.aspx'; 
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
      //ToggleBox();
        $('.RubberHide').hide();
        $('tr').hover(function () {
            $(this).find('img').stop(true,true).fadeIn('slow');
        },
        function () {
            $(this).find('img').stop(true,true).fadeOut('slow');
        }
        );
        

//        function ToggleBox() {

//            $("#divListing").slideToggle("slow");
//            if ($.browser.msie) {
//            if ($.browser.version <= 7) {
//                $('#divListing').css('overflow','auto');
//                }
//            }
            
//      }
   

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
    <ul id="manageStu">
        <li class="manageDataStu"><a title="จัดการข้อมูลนักเรียน" href="../ClassUpgrade/AlternativeClassUpgrade.aspx&iframe=true&width=800&height=300"
            rel="prettyPhoto">จัดการ<br />
            ข้อมูล<br />
            นักเรียน</a></li>
    </ul>
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
        });        
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
