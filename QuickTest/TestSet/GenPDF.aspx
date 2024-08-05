<%@ Page Title="QuickTest - ขั้นที่ 4: บันทึก และจัดพิมพ์ไฟล์ข้อสอบ" Language="vb"
    AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="GenPDF.aspx.vb"
    Inherits="QuickTest.GenPDF" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .Lightbox {
            background-color: Black;
            width: 100%;
            height: 100%;
            opacity: 0.7;
            position: absolute;
            top: 0;
            left: 0;
            z-index: 9999999;
        }

        .setupTestset {
            background-color: White;
            width: 520px;
            height: 340px;
            position: absolute;
            top: 50%;
            left: 50%;
            margin-left: -270px;
            margin-top: -170px;
            z-index: 99999999;
            padding: 10px;
            border-radius: 5px;
        }

            .setupTestset > div {
                background: url('../images/dialog_540_360.png');
                height: inherit;
            }

            .setupTestset input[type="text"] {
                background: rgba(239, 248, 251, 0.60) !important;
                border: 2px solid #60c9e6 !important;
                    font-weight: bold;
            }

            .setupTestset table {
                width: 100%;
                height: 100%;
                margin: initial;
            }

            .setupTestset span {
                /*background: rgba(239, 248, 251, 0.60) !important;*/
                padding: 2px 10px;
                border-radius: 5px;
                font-weight:bold;
            }

            .setupTestset table td:first-child {
                width: 40%;
                text-align: right;
                padding-right: 15px;
            }

            .setupTestset table td {
                background: initial;
                border-bottom: 0;
            }

        .Forbtn {
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
            /*background: #46C4DD;
            background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
            background: -moz-linear-gradient(top, #63CFDF, #17B2D9);*/
            text-shadow: 1px 1px #178497;
            behavior: url('../css/PIE.htc');
            -pie-track-active: false;
            margin-left: 40px;
            width: 100px;
            background: #63cfdf; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iIzYzY2ZkZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiMxN2IyZDkiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #63cfdf 0%, #17b2d9 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#63cfdf), color-stop(100%,#17b2d9)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #63cfdf 0%,#17b2d9 100%); /* IE10+ */
            background: linear-gradient(to bottom, #63cfdf 0%,#17b2d9 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63cfdf', endColorstr='#17b2d9',GradientType=0 ); /* IE6-8 */
        }

        .ddl {
            height: 42px;
            font: 90% 'THSarabunNew';
            border: 2px solid #60c9e6;
            background: #EFF8FB;
            color: #444;
            border-radius: 7px 0 0 7px;
            background: rgba(239, 248, 251, 0.60) !important;
            font-weight:bold;
            width:305px;
        }

        .txtTime {
            text-align: center;
        }

        body {
            background : none!important;
        }
    </style>
      <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
        <style type="text/css">
            html {
                background-image:none !important;
            }
    </style>
    <%End If %>
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <% If Not IE = "1" Then%>
<%--    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
<%--    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
    <%End If%>
    <telerik:RadCodeBlock ID='RadCodeblock1' runat="server">
        <script type="text/javascript">
            document.createElement('article');
            document.createElement('aside');
            document.createElement('figure');
            document.createElement('footer');
            document.createElement('header');
            document.createElement('hgroup');
            document.createElement('nav');
            document.createElement('section');

            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;
            $(function () {
                //ดักถ้าเข้าจาก Tablet ขงอครู
                if (isAndroid) {
                    $('#main').css('width', '885px');
                    $('#site_content').css('width', '885px');
                    $('.content').css('width', '885px');
                    $('#div-1').css('width', '795px');
                    $('#btnCancel').css({ 'width': '170px', 'height': '65px', 'font-size': '25px', 'margin-left': '10px' });
                    $('#btnOk').css({ 'width': '170px', 'height': '65px', 'font-size': '25px', 'margin-right': '10px' });
                    $('#btnPreview').css({ 'height': '70px', 'font-size': '20px' });
                    $('#btnDownload').css({ 'height': '70px', 'font-size': '22px' });
                    $('#btnAnswerSheet').css({ 'height': '70px', 'font-size': '22px' });
                    $('#BtnBack').css({ 'width': '200px', 'height': '70px', 'font-size': '22px' });
                }

            });

        </script>
        <script type="text/javascript">
            /* css for IE,Firefox */
            $(function () {
                if ($.browser.msie || !!navigator.userAgent.match(/Trident\/7\./) || navigator.userAgent.indexOf('Firefox') > -1) {
                    $('.setupTestset input').css({ 'height': '40px', 'padding-top': '0px', 'padding-bottom': '0px' });
                    $('.setupTestset').css({ 'padding-top': '0px', 'padding-bottom': '17px' });
                    $('.setupTestset .submit').css('position', 'relative');
                    $('.setupTestset table td:first-child').css('padding-left', '10%');
                    $('#btnCancel').css('margin-left', '0px');
                }
            });
        </script>

        <script type="text/javascript">
            function ClickGenWord(TypeToGen) {
                if (TypeToGen == 'SomeExam') {
                    $('#<%=btnPreview.ClientID %>').trigger('click');
                }
                else if (TypeToGen == 'FullExam') {
                    //alert('เข้า FullExam');
                    $('#<%=btnDownload.ClientID %>').trigger('click');
                }
                else if (TypeToGen == 'ShowCorrectAnswer') {
                    //alert('เข้า ShowCorrectAnswer');
                    $('#<%=btnAnswerSheet.ClientID %>').trigger('click');
                }
    }
        </script>
        <script type="text/javascript">
            function minmax(value, min, max) {
                console.log(value);
                if (parseInt(value) < min || isNaN(value)) {
                    return 1;
                } else if (parseInt(value) > max) {
                    return 240;
                } else {
                    return value;
                }
            }
            function checkIsnullvalue(value) {
                if (value == "") {
                    return 60;
                }
                return value;
            }
        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="saveset" runat="server">
        <div id="main">
            <%--       <header class="thinheader">
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
          <li><a href="step4.aspx" class="current"><%=txtStep4 %></a></li>	
           <%If HttpContext.Current.Application("NeedQuizMode") = True Then%>
          <li><a href="#">ขั้นที่ 5: จัดพิมพ์,เริ่มควิซ</a></li>	
          <%End If%>
          </ul>
        </div>
      </nav>
    </header>--%>
            <div id="site_content" style="border-radius: 10px;">                
                <div class="content" style="width: 930px;">
                    <center>
                    <h2>
                       เลือก ดาวน์โหลดไฟล์ข้อสอบ
                    </h2>
                        <span style="font-size:90%; color:red;">** กรุณาตรวจทานความถูกต้องของรูปแบบอักษร การตัดคำและการจัดหน้าในไฟล์ Word ก่อนพิมพ์นะคะ **</span>
                    <div id="div-1">
                        <table>
                            <tr>
                                <td style="border-bottom: initial;">
                                    <div class="form_settings">
                                        <table>
                                            <tr>
                                                <td style="text-align: center; width: 33%; border-bottom: 1px solid white;display:none;">
                                                    <a onclick='ClickGenWord("SomeExam")' style="text-decoration: none; cursor: pointer;">
                                                        <img alt="โหลดไฟล์ดัชนีชี้วัด" src="../images/preview.png" /></a>
                                                </td>
                                                <td style="text-align: center; width: 34%; border-bottom: 1px solid white;">
                                                    <a onclick='ClickGenWord("FullExam")' style="text-decoration: none; cursor: pointer;">
                                                        <img alt="โหลดไฟล์ข้อสอบทั้งหมดที่เลือกมา" src="../images/download.png" /></a>
                                                </td>
                                                <td style="text-align: center; width: 33%; border-bottom: 1px solid white;">
                                                    <a onclick='ClickGenWord("ShowCorrectAnswer")' style="text-decoration: none; cursor: pointer;">
                                                        <img alt="โหลดไฟล์เฉลยประกอบข้อสอบฉบับเต็ม" src="../images/answer.png" /></a>
                                                </td>
                                                <%If HttpContext.Current.Application("runmode") <> "twotests" Then%>
                                                 <td style="text-align: center; width: 33%; border-bottom: 1px solid white;">
                                                    <a onclick='ClickGenWord("ShowCorrectAnswer")' style="text-decoration: none; cursor: pointer;">
                                                        <img alt="โหลดไฟล์เฉลยประกอบข้อสอบฉบับเต็ม พร้อมคำอธบาย" src="../images/Answer_Wexplain.png" /></a>
                                                </td>
                                                <%End If %>
                                            </tr>
                                            <tr style="height: 40px;">
                                                <td style="border-bottom: 1px solid white; width: 33%; text-align: center;display:none;">
                                                    <%--   <a href="../report/<% =Session("OutputFileName") %>-preview.pdf" target="_blank"
                                                        style="text-decoration: none;">--%>
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 130px; position: relative;" ID="btnPreview"
                                                        runat="server" Text="ดัชนีชี้วัด" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                </td>
                                                <td style="border-bottom: 1px solid white; width: 34%; text-align: center;">
                                                    <%--     <a href="../report/<% =Session("OutputFileName") %>.pdf" target="_blank" style="text-decoration: none;">--%>
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 150px; position: relative;" ID="btnDownload"
                                                        runat="server" Text="ไฟล์เต็ม (Word)" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                </td>
                                                <td style="border-bottom: 1px solid white; width: 33%; text-align: center;">
                                                    <%-- <a href="../report/<% =Session("OutputFileName") %>-answer.pdf" target="_blank" style="text-decoration: none;">--%>
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 160px; position: relative;" ID="btnAnswerSheet"
                                                        runat="server" Text="ไฟล์เฉลย (Word)" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                </td>
                                                    <%If HttpContext.Current.Application("runmode") <> "twotests" Then%>
                                                 <td style="border-bottom: 1px solid white; width: 33%; text-align: center;">                                                  
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 250px; position: relative;" ID="btnAnwserSheetWithExplain"
                                                        runat="server" Text="ไฟล์เฉลย + คำอธิบาย (Word)" ClientIDMode="Static" class="submit"  />
                                                </td>
                                                <%End If %>
                                            </tr>
                                            <tr style="height:20px;"></tr>
                                            <tr style="height: 40px; display:none;">
                                                <td style="border-bottom: 1px solid white; width: 33%; text-align: center;display:none;">                                                   
                                                </td>
                                                <td style="border-bottom: 1px solid white; width: 34%; text-align: center;">                                                  
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 150px; position: relative;" ID="btnDownloadQuestionSheetPDF"
                                                        runat="server" Text="ไฟล์เต็ม (.PDF)" ClientIDMode="Static" class="submit" />
                                                </td>
                                                <td style="border-bottom: 1px solid white; width: 33%; text-align: center;">
                                                   
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 160px; position: relative;" ID="btnDownloadAnswerSheetPDF"
                                                        runat="server" Text="ไฟล์เฉลย (.PDF)" ClientIDMode="Static" class="submit" />
                                                </td>
                                                 <td style="border-bottom: 1px solid white; width: 33%; text-align: center;">                                                  
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 250px; position: relative;" ID="btnDownloadAnwserSheetWithExplainPDF"
                                                        runat="server" Text="ไฟล์เฉลย + คำอธิบาย (.PDF)" ClientIDMode="Static" class="submit"  />
                                                </td>
                                            </tr>
                                            <%-- <tr>
                                                <td colspan="3;" style="border-bottom: initial;">                                               
                                                    <div style="height: 40px; margin-left: auto;margin-top:10px; margin-right: auto; width: 510px; background-image: url('../Images/Activity/Logo.png');
                                                        background-repeat: no-repeat; background-position: right;" id="divUseTemplate">
                                                        <asp:CheckBox ID="chkUseTemplate" runat="server" Text="ให้เด็กทำด้วยกระดาษคำตอบคอมพิวเตอร์" /></div>
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <%-- <div class="facebook" style="width: 100%; margin-top: 10px; height: 40px; text-align: center;">
                            <iframe src="http://www.facebook.com/plugins/like.php?locale=th_TH&amp;href=http://www.wpp.co.th/quicktest/&amp;layout=button_count&amp;show_faces=true&amp;action=like&amp;font&amp;colorscheme=light&amp;height=23"
                                scrolling="no" frameborder="0" style="border: none; overflow: hidden; width: 100px;
                                height: 30px; text-align: center;" allowtransparency="true"></iframe>
                        </div>--%>
                    </div>
             <%--       <footer style="margin-top: 40px">
                            <a href="http://www.wpp.co.th">สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด</a>
                        </footer>--%>
                </center>
                </div>
                <asp:Button ID="BtnBack" ClientIDMode="Static" runat="server" CssClass='Forbtn' Text="กลับ" />
            </div>
        </div>
        <div id="divLightbox" class="Lightbox" runat="server">
        </div>
        <div id="setTestset" class="setupTestset" runat="server">
            <div>
                <table>
                    <tr>
                        <td><span>ตั้งชื่อว่า</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTestsetName" runat="server" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><span>ให้เวลาทำ</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTestsetTime" runat="server" Width="40px" MaxLength="3" onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup="this.value = minmax(this.value,1,240)" onfocusout="this.value = checkIsnullvalue(this.value)" CssClass="txtTime"></asp:TextBox><label
                                style="margin-left: 10px;font-weight:bold;">นาที</label>
                        </td>
                    </tr>
                    <tr>
                        <td><span>ตัวหนังสือ</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTextSize" runat="server" CssClass="ddl">
                                <asp:ListItem Value="6BF52DC7-314C-40ED-B7F3-BCC87F724880">ขนาดตัวปกติ (สำหรับเด็กมัธยม)</asp:ListItem>
                                <asp:ListItem Value="93B163B6-4F87-476D-8571-4029A6F34C84">ขนาดตัวใหญ่ (สำหรับประถมปลาย)</asp:ListItem>
                                <asp:ListItem Value="5F4765DB-0917-470B-8E43-6D1C7B030818">ขนาดตัวใหญ่มาก (สำหรับประถมต้น)</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%If ConfigurationManager.AppSettings("EnablePrefixForRunningNoInWordFile") = True Then%>
                    <tr>
                        <td><span>รหัสนำหน้าเลขข้อ</span>
                        </td>
                        <td>
                            <%--onkeyup="this.value=this.value.replace(/[^a-z]/g,'');"--%>
                            <asp:TextBox ID="txtPrefixRunningNo" runat="server" Width="38" MaxLength="3" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <% End If%>
                    <%If Convert.ToBoolean(HttpContext.Current.Application("NeedCheckmark")) = True Then%>
                    <tr>
                        <td colspan="2" style="width: initial; text-align: left; padding-left: 100px;">

                            <asp:CheckBox ID="chkUseCheckmark" runat="server" Text="ใช้กับกระดาษคำตอบ" />
                            <%--<img
                            id="imgCheckmark" src="../images/activity/setting/Settings-btnQuizMode-CheckMark_small.png" alt="" style="position: absolute; margin-top: -3px;" />--%>
                            <img
                                id="img1" src="../images/activity/logocheckpoint.png" alt="" style="position: absolute; margin-top: -18px;" />
                        </td>
                    </tr>
                    <% End If%>
                    <tr>
                        <td style="padding-left: initial; text-align: left;">
                            <asp:Button ID="btnCancel" runat="server" Text="" Style="margin-left: 50px; background-image: url('../images/btn_Cancel.png')!important; background-size: cover!important;" ClientIDMode="Static"
                                class="submit" />
                        </td>

                        <td style="text-align: right;">
                            <asp:Button ID="btnOk" runat="server" Text="" Style="margin-right: 50px; background-image: url('../images/btn_OK.png')!important; background-size: cover!important;" ClientIDMode="Static" class="submit" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~")%>js/jquery.blockUI.js" type="text/javascript" charset="utf-8"></script>
    <%--<link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css"
        media="screen" charset="utf-8" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
   
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $.prettyLoader();
        });
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
                modal: true
            });
            $.prettyLoader();

        });
  
    </script>--%>
    <style type="text/css">
        .setupTestset .submit {
            position: initial;
            margin: initial;
            font-size: initial;
            border: initial;
            background: initial !important;
            width: 120px;
            height: 40px;
            -webkit-box-shadow: initial;
            box-shadow: initial;
        }

        .setupTestset input {
            height: 20px;
            font-size: 20px;
            padding: 10px;
        }

        .setupTestset td {
            font-size: 20px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(':submit').off('click');
        });
    </script>
</asp:Content>
