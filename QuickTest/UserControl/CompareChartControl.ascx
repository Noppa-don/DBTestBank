<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CompareChartControl.ascx.vb"
    Inherits="QuickTest.CompareChartControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<style type="text/css">
    .ForMainDiv
    {
        width: 750px;
        height: 300px;
        margin-left: auto;
        margin-right: auto;
    }
    .ForMainDivNoData
    {
        width: 100%;
        height: 244px;
        overflow-y: auto;
        border: 1px dashed #FFA032;
        text-align: center;
        border-radius: 5px;
        color: #444;
    }
    .ForSpanNoData
    {
        font-size: 35px;
        position: relative;
        top: 45px;
    }
    /*.Forbtn
    {
        font: 100% 'THSarabunNew';
        border: 0;
        padding: 2px 0 3px 0;
        cursor: pointer;
        background: #1EC9F4;
        -moz-border-radius: .5em;
        -webkit-border-radius: .5em;
        border-radius: .5em; /*behavior:url(border-radius.htc);
        -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);/*
        -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
        box-shadow: 0 1px 2px rgba(0,0,0,.2);
        color: #FFF;
        border: solid 1px #0D8AA9;
        background: #46C4DD;
        background: -webkit-gradient(linear, left top, left bottom, from(#63CFDF), to(#17B2D9));
        background: -moz-linear-gradient(top,  #63CFDF,  #17B2D9);
        text-shadow: 1px 1px #178497;
        behavior: url('../css/PIE.htc');
        -pie-track-active: false; width: 128px;
    }*/
    .Forbtn
    {
        font: 100% 'THSarabunNew';
        width: 113px;
        height: 38px;
        line-height: 38px;
        margin: 10px 10px;
        display: inline-block;
        text-align: center;
        border-radius: .5em;
        cursor: pointer;
        color: #111;
        border: 1px solid rgb(255, 166, 53);
        background-color: #FFA032;
        background: -webkit-gradient(linear, left top, left bottom, from(rgb(250, 211, 147)), to(#FFA032));
    }
    .WidthSmallBtn
    {
        width: 100px;
    }
    .WidthMainBtn
    {
        width: 100px;
    }
    .ForbtnToggle
    {
        font: 100% 'THSarabunNew';
        width: 120px;
        height: 38px;
        line-height: 38px;
        display: inline-block;
        text-align: center;
        cursor: pointer;
        color: #fff;
        text-transform: uppercase;
        text-decoration: none;
        border: 1px solid rgb(235, 235, 235);
        background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
    }
</style>
<div id='MainDiv' runat="server">
    <div id='divBtn' style='width: 250px; float: left; text-align: center; display: table;
        padding-top: 5px;'>
        <asp:Button ID="btnHomework" CssClass='WidthMainBtn ForbtnToggle' runat="server"
            Text="การบ้าน" Style="border-radius: 5px 0 0 0;" />
        <asp:Button ID="btnQuiz" CssClass='WidthMainBtn ForbtnToggle' runat="server" Text="ควิซ"
            Style="border-radius: 0 5px  0 0;" />
        <div id='DivBtnSubject' runat="server" style='margin-top: 15px;'>
            <%--            <asp:Button ID="Button1" CssClass='WidthSmallBtn Forbtn' runat="server" Text="ไทย" />
            <asp:Button ID="Button2" CssClass='WidthSmallBtn Forbtn' runat="server" Text="การงานฯ" />
            <asp:Button ID="Button3" CssClass='WidthSmallBtn Forbtn' runat="server" Text="คณิต" />
            <asp:Button ID="Button4" CssClass='WidthSmallBtn Forbtn' runat="server" Text="วิทย์" />
            <asp:Button ID="Button5" CssClass='WidthSmallBtn Forbtn' runat="server" Text="ศิลปะ" />
            <asp:Button ID="Button6" CssClass='WidthSmallBtn Forbtn' runat="server" Text="สุขศึกษา" />
            <asp:Button ID="Button7" CssClass='WidthSmallBtn Forbtn' runat="server" Text="สังคม" />
            <asp:Button ID="Button8" CssClass='WidthSmallBtn Forbtn' runat="server" Text="อังกฤษ" />
            <asp:Button ID="Button9" CssClass='WidthSmallBtn Forbtn' runat="server" Text="หลายวิชา" />--%>
        </div>
    </div>
    <div id='DivChart' runat="server" style='width: 535px; float: left; margin-left: 10px;
        text-align: center;'>
        <telerik:RadChart ID='RadChart1' runat="server" Width='535px' Height='350px' Skin="DeepBlue">
            <Appearance >
                <Border Color="white"  />
                <FillStyle MainColor="White" SecondColor="White" FillType="Gradient" ></FillStyle>
               
            </Appearance>
            <Legend>
                <Appearance Dimensions-Margins="1%, 0px, 0px, 0px" Dimensions-Paddings="2px, 8px, 6px, 3px"
                    Position-AlignedPosition="TopRight">
                    <ItemTextAppearance TextProperties-Color="89, 89, 89">
                    </ItemTextAppearance>
                    <ItemMarkerAppearance Figure="Square">
                    </ItemMarkerAppearance>
                    <Border Color="black" />
                    <FillStyle MainColor="AliceBlue" SecondColor="AliceBlue" FillType="Gradient" ></FillStyle>
                </Appearance>
            </Legend>
            <PlotArea>
                <EmptySeriesMessage Visible="True">
                    <Appearance Visible="True">
                    </Appearance>
                </EmptySeriesMessage>
                <XAxis>
                    <Appearance Color="Gray" MajorTick-Color="Gray">
                        <MajorGridLines Color="Black" Width="0" />
                        <TextAppearance TextProperties-Color="51, 51, 51">
                        </TextAppearance>
                    </Appearance>
                    <AxisLabel>
                        <TextBlock>
                            <Appearance TextProperties-Color="51, 51, 51">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                </XAxis>
                <YAxis>
                    <Appearance Color="Gray" MajorTick-Color="Black" MinorTick-Color="Black">
                        <MajorGridLines Color="Black" />
                        <MinorGridLines Color="Black" />
                        <TextAppearance TextProperties-Color="51, 51, 51">
                        </TextAppearance>
                    </Appearance>
                </YAxis>
                <Appearance Dimensions-Margins="1%,12%,8%,5%" >
                    <FillStyle FillType="Solid" MainColor="Silver">
                    </FillStyle>
                    <Border Color="Gray" />
                </Appearance>
            </PlotArea>
            <ChartTitle>
                <Appearance>
                    <FillStyle MainColor="">
                    </FillStyle>
                </Appearance>
                <TextBlock>
                    <Appearance TextProperties-Color="Black" TextProperties-Font="Arial, 18pt">
                    </Appearance>
                </TextBlock>
            </ChartTitle>
        </telerik:RadChart>
        <asp:Label ID="lblChart" runat="server" Style='font-size: 30px; font-weight: bold;'
            Text="ปีการศึกษา 2556 เทอม 1"></asp:Label>
    </div>
</div>
