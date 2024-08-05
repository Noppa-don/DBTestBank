<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ChartStudentInfo.aspx.vb" Inherits="QuickTest.ChartStudentInfo" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //ปุ่ม วิเคราะห์ สีส้มๆ
            $('.ForbtnMenu').each(function () {
                new FastButton(this, TriggerServerButton);
            });

            //ปุ่มวิเคราะห์ สีฟ้าๆ
            if ($('.Forbtn').length != 0) {
                $('.Forbtn').each(function () {
                    new FastButton(this, TriggerServerButton);
                });
            }
            
            if (isAndroid) {
                $('.ForDivBtn').css('width', '740px');
                $('#MainDivInfoStudent').css('width', '740px');
                $('#MainDivInfoStudent').css('height', '440px');
                $('#MainDivInfoDifficult').css('width', '740px');
                $('#MainDivInfoDifficult').css('height', '440px');
                $('#ctl00_MainContent_btnScore').css('width', '200px');
                $('#ctl00_MainContent_btnScore').css('font-size', '21px');
                $('#ctl00_MainContent_BtnTime').css('width', '200px');
                $('#ctl00_MainContent_BtnTime').css('font-size', '21px');
                $('#ctl00_MainContent_BtnFrequency').css('width', '280px');
                $('#ctl00_MainContent_BtnFrequency').css('font-size', '21px');
            }
        });

        //function TriggerClick(e) {
        //    var obj = e.target;
        //    $(obj).trigger('click');
        //}

    </script>

    <style type="text/css">
        body {
            background: rgb(252, 236, 209) !important;
        }

        .MainDiv {
            width: 800px;
            height: 450px;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            background-color: white;
            margin-top: 10px;
            border-radius: 5px;
            background-color: rgb(252, 236, 209);
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
            width: 128px;
            margin-top: 8px;
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

        .ForDivBtn, .ForDivBtnBottom {
            width: 800px;
            margin: auto;
            border: solid 1px #DA7C0C;
            border-radius: .5em;
            text-align: center;
            display: table;
            margin-top: 20px;
            background: #faa51a; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2ZhYTUxYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmNDdhMjAiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #faa51a 0%, #f47a20 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#faa51a), color-stop(100%,#f47a20)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #faa51a 0%,#f47a20 100%); /* IE10+ */
            background: linear-gradient(to bottom, #faa51a 0%,#f47a20 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#faa51a', endColorstr='#f47a20',GradientType=0 ); /* IE6-8 */
        }

        .ForDivBtnBottom {
            border: initial;
            background: initial;
        }

            .ForDivBtnBottom #divBtn {
                border: 1px solid #FFA032;
                border-radius: .5em;
                margin-top: initial !important;
                text-align: center !important;
            }

        .ForbtnMenu {
            font: 100% 'THSarabunNew';
            font-size: 20px;
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

        .ForDivBottom {
            width: 800px;
            height: 300px;
            margin-left: auto;
            margin-right: auto;
            margin-top: 30px;
        }
    </style>
    <!--[if gte IE 9]>
        <style type="text/css">
        .gradient{filter:none;}
        </style>
        <![endif]-->
    <script type="text/javascript">
        // Css for IE
        $(function () {
            if ($.browser.msie || !!navigator.userAgent.match(/Trident\/7\./) || navigator.userAgent.indexOf('Firefox') > -1) {
                $('.ForbtnMenu').css('width', '49%');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <form id="FormChartStudent" runat="server">

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>

        <telerik:RadTabStrip ID="RadTabStrip1" SelectedIndex="0"
            MultiPageID="RadMultiPage1" runat="server" Skin="Telerik" Visible="False">
            <Tabs>
                <telerik:RadTab Text="วิเคราะห์ข้อมูลนักเรียนของคำถามชุดนี้" Width="270px"
                    Selected="True">
                </telerik:RadTab>
                <telerik:RadTab Text="วิเคราะห์ความยาก - ง่ายของคำถามชุดนี้" Width="270px"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>

        <div id='DivBtnChooseMode' class='ForDivBtn'>
            <asp:Button ID="bt1" ClientIDMode="Static" runat="server" Text="วิเคราะห์ข้อมูลนักเรียนของคำถามชุดนี้" CssClass='ForbtnMenu' />
            <asp:Button ID="bt2" ClientIDMode="Static" runat="server" Text="วิเคราะห์ความยาก - ง่ายของคำถามชุดนี้" CssClass='ForbtnMenu' Style="border-right: 0;" />

        </div>

        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0">

            <telerik:RadPageView ID="RadPageView1" runat="server">
                <div id="MainDivInfoStudent" class="MainDiv">
                    <asp:Button ID="btnScore" CssClass="Forbtn" runat="server" Text="วิเคราะห์คะแนน" />
                    <asp:Button ID="BtnTime" CssClass="Forbtn" runat="server" Text="วิเคราะห์เวลา" />
                    <asp:Button ID="BtnFrequency" CssClass="Forbtn" runat="server" Style="width: 220px;" Text="วิเคราะห์ความมั่นใจในการตอบ" />
                    <telerik:RadChart ID="RadChart1" runat="server" Width="700px"
                        Style="margin-left: auto; margin-right: auto; margin-top: 8px" Skin="Outlook">
                        <Appearance>
                            <Border Color="White" />
                        </Appearance>
                        <Series>
                            <telerik:ChartSeries Name="Series 1">
                                <Appearance>
                                    <FillStyle FillType="Solid" MainColor="128, 128, 255">
                                    </FillStyle>
                                    <TextAppearance TextProperties-Color="Black">
                                    </TextAppearance>
                                    <Border Color="Black" />
                                </Appearance>
                            </telerik:ChartSeries>
                            <telerik:ChartSeries Name="Series 2">
                                <Appearance>
                                    <FillStyle FillType="Solid" MainColor="128, 32, 96">
                                    </FillStyle>
                                    <TextAppearance TextProperties-Color="Black">
                                    </TextAppearance>
                                    <Border Color="Black" />
                                </Appearance>
                            </telerik:ChartSeries>
                        </Series>
                        <Legend>
                            <Appearance Dimensions-Margins="17.6%, 3%, 1px, 1px"
                                Dimensions-Paddings="2px, 8px, 6px, 3px" Position-AlignedPosition="TopRight">
                                <ItemTextAppearance TextProperties-Color="89, 89, 89">
                                </ItemTextAppearance>
                                <ItemMarkerAppearance Figure="Square">
                                </ItemMarkerAppearance>
                                <Border Color="Black" />
                            </Appearance>
                        </Legend>
                        <PlotArea>

                            <XAxis>
                                <Appearance Color="Gray" MajorTick-Color="Gray">
                                    <%--<MajorGridLines Color="Black" Width="0" />
                                        <TextAppearance TextProperties-Color="51, 51, 51">
                                        </TextAppearance>--%>
                                    <MajorGridLines Visible="false" />
                                    <MinorGridLines Visible="false" />
                                </Appearance>
                                <AxisLabel>
                                    <TextBlock>
                                        <Appearance TextProperties-Color="51, 51, 51">
                                        </Appearance>
                                    </TextBlock>
                                </AxisLabel>

                            </XAxis>
                            <YAxis>
                                <Appearance Color="Gray" MajorTick-Color="Black"
                                    MinorTick-Color="Black">
                                    <%-- <MajorGridLines Color="Black" />
                                        <MinorGridLines Color="Black" />
                                        <TextAppearance TextProperties-Color="51, 51, 51">
                                        </TextAppearance>--%>
                                    <MajorGridLines Visible="false" />
                                    <MinorGridLines Visible="false" />
                                </Appearance>
                            </YAxis>
                            <Appearance Border-Width="0">
                                <FillStyle FillType="Solid" MainColor="White">
                                </FillStyle>
                                <Border Color="Black" />
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
                    <asp:Label ID="Label1" Font-Size="XX-Large" ForeColor="Black" Style="position: relative; top: 25px;" runat="server" Text=""></asp:Label>
                </div>
            </telerik:RadPageView>

            <telerik:RadPageView ID="RadMultiPage2" runat="server">
                <div id="MainDivInfoDifficult" class="MainDiv">
                    <telerik:RadChart ID="RadChart2" runat="server" Width="700px"
                        Style="margin-left: auto; margin-right: auto; padding-top: 40px;"
                        Skin="Outlook">
                        <Appearance>
                            <Border Color="White" />
                        </Appearance>
                        <Series>
                            <telerik:ChartSeries Name="Series 1">
                                <Appearance>
                                    <FillStyle FillType="Solid" MainColor="128, 128, 255">
                                    </FillStyle>
                                    <TextAppearance TextProperties-Color="Black">
                                    </TextAppearance>
                                    <Border Color="Black" />
                                </Appearance>
                            </telerik:ChartSeries>
                            <telerik:ChartSeries Name="Series 2">
                                <Appearance>
                                    <FillStyle FillType="Solid" MainColor="128, 32, 96">
                                    </FillStyle>
                                    <TextAppearance TextProperties-Color="Black">
                                    </TextAppearance>
                                    <Border Color="Black" />
                                </Appearance>
                            </telerik:ChartSeries>
                        </Series>
                        <Legend>
                            <Appearance Dimensions-Margins="17.6%, 3%, 1px, 1px"
                                Dimensions-Paddings="2px, 8px, 6px, 3px" Position-AlignedPosition="TopRight">
                                <ItemTextAppearance TextProperties-Color="89, 89, 89">
                                </ItemTextAppearance>
                                <ItemMarkerAppearance Figure="Square">
                                </ItemMarkerAppearance>
                                <Border Color="Black" />
                            </Appearance>
                        </Legend>
                        <PlotArea>
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
                                    <%--   <MajorGridLines Color="Black" />
                                        <MinorGridLines Color="Black" />
                                        <TextAppearance TextProperties-Color="51, 51, 51">
                                        </TextAppearance>--%>
                                    <MajorGridLines Visible="false" />
                                    <MinorGridLines Visible="false" />
                                </Appearance>
                            </YAxis>
                            <Appearance Border-Width="0">
                                <FillStyle FillType="Solid" MainColor="White">
                                </FillStyle>
                                <Border Color="Black" />
                            </Appearance>
                        </PlotArea>
                        <ChartTitle>
                            <Appearance>
                                <FillStyle MainColor="">
                                </FillStyle>
                            </Appearance>
                            <TextBlock Text="ข้อที่">
                                <Appearance TextProperties-Color="Black" TextProperties-Font="Arial, 18pt">
                                </Appearance>
                            </TextBlock>
                        </ChartTitle>
                    </telerik:RadChart>
                    <asp:Label ID="Label2" Font-Size="XX-Large" Style='position: relative; top: 35px;' ForeColor="Black" runat="server" Text=""></asp:Label>
                </div>
            </telerik:RadPageView>

        </telerik:RadMultiPage>


    </form>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">

    <script type="text/javascript">

        $(function () {
            $('#Help').hide();
            $('#navigation').hide();
        });

        function btn2_onclick() {

        }

        function btn21_onclick() {

        }

    </script>
</asp:Content>
