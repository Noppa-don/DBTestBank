<%@ Page Title="QuickTest - ขั้นที่ 4: บันทึก และจัดพิมพ์ไฟล์ข้อสอบ" Language="vb"
    AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Step4.aspx.vb" ValidateRequest="false"
    Inherits="QuickTest.Step4" %>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../css/jquery-ui-1.8.18.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
    <%If Not IE = "1" Then%>
<%--    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>--%>
<%--    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
<%--    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
    <%End If%>
    <style type="text/css">
        .settingChkModeTestset .itemChk {
            width: 170px;
            height: 50px;
            padding: 0 0 0 50px;
            float: left;
            line-height: 50px;
            color: Black;
        }

            .settingChkModeTestset .itemChk > label {
                margin-top: -20px;
                /*display: inline-table;*/
                vertical-align: top;
            }
    </style>

    <%If IsAndroid = True Then%>
    <style type="text/css">
        #main, #site_content, .content {
            width: 880px !important;
        }

        nav, footer, #logo, .slogantext {
            width: 860px !important;
        }

        .bordered {
            width: 98% !important;
            margin-left: auto !important;
            margin-right: auto !important;
        }

        p label {
            margin-left: 10px !important;
        }

        #tdTestsetName {
            font-size: 25px !important;
        }

        #txtName {
            width: 450px !important;
            font-size: 25px !important;
        }

        #btnBack, #btnSave {
            height: 70px !important;
            width: 200px !important;
            font-size: 25px !important;
            left: 20px !important;
        }

        #divButton {
            height: 70px !important;
        }

        .settingChkModeTestset {
            font-size: 24px !important;
        }

        #btnSave {
            left: 630px !important;
        }
    </style>
    <% End If%>

      <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
        <style type="text/css">
            html {
                background-image:none !important;
            }

    </style>
    <%End If %>


    <script type="text/javascript">
        /* css for IE,Firefox */
        $(function () {
            if ($.browser.msie || !!navigator.userAgent.match(/Trident\/7\./)) {
                $('#txtName').css('width', '480px');
                $('.settingChkModeTestset .itemChk > label').css('margin-top', '10px');
                $('img').css({ 'position': 'absolute' });
            }
            if (navigator.userAgent.indexOf('Firefox') > -1) {
                $('#site_content').css({ 'position': 'relative', 'left': '-647px' });
            }
        });
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
    <div id="main" style="background-color:white;padding-bottom:10px;">
        <header class="thinheader">
      <div id="logo" class="slogantext">
        <div id="logo_text">
          <h2>ครบถ้วน ถูกต้อง ฉับไว </h2>		  
		  <%-- <input class="submitChangeFontSize" type="image" src="<%=ResolveUrl("~")%>images/minsize.png" name="largerFont" id="largerFont" value="ก+" onclick="resizeText(1,event)" style="margin-top: -40px;margin-left: 702px;"/> 
		  <input class="submitChangeFontSize" type="image" src="<%=ResolveUrl("~")%>images/maxsize.png" name="smallerFont"  id="smallerFont" value="ก-" onclick="resizeText(-1,event)" style="margin-top: -40px;margin-left: 832px;"/> --%>
		         </div>
      </div>
      <nav>
        <div id="menu_container">
          <%--<ul class="sf-menu" id="nav" stylesheet="text-align:center;"style="font-size:<%= ChkFontSize %>">
		  <li><a href="step1.aspx" class="current"><%=txtStep1 %></a></li>
          <li><a href="step2.aspx" class="current"><%=txtStep2 %></a></li> 
          <li><a href="step3.aspx" class="current"><%=txtStep3 %></a></li>						
          <li><a href="#" class="current"><%=txtStep4 %></a></li>
          <% Try%>	
           <%If HttpContext.Current.Application("NeedQuizMode").ToString() = "True" Then%>
          <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 5: จัดพิมพ์,เริ่มควิซ</a></li>	
           <%End If%>
           <%Catch%>
           <%Response.Redirect("~/LoginPage.aspx", False)%>
           <%End Try%>
          </ul>--%>
          <ul class="sf-menu" id="nav" style="font-size:20px;text-align:center;">
	        <li><a href="../Testset/DashboardSetupPage.aspx"><img id="imgBack" src="../Images/Home.png" style="position: absolute; margin-left: 5px; margin-top: -8px; cursor: pointer;"></a></li> 
            <li style="margin-left:45px;"><a href="../Testset/Step2.aspx" >ขั้นที่ 1 เลือกวิชา --></a></li> 
            <li><a href="../Testset/Step3.aspx" >ขั้นที่ 2 เลือกหน่วยการเรียนรู้ --></a></li> 
            <li><a href="#" class="current">ขั้นที่ 3 บันทึก</a></li>
          </ul>
        </div>
      </nav>
    </header>
        <form id="saveset" runat="server">
            <%--<section id="select">--%>
            <div id="site_content" style="padding: 0;margin:auto;">
                <%If HttpContext.Current.Session("EditID") IsNot Nothing And HttpContext.Current.Session("EditID") <> "" Then %>
                <div style="text-align: center;">
                    <span style="color: red; font-size: 20px;"><%= EditTestSetWarningText %></span>
                </div>
                <%End If %>
                <div class="content" style="width: 930px;">

                    <%--<div id="centeringdivouter">                     <div id="centeringdivmiddle">                         <div id="centeringdivinner">--%>
                    <center>
                    <%--<h2>
                        บันทึกข้อสอบชุดนี้ไว้
                    </h2>--%>
                    <div id="div-1" style=" height: 315px;">
                        
                        <div class="form_settings">
                          
                            <div style="width: 555px;margin-right: auto;margin-left: auto;background-color: #FEFEFE;border-radius: .5em;padding: 10px;
                                clear: both;height: 196px;" class="settingChkModeTestset">
                                 <table style=" margin: auto;">
                                <tr>
                                    <td style="text-align: center;border-bottom: 0px;background: transparent;font-size: 120%;font-weight: bold;">
                                        ใช้เป็น 
                                    </td>                                    
                                </tr>
                               </table>

                                <div class="itemChk" id="DivChkIsQuiz" style="margin-bottom: 10px;border-right: 1px solid #C9C9C9; margin-left: 48px;">
                                    <asp:CheckBox ID="ChkIsQuiz" runat="server" ClientIDMode="Static" Text="ควิซ" />
                                    <label for="ChkIsQuiz"></label><img src="../Images/SmallIconMode/IconQuiz.png" style="width: 60px;height: 60px;margin-left: 9px;" alt="" />
                                </div>

                                <div class="itemChk" id="DivChkIsHomework" style="margin-left: 10px; margin-bottom: 10px;">
                                    <asp:CheckBox ID="ChkIsHomework" ClientIDMode="Static" runat="server" Text="การบ้าน" />
                                    <label id="lblForChkIsHomework" for="ChkIsHomework">
                                        
                                    </label><img src="../Images/SmallIconMode/IconHomework.png" style=" width:60px; height:60px;" alt="" />
                                </div>

                                <div style="float: left;border-bottom: 1px solid #C9C9C9;width: 439px;margin-bottom: 10px;margin-left: 50px;margin-top: 4px;">
                                </div>

                                <div class="itemChk" id="DivChkIsPractice" style="margin-bottom: 10px;border-right: 1px solid #C9C9C9;margin-left: 48px;">
                                    <asp:CheckBox ID="ChkIsPractice" ClientIDMode="Static" runat="server" Text="ฝึกฝน" />
                                    <label id="lblForChkIsPractice" for="ChkIsPractice">
                                        
                                    </label><img src="../Images/SmallIconMode/IconPractice.png" style="width: 60px;height: 60px;margin-left: -4px;" alt="" />
                                </div>

                                <div class="itemChk" id="DivChkIsPrintTestset" style="margin-left: 10px; ">
                                    <asp:CheckBox ID="ChkIsPrintTestset" ClientIDMode="Static" runat="server" Text="ใบงาน" />
                                    <label id="lblForChkIsPrintTestset" for="ChkIsPrintTestset">
                                        
                                    </label><img src="../Images/SmallIconMode/IconPrint.png" style=" width:60px; height:60px;margin-left: 12px;" alt="" />
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>     
                              <table style="width: 575px; margin: auto;">
                                <tr>
                                    <td style=" border-bottom: 0px;" id="tdTestsetName">
                                        ตั้งชื่อว่า :&nbsp;                                    
                                        <asp:TextBox ID="txtName" runat="server" onclick="HidevalidateText();" Style="width: 485px" MaxLength="50"> 
                                        </asp:TextBox>
                                          <asp:Label ID="lblValidate" runat="server" Text="* ต้องกรอกชื่อด้วยนะคะ" 
                                        Width="482px" Visible="false" ForeColor="#FF3300"></asp:Label>
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="* ตั้งชื่อด้วยนะคะ" ForeColor="#FF3300" Display="Dynamic" Style="font-weight: 700"></asp:RequiredFieldValidator>--%>
                                    </td> 
                                </tr>
                            </table>                       
                        </div>                        
                    </div>
                </center>
                </div>
            </div>
            <div id="divButton" style="position: relative; height: 40px; width: 850px; margin: 0 auto;">
                <asp:Button Style="float: left; width: 150px; position: absolute; left: 0; margin: 0; font-size: 130%; height: 40px;"
                    ID="btnBack" ClientIDMode="Static" runat="server" Text="กลับ" class="submitChangeFontSize" />
                <asp:Button Style="float: right; width: 150px; position: absolute; right: 0; margin: 0; font-size: 130%; height: 40px;"
                    ID="btnSave" ClientIDMode="Static" runat="server" Text="บันทึก" class="submitChangeFontSize" />
            </div>
            <%-- </section>--%>
        </form>
        <%--<script type="text/javascript">                    var c = document.getElementById("select"); function resizeText(multiplier, e) { e.preventDefault(); if (c.style.fontSize == "") { c.style.fontSize = "1.0em"; } c.style.fontSize = parseFloat(c.style.fontSize) + (multiplier * 0.2) + "em"; } </script>--%>
        <footer style="margin-top: 15px">
                            <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
                        </footer>
    </div>
    <div id="dialogConfirm" title="บันทึกแล้วค่ะ">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />
    <%--<script src="<%=ResolveUrl("~")%>js/jquery.blockUI.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#MainContent_btnSave').click(function () {
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

            });
        });
    </script>--%>
    <script type="text/javascript">

        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        function HidevalidateText() {
            $('#HidevalidateText').hide();
        };

        $(function () {
            new FastButton(document.getElementById('btnBack'), TriggerServerButton);

            if (isAndroid) { new FastButton(document.getElementById('btnSave'), TriggerServerButton); }
            //new FastButton(document.getElementById('btnSave'), TriggerServerButton);

            //Check WebConfig RunMode
            var RunModeConfig = '<%= BusinessTablet360.ClsKNSession.RunMode %>';
            if (RunModeConfig == 'wordonly' || RunModeConfig == 'twotests') {
                $('.settingChkModeTestset').hide();
                $('#div-1').css('height', '100px');
                ; $('#ctl00_MainContent_txtName').css('width', '440px');
            }
            else if (RunModeConfig == 'standalonenotablet') {
                $('#ChkIsHomework').attr('disabled', 'disabled');
                $("label[for=ChkIsHomework]").css('background', 'url(../images/bullet-disable.gif) center left no-repeat');
                $('#lblForChkIsHomework').css('background', 'initial');
            }
            else if (RunModeConfig == 'labonly') {
                $('#ChkIsPractice').attr('disabled', 'disabled');
                $("label[for=ChkIsPractice]").css('background', 'url(../images/bullet-disable.gif) center left no-repeat');
                $('#lblForChkIsPractice').css('background', 'initial');
                $('#ChkIsHomework').attr('disabled', 'disabled');
                $("label[for=ChkIsHomework]").css('background', 'url(../images/bullet-disable.gif) center left no-repeat');
                $('#lblForChkIsHomework').css('background', 'initial');
            }

            //Chkbox ควิซ
            new FastButton(document.getElementById('DivChkIsQuiz'), TriggerChkIsQuiz);

            //Chkbox การบ้าน
            new FastButton(document.getElementById('DivChkIsHomework'), TriggerChkIsHomework);

            //Chkbox ฝึกฝน
            new FastButton(document.getElementById('DivChkIsPractice'), TriggerChkIsPractice);

            //Chkbox ออกรายงาน
            new FastButton(document.getElementById('DivChkIsPrintTestset'), TriggerChkIsPrintTestset);

            //ดักถ้าเข้าจาก Tablet ครู
            if (isAndroid) {
                //$('#main').css('width', '880px');
                //$('nav').css('width', '860px');
                //$('#site_content').css('width', '880px');
                //$('.content').css('width', '880px');
                //$('#ctl00_MainContent_btnNextStep4').css('margin-left', '670px');
                //$('footer').css('width', '860px');
                //$('#logo').css('width', '860px');
                //$('.slogantext').css('width', '860px');
                //$('.bordered').css({'width': '98%','margin-left': 'auto','margin-right': 'auto'});
                //$('p label').css('margin-left', '10px');
                //$('#tdTestsetName').css('font-size', '25px');
                //$('#txtName').css({ 'width': '450px', 'font-size': '25px' });
                //$('#btnBack').css({ 'height': '70px', 'width': '200px','font-size':'25px','left':'20px' });
                //$('#btnSave').css({ 'height': '70px', 'width': '200px','font-size': '25px','right':'20px' });
                //$('#divButton').css({ 'height': '70px' });
                //$('.settingChkModeTestset').css('font-size', '24px');
            }

            var Ispostback = '<%=CheckPostback %>';
            if (Ispostback == 'True') {
                $('#dialogConfirm').dialog({
                    buttons: {
                        'ตกลง': function () {
                            window.location = '<%=ResolveUrl("~")%>Testset/DashboardSetupPage.aspx';
                }
                }, draggable: false, resizable: false, modal: true
                }).dialog('open').dialog('option', 'title', 'บันทึกแล้วค่ะ');
                //if ($('.ui-button').length != 0) {
                //    $('.ui-button').each(function () {
                //        new FastButton(this, CloseDialogTestset);
                //    });
                //}
            }
        });
        function CloseDialogTestset() {
            window.location = '<%=ResolveUrl("~")%>Testset/DashboardSetupPage.aspx';
        }

        function TriggerChkIsQuiz() {
            if ($('#ChkIsQuiz').attr('checked') == 'checked') {
                $('#ChkIsQuiz').removeAttr('checked');
            }
            else {
                $('#ChkIsQuiz').attr('checked', 'checked');
            }
        }

        function TriggerChkIsHomework() {
            if ($('#ChkIsHomework').attr('checked') == 'checked') {
                $('#ChkIsHomework').removeAttr('checked');
            }
            else {
                $('#ChkIsHomework').attr('checked', 'checked');
            }
        }

        function TriggerChkIsPractice() {
            if ($('#ChkIsPractice').attr('checked') == 'checked') {
                $('#ChkIsPractice').removeAttr('checked');
            }
            else {
                $('#ChkIsPractice').attr('checked', 'checked');
            }
        }

        function TriggerChkIsPrintTestset() {
            if ($('#ChkIsPrintTestset').attr('checked') == 'checked') {
                $('#ChkIsPrintTestset').removeAttr('checked');
            }
            else {
                $('#ChkIsPrintTestset').attr('checked', 'checked');
            }
        }
    </script>
</asp:Content>
