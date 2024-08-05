<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/QuizCreator/QuizCreatorSiteMaster.Master" CodeBehind="DashboardQuizCreator.aspx.vb" Inherits="QuickTest.DashboardQuizCreator" %>
<%@ Register src="UserControl/FilterControl.ascx" tagname="FilterControl" tagprefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../js/jquery-1.7.1.js"></script>
    <script src="../js/highcharts.js"></script>

    <style type="text/css">
       
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
        #DivPieChart {
            margin-top:35px;
        }
        .ForDivPieChart {
        width:320px;
        display:inline-block;
        }
        .ForDivLineChart {
        width:870px;
        margin-left:auto;
        margin-right:auto;
        margin-top:20px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <div id="MainDivDashboard" class="ForMainDiv" >
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
        <div id="DivPieChart" >
                        
            <div id="TopUsageChart" class="ForDivPieChart">

            </div>
            <div id="TopDownloadChart" class="ForDivPieChart">

            </div>
            <div id="TopRatingChart" class="ForDivPieChart">

            </div>
        </div>
        
        <div id="DivLineChart" class="ForDivLineChart">

        </div>

    </div>
</asp:Content>
