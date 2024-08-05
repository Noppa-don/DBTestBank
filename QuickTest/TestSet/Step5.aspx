<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="Step5.aspx.vb" Inherits="QuickTest.Step5" %>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
<%--    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <script type="text/javascript">

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
            var nextPage = '<%=ResolveUrl("~")%>activity/settingactivity.aspx'; 
            nextPage = nextPage.toLowerCase();  
            var pdfPage = '<%=ResolveUrl("~")%>testset/genpdf.aspx';
            pdfPage = pdfPage.toLowerCase();  

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
                    $('.settingActivity').trigger('click');
                }
                else if (cmd == pdfPage) {
                    firstClick = false;
                    $('.genPDF').trigger('click');
                }                  
            };

            $.connection.hub.start().done(function () {
                SignalRCheck.server.addToGroup(groupname);

                $('.settingActivity').click(function(e) { 
                    withOutClick = false;
                    if (firstClick) {
                        e.preventDefault();
                        setCurrentPage(nextPage);                                         
                        SignalRCheck.server.sendCommand(groupname, nextPage);
                    }
                    else {
                        window.location = nextPage;
                    }                        
                });

                $('.genPDF').click(function(e) {
                    withOutClick = false; 
                    if (firstClick) {
                        e.preventDefault();                        
                        setCurrentPage(pdfPage);                                        
                        SignalRCheck.server.sendCommand(groupname, pdfPage);
                    }
                    else {
                        window.location = pdfPage;
                    }                   
                });
           

                if(changePage){                                      
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
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="Form1" runat="server">
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
          <ul class="sf-menu" id="nav" stylesheet="text-align:center;"style="font-size:<%= ChkFontSize %>">
		  <li><a href="step1.aspx" class="current"><%=txtStep1 %></a></li>
          <li><a href="step2.aspx" class="current"><%=txtStep2 %></a></li> 
          <li><a href="step3.aspx" class="current"><%=txtStep3 %></a></li>						
          <li><a href="step4.aspx" class="current"><%= txtStep4%></a></li>	
           <%If HttpContext.Current.Application("NeedQuizMode").ToString() = "True" Then%>
          <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 5: จัดพิมพ์,เริ่มควิซ</a></li>	
           <%End If%>

          </ul>
        </div>
      </nav>
    </header>
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <center>
                    <h2>
                        ต้องการทำควิซหรือสร้างไฟล์ข้อสอบคะ ?
                    </h2>
                    <div id="div-1">
                        <table>
                            <tr>
                                <td style="border-bottom: initial;">
                                    <div class="form_settings">
                                        <table>
                                            <tr>
                                                <td style="text-align: center; width: 34%; border-bottom: 1px solid white;">
                                                    <a class="settingActivity">
                                                        <img alt="ทำควิซ" src="../images/Activity/group3.png" height="120px" /></a>
                                                </td>
                                                <td style="text-align: center; width: 33%; border-bottom: 1px solid white;">
                                                    <%If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then%>
                                                    <a id="A1" onclick="OpenSettingCheckmark()">
                                                        <img alt="สร้างไฟล์ข้อสอบ" src="../images/Activity/Document-Add-icon.png" /></a>
                                                    <%Else%>
                                                    <%--<a class="genPDF" id="imgCreateFile">--%>
                                                    <a href="GenPDF.aspx" id="imgCreateFile">
                                                        <img alt="สร้างไฟล์ข้อสอบ" src="../images/Activity/Document-Add-icon.png" /></a>
                                                    <%End If%>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px;">
                                                <td style="text-align: center; border-bottom: 1px solid white;">
                                                    <a class="settingActivity" style="text-decoration: none;">
                                                        <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="btnActivity"
                                                            runat="server" Text="ทำควิซ" class="submit" /></a>
                                                </td>
                                                <td style="text-align: center; border-bottom: 1px solid white;">
                                                    <%If HttpContext.Current.Application("NeedCheckmark") = True Then%>
                                                    <a style="text-decoration: none;" onclick="OpenSettingCheckmark()">
                                                        <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="btnCreateFile"
                                                            runat="server" Text="สร้างไฟล์ข้อสอบ" class="submit" /></a>
                                                    <%Else%>
                                                    <%--<a class="genPDF" style="text-decoration: none;">--%>
                                                    <%--  <a href="GenPDF.aspx" style="text-decoration: none;">--%>
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="GoToGenPDF"
                                                        runat="server" Text="สร้างไฟล์ข้อสอบ" class="submit" /><%--</a>--%>
                                                    <%End If%>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <footer style="margin-top: 50px">
                           <%-- <a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
                    </footer>
                </center>
            </div>
        </div>
    </div>
    </form>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        //script ดักอีกที เมื่อโหลดเพจเสร็จ
        var CurrentPage;
        $(document).ready(function () {  
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $('#Help').remove();
            $('#navigation').css("display", "none");
        });
    </script>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
           
            var needCheckmark = getAppsettingNeedCheckmark();
            
            $('#<%= btnCreateFile.ClientID %>').click(function () {
                if(!needCheckmark){
                    WaitCreatePDF();
                }
            });
            $('#imgCreateFile').click(function () {
                if(!needCheckmark){
                   WaitCreatePDF();
                }
            });
                   

        });
        function OpenSettingCheckmark() {
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>testset/onfirmCheckmark.aspx',
                'type': 'iframe',
                'width': 650,
                'minHeight': 350                
            });
        }
        function WaitCreatePDF() {
            $.blockUI({ css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
                message: 'รอซักครู่ ประมาณ 3-5 นาที นะคะ กำลังจัดหน้า PDF ไฟล์ข้อสอบให้อยู่ค่ะ ... Please wait ....'
            });            
        }
        function getAppsettingNeedCheckmark() {
            var ncm;
            $.ajax({ type: "POST",
                url: "<%=ResolveUrl("~")%>TestSet/Step5.aspx/getAppsettingNeedcheckmark",                    
                async: false, // ทำงานให้เสร็จก่อน                        
                contentType: "application/json; charset=utf-8", dataType: "json",   
                success: function (data) {
                    valReturnFromCodeBehide = data.d;                   
                    ncm =  valReturnFromCodeBehide;  
                },
                error: function myfunction(request, status)  {    
                }
            });
            return ncm;                    
        }

//        function closeTheIFrame() {
//            $('.pp_pic_holder').css('display', 'none');
//            $('.pp_default').css('display', 'none');
//            $('.pp_overlay').css('display', 'none');
//            var url = '../TestSet/GenPDF.aspx';
//            window.parent.location = url;
//            WaitCreatePDF();
//        }
        
    </script>
</asp:Content>
