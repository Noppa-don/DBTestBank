<%@ Page Title="QuickTest - ขั้นที่ 2: เลือกหลักสูตร และ วิชาที่ต้องการนำมาจัดเป็นข้อสอบใหม่"
    Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Step2.aspx.vb"
    Inherits="QuickTest.Step2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script src="../js/json2.js" type="text/javascript"></script>
    <%If Not IE = "1" Then%>
<%--    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>--%>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>";</script>
<%--    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>--%>
    <%End If%>
    <script type="text/javascript">
        function GotoReport(GId, LId) {
            window.location = '../Testset/ReportManageExam.aspx?gId=' + GId + '&LId=' + LId;
        }
    </script>

    <%If IsAndroid = True Then%>
    <style type="text/css">
        #main, #site_content, .content {
            width: 880px !important;
        }

        nav, footer, #logo, .slogantext {
            width: 860px !important;
        }

        #btnNextStep3 {
                margin-top: -66px;
    width: 200px;
    height: 40px;
    font-size: 100%;
    top: 0px;
    position: inherit;
    float: right;
}


        .bordered {
            width: 98% !important;
            margin-left: auto !important;
            margin-right: auto !important;
        }

        table tr td {
            font-size: 20px !important;
        }

        p label {
            margin-left: 10px !important;
        }

         
    </style>
    <%Else %>
    <style type="text/css">
        .ForTimebtnMenu {
            font: 100% 'THSarabunNew';
            width: 50%;
            height: 45px;
            line-height: 45px;
            display: inline-block;
            text-align: center;
            cursor: pointer;
            color: #fff;
            border: 0;
            background: transparent;
            text-transform: uppercase;
            text-decoration: none;
            border-right: 1px solid;
        }
        .ForDivBtn {
            
            width: 100%;
            border: solid 1px #4f674f;
            text-align: center;
            display: table;
            margin-top: 20px;
            /*background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top, #FAA51A, #F47A20);*/
            background: #faa51a; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #96c396 0%,#4f674f 100%); /* IE10+ */
            background: linear-gradient(to bottom, #96c396 0%,#4f674f 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 ); /* IE6-8 */
        }

        #btnMattayom {
            border-right:none;
        }
    </style>
    <%End If%>

    <%If BusinessTablet360.ClsKNSession.RunMode.ToLower = "twotests" Then %>
        <style type="text/css">
            html {
                background-image:none !important;
            }
    </style>
    <%End If %>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../js/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <%-- <script src="../js/facescroll.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {

            //ดักถ้าเข้าจาก Tablet ครู
            if (isAndroid) {
                //chkbox 51 , 44
                new FastButton(document.getElementById('Divy51'), TriggerChky51);
                new FastButton(document.getElementById('Divy44'), TriggerChky44);

                //$('#main').css('width', '880px');
                //$('nav').css('width', '860px');
                //$('#site_content').css('width', '880px');
                //$('.content').css('width', '880px');
                //$('#btnNextStep3').css({'margin-left': '670px','height':'70px','font-size':'20px'});
                //$('footer').css('width', '860px');
                //$('#logo').css('width', '860px');
                //$('.slogantext').css('width', '860px');
                //$('.bordered').css({ 'width': '98%', 'margin-left': 'auto', 'margin-right': 'auto' });
                //$('table tr td').css('font-size', '20px');
                //$('p label').css('margin-left', '10px');
            }

            $('#btnPratom').click(function () {
                $('#btnPratom').css('color', "yellow");
                $('#btnMattayom').css('color', "white");
                $('#ListPratom').show();
                $('#ListMattayom').hide();
                
            });
            $('#btnMattayom').click(function () {
                $('#btnPratom').css('color', "white");
                $('#btnMattayom').css('color', "yellow");
                $('#ListPratom').hide();
                $('#ListMattayom').show();
            });
            
        });



        function TriggerChky51() {
            if ($('#y51').attr('checked') == 'checked') {
                $('#y51').removeAttr('checked');
            }
            else {
                $('#y51').attr('checked', 'checked');
            }
        }

        function TriggerChky44() {
            if ($('#y44').attr('checked') == 'checked') {
                $('#y44').removeAttr('checked');
            }
            else {
                $('#y44').attr('checked', 'checked');
            }
        }


    </script>
