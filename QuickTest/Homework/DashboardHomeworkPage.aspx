<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master"
    CodeBehind="DashboardHomeworkPage.aspx.vb" Inherits="QuickTest.DashboardHomeworkPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControl/RepeaterTestsetControl.ascx" TagName="UserControl"
    TagPrefix="myTestset" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--  <link href="css/fixMenuSlide.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {
            $('#navigation a').stop().animate({ 'marginLeft': '-52px' }, 1000);
            $('#navigation > li').hover(function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-2px' }, 200);
            }, function () {
                $('a', $(this)).stop().animate({ 'marginLeft': '-52px' }, 200);
            });
        });

    </script>
    <script type="text/javascript">
        /* css for IE */
        $(function () {
            if ($.browser.msie) {
                $('#GraphDiv td').css('background-color', 'white!important');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <myTestset:UserControl ID="MyCtlTestset" runat="server" />
                <div id="GraphDiv">
                    <table>
                        <tr style="background: initial">
                            <td style="background: initial; text-align: center;">
                                <telerik:RadChart ID="RadChartPie" runat="server" Width="400" Height="250" Legend-Appearance-Visible="false"
                                    IntelligentLabelsEnabled="true" DefaultType="Pie">
                                    <Appearance>
                                        <Border Color="White" />
                                    </Appearance>
                                    <PlotArea>
                                        <Appearance Dimensions-Margins="10,70,80,70" Border-Width="0">
                                            <FillStyle FillType="Solid" MainColor="White">
                                            </FillStyle>
                                            <Border Color="Black" />
                                        </Appearance>
                                    </PlotArea>
                                    <ChartTitle TextBlock-Text="สัดส่วน">
                                        <Appearance Position-AlignedPosition="Bottom">
                                        </Appearance>
                                    </ChartTitle>
                                </telerik:RadChart>
                            </td>
                            <td style="background: initial">
                                <telerik:RadChart ID="RadChartDashboard" runat="server" Width="400" Height="250"
                                    Legend-Appearance-Visible="false" DefaultType="Bar">
                                    <Appearance>
                                        <Border Color="White" />
                                    </Appearance>
                                    <PlotArea>
                                        <Appearance Dimensions-Margins="10,70,80,70" Border-Width="0">
                                            <FillStyle FillType="Solid" MainColor="White">
                                            </FillStyle>
                                            <Border Color="Black" />
                                        </Appearance>
                                        <XAxis>
                                            <AxisLabel Visible="true">
                                                <TextBlock Text="สัดส่วน" Appearance-Dimensions-Margins="0,0,110,300" Appearance-TextProperties-Color="Black"></TextBlock>
                                            </AxisLabel>
                                            <Appearance>
                                                <MajorGridLines Visible="false" />
                                                <MinorGridLines Visible="false" />
                                            </Appearance>
                                        </XAxis>
                                        <YAxis  MaxValue="100" AxisMode="Extended">
                                            <AxisLabel Visible="true">
                                                <TextBlock Text="%" Appearance-Dimensions-Margins="500,500,490,500" Appearance-TextProperties-Color="Black"></TextBlock>
                                            </AxisLabel>
                                        </YAxis>
                                    </PlotArea>
                                    <ChartTitle TextBlock-Text="สถิติการบ้านเทียบเป็นเปอร์เซนต์">
                                        <Appearance Position-AlignedPosition="Bottom">
                                        </Appearance>
                                    </ChartTitle>
                                </telerik:RadChart>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
