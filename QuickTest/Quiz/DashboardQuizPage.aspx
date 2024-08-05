<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/SiteDashboard.Master"
    CodeBehind="DashboardQuizPage.aspx.vb" Inherits="QuickTest.DashboardQuizPage" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <myTestset:UserControl ID="MyCtlTestset" runat="server" />
                <div id="GraphDiv">
                    <center>
                        <telerik:RadChart ID="RadChartDashboard" runat="server" Width="890" Legend-Appearance-Visible="false" 
                            DefaultType="Bar">
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
                                    <AxisLabel Visible="true" >
                                      <TextBlock Text="ห้อง"  Appearance-Dimensions-Margins="0,0,110,780" Appearance-TextProperties-Color="Black" ></TextBlock>
                                    </AxisLabel>
                                    <%--<Appearance>
                                        <MajorGridLines Visible="false" />
                                        <MinorGridLines Visible="false" />
                                        <TextAppearance TextProperties-Color="black"></TextAppearance>                                                                               
                                    </Appearance>      --%>              
                                </XAxis>
                                <YAxis>
                                    <AxisLabel Visible="true"  >
                                       <TextBlock Text="จำนวนควิซ"  Appearance-Dimensions-Margins="500,500,500,500" Appearance-TextProperties-Color="Black"   ></TextBlock>
                                    </AxisLabel>
                                    <Appearance>
                                        <TextAppearance TextProperties-Color="black"></TextAppearance>
                                    </Appearance>
                                </YAxis>
                            </PlotArea>
                            <ChartTitle TextBlock-Text="จำนวนครั้งการควิซรายห้อง">
                                <Appearance Position-AlignedPosition="Bottom">
                                </Appearance>
                            </ChartTitle>
                        </telerik:RadChart>
                    </center>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