<div id="main" style="width: 954px; background-color: white; padding-bottom: 10px;">
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
          <li><a href="#" class="current"><%=txtStep2 %></a></li> 
          <li><a href="#" style="cursor:not-allowed;"><%=txtStep3 %></a></li>		
          <li><a href="#" style="cursor:not-allowed;"><%=txtStep4 %></a></li>
          <%Try%>
        <%If HttpContext.Current.Application("NeedQuizMode").ToString() = "True" Then%>
          <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 5: จัดพิมพ์,เริ่มควิซ</a></li>	
           <%End If%>
		   <%Catch%>
           <%Response.Redirect("~/LoginPage.aspx", False)%>
           <%End Try%>	
           </ul>--%>          
           <ul class="sf-menu" id="nav" style="font-size:20px;text-align:center;">
		    <li><a href="../Testset/DashboardSetupPage.aspx"><img id="imgBack" src="../Images/Home.png" style="position: absolute; margin-left: 5px; margin-top: -8px; cursor: pointer;"></a></li> 
            <li style="margin-left:45px;width:190px;"><a href="#" class="current">ขั้นที่ 1 เลือกวิชา --></a></li> 
            <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 2 เลือกหน่วยการเรียนรู้ --></a></li> 
            <li><a href="#" style="cursor:not-allowed;">ขั้นที่ 3 บันทึก</a></li>
          </ul>
        </div>
      </nav>           
    </header>
        <form id="step2" runat="server">
            <div id="site_content" style="padding: 15px 0 0 10px; margin: auto;    display: inline-block;">
                <%If HttpContext.Current.Session("EditID") IsNot Nothing And HttpContext.Current.Session("EditID") <> "" Then %>
                    <div style="text-align: center;">
                        <span style="color: red; font-size: 20px;"><%= EditTestSetWarningText %></span>
                    </div>
                <%End If %>
                
                <div class="content" style="width: 930px; margin: 0 0 5px 0;">
                    <section id="select">
                        <div>

                      
                                    <img style="float: left; vertical-align: middle; margin: 0 10px 0 0;" 
                                        src="<%=ResolveUrl("~")%>images/another_page.png" alt="another page" />
                           
                                    <h2 style="margin: -20px 0 0 0;">เลือกวิชา</h2>
                            
                                    <asp:Button ID="btnNextStep3" ClientIDMode="Static" runat="server" class="submitChangeFontSize" Text="Next - ไปต่อขั้น 2 >>"
                                        Style="width: 200px; position: initial; height: 40px; float: right; margin-top: -66px;"></asp:Button>
                                  </div>
                            <div>
                                <p><label>เลือกวิชาที่ต้องการจัดชุดข้อสอบค่ะ</label></p>
                            </div>

                        <p>
                            <div id="Divy51" style="display:none;width:60px;">
                                <input type="checkbox" <%= GetYearChecked(51)%> name="y51" value="" id="y51"  />
                                <label for="y51">ปี 51</label> 
                            </div>
                            <div id="Divy44" style="display:none;width:100px;">
                                <input type="checkbox" <%= GetYearChecked(44)%> name="y44" value="" id="y44"  />
                                <label for="y44" style="margin-left:20px;">ปี 44</label>
                            </div>
                        </p>
                        

                        <div id='DivTimeFilter' class='ForDivBtn' style="width: 99.8%;border-radius: 0.5em;">
                            <input type="button" id="btnPratom"  value="ประถม" class='ForTimebtnMenu' style="color:yellow;" />
                            <input type="button"  id="btnMattayom" value="มัธยม" class='ForTimebtnMenu' />
                        </div>

                        <div id="ListPratom">
                            <asp:Repeater id="ListingMaster" runat="server">
                                <HeaderTemplate>
                                    <table class="bordered" style="width:100%; border-spacing:0;">
                                        <tr><th>ชั้น</th><th colspan='<%# ColsapnAmount%>' style='text-align:center'>วิชา</th></tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.DataItem("Level_ShortName")%></td>
                                            <%# CreateSubjectList(Eval("Level_Id").ToString)%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
          
                        <div id="ListMattayom" style="display:none;">
                            <asp:Repeater id="ListMaster2" runat="server">
                                <HeaderTemplate>
                                    <table class="bordered" style="width:100%; border-spacing:0;">
                                        <tr><th>ชั้น</th><th colspan='<%# ColsapnAmount%>' style='text-align:center'>วิชา</th></tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.DataItem("Level_ShortName")%></td>
                                            <%# CreateSubjectList(Eval("Level_Id").ToString)%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
		            </section>
                </div>
            </div>
        </form>
        <footer style="margin-top: 15px">
      <%--<a href="http://www.wpp.co.th"></a>--%>สงวนลิขสิทธิ์ &copy; บริษัท สำนักพิมพ์วัฒนาพานิช จำกัด
    </footer>
