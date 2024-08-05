<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UsageScore.aspx.vb" Inherits="QuickTest.UsageScore" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<%@ Register Src="../UserControl/TestsetHeaderControl.ascx" TagName="TestsetHeaderControl" TagPrefix="uc1" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //ดักถ้าเป็น Tablet ของครู
            if (isAndroid) {
                var ww = ($(window).width() < window.screen.width) ? $(window).width() : window.screen.width; //get proper width
                var mw = 480; // min width of site
                var ratio = ww / mw; //calculate ratio
                if (ww < mw) { //smaller than minimum size
                    $('#Viewport').attr('content', 'initial-scale=' + ratio + ', maximum-scale=' + ratio + ', minimum-scale=' + ratio + ', user-scalable=yes, width=' + ww);
                } else { //regular size
                    $('#Viewport').attr('content', 'initial-scale=1.0, maximum-scale=2, minimum-scale=1.0, user-scalable=yes, width=' + ww);
                }

                $('#MainDiv').css('width', '740px');
                $('#MainDiv').css('height', '440px');
                $('#GridDetail').css('height', '290px');
                $('table tr td').css('font-size', '30px');
            }
        });


    </script>


    <style type="text/css">
        /*.rgRow td, .rgAltRow td, .rgHeader td, .rgFilterRow td 
        { 
            border-left: solid 1px black !important; 
        } */
        .RadGrid .rgClipCells .rgHeader, .RadGrid .rgClipCells .rgFilterRow > td, .RadGrid .rgClipCells .rgRow > td, .RadGrid .rgClipCells .rgAltRow > td, .RadGrid .rgClipCells .rgEditRow > td, .RadGrid .rgClipCells .rgFooter > td {
            border-right: 1px solid #ccc;
        }

        .ForMainDiv {
            width: 750px;
            height: 300px;
            margin-left: auto;
            margin-right: auto;
        }

        .ForMainDivNoData {
            width: auto;
            height: 244px;
            overflow-y: auto;
            border: 1px dashed #FFA032;
            border-radius: 5px;
            color: #444;
        }

        .ForSpanNoData {
            font-size: 40px;
            position: relative;
            top: 85px;
        }

        .ForDivShowInFo {
            text-align: center;
        }

        table {
            margin: 0;
            font: normal 0.95em 'THSarabunNew'!important;
            color: #444;
            width: 100%;
        }
        /* .rgRow td:first-child,.rgAltRow td:first-child, th.rgHeader:first-child, .rgFilterRow td:first-child
    {
        border-right: 1px solid #ccc !important;width:48%;border-radius:5px 0 0 0;      
    }
    .rgRow td:last-child,.rgAltRow td:last-child,th.rgHeader:last-child, .rgFilterRow td:last-child
    {
        border-left: 1px solid #ccc !important;width:20%; text-align:left;border-radius: 0 5px 0 0;  
    }*/
        tr.rgRow {
            background-color: #FFFFCC!important;
            height: 45px;
            font-size: 16px;
        }

        tr.rgAltRow {
            background-color: #FFFFFF!important;
            height: 45px;
            font-size: 16px;
        }

            tr.rgRow td, tr.rgAltRow td {
                border-color: #ccc !important;
            }

                tr.rgRow td a, tr.rgAltRow td a {
                    color: #09D4FF!important;
                }

        thead tr {
            height: 55px;
        }

            thead tr th {
                background: #dce9f9!important;
                border-bottom: 1px solid #ccc !important;
                font-size: 20px!important;
            }

        .ForImg {
            float: right;
            margin-right: 20px;
            cursor: pointer;
            width: 30px;
            height: 30px;
        }

        .ForDivLogType {
            width: 50px;
            height: 30px;
            float: left;
            margin-left: 10px;
            margin-right: 10px;
            border-radius: 5px;
        }

        .ForMaindivLogType {
            line-height: 30px;
        }

        html {
            background-color: #D3F2F7!important;
        }
    </style>


    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <%--  <div id="BGDiv" style=" background:black url(../images/back.jpg) no-repeat center fixed;width: 100%;height: 491px;margin-top: -34px;">--%>

        <uc1:TestsetHeaderControl ID="TestsetHeaderControl1" runat="server" />
        <div id="MainDiv" style="width: 95.5%; height: 380px; margin-left: auto; margin-right: auto;margin-top:10px; background-color: #D3F2F7; position: relative; border: 2px solid #FFA032; background-color: wheat; border-radius: 5px; padding: 10px;">
            <div style="width: 100%; height: 380px;">
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" runat="server">
                    <telerik:RadGrid ID="GridDetail" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Medium" AllowPaging="true" runat="server" Height="322px" Font-Size="12">
                        <%--  <PagerStyle Mode="NextPrev" />--%>
                        <MasterTableView Font-Size="Medium"></MasterTableView>
                        <PagerStyle PageSizeLabelText="ข้อมูลทั้งหมด" PagerTextFormat="{4} ข้อมูลทั้งหมด {5} รายการ มีทั้งหมด {1} หน้า" />
                        <ClientSettings>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
            </div>
        </div>
        <%--</div>--%>
    </form>

</body>
</html>
