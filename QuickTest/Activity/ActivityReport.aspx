<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ActivityReport.aspx.vb"
    Inherits="QuickTest.ViewReport" Theme="Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script src="../js/Animation.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script src="../js/highcharts.js" type="text/javascript"></script>
    <script src="../js/jquery.qtip-1.0.0-rc3.js" type="text/javascript"></script>
    <link href="../css/menuFixReviewAns.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.prettyLoader.js" type="text/javascript"></script>
    <script src="../js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <link href="../css/prettyPhoto.css" rel="stylesheet" type="text/css" />



    <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
       
        
        <script type="text/javascript">

            var JsCheckIsFromReportMenu = '<%=CheckIsFromReportMenu %>';
            var JsMode = '<%=Mode%>';
            var testsetName = '<%= FullTestsetName%>';

            var ua = navigator.userAgent.toLowerCase();
            var isAndroid = ua.indexOf("android") > -1;

            $(function () {
                //qtip - TestsetName
                $('#lblTestSetName').qtip({
                    content: testsetName,
                    show: { event: 'mouseover' },
                    style: {
                        width: 200, padding: 5, background: '#F68500', color: 'white', textAlign: 'center', border: { width: 7, radius: 5, color: '#F68500' }, tip: 'topLeft', name: 'dark', 'font-weight': 'bold'
                    },
                    position: { corner: {target:'bottomMiddle'} },
                    hide: { when: { event: 'mouseout' }, fixed: false }
                });
                
                //hover animation
                InjectionHover('#imgBack', 5, false);
                InjectionHover('#ToggleChartState1', 3);
                InjectionHover('#BtnExportExcel', 5);
                InjectionHover('#btnFilter', 3);
                
                if ($('#txtFilter').val() == '') {
                    $('#btnFilter').css('opacity', '0.2');
                    $('#btnFilter').css('cursor', 'context-menu');
                    $('#lblFilterDetail').css('display', 'none');
                    InjectionHover('#btnFilter', 0);

                }

                $('#txtFilter').keyup(function () {
                    if ($('#txtFilter').val() == '') {
                        $('#btnFilter').css('opacity', '0.2');
                        $('#btnFilter').css('cursor', 'context-menu');
                        $('#lblFilterDetail').css('display', 'none');
                        InjectionHover('#btnFilter', 0);
                    }else{
                        $('#btnFilter').css('opacity', '');
                        $('#btnFilter').css('cursor', 'auto');
                        InjectionHover('#btnFilter', 3);
                    }
                });
            
                
                //Img Back Click
                new FastButton(document.getElementById('imgBack'), ImgBackClick);

                //ToggleChartState1 Click
                new FastButton(document.getElementById('ToggleChartState1'), TriggerToggleChartState)

                if (isAndroid) {
                    $('#DivReport').css('width', '500px');
                    $('#AllChart').css('width', '500px');
                    $('#LeftPanel').css('margin-top', '-5%');
                }
                
                if (JsCheckIsFromReportMenu == 'True') {
                    $('#imgBack').show();
                }

                //$('#imgBack').click(function () {
                //    if (JsMode == '1') {
                //        window.location = '<%=ResolveUrl("~")%>Quiz/RepeaterReportQuiz.aspx';
                //   }
                //   else if (JsMode == '2') {
                //       window.location = '<%=ResolveUrl("~")%>Homework/RepeaterReportHomework.aspx';
                //  }
                //  else if (JsMode == '3') {
                //      window.location = '<%=ResolveUrl("~")%>Practice/RepeaterReportPractice.aspx';
                //    }
                //});

                $("a[rel^='prettyPhoto']").prettyPhoto({
                    default_width: 800,
                    default_height: 600,
                    modal: true
                });
                $.prettyLoader();

                //-----------------------------------------------------------------------------------
                $('#DivReportTable').hide();

                if ('<%=CheckQuizIsUseTablet%>' == 'True') {
                   CheckChartOrTable();
                    }

                //------------------------------------------------------------ Onclick สำหรับ State 1
                //$('#ToggleChartState1').click(function () {
                    //if ($('#ToggleChartState1').attr('TypeChartOrTbl') == 'Chart') {
                    //    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    //    $('#DivReport').stop(true, true).hide(300);
                    //    $('#DivReportTable').stop(true, true).show(300);
                    //    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Table');
                    //    $('#ToggleChartState1').attr('src', '../Images/graph2.png');
                    //    $('#ToggleChartState1').attr('title', 'ดูแบบกราฟ');
                    //    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    //    $('#CheckType').val(2);
                    //}
                    //else {
                    //    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    //    $('#DivReportTable').stop(true, true).hide(300);
                    //    $('#DivReport').stop(true, true).show(300);
                    //    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Chart');
                    //    $('#ToggleChartState1').attr('src', '../Images/table.png');
                    //    $('#ToggleChartState1').attr('title', 'ดูแบบตาราง');
                    //    $('#CheckType').val(1);
                    //    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    //}
                //});

            });

            function ImgBackClick() {
                if (JsMode == '1') {
                    window.location = '<%=ResolveUrl("~")%>Quiz/RepeaterReportQuiz.aspx';
                  }
                  else if (JsMode == '2') {
                      window.location = '<%=ResolveUrl("~")%>Homework/RepeaterReportHomework.aspx';
                    }
                    else if (JsMode == '3') {
                        window.location = '<%=ResolveUrl("~")%>Practice/RepeaterReportPractice.aspx';
                  }
            }

            function TriggerToggleChartState() {
                if ($('#ToggleChartState1').attr('TypeChartOrTbl') == 'Chart') {
                    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    $('#DivReport').stop(true, true).hide(300);
                    $('#DivReportTable').stop(true, true).show(300);
                    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Table');
                    $('#ToggleChartState1').attr('src', '../Images/graph2.png');
                    $('#ToggleChartState1').attr('title', 'ดูแบบกราฟ');
                    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    $('#CheckType').val(2);
                }
                else {
                    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                    $('#DivReportTable').stop(true, true).hide(300);
                    $('#DivReport').stop(true, true).show(300);
                    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Chart');
                    $('#ToggleChartState1').attr('src', '../Images/table.png');
                    $('#ToggleChartState1').attr('title', 'ดูแบบตาราง');
                    $('#CheckType').val(1);
                    //alert($('#ToggleChartState1').attr('TypeChartOrTbl'));
                }
            }


        //---------------------------------------------------------------------

        function CheckChartOrTable() {
            if ($('#CheckType').val() == 1) {
                    $('#DivReportTable').hide();
                    $('#DivReport').show();
                    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Chart');
                    $('#ToggleChartState1').attr('src', '../Images/table.png');
                    $('#ToggleChartState1').attr('title', 'ดูแบบตาราง');
            }
            else
            {
                    $('#DivReport').hide();
                    $('#DivReportTable').show();
                    $('#ToggleChartState1').attr('TypeChartOrTbl', 'Table');
                    $('#ToggleChartState1').attr('src', '../Images/graph2.png');
                    $('#ToggleChartState1').attr('title', 'ดูแบบกราฟ');
            }
        }

            // เฉพาะตัวเลข
        //function inputOnlyNumbers(e) {
        //    var charCode;
        //    if (e.charCode > 0) {
        //        charCode = e.which || e.keyCode;
        //    } else if (typeof (e.charCode) != "undefined") {
        //        charCode = e.which || e.keyCode;
        //    }
        //    if (charCode == 46)
        //        return true
        //    if (charCode > 31 && (charCode < 48 || charCode > 57))
        //        return false;
        //    return true;
        //    //if (e.charCode >= 46 && e.charCode <= 57) {
        //    //    console.log(true);
        //    //    return e.charCode;
        //    //}
            
            //}

        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split(".");
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)                
            } else return o.getSelectionStart
        }
        </script>

    </telerik:RadScriptBlock>

    <style type="text/css">
        @import url(../fonts/thsarabunnew.css);
        .grid
        {
            margin-left: auto;
        }
        .style1
        {
            width: 222px;
        }
      .RadGrid_GirdQuickTest .rgMasterTable
        {
            margin-left:0%;
            margin-top:0%;
            }
            span
           {
               font:normal 100% 'THSarabunNew';
               color:#F68500;
               font-size:160%;
               }
               /*h2
               {
                   font:normal 100% 'THSarabunNew';
                   color:#F68500;
                   font-size:190%;
                   }*/
               label
               {
                   font:bold italic 15px 'THSarabunNew';
                   color:#555;
                   }
        .ForLessThanFilter {
        background-color:#FBB3B3;
        color:black;
        font-size:18px;
        }
        .ForMoreThanFilter {
        background-color:#AEECB2;
        color:black;
        font-size:18px;
        }
          .ForOddRow {
        background-color:#9CDAE8;
        color:black;
        font-size:18px;
        }
            .ForEvenRow {
        background-color:#DCF4F9;
        color:black;
        font-size:18px;
        }
        .RadGrid_GirdQuickTest .rgHeader:first-child, .RadGrid_GirdQuickTest th.rgResizeCol:first-child, .RadGrid_GirdQuickTest .rgFilterRow > td:first-child, .RadGrid_GirdQuickTest .rgRow > td:first-child, .RadGrid_GirdQuickTest .rgAltRow > td:first-child
        {
            padding:11px;
            }
        #DivNoTabletQuiz
        {
            width: 124%;
            position: absolute;
            top: 9%;
            border-color: orange;
            border-radius: 12px;
            margin: 0px auto 0 auto;
            padding: 10px 0 0 0;
            text-align: center;
            display: block;
            line-height: 456px;
            border-width: 2px;
            background-color: white;
            border-style: dashed;
            font-size: 26px;
            height: 115%;
        }
    </style>
