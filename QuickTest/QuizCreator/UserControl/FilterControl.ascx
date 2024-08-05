<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FilterControl.ascx.vb" Inherits="QuickTest.FilterControl" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    span {
        font-size:20px;
    }
</style>
<div id="DivControlFilter" >
    <span>ดูข้อมูลจาก</span>
    <asp:DropDownList ID="FilterMode" Font-Size="20px" style="padding:5px;" AutoPostBack="true" runat="server">
        <asp:ListItem Value="1">วันย้อนหลัง</asp:ListItem>
        <asp:ListItem Value="2">ช่วงวันที่</asp:ListItem>
    </asp:DropDownList>
    <asp:Panel ID="PanelFilter1" runat="server" style="display:inline-block;">
        <asp:DropDownList ID="SelectedDate" style="padding:5px;" Font-Size="20px" runat="server">
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>60</asp:ListItem>
            <asp:ListItem>120</asp:ListItem>
            <asp:ListItem>365</asp:ListItem>
        </asp:DropDownList>
        <span> วัน</span>
    </asp:Panel>
    <asp:Panel ID="PanelFilter2" runat="server" style="display:inline-block;">
        <span>จากวันที่ </span>
        <telerik:RadDatePicker ID="RadDatePicker1" Font-Size="25px" runat="server"></telerik:RadDatePicker>
        <span>ถึง </span>
        <telerik:RadDatePicker ID="RadDatePicker2" Font-Size="25px" runat="server"></telerik:RadDatePicker>
    </asp:Panel>
    </div>