</div>
    <div id="dialog"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" />

    <script type="text/javascript">

        var isAddQuestionByUser = '<%=BusinessTablet360.ClsKNSession.IsAddQuestionBySchool%>';
        //console.log(typeof isAddQuestionByUser);
        $(function () {
            //new FastButton(document.getElementById('btnNextStep3'), TriggerServerButton);

            //Checkbox ที่เลือกชั้น , วิชา
            if ($('.divchk').length != 0) {
                $('.divchk').each(function () {
                    new FastButton(this, TriggerChk);
                });
            }
            
            $('#btnNextStep3').click(function (e) {
                //alert(isAddQuestionByUser);
                if (isAddQuestionByUser == "True") {
                    var checkedAmount = $("input[type='checkbox']:checked").length;
                    console.log(checkedAmount);
                    if (checkedAmount == 2) {
                        e.preventDefault();
                        callDialog('เลือกวิชาก่อนค่ะ!');
                        return false;
                    }
                }                            
            });

            // ปิดวิชาอังกฤษ ม.ปลายไปก่อน
            $('#SC_อังกฤษ_2e0ffc04-bcee-45be-9c0c-b40742523f43').attr("disabled", "disabled");
            $('#SC_อังกฤษ_2e0ffc04-bcee-45be-9c0c-b40742523f43').next().css("color", "gray");
            $('#SC_อังกฤษ_2e0ffc04-bcee-45be-9c0c-b40742523f43').next().css("background-image", "url(../images/bullet-disable.gif)");

            $('#SC_อังกฤษ_6736d029-6b78-4570-9dbb-991217da8fee').attr("disabled", "disabled");
            $('#SC_อังกฤษ_6736d029-6b78-4570-9dbb-991217da8fee').next().css("color", "gray");
            $('#SC_อังกฤษ_6736d029-6b78-4570-9dbb-991217da8fee').next().css("background-image", "url(../images/bullet-disable.gif)");

            $('#SC_อังกฤษ_6bf52dc7-314c-40ed-b7f3-bcc87f724880').attr("disabled", "disabled");
            $('#SC_อังกฤษ_6bf52dc7-314c-40ed-b7f3-bcc87f724880').next().css("color", "gray");
            $('#SC_อังกฤษ_6bf52dc7-314c-40ed-b7f3-bcc87f724880').next().css("background-image", "url(../images/bullet-disable.gif)");
        });

        function TriggerChk(e) {
            var obj = e.target;
            if ($(obj).prop('tagName') == 'LABEL') {
                if ($(obj).prev().attr('checked') == 'checked') {
                    $(obj).prev().removeAttr('checked');
                }
                else {
                    $(obj).prev().attr('checked', 'checked');
                }
            }
            else {
                if ($(obj).attr('checked') == 'checked') {
                    $(obj).removeAttr('checked');
                }
                else {
                    $(obj).attr('checked', 'checked');
                }
            }
            //alert('zxc');
        }

        function callDialog(txt) {
            var $d = $('#dialog');
            var myBtn = {};          
            myBtn["ตกลง"] = function () {
                $d.dialog('close');                
            };
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', txt);
        }
    </script>

</asp:Content>
