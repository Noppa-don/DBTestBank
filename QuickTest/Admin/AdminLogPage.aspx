<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminLogPage.aspx.vb"
    Inherits="QuickTest.AdminLogPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="../UserControl/LocalControl.ascx" tagname="LocalControl" tagprefix="Lc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 321px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table class="style1">
        <tr>
        <td style="text-align: right" colspan="2">
            <asp:LinkButton ID="lbtnHome" runat="server">กลับเมนูหลัก</asp:LinkButton>
        </td>
        </tr>
            <tr>
                <td>
                    <Lc:LocalControl ID="Lc1" runat="server" />
                </td>
                <td style="text-align: left" class="style2">
                    LogType
                    <telerik:RadComboBox ID="cbLogType" runat="server">
                    </telerik:RadComboBox>
                </td>
                
            </tr>
            <tr>
                <td style="text-align: right" colspan="2">
                    <telerik:RadButton ID="BtnFind" runat="server" Style="text-align: right" Text="ค้นหา"
                        Width="100px">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <telerik:RadGrid ID="GvAdminLog" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            GridLines="None" PageSize="20" AllowMultiRowSelection="True" AllowSorting="True"
                            ShowFooter="True" AllowFilteringByColumn="True" >
                            <MasterTableView DataKeyNames="a" AllowMultiColumnSorting="true" NoMasterRecordsText="ไม่พบข้อมูล"
                                >
                                <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="a" HeaderStyle-HorizontalAlign="Center" HeaderText="ลำดับ"
                                        ItemStyle-Width="30" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Description" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="LogType" ItemStyle-Width="100">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="100px"></ItemStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SchoolName" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="โรงเรียน" ItemStyle-Width="100">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="100px"></ItemStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="username" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="ชื่อผู้ใช้" ItemStyle-Width="100">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="100px"></ItemStyle>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LastUpdate" HeaderStyle-HorizontalAlign="Center"
                                        HeaderText="วันที่" ItemStyle-Width="100">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="100px"></ItemStyle>
                                    </telerik:GridBoundColumn>
                                </Columns>
                               
                            </MasterTableView>
                            <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" ForeColor="Black" Wrap="True" />
                            <PagerStyle NextPageText="Next" PrevPageText="Previous" />
                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" EnableImageSprites="True">
                            </HeaderContextMenu>
                        </telerik:RadGrid>
                        
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
