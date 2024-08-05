<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/QuizCreator/QuizCreatorSiteMaster.Master" CodeBehind="DashboardTestset.aspx.vb" Inherits="QuickTest.DashboardTestset" %>
<%@ Register src="UserControl/FilterControl.ascx" tagname="FilterControl" tagprefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/highcharts.js"></script>
    <script type="text/javascript">

        $(function () {
            $('#imgBack').click(function () {
                window.location = 'MyTestSet.aspx'
            });
        });

  
    </script>

    <style type="text/css">
        #DivTopDetail {
        width:900px;
        margin-left:auto;
        margin-right:auto;
        text-align:center;
        }
        .myButton {
	        -moz-box-shadow:inset 0px 1px 3px 0px #91b8b3;
	        -webkit-box-shadow:inset 0px 1px 3px 0px #91b8b3;
	        box-shadow:inset 0px 1px 3px 0px #91b8b3;
	        background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #768d87), color-stop(1, #6c7c7c));
	        background:-moz-linear-gradient(top, #768d87 5%, #6c7c7c 100%);
	        background:-webkit-linear-gradient(top, #768d87 5%, #6c7c7c 100%);
	        background:-o-linear-gradient(top, #768d87 5%, #6c7c7c 100%);
	        background:-ms-linear-gradient(top, #768d87 5%, #6c7c7c 100%);
	        background:linear-gradient(to bottom, #768d87 5%, #6c7c7c 100%);
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#768d87', endColorstr='#6c7c7c',GradientType=0);
	        background-color:#768d87;
	        -moz-border-radius:5px;
	        -webkit-border-radius:5px;
	        border-radius:5px;
	        border:1px solid #566963;
	        display:inline-block;
	        cursor:pointer;
	        color:#ffffff;
	        font-family:arial;
	        font-size:15px;
	        font-weight:bold;
	        padding:11px 23px;
	        text-decoration:none;
	        text-shadow:0px -1px 0px #2b665e;
        }
        .myButton:hover {
	        background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #6c7c7c), color-stop(1, #768d87));
	        background:-moz-linear-gradient(top, #6c7c7c 5%, #768d87 100%);
	        background:-webkit-linear-gradient(top, #6c7c7c 5%, #768d87 100%);
	        background:-o-linear-gradient(top, #6c7c7c 5%, #768d87 100%);
	        background:-ms-linear-gradient(top, #6c7c7c 5%, #768d87 100%);
	        background:linear-gradient(to bottom, #6c7c7c 5%, #768d87 100%);
	        filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#6c7c7c', endColorstr='#768d87',GradientType=0);
	        background-color:#6c7c7c;
        }
        .myButton:active {
	        position:relative;
	        top:1px;
        }
        #DivChartTop {
        width:984px;margin-left:auto;margin-right:auto;margin-top:35px;
        }
        #DivChartBottom {
        width:984px;margin-left:auto;margin-right:auto;margin-top:35px;
        }
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <telerik:radscriptmanager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:radscriptmanager>
    <div id="MainDiv" class="ForMainDiv">
        <img id="imgBack" src="Images/Arrow back.png" style="width:70px;position:absolute;cursor:pointer;" />
        <div id="DivTopDetail">
            <div>
                <span style="font-size:40px;"><%=StrTestsetName %></span>
            </div>
            <div style="margin-top:5px;">
                <span style="font-size:28px;"><%=StrTestsetDetail %></span>
            </div>
        </div>
        <div id="DivFilter" style="margin-top:20px;">
        <table style="text-align:center;width:760px;margin-left:auto;margin-right:auto;">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                                <uc1:FilterControl ID="FilterControl1" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Button ID="btnFilter" class="myButton" runat="server" Text="ตกลง" />
                </td>
            </tr>
            <tr>
                <td style="padding-top:15px;">
                    <asp:Label ID="lblWarning" runat="server" Visible="false" Text="เลือกรูปแบบวันที่ไม่ถูกต้อง" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
            </div>
        <div id="DivChartTop">
            <div id="DivChartTotalDownloadAndUsage" style="width:390px;display:inline-block;">

            </div>
            <div id="DivLineUsage" style="width:590px;display:inline-block;">

            </div>
        </div>
        <div id="DivBarChartBottom">

        </div>

    </div>
</asp:Content>
