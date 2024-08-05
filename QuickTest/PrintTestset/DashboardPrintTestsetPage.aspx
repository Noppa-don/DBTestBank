<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master" CodeBehind="DashboardPrintTestsetPage.aspx.vb" Inherits="QuickTest.DashboardPrintTestsetPage" %>

<%@ Register Src="~/UserControl/RepeaterTestsetControl.ascx" TagName="UserControl"
    TagPrefix="myTestset" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--  <link href="css/fixMenuSlide.css" rel="stylesheet" type="text/css" />--%>
    <%-- <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {

            $('#navigation a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#navigation > li').hover(
             function () {
                 $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
             },
                 function () {
                     $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
                 }
                 );
                        
            if (getQueryStringByName('cancle') == 't') {
                $(".TestsetDiv").slideToggle();
            }
        });

        function getQueryStringByName(name) {
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"), results = regex.exec(document.URL);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <myTestset:UserControl ID="MyCtlTestset" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