</head>
<%--<body style='background-color:#FDF8F8;'>--%>
<body style='background-color:#EFF8FB;'>

    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    

    <div id='MainDiv' class="mainDiv" style='z-index:-999;background-color:#EFF8FB;border:none;'>

    <img src="../Images/ReportGraph/btnBack.png" id='imgBack' style='display:none;position:absolute;width:50px;cursor:pointer;top:-30px;left:95px;' />

    <asp:Label ID="lblTestSetName" Width='500' Height='70' style='font-weight:bold;position:relative;top:-22px;font-size:40px;left:260px;display:block;overflow:hidden;' runat="server" Text=""></asp:Label>
    
        <table id='LeftPanel' style='float: left; margin-top:-8%;'>
            <tr>
                <td style='border-bottom: 2px solid white;padding-top:5px;padding-bottom:5px;' class="style1">
                    <center>
                  
                        <div>
                                  <span>คะแนน</span>
                                <div>
                            <asp:Label ID="lblCompare" runat="server" Text=""></asp:Label> <%--เปรียบเทียบกับห้องอื่น--%></div>
                        
                            <asp:Label ID="lblClassAndRoom" runat="server" Text=""></asp:Label> 
                    
                            <%--<asp:Label ID="lblTestSetName" runat="server" Text=""></asp:Label>--%> <%--ชุดกิจกรรม - สังคมวันนี้--%>
              
                        </div>
                   
                    </center>
                </td>
            </tr>
            <tr>
                <td style='border-bottom: 2px solid white;padding-top:5px;padding-bottom:5px;' class="style1">
                    <center>
                        <div id='ForState1' runat="server" style='margin-top:7px;margin-bottom:7px;'>
                            <img id='ToggleChartState1' src="../Images/table.png" style='cursor: pointer; width: 40px;height: 40px;' TypeChartOrTbl ='Chart'  />
                        </div>
                    </center>
                </td>
            </tr>
            <tr>
                <td>
              <%--      <img id='ImgWeRoom' runat="server" src="../Images/Age-Child-Female-Light-icon.png" style='cursor: pointer;
                        height:50px;display:none' title='ดูคะแนนของห้องเรา'  />
                    <img id='ImgOtherRoom' runat="server" src="../Images/user-group-icon.png" style='cursor: pointer; float: right;
                        padding-right: 35px;display:none;' title='เปรียบเทียบกับห้องอื่น'  />--%>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                    <asp:ImageButton ID="btnWeRoom" Visible="false" runat="server" ImageUrl="~/Images/Age-Child-Female-Light-icon.png" />
                    <asp:ImageButton ID="btnOtherRoom"  Visible="false" runat="server"  ImageUrl="~/Images/user-group-icon.png" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td style='border-bottom: 2px solid white;padding-top:10px;padding-bottom:10px;' class="style1">
                    <table style='margin:0px;'>
                        <tr>
                            <td align='left'>
                                <%--  <input type="radio" name='SourceScore' id='FromNumber' onclick='FNumber()' /><label
                                    for='FromNumber'>เรียงตามเลขที่</label>--%>
                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                <asp:RadioButton ID="FromNumber" GroupName='TestGroup' Text='เรียงตามเลขที่' runat="server"
                                    AutoPostBack="True" />
                                              </ContentTemplate>
                     </asp:UpdatePanel>
                    
                            </td>
                        </tr>
                     <%--   <tr>
                            <td align='left'>
                                 <input type="radio" name='SourceScore' id='FromLessthanMore' onclick='FLessthanMore()' /><label
                                    for='FromLessthanMore'>เรียงคะแนนเฉลี่ยน้อยไปมาก</label>
                     
                     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                <asp:RadioButton ID="FromLessthanMore" GroupName='TestGroup' Text='เรียงคะแนนเฉลี่ยน้อยไปมาก'
                                    runat="server" AutoPostBack="True" />
                        </ContentTemplate>
                     </asp:UpdatePanel>
                     
                            </td>
                        </tr>--%>
                        <tr>
                            <td align='left'>
                                <%--  <input type="radio" name='SourceScore' id='FromMorethanLess' onclick='FMorethanLess()' /><label
                                    for='FromMorethanLess'>เรียงคะแนนเฉลี่ยมากไปน้อย</label>--%>
                                
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                <asp:RadioButton ID="FromMorethanLess" GroupName='TestGroup' Text='เรียงคะแนนมากไปน้อย'
                                    runat="server" AutoPostBack="True" />
                     </ContentTemplate>
                     </asp:UpdatePanel>                  
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style='border-bottom: 2px solid white;text-align: center;padding-left:10px;padding-top:5px;padding-bottom:5px;' class="style1">
                    <div>
                        <span style="font-size:15px;">เกณฑ์ผ่านที่</span>
                        <asp:TextBox ID="txtFilter" style="margin-left: 38px;margin-right:5px;font-size:30px;" Width="100" runat="server" MaxLength="6" onkeypress="return validateFloatKeyPress(this, event);"></asp:TextBox>

                  <%--      <input id="txttestt" type="text" onkeypress="return validateFloatKeyPress(this, event);" />--%>
                        <asp:ImageButton ID="btnFilter" ImageUrl="~/Images/Search.png" style='width:35px;vertical-align:sub;' runat="server" />
                    </div>
                    <div style="margin-top:5px;text-align:center;">
                        <asp:Label ID="lblFilterDetail" style="font-size:20px;font-weight:bold;" runat="server" Visible="false" Text=""></asp:Label>
                        <%--<span style="font-size:20px;font-weight:bold;">ผ่าน 5/10</span>--%>
                    </div>
                    <%--<asp:Button ID="btnFilter11" runat="server" Text="คะแนน" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ImageButton ID="BtnExportExcel" Width="40px" Height="40px" style="margin-top:10px;" ImageUrl="~/Images/logo-excel.png" runat="server" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div id='DivReport' runat="server" style='width: 400px; left: 39%;position:absolute; top: 16%;'>
            <highchart:ColumnChart ID='AllChart' runat='server'     Width='580'></highchart:ColumnChart>
        </div>
        
          <div id='DivNoTabletQuiz' runat="server">
           ควิซนี้ไม่มีผลคะแนนเลย เนื่องจากไม่ได้ใช้แท็บเลตทำควิซค่ะ
        </div>
        
        <div id='DivReportTable' runat="server" style='width:600px; left: 38%; top:14%; position: absolute;display:none;'>
            <telerik:RadGrid ID="GvPointFirstActivity" runat="server" EnableEmbeddedSkins="false"  AllowPaging="True" Style='width:600px;display:none;margin-left:auto;margin-right:auto;border-radius:5px;'
                GridLines="None" PageSize="500" AllowMultiRowSelection="True" AllowMultiRowEdit="True"
                AutoGenerateColumns="false"  Width="600" CssClass="grid" Font-Size="Large">
                <MasterTableView HeaderStyle-ForeColor="White"  HeaderStyle-BackColor="#3DA8CF" >
                    <Columns>
                        <telerik:GridBoundColumn DataField="เลขที่" HeaderStyle-HorizontalAlign="Center" HeaderText="เลขที่"
                             ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" Width='165' />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="คะแนน" HeaderStyle-HorizontalAlign="Center" HeaderText="คะแนน"
                            HeaderStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Center" Width='165' />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ลำดับ" HeaderStyle-HorizontalAlign="Center"
                            HeaderText="ลำดับที่" >
                            <HeaderStyle HorizontalAlign="Center" Width='165' />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
           
            </telerik:RadGrid>
            <telerik:RadGrid ID="GvTerms" runat="server" Skin='GirdQuickTest' EnableEmbeddedSkins="false" AutoGenerateColumns="true" Style='width:500px;display: none;margin-left:auto;margin-right:auto;'
                AllowPaging="True" GridLines="None"  PageSize="50" AllowMultiRowSelection="true" 
                >
                <MasterTableView HeaderStyle-ForeColor="Black">
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
            <telerik:RadGrid ID="GvPointRoom" runat="server" Skin='GirdQuickTest' EnableEmbeddedSkins="false" Style='width:500px;display: none;margin-left:auto;margin-right:auto;' AllowPaging="True"
                GridLines="None" PageSize="500"  AllowMultiRowSelection="True" AllowMultiRowEdit="True"
                AutoGenerateColumns="true" Width="500">
                <MasterTableView  HeaderStyle-ForeColor="Black">
                </MasterTableView>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                </HeaderContextMenu>
            </telerik:RadGrid>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        
      <asp:UpdatePanel ID='UpdatePanelHidden' runat="server">
      <ContentTemplate>
        <input type="hidden" id='CurrentState' runat="server" value='' />
        <input type="hidden" id='CheckType' runat="server" value='1'  />
   </ContentTemplate>
   </asp:UpdatePanel>
   
     </div>
    </form>

</body>
</html>
